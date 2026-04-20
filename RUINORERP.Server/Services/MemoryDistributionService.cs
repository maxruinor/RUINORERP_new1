using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.SmartReminder;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 内存分布统计服务
    /// 用于按功能模块统计内存占用情况，帮助定位内存占用热点
    /// </summary>
    public class MemoryDistributionService : IDisposable
    {
        private readonly ILogger<MemoryDistributionService> _logger;
        private readonly ISessionService _sessionService;
        private readonly ServerLockManager _lockManager;
        private readonly SmartReminderMonitor _smartReminderMonitor;
        private readonly Timer _analysisTimer;
        private bool _disposed = false;

        private MemoryDistributionSnapshot _lastSnapshot;
        private readonly object _lockObject = new object();

        public MemoryDistributionService(
            ILogger<MemoryDistributionService> logger,
            ISessionService sessionService = null,
            ServerLockManager lockManager = null,
            SmartReminderMonitor smartReminderMonitor = null)
        {
            _logger = logger;
            _sessionService = sessionService;
            _lockManager = lockManager;
            _smartReminderMonitor = smartReminderMonitor;

            _analysisTimer = new Timer(AnalyzeMemoryDistribution, null, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// 获取当前内存分布快照
        /// </summary>
        public MemoryDistributionSnapshot GetCurrentSnapshot()
        {
            lock (_lockObject)
            {
                return _lastSnapshot ?? CreateSnapshot();
            }
        }

        /// <summary>
        /// 立即分析内存分布
        /// </summary>
        public MemoryDistributionSnapshot AnalyzeNow()
        {
            return CreateSnapshot();
        }

        private void AnalyzeMemoryDistribution(object state)
        {
            try
            {
                var snapshot = CreateSnapshot();
                lock (_lockObject)
                {
                    _lastSnapshot = snapshot;
                }

                _logger.LogDebug("内存分布分析完成 - 总托管内存: {TotalManaged} MB", snapshot.TotalManagedMemoryMB);

                if (snapshot.TotalManagedMemoryMB > 500)
                {
                    _logger.LogWarning("托管内存占用较高: {Total} MB", snapshot.TotalManagedMemoryMB);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分析内存分布时发生错误");
            }
        }

        private MemoryDistributionSnapshot CreateSnapshot()
        {
            var snapshot = new MemoryDistributionSnapshot
            {
                Timestamp = DateTime.Now,
                TotalManagedMemoryMB = GC.GetTotalMemory(false) / (1024 * 1024),
                TotalWorkingSetMB = Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024)
            };

            try
            {
                snapshot.ModuleStatistics = new List<ModuleMemoryStatistic>
                {
                    GetSessionServiceMemory(),
                    GetLockManagerMemory(),
                    GetSmartReminderMemory(),
                    GetGeneralMemoryInfo()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建内存快照时发生错误");
            }

            return snapshot;
        }

        private ModuleMemoryStatistic GetSessionServiceMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "会话管理(SessionService)",
                Description = "客户端会话连接、SessionInfo对象、DataQueue等"
            };

            try
            {
                if (_sessionService != null)
                {
                    var sessionCount = _sessionService.ActiveSessionCount;
                    stat.ObjectCount = sessionCount;
                    
                    stat.EstimatedMemoryMB = EstimateSessionMemory(sessionCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取会话服务内存统计失败");
            }

            return stat;
        }

        private long EstimateSessionMemory(int sessionCount)
        {
            long baseMemory = 50;
            long perSessionMemory = 2;
            return baseMemory + (sessionCount * perSessionMemory);
        }

        private ModuleMemoryStatistic GetLockManagerMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "单据锁定(ServerLockManager)",
                Description = "文档锁定信息、孤儿锁检测"
            };

            try
            {
                if (_lockManager != null)
                {
                    var lockCount = GetLockCount();
                    stat.ObjectCount = lockCount;
                    stat.EstimatedMemoryMB = 1 + (long)(lockCount * 0.1);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取锁管理器内存统计失败");
            }

            return stat;
        }

        private long GetLockCount()
        {
            try
            {
                if (_lockManager != null)
                {
                    var method = _lockManager.GetType().GetMethod("GetLockItemCount");
                    if (method != null)
                    {
                        return (int)method.Invoke(_lockManager, null);
                    }
                }
            }
            catch
            {
            }
            return 0;
        }

        private ModuleMemoryStatistic GetSmartReminderMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "智能提醒(SmartReminder)",
                Description = "提醒规则、库存缓存、策略引擎"
            };

            try
            {
                if (_smartReminderMonitor != null)
                {
                    stat.ObjectCount = 0;
                    stat.EstimatedMemoryMB = 10;
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取智能提醒内存统计失败");
            }

            return stat;
        }

        private ModuleMemoryStatistic GetGeneralMemoryInfo()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "运行时基础内存",
                Description = "CLR运行时、字符串池、程序集加载等基础内存"
            };

            var gcInfo = GC.GetGCMemoryInfo();
            stat.TotalAvailableMB = gcInfo.TotalAvailableMemoryBytes / (1024 * 1024);
            stat.EstimatedMemoryMB = 100;

            var gen0Collections = GC.CollectionCount(0);
            var gen1Collections = GC.CollectionCount(1);
            var gen2Collections = GC.CollectionCount(2);

            stat.Description += $" | GC次数: Gen0={gen0Collections}, Gen1={gen1Collections}, Gen2={gen2Collections}";

            return stat;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _analysisTimer?.Dispose();
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// 内存分布快照
    /// </summary>
    public class MemoryDistributionSnapshot
    {
        public DateTime Timestamp { get; set; }
        public long TotalManagedMemoryMB { get; set; }
        public long TotalWorkingSetMB { get; set; }
        public List<ModuleMemoryStatistic> ModuleStatistics { get; set; } = new List<ModuleMemoryStatistic>();
    }

    /// <summary>
    /// 模块内存统计
    /// </summary>
    public class ModuleMemoryStatistic
    {
        public string ModuleName { get; set; }
        public string Description { get; set; }
        public long ObjectCount { get; set; }
        public long EstimatedMemoryMB { get; set; }
        public long TotalAvailableMB { get; set; }
    }
}
