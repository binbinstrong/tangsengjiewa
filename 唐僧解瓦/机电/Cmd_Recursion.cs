using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    class Cmd_Recursion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;

            var pipe =
                sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Pipe)).GetElement(doc) as Pipe;


            //var closedCon = pipe.ConnectorManager.Connectors.Cast<Connector>().Where(m => m.IsConnected).FirstOrDefault();

            var cons = pipe.ConnectorManager.Connectors;

            //foreach (var con in cons)
            //{
            //    var connector = con as Connector;
            //    var contype = connector.ConnectorType;

            //    //var connectedcon = connector
            //    MessageBox.Show(contype.ToString());
            //}

            var startcon = pipe.StartCon();

            var logicalcons = startcon.AllRefs.Cast<Connector>().Where(m=>m.ConnectorType == ConnectorType.Logical);

            MessageBox.Show("startcon:"+logicalcons.Count().ToString());

            var endcon = pipe.EndCon();

            var logicalcons1 = endcon.AllRefs.Cast<Connector>().Where(m => m.ConnectorType == ConnectorType.Logical);

            MessageBox.Show("endcon:" + logicalcons1.Count().ToString());




            return Result.Succeeded;
        }

        public Connector GetConnectedCon1(Connector con)
        {
            var result = default(Connector);

            var allrefs = con.AllRefs;

            foreach (Connector con1 in allrefs)
            {
                var contype = con1.ConnectorType;
                MessageBox.Show(contype.ToString());

                //bool isconnected = false;

                //var position = con.Origin;


                //MessageBox.Show(contype.ToString() + Environment.NewLine);


                //var position1 = con1.Origin;

                //isconnected = position.IsAlmostEqualTo(position1);



            }


            return result;
        }
    }
}
