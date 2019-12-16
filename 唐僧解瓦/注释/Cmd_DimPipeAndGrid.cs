using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace 唐僧解瓦.注释
{
    /// <summary>
    /// 标注管道和轴线距离  未完成
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_DimPipeAndGrid:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var acview = doc.ActiveView;

            var sel = uidoc.Selection;

             



            return Result.Succeeded;
        }
    }
}
