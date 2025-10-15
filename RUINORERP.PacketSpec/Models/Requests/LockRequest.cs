using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 锁定请求
    /// </summary>
    public class LockRequest : RequestBase
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public string ResourceId { get; set; } = string.Empty;

        /// <summary>
        /// 锁类型（EXCLUSIVE, SHARED）
        /// </summary>
        public string LockType { get; set; } = "EXCLUSIVE";

        /// <summary>
        /// 锁定超时时间（毫秒）
        /// </summary>
        public int TimeoutMs { get; set; } = 30000; // 默认30秒

        /// <summary>
        /// 锁定原因
        /// </summary>
        public string LockReason { get; set; } = string.Empty;

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = string.Empty;
    }
}