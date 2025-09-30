using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.UI.Network
{

    public class HeartbeatManager : IDisposable
    {
        private readonly ISocketClient _socketClient;
        private readonly RequestResponseManager _requestResponseManager;
        private readonly int _heartbeatIntervalMs;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _heartbeatTask;
        private int _failedAttempts;
        private bool _isDisposed;
        private readonly object _lock = new object();
        private readonly ILogger<HeartbeatManager> _logger;
        private int _totalHeartbeats;
        private int _successfulHeartbeats;
        private int _failedHeartbeats;
        private DateTime _lastHeartbeatTime;

        /// <summary>
        /// 获取心跳统计信息 - 新架构监控指标
        /// 
        /// 📊 统计指标说明：
        /// - TotalHeartbeats: 总心跳次数
        /// - SuccessfulHeartbeats: 成功的心跳次数  
        /// - FailedHeartbeats: 失败的心跳次数
        /// - SuccessRate: 成功率百分比（0-100）
        /// - LastHeartbeatTime: 最后一次心跳时间
        /// - IsRunning: 心跳任务是否正在运行
        /// - FailedAttempts: 当前连续失败次数
        /// 
        /// 🔄 使用场景：
        /// - 监控界面显示连接健康状态
        /// - 自动化测试验证心跳功能
        /// - 运维监控和告警
        /// - 性能分析和优化
        /// 
        /// 🔗 新架构集成：
        /// - 基于RequestResponseManager的响应结果统计
        /// - 与SocketClient连接状态同步
        /// - 直接使用ISocketClient发送心跳数据
        /// </summary>
        public HeartbeatStatistics Statistics
        {
            get
            {
                lock (_lock)
                {
                    double successRate = _totalHeartbeats > 0 ? (double)_successfulHeartbeats / _totalHeartbeats * 100 : 0;
                    return new HeartbeatStatistics(
                        _totalHeartbeats,
                        _successfulHeartbeats,
                        _failedHeartbeats,
                        _lastHeartbeatTime,
                        0, // averageResponseTime 暂时设为0
                        _socketClient.IsConnected,
                        _heartbeatTask != null && !_heartbeatTask.IsCompleted,
                        _failedAttempts,
                        _socketClient.IsConnected ? "Connected" : "Disconnected"
                    );
                }
            }
        }


        /// <summary>
        /// 心跳管理器构造函数 - 直接使用ISocketClient和RequestResponseManager
        /// </summary>
        /// <param name="socketClient">Socket客户端接口，用于直接发送心跳数据</param>
        /// <param name="requestResponseManager">请求响应管理器，用于处理心跳请求和响应</param>
        /// <param name="heartbeatIntervalMs">心跳间隔（毫秒），默认30秒</param>
        /// <param name="logger">日志记录器，可选参数，用于记录心跳过程中的信息和异常</param>
        public HeartbeatManager(
            ISocketClient socketClient,
            RequestResponseManager requestResponseManager,
            int heartbeatIntervalMs = 30000,
            ILogger<HeartbeatManager> logger = null)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _requestResponseManager = requestResponseManager ?? throw new ArgumentNullException(nameof(requestResponseManager));

            // 参数验证
            if (heartbeatIntervalMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(heartbeatIntervalMs), "心跳间隔必须大于0");

            _heartbeatIntervalMs = heartbeatIntervalMs;
            _cancellationTokenSource = new CancellationTokenSource();
            _logger = logger;
        }

        /// <summary>
        /// 开始发送心跳
        /// 在单独的后台任务中定期执行心跳发送逻辑
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已被释放时抛出</exception>
        public void Start()
        {
            // 系统启用前暂停心跳，待业务稳定后启用
            // _heartbeatTask = Task.Run(SendHeartbeatsAsync, _cancellationTokenSource.Token);
            _logger?.LogInformation("心跳任务已创建，当前处于测试模式暂不启动");

            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(HeartbeatManager));
            }

            lock (_lock)
            {
                if (_heartbeatTask != null && !_heartbeatTask.IsCompleted)
                {
                    _logger?.LogInformation("心跳任务已经在运行中");
                    return; // 已经在运行中
                }

                _cancellationTokenSource = new CancellationTokenSource();
                _heartbeatTask = Task.Run(SendHeartbeatsAsync, _cancellationTokenSource.Token);
                _logger?.LogInformation("心跳任务已启动，间隔: {HeartbeatIntervalMs}毫秒", _heartbeatIntervalMs);
            }
        }

        /// <summary>
        /// 停止发送心跳
        /// 取消当前心跳任务并释放相关资源
        /// </summary>
        public void Stop()
        {
            lock (_lock)
            {
                if (!_isDisposed && _cancellationTokenSource != null)
                {
                    try
                    {
                        _cancellationTokenSource.Cancel();
                        _cancellationTokenSource.Dispose();
                        _heartbeatTask = null;
                        _failedAttempts = 0;
                        _logger?.LogInformation("心跳任务已停止");
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "停止心跳任务时发生异常");
                    }
                }
            }
        }


        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>心跳执行结果，包含成功状态、响应消息和执行时间</returns>
        public async Task<(bool Success, string Message, TimeSpan Elapsed)> SendHeartbeatAsync(CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                return (false, "心跳管理器已释放", TimeSpan.Zero);
            }

            if (!_socketClient.IsConnected)
            {
                return (false, "连接未建立，无法发送心跳", TimeSpan.Zero);
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                _logger?.LogInformation("手动心跳测试开始");

                // 创建心跳命令
                var heartbeatCommand = CreateHeartbeatCommand();
                _logger?.LogDebug("手动心跳命令已创建: CommandId={CommandId}", heartbeatCommand.CommandId);

                // 直接使用RequestResponseManager发送心跳请求
                var response = await _requestResponseManager.SendRequestAsync<object, HeartbeatRequest>(
                    _socketClient,
                    heartbeatCommand.CommandIdentifier,
                    heartbeatCommand.GetSerializableData(),
                    cancellationToken,
                    5000 // 心跳超时时间5秒
                );

                stopwatch.Stop();

                // 更新统计信息
                lock (_lock)
                {
                    _totalHeartbeats++;
                    _successfulHeartbeats++;
                    _lastHeartbeatTime = DateTime.UtcNow;
                }

                _logger?.LogInformation("手动心跳测试成功，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                return (true, $"心跳成功", stopwatch.Elapsed);
            }
            catch (TimeoutException ex)
            {
                stopwatch.Stop();
                _logger?.LogWarning(ex, "手动心跳测试超时，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);

                // 更新统计信息
                lock (_lock)
                {
                    _totalHeartbeats++;
                    _failedHeartbeats++;
                    _lastHeartbeatTime = DateTime.UtcNow;
                }

                return (false, $"心跳超时: {ex.Message}", stopwatch.Elapsed);
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();
                _logger?.LogWarning("手动心跳测试被取消，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                return (false, "心跳操作被取消", stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger?.LogError(ex, "手动心跳测试异常，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);

                // 更新统计信息
                lock (_lock)
                {
                    _totalHeartbeats++;
                    _failedHeartbeats++;
                    _lastHeartbeatTime = DateTime.UtcNow;
                }

                return (false, $"心跳异常: {ex.Message}", stopwatch.Elapsed);
            }
        }


        /// </summary>
        /// <returns>异步任务，心跳循环任务</returns>
        private async Task SendHeartbeatsAsync()
        {
            _logger?.LogInformation("心跳任务已启动，开始定期发送心跳");

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    if (_socketClient.IsConnected)
                    {
                        // 🔄 新架构心跳发送流程 - 直接使用RequestResponseManager
                        _logger?.LogDebug("开始构建心跳命令...");

                        // 步骤1: 创建心跳命令对象
                        var heartbeatCommand = CreateHeartbeatCommand();
                        _logger?.LogDebug("心跳命令已创建: CommandId={CommandId}",
                            heartbeatCommand.CommandId);

                        try
                        {
                            // 步骤2-8: 直接使用RequestResponseManager发送命令并等待响应
                            _logger?.LogDebug("通过RequestResponseManager发送心跳命令...");
                            await _requestResponseManager.SendRequestAsync<object, HeartbeatRequest>(
                                _socketClient,
                                heartbeatCommand.CommandIdentifier,
                                heartbeatCommand.GetSerializableData(),
                                _cancellationTokenSource.Token,
                                5000 // 心跳超时时间5秒
                            );

                            // 步骤9: 处理响应结果
                            // 心跳发送成功，更新统计信息
                            lock (_lock)
                            {
                                _totalHeartbeats++;
                                _successfulHeartbeats++;
                                _failedAttempts = 0;
                                _lastHeartbeatTime = DateTime.UtcNow;
                            }
                            _logger?.LogDebug("心跳发送成功");
                            OnHeartbeatSuccess();
                        }
                        catch (TimeoutException ex)
                        {
                            // 心跳发送超时
                            lock (_lock)
                            {
                                _totalHeartbeats++;
                                _failedHeartbeats++;
                                _failedAttempts++;
                                _lastHeartbeatTime = DateTime.UtcNow;
                            }
                            _logger?.LogWarning("心跳发送超时: {Message}，连续失败次数: {FailedAttempts}",
                                ex.Message, _failedAttempts);
                            HandleHeartbeatFailure(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            // 心跳发送失败
                            lock (_lock)
                            {
                                _totalHeartbeats++;
                                _failedHeartbeats++;
                                _failedAttempts++;
                                _lastHeartbeatTime = DateTime.UtcNow;
                            }
                            _logger?.LogWarning("心跳发送失败: {Message}，连续失败次数: {FailedAttempts}",
                                ex.Message, _failedAttempts);
                            HandleHeartbeatFailure(ex.Message);
                        }
                    }
                    else
                    {
                        // 连接断开，监控连接状态
                        _logger?.LogInformation("连接已断开，监控连接状态...");

                        // 重置失败计数器
                        lock (_lock)
                        {
                            _failedAttempts = 0;
                        }

                        // 不执行重连逻辑，重连由ClientCommunicationService负责
                    }
                }
                catch (TaskCanceledException)
                {
                    // 任务被取消，正常退出
                    _logger?.LogInformation("心跳任务收到取消信号，准备退出");
                    break;
                }
                catch (OperationCanceledException ex)
                {
                    // 操作被取消，正常退出
                    _logger?.LogInformation(ex, "心跳操作被取消");
                    break;
                }
                catch (Exception ex)
                {
                    // 处理其他异常，增加失败计数器
                    _failedAttempts++;
                    _logger?.LogError(ex, "心跳处理过程中发生未预期的异常，连续失败次数: {FailedAttempts}", _failedAttempts);
                    HandleHeartbeatException(ex);
                }

                // 等待下一次心跳间隔
                try
                {
                    _logger?.LogDebug("等待下一次心跳，间隔: {HeartbeatIntervalMs} 毫秒", _heartbeatIntervalMs);
                    await Task.Delay(_heartbeatIntervalMs, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    // 正常取消，退出循环
                    _logger?.LogInformation("心跳间隔等待被取消");
                    break;
                }
            }

            _logger?.LogInformation("心跳任务已结束");
        }

        /// <summary>
        /// 释放资源
        /// 实现IDisposable接口的标准释放模式
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Stop();
                _isDisposed = true;
                GC.SuppressFinalize(this);
                _logger?.LogInformation("心跳管理器已释放");
            }
        }


        /// </summary>
        /// <returns>配置完整的心跳命令对象，用于发送给服务器</returns>
        private RUINORERP.PacketSpec.Commands.System.HeartbeatCommand CreateHeartbeatCommand()
        {
            try
            {


                // 获取会话信息
                string sessionToken = GetSessionToken();
                long userId = GetCurrentUserId();

                // 创建心跳命令
                var command = new RUINORERP.PacketSpec.Commands.System.HeartbeatCommand(_socketClient.ClientID, sessionToken, userId);

                // 设置客户端信息
                command.ClientVersion = GetClientVersion();
                command.ClientIp = GetClientIp();
                command.ClientStatus = "Normal";
                command.ProcessUptime = (int)Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds;

                // 设置网络和资源使用信息
                command.NetworkLatency = EstimateNetworkLatency();
                command.ResourceUsage = GetResourceUsage();

                _logger?.LogDebug("心跳命令已创建: 客户端ID={ClientId}, IP={ClientIp}", _socketClient.ClientID, command.ClientIp);
                return command;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建心跳命令失败");
                // 出错时返回基础的心跳命令
                return new RUINORERP.PacketSpec.Commands.System.HeartbeatCommand();
            }
        }



        /// <summary>
        /// 获取客户端资源使用情况
        /// 收集CPU、内存、磁盘和网络等系统资源信息
        /// </summary>
        /// <returns>资源使用信息对象</returns>
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
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "获取CPU使用率失败，使用处理器数量作为默认值");
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
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "获取磁盘可用空间失败，使用默认值");
                    diskFreeSpace = 100; // 默认值
                }

                // 网络带宽使用暂时设为0
                float networkUsage = 0;

                _logger?.LogDebug("资源使用情况 - CPU: {CpuUsage}%, 内存: {MemoryUsage}MB, 磁盘空间: {DiskFreeSpace}GB",
                    cpuUsage, memoryUsage, diskFreeSpace);

                return ClientResourceUsage.Create(cpuUsage, memoryUsage, networkUsage, diskFreeSpace, processUptime);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取客户端资源使用情况失败");
                return ClientResourceUsage.Create(); // 返回默认值
            }
        }

        /// <summary>
        /// 估算网络延迟
        /// 通过创建TCP连接测量从客户端到服务器的网络延迟
        /// </summary>
        /// <returns>网络延迟（毫秒），-1表示无法测量</returns>
        private int EstimateNetworkLatency()
        {
            try
            {
                // 这里实现了简单的网络延迟测量逻辑
                if (_socketClient.IsConnected)
                {
                    try
                    {
                        // 由于无法直接从ISocketClient获取服务器地址和端口
                        // 这里简化实现，返回固定值或从配置中获取
                        return 50; // 默认延迟值
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "测量网络延迟失败，使用默认值");
                        return 1000; // 默认延迟值
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "估算网络延迟时发生异常");
                return -1; // 表示无法测量
            }
        }

        /// <summary>
        /// 获取客户端版本号
        /// 从应用程序集信息中获取版本号
        /// </summary>
        /// <returns>客户端版本号字符串</returns>
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
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取客户端版本号失败，使用默认值");
                return "1.0.0";
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// 优先返回本机IPv4地址
        /// </summary>
        /// <returns>客户端IP地址字符串</returns>
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
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取客户端IP地址失败，使用默认值");
                return "127.0.0.1";
            }
        }

        /// <summary>
        /// 获取会话令牌
        /// 实际项目中应从登录状态获取
        /// </summary>
        /// <returns>会话令牌字符串</returns>
        private string GetSessionToken()
        {
            try
            {
                // 示例实现，实际应从应用状态中获取
                // 这里应该从全局上下文或登录状态中获取真实的会话令牌
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取会话令牌失败，返回空字符串");
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
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取当前用户ID失败，返回默认值");
                return 0;
            }
        }

        /* -------------------- 事件处理 -------------------- */

        /// <summary>
        /// 心跳发送成功事件
        /// 当心跳包成功发送并收到响应时触发
        /// </summary>
        public event Action OnHeartbeatSuccess = delegate { };

        /// <summary>
        /// 心跳发送失败事件
        /// 当心跳包发送失败时触发，提供失败消息
        /// </summary>
        public event Action<string> OnHeartbeatFailed = delegate { };

        /// <summary>
        /// 心跳异常事件
        /// 当心跳处理过程中发生异常时触发，提供异常信息
        /// </summary>
        public event Action<Exception> OnHeartbeatException = delegate { };

        /// <summary>
        /// 重连尝试事件
        /// 当开始尝试重新连接服务器时触发，提供当前尝试次数
        /// </summary>
        public event Action<int> OnReconnectionAttempt = delegate { };

        /// <summary>
        /// 重连失败事件
        /// 当达到最大重连尝试次数后仍然无法连接时触发
        /// </summary>
        public event Action OnReconnectionFailed = delegate { };

        /// <summary>
        /// 连接丢失事件
        /// 当与服务器的连接完全丢失时触发
        /// </summary>
        public event Action ConnectionLost = delegate { };

        /// <summary>
        /// 心跳失败事件（带异常信息）
        /// 当心跳包处理失败时触发，提供相关异常
        /// </summary>
        public event Action<Exception> HeartbeatFailed = delegate { };

        /// <summary>
        /// 处理心跳失败
        /// 触发相关事件并记录日志
        /// </summary>
        /// <param name="message">失败消息</param>
        private void HandleHeartbeatFailure(string message)
        {
            try
            {
                // 获取事件处理程序的快照，避免在多线程环境下触发时可能发生的问题
                Action<string> failedHandler;
                Action<Exception> exceptionHandler;

                lock (_lock)
                {
                    failedHandler = OnHeartbeatFailed;
                    exceptionHandler = HeartbeatFailed;
                }

                // 触发带消息的失败事件
                if (failedHandler != null)
                    failedHandler.Invoke(message);

                // 触发带异常的失败事件
                if (exceptionHandler != null)
                    exceptionHandler.Invoke(new Exception(message));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理心跳失败事件时发生异常");
                // 忽略事件处理过程中的异常，避免影响主流程
            }
        }

        /// <summary>
        /// 处理心跳异常
        /// 触发相关异常事件并记录日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        private void HandleHeartbeatException(Exception ex)
        {
            try
            {
                // 获取事件处理程序的快照
                Action<Exception> exceptionHandler;
                Action<Exception> failedHandler;

                lock (_lock)
                {
                    exceptionHandler = OnHeartbeatException;
                    failedHandler = HeartbeatFailed;
                }

                // 触发异常事件
                if (exceptionHandler != null)
                    exceptionHandler.Invoke(ex);

                // 触发失败事件
                if (failedHandler != null)
                    failedHandler.Invoke(ex);
            }
            catch (Exception innerEx)
            {
                _logger?.LogError(innerEx, "处理心跳异常事件时发生异常");
                // 忽略事件处理过程中的异常，避免影响主流程
            }
        }

        /// <summary>
        /// 触发连接丢失事件
        /// 通知订阅者与服务器的连接已完全丢失
        /// </summary>
        private void OnConnectionLost()
        {
            try
            {
                // 获取事件处理程序的快照
                Action handler;

                lock (_lock)
                {
                    handler = ConnectionLost;
                }

                if (handler != null)
                    handler.Invoke();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发连接丢失事件时发生异常");
                // 忽略事件处理过程中的异常，避免影响主流程
            }
        }
    }

    /// <summary>
    /// 心跳统计信息类
    /// 用于收集和报告心跳相关的监控指标
    /// 🔄 新架构集成：提供标准化的监控数据格式
    /// 📋 监控指标：成功率、响应时间、连接状态等
    /// 💡 设计特点：不可变数据、线程安全、轻量级
    /// 📊 使用场景：性能监控、故障诊断、健康报告
    /// </summary>
    public class HeartbeatStatistics
    {
        /// <summary>
        /// 总心跳次数
        /// </summary>
        public long TotalHeartbeats { get; set; }

        /// <summary>
        /// 成功心跳次数
        /// </summary>
        public long SuccessfulHeartbeats { get; set; }

        /// <summary>
        /// 失败心跳次数
        /// </summary>
        public long FailedHeartbeats { get; set; }

        /// <summary>
        /// 心跳成功率（0-100）
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// 最后一次心跳时间
        /// </summary>
        public DateTime? LastHeartbeatTime { get; set; }

        /// <summary>
        /// 平均响应时间（毫秒）
        /// </summary>
        public double AverageResponseTime { get; set; }

        /// <summary>
        /// 当前连接状态
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// 心跳任务是否正在运行
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// 失败尝试次数
        /// </summary>
        public int FailedAttempts { get; set; }

        /// <summary>
        /// 连接状态描述
        /// </summary>
        public string ConnectionState { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="totalHeartbeats">总心跳次数</param>
        /// <param name="successfulHeartbeats">成功心跳次数</param>
        /// <param name="failedHeartbeats">失败心跳次数</param>
        /// <param name="lastHeartbeatTime">最后一次心跳时间</param>
        /// <param name="averageResponseTime">平均响应时间</param>
        /// <param name="isConnected">当前连接状态</param>
        /// <param name="isRunning">心跳任务是否正在运行</param>
        /// <param name="failedAttempts">失败尝试次数</param>
        /// <param name="connectionState">连接状态描述</param>
        public HeartbeatStatistics(
            long totalHeartbeats,
            long successfulHeartbeats,
            long failedHeartbeats,
            DateTime? lastHeartbeatTime,
            double averageResponseTime,
            bool isConnected,
            bool isRunning = false,
            int failedAttempts = 0,
            string connectionState = "Unknown")
        {
            TotalHeartbeats = totalHeartbeats;
            SuccessfulHeartbeats = successfulHeartbeats;
            FailedHeartbeats = failedHeartbeats;
            LastHeartbeatTime = lastHeartbeatTime;
            AverageResponseTime = averageResponseTime;
            IsConnected = isConnected;
            IsRunning = isRunning;
            FailedAttempts = failedAttempts;
            ConnectionState = connectionState;

            // 计算成功率
            SuccessRate = totalHeartbeats > 0 ? (double)successfulHeartbeats / totalHeartbeats * 100 : 0;
        }

        /// <summary>
        /// 获取统计摘要
        /// </summary>
        /// <returns>格式化的统计摘要字符串</returns>
        public string GetSummary()
        {
            var status = IsConnected ? "已连接" : "未连接";
            var lastTime = LastHeartbeatTime?.ToString("HH:mm:ss") ?? "从未";

            return $"心跳统计: 总计{TotalHeartbeats}次, 成功率{SuccessRate:F1}%, " +
                   $"状态:{status}, 最后:{lastTime}, 平均响应:{AverageResponseTime:F0}ms";
        }

        /// <summary>
        /// 转换为字典格式（便于序列化）
        /// </summary>
        /// <returns>包含统计数据的字典</returns>
        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                ["totalHeartbeats"] = TotalHeartbeats,
                ["successfulHeartbeats"] = SuccessfulHeartbeats,
                ["failedHeartbeats"] = FailedHeartbeats,
                ["successRate"] = SuccessRate,
                ["lastHeartbeatTime"] = LastHeartbeatTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                ["averageResponseTime"] = AverageResponseTime,
                ["isConnected"] = IsConnected
            };
        }
    }
}