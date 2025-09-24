using RUINORERP.PacketSpec.Commands;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Server.Network
{
    /// <summary>
    /// 服务器端命令调度器
    /// 继承自公共的CommandDispatcher，添加服务器端特定的功能
    /// 负责服务器端命令的分发和处理
    /// </summary>
    public class ServerCommandDispatcher : CommandDispatcher
    {

        protected ILogger<CommandDispatcher> Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="commandTypeHelper">命令类型辅助类</param>
        public ServerCommandDispatcher(ILogger<CommandDispatcher> _logger, CommandTypeHelper commandTypeHelper = null)
            : base(_logger, null, commandTypeHelper)
        {
            // 设置日志记录器
            Logger = _logger;
        }

        /// <summary>
        /// 自动发现并注册服务器端命令处理器
        /// 扩展基类方法，添加服务器端程序集扫描
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>注册任务</returns>
        public async Task AutoDiscoverAndRegisterServerHandlersAsync(CancellationToken cancellationToken = default)
        {
            LogInfo("开始扫描并注册服务器端命令处理器...");

            // 扫描服务器项目中的处理器
            await AutoDiscoverAndRegisterHandlersAsync(
                cancellationToken,
                Assembly.GetExecutingAssembly() // 服务器项目程序集
            );
        }
    }
}