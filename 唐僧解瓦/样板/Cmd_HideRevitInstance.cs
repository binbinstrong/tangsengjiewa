using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace 唐僧解瓦.样板
{
    /// <summary>
    /// 隐藏revit链接
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_HideRevitInstance:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            var acview = doc.ActiveView;

            var collector = new FilteredElementCollector(doc);
            var revitlinktypes = collector.OfClass(typeof(RevitLinkType)).ToList();


            Transaction ts = new Transaction(doc, "hide revitlink");
            ts.Start();
            acview.HideElements(revitlinktypes.Select(m => m.Id).ToList());
            ts.Commit();
            return Result.Succeeded;
        }
    }
}
