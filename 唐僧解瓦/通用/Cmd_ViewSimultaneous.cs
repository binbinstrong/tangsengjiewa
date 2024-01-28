﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System.Collections.Generic;
using System.Linq;
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
        public static ElementId Id_view3;
        public static ElementId Id_view4;
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

            //第三个 和 第四个视图设置

            views.Remove(selector.combo2.SelectionBoxItem as View);
            selector.combo3.ItemsSource = views;
            selector.combo3.DisplayMemberPath = "Name";
            selector.combo3.SelectedIndex = 0;


            views.Remove(selector.combo3.SelectionBoxItem as View);
            selector.combo4.ItemsSource = views;
            selector.combo4.DisplayMemberPath = "Name";
            selector.combo4.SelectedIndex = 0;


            selector.ShowDialog();


            Id_view1 = (selector.combo1.SelectionBoxItem as View).Id;
            Id_view2 = (selector.combo2.SelectionBoxItem as View).Id;
            Id_view3 = (selector.combo3.SelectionBoxItem as View).Id;
            Id_view4 = (selector.combo4.SelectionBoxItem as View).Id;

            
            //激活两个窗口 并关闭其余窗口
            uidoc.ActiveView = doc.GetElement(Id_view2) as View;
            uidoc.ActiveView = doc.GetElement(Id_view1) as View;
            uidoc.ActiveView = doc.GetElement(Id_view3) as View;
            uidoc.ActiveView = doc.GetElement(Id_view4) as View;

            var uiviews = uidoc.GetOpenUIViews();
            foreach (var uiview in uiviews)
            {
                if (uiview.ViewId == Id_view1 || uiview.ViewId == Id_view2 || uiview.ViewId == Id_view3 || uiview.ViewId == Id_view4)
                {
                    continue;
                }
                else
                {
                    uiview.Close();
                }
            }

            //2.平铺窗口
            var tileviewcommand =
                default(RevitCommandId); // RevitCommandId.LookupPostableCommandId(PostableCommand.TileViews);
#if Revit2018
            tileviewcommand = RevitCommandId.LookupPostableCommandId(PostableCommand.TileWindows);
#endif

#if Revit2016
            tileviewcommand = RevitCommandId.LookupPostableCommandId(PostableCommand.TileWindows);
#endif
            uiapp.PostCommand(tileviewcommand);

            //在空闲事件中 同步视图
            uiapp.Idling += OnIdling;
            //视图激活事件中 检测目标视图是否关闭 若关闭则退出视图同步功能
            uiapp.ViewActivated += OnviewActivated;

            return Result.Succeeded;
        }

        private void OnviewActivated(object sender, ViewActivatedEventArgs e)
        {

            var a = default(double);

            var uiapp = sender as UIApplication;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            var acview = doc.ActiveView;
            if (acview.Id != Id_view1 && acview.Id != Id_view2 && acview.Id != Id_view3 && acview.Id !=Id_view4 )
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

            var acview = doc.ActiveView;

            var view1 = doc.GetElement(Id_view1) as View;
            var view2 = doc.GetElement(Id_view2) as View;

            //var view3 = doc.GetElement() as View;
            //var view4 = doc.GetElement() as View;

            var acuiview = uidoc.GetOpenUIViews().Where(m => m.ViewId == acview.Id).FirstOrDefault();
             
            var views = new List<ElementId>() {Id_view1, Id_view2, Id_view3, Id_view4};

            var leftviews = views.Where(m => m != acview.Id).ToList();
            var uiviews =new List<UIView>();

            uiviews =  uidoc.GetOpenUIViews().Where(m => leftviews.Contains(m.ViewId)).ToList();


            //var uiview1 = uidoc.GetOpenUIViews().Where(m => m.ViewId == Id_view1  ).FirstOrDefault();
            //var uiview2 = uidoc.GetOpenUIViews().Where(m => m.ViewId == Id_view2).FirstOrDefault();
            //var uiview3 = uidoc.GetOpenUIViews().Where(m => m.ViewId == Id_view3).FirstOrDefault();
            //var uiview4 = uidoc.GetOpenUIViews().Where(m => m.ViewId == Id_view4).FirstOrDefault();

            var viewcorners = acuiview.GetZoomCorners();
            var corner1 = viewcorners.First();
            var corner2 = viewcorners.Last();
             
            //LogHelper.LogWrite(corner1.ToString()+"::"+corner2.ToString(),@"c:\aaa.txt");

            foreach (UIView uiview in uiviews)
            {
                uiview.ZoomAndCenterRectangle(corner1, corner2);
            }

        }
    }
}
