using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端通信服务接口
    /// 定义客户端与服务器通信的标准方法
    /// </summary>
    public interface IClientCommunicationService : IDisposable
    {
        /// <summary>
        /// 当接收到服务器命令时触发的事件
        /// </summary>
        event Action<CommandId, object> CommandReceived;
        /// <summary>
        /// 发送命令并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>带包装的API响应</returns>
        Task<ApiResponse<TResponse>> SendCommandAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken cancellationToken = default,
            int timeoutMs = 30000);

        /// <summary>
        /// 发送命令并等待响应（使用命令对象）
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>带包装的API响应</returns>
        Task<ApiResponse<TResponse>> SendCommandAsync<TResponse>(
            ICommand command,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 发送单向命令（不等待响应）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否发送成功</returns>
        Task<bool> SendOneWayCommandAsync<TRequest>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 检查连接状态
        /// </summary>
        /// <returns>连接是否有效</returns>
        bool IsConnected { get; }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="serverUrl">服务器地址</param>
        /// <param name="port">端口号</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>连接是否成功</returns>
        Task<bool> ConnectAsync(string serverUrl, int port, CancellationToken cancellationToken = default);

        /// <summary>
        /// 断开连接
        /// </summary>
        void Disconnect();
    }
}