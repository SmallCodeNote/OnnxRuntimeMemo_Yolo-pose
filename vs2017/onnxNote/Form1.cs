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

            dataGridView_PoseLines.SortCompare += dataGridView_PoseLines_SortCompare;
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

            textBox_PoseInfo.Text = "";

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            List<string> FormContents = new List<string>(WinFormStringCnv.ToString(this).Replace("\r\n", "\n").Split('\n'));
            FormContents.RemoveAll(s => s.StartsWith("dataGridView_PoseLines"));
            FormContents.RemoveAll(s => s.StartsWith("trackBar_frameIndex"));


            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TEXT|*.txt";

            if (false && sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, string.Join("\r\n", FormContents));
            }
            else
            {
                string paramFilename = Path.Combine(thisExeDirPath, "_param.txt");
                File.WriteAllText(paramFilename, string.Join("\r\n", FormContents));
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

            yoloPoseModelHandle.SetOverlapThreshold((float)numericUpDown_bboxOverlapTh.Value, (float)numericUpDown_tolsoOverlapTh.Value, (float)numericUpDown_shoulderOverlapTh.Value);
            OpenMovieFile(ofd.FileName);
        }

        private void OpenMovieFile(string filePath)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() =>
                {
                    OpenMovieFile(filePath);
                }));
            }
            else
            {
                if (capture != null) { capture.Dispose(); targetFilename = ""; }

                string ext = Path.GetExtension(filePath);
                if (ext == ".mp4")
                {
                    targetFilename = Path.GetFileNameWithoutExtension(filePath);
                    capture = new VideoCapture(filePath);
                    trackBar_frameIndex.Maximum = capture.FrameCount;
                    trackBar_frameIndex.Value = 0;
                }
                else
                {
                    pictureBoxUpdate(pictureBox, new Bitmap(filePath));
                }
            }
        }

        private void trackBar_frameIndex_Scroll(object sender, EventArgs e)
        {
            if (capture != null)
            {
                DrawCaptureAndPoseBone();
            }
        }

        private void DrawCaptureAndPoseBone()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() =>
                {
                    DrawCaptureAndPoseBone();
                }));
            }
            else
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
            drawFrame();

        }

        private void drawFrame()
        {
            if (this.InvokeRequired) { this.Invoke((Action)(() => { drawFrame(); })); }
            else
            {
                if (capture != null)
                {
                    using (Mat frame = new Mat())
                    {
                        capture.PosFrames = trackBar_frameIndex.Value > 0 ? trackBar_frameIndex.Value - 1 : 0;

                        if (capture.IsDisposed || !capture.Read(frame) || frame.Empty()) return;

                        Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);

                        yoloPoseModelHandle.SetOverlapThreshold((float)numericUpDown_bboxOverlapTh.Value, (float)numericUpDown_tolsoOverlapTh.Value, (float)numericUpDown_shoulderOverlapTh.Value);
                        List<PoseInfo> poseInfos = yoloPoseModelHandle.Predict(bitmap);
                        drawPose(bitmap, poseInfos);

                        pictureBoxUpdate(pictureBox, bitmap);

                    }

                    label_FrameCount.Text = trackBar_frameIndex.Value.ToString() + " / " + capture.FrameCount.ToString();
                }
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
                yoloPoseModelHandle.SetOverlapThreshold((float)numericUpDown_bboxOverlapTh.Value, (float)numericUpDown_tolsoOverlapTh.Value, (float)numericUpDown_shoulderOverlapTh.Value);

            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                using (Bitmap b = new Bitmap(pictureBox.Image))
                {
                    Clipboard.SetImage(b);
                }
            }
            else
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

        public int PredictTaskBatchSize = 1024;
        List<string> capturePathList = new List<string>();
        string ext = "";

        YoloPoseModelHandle yoloPoseModelHandle;

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (button_Save.Text == "Save")
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "MP4|*.mp4";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                if (ofd.FileNames.Length == 1)
                {

                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                    sfd.Filter = "";
                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    masterDirectoryPath = Path.Combine(Path.GetDirectoryName(sfd.FileName), Path.GetFileNameWithoutExtension(sfd.FileName));

                }

                foreach (var filePath in ofd.FileNames)
                {
                    capturePathList.Add(filePath);
                }



                if (!int.TryParse(textBox_PredictBatchSize.Text, out PredictTaskBatchSize))
                {
                    PredictTaskBatchSize = 1024;
                }

                button_Save.Text = "Cancel";
                timer1.Start();

                yoloPoseModelHandle.SetOverlapThreshold((float)numericUpDown_bboxOverlapTh.Value, (float)numericUpDown_tolsoOverlapTh.Value, (float)numericUpDown_shoulderOverlapTh.Value);

                backgroundWorker_posePredict.RunWorkerAsync();

            }
            else
            {
                backgroundWorker_posePredict.CancelAsync();
            }
        }

        string progressReport = "";

        DateTime taskStartTime;

        BlockingCollection<frameDataSet> frameBitmapQueue;
        BlockingCollection<frameDataSet> frameTensorQueue;
        BlockingCollection<frameDataSet> framePoseInfoQueue;

        BlockingCollection<frameDataSet> frameReportQueue;
        BlockingCollection<frameDataSet> frameVideoMatQueue;
        BlockingCollection<frameDataSet> frameShowQueue;

        string masterDirectoryPath = "";

        private void backgroundWorker_posePredict_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                foreach (string capturePath in capturePathList)
                {
                    Console.WriteLine($"capturePath: {capturePath}");

                    ext = Path.GetExtension(capturePath);

                    if (ext == ".mp4")
                    {
                        if (capture != null) { capture.Dispose(); targetFilename = ""; }

                        capture = new VideoCapture(capturePath);
                        targetFilename = Path.GetFileNameWithoutExtension(capturePath);

                        if (this.InvokeRequired)
                        {
                            this.Invoke((Action)(() =>
                            {
                                trackBar_frameIndex.Maximum = capture.FrameCount;
                                trackBar_frameIndex.Value = 1;
                                Console.WriteLine($"{trackBar_frameIndex.Maximum} {trackBar_frameIndex.Value}");
                            }));
                        }
                    }
                    else
                    {
                        return;
                    }

                    if (capture == null) return;

                    if (capturePathList.Count > 1) masterDirectoryPath = Path.Combine(Path.GetDirectoryName(capturePath), Path.GetFileNameWithoutExtension(capturePath));
                    Console.WriteLine($"masterDirectoryPath: {masterDirectoryPath}");

                    BackgroundWorker worker = (BackgroundWorker)sender;
                    if (!Directory.Exists(masterDirectoryPath)) { Directory.CreateDirectory(masterDirectoryPath); };

                    taskStartTime = DateTime.Now;

                    frameBitmapQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize * 2);
                    frameTensorQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize * 2);
                    framePoseInfoQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize * 2);
                    frameReportQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize * 2);
                    frameVideoMatQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize * 2);
                    frameShowQueue = new BlockingCollection<frameDataSet>(PredictTaskBatchSize * 2);

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

                    timer1.Stop();
                    timer1_Tick(null, null);

                    frameBitmapQueue.Dispose();
                    frameTensorQueue.Dispose();
                    framePoseInfoQueue.Dispose();
                    frameReportQueue.Dispose();
                    frameVideoMatQueue.Dispose();
                    frameShowQueue.Dispose();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");

            }
        }

        private void backgroundWorker_posePredict_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label_FrameCount.Text = progressReport;
            label_FrameCount.Refresh();
        }

        private void backgroundWorker_posePredict_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button_Save.Text = "Save";
        }

        private void dequeue_frameVideoReader(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                int maxIndex = int.MinValue;
                List<frameDataSet> frameList = new List<frameDataSet>(PredictTaskBatchSize);

                int frameIndex = 0;

                using (Mat frame = new Mat())
                {
                    while (capture.Read(frame) && !frame.Empty())
                    {
                        frameIndex = capture.PosFrames;

                        maxIndex = Math.Max(maxIndex, frameIndex);
                        frameList.Add(new frameDataSet(BitmapConverter.ToBitmap(frame), frameIndex));

                        if (frameList.Count >= PredictTaskBatchSize)
                        {
                            progressReport = $"{frameIndex} / {capture.FrameCount}";
                            worker.ReportProgress(0);

                            Console.Write($"  Add+B start {frameList[0].frameIndex} + {frameList.Count}");
                            foreach (var item in frameList)
                            {
                                frameBitmapQueue.Add(item);
                            }
                            Console.WriteLine($"  Add+B comp {frameList[0].frameIndex} - {frameList[frameList.Count - 1].frameIndex}");

                            frameList.Clear();

                        }

                        if (worker.CancellationPending) { e.Cancel = true; break; }
                    }

                    Console.Write($"  Add+B start {frameList[0].frameIndex} + {frameList.Count}");
                    foreach (var item in frameList)
                    {
                        frameBitmapQueue.Add(item);
                    }
                    Console.WriteLine($"  Add+B comp {frameList[0].frameIndex} - {frameList[frameList.Count - 1].frameIndex}");

                    frameList.Clear();
                    frameBitmapQueue.CompleteAdding();
                }

                progressReport = $"{frameIndex} / {capture.FrameCount}";
                worker.ReportProgress(0);

                //capture.Dispose(); targetFilename = "";

                Console.WriteLine($"Complete: {maxIndex} { System.Reflection.MethodBase.GetCurrentMethod().Name}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");
            }
        }

        private void dequeue_frameBitmap()
        {
            try
            {
                int workersCount = Environment.ProcessorCount - 1;
                workersCount = workersCount < 1 ? 1 : workersCount;
                int frameListMasterSize = PredictTaskBatchSize / workersCount;

                var frameBitmapQueueTemp = new BlockingCollection<List<frameDataSet>>(workersCount);
                var frameNextIndexQueue = new ConcurrentQueue<int>();

                Task[] workers = new Task[workersCount];

                for (int i = 0; i < workers.Length; i++)
                {
                    workers[i] = Task.Run(() =>
                    {
                        try
                        {
                            List<frameDataSet> frameList = new List<frameDataSet>(frameListMasterSize);

                            foreach (var frameInfos in frameBitmapQueueTemp.GetConsumingEnumerable())
                            {
                                foreach (var frameInfo in frameInfos)
                                {
                                    Tensor<float> tensor = ConvertBitmapToTensor(frameInfo.bitmap);
                                    List<NamedOnnxValue> inputs = yoloPoseModelHandle.getInputs(tensor);
                                    frameList.Add(new frameDataSet(inputs, frameInfo.bitmap, frameInfo.frameIndex));
                                }

                                if (frameList.Count > 0)
                                {
                                    while (!frameNextIndexQueue.TryPeek(out int nextIndex) || frameList[0].frameIndex != nextIndex) { Thread.Sleep(10); }

                                    Console.Write($"   Add+T start {frameList[0].frameIndex} + {frameList.Count}");
                                    foreach (var item in frameList)
                                    {
                                        frameTensorQueue.Add(item);
                                    }
                                    Console.WriteLine($"  Add+T comp {frameList[0].frameIndex} - {frameList[frameList.Count - 1].frameIndex }");
                                    frameList.Clear();

                                    frameNextIndexQueue.TryDequeue(out int result);
                                }

                                frameInfos.Clear();
                            }
                        }
                        catch
                        {
                            Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name}");
                        }
                    });
                }

                List<frameDataSet> frameListMaster = new List<frameDataSet>(frameListMasterSize);

                while (frameBitmapQueue.Count > 0 || !frameBitmapQueue.IsCompleted)
                {
                    if (frameBitmapQueue.TryTake(out frameDataSet frameInfo, 10))
                    {
                        frameListMaster.Add(frameInfo);

                        if (frameListMaster.Count >= frameListMasterSize)
                        {
                            frameNextIndexQueue.Enqueue(frameListMaster[0].frameIndex);
                            frameBitmapQueueTemp.Add(frameListMaster);
                            frameListMaster = new List<frameDataSet>(frameListMasterSize);
                        }
                    }
                }

                if (frameListMaster.Count > 0)
                {
                    frameNextIndexQueue.Enqueue(frameListMaster[0].frameIndex);
                    frameBitmapQueueTemp.Add(frameListMaster);
                }

                frameBitmapQueueTemp.CompleteAdding();
                Task.WaitAll(workers);
                frameBitmapQueueTemp.Dispose();

                frameTensorQueue.CompleteAdding();

                Console.WriteLine($"Complete: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");
            }
        }

        public unsafe Tensor<float> ConvertBitmapToTensor(Bitmap bitmap, int width = 640, int height = 640)
        {
            var tensor = new DenseTensor<float>(new[] { 1, 3, height, width });
            float[] tensorArray = tensor.Buffer.ToArray();
            int widthMax = Math.Min(bitmap.Width, width);
            int heightMax = Math.Min(bitmap.Height, height);
            const float scale = 1.0f / 255.0f;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb
            );

            byte* ptr = (byte*)bitmapData.Scan0;
            int stride = bitmapData.Stride;

            Parallel.For(0, heightMax, y =>
            {
                byte* row = ptr + y * stride;
                int indexBase = y * width;
                int indexR = indexBase;
                int indexG = indexBase + height * width;
                int indexB = indexBase + height * 2 * width;

                for (int x = 0; x < widthMax; x++)
                {
                    int pixelIndex = x * 3;

                    tensorArray[indexR++] = row[pixelIndex + 2] * scale; // R
                    tensorArray[indexG++] = row[pixelIndex + 1] * scale; // G
                    tensorArray[indexB++] = row[pixelIndex] * scale;     // B
                }
            });

            bitmap.UnlockBits(bitmapData);

            var resultTensor = new DenseTensor<float>(tensorArray, new[] { 1, 3, height, width });
            return resultTensor;
        }

        private void dequeue_frameTensor()
        {
            try
            {
                int maxIndex = int.MinValue;
                List<frameDataSet> frameList = new List<frameDataSet>(PredictTaskBatchSize);

                while (frameTensorQueue.Count > 0 || !frameTensorQueue.IsCompleted)
                {
                    if (frameTensorQueue.TryTake(out frameDataSet frameInfo, 10))
                    {
                        frameList.Add(frameInfo);

                        if (frameList.Count >= PredictTaskBatchSize)
                        {
                            Console.WriteLine($"  StartPredictTask {frameList[0].frameIndex} - {frameList[frameList.Count - 1].frameIndex}");
                            PredictBatch(frameList);
                            frameList.Clear();
                        }
                    }
                }

                if (frameList.Count > 0)
                {
                    Console.WriteLine($"  LastPredictTask {frameList[0].frameIndex} - {frameList[frameList.Count - 1].frameIndex}");
                    PredictBatch(frameList);
                    frameList.Clear();
                }

                framePoseInfoQueue.CompleteAdding();
                Console.WriteLine($"Complete: {maxIndex} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");
            }
        }

        public void PredictBatch(IReadOnlyList<frameDataSet> frameList)
        {
            try
            {
                Console.WriteLine($".....Start: {frameList[0].frameIndex} + {frameList.Count} " + System.Reflection.MethodBase.GetCurrentMethod().Name);

                int arrayMax = frameList.Count;
                for (int i = 0; i < arrayMax; i++)
                {
                    frameList[i].results = yoloPoseModelHandle.PredicteResults(frameList[i].inputs);
                }

                Console.WriteLine($".....Complete: {frameList[0].frameIndex} - {frameList[frameList.Count - 1].frameIndex} {System.Reflection.MethodBase.GetCurrentMethod().Name}");

                framePoseInfoQueueEnqueue(frameList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");
            }
        }



        public void framePoseInfoQueueEnqueue(IReadOnlyList<frameDataSet> frameList)
        {
            try
            {
                Console.Write($"    Add+P start {frameList[0].frameIndex} + {frameList.Count}");
                foreach (var frameInfo in frameList)
                {
                    framePoseInfoQueue.Add(frameInfo);
                }
                Console.WriteLine($"  Add+P comp {frameList[0].frameIndex} - {frameList[frameList.Count - 1].frameIndex}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");
            }
        }

        private void dequeue_framePoseInfo()
        {
            try
            {
                List<frameDataSet> frameList = new List<frameDataSet>(PredictTaskBatchSize);

                while (framePoseInfoQueue.Count > 0 || !framePoseInfoQueue.IsCompleted)
                {
                    if (framePoseInfoQueue.TryTake(out frameDataSet frameInfo, 10))
                    {
                        frameList.Add(frameInfo);

                        if (frameList.Count >= PredictTaskBatchSize)
                        {
                            dequeue_framePoseInfo_addQueue(frameList);
                            frameList.Clear();
                        }
                    }
                }

                if (frameList.Count > 0)
                {
                    dequeue_framePoseInfo_addQueue(frameList);
                    frameList.Clear();
                }

                frameReportQueue.CompleteAdding();
                frameVideoMatQueue.CompleteAdding();
                frameShowQueue.CompleteAdding();

                Console.WriteLine($"Complete: { System.Reflection.MethodBase.GetCurrentMethod().Name}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");
            }
        }

        private void dequeue_framePoseInfo_addQueue(IReadOnlyList<frameDataSet> frameList)
        {
            Console.Write($"      Add+R start {frameList[0].frameIndex} + {frameList.Count}");

            foreach (var frameInfo in frameList)
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

            Console.WriteLine($"  Add+R comp {frameList[0].frameIndex} - {frameList[frameList.Count - 1].frameIndex }");
        }

        string targetFilename = "";

        private void dequeue_frameReport()
        {
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

                PoseValue.Add("filename,frame," + PoseInfo.ToLineStringHeader());


                if (dataGridView_PoseLines.InvokeRequired)
                {
                    dataGridView_PoseLines.Invoke(new Action(() => dataGridView_PoseLines.Rows.Clear()));
                }

                while (frameReportQueue.Count > 0 || !frameReportQueue.IsCompleted)
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

                                string poseString = pose.ToLineString();
                                linePose = targetFilename + "," + posFrame + "," + poseString;

                                PoseValue.Add(linePose);

                                if (dataGridView_PoseLines.InvokeRequired)
                                {
                                    dataGridView_PoseLines.Invoke(new Action(() => dataGridView_PoseLines.Rows.Add(new object[] { false, targetFilename, posFrame.ToString(), poseString })));
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
                while (frameShowQueue.Count > 0 || !frameShowQueue.IsCompleted)
                {
                    if (frameShowQueue.TryTake(out frameDataSet frameInfo, 10))
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
            int frameIndex = 0;
            try
            {
                string videoPath = Path.Combine(masterDirectoryPath, Path.GetFileNameWithoutExtension(masterDirectoryPath) + "_pose.mp4");

                using (VideoWriter video = new VideoWriter(videoPath, FourCC.FromString("mp4v"), 30, new OpenCvSharp.Size(640, 360)))
                {
                    if (!video.IsOpened())
                    {
                        Console.WriteLine("video not opened");
                    }

                    video.Set(VideoWriterProperties.HwAcceleration, (double)AVHWDeviceType.AV_HWDEVICE_TYPE_QSV);

                    while (frameVideoMatQueue.Count > 0 || !frameVideoMatQueue.IsCompleted)
                    {
                        if (frameVideoMatQueue.TryTake(out frameDataSet frameInfo, 10))
                        {
                            frameIndex = frameInfo.frameIndex;
                            video.Write(frameInfo.mat);
                            frameInfo.mat.Dispose();
                        }
                    }

                    video.Release();

                }
                Console.WriteLine("Complete:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: frameIndex={frameIndex} : {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");

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

            if (this.InvokeRequired) { this.Invoke((Action)(() => { drawFocus(g); })); }
            else
            {
                try
                {
                    var row = dataGridView_PoseLines.SelectedRows.Count > 0
                        ? dataGridView_PoseLines.SelectedRows[0]
                        : dataGridView_PoseLines.SelectedCells.Count > 0
                            ? dataGridView_PoseLines.Rows[dataGridView_PoseLines.SelectedCells[0].RowIndex]
                            : null;

                    if (row == null) return;

                    string[] poseLine = row.Cells[3].Value?.ToString()?.Split(',') ?? Array.Empty<string>();
                    if (poseLine.Length <= 2) return;

                    int cx = (int)double.Parse(poseLine[0]);
                    int cy = (int)double.Parse(poseLine[1]);
                    int r = 8;

                    g.FillEllipse(Brushes.LightGreen, cx - r, cy - r, r * 2, r * 2);

                    var headers = PoseInfo.ToLineStringHeader().Split(',');

                    if (headers.Length < poseLine.Length) return;

                    var result = new StringBuilder();
                    for (int i = 0; i < poseLine.Length; i++)
                    {
                        result.AppendLine($"{headers[i]}: {poseLine[i]}");
                    }
                    textBox_PoseInfo.Text = result.ToString();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");
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

            List<string> LabelItems = ((DataGridViewComboBoxColumn)dataGridView_PoseLines.Columns[4]).Items.Cast<string>().ToList();

            foreach (var Line in Lines)
            {
                string[] cols = Line.Split(',');
                string targetFilenameString = cols[0];
                string frameIndexString = cols[1];
                string labelString = cols[cols.Length - 1];
                string bodyString = string.Join(",", cols.Skip(2).Take(cols.Length - 3));

                int labelIndex = -1;
                if (!int.TryParse(labelString, out labelIndex)) { labelIndex = -1; }
                string label = labelIndex < 0 || labelIndex >= LabelItems.Count ? "" : LabelItems[labelIndex];


                if (int.TryParse(frameIndexString, out int frameindex))
                {
                    dataGridView_PoseLines.Rows.Add(new object[] { false, targetFilenameString, frameIndexString, bodyString, label });
                }
            }
        }

        private void dataGridView_PoseLines_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView_PoseLines.SelectedRows.Count < 1) return;

            DataGridViewRow row = dataGridView_PoseLines.SelectedRows[0];

            string filename = row.Cells[1].Value.ToString();

            if (capture == null || targetFilename != filename)
            {
                string filePath = Path.Combine(textBox_topDirectoryPath.Text, filename);

                if (Path.GetExtension(filePath) != ".mp4") { filePath += ".mp4"; }

                if (File.Exists(filePath)) OpenMovieFile(filePath);
                targetFilename = filename;
            }

            if (capture != null)
            {

                object value = row.Cells[2].Value;
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
            sfd.Filter = "CSV|*.csv";
            sfd.FileName = "LabeledPose.csv";
            sfd.OverwritePrompt = false;

            if (sfd.ShowDialog() != DialogResult.OK) return;

            List<string> Lines = new List<string>();
            List<string> LabelItems = ((DataGridViewComboBoxColumn)dataGridView_PoseLines.Columns[4]).Items.Cast<string>().ToList();


            if (File.Exists(sfd.FileName))
            {
                string[] LinesExist = File.ReadAllLines(sfd.FileName);
                Lines.AddRange(LinesExist);

            }
            else
            {
                Lines.Add("filename,frame," + PoseInfo.ToLineStringHeader());
            }

            foreach (DataGridViewRow row in dataGridView_PoseLines.Rows)
            {
                if (row.Cells.Count <= 4 || row.Cells[2].Value == null) continue;

                string filenameString = row.Cells[1].Value.ToString();
                string frameIndexString = row.Cells[2].Value.ToString();
                string bodyString = row.Cells[3].Value.ToString();
                string label = row.Cells[4].Value == null ? "-1" : row.Cells[4].Value.ToString();
                int labelIndex = LabelItems.IndexOf(label);

                try
                {
                    if ((bool)(row.Cells[0].Value))
                    {
                        string prefix = $"{filenameString},{frameIndexString},{bodyString}";
                        string newLine = $"{prefix},{labelIndex}";

                        int existingIndex = Lines.FindIndex(line => line.StartsWith(prefix + ",") || line == prefix);
                        if (existingIndex >= 0)
                        {
                            Lines[existingIndex] = newLine;
                        }
                        else
                        {
                            Lines.Add(newLine);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");
                }
            }

            string header = Lines.FirstOrDefault();
            var sortedLines = Lines
                .Skip(1)
                .OrderBy(line =>
                {
                    string[] parts = line.Split(',');
                    return parts[0];
                })
                .OrderBy(line =>
                {
                    string[] parts = line.Split(',');
                    if (parts.Length < 1) return int.MaxValue;

                    int frameIndex;
                    return int.TryParse(parts[1], out frameIndex) ? frameIndex : int.MaxValue;
                })
                .ToList();

            sortedLines.Insert(0, header);

            File.WriteAllLines(sfd.FileName, sortedLines);

        }

        private void dataGridView_PoseLines_CurrentCellChanged(object sender, EventArgs e)
        {
            if (capture != null && dataGridView_PoseLines.Rows.Count > 0)
            {
                try
                {
                    if (dataGridView_PoseLines.SelectedCells.Count < 1
                        || dataGridView_PoseLines.Rows[dataGridView_PoseLines.SelectedCells[0].RowIndex].Cells[2].Value == null) return;

                    string cellvalue = dataGridView_PoseLines.Rows[dataGridView_PoseLines.SelectedCells[0].RowIndex].Cells[2].Value.ToString();

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
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.StackTrace}");
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

        private void dataGridView_PoseLines_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {

            if (e.Column.Name == "Column_Frame")
            {
                int val1, val2;

                // 数値に変換して比較（変換できない場合は 0）
                bool parsed1 = int.TryParse(e.CellValue1?.ToString(), out val1);
                bool parsed2 = int.TryParse(e.CellValue2?.ToString(), out val2);

                e.SortResult = val1.CompareTo(val2);

                // 同値だったら行インデックスで比較
                if (e.SortResult == 0)
                    e.SortResult = e.RowIndex1.CompareTo(e.RowIndex2);

                e.Handled = true;
            }
        }

        private void tabPage_YOLOPOSE_Enter(object sender, EventArgs e)
        {
            string[] items = textBox_LabelList.Text.Replace("\r\n", "\n").Trim('\n').Split('\n');
            ((DataGridViewComboBoxColumn)dataGridView_PoseLines.Columns[4]).Items.Clear();
            int labelIndex = 0;
            foreach (var item in items)
            {
                ((DataGridViewComboBoxColumn)dataGridView_PoseLines.Columns[4]).Items.Add($"{labelIndex}_{item}");
                labelIndex++;
            }
        }

        private void button_CopyFromTop_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = (DataGridViewSelectedRowCollection)dataGridView_PoseLines.SelectedRows;

            if (rows.Count > 0)
            {
                bool topCheck = bool.Parse(rows[0].Cells[0].Value.ToString());
                string topLabel = rows[0].Cells[4].Value.ToString();

                List<string> rowLabels = new List<string>();
                foreach (DataGridViewRow row in rows)
                {
                    if (row.Cells[4].Value != null) rowLabels.Add(row.Cells[4].Value.ToString());

                }

                foreach (DataGridViewRow row in rows)
                {
                    row.Cells[0].Value = topCheck;
                    row.Cells[4].Value = topLabel;
                }

            }
        }

        private void button_SaveWorkSetting_Click(object sender, EventArgs e)
        {
            List<string> FormContents = new List<string>(WinFormStringCnv.ToString(this).Replace("\r\n", "\n").Split('\n'));
            FormContents.RemoveAll(s => s.StartsWith("dataGridView_PoseLines"));
            FormContents.RemoveAll(s => s.StartsWith("trackBar_frameIndex"));

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TEXT|*.txt";
            sfd.FileName = textBox_WorkTitle.Text;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, string.Join("\r\n", FormContents));
            }
        }

        private void button_LoadWorkSetting_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TEXT|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                WinFormStringCnv.setControlFromString(this, File.ReadAllText(ofd.FileName));
            }
        }

        private void numericUpDown_bboxOverlapTh_ValueChanged(object sender, EventArgs e)
        {
            if (yoloPoseModelHandle == null) return;
            yoloPoseModelHandle.OverlapBBoxThreshold = (float)numericUpDown_bboxOverlapTh.Value;
            trackBar_frameIndex_ValueChanged(null, null);
        }

        private void numericUpDown_tolsoOverlapTh_ValueChanged(object sender, EventArgs e)
        {
            if (yoloPoseModelHandle == null) return;
            yoloPoseModelHandle.OverlapTolsoThreshold = (float)numericUpDown_tolsoOverlapTh.Value;
            trackBar_frameIndex_ValueChanged(null, null);
        }

        private void numericUpDown_shoulderOverlapTh_ValueChanged(object sender, EventArgs e)
        {
            if (yoloPoseModelHandle == null) return;
            yoloPoseModelHandle.OverlapShoulderThreshold = (float)numericUpDown_shoulderOverlapTh.Value;
            trackBar_frameIndex_ValueChanged(null, null);
        }

        private void button_SwitchOverLapBbox_Click(object sender, EventArgs e)
        {
            numericUpDown_bboxOverlapTh.Value *= -1;
        }

        private void button_SwitchOverLapTolso_Click(object sender, EventArgs e)
        {
            numericUpDown_tolsoOverlapTh.Value *= -1;
        }

        private void button_SwitchOverLapShoulder_Click(object sender, EventArgs e)
        {
            numericUpDown_shoulderOverlapTh.Value *= -1;
        }

        private bool suppressMouseEnter = false;
        private void dataGridView_PoseLines_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (suppressMouseEnter) return;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var dgv = sender as DataGridView;

                if (dgv.CurrentCell != dgv.Rows[e.RowIndex].Cells[e.ColumnIndex])
                {
                    var column = dgv.Columns[e.ColumnIndex];

                    if (column is DataGridViewComboBoxColumn)
                    {
                        try
                        {
                            suppressMouseEnter = true;
                            dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                            //dgv.BeginEdit(true);
                        }
                        finally
                        {
                            suppressMouseEnter = false;
                        }
                    }
                }
            }
        }

        private void button_sortPoseInfoBBox_Click(object sender, EventArgs e)
        {
            int LineLen = dataGridView_PoseLines.Rows.Count - 1;

            for (int ri = 0; ri < LineLen; ri++)
            {
                DataGridViewRow row = dataGridView_PoseLines.Rows[ri];

                string filename = row.Cells[1].Value.ToString();
                if (capture == null || targetFilename != filename)
                {
                    string filePath = Path.Combine(textBox_topDirectoryPath.Text, filename);

                    if (Path.GetExtension(filePath) != ".mp4") { filePath += ".mp4"; }

                    if (File.Exists(filePath)) OpenMovieFile(filePath);
                    targetFilename = filename;
                }

                if (capture != null)
                {
                    object value = row.Cells[2].Value;
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
