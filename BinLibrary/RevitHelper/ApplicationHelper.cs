using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

namespace BinLibrary.RevitHelper
{
    public static class ApplicationHelper
    {
        /// <summary>
        /// 2016以上版本可以设置背景 以下版本不可以 
        /// 程序设置文件位置：C:\Users\XU\AppData\Roaming\Autodesk\Revit\Autodesk Revit 2016\Revit.ini 文件内 background = color 和 InvertBackground=0
        /// </summary>
        public static void SetBackGroundColor(this Document doc,Color color)
        {
            Application app = doc.Application;
            app.BackgroundColor = color;

        }
        
    }
}
