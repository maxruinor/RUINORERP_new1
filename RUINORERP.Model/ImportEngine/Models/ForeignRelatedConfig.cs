using System;

namespace RUINORERP.Model.ImportEngine.Models
{
    /// <summary>
    /// 可序列化键值对（通用辅助类）
    /// </summary>
    [Serializable]
    public class SerializableKeyValuePair<T>
    {
        /// <summary>
        /// 键
        /// </summary>
        public T Key { get; set; }
        
        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }
        
        public SerializableKeyValuePair()
        {
        }
        
        public SerializableKeyValuePair(T key, T value)
        {
            Key = key;
            Value = value;
        }
    }
    
    /// <summary>
    /// 外键来源列配置
    /// </summary>
    [Serializable]
    public class ForeignKeySourceColumnConfig
    {
        /// <summary>
        /// Excel列名
        /// </summary>
        public string ExcelColumnName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 数据库真实字段名
        /// </summary>
        public string DatabaseFieldName { get; set; }
    }

    /// <summary>
    /// 外键关联配置类
    /// 用于存储外部关联类型的列配置信息
    /// </summary>
    [Serializable]
    public class ForeignRelatedConfig
    {
        /// <summary>
        /// 外键表引用（Key=英文表名, Value=中文表名）
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyTable { get; set; }

        /// <summary>
        /// 外键字段引用（Key=英文字段名, Value=中文显示名）
        /// 用于指定关联表中用于匹配的字段（通常是编码字段）
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyField { get; set; }

        /// <summary>
        /// 外键来源列配置
        /// 指定Excel中作为外键关联依据的来源列
        /// </summary>
        public ForeignKeySourceColumnConfig ForeignKeySourceColumn { get; set; }

        /// <summary>
        /// 是否允许创建新记录
        /// 如果外键值在目标表中不存在，是否自动创建新记录
        /// </summary>
        public bool AllowCreateNew { get; set; }

        /// <summary>
        /// 默认值（当外键匹配失败时使用）
        /// </summary>
        public string DefaultValue { get; set; }

        public ForeignRelatedConfig()
        {
            ForeignKeyTable = new SerializableKeyValuePair<string>();
            ForeignKeyField = new SerializableKeyValuePair<string>();
            ForeignKeySourceColumn = new ForeignKeySourceColumnConfig();
            AllowCreateNew = false;
        }
    }
}
