using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BinLibrary.RevitHelper;
using BinLibrary.WinApi;

namespace BinLibrary.WinHelper
{
    public static class ReleaseButtonHelper
    {
        public static void Release()
        {
            WinApi.WinApi.SetForegroundWindow(RevitWindowHelper.GetRevitHandle());
            WinApi.WinApi.mouse_event((MouseKey)24, 0, 0, 0, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 0, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 2, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 0, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 2, 0);
        }
        public static void ReleaseOnce()
        {
            WinApi.WinApi.SetForegroundWindow(RevitWindowHelper.GetRevitHandle());
            WinApi.WinApi.mouse_event((MouseKey)24, 0, 0, 0, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 0, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 2, 0);
        }

        public static void EscDoublePress()
        {
            WinApi.WinApi.mouse_event((MouseKey)24, 0, 0, 0, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 0, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 2, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 0, 0);
            WinApi.WinApi.keybd_event(Keys.Escape, 0, 2, 0);
        }
    }
}
