﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

using WinFormStringCnvClass;
using OpenCvSharp;
using OpenCvSharp.Extensions;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;


namespace onnxNote
{
    public partial class Form1 : Form
    {
        VideoCapture capture;

        string thisExeDirPath;
        public Form1()
        {
            InitializeComponent();
            thisExeDirPath = Path.GetDirectoryName(Application.ExecutablePath);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TEXT|*.txt";
            if (false && ofd.ShowDialog() == DialogResult.OK)
            {
                WinFormStringCnv.setControlFromString(this, File.ReadAllText(ofd.FileName));
            }
            else
            {
                string paramFilename = Path.Combine(thisExeDirPath, "_param.txt");
                if (File.Exists(paramFilename))
                {
                    WinFormStringCnv.setControlFromString(this, File.ReadAllText(paramFilename));
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            trackBar_frameIndex.Value = 0;
            string FormContents = WinFormStringCnv.ToString(this);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TEXT|*.txt";

            if (false && sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, FormContents);
            }
            else
            {
                string paramFilename = Path.Combine(thisExeDirPath, "_param.txt");
                File.WriteAllText(paramFilename, FormContents);
            }
        }

        private void button_OpenModelFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Onnx|*.onnx";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            textBox_modelFilePath.Text = ofd.FileName;
        }

        private void panel_Main_Resize(object sender, EventArgs e)
        {
            setCenterPictureBox();
        }

        private void setCenterPictureBox()
        {
            pictureBox.Left = (panel_Main.ClientSize.Width - pictureBox.Width) / 2;
            pictureBox.Top = (panel_Main.ClientSize.Height - pictureBox.Height) / 2;
        }

        private void pictureBoxUpdate(PictureBox p, Bitmap bitmap)
        {
            if (p.Image != null) p.Image.Dispose();
            p.Image = bitmap;
        }

        private void button_OpenMovieFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP4|*.mp4";

            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (capture != null) capture.Dispose();

            capture = new VideoCapture(ofd.FileName);
            trackBar_frameIndex.Maximum = capture.FrameCount;
            trackBar_frameIndex.Value = 0;
        }

        private void trackBar_frameIndex_Scroll(object sender, EventArgs e)
        {
            if (capture != null)
            {
                using (Mat frame = new Mat())
                {
                    capture.PosFrames = trackBar_frameIndex.Value;
                    if (!capture.Read(frame) || frame.Empty()) return;

                    Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);
                    pictureBoxUpdate(pictureBox, bitmap);
                    label_FrameCount.Text = trackBar_frameIndex.Value.ToString() + " / " + capture.FrameCount.ToString();
                }
            }
        }

        private void button_frameShift_Click(object sender, EventArgs e)
        {
            int shiftLength = 0;

            if (sender == button_frameBackward10) { shiftLength = -10; }
            else if (sender == button_frameBackward05) { shiftLength = -5; }
            else if (sender == button_frameBackward01) { shiftLength = -1; }
            else if (sender == button_frameForward01) { shiftLength = 1; }
            else if (sender == button_frameForward05) { shiftLength = 5; }
            else if (sender == button_frameForward10) { shiftLength = 10; }

            int nextValue = trackBar_frameIndex.Value + shiftLength;

            nextValue = nextValue < 0 ? 0 : nextValue;
            nextValue = nextValue >= trackBar_frameIndex.Maximum ? trackBar_frameIndex.Maximum : nextValue;

            trackBar_frameIndex.Value = nextValue;

        }

        private void trackBar_frameIndex_ValueChanged(object sender, EventArgs e)
        {
            if (capture != null)
            {
                using (Mat frame = new Mat())
                {
                    capture.PosFrames = trackBar_frameIndex.Value;
                    if (!capture.Read(frame) || frame.Empty()) return;

                    Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);

                    Tensor<float> inputTensor = ConvertBitmapToTensor(bitmap);
                    var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(sessionInputName, inputTensor) };
                    var results = session.Run(inputs);
                    var output = results.First().AsEnumerable<float>().ToArray();
                    ParseYOLOOutput(output);

                    pictureBoxUpdate(pictureBox, bitmap);

                    results.Dispose();
                }

                label_FrameCount.Text = trackBar_frameIndex.Value.ToString() + " / " + capture.FrameCount.ToString();
            }
        }

        Tensor<float> ConvertBitmapToTensor(Bitmap bitmap, int width = 640, int height = 640)
        {
            var tensor = new DenseTensor<float>(new[] { 1, 3, height, width });

            int widthMax = bitmap.Width;
            int heightMax = bitmap.Height;

            for (int y = 0; y < heightMax; y++)
            {
                for (int x = 0; x < widthMax; x++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    tensor[0, 0, y, x] = color.R / 255.0f;
                    tensor[0, 1, y, x] = color.G / 255.0f;
                    tensor[0, 2, y, x] = color.B / 255.0f;
                }
            }
            return tensor;
        }

        void ParseYOLOOutput(float[] output)
        {
            Console.WriteLine("Detection results:");
            int s0 = 8400;


            for (int i = 0; i < s0; i++)
            {
                int offset = i * (56);

                // BBOX
                float center_x = output[i + s0 * 0];
                float center_y = output[i + s0 * 1];
                float w = output[i + s0 * 2];
                float h = output[i + s0 * 3];
                float confidence = output[i + s0 * 4];

                if (confidence >= 0.8)
                {
                    Console.WriteLine($"Confidence: {confidence}, Box: ({center_x}, {center_y}, {w}, {h})");
                    Console.WriteLine("Keypoints:");
                    for (int j = 0; j < 17; j++)
                    {
                        float keypointX = output[i + s0 * (j * 3 + 5) ];
                        float keypointY = output[i + s0 * (j * 3 + 6) ];
                        float confidenceKP = output[i + s0 * (j * 3 + 7) ];
                        Console.WriteLine($"Keypoint {j + 1}: ({keypointX}, {keypointY}, {confidenceKP})");
                    }
                }
            }
        }

        InferenceSession session;
        string sessionInputName;

        private void textBox_modelFilePath_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(textBox_modelFilePath.Text))
            {
                if (session != null) session.Dispose();
                session = new InferenceSession(textBox_modelFilePath.Text);
                sessionInputName = session.InputMetadata.Keys.First();

            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG|*.png";

            if (sfd.ShowDialog() != DialogResult.OK) return;

            using (Bitmap b = new Bitmap(pictureBox.Image))
            {
                b.Save(sfd.FileName);
            }
        }
    }
}
