using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace BinLibrary
{
    public static class DocumentExtension
    {
        

        public static bool CloseCurrentDocument(this UIDocument uidoc)
        {
            //Application app = doc.Application;

            Application app = uidoc.Document.Application;
            bool result = false;

            string templatefilename = @"C:\ProgramData\Autodesk\RVT 2016\Templates\China\DefaultCHSCHS.rte";
            string path = @"c:\temrvt.rvt";
            Document holderDoc = null;


            if (!System.IO.File.Exists(templatefilename))
            {
                throw new System.ArgumentException("模板文件不存在");
            }
            if (!System.IO.File.Exists(path))
            {
                holderDoc = app.NewProjectDocument(templatefilename);
                holderDoc.SaveAs(path);
                holderDoc.Close();
            }
            //else
            //{
            //    holderDoc = app.OpenDocumentFile(path);
            //    holderDoc.Close();
            //}
             
            UiCloseHleper closeHelper = new UiCloseHleper(uidoc.Application, path);
            result = closeHelper.CloseAndSave(uidoc);
            

            return result;
        }

      
        /// <summary>
        /// 找到当前文档中所有的元素类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static IList<T> GetElementTypes<T>(this Document doc)
        {
            List<T> result = new List<T>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(T)).WhereElementIsElementType();

            //result =  collector.OfClass(typeof(T)).WhereElementIsElementType().Cast<T>().PhysicalConToList(); //可以用本句代码代替以下代码
            //MessageBox.Show(collector.Count().ToString());

            foreach (var ele in collector)
            {
                dynamic tem = ele;
                result.Add(tem);
            }
            return result;
        }

        /// <summary>
        /// 根据名字找到目标type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static T GetTargetType<T>(this Document doc, string typeName)
        {
            T result = default(T);

            FilteredElementCollector collector = new FilteredElementCollector(doc);

            collector.OfClass(typeof(T));
            foreach (var v in collector)
            {
                dynamic d = v;
                if (d.Name == typeName)
                {
                    result = d;
                }
            }
             
            return result;
        }

        
    }


    /// <summary>
    /// 关闭当前文档的帮助类
    /// 创建空白文档 作文当前文档，目标文档变为后台文档，只有后台文档才可关闭
    /// </summary>
    public class UiCloseHleper
    {
        #region Data

        private Autodesk.Revit.UI.UIDocument m_placeHolderDoc;
        private string m_placeHolderPath;
        private dynamic m_uiAppInterface;

        #endregion
        /// <summary>
        /// 构造函数 初始化一个uiapplication 和 占位（placeholder）文档路径
        /// </summary>
        /// <param name="uiAppInterface"></param>
        /// <param name="PlaceHolderPath"></param>
        public UiCloseHleper(dynamic uiAppInterface, string PlaceHolderPath)
        {
            m_uiAppInterface = uiAppInterface;
            if (m_uiAppInterface == null)
            {
                throw new System.ArgumentNullException("m_uiAppInterface");
            }
            if ((!(uiAppInterface is Autodesk.Revit.UI.UIApplication)) && (!(uiAppInterface is Autodesk.Revit.UI.Macros.ApplicationEntryPoint)))
            {
                throw new System.ArgumentException("Must pass a UIApplication  or applicationEnryPoint");
            }

            SetPlaceHolderPath(PlaceHolderPath);

        }

        /// <summary>
        /// 关闭并保存文档 
        /// </summary>
        /// <param name="uiDoc"></param>
        /// <returns></returns>
        public bool CloseAndSave(Autodesk.Revit.UI.UIDocument uiDoc)
        {
            if (null == uiDoc)
            {
                throw new System.ArgumentNullException("uiDoc");
            }
            RejectClosingPlaceHolder(uiDoc);
            if (!EnsureActivePlaceHolder(m_uiAppInterface))
            {
                return false;
            }
            return uiDoc.SaveAndClose();
        }

        /// <summary>
        /// Closes a DB.Document with the same
        /// interface as DB.Document.Close
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public bool Close(Autodesk.Revit.DB.Document doc)
        {
            return Close(doc, true);
        }
        /// <summary>
        /// Closes a DB.Document with the same
        /// interface as DB.Document.Close
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="savemodefied"></param>
        /// <returns></returns>
        public bool Close(Autodesk.Revit.DB.Document doc, bool savemodefied)
        {
            if (null == doc)
            {
                throw new System.ArgumentNullException("doc");
            }

            RejectClosingPlaceHolder(doc);
            if (!EnsureActivePlaceHolder(m_uiAppInterface))
            {
                return false;
            }

            return doc.Close(savemodefied);
        }

        #region Helper methods

        /// <summary>
        /// Throws an exception if the user 
        /// passes the placeholder document
        /// </summary>
        /// <param name="doc"></param>
        private void RejectClosingPlaceHolder(Autodesk.Revit.DB.Document doc)
        {
            if (doc.Title == System.IO.Path.GetFileName(this.m_placeHolderPath))
            {
                throw new System.ArgumentNullException("Cannot close placeholder doc:" + doc.Title);
            }
        }

        /// <summary>
        /// Throws an exception if the user 
        /// passes the placeholder document
        /// </summary>
        /// <param name="uiDoc"></param>
        private void RejectClosingPlaceHolder(Autodesk.Revit.UI.UIDocument uiDoc)
        {
            RejectClosingPlaceHolder(uiDoc.Document);
        }

        /// <summary>
        /// Ensures that the placeholder document is open 
        /// and active to that another open document can be closed
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        private bool EnsureActivePlaceHolder(dynamic application)
        {
            try
            {
                if (m_placeHolderDoc == null)
                {
                    m_placeHolderDoc = application.OpenAndActivateDocument(m_placeHolderPath);
                }                                   
                else
                {
                    m_placeHolderDoc.SaveAndClose();
                    m_placeHolderDoc = application.OpenAndActivateDocument(m_placeHolderPath);
                }
                return true;
            }
            catch (Exception e )
            {
                throw e;
                return false;
            }
        }

        /// <summary>
        /// Sets the path of the placeholder document
        /// </summary>
        /// <param name="path"></param>
        private void SetPlaceHolderPath(string path)
        {
            m_placeHolderPath = path;
            if (!System.IO.File.Exists(m_placeHolderPath))
            {
                throw new System.ArgumentException("file not found:" + path);
            }
            if (!(System.IO.Path.GetExtension(m_placeHolderPath) != "rvt"))
            {
                throw new System.ArgumentException("Placeholder:" + path + "is not a revit file");
            }
        }

        #endregion


    }

    
}
