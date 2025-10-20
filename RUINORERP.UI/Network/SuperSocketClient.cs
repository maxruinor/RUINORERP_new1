using RUINORERP.PacketSpec.Models.Core;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Krypton.Toolkit;
using MySqlX.XDevAPI;

namespace RUINORERP.UI.Network
{/// <summary>
 /// SuperSocket客户端实现
 /// </summary>
    public class SuperSocketClient : ISocketClient
    {
        private EasyClient<BizPackageInfo> _client;
        private volatile bool _isConnected; // 使用volatile确保线程可见性
        private string _serverIp;
        private int _port;
        private readonly ILogger<SuperSocketClient> _logger;

        /// <summary>
        /// 构造函数 - 支持依赖注入
        /// </summary>
        public SuperSocketClient(ILogger<SuperSocketClient> logger = null)
        {
            _client = new EasyClient<BizPackageInfo>();
            _client.Initialize(new BizPipelineFilter());
            _logger = logger;
            if (ClientIP == null)
            {
                ClientIP = GetClientIp();
            }
            // 注册事件处理
            _client.Connected += OnClientConnected;
            _client.NewPackageReceived += OnPackageReceived;
            _client.Error += OnClientError;
            _client.Closed += OnClientClosed;
        }


        /// <summary>
        /// 获取客户端IP地址
        /// 优先返回本机IPv4地址
        /// </summary>
        /// <returns>客户端IP地址字符串</returns>
        private string GetClientIp()
        {
            try
            {
                // 获取本机IPv4地址
                foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取客户端IP地址失败，使用默认值");
                return "127.0.0.1";
            }
        }

        public string ClientID { get; set; }

        public string ClientIP { get; set; }

        public string SessionID { get; set; }
        /// <summary>
        /// 连接状态 - 确保状态同步
        /// </summary>
        public bool IsConnected
        {
            get
            {
                // 检查Socket实际状态并同步内部状态
                var socketConnected = _client?.Socket?.Connected == true;
                
                // 如果Socket已断开但内部状态仍为连接，则同步更新
                if (!socketConnected && _isConnected)
                {
                    _isConnected = false;
                    _logger?.LogDebug("检测到Socket连接已断开，同步更新连接状态");
                }
                
                return _isConnected && socketConnected;
            }
        }

        /// <summary>
        /// 接收到数据包时触发的事件 - 直接传递PacketModel，避免重复序列化/反序列化
        /// </summary>
        public event Action<PacketModel> Received;

        /// <summary>
        /// 连接关闭时触发的事件
        /// </summary>
        public event Action<EventArgs> Closed;

        /// <summary>
        /// 连接到服务器 - 优化连接状态同步
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
                    
                    // 确保状态同步
                    _isConnected = connected && _client.Socket?.Connected == true;
                   
                }
                else
                {
                    _isConnected = false;
                    _logger?.LogWarning("连接服务器 {ServerIp}:{Port} 失败", serverUrl, port);
                }
                
                return _isConnected;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "连接服务器 {ServerIp}:{Port} 失败", serverUrl, port);
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
        /// 异步发送数据到服务器 - 增强状态同步检查
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送任务</returns>
        /// <exception cref="InvalidOperationException">当未连接到服务器或连接断开时抛出</exception>
        public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            // 双重检查连接状态
            if (!_isConnected || _client == null)
            {
                throw new InvalidOperationException("未连接到服务器");
            }

            try
            {
                // 检查连接是否有效
                if (_client.Socket == null || !_client.Socket.Connected)
                {
                    _isConnected = false;
                    _logger?.LogWarning("尝试发送数据时发现Socket连接已断开");
                    throw new InvalidOperationException("连接已断开");
                }

                // 检查取消令牌是否已请求取消
                cancellationToken.ThrowIfCancellationRequested();

                // 使用Task.Run将同步Send方法包装成异步操作
                // 确保在IO操作期间不会阻塞调用线程
                await Task.Run(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    _client.Send(data);
                }, cancellationToken).ConfigureAwait(false);
                
                _logger?.LogDebug("成功发送数据到服务器，数据长度: {DataLength}", data?.Length ?? 0);
            }
            catch (OperationCanceledException)
            {
                // 处理取消操作
                _logger?.LogWarning("发送数据操作被取消");
                throw;
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Writing is not allowed after writer was completed"))
            {
                // 处理管道写入器已完成的特定异常
                _isConnected = false;
                _logger?.LogWarning("发送数据失败：管道写入器已完成，连接已断开");
                throw new InvalidOperationException("连接已断开，无法发送数据");
            }
            catch (Exception ex)
            {
                // 记录其他发送异常
                _isConnected = false;
                _logger?.LogError(ex, "发送数据到服务器失败");
                throw;
            }
        }

        /// <summary>
        /// 处理客户端连接事件 - 确保状态同步
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
            if (e.Package?.Packet != null)
            {
                Received?.Invoke(e.Package.Packet);
            }
        }

        /// <summary>
        /// 处理客户端错误事件 - 确保状态同步
        /// </summary>
        private void OnClientError(object sender, ErrorEventArgs e)
        {
            // 连接错误时设置连接状态为false
            if (_isConnected)
            {
                _isConnected = false;
                _logger?.LogError(e.Exception, "客户端发生错误，连接状态已更新为断开");
            }
        }

        /// <summary>
        /// 处理客户端关闭事件 - 确保状态同步
        /// </summary>
        private void OnClientClosed(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                _isConnected = false;
            }
            Closed?.Invoke(e);
        }

        /// <summary>
        /// 释放资源 - 确保状态正确清理
        /// </summary>
        public void Dispose()
        {
            if (_client != null)
            {
                _client.Close();
                _client = null;
            }
            
            if (_isConnected)
            {
                _isConnected = false;
            }
        }
    }
}