using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧揭瓦.BinLibrary.Extensions;
using 唐僧揭瓦.BinLibrary.Helpers;


namespace 唐僧揭瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]

    class Cmd_VerticalPipeConvert : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            View acview = uidoc.ActiveView;
            //UIView acuivew = uidoc.Activeuiview();

            var pipetype = doc.TCollector<PipeType>().First();
            var pipesystype = doc.TCollector<PipingSystemType>().First();
            var level = acview.GenLevel;
            var bottomele = level.Elevation;
            var topele = bottomele + 4000d / 304.8;
             
            var georef = sel.PickObject(ObjectType.PointOnElement);
            var cadlinkinstance = georef.GetElement(doc) as Instance;
            var cadgeometry = cadlinkinstance.get_Geometry(new Options());
            var cadGraphicStyle = cadgeometry.GraphicsStyleId.GetElement(doc);
            var transform = cadlinkinstance.GetTransform();
            var geomery = cadlinkinstance.GetGeometryObjectFromReference(georef);
             
            var gs = geomery.GraphicsStyleId.GetElement(doc) as GraphicsStyle;
            var gsc = gs.GraphicsStyleCategory;
            var layername = gsc.Name;
            MessageBox.Show(layername);

            var geometry = cadlinkinstance.get_Geometry(new Options());
            var geometries = geometry.GetGeometries();
            var count = 0;
            try
            {
                count = 0;
                foreach (GeometryObject geoobj in geometries)
                {
                     
                    var geoins = geoobj as GeometryInstance;
                    var symbolgeometry = geoins.GetSymbolGeometry();
                    var geoenu = symbolgeometry.GetEnumerator();
                  
                    geoenu.Reset();
                    while (geoenu.MoveNext())
                    {
                        if (count > 10) break;
                        var obj = geoenu.Current;
                        var arcgs = obj.GraphicsStyleId.GetElement(doc) as GraphicsStyle;
                        var arcgsName = arcgs.GraphicsStyleCategory.Name;

                        //if (arcgsName == layername)
                            //MessageBox.Show(arcgsName);

                        if (obj is Arc)
                        {

                            //MessageBox.Show("find arc");
                            var arc = obj as Arc;
                            var arccenter = arc.Center;
                            var line = Line.CreateBound(arc.Center, arc.Center + 10 * XYZ.BasisZ);
                            line = line.CreateTransformed(transform) as Line;
                            //doc.NewLine(line);

                            arccenter = transform. OfPoint(arccenter);
                            //doc.NewLine(line);
                            var startpo = new XYZ(arccenter.X, arccenter.Y, bottomele);
                            var endpo = new XYZ(arccenter.X, arccenter.Y, topele);
                            if(arcgsName==layername)
                            doc.Invoke(m => { Pipe.Create(doc, pipesystype.Id, pipetype.Id, level.Id, startpo, endpo); }, "创建立管");
                            //count++;
                        }
                        else if (obj is Ellipse)
                        {
                            
                            //count++;
                            var arc = obj as Ellipse;
                            var line = Line.CreateBound(arc.Center, arc.Center + 10 * XYZ.BasisZ);
                            line = line.CreateTransformed(transform) as Line;
                            //doc.NewLine(line);
                        }
                        else if (obj is GeometryInstance)
                        {
                             
                            var geoinstance1 = obj as GeometryInstance;
                            var transform1 = geoinstance1.Transform;
                            var geometrysmbol = (geoinstance1).SymbolGeometry;
                            var enu1 = geometrysmbol.GetEnumerator();
                            var gs1 = obj.GraphicsStyleId.GetElement(doc) as GraphicsStyle;
                            var gs1Name = gs1.GraphicsStyleCategory.Name;

                            while (enu1.MoveNext())
                            {
                                var obj1 = enu1.Current;
                                if (obj1 is Arc)
                                {
                                    var arc = obj1 as Arc;
                                    var arccenter = arc.Center;
                                    var line = Line.CreateBound(arccenter, arccenter + 10 * XYZ.BasisZ);
                                    line = line.CreateTransformed(transform.Multiply(transform1)) as Line;
                                    arccenter = transform.Multiply(transform1).OfPoint(arccenter);
                                    //doc.NewLine(line);
                                    var startpo = new XYZ(arccenter.X, arccenter.Y, bottomele);
                                    var endpo = new XYZ(arccenter.X, arccenter.Y, topele);
                                    if (arcgsName == layername)
                                        doc.Invoke(m => { Pipe.Create(doc, pipesystype.Id, pipetype.Id, level.Id, startpo, endpo); },"创建立管");
                                }
                                else if(obj1 is Ellipse)
                                {
                                    var arc = obj1 as Ellipse;
                                    if (arc == null) continue;
                                    var line = Line.CreateBound(arc.Center, arc.Center + 10 * XYZ.BasisZ);
                                    line = line.CreateTransformed(transform.Multiply(transform1)) as Line;
                                    //doc.NewLine(line);
                                }
                            }
                        }

                        //count++;
                    }
                    count++;
                }
 
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                //throw;
            }
            return Result.Succeeded;
        }
    }
}
