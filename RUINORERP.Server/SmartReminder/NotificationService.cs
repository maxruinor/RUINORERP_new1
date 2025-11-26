using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.Network.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Server.Network.CommandHandlers;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Messaging;

namespace RUINORERP.Server.SmartReminder
{
    /// <summary>
    /// 通知服务 - 使用新的消息系统发送智能提醒
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly ApplicationContext _appContext;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ISessionService _sessionService;
        private readonly MessageCommandHandler _messageCommandHandler;

        public NotificationService(ILogger<NotificationService> logger,
            ApplicationContext appContext,
            IUnitOfWorkManage unitOfWorkManage,
            ISessionService sessionService = null,
            MessageCommandHandler messageCommandHandler = null)
        {
            _logger = logger;
            _appContext = appContext;
            _unitOfWorkManage = unitOfWorkManage;
            _sessionService = sessionService ?? Startup.GetFromFac<ISessionService>();
            _messageCommandHandler = messageCommandHandler ?? Startup.GetFromFac<MessageCommandHandler>();
        }

        public async Task SendNotificationAsync(IReminderRule rule, string message, object contextData)
        {
            try
            {
                // 获取接收人员集合
                var recipients = rule.NotifyRecipients;
                if (recipients == null || recipients.Count == 0)
                    return;

                _logger.Debug("准备发送智能提醒：{Message} 给 {Count} 个用户", message, recipients.Count);

                // 构建消息数据
                var messageData = new Dictionary<string, object>
                {
                    { "Message", message },
                    { "RuleId", rule.RuleId },
                    { "Timestamp", DateTime.Now },
                    { "ContextData", contextData },
                    { "NotificationType", "Reminder" },
                    { "TargetUserIds", recipients.Select(id => id.ToString()).ToList() }
                };

                // 根据通知类型选择合适的消息命令
                uint commandType = MessageCommands.SendSystemNotification.OperationCode;
                
                // 如果是特定用户的提醒，使用用户消息
                if (recipients.Count <= 10) // 少于10个用户使用定向消息
                {
                    foreach (var userId in recipients)
                    {
                        var userMessageData = new Dictionary<string, object>(messageData)
                        {
                            { "TargetUserId", userId.ToString() }
                        };
                        
                        await SendUserMessageAsync(userMessageData, userId.ToString());
                    }
                }
                else // 大量用户使用系统通知
                {
                    await SendSystemNotificationAsync(messageData);
                }

                //_logger.Debug("智能提醒发送完成");
            }
            catch (Exception ex)
            {   
                _logger.LogError(ex, "发送智能提醒失败：{Message}", message);
            }
        }

        /// <summary>
        /// 发送用户消息
        /// </summary>
        private async Task SendUserMessageAsync(Dictionary<string, object> messageData, string targetUserId)
        {
            try
            {
                // 使用消息系统发送给特定用户
                var messageRequest = new MessageRequest
                {
                    CommandType =MessageType.Message ,
                    Data = messageData
                };

                // 通过SessionService发送给指定用户
                var targetSessions = _sessionService.GetUserSessions(targetUserId);
                foreach (var session in targetSessions)
                {
                    _logger.LogDebug("向用户 {UserId} 的会话 {SessionId} 发送智能提醒", targetUserId, session.SessionID);
                    // 通过现有的会话服务发送消息 - 使用新的发送方法
                    var success = await _sessionService.SendCommandAsync(
                        session.SessionID, 
                        MessageCommands.SendMessageToUser, 
                        messageRequest);
                    
                    if (!success)
                    {
                        _logger.LogWarning("向用户 {UserId} 发送智能提醒失败", targetUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送用户消息失败 - 用户ID: {UserId}", targetUserId);
            }
        }

        /// <summary>
        /// 发送系统通知
        /// </summary>
        private async Task SendSystemNotificationAsync(Dictionary<string, object> messageData)
        {
            try
            {
                var messageRequest = new MessageRequest
                {
                    CommandType = MessageType.Notice,
                    Data = messageData
                };

                // 广播给所有用户
                _logger.Debug("发送系统通知广播");
                //_sessionService.BroadcastCommand(
                //    "MessageCommandHandler",
                //    System.Text.Json.JsonSerializer.Serialize(messageRequest)
                //);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送系统通知失败");
            }
        }

        // 通知类型枚举 - 用于在消息系统中标识通知类型
        public enum ReminderNotificationType
        {
            /// <summary>
            /// 普通提醒
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 警告提醒
            /// </summary>
            Warning = 1,
            /// <summary>
            /// 紧急提醒
            /// </summary>
            Urgent = 2,
            /// <summary>
            /// 库存预警
            /// </summary>
            InventoryAlert = 3,
            /// <summary>
            /// 任务提醒
            /// </summary>
            TaskReminder = 4
        }
        
        /// <summary>
        /// 获取通知枚举值对应的消息系统通知类型
        /// </summary>
        private string GetNotificationTypeString(ReminderNotificationType type)
        {
            return type switch
            {
                ReminderNotificationType.Normal => "Reminder_Normal",
                ReminderNotificationType.Warning => "Reminder_Warning",
                ReminderNotificationType.Urgent => "Reminder_Urgent",
                ReminderNotificationType.InventoryAlert => "Reminder_Inventory",
                ReminderNotificationType.TaskReminder => "Reminder_Task",
                _ => "Reminder_Normal"
            };
        }
    }
}
