using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RUINORERP.Business.AIServices.Models;
using RUINORERP.Business.Config;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Business.AIServices.DataImport
{
    /// <summary>
    /// 列映射服务实现
    /// 使用AI分析Excel列与目标实体字段的映射关系
    /// </summary>
    public class ColumnMappingService : IIntelligentMappingService
    {
        private readonly ILLMService _llmService;
        private readonly SystemGlobalConfig _config;

        /// <summary>
        /// 初始化列映射服务
        /// </summary>
        public ColumnMappingService()
        {
            _config = ConfigManager.GetSystemGlobalConfig();
            _llmService = LLMServiceFactory.Create();
        }

        /// <summary>
        /// 初始化列映射服务（指定LLM服务）
        /// </summary>
        /// <param name="llmService">LLM服务实例</param>
        public ColumnMappingService(ILLMService llmService)
        {
            _llmService = llmService ?? throw new ArgumentNullException(nameof(llmService));
            _config = ConfigManager.GetSystemGlobalConfig();
        }

        /// <summary>
        /// 分析Excel列与目标实体字段的映射关系（接口实现 - 兼容旧版）
        /// </summary>
        public async Task<Dictionary<string, string>> AnalyzeColumnMappingAsync(List<string> excelColumns, List<string> targetFields)
        {
            // 为了兼容接口，我们这里简单地将字符串列表转换为元数据对象再调用增强版方法
            // 如果没有元数据，AI 效果会打折，但能保证功能可用
            var dbFields = targetFields.Select(f => new RUINORERP.Model.ImportFieldInfo 
            { 
                ColumnName = f, 
                Description = f, // 暂时用物理名作为描述
                IsPrimaryKey = f.Equals("ID", StringComparison.OrdinalIgnoreCase)
            }).ToList();

            var result = await AnalyzeWithMetadataAsync(excelColumns, dbFields);
            return result.Mappings.ToDictionary(k => k.Key, v => v.Value.TargetField);
        }

        /// <summary>
        /// 分析Excel列与目标实体字段的映射关系（带示例数据 - 接口实现）
        /// </summary>
        public async Task<Dictionary<string, string>> AnalyzeColumnMappingAsync(List<string> excelColumns, List<string> targetFields, List<Dictionary<string, object>> sampleData)
        {
            // 目前增强版方法暂未支持样本数据，直接调用基础版
            return await AnalyzeColumnMappingAsync(excelColumns, targetFields);
        }

        /// <summary>
        /// 分析Excel列与目标实体字段的映射关系（增强版 - 支持语义描述）
        /// </summary>
        /// <param name="excelHeaders">Excel表头列表</param>
        /// <param name="dbFields">数据库字段信息列表（含中文名、类型等）</param>
        /// <returns>包含映射关系和逻辑主键建议的结果</returns>
        public async Task<IntelligentMappingResult> AnalyzeWithMetadataAsync(List<string> excelHeaders, List<RUINORERP.Model.ImportFieldInfo> dbFields)
        {
            if (!_config.AIEnableDataImport || !excelHeaders.Any() || !dbFields.Any())
            {
                return new IntelligentMappingResult();
            }

            try
            {
                var prompt = BuildEnhancedMappingPrompt(excelHeaders, dbFields);
                var response = await _llmService.GenerateAsync(prompt, temperature: 0.1f, maxTokens: 2048);

                if (!response.IsSuccess) return new IntelligentMappingResult();

                return ParseEnhancedResponse(response.Response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AI 增强映射分析失败: {ex.Message}");
                return new IntelligentMappingResult();
            }
        }

        /// <summary>
        /// 构建增强版映射提示词（利用 BaseEntity 提供的元数据）
        /// </summary>
        private string BuildEnhancedMappingPrompt(List<string> excelHeaders, List<RUINORERP.Model.ImportFieldInfo> dbFields)
        {
            var sb = new StringBuilder();
            sb.AppendLine("你是一个资深的 ERP 系统数据导入专家。请根据以下信息建立 Excel 列与数据库字段的映射。");
            sb.AppendLine();
            sb.AppendLine("### Excel 表头:");
            foreach (var h in excelHeaders) sb.AppendLine($"- {h}");
            
            sb.AppendLine();
            sb.AppendLine("### 数据库字段定义 (格式: [物理名] | 中文描述 | 类型):");
            foreach (var f in dbFields)
            {
                sb.AppendLine($"- [{f.ColumnName}] | {f.Description} | {(f.IsPrimaryKey ? "主键" : "")}{(f.IsForeignKey ? "外键" : "")}");
            }

            sb.AppendLine();
            sb.AppendLine("### 任务要求:");
            sb.AppendLine("1. 找出最匹配的字段映射。");
            sb.AppendLine("2. 识别一个最适合做'逻辑主键'（用于去重/更新判断）的字段，通常是品号、编码或单号。");
            sb.AppendLine("3. 为每个匹配项给出 0-100 的置信度分数。");
            sb.AppendLine();
            sb.AppendLine("### 输出 JSON 格式:");
            sb.AppendLine(@"{
  ""mappings"": {
    ""Excel列名"": {
      ""targetField"": ""数据库物理名"",
      ""confidence"": 95
    }
  },
  ""suggestedLogicalKey"": ""数据库物理名""
}");
            return sb.ToString();
        }

        /// <summary>
        /// 解析增强版响应
        /// </summary>
        private IntelligentMappingResult ParseEnhancedResponse(string response)
        {
            var result = new IntelligentMappingResult();
            try
            {
                var jsonStart = response.IndexOf('{');
                var jsonEnd = response.LastIndexOf('}');
                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var json = response.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    dynamic obj = JsonConvert.DeserializeObject(json);

                    if (obj?.mappings != null)
                    {
                        foreach (var prop in obj.mappings)
                        {
                            result.Mappings[(string)prop.Name] = new MappingItem
                            {
                                TargetField = prop.Value.targetField,
                                Confidence = prop.Value.confidence ?? 80
                            };
                        }
                    }
                    result.SuggestedLogicalKey = obj?.suggestedLogicalKey;
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 验证数据质量
        /// </summary>
        /// <param name="data">要验证的数据</param>
        /// <param name="entityType">目标实体类型</param>
        /// <returns>数据质量报告</returns>
        public async Task<DataQualityReport> ValidateDataQualityAsync(List<Dictionary<string, object>> data, string entityType)
        {
            if (!_config.AIEnableDataImport || data == null || data.Count == 0)
            {
                return new DataQualityReport
                {
                    TotalRecords = data?.Count ?? 0,
                    ValidRecords = data?.Count ?? 0,
                    QualityScore = 100
                };
            }

            try
            {
                var prompt = BuildDataQualityPrompt(data, entityType);
                var response = await _llmService.GenerateAsync(prompt, temperature: 0.3f, maxTokens: 2048);

                if (!response.IsSuccess)
                {
                    return CreateDefaultQualityReport(data.Count);
                }

                return ParseQualityResponse(response.Response, data.Count);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AI数据质量验证失败: {ex.Message}");
                return CreateDefaultQualityReport(data.Count);
            }
        }

        /// <summary>
        /// 生成数据转换建议
        /// </summary>
        /// <param name="sourceData">源数据样本</param>
        /// <param name="targetEntity">目标实体类型</param>
        /// <returns>转换建议列表</returns>
        public async Task<List<DataTransformationSuggestion>> GenerateTransformationSuggestionsAsync(List<Dictionary<string, object>> sourceData, string targetEntity)
        {
            if (!_config.AIEnableDataImport || sourceData == null || sourceData.Count == 0)
            {
                return new List<DataTransformationSuggestion>();
            }

            try
            {
                var prompt = BuildTransformationPrompt(sourceData, targetEntity);
                var response = await _llmService.GenerateAsync(prompt, temperature: 0.3f, maxTokens: 2048);

                if (!response.IsSuccess)
                {
                    return new List<DataTransformationSuggestion>();
                }

                return ParseTransformationResponse(response.Response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AI转换建议生成失败: {ex.Message}");
                return new List<DataTransformationSuggestion>();
            }
        }

        #region 私有方法

        /// <summary>
        /// 构建映射提示词
        /// </summary>
        private string BuildMappingPrompt(List<string> excelColumns, List<string> targetFields)
        {
            var sb = new StringBuilder();
            sb.AppendLine("你是一个数据导入助手，请帮助分析Excel列名与目标系统字段的映射关系。");
            sb.AppendLine();
            sb.AppendLine("Excel文件中的列名：");
            for (int i = 0; i < excelColumns.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {excelColumns[i]}");
            }
            sb.AppendLine();
            sb.AppendLine("目标系统的字段名：");
            for (int i = 0; i < targetFields.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {targetFields[i]}");
            }
            sb.AppendLine();
            sb.AppendLine("请分析每个Excel列名最可能对应的目标字段，返回JSON格式：");
            sb.AppendLine("{");
            sb.AppendLine("  \"Excel列名1\": \"目标字段名1\",");
            sb.AppendLine("  \"Excel列名2\": \"目标字段名2\",");
            sb.AppendLine("  ...");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("注意：");
            sb.AppendLine("1. 只返回JSON，不要其他解释");
            sb.AppendLine("2. 如果某个列无法匹配，值设为null");
            sb.AppendLine("3. 考虑列名的语义相似性和常见命名习惯");

            return sb.ToString();
        }

        /// <summary>
        /// 构建带示例数据的映射提示词
        /// </summary>
        private string BuildMappingPromptWithSamples(List<string> excelColumns, List<string> targetFields, List<Dictionary<string, object>> sampleData)
        {
            var sb = new StringBuilder();
            sb.AppendLine("你是一个数据导入助手，请帮助分析Excel列名与目标系统字段的映射关系。");
            sb.AppendLine();
            sb.AppendLine("Excel文件中的列名：");
            for (int i = 0; i < excelColumns.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {excelColumns[i]}");
            }
            sb.AppendLine();
            sb.AppendLine("示例数据（前3行）：");
            sb.AppendLine("```");
            var sampleCount = Math.Min(sampleData.Count, 3);
            for (int i = 0; i < sampleCount; i++)
            {
                sb.AppendLine($"行{i + 1}: {JsonConvert.SerializeObject(sampleData[i])}");
            }
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("目标系统的字段名：");
            for (int i = 0; i < targetFields.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {targetFields[i]}");
            }
            sb.AppendLine();
            sb.AppendLine("请分析每个Excel列名最可能对应的目标字段，考虑列名语义和示例数据内容，返回JSON格式：");
            sb.AppendLine("{");
            sb.AppendLine("  \"Excel列名1\": \"目标字段名1\",");
            sb.AppendLine("  \"Excel列名2\": \"目标字段名2\",");
            sb.AppendLine("  ...");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("注意：只返回JSON，不要其他解释。如果某个列无法匹配，值设为null。");

            return sb.ToString();
        }

        /// <summary>
        /// 解析映射响应
        /// </summary>
        private Dictionary<string, string> ParseMappingResponse(string response, List<string> excelColumns)
        {
            var result = new Dictionary<string, string>();

            try
            {
                // 提取JSON部分
                var jsonStart = response.IndexOf('{');
                var jsonEnd = response.LastIndexOf('}');
                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var json = response.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    var mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    if (mapping != null)
                    {
                        foreach (var col in excelColumns)
                        {
                            if (mapping.ContainsKey(col) && !string.IsNullOrEmpty(mapping[col]))
                            {
                                result[col] = mapping[col];
                            }
                        }
                    }
                }
            }
            catch
            {
                // 解析失败返回空映射
            }

            return result;
        }

        /// <summary>
        /// 构建数据质量提示词
        /// </summary>
        private string BuildDataQualityPrompt(List<Dictionary<string, object>> data, string entityType)
        {
            var sb = new StringBuilder();
            sb.AppendLine("你是一个数据质量分析师，请分析以下数据的质量问题。");
            sb.AppendLine();
            sb.AppendLine($"目标实体类型: {entityType}");
            sb.AppendLine($"总记录数: {data.Count}");
            sb.AppendLine();
            sb.AppendLine("数据样本（前5行）：");
            sb.AppendLine("```");
            var sampleCount = Math.Min(data.Count, 5);
            for (int i = 0; i < sampleCount; i++)
            {
                sb.AppendLine($"行{i + 1}: {JsonConvert.SerializeObject(data[i])}");
            }
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("请分析数据质量问题，返回JSON格式：");
            sb.AppendLine("{");
            sb.AppendLine("  \"qualityScore\": 85,");
            sb.AppendLine("  \"validRecords\": 95,");
            sb.AppendLine("  \"issues\": [");
            sb.AppendLine("    {");
            sb.AppendLine("      \"issueType\": \"缺失值\",");
            sb.AppendLine("      \"description\": \"某列存在空值\",");
            sb.AppendLine("      \"affectedColumn\": \"列名\",");
            sb.AppendLine("      \"affectedRowCount\": 5,");
            sb.AppendLine("      \"suggestion\": \"建议填充默认值\"");
            sb.AppendLine("    }");
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("注意：只返回JSON，不要其他解释。qualityScore为0-100的质量评分。");

            return sb.ToString();
        }

        /// <summary>
        /// 创建默认质量报告
        /// </summary>
        private DataQualityReport CreateDefaultQualityReport(int totalRecords)
        {
            return new DataQualityReport
            {
                TotalRecords = totalRecords,
                ValidRecords = totalRecords,
                ProblematicRecords = 0,
                QualityScore = 100,
                Issues = new List<DataQualityIssue>()
            };
        }

        /// <summary>
        /// 解析质量响应
        /// </summary>
        private DataQualityReport ParseQualityResponse(string response, int totalRecords)
        {
            try
            {
                var jsonStart = response.IndexOf('{');
                var jsonEnd = response.LastIndexOf('}');
                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var json = response.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    dynamic result = JsonConvert.DeserializeObject(json);

                    var report = new DataQualityReport
                    {
                        TotalRecords = totalRecords,
                        QualityScore = result?.qualityScore ?? 100,
                        ValidRecords = result?.validRecords ?? totalRecords,
                        Issues = new List<DataQualityIssue>()
                    };

                    report.ProblematicRecords = totalRecords - report.ValidRecords;

                    if (result?.issues != null)
                    {
                        foreach (var issue in result.issues)
                        {
                            report.Issues.Add(new DataQualityIssue
                            {
                                IssueType = issue.issueType,
                                Description = issue.description,
                                AffectedColumn = issue.affectedColumn,
                                AffectedRowCount = issue.affectedRowCount,
                                Suggestion = issue.suggestion
                            });
                        }
                    }

                    return report;
                }
            }
            catch
            {
                // 解析失败返回默认报告
            }

            return CreateDefaultQualityReport(totalRecords);
        }

        /// <summary>
        /// 构建转换提示词
        /// </summary>
        private string BuildTransformationPrompt(List<Dictionary<string, object>> sourceData, string targetEntity)
        {
            var sb = new StringBuilder();
            sb.AppendLine("你是一个数据转换专家，请分析源数据并提供转换建议。");
            sb.AppendLine();
            sb.AppendLine($"目标实体: {targetEntity}");
            sb.AppendLine();
            sb.AppendLine("源数据样本（前3行）：");
            sb.AppendLine("```");
            var sampleCount = Math.Min(sourceData.Count, 3);
            for (int i = 0; i < sampleCount; i++)
            {
                sb.AppendLine($"行{i + 1}: {JsonConvert.SerializeObject(sourceData[i])}");
            }
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("请提供数据转换建议，返回JSON格式：");
            sb.AppendLine("{");
            sb.AppendLine("  \"suggestions\": [");
            sb.AppendLine("    {");
            sb.AppendLine("      \"sourceColumn\": \"源列名\",");
            sb.AppendLine("      \"targetField\": \"目标字段\",");
            sb.AppendLine("      \"transformationType\": \"转换类型\",");
            sb.AppendLine("      \"description\": \"转换说明\",");
            sb.AppendLine("      \"example\": \"示例\"");
            sb.AppendLine("    }");
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("注意：只返回JSON，不要其他解释。");

            return sb.ToString();
        }

        /// <summary>
        /// 解析转换响应
        /// </summary>
        private List<DataTransformationSuggestion> ParseTransformationResponse(string response)
        {
            var suggestions = new List<DataTransformationSuggestion>();

            try
            {
                var jsonStart = response.IndexOf('{');
                var jsonEnd = response.LastIndexOf('}');
                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var json = response.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    dynamic result = JsonConvert.DeserializeObject(json);

                    if (result?.suggestions != null)
                    {
                        foreach (var sug in result.suggestions)
                        {
                            suggestions.Add(new DataTransformationSuggestion
                            {
                                SourceColumn = sug.sourceColumn,
                                TargetField = sug.targetField,
                                TransformationType = sug.transformationType,
                                Description = sug.description,
                                Example = sug.example
                            });
                        }
                    }
                }
            }
            catch
            {
                // 解析失败返回空列表
            }

            return suggestions;
        }

        #endregion
    }
}
