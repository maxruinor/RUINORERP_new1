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
using RUINORERP.UI.Network.TimeoutStatistics;
using RUINORERP.UI.Network.RetryStrategy;
using System.Collections.Concurrent;
using RUINORERP.UI.Network.ErrorHandling;

using RUINORERP.UI.Network.Exceptions;
using FastReport.DevComponents.DotNetBar;
using RUINORERP.Common.Extensions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Timer = System.Threading.Timer;
using System.Collections.Generic;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 优化后的客户端通信与命令处理服务 - 统一网络通信核心组件
    /// </summary>
    public class ClientCommunicationService : IDisposable
    {
        /// <summary>
        /// 用户登录服务实例，用于重连后的认证恢复
        /// </summary>
        private UserLoginService _userLoginService;

        /// <summary>
        /// 设置用户登录服务实例
        /// </summary>
        /// <param name="loginService">用户登录服务</param>
        public void SetUserLoginService(UserLoginService loginService)
        {
            _userLoginService = loginService;
        }
        private readonly NetworkConfig _config;
        // Socket客户端，负责底层网络通信
        private readonly ISocketClient _socketClient;
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
        private bool _heartbeatIsRunning = false;

        // 网络配置
        private readonly NetworkConfig _networkConfig;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        // 请求队列，用于存储连接断开时的单向命令请求
        private readonly ConcurrentQueue<QueuedCommand> _queuedCommands = new ConcurrentQueue<QueuedCommand>();
        private bool _isProcessingQueue = false;
        private readonly SemaphoreSlim _queueProcessingLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 队列命令模型，支持单向命令和带响应命令
        /// </summary>
        private class QueuedCommand
        {
            public CommandId CommandId { get; set; }
            public object Data { get; set; }
            public CancellationToken CancellationToken { get; set; }
            public Type DataType { get; set; }
            public TaskCompletionSource<bool> CompletionSource { get; set; } // 用于单向命令
            public TaskCompletionSource<PacketModel> ResponseCompletionSource { get; set; } // 用于带响应命令
            public int TimeoutMs { get; set; } = 30000;
            public bool IsResponseCommand { get; set; } // 标识是否是带响应的命令
        }

        // 用于Token刷新的内部实现

        // 请求响应管理相关字段（从RequestResponseManager迁移）
        private readonly ConcurrentDictionary<string, PendingRequest> _pendingRequests = new();
        private readonly TimeoutStatisticsManager _timeoutStatistics;
        private readonly ErrorHandlingStrategyFactory _errorHandlingStrategyFactory;
        private Timer _cleanupTimer;

        private readonly TokenManager tokenManager;

        /// <summary>
        /// 待处理请求的内部类
        /// </summary>
        private class PendingRequest
        {
            public TaskCompletionSource<PacketModel> Tcs { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CommandId { get; set; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socketClient">Socket客户端接口，提供底层网络通信能力</param>
        /// <param name="commandCreationService">命令创建服务</param>
        /// <param name="commandDispatcher">命令调度器，用于分发命令到对应的处理类</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="networkConfig">网络配置</param>
        /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
        public ClientCommunicationService(
            ISocketClient socketClient,
        ICommandDispatcher commandDispatcher,
            ILogger<ClientCommunicationService> logger,
            TokenManager _tokenManager,
            NetworkConfig networkConfig = null)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _networkConfig = networkConfig ?? NetworkConfig.Default;
            _eventManager = new ClientEventManager();
            tokenManager = _tokenManager;
            // 初始化请求响应管理相关组件
            _timeoutStatistics = new TimeoutStatisticsManager();
            _errorHandlingStrategyFactory = new ErrorHandlingStrategyFactory();

            #region  扫描注册
            // 获取PacketSpec程序集
            var packetSpecAssembly = Assembly.GetAssembly(typeof(RUINORERP.PacketSpec.Models.Core.PacketModel));
            if (packetSpecAssembly == null)
            {
                return;
            }
            // 获取UI程序集
            var uiAssembly = Assembly.GetExecutingAssembly();

            // 扫描并注册命令到命令调度器
            _commandDispatcher.InitializeAsync(CancellationToken.None, packetSpecAssembly, uiAssembly);
            #endregion

            // 初始化定时清理任务
            _cleanupTimer = new Timer(CleanupTimeoutRequests, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
            UI.Common.HardwareInfo hardwareInfo = Startup.GetFromFac<HardwareInfo>();
            // 生成客户端ID
            if (string.IsNullOrEmpty(_socketClient.ClientID))
            {
                _socketClient.ClientID = hardwareInfo.GenerateClientId();
            }

            // 直接创建心跳管理器，传递ISocketClient和ClientCommunicationService
            // HeartbeatManager不再依赖任何登录服务，直接使用TokenManager
            _heartbeatManager = new HeartbeatManager(
                _socketClient,
                this, // 传递当前ClientCommunicationService实例
                tokenManager,
                _networkConfig.HeartbeatIntervalMs,
                _networkConfig.HeartbeatTimeoutMs
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
        /// 更新连接状态
        /// </summary>
        /// <param name="connected">是否已连接</param>
        private void UpdateConnectionState(bool connected)
        {
            var previousState = _isConnected;
            _isConnected = connected;

            // 如果连接状态发生变化，触发事件
            if (previousState != connected)
            {
                _eventManager.OnConnectionStatusChanged(connected);

                if (connected)
                {
                    _logger.Debug("客户端已连接到服务器");
                    // 注意：心跳将在用户登录成功后启动，而不是在连接建立时

                    // 连接恢复后，处理队列中的请求
                    if (_queuedCommands.Count > 0)
                    {
                        _logger.Debug($"连接恢复，开始处理队列中的{_queuedCommands.Count}个请求");
                        Task.Run(() => ProcessCommandQueueAsync());
                    }
                }
                else
                {
                    _logger.Debug("客户端与服务器断开连接");

                    // 停止心跳
                    if (_heartbeatIsRunning)
                    {
                        _heartbeatManager.Stop();
                        _heartbeatIsRunning = false;
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                // 检查SocketClient的实际连接状态，确保与_isConnected字段同步
                lock (_syncLock)
                {
                    if (_socketClient == null || _disposed)
                        return false;

                    // 检查SocketClient的实际连接状态
                    var socketConnected = _socketClient.IsConnected;

                    // 如果Socket实际已断开但_isConnected仍为true，则更新状态
                    if (!socketConnected && _isConnected)
                    {
                        _logger.LogWarning("检测到Socket连接已断开，但连接状态未同步更新，正在修复...");
                        UpdateConnectionState(false);
                    }

                    return _isConnected && socketConnected;
                }
            }
        }






        /// <summary>
        /// 获取网络配置
        /// </summary>
        public NetworkConfig NetworkConfig => _networkConfig;

        /// <summary>
        /// 命令接收事件，当从服务器接收到命令时触发
        /// </summary>
        public event Action<PacketModel, object> CommandReceived
        {
            add => _eventManager.CommandReceived += value;
            remove => _eventManager.CommandReceived -= value;
        }

        /// <summary>
        /// 订阅特定命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="handler">处理函数</param>
        public void SubscribeCommand(CommandId commandId, Action<PacketModel, object> handler)
        {
            _eventManager.SubscribeCommand(commandId, handler);
        }

        /// <summary>
        /// 取消订阅特定命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="handler">处理函数</param>
        public void UnsubscribeCommand(CommandId commandId, Action<PacketModel, object> handler)
        {
            _eventManager.UnsubscribeCommand(commandId, handler);
        }

        /// <summary>
        /// 重连失败事件，当客户端重连服务器失败时触发
        /// </summary>
        public event Action ReconnectFailed
        {
            add => _eventManager.ReconnectFailed += value;
            remove => _eventManager.ReconnectFailed -= value;
        }

        /// <summary>
        /// 连接状态变化事件，当连接状态改变时触发
        /// </summary>
        public event Action<bool> ConnectionStateChanged
        {
            add => _eventManager.ConnectionStatusChanged += value;
            remove => _eventManager.ConnectionStatusChanged -= value;
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
            // 使用信号量确保同一时间只有一个连接尝试
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
        /// 断开与服务器的连接 - 增强状态同步验证
        /// </summary>
        public void Disconnect()
        {
            lock (_syncLock)
            {
                if (_isConnected && !_disposed)
                {
                    try
                    {
                        // 验证Socket实际状态
                        if (_socketClient.IsConnected)
                        {
                            _socketClient.Disconnect();
                            _logger.Debug("主动断开与服务器的连接");
                        }
                        else
                        {
                            _logger.LogWarning("尝试断开连接时发现Socket已处于断开状态");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "断开连接时发生错误");
                        _eventManager.OnErrorOccurred(new Exception($"断开连接时发生错误: {ex.Message}", ex));
                    }
                    finally
                    {
                        // 使用统一的连接状态更新方法
                        UpdateConnectionState(false);
                    }
                }
            }
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
                // 如果已经连接，直接返回true
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
                        _heartbeatFailureCount = 0;
                        // 使用统一的连接状态更新方法
                        UpdateConnectionState(true);
                    }
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
        /// 在用户登录成功后启动心跳
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>启动心跳的任务</returns>
        public async Task StartHeartbeatAfterLoginAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // 确保连接已建立
                if (!_isConnected)
                {
                    _logger?.LogWarning("尝试启动心跳，但连接未建立");
                    return;
                }

                // 确保心跳未运行
                if (_heartbeatIsRunning)
                {
                    _logger?.LogDebug("心跳已在运行，无需重复启动");
                    return;
                }

                // 启动心跳

                _heartbeatManager.Start();
                _heartbeatIsRunning = true;

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "启动心跳时发生错误");
                _eventManager.OnErrorOccurred(new Exception($"启动心跳失败: {ex.Message}", ex));
                throw;
            }
        }





        /// <summary>
        /// 处理命令队列中的请求
        /// </summary>
        private async Task ProcessCommandQueueAsync()
        {
            // 使用信号量确保同一时间只有一个处理队列的任务在运行
            if (!await _queueProcessingLock.WaitAsync(0))
                return;

            try
            {
                _isProcessingQueue = true;
                _logger.Debug("开始处理命令队列");

                while (!_queuedCommands.IsEmpty && _isConnected)
                {
                    if (!_queuedCommands.TryDequeue(out var queuedCommand))
                        continue;

                    // 检查是否已取消
                    if (queuedCommand.CancellationToken.IsCancellationRequested)
                    {
                        if (queuedCommand.IsResponseCommand)
                        {
                            queuedCommand.ResponseCompletionSource.TrySetCanceled();
                        }
                        else
                        {
                            queuedCommand.CompletionSource.TrySetResult(false);
                        }
                        continue;
                    }

                    try
                    {
                        if (queuedCommand.IsResponseCommand)
                        {
                            // 处理带响应的命令
                            _logger.Debug("处理队列中的带响应命令: {CommandId}", queuedCommand.CommandId.Name);

                            // 调用SendCommandAsync方法
                            var result = await SendCommandAsync(
                                queuedCommand.CommandId,
                                queuedCommand.Data as IRequest,
                                queuedCommand.CancellationToken,
                                queuedCommand.TimeoutMs);

                            // 设置任务结果
                            queuedCommand.ResponseCompletionSource.TrySetResult(result);
                        }
                        else
                        {
                            // 处理单向命令
                            // 动态调用SendOneWayCommandAsync方法
                            var method = GetType().GetMethod(
                                nameof(SendOneWayCommandAsync),
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public,
                                null,
                                new[] { typeof(CommandId), queuedCommand.DataType, typeof(CancellationToken) },
                                null);

                            if (method != null)
                            {
                                var genericMethod = method.MakeGenericMethod(queuedCommand.DataType);
                                var result = (Task<bool>)genericMethod.Invoke(this, new object[]
                                { queuedCommand.CommandId, queuedCommand.Data, queuedCommand.CancellationToken });

                                bool success = await result;
                                queuedCommand.CompletionSource.TrySetResult(success);
                            }
                            else
                            {
                                _logger.LogError("找不到SendOneWayCommandAsync方法");
                                queuedCommand.CompletionSource.TrySetResult(false);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "处理队列命令时发生错误: {CommandId}", queuedCommand.CommandId.Name);
                        if (queuedCommand.IsResponseCommand)
                        {
                            queuedCommand.ResponseCompletionSource.TrySetException(ex);
                        }
                        else
                        {
                            queuedCommand.CompletionSource.TrySetResult(false);
                        }
                    }

                    // 添加短暂延迟，避免大量请求同时发送
                    await Task.Delay(10);
                }

                _logger.Debug("命令队列处理完成");
            }
            finally
            {
                _isProcessingQueue = false;
                _queueProcessingLock.Release();
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
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">请求超时时间（毫秒）</param>
        /// <returns>响应数据对象</returns>
        private async Task<PacketModel> SendRequestAsync<TRequest, TResponse>(
           CommandId commandId,
            TRequest request,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
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
                await SendPacketCoreAsync<TRequest>(_socketClient, commandId, request, _networkConfig.DefaultRequestTimeoutMs, ct, ResponseTypeName);

                // 等待响应或超时
                var timeoutTask = Task.Delay(timeoutMs, cts.Token);
                var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    _timeoutStatistics.RecordTimeout(commandId.ToString(), timeoutMs);
                    throw new TimeoutException($"请求超时（{timeoutMs}ms），指令类型：{commandId.ToString()}，请求ID: {request.RequestId}");
                }

                ct.ThrowIfCancellationRequested();

                var responsePacket = await tcs.Task;

                if (responsePacket != null)
                {
                    // 记录请求完成事件
                    _eventManager.OnRequestCompleted(request.RequestId, DateTime.UtcNow - pendingRequest.CreatedAt);
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
                if (tokenManager?.TokenStorage == null) return;

                // 获取令牌并验证有效性
                var tokenInfo = await tokenManager.TokenStorage.GetTokenAsync();

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

                // 验证数据包有效性
                if (!packet.IsValid())
                {
                    _logger.LogWarning("接收到无效数据包");
                    return;
                }

                // 1. 首先尝试作为响应处理（请求-响应模式）
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
                _eventManager.OnErrorOccurred(new Exception($"处理接收到的数据时发生错误: {ex.Message}", ex));
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
                   packet.Direction == PacketDirection.Response;
        }

        /// <summary>
        /// 判断是否为服务器主动推送的命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>是否为服务器主动推送的命令</returns>
        private bool IsServerPushCommand(PacketModel packet)
        {
            // 服务器主动推送的命令通常没有请求ID，或者方向为推送
            return packet.Direction == PacketDirection.ServerToClient ||
                   string.IsNullOrEmpty(packet?.ExecutionContext?.RequestId);
        }

        /// <summary>
        /// 处理响应数据包
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
                // 优先使用事件机制处理命令，避免重复处理
                _eventManager.OnCommandReceived(packet, packet.Response);

                // 如果事件机制没有处理该命令（没有订阅者），则使用命令调度器处理
                if (!_eventManager.HasCommandSubscribers(packet))
                {
                    await ProcessCommandAsync(packet);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理服务器主动推送命令时发生错误: {CommandId}", packet.CommandId);
                _eventManager.OnErrorOccurred(new Exception($"处理推送命令 {packet.CommandId} 时发生错误: {ex.Message}", ex));
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
                // 使用命令调度器处理命令
                await ProcessCommandAsync(packet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理通用命令时发生错误: {CommandId}", packet.CommandId);
                _eventManager.OnErrorOccurred(new Exception($"处理通用命令 {packet.CommandId} 时发生错误: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 清理超时请求
        /// </summary>
        /// <param name="state">状态对象</param>
        private void CleanupTimeoutRequests(object state)
        {
            var now = DateTime.UtcNow;
            var cut = now.AddMinutes(-5);
            var removedCount = 0;

            foreach (var kv in _pendingRequests)
            {
                if (kv.Value.CreatedAt < cut && _pendingRequests.TryRemove(kv.Key, out var pr))
                {
                    pr.Tcs.TrySetException(new TimeoutException($"请求 {kv.Key} 超时"));
                    removedCount++;
                    _logger?.LogDebug("清理超时请求: {RequestId}, 超时时间: 5分钟", kv.Key);
                }
            }

            if (removedCount > 0)
            {
                _logger?.LogDebug("清理了 {RemovedCount} 个超时请求", removedCount);
            }
        }

        /// <summary>
        /// 发送请求并等待响应，支持重试策略（合并自RequestResponseManager）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="request">命令对象</param>
        /// <param name="retryStrategy">重试策略，如果为null则使用默认策略</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">单次请求超时时间（毫秒），默认30000毫秒</param>
        /// <returns>包含响应数据的ApiResponse对象</returns>
        /// <exception cref="ArgumentException">当命令类别无效时抛出</exception>
        /// <exception cref="ArgumentOutOfRangeException">当超时时间小于等于0时抛出</exception>
        public async Task<PacketModel> SendRequestWithRetryAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest request,
            IRetryStrategy retryStrategy = null,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            if (!Enum.IsDefined(typeof(CommandCategory), commandId.Category))
                throw new ArgumentException($"无效的命令类别: {commandId.Category}", commandId.Name);

            if (timeoutMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutMs), "超时时间必须大于0");

            //// 使用默认重试策略如果没有提供
            //retryStrategy ??= _errorHandlingStrategyFactory.GetDefaultRetryStrategy();

            // 如果没有提供重试策略，使用默认策略
            if (retryStrategy == null)
            {
                retryStrategy = new ExponentialBackoffRetryStrategy(100);
            }


            var attempt = 0;
            Exception lastException = null;
            while (attempt < _networkConfig.MaxRetryAttempts)
            {
                attempt++;

                try
                {
                    _logger?.LogDebug("发送请求尝试 {Attempt}/{MaxRetries}", attempt, _networkConfig.MaxRetryAttempts);
                    var response = await SendRequestAsync<TRequest, TResponse>(commandId, request, ct, timeoutMs);
                    return response;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    var errorType = IdentifyErrorType(ex);

                    _logger?.LogWarning(ex, "请求失败（尝试 {Attempt}/{MaxRetries}），错误类型: {ErrorType}",
                        attempt, _networkConfig.MaxRetryAttempts, errorType);

                    // 检查是否应该重试
                    if (!IsRetryableException(ex) || attempt >= _networkConfig.MaxRetryAttempts)
                    {
                        _logger?.LogError(ex, "请求最终失败，不再重试");
                        throw new InvalidOperationException($"请求失败（尝试 {attempt} 次），错误: {ex.Message}", ex);
                    }

                    // 等待重试延迟
                    if (attempt < _networkConfig.MaxRetryAttempts)
                    {
                        var delay = retryStrategy.GetNextDelay(attempt);
                        _logger?.LogDebug("等待 {DelayMs}ms 后进行第 {NextAttempt} 次重试",
                            delay, attempt + 1);
                        await Task.Delay(delay, ct);
                    }
                }
            }

            throw new InvalidOperationException($"请求失败（尝试 {attempt} 次），错误: {lastException?.Message}", lastException);
        }


        /// 异步发送命令到服务器并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒），默认为30000毫秒</param>
        /// <returns>TResponse</returns>
        public async Task<PacketModel> SendCommandAsync(
            CommandId commandId,
             IRequest request,
            CancellationToken ct = default,
            int timeoutMs = 30000)
        {
            if (!Enum.IsDefined(typeof(CommandCategory), commandId.Category))
                throw new ArgumentException($"无效的命令类别: {commandId.Category}", nameof(commandId.Name));

            return await EnsureConnectedAsync<PacketModel>(async () =>
            {
                try
                {
                    // BaseCommand会自动处理Token管理，包括获取和刷新Token
                    return await SendRequestAsync<IRequest, IResponse>(commandId, request, ct, timeoutMs);
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
        /// 确保连接状态正常并执行操作
        /// </summary>
        /// <typeparam name="TResult">操作结果类型</typeparam>
        /// <param name="operation">要执行的操作</param>
        /// <returns>操作结果</returns>
        private async Task<TResult> EnsureConnectedAsync<TResult>(Func<Task<TResult>> operation)
        {
            // 检查连接状态，如果断开且启用了自动重连，则尝试重连
            if (!_isConnected && _networkConfig.AutoReconnect)
            {
                _logger.Debug("检测到连接断开，尝试自动重连");
                // 尝试重连并等待结果
                bool reconnected = await TryReconnectAsync().ConfigureAwait(false);

                // 如果重连失败，抛出异常
                if (!reconnected)
                {
                    var errorMsg = "未连接到服务器，重连失败";
                    _eventManager.OnErrorOccurred(new InvalidOperationException(errorMsg));
                    _logger.LogError(errorMsg);
                    throw new InvalidOperationException(errorMsg);
                }
            }
            // 如果未启用自动重连但连接已断开，直接抛出异常
            else if (!_isConnected)
            {
                var errorMsg = "未连接到服务器";
                _eventManager.OnErrorOccurred(new InvalidOperationException(errorMsg));
                _logger.LogError(errorMsg);
                throw new InvalidOperationException(errorMsg);
            }

            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                _eventManager.OnErrorOccurred(ex);
                _logger.LogError(ex, $"操作执行失败: {ex.Message}", operation.Method.Name);

                // 检查是否是网络异常，如果是则尝试重连
                if (_networkConfig.AutoReconnect && !_isConnected)
                {
                    _logger.Debug("连接已断开，尝试自动重连");
                    await TryReconnectAsync().ConfigureAwait(false);
                }

                throw;
            }
        }

        /// <summary>
        /// 确保连接状态正常并执行无返回值操作
        /// </summary>
        /// <param name="operation">要执行的操作</param>
        private async Task EnsureConnectedAsync(Func<Task> operation) =>
            await EnsureConnectedAsync(async () =>
            {
                await operation().ConfigureAwait(false);
                return true;
            }).ConfigureAwait(false);


        /// <summary>
        /// 尝试重连到服务器
        /// </summary>
        /// <returns>重连成功返回true，失败返回false</returns>
        private async Task<bool> TryReconnectAsync()
        {
            if (!_networkConfig.AutoReconnect || _disposed || string.IsNullOrEmpty(_serverAddress))
                return false;

            _logger.Debug("开始尝试重连服务器...");

            for (int attempt = 0; attempt < _networkConfig.MaxReconnectAttempts; attempt++)
            {
                if (_disposed)
                    break;

                _logger.Debug($"重连尝试 {attempt + 1}/{_networkConfig.MaxReconnectAttempts}");

                try
                {
                    // 使用连接锁确保不会并发重连
                    await _connectionLock.WaitAsync();
                    try
                    {
                        // 检查是否已经连接
                        if (_isConnected)
                        {
                            _logger.Debug("检测到已连接，取消重连尝试");
                            return true;
                        }

                        if (await _socketClient.ConnectAsync(_serverAddress, _serverPort, CancellationToken.None).ConfigureAwait(false))
                        {
                            lock (_syncLock)
                            {
                                _heartbeatFailureCount = 0;
                                // 使用统一的连接状态更新方法
                                UpdateConnectionState(true);
                            }

                            _logger.Debug("服务器重连成功");
                            // 通过UpdateConnectionState方法会触发连接状态变更事件
                            // UserLoginService可以订阅此事件并自行处理认证恢复
                            return true;
                        }
                    }
                    finally
                    {
                        _connectionLock.Release();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "重连失败");
                    _eventManager.OnErrorOccurred(new Exception($"重连尝试 {attempt + 1} 失败: {ex.Message}", ex));
                }

                // 等待重连延迟
                if (attempt < _networkConfig.MaxReconnectAttempts - 1)
                {
                    _logger.Debug($"等待 {_networkConfig.ReconnectDelay.TotalSeconds} 秒后进行下一次重连");
                    await Task.Delay(_networkConfig.ReconnectDelay, CancellationToken.None).ConfigureAwait(false);
                }
            }

            _logger.LogError("达到最大重连尝试次数，重连失败");
            _eventManager.OnErrorOccurred(new Exception("重连服务器失败: 达到最大尝试次数"));

            // 触发重连失败事件，通知UI层进行注销锁定处理
            _eventManager.OnReconnectFailed();

            return false;
        }


        /// <summary>
        /// 处理心跳包失败 - 增强状态同步检查
        /// </summary>
        private void HandleHeartbeatFailure(Exception exception)
        {
            lock (_syncLock)
            {
                _heartbeatFailureCount++;
                _logger.LogWarning($"心跳包失败次数: {_heartbeatFailureCount}/{_networkConfig.MaxHeartbeatFailures}");

                if (_heartbeatFailureCount >= _networkConfig.MaxHeartbeatFailures)
                {
                    _logger.LogError("心跳包连续失败，断开连接并尝试重连");

                    // 断开连接前检查实际Socket状态
                    if (_isConnected)
                    {
                        // 验证Socket实际状态
                        if (_socketClient.IsConnected)
                        {
                            _socketClient.Disconnect();
                        }
                        else
                        {
                            _logger.LogWarning("检测到Socket已断开，但连接状态未同步，立即更新状态");
                        }

                        // 使用统一的连接状态更新方法
                        UpdateConnectionState(false);
                    }

                    // 尝试重连
                    if (_networkConfig.AutoReconnect && !_disposed)
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
        /// 处理接收到的命令
        /// </summary>
        /// <param name="Packet">数据包</param>
        private async Task ProcessCommandAsync(PacketModel Packet)
        {
            try
            {

                // 根据命令类别进行特殊处理
                switch (Packet.CommandId.Category)
                {
                    case CommandCategory.System:
                        await ProcessSystemCommandAsync(Packet);
                        break;

                    case CommandCategory.Cache:
                        await ProcessCacheCommandAsync(Packet);
                        break;

                    case CommandCategory.Authentication:
                        await ProcessAuthenticationCommandAsync(Packet);
                        break;

                    default:
                        // 使用命令调度器处理其他命令
                        await _commandDispatcher.DispatchAsync(Packet, CancellationToken.None).ConfigureAwait(false);
                        break;
                }
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"处理命令 {Packet.CommandId} 时发生错误: {ex.Message}", ex));
                _logger.LogError(ex, "处理命令时发生错误");
            }
        }

        /// <summary>
        /// 处理系统命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task ProcessSystemCommandAsync(PacketModel packet)
        {
            // 处理系统命令，如心跳响应等
            if (packet.CommandId.FullCode == PacketSpec.Commands.System.SystemCommands.HeartbeatResponse.FullCode)
            {
                // 处理心跳响应，重置失败计数
                lock (_syncLock)
                {
                    _heartbeatFailureCount = 0;
                }
                _logger.LogDebug("收到心跳响应，重置心跳失败计数");
            }
            else
            {
                // 其他系统命令使用调度器处理
                await _commandDispatcher.DispatchAsync(packet, CancellationToken.None).ConfigureAwait(false);
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
            await _commandDispatcher.DispatchAsync(packet, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// 处理认证命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task ProcessAuthenticationCommandAsync(PacketModel packet)
        {
            // 认证命令使用调度器处理
            await _commandDispatcher.DispatchAsync(packet, CancellationToken.None).ConfigureAwait(false);
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
                    .WithDirection(PacketDirection.Request) // 明确设置请求方向
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
                packet.ExecutionContext.UserId = MainForm.Instance.AppContext.CurrentUser.UserID;
                //  CommandContext用于传递响应类型信息
                packet.ExecutionContext.ExpectedResponseTypeName = ResponseTypeName;

                await AutoAttachTokenAsync(packet.ExecutionContext);
                //除登陆登出命令，其他命令都需要附加令牌
                if (packet.CommandId != AuthenticationCommands.Login)
                {
                    if (packet.ExecutionContext.Token == null)
                    {
                        // 附加令牌
                        throw new Exception($"发送请求失败: 没有合法授权令牌,指令：{commandId.ToString()}");
                    }
                }
                // 序列化和加密数据包
                var payload = JsonCompressionSerializationService.Serialize<PacketModel>(packet);
                var original = new OriginalData((byte)packet.CommandId.Category, new[] { packet.CommandId.OperationCode }, payload);
                var encrypted = UnifiedEncryptionProtocol.EncryptClientDataToServer(original);

                await client.SendAsync(encrypted, ct);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"发送数据包时发生错误: CommandId={commandId}",
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
                if (!IsConnected)
                {
                    _logger.LogWarning("尝试发送单向命令但连接已断开，命令ID: {CommandId}", commandId);

                    // 如果启用了自动重连，将请求加入队列
                    if (_networkConfig.AutoReconnect && !_disposed)
                    {
                        _logger.Debug($"连接已断开，将命令{commandId.Name}加入队列等待发送");

                        // 创建任务完成源
                        var tcs = new TaskCompletionSource<bool>();

                        // 将请求加入队列
                        _queuedCommands.Enqueue(new QueuedCommand
                        {
                            CommandId = commandId,
                            Data = request,
                            CancellationToken = ct,
                            DataType = typeof(TRequest),
                            CompletionSource = tcs
                        });

                        // 在后台尝试重连
                        _ = Task.Run(() => TryReconnectAsync());

                        // 返回任务结果，让调用者等待连接恢复后发送
                        return await tcs.Task;
                    }
                    return false;
                }

                if (_disposed)
                    throw new ObjectDisposedException(nameof(ClientCommunicationService));

                // 使用现有的SendPacketCoreAsync发送请求
                await SendPacketCoreAsync<TRequest>(_socketClient, commandId, request, _networkConfig.DefaultRequestTimeoutMs, ct);

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

                // 检查是否因为连接断开导致的异常
                if (_networkConfig.AutoReconnect && !_disposed && IsRetryableException(ex))
                {
                    _logger.Debug($"因网络异常，将命令{commandId.Name}加入队列等待发送");

                    // 创建任务完成源
                    var tcs = new TaskCompletionSource<bool>();

                    // 将请求加入队列
                    _queuedCommands.Enqueue(new QueuedCommand
                    {
                        CommandId = commandId,
                        Data = request,
                        CancellationToken = ct,
                        DataType = typeof(TRequest),
                        CompletionSource = tcs
                    });

                    // 在后台尝试重连
                    _ = Task.Run(() => TryReconnectAsync());

                    // 返回任务结果，让调用者等待连接恢复后发送
                    return await tcs.Task;
                }
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
                // 只有在当前状态为连接时才处理断开连接
                if (_isConnected)
                {
                    // 使用统一的连接状态更新方法
                    UpdateConnectionState(false);

                    // 尝试重连
                    if (_networkConfig.AutoReconnect && !_disposed)
                    {
                        _logger.Debug("自动重连已启用，尝试重连服务器");
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
            _logger.Debug("客户端命令处理器已启动，开始监听服务器命令");
        }

        /// <summary>
        /// 当接收到服务器命令时触发
        /// 这个处理命令的过程，类型服务器处理。后面处理逻辑也是一样。只是在客户端而已。对于复杂的情况有用。
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        private async void OnCommandReceived(PacketModel packetModel, object data)
        {
            try
            {
                _logger.Debug("接收到服务器命令: {CommandId}", packetModel.CommandId);

                // 使用命令调度器处理命令
                await ProcessCommandAsync(packetModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理服务器命令时发生错误: {CommandId}", packetModel.CommandId);
                _eventManager.OnErrorOccurred(new Exception($"处理命令 {packetModel.CommandId} 时发生错误: {ex.Message}", ex));
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

                    // 清理定时器
                    _cleanupTimer?.Dispose();

                    // 清理超时统计
                    // _timeoutStatistics.TryDispose();

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


        /// <summary>
        /// 发送命令并处理响应，返回指令类型的响应数据
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>包含指令信息的响应数据</returns>
        public async Task<TResponse> SendCommandWithResponseAsync<TResponse>(
            CommandId commandId,
                    IRequest request,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TResponse : class, IResponse
        {
            try
            {
                // 检查连接状态
                if (!IsConnected)
                {
                    _logger.LogWarning("尝试发送带响应命令但连接已断开，命令ID: {CommandId}", commandId);

                    // 如果启用了自动重连，将请求加入队列
                    if (_networkConfig.AutoReconnect && !_disposed)
                    {
                        _logger.Debug($"连接已断开，将带响应命令{commandId.Name}加入队列等待发送");

                        // 创建任务完成源
                        var packetTcs = new TaskCompletionSource<PacketModel>();
                        var responseTcs = new TaskCompletionSource<TResponse>();

                        // 当packetTcs完成时，将结果转换为TResponse并设置到responseTcs
                        _ = packetTcs.Task.ContinueWith(task =>
                        {
                            if (task.IsCanceled)
                                responseTcs.TrySetCanceled();
                            else if (task.IsFaulted)
                                responseTcs.TrySetException(task.Exception);
                            else if (task.Result != null && task.Result.Response != null)
                                responseTcs.TrySetResult(task.Result.Response as TResponse);
                            else
                                responseTcs.TrySetResult(ResponseFactory.CreateSpecificErrorResponse<TResponse>("未收到有效响应数据"));
                        });

                        // 将请求加入队列
                        _queuedCommands.Enqueue(new QueuedCommand
                        {
                            CommandId = commandId,
                            Data = request,
                            CancellationToken = ct,
                            DataType = request.GetType(),
                            ResponseCompletionSource = packetTcs,
                            IsResponseCommand = true,
                            TimeoutMs = timeoutMs
                        });

                        // 在后台尝试重连
                        _ = Task.Run(() => TryReconnectAsync());

                        // 返回任务结果，让调用者等待连接恢复后发送
                        return await responseTcs.Task;
                    }

                    // 如果未启用自动重连，返回错误响应
                    return ResponseFactory.CreateSpecificErrorResponse<TResponse>("连接已断开，无法发送请求");
                }

                var packet = await SendCommandAsync(commandId, request, ct, timeoutMs);

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
                _eventManager.OnErrorOccurred(new Exception($"带响应命令发送失败: {ex.Message}", ex));

                // 检查是否因为连接断开导致的异常
                if (_networkConfig.AutoReconnect && !_disposed && IsRetryableException(ex))
                {
                    _logger.Debug($"因网络异常，将带响应命令{commandId.Name}加入队列等待发送");

                    // 创建任务完成源
                    var packetTcs = new TaskCompletionSource<PacketModel>();
                    var responseTcs = new TaskCompletionSource<TResponse>();

                    // 当packetTcs完成时，将结果转换为TResponse并设置到responseTcs
                    _ = packetTcs.Task.ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                            responseTcs.TrySetCanceled();
                        else if (task.IsFaulted)
                            responseTcs.TrySetException(task.Exception);
                        else if (task.Result != null && task.Result.Response != null)
                            responseTcs.TrySetResult(task.Result.Response as TResponse);
                        else
                            responseTcs.TrySetResult(ResponseBase.CreateError("未收到有效响应数据") as TResponse);
                    });

                    // 将请求加入队列
                    _queuedCommands.Enqueue(new QueuedCommand
                    {
                        CommandId = commandId,
                        Data = request,
                        CancellationToken = ct,
                        DataType = request.GetType(),
                        ResponseCompletionSource = packetTcs,
                        IsResponseCommand = true,
                        TimeoutMs = timeoutMs
                    });

                    // 在后台尝试重连
                    _ = Task.Run(() => TryReconnectAsync());

                    // 返回任务结果，让调用者等待连接恢复后发送
                    return await responseTcs.Task;
                }

                // 如果是操作取消异常，重新抛出
                if (ex is OperationCanceledException)
                {
                    throw;
                }

                // 返回错误响应
                return ResponseFactory.CreateSpecificErrorResponse<TResponse>($"命令执行失败: {ex.Message}");
            }
        }




    }

}