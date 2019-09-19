using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public class SelectionFilter : ISelectionFilter
    {
        private Document _doc;
        private Type _type;

        public SelectionFilter(Document doc, Type type)
        {
            _doc = doc;
            _type = type;
        }
        public bool AllowElement(Element elem)
        {
            if (elem.GetType() == _type)
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }

    public static class SelectionFilterHelper
    {
        

        public static MultiSelectionFilter GetSelectionFilter(this Document doc, Func<Element, bool> func1, Func<Reference, bool> func2 = null)
        {
            return new MultiSelectionFilter(func1, func2);
        }
    }
    public class MultiSelectionFilter : ISelectionFilter
    {
        private Func<Element, bool> eleFunc;
        private Func<Reference, bool> refFunc;


        public MultiSelectionFilter(Func<Element, bool> func, Func<Reference, bool> func1)
        {
            eleFunc = func;
            refFunc = func1;
        }
        public bool AllowElement(Element elem)
        {
            return refFunc != null ? true : eleFunc(elem);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return refFunc == null ? false : refFunc(reference);
            //return  refFunc(reference);
        }
    }
}
