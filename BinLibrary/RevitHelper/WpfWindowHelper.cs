using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace BinLibrary.RevitHelper
{
    public static class WpfWindowHelper
    {
        public static WindowInteropHelper helper(this Window win)
        {
            return new WindowInteropHelper(win);
        }

        public static System.Windows.Interop.IWin32Window ToWin32Window_presentation(this Window win)
        {
            return System.Windows.Forms.Form.FromHandle(win.helper().Handle) as System.Windows.Interop.IWin32Window;
        }

        public static System.Windows.Forms.IWin32Window ToWini32Window(this Window win)
        {
            return System.Windows.Forms.Form.FromHandle(win.helper().Handle);
        }

    }
}
