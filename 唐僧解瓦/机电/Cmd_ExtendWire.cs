using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.机电
{
    /// <summary>
    /// 延长导线命令 <electrical>
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_ExtendWire : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            var acview = doc.ActiveView;

            while (true)
            {
                try
                {
                    var wireref = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Wire));
                    var wire = wireref.GetElement(doc) as Wire;
                    //var connectors = (wire).ConnectorManager.Connectors.PhysicalConToList();
                    var locationcurve = wire.Location as LocationCurve;

                    //var con1 = connectors.First();
                    //var con2 = connectors.Last();
                    //var origin1 = con1.Origin;
                    //var origin2 = con2.Origin;
                    //var dir1 = connectors.First().CoordinateSystem.BasisZ;
                    //var dir2 = connectors.Last().CoordinateSystem.BasisZ;

                    //var line1 = Line.CreateUnbound(origin1, dir1);
                    //var line2 = Line.CreateUnbound(origin2, dir2);

                    var intersection = default(XYZ);

                    //if (!(locationcurve.Curve is Line))
                    //{
                    //    MessageBox.Show("本功能不适应于曲线类型的导线");
                    //}

                    var tempoint = sel.PickPoint();
                    var linefirst = Line.CreateBound(wire.GetVertex(0), wire.GetVertex(1));
                    var vertexcount = wire.NumberOfVertices;
                    var linelast = Line.CreateBound(wire.GetVertex(vertexcount - 2), wire.GetVertex(vertexcount - 1));

                    var globalpoint = wireref.GlobalPoint;

                    globalpoint = new XYZ(globalpoint.X, globalpoint.Y, linefirst.StartPoint().Z);


                    if (!globalpoint.IsOnLine(linefirst) && !globalpoint.IsOnLine(linelast))
                    {
                        MessageBox.Show("本功能不适应于曲线类型的导线 选择起始端或结束端导线" 
                                        //+ Environment.NewLine +
                                        //globalpoint.IsOnLine(linefirst).ToString() + Environment.NewLine +
                                        //globalpoint.IsOnLine(linelast).ToString() + Environment.NewLine +
                                        //globalpoint.ToString() + Environment.NewLine +
                                        //linefirst.StartPoint().ToString() + Environment.NewLine +
                                        //linelast.StartPoint().ToString()
                                        );
                        continue;
                    }

                    Transaction ts1 = new Transaction(doc, "更改导线");
                    ts1.Start();
                    if (vertexcount > 2)
                    {
                        if (globalpoint.IsOnLine(linefirst))
                        {
                            tempoint = tempoint.ProjectToXLine(linefirst);
                            wire.SetVertex(0, tempoint);
                        }
                        else if (globalpoint.IsOnLine(linelast))
                        {
                            tempoint = tempoint.ProjectToXLine(linelast);
                            wire.SetVertex(vertexcount - 1, tempoint);
                        }
                    }
                    else if (vertexcount == 2)
                    {
                        var startpo = wire.GetVertex(0);
                        var endpo = wire.GetVertex(1);
                        if (globalpoint.DistanceTo(startpo) < globalpoint.DistanceTo(endpo))
                        {
                            tempoint = tempoint.ProjectToXLine(linefirst);
                            wire.SetVertex(0, tempoint);
                        }
                        else
                        {
                            tempoint = tempoint.ProjectToXLine(linelast);
                            wire.SetVertex(vertexcount - 1, tempoint);
                        }
                    }
                    ts1.Commit();
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
