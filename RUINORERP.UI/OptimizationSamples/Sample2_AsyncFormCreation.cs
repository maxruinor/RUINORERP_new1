using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Performance;

namespace RUINORERP.UI.OptimizationSamples
{
    /// <summary>
    /// 场景 2 示例：菜单打开异步创建窗体
    /// 演示如何将窗体创建移到后台线程，避免阻塞 UI
    /// </summary>
    public static class Sample2_AsyncFormCreation
    {
        private static ILogger _logger;
        private static PerformanceBenchmark _benchmark;

        /// <summary>
        /// 初始化示例
        /// </summary>
        public static void Initialize(ILogger logger)
        {
            _logger = logger;
            _benchmark = new PerformanceBenchmark(logger);
        }

        /// <summary>
        /// 优化前的窗体创建方式（阻塞 UI）
        /// 在 UI 线程直接创建复杂窗体
        /// </summary>
        public static Form CreateFormBefore(string formTypeName)
        {
            try
            {
                _logger?.LogInformation("【优化前】开始在 UI 线程创建窗体：{FormType}", formTypeName);
                
                var sw = Stopwatch.StartNew();
                
                // ❌ 问题：在 UI 线程直接创建，复杂窗体可能耗时 500-1000ms
                // 导致界面冻结，用户无法进行任何操作
                Form form = null;
                
                switch (formTypeName)
                {
                    case "UCSaleOrder":
                        form = new UCSaleOrder(); // 复杂窗体，初始化控件多
                        break;
                    case "UCPurchaseOrder":
                        form = new UCPurchaseOrder();
                        break;
                    default:
                        form = new Form();
                        break;
                }
                
                sw.Stop();
                _logger?.LogWarning("【优化前】窗体创建完成，耗时：{Ms}ms，类型：{FormType}", 
                    sw.ElapsedMilliseconds, formTypeName);
                
                return form;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "【优化前】窗体创建失败");
                throw;
            }
        }

        /// <summary>
        /// 优化后的窗体创建方式（不阻塞 UI）
        /// 在后台线程异步创建窗体
        /// </summary>
        public static async Task<Form> CreateFormAfterAsync(string formTypeName)
        {
            try
            {
                _logger?.LogInformation("【优化后】开始在后台异步创建窗体：{FormType}", formTypeName);
                
                var sw = Stopwatch.StartNew();
                
                // ✅ 优化：在后台线程创建窗体，不阻塞 UI
                Form form = await Task.Run(() =>
                {
                    // 在后台线程创建窗体实例
                    switch (formTypeName)
                    {
                        case "UCSaleOrder":
                            return (Form)new UCSaleOrder();
                        case "UCPurchaseOrder":
                            return (Form)new UCPurchaseOrder();
                        default:
                            return new Form();
                    }
                });
                
                sw.Stop();
                _logger?.LogInformation("【优化后】窗体创建完成，耗时：{Ms}ms，类型：{FormType}", 
                    sw.ElapsedMilliseconds, formTypeName);
                
                return form;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "【优化后】窗体创建失败");
                throw;
            }
        }

        /// <summary>
        /// 完整的菜单打开异步流程（推荐用法）
        /// </summary>
        public static async Task OpenMenuAsync(string menuName, string formTypeName)
        {
            try
            {
                _logger?.LogInformation("========== 开始打开菜单：{MenuName} ==========", menuName);
                
                // 1. 显示加载提示
                MainForm.Instance.ShowStatusText($"正在打开 {menuName}...");
                
                // 2. 后台异步创建窗体（不阻塞 UI）
                var form = await CreateFormAfterAsync(formTypeName);
                
                // 3. 在 UI 线程显示窗体（先显示空壳）
                form.Show();
                MainForm.Instance.ShowStatusText($"{menuName} 已打开，正在加载数据...");
                
                // 4. 后台异步加载数据（不阻塞 UI）
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 模拟数据加载
                        await Task.Delay(1000);
                        
                        MainForm.Instance.BeginInvoke(new Action(() =>
                        {
                            MainForm.Instance.ShowStatusText($"{menuName} 加载完成");
                        }));
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "数据加载失败");
                        
                        MainForm.Instance.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show($"加载数据失败：{ex.Message}", "错误");
                        }));
                    }
                });
                
                _logger?.LogInformation("========== 菜单打开完成：{MenuName} ==========", menuName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "打开菜单失败");
                MainForm.Instance.ShowStatusText($"打开 {menuName} 失败：{ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 性能对比测试
        /// </summary>
        public static async Task PerformanceComparisonAsync()
        {
            try
            {
                _logger?.LogInformation("========== 开始性能对比测试 ==========");
                
                // 测试优化前
                _benchmark.Start("优化前 - UI 线程创建");
                var formBefore = CreateFormBefore("UCSaleOrder");
                var metricBefore = _benchmark.Stop("优化前 - UI 线程创建");
                formBefore.Close();
                formBefore.Dispose();
                
                // 短暂休息
                await Task.Delay(500);
                
                // 测试优化后
                _benchmark.Start("优化后 - 后台异步创建");
                var formAfter = await CreateFormAfterAsync("UCSaleOrder");
                var metricAfter = _benchmark.Stop("优化后 - 后台异步创建");
                formAfter.Close();
                formAfter.Dispose();
                
                // 输出对比结果
                _logger?.LogInformation("========== 性能对比结果 ==========");
                _logger?.LogInformation("优化前耗时：{Ms}ms", metricBefore.ExecutionTimeMs);
                _logger?.LogInformation("优化后耗时：{Ms}ms", metricAfter.ExecutionTimeMs);
                
                if (metricBefore.ExecutionTimeMs > 0)
                {
                    var improvement = (metricBefore.ExecutionTimeMs - metricAfter.ExecutionTimeMs) * 100.0 / metricBefore.ExecutionTimeMs;
                    _logger?.LogInformation("UI 响应提升：{P2}%（优化后 UI 立即响应）", improvement);
                }
                
                _logger?.LogInformation("========== 测试完成 ==========");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "性能对比测试失败");
            }
        }
    }
}
