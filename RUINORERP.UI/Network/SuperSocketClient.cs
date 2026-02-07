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
{    /// <summary>
 /// SuperSocket客户端实现 - 集成网络健康检查
 /// 状态管理：仅管理底层Socket连接状态,不保留应用层状态
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
        /// 连接状态变更事件 - 供上层订阅以同步状态
        /// </summary>
        public event Action<bool> ConnectionStateChanged;

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
        /// 优化版：减少不必要的检查，提高性能，避免频繁触发状态变更事件
        /// </summary>
        public bool IsConnected
        {
            get
            {
                try
                {
                    // 1. 快速检查客户端和Socket实例
                    if (_client == null || _client.Socket == null)
                    {
                        // 静默返回，不记录日志（避免日志风暴）
                        return false;
                    }

                    // 2. 直接检查Socket.Connected属性（最快的方式）
                    bool socketConnected = _client.Socket.Connected;

                    // 3. 快速返回，不进行复杂的Poll检查（避免性能问题）
                    // 复杂的检查在发送数据前进行即可
                    if (socketConnected)
                    {
                        // 检查状态标志是否与Socket状态一致
                        if (!_isConnected)
                        {
                            _logger?.LogDebug("连接状态同步：Socket.Connected=true但_isConnected=false，正在同步状态");
                            _isConnected = true;
                            ConnectionStateChanged?.Invoke(true);
                        }
                        return true;
                    }
                    else
                    {
                        // Socket.Connected为false
                        if (_isConnected)
                        {
                            _logger?.LogDebug("连接状态同步：Socket.Connected=false，正在同步状态");
                            _isConnected = false;
                            ConnectionStateChanged?.Invoke(false);
                        }
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // 减少日志级别，避免日志风暴
                    _logger?.LogDebug(ex, "检查连接状态时发生异常");
                    // 在出现异常时，认为连接已断开
                    if (_isConnected)
                    {
                        _isConnected = false;
                        ConnectionStateChanged?.Invoke(false);
                    }
                    return false;
                }
            }
        }
        
        /// <summary>
        /// 验证Socket连接是否真正可用（在发送数据前调用）
        /// </summary>
        /// <returns>连接是否可用</returns>
        public bool ValidateConnection()
        {
            try
            {
                if (_client == null || _client.Socket == null)
                    return false;
                
                if (!_client.Socket.Connected)
                    return false;
                
                // 进行更深入的检查
                if (!_client.Socket.Poll(0, SelectMode.SelectWrite))
                {
                    if (_client.Socket.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buffer = new byte[1];
                        if (_client.Socket.Receive(buffer, SocketFlags.Peek) == 0)
                        {
                            _logger?.LogDebug("验证连接：检测到Socket已关闭");
                            if (_isConnected)
                            {
                                _isConnected = false;
                                ConnectionStateChanged?.Invoke(false);
                            }
                            return false;
                        }
                    }
                }
                
                return true;
            }
            catch (SocketException ex)
            {
                _logger?.LogDebug(ex, "验证连接时发生Socket异常");
                if (_isConnected)
                {
                    _isConnected = false;
                    ConnectionStateChanged?.Invoke(false);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "验证连接时发生异常");
                return false;
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
                    // 立即检查Socket状态并设置连接状态
                    // 不再依赖事件触发，直接根据Socket.Connected状态设置
                    _isConnected = _client.Socket?.Connected == true;

                    if (_isConnected)
                    {
                        _logger?.LogDebug("连接成功，已更新连接状态为true");
                        ConnectionStateChanged?.Invoke(true);

                        // 启动网络健康检查服务
                        if (_healthCheckService == null)
                        {
                            InitializeHealthCheckService(serverUrl, port);
                        }
                    }
                    else
                    {
                        _logger?.LogWarning("EasyClient返回连接成功，但Socket.Connected为false");
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
        /// 处理客户端连接事件
        /// 仅用于确认连接状态，不作为主要的状态设置机制
        /// </summary>
        private void OnClientConnected(object sender, EventArgs e)
        {
            // 如果当前已经是连接状态，则忽略
            if (_isConnected)
            {
                _logger?.LogTrace("连接状态已为true，忽略OnClientConnected事件");
                return;
            }

            // 验证Socket是否真的连接
            if (_client?.Socket?.Connected == true)
            {
                _isConnected = true;
                _logger?.LogDebug("通过OnClientConnected事件更新连接状态为true");
                ConnectionStateChanged?.Invoke(true);
                _logger?.LogDebug("底层Socket连接已建立");
            }
            else
            {
                _logger?.LogWarning("OnClientConnected事件触发，但Socket未连接");
            }
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
                ConnectionStateChanged?.Invoke(false);
                _logger?.LogError(e.Exception, "客户端发生错误，连接状态已更新为断开");
            }
        }

        /// <summary>
        /// 处理客户端关闭事件
        /// 立即更新连接状态
        /// </summary>
        private void OnClientClosed(object sender, EventArgs e)
        {
            // 立即更新连接状态，无论当前状态如何
            if (_isConnected)
            {
                _isConnected = false;
                _logger?.LogDebug("通过OnClientClosed事件更新连接状态为false");
                ConnectionStateChanged?.Invoke(false);
                _logger?.LogDebug("底层Socket连接已关闭");
            }
            else
            {
                _logger?.LogTrace("连接状态已为false，仅触发Closed事件");
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