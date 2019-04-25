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

namespace BinLibrary.RevitExtension
{
    public static class ElementExtension
    {
        public static IList<Element> ToElements<Tsource>(this IList<Tsource> sources, Document doc)
        {
            List<Element> result = new List<Element>();

            foreach (Tsource t in sources)
            {
                //ElementId id = t as ElementId;
                ElementId id = null;

                if (t is ElementId)
                {
                    id = t as ElementId;
                }
                else if (t is Element)
                {
                    id = (t as Element).Id;
                }
                else if (t is Reference)
                {
                    id = (t as Reference).ElementId;
                }

                if (id != null)
                {
                    result.Add(doc.GetElement(id) as Element);
                }
            }

            return result;
        }

        public static IList<MEPCurve> ToMepcurves<Tsource>(this IList<Tsource> sources, Document doc)
        {
            List<MEPCurve> result = new List<MEPCurve>();

            foreach (Tsource t in sources)
            {
                //ElementId id = t as ElementId;
                ElementId id = null;

                if (t is ElementId)
                {
                    id = t as ElementId;
                }
                else if (t is Element)
                {
                    id = (t as Element).Id;
                }
                else if (t is Reference)
                {
                    id = (t as Reference).ElementId;
                }

                if (id != null)
                {
                    result.Add(doc.GetElement(id) as MEPCurve);
                }
            }

            return result;
        }

        #region 元素可见性 未测试

        /// <summary>
        /// 判断元素永久隐藏
        /// </summary>
        /// <param name="element"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool IsHiddenPermanent(this Element element, View view)
        {
           return  element.IsHidden(view);
        }

        /// <summary>
        /// 判断元素被临时隐藏  //未测试
        /// </summary>
        /// <param name="element"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool IsHiddenTemporary(this Element element, View view)
        {
            //element.IsHidden(view);
            
             
            var hideinIsolateMode = 
                view.IsElementVisibleInTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate, element.Id);
            if (hideinIsolateMode)
            {
                return false;
            }
            return true;
        }

        #endregion

        
    }
}
