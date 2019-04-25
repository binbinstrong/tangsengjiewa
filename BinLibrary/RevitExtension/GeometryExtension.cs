using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace BinLibrary.RevitExtension
{
    public static class GeometryExtension
    {
 
        /// <summary>
        /// 获取category的名称如果是cad图纸那么就获取图层名称
        /// </summary>
        /// <param name="geometryobj"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string LayerName(this GeometryObject geometryobj ,Document doc)
        {
            var graphicsstyle = geometryobj.GraphicsStyleId.GetElement(doc) as GraphicsStyle;
            return graphicsstyle.GraphicsStyleCategory?.Name;
        }

    }
}
