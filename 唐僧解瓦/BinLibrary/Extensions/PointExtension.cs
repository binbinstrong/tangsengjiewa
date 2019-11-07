using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
 
namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class PointExtension
    {


        /// <summary>
        /// 投影到射线上
        /// </summary>
        /// <param name="po"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static XYZ ProjectToXLine(this XYZ po, Line l/*有限长度线段*/)
        {
            Line l1 = l.Clone() as Line;
            if (l1.IsBound)
            {
                l1.MakeUnbound();
            }
            return l1.Project(po).XYZPoint;
        }
        /// <summary>
        /// 浮点数相等时的精度
        /// </summary>
        private static double precision = 0.000001;

        /// <summary>
        /// 判断两double数值是否相等
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool IsEqual(double d1, double d2)
        {
            double diff = Math.Abs(d1 - d2);
            return diff < precision;

        }
        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="p"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static bool IsOnLine(this XYZ p, Line l)
        {
            XYZ end1 = l.GetEndPoint(0);
            XYZ end2 = l.GetEndPoint(1);

            XYZ vec_pToEnd1 = end1 - p;
            XYZ vec_pToEnd2 = end2 - p;

            double precision = 0.0000001d;

            if (p.DistanceTo(end1) < precision || p.DistanceTo(end2) < precision)
            {
                return true;
            }
            if (vec_pToEnd1.IsOppositeDirection(vec_pToEnd2))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断点是否在线 或线的延长线上
        /// </summary>
        /// <param name="p"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static bool IsXOnLine(this XYZ p, Line l)
        {
            double precision = 0.0000001d;
            var l1 = l.Clone() as Line;
            l1.MakeUnbound();
            if (p.DistanceTo(l1) < precision)
            {
                return true;
            }
            return false;
        }

        public static double DistanceTo(this XYZ p1, Line xline)
        {
            double result = double.NegativeInfinity;

            XYZ p1_onLine = p1.ProjectToXLine(xline);

            result = p1.DistanceTo(p1_onLine);

            return result;
        }

        

    }
}
