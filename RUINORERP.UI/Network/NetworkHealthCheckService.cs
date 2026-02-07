using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 网络健康检查服务 - 优化版
    /// 提供网络连接状态检测和健康监控，针对局域网环境优化，减少误判
    /// </summary>
    public class NetworkHealthCheckService
    {
        private readonly ILogger<NetworkHealthCheckService> _logger;
        private readonly Timer _healthCheckTimer;
        private readonly int _healthCheckInterval;
        private readonly string _targetHost;
        private readonly int _targetPort;
        private volatile bool _isNetworkHealthy;
        private volatile bool _isRunning;
        private DateTime _lastSuccessfulCheck;
        private int _consecutiveFailures;
        private readonly object _stateLock = new object();
        
        // 用于检测连接状态变化的标志
        private bool _lastConnectionState;
        private DateTime _lastCheckTime = DateTime.MinValue;
        
        // 最小检查间隔，避免过于频繁的检查
        private const int MIN_CHECK_INTERVAL_MS = 5000;
        
        /// <summary>
        /// 网络健康状态变化事件
        /// </summary>
        public event Action<bool, string> NetworkHealthChanged;

        /// <summary>
        /// 获取当前网络健康状态
        /// </summary>
        public bool IsNetworkHealthy => _isNetworkHealthy;

        /// <summary>
        /// 获取最后一次成功检查的时间
        /// </summary>
        public DateTime LastSuccessfulCheck => _lastSuccessfulCheck;

        /// <summary>
        /// 获取连续失败次数
        /// </summary>
        public int ConsecutiveFailures => _consecutiveFailures;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="targetHost">目标主机地址</param>
        /// <param name="targetPort">目标端口</param>
        /// <param name="healthCheckInterval">健康检查间隔（毫秒）</param>
        /// <param name="logger">日志记录器</param>
        public NetworkHealthCheckService(string targetHost, int targetPort, int healthCheckInterval = 30000, ILogger<NetworkHealthCheckService> logger = null)
        {
            _targetHost = targetHost ?? throw new ArgumentNullException(nameof(targetHost));
            _targetPort = targetPort;
            _healthCheckInterval = Math.Max(healthCheckInterval, 10000); // 最小10秒间隔
            _logger = logger;
            _isNetworkHealthy = true; // 初始状态假设网络正常（乐观策略）
            _isRunning = false;
            _lastSuccessfulCheck = DateTime.Now;
            _consecutiveFailures = 0;
            _lastConnectionState = true;

            // 创建定时器，但不立即启动
            _healthCheckTimer = new Timer(async _ => await PerformHealthCheckAsync(), null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 启动网络健康检查
        /// </summary>
        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            _logger?.LogDebug("网络健康检查服务已启动，目标：{TargetHost}:{TargetPort}，检查间隔：{Interval}ms", 
                _targetHost, _targetPort, _healthCheckInterval);

            // 立即执行一次检查
            Task.Run(async () => await PerformHealthCheckAsync());

            // 启动定时器
            _healthCheckTimer.Change(_healthCheckInterval, _healthCheckInterval);
        }

        /// <summary>
        /// 停止网络健康检查
        /// </summary>
        public void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;
            _healthCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _logger?.LogDebug("网络健康检查服务已停止");
        }

        /// <summary>
        /// 执行网络健康检查 - 优化版
        /// 使用更宽松的检查策略，避免局域网环境下的误判
        /// </summary>
        private async Task PerformHealthCheckAsync()
        {
            if (!_isRunning) return;

            // 检查最小间隔，避免过于频繁的检查
            var now = DateTime.Now;
            var elapsedSinceLastCheck = (now - _lastCheckTime).TotalMilliseconds;
            if (elapsedSinceLastCheck < MIN_CHECK_INTERVAL_MS)
            {
                return;
            }
            _lastCheckTime = now;

            try
            {
                var isHealthy = await CheckNetworkConnectivityAsync();
                
                lock (_stateLock)
                {
                    if (isHealthy)
                    {
                        // 网络恢复
                        if (!_isNetworkHealthy)
                        {
                            _logger?.LogInformation("网络连接已恢复，目标：{TargetHost}:{TargetPort}", _targetHost, _targetPort);
                            NetworkHealthChanged?.Invoke(true, "网络连接已恢复");
                        }
                        
                        _isNetworkHealthy = true;
                        _lastSuccessfulCheck = DateTime.Now;
                        _consecutiveFailures = 0;
                    }
                    else
                    {
                        _consecutiveFailures++;
                        
                        if (_isNetworkHealthy)
                        {
                            _logger?.LogDebug("网络连接检查失败，目标：{TargetHost}:{TargetPort}，连续失败次数：{Failures}", 
                                _targetHost, _targetPort, _consecutiveFailures);
                        }
                        
                        // 优化：连续失败5次才认为网络真正出现问题（增加容错）
                        if (_consecutiveFailures >= 5)
                        {
                            if (_isNetworkHealthy)
                            {
                                _isNetworkHealthy = false;
                                _logger?.LogWarning("网络连接失败，连续失败次数：{Failures}，目标：{TargetHost}:{TargetPort}", 
                                    _consecutiveFailures, _targetHost, _targetPort);
                                NetworkHealthChanged?.Invoke(false, $"网络连接失败，连续失败次数：{_consecutiveFailures}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "网络健康检查过程中发生异常");
                lock (_stateLock)
                {
                    _consecutiveFailures++;
                    if (_consecutiveFailures >= 5 && _isNetworkHealthy)
                    {
                        _isNetworkHealthy = false;
                        NetworkHealthChanged?.Invoke(false, "网络健康检查异常");
                    }
                }
            }
        }

        /// <summary>
        /// 检查网络连通性 - 优化版
        /// 针对局域网环境优化，减少误判
        /// </summary>
        /// <returns>网络是否连通</returns>
        private async Task<bool> CheckNetworkConnectivityAsync()
        {
            try
            {
                // 1. 首先检查本地网络接口状态
                if (!IsLocalNetworkAvailable())
                {
                    _logger?.LogDebug("本地网络接口不可用");
                    return false;
                }

                // 2. 如果是127.0.0.1或localhost，直接认为网络正常（本地服务器）
                if (_targetHost == "127.0.0.1" || _targetHost.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                // 3. 如果是局域网地址（192.168.x.x, 10.x.x.x, 172.16-31.x.x），使用更宽松的检查
                if (IsPrivateNetworkAddress(_targetHost))
                {
                    // 局域网环境下，优先使用Ping检查而非TCP连接检查
                    // 因为TCP连接检查可能因为服务器负载或连接池限制而失败
                    var pingResult = await TestPingAsync();
                    if (pingResult)
                    {
                        return true;
                    }
                    
                    // Ping失败后，再尝试TCP连接
                    return await TestTcpConnectionAsync();
                }
                
                // 4. 外网地址，执行TCP连接测试
                return await TestTcpConnectionAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "网络连通性检查失败");
                return false;
            }
        }

        /// <summary>
        /// 检查是否为私有网络地址（局域网）
        /// </summary>
        private bool IsPrivateNetworkAddress(string host)
        {
            try
            {
                if (IPAddress.TryParse(host, out var ip))
                {
                    var bytes = ip.GetAddressBytes();
                    
                    // 10.0.0.0/8
                    if (bytes[0] == 10) return true;
                    
                    // 172.16.0.0/12
                    if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) return true;
                    
                    // 192.168.0.0/16
                    if (bytes[0] == 192 && bytes[1] == 168) return true;
                    
                    // 127.0.0.0/8 (回环地址)
                    if (bytes[0] == 127) return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 执行Ping测试
        /// </summary>
        private async Task<bool> TestPingAsync()
        {
            try
            {
                using (var ping = new Ping())
                {
                    // 局域网环境下，使用较短的超时时间
                    var reply = await ping.SendPingAsync(_targetHost, 2000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "Ping测试失败");
                return false;
            }
        }

        /// <summary>
        /// 检查本地网络接口是否可用
        /// </summary>
        /// <returns>本地网络是否可用</returns>
        private bool IsLocalNetworkAvailable()
        {
            try
            {
                // 检查是否有启用的网络接口
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                
                foreach (var networkInterface in networkInterfaces)
                {
                    // 排除回环接口和未启用的接口
                    if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                        networkInterface.OperationalStatus == OperationalStatus.Up)
                    {
                        // 检查是否有有效的IP地址
                        var ipProperties = networkInterface.GetIPProperties();
                        if (ipProperties.UnicastAddresses.Any(ip => 
                            ip.Address.AddressFamily == AddressFamily.InterNetwork &&
                            !IPAddress.IsLoopback(ip.Address)))
                        {
                            return true;
                        }
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查本地网络接口时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 测试TCP连接 - 优化版
        /// </summary>
        /// <returns>TCP连接是否成功</returns>
        private async Task<bool> TestTcpConnectionAsync()
        {
            using (var tcpClient = new TcpClient())
            {
                try
                {
                    // 优化：设置较短的超时时间，避免长时间等待
                    tcpClient.SendTimeout = 2000;
                    tcpClient.ReceiveTimeout = 2000;
                    
                    // 异步连接，设置取消令牌（3秒超时）
                    using (var cts = new CancellationTokenSource(3000))
                    {
                        await tcpClient.ConnectAsync(_targetHost, _targetPort);
                        
                        // 检查连接状态
                        if (tcpClient.Connected)
                        {
                            _logger?.LogDebug("TCP连接测试成功，目标：{TargetHost}:{TargetPort}", _targetHost, _targetPort);
                            return true;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger?.LogDebug("TCP连接测试超时，目标：{TargetHost}:{TargetPort}", _targetHost, _targetPort);
                }
                catch (SocketException sockEx)
                {
                    // 优化：减少日志级别，避免日志风暴
                    _logger?.LogDebug(sockEx, "TCP连接测试失败，目标：{TargetHost}:{TargetPort}", _targetHost, _targetPort);
                }
                catch (Exception ex)
                {
                    _logger?.LogDebug(ex, "TCP连接测试过程中发生异常，目标：{TargetHost}:{TargetPort}", _targetHost, _targetPort);
                }
                
                return false;
            }
        }

        /// <summary>
        /// 执行一次性网络检查
        /// </summary>
        /// <returns>网络是否健康</returns>
        public async Task<bool> CheckOnceAsync()
        {
            return await CheckNetworkConnectivityAsync();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Stop();
            _healthCheckTimer?.Dispose();
        }
    }
}
