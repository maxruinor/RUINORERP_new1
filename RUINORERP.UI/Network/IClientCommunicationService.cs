using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端通信服务接口 - 定义客户端与服务器通信的核心功能
    /// </summary>
    public interface IClientCommunicationService : IDisposable
    {
        /// <summary>
        /// 连接状态
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="serverAddress">服务器地址</param>
        /// <param name="port">服务器端口</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>连接是否成功</returns>
        Task<bool> ConnectAsync(string serverAddress, int port, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取当前连接的服务器地址
        /// </summary>
        string GetCurrentServerAddress();

        /// <summary>
        /// 获取当前连接的服务器端口
        /// </summary>
        int GetCurrentServerPort();

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns>断开连接是否成功</returns>
        Task<bool> Disconnect();

        /// <summary>
        /// 发送命令并等待响应
        /// </summary>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>响应数据包</returns>
        Task<PacketModel> SendCommandAsync(CommandId commandId, IRequest request, CancellationToken cancellationToken = default, int timeoutMs = 30000);

        /// <summary>
        /// 发送单向命令（不需要响应）
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送是否成功</returns>
        Task<bool> SendOneWayCommandAsync<TRequest>(CommandId commandId, TRequest request, CancellationToken cancellationToken = default) where TRequest : class, IRequest;



        /// <summary>
        /// 订阅特定命令的处理
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="handler">处理函数</param>
        void SubscribeCommand(CommandId commandId, Action<PacketModel, object> handler);
    }

}
