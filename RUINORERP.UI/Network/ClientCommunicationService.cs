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
using Fireasy.Common.Extensions;
using RUINORERP.UI.Network.Exceptions;
using FastReport.DevComponents.DotNetBar;
using RUINORERP.Common.Extensions;
using System.Linq;
using System.Reflection;
using System.Text;
using ICommand = RUINORERP.PacketSpec.Commands.ICommand;
using MessagePack;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 优化后的客户端通信与命令处理服务 - 统一网络通信核心组件
    /// </summary>
    public class ClientCommunicationService : IDisposable
    {
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
        private readonly CommandPacketAdapter commandPacketAdapter;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

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
        /// <param name="commandDispatcher">命令调度器，用于分发命令到对应的处理类</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="commandTypeHelper">命令类型助手，用于管理命令类型映射关系</param>
        /// <param name="networkConfig">网络配置</param>
        /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
        public ClientCommunicationService(
            ISocketClient socketClient,
            CommandPacketAdapter _commandPacketAdapter,
        ICommandDispatcher commandDispatcher,
            ILogger<ClientCommunicationService> logger,
            TokenManager _tokenManager,
            NetworkConfig networkConfig = null)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            commandPacketAdapter = _commandPacketAdapter;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _networkConfig = networkConfig ?? NetworkConfig.Default;
            _eventManager = new ClientEventManager();
            tokenManager = _tokenManager;
            // 初始化请求响应管理相关组件
            _timeoutStatistics = new TimeoutStatisticsManager();
            _errorHandlingStrategyFactory = new ErrorHandlingStrategyFactory();

            #region  扫描注册
            // 获取PacketSpec程序集
            var packetSpecAssembly = Assembly.GetAssembly(typeof(RUINORERP.PacketSpec.Commands.ICommand));
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
                    _logger.LogInformation("客户端已连接到服务器");
                    
                    // 启动心跳
                    if (!_heartbeatIsRunning)
                    {
                        _heartbeatManager.Start();
                        _heartbeatIsRunning = true;
                    }
                }
                else
                {
                    _logger.LogInformation("客户端与服务器断开连接");
                    
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
        /// 重连失败事件，当客户端重连服务器失败时触发
        /// </summary>
        public event Action ReconnectFailed
        {
            add => _eventManager.ReconnectFailed += value;
            remove => _eventManager.ReconnectFailed -= value;
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
                        _socketClient.Disconnect();
                    }
                    catch (Exception ex)
                    {
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
        /// 发送请求并等待响应（合并自RequestResponseManager）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>响应数据对象</returns>
        private async Task<PacketModel> SendRequestAsync<TRequest, TResponse>(
            BaseCommand<TRequest, TResponse> command,
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
                CommandId = command.ToString()
            };

            if (command.Request == null)
            {
                throw new InvalidOperationException($"请求数据不能为空，指令名称: {command.CommandIdentifier.Name}");
            }

            if (!_pendingRequests.TryAdd(command.Request.RequestId, pendingRequest))
            {
                throw new InvalidOperationException($"无法添加请求到待处理列表，请求ID: {command.Request.RequestId}");
            }

            try
            {

                // 使用现有的SendPacketCoreAsync发送请求
                await SendPacketCoreAsync(_socketClient, command, command.Request.RequestId, _networkConfig.DefaultRequestTimeoutMs, ct);

                // 等待响应或超时
                var timeoutTask = Task.Delay(timeoutMs, cts.Token);
                var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    _logger?.LogError("请求超时，请求ID: {RequestId}", command.Request.RequestId);
                    _timeoutStatistics.RecordTimeout(command.ToString(), timeoutMs);
                    throw new TimeoutException($"请求超时（{timeoutMs}ms），请求ID: {command.Request.RequestId}");
                }

                ct.ThrowIfCancellationRequested();

                var responsePacket = await tcs.Task;

                if (responsePacket != null)
                {

                }
                PacketModel packet = null;


                if (responsePacket is PacketModel)
                {
                    packet = responsePacket;

                }
                _logger?.LogDebug("成功接收响应，请求ID: {RequestId}", command.Request.RequestId);
                return packet;
            }
            catch (Exception ex) when (!(ex is TimeoutException) && !(ex is OperationCanceledException))
            {
                _logger?.LogError(ex, "请求处理失败，请求ID: {RequestId}", command.Request.RequestId);
                throw new InvalidOperationException($"请求处理失败，请求ID: {command.Request.RequestId}: {ex.Message}", ex);
            }
            finally
            {
                _pendingRequests.TryRemove(command.Request.RequestId, out _);
            }
        }


        #region 设置token

        /// <summary>
        /// 自动附加认证Token - 优化版
        /// 增强功能：确保Token的完整性、类型设置、ExecutionContext绑定和异常处理
        /// </summary>
        protected virtual async void AutoAttachToken(CmdContext ExecutionContext)
        {
            try
            {
                // 检查TokenManager是否可用
                if (tokenManager == null)
                {
                    return;
                }
                // 简化版：使用依赖注入的TokenManager
                var tokenInfo = await tokenManager.TokenStorage.GetTokenAsync();
                if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.AccessToken))
                {

                    // 自动设置到ExecutionContext，确保服务器端也能获取
                    if (ExecutionContext == null)
                        ExecutionContext = new CmdContext();
                    ExecutionContext.Token = tokenInfo;
                }

            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "自动附加Token失败");
            }
        }

        #endregion


        /// <summary>
        /// 处理接收到的响应数据包
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>是否是响应包</returns>
        private bool HandleResponse(PacketModel packet)
        {
            try
            {
                if (!string.IsNullOrEmpty(packet?.ExecutionContext.RequestId))
                {
                    var requestId = packet?.ExecutionContext.RequestId;
                    if (!string.IsNullOrEmpty(requestId) &&
                        _pendingRequests.TryRemove(requestId, out var pendingRequest))
                    {
                        return pendingRequest.Tcs.TrySetResult(packet);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理响应包时发生错误");
                return false;
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
                _logger?.LogInformation("清理了 {RemovedCount} 个超时请求", removedCount);
            }
        }

        /// <summary>
        /// 发送请求并等待响应，支持重试策略（合并自RequestResponseManager）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="retryStrategy">重试策略，如果为null则使用默认策略</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">单次请求超时时间（毫秒），默认30000毫秒</param>
        /// <returns>包含响应数据的ApiResponse对象</returns>
        /// <exception cref="ArgumentException">当命令类别无效时抛出</exception>
        /// <exception cref="ArgumentOutOfRangeException">当超时时间小于等于0时抛出</exception>
        public async Task<PacketModel> SendRequestWithRetryAsync<TRequest, TResponse>(
            BaseCommand<TRequest, TResponse> command,
            IRetryStrategy retryStrategy = null,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            if (!Enum.IsDefined(typeof(CommandCategory), command.CommandIdentifier.Category))
                throw new ArgumentException($"无效的命令类别: {command.CommandIdentifier.Category}", nameof(command.CommandIdentifier));

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

                    var response = await SendRequestAsync<TRequest, TResponse>(command, ct, timeoutMs);

                    _logger?.LogDebug("请求成功，尝试次数: {Attempt}", attempt);
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
                        _logger?.LogInformation("等待 {DelayMs}ms 后进行第 {NextAttempt} 次重试",
                            delay, attempt + 1);
                        await Task.Delay(delay, ct);
                    }
                }
            }

            throw new InvalidOperationException($"请求失败（尝试 {attempt} 次），错误: {lastException?.Message}", lastException);
        }



        /// <summary>
        /// 异步发送命令到服务器并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒），默认为30000毫秒</param>
        /// <returns>TResponse</returns>
        public async Task<PacketModel> SendCommandAsync<TRequest, ResponseBase>(
            BaseCommand<TRequest, ResponseBase> command,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TRequest : class, IRequest
            where ResponseBase : class, IResponse
        {
            if (!Enum.IsDefined(typeof(CommandCategory), command.CommandIdentifier.Category))
                throw new ArgumentException($"无效的命令类别: {command.CommandIdentifier.Category}", nameof(command.CommandIdentifier));

            return await EnsureConnectedAsync<PacketModel>(async () =>
            {
                try
                {
                    // BaseCommand会自动处理Token管理，包括获取和刷新Token
                    return await SendRequestAsync<TRequest, ResponseBase>(command, ct, timeoutMs);
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
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送成功返回true，失败返回false</returns>
        public async Task<bool> SendOneWayCommandAsync<TRequest>(
            BaseCommand<TRequest, IResponse> command,
            CancellationToken ct = default)
            where TRequest : class, IRequest
        {
            if (!Enum.IsDefined(typeof(CommandCategory), command.CommandIdentifier.Category))
                throw new ArgumentException($"无效的命令类别: {command.CommandIdentifier.Category}", nameof(command.CommandIdentifier));

            return await EnsureConnectedAsync<bool>(async () =>
            {
                try
                {
                    // 创建命令对象并设置Token
                    // var command = InitializeCommandAsync(commandId, requestData);

                    // 生成请求ID但不等待响应
                    string requestId = IdGenerator.GenerateRequestId(command.CommandIdentifier);

                    // 使用内部的SendPacketCoreAsync发送单向命令，确保Token正确附加
                    await SendPacketCoreAsync(_socketClient, command, command.Request.RequestId, _networkConfig.DefaultRequestTimeoutMs, ct);

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "发送单向命令失败: {CommandId}", command.CommandIdentifier.FullCode);
                    _eventManager.OnErrorOccurred(ex);
                    return false;
                }
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
                if (_networkConfig.AutoReconnect && !_isConnected)
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
                if (_networkConfig.AutoReconnect && !_isConnected)
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
        private async Task<GenericCommand<TData>> InitializeCommandAsync<TData>(CommandId commandId, TData data)
        {
            var command = new GenericCommand<TData>(commandId, data);
            command.TimeoutMs = _networkConfig.DefaultRequestTimeoutMs; // 使用网络配置中的默认超时时间
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
        /// 尝试重连到服务器
        /// </summary>
        /// <returns>重连成功返回true，失败返回false</returns>
        private async Task<bool> TryReconnectAsync()
        {
            if (!_networkConfig.AutoReconnect || _disposed || string.IsNullOrEmpty(_serverAddress))
                return false;

            _logger.LogInformation("开始尝试重连服务器...");

            for (int attempt = 0; attempt < _networkConfig.MaxReconnectAttempts; attempt++)
            {
                if (_disposed)
                    break;

                _logger.LogInformation($"重连尝试 {attempt + 1}/{_networkConfig.MaxReconnectAttempts}");

                try
                {
                    if (await _socketClient.ConnectAsync(_serverAddress, _serverPort, CancellationToken.None).ConfigureAwait(false))
                    {
                        lock (_syncLock)
                        {
                            _heartbeatFailureCount = 0;
                            // 使用统一的连接状态更新方法
                            UpdateConnectionState(true);
                        }

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
                if (attempt < _networkConfig.MaxReconnectAttempts - 1)
                {
                    _logger.LogInformation($"等待 {_networkConfig.ReconnectDelay.TotalSeconds} 秒后进行下一次重连");
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
        /// 处理心跳包失败
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

                    // 断开连接
                    if (_isConnected)
                    {
                        _socketClient.Disconnect();
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
                    if (HandleResponse(packet))
                    {
                        //这里直接返回，实际会到具体的服务中响应处理了
                        return; // 如果是响应，处理完成，不再作为命令处理
                    }

                    // 如果不是响应，再作为命令处理
                    if (packet.IsValid() && packet.CommandId.FullCode > 0)
                    {
                        // 优先使用事件机制处理命令，避免重复处理
                        _eventManager.OnCommandReceived(packet, packet.CommandData);

                        // 如果事件机制没有处理该命令（没有订阅者），则直接处理
                        if (!_eventManager.HasCommandSubscribers(packet))
                        {
                            await ProcessCommandAsync(packet);
                        }
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
        private async Task ProcessCommandAsync(PacketModel Packet)
        {
            try
            {
                // 创建一个临时的命令对象用于调度
                var command = new GenericCommand<object>(Packet.CommandId, Packet.CommandData);

                // 根据命令类别进行特殊处理
                if (Packet.CommandId.Category == CommandCategory.System)
                {
                    // 处理系统命令，如心跳响应等
                    if (Packet.CommandId.FullCode == PacketSpec.Commands.System.SystemCommands.HeartbeatResponse.FullCode)
                    {
                        // 处理心跳响应，重置失败计数
                        lock (_syncLock)
                        {
                            _heartbeatFailureCount = 0;
                        }
                    }
                }

                // 调度命令到命令处理器
                await _commandDispatcher.DispatchAsync(Packet, command, CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"处理命令 {Packet.CommandId} 时发生错误: {ex.Message}", ex));
                _logger.LogError(ex, "处理命令时发生错误");
            }
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
        private async Task SendPacketCoreAsync<TRequest, TResponse>(
            ISocketClient client,
            BaseCommand<TRequest, TResponse> command,
            string requestId,
            int timeoutMs,
            CancellationToken ct,
            string authToken = null)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                // 构建数据包
                var packet = PacketBuilder.Create()
                    .WithCommand(command.CommandIdentifier)
                    .WithMessagePackData(command)
                    .WithRequestId(command.Request.RequestId)
                    .WithTimeout(timeoutMs)
                    .Build();

                // 自动设置到ExecutionContext，确保服务器端也能获取
                if (packet.ExecutionContext == null)
                    packet.ExecutionContext = new CmdContext();

                packet.ExecutionContext.RequestId = command.Request.RequestId;
                packet.ExecutionContext.RequestType = command.Request.GetType();
                packet.ExecutionContext.CommandType = command.GetType();
                packet.ExecutionContext.ClientVersion =Application.ProductVersion;
                packet.ClientId = client.ClientID;

                // 序列化和加密数据包
                var payload = UnifiedSerializationService.SerializeWithMessagePack<PacketModel>(packet);
                var original = new OriginalData(
                    (byte)command.CommandIdentifier.Category,
                    new[] { command.CommandIdentifier.OperationCode },
                    payload);
                var encrypted = EncryptedProtocol.EncryptClientPackToServer(original);

                await client.SendAsync(encrypted, ct);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"发送数据包时发生错误: CommandId={command.CommandIdentifier}, RequestId={command.Request.RequestId}",
                    command.CommandIdentifier.FullCode, requestId);

                // 如果是取消操作，则直接抛出
                if (ex is OperationCanceledException)
                {
                    throw;
                }

                // 包装异常以便上层处理（包括可能的Token过期处理）
                throw new NetworkCommunicationException(
                    $"发送请求失败: {ex.Message}",
                    ex,
                    command.CommandIdentifier,
                    requestId);
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
        private async Task<bool> SendOneWayCommandAsync<TRequest>(CommandId commandId, TRequest data, CancellationToken ct)
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
                    // 使用统一的连接状态更新方法
                    UpdateConnectionState(false);

                    // 尝试重连
                    if (_networkConfig.AutoReconnect && !_disposed)
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
        /// 这个处理命令的过程，类型服务器处理。后面处理逻辑也是一样。只是在客户端而已。对于复杂的情况有用。
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        private async void OnCommandReceived(PacketModel packetModel, object data)
        {
            try
            {
                _logger.LogInformation("接收到服务器命令: {CommandId}", packetModel.CommandId);

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
                    _timeoutStatistics.TryDispose();

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
        /// 处理命令响应并提取业务数据
        /// 将重复的响应处理逻辑提取到通信服务层
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="packet">接收到的数据包</param>
        /// <param name="commandPacketAdapter">命令包适配器</param>
        /// <returns>处理后的响应数据</returns>
        public TResponse ProcessCommandResponseAsync<TResponse>(PacketModel packet, CommandPacketAdapter commandPacketAdapter)
            where TResponse : class, IResponse
        {
            if (packet == null)
            {
                _logger.LogWarning("接收到的数据包为空");
                return null;
            }

            try
            {
                // 创建命令对象 - 使用ExecutionContext中的响应类型进行反序列化
                ICommand baseCommand = commandPacketAdapter.CreateCommandFromBytes(packet.CommandData, packet.ExecutionContext?.CommandTypeName);

                // 处理登录命令的特殊情况
                TResponse response = default(TResponse);
                if (packet.ExecutionContext.ResponseType != null)
                {
                    // 反序列化响应数据
                    var responseData = MessagePackSerializer.Deserialize(packet.ExecutionContext.ResponseType, baseCommand.ResponseDataByMessagePack, UnifiedSerializationService.MessagePackOptions);
                    response = responseData as TResponse;
                }
                return response;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理命令响应时发生错误");
                throw new InvalidOperationException($"处理命令响应失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 发送命令并处理响应，返回指令类型的响应数据
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="commandPacketAdapter">命令包适配器</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>包含指令信息的响应数据</returns>
        public async Task<BaseCommand<TResponse>> SendCommandWithResponseAsync<TRequest, TResponse>(
            BaseCommand<TRequest, TResponse> command,
            CommandPacketAdapter commandPacketAdapter,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            var packet = await SendCommandAsync<TRequest, TResponse>(command, ct, timeoutMs);

            if (packet == null)
            {
                return BaseCommand<TResponse>.Error("未收到服务器响应", 408);
            }

            var responseData = ProcessCommandResponseAsync<TResponse>(packet, commandPacketAdapter);

            var commandResponse = BaseCommand<TResponse>.Success(responseData, (responseData as ResponseBase)?.Message ?? "操作成功");
            commandResponse.CommandId = command.CommandIdentifier;
            commandResponse.RequestId = command.Request?.RequestId;
            commandResponse.ExecutionContext = packet.ExecutionContext;

            return commandResponse;
        }

    }

}