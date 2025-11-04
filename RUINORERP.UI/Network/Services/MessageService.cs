using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using RUINORERP.UI.Network;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 消息服务类 - 提供客户端消息发送和接收功能
    /// 支持向指定用户、部门发送消息，以及广播消息等功能
    /// 支持双向通信，既可以发送消息也可以接收消息
    /// 注意：消息接收通过MessageCommandHandler集成到客户端命令处理架构
    /// </summary>
    public class MessageService
    {
        private readonly IMessageSender _messageSender;
        private readonly ILogger<MessageService> _logger;

        /// <summary>
        /// 当接收到弹窗消息时触发的事件
        /// </summary>
        public event Action<MessageData> PopupMessageReceived;

        /// <summary>
        /// 当接收到业务消息时触发的事件
        /// </summary>
        public event Action<MessageData> BusinessMessageReceived;

        /// <summary>
        /// 当接收到部门消息时触发的事件
        /// </summary>
        public event Action<MessageData> DepartmentMessageReceived;

        /// <summary>
        /// 当接收到广播消息时触发的事件
        /// </summary>
        public event Action<MessageData> BroadcastMessageReceived;

        /// <summary>
        /// 当接收到系统通知时触发的事件
        /// </summary>
        public event Action<MessageData> SystemNotificationReceived;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageSender">消息发送器</param>
        /// <param name="logger">日志记录器</param>
        public MessageService(
            IMessageSender messageSender,
            ILogger<MessageService> logger = null)
        {
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            _logger = logger;
            
            // 注意：不再直接订阅通信服务，消息接收通过MessageCommandHandler集成到客户端命令处理架构
        }

        /// <summary>
        /// 触发弹窗消息接收事件 (由MessageCommandHandler调用)
        /// </summary>
        /// <param name="messageData">消息数据</param>
        public void OnPopupMessageReceived(MessageData messageData)
        {
            try
            {
                PopupMessageReceived?.Invoke(messageData);
                _logger?.LogDebug("触发弹窗消息事件 - ID: {MessageId}", messageData?.Id);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发弹窗消息事件时发生异常");
            }
        }

        /// <summary>
        /// 触发业务消息接收事件 (由MessageCommandHandler调用)
        /// </summary>
        /// <param name="messageData">消息数据</param>
        public void OnBusinessMessageReceived(MessageData messageData)
        {
            try
            {
                BusinessMessageReceived?.Invoke(messageData);
                _logger?.LogDebug("触发业务消息事件 - ID: {MessageId}", messageData?.Id);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发业务消息事件时发生异常");
            }
        }

        /// <summary>
        /// 触发部门消息接收事件 (由MessageCommandHandler调用)
        /// </summary>
        /// <param name="messageData">消息数据</param>
        public void OnDepartmentMessageReceived(MessageData messageData)
        {
            try
            {
                DepartmentMessageReceived?.Invoke(messageData);
                _logger?.LogDebug("触发部门消息事件 - ID: {MessageId}", messageData?.Id);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发部门消息事件时发生异常");
            }
        }

        /// <summary>
        /// 触发广播消息接收事件 (由MessageCommandHandler调用)
        /// </summary>
        /// <param name="messageData">消息数据</param>
        public void OnBroadcastMessageReceived(MessageData messageData)
        {
            try
            {
                BroadcastMessageReceived?.Invoke(messageData);
                _logger?.LogDebug("触发广播消息事件 - ID: {MessageId}", messageData?.Id);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发广播消息事件时发生异常");
            }
        }

        /// <summary>
        /// 触发系统通知接收事件 (由MessageCommandHandler调用)
        /// </summary>
        /// <param name="messageData">消息数据</param>
        public void OnSystemNotificationReceived(MessageData messageData)
        {
            try
            {
                SystemNotificationReceived?.Invoke(messageData);
                _logger?.LogDebug("触发系统通知事件 - ID: {MessageId}", messageData?.Id);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发系统通知事件时发生异常");
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
                var request = new MessageRequest();
                request.Data = new
                {
                    TargetUserId = targetUserId,
                    Message = message,
                    Title = title,
                    MessageType = MessageType.Prompt.ToString()
                };
                
                var response = await _messageSender.SendCommandWithResponseAsync<MessageResponse>(
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
                return MessageResponse.Fail(MessageType.Prompt,  $"发送消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 转发弹窗消息给其他用户
        /// </summary>
        /// <param name="originalMessageId">原始消息ID</param>
        /// <param name="targetUserIds">目标用户ID列表</param>
        /// <param name="additionalMessage">附加消息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> ForwardPopupMessageAsync(
            Guid originalMessageId,
            List<Guid> targetUserIds,
            string additionalMessage = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new MessageRequest();
                request.Data = new
                {
                    OriginalMessageId = originalMessageId,
                    TargetUserIds = targetUserIds,
                    AdditionalMessage = additionalMessage,
                    MessageType = MessageType.Prompt.ToString()
                };
                var response = await _messageSender.SendCommandWithResponseAsync<MessageResponse>(
                    MessageCommands.ForwardPopupMessage, request, cancellationToken);

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
                return MessageResponse.Fail(MessageType.Prompt,  $"转发消息失败: {ex.Message}");
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
            MessageType messageType = MessageType.Text,
            CancellationToken ct = default)
        {
            try
            {
                var request = new MessageRequest();
                request.Data = new
                {
                    TargetUserId = targetUserId,
                    Message = message,
                    MessageType = messageType.ToString()
                };
                var response = await _messageSender.SendCommandWithResponseAsync<MessageResponse>(
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
                return MessageResponse.Fail(messageType,  $"发送消息失败: {ex.Message}");
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
            MessageType messageType = MessageType.Text,
            CancellationToken ct = default)
        {
            try
            {
                var request = new MessageRequest();
                request.Data = new
                {
                    DepartmentId = departmentId,
                    Message = message,
                    MessageType = messageType.ToString()
                };
                var response = await _messageSender.SendCommandWithResponseAsync<MessageResponse>(
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
                return MessageResponse.Fail(messageType,  $"发送消息失败: {ex.Message}");
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
            MessageType messageType = MessageType.Text,
            CancellationToken ct = default)
        {
            try
            {
                var request = new MessageRequest();
                request.Data = new
                {
                    Message = message,
                    MessageType = messageType.ToString()
                };
                var response = await _messageSender.SendCommandWithResponseAsync<MessageResponse>(
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
                return MessageResponse.Fail(messageType,  $"广播消息失败: {ex.Message}");
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
            MessageType notificationType = MessageType.System,
            CancellationToken ct = default)
        {
            try
            {
                var request = new MessageRequest();
                request.Data = new
                {
                    Message = message,
                    NotificationType = notificationType.ToString()
                };
                var response = await _messageSender.SendCommandWithResponseAsync<MessageResponse>(
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
                return MessageResponse.Fail(MessageType.System,  $"发送通知失败: {ex.Message}");
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
        public MessageType MessageType { get; set; }

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