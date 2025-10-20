using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 消息命令处理器 - 处理客户端发送的消息命令并分发给其他客户端
    /// 支持点对点消息、群发消息、广播消息等多种消息类型
    /// </summary>
    [CommandHandler("MessageCommandHandler", priority: 60)]
    public class MessageCommandHandler : BaseCommandHandler
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<MessageCommandHandler> _logger;

        public MessageCommandHandler(
            SessionService sessionService,
            ILogger<MessageCommandHandler> logger = null)
        {
            _sessionService = sessionService;
            _logger = logger;

            // 设置支持的命令
            SetSupportedCommands(
                MessageCommands.SendPopupMessage,
                MessageCommands.ForwardPopupMessage,
                MessageCommands.SendMessageToUser,
                MessageCommands.SendMessageToDepartment,
                MessageCommands.BroadcastMessage,
                MessageCommands.SendSystemNotification
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

                if (cmd.Packet.Request is MessageRequest messageRequest)
                {
                    return commandId switch
                    {
                        var id when id == MessageCommands.SendPopupMessage =>
                            await HandleSendPopupMessageAsync(messageRequest, cmd.Packet.ExecutionContext, cancellationToken),
                        var id when id == MessageCommands.ForwardPopupMessage =>
                            await HandleForwardPopupMessageAsync(messageRequest, cmd.Packet.ExecutionContext, cancellationToken),
                        var id when id == MessageCommands.SendMessageToUser =>
                            await HandleSendMessageToUserAsync(messageRequest, cmd.Packet.ExecutionContext, cancellationToken),
                        var id when id == MessageCommands.SendMessageToDepartment =>
                            await HandleSendMessageToDepartmentAsync(messageRequest, cmd.Packet.ExecutionContext, cancellationToken),
                        var id when id == MessageCommands.BroadcastMessage =>
                            await HandleBroadcastMessageAsync(messageRequest, cmd.Packet.ExecutionContext, cancellationToken),
                        var id when id == MessageCommands.SendSystemNotification =>
                            await HandleSendSystemNotificationAsync(messageRequest, cmd.Packet.ExecutionContext, cancellationToken),
                        _ => ResponseBase.CreateError($"不支持的消息命令: {commandId.Name}")
                                    .WithMetadata("ErrorCode", "UNSUPPORTED_MESSAGE_COMMAND")
                    };
                }
                else
                {
                    return ResponseBase.CreateError("消息请求格式错误")
                        .WithMetadata("ErrorCode", "INVALID_MESSAGE_REQUEST");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理消息命令时出错");
                return CreateExceptionResponse(ex, "MESSAGE_HANDLER_ERROR");
            }
        }

        /// <summary>
        /// 处理发送弹窗消息命令
        /// </summary>
        private async Task<IResponse> HandleSendPopupMessageAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("TargetUserId") || !messageData.ContainsKey("Message"))
                {
                    return MessageResponse.Fail(request.CommandType, 400, "消息数据格式错误");
                }

                var targetUserId = messageData["TargetUserId"].ToString();
                var message = messageData["Message"].ToString();
                var title = messageData.ContainsKey("Title") ? messageData["Title"].ToString() : "系统消息";

                // 查找目标用户的所有会话
                var targetSessions = _sessionService.GetUserSessions(targetUserId);
                if (!targetSessions.Any())
                {
                    _logger?.LogWarning("发送弹窗消息失败：目标用户不在线 - 用户ID: {TargetUserId}", targetUserId);
                    return MessageResponse.Fail(request.CommandType, 404, "目标用户不在线");
                }

                // 向目标用户的所有会话发送消息
                var sendCount = 0;
                foreach (var session in targetSessions)
                {
                    try
                    {
                        var responseMessage = new
                        {
                            Title = title,
                            Content = message,
                            Sender = executionContext.UserId,
                            Timestamp = DateTime.Now
                        };

                        var response = MessageResponse.Success(MessageCommands.SendPopupMessage.OperationCode, responseMessage);
                        await SendResponseToSessionAsync(session.SessionID, response, cancellationToken);
                        sendCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "向会话发送弹窗消息失败 - 会话ID: {SessionId}", session.SessionID);
                    }
                }

                // 只记录关键信息
                if (sendCount > 0)
                {
                    _logger?.LogDebug("弹窗消息发送完成 - 目标用户: {TargetUserId}, 成功发送: {SendCount} 个会话",
                        targetUserId, sendCount);
                }

                return MessageResponse.Success(request.CommandType, new { SendCount = sendCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理发送弹窗消息命令时出错");
                return MessageResponse.Fail(request.CommandType, 500, $"处理消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理转发弹窗消息命令
        /// </summary>
        private async Task<IResponse> HandleForwardPopupMessageAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("OriginalMessageId") || !messageData.ContainsKey("TargetUserIds"))
                {
                    return MessageResponse.Fail(request.CommandType, 400, "转发消息数据格式错误");
                }

                var originalMessageId = messageData["OriginalMessageId"].ToString();
                var targetUserIds = messageData["TargetUserIds"] as IEnumerable<object>;

                if (targetUserIds == null)
                {
                    return MessageResponse.Fail(request.CommandType, 400, "目标用户列表格式错误");
                }

                var sendCount = 0;
                foreach (var userIdObj in targetUserIds)
                {
                    var targetUserId = userIdObj.ToString();
                    var targetSessions = _sessionService.GetUserSessions(targetUserId);

                    foreach (var session in targetSessions)
                    {
                        try
                        {
                            var forwardMessage = new
                            {
                                OriginalMessageId = originalMessageId,
                                ForwardFrom = executionContext.UserId,
                                ForwardTime = DateTime.Now
                            };

                            var response = MessageResponse.Success(MessageCommands.ForwardPopupMessage.OperationCode, forwardMessage);
                            await SendResponseToSessionAsync(session.SessionID, response, cancellationToken);
                            sendCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "向会话转发弹窗消息失败 - 会话ID: {SessionId}", session.SessionID);
                        }
                    }
                }

                // 只记录关键信息
                if (sendCount > 0)
                {
                    _logger?.LogDebug("转发弹窗消息完成 - 原始消息: {OriginalMessageId}, 成功转发: {SendCount} 个会话",
                        originalMessageId, sendCount);
                }

                return MessageResponse.Success(request.CommandType, new { SendCount = sendCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理转发弹窗消息命令时出错");
                return MessageResponse.Fail(request.CommandType, 500, $"处理转发消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理发送消息给指定用户命令
        /// </summary>
        private async Task<IResponse> HandleSendMessageToUserAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("TargetUserId") || !messageData.ContainsKey("Message"))
                {
                    return MessageResponse.Fail(request.CommandType, 400, "消息数据格式错误");
                }

                var targetUserId = messageData["TargetUserId"].ToString();
                var message = messageData["Message"].ToString();
                var messageType = messageData.ContainsKey("MessageType") ? messageData["MessageType"].ToString() : "Text";

                // 查找目标用户的所有会话
                var targetSessions = _sessionService.GetUserSessions(targetUserId);
                if (!targetSessions.Any())
                {
                    _logger?.LogWarning("发送用户消息失败：目标用户不在线 - 用户ID: {TargetUserId}", targetUserId);
                    return MessageResponse.Fail(request.CommandType, 404, "目标用户不在线");
                }

                // 向目标用户的所有会话发送消息
                var sendCount = 0;
                foreach (var session in targetSessions)
                {
                    try
                    {
                        var userMessage = new
                        {
                            Content = message,
                            MessageType = messageType,
                            Sender = executionContext.UserId,
                            Timestamp = DateTime.Now
                        };

                        var response = MessageResponse.Success(MessageCommands.SendMessageToUser.OperationCode, userMessage);
                        await SendResponseToSessionAsync(session.SessionID, response, cancellationToken);
                        sendCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "向会话发送用户消息失败 - 会话ID: {SessionId}", session.SessionID);
                    }
                }

                // 只记录关键信息
                if (sendCount > 0)
                {
                    _logger?.LogDebug("用户消息发送完成 - 目标用户: {TargetUserId}, 成功发送: {SendCount} 个会话",
                        targetUserId, sendCount);
                }

                return MessageResponse.Success(request.CommandType, new { SendCount = sendCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理发送用户消息命令时出错");
                return MessageResponse.Fail(request.CommandType, 500, $"处理消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理发送消息给指定部门命令
        /// </summary>
        private async Task<IResponse> HandleSendMessageToDepartmentAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("DepartmentId") || !messageData.ContainsKey("Message"))
                {
                    return MessageResponse.Fail(request.CommandType, 400, "消息数据格式错误");
                }

                var departmentId = messageData["DepartmentId"].ToString();
                var message = messageData["Message"].ToString();
                var messageType = messageData.ContainsKey("MessageType") ? messageData["MessageType"].ToString() : "Text";

                // TODO: 实际项目中需要根据部门ID获取部门下的所有用户
                // 这里简化处理，假设部门ID就是用户ID列表的分隔符
                var departmentUserIds = departmentId.Split(',');

                var sendCount = 0;
                foreach (var userId in departmentUserIds)
                {
                    var targetSessions = _sessionService.GetUserSessions(userId.Trim());

                    foreach (var session in targetSessions)
                    {
                        try
                        {
                            var departmentMessage = new
                            {
                                Content = message,
                                MessageType = messageType,
                                DepartmentId = departmentId,
                                Sender = executionContext.UserId,
                                Timestamp = DateTime.Now
                            };

                            var response = MessageResponse.Success(MessageCommands.SendMessageToDepartment.OperationCode, departmentMessage);
                            await SendResponseToSessionAsync(session.SessionID, response, cancellationToken);
                            sendCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "向会话发送部门消息失败 - 会话ID: {SessionId}", session.SessionID);
                        }
                    }
                }

                // 只记录关键信息
                if (sendCount > 0)
                {
                    _logger?.LogDebug("部门消息发送完成 - 部门: {DepartmentId}, 成功发送: {SendCount} 个会话",
                        departmentId, sendCount);
                }

                return MessageResponse.Success(request.CommandType, new { SendCount = sendCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理发送部门消息命令时出错");
                return MessageResponse.Fail(request.CommandType, 500, $"处理消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理广播消息命令
        /// </summary>
        private async Task<IResponse> HandleBroadcastMessageAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("Message"))
                {
                    return MessageResponse.Fail(request.CommandType, 400, "消息数据格式错误");
                }

                var message = messageData["Message"].ToString();
                var messageType = messageData.ContainsKey("MessageType") ? messageData["MessageType"].ToString() : "Text";

                // 获取所有已认证的用户会话（排除发送者自己）
                var allSessions = _sessionService.GetAllUserSessions(executionContext.SessionId);

                var sendCount = 0;
                foreach (var session in allSessions)
                {
                    try
                    {
                        var broadcastMessage = new
                        {
                            Content = message,
                            MessageType = messageType,
                            Sender = executionContext.UserId,
                            Timestamp = DateTime.Now
                        };

                        var response = MessageResponse.Success(MessageCommands.BroadcastMessage.OperationCode, broadcastMessage);
                        await SendResponseToSessionAsync(session.SessionID, response, cancellationToken);
                        sendCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "向会话广播消息失败 - 会话ID: {SessionId}", session.SessionID);
                    }
                }

                // 只记录关键信息
                if (sendCount > 0)
                {
                    _logger?.LogDebug("广播消息发送完成 - 成功发送: {SendCount} 个会话", sendCount);
                }

                return MessageResponse.Success(request.CommandType, new { SendCount = sendCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理广播消息命令时出错");
                return MessageResponse.Fail(request.CommandType, 500, $"处理广播消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理发送系统通知命令
        /// </summary>
        private async Task<IResponse> HandleSendSystemNotificationAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("Message"))
                {
                    return MessageResponse.Fail(request.CommandType, 400, "通知数据格式错误");
                }

                var message = messageData["Message"].ToString();
                var notificationType = messageData.ContainsKey("NotificationType") ? messageData["NotificationType"].ToString() : "Info";

                // 获取所有已认证的用户会话
                var allSessions = _sessionService.GetAllUserSessions();

                var sendCount = 0;
                foreach (var session in allSessions)
                {
                    try
                    {
                        var notificationMessage = new
                        {
                            Content = message,
                            NotificationType = notificationType,
                            Timestamp = DateTime.Now
                        };

                        var response = MessageResponse.Success(MessageCommands.SendSystemNotification.OperationCode, notificationMessage);
                        await SendResponseToSessionAsync(session.SessionID, response, cancellationToken);
                        sendCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "向会话发送系统通知失败 - 会话ID: {SessionId}", session.SessionID);
                    }
                }

                // 只记录关键信息
                if (sendCount > 0)
                {
                    _logger?.LogDebug("系统通知发送完成 - 成功发送: {SendCount} 个会话", sendCount);
                }

                return MessageResponse.Success(request.CommandType, new { SendCount = sendCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理发送系统通知命令时出错");
                return MessageResponse.Fail(request.CommandType, 500, $"处理系统通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 向指定会话发送响应
        /// </summary>
        private async Task<bool> SendResponseToSessionAsync(string sessionId, IResponse response, CancellationToken cancellationToken)
        {
            try
            {
                // 这里需要根据实际的通信机制来实现
                // 可能需要将响应序列化并通过网络发送到客户端
                // 以下是一个示例实现，实际实现可能需要根据项目的通信协议进行调整

                // TODO: 实现实际的消息发送逻辑
                // 例如，通过SessionService获取会话并发送消息
                // var session = _sessionService.GetAppSession(sessionId);
                // if (session != null)
                // {
                //     // 发送响应到客户端
                //     await session.SendAsync(response);
                // }

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "向会话发送响应失败 - 会话ID: {SessionId}", sessionId);
                return false;
            }
        }

        /// <summary>
        /// 创建统一的异常响应
        /// </summary>
        private IResponse CreateExceptionResponse(Exception ex, string errorCode)
        {
            return ResponseBase.CreateError($"[{ex.GetType().Name}] {ex.Message}")
                .WithMetadata("ErrorCode", errorCode)
                .WithMetadata("Exception", ex.Message)
                .WithMetadata("StackTrace", ex.StackTrace);
        }
    }
}