using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BinLibrary.RevitHelper
{
    public static class ElementHelper
    {
        /// <summary>
        /// 参数设置（如果参数字典 paras 正确的个数大于错误个数则 结果包含true 否则包含false)
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static RightWrong SetParameters(this Element ele, Dictionary<string, object> paras)
        {
            RightWrong result = new RightWrong(){result = false,rightNum = 0,WrongNum = 0};
            //Document doc = ele.Document;
            //UIDocument uidoc = new UIDocument(doc);
            ParameterSet paraset = ele.Parameters;
            Dictionary<string, Parameter> paradic = paraset.GetParameterNameDic();//参数名重复的情况未处理 应该用内建参数来处理

            foreach (KeyValuePair<string, object> kp in paras)
            {
                if (paradic.ContainsKey(kp.Key))
                {
                    Parameter p = paradic[kp.Key];
                    p.SetParaValue(kp.Value);
                    result.rightNum++;
                }
                else
                {
                    result.WrongNum++;
                }
            }
            if (result.rightNum>result.WrongNum)
            {
                result.result = true;
            }
            else
            {
                result.result = false;
            }
            return result;
        }
        

       public  struct RightWrong
        {
            public bool result;
            public int rightNum;
            public int WrongNum;
        }

        /// <summary>
        /// 元素集合去重的比较 用于distinct方法
        /// </summary>
        public class ElementEqualityComParer : IEqualityComparer<Element>
        {
            
            public bool Equals(Element x, Element y)
            {
                return x.Id.IntegerValue == y.Id.IntegerValue;
            }

            public int GetHashCode(Element obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
