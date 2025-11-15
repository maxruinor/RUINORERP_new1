using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 强制解锁请求
    /// </summary>
    public class ForceUnlockRequest : RequestBase
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 强制解锁原因
        /// </summary>
        public string ForceReason { get; set; } = string.Empty;

        /// <summary>
        /// 操作者用户ID
        /// </summary>
        public long OperatorUserId { get; set; }
    }
}
