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
using RUINORERP.PacketSpec.DI;
using System.Reflection;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.Server.Network.SuperSocket;
using System.IO;
using System.IO.Packaging;
using RUINORERP.PacketSpec.Serialization;
using System.Net.Sockets;
using System.Net;
using System.Management;

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
        }

        /// <summary>
        /// 为了保持向后兼容性，保留这个构造函数
        /// </summary>
        public NetworkServer(IServiceCollection services, ILogger<NetworkServer> logger = null) : this(logger)
        {
        }

        public int Serverport { get; set; } // 初始化为0，等待配置文件读取后设置


        /// <summary>
        /// 启动服务器
        /// </summary>
        public async Task<IHost> StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 1. 注册验证检查（调试模式下跳过）
                await ValidateRegistrationOnStartupAsync();

                // 2. 读取配置文件，确保Serverport和configuredPorts使用的是配置文件中的值
                ERPServerOptions serverOptions = null;
                IConfiguration config = null;

                try
                {
                    config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

                    // 读取serverOptions配置
                    var serverOptionsSection = config.GetSection("serverOptions");
                    if (serverOptionsSection != null && serverOptionsSection.GetChildren().Any())
                    {
                        serverOptions = serverOptionsSection.Get<ERPServerOptions>();
                        if (serverOptions != null && serverOptions.Listeners.Count > 0)
                        {
                            // 设置Serverport为配置文件中的第一个监听器端口
                            Serverport = serverOptions.Listeners[0].Port;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "读取配置文件时发生错误，使用默认配置");
                    serverOptions = new ERPServerOptions();
                    Serverport = serverOptions.Listeners[0].Port;
                }

                // 在启动前检查所有配置的端口是否被占用
                var configuredPorts = GetConfiguredPorts(config, serverOptions);
                var occupiedPorts = new List<int>();
                
                // 检查所有端口占用情况
                foreach (var port in configuredPorts)
                {
                    if (await IsPortInUseAsync(port))
                    {
                        occupiedPorts.Add(port);
                    }
                }
                
                // 如果发现被占用的端口，提供详细错误信息
                if (occupiedPorts.Count > 0)
                {
                    // 只处理第一个被占用的端口（简化逻辑）
                    var errorMessage = BuildPortOccupiedMessage(occupiedPorts[0]);
                    LogError(errorMessage);
                    
                    // 同时在控制台显示彩色错误信息
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Diagnostics.Debug.WriteLine(errorMessage);
                    Console.ResetColor();
                    
                    throw new InvalidOperationException(errorMessage);
                }

                // 扫描RUINORERP.PacketSpec程序集以及其他相关程序集
                var packetSpecAssembly = Assembly.GetAssembly(typeof(PacketSpec.Commands.ICommandDispatcher));
                var serverAssembly = Assembly.GetExecutingAssembly();

                // 初始化命令调度器,里面会扫描并注册所有命令类型到命令调度器
                await _commandDispatcher.InitializeAsync(CancellationToken.None, packetSpecAssembly, serverAssembly).ConfigureAwait(false);

                // 添加日志记录，检查注册的处理器数量
                var handlerCount = _commandDispatcher.HandlerCount;  // 直接使用具体类型属性

                // 减少日志输出，仅在调试模式下显示已注册的处理器信息
#if DEBUG
                LogRegisteredHandlers();
#endif



                // 获取全局服务提供者，确保SuperSocket服务器使用与应用程序相同的服务
                var globalServiceProvider = Program.ServiceProvider;

                // 将全局服务提供者设置给命令调度器
                // 这确保命令处理器能够访问Startup中注册的所有服务
                if (globalServiceProvider != null && _commandDispatcher is RUINORERP.PacketSpec.Commands.CommandDispatcher commandDispatcherImpl)
                {
                    commandDispatcherImpl.ServiceProvider = globalServiceProvider;
                }

                _host = MultipleServerHostBuilder.Create()
                .ConfigureHostConfiguration(config =>
                {
                    // 配置主机配置，确保appsettings.json被正确加载
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // 配置应用程序配置，确保appsettings.json被正确加载
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .AddServer<SuperSocketService<ServerPackageInfo>, ServerPackageInfo, PacketPipelineFilter>(builder =>
                {

                    builder.ConfigureServerOptions((ctx, config) =>
                       {
                           // 根据SuperSocket 2.0文档，配置应该从"serverOptions"节点读取
                           // 简化配置读取逻辑，统一从"serverOptions"节点读取配置

                           // 确保至少有一个监听器配置
                           serverOptions.Validate();


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
                                loggingBuilder.Services.AddSingleton<ILoggerFactory>(globalLoggerFactory);
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

                          // 创建新的 ClientResponseHandler 实例
                          var logger = globalServiceProvider?.GetService<ILogger<ClientResponseHandler>>();
                          if (logger != null)
                          {
                              var clientResponseHandler = new ClientResponseHandler(logger);
                              services.AddSingleton<IClientResponseHandler>(clientResponseHandler);
                          }

                          //services.AddLogging(builder =>
                          //{
                          //    builder.AddConsole();
                          //    builder.SetMinimumLevel(LogLevel.Information);
                          //});
                      }
                  }).Build();

                // 启动服务器，使用StartAsync而不是RunAsync，这样不会阻塞线程
                await _host.StartAsync(cancellationToken);

                // 记录服务器启动时间
                StartTime = DateTime.Now;

                return _host;
            }
            catch (Exception ex)
            {
                // 确保在启动失败时清理资源
                if (_host != null)
                {
                    _host.Dispose();
                    _host = null;
                }

                // 检查是否是端口占用异常
                if (IsPortOccupiedException(ex))
                {
                    // 处理端口占用异常，记录详细信息
                    HandlePortOccupiedException(ex);
                    
                    // 创建包含详细解决方案的异常并向上抛出
                    var detailedErrorMessage = GetPortOccupiedDetailedMessage(ex);
                    throw new InvalidOperationException(detailedErrorMessage, ex);
                }
                else
                {
                    LogError($"启动服务器失败: {ex.Message}", ex);
                    // 对于其他类型的异常，也向上抛出，以便主窗体能够处理
                    throw;
                }
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
        /// 获取服务器配置信息
        /// </summary>
        public ServerInfo GetServerInfo()
        {
            var stats = _sessionManager.GetStatistics();
            return new ServerInfo
            {
                Status = _host != null ? "运行中" : "已停止",
                Ports = _configuredPorts?.ToList() ?? new List<int> { Serverport }, // 所有配置的端口号
                ServerIps = _configuredIps?.ToList() ?? new List<string> { "0.0.0.0" }, // 所有配置的IP地址
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
            public List<int> Ports { get; set; } // 支持多个监听端口
            public List<string> ServerIps { get; set; } // 支持多个监听IP
            public int MaxConnections { get; set; }
            public int CurrentConnections { get; set; }
            public int TotalConnections { get; set; }
            public int PeakConnections { get; set; }
            public DateTime LastActivityTime { get; set; }
            public DateTime? StartTime { get; set; }

            /// <summary>
            /// 向后兼容属性：返回第一个端口（如果有），否则返回0
            /// </summary>
            public int Port => Ports?.FirstOrDefault() ?? 0;

            /// <summary>
            /// 向后兼容属性：返回第一个IP地址（如果有），否则返回"0.0.0.0"
            /// </summary>
            public string ServerIp => ServerIps?.FirstOrDefault() ?? "0.0.0.0";

            public override string ToString()
            {
                var ports = string.Join(", ", Ports);
                var ips = string.Join(", ", ServerIps);
                return $"状态: {Status}, 端口: {ports}, IP: {ips}, 最大连接: {MaxConnections}, 当前连接: {CurrentConnections}";
            }
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
                // 增强参数检查
                if (globalProvider == null)
                {
                    _logger.LogWarning("全局服务提供者为空，无法复制服务");
                    return;
                }

                if (services == null)
                {
                    _logger.LogError("服务集合为空，无法复制服务");
                    return;
                }

                // 注册核心服务为单例，确保使用与全局相同的实例
                services.AddSingleton<ISessionService>(_sessionManager);
                services.AddSingleton(_commandDispatcher);

                // 注册客户端响应处理器
                var clientResponseHandler = globalProvider.GetService<IClientResponseHandler>();
                if (clientResponseHandler != null)
                {
                    services.AddSingleton<IClientResponseHandler>(clientResponseHandler);
                }

                // 注册全局服务提供者本身，以便在需要时可以访问所有全局服务
                services.AddSingleton(globalProvider);

                // 显式注册一些核心服务，提高可靠性
                try
                {
                    // 注册日志工厂
                    var loggerFactory = globalProvider.GetService<ILoggerFactory>();
                    if (loggerFactory != null)
                    {
                        services.AddSingleton(loggerFactory);
                    }

                    // 注册配置对象
                    var configuration = globalProvider.GetService<IConfiguration>();
                    if (configuration != null)
                    {
                        services.AddSingleton(configuration);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "注册额外服务时出错，但不影响核心功能");
                    // 继续执行，不中断服务启动
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从全局服务提供者复制服务时出错，但不影响服务器启动");
                // 继续执行，确保服务器能够启动
            }
        }

        /// <summary>
        /// 获取所有配置的端口，包括主端口和配置文件中的其他端口
        /// </summary>
        /// <param name="config">配置对象，可选</param>
        /// <param name="serverOptions">服务器配置对象，可选</param>
        /// <returns>所有配置的端口号列表</returns>
        // 存储配置的端口和IP地址，用于服务器信息展示
        private List<int> _configuredPorts;
        private List<string> _configuredIps;

        /// <summary>
        /// 获取所有配置的端口，包括主端口和配置文件中的其他端口
        /// </summary>
        /// <param name="config">配置对象，可选</param>
        /// <param name="serverOptions">服务器配置对象，可选</param>
        /// <returns>所有配置的端口号列表</returns>
        private List<int> GetConfiguredPorts(IConfiguration config = null, ERPServerOptions serverOptions = null)
        {
            var ports = new List<int>();
            _configuredIps = new List<string>();

            // 从配置文件中读取端口和IP
            try
            {
                // 如果没有传入配置对象，则创建一个
                if (config == null)
                {
                    try
                    {
                        config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // 改为可选，避免配置文件不存在导致崩溃
                            .Build();
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "创建配置对象时发生异常，将使用默认配置");
                        // 继续执行，使用默认配置
                    }
                }

                // 如果没有传入服务器配置对象，则尝试从配置文件中读取
                if (serverOptions == null)
                {
                    if (config != null)
                    {
                        try
                        {
                            var serverOptionsSection = config.GetSection("serverOptions");
                            if (serverOptionsSection != null && serverOptionsSection.Exists() && serverOptionsSection.GetChildren().Any())
                            {
                                serverOptions = serverOptionsSection.Get<ERPServerOptions>();
                                // 验证配置是否有效
                                if (serverOptions != null)
                                {
                                    serverOptions.Validate();
                                }
                            }
                            else
                            {
                                _logger?.LogDebug("未找到有效的服务器配置，将使用默认配置");
                                serverOptions = new ERPServerOptions();
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "从配置文件读取服务器配置时发生异常，将使用默认配置");
                            serverOptions = new ERPServerOptions();
                        }
                    }
                    else
                    {
                        _logger?.LogDebug("配置对象为空，将使用默认配置");
                        serverOptions = new ERPServerOptions();
                    }
                }

                // 首先添加serverOptions.listeners中的所有端口和IP
                if (serverOptions != null && serverOptions.Listeners != null && serverOptions.Listeners.Count > 0)
                {
                    foreach (var listener in serverOptions.Listeners)
                    {
                        if (listener == null)
                        {
                            _logger?.LogWarning("监听器配置为空，跳过");
                            continue;
                        }

                        if (listener.Port > 0 && !ports.Contains(listener.Port))
                        {
                            ports.Add(listener.Port);
                        }

                        // 记录IP地址
                        var ip = listener.Ip ?? "Any";
                        if (ip.Equals("Any", StringComparison.OrdinalIgnoreCase))
                        {
                            ip = "0.0.0.0";
                        }
                        if (!_configuredIps.Contains(ip))
                        {
                            _configuredIps.Add(ip);
                        }
                    }
                }
                else
                {
                    _logger?.LogInformation("未配置监听器，将使用默认配置");
                }

                // 如果没有配置任何端口，则添加默认端口
                if (ports.Count == 0)
                {
                    var defaultPort = new ListenOptions().Port;
                    ports.Add(defaultPort);
                    Serverport = defaultPort; // 更新Serverport
                    _configuredIps.Add("0.0.0.0"); // 默认监听所有IP
                    _logger?.LogInformation($"使用默认端口配置: {defaultPort}");
                }
                else
                {
                    // 更新Serverport为第一个配置的端口
                    Serverport = ports[0];
                    _logger?.LogInformation($"使用配置的端口: {string.Join(", ", ports)}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "读取配置文件中的端口信息时发生异常，将使用默认配置");

                // 发生异常时，确保清空之前可能的部分数据，使用完全的默认配置
                ports.Clear();
                _configuredIps.Clear();
                
                var defaultPort = new ListenOptions().Port;
                ports.Add(defaultPort);
                Serverport = defaultPort;
                _configuredIps.Add("0.0.0.0");
                
                _logger?.LogInformation($"发生异常后使用默认端口: {defaultPort}");
            }

            // 确保返回的端口列表不为空，且至少包含一个有效端口
            if (ports == null || ports.Count == 0)
            {
                var defaultPort = new ListenOptions().Port;
                ports = new List<int> { defaultPort };
                _configuredIps = new List<string> { "0.0.0.0" };
                Serverport = defaultPort;
                _logger?.LogError("端口列表为空，强制使用默认端口");
            }

            // 保存配置的端口，用于服务器信息展示
            _configuredPorts = ports;

            return ports;
        }

        /// <summary>
        /// 检查指定端口是否已被占用
        /// </summary>
        /// <param name="port">要检查的端口号</param>
        /// <returns>端口是否已被占用</returns>
        private async Task<bool> IsPortInUseAsync(int port)
        {
            try
            {
                // 使用TcpClient尝试连接端口，如果连接成功说明端口被占用
                using (var tcpClient = new System.Net.Sockets.TcpClient())
                {
                    // 尝试连接本地端口，设置超时时间
                    var connectTask = tcpClient.ConnectAsync(System.Net.IPAddress.Loopback, port);
                    var completedTask = await Task.WhenAny(connectTask, Task.Delay(TimeSpan.FromMilliseconds(500)));
                    var completed = completedTask == connectTask;

                    if (completed && tcpClient.Connected)
                    {
                        // 确保任务完成，避免资源泄漏
                        try
                        {
                            await connectTask;
                        }
                        catch
                        {
                            // 忽略连接异常
                        }
                        return true; // 端口已被占用 
                    }
                }

                // 如果本地连接失败，再检查所有网络接口
                var ipGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
                var tcpConnections = ipGlobalProperties.GetActiveTcpConnections();

                foreach (var connection in tcpConnections)
                {
                    if (connection.LocalEndPoint.Port == port)
                    {
                        return true; // 端口已被占用
                    }
                }

                return false; // 端口未被占用
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, $"检查端口 {port} 时发生异常");
                return false; // 发生异常时假设端口未被占用
            }
        }





        /// <summary>
        /// 生成端口占用的详细错误信息
        /// </summary>
        /// <param name="port">被占用的端口号</param>
        /// <param name="originalError">原始错误信息（可选）</param>
        /// <returns>包含详细解决方案的错误信息</returns>
        private string BuildPortOccupiedMessage(int port, string originalError = null)
        {
            var message = new System.Text.StringBuilder();
            message.AppendLine("==================================================");
            message.AppendLine("端口占用检测报告");
            message.AppendLine("==================================================\n");
            
            message.AppendLine($"检测结果：端口 {port} 已被其他进程占用！\n");
            
            // 获取占用端口的进程信息
            var processInfo = GetPortProcessInfo(port);
            if (!string.IsNullOrEmpty(processInfo))
            {
                message.AppendLine($"占用进程信息：");
                message.AppendLine($"{processInfo}\n");
            }
            
            message.AppendLine("解决方案：");
            message.AppendLine($"1. 查看端口占用情况：");
            message.AppendLine($"   netstat -ano | findstr :{port}");
            message.AppendLine($"\n2. 查看占用进程详情：");
            message.AppendLine($"   for /f \"tokens=5\" %a in ('netstat -ano ^| findstr :{port}') do tasklist | findstr %a");
            message.AppendLine($"\n3. 终止占用进程（谨慎操作）：");
            message.AppendLine($"   taskkill /PID [进程ID] /F");
            message.AppendLine($"\n==================================================");
            
            return message.ToString();
        }

        /// <summary>
        /// 获取端口占用进程的详细信息
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns>进程信息字符串</returns>
        private string GetPortProcessInfo(int port)
        {
            try
            {
                var processInfo = new System.Text.StringBuilder();
                
                // 使用netstat命令获取端口占用信息
                var startInfo = new ProcessStartInfo
                {
                    FileName = "netstat",
                    Arguments = $"-ano | findstr :{port}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                
                using (var process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        var output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                        
                        if (!string.IsNullOrEmpty(output))
                        {
                            var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var line in lines)
                            {
                                if (line.Contains($":{port}"))
                                {
                                    // 解析PID
                                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (parts.Length >= 5)
                                    {
                                        var pidStr = parts[4];
                                        if (int.TryParse(pidStr, out int pid))
                                        {
                                            // 获取进程信息
                                            var processDetails = GetProcessDetails(pid);
                                            processInfo.AppendLine($"   {line.Trim()}");
                                            processInfo.AppendLine($"   进程信息: {processDetails}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                return processInfo.Length > 0 ? processInfo.ToString() : "无法获取进程信息";
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取端口占用进程信息时发生异常");
                return "无法获取进程信息";
            }
        }

        /// <summary>
        /// 根据进程ID获取进程的详细信息
        /// </summary>
        /// <param name="pid">进程ID</param>
        /// <returns>进程详细信息</returns>
        private string GetProcessDetails(int pid)
        {
            try
            {
                using (var process = Process.GetProcessById(pid))
                {
                    string processName = process.ProcessName;
                    string processPath = string.Empty;
                    
                    // 尝试获取进程路径
                    try
                    {
                        processPath = process.MainModule?.FileName ?? "无法获取路径";
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, $"获取进程 {pid} 路径时发生异常，可能需要管理员权限");
                        processPath = "无法获取路径(需要管理员权限)";
                    }
                    
                    return $"PID: {pid}, 名称: {processName}, 路径: {processPath}";
                }
            }
            catch (ArgumentException)
            {
                return $"PID: {pid}, 进程已终止";
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, $"获取进程 {pid} 详情时发生异常");
                return $"PID: {pid}, 无法获取详情";
            }
        }
        
        /// <summary>
        /// 获取端口占用的详细错误信息，用于向上抛出异常
        /// </summary>
        /// <param name="ex">原始异常</param>
        /// <returns>包含详细解决方案的错误信息</returns>
        private string GetPortOccupiedDetailedMessage(Exception ex)
        {
            return "=========================================="  + Environment.NewLine +
                   "服务器启动失败：" + BuildPortOccupiedMessage(Serverport, ex.Message);
        }

        /// <summary>
        /// 检查异常是否为端口占用异常
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns>是否为端口占用异常</returns>
        private bool IsPortOccupiedException(Exception ex)
        {
            if (ex == null) return false;

            // 检查异常类型和消息
            if (ex is SocketException socketEx && socketEx.ErrorCode == 10048)
            {
                return true;
            }

            // 检查内部异常
            if (ex.InnerException != null)
            {
                return IsPortOccupiedException(ex.InnerException);
            }

            // 检查异常消息中是否包含端口占用的关键字
            var message = ex.Message?.ToLower() ?? "";
            return message.Contains("only one usage of each socket address") ||
                   message.Contains("address already in use") ||
                   message.Contains("端口") && message.Contains("占用") ||
                   message.Contains("failed to start any listener");
        }

        /// <summary>
        /// 处理端口占用异常，提供友好的错误信息和解决方案
        /// </summary>
        /// <param name="ex">异常对象</param>
        private void HandlePortOccupiedException(Exception ex)
        {
            var errorMessage = "=========================================="  + Environment.NewLine +
                               "服务器启动失败：" + BuildPortOccupiedMessage(Serverport, ex.Message);
        
            LogError(errorMessage);
        
            // 同时在控制台显示彩色错误信息
            Console.ForegroundColor = ConsoleColor.Red;
            System.Diagnostics.Debug.WriteLine(errorMessage);
            Console.ResetColor();
        }

        private void LogError(string message, Exception ex = null)
        {
            _logger?.LogError(ex, $"[NetworkServer] {message}");
            System.Diagnostics.Debug.WriteLine($"[NetworkServer] ERROR: {message}");
            if (ex != null)
            {
                System.Diagnostics.Debug.WriteLine($"[NetworkServer] Exception: {ex}");
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
                foreach (var handler in handlers)
                {
                    // 记录支持的命令类型
                    if (handler.SupportedCommands != null)
                    {
                        var commandCodes = string.Join(", ", handler.SupportedCommands);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录已注册命令处理器信息时出错");
            }
        }

        /// <summary>
        /// 服务器启动时验证注册信息
        /// </summary>
        private async Task ValidateRegistrationOnStartupAsync()
        {
            try
            {
                // 检查是否处于调试模式
                bool isDebugMode = false;
                
                // 尝试获取主窗体的调试模式状态
                var mainForm = System.Windows.Forms.Application.OpenForms.OfType<RUINORERP.Server.frmMainNew>().FirstOrDefault();
                if (mainForm != null)
                {
                    isDebugMode = mainForm.IsDebug;
                }
                
                // 调试模式下跳过注册验证
                if (isDebugMode)
                {
                    return;
                }

                // 获取注册服务
                var registrationService = Program.ServiceProvider.GetRequiredService<RUINORERP.Server.Services.IRegistrationService>();
                
                // 验证注册信息
                var registrationInfo = await registrationService.GetRegistrationInfoAsync();
                
                if (registrationInfo == null)
                {
                    _logger.LogError("无法获取注册信息，无法启动服务器");
                    throw new InvalidOperationException("无法获取注册信息，无法启动服务器");
                }
                
                if (!registrationInfo.IsRegistered)
                {
                    _logger.LogError("系统未注册，无法启动服务器");
                    throw new InvalidOperationException("系统未注册，无法启动服务器");
                }

                // 检查注册是否过期
                if (registrationService.IsRegistrationExpired(registrationInfo))
                {
                    _logger.LogError("系统注册已过期，无法启动服务器，到期时间: {ExpirationDate}", registrationInfo.ExpirationDate);
                    throw new InvalidOperationException($"系统注册已过期，到期时间: {registrationInfo.ExpirationDate:yyyy-MM-dd}");
                }

            }
            catch (InvalidOperationException ex)
            {
                // 重新抛出注册相关异常，阻止服务器启动
                _logger.LogError(ex, "注册验证失败，服务器启动被阻止");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证注册信息时发生未预期的错误");
                // 非致命异常，记录日志后继续启动
            }
        }


    }
}