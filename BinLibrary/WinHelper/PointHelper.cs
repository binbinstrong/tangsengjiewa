using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Point = System.Drawing.Point;

namespace BinLibrary.WinHelper
{
    public static class PointHelper
    {
        /// <summary>
        /// 转为AUTODESK.REVIT.DB.XYZ
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static XYZ ToRvtXYZ(this Point point)
        {
            return new XYZ(point.X, point.Y, 0);
        }

        /// <summary>
        /// 转为SYSTEM.DRAWING.POINT
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Point ToDrawingPoint(this XYZ position)
        {
            return new Point((int) position.X, (int) position.Y);
        }

    }
}
