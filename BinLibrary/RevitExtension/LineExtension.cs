using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using BinLibrary.Extensions;
using BinLibrary.WinHelper;
namespace BinLibrary.RevitExtension
{
    public static class LineExtension
    {
        /// <summary>
        /// 直线或其延长线与面相交
        /// </summary>
        /// <param name="line"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static XYZ Intersect(this Line line, Plane p)
        {
            XYZ result = null;
            XYZ lineorigin = line.Origin;
            XYZ linedir = line.Direction;
            var origin_p = p.Origin;
            var norm = p.Normal;
            double cos_angle = linedir.DotProduct(norm);
            var lineoriginOnplane = lineorigin.Project(p);
            var dir = (lineoriginOnplane - lineorigin).Normalize(); //直线原点在平面上的投影点 到直线原点的方向
            double distance = lineoriginOnplane.DistanceTo(lineorigin);
            //判断直线指向平面还是背离平面
            double dirvalue = 0;
            var angel = dir.AngleTo(linedir);
            if (angel > Math.PI) angel = angel - Math.PI;
            if (angel.Equals(Math.PI / 2)) return result;
            else if (angel < Math.PI / 2)
            {
                dirvalue = 1;
            }
            else if (angel > Math.PI / 2)
            {
                dirvalue = -1;
            }
            if (cos_angle.IsEqual(0))
                result = lineorigin.Project(p);
            else
            {
                result = lineorigin + dirvalue * linedir * distance / Math.Abs(cos_angle);
                //MessageBox.Show("here");
            }
            return result;
        }
        //public static IList<XYZ> Intersect(this Line line, Face face, bool extendFlag = false)
        //{
        //    IList<XYZ> result = null;
        //        var xline = line.Clone();
        //        if (extendFlag)
        //            xline.MakeUnbound();
        //    result=((xline as Curve).Intersect(face));
        //    return result;
        //}
        public static XYZ StartPoint(this Line line)
        {
            if(line.IsBound)
            return line.GetEndPoint(0);
            return null;
        }
        public static XYZ EndPoint(this Line line)
        {
            if (line.IsBound)
                return line.GetEndPoint(1);
            return null;
        }
        /// <summary>
        /// 延长线段
        /// </summary>
        /// <param name="line"></param>
        /// <param name="distance"></param>
        /// <param name="plusDirection"></param>
        /// <returns></returns>
        public static Line Extend(this Line line, double distance, bool plusDirection = true)
        {
            XYZ endpoint1 = line.GetEndPoint(0);
            XYZ endpoint2 = line.GetEndPoint(1);
            XYZ direction = line.Direction;
            XYZ minusdirection = -line.Direction;
            if (plusDirection)
            {
                endpoint2 = endpoint2 + direction * distance;
            }
            else
            {
                endpoint1 = endpoint1 + minusdirection * distance;
            }
            return Line.CreateBound(endpoint1, endpoint2);
        }
        /// <summary>
        /// 扩展线段 返回新线段
        /// </summary>
        /// <param name="l"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static Line ExtendLine(this Line l, double tolerance)
        {
            Line resultLine = null;
            XYZ end1 = l.GetEndPoint(0);
            XYZ end2 = l.GetEndPoint(1);
            XYZ direct_end1_2 = end2 - end1;
            XYZ direct_end2_1 = end1 - end2;
            XYZ end1_extend = end1 + direct_end2_1.Normalize() * tolerance;
            XYZ end2_extend = end2 + direct_end1_2.Normalize() * tolerance;
            resultLine = Line.CreateBound(end1_extend, end2_extend);
            return resultLine;
        }
        /// <summary>
        /// 获取两条线的交点（不延长）
        /// </summary>
        /// <param name="line"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static XYZ Intersect_cus(this Line line, Line l)
        {
            return line.Intersect_cus(l as Curve);
        }
        const double precision = 0.00001;    // 精度
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
        /// 判断两条线共线
        /// </summary>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        /// <returns></returns>
        public static bool IsOnSameLine(this Line l, Line l1)
        {
            if (l.Direction.IsSameDirection(l1.Direction) || l.Direction.IsOppositeDirection(l1.Direction))
            {
                //                l.GetEndPoint(0).DistanceTo(l1)
                if (IsEqual(l.DistanceTo(l1.GetEndPoint(0)), 0d))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// 判断两条线平行 包括共线的情况
        /// </summary>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        /// <returns></returns>
        public static bool IsParallel(this Line l, Line l1)
        {
            if (l.Direction.IsSameDirection(l1.Direction) || l.Direction.IsOppositeDirection(l1.Direction))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断两直线垂直 共面或异面
        /// </summary>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        /// <returns></returns>
        public static bool IsPerpendicularWith(this Line l, Line l1)
        {
            var dir1 = l.Direction.Normalize();
            var dir2 = l1.Direction.Normalize();
            var dotproduct = dir1.DotProduct(dir2);
            if (dotproduct.IsEqual(0d)) return true;
            return false;
        }
        /// <summary>
        /// 判断两条直线共面
        /// </summary>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        /// <returns></returns>
        public static bool IsOnSamePlane(this Line l, Line l1)
        {
            //MessageBox.Show(l1.Direction.ToString() + Environment.NewLine + l1.Direction.ToString());
            if (l.IsParallel(l1))
            {
                //MessageBox.Show("start of isonplane method");
                return true;
            }
            Line l_copy = Line.CreateUnbound(l.Origin, l.Direction);
            Line l1_copy = Line.CreateUnbound(l1.Origin, l1.Direction);
            #region for test
            //MessageBox.Show(l.Origin.ToString() + ":" +       l.Direction.ToString() + Environment.NewLine +
            //                l_copy.Origin.ToString() + ":" +  l_copy.Direction.ToString() + Environment.NewLine +
            //                l1.Origin.ToString() + ":" +      l1.Direction.ToString() + Environment.NewLine +
            //                l1_copy.Origin.ToString() + ":" + l1_copy.Direction.ToString());
            //MessageBox.Show("intersection point " + l.GetIntersection(l1).ToString());
            //MessageBox.Show("intersection point _ copy lines" + l_copy.GetIntersection(l1_copy).ToString());
            #endregion
            XYZ intersection = l_copy.Intersect_cus(l1_copy);
            #region 两直线延长相交则共面  不相交 则不共面
            if (intersection != null)
            {
                //MessageBox.Show("1 of isonplane method");
                return true;
            }
            else
            {
                return false;
            }
            #endregion
            //l上取两个点 避开 交点
            #region 方法理论可行 实际不行不知道为什么
            XYZ p1_l = l1.GetEndPoint(0);
            if (intersection.IsAlmostEqualTo(p1_l))
            {
                p1_l = p1_l + l.Direction;
            }
            XYZ p2_l = l1.GetEndPoint(0);
            if (intersection.IsAlmostEqualTo(p2_l))
            {
                p2_l = p2_l + l.Direction;
            }
            XYZ p1_l1 = l1.GetEndPoint(0);
            if (intersection.IsAlmostEqualTo(p1_l1))
            {
                p1_l1 = p1_l1 + l1.Direction;
            }
            XYZ p2_l1 = l1.GetEndPoint(1);
            if (intersection.IsAlmostEqualTo(p2_l1))
            {
                p2_l1 = p2_l1 + l1.Direction;
            }
            XYZ vector_between_two_line = p1_l - p1_l1;
            XYZ vector_between_two_line1 = p2_l - p2_l1;
            XYZ vector_along_l1 = l1.Direction;
            XYZ vector_along_l = l.Direction;
            XYZ vector1 = vector_along_l.CrossProduct(vector_between_two_line);
            XYZ vector2 = vector_along_l1.CrossProduct(vector_between_two_line1);
            MessageBox.Show(vector1.IsSameDirection(vector2).ToString());
            MessageBox.Show(vector1.IsOppositeDirection(vector2).ToString());
            if (vector1.IsSameDirection(vector2) || vector1.IsOppositeDirection(vector2))
            {
                return true;
            }
            else
            {
                return false;
            }
            #endregion
        }
        public static bool IsHorizontal(this Line l)
        {
            if (l.Direction.Z.IsEqual(0) && (!l.Direction.X.IsEqual(0) || !l.Direction.Y.IsEqual(0)))
            {
                return true;
            }
            return false;
        }
        public static bool IsVertical(this Line l)
        {
            if (l.Direction.X.IsEqual(0) && l.Direction.Y.IsEqual(0) && !l.Direction.Z.IsEqual(0))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 返回直线到点的距离
        /// </summary>
        /// <param name="l"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static double DistanceTo(this Line l, XYZ po)
        {
            var l1 = l.Clone();
            l1.MakeUnbound();
            XYZ pointOnLine = l1.Project(po).XYZPoint;
            return pointOnLine.DistanceTo(po);
        }
        /// <summary>
        /// 计算直线间的距离 平行 异面 (未测试)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        /// <returns></returns>
        public static double DistanceTo(this Line l, Line l1)
        {
            double distance;
            if (l.Direction.IsSameDirection(l1.Direction) || l.Direction.IsOppositeDirection(l1.Direction))
            {
                distance = l.Origin.DistanceTo(l1);
            }
            else
            {
                XYZ perpVecOfTwoLine = l.Direction.CrossProduct(l1.Direction);//两条线的公垂线方向
#if revit2016
                Plane p = new Plane(perpVecOfTwoLine.Normalize(), l.Origin);
#endif
#if revit2019
                Plane p =   Plane.CreateByNormalAndOrigin(perpVecOfTwoLine.Normalize(), l.Origin);
#endif
                XYZ point_on_l1 = l1.Origin;
                //计算l1起点到p的距离
                XYZ vec_l1_OriginTo_P_origin = p.Origin - point_on_l1;
                XYZ p_norm = p.Normal;
                distance = Math.Abs(vec_l1_OriginTo_P_origin.DotProduct(p_norm));
            }
            return distance;
        }
        /// <summary>
        /// 计算直线间的距离 平行 异面 输出异面直线公垂线与两个直线 多交点 (未测试)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double DistanceTo(this Line l, Line l1, out IList<XYZ> points)
        {
            points = new List<XYZ>();
#if revit2016
            Plane p = new Plane( l.Direction.CrossProduct(l1.Direction).Normalize(),l.Origin);
#endif
#if revit2019
            Plane p = Plane.CreateByNormalAndOrigin(l.Direction.CrossProduct(l1.Direction).Normalize(), l.Origin);
#endif
            XYZ intersecPo_l; //公垂线与l交点
            XYZ intersecPo_l1;//公垂线与l1交点
            Line l1_projectONplane;
            XYZ end1_l1 = l1.GetEndPoint(0);
            XYZ end2_l1 = l1.GetEndPoint(1);
            XYZ end1_l1_proj = end1_l1.Project(p);
            XYZ end2_l1_proj = end2_l1.Project(p);
            l1_projectONplane = Line.CreateBound(end1_l1_proj, end2_l1_proj);
            intersecPo_l = l.Intersect_cus(l1_projectONplane);
            points.Add(intersecPo_l);
#if revit2016
              Plane p1 = new Plane( l1.Direction.CrossProduct(l.Direction).Normalize(), l1.Origin);
#endif
#if revit2019
            Plane p1 = Plane.CreateByNormalAndOrigin(l1.Direction.CrossProduct(l.Direction).Normalize(), l1.Origin);
#endif
            intersecPo_l1 = intersecPo_l.Project(p1);
            points.Add(intersecPo_l1);
            return intersecPo_l.DistanceTo(intersecPo_l1);
        }
        /// <summary>
        /// 计算直线间的距离 平行 异面 输出异面直线公垂线
        /// </summary>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        /// <param name="outLine"></param>
        /// <returns></returns>
        public static double DistanceTo(this Line l, Line l1, out Line outLine)
        {
            outLine = null;
            if (l.IsParallel(l1))
            {
                return l.DistanceTo(l1);
            }
            else
            {
#if revit2016
                 Plane p = new Plane(l.Direction.CrossProduct(l1.Direction).Normalize(), l.Origin);
#endif
#if revit2019
                Plane p = Plane.CreateByNormalAndOrigin(l.Direction.CrossProduct(l1.Direction).Normalize(), l.Origin);
#endif
                XYZ intersecPo_l; //公垂线与l交点
                XYZ intersecPo_l1;//公垂线与l1交点
                Line l1_projectONplane;
                XYZ end1_l1 = l1.GetEndPoint(0);
                XYZ end2_l1 = l1.GetEndPoint(1);
                XYZ end1_l1_proj = end1_l1.Project(p);
                XYZ end2_l1_proj = end2_l1.Project(p);
                l1_projectONplane = Line.CreateBound(end1_l1_proj, end2_l1_proj);
                l1_projectONplane.MakeUnbound(); //无限延长直线
                l.MakeUnbound(); //无限延长直线
                intersecPo_l = l.Intersect_cus(l1_projectONplane);
                //points.Add(intersecPo_l);
#if revit2016
                Plane p1 = new Plane(l1.Direction.CrossProduct(l.Direction).Normalize(), l1.Origin);
#endif
#if revit2019
                Plane p1 =   Plane.CreateByNormalAndOrigin(l1.Direction.CrossProduct(l.Direction).Normalize(), l1.Origin);
#endif
                intersecPo_l1 = intersecPo_l.Project(p1);
                //points.Add(intersecPo_l1);
                outLine = Line.CreateBound(intersecPo_l, intersecPo_l1);
                return intersecPo_l.DistanceTo(intersecPo_l1);
            }
        }
        /// <summary>
        /// 获取两条异面直线的公垂线段
        /// </summary>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        /// <returns></returns>
        public static Line GetCommonPerpLine(this Line l, Line l1)
        {
            Line result = null;
            if (l.IsParallel(l1))
            {
                return null;
            }
#if revit2016
            Plane p = new Plane(l.Direction.CrossProduct(l1.Direction).Normalize(), l.Origin);
#endif
#if revit2019
            Plane p = Plane.CreateByNormalAndOrigin(l.Direction.CrossProduct(l1.Direction).Normalize(), l.Origin);
#endif
            XYZ intersecPo_l; //公垂线与l交点
            XYZ intersecPo_l1;//公垂线与l1交点
            Line l1_projectONplane;
            XYZ end1_l1 = l1.GetEndPoint(0);
            XYZ end2_l1 = l1.GetEndPoint(1);
            XYZ end1_l1_proj = end1_l1.Project(p);
            XYZ end2_l1_proj = end2_l1.Project(p);
            l1_projectONplane = Line.CreateBound(end1_l1_proj, end2_l1_proj);
            l1_projectONplane.MakeUnbound(); //无限延长直线
            l.MakeUnbound(); //无限延长直线
            intersecPo_l = l.Intersect_cus(l1_projectONplane);
            //points.Add(intersecPo_l);
#if revit2016
            Plane p1 = new Plane(l1.Direction.CrossProduct(l.Direction).Normalize(), l1.Origin);
#endif
#if revit2019
            Plane p1 =  Plane.CreateByNormalAndOrigin(l1.Direction.CrossProduct(l.Direction).Normalize(), l1.Origin);
#endif
            intersecPo_l1 = intersecPo_l.Project(p1);
            //points.Add(intersecPo_l1);
            result = Line.CreateBound(intersecPo_l, intersecPo_l1);
            return result;
        }
        /// <summary>
        /// 获取两条直线公垂线的交点集合 按直线顺序对应索引顺序
        /// </summary>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        /// <returns></returns>
        public static IList<XYZ> GetCommonPerpLineEnds(this Line l, Line l1)
        {
            IList<XYZ> result = new List<XYZ>();
            if (l.IsParallel(l1))
            {
                return result;
            }
#if revit2016
            Plane p = new Plane(l.Direction.CrossProduct(l1.Direction).Normalize(), l.Origin);
#endif
#if revit2019
            Plane p =   Plane.CreateByNormalAndOrigin(l.Direction.CrossProduct(l1.Direction).Normalize(), l.Origin);
#endif
            XYZ intersecPo_l; //公垂线与l交点
            XYZ intersecPo_l1;//公垂线与l1交点
            Line l1_projectONplane;
            XYZ end1_l1 = l1.GetEndPoint(0);
            XYZ end2_l1 = l1.GetEndPoint(1);
            XYZ end1_l1_proj = end1_l1.Project(p);
            XYZ end2_l1_proj = end2_l1.Project(p);
            l1_projectONplane = Line.CreateBound(end1_l1_proj, end2_l1_proj);
            l1_projectONplane.MakeUnbound(); //无限延长直线
            //l.MakeUnbound(); //无限延长直线
            Line l_copy = Line.CreateBound(l.GetEndPoint(0), l.GetEndPoint(1));
            l_copy.MakeUnbound();
            intersecPo_l = l_copy.Intersect_cus(l1_projectONplane);
            //points.Add(intersecPo_l);
#if revit2016
             Plane p1 = new Plane(l1.Direction.CrossProduct(l.Direction).Normalize(), l1.Origin);
#endif
#if revit2019
            Plane p1 =   Plane.CreateByNormalAndOrigin(l1.Direction.CrossProduct(l.Direction).Normalize(), l1.Origin);
#endif
            intersecPo_l1 = intersecPo_l.Project(p1);
            //points.Add(intersecPo_l1);
            result.Add(intersecPo_l);
            result.Add(intersecPo_l1);
            return result;
        }
        /* 这个方法有问题  错误经典案例*/
        /// <summary>
        /// 获取构造线与平面交点 已知直线 和平面
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [Obsolete]
        public static XYZ GetIntersectionPoint1(this Line l, Plane p)  //方法错误废弃
        {
            // 算法思路 直线 原点 和 方向 
            // 平面 原点 和 方向 
            // 从直线 原点开始算起 原点 + 直线方向* 直线原点到面的距离 除以 (直线方向和 面法线的向量点积)
            // 此情况 忽略了指向原点 在面一侧 且 方向远离 的情况
            XYZ po = l.Origin;
            XYZ dir = l.Direction;
            XYZ result = null;
            XYZ norm_dir = dir.Normalize();
            XYZ norm_p = p.Normal.Normalize();
            if (!IsEqual(norm_dir.DotProduct(norm_p), 0))
            {
                result = po + norm_dir * po.DistanceTo(p) / norm_dir.DotProduct(norm_p);
            }
            return result;
        }
        /// <summary>
        /// 长度线段和面相交 相交返回点 不相交 返回空
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static XYZ GetIntersectionPoint(this Line l, Plane p)
        {
            XYZ result = null;
            if (!IsEqual(l.Direction.DotProduct(p.Normal), 0))
            {
                if (l.IsBound)
                {
                    XYZ end1_p = l.GetEndPoint(0).Project(p);
                    XYZ end2_p = l.GetEndPoint(1).Project(p);
                    Line l_tem = Line.CreateUnbound(end1_p, end2_p);
                    result = l_tem.Intersect_cus(l);
                }
                else
                {
                    XYZ end1_p = l.Origin.Project(p);
                    XYZ end2_p = (l.Origin + l.Direction * 3).Project(p);
                    Line l_tem = Line.CreateUnbound(end1_p, end2_p);
                    result = l_tem.Intersect_cus(l);
                }
            }
            return result;
        }
        /// <summary>
        /// 判断直线是否和boundingbox相交
        /// 把一条线分成n段 判断等分点是否在box里面
        /// </summary>
        /// <param name="l"></param>
        /// <param name="box"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool IntersectWithBox(this Line l, BoundingBoxXYZ box, int precision /*等分点判断点是否在box内*/)
        {
            bool result = false;
            XYZ origin = l.Origin;
            XYZ dir = l.Direction.Normalize();
            for (int i = 0; i < precision; i++)
            {
                XYZ tempoint = origin + dir * i;
                if (tempoint.IsInBox(box))
                {
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// 与box四面构成的柱型体相交
        /// </summary>
        /// <param name="l"></param>
        /// <param name="box"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool IntersectWithRectangle(this Line l, BoundingBoxXYZ box, int precision /*等分点判断点是否在box内*/)
        {
            bool result = false;
            XYZ origin = l.Origin;
            XYZ dir = l.Direction.Normalize();
            for (int i = 0; i < precision; i++)
            {
                XYZ tempoint = origin + dir * i;
                if (tempoint.IsInRectangle(box))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
