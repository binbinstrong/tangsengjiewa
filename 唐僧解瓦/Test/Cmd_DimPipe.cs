using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.Test
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_DimPipe:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            var acview = uidoc.ActiveView;

            var pipe = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Pipe)).GetElement(doc) as Pipe;

            var geoele = pipe.get_Geometry(new Options()
                {DetailLevel = ViewDetailLevel.Fine, ComputeReferences = true});

            var line = pipe.LocationLine();


            var refs = GetEndPlanRefs(geoele);

            doc.Invoke(m =>
            {
                doc.Create.NewDimension(acview, line, refs);

            },"创建管道长度标注");

            return Result.Succeeded;
        }

        private ReferenceArray  GetEndPlanRefs(GeometryElement geoele)
        {
            var result = new ReferenceArray();

            var geometrys = geoele.Cast<GeometryObject>().ToList();

            foreach (GeometryObject geo in geometrys)
            {
                if (geo is Solid so)
                {
                    var faces = so.Faces;
                    foreach (var face in faces)
                    {
                        if (face is PlanarFace pface)
                        {
                            result.Append(pface.Reference);
                        }
                    }
                }
                else
                    continue;
            }

            return result;
        }
    }
}
