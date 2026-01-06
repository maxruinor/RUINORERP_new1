using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.Server.Network.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 服务器消息服务类 - 提供服务器向客户端发送消息并等待响应的功能
    /// 支持向指定用户、部门发送消息，以及广播消息等功能
    /// 支持双向通信，既可以发送消息也可以等待客户端响应
    /// </summary>
    public class SystemManagementService
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<SystemManagementService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sessionService">会话服务</param>
        /// <param name="logger">日志记录器</param>
        public SystemManagementService(
            SessionService sessionService,
            ILogger<SystemManagementService> logger = null)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _logger = logger;
        }

        /// <summary>
        /// 发送弹窗消息给指定用户并等待响应
        /// </summary>
        /// <param name="targetUserName">目标用户ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="title">消息标题</param>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendPopupMessageAsync(
            string targetUserName,
            string message,
            string title = "系统消息",
            int timeoutMs = 30000,
            CancellationToken ct = default)
        {
            try
            {
                // 使用MessageData类代替匿名对象，提高类型安全性和可维护性
                var messageData = new MessageData
                {
                    MessageId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId(),
                    MessageType = MessageType.Popup,
                    Title = title,
                    Content = message,
                    SenderName = "服务器消息",
                    ReceiverIds = new List<long> { long.TryParse(targetUserName, out long userId) ? userId : 0 },
                    SendTime = DateTime.Now
                };

                var request = new MessageRequest(MessageType.Popup, messageData);

                // 获取目标用户的所有会话
                var sessions = _sessionService.GetUserSessions(targetUserName);

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
                               MessageResponse.Fail(MessageType.Popup, "未收到有效响应");
                    }
                }

                return MessageResponse.Fail(MessageType.Popup, "目标用户不在线");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送弹窗消息时发生异常");
                return MessageResponse.Fail(MessageType.Popup, $"发送消息失败: {ex.Message}");
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
            MessageType messageType = MessageType.Popup,
            int timeoutMs = 30000,
            CancellationToken ct = default)
        {
            try
            {
                // 使用MessageData类代替匿名对象，提高类型安全性和可维护性
                var messageData = new MessageData
                {
                    MessageType = messageType,
                    Content = message,
                    ReceiverIds = new List<long> { long.TryParse(targetUserId, out long userId) ? userId : 0 },
                    SendTime = DateTime.Now
                };

                var request = new MessageRequest(messageData.MessageType, messageData);

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
                               MessageResponse.Fail(messageType, "未收到有效响应");
                    }
                }

                return MessageResponse.Fail(messageType, "目标用户不在线");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送用户消息时发生异常");
                return MessageResponse.Fail(messageType, $"发送消息失败: {ex.Message}");
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
        public async Task<bool> ForceLogoutAsync(
            SessionInfo session,
            int timeoutMs = 30000,
            CancellationToken ct = default)
        {
            try
            {
                var systemCommandRequest = new SystemCommandRequest();
                systemCommandRequest.CommandType = SystemManagementType.ExitERPSystem;
                systemCommandRequest.DelaySeconds = 5;
                // 发送命令（不等待响应）
                await _sessionService.SendPacketCoreAsync(session, SystemCommands.SystemManagement, systemCommandRequest, 1000, ct);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送用户消息时发生异常");
                return false;
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
                // 使用MessageData类代替匿名对象，提高类型安全性和可维护性
                var messageData = new MessageData
                {
                    MessageType = messageType == "Text" ? MessageType.Business : MessageType.Business,
                    Content = message,
                    SendTime = DateTime.Now
                };

                // 使用扩展数据存储部门ID
                messageData.ExtendedData["DepartmentId"] = departmentId;

                var request = new MessageRequest(messageData.MessageType, messageData);

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

                return MessageResponse.Success(MessageType.Business, new { SendCount = successCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送部门消息时发生异常");
                return MessageResponse.Fail(MessageType.Business, $"发送消息失败: {ex.Message}");
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
                // 使用MessageData类代替匿名对象，提高类型安全性和可维护性
                var messageData = new MessageData
                {
                    MessageType = messageType == "Text" ? MessageType.Business : MessageType.Business,
                    Content = message,
                    SendTime = DateTime.Now
                };

                var request = new MessageRequest(messageData.MessageType, messageData);

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

                return MessageResponse.Success(MessageType.Business, new { SendCount = successCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "广播消息时发生异常");
                return MessageResponse.Fail(MessageType.Business, $"广播消息失败: {ex.Message}");
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
                // 使用MessageData类代替匿名对象，提高类型安全性和可维护性
                var messageData = new MessageData
                {
                    MessageType = MessageType.System,
                    Content = message,
                    SendTime = DateTime.Now
                };

                // 使用扩展数据存储通知类型
                messageData.ExtendedData["NotificationType"] = notificationType;

                var request = new MessageRequest(MessageType.System, messageData);

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

                return MessageResponse.Success(MessageType.System, new { SendCount = successCount });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送系统通知时发生异常");
                return MessageResponse.Fail(MessageType.System, $"发送通知失败: {ex.Message}");
            }
        }
    }
}