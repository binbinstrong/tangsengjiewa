using Autodesk.Windows;
using System;
using System.Runtime.InteropServices;

namespace BinLibrary.RevitExtension
{
	public class Assist
	{
		internal delegate bool EnumWindowProc(IntPtr hwnd, IntPtr parameter);

		internal const int WM_CLICK = 245;

		internal static IntPtr finish;

		[DllImport("user32.dll")]
		internal static extern IntPtr FindWindow(string ClassName, string WindowNameW);

		[DllImport("user32.dll")]
		internal static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wparam, int iparam);

		[DllImport("user32.dll")]
		internal static extern bool EnumChildWindows(IntPtr window, Assist.EnumWindowProc callback, IntPtr Iparam);

		[DllImport("user32.dll")]
		internal static extern IntPtr FindWindowEx(IntPtr hwndparent, IntPtr hwndchildafter, string lpszclass, string lpszwindow);

		[DllImport("user32.dll")]
		internal static extern bool SetForGroundWindow(IntPtr hwnd);

		public static void Ok()
		{
			IntPtr applicationWindow = ComponentManager.ApplicationWindow;
			bool flag = applicationWindow != IntPtr.Zero;
			if (flag)
			{
				Assist.FindChildClassHwnd(applicationWindow, IntPtr.Zero);
				Assist.SendMessage(Assist.finish, 245, IntPtr.Zero, 0);
			}
		}
        /// <summary>
        /// 寻找子控件(方法有bug，如果找不到就进入死循环)
        /// </summary>
        /// <param name="hwndparent"></param>
        /// <param name="Iparam"></param>
        /// <returns></returns>
		internal static bool FindChildClassHwnd(IntPtr hwndparent, IntPtr Iparam)
		{
			Assist.EnumWindowProc callback = new Assist.EnumWindowProc(Assist.FindChildClassHwnd);
			IntPtr value = Assist.FindWindowEx(hwndparent, IntPtr.Zero, "Button", "完成");
			bool flag = value != IntPtr.Zero;
			bool result;
			if (flag)
			{
				Assist.finish = value;
				result = false;
			}
			else
			{
				Assist.EnumChildWindows(hwndparent, callback, IntPtr.Zero);
				result = true;
			}
			return result;
		}
	}
}
