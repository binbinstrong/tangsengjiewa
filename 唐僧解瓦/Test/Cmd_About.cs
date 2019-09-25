using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.RevitHelper;
using 唐僧解瓦.Test.UIs;

namespace 唐僧解瓦.Test
{
    /// <summary>
    /// 工具说明
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_About : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;

            AboutForm form = new AboutForm();
            form.Show(RevitWindowHelper.GetRevitWindow());

            return Result.Succeeded;
        }
    }
}
 