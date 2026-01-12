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

            // 设置支持的命令
            SetSupportedCommands(
                SystemCommands.Welcome,
                SystemCommands.WelcomeAck
            );
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
                var commandId = cmd.Packet.CommandId;

                if (commandId == SystemCommands.WelcomeAck)
                {
                    // 处理客户端对欢迎消息的确认回复
                    if (cmd.Packet.Request is WelcomeResponse welcomeResponse)
                    {
                        return await ProcessWelcomeAckAsync(welcomeResponse, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                }

                // 不支持的命令类型
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的命令类型");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "处理欢迎响应命令时发生异常");
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex);
            }
        }

        /// <summary>
        /// 处理客户端对欢迎消息的确认回复
        /// </summary>
        /// <param name="welcomeResponse">欢迎响应</param>
        /// <param name="executionContext">执行上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> ProcessWelcomeAckAsync(
            WelcomeResponse welcomeResponse,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var sessionInfo = SessionService.GetSession(executionContext.SessionId);
                if (sessionInfo == null)
                {
                    Logger.LogWarning($"[欢迎验证失败] SessionID={executionContext.SessionId}, 会话不存在");
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, "会话不存在");
                }

                if (sessionInfo.IsVerified)
                {
                    return ResponseFactory.CreateSpecificSuccessResponse(executionContext, "已验证");
                }

                sessionInfo.IsVerified = true;
                sessionInfo.IsConnected = true;
                sessionInfo.WelcomeAckReceived = true;

                if (sessionInfo.UserInfo == null)
                {
                    sessionInfo.UserInfo = new CurrentUserInfo();
                }

                sessionInfo.UserInfo.客户端版本 = welcomeResponse.ClientVersion;
                sessionInfo.UserInfo.操作系统 = welcomeResponse.ClientOS;
                sessionInfo.UserInfo.机器名 = welcomeResponse.ClientMachineName;
                sessionInfo.UserInfo.CPU信息 = welcomeResponse.ClientCPU;
                sessionInfo.UserInfo.内存大小 = welcomeResponse.ClientMemoryMB > 0
                    ? $"{welcomeResponse.ClientMemoryMB / 1024:F1} GB"
                    : "未知";

                Logger.LogInformation($"[欢迎成功] SessionID={sessionInfo.SessionID}, IP={sessionInfo.ClientIp}, 版本={welcomeResponse.ClientVersion}");

                SessionService.UpdateSession(sessionInfo);
                return ResponseFactory.CreateSpecificSuccessResponse(executionContext, "连接验证成功");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"欢迎回复处理异常: {executionContext.SessionId}");
                return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex, "处理欢迎回复异常");
            }
        }
    }
}
