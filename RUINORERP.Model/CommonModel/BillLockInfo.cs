using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    /// <summary>
    ///  单据锁定信息
    /// </summary>
    public class BillLockInfo
    {
        public int Id { get; set; }

        // 消息状态
        public MessageStatus Status { get; set; } = MessageStatus.Unread;

        // 消息优先级
        public MessagePriority Priority { get; set; }

        public string LockedName { get; set; }
        // 发送时间
        public string LockedTime { get; set; }

        public long SenderID { get; set; }
        // 发送者
        public string SenderName { get; set; }

        // 消息内容
        public string Content { get; set; }

        public int BizType { get; set; }

        public long BillID { get; set; }
        /// <summary>
        /// josn格式
        /// </summary>
        public string BillData { get; set; }

        // 下次处理者
        public string NextProcessor { get; set; }

        // 构造函数
        public BillLockInfo()
        {
            LockedTime = DateTime.Now.ToString();
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

