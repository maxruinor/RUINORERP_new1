using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Business.BNR
{
    /// <summary>
    /// 序列键一致性诊断工具
    /// 用于检测和修复因服务器重启导致的序列键不一致问题
    /// </summary>
    public class SequenceKeyConsistencyChecker
    {
        private readonly DatabaseSequenceService _sequenceService;

        public SequenceKeyConsistencyChecker(DatabaseSequenceService sequenceService)
        {
            _sequenceService = sequenceService;
        }

        /// <summary>
        /// 诊断所有业务类型的序列键一致性
        /// </summary>
        /// <returns>诊断报告</returns>
        public string DiagnoseAllSequences()
        {
            var report = new StringBuilder();
            report.AppendLine("=== 序列键一致性诊断报告 ===");
            report.AppendLine($"诊断时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();

            try
            {
                // 获取所有序列记录
                var allSequences = _sequenceService.GetAllSequences();
                
                report.AppendLine($"总序列记录数: {allSequences.Count}");
                report.AppendLine();

                // 按业务类型分组
                var groupedByBusinessType = allSequences
                    .Where(s => !string.IsNullOrEmpty(s.BusinessType))
                    .GroupBy(s => s.BusinessType)
                    .OrderBy(g => g.Key);

                foreach (var group in groupedByBusinessType)
                {
                    report.AppendLine($"业务类型: {group.Key}");
                    report.AppendLine($"  序列键数量: {group.Count()}");
                    
                    if (group.Count() > 1)
                    {
                        report.AppendLine($"  ⚠️ 警告: 发现多个序列键,可能存在不一致!");
                        foreach (var seq in group.OrderBy(s => s.SequenceKey))
                        {
                            report.AppendLine($"    - 键: {seq.SequenceKey}");
                            report.AppendLine($"      当前值: {seq.CurrentValue}");
                            report.AppendLine($"      重置类型: {seq.ResetType ?? "None"}");
                            report.AppendLine($"      最后更新: {seq.LastUpdated:yyyy-MM-dd HH:mm:ss}");
                        }
                    }
                    else
                    {
                        var seq = group.First();
                        report.AppendLine($"  ✅ 正常: 单一序列键");
                        report.AppendLine($"    键: {seq.SequenceKey}");
                        report.AppendLine($"    当前值: {seq.CurrentValue}");
                        report.AppendLine($"    重置类型: {seq.ResetType ?? "None"}");
                    }
                    
                    report.AppendLine();
                }

                // 检查没有业务类型的序列
                var noBusinessType = allSequences
                    .Where(s => string.IsNullOrEmpty(s.BusinessType))
                    .ToList();
                
                if (noBusinessType.Count > 0)
                {
                    report.AppendLine($"⚠️ 警告: 发现 {noBusinessType.Count} 条没有业务类型的序列记录:");
                    foreach (var seq in noBusinessType.Take(10)) // 只显示前10条
                    {
                        report.AppendLine($"  - {seq.SequenceKey}: CurrentValue={seq.CurrentValue}");
                    }
                    if (noBusinessType.Count > 10)
                    {
                        report.AppendLine($"  ... 还有 {noBusinessType.Count - 10} 条未显示");
                    }
                    report.AppendLine();
                }

                // 检查可能的重复模式
                report.AppendLine("=== 潜在问题分析 ===");
                CheckPotentialIssues(allSequences, report);

                return report.ToString();
            }
            catch (Exception ex)
            {
                return $"诊断失败: {ex.Message}\n{ex.StackTrace}";
            }
        }

        /// <summary>
        /// 检查特定业务类型的序列键
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <returns>诊断结果</returns>
        public string DiagnoseBusinessType(string businessType)
        {
            var report = new StringBuilder();
            report.AppendLine($"=== 业务类型 '{businessType}' 序列键诊断 ===");
            report.AppendLine($"诊断时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();

            try
            {
                var sequences = _sequenceService.GetSequencesByBusinessType(businessType);
                
                report.AppendLine($"找到 {sequences.Count} 条序列记录:");
                report.AppendLine();

                foreach (var seq in sequences.OrderBy(s => s.SequenceKey))
                {
                    report.AppendLine($"序列键: {seq.SequenceKey}");
                    report.AppendLine($"  当前值: {seq.CurrentValue}");
                    report.AppendLine($"  重置类型: {seq.ResetType ?? "None"}");
                    report.AppendLine($"  格式掩码: {seq.FormatMask ?? "N/A"}");
                    report.AppendLine($"  描述: {seq.Description ?? "N/A"}");
                    report.AppendLine($"  创建时间: {seq.CreatedAt:yyyy-MM-dd HH:mm:ss}");
                    report.AppendLine($"  最后更新: {seq.LastUpdated:yyyy-MM-dd HH:mm:ss}");
                    report.AppendLine();
                }

                if (sequences.Count > 1)
                {
                    report.AppendLine("⚠️ 警告: 发现多个序列键,建议合并或清理!");
                    report.AppendLine();
                    report.AppendLine("建议操作:");
                    report.AppendLine("1. 确认哪个序列键是当前正在使用的");
                    report.AppendLine("2. 将其他序列的CurrentValue合并到主序列");
                    report.AppendLine("3. 删除多余的序列记录");
                }
                else if (sequences.Count == 1)
                {
                    report.AppendLine("✅ 正常: 只有一个序列键");
                }
                else
                {
                    report.AppendLine("⚠️ 警告: 没有找到任何序列记录,可能尚未生成过编号");
                }

                return report.ToString();
            }
            catch (Exception ex)
            {
                return $"诊断失败: {ex.Message}\n{ex.StackTrace}";
            }
        }

        /// <summary>
        /// 清理重复的序列键(谨慎使用!)
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="keepSequenceKey">要保留的序列键</param>
        /// <returns>操作结果</returns>
        public string CleanupDuplicateSequences(string businessType, string keepSequenceKey)
        {
            var report = new StringBuilder();
            report.AppendLine($"=== 清理重复序列键 ===");
            report.AppendLine($"业务类型: {businessType}");
            report.AppendLine($"保留键: {keepSequenceKey}");
            report.AppendLine($"操作时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();

            try
            {
                var sequences = _sequenceService.GetSequencesByBusinessType(businessType);
                var toDelete = sequences.Where(s => s.SequenceKey != keepSequenceKey).ToList();

                if (toDelete.Count == 0)
                {
                    report.AppendLine("✅ 没有需要清理的重复序列");
                    return report.ToString();
                }

                report.AppendLine($"发现 {toDelete.Count} 条需要清理的序列:");
                foreach (var seq in toDelete)
                {
                    report.AppendLine($"  - {seq.SequenceKey}: CurrentValue={seq.CurrentValue}");
                }
                report.AppendLine();

                // 在实际实现中,这里应该调用删除方法
                // 但由于DatabaseSequenceService目前没有批量删除方法,这里只提供诊断信息
                report.AppendLine("⚠️ 注意: 此功能需要手动执行SQL删除操作");
                report.AppendLine("建议使用以下SQL语句:");
                report.AppendLine();
                report.AppendLine($"DELETE FROM SequenceNumbers WHERE BusinessType = '{businessType}' AND SequenceKey != '{keepSequenceKey}';");

                return report.ToString();
            }
            catch (Exception ex)
            {
                return $"清理失败: {ex.Message}\n{ex.StackTrace}";
            }
        }

        /// <summary>
        /// 检查潜在问题
        /// </summary>
        private void CheckPotentialIssues(List<SequenceNumbers> allSequences, StringBuilder report)
        {
            // 1. 检查相同业务类型但有不同重置类型的序列
            var potentialDuplicates = allSequences
                .Where(s => !string.IsNullOrEmpty(s.BusinessType))
                .GroupBy(s => s.BusinessType)
                .Where(g => g.Select(s => s.ResetType ?? "None").Distinct().Count() > 1);

            if (potentialDuplicates.Any())
            {
                report.AppendLine("1. 发现相同业务类型但重置类型不同的序列:");
                foreach (var group in potentialDuplicates)
                {
                    report.AppendLine($"   业务类型: {group.Key}");
                    foreach (var seq in group)
                    {
                        report.AppendLine($"     - {seq.SequenceKey} (重置类型: {seq.ResetType ?? "None"})");
                    }
                }
                report.AppendLine();
            }

            // 2. 检查序列键格式不一致
            var inconsistentKeys = allSequences
                .Where(s => s.SequenceKey.Contains("销售出库单") || 
                           s.SequenceKey.Contains("SOD"))
                .ToList();

            if (inconsistentKeys.Count > 0)
            {
                report.AppendLine("2. 销售出库单相关序列键:");
                foreach (var seq in inconsistentKeys)
                {
                    report.AppendLine($"   - {seq.SequenceKey}: CurrentValue={seq.CurrentValue}, ResetType={seq.ResetType ?? "None"}");
                }
                report.AppendLine();
            }

            // 3. 检查异常大的CurrentValue
            var abnormalValues = allSequences
                .Where(s => s.CurrentValue > 100000)
                .OrderByDescending(s => s.CurrentValue)
                .Take(5)
                .ToList();

            if (abnormalValues.Count > 0)
            {
                report.AppendLine("3. 异常大的序列值(可能表示问题):");
                foreach (var seq in abnormalValues)
                {
                    report.AppendLine($"   - {seq.SequenceKey}: CurrentValue={seq.CurrentValue}");
                }
                report.AppendLine();
            }
        }
    }
}
