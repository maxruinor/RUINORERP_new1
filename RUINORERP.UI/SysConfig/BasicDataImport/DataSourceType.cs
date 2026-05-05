using System.ComponentModel;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 数据来源类型枚举
    /// 用于标识字段数据的来源方式
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// Excel数据源（默认）
        /// 数据来源于Excel文件的对应列
        /// 对应配置类：ExcelConfig
        /// </summary>
        [Description("Excel数据源")]
        Excel = 0,
        
        /// <summary>
        /// 默认固定值
        /// 数据来源于用户配置的固定值（当Excel中没有对应数据源时使用）
        /// 对应配置类：DefaultValueConfig
        /// </summary>
        [Description("默认固定值")]
        DefaultFixedValue = 1,
        
        /// <summary>
        /// 系统生成
        /// 数据由系统自动生成（如时间、用户、业务编码等）
        /// 对应配置类：SystemGeneratedConfig
        /// </summary>
        [Description("系统生成")]
        SystemGenerated = 2,
        
        /// <summary>
        /// 数据库表关联引用
        /// 数据来源于数据库表的关联字段，支持两种模式：
        /// 1. 外键关联：关联其他表的字段（如产品关联供应商）
        /// 2. 自身表引用：关联当前目标表自身的字段（如产品类目的父类ID、字段复制）
        /// 对应配置类：DatabaseReferenceConfig
        /// </summary>
        [Description("数据库表关联引用")]
        ForeignKey = 3,
        
        /// <summary>
        /// 列拼接
        /// 将Excel中的多个列值拼接后赋值给目标字段
        /// 例如：将"姓氏"和"名字"列拼接为"姓名"字段
        /// 对应配置类：ColumnConcatConfig
        /// </summary>
        [Description("列拼接")]
        ColumnConcat = 4,
        
        /// <summary>
        /// Excel图片
        /// 数据来源于Excel中的嵌入式图片
        /// 支持提取图片并保存到指定目录
        /// 对应配置类：ExcelImageConfig
        /// </summary>
        [Description("Excel图片")]
        ExcelImage = 5
    }
    
    /// <summary>
    /// 去重策略
    /// </summary>
    public enum DeduplicationStrategy
    {
        /// <summary>
        /// 保留首次出现
        /// </summary>
        FirstOccurrence,
        
        /// <summary>
        /// 保留最后一次出现
        /// </summary>
        LastOccurrence
    }
}
