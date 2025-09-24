using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.UI.Common;
using System;

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 请求响应管理器
    /// 管理客户端的请求和对应的响应
    /// </summary>
    public class RequestResponseManager : IDisposable
    {
        private readonly ConcurrentDictionary<string, PendingRequest> _pendingRequests;

        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RequestResponseManager()
        {
            _pendingRequests = new ConcurrentDictionary<string, PendingRequest>();
        }

        /// <summary>
        /// 发送请求并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="socketClient">Socket客户端</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>响应数据</returns>
        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(
            ISocketClient socketClient,
            RUINORERP.PacketSpec.Commands.CommandId commandId,
            TRequest requestData,
            CancellationToken cancellationToken = default,
            int timeoutMs = 30000)
        {
            if (socketClient == null)
                throw new ArgumentNullException(nameof(socketClient));

            // 创建请求ID
            string requestId = Guid.NewGuid().ToString("N");

            // 创建完成任务源
            var tcs = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
            var pendingRequest = new PendingRequest
            {
                TaskCompletionSource = tcs,
                CreatedAt = DateTime.UtcNow
            };

            _pendingRequests.TryAdd(requestId, pendingRequest);

            try
            {
                // 创建和构建数据包
                var packet = PacketBuilder.Create()
                    .WithCommand(commandId)
                    .WithJsonData(requestData)
                    .WithRequestId(requestId)
                    .WithTimeout(timeoutMs)
                    .WithDirection(RUINORERP.PacketSpec.Enums.Core.PacketDirection.ClientToServer)
                    .Build();
                
                // 序列化数据
                byte[] payload;
                try
                {
                    payload = UnifiedSerializationService.SerializeWithMessagePack(packet);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"序列化数据包失败: {ex.Message}", ex);
                }

                // 分解CommandId为Category和OperationCode
                byte category = (byte)commandId.Category;
                byte operationCode = commandId.OperationCode;

                var original = new RUINORERP.PacketSpec.Protocol.OriginalData(category, new byte[] { operationCode }, payload);
                byte[] encryptedData = RUINORERP.PacketSpec.Security.EncryptedProtocol.EncryptClientPackToServer(original);

                // 发送数据
                await socketClient.SendAsync(encryptedData, cancellationToken);

                // 创建超时任务
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(timeoutMs);

                // 等待响应或超时
                byte[] responseData;
                try
                {
                    responseData = await tcs.Task.WaitAsync(TimeSpan.FromMilliseconds(timeoutMs), timeoutCts.Token);
                }
                catch (TimeoutException)
                {
                    throw new TimeoutException($"请求超时: {commandId}");
                }
                catch (OperationCanceledException)
                {
                    throw new OperationCanceledException($"请求被取消: {commandId}");
                }

                // 处理响应
                var decryptedData = RUINORERP.PacketSpec.Security.EncryptedProtocol.DecryptServerPack(responseData);
                PacketModel responsePacket;
                try
                {
                    responsePacket = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(decryptedData.One);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"反序列化响应数据包失败: {ex.Message}", ex);
                }
                return responsePacket.GetJsonData<TResponse>();
            }
            finally
            {
                // 清理等待的请求
                _pendingRequests.TryRemove(requestId, out _);
            }
        }

        /// <summary>
        /// 处理接收到的响应数据
        /// </summary>
        /// <param name="data">接收到的数据</param>
        public void HandleResponse(byte[] data)
        {
            try
            {
                // 解密数据
                var decryptedData = RUINORERP.PacketSpec.Security.EncryptedProtocol.DecryptServerPack(data);

                // 反序列化数据包
                PacketModel packet;
                try
                {
                    packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(decryptedData.Two);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"反序列化响应数据包失败: {ex.Message}");
                    return;
                }

                if (packet != null)
                {
                    // 检查是否包含请求ID
                    if (packet.Extensions.TryGetValue("RequestId", out var rid) &&
                        _pendingRequests.TryRemove(rid.ToString(), out var pendingRequest))
                    {
                        // 完成任务
                        pendingRequest.TaskCompletionSource.TrySetResult(data); // 传递原始加密数据
                    }
                }
               
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，避免影响其他处理
                Console.WriteLine($"处理响应数据时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理超时的请求
        /// </summary>
        public void CleanupTimeoutRequests()
        {
            var now = DateTime.UtcNow;
            var timeoutRequests = new ConcurrentBag<string>();

            foreach (var kvp in _pendingRequests)
            {
                var request = kvp.Value;
                if ((now - request.CreatedAt).TotalMilliseconds > request.TimeoutMs)
                {
                    timeoutRequests.Add(kvp.Key);
                }
            }

            foreach (var requestId in timeoutRequests)
            {
                if (_pendingRequests.TryRemove(requestId, out var pendingRequest))
                {
                    pendingRequest.TaskCompletionSource.TrySetException(
                        new TimeoutException($"请求超时: {requestId}"));
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                // 取消所有等待的请求
                foreach (var kvp in _pendingRequests)
                {
                    kvp.Value.TaskCompletionSource.TrySetException(
                        new ObjectDisposedException(nameof(RequestResponseManager)));
                }

                _pendingRequests.Clear();
                _disposed = true;
            }
        }

        /// <summary>
        /// 等待的请求信息
        /// </summary>
        private class PendingRequest
        {
            /// <summary>
            /// 任务完成源
            /// </summary>
            public TaskCompletionSource<byte[]> TaskCompletionSource { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime CreatedAt { get; set; }

            /// <summary>
            /// 超时时间（毫秒）
            /// </summary>
            public int TimeoutMs { get; set; } = 30000;
        }
    }
}