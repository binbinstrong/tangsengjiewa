using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class PipeExtension
    {
        public static FamilyInstance ElbowConnect(this Pipe pipe1, Pipe pipe2)
        {
            var line1 = pipe1.LocationLine();
            var line2 = pipe2.LocationLine();

            var line1copy = line1.Clone() as Line;
            var line2copy = line2.Clone() as Line;
            line2copy.MakeUnbound();
            line1copy.MakeUnbound();

            var intersection = line1copy.Intersect_cus(line2copy);

            if (intersection == null) return null;

            var newline1 = default(Line);
            if (line1.StartPoint().DistanceTo(intersection) <= line1.EndPoint().DistanceTo(intersection))
            {
                newline1 = Line.CreateBound(intersection, line1.EndPoint());
            }
            else
            {
                newline1 = Line.CreateBound(line1.StartPoint(), intersection);
            }

            var newline2 = default(Line);
            if (line2.StartPoint().DistanceTo(intersection) <= line2.EndPoint().DistanceTo(intersection))
            {
                newline2 = Line.CreateBound(intersection, line2.EndPoint());
            }
            else
            {
                newline2 = Line.CreateBound(line2.StartPoint(), intersection);
            }

            (pipe1.Location as LocationCurve).Curve = newline1;
            (pipe2.Location as LocationCurve).Curve = newline2;

            var doc = pipe1.Document;
            doc.Regenerate();

            var con1 = pipe1.ConnectorManager.Connectors.Cast<Connector>()
                .Where(m => m.ConnectorType == ConnectorType.Curve || m.ConnectorType == ConnectorType.End)
                .Where(m => m.Origin.IsAlmostEqualTo(intersection)).FirstOrDefault();

            var con2 = pipe2.ConnectorManager.Connectors.Cast<Connector>()
                .Where(m => m.ConnectorType == ConnectorType.Curve || m.ConnectorType == ConnectorType.End)
                .Where(m => m.Origin.IsAlmostEqualTo(intersection)).FirstOrDefault();

            var result = doc.Create.NewElbowFitting(con1, con2);

            return result;
        }
    }
}
