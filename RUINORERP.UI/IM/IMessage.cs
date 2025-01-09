using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.IM
{

    public enum MessagePriority
    {
        General,
        Important,
        Exception
    }
    // 消息类
    //后续可以添加更多的属性和方法来处理消息。是否要处理。处理得怎么样。
    public class IMessage
    {
        // 消息ID
        public int Id { get; set; }

        // 消息状态
        public MessageStatus Status { get; set; } = MessageStatus.Unread;

        // 消息优先级
        public MessagePriority Priority { get; set; }

        // 发送时间
        public string SendTime { get; set; }

        // 发送者
        public string Sender { get; set; }

        // 消息内容
        public string Content { get; set; }

        // 下次处理者
        public string NextProcessor { get; set; }

        // 构造函数
        public IMessage()
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

}

