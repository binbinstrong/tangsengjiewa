using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using BinLibrary.RevitHelper;

namespace BinLibrary.RevitExtension
{
    public static class ConnectorExtension
    {
        public static bool IsOpen(this Connector connector)
        {
            var connecttedCon = connector.GetConnectedCon();
            if (connecttedCon == null)
                return true;
            return false;
        }
    }
}
