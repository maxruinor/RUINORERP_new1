/*****************************************************************************************
 * 文件名称：ClientSystemInfoService.cs
 * 创建人员：RUINOR ERP系统
 * 创建时间：2024年
 * 文件描述：客户端系统信息收集服务，提供完整的客户端系统、硬件、环境信息收集功能
 * 版权所有：RUINOR ERP系统
 *****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Common.DI;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.UI.Services
{
    /// <summary>
    /// 客户端系统信息收集服务接口
    /// 定义客户端系统信息收集的标准接口
    /// </summary>
    public interface IClientSystemInfoService : IDependencyService
    {
        /// <summary>
        /// 获取完整的客户端系统信息（同步版本）
        /// </summary>
        /// <returns>客户端系统信息对象</returns>
        ClientSystemInfo GetClientSystemInfo();

        /// <summary>
        /// 获取完整的客户端系统信息（异步版本）
        /// </summary>
        /// <returns>客户端系统信息对象</returns>
        Task<ClientSystemInfo> GetClientSystemInfoAsync();

        /// <summary>
        /// 获取操作系统信息
        /// </summary>
        /// <returns>操作系统信息对象</returns>
        OperatingSystemInfo GetOperatingSystemInfo();

        /// <summary>
        /// 获取硬件信息
        /// </summary>
        /// <returns>硬件信息对象</returns>
        HardwareInfo GetHardwareInfo();

        /// <summary>
        /// 获取环境信息
        /// </summary>
        /// <returns>环境信息对象</returns>
        EnvironmentInfo GetEnvironmentInfo();

        /// <summary>
        /// 获取网络信息
        /// </summary>
        /// <returns>网络信息对象</returns>
        NetworkInfo GetNetworkInfo();

        /// <summary>
        /// 获取系统资源使用情况
        /// </summary>
        /// <returns>资源使用信息对象</returns>
        ClientResourceUsage GetResourceUsage();
    }

    /// <summary>
    /// 客户端系统信息收集服务实现
    /// 提供完整的客户端系统、硬件、环境信息收集功能
    /// </summary>
    public class ClientSystemInfoService : IClientSystemInfoService
    {
        private readonly ILogger<ClientSystemInfoService> _logger;
        private readonly HLH.Lib.Security.HardwareInfoService _hardwareInfoService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ClientSystemInfoService(ILogger<ClientSystemInfoService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hardwareInfoService = new HLH.Lib.Security.HardwareInfoService();
        }

        /// <summary>
        /// 获取完整的客户端系统信息（同步版本）
        /// </summary>
        /// <returns>客户端系统信息对象</returns>
        public ClientSystemInfo GetClientSystemInfo()
        {
            try
            {
                _logger.LogDebug("开始收集客户端系统信息（同步）");

                var systemInfo = new ClientSystemInfo
                {
                    OperatingSystem = GetOperatingSystemInfo(),
                    Hardware = GetHardwareInfo(),
                    Environment = GetEnvironmentInfo(),
                    Network = GetNetworkInfo(),
                    ResourceUsage = GetResourceUsage(),
                    CollectTime = DateTime.UtcNow
                };

                _logger.LogDebug("客户端系统信息收集完成（同步）");
                return systemInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "收集客户端系统信息失败（同步）");
                throw;
            }
        }

        /// <summary>
        /// 获取完整的客户端系统信息（异步版本）
        /// </summary>
        /// <returns>客户端系统信息对象</returns>
        public async Task<ClientSystemInfo> GetClientSystemInfoAsync()
        {
            try
            {
                _logger.LogDebug("开始收集客户端系统信息");

                var systemInfo = new ClientSystemInfo
                {
                    OperatingSystem = GetOperatingSystemInfo(),
                    Hardware = GetHardwareInfo(),
                    Environment = GetEnvironmentInfo(),
                    Network = GetNetworkInfo(),
                    ResourceUsage = GetResourceUsage(),
                    CollectTime = DateTime.UtcNow
                };

                _logger.LogDebug("客户端系统信息收集完成");
                return systemInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "收集客户端系统信息失败");
                throw;
            }
        }

        /// <summary>
        /// 获取操作系统信息
        /// </summary>
        /// <returns>操作系统信息对象</returns>
        public OperatingSystemInfo GetOperatingSystemInfo()
        {
            try
            {
                var os = Environment.OSVersion;
                var versionInfo = GetWindowsVersionInfo();

                return new OperatingSystemInfo
                {
                    Platform = os.Platform.ToString(),
                    Version = os.Version.ToString(),
                    ServicePack = GetServicePackInfo(),
                    VersionString = versionInfo.VersionString,
                    Edition = versionInfo.Edition,
                    Architecture = GetSystemArchitecture(),
                    Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                    MachineName = Environment.MachineName,
                    UserName = Environment.UserName,
                    UserDomainName = Environment.UserDomainName,
                    SystemDirectory = Environment.SystemDirectory,
                    BootMode = GetBootMode(),
                    InstallDate = GetInstallDate()
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取操作系统信息失败");
                return new OperatingSystemInfo();
            }
        }

        /// <summary>
        /// 获取硬件信息
        /// </summary>
        /// <returns>硬件信息对象</returns>
        public HardwareInfo GetHardwareInfo()
        {
            try
            {
                return new HardwareInfo
                {
                    CpuId = _hardwareInfoService.GetCpuId(),
                    HardDiskId = _hardwareInfoService.GetHardDiskId(),
                    MacAddress = _hardwareInfoService.GetMacAddress(),
                    ProcessorInfo = GetProcessorInfo(),
                    MemoryInfo = GetMemoryInfo(),
                    MotherboardInfo = GetMotherboardInfo(),
                    BiosInfo = GetBiosInfo(),
                    VideoControllerInfo = GetVideoControllerInfo()
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取硬件信息失败");
                return new HardwareInfo();
            }
        }

        /// <summary>
        /// 获取环境信息
        /// </summary>
        /// <returns>环境信息对象</returns>
        public EnvironmentInfo GetEnvironmentInfo()
        {
            try
            {
                var process = Process.GetCurrentProcess();
                var assembly = Assembly.GetEntryAssembly();

                return new EnvironmentInfo
                {
                    CurrentDirectory = Environment.CurrentDirectory,
                    CommandLine = Environment.CommandLine,
                    ProcessorCount = Environment.ProcessorCount,
                    SystemPageSize = Environment.SystemPageSize,
                    TickCount = Environment.TickCount,
                    WorkingSet = Environment.WorkingSet,
                    ProcessId = process.Id,
                    ProcessName = process.ProcessName,
                    ProcessStartTime = process.StartTime,
                    ProcessWorkingSet64 = process.WorkingSet64,
                    ProcessVirtualMemorySize64 = process.VirtualMemorySize64,
                    ApplicationVersion = assembly?.GetName().Version?.ToString() ?? "Unknown",
                    ApplicationName = assembly?.GetName().Name ?? "Unknown",
                    RuntimeVersion = RuntimeInformation.FrameworkDescription,
                    Is64BitProcess = Environment.Is64BitProcess,
                    OSVersion = Environment.OSVersion.ToString(),
                    EnvironmentVariables = GetEnvironmentVariables()
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取环境信息失败");
                return new EnvironmentInfo();
            }
        }

        /// <summary>
        /// 获取网络信息
        /// </summary>
        /// <returns>网络信息对象</returns>
        public NetworkInfo GetNetworkInfo()
        {
            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.OperationalStatus == OperationalStatus.Up)
                    .Select(ni => new NetworkInterfaceInfo
                    {
                        Name = ni.Name,
                        Description = ni.Description,
                        NetworkInterfaceType = ni.NetworkInterfaceType.ToString(),
                        Speed = ni.Speed,
                        MacAddress = ni.GetPhysicalAddress().ToString(),
                        IpAddresses = ni.GetIPProperties().UnicastAddresses
                            .Select(ip => ip.Address.ToString())
                            .ToList(),
                        GatewayAddresses = ni.GetIPProperties().GatewayAddresses
                            .Select(gw => gw.Address.ToString())
                            .ToList(),
                        DnsAddresses = ni.GetIPProperties().DnsAddresses
                            .Select(dns => dns.ToString())
                            .ToList()
                    })
                    .ToList();

                return new NetworkInfo
                {
                    NetworkInterfaces = networkInterfaces,
                    HostName = System.Net.Dns.GetHostName(),
                    DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取网络信息失败");
                return new NetworkInfo();
            }
        }

        /// <summary>
        /// 获取系统资源使用情况
        /// </summary>
        /// <returns>资源使用信息对象</returns>
        public ClientResourceUsage GetResourceUsage()
        {
            try
            {
                var process = Process.GetCurrentProcess();
                var now = DateTime.UtcNow;

                // CPU使用率
                float cpuUsage = 0;
                try
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT LoadPercentage FROM Win32_Processor"))
                    {
                        var cpuObjects = searcher.Get();
                        if (cpuObjects.Count > 0)
                        {
                            foreach (var obj in cpuObjects)
                            {
                                cpuUsage += Convert.ToSingle(obj["LoadPercentage"]);
                            }
                            cpuUsage /= cpuObjects.Count;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "获取CPU使用率失败");
                    cpuUsage = 0;
                }

                // 内存使用率
                long totalMemory = 0;
                long availableMemory = 0;
                try
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
                    {
                        foreach (var obj in searcher.Get())
                        {
                            totalMemory = Convert.ToInt64(obj["TotalPhysicalMemory"]);
                            break;
                        }
                    }

                    using (var searcher = new ManagementObjectSearcher("SELECT AvailableBytes FROM Win32_PerfRawData_PerfOS_Memory"))
                    {
                        foreach (var obj in searcher.Get())
                        {
                            availableMemory = Convert.ToInt64(obj["AvailableBytes"]);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "获取内存信息失败");
                }

                // 磁盘可用空间
                float diskFreeSpace = 0;
                try
                {
                    var currentDrive = Path.GetPathRoot(Environment.CurrentDirectory);
                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        if (drive.IsReady && drive.RootDirectory.FullName == currentDrive)
                        {
                            diskFreeSpace = drive.AvailableFreeSpace / (1024f * 1024f * 1024f);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "获取磁盘空间失败");
                }

                return ClientResourceUsage.Create(
                    cpuUsage: cpuUsage,
                    memoryUsage: process.WorkingSet64 / (1024 * 1024), // MB
                    networkUsage: 0, // 暂时无法准确获取
                    diskFreeSpace: diskFreeSpace, // GB
                    processUptime: (long)(now - process.StartTime.ToUniversalTime()).TotalSeconds
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取系统资源使用情况失败");
                return ClientResourceUsage.Create();
            }
        }

        #region 私有辅助方法

        /// <summary>
        /// 获取Windows版本信息
        /// </summary>
        /// <returns>版本信息</returns>
        private (string VersionString, string Edition) GetWindowsVersionInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Caption, OSArchitecture FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return (
                            VersionString: obj["Caption"]?.ToString() ?? "Unknown",
                            Edition: obj["OSArchitecture"]?.ToString() ?? "Unknown"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取Windows版本信息失败");
            }

            return ("Unknown", "Unknown");
        }

        /// <summary>
        /// 获取服务包信息
        /// </summary>
        /// <returns>服务包信息</returns>
        private string GetServicePackInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT CSDVersion FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return obj["CSDVersion"]?.ToString() ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取服务包信息失败");
            }

            return "";
        }

        /// <summary>
        /// 获取系统架构
        /// </summary>
        /// <returns>系统架构</returns>
        private string GetSystemArchitecture()
        {
            return RuntimeInformation.ProcessArchitecture.ToString();
        }

        /// <summary>
        /// 获取启动模式
        /// </summary>
        /// <returns>启动模式</returns>
        private string GetBootMode()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT BootupState FROM Win32_ComputerSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return obj["BootupState"]?.ToString() ?? "Unknown";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取启动模式失败");
            }

            return "Unknown";
        }

        /// <summary>
        /// 获取安装日期
        /// </summary>
        /// <returns>安装日期</returns>
        private DateTime? GetInstallDate()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT InstallDate FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        if (obj["InstallDate"] != null)
                        {
                            var installDateStr = obj["InstallDate"].ToString();
                            if (DateTime.TryParse(installDateStr, out var installDate))
                            {
                                return installDate;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取安装日期失败");
            }

            return null;
        }

        /// <summary>
        /// 获取处理器信息
        /// </summary>
        /// <returns>处理器信息</returns>
        private ProcessorInfo GetProcessorInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Name, Manufacturer, MaxClockSpeed, NumberOfCores, NumberOfLogicalProcessors FROM Win32_Processor"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return new ProcessorInfo
                        {
                            Name = obj["Name"]?.ToString() ?? "Unknown",
                            Manufacturer = obj["Manufacturer"]?.ToString() ?? "Unknown",
                            MaxClockSpeed = Convert.ToInt32(obj["MaxClockSpeed"] ?? 0),
                            NumberOfCores = Convert.ToInt32(obj["NumberOfCores"] ?? 1),
                            NumberOfLogicalProcessors = Convert.ToInt32(obj["NumberOfLogicalProcessors"] ?? 1)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取处理器信息失败");
            }

            return new ProcessorInfo();
        }

        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <returns>内存信息</returns>
        private MemoryInfo GetMemoryInfo()
        {
            try
            {
                long totalPhysicalMemory = 0;
                long totalVirtualMemory = 0;

                using (var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        totalPhysicalMemory = Convert.ToInt64(obj["TotalPhysicalMemory"] ?? 0);
                        break;
                    }
                }

                using (var searcher = new ManagementObjectSearcher("SELECT TotalVirtualMemorySize FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        totalVirtualMemory = Convert.ToInt64(obj["TotalVirtualMemorySize"] ?? 0);
                        break;
                    }
                }

                return new MemoryInfo
                {
                    TotalPhysicalMemory = totalPhysicalMemory,
                    TotalVirtualMemory = totalVirtualMemory * 1024 // Convert KB to bytes
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取内存信息失败");
            }

            return new MemoryInfo();
        }

        /// <summary>
        /// 获取主板信息
        /// </summary>
        /// <returns>主板信息</returns>
        private MotherboardInfo GetMotherboardInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Manufacturer, Product, SerialNumber FROM Win32_BaseBoard"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return new MotherboardInfo
                        {
                            Manufacturer = obj["Manufacturer"]?.ToString() ?? "Unknown",
                            Product = obj["Product"]?.ToString() ?? "Unknown",
                            SerialNumber = obj["SerialNumber"]?.ToString() ?? "Unknown"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取主板信息失败");
            }

            return new MotherboardInfo();
        }

        /// <summary>
        /// 获取BIOS信息
        /// </summary>
        /// <returns>BIOS信息</returns>
        private BiosInfo GetBiosInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Manufacturer, Name, Version, SerialNumber, ReleaseDate FROM Win32_BIOS"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return new BiosInfo
                        {
                            Manufacturer = obj["Manufacturer"]?.ToString() ?? "Unknown",
                            Name = obj["Name"]?.ToString() ?? "Unknown",
                            Version = obj["Version"]?.ToString() ?? "Unknown",
                            SerialNumber = obj["SerialNumber"]?.ToString() ?? "Unknown",
                            ReleaseDate = obj["ReleaseDate"]?.ToString() ?? "Unknown"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取BIOS信息失败");
            }

            return new BiosInfo();
        }

        /// <summary>
        /// 获取显卡信息
        /// </summary>
        /// <returns>显卡信息</returns>
        private VideoControllerInfo GetVideoControllerInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Name, AdapterRAM, VideoProcessor FROM Win32_VideoController"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return new VideoControllerInfo
                        {
                            Name = obj["Name"]?.ToString() ?? "Unknown",
                            AdapterRAM = Convert.ToInt64(obj["AdapterRAM"] ?? 0),
                            VideoProcessor = obj["VideoProcessor"]?.ToString() ?? "Unknown"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取显卡信息失败");
            }

            return new VideoControllerInfo();
        }

        /// <summary>
        /// 获取环境变量
        /// </summary>
        /// <returns>环境变量字典</returns>
        private Dictionary<string, string> GetEnvironmentVariables()
        {
            try
            {
                var variables = new Dictionary<string, string>();
                var envVariables = Environment.GetEnvironmentVariables();

                foreach (var key in envVariables.Keys)
                {
                    if (key != null)
                    {
                        variables[key.ToString()] = envVariables[key]?.ToString() ?? "";
                    }
                }

                return variables;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取环境变量失败");
                return new Dictionary<string, string>();
            }
        }

        #endregion
    }
}