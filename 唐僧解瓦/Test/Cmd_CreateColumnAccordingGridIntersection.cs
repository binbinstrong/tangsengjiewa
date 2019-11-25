using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using 唐僧解瓦.BinLibrary.Helpers;
using 唐僧解瓦.BinLibrary.RevitHelper;
using 唐僧解瓦.Test.UIs;

namespace 唐僧解瓦.Test
{
    /// <summary>
    /// 在轴线交点处生成柱子
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_CreateColumnAccordingGridIntersection : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            var acview = doc.ActiveView;

            //filter target columntypes 
            ElementFilter architectureColumnFilter = new ElementCategoryFilter(BuiltInCategory.OST_Columns);
            ElementFilter structuralColumnFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
            ElementFilter orfilter = new LogicalOrFilter(architectureColumnFilter,structuralColumnFilter);
            var collector = new FilteredElementCollector(doc);
            var columnstypes = collector.WhereElementIsElementType().WherePasses(orfilter).ToElements();

            ColumnTypesForm typesform = ColumnTypesForm.Getinstance(columnstypes.ToList());//new ColumnTypesForm(columnstypes.ToList());
            typesform.ShowDialog(RevitWindowHelper.GetRevitWindow());

            //get selected familysymbol of combobox in columntypesForm
            var familysymbol = typesform.symbolCombo.SelectedItem as FamilySymbol;

            //varient for setting bottom and top /*for learners self modifing*/
            var bottomlevel = default(Level);
            var bottomoffset = default(double);

            var toplevel = default(Level);
            var topoffset = default(double);
            
            var grids = doc.TCollector<Grid>();
            var points = new List<XYZ>();
            foreach (var grid in grids)
            {
                foreach (var grid1 in grids)
                {
                    if (grid.Id == grid1.Id)
                    {
                        continue;
                    }
                    var curve1 = grid.Curve;
                    var curve2 = grid1.Curve;
                    var res = new IntersectionResultArray();
                    var intersecRes = curve1.Intersect(curve2, out res);
                    if (intersecRes != SetComparisonResult.Disjoint)
                    {
                        if (res != null)
                        {
                            points.Add(res.get_Item(0).XYZPoint);
                        }
                    }
                }
            }
            //distinct points on same location
            points = points.Where((m, i) => points.FindIndex(n => n.IsAlmostEqualTo(m)) == i).ToList();

            //MessageBox.Show(points.Count.ToString());
            //CreateColumns as intersection point

            TransactionGroup tsg = new TransactionGroup(doc);
            tsg.Start("统一创建柱子");
            foreach (var point in points)
            {
                doc.Invoke(m =>
                {
                    if (!familysymbol.IsActive) familysymbol.Activate();
                    var instance = doc.Create.NewFamilyInstance(point, familysymbol, acview.GenLevel,
                        StructuralType.NonStructural);
                }, "创建柱子");
            }
            tsg.Assimilate();
            return Result.Succeeded;
        }

        public XYZ Intersect_cus(Curve c, Curve c1)
        {
            XYZ result = null;
            IntersectionResultArray resultArray = new IntersectionResultArray();

            var comparisonResult = c.Intersect(c1, out resultArray);
            if (comparisonResult != SetComparisonResult.Disjoint)
            {
                if (resultArray != null)
                    result = resultArray.get_Item(0).XYZPoint;
            }
            return result;
        }
    }
}

