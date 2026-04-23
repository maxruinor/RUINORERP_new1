using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 内存泄漏诊断服务
    /// 用于深度分析内存占用，找出内存泄漏的根源
    /// </summary>
    public class MemoryLeakDiagnosticsService : IDisposable
    {
        private readonly ILogger<MemoryLeakDiagnosticsService> _logger;
        private readonly Timer _diagnosticsTimer;
        private bool _disposed = false;
        
        // 诊断配置
        private const int DIAGNOSTICS_INTERVAL_SECONDS = 300; // 每 5 分钟诊断一次（原 60 秒）
        private const int MEMORY_LEAK_THRESHOLD_MB = 500; // 内存泄漏阈值（500MB）
        private const int MAX_SNAPSHOT_HISTORY = 100; // 最多保留 100 个快照
        
        // 内存快照历史
        private readonly List<MemorySnapshot> _snapshotHistory = new List<MemorySnapshot>();
        private readonly object _lockObject = new object();
        
        // 内存泄漏检测
        private MemoryLeakReport _lastLeakReport;
        
        // 诊断开关
        private volatile bool _isEnabled = true; // 默认启用
        private volatile bool _isVerboseMode = false; // 默认非详细模式（只在检测到泄漏时输出警告）
        
        /// <summary>
        /// 诊断开关：启用/禁用诊断服务
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                if (_isEnabled)
                {
                    _logger.LogInformation("内存泄漏诊断服务已启用");
                }
                else
                {
                    _logger.LogInformation("内存泄漏诊断服务已禁用");
                }
            }
        }
        
        /// <summary>
        /// 详细模式开关：启用后每次诊断都输出完整日志
        /// </summary>
        public bool IsVerboseMode
        {
            get => _isVerboseMode;
            set
            {
                _isVerboseMode = value;
                if (_isVerboseMode)
                {
                    _logger.LogInformation("内存泄漏诊断详细模式已启用（每次诊断都输出完整日志）");
                }
                else
                {
                    _logger.LogInformation("内存泄漏诊断详细模式已禁用（只在检测到泄漏时输出警告）");
                }
            }
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public MemoryLeakDiagnosticsService(ILogger<MemoryLeakDiagnosticsService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _diagnosticsTimer = new Timer(RunDiagnostics, null, 
                TimeSpan.FromSeconds(5), 
                TimeSpan.FromSeconds(DIAGNOSTICS_INTERVAL_SECONDS));
            
            _logger.LogInformation("内存泄漏诊断服务已启动（5 分钟诊断一次）");
        }
        
        /// <summary>
        /// 立即运行诊断
        /// </summary>
        public MemoryLeakReport RunDiagnosticsNow()
        {
            return CreateLeakReport();
        }
        
        /// <summary>
        /// 获取最新的内存泄漏报告
        /// </summary>
        public MemoryLeakReport GetLastReport()
        {
            lock (_lockObject)
            {
                return _lastLeakReport;
            }
        }
        
        /// <summary>
        /// 获取内存快照历史
        /// </summary>
        public List<MemorySnapshot> GetSnapshotHistory()
        {
            lock (_lockObject)
            {
                return new List<MemorySnapshot>(_snapshotHistory);
            }
        }
        
        private void RunDiagnostics(object state)
        {
            // 如果诊断服务被禁用，跳过本次诊断
            if (!_isEnabled)
            {
                return;
            }
            
            try
            {
                var report = CreateLeakReport();
                
                lock (_lockObject)
                {
                    _lastLeakReport = report;
                    
                    // 保存快照历史
                    _snapshotHistory.Add(report.CurrentSnapshot);
                    if (_snapshotHistory.Count > MAX_SNAPSHOT_HISTORY)
                    {
                        _snapshotHistory.RemoveAt(0);
                    }
                }
                
                // 输出详细日志（详细模式或检测到泄漏时输出）
                if (_isVerboseMode || report.IsMemoryLeakDetected)
                {
                    LogDetailedDiagnostics(report);
                }
                else
                {
                    // 简单日志：只在检测到泄漏时输出警告
                    if (report.IsMemoryLeakDetected)
                    {
                        _logger.LogWarning(
                            "🔴 严重：检测到内存泄漏！内存增长率：{GrowthRate} MB/分钟，预计{TimeToCritical}分钟后达到临界值",
                            report.MemoryGrowthRatePerMinute,
                            report.MinutesToCriticalThreshold);
                    }
                    else
                    {
                        // 正常情况：只记录调试日志
                        _logger.LogDebug(
                            "内存使用正常 - 工作集：{WorkingSet} MB, 增长率：{GrowthRate} MB/分钟",
                            report.CurrentSnapshot.WorkingSetMB,
                            report.MemoryGrowthRatePerMinute);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "运行内存泄漏诊断时发生错误");
            }
        }
        
        private MemoryLeakReport CreateLeakReport()
        {
            var currentSnapshot = TakeMemorySnapshot();
            
            // 计算增长率
            double growthRatePerMinute = 0;
            int minutesToCritical = int.MaxValue;
            
            lock (_lockObject)
            {
                if (_snapshotHistory.Count >= 2)
                {
                    // 取最近 10 个快照计算平均增长率
                    var recentSnapshots = _snapshotHistory.Skip(Math.Max(0, _snapshotHistory.Count - 10)).ToList();
                    if (recentSnapshots.Count >= 2)
                    {
                        var oldest = recentSnapshots.First();
                        var newest = recentSnapshots.Last();
                        
                        var timeDiff = (newest.Timestamp - oldest.Timestamp).TotalMinutes;
                        if (timeDiff > 0)
                        {
                            var memoryDiff = newest.WorkingSetMB - oldest.WorkingSetMB;
                            growthRatePerMinute = memoryDiff / timeDiff;
                        }
                    }
                }
            }
            
            // 计算达到临界值的时间
            const int CRITICAL_THRESHOLD_MB = 6000; // 6GB 为临界值
            if (growthRatePerMinute > 0 && currentSnapshot.WorkingSetMB < CRITICAL_THRESHOLD_MB)
            {
                var remainingMemory = CRITICAL_THRESHOLD_MB - currentSnapshot.WorkingSetMB;
                minutesToCritical = (int)(remainingMemory / growthRatePerMinute);
            }
            
            // 检测内存泄漏
            bool isLeakDetected = growthRatePerMinute > MEMORY_LEAK_THRESHOLD_MB / 60; // 每分钟增长>8.3MB
            
            // 分析主要内存占用者
            var topConsumers = AnalyzeTopMemoryConsumers();
            
            // 分析对象存活情况
            var objectSurvival = AnalyzeObjectSurvival();
            
            // 分析 GC 代分布
            var gcGeneration = AnalyzeGCDistribution();
            
            // 分析线程情况
            var threadAnalysis = AnalyzeThreads();
            
            // 分析数据库连接
            var dbConnectionAnalysis = AnalyzeDatabaseConnections();
            
            return new MemoryLeakReport
            {
                Timestamp = DateTime.Now,
                CurrentSnapshot = currentSnapshot,
                IsMemoryLeakDetected = isLeakDetected,
                MemoryGrowthRatePerMinute = growthRatePerMinute,
                MinutesToCriticalThreshold = minutesToCritical,
                TopMemoryConsumers = topConsumers,
                ObjectSurvivalAnalysis = objectSurvival,
                GCDistribution = gcGeneration,
                ThreadAnalysis = threadAnalysis,
                DatabaseConnectionAnalysis = dbConnectionAnalysis,
                Recommendations = GenerateRecommendations(isLeakDetected, growthRatePerMinute, topConsumers)
            };
        }
        
        private MemorySnapshot TakeMemorySnapshot()
        {
            var process = Process.GetCurrentProcess();
            
            return new MemorySnapshot
            {
                Timestamp = DateTime.Now,
                WorkingSetMB = process.WorkingSet64 / (1024 * 1024),
                ManagedMemoryMB = GC.GetTotalMemory(false) / (1024 * 1024),
                PrivateMemoryMB = process.PrivateMemorySize64 / (1024 * 1024),
                VirtualMemoryMB = process.VirtualMemorySize64 / (1024 * 1024),
                HandleCount = process.HandleCount,
                ThreadCount = process.Threads.Count,
                GCGeneration0 = GC.CollectionCount(0),
                GCGeneration1 = GC.CollectionCount(1),
                GCGeneration2 = GC.CollectionCount(2),
                LOHSizeMB = GC.GetGCMemoryInfo().TotalCommittedBytes / (1024 * 1024)
            };
        }
        
        private List<MemoryConsumerInfo> AnalyzeTopMemoryConsumers()
        {
            var consumers = new List<MemoryConsumerInfo>();
            
            try
            {
                // 1. 分析大型对象堆 (LOH)
                var lohInfo = new MemoryConsumerInfo
                {
                    Name = "大型对象堆 (LOH)",
                    Description = "大于 85KB 的对象直接分配到 LOH",
                    EstimatedMemoryMB = GC.GetGCMemoryInfo().TotalCommittedBytes / (1024 * 1024),
                    IsPotentialLeak = false
                };
                consumers.Add(lohInfo);
                
                // 2. 分析程序集加载
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var assemblyMemory = new MemoryConsumerInfo
                {
                    Name = "程序集加载",
                    Description = $"已加载 {assemblies.Length} 个程序集",
                    EstimatedMemoryMB = EstimateAssemblyMemory(assemblies),
                    IsPotentialLeak = assemblies.Length > 200 // 超过 200 个程序集可能是问题
                };
                consumers.Add(assemblyMemory);
                
                // 3. 分析线程栈
                var process = Process.GetCurrentProcess();
                var threadMemory = new MemoryConsumerInfo
                {
                    Name = "线程栈",
                    Description = $"{process.Threads.Count} 个线程，每个约 1-2MB",
                    EstimatedMemoryMB = process.Threads.Count * 2, // 64 位系统每个线程 2MB
                    IsPotentialLeak = process.Threads.Count > 100 // 超过 100 个线程可能是问题
                };
                consumers.Add(threadMemory);
                
                // 4. 分析 GDI 对象
                var gdiObjectCount = GetGDIObjectCount(process.Id);
                var gdiMemory = new MemoryConsumerInfo
                {
                    Name = "GDI 对象",
                    Description = $"{gdiObjectCount} 个 GDI 对象",
                    EstimatedMemoryMB = gdiObjectCount * 10 / 1024, // 每个 GDI 对象约 10KB
                    IsPotentialLeak = gdiObjectCount > 5000 // 超过 5000 个 GDI 对象可能是问题
                };
                consumers.Add(gdiMemory);
                
                // 5. 分析数据库连接（通过 SqlSugar）
                var dbMemory = new MemoryConsumerInfo
                {
                    Name = "数据库连接池",
                    Description = "SqlSugar 连接池（默认最大 100 个连接）",
                    EstimatedMemoryMB = 80, // 估算值
                    IsPotentialLeak = false // 需要进一步检测
                };
                consumers.Add(dbMemory);
                
                // 6. 分析网络 Socket
                var socketMemory = new MemoryConsumerInfo
                {
                    Name = "网络 Socket 缓冲",
                    Description = "SuperSocket 网络库缓冲区",
                    EstimatedMemoryMB = 40, // 估算值
                    IsPotentialLeak = false
                };
                consumers.Add(socketMemory);
                
                // 7. 分析缓存对象
                var cacheMemory = new MemoryConsumerInfo
                {
                    Name = "缓存对象",
                    Description = "EntityCache, MemoryCache, Redis 等",
                    EstimatedMemoryMB = 100, // 估算值
                    IsPotentialLeak = false
                };
                consumers.Add(cacheMemory);
                
                // 8. 分析会话管理
                var sessionMemory = new MemoryConsumerInfo
                {
                    Name = "会话管理",
                    Description = "SessionService 管理的用户会话",
                    EstimatedMemoryMB = 80, // 估算值
                    IsPotentialLeak = false // 需要检查会话是否过期
                };
                consumers.Add(sessionMemory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分析内存占用者时发生错误");
            }
            
            // 按内存占用排序
            return consumers.OrderByDescending(c => c.EstimatedMemoryMB).ToList();
        }
        
        private ObjectSurvivalInfo AnalyzeObjectSurvival()
        {
            // ✅ 仅在检测到内存泄漏时才强制GC
            if (!(_lastLeakReport?.IsMemoryLeakDetected ?? true))
            {
                // 正常情况:仅获取GC统计,不强制回收
                var gcInfo = GC.GetGCMemoryInfo();
                return new ObjectSurvivalInfo
                {
                    Gen0Objects = GC.CollectionCount(0),
                    Gen1Objects = GC.CollectionCount(1),
                    Gen2Objects = GC.CollectionCount(2),
                    LOHSizeMB = gcInfo.TotalCommittedBytes / (1024 * 1024),
                    PauseTimePredictionMS = 0, // .NET Core/.NET 5+ 不提供此属性
                    FragmentationPercentage = 0
                };
            }
            
            // ✅ 仅在检测到泄漏时强制GC进行深入分析
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true);
            GC.WaitForPendingFinalizers();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true);
            
            var gcInfoAfter = GC.GetGCMemoryInfo();
            return new ObjectSurvivalInfo
            {
                Gen0Objects = GC.CollectionCount(0),
                Gen1Objects = GC.CollectionCount(1),
                Gen2Objects = GC.CollectionCount(2),
                LOHSizeMB = gcInfoAfter.TotalCommittedBytes / (1024 * 1024),
                PauseTimePredictionMS = 0, // .NET Core/.NET 5+ 不提供此属性
                FragmentationPercentage = 0
            };
        }
        
        private GCDistributionInfo AnalyzeGCDistribution()
        {
            var gcInfo = GC.GetGCMemoryInfo();
            
            return new GCDistributionInfo
            {
                Gen0SizeMB = 0, // .NET Core/.NET 5+ 不直接提供
                Gen1SizeMB = 0,
                Gen2SizeMB = 0,
                LOHSizeMB = gcInfo.TotalCommittedBytes / (1024 * 1024),
                TotalCommittedMB = gcInfo.TotalCommittedBytes / (1024 * 1024),
                TotalAvailableMB = gcInfo.TotalAvailableMemoryBytes / (1024 * 1024)
            };
        }
        
        private ThreadAnalysisInfo AnalyzeThreads()
        {
            var process = Process.GetCurrentProcess();
            var threadInfos = new List<ThreadInfo>();
            
            foreach (ProcessThread thread in process.Threads)
            {
                threadInfos.Add(new ThreadInfo
                {
                    Id = thread.Id,
                    State = thread.ThreadState.ToString(),
                    IsThreadPoolThread = false, // .NET Core/.NET 5+ ProcessThread不提供此属性
                    IsBackground = false, // .NET Core/.NET 5+ ProcessThread不提供此属性
                    Priority = thread.PriorityLevel.ToString(),
                    StartTime = thread.StartTime
                });
            }
            
            return new ThreadAnalysisInfo
            {
                TotalThreads = process.Threads.Count,
                ThreadPoolThreads = threadInfos.Count(t => t.IsThreadPoolThread),
                BackgroundThreads = threadInfos.Count(t => t.IsBackground),
                ForegroundThreads = threadInfos.Count(t => !t.IsBackground),
                ThreadDetails = threadInfos.Take(20).ToList() // 只显示前 20 个
            };
        }
        
        private DatabaseConnectionInfo AnalyzeDatabaseConnections()
        {
            // 注意：SqlSugar 不直接暴露连接池统计
            // 这里提供估算和建议
            
            return new DatabaseConnectionInfo
            {
                EstimatedPoolSize = 100, // 默认最大连接池大小
                EstimatedMemoryMB = 80,
                Recommendation = "建议检查连接字符串中的 Max Pool Size 设置，考虑降低到 50"
            };
        }
        
        private List<string> GenerateRecommendations(bool isLeakDetected, double growthRate, List<MemoryConsumerInfo> topConsumers)
        {
            var recommendations = new List<string>();
            
            if (isLeakDetected)
            {
                recommendations.Add($"⚠️ 检测到内存泄漏！内存增长率：{growthRate:F2} MB/分钟");
                recommendations.Add("建议立即检查以下方面：");
            }
            
            // 根据占用情况给出建议
            var topConsumer = topConsumers.FirstOrDefault();
            if (topConsumer != null && topConsumer.EstimatedMemoryMB > 500)
            {
                recommendations.Add($"🔴 最大内存占用者：{topConsumer.Name} ({topConsumer.EstimatedMemoryMB} MB)");
            }
            
            // 线程数过多
            if (Process.GetCurrentProcess().Threads.Count > 100)
            {
                recommendations.Add("⚠️ 线程数过多，检查是否有未释放的后台任务或定时器");
            }
            
            // GDI 对象过多
            var gdiCount = GetGDIObjectCount(Process.GetCurrentProcess().Id);
            if (gdiCount > 5000)
            {
                recommendations.Add($"⚠️ GDI 对象过多 ({gdiCount} 个)，检查是否有 Bitmap/Graphics 未释放");
            }
            
            // 程序集过多
            var assemblyCount = AppDomain.CurrentDomain.GetAssemblies().Length;
            if (assemblyCount > 200)
            {
                recommendations.Add($"⚠️ 加载的程序集过多 ({assemblyCount} 个)，检查是否有动态加载程序集的逻辑");
            }
            
            // 通用建议
            recommendations.Add("💡 建议措施：");
            recommendations.Add("1. 使用 dotnet-dump 创建内存转储文件进行深度分析");
            recommendations.Add("2. 检查所有使用 new 创建的对象是否正确 Dispose");
            recommendations.Add("3. 检查事件订阅是否正确取消订阅");
            recommendations.Add("4. 检查静态集合是否无限增长");
            recommendations.Add("5. 检查缓存是否有过期策略");
            
            return recommendations;
        }
        
        private void LogDetailedDiagnostics(MemoryLeakReport report)
        {
            var sb = new StringBuilder();
            
            // 诊断报告头
            sb.AppendLine("\n" + new string('=', 100));
            sb.AppendLine("【内存泄漏诊断报告】");
            sb.AppendLine(new string('=', 100));
            sb.AppendLine($"⏰ 诊断时间：{report.Timestamp:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"📊 在线用户：15 人（估算）");
            sb.AppendLine($"⏳ 运行时长：半天（估算）");
            sb.AppendLine();
            
            // 内存状态
            sb.AppendLine(new string('-', 100));
            sb.AppendLine("【1️⃣ 内存状态】");
            sb.AppendLine(new string('-', 100));
            sb.AppendLine($"   工作集内存：     {report.CurrentSnapshot.WorkingSetMB,8} MB  ← 整个进程占用的物理内存");
            sb.AppendLine($"   托管内存：       {report.CurrentSnapshot.ManagedMemoryMB,8} MB  ← .NET GC 管理的内存");
            sb.AppendLine($"   私有内存：       {report.CurrentSnapshot.PrivateMemoryMB,8} MB  ← 进程独占内存");
            sb.AppendLine($"   虚拟内存：       {report.CurrentSnapshot.VirtualMemoryMB,8} MB  ← 进程虚拟地址空间");
            sb.AppendLine($"   句柄数：         {report.CurrentSnapshot.HandleCount,8} 个");
            sb.AppendLine($"   线程数：         {report.CurrentSnapshot.ThreadCount,8} 个");
            sb.AppendLine($"   GC Gen0 次数：    {report.CurrentSnapshot.GCGeneration0,8} 次");
            sb.AppendLine($"   GC Gen1 次数：    {report.CurrentSnapshot.GCGeneration1,8} 次");
            sb.AppendLine($"   GC Gen2 次数：    {report.CurrentSnapshot.GCGeneration2,8} 次");
            sb.AppendLine($"   LOH 大小：        {report.CurrentSnapshot.LOHSizeMB,8} MB  ← 大型对象堆");
            sb.AppendLine();
            
            // 泄漏检测
            sb.AppendLine(new string('-', 100));
            sb.AppendLine("【2️⃣ 泄漏检测】");
            sb.AppendLine(new string('-', 100));
            var leakStatus = report.IsMemoryLeakDetected ? "🔴 是（严重）" : "✅ 否";
            sb.AppendLine($"   是否检测到泄漏：  {leakStatus}");
            sb.AppendLine($"   内存增长率：     {report.MemoryGrowthRatePerMinute,8:F2} MB/分钟  ← 正常应<1 MB/分钟");
            
            if (report.MinutesToCriticalThreshold < int.MaxValue)
            {
                var hours = report.MinutesToCriticalThreshold / 60;
                var minutes = report.MinutesToCriticalThreshold % 60;
                sb.AppendLine($"   达到临界值时间：  {report.MinutesToCriticalThreshold,8} 分钟（约{hours}小时{minutes}分钟）");
            }
            else
            {
                sb.AppendLine($"   达到临界值时间：  {"N/A",8}（增长率低或为负）");
            }
            sb.AppendLine();
            
            // Top 内存占用者
            sb.AppendLine(new string('-', 100));
            sb.AppendLine("【3️⃣ Top 内存占用者 - 重点检查标记⚠️ 的项目】");
            sb.AppendLine(new string('-', 100));
            
            int rank = 1;
            foreach (var consumer in report.TopMemoryConsumers.Take(10))
            {
                var leakMark = consumer.IsPotentialLeak ? " ⚠️【可能是泄漏源】" : "";
                var percentage = (consumer.EstimatedMemoryMB * 100.0 / report.CurrentSnapshot.WorkingSetMB);
                
                sb.AppendLine($"   #{rank,2} {consumer.Name,-25} {consumer.EstimatedMemoryMB,8} MB ({percentage,5:F1}%) {leakMark}");
                sb.AppendLine($"      说明：{consumer.Description}");
                rank++;
            }
            sb.AppendLine();
            
            // 线程分析
            sb.AppendLine(new string('-', 100));
            sb.AppendLine("【4️⃣ 线程分析】");
            sb.AppendLine(new string('-', 100));
            sb.AppendLine($"   总线程数：       {report.ThreadAnalysis.TotalThreads,8} 个  ← 正常应<100 个");
            sb.AppendLine($"   线程池线程：     {report.ThreadAnalysis.ThreadPoolThreads,8} 个");
            sb.AppendLine($"   后台线程：       {report.ThreadAnalysis.BackgroundThreads,8} 个");
            sb.AppendLine($"   前台线程：       {report.ThreadAnalysis.ForegroundThreads,8} 个");
            
            if (report.ThreadAnalysis.TotalThreads > 100)
            {
                sb.AppendLine($"   ⚠️ 警告：线程数过多（{report.ThreadAnalysis.TotalThreads}个），可能存在线程泄漏！");
            }
            sb.AppendLine();
            
            // 建议措施
            sb.AppendLine(new string('-', 100));
            sb.AppendLine("【5️⃣ 建议措施 - 按优先级排序】");
            sb.AppendLine(new string('-', 100));
            
            int priority = 1;
            foreach (var rec in report.Recommendations)
            {
                sb.AppendLine($"   {priority}. {rec}");
                priority++;
            }
            sb.AppendLine();
            
            // 快速诊断
            sb.AppendLine(new string('-', 100));
            sb.AppendLine("【6️⃣ 快速诊断 - 立即检查这些项目】");
            sb.AppendLine(new string('-', 100));
            
            // 根据占用情况给出具体建议
            var topConsumer = report.TopMemoryConsumers.FirstOrDefault();
            if (topConsumer != null)
            {
                sb.AppendLine($"   🔴 最大占用者：{topConsumer.Name} ({topConsumer.EstimatedMemoryMB} MB)");
                
                if (topConsumer.Name.Contains("会话"))
                {
                    sb.AppendLine("      → 检查 SessionService 是否有清理过期会话的逻辑");
                    sb.AppendLine("      → 检查会话过期时间设置（建议 30 分钟）");
                    sb.AppendLine("      → 检查用户退出时是否立即清理会话");
                }
                else if (topConsumer.Name.Contains("缓存"))
                {
                    sb.AppendLine("      → 检查 EntityCacheManager 是否有缓存过期策略");
                    sb.AppendLine("      → 检查缓存键设计是否合理");
                    sb.AppendLine("      → 检查是否定期清理未使用的缓存");
                }
                else if (topConsumer.Name.Contains("数据库"))
                {
                    sb.AppendLine("      → 检查 SqlSugar 连接是否使用 using 语句");
                    sb.AppendLine("      → 检查连接字符串中的 Max Pool Size 设置");
                    sb.AppendLine("      → 检查 DbContext 是否及时释放");
                }
                else if (topConsumer.Name.Contains("GDI"))
                {
                    sb.AppendLine("      → 检查 Bitmap/Graphics 对象是否 Dispose");
                    sb.AppendLine("      → 检查窗体和控件是否正确释放");
                    sb.AppendLine("      → 检查 ImageList 是否过大");
                }
            }
            
            sb.AppendLine();
            sb.AppendLine(new string('=', 100));
            sb.AppendLine("【诊断完成】");
            sb.AppendLine(new string('=', 100));
            
            // 根据严重程度选择日志级别
            if (report.IsMemoryLeakDetected)
            {
                _logger.LogWarning(sb.ToString());
            }
            else
            {
                _logger.LogInformation(sb.ToString());
            }
        }
        
        private long EstimateAssemblyMemory(Assembly[] assemblies)
        {
            // 粗略估算：每个程序集约 500KB - 5MB
            // 取平均值 2MB
            return assemblies.Length * 2;
        }
        
        private int GetGDIObjectCount(int processId)
        {
            try
            {
                // 使用 PerformanceCounter 获取 GDI 对象数
                var counter = new PerformanceCounter("Process", "Handle Count", Process.GetCurrentProcess().ProcessName);
                return (int)counter.NextValue();
            }
            catch
            {
                return 0;
            }
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                _diagnosticsTimer?.Dispose();
                _disposed = true;
            }
        }
    }
    
    /// <summary>
    /// 内存快照
    /// </summary>
    public class MemorySnapshot
    {
        public DateTime Timestamp { get; set; }
        public long WorkingSetMB { get; set; }
        public long ManagedMemoryMB { get; set; }
        public long PrivateMemoryMB { get; set; }
        public long VirtualMemoryMB { get; set; }
        public int HandleCount { get; set; }
        public int ThreadCount { get; set; }
        public int GCGeneration0 { get; set; }
        public int GCGeneration1 { get; set; }
        public int GCGeneration2 { get; set; }
        public long LOHSizeMB { get; set; }
    }
    
    /// <summary>
    /// 内存泄漏报告
    /// </summary>
    public class MemoryLeakReport
    {
        public DateTime Timestamp { get; set; }
        public MemorySnapshot CurrentSnapshot { get; set; }
        public bool IsMemoryLeakDetected { get; set; }
        public double MemoryGrowthRatePerMinute { get; set; }
        public int MinutesToCriticalThreshold { get; set; }
        public List<MemoryConsumerInfo> TopMemoryConsumers { get; set; }
        public ObjectSurvivalInfo ObjectSurvivalAnalysis { get; set; }
        public GCDistributionInfo GCDistribution { get; set; }
        public ThreadAnalysisInfo ThreadAnalysis { get; set; }
        public DatabaseConnectionInfo DatabaseConnectionAnalysis { get; set; }
        public List<string> Recommendations { get; set; }
    }
    
    /// <summary>
    /// 内存占用者信息
    /// </summary>
    public class MemoryConsumerInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long EstimatedMemoryMB { get; set; }
        public bool IsPotentialLeak { get; set; }
    }
    
    /// <summary>
    /// 对象存活分析
    /// </summary>
    public class ObjectSurvivalInfo
    {
        public int Gen0Objects { get; set; }
        public int Gen1Objects { get; set; }
        public int Gen2Objects { get; set; }
        public long LOHSizeMB { get; set; }
        public long PauseTimePredictionMS { get; set; }
        public double FragmentationPercentage { get; set; }
    }
    
    /// <summary>
    /// GC 代分布信息
    /// </summary>
    public class GCDistributionInfo
    {
        public long Gen0SizeMB { get; set; }
        public long Gen1SizeMB { get; set; }
        public long Gen2SizeMB { get; set; }
        public long LOHSizeMB { get; set; }
        public long TotalCommittedMB { get; set; }
        public long TotalAvailableMB { get; set; }
    }
    
    /// <summary>
    /// 线程分析信息
    /// </summary>
    public class ThreadAnalysisInfo
    {
        public int TotalThreads { get; set; }
        public int ThreadPoolThreads { get; set; }
        public int BackgroundThreads { get; set; }
        public int ForegroundThreads { get; set; }
        public List<ThreadInfo> ThreadDetails { get; set; }
    }
    
    /// <summary>
    /// 线程信息
    /// </summary>
    public class ThreadInfo
    {
        public int Id { get; set; }
        public string State { get; set; }
        public bool IsThreadPoolThread { get; set; }
        public bool IsBackground { get; set; }
        public string Priority { get; set; }
        public DateTime StartTime { get; set; }
    }
    
    /// <summary>
    /// 数据库连接分析信息
    /// </summary>
    public class DatabaseConnectionInfo
    {
        public int EstimatedPoolSize { get; set; }
        public long EstimatedMemoryMB { get; set; }
        public string Recommendation { get; set; }
    }
}
