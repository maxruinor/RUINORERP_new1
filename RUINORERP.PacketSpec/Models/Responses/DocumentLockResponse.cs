using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 文档锁定响应
    /// </summary>
    public class DocumentLockResponse : ResponseBase
    {
        /// <summary>
        /// 是否锁定成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 锁定ID
        /// </summary>
        public string LockId { get; set; } = string.Empty;

        /// <summary>
        /// 锁定时间
        /// </summary>
        public DateTime LockTime { get; set; }

        /// <summary>
        /// 锁定过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 当前锁定信息
        /// </summary>
        public LockedInfo? CurrentLockInfo { get; set; }

        /// <summary>
        /// 等待队列位置（如果锁定失败且需要等待）
        /// </summary>
        public int? QueuePosition { get; set; }

        /// <summary>
        /// 预计等待时间（秒）
        /// </summary>
        public int? EstimatedWaitTime { get; set; }
    }
}