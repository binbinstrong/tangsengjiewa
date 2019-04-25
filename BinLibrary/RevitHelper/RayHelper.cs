using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Autodesk.Revit.UI;

namespace BinLibrary.RevitHelper
{
	public static class RayHelper
	{

        /// <summary>
        /// 从起点沿方向查找 无限延伸
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="tt">默认为wall</param>
        /// <param name="target"></param>
        /// <returns></returns>
		public static IList<ReferenceWithContext> RayfindDir(this Document doc, XYZ origin, XYZ direction, Type tt = null, FindReferenceTarget target =FindReferenceTarget.Element)
		{
             
            IList<ReferenceWithContext> list = new List<ReferenceWithContext>();

		    View3D view3D = null;
		    var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Where(m => !(m as View3D).IsTemplate).Cast<View3D>();
		    if (view3ds.Count() > 0)
		    {
		        view3D = view3ds.First();
		    }
		    else
		    {
		        throw new Exception("BinException:没有3D视图");
		    }

			View3D view3D2 = doc.ActiveView as View3D;
			 
			if (view3D2!=null)
			{
				view3D = view3D2;
			}
			bool isSectionBoxActive = view3D.IsSectionBoxActive;
			if (isSectionBoxActive)
			{
				view3D.IsSectionBoxActive=(false);
			}
			if (tt==null)
			{
				tt = typeof(Wall);
			}
			ReferenceIntersector referenceIntersector = new ReferenceIntersector(new ElementClassFilter(tt), target, view3D);
			return referenceIntersector.Find(origin, direction);
		}
        
        /// <summary>
        /// 从起点向端点查找 无限延伸
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="origin"></param>
        /// <param name="end"></param>
        /// <param name="tt">默认为wall类型</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IList<ReferenceWithContext> Rayfind(this Document doc, XYZ origin, XYZ end, Type tt = null, FindReferenceTarget target = FindReferenceTarget.Element)
	    {
	        IList<ReferenceWithContext> list = new List<ReferenceWithContext>();
	        var direction = (end - origin).Normalize();
	        View3D view3D = null;
	        var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Where(m => !(m as View3D).IsTemplate).Cast<View3D>();
	        if (view3ds.Count() > 0)
	        {
	            view3D = view3ds.First();
	        }
	        else
	        {
	            throw new Exception("BinException:没有3D视图");
	        }

	        View3D view3D2 = doc.ActiveView as View3D;

	        if (view3D2 != null)
	        {
	            view3D = view3D2;
	        }
	        bool isSectionBoxActive = view3D.IsSectionBoxActive;
	        if (isSectionBoxActive)
	        {
	            view3D.IsSectionBoxActive = (false);
	        }
	        if (tt == null)
	        {
	            tt = typeof(Wall);
	        }
	        ReferenceIntersector referenceIntersector = new ReferenceIntersector(new ElementClassFilter(tt), target, view3D);
	        return referenceIntersector.Find(origin, direction);
	    }

	    public static IList<ReferenceWithContext> RayfindDir(this Document doc, XYZ origin, XYZ _direction, ElementFilter filter, FindReferenceTarget target = FindReferenceTarget.Element)
	    {
	        IList<ReferenceWithContext> list = new List<ReferenceWithContext>();
	        var direction = _direction;//(end - origin).Normalize();
	        View3D view3D = null;
	        var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Where(m => !(m as View3D).IsTemplate).Cast<View3D>();
	        if (view3ds.Count() > 0)
	        {
	            view3D = view3ds.First();
	        }
	        else
	        {
	            throw new Exception("BinException:没有3D视图");
	        }

	        View3D view3D2 = doc.ActiveView as View3D;

	        if (view3D2 != null)
	        {
	            view3D = view3D2;
	        }
	        bool isSectionBoxActive = view3D.IsSectionBoxActive;
	        if (isSectionBoxActive)
	        {
	            view3D.IsSectionBoxActive = (false);
	        }
	        
	        ReferenceIntersector referenceIntersector = new ReferenceIntersector(filter, target, view3D);
	        return referenceIntersector.Find(origin, direction);
	    }

        /// <summary>
        /// 从起点沿方向搜索指定距离
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <param name="filter"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IList<ReferenceWithContext> Rayfind(this Document doc, XYZ origin, XYZ direction, double length, ElementFilter filter, FindReferenceTarget target = FindReferenceTarget.Element)
		{
			IList<ReferenceWithContext> list = new List<ReferenceWithContext>();

		    View3D view3D = null;
		    var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Where(m => !(m as View3D).IsTemplate).Cast<View3D>();
		    if (view3ds.Count() > 0)
		    {
		        view3D = view3ds.First();
		    }
		    else
		    {
		        throw new Exception("BinException:没有3D视图");
		    }

		    View3D view3D2 = doc.ActiveView as View3D;

		    if (view3D2 != null)
		    {
		        view3D = view3D2;
		    }
		    bool isSectionBoxActive = view3D.IsSectionBoxActive;
		    if (isSectionBoxActive)
		    {
		        view3D.IsSectionBoxActive = (false);
		    }

		    //MessageBox.Show("rayfind in view3d:" + view3D.Id.IntegerValue.ToString() + ":" + view3D.Name);
             
			ReferenceIntersector referenceIntersector = new ReferenceIntersector(filter, target, view3D);
			IList<ReferenceWithContext> list2 = referenceIntersector.Find(origin, direction);
			foreach (ReferenceWithContext current in list2)
			{
				bool flag2 = Math.Abs(current.Proximity - length) < 1E-06;
				if (flag2)
				{
					list.Add(current);
				}
				else
				{
					bool flag3 = current.Proximity < length;
					if (flag3)
					{
						list.Add(current);
					}
				}
			}
			return list;
		}
        /// <summary>
        /// 从起点查找到终点
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="filter"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IList<ReferenceWithContext> Rayfind(this Document doc, XYZ start, XYZ end, ElementFilter filter, FindReferenceTarget target)
		{
			IList<ReferenceWithContext> list = new List<ReferenceWithContext>();
			XYZ direction = (end - start).Normalize();
			double length = end.DistanceTo(start);
			return doc.Rayfind(start, direction, length, filter, target);
		}
		public static ElementId RayfindFirst(this Document doc, XYZ origin, XYZ direction)
		{
			IList<ReferenceWithContext> list = new List<ReferenceWithContext>();

		    View3D view3D = null;
		    var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Where(m => !(m as View3D).IsTemplate).Cast<View3D>();
		    if (view3ds.Count() > 0)
		    {
		        view3D = view3ds.First();
		    }
		    else
		    {
		        throw new Exception("BinException:没有3D视图");
		    }

		    View3D view3D2 = doc.ActiveView as View3D;

		    if (view3D2 != null)
		    {
		        view3D = view3D2;
		    }
		    bool isSectionBoxActive = view3D.IsSectionBoxActive;
		    if (isSectionBoxActive)
		    {
		        view3D.IsSectionBoxActive = (false);
		    }
            ReferenceIntersector referenceIntersector = new ReferenceIntersector(view3D);
			IList<ReferenceWithContext> list2 = referenceIntersector.Find(origin, direction);
			list = list2.OrderBy(m=>m.Proximity).ToList();
			bool flag2 = list.Count != 0;
			ElementId result;
			if (flag2)
			{
				result = list.First<ReferenceWithContext>().GetReference().ElementId;
			}
			else
			{
				result = null;
			}
			return result;
		}

        /// <summary>
        /// 传入三维视图
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="view3d"></param>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
	    public static ElementId RayfindFirst(this Document doc,View3D view3d, XYZ origin, XYZ direction)
	    {
	        IList<ReferenceWithContext> list = new List<ReferenceWithContext>();

	        View3D view3D = null;
	        var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Where(m => !(m as View3D).IsTemplate).Cast<View3D>();
	        if (view3ds.Count() > 0 && view3D == null )
	        {
	            view3D = view3ds.First();
	        }
            else if (view3D != null)
	        {
                
	        }
	        else
	        {
	            throw new Exception("BinException:没有3D视图");
	        }

	        View3D view3D2 = doc.ActiveView as View3D;

	        if (view3D2 != null)
	        {
	            view3D = view3D2;
	        }
            //优先采用 传入视图 view3D 
	        view3D = view3d ?? view3D2;

            bool isSectionBoxActive = view3D.IsSectionBoxActive;
	        if (isSectionBoxActive)
	        {
	            view3D.IsSectionBoxActive = (false);
	        }
	        ReferenceIntersector referenceIntersector = new ReferenceIntersector(view3D);
	        IList<ReferenceWithContext> list2 = referenceIntersector.Find(origin, direction);
	        list = list2.OrderBy(m => m.Proximity).ToList();
	        bool flag2 = list.Count != 0;
	        ElementId result;
	        if (flag2)
	        {
	            result = list.First<ReferenceWithContext>().GetReference().ElementId;
	        }
	        else
	        {
	            result = null;
	        }
	        return result;
	    }
        #region 从集合中查找

        public static IList<ReferenceWithContext> RayfindDir(this Document doc, XYZ origin, XYZ direction,ICollection<ElementId> elements, FindReferenceTarget target = FindReferenceTarget.Element)
	    {
	        IList<ReferenceWithContext> list = new List<ReferenceWithContext>();

	        View3D view3D = null;
	        var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Where(m => !(m as View3D).IsTemplate).Cast<View3D>();
	        if (view3ds.Count() > 0)
	        {
	            view3D = view3ds.First();
	        }
	        else
	        {
	            throw new Exception("BinException:没有3D视图");
	        }

	        View3D view3D2 = doc.ActiveView as View3D;

	        if (view3D2 != null)
	        {
	            view3D = view3D2;
	        }
	        bool isSectionBoxActive = view3D.IsSectionBoxActive;
	        if (isSectionBoxActive)
	        {
	            view3D.IsSectionBoxActive = (false);
	        }
	        
	        ReferenceIntersector referenceIntersector = new ReferenceIntersector(elements, target, view3D);
	        return referenceIntersector.Find(origin, direction);
        }
	    //public static IList<ReferenceWithContext> Rayfind(this Document doc, XYZ origin, XYZ end, ICollection<ElementId> elements, FindReferenceTarget target = FindReferenceTarget.Element)
	    //{
	    //    IList<ReferenceWithContext> list = new List<ReferenceWithContext>();

	    //    var direction = (end - origin).Normalize();

	    //    View3D view3D = null;
	    //    var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Where(m => !(m as View3D).IsTemplate).Cast<View3D>();
	    //    if (view3ds.Count() > 0)
	    //    {
	    //        view3D = view3ds.First();
	    //    }
	    //    else
	    //    {
	    //        throw new Exception("BinException:没有3D视图");
	    //    }

	    //    View3D view3D2 = doc.ActiveView as View3D;

	    //    if (view3D2 != null)
	    //    {
	    //        view3D = view3D2;
	    //    }
	    //    bool isSectionBoxActive = view3D.IsSectionBoxActive;
	    //    if (isSectionBoxActive)
	    //    {
	    //        view3D.IsSectionBoxActive = (false);
	    //    }

	    //    ReferenceIntersector referenceIntersector = new ReferenceIntersector(elements, target, view3D);
	    //    return referenceIntersector.Find(origin, direction);
	    //}
	    public static IList<ReferenceWithContext> Rayfind(this Document doc, XYZ origin, XYZ direction, double length, ICollection<ElementId> elements, FindReferenceTarget target = FindReferenceTarget.Element)
	    {
	        IList<ReferenceWithContext> list = new List<ReferenceWithContext>();

	        View3D view3D = null;
	        var view3ds = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Where(m => !(m as View3D).IsTemplate).Cast<View3D>();
	        if (view3ds.Count() > 0)
	        {
	            view3D = view3ds.First();
	        }
	        else
	        {
	            throw new Exception("BinException:没有3D视图");
	        }

	        View3D view3D2 = doc.ActiveView as View3D;

	        if (view3D2 != null)
	        {
	            view3D = view3D2;
	        }
	        bool isSectionBoxActive = view3D.IsSectionBoxActive;
	        if (isSectionBoxActive)
	        {
	            view3D.IsSectionBoxActive = (false);
	        }

	        ReferenceIntersector referenceIntersector = new ReferenceIntersector(elements, target, view3D);
	        IList<ReferenceWithContext> list2 = referenceIntersector.Find(origin, direction);
	        foreach (ReferenceWithContext current in list2)
	        {
	            bool flag2 = Math.Abs(current.Proximity - length) < 1E-06;
	            if (flag2)
	            {
	                list.Add(current);
	            }
	            else
	            {
	                bool flag3 = current.Proximity < length;
	                if (flag3)
	                {
	                    list.Add(current);
	                }
	            }
	        }
	        return list;
	    }

	    public static IList<ReferenceWithContext> Rayfind(this Document doc, XYZ start, XYZ end, ICollection<ElementId> elements, FindReferenceTarget target)
	    {
	        IList<ReferenceWithContext> list = new List<ReferenceWithContext>();
	        XYZ direction = (end - start).Normalize();
	        double length = end.DistanceTo(start);
	        return doc.Rayfind(start, direction, length, elements, target);
	    }

        #endregion
    }
}
