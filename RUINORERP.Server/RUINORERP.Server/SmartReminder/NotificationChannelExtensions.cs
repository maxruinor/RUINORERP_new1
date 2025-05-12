using Microsoft.VisualBasic.ApplicationServices;
using RUINORERP.Global.EnumExt;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    // 添加 NotificationChannel 扩展方法
    public static class NotificationChannelExtensions
    {
        public static List<string> ToChannelList(this List<NotificationChannel> channels)
        {
            return channels?.Select(c => c.ToString()).ToList() ?? new List<string>();
        }

        public static List<NotificationChannel> FromChannelList(this List<string> channelStrings)
        {
            return channelStrings?.ConvertAll(s =>
                (NotificationChannel)Enum.Parse(typeof(NotificationChannel), s)) ?? new List<NotificationChannel>();
        }
    }

    // 添加 User 类扩展方法
    public static class UserExtensions
    {
        public static string GetRecipientForChannel(this User user, NotificationChannel channel)
        {
            return channel switch
            {
                //NotificationChannel.Email => user.Email,
                //NotificationChannel.SMS => user.PhoneNumber,
                //NotificationChannel.Realtime => user.UserId.ToString(),
                _ => throw new NotSupportedException($"不支持的通知通道: {channel}")
            };
        }
    }

    // 添加 Exception 扩展方法
    public static class ExceptionExtensions
    {
        public static bool IsCritical(this Exception ex)
        {
            return ex is SqlException ||
                   ex is RedisConnectionException                   ;
        }
    }
}
