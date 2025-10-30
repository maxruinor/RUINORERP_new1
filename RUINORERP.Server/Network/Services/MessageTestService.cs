using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 消息测试服务，用于演示和测试服务器向客户端发送消息的功能
    /// </summary>
    public class MessageTestService
    {
        private readonly ServerMessageService _messageService;
        private readonly ILogger<MessageTestService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageService">服务器消息服务</param>
        /// <param name="logger">日志记录器</param>
        public MessageTestService(ServerMessageService messageService, ILogger<MessageTestService> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        /// <summary>
        /// 向指定用户发送弹窗消息（测试用）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="title">弹窗标题</param>
        /// <param name="message">弹窗内容</param>
        /// <returns>任务对象</returns>
        public async Task SendTestPopupMessage(string username, string title = "测试弹窗消息", string message = "这是一条来自服务器的测试弹窗消息")
        {
            try
            {
                // 发送消息 - 直接传递正确的参数类型
                await _messageService.SendPopupMessageAsync(username, message, title);
                _logger.LogInformation($"已向用户 {username} 发送测试弹窗消息");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"向用户 {username} 发送测试弹窗消息失败");
                throw;
            }
        }

        /// <summary>
        /// 向指定用户发送普通消息（测试用）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="message">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>任务对象</returns>
        public async Task SendTestUserMessage(string username, string message = "这是一条来自服务器的测试用户消息", string messageType = "Text")
        {
            try
            {
                // 发送消息 - 直接传递正确的参数类型
                await _messageService.SendMessageToUserAsync(username, message, messageType);
                _logger.LogInformation($"已向用户 {username} 发送测试用户消息");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"向用户 {username} 发送测试用户消息失败");
                throw;
            }
        }

        /// <summary>
        /// 向指定用户发送系统通知（测试用）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="message">通知内容</param>
        /// <param name="notificationType">通知类型</param>
        /// <returns>任务对象</returns>
        public async Task SendTestSystemNotification(string username, string message = "这是一条来自服务器的测试系统通知", string notificationType = "Info")
        {
            try
            {
                // 发送通知 - 系统通知不需要指定用户名，直接传递消息内容和类型
                await _messageService.SendSystemNotificationAsync(message, notificationType);
                _logger.LogInformation($"已发送测试系统通知");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "发送测试系统通知失败");
                throw;
            }
        }

        /// <summary>
        /// 向所有在线用户广播消息（测试用）
        /// </summary>
        /// <param name="message">广播内容</param>
        /// <returns>任务对象</returns>
        public async Task BroadcastTestMessage(string message = "这是一条来自服务器的测试广播消息")
        {
            try
            {
                // 广播消息 - 直接传递正确的参数类型
                await _messageService.BroadcastMessageAsync(message, "Broadcast");
                _logger.LogInformation($"已向所有在线用户发送测试广播消息");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "发送测试广播消息失败");
                throw;
            }
        }
    }
}