using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace 红瓦功能揭秘.BinLibrary.Extensions
{
    public static class XYZExtension
    {
        public static XYZ xyComponent(this XYZ po)
        {
            return new XYZ(po.X, po.Y, 0);
        }
    }
}
