using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Network
{
/// <summary>
    /// 客户端通信与命令处理服务 - 业务层通信统一入口与命令处理核心
    /// 
    /// 🔄 完整通信流程：
    /// 发送方向：
    /// 1. 接收业务层命令请求
    /// 2. 序列化命令数据
    /// 3. 通过 CommunicationManager 发送数据
    /// 4. 等待响应或处理超时
    /// 5. 返回业务执行结果
    /// 
    /// 接收方向：
    /// 1. 订阅 ClientEventManager.CommandReceived 事件
    /// 2. 接收服务器命令ID和业务数据
    /// 3. 使用 ClientCommandDispatcher 查找对应处理器
    /// 4. 创建命令实例并初始化
    /// 5. 执行具体业务逻辑
    /// 6. 处理执行结果和异常
    /// 
    /// 📋 核心职责：
    /// - 业务层通信接口（发送）
    /// - 服务器命令处理（接收）
    /// - 请求-响应生命周期管理
    /// - 命令生命周期管理（创建→初始化→执行→清理）
    /// - 异步操作支持
    /// - 超时控制
    /// - 错误处理与重试
    /// - 性能监控与日志
    /// - 线程安全管理
    /// 
    /// 🔗 与架构集成：
    /// - 为业务层提供统一通信接口
    /// - 使用 CommunicationManager 进行网络通信
    /// - 接收并处理服务器主动推送的命令
    /// - 协调请求-响应匹配
    /// - 处理业务层超时和重试
    /// 
    /// 💡 使用场景：
    /// - UserLoginService 等具体业务服务
    /// - 需要与服务器通信的所有业务组件
    /// - 需要请求-响应模式的业务操作
    /// - 需要处理服务器主动推送命令的场景
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
        // 日志记录器
        private readonly ILogger<ClientCommunicationService> _logger;
        // 心跳管理器
        private readonly HeartbeatManager _heartbeatManager;
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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socketClient">Socket客户端接口，提供底层网络通信能力</param>
        /// <param name="commandDispatcher">命令调度器，用于分发命令到对应的处理类</param>
        /// <param name="requestResponseManager">请求-响应管理器，处理请求和响应的匹配</param>
        /// <param name="clientEventManager">客户端事件管理器，管理连接状态和命令接收事件</param>
        /// <param name="heartbeatManager">心跳管理器，负责定期发送心跳包</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
        public ClientCommunicationService(
            ISocketClient socketClient,
            ICommandDispatcher commandDispatcher,
            RequestResponseManager requestResponseManager,
            ClientEventManager clientEventManager,
            HeartbeatManager heartbeatManager,
            ILogger<ClientCommunicationService> logger)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _rrManager = requestResponseManager ?? throw new ArgumentNullException(nameof(requestResponseManager));
            _eventManager = clientEventManager ?? throw new ArgumentNullException(nameof(clientEventManager));
            _heartbeatManager = heartbeatManager ?? throw new ArgumentNullException(nameof(heartbeatManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // 注册事件处理程序
            _socketClient.Received += OnReceived;
            _socketClient.Closed += OnClosed;
            _heartbeatManager.OnHeartbeatFailed += OnHeartbeatFailed;

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
            
            if (timeoutMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutMs), "超时时间必须大于0");

            return _rrManager.SendRequestAsync<TRequest, ApiResponse<TResponse>>(
                _socketClient, commandId, requestData, ct, timeoutMs);
        }

        /// <summary>
        /// 异步发送命令对象到服务器并等待响应
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>包含响应数据的ApiResponse对象</returns>
        public Task<ApiResponse<TResponse>> SendCommandAsync<TResponse>(
            ICommand command,
            CancellationToken ct = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return SendCommandAsync<object, TResponse>(
                command.CommandIdentifier,
                command.GetSerializableData(),
                ct,
                command.TimeoutMs > 0 ? command.TimeoutMs : 30000);
        }

        /// <summary>
        /// 异步发送单向命令到服务器（不等待响应）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送成功返回true，失败返回false</returns>
        public Task<bool> SendOneWayCommandAsync<TRequest>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken ct = default)
        {
            if (!Enum.IsDefined(typeof(CommandCategory), commandId.Category))
                throw new ArgumentException($"无效的命令类别: {commandId.Category}", nameof(commandId));

            return SafeSendOneWayAsync(commandId, requestData, ct);
        }

        /// <summary>
        /// 重新连接到服务器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>重连是否成功</returns>
        public async Task<bool> ReconnectAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_serverAddress) || _serverPort <= 0)
            {
                return false;
            }

            // 断开当前连接
            Disconnect();
            
            // 重新连接
            return await ConnectAsync(_serverAddress, _serverPort, cancellationToken);
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
        /// <param name="disposing">是否由Dispose方法调用</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // 取消订阅命令事件
                if (_eventManager != null)
                {
                    _eventManager.CommandReceived -= OnCommandReceived;
                }

                // 移除事件处理程序
                _socketClient.Received -= OnReceived;
                _socketClient.Closed -= OnClosed;
                
                // 断开连接
                try
                {
                    Disconnect();
                }
                catch { }
                
                // 释放托管资源
                _socketClient?.Dispose();
                _rrManager?.Dispose();
            }

            _disposed = true;
        }

        /* -------------------- 命令处理方法 -------------------- */

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
                await ProcessCommandAsync(commandId, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理服务器命令时发生错误: {CommandId}", commandId);
                _eventManager.OnErrorOccurred(new Exception($"处理命令 {commandId} 时发生错误: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 处理服务器命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        private async Task ProcessCommandAsync(CommandId commandId, object data)
        {
            try
            {
                // 创建命令实例
                var command = _commandDispatcher.CreateCommand(commandId);
                if (command == null)
                {
                    _logger.LogWarning("无法创建命令实例: {CommandId}", commandId);
                    throw new InvalidOperationException($"无法创建命令实例: {commandId}");
                }

                // 设置命令属性
                if (data != null)
                {
                    await InitializeCommandAsync(command, data);
                }

                // 执行命令
                _logger.LogInformation("开始执行命令: {CommandId}", commandId);
                var result = await command.ExecuteAsync();
                
                _logger.LogInformation("命令执行完成: {CommandId}, 结果: {Result}", commandId, result);
                
                // 无需清理命令，由命令调度器自行管理命令实例
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "命令执行失败: {CommandId}", commandId);
                throw new Exception($"命令 {commandId} 执行失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="command">命令实例</param>
        /// <param name="data">命令数据</param>
        private async Task InitializeCommandAsync(ICommand command, object data)
        {
            try
            {
                if (data != null)
                {
                    // 简化命令初始化
                        if (data is PacketModel packetModel)
                        {
                            // 使用BaseCommand类型和PacketModel.Body属性
                            if (command is RUINORERP.PacketSpec.Commands.BaseCommand baseCommand)
                            {
                                // 设置PacketModel对象到命令的Packet属性
                                baseCommand.Packet = packetModel;
                            }
                        }
                        else if (data is byte[] byteData)
                        {
                            // 如果是字节数据，反序列化为PacketModel后处理
                            var packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(byteData);
                            if (packet != null && command is RUINORERP.PacketSpec.Commands.BaseCommand baseCommand)
                            {
                                baseCommand.Packet = packet;
                            }
                        }
                        else if (command is RUINORERP.PacketSpec.Commands.BaseCommand baseCommand)
                        {
                            // 如果有其他数据类型，创建新的PacketModel并设置Body
                            var packet = new RUINORERP.PacketSpec.Models.Core.PacketModel();
                            if (data != null)
                            {
                                // 将数据转换为JSON字符串后设置为Body
                                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                                packet.SetJsonData(jsonData);
                            }
                            baseCommand.Packet = packet;
                        }
                }

                _logger.LogDebug("命令初始化完成: {CommandType}", command.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "命令初始化失败: {CommandType}", command.GetType().Name);
                throw new Exception($"命令 {command.GetType().Name} 初始化失败: {ex.Message}", ex);
            }
        }

        /* -------------------- 私有方法 -------------------- */

        /// <summary>
        /// 安全地异步连接到服务器（包含异常处理）
        /// </summary>
        /// <param name="serverUrl">服务器URL</param>
        /// <param name="port">服务器端口</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>连接成功返回true，失败返回false</returns>
        private async Task<bool> SafeConnectAsync(string serverUrl, int port, CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();
                
                lock (_syncLock)
                {
                    if (_disposed)
                        throw new ObjectDisposedException(nameof(ClientCommunicationService));
                }

                _isConnected = await _socketClient.ConnectAsync(serverUrl, port, ct);
                if (_isConnected)
                {
                    _serverAddress = serverUrl;
                    _serverPort = port;
                }
                _eventManager.OnConnectionStatusChanged(_isConnected);
                return _isConnected;
            }
            catch (OperationCanceledException)
            {
                _eventManager.OnErrorOccurred(new Exception("连接操作被取消"));
                _isConnected = false;
                return false;
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"连接服务器失败: {ex.Message}", ex));
                _isConnected = false;
                return false;
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

                // 序列化请求数据
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
        /// 处理接收到的数据
        /// </summary>
        /// <param name="data">接收到的原始数据</param>
        private void OnReceived(byte[] data)
        {
            try
            {
                if (data == null || data.Length == 0)
                    return;

                // 首先尝试将数据作为响应处理
                bool isResponse = _rrManager.HandleResponse(data);
                
                // 如果不是响应，则尝试作为命令处理
                if (!isResponse)
                {
                    // 解密服务器数据包
                    var decrypted = EncryptedProtocol.DecryptServerPack(data);
                    if (decrypted.Two == null)
                        return;

                    // 反序列化数据包
                    var packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(decrypted.Two);
                    if (packet?.Command != null && !packet.Extensions.ContainsKey("RequestId"))
                    {
                        // 触发命令接收事件
                        _eventManager.OnCommandReceived(packet.Command, packet.GetJsonData<object>());
                    }
                }
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"接收数据处理失败: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 处理连接关闭事件
        /// </summary>
        /// <param name="e">事件参数</param>
        private void OnClosed(EventArgs e)
        {
            lock (_syncLock)
            {
                _isConnected = false;
            }
            
            _eventManager.OnConnectionClosed();
            _eventManager.OnConnectionStatusChanged(false);
        }
        
        /// <summary>
        /// 处理心跳失败事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnHeartbeatFailed(object sender, EventArgs e)
        {
            _logger.LogWarning("心跳失败，检查连接状态");
            
            // 如果心跳连续失败，考虑断开连接并尝试重连
            if (_socketClient != null && !_socketClient.IsConnected)
            {
                _logger.LogError("心跳失败，连接已断开，尝试重连");
                Task.Run(async () =>
                {
                    try
                    {
                        await ReconnectAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "重连过程中发生错误");
                    }
                });
            }
        }
    }
}