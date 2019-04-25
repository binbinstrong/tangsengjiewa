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
using System.Text;
using Autodesk.Revit.UI;

namespace BinLibrary.RevitExtension
{
    public static class UIDocumentExtension
    {
        public static UIView ActiveUiview(this UIDocument uidoc)
        {
            IList<UIView> uiviews = uidoc.GetOpenUIViews();
            foreach (UIView uv in uiviews)
            {
                if (uv.ViewId.IntegerValue==uidoc.ActiveView.Id.IntegerValue)
                {
                    return uv;
                    break;
                }
            }
              
            return null;
        }
    }
}
