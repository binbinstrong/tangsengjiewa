using System;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class DoubleExtension
    {
        public static double precision = 1e-6;
        /// <summary>
        /// 判断双精度数值相等 小于1e-6 就认为相等
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static bool IsEqual(this double num1, double num2)
        {
            double subtract = num1 - num2;
            if (Math.Abs(subtract) < precision)
            {
                return true;
            }
            return false;
        }
        public static double MetricToFeet(this double double1)
        {
            return double1 / 304.8;
        }
        public static double FeetToMetric(this double double1)
        {
            return double1 * 304.8;
        }

        public static double RadiusToDegree(this double double1)
        {
            return double1 * 180 / Math.PI;
        }

        public static double DegreeToRadius(this double double1)
        {
            return double1 * Math.PI / 180;
        }
    }
}
