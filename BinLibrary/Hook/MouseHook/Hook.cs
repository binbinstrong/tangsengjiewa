using System;
using System.Runtime.InteropServices;

namespace BinLibrary.Hook.MouseHook
{
	public static class Hook
	{
		[DllImport("user32.dll")]
		public static extern int SetWindowsHookEx(HookType idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

		[DllImport("user32.dll")]
		public static extern bool UnhookWindowsHookEx(int idHook);

		[DllImport("user32.dll")]
		public static extern int CallNextHookEx(int idHook, int code, IntPtr wParam, IntPtr lParam);
	}
}
