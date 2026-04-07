using System;
using System.Collections.Generic;
using RUINORERP.Model.Base.StatusManager.PerformanceMonitoring;

namespace RUINORERP.Server.Network.Monitoring
{
    /// <summary>
    /// 单据锁定监控数据收集器
    /// 负责收集和聚合服务器端单据锁定系统的性能指标
    /// </summary>
    public class DocumentLockMetricsCollector
    {
        private readonly object _lockObject = new object();
        
        // 累计统计（从服务启动开始）
        private long _totalLockTimeouts = 0;
        private long _totalLockAcquires = 0;
        private long _totalLockConflicts = 0;
        private long _totalBroadcasts = 0;
        private long _totalBroadcastSuccesses = 0;
        private long _totalBroadcastFailures = 0;
        
        // 锁持有时间统计
        private double _totalLockHoldTimeSeconds = 0;
        private double _maxLockHoldTimeSeconds = 0;
        private int _lockHoldTimeSampleCount = 0;

        /// <summary>
        /// 记录锁获取成功
        /// </summary>
        public void RecordLockAcquired()
        {
            lock (_lockObject)
            {
                _totalLockAcquires++;
            }
        }

        /// <summary>
        /// 记录锁冲突（被其他用户锁定）
        /// </summary>
        public void RecordLockConflict()
        {
            lock (_lockObject)
            {
                _totalLockConflicts++;
            }
        }

        /// <summary>
        /// 记录锁超时
        /// </summary>
        public void RecordLockTimeout()
        {
            lock (_lockObject)
            {
                _totalLockTimeouts++;
            }
        }

        /// <summary>
        /// 记录锁释放（用于计算锁持有时间）
        /// </summary>
        /// <param name="holdTimeSeconds">锁持有时间（秒）</param>
        public void RecordLockReleased(double holdTimeSeconds)
        {
            lock (_lockObject)
            {
                _totalLockHoldTimeSeconds += holdTimeSeconds;
                _lockHoldTimeSampleCount++;
                
                if (holdTimeSeconds > _maxLockHoldTimeSeconds)
                {
                    _maxLockHoldTimeSeconds = holdTimeSeconds;
                }
            }
        }

        /// <summary>
        /// 记录广播发送
        /// </summary>
        /// <param name="success">是否成功</param>
        public void RecordBroadcast(bool success)
        {
            lock (_lockObject)
            {
                _totalBroadcasts++;
                if (success)
                {
                    _totalBroadcastSuccesses++;
                }
                else
                {
                    _totalBroadcastFailures++;
                }
            }
        }

        /// <summary>
        /// 收集当前时刻的监控指标
        /// </summary>
        /// <param name="activeLockCount">当前活跃锁数量</param>
        /// <param name="expiredLockCount">过期锁数量</param>
        /// <param name="orphanedLockCount">孤儿锁数量</param>
        /// <param name="pendingUnlockRequestCount">待处理解锁请求数量</param>
        /// <returns>单据锁定性能指标</returns>
        public DocumentLockMetric CollectMetrics(
            int activeLockCount,
            int expiredLockCount,
            int orphanedLockCount,
            int pendingUnlockRequestCount)
        {
            lock (_lockObject)
            {
                var metric = new DocumentLockMetric
                {
                    Timestamp = DateTime.Now,
                    ClientId = "Server", // 服务器端指标
                    MachineName = Environment.MachineName,
                    
                    // 实时状态
                    ActiveLockCount = activeLockCount,
                    ExpiredLockCount = expiredLockCount,
                    OrphanedLockCount = orphanedLockCount,
                    PendingUnlockRequestCount = pendingUnlockRequestCount,
                    
                    // 累计统计
                    LockTimeoutCount = _totalLockTimeouts,
                    LockAcquireSuccessCount = _totalLockAcquires,
                    LockConflictCount = _totalLockConflicts,
                    
                    // 广播统计
                    BroadcastTotalCount = _totalBroadcasts,
                    BroadcastSuccessCount = _totalBroadcastSuccesses,
                    BroadcastFailedCount = _totalBroadcastFailures,
                    
                    // 锁持有时间统计
                    AverageLockHoldTimeSeconds = _lockHoldTimeSampleCount > 0 
                        ? Math.Round(_totalLockHoldTimeSeconds / _lockHoldTimeSampleCount, 2) 
                        : 0,
                    MaxLockHoldTimeSeconds = (long)Math.Round(_maxLockHoldTimeSeconds, 2)
                };

                return metric;
            }
        }

        /// <summary>
        /// 重置统计数据
        /// </summary>
        public void ResetStatistics()
        {
            lock (_lockObject)
            {
                _totalLockTimeouts = 0;
                _totalLockAcquires = 0;
                _totalLockConflicts = 0;
                _totalBroadcasts = 0;
                _totalBroadcastSuccesses = 0;
                _totalBroadcastFailures = 0;
                _totalLockHoldTimeSeconds = 0;
                _maxLockHoldTimeSeconds = 0;
                _lockHoldTimeSampleCount = 0;
            }
        }
    }
}
