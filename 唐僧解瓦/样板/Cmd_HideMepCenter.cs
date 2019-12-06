using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.样板
{
    /// <summary>
    /// 隐藏管线立面中心线
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_HideMepCenter:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;

            var acview = doc.ActiveView;

            var ductrizedrop = Category.GetCategory(doc, BuiltInCategory.OST_DuctCurvesRiseDrop);
            var cablerizedrop = Category.GetCategory(doc, BuiltInCategory.OST_CableTrayRiseDrop);
            var conduitrizedrop = Category.GetCategory(doc, BuiltInCategory.OST_ConduitRiseDrop);
            var piperizedrop = Category.GetCategory(doc, BuiltInCategory.OST_PipeCurvesRiseDrop);

            doc.Invoke(m =>
            {
#if Revit2016
                acview.SetVisibility(ductrizedrop, false);
                acview.SetVisibility(cablerizedrop, false);
                acview.SetVisibility(conduitrizedrop, false);
                acview.SetVisibility(piperizedrop, false);
#endif
#if Revit2019
                acview.SetCategoryHidden(ductrizedrop.Id,false);
                acview.SetCategoryHidden(cablerizedrop.Id,false);
                acview.SetCategoryHidden(conduitrizedrop.Id,false);
                acview.SetCategoryHidden(piperizedrop.Id,false);
#endif

            }, "隐藏立面中心线");


            return Result.Succeeded;
        }
    }
}
