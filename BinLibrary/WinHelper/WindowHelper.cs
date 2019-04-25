using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace BinLibrary.WinHelper
{
    public class WindowHelper
    {
        private IntPtr _parenthandle;
        private string _subControlClass;
        private string _subcontrolname;
        private IntPtr _mdisubwinHandle;
        private string _subwinTitle;
        private IntPtr _subwinHandle;

        /// <summary>
        /// 构造函数1寻找子控件
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="subWinName">要查找子窗体或子控件名称</param>
        public WindowHelper(IntPtr parentHandle, string subwinclass, string subWinName)
        {
            ParentHandle = parentHandle;
            Subcontrolname = subWinName;
            SubControlClass = subwinclass;
            FindMdiChidSubWinHwnd(parentHandle);
        }

        public WindowHelper(IntPtr parentHandle, string subwintitle)
        {
            _parenthandle = parentHandle;
            _subwinTitle = subwintitle;
            //FindDescendentWin(parentHandle, 1);
        }

        public WindowHelper()
        {
            
        }

        public IntPtr ParentHandle
        {
            get { return _parenthandle; }
            set { _parenthandle = value; }
        }

        public string Subcontrolname
        {
            get { return _subcontrolname; }
            set { _subcontrolname = value; }
        }

        public IntPtr MdisubwinHandle
        {
            get { return _mdisubwinHandle; }
            set { _mdisubwinHandle = value; }
        }

        public string SubControlClass
        {
            get { return _subControlClass; }
            set { _subControlClass = value; }
        }

        public string SubwinTitle
        {
            get { return _subwinTitle; }
            set { _subwinTitle = value; }
        }

        public IntPtr SubwinHandle
        {
            get { return _subwinHandle; }
            set { _subwinHandle = value; }
        }

        public List<WindowInfo> ChildWindowInfos
        {
            get { return _childWindowInfos; }
            set { _childWindowInfos = value; }
        }

        /// <summary>
        /// 寻找子控件 (有bug 如果找不到则进入死循环)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        bool FindMdiChidSubWinHwnd1(IntPtr parent, IntPtr lparam)
        {
            //parent = ParentHandle;
            WinApi.WinApi.EnumWindowProc proc = new WinApi.WinApi.EnumWindowProc(FindMdiChidSubWinHwnd);
            //MessageBox.Show("inProc::" + "first");
            IntPtr hwnd = WinApi.WinApi.FindWindowEx(parent, IntPtr.Zero, SubControlClass, Subcontrolname);
            if (hwnd != IntPtr.Zero)
            {
                //for test
                //MessageBox.Show("inProc::" + hwnd.ToString());

                _mdisubwinHandle = hwnd;
                return false;
            }
            else
            {
                //StringBuilder s = new StringBuilder();
                //WinApi.WinApi.GetWindowText(parent, s, s.Capacity);
                //var title = s.ToString();
                //MessageBox.Show($"title:{title};;parenthandle:{parent.ToString()};inProc::" + hwnd.ToString());

                WinApi.WinApi.EnumChildWindows(parent, proc, IntPtr.Zero);
                return true;
            }

        }

        /// <summary>
        /// 寻找子控件的回调方法 如果找不到则进入死循环
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        bool FindMdiChidSubWinHwnd(IntPtr parent, IntPtr lparam)
        {
            bool result = false;
            //parent = ParentHandle;
            WinApi.WinApi.EnumWindowProc proc = new WinApi.WinApi.EnumWindowProc(FindMdiChidSubWinHwnd);
            //MessageBox.Show("inProc::" + "first");
            IntPtr hwnd = WinApi.WinApi.FindWindowEx(parent, IntPtr.Zero, SubControlClass, Subcontrolname);

            //MessageBox.Show("inProc::" + hwnd.ToString());

            if (hwnd != IntPtr.Zero)
            {
                //for test
                //MessageBox.Show("inProc2222222::" + hwnd.ToString());

                _mdisubwinHandle = hwnd;
                result = false;
            }
            else
            {
                //StringBuilder s = new StringBuilder();
                //WinApi.WinApi.GetWindowText(parent, s, s.Capacity);
                //var title = s.ToString();
                //MessageBox.Show($"title:{title};;parenthandle:{parent.ToString()};inProc::" + hwnd.ToString());

                WinApi.WinApi.EnumChildWindows(parent, proc, IntPtr.Zero);
                result = true;
            }
            return result;
        }

        void FindMdiChidSubWinHwnd(IntPtr parent)
        {
            var virtualResult = FindMdiChidSubWinHwnd(parent, IntPtr.Zero);
        }

        bool FindDescendentWin(IntPtr parent, int level)
        {
            IntPtr child = IntPtr.Zero;
            StringBuilder s = new StringBuilder();
            string text = "";

            do
            {
                child = WinApi.WinApi.FindWindowEx(parent, IntPtr.Zero, null, SubwinTitle);
                WinApi.WinApi.GetWindowText(child, s, s.Capacity);
                text = s.ToString();

                MessageBox.Show("finddescendent:" + text);

                if (text == _subwinTitle)
                {
                    _subwinHandle = child;
                    return true;
                }
                else
                {
                    //if (child != IntPtr.Zero)
                    {
                        FindDescendentWin(child, 1);
                    }
                }
            } while (child != IntPtr.Zero);

            return false;
        }

        bool FindDescendentWin(IntPtr parent, string subwintitle)
        {
            IntPtr child = IntPtr.Zero;
            StringBuilder s = new StringBuilder();
            string text = "";
            
            child = WinApi.WinApi.FindWindowEx(parent, IntPtr.Zero, null, SubwinTitle);

            if (child != IntPtr.Zero)
            {
                SubwinHandle = child;
                return true;
            }
            else
            {
                return false;
            }
        }




        public struct WindowInfo
        {
            public IntPtr hWnd;
            public string szWindowName;
            public string szClassName;
        }

        private List<WindowInfo> _childWindowInfos = new List<WindowInfo>();

        /// <summary>
        /// 暂时没用到 在enumchildwindows里面直接调用了匿名委托
        /// </summary>
        /// <param name="childHwnd"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool enumProc(IntPtr childHwnd, IntPtr handle)
        {
            StringBuilder s = new StringBuilder();
            WindowInfo wininfo = new WindowInfo();

            wininfo.hWnd = childHwnd;
            WinApi.WinApi.GetWindowText(childHwnd, s, s.Capacity);

            wininfo.szWindowName = s.ToString();

            WinApi.WinApi.GetClassNameW(childHwnd, s, s.Capacity);
            wininfo.szClassName = s.ToString();

            _childWindowInfos.Add(wininfo);

            return true;
        }

        public WindowInfo[] GetAllChidWindows(IntPtr parent, IntPtr lparam)
        {
            //用来保存窗口对象 列表
            List<WindowInfo> wndList = new List<WindowInfo>();

            //enum all desktop windows 
            WinApi.WinApi.EnumChildWindows(parent, delegate (IntPtr hWnd, IntPtr lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);

                //get hwnd 
                wnd.hWnd = hWnd;

                //get window name  
                WinApi.WinApi.GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();

                //get window class 
                WinApi.WinApi.GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();

                //add it into list 
                wndList.Add(wnd);
                return true;
            }, lparam);

            return wndList.ToArray();
        }
    }
}