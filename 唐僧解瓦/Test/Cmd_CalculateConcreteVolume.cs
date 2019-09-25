using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.Test
{
    /// <summary>
    /// 计算集合元素体积
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_CalculateConcreteVolume : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;

            var ele = sel.PickObject(ObjectType.Element).GetElement(doc);

            var options = new Options();
            options.DetailLevel = ViewDetailLevel.Fine;

            var geometryelement = ele.get_Geometry(options);

            var volume = getVolumes(geometryelement);
            var volumestring = Math.Round(volume, 3).ToString();

            MessageBox.Show(volumestring+"m^3");

            return Result.Succeeded;
        }

        public double getVolumes(GeometryElement geoEle)
        {
            double result = default(double);

            var geoenu = geoEle.GetEnumerator();
            while (geoenu.MoveNext())
            {
                var currentgeo = geoenu.Current;
                if (currentgeo is Solid solid)
                {
                    result += solid.Volume;
                    //MessageBox.Show(result.ToString());
                }
                else if (currentgeo is GeometryInstance geoins)
                {
                    var temgeoele = geoins.SymbolGeometry;
                    var geoenu1 = temgeoele.GetEnumerator();
                    while (geoenu1.MoveNext())
                    {
                        //MessageBox.Show("instance is not null");
                        var currentgeo1 = geoenu1.Current;
                        if (currentgeo1 is Solid solid1)
                        {
                            result += solid1.Volume;
                        }
                    }
                }
            }
            //单位转换 立方英尺 转 立方米
            result = UnitUtils.ConvertFromInternalUnits( result, DisplayUnitType.DUT_CUBIC_METERS);
            return result;
        }
    }
}
