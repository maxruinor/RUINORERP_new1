using NodaTime;
using System;

namespace RUINOR.Framework.Core.Common.Extention

{

    /// <summary>
    /// GUID扩展
    /// </summary>
    public static partial class ExtObject
    {
        /// <summary>
        /// 转为有序的GUID
        /// 注：长度为50字符
        /// </summary>
        /// <param name="guid">新的GUID</param>
        /// <returns></returns>
        public static string ToSequentialGuid(this Guid guid)
        {
            var timeStr = (DateTime.Now.ToCstTime().Ticks / 10000).ToString("x8");
            var newGuid = $"{timeStr.PadLeft(13, '0')}-{guid}";

            return newGuid;
        }

        /// <summary>
        /// 将DateTime.Now转换成cstTime，解决在linux与windows中相差8个小时的问题
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ToCstTime(this DateTime time)
        {
            return GetCstDateTime(time);
        }

        private static DateTime GetCstDateTime(DateTime time)
        {
            //Instant now = SystemClock.Instance.GetCurrentInstant();
            Instant instant = Instant.FromDateTimeUtc(time.ToUniversalTime());
            var shanghaiZone = DateTimeZoneProviders.Tzdb["Asia/Shanghai"];
            return instant.InZone(shanghaiZone).ToDateTimeUnspecified();
        }

    }
}