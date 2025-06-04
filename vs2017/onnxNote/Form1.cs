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
                    if (capture.IsDisposed || !capture.Read(frame) || frame.Empty()) return;
                    capture.PosFrames = trackBar_frameIndex.Value > 0 ? trackBar_frameIndex.Value - 1 : 0;

                    Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);
                    List<PoseInfo> poseInfos = yoloPoseModelHandle.Predict(bitmap);
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

        public int PredictTaskBatchSize = 1024;
        string masterDirectoryPath = "";
        YoloPoseModelHandle yoloPoseModelHandle;

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (button_Save.Text == "Save")
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "MP4|*.mp4";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                sfd.Filter = "";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                masterDirectoryPath = Path.Combine(Path.GetDirectoryName(sfd.FileName), Path.GetFileNameWithoutExtension(sfd.FileName));


                string ext = Path.GetExtension(ofd.FileName);

                if (ext == ".mp4")
                {
                    if (capture != null) capture.Dispose();
                    capture = new VideoCapture(ofd.FileName);
                    trackBar_frameIndex.Maximum = capture.FrameCount;
                    trackBar_frameIndex.Value = 1;
                }
                else
                {
                    return;
                }

                if (capture == null) return;

                if (!int.TryParse(textBox_PredictBatchSize.Text, out PredictTaskBatchSize))
                {
                    PredictTaskBatchSize = 1024;
                }

                button_Save.Text = "Cancel";
                timer1.Start();
                backgroundWorker_posePredict.RunWorkerAsync();

            }
            else
            {
                backgroundWorker_posePredict.CancelAsync();
            }
        }

        string progressReport = "";

        DateTime taskStartTime;

        private void backgroundWorker_posePredict_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            if (!Directory.Exists(masterDirectoryPath)) { Directory.CreateDirectory(masterDirectoryPath); };

            taskStartTime = DateTime.Now;

            Task task_frameVideoReader = Task.Run(() => dequeue_frameVideoReader(worker, e));
            Task task_frameBitmap = Task.Run(() => dequeue_frameBitmap());
            Task task_frameTensor = Task.Run(() => dequeue_frameTensor());
            Task task_framePoseInfo = Task.Run(() => dequeue_framePoseInfo());
            Task task_frameReport = Task.Run(() => dequeue_frameReport());
            Task task_frameVideoMat = Task.Run(() => dequeue_frameVideoMat());
            Task task_frameShow = Task.Run(() => dequeue_frameShow());


            task_frameVideoReader.Wait();
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


        BlockingCollection<frameDataSet> frameBitmapQueue;

        private void dequeue_frameVideoReader(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                int maxIndex = int.MinValue;
                Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);

                frameBitmapQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize);
                List<frameDataSet> frameBitmapList = new List<frameDataSet>();

                string posFrame = "";

                using (Mat frame = new Mat())
                {
                    while (capture.Read(frame) && !frame.Empty())
                    {
                        int frameIndex = capture.PosFrames;
                        posFrame = frameIndex.ToString();

                        maxIndex = Math.Max(maxIndex, frameIndex);
                        frameBitmapList.Add(new frameDataSet(BitmapConverter.ToBitmap(frame), frameIndex));

                        if (frameBitmapList.Count >= PredictTaskBatchSize / 2)
                        {
                            posFrame = frameIndex.ToString();
                            progressReport = $"{posFrame} / {capture.FrameCount}";
                            worker.ReportProgress(0);

                            foreach (var item in frameBitmapList)
                            {
                                frameBitmapQueue.Add(item);
                            }

                            frameBitmapList.Clear();
                        }
                        if (worker.CancellationPending) { e.Cancel = true; break; }
                    }

                    foreach (var item in frameBitmapList)
                    {
                        frameBitmapQueue.Add(item);
                    }

                    frameBitmapList.Clear();
                    frameBitmapQueue.CompleteAdding();
                }

                progressReport = $"{posFrame} / {capture.FrameCount}";
                worker.ReportProgress(0);
                capture.Dispose();
                Console.WriteLine($"Complete: {maxIndex} { System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");

            }

        }

        double minIndex_dequeue_frameBitmap = double.MaxValue;
        BlockingCollection<frameDataSet> frameTensorQueue = new BlockingCollection<frameDataSet>();


        /// <summary>
        /// frameBitmapQueue to frameTensorQueue
        /// </summary>
        private void dequeue_frameBitmap()
        {
            try
            {
                int maxIndex = int.MinValue;
                bool isFirst = true;

                frameTensorQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize);
                var frameBitmapQueueTemp = new BlockingCollection<frameDataSet[]>(256);

                Task[] workers = new Task[8];

                for (int i = 0; i < workers.Length; i++)
                {
                    workers[i] = Task.Run(() =>
                    {
                        try
                        {
                            int workerIndex = i;
                            List<frameDataSet> frameTensorListTemp = new List<frameDataSet>();

                            foreach (var frameInfos in frameBitmapQueueTemp.GetConsumingEnumerable())
                            {
                                int queueCount = frameInfos.Length;

                                foreach (var frameInfo in frameInfos)
                                {
                                    Interlocked.Exchange(ref maxIndex, Math.Max(maxIndex, frameInfo.frameIndex));
                                    Interlocked.Exchange(ref minIndex_dequeue_frameBitmap, Math.Min(minIndex_dequeue_frameBitmap, frameInfo.frameIndex));

                                    using (Bitmap srcImage = new Bitmap(frameInfo.bitmap))
                                    {
                                        Tensor<float> tensor = ConvertBitmapToTensor(srcImage);
                                        List<NamedOnnxValue> inputs = yoloPoseModelHandle.getInputs(tensor);
                                        frameTensorListTemp.Add(new frameDataSet(inputs, (Bitmap)srcImage.Clone(), frameInfo.frameIndex));
                                    }
                                    frameInfo.bitmap.Dispose();

                                }

                                Console.WriteLine($"Run:{workerIndex} - {queueCount} : {maxIndex} {System.Reflection.MethodBase.GetCurrentMethod().Name}");

                                if (frameTensorListTemp.Count > 0)
                                {
                                    foreach (var item in frameTensorListTemp)
                                    {
                                        frameTensorQueue.Add(item);
                                    }
                                    frameTensorListTemp.Clear();
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name}");

                        }
                    });
                }

                List<frameDataSet> frameTensorList = new List<frameDataSet>();
                while (frameBitmapQueue == null) { Thread.Sleep(1); }
                while (!frameBitmapQueue.IsCompleted)
                {
                    if (frameBitmapQueue.TryTake(out frameDataSet frameInfo, 500))
                    {
                        if (isFirst) { Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name); isFirst = false; }

                        frameTensorList.Add(frameInfo);

                        if (frameTensorList.Count >= 64)
                        {
                            frameBitmapQueueTemp.Add(frameTensorList.ToArray());
                            frameTensorList.Clear();
                        }
                    }
                }

                if (frameTensorList.Count > 0)
                {
                    frameBitmapQueueTemp.Add(frameTensorList.ToArray());
                    frameTensorList.Clear();
                }

                frameBitmapQueueTemp.CompleteAdding();

                Task.WaitAll(workers);

                frameTensorQueue.CompleteAdding();

                frameBitmapQueueTemp.Dispose();

                Console.WriteLine($"Complete: {maxIndex} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");

            }

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

        BlockingCollection<frameDataSet> framePoseInfoQueue;

        /// <summary>
        /// frameTensorQueue to framePoseInfoQueue
        /// </summary>
        private void dequeue_frameTensor()
        {
            try
            {
                Console.WriteLine($"Start: { System.Reflection.MethodBase.GetCurrentMethod().Name} ");
                int maxIndex = int.MinValue;
                framePoseInfoQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize * 2);

                List<frameDataSet> frameTensorList = new List<frameDataSet>();
                Task predictBatchTask = null;
                frameDataSet[] datasetArray;

                while (!frameTensorQueue.IsCompleted)
                {
                    if (frameTensorQueue.TryTake(out frameDataSet frameInfo, 500))
                    {
                        frameTensorList.Add(frameInfo);

                        if (frameTensorList.Count >= PredictTaskBatchSize / 2)
                        {
                            if (predictBatchTask != null) predictBatchTask.Wait();

                            datasetArray = frameTensorList.ToArray();
                            predictBatchTask = Task.Run(() => PredictBatch(datasetArray));
                            frameTensorList.Clear();
                        }
                    }
                }

                if (frameTensorList.Count > 0)
                {
                    if (predictBatchTask != null) predictBatchTask.Wait();

                    datasetArray = frameTensorList.ToArray();
                    predictBatchTask = Task.Run(() => PredictBatch(datasetArray));
                    frameTensorList.Clear();
                }

                if (predictBatchTask != null)
                {
                    predictBatchTask.Wait();
                }

                framePoseInfoQueue.CompleteAdding();

                Console.WriteLine($"Complete: {maxIndex} { System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");

            }


        }


        public void PredictBatch(frameDataSet[] frameTensorArray)
        {
            foreach (var frame in frameTensorArray)
            {
                frame.results = yoloPoseModelHandle.PredicteResults(frame.inputs);
            }

            Task.Run(() => framePoseInfoQueueEnqueue(frameTensorArray));

            return;
        }

        public void framePoseInfoQueueEnqueue(frameDataSet[] frameTensorArray)
        {
            Console.WriteLine($".....Start: {frameTensorArray.Length} " + System.Reflection.MethodBase.GetCurrentMethod().Name);

            foreach (var frameInfo in frameTensorArray)
            {
                framePoseInfoQueue.Add(frameInfo);
            }

            Console.WriteLine($".....Complete: { System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        BlockingCollection<frameDataSet> frameReportQueue;
        BlockingCollection<frameDataSet> frameVideoMatQueue;
        BlockingCollection<frameDataSet> frameShowQueue;

        private void dequeue_framePoseInfo_addQueue(frameDataSet frameInfo)
        {
            List<PoseInfo> poseInfos = yoloPoseModelHandle.PoseInfoRead(frameInfo.results);
            frameInfo.results.Dispose();

            frameReportQueue.Add(new frameDataSet(poseInfos, frameInfo.frameIndex));

            if (frameInfo.bitmap != null)
            {
                drawPose(frameInfo.bitmap, poseInfos);
                using (Mat mat = BitmapConverter.ToMat(frameInfo.bitmap))
                {
                    Mat mat3C = mat.CvtColor(ColorConversionCodes.BGRA2BGR);
                    frameVideoMatQueue.Add(new frameDataSet(mat3C, frameInfo.frameIndex));
                    frameShowQueue.Add(new frameDataSet(frameInfo.bitmap, frameInfo.frameIndex));
                }
            }
            else
            {
                Console.WriteLine($"ERROR: { System.Reflection.MethodBase.GetCurrentMethod().Name }  Null Bitmap");
            }
        }

        /// <summary>
        /// framePoseInfoQueue to OutputQueues
        /// </summary>
        private void dequeue_framePoseInfo()
        {
            try
            {
                while (framePoseInfoQueue == null) { Thread.Sleep(1); }


                Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                frameReportQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize);
                frameVideoMatQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize);
                frameShowQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize);

                int targetFrameIndex = 1;
                List<frameDataSet> ListBuff = new List<frameDataSet>();

                while (framePoseInfoQueue.Count > 0 || !framePoseInfoQueue.IsCompleted)
                {
                    if (framePoseInfoQueue != null && framePoseInfoQueue.TryTake(out frameDataSet frameInfo, 500))
                    {
                        if (targetFrameIndex == frameInfo.frameIndex)
                        {
                            Console.Write($":[ {targetFrameIndex}/{frameInfo.frameIndex} ]");
                            dequeue_framePoseInfo_addQueue(frameInfo);
                            targetFrameIndex++;
                        }
                        else
                        {
                            Console.Write($": {targetFrameIndex}/{frameInfo.frameIndex}");
                            ListBuff.Add(frameInfo);
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
                    }
                }

                frameReportQueue.CompleteAdding();
                frameVideoMatQueue.CompleteAdding();
                frameShowQueue.CompleteAdding();

                Console.WriteLine($"Complete: {targetFrameIndex} " + System.Reflection.MethodBase.GetCurrentMethod().Name);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");

            }
        }

        private void dequeue_frameReport()
        {
            //Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
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

                List<string> PoseValue = new List<string>();


                string pathHead = Path.Combine(masterDirectoryPath, "Head.csv");
                string pathNose = Path.Combine(masterDirectoryPath, "Nose.csv");
                string pathEye = Path.Combine(masterDirectoryPath, "Eye.csv");
                string pathEar = Path.Combine(masterDirectoryPath, "Ear.csv");
                string pathShoulder = Path.Combine(masterDirectoryPath, "Shoulder.csv");
                string pathElbow = Path.Combine(masterDirectoryPath, "Elbow.csv");
                string pathWrist = Path.Combine(masterDirectoryPath, "Wrist.csv");
                string pathHip = Path.Combine(masterDirectoryPath, "Hip.csv");
                string pathKnee = Path.Combine(masterDirectoryPath, "Knee.csv");
                string pathAnkle = Path.Combine(masterDirectoryPath, "Ankle.csv");

                string pathPose = Path.Combine(masterDirectoryPath, "Pose.csv");

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

                string linePose = "";

                bool isFirst = true;

                PoseValue.Add("frame," + PoseInfo.ToLineStringHeader());


                if (dataGridView_PoseLines.InvokeRequired)
                {
                    dataGridView_PoseLines.Invoke(new Action(() => dataGridView_PoseLines.Rows.Clear()));
                }

                while (!frameReportQueue.IsCompleted)
                {
                    if (frameReportQueue != null && frameReportQueue.TryTake(out frameDataSet frameInfo))
                    {
                        if (isFirst)
                        {
                            Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                            isFirst = false;
                        }

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
                                lineHead += $",{pose.KeyPoints.Head()}";
                                lineNose += $",{pose.KeyPoints.Nose}";
                                lineEye += $",{pose.KeyPoints.Eye()}";
                                lineEar += $",{pose.KeyPoints.Ear()}";
                                lineShoulder += $",{pose.KeyPoints.Shoulder()}";
                                lineElbow += $",{pose.KeyPoints.Elbow()}";
                                lineWrist += $",{pose.KeyPoints.Wrist()}";
                                lineHip += $",{pose.KeyPoints.Hip()}";
                                lineKnee += $",{pose.KeyPoints.Knee()}";
                                lineAnkle += $",{pose.KeyPoints.Ankle()}";

                                linePose = posFrame + "," + pose.ToLineString();

                                PoseValue.Add(linePose);

                                if (dataGridView_PoseLines.InvokeRequired)
                                {
                                    dataGridView_PoseLines.Invoke(new Action(() => dataGridView_PoseLines.Rows.Add(new object[] { false, posFrame.ToString(), linePose })));
                                }

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

                File.AppendAllLines(pathPose, PoseValue);

                Console.WriteLine("Complete:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");

            }

        }

        private void dequeue_frameShow()
        {
            try
            {
                bool isFirst = true;
                while (frameShowQueue == null) { Thread.Sleep(1); }
                while (!frameShowQueue.IsCompleted)
                {
                    if (frameShowQueue != null && frameShowQueue.TryTake(out frameDataSet frameInfo))
                    {
                        if (isFirst)
                        {
                            Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                            isFirst = false;
                        }

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
                        Thread.Sleep(3);
                    }
                }
                Console.WriteLine("Complete:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");

            }

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
            try
            {
                bool isFirst = true;
                string videoPath = Path.Combine(masterDirectoryPath, Path.GetFileNameWithoutExtension(masterDirectoryPath) + "_pose.mp4");

                using (VideoWriter video = new VideoWriter(videoPath, FourCC.FromString("mp4v"), 30, new OpenCvSharp.Size(640, 360)))
                {
                    if (!video.IsOpened())
                    {
                        Console.WriteLine("video not opened");
                    }

                    video.Set(VideoWriterProperties.HwAcceleration, (double)AVHWDeviceType.AV_HWDEVICE_TYPE_QSV);

                    while (!frameVideoMatQueue.IsCompleted)
                    {
                        if (frameVideoMatQueue.TryTake(out frameDataSet frameInfo))
                        {
                            if (isFirst)
                            {
                                Console.WriteLine("Start:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                                isFirst = false;
                            }

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
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");

            }

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
                drawFocus(g);
            }
        }

        public void drawBBoxs(Graphics g, List<PoseInfo> PoseInfos)
        {
            if (g != null)
            {
                foreach (var info in PoseInfos)
                {
                    bool selectedFlag = false;
                    string poseLine = info.ToLineString();
                    if (dataGridView_PoseLines.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dataGridView_PoseLines.SelectedRows)
                        {
                            if (row.Cells.Count > 2)
                            {
                                string rowLine = row.Cells[2].Value.ToString();
                                selectedFlag = poseLine == rowLine.Replace(rowLine.Split(',')[0], "");
                            }
                            if (selectedFlag) break;
                        }
                    }
                    if (selectedFlag)
                    {
                        g.DrawRectangle(Pens.PaleVioletRed, info.Bbox.Rectangle);
                    }
                    else
                    {
                        g.DrawRectangle(Pens.Blue, info.Bbox.Rectangle);
                    }
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

        public void drawFocus(Graphics g)
        {
            if (g == null) return;

            try
            {
                var row = dataGridView_PoseLines.SelectedRows.Count > 0
                    ? dataGridView_PoseLines.SelectedRows[0]
                    : dataGridView_PoseLines.SelectedCells.Count > 0
                        ? dataGridView_PoseLines.Rows[dataGridView_PoseLines.SelectedCells[0].RowIndex]
                        : null;

                if (row == null) return;

                string[] poseLine = row.Cells[2].Value?.ToString()?.Split(',') ?? Array.Empty<string>();
                if (poseLine.Length <= 2) return;

                int cx = (int)double.Parse(poseLine[1]);
                int cy = (int)double.Parse(poseLine[2]);
                int r = 8;

                g.FillEllipse(Brushes.LightGreen, cx - r, cy - r, r * 2, r * 2);
            }
            catch { }
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
                Console.WriteLine($"\t{(DateTime.Now - taskStartTime).TotalSeconds:0}\tB{frameBitmapQueue.Count}\tT{frameTensorQueue.Count}\tP{framePoseInfoQueue.Count}\tR{frameReportQueue.Count}");
            }
        }

        private void button_LoadPoseInfo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV|*csv";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string[] Lines = File.ReadAllLines(ofd.FileName);

            dataGridView_PoseLines.Rows.Clear();

            foreach (var Line in Lines)
            {
                if (int.TryParse(Line.Split(',')[0], out int frameindex))
                {
                    dataGridView_PoseLines.Rows.Add(new object[] { true, Line.Split(',')[0], Line });
                }
            }
        }

        private void dataGridView_PoseLines_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (capture != null)
            {
                if (dataGridView_PoseLines.SelectedRows.Count < 1) return;

                object value = dataGridView_PoseLines.SelectedRows[0].Cells[1].Value;
                if (value == null) return;

                string cellvalue = value.ToString();

                int newFrameIndex = int.Parse(cellvalue);

                if (trackBar_frameIndex.Value != newFrameIndex)
                {
                    trackBar_frameIndex.Value = newFrameIndex;
                }
                else
                {
                    trackBar_frameIndex_ValueChanged(null, null);
                }

            }
        }

        private void button_SaveFrameChecked_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() != DialogResult.OK) return;
            List<string> Lines = new List<string>();

            foreach (DataGridViewRow row in dataGridView_PoseLines.Rows)
            {
                if (row.Cells.Count <= 2) continue;
                if ((bool)row.Cells[0].Value) Lines.Add(row.Cells[2].Value.ToString());
            }

            File.WriteAllLines(sfd.FileName, Lines);

        }

        private void dataGridView_PoseLines_CurrentCellChanged(object sender, EventArgs e)
        {
            if (capture != null)
            {
                if (dataGridView_PoseLines.SelectedCells.Count < 1) return;
                string cellvalue = dataGridView_PoseLines.Rows[dataGridView_PoseLines.SelectedCells[0].RowIndex].Cells[1].Value.ToString();

                int newFrameIndex = int.Parse(cellvalue);

                if (trackBar_frameIndex.Value != newFrameIndex)
                {
                    trackBar_frameIndex.Value = newFrameIndex;
                }
                else
                {
                    trackBar_frameIndex_ValueChanged(null, null);
                }
            }
        }

        private void button_Check_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_PoseLines.SelectedRows)
            {
                if (row.Cells.Count < 3) continue;
                row.Cells[0].Value = true;
            }
        }

        private void button_UnCheck_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_PoseLines.SelectedRows)
            {
                if (row.Cells.Count < 3) continue;
                row.Cells[0].Value = false;
            }
        }

        private void dataGridView_PoseLines_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView_PoseLines.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn comboBoxColumn)
            {
                var comboBoxCell = dataGridView_PoseLines.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;

                if (comboBoxCell != null && !comboBoxColumn.Items.Contains(e.FormattedValue))
                {
                    comboBoxColumn.Items.Add(e.FormattedValue);
                    comboBoxCell.Value = e.FormattedValue;
                }
            }
        }

        private void tabPage_YOLOPOSE_Enter(object sender, EventArgs e)
        {
            string[] items = textBox_LabelList.Text.Replace("\r\n", "\n").Trim('\n').Split('\n');
            ((DataGridViewComboBoxColumn)dataGridView_PoseLines.Columns[3]).Items.Clear();
            int labelIndex = 1;
            foreach (var item in items)
            {
                ((DataGridViewComboBoxColumn)dataGridView_PoseLines.Columns[3]).Items.Add($"{labelIndex}_{item}");
                labelIndex++;
            }
        }

        private void button_CopyFromTop_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = (DataGridViewSelectedRowCollection)dataGridView_PoseLines.SelectedRows;

            if (rows.Count > 0)
            {
                bool topCheck = bool.Parse(rows[0].Cells[0].Value.ToString());
                string topLabel = rows[0].Cells[3].Value.ToString();

                List<string> rowLabels = new List<string>();
                foreach (DataGridViewRow row in rows)
                {
                    rowLabels.Add(row.Cells[3].Value.ToString());

                }

                foreach (DataGridViewRow row in rows)
                {
                    row.Cells[0].Value = topCheck;
                    row.Cells[3].Value = topLabel;
                }

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
