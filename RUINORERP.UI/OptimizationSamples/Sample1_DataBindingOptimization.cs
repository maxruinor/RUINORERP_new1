using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Performance;

namespace RUINORERP.UI.OptimizationSamples
{
    /// <summary>
    /// 场景 1 示例：销售订单数据绑定优化
    /// 演示如何使用 BindDataBatch 优化 DataGridView 数据绑定
    /// </summary>
    public static class Sample1_DataBindingOptimization
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
            UiPerformanceOptimizer.Initialize(logger);
        }

        /// <summary>
        /// 优化前的数据绑定方式（性能较差）
        /// 逐行添加数据，导致反复重绘
        /// </summary>
        public static void BindDataBefore(DataGridView dataGridView, List<tb_SaleOrderDetail> dataList)
        {
            try
            {
                _logger?.LogInformation("【优化前】开始逐行绑定数据，数据量：{Count}", dataList?.Count ?? 0);
                
                var sw = Stopwatch.StartNew();
                
                // ❌ 问题：逐行添加，每次 Add 都会触发重绘
                dataGridView.Rows.Clear();
                foreach (var item in dataList)
                {
                    dataGridView.Rows.Add(
                        item.ID,
                        item.ProdDetailID,
                        item.SKU,
                        item.Qty,
                        item.UnitPrice,
                        item.Amount
                    );
                    // 每次 Add 都会重绘一次，100 行数据重绘 100 次
                }
                
                sw.Stop();
                _logger?.LogWarning("【优化前】逐行绑定完成，耗时：{Ms}ms，数据量：{Count}", 
                    sw.ElapsedMilliseconds, dataList.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "【优化前】逐行绑定失败");
                throw;
            }
        }

        /// <summary>
        /// 优化后的数据绑定方式（性能优秀）
        /// 使用 BindDataBatch 批量绑定，只重绘一次
        /// </summary>
        public static void BindDataAfter(DataGridView dataGridView, List<tb_SaleOrderDetail> dataList)
        {
            try
            {
                _logger?.LogInformation("【优化后】开始批量绑定数据，数据量：{Count}", dataList?.Count ?? 0);
                
                var sw = Stopwatch.StartNew();
                
                // ✅ 优化：使用批量绑定，暂停重绘
                dataGridView.BindDataBatch(dataList, autoGenerateColumns: true);
                
                sw.Stop();
                _logger?.LogInformation("【优化后】批量绑定完成，耗时：{Ms}ms，数据量：{Count}", 
                    sw.ElapsedMilliseconds, dataList.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "【优化后】批量绑定失败");
                throw;
            }
        }

        /// <summary>
        /// 性能对比测试
        /// </summary>
        public static void PerformanceComparison(DataGridView dataGridView, int dataSize = 100)
        {
            try
            {
                _logger?.LogInformation("========== 开始性能对比测试 ==========");
                _logger?.LogInformation("测试数据量：{Size}行", dataSize);
                
                // 准备测试数据
                var testData = new List<tb_SaleOrderDetail>();
                for (int i = 0; i < dataSize; i++)
                {
                    testData.Add(new tb_SaleOrderDetail
                    {
                        ID = i + 1,
                        ProdDetailID = i + 1,
                        SKU = $"SKU-{i + 1:D6}",
                        Qty = 10 + i,
                        UnitPrice = 100.5m + i,
                        Amount = (100.5m + i) * (10 + i)
                    });
                }
                
                // 测试优化前
                _benchmark.Start("优化前 - 逐行绑定");
                BindDataBefore(dataGridView, testData);
                var metricBefore = _benchmark.Stop("优化前 - 逐行绑定", $"数据量：{dataSize}");
                
                // 清空数据
                dataGridView.Rows.Clear();
                System.Threading.Thread.Sleep(100); // 短暂休息
                
                // 测试优化后
                _benchmark.Start("优化后 - 批量绑定");
                BindDataAfter(dataGridView, testData);
                var metricAfter = _benchmark.Stop("优化后 - 批量绑定", $"数据量：{dataSize}");
                
                // 输出对比结果
                _logger?.LogInformation("========== 性能对比结果 ==========");
                _logger?.LogInformation("优化前耗时：{Ms}ms", metricBefore.ExecutionTimeMs);
                _logger?.LogInformation("优化后耗时：{Ms}ms", metricAfter.ExecutionTimeMs);
                
                if (metricBefore.ExecutionTimeMs > 0)
                {
                    var improvement = (metricBefore.ExecutionTimeMs - metricAfter.ExecutionTimeMs) * 100.0 / metricBefore.ExecutionTimeMs;
                    _logger?.LogInformation("性能提升：{P2}%", improvement);
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
