using System;
using System.Threading.Tasks;
using RUINORERP.TopServer.AuthorizationManagement;
using RUINORERP.TopServer.ConfigurationManagement;
using RUINORERP.TopServer.Network;
using RUINORERP.TopServer.ServerManagement;
using RUINORERP.TopServer.UserStatusMonitoring;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using ManagementCommands = RUINORERP.PacketSpec.Commands.ManagementCommands;

namespace RUINORERP.TopServer
{
    /// <summary>
    /// 应用管理核心类
    /// 负责初始化和管理各个模块
    /// </summary>
    public class AppManager
    {
        private static AppManager _instance;
        private static readonly object _lockObj = new();
        private NetworkServer _networkServer;
        private ServerManager _serverManager;
        private AuthorizationManager _authorizationManager;
        private UserManager _userManager;
        private TopServerConfigurationManager _configurationManager;

        /// <summary>
        /// 全局服务提供者
        /// </summary>
        public static IServiceProvider? ServiceProvider { get; set; }

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
                        _instance ??= new AppManager();
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
        public TopServerConfigurationManager ConfigurationManager => _configurationManager;

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
            _configurationManager = new TopServerConfigurationManager();
        }

        /// <summary>
        /// 注册模块间事件
        /// </summary>
        private void RegisterModuleEvents()
        {
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
        private void OnConfigurationUpdated(object? sender, ConfigurationUpdatedEventArgs e)
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
        /// 服务器实例注册事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnServerInstanceRegistered(object? sender, ServerInstanceRegisteredEventArgs e)
        {
            // 服务器实例注册时的处理逻辑
            Console.WriteLine($"服务器实例注册: {e.ServerInstance.InstanceName} ({e.ServerInstance.InstanceId})");
        }

        /// <summary>
        /// 服务器实例注销事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnServerInstanceUnregistered(object? sender, ServerInstanceUnregisteredEventArgs e)
        {
            // 服务器实例注销时的处理逻辑
            Console.WriteLine($"服务器实例注销: {e.ServerInstance.InstanceName} ({e.ServerInstance.InstanceId})");
        }

        /// <summary>
        /// 服务器实例状态变化事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnServerInstanceStatusChanged(object? sender, ServerInstanceStatusChangedEventArgs e)
        {
            // 服务器实例状态变化时的处理逻辑
            Console.WriteLine($"服务器实例状态变化: {e.ServerInstance.InstanceName} ({e.ServerInstance.InstanceId}) 从 {e.OldStatus} 变为 {e.ServerInstance.Status}");
        }

        /// <summary>
        /// 授权状态变化事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnAuthorizationStatusChanged(object? sender, AuthorizationStatusChangedEventArgs e)
        {
            // 授权状态变化时的处理逻辑
            Console.WriteLine($"授权状态变化: 实例ID={e.AuthorizationInfo.InstanceId} 从 {e.OldStatus} 变为 {e.AuthorizationInfo.Status}");
        }

        /// <summary>
        /// 授权到期提醒事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnAuthorizationExpiryReminder(object? sender, AuthorizationExpiryReminderEventArgs e)
        {
            // 授权到期提醒时的处理逻辑
            Console.WriteLine($"授权到期提醒: 实例ID={e.AuthorizationInfo.InstanceId} 距离到期还有 {e.DaysUntilExpiry} 天");
        }

        /// <summary>
        /// 用户登录事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUserLoggedIn(object? sender, UserLoggedInEventArgs e)
        {
            // 用户登录时的处理逻辑
            Console.WriteLine($"用户登录: 实例ID={e.InstanceId} 用户ID={e.UserInfo.UserId} 用户名={e.UserInfo.UserName}");
        }

        /// <summary>
        /// 用户登出事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUserLoggedOut(object? sender, UserLoggedOutEventArgs e)
        {
            // 用户登出时的处理逻辑
            Console.WriteLine($"用户登出: 实例ID={e.InstanceId} 用户ID={e.UserInfo.UserId} 用户名={e.UserInfo.UserName}");
        }

        /// <summary>
        /// 用户活动事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnUserActivity(object? sender, UserActivityEventArgs e)
        {
            // 用户活动时的处理逻辑
            Console.WriteLine($"用户活动: 实例ID={e.InstanceId} 用户ID={e.ActivityInfo.UserId} 活动类型={e.ActivityInfo.ActivityType} 描述={e.ActivityInfo.Description}");
        }

        #endregion
    }
}