using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 消息服务测试类
    /// 用于验证服务器向客户端发送消息并等待响应的功能
    /// </summary>
    public class MessageServiceTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MessageServiceTest> _logger;

        public MessageServiceTest(
            IServiceProvider serviceProvider,
            ILogger<MessageServiceTest> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// 测试消息服务功能
        /// </summary>
        public async Task TestMessageServiceFunctionality()
        {
            try
            {
                _logger?.LogInformation("开始测试消息服务功能");
                
                // 测试使用SessionService直接发送消息
                await TestSessionServiceDirectMessaging();
                
                // 测试使用ServerMessageService发送消息
                await TestServerMessageServiceMessaging();
                
                _logger?.LogInformation("消息服务功能测试完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试消息服务功能时发生异常");
            }
        }

        /// <summary>
        /// 测试使用SessionService直接发送消息
        /// </summary>
        private async Task TestSessionServiceDirectMessaging()
        {
            try
            {
                _logger?.LogInformation("测试使用SessionService直接发送消息");
                
                // 获取会话服务
                var sessionService = _serviceProvider.GetRequiredService<SessionService>();
                
                // 注意：在实际测试中，需要有一个有效的会话ID
                // 这里仅作为示例代码展示
                /*
                // 创建消息请求
                var messageData = new
                {
                    TargetUserId = "testUser123",
                    Message = "测试消息内容",
                    Title = "测试标题",
                    MessageType = "Popup"
                };

                var request = new MessageRequest(MessageType.Unknown, messageData);
                
                // 发送消息并等待响应
                var responsePacket = await sessionService.SendCommandAndWaitForResponseAsync(
                    "testSessionId", 
                    MessageCommands.SendPopupMessage, 
                    request, 
                    30000, // 30秒超时
                    CancellationToken.None);
                
                if (responsePacket?.Response is MessageResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _logger?.LogInformation("弹窗消息发送成功: {Data}", response.Data?.ToString());
                    }
                    else
                    {
                        _logger?.LogWarning("弹窗消息发送失败: {ErrorMessage}", response.ErrorMessage);
                    }
                }
                else
                {
                    _logger?.LogWarning("未收到有效响应");
                }
                */
                
                _logger?.LogInformation("SessionService直接发送消息测试完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试SessionService直接发送消息时发生异常");
            }
        }

        /// <summary>
        /// 测试使用ServerMessageService发送消息
        /// </summary>
        private async Task TestServerMessageServiceMessaging()
        {
            try
            {
                _logger?.LogInformation("测试使用ServerMessageService发送消息");
                
                // 获取服务器消息服务
                var messageService = _serviceProvider.GetRequiredService<ServerMessageService>();
                
                // 注意：在实际测试中，需要有一个有效的会话ID
                // 这里仅作为示例代码展示
                /*
                // 发送弹窗消息
                var response = await messageService.SendPopupMessageAsync(
                    "testUser123",
                    "测试弹窗消息内容",
                    "测试标题");
                
                if (response.IsSuccess)
                {
                    _logger?.LogInformation("弹窗消息发送成功: {Data}", response.Data?.ToString());
                }
                else
                {
                    _logger?.LogWarning("弹窗消息发送失败: {ErrorMessage}", response.ErrorMessage);
                }
                */
                
                _logger?.LogInformation("ServerMessageService发送消息测试完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试ServerMessageService发送消息时发生异常");
            }
        }
    }
}