using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Collections.Concurrent;
using System.Diagnostics;

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
                    drawPose(bitmap, yoloPoseModelHandle.PoseInfos);
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
                    List<PoseInfo> poseInfos = yoloPoseModelHandle.Predicte(bitmap);
                    drawPose(bitmap, poseInfos);
                    pictureBoxUpdate(pictureBox, bitmap);

                }

                label_FrameCount.Text = trackBar_frameIndex.Value.ToString() + " / " + capture.FrameCount.ToString();
            }
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
                int deviceID = comboBox_DeviceID.SelectedIndex - 1;
                yoloPoseModelHandle = new YoloPoseModelHandle(textBox_modelFilePath.Text, deviceID);
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

                masterPath = Path.Combine(Path.GetDirectoryName(sfd.FileName), Path.GetFileNameWithoutExtension(sfd.FileName));

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
                timer1.Start();
                backgroundWorker_posePredict.RunWorkerAsync();
            }
            else
            {
                backgroundWorker_posePredict.CancelAsync();
            }
        }

        ConcurrentQueue<frameDataSet> frameBitmapQueue;
        string progressReport = "";

        private void backgroundWorker_posePredict_DoWork(object sender, DoWorkEventArgs e)
        {
            Task task_frameBitmap = Task.Run(() => dequeue_frameBitmap());
            Task task_frameTensor = Task.Run(() => dequeue_frameTensor());
            Task task_framePoseInfo = Task.Run(() => dequeue_framePoseInfo());
            Task task_frameReport = Task.Run(() => dequeue_frameReport());
            Task task_frameVideoMat = Task.Run(() => dequeue_frameVideoMat());
            Task task_frameShow = Task.Run(() => dequeue_frameShow());

            BackgroundWorker worker = (BackgroundWorker)sender;
            frameBitmapQueue = new ConcurrentQueue<frameDataSet>();

            if (!Directory.Exists(masterPath)) { Directory.CreateDirectory(masterPath); };

            List<frameDataSet> frameBitmapList = new List<frameDataSet>();

            using (Mat frame = new Mat())
            {
                while (capture.Read(frame) && !frame.Empty())
                {
                    int frameIndex = capture.PosFrames;

                    frameBitmapList.Add(new frameDataSet(BitmapConverter.ToBitmap(frame), frameIndex));

                    string posFrame = frameIndex.ToString();
                    progressReport = posFrame + " / " + capture.FrameCount.ToString();

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    if (frameBitmapList.Count >= 64)
                    {
                        worker.ReportProgress(0);

                        foreach (var item in frameBitmapList)
                        {
                            frameBitmapQueue.Enqueue(item);
                        }

                        frameBitmapList.Clear();
                    }
                }

                foreach (var item in frameBitmapList)
                {
                    frameBitmapQueue.Enqueue(item);
                }

                frameBitmapList.Clear();
                frameBitmapQueue.Enqueue(new frameDataSet(-1));
            }

            task_frameBitmap.Wait();
            task_frameTensor.Wait();
            task_framePoseInfo.Wait();
            task_frameReport.Wait();
            task_frameVideoMat.Wait();
            task_frameShow.Wait();
        }

        private void backgroundWorker_posePredict_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label_FrameCount.Text = progressReport;
            label_FrameCount.Refresh();
        }

        private void backgroundWorker_posePredict_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer1.Stop();
            timer1_Tick(null, null);
            button_Save.Text = "Save";
        }

        double minIndex_dequeue_frameBitmap = double.MaxValue;
        ConcurrentQueue<frameDataSet> frameTensorQueue;

        private void dequeue_frameBitmap()
        {
            int maxIndex = int.MinValue;

            Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            frameTensorQueue = new ConcurrentQueue<frameDataSet>();

            var frameBitmapQueueTemp = new BlockingCollection<frameDataSet[]>();

            Task[] workers = new Task[8];

            for (int i = 0; i < workers.Length; i++)
            {
                workers[i] = Task.Run(() =>
                {
                    List<frameDataSet> frameTensorListTemp = new List<frameDataSet>();
                    foreach (var frameInfos in frameBitmapQueueTemp.GetConsumingEnumerable())
                    {
                        foreach (var frameInfo in frameInfos)
                        {
                            if (frameInfo.frameIndex >= 0)
                            {
                                maxIndex = maxIndex < frameInfo.frameIndex ? frameInfo.frameIndex : maxIndex;
                                minIndex_dequeue_frameBitmap = minIndex_dequeue_frameBitmap > frameInfo.frameIndex ? frameInfo.frameIndex : minIndex_dequeue_frameBitmap;

                                using (Bitmap srcImage = new Bitmap(frameInfo.bitmap))
                                {
                                    Tensor<float> tensor = ConvertBitmapToTensor(srcImage);
                                    List<NamedOnnxValue> inputs = yoloPoseModelHandle.getInputs(tensor);

                                    frameTensorListTemp.Add(new frameDataSet(inputs, frameInfo.bitmap, frameInfo.frameIndex));
                                }
                            }
                        }

                        for (int listIndex = 0; listIndex < frameTensorListTemp.Count; listIndex++)
                        {
                            frameTensorQueue.Enqueue(frameTensorListTemp[listIndex]);
                        }

                        frameTensorListTemp.Clear();
                    }
                });
            }

            List<frameDataSet> frameTensorList = new List<frameDataSet>();

            while (true)
            {
                if (frameBitmapQueue != null && frameBitmapQueue.TryDequeue(out frameDataSet frameInfo))
                {
                    if (frameInfo.frameIndex >= 0)
                    {
                        frameTensorList.Add(frameInfo);
                    }
                    else
                    {
                        if (frameTensorList.Count > 0)
                        {
                            frameBitmapQueueTemp.Add(frameTensorList.ToArray());
                            frameTensorList.Clear();
                        }

                        frameBitmapQueueTemp.CompleteAdding();
                        break;
                    }

                    if (frameTensorList.Count >= 64)
                    {
                        frameDataSet[] frameDataSets = frameTensorList.ToArray();
                        frameBitmapQueueTemp.Add(frameDataSets);
                        frameTensorList.Clear();
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }

            Task.WaitAll(workers);

            Console.WriteLine($"Complete: {maxIndex} { System.Reflection.MethodBase.GetCurrentMethod().Name}");
            frameTensorQueue.Enqueue(new frameDataSet(-1));

        }


        public unsafe Tensor<float> ConvertBitmapToTensor(Bitmap bitmap, int width = 640, int height = 640)
        {
            var tensor = new DenseTensor<float>(new[] { 1, 3, height, width });
            int widthMax = Math.Min(bitmap.Width, width);
            int heightMax = Math.Min(bitmap.Height, height);

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb
            );

            try
            {
                byte* ptr = (byte*)bitmapData.Scan0;

                for (int y = 0; y < heightMax; y++)
                {
                    byte* row = ptr + y * bitmapData.Stride;

                    for (int x = 0; x < widthMax; x++)
                    {
                        int pixelIndex = x * 3; // 24bpp
                        tensor[0, 0, y, x] = row[pixelIndex + 2] / 255.0f; // R
                        tensor[0, 1, y, x] = row[pixelIndex + 1] / 255.0f; // G
                        tensor[0, 2, y, x] = row[pixelIndex] / 255.0f;     // B
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            return tensor;
        }

        ConcurrentQueue<frameDataSet> framePoseInfoQueue;

        private void dequeue_frameTensor()
        {
            int maxIndex = int.MinValue;
            Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            framePoseInfoQueue = new ConcurrentQueue<frameDataSet>();

            List<frameDataSet> frameTensorList = new List<frameDataSet>();
            Task predictBatchTask = null;
            int loopCount = 0;
            while (true)
            {
                if (frameTensorQueue != null && !frameTensorQueue.IsEmpty && frameTensorQueue.TryDequeue(out frameDataSet frameInfo))
                {
                    if (frameInfo.frameIndex >= 0)
                    {
                        maxIndex = maxIndex < frameInfo.frameIndex ? frameInfo.frameIndex : maxIndex;
                        frameTensorList.Add(frameInfo);

                        if (frameTensorList.Count >= 256)
                        {
                            frameDataSet[] datasetArray = frameTensorList.ToArray();
                            if (predictBatchTask != null) { predictBatchTask.Wait(); }
                            predictBatchTask = Task.Run(() => PredictBatch(datasetArray));

                            frameTensorList.Clear();
                        }

                    }
                    else
                    {
                        frameTensorList.Add(new frameDataSet(-1));
                        frameDataSet[] datasetArray = frameTensorList.ToArray();
                        if (predictBatchTask != null) { predictBatchTask.Wait(); }
                        predictBatchTask = Task.Run(() => PredictBatch(datasetArray));

                        frameTensorList.Clear();

                        break;
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }

                loopCount++;
            }
            Console.WriteLine($"Complete: {maxIndex} { System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }


        public void PredictBatch(frameDataSet[] frameTensorArray)
        {
            foreach (var frame in frameTensorArray)
            {
                if (frame.frameIndex >= 0)
                {
                    frame.results = yoloPoseModelHandle.PredicteResults(frame.inputs);
                }
                else
                {
                    frame.frameIndex = -1;
                }
            }

            Task.Run(() => framePoseInfoQueueEnqueue(frameTensorArray));

            return;
        }

        public void framePoseInfoQueueEnqueue(frameDataSet[] frameTensorArray)
        {
            foreach (var frameInfo in frameTensorArray)
            {
                framePoseInfoQueue.Enqueue(frameInfo);

            }
        }

        ConcurrentQueue<frameDataSet> frameReportQueue;
        ConcurrentQueue<frameDataSet> frameVideoMatQueue;
        ConcurrentQueue<frameDataSet> frameShowQueue;

        private void dequeue_framePoseInfo_addQueue(frameDataSet frameInfo)
        {
            List<PoseInfo> poseInfos = yoloPoseModelHandle.PoseInfoRead(frameInfo.results);
            frameInfo.results.Dispose();

            frameReportQueue.Enqueue(new frameDataSet(poseInfos, frameInfo.frameIndex));

            if (frameInfo.bitmap != null)
            {
                drawPose(frameInfo.bitmap, poseInfos);
                using (Mat mat = BitmapConverter.ToMat(frameInfo.bitmap))
                {
                    Mat mat3C = mat.CvtColor(ColorConversionCodes.BGRA2BGR);
                    frameVideoMatQueue.Enqueue(new frameDataSet(mat3C, frameInfo.frameIndex));
                    frameShowQueue.Enqueue(new frameDataSet(frameInfo.bitmap, frameInfo.frameIndex));
                }
            }
            else
            {
                Console.WriteLine($"ERROR: { System.Reflection.MethodBase.GetCurrentMethod().Name }  Null Bitmap");
            }
        }


        private void dequeue_framePoseInfo()
        {
            Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            frameReportQueue = new ConcurrentQueue<frameDataSet>();

            frameVideoMatQueue = new ConcurrentQueue<frameDataSet>();
            frameShowQueue = new ConcurrentQueue<frameDataSet>();

            int targetFrameIndex = 1;
            List<frameDataSet> ListBuff = new List<frameDataSet>();

            bool flagFin = false;

            while (true)
            {
                if (framePoseInfoQueue != null && !framePoseInfoQueue.IsEmpty && framePoseInfoQueue.TryDequeue(out frameDataSet frameInfo))
                {
                    if (frameInfo.frameIndex >= 0)
                    {
                        if (targetFrameIndex == frameInfo.frameIndex)
                        {
                            dequeue_framePoseInfo_addQueue(frameInfo);
                            targetFrameIndex++;
                        }
                        else
                        {
                            ListBuff.Add(frameInfo);
                        }
                    }
                    else
                    {
                        flagFin = true;
                    }

                    for (int i = 0; i < ListBuff.Count; i++)
                    {
                        if (ListBuff[i].frameIndex == targetFrameIndex)
                        {
                            dequeue_framePoseInfo_addQueue(ListBuff[i]);
                            ListBuff.RemoveAt(i);
                            i = -1;
                            targetFrameIndex++;
                        }
                    }

                    if (flagFin)
                    {
                        frameReportQueue.Enqueue(new frameDataSet(-1));

                        frameVideoMatQueue.Enqueue(new frameDataSet(-1));
                        frameShowQueue.Enqueue(new frameDataSet(-1));

                        break;
                    }

                }
                else
                {
                    Thread.Sleep(1);
                }
            }
            Console.WriteLine($"Complete: {targetFrameIndex} " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }



        private void dequeue_frameReport()
        {
            Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);

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

            while (true)
            {
                if (frameReportQueue != null && !frameReportQueue.IsEmpty && frameReportQueue.TryDequeue(out frameDataSet frameInfo))
                {
                    if (frameInfo.frameIndex >= 0)
                    {
                        string posFrame = frameInfo.frameIndex.ToString();

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

                        foreach (var pose in frameInfo.PoseInfos)
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
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }
            }

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

            Console.WriteLine("Complete:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        private void dequeue_frameShow()
        {
            Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);

            while (true)
            {
                if (frameShowQueue != null && frameShowQueue.TryDequeue(out frameDataSet frameInfo))
                {
                    if (frameInfo.frameIndex >= 0)
                    {

                        if (frameInfo.bitmap != null)
                        {
                            pictureBoxUpdate(pictureBox, frameInfo.bitmap);
                        }
                        else
                        {
                            Console.WriteLine($"ERROR: { System.Reflection.MethodBase.GetCurrentMethod().Name }  Null Bitmap  frame:{frameInfo.frameIndex}");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
            Console.WriteLine("Complete:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        enum AVHWDeviceType
        {
            AV_HWDEVICE_TYPE_NONE,
            AV_HWDEVICE_TYPE_VDPAU,
            AV_HWDEVICE_TYPE_CUDA,
            AV_HWDEVICE_TYPE_VAAPI,
            AV_HWDEVICE_TYPE_DXVA2,
            AV_HWDEVICE_TYPE_QSV,
            AV_HWDEVICE_TYPE_VIDEOTOOLBOX,
            AV_HWDEVICE_TYPE_D3D11VA,
            AV_HWDEVICE_TYPE_DRM,
            AV_HWDEVICE_TYPE_OPENCL,
            AV_HWDEVICE_TYPE_MEDIACODEC,
            AV_HWDEVICE_TYPE_VULKAN,
            AV_HWDEVICE_TYPE_D3D12VA,
            AV_HWDEVICE_TYPE_AMF,
        };

        private void dequeue_frameVideoMat()
        {
            Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);

            string videoPath = Path.Combine(masterPath, Path.GetFileNameWithoutExtension(masterPath) + "_pose.mp4");

            using (VideoWriter video = new VideoWriter(videoPath, FourCC.FromString("mp4v"), 30, new OpenCvSharp.Size(640, 360)))
            {
                if (!video.IsOpened())
                {
                    Console.WriteLine("video not opened");
                }

                video.Set(VideoWriterProperties.HwAcceleration, (double)AVHWDeviceType.AV_HWDEVICE_TYPE_QSV);

                while (true)
                {
                    if (frameVideoMatQueue.TryDequeue(out frameDataSet frameInfo))
                    {
                        if (frameInfo.frameIndex >= 0)
                        {
                            video.Write(frameInfo.mat);
                            frameInfo.mat.Dispose();
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
                video.Release();

            }
            Console.WriteLine("Complete:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                p.Image = new Bitmap(bitmap);
            }
        }

        private void drawPose(Bitmap bitmap, List<PoseInfo> PoseInfos)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                drawBBoxs(g, PoseInfos);
                drawBones(g, PoseInfos);
            }
        }

        public void drawBBoxs(Graphics g, List<PoseInfo> PoseInfos)
        {
            if (g != null)
            {
                foreach (var info in PoseInfos)
                {
                    g.DrawRectangle(Pens.Blue, info.Bbox.Rectangle);
                }
            }
        }

        public void drawBones(Graphics g, List<PoseInfo> PoseInfos)
        {
            if (g != null)
            {
                foreach (var info in PoseInfos)
                {
                    info.KeyPoints.drawBone(g);
                }
            }
        }

        private void trackBar_Conf_ValueChanged(object sender, EventArgs e)
        {
            if (yoloPoseModelHandle != null)
            {
                yoloPoseModelHandle.ConfidenceThreshold = (float)(trackBar_Conf.Value / 100f);
                trackBar_frameIndex_ValueChanged(null, null);

            }
        }

        private void trackBar_Conf_Scroll(object sender, EventArgs e)
        {
            label_ConfThreshold.Text = ((float)(trackBar_Conf.Value / 100f)).ToString("0.00");
        }

        private void comboBox_DeviceID_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_modelFilePath_TextChanged(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (frameBitmapQueue != null && frameTensorQueue != null && framePoseInfoQueue != null && frameReportQueue != null)
            {
                Console.WriteLine($"\t{DateTime.Now:HH:mm:ss}\tB{frameBitmapQueue.Count}\tT{frameTensorQueue.Count}\tP{framePoseInfoQueue.Count}\tR{frameReportQueue.Count}");
            }
        }
    }

    public class frameDataSet
    {
        public List<PoseInfo> PoseInfos;
        public Tensor<float> tensor;
        public Bitmap bitmap;
        public int frameIndex;
        public Mat mat;
        public float[] output;
        public IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results;
        public List<NamedOnnxValue> inputs;


        public frameDataSet(int frameIndex)
        {
            this.frameIndex = frameIndex;
        }
        public frameDataSet(Mat mat, int frameIndex)
        {
            this.mat = mat;
            this.frameIndex = frameIndex;
        }
        public frameDataSet(float[] output, Bitmap bitmap, int frameIndex)
        {
            this.output = output;
            this.bitmap = bitmap;
            this.frameIndex = frameIndex;
        }

        public frameDataSet(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results, Bitmap bitmap, int frameIndex)
        {
            this.results = results;
            this.bitmap = bitmap;
            this.frameIndex = frameIndex;
        }

        public frameDataSet(List<PoseInfo> PoseInfos, Bitmap bitmap, int frameIndex)
        {
            this.PoseInfos = PoseInfos;
            this.bitmap = bitmap;
            this.frameIndex = frameIndex;
        }
        public frameDataSet(List<PoseInfo> PoseInfos, int frameIndex)
        {
            this.PoseInfos = PoseInfos;
            this.frameIndex = frameIndex;
        }
        public frameDataSet(Bitmap bitmap, int frameIndex)
        {
            this.bitmap = bitmap;
            this.frameIndex = frameIndex;
        }
        public frameDataSet(Tensor<float> tensor, int frameIndex)
        {
            this.tensor = tensor;
            this.frameIndex = frameIndex;
        }
        public frameDataSet(Tensor<float> tensor, Bitmap bitmap, int frameIndex)
        {
            this.tensor = tensor;
            this.bitmap = bitmap;
            this.frameIndex = frameIndex;
        }
        public frameDataSet(List<NamedOnnxValue> inputs, Bitmap bitmap, int frameIndex)
        {
            this.inputs = inputs;
            this.bitmap = bitmap;
            this.frameIndex = frameIndex;
        }

        public override string ToString()
        {
            return frameIndex.ToString();
        }
    }
}
