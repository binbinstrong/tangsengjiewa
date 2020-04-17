using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.通用
{
    [Transaction(TransactionMode.Manual)]
    //[Journaling(JournalingMode.UsingCommandData)]
    //[Regeneration(RegenerationOption.Manual)]
    class Cmd_ElementTest:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            var sel = uidoc.Selection;



            //var refence = sel.PickObject(ObjectType.Element,
            //    doc.GetSelectionFilter(m => m.Category.CategoryType == CategoryType.Model || m.Category.CategoryType == CategoryType.Internal));

            //var element = doc.GetElement(refence);

            //var name = element.Name;
            //var id = element.Id;

            //var category = element.Category;

            //var familyName = element.LookupParameter("族").AsValueString();

            //MessageBox.Show("name:" + name + Environment.NewLine +
            //                "id:" + id + Environment.NewLine +
            //                "category:" + category.Name + Environment.NewLine +
            //                "familyName:" + familyName);


            var collector = new FilteredElementCollector(doc);

            var ductCollector = collector.OfClass(typeof(Duct));


            var ductlist = new List<Duct>();

            foreach (Duct duct in ductCollector)
            {
                var lenth = duct.LocationLine().Length.FeetToMetric();

                if (lenth < 200)
                {
                    ductlist.Add(duct);
                }
                
            }

            sel.SetElementIds(ductlist.Select(m => m.Id).ToList());


            return Result.Succeeded;
        }
    }
}
