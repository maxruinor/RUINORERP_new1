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
    
    // 消息状态枚举（保留兼容性）
    public enum MessageStatus
    {
        Unread,
        Read,
        Processed,
        Cancel,
        WaitRminder,
        Unprocessed
    }
    // 注意：MessageStatus枚举已被废弃，使用IsRead布尔属性替代
    // 消息接口
    public interface IMessage
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        bool IsRead { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        int MessageID { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 标记为已读
        /// </summary>
        void MarkAsRead();

        /// <summary>
        /// 标记为已处理
        /// </summary>
        void MarkAsProcessed();
    }

}

