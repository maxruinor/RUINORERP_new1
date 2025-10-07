using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// </summary>
    public interface ISocketClient : IDisposable
    {
        /// <summary>
        /// 作为客户端要有唯一的ID
        /// </summary>
        string ClientID { get; set; }

        string ClientIP { get; set; }

        string SessionID { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
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

        /// <summary>
        /// 发送数据到服务器
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送任务</returns>
        Task SendAsync(byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// 接收到数据时触发的事件
        /// </summary>
        event Action<PacketModel> Received;

        /// <summary>
        /// 连接关闭时触发的事件
        /// </summary>
        event Action<EventArgs> Closed;

    }
}