using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 强制解锁响应
    /// </summary>
    public class ForceUnlockResponse : ResponseBase
    {
        /// <summary>
        /// 是否强制解锁成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 强制解锁时间
        /// </summary>
        public DateTime ForceUnlockTime { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 被强制解锁的锁定信息
        /// </summary>
        public LockedInfo? ReleasedLockInfo { get; set; }

        /// <summary>
        /// 操作员用户ID
        /// </summary>
        public long OperatorUserId { get; set; }

        /// <summary>
        /// 操作员用户名
        /// </summary>
        public string OperatorUserName { get; set; } = string.Empty;
    }
}