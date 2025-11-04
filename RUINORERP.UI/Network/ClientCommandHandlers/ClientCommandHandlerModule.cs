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

            // 注册命令处理器，同时注册为IClientCommandHandler接口，确保依赖注入可以正确解析
            builder.RegisterType<ConfigCommandHandler>()
                .As<IClientCommandHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageCommandHandler>()
                .As<IClientCommandHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();

        }
    }
}