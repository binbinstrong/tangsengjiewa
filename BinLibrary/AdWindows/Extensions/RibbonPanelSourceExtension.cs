using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinLibrary.AdWindows.Extensions
{
    public static class RibbonPanelSourceExtension
    {
        public static RibbonItemCollection MainPanelItems(this RibbonPanelSource panelsource)
        {
            var type = typeof(RibbonPanelSource);
            var mMainpanelitems = type.GetField("mMainPanelItems", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(panelsource) as RibbonItemCollection;
            return mMainpanelitems;
        }
        public static RibbonItemCollection Items(this RibbonPanelSource panelsource)
        {
            var type = typeof(RibbonPanelSource);
            var mitems = type.GetField("mItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(panelsource) as RibbonItemCollection;
            return mitems;
        }
        public static RibbonItemCollection SlideOutPanelItems(this RibbonPanelSource panelsource)
        {
            var type = typeof(RibbonPanelSource);
            var mSlideOutPanelItems = type.GetField("mSlideOutPanelItems", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(panelsource) as RibbonItemCollection;
            return mSlideOutPanelItems;
        }
    }
}
