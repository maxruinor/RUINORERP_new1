using SuperSocket.ProtoBase;
using TransInstruction;
using TransInstruction.DataPortal;
using TransInstruction.Enums;

namespace RUINORERP.UI.SuperSocketClient
{



    public class BizPackageInfo : IPackageInfo<string>
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
        public OriginalData od { get; set; }

        public SpecialOrder ecode { get; set; }
    }
}



