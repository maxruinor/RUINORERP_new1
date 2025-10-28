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
using RUINORERP.Model.TransModel;
using System.Windows.Forms;

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
                        var id when id == MessageCommands.BroadcastMessage =>
                            await HandleBroadcastMessageAsync(messageRequest, cmd.Packet.ExecutionContext, cancellationToken),
                        var id when id == MessageCommands.SendSystemNotification =>
                            await HandleSendSystemNotificationAsync(messageRequest, cmd.Packet.ExecutionContext, cancellationToken),
                        _ => ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, $"不支持的消息命令: {commandId.Name}")
                                    
                    };
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "消息请求格式错误  ");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理消息命令时出错");
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理消息命令时出错  ");
            }
        }

        /// <summary>
        /// 处理发送弹窗消息命令
        /// </summary>
        private async Task<ResponseBase> HandleSendPopupMessageAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("TargetUserId") || !messageData.ContainsKey("Message"))
                {
                    return MessageResponse.Fail(request.CommandType,  "消息数据格式错误");
                }

                var targetUserId = messageData["TargetUserId"].ToString();
                var message = messageData["Message"].ToString();
                var title = messageData.ContainsKey("Title") ? messageData["Title"].ToString() : "系统消息";

                // 查找目标用户的所有会话
                var targetSessions = _sessionService.GetUserSessions(targetUserId);
                if (!targetSessions.Any())
                {
                    _logger?.LogWarning("发送弹窗消息失败：目标用户不在线 - 用户ID: {TargetUserId}", targetUserId);
                    return MessageResponse.Fail(request.CommandType,  "目标用户不在线");
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

                        var response = MessageResponse.Success(MessageCmdType.Prompt , responseMessage);
                        // 使用SessionService发送响应
                        var messageRequest = new MessageRequest(MessageCmdType.Prompt, responseMessage);
                        var success = await _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.SendPopupMessage, 
                            messageRequest, 
                            cancellationToken);
                        
                        if (success)
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
                return MessageResponse.Fail(request.CommandType,  $"处理消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理转发弹窗消息命令
        /// </summary>
        private async Task<ResponseBase> HandleForwardPopupMessageAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("OriginalMessageId") || !messageData.ContainsKey("TargetUserIds"))
                {
                    return MessageResponse.Fail(request.CommandType,  "转发消息数据格式错误");
                }

                var originalMessageId = messageData["OriginalMessageId"].ToString();
                var targetUserIds = messageData["TargetUserIds"] as IEnumerable<object>;

                if (targetUserIds == null)
                {
                    return MessageResponse.Fail(request.CommandType,  "目标用户列表格式错误");
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

                            var response = MessageResponse.Success( MessageCmdType.Prompt, forwardMessage);
                            // 使用SessionService发送响应
                            var messageRequest = new MessageRequest(MessageCmdType.Prompt, forwardMessage);
                            var success = await _sessionService.SendCommandAsync(
                                session.SessionID, 
                                MessageCommands.ForwardPopupMessage, 
                                messageRequest, 
                                cancellationToken);
                            
                            if (success)
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
                return MessageResponse.Fail(request.CommandType,  $"处理转发消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理发送消息给指定用户命令
        /// </summary>
        private async Task<ResponseBase> HandleSendMessageToUserAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("TargetUserId") || !messageData.ContainsKey("Message"))
                {
                    return MessageResponse.Fail(request.CommandType,  "消息数据格式错误");
                }

                var targetUserId = messageData["TargetUserId"].ToString();
                var message = messageData["Message"].ToString();
                var messageType = messageData.ContainsKey("MessageType") ? messageData["MessageType"].ToString() : "Text";

                // 查找目标用户的所有会话
                var targetSessions = _sessionService.GetUserSessions(targetUserId);
                if (!targetSessions.Any())
                {
                    _logger?.LogWarning("发送用户消息失败：目标用户不在线 - 用户ID: {TargetUserId}", targetUserId);
                    return MessageResponse.Fail(request.CommandType,  "目标用户不在线");
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

                        var response = MessageResponse.Success( MessageCmdType.Message, userMessage);
                        // 使用SessionService发送响应
                        var messageRequest = new MessageRequest(MessageCmdType.Message, userMessage);
                        var success = await _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.SendMessageToUser, 
                            messageRequest, 
                            cancellationToken);
                        
                        if (success)
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
                return MessageResponse.Fail(request.CommandType,  $"处理消息失败: {ex.Message}");
            }
        }

        

        /// <summary>
        /// 处理广播消息命令
        /// </summary>
        private async Task<ResponseBase> HandleBroadcastMessageAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("Message"))
                {
                    return MessageResponse.Fail(request.CommandType,  "消息数据格式错误");
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

                        var response = MessageResponse.Success( MessageCmdType.Message, broadcastMessage);
                        // 使用SessionService发送响应
                        var messageRequest = new MessageRequest(MessageCmdType.Message, broadcastMessage);
                        var success = await _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.BroadcastMessage, 
                            messageRequest, 
                            cancellationToken);
                        
                        if (success)
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
                return MessageResponse.Fail(request.CommandType,  $"处理广播消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理发送系统通知命令
        /// </summary>
        private async Task<ResponseBase> HandleSendSystemNotificationAsync(
            MessageRequest request,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                var messageData = request.Data as IDictionary<string, object>;
                if (messageData == null || !messageData.ContainsKey("Message"))
                {
                    return MessageResponse.Fail(request.CommandType,  "通知数据格式错误");
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

                        var response = MessageResponse.Success(MessageCmdType.Message, notificationMessage);
                        // 使用SessionService发送响应
                        var messageRequest = new MessageRequest(MessageCmdType.Message, notificationMessage);
                        var success = await _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.SendSystemNotification, 
                            messageRequest, 
                            cancellationToken);
                        
                        if (success)
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
                return MessageResponse.Fail(request.CommandType,  $"处理系统通知失败: {ex.Message}");
            }
        }

      


    }
}