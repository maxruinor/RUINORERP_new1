using System;

namespace RUINORERP.UI.Network.TimeoutStatistics
{
    /// <summary>
    /// 命令超时统计详情类
    /// 记录特定命令类型的超时相关统计信息
    /// </summary>
    public class CommandTimeoutStats
    {
        /// <summary>
        /// 命令类型ID
        /// </summary>
        public string CommandId { get; internal set; }
        
        /// <summary>
        /// 超时次数
        /// </summary>
        public int TimeoutCount { get; set; }
        
        /// <summary>
        /// 总请求次数
        /// </summary>
        public int TotalRequestCount { get; set; }
        
        /// <summary>
        /// 成功请求次数
        /// </summary>
        public int SuccessRequestCount { get; set; }
        
        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageProcessingTimeMs { get; set; }
        
        /// <summary>
        /// 最大处理时间（毫秒）
        /// </summary>
        public long MaxProcessingTimeMs { get; set; }
        
        /// <summary>
        /// 最小处理时间（毫秒）
        /// </summary>
        public long MinProcessingTimeMs { get; set; }
        
        /// <summary>
        /// 最近一次请求时间
        /// </summary>
        public DateTime LastRequestTime { get; set; }
        
        /// <summary>
        /// 超时率
        /// </summary>
        public double TimeoutRate
        {
            get
            {
                if (TotalRequestCount == 0)
                    return 0;
                return (double)TimeoutCount / TotalRequestCount * 100;
            }
        }
        
        /// <summary>
        /// 成功率
        /// </summary>
        public double SuccessRate
        {
            get
            {
                if (TotalRequestCount == 0)
                    return 0;
                return (double)SuccessRequestCount / TotalRequestCount * 100;
            }
        }
    }
}