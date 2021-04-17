using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.Helpers;
namespace 唐僧解瓦.通用
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_MepLineUpDownVisble : IExternalCommand
    {
        public static bool Switch = false;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var app = uiapp.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            var cate_ducts = Category.GetCategory(doc, BuiltInCategory.OST_DuctCurves);
            var cate_pipes = Category.GetCategory(doc, BuiltInCategory.OST_PipeCurves);
            var cate_cabletras = Category.GetCategory(doc, BuiltInCategory.OST_CableTray);
            //var subCates1 = cate_ducts.SubCategories. 
            var cate1 = BuiltInCategory.OST_DuctCurvesRiseDrop;
            var cate2 = BuiltInCategory.OST_DuctCurvesDrop;
            var cate3 = BuiltInCategory.OST_PipeCurvesRiseDrop;
            var cate4 = BuiltInCategory.OST_PipeCurvesDrop;
            var cate5 = BuiltInCategory.OST_CableTrayDrop;
            var cate6 = BuiltInCategory.OST_CableTrayRiseDrop;

            var cateList = new List<Category>();
            cateList.Add(Category.GetCategory(doc, cate1));
            cateList.Add(Category.GetCategory(doc, cate2));
            cateList.Add(Category.GetCategory(doc, cate3));
            cateList.Add(Category.GetCategory(doc, cate4));
            cateList.Add(Category.GetCategory(doc, cate5));
            cateList.Add(Category.GetCategory(doc, cate6));
            var acview = uidoc.ActiveView;
            Switch = acview.GetCategoryHidden(cateList.ElementAt(0).Id);
            if (!Switch)
            {
                MessageBox.Show("已关闭");
            }
            else
            {
                MessageBox.Show("已开启");
            }
            doc.Invoke(m =>
            {
                foreach (Category category in cateList)
                {
                    if (Switch)
                        acview.SetCategoryHidden(category.Id, false);
                    else
                        acview.SetCategoryHidden(category.Id, true);
                }
            }, "隐藏/显示升降符号", true);
            return Result.Succeeded;
        }
    }
}
