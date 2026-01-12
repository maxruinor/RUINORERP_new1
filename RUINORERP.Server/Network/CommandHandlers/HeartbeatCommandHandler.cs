using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 心跳命令处理器
    /// 处理客户端发送的心跳命令，维持连接活跃状态
    /// 优化版：动态计算心跳间隔，增强错误处理
    /// </summary>
    [CommandHandler("HeartbeatCommandHandler", priority: 0)]
    public class HeartbeatCommandHandler : BaseCommandHandler
    {
        private readonly ISessionService _sessionService;


        /// <summary>
        /// 构造函数 - 通过依赖注入获取服务
        /// </summary>
        public HeartbeatCommandHandler(
             ISessionService sessionService,
            ILogger<HeartbeatCommandHandler> logger) : base(logger ?? throw new ArgumentNullException(nameof(logger)))
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
                    return await HandleHeartbeatAsync(cmd, cancellationToken);
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
        private async Task<IResponse> HandleHeartbeatAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken)
        {
            try
            {
                SessionInfo sessionInfo = new SessionInfo();

                if (queuedCommand.Packet.Request is HeartbeatRequest heartbeatRequest)
                {
                    // 确保 UserInfo 不为空
                    if (heartbeatRequest.UserInfo != null)
                    {
                        sessionInfo = SessionService.GetSession(heartbeatRequest.UserInfo.UserID);
                        if (heartbeatRequest.UserInfo.UserID == 0 || sessionInfo == null)
                        {
                            // 创建心跳响应数据
                            var responseNotLogin = new HeartbeatResponse
                            {
                                IsSuccess = true,
                                ErrorMessage = "用户不存在",
                                Status = "OK",
                                ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                                NextIntervalMs = 30000, // 默认30秒间隔
                                ServerInfo = new Dictionary<string, object>
                                {
                                    ["ServerTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    ["ServerVersion"] = "1.0.0",
                                }
                            };

                            return responseNotLogin;
                        }

                        sessionInfo.UserInfo = heartbeatRequest.UserInfo;
                    }

                    // 保存客户端系统信息到会话中
                    if (heartbeatRequest.SystemInfo != null)
                    {
                        sessionInfo.ClientSystemInfo = heartbeatRequest.SystemInfo;
                    }

                    // 动态计算下次心跳间隔
                    var nextIntervalMs = CalculateNextHeartbeatInterval(sessionInfo);

                    // 创建心跳响应数据
                    var response = new HeartbeatResponse
                    {
                        IsSuccess = true,
                        Status = "OK",
                        ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        NextIntervalMs = nextIntervalMs, // 使用动态计算的间隔
                        ServerInfo = new Dictionary<string, object>
                        {
                            ["ServerTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            ["ServerVersion"] = "1.0.0",
                            ["SessionId"] = sessionInfo.SessionID,
                            ["RecommendedInterval"] = nextIntervalMs // 推荐的心跳间隔
                        }
                    };

                    // 更新会话最后活动时间
                    sessionInfo.LastActivityTime = DateTime.Now;
                    SessionService.UpdateSession(sessionInfo);

                    return response;
                }
                else
                {
                    // 请求类型不正确
                    LogError($"心跳请求数据类型错误: {queuedCommand.Packet.Request?.GetType()}");
                    return ResponseFactory.CreateSpecificErrorResponse(queuedCommand.Packet, "心跳请求数据格式错误");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理心跳命令异常: {ex.Message}", ex);
                LogWarning($"[主动断开连接] 心跳命令处理异常，可能导致连接中断: {ex.Message}");
                
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

        /// <summary>
        /// 动态计算下次心跳间隔
        /// 根据网络状态和会话活跃度智能调整心跳间隔
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>下次心跳间隔（毫秒）</returns>
        private int CalculateNextHeartbeatInterval(SessionInfo sessionInfo)
        {
            // 默认30秒间隔
            const int defaultIntervalMs = 30000;
            const int minIntervalMs = 15000;  // 最小15秒
            const int maxIntervalMs = 60000;  // 最大60秒

            // 如果会话信息无效，使用默认间隔
            if (sessionInfo == null)
            {
                //logger?.LogWarning("会话信息为空，使用默认心跳间隔");
                return defaultIntervalMs;
            }

            // 根据会话活跃度调整间隔
            var timeSinceLastActivity = DateTime.Now - sessionInfo.LastActivityTime;
            var intervalMs = defaultIntervalMs;

            if (timeSinceLastActivity.TotalMinutes < 5)
            {
                // 5分钟内有活动，使用较短间隔（快速检测断线）
                intervalMs = minIntervalMs;
            }
            else if (timeSinceLastActivity.TotalMinutes < 30)
            {
                // 5-30分钟内有活动，使用默认间隔
                intervalMs = defaultIntervalMs;
            }
            else
            {
                // 超过30分钟无活动，使用较长间隔（节省资源）
                intervalMs = maxIntervalMs;
            }

            //logger?.LogDebug("动态计算心跳间隔: {IntervalMs}ms (最后活动: {Minutes}分钟前)", intervalMs, timeSinceLastActivity.TotalMinutes);

            return intervalMs;
        }


    }
}
