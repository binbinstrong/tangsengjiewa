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
using System.Diagnostics;
using System.IO;
using System.Text;
using Autodesk.Revit.DB;
namespace BinLibrary.WinHelper
{
    public static class LogHelper
    {
        public static void LogWrite(string path, string text)
        {
            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(text + "::" + DateTime.Now);
            sw.Close();
        }
        /// <summary>
        /// 是否添加时间
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <param name="tof"></param>
        public static void LogWrite(string path, string text, bool tof = true /*append or not*/)
        {
            StreamWriter sw = new StreamWriter(path, true);
            if (tof)
            {
                sw.WriteLine(text + "::" + DateTime.Now);
            }
            else
            {
                sw.WriteLine(text);
            }
            sw.Close();
        }
        public static string LogRead(string path, string text)
        {
            StreamReader sr = new StreamReader(path, true);
            return sr.ReadLine();
        }
        /// <summary>
        /// 异常捕捉记录
        /// </summary>
        /// <param name="action"></param>
        /// <param name="path"></param>
        public static void LogException(Action action, string path = @"c:\revitExceptionlog.txt")
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                LogWrite(path, "Exceptionlog:" + Environment.NewLine + action.Method.Name + "///错误信息是///" + Environment.NewLine + e.ToString());
            }
        }
        public static void LogException(Action action,Action action1, string path = @"c:\revitExceptionlog.txt")
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                action1();
                LogWrite(path, "Exceptionlog:" + Environment.NewLine + action.Method.Name + "///错误信息是///" + Environment.NewLine + e.ToString());
            }
        }
        /// <summary>
        /// 耗时记录
        /// </summary>
        /// <param name="action"></param>
        /// <param name="path"></param>
        public static void LogTimer(Action action, string path = @"c:\revitTimerlog.txt")
        {
            Stopwatch watch = new Stopwatch();
            var starttime = DateTime.Now;
            watch.Start();
            action();
            watch.Stop();
            var stoptime = DateTime.Now;
#if DEBUG
            LogWrite(path,
                "TimerLog:" + Environment.NewLine + action.Method.Name +Environment.NewLine+$"开始于{starttime},结束于{stoptime};"+ "///共耗时:///" + watch.Elapsed.Milliseconds.ToString() + "毫秒");
#endif
        }

        /// <summary>
        /// 默认文件记录信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="path"></param>
        public static void LogMessage(string message, string path = @"c:\revitMsglog.txt")
        {
#if DEBUG
            LogWrite(path, message, false);
#endif
        }
    }
}
