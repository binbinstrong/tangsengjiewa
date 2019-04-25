//======================================
//Copyright              2017
//CLR版本:               4.0.30319.42000
//机器名称:              XU-PC
//命名空间名称/文件名:   Techyard.Revit.Database/Class1
//创建人:                XU ZHAO BIN
//创建时间:              2017/12/10 22:31:43
//网址:                   
//======================================
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


namespace BinLibrary.RevitExtension
{
    public static class CreationHelper
    {
        ///// <summary>
        ///// 新建模型线 测试用
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="pStart"></param>
        ///// <param name="pEnd"></param>
        //public static void NewLine(this Document doc, XYZ pStart, XYZ pEnd)
        //{
        //    if (pStart.IsAlmostEqualTo(pEnd))
        //    {
        //        return;
        //    }
        //    if (doc.IsFamilyDocument)
        //    {
        //        using (Transaction tr = new Transaction(doc, Guid.NewGuid().ToString()))
        //        {
        //            try
        //            {
        //                tr.Start();
        //                Line line = Line.CreateBound(pStart, pEnd);
        //                double angle = line.Direction.AngleTo(XYZ.BasisX);
        //                XYZ norm = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
        //                if (angle - 0.0 < 1e-6)
        //                {
        //                    angle = line.Direction.AngleTo(XYZ.BasisY);
        //                    norm = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
        //                }
        //                if (angle - 0.0 < 1e-6)
        //                {
        //                    angle = line.Direction.AngleTo(XYZ.BasisZ);
        //                    norm = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
        //                }
        //                Plane plane = doc.Application.Create.NewPlane(norm, line.Origin);
                         
        //                SketchPlane skplane = SketchPlane.Create(doc, plane);
        //                ModelCurve newLine = doc.FamilyCreate.NewModelCurve(line, skplane);
        //                tr.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                tr.RollBack();
        //            }
        //        }
        //    }
        //    else
        //    using (Transaction tr = new Transaction(doc, Guid.NewGuid().ToString()))
        //    {
        //        try
        //        {
        //            tr.Start();
        //            Line line = Line.CreateBound(pStart, pEnd);
        //            double angle = line.Direction.AngleTo(XYZ.BasisX);
        //            XYZ norm = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
        //            if (angle - 0.0 < 1e-6)
        //            {
        //                angle = line.Direction.AngleTo(XYZ.BasisY);
        //                norm = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
        //            }
        //            if (angle - 0.0 < 1e-6)
        //            {
        //                angle = line.Direction.AngleTo(XYZ.BasisZ);
        //                norm = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
        //            }
        //            Plane plane = doc.Application.Create.NewPlane(norm, line.Origin);
        //            SketchPlane skplane = SketchPlane.Create(doc, plane);
        //            ModelCurve newLine = doc.Create.NewModelCurve(line, skplane);
        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.RollBack();
        //        }
        //    }
        //}
        ///// <summary>
        ///// 根据revitCurve 端点创建一条模型线
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="c"></param>
        //public static void NewLine(this Document doc, Curve c)
        //{
        //    if (c.GetEndPoint(0).IsAlmostEqualTo(c.GetEndPoint(1)))
        //    {
        //        return;
        //    }
        //    using (Transaction tr = new Transaction(doc, Guid.NewGuid().ToString()))
        //    {
        //        try
        //        {
        //            tr.Start();
        //            Line line = Line.CreateBound(c.GetEndPoint(0), c.GetEndPoint(1));
        //            double angle = line.Direction.AngleTo(XYZ.BasisX);
        //            XYZ norm = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
        //            if (angle - 0.0 < 1e-6)
        //            {
        //                angle = line.Direction.AngleTo(XYZ.BasisY);
        //                norm = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
        //            }
        //            if (angle - 0.0 < 1e-6)
        //            {
        //                angle = line.Direction.AngleTo(XYZ.BasisZ);
        //                norm = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
        //            }
        //            Plane plane = doc.Application.Create.NewPlane(norm, line.Origin);
        //            SketchPlane skplane = SketchPlane.Create(doc, plane);
        //            ModelCurve newLine = doc.Create.NewModelCurve(line, skplane);
        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.RollBack();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 创建曲线
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="c"></param>
        //public static void NewCurve(this Document doc, Curve c)
        //{
        //    //doc.Create.
            
        //}

        ///// <summary>
        ///// 根据点和transform用模型线绘制坐标系
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="point"></param>
        ///// <param name="ts"></param>
        //public static void NewModelLineXYZ(this Document doc, XYZ point,Transform ts = null/*默认为世界坐标系的transform*/)
        //{
        //    Transform tsinner = Transform.Identity;
        //    if (ts==null)
        //    {
                
        //    }
        //    else
        //    {
        //        tsinner = ts;
        //    }
        //    doc.NewLine(point, point + tsinner.BasisX * 2);
        //    doc.NewLine(point, point + tsinner.BasisY * 2);
        //    doc.NewLine(point, point + tsinner.BasisZ * 2);
        //}
         
        ///// <summary>
        ///// 带返回值的创建
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="c"></param>
        ///// <returns></returns>
        //public static Element NewLine_R(this Document doc, Curve c)
        //{

        //    ModelCurve newLine = null;
        //    if (c.GetEndPoint(0).IsAlmostEqualTo(c.GetEndPoint(1)))
        //    {
        //        return null;
        //    }
        //    using (Transaction tr = new Transaction(doc, Guid.NewGuid().ToString()))
        //    {
        //        try
        //        {
        //            tr.Start();
        //            Line line = Line.CreateBound(c.GetEndPoint(0), c.GetEndPoint(1));
        //            double angle = line.Direction.AngleTo(XYZ.BasisX);
        //            XYZ norm = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
        //            if (angle - 0.0 < 1e-6)
        //            {
        //                angle = line.Direction.AngleTo(XYZ.BasisY);
        //                norm = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
        //            }
        //            if (angle - 0.0 < 1e-6)
        //            {
        //                angle = line.Direction.AngleTo(XYZ.BasisZ);
        //                norm = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
        //            }
        //            Plane plane = doc.Application.Create.NewPlane(norm, line.Origin);
        //            SketchPlane skplane = SketchPlane.Create(doc, plane);
        //            newLine = doc.Create.NewModelCurve(line, skplane);
        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.RollBack();
        //        }
        //    }

        //    return newLine;
        //}

        ///// <summary>
        ///// 创建模型文字 只能通过复制已有的文字来实现
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="str"></param>
        ///// <returns></returns>
        ////public static Element NewModelText(this Document doc, Transform tsf, string str)
        ////{
        ////    UIDocument uidoc = new UIDocument(doc);
        ////    Selection sel = uidoc.Selection;
        ////    XYZ pickpoint = sel.PickPoint();

        ////    #region temTest

        ////    str = "abcd";

        ////    #endregion

        ////    Element result = null;

        ////    string path = "c:/modeltext_left.rvt"; //带有模型文字的文件路径

        ////    Document tarDoc = (new UIDocument(doc)).Application.Application.OpenDocumentFile(path);

        ////    FilteredElementCollector collector = new FilteredElementCollector(tarDoc);

        ////    ICollection<ElementId> modelText = collector.OfClass(typeof(ModelText)).ToElementIds();

        ////    //创建模型文字

        ////    //MessageBox.Show(modelText.ToString());


        ////    Transaction ts = new Transaction(doc, " sss");

        ////    ts.Start();

        ////    SketchPlane sk = SketchPlane.Create(doc, new Plane(XYZ.BasisZ, pickpoint));

        ////    Element e = ElementTransformUtils.CopyElements(tarDoc, modelText, doc, tsf, null).PhysicalConToList()[0].GetElement(doc);
        ////    result = e;

        ////    e.LookupParameter("文字").Set(str);
        ////    e.LookupParameter("深度").Set(20d.MetricToFeet());

        ////    //MessageBox.Show("succeed" + Environment.NewLine +
        ////    //                e.LookupParameter("深度").AsValueString());

        ////    ts.Commit();

        ////    tarDoc.Close(false);

        ////    return result;
        ////}

        ///// <summary>
        ///// 创建文字 不带事务（未测试）
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="str"></param>
        ///// <param name="origin"></param>
        ///// <param name="baseVec"></param>
        ///// <param name="upVec"></param>
        ///// <param name="lineWidth"></param>
        ///// <param name="align"></param>
        ///// <returns></returns>
        //public static Element NewTextNode(this Document doc, string str, XYZ origin, XYZ baseVec, XYZ upVec, double lineWidth = 1, TextAlignFlags align = TextAlignFlags.TEF_ALIGN_BOTTOM | TextAlignFlags.TEF_ALIGN_LEFT)
        //{
        //    Element result = null;
        //    View acview = doc.ActiveView;
        //    //result = doc.Create.NewTextNote(acview, origin, baseVec, upVec, lineWidth, align, str);
        //    //result = TextNote.Create(acview, origin, baseVec, upVec, lineWidth, align, str);
        //    result = TextNote.Create(doc,acview.Id, origin , lineWidth, str,new TextNoteOptions());
        //    return result;
        //}
         
        //#region 不带事务 的 新建模型线
        //public static void NewLine_WithoutTransaction(this Document doc, XYZ pStart, XYZ pEnd)
        //{
        //    if (pStart.IsAlmostEqualTo(pEnd))
        //    {
        //        return;
        //    }


        //    Line line = Line.CreateBound(pStart, pEnd);
        //    double angle = line.Direction.AngleTo(XYZ.BasisX);
        //    XYZ norm = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
        //    if (angle - 0.0 < 1e-6)
        //    {
        //        angle = line.Direction.AngleTo(XYZ.BasisY);
        //        norm = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
        //    }
        //    if (angle - 0.0 < 1e-6)
        //    {
        //        angle = line.Direction.AngleTo(XYZ.BasisZ);
        //        norm = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
        //    }
        //    Plane plane = doc.Application.Create.NewPlane(norm, line.Origin);
        //    SketchPlane skplane = SketchPlane.Create(doc, plane);
        //    ModelCurve newLine = doc.Create.NewModelCurve(line, skplane);


        //}
        //public static void NewLine_WithoutTransaction(this Document doc, Curve c)
        //{
        //    if (c.GetEndPoint(0).IsAlmostEqualTo(c.GetEndPoint(1)))
        //    {
        //        return;
        //    }


        //    Line line = Line.CreateBound(c.GetEndPoint(0), c.GetEndPoint(1));
        //    double angle = line.Direction.AngleTo(XYZ.BasisX);
        //    XYZ norm = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
        //    if (angle - 0.0 < 1e-6)
        //    {
        //        angle = line.Direction.AngleTo(XYZ.BasisY);
        //        norm = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
        //    }
        //    if (angle - 0.0 < 1e-6)
        //    {
        //        angle = line.Direction.AngleTo(XYZ.BasisZ);
        //        norm = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
        //    }
        //    Plane plane = doc.Application.Create.NewPlane(norm, line.Origin);
        //    SketchPlane skplane = SketchPlane.Create(doc, plane);
        //    ModelCurve newLine = doc.Create.NewModelCurve(line, skplane);


        //}
        //public static Element NewLine_WithoutTransaction_R(this Document doc, Curve c)
        //{
        //    if (c.GetEndPoint(0).IsAlmostEqualTo(c.GetEndPoint(1)))
        //    {
        //        return null;
        //    }


        //    Line line = Line.CreateBound(c.GetEndPoint(0), c.GetEndPoint(1));
        //    double angle = line.Direction.AngleTo(XYZ.BasisX);
        //    XYZ norm = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
        //    if (angle - 0.0 < 1e-6)
        //    {
        //        angle = line.Direction.AngleTo(XYZ.BasisY);
        //        norm = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
        //    }
        //    if (angle - 0.0 < 1e-6)
        //    {
        //        angle = line.Direction.AngleTo(XYZ.BasisZ);
        //        norm = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
        //    }
        //    Plane plane = doc.Application.Create.NewPlane(norm, line.Origin);
        //    SketchPlane skplane = SketchPlane.Create(doc, plane);
        //    ModelCurve newLine = doc.Create.NewModelCurve(line, skplane);

        //    return newLine;
        //}
        //#endregion
          
        ///// <summary>
        ///// 新建模型线构成的多边形 测试用
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="points"></param>
        //public static void NewPoligon(this Document doc, List<XYZ> points)
        //{
        //    if (points.Count >= 2)
        //    {
        //        for (int i = 0; i < points.Count; i++)
        //        {
        //            doc.NewLine(points[i], points[i + 1]);
        //        }
        //    }
        //}
        ///// <summary>
        ///// 根据面创建面的边框模型线
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="face"></param>
        //public static void NewPoligon(this Document doc, Face face)
        //{
        //    EdgeArrayArray edges = face.EdgeLoops;
        //    EdgeArray edge = null;
        //    face.GetEdgesAsCurveLoops();
        //    foreach (CurveLoop c in face.GetEdgesAsCurveLoops())
        //    {
        //        foreach (Curve c1 in c)
        //        {
        //            doc.NewLine(c1 as Line);

        //        }
        //    }
        //}

        //#region dimed
        /////// <summary>
        /////// 用模型线创建圆形 (根据椭圆创建） 待修改
        /////// </summary>
        /////// <param name="doc"></param>
        /////// <param name="ell"></param>
        ////public static void NewEllipse(this Document doc, XYZ center, XYZ normal, double radius)
        ////{

        ////    Line anxis = Line.CreateUnbound(center, normal);

        ////    using (Transaction tr = new Transaction(doc, Guid.NewGuid().ToString()))
        ////    {

        ////        //try
        ////        //{
        ////        tr.Start();

        ////        Transform trs = Transform.Identity;
        ////        //MessageBox.Show(anxis.DistanceTo(new XYZ(0, 0, 0)).ToString());

        ////        if (anxis.DistanceTo(new XYZ(0, 0, 0)) < 0.000001d)
        ////        {
        ////            trs.Origin = center;
        ////            trs.BasisZ = normal.Normalize();
        ////            trs.BasisX = (new XYZ(0, 0, 0) - anxis.Project(new XYZ(0, 0, 0)).XYZPoint).Normalize();
        ////            trs.BasisY = -(trs.BasisX.CrossProduct(trs.BasisZ)).Normalize();
        ////        }
        ////        else
        ////        {
        ////            trs.Origin = center;
        ////            trs.BasisZ = normal.Normalize();
        ////            trs.BasisX = (new XYZ(10, 10, 10) - anxis.Project(new XYZ(10, 10, 10)).XYZPoint).Normalize();
        ////            trs.BasisY = -trs.BasisX.CrossProduct(trs.BasisZ).Normalize();
        ////        }

        ////        //MessageBox.Show(trs.BasisY.ToString());

        ////        Ellipse ell = Ellipse.Create(center, radius, radius, trs.BasisX, trs.BasisY, 0.0, 2 * Math.PI); //最后两个参数是指起始角度


        ////        Curve c = ell;

        ////        SketchPlane sk = SketchPlane.Create(doc, new Plane(trs.BasisZ, trs.Origin));

        ////        doc.Create.NewModelCurve(c, sk);


        ////        tr.Commit();
        ////        //}
        ////        //catch (Exception e)
        ////        //{
        ////        //    MessageBox.Show(e.InnerException.ToString());
        ////        //    tr.RollBack();
        ////        //}

        ////    }
        ////}

        /////// <summary>
        /////// 用模型线创建圆形
        /////// </summary>
        /////// <param name="doc"></param>
        /////// <param name="center"></param>
        /////// <param name="normal"></param>
        /////// <param name="radius"></param>
        ////public static void NewCircle(this Document doc, XYZ center, XYZ normal, double radius = 3)
        ////{
        ////    Arc a = Arc.Create(new Plane(normal, center), radius, 0, 2 * Math.PI);

        ////    using (Transaction tr = new Transaction(doc, Guid.NewGuid().ToString()))
        ////    {
        ////        tr.Start();

        ////        SketchPlane sk = SketchPlane.Create(doc, new Plane(normal, center));

        ////        doc.Create.NewModelCurve(a, sk);

        ////        tr.Commit();

        ////    }


        ////}

        ////public static void NewCircle_withoutTransaction(this Document doc, XYZ center, XYZ normal, double radius = 3)
        ////{
        ////    Arc a = Arc.Create(new Plane(normal, center), radius, 0, 2 * Math.PI);

        ////    SketchPlane sk = SketchPlane.Create(doc, new Plane(normal, center));

        ////    doc.Create.NewModelCurve(a, sk);


        ////}

        ////#region 创建几何实体  相关类GeometryCreationUtilities.CreateExtrusionGeometry

        /////// <summary>
        /////// 创建拉伸
        /////// </summary>
        /////// <param name="doc"></param>
        /////// <param name="loopsList"></param>
        /////// <param name="direction"></param>
        /////// <param name="distance"></param>
        /////// <returns></returns>
        ////public static Solid NewExtrusion(this Document doc, IList<CurveLoop> loopsList, XYZ direction, double distance)
        ////{
        ////    return GeometryCreationUtilities.CreateExtrusionGeometry(loopsList, direction, distance);
        ////}


        ////#endregion

        //public static void NewBox(this Document doc, BoundingBoxXYZ box)
        //{
        //    Transform ts = box.Transform;

        //    XYZ min_o = box.Min;
        //    XYZ max_o = box.Max;

        //    //将上两点转换到世界坐标系 WCS/UCS
        //    XYZ min = ts.OfPoint(min_o);
        //    XYZ max = ts.OfPoint(max_o);


        //    Plane p1 = new Plane(ts.BasisX, min);    //构建6个平面 
        //    Plane p2 = new Plane(ts.BasisY, min);    //构建6个平面 
        //    Plane p3 = new Plane(ts.BasisZ, min);    //构建6个平面 

        //    Plane p4 = new Plane(ts.BasisX, max);    //构建6个平面 
        //    Plane p5 = new Plane(ts.BasisY, max);    //构建6个平面 
        //    Plane p6 = new Plane(ts.BasisZ, max);    //构建6个平面 


        //    XYZ min_x = min.Project(p4);    //最大最小点投影到另外一个点所在的xyz平面
        //    XYZ min_y = min.Project(p5);    //最大最小点投影到另外一个点所在的xyz平面
        //    XYZ min_z = max.Project(p3);    //最大最小点投影到另外一个点所在的xyz平面

        //    XYZ max_x = max.Project(p1);    //最大最小点投影到另外一个点所在的xyz平面
        //    XYZ max_y = max.Project(p2);    //最大最小点投影到另外一个点所在的xyz平面
        //    XYZ max_z = min.Project(p6);    //最大最小点投影到另外一个点所在的xyz平面

        //    doc.NewLine(min, min_x);         //下平面多边形
        //    doc.NewLine(min, min_y);         //下平面多边形
        //    doc.NewLine(min_x, min_z);       //下平面多边形
        //    doc.NewLine(min_y, min_z);       //下平面多边形

        //    doc.NewLine(max, max_x);       //上平面多边形
        //    doc.NewLine(max, max_y);       //上平面多边形
        //    doc.NewLine(max_x, max_z);       //上平面多边形
        //    doc.NewLine(max_y, max_z);       //上平面多边形

        //    doc.NewLine(min, max_z);          //平面中间连线
        //    doc.NewLine(min_x, max_y);       //平面中间连线
        //    doc.NewLine(min_y, max_x);        //平面中间连线
        //    doc.NewLine(min_z, max);          //平面中间连线


        //}

        //public static void NewBox_WithOutTransaction(this Document doc, BoundingBoxXYZ box)
        //{
        //    Transform ts = box.Transform;

        //    XYZ min_o = box.Min;
        //    XYZ max_o = box.Max;

        //    //将上两点转换到世界坐标系 WCS/UCS
        //    XYZ min = ts.OfPoint(min_o);
        //    XYZ max = ts.OfPoint(max_o);


        //    Plane p1 = new Plane(ts.BasisX, min);    //构建6个平面 
        //    Plane p2 = new Plane(ts.BasisY, min);    //构建6个平面 
        //    Plane p3 = new Plane(ts.BasisZ, min);    //构建6个平面 

        //    Plane p4 = new Plane(ts.BasisX, max);    //构建6个平面 
        //    Plane p5 = new Plane(ts.BasisY, max);    //构建6个平面 
        //    Plane p6 = new Plane(ts.BasisZ, max);    //构建6个平面 


        //    XYZ min_x = min.Project(p4);    //最大最小点投影到另外一个点所在的xyz平面
        //    XYZ min_y = min.Project(p5);    //最大最小点投影到另外一个点所在的xyz平面
        //    XYZ min_z = max.Project(p3);    //最大最小点投影到另外一个点所在的xyz平面

        //    XYZ max_x = max.Project(p1);    //最大最小点投影到另外一个点所在的xyz平面
        //    XYZ max_y = max.Project(p2);    //最大最小点投影到另外一个点所在的xyz平面
        //    XYZ max_z = min.Project(p6);    //最大最小点投影到另外一个点所在的xyz平面

        //    doc.NewLine_WithoutTransaction(min, min_x);         //下平面多边形
        //    doc.NewLine_WithoutTransaction(min, min_y);         //下平面多边形
        //    doc.NewLine_WithoutTransaction(min_x, min_z);       //下平面多边形
        //    doc.NewLine_WithoutTransaction(min_y, min_z);       //下平面多边形

        //    doc.NewLine_WithoutTransaction(max, max_x);       //上平面多边形
        //    doc.NewLine_WithoutTransaction(max, max_y);       //上平面多边形
        //    doc.NewLine_WithoutTransaction(max_x, max_z);       //上平面多边形
        //    doc.NewLine_WithoutTransaction(max_y, max_z);       //上平面多边形

        //    doc.NewLine_WithoutTransaction(min, max_z);          //平面中间连线
        //    doc.NewLine_WithoutTransaction(min_x, max_y);       //平面中间连线
        //    doc.NewLine_WithoutTransaction(min_y, max_x);        //平面中间连线
        //    doc.NewLine_WithoutTransaction(min_z, max);          //平面中间连线


        //}
         
        //#endregion
    }
}
