using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Media;
using System.IO;
using Encoder = System.Drawing.Imaging.Encoder;

namespace BinLibrary.WinHelper
{
    public static class ImageHelper
    {
        public static void ChangePictureSize(string picturePath, string outPicturePath, int outLength, int outWide)
        {
            Bitmap b = new Bitmap(picturePath);
            Bitmap thumbnail = ImageHelper.GetThumbnail(b, outLength, outWide);
            ImageHelper.SetFilePath(outPicturePath);
            thumbnail.Save(outPicturePath);
        }

        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            ImageFormat rawFormat = b.RawFormat;
            int width = b.Width;
            int height = b.Height;
            bool flag = height > destHeight || width > destWidth;
            int num;
            int num2;
            if (flag)
            {
                bool flag2 = width * destHeight > height * destWidth;
                if (flag2)
                {
                    num = destWidth;
                    num2 = destWidth * height / width;
                }
                else
                {
                    num2 = destHeight;
                    num = width * destHeight / height;
                }
            }
            else
            {
                num = width;
                num2 = height;
            }
            Bitmap bitmap = new Bitmap(destWidth, destHeight);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Transparent);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(b, new Rectangle((destWidth - num) / 2, (destHeight - num2) / 2, num, num2), 0, 0, b.Width, b.Height, GraphicsUnit.Pixel);
            graphics.Dispose();
            EncoderParameters encoderParameters = new EncoderParameters();
            long[] value = new long[]
            {
                100L
            };
            EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, value);
            encoderParameters.Param[0] = encoderParameter;
            b.Dispose();
            return bitmap;
        }

        public static bool SetFilePath(string path)
        {
            bool flag = File.Exists(path);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !Directory.Exists(Path.GetDirectoryName(path));
                if (flag2)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                result = true;
            }
            return result;
        }
    }
}
