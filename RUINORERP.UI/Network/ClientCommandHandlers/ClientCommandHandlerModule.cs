using Autofac;
using Castle.Core.Logging;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Module = Autofac.Module;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 客户端命令处理器Autofac模块
    /// 负责注册所有客户端命令处理相关组件到依赖注入容器
    /// </summary>
    public class ClientCommandHandlerModule : Module
    {
        private static readonly ILogger Logger = new LoggerFactory().CreateLogger("ClientCommandHandlerModule");

        static ClientCommandHandlerModule()
        {
            // 确保log4net配置正确加载
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// 加载模块，注册服务
        /// </summary>
        /// <param name="builder">Autofac容器构建器</param>
        protected override void Load(ContainerBuilder builder)
        {
            // 注册客户端命令调度器
            builder.RegisterType<ClientCommandDispatcher>()
                .As<IClientCommandDispatcher>()
                .InstancePerLifetimeScope();

            // 不再需要命令处理器注册器，直接使用ClientCommandDispatcher的扫描注册功能

            // 注册消息服务
            builder.RegisterType<RUINORERP.UI.Network.Services.MessageService>()
                .As<RUINORERP.UI.Network.Services.MessageService>()
                .InstancePerLifetimeScope();

            // 注册命令处理器
            builder.RegisterType<ConfigCommandHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageCommandHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();

            // 注册命令处理系统初始化服务
            // 避免使用async委托与AutoActivate组合，改用专用初始化服务
            builder.RegisterType<ClientCommandHandlerSystemInitializer>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            // 注册一个启动时触发的初始化器
            builder.RegisterBuildCallback(container =>
            {
                try
                {
                    // 在容器构建完成后获取初始化器并启动初始化流程
                    // 注意：这里不使用同步等待，而是异步启动并记录任务
                    var initializer = container.Resolve<ClientCommandHandlerSystemInitializer>();
                    _ = initializer.InitializeAsync(); // 异步启动，不阻塞容器构建
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "启动命令处理系统初始化器失败");
                }
            });
        }
    }
}