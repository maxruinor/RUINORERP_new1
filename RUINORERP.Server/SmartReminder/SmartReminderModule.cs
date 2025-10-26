using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.SmartReminder.InvReminder;
using RUINORERP.Server.SmartReminder.ReminderRuleStrategy;
using StackExchange.Redis;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Services;
using System.Net.Sockets;
using System.Threading;
using RUINORERP.Extensions.Redis;
using RUINORERP.Extensions;

namespace RUINORERP.Server.SmartReminder
{
    public class SmartReminderModule : Module
    {
        private readonly IConfiguration _configuration;
        private ConnectionMultiplexer _redisMultiplexer;
        private readonly ILogger _logger;

        // 添加公共无参数构造函数以满足RegisterModule方法的要求
        public SmartReminderModule()
        {
            // 无参数构造函数，在使用RegisterModule时会调用此构造函数
        }

        // 保留现有的带参数构造函数
        public SmartReminderModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // 带日志器的构造函数
        public SmartReminderModule(IConfiguration configuration, ILogger<SmartReminderModule> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // 注册工作流引擎
            builder.RegisterType<WorkflowHost>()
                .As<IWorkflowHost>()
                .SingleInstance();

            // 注册智能提醒监控器
            builder.RegisterType<SmartReminderMonitor>()
                .As<ISmartReminderMonitor>()
                .SingleInstance();

            // 注册通知服务
            builder.RegisterType<NotificationService>()
                .As<INotificationService>()
                .SingleInstance();

            // 注册提醒策略
            builder.RegisterType<SafetyStockStrategy>()
                .As<IReminderStrategy>()
                .SingleInstance();
            builder.RegisterType<SalesTrendStrategy>()
                .As<IReminderStrategy>()
                .SingleInstance();

            // 注册健康检查
            builder.RegisterType<SmartReminderHealthCheck>()
                .As<IHealthCheck>()
                .SingleInstance();

            // 注册背景服务
            builder.RegisterType<SmartReminderService>()
                .As<IHostedService>()
                .SingleInstance();

            // 注册Redis连接
            RegisterRedis(builder);
            
            // 注册单位工作管理器
            builder.RegisterType<UnitOfWorkManage>()
                .As<IUnitOfWorkManage>()
                .SingleInstance();
                
            // 注册规则引擎中心
            builder.RegisterType<RuleEngineCenter>().As<IRuleEngineCenter>().SingleInstance();
        }

        private void RegisterRedis(ContainerBuilder builder)
        {
            try
            {
                // 设置日志记录器到RedisConnectionHelper
                if (_logger != null)
                {
                    RedisConnectionHelper.Logger = _logger;
                }

                // 从配置中读取Redis连接信息，优先使用标准配置键名
                string redisServer = "192.168.0.254:6379"; // 用户提供的当前运行服务器IP和端口
                string redisPassword = string.Empty;
                
                if (_configuration != null)
                {
                    // 尝试从标准配置位置读取
                    redisServer = _configuration.GetValue<string>("RedisServer", redisServer);
                    redisPassword = _configuration.GetValue<string>("RedisServerPWD", string.Empty);
                    
                    // 同时支持从SmartReminder特定配置读取
                    redisServer = _configuration.GetValue<string>("SmartReminder:RedisServer", redisServer);
                    redisPassword = _configuration.GetValue<string>("SmartReminder:RedisServerPWD", redisPassword);
                }
                
                // 构建连接字符串
                string redisConnectionString = redisServer;
                if (!string.IsNullOrEmpty(redisPassword))
                {
                    redisConnectionString = $"{redisServer},password={redisPassword}";
                }
                
                // 添加其他必要的连接参数
                redisConnectionString = $"{redisConnectionString},allowAdmin=true,abortConnect=false,connectTimeout=5000,syncTimeout=5000,asyncTimeout=5000,connectRetry=3";
                
                _logger?.LogInformation("正在配置Redis连接: {ConnectionString}", MaskSensitiveInfo(redisConnectionString));
                
                // 使用RedisConnectionHelper创建连接
                _redisMultiplexer = RedisConnectionHelper.GetConnectionMultiplexer(redisConnectionString);
                
                if (_redisMultiplexer != null)
                {
                    // 验证连接是否成功
                    if (_redisMultiplexer.IsConnected)
                    {
                        _logger?.LogInformation("Redis连接成功: {EndPoints}", 
                            string.Join(", ", _redisMultiplexer.GetEndPoints().Select(e => e.ToString())));
                    }
                    else
                    {
                        _logger?.LogWarning("Redis连接已创建，但当前未连接到任何服务器");
                    }

                    // 注册Redis连接实例到依赖注入容器
                    builder.RegisterInstance(_redisMultiplexer)
                        .As<ConnectionMultiplexer>()
                        .SingleInstance()
                        .OnRelease(multiplexer =>
                        {
                            try
                            {
                                multiplexer.Close();
                                _logger?.LogInformation("Redis连接已关闭");
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, "关闭Redis连接时发生错误");
                            }
                        });

                    // 注册IDatabase到依赖注入容器
                    builder.Register(c => _redisMultiplexer.GetDatabase())
                        .As<IDatabase>()
                        .SingleInstance();

                    // 可选：注册RedisHelper相关功能
                    builder.RegisterType<RedisCacheManager>()
                        .As<IRedisCacheManager>()
                        .SingleInstance();
                }
                else
                {
                    _logger?.LogError("无法创建Redis连接，将使用降级方案");
                    // 可以在这里注册降级方案，比如内存缓存
                }
            }
            catch (Exception ex)
            {
                // 如果Redis连接失败，记录错误但不阻止应用启动
                _logger?.LogError(ex, "配置Redis时发生异常");
                
                // 记录异常详情
                if (ex is SocketException socketEx)
                {
                    _logger?.LogError("Socket错误: 代码={ErrorCode}, 消息={Message}", 
                        socketEx.ErrorCode, socketEx.Message);
                }
                else if (ex is RedisConnectionException redisEx)
                {
                    _logger?.LogError("Redis连接错误: 类型={Type}, 消息={Message}", 
                        redisEx.FailureType, redisEx.Message);
                }
            }
        }
        
        // Redis连接事件处理已在RedisConnectionHelper中实现，此处不再重复定义
        
        private string MaskSensitiveInfo(string connectionString)
        {
            // 屏蔽连接字符串中的敏感信息
            if (string.IsNullOrEmpty(connectionString))
                return connectionString;
                
            // 屏蔽密码等敏感信息
            var masked = connectionString;
            masked = System.Text.RegularExpressions.Regex.Replace(
                masked, 
                "password=[^,]*", 
                "password=***", 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            
            masked = System.Text.RegularExpressions.Regex.Replace(
                masked, 
                "pwd=[^,]*", 
                "pwd=***", 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                
            return masked;
        }

    }
}
