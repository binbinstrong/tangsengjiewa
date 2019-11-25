using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
//using BinLibrary;
//using BinLibrary.Extensions;
//using BinLibrary.RevitExtension;
//using BinLibrary.RevitHelper;
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.BinLibrary.RevitHelper;
using 唐僧解瓦.机电.ToolUIs;
namespace 唐僧解瓦.机电
{
    /// <summary>
    /// 管线排列
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    class Cmd_MepCurveRank : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            View acview = uidoc.ActiveView;

            var toolwin = MepcurveRank.Instance;
            toolwin.helper().Owner = RevitWindowHelper.GetRevitHandle();
            toolwin.Show(); //显示窗体 当点击窗体的 一键分布的时候 向revit窗体发送esc按键 结束 while循环
            var elelist = new List<ElementId>();
            while (true)
            {
                try
                {
                    var eleref = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is MEPCurve));
                    elelist.Add(eleref.ElementId);
                }
                catch (Exception e)
                {
                    break;
                }
            }
            var dis = default(double);
            var parseresult = double.TryParse(toolwin.horizontalDis.Text, out dis);
            if (!parseresult)
            {
                MessageBox.Show("间距设置错误");
                return Result.Cancelled;
            }
            //以第一根管不动 其他管 紧跟第一根管排列
            var meplist = elelist.Select(m => m.GetElement(doc) as MEPCurve).ToList();
            var stablemep = meplist.First();
            Transaction ts = new Transaction(doc, "***********");
            try
            {
                ts.Start();
                for (int i = 0; i < meplist.Count; i++)
                {
                    if (i == 0) continue;
                    var temdis = dis.MetricToFeet() * i;
                    var originaldir = -getprojectdir(meplist[i], stablemep);
                    var originaldis = getHorizontalDis(meplist[i], stablemep);
                    var moveVec = (temdis - originaldis) * originaldir;
                    ElementTransformUtils.MoveElement(doc, meplist[i].Id, moveVec);
                }
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
        /// <summary>
        /// 获取管线间的方向 由mep 指向 mep2  两条管线须平行
        /// </summary>
        /// <param name="mep"></param>
        /// <param name="mep2"></param>
        /// <returns></returns>
        XYZ getprojectdir(MEPCurve mep, MEPCurve mep2)
        {
            var line1 = mep.LocationLine();
            var line2 = mep2.LocationLine();
            var origin = line1.StartPoint();
            var originprojectonline2 = origin.ProjectToXLine(line2);
            var dir = originprojectonline2 - origin;
            var horizontaldir = dir.xyComponent().Normalize();
            return horizontaldir;
        }
        double getHorizontalDis(MEPCurve mep, MEPCurve mep2)
        {
            var line1 = mep.LocationLine();
            var line2 = mep2.LocationLine();
            var origin = line1.Origin;
            var originprojectonline2 = origin.ProjectToXLine(line2);
            var dir = originprojectonline2 - origin;
            var horizontaldir = dir.xyComponent();
            return horizontaldir.GetLength();
        }
    }
}
