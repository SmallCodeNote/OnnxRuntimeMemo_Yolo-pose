using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

using OpenCvSharp;

namespace YoloPoseOnnxHandle
{
    class YoloPoseModelHandle : IDisposable
    {
        public string SessionInputName = "";

        public Tensor<float> ImageTensor;
        public List<PoseInfo> PoseInfos;
        private InferenceSession session;
        private int modelOutputStride = 8400;

        public float ConfidenceThreshold = 0.6f;

        public YoloPoseModelHandle(string modelfilePath, int deviceID = -1)
        {
            if (File.Exists(modelfilePath))
            {
                if (session != null) session.Dispose();

                SessionOptions sessionOptions = new SessionOptions();
                try
                {
                    if (deviceID >= 0) { sessionOptions.AppendExecutionProvider_DML(deviceID); }
                    else { sessionOptions.AppendExecutionProvider_CPU(); }

                    sessionOptions.ExecutionMode = ExecutionMode.ORT_PARALLEL; // 並列実行モードの使用
                    sessionOptions.GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL; // グラフ最適化を有効化

                }
                catch
                {
                    Console.WriteLine("ExecutionProviderAppendError");
                }

                session = new InferenceSession(modelfilePath, sessionOptions);
                SessionInputName = session.InputMetadata.Keys.First();
            }
        }

        public void Dispose()
        {
            if (session != null) session.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool setModel(string modelfilePath, int deviceID = 0)
        {
            if (File.Exists(modelfilePath))
            {
                if (session != null) session.Dispose();

                SessionOptions sessionOptions = new SessionOptions();
                try
                {
                    sessionOptions.AppendExecutionProvider_DML(deviceID);
                }
                catch
                {
                    Console.WriteLine("ExecutionProviderAppendError");
                }

                session = new InferenceSession(modelfilePath, sessionOptions);
                SessionInputName = session.InputMetadata.Keys.First();
                return true;
            }
            return false;
        }

        public List<PoseInfo> Predicte(Bitmap bitmap, float confidenceThreshold = -1.0f)
        {
            if (confidenceThreshold < 0) { confidenceThreshold = ConfidenceThreshold; }
            ImageTensor = ConvertBitmapToTensor(bitmap);

            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(SessionInputName, ImageTensor) };
            var results = session.Run(inputs);
            var output = results.First().AsEnumerable<float>().ToArray();
            results.Dispose();

            return PoseInfoRead(output, confidenceThreshold);
        }

        public List<PoseInfo> Predicte(Tensor<float> ImageTensor, float confidenceThreshold = -1.0f)
        {
            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(SessionInputName, ImageTensor) };
            var results = session.Run(inputs);
            var output = results.First().AsEnumerable<float>().ToArray();
            results.Dispose();

            if (confidenceThreshold < 0) { confidenceThreshold = ConfidenceThreshold; }
            return PoseInfoRead(output, confidenceThreshold);
        }

        public float[] PredicteOutput(Tensor<float> ImageTensor, float confidenceThreshold = -1.0f)
        {
            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(SessionInputName, ImageTensor) };
            var results = session.Run(inputs);
            var output = results.First().AsEnumerable<float>().ToArray();
            results.Dispose();

            return output;
        }
        public IDisposableReadOnlyCollection<DisposableNamedOnnxValue> PredicteResults(Tensor<float> ImageTensor)
        {
            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(SessionInputName, ImageTensor) };
            return session.Run(inputs);
        }

        public IDisposableReadOnlyCollection<DisposableNamedOnnxValue> PredicteResults(List<NamedOnnxValue> inputs)
        {
            return session.Run(inputs);
        }

        public List<IDisposableReadOnlyCollection<DisposableNamedOnnxValue>> PredicteResults(List<List<NamedOnnxValue>> inputsList)
        {
            List<IDisposableReadOnlyCollection<DisposableNamedOnnxValue>> results = new List<IDisposableReadOnlyCollection<DisposableNamedOnnxValue>>();
            foreach (var inputs in inputsList)
            {
                results.Add(session.Run(inputs));
            }
            return results;
        }

        public List<IDisposableReadOnlyCollection<DisposableNamedOnnxValue>> PredictBatch(List<List<NamedOnnxValue>> batchedInputs)
        {
            var results = new List<IDisposableReadOnlyCollection<DisposableNamedOnnxValue>>();

            foreach (var inputs in batchedInputs)
            {
                using (var result = session.Run(inputs))
                {
                    results.Add(result);
                }
            }

            return results;
        }

        public List<NamedOnnxValue> getInputs(Tensor<float> ImageTensor)
        {
            return new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(SessionInputName, ImageTensor) };
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


        Tensor<float> ConvertMatToTensor(Mat mat, int width = 640, int height = 640)
        {
            Mat resizedMat = new Mat();
            Cv2.Resize(mat, resizedMat, new OpenCvSharp.Size(width, height));

            var tensor = new DenseTensor<float>(new[] { 1, 3, height, width });

            if (resizedMat.Type() != MatType.CV_8UC3)
            {
                throw new ArgumentException("Input Mat must be of type CV_8UC3 (8-bit, 3 channels).");
            }

            for (int y = 0; y < height; y++)
            {
                var row = resizedMat.Row(y);

                for (int x = 0; x < width; x++)
                {
                    Vec3b pixel = row.At<Vec3b>(0, x);
                    tensor[0, 0, y, x] = pixel.Item2 / 255.0f; // R
                    tensor[0, 1, y, x] = pixel.Item1 / 255.0f; // G
                    tensor[0, 2, y, x] = pixel.Item0 / 255.0f; // B
                }
            }

            return tensor;
        }



        public override string ToString()
        {
            return SessionInputName;
        }

        public List<PoseInfo> PoseInfoRead(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results, float confidenceThreshold = -1.0f)
        {
            float[] outputArray = results.First().AsEnumerable<float>().ToArray();
            return PoseInfoRead(outputArray);
        }


        public List<PoseInfo> PoseInfoRead(float[] outputArray, float confidenceThreshold = -1.0f)
        {
            if (confidenceThreshold < 0) { confidenceThreshold = ConfidenceThreshold; }
            List<PoseInfo> PoseInfos = new List<PoseInfo>();
            for (int i = 0; i < modelOutputStride; i++)
            {
                PoseInfo pi = new PoseInfo(outputArray, i);
                if (pi.Bbox.Confidence >= confidenceThreshold)
                {
                    if (PoseInfos.Count > 0)
                    {
                        bool update = false;

                        for (int index = 0; index < PoseInfos.Count; index++)
                        {
                            var item = PoseInfos[index];
                            if (item.Bbox.Overlap(pi.Bbox) >= 0.8)
                            {
                                item.Bbox.OverlapUpdate(pi.Bbox);
                                update = true;
                            }
                        }

                        if (!update) PoseInfos.Add(pi);
                    }
                    else
                    {
                        PoseInfos.Add(pi);
                    }
                }
            }
            this.PoseInfos = PoseInfos;
            return PoseInfos;
        }
    }

    public class PoseInfoSet
    {
        public List<PoseInfo> PoseInfos;
        private int stride = 8400;

        public PoseInfoSet(float[] outputArray, float confidenceThreshold = 0.6f)
        {
            PoseInfos = new List<PoseInfo>();
            for (int i = 0; i < stride; i++)
            {
                PoseInfo pi = new PoseInfo(outputArray, i);
                if (pi.Bbox.Confidence >= confidenceThreshold)
                {
                    if (PoseInfos.Count > 0)
                    {
                        bool update = false;

                        for (int index = 0; index < PoseInfos.Count; index++)
                        {
                            var item = PoseInfos[index];
                            if (item.Bbox.Overlap(pi.Bbox) >= 0.8)
                            {
                                item.Bbox.OverlapUpdate(pi.Bbox);
                                update = true;
                            }
                        }

                        if (!update) PoseInfos.Add(pi);
                    }
                    else
                    {
                        PoseInfos.Add(pi);
                    }
                }
            }
        }
    }

    public class PoseInfo
    {
        public Bbox Bbox;
        public PoseKeyPoints KeyPoints;

        public PoseInfo(float[] outputArray, int startIndex)
        {
            Bbox = new Bbox(outputArray, startIndex);
            KeyPoints = new PoseKeyPoints(outputArray, startIndex);
        }
    }

    public class Bbox
    {
        public float Center_x;
        public float Center_y;
        public float Width;
        public float Height;
        public float Confidence;
        private int stride = 8400;


        public Bbox(float[] outputArray, int startIndex)
        {
            this.Center_x = outputArray[startIndex + stride * 0];
            this.Center_y = outputArray[startIndex + stride * 1];
            this.Width = outputArray[startIndex + stride * 2];
            this.Height = outputArray[startIndex + stride * 3];
            this.Confidence = outputArray[startIndex + stride * 4];
        }

        public float Left { get { return Center_x - Width / 2.0f; } }
        public float Right { get { return Center_x + Width / 2.0f; } }
        public float Top { get { return Center_y - Height / 2.0f; } }
        public float Bottom { get { return Center_y + Height / 2.0f; } }
        public float Area { get { return Width * Height; } }

        public Rectangle Rectangle { get { return new Rectangle((int)Left, (int)Top, (int)Width, (int)Height); } }

        public float Overlap(Bbox bbox)
        {
            float intersectionLeft = Math.Max(this.Left, bbox.Left);
            float intersectionTop = Math.Max(this.Top, bbox.Top);
            float intersectionRight = Math.Min(this.Right, bbox.Right);
            float intersectionBottom = Math.Min(this.Bottom, bbox.Bottom);

            float intersectionWidth = Math.Max(0, intersectionRight - intersectionLeft);
            float intersectionHeight = Math.Max(0, intersectionBottom - intersectionTop);

            float intersectionArea = intersectionWidth * intersectionHeight;
            float thisArea = this.Area;
            float otherArea = bbox.Area;

            float unionArea = thisArea + otherArea - intersectionArea;

            return intersectionArea / unionArea;
        }

        public float OverlapUpdate(Bbox bbox)
        {
            float intersectionLeft = Math.Max(this.Left, bbox.Left);
            float intersectionTop = Math.Max(this.Top, bbox.Top);
            float intersectionRight = Math.Min(this.Right, bbox.Right);
            float intersectionBottom = Math.Min(this.Bottom, bbox.Bottom);

            float intersectionWidth = Math.Max(0, intersectionRight - intersectionLeft);
            float intersectionHeight = Math.Max(0, intersectionBottom - intersectionTop);

            float intersectionArea = intersectionWidth * intersectionHeight;
            float thisArea = this.Area;
            float otherArea = bbox.Area;

            float unionArea = thisArea + otherArea - intersectionArea;

            if (unionArea > 0)
            {
                this.Center_x = (intersectionLeft + intersectionRight) * 0.5f;
                this.Center_y = (intersectionTop + intersectionBottom) * 0.5f;
                this.Width = intersectionWidth;
                this.Height = intersectionHeight;
            }

            return intersectionArea / unionArea;
        }


        public override string ToString()
        {
            return $"{Confidence:0.00},{Center_x:0},{Center_y:0},{Width:0},{Height:0}";
        }
    }

    public class PoseKeyPoints
    {
        public KeyPoint Nose;
        public KeyPoint EyeLeft;
        public KeyPoint EyeRight;
        public KeyPoint EarLeft;
        public KeyPoint EarRight;
        public KeyPoint ShoulderLeft;
        public KeyPoint ShoulderRight;
        public KeyPoint ElbowLeft;
        public KeyPoint ElbowRight;
        public KeyPoint WristLeft;
        public KeyPoint WristRight;
        public KeyPoint HipLeft;
        public KeyPoint HipRight;
        public KeyPoint KneeLeft;
        public KeyPoint KneeRight;
        public KeyPoint AnkleLeft;
        public KeyPoint AnkleRight;

        private float KeyPointAngle(KeyPoint p0, float Confidence0, KeyPoint p1, float Confidence1, KeyPoint p2, float Confidence2)
        {
            if (p0.Confidence >= Confidence0 && p1.Confidence >= Confidence1 && p2.Confidence >= Confidence2)
            {
                double v1X = p1.X - p0.X;
                double v1Y = p1.Y - p0.Y;
                double v2X = p2.X - p0.X;
                double v2Y = p2.Y - p0.Y;

                double dotProduct = v1X * v2X + v1Y * v2Y;

                double magnitudeV1 = Math.Sqrt(v1X * v1X + v1Y * v1Y);
                double magnitudeV2 = Math.Sqrt(v2X * v2X + v2Y * v2Y);

                double angleRad = Math.Acos(dotProduct / (magnitudeV1 * magnitudeV2));

                return (float)(angleRad * (180 / Math.PI));
            }
            else
            {
                return -1f;
            }
        }

        private float KeyPointLength(KeyPoint p0, float Confidence0, KeyPoint p1, float Confidence1)
        {
            if (p0.Confidence >= Confidence0 && p1.Confidence >= Confidence1)
            {
                double v1X = p1.X - p0.X;
                double v1Y = p1.Y - p0.Y;

                return (float)(Math.Sqrt(v1X * v1X + v1Y * v1Y));
            }
            else
            {
                return -1f;
            }
        }


        private float KeyPointLength0(KeyPoint p0, float Confidence0, KeyPoint p1, float Confidence1)
        {
            if (p0.Confidence >= Confidence0 && p1.Confidence >= Confidence1)
            {
                double v1X = p1.X - p0.X;
                double v1Y = p1.Y - p0.Y;

                return (float)(Math.Sqrt(v1X * v1X + v1Y * v1Y));
            }
            else
            {
                return -1f;
            }
        }


        private KeyPoint KeyPointSum(KeyPoint p1, KeyPoint p2, float Confidence)
        {
            if (p1.Confidence >= Confidence && p2.Confidence >= Confidence)
            {
                return new KeyPoint((p1.X + p2.X) * 0.5f, (p1.Y + p2.Y) * 0.5f, p1.Confidence < p2.Confidence ? p1.Confidence : p2.Confidence);
            }
            else if (p1.Confidence >= Confidence)
            {
                return new KeyPoint(p1.X, p1.Y, p1.Confidence);
            }
            else if (p2.Confidence >= Confidence)
            {
                return new KeyPoint(p2.X, p2.Y, p2.Confidence);
            }


            return new KeyPoint();
        }

        private KeyPoint KeyPointAve(float Confidence, params KeyPoint[] ps)
        {
            float pCount = 0;
            float Xsum = 0;
            float Ysum = 0;
            float ConfidenceMin = 1f;

            foreach (var p in ps)
            {
                if (p.Confidence >= Confidence)
                {
                    ConfidenceMin = p.Confidence < ConfidenceMin ? p.Confidence : ConfidenceMin;
                    Xsum += p.X;
                    Ysum += p.Y;
                    pCount++;
                }
            }

            if (pCount <= 0) { return new KeyPoint(); }
            return new KeyPoint(Xsum / pCount, Ysum / pCount, ConfidenceMin);
        }

        public KeyPoint Head(float confidenceLevel) { return KeyPointAve(confidenceLevel, Nose, EyeLeft, EyeRight, EarLeft, EarRight); }

        public KeyPoint Eye(float confidenceLevel) { return KeyPointSum(EyeLeft, EyeRight, confidenceLevel); }
        public KeyPoint Ear(float confidenceLevel) { return KeyPointSum(EarLeft, EarRight, confidenceLevel); }
        public KeyPoint Shoulder(float confidenceLevel) { return KeyPointSum(ShoulderLeft, ShoulderRight, confidenceLevel); }
        public KeyPoint Elbow(float confidenceLevel) { return KeyPointSum(ElbowLeft, ElbowRight, confidenceLevel); }
        public KeyPoint Wrist(float confidenceLevel) { return KeyPointSum(WristLeft, WristRight, confidenceLevel); }
        public KeyPoint Hip(float confidenceLevel) { return KeyPointSum(HipLeft, HipRight, confidenceLevel); }
        public KeyPoint Knee(float confidenceLevel) { return KeyPointSum(KneeLeft, KneeRight, confidenceLevel); }
        public KeyPoint Ankle(float confidenceLevel) { return KeyPointSum(AnkleLeft, AnkleRight, confidenceLevel); }

        public KeyPoint Head() { return KeyPointAve(confidenceLevel_Head, Nose, EyeLeft, EyeRight, EarLeft, EarRight); }
        public KeyPoint Eye() { return KeyPointSum(EyeLeft, EyeRight, confidenceLevel_Eye); }
        public KeyPoint Ear() { return KeyPointSum(EarLeft, EarRight, confidenceLevel_Ear); }
        public KeyPoint Shoulder() { return KeyPointSum(ShoulderLeft, ShoulderRight, confidenceLevel_Shoulder); }
        public KeyPoint Elbow() { return KeyPointSum(ElbowLeft, ElbowRight, confidenceLevel_Elbow); }
        public KeyPoint Wrist() { return KeyPointSum(WristLeft, WristRight, confidenceLevel_Wrist); }
        public KeyPoint Hip() { return KeyPointSum(HipLeft, HipRight, confidenceLevel_Hip); }
        public KeyPoint Knee() { return KeyPointSum(KneeLeft, KneeRight, confidenceLevel_Knee); }
        public KeyPoint Ankle() { return KeyPointSum(AnkleLeft, AnkleRight, confidenceLevel_Ankle); }

        public float ElbowLeftAngle { get { return KeyPointAngle(ElbowLeft, confidenceLevel_Elbow, ShoulderLeft, confidenceLevel_Shoulder, WristLeft, confidenceLevel_Wrist); } }
        public float ElbowRightAngle { get { return KeyPointAngle(ElbowRight, confidenceLevel_Elbow, ShoulderRight, confidenceLevel_Shoulder, WristRight, confidenceLevel_Wrist); } }
        public float KneeLeftAngle { get { return KeyPointAngle(KneeLeft, confidenceLevel_Knee, HipLeft, confidenceLevel_Hip, AnkleLeft, confidenceLevel_Ankle); } }
        public float KneeRightAngle { get { return KeyPointAngle(KneeRight, confidenceLevel_Knee, HipRight, confidenceLevel_Hip,  AnkleRight, confidenceLevel_Ankle); } }

        public float WristLeftLength { get { return KeyPointLength(ElbowLeft, confidenceLevel_Elbow, WristLeft, confidenceLevel_Wrist); } }
        public float WristRightLength { get { return KeyPointLength(ElbowRight, confidenceLevel_Elbow, WristRight, confidenceLevel_Wrist); } }
        public float ElbowLeftLength { get { return KeyPointLength(ShoulderLeft, confidenceLevel_Shoulder, ElbowLeft, confidenceLevel_Elbow); } }
        public float ElbowRightLength { get { return KeyPointLength(ShoulderRight, confidenceLevel_Shoulder, ElbowRight, confidenceLevel_Elbow); } }
        public float KneeLeftLength { get { return KeyPointLength(HipLeft, confidenceLevel_Hip, KneeLeft, confidenceLevel_Knee); } }
        public float KneeRightLength { get { return KeyPointLength(HipRight, confidenceLevel_Hip, KneeRight, confidenceLevel_Knee); } }
        public float AnkleLeftLength { get { return KeyPointLength(KneeLeft, confidenceLevel_Knee, AnkleLeft, confidenceLevel_Ankle); } }
        public float AnkleRightLength { get { return KeyPointLength(KneeRight, confidenceLevel_Knee, AnkleRight, confidenceLevel_Ankle); } }

        public float ShoulderWidth { get { return KeyPointLength0(ShoulderLeft, confidenceLevel_Shoulder, ShoulderRight, confidenceLevel_Shoulder); } }
        public float HipWidth { get { return KeyPointLength0(HipLeft, confidenceLevel_Hip, HipRight, confidenceLevel_Hip); } }
        public float EyeWidth { get { return KeyPointLength0(EyeLeft, confidenceLevel_Eye, EyeRight, confidenceLevel_Eye); } }
        public float EarWidth { get { return KeyPointLength0(EarLeft, confidenceLevel_Ear, EarRight, confidenceLevel_Ear); } }


        public float confidenceLevel_Head = 0.6f;
        public float confidenceLevel_Eye = 0.6f;
        public float confidenceLevel_Ear = 0.6f;
        public float confidenceLevel_Shoulder = 0.6f;
        public float confidenceLevel_Elbow = 0.6f;
        public float confidenceLevel_Wrist = 0.6f;
        public float confidenceLevel_Hip = 0.6f;
        public float confidenceLevel_Knee = 0.6f;
        public float confidenceLevel_Ankle = 0.6f;

        public PoseKeyPoints(float[] output, int startIndex)
        {
            Nose = new KeyPoint(output, startIndex, 0);
            EyeLeft = new KeyPoint(output, startIndex, 1);
            EyeRight = new KeyPoint(output, startIndex, 2);
            EarLeft = new KeyPoint(output, startIndex, 3);
            EarRight = new KeyPoint(output, startIndex, 4);
            ShoulderLeft = new KeyPoint(output, startIndex, 5);
            ShoulderRight = new KeyPoint(output, startIndex, 6);
            ElbowLeft = new KeyPoint(output, startIndex, 7);
            ElbowRight = new KeyPoint(output, startIndex, 8);
            WristLeft = new KeyPoint(output, startIndex, 9);
            WristRight = new KeyPoint(output, startIndex, 10);
            HipLeft = new KeyPoint(output, startIndex, 11);
            HipRight = new KeyPoint(output, startIndex, 12);
            KneeLeft = new KeyPoint(output, startIndex, 13);
            KneeRight = new KeyPoint(output, startIndex, 14);
            AnkleLeft = new KeyPoint(output, startIndex, 15);
            AnkleRight = new KeyPoint(output, startIndex, 16);
        }

        public void drawBone(Graphics g, float confidenceLevel = 0.6f, float diameter = 8)
        {
            Pen p = new Pen(Color.Blue, 2);

            if (Nose.Confidence >= confidenceLevel && EyeLeft.Confidence >= confidenceLevel)
                g.DrawLine(p, Nose.Position, EyeLeft.Position);
            if (EyeLeft.Confidence >= confidenceLevel && EarLeft.Confidence >= confidenceLevel)
                g.DrawLine(p, EyeLeft.Position, EarLeft.Position);
            if (Nose.Confidence >= confidenceLevel && EyeRight.Confidence >= confidenceLevel)
                g.DrawLine(p, Nose.Position, EyeRight.Position);
            if (EyeRight.Confidence >= confidenceLevel && EarRight.Confidence >= confidenceLevel)
                g.DrawLine(p, EyeRight.Position, EarRight.Position);
            if (ShoulderLeft.Confidence >= confidenceLevel && ShoulderRight.Confidence >= confidenceLevel)
                g.DrawLine(p, ShoulderLeft.Position, ShoulderRight.Position);
            if (ShoulderLeft.Confidence >= confidenceLevel && ElbowLeft.Confidence >= confidenceLevel)
                g.DrawLine(p, ShoulderLeft.Position, ElbowLeft.Position);
            if (ElbowLeft.Confidence >= confidenceLevel && WristLeft.Confidence >= confidenceLevel)
                g.DrawLine(p, ElbowLeft.Position, WristLeft.Position);
            if (ShoulderRight.Confidence >= confidenceLevel && ElbowRight.Confidence >= confidenceLevel)
                g.DrawLine(p, ShoulderRight.Position, ElbowRight.Position);
            if (ElbowRight.Confidence >= confidenceLevel && WristRight.Confidence >= confidenceLevel)
                g.DrawLine(p, ElbowRight.Position, WristRight.Position);
            if (HipLeft.Confidence >= confidenceLevel && HipRight.Confidence >= confidenceLevel)
                g.DrawLine(p, HipLeft.Position, HipRight.Position);
            if (HipLeft.Confidence >= confidenceLevel && KneeLeft.Confidence >= confidenceLevel)
                g.DrawLine(p, HipLeft.Position, KneeLeft.Position);
            if (KneeLeft.Confidence >= confidenceLevel && AnkleLeft.Confidence >= confidenceLevel)
                g.DrawLine(p, KneeLeft.Position, AnkleLeft.Position);
            if (HipRight.Confidence >= confidenceLevel && KneeRight.Confidence >= confidenceLevel)
                g.DrawLine(p, HipRight.Position, KneeRight.Position);
            if (KneeRight.Confidence >= confidenceLevel && AnkleRight.Confidence >= confidenceLevel)
                g.DrawLine(p, KneeRight.Position, AnkleRight.Position);


            if (AnkleRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, AnkleRight.GetRectangle(diameter));
            if (AnkleLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, AnkleLeft.GetRectangle(diameter));
            if (KneeRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, KneeRight.GetRectangle(diameter));
            if (KneeLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, KneeLeft.GetRectangle(diameter));
            if (HipRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, HipRight.GetRectangle(diameter));
            if (HipLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, HipLeft.GetRectangle(diameter));
            if (WristRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, WristRight.GetRectangle(diameter));
            if (WristLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, WristLeft.GetRectangle(diameter));
            if (ElbowRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, ElbowRight.GetRectangle(diameter));
            if (ElbowLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, ElbowLeft.GetRectangle(diameter));
            if (ShoulderRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, ShoulderRight.GetRectangle(diameter));
            if (ShoulderLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, ShoulderLeft.GetRectangle(diameter));
            if (EarRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, EarRight.GetRectangle(diameter));
            if (EarLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, EarLeft.GetRectangle(diameter));
            if (EyeRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, EyeRight.GetRectangle(diameter));
            if (EyeLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, EyeLeft.GetRectangle(diameter));
            if (Nose.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.GreenYellow, Nose.GetRectangle(diameter));

            KeyPoint hip = Hip();
            if (hip.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.Violet, hip.GetRectangle(diameter - 2));

            KeyPoint shoulder = Shoulder();
            if (shoulder.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.Violet, shoulder.GetRectangle(diameter - 2));

            KeyPoint ear = Ear();
            if (ear.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.Violet, ear.GetRectangle(diameter - 2));

            KeyPoint eye = Eye();
            if (eye.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.Violet, eye.GetRectangle(diameter - 2));

            KeyPoint head = Head();
            if (head.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.Violet, head.GetRectangle(diameter - 2));

        }

        public override string ToString()
        {
            return $"{Nose.Confidence:0.00}, {EyeLeft.Confidence:0.00}, {EyeRight.Confidence:0.00}, " +
                   $"{EarLeft.Confidence:0.00}, {EarRight.Confidence:0.00}, {ShoulderLeft.Confidence:0.00}, " +
                   $"{ShoulderRight.Confidence:0.00}, {ElbowLeft.Confidence:0.00}, {ElbowRight.Confidence:0.00}, " +
                   $"{WristLeft.Confidence:0.00}, {WristRight.Confidence:0.00}, {HipLeft.Confidence:0.00}, " +
                   $"{HipRight.Confidence:0.00}, {KneeLeft.Confidence:0.00}, {KneeRight.Confidence:0.00}, " +
                   $"{AnkleLeft.Confidence:0.00}, {AnkleRight.Confidence:0.00}";
        }

    }

    public class KeyPoint
    {
        public float X;
        public float Y;
        public float Confidence;
        private int stride = 8400;

        public System.Drawing.Point Position { get { return new System.Drawing.Point((int)X, (int)Y); } }

        public Rectangle GetRectangle(float diameter = 12)
        {
            float radius = (diameter / 2.0f);
            return new Rectangle((int)(X - radius), (int)(Y - radius), (int)diameter, (int)diameter);
        }

        public KeyPoint(float[] output, int startIndex, int keyIndex)
        {
            this.X = output[startIndex + stride * (keyIndex * 3 + 5)];
            this.Y = output[startIndex + stride * (keyIndex * 3 + 6)];
            this.Confidence = output[startIndex + stride * (keyIndex * 3 + 7)];
        }

        public KeyPoint(float X, float Y, float Confidence)
        {
            this.X = X;
            this.Y = Y;
            this.Confidence = Confidence;
        }

        public KeyPoint()
        {
            this.X = 0;
            this.Y = 0;
            this.Confidence = 0;
        }

        public override string ToString()
        {
            return $"{X:0},{Y:0},{Confidence:0.00}";
        }

    }

}
