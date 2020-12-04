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
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.机电
{
    /// <summary>
    /// 旋转指定风口
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_MultiRotateDuctEnds : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;


            //1.find all ductEnds which needs ratate

            var collector = new FilteredElementCollector(doc);
            var familyinstances = collector.OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType().Cast<FamilyInstance>();

            var ductEnds = familyinstances.Where(m => m.Symbol.FamilyName.Contains("单层百叶风口"));


            MessageBox.Show(ductEnds.Count().ToString());

            Transaction ts = new Transaction(doc, "旋转风口");
            ts.Start();


            foreach (FamilyInstance fi in ductEnds)
            {
                var rotateAnxis = Line.CreateUnbound((fi.Location as LocationPoint).Point, XYZ.BasisZ);
                var facingOrientation = fi.FacingOrientation;

                var mepmodel = fi.MEPModel;
                if (mepmodel == null)
                {
                    MessageBox.Show("mepmodel is null");

                    continue;
                }

                var conman = mepmodel.ConnectorManager;
                var connectors = conman.Connectors;
                //MessageBox.Show(connectors.Size.ToString());

                var firstcon = connectors.Cast<Connector>().Where(m => m.ConnectorType == ConnectorType.End).First();
                if (firstcon == null)
                {
                    MessageBox.Show("firstcon is null");

                }

                var connectedcon = firstcon.GetConnectedCon();
                //var allrefs = firstcon.AllRefs;

                //foreach (Connector con in allrefs)
                //{
                //    MessageBox.Show(con.ConnectorType.ToString());

                //    if (con.ConnectorType == ConnectorType.Curve)
                //        MessageBox.Show("conMessage:" + con.Origin.ToString() + Environment.NewLine +
                //                        "firstconMsg:" + firstcon.Origin.ToString());
                //}

                //MessageBox.Show("allrefs::" + allrefs.Size.ToString());


                var ownerDuct = firstcon.GetConnectedCon().Owner as Duct;
                //if (ownerDuct == null)
                //{
                //    MessageBox.Show("Owner duct is null");

                //    continue;
                //}

                var ownerDuctDir = ownerDuct.LocationLine().Direction;
                if (!ownerDuctDir.Z.IsEqual(0)) continue;
                LogHelper.LogWrite(facingOrientation.ToString(), @"c:\abc.txt",true);
                LogHelper.LogWrite(ownerDuctDir.ToString(), @"c:\abc.txt",true);
                LogHelper.LogWrite("\n", @"c:\abc.txt",true);
                if (!facingOrientation.IsParallel(ownerDuctDir))
                {
                    //LogHelper.LogWrite(facingOrientation.ToString(), @"c:\abc.txt",true);
                    ElementTransformUtils.RotateElement(doc, fi.Id, rotateAnxis, Math.PI / 2);
                }

            }

            ts.Commit();



            return Result.Succeeded;
        }
    }
}
