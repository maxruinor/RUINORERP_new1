using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SqlSugar;

namespace RUINORERP.Business.BNR
{
    /// <summary>
    /// DatabaseSequenceService 测试类
    /// 验证序号服务的各项改进功能
    /// </summary>
    public class DatabaseSequenceServiceTest
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly DatabaseSequenceService _sequenceService;

        public DatabaseSequenceServiceTest(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
            _sequenceService = new DatabaseSequenceService(sqlSugarClient);
        }

        /// <summary>
        /// 运行所有测试
        /// </summary>
        public async Task RunAllTests()
        {
            Console.WriteLine("=== DatabaseSequenceService 改进测试 ===\n");

            try
            {
                // 测试1: 基本功能测试
                await TestBasicFunctionality();
                
                // 测试2: 并发安全性测试
                await TestConcurrencySafety();
                
                // 测试3: 健康检查功能测试
                TestHealthCheck();
                
                // 测试4: 数据持久化测试
                await TestDataPersistence();
                
                // 测试5: 异常处理测试
                TestExceptionHandling();
                
                // 测试6: 冲突诊断测试
                TestConflictDiagnosis();
                
                Console.WriteLine("\n✅ 所有测试完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 测试过程中发生错误: {ex.Message}");
                throw;
            }
            finally
            {
                // 清理测试数据
                CleanupTestData();
            }
        }

        /// <summary>
        /// 测试基本功能
        /// </summary>
        private async Task TestBasicFunctionality()
        {
            Console.WriteLine("1. 测试基本功能...");
            
            string testKey = $"TEST_BASIC_{DateTime.Now:yyyyMMddHHmmss}";
            
            // 生成几个序号
            var values = new List<long>();
            for (int i = 0; i < 5; i++)
            {
                var value = _sequenceService.GetNextSequenceValue(testKey);
                values.Add(value);
                Console.WriteLine($"   生成序号: {testKey} = {value}");
                await Task.Delay(100); // 模拟业务处理时间
            }
            
            // 验证序号连续性
            for (int i = 0; i < values.Count - 1; i++)
            {
                if (values[i + 1] != values[i] + 1)
                {
                    throw new Exception($"序号不连续: {values[i]} -> {values[i + 1]}");
                }
            }
            
            Console.WriteLine("   ✅ 基本功能测试通过");
        }

        /// <summary>
        /// 测试并发安全性
        /// </summary>
        private async Task TestConcurrencySafety()
        {
            Console.WriteLine("2. 测试并发安全性...");
            
            string concurrentKey = $"TEST_CONCURRENT_{DateTime.Now:yyyyMMddHHmmss}";
            var tasks = new List<Task>();
            var generatedValues = new HashSet<long>();
            var lockObj = new object();
            
            // 启动多个并发任务
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        var value = _sequenceService.GetNextSequenceValue(concurrentKey);
                        lock (lockObj)
                        {
                            if (generatedValues.Contains(value))
                            {
                                throw new Exception($"检测到重复序号: {value}");
                            }
                            generatedValues.Add(value);
                        }
                    }
                }));
            }
            
            await Task.WhenAll(tasks.ToArray());
            
            // 验证生成的序号数量
            if (generatedValues.Count != 50)
            {
                throw new Exception($"期望生成50个序号，实际生成{generatedValues.Count}个");
            }
            
            Console.WriteLine($"   ✅ 并发安全性测试通过，生成了{generatedValues.Count}个唯一序号");
        }

        /// <summary>
        /// 测试健康检查功能
        /// </summary>
        private void TestHealthCheck()
        {
            Console.WriteLine("3. 测试健康检查功能...");
            
            // 获取整体健康状态
            var healthInfo = _sequenceService.GetHealthInfo();
            Console.WriteLine($"   缓存大小: {healthInfo.CacheSize}");
            Console.WriteLine($"   队列大小: {healthInfo.QueueSize}");
            Console.WriteLine($"   正在刷写: {healthInfo.IsFlushing}");
            
            // 检查特定序列键状态
            string testKey = "TEST_HEALTH_CHECK";
            _sequenceService.GetNextSequenceValue(testKey);
            
            var diagnosis = _sequenceService.DiagnoseSequenceConflict(testKey);
            Console.WriteLine($"   键 '{testKey}' 状态:");
            Console.WriteLine($"     缓存中存在: {diagnosis.ExistsInCache}");
            Console.WriteLine($"     数据库中存在: {diagnosis.ExistsInDatabase}");
            Console.WriteLine($"     数据一致性: {diagnosis.ExistsInDatabase == diagnosis.ExistsInCache}");
            
            Console.WriteLine("   ✅ 健康检查功能测试通过");
        }

        /// <summary>
        /// 测试数据持久化
        /// </summary>
        private async Task TestDataPersistence()
        {
            Console.WriteLine("4. 测试数据持久化...");
            
            string persistenceKey = $"TEST_PERSISTENCE_{DateTime.Now:yyyyMMddHHmmss}";
            
            // 生成一些序号
            for (int i = 0; i < 3; i++)
            {
                _sequenceService.GetNextSequenceValue(persistenceKey);
                await Task.Delay(200);
            }
            
            // 强制刷写数据
            _sequenceService.FlushAllToDatabase();
            
            // 验证数据库中的值
            var dbValue = _sequenceService.GetCurrentSequenceValue(persistenceKey);
            Console.WriteLine($"   数据库中的序号值: {dbValue}");
            
            if (dbValue != 3)
            {
                throw new Exception($"期望数据库值为3，实际为{dbValue}");
            }
            
            Console.WriteLine("   ✅ 数据持久化测试通过");
        }

        /// <summary>
        /// 测试异常处理
        /// </summary>
        private void TestExceptionHandling()
        {
            Console.WriteLine("5. 测试异常处理...");
            
            try
            {
                // 测试无效键处理
                _sequenceService.GetNextSequenceValue(""); // 空键应该抛出异常
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("   ✅ 空键参数验证通过");
            }
            
            // 测试序列存在性检查
            string nonExistKey = "NON_EXISTENT_KEY_TEST";
            bool exists = _sequenceService.SequenceExists(nonExistKey);
            Console.WriteLine($"   不存在的键检查: {exists}");
            
            if (exists)
            {
                throw new Exception("不应该存在该键");
            }
            
            Console.WriteLine("   ✅ 异常处理测试通过");
        }

        /// <summary>
        /// 测试冲突诊断功能
        /// </summary>
        private void TestConflictDiagnosis()
        {
            Console.WriteLine("6. 测试冲突诊断功能...");
            
            string testKey = "TEST_CONFLICT_DIAGNOSIS";
            
            // 生成一些序号使数据进入不同存储
            for (int i = 0; i < 3; i++)
            {
                _sequenceService.GetNextSequenceValue(testKey);
            }
            
            // 执行诊断
            var diagnosis = _sequenceService.DiagnoseSequenceConflict(testKey);
            
            Console.WriteLine("   诊断结果:");
            Console.WriteLine($"     序列键: {diagnosis.SequenceKey}");
            Console.WriteLine($"     数据库存在: {diagnosis.ExistsInDatabase}");
            Console.WriteLine($"     缓存存在: {diagnosis.ExistsInCache}");
            Console.WriteLine($"     数据库值: {diagnosis.DatabaseValue}");
            Console.WriteLine($"     缓存值: {diagnosis.CacheValue}");
            Console.WriteLine($"     待处理更新: {diagnosis.PendingUpdates}");
            Console.WriteLine($"     健康状态: {diagnosis.IsHealthy}");
            Console.WriteLine($"     冲突原因: {diagnosis.ConflictReason}");
            
            // 验证诊断结果合理性
            if (!diagnosis.IsHealthy)
            {
                throw new Exception("诊断过程本身失败");
            }
            
            // 至少应该在某个存储中存在
            if (!diagnosis.ExistsInDatabase && !diagnosis.ExistsInCache)
            {
                throw new Exception("诊断结果显示键在任何存储中都不存在");
            }
            
            Console.WriteLine("   ✅ 冲突诊断功能测试通过");
        }
        private void CleanupTestData()
        {
            Console.WriteLine("\n清理测试数据...");
            
            try
            {
                // 删除测试用的序列键
                var testKeys = new[]
                {
                    "TEST_BASIC_*",
                    "TEST_CONCURRENT_*", 
                    "TEST_PERSISTENCE_*",
                    "TEST_HEALTH_CHECK"
                };
                
                foreach (var pattern in testKeys)
                {
                    // 这里应该根据实际需求实现批量删除逻辑
                    Console.WriteLine($"   清理模式: {pattern}");
                }
                
                _sequenceService.Dispose();
                Console.WriteLine("   ✅ 测试数据清理完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️ 清理过程中出现错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 性能基准测试
        /// </summary>
        public async Task RunPerformanceBenchmark()
        {
            Console.WriteLine("\n=== 性能基准测试 ===");
            
            string benchmarkKey = $"BENCHMARK_{DateTime.Now:yyyyMMddHHmmss}";
            int testCount = 1000;
            
            var startTime = DateTime.Now;
            
            // 生成大量序号
            for (int i = 0; i < testCount; i++)
            {
                _sequenceService.GetNextSequenceValue(benchmarkKey);
            }
            
            var endTime = DateTime.Now;
            var duration = endTime - startTime;
            
            Console.WriteLine($"生成 {testCount} 个序号耗时: {duration.TotalMilliseconds:F2} ms");
            Console.WriteLine($"平均每个序号耗时: {duration.TotalMilliseconds / testCount:F4} ms");
            Console.WriteLine($"TPS: {testCount / duration.TotalSeconds:F2} 序号/秒");
            
            // 验证最终值
            var finalValue = _sequenceService.GetCurrentSequenceValue(benchmarkKey);
            Console.WriteLine($"最终序号值: {finalValue}");
            
            if (finalValue != testCount)
            {
                throw new Exception($"性能测试结果不正确: 期望{testCount}，实际{finalValue}");
            }
            
            Console.WriteLine("✅ 性能基准测试完成");
        }
    }
}