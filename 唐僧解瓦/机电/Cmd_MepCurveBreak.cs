using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧揭瓦.BinLibrary.Extensions;

namespace 唐僧揭瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    class Cmd_MepCurveBreak : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            View acview = uidoc.ActiveView;
            //UIView acuivew = uidoc.Activeuiview();
            var eleref = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is MEPCurve),
                "拾取管线打断点");
            var pickpoint = eleref.GlobalPoint;
            var mep = eleref.GetElement(doc) as MEPCurve;
            var locationline = (mep.Location as LocationCurve).Curve as Line;
            pickpoint = pickpoint.ProjectToXLine(locationline); //ProjectToXLine 方法作用是 获取点投影到直线上的点
            var startpo = locationline.StartPoint();
            var endpo = locationline.EndPoint();
            var newstart = startpo;
            var newend = pickpoint;
            var newstart1 = pickpoint;
            var newend1 = endpo;
             
            Transaction ts = new Transaction(doc, "***********");
            try
            {
                ts.Start();
                var newmep = ElementTransformUtils.CopyElement(doc, mep.Id, new XYZ()).First().GetElement(doc) as MEPCurve;
                chagnemeplen(mep,newstart, newend);
                chagnemeplen(newmep, newstart1, newend1); 
                ts.Commit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                if (ts.GetStatus() == TransactionStatus.Started)
                {
                    ts.RollBack();
                }
                //throw;
            }
            return Result.Succeeded;
        }
        void chagnemeplen(MEPCurve mep,XYZ p1,XYZ p2)
        {
            var locationline = (mep.Location as LocationCurve).Curve as Line;
            (mep.Location as LocationCurve).Curve = Line.CreateBound(p1, p2);
        }
    }
}
