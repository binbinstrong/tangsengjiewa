using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BinLibrary.Extensions
{
    public static class BitmapExtension
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            BitmapImage result = null;

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, bitmap.RawFormat);
                result.BeginInit();
                result.StreamSource = ms;
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.EndInit();
                result.Freeze();
                return result;
            }
            return result;
        }
        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            BitmapSource result = null;
            IntPtr handle = bitmap.GetHbitmap();
            result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty,
            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            return result;
        }
        public static Bitmap ToBitmap(this BitmapImage bitmapimage)
        {
            Bitmap result = null;
            byte[] bytearray = null;
            Stream smarket = bitmapimage.StreamSource;

            if (smarket != null && smarket.Length > 0)
            {
                smarket.Position = 0;
                using (BinaryReader br = new BinaryReader(smarket))
                {
                    bytearray = br.ReadBytes((int)smarket.Length);
                }
            }
            if(bytearray!=null && bytearray.Length!=0)
            {
                MemoryStream ms = new MemoryStream(bytearray);
                result = new Bitmap(ms);
            }
            
            return result;
        }
    }
}
