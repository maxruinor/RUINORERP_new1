using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Communication
{
    /// <summary>
    /// 请求处理器接口
    /// 定义通用的请求处理契约
    /// </summary>
    /// <typeparam name="TRequest">请求数据类型</typeparam>
    /// <typeparam name="TResponse">响应数据类型</typeparam>
    public interface IRequestHandler<TRequest, TResponse>
    {
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应结果</returns>
        Task<ApiResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 命令请求处理器接口
    /// 专门处理基于命令的请求
    /// </summary>
    /// <typeparam name="TResponse">响应数据类型</typeparam>
    public interface ICommandRequestHandler<TResponse>
    {
        /// <summary>
        /// 处理命令请求
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应结果</returns>
        Task<ApiResponse<TResponse>> HandleAsync(ICommand command, CancellationToken cancellationToken = default);
    }
}