using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;

using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.RetryStrategy;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using Org.BouncyCastle.Ocsp;
using System.Diagnostics;
using RUINORERP.PacketSpec.Core.DataProcessing;
using Org.BouncyCastle.Bcpg;
using NPOI.POIFS.Crypt.Dsig;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.Network.Authentication;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Tokens;
using RUINORERP.PacketSpec.Commands;

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
    /// ✅ 支持Token管理和自动刷新
    /// 
    /// 🎯 核心能力：
    /// 1. 统一网络通信接口
    /// 2. 心跳生命周期与连接状态同步管理
    /// 3. 智能重连策略和故障恢复
    /// 4. 请求-响应生命周期管理
    /// 5. 命令生命周期管理
    /// 6. 事件管理和分发
    /// 7. Token自动附加和过期处理
    /// 
    /// 🔗 新架构定位：
    /// 取代原有的ClientCommunicationService和CommunicationManager，
    /// 成为统一的网络通信核心协调器
    /// </summary>
    public class ClientCommunicationService : IClientCommunicationService, IDisposable
    {
        private readonly NetworkConfig _config;
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
        // 命令类型助手
        private readonly CommandTypeHelper _commandTypeHelper;
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
        private readonly CommandPacketAdapter commandPacketAdapter;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        // 用于Token刷新的内部实现

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socketClient">Socket客户端接口，提供底层网络通信能力</param>
        /// <param name="commandDispatcher">命令调度器，用于分发命令到对应的处理类</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="commandTypeHelper">命令类型助手，用于管理命令类型映射关系</param>
        /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
        public ClientCommunicationService(
            ISocketClient socketClient,
            CommandPacketAdapter _commandPacketAdapter,
        ICommandDispatcher commandDispatcher,
            ILogger<ClientCommunicationService> logger,
            CommandTypeHelper commandTypeHelper = null)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            commandPacketAdapter = _commandPacketAdapter;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandTypeHelper = commandTypeHelper ?? new CommandTypeHelper();
            _rrManager = new RequestResponseManager();
            _eventManager = new ClientEventManager();
            NetworkConfig config = new NetworkConfig();
            UI.Common.HardwareInfo hardwareInfo = Startup.GetFromFac<HardwareInfo>();
            // 生成客户端ID
            if (string.IsNullOrEmpty(_socketClient.ClientID))
            {
                _socketClient.ClientID = hardwareInfo.GenerateClientId(); 
            }
     
            // 直接创建心跳管理器，传递ISocketClient和RequestResponseManager
            // 获取UserLoginService实例并创建心跳管理器
            var userLoginService = Startup.GetFromFac<RUINORERP.UI.Network.Services.UserLoginService>();
            _heartbeatManager = new HeartbeatManager(
                _socketClient,
                _rrManager,
                userLoginService,
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
        public async Task<bool> ConnectAsync(string serverUrl, int port, CancellationToken ct = default)
        {
            await _connectionLock.WaitAsync(ct);
            try
            {
                if (string.IsNullOrEmpty(serverUrl))
                    throw new ArgumentException("服务器URL不能为空", nameof(serverUrl));

                if (port <= 0 || port > 65535)
                    throw new ArgumentOutOfRangeException(nameof(port), "端口号必须在1-65535范围内");

                return await SafeConnectAsync(serverUrl, port, ct);
            }
            finally
            {
                _connectionLock.Release();
            }

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
        /// 异步发送命令到服务器并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒），默认为30000毫秒</param>
        /// <returns>TResponse</returns>
        public async Task<TResponse> SendCommandAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken ct = default,
            int timeoutMs = 30000)
        {
            if (!Enum.IsDefined(typeof(CommandCategory), commandId.Category))
                throw new ArgumentException($"无效的命令类别: {commandId.Category}", nameof(commandId));

            return await EnsureConnectedAsync<TResponse>(async () =>
            {
                var command = InitializeCommandAsync(commandId, requestData);

                try
                {
                    // BaseCommand会自动处理Token管理，包括获取和刷新Token
                    return await _rrManager.SendRequestAsync<TRequest, TResponse>(_socketClient, commandId, requestData, ct, timeoutMs, command.AuthToken);
                }
                catch (Exception ex) when (ex.Message.IndexOf("token expired", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   ex.Message.IndexOf("unauthorized", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   ex.Message.IndexOf("认证失败", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   ex.Message.IndexOf("未授权", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   ex.Message.IndexOf("权限不足", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Token过期情况，现在由BaseCommand统一处理
                    _logger.LogWarning("检测到Token过期，BaseCommand会自动处理刷新");
                    throw; // 抛出异常，让调用方处理或让BaseCommand的机制处理
                }
            });
        }


        public async Task<TResponse> SendCommandAsync<TRequest, TResponse>(
         BaseCommand command,
         CancellationToken ct = default,
         int timeoutMs = 30000)
        {
            if (!Enum.IsDefined(typeof(CommandCategory), command.CommandIdentifier.Category))
                throw new ArgumentException($"无效的命令类别: {command.CommandIdentifier.Category}", nameof(command.CommandIdentifier));

            return await EnsureConnectedAsync<TResponse>(async () =>
            {
                try
                {
                    // 由于RequestResponseManager的SendCommandAsync<TRequest, TResponse>方法需要BaseCommand<TRequest, TResponse>类型
                    // 而接口定义不允许添加泛型约束，我们需要使用非泛型版本的SendRequestAsync方法
                    // 发送请求并等待响应
                    return await _rrManager.SendRequestAsync<BaseCommand, TResponse>(
                        _socketClient,
                        command.CommandIdentifier,
                        command,
                        ct,
                        timeoutMs);
                }
                catch (Exception ex) when (ex.Message.IndexOf("token expired", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   ex.Message.IndexOf("unauthorized", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   ex.Message.IndexOf("认证失败", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   ex.Message.IndexOf("未授权", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   ex.Message.IndexOf("权限不足", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Token过期情况，现在由BaseCommand统一处理
                    _logger.LogWarning("检测到Token过期，BaseCommand会自动处理刷新");
                    throw; // 抛出异常，让调用方处理或让BaseCommand的机制处理
                }
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
                    // 创建命令对象并设置Token
                    var command = InitializeCommandAsync(commandId, requestData);

                    // 生成请求ID但不等待响应
                    string requestId = RUINORERP.PacketSpec.Core.IdGenerator.NewRequestId(commandId);

                    // 通过RequestResponseManager发送请求，确保Token正确附加
                    await _rrManager.SendCoreAsync(
                        _socketClient,
                        commandId,
                        requestData,
                        requestId,
                        5000, // 单向命令的较短超时时间
                        ct,
                        command.AuthToken).ConfigureAwait(false);

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
        /// <returns></returns>
        public Task<TResponse> SendCommandAsync<TResponse>(
            ICommand command,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return EnsureConnectedAsync<TResponse>(() =>
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
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                _eventManager.OnErrorOccurred(ex);
                _logger.LogError(ex, "操作执行失败: {Operation}", operation.Method.Name);

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
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                _eventManager.OnErrorOccurred(ex);
                _logger.LogError(ex, "操作执行失败: {Operation}", operation.Method.Name);

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

            // 附加认证令牌
            var (success, accessToken, _) = TokenManager.Instance.GetTokens();
            if (success && !string.IsNullOrEmpty(accessToken))
            {
                command.AuthToken = accessToken;
                command.TokenType = "Bearer";
            }

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
        /// 异步发送命令到服务器并等待响应，支持重试策略
        /// 提供带重试逻辑的命令发送，适用于网络不稳定环境下的可靠通信
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="retryStrategy">重试策略，如果为null则使用默认策略</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">单次请求超时时间（毫秒），默认30000毫秒</param>
        /// <returns>包含响应数据的ApiResponse对象</returns>
        /// <exception cref="ArgumentException">当命令类别无效时抛出</exception>
        /// <exception cref="ArgumentOutOfRangeException">当超时时间小于等于0时抛出</exception>
        public async Task<TResponse> SendCommandWithRetryAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest requestData,
            IRetryStrategy retryStrategy = null,
            CancellationToken ct = default,
            int timeoutMs = 30000)
        {
            if (!Enum.IsDefined(typeof(CommandCategory), commandId.Category))
                throw new ArgumentException($"无效的命令类别: {commandId.Category}", nameof(commandId));

            if (timeoutMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutMs), "超时时间必须大于0");

            // 不再手动附加Token，BaseCommand会自动处理Token管理
            return await _rrManager.SendRequestWithRetryAsync<TRequest, TResponse>(
                _socketClient, commandId, requestData, retryStrategy, ct, timeoutMs);
        }

        /// <summary>
        /// 异步发送命令对象到服务器并等待响应，支持重试策略
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="retryStrategy">重试策略，如果为null则使用默认策略</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>包含响应数据的ApiResponse对象</returns>
        /// <exception cref="ArgumentNullException">当命令对象为null时抛出</exception>
        public Task<TResponse> SendCommandWithRetryAsync<TResponse>(
            ICommand command,
            IRetryStrategy retryStrategy = null,
            CancellationToken ct = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return SendCommandWithRetryAsync<object, TResponse>(
                command.CommandIdentifier,
                command.GetSerializableData(),
                retryStrategy,
                ct,
                command.TimeoutMs > 0 ? command.TimeoutMs : 30000);
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
                    // 先尝试作为响应处理
                    if (_rrManager.HandleResponse(packet))
                    {
                        return; // 如果是响应，处理完成，不再作为命令处理
                    }

                    // 如果不是响应，再作为命令处理
                    if (packet.IsValid() && packet.Command.FullCode > 0)
                    {
                        _eventManager.OnCommandReceived(packet.Command, packet.CommandData);
                        await ProcessCommandAsync(packet.Command, packet.CommandData);
                    }

                    //// 处理请求响应
                    //if (packet.IsValid() && packet.Direction == PacketDirection.Request)
                    //{
                    //    //TODO 处理请求响应
                    //}
                    //// 处理服务器主动发送的命令
                    //else if (packet.IsValid() && packet.Command.FullCode > 0)
                    //{
                    //    _eventManager.OnCommandReceived(packet.Command, packet.Body);
                    //    await ProcessCommandAsync(packet.Command, packet.Body);
                    //}
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
        /// 异步发送单向命令到服务器（不等待响应），支持重试策略
        /// 提供带重试逻辑的单向命令发送，适用于网络不稳定环境下的可靠通信
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="retryStrategy">重试策略，如果为null则使用默认策略</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送成功返回true，失败返回false</returns>
        /// <exception cref="ArgumentException">当命令类别无效时抛出</exception>
        public async Task<bool> SendOneWayCommandWithRetryAsync<TRequest>(
            CommandId commandId,
            TRequest requestData,
            IRetryStrategy retryStrategy = null,
            CancellationToken ct = default)
        {
            if (!Enum.IsDefined(typeof(CommandCategory), commandId.Category))
                throw new ArgumentException($"无效的命令类别: {commandId.Category}", nameof(commandId));

            // 如果没有提供重试策略，使用默认策略
            if (retryStrategy == null)
            {
                retryStrategy = new ExponentialBackoffRetryStrategy(100);
            }

            int attempt = 0;
            Exception lastException = null;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                while (true)
                {
                    ct.ThrowIfCancellationRequested();

                    try
                    {
                        _logger?.LogDebug("开始发送单向命令，命令ID: {CommandId}，尝试次数: {Attempt}", commandId, attempt + 1);

                        // 发送单向命令
                        bool result = await SafeSendOneWayAsync(commandId, requestData, ct);

                        if (result)
                        {
                            _logger?.LogDebug("单向命令发送成功，命令ID: {CommandId}，总耗时: {TotalTime}ms，尝试次数: {Attempt}",
                                commandId, stopwatch.ElapsedMilliseconds, attempt + 1);
                            return true;
                        }
                        else
                        {
                            throw new Exception("单向命令发送失败");
                        }
                    }
                    catch (Exception ex) when (IsRetryableException(ex))
                    {
                        lastException = ex;

                        // 检查是否应该继续重试
                        if (!retryStrategy.ShouldContinue(attempt))
                        {
                            _logger?.LogError(ex, "单向命令重试失败，已达到最大重试次数，命令ID: {CommandId}", commandId);
                            return false;
                        }

                        // 获取下一次重试的延迟时间
                        int delayMs = retryStrategy.GetNextDelay(attempt);

                        _logger?.LogWarning(ex, "单向命令失败将重试，命令ID: {CommandId}，尝试次数: {Attempt}，延迟时间: {DelayMs}ms",
                            commandId, attempt + 1, delayMs);

                        // 等待重试延迟
                        await Task.Delay(delayMs, ct);
                        attempt++;
                    }
                    catch (OperationCanceledException)
                    {
                        // 不重试取消操作
                        throw;
                    }
                    catch (Exception ex)
                    {
                        // 处理不可重试的异常
                        lastException = ex;

                        _logger?.LogError(ex, "单向命令处理失败，不可重试的错误，命令ID: {CommandId}", commandId);
                        return false;
                    }
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }


        /// <summary>
        /// 安全地异步发送单向命令（包含异常处理）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="data">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送成功返回true，失败返回false</returns>
        private async Task<bool> SafeSendOneWayAsync<TRequest>(CommandId commandId, TRequest data, CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                lock (_syncLock)
                {
                    if (!_isConnected)
                        return false;

                    if (_disposed)
                        throw new ObjectDisposedException(nameof(ClientCommunicationService));
                }

                // 序列化请求数据，Token管理现在由BaseCommand统一处理
                var payload = UnifiedSerializationService.SerializeWithMessagePack(data);

                // 创建原始数据包
                var original = new OriginalData(
                    (byte)commandId.Category,
                    new[] { commandId.OperationCode },
                    payload);

                // 加密数据包
                var encrypted = EncryptedProtocol.EncryptClientPackToServer(original);

                // 发送数据
                await _socketClient.SendAsync(encrypted, ct);
                return true;
            }
            catch (OperationCanceledException)
            {
                _eventManager.OnErrorOccurred(new Exception("发送命令操作被取消"));
                return false;
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"单向命令发送失败: {ex.Message}", ex));
                return false;
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
            _eventManager.CommandReceived += OnCommandReceived;
            _logger.LogInformation("客户端命令处理器已启动，开始监听服务器命令");
        }

        /// <summary>
        /// 当接收到服务器命令时触发
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        private async void OnCommandReceived(CommandId commandId, object data)
        {
            try
            {
                _logger.LogInformation("接收到服务器命令: {CommandId}", commandId);

                // 使用命令调度器处理命令
                await ProcessCommandAsync(commandId, RUINORERP.PacketSpec.Serialization.UnifiedSerializationService.SerializeToBinary<object>(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理服务器命令时发生错误: {CommandId}", commandId);
                _eventManager.OnErrorOccurred(new Exception($"处理命令 {commandId} 时发生错误: {ex.Message}", ex));
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
        /// 释放资源的实际实现
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    Disconnect();

                    // 取消事件订阅
                    if (_socketClient != null)
                    {   // 取消事件订阅
                        _socketClient.Received -= OnReceived;
                        _socketClient.Closed -= OnClosed;
                        //_eventManager.CommandReceived -= OnCommandReceived;

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

                    // 断开连接
                    // 断开连接
                    try
                    {
                        Disconnect();
                    }
                    catch { }
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




        /// </summary>
        /// <typeparam name="TReq">请求DTO类型</typeparam>
        /// <typeparam name="TResp">响应DTO类型</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="adapter">自定义适配器；null 时使用默认 JsonPacketAdapter</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>响应DTO</returns>
        public Task<TResp> CallAsync<TReq, TResp>(
            TReq request,
            IPacketAdapter<TReq, TResp> adapter = null,
            CancellationToken ct = default)
        {
            // 1. 默认适配器（90% 场景够用）
            //adapter ??= new GenericCommandPacketAdapter<TReq, TResp>(_commandTypeHelper.GetCommandId<TReq>());

            // 2. 打包 -> 发送 -> 解包
            var packet = adapter.Pack(request, _socketClient.ClientID, null);


            return SendCommandAsync<PacketModel, PacketModel>(packet.Command, packet, ct)
                   .ContinueWith(t => adapter.Unpack(t.Result), ct,
                                 TaskContinuationOptions.ExecuteSynchronously,
                                 TaskScheduler.Default);


            //// 发送请求并等待响应
            //return _rrManager.SendRequestAsync<PacketModel, PacketModel>(_socketClient, commandId, packet, ct)
            //    .ContinueWith(task =>
            //    {
            //        if (task.IsFaulted)
            //            throw task.Exception.InnerException;

            //        // 解包响应数据
            //        return adapter.Unpack(task.Result);
            //    });


        }



        /// <summary>
        /// 发送请求并等待响应（兼容旧API）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <param name="adapter">数据包适配器</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>响应数据</returns>
        public async Task<TResponse> SendAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest request,
            IPacketAdapter<TRequest, TResponse> adapter = null,
            CancellationToken ct = default,
            int timeoutMs = 30000)
        {
            // 直接调用SendCommandAsync方法实现，该方法已包含Token管理逻辑
            return await SendCommandAsync<TRequest, TResponse>(commandId, request, ct, timeoutMs);
        }

        #region Token管理相关方法（简化版）
        // 删除所有手动Token管理方法，只保留框架自动处理
        // Token管理统一在BaseCommand中处理
        #endregion


        /// <summary>
        /// 发送命令并等待响应（使用命令对象）
        /// </summary>
        /// <typeparam name="TResp">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="adapter">数据包适配器</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>带包装的API响应</returns>
        public Task<TResp> CallAsync<TResp>(ICommand command,
                                         IPacketAdapter<object, TResp> adapter = null,
                                         CancellationToken ct = default) where TResp : class
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            // 创建默认适配器如果未提供
            //if (adapter == null)
            //    adapter = new GenericCommandPacketAdapter<object, TResp>(command.CommandIdentifier);

            // 构建数据包
            PacketModel packet = commandPacketAdapter.ToPacket(command);


            // 发送请求并等待响应
            return _rrManager.SendRequestAsync<PacketModel, PacketModel>(_socketClient, command.CommandIdentifier, packet, ct)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                        throw task.Exception.InnerException;

                    // 解包响应数据
                    return adapter.Unpack(task.Result);
                });
        }

    }


    /// <summary>
    /// 后面再优化 是不是DI注入 并且可以配置文件配置
    /// </summary>
    public class NetworkConfig
    {
        public int HeartbeatIntervalMs { get; set; } = 30000;
        public int MaxHeartbeatFailures { get; set; } = 3;
        public int MaxReconnectAttempts { get; set; } = 5;
        public TimeSpan ReconnectDelay { get; set; } = TimeSpan.FromSeconds(5);
        public int RequestTimeoutMs { get; set; } = 30000;
    }

}