using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 广播锁定响应
    /// </summary>
    public class BroadcastLockResponse : ResponseBase
    {
        /// <summary>
        /// 是否广播成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 广播的消息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 影响的会话数量
        /// </summary>
        public int AffectedSessions { get; set; }
    }
}