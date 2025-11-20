/*****************************************************************************************
 * 文件名称：ClientSystemInfoModels.cs
 * 创建人员：RUINOR ERP系统
 * 创建时间：2024年
 * 文件描述：客户端系统信息数据模型，定义客户端系统信息相关的数据结构和模型
 * 版权所有：RUINOR ERP系统
 *****************************************************************************************/

using RUINORERP.PacketSpec.Models.Requests;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 客户端系统信息
    /// 包含客户端完整的系统、硬件、环境信息
    /// </summary>
    public class ClientSystemInfo
    {
        /// <summary>
        /// 操作系统信息
        /// </summary>
        public OperatingSystemInfo OperatingSystem { get; set; } = new OperatingSystemInfo();

        /// <summary>
        /// 硬件信息
        /// </summary>
        public HardwareInfo Hardware { get; set; } = new HardwareInfo();

        /// <summary>
        /// 环境信息
        /// </summary>
        public EnvironmentInfo Environment { get; set; } = new EnvironmentInfo();

        /// <summary>
        /// 网络信息
        /// </summary>
        public NetworkInfo Network { get; set; } = new NetworkInfo();

        /// <summary>
        /// 资源使用情况
        /// </summary>
        public ClientResourceUsage ResourceUsage { get; set; } = ClientResourceUsage.Create();

        /// <summary>
        /// 信息收集时间（UTC）
        /// </summary>
        public DateTime CollectTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 操作系统名称（快捷属性）
        /// </summary>
        public string OSName => OperatingSystem?.Platform ?? "未知";

        /// <summary>
        /// 操作系统版本（快捷属性）
        /// </summary>
        public string OSVersion => OperatingSystem?.VersionString ?? "未知";

        /// <summary>
        /// 计算机名称（快捷属性）
        /// </summary>
        public string MachineName => OperatingSystem?.MachineName ?? "未知";

        /// <summary>
        /// CPU信息（快捷属性）
        /// </summary>
        public string CPUInfo => Hardware?.ProcessorInfo?.Name ?? "未知";

        /// <summary>
        /// 总内存（快捷属性）
        /// </summary>
        public long TotalMemory => Hardware?.MemoryInfo?.TotalPhysicalMemory ?? 0;
    }

    /// <summary>
    /// 操作系统信息
    /// </summary>
    public class OperatingSystemInfo
    {
        /// <summary>
        /// 平台类型
        /// </summary>
        public string Platform { get; set; } = string.Empty;

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 服务包
        /// </summary>
        public string ServicePack { get; set; } = string.Empty;

        /// <summary>
        /// 版本字符串（友好名称）
        /// </summary>
        public string VersionString { get; set; } = string.Empty;

        /// <summary>
        /// 版本类型
        /// </summary>
        public string Edition { get; set; } = string.Empty;

        /// <summary>
        /// 系统架构
        /// </summary>
        public string Architecture { get; set; } = string.Empty;

        /// <summary>
        /// 是否为64位操作系统
        /// </summary>
        public bool Is64BitOperatingSystem { get; set; }

        /// <summary>
        /// 计算机名称
        /// </summary>
        public string MachineName { get; set; } = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 用户域名
        /// </summary>
        public string UserDomainName { get; set; } = string.Empty;

        /// <summary>
        /// 系统目录
        /// </summary>
        public string SystemDirectory { get; set; } = string.Empty;

        /// <summary>
        /// 启动模式
        /// </summary>
        public string BootMode { get; set; } = string.Empty;

        /// <summary>
        /// 安装日期
        /// </summary>
        public DateTime? InstallDate { get; set; }
    }

    /// <summary>
    /// 硬件信息
    /// </summary>
    public class HardwareInfo
    {
        /// <summary>
        /// CPU ID
        /// </summary>
        public string CpuId { get; set; } = string.Empty;

        /// <summary>
        /// 硬盘ID
        /// </summary>
        public string HardDiskId { get; set; } = string.Empty;

        /// <summary>
        /// MAC地址
        /// </summary>
        public string MacAddress { get; set; } = string.Empty;

        /// <summary>
        /// 处理器信息
        /// </summary>
        public ProcessorInfo ProcessorInfo { get; set; } = new ProcessorInfo();

        /// <summary>
        /// 内存信息
        /// </summary>
        public MemoryInfo MemoryInfo { get; set; } = new MemoryInfo();

        /// <summary>
        /// 主板信息
        /// </summary>
        public MotherboardInfo MotherboardInfo { get; set; } = new MotherboardInfo();

        /// <summary>
        /// BIOS信息
        /// </summary>
        public BiosInfo BiosInfo { get; set; } = new BiosInfo();

        /// <summary>
        /// 显卡信息
        /// </summary>
        public VideoControllerInfo VideoControllerInfo { get; set; } = new VideoControllerInfo();
    }

    /// <summary>
    /// 处理器信息
    /// </summary>
    public class ProcessorInfo
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; } = string.Empty;

        /// <summary>
        /// 最大时钟频率（MHz）
        /// </summary>
        public int MaxClockSpeed { get; set; }

        /// <summary>
        /// 核心数
        /// </summary>
        public int NumberOfCores { get; set; }

        /// <summary>
        /// 逻辑处理器数
        /// </summary>
        public int NumberOfLogicalProcessors { get; set; }
    }

    /// <summary>
    /// 内存信息
    /// </summary>
    public class MemoryInfo
    {
        /// <summary>
        /// 总物理内存（字节）
        /// </summary>
        public long TotalPhysicalMemory { get; set; }

        /// <summary>
        /// 可用物理内存（字节）
        /// </summary>
        public long AvailablePhysicalMemory { get; set; }

        /// <summary>
        /// 总虚拟内存（字节）
        /// </summary>
        public long TotalVirtualMemory { get; set; }

        /// <summary>
        /// 可用虚拟内存（字节）
        /// </summary>
        public long AvailableVirtualMemory { get; set; }
    }

    /// <summary>
    /// 主板信息
    /// </summary>
    public class MotherboardInfo
    {
        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; } = string.Empty;

        /// <summary>
        /// 产品型号
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// 序列号
        /// </summary>
        public string SerialNumber { get; set; } = string.Empty;
    }

    /// <summary>
    /// BIOS信息
    /// </summary>
    public class BiosInfo
    {
        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 序列号
        /// </summary>
        public string SerialNumber { get; set; } = string.Empty;

        /// <summary>
        /// 发布日期
        /// </summary>
        public string ReleaseDate { get; set; } = string.Empty;
    }

    /// <summary>
    /// 显卡信息
    /// </summary>
    public class VideoControllerInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 显存大小（字节）
        /// </summary>
        public long AdapterRAM { get; set; }

        /// <summary>
        /// 视频处理器
        /// </summary>
        public string VideoProcessor { get; set; } = string.Empty;
    }

    /// <summary>
    /// 环境信息
    /// </summary>
    public class EnvironmentInfo
    {
        /// <summary>
        /// 当前目录
        /// </summary>
        public string CurrentDirectory { get; set; } = string.Empty;

        /// <summary>
        /// 命令行参数
        /// </summary>
        public string CommandLine { get; set; } = string.Empty;

        /// <summary>
        /// 处理器数量
        /// </summary>
        public int ProcessorCount { get; set; }

        /// <summary>
        /// 系统页面大小
        /// </summary>
        public int SystemPageSize { get; set; }

        /// <summary>
        /// 系统运行时间（毫秒）
        /// </summary>
        public int TickCount { get; set; }

        /// <summary>
        /// 工作集大小
        /// </summary>
        public long WorkingSet { get; set; }

        /// <summary>
        /// 进程ID
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName { get; set; } = string.Empty;

        /// <summary>
        /// 进程启动时间
        /// </summary>
        public DateTime ProcessStartTime { get; set; }

        /// <summary>
        /// 进程工作集大小（64位）
        /// </summary>
        public long ProcessWorkingSet64 { get; set; }

        /// <summary>
        /// 进程虚拟内存大小（64位）
        /// </summary>
        public long ProcessVirtualMemorySize64 { get; set; }

        /// <summary>
        /// 应用程序版本
        /// </summary>
        public string ApplicationVersion { get; set; } = string.Empty;

        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string ApplicationName { get; set; } = string.Empty;

        /// <summary>
        /// 运行时版本
        /// </summary>
        public string RuntimeVersion { get; set; } = string.Empty;

        /// <summary>
        /// 是否为64位进程
        /// </summary>
        public bool Is64BitProcess { get; set; }

        /// <summary>
        /// 操作系统版本
        /// </summary>
        public string OSVersion { get; set; } = string.Empty;

        /// <summary>
        /// 环境变量
        /// </summary>
        public Dictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// 网络质量信息
    /// </summary>
    public class NetworkQualityInfo
    {
        /// <summary>
        /// 延迟（毫秒）
        /// </summary>
        public double Latency { get; set; }

        /// <summary>
        /// 丢包率（0-1之间的小数）
        /// </summary>
        public double PacketLossRate { get; set; }

        /// <summary>
        /// 可用带宽（字节/秒）
        /// </summary>
        public long AvailableBandwidth { get; set; }

        /// <summary>
        /// 抖动（毫秒）
        /// </summary>
        public double Jitter { get; set; }
    }

    /// <summary>
    /// 网络信息
    /// </summary>
    public class NetworkInfo
    {
        /// <summary>
        /// 网络接口列表
        /// </summary>
        public List<NetworkInterfaceInfo> NetworkInterfaces { get; set; } = new List<NetworkInterfaceInfo>();

        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { get; set; } = string.Empty;

        /// <summary>
        /// 域名
        /// </summary>
        public string DomainName { get; set; } = string.Empty;

        /// <summary>
        /// 本地IP地址（当前连接）
        /// </summary>
        public string LocalIP { get; set; } = string.Empty;

        /// <summary>
        /// 本地端口（当前连接）
        /// </summary>
        public int LocalPort { get; set; }

        /// <summary>
        /// 远程IP地址（服务器）
        /// </summary>
        public string RemoteIP { get; set; } = string.Empty;

        /// <summary>
        /// 远程端口（服务器）
        /// </summary>
        public int RemotePort { get; set; }

        /// <summary>
        /// 网络质量信息
        /// </summary>
        public NetworkQualityInfo NetworkQuality { get; set; } = new NetworkQualityInfo();
    }

    /// <summary>
    /// 网络接口信息
    /// </summary>
    public class NetworkInterfaceInfo
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 接口描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 接口类型
        /// </summary>
        public string NetworkInterfaceType { get; set; } = string.Empty;

        /// <summary>
        /// 速度（bps）
        /// </summary>
        public long Speed { get; set; }

        /// <summary>
        /// MAC地址
        /// </summary>
        public string MacAddress { get; set; } = string.Empty;

        /// <summary>
        /// IP地址列表
        /// </summary>
        public List<string> IpAddresses { get; set; } = new List<string>();

        /// <summary>
        /// 网关地址列表
        /// </summary>
        public List<string> GatewayAddresses { get; set; } = new List<string>();

        /// <summary>
        /// DNS地址列表
        /// </summary>
        public List<string> DnsAddresses { get; set; } = new List<string>();
    }
}
