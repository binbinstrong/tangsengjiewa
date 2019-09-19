using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class ColorExtension
    {
        public static Color InversColor(this Color color)
        {
            var newcolor = default(Color);

            var newR = (byte)(255 - color.Red);
            var newG = (byte)(255 - color.Green);
            var newB = (byte)(255 - color.Blue);

            newcolor = new Color(newR, newG, newB);

            return newcolor;
        }
    }
}
