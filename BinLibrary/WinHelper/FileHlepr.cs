using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinLibrary;
using BinLibrary.WinApi;

namespace BinLibrary.FileHelper
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public static class FileOperation
    {
        /// <summary>
        /// 获取文件缩略图
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="smallIcon"></param>
        /// <returns></returns>
        public static Icon GetFileIcon(string fileName, bool smallIcon)
        {

            WinApi.WinApi.SHFILEINFO fi = new WinApi.WinApi.SHFILEINFO();
            Icon ic = null;
            int iTotal = (int)WinApi.WinApi.SHGetFileInfo(fileName, 100, ref fi, 0, (uint)(smallIcon ? 273 : 272));
            if (iTotal > 0)
            {
                ic = Icon.FromHandle(fi.hIcon);
            }
            return ic;
        }

        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;

        }

    }
}
