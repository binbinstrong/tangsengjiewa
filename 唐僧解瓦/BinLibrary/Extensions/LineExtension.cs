
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 唐僧揭瓦.BinLibrary.Extensions
{
   public static class LineExtension
    {
        public static XYZ StartPoint(this Line line)
        {
            if (line.IsBound)
                return line.GetEndPoint(0);
            return null;
        }
        public static XYZ EndPoint(this Line line)
        {
            if (line.IsBound)
                return line.GetEndPoint(1);
            return null;
        }
    }
}
