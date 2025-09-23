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
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using static RUINORERP.Server.Network.Core.ListenerOptions;
using RUINORERP.Server.Network.Commands;
using RUINORERP.PacketSpec.DI;

namespace RUINORERP.Server.Network.Core
{
    /// <summary>
    /// 网络服务器核心类，负责初始化和管理SuperSocket服务器实例
    /// 基于SuperSocket的实现，集成CommandDispatcher进行命令处理
    /// 提供完整的网络通信功能，包括会话管理、命令分发和消息处理
    /// </summary>
    public class NetworkServer
    {
        private readonly ILogger<NetworkServer> _logger;
        private readonly ISessionService _sessionManager;
        private readonly CommandDispatcher _commandDispatcher;
        private IHost _host;


        public NetworkServer(ILogger<NetworkServer> logger = null)
        {
            _logger = logger;
            
            // 使用全局服务提供者，避免创建多个SessionService实例
            _sessionManager = Program.ServiceProvider.GetRequiredService<ISessionService>();
            _commandDispatcher = new CommandDispatcher();
        }
        
        /// <summary>
        /// 为了保持向后兼容性，保留这个构造函数
        /// </summary>
        public NetworkServer(IServiceCollection services, ILogger<NetworkServer> logger = null) : this(logger)
        {
            // 注意：这里不再使用传入的services参数，因为我们现在使用全局的Program.ServiceProvider
        }

        public int Serverport { get; set; } = 7538;
        /// <summary>
        /// 启动服务器
        /// </summary>
        public async Task<IHost> StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 初始化命令调度器
                await _commandDispatcher.InitializeAsync();
                // 设置默认端口，以防配置读取失败

                // 声明一个外部作用域的变量，将在ConfigureServerOptions中被赋值
                ERPServerOptions serverOptions = null;

                _logger.LogInformation("正在初始化SuperSocket服务器...");
                _host = MultipleServerHostBuilder.Create()
                .AddServer<SuperSocketService<ServerPackageInfo>, ServerPackageInfo, PacketPipelineFilter>(builder =>
                {

                    builder.ConfigureServerOptions((ctx, config) =>
                       {
                           // 根据SuperSocket 2.0文档，配置应该从"serverOptions"节点读取
                           // 1. 首先尝试从"serverOptions"节点读取配置（符合SuperSocket官方文档）
                           ERPServerOptions localServerOptions = config.GetSection("serverOptions").Get<ERPServerOptions>();

                           // 2. 如果"serverOptions"节点不存在或配置无效，则尝试从"ERPServer"节点读取（向后兼容）
                           if (localServerOptions == null || !localServerOptions.Listeners.Any())
                           {
                               localServerOptions = config.GetSection("ERPServer").Get<ERPServerOptions>() ?? new ERPServerOptions();

                               // 3. 确保至少有一个监听器配置
                               localServerOptions.Validate();
                           }

                           // 设置外部作用域的变量，以便ConfigureSuperSocket回调可以使用
                           serverOptions = localServerOptions;

                           // 设置服务器端口和最大连接数
                           Serverport = localServerOptions.Listeners[0].Port;
                           (_sessionManager as SessionService)!.MaxSessionCount = localServerOptions.MaxConnectionCount;

                           // 返回配置节点，以便SuperSocket可以使用
                           return config.GetSection("serverOptions").Exists() ? config.GetSection("serverOptions") : config.GetSection("ERPServer");
                       })
                   .UseSession<SessionInfo>()
                   .UseCommand(commandOptions =>
                   {
                       // 注册SuperSocket命令适配器
                       commandOptions.AddCommand<SuperSocketCommandAdapter<IAppSession>>();
                       //commandOptions.AddCommand<SuperSocketCommandAdapter>();
                   })

                    .ConfigureSuperSocket(options =>
                    {
                        if (serverOptions != null)
                        {
                            // 使用从配置中读取的serverOptions对象设置SuperSocket选项
                            options.Listeners = serverOptions.Listeners.Select(l => new ListenOptions
                            {
                                Ip = l.Ip ?? "Any",  // 使用配置中的Ip，如果为空则使用默认值
                                Port = l.Port
                            }).ToList();
                            options.MaxPackageLength = serverOptions.MaxPackageLength;
                            options.ReceiveBufferSize = serverOptions.ReceiveBufferSize;
                            options.SendBufferSize = serverOptions.SendBufferSize;
                            options.ReceiveTimeout = serverOptions.ReceiveTimeout;
                            options.SendTimeout = serverOptions.SendTimeout;

                            // 如果设置了安全模式，则应用它
                            if (!string.IsNullOrEmpty(serverOptions.SecurityMode))
                            {
                                foreach (var listener in options.Listeners)
                                {
                                    listener.NoDelay = true; // 设置为true可以减少TCP延迟
                                    // 在实际应用中，根据安全模式配置SSL/TLS
                                    // 这里只是示例，实际配置可能需要证书等信息
                                }
                            }
                        }
                        else
                        {
                            // 如果没有有效的配置，使用默认值
                            options.Listeners = new List<ListenOptions> { new ListenOptions { Ip = "Any", Port = Serverport } };
                        }
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
                    .ConfigureLogging((hostCtx, loggingBuilder) =>
                    {
                        loggingBuilder.AddConsole();
                    })

                 .ConfigureServices((context, services) =>
                  {
                      // 注册核心服务 - 使用与全局相同的ISessionService实例
                      services.AddSingleton<ISessionService>(_sessionManager);

                      services.AddPacketSpecServices();
                      // 从全局服务提供者获取并注册CommandDispatcher
                      // 这解决了SuperSocketCommandAdapter无法解析CommandDispatcher的问题
                      var commandDispatcher = Program.ServiceProvider.GetRequiredService<CommandDispatcher>();
                      services.AddSingleton<CommandDispatcher>(commandDispatcher);


                      // 注册ICommandFactory服务，SuperSocketCommandAdapter也依赖它
                      var commandFactory = Program.ServiceProvider.GetRequiredService<ICommandFactory>();
                      services.AddSingleton<ICommandFactory>(commandFactory);

                      // 注册命令调度器和适配器
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
                Port = Serverport, // 从配置中获取的端口号
                ServerIp = "0.0.0.0", // 表示监听所有IP地址
                MaxConnections = (_sessionManager as SessionService)?.MaxSessionCount ?? 1000,
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