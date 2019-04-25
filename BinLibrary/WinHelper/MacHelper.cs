using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BinLibrary.WinHelper
{
    public class MacHelper
    {
        public static List<string> GetMacByIPConfig()
        {
            List<string> list = new List<string>();
            Process process = Process.Start(new ProcessStartInfo("ipconfig", "/all")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            });
            StreamReader standardOutput = process.StandardOutput;
            string text = standardOutput.ReadLine();
            while (!standardOutput.EndOfStream)
            {
                bool flag = !string.IsNullOrEmpty(text);
                if (flag)
                {
                    text = text.Trim();
                    bool flag2 = text.StartsWith("Phisical Address") || text.StartsWith("物理地址");
                    if (flag2)
                    {
                        string[] array = text.Split(new char[]
                        {
                            ':'
                        });
                        list.Add(array[1].Trim());
                    }
                }
                text = standardOutput.ReadLine();
            }
            process.WaitForExit();
            process.Close();
            standardOutput.Close();
            return list;
        }

        public static List<string> GetMacByWMI()
        {
            List<string> list = new List<string>();
            try
            {
                //ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementClass managementClass = new ManagementClass(WMIPath.Win32_NetworkAdapterConfiguration.ToString());
                ManagementObjectCollection instances = managementClass.GetInstances();
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ManagementObject managementObject = (ManagementObject)enumerator.Current;
                        bool flag = (bool)managementObject["IPEnabled"];
                        if (flag)
                        {
                            string item = managementObject["MacAddress"].ToString();
                            list.Add(item);
                        }
                        managementObject.Dispose();
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        public static List<string> GetMacByNetWorkInterface()
        {
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            List<string> list = new List<string>();
            NetworkInterface[] array = allNetworkInterfaces;
            for (int i = 0; i < array.Length; i++)
            {
                NetworkInterface networkInterface = array[i];
                list.Add(networkInterface.GetPhysicalAddress().ToString());
            }
            return list;
        }

        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(int DestIP, int SrcIp, ref long MacAddr, ref int PhyAddrLen);

        [DllImport("Ws2_32.dll")]
        private static extern int inet_addr(string ipaddr);

        public static string GetMacBySendARP(string remoteIP)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                int destIP = MacHelper.inet_addr(remoteIP);
                long value = 0L;
                int num = 6;
                MacHelper.SendARP(destIP, 0, ref value, ref num);
                string text = Convert.ToString(value, 16).PadLeft(12, '0').ToUpper();
                MessageBox.Show(text);
                int num2 = 12;
                for (int i = 0; i < 6; i++)
                {
                    bool flag = i == 5;
                    if (flag)
                    {
                        stringBuilder.Append(text.Substring(num2 - 2, 2));
                    }
                    else
                    {
                        stringBuilder.Append(text.Substring(num2 - 2, 2) + "-");
                    }
                    num2 -= 2;
                }
            }
            catch
            {
            }
            return stringBuilder.ToString();
        }
    }
   
}
