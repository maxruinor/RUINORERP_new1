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
using RUINORERP.UI.Network.Authentication;
using RUINORERP.UI.Network.Services;
using Microsoft.Extensions.Logging.Abstractions;
using RUINORERP.PacketSpec.Commands.Authentication;
using System.Linq;
using AutoUpdateTools;
using RUINORERP.Model.CommonModel;

namespace RUINORERP.UI.Network
{

    public class HeartbeatManager : IDisposable
    {
        private readonly ISocketClient _socketClient;
        private readonly ClientCommunicationService _communicationService;
        private readonly TokenManager _tokenManager;
        private readonly int _heartbeatIntervalMs;
        private readonly int _heartbeatTimeoutMs;
        private readonly int _resourceCheckIntervalMs; // 资源检查间隔
        private readonly int _maxFailedAttempts; // 最大连续失败次数
        private readonly int _minHeartbeatIntervalMs; // 最小心跳间隔
        private readonly int _maxHeartbeatIntervalMs; // 最大心跳间隔
        private CancellationTokenSource _cancellationTokenSource;
        private Task _heartbeatTask;
        private Task _resourceCheckTask; // 资源检查任务
        private int _failedAttempts;
        private bool _isDisposed;
        private readonly object _lock = new object();
        private readonly ILogger<HeartbeatManager> _logger;
        private int _totalHeartbeats;
        private int _successfulHeartbeats;
        private int _failedHeartbeats;
        private DateTime _lastHeartbeatTime;
        private int _currentHeartbeatIntervalMs; // 当前心跳间隔（用于自适应调整）
        private readonly Queue<int> _recentResponseTimes; // 最近的响应时间队列，用于自适应调整
        private const int MaxResponseTimeHistory = 10; // 保留的响应时间历史记录数量

        // 资源使用情况缓存
        private ClientResourceUsage _cachedResourceUsage;
        private DateTime _lastResourceCheckTime;


        public HeartbeatStatistics Statistics
        {
            get
            {
                lock (_lock)
                {
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
        /// 心跳管理器构造函数 - 直接使用ISocketClient和ClientCommunicationService
        /// </summary>
        /// <param name="socketClient">Socket客户端接口，用于直接发送心跳数据</param>
        /// <param name="communicationService">客户端通信服务，用于处理心跳请求和响应</param>
        /// <param name="tokenManager">Token管理器，用于检查Token状态</param>
        /// <param name="heartbeatIntervalMs">心跳间隔（毫秒）</param>
        /// <param name="heartbeatTimeoutMs">心跳超时时间（毫秒）</param>
        /// <param name="resourceCheckIntervalMs">资源检查间隔（毫秒），默认为5分钟</param>
        /// <param name="maxFailedAttempts">最大连续失败次数，超过此值判定连接断开，默认为3次</param>
        /// <param name="minHeartbeatIntervalMs">最小心跳间隔（毫秒），用于自适应调整，默认为10秒</param>
        /// <param name="maxHeartbeatIntervalMs">最大心跳间隔（毫秒），用于自适应调整，默认为2分钟</param>
        /// <param name="logger">日志记录器，可选参数，用于记录心跳过程中的信息和异常</param>
        public HeartbeatManager(
            ISocketClient socketClient,
            ClientCommunicationService communicationService,
            TokenManager tokenManager,
            int heartbeatIntervalMs,
            int heartbeatTimeoutMs = 5000,
            int resourceCheckIntervalMs = 600000, // 默认10分钟检查一次资源使用情况
            int maxFailedAttempts = 3, // 默认连续失败3次判定连接断开
            int minHeartbeatIntervalMs = 10000, // 默认最小心跳间隔10秒
            int maxHeartbeatIntervalMs = 120000, // 默认最大心跳间隔2分钟
            ILogger<HeartbeatManager> logger = null)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));

            // 参数验证
            if (heartbeatIntervalMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(heartbeatIntervalMs), "心跳间隔必须大于0");

            if (heartbeatTimeoutMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(heartbeatTimeoutMs), "心跳超时时间必须大于0");

            if (maxFailedAttempts <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxFailedAttempts), "最大失败次数必须大于0");

            if (minHeartbeatIntervalMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(minHeartbeatIntervalMs), "最小心跳间隔必须大于0");

            if (maxHeartbeatIntervalMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHeartbeatIntervalMs), "最大心跳间隔必须大于0");

            if (minHeartbeatIntervalMs > maxHeartbeatIntervalMs)
                throw new ArgumentException("最小心跳间隔不能大于最大心跳间隔");

            _heartbeatIntervalMs = heartbeatIntervalMs;
            _currentHeartbeatIntervalMs = heartbeatIntervalMs; // 初始化当前心跳间隔
            _heartbeatTimeoutMs = heartbeatTimeoutMs;
            _resourceCheckIntervalMs = resourceCheckIntervalMs;
            _maxFailedAttempts = maxFailedAttempts;
            _minHeartbeatIntervalMs = minHeartbeatIntervalMs;
            _maxHeartbeatIntervalMs = maxHeartbeatIntervalMs;
            _cancellationTokenSource = new CancellationTokenSource();
            _logger = logger ?? NullLogger<HeartbeatManager>.Instance;
            _failedAttempts = 0;
            _isDisposed = false;
            _totalHeartbeats = 0;
            _successfulHeartbeats = 0;
            _failedHeartbeats = 0;
            _lastHeartbeatTime = DateTime.MinValue;

            // 初始化资源缓存
            _cachedResourceUsage = ClientResourceUsage.Create(); // 初始默认值
            _lastResourceCheckTime = DateTime.MinValue;

            // 初始化响应时间队列
            _recentResponseTimes = new Queue<int>(MaxResponseTimeHistory);
        }

        /// <summary>
        /// 开始发送心跳
        /// 在单独的后台任务中定期执行心跳发送逻辑
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已被释放时抛出</exception>
        public void Start()
        {
            // 系统启用前暂停心跳，待业务稳定后启用
            //#warning 系统启用前暂停心跳，待业务稳定后启用
            //           return;

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

                // 启动资源检查任务
                if (_resourceCheckTask == null || _resourceCheckTask.IsCompleted)
                {
                    _resourceCheckTask = Task.Run(ResourceCheckLoopAsync, _cancellationTokenSource.Token);
                }

                _logger?.LogInformation("心跳任务已启动，间隔: {HeartbeatIntervalMs}毫秒，资源检查间隔: {ResourceCheckIntervalMs}毫秒",
                    _heartbeatIntervalMs, _resourceCheckIntervalMs);
            }
        }

        /// <summary>
        /// 停止发送心跳
        /// 取消当前心跳任务和资源检查任务并释放相关资源
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
                        _resourceCheckTask = null;
                        _failedAttempts = 0;
                        _logger?.LogInformation("心跳任务和资源检查任务已停止");
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "停止心跳任务时发生异常");
                    }
                }
            }
        }


        /// <summary>
        /// 发送心跳的公共方法
        /// 提取公共心跳发送逻辑，避免代码重复
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="isManual">是否为手动触发的心跳</param>
        /// <returns>心跳执行结果，包含成功状态、响应消息和执行时间</returns>
        private async Task<(bool Success, string Message, TimeSpan Elapsed)> SendHeartbeatCoreAsync(CancellationToken cancellationToken = default, bool isManual = false)
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
                // 创建心跳命令
                var heartbeatRequest = CreateHeartbeatRequest();

                // 使用ClientCommunicationService发送心跳请求
                var response = await _communicationService.SendCommandAsync(
                    SystemCommands.Heartbeat, heartbeatRequest,
                    cancellationToken,
                    _heartbeatTimeoutMs // 使用配置的心跳超时时间
                );

                // 处理心跳响应
                if (response != null)
                {
                    //客户端要处理服务器的数据

                    // 记录响应时间用于自适应调整
                    lock (_lock)
                    {
                        _recentResponseTimes.Enqueue((int)stopwatch.ElapsedMilliseconds);
                        // 保持队列大小不超过最大值
                        while (_recentResponseTimes.Count > MaxResponseTimeHistory)
                        {
                            _recentResponseTimes.Dequeue();
                        }
                    }

                    // 自适应调整心跳间隔
                    // AdjustHeartbeatInterval(response.NextIntervalMs);
                }

                stopwatch.Stop();

                // 更新统计信息
                lock (_lock)
                {
                    _totalHeartbeats++;
                    _successfulHeartbeats++;
                    _lastHeartbeatTime = DateTime.UtcNow;
                }

                _logger?.LogInformation("{HeartbeatType}心跳成功，耗时: {ElapsedMs}ms",
                    isManual ? "手动" : "自动", stopwatch.ElapsedMilliseconds);
                return (true, $"心跳成功", stopwatch.Elapsed);
            }
            catch (TimeoutException ex)
            {
                stopwatch.Stop();
                _logger?.LogWarning(ex, "{HeartbeatType}心跳超时，耗时: {ElapsedMs}ms",
                    isManual ? "手动" : "自动", stopwatch.ElapsedMilliseconds);

                // 记录响应时间用于自适应调整
                lock (_lock)
                {
                    _recentResponseTimes.Enqueue(_heartbeatTimeoutMs); // 超时情况下使用超时时间作为响应时间
                    // 保持队列大小不超过最大值
                    while (_recentResponseTimes.Count > MaxResponseTimeHistory)
                    {
                        _recentResponseTimes.Dequeue();
                    }
                }

                // 自适应调整心跳间隔
                AdjustHeartbeatInterval(0); // 0表示服务器没有建议间隔

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
                _logger?.LogWarning("{HeartbeatType}心跳被取消，耗时: {ElapsedMs}ms",
                    isManual ? "手动" : "自动", stopwatch.ElapsedMilliseconds);
                return (false, "心跳操作被取消", stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger?.LogError(ex, "{HeartbeatType}心跳异常，耗时: {ElapsedMs}ms",
                    isManual ? "手动" : "自动", stopwatch.ElapsedMilliseconds);

                // 记录响应时间用于自适应调整
                lock (_lock)
                {
                    _recentResponseTimes.Enqueue(_heartbeatTimeoutMs); // 异常情况下使用超时时间作为响应时间
                    // 保持队列大小不超过最大值
                    while (_recentResponseTimes.Count > MaxResponseTimeHistory)
                    {
                        _recentResponseTimes.Dequeue();
                    }
                }

                // 自适应调整心跳间隔
                AdjustHeartbeatInterval(0); // 0表示服务器没有建议间隔

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

        /// <summary>
        /// 自适应调整心跳间隔
        /// 根据网络状况和服务器建议动态调整心跳频率
        /// </summary>
        /// <param name="serverSuggestedInterval">服务器建议的心跳间隔，0表示没有建议</param>
        private void AdjustHeartbeatInterval(int serverSuggestedInterval)
        {
            lock (_lock)
            {
                int newInterval = _currentHeartbeatIntervalMs;

                // 优先使用服务器建议的间隔
                if (serverSuggestedInterval > 0)
                {
                    // 确保服务器建议的间隔在合理范围内
                    newInterval = Math.Max(_minHeartbeatIntervalMs, Math.Min(serverSuggestedInterval, _maxHeartbeatIntervalMs));
                    _logger?.LogInformation("根据服务器建议调整心跳间隔: {NewIntervalMs}ms", newInterval);
                }
                else
                {
                    // 根据最近的响应时间自适应调整
                    if (_recentResponseTimes.Count >= 3) // 至少需要3个样本才能进行自适应调整
                    {
                        var responseTimes = _recentResponseTimes.ToArray();
                        Array.Sort(responseTimes);

                        // 使用中位数作为平均响应时间的估计
                        int medianResponseTime = responseTimes[responseTimes.Length / 2];

                        // 计算响应时间的变化趋势
                        double responseTimeVariation = CalculateResponseTimeVariation(responseTimes);

                        // 根据响应时间和变化趋势调整心跳间隔
                        if (medianResponseTime > _heartbeatTimeoutMs * 0.8 || responseTimeVariation > 0.5)
                        {
                            // 响应时间较长或变化较大，增加心跳间隔
                            newInterval = Math.Min(_currentHeartbeatIntervalMs + 5000, _maxHeartbeatIntervalMs);
                            _logger?.LogDebug("网络状况不佳，增加心跳间隔至: {NewIntervalMs}ms", newInterval);
                        }
                        else if (medianResponseTime < _heartbeatTimeoutMs * 0.3 && responseTimeVariation < 0.2)
                        {
                            // 响应时间较短且稳定，减少心跳间隔
                            newInterval = Math.Max(_currentHeartbeatIntervalMs - 2000, _minHeartbeatIntervalMs);
                            _logger?.LogDebug("网络状况良好，减少心跳间隔至: {NewIntervalMs}ms", newInterval);
                        }
                    }
                }

                // 应用新的心跳间隔
                if (newInterval != _currentHeartbeatIntervalMs)
                {
                    _currentHeartbeatIntervalMs = newInterval;
                    _logger?.LogInformation("心跳间隔已调整为: {NewIntervalMs}ms", _currentHeartbeatIntervalMs);
                }
            }
        }

        /// <summary>
        /// 计算响应时间的变化率
        /// </summary>
        /// <param name="responseTimes">响应时间数组</param>
        /// <returns>响应时间的变化率（0-1之间，值越大表示变化越大）</returns>
        private double CalculateResponseTimeVariation(int[] responseTimes)
        {
            if (responseTimes.Length < 2)
                return 0;

            // 计算平均响应时间
            double average = responseTimes.Average();

            // 计算标准差
            double sumOfSquares = 0;
            foreach (int time in responseTimes)
            {
                sumOfSquares += Math.Pow(time - average, 2);
            }
            double standardDeviation = Math.Sqrt(sumOfSquares / responseTimes.Length);

            // 返回变异系数（标准差/平均值），作为变化率的度量
            return average > 0 ? standardDeviation / average : 0;
        }

        /// <summary>
        /// 手动发送心跳
        /// 提供手动测试心跳连接的公共接口
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>心跳执行结果，包含成功状态、响应消息和执行时间</returns>
        public async Task<(bool Success, string Message, TimeSpan Elapsed)> SendHeartbeatAsync(CancellationToken cancellationToken = default)
        {
            return await SendHeartbeatCoreAsync(cancellationToken, true);
        }


        /// <summary>
        /// 心跳发送循环
        /// 定期发送心跳并处理响应
        /// </summary>
        /// <returns>异步任务，心跳循环任务</returns>
        /// <summary>
        /// Token过期事件
        /// 当检测到Token已过期时触发，通知其他组件进行处理
        /// </summary>
        public event Action OnTokenExpired = delegate { };
        
        private async Task SendHeartbeatsAsync()
        {
            _logger?.LogInformation("心跳任务已启动，开始定期发送心跳");

            // 上次Token检查时间
            DateTime lastTokenCheckTime = DateTime.MinValue;
            // Token检查间隔（默认5分钟）
            TimeSpan tokenCheckInterval = TimeSpan.FromMinutes(5);

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    if (_socketClient.IsConnected)
                    {
                        // 定期检查Token状态（不必每次心跳都检查）
                        var now = DateTime.UtcNow;
                        if (now - lastTokenCheckTime >= tokenCheckInterval)
                        {
                            await CheckTokenValidityAsync();
                            lastTokenCheckTime = now;
                        }

                        // 使用公共心跳发送方法
                        var result = await SendHeartbeatCoreAsync(_cancellationTokenSource.Token, false);

                        if (result.Success)
                        {
                            // 心跳发送成功
                            lock (_lock)
                            {
                                _failedAttempts = 0;
                            }
                            OnHeartbeatSuccess();
                        }
                        else
                        {
                            // 心跳发送失败
                            lock (_lock)
                            {
                                _failedAttempts++;
                            }
                            HandleHeartbeatFailure(result.Message);

                            // 检查是否达到最大失败次数
                            if (_failedAttempts >= _maxFailedAttempts)
                            {
                                _logger?.LogError("连续心跳失败次数({FailedAttempts})达到最大阈值({MaxFailedAttempts})，判定连接已断开",
                                    _failedAttempts, _maxFailedAttempts);
                                OnConnectionLost();

                                // 重置失败计数器，避免重复触发连接丢失事件
                                lock (_lock)
                                {
                                    _failedAttempts = 0;
                                }
                            }
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
                    lock (_lock)
                    {
                        _failedAttempts++;
                    }
                    _logger?.LogError(ex, "心跳处理过程中发生未预期的异常，连续失败次数: {FailedAttempts}", _failedAttempts);
                    HandleHeartbeatException(ex);

                    // 检查是否达到最大失败次数
                    if (_failedAttempts >= _maxFailedAttempts)
                    {
                        _logger?.LogError("连续心跳异常次数({FailedAttempts})达到最大阈值({MaxFailedAttempts})，判定连接已断开",
                            _failedAttempts, _maxFailedAttempts);
                        OnConnectionLost();

                        // 重置失败计数器，避免重复触发连接丢失事件
                        lock (_lock)
                        {
                            _failedAttempts = 0;
                        }
                    }
                }

                // 等待下一次心跳间隔，使用自适应间隔
                try
                {
                    int currentInterval;
                    lock (_lock)
                    {
                        currentInterval = _currentHeartbeatIntervalMs;
                    }

                    _logger?.LogDebug("等待下一次心跳，间隔: {HeartbeatIntervalMs} 毫秒", currentInterval);
                    await Task.Delay(currentInterval, _cancellationTokenSource.Token);
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
        /// 资源检查循环
        /// 定期更新资源使用情况缓存
        /// </summary>
        /// <returns>异步任务</returns>
        private async Task ResourceCheckLoopAsync()
        {
            _logger?.LogInformation("资源检查任务已启动，间隔: {ResourceCheckIntervalMs}毫秒", _resourceCheckIntervalMs);

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    // 强制更新资源使用情况缓存
                    var now = DateTime.UtcNow;

                    // 检查是否需要更新缓存
                    bool needsUpdate = _lastResourceCheckTime == DateTime.MinValue ||
                                     (now - _lastResourceCheckTime).TotalMilliseconds >= _resourceCheckIntervalMs;

                    if (needsUpdate)
                    {
                        _logger?.LogDebug("开始更新资源使用情况缓存");

                        // 在后台线程中获取资源使用情况
                        await Task.Run(() =>
                        {
                            try
                            {
                                // 获取进程信息
                                var process = Process.GetCurrentProcess();

                                // 获取内存使用量（MB）
                                long memoryUsage = process.WorkingSet64 / (1024 * 1024);

                                // 获取进程运行时间（秒）
                                long processUptime = (long)(now - process.StartTime.ToUniversalTime()).TotalSeconds;

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

                                _logger?.LogDebug("资源使用情况已更新 - CPU: {CpuUsage}%, 内存: {MemoryUsage}MB, 磁盘空间: {DiskFreeSpace}GB",
                                    cpuUsage, memoryUsage, diskFreeSpace);

                                var resourceUsage = ClientResourceUsage.Create(cpuUsage, memoryUsage, networkUsage, diskFreeSpace, processUptime);

                                // 更新缓存
                                lock (_lock)
                                {
                                    _cachedResourceUsage = resourceUsage;
                                    _lastResourceCheckTime = now;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, "后台更新资源使用情况失败");
                            }
                        }, _cancellationTokenSource.Token);
                    }
                }
                catch (TaskCanceledException)
                {
                    // 任务被取消，正常退出
                    _logger?.LogInformation("资源检查任务收到取消信号，准备退出");
                    break;
                }
                catch (OperationCanceledException ex)
                {
                    // 操作被取消，正常退出
                    _logger?.LogInformation(ex, "资源检查操作被取消");
                    break;
                }
                catch (Exception ex)
                {
                    // 处理其他异常
                    _logger?.LogError(ex, "资源检查过程中发生未预期的异常");
                }

                // 等待下一次资源检查间隔
                try
                {
                    _logger?.LogDebug("等待下一次资源检查，间隔: {ResourceCheckIntervalMs} 毫秒", _resourceCheckIntervalMs);
                    await Task.Delay(_resourceCheckIntervalMs, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    // 正常取消，退出循环
                    _logger?.LogInformation("资源检查间隔等待被取消");
                    break;
                }
            }

            _logger?.LogInformation("资源检查任务已结束");
        }

        /// <summary>
        /// 释放资源
        /// 实现IDisposable接口的标准释放模式
        /// <summary>
        /// 释放资源
        /// 实现IDisposable接口，确保所有资源被正确释放
        /// </summary>
        public void Dispose()
        {            lock (_lock)
            {
                if (!_isDisposed)
                {
                    try
                    {
                        // 停止心跳任务
                        Stop();

                        // 清理事件处理器
                        OnHeartbeatSuccess = null;
                        OnHeartbeatFailed = null;
                        OnHeartbeatException = null;
                        OnReconnectionAttempt = null;
                        OnReconnectionFailed = null;
                        ConnectionLost = null;
                        HeartbeatFailed = null;
                        OnTokenExpired = null; // 清理新增的Token过期事件

                        // 清理响应时间队列
                        _recentResponseTimes.Clear();

                        // 标记为已释放
                        _isDisposed = true;

                        GC.SuppressFinalize(this);
                        _logger?.LogInformation("心跳管理器资源已释放");
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "释放心跳管理器资源时发生异常");
                    }
                }
            }
        }


        /// </summary>
        /// <returns>配置完整的心跳命令对象，用于发送给服务器</returns>
        private HeartbeatRequest CreateHeartbeatRequest()
        {
            try
            {
                // 创建心跳命令
                HeartbeatRequest heartbeatRequest = new HeartbeatRequest();
                // 设置客户端信息
                heartbeatRequest.ClientVersion = GetClientVersion();
                
                // 避免直接依赖MainForm.Instance，使用更可靠的方式获取用户信息
                if (_tokenManager != null)
                {
                    try
                    {
                        var tokenInfo = _tokenManager.TokenStorage.GetTokenAsync().GetAwaiter().GetResult();
                        if (tokenInfo != null)
                        {
                            // TokenInfo不包含UserInfo属性，这里使用会话ID和用户信息从MainForm获取
                            // 后续可以考虑通过Token验证结果获取用户信息
                        }
                    }
                    catch (Exception tokenEx)
                    {
                        _logger?.LogWarning(tokenEx, "从TokenManager获取Token信息失败");
                    }
                }
                
                // 如果通过TokenManager获取失败，再尝试使用MainForm
                if (heartbeatRequest.UserInfo == null)
                {
                    try
                    {
                        if (MainForm.Instance?.AppContext != null)
                        {
                            heartbeatRequest.UserInfo = MainForm.Instance.AppContext.CurrentUser ?? new Model.CommonModel.UserInfo();
                            if (heartbeatRequest.UserInfo != null && !string.IsNullOrEmpty(MainForm.Instance.AppContext.SessionId))
                            {
                                heartbeatRequest.UserInfo.SessionId = MainForm.Instance.AppContext.SessionId;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "从MainForm获取用户信息失败");
                    }
                }

                heartbeatRequest.ClientStatus = "Normal";

                // 设置网络和资源使用信息
                heartbeatRequest.NetworkLatency = EstimateNetworkLatency();
                heartbeatRequest.ResourceUsage = GetResourceUsage();

                return heartbeatRequest;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建心跳命令失败");
                // 出错时返回基础的心跳命令，但包含基本信息
                return new HeartbeatRequest
                {
                    ClientVersion = GetClientVersion(),
                    ClientStatus = "ErrorCreatingRequest"
                };
            }
        }



        /// <summary>
        /// 获取客户端资源使用情况（带缓存）
        /// 优先返回缓存的资源使用信息，避免频繁查询系统资源
        /// </summary>
        /// <returns>资源使用信息对象</returns>
        private ClientResourceUsage GetResourceUsage()
        {
            try
            {
                // 检查缓存是否有效（5分钟内）
                var now = DateTime.UtcNow;
                if (_cachedResourceUsage != null &&
                    _lastResourceCheckTime != DateTime.MinValue &&
                    (now - _lastResourceCheckTime).TotalMinutes < 5)
                {
                    _logger?.LogDebug("使用缓存的资源使用情况");
                    return _cachedResourceUsage;
                }

                // 获取进程信息
                var process = Process.GetCurrentProcess();

                // 获取内存使用量（MB）
                long memoryUsage = process.WorkingSet64 / (1024 * 1024);

                // 获取进程运行时间（秒）
                long processUptime = (long)(now - process.StartTime.ToUniversalTime()).TotalSeconds;

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

                var resourceUsage = ClientResourceUsage.Create(cpuUsage, memoryUsage, networkUsage, diskFreeSpace, processUptime);

                // 更新缓存
                lock (_lock)
                {
                    _cachedResourceUsage = resourceUsage;
                    _lastResourceCheckTime = now;
                }

                return resourceUsage;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取客户端资源使用情况失败");

                // 返回缓存（如果存在）或默认值
                return _cachedResourceUsage ?? ClientResourceUsage.Create();
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
                _logger?.LogWarning("心跳失败: {Message}", message);
                
                // 检查是否是Token相关错误
                if (message.Contains("token") || message.Contains("Token") || message.Contains("过期") || message.Contains("invalid"))
                {
                    _logger?.LogWarning("心跳失败可能与Token相关，将触发Token过期检查");
                    // 异步检查Token有效性，不阻塞当前流程
                    _ = Task.Run(async () => 
                    {
                        try
                        {
                            await CheckTokenValidityAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "异步检查Token有效性时发生异常");
                        }
                    });
                }
                
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
                {
                    try
                    {
                        failedHandler.Invoke(message);
                    }
                    catch (Exception handlerEx)
                    {
                        _logger?.LogError(handlerEx, "执行OnHeartbeatFailed事件处理器时发生异常");
                    }
                }

                // 触发带异常的失败事件
                if (exceptionHandler != null)
                {
                    try
                    {
                        exceptionHandler.Invoke(new Exception(message));
                    }
                    catch (Exception handlerEx)
                    {
                        _logger?.LogError(handlerEx, "执行HeartbeatFailed事件处理器时发生异常");
                    }
                }
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
        
        /// <summary>
        /// 检查Token有效性
        /// 验证存储的Token是否有效，如无效则触发Token过期事件
        /// </summary>
        /// <returns>异步任务</returns>
        private async Task CheckTokenValidityAsync()
        {
            try
            {
                if (_tokenManager != null)
                {
                    var tokenInfo = await _tokenManager.TokenStorage.GetTokenAsync();
                    if (tokenInfo != null)
                    {
                        var validationResult = await _tokenManager.ValidateStoredTokenAsync();
                        if (!validationResult.IsValid)
                        {
                            _logger?.LogInformation("检测到AccessToken已过期或无效，触发Token过期事件");
                            
                            // 触发Token过期事件
                            Action tokenExpiredHandler;
                            lock (_lock)
                            {
                                tokenExpiredHandler = OnTokenExpired;
                            }
                            
                            if (tokenExpiredHandler != null)
                            {
                                try
                                {
                                    tokenExpiredHandler.Invoke();
                                }
                                catch (Exception ex)
                                {
                                    _logger?.LogError(ex, "处理Token过期事件时发生异常");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查Token有效性时发生异常");
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