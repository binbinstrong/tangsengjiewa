using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.Test
{
    /// <summary>
    /// just for test
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_FlipTee:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;



            var ids = sel.GetElementIds();

            if (ids.Count>1||ids.Count==0)
            {
                return Result.Cancelled;
            }

            var id = sel.GetElementIds().First();

            var ele = id.GetElement(doc) as FamilyInstance;

             

            return Result.Succeeded;
        }
    }
}
