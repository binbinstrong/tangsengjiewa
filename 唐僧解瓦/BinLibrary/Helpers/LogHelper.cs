using System;
using System.IO;

namespace 唐僧解瓦.BinLibrary.Helpers
{
    public static class LogHelper
    {
        public static void LogException(Action action,string path)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                LogWrite(e.ToString(),path);
            }
        }

        public static void LogWrite(string msg, string path,bool append= false)
        {
            StreamWriter sw = new StreamWriter(path, append);
            sw.WriteLine(msg);
            sw.Close();
        }
    }
}
