using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.Server.Abstractions.Session;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Network.Commands.SuperSocket;
using RUINORERP.PacketSpec.Models.Core;
using SuperSocket.Server.Host;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Threading;

namespace RUINORERP.Server.Network.Core
{
    /// <summary>
    /// 网络服务器 - 基于SuperSocket的实现
    /// 集成CommandDispatcher进行命令处理，提供完整的网络通信功能
    /// </summary>
    public class NetworkServer
    {
        private readonly ILogger<NetworkServer> _logger;
        private readonly ISessionManager _sessionManager;
        private readonly CommandDispatcher _commandDispatcher;
        private IServiceCollection NetWorkServerServices { get; set; }
        private IHost _host;


        public NetworkServer(IServiceCollection services, ILogger<NetworkServer> logger = null)
        {
            _logger = logger;
            NetWorkServerServices = new ServiceCollection();

            // 确保传入的services不为null
            if (services != null)
            {
                foreach (var service in services)
                {
                    // 假设 service 是一个 ServiceDescriptor 对象
                    // 将 service 注册添加到 NetWorkServerServices 中
                    NetWorkServerServices.Add(service);
                }
            }

            // 从Startup中添加服务
            if (Startup.services != null)
            {
                foreach (var service in Startup.services)
                {
                    NetWorkServerServices.Add(service);
                }
            }

            // 创建依赖注入服务容器来解析SessionManager
            NetWorkServerServices.AddSingleton<ISessionManager, SessionManager>();
            NetWorkServerServices.AddSingleton<SessionManager>();

            // 使用NetWorkServerServices来构建服务提供器，而不是传入的services
            var serviceProvider = NetWorkServerServices.BuildServiceProvider();
            _sessionManager = serviceProvider.GetRequiredService<ISessionManager>();
            _commandDispatcher = new CommandDispatcher();
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        public async Task<IHost> StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 初始化命令调度器
                await _commandDispatcher.InitializeAsync();


                _host = MultipleServerHostBuilder.Create()
                .AddServer<SuperSocketService<ServerPackageInfo>, ServerPackageInfo, PacketPipelineFilter>(builder =>
                {
                    builder.ConfigureServerOptions((ctx, config) =>
                       {
                           //获取服务配置
                           var configSection = config.GetSection("ERPServer");

                           int maxConnections = config.GetSection("MaxConnectionCount").ObjToInt();
                           // 设置最大会话数
                           (_sessionManager as SessionManager)!.MaxSessionCount = maxConnections;
                           // LogInfo($"正在启动网络服务器，端口: {port}");
                           //tslblStatus.Text = "服务已启动。";
                           //if (IsDebug)
                           //{
                           //    PrintMsg($"port:{configSection.GetSection("listeners").GetSection("0").GetSection("port").Value}");
                           //   tslblStatus.Text = "服务已启动，端口：" + configSection.GetSection("listeners").GetSection("0").GetSection("port").Value;
                           //}
                           return configSection;
                       })
                   .UseSession<SessionInfo>()
                   .UseCommand(commandOptions =>
                   {
                       // 注册SuperSocket命令适配器
                       commandOptions.AddCommand<SuperSocketCommandAdapter<IAppSession>>();
                   })
                   .ConfigureSuperSocket(options =>
                   {
                       options.Name = "RUINORERP.Network.Server";
                       options.Listeners = new List<ListenOptions>
                       {
                           new ListenOptions
                           {
                               Ip = "Any",
                               Port = 3006 // 默认端口，可以从配置中读取
                           }
                       };
                       options.MaxPackageLength = 1024 * 1024; // 1MB
                       options.ReceiveBufferSize = 4096;
                       options.SendBufferSize = 4096;
                       options.ReceiveTimeout = 120000; // 2分钟
                       options.SendTimeout = 60000; // 1分钟
                   })
                   .UseSessionHandler(async (session) =>
                   {
                       // 会话连接
                       LogInfo($"客户端连接: {session.SessionID} from {session.RemoteEndPoint}");
                       await _sessionManager.AddSessionAsync(session);
                   }, async (session, reason) =>
                   {
                       // 会话断开
                       LogInfo($"客户端断开: {session.SessionID}, 原因: {reason}");
                       await _sessionManager.RemoveSessionAsync(session.SessionID);
                   });
                })
                 .ConfigureServices((context, services) =>
                  {
                      // 注册核心服务
                      services.AddSingleton<ISessionManager>(_sessionManager);
                      foreach (var service in NetWorkServerServices)
                      {
                          services.Add(service);
                      }

                      //services.AddSingleton<IUserService, UnifiedUserService>();
                      // services.AddSingleton<CacheService>();

                      // 注册命令调度器和适配器
                      // services.AddSingleton(_commandDispatcher);
                      services.AddSingleton<SuperSocketCommandAdapter<IAppSession>>();
                      services.AddLogging(builder =>
                      {
                          builder.AddConsole();
                          builder.SetMinimumLevel(LogLevel.Information);
                      });
                      // 注册命令处理器 - 这些处理器会被CommandDispatcher自动发现
                  }).Build();
                // 启动服务器，使用StartAsync而不是RunAsync，这样不会阻塞线程
                await _host.StartAsync(cancellationToken);

                return _host;
            }
            catch (Exception ex)
            {
                LogError($"启动服务器失败: {ex.Message}", ex);
                return null;
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
                    LogInfo("正在停止网络服务器...");
                    await _host.StopAsync();
                    _host.Dispose();
                    _host = null;
                    LogInfo("网络服务器已停止");
                }

                (_sessionManager as IDisposable)?.Dispose();
            }
            catch (Exception ex)
            {
                LogError($"停止服务器时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取服务器状态
        /// </summary>
        public object GetStatus()
        {
            var sessionStats = _sessionManager.GetStatistics();
            //return new
            //{
            //    Status = _host != null ? "Running" : "Stopped",
            //    StartTime = DateTime.UtcNow, // 可以记录实际启动时间
            //    ActiveSessions = sessionStats.ActiveConnections,
            //    TotalConnections = sessionStats.TotalConnections,
            //    LastUpdate = sessionStats.LastUpdateTime
            //};
            return sessionStats;
        }

        /// <summary>
        /// 获取服务器配置信息
        /// </summary>
        public ServerInfo GetServerInfo()
        {
            var stats = _sessionManager.GetStatistics();
            return new ServerInfo
            {
                Status = _host != null ? "运行中" : "已停止",
                Port = 3006, // 从配置中获取的端口号
                ServerIp = "0.0.0.0", // 表示监听所有IP地址
                MaxConnections = (_sessionManager as SessionManager)?.MaxSessionCount ?? 1000,
                CurrentConnections = stats.CurrentConnections,
                TotalConnections = stats.TotalConnections,
                PeakConnections = stats.PeakConnections,
                LastActivityTime = DateTime.Now
            };
        }

        /// <summary>
        /// 服务器信息类
        /// </summary>
        public class ServerInfo
        {
            public string Status { get; set; }
            public int Port { get; set; }
            public string ServerIp { get; set; }
            public int MaxConnections { get; set; }
            public int CurrentConnections { get; set; }
            public int TotalConnections { get; set; }
            public int PeakConnections { get; set; }
            public DateTime LastActivityTime { get; set; }

            public override string ToString()
            {
                return $"状态: {Status}, 端口: {Port}, IP: {ServerIp}, 最大连接: {MaxConnections}, 当前连接: {CurrentConnections}";
            }
        }

        /// <summary>
        /// 广播消息到所有客户端
        /// </summary>
        public async Task<int> BroadcastAsync<T>(uint command, T data)
        {
            try
            {
                var packet = new PacketModel
                {
                    //Command = command,
                    //SessionId = "SERVER_BROADCAST",
                    //Direction = PacketDirection.Response,
                    //Status = PacketStatus.Completed,
                    PacketId = GeneratePacketId()
                };

                // 设置数据包数据
                if (data != null)
                {
                    packet.Extensions["Data"] = data;
                }

                return await _sessionManager.BroadcastMessageAsync(packet);
            }
            catch (Exception ex)
            {
                LogError($"广播消息失败: {ex.Message}", ex);
                return 0;
            }
        }

        /// <summary>
        /// 发送消息到指定客户端
        /// </summary>
        //public async Task<bool> SendToClientAsync<T>(string sessionId, uint command, T data)
        //{
        //    try
        //    {
        //        var packet = new PacketModel
        //        {
        //            //Command = command,
        //            //SessionId = sessionId,
        //            //Direction = PacketDirection.Response,
        //            //Status = PacketStatus.Completed,
        //            PacketId = GeneratePacketId()
        //        };

        //        // 设置数据包数据
        //        if (data != null)
        //        {
        //            packet.Extensions["Data"] = data;
        //        }

        //        return await _sessionManager.SendPacketAsync(sessionId, packet);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError($"发送消息到客户端失败: SessionId={sessionId}, 错误: {ex.Message}", ex);
        //        return false;
        //    }
        //}

        /// <summary>
        /// 生成唯一的数据包ID
        /// </summary>
        private string GeneratePacketId()
        {
            return $"PKT_{Guid.NewGuid().ToString().Substring(0, 10)}_{DateTime.Now.Ticks.ToString().Substring(8)}";
        }

        private void LogInfo(string message)
        {
            _logger?.LogInformation($"[NetworkServer] {message}");
            Console.WriteLine($"[NetworkServer] INFO: {message}");
        }

        private void LogError(string message, Exception ex = null)
        {
            _logger?.LogError(ex, $"[NetworkServer] {message}");
            Console.WriteLine($"[NetworkServer] ERROR: {message}");
            if (ex != null)
            {
                Console.WriteLine($"[NetworkServer] Exception: {ex}");
            }
        }
    }
}