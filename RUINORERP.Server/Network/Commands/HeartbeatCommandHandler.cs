using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少ISessionService接口定义
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.Server.Network.Services;
// using RUINORERP.Server.Network.Interfaces.Services; // 暂时注释，缺少ISessionService接口定义

namespace RUINORERP.Server.Network.Commands
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
        /// </summary>
        public HeartbeatCommandHandler() : base()
        {
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HeartbeatCommandHandler(
             ISessionService sessionService,
            ILogger<HeartbeatCommandHandler> logger = null) : base(logger)
        {
            _sessionService = sessionService; // 暂时注释，缺少ISessionService接口定义

            // 使用安全方法设置支持的命令
            SetSupportedCommands(SystemCommands.Heartbeat);
        }
 
        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<BaseCommand<IRequest, IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Command.CommandIdentifier;

                if (commandId == SystemCommands.Heartbeat)
                {
                    return  HandleHeartbeatAsync(cmd, cancellationToken);
                }
                else
                {
                    return BaseCommand<IRequest, IResponse>.CreateError($"不支持的命令类型: {cmd.Command.CommandIdentifier}")
                        .WithMetadata("ErrorCode", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理心跳命令异常: {ex.Message}", ex);
                return BaseCommand<IRequest, IResponse>.CreateError($"处理异常: {ex.Message}")
                    .WithMetadata("ErrorCode", "HANDLER_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 会话管理服务
        /// </summary>
        private ISessionService SessionService => Program.ServiceProvider.GetRequiredService<ISessionService>();

        /// <summary>
        /// 处理心跳命令
        /// </summary>
        private BaseCommand<IRequest, IResponse> HandleHeartbeatAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken)
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
                    
                    return BaseCommand<IRequest, IResponse>.CreateError("会话不存在或已过期")
                        .WithMetadata("ErrorCode", "SESSION_NOT_FOUND")
                        .WithMetadata("SessionId", queuedCommand.Packet.ExecutionContext.SessionId);
                }
                
                if (queuedCommand.Command is HeartbeatCommand heartbeatCommand)
                {
                    // 确保 UserInfo 不为空
                    if (heartbeatCommand.Request.UserInfo != null)
                    {
                        heartbeatCommand.Request.UserInfo.SessionId = sessionInfo.SessionID;
                        sessionInfo.UserInfo = heartbeatCommand.Request.UserInfo;
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
                
                return BaseCommand<IRequest, IResponse>.CreateSuccess(response);
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
                
                return BaseCommand<IRequest, IResponse>.CreateError("处理心跳命令异常")
                    .WithMetadata("ErrorCode", "HEARTBEAT_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

       
       
    }
}