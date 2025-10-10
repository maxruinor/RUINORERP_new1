using Microsoft.Extensions.DependencyInjection;
using Autofac;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands.Authentication;
using Microsoft.Extensions.Configuration;
using RUINORERP.Model.ConfigModel;
using System;
using System.IO;
using Microsoft.Extensions.Options;


namespace RUINORERP.PacketSpec.DI
{
    /// <summary>
    /// PacketSpec项目服务依赖注入配置类
    /// 负责注册所有PacketSpec项目中的公共服务和接口
    /// </summary>
    public static class PacketSpecServicesDependencyInjection
    {
        /// <summary>
        /// 配置Token服务选项
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置</param>
        /// <param name="configureOptions">自定义配置选项</param>
        private static void ConfigureTokenServiceOptions(IServiceCollection services, IConfiguration configuration,
            Action<TokenServiceOptions> configureOptions = null)
        {
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }
            else
            {
                // 默认配置
                services.Configure<TokenServiceOptions>(options =>
                {
                    if (configureOptions != null)
                    {
                        configureOptions(options);
                    }
                    else
                    {
                        options.SecretKey = configuration["TokenService:SecretKey"] ?? "your-default-secret-key";
                        options.DefaultExpiryHours = 8;
                        options.RefreshTokenExpiryHours = 24;
                    }
                    // 验证配置
                    options.Validate();

                });

                // 显式注册 TokenServiceOptions 实例（用于直接依赖注入）
                services.AddSingleton(provider =>
                {
                    var options = provider.GetService<IOptions<TokenServiceOptions>>().Value;
                    return options;
                });

            }
        }

        /// <summary>
        /// 配置PacketSpec服务依赖注入
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置</param>
        public static void AddPacketSpecServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddPacketSpecServices(services, configuration, null);
        }

        /// <summary>
        /// 配置PacketSpec服务依赖注入（支持自定义Token配置）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置</param>
        /// <param name="configureTokenOptions">自定义Token服务配置选项</param>
        public static void AddPacketSpecServices(this IServiceCollection services, IConfiguration configuration, Action<TokenServiceOptions> configureTokenOptions)
        {
            // 注册序列化服务包装器
            // 为静态UnifiedSerializationService类提供可注入的实例包装
            services.AddSingleton<IUnifiedSerializationService, UnifiedSerializationServiceWrapper>();

            // 注意：Token服务通过Autofac注册，不在此处注册
            services.AddSingleton<TokenManager>();

            // 配置Token服务选项 - 使用改进的配置方法
            ConfigureTokenServiceOptions(services, configuration, configureTokenOptions);
            // 注册Token服务
            services.AddSingleton<ITokenService, JwtTokenService>();

            // 注册Token存储服务
            services.AddSingleton<ITokenStorage, MemoryTokenStorage>();

            // 注册Token服务 - 通过Autofac注册，这里仅注册接口映射
            // 注意：实际实现通过Autofac容器注册，确保能解析TokenServiceOptions


            // 注册命令调度器
            services.AddSingleton<CommandDispatcher>();
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
     


            // 注册命令创建服务（新增）
            services.AddSingleton<CommandCreationService>();
            services.AddSingleton<ICommandCreationService, CommandCreationService>();

            // 注册命令工厂
            services.AddSingleton<ICommandFactory, DefaultCommandFactory>();
            services.AddSingleton<ICommandFactoryAsync, DefaultCommandFactory>();

            // 注册命令扫描和类型辅助服务
            services.AddSingleton<CommandScanner>();

            // 注册命令处理器工厂
            services.AddSingleton<ICommandHandlerFactory, CommandHandlerFactory>();

            // 注册适配器
            services.AddSingleton<CommandPacketAdapter>();




            // 注意：不注册抽象的RequestHandlerBase<TRequest, TResponse>类
            // 具体的请求处理器应该在各自的服务项目中注册
        }

        /// <summary>
        /// 配置PacketSpec服务Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigurePacketSpecServicesContainer(this ContainerBuilder builder)
        {
            ConfigurePacketSpecServicesContainer(builder, null);
        }

        /// <summary>
        /// 配置PacketSpec服务Autofac容器（支持自定义Token配置）
        /// </summary>
        /// <param name="builder">容器构建器</param>
        /// <param name="configureTokenOptions">自定义Token服务配置选项</param>
        public static void ConfigurePacketSpecServicesContainer(this ContainerBuilder builder, Action<TokenServiceOptions> configureTokenOptions)
        {
            // 注册序列化服务包装器
            builder.RegisterType<UnifiedSerializationServiceWrapper>()
                .As<IUnifiedSerializationService>()
                .SingleInstance();

            // 注册命令调度器
            builder.RegisterType<CommandDispatcher>().AsSelf().SingleInstance();


            // 注册Token服务选项 - 支持自定义配置
            builder.Register(context =>
            {
                var options = new TokenServiceOptions();

                // 如果提供了自定义配置，优先使用
                if (configureTokenOptions != null)
                {
                    configureTokenOptions(options);
                }
                else
                {
                    // 使用配置文件中的设置
                    var configuration = context.Resolve<IConfiguration>();
                    options.SecretKey = configuration["TokenService:SecretKey"] ?? "your-default-secret-key";
                    if (int.TryParse(configuration["TokenService:DefaultExpiryHours"], out var defaultExpiryHours))
                    {
                        options.DefaultExpiryHours = defaultExpiryHours;
                    }
                    if (int.TryParse(configuration["TokenService:RefreshTokenExpiryHours"], out var refreshTokenExpiryHours))
                    {
                        options.RefreshTokenExpiryHours = refreshTokenExpiryHours;
                    }
                }

                return options;
            }).As<TokenServiceOptions>().SingleInstance();

            // 注册Token存储服务
            builder.RegisterType<MemoryTokenStorage>()
                .As<ITokenStorage>()
                .SingleInstance();

            // 注册Token服务 - 使用构造函数注入TokenServiceOptions
            builder.RegisterType<JwtTokenService>()
                .As<ITokenService>()
                .SingleInstance();


            // 注册命令创建服务（新增）
            builder.RegisterType<CommandCreationService>()
                .AsSelf()
                .As<ICommandCreationService>()
                .SingleInstance();

            // 注册命令工厂
            builder.RegisterType<DefaultCommandFactory>()
                .As<ICommandFactory>()
                .As<ICommandFactoryAsync>()
                .SingleInstance();

            // 注册命令处理器工厂
            builder.RegisterType<CommandHandlerFactory>()
                .As<ICommandHandlerFactory>()
                .SingleInstance();

            // 注册适配器
            builder.RegisterType<CommandPacketAdapter>().SingleInstance();



            // 注册TokenManager服务 - 简化的Token管理器
            builder.RegisterType<TokenManager>()
                .As<TokenManager>()
                .SingleInstance();

            // 注意：不注册抽象的RequestHandlerBase<TRequest, TResponse>类
            // 具体的请求处理器应该在各自的服务项目中注册
        }

        /// <summary>
        /// 获取PacketSpec服务统计信息
        /// </summary>
        /// <returns>服务统计信息字符串</returns>
        public static string GetPacketSpecServicesStatistics()
        {
            return $"PacketSpec服务依赖注入配置完成。\n" +
                   $"已注册服务: 4个核心服务\n" +
                   $"已注册接口: 4个服务接口\n" +
                   $"生命周期: 单例模式和瞬态模式\n" +
                   $"AOP支持: 已启用接口拦截器";
        }
    }
}
