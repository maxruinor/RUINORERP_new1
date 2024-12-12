using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.TransModel
{
    public class ServerReminderData : ReminderDataBase
    {

        // 消息状态
        public MessageStatus Status { get; set; } = MessageStatus.Unread;

        // 消息优先级
        public MessagePriority Priority { get; set; }

        // 发送时间
        public string SendTime { get; set; }

        public long SenderID { get; set; }
        // 发送者
        public string SenderName { get; set; }

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
        /// 可以提醒多个人。
        /// </summary>
        public long[] ReceiverIDs { get; set; }

        // 构造函数
        public ServerReminderData()
        {
            SendTime = DateTime.Now.ToString();
        }

        public void MarkAsWaitRminder()
        {
            Status = MessageStatus.WaitRminder;
        }

        // 可以添加其他方法来处理消息，例如标记为已读、已处理等
        public void MarkAsRead()
        {
            Status = MessageStatus.Read;
        }

        public void MarkAsProcessed()
        {
            Status = MessageStatus.Processed;
        }
    }

}
