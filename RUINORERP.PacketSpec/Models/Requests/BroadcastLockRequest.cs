using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 广播锁定请求
    /// </summary>
    [JsonObject]
    public class BroadcastLockRequest : RequestBase
    {
        /// <summary>
        /// 锁定信息列表
        /// </summary>
        [JsonProperty(Order=100)]
        public List<LockedInfo> LockedDocuments { get; set; } = new List<LockedInfo>();
    }
}


