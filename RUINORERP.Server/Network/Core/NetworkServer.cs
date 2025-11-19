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
using RUINORERP.PacketSpec.DI;
using System.Reflection;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.Server.Network.SuperSocket;
using System.IO;
using System.IO.Packaging;
using RUINORERP.PacketSpec.Serialization;
using System.Net.Sockets;

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
        private readonly CommandDispatcher _commandDispatcher;  // 修改为具体类型
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
        /// 主构造函数，支持完整的依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="sessionManager">会话管理器</param>
        /// <param name="commandDispatcher">命令调度器</param>
        public NetworkServer(ILogger<NetworkServer> logger, ISessionService sessionManager, CommandDispatcher commandDispatcher)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            LogDefaultPort();
        }

        /// <summary>
        /// 兼容构造函数，使用全局服务提供者（保留以支持现有代码）
        /// </summary>
        public NetworkServer(ILogger<NetworkServer> logger = null)
        {
            _logger = logger;

            // 使用全局服务提供者，避免创建多个SessionService实例
            _sessionManager = Program.ServiceProvider.GetRequiredService<ISessionService>();
            _commandDispatcher = Program.ServiceProvider.GetRequiredService<CommandDispatcher>();  // 修改为具体类型
            LogDefaultPort();
        }

        /// <summary>
        /// 为了保持向后兼容性，保留这个构造函数
        /// </summary>
        public NetworkServer(IServiceCollection services, ILogger<NetworkServer> logger = null) : this(logger)
        {
            // 注意：这里不再使用传入的services参数，因为我们现在使用全局的Program.ServiceProvider
            LogDefaultPort();
        }

        public int Serverport { get; set; } = new ListenerOptions().Port; // 使用ListenerOptions的默认端口，避免重复定义
        
        /// <summary>
        /// 构造函数中记录默认端口设置
        /// </summary>
        private void LogDefaultPort()
        {
            // 移除详细日志，只在需要时记录
        }

        /// <summary>
        /// 诊断配置加载问题（简化版本）
        /// </summary>
        private void DiagnoseConfiguration(IConfiguration config)
        {
            try
            {
                // 检查配置文件路径
                var currentDir = Directory.GetCurrentDirectory();
                var appSettingsPath = Path.Combine(currentDir, "appsettings.json");
                
                if (!File.Exists(appSettingsPath))
                {
                    _logger.LogError("appsettings.json文件不存在");
                    return;
                }
                
                // 检查serverOptions配置节
                var serverOptionsSection = config.GetSection("serverOptions");
                if (serverOptionsSection == null || !serverOptionsSection.GetChildren().Any())
                {
                    _logger.LogWarning("serverOptions配置节不存在或无子节点");
                    return;
                }
                
                var listenersSection = serverOptionsSection.GetSection("listeners");
                if (listenersSection == null || !listenersSection.GetChildren().Any())
                {
                    _logger.LogWarning("serverOptions.listeners配置节不存在或无子节点");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "配置诊断过程中发生错误");
            }
        }
        /// <summary>
        /// 启动服务器
        /// </summary>
        public async Task<IHost> StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 扫描RUINORERP.PacketSpec程序集以及其他相关程序集
                var packetSpecAssembly = Assembly.GetAssembly(typeof(PacketSpec.Commands.ICommandDispatcher));
                var serverAssembly = Assembly.GetExecutingAssembly();

                // 初始化命令调度器,里面会扫描并注册所有命令类型到命令调度器
                await _commandDispatcher.InitializeAsync(CancellationToken.None, packetSpecAssembly, serverAssembly);

                // 添加日志记录，检查注册的处理器数量
                var handlerCount = _commandDispatcher.HandlerCount;  // 直接使用具体类型属性
                // 移除详细日志
                // _logger.LogInformation($"命令处理器注册完成，当前已注册处理器数量: {handlerCount}");

                // 减少日志输出，仅在调试模式下显示已注册的处理器信息
                #if DEBUG
                LogRegisteredHandlers();
#endif
                ERPServerOptions serverOptions = null;

                // 设置默认端口，以防配置读取失败


                // 获取全局服务提供者，确保SuperSocket服务器使用与应用程序相同的服务
                var globalServiceProvider = Program.ServiceProvider;
                
                // 将全局服务提供者设置给命令调度器
                // 这确保命令处理器能够访问Startup中注册的所有服务
                if (globalServiceProvider != null && _commandDispatcher is RUINORERP.PacketSpec.Commands.CommandDispatcher commandDispatcherImpl)
                {   
                    commandDispatcherImpl.ServiceProvider = globalServiceProvider;
                    // 移除详细日志
                    // _logger.LogInformation("已将全局服务提供者设置给命令调度器");
                }
                
                _host = MultipleServerHostBuilder.Create()
                .ConfigureHostConfiguration(config =>
                {
                    // 配置主机配置，确保appsettings.json被正确加载
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    // 移除详细日志
                    // _logger.LogInformation("已配置主机配置，加载appsettings.json文件");
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // 配置应用程序配置，确保appsettings.json被正确加载
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    // 移除详细日志
                    // _logger.LogInformation("已配置应用程序配置，加载appsettings.json文件");
                })
                .AddServer<SuperSocketService<ServerPackageInfo>, ServerPackageInfo, PacketPipelineFilter>(builder =>
                {

                    builder.ConfigureServerOptions((ctx, config) =>
                       {
                           // 根据SuperSocket 2.0文档，配置应该从"serverOptions"节点读取
                           // 简化配置读取逻辑，统一从"serverOptions"节点读取配置
                           // 移除详细日志
                           // _logger.LogInformation("正在从配置文件中读取serverOptions配置...");
                           
                           // 执行配置诊断，帮助调试配置问题
                           DiagnoseConfiguration(config);
                           
                           // 检查是否存在SuperSocket配置节，避免配置冲突
                           var superSocketConfig = config.GetSection("SuperSocket");
                           if (superSocketConfig != null && superSocketConfig.GetChildren().Any())
                           {
                               _logger.LogWarning("检测到SuperSocket配置节，请注意可能存在配置冲突。优先使用serverOptions配置。");
                               // 移除详细日志
                               // var superSocketPort = superSocketConfig["Port"];
                               // var superSocketIP = superSocketConfig["IP"];
                               // _logger.LogInformation($"SuperSocket配置: IP={superSocketIP}, Port={superSocketPort}");
                           }
                           
                           try
                           {
                               // 尝试多种方式读取配置
                               var serverOptionsSection = config.GetSection("serverOptions");
                               if (serverOptionsSection != null && serverOptionsSection.GetChildren().Any())
                               {
                                   // 移除详细日志
                                   // _logger.LogInformation("找到serverOptions配置节，开始读取配置");
                                   
                                   // 方法1：直接绑定到ERPServerOptions
                                   serverOptions = serverOptionsSection.Get<ERPServerOptions>();
                                   
                                   // 如果方法1失败，尝试手动构建配置
                                   if (serverOptions == null || serverOptions.Listeners == null || serverOptions.Listeners.Count == 0)
                                   {
                                       // 移除详细日志
                                       // _logger.LogWarning("直接绑定配置失败，尝试手动读取监听器配置");
                                       serverOptions = new ERPServerOptions();
                                       
                                       // 手动读取监听器配置
                                       var listenersSection = serverOptionsSection.GetSection("listeners");
                                       if (listenersSection != null && listenersSection.GetChildren().Any())
                                       {
                                           var listeners = new List<ListenerOptions>();
                                           foreach (var listenerSection in listenersSection.GetChildren())
                                           {
                                               try
                                               {
                                                   var listener = new ListenerOptions();
                                                   
                                                   // 尝试多种属性名变体
                                                   var ip = listenerSection["ip"] ?? listenerSection["Ip"] ?? listenerSection["IP"] ?? "Any";
                                                   var portStr = listenerSection["port"] ?? listenerSection["Port"] ?? "3009";
                                                   listener.Ip = ip;
                                                   if (int.TryParse(portStr, out int port))
                                                   {
                                                       listener.Port = port;
                                                   }
                                                   else
                                                   {
                                                       // 使用ListenerOptions类中的默认值
                                                       listener.Port = new ListenerOptions().Port;
                                                       // 移除详细日志
                                                        _logger.LogWarning($"无法解析端口值 '{portStr}'，使用默认值{listener.Port}");
                                                   }
                                                   
                                                   listeners.Add(listener);
                                                   // 移除详细日志
                                                   _logger.LogInformation($"手动读取监听器: IP={listener.Ip}, Port={listener.Port}");
                                               }
                                               catch (Exception listenerEx)
                                               {
                                                   _logger.LogError(listenerEx, "读取单个监听器配置时发生错误");
                                               }
                                           }
                                           
                                           if (listeners.Count > 0)
                                           {
                                               serverOptions.Listeners = listeners;
                                               // 移除详细日志
                                               // _logger.LogInformation($"手动构建配置成功，监听器数量: {listeners.Count}");
                                           }
                                       }
                                   }
                                   
                                   if (serverOptions == null)
                                   {
                                       _logger.LogWarning("无法从配置文件读取serverOptions配置，使用默认配置");
                                       serverOptions = new ERPServerOptions();
                                   }
                                   else
                                   {
                                       // 移除详细日志
                                       // _logger.LogInformation($"成功读取serverOptions配置，监听器数量: {serverOptions.Listeners?.Count ?? 0}");
                                       if (serverOptions.Listeners?.Count > 0)
                                       {
                                           for (int i = 0; i < serverOptions.Listeners.Count; i++)
                                           {
                                               var listener = serverOptions.Listeners[i];
                                               _logger.LogInformation($"监听器 {i + 1}: IP={listener.Ip}, Port={listener.Port}");
                                           }
                                       }
                                   }
                               }
                               else
                               {
                                   _logger.LogWarning("未找到serverOptions配置节，使用默认配置");
                                   serverOptions = new ERPServerOptions();
                               }
                           }
                           catch (Exception ex)
                           {
                               _logger.LogError(ex, "读取serverOptions配置时发生错误，使用默认配置");
                               serverOptions = new ERPServerOptions();
                           }
                            
                           // 确保至少有一个监听器配置
                           serverOptions.Validate();

                           // 设置服务器端口和最大连接数
                           if (serverOptions != null && serverOptions.Listeners?.Count > 0)
                           {
                               Serverport = serverOptions.Listeners[0].Port;
                               // 移除详细日志
                               // _logger.LogInformation($"设置主监听端口为: {Serverport}");
                               
                               // 验证所有监听器的配置
                               for (int i = 0; i < serverOptions.Listeners.Count; i++)
                               {
                                   var listener = serverOptions.Listeners[i];
                                   if (string.IsNullOrEmpty(listener.Ip))
                                   {
                                       listener.Ip = "Any";
                                       // 移除详细日志
                                       // _logger.LogWarning($"监听器 {i + 1} IP地址为空，设置为默认值: Any");
                                   }
                                   if (listener.Port <= 0)
                                   {
                                       // 使用ListenerOptions类中的默认值
                                       listener.Port = new ListenerOptions().Port;
                                       // 移除详细日志
                                       // _logger.LogWarning($"监听器 {i + 1} 端口无效，设置为默认值: {listener.Port}");
                                   }
                                   // 移除详细日志
                                   // _logger.LogInformation($"验证监听器 {i + 1}: IP={listener.Ip}, Port={listener.Port}");
                               }
                           }
                           else
                           {
                               // 移除详细日志
                               // _logger.LogWarning("配置中没有有效的监听器，使用默认端口: 7538");
                           }
                           
                           (_sessionManager as SessionService)!.MaxSessionCount = serverOptions.MaxConnectionCount;

                           // 返回配置节点，以便SuperSocket可以使用
                           return config.GetSection("serverOptions");
                       })
                   .UseSession<SessionInfo>()
                   .UseCommand(commandOptions =>
                   {
                       // 注册SuperSocket命令适配器
                       commandOptions.AddCommand<SuperSocketCommandAdapter<IAppSession>>();
                   })

                    .ConfigureSuperSocket(options =>
                    {
                        if (serverOptions != null)
                        {
                            // 移除详细日志
                            // _logger.LogInformation("正在配置SuperSocket选项...");
                            
                            // 使用从配置中读取的serverOptions对象设置SuperSocket选项
                            options.Listeners = serverOptions.Listeners.Select(l => new ListenOptions
                            {
                                Ip = l.Ip ?? "Any",  // 使用配置中的Ip，如果为空则使用默认值
                                Port = l.Port
                            }).ToList();
                            
                            // 移除详细日志
                            // _logger.LogInformation($"SuperSocket将监听以下端口:");
                            // for (int i = 0; i < options.Listeners.Count; i++)
                            // {
                            //     var listener = options.Listeners[i];
                            //     _logger.LogInformation($"  监听器 {i + 1}: {listener.Ip}:{listener.Port}");
                            // }
                            
                            options.MaxPackageLength = serverOptions.MaxPackageLength;
                            options.ReceiveBufferSize = serverOptions.ReceiveBufferSize;
                            options.SendBufferSize = serverOptions.SendBufferSize;
                            options.ReceiveTimeout = serverOptions.ReceiveTimeout;
                            options.SendTimeout = serverOptions.SendTimeout;

                            // 如果设置了安全模式，则应用它
                            if (!string.IsNullOrEmpty(serverOptions.SecurityMode))
                            {
                                // 移除详细日志
                                // _logger.LogInformation($"启用安全模式: {serverOptions.SecurityMode}");
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
                            // 移除详细日志
                            // _logger.LogWarning("serverOptions为null，使用默认SuperSocket配置");
                            // 如果没有有效的配置，使用默认值
                            options.Listeners = new List<ListenOptions> { new ListenOptions { Ip = "Any", Port = Serverport } };
                        }
                    })

                   .UseSessionHandler(async (session) =>
                   {
                       // 会话连接
                       // 移除详细日志
                       // LogInfo($"客户端连接: {session.SessionID} from {session.RemoteEndPoint}");
                       await _sessionManager.AddSessionAsync(session);

                   }, async (session, reason) =>
                   {
                       await _sessionManager.RemoveSessionAsync(session.SessionID);
                   });
                })
                    .ConfigureLogging((hostCtx, loggingBuilder) =>
                    {
                        // 复制全局日志配置，确保日志一致性
                        if (globalServiceProvider != null)
                        {
                            // 获取全局日志工厂配置
                            var globalLoggerFactory = globalServiceProvider.GetService<ILoggerFactory>();
                            if (globalLoggerFactory != null)
                            {
                                loggingBuilder.AddConfiguration(hostCtx.Configuration.GetSection("Logging"));
                                // 添加全局日志提供者
                            }
                        }
                        loggingBuilder.AddConsole();
                    })

                 .ConfigureServices((context, services) =>
                  {
                      // 使用全局服务提供者的服务
                      if (globalServiceProvider != null)
                      {
                          // 从全局服务提供者复制所有注册的服务
                          // 这种方法可以确保SuperSocket服务器使用与应用程序相同的服务实例
                          CopyServicesFromGlobalProvider(globalServiceProvider, services);
                      }
                      else
                      {
                          // 回退方案：如果全局服务提供者不可用，使用本地服务
                          services.AddSingleton<ISessionService>(_sessionManager);
                          services.AddSingleton(_commandDispatcher);
                          
                          services.AddLogging(builder =>
                          {
                              builder.AddConsole();
                              builder.SetMinimumLevel(LogLevel.Information);
                          });
                      }
                  }).Build();
                
                // 启动服务器，使用StartAsync而不是RunAsync，这样不会阻塞线程
                await _host.StartAsync(cancellationToken);

                // 记录服务器启动时间
                StartTime = DateTime.Now;
                
                // 记录详细的启动成功信息，包括所有监听端口
                // 移除详细日志
                // if (serverOptions?.Listeners?.Count > 0)
                // {
                //     var listenerInfo = string.Join(", ", serverOptions.Listeners.Select(l => $"{l.Ip}:{l.Port}"));
                //     LogInfo($"网络服务器启动成功，监听端口: {listenerInfo} (主端口: {Serverport})");
                // }
                // else
                // {
                //     LogInfo($"网络服务器启动成功，监听端口: {Serverport} (使用默认配置)");
                // }

                return _host;
            }
            catch (Exception ex)
            {
                LogError($"启动服务器失败: {ex.Message}", ex);
                // 确保在启动失败时清理资源
                if (_host != null)
                {
                    _host.Dispose();
                    _host = null;
                }
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
                    // 移除详细日志
                    // LogInfo("正在停止网络服务器...");
                    await _host.StopAsync();
                    _host.Dispose();
                    _host = null;
                    
                    // 清除启动时间
                    StartTime = null;
                    
                    // 移除详细日志
                    // LogInfo("网络服务器已停止");
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
            //    StartTime = DateTime.Now, // 可以记录实际启动时间
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
                LastActivityTime = DateTime.Now,
                StartTime = StartTime // 添加启动时间
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
            public DateTime? StartTime { get; set; }

            public override string ToString()
            {
                return $"状态: {Status}, 端口: {Port}, IP: {ServerIp}, 最大连接: {MaxConnections}, 当前连接: {CurrentConnections}";
            }
        }





        private void LogInfo(string message)
        {
            _logger?.LogDebug($"[NetworkServer] {message}");
            Console.WriteLine($"[NetworkServer] INFO: {message}");
        }

        /// <summary>
        /// 从全局服务提供者复制服务到当前服务集合
        /// 确保SuperSocket服务器可以访问与应用程序相同的服务实例
        /// </summary>
        /// <param name="globalProvider">全局服务提供者</param>
        /// <param name="services">当前服务集合</param>
        private void CopyServicesFromGlobalProvider(IServiceProvider globalProvider, IServiceCollection services)
        {
            try
            {
                // 注册核心服务为单例，确保使用与全局相同的实例
                services.AddSingleton<ISessionService>(_sessionManager);
                services.AddSingleton(_commandDispatcher);
                
                // 注册全局服务提供者本身，以便在需要时可以访问所有全局服务
                services.AddSingleton(globalProvider);
                
                // 这里可以根据需要显式注册其他必要的服务
                // 例如：
                // services.AddSingleton(globalProvider.GetService<ILoggerFactory>());
                // services.AddSingleton(globalProvider.GetService<ISqlSugarClient>());
                // services.AddSingleton(globalProvider.GetService<IConfiguration>());
                
                _logger.LogInformation("已将全局服务提供者集成到SuperSocket服务器");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从全局服务提供者复制服务时出错");
            }
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

        /// <summary>
        /// 记录已注册的命令处理器信息
        /// </summary>
        private void LogRegisteredHandlers()
        {
            try
            {
                var handlers = _commandDispatcher.GetAllHandlers();  // 直接调用具体类型方法
                // 移除详细日志
                // _logger.LogInformation($"当前已注册的命令处理器数量: {handlers.Count}");

                foreach (var handler in handlers)
                {
                    // 移除详细日志
                    // _logger.LogInformation($"处理器: {handler.Name} (ID: {handler.HandlerId}), 状态: {handler.Status}");

                    // 记录支持的命令类型
                    if (handler.SupportedCommands != null)
                    {
                        var commandCodes = string.Join(", ", handler.SupportedCommands);
                        // 移除详细日志
                        // _logger.Debug($"  支持的命令类型: [{commandCodes}]");
                    }
                }
 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录已注册命令处理器信息时出错");
            }
        }
 
       
    }
}