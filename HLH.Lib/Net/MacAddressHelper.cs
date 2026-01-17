using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Diagnostics;

namespace HLH.Lib.Net
{
  
        /// <summary>
        /// MAC地址获取帮助类
        /// </summary>
        public class MacAddressHelper
        {
            ///<summary>
            /// 根据截取ipconfig /all命令的输出流获取网卡Mac，支持不同语言编码(容错性最好)（方法一）
            ///</summary>
            ///<returns></returns>
            /// <summary>
            /// 根据截取ipconfig /all命令的输出流获取网卡Mac，支持不同语言编码(容错性最好)
            /// </summary>
            /// <returns>MAC地址字符串，失败返回空字符串</returns>
            public string GetMacByIpConfig()
            {
                try
                {
                    List<string> macs = new List<string>();
                    var runCmd = ExecuteInCmd("chcp 437&&ipconfig/all");

                    if (string.IsNullOrEmpty(runCmd))
                    {
                        return string.Empty;
                    }

                    foreach (var line in runCmd.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()))
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line.StartsWith("Physical Address"))
                            {
                                if (line.Length > 36)
                                {
                                    macs.Add(line.Substring(36));
                                }
                            }
                            else if (line.StartsWith("DNS Servers") && line.Length > 36)
                            {
                                var substring = line.Substring(36);
                                if (substring.Contains("::"))
                                {
                                    macs.Clear();
                                }
                            }
                            else if (macs.Count > 0 && line.StartsWith("NetBIOS") && line.Contains("Enabled"))
                            {
                                return macs.Last();
                            }
                        }
                    }
                    return macs.FirstOrDefault() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
            /// <summary>
            /// 通过WMI读取系统信息里的网卡MAC
            /// </summary>
            /// <returns>MAC地址字符串，失败返回空字符串</returns>
            public string GetMacByWmi()
            {
                try
                {
                    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection moc = mc.GetInstances();
                    string macAddress = string.Empty;
                    foreach (ManagementObject mo in moc)
                    {
                        if (mo["IPEnabled"] != null && (bool)mo["IPEnabled"])
                        {
                            if (mo["MacAddress"] != null)
                            {
                                macAddress = mo["MacAddress"].ToString();
                                break;
                            }
                        }
                    }
                    return macAddress;
                }
                catch
                {
                    return string.Empty;
                }
            }
            /// <summary>
            /// 通过NetworkInterface读取网卡Mac
            /// </summary>
            /// <returns>MAC地址字符串，失败返回空字符串</returns>
            public string GetMacByNetworkInterface()
            {
                string key = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\";
                string macAddress = string.Empty;
                try
                {
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            string physicalAddress = adapter.GetPhysicalAddress().ToString();
                            if (!string.IsNullOrEmpty(physicalAddress))
                            {
                                string fRegistryKey = key + adapter.Id + "\\Connection";
                                RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                                if (rk != null)
                                {
                                    string fPnpInstanceID = rk.GetValue("PnpInstanceID", "")?.ToString() ?? string.Empty;
                                    if (fPnpInstanceID.Length > 3 && fPnpInstanceID.Substring(0, 3) == "PCI")
                                    {
                                        macAddress = physicalAddress;
                                        for (int i = 1; i < 6; i++)
                                        {
                                            macAddress = macAddress.Insert(3 * i - 1, ":");
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    return string.Empty;
                }
                return macAddress;
            }

            /// <summary>
            /// 执行内部命令（cmd.exe 中的命令）
            /// </summary>
            /// <param name="cmdline">命令行</param>
            /// <returns>执行结果</returns>
            public static string ExecuteInCmd(string cmdline)
            {
                try
                {
                    using (var process = new Process())
                    {
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardInput = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.CreateNoWindow = true;

                        process.Start();
                        process.StandardInput.AutoFlush = true;
                        process.StandardInput.WriteLine(cmdline + "&exit");

                        string output = process.StandardOutput.ReadToEnd();

                        process.WaitForExit();
                        process.Close();

                        return output;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
         
   
}
