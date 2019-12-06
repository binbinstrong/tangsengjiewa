using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using 唐僧解瓦.样板.UIs;

namespace 唐僧解瓦.样板
{
    /// <summary>
    /// 复制视图裁剪
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_CopyViewCut : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var acview = doc.ActiveView;

            var collecor = new FilteredElementCollector(doc);
            var planviews = collecor.OfClass(typeof(ViewPlan)).Where(m => !(m as ViewPlan).IsTemplate).OrderBy(m => m.Name);


            ViewSelector selector = new ViewSelector();
            selector.sourceView.ItemsSource = planviews;
            selector.sourceView.DisplayMemberPath = "Name";
            selector.sourceView.SelectedIndex = 0;

            selector.targetViewList.ItemsSource = planviews;
            selector.targetViewList.DisplayMemberPath = "Name";

            selector.ShowDialog();

            var sourceview = selector.sourceView.SelectionBoxItem as View;
            var targetviews = selector.targetViewList.SelectedItems.Cast<ViewPlan>();

            Transaction ts = new Transaction(doc, "复制裁剪");
            ts.Start();

            var boundingbox = sourceview.CropBox;

            foreach (var targetview in targetviews)
            {
                targetview.CropBox = boundingbox;
                var para_crop = targetview.get_Parameter(BuiltInParameter.VIEWER_CROP_REGION);
                var para_crop_visible = targetview.get_Parameter(BuiltInParameter.VIEWER_CROP_REGION_VISIBLE);
                para_crop_visible.Set(1);
                para_crop.Set(1);
            }

            ts.Commit();

            selector.Close();
            return Result.Succeeded;
        }
    }
}
