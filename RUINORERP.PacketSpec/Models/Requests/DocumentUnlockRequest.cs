using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 文档解锁请求
    /// </summary>
    [JsonObject]
    public class DocumentUnlockRequest : RequestBase
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        [JsonProperty(Order=100)]
        public long BillId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty(Order=101)]
        public long UserId { get; set; }

        /// <summary>
        /// 锁定ID
        /// </summary>
        [JsonProperty(Order=102)]
        public string LockId { get; set; } = string.Empty;

        /// <summary>
        /// 解锁原因
        /// </summary>
        [JsonProperty(Order=103)]
        public string UnlockReason { get; set; } = string.Empty;
    }
}


