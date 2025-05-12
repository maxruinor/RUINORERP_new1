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
        public static List<NotificationChannel> ParseChannels(string typeFlags)
        {
            List<NotificationChannel> channels = new List<NotificationChannel>();
            if (!string.IsNullOrEmpty(typeFlags))
            {
                var flags = typeFlags.Split(',');
                foreach (var flag in flags)
                {
                    if (Enum.TryParse(flag, out NotificationChannel channel))
                    {
                        channels.Add(channel);
                    }
                }
            }
            return channels;
        }
    }
}
