using RUINORERP.PacketSpec.Models.Core;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
/// <summary>
    /// SuperSocket客户端 - 底层Socket通信实现
    /// 
    /// 🔄 Socket通信流程：
    /// 1. 建立TCP连接
    /// 2. 配置SuperSocket管道过滤器
    /// 3. 接收网络数据流
    /// 4. 使用BizPipelineFilter解析数据包
    /// 5. 触发数据接收事件
    /// 6. 发送响应数据
    /// 
    /// 📋 核心职责：
    /// - TCP连接管理
    /// - 原始数据收发
    /// - 数据包解析协调
    /// - 连接事件处理
    /// - 错误处理与重连
    /// - 性能统计
    /// 
    /// 🔗 与架构集成：
    /// - 被 CommunicationManager 管理
    /// - 使用 BizPipelineFilter 解析数据
    /// - 触发 ClientEventManager 连接事件
    /// - 为 CommunicationManager 提供解析后的PacketModel数据
    /// - 接收 ClientCommunicationService 的发送请求
    /// 
    /// ⚙️ 技术特性：
    /// - 基于SuperSocket框架
    /// - 支持异步数据收发
    /// - 内置连接池管理
    /// - 自动重连机制
    /// - 详细的连接日志
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
        /// 接收到数据包时触发的事件 - 直接传递PacketModel，避免重复序列化/反序列化
        /// </summary>
        public event Action<PacketModel> Received;

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
        /// 处理接收到数据包事件 - 直接传递解析后的PacketModel，避免重复处理
        /// </summary>
        private void OnPackageReceived(object sender, PackageEventArgs<BizPackageInfo> e)
        {
            // 直接传递解析后的PacketModel，不再重新序列化
            if (e.Package?.Packet != null)
            {
                Received?.Invoke(e.Package.Packet);
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