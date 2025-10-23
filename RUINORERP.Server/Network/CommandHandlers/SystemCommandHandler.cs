using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using static RUINORERP.PacketSpec.Models.Requests.SystemCommandRequest;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 系统命令处理器 - 处理电脑状态查询、关闭电脑、退出系统等系统级命令
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
                SystemCommands.ShutdownComputer,
                SystemCommands.ExitSystem
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
                    return ResponseBase.CreateError("权限不足，仅管理员可执行此操作", 403)
                        .WithMetadata("ErrorCode", "INSUFFICIENT_PERMISSIONS");
                }

                if (commandId == SystemCommands.ComputerStatus)
                {
                    return await HandleComputerStatusAsync(cmd, cancellationToken);
                }
                else if (commandId == SystemCommands.ShutdownComputer)
                {
                    return await HandleShutdownComputerAsync(cmd, cancellationToken);
                }
                else if (commandId == SystemCommands.ExitSystem)
                {
                    return await HandleExitSystemAsync(cmd, cancellationToken);
                }
                else
                {
                    return ResponseBase.CreateError($"不支持的系统命令: {commandId.Name}")
                        .WithMetadata("ErrorCode", "UNSUPPORTED_SYSTEM_COMMAND");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理系统命令时出错: {Message}", ex.Message);
                return CreateExceptionResponse(ex, "SYSTEM_HANDLER_ERROR");
            }
        }

        /// <summary>
        /// 处理电脑状态查询命令
        /// </summary>
        private async Task<ResponseBase> HandleComputerStatusAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (cmd.Packet.Request is SystemCommandRequest request)
                {
                    if (!request.IsValid())
                    {
                        return SystemCommandResponse.CreateComputerStatusFailure("请求参数无效", "INVALID_REQUEST");
                    }

                    // 查找目标用户会话
                    var targetSessions = _sessionService.GetUserSessions(request.TargetUserId);
                    if (!targetSessions.Any())
                    {
                        return SystemCommandResponse.CreateComputerStatusFailure("目标用户不在线", "USER_OFFLINE");
                    }

                    // 向目标用户发送状态查询请求
                    var targetSession = targetSessions.First();
                    var response = await SendRequestToClientAsync<SystemCommandResponse>(
                        targetSession.SessionID, 
                        SystemCommands.ComputerStatus, 
                        SystemCommandRequest.CreateComputerStatusRequest(request.TargetUserId, request.RequestType),
                        cancellationToken);

                    if (response != null && response.IsSuccess)
                    {
                        _logger?.LogInformation("电脑状态查询成功 - 目标用户: {TargetUserId}", request.TargetUserId);
                        return response;
                    }
                    else
                    {
                        return SystemCommandResponse.CreateComputerStatusFailure(
                            response?.Message ?? "无法获取目标电脑状态", 
                            response?.Metadata?.ContainsKey("ErrorCode") == true ? 
                                response.Metadata["ErrorCode"].ToString() : "STATUS_QUERY_FAILED");
                    }
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
        private async Task<ResponseBase> HandleShutdownComputerAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (cmd.Packet.Request is SystemCommandRequest request)
                {
                    if (!request.IsValid())
                    {
                        return SystemCommandResponse.CreateShutdownFailure("请求参数无效", "INVALID_REQUEST");
                    }

                    // 查找目标用户会话
                    var targetSessions = _sessionService.GetUserSessions(request.TargetUserId);
                    if (!targetSessions.Any())
                    {
                        return SystemCommandResponse.CreateShutdownFailure("目标用户不在线", "USER_OFFLINE");
                    }

                    // 向目标用户发送关闭指令
                    var targetSession = targetSessions.First();
                    var response = await SendRequestToClientAsync<SystemCommandResponse>(
                        targetSession.SessionID, 
                        SystemCommands.ShutdownComputer, 
                        SystemCommandRequest.CreateShutdownRequest(request.TargetUserId, request.ShutdownType, request.DelaySeconds, request.AdminRemark),
                        cancellationToken);

                    if (response != null && response.IsSuccess)
                    {
                        _logger?.LogInformation("关闭电脑指令发送成功 - 目标用户: {TargetUserId}, 类型: {ShutdownType}", 
                            request.TargetUserId, request.ShutdownType);
                        return response;
                    }
                    else
                    {
                        return SystemCommandResponse.CreateShutdownFailure(
                            response?.Message ?? "无法发送关闭指令", 
                            response?.Metadata?.ContainsKey("ErrorCode") == true ? 
                                response.Metadata["ErrorCode"].ToString() : "SHUTDOWN_FAILED");
                    }
                }
                else
                {
                    return SystemCommandResponse.CreateShutdownFailure("请求格式错误", "INVALID_REQUEST_FORMAT");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理关闭电脑命令时出错");
                return SystemCommandResponse.CreateShutdownFailure($"处理失败: {ex.Message}", "SHUTDOWN_ERROR");
            }
        }

        /// <summary>
        /// 处理退出系统命令
        /// </summary>
        private async Task<ResponseBase> HandleExitSystemAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (cmd.Packet.Request is SystemCommandRequest request)
                {
                    if (!request.IsValid())
                    {
                        return SystemCommandResponse.CreateShutdownFailure("请求参数无效", "INVALID_REQUEST");
                    }

                    // 查找目标用户会话
                    var targetSessions = _sessionService.GetUserSessions(request.TargetUserId);
                    if (!targetSessions.Any())
                    {
                        return SystemCommandResponse.CreateShutdownFailure("目标用户不在线", "USER_OFFLINE");
                    }

                    // 向目标用户发送退出系统指令
                    var targetSession = targetSessions.First();
                    var response = await SendRequestToClientAsync<SystemCommandResponse>(
                        targetSession.SessionID, 
                        SystemCommands.ExitSystem, 
                        SystemCommandRequest.CreateExitSystemRequest(request.TargetUserId, request.DelaySeconds, request.AdminRemark),
                        cancellationToken);

                    if (response != null && response.IsSuccess)
                    {
                        _logger?.LogInformation("退出系统指令发送成功 - 目标用户: {TargetUserId}", request.TargetUserId);
                        return response;
                    }
                    else
                    {
                        return SystemCommandResponse.CreateShutdownFailure(
                            response?.Message ?? "无法发送退出系统指令", 
                            response?.Metadata?.ContainsKey("ErrorCode") == true ? 
                                response.Metadata["ErrorCode"].ToString() : "EXIT_SYSTEM_FAILED");
                    }
                }
                else
                {
                    return SystemCommandResponse.CreateShutdownFailure("请求格式错误", "INVALID_REQUEST_FORMAT");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理退出系统命令时出错");
                return SystemCommandResponse.CreateShutdownFailure($"处理失败: {ex.Message}", "EXIT_SYSTEM_ERROR");
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
        /// 向客户端发送请求并等待响应
        /// </summary>
        private async Task<T> SendRequestToClientAsync<T>(
            string sessionId, 
            CommandId commandId, 
            IRequest request, 
            CancellationToken cancellationToken) where T :  ResponseBase
        {
            // 这里需要实现实际的客户端通信逻辑
            // 由于这是一个示例，我们返回一个模拟的响应
            await Task.Delay(100, cancellationToken);
            
            if (typeof(T) == typeof(SystemCommandResponse))
            {
                return new SystemCommandResponse
                {
                    IsSuccess = true,
                    Message = "模拟响应",
                    UserId = "user123",
                    UserName = "测试用户",
                    ComputerName = "TestPC",
                    IpAddress = "192.168.1.100",
                    CpuUsage = 25.5f,
                    MemoryUsage = 45.2f,
                    SystemUptime = 3600,
                    ClientVersion = "1.0.0",
                    ConnectionStatus = "Connected"
                } as T;
            }

            return null;
        }

        /// <summary>
        /// 创建统一的异常响应
        /// </summary>
        private ResponseBase CreateExceptionResponse(Exception ex, string errorCode)
        {
            return ResponseBase.CreateError($"[{ex.GetType().Name}] {ex.Message}")
                .WithMetadata("ErrorCode", errorCode)
                .WithMetadata("Exception", ex.Message)
                .WithMetadata("StackTrace", ex.StackTrace);
        }
    }
}