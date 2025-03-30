using RUINORERP.UI.ChartFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.SqlSugar
{
    public static class SqlTimeGroupBuilder
    {
        public static string BuildGroupExpression(string fieldName, TimeRangeType rangeType)
        {
            return rangeType switch
            {
                TimeRangeType.Daily => BuildDailyGroup(fieldName),
                TimeRangeType.Monthly => BuildMonthlyGroup(fieldName),
                _ => throw new NotSupportedException()
            };
        }

        private static string BuildDailyGroup(string field)
            => $"CONVERT(varchar(10), {field}, 120)";

        private static string BuildMonthlyGroup(string field)
            => $"CONVERT(varchar, YEAR({field})) + '-' + RIGHT('0' + CONVERT(varchar, MONTH({field})), 2)";

        /// <summary>
        /// 获取时间分组的SQL表达式
        /// GROUP BY   YEAR(Created_at), MONTH(Created_at)
        /// </summary>
        public static string GetGroupByTimeField(string TimeField, TimeRangeType TimeGroupType)
        {
            return TimeGroupType switch
            {
                TimeRangeType.Daily => $"CONVERT(varchar(10), {TimeField}, 120)", // YYYY-MM-DD
                TimeRangeType.Weekly => $"CONCAT(YEAR({TimeField}), '-', DATEPART(week, {TimeField}))",
                TimeRangeType.Monthly => $"MONTH({TimeField})",
                TimeRangeType.YearlyMonthly => $"YEAR({TimeField}), MONTH({TimeField})",
                TimeRangeType.Quarterly => $"CONCAT(YEAR({TimeField}), '-Q', DATEPART(quarter, {TimeField}))",
                TimeRangeType.Yearly => $"YEAR({TimeField})",
                _ => throw new ArgumentException("Unsupported time group type")
            };
        }

        /// <summary>
        /// 获取时间分组的SQL表达式
        /// </summary>
        //public static string GetQueryTimeFieldBySql()
        //{
        //    return TimeGroupType switch
        //    {
        //        TimeGranularity.Daily => $"CONVERT(varchar(10), {TimeField}, 120)", // YYYY-MM-DD
        //        TimeGranularity.Weekly => $"CONCAT(YEAR({TimeField}), '-', DATEPART(week, {TimeField}))",
        //        TimeGranularity.Monthly => $"CONVERT(varchar, YEAR({TimeField})) + '-' + RIGHT('0' + CONVERT(varchar, MONTH({TimeField})), 2)",
        //        TimeGranularity.Quarterly => $"CONCAT(YEAR({TimeField}), '-Q', DATEPART(quarter, {TimeField}))",
        //        TimeGranularity.Yearly => $"YEAR({TimeField})",
        //        _ => throw new ArgumentException("Unsupported time group type")
        //    };
        //}

    }
  



  
 
    // 使用示例：
    // var groupExpr = SqlTimeGroupBuilder.BuildGroupExpression("CreatedAt", TimeRangeType.Monthly);
}
