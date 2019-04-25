using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using FilteredElementCollector = Autodesk.Revit.DB.FilteredElementCollector;

namespace BinLibrary.RevitHelper
{
    public static class CollectorHelper
    {
        public static FilteredElementCollector DuctCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(Duct));
        }
        public static FilteredElementCollector PipeCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(Pipe));
        }
        public static FilteredElementCollector CableTrayCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(CableTray));
        }
        public static FilteredElementCollector ConduitCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(Conduit));
        }
        public static FilteredElementCollector WallCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(Wall));
        }
        public static IList<View3D> _3DViewCollector(this Document doc)
        {
            var result = new List<View3D>();
            var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D))
                .Where(m => !(m as View3D).IsTemplate).Cast<View3D>().ToList();
            if (view3ds.Count > 0) result = view3ds;
            return result;
        }
        public static IList<View> ViewCollector(this Document doc)
        {
            var result = new List<View>();
            var views = new FilteredElementCollector(doc).OfClass(typeof(View))
                .Where(m => !(m as View).IsTemplate).Cast<View>().ToList();
            if (views.Count > 0) result = views;
            return result;
        }
        public static IList<ViewSection> ViewSectionCollector(this Document doc)
        {
            var result = new List<ViewSection>();
            var views = new FilteredElementCollector(doc).OfClass(typeof(ViewSection))
                .Where(m => !(m as ViewSection).IsTemplate).Cast<ViewSection>().ToList();
            if (views.Count > 0) result = views;
            return result;
        }
        public static IList<Grid> GridCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(Grid)).Cast<Grid>().ToList<Grid>();
        }
        public static IList<Level> Levelollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().ToList<Level>();
        }
        public static IList<FamilyInstance> FamilyInstanceCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().ToList();
        }
        public static IList<FamilyInstance> ModelFamilyInstanceCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                .Where(m => m.Category.CategoryType == CategoryType.Model)
                .Cast<FamilyInstance>().ToList();
        }
        public static IList<FamilyInstance> AnnotationFamilyInstanceCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                .Where(m => m.Category.CategoryType == CategoryType.Annotation)
                .Cast<FamilyInstance>().ToList();
        }
        public static IList<FamilySymbol> FamilySymbolCollector(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>().ToList();
        }

        public static IList<MEPCurve> MepcurveCollecotr(this Document doc)
        {
            return new FilteredElementCollector(doc).OfClass(typeof(MEPCurve)).WhereElementIsNotElementType()
                .Cast<MEPCurve>().ToList();
        }
        public static IList<T> TCollector<T>(this Document doc)
        {
            Type type = typeof(T);
            return new FilteredElementCollector(doc).OfClass(type)
                .Cast<T>().ToList();
        }
    }
}
