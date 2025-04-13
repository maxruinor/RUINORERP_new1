using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
    public class BlacklistEntry
    {
        public string IP地址 { get; set; }
        public DateTime 解封时间 { get; set; }
        public string 剩余时间 => (解封时间 - DateTime.Now).ToString(@"hh\:mm\:ss");
    }
}
