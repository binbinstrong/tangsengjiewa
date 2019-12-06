using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 净高分析
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_netHeightAnalyses : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            var collector = new FilteredElementCollector(doc);
            //var rooms = collector.OfClass(typeof(Room));

            //var phases = doc.Phases;
            //foreach (Phase phase in phases)
            //{
            //    MessageBox.Show(phase.Name.ToString());
            //}

            if (!(doc.ActiveView is ViewPlan))
            {
                MessageBox.Show("请转到平面视图");
                return Result.Cancelled;
            }
            //创建房间（根据墙围成的闭合图形生成房间）
            var rooms = new List<SpatialElement>();
            doc.Invoke(m =>
            {
                Createrooms(doc, doc.ActiveView.GenLevel, doc.Phases.get_Item(1));
            }, "当前视图楼层创建房间");
            //下一步用创建的房间进行标高分析
            rooms = doc.TCollector<SpatialElement>().ToList();
            MessageBox.Show(rooms.Count.ToString());

            var names = rooms.Select(m => m.Name);

            var namestring = string.Join("\n", names);

            MessageBox.Show(namestring);

            var geometrys = new List<GeometryObject>();
            foreach (SpatialElement spatialElement in rooms)
            {
                var geometry = default(GeometryObject);

                if (SpatialElementGeometryCalculator.IsRoomOrSpace(spatialElement))
                {
                    MessageBox.Show(spatialElement.Name +"is roomOrSpace");
                }
                else
                {
                    MessageBox.Show(spatialElement.Name + "is not roomOrSpace");
                }

                try
                {
                    geometry = new SpatialElementGeometryCalculator(doc).CalculateSpatialElementGeometry(spatialElement)
                        .GetGeometry();
                }
                catch (Exception e)
                {
                    var boundaries = spatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions());
                    MessageBox.Show(boundaries.Count.ToString());

                    foreach (var boundarySegments in boundaries)
                    {
                        foreach (var boundarySegment in boundarySegments)
                        {
                            var curve = boundarySegment.GetCurve() as Line;
                            if(curve==null) MessageBox.Show("curve is null");
                            doc.NewLine(curve);
                        }
                    }
                    MessageBox.Show("wrong Message Skip this loop");
                    continue;
                }
               
                //geometrys.Add(geometry);
                var geometrys1 = new List<GeometryObject>() { geometry };

#if Revit2019
                doc.Invoke(m =>
                {
                    var directShape = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
                    directShape.AppendShape(geometrys1);
                }, "创建内建模型");
#endif



            }

            //SpatialElementGeometryCalculator spatialGeometryCal = new SpatialElementGeometryCalculator(doc);
            //foreach (Room room in rooms)
            //{
            //    var roomGeo = spatialGeometryCal.CalculateSpatialElementGeometry(room);
            //}

            return Result.Succeeded;
        }

        /// <summary>
        /// 指定楼层根据topology 创建房间
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="level"></param>
        /// <param name="curPhase"></param>
        public List<Room> Createrooms(Document doc, Level level, Phase curPhase)
        {
            var rooms = new List<Room>();

            //var pologies = doc.PlanTopologies;
            var pology = doc.get_PlanTopology(level);
            var circuits = pology.Circuits;
            //MessageBox.Show(circuits.Size.ToString());
            //新相位
            var newphase = doc.Phases.Cast<Phase>().Where(m => m.Name.Contains("新构造") || m.Name.ToLower().Contains("new")).First();
            if (newphase == null) return rooms;
            foreach (PlanCircuit circuit in circuits)
            {
                var sheduleroom = doc.Create.NewRoom(newphase);
                var room = doc.Create.NewRoom(sheduleroom, circuit);
                rooms.Add(room);
            }

            
            return rooms;
        }
    }
}
