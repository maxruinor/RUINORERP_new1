using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Performance;

namespace RUINORERP.UI.OptimizationSamples
{
    /// <summary>
    /// 场景 3 示例：反射缓存优化
    /// 演示如何使用缓存避免重复反射，提升性能
    /// </summary>
    public static class Sample3_ReflectionCache
    {
        private static ILogger _logger;
        private static PerformanceBenchmark _benchmark;
        
        /// <summary>
        /// 反射缓存字典
        /// 缓存 PropertyInfo，避免重复反射
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PropertyInfo> _propertyCache 
            = new ConcurrentDictionary<Type, PropertyInfo>();

        /// <summary>
        /// 初始化示例
        /// </summary>
        public static void Initialize(ILogger logger)
        {
            _logger = logger;
            _benchmark = new PerformanceBenchmark(logger);
        }

        /// <summary>
        /// 优化前的反射方式（性能较差）
        /// 每次都重新反射获取 PropertyInfo
        /// </summary>
        public static void SetPropertyBefore(object obj, string propertyName, object value)
        {
            try
            {
                _logger?.LogInformation("【优化前】开始反射设置属性：{Type}.{Property}", 
                    obj?.GetType().Name, propertyName);
                
                var sw = Stopwatch.StartNew();
                
                // ❌ 问题：每次都重新反射，耗时 50-100ms
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(obj, value);
                }
                
                sw.Stop();
                _logger?.LogWarning("【优化前】属性设置完成，耗时：{Ms}ms", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "【优化前】反射设置属性失败");
                throw;
            }
        }

        /// <summary>
        /// 优化后的反射方式（性能优秀）
        /// 使用缓存，只反射一次
        /// </summary>
        public static void SetPropertyAfter(object obj, string propertyName, object value)
        {
            try
            {
                _logger?.LogInformation("【优化后】开始使用缓存设置属性：{Type}.{Property}", 
                    obj?.GetType().Name, propertyName);
                
                var sw = Stopwatch.StartNew();
                
                // ✅ 优化：使用缓存，只反射一次
                var objType = obj.GetType();
                var propertyInfo = _propertyCache.GetOrAdd(objType, type => 
                {
                    _logger?.LogDebug("缓存未命中，执行反射：{Type}", type.Name);
                    return type.GetProperty(propertyName);
                });
                
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(obj, value);
                }
                
                sw.Stop();
                _logger?.LogInformation("【优化后】属性设置完成，耗时：{Ms}ms", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "【优化后】反射设置属性失败");
                throw;
            }
        }

        /// <summary>
        /// 性能对比测试
        /// </summary>
        public static void PerformanceComparison(int iterations = 100)
        {
            try
            {
                _logger?.LogInformation("========== 开始性能对比测试 ==========");
                _logger?.LogInformation("测试次数：{Iterations}次", iterations);
                
                // 准备测试对象
                var testObj = new UCSaleOrder();
                var testMenuInfo = new tb_MenuInfo 
                { 
                    MenuName = "测试菜单",
                    CaptionCN = "测试"
                };
                
                // 测试优化前（重复反射 100 次）
                _benchmark.Start("优化前 - 重复反射");
                for (int i = 0; i < iterations; i++)
                {
                    SetPropertyBefore(testObj, "CurMenuInfo", testMenuInfo);
                }
                var metricBefore = _benchmark.Stop("优化前 - 重复反射", $"次数：{iterations}");
                
                // 清空缓存，确保公平
                _propertyCache.Clear();
                System.Threading.Thread.Sleep(100);
                
                // 测试优化后（使用缓存）
                _benchmark.Start("优化后 - 使用缓存");
                for (int i = 0; i < iterations; i++)
                {
                    SetPropertyAfter(testObj, "CurMenuInfo", testMenuInfo);
                }
                var metricAfter = _benchmark.Stop("优化后 - 使用缓存", $"次数：{iterations}");
                
                // 输出对比结果
                _logger?.LogInformation("========== 性能对比结果 ==========");
                _logger?.LogInformation("优化前总耗时：{Ms}ms，平均：{Avg}ms/次", 
                    metricBefore.ExecutionTimeMs, 
                    metricBefore.ExecutionTimeMs / iterations);
                _logger?.LogInformation("优化后总耗时：{Ms}ms，平均：{Avg}ms/次", 
                    metricAfter.ExecutionTimeMs, 
                    metricAfter.ExecutionTimeMs / iterations);
                
                if (metricBefore.ExecutionTimeMs > 0)
                {
                    var totalImprovement = (metricBefore.ExecutionTimeMs - metricAfter.ExecutionTimeMs) * 100.0 / metricBefore.ExecutionTimeMs;
                    var avgBefore = metricBefore.ExecutionTimeMs / iterations;
                    var avgAfter = metricAfter.ExecutionTimeMs / iterations;
                    var avgImprovement = (avgBefore - avgAfter) * 100.0 / avgBefore;
                    
                    _logger?.LogInformation("总性能提升：{P2}%", totalImprovement);
                    _logger?.LogInformation("平均每次提升：{P2}%（{AvgBefore}ms → {AvgAfter}ms）", 
                        avgImprovement, avgBefore, avgAfter);
                }
                
                _logger?.LogInformation("========== 测试完成 ==========");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "性能对比测试失败");
            }
        }

        /// <summary>
        /// 实际应用场景：设置 CurMenuInfo
        /// 在 MenuHelper 中可以直接使用这个优化方法
        /// </summary>
        public static void SetCurMenuInfoOptimized(object menu, tb_MenuInfo menuInfo)
        {
            try
            {
                // ✅ 使用缓存设置 CurMenuInfo
                SetPropertyAfter(menu, "CurMenuInfo", menuInfo);
                _logger?.LogDebug("设置 CurMenuInfo 成功：{MenuType}", menu?.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "设置 CurMenuInfo 失败");
                // 失败不影响主流程，继续执行
            }
        }
    }
}
