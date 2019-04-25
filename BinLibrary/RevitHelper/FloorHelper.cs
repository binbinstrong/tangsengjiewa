using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace BinLibrary.RevitHelper
{
    public static class FloorHelper
    {
        /// <summary>
        /// 获取楼板厚度(英制)
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public static double GetThick(this Floor floor)
        {
            return floor.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM).AsDouble();
        }
    }
}
