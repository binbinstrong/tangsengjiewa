using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autodesk.Internal.InfoCenter;
using Autodesk.Internal.Windows;
namespace BinLibrary.RevitHelper
{
    public static class BubbleHelper
    {
        /// <summary>
        /// 面板气泡提示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void PanelBubbleShow(string title = "title", string content = "content")
        {
            var paletteMgr = Autodesk.Windows.ComponentManager.InfoCenterPaletteManager;
            var resultItem = new ResultItem()
            {
                Category = title, ///气泡标题
                Title = content,///气泡内容
            };
            paletteMgr.ShowBalloon(resultItem);
        }

        /// <summary>
        /// 绘图区域弹出气泡
        /// </summary>
        /// <param name="showmessage"></param>
        public static void ElementBubbleShow(string showmessage)
        {
            UIFramework.EnhancedTooltipImpl.Show(showmessage);
        }

        public static void UnDisplayBubble()
        {
            UIFramework.EnhancedTooltipImpl.SuppressOnShow(false);
            UIFramework.EnhancedTooltipImpl.Close();
        }

        /// <summary>
        /// 未测试
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentMessate()
        {
            Type t = typeof(UIFramework.EnhancedTooltipImpl);
            var field = t.GetField("s_current", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            ConstructorInfo consinfo = t.GetConstructor(Type.EmptyTypes);
            UIFramework.EnhancedTooltipImpl instance = consinfo.Invoke(new object[] { }) as UIFramework.EnhancedTooltipImpl;
            var result = field.GetValue(instance) as string;
            return result;
        }
        /// <summary>
        /// 未测试
        /// </summary>
        /// <param name="newMsg"></param>
        public static void SetCurrentMessage(string newMsg)
        {
            Type t = typeof(UIFramework.EnhancedTooltipImpl);
            var field = t.GetField("s_current", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            ConstructorInfo consinfo = t.GetConstructor(Type.EmptyTypes);
            UIFramework.EnhancedTooltipImpl instance = consinfo.Invoke(new object[] {}) as UIFramework.EnhancedTooltipImpl;

            field.SetValue(instance, newMsg);
        }
        /// <summary>
        /// 未测试
        /// </summary>
        /// <returns></returns>
        public static ToolTip GetToolTip()
        {
            Type t = typeof(UIFramework.EnhancedTooltipImpl);
            var field = t.GetField("s_tooltip", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            ConstructorInfo consinfo = t.GetConstructor(Type.EmptyTypes);
            UIFramework.EnhancedTooltipImpl instance = consinfo.Invoke(new object[] { }) as UIFramework.EnhancedTooltipImpl;
            var result = field.GetValue(instance) as ToolTip;

            return result;
        }
    }
}
