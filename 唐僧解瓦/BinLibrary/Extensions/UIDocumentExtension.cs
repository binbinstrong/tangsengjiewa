using Autodesk.Revit.UI;
using System.Linq;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class UIDocumentExtension
    {
        public static UIView ActiveUIView(this UIDocument uidoc)
        {
            var result = default(UIView);
            var doc = uidoc.Document;
            var acview = doc.ActiveView;

            var uiviews = uidoc.GetOpenUIViews();
            result = uiviews.FirstOrDefault(m => m.ViewId == acview.Id);
            return result;
        }
    }
}
