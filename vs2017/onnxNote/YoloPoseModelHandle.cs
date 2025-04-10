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
        public string SessionInputName = "";

        public Tensor<float> ImageTensor;
        public List<PoseInfo> PoseInfos;
        private InferenceSession session;
        private int modelOutputStride = 8400;

        public YoloPoseModelHandle(string modelfilePath)
        {
            if (File.Exists(modelfilePath))
            {
                if (session != null) session.Dispose();
                session = new InferenceSession(modelfilePath);
                SessionInputName = session.InputMetadata.Keys.First();
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
                SessionInputName = session.InputMetadata.Keys.First();
                return true;
            }
            return false;
        }

        public void Predicte(Bitmap bitmap)
        {
            ImageTensor = ConvertBitmapToTensor(bitmap);

            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(SessionInputName, ImageTensor) };
            var results = session.Run(inputs);
            var output = results.First().AsEnumerable<float>().ToArray();

            PoseInfoSet(output);

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

        public override string ToString()
        {
            return SessionInputName;
        }

        public void PoseInfoSet(float[] outputArray, float confidenceThreshold = 0.6f)
        {
            PoseInfos = new List<PoseInfo>();
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
        }

        public void drawBBoxs(Graphics g)
        {
            if (g != null)
            {
                foreach (var info in PoseInfos)
                {
                    g.DrawRectangle(Pens.Blue, info.Bbox.Rectangle);
                }
            }
        }

        public void drawBones(Graphics g)
        {
            if (g != null)
            {
                foreach (var info in PoseInfos)
                {
                    info.KeyPoints.drawBone(g);
                }
            }
        }
    }


    public class PoseInfoSet
    {
        public List<PoseInfo> PoseInfos;
        private int stride = 8400;

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


        public void drawBone(Graphics g, float confidenceLevel = 0.6f)
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
                g.FillEllipse(Brushes.LightPink, AnkleRight.GetRectangle());
            if (AnkleLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, AnkleLeft.GetRectangle());
            if (KneeRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, KneeRight.GetRectangle());
            if (KneeLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, KneeLeft.GetRectangle());
            if (HipRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, HipRight.GetRectangle());
            if (HipLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, HipLeft.GetRectangle());
            if (WristRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, WristRight.GetRectangle());
            if (WristLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, WristLeft.GetRectangle());
            if (ElbowRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, ElbowRight.GetRectangle());
            if (ElbowLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, ElbowLeft.GetRectangle());
            if (ShoulderRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, ShoulderRight.GetRectangle());
            if (ShoulderLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, ShoulderLeft.GetRectangle());
            if (EarRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, EarRight.GetRectangle());
            if (EarLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, EarLeft.GetRectangle());
            if (EyeRight.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightPink, EyeRight.GetRectangle());
            if (EyeLeft.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.LightBlue, EyeLeft.GetRectangle());
            if (Nose.Confidence >= confidenceLevel)
                g.FillEllipse(Brushes.GreenYellow, Nose.GetRectangle());

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

        public Point Position { get { return new Point((int)X, (int)Y); } }

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
    }

}
