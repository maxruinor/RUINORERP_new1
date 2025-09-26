using RUINORERP.PacketSpec.Commands;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.UI.Network.Simplified
{
    /// <summary>
    /// 简化客户端接口
    /// 提供更直观、易用的API来发送请求和接收响应
    /// </summary>
    public interface ISimplifiedClient
    {
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
        Task<TResponse> SendAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest request,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TResponse : IResponse;

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
        Task<TResponse> SendAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest request,
            int retryCount,
            int retryDelayMs,
            CancellationToken ct = default,
            int timeoutMs = 30000)
            where TResponse : IResponse;

        /// <summary>
        /// 发送单向请求（不等待响应）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>发送是否成功</returns>
        Task<bool> SendOneWayAsync<TRequest>(
            CommandId commandId,
            TRequest request,
            CancellationToken ct = default);

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
        Task<bool> SendOneWayAsync<TRequest>(
            CommandId commandId,
            TRequest request,
            int retryCount,
            int retryDelayMs,
            CancellationToken ct = default);
    }
}