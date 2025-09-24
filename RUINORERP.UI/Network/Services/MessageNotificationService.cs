using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 消息提示服务
    /// 处理客户端接收和发送消息提示功能
    /// </summary>
    public class MessageNotificationService
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly ClientCommandDispatcher _commandDispatcher;

        /// <summary>
        /// 当接收到弹窗消息时触发的事件
        /// </summary>
        public event Action<string, string> PopupMessageReceived;

        /// <summary>
        /// 当接收到系统通知时触发的事件
        /// </summary>
        public event Action<string, string> SystemNotificationReceived;

        /// <summary>
        /// 当接收到广播消息时触发的事件
        /// </summary>
        public event Action<string, string> BroadcastMessageReceived;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="commandDispatcher">命令调度器</param>
        public MessageNotificationService(
            IClientCommunicationService communicationService,
            ClientCommandDispatcher commandDispatcher)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));

            // 订阅通信服务的命令接收事件
            _communicationService.CommandReceived += OnCommandReceived;
        }

        /// <summary>
        /// 处理接收到的命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        private void OnCommandReceived(CommandId commandId, object data)
        {
            try
            {
                // 根据命令类型处理不同的消息
                if (commandId.Category == CommandCategory.Message)
                {
                    switch (commandId.OperationCode)
                    {
                        case 0x00: // SendPopupMessage
                            HandlePopupMessage(data);
                            break;
                        case 0x07: // SendSystemNotification
                            HandleSystemNotification(data);
                            break;
                        case 0x06: // BroadcastMessage
                            HandleBroadcastMessage(data);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常但不中断其他消息处理
                Console.WriteLine($"处理消息命令时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理弹窗消息
        /// </summary>
        /// <param name="data">消息数据</param>
        private void HandlePopupMessage(object data)
        {
            try
            {
                // 解析消息数据
                var messageData = data as dynamic;
                var content = messageData?.Content?.ToString() ?? "";
                var sender = messageData?.Sender?.ToString() ?? "系统";

                // 触发弹窗消息事件
                PopupMessageReceived?.Invoke(sender, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理弹窗消息时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理系统通知
        /// </summary>
        /// <param name="data">通知数据</param>
        private void HandleSystemNotification(object data)
        {
            try
            {
                // 解析通知数据
                var notificationData = data as dynamic;
                var content = notificationData?.Content?.ToString() ?? "";
                var title = notificationData?.Title?.ToString() ?? "系统通知";

                // 触发系统通知事件
                SystemNotificationReceived?.Invoke(title, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理系统通知时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理广播消息
        /// </summary>
        /// <param name="data">广播数据</param>
        private void HandleBroadcastMessage(object data)
        {
            try
            {
                // 解析广播数据
                var broadcastData = data as dynamic;
                var content = broadcastData?.Content?.ToString() ?? "";
                var sender = broadcastData?.Sender?.ToString() ?? "管理员";

                // 触发广播消息事件
                BroadcastMessageReceived?.Invoke(sender, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理广播消息时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送弹窗消息
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果</returns>
        public async Task<ApiResponse<bool>> SendPopupMessageAsync(
            string content,
            string targetUserId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备消息数据
                var messageData = new
                {
                    Content = content,
                    TargetUserId = targetUserId,
                    Sender = "当前用户",
                    Timestamp = DateTime.UtcNow
                };

                // 发送弹窗消息命令
                var success = await _communicationService.SendOneWayCommandAsync(
                    MessageCommands.SendPopupMessage,
                    messageData,
                    cancellationToken);

                if (success)
                {
                    return ApiResponse<bool>.CreateSuccess(true, "弹窗消息发送成功");
                }
                else
                {
                    return ApiResponse<bool>.Failure("弹窗消息发送失败", 500);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"发送弹窗消息过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 发送系统通知
        /// </summary>
        /// <param name="title">通知标题</param>
        /// <param name="content">通知内容</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果</returns>
        public async Task<ApiResponse<bool>> SendSystemNotificationAsync(
            string title,
            string content,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备通知数据
                var notificationData = new
                {
                    Title = title,
                    Content = content,
                    Sender = "系统",
                    Timestamp = DateTime.UtcNow
                };

                // 发送系统通知命令
                var success = await _communicationService.SendOneWayCommandAsync(
                    MessageCommands.SendSystemNotification,
                    notificationData,
                    cancellationToken);

                if (success)
                {
                    return ApiResponse<bool>.CreateSuccess(true, "系统通知发送成功");
                }
                else
                {
                    return ApiResponse<bool>.Failure("系统通知发送失败", 500);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"发送系统通知过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 发送广播消息
        /// </summary>
        /// <param name="content">广播内容</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果</returns>
        public async Task<ApiResponse<bool>> BroadcastMessageAsync(
            string content,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备广播数据
                var broadcastData = new
                {
                    Content = content,
                    Sender = "当前用户",
                    Timestamp = DateTime.UtcNow
                };

                // 发送广播消息命令
                var success = await _communicationService.SendOneWayCommandAsync(
                    MessageCommands.BroadcastMessage,
                    broadcastData,
                    cancellationToken);

                if (success)
                {
                    return ApiResponse<bool>.CreateSuccess(true, "广播消息发送成功");
                }
                else
                {
                    return ApiResponse<bool>.Failure("广播消息发送失败", 500);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"发送广播消息过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 发送用户消息
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果</returns>
        public async Task<ApiResponse<bool>> SendMessageToUserAsync(
            string content,
            string targetUserId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备消息数据
                var messageData = new
                {
                    Content = content,
                    TargetUserId = targetUserId,
                    Sender = "当前用户",
                    Timestamp = DateTime.UtcNow
                };

                // 发送用户消息命令并等待响应
                var response = await _communicationService.SendCommandAsync<object, bool>(
                    MessageCommands.SendMessageToUser,
                    messageData,
                    cancellationToken);

                if (response.Success)
                {
                    return ApiResponse<bool>.CreateSuccess(response.Data, "用户消息发送成功");
                }
                else
                {
                    return ApiResponse<bool>.Failure(response.Message, response.Code);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"发送用户消息过程中发生异常: {ex.Message}", 500);
            }
        }
    }
}