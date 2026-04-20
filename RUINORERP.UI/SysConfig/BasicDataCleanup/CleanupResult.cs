using System;
using System.Collections.Generic;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 单条记录清理结果
    /// </summary>
    public class RecordCleanupResult
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string RecordId { get; set; }

        /// <summary>
        /// 记录标识（用于显示）
        /// </summary>
        public string RecordIdentifier { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 执行的操作类型
        /// </summary>
        public CleanupActionType ActionType { get; set; }

        /// <summary>
        /// 应用的规则ID
        /// </summary>
        public string AppliedRuleId { get; set; }

        /// <summary>
        /// 应用的规则名称
        /// </summary>
        public string AppliedRuleName { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 清理前的数据快照（JSON格式）
        /// </summary>
        public string BeforeDataSnapshot { get; set; }

        /// <summary>
        /// 清理后的数据快照（JSON格式）
        /// </summary>
        public string AfterDataSnapshot { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime ProcessTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RecordCleanupResult()
        {
            ProcessTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 规则执行结果
    /// </summary>
    public class RuleExecutionResult
    {
        /// <summary>
        /// 规则ID
        /// </summary>
        public string RuleId { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// 规则类型
        /// </summary>
        public CleanupRuleType RuleType { get; set; }

        /// <summary>
        /// 是否成功执行
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 匹配的记录数
        /// </summary>
        public int MatchedCount { get; set; }

        /// <summary>
        /// 成功处理的记录数
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失败的记录数
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// 跳过的记录数
        /// </summary>
        public int SkippedCount { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 执行耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 单条记录结果列表
        /// </summary>
        public List<RecordCleanupResult> RecordResults { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RuleExecutionResult()
        {
            RecordResults = new List<RecordCleanupResult>();
            StartTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 数据清理执行结果
    /// </summary>
    public class CleanupExecutionResult
    {
        /// <summary>
        /// 执行ID
        /// </summary>
        public string ExecutionId { get; set; }

        /// <summary>
        /// 配置ID
        /// </summary>
        public string ConfigId { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 是否为测试模式
        /// </summary>
        public bool IsTestMode { get; set; }

        /// <summary>
        /// 是否成功完成
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// 匹配清理条件的记录数
        /// </summary>
        public int MatchedRecordCount { get; set; }

        /// <summary>
        /// 成功处理的记录数
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失败的记录数
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// 跳过的记录数
        /// </summary>
        public int SkippedCount { get; set; }

        /// <summary>
        /// 删除的记录数
        /// </summary>
        public int DeletedCount { get; set; }

        /// <summary>
        /// 标记为无效的记录数
        /// </summary>
        public int MarkedInvalidCount { get; set; }

        /// <summary>
        /// 归档的记录数
        /// </summary>
        public int ArchivedCount { get; set; }

        /// <summary>
        /// 更新的记录数
        /// </summary>
        public int UpdatedCount { get; set; }

        /// <summary>
        /// 仅记录未执行的记录数
        /// </summary>
        public int LogOnlyCount { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 执行耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 规则执行结果列表
        /// </summary>
        public List<RuleExecutionResult> RuleResults { get; set; }

        /// <summary>
        /// 备份表名
        /// </summary>
        public string BackupTableName { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public string ExecutedBy { get; set; }

        /// <summary>
        /// 执行机器名
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CleanupExecutionResult()
        {
            ExecutionId = Guid.NewGuid().ToString("N");
            RuleResults = new List<RuleExecutionResult>();
            StartTime = DateTime.Now;
            MachineName = Environment.MachineName;
        }

        /// <summary>
        /// 完成执行
        /// </summary>
        public void Complete()
        {
            EndTime = DateTime.Now;
            ElapsedMilliseconds = (long)(EndTime - StartTime).TotalMilliseconds;
        }

        /// <summary>
        /// 获取执行摘要
        /// </summary>
        /// <returns>执行摘要文本</returns>
        public string GetSummary()
        {
            var summary = $"数据清理执行摘要\n";
            summary += $"==================\n\n";
            summary += $"配置名称: {ConfigName}\n";
            summary += $"执行模式: {(IsTestMode ? "测试模式" : "正式执行")}\n";
            summary += $"执行结果: {(IsSuccess ? "成功" : "失败")}\n\n";
            summary += $"统计信息:\n";
            summary += $"  - 总记录数: {TotalRecordCount}\n";
            summary += $"  - 匹配条件: {MatchedRecordCount}\n";
            summary += $"  - 成功处理: {SuccessCount}\n";
            summary += $"  - 处理失败: {FailedCount}\n";
            summary += $"  - 跳过记录: {SkippedCount}\n\n";
            summary += $"操作明细:\n";
            summary += $"  - 删除: {DeletedCount}\n";
            summary += $"  - 标记无效: {MarkedInvalidCount}\n";
            summary += $"  - 归档: {ArchivedCount}\n";
            summary += $"  - 更新: {UpdatedCount}\n";
            summary += $"  - 仅记录: {LogOnlyCount}\n\n";
            summary += $"执行耗时: {ElapsedMilliseconds} 毫秒\n";
            summary += $"执行时间: {StartTime:yyyy-MM-dd HH:mm:ss} ~ {EndTime:yyyy-MM-dd HH:mm:ss}\n";

            if (!string.IsNullOrEmpty(BackupTableName))
            {
                summary += $"备份表名: {BackupTableName}\n";
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                summary += $"\n错误信息: {ErrorMessage}\n";
            }

            return summary;
        }
    }

    /// <summary>
    /// 数据预览结果
    /// </summary>
    public class CleanupPreviewResult
    {
        /// <summary>
        /// 预览ID
        /// </summary>
        public string PreviewId { get; set; }

        /// <summary>
        /// 配置ID
        /// </summary>
        public string ConfigId { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// 各规则匹配的记录数
        /// </summary>
        public Dictionary<string, int> RuleMatchCounts { get; set; }

        /// <summary>
        /// 预览数据（前N条）
        /// </summary>
        public List<Dictionary<string, object>> PreviewData { get; set; }

        /// <summary>
        /// 将被删除的记录预览
        /// </summary>
        public List<Dictionary<string, object>> RecordsToDelete { get; set; }

        /// <summary>
        /// 将被更新的记录预览
        /// </summary>
        public List<Dictionary<string, object>> RecordsToUpdate { get; set; }

        /// <summary>
        /// 将被标记的记录预览
        /// </summary>
        public List<Dictionary<string, object>> RecordsToMark { get; set; }

        /// <summary>
        /// 将被归档的记录预览
        /// </summary>
        public List<Dictionary<string, object>> RecordsToArchive { get; set; }

        /// <summary>
        /// 预览生成时间
        /// </summary>
        public DateTime PreviewTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CleanupPreviewResult()
        {
            PreviewId = Guid.NewGuid().ToString("N");
            RuleMatchCounts = new Dictionary<string, int>();
            PreviewData = new List<Dictionary<string, object>>();
            RecordsToDelete = new List<Dictionary<string, object>>();
            RecordsToUpdate = new List<Dictionary<string, object>>();
            RecordsToMark = new List<Dictionary<string, object>>();
            RecordsToArchive = new List<Dictionary<string, object>>();
            PreviewTime = DateTime.Now;
        }
    }
}
