using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端通信服务实现类 - IClientCommunicationService 的具体实现
    /// 
    /// 设计目的：
    /// 1. 提供 IClientCommunicationService 接口的具体实现
    /// 2. 处理与服务器的具体通信逻辑
    /// 3. 作为业务层和底层Socket通信之间的桥梁
    /// 
    /// 职责说明：
    /// 1. 管理与服务器的连接状态
    /// 2. 发送和接收数据包
    /// 3. 处理命令请求和响应
    /// 4. 管理事件分发
    /// 
    /// 使用说明：
    /// - 此类通过依赖注入容器自动注册和解析
    /// - 业务层应通过 IClientCommunicationService 接口使用此类
    /// - 不要直接实例化此类，应通过依赖注入获取实例
    /// 
    /// 分层架构：
    /// 业务层 (UserLoginService 等) 
    ///     → 接口层 (IClientCommunicationService)
    ///     → 实现层 (ClientCommunicationService)
    ///     → 传输层 (ISocketClient/SuperSocketClient)
    /// 
    /// 关联组件：
    /// - 依赖 ISocketClient 进行底层Socket通信
    /// - 使用 ClientCommandDispatcher 进行命令分发
    /// - 使用 RequestResponseManager 处理请求响应
    /// - 使用 ClientEventManager 管理事件
    /// </summary>
    public class ClientCommunicationService : IClientCommunicationService
    {
        private readonly ISocketClient _socketClient;
        private readonly ClientCommandDispatcher _commandDispatcher;
        private readonly ClientCommandFactory _commandFactory;
        private readonly RequestResponseManager _requestResponseManager;
        private readonly ClientEventManager _eventManager;
        private bool _isConnected;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socketClient">Socket客户端</param>
        /// <param name="commandDispatcher">客户端命令调度器</param>
        public ClientCommunicationService(
            ISocketClient socketClient,
            ClientCommandDispatcher commandDispatcher)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _commandFactory = new ClientCommandFactory(_commandDispatcher);
            _requestResponseManager = new RequestResponseManager();
            _eventManager = new ClientEventManager();

            // 注册响应处理事件
            _socketClient.Received += OnReceived;
            _socketClient.Closed += OnClosed;
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// 当接收到服务器命令时触发的事件
        /// </summary>
        public event Action<CommandId, object> CommandReceived
        {
            add { _eventManager.CommandReceived += value; }
            remove { _eventManager.CommandReceived -= value; }
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="serverUrl">服务器地址</param>
        /// <param name="port">端口号</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>连接是否成功</returns>
        public async Task<bool> ConnectAsync(string serverUrl, int port, CancellationToken cancellationToken = default)
        {
            if (_isConnected)
            {
                return true;
            }

            try
            {
                // 直接使用_socketClient的连接结果，它已经处理了状态同步
                _isConnected = await _socketClient.ConnectAsync(serverUrl, port, cancellationToken);
                _eventManager.OnConnectionStatusChanged(_isConnected);
                return _isConnected;
            }
            catch (Exception ex)
            {
                // 记录连接失败异常
                _eventManager.OnErrorOccurred(new Exception($"连接服务器失败: {ex.Message}", ex));
                _isConnected = false;
                return false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (_isConnected)
            {
                _socketClient.Disconnect();
                _isConnected = false;
                _eventManager.OnConnectionStatusChanged(_isConnected);
            }
        }

        /// <summary>
        /// 发送命令并等待响应
        /// </summary>
        public async Task<ApiResponse<TResponse>> SendCommandAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken cancellationToken = default,
            int timeoutMs = 30000)
        {
            if (!_isConnected)
            {
                _eventManager.OnErrorOccurred(new Exception("未连接到服务器"));
                return ApiResponse<TResponse>.Failure("未连接到服务器");
            }

            try
            {
                return await _requestResponseManager.SendRequestAsync<TRequest, ApiResponse<TResponse>>(
                    _socketClient,
                    commandId,
                    requestData,
                    cancellationToken,
                    timeoutMs);
            }
            catch (TimeoutException ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"请求超时: {commandId}", ex));
                return ApiResponse<TResponse>.Failure("请求超时");
            }
            catch (OperationCanceledException ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"请求被取消: {commandId}", ex));
                return ApiResponse<TResponse>.Failure("请求被取消");
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"发送命令失败: {commandId}, {ex.Message}", ex));
                return ApiResponse<TResponse>.Failure($"发送命令失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送命令并等待响应（使用命令对象）
        /// </summary>
        public async Task<ApiResponse<TResponse>> SendCommandAsync<TResponse>(
            ICommand command,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                _eventManager.OnErrorOccurred(new ArgumentNullException(nameof(command)));
                throw new ArgumentNullException(nameof(command));
            }

            // 从命令对象中提取数据
            object requestData = null;
            var timeoutMs = command.TimeoutMs;

            // 如果命令是BaseCommand类型，可以获取可序列化的数据
            requestData = command.GetSerializableData();

            // 调用通用方法
            return await SendCommandAsync<object, TResponse>(
                command.CommandIdentifier,
                requestData,
                cancellationToken,
                timeoutMs);
        }

        /// <summary>
        /// 发送单向命令（不等待响应）
        /// </summary>
        public async Task<bool> SendOneWayCommandAsync<TRequest>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken cancellationToken = default)
        {
            if (!_isConnected)
            {
                _eventManager.OnErrorOccurred(new Exception("未连接到服务器"));
                return false;
            }

            try
            {
                // 创建数据包
                var packet = PacketBuilder.Create()
                    .WithCommand(commandId)
                    .WithJsonData(requestData)
                    .WithResponseRequired(false)
                    .WithDirection(PacketDirection.ClientToServer)
                    .Build();

                // 序列化
                byte[] payload;
                try
                {
                    payload = UnifiedSerializationService.SerializeWithMessagePack(packet);
                }
                catch (Exception ex)
                {
                    _eventManager.OnErrorOccurred(new Exception($"序列化数据包失败: {ex.Message}", ex));
                    return false;
                }

                // 分解CommandId
                byte category = (byte)commandId.Category;
                byte operationCode = commandId.OperationCode;

                var original = new OriginalData(category, new byte[] { operationCode }, payload);
                byte[] encryptedData = PacketSpec.Security.EncryptedProtocol.EncryptClientPackToServer(original);

                // 发送数据
                await _socketClient.SendAsync(encryptedData, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"发送单向命令失败: {commandId}, {ex.Message}", ex));
                return false;
            }
        }

        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        private void OnReceived(byte[] data)
        {
            try
            {
                // 让请求响应管理器先处理响应数据
                _requestResponseManager.HandleResponse(data);

                // 解密数据
                var decryptedData = PacketSpec.Security.EncryptedProtocol.DecryptServerPack(data);

                // 反序列化数据包
                PacketModel packet;
                try
                {
                    packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(decryptedData.Two);
                }
                catch (Exception ex)
                {
                    _eventManager.OnErrorOccurred(new Exception($"反序列化数据包失败: {ex.Message}", ex));
                    return;
                }

                // 如果没有请求ID，或者对应的任务已完成，则视为服务器主动推送的命令
                if (packet != null&& !packet.Extensions.ContainsKey("RequestId"))
                {
                    // 提取命令ID和数据
                    if (packet.Command != null)
                    {
                        object commandData = null;
                        try
                        {
                            // 尝试解析命令数据
                            commandData = packet.GetJsonData<object>();
                        }
                        catch (Exception ex)
                        {
                            _eventManager.OnErrorOccurred(new Exception($"解析命令数据失败: {ex.Message}", ex));
                        }

                        // 触发命令接收事件
                        _eventManager.OnCommandReceived(packet.Command, commandData);
                    }
                }
            }
            catch (Exception ex)
            {
                _eventManager.OnErrorOccurred(new Exception($"处理接收到的数据失败: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 处理连接关闭事件
        /// </summary>
        private void OnClosed(EventArgs e)
        {
            _isConnected = false;
            _eventManager.OnConnectionClosed();
            _eventManager.OnConnectionStatusChanged(_isConnected);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _socketClient.Received -= OnReceived;
            _socketClient.Closed -= OnClosed;
            _socketClient.Dispose();
            _requestResponseManager.Dispose();
            _isConnected = false;
        }
    }
}