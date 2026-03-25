using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket;
using SuperSocket.Server.Host;

namespace RUINORERP.TopServer.Network
{
    /// <summary>
    /// 网络服务器核心类
    /// 基于SuperSocket框架实现客户端-服务器通信
    /// </summary>
    public class NetworkServer
    {
        private readonly ILogger<NetworkServer> _logger;
        private IHost _host;

        /// <summary>
        /// 服务器启动时间
        /// </summary>
        public DateTime? StartTime { get; private set; }

        /// <summary>
        /// 服务器是否正在运行
        /// </summary>
        public bool IsRunning => _host != null;

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NetworkServer(ILogger<NetworkServer> logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // 读取配置文件
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // 读取服务器配置
                var serverOptions = new ServerOptions();
                config.GetSection("serverOptions").Bind(serverOptions);
                
                if (serverOptions.Listeners.Count > 0)
                {
                    ServerPort = serverOptions.Listeners[0].Port;
                }

                // 构建服务器主机
                _host = MultipleServerHostBuilder.Create()
                    .ConfigureHostConfiguration(configBuilder =>
                    {
                        configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                        configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        configBuilder.AddEnvironmentVariables();
                    })
                    .ConfigureAppConfiguration((hostingContext, configBuilder) =>
                    {
                        configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                        configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        configBuilder.AddEnvironmentVariables();
                    })
                    .ConfigureLogging((hostCtx, loggingBuilder) =>
                    {
                        loggingBuilder.AddConsole();
                    })
                    .Build();

                // 启动服务器
                await _host.StartAsync(cancellationToken);

                // 记录服务器启动时间
                StartTime = DateTime.Now;

                _logger?.LogInformation("网络服务器已启动，端口: {Port}", ServerPort);
            }
            catch (Exception ex)
            {
                // 确保在启动失败时清理资源
                if (_host != null)
                {
                    _host.Dispose();
                    _host = null;
                }

                _logger?.LogError(ex, "启动服务器失败");
                throw;
            }
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public async Task StopAsync()
        {
            try
            {
                if (_host != null)
                {
                    await _host.StopAsync();
                    _host.Dispose();
                    _host = null;

                    // 清除启动时间
                    StartTime = null;
                }

                _logger?.LogInformation("网络服务器已停止");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "停止服务器时出错");
            }
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        public ServerInfo GetServerInfo()
        {
            return new ServerInfo
            {
                Status = _host != null ? "运行中" : "已停止",
                Ports = new List<int> { ServerPort },
                ServerIps = new List<string> { "0.0.0.0" },
                MaxConnections = 1000,
                CurrentConnections = 0,
                TotalConnections = 0,
                PeakConnections = 0,
                LastActivityTime = DateTime.Now,
                StartTime = StartTime
            };
        }

        /// <summary>
        /// 服务器配置选项
        /// </summary>
        public class ServerOptions
        {
            public List<ListenOptions> Listeners { get; set; } = new List<ListenOptions> { new ListenOptions { Ip = "0.0.0.0", Port = 8080 } };
            public int MaxPackageLength { get; set; } = 1024 * 1024;
            public int ReceiveBufferSize { get; set; } = 4096;
            public int SendBufferSize { get; set; } = 4096;
            public int ReceiveTimeout { get; set; } = 0;
            public int SendTimeout { get; set; } = 0;
            public int MaxConnectionCount { get; set; } = 1000;
        }

        /// <summary>
        /// 监听选项
        /// </summary>
        public class ListenOptions
        {
            public string Ip { get; set; } = "0.0.0.0";
            public int Port { get; set; } = 8080;
        }

        /// <summary>
        /// 服务器信息类
        /// </summary>
        public class ServerInfo
        {
            public string Status { get; set; } = "已停止";
            public List<int> Ports { get; set; } = new List<int>();
            public List<string> ServerIps { get; set; } = new List<string>();
            public int MaxConnections { get; set; }
            public int CurrentConnections { get; set; }
            public int TotalConnections { get; set; }
            public int PeakConnections { get; set; }
            public DateTime LastActivityTime { get; set; }
            public DateTime? StartTime { get; set; }

            public int Port => Ports?.FirstOrDefault() ?? 0;
            public string ServerIp => ServerIps?.FirstOrDefault() ?? "0.0.0.0";

            public override string ToString()
            {
                var ports = string.Join(", ", Ports);
                var ips = string.Join(", ", ServerIps);
                return $"状态: {Status}, 端口: {ports}, IP: {ips}, 最大连接: {MaxConnections}, 当前连接: {CurrentConnections}";
            }
        }
    }
}