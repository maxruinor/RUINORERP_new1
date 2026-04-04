using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Repository.UnitOfWorks
{
    /// <summary>
    /// 事务性能监控指标 - 增强版
    /// 集成死锁统计、Top N 长事务、热点表分析
    /// </summary>
    public static class TransactionMetrics
    {
        // 事务总数（按操作类型和状态分组）
        private static readonly ConcurrentDictionary<string, long> _transactionCounts = new ConcurrentDictionary<string, long>();

        // 事务持续时间记录（用于计算百分位数）
        private static readonly ConcurrentQueue<TransactionDuration> _durations = new ConcurrentQueue<TransactionDuration>();

        // 死锁统计
        private static  ConcurrentQueue<DeadlockStatistic> _deadlockStats = new ConcurrentQueue<DeadlockStatistic>();

        // Top N 长事务记录
        private static readonly ConcurrentBag<TransactionDuration> _topLongTransactions = new ConcurrentBag<TransactionDuration>();

        // 热点表统计（按表名分组的事务次数）
        private static readonly ConcurrentDictionary<string, TableStatistics> _tableStats = new ConcurrentDictionary<string, TableStatistics>();

        // 锁对象，保护共享数据
        private static readonly object _durationLock = new object();
        private static readonly object _deadlockLock = new object();
        private static readonly object _topTransactionsLock = new object();

        /// <summary>
        /// 最大保留的持续时间记录数
        /// </summary>
        private const int MAX_DURATION_RECORDS = 10000;

        /// <summary>
        /// Top N 长事务数量
        /// </summary>
        private const int TOP_N_COUNT = 20;

        /// <summary>
        /// 死锁统计保留天数
        /// </summary>
        private const int DEADLOCK_RETENTION_DAYS = 7;

        /// <summary>
        /// 记录事务指标
        /// </summary>
        /// <param name="operation">操作类型（begin/commit/rollback）</param>
        /// <param name="caller">调用者方法</param>
        /// <param name="durationSeconds">持续时间（秒）</param>
        /// <param name="succeeded">是否成功</param>
        /// <param name="tableName">涉及的表名（可选）</param>
        public static void RecordTransaction(
            string operation, 
            string caller, 
            double durationSeconds, 
            bool succeeded,
            string tableName = null)
        {
            // 记录事务次数
            var key = $"operation:{operation},caller:{caller},status:{(succeeded ? "success" : "failure")}";
            _transactionCounts.AddOrUpdate(key, 1, (k, v) => v + 1);
        
            // 记录持续时间（只记录 commit/rollback）
            if (operation == "commit" || operation == "rollback")
            {
                var duration = new TransactionDuration
                {
                    Operation = operation,
                    Caller = caller,
                    DurationSeconds = durationSeconds,
                    Succeeded = succeeded,
                    TableName = tableName,
                    Timestamp = DateTime.Now
                };
        
                lock (_durationLock)
                {
                    // 清理旧记录
                    while (_durations.Count >= MAX_DURATION_RECORDS)
                    {
                        _durations.TryDequeue(out _);
                    }
        
                    _durations.Enqueue(duration);
                }
        
                // 检查是否进入 Top N 长事务
                CheckTopLongTransactions(duration);
        
                // 按表统计
                if (!string.IsNullOrEmpty(tableName))
                {
                    _tableStats.AddOrUpdate(tableName, 
                        new TableStatistics { TableName = tableName, TransactionCount = 1, TotalDuration = durationSeconds },
                        (k, v) => 
                        {
                            v.TransactionCount++;
                            v.TotalDuration += durationSeconds;
                            return v;
                        });
                }
            }
        }
        
        /// <summary>
        /// 记录死锁事件
        /// </summary>
        /// <param name="tableName">死锁涉及的表名</param>
        /// <param name="operation">导致死锁的操作</param>
        /// <param name="duration">等待时间</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="victim">牺牲者信息</param>
        public static void RecordDeadlock(
            string tableName, 
            string operation, 
            TimeSpan duration,
            string sql = null,
            string victim = null)
        {
            var deadlockStat = new DeadlockStatistic
            {
                TableName = tableName,
                Operation = operation,
                Duration = duration,
                Sql = sql,
                Victim = victim,
                Time = DateTime.Now
            };
        
            lock (_deadlockLock)
            {
                _deadlockStats.Enqueue(deadlockStat);
        
                // 清理超过 retention 天数的记录
                CleanupOldDeadlockRecords();
            }
        }
        
        /// <summary>
        /// 检查并更新 Top N 长事务
        /// </summary>
        private static void CheckTopLongTransactions(TransactionDuration duration)
        {
            lock (_topTransactionsLock)
            {
                // 如果当前事务比最慢的 Top N 事务还慢，则加入
                if (_topLongTransactions.Count < TOP_N_COUNT)
                {
                    _topLongTransactions.Add(duration);
                }
                else
                {
                    // 找到最慢的那个
                    var minDuration = _topLongTransactions.Min(d => d.DurationSeconds);
                    if (duration.DurationSeconds > minDuration)
                    {
                        // 移除最慢的，添加新的
                        var toRemove = _topLongTransactions.FirstOrDefault(d => d.DurationSeconds == minDuration);
                        if (toRemove != null)
                        {
                            _topLongTransactions.TryTake(out toRemove);
                            _topLongTransactions.Add(duration);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 清理旧的死锁记录
        /// </summary>
        private static void CleanupOldDeadlockRecords()
        {
            var cutoffTime = DateTime.Now.AddDays(-DEADLOCK_RETENTION_DAYS);
            var validItems = _deadlockStats.Where(d => d.Time >= cutoffTime).ToList();

            _deadlockStats = new ConcurrentQueue<DeadlockStatistic>();
            foreach (var item in validItems)
            {
                _deadlockStats.Enqueue(item);
            }
        }

        /// <summary>
        /// 获取事务统计信息
        /// </summary>
        public static TransactionStatistics GetStatistics()
        {
            var stats = new TransactionStatistics();

            // 计算总数
            stats.TotalTransactions = _transactionCounts.Values.Sum();
            stats.SuccessfulTransactions = _transactionCounts
                .Where(kvp => kvp.Key.Contains("status:success"))
                .Sum(kvp => kvp.Value);
            stats.FailedTransactions = _transactionCounts
                .Where(kvp => kvp.Key.Contains("status:failure"))
                .Sum(kvp => kvp.Value);

            // 计算持续时间统计
            lock (_durationLock)
            {
                if (_durations.Any())
                {
                    var durations = _durations.Select(d => d.DurationSeconds).OrderBy(d => d).ToList();

                    stats.MinDuration = durations.First();
                    stats.MaxDuration = durations.Last();
                    stats.AvgDuration = durations.Average();
                    stats.P50Duration = GetPercentile(durations, 0.5);
                    stats.P95Duration = GetPercentile(durations, 0.95);
                    stats.P99Duration = GetPercentile(durations, 0.99);
                }
            }

            // Top N 长事务
            lock (_topTransactionsLock)
            {
                stats.TopLongTransactions = _topLongTransactions
                    .OrderByDescending(d => d.DurationSeconds)
                    .Take(TOP_N_COUNT)
                    .ToList();
            }

            // 热点表统计
            stats.HotTables = _tableStats.Values
                .OrderByDescending(t => t.TransactionCount)
                .Take(20)
                .ToList();

            // 死锁统计
            lock (_deadlockLock)
            {
                stats.DeadlockCount = _deadlockStats.Count;
                stats.RecentDeadlocks = _deadlockStats
                    .OrderByDescending(d => d.Time)
                    .Take(10)
                    .ToList();
            }

            return stats;
        }

        /// <summary>
        /// 获取百分位数
        /// </summary>
        private static double GetPercentile(List<double> sortedData, double percentile)
        {
            if (!sortedData.Any())
                return 0;

            var index = (int)Math.Ceiling(sortedData.Count * percentile) - 1;
            index = Math.Max(0, Math.Min(index, sortedData.Count - 1));
            return sortedData[index];
        }

        /// <summary>
        /// 重置统计数据（用于测试）
        /// </summary>
        public static void Reset()
        {
            _transactionCounts.Clear();
            
            lock (_durationLock)
            {
                while (_durations.TryDequeue(out _)) { }
            }

            lock (_deadlockLock)
            {
                while (_deadlockStats.TryDequeue(out _)) { }
            }

            lock (_topTransactionsLock)
            {
                while (_topLongTransactions.TryTake(out _)) { }
            }

            _tableStats.Clear();
        }

        /// <summary>
        /// 导出统计报告
        /// </summary>
        public static string ExportReport()
        {
            var stats = GetStatistics();
            var sb = new StringBuilder();

            sb.AppendLine("=== 事务性能统计报告 ===");
            sb.AppendLine($"生成时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();
            sb.AppendLine("--- 总体统计 ---");
            sb.AppendLine($"总事务数：{stats.TotalTransactions}");
            sb.AppendLine($"成功：{stats.SuccessfulTransactions}");
            sb.AppendLine($"失败：{stats.FailedTransactions}");
            sb.AppendLine($"成功率：{(stats.TotalTransactions > 0 ? (double)stats.SuccessfulTransactions / stats.TotalTransactions * 100 : 0):F2}%");
            sb.AppendLine();
            sb.AppendLine("--- 持续时间统计 ---");
            sb.AppendLine($"最小：{stats.MinDuration:F3}秒");
            sb.AppendLine($"最大：{stats.MaxDuration:F3}秒");
            sb.AppendLine($"平均：{stats.AvgDuration:F3}秒");
            sb.AppendLine($"P50: {stats.P50Duration:F3}秒");
            sb.AppendLine($"P95: {stats.P95Duration:F3}秒");
            sb.AppendLine($"P99: {stats.P99Duration:F3}秒");
            sb.AppendLine();
            sb.AppendLine($"--- 死锁统计 ---");
            sb.AppendLine($"死锁总数：{stats.DeadlockCount}");
            if (stats.RecentDeadlocks.Any())
            {
                sb.AppendLine("最近死锁:");
                foreach (var dl in stats.RecentDeadlocks.Take(5))
                {
                    sb.AppendLine($"  [{dl.Time:HH:mm:ss}] 表：{dl.TableName}, 操作：{dl.Operation}, 耗时：{dl.Duration.TotalMilliseconds:F0}ms");
                }
            }
            sb.AppendLine();
            sb.AppendLine("--- Top 10 长事务 ---");
            foreach (var top in stats.TopLongTransactions.Take(10))
            {
                sb.AppendLine($"  {top.DurationSeconds:F2}秒 - {top.Caller} ({top.TableName})");
            }
            sb.AppendLine();
            sb.AppendLine("--- Top 10 热点表 ---");
            foreach (var table in stats.HotTables.Take(10))
            {
                sb.AppendLine($"  {table.TableName}: {table.TransactionCount}次，平均{table.AverageDuration:F3}秒");
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// 事务统计信息
    /// </summary>
    public class TransactionStatistics
    {
        /// <summary>
        /// 总事务数
        /// </summary>
        public long TotalTransactions { get; set; }

        /// <summary>
        /// 成功事务数
        /// </summary>
        public long SuccessfulTransactions { get; set; }

        /// <summary>
        /// 失败事务数
        /// </summary>
        public long FailedTransactions { get; set; }

        /// <summary>
        /// 最小持续时间（秒）
        /// </summary>
        public double MinDuration { get; set; }

        /// <summary>
        /// 最大持续时间（秒）
        /// </summary>
        public double MaxDuration { get; set; }

        /// <summary>
        /// 平均持续时间（秒）
        /// </summary>
        public double AvgDuration { get; set; }

        /// <summary>
        /// 中位数（P50）持续时间（秒）
        /// </summary>
        public double P50Duration { get; set; }

        /// <summary>
        /// P95 持续时间（秒）
        /// </summary>
        public double P95Duration { get; set; }

        /// <summary>
        /// P99 持续时间（秒）
        /// </summary>
        public double P99Duration { get; set; }

        /// <summary>
        /// Top N 长事务列表
        /// </summary>
        public List<TransactionDuration> TopLongTransactions { get; set; } = new List<TransactionDuration>();

        /// <summary>
        /// 热点表列表
        /// </summary>
        public List<TableStatistics> HotTables { get; set; } = new List<TableStatistics>();

        /// <summary>
        /// 死锁总数
        /// </summary>
        public int DeadlockCount { get; set; }

        /// <summary>
        /// 最近死锁列表
        /// </summary>
        public List<DeadlockStatistic> RecentDeadlocks { get; set; } = new List<DeadlockStatistic>();

        /// <summary>
        /// 转换为可读的字符串
        /// </summary>
        public override string ToString()
        {
            return $"Transaction Statistics:\r\n" +
                   $"  Total: {TotalTransactions}\r\n" +
                   $"  Success: {SuccessfulTransactions}\r\n" +
                   $"  Failed: {FailedTransactions}\r\n" +
                   $"  Min Duration: {MinDuration:F3}s\r\n" +
                   $"  Max Duration: {MaxDuration:F3}s\r\n" +
                   $"  Avg Duration: {AvgDuration:F3}s\r\n" +
                   $"  P50 Duration: {P50Duration:F3}s\r\n" +
                   $"  P95 Duration: {P95Duration:F3}s\r\n" +
                   $"  P99 Duration: {P99Duration:F3}s\r\n" +
                   $"  Deadlocks: {DeadlockCount}\r\n" +
                   $"  Hot Tables: {HotTables.Count}";
        }
    }

    /// <summary>
    /// 事务持续时间记录
    /// </summary>
    public class TransactionDuration
    {
        public string Operation { get; set; }
        public string Caller { get; set; }
        public double DurationSeconds { get; set; }
        public bool Succeeded { get; set; }
        public string TableName { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 死锁统计
    /// </summary>
    public class DeadlockStatistic
    {
        public string TableName { get; set; }
        public string Operation { get; set; }
        public TimeSpan Duration { get; set; }
        public string Sql { get; set; }
        public string Victim { get; set; }
        public DateTime Time { get; set; }
    }

    /// <summary>
    /// 表统计信息
    /// </summary>
    public class TableStatistics
    {
        public string TableName { get; set; }
        public long TransactionCount { get; set; }
        public double TotalDuration { get; set; }
        public double AverageDuration => TransactionCount > 0 ? TotalDuration / TransactionCount : 0;
    }
}
