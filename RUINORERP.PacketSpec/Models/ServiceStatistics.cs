using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 统一服务统计信息
    /// </summary>
    public class ServiceStatistics
    {

  
        public bool IsInitialized { get; set; }
        public long TotalPacketsProcessed { get; set; }
        public DateTime LastProcessTime { get; set; }

        /// <summary>
        /// 服务启动时间
        /// </summary>
        public DateTime ServiceStartTime { get; set; }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// 总接收包数
        /// </summary>
        public long TotalPacketsReceived { get; set; }

        /// <summary>
        /// 总发送包数
        /// </summary>
        public long TotalPacketsSent { get; set; }

        /// <summary>
        /// 总成功处理数
        /// </summary>
        public long TotalSuccessfulProcesses { get; set; }

        /// <summary>
        /// 总错误数
        /// </summary>
        public long TotalErrors { get; set; }

        /// <summary>
        /// 总广播数
        /// </summary>
        public long TotalBroadcasts { get; set; }

        /// <summary>
        /// 总创建会话数
        /// </summary>
        public long TotalSessionsCreated { get; set; }

        /// <summary>
        /// 活跃会话数
        /// </summary>
        public int ActiveSessionCount { get; set; }

        /// <summary>
        /// 注册的处理器数量
        /// </summary>
        public int RegisteredHandlerCount { get; set; }

        /// <summary>
        /// 最后处理包时间
        /// </summary>
        public DateTime LastPacketProcessTime { get; set; }

        /// <summary>
        /// 服务运行时间
        /// </summary>
        public TimeSpan Uptime => DateTime.UtcNow - ServiceStartTime;

        /// <summary>
        /// 成功率
        /// </summary>
        public double SuccessRate => TotalPacketsReceived > 0
            ? (double)TotalSuccessfulProcesses / TotalPacketsReceived * 100
            : 0;

        /// <summary>
        /// 平均处理速度（包/秒）
        /// </summary>
        public double AverageProcessingRate => Uptime.TotalSeconds > 0
            ? TotalPacketsReceived / Uptime.TotalSeconds
            : 0;
    }
}
