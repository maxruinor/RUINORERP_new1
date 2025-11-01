using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses.Message;
using RUINORERP.UI.Network.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

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
        public MessageCommandHandler(MessageService messageService, ILogger<MessageCommandHandler> logger = null)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _logger = logger;

            // 设置支持的命令
            SetSupportedCommands(
                 MessageCommands.SendPopupMessage,
                 MessageCommands.SendMessageToUser,
                 MessageCommands.SendMessageToDepartment,
                 MessageCommands.BroadcastMessage,
                 MessageCommands.SendSystemNotification
            );
        }

        /// <summary>
        /// 提取消息数据，处理不同类型的数据结构
        /// </summary>
        private object ExtractMessageData(object messageData)
        {
            try
            {
                if (messageData is MessageResponse response)
                {
                    return response.Data;
                }
                return messageData;
            }
            catch (Exception ex)
            {
                LogError("提取消息数据时发生异常", ex);
                return null;
            }
        }

        /// <summary>
        /// 将数据转换为字典格式
        /// </summary>
        private Dictionary<string, object> ConvertDataToDictionary(object data)
        {
            try
            {
                if (data == null)
                    return null;

                // 简单实现，如果有更复杂的需求可以使用JSON序列化/反序列化
                if (data is Dictionary<string, object> dict)
                    return dict;

                // 如果是其他类型，可以根据需要添加更多的转换逻辑
                return new Dictionary<string, object> { { "Data", data } };
            }
            catch (Exception ex)
            {
                LogError("转换数据为字典时发生异常", ex);
                return null;
            }
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
                LogInfo("消息命令处理器初始化成功");
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
                LogError("收到无效的数据包");
                return;
            }

            LogInfo($"收到消息命令: {(ushort)packet.CommandId}");

            // 根据命令ID处理不同的消息命令
            try
            {
                // 将消息数据转换为适当的类型
                object messageData;
                try
                {
                    // 修复PacketModel属性访问错误，使用CommandId而不是Data
                    // 尝试获取消息数据，如果是MessageResponse直接使用，否则尝试转换
                    messageData = packet.CommandId != null ? packet : ConvertDataToDictionary(packet);
                    if (messageData == null)
                    {
                        LogWarning("消息数据格式无效或转换失败");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LogError("转换消息数据时发生异常", ex);
                    return;
                }

                // 根据命令ID分发到对应的事件处理
                // 使用if else结构替换switch case以避免ushort类型常量错误
                ushort commandId = (ushort)packet.CommandId.OperationCode;
                
                if (commandId == (ushort)MessageCommands.SendPopupMessage)
                {
                    await HandlePopupMessageAsync(packet, messageData);
                }
                else if (commandId == (ushort)MessageCommands.SendMessageToUser)
                {
                    await HandleUserMessageAsync(packet, messageData);
                }
                else if (commandId == (ushort)MessageCommands.SendMessageToDepartment)
                {
                    await HandleDepartmentMessageAsync(packet, messageData);
                }
                else if (commandId == (ushort)MessageCommands.BroadcastMessage)
                {
                    await HandleBroadcastMessageAsync(packet, messageData);
                }
                else if (commandId == (ushort)MessageCommands.SendSystemNotification)
                {
                    await HandleSystemNotificationAsync(packet, messageData);
                }
                else
                {
                    LogWarning($"未处理的消息命令ID: {packet.CommandId.FullCode}");
                }
            }
            catch (Exception ex)
            {
                LogError("处理消息命令时发生异常", ex);
            }
        }

        /// <summary>
        /// 处理弹窗消息命令
        /// </summary>
        private async Task HandlePopupMessageAsync(PacketModel packet, object messageData)
        {
            try
            {
                var args = new MessageReceivedEventArgs
                {
                    MessageType = "Popup",
                    Data = ExtractMessageData(messageData),
                    Timestamp = DateTime.Now
                };

                // 触发MessageService中的事件
                _messageService.OnPopupMessageReceived(args);
                LogInfo("弹窗消息已处理");
            }
            catch (Exception ex)
            {
                LogError("处理弹窗消息命令时发生异常", ex);
            }
        }

        /// <summary>
        /// 处理用户消息命令
        /// </summary>
        private async Task HandleUserMessageAsync(PacketModel packet, object messageData)
        {
            try
            {
                var args = new MessageReceivedEventArgs
                {
                    MessageType = "User",
                    Data = ExtractMessageData(messageData),
                    Timestamp = DateTime.Now
                };

                // 触发MessageService中的事件
                _messageService.OnUserMessageReceived(args);
                LogInfo("用户消息已处理");
            }
            catch (Exception ex)
            {
                LogError("处理用户消息命令时发生异常", ex);
            }
        }

        /// <summary>
        /// 处理部门消息命令
        /// </summary>
        private async Task HandleDepartmentMessageAsync(PacketModel packet, object messageData)
        {
            try
            {
                var args = new MessageReceivedEventArgs
                {
                    MessageType = "Department",
                    Data = ExtractMessageData(messageData),
                    Timestamp = DateTime.Now
                };

                // 触发MessageService中的事件
                _messageService.OnDepartmentMessageReceived(args);
                LogInfo("部门消息已处理");
            }
            catch (Exception ex)
            {
                LogError("处理部门消息命令时发生异常", ex);
            }
        }

        /// <summary>
        /// 处理广播消息命令
        /// </summary>
        private async Task HandleBroadcastMessageAsync(PacketModel packet, object messageData)
        {
            try
            {
                var args = new MessageReceivedEventArgs
                {
                    MessageType = "Broadcast",
                    Data = ExtractMessageData(messageData),
                    Timestamp = DateTime.Now
                };

                // 触发MessageService中的事件
                _messageService.OnBroadcastMessageReceived(args);
                LogInfo("广播消息已处理");
            }
            catch (Exception ex)
            {
                LogError("处理广播消息命令时发生异常", ex);
            }
        }

        /// <summary>
        /// 处理系统通知命令
        /// </summary>
        private async Task HandleSystemNotificationAsync(PacketModel packet, object messageData)
        {
            try
            {
                var args = new MessageReceivedEventArgs
                {
                    MessageType = "SystemNotification",
                    Data = ExtractMessageData(messageData),
                    Timestamp = DateTime.Now
                };

                // 触发MessageService中的事件
                _messageService.OnSystemNotificationReceived(args);
                LogInfo("系统通知已处理");
            }
            catch (Exception ex)
            {
                LogError("处理系统通知命令时发生异常", ex);
            }
        }

        /// <summary>
        /// 处理消息的通用方法
        /// </summary>
        /// <param name="message">消息数据包</param>
        public async Task HandleMessageAsync(PacketModel message)
        {
            if (message == null || message.CommandId.Name != "Message")
                return;

            // 直接使用HandleAsync方法处理消息
            await HandleAsync(message);
        }
    }
}