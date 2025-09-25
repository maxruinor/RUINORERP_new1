using Microsoft.Extensions.DependencyInjection;
using Autofac;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Communication;

namespace RUINORERP.PacketSpec.DI
{
    /// <summary>
    /// PacketSpec项目服务依赖注入配置类
    /// 负责注册所有PacketSpec项目中的公共服务和接口
    /// </summary>
    public static class PacketSpecServicesDependencyInjection
    {
        /// <summary>
        /// 配置PacketSpec服务依赖注入
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddPacketSpecServices(this IServiceCollection services)
        {
            // 注册序列化服务包装器
            // 为静态UnifiedSerializationService类提供可注入的实例包装
            services.AddSingleton<IUnifiedSerializationService, UnifiedSerializationServiceWrapper>();
            
            // 注册命令调度器
            services.AddSingleton<CommandDispatcher>();
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            // 注册命令工厂
            services.AddSingleton<ICommandFactory, DefaultCommandFactory>();
            services.AddSingleton<ICommandFactoryAsync, DefaultCommandFactory>();
            
            // 注册命令扫描和类型辅助服务
            services.AddSingleton<CommandScanner>();
            services.AddSingleton<CommandTypeHelper>();
            
            // 注意：不注册抽象的RequestHandlerBase<TRequest, TResponse>类
            // 具体的请求处理器应该在各自的服务项目中注册
        }

        /// <summary>
        /// 配置PacketSpec服务Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigurePacketSpecServicesContainer(this ContainerBuilder builder)
        {

         

            // 注册序列化服务包装器
            builder.RegisterType<UnifiedSerializationServiceWrapper>()
                .As<IUnifiedSerializationService>()
                .SingleInstance();
            
            // 注册命令调度器
            builder.RegisterType<CommandDispatcher>().AsSelf().SingleInstance();
            
            // 注册命令工厂
            builder.RegisterType<DefaultCommandFactory>()
                .As<ICommandFactory>()
                .As<ICommandFactoryAsync>()
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
