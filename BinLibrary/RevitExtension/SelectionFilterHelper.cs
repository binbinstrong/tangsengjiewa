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
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI.Selection;
using BinLibrary.WinHelper;

namespace BinLibrary.RevitExtension
{
    public static class SelectionFilterHelper
    {
        /// <summary>
        /// 选择连接模型中的图元
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static LinkDocumentFilter GetSelectionFilterInLinkFile(this Document doc, Type type)
        {
            return new LinkDocumentFilter(doc, type);//, element);
        }

        public static SelectionFilter GetSelectionFilter(this Document doc, Type type)
        {
            return new SelectionFilter(doc, type);
        }

        public static MultiSelectionFilter GetSelectionFilter(this Document doc, Func<Element, bool> func1, Func<Reference, bool> func2 = null)
        {
            return new MultiSelectionFilter(func1, func2);
        }
    }

    /// <summary>
    /// Rvt连接文件过滤器 
    /// </summary>
    public class LinkDocumentFilter : ISelectionFilter
    {
        public Document _doc = null;
        public Type _type = null;
        //public Type _t = null;

        /// <summary>
        /// Type 用来判断 元素类别
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        public LinkDocumentFilter(Document doc, Type type)
        {
            _doc = doc;
            _type = type;
        }
        
        public bool AllowElement(Element elem)
        {
            return true;
        }
        public bool AllowReference(Reference reference, XYZ position)
        {
            //_element.Assembly.CreateInstance(_element);
            Element ele = _doc.GetElement(reference);

            RevitLinkInstance rvtlinkinstance = ele as RevitLinkInstance;

            Document linkDoc = rvtlinkinstance.GetLinkDocument();

            if (null != linkDoc)
            {
                Element element = linkDoc.GetElement(reference.LinkedElementId);
                //LogHelper.LogWrite(@"c:\abc.txt", element.GetType().Name + ":" + _type.Name);

                if (element.GetType() == this._type)
                {
                    //element.GetType()
                    return true;
                }
            }
            return false;
        }

    }
     
    /// <summary>
    /// 当前文档元素过滤
    /// </summary>
    public class SelectionFilter : ISelectionFilter
    {
        private Document _doc;
        private Type _type;

        public SelectionFilter(Document doc, Type type)
        {
            _doc = doc;
            _type = type;
        }
        public bool AllowElement(Element elem)
        {
            if (elem.GetType() == _type)
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }

    public class MultiSelectionFilter : ISelectionFilter
    {
        private Func<Element, bool> eleFunc;
        private Func<Reference, bool> refFunc;


        public MultiSelectionFilter(Func<Element, bool> func, Func<Reference, bool> func1)
        {
            eleFunc = func;
            refFunc = func1;
        }
        public bool AllowElement(Element elem)
        {
            return refFunc != null ? true : eleFunc(elem);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return refFunc == null ? false : refFunc(reference);
            //return  refFunc(reference);
        }
    }

    /// <summary>
    /// 选择cad里面线条
    /// </summary>
    public class CadSelectionFilter : ISelectionFilter
    {
        public Document _doc = null;
        public Type _type = null;
        //public Type _t = null;

        /// <summary>
        /// Type 用来判断 元素类别
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        public CadSelectionFilter(Document doc, Type type)
        {
            _doc = doc;
            _type = type;
        }
        public bool AllowElement(Element elem)
        {
            
            _doc = elem.Document;
            if(elem is ImportInstance)
            return true;
            return false;
        }
        public bool AllowReference(Reference reference, XYZ position)
        {
            //_element.Assembly.CreateInstance(_element);
            Element ele = _doc.GetElement(reference);

            //CADLinkType cadlink = ele as CADLinkType;
            ImportInstance importinstance = ele as ImportInstance;
            if (importinstance == null) return false;

            Options options = new Options();
            options.ComputeReferences = true;
            options.DetailLevel = ViewDetailLevel.Fine;
            options.View = _doc.ActiveView;
            GeometryElement geometryele= importinstance.get_Geometry(new Options());

            //var objettype = importinstance.GetTypeId();

            var geoobj = ele.GetGeometryObjectFromReference(reference);
            if (geoobj.GetType().Name == _type.GetType().Name) return true;

            //var graphicstyleid = geometryele.GraphicsStyleId;
            
            return false;
        }
    }

    public class cadSelectionFilter:ISelectionFilter
    {
        private ImportInstance _importInstance;
        private readonly Dictionary<string, Type> _filters;

        public cadSelectionFilter(List<Type> filters = null)
        {
            if (filters!=null)
            {
                _filters = new Dictionary<string, Type>();
                foreach (var filter in filters)
                {
                    if (!_filters.ContainsKey(filter.Name))
                    {
                        _filters.Add(filter.Name,filter);
                    }
                }
            }
        }

        public bool AllowElement(Element elem)
        {
            _importInstance = elem as ImportInstance;
            return _importInstance != null && _importInstance.IsLinked;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            try
            {
                var go = _importInstance?.Document.GetElement(reference).GetGeometryObjectFromReference(reference);
                if (go!=null&& go.GraphicsStyleId.IntegerValue!=-1&&(_filters==null||_filters.ContainsKey(go.GetType().Name)))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
