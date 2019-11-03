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

            var selectionCollector = new FilteredElementCollector(doc, sel.GetElementIds());//选择集集合

            var beamFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);

            var roofFilter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);
            var floorFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            var rampFilter = new ElementCategoryFilter(BuiltInCategory.OST_Ramps);
            var structuralFoundationFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);

            var roofcollector = new FilteredElementCollector(doc).WhereElementIsNotElementType().WherePasses(roofFilter);
            var floorCollector = new FilteredElementCollector(doc).WhereElementIsNotElementType().WherePasses(floorFilter);
            var rampCollector = new FilteredElementCollector(doc).WhereElementIsNotElementType().WherePasses(rampFilter);
            var strFoundationCollector = new FilteredElementCollector(doc).WhereElementIsNotElementType().WherePasses(structuralFoundationFilter);
            var beamCollector = new FilteredElementCollector(doc).WhereElementIsNotElementType().WherePasses(beamFilter);

            //（1.梁随屋面）将与屋面在同一层的梁进行处理 使之紧贴屋面
            // -1. 获取屋面顶面或底面边界线

            var rooffaces = default(IList<Reference>);
            foreach (RoofBase roof in roofcollector)
            {
                if (IsAlignBottomFAce)
                {
                    rooffaces = HostObjectUtils.GetBottomFaces(roof);

                }
                else if (IsAlignTopFAce)
                {
                    rooffaces = HostObjectUtils.GetTopFaces(roof);
                }
                //排除空引用 
                rooffaces = rooffaces.Where(m => roof.GetGeometryObjectFromReference(m) as Face != null).ToList();

                //for test
                #region test  weather face is null
                foreach (var reference in rooffaces)
                {
                    //var type = roof.GetGeometryObjectFromReference(reference).GetType().ToString();
                    //MessageBox.Show(type);

                    var face = roof.GetGeometryObjectFromReference(reference) as Face;
                    if (face != null)
                    {
                        var edgeloops = face.GetEdgesAsCurveLoops();
                        foreach (var edgeloop in edgeloops)
                        {
                            foreach (Curve c in edgeloop)
                            {
                                doc.NewLine(c as Line);
                            }
                        }
                    }
                    else
                    {
                        //var face1 = roof.GetGeometryObjectFromReference(reference);
                        //MessageBox.Show("null");
                    }
                }
                #endregion

                if (rooffaces.Count == 0 || rooffaces == null) continue;

                //确定屋面所在楼层
                var currentLevelId = roof.LevelId;
                var currentLevel = roof.LevelId.GetElement(doc) as Level;
                //找到本楼层的梁
                var beamsOfThisFloor = beamCollector.Where(m => m.LevelId == currentLevelId).ToList();

                //用屋面边线切断所有 投影相交的梁

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
    }
}
