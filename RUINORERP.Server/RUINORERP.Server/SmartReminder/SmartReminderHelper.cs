using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    public static class SmartReminderHelper
    {
        public static List<NotifyChannel> ParseChannels(string typeFlags)
        {
            List<NotifyChannel> channels = new List<NotifyChannel>();
            if (!string.IsNullOrEmpty(typeFlags))
            {
                var flags = typeFlags.Split(',');
                foreach (var flag in flags)
                {
                    if (Enum.TryParse(flag, out NotifyChannel channel))
                    {
                        channels.Add(channel);
                    }
                }
            }
            return channels;
        }
    }
}
