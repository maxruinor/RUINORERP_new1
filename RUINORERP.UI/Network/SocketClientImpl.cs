using RUINORERP.UI.SuperSocketClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// Socket客户端实现类
    /// 使用EasyClientService实现ISocketClient接口
    /// </summary>
    public class SocketClientImpl : ISocketClient
    {
        private readonly EasyClientService _easyClientService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SocketClientImpl()
        {
            // 创建EasyClientService实例
            _easyClientService = new EasyClientService();
            // 注册事件处理
            _easyClientService.OnConnectClosed += OnConnectClosed;
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected => _easyClientService.IsConnected;

        /// <summary>
        /// 接收到数据时触发的事件
        /// </summary>
        public event Action<byte[]> Received;

        /// <summary>
        /// 连接关闭时触发的事件
        /// </summary>
        public event Action<EventArgs> Closed;

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="serverUrl">服务器地址</param>
        /// <param name="port">端口号</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>连接是否成功</returns>
        public async Task<bool> ConnectAsync(string serverUrl, int port, CancellationToken cancellationToken = default)
        {
            // 设置服务器地址和端口
            _easyClientService.ServerIp = serverUrl;
            _easyClientService.Port = port;

            // 连接服务器
            return await _easyClientService.Connect(cancellationToken);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            // 由于EasyClientService没有直接的Disconnect方法，我们通过调用Dispose来实现
            Dispose();
        }

        /// <summary>
        /// 发送数据到服务器
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送任务</returns>
        public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            // 检查连接状态
            if (!_easyClientService.IsConnected)
            {
                throw new InvalidOperationException("未连接到服务器");
            }

            // 将数据添加到发送队列
            _easyClientService.DataQueue.Enqueue(data);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理连接关闭事件
        /// </summary>
        private void OnConnectClosed(bool isConnected)
        {
            if (!isConnected)
            {
                Closed?.Invoke(EventArgs.Empty);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // 清理事件注册
            _easyClientService.OnConnectClosed -= OnConnectClosed;
        }
    }
}