using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using UIFrameworkServices;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.注释
{
    /// <summary>
    /// 翻转尺寸线
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_FlipDimension:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;


            var dim =
                sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Dimension))
                    .GetElement(doc) as Dimension;

            sel.SetElementIds(new List<ElementId>(){dim.Id});
            CommandHandlerService.invokeCommandHandler("ID_FLIP_DIMENSION_DIRECTION");

            return Result.Succeeded;
        }
    }
}
