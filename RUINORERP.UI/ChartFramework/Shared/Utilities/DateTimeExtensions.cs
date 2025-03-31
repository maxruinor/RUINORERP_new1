using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Shared.Utilities
{
    public static class DateTimeExtensions
    {
        // 获取ISO周数
        public static int GetWeekOfYear(this DateTime date)
        {
            return ISOWeekHelper.GetWeekOfYear(date);
        }

        // 获取周的显示名称(如"04/10-04/16")
        public static string GetWeekDisplayName(this DateTime date)
        {
            var start = date.Date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
            var end = start.AddDays(6);
            return $"{start:MM/dd}-{end:MM/dd}";
        }
    }
}
