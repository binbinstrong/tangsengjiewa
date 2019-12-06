using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class DocumentExtension
    {
        public static UIView ActiveUiview(this UIDocument uidoc)
        {
            var acview = uidoc.ActiveView;
            var uiviews = uidoc.GetOpenUIViews();
            var acuiview = uiviews.Where(m => acview.Id == m.ViewId).FirstOrDefault();
            return acuiview;
        }
    }
}
