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

        public static Color ToRvtColor(this System.Drawing.Color color)
        {
            var r = color.R;
            var g = color.G;
            var b = color.B;

            return new Color(r,g,b);
        }
    }
}
