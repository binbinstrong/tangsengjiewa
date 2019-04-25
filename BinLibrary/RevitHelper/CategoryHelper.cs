using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;

namespace BinLibrary.RevitHelper
{
   public class CategoryHelper
    {
        /// <summary>
        /// 设置类别的 对象样式 中的线条颜色
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="cate"></param>
        /// <param name="color"></param>
        public static void ChangeSubcategory(Document doc,Category cate ,Color color)
        {
            cate.LineColor = color;

            var subcategories = cate.SubCategories;
            foreach (object o in subcategories)
            {
                Category cate_in = o as Category;
                cate_in.LineColor =color;
            }
        }

    }
}
