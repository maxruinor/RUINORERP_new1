using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Performance
{
    /// <summary>
    /// 性能指标数据模型
    /// </summary>
    public class PerformanceMetric
    {
        /// <summary>
        /// 指标名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }
        
        /// <summary>
        /// 内存使用（字节）
        /// </summary>
        public long MemoryUsageBytes { get; set; }
        
        /// <summary>
        /// GC 收集次数
        /// </summary>
        public int GcCollectionCount { get; set; }
        
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// 附加信息
        /// </summary>
        public string AdditionalInfo { get; set; }
    }

    /// <summary>
    /// 性能基准测试工具类
    /// 用于测量和记录各种操作的性能指标
    /// </summary>
    public class PerformanceBenchmark
    {
        private readonly Stopwatch _stopwatch;
        private readonly ConcurrentBag<PerformanceMetric> _metrics;
        private readonly ILogger _logger;
        private readonly long _memoryThresholdWarning;
        private readonly long _executionTimeThresholdWarning;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="memoryThresholdWarningMB">内存告警阈值（MB）</param>
        /// <param name="executionTimeThresholdWarningMs">执行时间告警阈值（毫秒）</param>
        public PerformanceBenchmark(
            ILogger logger = null, 
            long memoryThresholdWarningMB = 500,
            long executionTimeThresholdWarningMs = 3000)
        {
            _stopwatch = new Stopwatch();
            _metrics = new ConcurrentBag<PerformanceMetric>();
            _logger = logger;
            _memoryThresholdWarning = memoryThresholdWarningMB * 1024 * 1024;
            _executionTimeThresholdWarning = executionTimeThresholdWarningMs;
        }

        /// <summary>
        /// 开始测量
        /// </summary>
        /// <param name="metricName">指标名称</param>
        public void Start(string metricName)
        {
            _stopwatch.Restart();
            GC.Collect(); // 强制 GC 以便获得更准确的内存使用
            GC.WaitForPendingFinalizers();
            
            var initialMemory = Process.GetCurrentProcess().WorkingSet64;
            var initialGcCount = GC.CollectionCount(0);
            
            _logger?.LogDebug($"开始测量：{metricName}, 初始内存：{initialMemory / 1024 / 1024}MB");
        }

        /// <summary>
        /// 停止测量并记录结果
        /// </summary>
        /// <param name="metricName">指标名称</param>
        /// <param name="additionalInfo">附加信息</param>
        /// <returns>性能指标</returns>
        public PerformanceMetric Stop(string metricName, string additionalInfo = null)
        {
            _stopwatch.Stop();
            
            var finalMemory = Process.GetCurrentProcess().WorkingSet64;
            var finalGcCount = GC.CollectionCount(0);
            
            var metric = new PerformanceMetric
            {
                Name = metricName,
                ExecutionTimeMs = _stopwatch.ElapsedMilliseconds,
                MemoryUsageBytes = finalMemory,
                GcCollectionCount = finalGcCount,
                Timestamp = DateTime.Now,
                AdditionalInfo = additionalInfo
            };
            
            _metrics.Add(metric);
            
            // 记录日志
            _logger?.LogInformation(
                $"性能指标 - {metricName}: " +
                $"耗时={metric.ExecutionTimeMs}ms, " +
                $"内存={metric.MemoryUsageBytes / 1024 / 1024}MB, " +
                $"GC 次数={metric.GcCollectionCount}");
            
            // 检查是否超过阈值
            CheckThresholds(metric);
            
            return metric;
        }

        /// <summary>
        /// 异步测量方法
        /// </summary>
        /// <param name="metricName">指标名称</param>
        /// <param name="action">要执行的操作</param>
        /// <param name="additionalInfo">附加信息</param>
        /// <returns>性能指标</returns>
        public async Task<PerformanceMetric> MeasureAsync(
            string metricName, 
            Func<Task> action, 
            string additionalInfo = null)
        {
            Start(metricName);
            
            await action();
            
            return Stop(metricName, additionalInfo);
        }

        /// <summary>
        /// 同步测量方法
        /// </summary>
        /// <param name="metricName">指标名称</param>
        /// <param name="action">要执行的操作</param>
        /// <param name="additionalInfo">附加信息</param>
        /// <returns>性能指标</returns>
        public PerformanceMetric Measure(
            string metricName, 
            Action action, 
            string additionalInfo = null)
        {
            Start(metricName);
            
            action();
            
            return Stop(metricName, additionalInfo);
        }

        /// <summary>
        /// 测量窗体加载时间
        /// </summary>
        /// <typeparam name="T">窗体类型</typeparam>
        /// <param name="createForm">创建窗体的工厂方法</param>
        /// <param name="loadDataAction">数据加载操作</param>
        /// <returns>性能指标</returns>
        public async Task<PerformanceMetric> MeasureFormLoadAsync<T>(
            Func<T> createForm,
            Func<T, Task> loadDataAction) where T : Form
        {
            var metricName = $"Form_Load_{typeof(T).Name}";
            Start(metricName);
            
            var form = createForm();
            await loadDataAction(form);
            
            form.Close();
            form.Dispose();
            
            return Stop(metricName, $"窗体：{typeof(T).Name}");
        }

        /// <summary>
        /// 测量数据绑定时间
        /// </summary>
        /// <param name="bindingName">绑定名称</param>
        /// <param name="bindAction">绑定操作</param>
        /// <param name="dataCount">数据量</param>
        /// <returns>性能指标</returns>
        public PerformanceMetric MeasureDataBinding(
            string bindingName,
            Action bindAction,
            int dataCount = 0)
        {
            return Measure(
                $"DataBinding_{bindingName}",
                bindAction,
                $"数据量：{dataCount}");
        }

        /// <summary>
        /// 测量数据库查询时间
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="queryName">查询名称</param>
        /// <param name="queryAction">查询操作</param>
        /// <returns>性能指标和数据结果</returns>
        public async Task<(PerformanceMetric Metric, T Result)> MeasureQueryAsync<T>(
            string queryName,
            Func<Task<T>> queryAction)
        {
            Start($"Query_{queryName}");
            
            var result = await queryAction();
            
            var metric = Stop($"Query_{queryName}", $"查询：{queryName}");
            
            return (metric, result);
        }

        /// <summary>
        /// 检查性能指标是否超过阈值
        /// </summary>
        /// <param name="metric">性能指标</param>
        private void CheckThresholds(PerformanceMetric metric)
        {
            if (metric.ExecutionTimeMs > _executionTimeThresholdWarning)
            {
                _logger?.LogWarning(
                    $"⚠️ 执行时间过长：{metric.Name} 耗时 {metric.ExecutionTimeMs}ms " +
                    $"(阈值：{_executionTimeThresholdWarning}ms)");
            }
            
            if (metric.MemoryUsageBytes > _memoryThresholdWarning)
            {
                _logger?.LogWarning(
                    $"⚠️ 内存占用过高：{metric.Name} 占用 {metric.MemoryUsageBytes / 1024 / 1024}MB " +
                    $"(阈值：{_memoryThresholdWarning / 1024 / 1024}MB)");
            }
        }

        /// <summary>
        /// 获取所有性能指标
        /// </summary>
        /// <returns>性能指标列表</returns>
        public IEnumerable<PerformanceMetric> GetAllMetrics()
        {
            return _metrics.ToList();
        }

        /// <summary>
        /// 按名称获取性能指标
        /// </summary>
        /// <param name="name">指标名称</param>
        /// <returns>性能指标列表</returns>
        public IEnumerable<PerformanceMetric> GetMetricsByName(string name)
        {
            return _metrics.Where(m => m.Name.Contains(name)).ToList();
        }

        /// <summary>
        /// 生成性能报告
        /// </summary>
        /// <returns>性能报告字符串</returns>
        public string GenerateReport()
        {
            var report = new System.Text.StringBuilder();
            report.AppendLine("=== 性能基准测试报告 ===");
            report.AppendLine();
            
            // 按指标名称分组统计
            var groupedMetrics = _metrics
                .GroupBy(m => m.Name)
                .OrderByDescending(g => g.Average(m => m.ExecutionTimeMs));
            
            foreach (var group in groupedMetrics)
            {
                report.AppendLine($"指标：{group.Key}");
                report.AppendLine($"  执行次数：{group.Count()}");
                report.AppendLine($"  平均耗时：{group.Average(m => m.ExecutionTimeMs):F2}ms");
                report.AppendLine($"  最短耗时：{group.Min(m => m.ExecutionTimeMs)}ms");
                report.AppendLine($"  最长耗时：{group.Max(m => m.ExecutionTimeMs)}ms");
                report.AppendLine($"  平均内存：{group.Average(m => m.MemoryUsageBytes) / 1024 / 1024:F2}MB");
                report.AppendLine($"  平均 GC 次数：{group.Average(m => m.GcCollectionCount):F2}");
                report.AppendLine();
            }
            
            // 总体统计
            report.AppendLine("=== 总体统计 ===");
            report.AppendLine($"总测量次数：{_metrics.Count}");
            report.AppendLine($"总耗时：{_metrics.Sum(m => m.ExecutionTimeMs)}ms");
            report.AppendLine($"平均内存占用：{_metrics.Average(m => m.MemoryUsageBytes) / 1024 / 1024:F2}MB");
            
            return report.ToString();
        }

        /// <summary>
        /// 导出性能指标为 CSV 格式
        /// </summary>
        /// <returns>CSV 格式字符串</returns>
        public string ExportToCsv()
        {
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Name,ExecutionTimeMs,MemoryUsageBytes,GcCollectionCount,Timestamp,AdditionalInfo");
            
            foreach (var metric in _metrics.OrderBy(m => m.Timestamp))
            {
                csv.AppendLine($"{metric.Name},{metric.ExecutionTimeMs},{metric.MemoryUsageBytes}," +
                              $"{metric.GcCollectionCount},{metric.Timestamp:yyyy-MM-dd HH:mm:ss}," +
                              $"\"{metric.AdditionalInfo}\"");
            }
            
            return csv.ToString();
        }

        /// <summary>
        /// 清空所有性能指标
        /// </summary>
        public void Clear()
        {
            _metrics.Clear();
            _logger?.LogInformation("性能指标已清空");
        }
    }

    /// <summary>
    /// 性能监控器
    /// 持续监控系统性能指标
    /// </summary>
    public class PerformanceMonitor : IDisposable
    {
        private readonly Timer _memoryTimer;
        private readonly Timer _cpuTimer;
        private readonly Timer _gcTimer;
        private readonly ILogger _logger;
        private readonly PerformanceCounter _cpuCounter;
        private readonly long _memoryThresholdWarningMB;
        private readonly float _cpuThresholdWarning;
        private bool _disposed;

        /// <summary>
        /// 内存告警事件
        /// </summary>
        public event EventHandler<long> MemoryThresholdExceeded;
        
        /// <summary>
        /// CPU 告警事件
        /// </summary>
        public event EventHandler<float> CpuThresholdExceeded;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="memoryThresholdWarningMB">内存告警阈值（MB）</param>
        /// <param name="cpuThresholdWarning">CPU 告警阈值（百分比）</param>
        public PerformanceMonitor(
            ILogger logger = null,
            long memoryThresholdWarningMB = 800,
            float cpuThresholdWarning = 80.0f)
        {
            _logger = logger;
            _memoryThresholdWarningMB = memoryThresholdWarningMB;
            _cpuThresholdWarning = cpuThresholdWarning;
            
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建 CPU 性能计数器失败");
            }
            
            _memoryTimer = new Timer(OnMemoryCheck, null, 0, 60000); // 每分钟检查
            _cpuTimer = new Timer(OnCpuCheck, null, 0, 5000); // 每 5 秒检查
            _gcTimer = new Timer(OnGcCheck, null, 0, 30000); // 每 30 秒检查 GC
        }

        /// <summary>
        /// 内存检查回调
        /// </summary>
        private void OnMemoryCheck(object state)
        {
            try
            {
                var memory = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;
                
                if (memory > _memoryThresholdWarningMB)
                {
                    _logger?.LogWarning($"⚠️ 内存占用过高：{memory}MB (阈值：{_memoryThresholdWarningMB}MB)");
                    MemoryThresholdExceeded?.Invoke(this, memory);
                }
                else
                {
                    _logger?.LogDebug($"内存占用：{memory}MB");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "内存检查失败");
            }
        }

        /// <summary>
        /// CPU 检查回调
        /// </summary>
        private void OnCpuCheck(object state)
        {
            try
            {
                if (_cpuCounter == null) return;
                
                var cpu = _cpuCounter.NextValue();
                
                if (cpu > _cpuThresholdWarning)
                {
                    _logger?.LogWarning($"⚠️ CPU 占用过高：{cpu:F1}% (阈值：{_cpuThresholdWarning}%)");
                    CpuThresholdExceeded?.Invoke(this, cpu);
                }
                else
                {
                    _logger?.LogDebug($"CPU 占用：{cpu:F1}%");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "CPU 检查失败");
            }
        }

        /// <summary>
        /// GC 检查回调
        /// </summary>
        private void OnGcCheck(object state)
        {
            try
            {
                var gcCount0 = GC.CollectionCount(0);
                var gcCount1 = GC.CollectionCount(1);
                var gcCount2 = GC.CollectionCount(2);
                
                _logger?.LogDebug($"GC 统计 - Gen0: {gcCount0}, Gen1: {gcCount1}, Gen2: {gcCount2}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "GC 检查失败");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _memoryTimer?.Dispose();
                _cpuTimer?.Dispose();
                _gcTimer?.Dispose();
                _cpuCounter?.Dispose();
                _disposed = true;
            }
        }
    }
}
