using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.UI;

namespace BinLibrary.RevitHelper
{
    public class DockablePaneHelper
    {
        /// <summary>
        /// 注册并添加可停靠面板
        /// </summary>
        /// <param name="uiapplication"></param>
        /// <param name="paneId"></param>
        /// <param name="dockablePane"></param>
        public static void AddDockablePane(UIControlledApplication uiapplication, DockablePaneId paneId, object dockablePane, string title)
        {
            IDockablePaneProvider pane = dockablePane as IDockablePaneProvider;
            uiapplication.RegisterDockablePane(paneId, title, pane);
        }
        public static void AddDockablePane(UIControlledApplication uiapplication, Guid guid, object dockablePane, string title)
        {
            DockablePaneId paneId = new DockablePaneId(guid);
            AddDockablePane(uiapplication, paneId, dockablePane, title);
        }

        public static void AddDockablePane(UIApplication uiapplication, DockablePaneId paneId, object dockablePane, string title)
        {
            IDockablePaneProvider pane = dockablePane as IDockablePaneProvider;
            uiapplication.RegisterDockablePane(paneId, title, pane);
        }

        public static void AddDockablePane(UIApplication uiapp, Guid guid, object dockablePane, string title)
        {
            DockablePaneId paneId = new DockablePaneId(guid);
            AddDockablePane(uiapp, paneId, dockablePane, title);
        }
        

    }
}
