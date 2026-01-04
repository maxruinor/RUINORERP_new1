using Newtonsoft.Json;
using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Model.TransModel
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
        /// 转换为请求数据字典
        /// </summary>
        /// <returns>包含消息所有属性的字典</returns>
        public Dictionary<string, object> ToDictionary()
        {
            var data = new Dictionary<string, object>
            {
                { "Id", MessageId },
                { "MessageType", MessageType },
                { "SenderId", SenderId },
                { "Sender", SenderName },
                { "ReceiverIds", ReceiverIds },
                { "Title", Title },
                { "Content", Content },
                { "BizType", BizType },
                { "BizId", BizId },
                { "SendTime", SendTime },
                { "IsRead", IsRead },
                { "NeedConfirmation", NeedConfirmation },
                { "ConfirmStatus", ConfirmStatus }

            };

            if (ConfirmTime.HasValue)
            {
                data.Add("ConfirmTime", ConfirmTime.Value);
            }

            if (BizData != null)
            {
                data.Add("BizData", BizData);
            }

            // 添加扩展数据
            foreach (var item in ExtendedData)
            {
                data[item.Key] = item.Value;
            }

            return data;
        }

        /// <summary>
        /// 从字典创建MessageData对象
        /// </summary>
        /// <param name="data">包含消息数据的字典</param>
        /// <returns>创建的MessageData对象</returns>
        public static MessageData FromDictionary(Dictionary<string, object> data)
        {
            var messageData = new MessageData();

            if (data == null)
                return messageData;

            // 基本属性赋值
            if (data.ContainsKey("Id") && data["Id"] != null)
                messageData.MessageId = Convert.ToInt64(data["Id"]);

            if (data.ContainsKey("MessageType") && data["MessageType"] != null)
            {
                // 兼容处理：如果是字符串形式的旧枚举值，需要转换
                string typeStr = data["MessageType"].ToString();
                if (Enum.TryParse<MessageType>(typeStr, out var messageType))
                {
                    messageData.MessageType = messageType;
                }
                else
                {
                    // 尝试从数值转换并映射到新的消息类型
                    if (int.TryParse(typeStr, out int typeValue))
                    {
                        // 根据旧的类型值映射到新的类型
                        // 旧类型映射规则：
                        // 1-5, 7-8, 10-11, 13-16 -> Popup
                        // 6, 15 -> Business
                        // 9, 12 -> System
                        messageData.MessageType = MapOldMessageTypeToNew(typeValue);
                    }
                    else
                    {
                        // 默认使用Business类型
                        messageData.MessageType = MessageType.Business;
                    }
                }
            }



            if (data.ContainsKey("SenderId") && data["SenderId"] != null)
                messageData.SenderId = Convert.ToInt64(data["SenderId"]);

            if (data.ContainsKey("Sender") && data["Sender"] != null)
                messageData.SenderName = data["Sender"].ToString();

            if (data.ContainsKey("ReceiverIds") && data["ReceiverIds"] != null)
            {
                var receiverIds = data["ReceiverIds"] as List<object>;
                if (receiverIds != null)
                    messageData.ReceiverIds = receiverIds.Select(id => Convert.ToInt64(id)).ToList();
            }

            if (data.ContainsKey("Title") && data["Title"] != null)
                messageData.Title = data["Title"].ToString();

            if (data.ContainsKey("Content") && data["Content"] != null)
                messageData.Content = data["Content"].ToString();

            if (data.ContainsKey("BizType") && data["BizType"] != null)
                messageData.BizType = (BizType)Enum.Parse(typeof(BizType), data["BizType"].ToString());

            if (data.ContainsKey("BizId") && data["BizId"] != null)
                messageData.BizId = Convert.ToInt64(data["BizId"]);

            if (data.ContainsKey("SendTime") && data["SendTime"] != null)
                messageData.SendTime = Convert.ToDateTime(data["SendTime"]);

            if (data.ContainsKey("IsRead") && data["IsRead"] != null)
                messageData.IsRead = Convert.ToBoolean(data["IsRead"]);

            if (data.ContainsKey("NeedConfirmation") && data["NeedConfirmation"] != null)
                messageData.NeedConfirmation = Convert.ToBoolean(data["NeedConfirmation"]);

            if (data.ContainsKey("ConfirmStatus") && data["ConfirmStatus"] != null)
                messageData.ConfirmStatus = (ConfirmStatus)Enum.Parse(typeof(ConfirmStatus), data["ConfirmStatus"].ToString());

            if (data.ContainsKey("ConfirmTime") && data["ConfirmTime"] != null)
                messageData.ConfirmTime = Convert.ToDateTime(data["ConfirmTime"]);



            if (data.ContainsKey("BizData") && data["BizData"] != null)
                messageData.BizData = data["BizData"];

            // 提取扩展数据
            var knownKeys = new HashSet<string>
            {
                "Id", "MessageType", "SenderId", "Sender", "ReceiverIds",
                "Title", "Content", "BizType", "BizId", "SendTime", "IsRead",
                "NeedConfirmation", "ConfirmStatus", "ConfirmTime", "Source", "BizData"
            };

            foreach (var key in data.Keys)
            {
                if (!knownKeys.Contains(key))
                {
                    messageData.ExtendedData[key] = data[key];
                }
            }

            return messageData;
        }

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
        /// 将旧的消息类型值映射到新的消息类型
        /// 用于兼容旧系统发送的消息
        /// </summary>
        /// <param name="oldTypeValue">旧的消息类型数值</param>
        /// <returns>新的消息类型</returns>
        private static MessageType MapOldMessageTypeToNew(int oldTypeValue)
        {
            switch (oldTypeValue)
            {
                // 弹出式提醒：需要立即用户注意的消息
                case 1: // Message
                case 2: // Reminder
                case 3: // Event
                case 4: // Task
                case 5: // Notice
                case 7: // Prompt
                case 8: // UnLockRequest
                case 10: // Broadcast
                case 11: // Approve
                case 13: // Text
                case 14: // IM
                case 16: // UserMessage
                    return MessageType.Popup;
                
                // 业务性提醒：业务流程相关的消息
                case 6: // Business
                case 15: // BusinessData
                    return MessageType.Business;
                
                // 系统消息：系统级通知和日志
                case 9: // ExceptionLog
                case 12: // System
                    return MessageType.System;
                
                // 默认值
                default:
                    return MessageType.Business;
            }
        }
    }
}