using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Globalization;
namespace RUINORERP.UI.ChartFramework.Shared.Utilities
{
    using System;
    using System.Globalization;

    /// <summary>
    /// ISO 8601 周数计算工具（兼容 .NET Framework 4.8）
    /// 严格遵循标准：每周从周一开始，第一周包含当年至少4天
    /// </summary>
    public static class ISOWeekHelper
    {
        private static readonly Calendar _calendar = CultureInfo.InvariantCulture.Calendar;
        private const CalendarWeekRule _weekRule = CalendarWeekRule.FirstFourDayWeek;
        private const DayOfWeek _firstDayOfWeek = DayOfWeek.Monday;

        /// <summary>
        /// 获取日期对应的 ISO 8601 周数（1-53）
        /// </summary>
        public static int GetWeekOfYear(DateTime date)
        {
            return _calendar.GetWeekOfYear(date, _weekRule, _firstDayOfWeek);
        }

        /// <summary>
        /// 获取日期对应的 ISO 8601 年份（可能与日期的年份不同）
        /// </summary>
        public static int GetYear(DateTime date)
        {
            int week = GetWeekOfYear(date);
            int year = date.Year;

            // 处理跨年周
            if (week >= 52 && date.Month == 1)
            {
                year--; // 属于前一年的第52/53周（例如：2023-01-01 可能是 2022-W52）
            }
            else if (week == 1 && date.Month == 12)
            {
                year++; // 属于下一年的第1周（例如：2022-12-31 可能是 2023-W01）
            }

            return year;
        }

        /// <summary>
        /// 生成标准周标识符（格式：YYYY-WWW，例如：2023-W42）
        /// </summary>
        public static string GetWeekKey(DateTime date)
        {
            int year = GetYear(date);
            int week = GetWeekOfYear(date);
            return $"{year}-W{week:D2}";
        }

        /// <summary>
        /// 从 ISO 周标识符解析日期范围（返回该周的周一和周日）
        /// </summary>
        public static (DateTime Start, DateTime End) ParseWeekKey(string weekKey)
        {
            if (string.IsNullOrEmpty(weekKey))
            {
                throw new ArgumentException("Week key cannot be empty.");
            }


            string[] parts = weekKey.Split('-');
            if (parts.Length != 2 || !parts[1].StartsWith("W", StringComparison.OrdinalIgnoreCase))
                throw new FormatException("Invalid ISO week format. Expected 'YYYY-WWW'.");

            int year = int.Parse(parts[0]);
            int week = int.Parse(parts[1].Substring(1));

            return GetWeekRange(year, week);
        }

        /// <summary>
        /// 获取指定 ISO 年份和周数的日期范围（周一至周日）
        /// </summary>
        public static (DateTime Start, DateTime End) GetWeekRange(int year, int week)
        {
            // 找到该年的第一个周四（ISO 标准定义：第一周包含该年的第一个周四）
            DateTime jan4 = new DateTime(year, 1, 4);
            int delta = DayOfWeek.Thursday - jan4.DayOfWeek;
            DateTime firstThursday = jan4.AddDays(delta);

            // 计算该年的第一个周一
            DateTime firstMonday = firstThursday.AddDays(-3);

            // 目标周的周一
            DateTime start = firstMonday.AddDays((week - 1) * 7);
            DateTime end = start.AddDays(6);

            // 验证周数是否有效（1-52或53）
            int maxWeek = GetWeekOfYear(new DateTime(year, 12, 31));
            if (week < 1 || week > maxWeek)
                throw new ArgumentOutOfRangeException(nameof(week), $"Week must be between 1 and {maxWeek} for year {year}.");

            return (start, end);
        }
    }
}
