using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.UI.Network.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Models.Core;

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
                _logger.LogInformation("消息命令处理器初始化成功");
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

            _logger.LogInformation($"收到消息命令: {(ushort)packet.CommandId}");

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
                _logger.LogInformation("弹窗消息已处理");
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
                _logger.LogInformation("用户消息已处理");
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
                // 触发MessageService中的事件
                _messageService.OnDepartmentMessageReceived(messageData);
                _logger.LogInformation("部门消息已处理");
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
                // 触发MessageService中的事件
                _messageService.OnBroadcastMessageReceived(messageData);
                _logger.LogInformation("广播消息已处理");
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
                _logger.LogInformation("系统通知已处理");
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
                    // 如果Data是字典类型，尝试从中提取数据
                    if (messageRequest.Data is Dictionary<string, object> dataDict)
                    {                    
                        messageData = MessageData.FromDictionary(dataDict);
                    }
                    // 如果Data是MessageData类型，直接使用
                    else if (messageRequest.Data is MessageData data)
                    {                    
                        messageData = data;
                    }
                    // 处理其他类型的数据
                    else
                    {                        
                        // 尝试将Data转换为字典并提取
                        try
                        {
                            var dict = new Dictionary<string, object>();
                            // 添加默认的MessageType如果未指定
                            dict["MessageType"] = MessageType.Text.ToString();
                            messageData = MessageData.FromDictionary(dict);
                        }
                        catch (Exception ex)
                        {                             
                            _logger.LogWarning(ex, "无法转换消息数据");
                        }
                    }
                }

                // 如果消息类型仍未设置，尝试从CommandType映射
                if (messageData.MessageType == MessageType.Unknown && messageRequest.CommandType != MessageType.Unknown)
                {                    
                    switch (messageRequest.CommandType)
                    {                    
                        case MessageType.Message:
                            messageData.MessageType = MessageType.Text;
                            break;
                        case MessageType.Prompt:
                            messageData.MessageType = MessageType.Prompt;
                            break;
                        case MessageType.Reminder:
                            messageData.MessageType = MessageType.Reminder;
                            break;
                        case MessageType.System:
                            messageData.MessageType = MessageType.System;
                            break;
                        default:
                            messageData.MessageType = MessageType.Unknown;
                            break;
                    }
                }
            }
            // 尝试从Response中获取消息数据
            else if (packet.Response != null && packet.Response is Dictionary<string, object> responseDict)
            {                
                messageData = MessageData.FromDictionary(responseDict);
            }
            // 尝试从Extensions中提取消息数据
            else if (packet.Extensions != null)
            {                
                // 创建一个字典用于FromDictionary方法
                var dict = new Dictionary<string, object>();
                foreach (var kvp in packet.Extensions)
                {                    
                    dict.Add(kvp.Key, kvp.Value);
                }
                messageData = MessageData.FromDictionary(dict);
            }

            // 如果消息类型仍未设置，使用默认值Text
            if (messageData.MessageType == MessageType.Unknown)
            {
                messageData.MessageType = MessageType.Text;
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