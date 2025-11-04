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
                
                // 使用自动扫描注册机制
                int scannedCount = 0;
                if (CommandDispatcher is ClientCommandDispatcher dispatcher)
                {
                    // 获取当前程序集和相关程序集
                    var assemblies = new[] 
                    {
                        System.Reflection.Assembly.GetExecutingAssembly()
                    };
                    
                    // 从DI容器获取Autofac生命周期作用域
                    var lifetimeScope = Startup.GetFromFac<Autofac.ILifetimeScope>();
                    
                    // 执行自动扫描注册
                    scannedCount = await dispatcher.ScanAndRegisterHandlersAsync(assemblies, lifetimeScope);
                    Logger.Info($"通过自动扫描成功注册 {scannedCount} 个客户端命令处理器");
                }
                
                Logger.Info("客户端命令处理器注册完成");
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