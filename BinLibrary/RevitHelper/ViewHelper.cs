using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BinLibrary.RevitExtension;
using BinLibrary.Extensions;
using Application = Autodesk.Revit.ApplicationServices.Application;
using Color = Autodesk.Revit.DB.Color;
#if revit2016
using Rectangle = Autodesk.Revit.UI.Rectangle;
#endif
#if revit2019
using Rectangle = Autodesk.Revit.DB.Rectangle;
#endif
namespace BinLibrary.RevitHelper
{
    public static class ViewHelper
    {
        /// <summary>
        /// 设置视图背景颜色
        /// </summary>
        /// <param name="view"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static bool SetBackGroundColor(this View view, Color color)
        {
            try
            {
                Document doc = view.Document;
                Application app = doc.Application;
                app.BackgroundColor = color;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static Color GetBackGroundColor(this View view)
        {
            try
            {
                Document doc = view.Document;
                Application app = doc.Application;
                return app.BackgroundColor;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 屏幕点转化为三维空间点
        /// </summary>
        /// <param name="view"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static XYZ ScreenToWorld(this View view, System.Drawing.Point point)
        {
            XYZ result = null;
            if (view == null)
            {
                throw new NullReferenceException();
            }
            var uidoc = new UIDocument(view.Document);
            var uiview = uidoc.GetOpenUIViews().FirstOrDefault(m => m.ViewId == view.Id);
            if (uiview == null)
            {
                throw new NullReferenceException();
            }
            var rect = uiview.GetWindowRectangle();
            var corners = uiview.GetZoomCorners();
            var screenleftlower = new XYZ(rect.Left, rect.Bottom, 0);
            var screenrighttop = new XYZ(rect.Right, rect.Top, 0);
            var scale = corners[0].DistanceTo(corners[1]) / screenleftlower.DistanceTo(screenrighttop);
            var xdis = scale * (point.X - rect.Left);
            var ydis = scale * (rect.Bottom - point.Y);
            var vup = uidoc.ActiveView.UpDirection;
            var vri = uidoc.ActiveView.RightDirection;
            result = corners[0] + xdis * vri + ydis * vup;
            return result;
        }
        /// <summary>
        /// 将世界坐标系的点投影到屏幕平面然后计算在屏幕坐标系上的坐标 修改版
        /// </summary>
        /// <param name="view"></param>
        /// <param name="xyz"></param>
        /// <returns></returns>
        public static System.Drawing.Point WorldToScreen(this View view, XYZ xyz)
        {
            if (view == null)
            {
                throw new NullReferenceException();
            }
            System.Drawing.Point resultPoint = System.Drawing.Point.Empty;
            Plane p = null;
            UIDocument uidoc = new UIDocument(view.Document);
            UIView uiview = uidoc.ActiveUiview();
            XYZ pointOnPlane = uiview.GetZoomCorners()[0];
#if revit2016
            p = new Plane(view.ViewDirection.Normalize(), pointOnPlane);
#endif
#if revit2019
            p = Plane.CreateByNormalAndOrigin(view.ViewDirection.Normalize(), pointOnPlane);
#endif
            Line rayLine = Line.CreateUnbound(xyz, view.ViewDirection);
            //XYZ intersectionPoint = Ray.GetIntersectionPoint(rayLine, p);
            XYZ intersectionPoint = xyz.Project(p);
            Rectangle rect = uiview.GetWindowRectangle();
            IList<XYZ> corners = uiview.GetZoomCorners();
            var leftlower = new XYZ((double)rect.Left, (double)rect.Bottom, 0d);
            var righttop = new XYZ((double)rect.Right, (double)rect.Top, 0d);
            var lefttop = new XYZ((double)rect.Left, (double)rect.Top, 0d);
            double scale = leftlower.DistanceTo(righttop) / corners[0].DistanceTo(corners[1]);
            Line xline = Line.CreateUnbound(corners[0], view.RightDirection);     // 下边线
            Line yline = Line.CreateUnbound(corners[0], view.UpDirection);        //左 边线
            Line xline_up = Line.CreateUnbound(corners[1], view.RightDirection);     // 上边线
            Line yline_right = Line.CreateUnbound(corners[1], view.UpDirection);     //右 边线
            double ydis = xline_up.Distance(intersectionPoint) * scale;    //左上角点为基准求坐标
            double xdis = yline.Distance(intersectionPoint) * scale;       //左上角点为基准求坐标
            //Transform ts = Transform.Identity;
            //ts.Origin = corners[0];
            //ts.BasisX = view.RightDirection.Normalize();
            //ts.BasisY = view.UpDirection.Normalize();
            //ts.BasisZ = view.RightDirection.CrossProduct(view.UpDirection).Normalize();
            //uiview 左上角点构造transform 方向x 向右 y 向下
            Transform ts = Transform.Identity;
            ts.Origin = xline_up.Intersect_cus(yline_right);
            ts.BasisX = view.RightDirection.Normalize();
            ts.BasisY = -view.UpDirection.Normalize();
            ts.BasisZ = ts.BasisX.CrossProduct(ts.BasisY).Normalize();
            XYZ result = lefttop + new XYZ(xdis, ydis, 0);         //左上角点为基准
            resultPoint = new System.Drawing.Point((int)result.X, (int)result.Y);
            return resultPoint;
        }
        /// <summary>
        /// 将世界坐标系的点投影到屏幕平面然后计算在屏幕坐标系上的坐标 修改版 此方法可以越过视图边界
        /// </summary>
        /// <param name="view"></param>
        /// <param name="xyz"></param>
        /// <returns></returns>
        public static System.Drawing.Point WorldToScreen_CrossBorder(this View view, XYZ xyz)
        {
            if (view == null)
            {
                throw new NullReferenceException();
            }
            System.Drawing.Point resultPoint = System.Drawing.Point.Empty;
            Plane p = null;
            UIDocument uidoc = new UIDocument(view.Document);
            UIView uiview = uidoc.ActiveUiview();
            XYZ pointOnPlane = uiview.GetZoomCorners()[0];
#if revit2016
            p = new Plane(view.ViewDirection.Normalize(), pointOnPlane);
#endif
#if revit2019
            p = Plane.CreateByNormalAndOrigin(view.ViewDirection.Normalize(), pointOnPlane);
#endif
            Line rayLine = Line.CreateUnbound(xyz, view.ViewDirection);
            //XYZ intersectionPoint = Ray.GetIntersectionPoint(rayLine, p);
            XYZ intersectionPoint = xyz.Project(p);
            Rectangle rect = uiview.GetWindowRectangle();
            IList<XYZ> corners = uiview.GetZoomCorners();
            var leftlower = new XYZ((double)rect.Left, (double)rect.Bottom, 0d);
            var righttop = new XYZ((double)rect.Right, (double)rect.Top, 0d);
            var lefttop = new XYZ((double)rect.Left, (double)rect.Top, 0d);
            double scale = leftlower.DistanceTo(righttop) / corners[0].DistanceTo(corners[1]);
            Line xline = Line.CreateUnbound(corners[0], view.RightDirection);     // 下边线
            Line yline = Line.CreateUnbound(corners[0], view.UpDirection);        //左 边线
            Line xline_up = Line.CreateUnbound(corners[1], view.RightDirection);     // 上边线
            Line yline_right = Line.CreateUnbound(corners[1], view.UpDirection);     //右 边线
            double ydis = xline_up.Distance(intersectionPoint) * scale;    //左上角点为基准求坐标
            double xdis = yline.Distance(intersectionPoint) * scale;       //左上角点为基准求坐标
            //Transform ts = Transform.Identity;
            //ts.Origin = corners[0];
            //ts.BasisX = view.RightDirection.Normalize();
            //ts.BasisY = view.UpDirection.Normalize();
            //ts.BasisZ = view.RightDirection.CrossProduct(view.UpDirection).Normalize();
            //uiview 左上角点构造transform 方向x 向右 y 向下
            Transform ts = Transform.Identity;
            ts.Origin = xline_up.Intersect_cus(yline);
            ts.BasisX = view.RightDirection.Normalize();
            ts.BasisY = -view.UpDirection.Normalize();
            ts.BasisZ = ts.BasisX.CrossProduct(ts.BasisY).Normalize();
            //将屏幕面与 位置点法线的交点 通过trnasform 转换到 transform坐标系 （右为 x正方向 下 为Y正方向）
            XYZ intersec_trs = ts.Inverse.OfPoint(intersectionPoint);
            if (intersec_trs.X < 0)
            {
                xdis = -xdis;
            }
            if (intersec_trs.Y < 0)
            {
                ydis = -ydis;
            }
            XYZ result = lefttop + new XYZ(xdis, ydis, 0);         //左上角点为基准
            resultPoint = new System.Drawing.Point((int)result.X, (int)result.Y);
            return resultPoint;
        }
        /// <summary>
        /// 世界坐标到屏幕坐标
        /// </summary>
        /// <param name="view"></param>
        /// <param name="xyz"></param>
        /// <returns></returns>
        public static System.Drawing.Point WorldToScreen_CrossBorder1(this View view, XYZ xyz)
        {
            System.Drawing.Point result = System.Drawing.Point.Empty;
            if (view == null)
            {
                throw new Exception("view is null");
            }
            var uidoc = new UIDocument(view.Document);
            var uiview = uidoc.GetOpenUIViews().First(m => m.ViewId == view.Id);
            var corners = uiview.GetZoomCorners();
            var rect = uiview.GetWindowRectangle();
#if revit2016
            var plane = new Plane(view.ViewDirection, corners[0]);
#endif
#if revit2019
            var plane = Plane.CreateByNormalAndOrigin(view.ViewDirection, corners[0]);
#endif
            var pointOnplane = xyz.Project(plane);
            var line_x_up = Line.CreateUnbound(corners[1], view.RightDirection);
            var line_y_left = Line.CreateUnbound(corners[0], view.UpDirection);
            var lefttopInWorld = line_x_up.Intersect_cus(line_y_left);
            var leftlower = new XYZ(rect.Left, rect.Bottom, 0d);
            var righttop = new XYZ(rect.Right, rect.Top, 0d);
            var scale = leftlower.DistanceTo(righttop) / corners[0].DistanceTo(corners[1]);
            Transform ts = Transform.Identity;
            ts.Origin = lefttopInWorld;
            ts.BasisX = view.RightDirection;
            ts.BasisY = -view.UpDirection;
            ts.BasisZ = -view.ViewDirection;
            var pointOnWindow = ts.Inverse.OfPoint(pointOnplane);
            result = new System.Drawing.Point((int)(pointOnWindow.X * scale), (int)(pointOnWindow.Y * scale));
            return result;
        }
        /// <summary>
        /// 设置元素颜色(封装的意义不太大)
        /// </summary>
        /// <param name="view"></param>
        /// <param name="element"></param>
        /// <param name="color"></param> 
        public static void SetElementColor(this View view, Element element, Color color)
        {
            var overrideGraphicsettings = new OverrideGraphicSettings();
            var elementOverrides = view.GetElementOverrides(element.Id);
            overrideGraphicsettings.SetProjectionFillColor(RvtColor.LightBlue);
            overrideGraphicsettings.SetSurfaceTransparency(70);
            view.SetElementOverrides(element.Id, overrideGraphicsettings);
        }

        /// <summary>
        /// 三点创建剖面
        /// </summary>
        /// <param name="seedView"></param>
        /// <param name="firstP"></param>
        /// <param name="secondP"></param>
        /// <param name="thirdP"></param>
        /// <param name="sectopEle"></param>
        /// <param name="secbottomEle"></param>
        /// <returns></returns>
        public static ViewSection CreateRandomSection(this View seedView, XYZ firstP, XYZ secondP, XYZ thirdP, double sectopEle, double secbottomEle)
        {
            Document doc = seedView.Document;
            View acview = seedView;
            if (!(acview is ViewPlan))
            {
                //MessageBox.Show("当前视图不是平面图 请转到平面图");
                //return;
            }
            var viewfamilytype = doc.TCollector<ViewFamilyType>().First(m => m.ViewFamily == ViewFamily.Section);

            //顶-底 的差
            double delta = sectopEle - secbottomEle;
            if (delta <= 0)
            {
                delta = 4000d.MetricToFeet();
            }
            var secdepth = 0d;

            secdepth = secdepth.MetricToFeet();
            ViewSection vc = null;

            var firstpoint = firstP;//_sel.PickPoint("拾取剖面第一点");
            var secondpoint = secondP;// _sel.PickPoint("拾取剖面第二点");
            var directionPoint = thirdP;//_sel.PickPoint("拾取剖视线一侧的点");
            Line line = Line.CreateBound(firstpoint, secondpoint);
            var directionPointOnLine = directionPoint.ProjectToXLine(line);
            //if (setdepthChk.IsChecked != true)
            secdepth = line.DistanceTo(directionPoint);
            var width = line.Length;
            var height = delta;
            var depth = secdepth;
            var originElevation = (sectopEle + secbottomEle) / 2;
            var origin = (firstpoint + secondpoint) / 2;
            origin = new XYZ(origin.X, origin.Y, originElevation);
            var dirY = XYZ.BasisZ;
            var dirZ = (-directionPointOnLine + directionPoint).Normalize();
            var dirX = dirY.CrossProduct(dirZ);
            //origin 沿z轴负向偏移 
            //origin = origin + dirZ * secdepth; // 与下面的是对应的   ------>|
            Transform ts = Transform.Identity;                             // |
            ts.Origin = origin;                                            // |
            ts.BasisX = dirX;                                              // |
            ts.BasisY = dirY;                                              // |
            ts.BasisZ = dirZ;                                              // |
                                                                           // |
                                                                           ////for test                                                   // |
                                                                           //doc.NewLine_WithoutTransaction(origin,origin + dirX*5);      // |
                                                                           //doc.NewLine_WithoutTransaction(origin,origin + dirY*3);      // |
                                                                           //doc.NewLine_WithoutTransaction(origin,origin + dirZ*2);      // |
                                                                           // |
            BoundingBoxXYZ box = new BoundingBoxXYZ();                     // |
            box.Transform = ts;                                            // |
                                                                           // |
                                                                           //box.Min = new XYZ(-width / 2, -height / 2, -depth);  <-------// |
                                                                           //box.Max = new XYZ(width / 2, height / 2, 0);         <-------// |

            box.Min = new XYZ(-width / 2, -height / 2, 0);
            box.Max = new XYZ(width / 2, height / 2, depth);

            ////for test
            //doc.NewModelLineXYZ_withoutTransaction(ts.OfPoint(box.Min));
            //doc.NewModelLineXYZ_withoutTransaction(ts.OfPoint(box.Max));

            vc = ViewSection.CreateSection(doc, viewfamilytype.Id, box);
            return vc;
        }

        /// <summary>
        /// 在三维视图中创建剖面 根据鼠标拾取的第一点和第二点确定剖视方向（第一点第二点连线投影到水平面然后求得方向）
        /// 如果上下标高存在则高度为上下标高的高度，如果不存在那么 高度按照指定高度。
        /// </summary>
        /// <param name="seedView"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ViewSection CreateRandomSecInView3D(this View3D seedView, XYZ point1, XYZ point2, double width/*剖面宽*/, double height/*剖面高*/)
        {
            ViewSection result = null;

            var horizontalOfpo1 = point1.xyComponent();
            var horizontalOfpo2 = point2.xyComponent();

            if (horizontalOfpo1.IsAlmostEqualTo(horizontalOfpo2)) throw new Exception("两点水平间距太小");

            var doc = seedView.Document;
            var levels = doc.TCollector<Level>();
            var downlevel = levels.Where(m => ( m.Elevation - point1.Z) <= 1e-6).OrderBy(m => m.Elevation)?.Last();
            var overlevels = levels.Where(m => (m.Elevation - point1.Z) > 1e-6);
            var uplevel = overlevels.Count() > 0 ? overlevels.OrderBy(m => m.Elevation)?.First() : null;

            var viewfamilytype = doc.TCollector<ViewFamilyType>().First(m => m.ViewFamily == ViewFamily.Section);

            var downEle = downlevel.Elevation;
            var upEle = uplevel?.Elevation ?? downEle + height;

            //MessageBox.Show(downEle.FeetToMetric().ToString() + Environment.NewLine +
            //                upEle.FeetToMetric().ToString());

            var delta = upEle - downEle;

            var pointVec = (point2 - point1);
            var viewVec = pointVec.xyComponent();
            var viewDir = viewVec.Normalize();

            var veclength = viewVec.GetLength();//两点水平长度 用于指示剖面深度

            var viewStartPo = point1;
            var viewEndPo = viewStartPo + viewVec;

            //var origin = viewStartPo + XYZ.BasisZ * delta / 2;
            var origin = viewStartPo.xyComponent() + XYZ.BasisZ * (downEle + upEle) / 2;
            var vecY = XYZ.BasisZ;
            var vecZ = viewDir;
            var vecX = vecY.CrossProduct(vecZ).Normalize();

            var min = new XYZ(-width / 2, -delta / 2, 0);
            var max = new XYZ(width / 2, delta / 2, veclength);

            var transform = Transform.Identity;
            transform.Origin = origin;
            transform.BasisX = vecX;
            transform.BasisY = vecY;
            transform.BasisZ = vecZ;

            BoundingBoxXYZ box = new BoundingBoxXYZ();
            box.Transform = transform;
            box.Min = min;
            box.Max = max;

            result = ViewSection.CreateSection(doc, viewfamilytype.Id, box);
            return result;
        }

        /// <summary>
        /// 拾取框创建剖面
        /// </summary>
        /// <param name="seedView"></param>
        /// <param name="pb"></param>
        /// <param name="sectopEle"></param>
        /// <param name="secbottomEle"></param>
        /// <param name="bottomoffset"></param>
        /// <param name="topoffset"></param>
        /// <returns></returns>
        public static ViewSection CreateHorisontalSection(this View seedView, PickedBox pb, double sectopEle, double secbottomEle, double bottomoffset, double topoffset)
        {
            ViewSection vsec1 = null;
            var activeDoc = seedView.Document;

            //Transaction ts = new Transaction(activeDoc, "test");
            try
            {
                //ts.Start();
                #region creat_new_section
                XYZ raw1, raw2;      //save picked 2 points
                double raw_z, raw_z1; //define raw_z and raw_z1 as bottom and top of new cropbox
                //if (currentlevel != null)
                //{
                //    raw_z = currentlevel.ProjectElevation + bottomoffset; //在剖面中 由于当前视图没有与之相关联的 GenLevel 所以在剖面中此句代码会出错 故用if来判断是否在平面中
                //}
                //else
                //{
                //    raw_z = 0;
                //}
                ////raw_z1 = raw_z + 3800 / 304.8;
                //if (levelup == null)
                //{
                //    raw_z1 = raw_z + 4000 / 304.8;
                //}
                //else
                //{
                //    raw_z1 = levelup.ProjectElevation + topoffset;
                //}

                raw_z = secbottomEle + bottomoffset; //在剖面中 由于当前视图没有与之相关联的 GenLevel 所以在剖面中此句代码会出错 故用if来判断是否在平面中
                raw_z1 = sectopEle + topoffset;
                //获得视图类型id 
                IEnumerable<ViewFamilyType> viewFamilyTypes = from elem in new FilteredElementCollector(activeDoc).OfClass(typeof(ViewFamilyType))
                                                              let type = elem as ViewFamilyType
                                                              where type.ViewFamily == ViewFamily.Section
                                                              select type;
                //鼠标拾取平面框的 min max
                XYZ min = pb.Min;
                XYZ max = pb.Max;
                double[] xnum = new[] { min.X, max.X };
                double[] ynum = new[] { min.Y, max.Y };
                double[] znum = new[] { raw_z, raw_z1 };
                //TaskDialog.Show("min max", min.ToString() + Environment.NewLine +
                //                           max.ToString());
                //转变前cropBox角点值 由下向上看
                XYZ cropXyz_min = new XYZ(xnum.Min(), ynum.Min(), znum.Min()); //最小角点 
                XYZ cropXyz_max = new XYZ(xnum.Max(), ynum.Max(), znum.Max()); //最大角点
                                                                               //转变前cropbox角点值 由上向下看
                XYZ cropXyz_min2 = new XYZ(cropXyz_max.X, cropXyz_max.Y, cropXyz_min.Z);
                XYZ cropXyz_max2 = new XYZ(cropXyz_min.X, cropXyz_min.Y, cropXyz_max.Z);
                //TaskDialog.Show("cropxyz_min cropxyz_max", cropXyz_min.ToString() + Environment.NewLine +
                //                                            cropXyz_max.ToString());
                //获取拾取框上边和下边中点点坐标
                XYZ max_second = new XYZ((cropXyz_min.X + cropXyz_max.X) / 2, cropXyz_max.Y, cropXyz_min.Z);   //上边中点坐标
                XYZ min_second = new XYZ((cropXyz_min.X + cropXyz_max.X) / 2, cropXyz_min.Y, cropXyz_min.Z);   //下边中点坐标
                                                                                                               //获取拾取框右下角坐标
                XYZ cropXyz_min_1 = new XYZ(cropXyz_max.X, cropXyz_min.Y, znum.Min());   //端点 用于计算 沿线向量
                //底边中点 向右的单位向量
                XYZ alongdirection = (min_second - cropXyz_min).Normalize();
                //底边中点向左的单位向量
                XYZ alongdirection1 = (min_second - cropXyz_min_1).Normalize();
                //从底边中点由下向上的单位向量
                XYZ viewdirection = (min_second - max_second).Normalize();
                //从顶边中点由上向下的单位向量
                XYZ viewdirection1 = (max_second - min_second).Normalize();
                //定义transform
                Transform tsf1 = Transform.Identity;
                tsf1.BasisX = alongdirection1;
                tsf1.BasisY = XYZ.BasisZ;
                tsf1.BasisZ = alongdirection1.CrossProduct(tsf1.BasisY);
                tsf1.Origin = min_second;

                //activeDoc.NewLine_WithoutTransaction(min_second,min_second+tsf1.BasisX*5);
                //activeDoc.NewLine_WithoutTransaction(min_second,min_second+tsf1.BasisY*3);
                //activeDoc.NewLine_WithoutTransaction(min_second,min_second+tsf1.BasisZ*2);
                //activeDoc.NewModelLineXYZ_withoutTransaction(min_second);
                //activeDoc.NewModelLineXYZ_withoutTransaction(cropXyz_min_1);

                //计算transform2
                //左上角点 
                XYZ cropXyz_max_1 = new XYZ(cropXyz_min.X, cropXyz_max.Y, znum.Min());
                Transform tsf2 = Transform.Identity;
                tsf2.BasisX = (max_second - cropXyz_max_1).Normalize();
                tsf2.BasisY = XYZ.BasisZ;
                tsf2.BasisZ = tsf2.BasisX.CrossProduct(XYZ.BasisZ);
                tsf2.Origin = max_second;
                /*据说cropbox的 左上角点为 cropbox 的max 右下角点 为cropbox的min  据此计算transform 在进行一次实验 验证此说法，以前一直以为左下是min 右上 是max*/
                //TaskDialog.Show("trans.basis x y z", "origin" + tsf1.Origin + Environment.NewLine +
                //                                     "x" + tsf1.BasisX.ToString() + Environment.NewLine +
                //                                     "y" + tsf1.BasisY.ToString() + Environment.NewLine +
                //                                     "z" + tsf1.BasisZ.ToString());
                //转变后cropBox角点1
                XYZ cropXyz_min_trans = tsf1.Inverse.OfPoint(cropXyz_min);
                XYZ corpXyz_max_trans = tsf1.Inverse.OfPoint(cropXyz_max);
                double[] xnum1 = new double[] { cropXyz_min_trans.X, corpXyz_max_trans.X };
                double[] ynum1 = new double[] { cropXyz_min_trans.Y, corpXyz_max_trans.Y };
                double[] znum1 = new double[] { cropXyz_min_trans.Z, corpXyz_max_trans.Z };
                XYZ cropXyz_min1 = new XYZ(xnum1.Min(), ynum1.Min(), znum1.Min());
                XYZ cropXyz_max1 = new XYZ(xnum1.Max(), ynum1.Max(), znum1.Max());//
                BoundingBoxXYZ cropbox = new BoundingBoxXYZ();
                cropbox.Min = cropXyz_min1;
                cropbox.Max = cropXyz_max1;
                cropbox.Transform = tsf1;


                //activeDoc.NewModelLineXYZ_withoutTransaction(tsf1.OfPoint(cropbox.Min));
                //activeDoc.NewModelLineXYZ_withoutTransaction(tsf1.OfPoint(cropbox.Max));


                //转变后cropbox角点2
                XYZ cropXyz_min_trans2 = tsf2.Inverse.OfPoint(cropXyz_min2);
                XYZ cropXyz_max_trans2 = tsf2.Inverse.OfPoint(cropXyz_max2);
                double[] xnum2 = new double[] { cropXyz_min_trans2.X, cropXyz_max_trans2.X };
                double[] ynum2 = new double[] { cropXyz_min_trans2.Y, cropXyz_max_trans2.Y };
                double[] znum2 = new double[] { cropXyz_min_trans2.Z, cropXyz_max_trans2.Z };
                XYZ cropXyz_min_2 = new XYZ(xnum2.Min(), ynum2.Min(), znum2.Min());
                XYZ cropXyz_max_2 = new XYZ(xnum2.Max(), ynum2.Max(), znum2.Max());
                BoundingBoxXYZ cropbox2 = new BoundingBoxXYZ();
                cropbox2.Min = cropXyz_min_2;
                cropbox2.Max = cropXyz_max_2;
                cropbox2.Transform = tsf2;
                #region pickpoint 和 pickbox 在两个线程中进行 用来判断 输入点的先后位置
                //打算在两次点击pickpoint 的同时调用 pickbox
                //new Thread(()=> {
                //    PickedBox pb1 = uiDoc.Selection.PickBox(PickBoxStyle.Directional, "拾取一个框");
                //});
                #endregion
                //cropbox.Min = tsf1.OfPoint(cropXyz_min);
                //cropbox.Max = tsf1.OfPoint(cropXyz_max);
                if (pb.Max.Y > pb.Min.Y)
                {
                    vsec1 = ViewSection.CreateSection(activeDoc, viewFamilyTypes.First().Id, cropbox);
                }
                else
                {
                    vsec1 = ViewSection.CreateSection(activeDoc, viewFamilyTypes.First().Id, cropbox2);
                }
                #endregion
                //ts.Commit();
            }
            catch (Exception e)
            {
                MessageBox.Show("设置不正确，不能创建视图");
            }
            return vsec1;
        }

        /// <summary>
        /// 拾取框创建剖面
        /// </summary>
        /// <param name="seedView"></param>
        /// <param name="pb"></param>
        /// <param name="sectopEle"></param>
        /// <param name="secbottomEle"></param>
        /// <param name="bottomoffset"></param>
        /// <param name="topoffset"></param>
        /// <returns></returns>
        public static ViewSection CreateVerticalSection(this View seedView, PickedBox pb, double sectopEle, double secbottomEle, double bottomoffset, double topoffset)
        {
            ViewSection vsec1 = null;
            var activeDoc = seedView.Document;

            Transform tsf;
            tsf = activeDoc.ActiveView.CropBox.Transform;

            #region creat_new_section
            XYZ raw1, raw2;      //save picked 2 points
            double raw_z, raw_z1; //define raw_z and raw_z1 as bottom and top of new cropbox

            raw_z = secbottomEle + bottomoffset; //在剖面中 由于当前视图没有与之相关联的 GenLevel 所以在剖面中此句代码会出错 故用if来判断是否在平面中
            raw_z1 = sectopEle + topoffset;
            //获得视图类型id 
            IEnumerable<ViewFamilyType> viewFamilyTypes = from elem in new FilteredElementCollector(activeDoc).OfClass(typeof(ViewFamilyType))
                                                          let type = elem as ViewFamilyType
                                                          where type.ViewFamily == ViewFamily.Section
                                                          select type;
            //鼠标拾取平面框的 min max
            XYZ min = pb.Min;
            XYZ max = pb.Max;
            double[] xnum = new[] { min.X, max.X };
            double[] ynum = new[] { min.Y, max.Y };
            double[] znum = new[] { raw_z, raw_z1 };
            //TaskDialog.Show("min max", min.ToString() + Environment.NewLine +
            //                           max.ToString());
            //转变前cropBox角点值 由下向上看
            XYZ cropXyz_min = new XYZ(xnum.Min(), ynum.Min(), znum.Min()); //最小角点 
            XYZ cropXyz_max = new XYZ(xnum.Max(), ynum.Max(), znum.Max()); //最大角点
                                                                           //转变前cropbox角点值 由上向下看
            XYZ cropXyz_min2 = new XYZ(cropXyz_max.X, cropXyz_max.Y, cropXyz_min.Z);
            XYZ cropXyz_max2 = new XYZ(cropXyz_min.X, cropXyz_min.Y, cropXyz_max.Z);
            //TaskDialog.Show("cropxyz_min cropxyz_max", cropXyz_min.ToString() + Environment.NewLine +
            //                                            cropXyz_max.ToString());
            //获取拾取框左边和右边中点点坐标
            XYZ left_second = new XYZ(cropXyz_min.X, cropXyz_min.Y / 2 + cropXyz_max.Y / 2, cropXyz_min.Z);   //左边中点坐标
            XYZ right_second = new XYZ(cropXyz_max.X, cropXyz_min.Y / 2 + cropXyz_max.Y / 2, cropXyz_min.Z);   //右边中点坐标
                                                                                                               //获取拾取框右下角坐标
            XYZ cropXyz_min_1 = new XYZ(cropXyz_min.X, cropXyz_max.Y, znum.Min());   //端点 用于计算 沿线向量
                                                                                     //左边中点向下的单位向量
            XYZ alongdirection = (left_second - cropXyz_min).Normalize();
            //左边中点向上的单位向量
            XYZ alongdirection1 = (left_second - cropXyz_min_1).Normalize();
            //左边中间由左向右的单位向量
            XYZ viewdirection = (left_second - right_second).Normalize();
            //右边中间由右向左的单位向量
            XYZ viewdirection1 = (right_second - left_second).Normalize();
            //定义transform
            Transform tsf1 = Transform.Identity;
            tsf1.BasisX = alongdirection;
            tsf1.BasisY = XYZ.BasisZ;
            tsf1.BasisZ = alongdirection.CrossProduct(tsf1.BasisY);
            tsf1.Origin = left_second;
            //计算transform2
            //左上角点 
            XYZ cropXyz_max_1 = new XYZ(cropXyz_max.X, cropXyz_max.Y, znum.Min());
            Transform tsf2 = Transform.Identity;
            tsf2.BasisX = (right_second - cropXyz_max_1).Normalize();
            tsf2.BasisY = XYZ.BasisZ;
            tsf2.BasisZ = tsf2.BasisX.CrossProduct(XYZ.BasisZ);
            tsf2.Origin = right_second;
            /*据说cropbox的 左上角点为 cropbox 的max 右下角点 为cropbox的min  据此计算transform 在进行一次实验 验证此说法，以前一直以为左下是min 右上 是max*/
            //TaskDialog.Show("trans.basis x y z", "origin" + tsf1.Origin + Environment.NewLine +
            //                                     "x" + tsf1.BasisX.ToString() + Environment.NewLine +
            //                                     "y" + tsf1.BasisY.ToString() + Environment.NewLine +
            //                                     "z" + tsf1.BasisZ.ToString());
            //转变后cropBox角点1
            XYZ cropXyz_min_trans = tsf1.Inverse.OfPoint(cropXyz_min);
            XYZ corpXyz_max_trans = tsf1.Inverse.OfPoint(cropXyz_max);
            double[] xnum1 = new double[] { cropXyz_min_trans.X, corpXyz_max_trans.X };
            double[] ynum1 = new double[] { cropXyz_min_trans.Y, corpXyz_max_trans.Y };
            double[] znum1 = new double[] { cropXyz_min_trans.Z, corpXyz_max_trans.Z };
            XYZ cropXyz_min1 = new XYZ(xnum1.Min(), ynum1.Min(), znum1.Min());
            XYZ cropXyz_max1 = new XYZ(xnum1.Max(), ynum1.Max(), znum1.Max());//
            BoundingBoxXYZ cropbox = new BoundingBoxXYZ();
            cropbox.Min = cropXyz_min1;
            cropbox.Max = cropXyz_max1;
            cropbox.Transform = tsf1;
            //转变后cropbox角点2
            XYZ cropXyz_min_trans2 = tsf2.Inverse.OfPoint(cropXyz_min2);
            XYZ cropXyz_max_trans2 = tsf2.Inverse.OfPoint(cropXyz_max2);
            double[] xnum2 = new double[] { cropXyz_min_trans2.X, cropXyz_max_trans2.X };
            double[] ynum2 = new double[] { cropXyz_min_trans2.Y, cropXyz_max_trans2.Y };
            double[] znum2 = new double[] { cropXyz_min_trans2.Z, cropXyz_max_trans2.Z };
            XYZ cropXyz_min_2 = new XYZ(xnum2.Min(), ynum2.Min(), znum2.Min());
            XYZ cropXyz_max_2 = new XYZ(xnum2.Max(), ynum2.Max(), znum2.Max());
            BoundingBoxXYZ cropbox2 = new BoundingBoxXYZ();
            cropbox2.Min = cropXyz_min_2;
            cropbox2.Max = cropXyz_max_2;
            cropbox2.Transform = tsf2;


            if (pb.Max.X > pb.Min.X)
            {
                vsec1 = ViewSection.CreateSection(activeDoc, viewFamilyTypes.First().Id, cropbox);
            }
            else
            {
                vsec1 = ViewSection.CreateSection(activeDoc, viewFamilyTypes.First().Id, cropbox2);
            }
            #endregion

            return vsec1;
        }

        /// <summary>
        /// 在剖面上拾取框创建新剖面
        /// </summary>
        /// <param name="seedView"></param>
        /// <param name="pb"></param>
        /// <returns></returns>
        public static ViewSection CreateVerticalSectionInSecView(this View seedView, PickedBox pb)
        {
            ViewSection result = null;
            if (seedView.ViewType != ViewType.Section)
            {
                return result;
            }
            Document doc = seedView.Document;
            ViewFamilyType viewfamilytype = new FilteredElementCollector(doc)
                                            .OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>()
                                            .Where(m => m.ViewFamily == ViewFamily.Section)
                                            .First();

            //ViewFamilyType viewfamilytype = doc.TCollector<ViewFamilyType>().Where(m => m.ViewFamily == ViewFamily.Section).First();
            try
            {
                XYZ pbmin = pb.Min;
                XYZ pbmax = pb.Max;
                BoundingBoxXYZ oldbox = seedView.CropBox;
                Transform oldtransform = oldbox.Transform;
                //pb.min所在平面
#if revit2016
                Plane planOfPbMin = new Plane(oldtransform.BasisX, pb.Min);
#endif
#if revit2019
                Plane planOfPbMin = Plane.CreateByNormalAndOrigin(oldtransform.BasisX, pb.Min);
#endif

                XYZ pbminInOldTrans = oldtransform.Inverse.OfPoint(pbmin);
                XYZ pbmaxInOldTrans = oldtransform.Inverse.OfPoint(pbmax);
                //doc.NewLine_WithoutTransaction(pb.Min, pb.Max);
                double[] xnum = new double[] { pbmaxInOldTrans.X, pbminInOldTrans.X };
                double[] ynum = new double[] { pbmaxInOldTrans.Y, pbminInOldTrans.Y };
                double[] znum = new double[] { oldbox.Min.Z, oldbox.Max.Z };
                XYZ newMin = new XYZ(xnum.Min(), ynum.Min(), znum.Min());
                XYZ newMax = new XYZ(xnum.Max(), ynum.Max(), znum.Max());
                //BoundingBoxXYZ tembox = new BoundingBoxXYZ();
                //tembox.Transform = Transform.Identity;
                //tembox.Min = tembox.Transform.Inverse.OfPoint(newMin);
                //tembox.Max = tembox.Transform.Inverse.OfPoint(newMax);
                //doc.NewBox_WithOutTransaction(tembox);
                //MessageBox.Show(tembox.Min.ToString() + Environment.NewLine +
                //                tembox.Max.ToString());
                XYZ newtransOrigin = (oldtransform.OfPoint((newMin + newMax) / 2)).Project(planOfPbMin);
                //doc.NewLine_WithoutTransaction(newtransOrigin, newtransOrigin + XYZ.BasisX*3);
                //doc.NewLine_WithoutTransaction(newtransOrigin, newtransOrigin + XYZ.BasisY*3);
                //doc.NewLine_WithoutTransaction(newtransOrigin, newtransOrigin + XYZ.BasisZ*3);
                Transform newtransform = Transform.Identity;
                newtransform.Origin = newtransOrigin;
                newtransform.BasisX = pbmaxInOldTrans.X > pbminInOldTrans.X ? -oldtransform.BasisZ : oldtransform.BasisZ;
                newtransform.BasisY = oldtransform.BasisY;
                newtransform.BasisZ = newtransform.BasisX.CrossProduct(newtransform.BasisY);
                newMin = oldtransform.OfPoint(newMin);
                newMax = oldtransform.OfPoint(newMax);
                XYZ MinInbox = newtransform.Inverse.OfPoint(newMin);
                XYZ MaxInbox = newtransform.Inverse.OfPoint(newMax);
                double[] newxnum = new double[] { MinInbox.X, MaxInbox.X };
                double[] newynum = new double[] { MinInbox.Y, MaxInbox.Y };
                double[] newznum = new double[] { MinInbox.Z, MaxInbox.Z };
                MinInbox = new XYZ(newxnum.Min(), newynum.Min(), newznum.Min());
                MaxInbox = new XYZ(newxnum.Max(), newynum.Max(), newznum.Max());
                BoundingBoxXYZ newbox = new BoundingBoxXYZ();
                newbox.Transform = newtransform;
                newbox.Min = MinInbox;
                newbox.Max = MaxInbox;
                //doc.NewBox_WithOutTransaction(newbox);
                //doc.NewLine_WithoutTransaction(newtransOrigin, newtransOrigin + newtransform.BasisX * 3);
                //doc.NewLine_WithoutTransaction(newtransOrigin, newtransOrigin + newtransform.BasisY * 2);
                //doc.NewLine_WithoutTransaction(newtransOrigin, newtransOrigin + newtransform.BasisZ * 1);
                //MessageBox.Show(newtransform.BasisZ.ToString());
                result = ViewSection.CreateSection(doc, viewfamilytype.Id, newbox);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return result;
            }
            return result;
        }

        /// <summary>
        /// 剖面转3D视图
        /// </summary>
        /// <param name="seedView"></param>
        /// <returns></returns>
        public static View3D ViewSectionTo3D(this View seedView)
        {
            View3D result = null;
            var doc = seedView.Document;
            var vft = doc.TCollector<ViewFamilyType>().Where(m => m.ViewFamily == ViewFamily.ThreeDimensional).First();

            if (!(seedView is ViewSection)) return null;
            var box = seedView.CropBox;
            var oldmin = box.Min;
            var oldmax = box.Max;

            Transform ts1 = box.Transform;

            Transform ts2 = Transform.Identity;
            ts2.Origin = ts1.Origin;
            ts2.BasisX = ts1.BasisX;
            ts2.BasisZ = XYZ.BasisZ;
            ts2.BasisY = XYZ.BasisZ.CrossProduct(ts2.BasisX);

            var oldminInWorld = ts1.OfPoint(oldmin);
            var oldmaxInWorld = ts1.OfPoint(oldmax);

            var oldminInts2 = ts2.Inverse.OfPoint(oldminInWorld);
            var oldmaxInts2 = ts2.Inverse.OfPoint(oldmaxInWorld);

            var xnum = new double[] { oldminInts2.X, oldmaxInts2.X };
            var ynum = new double[] { oldminInts2.Y, oldmaxInts2.Y };
            var znum = new double[] { oldminInts2.Z, oldmaxInts2.Z };

            var newminInts2 = new XYZ(xnum.Min(), ynum.Min(), znum.Min());
            var newmaxInts2 = new XYZ(xnum.Max(), ynum.Max(), znum.Max());

            var newbox = new BoundingBoxXYZ();
            newbox.Transform = ts2;
            newbox.Min = newminInts2;
            newbox.Max = newmaxInts2;

            result = View3D.CreateIsometric(doc, vft.Id);
            result.SetSectionBox(newbox);

            return result;
        }

        /// <summary>
        /// 重置视图范围 包含平面 剖面和 三维
        /// </summary>
        /// <param name="seedView"></param>
        public static void ResetViewbox(this View seedView, PickedBox pb)
        {
            if (seedView is ViewPlan)
            {
                //平面图
                var oldbox = seedView.CropBox;
                double[] xnums = { pb.Min.X, pb.Max.X };
                double[] ynums = new double[] { pb.Min.Y, pb.Max.Y };
                double[] znums = new double[] { oldbox.Min.Z, oldbox.Max.Z };

                var newmin = new XYZ(xnums.Min(), ynums.Min(), znums.Min());
                var newmax = new XYZ(xnums.Max(), ynums.Max(), znums.Max());

                var newbox = new BoundingBoxXYZ();
                newbox.Transform = oldbox.Transform;
                newbox.Min = newmin;
                newbox.Max = newmax;

                seedView.CropBox = newbox;
            }
            else if (seedView is ViewSection)
            {
                var view = seedView as ViewSection;
                //剖面图
                var oldbox = seedView.CropBox;
                var oldtransform = oldbox.Transform;

                var pbMin = pb.Min;
                var pbMax = pb.Max;

                var pbMinInTrans = oldtransform.Inverse.OfPoint(pbMin);
                var pbMaxInTrans = oldtransform.Inverse.OfPoint(pbMax);

                var xnums = new double[] { pbMinInTrans.X, pbMaxInTrans.X };
                var ynums = new double[] { pbMinInTrans.Y, pbMaxInTrans.Y };
                var znums = new double[] { oldbox.Min.Z, oldbox.Max.Z };

                var newmin = new XYZ(xnums.Min(), ynums.Min(), 0d);
                var newmax = new XYZ(xnums.Max(), ynums.Max(), 0d);

                var newbox = new BoundingBoxXYZ();
                newbox.Transform = oldtransform;
                newbox.Min = newmin;
                newbox.Max = newmax;

                seedView.CropBox = newbox;

                Parameter parameter = seedView.get_Parameter((BuiltInParameter)(-1005104));
                parameter.Set(znums.Max() - znums.Min());

            }
            else if (seedView is View3D)
            {
                //三维视图
                Reset3dViewbox((View3D)seedView, pb);
            }
        }

        /// <summary>
        /// 重置三维视图剖面框
        /// </summary>
        /// <param name="_3dView"></param>
        /// <param name="pb"></param>
        static void Reset3dViewbox(this View3D _3dView, PickedBox pb)
        {
            var dir = _3dView.ViewDirection;
            var cropbox = _3dView.GetSectionBox();
            var newbox = ResetBox1(dir, pb, cropbox);
            _3dView.SetSectionBox(newbox);
            BoundingBoxXYZ ResetBox1(XYZ Viewdir, PickedBox px, BoundingBoxXYZ box)
            {
                Transform transform = box.Transform;
                XYZ min = box.Min;
                XYZ max = box.Max;
                XYZ boxMin = transform.OfPoint(min);
                XYZ boxMax = transform.OfPoint(max);
                XYZ pxMin = px.Min;
                XYZ pxMax = px.Max;
                double[] xnums;
                double[] ynums;
                double[] znums;
                BoundingBoxXYZ result;
                if (Viewdir.IsParallel(XYZ.BasisX))
                {
                    xnums = new double[]
                    {
                    boxMin.X,
                    boxMax.X
                    };
                    ynums = new double[]
                    {
                    pxMin.Y,
                    pxMax.Y
                    };
                    znums = new double[]
                    {
                    pxMin.Z,
                    pxMax.Z
                    };
                }
                else if (Viewdir.IsParallel(XYZ.BasisY))
                {
                    xnums = new double[]
                    {
                    pxMin.X,
                    pxMax.X
                    };
                    ynums = new double[]
                    {
                    boxMin.Y,
                    boxMax.Y
                    };
                    znums = new double[]
                    {
                    pxMin.Z,
                    pxMax.Z
                    };
                }
                else
                {
                    if (!Viewdir.IsParallel(XYZ.BasisZ))
                    {
                        result = null;
                        return result;
                    }
                    xnums = new double[]
                    {
                    pxMin.X,
                    pxMax.X
                    };
                    ynums = new double[]
                    {
                    pxMin.Y,
                    pxMax.Y
                    };
                    znums = new double[]
                    {
                    boxMin.Z,
                    boxMax.Z
                    };
                }
                XYZ minInWorld = new XYZ(xnums.Min(), ynums.Min(), znums.Min());
                XYZ maxInWorld = new XYZ(xnums.Max(), ynums.Max(), znums.Max());
                XYZ minInTrans = transform.Inverse.OfPoint(minInWorld);
                XYZ maxInTrans = transform.Inverse.OfPoint(maxInWorld);

                //XYZ minInTrans = transform.OfPoint(minInWorld);
                //XYZ maxInTrans = transform.OfPoint(maxInWorld);

                double[] newXnums = new double[]
                {
                minInTrans.X,
                maxInTrans.X
                };
                double[] newYnums = new double[]
                {
                minInTrans.Y,
                maxInTrans.Y
                };
                double[] newZnums = new double[]
                {
                minInTrans.Z,
                maxInTrans.Z
                };

                minInTrans = new XYZ(newXnums.Min(), newYnums.Min(), newZnums.Min());
                maxInTrans = new XYZ(newXnums.Max(), newYnums.Max(), newZnums.Max());
                BoundingBoxXYZ boundingBoxXYZ = new BoundingBoxXYZ();
                boundingBoxXYZ.Transform = box.Transform;
                boundingBoxXYZ.Min = minInTrans;
                boundingBoxXYZ.Max = maxInTrans;
                result = boundingBoxXYZ;
                return result;
            }
        }

        /// <summary>
        /// 平面转3D
        /// </summary>
        /// <param name="seedView"></param>
        /// <param name="pb"></param>
        /// <param name="minHeight"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static View Create3DViewFromPlane(this View seedView, PickedBox pb, double? minHeight = null, double? maxHeight = null)
        {
            if (!(seedView is ViewPlan)) return null;
            var doc = seedView.Document;
            var viewfmailytype = doc.TCollector<ViewFamilyType>().First(m => m.ViewFamily == ViewFamily.ThreeDimensional);

            var level = seedView.GenLevel;
            var levelEle = level.Elevation; //当前楼层标高
            var levels = doc.TCollector<Level>().Where(m => m.Elevation > levelEle);
            var levelup = levels.Count() > 0 ? levels.OrderBy(m => m.Elevation)?.First() : null;

            var height = 4000d.MetricToFeet();

            var bottomHeight = 0d;
            var topHeight = 0d;

            bottomHeight = minHeight ?? levelEle;

            //首先取设置顶标高 maxHeight 如果没有就取上层标高，如果上层标高为空，则取当前楼层标高+4000mm
            topHeight = maxHeight ?? (levelup?.Elevation ?? levelEle + height);

            var cropbox = seedView.CropBox;
            var transform = cropbox.Transform;
            var xnums = new double[] { pb.Min.X, pb.Max.X };
            var ynums = new double[] { pb.Min.Y, pb.Max.Y };
            var znums = new double[] { topHeight, bottomHeight };

            var newmin = new XYZ(xnums.Min(), ynums.Min(), znums.Min());
            var newmax = new XYZ(xnums.Max(), ynums.Max(), znums.Max());

            newmin = transform.Inverse.OfPoint(newmin);
            newmax = transform.Inverse.OfPoint(newmax);

            cropbox.Min = newmin;
            cropbox.Max = newmax;

            var view = View3D.CreateIsometric(doc, viewfmailytype.Id);
            view.SetSectionBox(cropbox);
            return view;
        }
    }
}
