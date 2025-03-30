using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    // 扩展方法
    public static class DateTimeHelper
    {
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            return dt.StartOfWeek(startOfWeek).AddDays(6).AddTicks(-1);
        }

        //public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        //{
        //    return dt.StartOfWeek(startOfWeek).AddDays(6).Date.AddDays(1).AddTicks(-1);
        //}

        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }






        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

  





        /// <summary>
        /// 获取当前日期所在季度的第一天
        /// </summary>
        /// <example>2023-05-15 -> 2023-04-01</example>
        public static DateTime FirstDayOfQuarter(this DateTime date)
        {
            // 计算季度序号（1-4）
            int quarter = (date.Month - 1) / 3 + 1;

            // 计算季度起始月份（1,4,7,10）
            int startMonth = (quarter - 1) * 3 + 1;

            return new DateTime(date.Year, startMonth, 1);
        }


        //public static DateTime LastDayOfQuarter(this DateTime dt)
        //{
        //    return dt.FirstDayOfQuarter().AddMonths(3).AddTicks(-1);
        //}

        /// <summary>
        /// 获取当前日期所在季度的最后一天
        /// </summary>
        /// <example>2023-08-20 -> 2023-09-30</example>
        public static DateTime LastDayOfQuarter(this DateTime date)
        {
            // 计算季度序号（1-4）
            int quarter = (date.Month - 1) / 3 + 1;

            // 计算季度末月（3,6,9,12）
            int endMonth = quarter * 3;

            // 获取该月最后一天
            int endDay = DateTime.DaysInMonth(date.Year, endMonth);

            return new DateTime(date.Year, endMonth, endDay);
        }

    }

}
