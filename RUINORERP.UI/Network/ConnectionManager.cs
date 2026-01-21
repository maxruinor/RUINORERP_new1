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
    /// 职责：统一管理连接状态和重连逻辑,作为状态唯一真实来源
    /// </summary>
    public class ConnectionManager : IDisposable
    {
        private readonly ISocketClient _socketClient;
        private readonly ILogger<ConnectionManager> _logger;
        private readonly ConnectionManagerConfig _config;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private bool _isReconnecting = false;
        private bool _disposed = false;
        private Task _reconnectTask = null;
        private bool _reconnectStopped = false;
        private bool _isNetworkAvailable = true;

        private string _serverAddress = string.Empty;
        private int _serverPort = 0;
        private DateTime _lastReconnectAttempt = DateTime.MinValue;
        private readonly object _reconnectStateLock = new object();

        // 网络状态变化事件句柄
        private readonly System.Net.NetworkInformation.NetworkAvailabilityChangedEventHandler _networkAvailabilityChangedHandler;

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
        /// 直接查询底层Socket客户端状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                try
                {
                    // 直接返回Socket客户端的连接状态
                    return _socketClient.IsConnected;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "ConnectionManager检查连接状态时发生异常");
                    return false;
                }
            }
        }

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

            // 订阅Socket客户端事件 - 确保状态同步
            _socketClient.Closed += OnSocketClosed;
            
            // 订阅Socket客户端连接状态变更事件
            if (_socketClient is SuperSocketClient superSocketClient)
            {
                superSocketClient.ConnectionStateChanged += OnSocketConnectionStateChanged;
            }

            // 初始化网络状态变化事件处理
            _networkAvailabilityChangedHandler = OnNetworkAvailabilityChanged;
            System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged += _networkAvailabilityChangedHandler;
            // 初始检查网络状态
            _isNetworkAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            _logger?.LogDebug("初始网络状态：{IsAvailable}", _isNetworkAvailable);
        }

        /// <summary>
        /// Socket连接状态变更事件处理
        /// 触发连接状态变更事件
        /// </summary>
        private void OnSocketConnectionStateChanged(bool connected)
        {
            _logger?.LogDebug("收到Socket连接状态变更通知: {Connected}", connected);

            // 如果连接成功，清除重连状态标志
            if (connected && _isReconnecting)
            {
                lock (_reconnectStateLock)
                {
                    if (_isReconnecting)
                    {
                        _logger?.LogDebug("Socket连接成功，清除IsReconnecting标志");
                        _isReconnecting = false;
                    }
                }
            }

            // 触发状态变更事件
            OnConnectionStateChanged(connected);
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
                // 保存服务器信息 - 必须在判断之前更新，确保后续判断使用的是新地址
                _serverAddress = serverAddress;
                _serverPort = serverPort;

                // 如果已经连接到同一服务器，先验证Socket是否真的可用
                if (IsConnected && _serverAddress == serverAddress && _serverPort == serverPort)
                {
                    _logger?.LogDebug("检查连接到目标服务器 {ServerAddress}:{ServerPort} 的状态", serverAddress, serverPort);

                    // 验证Socket是否真的可用
                    if (_socketClient.IsConnected)
                    {
                        _logger?.LogDebug("已经连接到目标服务器 {ServerAddress}:{ServerPort}，且Socket可用", serverAddress, serverPort);
                        return true;
                    }
                    else
                    {
                        // 虽然IsConnected标志为true，但Socket已断开，需要重新连接
                        _logger?.LogWarning("连接状态标志显示已连接，但Socket实际已断开，需要重新连接");
                        await DisconnectInternalAsync();
                    }
                }

                // 如果已连接到其他服务器，先断开
                if (IsConnected)
                {
                    //断开当前连接，准备连接到新服务器;
                    await DisconnectInternalAsync();
                }

                // 尝试连接

                bool connected = await _socketClient.ConnectAsync(serverAddress, serverPort, cancellationToken);

                if (connected)
                {
                    _logger?.LogDebug("成功连接到服务器 {ServerAddress}:{ServerPort}", serverAddress, serverPort);

                    // 清除重连状态标志
                    lock (_reconnectStateLock)
                    {
                        if (_isReconnecting)
                        {
                            _logger?.LogDebug("连接成功，清除IsReconnecting标志");
                            _isReconnecting = false;
                        }
                    }

                    OnConnectionStateChanged(true);
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
            if (!IsConnected)
                return true;

            try
            {
                _logger?.LogWarning("[主动断开连接] 开始断开与服务器的连接: {ServerAddress}:{ServerPort}", _serverAddress, _serverPort);

                bool result = await _socketClient.Disconnect();
                OnConnectionStateChanged(false);
                _logger?.LogDebug("已成功断开与服务器的连接: {ServerAddress}:{ServerPort}", _serverAddress, _serverPort);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "断开与服务器 {ServerAddress}:{ServerPort} 的连接时发生异常", _serverAddress, _serverPort);
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
        /// 重连循环（优化版 - 修复退出逻辑）
        /// 避免资源竞争和重复重连，提高稳定性
        /// 确保重连成功后正确退出循环
        /// </summary>
        private async Task ReconnectLoopAsync()
        {
            _logger?.LogDebug("进入重连循环");
            int reconnectAttempts = 0;
            int currentBackoffInterval = _config.ReconnectInterval;

            while (ShouldContinueReconnecting())
            {
                await Task.Delay(_config.ReconnectCheckInterval, _cancellationTokenSource.Token).ConfigureAwait(false);

                // 如果已连接，退出重连循环
                if (IsConnected)
                {
                    if (reconnectAttempts > 0)
                    {
                        HandleReconnectSuccess(ref reconnectAttempts, ref currentBackoffInterval);
                    }
                    break;
                }

                // 检查服务器信息完整性
                if (!HasValidServerInfo())
                {
                    continue;
                }

                // 检查是否达到最大重连次数
                if (ShouldStopReconnecting(reconnectAttempts))
                {
                    StopReconnecting();
                    break;
                }

                // 执行重连尝试
                bool reconnectResult = await AttemptReconnectAsync(reconnectAttempts);

                if (reconnectResult)
                {
                    // 重连成功，退出循环
                    HandleReconnectSuccess(ref reconnectAttempts, ref currentBackoffInterval);
                    break;
                }
                else
                {
                    reconnectAttempts++;
                    _lastReconnectAttempt = DateTime.Now;
                    currentBackoffInterval = CalculateNextBackoffInterval(reconnectAttempts, currentBackoffInterval);
                }
            }

            // 清除重连状态标志
            lock (_reconnectStateLock)
            {
                _isReconnecting = false;
            }

            _logger?.LogDebug("退出重连循环");
        }

        /// <summary>
        /// 检查是否应该继续重连
        /// </summary>
        /// <returns>是否继续重连</returns>
        private bool ShouldContinueReconnecting()
        {
            return !_disposed && !_reconnectStopped && _isReconnecting && !_cancellationTokenSource.Token.IsCancellationRequested;
        }

        /// <summary>
        /// 检查服务器信息是否有效
        /// </summary>
        /// <returns>服务器信息是否有效</returns>
        private bool HasValidServerInfo()
        {
            return !string.IsNullOrEmpty(_serverAddress) && _serverPort > 0;
        }

        /// <summary>
        /// 检查是否应该停止重连
        /// </summary>
        /// <param name="attempts">当前尝试次数</param>
        /// <returns>是否应该停止</returns>
        private bool ShouldStopReconnecting(int attempts)
        {
            return _config.MaxReconnectAttempts > 0 && attempts >= _config.MaxReconnectAttempts;
        }

        /// <summary>
        /// 停止重连操作
        /// </summary>
        private void StopReconnecting()
        {
            lock (_reconnectStateLock)
            {
                _isReconnecting = false;
                _reconnectStopped = true;
            }
            
            _logger?.LogWarning("已达到最大重连次数 {MaxAttempts}，停止重连", _config.MaxReconnectAttempts);
            OnReconnectFailed();
        }

        /// <summary>
        /// 执行重连尝试
        /// </summary>
        /// <param name="attempt">当前尝试次数</param>
        /// <returns>重连是否成功</returns>
        private async Task<bool> AttemptReconnectAsync(int attempt)
        {
            _logger?.LogDebug("尝试重新连接到服务器 {ServerAddress}:{ServerPort}，第 {Attempt} 次尝试", _serverAddress, _serverPort, attempt);
            OnReconnectAttempt(attempt, _config.MaxReconnectAttempts);

            try
            {
                bool connected = await _socketClient.ConnectAsync(_serverAddress, _serverPort, _cancellationTokenSource.Token);

                if (connected)
                {
                    _logger?.LogDebug("重连成功 {ServerAddress}:{ServerPort}，共尝试 {Attempt} 次", _serverAddress, _serverPort, attempt);
                    OnReconnectSucceeded();
                    return true;
                }
                else
                {
                    _logger?.LogWarning("重连失败 {ServerAddress}:{ServerPort}，第 {Attempt} 次尝试", _serverAddress, _serverPort, attempt);
                    return false;
                }
            }
            catch (OperationCanceledException)
            {
                _logger?.LogDebug("重连操作被取消");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "重连过程中发生异常 {ServerAddress}:{ServerPort}，第 {Attempt} 次尝试", _serverAddress, _serverPort, attempt);
                return false;
            }
        }

        /// <summary>
        /// 处理重连成功
        /// </summary>
        /// <param name="attempts">当前尝试次数</param>
        /// <param name="backoffInterval">当前退避间隔</param>
        private void HandleReconnectSuccess(ref int attempts, ref int backoffInterval)
        {
            if (attempts > 0)
            {
                _logger?.LogDebug("重连成功，重置重连计数器");
                OnReconnectSucceeded();
            }

            attempts = 0;
            backoffInterval = _config.ReconnectInterval;
        }

        /// <summary>
        /// 计算下一个退避间隔
        /// </summary>
        /// <param name="attempts">当前尝试次数</param>
        /// <param name="currentInterval">当前间隔</param>
        /// <returns>下一个退避间隔</returns>
        private int CalculateNextBackoffInterval(int attempts, int currentInterval)
        {
            // 如果网络不可用，使用更长的重连间隔
            if (!_isNetworkAvailable)
            {
                // 网络不可用时，使用最大间隔的一半
                int networkDownInterval = _config.MaxBackoffInterval / 2;
                _logger?.LogDebug("网络不可用，使用更长的重连间隔 {Interval} 毫秒", networkDownInterval);
                return networkDownInterval;
            }

            if (!_config.EnableExponentialBackoff || attempts <= 0)
            {
                return _config.ReconnectInterval;
            }

            // 计算指数退避时间
            var baseInterval = (int)Math.Min(
                currentInterval * _config.BackoffMultiplier,
                _config.MaxBackoffInterval);
            
            // 添加随机抖动避免雷群效应
            if (_config.EnableRandomJitter)
            {
                var randomJitter = new Random().Next(-baseInterval / 5, baseInterval / 5);
                baseInterval = Math.Max(_config.ReconnectInterval, baseInterval + randomJitter);
            }
            
            _logger?.LogDebug("应用指数退避，下次重连间隔 {Interval} 毫秒", baseInterval);
            return baseInterval;
        }

        /// <summary>
        /// Socket连接关闭事件处理
        /// </summary>
        private void OnSocketClosed(EventArgs e)
        {
            // 触发连接断开事件
            OnConnectionStateChanged(false);

            bool shouldStartReconnect = false;

            lock (_reconnectStateLock)
            {
                // 检查是否需要启动重连
                if (!_isReconnecting && !_disposed && _config.AutoReconnect)
                {
                    shouldStartReconnect = true;
                    _isReconnecting = true;
                    _logger?.LogDebug("准备启动自动重连机制");
                }
            }

            // 在锁外启动重连
            if (shouldStartReconnect)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(_config.ReconnectDelayMs).ConfigureAwait(false);

                    // 再次检查连接状态，避免重复启动
                    lock (_reconnectStateLock)
                    {
                        if (!IsConnected && _isReconnecting && !_disposed)
                        {
                            StartAutoReconnect();
                        }
                        else if (IsConnected)
                        {
                            _logger?.LogDebug("连接已恢复，取消重连");
                            _isReconnecting = false;
                        }
                    }
                }).ConfigureAwait(false);
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
                // 1. 首先检查本地网络接口状态
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    _logger?.LogDebug("本地网络接口不可用");
                    return false;
                }

                // 2. 检查本地回环地址，确认网络栈正常
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var localReply = await ping.SendPingAsync("127.0.0.1", 1000); // 本地回环，1秒超时
                    if (localReply.Status != System.Net.NetworkInformation.IPStatus.Success)
                    {
                        _logger?.LogDebug("本地回环地址Ping失败，网络栈异常：{Status}", localReply.Status);
                        return false;
                    }
                }

                // 3. 可选：尝试检查网关地址（如果能获取到）
                try
                {
                    var networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                        .Where(nic => nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up &&
                                      nic.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Loopback);

                    foreach (var networkInterface in networkInterfaces)
                    {
                        var gatewayAddresses = networkInterface.GetIPProperties().GatewayAddresses;
                        foreach (var gatewayAddress in gatewayAddresses)
                        {
                            if (gatewayAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                using (var ping = new System.Net.NetworkInformation.Ping())
                                {
                                    var gatewayReply = await ping.SendPingAsync(gatewayAddress.Address, 2000); // 网关，2秒超时
                                    if (gatewayReply.Status == System.Net.NetworkInformation.IPStatus.Success)
                                    {
                                        // 网关可达，网络基本可用
                                        _logger?.LogDebug("网关可达，网络可用");
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogDebug(ex, "网关检查失败，继续进行其他检查");
                    // 网关检查失败，继续进行其他检查
                }

                // 4. 网络接口可用，但无法访问网关，仍尝试重连
                _logger?.LogDebug("网络接口可用，但网关不可达，仍尝试重连");
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "网络可用性检查异常，假设网络可用");
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
        /// 网络可用性变化事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnNetworkAvailabilityChanged(object sender, System.Net.NetworkInformation.NetworkAvailabilityEventArgs e)
        {
            bool oldState = _isNetworkAvailable;
            _isNetworkAvailable = e.IsAvailable;
            
            _logger?.LogDebug("网络状态变化：从 {OldState} 变为 {NewState}", oldState, _isNetworkAvailable);
            
            // 如果网络从不可用变为可用，且当前未连接，触发重连
            if (!oldState && _isNetworkAvailable && !IsConnected)
            {
                _logger?.LogInformation("网络恢复，尝试重新连接到服务器 {ServerAddress}:{ServerPort}", _serverAddress, _serverPort);
                
                // 检查服务器信息完整性
                if (HasValidServerInfo())
                {
                    // 使用Task.Run避免阻塞事件线程
                    _ = Task.Run(async () =>
                    {
                        // 延迟一下再重连，确保网络完全恢复
                        await Task.Delay(1000);
                        
                        // 检查是否应该重连
                        if (_config.AutoReconnect && !IsConnected && !_isReconnecting && !_disposed)
                        {
                            StartAutoReconnect();
                        }
                    });
                }
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

                // 取消网络状态变化事件订阅
                System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged -= _networkAvailabilityChangedHandler;

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