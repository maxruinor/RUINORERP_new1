using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 文档解锁响应
    /// </summary>
    public class DocumentUnlockResponse : ResponseBase
    {
        /// <summary>
        /// 是否解锁成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        public DateTime UnlockTime { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 之前锁定信息
        /// </summary>
        public LockedInfo? PreviousLockInfo { get; set; }

        /// <summary>
        /// 解锁用户ID
        /// </summary>
        public long UnlockUserId { get; set; }

        /// <summary>
        /// 解锁用户名
        /// </summary>
        public string UnlockUserName { get; set; } = string.Empty;
    }
}