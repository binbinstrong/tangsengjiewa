using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace BinLibrary.RevitHelper
{
    //还需要详细修改

    /// <summary>
    /// 内建模型帮助类 用于创建基本几何体
    /// </summary>
    public static class DirectShapeHelper
    {

        //public static void CreateDirectShape(Document doc, Category cate, Solid solid)
        //{
        //    //GeometryCreationUtilities.CreateRevolvedGeometry()
        //    var ds = DirectShape.CreateElement(doc, cate.Id, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),Transform.Identity);
        //    ds.AppendShape(new List<GeometryObject>() { solid });
        //}
        /// <summary>
        /// 创建球体
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public static void CreateSphere(Document doc, Category cate, XYZ center, double radius)
        {
            Solid solid = null;
            // solid = GeometryCreationUtilities.CreateRevolvedGeometry()
            //CreateDirectShape(doc, cate, solid);
        }
        /// <summary>
        /// 创建圆锥体
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        public static void CreateCone(Document doc, XYZ center, double radius, double height)
        {

        }

        /// <summary>
        /// 创建圆柱体
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="radius"></param>
        public static void CreateCylinder(Document doc, XYZ p1, XYZ p2, double radius)
        {

        }

    }
}
