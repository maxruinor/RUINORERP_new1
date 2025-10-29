using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Requests.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Global;
using System.Threading;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 增强的服务器消息服务
    /// </summary>
    public class EnhancedServerMessageService : ServerMessageService
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<EnhancedServerMessageService> _logger;

        public EnhancedServerMessageService(
            SessionService sessionService,
            ILogger<EnhancedServerMessageService> logger = null) 
            : base(sessionService, logger as ILogger<ServerMessageService>)
        {
            _sessionService = sessionService;
            _logger = logger;
        }

        /// <summary>
        /// 发送业务消息给指定用户
        /// </summary>
        public async Task<bool> SendBusinessMessageAsync(
            string targetUserId,
            BizType bizType,
            long bizId,
            string title,
            string content,
            CancellationToken ct = default)
        {
            try
            {
                var reminderData = new ReminderData
                {
                    BizType = bizType,
                    BizPrimaryKey = bizId,
                    RemindSubject = title,
                    ReminderContent = content,
                    SenderEmployeeName = "系统",
                    messageCmd = MessageCmdType.Business,
                    CreateTime = DateTime.Now,
                    IsRead = false
                };

                var messageData = new
                {
                    BizType = bizType.ToString(),
                    BizPrimaryKey = bizId,
                    Title = title,
                    Content = content,
                    SenderEmployeeName = "系统",
                    MessageCmd = MessageCmdType.Business.ToString()
                };

                var request = new MessageRequest(MessageCmdType.Business, messageData);

                // 获取目标用户的所有会话
                var sessions = _sessionService.GetUserSessions(targetUserId);
                if (!sessions.Any())
                {
                    _logger?.LogWarning("发送业务消息失败：目标用户不在线 - 用户ID: {TargetUserId}", targetUserId);
                    return false;
                }

                // 向目标用户的所有会话发送消息
                var sendCount = 0;
                foreach (var session in sessions)
                {
                    try
                    {
                        var success = await _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.SendMessageToUser, 
                            request, 
                            ct);

                        if (success)
                            sendCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "向会话发送业务消息失败 - 会话ID: {SessionId}", session.SessionID);
                    }
                }

                _logger?.LogDebug("业务消息发送完成 - 目标用户: {TargetUserId}, 成功发送: {SendCount} 个会话",
                    targetUserId, sendCount);

                return sendCount > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送业务消息时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 发送工作流提醒消息
        /// </summary>
        public async Task<bool> SendWorkflowReminderAsync(
            string targetUserId,
            BizType bizType,
            long bizId,
            string workflowId,
            string title,
            string content,
            CancellationToken ct = default)
        {
            try
            {
                var reminderData = new ReminderData
                {
                    BizType = bizType,
                    BizPrimaryKey = bizId,
                    WorkflowId = workflowId,
                    RemindSubject = title,
                    ReminderContent = content,
                    SenderEmployeeName = "工作流系统",
                    messageCmd = MessageCmdType.Task,
                    CreateTime = DateTime.Now,
                    IsRead = false
                };

                var messageData = new
                {
                    BizType = bizType.ToString(),
                    BizPrimaryKey = bizId,
                    WorkflowId = workflowId,
                    Title = title,
                    Content = content,
                    SenderEmployeeName = "工作流系统",
                    MessageCmd = MessageCmdType.Task.ToString()
                };

                var request = new MessageRequest(MessageCmdType.Task, messageData);

                // 获取目标用户的所有会话
                var sessions = _sessionService.GetUserSessions(targetUserId);
                if (!sessions.Any())
                {
                    _logger?.LogWarning("发送工作流提醒失败：目标用户不在线 - 用户ID: {TargetUserId}", targetUserId);
                    return false;
                }

                // 向目标用户的所有会话发送消息
                var sendCount = 0;
                foreach (var session in sessions)
                {
                    try
                    {
                        var success = await _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.SendMessageToUser, 
                            request, 
                            ct);

                        if (success)
                            sendCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "向会话发送工作流提醒失败 - 会话ID: {SessionId}", session.SessionID);
                    }
                }

                _logger?.LogDebug("工作流提醒发送完成 - 目标用户: {TargetUserId}, 成功发送: {SendCount} 个会话",
                    targetUserId, sendCount);

                return sendCount > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送工作流提醒时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 批量发送消息给多个用户
        /// </summary>
        public async Task<Dictionary<string, bool>> SendMessagesToMultipleUsersAsync(
            List<string> targetUserIds,
            string title,
            string content,
            MessageCmdType messageType = MessageCmdType.Message,
            CancellationToken ct = default)
        {
            var results = new Dictionary<string, bool>();

            foreach (var userId in targetUserIds)
            {
                try
                {
                    var messageData = new
                    {
                        Title = title,
                        Content = content,
                        SenderEmployeeName = "系统",
                        MessageCmd = messageType.ToString()
                    };

                    var request = new MessageRequest(messageType, messageData);

                    // 获取目标用户的所有会话
                    var sessions = _sessionService.GetUserSessions(userId);
                    if (!sessions.Any())
                    {
                        _logger?.LogWarning("发送消息失败：目标用户不在线 - 用户ID: {TargetUserId}", userId);
                        results[userId] = false;
                        continue;
                    }

                    // 向目标用户的所有会话发送消息
                    var sendCount = 0;
                    foreach (var session in sessions)
                    {
                        try
                        {
                            var success = await _sessionService.SendCommandAsync(
                                session.SessionID, 
                                MessageCommands.SendMessageToUser, 
                                request, 
                                ct);

                            if (success)
                                sendCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "向会话发送消息失败 - 会话ID: {SessionId}", session.SessionID);
                        }
                    }

                    results[userId] = sendCount > 0;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "发送消息给用户 {UserId} 时发生异常", userId);
                    results[userId] = false;
                }
            }

            return results;
        }
    }
}