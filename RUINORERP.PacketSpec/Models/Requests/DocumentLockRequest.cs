using System;
using RUINORERP.Model.CommonModel;
using MessagePack;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 文档锁定请求 - 用于锁定业务单据
    /// </summary>
    [MessagePackObject]
    public class DocumentLockRequest : RequestBase
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        [Key(10)]
        public long BillId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Key(11)]
        public long UserId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [Key(12)]
        public long MenuId { get; set; }

        /// <summary>
        /// 单据数据
        /// </summary>
        [Key(13)]
        public CommBillData? BillData { get; set; }

        /// <summary>
        /// 锁定原因
        /// </summary>
        [Key(14)]
        public string LockReason { get; set; } = string.Empty;

        /// <summary>
        /// 锁定超时时间（毫秒）
        /// </summary>
        [Key(15)]
        public int TimeoutMs { get; set; } = 300000; // 默认5分钟
    }
}
