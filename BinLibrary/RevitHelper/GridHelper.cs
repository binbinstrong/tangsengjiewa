using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BinLibrary.RevitExtension;
using Level = Autodesk.Revit.DB.Level;
using Grid = Autodesk.Revit.DB.Grid;

namespace BinLibrary.RevitHelper
{
    public static class GridHelper
    {

        /// <summary>
        /// 根据轴网标高生成线  未测试
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="l"></param>
        /// <returns></returns>e
        public static Line GenLineAccordingGrid(Grid grid, Level l)
        {

            Line resultline = null;

            Curve gridCurve = grid.Curve;

            double elevation = l.Elevation;

            //轴网端点
            XYZ end1 = gridCurve.GetEndPoint(0);
            XYZ end2 = gridCurve.GetEndPoint(1);

            //当前楼层标高生成线
            //1 取得本楼层轴线的两个投影点
            XYZ end1_l = new XYZ(end1.X, end1.Y, elevation);
            XYZ end2_l = new XYZ(end2.X, end2.Y, elevation);

            //由投影点生成线
            resultline = Line.CreateBound(end1_l, end2_l);

            return resultline;
        }

        /// <summary>
        /// 根据轴网标高生成线 并返回带轴线信息的线段 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="l"></param>
        /// <param name="LineWithGridMsg"></param>
        /// <returns></returns>
        public static Line GenLineAccordingGrid(Grid grid, Level l, out KeyValuePair<Line, Grid> LineWithGridMsg)
        {
            LineWithGridMsg = new KeyValuePair<Line, Grid>();

            Line resultline = null;

            Curve gridCurve = grid.Curve;

            double elevation = l.Elevation;

            //轴网端点
            XYZ end1 = gridCurve.GetEndPoint(0);
            XYZ end2 = gridCurve.GetEndPoint(1);

            //当前楼层标高生成线
            //1 取得本楼层轴线的两个投影点
            XYZ end1_l = new XYZ(end1.X, end1.Y, elevation);
            XYZ end2_l = new XYZ(end2.X, end2.Y, elevation);

            //由投影点生成线
            resultline = Line.CreateBound(end1_l, end2_l);

            LineWithGridMsg = new KeyValuePair<Line, Grid>(resultline, grid);

            return resultline;
        }

        /// <summary>
        /// 三维里根据 轴网 标高 和 Sectionbox 生成线(与sectionbox四边的延伸面相交)
        /// 如果线的一端在box内 则生成此点和交点线段 并返回带轴线信息的线段   注意：必须是三维视图的sectionbox 而不是视图的boundingbox
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="l"></param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static Line GenLineAccordingGrid(Grid grid, Level l, BoundingBoxXYZ box, out KeyValuePair<Line, Grid> LineWithGridMsg)
        {
            LineWithGridMsg = new KeyValuePair<Line, Grid>();

            Line resultLine = null;

            List<XYZ> points = new List<XYZ>();
            HashSet<XYZ> Hapoints = new HashSet<XYZ>();

            Transform tsf = box.Transform;
#if revit2016
            Plane p1 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p2 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p3 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p4 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p5 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p6 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
#endif
#if revit2019
            Plane p1 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p2 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p3 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p4 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p5 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p6 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
#endif


            List<Plane> plans = new List<Plane>();
            plans.Add(p1);
            plans.Add(p2);
            plans.Add(p3);
            plans.Add(p4);
            plans.Add(p5);
            plans.Add(p6);

            
            Curve gridCurve = grid.Curve;
            double elevation = l.Elevation;

            //轴网端点
            XYZ end1 = gridCurve.GetEndPoint(0);
            XYZ end2 = gridCurve.GetEndPoint(1);

            //当前楼层标高生成线
            //1 取得本楼层轴线的两个投影点
            XYZ end1_l = new XYZ(end1.X, end1.Y, elevation);
            XYZ end2_l = new XYZ(end2.X, end2.Y, elevation);

            //下面一句有bug 当 超出轴线的范围时就没有交点 若要超出轴线也有交点必须 把line 修改为无限延伸的线
            Line line = Line.CreateBound(end1_l, end2_l);
            Line line_copy = Line.CreateBound(end1_l, end2_l);

            line.MakeUnbound();//修复上面描述的bug

            #region 绘制线测试 box位置

            //XYZ min_tr = tsf.OfPoint(box.Min);
            //XYZ max_tr = tsf.OfPoint(box.Max);

            //l.Document.NewLine(Line.CreateBound(min_tr, max_tr));

            //l.Document.NewLine(line);

            //l.Document.NewCircle(p1.Origin, p1.Normal, 6);
            //l.Document.NewCircle(p2.Origin, p2.Normal, 6);
            //l.Document.NewCircle(p3.Origin, p3.Normal, 6);
            //l.Document.NewCircle(p4.Origin, p4.Normal, 6);
            //l.Document.NewCircle(p5.Origin, p5.Normal, 6);
            //l.Document.NewCircle(p6.Origin, p6.Normal, 6);

            #endregion


            //遍历各个面 求直线 与 box 构成的 交点

            #region 方法一 不对

            //foreach (Plane p in plans)
            //{
            //    if (!line.GetEndPoint(0).IsInBox(box))
            //    {
            //        //points.Add(line.GetIntersectionPoint(p));
            //        if (line.GetIntersectionPoint(p) != null)
            //        {
            //            Hapoints.Add(line.GetIntersectionPoint(p));
            //        }
            //    }
            //    else
            //    {
            //        //points.Add(line.GetEndPoint(0));
            //        Hapoints.Add(line.GetEndPoint(0));
            //    }


            //    if (!line.GetEndPoint(1).IsInBox(box))
            //    {
            //        //points.Add(line.GetIntersectionPoint(p));
            //        if (line.GetIntersectionPoint(p) != null)
            //        {
            //            Hapoints.Add(line.GetIntersectionPoint(p));
            //        }
            //    }
            //    else
            //    {
            //        //points.Add(line.GetEndPoint(1));
            //        Hapoints.Add(line.GetEndPoint(1));
            //    }

            //}
            #endregion


            #region 方法二也不对

            //if (end1_l.IsInBox(box) && end2_l.IsInBox(box))
            //{
            //    resultLine = Line.CreateBound(end1_l, end2_l);
            //}

            //if (end1_l.IsInBox(box) && !end2_l.IsInBox(box))
            //{

            //    XYZ intersec = null;
            //    foreach (Plane p in plans)
            //    {
            //        intersec = line.GetIntersectionPoint(p);
            //    }
            //    resultLine = Line.CreateBound(end1_l, intersec);
            //}

            //if (!end1_l.IsInBox(box) && end2_l.IsInBox(box))
            //{
            //    XYZ intersec = null;
            //    foreach (Plane p in plans)
            //    {
            //        intersec = line.GetIntersectionPoint(p);
            //    }
            //    resultLine = Line.CreateBound(end2_l, intersec);
            //}

            //if (!end1_l.IsInBox(box) && !end2_l.IsInBox(box))
            //{
            //    XYZ intersec1 = null;
            //    XYZ intersec2 = null;
            //    foreach (Plane p in plans)
            //    {
            //        if (intersec1 == null)
            //        {
            //            intersec1 = line.GetIntersectionPoint(p);
            //        }
            //        intersec2 = line.GetIntersectionPoint(p);
            //    }
            //    if (intersec2 != null && intersec1 != null)
            //    {
            //        resultLine = Line.CreateBound(intersec1, intersec2);
            //    }
            //}

            #endregion

            #region 查看点的信息

            #region 方法三

            //MessageBox.Show(plans.Count.ToString());

             

            if (line.IntersectWithBox(box, 500))
            {

                foreach (Plane p in plans)
                {
                    #region test

                    
                    //l.Document.NewCircle(p.Origin, p.Normal);
                    //l.Document.NewLine(p.Origin, p.Origin + p.Normal * 3);

                    #endregion

                    if (line.GetIntersectionPoint(p) != null)
                    {
                        points.Add(line.GetIntersectionPoint(p));
                    }

                     

                    #region test

                    //MessageBox.Show("intersec1 to be construct");

                    //XYZ tem = line.GetIntersectionPoint(p);

                    //l.Document.NewLine(line);

                    //l.Document.NewCircle(tem, p.Normal);

                    //l.Document.NewLine(tem, tem + p.Normal*3);
                    //l.Document.NewLine(tem, tem + p.XVec*3);
                    //l.Document.NewLine(tem, tem + p.YVec*3);

                    //MessageBox.Show("intersec1   constructed");

                    #endregion
                }
            }
            else
            {
                return null;
            }
#if DEBUG
            //LogHelper.LogWrite(@"c:\GenLineAccordingGrid.txt",grid.Name+"::"+l.Name+"::"+
            //    l.Elevation.ToString() + "__" + end1_l.ToString() + "__" + end2_l.ToString() + "points:" + points.Count+">>>>");
#endif

           

            //MessageBox.Show(points.Count.ToString());

            if (points.Count == 2)
            {
                resultLine = Line.CreateBound(points[0], points[1]);

                //l.Document.NewCircle(points[0], resultLine.Direction);   //test
                //l.Document.NewCircle(points[1], resultLine.Direction);   //test

            }
            if (points.Count == 1)
            {
                //MessageBox.Show(points[0].ToString());

                if (end1_l.IsInBox(box))
                {
                    resultLine = Line.CreateBound(points[0], end1_l);
                }
                if (end2_l.IsInBox(box))
                {
                    resultLine = Line.CreateBound(points[0], end2_l);
                }
            }
            if (points.Count == 0)
            {
                //MessagedBox.Show("inbox");

                if (line_copy.GetEndPoint(0).IsInBox(box) || line_copy.GetEndPoint(1).IsInBox(box))
                {
                    resultLine = line;
                    //MessageBox.Show("inbox");
                }
                else
                {
                    //resultLine = null;
                }
                //resultLine = null;
            }

            //if (resultLine != null)
            //{
            //l.Document.NewLine(resultLine);
            //}


            //l.Document.NewCircle(resultLine.GetEndPoint(0), resultLine.Direction);
            //l.Document.NewCircle(resultLine.GetEndPoint(1), resultLine.Direction);


            #endregion


            //MessageBox.Show(resultLine.GetEndPoint(0).ToString());

            #endregion

            LineWithGridMsg = new KeyValuePair<Line, Grid>(resultLine, grid);

            return resultLine;
        }

        public static Line GenLineAccordingGrid_Unbound(Grid grid, Level l, BoundingBoxXYZ box, out KeyValuePair<Line, Grid> LineWithGridMsg)
        {
            LineWithGridMsg = new KeyValuePair<Line, Grid>();

            Line resultLine = null;

            List<XYZ> points = new List<XYZ>();
            HashSet<XYZ> Hapoints = new HashSet<XYZ>();

            Transform tsf = box.Transform;
#if revit2016
            Plane p1 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p2 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p3 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p4 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p5 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p6 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
#endif
#if revit2019
            Plane p1 =   Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p2 =   Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p3 =   Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p4 =   Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p5 =   Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p6 =   Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
#endif


            List<Plane> plans = new List<Plane>();
            plans.Add(p1);
            plans.Add(p2);
            plans.Add(p3);
            plans.Add(p4);
            plans.Add(p5);
            plans.Add(p6);


            Curve gridCurve = grid.Curve;
            double elevation = l.Elevation;


            if (!gridCurve.IsBound)
            {
                MessageBox.Show(grid.Name + Environment.NewLine + grid.Id);
                return resultLine;
            }
            //轴网端点
            XYZ end1 = gridCurve.GetEndPoint(0);
            XYZ end2 = gridCurve.GetEndPoint(1);

            //当前楼层标高生成线
            //1 取得本楼层轴线的两个投影点
            XYZ end1_l = new XYZ(end1.X, end1.Y, elevation);
            XYZ end2_l = new XYZ(end2.X, end2.Y, elevation);

            //下面一句有bug 当 超出轴线的范围时就没有交点 若要超出轴线也有交点必须 把line 修改为无限延伸的线
            Line line = Line.CreateBound(end1_l, end2_l);
            line.MakeUnbound();


            //遍历各个面 求直线 与 box 构成的 交点
            if (line.IntersectWithRectangle(box, 100))
            {
                foreach (Plane p in plans)
                {
                    #region test

                    //l.Document.NewCircle(p.Origin, p.Normal);
                    //l.Document.NewLine(p.Origin, p.Origin + p.Normal * 3);

                    #endregion

                    if (line.GetIntersectionPoint(p) != null)
                    {
                        points.Add(line.GetIntersectionPoint(p));
                    }

                }
            }
            //MessageBox.Show(points.Count.ToString());

            if (points.Count == 2)
            {
                resultLine = Line.CreateBound(points[0], points[1]);

                //l.Document.NewCircle(points[0], resultLine.Direction);   //test
                //l.Document.NewCircle(points[1], resultLine.Direction);   //test

            }
            //if (points.Count == 1)
            //{
            //    //MessageBox.Show(points[0].ToString());

            //    if (end1_l.IsInBox(box))
            //    {
            //        resultLine = Line.CreateBound(points[0], end1_l);
            //    }
            //    if (end2_l.IsInBox(box))
            //    {
            //        resultLine = Line.CreateBound(points[0], end2_l);
            //    }
            //}
            //if (points.Count == 0)
            //{
            //    //MessagedBox.Show("inbox");

            //    if (line.GetEndPoint(0).IsInBox(box))
            //    {
            //        resultLine = line;
            //        //MessageBox.Show("inbox");
            //    }
            //    else
            //    {
            //        //resultLine = null;
            //    }
            //    //resultLine = null;
            //}

            LineWithGridMsg = new KeyValuePair<Line, Grid>(resultLine, grid);

            return resultLine;
        }

        /// <summary>
        /// 获取轴线在楼层平面的投影线与 sectionbox四面交点 并输出
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="l"></param>
        /// <param name="box"></param>
        /// <param name="rightPo"></param>
        /// <param name="leftPo"></param>
        /// <param name="topPo"></param>
        /// <param name="bottomPo"></param>
        /// <returns></returns>
        public static Line GetIntersectionPointOnBox(Grid grid, Level l, BoundingBoxXYZ box, out KeyValuePair<XYZ, string> rightPo, out KeyValuePair<XYZ, string> leftPo, out KeyValuePair<XYZ, string> topPo, out KeyValuePair<XYZ, string> bottomPo)
        {
            rightPo = new KeyValuePair<XYZ, string>();
            leftPo = new KeyValuePair<XYZ, string>();
            topPo = new KeyValuePair<XYZ, string>();
            bottomPo = new KeyValuePair<XYZ, string>();

            Line resultLine = null;

            List<XYZ> points = new List<XYZ>();
            HashSet<XYZ> Hapoints = new HashSet<XYZ>();

            Transform tsf = box.Transform;
#if revit2016
              Plane p1 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p2 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p3 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p4 = new Plane(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p5 = new Plane(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p6 = new Plane(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
#endif
#if revit2019
            Plane p1 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p2 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p3 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Min));     //将模型转到世界坐标系
            Plane p4 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisX), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p5 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisY), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
            Plane p6 =  Plane.CreateByNormalAndOrigin(tsf.OfVector(box.Transform.BasisZ), tsf.OfPoint(box.Max));     //将模型转到世界坐标系
#endif



            Curve gridCurve = grid.Curve;
            double elevation = l.Elevation;

            //轴网端点
            XYZ end1 = gridCurve.GetEndPoint(0);
            XYZ end2 = gridCurve.GetEndPoint(1);

            //当前楼层标高生成线
            //1 取得本楼层轴线的两个投影点
            XYZ end1_l = new XYZ(end1.X, end1.Y, elevation);
            XYZ end2_l = new XYZ(end2.X, end2.Y, elevation);

            Line line = Line.CreateBound(end1_l, end2_l);
            line.MakeUnbound();



            XYZ left = line.GetIntersectionPoint(p1);
            if (left != null)
            {
                leftPo = new KeyValuePair<XYZ, string>(left, grid.Name);
            }
            var right = line.GetIntersectionPoint(p4);
            if (right != null)
            {
                rightPo = new KeyValuePair<XYZ, string>(right, grid.Name);
            }
            var top = line.GetIntersectionPoint(p2);
            if (top != null)
            {
                bottomPo = new KeyValuePair<XYZ, string>(top, grid.Name);
            }
            var bottom = line.GetIntersectionPoint(p5);
            if (bottom != null)
            {
                topPo = new KeyValuePair<XYZ, string>(bottom, grid.Name);
            }

            return resultLine;
        }
    }
}
