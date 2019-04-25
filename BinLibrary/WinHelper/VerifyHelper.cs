using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinLibrary.WinHelper
{
    public class VerifyHelper
    {
        //public enum Verifiedcompyters
        //{
             
        //}

        public static IList<string> VerifiedMacs = new List<string>()
        {
            "30-5A-3A-54-85-01" /*zhuzan*/,
            "0C-9D-92-0D-62-F2" /*zhuzan*/,
            "E4-A4-71-D5-BC-19" /*liliangliang*/,
            "D4-81-D7-8A-E8-33",/*qijiwei*/
            "00-50-56-C0-00-08",/*bin*/
            "78-2B-CB-B8-82-19"
        };
    }

    class MacCheck
    {
        public static bool maccheck()
        {
            bool result = false;

            IList<string> mymacs = WinHelper.MacHelper.GetMacByIPConfig();
            string tem = "";

            foreach (string s in mymacs)
            {
                //MessageBox.Show(s.Length.ToString());
                //tem += s + Environment.NewLine;
                if (VerifyHelper.VerifiedMacs.Contains(s))
                {
                    result = true;
                    //MessageBox.Show("包含此电脑，授权使用");
                }
            }

            //tem += VerifyHelper.VerifiedMacs[3];

            //MessageBox.Show(tem);

            //MessageBox.Show(VerifyHelper.VerifiedMacs[3] );

            return result;
        }
    }
}
