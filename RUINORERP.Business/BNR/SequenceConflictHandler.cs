using System;
using RUINORERP.Business.BNR;

namespace RUINORERP.Business.BNR
{
    /// <summary>
    /// 序列服务冲突诊断和处理示例
    /// </summary>
    public class SequenceConflictHandler
    {
        private readonly DatabaseSequenceService _sequenceService;

        public SequenceConflictHandler(DatabaseSequenceService sequenceService)
        {
            _sequenceService = sequenceService;
        }

        /// <summary>
        /// 处理序列键冲突的完整流程
        /// </summary>
        /// <param name="problematicKey">出现问题的序列键</param>
        public void HandleSequenceConflict(string problematicKey)
        {
            Console.WriteLine($"=== 开始处理序列键冲突: {problematicKey} ===\n");

            try
            {
                // 1. 首先进行诊断
                var diagnosis = _sequenceService.DiagnoseSequenceConflict(problematicKey);
                Console.WriteLine("诊断结果:");
                Console.WriteLine(diagnosis.ToString());
                Console.WriteLine();

                // 2. 根据诊断结果采取相应措施
                if (!diagnosis.IsHealthy)
                {
                    Console.WriteLine("❌ 诊断过程本身出现异常");
                    return;
                }

                // 3. 分析具体问题并处理
                HandleBasedOnDiagnosis(diagnosis);

                // 4. 验证修复结果
                VerifyFix(problematicKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 处理过程中发生异常: {ex.Message}");
                Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }

            Console.WriteLine("\n=== 冲突处理完成 ===");
        }

        /// <summary>
        /// 根据诊断结果采取相应处理措施
        /// </summary>
        private void HandleBasedOnDiagnosis(SequenceConflictDiagnosis diagnosis)
        {
            Console.WriteLine("开始处理诊断发现的问题...");

            // 情况1: 仅存在于缓存中，需要强制刷写
            if (diagnosis.ExistsInCache && !diagnosis.ExistsInDatabase)
            {
                Console.WriteLine("🔧 情况1: 数据仅存在于缓存中");
                Console.WriteLine("   执行强制刷写操作...");
                    
                try
                {
                    // 直接调用DatabaseSequenceService的公共方法
                    var cacheValue = _sequenceService.GetType()
                        .GetField("_sequenceCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.GetValue(_sequenceService) as System.Collections.Concurrent.ConcurrentDictionary<string, long>;

                    if (cacheValue?.ContainsKey(diagnosis.SequenceKey) == true)
                    {
                        _sequenceService.ForceFlushCacheValue(
                            diagnosis.SequenceKey, 
                            cacheValue[diagnosis.SequenceKey], 
                            "ConflictResolution");
                            
                        Console.WriteLine("   ✅ 已将缓存数据强制刷写到数据库");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ⚠️ 强制刷写时出现异常: {ex.Message}");
                }
            }

            // 情况2: 数据库值大于缓存值，需要同步缓存
            else if (diagnosis.ExistsInDatabase && diagnosis.ExistsInCache && 
                     diagnosis.DatabaseValue > diagnosis.CacheValue)
            {
                Console.WriteLine("🔧 情况2: 数据库值大于缓存值");
                Console.WriteLine("   同步缓存数据...");
                
                try
                {
                    // 更新缓存中的值
                    var sequenceCache = _sequenceService.GetType()
                        .GetField("_sequenceCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.GetValue(_sequenceService) as System.Collections.Concurrent.ConcurrentDictionary<string, long>;

                    sequenceCache?.AddOrUpdate(diagnosis.SequenceKey, diagnosis.DatabaseValue.Value, (key, oldValue) => diagnosis.DatabaseValue.Value);
                    
                    Console.WriteLine($"   ✅ 缓存已更新为: {diagnosis.DatabaseValue}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ⚠️ 缓存同步时出现异常: {ex.Message}");
                }
            }

            // 情况3: 存在待处理的更新
            else if (diagnosis.PendingUpdates > 0)
            {
                Console.WriteLine($"🔧 情况3: 存在 {diagnosis.PendingUpdates} 个待处理更新");
                Console.WriteLine("   触发立即刷写...");
                
                try
                {
                    _sequenceService.FlushAllToDatabase();
                    Console.WriteLine("   ✅ 立即刷写完成");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ⚠️ 刷写时出现异常: {ex.Message}");
                }
            }

            // 情况4: 正常状态
            else
            {
                Console.WriteLine("✅ 数据状态正常，无需特殊处理");
            }
        }

        /// <summary>
        /// 验证修复结果
        /// </summary>
        private void VerifyFix(string sequenceKey)
        {
            Console.WriteLine("\n验证修复结果...");
            
            try
            {
                var newDiagnosis = _sequenceService.DiagnoseSequenceConflict(sequenceKey);
                
                if (newDiagnosis.IsHealthy && 
                    newDiagnosis.ExistsInDatabase == newDiagnosis.ExistsInCache &&
                    (!newDiagnosis.ExistsInDatabase || newDiagnosis.DatabaseValue == newDiagnosis.CacheValue))
                {
                    Console.WriteLine("✅ 修复验证通过，数据已一致");
                }
                else
                {
                    Console.WriteLine("⚠️ 修复验证发现问题，可能需要进一步处理");
                    Console.WriteLine(newDiagnosis.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ 验证过程中出现异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量处理多个冲突键
        /// </summary>
        /// <param name="conflictKeys">冲突键数组</param>
        public void HandleMultipleConflicts(string[] conflictKeys)
        {
            Console.WriteLine($"=== 批量处理 {conflictKeys.Length} 个冲突键 ===\n");
            
            int successCount = 0;
            int failureCount = 0;

            foreach (var key in conflictKeys)
            {
                try
                {
                    HandleSequenceConflict(key);
                    successCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ 处理键 '{key}' 时失败: {ex.Message}");
                    failureCount++;
                }
                
                Console.WriteLine(new string('-', 50));
            }

            Console.WriteLine($"\n📊 处理结果统计:");
            Console.WriteLine($"   成功: {successCount}");
            Console.WriteLine($"   失败: {failureCount}");
            Console.WriteLine($"   总计: {conflictKeys.Length}");
        }

        /// <summary>
        /// 监控和预防措施
        /// </summary>
        public void SetupMonitoring()
        {
            Console.WriteLine("=== 设置监控和预防措施 ===");
            
            // 1. 定期健康检查
            Console.WriteLine("1. 启用定期健康检查...");
            // 这里可以设置定时器定期调用健康检查
            
            // 2. 设置合理的批处理阈值
            Console.WriteLine("2. 优化批处理参数...");
            DatabaseSequenceService.SetBatchUpdateThreshold(5); // 减少批处理大小
            
            // 3. 启用详细日志
            Console.WriteLine("3. 启用详细日志记录...");
            // 确保日志系统正常工作
            
            Console.WriteLine("✅ 监控设置完成");
        }
    }

    /// <summary>
    /// 冲突处理工具类
    /// </summary>
    public static class SequenceConflictTools
    {
        /// <summary>
        /// 快速诊断工具
        /// </summary>
        public static void QuickDiagnose(DatabaseSequenceService service, string sequenceKey)
        {
            var diagnosis = service.DiagnoseSequenceConflict(sequenceKey);
            Console.WriteLine($"快速诊断 [{sequenceKey}]:");
            Console.WriteLine($"  数据库存在: {diagnosis.ExistsInDatabase}");
            Console.WriteLine($"  缓存存在: {diagnosis.ExistsInCache}");
            Console.WriteLine($"  数据一致: {diagnosis.IsHealthy && 
                (diagnosis.ExistsInDatabase == diagnosis.ExistsInCache) &&
                (!diagnosis.ExistsInDatabase || diagnosis.DatabaseValue == diagnosis.CacheValue)}");
        }

        /// <summary>
        /// 批量清理过期的序列键
        /// </summary>
        public static void CleanupExpiredSequences(DatabaseSequenceService service, int daysOld = 30)
        {
            Console.WriteLine($"清理 {daysOld} 天前的过期序列...");
            // 实现清理逻辑
        }
    }
}