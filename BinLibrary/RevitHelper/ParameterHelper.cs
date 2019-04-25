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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using BinLibrary.RevitExtension;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace BinLibrary.RevitHelper
{

    /// <summary>
    /// 参数帮助类 共享参数重绑定的方法未封装
    /// </summary>
    public static class ParameterHelper
    {

         
        ////+++++族参数添加删除++++++
        /// <summary>
        /// 给族添加参数 使用前先判断是否已经添加该参数
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="paraName"></param>
        /// <param name="bpg"></param>
        /// <param name="pt"></param>
        /// <param name="Isinstance"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool AddParameter(this FamilyInstance fi,
            string paraName,
            BuiltInParameterGroup bpg = BuiltInParameterGroup.PG_TEXT,
            ParameterType pt = ParameterType.Text,
            bool Isinstance = true
            )
        {
            bool result = false;
            Document _doc = fi.Document;
            Document familyDoc = _doc.EditFamily(fi.Symbol.Family);
            //Document familyDoc = null;// familyDoc=_doc.EditFamily(fi.Symbol.Family);
            try
            {

                familyDoc.Invoke(m =>
                {
                    if (m.GetStatus() == TransactionStatus.Started)
                    {
                        familyDoc.SubtranInvoke(n =>
                        {
                            FamilyManager fm = familyDoc.FamilyManager;
                            if (Isinstance)
                            {
                                fm.AddParameter(paraName, bpg, pt, Isinstance);
                            }
                            else
                            {
                                fm.AddParameter(paraName, bpg, pt, Isinstance);
                            }

                            familyDoc.LoadFamily(_doc, new FamilyOptions());
                        });
                    }

                }, "modifyFamily");

                familyDoc.Close(false);
                result = true;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                result = false;
                familyDoc.Close(false);
            }

            return result;
        }

        /// <summary>
        /// 给族添加参数 使用前先判断是否已经添加该参数
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="pa"></param>
        /// <param name="bpg"></param>
        /// <param name="pt"></param>
        /// <param name="Isinstance"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool AddParameter(this FamilyInstance fi,
            Parameter pa,
            BuiltInParameterGroup bpg = BuiltInParameterGroup.PG_TEXT,
            ParameterType pt = ParameterType.Text,
            bool Isinstance = true
            )
        {

            return AddParameter(fi, pa.Definition.Name, bpg, pt, Isinstance);

        }
        /// <summary>
        /// 删除族参数
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="paraName"></param>
        /// <param name="bpg"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static bool RemoveParameter(this FamilyInstance fi,string paraName)
        {
            bool result = false;
            Document _doc = fi.Document;
            Document familyDoc = _doc.EditFamily(fi.Symbol.Family);
            //Document familyDoc = null;// familyDoc=_doc.EditFamily(fi.Symbol.Family);
            try
            {

                familyDoc.Invoke(m =>
                {
                    if (m.GetStatus() == TransactionStatus.Started)
                    {
                        familyDoc.SubtranInvoke(n =>
                        {
                            FamilyManager fm = familyDoc.FamilyManager;

                            FamilyParameterSet paraset = fm.Parameters;

                            foreach (FamilyParameter para in paraset)
                            {
                                if (para.Definition.Name == paraName)
                                {
                                    fm.RemoveParameter(para);
                                    break;
                                }
                            }

                            familyDoc.LoadFamily(_doc, new FamilyOptions());
                        });
                    }

                }, "modifyFamily");

                familyDoc.Close(false);
                result = true;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                result = false;
                familyDoc.Close(false);
            }

            return result;
        }
        /// <summary>
        /// 删除族参数
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="fmPara"></param>
        /// <param name="bpg"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static bool RemoveParameter(this FamilyInstance fi,
            FamilyParameter fmPara,
            BuiltInParameterGroup bpg = BuiltInParameterGroup.PG_TEXT,
            ParameterType pt = ParameterType.Text 
             
        )
        {
            return RemoveParameter(fi, fmPara.Definition.Name);
        }


         
        ////+++++共享参数添加++++++


        /// <summary>
        /// 为类别添加共享参数
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="doc"></param>
        /// <param name="builtincategory"></param>
        /// <param name="shareparameterName"></param>
        /// <param name="ca"></param>
        /// <returns></returns>
        public static bool AddSharedParameter(
            UIApplication uiapp,
            string shareparameterName,
            string shareparametergroupName,
            BuiltInCategory builtincategory, //要添加共享参数的类别
            ParameterType parametertype,//参数类型
            BuiltInParameterGroup builtinparameterGroup = BuiltInParameterGroup.INVALID, //参数组
            bool isInstance = true, 
            bool isVisible = true,
            string shareparameterfilename = ""
            )
        {

            Application _app = uiapp.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            //Document _doc = doc;
            //Autodesk.Revit.Creation.Application app1 = null;

            string _shareparameterfileName = shareparameterfilename;
            string _shareparameterName = shareparameterName;
            string _shareparameterGroupName = shareparametergroupName;

            ParameterType _parameterType = parametertype;
            BuiltInCategory _builtinCategory = builtincategory;

            //共享参数文件路径设置
            

            if (!File.Exists(uiapp.Application.SharedParametersFilename))
            {
                uiapp.Application.SharedParametersFilename = _shareparameterfileName;
            }
            else
            {
                _shareparameterfileName = uiapp.Application.SharedParametersFilename;
            }
            DefinitionFile shareparameterFile = uiapp.Application.OpenSharedParameterFile();

            
            //绑定到类
            CategorySet categories = null;
            categories = uiapp.Application.Create.NewCategorySet();


            Category TargetCategory = null;
            TargetCategory = uiapp.ActiveUIDocument.Document.Settings.Categories.get_Item(_builtinCategory);
            categories.Insert(TargetCategory);


            //创建绑定
            InstanceBinding instanceBinding = null;
            instanceBinding = uiapp.Application.Create.NewInstanceBinding(categories);

            TypeBinding typeBinding = null;
            typeBinding = uiapp.Application.Create.NewTypeBinding(categories);


            //参数
            //共享参数分组
            Autodesk.Revit.DB.DefinitionGroup shareparameterGroup = null;
            shareparameterGroup = shareparameterFile.Groups.get_Item(_shareparameterGroupName);

            if (null == shareparameterGroup)
            {
                shareparameterGroup = shareparameterFile.Groups.Create(_shareparameterGroupName);
            }

            //参数值
            Definition shareParameterDefinition = null;
            shareParameterDefinition = shareparameterGroup.Definitions.get_Item(_shareparameterName);

            if (shareParameterDefinition == null)
            {
                ExternalDefinitionCreationOptions definitioncreateOptions =
            new ExternalDefinitionCreationOptions(_shareparameterName, _parameterType);
                definitioncreateOptions.Visible = isVisible; //控制参数对用户是否可见

                shareParameterDefinition = shareparameterGroup.Definitions.Create(definitioncreateOptions);
            }

            if (isInstance)
            {
                return uiapp.ActiveUIDocument.Document.ParameterBindings.Insert(
                    shareParameterDefinition, instanceBinding, builtinparameterGroup
                );
            }
            else
            {
                return uiapp.ActiveUIDocument.Document.ParameterBindings.Insert(shareParameterDefinition, typeBinding,
                    builtinparameterGroup);
            }
           
        }

        /// <summary>
        /// 为一组类别添加共享参数
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="shareparameterName"></param>
        /// <param name="shareparametergroupName"></param>
        /// <param name="builtincategory"></param>
        /// <param name="parametertype"></param>
        /// <param name="builtinparameterGroup"></param>
        /// <param name="isInstance"></param>
        /// <param name="isVisible"></param>
        /// <param name="shareparameterfilename"></param>
        /// <returns></returns>
        public static bool AddSharedParameter(
            UIApplication uiapp,
            string shareparameterName,
            string shareparametergroupName,
            IList<BuiltInCategory> builtincategory, //要添加共享参数的类别
            ParameterType parametertype,//参数类型
            BuiltInParameterGroup builtinparameterGroup = BuiltInParameterGroup.INVALID, //参数组
            bool isInstance = true,
            bool isVisible = true,
            string shareparameterfilename = ""
            )
        {

            Application _app = uiapp.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            //Document _doc = doc;
            //Autodesk.Revit.Creation.Application app1 = null;

            string _shareparameterfileName = shareparameterfilename;
            string _shareparameterName = shareparameterName;
            string _shareparameterGroupName = shareparametergroupName;

            ParameterType _parameterType = parametertype;
            IList<BuiltInCategory> _builtinCategory = builtincategory;

            //共享参数文件路径设置

            if (!File.Exists(uiapp.Application.SharedParametersFilename))
            {
                uiapp.Application.SharedParametersFilename = _shareparameterfileName;
            }
            else
            {
                _shareparameterfileName = uiapp.Application.SharedParametersFilename;
            }
            DefinitionFile shareparameterFile = uiapp.Application.OpenSharedParameterFile();


            //绑定到类
            CategorySet categories = null;
            categories = uiapp.Application.Create.NewCategorySet();


            Category TargetCategory = null;
            foreach (BuiltInCategory builtincate in _builtinCategory)
            {
                TargetCategory = uiapp.ActiveUIDocument.Document.Settings.Categories.get_Item(builtincate);
                categories.Insert(TargetCategory);
            }
            


            //创建绑定
            InstanceBinding instanceBinding = null;
            instanceBinding = uiapp.Application.Create.NewInstanceBinding(categories);

            TypeBinding typeBinding = null;
            typeBinding = uiapp.Application.Create.NewTypeBinding(categories);


            //参数
            //共享参数分组
            Autodesk.Revit.DB.DefinitionGroup shareparameterGroup = null;
            shareparameterGroup = shareparameterFile.Groups.get_Item(_shareparameterGroupName);

            if (null == shareparameterGroup)
            {
                shareparameterGroup = shareparameterFile.Groups.Create(_shareparameterGroupName);
            }

            //参数值
            Definition shareParameterDefinition = null;
            shareParameterDefinition = shareparameterGroup.Definitions.get_Item(_shareparameterName);

            if (shareParameterDefinition == null)
            {
                ExternalDefinitionCreationOptions definitioncreateOptions =
            new ExternalDefinitionCreationOptions(_shareparameterName, _parameterType);
                definitioncreateOptions.Visible = isVisible; //控制参数对用户是否可见

                shareParameterDefinition = shareparameterGroup.Definitions.Create(definitioncreateOptions);
            }

            if (isInstance)
            {
                return uiapp.ActiveUIDocument.Document.ParameterBindings.Insert(
                    shareParameterDefinition, instanceBinding, builtinparameterGroup
                );
            }
            else
            {
                return uiapp.ActiveUIDocument.Document.ParameterBindings.Insert(shareParameterDefinition, typeBinding,
                    builtinparameterGroup);
            }

        }


 
         
        ////+++++判断参数+++++++

        /// <summary>
        /// 判断是共享参数
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsSharedParameter(Parameter p)
        {
            return p.IsShared;
        }

        /// <summary>
        /// 判断是项目参数
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsProjectParameter(Document doc, Parameter p)
        {
            bool result = false;
            BindingMap maps = doc.ParameterBindings;

            foreach (Definition def in maps)
            {
                if (def == p.Definition && !p.IsShared) //p 不是共享参数 也不再项目的bindingmap内
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 判断某个类别是否已具有共享参数
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="paraName"></param>
        /// <param name="builtincategory"></param>
        /// <returns></returns>
        public static bool AlreadyAddedSharedParameter(Document doc, string paraName, BuiltInCategory builtincategory)
        {
            try
            {
                BindingMap bindingMap = doc.ParameterBindings;
                DefinitionBindingMapIterator bindingmapIter = bindingMap.ForwardIterator();

                while (bindingmapIter.MoveNext())
                {
                    if (bindingmapIter.Key.Name.Equals(paraName))
                    {
                        ElementBinding binding = bindingmapIter.Current as ElementBinding;
                        CategorySet categories = binding.Categories;
                        foreach (Category category in categories)
                        {
                            if (category.Id.IntegerValue.Equals((int)builtincategory))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 判断参数是不是类型参数
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsTypeParameter(this Parameter p)
        {
            Element ele = p.Element;
            if (ele is ElementType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

         
        ////+++++删除参数++++++

        /// <summary>
        /// 删除一个元素的共享参数
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        /// <returns></returns>
        public static bool RemoveSharedParametersOfElement(this Element ele)
        {
            bool result = false;
            Document doc = ele.Document;
            BindingMap maps = doc.ParameterBindings;

            ParameterSet paraset = ele.Parameters;

            foreach (Parameter p in paraset)
            {
                if (p.IsShared)
                {
                    result = maps.Remove(p.Definition);
                }
            }
            return result;
        }

      
        /// <summary>
        /// 删除项目所有共享参数
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static void RemoveAllSharedParameterOfAProject(Document doc)
        {
            BindingMap maps = doc.ParameterBindings;

            IList<Parameter> paras = GetAllSharedParameter(doc);
            foreach (Parameter para in paras)
            {
                maps.Remove(para.Definition);
            }

            return;
        }

         
        ////+++++获取参数++++++


        /// <summary>
        /// 获取一个元素所有的共享参数
        /// </summary>
        /// <param name="ele"></param>
        /// <returns></returns>
        public static IList<Parameter> GetShareParametersOfElement(this Element ele)
        {
            IList<Parameter> result = new List<Parameter>();
            foreach (Parameter para in ele.Parameters)
            {
                if (para.IsShared)
                {
                    result.Add(para);
                }
            }

            return result;
        }


        /// <summary>
        /// 从文档中获取所有的共享参数 /*共享文件也可以获取共享参数但不一定与当前文档中的共享参数对应*/
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static IList<Parameter> GetAllSharedParameter(Document doc)
        {
            IList<Parameter> result = new List<Parameter>();

            Categories cates = doc.Settings.Categories;
            IList<Parameter> shareparameters = new List<Parameter>();
            foreach (Category cate in cates)
            {
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                FilteredElementCollector collector_element = new FilteredElementCollector(doc);

                BuiltInCategory category = (BuiltInCategory)cate.Id.IntegerValue;

                Element ele = collector_element.OfCategory(category).WhereElementIsNotElementType().FirstElement();//过滤元素（不包含类型）
                Element typeele = collector.OfCategory(category).WhereElementIsElementType().FirstElement(); //仅过滤类型

                if (ele != null)
                {
                    //MessageBox.Show("inner1"+ele.Name);

                    ParameterSet paraset = ele.Parameters;
                    ///获取元素实例参数中的共享参数

                    foreach (Parameter para in paraset)
                    {
                        if (para.IsShared)
                        {
                            shareparameters.Add(para);
                            //MessageBox.Show("inner2"+para.Definition.Name);
                        }
                    }
                }
                if (typeele != null)
                {
                    //MessageBox.Show((typeele is ElementType).ToString());

                    ParameterSet typeparaset = typeele.Parameters;

                    ///获取元素的类型参数中的共享参数
                    foreach (Parameter para in typeparaset)
                    {
                        if (para.IsShared)
                        {
                            shareparameters.Add(para);
                        }
                    }
                }
            }
            return result = shareparameters;
        }
        
        //+++++++++++设置参数++++++++++++
        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="value"></param>
        public static void SetParaValue(this Parameter p, object value)
        {
#if DEBUG
            //MessageBox.Show(value.ToString());
            //var value1 = UnitUtils.ConvertToInternalUnits(Convert.ToDouble(value), DisplayUnitType.DUT_MILLIMETERS);
            //MessageBox.Show(value1.ToString());
#endif
            switch (p.StorageType)
            {
                case StorageType.Double:
                    p.Set(UnitUtils.ConvertToInternalUnits(Convert.ToDouble(value),DisplayUnitType.DUT_MILLIMETERS));
                    break;
                case StorageType.ElementId:
                    p.Set((ElementId)value);
                    break;
                case StorageType.Integer:
                    p.Set(Convert.ToInt32(value));
                    break;
                case StorageType.String:
                    p.Set(Convert.ToString(value));
                    break;
                default: break;
            }
        }

        

    }

    internal class FamilyOptions : IFamilyLoadOptions
    {
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            if (familyInUse)
            {
                overwriteParameterValues = true;
                return true;
            }
            overwriteParameterValues = false;
            return true;
        }

        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
        {
            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
        }
    }
}
