using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
/*
 知识点
 1. 隐藏取消隐藏对象需要在事务内部完成
 2. 取消临时隐藏 最好用 静态集合 记录 隐藏对象 然后对隐藏对象进行取消临时隐藏
 3. 在ShowAllElements（） 方法中 采用 枚举类别（Catetory）的方法来显示所有类别 达到取消隐藏的目的
 */
namespace BinLibrary
{
    /// <summary>
    /// 可视控制类 控制视图中元素显示隐藏
    /// </summary>
    public static class VisiableHelper
    {

        #region 永久显示隐藏

        /// <summary>
        /// 使元素仅在当前视图（永久）可见
        /// </summary>
        /// <param name="element"></param>
        /// <param name="view"></param>
        public static void UniqueViewShowElement(Element element, View view)
        {
            Document doc = element.Document;
            UIDocument uidoc = new UIDocument(doc);
            Selection sel = uidoc.Selection;

            FilteredElementCollector viewCollector = new FilteredElementCollector(doc);
            viewCollector.OfClass(typeof(View)).Where(m => !(m as View).IsTemplate);
            IList<Element> viewelements = viewCollector.ToElements();

            #region fortest

            //string viewnames = "";
            //foreach (Element viewelement in viewelements)
            //{
            //    View v = viewelement as View;
            //    viewnames += "view:" + v.Name + Environment.NewLine;
            //    //LogHelper.LogWrite(@"c:\views.txt",v.Name);
            //}

            //FilteredElementCollector viewTemplatecolector = new FilteredElementCollector(doc);

            //viewTemplatecolector.OfClass(typeof (View)).Where(m => (m as View).IsTemplate);
            //IList<Element> viewtemeplates = viewCollector.ToElements();
            //string viewtemplatenames = "";
            //foreach (Element viewtemplate in viewtemeplates)
            //{
            //    View v = viewtemplate as View;
            //    viewtemplatenames += "viewtemplate:" + viewtemplate.Name + Environment.NewLine;
            //}
            //LogHelper.LogWrite(@"c:\views.txt", viewnames);
            //LogHelper.LogWrite(@"c:\views.txt", viewtemplatenames);

            #endregion

            foreach (Element viewelement in viewelements)
            {
                View v = viewelement as View;

                if (v.ViewType == ViewType.Elevation ||
                    v.ViewType == ViewType.AreaPlan ||
                    v.ViewType == ViewType.CeilingPlan ||
                    v.ViewType == ViewType.Detail ||
                    v.ViewType == ViewType.EngineeringPlan ||
                    v.ViewType == ViewType.FloorPlan ||
                    v.ViewType == ViewType.Section ||
                    v.ViewType == ViewType.ThreeD ||
                    v.ViewType == ViewType.Walkthrough)
                {
                    if (v.Name != view.Name && element.CanBeHidden(v))
                    {
                        ViewHideElement(element, v);
                    }
                }
            }
 
        }

        public static void UniqueViewShowElements(IList<Element> elements, View view)
        {
            Document doc = view.Document;
            UIDocument uidoc = new UIDocument(doc);
            Selection sel = uidoc.Selection;

            FilteredElementCollector viewCollector = new FilteredElementCollector(doc);
            viewCollector.OfClass(typeof(View)).Where(m => !(m as View).IsTemplate);
            IList<Element> viewelements = viewCollector.ToElements();

            #region fortest

            //string viewnames = "";
            //foreach (Element viewelement in viewelements)
            //{
            //    View v = viewelement as View;
            //    viewnames += "view:" + v.Name + Environment.NewLine;
            //    //LogHelper.LogWrite(@"c:\views.txt",v.Name);
            //}

            //FilteredElementCollector viewTemplatecolector = new FilteredElementCollector(doc);

            //viewTemplatecolector.OfClass(typeof (View)).Where(m => (m as View).IsTemplate);
            //IList<Element> viewtemeplates = viewCollector.ToElements();
            //string viewtemplatenames = "";
            //foreach (Element viewtemplate in viewtemeplates)
            //{
            //    View v = viewtemplate as View;
            //    viewtemplatenames += "viewtemplate:" + viewtemplate.Name + Environment.NewLine;
            //}
            //LogHelper.LogWrite(@"c:\views.txt", viewnames);
            //LogHelper.LogWrite(@"c:\views.txt", viewtemplatenames);

            #endregion

            foreach (Element viewelement in viewelements)
            {
                View v = viewelement as View;

                if (
                    v.ViewType == ViewType.Elevation ||
                    v.ViewType == ViewType.AreaPlan ||
                    v.ViewType == ViewType.CeilingPlan ||
                    v.ViewType == ViewType.Detail ||
                    v.ViewType == ViewType.EngineeringPlan ||
                    v.ViewType == ViewType.FloorPlan ||
                    v.ViewType == ViewType.Section ||
                    v.ViewType == ViewType.ThreeD ||
                    v.ViewType == ViewType.Walkthrough
                    
                    )
                {
                    if (v.Name != view.Name )
                    {
                        v.HideElements(elements.Select(m=>m.Id).ToList());
                    }
                }
            }

        }

        /// <summary>
        /// 在所有视图中显示元素
        /// </summary>
        /// <param name="element"></param>
        /// <param name="view"></param>
        public static void ViewsUnHideElement(Element element, View view)
        {
            Document doc = element.Document;
            UIDocument uidoc = new UIDocument(doc);
            Selection sel = uidoc.Selection;

            FilteredElementCollector viewCollector = new FilteredElementCollector(doc);
            viewCollector.OfClass(typeof(View)).Where(m => !(m as View).IsTemplate);
            IList<Element> viewelements = viewCollector.ToElements();

            foreach (Element viewelement in viewelements)
            {
                View v = viewelement as View;

                if (v.ViewType == ViewType.Elevation ||
                    v.ViewType == ViewType.AreaPlan ||
                    v.ViewType == ViewType.CeilingPlan ||
                    v.ViewType == ViewType.Detail ||
                    v.ViewType == ViewType.EngineeringPlan ||
                    v.ViewType == ViewType.FloorPlan ||
                    v.ViewType == ViewType.Section ||
                    v.ViewType == ViewType.ThreeD ||
                    v.ViewType == ViewType.Walkthrough)
                {
                    if (element.IsHidden(v))
                    {
                        ViewUnHideElement(element, v);
                    }
                }
            }
        }

        /// <summary>
        /// 在视图中隐藏对象
        /// </summary>
        /// <param name="element"></param>
        /// <param name="view"></param>
        public static void ViewHideElement(Element element, View view)
        {
            ICollection<ElementId> elementids = new List<ElementId>();

            if (element.CanBeHidden(view))
            {
                elementids.Clear();
                elementids.Add(element.Id);
                view.HideElements(elementids);
            }
        }
        public static void ViewHideElements(IList<Element> elements, View view)
        {
             view.HideElements(elements.Select(m => m.Id).ToList());
        }

        /// <summary>
        /// 在视图中显示隐藏对象
        /// </summary>
        /// <param name="element"></param>
        /// <param name="view"></param>
        public static void ViewUnHideElement(Element element, View view)
        {
            ICollection<ElementId> elementids = new List<ElementId>();
            if (element.IsHidden(view))
            {
                elementids.Clear();
                elementids.Add(element.Id);
                view.UnhideElements(elementids);
            }
        }
        public static void ViewUnHideElements(IList<Element> elements, View view)
        {
            foreach (var element in elements)
            {
                ViewUnHideElement(element, view);
            }
        }

        #endregion


        #region 临时显示隐藏元素
        /// <summary>
        /// 临时隐藏元素
        /// </summary>
        /// <param name="element"></param>
        /// <param name="view"></param>
        public static void ViewTemHideElement(Element element, View view)
        {
            ICollection<ElementId> elementids = new List<ElementId>();

            if (element.CanBeHidden(view))
            {
                elementids.Clear();
                elementids.Add(element.Id);
                view.HideElementsTemporary(elementids);
            }
        }
        public static void ViewTemHideElements(IList<Element> elements, View view)
        {
            foreach (var element in elements)
            {
                ViewTemHideElement(element, view);
            }
        }

        /// <summary>
        /// 取消临时隐藏元素
        /// </summary>
        /// <param name="element"></param>
        /// <param name="view"></param>
        public static void ViewTemUnHideElement(Element element, View view)
        {
            ICollection<ElementId> elementids = new List<ElementId>();
            if (element.IsHidden(view))
            {
                elementids.Clear();
                elementids.Add(element.Id);
                view.UnhideElements(elementids);
            }
        }
        public static void ViewTemUnHideElements(IList<Element> elements, View view)
        {
            foreach (var element in elements)
            {
                ViewUnHideElement(element, view);
            }
        }
        #endregion


        /// <summary>
        /// 显示所有元素
        /// </summary>
        /// <param name="view"></param>
        public static void ShowAllElements(View view)
        {
            Document doc = view.Document;
            foreach (int ct in Utility.TargetCategories)
            {
                Category cat = doc.Settings.Categories.get_Item((BuiltInCategory) ct);
                if (cat!=null)
                {
#if revit2016
                    view.SetVisibility(cat,true);
#endif
#if revit2019
                    view.SetCategoryHidden(cat.Id,false);
#endif
                }
            }
        }
        
    }

    public class Utility
    {
        #region field1

        
        private static HashSet<int> _targetCategories = new HashSet<int>()
        {
            -2000011,
            -2000035,
            -2000032,
            -2000100,
            -2001330,
            -2000014,
            -2000023,
            -2001340,
            -2001260,
            -2000120,
            -2000180,
            -2001300,
            -2001336,
            -2001320,
            -2001327,
            -2000126,
            -2000170,
            -2000171,
            -2000340,
            -2003200,
            -2008044,
            -2008000,
            -2008049,
            -2008010,
            -2001350,
            -2003400,
            -2001180,
            -2001160,
            -2008099,
            -2000038,
            -2008079,
            -2000080,
            -2001100,
            -2008039,
            -2000151,
            -2008077,
            -2008083,
            -2001140,
            -2001360,
            -2001000,
            -2008085,
            -2008087,
            -2001120,
            -2001370,
            -2001060,
            -2001040,
            -2008130,
            -2008126,
            -2008075,
            -2000996,
            -2008161,
            -2008055,
            -2008122,
            -2008132,
            -2008128,
            -2000269,
            -2001354,
            -2009003,
            -2009009,
            -2009030,
            -2009000,
            -2009016,
            -2009017,
            -2002000,
            -2008050,
            -2008020,
            -2008081,
            -2001220,
            -2008124,
            -2008160,
            -2008016,
            -2008123,
            -2008013,
        };

        #endregion

        public static HashSet<int> TargetCategories
        {
            get { return _targetCategories; }
            set { _targetCategories = value; }
        }
    }
}
