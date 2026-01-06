
using RUINORERP.PacketSpec.Commands;
using RUINORERP.UI.Network.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 消息命令处理器
    /// 负责处理与消息相关的命令，如系统消息、通知等
    /// </summary>
    [ClientCommandHandler("MessageCommandHandler", 50)]
    public class MessageCommandHandler : BaseClientCommandHandler
    {
        private readonly ILogger<MessageCommandHandler> _logger;
        private readonly MessageService _messageService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageService">消息服务</param>
        /// <param name="logger">日志记录器</param>
        public MessageCommandHandler(MessageService messageService, ILogger<MessageCommandHandler> logger = null) :
            base(logger ?? Startup.GetFromFac<ILogger<BaseClientCommandHandler>>())
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _logger = logger ?? Startup.GetFromFac<ILogger<MessageCommandHandler>>();

            // 保留通过SetSupportedCommands方法设置命令的方式，使用枚举值而非硬编码字符串
            SetSupportedCommands(
                MessageCommands.SendPopupMessage,
                MessageCommands.SendMessageToUser,
                MessageCommands.SendMessageToDepartment,
                MessageCommands.BroadcastMessage,
                MessageCommands.SendSystemNotification
            );
        }


        /// <summary>
        /// 初始化处理器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public override async Task<bool> InitializeAsync()
        {
            bool initialized = await base.InitializeAsync();
            if (initialized)
            {
                _logger.LogDebug("消息命令处理器初始化成功");
            }
            return initialized;
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        public override async Task HandleAsync(PacketModel packet)
        {
            if (packet == null || packet.CommandId == null)
            {
                _logger.LogError("收到无效的数据包");
                return;
            }

            _logger.LogDebug($"收到消息命令: {(ushort)packet.CommandId}");

            try
            {
                // 提取MessageData对象
                var messageData = ExtractMessageData(packet);
                if (messageData == null)
                {
                    _logger.LogWarning("无法解析消息数据");
                    return;
                }

                // 根据命令ID分发到对应的事件处理
                if (packet.CommandId == MessageCommands.SendPopupMessage)
                {
                    await HandlePopupMessageAsync(messageData);
                }
                else if (packet.CommandId == MessageCommands.SendMessageToUser)
                {
                    await HandleUserMessageAsync(messageData);
                }
                else if (packet.CommandId == MessageCommands.SendMessageToDepartment)
                {
                    await HandleDepartmentMessageAsync(messageData);
                }
                else if (packet.CommandId == MessageCommands.BroadcastMessage)
                {
                    await HandleBroadcastMessageAsync(messageData);
                }
                else if (packet.CommandId == MessageCommands.SendSystemNotification)
                {
                    await HandleSystemNotificationAsync(messageData);
                }
                else
                {
                    _logger.LogWarning($"未处理的消息命令ID: {packet.CommandId.FullCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理消息命令时发生异常");
            }
        }

        /// <summary>
        /// 处理弹窗消息命令
        /// </summary>
        private async Task HandlePopupMessageAsync(MessageData messageData)
        {
            try
            {
                // 触发MessageService中的事件
                _messageService.OnPopupMessageReceived(messageData);
                _logger.LogDebug("弹窗消息已处理");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理弹窗消息命令时发生异常");
            }
        }

        /// <summary>
        /// 处理用户消息命令
        /// </summary>
        private async Task HandleUserMessageAsync(MessageData messageData)
        {
            try
            {
                // 触发MessageService中的事件
                _messageService.OnBusinessMessageReceived(messageData);
                _logger.LogDebug("用户消息已处理");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理用户消息命令时发生异常");
            }
        }

        /// <summary>
        /// 处理部门消息命令
        /// </summary>
        private async Task HandleDepartmentMessageAsync(MessageData messageData)
        {
            try
            {
                // 根据消息类型触发相应的事件
                if (messageData.MessageType == MessageType.Popup)
                {
                    _messageService.OnPopupMessageReceived(messageData);
                }
                else if (messageData.MessageType == MessageType.System)
                {
                    _messageService.OnSystemNotificationReceived(messageData);
                }
                else
                {
                    _messageService.OnBusinessMessageReceived(messageData);
                }
                _logger.LogDebug("部门消息已处理");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理部门消息命令时发生异常");
            }
        }

        /// <summary>
        /// 处理广播消息命令
        /// </summary>
        private async Task HandleBroadcastMessageAsync(MessageData messageData)
        {
            try
            {
                // 根据消息类型触发相应的事件
                if (messageData.MessageType == MessageType.Popup)
                {
                    _messageService.OnPopupMessageReceived(messageData);
                }
                else if (messageData.MessageType == MessageType.System)
                {
                    _messageService.OnSystemNotificationReceived(messageData);
                }
                else
                {
                    _messageService.OnBusinessMessageReceived(messageData);
                }
                _logger.LogDebug("广播消息已处理");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理广播消息命令时发生异常");
            }
        }

        /// <summary>
        /// 处理系统通知命令
        /// </summary>
        private async Task HandleSystemNotificationAsync(MessageData messageData)
        {
            try
            {
                // 确保系统通知使用正确的消息类型
                messageData.MessageType = MessageType.System;
                // 触发系统通知事件
                _messageService.OnSystemNotificationReceived(messageData);
                _logger.LogDebug("系统通知已处理");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理系统通知命令时发生异常");
            }
        }

        /// <summary>
        /// 处理消息的通用方法
        /// </summary>
        /// <param name="message">消息数据包</param>
        /// <summary>
        /// 从数据包中提取消息数据
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>提取的消息数据对象</returns>
        private MessageData ExtractMessageData(PacketModel packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet), "数据包不能为空");

            // 创建一个新的MessageData对象
            var messageData = new MessageData();

            // 尝试从Request中获取消息数据
            if (packet.Request != null && packet.Request is MessageRequest messageRequest)
            {
                // 优先从Data中提取MessageType
                if (messageRequest.Data != null)
                {
                    if (messageRequest.Data is MessageData data)
                    {
                        messageData = data;
                    }
                    // 处理其他类型的数据
                    else
                    {
                        _logger.LogWarning("请完成：尝试提取消息数据 没有完成");
                    }
                }
            }

            return messageData;
        }

        public async Task HandleMessageAsync(PacketModel message)
        {
            if (message == null || message.CommandId.Name != "Message")
                return;

            // 直接使用HandleAsync方法处理消息
            await HandleAsync(message);
        }
    }
}