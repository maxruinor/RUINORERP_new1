using System;
using System.Collections.Generic;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 清理进度事件参数
    /// </summary>
    public class CleanupProgressEventArgs : EventArgs
    {
        /// <summary>
        /// 当前进度
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 总进度
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 进度消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 进度百分比
        /// </summary>
        public int Percentage { get; set; }
    }

    /// <summary>
    /// 清理预览结果
    /// </summary>
    public class CleanupPreviewResult
    {
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
        /// 规则匹配数量（规则名 -> 匹配数）
        /// </summary>
        public Dictionary<string, int> RuleMatchCounts { get; set; }

        /// <summary>
        /// 将要删除的记录预览
        /// </summary>
        public List<Dictionary<string, object>> RecordsToDelete { get; set; }

        /// <summary>
        /// 将要更新的记录预览
        /// </summary>
        public List<Dictionary<string, object>> RecordsToUpdate { get; set; }

        /// <summary>
        /// 将要标记的记录预览
        /// </summary>
        public List<Dictionary<string, object>> RecordsToMark { get; set; }

        /// <summary>
        /// 将要归档的记录预览
        /// </summary>
        public List<Dictionary<string, object>> RecordsToArchive { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CleanupPreviewResult()
        {
            RuleMatchCounts = new Dictionary<string, int>();
            RecordsToDelete = new List<Dictionary<string, object>>();
            RecordsToUpdate = new List<Dictionary<string, object>>();
            RecordsToMark = new List<Dictionary<string, object>>();
            RecordsToArchive = new List<Dictionary<string, object>>();
        }
    }

    /// <summary>
    /// 清理执行结果
    /// </summary>
    public class CleanupExecutionResult
    {
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
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// 匹配的记录数
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
        /// 仅记录的记录数
        /// </summary>
        public int LogOnlyCount { get; set; }

        /// <summary>
        /// 备份表名
        /// </summary>
        public string BackupTableName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 规则执行结果列表
        /// </summary>
        public List<object> RuleResults { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CleanupExecutionResult()
        {
            StartTime = DateTime.Now;
            RuleResults = new List<object>();
        }

        /// <summary>
        /// 完成执行
        /// </summary>
        public void Complete()
        {
            EndTime = DateTime.Now;
            ElapsedMilliseconds = (long)(EndTime - StartTime).TotalMilliseconds;
        }
    }
}
