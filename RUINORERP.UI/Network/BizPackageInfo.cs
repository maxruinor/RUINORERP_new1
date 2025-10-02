using SuperSocket.ProtoBase;

using SuperSocket.ProtoBase;
using System;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// </summary>
    public class BizPackageInfo : IPackageInfo<string>
    {
        /// <summary>
        /// 包标识
        /// 用于在SuperSocket框架中标识数据包类型
        /// </summary>
        public string Key { get; set; }
        public PacketModel Packet { get; set; }
    }
}