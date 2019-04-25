using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using BinLibrary.RevitExtension;

namespace BinLibrary.RevitHelper
{
    public static class LinkPathHelper
    {
        /// <summary>
        /// 获取CAD路径
        /// </summary>
        /// <param name="cadlink"></param>
        /// <returns></returns>
        public static string GetCadLinkPath(this CADLinkType cadlink)
        {
            var isexternalFile = cadlink.IsExternalFileReference();
            if (!isexternalFile) throw new Exception("this file is not a external file!");
            var filepath =
                ModelPathUtils.ConvertModelPathToUserVisiblePath(cadlink.GetExternalFileReference().GetAbsolutePath());
            return filepath;
        }
        public static string GetCadLinkPath(this ImportInstance importins)
        {
            var doc = importins.Document;
            var cadlinktype = doc.GetElement(importins.GetTypeId()) as CADLinkType;
            if (cadlinktype == null) return "";
            return cadlinktype.GetCadLinkPath();
        }

        /// <summary>
        /// 获取REVIT路径
        /// </summary>
        /// <param name="revitlink"></param>
        /// <returns></returns>
        public static string GetRevitLinkPath(this RevitLinkInstance revitlink)
        {
            var doc = revitlink.Document;
            var rvtlinktype = revitlink.GetTypeId().GetElement(doc) as RevitLinkType;
            var filepath = rvtlinktype.GetRevitLinkPath();
            return filepath;
        }
        public static string GetRevitLinkPath(this RevitLinkType revitlinktype)
        {
            var filepath =
                ModelPathUtils.ConvertModelPathToUserVisiblePath(revitlinktype.GetExternalFileReference()
                    .GetAbsolutePath());
            return filepath;
        }
    }
}
