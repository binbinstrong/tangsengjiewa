using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Text;
using BinLibrary.Extensions;

namespace BinLibrary.RevitHelper
{
    public static class GeometryCreationProfileHelper
    {
        /// <summary>
        /// 创建正方形
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="sidelen"></param>
        /// <returns></returns>
        public static CurveLoop CreateSquare(Transform ts, double sidelen)
        {
            var origin = ts.Origin;
            var xdir = ts.BasisX;
            var ydir = ts.BasisY;
            var zdir = ts.BasisZ;
            var halflen = sidelen / 2;
            var point1 = origin + halflen * xdir + halflen * ydir;//第一象限点
            var point2 = origin - halflen * xdir + halflen * ydir;//第二象限点
            var point3 = origin - halflen * xdir - halflen * ydir;//第二象限点
            var point4 = origin + halflen * xdir - halflen * ydir;//第二象限点

            CurveLoop cp = new CurveLoop();
            cp.Append(Line.CreateBound(point1, point2));
            cp.Append(Line.CreateBound(point2, point3));
            cp.Append(Line.CreateBound(point3, point4));
            cp.Append(Line.CreateBound(point4, point1));

            return cp;
        }
        /// <summary>
        /// 创建倒角的正方形
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="sidelen"></param>
        /// <returns></returns>
        public static CurveLoop CreateSquare(Transform ts, double sidelen, double chamferRadius)
        {
            CurveLoop cp = new CurveLoop();
            cp = CreateFilletedRectangle(ts, sidelen, sidelen, chamferRadius);
            return cp;
        }
        /// <summary>
        /// 创建长方形
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="len"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static CurveLoop CreateRectangle(Transform ts, double len, double width)
        {
            var origin = ts.Origin;
            var xdir = ts.BasisX;
            var ydir = ts.BasisY;
            var zdir = ts.BasisZ;
            var halfheight = len / 2;
            var halfwid = width / 2;

            var point1 = origin + halfwid * xdir + halfheight * ydir;//第一象限点
            var point2 = origin - halfwid * xdir + halfheight * ydir;//第二象限点
            var point3 = origin - halfwid * xdir - halfheight * ydir;//第二象限点
            var point4 = origin + halfwid * xdir - halfheight * ydir;//第二象限点

            CurveLoop cp = new CurveLoop();
            cp.Append(Line.CreateBound(point1, point2));
            cp.Append(Line.CreateBound(point2, point3));
            cp.Append(Line.CreateBound(point3, point4));
            cp.Append(Line.CreateBound(point4, point1));

            return cp;
        }
        /// <summary>
        /// 创建带倒角的长方形
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="len"></param>
        /// <param name="width"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static CurveLoop CreateFilletedRectangle(Transform ts, double len, double width, double radius)
        {
            var origin = ts.Origin;
            var xdir = ts.BasisX;
            var ydir = ts.BasisY;
            var zdir = ts.BasisZ;

            var halfheight = len / 2;
            var halfwid = width / 2;
            if (radius > halfwid || radius > halfheight) return null;

            //倒角半径小于0.8mm 直接返回
            if (radius.FeetToMetric() < 0.8)
            {
                return CreateRectangle(ts, len, width);
            };

            var centerPoint1 = origin + (halfwid - radius) * xdir + (halfheight - radius) * ydir;//第一象限点
            var centerPoint2 = origin - (halfwid - radius) * xdir + (halfheight - radius) * ydir;//第二象限点
            var centerPoint3 = origin - (halfwid - radius) * xdir - (halfheight - radius) * ydir;//第二象限点
            var centerPoint4 = origin + (halfwid - radius) * xdir - (halfheight - radius) * ydir;//第二象限点

            Arc arc1 = Arc.Create(centerPoint1, radius, 0, Math.PI / 2, xdir, ydir);
            Arc arc2 = Arc.Create(centerPoint2, radius, Math.PI / 2, Math.PI, xdir, ydir);
            Arc arc3 = Arc.Create(centerPoint3, radius, Math.PI, Math.PI * 3 / 2, xdir, ydir);
            Arc arc4 = Arc.Create(centerPoint4, radius, Math.PI * 3 / 2, Math.PI * 2, xdir, ydir);

            var point1 = arc1.GetEndPoint(0);
            var point2 = arc1.GetEndPoint(1);

            var point3 = arc2.GetEndPoint(0);
            var point4 = arc2.GetEndPoint(1);

            var point5 = arc3.GetEndPoint(0);
            var point6 = arc3.GetEndPoint(1);

            var point7 = arc4.GetEndPoint(0);
            var point8 = arc4.GetEndPoint(1);

            

            var line1 = Line.CreateBound(point2, point3);
            var line2 = Line.CreateBound(point4, point5);
            var line3 = Line.CreateBound(point6, point7);
            var line4 = Line.CreateBound(point8, point1);
            CurveLoop cp = new CurveLoop();

            cp.Append(arc1);
            cp.Append(line1);
            cp.Append(arc2);
            cp.Append(line2);
            cp.Append(arc3);
            cp.Append(line3);
            cp.Append(arc4);
            cp.Append(line4);
            return cp;
        }
        /// <summary>
        /// 创建正多边形
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="sideNum"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static CurveLoop CreatePolygon(Transform ts, int sideNum, double radius /*外接圆半径*/)
        {
            var result = new CurveLoop();
            //var origin = ts.Origin;
            //创建多边形
            //1 找到顶点
            var stepAngle = 2 * Math.PI / sideNum;
            var pointList = new List<XYZ>();
            for (int i = 0; i < sideNum; i++)
            {
                var currentAngle = i * stepAngle;
                var x = Math.Cos(currentAngle);
                var y = Math.Sin(currentAngle);
                var z = 0d;
                var point = new XYZ(x, y, z);
                pointList.Add(point);
            }
            for (int i = 0; i < sideNum; i++)
            {
                var pointA = pointList[i];
                var nextnum = i + 1;
                if (nextnum == sideNum) nextnum = 0;
                var pointB = pointList[nextnum];

                var line = Line.CreateBound(pointA, pointB);
                line = line.CreateTransformed(ts) as Line;
                //doc.FamilyCreate.NewModelCurve(line, doc.ActiveView.SketchPlane);
                result.Append(line);
            }
            return result;
        }
        /// <summary>
        /// 创建工字钢截面
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="height"></param>
        /// <param name="bride"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        //public static CurveLoop CreateSteelShape_C(Transform ts, double height, double bride, double d)
        //{

        //}

    }
}
