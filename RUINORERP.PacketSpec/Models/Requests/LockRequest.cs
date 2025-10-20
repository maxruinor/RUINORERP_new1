using System;
using MessagePack;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 锁定请求 - 用于申请锁定资源
    /// </summary>
    [MessagePackObject]
    public class LockRequest : RequestBase
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        [Key(10)]
        public string ResourceId { get; set; } = string.Empty;

        /// <summary>
        /// 锁类型（EXCLUSIVE, SHARED）
        /// </summary>
        [Key(11)]
        public string LockType { get; set; } = "EXCLUSIVE";

        /// <summary>
        /// 锁定超时时间（毫秒）
        /// </summary>
        [Key(12)]
        public int TimeoutMs { get; set; } = 30000; // 默认30秒

        /// <summary>
        /// 锁定原因
        /// </summary>
        [Key(13)]
        public string LockReason { get; set; } = string.Empty;

        /// <summary>
        /// 用户ID
        /// </summary>
        [Key(14)]
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Key(15)]
        public string UserName { get; set; } = string.Empty;
    }
}