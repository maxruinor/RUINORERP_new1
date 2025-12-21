using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.UI.UserCenter.DataParts;
using System;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 客户端任务状态通知服务
    /// 负责向服务器发送任务状态变更通知
    /// </summary>
    public class ClientTaskStatusNotificationService
    {
        private readonly ILogger<ClientTaskStatusNotificationService> _logger;
        private readonly MessageService _messageService;

        public ClientTaskStatusNotificationService(
            ILogger<ClientTaskStatusNotificationService> logger,
            MessageService messageService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }

        /// <summary>
        /// 发送任务状态变更通知到服务器
        /// </summary>
        /// <param name="update">任务状态更新信息</param>
        public async Task NotifyTaskStatusChangeAsync(NetworkTaskStatusUpdate update)
        {
            try
            {
                if (update == null)
                {
                    _logger.LogWarning("任务状态更新信息为空，无法发送通知");
                    return;
                }

                // 构造推送消息
                var messageData = new MessageData
                {
                    Id = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId(),
                    MessageType = MessageType.Business,
                    Title = "任务状态变更",
                    Content = $"[{update.BusinessType}]{update.OperationDescription}",
                    Data = update,
                    SendTime = DateTime.Now
                };

                var request = new MessageRequest(MessageType.Business, messageData);

                // 通过消息服务发送到服务器
                await _messageService.SendCommandAsync(
                    MessageCommands.SendTaskStatusNotification,
                    request);

                _logger.LogDebug($"已发送任务状态变更通知: {update.BusinessType} - {update.TaskId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送任务状态变更通知时发生异常");
            }
        }
    }
}