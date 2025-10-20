using System;
using System.Collections.Generic;
using MessagePack;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 磁盘信息
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class DiskInfo
    {
        /// <summary>
        /// 磁盘名称
        /// </summary>
        [Key(0)]
        public string Name { get; set; }

        /// <summary>
        /// 总大小（GB）
        /// </summary>
        [Key(1)]
        public float TotalSize { get; set; }

        /// <summary>
        /// 可用空间（GB）
        /// </summary>
        [Key(2)]
        public float AvailableSpace { get; set; }

        /// <summary>
        /// 使用率（百分比）
        /// </summary>
        [Key(3)]
        public float UsagePercentage { get; set; }
    }

    /// <summary>
    /// 本地电脑状态信息
    /// </summary>
    public class LocalComputerStatus
    {
        /// <summary>
        /// CPU使用率
        /// </summary>
        public float CpuUsage { get; set; }

        /// <summary>
        /// 内存使用率
        /// </summary>
        public float MemoryUsage { get; set; }

        /// <summary>
        /// 磁盘使用情况
        /// </summary>
        public Dictionary<string, DiskInfo> DiskUsage { get; set; } = new Dictionary<string, DiskInfo>();

        /// <summary>
        /// 系统运行时间（秒）
        /// </summary>
        public long SystemUptime { get; set; }
    }
}