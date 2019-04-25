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
    public static class GenericExtension
    {
        public static IList<string> ToNameList<T>(this List<T> list)
        {
            IList<string> result = new List<string>();

            foreach (T v in list)
            {
                Element e = v as Element;
                if (e!=null)
                {
                    result.Add(e.Name);
                }
                
            }

            return result;
        }

        /// <summary>
        /// 转为对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<object> ToObjectList<T>(this List<T> list)
        {
            IList<object> result = new List<object>();

            foreach (T v in list)
            {
                object o = v;
                result.Add(o);
            }

            return result;
        }

        /// <summary>
        /// 转为object数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<object> ToObjectArray<T>(this List<T> list)
        {

            object[] result = new object[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                result[i] = list[i];
            }
            return result;
        }

        
    }
}
