using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
//using System.Windows.Media.Animation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BinLibrary.RevitExtension;

namespace BinLibrary.RevitHelper
{
    public static class ConnectorHelper
    {
        /// <summary>
        /// 获取相连的物理连接件
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static Connector GetConnectedCon(this Connector con)
        {
            Connector result = null;
            if (con.IsConnected)
            {
                try
                {
                    var refcons = con.AllRefs.PhysicalConToList();
                    {
                        //MessageBox.Show(refcons.Count.ToString());
                        //foreach (var connector in refcons)
                        //MessageBox.Show(con.Owner.Id.IntegerValue.ToString() + ":" + Environment.NewLine +
                        //                refcons.Count.ToString() + Environment.NewLine +
                        //                //refcons.First().GetHashCode()+Environment.NewLine + 
                        //                //refcons.Last().GetHashCode()+Environment.NewLine+
                        //                con.Id.ToString()+Environment.NewLine + 
                        //                refcons.First().Id.ToString()+Environment.NewLine +
                        //                refcons.Last().Id.ToString()+Environment.NewLine +
                        //                con.Owner.Id.IntegerValue+Environment.NewLine+
                        //                refcons.First().Owner.Id.IntegerValue + Environment.NewLine +
                        //                refcons.Last().Owner.Id.IntegerValue + Environment.NewLine +
                        //                con.CoordinateSystem.BasisZ.ToString() + Environment.NewLine+
                        //                refcons.First().CoordinateSystem.BasisZ.ToString() + con.IsConnectedTo(refcons.First()).ToString() + Environment.NewLine +
                        //                refcons.Last().CoordinateSystem.BasisZ.ToString() + con.IsConnectedTo(refcons.Last()).ToString());
                    }
                    result = refcons.ToList().Find(m =>
                        m.IsConnectedTo(con) && m.Owner.Id.IntegerValue != con.Owner.Id.IntegerValue &&
                        (m.ConnectorType == ConnectorType.Curve || m.ConnectorType == ConnectorType.End));
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return result;
        }

        public static List<Connector> GetBackCons(this Connector con)
        {
            var allrefs = con.AllRefs;
            return allrefs.PhysicalConToList().Where(m => m.Owner.Id.IntegerValue == con.Owner.Id.IntegerValue).ToList();
        }

        [Obsolete]
        /// <summary>
        /// 获取连接件背部连接件
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static Connector GetStraightBackcon(this Connector con)
        {
            Connector result = null;
            var allrefs = con.AllRefs;
            var conline = Line.CreateUnbound(con.Origin, con.CoordinateSystem.BasisZ);

            foreach (Connector item in allrefs)
            {
                if (con.Owner.Id.IntegerValue == item.Owner.Id.IntegerValue) //先判断连接件附着主体相同
                    if (item.ConnectorType == ConnectorType.Curve || item.ConnectorType == ConnectorType.End)
                    {
                        if (con.CoordinateSystem.BasisZ.IsOppositeDirection(item.CoordinateSystem.BasisZ) &&
                            item.Origin.IsXOnLine(conline))
                            result = item;
                    }
            }
            return result;
        }

        public static Connector GetBackConInStraight(this Connector con)
        {
            //var fi = con.Owner as FamilyInstance;
            //if (fi == null) return null;

            //var mepmoel = fi.MEPModel;
            //var cons = mepmoel.ConnectorManager.Connectors;
            Connector result = null;
            var conOrigin = con.Origin;
            var conDir = con.CoordinateSystem.BasisZ;
            var conLine = Line.CreateUnbound(conOrigin, conDir);

            var cons = con.AllRefs;
            var enu = cons.GetEnumerator();
            enu.Reset();

            while (enu.MoveNext())
            {
                var curCon = enu.Current as Connector;
                var curconType = curCon.ConnectorType;
                if (curconType != ConnectorType.Curve && curconType != ConnectorType.End) continue;
                if (con.Owner.Id.IntegerValue == curCon.Owner.Id.IntegerValue &&
                    curCon.Origin.IsXOnLine(conLine) &&
                    curCon.CoordinateSystem.BasisZ.IsOppositeDirection(conDir)
                )
                {
                    result = curCon;
                    break;
                }
            }
            return result;
        }


        /// <summary>
        /// 根据连接件连接情况来遍历获取所有连接的元素
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="refIdList"></param>
        public static void Recursion(Element elem, ref List<ElementId> refIdList)
        {
            ConnectorManager conman = null;
            refIdList.Add(elem.Id);
            if (elem is FamilyInstance)
            {
                conman = (elem as FamilyInstance).MEPModel.ConnectorManager;
            }
            else if (elem is MEPCurve)
            {
                conman = (elem as MEPCurve).ConnectorManager;
            }
            if (null == conman)
            {
                return;
            }
            IEnumerator enu = conman.Connectors.GetEnumerator();

            int i = 0;
            enu.Reset();
            while (enu.MoveNext())
            {
                Connector con = enu.Current as Connector;
                if (!con.IsConnected)
                {
                    continue;
                }
                if (con.ConnectorType == ConnectorType.End || con.ConnectorType == ConnectorType.Curve)
                {
                    Connector connectedCon = con.GetConnectedCon();
                    if (connectedCon == null)
                    {
                        continue;
                    }
                    Element owner = null;
                    if (connectedCon.ConnectorType == ConnectorType.End ||
                        connectedCon.ConnectorType == ConnectorType.Curve)
                    {
                        owner = connectedCon.Owner;
                    }
                    else
                        continue;
                    if (!refIdList.Contains(owner.Id))
                    {
                        Recursion(owner, ref refIdList);
                    }
                }
            }
            //return;
        }

        public static void Recursion(Connector connector, ref List<ElementId> refIdList)
        {
            var element = connector.GetConnectedCon().Owner;
            List<ElementId> _refIdList = new List<ElementId>();
            _refIdList.Add(connector.Owner.Id);
            Recursion(element, ref _refIdList);
        }
        //判断连接件主体类型

    }

    public enum ConnectorDomain
    {
        Duct,
        Pipe,
        CableTray,
        Conduit,

        EquipMent,

        DuctAccessary,
        PipeAccessary,

        DuctFitting,
        PipeFitting,

        CableTrayFing,
        ConduitFitting,

    }
}
