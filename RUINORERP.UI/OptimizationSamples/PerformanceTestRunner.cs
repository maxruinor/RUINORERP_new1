using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.OptimizationSamples
{
    /// <summary>
    /// 性能测试工具类
    /// 用于执行三个优化场景的性能对比测试
    /// </summary>
    public static class PerformanceTestRunner
    {
        private static ILogger _logger;
        private static StringBuilder _testResults;

        /// <summary>
        /// 初始化测试工具
        /// </summary>
        public static void Initialize(ILogger logger)
        {
            _logger = logger;
            _testResults = new StringBuilder();
            
            // 初始化三个示例
            Sample1_DataBindingOptimization.Initialize(logger);
            Sample2_AsyncFormCreation.Initialize(logger);
            Sample3_ReflectionCache.Initialize(logger);
        }

        /// <summary>
        /// 运行所有性能测试
        /// </summary>
        public static void RunAllTests()
        {
            try
            {
                _testResults.Clear();
                _testResults.AppendLine("========================================");
                _testResults.AppendLine("性能优化测试报告");
                _testResults.AppendLine($"测试时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                _testResults.AppendLine("========================================");
                _testResults.AppendLine();
                
                _logger?.LogInformation(_testResults.ToString());
                
                // 测试 1: 数据绑定优化
                TestDataBindingOptimization();
                
                // 测试 2: 异步窗体创建
                // TestAsyncFormCreation(); // 需要在 UI 线程执行
                
                // 测试 3: 反射缓存优化
                TestReflectionCache();
                
                // 输出总结
                var summary = GenerateSummary();
                _logger?.LogInformation(summary);
                
                MessageBox.Show(summary, "性能测试完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "性能测试失败");
                MessageBox.Show($"测试失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 测试 1: 数据绑定优化
        /// </summary>
        private static void TestDataBindingOptimization()
        {
            try
            {
                _logger?.LogInformation("========== 测试 1: 数据绑定优化 ==========");
                _testResults.AppendLine("【测试 1】数据绑定优化");
                
                // 创建测试用的 DataGridView
                using (var dataGridView = new DataGridView())
                {
                    dataGridView.AutoSize = true;
                    
                    // 测试 100 行数据
                    Sample1_DataBindingOptimization.PerformanceComparison(dataGridView, dataSize: 100);
                    _testResults.AppendLine("  ✓ 100 行数据绑定测试完成");
                    
                    // 测试 500 行数据
                    dataGridView.Rows.Clear();
                    Sample1_DataBindingOptimization.PerformanceComparison(dataGridView, dataSize: 500);
                    _testResults.AppendLine("  ✓ 500 行数据绑定测试完成");
                }
                
                _testResults.AppendLine();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试 1 失败");
                _testResults.AppendLine($"  ✗ 测试失败：{ex.Message}");
                _testResults.AppendLine();
            }
        }

        /// <summary>
        /// 测试 2: 异步窗体创建
        /// </summary>
        private static async void TestAsyncFormCreation()
        {
            try
            {
                _logger?.LogInformation("========== 测试 2: 异步窗体创建 ==========");
                _testResults.AppendLine("【测试 2】异步窗体创建");
                
                await Sample2_AsyncFormCreation.PerformanceComparisonAsync();
                _testResults.AppendLine("  ✓ 窗体创建测试完成");
                _testResults.AppendLine();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试 2 失败");
                _testResults.AppendLine($"  ✗ 测试失败：{ex.Message}");
                _testResults.AppendLine();
            }
        }

        /// <summary>
        /// 测试 3: 反射缓存优化
        /// </summary>
        private static void TestReflectionCache()
        {
            try
            {
                _logger?.LogInformation("========== 测试 3: 反射缓存优化 ==========");
                _testResults.AppendLine("【测试 3】反射缓存优化");
                
                Sample3_ReflectionCache.PerformanceComparison(iterations: 100);
                _testResults.AppendLine("  ✓ 100 次反射测试完成");
                
                Sample3_ReflectionCache.PerformanceComparison(iterations: 1000);
                _testResults.AppendLine("  ✓ 1000 次反射测试完成");
                
                _testResults.AppendLine();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "测试 3 失败");
                _testResults.AppendLine($"  ✗ 测试失败：{ex.Message}");
                _testResults.AppendLine();
            }
        }

        /// <summary>
        /// 生成测试总结
        /// </summary>
        private static string GenerateSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine("========================================");
            sb.AppendLine("性能优化测试总结");
            sb.AppendLine("========================================");
            sb.AppendLine();
            sb.AppendLine("测试项目:");
            sb.AppendLine("  1. 数据绑定优化 - 使用 BindDataBatch");
            sb.AppendLine("  2. 异步窗体创建 - 后台创建不阻塞 UI");
            sb.AppendLine("  3. 反射缓存优化 - 避免重复反射");
            sb.AppendLine();
            sb.AppendLine("预期效果:");
            sb.AppendLine("  • 数据绑定性能提升：70-90%");
            sb.AppendLine("  • 窗体创建 UI 响应：立即响应");
            sb.AppendLine("  • 反射性能提升：95-99%");
            sb.AppendLine();
            sb.AppendLine("建议:");
            sb.AppendLine("  ✓ 在 UCSaleOrder 等复杂窗体中使用批量绑定");
            sb.AppendLine("  ✓ 在 MenuHelper 中使用异步创建窗体");
            sb.AppendLine("  ✓ 在所有反射场景中使用缓存");
            sb.AppendLine();
            sb.AppendLine("========================================");
            
            return sb.ToString();
        }
    }
}
