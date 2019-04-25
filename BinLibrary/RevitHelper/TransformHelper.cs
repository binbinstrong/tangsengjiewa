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

namespace BinLibrary.RevitHelper
{
    public static class TransformHelper
    {

        //public static Line OfLine(this Transform ts, Line line)
        //{
        //    Line result = null;
        //    if (!line.IsBound)
        //    {

        //        XYZ origin = line.Origin;
        //        XYZ dir = line.Direction;

        //        XYZ neworigin = ts.OfPoint(origin);
        //        XYZ newdir = ts.OfVector(dir);

        //        result = Line.CreateUnbound(neworigin, newdir);

        //        //throw new Exception("线不是有长度的线段");
        //    }
        //    else
        //    {
                 
        //        XYZ end1 = line.GetEndPoint(0);
        //        XYZ end2 = line.GetEndPoint(1);

        //        XYZ newend1 = ts.OfPoint(end1);
        //        XYZ newend2 = ts.OfPoint(end2);


        //        result = Line.CreateBound(newend1, newend2);
        //    }
        //    return result;
        //}

        public static Curve OfCurve(this Transform ts, Curve c)
        {
            Curve result = default(Curve);
            result = c.CreateTransformed(ts);
            return result;

        }
    }
}
