using RUINORERP.Global;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RUINORERP.PacketSpec.Models.Lock
{
    /// <summary>
    /// 锁定信息统计类
    /// 用于存储和传输锁定系统的统计数据
    /// </summary>
    [DataContract]
    public class LockInfoStatistics
    {
        /// <summary>
        /// 总锁定数
        /// </summary>
        [DataMember]
        public int TotalLocks { get; set; }

        /// <summary>
        /// 活跃锁定数
        /// </summary>
        [DataMember]
        public int ActiveLocks { get; set; }

        /// <summary>
        /// 过期锁定数
        /// </summary>
        [DataMember]
        public int ExpiredLocks { get; set; }

        /// <summary>
        /// 请求解锁的锁定数
        /// </summary>
        [DataMember]
        public int RequestingUnlock { get; set; }

        /// <summary>
        /// 锁定的用户数
        /// </summary>
        [DataMember]
        public int LocksByUser { get; set; }

        /// <summary>
        /// 按业务类型统计的锁定数
        /// </summary>
        [DataMember]
        public Dictionary<BizType, int> LocksByBizType { get; set; }

        /// <summary>
        /// 按状态统计的锁定数
        /// </summary>
        [DataMember]
        public Dictionary<LockStatus, int> LocksByStatus { get; set; }

        /// <summary>
        /// 历史记录数
        /// </summary>
        [DataMember]
        public int HistoryRecordCount { get; set; }

        /// <summary>
        /// 监控数据
        /// </summary>
        [DataMember]
        public LockMonitorSnapshot MonitorData { get; set; }

        /// <summary>
        /// 服务版本信息
        /// </summary>
        [DataMember]
        public string ServiceVersion { get; set; }

        /// <summary>
        /// 版本日期
        /// </summary>
        [DataMember]
        public string VersionDate { get; set; }

        /// <summary>
        /// 版本特性
        /// </summary>
        [DataMember]
        public string VersionFeatures { get; set; }

        /// <summary>
        /// 平均锁定年龄（分钟）
        /// </summary>
        [DataMember]
        public double AverageLockAge { get; set; }

        /// <summary>
        /// 是否启用心跳
        /// </summary>
        [DataMember]
        public bool HeartbeatEnabled { get; set; }

        /// <summary>
        /// 最后清理时间
        /// </summary>
        [DataMember]
        public System.DateTime LastCleanup { get; set; }

        /// <summary>
        /// 内存使用量（MB）
        /// </summary>
        [DataMember]
        public double MemoryUsage { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LockInfoStatistics()
        {
            LocksByBizType = new Dictionary<BizType, int>();
            LocksByStatus = new Dictionary<LockStatus, int>();
        }
    }

    /// <summary>
    /// 锁定监控快照
    /// 用于存储锁定系统的监控数据
    /// </summary>
    [DataContract]
    public class LockMonitorSnapshot
    {
        /// <summary>
        /// 总添加锁定数
        /// </summary>
        [DataMember]
        public int TotalLocksAdded { get; set; }

        /// <summary>
        /// 总移除锁定数
        /// </summary>
        [DataMember]
        public int TotalLocksRemoved { get; set; }

        /// <summary>
        /// 总过期锁定数
        /// </summary>
        [DataMember]
        public int TotalLocksExpired { get; set; }

        /// <summary>
        /// 当前并发锁定数
        /// </summary>
        [DataMember]
        public int CurrentConcurrentLocks { get; set; }

        /// <summary>
        /// 峰值并发锁定数
        /// </summary>
        [DataMember]
        public int PeakConcurrentLocks { get; set; }

        /// <summary>
        /// 最后重置时间
        /// </summary>
        [DataMember]
        public System.DateTime LastResetTime { get; set; }
    }

    /// <summary>
    /// 锁定历史记录
    /// 用于记录锁定操作的历史信息
    /// </summary>
    [DataContract]
    public class LockHistoryRecord
    {
        /// <summary>
        /// 操作时间
        /// </summary>
        [DataMember]
        public System.DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [DataMember]
        public LockHistoryAction Action { get; set; }

        /// <summary>
        /// 单据ID
        /// </summary>
        [DataMember]
        public long BillID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public long UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 锁定类型
        /// </summary>
        [DataMember]
        public string LockType { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public string Remarks { get; set; }
    }

    /// <summary>
    /// 锁定历史操作类型
    /// </summary>
    public enum LockHistoryAction
    {
        /// <summary>
        /// 创建锁定
        /// </summary>
        Created,

        /// <summary>
        /// 更新锁定
        /// </summary>
        Updated,

        /// <summary>
        /// 移除锁定
        /// </summary>
        Removed,

        /// <summary>
        /// 强制解锁
        /// </summary>
        ForceUnlocked,

        /// <summary>
        /// 请求解锁
        /// </summary>
        RequestedUnlock
    }




}
