using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using System.Linq;
using System.Windows.Forms;
using BinLibrary.RevitExtension;
using BinLibrary.WinHelper;

namespace BinLibrary.RevitHelper
{
    public static class FamilySymbolHelper
    {

        public static bool AddNewTypes_Duplicate(this FamilySymbol fs, Document doc, Dictionary<string, Dictionary<string, object>> paraDic/*字典里面包含参数名称 类型 值*/)
        {
            bool result = false;
            Document _doc = doc;
            //Document familyDoc = doc.EditFamily(fs.Symbol.Family);
            //取得当前族的所有族实例的名字
            ISet<ElementId> symbolIds = fs.Family.GetFamilySymbolIds();
            List<string> symbolnames = symbolIds.Select(m => m.GetElement(_doc).Name).ToList();
            Dictionary<string, FamilySymbol> symbolDic = new Dictionary<string, FamilySymbol>();

            symbolDic = symbolIds.ToDictionary(m => m.GetElement(_doc).Name, n => n.GetElement(_doc) as FamilySymbol);

            try
            {
                FamilySymbol _fs = fs;

                FamilySymbol newfs = null;//= fs.Duplicate($"{fs.Name}副本") as FamilySymbol;

                foreach (KeyValuePair<string, Dictionary<string, object>> kp in paraDic)
                {
                    try
                    {
                        //string tem = string.Join("\n", symbolnames);
                        //MessageBox.Show("line40:" + kp + Environment.NewLine );

                        if (symbolnames.Contains(kp.Key)) //如果文档中族类型包含excel表中的名字 则进行参数设置 否则新建族类型
                        {
                            foreach (KeyValuePair<string, object> kp1 in kp.Value)
                            {
                                //MessageBox.Show(kp.Key.ToString());
                                //MessageBox.Show(kp1.Key + Environment.NewLine);
                                //MessageBox.Show(kp1.Value.ToString());

                                Parameter pa = symbolDic[kp.Key].LookupParameter(kp1.Key);
                                if (kp1.Value != null&&pa!=null)
                                   pa.SetParaValue(kp1.Value.ToString());
                            }
                            //MessageBox.Show("contains");
                            continue;
                        }
                        //newfs = fs.Duplicate($"{fs.Name}副本") as FamilySymbol;
                         
                        newfs = _fs.Duplicate( Guid.NewGuid().ToString()) as FamilySymbol;
                        newfs.Name = kp.Key;

                        //MessageBox.Show("line62:"+kp.Value.Keys.ElementAt(0) + Environment.NewLine +
                        //                kp.Value.Keys.ElementAt(1));

                        foreach (KeyValuePair<string, object> kp1 in kp.Value)
                        {
                            //MessageBox.Show("kp.value" + kp.Key + Environment.NewLine +
                            //    kp1.Key + ":" + kp1.Value);

                            Parameter pa = newfs.LookupParameter(kp1.Key);
                            if (kp1.Value != null && pa != null)
                                pa.SetParaValue(kp1.Value.ToString());
                        }
                        result = true;
                    }
                    catch (Exception e)
                    {
                        //LogHelper.LogWrite(@"c:\圆洞口.txt", $"inner" + e.ToString());
                        //throw e;
                        MessageBox.Show(newfs.Name + ":参数值不存在或者参数类型错误");
                    }
                }

            }
            catch (Exception e)
            {
                //LogHelper.LogWrite(@"c:\圆洞口.txt", e.ToString());

                //throw e;
                //MessageBox.Show(e.ToString());
                //familyDoc.Close(false);
                result = false;
            }
            return result;
        }
    }
}
