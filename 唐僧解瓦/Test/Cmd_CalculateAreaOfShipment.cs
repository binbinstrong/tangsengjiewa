using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.Test
{
    /// <summary>
    /// 计算墙的两面 面积
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_CalculateAreaOfShipment : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;

            var wall = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Wall)).GetElement(doc) as Wall;

            var facesoutRef= HostObjectUtils.GetSideFaces(wall,ShellLayerType.Exterior);

            var facesinRef = HostObjectUtils.GetSideFaces(wall, ShellLayerType.Interior);

            var faceout = wall.GetGeometryObjectFromReference(facesoutRef.First()) as Face;
            var facein = wall.GetGeometryObjectFromReference(facesinRef.First()) as Face;

            var area = default(double);
            area += faceout.Area;
            area += facein.Area;
            area = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);

            area = Math.Round(area, 3);

            MessageBox.Show(area.ToString()+"m^2");
            
            return Result.Succeeded;
        }
    }
}

 