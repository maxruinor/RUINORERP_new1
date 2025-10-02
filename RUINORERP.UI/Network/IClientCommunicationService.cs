using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading.Tasks;
using System.Threading;
using RUINORERP.UI.Network.RetryStrategy;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端通信服务接口 - 业务层通信统一入口
    /// 
    /// 提供命令发送、接收和连接管理功能，是业务层与网络层的桥梁
    /// 支持同步和异步命令处理、连接状态管理、自动重连和重试策略
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
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>带包装的API响应</returns>
        Task<TResponse> SendCommandAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken ct = default,
            int timeoutMs = 30000);


        /// <summary>
        /// 发送命令并等待响应
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>带包装的API响应</returns>
        Task<TResponse> SendCommandAsync<TRequest, TResponse>(
            BaseCommand command,
            CancellationToken ct = default,
            int timeoutMs = 30000);
        /// <summary>
        /// 发送命令并等待响应（使用命令对象）
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>带包装的API响应</returns>
        Task<TResponse> SendCommandAsync<TResponse>(
            ICommand command,
            CancellationToken ct = default);

        /// <summary>
        /// 发送命令并等待响应，支持重试策略
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="retryStrategy">重试策略，如果为null则使用默认策略</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>带包装的API响应</returns>
        Task<TResponse> SendCommandWithRetryAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest requestData,
            IRetryStrategy retryStrategy = null,
            CancellationToken ct = default,
            int timeoutMs = 30000);

        /// <summary>
        /// 发送命令并等待响应（使用命令对象），支持重试策略
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="retryStrategy">重试策略，如果为null则使用默认策略</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>带包装的API响应</returns>
        Task<TResponse> SendCommandWithRetryAsync<TResponse>(
            ICommand command,
            IRetryStrategy retryStrategy = null,
            CancellationToken ct = default);

        /// <summary>
        /// 发送单向命令（不等待响应）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>是否发送成功</returns>
        Task<bool> SendOneWayCommandAsync<TRequest>(
            CommandId commandId,
            TRequest requestData,
            CancellationToken ct = default);

        /// <summary>
        /// 发送单向命令（不等待响应，支持重试策略）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="retryStrategy">重试策略，如果为null则使用默认策略</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>是否发送成功</returns>
        Task<bool> SendOneWayCommandWithRetryAsync<TRequest>(
            CommandId commandId,
            TRequest requestData,
            IRetryStrategy retryStrategy = null,
            CancellationToken ct = default);

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
        /// <param name="ct">取消令牌</param>
        /// <returns>连接是否成功</returns>
        Task<bool> ConnectAsync(string serverUrl, int port, CancellationToken ct = default);

        /// <summary>
        /// 断开连接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 重新连接到服务器
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>重连是否成功</returns>
        Task<bool> ReconnectAsync(CancellationToken ct = default);

        /// <summary>
        /// 服务器地址
        /// </summary>
        string ServerAddress { get; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        int ServerPort { get; }


        /// <summary>
        /// 简洁的命令调用方法 - 业务层核心API
        /// 自动完成请求DTO到PacketModel的转换，以及响应PacketModel到DTO的转换
        /// 让业务代码只关心"我要发什么、拿到什么"，不再关心传输层细节
        /// </summary>
        /// <typeparam name="TReq">请求DTO类型</typeparam>
        /// <typeparam name="TResp">响应DTO类型</typeparam>
        /// <param name="req">请求DTO对象</param>
        /// <param name="adapter">数据包适配器，默认为null（使用默认JSON适配器）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>响应DTO对象</returns>
        Task<TResp> CallAsync<TReq, TResp>(TReq req,
                                          IPacketAdapter<TReq, TResp> adapter = null,
                                          CancellationToken ct = default);


        /* 无参/单向/重试 全部通过重载内聚，不暴露多余泛型 */
        Task<TResp> CallAsync<TResp>(ICommand command, IPacketAdapter<object, TResp> adapter = null, CancellationToken ct = default) where TResp : class;

        /// <summary>
        /// 发送请求并等待响应（兼容旧API）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <param name="adapter">数据包适配器</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>响应数据</returns>
        Task<TResponse> SendAsync<TRequest, TResponse>(
            CommandId commandId,
            TRequest request,
            IPacketAdapter<TRequest, TResponse> adapter = null,
            CancellationToken ct = default,
            int timeoutMs = 30000);


    }
}