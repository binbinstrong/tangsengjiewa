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
using Autodesk.Revit.DB;

namespace BinLibrary.RevitExtension
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
        /// 判断垂直
        /// </summary>
        /// <param name="dir1"></param>
        /// <param name="dir2"></param>
        /// <returns></returns>
        public static bool IsPerpendicular(this XYZ dir1, XYZ dir2)
        {
            bool result = false;

            double dotproduct = dir1.Normalize().DotProduct(dir2.Normalize());

            if (Math.Abs(dotproduct) < precision)
            {
                result = true;
            }

            return result;
        }
        /// <summary>
        /// 点投射到平面上 返回投影点
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static XYZ Project(this XYZ p1, Plane p)
        {
            XYZ result = null;

            XYZ origin = p.Origin;
            XYZ dir = p.Normal;

            XYZ vector = origin - p1;

            double dotproduct = vector.DotProduct(dir);

            result = p1 + dir * dotproduct;

            return result;
        }

        /// <summary>
        /// 计算点到面的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double DistanceTo(this XYZ p1, Plane p)
        {
            double result = Double.NegativeInfinity;

            XYZ p1_onPlane = p1.Project(p);

            result = p1.DistanceTo(p1_onPlane);

            return result;
        }

        /// <summary>
        /// 计算点到直线的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="xline"></param>
        /// <returns></returns>
        public static double DistanceTo(this XYZ p1, Line xline)
        {
            double result = double.NegativeInfinity;

            XYZ p1_onLine = p1.ProjectToXLine(xline);

            result = p1.DistanceTo(p1_onLine);

            return result;
        }

        /// <summary>
        /// 返回点到曲面的距离 如果找不到投影点 则返回-1
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static double DistanceTo(this XYZ p1, Face f)
        {

            double result = -1;
            IntersectionResult intersection = f.Project(p1);

            if (intersection==null)
            {
                return result;
            }

            result = intersection.XYZPoint.DistanceTo(p1);
            return result;
        }

        /// <summary>
        /// 测试用
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double DistanceTo(this XYZ p1, Plane p,bool t)
        {
            double result=-1;

            //PlanarFace pf = 
            //Face f = p as Face;

            return result;
        }

        /// <summary>
        /// 两点水平距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double DistanceTo_Hosizontal(this XYZ p1, XYZ p2)
        {
            return p1.DistanceTo(new XYZ(p2.X, p2.Y, p1.Z));
        }

        /// <summary>
        /// 点到线的水平距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="xline"></param>
        /// <returns></returns>
        public static double DistanceTo_Hosizontal(this XYZ p1, Line xline)
        {
            double result = double.NegativeInfinity;

            XYZ p1_onLine = p1.ProjectToXLine(xline);

            p1_onLine = new XYZ(p1_onLine.X, p1_onLine.Y, p1.Z);

            result = p1.DistanceTo(p1_onLine);

            return result;
        }

        /// <summary>
        /// 判断点在面上
        /// </summary>
        /// <param name="po"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsOnPlane(this XYZ po, Plane p)
        {
            XYZ origin = p.Origin;
            XYZ normal = p.Normal;

            XYZ vector_po_origin = po - origin;
            if (IsEqual(normal.DotProduct(vector_po_origin), 0d))
            {
                return true;
            }
            else
            {
                return false;
            }

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

         

        /// <summary>
        /// 判断点在box里面 未测试
        /// </summary>
        /// <param name="po"></param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static bool IsInBox(this XYZ po, BoundingBoxXYZ box)
        {
            bool result = false;

            Transform tsf = box.Transform;

#if revit2016
            Plane p1 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));
            Plane p2 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));
            Plane p3 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));
            Plane p4 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));
            Plane p5 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));
            Plane p6 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));
#endif
#if revit2019
            Plane p1 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));
            Plane p2 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));
            Plane p3 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));
            Plane p4 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));
            Plane p5 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));
            Plane p6 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));
#endif



            //p1 p4
            //p2 p5
            //p3 p6

            XYZ po_p1 = po.Project(p1);
            XYZ po_p4 = po.Project(p4);
            Line l_p1_4 = Line.CreateBound(po_p1, po_p4);

            XYZ po_p2 = po.Project(p2);
            XYZ po_p5 = po.Project(p5);
            Line l_p2_5 = Line.CreateBound(po_p2, po_p5);

            XYZ po_p3 = po.Project(p3);
            XYZ po_p6 = po.Project(p6);
            Line l_p3_6 = Line.CreateBound(po_p3, po_p6);



            if (po.IsOnLine(l_p1_4) && po.IsOnLine(l_p2_5) && po.IsOnLine(l_p3_6))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 判断点是否在 box 垂直的四个面内部
        /// </summary>
        /// <param name="po"></param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static bool IsInRectangle(this XYZ po, BoundingBoxXYZ box)
        {
            bool result = false;

            Transform tsf = box.Transform;

#if revit2016
            Plane p1 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));
            Plane p2 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));
            //Plane p3 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));
            Plane p4 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));
            Plane p5 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));
            //Plane p6 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));
#endif
#if revit2019
            Plane p1 =   Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));
            Plane p2 =   Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));
            //Plane p3 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));
            Plane p4 =    Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));
            Plane p5 =   Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));
            //Plane p6 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));
#endif


            //p1 p4
            //p2 p5
            //p3 p6

            XYZ po_p1 = po.Project(p1);
            XYZ po_p4 = po.Project(p4);
            Line l_p1_4 = Line.CreateBound(po_p1, po_p4);

            XYZ po_p2 = po.Project(p2);
            XYZ po_p5 = po.Project(p5);
            Line l_p2_5 = Line.CreateBound(po_p2, po_p5);

            //XYZ po_p3 = po.ProjectToPlane(p3);
            //XYZ po_p6 = po.ProjectToPlane(p6);
            //Line l_p3_6 = Line.CreateBound(po_p3, po_p6);



            if (po.IsOnLine(l_p1_4) && po.IsOnLine(l_p2_5))
            {
                result = true;
            }

            return result;
        }


    }
}
