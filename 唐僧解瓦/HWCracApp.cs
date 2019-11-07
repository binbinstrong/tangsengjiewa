using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Media.Imaging;
using 唐僧解瓦.机电;

#region Copyright
/************************************************************************************ 
 * Copyright (c) 2017Microsoft All Rights Reserved. 
 * CLR版本： 4.0.30319.18444 
 *机器名称：MDELL-PC-XZB 
 *公司名称：Microsoft 
 *命名空间：$rootnamespace$ 
 *文件名：  Class1 
 *版本号：  V1.0.0.0 
 *唯一标识：cab6412c-0eb8-4290-ba35-6f4ebad7bbc4 
 *当前的用户域：MDELL-PC-XZB 
 *创建人：  xuzhaobin 
 *电子邮箱：384785044@qq.com 
 *创建时间：2017/1/13 14:34:10 
 *描述： 
 * 
 *===================================================================== 
 *修改标记 
 *修改时间：2017/1/13 14:34:10 
 *修改人： 徐召彬 
 *版本号： V1.0.0.0 
 *描述： 
 * 
/************************************************************************************/
#endregion
namespace 唐僧解瓦
{
    class HWCracApp : IExternalApplication
    {
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {

            string bintab = "唐僧工具箱";
            application.CreateRibbonTab(bintab);

            var asmpath = Assembly.GetExecutingAssembly().Location;

            Type extendwireT = typeof(Cmd_ExtendWire);
            Type Cmd_HideSplitWireT = typeof(Cmd_HideSplitWire);
            Type Cmd_HideSplitWiretestT = typeof(Cmd_HideSplitWiretest);

            //PushButtonData button1 = new PushButtonData("binbox", "resetbox", @"C:\ProgramData\Autodesk\Revit\Addins\2015\bincropbox.dll", "bincropbox.CropBoxQuickSet");
            //PushButtonData button2 = new PushButtonData("changeplane", "changeplane", @"C:\ProgramData\Autodesk\Revit\Addins\2015\changeplane.dll", "changeplane.binchangeplane");


            RibbonPanel m_projectPanel = application.CreateRibbonPanel(bintab, "电气");
            // Add the buttons to the panel
            List<RibbonItem> binButtons = new List<RibbonItem>();

            PushButtonData extendwirebuttonData = new PushButtonData("延长导线","延长导线",asmpath,extendwireT.FullName);
            PushButtonData Cmd_HideSplitWireTButtonData =
                new PushButtonData("导线断线", "导线断线", asmpath, Cmd_HideSplitWireT.FullName);
            PushButtonData Cmd_HideSplitWiretestTButtonData =
                new PushButtonData("手动断线", "手动断线", asmpath, Cmd_HideSplitWiretestT.FullName);

            m_projectPanel.AddItem(extendwirebuttonData);
            m_projectPanel.AddSeparator();
            m_projectPanel.AddItem(Cmd_HideSplitWireTButtonData);
            m_projectPanel.AddSeparator();
            m_projectPanel.AddItem(Cmd_HideSplitWiretestTButtonData);

            //binButtons.AddRange(m_projectPanel.AddStackedItems(extendwirebuttonData));
            
            
            // add new ribbon panel 
            //RibbonPanel ribbonPanel1 = application.CreateRibbonPanel("binpanel1");
            ////create a push button inthe bibbon panel "newbibbonpanel"
            ////the add=in applintion "helloworld"willbe triggered when button is pushed
            //PushButton pushButton1_1 = ribbonPanel1.AddItem(new PushButtonData("helloworld", "helloworld", @"D:\helloworld.dll", "HelloWorld.CsHelloWorld")) as PushButton;
            //PushButton pushButton1_2 = ribbonPanel1.AddItem(new PushButtonData("bin", "bin", @"D:\helloworld.dll", "HelloWorld.CsHelloWorld")) as PushButton;
            //PushButton pushButton1_3 = ribbonPanel1.AddItem(new PushButtonData("bin1", "bin1", @"D:\helloworld.dll", "HelloWorld.CsHelloWorld")) as PushButton;
            //ribbonPanel1.AddSeparator();

            //RibbonPanel ribbonPanel2 = application.CreateRibbonPanel("binpanel2");

            //PushButton pushbutton2_1 = ribbonPanel2.AddItem(new PushButtonData("binst1", "pipe", @"D:\RevitDebug\bin\postcommand.dll", "BinPostCommand.binpostcommand")) as PushButton;
            //PushButton pushbutton2_2 = ribbonPanel2.AddItem(new PushButtonData("binst2", "resetbox", @"C:\ProgramData\Autodesk\Revit\Addins\2015\bincropbox.dll", "bincropbox.CropBoxQuickSet")) as PushButton;
            //PushButton pushbutton2_3 = ribbonPanel2.AddItem(new PushButtonData("binst3", "binst3", @"D:\helloworld.dll", "HelloWorld.CsHelloWorld")) as PushButton;
            //ribbonPanel2.AddSeparator();

            //RibbonPanel ribbonPanel3 = application.CreateRibbonPanel("binpanel3");

            //PushButton pushbutton3_1 = ribbonPanel3.AddItem(new PushButtonData("binst4","binst4",@"D:\helloworld.dll", "HelloWorld.CsHelloWorld")) as PushButton;
            //PushButton pushbutton3_2 = ribbonPanel3.AddItem(new PushButtonData("binst5","binst4",@"D:\helloworld.dll", "HelloWorld.CsHelloWorld")) as PushButton;
            //PushButton pushbutton3_3 = ribbonPanel3.AddItem(new PushButtonData("binst6","binst4",@"D:\helloworld.dll", "HelloWorld.CsHelloWorld")) as PushButton;
            //ribbonPanel3.AddSeparator();
            
            return Result.Succeeded;

        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }

}