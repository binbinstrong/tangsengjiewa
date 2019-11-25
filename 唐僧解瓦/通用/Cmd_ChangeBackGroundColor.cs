using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using 唐僧解瓦.BinLibrary.Extensions;
using Color = System.Drawing.Color;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 更改视图背景颜色
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

            ColorDialog colordialog = new ColorDialog();
            var colorResult = colordialog.ShowDialog();
            var targetColor = default(Color);
            if (colorResult == DialogResult.OK)
            {
                targetColor = colordialog.Color;
            }
            else
            {
                return Result.Cancelled;
            }

            uiapp.Application.BackgroundColor = targetColor.ToRvtColor();// uiapp.Application.BackgroundColor.InversColor();

            return Result.Succeeded;
        }
    }
}
