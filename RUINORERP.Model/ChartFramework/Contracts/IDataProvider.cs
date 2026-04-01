using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ChartFramework.Contracts
{
    /// <summary>
    /// 图表数据提供者接口
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 获取图表数据
        /// </summary>
        /// <param name="request">图表请求参数</param>
        Task<ChartData> GetDataAsync(DataRequest request);
        
        /// <summary>
        /// 获取可用指标配置
        /// </summary>
        IEnumerable<MetricConfig> GetMetrics();
        
        /// <summary>
        /// 获取可用维度配置
        /// </summary>
        IEnumerable<DimensionConfig> GetDimensions();
    }
}
