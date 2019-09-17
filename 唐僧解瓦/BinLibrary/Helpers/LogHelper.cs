using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void LogWrite(string msg, string path)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(msg);
            sw.Close();
        }
    }
}
