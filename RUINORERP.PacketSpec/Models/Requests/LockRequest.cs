using System;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 锁定请求 - 用于申请锁定资源
    /// </summary>
    [JsonObject]
    public class LockRequest : RequestBase
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        [JsonProperty(Order=10)]
        public string ResourceId { get; set; } = string.Empty;

        /// <summary>
        /// 锁类型（EXCLUSIVE, SHARED�?        /// </summary>
        [JsonProperty(Order=11)]
        public string LockType { get; set; } = "EXCLUSIVE";

        /// <summary>
        /// 锁定超时时间（毫秒）
        /// </summary>
        [JsonProperty(Order=12)]
        public int TimeoutMs { get; set; } = 30000; // 默认30�?
        /// <summary>
        /// 锁定原因
        /// </summary>
        [JsonProperty(Order=13)]
        public string LockReason { get; set; } = string.Empty;

        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty(Order=14)]
        public long UserId { get; set; }

        /// <summary>
        /// 用户�?        /// </summary>
        [JsonProperty(Order=15)]
        public string UserName { get; set; } = string.Empty;
    }
}


