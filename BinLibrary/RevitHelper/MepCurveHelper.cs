using Autodesk.Revit.DB;
using BinLibrary.RevitExtension;
using System;

namespace BinLibrary.RevitHelper
{
    public static class MepCurveHelper
    {
        /// <summary>
        /// 延长距离指定点近的点为新的newend
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="newend"></param>
        /// <returns></returns>
        public static MEPCurve ExtendCurve(this MEPCurve curve, XYZ newend)
        {
            Line line = curve.LocationLine();
            bool flag = !newend.IsXOnLine(line);
            if (flag)
            {
                newend = newend.ProjectToXLine(line);
            }
            XYZ xYZ = line.GetEndPoint(0);
            XYZ xYZ2 = line.GetEndPoint(1);
            bool flag2 = xYZ.DistanceTo(newend) <= xYZ2.DistanceTo(newend);
            if (flag2)
            {
                xYZ = newend;
            }
            else
            {
                xYZ2 = newend;
            }
            (curve.Location as LocationCurve).Curve = (Line.CreateBound(xYZ, xYZ2));
            return curve;
        }

        /// <summary>
        /// 延长距离指定点（positioner）近的端点 为 newend
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="positioner"></param>
        /// <param name="newend"></param>
        /// <returns></returns>
        public static MEPCurve ExtendCurve(this MEPCurve curve, XYZ positioner, XYZ newend)
        {
            Line line = curve.LocationLine();
             
            if (newend.IsXOnLine(line))
            {
                newend = newend.ProjectToXLine(line);
            }
             
            if (!positioner.IsXOnLine(line))
            {
                positioner = positioner.ProjectToXLine(line);
            }
            XYZ startpoint = line.GetEndPoint(0);
            XYZ endpo = line.GetEndPoint(1);
            
            if (startpoint.DistanceTo(positioner) < endpo.DistanceTo(positioner))
            {
                startpoint = newend;
            }
            else
            {
                endpo = newend;
            }
            (curve.Location as LocationCurve).Curve = (Line.CreateBound(startpoint, endpo));
            return curve;
        }

        /// <summary>
        /// 延长距离指定点近的端点 一定的距离
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="positioner"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static MEPCurve ExtendCurve(this MEPCurve curve, XYZ positioner, double distance)
        {
            Line line = curve.LocationLine();
            bool flag = !positioner.IsXOnLine(line);
            if (flag)
            {
                positioner = positioner.ProjectToXLine(line);
            }
            XYZ xYZ = line.GetEndPoint(0);
            XYZ xYZ2 = line.GetEndPoint(1);
            bool flag2 = xYZ.DistanceTo(positioner) <= xYZ2.DistanceTo(positioner);
            if (flag2)
            {
                xYZ -= distance * line.Direction;
            }
            else
            {
                xYZ2 += distance * line.Direction;
            }
            (curve.Location as LocationCurve).Curve = (Line.CreateBound(xYZ, xYZ2));
            return curve;
        }
    }
}
