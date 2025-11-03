using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.TransModel
{
 

    /// <summary>
    /// 提醒数据用于工作流和所有交互
    /// </summary>
    public class ReminderData : ReminderDataBase
    {
        /// <summary>
        /// 锁单时 保存的是CommBillData
        /// </summary>
        public object BizData { get; set; }

        /// <summary>
        /// 单据主键 这种业务性keyid
        /// </summary>
        public long BizKeyID { get; set; }
        public MessageType messageCmd { get; set; } = MessageType.Unknown;
        // 消息状态
        /// <summary>
        /// 消息是否已读
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// 提醒状态（兼容旧版本）
        /// </summary>
        [Obsolete("Use IsRead property instead")]
        public MessageStatus Status
        {
            get { return IsRead ? MessageStatus.Read : MessageStatus.Unread; }
            set { IsRead = (value == MessageStatus.Read || value == MessageStatus.Processed); }
        }

        // 消息优先级
        public MessagePriority Priority { get; set; }

        // 发送时间
        public string SendTime { get; set; }

        public long SenderEmployeeID { get; set; }
        // 发送者
        public string SenderEmployeeName { get; set; }

        // 消息内容
        public string ReminderContent { get; set; }

        public string EntityType { get; set; }

        /// <summary>
        /// josn格式
        /// </summary>
        public string BillData { get; set; }

        // 下次处理者
        //public string NextProcessor { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string RemindSubject { get; set; }

        public int RemindTimes { get; set; }

        /// <summary>
        /// 提醒间隔，默认20秒
        /// </summary>
        public double RemindInterval { get; set; } = 20;
        /// <summary>
        /// 可以提醒多个人,一定是能使用系统的用户
        /// </summary>
        public List<long> ReceiverUserIDs { get; set; } = new List<long>();

        public string ReceiverUser
        {
            get
            {
                return string.Join(",", ReceiverUserIDs);
            }
        }

        public object CreateTime { get; set; }
        public DateTime ReadTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
        public bool IsSystemMessage { get; set; }

        // 构造函数
        public ReminderData()
        {
            SendTime = DateTime.Now.ToString();
        }

        public void MarkAsWaitRminder()
        {
            // 等待提醒状态仍保持为未读
            IsRead = false;
        }

        // 可以添加其他方法来处理消息，例如标记为已读、已处理等
        public void MarkAsRead()
        {
            // 标记为已读
            IsRead = true;
        }

        public void MarkAsProcessed()
        {
            // 处理完成也标记为已读
            IsRead = true;
        }
    }
}
       