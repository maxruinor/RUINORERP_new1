using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 性能监控服务接口
    /// </summary>
    public interface IPerformanceMonitoringService
    {
        /// <summary>
        /// 获取详细的性能报告
        /// </summary>
        /// <returns>性能报告</returns>
        string GetPerformanceReport();

        /// <summary>
        /// 获取实时监控数据
        /// </summary>
        /// <returns>实时监控数据</returns>
        RealTimeMonitoringData GetRealTimeData();
    }
}