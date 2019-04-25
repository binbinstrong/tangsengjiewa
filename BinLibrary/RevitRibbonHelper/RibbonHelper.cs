using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Windows;
using UIFramework;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

/// <summary>
/// 注意区分RibbonPanel 的命名空间 一个是Autodesk.Windows  另一个是Autodesk.Revit.UI
/// </summary>
namespace BinLibrary.RevitRibbonHelper
{
    public static class RibbonHelper
    {
        /// <summary>
        /// 获取RibbonControl主控件
        /// </summary>
        /// <returns></returns>
        public static RevitRibbonControl GetMainControl()
        {
            return RevitRibbonControl.RibbonControl;
        }

        /// <summary>
        /// 获取指定RibbonPanel
        /// </summary>
        /// <param name="tabtitle"></param>
        /// <param name="panelAutoName"></param>
        /// <returns></returns>
        public static RibbonPanel GetRibbonPanel_AutoName(string tabtitle, string panelAutoName)
        {
            RibbonPanel result = null;
            var mainRibbonControl = RevitRibbonControl.RibbonControl;
            var tabs = mainRibbonControl.Tabs;
            foreach (RibbonTab ribbonTab in tabs)
            {
                var tabid = ribbonTab.Id;
                var title = ribbonTab.Title;
                if (tabtitle != title) continue;
                if (tabtitle == title)
                {

                    var panels = ribbonTab.Panels;
                    foreach (Autodesk.Windows.RibbonPanel ribbonPanel in panels)
                    {
                        var source = ribbonPanel.Source;
                        var sourceAutoname = source.AutomationName;
                        var sourceId = source.Id;

                        if (sourceAutoname == panelAutoName)
                        {
                            var mypanel = default(RibbonPanel);
                            //反射获取panel
                            var typ = typeof(RibbonPanel);
                            ConstructorInfo t1Constructor = typ.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
                            Object reflectPanel = t1Constructor.Invoke(new object[] { ribbonPanel, tabid });
                            mypanel = (RibbonPanel)reflectPanel;
                            mypanel.Visible = true;
                            mypanel.Enabled = true;
                            result = mypanel;
                            break;
                        }
                    }
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取指定RibbonPanel
        /// </summary>
        /// <param name="tabid"></param>
        /// <param name="panelId"></param>
        /// <returns></returns>
        public static RibbonPanel GetRibbonPanel_Id(string tabid, string panelId)
        {
            RibbonPanel result = null;
            var mainRibbonControl = RevitRibbonControl.RibbonControl;
            var tabs = mainRibbonControl.Tabs;
            foreach (RibbonTab ribbonTab in tabs)
            {

                var ribbontabId = ribbonTab.Id;
                var title = ribbonTab.Title;
                if (ribbontabId != tabid) continue;
                if (ribbontabId == tabid)
                {
                    var panels = ribbonTab.Panels;
                    foreach (Autodesk.Windows.RibbonPanel ribbonPanel in panels)
                    {
                        var source = ribbonPanel.Source;
                        var sourceAutoname = source.AutomationName;
                        var sourceId = source.Id;

                        if (sourceId == panelId)
                        {
                            var mypanel = default(RibbonPanel);
                            //反射获取panel
                            var typ = typeof(RibbonPanel);
                            ConstructorInfo t1Constructor = typ.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
                            Object reflectPanel = t1Constructor.Invoke(new object[] { ribbonPanel, tabid });
                            mypanel = (RibbonPanel)reflectPanel;
                            mypanel.Visible = true;
                            mypanel.Enabled = true;
                            result = mypanel;
                            break;
                        }
                    }
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 在指定选项卡上创建 一个Revit的 RibbonPanel
        /// </summary>
        /// <param name="tabtitle"></param>
        /// <param name="panelAutoName"></param>
        /// <returns></returns>
        public static RibbonPanel CreateRibbonPanel(string tabtitle, string panelAutoName)
        {
            RibbonPanel result = null;
            var mainRibbonControl = RevitRibbonControl.RibbonControl;
            var tabs = mainRibbonControl.Tabs;
            foreach (RibbonTab ribbonTab in tabs)
            {
                var tabid = ribbonTab.Id;
                var title = ribbonTab.Title;
                if (tabtitle != title) continue;
                if (tabtitle == title)
                {
                    var panels = ribbonTab.Panels;

                    //创建autodesk.windows的panel
                    var adpanel = new Autodesk.Windows.RibbonPanel();
                    RibbonPanelSource source = new RibbonPanelSource();
                    source.Name = panelAutoName;
                    source.Id = Guid.NewGuid().ToString();
                    adpanel.Source = source;
                    adpanel.FloatingOrientation = Orientation.Horizontal;
                    adpanel.CanToggleOrientation = false;
                    adpanel.IsVisible = true;
                    adpanel.IsEnabled = true;

                    panels.Add(adpanel);//添加到RibbonControl上

                    //创建panel
                    var mypanel = default(RibbonPanel);
                    //反射获取Revit封装的ribbonpanel的构造函数
                    var typ = typeof(RibbonPanel);
                    ConstructorInfo t1Constructor = typ.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
                    Object reflectPanel = t1Constructor.Invoke(new object[] { adpanel, tabid });
                    mypanel = (RibbonPanel)reflectPanel;
                    mypanel.Visible = true;
                    mypanel.Enabled = true;

                    mypanel.Title = panelAutoName;

                    result = mypanel;

                    break;
                }
                continue;
            }

            return result;
        }

        /// <summary>
        /// 将面板上的所有按钮反序
        /// </summary>
        /// <param name="tabid"></param>
        /// <returns></returns>
        public static bool ReversAllButtonsOnPanel(string tabid)
        {
            bool result = false;
            try
            {
                var tab = RibbonHelper.GetMainControl().Tabs.Where(m => m.Id == tabid).First();
                foreach (var ribbonPanel in tab.Panels)
                {
                    var items = ribbonPanel.Source.Items;
                    //items.Remove(items.Last());
                    var items_reorder = items.Reverse();

                    foreach (var ribbonItem in items_reorder)
                    {
                        items.Add(ribbonItem);
                    }
                    for (int i = 0; i < items.Count; i++)
                    {
                        items.Remove(items[i]);
                    }

                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 删除面板上的所有按钮
        /// </summary>
        /// <param name="tabid"></param>
        /// <returns></returns>
        public static bool RemoveAllButtonsOnPanel(string tabid)
        {
            bool result = false;
            try
            {
                var tab = RibbonHelper.GetMainControl().Tabs.Where(m => m.Id == tabid).First();
                var panels = tab.Panels;

                foreach (var ribbonPanel in panels)
                {
                    var items = ribbonPanel.Source.Items;
                    //items.Remove(items.Last());
                    //var items_reorder = items.Reverse();

                    //foreach (var ribbonItem in items_reorder)
                    //{
                    //    items.Add(ribbonItem);
                    //}
                    for (int i = 0; i < items.Count;)
                    {
                        items.Remove(items[i]);
                    }
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
    }


}
