using System;
using System.Globalization;


namespace RUINORERP.Common.Extensions

{


    /// <summary>
    /// 时间扩展
    /// </summary>
    public static partial class ExtObject
    {

 
        /// <summary>
        /// 将指定的时间处理为当天的结束时间。
        /// </summary>
        /// <param name="date">指定的日期部分。</param>
        /// <returns>一个元组，第一个元素是当天的开始时间（00:00:00），第二个元素是当天的结束时间（23:59:59）。</returns>
        public static DateTime GetDayTimeEnd(this DateTime date)
        {
            // 设置时间为当天的开始
            DateTime startOfDay = date.Date; // 这将时间设置为 00:00:00
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1); // 这将时间设置为 23:59:59.9999999 (DateTime 的最大值)
            return endOfDay;
        }


        /// <summary>
        /// 将指定的时间处理为当天的开始和结束时间。
        /// </summary>
        /// <param name="date">指定的日期部分。</param>
        /// <returns>一个元组，第一个元素是当天的开始时间（00:00:00），第二个元素是当天的结束时间（23:59:59）。</returns>
        public static (DateTime startOfDay, DateTime endOfDay) GetDayTimeRange(this DateTime date)
        {
            // 设置时间为当天的开始
            DateTime startOfDay = date.Date; // 这将时间设置为 00:00:00
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1); // 这将时间设置为 23:59:59.9999999 (DateTime 的最大值)
            return (startOfDay, endOfDay);
        }



        ///   <summary> 
        ///  获取某一日期是该年中的第几周
        ///   </summary> 
        ///   <param name="dateTime"> 日期 </param> 
        ///   <returns> 该日期在该年中的周数 </returns> 
        public static int GetWeekOfYear(this DateTime dateTime)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// 获取Js格式的timestamp
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static long ToJsTimestamp(this DateTime dateTime)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            long result = (dateTime.Ticks - startTime.Ticks) / 10000; //除10000调整为13位
            return result;
        }

        /// <summary>
        /// 获取js中的getTime()
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static Int64 JsGetTime(this DateTime dt)
        {
            Int64 retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = dt.ToUniversalTime() - st;
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        /// <summary>
        /// 返回默认时间1970-01-01
        /// </summary>
        /// <param name="dt">时间日期</param>
        /// <returns></returns>
        public static DateTime Default(this DateTime dt)
        {
            return DateTime.Parse("1970-01-01");
        }



        /// <summary>
        /// 转为本地时间
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static DateTime ToLocalTime(this DateTime time)
        {
            return TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Local);
        }

        /// <summary>
        /// 转为转换为Unix时间戳格式(精确到秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static int ToUnixTimeStampSecond(this DateTime time)
        {
            DateTime startTime = new DateTime(1970, 1, 1).ToLocalTime();
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 转为转换为Unix时间戳格式(精确到毫秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static long ToUnixTimeStampMillisecond(this DateTime time)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime();
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}