using System;

namespace HLH.Lib.Helper
{
    public class DateTimeHelper
    {

        /// <summary>
        /// 转为本地时间
        /// </summary>
        /// <param name="GMTWithTimeZone">20170614222054000-0700</param>
        /// <returns></returns>
        public static DateTime GetLocTime(string GMTWithTimeZone)
        {
            //格林威治标准时间
            //太平洋时区是西八区，GMT-0800
            string refresh_token_timeout = GMTWithTimeZone;
            string[] times = refresh_token_timeout.Split('-');
            DateTime timeRS = DateTime.ParseExact(times[0], "yyyyMMddHHmmssfff", new System.Globalization.CultureInfo("zh-CN", true), System.Globalization.DateTimeStyles.AllowInnerWhite);
            return timeRS;
        }


        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            // DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            // long lTime = long.Parse(timeStamp + "0000000");
            // TimeSpan toNow = new TimeSpan(lTime);
            //return dtStart.Add(toNow);

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long lTime = long.Parse(timeStamp);
            //  TimeSpan toNow = new TimeSpan(lTime);
            DateTime nowTime = startTime.AddMilliseconds(lTime);
            return nowTime;
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = time;
            //DateTime nowTime = DateTime.Now;
            //DateTime timeStamp = DateTime.Parse("2013-11-25 20:23:30");
            //以上面这个为标准的话，这个是结果：1385382210000
            // DateTime nowTime = timeStamp;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);

            return unixTime;
            //  System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            //  return (int)(time - startTime).TotalSeconds;
        }


        public static long GetCurrentTimeStamp()
        {
            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(DateTime.Now - dt).TotalSeconds;
        }

        public static long GetTimeStamp(DateTime dt)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(dt - dtStart).TotalSeconds;
        }

        /*
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }*/

    }
}
