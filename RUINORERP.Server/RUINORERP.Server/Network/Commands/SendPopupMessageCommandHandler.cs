using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少IMessageService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 发送弹窗消息命令处理器 - 处理向客户端发送弹窗消息的请求
    /// </summary>
    [CommandHandler("SendPopupMessageCommandHandler", priority: 88)]
    public class SendPopupMessageCommandHandler : UnifiedCommandHandlerBase
    {
        // private readonly IMessageService _messageService; // 暂时注释，缺少IMessageService接口定义

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public SendPopupMessageCommandHandler() : base()
        {
            // _messageService = Program.ServiceProvider.GetRequiredService<IMessageService>(); // 暂时注释，缺少IMessageService接口定义
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SendPopupMessageCommandHandler(
            // IMessageService messageService, // 暂时注释，缺少IMessageService接口定义
            ILogger<SendPopupMessageCommandHandler> logger = null) : base(logger)
        {
            // _messageService = messageService; // 暂时注释，缺少IMessageService接口定义
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)MessageCommands.SendPopupMessage
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 88;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == MessageCommands.SendPopupMessage)
                {
                    return await HandleSendPopupMessageAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理发送弹窗消息命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理发送弹窗消息命令
        /// </summary>
        private async Task<CommandResult> HandleSendPopupMessageAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理发送弹窗消息命令 [会话: {command.SessionID}]");

                // 解析弹窗消息数据
                var popupData = command.Packet.GetJsonData<PopupMessageData>();
                if (popupData == null)
                {
                    return CommandResult.Failure("弹窗消息数据格式错误", "INVALID_POPUP_DATA");
                }

                // 暂时返回模拟结果，因为缺少IMessageService接口定义
                var sendResult = new PopupMessageResult
                {
                    IsSuccess = true,
                    SentCount = popupData.TargetUserIds?.Length ?? 0,
                    Message = "弹窗消息发送成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreatePopupMessageResponse(sendResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        Title = popupData.Title,
                        MessageType = popupData.MessageType,
                        TargetCount = popupData.TargetUserIds?.Length ?? 0,
                        SentCount = sendResult.SentCount,
                        IsSuccess = sendResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: sendResult.IsSuccess ? "弹窗消息发送成功" : "弹窗消息发送失败"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理发送弹窗消息命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"弹窗消息发送异常: {ex.Message}", "POPUP_MESSAGE_ERROR", ex);
            }
        }

        /// <summary>
        /// 解析弹窗消息数据
        /// </summary>
        private PopupMessageData ParsePopupMessageData(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return null;

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                if (parts.Length >= 4)
                {
                    return new PopupMessageData
                    {
                        Title = parts[0],
                        Content = parts[1],
                        MessageType = parts[2],
                        TargetUserIds = parts[3].Split(',')
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                LogError($"解析弹窗消息数据异常: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建弹窗消息响应
        /// </summary>
        private OriginalData CreatePopupMessageResponse(PopupMessageResult sendResult)
        {
            var responseData = $"POPUP_RESULT|{sendResult.IsSuccess}|{sendResult.SentCount}|{sendResult.Message}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)MessageCommands.SendPopupMessage;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }
    }

    /// <summary>
    /// 弹窗消息数据
    /// </summary>
    public class PopupMessageData
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string MessageType { get; set; }
        public string[] TargetUserIds { get; set; }
    }

    /// <summary>
    /// 弹窗消息发送结果
    /// </summary>
    public class PopupMessageResult
    {
        public bool IsSuccess { get; set; }
        public int SentCount { get; set; }
        public string Message { get; set; }
    }
}