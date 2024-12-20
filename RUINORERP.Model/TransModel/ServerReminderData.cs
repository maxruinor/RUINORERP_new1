using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.TransModel
{

    /// <summary>
    /// 注意所有发送者  接受者都是员工
    /// </summary>
    public class ServerReminderData : ReminderDataBase
    {

        // 消息状态
        public MessageStatus Status { get; set; } = MessageStatus.Unread;

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
        /// 可以提醒多个人 注意是员工ID
        /// </summary>
        public long[] ReceiverEmployeeIDs { get; set; }

        public string ReceiverEmployee
        { 
            get
            {
                return string.Join(",", ReceiverEmployeeIDs);
            }
        }

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
