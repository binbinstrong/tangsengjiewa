using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinLibrary.RevitExtension
{
    public static class TextNodeExtension
    {
        /// <summary>
        /// 获取文字边框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static List<Line> Boundaries(this TextNote text, View view)
        {
            var result = new List<Line>();
            var box = text.get_BoundingBox(view);
            var min = box.Min;
            var max = box.Max;
            var xmin = min.X;
            var xmax = max.X;
            var ymin = min.Y;
            var ymax = max.Y;
            var po1 = min;
            var po2 = new XYZ(xmax, ymin, 0);
            var po3 = max;
            var po4 = new XYZ(xmin, ymax, 0);
            var line1 = Line.CreateBound(po1, po2);
            var line2 = Line.CreateBound(po2, po3);
            var line3 = Line.CreateBound(po3, po4);
            var line4 = Line.CreateBound(po4, po1);
            result.Add(line1);
            result.Add(line2);
            result.Add(line3);
            result.Add(line4);
            return result;
        }
        /// <summary>
        /// 获取文字宽
        /// </summary>
        /// <param name="text"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static  double Width(this TextNote text, View view)
        {
            var result = default(double);
            var box = text.get_BoundingBox(view);
            var min = box.Min;
            var max = box.Max;
            var xmin = min.X;
            var xmax = max.X;
            var ymin = min.Y;
            var ymax = max.Y;
            result = Math.Abs(xmax - xmin);
            return result;
        }
        /// <summary>
        /// 获取文字高
        /// </summary>
        /// <param name="text"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static double Height(this TextNote text, View view)
        {
            var result = default(double);
            var box = text.get_BoundingBox(view);
            var min = box.Min;
            var max = box.Max;
            var xmin = min.X;
            var xmax = max.X;
            var ymin = min.Y;
            var ymax = max.Y;
            result = Math.Abs(ymax - ymin);
            return result;
        }
    }
}
