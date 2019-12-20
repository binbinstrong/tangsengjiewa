using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Linq;
using System.Windows;
using Autodesk.Revit.DB.Plumbing;
using 唐僧解瓦.BinLibrary.Extensions;


namespace 唐僧解瓦.机电
{
    /// <summary>
    /// 设备连接 (消火栓)
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    class Cmd_ConnectEquipMent : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            View acview = uidoc.ActiveView;
            //UIView acuivew = uidoc.Activeuiview();

            var familyInsRef = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is FamilyInstance));
            var pipeRef = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Pipe));

            var pipe = pipeRef.GetElement(doc) as Pipe;


            var familyIns = familyInsRef.GetElement(doc) as FamilyInstance;

            var equipmentCons = familyIns.MEPModel.ConnectorManager.Connectors;

            var firstEquipCon = equipmentCons.Cast<Connector>().First(m =>
            {
                return m.ConnectorType == ConnectorType.Curve || m.ConnectorType == ConnectorType.End;

            });

            var conRadius = firstEquipCon.Radius;
            var condia = conRadius * 2;
            var origin = firstEquipCon.Origin;
            var conDir = firstEquipCon.CoordinateSystem.BasisZ;
             
            var pipelocationline = pipe.LocationLine();
             
            Transaction ts = new Transaction(doc, "设备连接");
            try
            {

                ts.Start();
                //由连接件 水平生成一段管 
                var firstlineEnd1 = origin;
                var firstlineEnd2 = origin + 200d.MetricToFeet() * conDir;
                var firsline = Line.CreateBound(firstlineEnd1, firstlineEnd2);
                var firstPipe = Pipe.Create(doc, pipe.MEPSystem.GetTypeId(), pipe.GetTypeId(), pipe.ReferenceLevel.Id, firstlineEnd1, firstlineEnd2);

                

                firstPipe.LookupParameter("直径").Set(conRadius * 2);
                 

                //生成垂直管道
                var secondlineEnd1 = firstlineEnd2;
                var secondlineEnd2 = secondlineEnd1 + XYZ.BasisZ * (pipelocationline.StartPoint().Z - secondlineEnd1.Z);

                var secondPipe = Pipe.Create(doc, pipe.MEPSystem.GetTypeId(), pipe.GetTypeId(), pipe.ReferenceLevel.Id, secondlineEnd1, secondlineEnd2);

                 

                secondPipe.LookupParameter("直径").Set(conRadius * 2);
                //生成第三根管道

                var thirdlineEnd1 = secondlineEnd2;
                var thirdlineEnd2 = thirdlineEnd1.ProjectToXLine(pipelocationline);

                var thirdPipe = Pipe.Create(doc, pipe.MEPSystem.GetTypeId(), pipe.GetTypeId(), pipe.ReferenceLevel.Id, thirdlineEnd1, thirdlineEnd2);
 

                thirdPipe.LookupParameter("直径").Set(conRadius * 2);

                //链接所有管道

                var firstpipeCons = firstPipe.ConnectorManager.Connectors;
                foreach (Connector item in firstpipeCons)
                {
                    if (item.ConnectorType == ConnectorType.End || ConnectorType.Curve == item.ConnectorType)
                        if (firstEquipCon.Origin.IsAlmostEqualTo(item.Origin))
                        {
                            item.ConnectTo(firstEquipCon);
                        }
                }

                firstPipe.ElbowConnect(secondPipe);
                secondPipe.ElbowConnect(thirdPipe);

                //链接剩余管道
                //Do it yourself!!
                //…………

                ts.Commit();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                if (ts.GetStatus() == TransactionStatus.Started)
                {
                    ts.RollBack();
                }
                //throw;
            }
            return Result.Succeeded;
        }
    }
}
