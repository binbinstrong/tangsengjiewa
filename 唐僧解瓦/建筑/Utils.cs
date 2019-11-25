using Autodesk.Revit.DB;
using System;

namespace 唐僧解瓦.建筑
{
    class Utils
    {
        public static bool CutBeam(FamilyInstance beam, XYZ cutpoint)
        {
            var result = false;
            if (beam.Category.Id.IntegerValue != (int)BuiltInCategory.OST_StructuralFraming)
            {
                throw new Exception("Element being cut is not Beam!");
            }

            var locationline = beam.Location as LocationCurve;
            
            

            return result;
        }
    }
}
