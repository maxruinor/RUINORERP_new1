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
        
        // ✅ 会话统计缓存,避免频繁反射开销
        private DateTime _lastSessionStatsTime = DateTime.MinValue;
        private ModuleMemoryStatistic _cachedSessionStats;
        private const int SESSION_STATS_CACHE_SECONDS = 60;

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
                    GetSessionServiceMemory(), // 增强：详细统计每个会话
                    GetLockManagerMemory(),
                    GetSmartReminderMemory(),
                    GetRuleEngineMemory(),
                    GetPerformanceDataStorageMemory(),
                    GetFileStorageMonitorMemory(),
                    GetCacheSyncMetadataMemory(),
                    GetRuntimeAdvancedMetrics(), // 运行时高级指标
                    GetWorkflowMemory(), // 新增：WorkflowCore 内存占用
                    GetDatabaseConnectionMemory(), // 新增：数据库连接池
                    GetEventHandlersMemory(), // 新增：事件处理器
                    GetDelegateCacheMemory(), // 新增：委托缓存
                    GetReflectionCacheMemory(), // 新增：反射缓存
                    GetOtherManagedMemory() // 新增：其他托管内存
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

        /// <summary>
        /// 获取会话服务的内存统计
        /// </summary>
        private ModuleMemoryStatistic GetSessionServiceMemory()
        {
            // ✅ 缓存会话统计结果,避免每次分析都执行反射操作
            if ((DateTime.Now - _lastSessionStatsTime).TotalSeconds < SESSION_STATS_CACHE_SECONDS 
                && _cachedSessionStats != null)
            {
                return _cachedSessionStats;
            }
            
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "会话管理 (SessionService)",
                Description = "客户端会话连接、SessionInfo 对象、DataQueue 等"
            };

            try
            {
                if (SessionService != null)
                {
                    var sessionCount = SessionService.ActiveSessionCount;
                    stat.ObjectCount = sessionCount;
                    
                    // 详细统计每个会话（限制数量以避免过多内存占用）
                    const int MAX_SESSION_DETAILS = 5; // ✅ 限制详细统计的会话数量
                    var sessionDetails = new List<(string SessionId, string UserName, long DataQueueSize, int MessageCount, long ConnectedMinutes)>();
                    long totalDataQueueSize = 0;
                    int totalQueuedMessages = 0;
                    int processedSessions = 0;
                    
                    try
                    {
                        // 通过反射获取所有会话的详细信息
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
                                    try
                                    {
                                        if (session == null) continue;
                                        
                                        var sessionId = "Unknown";
                                        var userName = "Unknown";
                                        int messageCount = 0;
                                        long dataQueueSize = 0;
                                        DateTime connectedTime = DateTime.Now;
                                        
                                        // 获取 SessionID
                                        var sessionIdProp = session.GetType().GetProperty("SessionID");
                                        if (sessionIdProp != null)
                                            sessionId = sessionIdProp.GetValue(session)?.ToString() ?? "Unknown";
                                        
                                        // 获取 UserName
                                        var userInfoProp = session.GetType().GetProperty("UserInfo");
                                        if (userInfoProp != null)
                                        {
                                            var userInfo = userInfoProp.GetValue(session);
                                            if (userInfo != null)
                                            {
                                                var userNameProp = userInfo.GetType().GetProperty("UserName");
                                                if (userNameProp != null)
                                                    userName = userNameProp.GetValue(userInfo)?.ToString() ?? "Anonymous";
                                                
                                                // 获取连接时间
                                                var connectedTimeProp = userInfo.GetType().GetProperty("ConnectedTime");
                                                if (connectedTimeProp != null)
                                                {
                                                    var ctValue = connectedTimeProp.GetValue(userInfo);
                                                    if (ctValue is DateTime dt)
                                                        connectedTime = dt;
                                                }
                                            }
                                        }
                                        
                                        // 获取 DataQueue
                                        var dataQueueProperty = session.GetType().GetProperty("DataQueue");
                                        if (dataQueueProperty != null)
                                        {
                                            var dataQueue = dataQueueProperty.GetValue(session) as System.Collections.ICollection;
                                            if (dataQueue != null)
                                            {
                                                messageCount = dataQueue.Count;
                                                dataQueueSize = dataQueue.Count * 1024;
                                                totalQueuedMessages += messageCount;
                                                totalDataQueueSize += dataQueueSize;
                                            }
                                        }
                                        
                                        // ✅ 只收集前 N 个会话的详情，避免内存占用过大
                                        if (processedSessions < MAX_SESSION_DETAILS)
                                        {
                                            sessionDetails.Add((sessionId, userName, dataQueueSize, messageCount, (long)(DateTime.Now - connectedTime).TotalMinutes));
                                        }
                                        processedSessions++;
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogDebug(ex, "处理会话详情时发生错误，跳过该会话");
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug(ex, "反射获取会话详情失败");
                    }
                    
                    // 基础内存 + 会话对象 + 数据队列
                    long baseMemory = 30 * 1024 * 1024; // 30MB 基础（服务本身）
                    long perSessionMemory = sessionCount * 1500 * 1024; // 每会话 1.5MB（SessionInfo 对象 + 缓冲区）
                    stat.EstimatedMemoryMB = (baseMemory + perSessionMemory + totalDataQueueSize) / (1024 * 1024);
                    
                    // 添加详细子项
                    stat.SubItems = new List<SubItemStatistic>();
                    
                    // 基础服务占用
                    stat.SubItems.Add(new SubItemStatistic
                    {
                        Name = "服务基础",
                        Description = "SessionService 本身、字典结构、事件处理器等",
                        ObjectCount = 1,
                        EstimatedMemoryKB = 30 * 1024
                    });
                    
                    // 会话对象占用
                    stat.SubItems.Add(new SubItemStatistic
                    {
                        Name = "会话对象",
                        Description = $"每个会话约 1.5MB（SessionInfo+ 缓冲区 + 属性）",
                        ObjectCount = sessionCount,
                        EstimatedMemoryKB = sessionCount * 1500
                    });
                    
                    // 数据队列
                    if (totalQueuedMessages > 0)
                    {
                        stat.SubItems.Add(new SubItemStatistic
                        {
                            Name = "数据队列 (DataQueue)",
                            Description = $"总计 {totalQueuedMessages} 条待发送消息",
                            ObjectCount = totalQueuedMessages,
                            EstimatedMemoryKB = totalDataQueueSize / 1024
                        });
                    }
                    
                    // 添加每个会话的详情（前 N 个）
                    foreach (var (sessionId, userName, queueSize, msgCount, connectedMinutes) in sessionDetails)
                    {
                        var sessionName = string.IsNullOrWhiteSpace(userName) || userName == "Anonymous" 
                            ? sessionId 
                            : $"{userName} ({sessionId})";
                        
                        stat.SubItems.Add(new SubItemStatistic
                        {
                            Name = $"会话：{sessionName}",
                            Description = $"连接 {connectedMinutes} 分钟，队列：{msgCount} 条消息",
                            ObjectCount = msgCount,
                            EstimatedMemoryKB = queueSize / 1024 + 1500 // 队列 + 基础会话对象
                        });
                    }
                    
                    // 如果超过 N 个会话，添加汇总
                    if (processedSessions > MAX_SESSION_DETAILS)
                    {
                        var remaining = processedSessions - MAX_SESSION_DETAILS;
                        var remainingMemory = remaining * 1500; // 估算
                        stat.SubItems.Add(new SubItemStatistic
                        {
                            Name = $"其他 {remaining} 个会话",
                            Description = "详见日志输出",
                            ObjectCount = remaining,
                            EstimatedMemoryKB = remainingMemory
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取会话服务内存统计失败");
            }

            // ✅ 更新缓存
            _cachedSessionStats = stat;
            _lastSessionStatsTime = DateTime.Now;

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
                                try
                                {
                                    if (store == null) continue;
                                    
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
                                catch (Exception ex)
                                {
                                    _logger.LogDebug(ex, "获取性能数据存储详情时发生错误");
                                    continue;
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

        /// <summary>
        /// 获取运行时高级指标（LOH、线程、句柄等）
        /// </summary>
        private ModuleMemoryStatistic GetRuntimeAdvancedMetrics()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "运行时高级指标(Runtime Advanced)",
                Description = "LOH碎片、线程栈、句柄、DB连接池等"
            };

            try
            {
                var gcInfo = GC.GetGCMemoryInfo();
                var process = Process.GetCurrentProcess();

                // 1. LOH (大对象堆) 统计
                long lohSizeMB = gcInfo.HeapSizeBytes / (1024 * 1024);
                
                // 2. 线程统计
                int threadCount = process.Threads.Count;
                long threadStackEstimateMB = threadCount; // 默认每个线程栈约 1MB

                // 3. 句柄统计
                int handleCount = process.HandleCount;
                
                // 4. 计算"隐形"内存 (工作集 - 托管内存)
                long workingSetMB = process.WorkingSet64 / (1024 * 1024);
                long managedMemoryMB = GC.GetTotalMemory(false) / (1024 * 1024);
                long unmanagedEstimateMB = workingSetMB - managedMemoryMB;
                if (unmanagedEstimateMB < 0) unmanagedEstimateMB = 0;

                stat.ObjectCount = threadCount + handleCount;
                stat.EstimatedMemoryMB = lohSizeMB + threadStackEstimateMB + unmanagedEstimateMB;

                stat.SubItems = new List<SubItemStatistic>
                {
                    new SubItemStatistic
                    {
                        Name = "LOH (大对象堆)",
                        Description = $"大小: {gcInfo.HeapSizeBytes / (1024*1024)} MB, 碎片化可能导致工作集虚高",
                        ObjectCount = 0,
                        EstimatedMemoryKB = gcInfo.HeapSizeBytes / 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "线程栈(Thread Stacks)",
                        Description = $"当前活跃线程数: {threadCount}",
                        ObjectCount = threadCount,
                        EstimatedMemoryKB = threadStackEstimateMB * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "非托管/原生内存(Native)",
                        Description = $"估算值: {unmanagedEstimateMB} MB (JIT代码、网络缓冲区、内核对象)",
                        ObjectCount = 0,
                        EstimatedMemoryKB = unmanagedEstimateMB * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "系统句柄(Handles)",
                        Description = $"总句柄数: {handleCount} (文件/网络/同步对象)",
                        ObjectCount = handleCount,
                        EstimatedMemoryKB = 0
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取运行时高级指标失败");
            }

            return stat;
        }

        private ModuleMemoryStatistic GetGeneralMemoryInfo()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "运行时基础内存",
                Description = "CLR 运行时、字符串池、程序集加载等基础内存"
            };

            var gcInfo = GC.GetGCMemoryInfo();
            stat.TotalAvailableMB = gcInfo.TotalAvailableMemoryBytes / (1024 * 1024);
            stat.EstimatedMemoryMB = 100;

            var gen0Collections = GC.CollectionCount(0);
            var gen1Collections = GC.CollectionCount(1);
            var gen2Collections = GC.CollectionCount(2);

            stat.Description += $" | GC 次数：Gen0={gen0Collections}, Gen1={gen1Collections}, Gen2={gen2Collections}";

            return stat;
        }

        /// <summary>
        /// 获取 WorkflowCore 的内存占用
        /// </summary>
        private ModuleMemoryStatistic GetWorkflowMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "工作流引擎 (WorkflowCore)",
                Description = "工作流实例、运行数据、事件订阅等"
            };

            try
            {
                // 通过反射获取 WorkflowHost 的实例
                var workflowHostType = Type.GetType("WorkflowCore.Interface.IWorkflowHost, WorkflowCore.Interface");
                if (workflowHostType == null)
                {
                    workflowHostType = Type.GetType("WorkflowCore.Interface.IWorkflowHost, WorkflowCore");
                }

                if (workflowHostType != null)
                {
                    // 尝试从 DI 容器获取实例
                    var serviceProvider = RUINORERP.Server.Program.ServiceProvider;
                    if (serviceProvider != null)
                    {
                        var workflowHost = serviceProvider.GetService(workflowHostType);
                        if (workflowHost != null)
                        {
                            // 获取运行中的工作流数量
                            var runningWorkflowsProperty = workflowHostType.GetProperty("RunningWorkflows");
                            if (runningWorkflowsProperty != null)
                            {
                                var runningWorkflows = runningWorkflowsProperty.GetValue(workflowHost) as System.Collections.IEnumerable;
                                if (runningWorkflows != null)
                                {
                                    var count = 0;
                                    try
                                    {
                                        foreach (var wf in runningWorkflows)
                                        {
                                            if (wf != null) count++;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogDebug(ex, "遍历运行中的工作流时发生错误");
                                    }

                                    stat.ObjectCount = count;
                                    // 每个工作流实例约占用 500KB-2MB
                                    stat.EstimatedMemoryMB = Math.Max(10, count * 1);

                                    if (count > 0)
                                    {
                                        stat.SubItems = new List<SubItemStatistic>
                                        {
                                            new SubItemStatistic
                                            {
                                                Name = "运行中的工作流",
                                                Description = $"当前有 {count} 个工作流实例在运行",
                                                ObjectCount = count,
                                                EstimatedMemoryKB = count * 1024
                                            }
                                        };
                                    }
                                }
                            }
                        }
                    }
                }

                // 如果无法获取，使用默认估算
                if (stat.EstimatedMemoryMB == 0)
                {
                    stat.EstimatedMemoryMB = 20; // 默认估算值
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取工作流引擎内存统计失败");
                stat.EstimatedMemoryMB = 20;
            }

            return stat;
        }

        /// <summary>
        /// 获取数据库连接池的内存占用
        /// </summary>
        private ModuleMemoryStatistic GetDatabaseConnectionMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "数据库连接池 (SqlSugar)",
                Description = "数据库连接、命令缓存、结果集缓存等"
            };

            try
            {
                // 估算：每个连接约占用 2-5MB（包括连接池、命令缓存等）
                // SqlSugar 默认连接池大小为 100
                const int estimatedConnections = 50; // 假设活跃连接数
                const long memoryPerConnection = 3 * 1024 * 1024; // 3MB per connection

                stat.EstimatedMemoryMB = estimatedConnections * memoryPerConnection / (1024 * 1024);
                stat.ObjectCount = estimatedConnections;

                stat.SubItems = new List<SubItemStatistic>
                {
                    new SubItemStatistic
                    {
                        Name = "连接池",
                        Description = $"估算 {estimatedConnections} 个活跃连接",
                        ObjectCount = estimatedConnections,
                        EstimatedMemoryKB = estimatedConnections * 3 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "命令缓存",
                        Description = "SQL 命令缓存、参数缓存等",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 10 * 1024 // 估算 10MB
                    },
                    new SubItemStatistic
                    {
                        Name = "查询结果缓存",
                        Description = "临时结果集、数据 reader 缓存等",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 20 * 1024 // 估算 20MB
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取数据库连接池内存统计失败");
                stat.EstimatedMemoryMB = 50;
            }

            return stat;
        }

        /// <summary>
        /// 获取事件处理器的内存占用
        /// </summary>
        private ModuleMemoryStatistic GetEventHandlersMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "事件处理器 (EventHandlers)",
                Description = "事件订阅、委托引用、事件参数等"
            };

            try
            {
                // 统计主要事件源
                var eventSources = new List<(string Name, long EstimatedMemory)>
                {
                    ("SessionService 事件", 5 * 1024), // 5MB
                    ("WorkflowCore 事件", 10 * 1024), // 10MB
                    ("Cache 事件", 3 * 1024), // 3MB
                    ("UI 事件", 2 * 1024), // 2MB
                    ("其他事件", 5 * 1024) // 5MB
                };

                long totalMemory = 0;
                stat.SubItems = new List<SubItemStatistic>();

                foreach (var (name, memory) in eventSources)
                {
                    stat.SubItems.Add(new SubItemStatistic
                    {
                        Name = name,
                        Description = "事件订阅和处理器",
                        ObjectCount = 0,
                        EstimatedMemoryKB = memory
                    });
                    totalMemory += memory;
                }

                stat.EstimatedMemoryMB = totalMemory / 1024;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取事件处理器内存统计失败");
                stat.EstimatedMemoryMB = 25;
            }

            return stat;
        }

        /// <summary>
        /// 获取委托缓存的内存占用
        /// </summary>
        private ModuleMemoryStatistic GetDelegateCacheMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "委托缓存 (DelegateCache)",
                Description = "Lambda 表达式、闭包、动态生成的委托等"
            };

            try
            {
                // 估算委托缓存占用
                // 包括：LINQ 表达式编译、动态代理、反射委托等
                stat.EstimatedMemoryMB = 30;

                stat.SubItems = new List<SubItemStatistic>
                {
                    new SubItemStatistic
                    {
                        Name = "LINQ 编译缓存",
                        Description = "编译后的 LINQ 表达式树",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 10 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "动态代理",
                        Description = "Castle DynamicProxy 生成的代理类",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 10 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "反射委托",
                        Description = "MethodInfo.Invoke 等反射操作的委托封装",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 5 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "其他委托",
                        Description = "其他动态生成的委托",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 5 * 1024
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取委托缓存内存统计失败");
                stat.EstimatedMemoryMB = 30;
            }

            return stat;
        }

        /// <summary>
        /// 获取反射缓存的内存占用
        /// </summary>
        private ModuleMemoryStatistic GetReflectionCacheMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "反射缓存 (ReflectionCache)",
                Description = "Type 信息缓存、PropertyInfo、MethodInfo 等"
            };

            try
            {
                // 估算反射缓存占用
                // 包括：Type 缓存、MemberInfo 缓存、Attribute 缓存等
                stat.EstimatedMemoryMB = 20;

                stat.SubItems = new List<SubItemStatistic>
                {
                    new SubItemStatistic
                    {
                        Name = "Type 信息缓存",
                        Description = "System.Type 对象及其元数据",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 8 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "MemberInfo 缓存",
                        Description = "PropertyInfo、MethodInfo、FieldInfo 等",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 6 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "Attribute 缓存",
                        Description = "自定义 Attribute 实例",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 3 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "表达式树缓存",
                        Description = "Expression 树及其编译结果",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 3 * 1024
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取反射缓存内存统计失败");
                stat.EstimatedMemoryMB = 20;
            }

            return stat;
        }

        /// <summary>
        /// 获取其他托管内存的占用
        /// </summary>
        private ModuleMemoryStatistic GetOtherManagedMemory()
        {
            var stat = new ModuleMemoryStatistic
            {
                ModuleName = "其他托管内存 (Other)",
                Description = "未分类的托管内存、临时对象、GC 堆碎片等"
            };

            try
            {
                // 计算已统计的内存总量
                var totalManagedMB = GC.GetTotalMemory(false) / (1024 * 1024);
                long accountedMemory = 0;

                // 这里不重复计算，只是给出一个估算值
                // 其他托管内存包括：
                // - 临时对象
                // - GC 堆碎片
                // - 大对象堆 (LOH)
                // - 冻结对象
                // - 同步块表
                // - 方法表等

                stat.EstimatedMemoryMB = Math.Max(50, (int)(totalManagedMB * 0.15)); // 至少 50MB 或 15% 的总内存

                stat.SubItems = new List<SubItemStatistic>
                {
                    new SubItemStatistic
                    {
                        Name = "临时对象",
                        Description = "短生命周期的临时对象",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 10 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "GC 堆碎片",
                        Description = "GC 堆中的空闲空间和碎片",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 20 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "大对象堆 (LOH)",
                        Description = "大于 85KB 的对象",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 15 * 1024
                    },
                    new SubItemStatistic
                    {
                        Name = "CLR 内部结构",
                        Description = "方法表、同步块表等 CLR 内部数据结构",
                        ObjectCount = 0,
                        EstimatedMemoryKB = 10 * 1024
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "获取其他托管内存统计失败");
                stat.EstimatedMemoryMB = 50;
            }

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
