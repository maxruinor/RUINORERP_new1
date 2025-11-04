using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using RUINORERP.Server.Network.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 服务器消息服务使用示例
    /// 展示如何在服务器端使用SessionService向客户端发送消息并等待响应
    /// </summary>
    public class ServerMessageServiceExample
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<ServerMessageServiceExample> _logger;

        public ServerMessageServiceExample(
            SessionService sessionService,
            ILogger<ServerMessageServiceExample> logger)
        {
            _sessionService = sessionService;
            _logger = logger;
        }

        /// <summary>
        /// 示例：向指定用户发送弹窗消息并等待响应
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="message">消息内容</param>
        /// <param name="title">消息标题</param>
        public async Task Example_SendPopupMessageWithResponse(string sessionId, string targetUserId, string message, string title)
        {
            try
            {
                _logger?.LogInformation("开始发送弹窗消息示例");
                
                // 创建消息请求
                var messageData = new
                {
                    TargetUserId = targetUserId,
                    Message = message,
                    Title = title,
                    MessageType = "Popup"
                };

                var request = new MessageRequest(MessageType.Unknown, messageData);
                
                // 发送消息并等待响应
                var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                    sessionId, 
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
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送弹窗消息示例时发生异常");
            }
        }

        /// <summary>
        /// 示例：向指定用户发送文本消息（单向，不等待响应）
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="message">消息内容</param>
        public async Task Example_SendTextMessage(string sessionId, string targetUserId, string message)
        {
            try
            {
                _logger?.LogInformation("开始发送文本消息示例");
                
                // 创建消息请求
                var messageData = new
                {
                    TargetUserId = targetUserId,
                    Message = message,
                    MessageType = "Text"
                };

                var request = new MessageRequest(MessageType.Unknown, messageData);
                
                // 发送消息（不等待响应）
                var success = await _sessionService.SendCommandAsync(
                    sessionId, 
                    MessageCommands.SendMessageToUser, 
                    request, 
                    CancellationToken.None);
                
                if (success)
                {
                    _logger?.LogInformation("文本消息发送成功");
                }
                else
                {
                    _logger?.LogWarning("文本消息发送失败");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送文本消息示例时发生异常");
            }
        }

        /// <summary>
        /// 示例：广播消息给所有用户
        /// </summary>
        /// <param name="message">消息内容</param>
        public async Task Example_BroadcastMessage(string message)
        {
            try
            {
                _logger?.LogInformation("开始广播消息示例");
                
                // 获取所有用户会话
                var sessions = _sessionService.GetAllUserSessions();
                
                int successCount = 0;
                foreach (var session in sessions)
                {
                    if (session is SessionInfo sessionInfo)
                    {
                        // 创建消息请求
                        var messageData = new
                        {
                            Message = message,
                            MessageType = "Broadcast"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        
                        // 发送消息（不等待响应）
                        var success = await _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.BroadcastMessage, 
                            request, 
                            CancellationToken.None);
                        
                        if (success)
                        {
                            successCount++;
                        }
                    }
                }
                
                _logger?.LogInformation("广播消息发送完成，成功发送给 {SuccessCount} 个会话", successCount);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "广播消息示例时发生异常");
            }
        }
    }
}