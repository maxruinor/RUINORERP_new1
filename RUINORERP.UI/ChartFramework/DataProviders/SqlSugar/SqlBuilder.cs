using RUINORERP.UI.ChartFramework.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.SqlSugar
{
    // SQL构建器
    [Obsolete("使用SqlSugar的Queryable 链式调用")]
    public class SqlBuilder
    {
        private readonly StringBuilder _selectClause = new();
        private readonly StringBuilder _fromClause = new();
        private readonly StringBuilder _whereClause = new();
        private readonly StringBuilder _groupByClause = new();
        private readonly List<SugarParameter> _parameters = new();
        private bool _hasWhere;

        public (string sql, SugarParameter[] parameters) Build()
        {
            var sql = $"""
            {_selectClause}
            {_fromClause}
            {(_whereClause.Length > 0 ? _whereClause : "")}
            {(_groupByClause.Length > 0 ? _groupByClause : "")}
            """;

            return (sql.Trim(), _parameters.ToArray());
        }

        public SqlBuilder WithTimeRange(DataRequest request)
        {
            if (request.StartTime.HasValue || request.EndTime.HasValue)
            {
                AddWhereClause();

                if (request.StartTime.HasValue && request.EndTime.HasValue)
                {
                    _whereClause.Append($"{request.TimeField} BETWEEN @start AND @end");
                    _parameters.Add(new SugarParameter("@start", request.StartTime));
                    _parameters.Add(new SugarParameter("@end", request.EndTime));
                }
                else if (request.StartTime.HasValue)
                {
                    _whereClause.Append($"{request.TimeField} >= @start");
                    _parameters.Add(new SugarParameter("@start", request.StartTime));
                }
                else
                {
                    _whereClause.Append($"{request.TimeField} <= @end");
                    _parameters.Add(new SugarParameter("@end", request.EndTime));
                }
            }
            return this;
        }

        public SqlBuilder WithDimensions(IEnumerable<string> dimensions)
        {
            if (dimensions?.Any() == true)
            {
                // 处理带表别名的字段（如 "Customer.Name"）
                var processedDims = dimensions.Select(d =>
                    d.Contains('.') ? d : $"t.{d}");

                _selectClause.AppendLine(
                    $", {string.Join(", ", processedDims.Select(d => $"{d} AS {SanitizeAlias(d)}"))}");

                _groupByClause.AppendLine(
                    $"GROUP BY {string.Join(", ", processedDims)}");
            }
            return this;
        }

        public SqlBuilder WithMetrics(IEnumerable<string> metrics)
        {
            if (metrics?.Any() == true)
            {
                _selectClause.Append("SELECT ");
                _selectClause.AppendLine(string.Join(", ",
                    metrics.Select(m => GetMetricExpression(m))));
            }
            return this;
        }

        public SqlBuilder WithFilters(IEnumerable<FieldFilter> filters)
        {
            if (filters?.Any() == true)
            {
                foreach (var filter in filters)
                {
                    AddWhereClause();

                    var paramName = $"@p{_parameters.Count}";
                    _whereClause.Append($"{filter.Field} {GetOperator(filter.Field)} {paramName}");
                    _parameters.Add(new SugarParameter(paramName, filter.Value));
                }
            }
            return this;
        }

        public SqlBuilder WithMainTable(string tableName)
        {
            _fromClause.AppendLine($"FROM {tableName} t");
            return this;
        }

        public SqlBuilder WithJoin(string joinClause)
        {
            _fromClause.AppendLine(joinClause);
            return this;
        }

        private void AddWhereClause()
        {
            _whereClause.Append(_hasWhere ? " AND " : "WHERE ");
            _hasWhere = true;
        }

        private static string GetMetricExpression(string metric)
        {
            return metric switch
            {
                "Count" => "COUNT(1) AS Count",
                "Sum" => "SUM(Amount) AS Sum",
                "Avg" => "AVG(Amount) AS Avg",
                "Max" => "MAX(Amount) AS Max",
                "Min" => "MIN(Amount) AS Min",
                _ when metric.StartsWith("Sum_") =>
                    $"SUM({metric[4]}) AS {metric}",
                _ => metric
            };
        }

        private static string GetOperator(string input)
        {
            return input.ToUpper() switch
            {
                "GT" => ">",
                "LT" => "<",
                "EQ" => "=",
                "NEQ" => "<>",
                "GTE" => ">=",
                "LTE" => "<=",
                "LIKE" => "LIKE",
                _ => input
            };
        }

        private static string SanitizeAlias(string input)
        {
            // 将 "Customer.Name" 转为 "Customer_Name"
            return input.Replace(".", "_");
        }
    }
}
