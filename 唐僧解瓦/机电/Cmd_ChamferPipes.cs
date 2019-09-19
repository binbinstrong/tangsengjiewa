using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧揭瓦.BinLibrary.Extensions;


namespace 唐僧揭瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]

    class Cmd_ChamferPipes : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            View acview = uidoc.ActiveView;

            var chamferDis = 300d; //倒角距离

            var ref1 = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Pipe));
            var ref2 = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Pipe));
            var pipe1 = ref1.GetElement(doc) as Pipe;
            var pipe2 = ref2.GetElement(doc) as Pipe;

            var pickpoint1 = ref1.GlobalPoint;
            var pickpoint2 = ref2.GlobalPoint;

            var line1 = (pipe1.Location as LocationCurve).Curve as Line;
            var line2 = (pipe2.Location as LocationCurve).Curve as Line;

            //取得拾取点在管道轴线上的投影点
            pickpoint1 = pickpoint1.ProjectToXLine(line1);
            pickpoint2 = pickpoint2.ProjectToXLine(line2);

            var line1copy = line1.Clone();
            var line2copy = line2.Clone();
            line1copy.MakeUnbound();
            line2copy.MakeUnbound();
            //取得交点
            var intersectionresult = default(IntersectionResult);
            IntersectionResultArray resultarr = default(IntersectionResultArray);
            var comparisonResult = line1copy.Intersect(line2copy, out resultarr);
            if (comparisonResult != SetComparisonResult.Disjoint)
            {
                intersectionresult = resultarr.get_Item(0);
            }
            var intersection = intersectionresult.XYZPoint;

            var dir1 = (pickpoint1 - intersection).Normalize();
            var dir2 = (pickpoint2 - intersection).Normalize();

            var chamferpoint1 = intersection + dir1 * chamferDis.MetricToFeet();
            var chamferpoint2 = intersection + dir2 * chamferDis.MetricToFeet();
             
            Transaction ts = new Transaction(doc, "管线倒角");
            try
            {
                ts.Start();

                pipe1 = changeSizeOfpipe(pipe1, dir1, chamferpoint1);
                pipe2 = changeSizeOfpipe(pipe2, dir2, chamferpoint2);

                var resultpipeid = ElementTransformUtils.CopyElements(doc, new Collection<ElementId>() {pipe1.Id}, new XYZ()).First();
                var resultpipe = resultpipeid.GetElement(doc) as Pipe ;
                (resultpipe.Location as LocationCurve).Curve = Line.CreateBound(chamferpoint1, chamferpoint2);

                //连接管道
                var con1 = pipe1.ConnectorManager.Connectors.Cast<Connector>().Where(m => m?.Origin != null && m.Origin.IsAlmostEqualTo(chamferpoint1)).First();
                var con2 = resultpipe.ConnectorManager.Connectors.Cast<Connector>().Where(m => m?.Origin != null && m.Origin.IsAlmostEqualTo(chamferpoint1)).First();

                doc.Create.NewElbowFitting(con1, con2);

                var con3 = pipe2.ConnectorManager.Connectors.Cast<Connector>().Where(m => m?.Origin != null && m.Origin.IsAlmostEqualTo(chamferpoint2)).First();
                var con4 = resultpipe.ConnectorManager.Connectors.Cast<Connector>().Where(m => m?.Origin != null && m.Origin.IsAlmostEqualTo(chamferpoint2)).First();

                doc.Create.NewElbowFitting(con3, con4);

                ts.Commit();
            }
            catch (Exception)
            {
                if (ts.GetStatus() == TransactionStatus.Started)
                {
                    ts.RollBack();
                }
                //throw;
            }
            return Result.Succeeded;
        }

        private Pipe changeSizeOfpipe(Pipe pipe, XYZ dir, XYZ chamferpoint1)
        {
            var locationline = (pipe.Location as LocationCurve).Curve as Line;
            var startpo = locationline.GetEndPoint(0);
            var endpo = locationline.GetEndPoint(1);

            var stablepoint = default(XYZ);
            if (Math.Abs((startpo - endpo).Normalize().DotProduct(dir) - 1) < 1e-6)
            {
                stablepoint = startpo;
            }
            else
            {
                stablepoint = endpo;
            }
            var newstart = stablepoint.IsAlmostEqualTo(startpo) ? stablepoint : chamferpoint1;
            var newend = stablepoint.IsAlmostEqualTo(endpo) ? stablepoint : chamferpoint1;
            (pipe.Location as LocationCurve).Curve = Line.CreateBound(newstart, newend);
            return pipe;
        }
    }
}
