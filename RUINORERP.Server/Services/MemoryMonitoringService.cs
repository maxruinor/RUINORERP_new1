using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 内存监控服务
    /// </summary>
    public class MemoryMonitoringService : IDisposable
    {
        private readonly ILogger<MemoryMonitoringService> _logger;
        private readonly Timer _monitoringTimer;
        private readonly object _lockObject = new object();
        private bool _disposed = false;
        
        // 内存使用阈值（以MB为单位）
        public long WarningThreshold { get; set; } = 1024; // 1GB
        public long CriticalThreshold { get; set; } = 2048; // 2GB

        // Phase 3.3 优化：自动垃圾回收配置
        private long _autoGCThreshold = 1536; // 1.5GB 时自动GC
        private long _lastGCTime = 0; // 上次GC时间（Unix时间戳）
        private const int GC_COOLDOWN_SECONDS = 300; // GC冷却时间：5分钟
        private const int MAX_GC_ATTEMPTS_PER_HOUR = 12; // 每小时最多GC 12次
        
        // 内存监控事件
        public event EventHandler<MemoryUsageEventArgs> MemoryUsageWarning;
        public event EventHandler<MemoryUsageEventArgs> MemoryUsageCritical;
        public event EventHandler<MemoryUsageEventArgs> MemoryUsageNormal;
        
        public MemoryMonitoringService(ILogger<MemoryMonitoringService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // 每30秒检查一次内存使用情况
            _monitoringTimer = new Timer(MonitorMemoryUsage, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }
        
        /// <summary>
        /// 获取当前内存使用情况
        /// </summary>
        /// <returns></returns>
        public MemoryUsageInfo GetCurrentMemoryUsage()
        {
            var process = Process.GetCurrentProcess();
            var workingSet = process.WorkingSet64;
            var managedMemory = GC.GetTotalMemory(false);
            
            return new MemoryUsageInfo
            {
                WorkingSet = workingSet,
                ManagedMemory = managedMemory,
                WorkingSetMB = workingSet / (1024 * 1024),
                ManagedMemoryMB = managedMemory / (1024 * 1024),
                Timestamp = DateTime.UtcNow
            };
        }
        
        /// <summary>
        /// 监控内存使用情况
        /// </summary>
        /// <param name="state"></param>
        private void MonitorMemoryUsage(object state)
        {
            try
            {
                var memoryInfo = GetCurrentMemoryUsage();
                _logger.LogDebug($"内存使用情况 - 工作集: {memoryInfo.WorkingSetMB} MB, 托管内存: {memoryInfo.ManagedMemoryMB} MB");

                // Phase 3.3 优化：自动垃圾回收
                long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (memoryInfo.WorkingSetMB >= _autoGCThreshold &&
                    (currentTime - _lastGCTime) > GC_COOLDOWN_SECONDS)
                {
                    _logger.LogInformation($"内存使用达到自动GC阈值: {memoryInfo.WorkingSetMB} MB (阈值: {_autoGCThreshold} MB)");
                    PerformAutoGC();
                }

                // 根据内存使用情况触发相应事件
                if (memoryInfo.WorkingSetMB >= CriticalThreshold)
                {
                    _logger.LogWarning($"内存使用达到临界阈值: {memoryInfo.WorkingSetMB} MB");
                    MemoryUsageCritical?.Invoke(this, new MemoryUsageEventArgs(memoryInfo));
                }
                else if (memoryInfo.WorkingSetMB >= WarningThreshold)
                {
                    _logger.LogInformation($"内存使用达到警告阈值: {memoryInfo.WorkingSetMB} MB");
                    MemoryUsageWarning?.Invoke(this, new MemoryUsageEventArgs(memoryInfo));
                }
                else
                {
                    MemoryUsageNormal?.Invoke(this, new MemoryUsageEventArgs(memoryInfo));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "监控内存使用情况时发生错误");
            }
        }

        /// <summary>
        /// Phase 3.3 优化：执行自动垃圾回收
        /// </summary>
        private void PerformAutoGC()
        {
            try
            {
                _logger.LogInformation("开始自动垃圾回收");
                var beforeMemory = GetCurrentMemoryUsage();

                // 执行垃圾回收
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: true, compacting: true);
                GC.WaitForPendingFinalizers();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: true, compacting: true);

                var afterMemory = GetCurrentMemoryUsage();
                _lastGCTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                _logger.LogInformation($"自动垃圾回收完成 - 回收前: {beforeMemory.WorkingSetMB} MB, 回收后: {afterMemory.WorkingSetMB} MB, " +
                                      $"回收了 {(beforeMemory.WorkingSet - afterMemory.WorkingSet) / (1024 * 1024)} MB");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行自动垃圾回收时发生错误");
            }
        }
        
        /// <summary>
        /// 强制执行垃圾回收
        /// </summary>
        public void ForceGarbageCollection()
        {
            try
            {
                _logger.LogInformation("开始强制垃圾回收");
                var beforeMemory = GetCurrentMemoryUsage();
                
                // 执行垃圾回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                
                var afterMemory = GetCurrentMemoryUsage();
                _logger.LogInformation($"垃圾回收完成 - 回收前: {beforeMemory.WorkingSetMB} MB, 回收后: {afterMemory.WorkingSetMB} MB, " +
                                      $"回收了 {(beforeMemory.WorkingSet - afterMemory.WorkingSet) / (1024 * 1024)} MB");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行垃圾回收时发生错误");
            }
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 停止并释放定时器
                    _monitoringTimer?.Change(Timeout.Infinite, Timeout.Infinite);
                    _monitoringTimer?.Dispose();
                }
                
                _disposed = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    
    /// <summary>
    /// 内存使用信息
    /// </summary>
    public class MemoryUsageInfo
    {
        public long WorkingSet { get; set; }
        public long ManagedMemory { get; set; }
        public long WorkingSetMB { get; set; }
        public long ManagedMemoryMB { get; set; }
        public DateTime Timestamp { get; set; }
    }
    
    /// <summary>
    /// 内存使用事件参数
    /// </summary>
    public class MemoryUsageEventArgs : EventArgs
    {
        public MemoryUsageInfo MemoryInfo { get; }
        
        public MemoryUsageEventArgs(MemoryUsageInfo memoryInfo)
        {
            MemoryInfo = memoryInfo;
        }
    }
}