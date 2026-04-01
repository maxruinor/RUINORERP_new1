using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Services.Statistics
{
    /// <summary>
    /// 动态 SQL 构建器
    /// </summary>
    public class DynamicSqlBuilder
    {
        private ISugarQueryable<dynamic> _query;
        private readonly ISqlSugarClient _db;

        public DynamicSqlBuilder(ISqlSugarClient db)
        {
            _db = db;
        }

        /// <summary>
        /// 指定数据源
        /// </summary>
        public DynamicSqlBuilder From(string tableName)
        {
            _query = _db.Queryable<dynamic>(tableName);
            return this;
        }

        /// <summary>
        /// 选择维度字段
        /// </summary>
        public DynamicSqlBuilder SelectDimensions(List<string> dimensions)
        {
            if (dimensions == null || !dimensions.Any())
                return this;

            // 构建 SELECT 子句的维度部分
            var selectExpr = string.Join(", ", dimensions);
            _query = _query.Select<dynamic>(selectExpr);
            return this;
        }

        /// <summary>
        /// 选择聚合指标
        /// </summary>
        public DynamicSqlBuilder SelectMetrics(List<string> metrics)
        {
            if (metrics == null || !metrics.Any())
                return this;

            // 根据指标类型构建聚合表达式
            var aggExprs = new List<string>();
            foreach (var metric in metrics)
            {
                // TODO: 需要根据配置确定聚合类型 (SUM/COUNT/AVG 等)
                // 这里默认使用 SUM
                aggExprs.Add($"SUM({metric}) AS {metric}");
            }

            var selectExpr = string.Join(", ", aggExprs);
            _query = _query.Select<dynamic>(selectExpr);
            return this;
        }

        /// <summary>
        /// 应用时间范围过滤
        /// </summary>
        public DynamicSqlBuilder WhereTimeRange(string timeField, DateTime? startTime, DateTime? endTime)
        {
            if (string.IsNullOrWhiteSpace(timeField))
                return this;

            var sb = new StringBuilder();
            var parameters = new Dictionary<string, object>();

            if (startTime.HasValue)
            {
                sb.Append($"{timeField} >= @StartTime");
                parameters["StartTime"] = startTime.Value;
            }

            if (endTime.HasValue)
            {
                if (sb.Length > 0)
                    sb.Append(" AND ");
                sb.Append($"{timeField} <= @EndTime");
                parameters["EndTime"] = endTime.Value;
            }

            if (sb.Length > 0)
            {
                _query = _query.Where(sb.ToString(), parameters);
            }

            return this;
        }

        /// <summary>
        /// 应用自定义过滤条件
        /// </summary>
        public DynamicSqlBuilder ApplyFilters(Dictionary<string, object> filters)
        {
            if (filters == null || !filters.Any())
                return this;

            foreach (var filter in filters)
            {
                var condition = $"{filter.Key} = @{filter.Key}";
                _query = _query.Where(condition, new Dictionary<string, object> { [filter.Key] = filter.Value });
            }

            return this;
        }

        /// <summary>
        /// 应用分组
        /// </summary>
        public DynamicSqlBuilder GroupByDimensions(List<string> dimensions)
        {
            if (dimensions == null || !dimensions.Any())
                return this;

            var groupExpr = string.Join(", ", dimensions);
            _query = _query.GroupBy(groupExpr);
            return this;
        }

        /// <summary>
        /// 应用分页
        /// </summary>
        public DynamicSqlBuilder Page(int pageIndex, int pageSize)
        {
            if (pageIndex <= 0 || pageSize <= 0)
                return this;

            _query = _query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return this;
        }

        /// <summary>
        /// 获取可查询对象
        /// </summary>
        public ISugarQueryable<dynamic> Build()
        {
            return _query;
        }
    }
}
