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
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BinLibrary.RevitHelper
{
    public static class ModelLineHelper
    {

/*在revit2017以后的版本可以设置线样式，在revit2016里面不可以设置线的样式（至少是没找到方法）
 在revit2017中设置线样式的方法案例为 newCategory.SetLinePatternId(LinePatternElementId,GraphicStyleType.Projection)*/


        /// <summary>
        /// 创建线类型带事务
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static Category CreateNewLineCategory(Document doc,string newCategoryName/*新线型名字*/,Color color )
        {
            Category result = null;
            color = new Color(244, 255, 0);

            Category newCategory = null;

            Transaction ts = new Transaction(doc,"新建线类型");
            ts.Start();

            //parentCategory
            Category parentCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

            newCategory = doc.Settings.Categories.NewSubcategory(parentCategory, newCategoryName);

            newCategory.LineColor = color;

             
            result = newCategory;

            ts.Commit();



            return result;
        }

        public static LinePatternElement CreateLinePatternElement(Document doc, string LinePatternName)
        {
            //LinePatternElement result = null;
            List<LinePatternSegment> segments = new List<LinePatternSegment>();
            segments.Add(new LinePatternSegment(LinePatternSegmentType.Dot, 0.1));
            segments.Add(new LinePatternSegment(LinePatternSegmentType.Dash, 0.02));
            segments.Add(new LinePatternSegment(LinePatternSegmentType.Space, 0.02));
            segments.Add(new LinePatternSegment(LinePatternSegmentType.Dash, 0.02));

            LinePattern linePattern = new LinePattern(LinePatternName);
            linePattern.SetSegments(segments);
            LinePatternElement linepatternelement = LinePatternElement.Create(doc, linePattern);

            return linepatternelement;
        }

        /// <summary>
        /// 创建新的线型
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="newLineStyleName"></param>
        /// <returns></returns>
        public static Category CreateLineStyle(Document doc, string newLineStyleName/*is a sub category*/)
        {
            Category result = null;
            Category lineCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            Category newLineCategory = doc.Settings.Categories.NewSubcategory(lineCategory, newLineStyleName);
            result = newLineCategory;

          
            return result;
        }

        public static Category CreateLineStyle(Document doc, string NewlineStyleName, Color color)
        {
            Category newLineCategory = CreateLineStyle(doc, NewlineStyleName);
            newLineCategory.LineColor = color;
            return newLineCategory;
        }


    }
}
