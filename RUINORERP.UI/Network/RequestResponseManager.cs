using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;
using Org.BouncyCastle.Bcpg;
using RUINORERP.PacketSpec.Security;
using System.Collections.Concurrent;

namespace RUINORERP.UI.Network
{
    /// <summary> 
    /// 请求响应管理器 
    /// 管理客户端的请求和对应的响应，提供异步请求-响应模式的支持 
    /// 负责维护请求状态、处理超时、序列化/反序列化数据包 
    /// 是客户端网络通信中的核心组件，确保请求-响应的正确匹配和超时处理 
    /// </summary> 
    public class RequestResponseManager : IDisposable
    {
        private readonly ConcurrentDictionary<string, PendingRequest> _pending = new();
        private readonly ILogger<RequestResponseManager> _logger;
        private readonly object _lock = new object();
        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器，用于记录请求响应过程中的信息和异常</param>
        public RequestResponseManager(ILogger<RequestResponseManager> logger = null)
        {
            _logger = logger;
            _logger?.LogDebug("RequestResponseManager已初始化");
        }

        /// <summary> 
        /// 发送请求并等待响应 
        /// 提供类型安全的异步请求-响应模式，支持超时和取消操作 
        /// </summary> 
        /// <typeparam name="TRequest">请求数据类型</typeparam> 
        /// <typeparam name="TResponse">响应数据类型</typeparam> 
        /// <param name="socketClient">Socket客户端实例，用于发送请求</param> 
        /// <param name="commandId">命令ID，标识请求的命令类型</param> 
        /// <param name="requestData">请求数据对象</param> 
        /// <param name="ct">取消令牌，用于取消请求</param> 
        /// <param name="timeoutMs">超时时间（毫秒），默认30秒</param> 
        /// <returns>响应数据对象</returns> 
        /// <exception cref="ArgumentNullException">当socketClient为空时抛出</exception> 
        /// <exception cref="TimeoutException">当请求超时时抛出</exception> 
        /// <exception cref="OperationCanceledException">当请求被取消时抛出</exception> 
        /// <exception cref="InvalidOperationException">当请求处理过程中发生异常时抛出</exception> 
        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(
            ISocketClient socketClient,
            RUINORERP.PacketSpec.Commands.CommandId commandId,
            TRequest requestData,
            CancellationToken ct = default,
            int timeoutMs = 30000)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RequestResponseManager));
            if (socketClient == null) throw new ArgumentNullException(nameof(socketClient), "Socket客户端不能为空");
            if (commandId == null) throw new ArgumentNullException(nameof(commandId), "命令ID不能为空");
            if (requestData == null) throw new ArgumentNullException(nameof(requestData), "请求数据不能为空");
            if (timeoutMs <= 0) throw new ArgumentException("超时时间必须大于0", nameof(timeoutMs));

            var requestId = Guid.NewGuid().ToString("N");
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(timeoutMs);

            var tcs = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
            var pendingRequest = new PendingRequest { Tcs = tcs, CreatedAt = DateTime.UtcNow, CommandId = commandId.ToString() };

            if (!_pending.TryAdd(requestId, pendingRequest))
            {
                throw new InvalidOperationException($"无法添加请求到待处理列表，请求ID: {requestId}");
            }

            _logger?.LogDebug("发送请求，请求ID: {RequestId}, 命令ID: {CommandId}, 超时时间: {TimeoutMs}ms",
                requestId, commandId.FullCode, timeoutMs);

            try
            {
                await SendCoreAsync(socketClient, commandId, requestData, requestId, timeoutMs, ct);

                // 等待响应或超时
                var timeoutTask = Task.Delay(timeoutMs, cts.Token);
                var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    _logger?.LogError("请求超时，请求ID: {RequestId}", requestId);
                    throw new TimeoutException($"请求超时（{timeoutMs}ms），请求ID: {requestId}");
                }

                // 检查是否被取消
                ct.ThrowIfCancellationRequested();

                // 确保任务已完成并获取结果
                var raw = await tcs.Task;
                var response = DeserializeResponse<TResponse>(raw);

                _logger?.LogDebug("成功接收响应，请求ID: {RequestId}", requestId);
                return response;
            }
            catch (Exception ex) when (!(ex is TimeoutException) && !(ex is OperationCanceledException))
            {
                _logger?.LogError(ex, "请求处理失败，请求ID: {RequestId}", requestId);
                throw new InvalidOperationException($"请求处理失败，请求ID: {requestId}: {ex.Message}", ex);
            }
            finally
            {
                if (_pending.TryRemove(requestId, out _))
                {
                    _logger?.LogTrace("已从待处理列表移除请求，请求ID: {RequestId}", requestId);
                }
            }
        }

        /// <summary> 
        /// 处理接收到的响应数据 
        /// 解密响应数据，提取请求ID，并匹配对应的待处理请求 
        /// </summary> 
        /// <param name="data">接收到的原始响应数据</param> 
        /// <exception cref="ArgumentNullException">当数据为空时抛出</exception> 
        /// <exception cref="ArgumentException">当数据长度无效时抛出</exception> 
        public bool HandleResponse(byte[] data)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RequestResponseManager));


            try
            {
                if (data == null || data.Length == 0)
                {
                    _logger?.LogWarning("接收到的响应数据为空");
                    return false;
                }

                // 解密服务器数据包
                var decrypted = EncryptedProtocol.DecryptServerPack(data);
                if (decrypted.Two == null)
                {
                    _logger?.LogWarning("解密响应数据失败");
                    return false;
                }

                // 反序列化数据包
                var packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(decrypted.Two);
                if (packet?.Extensions?.TryGetValue("RequestId", out var rid) == true &&
                    _pending.TryRemove(rid.ToString(), out var pr))
                {
                    _logger?.LogDebug("成功匹配响应到请求，请求ID: {RequestId}", rid);
                    return pr.Tcs.TrySetResult(data);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理响应失败，数据长度: {DataLength}", data?.Length ?? 0);
            }
            return false;
        }

        /// <summary> 
        /// 清理超时的请求 
        /// 遍历所有待处理请求，将超时的请求标记为失败并从字典中移除 
        /// </summary> 
        /// <returns>清理的超时请求数量</returns> 
        public int CleanupTimeoutRequests()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RequestResponseManager));

            var now = DateTime.UtcNow;
            var cut = now.AddMinutes(-5);
            var removedCount = 0;

            foreach (var kv in _pending)
            {
                if (kv.Value.CreatedAt < cut && _pending.TryRemove(kv.Key, out var pr))
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

            return removedCount;
        }

        /// <summary> 
        /// 释放资源 
        /// 取消所有待处理请求并清理资源 
        /// </summary> 
        public void Dispose()
        {
            if (_disposed) return;

            lock (_lock)
            {
                if (_disposed) return;
                _disposed = true;
            }

            var pendingCount = _pending.Count;
            _logger?.LogInformation("开始释放RequestResponseManager资源，待处理请求数量: {PendingCount}", pendingCount);

            try
            {
                foreach (var kv in _pending.Values)
                {
                    try
                    {
                        kv.Tcs.TrySetCanceled();
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "取消请求时发生异常");
                    }
                }

                _pending.Clear();
                _logger?.LogInformation("RequestResponseManager资源已释放");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "释放资源时发生异常");
            }
        }

        /* -------------------- 私有模板 -------------------- */

        private async Task SendCoreAsync<TRequest>(
            ISocketClient client,
            RUINORERP.PacketSpec.Commands.CommandId cmd,
            TRequest body,
            string requestId,
            int timeoutMs,
            CancellationToken ct)
        {
            var packet = PacketBuilder.Create()
                                      .WithCommand(cmd)
                                      .WithJsonData(body)
                                      .WithRequestId(requestId)
                                      .WithTimeout(timeoutMs)
                                      .Build();

            var payload = UnifiedSerializationService.SerializeWithMessagePack(packet);
            var original = new OriginalData(
                (byte)cmd.Category,
                new[] { cmd.OperationCode },
                payload);

            var encrypted = RUINORERP.PacketSpec.Security.EncryptedProtocol.EncryptClientPackToServer(original);
            await client.SendAsync(encrypted, ct);
        }

        private static TResponse DeserializeResponse<TResponse>(byte[] raw)
        {
            var decrypted = RUINORERP.PacketSpec.Security.EncryptedProtocol.DecryptServerPack(raw);
            var pkt = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(decrypted.Two);
            return pkt.GetJsonData<TResponse>();
        }

        /* -------------------- 内部类型 -------------------- */

        /// <summary>
        /// 待处理请求的内部类
        /// 封装了请求的异步完成源、创建时间和命令ID
        /// </summary>
        private class PendingRequest
        {
            /// <summary>异步完成源，用于等待请求响应</summary>
            public TaskCompletionSource<byte[]> Tcs { get; set; }

            /// <summary>请求创建时间</summary>
            public DateTime CreatedAt { get; set; }

            /// <summary>关联的命令ID</summary>
            public string CommandId { get; set; }
        }

        /// <summary>
        /// 待处理请求信息类
        /// 用于外部查询待处理请求的详细信息
        /// </summary>
        public class PendingRequestInfo
        {
            /// <summary>请求ID</summary>
            public string RequestId { get; set; }

            /// <summary>请求创建时间</summary>
            public DateTime CreatedAt { get; set; }

            /// <summary>关联的命令ID</summary>
            public string CommandId { get; set; }

            /// <summary>已运行时间（毫秒）</summary>
            public double ElapsedMs { get; set; }
        }
    }
}