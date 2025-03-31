using RUINORERP.UI.ChartAnalyzer;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Contracts
{

    /// <summary>
    /// 图表数据源通用接口
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 获取图表数据
        /// </summary>
        /// <param name="request">图表请求参数</param>
        public Task<ChartData> GetDataAsync(DataRequest request);
        
        /// <summary>
        /// 获取可用指标配置
        /// </summary>
        public IEnumerable<MetricConfig> GetMetrics();
        public IEnumerable<DimensionConfig> GetDimensions();
        public enum FieldType { String, Number, DateTime, Boolean }

    }







}
