using System;
using MessagePack;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 文档锁定响应 - 表示业务单据锁定的结果
    /// </summary>
    [MessagePackObject]
    public class DocumentLockResponse : ResponseBase
    {
        /// <summary>
        /// 是否锁定成功
        /// </summary>
        [Key(10)]
        public bool Success { get; set; }

        /// <summary>
        /// 锁定ID
        /// </summary>
        [Key(11)]
        public string LockId { get; set; } = string.Empty;

        /// <summary>
        /// 锁定时间
        /// </summary>
        [Key(12)]
        public DateTime LockTime { get; set; }

        /// <summary>
        /// 锁定过期时间
        /// </summary>
        [Key(13)]
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [Key(14)]
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 当前锁定信息
        /// </summary>
        [Key(15)]
        public LockedInfo? CurrentLockInfo { get; set; }

        /// <summary>
        /// 等待队列位置（如果锁定失败且需要等待）
        /// </summary>
        [Key(16)]
        public int? QueuePosition { get; set; }

        /// <summary>
        /// 预计等待时间（秒）
        /// </summary>
        [Key(17)]
        public int? EstimatedWaitTime { get; set; }
    }
}