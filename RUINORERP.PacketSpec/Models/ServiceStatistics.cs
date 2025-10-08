using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 统一服务统计信息
    /// </summary>
    [MessagePackObject]
    public class ServiceStatistics
    {

  
        [Key(0)]
        public bool IsInitialized { get; set; }
        [Key(1)]
        public long TotalPacketsProcessed { get; set; }
        [Key(2)]
        public DateTime LastProcessTime { get; set; }

        /// <summary>
        /// 服务启动时间
        /// </summary>
        [Key(3)]
        public DateTime ServiceStartTime { get; set; }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        [Key(4)]
        public bool IsRunning { get; set; }

        /// <summary>
        /// 总接收包数
        /// </summary>
        [Key(5)]
        public long TotalPacketsReceived { get; set; }

        /// <summary>
        /// 总发送包数
        /// </summary>
        [Key(6)]
        public long TotalPacketsSent { get; set; }

        /// <summary>
        /// 总成功处理数
        /// </summary>
        [Key(7)]
        public long TotalSuccessfulProcesses { get; set; }

        /// <summary>
        /// 总错误数
        /// </summary>
        [Key(8)]
        public long TotalErrors { get; set; }

        /// <summary>
        /// 总广播数
        /// </summary>
        [Key(9)]
        public long TotalBroadcasts { get; set; }

        /// <summary>
        /// 总创建会话数
        /// </summary>
        [Key(10)]
        public long TotalSessionsCreated { get; set; }

        /// <summary>
        /// 活跃会话数
        /// </summary>
        [Key(11)]
        public int ActiveSessionCount { get; set; }

        /// <summary>
        /// 注册的处理器数量
        /// </summary>
        [Key(12)]
        public int RegisteredHandlerCount { get; set; }

        /// <summary>
        /// 最后处理包时间
        /// </summary>
        [Key(13)]
        public DateTime LastPacketProcessTime { get; set; }

        /// <summary>
        /// 服务运行时间
        /// </summary>
        [IgnoreMember]
        public TimeSpan Uptime => DateTime.UtcNow - ServiceStartTime;

        /// <summary>
        /// 成功率
        /// </summary>
        [IgnoreMember]
        public double SuccessRate => TotalPacketsReceived > 0
            ? (double)TotalSuccessfulProcesses / TotalPacketsReceived * 100
            : 0;

        /// <summary>
        /// 平均处理速度（包/秒）
        /// </summary>
        [IgnoreMember]
        public double AverageProcessingRate => Uptime.TotalSeconds > 0
            ? TotalPacketsReceived / Uptime.TotalSeconds
            : 0;
    }
}
