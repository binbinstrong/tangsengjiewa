using Autodesk.Revit.DB;

namespace 唐僧解瓦.BinLibrary.Extensions
{

    public static class ReferenceExtension
        {
            public static Element GetElement(this Reference thisref, Document doc)
            {
                return doc.GetElement(thisref);
            }
        }
    
}
