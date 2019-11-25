using System.Windows;
using System.Windows.Interop;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class WindwoExtension
    {
        public static WindowInteropHelper helper(this Window win)
        {
            var helper = new WindowInteropHelper(win);
            return helper;
        }
    }
}
