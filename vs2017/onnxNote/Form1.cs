using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Collections.Concurrent;

using WinFormStringCnvClass;
using OpenCvSharp;
using OpenCvSharp.Extensions;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

using YoloPoseOnnxHandle;


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
            if (p.InvokeRequired)
            {
                p.Invoke(new Action(() => pictureBoxUpdate(p, bitmap)));
                return;
            }
            else
            {
                if (bitmap == null) return;
                if (p.Image != null) p.Image.Dispose();
                p.Image = new Bitmap( bitmap);
            }
        }


        private void drawPose(Bitmap bitmap)
        {
            yoloPoseModelHandle.Predicte(bitmap);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                yoloPoseModelHandle.drawBBoxs(g);
                yoloPoseModelHandle.drawBones(g);
            }

        }

        private void button_OpenMovieFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG|*.png|MP4|*.mp4";
            ofd.FilterIndex = 2;

            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (capture != null) capture.Dispose();

            string ext = Path.GetExtension(ofd.FileName);
            if (ext == ".mp4")
            {
                capture = new VideoCapture(ofd.FileName);
                trackBar_frameIndex.Maximum = capture.FrameCount;
                trackBar_frameIndex.Value = 0;
            }
            else
            {
                pictureBoxUpdate(pictureBox, new Bitmap(ofd.FileName));
            }

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
                    drawPose(bitmap);
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
                    drawPose(bitmap);
                    pictureBoxUpdate(pictureBox, bitmap);


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
                        float keypointX = output[i + s0 * (j * 3 + 5)];
                        float keypointY = output[i + s0 * (j * 3 + 6)];
                        float confidenceKP = output[i + s0 * (j * 3 + 7)];
                        Console.WriteLine($"Keypoint {j + 1}: ({keypointX}, {keypointY}, {confidenceKP})");
                    }
                }
            }
        }

        private void textBox_modelFilePath_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(textBox_modelFilePath.Text))
            {
                yoloPoseModelHandle = new YoloPoseModelHandle(textBox_modelFilePath.Text);
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

        YoloPoseModelHandle yoloPoseModelHandle;
        string masterPath = "";


        private void button_Save_Click(object sender, EventArgs e)
        {

            if (button_Save.Text == "Save")
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "MP4|*.mp4";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                sfd.Filter = "";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                masterPath = Path.Combine(Path.GetDirectoryName(sfd.FileName), Path.GetFileNameWithoutExtension(ofd.FileName));

                string ext = Path.GetExtension(ofd.FileName);

                if (ext == ".mp4")
                {
                    if (capture != null) capture.Dispose();
                    capture = new VideoCapture(ofd.FileName);
                }
                else
                {
                    return;
                }

                if (capture == null) return;

                button_Save.Text = "Cancel";
                backgroundWorker_posePredict.RunWorkerAsync();
            }
            else
            {
                backgroundWorker_posePredict.CancelAsync();
            }
        }

        ConcurrentQueue<Action> taskQueue;

        private void backgroundWorker_posePredict_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            List<string> HeadKeyPoint = new List<string>();
            List<string> NoseKeyPoint = new List<string>();
            List<string> EyeKeyPoint = new List<string>();
            List<string> EarKeyPoint = new List<string>();
            List<string> ShoulderKeyPoint = new List<string>();
            List<string> ElbowKeyPoint = new List<string>();
            List<string> WristKeyPoint = new List<string>();
            List<string> HipKeyPoint = new List<string>();
            List<string> KneeKeyPoint = new List<string>();
            List<string> AnkleKeyPoint = new List<string>();

            string pathHead = Path.Combine(masterPath, "Head.csv");
            string pathNose = Path.Combine(masterPath, "Nose.csv");
            string pathEye = Path.Combine(masterPath, "Eye.csv");
            string pathEar = Path.Combine(masterPath, "Ear.csv");
            string pathShoulder = Path.Combine(masterPath, "Shoulder.csv");
            string pathElbow = Path.Combine(masterPath, "Elbow.csv");
            string pathWrist = Path.Combine(masterPath, "Wrist.csv");
            string pathHip = Path.Combine(masterPath, "Hip.csv");
            string pathKnee = Path.Combine(masterPath, "Knee.csv");
            string pathAnkle = Path.Combine(masterPath, "Ankle.csv");

            string lineHead = "";
            string lineNose = "";
            string lineEye = "";
            string lineEar = "";
            string lineShoulder = "";
            string lineElbow = "";
            string lineWrist = "";
            string lineHip = "";
            string lineKnee = "";
            string lineAnkle = "";

            string videoPath = Path.Combine(masterPath, Path.GetFileNameWithoutExtension(masterPath) + "_pose.mov");

            if (!Directory.Exists(masterPath)) { Directory.CreateDirectory(masterPath); };


            using (VideoWriter video = new VideoWriter(videoPath, FourCC.MP4V, 30, new OpenCvSharp.Size(640, 360)))
            using (Mat frame = new Mat())
            {
                if (video.IsOpened())
                {
                    Console.WriteLine("video not opened");
                }

                while (capture.Read(frame) && !frame.Empty())
                {
                    Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);
                    drawPose(bitmap);
                    using (Mat writeFrame = BitmapConverter.ToMat(bitmap))
                    {
                        video.Write(writeFrame);
                    }
                    pictureBoxUpdate(pictureBox, bitmap);

                    string posFrame = capture.PosFrames.ToString();
                    progressReport = posFrame + " / " + capture.FrameCount.ToString();

                    lineHead = posFrame;
                    lineNose = posFrame;
                    lineEye = posFrame;
                    lineEar = posFrame;
                    lineShoulder = posFrame;
                    lineElbow = posFrame;
                    lineWrist = posFrame;
                    lineHip = posFrame;
                    lineKnee = posFrame;
                    lineAnkle = posFrame;

                    foreach (var pose in yoloPoseModelHandle.PoseInfos)
                    {
                        lineHead += "," + pose.KeyPoints.Head().ToString();
                        lineNose += "," + pose.KeyPoints.Nose.ToString();
                        lineEye += "," + pose.KeyPoints.Eye().ToString();
                        lineEar += "," + pose.KeyPoints.Ear().ToString();
                        lineShoulder += "," + pose.KeyPoints.Shoulder().ToString();
                        lineElbow += "," + pose.KeyPoints.Elbow().ToString();
                        lineWrist += "," + pose.KeyPoints.Wrist().ToString();
                        lineHip += "," + pose.KeyPoints.Hip().ToString();
                        lineKnee += "," + pose.KeyPoints.Knee().ToString();
                        lineAnkle += "," + pose.KeyPoints.Ankle().ToString();
                    }

                    HeadKeyPoint.Add(lineHead);
                    NoseKeyPoint.Add(lineNose);
                    EyeKeyPoint.Add(lineEye);
                    EarKeyPoint.Add(lineEar);
                    ShoulderKeyPoint.Add(lineShoulder);
                    ElbowKeyPoint.Add(lineElbow);
                    WristKeyPoint.Add(lineWrist);
                    HipKeyPoint.Add(lineHip);
                    KneeKeyPoint.Add(lineKnee);
                    AnkleKeyPoint.Add(lineAnkle);
                    
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    worker.ReportProgress(0);
                }

                video.Release();

                File.AppendAllLines(pathHead, HeadKeyPoint);
                File.AppendAllLines(pathNose, NoseKeyPoint);
                File.AppendAllLines(pathEye, EyeKeyPoint);
                File.AppendAllLines(pathEar, EarKeyPoint);
                File.AppendAllLines(pathShoulder, ShoulderKeyPoint);
                File.AppendAllLines(pathElbow, ElbowKeyPoint);
                File.AppendAllLines(pathWrist, WristKeyPoint);
                File.AppendAllLines(pathHip, HipKeyPoint);
                File.AppendAllLines(pathKnee, KneeKeyPoint);
                File.AppendAllLines(pathAnkle, AnkleKeyPoint);

            }
        }

        string progressReport = "";
        private void backgroundWorker_posePredict_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label_FrameCount.Text = progressReport;
            label_FrameCount.Refresh();
        }

        private void backgroundWorker_posePredict_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button_Save.Text = "Save";
        }
    }
}
