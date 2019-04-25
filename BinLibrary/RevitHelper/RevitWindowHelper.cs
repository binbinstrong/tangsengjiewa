//======================================
//Copyright              2017
//CLR版本:               4.0.30319.42000
//机器名称:              XU-PC
//命名空间名称/文件名:   Techyard.Revit.Database/Class1
//创建人:                XU ZHAO BIN
//创建时间:              2017/12/10 22:31:43
//网址:                   
//======================================
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Windows;
using System.Windows.Forms;

namespace BinLibrary.RevitHelper
{
    public static class RevitWindowHelper
    {
        /// <summary>
        /// 获取revit窗体句柄
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetRevitHandle()
        {
            return Autodesk.Windows.ComponentManager.ApplicationWindow;
        }
        /// <summary>
        /// 获取revit窗体
        /// </summary>
        /// <returns></returns>
        public static System.Windows.Forms.Form GetRevitWindow()
        {
            return System.Windows.Forms.Control.FromHandle(GetRevitHandle()) as System.Windows.Forms.Form;
        }
        public static IWin32Window GetRevitWindow_win32()
        {
            return System.Windows.Forms.Control.FromHandle(GetRevitHandle()) as IWin32Window;
        }
    }

    public class WindowHandle : IWin32Window
    {
        public IntPtr Handle { get; }

        public WindowHandle(IntPtr h)
        {
            Handle = h;
        }
    }


}
