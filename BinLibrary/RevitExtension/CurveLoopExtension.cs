using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinLibrary.RevitExtension
{
    public static class CurveLoopExtension
    {
        public static CurveArray ToCurveArray(this CurveLoop cloop)
        {
            var result = new CurveArray();
            var enu = cloop.GetEnumerator();
            while (enu.MoveNext())
            {
                var curve = enu.Current;
                result.Append(curve);
            }
            return result;
        }
    }
}
