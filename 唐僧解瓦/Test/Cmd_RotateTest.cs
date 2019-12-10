using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.Test
{
    /// <summary>
    /// 旋转动画
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_RotateTest : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            var acview = doc.ActiveView;


            var pipe =
                sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Pipe)).GetElement(doc) as Pipe;
            var locationline = pipe.LocationLine();

            var startpo = locationline.StartPoint();

            var anxisline = Line.CreateUnbound(startpo, XYZ.BasisZ);

            Transaction ts = new Transaction(doc, "rotate");
            ts.Start();
            for (int i = 0; i < 20; i++)
            {

                Thread.Sleep(200);
 
                ElementTransformUtils.RotateElement(doc, pipe.Id, anxisline, Math.PI / 120);
                //doc.Regenerate();
                uidoc.RefreshActiveView();
                
            }
            ts.Commit();

            //var angle = locationline.Direction.AngleOnPlaneTo(XYZ.BasisX, XYZ.BasisZ);
            //MessageBox.Show(angle.RadiusToDegree().ToString());

            return Result.Succeeded;
        }
    }
}
