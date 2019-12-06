using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.建筑
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_BeamAlignToRoofAndFloor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            var acview = doc.ActiveView;

            var IsAlignTopFAce = false;   //根据设置确定
            var IsAlignBottomFAce = true; //根据设置确定

            var selectedIds = sel.GetElementIds();

            var selectionCollector = new FilteredElementCollector(doc, selectedIds);//选择集集合

            var beamFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);

            var roofFilter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);
            var floorFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            var rampFilter = new ElementCategoryFilter(BuiltInCategory.OST_Ramps);
            var structuralFoundationFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);

            var roofcollector = new FilteredElementCollector(doc, selectedIds).WhereElementIsNotElementType().WherePasses(roofFilter);
            var floorCollector = new FilteredElementCollector(doc, selectedIds).WhereElementIsNotElementType().WherePasses(floorFilter);
            var rampCollector = new FilteredElementCollector(doc, selectedIds).WhereElementIsNotElementType().WherePasses(rampFilter);
            var strFoundationCollector = new FilteredElementCollector(doc, selectedIds).WhereElementIsNotElementType().WherePasses(structuralFoundationFilter);
            var beamCollector = new FilteredElementCollector(doc, selectedIds).WhereElementIsNotElementType().WherePasses(beamFilter);

            //（1.梁随屋面）将与屋面在同一层的梁进行处理 使之紧贴屋面
            // -1. 获取屋面顶面或底面边界线

            var floorfaces = default(IList<Reference>);
            foreach (Floor floor in floorCollector)
            {
                if (IsAlignBottomFAce)
                {
                    floorfaces = HostObjectUtils.GetBottomFaces(floor);
                }
                else if (IsAlignTopFAce)
                {
                    floorfaces = HostObjectUtils.GetTopFaces(floor);
                }
                //排除空引用 
                floorfaces = floorfaces.Where(m => floor.GetGeometryObjectFromReference(m) as Face != null).ToList();

                //for test
                #region test  weather face is null
                //foreach (var reference in floorfaces)
                //{
                //    //var type = roof.GetGeometryObjectFromReference(reference).GetType().ToString();
                //    //MessageBox.Show(type);

                //    var face = floor.GetGeometryObjectFromReference(reference) as Face;
                //    if (face != null)
                //    {
                //        var edgeloops = face.GetEdgesAsCurveLoops();
                //        foreach (var edgeloop in edgeloops)
                //        {
                //            foreach (Curve c in edgeloop)
                //            {
                //                doc.NewLine(c as Line);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        //var face1 = roof.GetGeometryObjectFromReference(reference);
                //        //MessageBox.Show("null");
                //    }
                //}
                #endregion

                if (floorfaces.Count == 0 || floorfaces == null) continue;
                 
                //用屋面边线切断所有 投影相交的梁

                foreach (FamilyInstance beam in beamCollector)
                {
                    
                    
                    
                    
                }



            }

            return Result.Succeeded;
        }

        public List<Curve> GetEdgeCurves(Element ele, List<Reference> faceRefs)
        {
            var result = new List<Curve>();
            foreach (var reference in faceRefs)
            {
                var temface = ele.GetGeometryObjectFromReference(reference) as Face;
                if (temface == null) continue;
                var curveloops = temface.GetEdgesAsCurveLoops();
                foreach (CurveLoop curveloop in curveloops)
                {
                    result.AddRange(curveloop.Cast<Curve>().ToList());
                }
            }
            return result;
        }

        /// <summary>
        /// 构造点所在平面
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public Plane ConstructPlane(XYZ origin, XYZ vector1, XYZ vector2)
        {
            var result = default(Plane);
            result = Plane.CreateByNormalAndOrigin(vector1.CrossProduct(vector2).Normalize(), origin);
            return result;
        }
        /// <summary>
        /// 构造线所在平面
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public Plane ConstructPlane(Line line, XYZ vector)
        {
            var result = Plane.CreateByNormalAndOrigin(line.Direction.CrossProduct(vector).Normalize(), line.Origin);
            return result;
        }

        public List<FamilyInstance> CutBeam(FamilyInstance beam, XYZ point)
        {
            var result = new List<FamilyInstance>();
            var doc = beam.Document;

            var locationcurve = beam.Location as LocationCurve;
            var locationline = locationcurve.Curve as Line;
            if (locationline == null) return result;

            point = point.ProjectToXLine(locationline);
            if (point.IsOnLine(locationline))
            {
                var start = locationline.StartPoint();
                var end = locationline.EndPoint();

                var line1 = Line.CreateBound(start, point);
                var line2 = Line.CreateBound(point, end);

                (beam.Location as LocationCurve).Curve = line1;

                var copiedBeams = ElementTransformUtils.CopyElement(beam.Document, beam.Id, new XYZ());
                var beam2Id = copiedBeams.First();
                var beam2 = beam2Id.GetElement(doc) as FamilyInstance;

                (beam2.Location as LocationCurve).Curve = line2;

                result.Add(beam);
                result.Add(beam2);
            }
            else
            {
                throw new Exception("point is not on beam,can not cut beam!");
            }

            return result;
        }

        public List<FamilyInstance> CutBeam(FamilyInstance beam, Plane p)
        {
            var locationcurve = beam.Location as LocationCurve;
            var locationline = locationcurve.Curve as Line;

            var intersectPo = locationline.Intersect_cus(p);

            return CutBeam(beam, intersectPo);
        }
    }
}
