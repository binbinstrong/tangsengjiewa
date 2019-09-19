using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Windows;
namespace 唐僧揭瓦.BinLibrary.RevitHelper
{
    public static class RevitWindowHelper
    {
        public static IntPtr GetRevitHandle()
        {
            return Autodesk.Windows.ComponentManager.ApplicationWindow;
        }
    }
}
