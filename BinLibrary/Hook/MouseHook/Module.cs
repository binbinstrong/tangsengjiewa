using System;
using System.Runtime.InteropServices;

namespace BinLibrary.Hook.MouseHook
{
	public static class Module
	{
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll")]
		public static extern int GetCurrentThreadId();

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetCurrentProcess();

		[DllImport("kernel32.dll")]
		public static extern uint GetLastError();

		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string path);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetProcAddress(IntPtr lib, string funcName);

		[DllImport("kernel32.dll")]
		private static extern bool FreeLibrary(IntPtr lib);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll")]
		public static extern int VirtualAllocEx(IntPtr hwnd, int lpaddress, int size, int type, int tect);

		[DllImport("kernel32.dll")]
		public static extern int CreateRemoteThread(IntPtr hwnd, int attrib, int size, int address, int par, int flags, int threadid);

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetDllDirectory(string pathName);
	}
}
