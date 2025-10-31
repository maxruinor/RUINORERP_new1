using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.UI.SysConfig;
using Autofac;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 客户端命令处理器注册器
    /// 负责注册所有的客户端命令处理器到命令调度器
    /// </summary>
    public class ClientCommandHandlerRegistry
    {
        private readonly IClientCommandDispatcher _commandDispatcher;
        private readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="lifetimeScope">Autofac生命周期作用域，用于解析依赖</param>
        public ClientCommandHandlerRegistry(IClientCommandDispatcher commandDispatcher, ILifetimeScope lifetimeScope)
        {
            _commandDispatcher = commandDispatcher ?? throw new System.ArgumentNullException(nameof(commandDispatcher));
            _lifetimeScope = lifetimeScope ?? throw new System.ArgumentNullException(nameof(lifetimeScope));
        }

        /// <summary>
        /// 注册所有内置命令处理器
        /// </summary>
        /// <returns>注册的处理器数量</returns>
        public async Task<int> RegisterAllHandlersAsync()
        {
            int registeredCount = 0;

            // 注册配置命令处理器
            if (await RegisterConfigCommandHandlerAsync())
            {
                registeredCount++;
            }

            // 注册消息命令处理器
            if (await RegisterMessageCommandHandlerAsync())
            {
                registeredCount++;
            }

            // 可以在这里添加更多的命令处理器注册

            return registeredCount;
        }

        /// <summary>
        /// 注册配置命令处理器
        /// </summary>
        /// <returns>注册是否成功</returns>
        private async Task<bool> RegisterConfigCommandHandlerAsync()
        {
            try
            {
                // 从依赖注入容器中解析配置管理器
                var optionsMonitorConfigManager = _lifetimeScope.Resolve<OptionsMonitorConfigManager>();
                
                // 创建配置命令处理器
                var configCommandHandler = new ConfigCommandHandler(optionsMonitorConfigManager);
                
                // 注册到命令调度器
                return await _commandDispatcher.RegisterHandlerAsync(configCommandHandler);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"注册配置命令处理器失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 注册消息命令处理器
        /// </summary>
        /// <returns>注册是否成功</returns>
        private async Task<bool> RegisterMessageCommandHandlerAsync()
        {
            try
            {
                // 从依赖注入容器中解析消息服务
                var messageService = Startup.GetFromFac<MessageService>();
                
                // 创建消息命令处理器并传入依赖
                var messageCommandHandler = new MessageCommandHandler(messageService);
                
                // 注册到命令调度器
                return await _commandDispatcher.RegisterHandlerAsync(messageCommandHandler);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"注册消息命令处理器失败: {ex.Message}");
                return false;
            }
        }
    }
}