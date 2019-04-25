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
using System.Runtime.CompilerServices;
using System.Text;
using Autodesk.Revit.DB;


namespace BinLibrary.RevitExtension
{
    public static class ElementidExtension
    {
         
        public static Element GetElement(this ElementId eid, Document doc)
        {
            Element result = null;

            result = doc.GetElement(eid);

            return result;
        }

        /// <summary>
        /// 转为名字集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
       

    }
}
