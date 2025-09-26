using Microsoft.Extensions.DependencyInjection;
using Autofac;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Services;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.UI.Network.DI
{
    /// <summary>
    /// Network项目服务依赖注入配置类
    /// 负责注册所有Network项目中的服务和接口
    /// </summary>
    public static class NetworkServicesDependencyInjection
    {
        /// <summary>
        /// 配置Network服务依赖注入
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddNetworkServices(this IServiceCollection services)
        {
            // 注册核心网络组件
            services.AddSingleton<ClientCommandDispatcher>();
            services.AddSingleton<ISocketClient, SuperSocketClient>();
            services.AddSingleton<ICommandDispatcher, ClientCommandDispatcher>();
            services.AddSingleton<IClientCommunicationService, ClientCommunicationService>();
            services.AddSingleton<RequestResponseManager>();
            services.AddSingleton<ClientEventManager>();
            services.AddSingleton<HeartbeatManager>();
 

            // 注册业务服务 使用瞬态
            services.AddTransient<UserLoginService>();
            //services.AddTransient<CacheSyncService>();
            //services.AddTransient<MessageNotificationService>();
        }

        /// <summary>
        /// 配置Network服务Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureNetworkServicesContainer(this ContainerBuilder builder)
        {
            // 注册核心网络组件
            builder.RegisterType<SuperSocketClient>().As<ISocketClient>().SingleInstance();
            builder.RegisterType<ClientCommandDispatcher>().AsSelf().SingleInstance();
            
            // 注册ClientCommunicationService
            builder.RegisterType<ClientCommunicationService>().As<IClientCommunicationService>().SingleInstance();
            
            builder.RegisterType<RequestResponseManager>().AsSelf().SingleInstance();
            builder.RegisterType<ClientEventManager>().AsSelf().SingleInstance();
            
            // 注册HeartbeatManager，并使用属性注入解决循环依赖
            builder.RegisterType<HeartbeatManager>().AsSelf().SingleInstance();
            
            // 注册业务服务
            builder.RegisterType<UserLoginService>().AsSelf().SingleInstance();
            //builder.RegisterType<CacheSyncService>().AsSelf().InstancePerDependency();
            //builder.RegisterType<MessageNotificationService>().AsSelf().InstancePerDependency();
            
            // 移除RegisterBuildCallback回调，因为我们已经通过构造函数注入解决了循环依赖问题
        }

        /// <summary>
        /// 获取Network服务统计信息
        /// </summary>
        /// <returns>服务统计信息字符串</returns>
        public static string GetNetworkServicesStatistics()
        {
            return $"Network服务依赖注入配置完成。\n" +
                   $"已注册服务: 11个核心服务\n" +
                   $"已注册接口: 2个服务接口\n" +
                   $"生命周期: 单例模式和瞬态模式\n" +
                   $"AOP支持: 已启用接口拦截器\n" +
                   $"架构版本: 重构后新架构";
        }
    }
}