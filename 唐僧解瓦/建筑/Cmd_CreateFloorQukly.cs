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
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.建筑
{
    /// <summary>
    /// 一键成板（选择梁 根据梁围成的闭合区域生成板）
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    class Cmd_CreateFloorQukly : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            var acview = doc.ActiveView;

            var beamrefs = sel.PickObjects(ObjectType.Element,
                doc.GetSelectionFilter(m => m.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming),
                "选择生成板的梁");
            var beams = beamrefs.Select(m => m.GetElement(doc));

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

            Transaction temtran = new Transaction(doc, "temtran");
            temtran.Start();
            foreach (Element beam in beams)
            {
                var joinedelements = JoinGeometryUtils.GetJoinedElements(doc, beam);

                if (joinedelements.Count > 0)
                {
                    foreach (var id in joinedelements)
                    {
                        var temele = id.GetElement(doc);
                        var isjoined = JoinGeometryUtils.AreElementsJoined(doc, beam, temele);
                        if (isjoined)
                        {
                            JoinGeometryUtils.UnjoinGeometry(doc, beam, temele);
                        }
                    }
                }
            }
            temtran.RollBack();

            var solidss = new List<GeometryObject>();//beams.Select(m => m.Getsolids());
            foreach (var element in beams)
            {
                solidss.AddRange(element.Getsolids());
            }

            var joinedsolid = MergeSolids(solidss.Cast<Solid>().ToList());

            //var upfaces = beams.Select(m => m.get_Geometry(new Options() { DetailLevel = ViewDetailLevel.Fine }).Getupfaces().First());
            //MessageBox.Show(upfaces.Count().ToString());

            //var solids = upfaces.Select(m =>
            //    GeometryCreationUtilities.CreateExtrusionGeometry(m.GetEdgesAsCurveLoops(), XYZ.BasisZ, 10)).ToList();

            //MessageBox.Show("solids count" + solids.Count().ToString());
            //var targetsolid = MergeSolids(solids);

            //List<GeometryObject> geoobjs = solids.Cast<GeometryObject>().ToList();

            var upfaces = joinedsolid.Getupfaces();

            var edgeArrays = upfaces.First().EdgeLoops.Cast<EdgeArray>().ToList();

            //var curvearrays = curveloops.Select();

            var curveloops = upfaces.First().GetEdgesAsCurveLoops();

            var orderedcurveloops = curveloops.OrderBy(m => m.GetExactLength()).ToList();
            orderedcurveloops.RemoveAt(orderedcurveloops.Count - 1);
            curveloops = orderedcurveloops;
                 
            var curvearrays = curveloops.Select(m => m.ToCurveArray());


            doc.Invoke(m =>
            {
                foreach (var curvearray in curvearrays)
                {
                    doc.Create.NewFloor(curvearray, false);
                }
            },"一键楼板");

            //doc.Invoke(m =>
            //{
            //    var directshape = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            //    //directshape.AppendShape(new List<GeometryObject>(){targetsolid});
            //    //directshape.AppendShape(solidss);
            //    directshape.AppendShape(new List<GeometryObject>() { joinedsolid });
            //}, "temsolid");

            return Result.Succeeded;
        }

        public Solid MergeSolids(Solid solid1, Solid solid2)
        {
            var result = default(Solid);
            try
            {
                result = BooleanOperationsUtils.ExecuteBooleanOperation(solid1, solid2, BooleanOperationsType.Union);
            }
            catch (Exception e)
            {
                result = null;
            }
            return result;
        }

        public Solid MergeSolids(List<Solid> solids)
        {
            var result = default(Solid);
            foreach (var solid in solids)
            {
                if (result == null)
                {
                    result = solid;
                }
                else
                {
                    var temsolid = MergeSolids(result, solid);
                    if (temsolid == null) continue;
                    result = temsolid;
                }
            }
            return result;
        }

    }

    public static class TemUtils
    {
        public static List<Face> Getupfaces(this Solid solid)
        {
            var upfaces = new List<Face>();
            var faces = solid.Faces;
            foreach (Face face in faces)
            {
                var normal = face.ComputeNormal(new UV());
                if (normal.IsSameDirection(XYZ.BasisZ))
                {
                    upfaces.Add(face);
                }
            }
            return upfaces;
        }

        public static List<Face> Getupfaces(this GeometryObject geoele)
        {
            var solids = geoele.Getsolids();
            var upfaces = new List<Face>();

            foreach (var solid in solids)
            {
                var temupfaces = solid.Getupfaces();
                if (temupfaces.Count > 0)
                {
                    upfaces.AddRange(temupfaces);
                }
            }
            return upfaces;
        }

        public static List<Solid> Getsolids(this GeometryObject geoobj)
        {
            var solids = new List<Solid>();
            if (geoobj is Solid solid)
            {
                solids.Add(solid);
            }
            else if (geoobj is GeometryInstance geoInstance)
            {
                var transform = geoInstance.Transform;
                var symbolgeometry = geoInstance.SymbolGeometry;
                var enu = symbolgeometry.GetEnumerator();
                while (enu.MoveNext())
                {
                    var temgeoobj = enu.Current as GeometryObject;
                    solids.AddRange(Getsolids(temgeoobj));
                }
            }
            else if (geoobj is GeometryElement geoElement)
            {
                var enu = geoElement.GetEnumerator();
                while (enu.MoveNext())
                {
                    var temgeoobj = enu.Current as GeometryObject;
                    solids.AddRange(Getsolids(temgeoobj));
                }
            }
            return solids;
        }

        public static List<Solid> Getsolids(this GeometryObject geoobj, Transform trs)
        {

            var solids = new List<Solid>();
            if (geoobj is Solid solid)
            {
                if (trs != null || trs != Transform.Identity)
                {
                    solid = SolidUtils.CreateTransformed(solid, trs);
                }
                solids.Add(solid);
            }
            else if (geoobj is GeometryInstance geoInstance)
            {
                var transform = geoInstance.Transform;
                var symbolgeometry = geoInstance.SymbolGeometry;
                var enu = symbolgeometry.GetEnumerator();
                while (enu.MoveNext())
                {
                    var temgeoobj = enu.Current as GeometryObject;
                    solids.AddRange(Getsolids(temgeoobj, transform));
                }
            }
            else if (geoobj is GeometryElement geoElement)
            {
                var enu = geoElement.GetEnumerator();
                while (enu.MoveNext())
                {
                    var temgeoobj = enu.Current as GeometryObject;
                    solids.AddRange(Getsolids(temgeoobj, trs));
                }
            }
            return solids;
        }
        /// <summary>
        /// 从元素获取solids
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static List<GeometryObject> Getsolids(this Element element)
        {
            var result = new List<GeometryObject>();

            //var transform = default(Transform);
            //if (element is FamilyInstance)
            //    transform = (element as FamilyInstance).GetTotalTransform();
            //else transform = Transform.Identity;

            var geometryEle = element.get_Geometry(new Options() { DetailLevel = ViewDetailLevel.Fine });

            var enu = geometryEle.GetEnumerator();
            while (enu.MoveNext())
            {
                var curGeoobj = enu.Current;
                //if (curGeoobj is Solid solid)
                //{
                //    result.Add(solid);
                //}
                //else if (curGeoobj is GeometryElement)
                //{
                //}
                //else if (curGeoobj is GeometryInstance geoIns)
                //{
                //}

                result.AddRange(curGeoobj.Getsolids(Transform.Identity));
            }

            return result;
        }

        public static CurveArray ToCurveArray(this CurveLoop curveloop)
        {
            var result = new CurveArray();
            foreach (Curve c in curveloop)
            {
                result.Append(c);
            }

            return result;
        }
    }


}
