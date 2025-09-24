using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 心跳管理器
    /// 负责定期发送心跳命令以保持连接活跃
    /// </summary>
    public class HeartbeatManager : IDisposable
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly int _heartbeatIntervalMs;
        private readonly int _reconnectAttempts;
        private readonly int _reconnectIntervalMs;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _heartbeatTask;
        private int _failedAttempts;
        private bool _isDisposed;

        /// <summary>
        /// 心跳管理器构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="heartbeatIntervalMs">心跳间隔（毫秒），默认30秒</param>
        /// <param name="reconnectAttempts">重连尝试次数，默认3次</param>
        /// <param name="reconnectIntervalMs">重连间隔（毫秒），默认5秒</param>
        public HeartbeatManager(
            IClientCommunicationService communicationService,
            int heartbeatIntervalMs = 30000,
            int reconnectAttempts = 3,
            int reconnectIntervalMs = 5000)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _heartbeatIntervalMs = heartbeatIntervalMs;
            _reconnectAttempts = reconnectAttempts;
            _reconnectIntervalMs = reconnectIntervalMs;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 开始发送心跳
        /// </summary>
        public void Start()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(HeartbeatManager));
            }

            if (_heartbeatTask != null && !_heartbeatTask.IsCompleted)
            {
                return; // 已经在运行中
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _heartbeatTask = Task.Run(SendHeartbeatsAsync, _cancellationTokenSource.Token);
        }

        /// <summary>
        /// 停止发送心跳
        /// </summary>
        public void Stop()
        {
            if (!_isDisposed)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _heartbeatTask = null;
            }
        }

        /// <summary>
        /// 定期发送心跳
        /// </summary>
        private async Task SendHeartbeatsAsync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    if (_communicationService.IsConnected)
                    {
                        // 创建并发送心跳命令
                        var heartbeatCommand = CreateHeartbeatCommand();
                        var response = await _communicationService.SendCommandAsync<object>(heartbeatCommand, _cancellationTokenSource.Token);

                        if (response.Success)
                        {
                            // 心跳发送成功，重置失败计数器
                            _failedAttempts = 0;
                            OnHeartbeatSuccess();
                        }
                        else
                        {
                            // 心跳发送失败，增加失败计数器
                            _failedAttempts++;
                            OnHeartbeatFailed(response.Message);
                        }
                    }
                    else if (_failedAttempts < _reconnectAttempts)
                    {
                        // 如果连接断开，尝试重连
                        _failedAttempts++;
                        OnReconnectionAttempt(_failedAttempts);
                    }
                    else
                    {
                        // 超过重连次数，触发重连失败事件
                        OnReconnectionFailed();
                        _failedAttempts = 0;
                    }
                }
                catch (Exception ex)
                {
                    _failedAttempts++;
                    OnHeartbeatException(ex);
                }

                // 等待下一次心跳间隔
                try
                {
                    await Task.Delay(_heartbeatIntervalMs, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    // 正常取消，不需要处理
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Stop();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        #region 心跳命令构建方法

        /// <summary>
        /// 创建心跳命令
        /// 包含客户端资源使用情况等高级属性
        /// </summary>
        /// <returns>配置好的心跳命令</returns>
        private RUINORERP.PacketSpec.Commands.System.HeartbeatCommand CreateHeartbeatCommand()
        {
            try
            {
                // 生成客户端ID
                string clientId = GenerateClientId();
                
                // 获取会话信息 (实际项目中应从登录状态获取)
                string sessionToken = GetSessionToken();
                long userId = GetCurrentUserId();
                
                // 创建心跳命令
                var command = new RUINORERP.PacketSpec.Commands.System.HeartbeatCommand(clientId, sessionToken, userId);
                
                // 设置客户端信息
                command.ClientVersion = GetClientVersion();
                command.ClientIp = GetClientIp();
                command.ClientStatus = "Normal";
                command.ProcessUptime = (int)Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds;
                
                // 设置网络和资源使用信息
                command.NetworkLatency = EstimateNetworkLatency();
                command.ResourceUsage = GetResourceUsage();
                
                return command;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建心跳命令失败: {ex.Message}");
                // 出错时返回基础的心跳命令
                return new RUINORERP.PacketSpec.Commands.System.HeartbeatCommand();
            }
        }

        /// <summary>
        /// 生成客户端ID
        /// </summary>
        /// <returns>客户端ID</returns>
        private string GenerateClientId()
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
            catch
            {
                // 出现异常时返回基于时间戳的备用ID
                return $"fallback_{DateTime.UtcNow.Ticks}";
            }
        }

        /// <summary>
        /// 获取机器唯一标识
        /// </summary>
        /// <returns>机器唯一标识</returns>
        private string GetMachineUniqueId()
        {
            try
            {
                // 在Windows环境下使用主板序列号或MAC地址
                return GetWindowsMachineId();
            }
            catch
            {
                // 获取失败时返回基于系统信息的哈希值
                string systemInfo = $"{Environment.OSVersion}_{Environment.ProcessorCount}_{Environment.MachineName}";
                return systemInfo.GetHashCode().ToString("X");
            }
        }

        /// <summary>
        /// 获取Windows机器唯一标识
        /// </summary>
        /// <returns>Windows机器唯一标识</returns>
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
            catch
            {
                return "unknown";
            }
        }

        /// <summary>
        /// 获取应用实例ID
        /// </summary>
        /// <returns>应用实例ID</returns>
        private string GetApplicationInstanceId()
        {
            try
            {
                // 使用AppDomain的ID作为实例标识
                return AppDomain.CurrentDomain.Id.ToString();
            }
            catch
            {
                return "1";
            }
        }

        /// <summary>
        /// 获取客户端资源使用情况
        /// </summary>
        /// <returns>资源使用信息</returns>
        private ClientResourceUsage GetResourceUsage()
        {
            try
            {
                var process = Process.GetCurrentProcess();
                
                // 获取内存使用量（MB）
                long memoryUsage = process.WorkingSet64 / (1024 * 1024);
                
                // 获取进程运行时间（秒）
                long processUptime = (long)(DateTime.UtcNow - process.StartTime.ToUniversalTime()).TotalSeconds;
                
                // 估算CPU使用率
                float cpuUsage = 0;
                try
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT LoadPercentage FROM Win32_Processor"))
                    {
                        foreach (var obj in searcher.Get())
                        {
                            cpuUsage += Convert.ToSingle(obj["LoadPercentage"]);
                        }
                        cpuUsage /= Environment.ProcessorCount;
                    }
                }
                catch
                {
                    // 如果无法获取CPU使用率，使用处理器数量作为默认值
                    cpuUsage = Environment.ProcessorCount;
                }
                
                // 估算磁盘可用空间（GB）
                float diskFreeSpace = 0;
                try
                {
                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        if (drive.IsReady && drive.RootDirectory.FullName == Path.GetPathRoot(Environment.CurrentDirectory))
                        {
                            diskFreeSpace = drive.AvailableFreeSpace / (1024f * 1024f * 1024f);
                            break;
                        }
                    }
                }
                catch
                {
                    diskFreeSpace = 100; // 默认值
                }
                
                // 网络带宽使用暂时设为0
                float networkUsage = 0;
                
                return ClientResourceUsage.Create(cpuUsage, memoryUsage, networkUsage, diskFreeSpace, processUptime);
            }
            catch
            {
                return ClientResourceUsage.Create(); // 返回默认值
            }
        }

        /// <summary>
        /// 估算网络延迟
        /// </summary>
        /// <returns>网络延迟（毫秒）</returns>
        private int EstimateNetworkLatency()
        {
            try
            {
                // 这是一个简化的实现，实际项目中应使用多次ping或其他方式测量
                return 0; // 临时返回0，实际应用中应实现真实的延迟测量
            }
            catch
            {
                return -1; // 表示无法测量
            }
        }

        /// <summary>
        /// 获取客户端版本号
        /// </summary>
        /// <returns>客户端版本号</returns>
        private string GetClientVersion()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetEntryAssembly();
                if (assembly != null)
                {
                    var version = assembly.GetName().Version;
                    return version?.ToString() ?? "1.0.0";
                }
                return "1.0.0";
            }
            catch
            {
                return "1.0.0";
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>客户端IP地址</returns>
        private string GetClientIp()
        {
            try
            {
                // 获取本机IPv4地址
                foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        /// <summary>
        /// 获取会话令牌
        /// 实际项目中应从登录状态获取
        /// </summary>
        /// <returns>会话令牌</returns>
        private string GetSessionToken()
        {
            try
            {
                // 示例实现，实际应从应用状态中获取
                // 这里应该从全局上下文或登录状态中获取真实的会话令牌
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取当前用户ID
        /// 实际项目中应从登录状态获取
        /// </summary>
        /// <returns>用户ID</returns>
        private long GetCurrentUserId()
        {
            try
            {
                // 示例实现，实际应从应用状态中获取
                // 这里应该从全局上下文或登录状态中获取真实的用户ID
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 心跳发送成功事件
        /// </summary>
        public event Action OnHeartbeatSuccess = delegate { };

        /// <summary>
        /// 心跳发送失败事件
        /// </summary>
        public event Action<string> OnHeartbeatFailed = delegate { };

        /// <summary>
        /// 心跳异常事件
        /// </summary>
        public event Action<Exception> OnHeartbeatException = delegate { };

        /// <summary>
        /// 重连尝试事件
        /// </summary>
        public event Action<int> OnReconnectionAttempt = delegate { };

        /// <summary>
        /// 重连失败事件
        /// </summary>
        public event Action OnReconnectionFailed = delegate { };

        /// <summary>
        /// 连接丢失事件
        /// </summary>
        public event Action ConnectionLost = delegate { };

        /// <summary>
        /// 心跳失败事件
        /// </summary>
        public event Action<Exception> HeartbeatFailed = delegate { };

        #endregion
    }
}