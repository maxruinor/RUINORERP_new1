using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Services.Statistics
{
    /// <summary>
    /// 统计服务接口
    /// </summary>
    public interface IStatisticsService
    {
        /// <summary>
        /// 执行统计查询
        /// </summary>
        /// <param name="query">统计查询参数</param>
        /// <returns>图表数据</returns>
        Task<ChartData> ExecuteQueryAsync(StatisticsQuery query);

        /// <summary>
        /// 获取可用维度
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>维度列表</returns>
        IEnumerable<DimensionInfo> GetDimensions(string entityType);

        /// <summary>
        /// 获取可用指标
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>指标列表</returns>
        IEnumerable<MetricInfo> GetMetrics(string entityType);
    }
}
