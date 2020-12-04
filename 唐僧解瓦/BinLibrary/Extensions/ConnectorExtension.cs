using Autodesk.Revit.DB;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class ConnectorExtension
    {
        public static Connector GetConnectedCon(this Connector connector)
        {
            var result = default(Connector);
            var conectors = connector.AllRefs;
            var connectordir = connector.CoordinateSystem.BasisZ;
            var connectorOrigin = connector.Origin;

            foreach (Connector con in conectors)
            {
                if (con.IsConnectedTo(connector) && con.ConnectorType != ConnectorType.Logical)
                {
                    result = con;
                }

                //if (con.ConnectorType == ConnectorType.End )
                //{
                //    var conOrigin = con.Origin;
                //    var condir = con.CoordinateSystem.BasisZ;
                //    if (connectorOrigin.IsAlmostEqualTo(conOrigin) && connectordir.IsOppositeDirection(condir))
                //    {
                //        result = con;
                //    }
                //}
                //else if (con.ConnectorType == ConnectorType.Curve)
                //{
                //    var conOrigin = con.Origin;
                //    var condir = con.CoordinateSystem.BasisZ;
                //    if (connectorOrigin.IsAlmostEqualTo(conOrigin) && connectordir.IsOppositeDirection(condir))
                //    {
                //        result = con;
                //    }
                //}
            }
            return result;
        }
    }
}
