using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 实时监控数据
    /// </summary>
    public class RealTimeMonitoringData
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 处理器总数
        /// </summary>
        public int TotalHandlers { get; set; }

        /// <summary>
        /// 活跃处理器数
        /// </summary>
        public int ActiveHandlers { get; set; }

        /// <summary>
        /// 总处理命令数
        /// </summary>
        public long TotalCommandsProcessed { get; set; }

        /// <summary>
        /// 当前正在处理的命令数
        /// </summary>
        public long CurrentProcessing { get; set; }

        /// <summary>
        /// 成功率（百分比）
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageProcessingTime { get; set; }
    }
}