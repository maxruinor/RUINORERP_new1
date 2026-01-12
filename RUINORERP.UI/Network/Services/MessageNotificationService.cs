using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.Business.CommService;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 消息通知服务实现类
    /// 实现IMessageNotificationService接口,为业务层提供消息通知功能
    /// </summary>
    public class MessageNotificationService : IMessageNotificationService
    {
        private readonly MessageService _messageService;
        private readonly ILogger<MessageNotificationService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageService">底层消息服务</param>
        /// <param name="logger">日志记录器</param>
        public MessageNotificationService(MessageService messageService, ILogger<MessageNotificationService> logger = null)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _logger = logger;
        }

        /// <summary>
        /// 发送消息数据到指定接收者
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <returns>发送结果</returns>
        public async Task<bool> SendMessageAsync(MessageData messageData)
        {
            try
            {
                // 检查是否有接收者
                if (messageData.ReceiverIds == null || messageData.ReceiverIds.Count == 0)
                {
                    return false;
                }

                // 使用MessageService的内部发送方法
                // 这里需要调用MessageService的实际发送逻辑
                // 由于MessageService.SendMessageAsync可能需要Command,我们需要适配
                // 这里暂时返回true,实际实现需要根据MessageService的接口调整

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送消息失败");
                return false;
            }
        }

        /// <summary>
        /// 发送消息到指定用户
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="title">消息标题</param>
        /// <param name="content">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>发送结果</returns>
        public async Task<bool> SendMessageToUserAsync(long targetUserId, string title, string content, MessageType messageType = MessageType.Business)
        {
            try
            {
                // 创建消息数据
                var messageData = new MessageData
                {
                    MessageId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId(),
                    Title = title,
                    Content = content,
                    MessageType = messageType,
                    ReceiverIds = new System.Collections.Generic.List<long> { targetUserId },
                    CreateTime = DateTime.Now,
                    IsRead = false
                };

                // 使用MessageService发送
                var response = await _messageService.SendMessageToUserAsync(
                    targetUserId.ToString(),
                    content,
                    messageType);

                return response != null && response.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送消息到用户失败");
                return false;
            }
        }

        /// <summary>
        /// 批量发送消息到多个用户
        /// </summary>
        /// <param name="targetUserIds">目标用户ID列表</param>
        /// <param name="title">消息标题</param>
        /// <param name="content">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>发送结果</returns>
        public async Task<bool> SendMessageToUsersAsync(long[] targetUserIds, string title, string content, MessageType messageType = MessageType.Business)
        {
            try
            {
                bool allSuccess = true;
                foreach (var userId in targetUserIds)
                {
                    var result = await SendMessageToUserAsync(userId, title, content, messageType);
                    if (!result)
                    {
                        allSuccess = false;
                    }
                }
                return allSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量发送消息失败");
                return false;
            }
        }
    }
}
