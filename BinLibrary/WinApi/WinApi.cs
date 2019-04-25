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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BinLibrary.WinApi
{
    public static class WinApi
    {
        #region Window类
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        internal static extern IntPtr FindWindow(string ClassName, string WindowNameW);

        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr Iparam);

        internal delegate bool EnumWindowProc(IntPtr hwnd, IntPtr parameter);//回调函数

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        internal static extern IntPtr FindWindowEx(IntPtr hwndparent, IntPtr hwndchildafter, string lpszclass,
            string lpszwindow);

        [DllImport("user32.dll")]
        internal static extern bool SetForGroundWindow(IntPtr hwnd);
         
        [DllImport("user32.dll")]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);


        public delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        //用来遍历所有窗口 
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        //获取窗口Text 
        [DllImport("user32.dll")]
        public static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        //获取窗口类名 
        [DllImport("user32.dll")]
        public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount); 
         
         

        #endregion

        #region 线程类
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        #endregion

        #region 消息类
        [DllImport("user32.dll")]
        internal static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wparam, int iparam);

        internal const int WM_CLICK = 0x00f5;

        #endregion

        #region Keyboard类
        //第一个为按键的虚拟键值，如回车键为vk_return, tab键为vk_tab（其他具体的参见附录：常用模拟键的键值对照表）；
        //第二个参数为扫描码，一般不用设置，用0代替就行；
        //第三个参数为选项标志，如果为keydown则置0即可，如果为keyup则设成2 "KEYEVENTF_KEYUP"；
        //第四个参数一般也是置0即可。

        //例子1：模拟按下'A'键
        //keybd_event(65,0,0,0);   //A按下时第三个参数为0
        //keybd_event(65,0,KEYEVENTF_KEYUP,0);

        //第一个值为虚拟键值，第二个参数为扫描不设置为0，第三个数为按键状态选项 keydown为0，如果为keyup 则设置成2，KEYEVENT_KEYUP,第四个参数一般为0

        [DllImport("user32.dll")]
        //public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        //public static extern void keybd_event(KeyBoard bVk, byte bScan, KeyUpOrDown dwFlags, int dwExtraInfo);
        public static extern void keybd_event(Keys bVk, byte bScan, int dwFlags, int dwExtraInfo);
        //public static extern void keybd_event(Keys bVk, byte bScan, KeyUpOrDown dwFlags, int dwExtraInfo);

        #endregion

        #region Mouse类
        // 例子      1、这里是鼠标左键按下和松开两个事件的组合即一次单击： 
        // 例子   mouse_event (MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0 )

        [DllImport("user32.dll")]
        //public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public static extern void mouse_event(MouseKey dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //++参数 意义 
        //dwFlags Long，下表中标志之一或它们的组合 
        //dx，dy Long，根据MOUSEEVENTF_ABSOLUTE标志，指定x，y方向的绝对位置或相对位置 
        //cButtons Long，没有使用 
        //dwExtraInfo Long，没有使用

        //++dwFlags常数 意义

        //const int MOUSEEVENTF_MOVE = 0x0001;      移动鼠标 
        //const int MOUSEEVENTF_LEFTDOWN = 0x0002; 模拟鼠标左键按下 
        //const int MOUSEEVENTF_LEFTUP = 0x0004; 模拟鼠标左键抬起 
        //const int MOUSEEVENTF_RIGHTDOWN = 0x0008; 模拟鼠标右键按下 
        //const int MOUSEEVENTF_RIGHTUP = 0x0010; 模拟鼠标右键抬起 
        //const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; 模拟鼠标中键按下 
        //const int MOUSEEVENTF_MIDDLEUP = 0x0040; 模拟鼠标中键抬起 
        //const int MOUSEEVENTF_ABSOLUTE = 0x8000; 标示是否采用绝对坐标
        #endregion

        #region 文件类
        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            public char szDisplayName;
            public char szTypeName;
        }

        #endregion
    }
    /// <summary>
    /// 鼠标按下或弹起动作
    /// </summary>
    public enum KeyUpOrDown
    {
        KEYEVENT_KEYDOWN = 0,
        KEYEVENT_KEYUP = 2,
    }
    /// <summary>
    /// 鼠标按键
    /// </summary>
    public enum MouseKey : int
    {
        MOUSEEVENTF_MOVE = 0x0001,       // 移动鼠标 
        MOUSEEVENTF_LEFTDOWN = 0x0002,   // 模拟鼠标左键按下 
        MOUSEEVENTF_LEFTUP = 0x0004,     // 模拟鼠标左键抬起 
        MOUSEEVENTF_RIGHTDOWN = 0x0008,  // 模拟鼠标右键按下 
        MOUSEEVENTF_RIGHTUP = 0x0010,    // 模拟鼠标右键抬起 
        MOUSEEVENTF_MIDDLEDOWN = 0x0020, // 模拟鼠标中键按下 
        MOUSEEVENTF_MIDDLEUP = 0x0040,   // 模拟鼠标中键抬起 
        MOUSEEVENTF_ABSOLUTE = 0x8000,   // 标示是否采用绝对坐标
    }
    /// <summary>
    /// 键盘码枚举
    /// </summary>
    public enum KeyBoard : int
    {
        //键　　 键码　  　键　　 键码　　　 键　　 键码 　　  键　　　　键码 
        Key_A = 65,
        Key_B = 66,
        Key_C = 67,
        Key_D = 68,
        Key_E = 69,
        Key_F = 70,
        Key_G = 71,
        Key_H = 72,
        Key_I = 73,
        Key_J = 74,
        Key_K = 75,
        Key_L = 76,
        Key_M = 77,
        Key_N = 78,
        Key_O = 79,
        Key_P = 80,
        Key_Q = 81,
        Key_R = 82,
        Key_S = 83,
        Key_T = 84,
        Key_U = 85,
        Key_V = 86,
        Key_W = 87,
        Key_X = 88,
        Key_Y = 89,
        Key_Z = 90,
        Key_0 = 48,
        Key_1 = 49,
        Key_2 = 50,
        Key_3 = 51,
        Key_4 = 52,
        Key_5 = 53,
        Key_6 = 54,
        Key_7 = 55,
        Key_8 = 56,
        Key_9 = 57,
        Key_0n = 96,
        Key_1n = 97,
        Key_2n = 98,     //小键盘
        Key_3n = 99,     //小键盘
        Key_4n = 100,    //小键盘
        Key_5n = 101,    //小键盘
        Key_6n = 102,    //小键盘
        Key_7n = 103,    //小键盘
        Key_8n = 104,    //小键盘
        Key_9n = 105,    //小键盘
        Key_mutiple = 106,//乘以   //小键盘
        Key_add = 107,//加         //小键盘
        Key_Entern = 108,          //小键盘
        Key_minus = 109,//减       //小键盘
        Key_dot = 110,//点         //小键盘
        Key_divide = 111,//除      //小键盘

        Key_F1 = 112,
        Key_F2 = 113,
        Key_F3 = 114,
        Key_F4 = 115,
        Key_F5 = 116,
        Key_F6 = 117,
        Key_F7 = 118,
        Key_F8 = 119,
        Key_F9 = 120,
        Key_F10 = 121,
        Key_F11 = 122,
        Key_F12 = 123,
        Key_Backspace = 8,
        Key_Tab = 9,
        Key_Clear = 12,
        Key_Enter = 13,
        Key_Shift = 16,
        Key_Control = 17,
        Key_Alt = 18,
        Key_CapsLock = 20,
        Key_Esc = 27,
        Key_Spacebar = 32,
        Key_PageUp = 33,
        Key_PageDown = 34,
        Key_end = 35,
        Key_Home = 36,
        Key_LeftArrow = 37,
        Key_UpArrow = 38,
        Key_RightArrow = 39,
        Key_DownArrow = 40,
        Key_Insert = 45,
        Key_Delete = 46,
        Key_Help = 47,
        Key_NumLock = 144,
    }

}
