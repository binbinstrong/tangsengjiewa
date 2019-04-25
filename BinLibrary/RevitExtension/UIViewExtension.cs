using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using View = Autodesk.Revit.DB.View;
using Point = System.Drawing.Point;

namespace BinLibrary.RevitExtension
{
    public static class UIViewExtension
    {
        public static UIView ActiveUiview(this Document doc)
        {
            UIView result = null;
            UIDocument uIDocument = new UIDocument(doc);
            View activeView = doc.ActiveView;
            foreach (UIView current in uIDocument.GetOpenUIViews())
            {
                bool flag = current.ViewId == activeView.Id;
                if (flag)
                {
                    result = current;
                }
            }
            return result;
        }

        public static bool IsCursorIn(this UIView uiview)
        {
            Point position = Cursor.Position;
            return uiview.IsPointIn(position);
        }

        public static bool IsPointIn(this UIView uiview, Point point)
        {
            Rectangle windowRectangle = uiview.GetWindowRectangle();
            int left = windowRectangle.Left;
            int right = windowRectangle.Right;
            int top = windowRectangle.Top;
            int bottom = windowRectangle.Bottom;
            return point.X > left && point.X < right && point.Y > top && point.Y < bottom;
        }
    }
}
