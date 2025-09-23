using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 基于SuperSocket.ClientEngine的Socket客户端实现
    /// 
    /// DI兼容性说明：
    /// 1. 通过无参构造函数支持依赖注入容器实例化
    /// 2. 实现ISocketClient接口便于接口注入
    /// 3. 实现IDisposable接口便于资源管理
    /// 4. 注册为单例服务以保持连接状态一致性
    /// 
    /// 线程安全说明：
    /// 1. 连接状态字段使用volatile关键字确保可见性
    /// 2. 事件处理方法中对共享状态的访问是安全的
    /// 
    /// 使用示例：
    /// // 通过依赖注入获取实例
    /// public class ClientCommunicationService
    /// {
    ///     private readonly ISocketClient _socketClient;
    ///     
    ///     public ClientCommunicationService(ISocketClient socketClient)
    ///     {
    ///         _socketClient = socketClient;
    ///     }
    ///     
    ///     public async Task<bool> ConnectAsync(string serverUrl, int port)
    ///     {
    ///         return await _socketClient.ConnectAsync(serverUrl, port);
    ///     }
    /// }
    /// </summary>
    public class SuperSocketClient : ISocketClient
    {
        private EasyClient<BizPackageInfo> _client;
        private volatile bool _isConnected; // 使用volatile确保线程可见性
        private string _serverIp;
        private int _port;

        /// <summary>
        /// 构造函数 - 支持依赖注入
        /// </summary>
        public SuperSocketClient()
        {
            _client = new EasyClient<BizPackageInfo>();
            _client.Initialize(new BizPipelineFilter());
            
            // 注册事件处理
            _client.Connected += OnClientConnected;
            _client.NewPackageReceived += OnPackageReceived;
            _client.Error += OnClientError;
            _client.Closed += OnClientClosed;
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected => _isConnected;

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
            _serverIp = serverUrl;
            _port = port;

            try
            {
                var connected = await _client.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverUrl), port));
                if (connected)
                {
                    // 等待连接状态更新（最多等待500ms）
                    for (int i = 0; i < 10 && !_isConnected; i++)
                    {
                        await Task.Delay(50, cancellationToken);
                    }
                }
                else
                {
                    _isConnected = false;
                }
                return _isConnected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接服务器失败: {ex.Message}");
                _isConnected = false;
                return false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (_isConnected)
            {
                _client.Close();
                _isConnected = false;
            }
        }

        /// <summary>
        /// 发送数据到服务器
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送任务</returns>
        public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            if (!_isConnected)
            {
                throw new InvalidOperationException("未连接到服务器");
            }

            _client.Send(data);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理客户端连接事件
        /// </summary>
        private void OnClientConnected(object sender, EventArgs e)
        {
            _isConnected = true;
        }

        /// <summary>
        /// 处理接收到数据包事件
        /// </summary>
        private void OnPackageReceived(object sender, PackageEventArgs<BizPackageInfo> e)
        {
            // 将接收到的数据包转换为字节数组并触发Received事件
            if (e.Package.Body != null)
            {
                Received?.Invoke(e.Package.Body);
            }
        }

        /// <summary>
        /// 处理客户端错误事件
        /// </summary>
        private void OnClientError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"Socket客户端错误: {e.Exception.Message}");
            // 连接错误时设置连接状态为false
            _isConnected = false;
        }

        /// <summary>
        /// 处理客户端关闭事件
        /// </summary>
        private void OnClientClosed(object sender, EventArgs e)
        {
            _isConnected = false;
            Closed?.Invoke(e);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_client != null)
            {
                _client.Close();
                _client = null;
            }
            _isConnected = false;
        }
    }
}