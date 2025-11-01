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

            // 注册命令处理器注册器
            builder.RegisterType<ClientCommandHandlerRegistry>()
                .AsSelf()
                .InstancePerLifetimeScope();

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

            // 注册初始化任务，在容器构建完成后执行
            // 使用异步初始化方法，避免在构建回调中使用同步等待
            builder.Register(async (context, cancellationToken) =>
            {
                try
                {
                    // 获取命令调度器
                    var commandDispatcher = context.Resolve<IClientCommandDispatcher>();
                    
                    // 获取命令处理器注册器
                    var handlerRegistry = context.Resolve<ClientCommandHandlerRegistry>();
                    
                    // 异步初始化并启动调度器
                    await commandDispatcher.InitializeAsync();
                    await commandDispatcher.StartAsync();
                    
                    // 异步注册所有命令处理器
                    int registeredCount = await handlerRegistry.RegisterAllHandlersAsync();
                    
                    Logger.Info($"成功注册 {registeredCount} 个客户端命令处理器");
                    return registeredCount;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "初始化客户端命令处理系统失败");
                    return 0;
                }
            }).As<int>().SingleInstance().AutoActivate();
        }
    }
}