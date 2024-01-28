using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace ClassLibrary3
{
    class Utils
    {
        /// <summary>
        /// Gets solid objects of specified element.
        /// </summary>
        /// <param name="elem">Element to retrieve solid geometry.</param>
        /// <param name="opt">geometry option.</param>
        /// <returns>Solids of the geometry.</returns>
        static public List<Solid> GetElementSolids(Element elem, Options opt = null)
        {
            if (null == elem)
            {
                return null;
            }
            if (null == opt)
                opt = new Options();
            List<Solid> solids = new List<Solid>();
            GeometryElement gElem = null;
            try
            {
                gElem = elem.get_Geometry(opt);

                IEnumerator<GeometryObject> gIter = gElem.GetEnumerator();
                gIter.Reset();
                while (gIter.MoveNext())
                {
                    solids.AddRange(GetSolids(gIter.Current));
                }
            }
            catch (Exception ex)
            {
                // In Revit, sometime get the geometry will failed.
                string error = ex.Message;
            }
            return solids;
        }

        /// <summary>
        /// Gets all solid objects from geometry object.
        /// </summary>
        /// <param name="gObj">Geometry object from where to get solids. </param>
        /// <returns>The solids of the geometry object. </returns>
        static public List<Solid> GetSolids(GeometryObject gObj)
        {
            List<Solid> solids = new List<Solid>();
            if (gObj is Solid) // already solid
            {
                Solid solid = gObj as Solid;
                if (solid.Faces.Size > 0 && solid.Volume > 0) // some solid may have not any face?!
                    solids.Add(gObj as Solid);
            }
            else if (gObj is GeometryInstance) // find solids from GeometryInstance
            {
                IEnumerator<GeometryObject> gIter2 = (gObj as GeometryInstance).GetInstanceGeometry().GetEnumerator();
                gIter2.Reset();
                while (gIter2.MoveNext())
                {
                    solids.AddRange(GetSolids(gIter2.Current));
                }
            }
            else if (gObj is GeometryElement) // find solids from GeometryElement, this will not happen at all?
            {
                IEnumerator<GeometryObject> gIter2 = (gObj as GeometryElement).GetEnumerator();
                gIter2.Reset();
                while (gIter2.MoveNext())
                {
                    solids.AddRange(GetSolids(gIter2.Current));
                }
            }
            return solids;
        }

        /// <summary>
        /// Draws model curves by given geometry curves.
        /// </summary>
        /// <param name="revitDoc"></param>
        /// <param name="curves"></param>
        /// <param name="reviseTrf"></param>
        /// <returns></returns>
        public static List<ElementId> DrawModelCurves(Document revitDoc, List<Curve> curves, Transform reviseTrf = null)
        {
            List<ElementId> newIds = new List<ElementId>();
            foreach (Curve crv in curves)
            {
                if (crv.IsBound)
                {
                    Curve reviseCrv = (null != reviseTrf && !reviseTrf.IsIdentity) ? crv.CreateTransformed(reviseTrf) : crv;
                    newIds.Add(createModelCurve(revitDoc, reviseCrv));
                }
            }
            return newIds;
        }

        private static ElementId createModelCurve(Document document, Curve curve, SketchPlane sp = null)
        {
            Line line = curve as Line;
            Arc arc = curve as Arc;
            Ellipse ellipse = curve as Ellipse;
            HermiteSpline spline = curve as HermiteSpline;
            NurbSpline nbSpline = curve as NurbSpline;
            
            if (line != null && null == sp)
            {
                XYZ normal = getVertVec(line.Direction).Normalize();
                XYZ origin = line.GetEndPoint(0);
#if Revit2016
                sp = SketchPlane.Create(document, new Plane(normal, origin)) ;
#endif
#if Revit2018
                 sp = SketchPlane.Create(document, Plane.CreateByNormalAndOrigin(normal, origin));
#endif

            }
            else if (arc != null && null == sp)
            {
                XYZ normal = arc.Normal;
#if Revit2016
                sp = SketchPlane.Create(document, new Plane(normal, arc.Center));
#endif
#if Revit2018
                sp = SketchPlane.Create(document, Plane.CreateByNormalAndOrigin(normal, arc.Center));
#endif
            }
            else if (ellipse != null && null == sp)
            {
                XYZ normal = ellipse.Normal;
#if Revit2016
                sp = SketchPlane.Create(document,new Plane(normal, ellipse.Center));
#endif
#if Revit2018
                sp = SketchPlane.Create(document, Plane.CreateByNormalAndOrigin(normal, ellipse.Center));
#endif
            }
            else if (spline != null && null == sp)
            {
                Transform tran = spline.ComputeDerivatives(0, false);
                XYZ normal = getVertVec(tran.BasisX).Normalize();
                XYZ origin = spline.GetEndPoint(0);
#if Revit2016
                sp = SketchPlane.Create(document,new Plane(normal, origin));

#endif
#if Revit2018
                sp = SketchPlane.Create(document, Plane.CreateByNormalAndOrigin(normal, origin));

#endif
            }
            else if (nbSpline != null && null == sp)
            {
                Transform tran = nbSpline.ComputeDerivatives(0, false);
                XYZ normal = getVertVec(tran.BasisX).Normalize();
                XYZ origin = nbSpline.GetEndPoint(0);
#if Revit2016
                sp = SketchPlane.Create(document, new Plane(normal, origin));

#endif
#if Revit2018
                sp = SketchPlane.Create(document, Plane.CreateByNormalAndOrigin(normal, origin));

#endif
            }

            if (sp == null)
            {
                throw new ArgumentException("Not valid sketchplane to create curve:" + curve.GetType().Name);
            }
            //
            // create model line with curve and the specified sketch plane.
            ModelCurve mCurve = document.Create.NewModelCurve(curve, sp);
            return (null != mCurve) ? mCurve.Id : ElementId.InvalidElementId;
        }

        /// <summary>
        /// Gets one vertical vector of given vector, the return vector is not normalized.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        private static XYZ getVertVec(XYZ vec)
        {
            XYZ ret = new XYZ(-vec.Y + vec.Z, vec.X + vec.Z, -vec.Y - vec.X);
            return ret;
        }

        static public bool CanMakeBound(XYZ end0, XYZ end1)
        {
            try
            {
                Line ln = Line.CreateBound(end0, end1);
                return (null != ln);
            }
            catch (System.Exception)
            {
                return false;
            }
        }
         
    }
}
