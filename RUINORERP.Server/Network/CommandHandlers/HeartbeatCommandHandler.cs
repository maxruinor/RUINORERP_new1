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
        /// 处理心跳命令（优化版 - 异步处理提高响应速度）
        /// </summary>
        private async Task<IResponse> HandleHeartbeatAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken)
        {
            var startTime = DateTime.Now;
            
            try
            {
                SessionInfo sessionInfo = new SessionInfo();

                if (queuedCommand.Packet.Request is HeartbeatRequest heartbeatRequest)
                {
                    // 使用UserId进行会话验证，不再依赖完整的UserInfo
                    sessionInfo = SessionService.GetSession(heartbeatRequest.UserId);
                    
                    if (heartbeatRequest.UserId == 0 || sessionInfo == null)
                    {
                        var responseNotLogin = HeartbeatResponse.Create(false, "用户不存在或会话已过期")
                            .WithNextInterval(30000);
                        return responseNotLogin;
                    }

                    // 立即更新最后活动时间（快速路径）
                    sessionInfo.LastActivityTime = DateTime.Now;

                    // 动态计算下次心跳间隔
                    var nextIntervalMs = CalculateNextHeartbeatInterval(sessionInfo);

                    // 创建心跳响应（快速返回，不等待异步处理）
                    var response = new HeartbeatResponse
                    {
                        IsSuccess = true,
                        Status = "OK",
                        ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        NextIntervalMs = nextIntervalMs,
                        ServerInfo = new Dictionary<string, object>
                        {
                            ["ServerTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            ["ServerVersion"] = "1.0.0",
                            ["SessionId"] = sessionInfo.SessionID,
                            ["RecommendedInterval"] = nextIntervalMs,
                            ["ProcessingDurationMs"] = 0 // 初始值
                        }
                    };

                    // 异步更新完整会话信息（不阻塞响应）
                    if (heartbeatRequest.UserOperationInfo != null)
                    {
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                // 更新用户操作信息
                                sessionInfo.UserInfo.用户名 = heartbeatRequest.UserOperationInfo.用户名;
                                sessionInfo.UserInfo.姓名 = heartbeatRequest.UserOperationInfo.姓名;
                                sessionInfo.UserInfo.当前模块 = heartbeatRequest.UserOperationInfo.当前模块;
                                sessionInfo.UserInfo.当前窗体 = heartbeatRequest.UserOperationInfo.当前窗体;
                                sessionInfo.UserInfo.登录时间 = heartbeatRequest.UserOperationInfo.登录时间;
                                sessionInfo.UserInfo.心跳数 = heartbeatRequest.UserOperationInfo.心跳数;
                                sessionInfo.UserInfo.客户端版本 = heartbeatRequest.UserOperationInfo.客户端版本;
                                sessionInfo.UserInfo.客户端IP = heartbeatRequest.UserOperationInfo.客户端IP;
                                sessionInfo.UserInfo.静止时间 = heartbeatRequest.UserOperationInfo.静止时间;
                                sessionInfo.UserInfo.超级用户 = heartbeatRequest.UserOperationInfo.超级用户;
                                sessionInfo.UserInfo.授权状态 = heartbeatRequest.UserOperationInfo.授权状态;
                                sessionInfo.UserInfo.操作系统 = heartbeatRequest.UserOperationInfo.操作系统;
                                sessionInfo.UserInfo.机器名 = heartbeatRequest.UserOperationInfo.机器名;
                                sessionInfo.UserInfo.CPU信息 = heartbeatRequest.UserOperationInfo.CPU信息;
                                sessionInfo.UserInfo.内存大小 = heartbeatRequest.UserOperationInfo.内存大小;

                                // 异步更新到SessionService
                                SessionService.UpdateSession(sessionInfo);
                            }
                            catch (Exception ex)
                            {
                                LogError(ex, "异步更新会话信息失败");
                            }
                        }, cancellationToken);
                    }
                    else
                    {
                        // 如果没有用户操作信息，直接同步更新
                        SessionService.UpdateSession(sessionInfo);
                    }

                    var processingDuration = (DateTime.Now - startTime).TotalMilliseconds;
                    
                    if (response.ServerInfo != null)
                    {
                        response.ServerInfo["ProcessingDurationMs"] = processingDuration;
                    }

                    return response;
                }
                else
                {
                    LogError($"心跳请求数据类型错误: {queuedCommand.Packet.Request?.GetType()}");
                    return ResponseFactory.CreateSpecificErrorResponse(queuedCommand.Packet, "心跳请求数据格式错误");
                }
            }
            catch (Exception ex)
            {
                var processingDuration = (DateTime.Now - startTime).TotalMilliseconds;
                LogError(ex, "处理心跳命令异常，耗时: {Duration}ms", processingDuration);
                LogWarning($"[主动断开连接] 心跳命令处理异常，可能导致连接中断: {ex.Message}");
                
                var errorResponse = new HeartbeatResponse
                {
                    IsSuccess = false,
                    Status = "ERROR",
                    ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    NextIntervalMs = 5000,
                    ServerInfo = new Dictionary<string, object>
                    {
                        ["Error"] = ex.Message,
                        ["ErrorCode"] = "HEARTBEAT_ERROR",
                        ["ProcessingDurationMs"] = processingDuration
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
