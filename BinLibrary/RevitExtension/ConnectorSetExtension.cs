using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BinLibrary.RevitExtension
{
    public static class ConnectorSetExtension
    {
        /// <summary>
        /// 返回集合中的连接件（end curve 类型)
        /// </summary>
        /// <param name="conset"></param>
        /// <returns></returns>
        public static IList<Connector> PhysicalConToList(this ConnectorSet conset)
        {
            List<Connector> result = new List<Connector>();

            var enu = conset.GetEnumerator();
            enu.Reset();
            while (enu.MoveNext())
            {
                Connector con = enu.Current as Connector;
                if (con.ConnectorType==ConnectorType.End||con.ConnectorType==ConnectorType.Curve)
                {
                    result.Add(con);
                }
            }

            return result;
        }
    }
}
