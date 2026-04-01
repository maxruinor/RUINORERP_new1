using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Services.Statistics
{
    /// <summary>
    /// 统计查询参数
    /// </summary>
    public class StatisticsQuery
    {
        /// <summary>
        /// 实体类型 (如"SaleOrder","Inventory")
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 时间字段名称
        /// </summary>
        public string TimeField { get; set; } = "Created_at";

        /// <summary>
        /// 分组维度列表
        /// </summary>
        public List<string> Dimensions { get; set; } = new List<string>();

        /// <summary>
        /// 聚合指标列表
        /// </summary>
        public List<string> Metrics { get; set; } = new List<string>();

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 自定义过滤条件
        /// </summary>
        public Dictionary<string, object> Filters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 页码 (用于分页)
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; } = 1000;

        /// <summary>
        /// 验证查询参数有效性
        /// </summary>
        public bool Validate(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(EntityType))
            {
                errorMessage = "必须指定实体类型";
                return false;
            }

            if (Metrics.Count == 0)
            {
                errorMessage = "必须至少选择一个聚合指标";
                return false;
            }

            if (Dimensions.Count > 5)
            {
                errorMessage = "最多支持 5 个分组维度";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }

    /// <summary>
    /// 维度信息
    /// </summary>
    public class DimensionInfo
    {
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public Type FieldType { get; set; }
    }

    /// <summary>
    /// 指标信息
    /// </summary>
    public class MetricInfo
    {
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public MetricType AggregationType { get; set; }
        public string Unit { get; set; }
    }
}
