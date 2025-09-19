using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Handlers;
using MessagePack;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Enums.Message;
using RUINORERP.PacketSpec.Enums.Workflow;
using RUINORERP.PacketSpec.Enums.Exception;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RUINORERP.PacketSpec.Commands.Handlers
{
    /// <summary>
    /// 消息命令处理器(实例)
    /// </summary>
    [CommandHandler("MessageCommandHandler", 100, true)]
    public class MessageCommandHandler : BaseCommandHandler
    {
        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 100;

        /// <summary>
        /// 支持的命令列表
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands { get; } = new List<uint> {
            0x0300, // SendPopupMessage
            0x0304, // SendMessageToUser
            0x0305, // SendMessageToDepartment
            0x0306, // BroadcastMessage
            0x0307  // SendSystemNotification
        }.AsReadOnly();

        /// <summary>
        /// 执行核心处理逻辑
        /// </summary>
        protected override async Task<CommandResult> OnHandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;
            LogInfo($"处理消息命令: {commandId}");
            return commandId.FullCode switch
                {
                    // SendPopupMessage = 0x0300
                    0x0300 => await HandleSendPopupMessageAsync(command, cancellationToken),
                    // SendMessageToUser = 0x0304
                    0x0304 => await HandleSendMessageToUserAsync(command, cancellationToken),
                    // SendMessageToDepartment = 0x0305
                    0x0305 => await HandleSendMessageToDepartmentAsync(command, cancellationToken),
                    // BroadcastMessage = 0x0306
                    0x0306 => await HandleBroadcastMessageAsync(command, cancellationToken),
                    // SendSystemNotification = 0x0307
                    0x0307 => await HandleSendSystemNotificationAsync(command, cancellationToken),
                    _ => CommandResult.Failure($"不支持的消息命令类型: {commandId}", ErrorCodes.UnsupportedMessageCommand)
                };
            }
            catch (Exception ex)
            {
                LogError($"处理消息命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", ErrorCodes.MessageHandlerError, ex);
            }
        }

        /// <summary>
        /// 处理发送弹窗消息
        /// </summary>
        private async Task<CommandResult> HandleSendPopupMessageAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理发送弹窗消息 [会话: {command.CommandId}]");

            try
            {
                // 模拟处理逻辑
                await Task.Delay(20, cancellationToken);

                // 解析消息数据
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("弹窗消息数据格式错误", ErrorCodes.InvalidPopupData);
                }

                // 创建响应
                var response = CreateMessageResponse(0x0300, messageData, "弹窗消息发送成功");

                return CommandResult.Success(
                    data: response,
                    message: "弹窗消息发送成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"发送弹窗消息异常: {ex.Message}", ex);
                return CommandResult.Failure($"发送弹窗消息异常: {ex.Message}", ErrorCodes.PopupSendError, ex);
            }
        }

        /// <summary>
        /// 处理发送用户消息
        /// </summary>
        private async Task<CommandResult> HandleSendMessageToUserAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理发送用户消息 [会话: {command.CommandId}]");

            try
            {
                // 模拟处理逻辑
                await Task.Delay(20, cancellationToken);

                // 解析消息数据
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("用户消息数据格式错误", ErrorCodes.InvalidUserData);
                }

                // 创建响应
                var response = CreateMessageResponse(0x0304, messageData, "用户消息发送成功");

                return CommandResult.Success(
                    data: response,
                    message: "用户消息发送成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"发送用户消息异常: {ex.Message}", ex);
                return CommandResult.Failure($"发送用户消息异常: {ex.Message}", ErrorCodes.UserMessageSendError, ex);
            }
        }

        /// <summary>
        /// 处理发送部门消息
        /// </summary>
        private async Task<CommandResult> HandleSendMessageToDepartmentAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理发送部门消息 [会话: {command.CommandId}]");

            try
            {
                // 模拟处理逻辑
                await Task.Delay(20, cancellationToken);

                // 解析消息数据
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("部门消息数据格式错误", ErrorCodes.InvalidDepartmentData);
                }

                // 创建响应
                var response = CreateMessageResponse(0x0305, messageData, "部门消息发送成功");

                return CommandResult.Success(
                    data: response,
                    message: "部门消息发送成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"发送部门消息异常: {ex.Message}", ex);
                return CommandResult.Failure($"发送部门消息异常: {ex.Message}", ErrorCodes.DepartmentMessageSendError, ex);
            }
        }

        /// <summary>
        /// 处理广播消息
        /// </summary>
        private async Task<CommandResult> HandleBroadcastMessageAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理广播消息 [会话: {command.CommandId}]");

            try
            {
                // 模拟处理逻辑
                await Task.Delay(20, cancellationToken);

                // 解析消息数据
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("广播消息数据格式错误", ErrorCodes.InvalidBroadcastData);
                }

                // 创建响应
                var response = CreateMessageResponse(0x0306, messageData, "广播消息发送成功");

                return CommandResult.Success(
                    data: response,
                    message: "广播消息发送成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"发送广播消息异常: {ex.Message}", ex);
                return CommandResult.Failure($"发送广播消息异常: {ex.Message}", ErrorCodes.BroadcastSendError, ex);
            }
        }

        /// <summary>
        /// 处理系统通知
        /// </summary>
        private async Task<CommandResult> HandleSendSystemNotificationAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理系统通知 [会话: {command.CommandId}]");

            try
            {
                // 模拟处理逻辑
                await Task.Delay(20, cancellationToken);

                // 解析消息数据
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("系统通知数据格式错误", ErrorCodes.InvalidNotificationData);
                }

                // 创建响应
                var response = CreateMessageResponse(0x0307, messageData, "系统通知发送成功");

                return CommandResult.Success(
                    data: response,
                    message: "系统通知发送成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"发送系统通知异常: {ex.Message}", ex);
                return CommandResult.Failure($"发送系统通知异常: {ex.Message}", ErrorCodes.NotificationSendError, ex);
            }
        }

        /// <summary>
        /// 解析消息数据
        /// </summary>
        private MessageData ParseMessageData(OriginalData originalData)
        {
            try
            {
                // 使用MessagePack进行反序列化
                if (originalData.Two != null && originalData.Two.Length > 0)
                {
                    return MessagePackService.Deserialize<MessageData>(originalData.Two);
                }

                return null;
            }
            catch (Exception ex)
            {
                LogError($"解析消息数据异常: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建消息响应
        /// </summary>
        private OriginalData CreateMessageResponse(uint responseCommand, MessageData messageData, string statusMessage)
        {
            try
            {
                var response = new
                {
                    Command = $"0x{responseCommand:X4}",
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = "Success",
                    Message = statusMessage,
                    OriginalMessageType = messageData.MessageType.ToString(),
                    Data = messageData.BusinessData
                };

                var json = JsonConvert.SerializeObject(response);
                var responseData = Encoding.UTF8.GetBytes(json);

                return new OriginalData(
                    (byte)responseCommand,
                    responseData,
                    null
                );
            }
            catch (Exception ex)
            {
                LogError($"创建消息响应异常: {ex.Message}", ex);
                return new OriginalData((byte)responseCommand, null, null);
            }
        }
    }

    /// <summary>
    /// 消息数据模型
    /// </summary>
    [MessagePackObject]
    public class MessageData
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        [Key(0)]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 消息类型
        /// </summary>
        [Key(1)]
        public MessageType MessageType { get; set; } = MessageType.Text;

        /// <summary>
        /// 提示类型
        /// </summary>
        [Key(2)]
        public PromptType PromptType { get; set; } = PromptType.Information;

        /// <summary>
        /// 消息内容
        /// </summary>
        [Key(3)]
        public string MessageContent { get; set; }

        /// <summary>
        /// 下一步处理
        /// </summary>
        [Key(4)]
        public NextProcessStep NextStep { get; set; } = NextProcessStep.NoOperation;

        /// <summary>
        /// 接收者会话ID
        /// </summary>
        [Key(5)]
        public string ReceiverSessionId { get; set; }

        /// <summary>
        /// 业务数据
        /// </summary>
        [Key(6)]
        public object BusinessData { get; set; }

        /// <summary>
        /// 发送者信息
        /// </summary>
        [Key(7)]
        public string SenderInfo { get; set; }
    }
}
