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
using RUINORERP.Business.Cache;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Comm;
using RUINORERP.Model.BusinessImage;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 内存分布统计服务
    /// 用于按功能模块统计内存占用情况，帮助定位内存占用热点
    /// </summary>
    public class MemoryDistributionService : IDisposable
    {
        private readonly Timer _analysisTimer;
        private bool _disposed = false;

        private MemoryDistributionSnapshot _lastSnapshot;
        private readonly object _lockObject = new object();

        /// <summary>
        /// 使用属性注入获取依赖服务
        /// 这些服务可能为null，如果对应的服务未在DI容器中注册
        /// </summary>
        public ILogger<MemoryDistributionService> Logger { private get; set; }
        public ISessionService SessionService { private get; set; }
        public ServerLockManager LockManager { private get; set; }
        public SmartReminderMonitor SmartReminderMonitor { private get; set; }
        public IEntityCacheManager EntityCacheManager { private get; set; }
        public IStockCacheService StockCacheService { private get; set; }
        public IMemoryCache MemoryCache { private get; set; }
        public IRedisCacheService RedisCacheService { private get; set; }
        public ImageCacheServiceBase ImageCacheService { private get; set; }
        public FileStorageMonitorService FileStorageMonitorService { private get; set; }
        public PerformanceDataStorageService PerformanceDataStorageService { private get; set; }
        // 注意：CachedRuleEngineCenter 可能未注册，改用基类 RuleEngineCenter
        public RUINORERP.Server.SmartReminder.RuleEngineCenter RuleEngineCenter { private get; set; }

        /// <summary>
        /// 无参构造函数（用于Autofac属性注入）
        /// </summary>
        public MemoryDistributionService()
        {
            _analysisTimer = new Timer(AnalyzeMemoryDistribution, null, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// 诊断：检查各依赖服务是否已注入
        /// </summary>
        public string GetDependencyStatus()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Logger: {(Logger != null ? "已注入" : "null")}");
            sb.AppendLine($"SessionService: {(SessionService != null ? "已注入" : "null")}");
            sb.AppendLine($"LockManager: {(LockManager != null ? "已注入" : "null")}");
            sb.AppendLine($"SmartReminderMonitor: {(SmartReminderMonitor != null ? "已注入" : "null")}");
            sb.AppendLine($"EntityCacheManager: {(EntityCacheManager != null ? "已注入" : "null")}");
            sb.AppendLine($"StockCacheService: {(StockCacheService != null ? "已注入" : "null")}");
            sb.AppendLine($"MemoryCache: {(MemoryCache != null ? "已注入" : "null")}");
            sb.AppendLine($"RedisCacheService: {(RedisCacheService != null ? "已注入" : "null")}");
            sb.AppendLine($"ImageCacheService: {(ImageCacheService != null ? "已注入" : "null")}");
            sb.AppendLine($"FileStorageMonitorService: {(FileStorageMonitorService != null ? "已注入" : "null")}");
            sb.AppendLine($"PerformanceDataStorageService: {(PerformanceDataStorageService != null ? "已注入" : "null")}");
            sb.AppendLine($"RuleEngineCenter: {(RuleEngineCenter != null ? "已注入" : "null")}");
            return sb.ToString();
        }

        /// <summary>
        /// 获取内部logger（兼容旧代码）
        /// </summary>
        private ILogger<MemoryDistributionService> _logger => Logger;

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
                    GetEntityCacheMemory(),
                    GetIMemoryCacheMemory(),
                    GetRedisCacheMemory(),
                    GetImageCacheMemory(),
                    GetSessionServiceMemory(),
                    GetLockManagerMemory(),
                    GetSmartReminderMemory(),
                    GetRuleEngineMemory(),
                    GetPerformanceDataStorageMemory(),
                    GetFileStorageMonitorMemory(),
                    GetCacheSyncMetadataMemory(),
                    GetGeneralMemoryInfo()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建内存快照时发生错误");
            }

            return snapshot;
        }

        /// <summary>
        /// 获取实体缓存管理器的详细内存统计
        /// </summary>
        private ModuleMemoryStatistic GetEntityCacheMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "实体缓存(EntityCacheManager)",
                Description = "使用CacheManager.Core，按表名分组统计"
            };

            try
            {
                if (EntityCacheManager != null)
                {
                    // 获取总体统计
                    stat.ObjectCount = EntityCacheManager.CacheItemCount;
                    stat.EstimatedMemoryMB = EntityCacheManager.EstimatedCacheSize / (1024 * 1024);
                    
                    // 获取按表名分组的详细统计
                    var tableStats = EntityCacheManager.GetTableCacheStatistics();
                    if (tableStats != null && tableStats.Count > 0)
                    {
                        stat.SubItems = new List<SubItemStatistic>();
                        foreach (var kvp in tableStats.OrderByDescending(x => x.Value.EstimatedTotalSize))
                        {
                            var tableStat = kvp.Value;
                            stat.SubItems.Add(new SubItemStatistic
                            {
                                Name = tableStat.TableName,
                                Description = $"列表:{tableStat.EntityListCount}, 实体:{tableStat.EntityCount}, 显示值:{tableStat.DisplayValueCount}",
                                ObjectCount = tableStat.TotalItemCount,
                                EstimatedMemoryKB = tableStat.EstimatedTotalSize / 1024,
                                HitRatio = tableStat.HitRatio
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取实体缓存内存统计失败");
            }

            return stat;
        }

        /// <summary>
        /// 获取IMemoryCache的内存统计（包含StockCache、ImageCache等）
        /// </summary>
        private ModuleMemoryStatistic GetIMemoryCacheMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "内存缓存(IMemoryCache)",
                Description = "共享缓存池：库存缓存、图片缓存、规则引擎缓存等"
            };

            try
            {
                // 获取StockCacheService的详细统计
                if (StockCacheService != null)
                {
                    var stockStats = StockCacheService.GetCacheStatistics();
                    stat.SubItems = new List<SubItemStatistic>();
                    
                    stat.SubItems.Add(new SubItemStatistic
                    {
                        Name = "库存缓存(StockCache)",
                        Description = $"命中:{stockStats.CacheHits}, 未命中:{stockStats.CacheMisses}, 命中率:{stockStats.HitRatio:P2}",
                        ObjectCount = stockStats.CurrentCacheSize,
                        EstimatedMemoryKB = EstimateStockCacheMemory(stockStats.CurrentCacheSize)
                    });
                }

                // 估算其他使用IMemoryCache的服务
                // ImageCacheService、SmartReminderMonitor、CachedRuleEngineCenter等都使用同一个IMemoryCache实例
                // 由于IMemoryCache没有直接提供大小查询，这里基于配置和对象数量进行估算
                long totalEstimatedKB = stat.SubItems?.Sum(s => s.EstimatedMemoryKB) ?? 0;
                
                // 如果已经有子项统计，尝试估算总占用
                if (stat.SubItems != null && stat.SubItems.Count > 0)
                {
                    // 假设StockCache占总IMemoryCache的60%，其他服务占40%
                    long estimatedTotalKB = (long)(totalEstimatedKB / 0.6);
                    stat.EstimatedMemoryMB = estimatedTotalKB / 1024;
                    
                    stat.SubItems.Add(new SubItemStatistic
                    {
                        Name = "其他缓存(估算)",
                        Description = "图片缓存、规则引擎缓存、智能提醒缓存等",
                        ObjectCount = 0,
                        EstimatedMemoryKB = estimatedTotalKB - totalEstimatedKB
                    });
                }
                else
                {
                    stat.EstimatedMemoryMB = 50; // 默认估算值
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取IMemoryCache内存统计失败");
            }

            return stat;
        }

        /// <summary>
        /// 估算库存缓存的内存占用
        /// </summary>
        private long EstimateStockCacheMemory(int cacheSize)
        {
            // 每个tb_Inventory对象约占用2-3KB（包含属性和对象头）
            const long bytesPerItem = 2500;
            return (cacheSize * bytesPerItem) / 1024; // 转换为KB
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
                if (SessionService != null)
                {
                    var sessionCount = SessionService.ActiveSessionCount;
                    stat.ObjectCount = sessionCount;
                    
                    // 尝试获取实际的数据队列占用
                    long totalDataQueueSize = 0;
                    int totalQueuedMessages = 0;
                    
                    try
                    {
                        // 通过反射获取所有会话的 DataQueue 大小
                        var sessionServiceType = SessionService.GetType();
                        System.Reflection.MemberInfo sessionsMember = sessionServiceType.GetProperty("Sessions");
                        if (sessionsMember == null)
                            sessionsMember = sessionServiceType.GetField("Sessions");
                        
                        if (sessionsMember != null)
                        {
                            object sessions = null;
                            if (sessionsMember is System.Reflection.PropertyInfo prop)
                                sessions = prop.GetValue(SessionService);
                            else if (sessionsMember is System.Reflection.FieldInfo field)
                                sessions = field.GetValue(SessionService);
                            
                            if (sessions is System.Collections.IEnumerable sessionsEnum && sessionsEnum != null)
                            {
                                foreach (var session in sessionsEnum)
                                {
                                    var dataQueueProperty = session.GetType().GetProperty("DataQueue");
                                    if (dataQueueProperty != null)
                                    {
                                        var dataQueue = dataQueueProperty.GetValue(session) as System.Collections.ICollection;
                                        if (dataQueue != null)
                                        {
                                            totalQueuedMessages += dataQueue.Count;
                                            // 估算每个消息平均 1KB
                                            totalDataQueueSize += dataQueue.Count * 1024;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // 如果反射失败，使用默认估算
                    }
                    
                    // 基础内存 + 会话对象 + 数据队列
                    long baseMemory = 50 * 1024 * 1024; // 50MB 基础
                    long perSessionMemory = sessionCount * 2 * 1024 * 1024; // 每会话 2MB
                    stat.EstimatedMemoryMB = (baseMemory + perSessionMemory + totalDataQueueSize) / (1024 * 1024);
                    
                    if (totalQueuedMessages > 0)
                    {
                        stat.SubItems = new List<SubItemStatistic>
                        {
                            new SubItemStatistic
                            {
                                Name = "数据队列(DataQueue)",
                                Description = $"总计 {totalQueuedMessages} 条待发送消息",
                                ObjectCount = totalQueuedMessages,
                                EstimatedMemoryKB = totalDataQueueSize / 1024
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取会话服务内存统计失败");
            }

            return stat;
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
                if (LockManager != null)
                {
                    var lockCount = GetLockCount();
                    stat.ObjectCount = lockCount;
                    
                    // 每个 LockInfo 对象约占用 500 字节
                    long estimatedBytes = lockCount * 500;
                    stat.EstimatedMemoryMB = Math.Max(1, estimatedBytes / (1024 * 1024));
                    
                    if (lockCount > 0)
                    {
                        stat.SubItems = new List<SubItemStatistic>
                        {
                            new SubItemStatistic
                            {
                                Name = "活动锁",
                                Description = $"当前有 {lockCount} 个文档被锁定",
                                ObjectCount = lockCount,
                                EstimatedMemoryKB = estimatedBytes / 1024
                            }
                        };
                    }
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
                if (LockManager != null)
                {
                    var method = LockManager.GetType().GetMethod("GetLockItemCount");
                    if (method != null)
                    {
                        return (int)method.Invoke(LockManager, null);
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
                if (SmartReminderMonitor != null)
                {
                    // 获取智能提醒监控的详细统计
                    var monitorType = SmartReminderMonitor.GetType();
                    
                    // 尝试获取规则数量
                    System.Reflection.MemberInfo rulesMember = monitorType.GetProperty("RuleCount");
                    if (rulesMember == null)
                        rulesMember = monitorType.GetField("RuleCount");
                    
                    long ruleCount = 0;
                    if (rulesMember != null)
                    {
                        object value = null;
                        if (rulesMember is System.Reflection.PropertyInfo prop)
                            value = prop.GetValue(SmartReminderMonitor);
                        else if (rulesMember is System.Reflection.FieldInfo field)
                            value = field.GetValue(SmartReminderMonitor);
                        
                        if (value != null)
                        {
                            ruleCount = Convert.ToInt64(value);
                        }
                    }
                    
                    stat.ObjectCount = ruleCount;
                    
                    // 估算内存占用：每个规则约 10KB
                    long estimatedBytes = ruleCount * 10 * 1024;
                    stat.EstimatedMemoryMB = Math.Max(5, estimatedBytes / (1024 * 1024));
                    
                    if (ruleCount > 0)
                    {
                        stat.SubItems = new List<SubItemStatistic>
                        {
                            new SubItemStatistic
                            {
                                Name = "提醒规则",
                                Description = $"已加载 {ruleCount} 条提醒规则",
                                ObjectCount = ruleCount,
                                EstimatedMemoryKB = estimatedBytes / 1024
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取智能提醒内存统计失败");
            }

            return stat;
        }

        /// <summary>
        /// 获取缓存同步元数据管理器的内存统计
        /// </summary>
        private ModuleMemoryStatistic GetCacheSyncMetadataMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "缓存同步(CacheSyncMetadata)",
                Description = "分布式缓存同步元数据、失效标记等"
            };

            try
            {
                // 尝试通过 EntityCacheManager 访问内部的 CacheSyncMetadataManager
                if (EntityCacheManager != null)
                {
                    var cacheManagerType = EntityCacheManager.GetType();
                    var syncMetadataField = cacheManagerType.GetField("_cacheSyncMetadata", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    
                    if (syncMetadataField != null)
                    {
                        var syncMetadata = syncMetadataField.GetValue(EntityCacheManager);
                        if (syncMetadata != null)
                        {
                            var syncType = syncMetadata.GetType();
                            
                            // 尝试获取 Count 属性
                            var countProperty = syncType.GetProperty("Count");
                            if (countProperty != null)
                            {
                                var countValue = countProperty.GetValue(syncMetadata);
                                if (countValue != null)
                                {
                                    long metadataCount = Convert.ToInt64(countValue);
                                    stat.ObjectCount = metadataCount;
                                    
                                    // 每个 CacheSyncInfo 约占用 1KB
                                    long estimatedBytes = metadataCount * 1024;
                                    stat.EstimatedMemoryMB = Math.Max(1, estimatedBytes / (1024 * 1024));
                                    
                                    if (metadataCount > 0)
                                    {
                                        stat.SubItems = new List<SubItemStatistic>
                                        {
                                            new SubItemStatistic
                                            {
                                                Name = "同步元数据",
                                                Description = $"跟踪 {metadataCount} 个缓存项的同步状态",
                                                ObjectCount = metadataCount,
                                                EstimatedMemoryKB = estimatedBytes / 1024
                                            }
                                        };
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取缓存同步元数据统计失败");
            }

            return stat;
        }

        /// <summary>
        /// 获取Redis缓存统计
        /// </summary>
        private ModuleMemoryStatistic GetRedisCacheMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "Redis缓存(IRedisCacheService)",
                Description = "分布式缓存：会话Token、业务数据缓存等"
            };

            try
            {
                if (RedisCacheService != null)
                {
                    var redisStats = RedisCacheService.GetStatistics();
                    stat.ObjectCount = redisStats.TotalRequests;
                    
                    // 估算：每个缓存项约2KB
                    long estimatedBytes = redisStats.TotalRequests * 2 * 1024;
                    stat.EstimatedMemoryMB = Math.Max(1, estimatedBytes / (1024 * 1024));
                    
                    stat.Description = $"总请求:{redisStats.TotalRequests}, 命中:{redisStats.HitCount}, 未命中:{redisStats.MissCount}, 命中率:{redisStats.HitRate:P1}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取Redis缓存统计失败");
            }

            return stat;
        }

        /// <summary>
        /// 获取图片缓存统计
        /// </summary>
        private ModuleMemoryStatistic GetImageCacheMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "图片缓存(ImageCacheService)",
                Description = "本地内存缓存：图片信息、缩略图等"
            };

            try
            {
                if (ImageCacheService != null)
                {
                    // 图片缓存使用IMemoryCache，通过反射获取内部缓存条目数
                    var cacheType = ImageCacheService.GetType();
                    var memoryCacheField = cacheType.GetField("_memoryCache", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    
                    if (memoryCacheField != null)
                    {
                        var memoryCache = memoryCacheField.GetValue(ImageCacheService) as IMemoryCache;
                        if (memoryCache != null)
                        {
                            // 尝试获取缓存大小
                            var countProp = memoryCache.GetType().GetProperty("Count");
                            if (countProp != null)
                            {
                                var countValue = countProp.GetValue(memoryCache);
                                if (countValue != null)
                                {
                                    stat.ObjectCount = Convert.ToInt64(countValue);
                                    // 每张图片信息约5KB估算
                                    long estimatedBytes = stat.ObjectCount * 5 * 1024;
                                    stat.EstimatedMemoryMB = Math.Max(1, estimatedBytes / (1024 * 1024));
                                    stat.Description = $"缓存 {stat.ObjectCount} 张图片信息";
                                }
                            }
                        }
                    }
                    
                    if (stat.ObjectCount == 0)
                    {
                        // 无法获取时使用默认估算
                        stat.EstimatedMemoryMB = 10;
                        stat.Description = "本地内存缓存：图片信息、缩略图等";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取图片缓存统计失败");
            }

            return stat;
        }

        /// <summary>
        /// 获取规则引擎内存统计
        /// </summary>
        private ModuleMemoryStatistic GetRuleEngineMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "规则引擎(CachedRuleEngineCenter)",
                Description = "业务规则缓存、决策树等"
            };

            try
            {
                if (RuleEngineCenter != null)
                {
                    // 规则引擎使用IMemoryCache，尝试获取缓存条目数
                    var engineType = RuleEngineCenter.GetType();
                    var cacheField = engineType.GetField("_cache", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    
                    if (cacheField != null)
                    {
                        var memoryCache = cacheField.GetValue(RuleEngineCenter) as IMemoryCache;
                        if (memoryCache != null)
                        {
                            var countProp = memoryCache.GetType().GetProperty("Count");
                            if (countProp != null)
                            {
                                var countValue = countProp.GetValue(memoryCache);
                                if (countValue != null)
                                {
                                    stat.ObjectCount = Convert.ToInt64(countValue);
                                    // 每个规则缓存约10KB估算
                                    long estimatedBytes = stat.ObjectCount * 10 * 1024;
                                    stat.EstimatedMemoryMB = Math.Max(1, estimatedBytes / (1024 * 1024));
                                    stat.Description = $"缓存 {stat.ObjectCount} 个规则评估结果";
                                }
                            }
                        }
                    }
                    
                    if (stat.ObjectCount == 0)
                    {
                        stat.EstimatedMemoryMB = 20;
                        stat.Description = "业务规则缓存、决策树等";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取规则引擎统计失败");
            }

            return stat;
        }

        /// <summary>
        /// 获取性能数据存储统计
        /// </summary>
        private ModuleMemoryStatistic GetPerformanceDataStorageMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "性能数据存储(PerformanceDataStorage)",
                Description = "客户端性能监控数据存储"
            };

            try
            {
                if (PerformanceDataStorageService != null)
                {
                    // 通过反射获取客户端数据存储字典
                    var serviceType = PerformanceDataStorageService.GetType();
                    var dataStoresField = serviceType.GetField("_clientDataStores", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    
                    if (dataStoresField != null)
                    {
                        var dataStores = dataStoresField.GetValue(PerformanceDataStorageService) as System.Collections.IDictionary;
                        if (dataStores != null)
                        {
                            long totalRecords = 0;
                            foreach (var store in dataStores.Values)
                            {
                                var totalCountProp = store.GetType().GetProperty("TotalCount");
                                if (totalCountProp != null)
                                {
                                    var count = totalCountProp.GetValue(store);
                                    if (count != null)
                                    {
                                        totalRecords += Convert.ToInt64(count);
                                    }
                                }
                            }
                            
                            stat.ObjectCount = dataStores.Count;
                            // 每条性能数据约1KB估算
                            long estimatedBytes = totalRecords * 1 * 1024;
                            stat.EstimatedMemoryMB = Math.Max(1, estimatedBytes / (1024 * 1024));
                            stat.Description = $"{dataStores.Count} 个客户端, 共 {totalRecords:N0} 条性能记录";
                        }
                    }
                    
                    if (stat.ObjectCount == 0)
                    {
                        stat.EstimatedMemoryMB = 10;
                        stat.Description = "客户端性能监控数据存储";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取性能数据存储统计失败");
            }

            return stat;
        }

        /// <summary>
        /// 获取文件存储监控统计
        /// </summary>
        private ModuleMemoryStatistic GetFileStorageMonitorMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "文件存储监控(FileStorageMonitor)",
                Description = "文件存储空间监控、清理任务等"
            };

            try
            {
                if (FileStorageMonitorService != null)
                {
                    // 文件存储监控服务本身占用内存较小，主要是配置和定时器
                    stat.ObjectCount = 1;
                    stat.EstimatedMemoryMB = 5;
                    stat.Description = "文件存储空间监控服务";
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取文件存储监控统计失败");
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
        
        /// <summary>
        /// 子项统计列表（用于详细展示各表/各组件的内存占用）
        /// </summary>
        public List<SubItemStatistic> SubItems { get; set; } = new List<SubItemStatistic>();
    }

    /// <summary>
    /// 子项统计信息
    /// </summary>
    public class SubItemStatistic
    {
        /// <summary>
        /// 子项名称（如表名、组件名等）
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 对象数量
        /// </summary>
        public long ObjectCount { get; set; }
        
        /// <summary>
        /// 估算内存占用（KB）
        /// </summary>
        public long EstimatedMemoryKB { get; set; }
        
        /// <summary>
        /// 命中率（0-1之间）
        /// </summary>
        public double HitRatio { get; set; }
    }
}
