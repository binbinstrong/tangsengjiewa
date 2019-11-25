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
    /// 打断剖面线
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_SectionGap:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uidoc = commandData.Application.ActiveUIDocument;
            var sel = uidoc.Selection;


            var eleId = sel.PickObject(ObjectType.Element,
                uidoc.Document.GetSelectionFilter(m =>
                {
                    return m.Category.Id == new ElementId(BuiltInCategory.OST_Viewers);

                })).ElementId;

            string commandId = "ID_SECTION_GAP";

            sel.SetElementIds(new List<ElementId>() {eleId});

            CommandHandlerService.invokeCommandHandler(commandId);

            return Result.Succeeded;
        }
    }
}
