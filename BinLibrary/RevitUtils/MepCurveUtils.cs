using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BinLibrary.RevitExtension;
using BinLibrary.RevitHelper;
namespace BinLibrary.RevitUtils
{
    public class MepCurveUtils
    {
        /// <summary>
        /// 用于打断桥架(风管水管用api自带的方法)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="mepId"></param>
        /// <param name="breakpoint"></param>
        /// <returns></returns>
        public static bool BreakCurve(Document doc, ElementId mepId, XYZ breakpoint)
        {
            var result = true;
            try
            {
                var mepcurve = doc.GetElement(mepId) as MEPCurve;
                //if(mepcurve ==null) throw new Exception("目标不是机电管线");
                var connectorman = mepcurve.ConnectorManager;
                var startpoint = mepcurve.StartPoint();
                var endpoint = mepcurve.EndPoint();
                var cons = connectorman.Connectors.PhysicalConToList();
                var backcon1 = cons.ElementAt(0).GetConnectedCon();
                var backcon2 = cons.ElementAt(1).GetConnectedCon();
                var line1 = Line.CreateBound(startpoint, breakpoint);
                var line2 = Line.CreateBound(breakpoint, endpoint);
                (mepcurve.Location as LocationCurve).Curve = line1;
                var copyResult = ElementTransformUtils.CopyElement(doc, mepId, breakpoint - startpoint);
                var mepcurveCopy = copyResult.First().GetElement(doc) as MEPCurve;
                (mepcurveCopy.Location as LocationCurve).Curve = line2;
                var overlapcon1 = mepcurve.ConnectorManager.Connectors.PhysicalConToList();
                var overlapcon2 = mepcurveCopy.ConnectorManager.Connectors.PhysicalConToList();
                var unionCons = overlapcon1.Union(overlapcon2).ToList();
                unionCons.ForEach(m =>
                {
                    if (backcon1.Origin.IsAlmostEqualTo(m.Origin)) m.ConnectTo(backcon1);
                    if (backcon2.Origin.IsAlmostEqualTo(m.Origin)) m.ConnectTo(backcon2);
                });
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        public static bool BreakCurve(Document doc, ElementId mepId, XYZ breakpoint, out MEPCurve mep)
        {
            var result = true;
            try
            {
                var mepcurve = doc.GetElement(mepId) as MEPCurve;
                //if(mepcurve ==null) throw new Exception("目标不是机电管线");
                var connectorman = mepcurve.ConnectorManager;
                var startpoint = mepcurve.StartPoint();
                var endpoint = mepcurve.EndPoint();
                var cons = connectorman.Connectors.PhysicalConToList();
                var backcon1 = cons.ElementAt(0).GetConnectedCon();
                var backcon2 = cons.ElementAt(1).GetConnectedCon();
                var line1 = Line.CreateBound(startpoint, breakpoint);
                var line2 = Line.CreateBound(breakpoint, endpoint);
                (mepcurve.Location as LocationCurve).Curve = line1;
                var copyResult = ElementTransformUtils.CopyElement(doc, mepId, breakpoint - startpoint);
                var mepcurveCopy = copyResult.First().GetElement(doc) as MEPCurve;
                (mepcurveCopy.Location as LocationCurve).Curve = line2;
                var overlapcon1 = mepcurve.ConnectorManager.Connectors.PhysicalConToList();
                var overlapcon2 = mepcurveCopy.ConnectorManager.Connectors.PhysicalConToList();
                var unionCons = overlapcon1.Union(overlapcon2).ToList();
                unionCons.ForEach(m =>
                {
                    if (backcon1 != null && backcon1.Origin.IsAlmostEqualTo(m.Origin)) m.ConnectTo(backcon1);
                    if (backcon2 != null && backcon2.Origin.IsAlmostEqualTo(m.Origin)) m.ConnectTo(backcon2);
                });
                mep = mepcurveCopy;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                result = false;
                mep = null;
            }
            return result;
        }
        public static bool BreakCurve(Document doc, ElementId mepId, List<XYZ> points)
        {
            var result = true;
            MEPCurve mepcurve = mepId.GetElement(doc) as MEPCurve;
            var line = mepcurve.LocationLine();
            var pointscount = points.Count;
            if (pointscount > 1)
                for (int i = 0; i < pointscount; i++)
                {
                    if (i >= points.Count) break;
                    //MessageBox.Show(points.Count.ToString() + Environment.NewLine +
                    //                i);
                    var point = points.ElementAt(i);
                    if (point.IsAlmostEqualTo(line.StartPoint()) || point.IsAlmostEqualTo(line.EndPoint()))
                    {
                        points.Remove(point);
                        i--;
                    }
                }
            var onlineflag = true;
            //points.ForEach(m =>
            //{
            //    if (!m.IsOnLine(line)) onlineflag = false;
            //});
            if (!onlineflag) throw new Exception("points is not on line");
            //MessageBox.Show(points.Count.ToString());
            if (points.Count > 1)
            {
                var targetpoint = points.ElementAt(0);
                points.RemoveAt(0);
                var leftpoints = points;
                MEPCurve mep2 = null;
                BreakCurve(doc, mepId, targetpoint, out mep2);
                var pointsOnmep1 = leftpoints.Where(m => m.IsOnLine(mepcurve.LocationLine()));
                //MessageBox.Show(pointsOnmep1.Count().ToString());
                if (pointsOnmep1.Count() >= 1)
                {
                    ////bin test
                    //new UIDocument(doc).Selection.SetElementIds(new List<ElementId>() {mepcurve.Id});
                    //MessageBox.Show("selected1" + Environment.NewLine +
                    //                pointsOnmep1.Count());
                    BreakCurve(doc, mepcurve.Id, pointsOnmep1.ToList());
                }
                IEnumerable<XYZ> pointsOnmep2 = new List<XYZ>();
                if (mep2 != null)
                {
                    ////bin test
                    //new UIDocument(doc).Selection.SetElementIds(new List<ElementId>() { mep2.Id });
                    //MessageBox.Show("selected2");
                    pointsOnmep2 = leftpoints.Where(m => m.IsOnLine(mep2.LocationLine()));
                    //MessageBox.Show(pointsOnmep2.Count().ToString());
                    if (pointsOnmep2.Count() >= 1)
                    {
                        BreakCurve(doc, mep2.Id, pointsOnmep2.ToList());
                    }
                }
            }
            else if (points.Count == 1)
            {
                BreakCurve(doc, mepId, points.First());
            }
            return result;
        }
    }
}
