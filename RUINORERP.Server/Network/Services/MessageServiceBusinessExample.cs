using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 消息服务业务使用示例
    /// 展示在实际业务场景中如何选择和使用两种响应处理方式
    /// </summary>
    public class MessageServiceBusinessExample
    {
        private readonly MessageServiceUsageExample _messageServiceUsage;
        private readonly ILogger<MessageServiceBusinessExample> _logger;

        public MessageServiceBusinessExample(
            MessageServiceUsageExample messageServiceUsage,
            ILogger<MessageServiceBusinessExample> logger)
        {
            _messageServiceUsage = messageServiceUsage;
            _logger = logger;
        }

        /// <summary>
        /// 业务场景1: 用户注册成功通知
        /// 需要确保通知发送成功后再返回注册成功结果
        /// 使用同步等待方式
        /// </summary>
        public async Task<bool> HandleUserRegistrationAsync(string sessionId, string userId, string email)
        {
            try
            {
                _logger?.LogInformation("处理用户注册: UserId={UserId}, Email={Email}", userId, email);
                
                // 执行用户注册逻辑
                var registrationResult = await RegisterUserAsync(userId, email);
                if (!registrationResult)
                {
                    return false;
                }
                
                // 注册成功后发送通知，需要确保发送成功
                // 使用同步等待方式，确保通知发送成功后再返回
                var notificationResult = await _messageServiceUsage.SendPopupMessageAndWaitForResponseAsync(
                    sessionId, 
                    userId, 
                    $"欢迎注册成功！您的用户ID是: {userId}", 
                    "注册成功通知");
                
                if (notificationResult)
                {
                    _logger?.LogInformation("用户注册处理完成: UserId={UserId}", userId);
                    return true;
                }
                else
                {
                    _logger?.LogWarning("用户注册成功但通知发送失败: UserId={UserId}", userId);
                    // 可以选择回滚注册操作或记录警告
                    return true; // 注册本身成功
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理用户注册时发生异常: UserId={UserId}", userId);
                return false;
            }
        }

        /// <summary>
        /// 业务场景2: 批量用户通知
        /// 发送通知后不需要等待每个响应，可以继续执行其他操作
        /// 使用事件驱动方式
        /// </summary>
        public async Task HandleBatchUserNotificationAsync(string[] sessionIds, string[] userIds, string message)
        {
            try
            {
                _logger?.LogInformation("处理批量用户通知: UserCount={UserCount}", userIds.Length);
                
                // 并行发送通知，不需要等待每个响应
                var tasks = new Task[userIds.Length];
                for (int i = 0; i < userIds.Length; i++)
                {
                    var sessionId = sessionIds[i];
                    var userId = userIds[i];
                    
                    // 使用事件驱动方式发送通知
                    tasks[i] = _messageServiceUsage.SendPopupMessageWithEventHandlingAsync(
                        sessionId, 
                        userId, 
                        message, 
                        "系统通知");
                }
                
                // 等待所有发送请求提交完成（不是等待响应）
                // 过滤掉可能的null任务，防止ArgumentException异常
                var validTasks = tasks.Where(t => t != null).ToList();
                if (validTasks.Any())
                {
                    await Task.WhenAll(validTasks);
                }
                
                _logger?.LogInformation("批量用户通知发送请求提交完成: UserCount={UserCount}", userIds.Length);
                
                // 继续执行其他业务逻辑，响应将通过事件处理
                await ProcessOtherBusinessLogicAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理批量用户通知时发生异常");
            }
        }

        /// <summary>
        /// 业务场景3: 系统维护通知
        /// 需要发送给所有在线用户，使用ServerMessageService
        /// </summary>
        public async Task HandleSystemMaintenanceNotificationAsync(string maintenanceMessage)
        {
            try
            {
                _logger?.LogInformation("发送系统维护通知: {Message}", maintenanceMessage);
                
                // 发送系统广播通知
                await _messageServiceUsage.SendUserMessageWithEventHandlingAsync(
                    "ALL_USERS", 
                    maintenanceMessage);
                
                _logger?.LogInformation("系统维护通知发送完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送系统维护通知时发生异常");
            }
        }

        /// <summary>
        /// 业务场景4: 混合使用方式
        /// 对于重要操作使用同步等待，对于普通操作使用事件驱动
        /// </summary>
        public async Task HandleMixedScenarioAsync(string sessionId, string userId, string action)
        {
            try
            {
                _logger?.LogInformation("处理混合场景: UserId={UserId}, Action={Action}", userId, action);
                
                switch (action)
                {
                    case "CRITICAL":
                        // 重要操作，使用同步等待方式
                        var criticalResult = await _messageServiceUsage.SendPopupMessageAndWaitForResponseAsync(
                            sessionId, 
                            userId, 
                            "这是一个重要操作，请确认！", 
                            "重要操作确认");
                        
                        if (criticalResult)
                        {
                            _logger?.LogInformation("重要操作确认成功: UserId={UserId}", userId);
                            // 执行重要操作
                            await ExecuteCriticalOperationAsync(userId);
                        }
                        else
                        {
                            _logger?.LogWarning("重要操作未确认: UserId={UserId}", userId);
                        }
                        break;
                        
                    case "NORMAL":
                        // 普通操作，使用事件驱动方式
                        await _messageServiceUsage.SendPopupMessageWithEventHandlingAsync(
                            sessionId, 
                            userId, 
                            "这是一个普通操作", 
                            "普通操作通知");
                        
                        _logger?.LogInformation("普通操作通知发送完成: UserId={UserId}", userId);
                        // 继续执行其他操作
                        await ProcessOtherBusinessLogicAsync();
                        break;
                        
                    default:
                        _logger?.LogWarning("未知操作类型: Action={Action}", action);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理混合场景时发生异常: UserId={UserId}, Action={Action}", userId, action);
            }
        }

        #region 辅助业务方法

        /// <summary>
        /// 模拟用户注册逻辑
        /// </summary>
        private async Task<bool> RegisterUserAsync(string userId, string email)
        {
            try
            {
                // 模拟数据库操作
                await Task.Delay(100);
                _logger?.LogDebug("用户注册完成: UserId={UserId}, Email={Email}", userId, email);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "用户注册失败: UserId={UserId}, Email={Email}", userId, email);
                return false;
            }
        }

        /// <summary>
        /// 执行重要操作
        /// </summary>
        private async Task ExecuteCriticalOperationAsync(string userId)
        {
            try
            {
                // 模拟重要操作
                await Task.Delay(200);
                _logger?.LogDebug("重要操作执行完成: UserId={UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "重要操作执行失败: UserId={UserId}", userId);
            }
        }

        /// <summary>
        /// 执行其他业务逻辑
        /// </summary>
        private async Task ProcessOtherBusinessLogicAsync()
        {
            try
            {
                // 模拟其他业务逻辑
                await Task.Delay(150);
                _logger?.LogDebug("其他业务逻辑执行完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "其他业务逻辑执行失败");
            }
        }

        #endregion

        #region 使用建议

        /// <summary>
        /// 使用方式选择指南
        /// </summary>
        public void UsageGuidelines()
        {
            _logger?.LogInformation("=== 消息服务使用方式选择指南 ===");
            _logger?.LogInformation("");
            _logger?.LogInformation("1. 同步等待方式适用场景:");
            _logger?.LogInformation("   - 需要根据响应结果决定后续操作");
            _logger?.LogInformation("   - 重要操作需要确保执行成功");
            _logger?.LogInformation("   - 需要立即获得操作结果反馈");
            _logger?.LogInformation("   - 错误处理需要立即响应");
            _logger?.LogInformation("");
            _logger?.LogInformation("2. 事件驱动方式适用场景:");
            _logger?.LogInformation("   - 发送后不需要立即处理响应");
            _logger?.LogInformation("   - 批量操作提高性能");
            _logger?.LogInformation("   - 异步处理提高系统响应性");
            _logger?.LogInformation("   - 不影响主业务流程的操作");
            _logger?.LogInformation("");
            _logger?.LogInformation("3. 最佳实践:");
            _logger?.LogInformation("   - 重要操作使用同步等待方式");
            _logger?.LogInformation("   - 普通通知使用事件驱动方式");
            _logger?.LogInformation("   - 合理设置超时时间");
            _logger?.LogInformation("   - 完善异常处理机制");
            _logger?.LogInformation("   - 记录关键操作日志");
        }

        #endregion
    }
}