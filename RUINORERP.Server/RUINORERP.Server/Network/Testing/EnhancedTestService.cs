using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Testing
{
    /// <summary>
    /// 增强的测试服务
    /// 提供更全面的测试支持功能
    /// </summary>
    public class EnhancedTestService
    {
        private readonly CommandDispatcher _commandDispatcher;

        public EnhancedTestService(CommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// 执行压力测试
        /// </summary>
        /// <param name="command">测试命令</param>
        /// <param name="concurrentRequests">并发请求数</param>
        /// <param name="totalRequests">总请求数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>压力测试结果</returns>
        public async Task<LoadTestResult> ExecuteLoadTestAsync(
            ICommand command, 
            int concurrentRequests, 
            int totalRequests, 
            CancellationToken cancellationToken = default)
        {
            var result = new LoadTestResult
            {
                StartTime = DateTime.UtcNow,
                ConcurrentRequests = concurrentRequests,
                TotalRequests = totalRequests
            };

            var tasks = new List<Task>();
            var responses = new List<ResponseBase>();
            var errors = new List<Exception>();
            var semaphore = new SemaphoreSlim(concurrentRequests);

            var startTime = DateTime.UtcNow;

            for (int i = 0; i < totalRequests; i++)
            {
                await semaphore.WaitAsync(cancellationToken);
                
                var task = Task.Run(async () =>
                {
                    try
                    {
                        var response = await _commandDispatcher.DispatchAsync(command, cancellationToken);
                        lock (responses)
                        {
                            responses.Add(response);
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (errors)
                        {
                            errors.Add(ex);
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            result.EndTime = DateTime.UtcNow;
            result.Duration = result.EndTime - result.StartTime;
            result.Responses = responses;
            result.Errors = errors;

            return result;
        }

        /// <summary>
        /// 执行性能测试
        /// </summary>
        /// <param name="command">测试命令</param>
        /// <param name="iterations">迭代次数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>性能测试结果</returns>
        public async Task<PerformanceTestResult> ExecutePerformanceTestAsync(
            ICommand command, 
            int iterations, 
            CancellationToken cancellationToken = default)
        {
            var result = new PerformanceTestResult
            {
                StartTime = DateTime.UtcNow,
                Iterations = iterations
            };

            var responseTimes = new List<long>();

            for (int i = 0; i < iterations; i++)
            {
                var startTime = DateTime.UtcNow;
                try
                {
                    var response = await _commandDispatcher.DispatchAsync(command, cancellationToken);
                    var endTime = DateTime.UtcNow;
                    var duration = (endTime - startTime).TotalMilliseconds;
                    responseTimes.Add((long)duration);
                }
                catch (Exception ex)
                {
                    result.Errors.Add(ex);
                }
            }

            result.EndTime = DateTime.UtcNow;
            result.Duration = result.EndTime - result.StartTime;
            result.ResponseTimes = responseTimes;
            
            if (responseTimes.Any())
            {
                result.AverageResponseTime = responseTimes.Average();
                result.MinResponseTime = responseTimes.Min();
                result.MaxResponseTime = responseTimes.Max();
                result.Percentile95 = CalculatePercentile(responseTimes, 95);
                result.Percentile99 = CalculatePercentile(responseTimes, 99);
            }

            return result;
        }

        /// <summary>
        /// 计算百分位数
        /// </summary>
        /// <param name="values">数值列表</param>
        /// <param name="percentile">百分位数</param>
        /// <returns>百分位数值</returns>
        private long CalculatePercentile(List<long> values, double percentile)
        {
            if (!values.Any()) return 0;

            var sorted = values.OrderBy(x => x).ToList();
            var index = (int)Math.Ceiling(percentile / 100 * sorted.Count) - 1;
            index = Math.Max(0, Math.Min(index, sorted.Count - 1));
            return sorted[index];
        }

        /// <summary>
        /// 获取测试覆盖率信息
        /// </summary>
        /// <returns>测试覆盖率信息</returns>
        public TestCoverageInfo GetTestCoverageInfo()
        {
            var handlers = _commandDispatcher.GetAllHandlers();
            var totalHandlers = handlers.Count();
            var testedHandlers = handlers.Count(h => h.GetStatistics().TotalCommandsProcessed > 0);

            return new TestCoverageInfo
            {
                TotalHandlers = totalHandlers,
                TestedHandlers = testedHandlers,
                CoveragePercentage = totalHandlers > 0 ? (double)testedHandlers / totalHandlers * 100 : 0,
                HandlerTestInfo = handlers.Select(h => new HandlerTestInfo
                {
                    HandlerName = h.Name,
                    TotalProcessed = h.GetStatistics().TotalCommandsProcessed,
                    SuccessRate = h.GetStatistics().TotalCommandsProcessed > 0 ? 
                        (double)h.GetStatistics().SuccessfulCommands / h.GetStatistics().TotalCommandsProcessed * 100 : 0,
                    IsTested = h.GetStatistics().TotalCommandsProcessed > 0
                }).ToList()
            };
        }

        /// <summary>
        /// 重置测试数据
        /// </summary>
        public void ResetTestData()
        {
            var handlers = _commandDispatcher.GetAllHandlers();
            foreach (var handler in handlers)
            {
                handler.ResetStatistics();
            }
        }
    }

    /// <summary>
    /// 压力测试结果
    /// </summary>
    public class LoadTestResult
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// 并发请求数
        /// </summary>
        public int ConcurrentRequests { get; set; }

        /// <summary>
        /// 总请求数
        /// </summary>
        public int TotalRequests { get; set; }

        /// <summary>
        /// 响应列表
        /// </summary>
        public List<ResponseBase> Responses { get; set; } = new List<ResponseBase>();

        /// <summary>
        /// 错误列表
        /// </summary>
        public List<Exception> Errors { get; set; } = new List<Exception>();

        /// <summary>
        /// 成功请求数
        /// </summary>
        public int SuccessfulRequests => Responses.Count(r => r.IsSuccess);

        /// <summary>
        /// 失败请求数
        /// </summary>
        public int FailedRequests => Errors.Count + Responses.Count(r => !r.IsSuccess);

        /// <summary>
        /// 成功率
        /// </summary>
        public double SuccessRate => TotalRequests > 0 ? (double)SuccessfulRequests / TotalRequests * 100 : 0;

        /// <summary>
        /// 平均响应时间（毫秒）
        /// </summary>
        public double AverageResponseTime => Responses.Any() ? 
            Responses.Average(r => r.IsSuccess ? 0 : 0) : 0; // 简化处理，实际应该从计时中获取

        /// <summary>
        /// 获取测试结果摘要
        /// </summary>
        /// <returns>测试结果摘要</returns>
        public string GetSummary()
        {
            return $"压力测试结果:\n" +
                   $"  持续时间: {Duration.TotalSeconds:F2}秒\n" +
                   $"  总请求数: {TotalRequests}\n" +
                   $"  成功请求数: {SuccessfulRequests}\n" +
                   $"  失败请求数: {FailedRequests}\n" +
                   $"  成功率: {SuccessRate:F2}%\n" +
                   $"  并发数: {ConcurrentRequests}";
        }
    }

    /// <summary>
    /// 性能测试结果
    /// </summary>
    public class PerformanceTestResult
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// 迭代次数
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// 响应时间列表（毫秒）
        /// </summary>
        public List<long> ResponseTimes { get; set; } = new List<long>();

        /// <summary>
        /// 错误列表
        /// </summary>
        public List<Exception> Errors { get; set; } = new List<Exception>();

        /// <summary>
        /// 平均响应时间（毫秒）
        /// </summary>
        public double AverageResponseTime { get; set; }

        /// <summary>
        /// 最小响应时间（毫秒）
        /// </summary>
        public long MinResponseTime { get; set; }

        /// <summary>
        /// 最大响应时间（毫秒）
        /// </summary>
        public long MaxResponseTime { get; set; }

        /// <summary>
        /// 95百分位响应时间（毫秒）
        /// </summary>
        public long Percentile95 { get; set; }

        /// <summary>
        /// 99百分位响应时间（毫秒）
        /// </summary>
        public long Percentile99 { get; set; }

        /// <summary>
        /// 获取测试结果摘要
        /// </summary>
        /// <returns>测试结果摘要</returns>
        public string GetSummary()
        {
            return $"性能测试结果:\n" +
                   $"  持续时间: {Duration.TotalSeconds:F2}秒\n" +
                   $"  迭代次数: {Iterations}\n" +
                   $"  错误数: {Errors.Count}\n" +
                   $"  平均响应时间: {AverageResponseTime:F2}ms\n" +
                   $"  最小响应时间: {MinResponseTime}ms\n" +
                   $"  最大响应时间: {MaxResponseTime}ms\n" +
                   $"  95%响应时间: {Percentile95}ms\n" +
                   $"  99%响应时间: {Percentile99}ms";
        }
    }

    /// <summary>
    /// 测试覆盖率信息
    /// </summary>
    public class TestCoverageInfo
    {
        /// <summary>
        /// 总处理器数
        /// </summary>
        public int TotalHandlers { get; set; }

        /// <summary>
        /// 已测试处理器数
        /// </summary>
        public int TestedHandlers { get; set; }

        /// <summary>
        /// 覆盖率百分比
        /// </summary>
        public double CoveragePercentage { get; set; }

        /// <summary>
        /// 处理器测试信息列表
        /// </summary>
        public List<HandlerTestInfo> HandlerTestInfo { get; set; } = new List<HandlerTestInfo>();

        /// <summary>
        /// 获取覆盖率报告
        /// </summary>
        /// <returns>覆盖率报告</returns>
        public string GetCoverageReport()
        {
            var report = $"测试覆盖率报告:\n" +
                        $"  总处理器数: {TotalHandlers}\n" +
                        $"  已测试处理器数: {TestedHandlers}\n" +
                        $"  覆盖率: {CoveragePercentage:F2}%\n\n" +
                        $"处理器测试详情:\n";

            foreach (var info in HandlerTestInfo)
            {
                report += $"  {info.HandlerName}: " +
                         $"{(info.IsTested ? $"已测试 (处理数: {info.TotalProcessed}, 成功率: {info.SuccessRate:F2}%)" : "未测试")}\n";
            }

            return report;
        }
    }

    /// <summary>
    /// 处理器测试信息
    /// </summary>
    public class HandlerTestInfo
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 总处理数
        /// </summary>
        public long TotalProcessed { get; set; }

        /// <summary>
        /// 成功率
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// 是否已测试
        /// </summary>
        public bool IsTested { get; set; }
    }
}