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
    public static class CurveExtension
    {

        //public static bool IsClockWise(this Curve c, XYZ clockNorm/*垂直钟表平面向上法向量*/)
        //{
        //    bool result = false;


        //    if (c.IsBound && (c is Arc))
        //    {

        //        XYZ pointonCurve = c.Evaluate(0.5, true);
        //        XYZ start = c.GetEndPoint(0);
        //        XYZ end = c.GetEndPoint(1);

        //        XYZ vector = start - pointonCurve;//pointonCurve - start;
        //        XYZ vector1 = end - pointonCurve;

        //        XYZ norm = vector.CrossProduct(vector1).Normalize();

        //        //未完待续
        //        //判断法向量与 钟表 法向量是否平行 若同向 则为逆时针方向 （false） 反之 则 为（true）

        //        double dotproductValue = norm.DotProduct(clockNorm);

        //        if (Math.Abs(dotproductValue + 1) < 0.000001)
        //        {
        //            result = false;
        //        }
        //        else if (Math.Abs(dotproductValue - 1) < 0.0000001)
        //        {
        //            result = true;
        //        }

        //    }

        //    return result;
        //}

        /// <summary>
        /// 根据计算法向量 判断顺时针
        /// </summary>
        /// <param name="c"></param>
        /// <param name="clockNorm"></param>
        /// <returns></returns>
        public static bool IsClockWise(this Curve c, XYZ clockNorm)
        {
            bool result = false;

            if (c.IsBound && c is Arc)
            {
                XYZ start = c.GetEndPoint(0);
                XYZ end = c.GetEndPoint(1);
                XYZ onCurvePo = c.Evaluate(0.5, true);

                XYZ vector1 = start - onCurvePo;
                XYZ vector2 = end - onCurvePo;

                XYZ vec_res = vector1.CrossProduct(vector2).Normalize();

                if (Math.Abs(vec_res.DotProduct(XYZ.BasisZ) - 1) < 0.000001)
                {
                    result = true;
                }
                if (Math.Abs(vec_res.DotProduct(XYZ.BasisZ) + 1) < 0.000001)
                {
                    result = false;
                }

            }

            return result;
        }
        public static IList<XYZ> Intersect(this Curve c, Face face,bool extendFlag=false)
        {
            Curve c1= c.Clone();
            if(extendFlag)
            c1.MakeUnbound();

            IList<XYZ> result = new List<XYZ>();

            IntersectionResultArray resultarray = new IntersectionResultArray();

            var comparisonResult = face.Intersect(c1, out resultarray);
            if (comparisonResult != SetComparisonResult.Disjoint)
            {
                if (resultarray != null)
                {
                    result.Add(resultarray.get_Item(0).XYZPoint);
                }
            }
            return result;
        }
        public static XYZ Intersect_cus(this Curve c, Curve c1)
        {
            XYZ result = null;
            IntersectionResultArray resultArray = new IntersectionResultArray();

            var comparisonResult = c.Intersect(c1, out resultArray);
            if (comparisonResult!=SetComparisonResult.Disjoint)
            {
                if (resultArray != null)
                    result = resultArray.get_Item(0).XYZPoint;
            }

            return result;
        }
         
    }
}
