using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{



    public enum MessageStatus
    {
        /// <summary>
        /// 未读
        /// </summary>
        Unread,

        /// <summary>
        /// 已读
        /// </summary>
        Read,

        /// <summary>
        /// 未处理
        /// </summary>
        Unprocessed,

        /// <summary>
        /// 已处理
        /// </summary>
        Processed,

        /// <summary>
        /// 稍候提醒
        /// </summary>
        WaitRminder,

        Cancel,
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

