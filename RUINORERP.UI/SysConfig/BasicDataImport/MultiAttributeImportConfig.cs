using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 多属性产品导入配置
    /// 用于配置如何从Excel数据中识别并导入多属性产品
    /// </summary>
    [Serializable]
    public class MultiAttributeImportConfig
    {
        /// <summary>
        /// 是否启用多属性导入
        /// </summary>
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// 产品分组字段（用于识别同一基础产品的不同SKU）
        /// 例如："末位流水编号" 或 "编码代码（小类）"
        /// 相同分组值的行属于同一个基础产品
        /// </summary>
        public string GroupByField { get; set; }

        /// <summary>
        /// 基础产品信息字段映射
        /// 用于提取产品基础信息（品名、规格等）
        /// </summary>
        public BaseProductFieldMapping BaseProductFields { get; set; } = new BaseProductFieldMapping();

        /// <summary>
        /// 属性提取规则
        /// 用于从Excel列中提取属性信息
        /// </summary>
        public List<AttributeExtractionRule> AttributeExtractionRules { get; set; } = new List<AttributeExtractionRule>();

        /// <summary>
        /// SKU明细字段映射
        /// 用于提取SKU明细信息（编码、单价、成本价等）
        /// </summary>
        public List<SKUDetailFieldMapping> SKUDetailFields { get; set; } = new List<SKUDetailFieldMapping>();

        /// <summary>
        /// 属性值标准化规则
        /// 用于统一属性值的表达方式
        /// 例如："粉红色"、"玫红色" -> "粉色"
        /// </summary>
        public List<AttributeValueMapping> AttributeValueMappings { get; set; } = new List<AttributeValueMapping>();
    }

    /// <summary>
    /// 基础产品字段映射
    /// </summary>
    [Serializable]
    public class BaseProductFieldMapping
    {
        /// <summary>
        /// 品号Excel列名
        /// </summary>
        public string ProductNoColumn { get; set; }

        /// <summary>
        /// 品名Excel列名（如果不指定，使用供货商来货品名）
        /// </summary>
        public string CNNameColumn { get; set; }

        /// <summary>
        /// 规格Excel列名
        /// </summary>
        public string SpecificationsColumn { get; set; }

        /// <summary>
        /// 类别Excel列名
        /// </summary>
        public string CategoryColumn { get; set; }

        /// <summary>
        /// 单位Excel列名
        /// </summary>
        public string UnitColumn { get; set; }

        /// <summary>
        /// 品名清理规则
        /// 用于从"供货商来货品名"中清理属性信息，只保留纯品名
        /// 例如："8连长方肥皂225手工皂烘焙糕点饼干松饼蛋糕模具甜点厨房工具 颜色: 紫色"
        ///      清理后："8连长方肥皂225手工皂烘焙糕点饼干松饼蛋糕模具甜点厨房工具"
        /// </summary>
        public string CNNameCleanupPattern { get; set; } = @"[\s]+\w+:\s*[\u4e00-\u9fa5\w\s\-\(\)]+$";
    }

    /// <summary>
    /// 属性提取规则
    /// </summary>
    [Serializable]
    public class AttributeExtractionRule
    {
        /// <summary>
        /// 属性名称（系统中的属性名，如"颜色"、"规格"）
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Excel列名（属性值所在的列）
        /// 如果为空，则从"供货商来货品名"列中提取
        /// </summary>
        public string ExcelColumn { get; set; }

        /// <summary>
        /// 正则表达式模式
        /// 用于从文本中提取属性值
        /// 例如："颜色:\s*([\u4e00-\u9fa5\w\s\-\(\)]+)"
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 正则表达式分组索引
        /// 用于提取匹配结果中的特定分组
        /// </summary>
        public int GroupIndex { get; set; } = 1;

        /// <summary>
        /// 是否必填属性
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// 提取属性值
        /// </summary>
        /// <param name="inputText">输入文本</param>
        /// <returns>属性值，如果未找到则返回null</returns>
        public string ExtractValue(string inputText)
        {
            if (string.IsNullOrEmpty(inputText) || string.IsNullOrEmpty(Pattern))
            {
                return null;
            }

            try
            {
                var match = Regex.Match(inputText, Pattern, RegexOptions.IgnoreCase);
                if (match.Success && match.Groups.Count > GroupIndex)
                {
                    return match.Groups[GroupIndex].Value.Trim();
                }
            }
            catch
            {
                // 正则表达式错误，忽略
            }

            return null;
        }
    }

    /// <summary>
    /// SKU明细字段映射
    /// </summary>
    [Serializable]
    public class SKUDetailFieldMapping
    {
        /// <summary>
        /// 系统字段名（如"ProductNo"、"PurchasePrice"等）
        /// </summary>
        public string SystemField { get; set; }

        /// <summary>
        /// Excel列名
        /// </summary>
        public string ExcelColumn { get; set; }

        /// <summary>
        /// 字段类型（string、decimal、int等）
        /// </summary>
        public string DataType { get; set; } = "string";
    }

    /// <summary>
    /// 属性值标准化映射
    /// </summary>
    [Serializable]
    public class AttributeValueMapping
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 原始值（Excel中的值）
        /// </summary>
        public string OriginalValue { get; set; }

        /// <summary>
        /// 标准化值（系统中使用的值）
        /// </summary>
        public string StandardizedValue { get; set; }
    }
}
