using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.ManagementServer.AuthorizationManagement;
using RUINORERP.ManagementServer.ConfigurationManagement;
using RUINORERP.ManagementServer.Network;
using RUINORERP.ManagementServer.ServerManagement;
using RUINORERP.ManagementServer.UserStatusMonitoring;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;

namespace RUINORERP.ManagementServer
{
    /// <summary>
    /// 应用管理核心类
    /// 负责初始化和管理各个模块
    /// </summary>
    public class AppManager
    {
        private static AppManager _instance;
        private static readonly object _lockObj = new object();
        private NetworkServer _networkServer;
        private ServerManager _serverManager;
        private AuthorizationManager _authorizationManager;
        private UserManager _userManager;
        private ConfigurationManager _configurationManager;

        /// <summary>
        /// 单例实例
        /// </summary>
        public static AppManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AppManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 网络服务器
        /// </summary>
        public NetworkServer NetworkServer => _networkServer;

        /// <summary>
        /// 服务器管理器
        /// </summary>
        public ServerManager ServerManager => _serverManager;

        /// <summary>
        /// 授权管理器
        /// </summary>
        public AuthorizationManager AuthorizationManager => _authorizationManager;

        /// <summary>
        /// 用户管理器
        /// </summary>
        public UserManager UserManager => _userManager;
        
        /// <summary>
        /// 配置管理器
        /// </summary>
        public ConfigurationManager ConfigurationManager => _configurationManager;

        /// <summary>
        /// 应用程序是否已初始化
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        private AppManager()
        {
        }

        /// <summary>
        /// 初始化应用程序
        /// </summary>
        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            // 初始化各个模块
            InitializeModules();
            // 注册模块间事件
            RegisterModuleEvents();
            // 启动网络服务器
            StartNetworkServer();

            IsInitialized = true;
        }

        /// <summary>
        /// 初始化各个模块
        /// </summary>
        private void InitializeModules()
        {
            // 初始化网络服务器
            _networkServer = new NetworkServer();
            // 初始化服务器管理器
            _serverManager = new ServerManager();
            // 初始化授权管理器
            _authorizationManager = new AuthorizationManager();
            // 初始化用户管理器
            _userManager = new UserManager();
            // 初始化配置管理器
            _configurationManager = new ConfigurationManager();
        }

        /// <summary>
        /// 注册模块间事件
        /// </summary>
        private void RegisterModuleEvents()
        {
            // 网络服务器事件
            _networkServer.ClientConnected += OnClientConnected;
            _networkServer.ClientDisconnected += OnClientDisconnected;
            _networkServer.MessageReceived += OnMessageReceived;

            // 服务器管理器事件
            _serverManager.ServerInstanceRegistered += OnServerInstanceRegistered;
            _serverManager.ServerInstanceUnregistered += OnServerInstanceUnregistered;
            _serverManager.ServerInstanceStatusChanged += OnServerInstanceStatusChanged;

            // 授权管理器事件
            _authorizationManager.AuthorizationStatusChanged += OnAuthorizationStatusChanged;
            _authorizationManager.AuthorizationExpiryReminder += OnAuthorizationExpiryReminder;

            // 用户管理器事件
            _userManager.UserLoggedIn += OnUserLoggedIn;
            _userManager.UserLoggedOut += OnUserLoggedOut;
            _userManager.UserActivity += OnUserActivity;
            
            // 配置管理器事件
            _configurationManager.ConfigurationUpdated += OnConfigurationUpdated;
        }
        
        /// <summary>
        /// 配置更新事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnConfigurationUpdated(object sender, ConfigurationUpdatedEventArgs e)
        {
            // 配置更新时的处理逻辑
            Console.WriteLine($"配置更新: 名称={e.ConfigurationInfo.Name}, 新值={e.ConfigurationInfo.Value}");
        }

        /// <summary>
        /// 启动网络服务器
        /// </summary>
        private void StartNetworkServer()
        {
            try
            {
                _networkServer.Initialize();
                _networkServer.StartAsync().Wait();
            }
            catch (Exception ex)
            {
                // 记录异常日志
                Console.WriteLine($"启动网络服务器失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 停止应用程序
        /// </summary>
        public void Shutdown()
        {
            if (!IsInitialized)
            {
                return;
            }

            // 停止网络服务器
            _networkServer.StopAsync().Wait();

            IsInitialized = false;
        }

        #region 事件处理方法

        /// <summary>
        /// 客户端连接事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            // 客户端连接时的处理逻辑
            Console.WriteLine($"客户端连接: {e.Session.SessionID}");
        }

        /// <summary>
        /// 客户端断开连接事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            // 客户端断开连接时的处理逻辑
            Console.WriteLine($"客户端断开连接: {e.Session.SessionID}");
            // 从服务器管理器中注销服务器实例
            _serverManager.UnregisterServerInstance(e.Session);
        }

        /// <summary>
        /// 收到消息事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            // 收到消息时的处理逻辑
            Console.WriteLine($"收到消息: SessionId={e.Session.SessionID}, CommandId={e.Packet.CommandId}");
            // 根据消息类型进行处理
            ProcessMessage(e.Session, e.Packet);
        }

        /// <summary>
        /// 服务器实例注册事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnServerInstanceRegistered(object sender, ServerInstanceRegisteredEventArgs e)
        {
            // 服务器实例注册时的处理逻辑
            Console.WriteLine($"服务器实例注册: {e.ServerInstance.InstanceName} ({e.ServerInstance.InstanceId})");
        }

        /// <summary>
        /// 服务器实例注销事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnServerInstanceUnregistered(object sender, ServerInstanceUnregisteredEventArgs e)
        {
            // 服务器实例注销时的处理逻辑
            Console.WriteLine($"服务器实例注销: {e.ServerInstance.InstanceName} ({e.ServerInstance.InstanceId})");
        }

        /// <summary>
        /// 服务器实例状态变化事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnServerInstanceStatusChanged(object sender, ServerInstanceStatusChangedEventArgs e)
        {
            // 服务器实例状态变化时的处理逻辑
            Console.WriteLine($"服务器实例状态变化: {e.ServerInstance.InstanceName} ({e.ServerInstance.InstanceId}) 从 {e.OldStatus} 变为 {e.ServerInstance.Status}");
        }

        /// <summary>
        /// 授权状态变化事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnAuthorizationStatusChanged(object sender, AuthorizationStatusChangedEventArgs e)
        {
            // 授权状态变化时的处理逻辑
            Console.WriteLine($"授权状态变化: 实例ID={e.AuthorizationInfo.InstanceId} 从 {e.OldStatus} 变为 {e.AuthorizationInfo.Status}");
        }

        /// <summary>
        /// 授权到期提醒事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnAuthorizationExpiryReminder(object sender, AuthorizationExpiryReminderEventArgs e)
        {
            // 授权到期提醒时的处理逻辑
            Console.WriteLine($"授权到期提醒: 实例ID={e.AuthorizationInfo.InstanceId} 距离到期还有 {e.DaysUntilExpiry} 天");
        }

        /// <summary>
        /// 用户登录事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUserLoggedIn(object sender, UserLoggedInEventArgs e)
        {
            // 用户登录时的处理逻辑
            Console.WriteLine($"用户登录: 实例ID={e.InstanceId} 用户ID={e.UserInfo.UserId} 用户名={e.UserInfo.UserName}");
        }

        /// <summary>
        /// 用户登出事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUserLoggedOut(object sender, UserLoggedOutEventArgs e)
        {
            // 用户登出时的处理逻辑
            Console.WriteLine($"用户登出: 实例ID={e.InstanceId} 用户ID={e.UserInfo.UserId} 用户名={e.UserInfo.UserName}");
        }

        /// <summary>
        /// 用户活动事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUserActivity(object sender, UserActivityEventArgs e)
        {
            // 用户活动时的处理逻辑
            Console.WriteLine($"用户活动: 实例ID={e.InstanceId} 用户ID={e.ActivityInfo.UserId} 活动类型={e.ActivityInfo.ActivityType} 描述={e.ActivityInfo.Description}");
        }

        #endregion

        #region 消息处理方法

        /// <summary>
        /// 处理收到的消息
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void ProcessMessage(ServerSession session, PacketModel packet)
        {
            if (packet == null)
            {
                return;
            }

            // 根据命令ID处理不同的消息
            switch (packet.CommandId)
            {
                case CommandId.RegisterServer:
                    HandleRegisterServer(session, packet);
                    break;
                case CommandId.Heartbeat:
                    HandleHeartbeat(session, packet);
                    break;
                case CommandId.ReportStatus:
                    HandleReportStatus(session, packet);
                    break;
                case CommandId.ReportUsers:
                    HandleReportUsers(session, packet);
                    break;
                case CommandId.ReportConfiguration:
                    HandleReportConfiguration(session, packet);
                    break;
                case CommandId.UpdateConfiguration:
                    HandleUpdateConfiguration(session, packet);
                    break;
                case CommandId.GetConfiguration:
                    HandleGetConfiguration(session, packet);
                    break;
                case CommandId.ReportUserActivity:
                    HandleReportUserActivity(session, packet);
                    break;
                default:
                    HandleUnknownCommand(session, packet);
                    break;
            }
        }

        /// <summary>
        /// 处理服务器注册命令
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void HandleRegisterServer(ServerSession session, PacketModel packet)
        {
            try
            {
                // 解析服务器注册信息
                var serverInfo = packet.Request as ServerInstanceInfo;
                if (serverInfo != null)
                {
                    // 注册服务器实例
                    _serverManager.RegisterServerInstance(session, serverInfo);
                    
                    // 创建默认授权信息
                    var authInfo = new AuthorizationInfo
                    {
                        InstanceId = serverInfo.InstanceId,
                        AuthorizationType = AuthorizationType.Trial,
                        StartTime = DateTime.Now,
                        ExpireTime = DateTime.Now.AddDays(30),
                        MaxUsers = 50,
                        MaxTransactions = 500000
                    };
                    _authorizationManager.SetAuthorizationInfo(serverInfo.InstanceId, authInfo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理服务器注册命令时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理心跳命令
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void HandleHeartbeat(ServerSession session, PacketModel packet)
        {
            try
            {
                // 更新服务器实例心跳
                _serverManager.UpdateHeartbeat(session);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理心跳命令时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理状态上报命令
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void HandleReportStatus(ServerSession session, PacketModel packet)
        {
            try
            {
                // 解析状态信息
                var statusInfo = packet.Request as ServerStatusInfo;
                if (statusInfo != null && session.ServerInstance != null)
                {
                    // 更新服务器实例状态
                    _serverManager.UpdateServerInstanceStatus(session.ServerInstance.InstanceId, statusInfo.Status);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理状态上报命令时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理用户信息上报命令
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void HandleReportUsers(ServerSession session, PacketModel packet)
        {
            try
            {
                // 解析用户信息列表
                var userInfos = packet.Request as List<UserInfo>;
                if (userInfos != null && session.ServerInstance != null)
                {
                    foreach (var userInfo in userInfos)
                    {
                        // 添加或更新用户信息
                        _userManager.AddOrUpdateUser(session.ServerInstance.InstanceId, userInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理用户信息上报命令时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理用户活动上报命令
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void HandleReportUserActivity(ServerSession session, PacketModel packet)
        {
            try
            {
                // 解析用户活动信息
                var activityInfo = packet.Request as UserActivityInfo;
                if (activityInfo != null && session.ServerInstance != null)
                {
                    // 更新用户活动
                    _userManager.UpdateUserActivity(session.ServerInstance.InstanceId, activityInfo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理用户活动上报命令时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理配置上报命令
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void HandleReportConfiguration(ServerSession session, PacketModel packet)
        {
            try
            {
                // 解析配置信息
                var configInfo = packet.Request as ServerConfigurationInfo;
                if (configInfo != null && session.ServerInstance != null)
                {
                    // 保存服务器配置
                    // 这里可以根据实际需求扩展，比如保存到数据库或缓存
                    Console.WriteLine($"收到服务器配置: {session.ServerInstance.InstanceName} - {configInfo.ConfigurationName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理配置上报命令时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理配置更新命令
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void HandleUpdateConfiguration(ServerSession session, PacketModel packet)
        {
            try
            {
                // 解析配置更新信息
                var configUpdate = packet.Request as ConfigurationUpdateInfo;
                if (configUpdate != null)
                {
                    // 这里可以根据实际需求扩展，比如广播配置更新到所有服务器实例
                    Console.WriteLine($"更新配置: {configUpdate.ConfigurationName} - {configUpdate.ConfigurationValue}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理配置更新命令时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理获取配置命令
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void HandleGetConfiguration(ServerSession session, PacketModel packet)
        {
            try
            {
                // 解析获取配置请求
                var configRequest = packet.Request as ConfigurationRequestInfo;
                if (configRequest != null)
                {
                    // 这里可以根据实际需求扩展，比如从数据库或缓存获取配置
                    Console.WriteLine($"获取配置: {configRequest.ConfigurationName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理获取配置命令时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理未知命令
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="packet">数据包</param>
        private void HandleUnknownCommand(ServerSession session, PacketModel packet)
        {
            // 记录未知命令
            Console.WriteLine($"收到未知命令: {packet.CommandId}");
        }

        /// <summary>
        /// 服务器配置信息类
        /// </summary>
        public class ServerConfigurationInfo
        {
            /// <summary>
            /// 配置名称
            /// </summary>
            public string ConfigurationName { get; set; }
            
            /// <summary>
            /// 配置值
            /// </summary>
            public string ConfigurationValue { get; set; }
            
            /// <summary>
            /// 配置描述
            /// </summary>
            public string Description { get; set; }
            
            /// <summary>
            /// 最后更新时间
            /// </summary>
            public DateTime LastUpdateTime { get; set; }
        }
        
        /// <summary>
        /// 配置更新信息类
        /// </summary>
        public class ConfigurationUpdateInfo
        {
            /// <summary>
            /// 配置名称
            /// </summary>
            public string ConfigurationName { get; set; }
            
            /// <summary>
            /// 配置值
            /// </summary>
            public string ConfigurationValue { get; set; }
            
            /// <summary>
            /// 更新说明
            /// </summary>
            public string UpdateDescription { get; set; }
        }
        
        /// <summary>
        /// 配置请求信息类
        /// </summary>
        public class ConfigurationRequestInfo
        {
            /// <summary>
            /// 配置名称
            /// </summary>
            public string ConfigurationName { get; set; }
        }
        
        /// <summary>
        /// 服务器状态信息类
        /// </summary>
        public class ServerStatusInfo
        {
            /// <summary>
            /// 服务器状态
            /// </summary>
            public ServerInstanceStatus Status { get; set; }
            
            /// <summary>
            /// CPU使用率
            /// </summary>
            public double CpuUsage { get; set; }
            
            /// <summary>
            /// 内存使用率
            /// </summary>
            public double MemoryUsage { get; set; }
            
            /// <summary>
            /// 磁盘使用率
            /// </summary>
            public double DiskUsage { get; set; }
            
            /// <summary>
            /// 活动连接数
            /// </summary>
            public int ActiveConnections { get; set; }
            
            /// <summary>
            /// 上报时间
            /// </summary>
            public DateTime ReportTime { get; set; }
        }

        #endregion
    }
}