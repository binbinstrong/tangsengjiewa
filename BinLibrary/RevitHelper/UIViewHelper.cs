using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BinLibrary.RevitExtension;

namespace BinLibrary.RevitHelper
{
    public static class UIViewHelper
    {
        public static UIView GetLastUIView(this UIDocument uidoc)
        {
            UIView result = null;
            var uiviewList = uidoc.GetOpenUIViews();
            

            if (uiviewList.Count >= 2) 
            result = uiviewList.ElementAt(1);
            //var tem = string.Join("\n", uiviewList.Select(m => m.ViewId.GetElement(uidoc.Document).Name));
            //MessageBox.Show(tem);

            return result;
        }

        /// <summary>
        /// 转到上个视图
        /// </summary>
        /// <param name="uidoc"></param>
        /// <returns></returns>
        public static bool TurnToLastView(this UIDocument uidoc)
        {
            UIView lastuiview = uidoc.GetLastUIView();
            if (lastuiview == null)
            {
                return false;
            }
            View lastview = lastuiview.ViewId.GetElement(uidoc.Document) as View;
            if (lastview != null)
            {
                uidoc.ActiveView = lastview;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uidoc"></param>
        /// <returns></returns>
        //public static View LastView(this UIDocument uidoc)
        //{
        //    return uidoc.Document.
        //}


    }
}
