using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Simplified
{
    /// <summary>
    /// 简化客户端实现
    /// 封装复杂的请求-响应处理逻辑，提供更简单的API
    /// </summary>
    public class SimplifiedClient : ISimplifiedClient
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly int _defaultRetryCount = 3;
        private readonly int _defaultRetryDelayMs = 1000;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">底层通信服务</param>
        public SimplifiedClient(IClientCommunicationService communicationService)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
        }

        /// <summary>
        /// 发送请求并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>响应数据</returns>
        /// <exception cref="CommunicationException">通信异常</exception>
        public async Task<TResponse> SendAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest request,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TResponse : IResponse
        {
            return await SendAsync<TRequest, TResponse>(commandId, request, _defaultRetryCount, _defaultRetryDelayMs, ct, timeoutMs);
        }

        /// <summary>
        /// 发送请求并等待响应（带重试机制）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求数据</param>
        /// <param name="retryCount">重试次数</param>
        /// <param name="retryDelayMs">重试延迟（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>响应数据</returns>
        /// <exception cref="CommunicationException">通信异常</exception>
        public async Task<TResponse> SendAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest request,
            int retryCount,
            int retryDelayMs,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TResponse : IResponse
        {
            Exception lastException = null;
            
            for (int attempt = 0; attempt <= retryCount; attempt++)
            {
                try
                {
                    // 使用底层通信服务发送请求
                    var response = await _communicationService.SendCommandAsync<TRequest, TResponse>(
                        commandId, 
                        request, 
                        ct, 
                        timeoutMs);

                    // 检查响应是否成功
                    if (response.IsSuccess)
                    {
                        // 直接返回响应数据，无需额外解析
                        return response;
                    }

                    // 如果失败，抛出通信异常
                    throw new CommunicationException(response.Message, response.Code);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    
                    // 如果是最后一次尝试，或者不支持重试的异常，则抛出异常
                    if (attempt == retryCount || !IsRetryableException(ex))
                    {
                        throw new CommunicationException($"请求失败（已重试 {attempt} 次）: {ex.Message}", ex);
                    }
                    
                    // 等待重试延迟
                    await Task.Delay(retryDelayMs, ct);
                }
            }
            
            // 如果所有重试都失败了，抛出最后一个异常
            throw new CommunicationException($"请求失败（已重试 {retryCount} 次）: {lastException?.Message}", lastException);
        }

        /// <summary>
        /// 发送单向请求（不等待响应）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送是否成功</returns>
        public async Task<bool> SendOneWayAsync<TRequest>(
            CommandId commandId,
            TRequest request,
            CancellationToken ct = default)
        {
            return await SendOneWayAsync(commandId, request, _defaultRetryCount, _defaultRetryDelayMs, ct);
        }

        /// <summary>
        /// 发送单向请求（不等待响应，带重试机制）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求数据</param>
        /// <param name="retryCount">重试次数</param>
        /// <param name="retryDelayMs">重试延迟（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送是否成功</returns>
        public async Task<bool> SendOneWayAsync<TRequest>(
            CommandId commandId,
            TRequest request,
            int retryCount,
            int retryDelayMs,
            CancellationToken ct = default)
        {
            Exception lastException = null;
            
            for (int attempt = 0; attempt <= retryCount; attempt++)
            {
                try
                {
                    // 直接使用底层通信服务发送单向请求
                    return await _communicationService.SendOneWayCommandAsync(commandId, request, ct);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    
                    // 如果是最后一次尝试，或者不支持重试的异常，则抛出异常
                    if (attempt == retryCount || !IsRetryableException(ex))
                    {
                        throw new CommunicationException($"单向请求发送失败（已重试 {attempt} 次）: {ex.Message}", ex);
                    }
                    
                    // 等待重试延迟
                    await Task.Delay(retryDelayMs, ct);
                }
            }
            
            // 如果所有重试都失败了，抛出最后一个异常
            throw new CommunicationException($"单向请求发送失败（已重试 {retryCount} 次）: {lastException?.Message}", lastException);
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
                   (ex is CommunicationException commEx && commEx.Code >= 500); // 服务器错误支持重试
        }
    }
}