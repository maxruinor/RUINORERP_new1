using System;
using System.Collections.Generic;

using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.PacketSpec.Models.Core
{
    public interface IRequest
    {
        /// <summary>
        /// 请求唯一标识
        /// </summary>
        string RequestId { get; set; }

        /// <summary>
        /// 请求时间戳
        /// </summary>
        DateTime Timestamp { get; set; }

    }
}
