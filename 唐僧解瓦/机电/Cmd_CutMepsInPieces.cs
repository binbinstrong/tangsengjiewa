using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Electrical;
using 唐僧揭瓦.BinLibrary.Extensions;

namespace 唐僧揭瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    public class Cmd_BreakMepcurvesInPieces : IExternalCommand
    {
        
        #region uiapp uid doc 
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uid = uiapp.ActiveUIDocument;
            Document doc = uid.Document;
            Reference r = uid.Selection.PickObject(ObjectType.PointOnElement, "请选择一个分段点");
            Element elem = doc.GetElement(r);
            MEPCurve mec = elem as MEPCurve;
            Line line = (mec.Location as LocationCurve).Curve as Line;
            XYZ splitpoint = line.Project(r.GlobalPoint).XYZPoint;
            Level level = doc.ActiveView.GenLevel;
            using (Transaction ts = new Transaction(doc))
            {
                try
                {
                    ts.Start("开始分段");
                    var collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
                    var mepcollector = collector.OfClass(typeof(MEPCurve)).WhereElementIsNotElementType().Where(m => (m as MEPCurve).LocationLine().Length > 2100d.MetricToFeet()).Cast<MEPCurve>().ToList();
                    var perlength = 6000d.MetricToFeet();
                    var ductperlength = 4000d.MetricToFeet();
                    var cabletrayperlength = 2000d.MetricToFeet();
                    var pipeperlength = 6000d.MetricToFeet();
                    foreach (MEPCurve mep in mepcollector)
                    {
                        if (mep is CableTray)
                        {
                            perlength = cabletrayperlength;
                        }
                        else if (mep is Duct)
                        {
                            perlength = ductperlength;
                        }
                        else if (mep is Pipe)
                        {
                            perlength = pipeperlength;
                        }
                        var temline = mep.LocationLine();
                        var length = temline.Length;
                        if (length <= perlength) continue;
                        var points = new List<XYZ>();
                        for (int i = 0; i < length / perlength; i++)
                        {
                            var tempoint = temline.StartPoint() + perlength * i * temline.Direction;
                            points.Add(tempoint);
                        }
                        var temmeplist = new List<ElementId>();
                        BreakMep(mep, points, ref temmeplist);
                        foreach (ElementId mepid in temmeplist)
                        {
                            foreach (ElementId mepid2 in temmeplist)
                            {
                                if (mepid.IntegerValue != mepid2.IntegerValue)
                                {
                                    var temmep = mepid.GetElement(doc) as MEPCurve;
                                    var temmep2 = mepid2.GetElement(doc) as MEPCurve;
                                    var lien1 = temmep.LocationLine();
                                    var line2 = temmep2.LocationLine();
                                    var cons1 = temmep.ConnectorManager.Connectors;
                                    var cons2 = temmep2.ConnectorManager.Connectors;
                                    var enu1 = cons1.GetEnumerator();
                                    var enu2 = cons2.GetEnumerator();
                                    enu2.Reset();
                                    while (enu1.MoveNext())
                                    {
                                        enu2.Reset();
                                        while (enu2.MoveNext())
                                        {
                                            var con1 = enu1.Current as Connector;
                                            var con2 = enu2.Current as Connector;
                                            var origin1 = con1?.Origin;
                                            var origin2 = con2?.Origin;
                                            if (origin1 != null && origin2 != null && origin1.IsAlmostEqualTo(origin2))
                                            {
                                                doc.Create.NewUnionFitting(con1, con2);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ts.Commit();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            return Result.Succeeded;
            #endregion
        }
        private void BreakMep(MEPCurve mep, List<XYZ> points, ref List<ElementId> meplist)
        {
            if (points.Count < 1) return;
            if (!meplist.Contains(mep.Id))
                meplist.Add(mep.Id);
            var point = points.ElementAt(0);
            var leftpoints = points.Remove(point);
            MEPCurve newmep = null;
            var flag1 = point.IsOnLine(mep.LocationLine());
            var flag2 = mep.LocationLine().StartPoint().DistanceTo(point) > 1d.MetricToFeet();
            var flag3 = mep.LocationLine().EndPoint().DistanceTo(point) > 1d.MetricToFeet();
            if (flag1 && flag2 && flag3)
            {
                newmep = mep.BreakCurve(point);
            }
            if (newmep != null)
                meplist.Add(newmep.Id);
            var pointsonmep = points.Where(m => m.IsOnLine(mep.LocationLine())).ToList();
            if (pointsonmep.Count > 0)
            {
                BreakMep(mep, pointsonmep, ref meplist);
            }
            var pointsonnewmep = new List<XYZ>();
            if (newmep != null)
            {
                pointsonnewmep = points.Where(m => m.IsOnLine(newmep.LocationLine())).ToList();
                if (pointsonnewmep.Count > 0)
                    BreakMep(newmep, pointsonnewmep, ref meplist);
            }
        }
    }
    public static class TemExtension
    {
        public static MEPCurve BreakCurve(this MEPCurve mep, XYZ point)
        {
            var locationline = mep.LocationLine();
            var start = locationline.StartPoint();
            var end = locationline.EndPoint();
            var executeflag = point.IsOnLine(locationline) && point.DistanceTo(start) > 1d.MetricToFeet() &&
                              point.DistanceTo(end) > 1d.MetricToFeet();
            if (!executeflag) throw new Exception("点不在线上");
            var doc = mep.Document;
            ElementId result = null;
            if (mep is Duct)
            {
                result = MechanicalUtils.BreakCurve(doc, mep.Id, point);
            }
            else if (mep is Pipe)
            {
                result = PlumbingUtils.BreakCurve(doc, mep.Id, point);
            }
            else if (mep is CableTray)
            {
                var newline1 = Line.CreateBound(start, point);
                var newline2 = Line.CreateBound(point, end);
                (mep.Location as LocationCurve).Curve = newline1;
                var newcabletray = CableTray.Create(doc, mep.GetTypeId(), point, end, mep.ReferenceLevel.Id);
                var para_w = newcabletray.get_Parameter(BuiltInParameter.RBS_CABLETRAY_WIDTH_PARAM);
                var para_H = newcabletray.get_Parameter(BuiltInParameter.RBS_CABLETRAY_HEIGHT_PARAM);
                para_w.Set(mep.Width);
                para_H.Set(mep.Height);
                result = newcabletray.Id;
            }
            return result.GetElement(doc) as MEPCurve;
        }
    }
}
