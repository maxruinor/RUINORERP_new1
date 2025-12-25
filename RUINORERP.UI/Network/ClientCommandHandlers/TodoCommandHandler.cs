using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.UserCenter.DataParts;
using System;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 任务状态命令处理器
    /// 统一处理来自服务器的任务状态变更通知
    /// 作为接收服务器状态通知的唯一入口点
    /// </summary>
    [ClientCommandHandler("TodoCommandHandler", 60)]
    public class TodoCommandHandler : BaseClientCommandHandler
    {
        private readonly ILogger<TodoCommandHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public TodoCommandHandler(ILogger<TodoCommandHandler> logger = null) :
            base(logger ?? Startup.GetFromFac<ILogger<BaseClientCommandHandler>>())
        {
            _logger = logger ?? Startup.GetFromFac<ILogger<TodoCommandHandler>>();

            // 设置支持的命令列表，整合原有三个类支持的所有命令
            SetSupportedCommands(
                MessageCommands.SendTodoNotification
            );

            _logger.LogDebug("TodoCommandHandler已初始化，整合了所有任务状态通知处理功能");
        }

        /// <summary>
        /// 处理接收到的任务状态变更命令
        /// </summary>
        /// <param name="packet">数据包</param>
        public override async Task HandleAsync(PacketModel packet)
        {
            if (packet == null || packet.CommandId == null)
            {
                _logger.LogError("收到无效的数据包");
                return;
            }

            try
            {
                // 从不同类型的请求中提取任务状态更新数据
                TodoUpdate update = null;
                MessageData messageData = null;

                // 处理MessageRequest类型的请求
                if (packet.Request is MessageRequest messageRequest &&
                    messageRequest.Data is MessageData msgData)
                {
                    messageData = msgData;

                    // 尝试从不同属性获取更新数据，确保兼容性
                    if (msgData.BizData is TodoUpdate bizDataUpdate)
                    {
                        update = bizDataUpdate;
                    }

                }


                // 如果成功提取到更新数据，则进行处理
                if (update != null)
                {
                    // 调用核心处理方法
                    ProcessTaskStatusChangeAsync(update, messageData);
                }
                else
                {
                    _logger.LogWarning($"无法从数据包中提取有效的TodoUpdate数据，命令ID: {packet.CommandId.FullCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理任务状态变更通知时发生异常，命令ID: {packet.CommandId.FullCode}");
            }
        }

        /// <summary>
        /// 处理任务状态变更的核心方法
        /// </summary>
        /// <param name="update">任务状态更新信息</param>
        /// <param name="messageData">原始消息数据（可选）</param>
        private void ProcessTaskStatusChangeAsync(TodoUpdate update, MessageData messageData = null)
        {
            try
            {
                // 标记为来自服务器的更新
                update.IsFromServer = true;

                // 业务逻辑处理：根据不同业务类型执行特定处理
                ProcessBusinessSpecificLogic(update);

                // 通知本地任务状态同步管理器
                TodoSyncManager.Instance.PublishUpdate(update);

                // 同时添加到消息管理器中
                AddToMessageManager(update, messageData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理任务状态变更核心逻辑时发生异常");
                throw; // 重新抛出异常以便外层捕获和记录
            }
        }

        /// <summary>
        /// 处理业务特定逻辑
        /// 根据不同的业务类型执行特定的处理逻辑
        /// </summary>
        /// <param name="update">任务状态更新信息</param>
        private void ProcessBusinessSpecificLogic(TodoUpdate update)
        {
            try
            {
                // 根据业务类型执行特定逻辑
                switch (update.BusinessType)
                {
                    default:
                        // 默认处理逻辑
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理业务特定逻辑时发生异常");
                // 这里不抛出异常，因为业务特定逻辑失败不应影响主要功能
            }
        }

        /// <summary>
        /// 将任务状态变更添加到消息管理器
        /// </summary>
        /// <param name="update">任务状态更新信息</param>
        /// <param name="originalMessageData">原始消息数据（可选）</param>
        private void AddToMessageManager(TodoUpdate update, MessageData originalMessageData = null)
        {
            try
            {
                var messageService = Startup.GetFromFac<MessageService>();
                if (messageService != null)
                {
                    // 创建消息数据
                    var message = originalMessageData != null ?
                        new MessageData
                        {
                            MessageId = originalMessageData.MessageId,
                            MessageType = MessageType.Business,
                            Title = originalMessageData.Title,
                            Content = originalMessageData.Content,
                            BizType = update.BusinessType,
                            BizId = update.BillId,
                            BizData = update,
                            SendTime = originalMessageData.SendTime,
                            IsRead = false
                        } :
                        new MessageData
                        {
                            MessageId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId(),
                            MessageType = MessageType.Business,
                            Title = "任务状态变更",
                            Content = $"[{update.BusinessType}]{update.OperationDescription}",
                            BizType = update.BusinessType,
                            BizId = update.BillId,
                            BizData = update,
                            SendTime = DateTime.Now,
                            IsRead = false
                        };

                    // 添加到消息管理器
                    messageService.OnBusinessMessageReceived(message);
                    _logger.LogDebug("已将任务状态变更通知添加到消息中心");
                }
                else
                {
                    _logger.LogWarning("无法获取MessageService实例，跳过消息添加");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "将任务状态变更通知添加到消息中心时发生异常");
                // 这里不抛出异常，因为消息管理器失败不应影响主要功能
            }
        }
    }
}