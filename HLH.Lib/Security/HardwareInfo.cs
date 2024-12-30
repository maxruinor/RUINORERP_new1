using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HLH.Lib.Security
{
    public class HardwareInfo
    {

        public string GetCpuId()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    return mo.Properties["ProcessorId"].Value.ToString();
                }
                return "Not Available";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while retrieving CPU ID: " + ex.Message);
                return "Error";
            }
        }

        public string GetHardDiskId()
        {
            try
            {
                ManagementClass diskDrive = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection diskDrives = diskDrive.GetInstances();
                foreach (ManagementObject drive in diskDrives)
                {
                    // 获取硬盘的SerialNumber属性
                    string serialNumber = drive.Properties["SerialNumber"].Value?.ToString();
                    if (!string.IsNullOrEmpty(serialNumber))
                    {
                        // 注意：有时SerialNumber可能包含非数字字符，需要根据实际情况处理
                        return serialNumber.Trim();
                    }
                }
                return "Not Available";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while retrieving Hard Disk ID: " + ex.Message);
                return "Error";
            }
        }

        /*
          //获取CPU ID
    string CPU_ID = GetComputerHardWareInfo("Win32_Processor", "ProcessorId");
    //获取主板序列号
    string Board_SN = GetComputerHardWareInfo("Win32_BaseBoard", "SerialNumber");
    //获取硬盘序列号
    string Disk_SN = GetComputerHardWareInfo("Win32_DiskDrive", "Model");
    //获取UUID
    string UUID = GetComputerHardWareInfo("Win32_ComputerSystemProduct", "UUID");
         */
        public string GetComputerHardWareInfo(string path, string key)
        {
            try
            {
                ManagementClass managementClass = new ManagementClass(path);
                ManagementObjectCollection moc = managementClass.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    return mo.Properties[key].Value.ToString();
                }
            }
            catch
            {
                //记录异常信息
            }
            return string.Empty;
        }
    }
}
