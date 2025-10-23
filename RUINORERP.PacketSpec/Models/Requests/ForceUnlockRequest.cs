using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 强制解锁请求
    /// </summary>
    [JsonObject]
    public class ForceUnlockRequest : RequestBase
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        [JsonProperty(Order=100)]
        public long BillId { get; set; }

        /// <summary>
        /// 强制解锁原因
        /// </summary>
        [JsonProperty(Order=101)]
        public string ForceReason { get; set; } = string.Empty;

        /// <summary>
        /// 操作者用户ID
        /// </summary>
        [JsonProperty(Order=102)]
        public long OperatorUserId { get; set; }
    }
}


