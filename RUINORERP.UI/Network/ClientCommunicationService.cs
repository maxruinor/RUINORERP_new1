using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端通信服务实现类
    /// 提供统一的客户端与服务器通信接口
    /// </summary>
    public class ClientCommunicationService : IClientCommunicationService
    {
        private readonly ISocketClient _socketClient;
        private readonly IEncryptionService _encryptionService;
        private readonly ICommandFactory _commandFactory;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<byte[]>> _pendingResponses;
        private bool _isConnected;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socketClient">Socket客户端</param>
        /// <param name="encryptionService">加密服务</param>
        /// <param name="commandFactory">命令工厂</param>
        public ClientCommunicationService(
            ISocketClient socketClient,
            IEncryptionService encryptionService,
            ICommandFactory commandFactory)
        {
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _encryptionService = encryptionService ?? new EncryptedProtocolV2Adapter();
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            _pendingResponses = new ConcurrentDictionary<string, TaskCompletionSource<byte[]>>();

            // 注册响应处理事件
            _socketClient.Received += OnReceived;
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// 当接收到服务器命令时触发的事件
        /// </summary>
        public event Action<CommandId, object> CommandReceived;

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
                _isConnected = await _socketClient.ConnectAsync(serverUrl, port, cancellationToken);
                return _isConnected;
            }
            catch (Exception ex)
            {
                // 记录连接失败异常
                Console.WriteLine($"连接服务器失败: {ex.Message}");
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
                return ApiResponse<TResponse>.Failure("未连接到服务器");
            }

            try
            {
                // 创建请求ID
                string requestId = Guid.NewGuid().ToString("N");
                
                // 创建完成任务源
                var tcs = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
                _pendingResponses.TryAdd(requestId, tcs);

                // 创建和构建数据包
                var packet = PacketBuilder.Create()
                    .WithCommand(commandId)
                    .WithJsonData(requestData)
                    .WithRequestId(requestId)
                    .WithTimeout(timeoutMs)
                    .WithDirection(PacketDirection.ClientToServer)
                    .Build();

                // 序列化并加密数据
                byte[] payload = UnifiedSerializationService.SerializeWithMessagePack(packet);
                
                // 分解CommandId为Category和OperationCode
                byte category = (byte)commandId.Category;
                byte operationCode = commandId.OperationCode;
                
                var original = new OriginalData(category, new byte[] { operationCode }, payload);
                byte[] encryptedData = _encryptionService.EncryptClientPackToServer(original);

                // 发送数据
                await _socketClient.SendAsync(encryptedData, cancellationToken);

                // 创建超时任务
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(timeoutMs);

                // 等待响应或超时
                byte[] responseData;
                try
                {
                    responseData = await tcs.Task.WaitAsync(timeoutCts.Token);
                }
                catch (TimeoutException)
                {
                    _pendingResponses.TryRemove(requestId, out _);
                    return ApiResponse<TResponse>.Failure("请求超时");
                }
                catch (OperationCanceledException)
                {
                    _pendingResponses.TryRemove(requestId, out _);
                    return ApiResponse<TResponse>.Failure("请求被取消");
                }

                // 处理响应
                var responsePacket = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(responseData);
                return responsePacket.GetJsonData<ApiResponse<TResponse>>();
            }
            catch (Exception ex)
            {
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
                throw new ArgumentNullException(nameof(command));
            }

            // 从命令对象中提取数据
            var requestData = command.GetSerializableData();
            var timeoutMs = command.TimeoutMs;

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

                // 序列化并加密
                byte[] payload = UnifiedSerializationService.SerializeWithMessagePack(packet);
                
                // 分解CommandId
                byte category = (byte)commandId.Category;
                byte operationCode = commandId.OperationCode;
                
                var original = new OriginalData(category, new byte[] { operationCode }, payload);
                byte[] encryptedData = _encryptionService.EncryptClientPackToServer(original);

                // 发送数据
                await _socketClient.SendAsync(encryptedData, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送单向命令失败: {ex.Message}");
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
                // 解密数据
                var decryptedData = _encryptionService.DecryptServerPack(data);
                
                // 反序列化数据包
                var packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(decryptedData.One);
                
                // 检查是否包含请求ID
                if (packet.Extensions.TryGetValue("RequestId", out var rid) &&
                    _pendingResponses.TryRemove(rid.ToString(), out var tcs))
                {
                    tcs.TrySetResult(decryptedData.One);
                }
                // 如果没有请求ID，或者对应的任务已完成，则视为服务器主动推送的命令
                else
                {
                    // 提取命令ID和数据
                    if (packet.CommandId != null)
                    {
                        object commandData = null;
                        try
                        {
                            // 尝试解析命令数据
                            commandData = packet.GetJsonData<object>();
                        }
                        catch { /* 如果解析失败，保持为null */ }
                        
                        // 触发命令接收事件
                        CommandReceived?.Invoke(packet.CommandId, commandData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理接收到的数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _socketClient.Received -= OnReceived;
            _socketClient.Dispose();
            _isConnected = false;
        }
    }
}