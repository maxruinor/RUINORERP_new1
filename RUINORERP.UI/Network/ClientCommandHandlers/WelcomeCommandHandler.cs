using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.Devices;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 欢迎命令处理器
    /// 处理服务器发送的欢迎消息，并发送欢迎确认回复
    /// </summary>
    [ClientCommandHandler("WelcomeCommandHandler", 70)]
    public class WelcomeCommandHandler : BaseClientCommandHandler
    {
        private readonly ILogger<WelcomeCommandHandler> _logger;
        private readonly IClientCommunicationService _communicationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="communicationService">通信服务</param>
        public WelcomeCommandHandler(
            ILogger<WelcomeCommandHandler> logger,
            IClientCommunicationService communicationService)
            : base(logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));

            // 注册支持的命令
            SetSupportedCommands(SystemCommands.Welcome);
        }

        /// <summary>
        /// 初始化处理器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public override async Task<bool> InitializeAsync()
        {
            bool initialized = await base.InitializeAsync();
            if (initialized)
            {
                _logger.LogDebug("欢迎命令处理器初始化成功");
            }
            return initialized;
        }

        /// <summary>
        /// 处理欢迎命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        public override async Task HandleAsync(PacketModel packet)
        {
            if (packet == null || packet.CommandId == null)
            {
                _logger.LogError("收到无效的数据包");
                return;
            }

            try
            {
                if (packet.Request is WelcomeRequest welcomeRequest)
                {
                    // 收集客户端系统信息
                    var systemInfo = CollectClientSystemInfo();

                    _logger.LogInformation($"[欢迎响应] 版本={systemInfo.ClientVersion}, OS={systemInfo.ClientOS}");

                    // 创建欢迎响应（使用请求的RequestId以便服务器匹配）
                    var welcomeResponse = WelcomeResponse.Create(
                        systemInfo.ClientVersion,
                        systemInfo.ClientOS,
                        systemInfo.ClientMachineName,
                        systemInfo.ClientCPU,
                        systemInfo.ClientMemoryMB
                    );

                    // 设置响应的RequestId与请求一致，以便服务器匹配响应
                    welcomeResponse.RequestId = welcomeRequest.RequestId;

                    // 使用SendResponseAsync发送响应包，回复服务器的ServerRequest
                    bool rs = await _communicationService.SendResponseAsync<WelcomeResponse>(
                        SystemCommands.WelcomeAck,
                        welcomeResponse,
                        welcomeRequest.RequestId
                    );
                }
                else
                {
                    _logger.LogWarning("服务器的欢迎请求数据格式不正确");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理欢迎命令时发生异常");
            }
        }

        /// <summary>
        /// 收集客户端系统信息
        /// </summary>
        /// <returns>客户端系统信息</returns>
        private ClientSystemInfo CollectClientSystemInfo()
        {
            var systemInfo = new ClientSystemInfo();

            try
            {
                // 获取客户端版本信息
                systemInfo.ClientVersion = GetClientVersion();

                // 获取操作系统信息
                systemInfo.ClientOS = Environment.OSVersion.ToString();

                // 获取机器名
                systemInfo.ClientMachineName = Environment.MachineName;

                // 获取CPU信息
                systemInfo.ClientCPU = GetCPUInfo();

                // 获取内存信息
                systemInfo.ClientMemoryMB = GetTotalMemoryMB();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "收集客户端系统信息时发生部分错误");
            }

            return systemInfo;
        }

        /// <summary>
        /// 获取客户端版本
        /// </summary>
        /// <returns>客户端版本号</returns>
        private string GetClientVersion()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                return versionInfo.FileVersion ?? "1.0.0.0";
            }
            catch
            {
                return "1.0.0.0";
            }
        }

        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns>CPU名称</returns>
        private string GetCPUInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["Name"]?.ToString() ?? "Unknown CPU";
                    }
                }
            }
            catch
            {
                return "Unknown CPU";
            }

            return "Unknown CPU";
        }

        /// <summary>
        /// 获取总内存大小（MB）
        /// </summary>
        /// <returns>内存大小（MB）</returns>
        private long GetTotalMemoryMB()
        {
            try
            {
                // 使用 Microsoft.VisualBasic.Devices.ComputerInfo 获取物理内存
                // 此方法兼容 .NET Framework 和大多数 .NET 版本
                var computerInfo = new ComputerInfo();
                ulong totalMemoryBytes = computerInfo.TotalPhysicalMemory;
                return (long)(totalMemoryBytes / (1024 * 1024));
            }
            catch
            {
                // 备用方法：使用PerformanceCounter
                try
                {
                    using (var ramCounter = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes"))
                    {
                        // 获取可用内存，估算总内存（不太准确，但可作为备用方案）
                        return (long)(ramCounter.NextValue() * 2); // 简单估算，实际应该获取总内存
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }
    }

    /// <summary>
    /// 客户端系统信息
    /// </summary>
    internal class ClientSystemInfo
    {
        /// <summary>
        /// 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 客户端操作系统信息
        /// </summary>
        public string ClientOS { get; set; }

        /// <summary>
        /// 客户端机器名
        /// </summary>
        public string ClientMachineName { get; set; }

        /// <summary>
        /// 客户端CPU信息
        /// </summary>
        public string ClientCPU { get; set; }

        /// <summary>
        /// 客户端内存大小（MB）
        /// </summary>
        public long ClientMemoryMB { get; set; }
    }
}
