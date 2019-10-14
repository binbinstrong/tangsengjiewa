using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace 唐僧解瓦.BinLibrary.Helpers
{
   public static class CollectorHelper
    {
        public static IList<T> TCollector<T>(this Document doc)
        {
            Type type = typeof(T);
            return new FilteredElementCollector(doc).OfClass(type).Cast<T>().ToList();
        }
    }
}
