using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//获取路径类 
namespace BinLibrary
{
    public static class PathHelper
    {
        /// <summary>
        /// 获取当前dll路径
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static string GetWorkPath(Assembly asm)
        {
            return Path.GetDirectoryName(asm.Location);
        }

        /// <summary>
        /// 获取系统公共存储库文件夹路径 c:/ProgramData
        /// </summary>
        /// <returns></returns>
        public static string GetCommonAppPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        }

        /// <summary>
        /// 获取用户程序存储库文件夹 user/用户名/Appdata/Roaming/
        /// </summary>
        /// <returns></returns>
        public static string GetUserAppPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
         
        /// <summary>
        /// 获取当前路径父路径
        /// </summary>
        /// <param name="path">path必须是一个路径而不是文件名</param>
        /// <returns></returns>
        public static string GetFatherPath(string path)
        {
            string resultPath = string.Empty;
            List<int> Indexofseperator = new List<int>();

            char[] chararray = new char[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                //Console.WriteLine(path[i]);
                if (path[i] == '/' || path[i] == '\\')
                {
                    Indexofseperator.Add(i);
                }
            }

            int a = path.LastIndexOf("/");
            int b = path.LastIndexOf("\\");
            if (a == path.Length - 1 || b == path.Length - 1)
            {
                resultPath = path.Substring(0, Indexofseperator[Indexofseperator.Count - 2]);
            }
            else
            {
                resultPath = path.Substring(0, Indexofseperator[Indexofseperator.Count - 1]);
            }
            return resultPath;

        }

        /// <summary>
        /// 由文件名直接获取父路径
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="isfilename"></param>
        /// <returns></returns>
        public static string GetFatherPath(string filename, bool isfilename)
        {
            string resultPath = string.Empty;
            List<int> Indexofseperator = new List<int>();

            //char[] chararray = new char[filename.Length];
            for (int i = 0; i < filename.Length; i++)
            {
                //Console.WriteLine(path[i]);
                if (filename[i] == '/' || filename[i] == '\\')
                {
                    Indexofseperator.Add(i);

                }
            }
            
            resultPath = filename.Substring(0, Indexofseperator[Indexofseperator.Count - 2]);

            return resultPath;

        }
    }

}
