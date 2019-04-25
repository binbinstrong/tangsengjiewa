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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using BinLibrary.Extensions;
using BinLibrary.RevitHelper;

namespace BinLibrary.RevitExtension
{
    public static class MepcurveExtension
    {
        /// <summary>
        /// 添加新的管线类型
        /// </summary>
        /// <param name="mepType"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static MEPCurveType AddNewMepCurveType(this MEPCurveType mepType, string typeName)
        {
            MEPCurveType result = null;

            result = mepType.Duplicate(typeName) as MEPCurveType;

            return result;
        }

        /// <summary>
        /// 获取所有的Mepcurvetype类型
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static IList<MEPCurveType> GetAllMepcurveTypes(this Document doc)
        {
            return doc.GetElementTypes<MEPCurveType>();
        }

        #region 分类获取管线类型
        
        public static IList<PipeType> GetAllPipeTypes(this Document doc)
        {
            return doc.GetElementTypes<PipeType>();
        }

        public static IList<DuctType> GetAllDuctTypes(this Document doc)
        {
            return doc.GetElementTypes<DuctType>();
        }

        public static IList<CableTrayType> GetAllCableTrayType(this Document doc)
        {
            return doc.GetElementTypes<CableTrayType>();
        }

        public static IList<Conduit> GetAllConduitType(this Document doc)
        {
            return doc.GetElementTypes<Conduit>();
        }

        public static IList<FlexDuctType> GetAllFlexDuctTypes(this Document doc)
        {
            return doc.GetElementTypes<FlexDuctType>();
        }

        public static IList<FlexPipeType> GetAllFlexPipeTypes(this Document doc)
        {
            return doc.GetElementTypes<FlexPipeType>();
        }

        #endregion

        /// <summary>
        /// 删除管线类型
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="t"></param>
        /// <param name="Typename"></param>
        /// <returns></returns>
        public static bool RemoveMepcurveType(this Document doc, Type t, string Typename)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            Element ele = collector.OfClass(t).Where(m => m.Name == Typename).First();

            try
            {
                doc.Delete(ele.Id);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除桥架类型(连同桥架连接件一起删除)
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static bool Remove(this CableTrayType ct)
        {
            Document doc = ct.Document;

            ParameterSet paraset = ct.Parameters;
            Dictionary<BuiltInParameter, Parameter> paraDic = paraset.GetParameterBuitInDic();

            try
            {
                foreach (CableTrayFittings ctf in Enum.GetValues(typeof(CableTrayFittings)))
                {
                    Parameter p = paraDic[(BuiltInParameter)ctf];
                    ElementId partId = p.AsElementId();
                    doc.Delete(partId);
                }

                doc.Delete(ct.Id);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        #region 桥架类型配置

        /// <summary>
        /// 桥架类型配置 使用桥架类型名称配置桥架连接件名称
        /// </summary>
        /// <param name="ctt"></param>
        public static void ConfigCableTrayType(this CableTrayType ctt, string name = "标准"/*, Dictionary<string, ElementId> fittingDic*/)
        {
            string _name = name;
            Document doc = ctt.Document;
            ParameterSet paraset = ctt.Parameters;

            //获取builtinparameter 和 parameter的对应字典 用于获取桥架类型内的 配置参数
            Dictionary<BuiltInParameter, Parameter> dic = new Dictionary<BuiltInParameter, Parameter>();

            dic = paraset.GetParameterBuitInDic();

            foreach (CableTrayFittings ctf in Enum.GetValues(typeof(CableTrayFittings)))
            {

                Parameter p = null;
                dic.TryGetValue((BuiltInParameter)ctf, out p);
                if (p == null)
                {
                    continue;
                }
                //获取管道配置中连接件的参数

                ElementId partEleId = p.AsElementId();//获取管道配置中连接件的参数 指定的连接件 族id

                FamilySymbol partEleSym = null;

                if (partEleId != null && partEleId.IntegerValue > 0)
                {
                    partEleSym = partEleId.GetElement(doc) as FamilySymbol;
                }
                else
                {
                    return;
                }

                FamilySymbol partEle_tar = partEleSym.Duplicate(_name) as FamilySymbol;

                /* Parameter p_target =*/
                ctt.get_Parameter((BuiltInParameter)ctf);

                p.Set(partEle_tar.Id);

                //partEleIns.AddNewType(cabletrayName);
            }
        }

        /// <summary>
        /// 配置桥架连接件并且根据字典设置参数
        /// </summary>
        /// <param name="ctt"></param>
        /// <param name="paradic"></param>
        /// <param name="name"></param>
        public static void ConfigCableTrayTypeAndSetPara(this CableTrayType ctt, Dictionary<string, object> paradic, string name = "标准"/*(类型名)*/   /*, Dictionary<string, ElementId> fittingDic*/)
        {
            string _name = name;
            Document doc = ctt.Document;
            ParameterSet paraset = ctt.Parameters;

            List<FamilySymbol> fittinglist = (new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_CableTrayFitting)
                 .OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>().ToList());
            //取得桥架连接件名字字典
            Dictionary<string, FamilySymbol> fittingDic = fittinglist.ToDictionary(m => string.Concat(m.FamilyName, m.Name), n => n);

            //设置桥架类型 直线段 参数
            ctt.SetParameters(paradic);

            //获取builtinparameter 和 parameter的对应字典 用于获取桥架类型内的 配置参数
            Dictionary<BuiltInParameter, Parameter> dic = new Dictionary<BuiltInParameter, Parameter>();

            dic = paraset.GetParameterBuitInDic();

            foreach (CableTrayFittings ctf in Enum.GetValues(typeof(CableTrayFittings)))
            {

                // Parameter p = dic[(BuiltInParameter)ctf]; //获取管道配置中连接件的参数
                Parameter p = null;
                dic.TryGetValue((BuiltInParameter)ctf, out p);
                if (p == null)
                {
                    continue;
                }

                ElementId partEleId = p.AsElementId();//获取管道配置中连接件的参数 指定的连接件 族id

                FamilySymbol partEleSym = null;

                if (partEleId != null && partEleId.IntegerValue > 0)
                {
                    partEleSym = partEleId.GetElement(doc) as FamilySymbol;

                    //设置桥架连接件族符号 参数
                    partEleSym.SetParameters(paradic);
                }
                else
                {
                    return;
                }

                FamilySymbol partEle_tar = null;//= partEleSym.Duplicate(_name) as FamilySymbol;

                if (!fittingDic.ContainsKey(partEleSym.FamilyName + _name))
                {
                    partEle_tar = partEleSym.Duplicate(_name) as FamilySymbol;
                }
                else
                {
                    partEle_tar = fittingDic[partEleSym.FamilyName + _name];
                }
                /* Parameter p_target =*/
                //ctt.get_Parameter((BuiltInParameter)ctf);
                p.Set(partEle_tar.Id);

                //partEleIns.AddNewType(cabletrayName);
            }
        }

        /// <summary>
        /// 设置桥架新类型的参数
        /// </summary>
        /// <param name="ctt"></param>
        /// <param name="paraName"></param>
        /// <param name="paraValue"></param>
        public static void SetCableTrayTypeParameter(this CableTrayType ctt, string paraName, object paraValue/*, Dictionary<string, ElementId> fittingDic*/)
        {
            //++++++++++设置桥架参数
            Parameter para_type = ctt.LookupParameter(paraName);
            if (para_type != null)
                para_type.SetParaValue(paraValue);

            string cabletrayName = ctt.Name;
            Document doc = ctt.Document;
            ParameterSet paraset = ctt.Parameters;

            //获取builtinparameter 和 parameter的对应字典 用于获取桥架类型内的 配置参数
            Dictionary<BuiltInParameter, Parameter> dic = new Dictionary<BuiltInParameter, Parameter>();

            dic = paraset.GetParameterBuitInDic();

            foreach (CableTrayFittings ctf in Enum.GetValues(typeof(CableTrayFittings)))
            {

                Parameter p = dic[(BuiltInParameter)ctf]; //获取管道配置中连接件的参数

                ElementId partEleId = p.AsElementId();//获取管道配置中连接件的参数 指定的连接件 族id

                FamilySymbol partEleSym = null;

                if (partEleId != null && partEleId.IntegerValue > 0)
                {
                    partEleSym = partEleId.GetElement(doc) as FamilySymbol;
                }
                else
                {
                    return;
                }

                //FamilySymbol partEle_tar = partEleSym.Duplicate(cabletrayName) as FamilySymbol;
                //p.Set(partEle_tar.Id);

                //++++++++++此处设置桥架连接件的参数
                Parameter para_sym = partEleSym.LookupParameter(paraName);
                if (para_sym != null)
                {
                    para_sym.SetParaValue(paraValue);
                }
            }
        }

        /// <summary>
        /// 设置桥架 和 其所有连接件的参数
        /// </summary>
        /// <param name="ctt"></param>
        /// <param name="paras"></param>
        public static void SetCableTrayTypeParameters(this CableTrayType ctt, Dictionary<string, object> paras)
        {
            Document doc = ctt.Document;
            //设置桥架直线段参数
            ctt.SetParameters(paras);
            Dictionary<BuiltInParameter, Parameter> fittingDic = ctt.Parameters.GetParameterBuitInDic();
            foreach (CableTrayFittings value in Enum.GetValues(typeof(CableTrayFittings)))
            {
                Parameter p = fittingDic[(BuiltInParameter)value];
                ElementId partEleId = p.AsElementId();
                //FamilySymbol fs = p.AsElementId().GetElement(doc) as FamilySymbol;
                FamilySymbol partEleSymbol = null;

                if (partEleId != null && partEleId.IntegerValue > 0)
                {
                    partEleSymbol = partEleId.GetElement(doc) as FamilySymbol;
                }
                else
                {
                    return;
                }
                if (partEleSymbol != null)
                {
                    partEleSymbol.SetParameters(paras);
                }
            }
        }

        #endregion


        public static XYZ StartPoint(this MEPCurve mep)
        {
            var line = mep.LocationLine();
            return line.StartPoint();
        }

        public static XYZ EndPoint(this MEPCurve mep)
        {
            var line = mep.LocationLine();
            return line.EndPoint();
        }

        public static Connector StartCon(this MEPCurve mep)
        {
            Connector result = null;
            var connectors = mep.ConnectorManager.Connectors;
            foreach (Connector con in connectors)
            {
                var connectortype = con.ConnectorType;
                if (connectortype == ConnectorType.Curve || connectortype == ConnectorType.End)
                {
                    if (con.Origin.IsAlmostEqualTo(mep.StartPoint()))
                    {
                        result = con;
                        break;
                    }
                }
            }
            return result;
        }
        public static Connector EndCon(this MEPCurve mep)
        {
            Connector result = null;
            var connectors = mep.ConnectorManager.Connectors;
            foreach (Connector con in connectors)
            {
                var connectortype = con.ConnectorType;
                if (connectortype == ConnectorType.Curve || connectortype == ConnectorType.End)
                {
                    if (con.Origin.IsAlmostEqualTo(mep.EndPoint()))
                    {
                        result = con;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获得位置线
        /// </summary>
        /// <param name="mep"></param>
        /// <returns></returns>
        public static Line LocationLine(this MEPCurve mep)
        {
            Line result = null;

            result = (mep.Location as LocationCurve).Curve as Line;

            return result;
        }

        /// <summary>
        /// 判断有打开的连接件
        /// </summary>
        /// <param name="mep"></param>
        /// <returns></returns>
        public static bool ContainsOpenCon(this MEPCurve mep)
        {
            var connectors = mep.ConnectorManager.Connectors.PhysicalConToList();

            var opencons = connectors.Where(m => !m.IsConnected);

            return opencons.Count() > 0;
        }
        public static double? Elevation(this MEPCurve mep)
        {
            double? result = null;
            if (mep.IsAlmostHorizontal())
            {
                result = mep.StartPoint().Z;
            }
            return result;
        }
        /// <summary>
        /// 判断管线竖直
        /// </summary>
        /// <param name="mep"></param>
        /// <returns></returns>
        public static bool IsVertical(this MEPCurve mep)
        {
            var locationline = mep.LocationLine();

            if (DoubleExtension.IsEqual(locationline.GetEndPoint(0).DistanceTo_Hosizontal(locationline.GetEndPoint(1)), 0))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断管线水平
        /// </summary>
        /// <param name="mep"></param>
        /// <returns></returns>
        public static bool IsHorizontal(this MEPCurve mep)
        {
            var locationline = mep.LocationLine();

            if (locationline.GetEndPoint(0).Z.IsEqual(locationline.GetEndPoint(1).Z))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断管线近似水平
        /// </summary>
        /// <param name="mep"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsAlmostHorizontal(this MEPCurve mep, double tolerance = 0.1d)
        {
            var locationline = mep.LocationLine();
            if (Math.Abs(locationline.GetEndPoint(0).Z - locationline.GetEndPoint(1).Z) < tolerance.MetricToFeet())
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断管线近似竖直
        /// </summary>
        /// <param name="mep"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsAlmostVertical(this MEPCurve mep, double tolerance=0.1d)
        {
            var locationline = mep.LocationLine();
            if (locationline.GetEndPoint(0).DistanceTo_Hosizontal(locationline.GetEndPoint(1)) < tolerance.MetricToFeet())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取管道形状（圆形 方形 椭圆形）
        /// </summary>
        /// <param name="mep"></param>
        /// <returns></returns>
        public static ConnectorProfileType Shape(this MEPCurve mep)
        {
            var con = mep.ConnectorManager.Connectors.PhysicalConToList().First();
            return con.Shape;
        }

        /// <summary>
        /// 管线等效高度
        /// </summary>
        /// <param name="mep"></param>
        /// <returns></returns>
        public static double Heightequivalent(this MEPCurve mep)
        {
            double result = 0;

            var shape = mep.Shape();

            switch (shape)
            {
                case ConnectorProfileType.Round:
                    if (mep is Pipe)
                    {
                        result = mep.get_Parameter(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER).AsDouble();
                    }
                    else
                        result = mep.Diameter;
                    break;
                case ConnectorProfileType.Rectangular:
                    result = mep.Height;
                    break;
                case ConnectorProfileType.Oval:
                    result = mep.Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }
        /// <summary>
        /// 管线等效宽度
        /// </summary>
        /// <param name="mep"></param>
        /// <returns></returns>
        public static double Widthequivalent(this MEPCurve mep)
        {
            double result = 0;
            var shape = mep.Shape();
            switch (shape)
            {
                case ConnectorProfileType.Round:
                    if (mep is Pipe)
                    {
                        result = mep.get_Parameter(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER).AsDouble();
                    }
                    else
                        result = mep.Diameter;
                    break;
                case ConnectorProfileType.Rectangular:
                    result = mep.Width;
                    break;
                case ConnectorProfileType.Oval:
                    result = mep.Width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }
    }

    public enum CableTrayFittings
    {
        HorizontalElbow = BuiltInParameter.RBS_CURVETYPE_DEFAULT_HORIZONTAL_BEND_PARAM,
        VerticalinnerElbow = BuiltInParameter.RBS_CURVETYPE_DEFAULT_ELBOWUP_PARAM,
        VerticalouterElbow = BuiltInParameter.RBS_CURVETYPE_DEFAULT_ELBOWDOWN_PARAM,
        Tee = BuiltInParameter.RBS_CURVETYPE_DEFAULT_TEE_PARAM,
        Cross = BuiltInParameter.RBS_CURVETYPE_DEFAULT_CROSS_PARAM,
        Transition = BuiltInParameter.RBS_CURVETYPE_DEFAULT_TRANSITION_PARAM,
        Union = BuiltInParameter.RBS_CURVETYPE_DEFAULT_UNION_PARAM,
    }


}
