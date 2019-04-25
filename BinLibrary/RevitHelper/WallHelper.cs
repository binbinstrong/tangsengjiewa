using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace BinLibrary.RevitHelper
{
    /*WallUtils 用于判断墙端点是否允许连接 设置墙的端点是否允许连接等
     * 找到与端点连接的元素 用 LocationCurve.ElementsAtJoin 属性来实现
     */

    /// <summary>
    /// 墙帮助类 用于判断墙的连接等操作
    /// </summary>
    public static class WallHelper
    {
        /// <summary>
        /// 找到与墙的端点相连的墙
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        public static List<Wall> GetJoinedWall(this Wall wall)
        {
            List<Wall> result = new List<Wall>();

            var locationcurve = (wall.Location as LocationCurve);

            //判断墙在端点0处是否有允许连接 如果允许连接 找到与之相连的元素
            ElementArray list1 = new ElementArray();
            if (WallUtils.IsWallJoinAllowedAtEnd(wall, 0))
                list1 = locationcurve.get_ElementsAtJoin(0);
            //判断墙在端点1处是否有允许连接 如果允许连接 找到与之相连的元素
            ElementArray list2 = new ElementArray();
            if (WallUtils.IsWallJoinAllowedAtEnd(wall, 1))
                list2 = locationcurve.get_ElementsAtJoin(1);

            var enu2 = list2.GetEnumerator();
            while (enu2.MoveNext())
            {
                var element = enu2.Current as Element;
                if (element is Wall)
                {
                    list1.Append(element);
                }
            }

            var enu1 = list1.GetEnumerator();

            while (enu1.MoveNext())
            {
                var element = enu1.Current as Element;
                if (element is Wall)
                {
                    result.Add(element as Wall);
                }
            }
            return result;
        }


        /// <summary>
        /// 判断两面墙是否相连
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="wall1"></param>
        /// <returns></returns>
        public static bool IsWallJoinedWith(this Wall wall, Wall wall1)
        {
            var joinedWalls = wall.GetJoinedWall();
            if (joinedWalls.Contains(wall1))
            {
                return true;
            }
            return false;
        }

    }
}
