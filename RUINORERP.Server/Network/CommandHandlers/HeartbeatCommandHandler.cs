using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Errors;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 心跳命令处理器
    /// 处理客户端发送的心跳命令，维持连接活跃状态
    /// </summary>
    [CommandHandler("HeartbeatCommandHandler", priority: 0)]
    public class HeartbeatCommandHandler : BaseCommandHandler
    {
        private readonly ISessionService _sessionService;

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// 注意：这是临时解决方案，优先使用带参构造函数
        /// </summary>
        public HeartbeatCommandHandler() : base()
        {
            // 为了兼容性保留这个构造函数，但实际运行时应使用依赖注入的构造函数
        }

        /// <summary>
        /// 构造函数 - 通过依赖注入获取服务
        /// </summary>
        public HeartbeatCommandHandler(
             ISessionService sessionService,
            ILogger<HeartbeatCommandHandler> logger = null) : base(logger)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));

            // 使用安全方法设置支持的命令
            SetSupportedCommands(SystemCommands.Heartbeat);
        }

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Packet.CommandId;

                if (commandId == SystemCommands.Heartbeat)
                {
                    return HandleHeartbeatAsync(cmd, cancellationToken);
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的命令类型: {cmd.Packet.CommandId.ToString()}");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理心跳命令异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理心跳命令异常");
            }
        }

        /// <summary>
        /// 会话管理服务
        /// </summary>
        private ISessionService SessionService => _sessionService;

        /// <summary>
        /// 处理心跳命令
        /// </summary>
        private IResponse HandleHeartbeatAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken)
        {
            try
            {
                // 获取或创建会话信息
                var sessionInfo = SessionService.GetSession(queuedCommand.Packet.ExecutionContext.SessionId);

                // 检查会话是否存在
                if (sessionInfo == null)
                {
                    LogWarning($"未找到会话信息: SessionId={queuedCommand.Packet.ExecutionContext.SessionId}");
                    var errorResponse = new HeartbeatResponse
                    {
                        IsSuccess = false,
                        Status = "ERROR",
                        ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        NextIntervalMs = 5000, // 错误情况下缩短间隔
                        ServerInfo = new Dictionary<string, object>
                        {
                            ["Error"] = "会话不存在或已过期",
                            ["ErrorCode"] = "SESSION_NOT_FOUND",
                            ["SessionId"] = queuedCommand.Packet.ExecutionContext.SessionId
                        }
                    };
                    return ResponseFactory.CreateSpecificErrorResponse(queuedCommand.Packet, "会话不存在或已过期");
                }

                if (queuedCommand.Packet.Request is HeartbeatRequest heartbeatRequest)
                {
                    // 确保 UserInfo 不为空
                    if (heartbeatRequest.UserInfo != null)
                    {
                        heartbeatRequest.UserInfo.SessionId = sessionInfo.SessionID;
                        sessionInfo.UserInfo = heartbeatRequest.UserInfo;
                    }
                }

                // 创建心跳响应数据
                var response = new HeartbeatResponse
                {
                    IsSuccess = true,
                    Status = "OK",
                    ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    NextIntervalMs = 30000, // 默认30秒间隔
                    ServerInfo = new Dictionary<string, object>
                    {
                        ["ServerTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["ServerVersion"] = "1.0.0",
                        ["SessionId"] = sessionInfo.SessionID
                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                LogError($"处理心跳命令异常: {ex.Message}", ex);
                var errorResponse = new HeartbeatResponse
                {
                    IsSuccess = false,
                    Status = "ERROR",
                    ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    NextIntervalMs = 5000, // 错误情况下缩短间隔
                    ServerInfo = new Dictionary<string, object>
                    {
                        ["Error"] = ex.Message,
                        ["ErrorCode"] = "HEARTBEAT_ERROR"
                    }
                };

                return ResponseFactory.CreateSpecificErrorResponse(queuedCommand.Packet.ExecutionContext, ex, "处理心跳命令异常");
            }
        }



    }
}