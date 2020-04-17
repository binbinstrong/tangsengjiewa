using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.RevitHelper;
using 唐僧解瓦.高级.UIs;

namespace 唐僧解瓦.高级
{
    [Transaction(TransactionMode.Manual)]
    class Cmd_DragTest:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            
            DragForm toolform = DragForm.Instance;
            toolform.Show(RevitWindowHelper.GetRevitWindow());
            

            return Result.Succeeded;
        }
    }
}
