using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.InvReminder
{
    // 配置读取类
    public class InventoryMonitorConfig
    {
        public TimeSpan InitialDelay { get; set; }
        public TimeSpan CheckInterval { get; set; }
        public TimeSpan EmergencyCheckInterval { get; set; }
        public bool EnableRealTime { get; set; }
    }
}
