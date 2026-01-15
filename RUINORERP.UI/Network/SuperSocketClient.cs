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
using RUINORERP.PacketSpec.Models.Common;
using System.Linq;

namespace RUINORERP.UI.Network
{/// <summary>
 /// SuperSocket客户端实现 - 集成网络健康检查
 /// </summary>
    public class SuperSocketClient : ISocketClient
    {
        private EasyClient<BizPackageInfo> _client;
        private volatile bool _isConnected; // 使用volatile确保线程可见性
        private string _serverIp;
        private int _port;
        private readonly ILogger<SuperSocketClient> _logger;
        private NetworkHealthCheckService _healthCheckService;
        private volatile bool _networkHealthWarningShown;

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
            _networkHealthWarningShown = false;
        }

        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string ServerIP => _serverIp;

        /// <summary>
        /// 服务器端口号
        /// </summary>
        public int ServerPort => _port;


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
        /// 获取客户端是否已连接到服务器
        /// 增强版：更全面的连接状态检测
        /// </summary>
        public bool IsConnected
        {
            get
            {
                try
                {
                    // 1. 快速检查基本状态标志
                    if (!_isConnected || _client == null)
                    {
                        _logger?.LogTrace("连接状态检查：基本状态标志为false");
                        return false;
                    }
                    
                    // 2. 检查Socket实例是否存在且已连接
                    if (_client.Socket == null || !_client.Socket.Connected)
                    {
                        _logger?.LogTrace("连接状态检查：Socket不存在或未连接");
                        return false;
                    }
                    
                    // 3. 检查Socket是否可用 - 更可靠的检测方法
                    try
                    {
                        // 检查Socket是否有挂起的错误
                        int errorCode = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        if (errorCode != 0)
                        {
                            _logger?.LogDebug("连接状态检查：检测到Socket错误，错误码: {ErrorCode}", errorCode);
                            return false;
                        }
                        
                        // 检查Socket是否可写（不发送数据，只检查状态）
                        if (!_client.Socket.Poll(0, SelectMode.SelectWrite))
                        {
                            // 检查是否可读，如果可读但没有数据，则连接可能已关闭
                            if (_client.Socket.Poll(0, SelectMode.SelectRead))
                            {
                                byte[] buffer = new byte[1];
                                if (_client.Socket.Receive(buffer, SocketFlags.Peek) == 0)
                                {
                                    _logger?.LogDebug("连接状态检查：检测到Socket已关闭（远程主机已关闭连接）");
                                    return false;
                                }
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                        // Socket异常，认为连接已断开
                        _logger?.LogDebug("连接状态检查：Socket状态检查异常: {ExceptionMessage}", ex.Message);
                        return false;
                    }
                    
                    // 所有检查通过，认为连接正常
                    _logger?.LogTrace("连接状态检查：所有检查通过，连接正常");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "检查连接状态时发生异常");
                    // 在出现异常时，认为连接已断开
                    return false;
                }
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
        /// 连接到服务器 - 集成网络健康检查
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
                // 检查是否已连接，如果已连接则先断开
                if (_isConnected || (_client?.Socket != null && _client.Socket.Connected))
                {
                    _logger?.LogDebug("检测到已存在连接，先断开旧连接再连接到新服务器");
                    try
                    {
                        await Disconnect();
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "断开现有连接时发生异常，将继续尝试新连接");
                        // 重置状态标志
                        _isConnected = false;
                    }
                }
                
                // 连接前进行网络健康检查
                if (_healthCheckService != null && !_healthCheckService.IsNetworkHealthy)
                {
                    _logger?.LogWarning("网络健康检查失败，延迟连接尝试，目标：{ServerIp}:{Port}", serverUrl, port);

                    // 尝试进行一次即时网络检查
                    var immediateCheck = await _healthCheckService.CheckOnceAsync();
                    if (!immediateCheck)
                    {
                        _logger?.LogError("网络连接不可用，取消连接尝试，目标：{ServerIp}:{Port}", serverUrl, port);
                        return false;
                    }
                }

                // 支持域名解析 - 先尝试解析为IP地址
                IPAddress ipAddress;
                try
                {
                    // 尝试直接解析为IP地址
                    ipAddress = IPAddress.Parse(serverUrl);
                }
                catch (FormatException)
                {
                    // 如果不是IP地址格式，尝试进行域名解析
                    try
                    {
                        var hostEntry = await Dns.GetHostEntryAsync(serverUrl);
                        ipAddress = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                        if (ipAddress == null)
                        {
                            throw new InvalidOperationException($"无法解析域名 '{serverUrl}' 到IPv4地址");
                        }
                    }
                    catch (Exception dnsEx)
                    {
                        throw new InvalidOperationException($"域名解析失败 '{serverUrl}': {dnsEx.Message}", dnsEx);
                    }
                }

                var connected = await _client.ConnectAsync(new IPEndPoint(ipAddress, port));

                if (connected)
                {
                    // 等待连接状态更新（最多等待500ms）
                    for (int i = 0; i < 10 && !_isConnected; i++)
                    {
                        await Task.Delay(50, cancellationToken);
                    }

                    // 确保状态同步
                    _isConnected = connected && _client.Socket?.Connected == true;

                    // 启动网络健康检查服务
                    if (_isConnected && _healthCheckService == null)
                    {
                        InitializeHealthCheckService(serverUrl, port);
                    }

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
        /// 初始化网络健康检查服务
        /// </summary>
        /// <param name="serverUrl">服务器地址</param>
        /// <param name="port">端口号</param>
        private void InitializeHealthCheckService(string serverUrl, int port)
        {
            try
            {
                _healthCheckService = new NetworkHealthCheckService(serverUrl, port, 30000, null);
                _healthCheckService.NetworkHealthChanged += OnNetworkHealthChanged;
                _healthCheckService.Start();
                _logger?.LogDebug("网络健康检查服务已启动，目标：{ServerIp}:{Port}", serverUrl, port);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化网络健康检查服务失败");
            }
        }

        /// <summary>
        /// 网络健康状态变化处理
        /// </summary>
        private void OnNetworkHealthChanged(bool isHealthy, string message)
        {
            if (!isHealthy)
            {
                if (!_networkHealthWarningShown)
                {
                    _logger?.LogWarning("网络健康状态异常：{Message}", message);
                    _networkHealthWarningShown = true;
                }
            }
            else
            {
                _logger?.LogDebug("网络健康状态已恢复：{Message}", message);
                _networkHealthWarningShown = false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns>断开连接是否成功</returns>
        public async Task<bool> Disconnect()
        {
            try
            {
                // 添加主动断开连接警告日志1
                _logger?.LogWarning("[主动断开连接] 开始断开与服务器的连接 - 服务器: {ServerIp}:{Port}", _serverIp, _port);
                
                _isConnected = false;
                _healthCheckService?.Stop();
                _healthCheckService?.Dispose();
                _healthCheckService = null;
                var closeResult = await _client.Close();
                _networkHealthWarningShown = false;
                
                _logger?.LogWarning("[主动断开连接] 已成功断开与服务器的连接 - 服务器: {ServerIp}:{Port}", _serverIp, _port);
                return closeResult;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "断开连接时发生异常 - 服务器: {ServerIp}:{Port}", _serverIp, _port);
                _logger?.LogWarning("[主动断开连接] 断开连接时发生异常 - 服务器: {ServerIp}:{Port}", _serverIp, _port);
                // 即使发生异常，也确保状态正确设置
                _isConnected = false;
                return false;
            }
        }

        /// <summary>
        /// 异步发送数据到服务器 - 集成网络健康检查，增强状态同步
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送任务</returns>
        /// <exception cref="InvalidOperationException">当未连接到服务器或连接断开时抛出</exception>
        public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            // 发送前检查网络健康状态
            if (_healthCheckService != null && !_healthCheckService.IsNetworkHealthy)
            {
                _logger?.LogDebug("网络健康检查失败，延迟发送数据尝试");

                // 尝试进行一次即时网络检查
                var immediateCheck = await _healthCheckService.CheckOnceAsync();
                if (!immediateCheck)
                {
                    throw new InvalidOperationException("网络连接不可用，无法发送数据");
                }
            }

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
                    // 在发送过程中再次检查连接状态
                    if (_client.Socket == null || !_client.Socket.Connected)
                    {
                        _isConnected = false;
                        _logger?.LogWarning("发送数据过程中发现Socket连接已断开");
                        throw new InvalidOperationException("连接已断开");
                    }
                    _client.Send(data);
                }, cancellationToken).ConfigureAwait(false);

                // 发送完成后再次检查连接状态
                if (_client.Socket == null || !_client.Socket.Connected)
                {
                    _isConnected = false;
                    _logger?.LogWarning("发送数据完成后发现Socket连接已断开");
                }

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
                _healthCheckService?.Stop();
                _healthCheckService?.Dispose();
                _healthCheckService = null;

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