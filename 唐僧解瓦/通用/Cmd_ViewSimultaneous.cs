using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using 唐僧解瓦.BinLibrary.Helpers;
using 唐僧解瓦.通用.UIs;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 视图同步
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_ViewSimultaneous : IExternalCommand
    {
        public static ElementId Id_view1;
        public static ElementId Id_view2;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection; 

            var planviewfilter = new ElementClassFilter(typeof(ViewPlan));
            var view3dfilter = new ElementClassFilter(typeof(View3D));
            var viewdraftingfilter = new ElementClassFilter(typeof(ViewDrafting));

            var logicfilter = new LogicalOrFilter(new List<ElementFilter>(){planviewfilter,view3dfilter,viewdraftingfilter});

            var views = new FilteredElementCollector(doc).WhereElementIsNotElementType().WherePasses(logicfilter)
                .Where(m => !(m as View).IsTemplate).ToList();
             
            //1.选择 联动的两个窗口
            var selector = ViewSemutaneousSelector.Instance;
            selector.combo1.ItemsSource = views;
            selector.combo1.DisplayMemberPath = "Name";
            selector.combo1.SelectedIndex = 0;

            views.Remove(selector.combo1.SelectionBoxItem as View);

            selector.combo2.ItemsSource = views  ;
            selector.combo2.DisplayMemberPath = "Name";
            selector.combo2.SelectedIndex = 0;
            selector.ShowDialog();

            Id_view1 = (selector.combo1.SelectionBoxItem as View).Id;
            Id_view2 = (selector.combo2.SelectionBoxItem as View).Id;
             

            //激活两个窗口 并关闭其余窗口
            uidoc.ActiveView = doc.GetElement(Id_view2) as View;
            uidoc.ActiveView = doc.GetElement(Id_view1) as View;
            var uiviews = uidoc.GetOpenUIViews();
            foreach (var uiview in uiviews)
            {
                if (uiview.ViewId == Id_view1 || uiview.ViewId == Id_view2)
                {
                    continue;
                }
                else
                {
                    uiview.Close();
                }
            }

            //2.平铺窗口
            var tileviewcommand = RevitCommandId.LookupPostableCommandId(PostableCommand.TileViews);
            uiapp.PostCommand(tileviewcommand);

            //在空闲事件中 同步视图
            uiapp.Idling += OnIdling;
            //视图激活事件中 检测目标视图是否关闭 若关闭则退出视图同步功能
            uiapp.ViewActivated += OnviewActivated;

            return Result.Succeeded;
        }

        private void OnviewActivated(object sender, ViewActivatedEventArgs e)
        {
            var uiapp = sender as UIApplication;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            var acview = doc.ActiveView;
            if (acview.Id != Id_view1 && acview.Id != Id_view2)
            {
                uiapp.Idling -= OnIdling;
            }
        }

        private void OnIdling(object sender, IdlingEventArgs e)
        {
            var uiapp = sender as UIApplication;

            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;

            //var acview = doc.ActiveView;
            var view1 = doc.GetElement(Id_view1) as View;
            var view2 = doc.GetElement(Id_view2) as View;

            var uiview1 = uidoc.GetOpenUIViews().Where(m => m.ViewId == Id_view1).FirstOrDefault();
            var uiview2 = uidoc.GetOpenUIViews().Where(m => m.ViewId == Id_view2).FirstOrDefault();


            var viewcorners = uiview1.GetZoomCorners();
            var corner1 = viewcorners.First();
            var corner2 = viewcorners.Last();
             
            //LogHelper.LogWrite(corner1.ToString()+"::"+corner2.ToString(),@"c:\aaa.txt");
            uiview2.ZoomAndCenterRectangle(corner1, corner2);

        }
    }
}
