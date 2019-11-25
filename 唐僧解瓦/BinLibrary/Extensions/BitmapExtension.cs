using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class BitmapExtension
    {
        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            BitmapSource result = null;
            IntPtr handle = bitmap.GetHbitmap();
            result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            return result;
        }
    }
}
