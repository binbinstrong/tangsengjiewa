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

namespace 唐僧解瓦.建筑
{
    /// <summary>
    /// 一键成板（选择梁 根据梁围成的闭合区域生成板）
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    class Cmd_CreateFloorQukly:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            var acview = doc.ActiveView;

            var beams = sel.PickObjects(ObjectType.Element,
                doc.GetSelectionFilter(m => m.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming),
                "选择生成板的梁");

             
            //var ele = sel.PickObject(ObjectType.Element).GetElement(doc);
            //var solid = ele.get_Geometry(new Options()).GetSolidOfGeometryObject().First();
            //var trans = Transform.Identity;
            //trans.Origin = new XYZ(-10, 0, 0);

            //doc.Invoke(m =>
            //{
            //    var newsolid = SolidUtils.CreateTransformed(solid, trans);
            //    var directshape = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));

            //    directshape.AppendShape(new List<GeometryObject>() {newsolid});
            //}, "test");

            //doc.Create.NewFloor()



            return Result.Succeeded;
        }
    }
}
