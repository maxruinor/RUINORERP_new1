using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using RUINORERP.UI.Network;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 消息服务类 - 提供客户端消息发送和接收功能
    /// 支持向指定用户、部门发送消息，以及广播消息等功能
    /// 支持双向通信，既可以发送消息也可以接收消息
    /// </summary>
    public class MessageService
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<MessageService> _logger;

        /// <summary>
        /// 当接收到弹窗消息时触发的事件
        /// </summary>
        public event Action<MessageReceivedEventArgs> PopupMessageReceived;

        /// <summary>
        /// 当接收到用户消息时触发的事件
        /// </summary>
        public event Action<MessageReceivedEventArgs> UserMessageReceived;

        /// <summary>
        /// 当接收到部门消息时触发的事件
        /// </summary>
        public event Action<MessageReceivedEventArgs> DepartmentMessageReceived;

        /// <summary>
        /// 当接收到广播消息时触发的事件
        /// </summary>
        public event Action<MessageReceivedEventArgs> BroadcastMessageReceived;

        /// <summary>
        /// 当接收到系统通知时触发的事件
        /// </summary>
        public event Action<MessageReceivedEventArgs> SystemNotificationReceived;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="logger">日志记录器</param>
        public MessageService(
            ClientCommunicationService communicationService,
            ILogger<MessageService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _logger = logger;

            // 订阅消息相关的命令
            SubscribeToMessages();
        }

        /// <summary>
        /// 订阅消息相关的命令
        /// </summary>
        private void SubscribeToMessages()
        {
            // 订阅弹窗消息
            _communicationService.SubscribeCommand(
                MessageCommands.SendPopupMessage,
                (packet, data) => HandlePopupMessageReceived(packet, data));

            // 订阅用户消息
            _communicationService.SubscribeCommand(
                MessageCommands.SendMessageToUser,
                (packet, data) => HandleUserMessageReceived(packet, data));

            // 订阅部门消息
            _communicationService.SubscribeCommand(
                MessageCommands.SendMessageToDepartment,
                (packet, data) => HandleDepartmentMessageReceived(packet, data));

            // 订阅广播消息
            _communicationService.SubscribeCommand(
                MessageCommands.BroadcastMessage,
                (packet, data) => HandleBroadcastMessageReceived(packet, data));

            // 订阅系统通知
            _communicationService.SubscribeCommand(
                MessageCommands.SendSystemNotification,
                (packet, data) => HandleSystemNotificationReceived(packet, data));
        }

        /// <summary>
        /// 处理接收到的弹窗消息
        /// </summary>
        private void HandlePopupMessageReceived(PacketModel packet, object data)
        {
            try
            {
                var messageData = data as MessageResponse;
                if (messageData?.Data != null)
                {
                    var args = new MessageReceivedEventArgs
                    {
                        MessageType = "Popup",
                        Data = messageData.Data,
                        Timestamp = DateTime.Now
                    };

                    PopupMessageReceived?.Invoke(args);
                    _logger?.LogDebug("接收到弹窗消息");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理弹窗消息时发生异常");
            }
        }

        /// <summary>
        /// 处理接收到的用户消息
        /// </summary>
        private void HandleUserMessageReceived(PacketModel packet, object data)
        {
            try
            {
                var messageData = data as MessageResponse;
                if (messageData?.Data != null)
                {
                    var args = new MessageReceivedEventArgs
                    {
                        MessageType = "User",
                        Data = messageData.Data,
                        Timestamp = DateTime.Now
                    };

                    UserMessageReceived?.Invoke(args);
                    _logger?.LogDebug("接收到用户消息");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理用户消息时发生异常");
            }
        }

        /// <summary>
        /// 处理接收到的部门消息
        /// </summary>
        private void HandleDepartmentMessageReceived(PacketModel packet, object data)
        {
            try
            {
                var messageData = data as MessageResponse;
                if (messageData?.Data != null)
                {
                    var args = new MessageReceivedEventArgs
                    {
                        MessageType = "Department",
                        Data = messageData.Data,
                        Timestamp = DateTime.Now
                    };

                    DepartmentMessageReceived?.Invoke(args);
                    _logger?.LogDebug("接收到部门消息");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理部门消息时发生异常");
            }
        }

        /// <summary>
        /// 处理接收到的广播消息
        /// </summary>
        private void HandleBroadcastMessageReceived(PacketModel packet, object data)
        {
            try
            {
                var messageData = data as MessageResponse;
                if (messageData?.Data != null)
                {
                    var args = new MessageReceivedEventArgs
                    {
                        MessageType = "Broadcast",
                        Data = messageData.Data,
                        Timestamp = DateTime.Now
                    };

                    BroadcastMessageReceived?.Invoke(args);
                    _logger?.LogDebug("接收到广播消息");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理广播消息时发生异常");
            }
        }

        /// <summary>
        /// 处理接收到的系统通知
        /// </summary>
        private void HandleSystemNotificationReceived(PacketModel packet, object data)
        {
            try
            {
                var messageData = data as MessageResponse;
                if (messageData?.Data != null)
                {
                    var args = new MessageReceivedEventArgs
                    {
                        MessageType = "SystemNotification",
                        Data = messageData.Data,
                        Timestamp = DateTime.Now
                    };

                    SystemNotificationReceived?.Invoke(args);
                    _logger?.LogDebug("接收到系统通知");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理系统通知时发生异常");
            }
        }

        /// <summary>
        /// 发送弹窗消息给指定用户
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="title">消息标题</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendPopupMessageAsync(
            string targetUserId,
            string message,
            string title = "系统消息",
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    TargetUserId = targetUserId,
                    Message = message,
                    Title = title,
                    MessageType = "Popup"
                };

                var request = new MessageRequest(Model.TransModel.MessageCmdType.Message, messageData);
                var response = await _communicationService.SendCommandWithResponseAsync<MessageResponse>(
                    MessageCommands.SendPopupMessage, request, ct);

                // 只记录关键信息和错误
                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("弹窗消息发送成功 - 目标用户: {TargetUserId}", targetUserId);
                }
                else
                {
                    _logger?.LogWarning("弹窗消息发送失败 - 目标用户: {TargetUserId}, 错误: {ErrorMessage}",
                        targetUserId, response?.ErrorMessage ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送弹窗消息时发生异常");
                return MessageResponse.Fail(MessageCmdType.Message,  $"发送消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 转发弹窗消息给其他用户
        /// </summary>
        /// <param name="originalMessageId">原始消息ID</param>
        /// <param name="targetUserIds">目标用户ID列表</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> ForwardPopupMessageAsync(
            string originalMessageId,
            string[] targetUserIds,
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    OriginalMessageId = originalMessageId,
                    TargetUserIds = targetUserIds,
                    MessageType = "ForwardPopup"
                };

                var request = new MessageRequest(messageData);
                var response = await _communicationService.SendCommandWithResponseAsync<MessageResponse>(
                    MessageCommands.ForwardPopupMessage, request, ct);

                // 只记录关键信息和错误
                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("转发弹窗消息成功 - 原始消息: {OriginalMessageId}", originalMessageId);
                }
                else
                {
                    _logger?.LogWarning("转发弹窗消息失败 - 原始消息: {OriginalMessageId}, 错误: {ErrorMessage}",
                        originalMessageId, response?.ErrorMessage ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "转发弹窗消息时发生异常");
                return MessageResponse.Fail(MessageCmdType.Message,  $"转发消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送消息给指定用户
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendMessageToUserAsync(
            string targetUserId,
            string message,
            string messageType = "Text",
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    TargetUserId = targetUserId,
                    Message = message,
                    MessageType = messageType
                };

                var request = new MessageRequest(messageData);
                var response = await _communicationService.SendCommandWithResponseAsync<MessageResponse>(
                    MessageCommands.SendMessageToUser, request, ct);

                // 只记录关键信息和错误
                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("用户消息发送成功 - 目标用户: {TargetUserId}", targetUserId);
                }
                else
                {
                    _logger?.LogWarning("用户消息发送失败 - 目标用户: {TargetUserId}, 错误: {ErrorMessage}",
                        targetUserId, response?.ErrorMessage ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送用户消息时发生异常");
                return MessageResponse.Fail(MessageCmdType.Message,  $"发送消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送消息给指定部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendMessageToDepartmentAsync(
            string departmentId,
            string message,
            string messageType = "Text",
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    DepartmentId = departmentId,
                    Message = message,
                    MessageType = messageType
                };

                var request = new MessageRequest(messageData);
                var response = await _communicationService.SendCommandWithResponseAsync<MessageResponse>(
                    MessageCommands.SendMessageToDepartment, request, ct);

                // 只记录关键信息和错误
                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("部门消息发送成功 - 部门: {DepartmentId}", departmentId);
                }
                else
                {
                    _logger?.LogWarning("部门消息发送失败 - 部门: {DepartmentId}, 错误: {ErrorMessage}",
                        departmentId, response?.ErrorMessage ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送部门消息时发生异常");
                return MessageResponse.Fail(MessageCmdType.Message,  $"发送消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 广播消息给所有用户
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> BroadcastMessageAsync(
            string message,
            string messageType = "Text",
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    Message = message,
                    MessageType = messageType
                };

                var request = new MessageRequest(messageData);
                var response = await _communicationService.SendCommandWithResponseAsync<MessageResponse>(
                    MessageCommands.BroadcastMessage, request, ct);

                // 只记录关键信息和错误
                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("广播消息发送成功");
                }
                else
                {
                    _logger?.LogWarning("广播消息发送失败 - 错误: {ErrorMessage}", response?.ErrorMessage ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "广播消息时发生异常");
                return MessageResponse.Fail(MessageCmdType.Message,  $"广播消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送系统通知
        /// </summary>
        /// <param name="message">通知内容</param>
        /// <param name="notificationType">通知类型</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendSystemNotificationAsync(
            string message,
            string notificationType = "Info",
            CancellationToken ct = default)
        {
            try
            {
                var messageData = new
                {
                    Message = message,
                    NotificationType = notificationType
                };

                var request = new MessageRequest(messageData);
                var response = await _communicationService.SendCommandWithResponseAsync<MessageResponse>(
                    MessageCommands.SendSystemNotification, request, ct);

                // 只记录关键信息和错误
                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("系统通知发送成功");
                }
                else
                {
                    _logger?.LogWarning("系统通知发送失败 - 错误: {ErrorMessage}", response?.ErrorMessage ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送系统通知时发生异常");
                return MessageResponse.Fail(MessageCmdType.Message,  $"发送通知失败: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 消息接收事件参数
    /// </summary>
    public class MessageReceivedEventArgs
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 消息数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}