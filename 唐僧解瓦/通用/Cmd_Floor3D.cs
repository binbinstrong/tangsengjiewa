using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using 唐僧解瓦.BinLibrary.Helpers;
using 唐僧解瓦.通用.UIs;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 楼层三维
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    class Cmd_Floor3D : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            var acview = doc.ActiveView;

            var viewfamilytypes = doc.TCollector<ViewFamilyType>();
            var viewplanfamilytype = viewfamilytypes.Where(m => m.ViewFamily == ViewFamily.FloorPlan).First();
            var view3Dfamilytype = viewfamilytypes.Where(m => m.ViewFamily == ViewFamily.ThreeDimensional).First();

            var levels = doc.TCollector<Level>();

            FloorSelector fsui = FloorSelector.Instance;
            fsui.FloorBox.ItemsSource = levels;
            fsui.FloorBox.DisplayMemberPath = "Name";
            fsui.FloorBox.SelectedIndex = 0;
            fsui.ShowDialog();

            var targetfloor = fsui.FloorBox.SelectionBoxItem as Level;
            var upperfloor = levels.Where(m => m.Elevation > targetfloor.Elevation)?.OrderBy(m => m.Elevation)?.FirstOrDefault();
             
            var categories = doc.Settings.Categories;
            var modelcategories = categories.Cast<Category>().Where(m => m.CategoryType == CategoryType.Model).ToList();

            var filterslist = new List<ElementFilter>();
            foreach (var modelcategory in modelcategories)
            {
                var categoryfilter = new ElementCategoryFilter(modelcategory.Id);
                filterslist.Add(categoryfilter);
            }
            var logicOrFilter = new LogicalOrFilter(filterslist);
            var collector = new FilteredElementCollector(doc);
            var modelelements = collector.WherePasses(logicOrFilter).WhereElementIsNotElementType().Where(m => m.Category.CategoryType == CategoryType.Model);

            var modelelementsids = modelelements.Select(m => m.Id).ToList();

            var temboundingbox = default(BoundingBoxXYZ);

            Transaction temtran = new Transaction(doc, "temtransaction");
            temtran.Start();
            var temgroup = doc.Create.NewGroup(modelelementsids);
            var temview = ViewPlan.Create(doc, viewplanfamilytype.Id, targetfloor.Id);
            temboundingbox = temgroup.get_BoundingBox(temview);
            temtran.RollBack();

            var zMin = targetfloor.Elevation;
            var zMax = upperfloor?.Elevation ?? temboundingbox.Max.Z;

            var oldmin = temboundingbox.Min;
            var oldmax = temboundingbox.Max;

            BoundingBoxXYZ newbox = new BoundingBoxXYZ();
            newbox.Min = new XYZ(oldmin.X, oldmin.Y, zMin);
            newbox.Max = new XYZ(oldmax.X, oldmax.Y, zMax);
            var new3dview = default(View3D);
            
            doc.Invoke(m =>
            {
                new3dview = View3D.CreateIsometric(doc, view3Dfamilytype.Id);
                new3dview.SetSectionBox(newbox);
            },"楼层三维");

            uidoc.ActiveView = new3dview;

            return Result.Succeeded;
        }
    }
}
