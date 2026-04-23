using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 任务状态通知服务 - 服务器广播中心
    /// 单一职责：负责将任务状态变更事件广播给所有连接的客户端
    /// 作为服务器端的消息分发枢纽，确保状态更新能实时传递给所有相关用户
    /// </summary>
    public class TodoNotificationService
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<TodoNotificationService> _logger;

        public TodoNotificationService(
            SessionService sessionService,
            ILogger<TodoNotificationService> logger = null)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _logger = logger;
        }

        /// <summary>
        /// 向相关用户推送任务状态变更通知
        /// </summary>
        /// <param name="update">任务状态更新信息</param>
        public async Task NotifyTodoChangeAsync(TodoUpdate update, MessageData messageData)
        {
            try
            {
                if (update == null)
                {
                    _logger?.LogWarning("任务状态更新信息为空，无法发送通知");
                    return;
                }

                // 1. 严格校验：检查是否有意外的大对象残留
                if (update.Entity != null)
                {
                    _logger?.LogWarning("检测到 TodoUpdate 中包含未清理的实体对象 [Type: {EntityType}]，已强制清空以防止 OOM", 
                        update.Entity.GetType().FullName);
                    update.Entity = null; // 强制清空，确保网络传输安全
                }

                // 2. 构造推送消息
                messageData.SendTime = System.DateTime.Now;
                var request = new MessageRequest(MessageType.Business, messageData);

                // 3. 序列化前的大小监控与熔断保护
                byte[] payload;
                try
                {
                    payload = RUINORERP.PacketSpec.Serialization.JsonCompressionSerializationService.Serialize(request);
                }
                catch (System.OutOfMemoryException ex)
                {
                    _logger?.LogError(ex, "待办通知序列化发生内存溢出 - 业务类型: {BizType}, 单据ID: {BillId}", 
                        update.BusinessType, update.BillId);
                    return; // 直接返回，不再尝试广播
                }

                if (payload.Length > 1024 * 50) // 超过 50KB 视为严重异常
                {
                    _logger?.LogError("待办通知数据包严重超标: {Size} bytes, 业务类型: {BizType}, 单据ID: {BillId}。已取消广播。", 
                        payload.Length, update.BusinessType, update.BillId);
                    return;
                }
                else if (payload.Length > 1024 * 10) // 超过 10KB 警告
                {
                    _logger?.LogWarning("待办通知数据包偏大: {Size} bytes, 业务类型: {BizType}, 单据ID: {BillId}", 
                        payload.Length, update.BusinessType, update.BillId);
                }

                // 4. 获取所有在线用户会话并广播（并行发送优化）
                var sessions = _sessionService.GetAllUserSessions();
                var targetSessions = sessions
                    .OfType<SessionInfo>()
                    .Where(s => 
                        s.IsConnected && // ✅ 只发送给已连接的会话
                        (s.UserId != messageData.SenderId || messageData.SendToSelf))
                    .ToList();

                if (targetSessions.Count > 0)
                {
                    var sendTasks = targetSessions.Select(session => 
                        _sessionService.SendPacketCoreAsync(session, MessageCommands.SendTodoNotification, request, 5000));
                    
                    try
                    {
                        await Task.WhenAll(sendTasks);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "部分会话通知发送失败");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "推送任务状态变更通知时发生异常");
            }
        }
    }
}