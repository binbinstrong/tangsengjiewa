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
using 唐僧解瓦.机电.ToolUIs;

namespace 唐僧解瓦.机电
{
    /// <summary>
    /// 管道三通支管提升
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_RaiseTeeBranch : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            var acview = doc.ActiveView;

            ValueSettingUI settingWin = new ValueSettingUI();
            n:;
            settingWin.ShowDialog();

            var valuestring = settingWin.settingValue.Text;
            var value = default(double);

            var parseResult = double.TryParse(valuestring, out value);

            if (!parseResult)
            {
                MessageBox.Show("数值错误，请重新输入");
                settingWin.Hide();
                goto n;
            }

            while (true)
            {
                try
                {
                    var eleref = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Pipe));

                    var pipe = eleref.GetElement(doc) as Pipe;

                    var pipecons = pipe.ConnectorManager.Connectors.Cast<Connector>();

                    var validcons = pipecons.Where(m =>
                            m.IsConnected && (m.ConnectorType == ConnectorType.End || m.ConnectorType == ConnectorType.Curve))
                        .ToList();

                    if (validcons.Count < 1) return Result.Cancelled;
                    var connectedPipeFittings = validcons.Select(m => m.GetConnectedCon().Owner).Cast<FamilyInstance>().Where(m => m != null);
                    //MessageBox.Show(connectedPipeFittings.Count().ToString());

                    var teeFitting = default(IEnumerable<FamilyInstance>);
                    teeFitting = connectedPipeFittings.Where(m => m.Symbol.Family.get_Parameter(BuiltInParameter.FAMILY_CONTENT_PART_TYPE).AsValueString().Contains("三通")&&m.FacingOrientation.IsParallel(pipe.LocationLine().Direction));

                    if (teeFitting.Count() == 1)
                    {
                        var tee = teeFitting.FirstOrDefault();
                        var location = (tee.Location as LocationPoint).Point;
                        var facingdir = tee.FacingOrientation;
                        var handdir = tee.HandOrientation;
                        var anxisline = Line.CreateUnbound(location, handdir);

                        var updir = -facingdir.CrossProduct(handdir);
                        var newupdir = default(XYZ);
                        if (updir.AngleTo(XYZ.BasisZ) <= Math.PI / 2)
                        {
                            newupdir = updir;
                        }
                        else
                        {
                            newupdir = -updir;
                        }
                         
                        var consOfTee = tee.MEPModel.ConnectorManager.Connectors.Cast<Connector>();
                        var branchCon = consOfTee.Where(m => m.CoordinateSystem.BasisZ.IsSameDirection(-facingdir)).FirstOrDefault();

                        var connectedconOfBranchCon = branchCon.GetConnectedCon();

                        Transaction ts = new Transaction(doc, "提升支管高度");
                        ts.Start();

                        branchCon.DisconnectFrom(connectedconOfBranchCon);
                        //改变支管高度
                        ElementTransformUtils.MoveElement(doc, pipe.Id, newupdir * value / 304.8);
                        
                        //旋转Tee
                        ElementTransformUtils.RotateElement(doc, tee.Id, anxisline, facingdir.AngleOnPlaneTo(newupdir * (value) / Math.Abs(value), -handdir) );
                        doc.Regenerate();

                        var branchConPosition = branchCon.Origin;
                        var distance = branchConPosition.DistanceTo(pipe.LocationLine());

                        //新创建管道
                        var startpo = branchConPosition;
                        var endpo = startpo + newupdir * (value) / Math.Abs(value) * distance;

                        var newline = Line.CreateBound(startpo, endpo);

                        var newpipeid = ElementTransformUtils.CopyElement(doc, pipe.Id, new XYZ()).FirstOrDefault();

                        var newpipe = newpipeid.GetElement(doc) as Pipe;

                        (newpipe.Location as LocationCurve).Curve = newline;

                        foreach (Connector con in newpipe.ConnectorManager.Connectors)
                        {
                            var conorigin = con?.Origin;
                            if (conorigin == null) continue;
                            if (conorigin.IsAlmostEqualTo(branchConPosition))
                            {
                                con.ConnectTo(branchCon);
                            }
                        }
                        pipe.ElbowConnect(newpipe);

                        ts.Commit();

                    }
                    else if (teeFitting.Count() == 2)
                    {
                        //两端都是三通的情况 暂未处理
                    }
                }
                catch (Exception e)
                {
                    break;
                }
            }
            return Result.Succeeded;
        }
    }
}
