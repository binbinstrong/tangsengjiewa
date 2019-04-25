using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using BinLibrary.RevitExtension;
using BinLibrary.Extensions;

namespace BinLibrary.RevitHelper
{
    public static class MepcurveConnectHelper
    {
        /// <summary>
        /// 弯头连接两条管道 若是共线用变径连接 若是 共线管径相等则删除第二条管道 后两者返回值为空
        /// </summary>
        /// <param name="mepc1"></param>
        /// <param name="mepc2"></param>
        /// <returns></returns>
        public static FamilyInstance ElbowConnectTo(this MEPCurve mepc1, MEPCurve mepc2)
        {
            bool debugflag = false;
            if (!((mepc1 is Pipe && mepc2 is Pipe) || (mepc1 is Duct && mepc2 is Duct) ||
                (mepc2 is CableTray && mepc1 is CableTray) || (mepc1 is Conduit && mepc2 is Conduit)))
            {
                throw new Exception("BinException ConnectTo:Is not same class");
            }

            FamilyInstance result = null;
            Document doc = mepc1.Document;

            var mepcLine1 = (mepc1.Location as LocationCurve).Curve as Line;
            var mepcLine2 = (mepc2.Location as LocationCurve).Curve as Line;

            if (!mepcLine1.IsOnSamePlane(mepcLine2))
            {
                throw new Exception("BinException ConnectTo:is not on same plane");
            }

            //var mepcDir = mepcLine1.Direction;
            //var tarmepDir = mepcLine2.Direction;

            //var end1 = mepcLine1.GetEndPoint(0);
            //var end2 = mepcLine1.GetEndPoint(1);

            //var end3 = mepcLine2.GetEndPoint(0);
            //var end4 = mepcLine2.GetEndPoint(1);
            
            //记录mepc1 和 mepc2的相连的连接件
            var connectedCons = new List<Connector>();

            var connectedcons1 =
                mepc1.ConnectorManager.Connectors.PhysicalConToList().Where(m => m.IsConnected)?.Select(m => m.GetConnectedCon()).ToList();

            var connectedcons2 =
                mepc2.ConnectorManager.Connectors.PhysicalConToList().Where(m => m.IsConnected)?.Select(m => m.GetConnectedCon()).ToList();

            if (connectedcons1.Count() > 0)
            {
                connectedCons.AddRange(connectedcons1);
            }
            if (connectedcons2.Count > 0)
            {
                connectedCons.AddRange(connectedcons2);
            }


            if (mepcLine1.IsOnSameLine(mepcLine2))
            {
                //var temlist = new List<XYZ>() { end1, end2, end3, end4 };
                ////找到最外侧的两个点
                //XYZ outerpoint1 = temlist.Aggregate((m, n) => end1.DistanceTo(m) > end2.DistanceTo(n) ? m : n);
                //XYZ outerpoint2 = temlist.Aggregate((m, n) => outerpoint1.DistanceTo(m) > outerpoint1.DistanceTo(n) ? m : n);

                mepc1.TransitionConnectTo(mepc2);
            }
            else
            {
                var mepclin1_copy = mepcLine1.Clone() as Line;
                var mepclin2_copy = mepcLine2.Clone() as Line;

                //doc.NewLine_WithoutTransaction(mepclin1_copy.ExtendLine(500));
                //doc.NewLine_WithoutTransaction(mepclin2_copy);

                mepclin1_copy.MakeUnbound();
                mepclin2_copy.MakeUnbound();

                var intersection = mepclin1_copy.Intersect_cus(mepclin2_copy);
                if (debugflag)
                    MessageBox.Show(intersection.ToString());

                var connectors1 = mepc1.ConnectorManager.Connectors.PhysicalConToList();
                var connectors2 = mepc2.ConnectorManager.Connectors.PhysicalConToList();

                var nearCon1 = connectors1.Aggregate((m, n) =>
                    m.Origin.DistanceTo(intersection) > n.Origin.DistanceTo(intersection) ? n : m);

                var nearCon2 = connectors2.Aggregate((m, n) =>
                    m.Origin.DistanceTo(intersection) > n.Origin.DistanceTo(intersection) ? n : m);

                result = doc.Create.NewElbowFitting(nearCon1, nearCon2);
            }

            return result;
        }
        /// <summary>
        /// 两管共线 在中间用变径连接 （一条管道完全在另一条管道内的情况不处理）
        /// </summary>
        /// <param name="mepc1"></param>
        /// <param name="mepc2"></param>
        public static FamilyInstance TransitionConnectTo(this MEPCurve mepc1, MEPCurve mepc2)
        {
            if (!((mepc1 is Pipe && mepc2 is Pipe) || (mepc1 is Duct && mepc2 is Duct) ||
                  (mepc2 is CableTray && mepc1 is CableTray) || (mepc1 is Conduit && mepc2 is Conduit)))
            {
                throw new Exception("BinException ConnectTo:Is not same class");
            }

            FamilyInstance result = null;
            Document doc = mepc1.Document;

            var mepcLine1 = (mepc1.Location as LocationCurve).Curve as Line;
            var mepcLine2 = (mepc2.Location as LocationCurve).Curve as Line;

            var mepcDir = mepcLine1.Direction;
            var tarmepDir = mepcLine2.Direction;

            var end1 = mepcLine1.GetEndPoint(0);
            var end2 = mepcLine1.GetEndPoint(1);

            var end3 = mepcLine2.GetEndPoint(0);
            var end4 = mepcLine2.GetEndPoint(1);

            //记录mepc1 和 mepc2的相连的连接件
            var connectedCons = new List<Connector>();

            var connectedcons1 =
                mepc1.ConnectorManager.Connectors.PhysicalConToList().Where(m => m.IsConnected)?.Select(m=>m.GetConnectedCon()).ToList();

            var connectedcons2 =
                mepc2.ConnectorManager.Connectors.PhysicalConToList().Where(m => m.IsConnected)?.Select(m => m.GetConnectedCon()).ToList();

            if (connectedcons1.Count()>0)
            {
                connectedCons.AddRange(connectedcons1);
            }
            if (connectedcons2.Count>0)
            {
                connectedCons.AddRange(connectedcons2);
            }

            //foreach (Connector con in connectedCons)
            //{
            //    XYZ position = con.Origin;
            //    doc.NewLine_WithoutTransaction(position, position + XYZ.BasisZ * 3);
            //}
             
            if (mepcLine1.IsOnSameLine(mepcLine2))
            {
                var temlist = new List<XYZ>() { end1, end2, end3, end4 };
                //找到最外侧的两个点
                XYZ outerpoint1 = temlist.Aggregate((m, n) => end1.DistanceTo(m) > end2.DistanceTo(n) ? m : n);
                XYZ outerpoint2 = temlist.Aggregate((m, n) => outerpoint1.DistanceTo(m) > outerpoint1.DistanceTo(n) ? m : n);

             

                //取两条管线的中点
                var middle1 = (end1 + end2) / 2;
                var middle2 = (end3 + end4) / 2;

                if (DoubleExtension.IsEqual(middle1.DistanceTo(middle2), 0d))
                {
                    throw new Exception("BinException:Middles are overlapped.");
                }
                 
                //最终的中点
                var totalmiddle = (middle2 + middle1) / 2;

                //判断两条管线尺寸相同 则删除第二条 将第一条延伸至第二条管线的位置 并将连接件连接起来

                if ((mepc1 is Pipe && DoubleExtension.IsEqual(mepc1.Diameter, mepc2.Diameter)) ||
                    (mepc1 is Conduit && DoubleExtension.IsEqual(mepc1.Diameter, mepc2.Diameter)) ||
                    (mepc1 is Duct && (DoubleExtension.IsEqual(mepc1.Width, mepc2.Width) && (DoubleExtension.IsEqual(mepc1.Height, mepc2.Height)))) ||
                    (mepc1 is CableTray && (DoubleExtension.IsEqual(mepc1.Width, mepc2.Width) && (DoubleExtension.IsEqual(mepc1.Height, mepc2.Height)))))
                {


                    //找到第二条线上的较远的点
                    XYZ mepc2_farend;
                    if (end3.DistanceTo(totalmiddle) > end4.DistanceTo(totalmiddle))
                    {
                        mepc2_farend = end3;
                    }
                    else
                    {
                        mepc2_farend = end4;
                    }

                    //第一条线上的较远的点
                    XYZ mepc1_farend;
                    if (end1.DistanceTo(totalmiddle) > end2.DistanceTo(totalmiddle))
                    {
                        mepc1_farend = end1;
                    }
                    else
                    {

                        mepc1_farend = end2;
                    }

                    doc.Delete(mepc2.Id);

                    (mepc1.Location as LocationCurve).Curve = Line.CreateBound(mepc1_farend, mepc2_farend);

                    foreach (Connector con in connectedcons2)
                    {
                        var connectors = mepc1.ConnectorManager.Connectors.PhysicalConToList();
                        foreach (Connector conIn in connectors)
                        {
                            if (con.Origin.IsAlmostEqualTo(conIn.Origin))
                            {
                                con.ConnectTo(conIn);
                            }
                        }
                    }

                }
                else
                {
                     
                var conset1 = mepc1.ConnectorManager.Connectors.PhysicalConToList();
                var conset2 = mepc2.ConnectorManager.Connectors.PhysicalConToList();

                var dir1 = totalmiddle - middle1;
                var dir2 = totalmiddle - middle2;

                var connector1 = conset1.Where(m => m.CoordinateSystem.BasisZ.IsSameDirection(dir1)).First();
                var connector2 = conset2.Where(m => m.CoordinateSystem.BasisZ.IsSameDirection(dir2)).First();

                result = doc.Create.NewTransitionFitting(connector1,connector2);
                }
            }
            else
            {
                throw new Exception("BinException TransactionConnect:both curve is not on same line.");
            }
            return result;
        }

        /// <summary>
        /// 三通连接 连接最近的三个连接件
        /// </summary>
        /// <param name="mep1"></param>
        /// <param name="mep2"></param>
        /// <param name="mep3"></param>
        /// <returns></returns>
        public static FamilyInstance TeeConnectTo(this MEPCurve mep1/*branch*/, MEPCurve mep2, MEPCurve mep3)
        {
            FamilyInstance result = null;
            var doc = mep1.Document;
            var l1 = mep1.LocationLine();
            var l2 = mep2.LocationLine();
            var l3 = mep3.LocationLine();
            l1.MakeUnbound();
            l2.MakeUnbound();
            //l2 l3 共线  l1 l2 共面
            if (l2.IsOnSameLine(l3) && l1.IsOnSamePlane(l2))
            {
                var intersection = l1.Intersect_cus(l2);
                var conset1 = mep1.ConnectorManager.Connectors.PhysicalConToList();
                var conset2 = mep2.ConnectorManager.Connectors.PhysicalConToList();
                var conset3 = mep3.ConnectorManager.Connectors.PhysicalConToList();
                var tarCon1 = conset1.OrderBy(m => m.Origin.DistanceTo(intersection)).ElementAt(0);
                var tarCon2 = conset2.OrderBy(m => m.Origin.DistanceTo(intersection)).ElementAt(0);
                var tarCon3 = conset3.OrderBy(m => m.Origin.DistanceTo(intersection)).ElementAt(0);
                //doc.NewLine_WithoutTransaction(tarCon3.Origin, tarCon3.Origin + tarCon3.CoordinateSystem.BasisZ * 10);
                //doc.NewLine_WithoutTransaction(tarCon2.Origin ,tarCon2.Origin+tarCon2.CoordinateSystem.BasisZ*10);
                //doc.NewLine_WithoutTransaction(tarCon1.Origin, tarCon1.Origin + tarCon1.CoordinateSystem.BasisZ * 10);
                doc.Create.NewTeeFitting(tarCon2, tarCon3, tarCon1);
            }
            return result;
        }

        public static FamilyInstance TeeConnectTo(this MEPCurve mep1/*branch*/, MEPCurve mep2)
        {
            var doc = mep1.Document;
            FamilyInstance result = null;
            var nonOpencons1 = mep1.ConnectorManager.Connectors.PhysicalConToList().Where(m => !m.IsOpen());
            var nonOpencons2 = mep2.ConnectorManager.Connectors.PhysicalConToList().Where(m => !m.IsOpen());
            var consTobecoonnected = new List<Connector>();
            consTobecoonnected.AddRange(nonOpencons1);
            consTobecoonnected.AddRange(nonOpencons2);
            //MessageBox.Show(consTobecoonnected.Count.ToString());
            consTobecoonnected = consTobecoonnected.Select(m => m.GetConnectedCon()).ToList();
            consTobecoonnected.ForEach(m => m.DisconnectFrom(m.GetConnectedCon()));
            var line1 = mep1.LocationLine();
            var line2 = mep2.LocationLine();
            var line1copy = line1.Clone();
            var line2copy = line2.Clone();
            line1copy.MakeUnbound();
            line2copy.MakeUnbound();
            var intersection = line1copy.Intersect_cus(line2copy);
            //复制mep2 
            var newmep2 =
                ElementTransformUtils.CopyElement(doc, mep2.Id, new XYZ()).First().GetElement(doc) as MEPCurve;
            XYZ start1 = null;
            XYZ end1 = null;
            XYZ start2 = null;
            XYZ end2 = null;
            start1 = line2.StartPoint();
            end1 = intersection;
            start2 = intersection;
            end2 = line2.EndPoint();
            var newline1 = Line.CreateBound(start1, end1);
            var newline2 = Line.CreateBound(start2, end2);
            (mep2.Location as LocationCurve).Curve = newline1;
            (newmep2.Location as LocationCurve).Curve = newline2;
            mep1.TeeConnectTo(mep2, newmep2);
            //连接剩余连接件
            var connectorsofmeps = new List<Connector>();
            connectorsofmeps.AddRange(mep1.ConnectorManager.Connectors.PhysicalConToList());
            connectorsofmeps.AddRange(mep2.ConnectorManager.Connectors.PhysicalConToList());
            connectorsofmeps.AddRange(newmep2.ConnectorManager.Connectors.PhysicalConToList());
            foreach (Connector con1 in consTobecoonnected)
            {
                foreach (Connector con2 in connectorsofmeps)
                {
                    if (con1.Origin.IsAlmostEqualTo(con2.Origin))
                    {
                        con1.ConnectTo(con2);
                    }
                }
            }
            return result;
        }
    }
}
