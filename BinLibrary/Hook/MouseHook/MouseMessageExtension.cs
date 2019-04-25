using System;
using System.Windows.Forms;

namespace BinLibrary.Hook.MouseHook
{
	public static class MouseMessageExtension
	{
		public static MouseButtons ToMouseButtons(this MouseMessage msg)
		{
			return MouseMessageExtension.getMouseButtons(msg);
		}

		private static MouseButtons getMouseButtons(MouseMessage mouseMsg)
		{
			MouseButtons result = MouseButtons.None;
			switch (mouseMsg)
			{
			case MouseMessage.WM_LBUTTONDOWN:
			case MouseMessage.WM_LBUTTONUP:
			case MouseMessage.WM_LBUTTONDBLCLK:
				result = MouseButtons.Left;
				break;
			case MouseMessage.WM_RBUTTONDOWN:
			case MouseMessage.WM_RBUTTONUP:
			case MouseMessage.WM_RBUTTONDBLCLK:
				result = MouseButtons.Right;
				break;
			case MouseMessage.WM_MBUTTONDOWN:
			case MouseMessage.WM_MBUTTONUP:
			case MouseMessage.WM_MBUTTONDBLCLK:
				result = MouseButtons.Middle;
				break;
			}
			return result;
		}
	}
}
