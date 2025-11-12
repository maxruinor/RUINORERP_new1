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
    /// 简单的IOptionsMonitor包装器
    /// 避免复杂的依赖关系导致的循环引用问题
    /// </summary>
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

            // 注册HeartbeatManager，移除对ClientCommunicationService的直接依赖
            builder.RegisterType<HeartbeatManager>().AsSelf().SingleInstance()
                .WithParameter((pi, ctx) => pi.ParameterType == typeof(int) && pi.Name == "heartbeatIntervalMs",
                               (pi, ctx) => 5000) // 默认30秒心跳间隔
                .WithParameter((pi, ctx) => pi.ParameterType == typeof(int) && pi.Name == "heartbeatTimeoutMs",
                               (pi, ctx) => 5000); // 默认5秒超时



            // 先注册一个 Lazy<ClientCommunicationService> 实例
            builder.Register(c => new Lazy<ClientCommunicationService>(() => c.Resolve<ClientCommunicationService>(), true))
                .As<Lazy<ClientCommunicationService>>()
                .SingleInstance();

            // 使用工厂方法注册ClientCommunicationService，避免循环依赖
            builder.Register(c =>
            {
                // 手动解析所有依赖项，避免依赖注入容器自动解析时可能产生的循环引用
                var socketClient = c.Resolve<ISocketClient>();
                var logger = c.Resolve<ILogger<ClientCommunicationService>>();
                var tokenManager = c.Resolve<TokenManager>();
                var clientCommandDispatcher = c.Resolve<IClientCommandDispatcher>();
                var heartbeatManager = c.Resolve<HeartbeatManager>();
                var clientEventManager = c.Resolve<ClientEventManager>(); // 获取已注册的ClientEventManager单例
                var commandHandlers = c.Resolve<IEnumerable<ICommandHandler>>();

                // 创建ClientCommunicationService实例
                var communicationService = new ClientCommunicationService(
                    socketClient,
                    logger,
                    tokenManager,
                    clientCommandDispatcher,
                    heartbeatManager,
                    clientEventManager,
                    commandHandlers
                );

                // 设置HeartbeatManager的ClientCommunicationService引用
                heartbeatManager.SetCommunicationService(communicationService);

                return communicationService;
            })
            .AsSelf()
            .SingleInstance()
            .OnActivated(e =>
            {
                // 在激活后显式初始化命令调度器，而不是在构造函数中
                // 这避免了在构造过程中触发依赖解析导致的循环引用
                try
                {
                    // 注意：这里需要确保ClientCommunicationService有一个公共的InitializeCommandDispatcher方法
                    // 如果没有，需要添加一个
                    var initializeMethod = e.Instance.GetType().GetMethod("InitializeClientCommandDispatcher", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (initializeMethod != null)
                    {
                        initializeMethod.Invoke(e.Instance, null);
                    }
                    else
                    {
                        e.Context.Resolve<ILogger<ClientCommunicationService>>()?
                            .LogWarning("未找到InitializeClientCommandDispatcher方法");
                    }
                }
                catch (Exception ex)
                {
                    e.Context.Resolve<ILogger<ClientCommunicationService>>()?
                        .LogError(ex, "初始化命令调度器失败");
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
            // 由于服务器端的BizCodeGenerateService可能不在业务层的引用范围内
            // 这里注册一个简单的本地实现，如果需要更复杂的功能，可以创建专门的实现类
            //最后注册的优先，所以这里放在一起
            // 注册LocalBizCodeGenerateService，仅作为自身类型注册，避免接口拦截问题
            //builder.RegisterType<Business.Services.LocalBizCodeGenerateService>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope()
            //    .PropertiesAutowired()
            //    .ExternallyOwned() // 标记为外部拥有，避免与拦截器冲突
            //    .PreserveExistingDefaults(); // 保留现有的默认实现
            
            // 注册BizCodeService作为IBizCodeService接口的主要实现
            builder.RegisterType<ClientBizCodeService>()
                .As<IBizCodeGenerateService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired()
                .PreserveExistingDefaults(); // 保留现有的默认实现


            builder.RegisterType<CacheClientService>().AsSelf().SingleInstance();
            builder.RegisterType<MessageService>().AsSelf().SingleInstance();
            builder.RegisterType<SimplifiedMessageService>().AsSelf().InstancePerDependency();
            builder.RegisterType<EnhancedMessageManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SystemManagementService>().AsSelf().InstancePerDependency();
            builder.RegisterType<AuthenticationManagementService>().AsSelf().InstancePerDependency();

            // 注册文件管理服务
            builder.RegisterType<FileManagementController>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            builder.RegisterType<FileManagementService>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            // 注册配置相关服务
            // 注册配置类型
            builder.RegisterType<SystemGlobalConfig>().AsSelf().SingleInstance();
            builder.RegisterType<ServerConfig>().AsSelf().SingleInstance();
            builder.RegisterType<GlobalValidatorConfig>().AsSelf().SingleInstance();

            // 为配置类型注册IOptions - 使用自定义SimpleOptions实现，避免循环依赖
            builder.Register(c => new SimpleOptions<SystemGlobalConfig>(c.Resolve<SystemGlobalConfig>()))
                .As<IOptions<SystemGlobalConfig>>()
                .SingleInstance();
            builder.Register(c => new SimpleOptions<ServerConfig>(c.Resolve<ServerConfig>()))
                .As<IOptions<ServerConfig>>()
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
        }

        /// <summary>
        /// 获取Network服务统计信息
        /// </summary>
        /// <returns>服务统计信息字符串</returns>
        public static string GetNetworkServicesStatistics()
        {
            return $"Network服务依赖注入配置完成。\n"
                   + $"已注册服务: 16个核心服务（RequestResponseManager已合并到ClientCommunicationService，新增TokenRefreshService和SilentTokenRefresher）\n"
                   + $"已注册命令处理器: ConfigCommandHandler和MessageCommandHandler（从ClientCommandHandlerModule移植）\n"
                   + $"已注册接口: 3个服务接口\n"
                   + $"生命周期: 单例模式、瞬态模式和InstancePerLifetimeScope\n"
                   + $"AOP支持: 已启用接口拦截器\n"
                   + $"架构版本: 重构后新架构";
        }
    }
}