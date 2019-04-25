using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace BinLibrary.Hook.MouseHook
{
	public class MouseHook : INativedHook
	{
		private static List<MouseHook> m_MouseHooks;

		private int m_Hook;

		private int m_ThreadId;

		private HookProc m_HookProc;

		 
		public event WinHandler<MouseEventArgs> MouseDoubleClick;

		 
		public event WinHandler<MouseEventArgs> MouseMove;

		 
		public event WinHandler<MouseEventArgs> MouseDown;

		 
		public event WinHandler<MouseEventArgs> MouseUp;

		public static MouseHook Instance(int threadId = 0)
		{
			threadId = ((threadId == 0) ? HookNatived.CurrentThreadId() : threadId);
			MouseHook mouseHook = MouseHook.m_MouseHooks.Find((MouseHook m) => m.m_ThreadId == threadId);
			bool flag = mouseHook == null;
			bool flag2 = flag;
			MouseHook result;
			if (flag2)
			{
				MouseHook mouseHook2 = new MouseHook(threadId);
				MouseHook.m_MouseHooks.Add(mouseHook2);
				result = mouseHook2;
			}
			else
			{
				result = mouseHook;
			}
			return result;
		}

		public bool Install()
		{
			bool flag = this.m_Hook == 0;
			bool flag2 = flag;
			if (flag2)
			{
				this.m_Hook = HookNatived.InstallThreadHook(this.m_HookProc, HookType.WH_MOUSE, this.m_ThreadId);
			}
			return this.m_Hook != 0;
		}

		public bool Uninstall()
		{
			bool flag = HookNatived.UninstallHook(this.m_Hook);
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				MouseHook.m_MouseHooks.RemoveAll((MouseHook m) => m.m_ThreadId == this.m_ThreadId);
				this.m_Hook = 0;
			}
			return flag;
		}

		private int OnMouseDoubleClick(MouseButtons button, int clicks, int x, int y, int delta)
		{
			bool flag = this.MouseDoubleClick != null;
			bool flag2 = flag;
			int result;
			if (flag2)
			{
				result = this.MouseDoubleClick(this, new MouseEventArgs(button, clicks, x, y, delta));
			}
			else
			{
				result = 0;
			}
			return result;
		}

		private int OnMouseDown(MouseButtons button, int clicks, int x, int y, int delta)
		{
			bool flag = this.MouseDown != null;
			bool flag2 = flag;
			int result;
			if (flag2)
			{
				result = this.MouseDown(this, new MouseEventArgs(button, clicks, x, y, delta));
			}
			else
			{
				result = 0;
			}
			return result;
		}

		private int OnMouseUp(MouseButtons button, int clicks, int x, int y, int delta)
		{
			bool flag = this.MouseUp != null;
			bool flag2 = flag;
			int result;
			if (flag2)
			{
				result = this.MouseUp(this, new MouseEventArgs(button, clicks, x, y, delta));
			}
			else
			{
				result = 0;
			}
			return result;
		}

		private int OnMouseMove(MouseButtons button, int clicks, int x, int y, int delta)
		{
			bool flag = this.MouseMove != null;
			bool flag2 = flag;
			int result;
			if (flag2)
			{
				result = this.MouseMove(this, new MouseEventArgs(button, clicks, x, y, delta));
			}
			else
			{
				result = 0;
			}
			return result;
		}

		private int hookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			bool flag = nCode >= 0;
			bool flag2 = flag;
			int num;
			int result;
			if (flag2)
			{
				MouseMessage msg = (MouseMessage)((int)wParam);
				MSLLHOOKSTRUCT mSLLHOOKSTRUCT = lParam.ToStructure<MSLLHOOKSTRUCT>();
				switch (msg)
				{
				case MouseMessage.WM_MOUSEMOVE:
					num = this.OnMouseMove(MouseButtons.None, 0, mSLLHOOKSTRUCT.pt.x, mSLLHOOKSTRUCT.pt.y, mSLLHOOKSTRUCT.mouseData);
					result = num;
					return result;
				case MouseMessage.WM_LBUTTONDOWN:
				case MouseMessage.WM_RBUTTONDOWN:
				case MouseMessage.WM_MBUTTONDOWN:
				{
					MouseButtons button = msg.ToMouseButtons();
					num = this.OnMouseDown(button, 1, mSLLHOOKSTRUCT.pt.x, mSLLHOOKSTRUCT.pt.y, mSLLHOOKSTRUCT.mouseData);
					result = num;
					return result;
				}
				case MouseMessage.WM_LBUTTONUP:
				case MouseMessage.WM_RBUTTONUP:
				case MouseMessage.WM_MBUTTONUP:
				{
					MouseButtons button2 = msg.ToMouseButtons();
					num = this.OnMouseUp(button2, 1, mSLLHOOKSTRUCT.pt.x, mSLLHOOKSTRUCT.pt.y, mSLLHOOKSTRUCT.mouseData);
					result = num;
					return result;
				}
				case MouseMessage.WM_LBUTTONDBLCLK:
				case MouseMessage.WM_RBUTTONDBLCLK:
				case MouseMessage.WM_MBUTTONDBLCLK:
					num = this.OnMouseDoubleClick(msg.ToMouseButtons(), 1, mSLLHOOKSTRUCT.pt.x, mSLLHOOKSTRUCT.pt.y, mSLLHOOKSTRUCT.mouseData);
					result = num;
					return result;
				}
			}
			num = Hook.CallNextHookEx(this.m_Hook, nCode, wParam, lParam);
			result = num;
			return result;
		}

		static MouseHook()
		{
			MouseHook.m_MouseHooks = new List<MouseHook>();
		}

		private MouseHook(int threadId)
		{
			this.m_Hook = 0;
			this.m_ThreadId = threadId;
			this.m_HookProc = new HookProc(this.hookProc);
		}
	}
}
