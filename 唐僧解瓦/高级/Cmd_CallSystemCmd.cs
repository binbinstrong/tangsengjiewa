using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using UIFrameworkServices;

namespace 唐僧解瓦.高级
{
    /// <summary>
    /// 调用revit 本身命令 模拟手动操作
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_CallSystemCmd:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uidoc = commandData.Application.ActiveUIDocument;

            var sel = uidoc.Selection;
            var doc = uidoc.Document;

            var acview = doc.ActiveView as ViewSection;

            
            var ele = sel.PickObject(ObjectType.Element);
             
            sel.SetElementIds(new List<ElementId>() {ele.ElementId}); //选中剖面符号

            CommandHandlerService.invokeCommandHandler("ID_SECTION_GAP"); //打断剖面符号

           
            sel.SetElementIds(new List<ElementId>());

            return Result.Succeeded;
        }
    }
}
