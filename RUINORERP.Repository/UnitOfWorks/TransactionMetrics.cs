using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Repository.UnitOfWorks
{
    /// <summary>
    /// 事务性能监控指标
    /// 集成 Prometheus 监控，提供事务性能数据
    /// </summary>
    public static class TransactionMetrics
    {
        // 由于我们没有引入 Prometheus 库，这里使用简单的计数器和计时器
        // 实际项目中可以替换为 Prometheus 的 Counter, Histogram 等类型

        /// <summary>
        /// 事务总数（按操作类型和状态分组）
        /// </summary>
        private static readonly Dictionary<string, long> _transactionCounts = new Dictionary<string, long>();

        /// <summary>
        /// 事务持续时间记录（用于计算百分位数）
        /// </summary>
        private static readonly List<TransactionDuration> _durations = new List<TransactionDuration>();

        /// <summary>
        /// 锁对象，保护共享数据
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// 最大保留的持续时间记录数
        /// </summary>
        private const int MAX_DURATION_RECORDS = 10000;

        /// <summary>
        /// 事务持续时间记录
        /// </summary>
        private class TransactionDuration
        {
            public string Operation { get; set; }
            public string Caller { get; set; }
            public double DurationSeconds { get; set; }
            public bool Succeeded { get; set; }
            public DateTime Timestamp { get; set; }
        }

        /// <summary>
        /// 记录事务指标
        /// </summary>
        /// <param name="operation">操作类型（begin/commit/rollback）</param>
        /// <param name="caller">调用者方法</param>
        /// <param name="durationSeconds">持续时间（秒）</param>
        /// <param name="succeeded">是否成功</param>
        public static void RecordTransaction(string operation, string caller, double durationSeconds, bool succeeded)
        {
            lock (_lock)
            {
                // 记录事务总数
                var key = $"operation:{operation},caller:{caller},status:{(succeeded ? "success" : "failure")}";
                if (!_transactionCounts.ContainsKey(key))
                {
                    _transactionCounts[key] = 0;
                }
                _transactionCounts[key]++;

                // 记录持续时间（只记录commit操作，且最多保留MAX_DURATION_RECORDS条）
                if (operation == "commit" || operation == "rollback")
                {
                    if (_durations.Count >= MAX_DURATION_RECORDS)
                    {
                        // 移除最旧的记录
                        _durations.RemoveAt(0);
                    }

                    _durations.Add(new TransactionDuration
                    {
                        Operation = operation,
                        Caller = caller,
                        DurationSeconds = durationSeconds,
                        Succeeded = succeeded,
                        Timestamp = DateTime.Now
                    });
                }
            }
        }

        /// <summary>
        /// 获取事务统计信息
        /// </summary>
        public static TransactionStatistics GetStatistics()
        {
            lock (_lock)
            {
                var stats = new TransactionStatistics();

                // 计算总数
                foreach (var kvp in _transactionCounts)
                {
                    stats.TotalTransactions += kvp.Value;
                }

                // 计算持续时间统计
                if (_durations.Any())
                {
                    var durations = _durations.Select(d => d.DurationSeconds).ToList();
                    durations.Sort();

                    stats.MinDuration = durations.First();
                    stats.MaxDuration = durations.Last();
                    stats.AvgDuration = durations.Average();
                    stats.P50Duration = GetPercentile(durations, 0.5);
                    stats.P95Duration = GetPercentile(durations, 0.95);
                    stats.P99Duration = GetPercentile(durations, 0.99);
                }

                return stats;
            }
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
            lock (_lock)
            {
                _transactionCounts.Clear();
                _durations.Clear();
            }
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
        /// 转换为可读的字符串
        /// </summary>
        public override string ToString()
        {
            return $"Transaction Statistics:\r\n" +
                   $"  Total: {TotalTransactions}\r\n" +
                   $"  Min Duration: {MinDuration:F3}s\r\n" +
                   $"  Max Duration: {MaxDuration:F3}s\r\n" +
                   $"  Avg Duration: {AvgDuration:F3}s\r\n" +
                   $"  P50 Duration: {P50Duration:F3}s\r\n" +
                   $"  P95 Duration: {P95Duration:F3}s\r\n" +
                   $"  P99 Duration: {P99Duration:F3}s";
        }
    }
}
