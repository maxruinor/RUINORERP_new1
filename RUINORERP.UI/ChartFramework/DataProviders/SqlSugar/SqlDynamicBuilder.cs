using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.SqlSugar
{
    // Data/Builders/SqlDynamicBuilder.cs
    public class SqlDynamicBuilder
    {
        public (string Sql, List<SugarParameter> Parameters) BuildQuery(DataRequest request)
        {
            var parameters = new List<SugarParameter>();
            var sql = new StringBuilder("SELECT ");

            // 构建SELECT部分
            var selects = new List<string>();
            foreach (var dim in request.Dimensions)
            {
                selects.Add($"{dim} AS {SanitizeColumnName(dim)}");
            }

            foreach (var metric in request.Metrics)
            {
                selects.Add($"{GetAggregateExpression(metric)} AS {metric}");
            }
            sql.Append(string.Join(", ", selects));

            // 构建FROM（简化示例）
           // sql.Append(" FROM " + GetTableName(request));

            // 构建WHERE
            if (request.Filters.Any() || request.StartTime.HasValue)
            {
                sql.Append(" WHERE ");
                var conditions = new List<string>();

                if (request.StartTime.HasValue)
                {
                    parameters.Add(new SugarParameter("@start", request.StartTime));
                    conditions.Add($"{request.TimeField} >= @start");
                }

                foreach (var filter in request.Filters)
                {
                    parameters.Add(new SugarParameter($"@p{parameters.Count}", filter.Value));
                    conditions.Add($"{filter.Field} {filter.Operator} @p{parameters.Count - 1}");
                }

                sql.Append(string.Join(" AND ", conditions));
            }

            // 构建GROUP BY
            if (request.Dimensions.Any())
            {
                sql.Append(" GROUP BY " + string.Join(", ", request.Dimensions));
            }

            return (sql.ToString(), parameters);
        }

        private string GetAggregateExpression(string metric)
        {
            return metric switch
            {
                "Count" => "COUNT(1)",
                "Amount" => "SUM(Amount)",
                _ => $"SUM({metric})"
            };
        }

        private string SanitizeColumnName(string name) => name.Replace(".", "_");

 

    }
}
