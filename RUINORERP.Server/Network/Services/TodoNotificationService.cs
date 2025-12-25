using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 任务状态通知服务 - 服务器广播中心
    /// 单一职责：负责将任务状态变更事件广播给所有连接的客户端
    /// 作为服务器端的消息分发枢纽，确保状态更新能实时传递给所有相关用户
    /// </summary>
    public class TodoNotificationService
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<TodoNotificationService> _logger;

        public TodoNotificationService(
            SessionService sessionService,
            ILogger<TodoNotificationService> logger = null)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _logger = logger;
        }

        /// <summary>
        /// 向相关用户推送任务状态变更通知
        /// </summary>
        /// <param name="update">任务状态更新信息</param>
        public async Task NotifyTodoChangeAsync(TodoUpdate update, MessageData messageData)
        {
            try
            {
                if (update == null)
                {
                    _logger?.LogWarning("任务状态更新信息为空，无法发送通知");
                    return;
                }

                // 构造推送消息
                messageData.SendTime = System.DateTime.UtcNow;

                var request = new MessageRequest(MessageType.Business, messageData);

                // 获取所有在线用户会话（实际应用中应该只推送给相关的用户）
                var sessions = _sessionService.GetAllUserSessions();

                foreach (var session in sessions)
                {
                    if (session is SessionInfo sessionInfo)
                    {
                        if (sessionInfo.UserId == messageData.SenderId && messageData.SendToSelf == false)
                        {
                            continue; // 不向发送者自己推送通知
                        }

                        await _sessionService.SendPacketCoreAsync(sessionInfo, MessageCommands.SendTodoNotification, request, 5000);
                        
                    }
                }

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "推送任务状态变更通知时发生异常");
            }
        }
    }
}