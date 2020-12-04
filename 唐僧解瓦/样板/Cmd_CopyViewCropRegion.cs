using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using 唐僧解瓦.样板.UIs;
using View = Autodesk.Revit.DB.View;

namespace 唐僧解瓦.样板
{
    /// <summary>
    /// 复制视图裁剪
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_CopyViewCropRegion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            try
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


                //MessageBox.Show(boundingbox.Max.ToString() + Environment.NewLine+ 
                //    boundingbox.Min.ToString());

                foreach (var targetview in targetviews)
                {
                    var boundingbox1 = new BoundingBoxXYZ();
                    boundingbox1.Transform = targetview.CropBox.Transform;
                    boundingbox1.Max = boundingbox.Max;
                    boundingbox1.Min = boundingbox.Min;

                    targetview.CropBox = boundingbox1;

                    var para_crop = targetview.get_Parameter(BuiltInParameter.VIEWER_CROP_REGION);
                    var para_crop_visible = targetview.get_Parameter(BuiltInParameter.VIEWER_CROP_REGION_VISIBLE);
                    para_crop.Set(1);
                    para_crop_visible.Set(1);
                }

                ts.Commit();

                selector.Close();
            }
            catch (Exception e)
            {
                return Result.Cancelled;
            }
            return Result.Succeeded;
        }
    }
}
