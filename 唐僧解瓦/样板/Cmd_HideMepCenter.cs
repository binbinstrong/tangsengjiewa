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
            var ductrdrop = Category.GetCategory(doc, BuiltInCategory.OST_DuctCurvesDrop);

            var cablerizedrop = Category.GetCategory(doc, BuiltInCategory.OST_CableTrayRiseDrop);
            var cabledrop = Category.GetCategory(doc, BuiltInCategory.OST_CableTrayDrop);

            var conduitrizedrop = Category.GetCategory(doc, BuiltInCategory.OST_ConduitRiseDrop);
            var conduitdrop = Category.GetCategory(doc, BuiltInCategory.OST_ConduitDrop);

            var piperizedrop = Category.GetCategory(doc, BuiltInCategory.OST_PipeCurvesRiseDrop);
            var pipedrop = Category.GetCategory(doc, BuiltInCategory.OST_PipeCurvesDrop);

            doc.Invoke(m =>
            {
#if Revit2016
                acview.SetVisibility(ductrizedrop, false);
                acview.SetVisibility(cablerizedrop, false);
                acview.SetVisibility(conduitrizedrop, false);
                acview.SetVisibility(piperizedrop, false);

                acview.SetVisibility(ductrdrop, false);
                acview.SetVisibility(cabledrop, false);
                acview.SetVisibility(conduitdrop, false);
                acview.SetVisibility(pipedrop, false);
#endif
#if Revit2018
                acview.SetCategoryHidden(ductrizedrop.Id,false);
                acview.SetCategoryHidden(cablerizedrop.Id,false);
                acview.SetCategoryHidden(conduitrizedrop.Id,false);
                acview.SetCategoryHidden(piperizedrop.Id,false);

                acview.SetCategoryHidden(ductrdrop.Id,false);
                acview.SetCategoryHidden(cabledrop.Id,false);
                acview.SetCategoryHidden(conduitdrop.Id,false);
                acview.SetCategoryHidden(pipedrop.Id,false);
#endif

            }, "隐藏立面中心线");


            return Result.Succeeded;
        }
    }
}
