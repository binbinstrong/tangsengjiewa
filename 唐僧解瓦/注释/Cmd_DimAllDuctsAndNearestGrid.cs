using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.注释
{
    [Transaction(TransactionMode.Manual)]
    class Cmd_DimAllDuctsAndNearestGrid : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var acview = doc.ActiveView;

            var collecorForGrid = new FilteredElementCollector(doc);

            var collectorForDucts = new FilteredElementCollector(doc, acview.Id);

            var grids = collecorForGrid.OfClass(typeof(Grid)).Cast<Grid>();
            var ducts = collectorForDucts.OfClass(typeof(Duct)).Cast<Duct>();

            TransactionGroup tsg = new TransactionGroup(doc, "标注所有风管定位");
            tsg.Start();
            foreach (var duct in ducts)
            {
                var ductline = duct.LocationLine();

                var ductcenter = (ductline.StartPoint() + ductline.EndPoint()) / 2;
                var grid = grids.Where(m => (m.Curve as Line != null)&&((m.Curve as Line).Direction.IsParallel(ductline.Direction)))
                    .OrderBy(m => ductcenter.DistanceTo(m.Curve as Line)).FirstOrDefault();

                if (grid == null)
                {
                    //MessageBox.Show("grid is null1");
                    continue;
                }

                var gridline = grid.Curve as Line;
                if (gridline == null)
                {
                    //MessageBox.Show("grid is null2");
                    continue;
                }
                if (!ductline.Direction.IsParallel(gridline.Direction)) continue;

                var ductref = new Reference(duct);
                var gridref = new Reference(grid);


                var dir = ductline.Direction;
                var updir = XYZ.BasisZ;

                var dimDir = dir.CrossProduct(updir);

                var dimline = Line.CreateUnbound(ductcenter, dimDir);
                var refarray = new ReferenceArray();
                refarray.Append(ductref);
                refarray.Append(gridref);

                Transaction ts = new Transaction(doc, "标注所有风管定位");
                ts.Start();
                doc.Create.NewDimension(acview, dimline, refarray);
                ts.Commit();
            }

            tsg.Assimilate();



            return Result.Succeeded;
        }
    }
}
