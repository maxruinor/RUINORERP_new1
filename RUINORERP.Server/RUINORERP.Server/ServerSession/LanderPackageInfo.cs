using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Server.ServerSession
{
    public class LanderPackageInfo : IKeyedPackageInfo<string>
    {
        public string Key { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        //        public byte[] Body { get; set; }
        public string Body { get; set; }
    }

}
