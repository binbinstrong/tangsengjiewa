using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace BinLibrary.RevitExtension
{
    public static class XYZExtension
    {
        /// <summary>
        /// 求向量的水平分量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static XYZ xyComponent(this XYZ vector)
        {
            return new XYZ(vector.X,vector.Y,0);
        }
        /// <summary>
        /// 求向量的yz面上的分量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static XYZ yzComponent(this XYZ vector)
        {
            return new XYZ(0, vector.Y, vector.Z);
        }
        /// <summary>
        /// 求向量xz面上的分量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static XYZ xzComponent(this XYZ vector)
        {
            return new XYZ(vector.X, 0, vector.Z);
        }
    }
}
