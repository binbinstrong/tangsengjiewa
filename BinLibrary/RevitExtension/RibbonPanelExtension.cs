using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace BinLibrary.RevitExtension
{
    public static class RibbonPanelExtension
    {
        public static Autodesk.Windows.RibbonPanel InternalPanel(this RibbonPanel panel)
        {
            var result =
                panel.GetType().GetField("m_RibbonPanel", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(panel) as Autodesk.Windows.RibbonPanel;
            return result;
        }
        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="buttonname"></param>
        /// <param name="Text"></param>
        /// <param name="dllpath"></param>
        /// <param name="classname"></param>
        /// <param name="tooltip"></param>
        /// <param name="imagepath"></param>
        /// <returns></returns>
        public static PushButton AddButton(this RibbonPanel panel, string buttonname, string Text, 
            string dllpath, string classname, string tooltip = "", string imagepath = "")
        {
            PushButtonData pushButtonData = new PushButtonData(buttonname, Text, dllpath, classname);
            PushButton pushButton = panel.AddItem(pushButtonData) as PushButton;
            if (tooltip != "")
            {
                pushButton.ToolTip = tooltip;
            }
            if (imagepath != "")
            {
                pushButton.LargeImage = (new BitmapImage(new Uri(imagepath, UriKind.Absolute)));
            }
            return pushButton;
        }

        public static PushButton AddButton1(this RibbonPanel panel, string buttonname, string Text,
            string dllpath, string classname, string tooltip = "", BitmapSource imagesource = null)
        {
            PushButtonData pushButtonData = new PushButtonData(buttonname, Text, dllpath, classname);
            PushButton pushButton = panel.AddItem(pushButtonData) as PushButton;
            if (tooltip != "")
            {
                pushButton.ToolTip = tooltip;
            }
            if (imagesource != null)
            {
                pushButton.LargeImage = imagesource; //(new BitmapImage(new Uri(imagepath, UriKind.Absolute)));
            }
            return pushButton;
        }

        /// <summary>
        /// 面板上插入按钮
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="buttonname"></param>
        /// <param name="Text"></param>
        /// <param name="dllpath"></param>
        /// <param name="classname"></param>
        /// <param name="index"></param>
        /// <param name="tooltip"></param>
        /// <param name="imagepath"></param>
        /// <returns></returns>
        public static PushButton InsertPushButton(this RibbonPanel panel, string buttonname, string Text,
            string dllpath, string classname, int index = 0, string tooltip = "", string imagepath = "")
        {
            Autodesk.Windows.RibbonPanel innerpanel = panel.GetType().GetField("m_RibbonPanel", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(panel) as Autodesk.Windows.RibbonPanel;

            var items = innerpanel.Source.Items;
            //MessageBox.Show(items.Count.ToString());
            var count = items.Count;

            //var items = panel.GetItems(); //此方法不对
            PushButtonData pushButtonData = new PushButtonData(buttonname, Text, dllpath, classname);
            PushButton pushButton = panel.AddItem(pushButtonData) as PushButton;

            if (tooltip != "")
            {
                pushButton.ToolTip = tooltip;
            }
            if (imagepath != "")
            {
                pushButton.LargeImage = (new BitmapImage(new Uri(imagepath, UriKind.Absolute)));
            }

            //MessageBox.Show(items.Count.ToString());

            var targetitem = items.Last();
            for (int i = 0; i < count + 1; i++)
            {
                if (i < index)
                {
                    items.Add(items[i]);
                }
                else if (i == index)
                {
                    items.Add(targetitem);
                }
                else if (i > index)
                    items.Add(items[i - 1]);
            }

            for (int i = 0; i < count + 1; i++)
            {
                items.Remove(items[0]);
            }
            //items.Insert(index, items.Last());
            //items.Remove(items.Last());
            //MessageBox.Show("end");
            return pushButton;
        }

        public static PushButton InsertPushButton1(this RibbonPanel panel, string buttonname, string Text,
          string dllpath, string classname, int index = 0, string tooltip = "", BitmapSource imagesource = null)
        {
            Autodesk.Windows.RibbonPanel innerpanel = panel.GetType().GetField("m_RibbonPanel", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(panel) as Autodesk.Windows.RibbonPanel;

            var items = innerpanel.Source.Items;
            //MessageBox.Show(items.Count.ToString());
            var count = items.Count;

            //var items = panel.GetItems(); //此方法不对
            PushButtonData pushButtonData = new PushButtonData(buttonname, Text, dllpath, classname);
            PushButton pushButton = panel.AddItem(pushButtonData) as PushButton;

            if (tooltip != "")
            {
                pushButton.ToolTip = tooltip;
            }
            if (imagesource != null)
            {
                pushButton.LargeImage = imagesource;// (new BitmapImage(new Uri(imagepath, UriKind.Absolute)));
            }

            //MessageBox.Show(items.Count.ToString());

            var targetitem = items.Last();
            for (int i = 0; i < count + 1; i++)
            {
                if (i < index)
                {
                    items.Add(items[i]);
                }
                else if (i == index)
                {
                    items.Add(targetitem);
                }
                else if (i > index)
                    items.Add(items[i - 1]);
            }

            for (int i = 0; i < count + 1; i++)
            {
                items.Remove(items[0]);
            }
            //items.Insert(index, items.Last());
            //items.Remove(items.Last());
            //MessageBox.Show("end");
            return pushButton;
        }

        public static PushButton AddButtonToPanel(this RibbonPanel panel, string buttonname, string Text, string dllpath, string classname, string tooltip = "", string imagepath = "")
        {
            PushButtonData pushButtonData = new PushButtonData(buttonname, Text, dllpath, classname);
            PushButton pushButton = panel.AddItem(pushButtonData) as PushButton;
            if (tooltip != "")
            {
                pushButton.ToolTip = tooltip;
            }
            if (imagepath != "")
            {
                pushButton.LargeImage = (new BitmapImage(new Uri(imagepath, UriKind.Absolute)));
            }
            return pushButton;
        }

        public static IList<RibbonItem> AddSmallButtonsToPanel(this RibbonPanel panel, PushButtonData pda1, PushButtonData pda2)
        {
            return panel.AddStackedItems(pda1, pda2) as List<RibbonItem>;
        }

        public static IList<RibbonItem> AddSmallButtonsToPanel(this RibbonPanel panel, string dllpath, 
            string buttonName1, string text1, string className1, string tooltip1,
            string buttonName2, string text2, string className2, string tooltip2, 
            string image1 = "", string image2 = "", string largeImage1 = "", string largeImage2 = "")
        {
            PushButtonData pushButtonData = new PushButtonData(buttonName1, text1, dllpath, className1);
            pushButtonData.ToolTip = tooltip1;

            if (image1 != "")
            {
                pushButtonData.Image = (new BitmapImage(new Uri(image1, UriKind.Absolute)));
            }
            if (largeImage1 != "")
            {
                pushButtonData.LargeImage = (new BitmapImage(new Uri(largeImage1, UriKind.Absolute)));
            }
            PushButtonData pushButtonData2 = new PushButtonData(buttonName2, text2, dllpath, className2);
            pushButtonData2.ToolTip = tooltip2;
            if (image2 != "")
            {
                pushButtonData2.Image = (new BitmapImage(new Uri(image2, UriKind.Absolute)));
            }
            if (largeImage2 != "")
            {
                pushButtonData2.LargeImage = (new BitmapImage(new Uri(largeImage2, UriKind.Absolute)));
            }
            return panel.AddStackedItems(pushButtonData, pushButtonData2) as List<RibbonItem>;
        }
        public static IList<RibbonItem> AddSmallButtonsToPanel1(this RibbonPanel panel, string dllpath,
           string buttonName1, string text1, string className1, string tooltip1,
           string buttonName2, string text2, string className2, string tooltip2,
           BitmapSource image1 = null, BitmapSource image2 = null, BitmapSource largeImage1 = null, BitmapSource largeImage2 = null)
        {
            PushButtonData pushButtonData = new PushButtonData(buttonName1, text1, dllpath, className1);
            pushButtonData.ToolTip = tooltip1;

            if (image1 != null)
            {
                pushButtonData.Image = image1;// (new BitmapImage(new Uri(image1, UriKind.Absolute)));
            }
            if (largeImage1 != null)
            {
                pushButtonData.LargeImage = image2;// (new BitmapImage(new Uri(largeImage1, UriKind.Absolute)));
            }
            PushButtonData pushButtonData2 = new PushButtonData(buttonName2, text2, dllpath, className2);
            pushButtonData2.ToolTip = tooltip2;
            if (image2 != null)
            {
                pushButtonData2.Image = largeImage1;// (new BitmapImage(new Uri(image2, UriKind.Absolute)));
            }
            if (largeImage2 != null)
            {
                pushButtonData2.LargeImage = largeImage2;// (new BitmapImage(new Uri(largeImage2, UriKind.Absolute)));
            }
            return panel.AddStackedItems(pushButtonData, pushButtonData2) as List<RibbonItem>;
        }

        public static IList<RibbonItem> AddSmallButtonsToPanel(this RibbonPanel panel, PushButtonData pda1, PushButtonData pda2, PushButtonData pda3)
        {
            return panel.AddStackedItems(pda1, pda2, pda3) as List<RibbonItem>;
        }

        public static IList<RibbonItem> AddSmallButtonsToPanel(this RibbonPanel panel, string dllpath, string buttonName1, string text1, string className1, string tooltip1, 
            string buttonName2, string text2, string className2, string tooltip2,
            string buttonName3, string text3, string className3, string tooltip3,
            string image1 = "", string image2 = "", string image3 = "", 
            string largeImage1 = "", string largeImage2 = "", string largeImage3 = "")
        {
            PushButtonData pushButtonData = new PushButtonData(buttonName1, text1, dllpath, className1);
            pushButtonData.ToolTip = (tooltip1);
            pushButtonData.Text = (text1);
            if (image1 != "")
            {
                pushButtonData.Image = (new BitmapImage(new Uri(image1, UriKind.Absolute)));
            }
            if (largeImage1 != "")
            {
                pushButtonData.LargeImage = (new BitmapImage(new Uri(largeImage1, UriKind.Absolute)));
            }
            PushButtonData pushButtonData2 = new PushButtonData(buttonName2, text2, dllpath, className2);
            pushButtonData2.ToolTip = (tooltip2);
            if (image2 != "")
            {
                pushButtonData2.Image = (new BitmapImage(new Uri(image2, UriKind.Absolute)));
            }
            if (largeImage2 != "")
            {
                pushButtonData2.LargeImage = (new BitmapImage(new Uri(largeImage2, UriKind.Absolute)));
            }
            PushButtonData pushButtonData3 = new PushButtonData(buttonName3, text3, dllpath, className3);
            pushButtonData3.ToolTip = (tooltip3);
            if (image3 != "")
            {
                pushButtonData3.Image = (new BitmapImage(new Uri(image3, UriKind.Absolute)));
            }
            if (largeImage3 != "")
            {
                pushButtonData3.LargeImage = (new BitmapImage(new Uri(largeImage3, UriKind.Absolute)));
            }
            return panel.AddStackedItems(pushButtonData, pushButtonData2, pushButtonData3) as List<RibbonItem>;
        }
        
    }
}
