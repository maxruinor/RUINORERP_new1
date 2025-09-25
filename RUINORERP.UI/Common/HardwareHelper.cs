using FastReport.DevComponents.Editors.DateTimeAdv;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public class HardwareInfo
    {
        private readonly ILogger<HardwareInfo> _logger;
        public HardwareInfo(ILogger<HardwareInfo> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 生成客户端ID
        /// 结合机器唯一标识、应用实例ID和进程ID确保唯一性
        /// </summary>
        /// <returns>唯一的客户端ID字符串</returns>
        public string GenerateClientId()
        {
            try
            {
                // 使用机器唯一标识、应用实例ID和进程ID组合生成唯一客户端ID
                string machineId = GetMachineUniqueId();
                string appInstanceId = GetApplicationInstanceId();
                string processId = Process.GetCurrentProcess().Id.ToString();

                // 客户端ID格式: {机器唯一标识}_{应用实例ID}_{进程ID}
                return $"{machineId}_{appInstanceId}_{processId}";
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "生成客户端ID失败，使用备用ID");
                // 出现异常时返回基于时间戳的备用ID
                return $"fallback_{DateTime.UtcNow.Ticks}";
            }
        }

        /// <summary>
        /// 获取机器唯一标识
        /// 优先使用Windows特定的标识方法
        /// </summary>
        /// <returns>机器唯一标识字符串</returns>
        private string GetMachineUniqueId()
        {
            try
            {
                // 在Windows环境下使用主板序列号或MAC地址
                return GetWindowsMachineId();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取Windows机器唯一标识失败，使用系统信息哈希");
                // 获取失败时返回基于系统信息的哈希值
                string systemInfo = $"{Environment.OSVersion}_{Environment.ProcessorCount}_{Environment.MachineName}";
                return systemInfo.GetHashCode().ToString("X");
            }
        }

        /// <summary>
        /// 获取Windows机器唯一标识
        /// 尝试从WMI获取主板序列号，如果失败则尝试获取MAC地址
        /// </summary>
        /// <returns>Windows机器唯一标识字符串</returns>
        private string GetWindowsMachineId()
        {
            try
            {
                // 使用WMI获取主板序列号
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                {
                    foreach (var queryObj in searcher.Get())
                    {
                        string serial = queryObj["SerialNumber"]?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(serial))
                        {
                            return Regex.Replace(serial, @"\s+", "");
                        }
                    }
                }

                _logger?.LogDebug("未获取到主板序列号，尝试获取MAC地址");
                // 如果获取主板序列号失败，尝试获取MAC地址
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up &&
                        !nic.Description.ToLower().Contains("virtual") &&
                        !nic.Description.ToLower().Contains("loopback"))
                    {
                        return nic.GetPhysicalAddress().ToString();
                    }
                }

                return "unknown";
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取Windows机器唯一标识失败");
                return "unknown";
            }
        }

        /// <summary>
        /// 获取应用实例ID
        /// 使用AppDomain的ID作为当前应用实例的标识
        /// </summary>
        /// <returns>应用实例ID字符串</returns>
        private string GetApplicationInstanceId()
        {
            try
            {
                // 使用AppDomain的ID作为实例标识
                return AppDomain.CurrentDomain.Id.ToString();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取应用实例ID失败，使用默认值");
                return "1";
            }
        }
    }
}
