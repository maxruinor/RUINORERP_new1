using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 服务器消息服务使用示例
    /// 展示如何在服务器端使用ServerMessageService向客户端发送消息并等待响应
    /// </summary>
    public class ServerMessageServiceUsageExample
    {
        private readonly ServerMessageService _messageService;
        private readonly ILogger<ServerMessageServiceUsageExample> _logger;

        public ServerMessageServiceUsageExample(
            ServerMessageService messageService,
            ILogger<ServerMessageServiceUsageExample> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        /// <summary>
        /// 示例：向指定用户发送弹窗消息
        /// </summary>
        public async Task Example_SendPopupMessage()
        {
            try
            {
                _logger?.LogInformation("开始发送弹窗消息示例");
                
                // 发送弹窗消息给用户"user123"
                var response = await _messageService.SendPopupMessageAsync(
                    "user123", 
                    "这是一条测试弹窗消息", 
                    "系统通知",
                    30000, // 30秒超时
                    CancellationToken.None);
                
                if (response.IsSuccess)
                {
                    _logger?.LogInformation("弹窗消息发送成功: {Data}", response.Data?.ToString());
                }
                else
                {
                    _logger?.LogWarning("弹窗消息发送失败: {ErrorMessage}", response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送弹窗消息示例时发生异常");
            }
        }

        /// <summary>
        /// 示例：向指定用户发送文本消息
        /// </summary>
        public async Task Example_SendTextMessage()
        {
            try
            {
                _logger?.LogInformation("开始发送文本消息示例");
                
                // 发送文本消息给用户"user456"
                var response = await _messageService.SendMessageToUserAsync(
                    "user456", 
                    "这是一条测试文本消息", 
                    "Text",
                    30000, // 30秒超时
                    CancellationToken.None);
                
                if (response.IsSuccess)
                {
                    _logger?.LogInformation("文本消息发送成功: {Data}", response.Data?.ToString());
                }
                else
                {
                    _logger?.LogWarning("文本消息发送失败: {ErrorMessage}", response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送文本消息示例时发生异常");
            }
        }

        /// <summary>
        /// 示例：向部门发送消息
        /// </summary>
        public async Task Example_SendDepartmentMessage()
        {
            try
            {
                _logger?.LogInformation("开始发送部门消息示例");
                
                // 发送消息给部门"dept001"
                var response = await _messageService.SendMessageToDepartmentAsync(
                    "dept001", 
                    "这是一条部门通知消息", 
                    "Notification",
                    30000, // 30秒超时
                    CancellationToken.None);
                
                if (response.IsSuccess)
                {
                    _logger?.LogInformation("部门消息发送成功: {Data}", response.Data?.ToString());
                }
                else
                {
                    _logger?.LogWarning("部门消息发送失败: {ErrorMessage}", response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送部门消息示例时发生异常");
            }
        }

        /// <summary>
        /// 示例：广播消息给所有用户
        /// </summary>
        public async Task Example_BroadcastMessage()
        {
            try
            {
                _logger?.LogInformation("开始广播消息示例");
                
                // 广播消息给所有用户
                var response = await _messageService.BroadcastMessageAsync(
                    "这是一条系统广播消息", 
                    "Broadcast",
                    30000, // 30秒超时
                    CancellationToken.None);
                
                if (response.IsSuccess)
                {
                    _logger?.LogInformation("广播消息发送成功: {Data}", response.Data?.ToString());
                }
                else
                {
                    _logger?.LogWarning("广播消息发送失败: {ErrorMessage}", response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "广播消息示例时发生异常");
            }
        }

        /// <summary>
        /// 示例：发送系统通知
        /// </summary>
        public async Task Example_SendSystemNotification()
        {
            try
            {
                _logger?.LogInformation("开始发送系统通知示例");
                
                // 发送系统通知
                var response = await _messageService.SendSystemNotificationAsync(
                    "系统维护通知：服务器将在今晚12点进行维护", 
                    "Warning",
                    30000, // 30秒超时
                    CancellationToken.None);
                
                if (response.IsSuccess)
                {
                    _logger?.LogInformation("系统通知发送成功: {Data}", response.Data?.ToString());
                }
                else
                {
                    _logger?.LogWarning("系统通知发送失败: {ErrorMessage}", response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送系统通知示例时发生异常");
            }
        }

        /// <summary>
        /// 综合示例：演示多种消息发送方式
        /// </summary>
        public async Task ComprehensiveExample()
        {
            _logger?.LogInformation("开始综合示例");
            
            // 1. 发送弹窗消息
            await Example_SendPopupMessage();
            
            // 2. 发送文本消息
            await Example_SendTextMessage();
            
            // 3. 发送部门消息
            await Example_SendDepartmentMessage();
            
            // 4. 广播消息
            await Example_BroadcastMessage();
            
            // 5. 发送系统通知
            await Example_SendSystemNotification();
            
            _logger?.LogInformation("综合示例完成");
        }
    }
}