using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 隐藏族中元素
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]

    class Cmd_HideElementInFamilyDoc : IExternalCommand
    { 
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            if (!doc.IsFamilyDocument)
            {
                MessageBox.Show("当前不是族文档，请到族文档使用该命令");
            }

            var views = doc.TCollector<View>().Where(m=>!(m.IsTemplate));//doc.ViewCollector();

            FilteredElementCollector collector = new FilteredElementCollector(doc);

            var elelist = collector.WhereElementIsNotElementType();

            Transaction ts = new Transaction(doc, "显示族元素");
            try
            { 
                ts.Start();
                foreach (var view in views)
                {
                    if (view is ViewPlan || view is ViewSection || view is View3D)
                        //ele.IsHidden(acview);
                        //acview.HideElements(new List<ElementId>(){ele.Id});
                        foreach (var item in elelist)
                        {
                            if (item.IsHidden(view))
                            {
                                view.UnhideElements(new List<ElementId>() { item.Id });
                            }
                        }
                }
                ts.Commit();
            }
            catch (Exception)
            {
                if (ts.GetStatus() == TransactionStatus.Started)
                {
                    ts.RollBack();
                }
                //throw;
            }
            return Result.Succeeded;
        }
    }
}
