using System;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 通用请求类
    /// </summary>
    [Serializable]
    public class GeneralRequest : RequestBase
    {
        /// <summary>
        /// 通用请求数据
        /// </summary>
        public object Data { get; set; }
    }
}
