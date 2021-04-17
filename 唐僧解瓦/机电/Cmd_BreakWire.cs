using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    class Cmd_BreakWire : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            View acview = uidoc.ActiveView;

       

            while (true)
            {
                try
                {
                var eleref = sel.PickObject(ObjectType.PointOnElement, doc.GetSelectionFilter(m => true, n => n.GetElement(doc) is Wire));
                var eleref1 = sel.PickObject(ObjectType.PointOnElement, doc.GetSelectionFilter(m => true, n => n.ElementId == eleref.ElementId));

                var wire = eleref.GetElement(doc) as Wire;
                var wiretype = wire.GetTypeId().GetElement(doc) as WireType;
                var wiringtype = wire.WiringType;

                var point = eleref.GlobalPoint;
                var point1 = eleref1.GlobalPoint;

                var vertexs = new List<XYZ>();
                var count = wire.NumberOfVertices;

                var pointlist1 = new List<XYZ>();
                var pointlist2 = new List<XYZ>();

                var resultflag = false;
                var flag = false;
                for (int i = 0; i < count - 1; i++)
                {
                    var curpo = wire.GetVertex(i);
                    var nextpo = wire.GetVertex(i + 1);

                    var line = Line.CreateBound(curpo, nextpo);
                    //vertexs.Add(wire.GetVertex(i));
                    if (!flag)
                    {
                        flag = point.IsOnLine(line);
                        pointlist1.Add(curpo);
                    }

                    if (!resultflag)
                        resultflag = point1.IsOnLine(line);

                    if (flag && resultflag)
                    {
                        pointlist2.Add(nextpo);
                    }
                }


                var firstLinelast = pointlist1.LastOrDefault();
                var secondLinefirst = pointlist2.FirstOrDefault();
                var firstpo = firstLinelast.DistanceTo(point) < firstLinelast.DistanceTo(point1)
                    ? (point)
                    : (point1);
                pointlist1.Add(firstpo);

                var secondpo = secondLinefirst.DistanceTo(point) < secondLinefirst.DistanceTo(point1) ? point : point1;

                pointlist2.Insert(0, secondpo);

                #region fortest

                //for (int i = 0; i < pointlist1.Count - 1; i++)
                //{
                //    var curpo = pointlist1.ElementAt(i);//wire.GetVertex(i);
                //    var nextpo = pointlist1.ElementAt(i + 1);//wire.GetVertex(i + 1);
                //    var line = Line.CreateBound(curpo, nextpo);
                //    doc.NewLine(line);

                //}
                //for (int i = 0; i < pointlist2.Count - 1; i++)
                //{
                //    var curpo = pointlist2.ElementAt(i);//wire.GetVertex(i);
                //    var nextpo = pointlist2.ElementAt(i + 1);//wire.GetVertex(i + 1);
                //    var line = Line.CreateBound(curpo, nextpo);
                //    doc.NewLine(line);
                //}

                #endregion


                doc.Invoke(m =>
                {
                    Wire.Create(doc, wiretype.Id, acview.Id, wiringtype, pointlist1, null, null);
                    Wire.Create(doc, wiretype.Id, acview.Id, wiringtype, pointlist2, null, null);

                    doc.Delete(wire.Id);
                }, "分割导线");
                }
                catch (Exception)
                {

                    break;
                }
            }

            return Result.Succeeded;
        }
    }
}
