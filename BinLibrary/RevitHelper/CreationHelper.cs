using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Windows;
using BinLibrary.RevitExtension;

namespace BinLibrary.RevitHelper
{
	public static class CreationHelper
	{
        public static void NewLine(this Document doc, XYZ pStart, XYZ pEnd)
        {
            bool flag = pStart.IsAlmostEqualTo(pEnd);
            if (!flag)
            {
                bool isFamilyDocument = doc.IsFamilyDocument;
                if (isFamilyDocument)
                {
                    using (Transaction transaction = new Transaction(doc, Guid.NewGuid().ToString()))
                    {
                        try
                        {
                            transaction.Start();
                            Line line = Line.CreateBound(pStart, pEnd);
                            double num = line.Direction.AngleTo(XYZ.BasisX);
                            XYZ xYZ = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
                            bool flag2 = num - 0.0 < 1E-06;
                            if (flag2)
                            {
                                num = line.Direction.AngleTo(XYZ.BasisY);
                                xYZ = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
                            }
                            bool flag3 = num - 0.0 < 1E-06;
                            if (flag3)
                            {
                                num = line.Direction.AngleTo(XYZ.BasisZ);
                                xYZ = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
                            }
#if revit2016
                            Plane plane = doc.Application.Create.NewPlane(xYZ, line.Origin);
#endif
#if revit2019
                            Plane plane = Plane.CreateByNormalAndOrigin(xYZ, line.Origin);
#endif

                            SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
                            ModelCurve modelCurve = doc.FamilyCreate.NewModelCurve(line, sketchPlane);
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());

                            if (transaction.GetStatus() == TransactionStatus.Started)
                                transaction.RollBack();
                        }
                    }
                }
                else
                {
                    using (Transaction transaction2 = new Transaction(doc, Guid.NewGuid().ToString()))
                    {
                        try
                        {
                            transaction2.Start();
                            Line line2 = Line.CreateBound(pStart, pEnd);
                            double num2 = line2.Direction.AngleTo(XYZ.BasisX);
                            XYZ xYZ2 = line2.Direction.CrossProduct(XYZ.BasisX).Normalize();
                            bool flag4 = num2 - 0.0 < 1E-06;
                            if (flag4)
                            {
                                num2 = line2.Direction.AngleTo(XYZ.BasisY);
                                xYZ2 = line2.Direction.CrossProduct(XYZ.BasisY).Normalize();
                            }
                            bool flag5 = num2 - 0.0 < 1E-06;
                            if (flag5)
                            {
                                num2 = line2.Direction.AngleTo(XYZ.BasisZ);
                                xYZ2 = line2.Direction.CrossProduct(XYZ.BasisZ).Normalize();
                            }
#if revit2016
                            Plane plane2 = doc.Application.Create.NewPlane(xYZ2, line2.Origin);
#endif
#if revit2019
                            Plane plane2 = Plane.CreateByNormalAndOrigin(xYZ2, line2.Origin);
#endif
                            SketchPlane sketchPlane2 = SketchPlane.Create(doc, plane2);
                            ModelCurve modelCurve2 = doc.Create.NewModelCurve(line2, sketchPlane2);
                            transaction2.Commit();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());

                            if(transaction2.GetStatus()==TransactionStatus.Started)
                            transaction2.RollBack();
                        }
                    }
                }
            }
        }

        public static void NewLine(this Document doc, Curve c)
		{
			bool flag = c.GetEndPoint(0).IsAlmostEqualTo(c.GetEndPoint(1));
			if (!flag)
			{
				using (Transaction transaction = new Transaction(doc, Guid.NewGuid().ToString()))
				{
					try
					{
						transaction.Start();
						Line line = Line.CreateBound(c.GetEndPoint(0), c.GetEndPoint(1));
						double num = line.Direction.AngleTo(XYZ.BasisX);
						XYZ xYZ = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
						bool flag2 = num - 0.0 < 1E-06;
						if (flag2)
						{
							num = line.Direction.AngleTo(XYZ.BasisY);
							xYZ = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
						}
						bool flag3 = num - 0.0 < 1E-06;
						if (flag3)
						{
							num = line.Direction.AngleTo(XYZ.BasisZ);
							xYZ = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
						}
#if revit2016
                        Plane plane = doc.Application.Create.NewPlane(xYZ, line.Origin);
#endif
#if revit2019
					    Plane plane = Plane.CreateByNormalAndOrigin(xYZ, line.Origin);
#endif
                        SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
						ModelCurve modelCurve = doc.Create.NewModelCurve(line, sketchPlane);
						transaction.Commit();
					}
					catch (Exception var_11_14C)
					{
						transaction.RollBack();
					}
				}
			}
		}

		public static void NewCurve(this Document doc, Curve c)
		{
		}

		public static void NewModelLineXYZ(this Document doc, XYZ point, Transform ts = null)
		{
			Transform transform = Transform.Identity;
			bool flag = ts == null;
			if (!flag)
			{
				transform = ts;
			}
			doc.NewLine(point, point + transform.BasisX * 2.0);
			doc.NewLine(point, point + transform.BasisY * 2.0);
			doc.NewLine(point, point + transform.BasisZ * 2.0);
		}
	    public static void NewModelLineXYZ_withoutTransaction(this Document doc, XYZ point, Transform ts = null)
	    {
	        Transform transform = Transform.Identity;
	        bool flag = ts == null;
	        if (!flag)
	        {
	            transform = ts;
	        }
	        doc.NewLine_WithoutTransaction(point, point + transform.BasisX * 2.0);
	        doc.NewLine_WithoutTransaction(point, point + transform.BasisY * 2.0);
	        doc.NewLine_WithoutTransaction(point, point + transform.BasisZ * 2.0);
	    }

        public static Element NewLine_R(this Document doc, Curve c)
		{
			ModelCurve modelCurve = null;
			bool flag = c.GetEndPoint(0).IsAlmostEqualTo(c.GetEndPoint(1));
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				using (Transaction transaction = new Transaction(doc, Guid.NewGuid().ToString()))
				{
					try
					{
						transaction.Start();
						Line line = Line.CreateBound(c.GetEndPoint(0), c.GetEndPoint(1));
						double num = line.Direction.AngleTo(XYZ.BasisX);
						XYZ xYZ = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
						bool flag2 = num - 0.0 < 1E-06;
						if (flag2)
						{
							num = line.Direction.AngleTo(XYZ.BasisY);
							xYZ = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
						}
						bool flag3 = num - 0.0 < 1E-06;
						if (flag3)
						{
							num = line.Direction.AngleTo(XYZ.BasisZ);
							xYZ = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
						}
#if revit2016
					     Plane plane = doc.Application.Create.NewPlane(xYZ, line.Origin);
#endif
#if revit2019

					    Plane plane =Plane.CreateByNormalAndOrigin(xYZ, line.Origin);
#endif

                        SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
						modelCurve = doc.Create.NewModelCurve(line, sketchPlane);
						transaction.Commit();
					}
					catch (Exception var_12_159)
					{
						transaction.RollBack();
					}
				}
				result = modelCurve;
			}
			return result;
		}

		public static Element NewTextNode(this Document doc, string str, XYZ origin, XYZ baseVec, XYZ upVec, double lineWidth = 1.0, TextAlignFlags align = (TextAlignFlags)2112)
		{
			View activeView = doc.ActiveView;
			return TextNote.Create(doc, activeView.Id, origin, lineWidth, str, new TextNoteOptions());
		}

		public static void NewLine_WithoutTransaction(this Document doc, XYZ pStart, XYZ pEnd)
		{
			bool flag = pStart.IsAlmostEqualTo(pEnd);
			if (!flag)
			{
				Line line = Line.CreateBound(pStart, pEnd);
				double num = line.Direction.AngleTo(XYZ.BasisX);
				XYZ xYZ = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
				bool flag2 = num - 0.0 < 1E-06;
				if (flag2)
				{
					num = line.Direction.AngleTo(XYZ.BasisY);
					xYZ = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
				}
				bool flag3 = num - 0.0 < 1E-06;
				if (flag3)
				{
					num = line.Direction.AngleTo(XYZ.BasisZ);
					xYZ = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
				}
#if revit2016
                Plane plane = doc.Application.Create.NewPlane(xYZ, line.Origin);
#endif
#if revit2019
			    Plane plane = Plane.CreateByNormalAndOrigin(xYZ, line.Origin);
#endif
                SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
				ModelCurve modelCurve = doc.Create.NewModelCurve(line, sketchPlane);
			}
		}

		public static void NewLine_WithoutTransaction(this Document doc, Curve c)
		{
			bool flag = c.GetEndPoint(0).IsAlmostEqualTo(c.GetEndPoint(1));
		    if (!flag)
		    {
		        Line line = Line.CreateBound(c.GetEndPoint(0), c.GetEndPoint(1));
		        double num = line.Direction.AngleTo(XYZ.BasisX);
		        XYZ xYZ = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
		        bool flag2 = num - 0.0 < 1E-06;
		        if (flag2)
		        {
		            num = line.Direction.AngleTo(XYZ.BasisY);
		            xYZ = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
		        }
		        bool flag3 = num - 0.0 < 1E-06;
		        if (flag3)
		        {
		            num = line.Direction.AngleTo(XYZ.BasisZ);
		            xYZ = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
		        }
#if revit2016
                Plane plane = doc.Application.Create.NewPlane(xYZ, line.Origin);
#endif
#if revit2019
		        Plane plane = Plane.CreateByNormalAndOrigin(xYZ, line.Origin);
#endif
                SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
		        ModelCurve modelCurve = doc.Create.NewModelCurve(line, sketchPlane);
		    }

		}

		public static Element NewLine_WithoutTransaction_R(this Document doc, Curve c)
		{
			bool flag = c.GetEndPoint(0).IsAlmostEqualTo(c.GetEndPoint(1));
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Line line = Line.CreateBound(c.GetEndPoint(0), c.GetEndPoint(1));
				double num = line.Direction.AngleTo(XYZ.BasisX);
				XYZ xYZ = line.Direction.CrossProduct(XYZ.BasisX).Normalize();
				bool flag2 = num - 0.0 < 1E-06;
				if (flag2)
				{
					num = line.Direction.AngleTo(XYZ.BasisY);
					xYZ = line.Direction.CrossProduct(XYZ.BasisY).Normalize();
				}
				bool flag3 = num - 0.0 < 1E-06;
				if (flag3)
				{
					num = line.Direction.AngleTo(XYZ.BasisZ);
					xYZ = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();
				}
#if revit2016
                Plane plane = doc.Application.Create.NewPlane(xYZ, line.Origin);
#endif
#if revit2019
			    Plane plane = Plane.CreateByNormalAndOrigin(xYZ, line.Origin);
#endif
                SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
				ModelCurve modelCurve = doc.Create.NewModelCurve(line, sketchPlane);
				result = modelCurve;
			}
			return result;
		}

		public static void NewPoligon(this Document doc, List<XYZ> points)
		{
			bool flag = points.Count >= 2;
			if (flag)
			{
				for (int i = 0; i < points.Count; i++)
				{
					doc.NewLine(points[i], points[i + 1]);
				}
			}
		}

		public static void NewPoligon(this Document doc, Face face)
		{
			EdgeArrayArray edgeLoops = face.EdgeLoops;
			face.GetEdgesAsCurveLoops();
			foreach (CurveLoop current in face.GetEdgesAsCurveLoops())
			{
				foreach (Curve current2 in current)
				{
					doc.NewLine(current2 as Line);
				}
			}
		}

		public static void NewBox(this Document doc, BoundingBoxXYZ box)
		{
			Transform transform = box.Transform;
			XYZ min = box.Min;
			XYZ max = box.Max;
			XYZ xYZ = transform.OfPoint(min);
			XYZ xYZ2 = transform.OfPoint(max);
#if revit2016
            Plane p = new Plane(transform.BasisX, xYZ);
			Plane p2 = new Plane(transform.BasisY, xYZ);
			Plane p3 = new Plane(transform.BasisZ, xYZ);
			Plane p4 = new Plane(transform.BasisX, xYZ2);
			Plane p5 = new Plane(transform.BasisY, xYZ2);
			Plane p6 = new Plane(transform.BasisZ, xYZ2);
#endif
#if revit2019
		    Plane p =   Plane.CreateByNormalAndOrigin(transform.BasisX, xYZ);
		    Plane p2 =  Plane.CreateByNormalAndOrigin(transform.BasisY, xYZ);
		    Plane p3 =  Plane.CreateByNormalAndOrigin(transform.BasisZ, xYZ);
		    Plane p4 =  Plane.CreateByNormalAndOrigin(transform.BasisX, xYZ2);
		    Plane p5 =  Plane.CreateByNormalAndOrigin(transform.BasisY, xYZ2);
		    Plane p6 =  Plane.CreateByNormalAndOrigin(transform.BasisZ, xYZ2);
#endif
            XYZ xYZ3 = xYZ.Project(p4);
			XYZ xYZ4 = xYZ.Project(p5);
			XYZ xYZ5 = xYZ2.Project(p3);
			XYZ xYZ6 = xYZ2.Project(p);
			XYZ xYZ7 = xYZ2.Project(p2);
			XYZ pEnd = xYZ.Project(p6);
			doc.NewLine(xYZ, xYZ3);
			doc.NewLine(xYZ, xYZ4);
			doc.NewLine(xYZ3, xYZ5);
			doc.NewLine(xYZ4, xYZ5);
			doc.NewLine(xYZ2, xYZ6);
			doc.NewLine(xYZ2, xYZ7);
			doc.NewLine(xYZ6, pEnd);
			doc.NewLine(xYZ7, pEnd);
			doc.NewLine(xYZ, pEnd);
			doc.NewLine(xYZ3, xYZ7);
			doc.NewLine(xYZ4, xYZ6);
			doc.NewLine(xYZ5, xYZ2);
		}

		public static void NewBox_WithOutTransaction(this Document doc, BoundingBoxXYZ box)
		{
			Transform transform = box.Transform;
			XYZ min = box.Min;
			XYZ max = box.Max;
			XYZ xYZ = transform.OfPoint(min);
			XYZ xYZ2 = transform.OfPoint(max);
#if revit2016
            Plane p = new Plane(transform.BasisX, xYZ);
			Plane p2 = new Plane(transform.BasisY, xYZ);
			Plane p3 = new Plane(transform.BasisZ, xYZ);
			Plane p4 = new Plane(transform.BasisX, xYZ2);
			Plane p5 = new Plane(transform.BasisY, xYZ2);
			Plane p6 = new Plane(transform.BasisZ, xYZ2);
#endif
#if revit2019
		    Plane p = Plane.CreateByNormalAndOrigin(transform.BasisX, xYZ);
		    Plane p2 = Plane.CreateByNormalAndOrigin(transform.BasisY, xYZ);
		    Plane p3 = Plane.CreateByNormalAndOrigin(transform.BasisZ, xYZ);
		    Plane p4 = Plane.CreateByNormalAndOrigin(transform.BasisX, xYZ2);
		    Plane p5 = Plane.CreateByNormalAndOrigin(transform.BasisY, xYZ2);
		    Plane p6 = Plane.CreateByNormalAndOrigin(transform.BasisZ, xYZ2);
#endif
            XYZ xYZ3 = xYZ.Project(p4);
			XYZ xYZ4 = xYZ.Project(p5);
			XYZ xYZ5 = xYZ2.Project(p3);
			XYZ xYZ6 = xYZ2.Project(p);
			XYZ xYZ7 = xYZ2.Project(p2);
			XYZ pEnd = xYZ.Project(p6);
			doc.NewLine_WithoutTransaction(xYZ, xYZ3);
			doc.NewLine_WithoutTransaction(xYZ, xYZ4);
			doc.NewLine_WithoutTransaction(xYZ3, xYZ5);
			doc.NewLine_WithoutTransaction(xYZ4, xYZ5);
			doc.NewLine_WithoutTransaction(xYZ2, xYZ6);
			doc.NewLine_WithoutTransaction(xYZ2, xYZ7);
			doc.NewLine_WithoutTransaction(xYZ6, pEnd);
			doc.NewLine_WithoutTransaction(xYZ7, pEnd);
			doc.NewLine_WithoutTransaction(xYZ, pEnd);
			doc.NewLine_WithoutTransaction(xYZ3, xYZ7);
			doc.NewLine_WithoutTransaction(xYZ4, xYZ6);
			doc.NewLine_WithoutTransaction(xYZ5, xYZ2);
		}
	}
}
