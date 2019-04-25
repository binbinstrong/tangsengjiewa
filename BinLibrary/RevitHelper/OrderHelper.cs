using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using BinLibrary.RevitExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BinLibrary.RevitHelper
{
    public static class OrderHelper
    {
        public static List<Element> Order_cus(List<Element> list, XYZ po)
        {
            bool flag = list.Count < 2;
            List<Element> result;
            if (flag)
            {
                result = list;
            }
            else
            {
                List<Element> list2 = new List<Element>();
                List<Element> list3 = new List<Element>();
                List<Element> list4 = new List<Element>();
                list3.AddRange(list);
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    Element element = list3.ElementAt(i);
                    Line xline = (element.Location as LocationCurve).Curve as Line;
                    double num = po.DistanceTo_Hosizontal(xline);
                    int index = i;
                    for (int j = i + 1; j < count; j++)
                    {
                        Element element2 = list3.ElementAt(j);
                        Line xline2 = (element2.Location as LocationCurve).Curve as Line;
                        double num2 = po.DistanceTo_Hosizontal(xline2);
                        bool flag2 = num > num2;
                        if (flag2)
                        {
                            num = num2;
                            index = j;
                        }
                    }
                    list2.Add(list3.ElementAt(index));
                    list3.RemoveAt(index);
                    list3.Insert(i, list2.ElementAt(i));
                }
                result = list3;
            }
            return result;
        }

        public static List<Element> Order_cus1(List<Element> list, XYZ po)
        {
            if (list.Count < 2) return list;
            var doc = list.ElementAt(0).Document;
            var elemetnDic = list.ToDictionary(m => m.Id, n => po.DistanceTo_Hosizontal((n as Pipe).LocationLine()));
            var newdic = elemetnDic.OrderBy(m => m.Value);
            var result = newdic.Select(m => m.Key.GetElement(doc)).ToList();
            return result; 
        }

        public static List<Element> Order_cus2(List<Element> list, XYZ po)
        {
            if (list.Count < 2) return list;
            var doc = list.ElementAt(0).Document;
            var result = list.OrderBy(m => po.DistanceTo_Hosizontal((m as Pipe).LocationLine()));
            return result.ToList();
        }
    }
}
