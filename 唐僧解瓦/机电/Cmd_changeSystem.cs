using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System.Linq;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]

    class Cmd_changeSystem : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var dbapp = uiapp.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;


            #region deletepipes without pipingsystem

            var collector = new FilteredElementCollector(doc);
            var pipes = collector.OfClass(typeof(Pipe)).ToElements().Cast<Pipe>();

            var nullpipes = pipes.Where(m => m.MEPSystem == null).Select(m=>m.Id);

            doc.Invoke(m => { doc.Delete(nullpipes.ToList()); },"delete");
            #endregion

            //var mepsyscollector = new FilteredElementCollector(doc);
            //var mepsysTypes = mepsyscollector.OfClass(typeof(MEPSystemType)).Cast<MEPSystemType>().ToList();

            //var ldg = mepsysTypes.Where(m => m.Name == "空调循环供水").First();
            //var ldh = mepsysTypes.Where(m => m.Name == "空调循环回水").First();

            //var collector = new FilteredElementCollector(doc);
            //var pipes = collector.OfClass(typeof(Pipe)).ToElements().Cast<Pipe>();

            //Transaction trs = new Transaction(doc, "genggaixitong");
            //trs.Start();

            //int count = 0;
            ////MessageBox.Show(pipes.Count().ToString());

            ////foreach (var pipe in pipes)
            //for (int i = 0; i < pipes.Count(); i++)
            //{
            //    var pipe = pipes.ElementAt(i);

            //    //var para = pipe.LookupParameter("管段编号");
            //    //var paravalueStr = para.AsString();
            //    //if (string.IsNullOrEmpty(paravalueStr)) continue;
            //    #region MyRegion

            //    var mepsystem = pipe.MEPSystem;

            //    var cons = pipe.ConnectorManager.Connectors.Cast<Connector>().Where(m =>
            //    {
            //        return m.ConnectorType == ConnectorType.End || m.ConnectorType == ConnectorType.Curve;
            //    }).ToList();

            //    var connectedcons = cons.Select(m => m.GetConnectedCon()).Where(m => m != null).ToList();

            //    var newpipe = ElementTransformUtils.CopyElement(doc, pipe.Id, new XYZ()).First().GetElement(doc) as Pipe;

            //    var newcons = newpipe.ConnectorManager.Connectors.Cast<Connector>().Where(m =>
            //    {
            //        return m.ConnectorType == ConnectorType.End || m.ConnectorType == ConnectorType.Curve;
            //    }).ToList();

            //    foreach (var connector in newcons)
            //    {
            //        foreach (var connectedcon in connectedcons)
            //        {
            //            if (connectedcon.Origin.IsAlmostEqualTo(connector.Origin))
            //            {
            //                connector.ConnectTo(connectedcon);
            //            }
            //        }
            //    }

            //    #endregion

            //    //if (paravalueStr.Contains("LDG"))
            //    //{
            //    //    MessageBox.Show("ldh");
            //    //    pipe.SetSystemType(ldg.Id);
            //    //}
            //    //else if (paravalueStr.Contains("LDH"))
            //    //{
            //    //    pipe.SetSystemType(ldh.Id);
            //    //}
            //    //else if (paravalueStr.Contains("LQG"))
            //    //{
            //    //    pipe.SetSystemType(ldg.Id);
            //    //}
            //    //else if (paravalueStr.Contains("LQH"))
            //    //{
            //    //    pipe.SetSystemType(ldh.Id);
            //    //}

            //}
            //trs.Commit();

            return Result.Succeeded;

        }
    }
}
