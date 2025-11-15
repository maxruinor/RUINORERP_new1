using System;
using System.Diagnostics;
using System.Reflection;
using RUINORERP.UI;
using RUINORERP.IServices;
using RUINORERP.Business.Cache;

namespace RUINORERP.UI.Testing
{
    /// <summary>
    /// 服务缓存性能测试类，用于测试和比较使用缓存前后的性能差异
    /// </summary>
    public static class ServiceCachePerformanceTest
    {
        /// <summary>
        /// 执行服务获取性能测试
        /// </summary>
        /// <param name="serviceTypeName">要测试的服务类型名称</param>
        /// <param name="iterations">测试迭代次数</param>
        /// <param name="clearCacheBeforeTest">是否在测试前清空缓存</param>
        /// <returns>测试结果字符串</returns>
        public static string RunPerformanceTest(string serviceTypeName, int iterations = 1000, bool clearCacheBeforeTest = true)
        {
            try
            {
                // 获取服务类型
                Type serviceType = Type.GetType(serviceTypeName);
                if (serviceType == null)
                {
                    return $"错误: 无法找到类型 '{serviceTypeName}'";
                }

                // 如果需要，清空缓存
                if (clearCacheBeforeTest)
                {
                    Startup.ClearServiceCache();
                }

                // 记录初始统计信息
                string initialStats = Startup.GetServiceCacheStatistics();

                // 创建计时器
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // 执行测试
                for (int i = 0; i < iterations; i++)
                {
                    // 使用反射调用GetFromFac方法
                    var method = typeof(Startup).GetMethod("GetFromFac").MakeGenericMethod(serviceType);
                    var serviceResult = method.Invoke(null, null);
                    
                    if (serviceResult == null)
                    {
                        return $"错误: 第 {i} 次调用返回 null";
                    }
                }

                stopwatch.Stop();

                // 记录最终统计信息
                string finalStats = Startup.GetServiceCacheStatistics();

                // 计算性能指标
                double totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
                double avgMillisecondsPerCall = totalMilliseconds / iterations;
                double callsPerSecond = iterations / (totalMilliseconds / 1000);

                // 格式化结果
                string result = $"服务类型: {serviceTypeName}\n" +
                               $"迭代次数: {iterations:N0}\n" +
                               $"总耗时: {totalMilliseconds:F2} 毫秒\n" +
                               $"平均每次调用: {avgMillisecondsPerCall:F4} 毫秒\n" +
                               $"每秒调用次数: {callsPerSecond:F2}\n" +
                               $"初始缓存状态: {initialStats}\n" +
                               $"最终缓存状态: {finalStats}";

                return result;
            }
            catch (Exception ex)
            {
                return $"测试过程中发生错误: {ex.Message}\n堆栈跟踪: {ex.StackTrace}";
            }
        }

        /// <summary>
        /// 执行多种服务的性能对比测试
        /// </summary>
        /// <param name="iterations">每种服务的测试迭代次数</param>
        /// <returns>测试结果字符串</returns>
        public static string RunComparisonTest(int iterations = 1000)
        {
            // 定义要测试的服务类型列表
            string[] serviceTypes = {
                "RUINORERP.IServices.IEntityCacheManager",
                "RUINORERP.IRepository.Base.IBaseRepository`1[[RUINORERP.Model.Base.BaseEntity, RUINORERP.Model]]",
                "RUINORERP.Common.Logging.ILogger"
            };

            System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder();
            resultBuilder.AppendLine("=== 服务缓存性能对比测试 ===\n");

            foreach (string serviceType in serviceTypes)
            {
                resultBuilder.AppendLine($"--- 测试服务: {serviceType} ---");
                
                // 测试不使用缓存的情况
                resultBuilder.AppendLine("不使用缓存:");
                Startup.ClearServiceCache();
                string resultWithoutCache = RunPerformanceTest(serviceType, iterations, false);
                resultBuilder.AppendLine(resultWithoutCache);
                resultBuilder.AppendLine();

                // 测试使用缓存的情况
                resultBuilder.AppendLine("使用缓存:");
                Startup.ClearServiceCache();
                string resultWithCache = RunPerformanceTest(serviceType, iterations, true);
                resultBuilder.AppendLine(resultWithCache);
                resultBuilder.AppendLine();
                
                // 计算性能提升
                // 这里可以添加更详细的性能对比逻辑
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// 测试缓存命中率和容量限制
        /// </summary>
        /// <param name="serviceCount">要测试的服务数量</param>
        /// <returns>测试结果字符串</returns>
        public static string TestCacheCapacity(int serviceCount = 150)
        {
            try
            {
                System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder();
                resultBuilder.AppendLine("=== 缓存容量测试 ===\n");

                // 清空缓存
                Startup.ClearServiceCache();
                resultBuilder.AppendLine("初始状态: " + Startup.GetServiceCacheStatistics());

                // 获取多个不同类型的服务，测试缓存容量限制
                for (int i = 0; i < serviceCount; i++)
                {
                    // 这里使用一个通用的服务类型进行测试
                    // 在实际应用中，可能会有更多不同的服务类型
                    var method = typeof(Startup).GetMethod("GetFromFac").MakeGenericMethod(typeof(IEntityCacheManager));
                    var result = method.Invoke(null, null);
                    
                    if (i % 10 == 0)
                    {
                        resultBuilder.AppendLine($"获取第 {i} 个服务后: " + Startup.GetServiceCacheStatistics());
                    }
                }

                resultBuilder.AppendLine($"最终状态: " + Startup.GetServiceCacheStatistics());

                return resultBuilder.ToString();
            }
            catch (Exception ex)
            {
                return $"缓存容量测试过程中发生错误: {ex.Message}\n堆栈跟踪: {ex.StackTrace}";
            }
        }
    }
}