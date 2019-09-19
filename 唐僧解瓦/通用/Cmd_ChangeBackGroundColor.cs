using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.Extensions;
using Color = System.Windows.Media.Color;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 更改视图背景
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_ChangeBackGroundColor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;

            uiapp.Application.BackgroundColor = uiapp.Application.BackgroundColor.InversColor();
            
            return Result.Succeeded;
        }
    }
}
