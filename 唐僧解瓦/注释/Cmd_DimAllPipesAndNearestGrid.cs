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
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.注释
{
    /// <summary>
    /// 标注成排管道 
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_DimAllPipesAndNearestGrid : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var acview = doc.ActiveView;

            var sel = uidoc.Selection;

            var collecorForGrid = new FilteredElementCollector(doc);

            var collectorForPipe = new FilteredElementCollector(doc, acview.Id);

            var pipes = collectorForPipe.OfClass(typeof(Pipe)).ToElementIds().ToList();

            //Delegate dele(Document doc, ElementId id, Line line)
            //{
            //    (id.GetElement(doc) as Pipe).LocationLine();}
            //;

            Func<Element, Line> @delegate = (Element ele) => { return (ele as Pipe).LocationLine(); };
            Func<Element, Line> @delegate1 = delegate(Element ele) { return (ele as Pipe).LocationLine();};


            //1.将管道按照方向分组
            var paralleledpipes = pipes.Select(m => m.GetElement(doc)).GroupBy(@delegate);
             
            var groups = GroupPipes(pipes, doc, 300, 300);
            MessageBox.Show(groups.Count.ToString());

            foreach (var group in groups)
            {
                sel.SetElementIds(group);
                if (group.Count > 3)
                {
                    uidoc.ShowElements(group);

                    var ids = "count" + string.Join("::", group.Select(m => m.IntegerValue)) + "\n";

                    LogHelper.LogWrite(ids, @"c:\tongji.txt", true);
                }
            }



            return Result.Succeeded;
        }

        public List<List<ElementId>> GroupPipes(List<ElementId> pipeids, Document doc, double horDis/*管线间水平间距*/, double tolerance/*共线端点间距容差 feet*/)
        {
            var result = new List<List<ElementId>>();

            var pipeRecord = new List<ElementId>();
            var count = 0;


            foreach (var pipeid in pipeids)
            {
                //MessageBox.Show("section1");
                if (pipeRecord.Contains(pipeid)) continue;

                var pipe = pipeid.GetElement(doc) as Pipe;
                var line = pipe.LocationLine();
                var startpo = line.StartPoint();
                var endpo = line.EndPoint();

                if (result.Count == 0)
                {
                    result.Add(new List<ElementId>());
                    result.ElementAt(0).Add(pipeid);
                    pipeRecord.Add(pipeid);
                }

                bool contains = false;

                var count1 = 0;
                foreach (List<ElementId> pipegroup in result)
                {
                    if (pipeRecord.Contains(pipeid)) break;

                    var ids = "<<" + pipegroup.Count + ">>" + "\n" + string.Join("::", pipegroup);

                    if (result.Count > 500)
                    {
                        //LogHelper.LogWrite(ids, @"c:\groupids.txt", true);
                        //LogHelper.LogWrite(result.Count.ToString() + "\n", "c:\\resultcount.txt", true);
                    }

                    contains = false;

                    var pipegroupcount = pipegroup.Count;
                    var count3 = 0;

                    //if(result.Count>500){
                    //LogHelper.LogWrite(pipegroupcount+"\n",@"c:\pipecouont.txt",true);}

                    for (int i = 0; i < pipegroupcount; i++)
                    {
                        count++;
                        var pipe1 = pipegroup.ElementAt(i).GetElement(doc) as Pipe;
                        var line1 = pipe1.LocationLine();
                        if (!line.Direction.IsParallel(line1.Direction)) continue;

                        var startpo1 = line1.StartPoint();
                        var endpo1 = line1.EndPoint();


                        //1.共线管线添加到组里面
                        var flag1 = false; //距离标志
                        if (startpo.DistanceTo(startpo1) < tolerance / 304.8 ||
                            startpo.DistanceTo(endpo1) < tolerance / 304.8 ||
                            endpo.DistanceTo(startpo1) < tolerance / 304.8 ||
                            endpo.DistanceTo(endpo1) < tolerance / 304.8
                        ) flag1 = true;

                        var flag2 = false; //共线标志
                        if (line.Direction.IsXOnLine(line1))
                        {
                            flag2 = true;
                        }

                        // 2.平行水平间距小于一定的距离HorDis 也添加到组里面
                        var flag3 = false;
                        if (line.Direction.IsParallel(line1.Direction))
                        {
                            flag3 = true;
                        }

                        var flag4 = false;
                        var pipeHorDis = GetHorizontalDis(line, line1).Value;

                        if (pipeHorDis < horDis / 304.8) //水平间距小于一定距离
                        {
                            flag4 = true;
                        }

                        if (flag1 && flag2)
                        {
                            pipegroup.Add(pipeid);
                            pipeRecord.Add(pipeid);
                            contains = true;
                            break;
                        }
                        else if (flag3 && flag4) //平行 且 间距小于一定值则添加到组里面
                        {
                            pipegroup.Add(pipeid);
                            pipeRecord.Add(pipeid);
                            contains = true;
                            break;
                        }
                    }
                }

                if (!contains)
                {
                    result.Add(new List<ElementId>());
                    result.Last().Add(pipeid);
                }

            }

            LogHelper.LogWrite(count.ToString(), @"c:\totalcount.txt");

            return result;
        }

        public double? GetHorizontalDis(Line line1, Line line2)
        {
            if (!line1.Direction.IsParallel(line2.Direction))
            {
                return null;
            }

            var result = default(double);

            var origin = line1.Origin;

            var originProjOnLine2 = origin.ProjectToXLine(line2);


            var neworigin = new XYZ(origin.X, origin.Y, 0);
            var newprojOrigin = new XYZ(originProjOnLine2.X, originProjOnLine2.Y, 0);

            result = neworigin.DistanceTo(newprojOrigin);

            return result;
        }

    }
}
