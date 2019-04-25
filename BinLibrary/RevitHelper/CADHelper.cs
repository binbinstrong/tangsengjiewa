using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using BinLibrary.RevitExtension;

namespace BinLibrary.RevitHelper
{
    /// <summary>
    /// 未经过测试
    /// </summary>
    public static class CADHelper
    {
        /// <summary>
        /// 获取连接CAD路径
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static string GetCadPath(Document doc,Reference reference)
        {
            //Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "选择链接cad文件");
            Element elem = doc.GetElement(reference);
            CADLinkType type = doc.GetElement(elem.GetTypeId()) as CADLinkType;
            if (!type.IsExternalFileReference()) throw new Exception("this file is not a external file!");
            string filePath = ModelPathUtils.ConvertModelPathToUserVisiblePath(type.GetExternalFileReference().GetAbsolutePath());
            return filePath;
        }

        public static string GetLayerName(Document doc, Reference reference)
        {
            string result = null;

            var geoele = doc.GetElement(reference);

            var geoobj = geoele.GetGeometryObjectFromReference(reference);

            var graphicstyleid = geoobj.GraphicsStyleId;
            GraphicsStyle graphicstyle = null;

            if (graphicstyleid!=ElementId.InvalidElementId)
            {
                graphicstyle = graphicstyleid.GetElement(doc) as GraphicsStyle;
            }
            else
            {
                return result;
            }
            
            if (graphicstyle != null)
            {
                result = graphicstyle.GraphicsStyleCategory.Name;
            }
            
            return result;

        }
        /// <summary>
        /// 获取CAD图块名
        /// </summary>
        /// <param name="importinstance"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static string GetBlockReferenceName(ImportInstance importinstance, Reference reference)
        {
            var doc = importinstance.Document;
            string name = null;
            GeometryObject go = importinstance.GetGeometryObjectFromReference(reference);
            using (TransactionGroup tg = new TransactionGroup(doc))
            {
                tg.Start();
                DirectShape ds = null;
                using (Transaction ts = new Transaction(doc))
                {
                    ts.Start("temp directionshape");
#if revit2016
                    ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                    ds.AppendShape(new List<GeometryObject>(){go});
#endif
#if revit2019
                    
#endif
                    ts.Commit();
                }
                Options options = new Options {ComputeReferences = true, View = doc.ActiveView};
                var gi = ds.get_Geometry(options).FirstOrDefault(i => i is GeometryInstance) as GeometryInstance;
                name = gi?.Symbol?.Name;
                tg.RollBack();
            }
            return name;
        }
    }
}
