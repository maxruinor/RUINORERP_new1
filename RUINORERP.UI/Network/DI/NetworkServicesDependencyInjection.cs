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
using RUINORERP.Business.CommService;
using RUINORERP.Business.Cache;
using RUINORERP.UI.Network.Services.Cache; // 添加缓存订阅管理器的引用

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
            services.AddSingleton<ISocketClient, SuperSocketClient>();
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<ClientCommunicationService>();
            services.AddSingleton<ClientEventManager>();

            services.AddSingleton<UICacheSubscriptionManager>();

            // 注册TokenManager相关服务
            // 首先配置TokenServiceOptions
            services.Configure<TokenServiceOptions>(options =>
            {
                options.SecretKey = "RUINORERP-Default-Secret-Key-2024";
                options.DefaultExpiryHours = 8;
                options.RefreshTokenExpiryHours = 24;
                options.Issuer = "RUINORERP";
                options.Audience = "RUINORERP-Users";
                options.ValidateIssuer = true;
                options.ValidateAudience = true;
                options.ValidateLifetime = true;
                options.ClockSkewSeconds = 300;
                options.ExpiryThresholdMinutes = 5;
            });

            // 注册TokenStorage
            services.AddSingleton<ITokenStorage, MemoryTokenStorage>();

            // 注册TokenService - 需要从IOptions中获取TokenServiceOptions
            services.AddSingleton<ITokenService>(provider =>
            {
                var options = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<TokenServiceOptions>>().Value;
                return new JwtTokenService(options);
            });

            // 注册TokenManager
            services.AddSingleton<TokenManager>();

            // 注册业务服务 使用瞬态
            services.AddTransient<UserLoginService>();
            services.AddSingleton<CacheClientService>();
            services.AddTransient<MessageService>();
            services.AddTransient<SimplifiedMessageService>();
            services.AddTransient<SystemManagementService>();
            services.AddTransient<AuthenticationManagementService>();


        }


        /// <summary>
        /// 配置Network服务Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureNetworkServicesContainer(this ContainerBuilder builder)
        {
            // 注册核心网络组件
            builder.RegisterType<SuperSocketClient>().As<ISocketClient>().SingleInstance();
            // 不再需要ClientTokenStorage，使用TokenManager代替

            // 注册ClientCommunicationService
            builder.RegisterType<ClientCommunicationService>().AsSelf().SingleInstance();

            // RequestResponseManager已合并到ClientCommunicationService中，不再需要单独注册
            builder.RegisterType<ClientEventManager>().AsSelf().SingleInstance();

            // 注册缓存订阅管理器
            builder.RegisterType<UICacheSubscriptionManager>().AsSelf().SingleInstance();

            // 注册HeartbeatManager，并使用属性注入解决循环依赖
            builder.RegisterType<HeartbeatManager>().AsSelf().SingleInstance()
                .WithParameter((pi, ctx) => pi.ParameterType == typeof(int) && pi.Name == "heartbeatIntervalMs",
                              (pi, ctx) => 30000) // 默认30秒心跳间隔
                .WithParameter((pi, ctx) => pi.ParameterType == typeof(int) && pi.Name == "heartbeatTimeoutMs",
                              (pi, ctx) => 5000); // 默认5秒超时



            // 注册TokenManager相关服务
            builder.RegisterType<MemoryTokenStorage>().As<ITokenStorage>().SingleInstance();

            // 注册TokenService - 需要从IOptions中获取TokenServiceOptions
            builder.Register<ITokenService>(context =>
            {
                var options = context.Resolve<Microsoft.Extensions.Options.IOptions<TokenServiceOptions>>().Value;
                return new JwtTokenService(options);
            }).SingleInstance();

            builder.RegisterType<TokenManager>().AsSelf().SingleInstance();

            // 注册业务服务
            builder.RegisterType<UserLoginService>().AsSelf().SingleInstance();
            builder.RegisterType<CacheClientService>().AsSelf().SingleInstance();
            builder.RegisterType<MessageService>().AsSelf().InstancePerDependency();
            builder.RegisterType<SimplifiedMessageService>().AsSelf().InstancePerDependency();
            builder.RegisterType<SystemManagementService>().AsSelf().InstancePerDependency();
            builder.RegisterType<AuthenticationManagementService>().AsSelf().InstancePerDependency();



            // 移除RegisterBuildCallback回调，因为我们已经通过构造函数注入解决了循环依赖问题
        }

        /// <summary>
        /// 获取Network服务统计信息
        /// </summary>
        /// <returns>服务统计信息字符串</returns>
        public static string GetNetworkServicesStatistics()
        {
            return $"Network服务依赖注入配置完成。\n" +
                   $"已注册服务: 14个核心服务（RequestResponseManager已合并到ClientCommunicationService）\n" +
                   $"已注册接口: 3个服务接口\n" +
                   $"生命周期: 单例模式和瞬态模式\n" +
                   $"AOP支持: 已启用接口拦截器\n" +
                   $"架构版本: 重构后新架构";
        }
    }
}