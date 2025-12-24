using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using static RUINORERP.PacketSpec.Models.Requests.SystemCommandRequest;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Common;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 系统命令处理器 - 处理电脑状态查询、退出系统等系统级命令
    /// 这应该是处理超级管理员的命令，需要验证管理员权限
    /// </summary>
    [CommandHandler("SystemCommandHandler", priority: 10)]
    public class SystemCommandHandler : BaseCommandHandler
    {
        private readonly ISessionService _sessionService;
        private readonly ILogger<SystemCommandHandler> _logger;

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public SystemCommandHandler(ISessionService sessionService) : base()
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemCommandHandler(
            ISessionService sessionService,
            ILogger<SystemCommandHandler> logger = null) : base(logger)
        {
            _sessionService = sessionService;
            _logger = logger;

            // 设置支持的命令
            SetSupportedCommands(
                SystemCommands.ComputerStatus,
                SystemCommands.ExceptionReport,
                 SystemCommands.SystemManagement
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
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"权限不足，仅管理员可执行此操作{cmd.Packet.CommandId.ToString()}");
                }

                if (commandId == SystemCommands.ComputerStatus)
                {
                    return await HandleComputerStatusAsync(cmd, cancellationToken);
                }
                //else if (commandId == SystemCommands.ExceptionReport)
                //{
                //    return await HandleExceptionReportAsync(cmd, cancellationToken);
                //}
                //else if (commandId == SystemCommands.SystemManagement)
                //{
                //    return await HandleSystemManagementAsync(cmd, cancellationToken);
                //}
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的系统命令: {commandId.Name}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理系统命令时出错: {Message}", ex.Message);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"处理系统命令时出错{cmd.Packet.CommandId.ToString()}");
            }
        }

        /// <summary>
        /// 处理电脑状态查询命令
        /// </summary>
        private async Task<SystemCommandResponse> HandleComputerStatusAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (cmd.Packet.Request is SystemCommandRequest request)
                {


                    // 查找目标用户会话
                    var targetSessions = _sessionService.GetUserSessions(request.TargetUserId);
                    if (!targetSessions.Any())
                    {
                        return SystemCommandResponse.CreateComputerStatusFailure("目标用户不在线", "USER_OFFLINE");
                    }

                    // 向目标用户发送状态查询请求
                    var targetSession = targetSessions.First();
                    var response = await _sessionService.SendCommandAndWaitForResponseAsync<SystemCommandRequest>(
                        targetSession.SessionID,
                        SystemCommands.ComputerStatus,
                        SystemCommandRequest.CreateComputerStatusRequest(request.TargetUserId), 30,
                        cancellationToken);
                    return new SystemCommandResponse();
                   //if (response != null)
                   //{
                   //    _logger?.Debug("电脑状态查询成功 - 目标用户: {TargetUserId}", request.TargetUserId);
                   //    return response;
                   //}
                   //else
                   //{
                   //    return SystemCommandResponse.CreateComputerStatusFailure(
                   //        response?.Message ?? "无法获取目标电脑状态",
                   //        response?.Metadata?.ContainsKey("ErrorCode") == true ?
                   //            response.Metadata["ErrorCode"].ToString() : "STATUS_QUERY_FAILED");
                   //}
                }
                else
                {
                    return SystemCommandResponse.CreateComputerStatusFailure("请求格式错误", "INVALID_REQUEST_FORMAT");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理电脑状态查询命令时出错");
                return SystemCommandResponse.CreateComputerStatusFailure($"处理失败: {ex.Message}", "STATUS_QUERY_ERROR");
            }
        }

        /// <summary>
        /// 处理关闭电脑命令
        /// </summary>
        private async Task HandleExceptionReportAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (cmd.Packet.Request is SystemCommandRequest request)
                {
                    // 查找目标用户会话
                    var targetSessions = _sessionService.GetUserSessions(request.TargetUserId);
                    if (!targetSessions.Any())
                    {
                        // return SystemCommandResponse.CreateShutdownFailure("目标用户不在线", "USER_OFFLINE");
                        return;
                    }

                    SystemCommandRequest systemCommandRequest = new SystemCommandRequest();

                    // 将客户端发过来的异常报告 转发到管理员
                    var targetSession = targetSessions.First();
                    var response = await _sessionService.SendCommandAndWaitForResponseAsync<SystemCommandRequest>(
                        targetSession.SessionID,
                        SystemCommands.ExceptionReport,
                        systemCommandRequest
                        );


                }
                else
                {
                    //return SystemCommandResponse.CreateShutdownFailure("请求格式错误", "INVALID_REQUEST_FORMAT");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理关闭电脑命令时出错");
                //return SystemCommandResponse.CreateShutdownFailure($"处理失败: {ex.Message}", "SHUTDOWN_ERROR");
            }
        }

        /// <summary>
        /// 处理退出系统命令
        /// </summary>
        private async Task HandleSystemManagementAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (cmd.Packet.Request is SystemCommandRequest request)
                {


                    // 查找目标用户会话
                    var targetSessions = _sessionService.GetUserSessions(request.TargetUserId);
                    if (!targetSessions.Any())
                    {
                        //return SystemCommandResponse.CreateShutdownFailure("目标用户不在线", "USER_OFFLINE");
                    }
                    SystemCommandRequest systemCommandRequest = new SystemCommandRequest();
                    var targetSession = targetSessions.First();
                    var response = await _sessionService.SendCommandAndWaitForResponseAsync<SystemCommandRequest>(targetSession.SessionID, SystemCommands.SystemManagement, systemCommandRequest);
                }
                else
                {
                    //return SystemCommandResponse.CreateShutdownFailure("请求格式错误", "INVALID_REQUEST_FORMAT");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理退出系统命令时出错");
                //return SystemCommandResponse.CreateShutdownFailure($"处理失败: {ex.Message}", "EXIT_SYSTEM_ERROR");
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


    }
}