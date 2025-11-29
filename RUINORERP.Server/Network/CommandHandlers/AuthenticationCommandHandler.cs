using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Common;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 认证命令处理器 - 处理强制用户下线等认证相关命令
    /// </summary>
    [CommandHandler("AuthenticationCommandHandler", priority: 20)]
    public class AuthenticationCommandHandler : BaseCommandHandler
    {
        private readonly ISessionService _sessionService;
        private readonly ILogger<AuthenticationCommandHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuthenticationCommandHandler(
            ISessionService sessionService,
            ILogger<AuthenticationCommandHandler> logger = null) : base(logger)
        {
            _sessionService = sessionService;
            _logger = logger;

            // 设置支持的命令
            SetSupportedCommands(
                AuthenticationCommands.ForceLogout
            );
        }

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Packet.CommandId;

                // 验证管理员权限
                if (!await ValidateAdminPermissionAsync(cmd.Packet.ExecutionContext))
                {
                    return SystemCommandResponse.CreateForceLogoutFailure("权限不足，仅管理员可执行此操作", "INSUFFICIENT_PERMISSIONS");
                }

                if (commandId == AuthenticationCommands.ForceLogout)
                {
                    return await HandleForceLogoutAsync(cmd, cancellationToken);
                }
                else
                {
                    return SystemCommandResponse.CreateForceLogoutFailure($"不支持的认证命令: {commandId.Name}", "UNSUPPORTED_AUTH_COMMAND");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理认证命令时出错: {Message}", ex.Message);
                return CreateExceptionResponse(ex, "AUTH_HANDLER_ERROR");
            }
        }

        /// <summary>
        /// 处理强制用户下线命令
        /// </summary>
        private async Task<IResponse> HandleForceLogoutAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (cmd.Packet.Request is SystemCommandRequest request)
                {
                    // 查找目标用户会话
                    var targetSessions = _sessionService.GetUserSessions(request.TargetUserId);
                    if (!targetSessions.Any())
                    {
                        return SystemCommandResponse.CreateForceLogoutFailure("目标用户不在线", "USER_OFFLINE");
                    }

                    // 向目标用户发送强制下线通知
                    var targetSession = targetSessions.First();
                    var response = await SendForceLogoutNotificationAsync(
                        targetSession.SessionID,
                        request,
                        cancellationToken);

                    if (response != null && response.IsSuccess)
                    {
                        _logger?.LogInformation("强制用户下线指令发送成功 - 目标用户: {TargetUserId}", request.TargetUserId);

                        // 返回成功响应
                        return SystemCommandResponse.CreateForceLogoutSuccess(
                            request.TargetUserId,
                            targetSession.UserName ?? "未知用户",
                            request.AdminUserId);
                    }
                    else
                    {
                        return SystemCommandResponse.CreateForceLogoutFailure(
                            response?.Message ?? "无法发送强制下线指令",
                            response?.Metadata?.ContainsKey("ErrorCode") == true ?
                                response.Metadata["ErrorCode"].ToString() : "FORCE_LOGOUT_FAILED");
                    }
                }
                else
                {
                    return SystemCommandResponse.CreateForceLogoutFailure("请求格式错误", "INVALID_REQUEST_FORMAT");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理强制用户下线命令时出错");
                return SystemCommandResponse.CreateForceLogoutFailure($"处理失败: {ex.Message}", "FORCE_LOGOUT_ERROR");
            }
        }

        /// <summary>
        /// 验证管理员权限
        /// </summary>
        private async Task<bool> ValidateAdminPermissionAsync(CommandContext context)
        {
            try
            {
                // 获取当前会话
                var session = _sessionService.GetSession(context.SessionId);
                if (session == null)
                {
                    return false;
                }

                // 检查是否为管理员
                return session.IsAdmin;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证管理员权限时出错");
                return false;
            }
        }

        /// <summary>
        /// 发送强制下线通知到客户端
        /// </summary>
        private async Task<IResponse> SendForceLogoutNotificationAsync(
            string sessionId,
            SystemCommandRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                // 通知客户端强制下线
                await _sessionService.DisconnectSessionAsync(sessionId, $"管理员[{request.AdminUserId}]强制下线");

                // 创建成功响应
                var response = new ResponseBase
                {
                    IsSuccess = true,
                    Message = "用户已成功强制下线"
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送强制下线通知时出错");
                return SystemCommandResponse.CreateForceLogoutFailure($"发送通知失败: {ex.Message}", "NOTIFICATION_ERROR");
            }
        }

        /// <summary>
        /// 创建统一的异常响应
        /// </summary>
        private IResponse CreateExceptionResponse(Exception ex, string errorCode)
        {
            return SystemCommandResponse.CreateForceLogoutFailure($"处理异常: {ex.Message}", errorCode);
        }
    }
}