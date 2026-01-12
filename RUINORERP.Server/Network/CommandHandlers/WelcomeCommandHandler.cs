using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Handlers;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 欢迎响应命令处理器 - 处理客户端连接的欢迎握手流程
    /// 服务器在客户端连接时发送欢迎消息，客户端需要回复确认才能完成连接验证
    /// </summary>
    [CommandHandler("WelcomeCommandHandler", priority: 200)]
    public class WelcomeCommandHandler : BaseCommandHandler
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<WelcomeCommandHandler> Logger { get; set; }

        /// <summary>
        /// 会话管理服务
        /// </summary>
        protected ISessionService SessionService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="sessionService">会话管理服务</param>
        public WelcomeCommandHandler(
            ILogger<WelcomeCommandHandler> logger,
            ISessionService sessionService) : base(logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            SetSupportedCommands(SystemCommands.Welcome);
            SetSupportedCommands(SystemCommands.WelcomeAck);
        }

        /// <summary>
        /// 核心处理方法
        /// </summary>
        /// <param name="cmd">队列命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                // 此处理器不应该被调用，因为欢迎流程通过SendCommandAndWaitForResponseAsync处理
                // WelcomeAck由服务器端的HandleClientResponse方法直接处理
                Logger.LogWarning("WelcomeCommandHandler不应该被调用，欢迎响应由HandleClientResponse处理");
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, "不支持的命令类型");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "处理欢迎命令时发生异常");
                Logger.LogWarning($"[主动断开连接] 处理欢迎命令异常，可能导致连接问题: {ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex);
            }
        }
    }
}
