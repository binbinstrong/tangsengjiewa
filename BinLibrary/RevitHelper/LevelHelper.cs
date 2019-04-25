using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;

namespace BinLibrary.RevitHelper
{
    public static class LevelHelper
    {
        //待测试
        /// <summary>
        /// 获取目标点最近的下方标高
        /// </summary>
        /// <param name="level"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Level GetNearestDownLevel(this Document _doc, XYZ location)
        {
            Level result = null;

            Document doc = _doc;// level.Document;

            var levels = (new FilteredElementCollector(doc)).OfClass(typeof(Level)).WhereElementIsNotElementType()
                .Select(m => m as Level).ToList();
            if (levels.Count == 0) return result;
            
            var levellist = levels.Where(m =>
                                        {
                                            return m.ProjectElevation < location.Z && Math.Abs(m.ProjectElevation - location.Z) > 1e-6;
                                        });
            if (levellist.Count() == 0) return result;

             
            //var max=levellist.Max(m => m.ProjectElevation);
            //result =levellist.Where(m => Math.Abs(m.ProjectElevation - max) < 1e-6).First();

            var abc = levellist.Aggregate((l1, l2) => l1.ProjectElevation > l2.ProjectElevation ? l1 : l2);
            result = abc;

            return result;
        }
        //待测试
        /// <summary>
        /// 获取目标点最近的上方标高
        /// </summary>
        /// <param name="level"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Level GetNearestUpLevel(this Document _doc, XYZ location)
        {
            Level result = null;

            Document doc =_doc;// = level.Document;

            var levels = (new FilteredElementCollector(doc).OfClass(typeof(Level)).WhereElementIsNotElementType()
                .Select(m => m as Level).ToList());
            if (levels.Count == 0) return result;

            var levellist = levels.Where(m =>
            {
                return m.ProjectElevation > location.Z && Math.Abs(m.ProjectElevation - location.Z) > 1e-6;
            });
            if (levellist.Count() == 0) return result;

            var abc = levellist.Aggregate((l1, l2) => l1.ProjectElevation < l2.ProjectElevation ? l1 : l2);
            result = abc;
            return result;
        }

        public static Level GetNearestUpLevel(this Document _doc, double location/*目标点的项目标高*/)
        {
            Level result = null;

            Document doc = _doc;// = level.Document;

            var levels = (new FilteredElementCollector(doc).OfClass(typeof(Level)).WhereElementIsNotElementType()
                .Select(m => m as Level).ToList());
            if (levels.Count == 0) return result;

            var levellist = levels.Where(m =>
            {
                return m.ProjectElevation > location && Math.Abs(m.ProjectElevation - location) > 1e-6;
            });
            if (levellist.Count() == 0) return result;

            var abc = levellist.Aggregate((l1, l2) => l1.ProjectElevation < l2.ProjectElevation ? l1 : l2);
            result = abc;
            return result;
        }
        /// <summary>
        /// 根据名字获取标高
        /// </summary>
        /// <param name="level"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Level GetLevelByName(this Document _doc, string name)
        {
            Level result = null;

            var doc = _doc;// level.Document;

            var levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Where(m => m.Name == name).ToList();
            if (levels.Count == 0) return result;

            result = levels.First() as Level;

            return result;
        }

    }
}
