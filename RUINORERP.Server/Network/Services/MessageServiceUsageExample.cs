using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 消息服务使用示例
    /// 展示如何在业务类中使用SessionService的两种响应处理方式
    /// </summary>
    public class MessageServiceUsageExample
    {
        private readonly SessionService _sessionService;
        private readonly ServerMessageService _serverMessageService;
        private readonly ILogger<MessageServiceUsageExample> _logger;

        public MessageServiceUsageExample(
            SessionService sessionService,
            ServerMessageService serverMessageService,
            ILogger<MessageServiceUsageExample> logger)
        {
            _sessionService = sessionService;
            _serverMessageService = serverMessageService;
            _logger = logger;

            // 订阅事件驱动的响应处理
            SubscribeToEvents();
        }

        /// <summary>
        /// 订阅事件驱动的响应处理
        /// </summary>
        private void SubscribeToEvents()
        {
            // 订阅通用消息响应事件
            _sessionService.MessageResponseReceived += OnMessageResponseReceived;
            
            // 订阅特定类型的消息响应事件
            _sessionService.PopupMessageResponseReceived += OnPopupMessageResponseReceived;
            _sessionService.UserMessageResponseReceived += OnUserMessageResponseReceived;
            _sessionService.DepartmentMessageResponseReceived += OnDepartmentMessageResponseReceived;
            _sessionService.BroadcastMessageResponseReceived += OnBroadcastMessageResponseReceived;
            _sessionService.SystemNotificationResponseReceived += OnSystemNotificationResponseReceived;
        }

        #region 同步等待方式示例

        /// <summary>
        /// 示例1: 使用同步等待方式发送弹窗消息
        /// 适用于需要根据响应结果立即执行后续操作的场景
        /// </summary>
        public async Task<bool> SendPopupMessageAndWaitForResponseAsync(string sessionId, string userId, string message, string title)
        {
            try
            {
                _logger?.LogInformation("开始使用同步等待方式发送弹窗消息");
                
                // 构造消息请求
                var messageData = new
                {
                    TargetUserId = userId,
                    Message = message,
                    Title = title,
                    MessageType = "Popup"
                };

                var request = new MessageRequest(MessageType.Unknown, messageData);
                
                // 发送消息并等待响应 - 同步等待方式
                var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                    sessionId, 
                    MessageCommands.SendPopupMessage, 
                    request, 
                    30000, // 30秒超时
                    CancellationToken.None);
                
                // 直接在业务类中处理响应
                if (responsePacket?.Response is MessageResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _logger?.LogInformation("弹窗消息发送成功: {Data}", response.Data?.ToString());
                        
                        // 执行业务逻辑
                        await ProcessSuccessfulPopupMessage(userId, message);
                        
                        return true;
                    }
                    else
                    {
                        _logger?.LogWarning("弹窗消息发送失败: {ErrorMessage}", response.ErrorMessage);
                        
                        // 执行错误处理逻辑
                        await ProcessFailedPopupMessage(userId, message, response.ErrorMessage);
                        
                        return false;
                    }
                }
                else
                {
                    _logger?.LogWarning("未收到有效响应");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送弹窗消息时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 示例2: 使用同步等待方式发送用户消息
        /// </summary>
        public async Task<bool> SendMessageToUserAndWaitForResponseAsync(string sessionId, string userId, string message)
        {
            try
            {
                _logger?.LogInformation("开始使用同步等待方式发送用户消息");
                
                // 构造消息请求
                var messageData = new
                {
                    TargetUserId = userId,
                    Message = message,
                    MessageType = "Text"
                };

                var request = new MessageRequest(MessageType.Unknown, messageData);
                
                // 发送消息并等待响应
                var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                    sessionId, 
                    MessageCommands.SendMessageToUser, 
                    request, 
                    30000, // 30秒超时
                    CancellationToken.None);
                
                // 直接在业务类中处理响应
                if (responsePacket?.Response is MessageResponse response)
                {
                    if (response.IsSuccess)
                    {
                        _logger?.LogInformation("用户消息发送成功: {Data}", response.Data?.ToString());
                        return true;
                    }
                    else
                    {
                        _logger?.LogWarning("用户消息发送失败: {ErrorMessage}", response.ErrorMessage);
                        return false;
                    }
                }
                else
                {
                    _logger?.LogWarning("未收到有效响应");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送用户消息时发生异常");
                return false;
            }
        }

        #endregion

        #region 事件驱动方式示例

        /// <summary>
        /// 示例3: 使用事件驱动方式发送弹窗消息
        /// 适用于发送后不需要立即处理响应的场景
        /// </summary>
        public async Task SendPopupMessageWithEventHandlingAsync(string sessionId, string userId, string message, string title)
        {
            try
            {
                _logger?.LogInformation("开始使用事件驱动方式发送弹窗消息");
                
                // 构造消息请求
                var messageData = new
                {
                    TargetUserId = userId,
                    Message = message,
                    Title = title,
                    MessageType = "Popup"
                };

                var request = new MessageRequest(MessageType.Unknown, messageData);
                
                // 发送消息（不等待响应，通过事件处理）
                var success = await _sessionService.SendCommandAsync(
                    sessionId, 
                    MessageCommands.SendPopupMessage, 
                    request, 
                    CancellationToken.None);
                
                if (success)
                {
                    _logger?.LogInformation("弹窗消息发送请求已提交，等待客户端响应");
                }
                else
                {
                    _logger?.LogWarning("弹窗消息发送请求提交失败");
                }
                
                // 响应将通过事件处理，不需要在这里等待
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送弹窗消息时发生异常");
            }
        }

        /// <summary>
        /// 示例4: 使用ServerMessageService的事件驱动方式
        /// </summary>
        public async Task SendUserMessageWithEventHandlingAsync(string userId, string message)
        {
            try
            {
                _logger?.LogInformation("开始使用ServerMessageService发送用户消息");
                
                // 使用ServerMessageService发送消息（不等待响应）
                // 响应将通过事件处理
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var response = await _serverMessageService.SendMessageToUserAsync(
                            userId, 
                            message, 
                            "Text",
                            30000,
                            CancellationToken.None);
                        
                        _logger?.LogInformation("用户消息发送完成: Success={IsSuccess}", response.IsSuccess);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "ServerMessageService发送用户消息时发生异常");
                    }
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送用户消息时发生异常");
            }
        }

        #endregion

        #region 事件处理方法

        /// <summary>
        /// 处理通用消息响应事件
        /// </summary>
        private async void OnMessageResponseReceived(object sender, MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("收到通用消息响应: CommandId={CommandId}, Success={IsSuccess}", 
                    e.CommandId.Name, e.IsSuccess);
                
                // 异步处理响应
                await ProcessMessageResponseAsync(e);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理通用消息响应事件时发生异常");
            }
        }

        /// <summary>
        /// 处理弹窗消息响应事件
        /// </summary>
        private async void OnPopupMessageResponseReceived(object sender, MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("收到弹窗消息响应: SessionId={SessionId}, Success={IsSuccess}", 
                    e.SessionId, e.IsSuccess);
                
                if (e.ResponseData is MessageResponse response && response.IsSuccess)
                {
                    // 处理成功响应
                    await ProcessSuccessfulPopupMessageResponse(e);
                }
                else
                {
                    // 处理失败响应
                    await ProcessFailedPopupMessageResponse(e);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理弹窗消息响应事件时发生异常");
            }
        }

        /// <summary>
        /// 处理用户消息响应事件
        /// </summary>
        private async void OnUserMessageResponseReceived(object sender, MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("收到用户消息响应: SessionId={SessionId}, Success={IsSuccess}", 
                    e.SessionId, e.IsSuccess);
                
                // 异步处理响应
                await ProcessUserMessageResponseAsync(e);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理用户消息响应事件时发生异常");
            }
        }

        /// <summary>
        /// 处理部门消息响应事件
        /// </summary>
        private void OnDepartmentMessageResponseReceived(object sender, MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("收到部门消息响应: SessionId={SessionId}, Success={IsSuccess}", 
                    e.SessionId, e.IsSuccess);
                
                // 可以在这里处理部门消息响应
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理部门消息响应事件时发生异常");
            }
        }

        /// <summary>
        /// 处理广播消息响应事件
        /// </summary>
        private void OnBroadcastMessageResponseReceived(object sender, MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("收到广播消息响应: SessionId={SessionId}, Success={IsSuccess}", 
                    e.SessionId, e.IsSuccess);
                
                // 可以在这里处理广播消息响应
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理广播消息响应事件时发生异常");
            }
        }

        /// <summary>
        /// 处理系统通知响应事件
        /// </summary>
        private void OnSystemNotificationResponseReceived(object sender, MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("收到系统通知响应: SessionId={SessionId}, Success={IsSuccess}", 
                    e.SessionId, e.IsSuccess);
                
                // 可以在这里处理系统通知响应
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理系统通知响应事件时发生异常");
            }
        }

        #endregion

        #region 业务逻辑处理方法

        /// <summary>
        /// 处理成功的弹窗消息
        /// </summary>
        private async Task ProcessSuccessfulPopupMessage(string userId, string message)
        {
            try
            {
                // 执行业务逻辑
                _logger?.LogInformation("处理成功的弹窗消息: UserId={UserId}", userId);
                
                // 例如：更新数据库、记录日志、触发其他业务流程等
                await UpdateNotificationRecordAsync(userId, message, true);
                await LogNotificationAsync(userId, message, "Success");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理成功的弹窗消息时发生异常: UserId={UserId}", userId);
            }
        }

        /// <summary>
        /// 处理失败的弹窗消息
        /// </summary>
        private async Task ProcessFailedPopupMessage(string userId, string message, string errorMessage)
        {
            try
            {
                // 执行错误处理逻辑
                _logger?.LogInformation("处理失败的弹窗消息: UserId={UserId}, Error={ErrorMessage}", 
                    userId, errorMessage);
                
                // 例如：记录错误日志、触发重试机制等
                await LogNotificationAsync(userId, message, "Failed", errorMessage);
                await HandleNotificationFailureAsync(userId, message, errorMessage);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理失败的弹窗消息时发生异常: UserId={UserId}", userId);
            }
        }

        /// <summary>
        /// 处理成功的弹窗消息响应事件
        /// </summary>
        private async Task ProcessSuccessfulPopupMessageResponse(MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("异步处理成功的弹窗消息响应: SessionId={SessionId}", e.SessionId);
                
                // 执行异步业务逻辑
                // 例如：更新数据库、记录日志等
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "异步处理成功的弹窗消息响应时发生异常: SessionId={SessionId}", e.SessionId);
            }
        }

        /// <summary>
        /// 处理失败的弹窗消息响应事件
        /// </summary>
        private async Task ProcessFailedPopupMessageResponse(MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("异步处理失败的弹窗消息响应: SessionId={SessionId}, Error={ErrorMessage}", 
                    e.SessionId, e.ErrorMessage);
                
                // 执行异步错误处理逻辑
                // 例如：记录错误日志、触发告警等
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "异步处理失败的弹窗消息响应时发生异常: SessionId={SessionId}", e.SessionId);
            }
        }

        /// <summary>
        /// 处理消息响应
        /// </summary>
        private async Task ProcessMessageResponseAsync(MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("异步处理消息响应: CommandId={CommandId}", e.CommandId.Name);
                
                // 根据命令类型执行不同的业务逻辑
                switch (e.CommandId.FullCode)
                {
                    case var code when code == MessageCommands.SendPopupMessage.FullCode:
                        await ProcessPopupMessageResponseAsync(e);
                        break;
                    case var code when code == MessageCommands.SendMessageToUser.FullCode:
                        await ProcessUserMessageResponseAsync(e);
                        break;
                    // 可以添加更多命令类型的处理
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "异步处理消息响应时发生异常");
            }
        }

        /// <summary>
        /// 处理弹窗消息响应
        /// </summary>
        private async Task ProcessPopupMessageResponseAsync(MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("处理弹窗消息响应");
                // 执行弹窗消息响应的业务逻辑
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理弹窗消息响应时发生异常");
            }
        }

        /// <summary>
        /// 处理用户消息响应
        /// </summary>
        private async Task ProcessUserMessageResponseAsync(MessageResponseEventArgs e)
        {
            try
            {
                _logger?.LogInformation("处理用户消息响应");
                // 执行用户消息响应的业务逻辑
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理用户消息响应时发生异常");
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 更新通知记录
        /// </summary>
        private async Task UpdateNotificationRecordAsync(string userId, string message, bool success)
        {
            // 模拟数据库更新操作
            await Task.Delay(100);
            _logger?.LogDebug("更新通知记录: UserId={UserId}, Success={Success}", userId, success);
        }

        /// <summary>
        /// 记录通知日志
        /// </summary>
        private async Task LogNotificationAsync(string userId, string message, string status, string error = null)
        {
            // 模拟日志记录操作
            await Task.Delay(50);
            _logger?.LogDebug("记录通知日志: UserId={UserId}, Status={Status}, Error={Error}", userId, status, error);
        }

        /// <summary>
        /// 处理通知失败
        /// </summary>
        private async Task HandleNotificationFailureAsync(string userId, string message, string errorMessage)
        {
            // 模拟失败处理操作
            await Task.Delay(100);
            _logger?.LogDebug("处理通知失败: UserId={UserId}, Error={ErrorMessage}", userId, errorMessage);
        }

        #endregion

        #region 综合示例

        /// <summary>
        /// 综合示例：演示两种方式的使用场景
        /// </summary>
        public async Task ComprehensiveExampleAsync(string sessionId, string userId)
        {
            _logger?.LogInformation("开始综合示例");
            
            // 场景1: 需要立即知道发送结果的操作 - 使用同步等待方式
            var popupResult = await SendPopupMessageAndWaitForResponseAsync(
                sessionId, userId, "这是一条重要通知", "重要通知");
            
            if (popupResult)
            {
                _logger?.LogInformation("重要通知发送成功，执行后续操作");
                // 可以根据成功结果执行后续业务逻辑
            }
            else
            {
                _logger?.LogWarning("重要通知发送失败，执行失败处理");
                // 可以执行失败处理逻辑
            }
            
            // 场景2: 不需要立即知道发送结果的操作 - 使用事件驱动方式
            await SendPopupMessageWithEventHandlingAsync(
                sessionId, userId, "这是一条普通通知", "普通通知");
            
            _logger?.LogInformation("普通通知发送请求已提交，继续执行其他操作");
            // 可以继续执行其他操作，响应将通过事件处理
        }

        #endregion
    }
}