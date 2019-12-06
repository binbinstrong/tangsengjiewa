using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Microsoft.Win32;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 批量链接
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]

    class Cmd_MultipleLinkFile : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            var acview = doc.ActiveView;
            // pop up dialog let users to select files
            // 未完待续

            var opdg = new OpenFileDialog();
            opdg.Multiselect = true;
            opdg.Filter = "(*.rvt)|*.rvt";
            var dialogresult = opdg.ShowDialog();

            var count = opdg.FileNames.Length;
            string[] files = new string[count];

            if (dialogresult == true)
            {
                files = opdg.FileNames;
            }
            doc.Invoke(m =>
            {
                foreach (var file in files)
                {
                    var linktypeId = CreateRevitLink(doc, file);
                    CreateLinkInstances(doc, linktypeId);
                }
            }, "批量链接");

            return Result.Succeeded;
        }

        public ElementId CreateRevitLink(Document doc, string pathName)
        {
            FilePath path = new FilePath(pathName);
            RevitLinkOptions options = new RevitLinkOptions(false);
            // Create new revit link storing absolute path to file  
#if Revit2019
            LinkLoadResult result = RevitLinkType.Create(doc, path, options);
    return (result.ElementId);
#endif
#if Revit2016
            return null;
#endif
        }

        public void CreateLinkInstances(Document doc, ElementId linkTypeId)
        {
            // Create revit link instance at origin   
            //RevitLinkInstance.Create(doc, linkTypeId);
            RevitLinkInstance instance2 = RevitLinkInstance.Create(doc, linkTypeId);
            // Offset second instance by 100 feet   
            //Location location = instance2.Location;
            //location.Move(new XYZ(0, -100, 0));
        }



    }
}
