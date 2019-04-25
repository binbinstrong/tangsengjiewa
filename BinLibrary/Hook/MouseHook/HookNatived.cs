using System;
using System.Diagnostics;

namespace BinLibrary.Hook.MouseHook
{
	public class HookNatived
	{
		public static int InstallGlobalHook(HookProc hookProc, HookType hookType, IntPtr intptr)
		{
			return Hook.SetWindowsHookEx(hookType, hookProc, intptr, 0);
		}

		public static int InstallThreadHook(HookProc hooKProc, HookType hookType, int threadId)
		{
			return Hook.SetWindowsHookEx(hookType, hooKProc, IntPtr.Zero, threadId);
		}

		public static IntPtr CurrentProgressId()
		{
			string moduleName = Process.GetCurrentProcess().MainModule.ModuleName;
			return Module.GetModuleHandle(moduleName);
		}

		public static int CurrentThreadId()
		{
			return Module.GetCurrentThreadId();
		}

		public static bool UninstallHook(int mHook)
		{
			bool result;
			try
			{
				bool flag = mHook != 0;
				bool flag2 = flag;
				result = (flag2 && Hook.UnhookWindowsHookEx(mHook));
			}
			catch (Exception var_3_1E)
			{
				result = false;
			}
			return result;
		}
	}
}
