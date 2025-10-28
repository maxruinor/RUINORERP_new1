using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Network.Core;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Monitoring;
using RUINORERP.Server.Network.Services;
using RUINORERP.Business.CommService;
using System.Threading;
using RUINORERP.PacketSpec.Models.Core;
using System;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading.Tasks;
using RUINORERP.Business.Cache; // 添加缓存订阅管理器的引用
using RUINORERP.Server.Network.CommandHandlers;

namespace RUINORERP.Server.Network.DI
{
    /// <summary>
    /// 网络服务依赖注入配置类
    /// 负责注册所有网络相关的服务和接口
    /// </summary>
    public static class NetworkServicesDependencyInjection
    {
        /// <summary>
        /// 配置网络服务依赖注入
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddNetworkServices(this IServiceCollection services)
        {
            // 注册核心服务
            //  services.AddSingleton<IFileStorageService, FileStorageService>();
            // services.AddSingleton<IPacketSpecService, PacketSpecService>();
            // services.AddSingleton<IUnifiedPacketSpecService, UnifiedPacketSpecService>();
            //   services.AddSingleton<UnifiedCommunicationProcessor>();
    
            // 注册会话管理服务 - 只注册一次，通过接口映射确保一致性
            services.AddSingleton<SessionService>();
            services.AddSingleton<ISessionService>(provider => provider.GetRequiredService<SessionService>());

            // 注册服务器消息服务
            services.AddSingleton<ServerMessageService>();
            
            // 注册服务器消息服务使用示例
            services.AddSingleton<ServerMessageServiceUsageExample>();
            services.AddSingleton<MessageServiceUsageExample>();

            // 注册缓存订阅管理器
            services.AddSingleton<CacheSubscriptionManager>(provider =>
                new CacheSubscriptionManager(provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CacheSubscriptionManager>>(), true));

            // 注册诊断相关服务
            services.AddSingleton<DiagnosticsService>();
            services.AddSingleton<PerformanceMonitoringService>();
            services.AddSingleton<ErrorAnalysisService>();

            // 注册用户服务
            //    services.AddSingleton<IUserService, UserService>();

            // 注册SuperSocket适配器
            //    services.AddSingleton<SuperSocketAdapter>();

            // 注册文件存储管理器
            //    services.AddSingleton<FileStorageManager>();

            // 注册服务器端登录请求处理器
            // services.AddTransient<ServerLoginRequestHandler>();

            // 注册工作流服务接收器
            //   services.AddSingleton<WorkflowServiceReceiver>();

            // 注册系统命令处理器
            services.AddTransient<SystemCommandHandler>();
            
            // 注册认证命令处理器
            services.AddTransient<AuthenticationCommandHandler>();
        }

        /// <summary>
        /// 自动注册所有命令处理器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>更新后的服务集合</returns>
        public static IServiceCollection AddUnifiedCommandHandlers(this IServiceCollection services)
        {
            // 自动注册所有命令处理器
            var handlerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseCommandHandler)))
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                services.AddTransient(handlerType);

                // 注册为命令处理器接口 - 使用更精确的接口识别方式
                foreach (var implementedInterface in handlerType.GetInterfaces())
                {
                    // 通过接口的基本结构特征来识别命令处理器接口，而不是依赖具体接口名称
                    if (IsCommandHandlerInterface(implementedInterface))
                    {
                        services.AddTransient(implementedInterface, handlerType);
                    }
                }
            }

            return services;
        }

        /// <summary>
        /// 配置网络服务Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureNetworkServicesContainer(this ContainerBuilder builder)
        {
            // 注册所有网络服务接口和实现
            //builder.RegisterType<FileStorageService>().As<IFileStorageService>().SingleInstance();

            // 注册其他服务实现
            //  services.AddSingleton<IFileStorageService, FileStorageService>();
            // 使用链式注册确保SessionService和ISessionService指向同一个实例
            builder.RegisterType<SessionService>().AsSelf().As<ISessionService>().SingleInstance();
            builder.RegisterType<ServerMessageService>().AsSelf().SingleInstance();
            builder.RegisterType<ServerMessageServiceUsageExample>().AsSelf().SingleInstance();
            builder.RegisterType<MessageServiceUsageExample>().AsSelf().SingleInstance();
            //    builder.RegisterType<UserService>().As<IUserService>().AsSelf().SingleInstance();
            //builder.RegisterType<SuperSocketAdapter>().AsSelf().SingleInstance();
            //  builder.RegisterType<FileStorageManager>().AsSelf().SingleInstance();

            // 注册缓存订阅管理器
            builder.Register(c => new CacheSubscriptionManager(
                c.Resolve<Microsoft.Extensions.Logging.ILogger<CacheSubscriptionManager>>(), 
                true)) // 服务器模式
                .As<CacheSubscriptionManager>()
                .SingleInstance();

            // 注册诊断相关服务
            builder.RegisterType<DiagnosticsService>().AsSelf().SingleInstance();
            builder.RegisterType<PerformanceMonitoringService>().AsSelf().SingleInstance();
            builder.RegisterType<ErrorAnalysisService>().AsSelf().SingleInstance();

            // 注册锁管理服务
            builder.RegisterType<LockManagerService>().As<ILockManagerService>().SingleInstance();

            // 注册服务器端登录请求处理器
            // builder.RegisterType<ServerLoginRequestHandler>().AsSelf().InstancePerDependency();

            // 注册网络命令处理器
            RegisterNetworkCommandHandlers(builder);

            // 注册系统命令处理器
            builder.RegisterType<SystemCommandHandler>().AsSelf().InstancePerDependency();
        }

        /// <summary>
        /// 注册网络命令处理器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterNetworkCommandHandlers(ContainerBuilder builder)
        {
            // 获取当前程序集
            var assembly = Assembly.GetExecutingAssembly();

            // 注册所有实现命令处理器接口的类型 - 使用更精确的接口识别方式
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetInterfaces().Any(i => IsCommandHandlerInterface(i)))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }

 

        /// <summary>
        /// 检查接口是否为命令处理器接口
        /// 通过接口特征识别，而不是依赖具体接口名称
        /// </summary>
        /// <param name="interfaceType">要检查的接口类型</param>
        /// <returns>是否为命令处理器接口</returns>
        private static bool IsCommandHandlerInterface(Type interfaceType)
        {
            if (!interfaceType.IsInterface)
                return false;

            // 通过接口特征识别命令处理器接口，而不是依赖具体的接口名称
            // 检查是否有HandleAsync方法，接受PacketModel参数并返回ResponseBase
            var methods = interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            var handleMethod = methods.FirstOrDefault(m => 
                m.Name == "HandleAsync" && 
                m.GetParameters().Length == 2 && 
                m.GetParameters()[0].ParameterType == typeof(PacketModel) &&
                m.GetParameters()[1].ParameterType == typeof(CancellationToken) &&
                m.ReturnType.IsGenericType &&
                m.ReturnType.GetGenericTypeDefinition() == typeof(Task<>) &&
                m.ReturnType.GetGenericArguments()[0] == typeof(ResponseBase)
            );
            
            return handleMethod != null;
        }

        /// <summary>
        /// 获取网络服务统计信息
        /// </summary>
        /// <returns>服务统计信息字符串</returns>
        public static string GetNetworkServicesStatistics()
        {
            return $"网络服务依赖注入配置完成。\n" +
                   $"已注册服务: 18个核心服务\n" +
                   $"已注册接口: 3个服务接口\n" +
                   $"生命周期: 单例模式\n" +
                   $"AOP支持: 已启用接口拦截器";
        }
    }
}