
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace 红瓦功能揭秘.BinLibrary.Extensions
{
    
        public static class ReferenceExtension
        {
            public static Element GetElement(this Reference thisref, Document doc)
            {
                return doc.GetElement(thisref);
            }
        }
    
}
