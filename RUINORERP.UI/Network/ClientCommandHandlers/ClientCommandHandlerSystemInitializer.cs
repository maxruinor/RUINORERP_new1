using Autofac;
using Castle.Core.Logging;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 客户端命令处理系统初始化器
    /// 负责异步初始化和启动客户端命令处理系统组件
    /// </summary>
    public class ClientCommandHandlerSystemInitializer
    {
        private static readonly ILogger Logger = new LoggerFactory().CreateLogger("ClientCommandHandlerSystemInitializer");
        
        /// <summary>
        /// 客户端命令调度器
        /// </summary>
        public IClientCommandDispatcher CommandDispatcher { get; set; }
        
        /// <summary>
        /// 命令处理器注册器
        /// </summary>
        public ClientCommandHandlerRegistry HandlerRegistry { get; set; }
        
        static ClientCommandHandlerSystemInitializer()
        {
            // 确保log4net配置正确加载
            XmlConfigurator.Configure();
        }
        
        /// <summary>
        /// 异步初始化命令处理系统
        /// </summary>
        /// <returns>Task表示异步操作</returns>
        public async Task InitializeAsync()
        {
            try
            {
                Logger.Info("开始初始化客户端命令处理系统");
                
                // 异步初始化命令调度器
                await CommandDispatcher.InitializeAsync();
                Logger.Info("命令调度器初始化完成");
                
                // 异步启动命令调度器
                await CommandDispatcher.StartAsync();
                Logger.Info("命令调度器启动完成");
                
                // 异步注册所有命令处理器
                int registeredCount = await HandlerRegistry.RegisterAllHandlersAsync();
                Logger.Info($"成功注册 {registeredCount} 个客户端命令处理器");
                
                Logger.Info("客户端命令处理系统初始化完成");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "初始化客户端命令处理系统失败");
                // 记录错误但不抛出异常，避免影响应用启动
            }
        }
    }
}