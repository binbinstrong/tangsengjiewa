using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;
 using Microsoft.VisualBasic;
using FaceArray = Autodesk.Revit.DB.FaceArray;

namespace 唐僧解瓦.机电
{
    /// <summary>
    /// 管线随板
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_MepsAlongFloor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;

            //选择管线
            var mep = sel.PickObject(ObjectType.Element,
                doc.GetSelectionFilter(m => m is MEPCurve && !(m is InsulationLiningBase)),"选择管线").GetElement(doc) as MEPCurve;
             
            //选择楼板
            var floor =
                sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Floor),"选择楼板").GetElement(doc) as Floor;

            var bottomfaceref = HostObjectUtils.GetBottomFaces(floor).First();

            var bottomface = floor.GetGeometryObjectFromReference(bottomfaceref) as Face;

            var planarFace = (bottomface as PlanarFace);

            var planeNorm = planarFace.FaceNormal;
            var planeOrigin = planarFace.Origin;
            var plane = Plane.CreateByNormalAndOrigin(planeNorm, planeOrigin);

            Transform trs = Transform.Identity;
            trs.Origin = planeOrigin;
            trs.BasisX = planarFace.XVector;
            trs.BasisY = planarFace.YVector;
            trs.BasisZ = planeNorm;
             
            var inputboxStr = Microsoft.VisualBasic.Interaction.InputBox("输入距离楼板距离","距离","0");

            var distance = default(double);
            
            var parseresult =double.TryParse(inputboxStr, out distance);
            if (!parseresult) return Result.Cancelled;

            distance = distance.MetricToFeet();

            var locationline = mep.LocationLine();
            var startpo = locationline.StartPoint();
            var endpo = locationline.EndPoint();

            var startpo_intrans = trs.Inverse.OfPoint(startpo);
            var endpo_intrans = trs.Inverse.OfPoint(endpo);

            var startpo_proj = new XYZ(startpo_intrans.X, startpo_intrans.Y,0);
            var endpo_proj = new XYZ(endpo_intrans.X, endpo_intrans.Y, 0); ;

            startpo_proj = trs.OfPoint(startpo_proj);
            endpo_proj = trs.OfPoint(endpo_proj);
             
            var dir = (endpo - endpo_proj).Normalize();

            var newstart = startpo_proj + dir * distance;
            var newend = endpo_proj + dir * distance;

            Transaction ts = new Transaction(doc, "更改管线到楼板距离");
             
            ts.Start();

            (mep.Location as LocationCurve).Curve = Line.CreateBound(newstart, newend);

            ts.Commit();
             
            return Result.Succeeded;
        }
    }
}
