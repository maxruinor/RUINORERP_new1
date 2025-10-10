using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Network.Commands;
using RUINORERP.Server.Network.Core;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Services;

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
            // 注册缓存服务
            //services.AddSingleton<CacheService>();

            // 注册会话管理服务
            services.AddSingleton<SessionService>();
            services.AddSingleton<ISessionService, SessionService>();

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

                // 注册为ICommandHandler
                var handlerInterface = typeof(ICommandHandler);
                if (handlerInterface.IsAssignableFrom(handlerType))
                {
                    services.AddTransient(handlerInterface, handlerType);
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
            //  builder.RegisterType<CacheService>().AsSelf().SingleInstance();
            builder.RegisterType<SessionService>().AsSelf().SingleInstance();
            builder.RegisterType<SessionService>().As<ISessionService>().SingleInstance();
            //    builder.RegisterType<UserService>().As<IUserService>().AsSelf().SingleInstance();
            //builder.RegisterType<SuperSocketAdapter>().AsSelf().SingleInstance();
            //  builder.RegisterType<FileStorageManager>().AsSelf().SingleInstance();

            // 注册服务器端登录请求处理器
            // builder.RegisterType<ServerLoginRequestHandler>().AsSelf().InstancePerDependency();

            // 注册锁管理服务
            builder.RegisterType<LockManagerService>().As<ILockManagerService>().SingleInstance();


            // 注册网络命令处理器
            RegisterNetworkCommandHandlers(builder);

            // 注册网络命令
            RegisterNetworkCommands(builder);
        }

        /// <summary>
        /// 注册网络命令处理器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterNetworkCommandHandlers(ContainerBuilder builder)
        {
            // 获取当前程序集
            var assembly = Assembly.GetExecutingAssembly();

            // 注册所有实现ICommandHandler接口的类型
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICommandHandler)))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }

        /// <summary>
        /// 注册网络命令
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterNetworkCommands(ContainerBuilder builder)
        {
            // 获取当前程序集
            var assembly = Assembly.GetExecutingAssembly();

            // 注册所有实现IServerCommand接口的类型
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsAbstract)
                .AsSelf()
                .InstancePerDependency();
        }

        /// <summary>
        /// 获取网络服务统计信息
        /// </summary>
        /// <returns>服务统计信息字符串</returns>
        public static string GetNetworkServicesStatistics()
        {
            return $"网络服务依赖注入配置完成。\n" +
                   $"已注册服务: 11个核心服务\n" +
                   $"已注册接口: 3个服务接口\n" +
                   $"生命周期: 单例模式\n" +
                   $"AOP支持: 已启用接口拦截器";
        }
    }
}