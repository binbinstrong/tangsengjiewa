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
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 净高分析
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_netHeightAnalyses:IExternalCommand
    {   
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        { 
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            var collector = new FilteredElementCollector(doc);
            //var rooms = collector.OfClass(typeof(Room));
             
            //var phases = doc.Phases;
            //foreach (Phase phase in phases)
            //{
            //    MessageBox.Show(phase.Name.ToString());
            //}

            if (!(doc.ActiveView is ViewPlan))
            {
                MessageBox.Show("请转到平面视图");
                return Result.Cancelled;
            }
            //创建房间（根据墙围成的闭合图形生成房间）
            doc.Invoke(m => { Createrooms(doc, doc.ActiveView.GenLevel, doc.Phases.get_Item(1)); }, "当前视图楼层创建房间");
            //下一步用创建的房间进行标高分析

            

            return Result.Succeeded;
        }

        /// <summary>
        /// 指定楼层根据topology 创建房间
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="level"></param>
        /// <param name="curPhase"></param>
        public void Createrooms(Document doc, Level level, Phase curPhase)
        {
            //var pologies = doc.PlanTopologies;
            var pology = doc.get_PlanTopology(level);
            var circuits = pology.Circuits;
            //MessageBox.Show(circuits.Size.ToString());
            //新相位
            var newphase = doc.Phases.Cast<Phase>().Where(m => m.Name.Contains("新建")).First();
            if (newphase == null) return;
            foreach (PlanCircuit circuit in circuits)
            {
                var sheduleroom = doc.Create.NewRoom(curPhase);
                doc.Create.NewRoom(sheduleroom, circuit);
            }
        }
    }
}
