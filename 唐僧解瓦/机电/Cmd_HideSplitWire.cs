using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.BinLibrary.Geometryalgorithm;
using 唐僧解瓦.BinLibrary.Helpers;
using Point = System.Windows.Point;

namespace 唐僧解瓦.机电
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    class Cmd_HideSplitWire : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            var acview = uidoc.ActiveView;

            if (!(acview is ViewPlan))
            {
                MessageBox.Show("请到平面视图执行此命令");
                return Result.Cancelled;
            }

            Category equipmentCategory = Category.GetCategory(doc, BuiltInCategory.OST_FireAlarmDevices);

            //电气设备集合
            ElementCategoryFilter catefilter1 = new ElementCategoryFilter(BuiltInCategory.OST_FireAlarmDevices);
            ElementCategoryFilter catefilter2 = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalFixtures);
            ElementCategoryFilter catefilter3 = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalEquipment);

            var filterlist = new List<ElementFilter>() {catefilter3, catefilter2, catefilter1};
            LogicalOrFilter ElectricaldeviceOrfixturesFilter = new LogicalOrFilter(filterlist);

            var elecEquipCollector = new FilteredElementCollector(doc, acview.Id).WherePasses(ElectricaldeviceOrfixturesFilter).WhereElementIsNotElementType().Cast<FamilyInstance>().ToList();
            var fsInfoCol = new List<FsInfo>();
            //LogHelper.LogWrite();
            //MessageBox.Show("sec1:" + elecEquipCollector.Count);

            var namelist = elecEquipCollector.Select(m => m.Symbol.FamilyName);
            var namestring = string.Join("\n", namelist);
            //MessageBox.Show(namestring);

            foreach (FamilyInstance fs in elecEquipCollector)
            {
                var fsinfo = new FsInfo(fs);
                fsInfoCol.Add(fsinfo);
            }


            //电线折线集合
            var wirecollector = new FilteredElementCollector(doc, acview.Id).OfClass(typeof(Wire)).Cast<Wire>().ToList();
            var wirelineCol = new List<Line>();
            foreach (Wire wire in wirecollector)
            {
                var lines = getwirelines(wire);
                wirelineCol.AddRange(lines);
            }

            //MessageBox.Show("sec2:" + wirecollector.Count);

            var wiretypes = doc.TCollector<WireType>();
            string wiretypename = "DLcoverwiretype";
            var coverwiretype = default(WireType);


            bool targetWireTypeexist = false;
            //var wiretypenames = wiretypes.Select(m => m.Name);
            coverwiretype = wiretypes.Where(m => m.Name == wiretypename)?.FirstOrDefault();

            if (coverwiretype != null)
            {
                targetWireTypeexist = true;
            }
            var firswiretype = wiretypes.First();

            TransactionGroup tsg = new TransactionGroup(doc, "创建电缆遮挡");
            tsg.Start();

            Transaction ts1 = new Transaction(doc, "创建电缆类型");
            ts1.Start();
            if (!targetWireTypeexist)
                coverwiretype = firswiretype.Duplicate(wiretypename) as WireType;
            ts1.Commit();

            Transaction ts2 = new Transaction(doc, "覆盖");
            ts2.Start();

            //辅助计数
            var count = default(long);

            foreach (var fsinfo in fsInfoCol)
            {
                foreach (Line line in wirelineCol)
                {
                    #region fortest
                    //var temins = (fsinfo.Id.GetElement(doc) as FamilyInstance);
                    //if (temins.Symbol.FamilyName.Contains("单相空调插座"))
                    //{
                    //    MessageBox.Show(fsinfo.Outline1.Count.ToString());

                    //    drawoutline(fsinfo.Outline1, doc);
                    //    //MessageBox.Show(fsinfo.Width.ToString() + Environment.NewLine + fsinfo.Height.ToString());
                    //}
                    #endregion


                    //添加遮挡导线
                    if (lineIntersecOutline(line, fsinfo.Outline1))
                    {
                        CreateHideWire(doc, fsinfo, line, coverwiretype, WiringType.Chamfer, acview);

                        //绘制外轮廓
                        //drawoutline(fsinfo.Outline1, doc);
                    }
                }
            }

            //MessageBox.Show("inbox count:" + count.ToString());

            ts2.Commit();
            tsg.Assimilate();

            return Result.Succeeded;
        }

        private List<Line> getwirelines(Wire wire)
        {
            var result = new List<Line>();
            var vertexNums = wire.NumberOfVertices;

            for (int i = 0; i < vertexNums - 1; i++)
            {
                var line = Line.CreateBound(wire.GetVertex(i), wire.GetVertex(i + 1));
                result.Add(line);
            }

            return result;
        }
        bool lineIntersecOutline(Line line, List<XYZ> outline)
        {
            var points = outline.Select(m => m.ProjectToXLine(line));
            points = points.Where(m => m.IsOnLine(line));
            foreach (var point in points)
            {
                var newpoint = new XYZ(point.X, point.Y, outline.First().Z);
                var inpolygon = PolygonHelper.IsPointInRegion(newpoint, outline, XYZ.BasisZ);
                if (inpolygon)
                {
                    return true;
                }
            }

            return false;
        }
        public void CreateHideWire(XYZ po, Line wireline, WireType type, WiringType wiringtype, View view, double halfdis/*mm*/)
        {
            var doc = type.Document;

            var pointonLine = po.ProjectToXLine(wireline);

            var dir = wireline.Direction.CrossProduct(XYZ.BasisZ).Normalize();//(pointonLine - po).Normalize();

            var point1 = pointonLine + dir * halfdis / 304.8;
            var point2 = pointonLine - dir * halfdis / 304.8;

            //MessageBox.Show(dir.ToString() + Environment.NewLine +
            //    point1.ToString() + Environment.NewLine +
            //                point2.ToString());

            var polist = new List<XYZ>();

            polist.Add(point1);
            polist.Add(point2);

            Wire.Create(doc, type.Id, view.Id, wiringtype, polist, null, null);
        }

        public void CreateHideWire(Document doc, FsInfo insinfo, Line wireLine, WireType type, WiringType wiringtype, View view)
        {
            //var doc = ins.Document;
            var ins = insinfo.Id.GetElement(doc) as FamilyInstance;
            var locationpo = (ins.Location as LocationPoint).Point;

            var linedir = wireLine.Direction;

            if (insinfo.LocationInCenter)// IsLocationInCenter(ins)
            {
                var facingDir = ins.FacingOrientation;
                var handDir = ins.HandOrientation;

                var viewscale = view.Scale;
                var lengthRatio = viewscale / 50.0;
                var width = lengthRatio * insinfo.Width;//getWidth(ins);
                var height = lengthRatio * insinfo.Height;//getHeight(ins);

                var po1 = default(XYZ);// locationpo + height / 4 * handDir;
                var po2 = default(XYZ);//locationpo - height / 4 * handDir;

                if (linedir.AngleTo(handDir) < Math.PI / 4)
                {
                    po1 = locationpo + width / 4 * handDir;
                    po2 = locationpo - width / 4 * handDir;
                }
                else
                {
                    po1 = locationpo + height / 4 * facingDir;
                    po2 = locationpo - height / 4 * facingDir;
                }

                var polist = new List<XYZ>();
                polist.Add(po1);
                polist.Add(locationpo);
                polist.Add(po2);

                //Wire.Create(doc, type.Id, view.Id, wiringtype,, null, null);

                foreach (var po in polist)
                {
                    CreateHideWire(po, wireLine, type, wiringtype, view, 3);
                }
            }

            if (insinfo.LocationNearBottom)//IslocationNearBottom(ins))
            {
                var facingDir = ins.FacingOrientation;
                var handDir = ins.HandOrientation;

                var viewscale = view.Scale;
                var lengthRatio = viewscale / 50.0;
                var width = lengthRatio * insinfo.Width;// getWidth(ins);
                var height = lengthRatio * insinfo.Height;// getHeight(ins);

                var po1 = default(XYZ);// locationpo + height / 4 * handDir;
                var po2 = default(XYZ);//locationpo - height / 4 * handDir;

                if (linedir.AngleTo(handDir) < Math.PI / 4)
                {
                    po1 = locationpo + width / 4.0 * handDir;
                    po2 = locationpo - width / 4.0 * handDir;
                }
                else
                {

                    po1 = locationpo + height / 4 * facingDir;
                    po2 = po1 + height / 4 * facingDir;
                }

                var polist = new List<XYZ>();
                polist.Add(po1);
                polist.Add(locationpo);
                polist.Add(po2);

                //Wire.Create(doc, type.Id, view.Id, wiringtype,, null, null);

                foreach (var po in polist)
                {
                    CreateHideWire(po, wireLine, type, wiringtype, view, 3);
                }

            }
            else
            {
                var facingDir = ins.FacingOrientation;
                var handDir = ins.HandOrientation;

                var viewscale = view.Scale;
                var lengthRatio = viewscale / 50.0;
                var width = lengthRatio * insinfo.Width;// getWidth(ins);
                var height = lengthRatio * insinfo.Height;// getHeight(ins);

                var po1 = default(XYZ);// locationpo + height / 4 * handDir;
                var po2 = default(XYZ);//locationpo - height / 4 * handDir;

                if (linedir.AngleTo(handDir) < Math.PI / 4)
                {
                    po1 = locationpo + width / 4 * handDir;
                    po2 = locationpo - width / 4 * handDir;
                }
                else
                {
                    po1 = locationpo + height / 4 * facingDir;
                    po2 = locationpo - height / 4 * facingDir;
                }

                var polist = new List<XYZ>();
                polist.Add(po1);
                polist.Add(locationpo);
                polist.Add(po2);

                //Wire.Create(doc, type.Id, view.Id, wiringtype,, null, null);


                foreach (var po in polist)
                {
                    CreateHideWire(po, wireLine, type, wiringtype, view, 3);
                }

            }
        }

        void drawoutline(List<XYZ> outline, Document doc)
        {
            for (int i = 0; i <= outline.Count - 1; i++)
            {
                var line = default(Line);
                if (i < outline.Count - 1)
                {
                    var end1 = outline.ElementAt(i);
                    var end2 = outline.ElementAt(i + 1);
                    line = Line.CreateBound(end1, end2);
                }
                else if (i == outline.Count - 1)
                {
                    var end1 = outline.ElementAt(i);
                    var end2 = outline.ElementAt(0);
                    line = Line.CreateBound(end1, end2);
                }


                doc.NewLine_withoutTransaction(line);
            }

        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Cmd_HideSplitWiretest : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var acview = uidoc.ActiveView;
            var sel = uidoc.Selection;

            var wiretype = doc.TCollector<WireType>().First();

            while (true)
            {

                try
                {
                     
                    var ele = sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m =>
                    {
                        return m is FamilyInstance;

                    }),"选择电气设备族").GetElement(doc) as FamilyInstance;
                     
                    var wire =
                        sel.PickObject(ObjectType.Element, doc.GetSelectionFilter(m => m is Wire),"选择导线").GetElement(doc) as Wire;

                    var locationline = wire.LocationLine();

                    var geometryele = ele.get_Geometry(new Options() { View = acview, ComputeReferences = true, });

                    var locationpo = (ele.Location as LocationPoint).Point;

                    var box = ele.get_BoundingBox(acview);

                    var transform = ele.GetTotalTransform();
                    //box.Transform = transform;
                    //MessageBox.Show("command Result:" + IslocationNearBottom(ele).ToString());

                    //doc.Newbox(box);
                    //doc.NewCoordinate(locationpo, transform);
                     
                    CreateHideWire(ele, locationline, wiretype, WiringType.Chamfer, acview);
                }
                catch (Exception e)
                {
                    break;

                }
            }
            return Result.Succeeded;
        }

        //public XYZ GetPosition(FamilyInstance ele)
        //{
        //    var transfrom = ele.GetTotalTransform();
        //}

        public double getWidth(FamilyInstance ins)
        {
            var result = default(double);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();
            doc.Regenerate();
            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();

            var box = newIns.get_BoundingBox(doc.ActiveView);
            result = box.Max.X - box.Min.X;
            ts.RollBack();
            return result;
        }
        public double getHeight(FamilyInstance ins)
        {
            var result = default(double);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();

            var box = newIns.get_BoundingBox(doc.ActiveView);

            result = box.Max.Y - box.Min.Y;

            ts.RollBack();
            return result;
        }
        public bool IsLocationInCenter(FamilyInstance ins)
        {
            var result = default(bool);
            var locationpo = default(XYZ);

            var boxcenter = default(XYZ);

            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();
            var box = newIns.get_BoundingBox(doc.ActiveView);

            locationpo = (newIns.Location as LocationPoint).Point;
            boxcenter = (box.Max + box.Min) / 2;
            doc.Regenerate();

            ts.RollBack();

            boxcenter = new XYZ(boxcenter.X, boxcenter.Y, locationpo.Z);
            var horizontalDis = boxcenter.DistanceTo(locationpo);

            if (horizontalDis > 20 / 304.8)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
        public bool IslocationNearBottom(FamilyInstance ins)
        {
            var result = default(bool);
            var locationpo = default(XYZ);

            var boxcenter = default(XYZ);
            var box = default(BoundingBoxXYZ);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();
            box = newIns.get_BoundingBox(doc.ActiveView);

            locationpo = (newIns.Location as LocationPoint).Point;

            ts.RollBack();

            var trans = box.Transform;
            var min = box.Min;
            var max = box.Max;
            var x = max.X - min.X;
            var y = max.Y - min.Y;
            var z = max.Z - min.Z;

            var lineBottom = Line.CreateBound(min, min + x * trans.BasisX);//下边
            var lineLeft = Line.CreateBound(min, min + y * trans.BasisY);//左边

            var newmax = new XYZ(max.X, max.Y, min.Z);
            var lineTop = Line.CreateBound(max, max - x * trans.BasisX);//上边
            var lineRight = Line.CreateBound(max, max - y * trans.BasisY);//右边

            //var dic = new Dictionary<Line, double>();

            //dic.Add(lineBottom,locationpo.DistanceTo(lineBottom));
            //dic.Add(lineLeft,locationpo.DistanceTo(lineLeft));
            //dic.Add(lineRight,locationpo.DistanceTo(lineRight));
            //dic.Add(lineTop,locationpo.DistanceTo(lineTop));

            //dic.OrderBy(m => m.Value);

            //MessageBox.Show(dic[lineBottom].ToString() + Environment.NewLine +
            //                dic[lineTop].ToString());
            //MessageBox.Show("bottom top:"+(lineTop == lineBottom).ToString() + Environment.NewLine +
            //                (dic.First().Key == (lineBottom)).ToString());

            if (lineBottom.Distance(locationpo) < 10 / 304.8)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public bool IslocationNearLeft(FamilyInstance ins)
        {
            var result = default(bool);
            var locationpo = default(XYZ);

            var boxcenter = default(XYZ);
            var box = default(BoundingBoxXYZ);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();
            box = newIns.get_BoundingBox(doc.ActiveView);

            locationpo = (newIns.Location as LocationPoint).Point;

            ts.RollBack();

            var trans = box.Transform;
            var min = box.Min;
            var max = box.Max;
            var x = max.X - min.X;
            var y = max.Y - min.Y;
            var z = max.Z - min.Z;

            var lineBottom = Line.CreateBound(min, min + x * trans.BasisX);//下边
            var lineLeft = Line.CreateBound(min, min + y * trans.BasisY);//左边

            var newmax = new XYZ(max.X, max.Y, min.Z);
            var lineTop = Line.CreateBound(max, max - x * trans.BasisX);//上边
            var lineRight = Line.CreateBound(max, max - y * trans.BasisY);//右边

            //var dic = new Dictionary<Line, double>();

            //dic.Add(lineBottom,locationpo.DistanceTo(lineBottom));
            //dic.Add(lineLeft,locationpo.DistanceTo(lineLeft));
            //dic.Add(lineRight,locationpo.DistanceTo(lineRight));
            //dic.Add(lineTop,locationpo.DistanceTo(lineTop));

            //dic.OrderBy(m => m.Value);

            //MessageBox.Show(dic[lineBottom].ToString() + Environment.NewLine +
            //                dic[lineTop].ToString());
            //MessageBox.Show("bottom top:"+(lineTop == lineBottom).ToString() + Environment.NewLine +
            //                (dic.First().Key == (lineBottom)).ToString());

            if (lineLeft.Distance(locationpo) < 10 / 304.8)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public bool IslocationNearTop(FamilyInstance ins)
        {
            var result = default(bool);
            var locationpo = default(XYZ);

            var boxcenter = default(XYZ);
            var box = default(BoundingBoxXYZ);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();
            box = newIns.get_BoundingBox(doc.ActiveView);

            locationpo = (newIns.Location as LocationPoint).Point;

            ts.RollBack();

            var trans = box.Transform;
            var min = box.Min;
            var max = box.Max;
            var x = max.X - min.X;
            var y = max.Y - min.Y;
            var z = max.Z - min.Z;

            var lineBottom = Line.CreateBound(min, min + x * trans.BasisX);//下边
            var lineLeft = Line.CreateBound(min, min + y * trans.BasisY);//左边

            var newmax = new XYZ(max.X, max.Y, min.Z);
            var lineTop = Line.CreateBound(max, max - x * trans.BasisX);//上边
            var lineRight = Line.CreateBound(max, max - y * trans.BasisY);//右边

            //var dic = new Dictionary<Line, double>();

            //dic.Add(lineBottom,locationpo.DistanceTo(lineBottom));
            //dic.Add(lineLeft,locationpo.DistanceTo(lineLeft));
            //dic.Add(lineRight,locationpo.DistanceTo(lineRight));
            //dic.Add(lineTop,locationpo.DistanceTo(lineTop));

            //dic.OrderBy(m => m.Value);

            //MessageBox.Show(dic[lineBottom].ToString() + Environment.NewLine +
            //                dic[lineTop].ToString());
            //MessageBox.Show("bottom top:"+(lineTop == lineBottom).ToString() + Environment.NewLine +
            //                (dic.First().Key == (lineBottom)).ToString());

            if (lineTop.Distance(locationpo) < 10 / 304.8)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public void CreateHideWire(FamilyInstance ins, Line wireLine, WireType type, WiringType wiringtype, View view)
        {
            var doc = ins.Document;
            var locationpo = (ins.Location as LocationPoint).Point;

            var linedir = wireLine.Direction;

            if (IsLocationInCenter(ins))
            {
                var facingDir = ins.FacingOrientation;
                var handDir = ins.HandOrientation;

                var viewscale = view.Scale;
                var lengthRatio = viewscale / 50.0;
                var width = lengthRatio * getWidth(ins);
                var height = lengthRatio * getHeight(ins);

                var po1 = default(XYZ);// locationpo + height / 4 * handDir;
                var po2 = default(XYZ);//locationpo - height / 4 * handDir;

                if (linedir.AngleTo(handDir) < Math.PI / 4)
                {
                    po1 = locationpo + width / 4 * handDir;
                    po2 = locationpo - width / 4 * handDir;
                }
                else
                {
                    po1 = locationpo + height / 4 * facingDir;
                    po2 = locationpo - height / 4 * facingDir;
                }

                var polist = new List<XYZ>();
                polist.Add(po1);
                polist.Add(locationpo);
                polist.Add(po2);

                //Wire.Create(doc, type.Id, view.Id, wiringtype,, null, null);
                doc.Invoke(m =>
                {
                    foreach (var po in polist)
                    {
                        CreateHideWire(po, wireLine, type, wiringtype, view, 3);
                    }
                }, "tem");
            }

            if (IslocationNearBottom(ins))
            {
                var facingDir = ins.FacingOrientation;
                var handDir = ins.HandOrientation;

                var viewscale = view.Scale;
                var lengthRatio = viewscale / 50.0;
                var width = lengthRatio * getWidth(ins);
                var height = lengthRatio * getHeight(ins);

                var po1 = default(XYZ);// locationpo + height / 4 * handDir;
                var po2 = default(XYZ);//locationpo - height / 4 * handDir;

                if (linedir.AngleTo(handDir) < Math.PI / 4)
                {
                    po1 = locationpo + width / 4.0 * handDir;
                    po2 = locationpo - width / 4.0 * handDir;
                }
                else
                {

                    po1 = locationpo + height / 4 * facingDir;
                    po2 = po1 + height / 4 * facingDir;
                }

                var polist = new List<XYZ>();
                polist.Add(po1);
                polist.Add(locationpo);
                polist.Add(po2);

                //Wire.Create(doc, type.Id, view.Id, wiringtype,, null, null);
                doc.Invoke(m =>
                {
                    foreach (var po in polist)
                    {
                        CreateHideWire(po, wireLine, type, wiringtype, view, 3);
                    }
                }, "tem");

            }
            else
            {
                var facingDir = ins.FacingOrientation;
                var handDir = ins.HandOrientation;

                var viewscale = view.Scale;
                var lengthRatio = viewscale / 50.0;
                var width = lengthRatio * getWidth(ins);
                var height = lengthRatio * getHeight(ins);

                var po1 = default(XYZ);// locationpo + height / 4 * handDir;
                var po2 = default(XYZ);//locationpo - height / 4 * handDir;

                if (linedir.AngleTo(handDir) < Math.PI / 4)
                {
                    po1 = locationpo + width / 4 * handDir;
                    po2 = locationpo - width / 4 * handDir;
                }
                else
                {
                    po1 = locationpo + height / 4 * facingDir;
                    po2 = locationpo - height / 4 * facingDir;
                }

                var polist = new List<XYZ>();
                polist.Add(po1);
                polist.Add(locationpo);
                polist.Add(po2);

                //Wire.Create(doc, type.Id, view.Id, wiringtype,, null, null);
                doc.Invoke(m =>
                {
                    foreach (var po in polist)
                    {
                        CreateHideWire(po, wireLine, type, wiringtype, view, 3);
                    }
                }, "tem");

            }


        }

        public void CreateHideWire(XYZ po, Line wireline, WireType type, WiringType wiringtype, View view, double halfdis/*mm*/)
        {
            var doc = type.Document;

            var pointonLine = po.ProjectToXLine(wireline);

            var dir = wireline.Direction.CrossProduct(XYZ.BasisZ).Normalize();//(pointonLine - po).Normalize();

            var point1 = pointonLine + dir * halfdis / 304.8;
            var point2 = pointonLine - dir * halfdis / 304.8;

            //MessageBox.Show(dir.ToString() + Environment.NewLine +
            //    point1.ToString() + Environment.NewLine +
            //                point2.ToString());

            var polist = new List<XYZ>();

            polist.Add(point1);
            polist.Add(point2);

            Wire.Create(doc, type.Id, view.Id, wiringtype, polist, null, null);
        }
    }

    public class FsInfo
    {
        public FsInfo(FamilyInstance ins)
        {
            id = ins.Id;
            width = getWidth(ins);
            height = getHeight(ins);
            _locationInCenter = IsLocationInCenter(ins);
            locationNearBottom = IslocationNearBottom(ins);
            outline = getoutline(ins, width, height);
        }

        public FsInfo(FamilySymbol symbol)
        {
            //id = symbol.Id;
            //width = getWidth(symbol);
            //height = getHeight(symbol);
            //_locationInCenter = IsLocationInCenter(symbol);
            //locationNearBottom = IslocationNearBottom(symbol);
        }

        private ElementId id;
        private double width;
        private double height;
        private List<XYZ> outline;

        private bool _locationInCenter;
        private bool locationNearBottom;

        public ElementId Id
        {
            get { return id; }
            set { id = value; }
        }

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public List<XYZ> Outline1
        {
            get { return outline; }
            set { outline = value; }
        }

        public bool LocationInCenter
        {
            get { return _locationInCenter; }
            set { _locationInCenter = value; }
        }

        public bool LocationNearBottom
        {
            get { return locationNearBottom; }
            set { locationNearBottom = value; }
        }

        private double getWidth(FamilyInstance ins)
        {
            var result = default(double);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();
            doc.Regenerate();
            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();

            var box = newIns.get_BoundingBox(doc.ActiveView);
            result = box.Max.X - box.Min.X;
            ts.RollBack();
            return result;
        }
        private double getHeight(FamilyInstance ins)
        {
            var result = default(double);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();

            var box = newIns.get_BoundingBox(doc.ActiveView);

            result = box.Max.Y - box.Min.Y;

            ts.RollBack();
            return result;
        }
        private bool IsLocationInCenter(FamilyInstance ins)
        {
            var result = default(bool);
            var locationpo = default(XYZ);

            var boxcenter = default(XYZ);

            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();
            var box = newIns.get_BoundingBox(doc.ActiveView);

            locationpo = (newIns.Location as LocationPoint).Point;
            boxcenter = (box.Max + box.Min) / 2;
            doc.Regenerate();

            ts.RollBack();

            boxcenter = new XYZ(boxcenter.X, boxcenter.Y, locationpo.Z);
            var horizontalDis = boxcenter.DistanceTo(locationpo);

            if (horizontalDis > 20 / 304.8)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
        private bool IslocationNearBottom(FamilyInstance ins)
        {
            var result = default(bool);
            var locationpo = default(XYZ);

            var boxcenter = default(XYZ);
            var box = default(BoundingBoxXYZ);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();
            box = newIns.get_BoundingBox(doc.ActiveView);

            locationpo = (newIns.Location as LocationPoint).Point;

            ts.RollBack();

            var trans = box.Transform;
            var min = box.Min;
            var max = box.Max;
            var x = max.X - min.X;
            var y = max.Y - min.Y;
            var z = max.Z - min.Z;

            var lineBottom = Line.CreateBound(min, min + x * trans.BasisX);//下边
            var lineLeft = Line.CreateBound(min, min + y * trans.BasisY);//左边

            var newmax = new XYZ(max.X, max.Y, min.Z);
            var lineTop = Line.CreateBound(max, max - x * trans.BasisX);//上边
            var lineRight = Line.CreateBound(max, max - y * trans.BasisY);//右边

            //var dic = new Dictionary<Line, double>();

            //dic.Add(lineBottom,locationpo.DistanceTo(lineBottom));
            //dic.Add(lineLeft,locationpo.DistanceTo(lineLeft));
            //dic.Add(lineRight,locationpo.DistanceTo(lineRight));
            //dic.Add(lineTop,locationpo.DistanceTo(lineTop));

            //dic.OrderBy(m => m.Value);

            //MessageBox.Show(dic[lineBottom].ToString() + Environment.NewLine +
            //                dic[lineTop].ToString());
            //MessageBox.Show("bottom top:"+(lineTop == lineBottom).ToString() + Environment.NewLine +
            //                (dic.First().Key == (lineBottom)).ToString());

            if (lineBottom.Distance(locationpo) < 10 / 304.8)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        private bool IslocationNearLeft(FamilyInstance ins)
        {
            var result = default(bool);
            var locationpo = default(XYZ);

            var boxcenter = default(XYZ);
            var box = default(BoundingBoxXYZ);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();
            box = newIns.get_BoundingBox(doc.ActiveView);

            locationpo = (newIns.Location as LocationPoint).Point;

            ts.RollBack();

            var trans = box.Transform;
            var min = box.Min;
            var max = box.Max;
            var x = max.X - min.X;
            var y = max.Y - min.Y;
            var z = max.Z - min.Z;

            var lineBottom = Line.CreateBound(min, min + x * trans.BasisX);//下边
            var lineLeft = Line.CreateBound(min, min + y * trans.BasisY);//左边

            var newmax = new XYZ(max.X, max.Y, min.Z);
            var lineTop = Line.CreateBound(max, max - x * trans.BasisX);//上边
            var lineRight = Line.CreateBound(max, max - y * trans.BasisY);//右边

            //var dic = new Dictionary<Line, double>();

            //dic.Add(lineBottom,locationpo.DistanceTo(lineBottom));
            //dic.Add(lineLeft,locationpo.DistanceTo(lineLeft));
            //dic.Add(lineRight,locationpo.DistanceTo(lineRight));
            //dic.Add(lineTop,locationpo.DistanceTo(lineTop));

            //dic.OrderBy(m => m.Value);

            //MessageBox.Show(dic[lineBottom].ToString() + Environment.NewLine +
            //                dic[lineTop].ToString());
            //MessageBox.Show("bottom top:"+(lineTop == lineBottom).ToString() + Environment.NewLine +
            //                (dic.First().Key == (lineBottom)).ToString());

            if (lineLeft.Distance(locationpo) < 10 / 304.8)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        private bool IslocationNearTop(FamilyInstance ins)
        {
            var result = default(bool);
            var locationpo = default(XYZ);

            var boxcenter = default(XYZ);
            var box = default(BoundingBoxXYZ);
            var doc = ins.Document;
            var symbol = ins.Symbol;
            Transaction ts = new Transaction(doc, "tem");

            ts.Start();

            var newIns = doc.Create.NewFamilyInstance(new XYZ(), symbol, StructuralType.NonStructural);
            doc.Regenerate();
            box = newIns.get_BoundingBox(doc.ActiveView);

            locationpo = (newIns.Location as LocationPoint).Point;

            ts.RollBack();

            var trans = box.Transform;
            var min = box.Min;
            var max = box.Max;
            var x = max.X - min.X;
            var y = max.Y - min.Y;
            var z = max.Z - min.Z;

            var lineBottom = Line.CreateBound(min, min + x * trans.BasisX);//下边
            var lineLeft = Line.CreateBound(min, min + y * trans.BasisY);//左边

            var newmax = new XYZ(max.X, max.Y, min.Z);
            var lineTop = Line.CreateBound(max, max - x * trans.BasisX);//上边
            var lineRight = Line.CreateBound(max, max - y * trans.BasisY);//右边

            //var dic = new Dictionary<Line, double>();

            //dic.Add(lineBottom,locationpo.DistanceTo(lineBottom));
            //dic.Add(lineLeft,locationpo.DistanceTo(lineLeft));
            //dic.Add(lineRight,locationpo.DistanceTo(lineRight));
            //dic.Add(lineTop,locationpo.DistanceTo(lineTop));

            //dic.OrderBy(m => m.Value);

            //MessageBox.Show(dic[lineBottom].ToString() + Environment.NewLine +
            //                dic[lineTop].ToString());
            //MessageBox.Show("bottom top:"+(lineTop == lineBottom).ToString() + Environment.NewLine +
            //                (dic.First().Key == (lineBottom)).ToString());

            if (lineTop.Distance(locationpo) < 10 / 304.8)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        List<XYZ> getoutline(FamilyInstance ins, double width, double height)
        {
            var result = new List<XYZ>();

            var locationPo = (ins.Location as LocationPoint).Point;
            var trans = ins.GetTotalTransform();

            var xdir = trans.BasisX;
            var ydir = trans.BasisY;
            var zdir = trans.BasisZ;

            var ratio = ins.Document.ActiveView.Scale / 50;
            height = ratio * height;
            width = ratio * width;
            if (IsLocationInCenter(ins))
            {
                var tempo1 = locationPo + xdir * width / 2;
                var tempo2 = locationPo - xdir * width / 2;

                var vertex1 = tempo2 + ydir * height / 2;
                var vertex2 = tempo1 + ydir * height / 2;

                var vertex3 = tempo1 - ydir * height / 2;
                var vertex4 = tempo2 - ydir * height / 2;

                result.Add(vertex1);
                result.Add(vertex2);
                result.Add(vertex3);
                result.Add(vertex4);

            }

            if (IslocationNearBottom(ins))
            {
                var tempo1 = locationPo + xdir * width / 2;
                var tempo2 = locationPo - xdir * width / 2;

                var vertex1 = tempo2 + ydir * height;
                var vertex2 = tempo1 + ydir * height;

                var vertex3 = tempo1;
                var vertex4 = tempo2;

                result.Add(vertex1);
                result.Add(vertex2);
                result.Add(vertex3);
                result.Add(vertex4);
            }

            return result;
        }

    }
}
