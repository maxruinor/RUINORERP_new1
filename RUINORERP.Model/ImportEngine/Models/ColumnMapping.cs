using System;
using RUINORERP.Global;
using RUINORERP.Model.ImportEngine.Enums;

namespace RUINORERP.Model.ImportEngine.Models
{
    /// <summary>
    /// 列映射配置（通用模型，供UI层和Business层共享）
    /// </summary>
    public class ColumnMapping
    {
        /// <summary>
        /// Excel表头名称
        /// </summary>
        public string ExcelHeader { get; set; }
        
        /// <summary>
        /// Excel列名（兼容旧版本）
        /// </summary>
        public string ExcelColumn { get; set; }
        
        /// <summary>
        /// 数据库字段名
        /// </summary>
        public string DbColumn { get; set; }
        
        /// <summary>
        /// 系统字段信息（Key:显示名称, Value:数据库字段名）
        /// </summary>
        public SerializableKeyValuePair<string> SystemField { get; set; }
        
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }
        
        /// <summary>
        /// 是否为唯一键
        /// </summary>
        public bool IsUniqueKey { get; set; }
        
        /// <summary>
        /// 数据类型（String, Int, Decimal, DateTime等）
        /// </summary>
        public string DataType { get; set; }
        
        /// <summary>
        /// 数据源类型
        /// </summary>
        public DataSourceType DataSourceType { get; set; }
        
        /// <summary>
        /// 外键配置（当DataSourceType为ForeignKey时使用）
        /// </summary>
        public ForeignRelatedConfig ForeignConfig { get; set; }
        
        /// <summary>
        /// 转换规则
        /// </summary>
        public string TransformRule { get; set; }
        
        /// <summary>
        /// 是否忽略空值
        /// </summary>
        public bool IgnoreEmptyValue { get; set; }
        
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
