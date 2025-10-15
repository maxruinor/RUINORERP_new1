using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 文档锁定请求
    /// </summary>
    public class DocumentLockRequest : RequestBase
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
        /// 菜单ID
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 单据数据
        /// </summary>
        public CommBillData? BillData { get; set; }

        /// <summary>
        /// 锁定原因
        /// </summary>
        public string LockReason { get; set; } = string.Empty;

        /// <summary>
        /// 锁定超时时间（毫秒）
        /// </summary>
        public int TimeoutMs { get; set; } = 300000; // 默认5分钟
    }
}
