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
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using BinLibrary.RevitExtension;

namespace BinLibrary.RevitHelper
{
    public static class FamilyInstanceHelper
    {
        /// <summary>
        /// 新建族类型
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="TypeName"></param>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FamilySymbol AddNewType(this FamilyInstance fi, string TypeName)
        {
            FamilySymbol result = null;
            Document doc = fi.Document;
            Document familyDoc = doc.EditFamily(fi.Symbol.Family);

            familyDoc.Invoke(m =>
            {
                familyDoc.SubtranInvoke(n =>
                {
                    FamilyManager fm = familyDoc.FamilyManager;

                    FamilyType newfamilytype = fm.NewType(TypeName);

                    //FamilyParameter fmp = fm.get_Parameter(paraName);
                    //if (fmp != null)
                    //{
                    //    ///写法不完善 需要区分参数值的存储类型（storagetype）
                    //    fm.Set(fmp, (double)value);
                    //}
                    familyDoc.LoadFamily(doc, new FamilyOptions());
                });

            }, "添加类型");

            familyDoc.Close(false);

            return result;
        }
        /// <summary>
        /// 新建族类型（通过复制方式创建）
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public static FamilySymbol AddNewType_duplicate(this FamilyInstance fi, string TypeName)
        {
            FamilySymbol result = null;
            Document doc = fi.Document;

            doc.Invoke(m =>
            {
                result = fi.Symbol.Duplicate(TypeName) as FamilySymbol;
            }, "新建类型");

            return result;
        }


        /// <summary>
        /// 为族新建类型
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="TypeName"></param>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FamilySymbol AddNewType(this FamilyInstance fi, string TypeName, string paraName, object value)
        {
            FamilySymbol result = null;
            Document doc = fi.Document;
            Document familyDoc = doc.EditFamily(fi.Symbol.Family);

            familyDoc.Invoke(m =>
            {
                familyDoc.SubtranInvoke(n =>
                {
                    FamilyManager fm = familyDoc.FamilyManager;

                    FamilyType newfamilytype = fm.NewType(TypeName);

                    FamilyParameter fmp = fm.get_Parameter(paraName);
                    if (fmp != null)
                    {
                        ///写法不完善 需要区分参数值的存储类型（storagetype）
                        fm.Set(fmp, (double)value);
                    }
                    familyDoc.LoadFamily(doc, new FamilyOptions());
                });

            }, "添加类型");

            familyDoc.Close(false);

            return result;
        }

        /// <summary>
        /// 新建族类型
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="TypeName"></param>
        /// <param name="paraNameTypeValue"></param>
        /// <returns></returns>
        public static bool AddNewType(this FamilyInstance fi, string TypeName, Dictionary<string, object> paraDic/*字典里面包含参数名称 类型 值*/)
        {
            bool result = false;
            Document doc = fi.Document;
            Document familyDoc = doc.EditFamily(fi.Symbol.Family);

            try
            {
                familyDoc.Invoke(m =>
                {
                    familyDoc.SubtranInvoke(n =>
                    {
                        FamilyManager fm = familyDoc.FamilyManager;

                        //判断TypeName是否存在
                        bool isExist = false;
                        foreach (FamilyType familyType in fm.Types)
                        {
                            if (familyType.Name == TypeName)
                            {
                                isExist = true;
                                break;
                            }
                        }

                        FamilyType newfamilytype = null;
                        if (!isExist)
                        {
                            newfamilytype = fm.NewType(TypeName);
                        }
                        else
                        {
                            result = false;
                        }

                        fm.CurrentType = newfamilytype;

                        foreach (KeyValuePair<string, object> kp in paraDic)
                        {
                            FamilyParameter fmp = fm.get_Parameter(kp.Key);

                            if (fmp != null)
                            {
                                fmp.SetParaValue(fm, kp.Value);

                            }
                        }

                        //FamilyParameter fmp = fm.get_Parameter(paraName);
                        //if (fmp != null)
                        //{
                        //    fm.Set(fmp, (double)value);
                        //}
                        familyDoc.LoadFamily(doc, new FamilyOptions());
                        result = true;
                    });

                }, "添加类型");

                familyDoc.Close(false);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                familyDoc.Close(false);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 新建一组族类型(遇见重名的直接跳过)
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="paraDic"></param>
        /// <returns></returns>
        [Obsolete]
        public static bool AddNewTypes(this FamilyInstance fi, Dictionary<string, Dictionary<string, KeyValuePair<StorageType, object>>> paraDic/*字典里面包含参数名称 类型 值*/)
        {
            bool result = false;
            Document doc = fi.Document;
            Document familyDoc = doc.EditFamily(fi.Symbol.Family);


            try
            {

                familyDoc.Invoke(m =>
                {
                    familyDoc.SubtranInvoke(n =>
                    {
                        FamilyManager fm = familyDoc.FamilyManager;

                        //判断TypeName是否存在

                        foreach (KeyValuePair<string, Dictionary<string, KeyValuePair<StorageType, object>>> kpOuter in paraDic)
                        {

                            string TypeName = kpOuter.Key;

                            bool isExist = false;

                            foreach (FamilyType familyType in fm.Types)
                            {
                                if (familyType.Name == TypeName)
                                {
                                    isExist = true;
                                    break;
                                }
                            }

                            FamilyType newfamilytype = null;
                            if (!isExist)
                            {
                                newfamilytype = fm.NewType(TypeName);
                            }
                            else
                            {

                                result = false;
                                continue;
                            }

                            fm.CurrentType = newfamilytype;

                            //MessageBox.Show(kpOuter.Value.Count.ToString());

                            foreach (KeyValuePair<string, KeyValuePair<StorageType, object>> kp in kpOuter.Value)
                            {
                                FamilyParameter fmp = fm.get_Parameter(kp.Key);

                                //MessageBox.Show(kp.Key+":"+kp.Value.Key);

                                if (fmp != null)
                                {

                                    //fmp.SetParaValue(fm,kp.Value.Value);

                                    switch (kp.Value.Key)
                                    {
                                        case StorageType.ElementId:
                                            fm.Set(fmp, (ElementId)kp.Value.Value);
                                            break;
                                        case StorageType.Double:
                                            fm.Set(fmp, (double)kp.Value.Value);
                                            break;
                                        case StorageType.Integer:
                                            fm.Set(fmp, (int)kp.Value.Value);
                                            break;
                                        case StorageType.String:
                                            fm.Set(fmp, (string)kp.Value.Value);
                                            break;
                                        default: break;
                                    }
                                }
                            }

                        }


                        //FamilyParameter fmp = fm.get_Parameter(paraName);
                        //if (fmp != null)
                        //{
                        //    fm.Set(fmp, (double)value);
                        //}
                        familyDoc.LoadFamily(doc, new FamilyOptions());
                        result = true;
                    });

                }, "添加类型");

                familyDoc.Close(false);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                familyDoc.Close(false);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 新建一组族类型(通过修改族的方式)
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="paraDic"></param>
        /// <returns></returns>
        public static bool AddNewTypes(this FamilyInstance fi, Dictionary<string, Dictionary<string, object>> paraDic/*字典里面包含参数名称 类型 值*/)
        {

            bool result = false;
            Document doc = fi.Document;
            Document familyDoc = doc.EditFamily(fi.Symbol.Family);


            try
            {

                familyDoc.Invoke(m =>
                {
                    familyDoc.SubtranInvoke(n =>
                    {
                        FamilyManager fm = familyDoc.FamilyManager;

                        //判断TypeName是否存在


                        foreach (KeyValuePair<string, Dictionary<string, object>> kpOuter in paraDic)
                        {

                            string TypeName = kpOuter.Key;

                            bool isExist = false;

                            foreach (FamilyType familyType in fm.Types)
                            {
                                if (familyType.Name == TypeName)
                                {
                                    isExist = true;
                                    break;
                                }
                            }

                            FamilyType newfamilytype = null;
                            if (!isExist)
                            {
                                newfamilytype = fm.NewType(TypeName);
                            }
                            else
                            {

                                result = false;
                                continue;
                            }

                            fm.CurrentType = newfamilytype;

                            //MessageBox.Show(kpOuter.Value.Count.ToString());

                            foreach (KeyValuePair<string, object> kp in kpOuter.Value)
                            {
                                FamilyParameter fmp = fm.get_Parameter(kp.Key);

                                //MessageBox.Show(kp.Key+":"+kp.Value.Key);

                                if (fmp != null)
                                {

                                    fmp.SetParaValue(fm, kp.Value);

                                }
                            }

                        }


                        //FamilyParameter fmp = fm.get_Parameter(paraName);
                        //if (fmp != null)
                        //{
                        //    fm.Set(fmp, (double)value);
                        //}
                        familyDoc.LoadFamily(doc, new FamilyOptions());
                        result = true;
                    });

                }, "添加类型");

                familyDoc.Close(false);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                familyDoc.Close(false);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 通过复制的方式新建类型（已测试 这种方式比编辑族的方式速度快）
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="paraDic"></param>
        /// <returns></returns>
        public static bool AddNewTypes_Duplicate(this FamilyInstance fi, Dictionary<string, Dictionary<string, object>> paraDic/*字典里面包含参数名称 类型 值*/)
        {
            bool result = false;
            Document doc = fi.Document;
            //Document familyDoc = doc.EditFamily(fi.Symbol.Family);
            //取得当前族的所有族实例的名字
            Family fm = fi.Symbol.Family;
            ISet<ElementId> symbolIds = fm.GetFamilySymbolIds();
            List<string> symbolnames = symbolIds.Select(m => m.GetElement(doc).Name).ToList();
            Dictionary<string, FamilySymbol> symbolDic = new Dictionary<string, FamilySymbol>();

            symbolDic = symbolIds.ToDictionary(m => m.GetElement(doc).Name, n => n.GetElement(doc) as FamilySymbol);

            #region dimed

            //foreach (ElementId id in symbolIds)
            //{
            //    symbolDic.Add(id.GetElement(doc).Name, id.GetElement(doc) as FamilySymbol);
            //}

            #endregion
            try
            {
                doc.Invoke(m =>
                      {
                          FamilySymbol fs = fi.Symbol;

                          FamilySymbol newfs = null;//= fs.Duplicate($"{fi.Name}副本") as FamilySymbol;

                          foreach (KeyValuePair<string, Dictionary<string, object>> kp in paraDic)
                          {
                              try
                              {
                                  if (symbolnames.Contains(kp.Key))
                                  {
                                      foreach (KeyValuePair<string, object> kp1 in kp.Value)
                                      {
                                          //MessageBox.Show(kp.Key.ToString());
                                          //MessageBox.Show(kp1.Key + Environment.NewLine);
                                          //MessageBox.Show(kp1.Value.ToString());
                                          if (kp1.Value != null)
                                              symbolDic[kp.Key].LookupParameter(kp1.Key).SetParaValue(kp1.Value.ToString());
                                      }
                                      continue;
                                  }
                                  //newfs = fs.Duplicate($"{fi.Name}副本") as FamilySymbol;
                                  newfs = fs.Duplicate((new Guid()).ToString()) as FamilySymbol;
                                  newfs.Name = kp.Key;

                                  foreach (KeyValuePair<string, object> kp1 in kp.Value)
                                  {
                                      //MessageBox.Show(kp.Key + Environment.NewLine +
                                      //    kp1.Key + ":" + kp1.Value);
                                      if (kp1.Value != null)
                                          newfs.LookupParameter(kp1.Key).SetParaValue(kp1.Value.ToString());
                                  }
                              }
                              catch (Exception e)
                              {
                                  //MessageBox.Show(newfs.Name + ":参数值不存在或者参数类型错误");
                              }
                          }
                      }, "新建类型");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                //familyDoc.Close(false);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 通过复制的方式新建类型 不带事务
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="doc"></param>
        /// <param name="paraDic"></param>
        /// <returns></returns>
        public static bool AddNewTypes_Duplicate(this FamilyInstance fi,Document doc, Dictionary<string, Dictionary<string, object>> paraDic/*字典里面包含参数名称 类型 值*/)
        {
            bool result = false;
            Document _doc = doc;
            //Document familyDoc = doc.EditFamily(fi.Symbol.Family);
            //取得当前族的所有族实例的名字
            Family fm = fi.Symbol.Family;
            ISet<ElementId> symbolIds = fm.GetFamilySymbolIds();
            List<string> symbolnames = symbolIds.Select(m => m.GetElement(_doc).Name).ToList();
            Dictionary<string, FamilySymbol> symbolDic = new Dictionary<string, FamilySymbol>();

            symbolDic = symbolIds.ToDictionary(m => m.GetElement(_doc).Name, n => n.GetElement(_doc) as FamilySymbol);

            #region dimed

            //foreach (ElementId id in symbolIds)
            //{
            //    symbolDic.Add(id.GetElement(doc).Name, id.GetElement(doc) as FamilySymbol);
            //}

            #endregion
            try
            {
               
                    FamilySymbol fs = fi.Symbol;

                    FamilySymbol newfs = null;//= fs.Duplicate($"{fi.Name}副本") as FamilySymbol;

                    foreach (KeyValuePair<string, Dictionary<string, object>> kp in paraDic)
                    {
                        try
                        {
                            if (symbolnames.Contains(kp.Key))
                            {
                                foreach (KeyValuePair<string, object> kp1 in kp.Value)
                                {
                                //MessageBox.Show(kp.Key.ToString());
                                //MessageBox.Show(kp1.Key + Environment.NewLine);
                                //MessageBox.Show(kp1.Value.ToString());

                                    Parameter pa = symbolDic[kp.Key].LookupParameter(kp1.Key);
                                    if (kp1.Value != null && pa != null)
                                        pa.SetParaValue(kp1.Value.ToString());
                                     
                                }
                                continue;
                            }
                            //newfs = fs.Duplicate($"{fi.Name}副本") as FamilySymbol;
                            newfs = fs.Duplicate((new Guid()).ToString()) as FamilySymbol;
                            newfs.Name = kp.Key;

                            foreach (KeyValuePair<string, object> kp1 in kp.Value)
                            {
                                //MessageBox.Show(kp.Key + Environment.NewLine +
                                //    kp1.Key + ":" + kp1.Value);
                                Parameter pa = newfs.LookupParameter(kp1.Key);
                                if (kp1.Value != null&&pa!=null)
                                   pa.SetParaValue(kp1.Value.ToString());
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(newfs.Name + ":参数值不存在或者参数类型错误");
                        }
                    }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                //familyDoc.Close(false);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 移除族的类型（从族文档中）
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool RemoveFamilyTypeOfFamily(this FamilyInstance fi, string typeName)
        {
            bool result = false;
            Document doc = fi.Document;
            Document familyDoc = doc.EditFamily(fi.Symbol.Family);

            try
            {
                familyDoc.Invoke(m =>
                {
                    familyDoc.SubtranInvoke(n =>
                    {
                        FamilyManager fm = familyDoc.FamilyManager;


                        //FamilyType newfamilytype = fm.NewType(TypeName);
                        FamilyTypeSet familytypeset = fm.Types;

                        FamilyType tarft = null; //targetFamilyType

                        FamilyTypeSetIterator iter = familytypeset.ForwardIterator();


                        iter.Reset();

                        while (iter.MoveNext())
                        {
                            FamilyType ft = iter.Current as FamilyType;
                            MessageBox.Show(ft.Name);

                            if (ft.Name == typeName)
                            {
                                tarft = (ft);
                                break;
                                MessageBox.Show("find target");
                            }
                        }

                        if (tarft != null)
                        {
                            fm.CurrentType = tarft;
                            fm.DeleteCurrentType();

                            result = true;
                        }

                        //FamilyParameter fmp = fm.get_Parameter(paraName);
                        //if (fmp != null)
                        //{
                        //    fm.Set(fmp, (double)value);
                        //}

                        familyDoc.LoadFamily(doc, new FamilyOptions());
                        //result = true;
                    });

                }, "删除类型");

                familyDoc.Close(false);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                familyDoc.Close(false);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 删除族类型(方法是从当前项目中删除族的symbol)
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool RemoveFamilyTypeOfDoc(this FamilyInstance fi, string typeName)
        {
            bool result = false;
            Document doc = fi.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);


            Element ele = collector.OfClass(typeof(FamilySymbol)).Where(m => m.Name == typeName).First();

            MessageBox.Show(ele.Name);

            try
            {
                Transaction ts = new Transaction(doc, (new Guid()).ToString());
                ts.Start();

                doc.Delete(ele.Id);
                ts.Commit();

                result = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                result = false;
            }
            return result;
        }
    }
}
