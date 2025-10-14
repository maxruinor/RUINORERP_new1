using Microsoft.Extensions.Logging;
using System;
using System.Management;
using System.Text;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 硬件信息服务实现
    /// 提供系统硬件信息的获取功能
    /// </summary>
    public class HardwareInfoService : IHardwareInfoService
    {
        private readonly ILogger<HardwareInfoService> _logger;

        public HardwareInfoService(ILogger<HardwareInfoService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取唯一的硬件信息
        /// </summary>
        /// <returns>硬件信息字符串</returns>
        public string GetUniqueHardwareInfo()
        {
            try
            {
                string hardDiskId = GetDiskInfo();
                string macAddress = GetMacAddress();
                return hardDiskId + macAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取硬件信息失败");
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns>CPU信息</returns>
        private string GetCpuInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["ProcessorId"]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取CPU信息失败");
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取主板信息
        /// </summary>
        /// <returns>主板信息</returns>
        private string GetMotherboardInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取主板信息失败");
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取硬盘信息
        /// </summary>
        /// <returns>硬盘信息</returns>
        private string GetDiskInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive WHERE MediaType='Fixed hard disk media'"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取硬盘信息失败");
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取MAC地址
        /// </summary>
        /// <returns>MAC地址</returns>
        private string GetMacAddress()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT MACAddress FROM Win32_NetworkAdapter WHERE NetConnectionStatus=2"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["MACAddress"]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取MAC地址失败");
            }
            return string.Empty;
        }
    }
}