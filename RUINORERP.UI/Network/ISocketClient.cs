using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Threading; 
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
/// <summary>
    /// Socket客户端接口 - 底层网络通信抽象
    /// 
    /// 🔄 接口设计目标：
    /// 1. 抽象底层Socket实现细节
    /// 2. 提供统一的网络通信接口
    /// 3. 支持多种Socket实现
    /// 4. 便于依赖注入和测试
    /// 
    /// 📋 核心功能：
    /// - 连接管理（建立/断开）
    /// - 数据发送与接收
    /// - 连接状态监控
    /// - 事件通知机制
    /// - 错误处理与日志
    /// 
    /// 🔗 与架构集成：
    /// - SuperSocketClient 的具体实现
    /// - 被 CommunicationManager 使用
    /// - 支持依赖注入容器
    /// - 便于单元测试mock
    /// 
    /// 📡 事件定义：
    /// - DataReceived: 数据接收事件
    /// - Connected: 连接建立事件
    /// - Disconnected: 连接断开事件
    /// - ErrorOccurred: 错误发生事件
    /// 
    /// 💡 设计原则：
    /// - 接口隔离原则
    /// - 依赖倒置原则
    /// - 易于扩展和替换
    /// </summary>
    public interface ISocketClient : IDisposable
    {
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
        event Action<byte[]> Received;

        /// <summary>
        /// 连接关闭时触发的事件
        /// </summary>
        event Action<EventArgs> Closed;
    }
}