using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace BinLibrary.WinHelper
{
    public static class RectangleHelper
    {
        /// <summary>
        /// 矩形框包含点
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Contains(this Rectangle rec , Point p)
        {
            if (p.X > rec.Left && p.X < rec.Right && p.Y < rec.Bottom && p.Y > rec.Top)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 两个rectangle相交 rectangle
        /// </summary>
        /// <param name="rec1"></param>
        /// <param name="rec2"></param>
        /// <param name="boundaryDis"></param>
        /// <returns></returns>
        public static Rectangle Intersect(this Rectangle rec1, Rectangle rec2,int boundaryDis = 0)
        {
            int[] tops = new int[] { rec1.Top + boundaryDis, rec2.Top + boundaryDis };
            int[] lefts = new int[] { rec1.Left + boundaryDis, rec2.Left + boundaryDis };
            int[] bottoms = new int[] { rec1.Bottom - boundaryDis, rec2.Bottom - boundaryDis };
            int[] rights = new int[] { rec1.Right - boundaryDis, rec2.Right - boundaryDis };

            return new Rectangle(lefts.Max(), tops.Max(), rights.Min(), bottoms.Min());
        }
    }
}
