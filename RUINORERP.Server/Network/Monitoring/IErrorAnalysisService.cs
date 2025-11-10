using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 错误分析服务接口
    /// </summary>
    public interface IErrorAnalysisService
    {
        /// <summary>
        /// 获取错误分析报告
        /// </summary>
        /// <returns>错误分析报告</returns>
        string GetErrorAnalysisReport();
        
        /// <summary>
        /// 重置统计信息
        /// </summary>
        void ResetStatistics();
    }
}