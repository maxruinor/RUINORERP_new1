using SuperSocket.ProtoBase;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 业务数据包信息类
    /// </summary>
public class BizPackageInfo : IPackageInfo<string>
    {
        /// <summary>
        /// 包标识
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 包头长度不够18
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 整个原始包，包括包头
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public OriginalData od { get; set; }
    }
}