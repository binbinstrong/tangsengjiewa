using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 框选三维
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_PickBox3D:IExternalCommand ,IExternalCommandAvailability
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            var acview = doc.ActiveView;

            var viewfamilytype = doc.TCollector<ViewFamilyType>()
                .Where(m => m.ViewFamily == ViewFamily.ThreeDimensional).First();

            var elementRefs = sel.PickObjects(ObjectType.Element, 
                doc.GetSelectionFilter(m =>{ return m.Category.CategoryType == CategoryType.Model; }));
            var eles = elementRefs.Select(m => m.ElementId.GetElement(doc));
            var eleids = elementRefs.Select(m => m.ElementId).ToList();

            var tembox = default(BoundingBoxXYZ);

            Transaction temtran = new Transaction(doc, "temTran");
            temtran.Start();
            var group = doc.Create.NewGroup(eleids);
              tembox = group.get_BoundingBox(acview);
            temtran.RollBack();
            
            var newAcview = default(View);
            doc.Invoke(m =>
            {
                var _3dview = View3D.CreateIsometric(doc, viewfamilytype.Id);
                _3dview.SetSectionBox(tembox);
                newAcview = _3dview;
            }, "框选三维");

            uidoc.ActiveView = newAcview;
            return Result.Succeeded;
        }

        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            //throw new NotImplementedException();

            return true;
        }
    }
}
