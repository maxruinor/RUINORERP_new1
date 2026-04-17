using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Business.AIServices.DataImport
{
    /// <summary>
    /// 智能映射服务接口
    /// 用于数据导入时的字段自动映射
    /// </summary>
    public interface IIntelligentMappingService
    {
        /// <summary>
        /// 分析Excel列与目标实体字段的映射关系
        /// </summary>
        /// <param name="excelColumns">Excel文件中的列名列表</param>
        /// <param name="targetFields">目标实体的字段名列表</param>
        /// <returns>列名到字段名的映射建议</returns>
        Task<Dictionary<string, string>> AnalyzeColumnMappingAsync(List<string> excelColumns, List<string> targetFields);

        /// <summary>
        /// 分析Excel列与目标实体字段的映射关系（带示例数据）
        /// </summary>
        /// <param name="excelColumns">Excel文件中的列名列表</param>
        /// <param name="targetFields">目标实体的字段名列表</param>
        /// <param name="sampleData">示例数据（前N行）</param>
        /// <returns>列名到字段名的映射建议</returns>
        Task<Dictionary<string, string>> AnalyzeColumnMappingAsync(List<string> excelColumns, List<string> targetFields, List<Dictionary<string, object>> sampleData);

        /// <summary>
        /// 验证数据质量
        /// </summary>
        /// <param name="data">要验证的数据</param>
        /// <param name="entityType">目标实体类型</param>
        /// <returns>数据质量报告</returns>
        Task<DataQualityReport> ValidateDataQualityAsync(List<Dictionary<string, object>> data, string entityType);

        /// <summary>
        /// 生成数据转换建议
        /// </summary>
        /// <param name="sourceData">源数据样本</param>
        /// <param name="targetEntity">目标实体类型</param>
        /// <returns>转换建议列表</returns>
        Task<List<DataTransformationSuggestion>> GenerateTransformationSuggestionsAsync(List<Dictionary<string, object>> sourceData, string targetEntity);
    }

    /// <summary>
    /// 数据质量报告
    /// </summary>
    public class DataQualityReport
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 有效记录数
        /// </summary>
        public int ValidRecords { get; set; }

        /// <summary>
        /// 问题记录数
        /// </summary>
        public int ProblematicRecords { get; set; }

        /// <summary>
        /// 数据质量问题列表
        /// </summary>
        public List<DataQualityIssue> Issues { get; set; } = new List<DataQualityIssue>();

        /// <summary>
        /// 质量评分（0-100）
        /// </summary>
        public int QualityScore { get; set; }
    }

    /// <summary>
    /// 数据质量问题
    /// </summary>
    public class DataQualityIssue
    {
        /// <summary>
        /// 问题类型
        /// </summary>
        public string IssueType { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 受影响的列
        /// </summary>
        public string AffectedColumn { get; set; }

        /// <summary>
        /// 受影响的行数
        /// </summary>
        public int AffectedRowCount { get; set; }

        /// <summary>
        /// 修复建议
        /// </summary>
        public string Suggestion { get; set; }
    }

    /// <summary>
    /// 数据转换建议
    /// </summary>
    public class DataTransformationSuggestion
    {
        public string SourceColumn { get; set; }
        public string TargetField { get; set; }
        public string TransformationType { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
    }

    /// <summary>
    /// AI 智能映射结果
    /// </summary>
    public class IntelligentMappingResult
    {
        /// <summary>
        /// 映射字典: Key=Excel列名, Value=映射详情
        /// </summary>
        public Dictionary<string, MappingItem> Mappings { get; set; } = new Dictionary<string, MappingItem>();

        /// <summary>
        /// AI 建议的逻辑主键（数据库物理字段名）
        /// </summary>
        public string SuggestedLogicalKey { get; set; }
    }

    /// <summary>
    /// 单个字段的映射详情
    /// </summary>
    public class MappingItem
    {
        /// <summary>
        /// 目标数据库字段名
        /// </summary>
        public string TargetField { get; set; }

        /// <summary>
        /// 置信度 (0-100)
        /// </summary>
        public int Confidence { get; set; }
    }
}