using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.UI.Network.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 工作流命令处理器
    /// 负责处理与工作流相关的命令，如工作流提醒、审批通知等
    /// </summary>
    [ClientCommandHandler("WorkflowCommandHandler", 40)]
    public class WorkflowCommandHandler : BaseClientCommandHandler
    {
        private readonly ILogger<WorkflowCommandHandler> _logger;
        private readonly MessageService _messageService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageService">消息服务</param>
        /// <param name="logger">日志记录器</param>
        public WorkflowCommandHandler(MessageService messageService, ILogger<WorkflowCommandHandler> logger = null) : 
            base(logger ?? Startup.GetFromFac<ILogger<BaseClientCommandHandler>>())
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _logger = logger ?? Startup.GetFromFac<ILogger<WorkflowCommandHandler>>();
            
            // 设置支持的命令
            SetSupportedCommands(
                WorkflowCommands.WorkflowReminder,
                WorkflowCommands.WorkflowStatusUpdate,
                WorkflowCommands.WorkflowApproval,
                WorkflowCommands.WorkflowStart,
                WorkflowCommands.WorkflowCommand,
                WorkflowCommands.NotifyApprover,
                WorkflowCommands.NotifyApprovalComplete,
                WorkflowCommands.NotifyStartSuccess,
                WorkflowCommands.WorkflowReminderRequest,
                WorkflowCommands.WorkflowReminderChanged,
                WorkflowCommands.WorkflowReminderReply
            );
        }

        /// <summary>
        /// 初始化处理器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public override async Task<bool> InitializeAsync()
        {
            bool initialized = await base.InitializeAsync();
            if (initialized)
            {
                _logger.LogDebug("工作流命令处理器初始化成功");
            }
            return initialized;
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        public override async Task HandleAsync(PacketModel packet)
        {
            if (packet == null || packet.CommandId == null)
            {
                _logger.LogError("收到无效的工作流数据包");
                return;
            }

            _logger.LogDebug($"收到工作流命令: {(ushort)packet.CommandId}");

            try
            {
                // 根据命令ID分发到对应的处理方法
                if (packet.CommandId == WorkflowCommands.WorkflowReminder)
                {
                    await HandleWorkflowReminderAsync(packet);
                }
                else if (packet.CommandId == WorkflowCommands.WorkflowStatusUpdate)
                {
                    await HandleWorkflowStatusUpdateAsync(packet);
                }
                else if (packet.CommandId == WorkflowCommands.WorkflowApproval)
                {
                    await HandleWorkflowApprovalAsync(packet);
                }
                else
                {
                    _logger.LogWarning($"未处理的工作流命令ID: {packet.CommandId.FullCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理工作流命令时发生异常");
            }
        }

        /// <summary>
        /// 处理工作流提醒命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task HandleWorkflowReminderAsync(PacketModel packet)
        {
            try
            {
                // 从数据包中提取工作流提醒数据
                var reminderData = ExtractWorkflowReminderData(packet);
                if (reminderData == null)
                {
                    _logger.LogWarning("无法解析工作流提醒数据");
                    return;
                }

                // 确保在UI线程中显示提醒
                if (Application.OpenForms.Count > 0)
                {
                    // 在主窗体线程中执行
                    var mainForm = Application.OpenForms[0];
                    if (mainForm.InvokeRequired)
                    {
                        mainForm.Invoke(new Action(() => ShowWorkflowReminder(reminderData)));
                    }
                    else
                    {
                        ShowWorkflowReminder(reminderData);
                    }
                }
                else
                {
                    // 如果没有打开的窗体，直接显示
                    ShowWorkflowReminder(reminderData);
                }

                _logger.LogDebug($"工作流提醒已处理 - 主题: {reminderData.RemindSubject}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理工作流提醒命令时发生异常");
            }
        }

        /// <summary>
        /// 显示工作流提醒
        /// </summary>
        /// <param name="reminderData">提醒数据</param>
        private void ShowWorkflowReminder(ReminderData reminderData)
        {
            try
            {
                // 创建并显示消息提示窗体
                var messageData = CreateMessageDataFromReminder(reminderData);
                var messagePrompt = new RUINORERP.UI.IM.MessagePrompt(messageData);
                
                // 设置窗体属性
                messagePrompt.StartPosition = FormStartPosition.CenterScreen;
                messagePrompt.TopMost = true; // 保持窗体置顶
                
                // 显示窗体（非模态，允许用户继续工作）
                messagePrompt.Show();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "显示工作流提醒时发生异常");
            }
        }

        /// <summary>
        /// 从提醒数据创建消息数据
        /// </summary>
        /// <param name="reminderData">提醒数据</param>
        /// <returns>消息数据</returns>
        private MessageData CreateMessageDataFromReminder(ReminderData reminderData)
        {
            var messageData = new MessageData
            {
                MessageId = DateTime.Now.Ticks, // 使用时间戳作为消息ID
                Title = reminderData.RemindSubject ?? "工作流提醒",
                Content = reminderData.ReminderContent ?? reminderData.RemindSubject ?? "您有新的工作流提醒",
                MessageType = MessageType.Reminder,
                SendTime = DateTime.Now,
                IsRead = false,
                BizId = reminderData.BizKeyID,
                BizType = reminderData.BizType,
                BizData = reminderData.BizData,
                NeedConfirmation = true // 工作流提醒需要确认
            };

            return messageData;
        }

        /// <summary>
        /// 从数据包中提取工作流提醒数据
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>提醒数据</returns>
        private ReminderData ExtractWorkflowReminderData(PacketModel packet)
        {
            if (packet == null)
                return null;

            try
            {
                // 尝试从Request中获取数据
                if (packet.Request != null && packet.Request is MessageRequest messageRequest)
                {
                    if (messageRequest.Data != null)
                    {
                        // 如果数据是字符串（JSON格式），反序列化
                        if (messageRequest.Data is string jsonData)
                        {
                            var reminderInfo = JsonSerializer.Deserialize<ReminderInfo>(jsonData);
                            if (reminderInfo?.ReminderData != null)
                            {
                                return reminderInfo.ReminderData;
                            }
                        }
                        // 如果数据是字典，从中提取
                        else if (messageRequest.Data is System.Collections.Generic.Dictionary<string, object> dataDict)
                        {
                            if (dataDict.TryGetValue("ReminderData", out var reminderDataObj) && reminderDataObj is ReminderData reminderData)
                            {
                                return reminderData;
                            }
                        }
                    }
                }

                // 尝试从Extensions中提取
                if (packet.Extensions != null)
                {
                    foreach (var kvp in packet.Extensions)
                    {
                        if (kvp.Key == "ReminderData")
                        {
                            try
                            {
                                // 使用JSON反序列化处理JToken到ReminderData的转换
                                var jsonString = kvp.Value?.ToString();
                                if (!string.IsNullOrEmpty(jsonString))
                                {
                                    var reminderData = JsonSerializer.Deserialize<ReminderData>(jsonString);
                                    if (reminderData != null)
                                    {
                                        return reminderData;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "从Extensions中反序列化ReminderData失败");
                            }
                        }
                    }
                }

                _logger.LogWarning("无法从数据包中提取工作流提醒数据");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "提取工作流提醒数据时发生异常");
                return null;
            }
        }

        /// <summary>
        /// 处理工作流状态更新命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task HandleWorkflowStatusUpdateAsync(PacketModel packet)
        {
            try
            {
                _logger.LogDebug("处理工作流状态更新命令");
                // 这里可以添加工作流状态更新的处理逻辑
                // 例如：更新界面中的工作流状态显示
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理工作流状态更新命令时发生异常");
            }
        }

        /// <summary>
        /// 处理工作流审批命令
        /// </summary>
        /// <param name="packet">数据包</param>
        private async Task HandleWorkflowApprovalAsync(PacketModel packet)
        {
            try
            {
                _logger.LogDebug("处理工作流审批命令");
                // 这里可以添加工作流审批的处理逻辑
                // 例如：显示审批对话框
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理工作流审批命令时发生异常");
            }
        }

        /// <summary>
        /// 辅助类，用于反序列化工作流提醒数据
        /// </summary>
        private class ReminderInfo
        {
            public ReminderData ReminderData { get; set; }
            public string SendTime { get; set; }
            public bool ForcePopup { get; set; }
        }
    }
}