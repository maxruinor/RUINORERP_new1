using RUINORERP.Server.Commands;
using SuperSocket.ProtoBase;
using TransInstruction.DataPortal;

namespace RUINORERP.Server.ServerSession
{

    public class BizPackageInfo : IKeyedPackageInfo<string>
    {
        public string Key { get; set; }

        /// <summary>
        /// 包头长度不够18
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// 整个原始包，包括包头
        /// </summary>
        public byte[] Body { get; set; }
        public KxData kd { get; set; }

        public SpecialOrder ecode { get; set; }
    }
}


