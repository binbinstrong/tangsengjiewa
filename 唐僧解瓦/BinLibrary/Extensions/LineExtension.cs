
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 唐僧解瓦.BinLibrary.Extensions
{
   public static class LineExtension
    {
        public static XYZ StartPoint(this Line line)
        {
            if (line.IsBound)
                return line.GetEndPoint(0);
            return null;
        }
        public static XYZ EndPoint(this Line line)
        {
            if (line.IsBound)
                return line.GetEndPoint(1);
            return null;
        }

        public static XYZ Intersect_cus(this Line line, Plane p)
        {
            var lineorigin = line.Origin;
            var linedir = line.Direction;

            var pointOnLine = lineorigin + linedir;

            var trans = Transform.Identity;
            trans.Origin = p.Origin;
            trans.BasisX = p.XVec;
            trans.BasisY = p.YVec;
            trans.BasisZ = p.Normal;

            var point1 = lineorigin;
            var point2 = pointOnLine;

            var point1Intrans = trans.Inverse.OfPoint(point1);
            var point2Intrans = trans.Inverse.OfPoint(point2);
            
            point1Intrans = new XYZ(point1Intrans.X,point1Intrans.Y,0);
            point2Intrans = new XYZ(point2Intrans.X,point2Intrans.Y,0);

            var point1Inworld = trans.OfPoint(point1Intrans);
            var point2Inworld = trans.OfPoint(point2Intrans);

            var newlineInPlan = Line.CreateBound(point1Inworld, point2Inworld);

            var unboundnewLine = newlineInPlan.Clone() as Line;
            unboundnewLine.MakeUnbound();

            var unboundOriginalLine = line.Clone() as Line;
            unboundOriginalLine.MakeUnbound();

            return unboundnewLine.Intersect_cus(unboundOriginalLine);
        }

        public static XYZ Intersect_cus(this Line line1, Line line2)
        {
            var compareResulst = line1.Intersect(line2, out IntersectionResultArray intersectResult);
           
            if(compareResulst!=SetComparisonResult.Disjoint)
            {
                var result = intersectResult.Cast<XYZ>().First();
                return result;
            }

            return null;
        }
    }
}
