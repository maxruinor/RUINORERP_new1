using Microsoft.Extensions.Logging;
using RUINORERP.UI.Network;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 简化版消息服务类 - 提供更简便的消息发送和接收功能
    /// 封装了复杂的消息处理逻辑，提供一键式消息发送方法
    /// </summary>
    public class SimplifiedMessageService
    {
        private readonly MessageService _messageService;
        private readonly ILogger<SimplifiedMessageService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageService">消息服务</param>
        /// <param name="logger">日志记录器</param>
        public SimplifiedMessageService(
            MessageService messageService,
            ILogger<SimplifiedMessageService> logger = null)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _logger = logger;

            // 订阅消息事件
            SubscribeToMessages();
        }

        /// <summary>
        /// 订阅消息事件
        /// </summary>
        private void SubscribeToMessages()
        {
            _messageService.PopupMessageReceived += OnPopupMessageReceived;
            _messageService.UserMessageReceived += OnUserMessageReceived;
            _messageService.DepartmentMessageReceived += OnDepartmentMessageReceived;
            _messageService.BroadcastMessageReceived += OnBroadcastMessageReceived;
            _messageService.SystemNotificationReceived += OnSystemNotificationReceived;
        }

        /// <summary>
        /// 处理接收到的弹窗消息
        /// </summary>
        private void OnPopupMessageReceived(MessageReceivedEventArgs args)
        {
            try
            {
                // 这里可以添加自定义的弹窗消息处理逻辑
                _logger?.LogDebug("接收到弹窗消息");
                PopupMessageReceived?.Invoke(args);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理弹窗消息时发生异常");
            }
        }

        /// <summary>
        /// 处理接收到的用户消息
        /// </summary>
        private void OnUserMessageReceived(MessageReceivedEventArgs args)
        {
            try
            {
                // 这里可以添加自定义的用户消息处理逻辑
                _logger?.LogDebug("接收到用户消息");
                UserMessageReceived?.Invoke(args);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理用户消息时发生异常");
            }
        }

        /// <summary>
        /// 处理接收到的部门消息
        /// </summary>
        private void OnDepartmentMessageReceived(MessageReceivedEventArgs args)
        {
            try
            {
                // 这里可以添加自定义的部门消息处理逻辑
                _logger?.LogDebug("接收到部门消息");
                DepartmentMessageReceived?.Invoke(args);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理部门消息时发生异常");
            }
        }

        /// <summary>
        /// 处理接收到的广播消息
        /// </summary>
        private void OnBroadcastMessageReceived(MessageReceivedEventArgs args)
        {
            try
            {
                // 这里可以添加自定义的广播消息处理逻辑
                _logger?.LogDebug("接收到广播消息");
                BroadcastMessageReceived?.Invoke(args);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理广播消息时发生异常");
            }
        }

        /// <summary>
        /// 处理接收到的系统通知
        /// </summary>
        private void OnSystemNotificationReceived(MessageReceivedEventArgs args)
        {
            try
            {
                // 这里可以添加自定义的系统通知处理逻辑
                _logger?.LogDebug("接收到系统通知");
                SystemNotificationReceived?.Invoke(args);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理系统通知时发生异常");
            }
        }

        #region 简化消息发送方法

        /// <summary>
        /// 发送简单文本消息给指定用户
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>是否发送成功</returns>
        public async Task<bool> SendTextMessageToUserAsync(
            string targetUserId, 
            string message, 
            CancellationToken ct = default)
        {
            try
            {
                var response = await _messageService.SendMessageToUserAsync(
                    targetUserId, message, "Text", ct);

                return response != null && response.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送文本消息给用户时发生异常 - 目标用户: {TargetUserId}", targetUserId);
                return false;
            }
        }

        /// <summary>
        /// 发送弹窗消息给指定用户
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="title">弹窗标题</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>是否发送成功</returns>
        public async Task<bool> SendPopupMessageToUserAsync(
            string targetUserId, 
            string message, 
            string title = "系统消息",
            CancellationToken ct = default)
        {
            try
            {
                var response = await _messageService.SendPopupMessageAsync(
                    targetUserId, message, title, ct);

                return response != null && response.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送弹窗消息给用户时发生异常 - 目标用户: {TargetUserId}", targetUserId);
                return false;
            }
        }

        /// <summary>
        /// 发送消息给指定部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>是否发送成功</returns>
        public async Task<bool> SendMessageToDepartmentAsync(
            string departmentId, 
            string message, 
            CancellationToken ct = default)
        {
            try
            {
                var response = await _messageService.SendMessageToDepartmentAsync(
                    departmentId, message, "Text", ct);

                return response != null && response.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送消息给部门时发生异常 - 部门: {DepartmentId}", departmentId);
                return false;
            }
        }

        /// <summary>
        /// 广播消息给所有用户
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>是否发送成功</returns>
        public async Task<bool> BroadcastMessageAsync(
            string message, 
            CancellationToken ct = default)
        {
            try
            {
                var response = await _messageService.BroadcastMessageAsync(
                    message, "Text", ct);

                return response != null && response.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "广播消息时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 发送系统通知
        /// </summary>
        /// <param name="message">通知内容</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>是否发送成功</returns>
        public async Task<bool> SendSystemNotificationAsync(
            string message, 
            CancellationToken ct = default)
        {
            try
            {
                var response = await _messageService.SendSystemNotificationAsync(
                    message, "Info", ct);

                return response != null && response.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送系统通知时发生异常");
                return false;
            }
        }

        #endregion

        #region 消息接收事件

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

        #endregion
    }
}