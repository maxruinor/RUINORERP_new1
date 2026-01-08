using Microsoft.Extensions.DependencyInjection;
using Autofac;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Services;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.UI.Network.Authentication;
using RUINORERP.PacketSpec.Commands.Authentication;
using SourceLibrary.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Cache;
using RUINORERP.UI.Network.Services.Cache;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.Business;
using RUINORERP.Model.ConfigModel;
using RUINORERP.UI.SysConfig;
using RUINORERP.UI.IM;
using RUINORERP.UI.Network.ClientCommandHandlers;
using System.Collections.Generic;
using RUINORERP.Business.Network;
using RUINORERP.IServices;

namespace RUINORERP.UI.Network.DI
{
    /// <summary>
    /// 简单的IOptions<T>实现，避免复杂依赖导致的循环引用
    /// </summary>
    /// <typeparam name="TOptions">选项类型</typeparam>
    internal class SimpleOptions<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value">选项值</param>
        public SimpleOptions(TOptions value)
        {
            Value = value;
        }

        /// <summary>
        /// 获取选项值
        /// </summary>
        public TOptions Value { get; }
    }

    /// <summary>
    /// Network项目服务依赖注入配置类
    /// 负责注册所有Network项目中的服务和接口
    /// </summary>
    public static class NetworkServicesDependencyInjection
    {
        /// <summary>
        /// 配置Network服务Autofac容器
        /// 包含所有网络相关服务的注册配置
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureNetworkServicesContainer(this ContainerBuilder builder)
        {
            // 注册核心网络组件
            builder.RegisterType<SuperSocketClient>().As<ISocketClient>().SingleInstance();
            
            // 注册连接管理器
            builder.RegisterType<ConnectionManager>().AsSelf().SingleInstance()
                .WithParameter((pi, ctx) => pi.ParameterType == typeof(ConnectionManagerConfig),
                               (pi, ctx) => ConnectionManagerConfig.Default);

            // 注册客户端命令调度器
            builder.RegisterType<ClientCommandDispatcher>()
                .As<IClientCommandDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClientEventManager>().AsSelf().SingleInstance();

            // 注册缓存相关服务
            builder.RegisterType<EntityCacheManager>().As<IEntityCacheManager>().SingleInstance();
            builder.RegisterType<EventDrivenCacheManager>().AsSelf().SingleInstance();
            builder.RegisterType<CacheRequestManager>().AsSelf().SingleInstance();
            builder.RegisterType<CacheResponseProcessor>().AsSelf().SingleInstance();

            // 简化的ClientCommunicationService注册方式
            // 移除了复杂的工厂方法和Lazy初始化，使用标准的注册方式
            builder.RegisterType<ClientCommunicationService>()
                .AsSelf()
                .As<IClientCommunicationService>()
                .SingleInstance()
                .OnActivated(e =>
                {
                    // 在激活阶段就解析出logger，避免在异步任务中使用可能已释放的Context
                    var logger = e.Context.Resolve<ILogger<ClientCommunicationService>>();
                    
                    // 在激活后显式初始化命令调度器，但不使用同步等待来避免死锁
                    try
                    {
                        // 获取实例
                        var service = e.Instance;
                        
                        // 使用反射调用InitializeClientCommandDispatcherAsync方法
                        var initializeMethod = typeof(ClientCommunicationService).GetMethod("InitializeClientCommandDispatcherAsync", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (initializeMethod != null)
                        {
                            // 使用Task.Run避免在依赖注入过程中产生死锁
                            Task.Run(() =>
                            {
                                try
                                {
                                    var task = (Task)initializeMethod.Invoke(service, null);
                                    task.GetAwaiter().GetResult();
                                }
                                catch (Exception ex)
                                {
                                    logger?.LogError(ex, "初始化命令调度器失败");
                                }
                            });
                        }
                        else
                        {
                            logger?.LogWarning("未找到InitializeClientCommandDispatcherAsync方法");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "初始化命令调度器失败");
                    }
                });

            // 配置TokenServiceOptions - 使用自定义IOptions实现，避免OptionsWrapper可能导致的循环依赖
            var tokenServiceOptions = new TokenServiceOptions
            {
                SecretKey = "RUINORERP-Default-Secret-Key-2024",
                DefaultExpiryHours = 8,
                Issuer = "RUINORERP",
                Audience = "RUINORERP-Users",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkewSeconds = 300
            };

            // 直接注册TokenServiceOptions实例
            builder.RegisterInstance(tokenServiceOptions).SingleInstance();

            // 使用自定义的SimpleOptions实现，避免OptionsWrapper可能导致的循环依赖
            builder.RegisterInstance(new SimpleOptions<TokenServiceOptions>(tokenServiceOptions))
                .As<IOptions<TokenServiceOptions>>()
                .SingleInstance();

            // 注册TokenStorage
            builder.RegisterType<MemoryTokenStorage>().As<ITokenStorage>().SingleInstance();

            // 注册TokenService - 从IOptions中获取TokenServiceOptions，与参考代码风格一致
            builder.Register<ITokenService>(provider =>
            {
                var options = provider.Resolve<IOptions<TokenServiceOptions>>().Value;
                return new JwtTokenService(options);
            }).SingleInstance();

            // 注册TokenManager
            builder.RegisterType<TokenManager>().AsSelf().SingleInstance();

            // 注册Token刷新服务
            builder.RegisterType<TokenRefreshService>().AsSelf().SingleInstance();
            builder.RegisterType<SilentTokenRefresher>().AsSelf().SingleInstance();

            // 注册业务服务
            builder.RegisterType<UserLoginService>().AsSelf().SingleInstance();

            // 注册本地业务编码生成服务，作为服务器通信失败时的备用方案
            builder.RegisterType<ClientBizCodeService>()
                .As<IBizCodeGenerateService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired()
                .PreserveExistingDefaults();

            builder.RegisterType<CacheClientService>().AsSelf().SingleInstance();
            // 使用工厂方法注册MessageService，确保正确注入Lazy<ClientCommunicationService>
            builder.Register(c =>
            {
                // 获取Lazy<ClientCommunicationService>实例
                var lazyCommunicationService = c.Resolve<Lazy<ClientCommunicationService>>();
                var logger = c.Resolve<ILogger<MessageService>>();
                
                // 创建MessageService实例，传入Lazy依赖以避免循环引用
                return new MessageService(lazyCommunicationService, logger);
            }).AsSelf().SingleInstance();
            builder.RegisterType<EnhancedMessageManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SystemManagementService>().AsSelf().InstancePerDependency();
           // builder.RegisterType<BusinessNotificationService>().AsSelf().InstancePerLifetimeScope();

            // 注册文件管理服务
            builder.RegisterType<FileManagementController>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            builder.RegisterType<FileManagementService>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            // 注册锁状态通知服务
            builder.RegisterType<LockStatusNotificationService>()
                .AsSelf()
                .SingleInstance();

            // 注册锁管理相关服务
            RegisterLockManagementServices(builder);

            // 注册配置相关服务
            // 注册配置类型
            builder.RegisterType<SystemGlobalConfig>().AsSelf().SingleInstance();
            builder.RegisterType<ServerGlobalConfig>().AsSelf().SingleInstance();
            builder.RegisterType<GlobalValidatorConfig>().AsSelf().SingleInstance();

            // 为配置类型注册IOptions - 使用自定义SimpleOptions实现，避免循环依赖
            builder.Register(c => new SimpleOptions<SystemGlobalConfig>(c.Resolve<SystemGlobalConfig>()))
                .As<IOptions<SystemGlobalConfig>>()
                .SingleInstance();
            builder.Register(c => new SimpleOptions<ServerGlobalConfig>(c.Resolve<ServerGlobalConfig>()))
                .As<IOptions<ServerGlobalConfig>>()
                .SingleInstance();
            builder.Register(c => new SimpleOptions<GlobalValidatorConfig>(c.Resolve<GlobalValidatorConfig>()))
                .As<IOptions<GlobalValidatorConfig>>()
                .SingleInstance();

            // 扫描并注册所有命令处理器
            var commandHandlerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(ICommandHandler).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

            foreach (var handlerType in commandHandlerTypes)
            {
                builder.RegisterType(handlerType)
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerLifetimeScope();
            }

            // 注册特定的命令处理器
            builder.RegisterType<ConfigCommandHandler>()
                .As<IClientCommandHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageCommandHandler>()
                .As<IClientCommandHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();

            // 注册工作流命令处理器
            builder.RegisterType<WorkflowCommandHandler>()
                .As<IClientCommandHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();

            //builder.RegisterType<BusinessNotificationHandler>()
            //    .As<IClientCommandHandler>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();

            // 注册LockCommandHandler，注入LockStatusNotificationService
            builder.Register(c =>
            {
                var logger = c.Resolve<ILogger<LockCommandHandler>>();
                var lockCache = c.Resolve<ClientLocalLockCacheService>();
                var notificationService = c.Resolve<LockStatusNotificationService>();
                
                return new LockCommandHandler(logger, lockCache, notificationService);
            })
            .As<IClientCommandHandler>()
            .AsSelf()
            .InstancePerLifetimeScope();
        }

        /// <summary>
        /// 注册锁管理相关服务
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterLockManagementServices(ContainerBuilder builder)
        {
            // 使用工厂模式先注册ClientLockManagementService
            // 使用Lazy模式解析ClientCommunicationService来避免循环依赖
            var lockServiceRegistration = builder.Register(c =>
            {
                // 解析核心依赖 - 使用Lazy延迟解析ClientCommunicationService
                var lazyCommunicationService = c.Resolve<Lazy<ClientCommunicationService>>();
                var logger = c.Resolve<ILogger<ClientLockManagementService>>();
                var notificationService = c.Resolve<LockStatusNotificationService>();

                // 创建锁管理服务实例
                // 注意：移除了对HeartbeatManager的依赖
                return new ClientLockManagementService(
                    lazyCommunicationService,
                    logger,
                    null,
                    null,
                    notificationService);
            })
            .AsSelf()
            .SingleInstance()
            .OnActivated(e =>
            {
                // 在ClientLockManagementService激活后，提取其内部的ClientLockCache实例并注册到容器
                try
                {
                    var lockService = e.Instance;
                    
                    // 通过反射获取内部的ClientLockCache
                    var clientCacheField = typeof(ClientLockManagementService)
                        .GetField("_clientCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    
                    if (clientCacheField?.GetValue(lockService) is ClientLocalLockCacheService clientCache)
                    {
                        // 手动注册ClientLockCache实例到容器
                        // 由于Autofac不支持动态注册，我们将使用另一种方法
                    }
                }
                catch (Exception ex)
                {
                    //logger?.LogError(ex, "处理ClientLockManagementService激活事件失败");
                }
            });

            // 注册ClientLockCache作为ClientLockManagementService的属性访问
            builder.Register(c =>
            {
                var lockService = c.Resolve<ClientLockManagementService>();
                // 使用反射获取ClientLockCache字段
                var clientCacheField = typeof(ClientLockManagementService)
                    .GetField("_clientCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (clientCacheField != null)
                {
                    return clientCacheField.GetValue(lockService) as ClientLocalLockCacheService;
                }
                return null;
            })
            .As<ClientLocalLockCacheService>()
            .SingleInstance();
        }

        /// <summary>
        /// 获取Network服务统计信息
        /// </summary>
        /// <returns>服务统计信息字符串</returns>
        public static string GetNetworkServicesStatistics()
        {   
            return $"Network服务依赖注入配置完成。\n"
                   + $"已注册服务: 15个核心服务（心跳功能已集成到ClientCommunicationService）\n"
                   + $"已注册命令处理器: ConfigCommandHandler和MessageCommandHandler\n"
                   + $"已注册接口: 3个服务接口\n"
                   + $"已注册锁管理服务: ClientLockManagementService, ClientLockCache\n"
                   + $"生命周期: 单例模式、瞬态模式和InstancePerLifetimeScope\n"
                   + $"架构版本: 重构后新架构，心跳功能已集成到通信服务中";      
        }
    }
}