using System;

namespace BinLibrary.Hook.MouseHook
{
	public struct MSLLHOOKSTRUCT : IWINSTRUCT
	{
		public POINT pt;

		public int mouseData;

		public int flags;

		public uint wHitTestCode;

		public uint dwExtraInfo;
	}
}
