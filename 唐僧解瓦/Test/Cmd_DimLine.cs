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
    class Cmd_DimLine:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var app = uiapp.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            var acview = uidoc.ActiveView;

            var pipe = uidoc.Selection.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Pipe))
                .GetElement(doc) as Pipe;
            
            var location = pipe.Location as LocationCurve;


            var ref1 = location.Curve.GetEndPointReference(0);
            var ref2 = location.Curve.GetEndPointReference(1);


            var referencearray = new ReferenceArray();
            referencearray.Append(ref1);
            referencearray.Append(ref2);

            var line = pipe.LocationLine();

            doc.Invoke(m =>
            {

                doc.Create.NewDimension(acview, line, referencearray);

            },"dim");



            return Result.Succeeded;
        }
    }
}
