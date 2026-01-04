using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.UI.Network;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using RUINORERP.PacketSpec.Models.Messaging;
using System.Linq;

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
        private readonly Lazy<ClientCommunicationService> _messageSender;
        private readonly ILogger<MessageService> _logger;
        
        // 用于消息去重的集合，存储最近处理过的消息ID
        private readonly ConcurrentDictionary<string, DateTime> _processedMessages = new ConcurrentDictionary<string, DateTime>();
        // 消息去重的过期时间（毫秒）
        private const int MESSAGE_PROCESS_TIMEOUT = 5000; // 5秒内重复消息不处理
        
        // 本地消息存储：存储所有收到的消息
        private readonly ConcurrentDictionary<long, MessageData> _localMessages = new ConcurrentDictionary<long, MessageData>();
        // 最大本地消息存储数量
        private const int MAX_LOCAL_MESSAGES = 1000;

        /// <summary>
        /// 当接收到弹窗消息时触发的事件
        /// </summary>
        public event Action<MessageData> PopupMessageReceived;

        /// <summary>
        /// 当接收到业务消息时触发的事件
        /// </summary>
        public event Action<MessageData> BusinessMessageReceived;

        /// <summary>
        /// 当接收到系统通知时触发的事件
        /// </summary>
        public event Action<MessageData> SystemNotificationReceived;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageSender">消息发送器（延迟加载）</param>
        /// <param name="logger">日志记录器</param>
        public MessageService(
            Lazy<ClientCommunicationService> messageSender,
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
                // 检查消息数据是否有效
                if (messageData == null)
                {
                    _logger?.LogWarning("收到空的弹窗消息数据");
                    return;
                }
                
                // 消息去重处理
                if (!IsMessageValid(messageData.MessageId))
                {
                    _logger?.LogDebug("跳过重复的弹窗消息 - ID: {MessageId}", messageData.MessageId);
                    return;
                }
                
                // 保存消息到本地存储
                SaveMessageToLocalStorage(messageData);
                
                // 检查事件订阅者
                if (PopupMessageReceived == null)
                {
                    _logger?.LogWarning("弹窗消息事件没有订阅者 - ID: {MessageId}, 标题: {Title}", 
                        messageData.MessageId, messageData.Title);
                    // 手动调用弹窗显示方法作为备份
                    ShowPopupMessageDirectly(messageData);
                    return;
                }
                
                // 触发事件
                PopupMessageReceived.Invoke(messageData);
                _logger?.LogDebug("触发弹窗消息事件 - ID: {MessageId}, 标题: {Title}", 
                    messageData.MessageId, messageData.Title);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发弹窗消息事件时发生异常");
                // 异常发生时尝试直接显示消息
                try
                {
                    ShowPopupMessageDirectly(messageData);
                }
                catch (Exception innerEx)
                {
                    _logger?.LogError(innerEx, "直接显示弹窗消息也失败");
                }
            }
        }
        
        /// <summary>
        /// 检查消息是否有效（非重复且在有效期内）
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <returns>是否有效</returns>
        private bool IsMessageValid(long messageId)
        {
            string idStr = messageId.ToString();
            if (string.IsNullOrEmpty(idStr))
                return true; // 没有ID的消息无法去重，直接处理
            
            // 清理过期的消息记录
            CleanupExpiredMessages();
            
            // 检查是否为重复消息
            if (_processedMessages.TryGetValue(idStr, out DateTime timestamp))
            {
                // 消息在过期时间内，视为重复
                return false;
            }
            
            // 记录新消息
            _processedMessages[idStr] = DateTime.Now;
            return true;
        }
        
        /// <summary>
        /// 清理过期的消息记录
        /// </summary>
        private void CleanupExpiredMessages()
        {
            DateTime cutoffTime = DateTime.Now.AddMilliseconds(-MESSAGE_PROCESS_TIMEOUT);
            
            foreach (var key in _processedMessages.Keys)
            {
                if (_processedMessages.TryGetValue(key, out DateTime timestamp) && timestamp < cutoffTime)
                {
                    _processedMessages.TryRemove(key, out _);
                }
            }
        }
        
        /// <summary>
        /// 直接显示弹窗消息（作为备用机制，当事件没有订阅者时使用）
        /// </summary>
        /// <param name="messageData">消息数据</param>
        private void ShowPopupMessageDirectly(MessageData messageData)
        {
            try
            {
                _logger?.LogDebug("直接显示弹窗消息- 标题: {Title}", 
                    messageData?.MessageId, messageData?.Title);
                
                // 检查是否在UI线程
                if (SynchronizationContext.Current != null)
                {
                    // 直接在当前线程显示消息（假设当前是UI线程）
                    var messagePrompt = new RUINORERP.UI.IM.MessagePrompt();
                    messagePrompt.Title = messageData?.Title ?? "系统消息";
                    messagePrompt.Content = messageData?.Content ?? "";
                    messagePrompt.MessageData = messageData;
                    messagePrompt.ShowDialog();
                }
                else
                {
                    // 使用SynchronizationContext作为替代方案
                    SynchronizationContext context = new SynchronizationContext();
                    context.Post((state) =>
                    {
                        try
                        {
                            var prompt = new RUINORERP.UI.IM.MessagePrompt();
                            prompt.Title = messageData?.Title ?? "系统消息";
                            prompt.Content = messageData?.Content ?? "";
                            prompt.MessageData = messageData;
                            prompt.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "在同步上下文中显示弹窗消息失败");
                        }
                    }, null);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "直接显示弹窗消息失败");
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
                // 检查消息数据是否有效
                if (messageData == null)
                {
                    _logger?.LogWarning("收到空的业务消息数据");
                    return;
                }
                
                // 消息去重处理
                if (!IsMessageValid(messageData.MessageId))
                {
                    _logger?.LogDebug("跳过重复的业务消息 - ID: {MessageId}", messageData.MessageId);
                    return;
                }
                
                // 保存消息到本地存储
                SaveMessageToLocalStorage(messageData);
                
                // 检查事件订阅者
                if (BusinessMessageReceived == null)
                {
                    _logger?.LogWarning("业务消息事件没有订阅者 - ID: {MessageId}", messageData.MessageId);
                }
                
                // 触发事件
                BusinessMessageReceived?.Invoke(messageData);
                _logger?.LogDebug("触发业务消息事件 - ID: {MessageId}", messageData.MessageId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发业务消息事件时发生异常");
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
                // 检查消息数据是否有效
                if (messageData == null)
                {
                    _logger?.LogWarning("收到空的系统通知数据");
                    return;
                }
                
                // 消息去重处理
                if (!IsMessageValid(messageData.MessageId))
                {
                    _logger?.LogDebug("跳过重复的系统通知 - ID: {MessageId}", messageData.MessageId);
                    return;
                }
                
                // 保存消息到本地存储
                SaveMessageToLocalStorage(messageData);
                
                // 检查事件订阅者
                if (SystemNotificationReceived == null)
                {
                    _logger?.LogWarning("系统通知事件没有订阅者 - ID: {MessageId}", messageData.MessageId);
                }
                
                // 触发事件
                SystemNotificationReceived?.Invoke(messageData);
                _logger?.LogDebug("触发系统通知事件 - ID: {MessageId}", messageData.MessageId);
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
                    MessageType = MessageType.Popup.ToString()
                };
                
                var response = await _messageSender.Value.SendCommandWithResponseAsync<MessageResponse>(
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
                return MessageResponse.Fail(MessageType.Popup,  $"发送消息失败: {ex.Message}");
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
                    MessageType = MessageType.Popup.ToString()
                };
                var response = await _messageSender.Value.SendCommandWithResponseAsync<MessageResponse>(
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
                return MessageResponse.Fail(MessageType.Popup,  $"转发消息失败: {ex.Message}");
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
            MessageType messageType = MessageType.Popup,
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
                var response = await _messageSender.Value.SendCommandWithResponseAsync<MessageResponse>(
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
            MessageType messageType = MessageType.Popup,
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
                var response = await _messageSender.Value.SendCommandWithResponseAsync<MessageResponse>(
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
            MessageType messageType = MessageType.Popup,
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
                var response = await _messageSender.Value.SendCommandWithResponseAsync<MessageResponse>(
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
                var response = await _messageSender.Value.SendCommandWithResponseAsync<MessageResponse>(
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

        /// <summary>
        /// 发送业务通知命令
        /// </summary>
        /// <param name="command">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendCommandAsync(CommandId command, MessageRequest request, CancellationToken ct = default)
        {
            try
            {
                var response = await _messageSender.Value.SendCommandWithResponseAsync<MessageResponse>(
                    command, request, ct);

                // 只记录关键信息和错误
                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("业务命令发送成功 - 命令: {Command}", command.Name);
                }
                else
                {
                    _logger?.LogWarning("业务命令发送失败 - 命令: {Command}, 错误: {ErrorMessage}", 
                        command.Name, response?.ErrorMessage ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送业务命令时发生异常 - 命令: {Command}", command.Name);
                return MessageResponse.Fail(MessageType.Business, $"发送业务命令失败: {ex.Message}");
            }
        }

        #region 消息存储与管理

        /// <summary>
        /// 保存消息到本地存储
        /// </summary>
        /// <param name="messageData">消息数据</param>
        private void SaveMessageToLocalStorage(MessageData messageData)
        {
            if (messageData == null)
                return;

            // 如果消息数量超过最大值，删除最旧的消息
            if (_localMessages.Count >= MAX_LOCAL_MESSAGES)
            {
                // 找到最旧的消息
                var oldestMessage = _localMessages.OrderBy(m => m.Value.SendTime).FirstOrDefault();
                if (oldestMessage.Key != 0)
                {
                    _localMessages.TryRemove(oldestMessage.Key, out _);
                }
            }

            // 添加或更新消息
            _localMessages[messageData.MessageId] = messageData;
            _logger?.LogDebug("消息已保存到本地存储 - ID: {MessageId}", messageData.MessageId);
        }

        /// <summary>
        /// 获取历史消息
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="messageType">消息类型（可选，为null则获取所有类型）</param>
        /// <param name="isRead">是否已读（可选，为null则获取所有状态）</param>
        /// <returns>消息列表</returns>
        public List<MessageData> GetMessages(int pageIndex = 1, int pageSize = 20, MessageType? messageType = null, bool? isRead = null)
        {
            try
            {
                var query = _localMessages.Values.AsQueryable();

                // 根据消息类型过滤
                if (messageType.HasValue)
                {
                    query = query.Where(m => m.MessageType == messageType.Value);
                }

                // 根据已读状态过滤
                if (isRead.HasValue)
                {
                    query = query.Where(m => m.IsRead == isRead.Value);
                }

                // 按发送时间倒序排序，最新的消息在前
                query = query.OrderByDescending(m => m.SendTime);

                // 分页
                var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                _logger?.LogDebug("获取历史消息 - 页码: {PageIndex}, 每页大小: {PageSize}, 结果数量: {ResultCount}",
                    pageIndex, pageSize, result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取历史消息时发生异常");
                return new List<MessageData>();
            }
        }

        /// <summary>
        /// 获取消息总数
        /// </summary>
        /// <param name="messageType">消息类型（可选，为null则获取所有类型）</param>
        /// <param name="isRead">是否已读（可选，为null则获取所有状态）</param>
        /// <returns>消息总数</returns>
        public int GetMessageCount(MessageType? messageType = null, bool? isRead = null)
        {
            try
            {
                var query = _localMessages.Values.AsQueryable();

                // 根据消息类型过滤
                if (messageType.HasValue)
                {
                    query = query.Where(m => m.MessageType == messageType.Value);
                }

                // 根据已读状态过滤
                if (isRead.HasValue)
                {
                    query = query.Where(m => m.IsRead == isRead.Value);
                }

                var count = query.Count();
                _logger?.LogDebug("获取消息总数 - 类型: {MessageType}, 已读状态: {IsRead}, 数量: {Count}",
                    messageType, isRead, count);

                return count;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取消息总数时发生异常");
                return 0;
            }
        }

        /// <summary>
        /// 标记消息为已读
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <returns>是否成功</returns>
        public bool MarkAsRead(long messageId)
        {
            try
            {
                if (_localMessages.TryGetValue(messageId, out var messageData))
                {
                    messageData.IsRead = true;
                    messageData.ReadTime = DateTime.Now;
                    _localMessages[messageId] = messageData; // 更新存储
                    _logger?.LogDebug("消息已标记为已读 - ID: {MessageId}", messageId);
                    return true;
                }
                _logger?.LogWarning("未找到要标记为已读的消息 - ID: {MessageId}", messageId);
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "标记消息为已读时发生异常 - ID: {MessageId}", messageId);
                return false;
            }
        }

        /// <summary>
        /// 标记所有消息为已读
        /// </summary>
        /// <param name="messageType">消息类型（可选，为null则标记所有类型）</param>
        /// <returns>标记成功的消息数量</returns>
        public int MarkAllAsRead(MessageType? messageType = null)
        {
            try
            {
                var count = 0;
                foreach (var key in _localMessages.Keys.ToList())
                {
                    if (_localMessages.TryGetValue(key, out var messageData))
                    {
                        // 如果指定了消息类型，只标记该类型的消息
                        if (messageType.HasValue && messageData.MessageType != messageType.Value)
                        {
                            continue;
                        }

                        messageData.IsRead = true;
                        messageData.ReadTime = DateTime.Now;
                        _localMessages[key] = messageData; // 更新存储
                        count++;
                    }
                }
                _logger?.LogDebug("已标记所有消息为已读 - 类型: {MessageType}, 数量: {Count}", messageType, count);
                return count;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "标记所有消息为已读时发生异常");
                return 0;
            }
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="messageType">消息类型（可选，为null则获取所有类型）</param>
        /// <returns>未读消息数量</returns>
        public int GetUnreadMessageCount(MessageType? messageType = null)
        {
            return GetMessageCount(messageType, false);
        }

        /// <summary>
        /// 根据ID获取消息
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <returns>消息数据，如果未找到则返回null</returns>
        public MessageData GetMessageById(long messageId)
        {
            try
            {
                if (_localMessages.TryGetValue(messageId, out var messageData))
                {
                    return messageData;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "根据ID获取消息时发生异常 - ID: {MessageId}", messageId);
                return null;
            }
        }

        /// <summary>
        /// 删除单个消息
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <returns>是否成功删除</returns>
        public bool DeleteMessage(long messageId)
        {
            try
            {
                if (_localMessages.TryRemove(messageId, out _))
                {
                    _logger?.LogDebug("消息已从本地存储中删除 - ID: {MessageId}", messageId);
                    return true;
                }
                _logger?.LogWarning("未找到要删除的消息 - ID: {MessageId}", messageId);
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "删除消息时发生异常 - ID: {MessageId}", messageId);
                return false;
            }
        }

        /// <summary>
        /// 批量删除消息
        /// </summary>
        /// <param name="messageIds">消息ID列表</param>
        /// <returns>成功删除的消息数量</returns>
        public int DeleteMessages(IEnumerable<long> messageIds)
        {
            try
            {
                int deletedCount = 0;
                foreach (var messageId in messageIds)
                {
                    if (_localMessages.TryRemove(messageId, out _))
                    {
                        deletedCount++;
                    }
                }
                _logger?.LogDebug("已从本地存储中删除 {DeletedCount} 条消息", deletedCount);
                return deletedCount;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量删除消息时发生异常");
                return 0;
            }
        }

        /// <summary>
        /// 清除所有消息
        /// </summary>
        public void ClearAllMessages()
        {
            try
            {
                _localMessages.Clear();
                _logger?.LogDebug("已清除所有本地消息");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "清除所有消息时发生异常");
            }
        }

        #endregion

        /// <summary>
        /// 发送任务状态通知
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <param name="sendToSelf">是否发送给当前用户（用于测试或特殊情况）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>消息响应</returns>
        public async Task<MessageResponse> SendTodoNotificationAsync(
            MessageData messageData,
            bool sendToSelf = false,
            CancellationToken ct = default)
        {
            try
            {
                if (messageData == null)
                {
                    _logger?.LogWarning("任务状态通知消息数据为空");
                    return MessageResponse.Fail(MessageType.Business, "消息数据为空");
                }

                // 设置发送者信息（用于服务器端过滤）
                var request = new MessageRequest(MessageType.Business, messageData);
            

                var response = await _messageSender.Value.SendCommandWithResponseAsync<MessageResponse>(
                    MessageCommands.SendTodoNotification, request, ct);

                // 只记录关键信息和错误
                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("任务状态通知发送成功 - 业务类型: {BusinessType}", 
                        messageData.BizData?.GetType()?.Name ?? "未知");
                }
                else
                {
                    _logger?.LogWarning("任务状态通知发送失败 - 业务类型: {BusinessType}, 错误: {ErrorMessage}",
                        messageData.BizData?.GetType()?.Name ?? "未知", response?.ErrorMessage ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送任务状态通知时发生异常");
                return MessageResponse.Fail(MessageType.Business, $"发送任务状态通知失败: {ex.Message}");
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