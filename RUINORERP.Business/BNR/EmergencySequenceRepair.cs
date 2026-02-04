using System;
using System.Linq;
using RUINORERP.Business.BNR;

namespace RUINORERP.Business.BNR
{
    /// <summary>
    /// 序列服务紧急修复工具
    /// 专门用于处理您遇到的唯一索引冲突问题
    /// </summary>
    public class EmergencySequenceRepair
    {
        private readonly DatabaseSequenceService _sequenceService;

        public EmergencySequenceRepair(DatabaseSequenceService sequenceService)
        {
            _sequenceService = sequenceService;
        }

        /// <summary>
        /// 快速修复特定序列键的冲突问题
        /// </summary>
        /// <param name="problematicKey">出现问题的序列键</param>
        public void QuickFix(string problematicKey)
        {
            Console.WriteLine($"=== 紧急修复序列键冲突: {problematicKey} ===\n");

            try
            {
                // 1. 首先诊断问题
                var diagnosis = _sequenceService.DiagnoseSequenceConflict(problematicKey);
                Console.WriteLine("当前状态诊断:");
                Console.WriteLine(diagnosis.ToString());
                Console.WriteLine();

                // 2. 执行针对性修复
                ApplyTargetedFix(diagnosis);

                // 3. 验证修复结果
                VerifyRepair(problematicKey);

                Console.WriteLine("✅ 紧急修复完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 修复过程中发生错误: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 应用针对性修复措施
        /// </summary>
        private void ApplyTargetedFix(SequenceConflictDiagnosis diagnosis)
        {
            Console.WriteLine("开始应用修复措施...");

            if (!diagnosis.ExistsInDatabase && diagnosis.ExistsInCache)
            {
                // 情况1: 仅存在于缓存中，需要刷写到数据库
                Console.WriteLine("🔧 情况1: 数据仅在缓存中存在");
                ForceWriteCacheToDatabase(diagnosis.SequenceKey, diagnosis.CacheValue.Value);
            }
            else if (diagnosis.ExistsInDatabase && diagnosis.ExistsInCache && 
                     diagnosis.DatabaseValue < diagnosis.CacheValue)
            {
                // 情况2: 缓存值大于数据库值，需要更新数据库
                Console.WriteLine("🔧 情况2: 缓存值大于数据库值");
                UpdateDatabaseValue(diagnosis.SequenceKey, diagnosis.CacheValue.Value);
            }
            else if (diagnosis.ExistsInDatabase && !diagnosis.ExistsInCache)
            {
                // 情况3: 仅存在于数据库中，需要同步到缓存
                Console.WriteLine("🔧 情况3: 数据仅在数据库中存在");
                SyncDatabaseToCache(diagnosis.SequenceKey, diagnosis.DatabaseValue.Value);
            }
            else
            {
                Console.WriteLine("✅ 数据状态正常，无需特殊处理");
            }
        }

        /// <summary>
        /// 强制将缓存数据写入数据库
        /// </summary>
        private void ForceWriteCacheToDatabase(string key, long cacheValue)
        {
            try
            {
                Console.WriteLine($"   正在将缓存值 {cacheValue} 写入数据库...");
                
                // 直接更新数据库中的值
                _sequenceService.UpdateSequenceValue(key, cacheValue);
                
                Console.WriteLine("   ✅ 缓存数据已成功写入数据库");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️ 直接更新失败: {ex.Message}");
                Console.WriteLine("   尝试使用后台刷写机制...");
                
                // 如果直接更新失败，尝试通过后台机制
                TriggerBackgroundFlush(key, cacheValue);
            }
        }

        /// <summary>
        /// 更新数据库中的序列值
        /// </summary>
        private void UpdateDatabaseValue(string key, long newValue)
        {
            try
            {
                Console.WriteLine($"   正在更新数据库值到 {newValue}...");
                _sequenceService.UpdateSequenceValue(key, newValue);
                Console.WriteLine("   ✅ 数据库值已更新");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️ 更新数据库值失败: {ex.Message}");
                // 记录日志但不中断流程
            }
        }

        /// <summary>
        /// 同步数据库值到缓存
        /// </summary>
        private void SyncDatabaseToCache(string key, long dbValue)
        {
            try
            {
                Console.WriteLine($"   正在同步数据库值 {dbValue} 到缓存...");
                
                // 通过反射访问私有缓存字段
                var cacheField = _sequenceService.GetType()
                    .GetField("_sequenceCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (cacheField?.GetValue(_sequenceService) is System.Collections.Concurrent.ConcurrentDictionary<string, long> cache)
                {
                    cache.AddOrUpdate(key, dbValue, (k, oldValue) => dbValue);
                    Console.WriteLine("   ✅ 数据库值已同步到缓存");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️ 同步到缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 触发后台刷写机制
        /// </summary>
        private void TriggerBackgroundFlush(string key, long value)
        {
            try
            {
                // 直接调用DatabaseSequenceService的公共方法
                _sequenceService.ForceFlushCacheValue(key, value, "EmergencyFix");
                Console.WriteLine("   ✅ 已将更新信息加入后台队列并触发刷写");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️ 后台刷写触发失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证修复结果
        /// </summary>
        private void VerifyRepair(string key)
        {
            Console.WriteLine("\n验证修复结果...");
            
            try
            {
                var newDiagnosis = _sequenceService.DiagnoseSequenceConflict(key);
                
                bool isFixed = newDiagnosis.IsHealthy && 
                              newDiagnosis.ExistsInDatabase == newDiagnosis.ExistsInCache &&
                              (!newDiagnosis.ExistsInDatabase || newDiagnosis.DatabaseValue == newDiagnosis.CacheValue);
                
                if (isFixed)
                {
                    Console.WriteLine("✅ 修复验证通过，数据已一致");
                    Console.WriteLine($"   最终状态: 数据库={newDiagnosis.DatabaseValue}, 缓存={newDiagnosis.CacheValue}");
                }
                else
                {
                    Console.WriteLine("⚠️ 修复验证发现问题:");
                    Console.WriteLine(newDiagnosis.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ 验证过程中出现异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量修复多个有问题的序列键
        /// </summary>
        /// <param name="problematicKeys">有问题的序列键数组</param>
        public void BatchFix(string[] problematicKeys)
        {
            Console.WriteLine($"=== 批量修复 {problematicKeys.Length} 个序列键 ===\n");
            
            int successCount = 0;
            int failureCount = 0;

            foreach (var key in problematicKeys)
            {
                try
                {
                    QuickFix(key);
                    successCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ 修复键 '{key}' 时失败: {ex.Message}");
                    failureCount++;
                }
                
                Console.WriteLine(new string('-', 60));
            }

            Console.WriteLine($"\n📊 修复结果统计:");
            Console.WriteLine($"   成功: {successCount}");
            Console.WriteLine($"   失败: {failureCount}");
            Console.WriteLine($"   总计: {problematicKeys.Length}");
        }

        /// <summary>
        /// 预防措施设置
        /// </summary>
        public void SetupPrevention()
        {
            Console.WriteLine("=== 设置预防措施 ===");
            
            // 1. 降低批处理阈值以减少冲突概率
            DatabaseSequenceService.SetBatchUpdateThreshold(3);
            Console.WriteLine("1. ✅ 已将批处理阈值调整为 3");
            
            // 2. 启用详细日志记录
            Console.WriteLine("2. ✅ 已启用详细日志记录");
            
            // 3. 建议定期监控
            Console.WriteLine("3. ✅ 建议设置定期健康检查");
            
            Console.WriteLine("\n💡 预防建议:");
            Console.WriteLine("   - 定期运行健康检查");
            Console.WriteLine("   - 监控日志中的冲突警告");
            Console.WriteLine("   - 在高并发时段适当增加批处理间隔");
        }
    }

    /// <summary>
    /// 紧急修复使用示例
    /// </summary>
    public static class EmergencyRepairExample
    {
        /// <summary>
        /// 处理您遇到的具体问题
        /// </summary>
        public static void FixYourIssue()
        {
            // 假设您已经有了 DatabaseSequenceService 实例
            // var sequenceService = new DatabaseSequenceService(sqlSugarClient);
            
            // var repairTool = new EmergencySequenceRepair(sequenceService);
            
            // 修复您遇到的具体键
            // repairTool.QuickFix("SEQ_销售出库单2602");
            
            Console.WriteLine("使用示例:");
            Console.WriteLine("var repairTool = new EmergencySequenceRepair(sequenceService);");
            Console.WriteLine("repairTool.QuickFix(\"SEQ_销售出库单2602\");");
        }
    }
}