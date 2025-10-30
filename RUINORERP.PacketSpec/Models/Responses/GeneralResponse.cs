using System;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 通用响应类
    /// </summary>
    [Serializable]
    public class GeneralResponse : ResponseBase
    {
        /// <summary>
        /// 通用响应数据
        /// </summary>
        public object Data { get; set; }
    }
}
