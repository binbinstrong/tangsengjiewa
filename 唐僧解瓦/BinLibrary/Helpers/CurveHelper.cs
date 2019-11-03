using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace 唐僧解瓦.BinLibrary.Helpers
{
    public static class CurveHelper
    {
        /// <summary>
        /// 根据一点 和一条曲线 求得与 点到曲线连线相垂直的切线的切点
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="po"></param>
        /// <returns></returns>
        public static List<XYZ> GetPerpendicularPoint(Curve curve, XYZ po)
        {
            var result = default(List<XYZ>);

            //if (curve is Arc arc)
            //{
            //    arc.ComputeDerivatives()
            //}
            //else if (curve is Ellipse)
            //{


            //}else if (curve is NurbSpline)
            //{
                
            //}


            return result;
        }
    }
}
