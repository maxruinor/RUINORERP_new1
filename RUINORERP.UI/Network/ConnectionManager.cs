using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using RUINORERP.UI.Network;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 简化的客户端连接管理器
    /// 负责统一管理连接状态和重连逻辑
    /// </summary>
    public class ConnectionManager : IDisposable
    {
        private readonly ISocketClient _socketClient;
        private readonly ILogger<ConnectionManager> _logger;
        private readonly ConnectionManagerConfig _config;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private bool _isConnected = false;
        private bool _isReconnecting = false;
        private bool _disposed = false;
        private Task _reconnectTask = null;
        private bool _reconnectStopped = false; // 新增：重连是否已停止标志

        private string _serverAddress = string.Empty;
        private int _serverPort = 0;
        private DateTime _lastReconnectAttempt = DateTime.MinValue; // 新增：最后一次重连尝试时间
        private readonly object _reconnectStateLock = new object(); // 新增：重连状态同步锁

        /// <summary>
        /// 连接状态变更事件
        /// </summary>
        public event Action<bool> ConnectionStateChanged;

        /// <summary>
        /// 重连失败事件
        /// 当达到最大重连次数时触发
        /// </summary>
        public event Action ReconnectFailed;

        /// <summary>
        /// 重连尝试事件
        /// 每次重连尝试时触发，参数为当前尝试次数和最大尝试次数
        /// </summary>
        public event Action<int, int> ReconnectAttempt;

        /// <summary>
        /// 重连成功事件
        /// 重连成功时触发
        /// </summary>
        public event Action ReconnectSucceeded;

        /// <summary>
        /// 当前连接状态
        /// </summary>
        public bool IsConnected => _isConnected && _socketClient.IsConnected;

        /// <summary>
        /// 当前服务器地址
        /// </summary>
        public string CurrentServerAddress => _serverAddress;

        /// <summary>
        /// 当前服务器端口
        /// </summary>
        public int CurrentServerPort => _serverPort;

        /// <summary>
        /// 是否正在重连
        /// </summary>
        public bool IsReconnecting => _isReconnecting;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socketClient">Socket客户端</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="config">配置参数</param>
        public ConnectionManager(
            ISocketClient socketClient,
            ILogger<ConnectionManager> logger = null,
            ConnectionManagerConfig config = null)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<ConnectionManager>.Instance;
            _config = config ?? ConnectionManagerConfig.Default;

            // 订阅Socket客户端事件
            _socketClient.Closed += OnSocketClosed;
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="serverAddress">服务器地址</param>
        /// <param name="serverPort">服务器端口</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>连接是否成功</returns>
        public async Task<bool> ConnectAsync(string serverAddress, int serverPort, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(serverAddress))
                throw new ArgumentException("服务器地址不能为空", nameof(serverAddress));

            if (serverPort <= 0 || serverPort > 65535)
                throw new ArgumentOutOfRangeException(nameof(serverPort), "端口号必须在1-65535范围内");

            await _connectionLock.WaitAsync(cancellationToken);
            try
            {
                // 如果已经连接到同一服务器，直接返回成功
                if (IsConnected && _serverAddress == serverAddress && _serverPort == serverPort)
                {
                    _logger?.LogDebug("已经连接到目标服务器 {ServerAddress}:{ServerPort}", serverAddress, serverPort);
                    return true;
                }

                // 如果已连接到其他服务器，先断开
                if (IsConnected)
                {
                    //断开当前连接，准备连接到新服务器;
                    await DisconnectInternalAsync();
                }

                // 保存服务器信息
                _serverAddress = serverAddress;
                _serverPort = serverPort;

                // 尝试连接

                bool connected = await _socketClient.ConnectAsync(serverAddress, serverPort, cancellationToken);

                if (connected)
                {
                    _isConnected = true;
                    OnConnectionStateChanged(true);
                    _logger?.LogDebug("成功连接到服务器 {ServerAddress}:{ServerPort}", serverAddress, serverPort);

                    // 停止可能存在的重连任务
                    StopReconnectTask();
                }
                else
                {
                    _logger?.LogWarning("连接服务器失败 {ServerAddress}:{ServerPort}", serverAddress, serverPort);
                }

                return connected;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns>断开连接是否成功</returns>
        public async Task<bool> DisconnectAsync()
        {
            await _connectionLock.WaitAsync();
            try
            {
                StopReconnectTask();
                return await DisconnectInternalAsync();
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        /// <summary>
        /// 启动自动重连
        /// </summary>
        public void StartAutoReconnect()
        {
            lock (_reconnectStateLock)
            {
                if (_isReconnecting || _disposed)
                    return;

                _isReconnecting = true;
                _reconnectStopped = false;
                _reconnectTask = Task.Run(ReconnectLoopAsync, _cancellationTokenSource.Token);
                _logger?.LogDebug("启动自动重连任务");
            }
        }

        /// <summary>
        /// 停止自动重连
        /// </summary>
        public void StopAutoReconnect()
        {
            lock (_reconnectStateLock)
            {
                _isReconnecting = false;
                _reconnectStopped = true;
                StopReconnectTask();
                _logger?.LogDebug("停止自动重连任务");
            }
        }

        /// <summary>
        /// 内部断开连接方法
        /// </summary>
        /// <returns>断开连接是否成功</returns>
        private async Task<bool> DisconnectInternalAsync()
        {
            if (!_isConnected)
                return true;

            try
            {
                bool result = await _socketClient.Disconnect();
                _isConnected = false;
                OnConnectionStateChanged(false);
                _logger?.LogDebug("已断开与服务器的连接");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "断开连接时发生异常");
                _isConnected = false;
                OnConnectionStateChanged(false);
                return false;
            }
        }

        /// <summary>
        /// 停止重连任务
        /// </summary>
        private void StopReconnectTask()
        {
            if (_reconnectTask != null)
            {
                try
                {
                    _cancellationTokenSource.Cancel();
                    if (!_reconnectTask.IsCompleted)
                    {
                        _reconnectTask.Wait(TimeSpan.FromSeconds(2));
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "停止重连任务时发生异常");
                }
                finally
                {
                    _reconnectTask = null;
                    // 重新创建取消令牌源
                    if (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        _cancellationTokenSource.Dispose();
                    }
                    // 注意：这里不重新创建，因为我们可能在重连循环中
                }
            }
        }

        /// <summary>
        /// 重连循环
        /// </summary>
        private async Task ReconnectLoopAsync()
        {
            _logger?.LogDebug("进入重连循环");
            int reconnectAttempts = 0;
            int currentBackoffInterval = _config.ReconnectInterval;

            while (!_disposed && !_reconnectStopped && _isReconnecting && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                // 如果已经连接，重置重连计数并等待一段时间再检查
                if (IsConnected)
                {
                    if (reconnectAttempts > 0)
                    {
                        _logger?.LogDebug("重连成功，重置重连计数器");
                        OnReconnectSucceeded();
                    }
                    reconnectAttempts = 0;
                    currentBackoffInterval = _config.ReconnectInterval; // 重置退避间隔
                    await Task.Delay(_config.ReconnectCheckInterval, _cancellationTokenSource.Token);
                    continue;
                }

                // 如果没有服务器信息，等待重连
                if (string.IsNullOrEmpty(_serverAddress) || _serverPort == 0)
                {
                    _logger?.LogDebug("服务器信息不完整，等待重连检查间隔");
                    await Task.Delay(_config.ReconnectCheckInterval, _cancellationTokenSource.Token);
                    continue;
                }

                // 检查是否达到最大重连次数
                if (_config.MaxReconnectAttempts > 0 && reconnectAttempts >= _config.MaxReconnectAttempts)
                {
                    _logger?.LogWarning("已达到最大重连次数 {MaxAttempts}，停止重连", _config.MaxReconnectAttempts);
                    lock (_reconnectStateLock)
                    {
                        _isReconnecting = false;
                        _reconnectStopped = true;
                    }
                    // 触发重连失败事件
                    OnReconnectFailed();
                    break;
                }

                reconnectAttempts++;
                _lastReconnectAttempt = DateTime.Now;
                
                // 触发重连尝试事件
                OnReconnectAttempt(reconnectAttempts, _config.MaxReconnectAttempts);
                
                _logger?.LogDebug("尝试重新连接到服务器 {ServerAddress}:{ServerPort}，第 {Attempt} 次尝试", _serverAddress, _serverPort, reconnectAttempts);

                try
                {
                    // 检查网络状态（简单检测）
                    if (!await CheckNetworkAvailabilityAsync())
                    {
                        _logger?.LogWarning("网络不可用，跳过此次重连尝试");
                        await Task.Delay(_config.ReconnectCheckInterval, _cancellationTokenSource.Token);
                        continue;
                    }

                    bool connected = await _socketClient.ConnectAsync(_serverAddress, _serverPort, _cancellationTokenSource.Token);

                    if (connected)
                    {
                        _isConnected = true;
                        OnConnectionStateChanged(true);
                        _logger?.LogDebug("重连成功 {ServerAddress}:{ServerPort}，共尝试 {Attempt} 次", _serverAddress, _serverPort, reconnectAttempts);
                        OnReconnectSucceeded();

                        // 重连成功后重置计数器
                        reconnectAttempts = 0;
                        currentBackoffInterval = _config.ReconnectInterval;
                    }
                    else
                    {
                        _logger?.LogWarning("重连失败 {ServerAddress}:{ServerPort}，第 {Attempt} 次尝试", _serverAddress, _serverPort, reconnectAttempts);
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger?.LogDebug("重连操作被取消");
                    break;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "重连过程中发生异常 {ServerAddress}:{ServerPort}，第 {Attempt} 次尝试", _serverAddress, _serverPort, reconnectAttempts);
                }

                // 等待重连间隔，应用指数退避算法
                if (!IsConnected && !_reconnectStopped)
                {
                    if (_config.EnableExponentialBackoff && reconnectAttempts > 0)
                    {
                        // 计算指数退避时间，添加随机抖动避免雷群效应
                        var baseInterval = (int)Math.Min(
                            currentBackoffInterval * _config.BackoffMultiplier,
                            _config.MaxBackoffInterval);
                        
                        // 添加±20%的随机抖动
                        var randomJitter = new Random().Next(-baseInterval / 5, baseInterval / 5);
                        currentBackoffInterval = Math.Max(_config.ReconnectInterval, baseInterval + randomJitter);
                        
                        _logger?.LogDebug("应用指数退避，下次重连间隔 {Interval} 毫秒（含抖动）", currentBackoffInterval);
                    }

                    try
                    {
                        await Task.Delay(currentBackoffInterval, _cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger?.LogDebug("重连等待被取消");
                        break;
                    }
                }
            }

            _logger?.LogDebug("退出重连循环");
        }

        /// <summary>
        /// Socket连接关闭事件处理
        /// </summary>
        private void OnSocketClosed(EventArgs e)
        {
            lock (_reconnectStateLock)
            {
                if (_isConnected)
                {
                    _isConnected = false;
                    OnConnectionStateChanged(false);
                    _logger?.LogDebug("检测到连接已断开");

                    // 如果启用了自动重连且重连未在进行中，启动重连任务
                    if (_config.AutoReconnect && !_isReconnecting && !_disposed)
                    {
                        _logger?.LogDebug("启动自动重连机制");
                        StartAutoReconnect();
                    }
                    else if (_isReconnecting)
                    {
                        _logger?.LogDebug("重连已在进行中，不重复启动");
                    }
                }
            }
        }

        /// <summary>
        /// 触发连接状态变更事件
        /// </summary>
        /// <param name="connected">连接状态</param>
        private void OnConnectionStateChanged(bool connected)
        {
            try
            {
                ConnectionStateChanged?.Invoke(connected);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "触发连接状态变更事件时发生异常");
            }
        }

        /// <summary>
        /// 触发重连失败事件
        /// </summary>
        private void OnReconnectFailed()
        {
            try
            {
                ReconnectFailed?.Invoke();
                _logger?.LogDebug("触发重连失败事件");
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "触发重连失败事件时发生异常");
            }
        }

        /// <summary>
        /// 触发重连尝试事件
        /// </summary>
        /// <param name="currentAttempt">当前尝试次数</param>
        /// <param name="maxAttempts">最大尝试次数</param>
        private void OnReconnectAttempt(int currentAttempt, int maxAttempts)
        {
            try
            {
                ReconnectAttempt?.Invoke(currentAttempt, maxAttempts);
                _logger?.LogDebug("触发重连尝试事件: {CurrentAttempt}/{MaxAttempts}", currentAttempt, maxAttempts);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "触发重连尝试事件时发生异常");
            }
        }

        /// <summary>
        /// 触发重连成功事件
        /// </summary>
        private void OnReconnectSucceeded()
        {
            try
            {
                ReconnectSucceeded?.Invoke();
                _logger?.LogDebug("触发重连成功事件");
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "触发重连成功事件时发生异常");
            }
        }

        /// <summary>
        /// 检查网络可用性
        /// </summary>
        /// <returns>网络是否可用</returns>
        private async Task<bool> CheckNetworkAvailabilityAsync()
        {
            try
            {
                // 简单的网络可用性检查
                // 可以根据需要扩展为更详细的网络检测
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = await ping.SendPingAsync("8.8.8.8", 3000); // 谷歌DNS，3秒超时
                    bool isNetworkAvailable = reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                    
                    if (!isNetworkAvailable)
                    {
                        _logger?.LogDebug("网络不可用：Ping失败，状态：{Status}", reply.Status);
                    }
                    
                    return isNetworkAvailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "网络可用性检查失败，假设网络可用");
                // 如果网络检查失败，假设网络可用，避免阻塞重连
                return true;
            }
        }

        /// <summary>
        /// 获取重连状态信息
        /// </summary>
        /// <returns>重连状态信息</returns>
        public (bool IsReconnecting, DateTime LastAttempt, int AttemptCount) GetReconnectStatus()
        {
            lock (_reconnectStateLock)
            {
                return (_isReconnecting, _lastReconnectAttempt, 0); // 简化版本，实际可以跟踪具体尝试次数
            }
        }

        /// <summary>
        /// 手动触发重连
        /// </summary>
        /// <returns>重连任务</returns>
        public async Task<bool> ManualReconnectAsync()
        {
            _logger?.LogDebug("手动触发重连");
            
            if (IsConnected)
            {
                _logger?.LogDebug("已连接，无需重连");
                return true;
            }

            lock (_reconnectStateLock)
            {
                // 重置重连状态
                _reconnectStopped = false;
                _lastReconnectAttempt = DateTime.Now;
            }

            try
            {
                bool connected = await _socketClient.ConnectAsync(_serverAddress, _serverPort, CancellationToken.None);
                if (connected)
                {
                    _isConnected = true;
                    OnConnectionStateChanged(true);
                    _logger?.LogDebug("手动重连成功");
                    OnReconnectSucceeded();
                }
                return connected;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "手动重连失败");
                return false;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                // 设置重连停止标志
                lock (_reconnectStateLock)
                {
                    _reconnectStopped = true;
                    _isReconnecting = false;
                }

                StopAutoReconnect();
                _cancellationTokenSource.Cancel();

                // 取消事件订阅
                if (_socketClient != null)
                {
                    _socketClient.Closed -= OnSocketClosed;
                }

                // 清空所有事件处理器
                ConnectionStateChanged = null;
                ReconnectFailed = null;
                ReconnectAttempt = null;
                ReconnectSucceeded = null;

                // 断开连接 - 使用同步方式断开连接，避免异步调用被丢弃
                try
                {
                    // 同步方式停止重连任务
                    StopReconnectTask();
                    // 同步断开连接，使用ConfigureAwait(false)避免上下文捕获导致的死锁
                    bool disconnectResult = DisconnectInternalAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    _logger?.LogDebug("连接断开{Result}", disconnectResult ? "成功" : "失败");
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "同步断开连接时发生异常");
                }

                _connectionLock?.Dispose();
                _cancellationTokenSource?.Dispose();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "释放连接管理器资源时发生异常");
            }
        }
    }

    /// <summary>
    /// 连接管理器配置
    /// 提供灵活的重连配置选项，支持不同的网络环境和应用场景
    /// </summary>
    public class ConnectionManagerConfig
    {
        /// <summary>
        /// 是否启用自动重连
        /// </summary>
        public bool AutoReconnect { get; set; } = true;

        /// <summary>
        /// 重连间隔（毫秒）
        /// </summary>
        public int ReconnectInterval { get; set; } = 5000; // 5秒

        /// <summary>
        /// 重连检查间隔（毫秒）
        /// </summary>
        public int ReconnectCheckInterval { get; set; } = 1000; // 1秒

        /// <summary>
        /// 最大重连次数（0表示无限重试）
        /// </summary>
        public int MaxReconnectAttempts { get; set; } = 5;

        /// <summary>
        /// 是否启用指数退避算法
        /// </summary>
        public bool EnableExponentialBackoff { get; set; } = true;

        /// <summary>
        /// 最大退避时间（毫秒）
        /// </summary>
        public int MaxBackoffInterval { get; set; } = 60000; // 1分钟

        /// <summary>
        /// 退避乘数
        /// </summary>
        public double BackoffMultiplier { get; set; } = 1.5;

        /// <summary>
        /// 启用随机抖动，避免雷群效应
        /// </summary>
        public bool EnableRandomJitter { get; set; } = true;

        /// <summary>
        /// 随机抖动的百分比范围（0-1），例如0.2表示±20%
        /// </summary>
        public double JitterRatio { get; set; } = 0.2;

        /// <summary>
        /// 网络可用性检查间隔（毫秒），0表示不检查
        /// </summary>
        public int NetworkCheckIntervalMs { get; set; } = 30000; // 30秒

        /// <summary>
        /// 网络检查超时时间（毫秒）
        /// </summary>
        public int NetworkCheckTimeoutMs { get; set; } = 3000; // 3秒

        /// <summary>
        /// 是否在网络恢复时立即尝试重连
        /// </summary>
        public bool ReconnectOnNetworkRecovery { get; set; } = true;

        /// <summary>
        /// 连接断开后延迟重连的时间（毫秒），避免频繁重连
        /// </summary>
        public int ReconnectDelayMs { get; set; } = 2000; // 2秒

        /// <summary>
        /// 手动重连的最小间隔（毫秒）
        /// </summary>
        public int ManualReconnectMinIntervalMs { get; set; } = 5000; // 5秒

        /// <summary>
        /// 重连失败后的冷却时间（毫秒），超过此时间才允许再次自动重连
        /// </summary>
        public int ReconnectFailureCooldownMs { get; set; } = 60000; // 1分钟

        /// <summary>
        /// 连接断开时是否显示通知
        /// </summary>
        public bool ShowConnectionNotifications { get; set; } = true;

        /// <summary>
        /// 默认配置
        /// </summary>
        public static ConnectionManagerConfig Default => new ConnectionManagerConfig();

        /// <summary>
        /// 开发环境配置
        /// 更积极的重连策略，适合开发测试
        /// </summary>
        public static ConnectionManagerConfig Development => new ConnectionManagerConfig
        {
            ReconnectInterval = 2000,           // 2秒
            ReconnectCheckInterval = 500,       // 0.5秒
            MaxReconnectAttempts = 10,           // 10次
            EnableExponentialBackoff = true,
            MaxBackoffInterval = 30000,         // 30秒
            BackoffMultiplier = 1.2,
            EnableRandomJitter = true,
            JitterRatio = 0.1,                // 10%抖动
            NetworkCheckIntervalMs = 15000,      // 15秒
            ReconnectDelayMs = 1000,            // 1秒
            ManualReconnectMinIntervalMs = 2000, // 2秒
            ReconnectFailureCooldownMs = 30000    // 30秒
        };

        /// <summary>
        /// 生产环境配置
        /// 保守稳定的重连策略，适合生产环境
        /// </summary>
        public static ConnectionManagerConfig Production => new ConnectionManagerConfig
        {
            ReconnectInterval = 5000,           // 5秒
            ReconnectCheckInterval = 1000,       // 1秒
            MaxReconnectAttempts = 5,            // 5次
            EnableExponentialBackoff = true,
            MaxBackoffInterval = 120000,        // 2分钟
            BackoffMultiplier = 1.5,
            EnableRandomJitter = true,
            JitterRatio = 0.2,                // 20%抖动
            NetworkCheckIntervalMs = 30000,      // 30秒
            ReconnectDelayMs = 3000,            // 3秒
            ManualReconnectMinIntervalMs = 5000,  // 5秒
            ReconnectFailureCooldownMs = 120000   // 2分钟
        };

        /// <summary>
        /// 高可靠性配置
        /// 专门针对不稳定网络环境，最大化连接可靠性
        /// </summary>
        public static ConnectionManagerConfig HighReliability => new ConnectionManagerConfig
        {
            ReconnectInterval = 3000,           // 3秒
            ReconnectCheckInterval = 500,        // 0.5秒
            MaxReconnectAttempts = 0,           // 无限重试
            EnableExponentialBackoff = true,
            MaxBackoffInterval = 300000,        // 5分钟
            BackoffMultiplier = 2.0,
            EnableRandomJitter = true,
            JitterRatio = 0.3,                // 30%抖动
            NetworkCheckIntervalMs = 10000,      // 10秒
            ReconnectDelayMs = 1000,            // 1秒
            ManualReconnectMinIntervalMs = 3000, // 3秒
            ReconnectFailureCooldownMs = 180000   // 3分钟
        };

        /// <summary>
        /// 验证配置参数的有效性
        /// </summary>
        /// <exception cref="ArgumentException">当配置参数无效时抛出</exception>
        public void Validate()
        {
            if (ReconnectInterval <= 0)
                throw new ArgumentException("重连间隔必须大于0", nameof(ReconnectInterval));

            if (ReconnectCheckInterval <= 0)
                throw new ArgumentException("重连检查间隔必须大于0", nameof(ReconnectCheckInterval));

            if (MaxReconnectAttempts < 0)
                throw new ArgumentException("最大重连次数不能为负数", nameof(MaxReconnectAttempts));

            if (EnableExponentialBackoff)
            {
                if (MaxBackoffInterval <= 0)
                    throw new ArgumentException("最大退避时间必须大于0", nameof(MaxBackoffInterval));

                if (BackoffMultiplier <= 1.0)
                    throw new ArgumentException("退避乘数必须大于1", nameof(BackoffMultiplier));
            }

            if (EnableRandomJitter && (JitterRatio < 0 || JitterRatio > 1))
                throw new ArgumentException("抖动比例必须在0-1之间", nameof(JitterRatio));

            if (NetworkCheckIntervalMs < 0)
                throw new ArgumentException("网络检查间隔不能为负数", nameof(NetworkCheckIntervalMs));

            if (NetworkCheckTimeoutMs <= 0)
                throw new ArgumentException("网络检查超时时间必须大于0", nameof(NetworkCheckTimeoutMs));

            if (ReconnectDelayMs < 0)
                throw new ArgumentException("重连延迟时间不能为负数", nameof(ReconnectDelayMs));

            if (ManualReconnectMinIntervalMs <= 0)
                throw new ArgumentException("手动重连最小间隔必须大于0", nameof(ManualReconnectMinIntervalMs));

            if (ReconnectFailureCooldownMs < 0)
                throw new ArgumentException("重连失败冷却时间不能为负数", nameof(ReconnectFailureCooldownMs));
        }

        /// <summary>
        /// 创建配置的深拷贝
        /// </summary>
        /// <returns>新的配置实例</returns>
        public ConnectionManagerConfig Clone()
        {
            return new ConnectionManagerConfig
            {
                AutoReconnect = this.AutoReconnect,
                ReconnectInterval = this.ReconnectInterval,
                ReconnectCheckInterval = this.ReconnectCheckInterval,
                MaxReconnectAttempts = this.MaxReconnectAttempts,
                EnableExponentialBackoff = this.EnableExponentialBackoff,
                MaxBackoffInterval = this.MaxBackoffInterval,
                BackoffMultiplier = this.BackoffMultiplier,
                EnableRandomJitter = this.EnableRandomJitter,
                JitterRatio = this.JitterRatio,
                NetworkCheckIntervalMs = this.NetworkCheckIntervalMs,
                NetworkCheckTimeoutMs = this.NetworkCheckTimeoutMs,
                ReconnectOnNetworkRecovery = this.ReconnectOnNetworkRecovery,
                ReconnectDelayMs = this.ReconnectDelayMs,
                ManualReconnectMinIntervalMs = this.ManualReconnectMinIntervalMs,
                ReconnectFailureCooldownMs = this.ReconnectFailureCooldownMs,
                ShowConnectionNotifications = this.ShowConnectionNotifications
            };
        }
    }
}