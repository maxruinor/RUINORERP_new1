using System.ComponentModel;

namespace RUINORERP.Model.ImportEngine.Enums
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
        /// </summary>
        [Description("Excel数据源")]
        Excel = 0,
    
        /// <summary>
        /// 默认值
        /// 数据来源于配置的默认值
        /// </summary>
        [Description("默认值")]
        DefaultValue = 1,
    
        /// <summary>
        /// 系统生成
        /// 数据由系统自动生成（如自增ID、时间戳等）
        /// </summary>
        [Description("系统生成")]
        SystemGenerated = 2,
    
        /// <summary>
        /// 外键关联
        /// 数据来源于其他表的外键关联字段
        /// </summary>
        [Description("外键关联")]
        ForeignKey = 3,
    
        /// <summary>
        /// 自身字段引用
        /// 数据来源于当前表自身的其他字段（如树结构中的父类ID）
        /// </summary>
        [Description("自身字段引用")]
        SelfReference = 4,
    
        /// <summary>
        /// 字段复制
        /// 复制同一记录中另一个字段的值
        /// 例如：ProductName字段复制ProductCode字段的值
        /// </summary>
        [Description("字段复制")]
        FieldCopy = 5,
    
        /// <summary>
        /// 列拼接
        /// 将Excel中的多个列值拼接后赋值给目标字段
        /// 例如：将“姓氏”和“名字”列拼接为“姓名”字段
        /// </summary>
        [Description("列拼接")]
        ColumnConcat = 6,
    
        /// <summary>
        /// Excel图片
        /// 数据来源于Excel中的嵌入式图片
        /// 支持提取图片并保存到指定目录
        /// </summary>
        [Description("Excel图片")]
        ExcelImage = 7
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
