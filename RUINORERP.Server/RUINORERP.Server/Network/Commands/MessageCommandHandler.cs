using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Handlers;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.PacketSpec.Enums.Message;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands.Handlers;
using Microsoft.Extensions.DependencyInjection; // 添加依赖注入相关命名空间

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 统一消息命令处理器 - 整合了命令模式和处理器模式的消息处理
    /// 包含消息发送、接收、广播、弹窗消息、转发等核心功能
    /// 移除了重复代码，统一了消息处理核心逻辑
    /// </summary>
    /// <summary>
    /// 消息命令处理器(实例)
    /// </summary>
    [CommandHandler("MessageCommandsHandler", priority: 90)]
    public class MessageCommandsHandler : BaseCommandHandler
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<MessageCommandsHandler> _logger;

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            
            (uint)MessageCommands.BroadcastMessage,
            (uint)MessageCommands.SendPopupMessage,
            (uint)MessageCommands.ForwardPopupMessage,
            (uint)MessageCommands.NotificationMessage
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 90;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageCommandsHandler(SessionService sessionService, ILogger<MessageCommandsHandler> logger = null)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _logger = logger;
        }

        /// <summary>
        /// 执行核心处理逻辑
        /// </summary>
        protected override async Task<CommandResult> OnHandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandType = (MessageCommands)command.CommandType;
                
                return commandType switch
                {
                    MessageCommands.SendMessage => await HandleSendMessageAsync(command, cancellationToken),
                    MessageCommands.ReceiveMessage => await HandleReceiveMessageAsync(command, cancellationToken),
                    MessageCommands.MessageRead => await HandleMessageReadAsync(command, cancellationToken),
                    MessageCommands.BroadcastMessage => await HandleBroadcastMessageAsync(command, cancellationToken),
                    MessageCommands.SendPopupMessage => await HandleSendPopupMessageAsync(command, cancellationToken),
                    MessageCommands.ForwardPopupMessage => await HandleForwardPopupMessageAsync(command, cancellationToken),
                    MessageCommands.NotificationMessage => await HandleNotificationAsync(command, cancellationToken),
                    MessageCommands.BusinessData => await HandleBusinessDataAsync(command, cancellationToken),
                    _ => CommandResult.Failure($"不支持的消息命令类型: {commandType}", "UNSUPPORTED_MESSAGE_COMMAND")
                };
            }
            catch (Exception ex)
            {
                LogError($"处理消息命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"消息处理异常: {ex.Message}", "MESSAGE_HANDLER_ERROR", ex);
            }
        }

        #region 核心消息处理方法

        /// <summary>
        /// 处理发送消息命令
        /// </summary>
        private async Task<CommandResult> HandleSendMessageAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理发送消息请求 [会话: {command.SessionInfo?.SessionId}]");
            
            try
            {
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("消息数据格式错误", "INVALID_MESSAGE_DATA");
                }

                if (string.IsNullOrEmpty(messageData.ReceiverSessionId))
                {
                    return CommandResult.Failure("接收者会话ID不能为空", "INVALID_RECEIVER_SESSION");
                }

                LogInfo($"发送消息: From={command.SessionInfo?.SessionId}, To={messageData.ReceiverSessionId}, Content={messageData.MessageContent?.Substring(0, Math.Min(50, messageData.MessageContent?.Length ?? 0))}...");

                // 创建接收消息数据包
                var receivePacket = CreateReceiveMessagePacket(command.SessionInfo, messageData);

                // 发送消息到目标会话
                var success = await _sessionService.SendPacketAsync(messageData.ReceiverSessionId, receivePacket);

                var responseData = CreateMessageResponse(
                    MessageCommands.SendMessage,
                    new { 
                        Success = success, 
                        Message = success ? "消息发送成功" : "消息发送失败",
                        MessageId = Guid.NewGuid(),
                        Timestamp = DateTime.UtcNow
                    }
                );

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        Success = success, 
                        MessageId = Guid.NewGuid(),
                        Timestamp = DateTime.UtcNow 
                    },
                    message: success ? "消息发送成功" : "消息发送失败"
                );
            }
            catch (Exception ex)
            {
                LogError($"发送消息时出错: {ex.Message}", ex);
                return CommandResult.Failure($"发送消息失败: {ex.Message}", "SEND_MESSAGE_FAILED", ex);
            }
        }

        /// <summary>
        /// 处理接收消息命令
        /// </summary>
        private async Task<CommandResult> HandleReceiveMessageAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理接收消息请求 [会话: {command.SessionInfo?.SessionId}]");
            
            try
            {
                // 这里可以实现消息队列或离线消息的拉取逻辑
                LogInfo($"客户端请求接收消息: SessionID={command.SessionInfo?.SessionId}");

                // 模拟返回消息列表
                var messages = new[]
                {
                    new ReceiveMessageData
                    {
                        FromSessionId = "SYSTEM",
                        FromUserName = "System",
                        Content = "欢迎使用RUINORERP系统",
                        MessageType = "system",
                        Timestamp = DateTime.UtcNow
                    }
                };

                var responseData = CreateMessageResponse(
                    MessageCommands.ReceiveMessage,
                    new {
                        Success = true,
                        Messages = messages,
                        Count = messages.Length,
                        Timestamp = DateTime.UtcNow
                    }
                );

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        Messages = messages, 
                        Count = messages.Length,
                        Timestamp = DateTime.UtcNow 
                    },
                    message: "消息获取成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"接收消息时出错: {ex.Message}", ex);
                return CommandResult.Failure($"接收消息失败: {ex.Message}", "RECEIVE_MESSAGE_FAILED", ex);
            }
        }

        /// <summary>
        /// 处理消息已读确认命令
        /// </summary>
        private async Task<CommandResult> HandleMessageReadAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理消息已读确认 [会话: {command.SessionInfo?.SessionId}]");
            
            try
            {
                var readData = ParseReadData(command.OriginalData);
                
                LogInfo($"消息已读确认: SessionID={command.SessionInfo?.SessionId}, MessageId={readData?.MessageId}");

                // 这里可以实现消息已读状态的更新逻辑
                // await UpdateMessageReadStatus(readData.MessageId, command.sessionInfo.SessionID);

                var responseData = CreateMessageResponse(
                    MessageCommands.MessageRead,
                    new {
                        Success = true,
                        Message = "已读状态更新成功",
                        MessageId = readData?.MessageId,
                        Timestamp = DateTime.UtcNow
                    }
                );

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        MessageId = readData?.MessageId,
                        Timestamp = DateTime.UtcNow 
                    },
                    message: "已读状态更新成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理消息已读时出错: {ex.Message}", ex);
                return CommandResult.Failure($"处理消息已读失败: {ex.Message}", "MESSAGE_READ_FAILED", ex);
            }
        }

        /// <summary>
        /// 处理广播消息命令
        /// </summary>
        private async Task<CommandResult> HandleBroadcastMessageAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理广播消息请求 [会话: {command.SessionInfo?.SessionId}]");
            
            try
            {
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("广播消息数据格式错误", "INVALID_BROADCAST_DATA");
                }

                LogInfo($"广播消息: From={command.SessionInfo?.SessionId}, Content={messageData.MessageContent?.Substring(0, Math.Min(50, messageData.MessageContent?.Length ?? 0))}...");

                // 创建广播消息数据包
                var broadcastPacket = CreateBroadcastMessagePacket(command.SessionInfo, messageData);

                // 广播到所有活动会话
                var sentCount = await _sessionService.BroadcastPacketAsync(broadcastPacket);

                var responseData = CreateMessageResponse(
                    MessageCommands.BroadcastMessage,
                    new {
                        Success = sentCount > 0,
                        Message = $"广播消息已发送给 {sentCount} 个客户端",
                        SentCount = sentCount,
                        Timestamp = DateTime.UtcNow
                    }
                );

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        SentCount = sentCount,
                        Timestamp = DateTime.UtcNow 
                    },
                    message: $"广播消息已发送给 {sentCount} 个客户端"
                );
            }
            catch (Exception ex)
            {
                LogError($"广播消息时出错: {ex.Message}", ex);
                return CommandResult.Failure($"广播消息失败: {ex.Message}", "BROADCAST_MESSAGE_FAILED", ex);
            }
        }

        /// <summary>
        /// 处理发送弹窗消息命令
        /// </summary>
        private async Task<CommandResult> HandleSendPopupMessageAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理发送弹窗消息 [会话: {command.SessionInfo?.SessionId}]");
            
            try
            {
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("弹窗消息数据格式错误", "INVALID_POPUP_DATA");
                }

                if (string.IsNullOrEmpty(messageData.ReceiverSessionId))
                {
                    return CommandResult.Failure("接收者会话ID不能为空", "INVALID_RECEIVER_SESSION");
                }

                LogInfo($"发送弹窗消息: From={command.SessionInfo?.SessionId}, To={messageData.ReceiverSessionId}, Content={messageData.MessageContent}");

                // 创建弹窗消息数据包
                var popupPacket = CreatePopupMessagePacket(command.SessionInfo, messageData);

                // 发送弹窗消息到目标会话
                var success = await _sessionService.SendPacketAsync(messageData.ReceiverSessionId, popupPacket);

                var responseData = CreateMessageResponse(
                    MessageCommands.SendPopupMessage,
                    new {
                        Success = success,
                        Message = success ? "弹窗消息发送成功" : "弹窗消息发送失败",
                        MessageId = Guid.NewGuid(),
                        Timestamp = DateTime.UtcNow
                    }
                );

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        Success = success, 
                        MessageId = Guid.NewGuid(),
                        Timestamp = DateTime.UtcNow 
                    },
                    message: success ? "弹窗消息发送成功" : "弹窗消息发送失败"
                );
            }
            catch (Exception ex)
            {
                LogError($"发送弹窗消息时出错: {ex.Message}", ex);
                return CommandResult.Failure($"发送弹窗消息失败: {ex.Message}", "SEND_POPUP_FAILED", ex);
            }
        }

        /// <summary>
        /// 处理转发弹窗消息命令
        /// </summary>
        private async Task<CommandResult> HandleForwardPopupMessageAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理转发弹窗消息 [会话: {command.SessionInfo?.SessionId}]");
            
            try
            {
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("转发弹窗消息数据格式错误", "INVALID_FORWARD_DATA");
                }

                if (string.IsNullOrEmpty(messageData.ReceiverSessionId))
                {
                    return CommandResult.Failure("接收者会话ID不能为空", "INVALID_RECEIVER_SESSION");
                }

                LogInfo($"转发弹窗消息: From={command.SessionInfo?.SessionId}, To={messageData.ReceiverSessionId}, Content={messageData.MessageContent}");

                // 创建转发弹窗消息数据包
                var forwardPacket = CreateForwardPopupMessagePacket(command.SessionInfo, messageData);

                // 发送转发弹窗消息到目标会话
                var success = await _sessionService.SendPacketAsync(messageData.ReceiverSessionId, forwardPacket);

                var responseData = CreateMessageResponse(
                    MessageCommands.ForwardPopupMessage,
                    new {
                        Success = success,
                        Message = success ? "弹窗消息转发成功" : "弹窗消息转发失败",
                        MessageId = Guid.NewGuid(),
                        Timestamp = DateTime.UtcNow
                    }
                );

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        Success = success, 
                        MessageId = Guid.NewGuid(),
                        Timestamp = DateTime.UtcNow 
                    },
                    message: success ? "弹窗消息转发成功" : "弹窗消息转发失败"
                );
            }
            catch (Exception ex)
            {
                LogError($"转发弹窗消息时出错: {ex.Message}", ex);
                return CommandResult.Failure($"转发弹窗消息失败: {ex.Message}", "FORWARD_POPUP_FAILED", ex);
            }
        }

        /// <summary>
        /// 处理通知命令
        /// </summary>
        private async Task<CommandResult> HandleNotificationAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理通知请求 [会话: {command.SessionInfo?.SessionId}]");
            
            try
            {
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("通知数据格式错误", "INVALID_NOTIFICATION_DATA");
                }

                LogInfo($"处理通知: {messageData.MessageContent}");

                // 根据通知内容进行不同处理
                var notificationResult = new
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    Content = messageData.MessageContent,
                    ProcessedAt = DateTime.UtcNow,
                    Status = "Processed"
                };

                var responseData = CreateMessageResponse(
                    MessageCommands.NotificationMessage,
                    notificationResult
                );

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: notificationResult,
                    message: "通知处理成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理通知时出错: {ex.Message}", ex);
                return CommandResult.Failure($"处理通知失败: {ex.Message}", "NOTIFICATION_FAILED", ex);
            }
        }

        /// <summary>
        /// 处理业务数据命令
        /// </summary>
        private async Task<CommandResult> HandleBusinessDataAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理业务数据请求 [会话: {command.SessionInfo?.SessionId}]");
            
            try
            {
                var messageData = ParseMessageData(command.OriginalData);
                if (messageData == null)
                {
                    return CommandResult.Failure("业务数据格式错误", "INVALID_BUSINESS_DATA");
                }

                LogInfo($"处理业务数据: Type={messageData.MessageType}, ContentLength={messageData.MessageContent?.Length}");

                // 处理业务数据
                var businessResult = new
                {
                    DataId = Guid.NewGuid().ToString(),
                    ProcessedAt = DateTime.UtcNow,
                    Status = "Processed",
                    DataType = messageData.MessageType.ToString()
                };

                var responseData = CreateMessageResponse(
                    MessageCommands.BusinessData,
                    businessResult
                );

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: businessResult,
                    message: "业务数据处理成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理业务数据时出错: {ex.Message}", ex);
                return CommandResult.Failure($"处理业务数据失败: {ex.Message}", "BUSINESS_DATA_FAILED", ex);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 创建接收消息数据包
        /// </summary>
        private PacketInfo CreateReceiveMessagePacket(SessionInfo sessionInfo, MessageData messageData)
        {
            var receivePacket = new PacketInfo
            {
                Command = (uint)MessageCommands.ReceiveMessage,
                SessionId = messageData.ReceiverSessionId
            };

            var receiveData = new ReceiveMessageData
            {
                FromSessionId = sessionInfo?.SessionId,
                FromUserName = GetUserNameFromSession(sessionInfo),
                Content = messageData.MessageContent,
                MessageType = GetMessageTypeString(messageData.MessageType),
                Timestamp = DateTime.UtcNow
            };

            receivePacket.SetDataFromJson(receiveData);
            return receivePacket;
        }

        /// <summary>
        /// 创建广播消息数据包
        /// </summary>
        private PacketInfo CreateBroadcastMessagePacket(SessionInfo sessionInfo, MessageData messageData)
        {
            var broadcastPacket = new PacketInfo
            {
                Command = (uint)MessageCommands.ReceiveMessage,
                SessionId = "BROADCAST"
            };

            var broadcastData = new ReceiveMessageData
            {
                FromSessionId = sessionInfo?.SessionId,
                FromUserName = GetUserNameFromSession(sessionInfo),
                Content = messageData.MessageContent,
                MessageType = "broadcast",
                Timestamp = DateTime.UtcNow
            };

            broadcastPacket.SetDataFromJson(broadcastData);
            return broadcastPacket;
        }

        /// <summary>
        /// 创建弹窗消息数据包
        /// </summary>
        private PacketInfo CreatePopupMessagePacket(SessionInfo sessionInfo, MessageData messageData)
        {
            var popupPacket = new PacketInfo
            {
                Command = (uint)MessageCommands.SendPopupMessage,
                SessionId = messageData.ReceiverSessionId
            };

            var popupData = new
            {
                FromSessionId = sessionInfo?.SessionId,
                FromUserName = GetUserNameFromSession(sessionInfo),
                Content = messageData.MessageContent,
                MessageType = "popup",
                Timestamp = DateTime.UtcNow,
                PopupType = messageData.PromptType.ToString()
            };

            popupPacket.SetDataFromJson(popupData);
            return popupPacket;
        }

        /// <summary>
        /// 创建转发弹窗消息数据包
        /// </summary>
        private PacketInfo CreateForwardPopupMessagePacket(SessionInfo sessionInfo, MessageData messageData)
        {
            var forwardPacket = new PacketInfo
            {
                Command = (uint)MessageCommands.ForwardPopupMessage,
                SessionId = messageData.ReceiverSessionId
            };

            var forwardData = new
            {
                OriginalSender = sessionInfo?.SessionId,
                ForwardedBy = GetUserNameFromSession(sessionInfo),
                Content = messageData.MessageContent,
                MessageType = "forwarded_popup",
                Timestamp = DateTime.UtcNow,
                OriginalTimestamp = messageData.Timestamp
            };

            forwardPacket.SetDataFromJson(forwardData);
            return forwardPacket;
        }

        /// <summary>
        /// 从会话信息获取用户名
        /// </summary>
        private string GetUserNameFromSession(SessionInfo sessionInfo)
        {
            if (sessionInfo == null || string.IsNullOrEmpty(sessionInfo.SessionID))
                return "Unknown";

            // 这里应该从会话服务获取用户名
            return _sessionService.GetSessionProperty<string>(sessionInfo.SessionID, "UserName", "Unknown");
        }

        /// <summary>
        /// 获取消息类型字符串
        /// </summary>
        private string GetMessageTypeString(MessageType messageType)
        {
            return messageType switch
            {
                MessageType.IM => "im",
                MessageType.Prompt => "prompt",
                MessageType.BusinessData => "business",
                MessageType.Text => "text",
                _ => "unknown"
            };
        }

        /// <summary>
        /// 解析消息数据
        /// </summary>
        private MessageData ParseMessageData(OriginalData originalData)
        {
            try
            {
                if (originalData.Two == null || originalData.Two.Length == 0)
                {
                    return null;
                }

                var data = new MessageData();
                int index = 0;

                // 解析时间戳
                var timeStr = GetStringFromBytes(originalData.Two, ref index);
                if (DateTime.TryParse(timeStr, out var timestamp))
                {
                    data.Timestamp = timestamp;
                }

                // 解析消息类型
                var messageTypeStr = GetStringFromBytes(originalData.Two, ref index);
                if (int.TryParse(messageTypeStr, out var messageTypeInt))
                {
                    data.MessageType = (MessageType)messageTypeInt;
                }

                // 根据消息类型解析不同的数据
                switch (data.MessageType)
                {
                    case MessageType.IM:
                        data.MessageContent = GetStringFromBytes(originalData.Two, ref index);
                        var nextStepStr = GetStringFromBytes(originalData.Two, ref index);
                        if (int.TryParse(nextStepStr, out var nextStepInt))
                        {
                            data.NextStep = (NextProcessStep)nextStepInt;
                        }
                        data.ReceiverSessionId = GetStringFromBytes(originalData.Two, ref index);
                        break;

                    case MessageType.Prompt:
                        data.MessageContent = GetStringFromBytes(originalData.Two, ref index);
                        var promptTypeStr = GetStringFromBytes(originalData.Two, ref index);
                        if (int.TryParse(promptTypeStr, out var promptTypeInt))
                        {
                            data.PromptType = (PromptType)promptTypeInt;
                        }
                        data.ReceiverSessionId = GetStringFromBytes(originalData.Two, ref index);
                        break;

                    case MessageType.BusinessData:
                        var jsonData = GetStringFromBytes(originalData.Two, ref index);
                        if (!string.IsNullOrEmpty(jsonData))
                        {
                            try
                            {
                                data.BusinessData = JsonConvert.DeserializeObject(jsonData);
                            }
                            catch (Exception ex)
                            {
                                LogError($"解析业务数据JSON失败: {ex.Message}", ex);
                            }
                        }
                        break;

                    default:
                        data.MessageContent = GetStringFromBytes(originalData.Two, ref index);
                        break;
                }

                return data;
            }
            catch (Exception ex)
            {
                LogError($"解析消息数据异常: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 解析已读数据
        /// </summary>
        private ReadData ParseReadData(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return new ReadData();

                var dataString = Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                if (parts.Length >= 1)
                {
                    return new ReadData
                    {
                        MessageId = Guid.TryParse(parts[0], out var messageId) ? messageId : Guid.Empty,
                        ReadTime = parts.Length > 1 && DateTime.TryParse(parts[1], out var readTime) ? readTime : DateTime.Now
                    };
                }

                return new ReadData();
            }
            catch
            {
                return new ReadData();
            }
        }

        /// <summary>
        /// 创建消息响应
        /// </summary>
        private OriginalData CreateMessageResponse(MessageCommands responseCommand, object responseData)
        {
            try
            {
                var json = JsonConvert.SerializeObject(responseData);
                var dataBytes = Encoding.UTF8.GetBytes(json);

                // 将完整的CommandId正确分解为Category和OperationCode
                byte category = (byte)(responseCommand & 0xFF); // 取低8位作为Category
                byte operationCode = (byte)((responseCommand >> 8) & 0xFF); // 取次低8位作为OperationCode
                
                return new OriginalData(
                    category,
                    new byte[] { operationCode },
                    dataBytes
                );
            }
            catch (Exception ex)
            {
                LogError($"创建消息响应异常: {ex.Message}", ex);
                
                // 将完整的CommandId正确分解为Category和OperationCode
                byte category = (byte)(responseCommand & 0xFF); // 取低8位作为Category
                byte operationCode = (byte)((responseCommand >> 8) & 0xFF); // 取次低8位作为OperationCode
                
                return new OriginalData(category, new byte[] { operationCode }, null);
            }
        }

        /// <summary>
        /// 从字节数组中获取字符串
        /// </summary>
        private string GetStringFromBytes(byte[] bytes, ref int index)
        {
            if (bytes == null || index >= bytes.Length)
                return string.Empty;

            var nullIndex = Array.IndexOf(bytes, (byte)0, index);
            if (nullIndex == -1)
                nullIndex = bytes.Length;

            var length = nullIndex - index;
            if (length <= 0)
                return string.Empty;

            var result = Encoding.UTF8.GetString(bytes, index, length);
            index = nullIndex + 1;
            return result;
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        private void LogInfo(string message)
        {
            _logger?.LogInformation($"[MessageCommandsHandler] {message}");
            Console.WriteLine($"[MessageCommandsHandler] INFO: {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void LogError(string message, Exception ex = null)
        {
            _logger?.LogError(ex, $"[MessageCommandsHandler] {message}");
            Console.WriteLine($"[MessageCommandsHandler] ERROR: {message}");
            if (ex != null)
            {
                Console.WriteLine($"[MessageCommandsHandler] Exception: {ex}");
            }
        }

        #endregion
    }

    #region 数据模型类

    /// <summary>
    /// 接收消息数据
    /// </summary>
    public class ReceiveMessageData
    {
        public string FromSessionId { get; set; }
        public string FromUserName { get; set; }
        public string Content { get; set; }
        public string MessageType { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }

    /// <summary>
    /// 已读数据
    /// </summary>
    public class ReadData
    {
        public Guid MessageId { get; set; }
        public DateTime ReadTime { get; set; } = DateTime.Now;
    }

    #endregion
}