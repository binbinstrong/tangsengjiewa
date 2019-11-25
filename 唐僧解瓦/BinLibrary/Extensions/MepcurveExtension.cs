using Autodesk.Revit.DB;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class MepcurveExtension
    {
        public static Line LocationLine(this MEPCurve mep)
        {
            Line result = null;

            result = (mep.Location as LocationCurve).Curve as Line;

            return result;
        }
    }
}
