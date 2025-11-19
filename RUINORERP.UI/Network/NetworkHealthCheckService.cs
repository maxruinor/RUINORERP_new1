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
    /// 网络健康检查服务 - 提供网络连接状态检测和健康监控
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
            _healthCheckInterval = healthCheckInterval;
            _logger = logger;
            _isNetworkHealthy = true; // 初始状态假设网络正常
            _isRunning = false;
            _lastSuccessfulCheck = DateTime.Now;
            _consecutiveFailures = 0;

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
            _logger?.LogInformation("网络健康检查服务已启动，目标：{TargetHost}:{TargetPort}，检查间隔：{Interval}ms", 
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
            _logger?.LogInformation("网络健康检查服务已停止");
        }

        /// <summary>
        /// 执行网络健康检查
        /// </summary>
        private async Task PerformHealthCheckAsync()
        {
            if (!_isRunning) return;

            try
            {
                var isHealthy = await CheckNetworkConnectivityAsync();
                
                if (isHealthy)
                {
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
                        _logger?.LogWarning("网络连接出现问题，目标：{TargetHost}:{TargetPort}，连续失败次数：{Failures}", 
                            _targetHost, _targetPort, _consecutiveFailures);
                    }
                    
                    // 连续失败3次才认为网络真正出现问题，避免误判
                    if (_consecutiveFailures >= 3)
                    {
                        _isNetworkHealthy = false;
                        NetworkHealthChanged?.Invoke(false, $"网络连接失败，连续失败次数：{_consecutiveFailures}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "网络健康检查过程中发生异常");
                _consecutiveFailures++;
                
                if (_consecutiveFailures >= 3)
                {
                    _isNetworkHealthy = false;
                    NetworkHealthChanged?.Invoke(false, "网络健康检查异常");
                }
            }
        }

        /// <summary>
        /// 检查网络连通性
        /// </summary>
        /// <returns>网络是否连通</returns>
        private async Task<bool> CheckNetworkConnectivityAsync()
        {
            try
            {
                // 1. 首先检查本地网络接口状态
                if (!IsLocalNetworkAvailable())
                {
                    _logger?.LogWarning("本地网络接口不可用");
                    return false;
                }

                // 2. 执行TCP连接测试
                return await TestTcpConnectionAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "网络连通性检查失败");
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
        /// 测试TCP连接
        /// </summary>
        /// <returns>TCP连接是否成功</returns>
        private async Task<bool> TestTcpConnectionAsync()
        {
            using (var tcpClient = new TcpClient())
            {
                try
                {
                    // 设置较短的超时时间，避免长时间等待
                    tcpClient.SendTimeout = 3000;
                    tcpClient.ReceiveTimeout = 3000;
                    
                    // 异步连接，设置取消令牌
                    using (var cancellationTokenSource = new CancellationTokenSource(5000))
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
                    _logger?.LogWarning("TCP连接测试超时，目标：{TargetHost}:{TargetPort}", _targetHost, _targetPort);
                }
                catch (SocketException sockEx)
                {
                    _logger?.LogWarning(sockEx, "TCP连接测试失败，目标：{TargetHost}:{TargetPort}", _targetHost, _targetPort);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "TCP连接测试过程中发生异常，目标：{TargetHost}:{TargetPort}", _targetHost, _targetPort);
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