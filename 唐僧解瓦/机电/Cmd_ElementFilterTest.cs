using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.BinLibrary.Helpers;
using 唐僧解瓦.机电.ToolUIs;

namespace 唐僧解瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    class Cmd_ElementFilterTest : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            
            var FilterWin = new FilterWindow(doc);

            FilterWin.ShowDialog();
            

            return Result.Succeeded;
        }
    }
}
