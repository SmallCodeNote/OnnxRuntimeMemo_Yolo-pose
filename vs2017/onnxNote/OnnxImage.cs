using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Microsoft.ML.OnnxRuntime.Tensors;


namespace onnxNote
{
    class OnnxImage
    {
        public Bitmap srcBitmap;
        public int srcWidth;
        public int srcHeight;
        public int resizedWidth = 224;
        public int resizedHeight = 224;


        DenseTensor<float> Image;

        public Tensor<float> ConvertBitmapToTensor(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            var tensor = new DenseTensor<float>(new[] { 1, 3, height, width });

            // Bitmapのピクセル値をTensorに設定
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    tensor[0, 0, y, x] = color.R / 255.0f;
                    tensor[0, 1, y, x] = color.G / 255.0f;
                    tensor[0, 2, y, x] = color.B / 255.0f;
                }
            }
            return tensor;

        }

        public class Prediction
        {
            public Box Box { get; set; }
            public string Label { get; set; }
            public float Confidence { get; set; }
        }

        public class Box
        {
            public float Xmin { get; set; }
            public float Ymin { get; set; }
            public float Xmax { get; set; }
            public float Ymax { get; set; }

            public Box(float xmin, float ymin, float xmax, float ymax)
            {
                Xmin = xmin;
                Ymin = ymin;
                Xmax = xmax;
                Ymax = ymax;

            }
        }



    }
}
