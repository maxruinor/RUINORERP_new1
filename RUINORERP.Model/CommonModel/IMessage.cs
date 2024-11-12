using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    /// <summary>
    /// 消息类,后续可以添加更多的属性和方法来处理消息。是否要处理。处理得怎么样。
    /// 如果没有处理则要保存在服务器上，下次登录时再处理
    /// </summary>
    public class TranMessage
    {
        // 消息ID
        public int Id { get; set; }

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
        public string Content { get; set; }

        public string EntityType { get; set; }

        /// <summary>
        /// josn格式
        /// </summary>
        public string BillData { get; set; }

        // 下次处理者
        public string NextProcessor { get; set; }

        // 构造函数
        public TranMessage()
        {
            SendTime = DateTime.Now.ToString();
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
    public enum MessageStatus
    {
        Unread,
        Read,
        Unprocessed,
        Processed
    }

    public enum MessagePriority
    {
        /// <summary>
        /// 一般消息
        /// </summary>
        General,

        /// <summary>
        /// 重要消息
        /// </summary>
        Important,

        /// <summary>
        /// 紧急消息
        /// </summary>
        Exception
    }
}

