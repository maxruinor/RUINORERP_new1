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

        private string _serverAddress = string.Empty;
        private int _serverPort = 0;

        /// <summary>
        /// 连接状态变更事件
        /// </summary>
        public event Action<bool> ConnectionStateChanged;

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
            if (_isReconnecting || _disposed)
                return;

            _isReconnecting = true;
            _reconnectTask = Task.Run(ReconnectLoopAsync, _cancellationTokenSource.Token);
            _logger?.LogDebug("启动自动重连任务");
        }

        /// <summary>
        /// 停止自动重连
        /// </summary>
        public void StopAutoReconnect()
        {
            _isReconnecting = false;
            StopReconnectTask();
            _logger?.LogDebug("停止自动重连任务");
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

            while (!_disposed && _isReconnecting && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                // 如果已经连接，等待一段时间再检查
                if (IsConnected)
                {
                    await Task.Delay(_config.ReconnectCheckInterval, _cancellationTokenSource.Token);
                    continue;
                }

                // 如果没有服务器信息，等待重连
                if (string.IsNullOrEmpty(_serverAddress) || _serverPort == 0)
                {
                    await Task.Delay(_config.ReconnectCheckInterval, _cancellationTokenSource.Token);
                    continue;
                }

                _logger?.LogDebug("尝试重新连接到服务器 {ServerAddress}:{ServerPort}", _serverAddress, _serverPort);

                try
                {
                    bool connected = await _socketClient.ConnectAsync(_serverAddress, _serverPort, _cancellationTokenSource.Token);

                    if (connected)
                    {
                        _isConnected = true;
                        OnConnectionStateChanged(true);
                        _logger?.LogDebug("重连成功 {ServerAddress}:{ServerPort}", _serverAddress, _serverPort);

                        // 重连成功后，继续监控连接状态
                        continue;
                    }
                    else
                    {
                        _logger?.LogWarning("重连失败 {ServerAddress}:{ServerPort}", _serverAddress, _serverPort);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "重连过程中发生异常 {ServerAddress}:{ServerPort}", _serverAddress, _serverPort);
                }

                // 等待重连间隔
                await Task.Delay(_config.ReconnectInterval, _cancellationTokenSource.Token);
            }

            _logger?.LogDebug("退出重连循环");
        }

        /// <summary>
        /// Socket连接关闭事件处理
        /// </summary>
        private void OnSocketClosed(EventArgs e)
        {
            if (_isConnected)
            {
                _isConnected = false;
                OnConnectionStateChanged(false);
                _logger?.LogDebug("检测到连接已断开");

                // 如果启用了自动重连，启动重连任务
                if (_config.AutoReconnect && !_isReconnecting)
                {
                    StartAutoReconnect();
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
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                StopAutoReconnect();
                _cancellationTokenSource.Cancel();

                // 取消事件订阅
                if (_socketClient != null)
                {
                    _socketClient.Closed -= OnSocketClosed;
                }

                // 断开连接
                _ = DisconnectAsync();

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
        /// 默认配置
        /// </summary>
        public static ConnectionManagerConfig Default => new ConnectionManagerConfig();
    }
}