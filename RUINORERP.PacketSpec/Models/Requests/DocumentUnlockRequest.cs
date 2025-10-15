using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 文档解锁请求
    /// </summary>
    public class DocumentUnlockRequest : RequestBase
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 锁定ID
        /// </summary>
        public string LockId { get; set; } = string.Empty;

        /// <summary>
        /// 解锁原因
        /// </summary>
        public string UnlockReason { get; set; } = string.Empty;
    }
}