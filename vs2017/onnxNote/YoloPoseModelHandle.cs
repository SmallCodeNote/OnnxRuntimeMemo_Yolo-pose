using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;


namespace YoloPoseOnnxHandle
{
    class YoloPoseModelHandle : IDisposable
    {
        public Tensor<float> ImageTensor;
        public PoseInfoSet poseInfoSet;
        public int modelOutputStride = 8400;

        InferenceSession session;
        public string sessionInputName = "";

        public YoloPoseModelHandle(string modelfilePath)
        {
            if (File.Exists(modelfilePath))
            {
                if (session != null) session.Dispose();
                session = new InferenceSession(modelfilePath);
                sessionInputName = session.InputMetadata.Keys.First();
            }
        }

        public void Dispose()
        {
            if (session != null) session.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool setModel(string modelfilePath)
        {
            if (File.Exists(modelfilePath))
            {
                if (session != null) session.Dispose();
                session = new InferenceSession(modelfilePath);
                sessionInputName = session.InputMetadata.Keys.First();
                return true;
            }
            return false;
        }

        public void Predicte(Bitmap bitmap)
        {
            ImageTensor = ConvertBitmapToTensor(bitmap);

            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(sessionInputName, ImageTensor) };
            var results = session.Run(inputs);
            var output = results.First().AsEnumerable<float>().ToArray();

            poseInfoSet = new PoseInfoSet(output);

            results.Dispose();

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
    }

    public class PoseInfoSet
    {
        public List<PoseInfo> PoseInfos;
        public int stride = 8400;

        public PoseInfoSet(float[] outputArray, float confidenceThreshold = 0.8f)
        {
            PoseInfos = new List<PoseInfo>();
            for (int i = 0; i < stride; i++)
            {
                PoseInfo pi = new PoseInfo(outputArray, i);
                if (pi.Bbox.Confidence >= confidenceThreshold)
                {
                    PoseInfos.Add(pi);
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
        public int stride = 8400;

        public Bbox(float[] outputArray, int startIndex)
        {
            this.Center_x = outputArray[startIndex + stride * 0];
            this.Center_y = outputArray[startIndex + stride * 1];
            this.Width = outputArray[startIndex + stride * 2];
            this.Height = outputArray[startIndex + stride * 3];
            this.Confidence = outputArray[startIndex + stride * 4];
        }
    }

    public class PoseKeyPoints
    {
        public KeyPoint Nose;
        public KeyPoint LeftEye;
        public KeyPoint RightEye;
        public KeyPoint LeftEar;
        public KeyPoint RightEar;
        public KeyPoint LeftShoulder;
        public KeyPoint RightShoulder;
        public KeyPoint LeftElbow;
        public KeyPoint RightElbow;
        public KeyPoint LeftWrist;
        public KeyPoint RightWrist;
        public KeyPoint LeftHip;
        public KeyPoint RightHip;
        public KeyPoint LeftKnee;
        public KeyPoint RightKnee;
        public KeyPoint LeftAnkle;
        public KeyPoint RightAnkle;

        public PoseKeyPoints(float[] outputArray, int startIndex)
        {
            Nose = new KeyPoint(outputArray, startIndex, 0);
            LeftEye = new KeyPoint(outputArray, startIndex, 1);
            RightEye = new KeyPoint(outputArray, startIndex, 2);
            LeftEar = new KeyPoint(outputArray, startIndex, 3);
            RightEar = new KeyPoint(outputArray, startIndex, 4);
            LeftShoulder = new KeyPoint(outputArray, startIndex, 5);
            RightShoulder = new KeyPoint(outputArray, startIndex, 6);
            LeftElbow = new KeyPoint(outputArray, startIndex, 7);
            RightElbow = new KeyPoint(outputArray, startIndex, 8);
            LeftWrist = new KeyPoint(outputArray, startIndex, 9);
            RightWrist = new KeyPoint(outputArray, startIndex, 10);
            LeftHip = new KeyPoint(outputArray, startIndex, 11);
            RightHip = new KeyPoint(outputArray, startIndex, 12);
            LeftKnee = new KeyPoint(outputArray, startIndex, 13);
            RightKnee = new KeyPoint(outputArray, startIndex, 14);
            LeftAnkle = new KeyPoint(outputArray, startIndex, 15);
            RightAnkle = new KeyPoint(outputArray, startIndex, 16);
        }
    }

    public class KeyPoint
    {
        public float X;
        public float Y;
        public float Confidence;
        public int stride = 8400;

        public KeyPoint(float[] output, int startIndex, int keyIndex)
        {
            this.X = output[startIndex + stride * (keyIndex * 3 + 5)];
            this.Y = output[startIndex + stride * (keyIndex * 3 + 6)];
            this.Confidence = output[startIndex + stride * (keyIndex * 3 + 7)];
        }
    }

}
