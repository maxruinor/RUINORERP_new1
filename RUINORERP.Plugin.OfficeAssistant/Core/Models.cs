using System;
using System.Collections.Generic;

namespace RUINORERP.Plugin.OfficeAssistant.Core
{
    /// <summary>
    /// 对比配置
    /// </summary>
    public class ComparisonConfig
    {
        /// <summary>
        /// 旧文件工作表名称
        /// </summary>
        public string OldWorksheetName { get; set; }
        
        /// <summary>
        /// 新文件工作表名称
        /// </summary>
        public string NewWorksheetName { get; set; }
        
        /// <summary>
        /// 旧文件键列索引
        /// </summary>
        public List<int> OldKeyColumns { get; set; } = new List<int>();
        
        /// <summary>
        /// 新文件键列索引
        /// </summary>
        public List<int> NewKeyColumns { get; set; } = new List<int>();
        
        /// <summary>
        /// 旧文件比较列索引
        /// </summary>
        public List<int> OldCompareColumns { get; set; } = new List<int>();
        
        /// <summary>
        /// 新文件比较列索引
        /// </summary>
        public List<int> NewCompareColumns { get; set; } = new List<int>();
        
        /// <summary>
        /// 比较列索引（用于兼容性）
        /// </summary>
        public List<int> CompareColumns { get; set; } = new List<int>();
        
        /// <summary>
        /// 是否区分大小写
        /// </summary>
        public bool CaseSensitive { get; set; }
        
        /// <summary>
        /// 是否忽略空格
        /// </summary>
        public bool IgnoreSpaces { get; set; }
        
        /// <summary>
        /// 对比模式
        /// </summary>
        public ComparisonMode Mode { get; set; }
    }
    
    /// <summary>
    /// 对比模式
    /// </summary>
    public enum ComparisonMode
    {
        /// <summary>
        /// 存在性检查
        /// </summary>
        ExistenceCheck,
        
        /// <summary>
        /// 数据差异
        /// </summary>
        DataDifference,
        
        /// <summary>
        /// 自定义列对比
        /// </summary>
        CustomColumns
    }
    
    /// <summary>
    /// 对比结果
    /// </summary>
    public class ComparisonResult
    {
        /// <summary>
        /// 新增记录
        /// </summary>
        public List<DiffRecord> AddedRecords { get; set; } = new List<DiffRecord>();
        
        /// <summary>
        /// 删除记录
        /// </summary>
        public List<DiffRecord> DeletedRecords { get; set; } = new List<DiffRecord>();
        
        /// <summary>
        /// 修改记录
        /// </summary>
        public List<ModifiedRecord> ModifiedRecords { get; set; } = new List<ModifiedRecord>();
        
        /// <summary>
        /// 对比摘要
        /// </summary>
        public ComparisonSummary Summary { get; set; }
    }
    
    /// <summary>
    /// 差异记录
    /// </summary>
    public class DiffRecord
    {
        /// <summary>
        /// 键值
        /// </summary>
        public string[] KeyValues { get; set; }
        
        /// <summary>
        /// 数据
        /// </summary>
        public Dictionary<string, object> Data { get; set; }
    }
    
    /// <summary>
    /// 修改记录
    /// </summary>
    public class ModifiedRecord
    {
        /// <summary>
        /// 键值
        /// </summary>
        public string[] KeyValues { get; set; }
        
        /// <summary>
        /// 差异
        /// </summary>
        public Dictionary<string, ValueDifference> Differences { get; set; }
    }
    
    /// <summary>
    /// 值差异
    /// </summary>
    public class ValueDifference
    {
        /// <summary>
        /// 旧值
        /// </summary>
        public string OldValue { get; set; }
        
        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; set; }
    }
    
    /// <summary>
    /// 对比摘要
    /// </summary>
    public class ComparisonSummary
    {
        /// <summary>
        /// 旧记录总数
        /// </summary>
        public int TotalOldRecords { get; set; }
        
        /// <summary>
        /// 新记录总数
        /// </summary>
        public int TotalNewRecords { get; set; }
        
        /// <summary>
        /// 新增记录数
        /// </summary>
        public int AddedCount { get; set; }
        
        /// <summary>
        /// 删除记录数
        /// </summary>
        public int DeletedCount { get; set; }
        
        /// <summary>
        /// 修改记录数
        /// </summary>
        public int ModifiedCount { get; set; }
    }
}