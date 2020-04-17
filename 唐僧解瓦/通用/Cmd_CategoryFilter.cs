using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace 唐僧解瓦.通用
{
    [Transaction(TransactionMode.Manual)]
    class Cmd_CategoryFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;


            //方法一
            var categories = doc.Settings.Categories;

            var categoryFilters = new List<ElementFilter>();

            foreach (Category cate in categories)
            {
                var categoryType = cate.CategoryType;
                if (categoryType == CategoryType.Model)
                {
                    var categoryfilter = new ElementCategoryFilter(cate.Id);
                    categoryFilters.Add(categoryfilter);
                }
            }

            var logicalOrFilter = new LogicalOrFilter(categoryFilters);


            var collector = new FilteredElementCollector(doc);


            var targetCollector = collector.WherePasses(logicalOrFilter);

            //collector.WhereElementIsNotElementType().Where(m => m.Category.CategoryType == CategoryType.Model);


            //方法二

            foreach (BuiltInCategory builtInid in Enum.GetValues(typeof(BuiltInCategory)))
            {
                var cate = Category.GetCategory(doc, builtInid);

                var categoryType = cate.CategoryType;
                if (categoryType == CategoryType.Model)
                {
                    //////
                }
            }

            


            return Result.Succeeded;
        }
    }
}
