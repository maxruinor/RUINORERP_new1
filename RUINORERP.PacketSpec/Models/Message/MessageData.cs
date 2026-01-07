using Newtonsoft.Json;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using static WorkflowCore.Models.ActivityResult;

namespace RUINORERP.PacketSpec.Models.Message
{
    /// <summary>
    /// 消息确认状态枚举
    /// 表示消息的确认处理状态
    /// </summary>
    public enum ConfirmStatus
    {
        /// <summary>
        /// 未确认
        /// 默认状态，消息尚未被确认
        /// </summary>
        Unconfirmed = 0,

        /// <summary>
        /// 已确认
        /// 消息已被接收方确认
        /// </summary>
        Confirmed = 1,

        /// <summary>
        /// 已拒绝
        /// 消息被接收方拒绝
        /// </summary>
        Rejected = 2,

        /// <summary>
        /// 处理中
        /// 消息正在处理中
        /// </summary>
        Processing = 3,

        /// <summary>
        /// 已完成
        /// 消息相关的任务已完成
        /// </summary>
        Completed = 4
    }

    /// <summary>
    /// 消息数据模型
    /// 替代旧的ReminderData类型，用于客户端与服务器之间的消息传递
    /// </summary>
    public class MessageData
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public long MessageId { get; set; }

        /// <summary>
        /// 消息类型 - 使用统一的MessageType枚举
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 发送者ID
        /// </summary>
        public long SenderId { get; set; }

        /// <summary>
        /// 发送者名称
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 是否发送给自己
        /// 提醒类的可以发送给自己
        /// </summary>
        public bool SendToSelf { get; set; } = false;


        /// <summary>
        /// 接收者ID列表
        /// </summary>
        public List<long> ReceiverIds { get; set; } = new List<long>();

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BizType BizType { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public long BizId { get; set; }

        /// <summary>
        /// 业务数据
        /// </summary>
        public object BizData { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 消息创建时间
        /// 与SendTime保持同步
        /// </summary>
        [JsonIgnore]
        public DateTime CreateTime
        {
            get => SendTime;
            set => SendTime = value;
        }

        /// <summary>
        /// 接收者ID的别名
        /// 为保持API兼容性保留
        /// </summary>
        [JsonIgnore]
        public List<long> RecipientIds
        {
            get => ReceiverIds;
            set => ReceiverIds = value;
        }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 是否需要确认
        /// </summary>
        public bool NeedConfirmation { get; set; }

        /// <summary>
        /// 确认状态
        /// </summary>
        public ConfirmStatus ConfirmStatus { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? ConfirmTime { get; set; }

        /// <summary>
        /// 扩展数据字典
        /// </summary>
        public Dictionary<string, object> ExtendedData { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 是否为弹窗消息（默认false，所有消息直接加载到消息中心）
        /// </summary>
        public bool IsPopupMessage { get; set; } = false;

        /// <summary>
        /// 消息优先级（用于消息中心排序）
        /// </summary>
        public int Priority { get; set; } = 0;

        /// <summary>
        /// 是否自动添加到消息中心（默认true）
        /// </summary>
        public bool AutoAddToMessageCenter { get; set; } = true;

        /// <summary>
        /// 消息类别（用于消息中心分类）
        /// </summary>
        public string Category { get; set; } = "业务通知";


        /// <summary>
        /// 标记消息为已读
        /// 更新已读状态并设置阅读时间
        /// </summary>
        public void MarkAsRead()
        {
            IsRead = true;
            ReadTime = DateTime.Now;
        }

        /// <summary>
        /// 标记消息为已处理
        /// 更新确认状态为已完成并设置确认时间
        /// </summary>
        public void MarkAsProcessed()
        {
            ConfirmStatus = ConfirmStatus.Completed;
            ConfirmTime = DateTime.Now;
        }


        /// <summary>
        /// 阅读时间
        /// 兼容UI层使用的属性
        /// </summary>
        [JsonIgnore]
        public DateTime? ReadTime { get; set; }


        /// <summary>
        /// 创建任务状态变更消息数据的统一方法
        /// 用于替换重复的构造代码，并智能生成详细的消息内容
        /// </summary>
        /// <param name="update">任务更新数据</param>
        /// <param name="senderId">发送者ID</param>
        /// <param name="senderName">发送者名称</param>
        /// <param name="isPopupMessage">是否为弹窗消息（默认false）</param>
        /// <param name="receiverIds">接收者ID列表（可选）</param>
        /// <returns>创建的消息数据对象</returns>
        public static MessageData CreateTodoUpdateMessage(
            TodoUpdate update,
            long senderId = 0,
            string senderName = null,
            bool isPopupMessage = false,
            List<long> receiverIds = null)
        {
            if (update == null)
                throw new ArgumentNullException(nameof(update), "任务更新数据不能为空");

            // 生成详细的消息标题和内容
            string title = GenerateDetailedTitle(update);
            string content = GenerateDetailedContent(update, senderName);

            return new MessageData
            {
                MessageId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId(),
                MessageType = MessageType.Business,
                Title = title,
                Content = content,
                BizType = update.BusinessType,
                BizId = update.BillId,
                BizData = update,
                SenderId = senderId,
                SenderName = senderName,
                SendTime = DateTime.Now,
                IsPopupMessage = isPopupMessage,
                ReceiverIds = receiverIds ?? new List<long>(),
                // 设置消息为业务消息类型，直接加载到消息中心
                AutoAddToMessageCenter = true,
                Category = "业务通知"
            };
        }

        /// <summary>
        /// 根据任务更新数据生成详细的消息标题
        /// </summary>
        /// <param name="update">任务更新数据</param>
        /// <returns>生成的详细标题</returns>
        private static string GenerateDetailedTitle(TodoUpdate update)
        {
            string billInfo = !string.IsNullOrEmpty(update.BillNo)
                ? $"【{update.BillNo}】"
                : $"【{update.BusinessType} #{update.BillId}】";

            switch (update.UpdateType)
            {
                case TodoUpdateType.StatusChanged:
                    return $"{update.BusinessType}{billInfo}{GetStatusChangeAction(update)}";
                case TodoUpdateType.Deleted:
                    return $"{update.BusinessType}{billInfo}已删除";
                case TodoUpdateType.Created:
                    return $"{update.BusinessType}{billInfo}已创建";
                default:
                    return $"{update.BusinessType}{billInfo}任务状态变更";
            }
        }

        /// <summary>
        /// 根据任务更新数据生成详细的消息内容
        /// </summary>
        /// <param name="update">任务更新数据</param>
        /// <param name="senderName">发送者名称</param>
        /// <returns>生成的详细内容</returns>
        private static string GenerateDetailedContent(TodoUpdate update, string senderName)
        {
            string billInfo = !string.IsNullOrEmpty(update.BillNo)
                ? $"{update.BillNo}\n"
                : $"单据ID：{update.BillId}\n";

         
            string statusInfo = GetStatusInfo(update);
            string operationInfo = !string.IsNullOrEmpty(update.OperationDescription)
                ? $" 操作描述:{update.OperationDescription}\n"
                : string.Empty;
            string operatorInfo = !string.IsNullOrEmpty(senderName)
                ? $" 操作人:{senderName}\n"
                : string.Empty;
            string timeInfo = $" 操作时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

            return $"{update.BusinessType}{billInfo},{operationInfo},{statusInfo},{operatorInfo},{timeInfo}";
        }

        /// <summary>
        /// 根据任务更新数据获取状态变更动作描述
        /// </summary>
        /// <param name="update">任务更新数据</param>
        /// <returns>状态变更动作描述</returns>
        private static string GetStatusChangeAction(TodoUpdate update)
        {
            // 根据BusinessStatusValue获取状态描述
            if (update.BizStatusValue != null)
            {
                if (update.BizStatusValue is DataStatus dataStatus)
                {
                    return dataStatus switch
                    {
                        DataStatus.草稿 => "已保存为草稿",
                        DataStatus.新建 => "已提交",
                        DataStatus.确认 => "已审核",
                        DataStatus.完结 => "已结案",
                        DataStatus.作废 => "已取消",
                        _ => $"状态已变更为{dataStatus}"
                    };
                }
                else if (update.BizStatusValue is ApprovalStatus approvalStatus)
                {
                    return approvalStatus switch
                    {
                        ApprovalStatus.未审核 => "审批状态已重置为未审核",
                        //ApprovalStatus.审核中 => "审批中",
                        ApprovalStatus.审核通过 => "审批通过",
                        ApprovalStatus.审核驳回 => "审批驳回",
                        _ => $"审批状态已变更为{approvalStatus}"
                    };
                }
                else if (update.BizStatusValue is string statusStr)
                {
                    return $"状态已变更为{statusStr}";
                }
            }

            return "状态已变更";
        }

        /// <summary>
        /// 根据任务更新数据获取状态信息
        /// </summary>
        /// <param name="update">任务更新数据</param>
        /// <returns>状态信息</returns>
        private static string GetStatusInfo(TodoUpdate update)
        {

            string statusValue = update.BizStatusValue?.ToString() ?? "未知";


            string statusDescription = GlobalStateRulesManager.Instance.GetStatusTypeDescription(update.BizStatusType);//GetStatusDescription(update.BusinessStatusValue);

            return $"当前状态:{statusValue}{(!string.IsNullOrEmpty(statusDescription) ? $"（{statusDescription}）" : string.Empty)}\n";
        }
    }
}
