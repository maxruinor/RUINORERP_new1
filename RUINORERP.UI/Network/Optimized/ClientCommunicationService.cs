using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Common;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 优化后的客户端通信与命令处理服务 - 统一网络通信核心组件
    /// 
    /// 🚀 架构升级：
    /// ✅ 整合通信服务与心跳管理功能
    /// ✅ 提供统一的网络通信接口
    /// ✅ 增强异常处理和状态监控
    /// ✅ 支持智能重连和资源管理
    /// 
    /// 🎯 核心能力：
    /// 1. 统一网络通信接口
    /// 2. 心跳生命周期与连接状态同步管理
    /// 3. 智能重连策略和故障恢复
    /// 4. 请求-响应生命周期管理
    /// 5. 命令生命周期管理
    /// 6. 事件管理和分发
    /// 
    /// 🔗 新架构定位：
    /// 取代原有的ClientCommunicationService和CommunicationManager，
    /// 成为统一的网络通信核心协调器
    /// </summary>
    public class ClientCommunicationService : IClientCommunicationService, IDisposable
    {
        // Socket客户端，负责底层网络通信
        private readonly ISocketClient _socketClient;
        // 请求-响应管理器，处理请求和响应的匹配
        private readonly RequestResponseManager _rrManager;
        // 客户端事件管理器，管理连接状态和命令接收事件
        private readonly ClientEventManager _eventManager;
        // 命令调度器，用于分发命令到对应的处理类
        private readonly ICommandDispatcher _commandDispatcher;
        // 心跳管理器
        private readonly HeartbeatManager _heartbeatManager;        // 日志记录器
        private readonly ILogger<ClientCommunicationService> _logger;
        // 连接状态标志
        private bool _isConnected;
        // 用于线程同步的锁
        private readonly object _syncLock = new object();
        // 是否已释放资源
        private bool _disposed = false;
        // 服务器地址
        private string _serverAddress;
        // 服务器端口
        private int _serverPort;

        // 心跳相关配置
        private int _heartbeatFailureCount = 0;
        private const int MaxHeartbeatFailures = 3;
        private bool _heartbeatIsRunning = false;

        // 重连相关配置
        private bool _autoReconnect = true;
        private int _maxReconnectAttempts = 5;
        private TimeSpan _reconnectDelay = TimeSpan.FromSeconds(5);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socketClient">Socket客户端接口，提供底层网络通信能力</param>
        /// <param name="commandDispatcher">命令调度器，用于分发命令到对应的处理类</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
        public ClientCommunicationService(
            ISocketClient socketClient,
            ICommandDispatcher commandDispatcher,
            ILogger<ClientCommunicationService> logger)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _rrManager = new RequestResponseManager();
            _eventManager = new ClientEventManager();

            UI.Common.HardwareInfo hardwareInfo = Startup.GetFromFac<HardwareInfo>();
            // 生成客户端ID
            string clientId = hardwareInfo.GenerateClientId();
            _socketClient.ClientID = clientId;
            // 直接创建心跳管理器，传递ISocketClient和RequestResponseManager
            _heartbeatManager = new HeartbeatManager(
                _socketClient,
                _rrManager,
                30000 // 默认30秒心跳间隔
            );

            // 注册事件处理程序
            _socketClient.Received += OnReceived;
            _socketClient.Closed += OnClosed;

            // 订阅心跳失败事件
            _heartbeatManager.HeartbeatFailed += OnHeartbeatFailed;

            // 订阅命令接收事件
            SubscribeCommandEvents();
        }

        /// <summary>
        /// 获取当前连接状态
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// 获取服务器地址
        /// </summary>
        public string ServerAddress => _serverAddress;

        /// <summary>
        /// 获取服务器端口
        /// </summary>
        public int ServerPort => _serverPort;

        /// <summary>
        /// 是否启用自动重连功能，默认为true
        /// </summary>
        public bool AutoReconnect
        {
            get => _autoReconnect;
            set => _autoReconnect = value;
        }

        /// <summary>
        /// 最大重连尝试次数，默认为5次
        /// </summary>
        public int MaxReconnectAttempts
        {
            get => _maxReconnectAttempts;
            set => _maxReconnectAttempts = value;
        }

        /// <summary>
        /// 重连间隔时间，默认为5秒
        /// </summary>
        public TimeSpan ReconnectDelay
        {
            get => _reconnectDelay;
            set => _reconnectDelay = value;
        }

        /// <summary>
        /// 命令接收事件，当从服务器接收到命令时触发
        /// </summary>
        public event Action<CommandId, object> CommandReceived
        {
            add => _eventManager.CommandReceived += value;
            remove => _eventManager.CommandReceived -= value;
        }

        /// <summary>
        /// 异步连接到服务器
        /// </summary>
        /// <param name="serverUrl">服务器URL或IP地址</param>
        /// <param name="port">服务器端口号</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>连接成功返回true，失败返回false</returns>
        public Task<bool> ConnectAsync(string serverUrl, int port, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(serverUrl))
                throw new ArgumentException("服务器URL不能为空", nameof(serverUrl));

            if (port <= 0 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port), "端口号必须在1-65535范围内");

            return SafeConnectAsync(serverUrl, port, ct);
        }

        /// <summary>
        /// 断开与服务器的连接
        /// </summary>
        public void Disconnect()
        {
            lock (_syncLock)
            {
                if (_isConnected && !_disposed)
                {
                    try
                    {
                        // 停止心跳
                        if (_heartbeatIsRunning)
                        {
                            _heartbeatManager.Stop();
                            _heartbeatIsRunning = false;
                        }

                        _socketClient.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        _eventManager.OnErrorOccurred(new Exception($"断开连接时发生错误: {ex.Message}", ex));
                    }
                    finally
                    {
                        _isConnected = false;
                        _eventManager.OnConnectionStatusChanged(false);
                    }
                }
            }
        }

        /// <summary>
        /// 异步发送命令到服务器并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒），默认为30000毫秒</param>
        /// <returns>包含响应数据的ApiResponse对象</returns>
        public Task<ApiResponse<TResponse>> SendCommandAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken ct = default,
            int timeoutMs = 30000)
        {
            if (!Enum.IsDefined(typeof(CommandCategory), commandId.Category))
                throw new ArgumentException($"无效的命令类别: {commandId.Category}", nameof(commandId));

            return EnsureConnectedAsync<ApiResponse<TResponse>>(() =>
            {
                var command = InitializeCommandAsync(commandId, requestData);
                return _rrManager.SendRequestAsync<TRequest, ApiResponse<TResponse>>(_socketClient, commandId, requestData, ct, timeoutMs);
            });
        }

        /// <summary>
        /// 异步发送命令到服务器但不等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送成功返回true，失败返回false</returns>
        public async Task<bool> SendOneWayCommandAsync<TRequest>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken ct = default)
        {
            if (!Enum.IsDefined(typeof(CommandCategory), commandId.Category))
                throw new ArgumentException($"无效的命令类别: {commandId.Category}", nameof(commandId));

            return await EnsureConnectedAsync<bool>(async () =>
            {
                try
                {
                    var command = InitializeCommandAsync(commandId, requestData);
                    await _socketClient.SendAsync(command.Packet.ToBinary(), ct).ConfigureAwait(false);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "发送单向命令失败: {CommandId}", commandId.FullCode);
                    _eventManager.OnErrorOccurred(ex);
                    return false;
                }
            });
        }

        /// <summary>
        /// 异步发送命令对象到服务器并等待响应
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>包含响应数据的ApiResponse对象</returns>
        public Task<ApiResponse<TResponse>> SendCommandAsync<TResponse>(
            ICommand command,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return EnsureConnectedAsync<ApiResponse<TResponse>>(() =>
            {
                return SendCommandAsync<object, TResponse>(
                    command.CommandIdentifier,
                    command.GetSerializableData(),
                    cancellationToken,
                    command.TimeoutMs > 0 ? command.TimeoutMs : 30000);
            });
        }

        /// <summary>
        /// 重新连接到服务器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>重连是否成功</returns>
        public Task<bool> ReconnectAsync(CancellationToken cancellationToken = default)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ClientCommunicationService));

            // 断开当前连接（如果有）
            Disconnect();

            // 如果已有服务器地址和端口，则尝试重新连接
            if (!string.IsNullOrEmpty(_serverAddress) && _serverPort > 0)
            {
                return ConnectAsync(_serverAddress, _serverPort, cancellationToken);
            }

            // 如果没有保存的服务器地址，返回失败
            _eventManager.OnErrorOccurred(new InvalidOperationException("没有保存的服务器地址和端口"));
            return Task.FromResult(false);
        }

        /// <summary>
        /// 确保连接状态正常并执行操作
        /// </summary>
        /// <typeparam name="TResult">操作结果类型</typeparam>
        /// <param name="operation">要执行的操作</param>
        /// <returns>操作结果</returns>
        private async Task EnsureConnectedAsync(Func<Task> operation)
        {
            if (!_isConnected)
            {
                _eventManager.OnErrorOccurred(new InvalidOperationException("未连接到服务器"));
                throw new InvalidOperationException("未连接到服务器");
            }

            try
            {
                await operation().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(ex);
                _logger.LogError(ex, "操作执行失败");

                // 连接断开时尝试重连
                if (_autoReconnect && !_isConnected)
                {
                    _logger.LogInformation("连接已断开，尝试自动重连");
                    await TryReconnectAsync().ConfigureAwait(false);
                }

                throw;
            }
        }

        /// <summary>
        /// 确保连接状态正常并执行操作
        /// </summary>
        /// <typeparam name="TResult">操作结果类型</typeparam>
        /// <param name="operation">要执行的操作</param>
        /// <returns>操作结果</returns>
        private async Task<TResult> EnsureConnectedAsync<TResult>(Func<Task<TResult>> operation)
        {
            if (!_isConnected)
            {
                _eventManager.OnErrorOccurred(new InvalidOperationException("未连接到服务器"));
                _logger.LogError("未连接到服务器，无法执行操作");
                throw new InvalidOperationException("未连接到服务器");
            }

            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(ex);
                _logger.LogError(ex, "操作执行失败");

                // 连接断开时尝试重连
                if (_autoReconnect && !_isConnected)
                {
                    _logger.LogInformation("连接已断开，尝试自动重连");
                    await TryReconnectAsync().ConfigureAwait(false);
                }

                throw;
            }
        }

        /// <summary>
        /// 初始化命令对象
        /// </summary>
        /// <typeparam name="TData">命令数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="data">命令数据</param>
        /// <returns>初始化后的GenericCommand对象</returns>
        private GenericCommand<TData> InitializeCommandAsync<TData>(CommandId commandId, TData data)
        {
            var command = new GenericCommand<TData>(commandId, data);
            command.TimeoutMs = 30000; // 设置默认超时时间
            command.UpdateTimestamp();

            return command;
        }

        /// <summary>
        /// 安全连接异步方法
        /// </summary>
        /// <param name="serverUrl">服务器URL</param>
        /// <param name="port">服务器端口</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>连接成功返回true，失败返回false</returns>
        private async Task<bool> SafeConnectAsync(string serverUrl, int port, CancellationToken ct)
        {
            lock (_syncLock)
            {
                if (_isConnected)
                    return true;

                _serverAddress = serverUrl;
                _serverPort = port;
            }

            try
            {
                bool connected = await _socketClient.ConnectAsync(serverUrl, port, ct).ConfigureAwait(false);

                if (connected)
                {
                    lock (_syncLock)
                    {
                        _isConnected = true;
                        _heartbeatFailureCount = 0;
                    }

                    // 启动心跳
                    _heartbeatManager.Start();

                    _heartbeatIsRunning = true;
                    _eventManager.OnConnectionStatusChanged(true);
                }

                return connected;
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"连接到服务器失败: {ex.Message}", ex));
                _logger.LogError(ex, "连接服务器时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 尝试重连到服务器
        /// </summary>
        /// <returns>重连成功返回true，失败返回false</returns>
        private async Task<bool> TryReconnectAsync()
        {
            if (!_autoReconnect || _disposed || string.IsNullOrEmpty(_serverAddress))
                return false;

            _logger.LogInformation("开始尝试重连服务器...");

            for (int attempt = 0; attempt < _maxReconnectAttempts; attempt++)
            {
                if (_disposed)
                    break;

                _logger.LogInformation($"重连尝试 {attempt + 1}/{_maxReconnectAttempts}");

                try
                {
                    if (await _socketClient.ConnectAsync(_serverAddress, _serverPort, CancellationToken.None).ConfigureAwait(false))
                    {
                        lock (_syncLock)
                        {
                            _isConnected = true;
                            _heartbeatFailureCount = 0;
                        }

                        // 重启心跳
                        if (!_heartbeatIsRunning)
                        {
                            _heartbeatManager.Start();
                            _heartbeatIsRunning = true;
                        }

                        _eventManager.OnConnectionStatusChanged(true);
                        _logger.LogInformation("服务器重连成功");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "重连失败");
                    _eventManager.OnErrorOccurred(new Exception($"重连尝试 {attempt + 1} 失败: {ex.Message}", ex));
                }

                // 等待重连延迟
                if (attempt < _maxReconnectAttempts - 1)
                {
                    _logger.LogInformation($"等待 {_reconnectDelay.TotalSeconds} 秒后进行下一次重连");
                    await Task.Delay(_reconnectDelay, CancellationToken.None).ConfigureAwait(false);
                }
            }

            _logger.LogError("达到最大重连尝试次数，重连失败");
            _eventManager.OnErrorOccurred(new Exception("重连服务器失败: 达到最大尝试次数"));
            return false;
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <returns>任务</returns>
        private Task SendHeartbeatAsync()
        {
            var heartbeatCommandId = new CommandId(CommandCategory.System, PacketSpec.Commands.System.SystemCommands.Heartbeat.OperationCode);

            // 使用RequestResponseManager发送心跳请求
            return _rrManager.SendRequestAsync<object, object>(_socketClient,
                heartbeatCommandId,
                null,
                CancellationToken.None,
                5000 // 设置心跳包超时时间为5秒
            ).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled || (task.IsCompleted))
                {
                    HandleHeartbeatFailure(new Exception("cancel"));
                }
                else
                {
                    lock (_syncLock)
                    {
                        _heartbeatFailureCount = 0; // 重置失败计数
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        /// <summary>
        /// 处理心跳包失败
        /// </summary>
        private void HandleHeartbeatFailure(Exception exception)
        {
            lock (_syncLock)
            {
                _heartbeatFailureCount++;
                _logger.LogWarning($"心跳包失败次数: {_heartbeatFailureCount}/{MaxHeartbeatFailures}");

                if (_heartbeatFailureCount >= MaxHeartbeatFailures)
                {
                    _logger.LogError("心跳包连续失败，断开连接并尝试重连");

                    // 断开连接
                    if (_isConnected)
                    {
                        _socketClient.Disconnect();
                        _isConnected = false;
                        _eventManager.OnConnectionStatusChanged(false);
                    }

                    // 尝试重连
                    if (_autoReconnect && !_disposed)
                    {
                        Task.Run(() => TryReconnectAsync());
                    }
                }
            }
        }

        /// <summary>
        /// 处理心跳失败事件
        /// </summary>
        private void OnHeartbeatFailed(Exception exception)
        {
            try
            {
                HandleHeartbeatFailure(exception);
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(ex);
                _logger.LogError(ex, "处理心跳失败事件时发生异常");
            }
        }

        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        /// <param name="data">接收到的数据</param>
        private async void OnReceived(PacketModel packet)
        {
            try
            {
                if (packet != null)
                {
                    // 处理请求响应
                    if (packet.IsValid() && packet.Direction == PacketDirection.Request)
                    {
                        //TODO 处理请求响应
                    }
                    // 处理服务器主动发送的命令
                    else if (packet.IsValid() && packet.Command.FullCode > 0)
                    {
                        _eventManager.OnCommandReceived(packet.Command, packet.Body);
                        await ProcessCommandAsync(packet.Command, packet.Body);
                    }
                }
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"处理接收到的数据时发生错误: {ex.Message}", ex));
                _logger.LogError(ex, "处理接收到的数据时发生错误");
            }
        }

        /// <summary>
        /// 处理接收到的命令
        /// </summary>
        /// <param name="commandId">命令标识符</param>
        /// <param name="data">命令回应的业务数据</param>
        private async Task ProcessCommandAsync(CommandId commandId, byte[] data)
        {
            try
            {
                // 创建一个临时的命令对象用于调度
                var command = new GenericCommand<object>(commandId, data);

                // 根据命令类别进行特殊处理
                if (commandId.Category == CommandCategory.System)
                {
                    // 处理系统命令，如心跳响应等
                    if (commandId.FullCode == PacketSpec.Commands.System.SystemCommands.HeartbeatResponse.FullCode)
                    {
                        // 处理心跳响应，重置失败计数
                        lock (_syncLock)
                        {
                            _heartbeatFailureCount = 0;
                        }
                    }
                }

                // 调度命令到命令处理器
                await _commandDispatcher.DispatchAsync(command, CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"处理命令 {commandId} 时发生错误: {ex.Message}", ex));
                _logger.LogError(ex, "处理命令时发生错误");
            }
        }

        /// <summary>
        /// 处理连接关闭事件
        /// </summary>
        private void OnClosed(EventArgs eventArgs)
        {
            lock (_syncLock)
            {
                if (_isConnected)
                {
                    _isConnected = false;

                    // 停止心跳
                    if (_heartbeatIsRunning)
                    {
                        _heartbeatManager.Stop();
                        _heartbeatIsRunning = false;
                    }

                    _eventManager.OnConnectionStatusChanged(false);
                    _logger.LogInformation("连接已关闭");

                    // 尝试重连
                    if (_autoReconnect && !_disposed)
                    {
                        _logger.LogInformation("自动重连已启用，尝试重连服务器");
                        Task.Run(() => TryReconnectAsync());
                    }
                }
            }
        }



        /// <summary>
        /// 订阅命令接收事件
        /// </summary>
        private void SubscribeCommandEvents()
        {
            // 这里可以根据需要添加特定命令的事件订阅
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
        /// 释放资源的实际实现
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    Disconnect();

                    // 取消事件订阅
                    if (_socketClient != null)
                    {
                        _socketClient.Received -= OnReceived;
                        _socketClient.Closed -= OnClosed;
                    }

                    // 取消心跳失败事件订阅
                    if (_heartbeatManager != null)
                    {
                        _heartbeatManager.HeartbeatFailed -= OnHeartbeatFailed;
                    }

                    // 停止心跳并释放资源
                    if (_heartbeatManager != null)
                    {
                        _heartbeatManager.Stop();
                        _heartbeatManager.Dispose();
                    }
                }

                // 释放非托管资源
                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~ClientCommunicationService()
        {
            Dispose(false);
        }
    }
}