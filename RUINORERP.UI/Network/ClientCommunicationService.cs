using FastReport.DevComponents.DotNetBar;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Ocsp;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model.Context;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Core.DataProcessing;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.Authentication;
using RUINORERP.UI.Network.ClientCommandHandlers;
using RUINORERP.UI.Network.ErrorHandling;
using RUINORERP.UI.Network.Exceptions;
using RUINORERP.UI.Network.RetryStrategy;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.Network.TimeoutStatistics;
using RUINORERP.UI.SysConfig;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 心跳失败类型枚举
    /// 用于区分不同原因导致的心跳失败，采取不同的处理策略
    /// </summary>
    internal enum HeartbeatFailureType
    {
        /// <summary>超时 - 网络延迟高或服务器响应慢</summary>
        Timeout,
        /// <summary>网络错误 - 网络连接问题</summary>
        NetworkError,
        /// <summary>服务器繁忙 - 服务器处理能力不足</summary>
        ServerBusy,
        /// <summary>会话过期 - 会话已失效需要重新登录</summary>
        SessionExpired,
        /// <summary>未知原因 - 无法确定具体原因</summary>
        Unknown
    }

    /// <summary>
    /// 心跳失败追踪器 - 智能检测是否需要锁定
    /// 根据失败类型和时间模式判断是否达到锁定条件
    /// </summary>
    internal class HeartbeatFailureTracker
    {
        private readonly Dictionary<HeartbeatFailureType, Queue<DateTime>> _failureHistory = new Dictionary<HeartbeatFailureType, Queue<DateTime>>();
        private readonly ILogger _logger;

        private const int HISTORY_WINDOW = 5; // 保留最近5次失败时间

        /// <summary>
        /// 构造函数
        /// </summary>
        public HeartbeatFailureTracker(ILogger logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// 记录失败
        /// </summary>
        public void RecordFailure(HeartbeatFailureType type)
        {
            if (!_failureHistory.ContainsKey(type))
                _failureHistory[type] = new Queue<DateTime>();

            _failureHistory[type].Enqueue(DateTime.Now);

            // 只保留最近的HISTORY_WINDOW次失败
            while (_failureHistory[type].Count > HISTORY_WINDOW)
                _failureHistory[type].Dequeue();
        }

        /// <summary>
        /// 判断是否应该触发锁定
        /// 如果在短时间内(2分钟)发生3次同类型失败，则锁定
        /// </summary>
        public bool ShouldTriggerLockout()
        {
            foreach (var kvp in _failureHistory)
            {
                var failures = kvp.Value;

                if (failures.Count < 3)
                    continue;

                // 检查最近的失败是否在短时间内
                // 兼容.NET Framework: 使用Skip替代TakeLast
                var skipCount = failures.Count - 3;
                var recentFailures = failures.Skip(skipCount).ToList();
                var timeSpan = recentFailures[2] - recentFailures[0];

                // 如果3次失败在2分钟内，触发锁定
                if (timeSpan.TotalMinutes <= 2)
                {
                    _logger?.LogDebug("智能锁定检测：检测到同类型失败 {Count} 次，时间间隔 {TimeSpan} 分钟，触发锁定",
                        recentFailures.Count, timeSpan.TotalMinutes);
                    return true;
                }
            }

            // 检查所有类型的失败总数
            int totalFailures = _failureHistory.Sum(kvp => kvp.Value.Count);
            if (totalFailures >= 5) // 如果总失败次数达到5次，也触发锁定
            {
                _logger?.LogDebug("智能锁定检测：检测到总失败 {TotalFailures} 次，触发锁定", totalFailures);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 重置失败记录
        /// </summary>
        public void Reset()
        {
            foreach (var queue in _failureHistory.Values)
                queue.Clear();
        }
    }

    /// <summary>
    /// 优化后的客户端通信与命令处理服务 - 统一网络通信核心组件
    /// 简化版：专注于命令发送和接收，连接管理委托给ConnectionManager，集成心跳检测功能
    /// </summary>
    public class ClientCommunicationService : IClientCommunicationService, IDisposable
    {
        #region 私有字段

        /// <summary>
        /// 用户登录服务实例，用于重连后的认证恢复
        /// </summary>
        private UserLoginService _userLoginService;

        /// <summary>
        /// Socket客户端，负责实际的网络通信
        /// </summary>
        private readonly ISocketClient _socketClient;

        /// <summary>
        /// 连接管理器，负责统一的连接管理和重连逻辑
        /// </summary>
        private readonly ConnectionManager _connectionManager;

        /// <summary>
        /// 客户端事件管理器，管理连接状态和命令接收事件
        /// </summary>
        private readonly ClientEventManager _clientEventManager;

        /// <summary>
        /// 命令处理器集合
        /// </summary>
        private readonly IEnumerable<ICommandHandler> _commandHandlers;

        /// <summary>
        /// 命令调度器，用于分发命令到对应的处理类
        /// </summary>
        private readonly IClientCommandDispatcher _commandDispatcher;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<ClientCommunicationService> _logger;

        /// <summary>
        /// Token管理器
        /// </summary>
        private readonly TokenManager _tokenManager;

        // 新增心跳相关字段
        private int _heartbeatIntervalMs = 30000; // 默认30秒心跳间隔（与服务器端保持一致）
        private int _baseHeartbeatIntervalMs = 30000; // 基础心跳间隔
        private int _minHeartbeatIntervalMs = 10000; // 最小心跳间隔10秒
        private int _maxHeartbeatIntervalMs = 120000; // 最大心跳间隔2分钟
        private int _networkQualityThresholdGood = 100; // 网络质量良好的阈值（毫秒）
        private int _networkQualityThresholdPoor = 500; // 网络质量差的阈值（毫秒）
        private int _heartbeatTimeoutMs = 60000; // 心跳超时时间（60秒）
        private CancellationTokenSource _heartbeatCancellationTokenSource;
        private CancellationTokenSource _heartbeatCts; // 心跳取消令牌源
        private Model.Context.ApplicationContext _applicationContext;
        private Task _heartbeatTask;
        private int _heartbeatFailedAttempts;
        private bool _isHeartbeatRunning;
        private readonly Queue<double> _latencyHistory = new Queue<double>();
        private readonly int _maxLatencyHistory = 10; // 延迟历史记录的最大数量
        private HeartbeatFailureTracker _heartbeatFailureTracker; // 心跳失败追踪器
        private int _totalHeartbeatAttempts = 0; // 总心跳尝试次数
        private int _totalHeartbeatSuccess = 0; // 总心跳成功次数
        private int _totalHeartbeatFailures = 0; // 总心跳失败次数



        /// <summary>
        /// 请求响应管理相关字段
        /// </summary>
        private readonly ConcurrentDictionary<string, PendingRequest> _pendingRequests = new();

        /// <summary>
        /// 待发送命令队列，用于连接断开时的命令缓存
        /// </summary>
        private readonly ConcurrentQueue<ClientQueuedCommand> _queuedCommands = new();
        private readonly SemaphoreSlim _queueLock = new SemaphoreSlim(1, 1);
        private bool _isProcessingQueue = false;
        private bool _isReconnecting = false; // 重连状态标志
        private bool _isDisposed = false; // 资源释放状态标志
        private readonly object _heartbeatLock = new object();
        private DateTime _lastHeartbeatTime;
        private readonly object _reconnectCoordinationLock = new object(); // 新增：重连协调锁
        private DateTime _lastManualReconnectAttempt = DateTime.MinValue; // 新增：最后一次手动重连尝试时间

        /// <summary>
        /// 待处理请求的内部类
        /// </summary>
        private class PendingRequest
        {
            public TaskCompletionSource<PacketModel> Tcs { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CommandId { get; set; }
        }


        private Timer _cleanupTimer;
        // 是否已释放资源
        private bool _disposed = false;
        #endregion

        #region 公共属性

        /// <summary>
        /// 连接管理器 - 提供连接状态管理
        /// </summary>
        public ConnectionManager ConnectionManager => _connectionManager;

        /// <summary>
        /// 获取当前连接的服务器地址
        /// </summary>
        public string GetCurrentServerAddress() => _connectionManager.CurrentServerAddress;

        /// <summary>
        /// 获取当前连接的服务器端口
        /// </summary>
        public int GetCurrentServerPort() => _connectionManager.CurrentServerPort;

        #endregion

        #region 心跳相关公共属性和事件

        /// <summary>
        /// 心跳失败阈值
        /// 连续心跳失败达到此阈值时触发客户端锁定
        /// 调整为3次，避免过长的不确定状态
        /// </summary>
        public const int HEARTBEAT_FAILURE_THRESHOLD = 3;

        /// <summary>
        /// 心跳失败事件
        /// 当心跳失败时触发，参数为连续失败次数
        /// </summary>
        public event Action<int> HeartbeatFailed;

        /// <summary>
        /// 心跳恢复事件
        /// 当心跳从失败状态恢复时触发
        /// </summary>
        public event Action HeartbeatRecovered;

        /// <summary>
        /// 心跳失败达到阈值事件
        /// 当连续心跳失败次数达到阈值时触发
        /// </summary>
        public event Action HeartbeatFailureThresholdReached;

        /// <summary>
        /// 会话过期事件
        /// 当检测到会话过期时触发，需要重新登录
        /// </summary>
        public event Action SessionExpired;

        /// <summary>
        /// 本地备用重连失败事件，当_clientEventManager失败时使用
        /// </summary>
        private event Action _fallbackReconnectFailed;

        /// <summary>
        /// 最后一次心跳时间
        /// </summary>
        public DateTime LastHeartbeatTime => _lastHeartbeatTime;

        /// <summary>
        /// 当前心跳失败次数
        /// </summary>
        public int CurrentHeartbeatFailedAttempts => _heartbeatFailedAttempts;

        /// <summary>
        /// 获取当前队列中的命令数量
        /// </summary>
        public int QueuedCommandCount => _queuedCommands.Count;

        /// <summary>
        /// 获取当前待处理响应的数量
        /// </summary>
        public int PendingResponseCount => _pendingRequests.Count;

        /// <summary>
        /// 检查是否正在进行重连操作
        /// </summary>
        public bool IsReconnecting => _connectionManager.IsReconnecting;

        #endregion

        #region 网络质量评估方法

        /// <summary>
        /// 记录网络延迟
        /// </summary>
        /// <param name="latencyMs">延迟时间（毫秒）</param>
        private void RecordLatency(double latencyMs)
        {
            _latencyHistory.Enqueue(latencyMs);
            if (_latencyHistory.Count > _maxLatencyHistory)
            {
                _latencyHistory.Dequeue();
            }
        }

        /// <summary>
        /// 计算平均延迟
        /// </summary>
        /// <returns>平均延迟时间</returns>
        private double GetAverageLatency()
        {
            if (_latencyHistory.Count == 0) return 0;
            return _latencyHistory.Average();
        }

        /// <summary>
        /// 根据网络质量动态调整心跳间隔
        /// </summary>
        private void AdjustHeartbeatInterval()
        {
            var avgLatency = GetAverageLatency();

            if (avgLatency == 0) return; // 没有延迟数据，不调整

            int newInterval;
            if (avgLatency < _networkQualityThresholdGood) // 网络良好
            {
                newInterval = Math.Max(_baseHeartbeatIntervalMs / 2, _minHeartbeatIntervalMs); // 减少心跳间隔，但不低于最小值
            }
            else if (avgLatency < _networkQualityThresholdPoor) // 网络一般
            {
                newInterval = _baseHeartbeatIntervalMs; // 使用基础间隔
            }
            else // 网络较差
            {
                newInterval = Math.Min(_baseHeartbeatIntervalMs * 2, _maxHeartbeatIntervalMs); // 增加心跳间隔，但不超过最大值
            }

            // 如果心跳失败次数较多，适当增加间隔
            if (_heartbeatFailedAttempts > 1)
            {
                newInterval = Math.Min(newInterval * (_heartbeatFailedAttempts + 1), _maxHeartbeatIntervalMs);
            }

            _heartbeatIntervalMs = newInterval;
        }

        #endregion



        #region 构造函数

        /// <summary>
        /// 设置用户登录服务实例
        /// </summary>
        /// <param name="loginService">用户登录服务</param>
        public void SetUserLoginService(UserLoginService loginService)
        {
            _userLoginService = loginService;
        }

        /// <summary>
        /// 构造函数 - 集成心跳检测功能版本
        /// </summary>
        /// <param name="socketClient">Socket客户端</param>
        /// <param name="connectionManager">连接管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="tokenManager">令牌管理器</param>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="clientEventManager">客户端事件管理器</param>
        /// <param name="commandHandlers">命令处理器集合</param>
        public ClientCommunicationService(
            ISocketClient socketClient,
            ConnectionManager connectionManager,
            ILogger<ClientCommunicationService> logger,
            TokenManager tokenManager,
            IClientCommandDispatcher commandDispatcher,
            ClientEventManager clientEventManager,
            IEnumerable<ICommandHandler> commandHandlers)
        {
            // 参数验证
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _clientEventManager = clientEventManager ?? throw new ArgumentNullException(nameof(clientEventManager));
            _commandHandlers = commandHandlers ?? throw new ArgumentNullException(nameof(commandHandlers));
            _applicationContext = Startup.GetFromFac<Model.Context.ApplicationContext>();
            // 初始化心跳相关字段
            _heartbeatFailedAttempts = 0;
            _isHeartbeatRunning = false;
            _lastHeartbeatTime = DateTime.MinValue;
            _heartbeatFailureTracker = new HeartbeatFailureTracker(_logger);

            // 初始化定时清理任务
            _cleanupTimer = new Timer(CleanupTimeoutRequests, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            UI.Common.HardwareInfo hardwareInfo = Startup.GetFromFac<HardwareInfo>();
            if (string.IsNullOrEmpty(_socketClient.ClientID))
            {
                _socketClient.ClientID = hardwareInfo.GenerateClientId();
            }

            // 订阅事件
            _socketClient.Received -= OnReceived;
            _socketClient.Received += OnReceived;

            _connectionManager.ConnectionStateChanged -= OnConnectionStateChanged;
            _connectionManager.ConnectionStateChanged += OnConnectionStateChanged;

            // 订阅连接状态变化事件以管理心跳
            _connectionManager.ConnectionStateChanged -= OnConnectionStateChangedForHeartbeat;
            _connectionManager.ConnectionStateChanged += OnConnectionStateChangedForHeartbeat;

            // 订阅重连相关事件
            _connectionManager.ReconnectFailed -= OnReconnectFailed;
            _connectionManager.ReconnectFailed += OnReconnectFailed;

            _connectionManager.ReconnectAttempt -= OnReconnectAttempt;
            _connectionManager.ReconnectAttempt += OnReconnectAttempt;

            _connectionManager.ReconnectSucceeded -= OnReconnectSucceeded;
            _connectionManager.ReconnectSucceeded += OnReconnectSucceeded;

            // 注意：不再在构造函数中初始化命令调度器，以避免循环依赖
            // 而是通过外部调用InitializeClientCommandDispatcherAsync方法进行初始化
        }

        #endregion



        #region 命令调度器初始化方法

        /// <summary>
        /// 初始化客户端命令调度器
        /// 此方法通过依赖注入容器调用，用于避免构造函数中的循环依赖问题
        /// </summary>
        /// <returns>初始化结果和注册的处理器数量</returns>
        private async Task<(bool success, int registeredCount)> InitializeClientCommandDispatcherAsync()
        {
            try
            {
                _logger?.LogDebug("开始初始化客户端命令调度器");

                // 使用一键式初始化方法
                var result = await _commandDispatcher.InitializeAndStartAsync();

                _logger?.LogDebug($"客户端命令调度器初始化完成，结果: {{result.success}}, 注册处理器数: {{result.registeredCount}}");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化客户端命令调度器时发生异常");
                return (false, 0);
            }
        }

        #endregion

        #region 心跳检测相关方法

        /// <summary>
        /// 处理连接状态变化以管理心跳检测
        /// </summary>
        /// <param name="isConnected">是否已连接</param>
        private void OnConnectionStateChangedForHeartbeat(bool isConnected)
        {
            if (!isConnected)
            {
                // 连接断开，停止心跳
                StopHeartbeat();
            }
            // 连接成功时不再自动启动心跳，心跳将在登录成功后手动启动
        }

        /// <summary>
        /// 处理连接管理器重连失败事件
        /// </summary>
        private void OnReconnectFailed()
        {
            try
            {
                _logger?.LogWarning("连接管理器报告重连失败，触发客户端事件管理器的重连失败事件");

                lock (_reconnectCoordinationLock)
                {
                    _isReconnecting = false;
                }

                // 显示重连失败信息到UI
                try
                {
                    if (MainForm.Instance != null && !MainForm.Instance.IsDisposed)
                    {
                        if (MainForm.Instance.InvokeRequired)
                        {
                            MainForm.Instance.BeginInvoke(new Action(() =>
                            {
                                // 使用状态标签显示重连失败信息
                                string statusText = "重连失败，请检查网络连接或手动重试";
                                MainForm.Instance.ShowStatusText(statusText);

                                // 同时添加到日志
                                MainForm.Instance.PrintInfoLog("重连失败，已达到最大重试次数");
                            }));
                        }
                        else
                        {
                            // 使用状态标签显示重连失败信息
                            string statusText = "重连失败，请检查网络连接或手动重试";
                            MainForm.Instance.ShowStatusText(statusText);

                            // 同时添加到日志
                            MainForm.Instance.PrintInfoLog("重连失败，已达到最大重试次数");
                        }
                    }
                }
                catch (Exception uiEx)
                {
                    _logger?.LogWarning(uiEx, "更新重连失败UI时发生异常");
                }

                // 清空队列中的待处理命令，避免长时间等待
                ClearQueue("重连失败");

                // 触发客户端事件管理器的重连失败事件，添加容错处理
                try
                {
                    _clientEventManager.OnReconnectFailed();
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "触发客户端事件管理器重连失败事件时发生异常");

                    // 如果事件管理器失败，直接触发本地事件作为备用方案
                    try
                    {
                        _logger?.LogWarning("事件管理器失败，尝试触发本地备用重连失败事件");
                        _fallbackReconnectFailed?.Invoke();
                    }
                    catch (Exception fallbackEx)
                    {
                        _logger?.LogError(fallbackEx, "备用重连失败事件触发也失败");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理重连失败事件时发生异常");
            }
        }

        /// <summary>
        /// 处理连接管理器重连尝试事件
        /// </summary>
        /// <param name="currentAttempt">当前尝试次数</param>
        /// <param name="maxAttempts">最大尝试次数</param>
        private void OnReconnectAttempt(int currentAttempt, int maxAttempts)
        {
            try
            {
                _logger?.LogDebug("重连尝试：第 {CurrentAttempt}/{MaxAttempts} 次", currentAttempt, maxAttempts);

                // 显示重连状态到UI
                try
                {
                    if (MainForm.Instance != null && !MainForm.Instance.IsDisposed)
                    {
                        if (MainForm.Instance.InvokeRequired)
                        {
                            MainForm.Instance.BeginInvoke(new Action(() =>
                            {
                                // 使用状态标签显示重连信息
                                string statusText = $"正在尝试重新连接服务器... ({currentAttempt}/{maxAttempts})";
                                MainForm.Instance.ShowStatusText(statusText);

                                // 同时添加到日志
                                MainForm.Instance.PrintInfoLog($"重连尝试：第 {currentAttempt}/{maxAttempts} 次");
                            }));
                        }
                        else
                        {
                            // 使用状态标签显示重连信息
                            string statusText = $"正在尝试重新连接服务器... ({currentAttempt}/{maxAttempts})";
                            MainForm.Instance.ShowStatusText(statusText);

                            // 同时添加到日志
                            MainForm.Instance.PrintInfoLog($"重连尝试：第 {currentAttempt}/{maxAttempts} 次");
                        }
                    }
                }
                catch (Exception uiEx)
                {
                    _logger?.LogWarning(uiEx, "更新重连状态UI时发生异常");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理重连尝试事件时发生异常");
            }
        }

        /// <summary>
        /// 处理连接管理器重连成功事件
        /// </summary>
        private void OnReconnectSucceeded()
        {
            try
            {
                _logger?.LogDebug("重连成功，开始处理排队的命令");

                lock (_reconnectCoordinationLock)
                {
                    _isReconnecting = false;
                }

                // 显示重连成功信息到UI
                try
                {
                    if (MainForm.Instance != null && !MainForm.Instance.IsDisposed)
                    {
                        if (MainForm.Instance.InvokeRequired)
                        {
                            MainForm.Instance.BeginInvoke(new Action(() =>
                            {
                                // 使用状态标签显示重连成功信息
                                string statusText = "已成功重新连接到服务器";
                                MainForm.Instance.ShowStatusText(statusText);

                                // 同时添加到日志
                                MainForm.Instance.PrintInfoLog("重连成功，已恢复与服务器的连接");


                            }));
                        }
                        else
                        {
                            // 使用状态标签显示重连成功信息
                            string statusText = "已成功重新连接到服务器";
                            MainForm.Instance.ShowStatusText(statusText);

                            // 同时添加到日志
                            MainForm.Instance.PrintInfoLog("重连成功，已恢复与服务器的连接");

                            // 如果之前是锁定状态，现在应该解除锁定
                            if (MainForm.Instance.IsLocked)
                            {
                                MainForm.Instance.UpdateLockStatus(false);
                            }
                        }
                    }
                }
                catch (Exception uiEx)
                {
                    _logger?.LogWarning(uiEx, "更新重连成功UI时发生异常");
                }

                // 重连成功后，立即启动队列处理
                _ = Task.Run(ProcessCommandQueueAsync);

                // 重置心跳失败次数
                Interlocked.Exchange(ref _heartbeatFailedAttempts, 0);

                // 重置心跳间隔为基础值
                _heartbeatIntervalMs = _baseHeartbeatIntervalMs;

                // 重新启动心跳
                StartHeartbeat();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理重连成功事件时发生异常");
            }
        }

        /// <summary>
        /// 启动心跳检测（优化版）
        /// 使用更安全的启动机制，避免重复启动
        /// </summary>
        public void StartHeartbeat()
        {
            // 使用轻量级检查，避免不必要的锁操作
            if (Volatile.Read(ref _isHeartbeatRunning))
                return;

            lock (_heartbeatLock)
            {
                // 双重检查，确保线程安全
                if (_isHeartbeatRunning || (_heartbeatTask != null && !_heartbeatTask.IsCompleted))
                    return;

                _isHeartbeatRunning = true;
                Interlocked.Exchange(ref _heartbeatFailedAttempts, 0);
                _heartbeatCancellationTokenSource = new CancellationTokenSource();

                // 使用ConfigureAwait(false)避免UI线程上下文捕获
                _heartbeatTask = Task.Run(async () =>
                    await HeartbeatLoopAsync(_heartbeatCancellationTokenSource.Token).ConfigureAwait(false));

                _logger?.LogDebug("心跳检测已启动，间隔：{IntervalMs}ms", _heartbeatIntervalMs);
            }
        }

        /// <summary>
        /// 停止心跳检测（优化版）
        /// 使用更安全的停止机制，确保资源正确释放
        /// </summary>
        private void StopHeartbeat()
        {
            // 快速检查，避免不必要的锁操作
            if (!Volatile.Read(ref _isHeartbeatRunning))
                return;

            lock (_heartbeatLock)
            {
                // 双重检查，确保线程安全
                if (!_isHeartbeatRunning)
                    return;

                _isHeartbeatRunning = false;

                // 取消心跳任务
                _heartbeatCancellationTokenSource?.Cancel();

                // 安全地等待任务完成，避免阻塞
                SafeWaitForHeartbeatTaskCompletion();

                _logger?.LogDebug("心跳检测已停止");
            }
        }

        /// <summary>
        /// 安全地等待心跳任务完成
        /// 避免长时间阻塞，确保资源正确释放
        /// </summary>
        private void SafeWaitForHeartbeatTaskCompletion()
        {
            if (_heartbeatTask == null)
                return;

            try
            {
                // 使用异步方式等待，避免阻塞UI线程
                if (!_heartbeatTask.IsCompleted)
                {
                    // 使用短时间等待，避免长时间阻塞
                    bool completed = _heartbeatTask.Wait(TimeSpan.FromSeconds(2));

                    if (!completed)
                    {
                        _logger?.LogWarning("心跳任务未在指定时间内完成，强制清理资源");
                    }
                }
            }
            catch (AggregateException ae)
            {
                // 处理可能的异常
                foreach (var ex in ae.InnerExceptions)
                {
                    if (ex is not OperationCanceledException)
                    {
                        _logger?.LogWarning(ex, "等待心跳任务完成时发生异常");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "等待心跳任务完成时发生异常");
            }
            finally
            {
                // 确保资源正确释放
                SafeDisposeHeartbeatResources();
            }
        }

        /// <summary>
        /// 安全释放心跳资源
        /// </summary>
        private void SafeDisposeHeartbeatResources()
        {
            try
            {
                _heartbeatTask = null;

                if (_heartbeatCancellationTokenSource != null)
                {
                    _heartbeatCancellationTokenSource.Dispose();
                    _heartbeatCancellationTokenSource = null;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "释放心跳资源时发生异常");
            }
        }

        /// <summary>
        /// 心跳检测循环（优化版）
        /// 定期执行心跳检测，避免UI阻塞和资源竞争
        /// </summary>
        private async Task HeartbeatLoopAsync(CancellationToken cancellationToken)
        {
            _logger?.LogDebug("进入心跳循环（优化版），心跳间隔：{IntervalMs}ms", _heartbeatIntervalMs);

            while (!cancellationToken.IsCancellationRequested && _isHeartbeatRunning)
            {
                try
                {
                    _logger?.LogTrace("心跳循环迭代开始");

                    // 使用动态心跳间隔，使用ConfigureAwait(false)避免UI线程阻塞
                    await Task.Delay(_heartbeatIntervalMs, cancellationToken).ConfigureAwait(false);

                    // 检查连接状态 - 双重验证
                    bool socketConnected = _socketClient.IsConnected;
                    bool managerConnected = _connectionManager.IsConnected;
                    bool initialConnected = socketConnected && managerConnected;
                    _logger?.LogTrace("心跳前连接状态检查: Socket={SocketConnected}, Manager={ManagerConnected}",
                        socketConnected, managerConnected);

                    if (!initialConnected)
                    {
                        _logger?.LogDebug("连接已断开，跳过本次心跳发送");
                        // 如果连接已断开，重置心跳失败计数
                        Interlocked.Exchange(ref _heartbeatFailedAttempts, 0);
                        _heartbeatFailureTracker.Reset();
                        continue;
                    }

                    // 检查心跳失败次数，如果已达到阈值则暂停发送心跳
                    if (_heartbeatFailedAttempts >= HEARTBEAT_FAILURE_THRESHOLD)
                    {
                        _logger?.LogDebug("心跳失败次数已达到阈值({Threshold})，暂停发送心跳");
                        // 每隔一段时间检查一次连接状态，避免长时间无响应
                        await Task.Delay(5000, cancellationToken).ConfigureAwait(false);
                        continue;
                    }

                    // 发送心跳（异步执行，避免阻塞）
                    _logger?.LogTrace("开始发送心跳请求");
                    Exception lastException = null;
                    bool success = true;

                    try
                    {
                        success = await SendHeartbeatAsync(cancellationToken).ConfigureAwait(false);
                    }
                    catch (Exception ex) when (ex is OperationCanceledException)
                    {
                        _logger?.LogDebug("心跳发送被取消");
                        success = false;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        success = false;
                        _logger?.LogError(ex, "心跳发送过程中发生异常");
                    }

                    _logger?.LogTrace("心跳请求发送完成，结果: {Success}", success);

                    // 心跳失败时立即检查连接状态
                    if (!success)
                    {
                        // 主动检查连接状态，确保与实际网络状态一致 - 双重验证
                        bool failSocketConnected = _socketClient.IsConnected;
                        bool failManagerConnected = _connectionManager.IsConnected;
                        bool actualConnected = failSocketConnected && failManagerConnected;
                        _logger?.LogTrace("心跳失败，实际连接状态: Socket={SocketConnected}, Manager={ManagerConnected}",
                            failSocketConnected, failManagerConnected);

                        if (!actualConnected)
                        {
                            _logger?.LogWarning("心跳失败，检测到实际连接已断开，更新连接状态");
                            // 直接更新连接状态，触发重连
                            OnConnectionStateChanged(false);
                            // 重置心跳失败计数
                            Interlocked.Exchange(ref _heartbeatFailedAttempts, 0);
                            _heartbeatFailureTracker.Reset();
                        }
                        else
                        {
                            // 网络连接正常但心跳失败，可能是服务器繁忙或网络暂时波动
                            // 不立即触发重连，让心跳机制继续运行
                            _logger?.LogDebug("心跳失败但网络连接正常，等待下一次心跳");
                        }
                    }

                    // 使用轻量级同步机制处理状态更新，传递异常信息用于失败类型检测
                    UpdateHeartbeatState(success, lastException);

                    _logger?.LogTrace("心跳循环迭代结束");
                }
                catch (OperationCanceledException)
                {
                    _logger?.LogDebug("心跳循环被取消");
                    break;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "心跳循环中发生异常");
                    // 异常情况下也更新状态
                    UpdateHeartbeatState(false, ex);
                }
            }

            _logger?.LogDebug("退出心跳循环");
        }

        /// <summary>
        /// 更新心跳状态（增强版 - 集成失败类型检测和智能阈值判断）
        /// 避免频繁的锁操作，减少资源竞争
        /// </summary>
        /// <param name="success">心跳是否成功</param>
        /// <param name="failureException">失败时的异常信息</param>
        private void UpdateHeartbeatState(bool success, Exception failureException = null)
        {
            try
            {
                if (success)
                {
                    // 心跳成功 - 重置所有失败计数
                    int previousFailures = Interlocked.Exchange(ref _heartbeatFailedAttempts, 0);
                    _lastHeartbeatTime = DateTime.Now;
                    _heartbeatFailureTracker.Reset();

                    if (previousFailures > 0)
                    {
                        Task.Run(() => HeartbeatRecovered?.Invoke()).ConfigureAwait(false);
                        _logger?.LogInformation("✅ 心跳恢复，之前连续失败: {PreviousFailures}次", previousFailures);

                        try
                        {
                            _connectionManager.StartAutoReconnect();
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "恢复自动重连时发生异常");
                        }

                        // 心跳恢复后，适当减小心跳间隔
                        _heartbeatIntervalMs = Math.Max(_baseHeartbeatIntervalMs, _heartbeatIntervalMs / 2);
                    }
                    else
                    {
                        _logger?.LogTrace("心跳成功");
                    }
                }
                else
                {
                    // 心跳失败 - 检测失败类型
                    bool isConnected = _socketClient.IsConnected;
                    HeartbeatFailureType? failureType = null;

                    if (failureException != null)
                    {
                        failureType = DetectFailureType(failureException);
                        _logger?.LogWarning("❌ 心跳失败，类型: {FailureType}, 原因: {Message}",
                            failureType, failureException.Message);
                    }

                    int currentFailures;
                    if (isConnected)
                    {
                        // 只有当前失败次数小于阈值时才递增，避免计数无限增长
                        int currentValue = Volatile.Read(ref _heartbeatFailedAttempts);
                        if (currentValue < HEARTBEAT_FAILURE_THRESHOLD)
                        {
                            currentFailures = Interlocked.Increment(ref _heartbeatFailedAttempts);
                        }
                        else
                        {
                            currentFailures = currentValue; // 保持当前值，不再递增
                        }

                        Task.Run(() => HeartbeatFailed?.Invoke(currentFailures)).ConfigureAwait(false);

                        // 根据失败类型采取不同策略
                        if (failureType.HasValue)
                        {
                            switch (failureType.Value)
                            {
                                case HeartbeatFailureType.Timeout:
                                    _logger?.LogDebug("⏱️ 心跳超时，可能原因:网络延迟高/服务器响应慢");
                                    // 临时增加心跳间隔
                                    _heartbeatIntervalMs = (int)Math.Min(_heartbeatIntervalMs * 1.2, _maxHeartbeatIntervalMs);
                                    break;

                                case HeartbeatFailureType.NetworkError:
                                    _logger?.LogError("🌐 网络错误，将触发重连机制");
                                    _connectionManager.StartAutoReconnect();
                                    break;

                                case HeartbeatFailureType.ServerBusy:
                                    _logger?.LogDebug("⚠️ 服务器繁忙，延长心跳间隔");
                                    _heartbeatIntervalMs = (int)Math.Min(_heartbeatIntervalMs * 1.5, _maxHeartbeatIntervalMs);
                                    break;

                                case HeartbeatFailureType.SessionExpired:
                                    _logger?.LogError("🔒 会话过期，触发重新登录");
                                    SessionExpired?.Invoke();
                                    break;

                                default:
                                    _logger?.LogDebug("❓ 未知心跳失败类型");
                                    break;
                            }

                            // 记录失败到追踪器
                            _heartbeatFailureTracker.RecordFailure(failureType.Value);
                        }

                        _logger?.LogDebug("❌ 心跳失败，连续失败: {CurrentFailures}/{Threshold}",
                            currentFailures, HEARTBEAT_FAILURE_THRESHOLD);
                    }
                    else
                    {
                        Interlocked.Exchange(ref _heartbeatFailedAttempts, 0);
                        _heartbeatFailureTracker.Reset();
                        currentFailures = 0;

                        _logger?.LogWarning("❌ 心跳失败且连接已断开，重置失败计数并触发重连机制");

                        try
                        {
                            _connectionManager.StartAutoReconnect();
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "启动自动重连时发生异常");
                        }
                    }

                    // 智能阈值检测：使用追踪器判断是否应该锁定
                    if (isConnected && (_heartbeatFailureTracker.ShouldTriggerLockout() || currentFailures >= HEARTBEAT_FAILURE_THRESHOLD))
                    {
                        _logger?.LogError("🚨 心跳失败达到阈值({Threshold})，触发锁定机制", HEARTBEAT_FAILURE_THRESHOLD);

                        Task.Run(() => HeartbeatFailureThresholdReached?.Invoke()).ConfigureAwait(false);

                        _logger?.LogDebug("💡 锁定后保持自动重连机制运行，以便网络恢复后自动重连");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新心跳状态时发生异常");
            }
        }

        /// <summary>
        /// 判断心跳失败类型
        /// 根据异常信息判断失败原因，采取不同的处理策略
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns>心跳失败类型</returns>
        private HeartbeatFailureType DetectFailureType(Exception ex)
        {
            if (ex == null)
                return HeartbeatFailureType.Unknown;

            if (ex is TimeoutException)
                return HeartbeatFailureType.Timeout;

            if (ex is SocketException socketEx)
            {
                switch (socketEx.SocketErrorCode)
                {
                    case SocketError.TimedOut:
                        return HeartbeatFailureType.Timeout;
                    case SocketError.ConnectionRefused:
                    case SocketError.NetworkDown:
                    case SocketError.NetworkUnreachable:
                    case SocketError.HostUnreachable:
                        return HeartbeatFailureType.NetworkError;
                    default:
                        return HeartbeatFailureType.Unknown;
                }
            }

            // 检查异常消息中的关键字
            var errorMessage = ex.Message?.ToLower() ?? "";
            if (errorMessage.Contains("超时") || errorMessage.Contains("timeout"))
                return HeartbeatFailureType.Timeout;
            if (errorMessage.Contains("网络") || errorMessage.Contains("network"))
                return HeartbeatFailureType.NetworkError;
            if (errorMessage.Contains("繁忙") || errorMessage.Contains("busy") || errorMessage.Contains("忙碌"))
                return HeartbeatFailureType.ServerBusy;
            if (errorMessage.Contains("过期") || errorMessage.Contains("expired") ||
                errorMessage.Contains("未登录") || errorMessage.Contains("未登录状态"))
                return HeartbeatFailureType.SessionExpired;

            return HeartbeatFailureType.Unknown;
        }

        /// <summary>
        /// 发送单个心跳请求（增强版 - 包含详细日志和智能重试）
        /// </summary>
        private async Task<bool> SendHeartbeatAsync(CancellationToken cancellationToken)
        {
            var attemptNumber = Interlocked.Increment(ref _totalHeartbeatAttempts);
            var logContext = new
            {
                Timestamp = DateTime.Now,
                SessionId = MainForm.Instance?.AppContext?.SessionId ?? "N/A",
                UserId = MainForm.Instance?.AppContext?.CurUserInfo?.UserID ?? 0,
                ClientIP = _socketClient?.ClientIP ?? "N/A",
                AttemptNumber = attemptNumber,
                CurrentInterval = _heartbeatIntervalMs
            };

            try
            {
                // 检查是否有有效的Session ID
                if (string.IsNullOrEmpty(logContext.SessionId) || logContext.UserId == 0)
                {
                    _logger?.LogDebug("⚠️ [{Timestamp:HH:mm:ss.fff}] 未登录状态，跳过心跳 #{AttemptNumber}",
                        logContext.Timestamp, logContext.AttemptNumber);
                    return false;
                }

                // 检查心跳失败次数，如果已达到阈值则停止发送心跳
                if (_heartbeatFailedAttempts >= HEARTBEAT_FAILURE_THRESHOLD)
                {
                    _logger?.LogDebug("⛔ 心跳失败次数已达到阈值({Threshold})，暂停发送心跳请求",
                        HEARTBEAT_FAILURE_THRESHOLD);
                    return false;
                }

                _logger?.LogTrace("🔄 [{Timestamp:HH:mm:ss.fff}] 开始发送心跳 #{AttemptNumber}, " +
                    "会话: {SessionId}, 用户: {UserId}, IP: {ClientIP}, 当前间隔: {Interval}ms",
                    logContext.Timestamp, logContext.AttemptNumber, logContext.SessionId,
                    logContext.UserId, logContext.ClientIP, logContext.CurrentInterval);

                var heartbeatRequest = new HeartbeatRequest
                {
                    UserId = MainForm.Instance.AppContext.CurUserInfo.UserID,
                    ClientId = _socketClient.ClientID,
                    ClientTime = DateTime.Now,
                    ClientStatus = "Normal",
                    UserOperationInfo = new RUINORERP.Model.UserOperationInfo
                    {
                        用户名 = MainForm.Instance.AppContext.CurUserInfo.用户名,
                        姓名 = MainForm.Instance.AppContext.CurUserInfo.姓名,
                        当前模块 = MainForm.Instance.AppContext.CurUserInfo.当前模块,
                        当前窗体 = MainForm.Instance.AppContext.CurUserInfo.当前窗体,
                        登录时间 = MainForm.Instance.AppContext.CurUserInfo.登录时间,
                        心跳数 = MainForm.Instance.AppContext.CurUserInfo.心跳数,
                        客户端版本 = MainForm.Instance.AppContext.CurUserInfo.客户端版本,
                        客户端IP = MainForm.Instance.AppContext.CurUserInfo.客户端IP,
                        静止时间 = MainForm.Instance.AppContext.CurUserInfo.静止时间,
                        超级用户 = MainForm.Instance.AppContext.CurUserInfo.超级用户,
                        授权状态 = MainForm.Instance.AppContext.CurUserInfo.授权状态,
                        操作系统 = MainForm.Instance.AppContext.CurUserInfo.操作系统,
                        机器名 = MainForm.Instance.AppContext.CurUserInfo.机器名,
                        CPU信息 = MainForm.Instance.AppContext.CurUserInfo.CPU信息,
                        内存大小 = MainForm.Instance.AppContext.CurUserInfo.内存大小
                    }
                };

                // 增强心跳重试机制，最多重试2次，使用指数退避
                const int maxRetries = 2;
                Exception lastException = null;

                for (int retry = 0; retry <= maxRetries; retry++)
                {
                    try
                    {
                        // 根据重试次数动态调整超时时间
                        var timeout = _heartbeatTimeoutMs * (retry + 1);
                        _logger?.LogTrace("💓 心跳重试 {Retry}/{MaxRetries}, 超时: {Timeout}ms",
                            retry + 1, maxRetries + 1, timeout);

                        var sendStartTime = DateTime.Now;

                        var response = await SendCommandWithResponseAsync<HeartbeatResponse>(
                            SystemCommands.Heartbeat,
                            heartbeatRequest,
                            cancellationToken,
                            timeout); // 动态超时

                        var sendDuration = (DateTime.Now - sendStartTime).TotalMilliseconds;

                        if (response != null && response.IsSuccess)
                        {
                            Interlocked.Increment(ref _totalHeartbeatSuccess);

                            // 计算网络延迟
                            var roundTripTime = sendDuration;
                            var estimatedLatency = roundTripTime / 2;
                            RecordLatency(estimatedLatency);

                            // 动态调整心跳间隔
                            AdjustHeartbeatInterval();

                            // 检查服务器是否推荐了新的心跳间隔
                            if (response.ServerInfo != null &&
                                response.ServerInfo.ContainsKey("RecommendedInterval") &&
                                int.TryParse(response.ServerInfo["RecommendedInterval"]?.ToString(), out int recommendedInterval))
                            {
                                // 使用服务器推荐的间隔，并在客户端设置的范围内
                                _heartbeatIntervalMs = Clamp(recommendedInterval, _minHeartbeatIntervalMs, _maxHeartbeatIntervalMs);
                            }

                            var lastSuccessTime = _lastHeartbeatTime;
                            _lastHeartbeatTime = DateTime.Now;

                            MainForm.Instance.lblServerInfo.Text = $"服务器信息：{_socketClient.ServerIP}:{_socketClient.ServerPort}";
                            return true;
                        }
                        else
                        {
                            _logger?.LogDebug("❌ [{Now:HH:mm:ss.fff}] 心跳失败 #{AttemptNumber}(尝试{Retry}/{MaxRetries}), " +
                                "耗时: {Duration}ms, 服务器响应: {IsSuccess}, 错误: {ErrorMessage}",
                                DateTime.Now, logContext.AttemptNumber, retry + 1, maxRetries + 1,
                                sendDuration, response?.IsSuccess, response?.ErrorMessage);

                            if (retry < maxRetries)
                            {
                                // 指数退避：500ms, 1000ms, 1500ms
                                int waitTime = 500 * (retry + 1);
                                _logger?.LogTrace("⏳ 等待 {WaitTime}ms后重试...", waitTime);
                                await Task.Delay(waitTime, cancellationToken);
                            }
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        _logger?.LogTrace("⏸️ 心跳发送被取消");
                        throw;
                    }
                    catch (TimeoutException ex)
                    {
                        lastException = ex;
                        _logger?.LogDebug("⏱️ [{Now:HH:mm:ss.fff}] 心跳超时 #{AttemptNumber}(尝试{Retry}/{MaxRetries}), " +
                            "超时: {Timeout}ms, 当前间隔: {Interval}ms, " +
                            "最近成功: {LastSuccess:yyyy-MM-dd HH:mm:ss}",
                            DateTime.Now, logContext.AttemptNumber, retry + 1, maxRetries + 1,
                            _heartbeatTimeoutMs * (retry + 1), _heartbeatIntervalMs, _lastHeartbeatTime);

                        // 心跳超时时主动检查连接状态
                        if (!_socketClient.IsConnected)
                        {
                            _logger?.LogDebug("🌐 心跳超时，检测到连接已断开，立即触发重连");
                            _connectionManager.StartAutoReconnect();
                            return false;
                        }

                        if (retry < maxRetries)
                        {
                            // 指数退避
                            int waitTime = 500 * (retry + 1);
                            _logger?.LogTrace("⏳ 超时后等待 {WaitTime}ms重试...", waitTime);
                            await Task.Delay(waitTime, cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        _logger?.LogError(ex, "💥 [{Now:HH:mm:ss.fff}] 心跳发送异常 #{AttemptNumber}(尝试{Retry}/{MaxRetries})",
                            DateTime.Now, logContext.AttemptNumber, retry + 1, maxRetries + 1);

                        if (retry >= maxRetries)
                        {
                            // 非超时异常,不再重试
                            break;
                        }

                        // 指数退避
                        int waitTime = 500 * (retry + 1);
                        _logger?.LogTrace("⏳ 异常后等待 {WaitTime}ms重试...", waitTime);
                        await Task.Delay(waitTime, cancellationToken);
                    }
                }

                // 所有重试都失败
                Interlocked.Increment(ref _totalHeartbeatFailures);
                _logger?.LogError("❌ 心跳重试失败 #{AttemptNumber}, 已达到最大重试次数, " +
                    "总失败: {TotalFailures}/{TotalAttempts}({FailureRate:P1})",
                    logContext.AttemptNumber, _totalHeartbeatFailures, _totalHeartbeatAttempts,
                    (double)_totalHeartbeatFailures / _totalHeartbeatAttempts);
                return false;
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _totalHeartbeatFailures);
                _logger?.LogError(ex, "💥 发送心跳时发生未处理异常 #{AttemptNumber}", logContext.AttemptNumber);
                return false;
            }
        }

        #endregion

        #region 连接管理方法

        /// <summary>
        /// 连接到服务器 - 简化版，委托给ConnectionManager
        /// </summary>
        /// <param name="serverAddress">服务器地址</param>
        /// <param name="serverPort">服务器端口</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>连接是否成功</returns>
        public async Task<bool> ConnectAsync(string serverAddress, int serverPort, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("尝试连接到服务器 {ServerAddress}:{ServerPort}", serverAddress, serverPort);
            return await _connectionManager.ConnectAsync(serverAddress, serverPort, cancellationToken);
        }

        /// <summary>
        /// 断开连接 - 简化版，委托给ConnectionManager
        /// </summary>
        /// <returns>断开连接是否成功</returns>
        public async Task<bool> Disconnect()
        {
            // 添加主动断开连接警告日志
            _logger?.LogWarning("[主动断开连接] 开始断开服务器连接");

            // 停止心跳
            try
            {
                // 使用内部实现的StopHeartbeat方法
                StopHeartbeat();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "停止心跳时发生异常");
            }

            // 清除清理定时器
            _cleanupTimer?.Dispose();
            _cleanupTimer = null;

            // 断开Socket连接
            await _socketClient?.Disconnect();
            return true;

        }

        /// <summary>
        /// 取消所有重连操作并强制断开连接（用于服务器切换）
        /// </summary>
        /// <returns>操作是否成功</returns>
        public async Task<bool> CancelReconnectAndForceDisconnectAsync()
        {
            try
            {
                // 添加主动断开连接警告日志
                _logger?.LogWarning("[主动断开连接] 取消重连操作并强制断开连接（服务器切换）");

                // 停止心跳
                StopHeartbeat();

                // 取消重连操作
                _connectionManager.StopAutoReconnect();

                // 强制断开连接
                return await _connectionManager.DisconnectAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "取消重连并强制断开连接时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 连接状态变更事件处理
        /// </summary>
        private void OnConnectionStateChanged(bool connected)
        {
            try
            {
                _clientEventManager?.OnConnectionStatusChanged(connected);

                if (connected)
                {
                    _logger?.LogDebug("客户端已连接到服务器");

                    // 连接恢复时，开始处理队列中的命令
                    _ = Task.Run(ProcessCommandQueueAsync);
                }
                else
                {
                    _logger?.LogDebug("客户端与服务器断开连接");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理连接状态变更时发生异常");
            }
        }

        #endregion





        /// <summary>
        /// 命令接收事件，当从服务器接收到命令时触发
        /// </summary>
        public event Action<PacketModel, object> CommandReceived
        {
            add => _clientEventManager.CommandReceived += value;
            remove => _clientEventManager.CommandReceived -= value;
        }

        /// <summary>
        /// 订阅特定命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="handler">处理函数</param>
        public void SubscribeCommand(CommandId commandId, Action<PacketModel, object> handler)
        {
            _clientEventManager.SubscribeCommand(commandId, handler);
        }

        /// <summary>
        /// 取消订阅特定命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="handler">处理函数</param>
        public void UnsubscribeCommand(CommandId commandId, Action<PacketModel, object> handler)
        {
            _clientEventManager.UnsubscribeCommand(commandId, handler);
        }

        /// <summary>
        /// 重连失败事件，当客户端重连服务器失败时触发
        /// </summary>
        public event Action ReconnectFailed
        {
            add
            {
                _clientEventManager.ReconnectFailed += value;
                _fallbackReconnectFailed += value; // 同时订阅备用事件
            }
            remove
            {
                _clientEventManager.ReconnectFailed -= value;
                _fallbackReconnectFailed -= value; // 同时取消订阅备用事件
            }
        }

        /// <summary>
        /// 连接状态变化事件，当连接状态改变时触发
        /// </summary>
        public event Action<bool> ConnectionStateChanged
        {
            add => _clientEventManager.ConnectionStatusChanged += value;
            remove => _clientEventManager.ConnectionStatusChanged -= value;
        }





        /// <summary>
        /// 判断异常是否支持重试
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>是否支持重试</returns>
        private bool IsRetryableException(Exception ex)
        {
            // 网络异常、超时异常等通常支持重试
            return ex is TimeoutException ||
                   ex is System.Net.Sockets.SocketException ||
                   ex is System.IO.IOException ||
                    ex.Message.Contains("connection") ||
                     ex.Message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0; // 服务器错误支持重试
        }

        /// <summary>
        /// 识别错误类型
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>错误类型</returns>
        private NetworkErrorType IdentifyErrorType(Exception ex)
        {
            // 根据异常类型和消息识别错误类型
            if (ex is TimeoutException)
                return NetworkErrorType.TimeoutError;
            else if (ex is System.IO.IOException || ex is System.Net.Sockets.SocketException)
                return NetworkErrorType.ConnectionError;
            else if (ex.Message.IndexOf("unauthorized", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     ex.Message.IndexOf("permission", StringComparison.OrdinalIgnoreCase) >= 0)
                return NetworkErrorType.AuthorizationError;
            else if (ex.Message.IndexOf("authenticate", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     ex.Message.IndexOf("login", StringComparison.OrdinalIgnoreCase) >= 0)
                return NetworkErrorType.AuthenticationError;
            else if (ex.Message.IndexOf("serialize", StringComparison.OrdinalIgnoreCase) >= 0)
                return NetworkErrorType.SerializationError;
            else if (ex.Message.IndexOf("deserialize", StringComparison.OrdinalIgnoreCase) >= 0)
                return NetworkErrorType.DeserializationError;
            else if (ex.Message.IndexOf("command", StringComparison.OrdinalIgnoreCase) >= 0)
                return NetworkErrorType.CommandError;
            else if (ex.Message.IndexOf("server", StringComparison.OrdinalIgnoreCase) >= 0)
                return NetworkErrorType.ServerError;
            else
                return NetworkErrorType.UnknownError;
        }


        /// <summary>
        /// 发送请求并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">请求超时时间（毫秒），如果未指定则根据命令类型自动设置</param>
        /// <returns>响应数据对象</returns>
        private async Task<PacketModel> SendRequestAsync<TRequest, TResponse>(
           CommandId commandId,
            TRequest request,
            CancellationToken ct = default,
            int timeoutMs = 0)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            // 根据命令类型设置不同的超时时间
            if (timeoutMs <= 0)
            {
                timeoutMs = GetTimeoutByCommandType(commandId);
            }

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(timeoutMs);

            var tcs = new TaskCompletionSource<PacketModel>(TaskCreationOptions.RunContinuationsAsynchronously);
            var pendingRequest = new PendingRequest
            {
                Tcs = tcs,
                CreatedAt = DateTime.UtcNow,
                CommandId = commandId.ToString()
            };

            if (request == null)
            {
                throw new InvalidOperationException($"请求数据不能为空，指令名称: {commandId.Name}");
            }

            if (!_pendingRequests.TryAdd(request.RequestId, pendingRequest))
            {
                throw new InvalidOperationException($"无法添加请求到待处理列表，指令类型：{commandId.ToString()}，请求ID: {request.RequestId}");
            }

            try
            {
                string ResponseTypeName = typeof(TResponse).AssemblyQualifiedName;

                // 使用现有的SendPacketCoreAsync发送请求，并传递带有响应类型信息的上下文
                await SendPacketCoreAsync<TRequest>(_socketClient, commandId, request, timeoutMs, ct, ResponseTypeName);



                // 等待响应或超时
                var timeoutTask = Task.Delay(timeoutMs, cts.Token);
                var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    throw new TimeoutException($"请求超时（{timeoutMs}ms），指令类型：{commandId.ToString()}，请求ID: {request.RequestId}");
                }

                ct.ThrowIfCancellationRequested();

                var responsePacket = await tcs.Task;

                if (responsePacket != null)
                {
                    // 记录请求完成事件
                    _clientEventManager.OnRequestCompleted(request.RequestId, DateTime.UtcNow - pendingRequest.CreatedAt);
                }

                // 直接进行类型检查并返回响应包
                return responsePacket as PacketModel;
            }
            catch (Exception ex) when (!(ex is TimeoutException) && !(ex is OperationCanceledException))
            {
                throw new InvalidOperationException($"请求处理失败，指令类型：{commandId.ToString()}，请求ID: {request.RequestId}: {ex.Message}", ex);
            }
            finally
            {
                _pendingRequests.TryRemove(request.RequestId, out _);
            }
        }

        /// <summary>
        /// 根据命令类型获取超时时间
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>超时时间（毫秒）</returns>
        private int GetTimeoutByCommandType(CommandId commandId)
        {
            // 根据命令类型设置不同的超时时间
            // 缓存相关请求设置较短超时，避免UI阻塞
            if (commandId.Name.Contains("Cache"))
            {
                return 5000; // 缓存请求5秒超时
            }

            // 认证相关请求设置较短超时
            if (commandId.Name.Contains("Auth") || commandId.Name.Contains("Login"))
            {
                return 8000; // 认证请求8秒超时
            }

            // 查询相关请求
            if (commandId.Name.Contains("Query") || commandId.Name.Contains("Search"))
            {
                return 10000; // 查询请求10秒超时
            }

            // 数据保存相关请求
            if (commandId.Name.Contains("Save") || commandId.Name.Contains("Update") || commandId.Name.Contains("Delete"))
            {
                return 15000; // 保存/更新/删除请求15秒超时
            }

            // 报表相关请求
            if (commandId.Name.Contains("Report") || commandId.Name.Contains("Export"))
            {
                return 30000; // 报表/导出请求30秒超时
            }

            // 默认超时时间
            return 20000; // 默认20秒超时
        }



        #region 设置token

        /// <summary>
        /// 自动附加认证Token - 优化版
        /// 增强功能：确保Token的完整性、类型设置、ExecutionContext绑定和异常处理
        /// </summary>
        /// <summary>
        /// 自动将访问令牌附加到命令上下文中
        /// </summary>
        /// <param name="executionContext">命令执行上下文，不能为空</param>
        /// <exception cref="Exception">附加令牌过程中发生的任何异常都将被捕获并记录</exception>
        protected virtual async Task AutoAttachTokenAsync(CommandContext executionContext)
        {
            try
            {
                // 使用null条件运算符简化检查
                if (_tokenManager?.TokenStorage == null) return;

                // 获取令牌并验证有效性
                var tokenInfo = await _tokenManager.TokenStorage.GetTokenAsync();

                // 简化条件判断并设置访问令牌
                if (tokenInfo?.AccessToken != null)
                {
                    executionContext.Token = tokenInfo;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "自动附加Token失败");
            }
        }

        #endregion


        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        /// <param name="packet">接收到的数据包</param>
        private async void OnReceived(PacketModel packet)
        {
            try
            {
                if (packet == null)
                {
                    _logger.LogWarning("接收到空数据包");
                    return;
                }
                if (_applicationContext != null)
                {
                    _applicationContext.SessionId = packet.SessionId;
                    if (_applicationContext.CurUserInfo != null)
                    {
                        _applicationContext.CurUserInfo.SessionId = packet.SessionId;
                    }
                }




                // 验证数据包有效性
                if (!packet.IsValid())
                {
                    var now = DateTime.Now;
                    var timeDiff = Math.Abs((now - packet.CreatedTime).TotalMinutes);
                    var isValidPacketId = !string.IsNullOrEmpty(packet.PacketId);
                    
                    _logger.LogWarning($"接收到无效数据包！CommandId: {packet.CommandId}, PacketId: {packet.PacketId}, CreatedTime: {packet.CreatedTime:yyyy-MM-dd HH:mm:ss}, CurrentTime: {now:yyyy-MM-dd HH:mm:ss}");
                    _logger.LogWarning($"时间差异: {timeDiff:F2}分钟, PacketId有效: {isValidPacketId}");
                    
                    // 如果时间差异超过2分钟，可能是客户端或服务器时间不同步
                    if (timeDiff > 2)
                    {
                        var errorMsg = $"系统时间不同步！\n服务器时间: {now:yyyy-MM-dd HH:mm:ss}\n数据包创建时间: {packet.CreatedTime:yyyy-MM-dd HH:mm:ss}\n时间差异: {timeDiff:F1}分钟\n\n请检查您的电脑系统时间是否正确。";
                        _logger.Error(errorMsg);
                        
                        // 在UI线程显示错误提示
                        System.Windows.Forms.MessageBox.Show(errorMsg, "时间同步错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                    
                    return;
                }

                if (packet.CommandId != SystemCommands.Heartbeat)
                {

                }
                if (packet.CommandId == AuthenticationCommands.Login)
                {

                }

                /*
客户端发送请求（SendCommandWithResponseAsync）
    ↓
创建 PendingRequest 并添加到 _pendingRequests
    ↓
发送请求到服务器
    ↓
等待 TaskCompletionSource（通过 Task.WhenAny）
    ↓
[并发] 服务器返回响应
    ↓
[并发] HandleResponsePacket 匹配 RequestId
    ↓
[并发] 从 _pendingRequests 移除并设置 Tcs 结果
    ↓
SendCommandWithResponseAsync 恢复执行并返回响应

                 */
                // 1. 首先尝试作为响应处理（请求-响应模式） 响应
                if (IsResponsePacket(packet))
                {
                    if (HandleResponsePacket(packet))
                    {
                        _logger.LogDebug("数据包作为响应处理完成，请求ID: {RequestId}", packet.Request?.RequestId);
                        return;
                    }
                }

                // 2. 作为服务器主动推送的命令处理（推送模式）
                if (IsServerPushCommand(packet))
                {
                    await HandleServerPushCommandAsync(packet);
                    return;
                }

                // 3. 作为通用命令处理
                await HandleGeneralCommandAsync(packet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理接收到的数据时发生错误");
                _clientEventManager.OnErrorOccurred(new Exception($"处理接收到的数据时发生错误: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 判断是否为响应数据包
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>是否为响应数据包</returns>
        private bool IsResponsePacket(PacketModel packet)
        {
            // 响应包通常包含请求ID，并且是服务器对客户端请求的响应
            return !string.IsNullOrEmpty(packet?.ExecutionContext?.RequestId) &&
                   packet.Direction == PacketDirection.ServerResponse;
        }

        /// <summary>
        /// 判断是否为服务器主动推送的命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>是否为服务器主动推送的命令</returns>
        private bool IsServerPushCommand(PacketModel packet)
        {
            // 服务器主动推送的命令通常没有请求ID，或者方向为推送
            return packet.Direction == PacketDirection.ServerRequest ||
                   string.IsNullOrEmpty(packet?.ExecutionContext?.RequestId);
        }

        /// <summary>
        /// 处理响应数据包
        /// 
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>是否成功处理</returns>
        private bool HandleResponsePacket(PacketModel packet)
        {
            try
            {
                var requestId = packet?.ExecutionContext?.RequestId;
                if (string.IsNullOrEmpty(requestId))
                    return false;

                if (_pendingRequests.TryRemove(requestId, out var pendingRequest))
                {
                    //会到后面执行：
                    return pendingRequest.Tcs.TrySetResult(packet);
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理响应包时发生错误，请求ID: {RequestId}", packet?.Request?.RequestId);
                return false;
            }
        }

        /// <summary>
        /// 处理服务器主动推送的命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task HandleServerPushCommandAsync(PacketModel packet)
        {
            _logger.LogDebug("处理服务器主动推送命令: {CommandId}", packet.CommandId);

            try
            {
                // 优先使用事件机制处理命令
                _clientEventManager.OnServerPushCommandReceived(packet, packet.Response);

                // 只有在没有事件订阅者时才使用调度器处理
                if (!_clientEventManager.HasCommandSubscribers(packet))
                {
                    await ProcessCommandAsync(packet);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理服务器主动推送命令时发生错误: {CommandId}", packet.CommandId);
                _clientEventManager.OnErrorOccurred(new Exception($"处理推送命令 {packet.CommandId} 时发生错误: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 处理通用命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task HandleGeneralCommandAsync(PacketModel packet)
        {
            _logger.LogDebug("处理通用命令: {CommandId}", packet.CommandId);

            try
            {
                // 使用统一的命令处理流程
                await ProcessCommandAsync(packet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理通用命令 {CommandId} 时发生错误", packet.CommandId);
                _clientEventManager.OnErrorOccurred(new Exception($"处理通用命令 {packet.CommandId} 时发生错误: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 清理超时请求
        /// </summary>
        /// <param name="state">状态对象</param>
        private void CleanupTimeoutRequests(object state)
        {
            var now = DateTime.UtcNow;
            var removedCount = 0;
            var queuedRemovedCount = 0;

            // 清理超时的待处理响应
            var responseCut = now.AddMinutes(-5);
            foreach (var kv in _pendingRequests)
            {
                if (kv.Value.CreatedAt < responseCut && _pendingRequests.TryRemove(kv.Key, out var pr))
                {
                    pr.Tcs.TrySetException(new TimeoutException($"请求 {kv.Key} 超时"));
                    removedCount++;
                    _logger?.LogDebug("清理超时请求: {RequestId}, 超时时间: 5分钟", kv.Key);
                }
            }

            // 清理超时的队列命令（超过10分钟）
            var queueCut = now.AddMinutes(-10);
            var tempQueue = new List<ClientQueuedCommand>();

            // 从队列中取出所有命令进行清理
            while (_queuedCommands.TryDequeue(out var queuedCommand))
            {
                if (queuedCommand.CreatedAt < queueCut)
                {
                    // 超时命令，取消并记录
                    queuedCommand.CompletionSource?.TrySetException(new TimeoutException($"队列命令 {queuedCommand.CommandId} 超时"));
                    queuedCommand.ResponseCompletionSource?.TrySetException(new TimeoutException($"队列响应命令 {queuedCommand.CommandId} 超时"));
                    queuedRemovedCount++;
                    _logger?.LogDebug("清理超时队列命令: {CommandId}, 创建时间: {CreatedAt}, 超时时间: 10分钟",
                        queuedCommand.CommandId, queuedCommand.CreatedAt);
                }
                else
                {
                    // 未超时命令，重新加入队列
                    tempQueue.Add(queuedCommand);
                }
            }

            // 将未超时的命令重新加入队列
            foreach (var command in tempQueue)
            {
                _queuedCommands.Enqueue(command);
            }

            if (removedCount > 0)
            {
                _logger?.LogDebug("清理了 {RemovedCount} 个超时请求", removedCount);
            }

            if (queuedRemovedCount > 0)
            {
                _logger?.LogDebug("清理了 {RemovedCount} 个超时队列命令", queuedRemovedCount);
            }
        }















        /// <summary>
        /// 处理接收到的命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task ProcessCommandAsync(PacketModel packet)
        {
            try
            {
                _logger.LogDebug("开始处理命令: {CommandId}", packet.CommandId);

                // 首先尝试使用客户端专用命令调度器处理命令
                bool dispatchedByClientDispatcher = await _commandDispatcher.DispatchAsync(packet);

                if (dispatchedByClientDispatcher)
                {
                    // 如果命令已被客户端命令调度器处理，直接返回
                    _logger.LogDebug("命令 {CommandId} 已被客户端命令调度器处理", packet.CommandId);
                    return;
                }

                // 如果客户端命令调度器未处理，则回退到原有的命令处理逻辑
                // 根据命令类别进行特殊处理
                switch (packet.CommandId.Category)
                {
                    case CommandCategory.System:
                        await ProcessSystemCommandAsync(packet);
                        break;

                    case CommandCategory.Cache:
                        await ProcessCacheCommandAsync(packet);
                        break;

                    case CommandCategory.Authentication:
                        await ProcessAuthenticationCommandAsync(packet);
                        break;

                    case CommandCategory.Config:
                        // 配置命令处理 - 主要通过ConfigCommandHandler处理，此处保留作为备用
                        await ProcessConfigCommandAsync(packet);
                        break;

                    default:
                        // 使用命令调度器处理其他命令
                        _logger.LogDebug("使用主命令调度器处理命令: {CommandId}", packet.CommandId);
                        await _commandDispatcher.DispatchAsync(packet).ConfigureAwait(false);
                        break;
                }
            }
            catch (Exception ex)
            {
                _clientEventManager.OnErrorOccurred(new Exception($"处理命令 {packet.CommandId} 时发生错误: {ex.Message}", ex));
                _logger.LogError(ex, "处理命令 {CommandId} 时发生错误", packet.CommandId);
            }
        }

        /// <summary>
        /// 处理配置相关命令（作为ConfigCommandHandler的备用机制）
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task ProcessConfigCommandAsync(PacketModel packet)
        {
            try
            {
                _logger.LogDebug("使用备用机制处理配置命令: {CommandId}", packet.CommandId);

                // 检查是否是配置同步命令
                if (packet.CommandId.FullCode == RUINORERP.PacketSpec.Commands.GeneralCommands.ConfigSync.FullCode)
                {
                    // 提取配置类型和配置数据
                    if (packet.Request is IDictionary<string, object> requestData)
                    {
                        if (requestData.TryGetValue("ConfigType", out var configTypeObj) &&
                            requestData.TryGetValue("ConfigData", out var configDataObj))
                        {
                            string configType = configTypeObj.ToString();
                            string configData = JsonConvert.SerializeObject(configDataObj);

                            _logger.LogDebug("接收到配置同步命令: {ConfigType}", configType);

                            // 调用OptionsMonitorConfigManager处理配置同步
                        }
                        else
                        {
                            _logger.LogWarning("配置同步命令数据格式不正确，缺少必需字段");
                        }
                    }
                    else if (packet.Request != null)
                    {
                        // 如果请求不是字典类型，尝试直接解析JSON
                        string jsonData = JsonConvert.SerializeObject(packet.Request);
                        var requestObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);

                        if (requestObj != null &&
                            requestObj.TryGetValue("ConfigType", out var configTypeObj) &&
                            requestObj.TryGetValue("ConfigData", out var configDataObj))
                        {
                            string configType = configTypeObj.ToString();
                            string configData = JsonConvert.SerializeObject(configDataObj);

                            _logger.LogDebug("接收到配置同步命令: {ConfigType}", configType);

                            // 调用OptionsMonitorConfigManager处理配置同步
                        }
                    }
                }
                else
                {
                    // 其他配置命令尝试使用客户端命令调度器处理
                    await _commandDispatcher.DispatchAsync(packet);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理配置命令时发生错误，命令ID: {CommandId}", packet.CommandId);
                throw;
            }
        }

        /// <summary>
        /// 处理系统命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task ProcessSystemCommandAsync(PacketModel packet)
        {
            // 处理系统命令，如心跳响应等

            if (packet.CommandId == SystemCommands.Heartbeat)
            {

            }

            if (packet.CommandId == SystemCommands.SystemManagement)
            {
                if (packet.Request is SystemCommandRequest commandRequest)
                {
                    switch (commandRequest.CommandType)
                    {
                        case SystemManagementType.ExitERPSystem:
                            if (commandRequest.CommandType == SystemManagementType.ExitERPSystem)
                            {
                                // 在UI线程显示退出提示并执行退出
                                await Task.Run(() =>
                                {
                                    try
                                    {
                                        int delaySeconds = commandRequest.DelaySeconds;

                                        if (delaySeconds > 0)
                                        {
                                            // 显示倒计时提示
                                            string message = $"系统将在 {delaySeconds} 秒后退出，这是管理员要求的操作。";
                                            string title = "系统即将退出";
                                            _logger.LogInformation($"收到延时退出命令，将在{delaySeconds}秒后退出系统");

                                            // 创建一个新的线程来执行延时退出
                                            ThreadPool.QueueUserWorkItem((state) =>
                                            {
                                                try
                                                {
                                                    // 等待指定的延时时间
                                                    Thread.Sleep(delaySeconds * 1000);

                                                    // 延时后执行系统退出
                                                    _logger.LogInformation("延时结束，执行系统退出");
                                                    System.Windows.Forms.Application.Exit();
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logger.LogError(ex, "执行延时退出时发生异常");
                                                }
                                            });

                                            // 显示提示信息
                                            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            // 立即执行系统退出
                                            _logger.LogInformation("收到立即退出命令，执行系统退出");
                                            System.Windows.Forms.Application.Exit();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "处理退出系统命令时发生异常");
                                    }
                                });
                            }
                            break;
                        case SystemManagementType.PushVersionUpdate:
                            // 处理服务器推送的版本更新请求
                            // 在UI线程中调用MainForm的UpdateSys方法
                            if (MainForm.Instance != null)
                            {
                                try
                                {
                                    // 使用Invoke确保在UI线程执行
                                    if (MainForm.Instance.InvokeRequired)
                                    {
                                        MainForm.Instance.Invoke(async () =>
                                        {
                                            // 调用UpdateSys方法，显示消息框并强制更新
                                            await MainForm.Instance.UpdateSys(true, true);
                                        });
                                    }
                                    else
                                    {
                                        await MainForm.Instance.UpdateSys(true, true);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "执行版本更新操作时发生异常");
                                }
                            }
                            else
                            {
                                _logger.LogWarning("MainForm实例不存在，无法执行版本更新操作");
                            }
                            break;

                    }


                }
            }
            if (packet.CommandId == SystemCommands.ExceptionReport)
            {

            }
            if (packet.CommandId == SystemCommands.ComputerStatus)
            {

            }


        }

        /// <summary>
        /// 处理缓存命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task ProcessCacheCommandAsync(PacketModel packet)
        {
            // 缓存命令可以使用专门的缓存服务处理
            // 或者使用命令调度器处理
            await _commandDispatcher.DispatchAsync(packet).ConfigureAwait(false);
        }

        /// <summary>
        /// 处理认证命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task ProcessAuthenticationCommandAsync(PacketModel packet)
        {
            // 认证命令使用调度器处理
            await _commandDispatcher.DispatchAsync(packet).ConfigureAwait(false);
        }




        /// <summary>
        /// 发送数据包的核心私有方法
        /// 封装了构建数据包、序列化、加密和发送的公共逻辑
        /// </summary>
        /// <param name="client">Socket客户端</param>
        /// <param name="commandId">命令标识符</param>
        /// <param name="data">要发送的数据</param>
        /// <param name="requestId">请求ID</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="authToken">认证令牌（可选）</param>
        /// <exception cref="OperationCanceledException">当操作被取消时抛出</exception>
        /// <exception cref="NetworkCommunicationException">当网络通信失败时抛出</exception>
        private async Task SendPacketCoreAsync<TRequest>(
            ISocketClient client,
               CommandId commandId,
            TRequest request,
            int timeoutMs,
            CancellationToken ct,
            string ResponseTypeName = null)
            where TRequest : class, IRequest
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                // 构建数据包
                var packet = PacketBuilder.Create()
                    .WithDirection(PacketDirection.ClientRequest) // 明确设置请求方向
                    .WithTimeout(timeoutMs)
                    .WithRequest(request)
                    .Build();

                // 自动设置到ExecutionContext，确保服务器端也能获取
                if (packet.ExecutionContext == null)
                {
                    packet.ExecutionContext = new CommandContext();
                }

                // 确保必要的上下文属性被设置
                packet.ExecutionContext.RequestId = request.RequestId;
                packet.CommandId = commandId;
                packet.ExecutionContext.SessionId = MainForm.Instance.AppContext.SessionId;
                packet.ExecutionContext.UserId = MainForm.Instance.AppContext.CurUserInfo.UserID;
                if (ResponseTypeName == null)
                {
                    //默认给基类。因为服务器处理时只是会在最后响应时才看是否真的需要响应。因为处理中会响应错误信息。
                    packet.ExecutionContext.NeedResponse = false;
                    packet.ExecutionContext.ExpectedResponseTypeName = nameof(RUINORERP.PacketSpec.Models.Core.ResponseBase);
                }
                else
                {
                    //  CommandContext用于传递响应类型信息
                    packet.ExecutionContext.ExpectedResponseTypeName = ResponseTypeName;
                    packet.ExecutionContext.NeedResponse = true;
                }


                await AutoAttachTokenAsync(packet.ExecutionContext);
                //除登录登出命令，其他命令都需要附加令牌
                if (packet.CommandId != AuthenticationCommands.Login)
                {
                    if (packet.ExecutionContext.Token == null)
                    {
                        // 附加令牌
                        throw new Exception($"发送请求失败: 没有合法授权令牌,指令：{commandId.ToString()}");
                    }
                }
                if (packet.CommandId == SystemCommands.Heartbeat)
                {

                }
                if (packet.CommandId == AuthenticationCommands.Login)
                {

                }

                // 序列化和加密数据包
                var payload = JsonCompressionSerializationService.Serialize<PacketModel>(packet);
                var original = new OriginalData((byte)packet.CommandId.Category, new[] { packet.CommandId.OperationCode }, payload);
                var encrypted = UnifiedEncryptionProtocol.EncryptClientDataToServer(original);

                await client.SendAsync(encrypted, ct);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"发送数据包时发生错误: CommandId={commandId}, {ex.Message}",
                    commandId.FullCode, commandId.Name);

                // 如果是取消操作，则直接抛出
                if (ex is OperationCanceledException)
                {
                    throw;
                }

                // 包装异常以便上层处理（包括可能的Token过期处理）
                throw new NetworkCommunicationException(
                    $"发送请求失败: {ex.Message}",
                    ex,
                    commandId,
                    commandId.Name);
            }
        }




        /// <summary>
        /// 安全地异步发送单向命令（包含异常处理）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送成功返回true，失败返回false</returns>
        public async Task<bool> SendOneWayCommandAsync<TRequest>(CommandId commandId, TRequest request, CancellationToken ct = default)
              where TRequest : class, IRequest
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                // 检查连接状态
                if (!_connectionManager.IsConnected)
                {
                    _logger.LogWarning("尝试发送单向命令但连接已断开，命令ID: {CommandId}", commandId);

                    // 如果启用了自动重连，将请求加入队列

                    _logger.Debug($"连接已断开，将命令{commandId.Name}加入队列等待发送");

                    // 创建任务完成源
                    var tcs = new TaskCompletionSource<bool>();

                    // 将请求加入队列
                    _queuedCommands.Enqueue(new ClientQueuedCommand
                    {
                        CommandId = commandId,
                        Data = request,
                        CancellationToken = ct,
                        DataType = typeof(TRequest),
                        CompletionSource = tcs,
                        CreatedAt = DateTime.UtcNow,
                        IsResponseCommand = false,
                        TimeoutMs = 20000
                    });

                    // 启动队列处理（如果未启动）
                    _ = Task.Run(ProcessCommandQueueAsync);

                    // 返回任务结果，让调用者等待连接恢复后发送
                    return await tcs.Task;

                }

                if (_disposed)
                    throw new ObjectDisposedException(nameof(ClientCommunicationService));

                // 使用现有的SendPacketCoreAsync发送请求
                await SendPacketCoreAsync<TRequest>(_socketClient, commandId, request, 20000, ct);

                return true;
            }
            catch (OperationCanceledException)
            {
                _clientEventManager.OnErrorOccurred(new Exception("发送命令操作被取消"));
                return false;
            }
            catch (Exception ex)
            {
                _clientEventManager.OnErrorOccurred(new Exception($"单向命令发送失败: {ex.Message}", ex));
                return false;
            }
        }

        /// <summary>
        /// 发送响应包（回复服务器的ServerRequest）
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="response">响应数据</param>
        /// <param name="originalRequestId">原始请求的RequestId（用于匹配）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送是否成功</returns>
        public async Task<bool> SendResponseAsync<TResponse>(CommandId commandId, TResponse response, string originalRequestId, CancellationToken ct = default)
            where TResponse : class, IResponse
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                // 检查连接状态
                if (!_connectionManager.IsConnected)
                {
                    _logger.LogWarning("尝试发送响应但连接已断开，命令ID: {CommandId}", commandId);
                    return false;
                }

                // 构建响应数据包
                var packet = PacketBuilder.Create()
                    .WithDirection(PacketDirection.ClientResponse) // 设置为客户端响应方向
                    .WithCommand(commandId)
                    .Build();

                // 设置响应数据
                packet.Response = response;

                // 设置ExecutionContext
                if (packet.ExecutionContext == null)
                {
                    packet.ExecutionContext = new CommandContext();
                }

                // 关键：设置RequestId为原始请求的RequestId，以便服务器匹配响应
                packet.ExecutionContext.RequestId = originalRequestId;
                packet.ExecutionContext.SessionId = _applicationContext.SessionId;
                packet.ExecutionContext.UserId = _applicationContext.CurUserInfo?.UserID ?? 0;

                // 附加令牌（如果需要）
                await AutoAttachTokenAsync(packet.ExecutionContext);

                // 序列化和加密数据包（使用与SendPacketCoreAsync相同的方式）
                var payload = JsonCompressionSerializationService.Serialize<PacketModel>(packet);
                var original = new OriginalData((byte)packet.CommandId.Category, new[] { packet.CommandId.OperationCode }, payload);
                var encrypted = UnifiedEncryptionProtocol.EncryptClientDataToServer(original);

                await _socketClient.SendAsync(encrypted);

                _logger.LogDebug("响应包发送成功: CommandId={CommandId}, RequestId={RequestId}", commandId, originalRequestId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送响应包失败: CommandId={CommandId}, RequestId={RequestId}", commandId, originalRequestId);
                return false;
            }
        }


        /// <summary>
        /// 初始化命令处理相关功能
        /// </summary>
        private void SubscribeCommandEvents()
        {
            // 不再订阅CommandReceived事件，因为它没有被触发
            // 命令处理现在通过OnServerPushCommandReceived和SubscribeCommand机制完成
            _logger.Debug("客户端命令处理器已启动，开始监听服务器命令");
        }

        /// <summary>
        /// 当接收到服务器命令时触发
        /// 这个处理命令的过程，类型服务器处理。后面处理逻辑也是一样。只是在客户端而已。对于复杂的情况有用。
        /// </summary>
        /// <param name="packetModel">数据包模型</param>
        /// <param name="data">命令数据</param>
        private async void OnCommandReceived(PacketModel packetModel, object data)
        {
            try
            {
                _logger.Debug("接收到服务器命令: {CommandId}", packetModel.CommandId);

                // 使用命令调度器处理命令
                await _commandDispatcher.DispatchAsync(packetModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理服务器命令时发生错误: {CommandId}", packetModel.CommandId);
                _clientEventManager.OnErrorOccurred(new Exception($"处理命令 {packetModel.CommandId} 时发生错误: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源（优化版）
        /// 确保所有资源正确释放，避免资源泄漏和死锁
        /// </summary>
        /// <param name="disposing">是否手动调用</param>
        protected virtual void Dispose(bool disposing)
        {
            // 检查是否已经释放
            if (!_disposed)
            {
                // 立即设置disposed标志，防止新任务启动
                _disposed = true;
                _isProcessingQueue = false;
                _isReconnecting = false;

                if (disposing)
                {
                    // 安全释放所有托管资源
                    SafeDisposeManagedResources();
                }

                _logger?.Debug("ClientCommunicationService资源释放完成");
            }
        }

        /// <summary>
        /// 安全释放托管资源
        /// 按顺序释放，避免资源泄漏和死锁
        /// </summary>
        private void SafeDisposeManagedResources()
        {
            try
            {
                // 第一步：取消事件订阅（防止事件继续触发）
                SafeUnsubscribeEvents();

                // 第二步：停止心跳检测（避免后台任务继续运行）
                SafeStopHeartbeat();

                // 第三步：释放计时器（避免定时任务继续运行）
                SafeDisposeTimers();

                // 第四步：断开Socket连接（释放网络资源）
                SafeDisconnectSocket();

                // 第五步：清理队列和取消待处理任务（避免任务无限等待）
                SafeClearQueuesAndCancelTasks();

                // 第六步：释放资源锁（释放同步原语）
                SafeDisposeLocks();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "释放托管资源时发生异常");
            }
        }

        /// <summary>
        /// 安全取消事件订阅
        /// </summary>
        private void SafeUnsubscribeEvents()
        {
            try
            {
                if (_connectionManager != null)
                {
                    _connectionManager.ReconnectFailed -= OnReconnectFailed;
                    _connectionManager.ReconnectAttempt -= OnReconnectAttempt;
                    _connectionManager.ReconnectSucceeded -= OnReconnectSucceeded;
                    _connectionManager.ConnectionStateChanged -= OnConnectionStateChanged;
                    _connectionManager.ConnectionStateChanged -= OnConnectionStateChangedForHeartbeat;
                }

                if (_socketClient != null)
                {
                    _socketClient.Received -= OnReceived;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "取消事件订阅时发生异常");
            }
        }

        /// <summary>
        /// 安全停止心跳检测
        /// </summary>
        private void SafeStopHeartbeat()
        {
            try
            {
                StopHeartbeat();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "停止心跳时发生异常");
            }
        }

        /// <summary>
        /// 安全释放计时器
        /// </summary>
        private void SafeDisposeTimers()
        {
            try
            {
                _cleanupTimer?.Dispose();
                _cleanupTimer = null;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "释放清理定时器时发生异常");
            }
        }

        /// <summary>
        /// 安全断开Socket连接
        /// </summary>
        private void SafeDisconnectSocket()
        {
            try
            {
                if (_socketClient != null)
                {
                    // 异步断开连接，避免阻塞
                    var disconnectTask = Task.Run(async () => await _socketClient.Disconnect());

                    // 等待最多3秒
                    if (!disconnectTask.Wait(TimeSpan.FromSeconds(3)))
                    {
                        _logger?.LogWarning("断开Socket连接超时");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "断开Socket连接时发生异常");
            }
        }

        /// <summary>
        /// 安全清理队列和取消任务
        /// </summary>
        private void SafeClearQueuesAndCancelTasks()
        {
            try
            {
                // 清理命令队列
                _logger?.Debug($"清理命令队列，当前队列大小: {_queuedCommands.Count}");
                while (_queuedCommands.TryDequeue(out var command))
                {
                    SafeCancelTaskCompletionSource(command.CompletionSource, "命令队列");
                    SafeCancelTaskCompletionSource(command.ResponseCompletionSource, "响应队列");
                }

                // 清理待处理请求
                _logger?.Debug($"清理待处理请求，数量: {_pendingRequests.Count}");
                foreach (var pendingRequest in _pendingRequests.Values)
                {
                    SafeCancelTaskCompletionSource(pendingRequest.Tcs, "待处理请求");
                }
                _pendingRequests.Clear();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "清理队列和取消任务时发生异常");
            }
        }

        /// <summary>
        /// 安全取消TaskCompletionSource（泛型版本）
        /// </summary>
        /// <typeparam name="T">TaskCompletionSource的类型参数</typeparam>
        /// <param name="tcs">TaskCompletionSource对象</param>
        /// <param name="sourceName">源名称（用于日志）</param>
        private void SafeCancelTaskCompletionSource<T>(TaskCompletionSource<T> tcs, string sourceName)
        {
            try
            {
                tcs?.TrySetCanceled();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "取消{SourceName}的TaskCompletionSource时发生异常", sourceName);
            }
        }

        /// <summary>
        /// 安全释放资源锁
        /// </summary>
        private void SafeDisposeLocks()
        {
            try
            {
                _queueLock?.Dispose();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "释放队列锁时发生异常");
            }
        }




        /// <summary>
        /// 析构函数
        /// </summary>
        ~ClientCommunicationService()
        {
            Dispose(false);
        }


        /// <summary>
        /// 发送命令并处理响应，返回指令类型的响应数据
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒），如果未指定则根据命令类型自动设置</param>
        /// <returns>包含指令信息的响应数据</returns>
        public async Task<TResponse> SendCommandWithResponseAsync<TResponse>(
            CommandId commandId,
            IRequest request,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TResponse : class, IResponse
        {
            const int maxRetries = 2;
            int retryCount = 0;

            while (true)
            {
                try
                {
                    var packet = await SendRequestAsync<IRequest, TResponse>(commandId, request, ct, timeoutMs);

                    if (packet == null)
                    {
                        return ResponseFactory.CreateSpecificErrorResponse<TResponse>("未收到服务器响应");
                    }

                    var responseData = packet.Response;

                    // 检查响应数据是否为空
                    if (responseData == null)
                    {
                        _logger.LogWarning($"命令响应数据为空或处理失败。命令ID: {commandId}");
                        return ResponseFactory.CreateSpecificErrorResponse<TResponse>("服务器返回了空响应数据");
                    }
                    return responseData as TResponse;
                }
                catch (Exception ex)
                {
                    _clientEventManager.OnErrorOccurred(new Exception($"带响应命令发送失败: {ex.Message}", ex));
                    // 如果是操作取消异常，重新抛出
                    if (ex is OperationCanceledException)
                    {
                        throw;
                    }

                    // 检查是否是可重试异常并且还有重试次数
                    if (IsRetryableException(ex) && retryCount < maxRetries)
                    {
                        retryCount++;
                        _logger?.LogWarning(ex, "命令发送失败，将进行第 {RetryCount}/{MaxRetries} 次重试。命令ID: {CommandId}", retryCount, maxRetries, commandId);

                        // 指数退避等待
                        int backoffMs = (int)Math.Pow(2, retryCount) * 1000; // 1秒, 2秒, 4秒...
                        await Task.Delay(backoffMs, ct);

                        // 继续重试
                        continue;
                    }

                    // 返回错误响应
                    return ResponseFactory.CreateSpecificErrorResponse<TResponse>($"命令执行失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 将带响应命令加入队列并等待处理
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>响应数据</returns>
        private async Task<TResponse> EnqueueCommandWithResponseAsync<TResponse>(CommandId commandId, IRequest request, CancellationToken ct, int timeoutMs)
            where TResponse : class, IResponse
        {
            // 创建任务完成源
            var packetTcs = new TaskCompletionSource<PacketModel>();
            var responseTcs = new TaskCompletionSource<TResponse>();

            // 当packetTcs完成时，将结果转换为TResponse并设置到responseTcs
            _ = packetTcs.Task.ContinueWith(async task =>
            {
                if (task.IsCanceled)
                    responseTcs.TrySetCanceled();
                else if (task.IsFaulted)
                    responseTcs.TrySetException(task.Exception);
                else if (task.Result != null && task.Result.Response != null)
                    responseTcs.TrySetResult(task.Result.Response as TResponse);
                else
                    responseTcs.TrySetResult(ResponseFactory.CreateSpecificErrorResponse<TResponse>("未收到有效响应数据") as TResponse);

                await Task.CompletedTask; // 避免异步转同步问题
            });

            // 将请求加入队列
            _queuedCommands.Enqueue(new ClientQueuedCommand
            {
                CommandId = commandId,
                Data = request,
                CancellationToken = ct,
                DataType = request.GetType(),
                ResponseCompletionSource = packetTcs,
                IsResponseCommand = true,
                TimeoutMs = timeoutMs,
                CreatedAt = DateTime.UtcNow,
                CompletionSource = null // 响应命令不需要简单的bool完成源
            });



            // 返回任务结果，让调用者等待连接恢复后发送
            return await responseTcs.Task;
        }

        /// <summary>
        /// 处理命令队列，在连接恢复时发送排队的命令
        /// </summary>
        /// <summary>
        /// 尝试在需要时进行重连，避免重复触发重连操作
        /// </summary>
        /// <returns>重连任务</returns>
        private async Task TryReconnectIfNeededAsync()
        {
            lock (_reconnectCoordinationLock)
            {
                // 检查是否需要重连并避免重复触发
                if (_connectionManager.IsConnected || _isReconnecting || _isDisposed)
                {
                    return;
                }

                // 防止频繁手动重连，至少间隔5秒
                var timeSinceLastAttempt = DateTime.Now - _lastManualReconnectAttempt;
                if (timeSinceLastAttempt.TotalSeconds < 5)
                {
                    _logger?.LogDebug("距离上次重连尝试时间过短，跳过此次重连");
                    return;
                }

                _isReconnecting = true;
                _lastManualReconnectAttempt = DateTime.Now;
            }

            try
            {
                _logger?.LogDebug("命令处理时检测到连接断开，尝试重连");

                // 获取当前服务器地址和端口
                string serverAddress = GetCurrentServerAddress();
                int serverPort = GetCurrentServerPort();

                if (!string.IsNullOrEmpty(serverAddress) && serverPort > 0)
                {
                    // 使用ConnectionManager的连接方法，它会处理自动重连逻辑
                    bool connected = await _connectionManager.ConnectAsync(serverAddress, serverPort, CancellationToken.None);

                    if (!connected)
                    {
                        _logger?.LogWarning("连接失败，将由ConnectionManager的自动重连机制处理");
                    }
                }
                else
                {
                    _logger?.LogWarning("服务器地址或端口无效，无法重连");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "尝试重连时发生异常");
            }
            finally
            {
                lock (_reconnectCoordinationLock)
                {
                    _isReconnecting = false;
                }
            }
        }

        /// <summary>
        /// 手动触发重连
        /// </summary>
        /// <returns>重连是否成功</returns>
        public async Task<bool> ManualReconnectAsync()
        {
            lock (_reconnectCoordinationLock)
            {
                if (_isDisposed)
                {
                    _logger?.LogWarning("服务已释放，无法进行手动重连");
                    return false;
                }

                // 防止频繁手动重连
                var timeSinceLastAttempt = DateTime.Now - _lastManualReconnectAttempt;
                if (timeSinceLastAttempt.TotalSeconds < 3)
                {
                    _logger?.LogDebug("手动重连过于频繁，请稍后再试");
                    return false;
                }

                _isReconnecting = true;
                _lastManualReconnectAttempt = DateTime.Now;
            }

            try
            {
                _logger?.LogDebug("用户手动触发重连");

                // 使用ConnectionManager的手动重连方法
                bool result = await _connectionManager.ManualReconnectAsync();

                if (result)
                {
                    _logger?.LogDebug("手动重连成功");

                    // 重连成功后，立即启动队列处理
                    _ = Task.Run(ProcessCommandQueueAsync);
                }
                else
                {
                    _logger?.LogWarning("手动重连失败");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "手动重连时发生异常");
                return false;
            }
            finally
            {
                lock (_reconnectCoordinationLock)
                {
                    _isReconnecting = false;
                }
            }
        }

        private async Task ProcessCommandQueueAsync()
        {
            // 使用信号量确保同时只有一个队列处理任务
            if (!await _queueLock.WaitAsync(TimeSpan.Zero))
                return;

            try
            {
                if (_isProcessingQueue)
                    return;

                _isProcessingQueue = true;
                _logger?.LogDebug("开始处理命令队列，当前队列大小：{QueueSize}", _queuedCommands.Count);

                var processedCount = 0;
                var failedCount = 0;

                // 使用临时列表来避免长时间锁定队列
                var commandsToProcess = new List<ClientQueuedCommand>();

                // 批量取出队列中的命令
                while (_queuedCommands.TryDequeue(out var command))
                {
                    commandsToProcess.Add(command);

                    // 限制单次处理数量，避免长时间阻塞
                    if (commandsToProcess.Count >= 50)
                        break;
                }

                // 按照优先级处理命令（响应命令优先）
                var responseCommands = commandsToProcess.Where(c => c.IsResponseCommand).ToList();
                var oneWayCommands = commandsToProcess.Where(c => !c.IsResponseCommand).ToList();

                // 处理响应命令
                foreach (var command in responseCommands)
                {
                    try
                    {
                        if (command.CancellationToken.IsCancellationRequested)
                        {
                            command.ResponseCompletionSource?.TrySetCanceled();
                            continue;
                        }

                        // 检查连接状态（原子操作）
                        bool isConnectedNow = _connectionManager.IsConnected;
                        if (!isConnectedNow)
                        {
                            // 连接仍然断开，重新加入队列
                            _queuedCommands.Enqueue(command);
                            // 触发重连尝试（使用更智能的重连策略）
                            if (!_isReconnecting)
                            {
                                _ = TryReconnectIfNeededAsync();
                            }
                            continue;
                        }

                        // 发送命令
                        var response = await SendRequestAsync<IRequest, IResponse>(
                            command.CommandId,
                            (IRequest)command.Data,
                            command.CancellationToken,
                            command.TimeoutMs);

                        if (response != null)
                        {
                            command.ResponseCompletionSource?.TrySetResult(response);
                            processedCount++;
                        }
                        else
                        {
                            command.ResponseCompletionSource?.TrySetException(new Exception("发送命令失败：未收到响应"));
                            failedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        command.ResponseCompletionSource?.TrySetException(ex);
                        failedCount++;
                        _logger?.LogError(ex, "处理响应命令时发生异常：{CommandId}", command.CommandId);
                    }
                }

                // 处理单向命令
                foreach (var command in oneWayCommands)
                {
                    try
                    {
                        if (command.CancellationToken.IsCancellationRequested)
                        {
                            command.CompletionSource?.TrySetCanceled();
                            continue;
                        }

                        // 检查连接状态（原子操作）
                        bool isConnectedNow = _connectionManager.IsConnected;
                        if (!isConnectedNow)
                        {
                            // 连接仍然断开，重新加入队列
                            _queuedCommands.Enqueue(command);
                            // 触发重连尝试（使用更智能的重连策略）
                            if (!_isReconnecting)
                            {
                                _ = TryReconnectIfNeededAsync();
                            }
                            continue;
                        }

                        // 发送命令
                        await SendPacketCoreAsync<IRequest>(_socketClient, command.CommandId, (IRequest)command.Data, command.TimeoutMs, command.CancellationToken);
                        command.CompletionSource?.TrySetResult(true);
                        processedCount++;
                    }
                    catch (Exception ex)
                    {
                        command.CompletionSource?.TrySetException(ex);
                        failedCount++;
                        _logger?.LogError(ex, "处理单向命令时发生异常：{CommandId}", command.CommandId);
                    }
                }

                _logger?.LogDebug("命令队列处理完成，成功：{ProcessedCount}，失败：{FailedCount}，剩余队列大小：{QueueSize}",
                    processedCount, failedCount, _queuedCommands.Count);

                // 如果队列中还有命令，设置标志以便下次处理
                if (!_queuedCommands.IsEmpty && !_isDisposed && !_disposed)
                {
                    // 避免递归调用，改为使用Task.Run启动新的处理任务
                    // 不使用递归调用避免可能的栈溢出和过多任务创建
                    _ = Task.Run(async () =>
                    {
                        // 小延迟避免CPU占用过高
                        await Task.Delay(100);
                        await ProcessCommandQueueAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理命令队列时发生异常");
            }
            finally
            {
                _isProcessingQueue = false;
                _queueLock.Release();
            }
        }

        /// <summary>
        /// 获取队列统计信息
        /// </summary>
        /// <returns>队列统计信息</returns>
        public (int TotalQueued, int ResponseCommands, int OneWayCommands, int PendingResponses, bool IsProcessing) GetQueueStatistics()
        {
            var responseCommands = 0;
            var oneWayCommands = 0;

            // 遍历队列获取统计信息（不影响性能的方式）
            var tempCommands = new List<ClientQueuedCommand>();
            while (_queuedCommands.TryDequeue(out var command))
            {
                if (command.IsResponseCommand)
                    responseCommands++;
                else
                    oneWayCommands++;

                tempCommands.Add(command);
            }

            // 重新加入队列
            foreach (var command in tempCommands)
            {
                _queuedCommands.Enqueue(command);
            }

            return (
                TotalQueued: _queuedCommands.Count,
                ResponseCommands: responseCommands,
                OneWayCommands: oneWayCommands,
                PendingResponses: _pendingRequests.Count,
                IsProcessing: _isProcessingQueue
            );
        }

        /// <summary>
        /// 清空队列（慎用）
        /// </summary>
        /// <param name="reason">清空原因</param>
        public void ClearQueue(string reason = "手动清空")
        {
            var clearedCount = 0;
            while (_queuedCommands.TryDequeue(out var command))
            {
                command.CompletionSource?.TrySetCanceled();
                command.ResponseCompletionSource?.TrySetCanceled();
                clearedCount++;
            }

            _logger?.LogDebug("已清空命令队列，清空原因：{Reason}，清空数量：{Count}", reason, clearedCount);
        }




        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }

}