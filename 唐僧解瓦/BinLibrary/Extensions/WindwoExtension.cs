using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace 唐僧揭瓦.BinLibrary.Extensions
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
