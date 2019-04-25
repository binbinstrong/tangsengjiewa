using System;
using System.Runtime.InteropServices;

namespace BinLibrary.Hook.MouseHook
{
	public static class IntPtrExtension
	{
		public static bool IsEqual(this IntPtr t, KeyboardMessage msg)
		{
			return t.Equals((IntPtr)((int)msg));
		}

		public static T ToStructure<T>(this IntPtr t) where T : IWINSTRUCT
		{
			return (T)((object)Marshal.PtrToStructure(t, typeof(T)));
		}
	}
}
