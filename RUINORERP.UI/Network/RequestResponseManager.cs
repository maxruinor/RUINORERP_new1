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
using RUINORERP.PacketSpec.Commands;
using RUINORERP.UI.Network.RetryStrategy;
using RUINORERP.UI.Network.TimeoutStatistics;
using RUINORERP.UI.Network.ErrorHandling;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using RUINORERP.UI.Network.Exceptions;

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
        
        // 超时统计管理器
        private readonly TimeoutStatisticsManager _timeoutStatistics;
        
        // 错误处理策略工厂
        private readonly ErrorHandlingStrategyFactory _errorHandlingStrategyFactory;
        
        // 默认重试策略
        private readonly IRetryStrategy _defaultRetryStrategy;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器，用于记录请求响应过程中的信息和异常</param>
        public RequestResponseManager(ILogger<RequestResponseManager> logger = null)
        {
            _logger = logger;
            _timeoutStatistics = new TimeoutStatisticsManager();
            _errorHandlingStrategyFactory = new ErrorHandlingStrategyFactory();
            _defaultRetryStrategy = new ExponentialBackoffRetryStrategy(3, 1000, 2.0, 10000);
            _logger?.LogDebug("RequestResponseManager已初始化");
        }
        
        /// <summary>
        /// 发送请求并等待响应，支持重试策略
        /// 提供带重试逻辑的异步请求-响应模式，支持超时、取消操作和自动重试
        /// 特别适用于网络不稳定环境下的可靠通信
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="socketClient">Socket客户端实例，用于发送请求</param>
        /// <param name="commandId">命令ID，标识请求的命令类型</param>
        /// <param name="requestData">请求数据对象</param>
        /// <param name="retryStrategy">重试策略，如果为null则使用默认策略</param>
        /// <param name="ct">取消令牌，用于取消请求</param>
        /// <param name="timeoutMs">单次请求超时时间（毫秒），默认30秒</param>
        /// <returns>响应数据对象</returns>
        /// <exception cref="ArgumentNullException">当socketClient为空时抛出</exception>
        /// <exception cref="TimeoutException">当所有重试都超时时抛出</exception>
        /// <exception cref="OperationCanceledException">当请求被取消时抛出</exception>
        /// <exception cref="InvalidOperationException">当请求处理过程中发生不可重试的异常时抛出</exception>
        public async Task<TResponse> SendRequestWithRetryAsync<TRequest, TResponse>(
            ISocketClient socketClient, 
            RUINORERP.PacketSpec.Commands.CommandId commandId, 
            TRequest requestData,
            IRetryStrategy retryStrategy = null,
            CancellationToken ct = default,
            int timeoutMs = 30000)
        {
            if (socketClient == null)
                throw new ArgumentNullException(nameof(socketClient));
            
            // 使用提供的重试策略或默认策略
            var strategy = retryStrategy ?? _defaultRetryStrategy;
            
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
                        _logger?.LogDebug("开始发送请求，命令ID: {CommandId}，尝试次数: {Attempt}", commandId, attempt + 1);
                        
                        // 记录开始时间
                        var startTime = DateTime.Now;
                        
                        // 发送请求并获取响应
                        var response = await SendRequestAsync<TRequest, TResponse>(
                            socketClient, 
                            commandId, 
                            requestData, 
                            ct, 
                            timeoutMs);
                        
                        // 计算处理时间
                        var processingTimeMs = (long)(DateTime.Now - startTime).TotalMilliseconds;
                        
                        // 记录成功信息
                        _timeoutStatistics.RecordSuccess(commandId.ToString(), processingTimeMs);
                        
                        _logger?.LogDebug("请求成功完成，命令ID: {CommandId}，总耗时: {TotalTime}ms，尝试次数: {Attempt}", 
                            commandId, stopwatch.ElapsedMilliseconds, attempt + 1);
                        
                        return response;
                    }
                    catch (Exception ex) when (IsRetryableException(ex))
                    {
                        lastException = ex;
                        
                        // 检查是否应该继续重试
                        if (!strategy.ShouldContinue(attempt))
                        {
                            _logger?.LogError(ex, "请求重试失败，已达到最大重试次数，命令ID: {CommandId}", commandId);
                            throw;
                        }
                        
                        // 获取下一次重试的延迟时间
                        int delayMs = strategy.GetNextDelay(attempt);
                        
                        _logger?.LogWarning(ex, "请求失败将重试，命令ID: {CommandId}，尝试次数: {Attempt}，延迟时间: {DelayMs}ms", 
                            commandId, attempt + 1, delayMs);
                        
                        // 等待重试延迟
                        await Task.Delay(delayMs, ct);
                        attempt++;
                    }
                    catch (TimeoutException ex)
                    {
                        lastException = ex;
                        
                        // 记录超时信息
                        _timeoutStatistics.RecordTimeout(commandId.ToString(), timeoutMs);
                        
                        // 处理超时错误
                        await _errorHandlingStrategyFactory.GetStrategy(TimeoutStatistics.NetworkErrorType.TimeoutError)
                            .HandleErrorAsync(TimeoutStatistics.NetworkErrorType.TimeoutError, ex.Message, commandId.ToString());
                        
                        // 检查是否应该继续重试
                        if (!strategy.ShouldContinue(attempt))
                        {
                            _logger?.LogError(ex, "请求超时失败，已达到最大重试次数，命令ID: {CommandId}", commandId);
                            throw;
                        }
                        
                        // 获取下一次重试的延迟时间
                        int delayMs = strategy.GetNextDelay(attempt);
                        
                        _logger?.LogWarning(ex, "请求超时将重试，命令ID: {CommandId}，尝试次数: {Attempt}，延迟时间: {DelayMs}ms", 
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
                        
                        // 识别错误类型
                        var errorType = IdentifyErrorType(ex);
                        
                        // 记录错误统计
                        if (errorType == TimeoutStatistics.NetworkErrorType.TimeoutError)
                        {
                            _timeoutStatistics.RecordTimeout(commandId.ToString(), timeoutMs);
                        }
                        
                        // 处理错误
                        await _errorHandlingStrategyFactory.GetStrategy(errorType)
                            .HandleErrorAsync(errorType, ex.Message, commandId.ToString());
                        
                        _logger?.LogError(ex, "请求处理失败，不可重试的错误，命令ID: {CommandId}", commandId);
                        throw;
                    }
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }
        
        /// <summary>
        /// 判断异常是否可重试
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>是否可重试</returns>
        private bool IsRetryableException(Exception ex)
        {
            // 根据异常类型判断是否可重试
            // 这里可以根据实际业务需求进行扩展
            return ex is TimeoutException ||
                   ex is System.IO.IOException ||
                   ex is System.Net.Sockets.SocketException ||
                   ex.Message.IndexOf("connection", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   ex.Message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        
        /// <summary>
        /// 识别错误类型
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>错误类型</returns>
        private NetworkErrorType IdentifyErrorType(Exception ex)
        {
            // 根据异常类型和消息识别错误类型
            // 这里可以根据实际业务需求进行扩展
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
        /// 获取超时统计信息
        /// </summary>
        /// <returns>超时统计管理器</returns>
        public TimeoutStatisticsManager GetTimeoutStatistics()
        {
            return _timeoutStatistics;
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
            
            var requestId = Guid.NewGuid().ToString("N");
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(timeoutMs);

            var tcs = new TaskCompletionSource<PacketModel>(TaskCreationOptions.RunContinuationsAsynchronously);
            var pendingRequest = new PendingRequest { Tcs = tcs, CreatedAt = DateTime.UtcNow, CommandId = commandId.ToString() };

            if (!_pending.TryAdd(requestId, pendingRequest))
            {
                throw new InvalidOperationException($"无法添加请求到待处理列表，请求ID: {requestId}");
            }

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
            var responsePacket = await tcs.Task;
            var response = responsePacket.GetJsonData<TResponse>();

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
        /// 处理接收到的响应数据包
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>是否是响应包</returns>
        public bool HandleResponse(PacketModel packet)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RequestResponseManager));

            try
            {
                if (packet?.Extensions?.TryGetValue("RequestId", out var requestIdObj) == true)
                {
                    var requestId = requestIdObj?.ToString();

                    if (!string.IsNullOrEmpty(requestId) &&
                        _pending.TryRemove(requestId, out var pendingRequest))
                    {
                        // ✅ 直接设置PacketModel作为结果
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


        /// <summary>
        /// 发送请求的核心方法
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="client">Socket客户端</param>
        /// <param name="cmd">命令ID</param>
        /// <param name="body">请求数据（可能已附加Token）</param>
        /// <param name="requestId">请求ID</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        private async Task SendCoreAsync<TRequest>(
            ISocketClient client,
            RUINORERP.PacketSpec.Commands.CommandId cmd,
            TRequest body,
            string requestId,
            int timeoutMs,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                // 构建数据包，包含可能已附加Token的请求数据
                var packet = PacketBuilder.Create()
                                          .WithCommand(cmd)
                                          .WithJsonData(body)  // body可能已包含Token信息
                                          .WithRequestId(requestId)
                                          .WithTimeout(timeoutMs)
                                          .Build();

                packet.ClientId = client.ClientID;

                // 序列化和加密数据包
                var payload = UnifiedSerializationService.SerializeWithMessagePack(packet);
                var original = new OriginalData(
                    (byte)cmd.Category,
                    new[] { cmd.OperationCode },
                    payload);

                var encrypted = RUINORERP.PacketSpec.Security.EncryptedProtocol.EncryptClientPackToServer(original);
                await client.SendAsync(encrypted, ct);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "SendCoreAsync方法发送请求时发生错误: CommandId={CommandId}, RequestId={RequestId}", 
                    cmd.FullCode, requestId);
                
                // 如果是取消操作，则直接抛出
                if (ex is OperationCanceledException) {
                    throw;
                }
                
                // 包装异常以便上层处理（包括可能的Token过期处理）
                throw new NetworkCommunicationException(
                    $"发送请求失败: {ex.Message}", 
                    ex, 
                    cmd, 
                    requestId);
            }
        }

       

        /* -------------------- 内部类型 -------------------- */

        /// <summary>
        /// 待处理请求的内部类
        /// 封装了请求的异步完成源、创建时间和命令ID
        /// </summary>
        private class PendingRequest
        {
            /// <summary>异步完成源，用于等待请求响应</summary>
            public TaskCompletionSource<PacketModel> Tcs { get; set; }

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