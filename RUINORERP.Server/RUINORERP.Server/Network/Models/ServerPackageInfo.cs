using RUINORERP.PacketSpec.Models.Core;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Models
{
    public class ServerPackageInfo : PacketModel,IKeyedPackageInfo<string>
    {
        public string Key { get; set; }
    }
}
