using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    class Cmd_SelectLinkedPipes : IExternalCommand
    {
        private static bool eventSwitch;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;

            var acview = uidoc.ActiveView;
            var acview1 = doc.ActiveView;

            //uidoc.ActiveUIView();
            
            var uiviews = uidoc.GetOpenUIViews();

            //MessageBox.Show(uiviews.Count.ToString()+Environment.NewLine+
            //                uiviews[0].ViewId.GetElement(doc).Name + Environment.NewLine + 
            //                uiviews[1].ViewId.GetElement(doc).Name);
            
            //uidoc.ActiveUIView().GetZoomCorners();
            
            //var ele = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Pipe || m is FamilyInstance))
            //    .GetElement(doc);

            //var pipe = default(Pipe);
            //var pipefitting = default(FamilyInstance);

            //if (ele is Pipe) pipe = ele as Pipe;
            //else if(ele is FamilyInstance) pipefitting = ele as FamilyInstance;

            //var connectors = pipe.ConnectorManager.Connectors.Cast<Connector>().
             

            return Result.Succeeded;
        }
 
    }
}
