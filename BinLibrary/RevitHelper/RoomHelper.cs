using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using BinLibrary.RevitExtension;
using System;
using System.Collections.Generic;

namespace BinLibrary.RevitHelper
{
    public static class RoomHelper
    {
        public static void createNewRooms(Document doc)
        {
            if (doc.ActiveView.ViewType == ViewType.FloorPlan)
            {
                Level genLevel = doc.ActiveView.GenLevel;
                PlanTopology planTopology = doc.get_PlanTopology(genLevel);
                foreach (PlanCircuit planCircuit in planTopology.Circuits)
                {
                    bool isRoomLocated = planCircuit.IsRoomLocated;
                    if (isRoomLocated)
                    {
                        Room room = doc.Create.NewRoom(null, planCircuit);
                        LinkElementId linkElementId = new LinkElementId(room.Id);
                        Location location = room.Location;
                        LocationPoint locationPoint = location as LocationPoint;
                        XYZ point = locationPoint.Point;
                        UV uV = new UV(point.X, point.Y);
                    }
                }
            }
        }

        public static IList<BoundarySegment> GetHorizontalBoundary(Room room)
        {
            IList<BoundarySegment> result = new List<BoundarySegment>();
            GeometryElement closedShell = room.ClosedShell;
            IList<Face> facesOfGeometryObject = closedShell.GetFacesOfGeometryObject();
            return result;
        }
    }
}
