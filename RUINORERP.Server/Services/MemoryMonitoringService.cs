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
        public long WarningThreshold { get; set; } = 2048; // 2GB（告警阈值）
        public long CriticalThreshold { get; set; } = 3072; // 3GB（临界阈值）
        
        // ✅ 简化：移除自动GC配置，改为纯监控告警
        // 如需手动GC，请调用 ForceGarbageCollection() 方法
        
        // 自动 Dump 配置
        private long _dumpThreshold = 4096; // 4GB 时自动触发 Dump（降低频率，避免频繁Dump）
        private string _dumpPath = "D:\\Dumps";
        private bool _isDumping = false;
        private long _lastDumpTime = 0; // 上次Dump时间
        private const int DUMP_COOLDOWN_HOURS = 4; // Dump冷却时间：4小时（降低频率）
        
        // 内存压力状态（解决高内存导致用户掉线问题）
        private bool _isUnderMemoryPressure = false;
        private DateTime _lastMemoryPressureNotification = DateTime.MinValue;
        
        // 内存监控事件
        public event EventHandler<MemoryUsageEventArgs> MemoryUsageWarning;
        public event EventHandler<MemoryUsageEventArgs> MemoryUsageCritical;
        public event EventHandler<MemoryUsageEventArgs> MemoryUsageNormal;
        
        /// <summary>
        /// 内存压力事件 - 高内存时通知相关服务采取措施防止用户掉线
        /// </summary>
        public event EventHandler<MemoryPressureEventArgs> MemoryPressureIncreased;
        
        /// <summary>
        /// 当前是否处于内存压力状态
        /// </summary>
        public bool IsUnderMemoryPressure => _isUnderMemoryPressure;
        
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

                // 注意：不再自动触发GC，高频GC会导致CPU升高和响应延迟
                // 内存压力应该通过增加超时容忍度等机制来缓解

                // 根据内存使用情况触发相应事件
                if (memoryInfo.WorkingSetMB >= CriticalThreshold)
                {
                    _logger.LogWarning($"内存使用达到临界阈值: {memoryInfo.WorkingSetMB} MB");
                    MemoryUsageCritical?.Invoke(this, new MemoryUsageEventArgs(memoryInfo));
                    
                    // 触发内存压力事件（解决高内存导致用户掉线问题）
                    HandleMemoryPressure(memoryInfo.WorkingSetMB, CriticalThreshold, MemoryPressureLevel.Critical);
                    
                    // 检查是否达到自动 Dump 阈值
                    if (memoryInfo.WorkingSetMB >= _dumpThreshold && !_isDumping)
                    {
                        Task.Run(() => TriggerAutoDump(memoryInfo.WorkingSetMB));
                    }
                }
                else if (memoryInfo.WorkingSetMB >= WarningThreshold)
                {
                    _logger.LogInformation($"内存使用达到警告阈值: {memoryInfo.WorkingSetMB} MB");
                    MemoryUsageWarning?.Invoke(this, new MemoryUsageEventArgs(memoryInfo));
                    
                    // 触发内存压力事件（解决高内存导致用户掉线问题）
                    HandleMemoryPressure(memoryInfo.WorkingSetMB, WarningThreshold, MemoryPressureLevel.Warning);
                }
                else
                {
                    MemoryUsageNormal?.Invoke(this, new MemoryUsageEventArgs(memoryInfo));
                    
                    // 恢复正常，关闭内存压力状态
                    if (_isUnderMemoryPressure)
                    {
                        _isUnderMemoryPressure = false;
                        _logger.LogInformation("内存使用恢复正常，内存压力状态已解除");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "监控内存使用情况时发生错误");
            }
        }
        
        /// <summary>
        /// 处理内存压力状态 - 通知相关服务采取措施防止用户掉线
        /// </summary>
        /// <param name="currentMemoryMB">当前内存使用（MB）</param>
        /// <param name="thresholdMB">触发阈值（MB）</param>
        /// <param name="pressureLevel">压力级别</param>
        private void HandleMemoryPressure(long currentMemoryMB, long thresholdMB, MemoryPressureLevel pressureLevel)
        {
            // 避免频繁触发事件（每分钟最多一次）
            if ((DateTime.Now - _lastMemoryPressureNotification).TotalMinutes < 1)
            {
                return;
            }
            
            _lastMemoryPressureNotification = DateTime.Now;
            _isUnderMemoryPressure = pressureLevel == MemoryPressureLevel.Critical || 
                                     (pressureLevel == MemoryPressureLevel.Warning && currentMemoryMB >= CriticalThreshold);
            
            _logger.LogWarning($"内存压力增加: {currentMemoryMB} MB, 阈值: {thresholdMB} MB, 级别: {pressureLevel}");
            
            // 触发内存压力事件，让订阅者可以采取相应措施
            MemoryPressureIncreased?.Invoke(this, new MemoryPressureEventArgs(currentMemoryMB, thresholdMB, pressureLevel));
        }

        /// <summary>
        /// 触发自动 Dump
        /// </summary>
        private async Task TriggerAutoDump(long currentMemoryMB)
        {
            // 检查冷却时间
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 3600; // 转换为小时
            if (currentTime - _lastDumpTime < DUMP_COOLDOWN_HOURS)
            {
                _logger.LogDebug("自动 Dump处于冷却期内，跳过本次 Dump，上次 Dump时间: {LastDumpTime}小时后", currentTime - _lastDumpTime);
                return;
            }

            lock (_lockObject)
            {
                if (_isDumping) return;
                _isDumping = true;
            }

            try
            {
                _lastDumpTime = currentTime;
                _logger.LogWarning("==================== 自动Dump启动 ====================");
                _logger.LogWarning($"内存达到 {currentMemoryMB} MB，正在后台触发自动 Dump...");
                _logger.LogWarning($"Dump文件将保存至: {_dumpPath}");
                _logger.LogWarning($"冷却时间: {DUMP_COOLDOWN_HOURS}小时");
                _logger.LogWarning("=====================================================");
                
                if (!System.IO.Directory.Exists(_dumpPath))
                {
                    System.IO.Directory.CreateDirectory(_dumpPath);
                }

                var pid = Process.GetCurrentProcess().Id;
                var fileName = $"server_dump_{DateTime.Now:yyyyMMdd_HHmmss}_{currentMemoryMB}MB.dmp";
                var fullPath = System.IO.Path.Combine(_dumpPath, fileName);

                // 调用 dotnet-dump 工具
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"dump collect -p {pid} -t full -o \"{fullPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    // ✅ 添加超时控制(最多等待10分钟)
                    var timeoutTask = Task.Delay(TimeSpan.FromMinutes(10));
                    var exitTask = process.WaitForExitAsync();
                    
                    var completedTask = await Task.WhenAny(exitTask, timeoutTask);
                    
                    if (completedTask == timeoutTask)
                    {
                        _logger.LogError("自动Dump超时(10分钟),终止进程");
                        try
                        {
                            process.Kill();
                        }
                        catch { }
                        return;
                    }
                    
                    var output = await process.StandardOutput.ReadToEndAsync();
                    var error = await process.StandardError.ReadToEndAsync();
                    
                    if (process.ExitCode == 0)
                    {
                        _logger.LogInformation($"自动 Dump 成功: {fullPath}");
                    }
                    else
                    {
                        _logger.LogError($"自动 Dump 失败: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行自动 Dump 时发生异常");
            }
            finally
            {
                lock (_lockObject)
                {
                    _isDumping = false;
                }
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
                // 优化：使用 Compacting 模式强制压缩 LOH，减少工作集虚高
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: true, compacting: true);
                GC.WaitForPendingFinalizers();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: true, compacting: true);
                
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
    
    /// <summary>
    /// 内存压力事件参数
    /// </summary>
    public class MemoryPressureEventArgs : EventArgs
    {
        public long MemoryUsageMB { get; }
        public long ThresholdMB { get; }
        public MemoryPressureLevel PressureLevel { get; }
        
        public MemoryPressureEventArgs(long memoryUsageMB, long thresholdMB, MemoryPressureLevel pressureLevel)
        {
            MemoryUsageMB = memoryUsageMB;
            ThresholdMB = thresholdMB;
            PressureLevel = pressureLevel;
        }
    }
    
    /// <summary>
    /// 内存压力级别
    /// </summary>
    public enum MemoryPressureLevel
    {
        Normal = 0,
        Warning = 1,
        Critical = 2
    }
}