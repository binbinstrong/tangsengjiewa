using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

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



            return Result.Succeeded;
        }

        public ElementId CreateRevitLink(Document doc, string pathName)
        {
            FilePath path = new FilePath(pathName);
            RevitLinkOptions options = new RevitLinkOptions(false);
            // Create new revit link storing absolute path to file   
            LinkLoadResult result = RevitLinkType.Create(doc, path, options);
            return (result.ElementId);
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
