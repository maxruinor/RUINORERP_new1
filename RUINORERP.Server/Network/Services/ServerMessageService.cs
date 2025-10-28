using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using RUINORERP.Server.Network.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 服务器消息服务类 - 提供服务器向客户端发送消息并等待响应的功能
    /// 支持向指定用户、部门发送消息，以及广播消息等功能
    /// 支持双向通信，既可以发送消息也可以等待客户端响应
    /// </summary>
    public class ServerMessageService
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<ServerMessageService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sessionService">会话服务</param>
        /// <param name="logger">日志记录器</param>
        public ServerMessageService(
            SessionService sessionService,
            ILogger<ServerMessageService> logger = null)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _logger = logger;
        }

        /// <summary>
        /// 发送弹窗消息给指定用户并等待响应
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="title">消息标题</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendPopupMessageAsync(
            string targetUserId,
            string message,
            string title = "系统消息",
            int timeoutMs = 30000,
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    TargetUserId = targetUserId,
                    Message = message,
                    Title = title,
                    MessageType = "Popup"
                };

                var request = new MessageRequest(MessageCmdType.Unknown, messageData);
                
                // 获取目标用户的所有会话
                var sessions = _sessionService.GetUserSessions(targetUserId);
                
                // 向第一个会话发送消息并等待响应
                foreach (var session in sessions)
                {
                    if (session is SessionInfo sessionInfo)
                    {
                        var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                            session.SessionID, 
                            MessageCommands.SendPopupMessage, 
                            request, 
                            timeoutMs, 
                            ct);
                        
                        return responsePacket?.Response as MessageResponse ?? 
                               MessageResponse.Fail(MessageCmdType.Unknown,  "未收到有效响应");
                    }
                }
                
                return MessageResponse.Fail(MessageCmdType.Unknown,  "目标用户不在线");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送弹窗消息时发生异常");
                return MessageResponse.Fail(MessageCmdType.Unknown, $"发送消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送消息给指定用户并等待响应
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendMessageToUserAsync(
            string targetUserId,
            string message,
            string messageType = "Text",
            int timeoutMs = 30000,
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    TargetUserId = targetUserId,
                    Message = message,
                    MessageType = messageType
                };

                var request = new MessageRequest(MessageCmdType.Unknown, messageData);
                
                // 获取目标用户的所有会话
                var sessions = _sessionService.GetUserSessions(targetUserId);
                
                // 向第一个会话发送消息并等待响应
                foreach (var session in sessions)
                {
                    if (session is SessionInfo sessionInfo)
                    {
                        var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                            session.SessionID, 
                            MessageCommands.SendMessageToUser, 
                            request, 
                            timeoutMs, 
                            ct);
                        
                        return responsePacket?.Response as MessageResponse ?? 
                               MessageResponse.Fail(MessageCmdType.Unknown,  "未收到有效响应");
                    }
                }
                
                return MessageResponse.Fail(MessageCmdType.Unknown,  "目标用户不在线");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送用户消息时发生异常");
                return MessageResponse.Fail(MessageCmdType.Unknown,  $"发送消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送消息给指定部门并等待响应
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendMessageToDepartmentAsync(
            string departmentId,
            string message,
            string messageType = "Text",
            int timeoutMs = 30000,
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    DepartmentId = departmentId,
                    Message = message,
                    MessageType = messageType
                };

                var request = new MessageRequest(MessageCmdType.Unknown, messageData);
                
                // 获取目标部门用户的所有会话
                // 这里简化处理，实际项目中需要根据部门ID获取部门下的所有用户
                var sessions = _sessionService.GetAllUserSessions();
                
                // 向所有会话发送消息并等待响应
                int successCount = 0;
                foreach (var session in sessions)
                {
                    if (session is SessionInfo sessionInfo)
                    {
                        var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                            session.SessionID, 
                            MessageCommands.SendMessageToDepartment, 
                            request, 
                            timeoutMs, 
                            ct);
                        
                        if (responsePacket?.Response is MessageResponse response && response.IsSuccess)
                        {
                            successCount++;
                        }
                    }
                }
                
                return MessageResponse.Success(MessageCmdType.Unknown, new { SendCount = successCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送部门消息时发生异常");
                return MessageResponse.Fail(MessageCmdType.Message, $"发送消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 广播消息给所有用户并等待响应
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> BroadcastMessageAsync(
            string message,
            string messageType = "Text",
            int timeoutMs = 30000,
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    Message = message,
                    MessageType = messageType
                };

                var request = new MessageRequest(MessageCmdType.Unknown, messageData);
                
                // 获取所有用户会话
                var sessions = _sessionService.GetAllUserSessions();
                
                // 向所有会话发送消息并等待响应
                int successCount = 0;
                foreach (var session in sessions)
                {
                    if (session is SessionInfo sessionInfo)
                    {
                        var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                            session.SessionID, 
                            MessageCommands.BroadcastMessage, 
                            request, 
                            timeoutMs, 
                            ct);
                        
                        if (responsePacket?.Response is MessageResponse response && response.IsSuccess)
                        {
                            successCount++;
                        }
                    }
                }
                
                return MessageResponse.Success(MessageCmdType.Unknown, new { SendCount = successCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "广播消息时发生异常");
                return MessageResponse.Fail(MessageCmdType.Message, $"广播消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送系统通知并等待响应
        /// </summary>
        /// <param name="message">通知内容</param>
        /// <param name="notificationType">通知类型</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendSystemNotificationAsync(
            string message,
            string notificationType = "Info",
            int timeoutMs = 30000,
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    Message = message,
                    NotificationType = notificationType
                };

                var request = new MessageRequest(MessageCmdType.Unknown, messageData);
                
                // 获取所有用户会话
                var sessions = _sessionService.GetAllUserSessions();
                
                // 向所有会话发送消息并等待响应
                int successCount = 0;
                foreach (var session in sessions)
                {
                    if (session is SessionInfo sessionInfo)
                    {
                        var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                            session.SessionID, 
                            MessageCommands.SendSystemNotification, 
                            request, 
                            timeoutMs, 
                            ct);
                        
                        if (responsePacket?.Response is MessageResponse response && response.IsSuccess)
                        {
                            successCount++;
                        }
                    }
                }
                
                return MessageResponse.Success(MessageCmdType.Unknown, new { SendCount = successCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送系统通知时发生异常");
                return MessageResponse.Fail(MessageCmdType.Message,  $"发送通知失败: {ex.Message}");
            }
        }
    }
}