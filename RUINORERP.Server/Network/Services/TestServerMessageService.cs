using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Messaging;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 服务器消息服务测试类
    /// 用于测试服务器向客户端发送消息并等待响应的功能
    /// </summary>
    public class TestServerMessageService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TestServerMessageService> _logger;

        public TestServerMessageService(
            IServiceProvider serviceProvider,
            ILogger<TestServerMessageService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// 测试服务器消息服务功能
        /// </summary>
        public async Task TestServerMessageFunctionality()
        {
            try
            {
                _logger?.LogInformation("开始测试服务器消息服务功能");
                
                // 获取服务器消息服务
                var messageService = _serviceProvider.GetRequiredService<ServerMessageService>();
                
                // 测试发送弹窗消息
                await TestSendPopupMessage(messageService);
                
                // 测试发送文本消息
                await TestSendTextMessage(messageService);
                
                // 测试发送系统通知
                await TestSendSystemNotification(messageService);
                
                _logger?.LogInformation("服务器消息服务功能测试完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试服务器消息服务功能时发生异常");
            }
        }

        /// <summary>
        /// 测试发送弹窗消息
        /// </summary>
        private async Task TestSendPopupMessage(ServerMessageService messageService)
        {
            try
            {
                _logger?.LogInformation("测试发送弹窗消息");
                
                var response = await messageService.SendPopupMessageAsync(
                    "testUser123",
                    "测试弹窗消息内容",
                    "测试标题");
                
                LogResponseResult("弹窗消息", response);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试发送弹窗消息时发生异常");
            }
        }

        /// <summary>
        /// 测试发送文本消息
        /// </summary>
        private async Task TestSendTextMessage(ServerMessageService messageService)
        {
            try
            {
                _logger?.LogInformation("测试发送文本消息");
                
                var response = await messageService.SendMessageToUserAsync(
                    "testUser456",
                    "测试文本消息内容",
                    "Text");
                
                LogResponseResult("文本消息", response);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试发送文本消息时发生异常");
            }
        }

        /// <summary>
        /// 测试发送系统通知
        /// </summary>
        private async Task TestSendSystemNotification(ServerMessageService messageService)
        {
            try
            {
                _logger?.LogInformation("测试发送系统通知");
                
                var response = await messageService.SendSystemNotificationAsync(
                    "测试系统通知内容",
                    "Info");
                
                LogResponseResult("系统通知", response);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试发送系统通知时发生异常");
            }
        }

        /// <summary>
        /// 记录响应结果
        /// </summary>
        private void LogResponseResult(string messageType, MessageResponse response)
        {
            if (response.IsSuccess)
            {
                _logger?.LogInformation("{MessageType}发送成功: {Data}", messageType, response.Data?.ToString());
            }
            else
            {
                _logger?.LogWarning("{MessageType}发送失败: {ErrorMessage}", messageType, response.ErrorMessage);
            }
        }
    }
}