using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RUINORERP.Global;
using RUINORERP.UI.SysConfig.BasicDataImport;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    #region Excel数据源配置

    /// <summary>
    /// Excel数据源配置
    /// 对应枚举值：DataSourceType.Excel
    /// 数据来源于Excel文件的对应列
    /// </summary>
    [Serializable]
    [XmlRoot("ExcelConfig")]
    public class ExcelConfig : DataSourceConfigBase
    {
        /// <summary>
        /// Excel列名
        /// </summary>
        [XmlElement("ExcelColumn")]
        public string ExcelColumn { get; set; }

        private bool _ignoreEmptyValue;
        /// <summary>
        /// 是否忽略空值（为空时不导入）
        /// 与 EmptyValueDefault 互斥，只能二选一
        /// </summary>
        [XmlElement("IgnoreEmptyValue")]
        public bool IgnoreEmptyValue
        {
            get => _ignoreEmptyValue;
            set
            {
                if (_ignoreEmptyValue != value)
                {
                    _ignoreEmptyValue = value;
                    // 如果启用忽略空值，则清空空值默认值
                    if (value)
                    {
                        EmptyValueDefault = string.Empty;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private string _emptyValueDefault;
        /// <summary>
        /// 空值时使用的默认值
        /// 与 IgnoreEmptyValue 互斥，只能二选一
        /// 当此值不为空时，自动取消忽略空值设置
        /// </summary>
        [XmlElement("EmptyValueDefault")]
        public string EmptyValueDefault
        {
            get => _emptyValueDefault;
            set
            {
                if (_emptyValueDefault != value)
                {
                    _emptyValueDefault = value;
                    // 如果设置了空值默认值，则取消忽略空值
                    if (!string.IsNullOrEmpty(value))
                    {
                        _ignoreEmptyValue = false;
                        OnPropertyChanged(nameof(IgnoreEmptyValue));
                    }
                    OnPropertyChanged();
                }
            }
        }
    }

    #endregion

    #region 默认固定值配置 

    /// <summary>
    /// 默认固定值配置
    /// 对应枚举值：DataSourceType.DefaultFixedValue
    /// 用于用户配置Excel导入时，将某些没有Excel数据源的目标列设置为一个固定的值
    /// 用户在配置中直接输入指定的固定值（字符串形式）
    /// 后台处理时会根据目标列的数据类型自动进行类型转换
    /// </summary>
    [Serializable]
    [XmlRoot("DefaultValueConfig")]
    public class DefaultValueConfig : DataSourceConfigBase
    {
        /// <summary>
        /// 数据来源类型
        /// </summary>
        /// <summary>
        /// 默认固定值（字符串形式）
        /// 当Excel中没有对应数据源时，使用此固定值填充目标字段
        /// 后台会根据目标列的类型自动转换为相应的数据类型（如bool、int、DateTime等）
        /// </summary>
        [XmlElement("Value")]
        public string Value { get; set; }
    }
    

    #endregion

    #region 系统生成配置

    /// <summary>
    /// 系统生成类型枚举
    /// </summary>
    public enum SystemGeneratedType
    {
        [System.ComponentModel.Description("系统时间")]
        DateTime = 0,

        [System.ComponentModel.Description("系统日期")]
        Date = 1,

        [System.ComponentModel.Description("创建人")]
        CreateUser = 2,

        //[System.ComponentModel.Description("创建人名称")]
        //CreateUserName = 3,

        [System.ComponentModel.Description("更新时间")]
        UpdateTime = 4,

        [System.ComponentModel.Description("更新人")]
        UpdateUser = 5,

        [System.ComponentModel.Description("业务编码")]
        BusinessCode = 6,

        [System.ComponentModel.Description("GUID")]
        Guid = 7,

        [System.ComponentModel.Description("状态值")]
        Status = 8,

        [System.ComponentModel.Description("删除标记")]
        IsDeleted = 9,

        [System.ComponentModel.Description("序号")]
        Sequence = 10,

        [System.ComponentModel.Description("自定义表达式")]
        CustomExpression = 11,

        [System.ComponentModel.Description("系统编号生成")]
        EntityBizCode = 12
    }

    /// <summary>
    /// 业务编码生成规则枚举
    /// </summary>
    public enum BusinessCodeRule
    {
        [System.ComponentModel.Description("日期+序号")]
        DateSequence = 0,

        [System.ComponentModel.Description("前缀+日期+序号")]
        PrefixDateSequence = 1,

        [System.ComponentModel.Description("前缀+序号")]
        PrefixSequence = 2,

        [System.ComponentModel.Description("仅序号")]
        OnlySequence = 3,

        [System.ComponentModel.Description("年份+序号")]
        YearSequence = 4,

        [System.ComponentModel.Description("年月+序号")]
        YearMonthSequence = 5
    }

    /// <summary>
    /// 系统生成配置
    /// 对应枚举值：DataSourceType.SystemGenerated
    /// 数据由系统自动生成（如时间、用户、业务编码等）
    /// </summary>
    [Serializable]
    [XmlRoot("SystemGeneratedConfig")]
    public class SystemGeneratedConfig : DataSourceConfigBase
    {
        /// <summary>
        /// 系统生成类型
        /// </summary>
        [XmlElement("GeneratedType")]
        public SystemGeneratedType GeneratedType { get; set; } = SystemGeneratedType.DateTime;

        /// <summary>
        /// 时间格式（当GeneratedType为DateTime或Date时使用）
        /// </summary>
        [XmlElement("DateTimeFormat")]
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 业务编码前缀（当GeneratedType为BusinessCode时使用）
        /// </summary>
        [XmlElement("BusinessCodePrefix")]
        public string BusinessCodePrefix { get; set; } = string.Empty;

        /// <summary>
        /// 业务编码生成规则（当GeneratedType为BusinessCode时使用）
        /// </summary>
        [XmlElement("BusinessCodeRule")]
        public BusinessCodeRule BusinessCodeRule { get; set; } = BusinessCodeRule.DateSequence;

        /// <summary>
        /// 序号位数（当GeneratedType为BusinessCode或Sequence时使用）
        /// </summary>
        [XmlElement("SequenceDigits")]
        public int SequenceDigits { get; set; } = 4;

        /// <summary>
        /// 自定义表达式（当GeneratedType为CustomExpression时使用）
        /// 支持的变量：{Now}, {NowDate}, {NowTime}, {UserID}, {UserName}, {Guid}, {Sequence}
        /// </summary>
        [XmlElement("CustomExpression")]
        public string CustomExpression { get; set; } = string.Empty;

        /// <summary>
        /// 自定义默认值（当GeneratedType为Status或IsDeleted时使用）
        /// </summary>
        [XmlElement("CustomDefaultValue")]
        public string CustomDefaultValue { get; set; } = "1";

        /// <summary>
        /// 实体业务编号类型（当GeneratedType为EntityBizCode时使用）
        /// 对应 BaseInfoType 枚举值，如：ProductNo、SKU_No、ProCategories、Customer、Supplier 等
        /// </summary>
        [XmlElement("EntityBizCodeType")]
        public int EntityBizCodeType { get; set; } = 0;
        /// <summary>
        /// 获取业务编码生成规则的显示名称
        /// </summary>
        public string GetBusinessCodeRuleDisplayName()
        {
            return GetEnumDescription(BusinessCodeRule);
        }

        /// <summary>
        /// 获取系统生成类型的显示名称
        /// </summary>
        public string GetGeneratedTypeDisplayName()
        {
            return GetEnumDescription(GeneratedType);
        }

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        private string GetEnumDescription(System.Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field != null)
            {
                var attr = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                    as System.ComponentModel.DescriptionAttribute[];
                if (attr != null && attr.Length > 0)
                {
                    return attr[0].Description;
                }
            }
            return enumValue.ToString();
        }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        public override bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            switch (GeneratedType)
            {
                case SystemGeneratedType.BusinessCode:
                    if (BusinessCodeRule == BusinessCodeRule.PrefixDateSequence ||
                        BusinessCodeRule == BusinessCodeRule.PrefixSequence)
                    {
                        if (string.IsNullOrWhiteSpace(BusinessCodePrefix))
                        {
                            errorMessage = "业务编码规则选择了带前缀的方式，请输入前缀";
                            return false;
                        }
                    }
                    if (SequenceDigits < 1 || SequenceDigits > 10)
                    {
                        errorMessage = "序号位数必须在1-10之间";
                        return false;
                    }
                    break;

                case SystemGeneratedType.CustomExpression:
                    if (string.IsNullOrWhiteSpace(CustomExpression))
                    {
                        errorMessage = "请输入自定义表达式";
                        return false;
                    }
                    break;

                case SystemGeneratedType.Sequence:
                    if (SequenceDigits < 1 || SequenceDigits > 10)
                    {
                        errorMessage = "序号位数必须在1-10之间";
                        return false;
                    }
                    break;
            }

            return true;
        }
    }

    #endregion

    #region 数据库表关联引用配置

    /// <summary>
    /// 数据库表关联引用配置
    /// 对应枚举值：DataSourceType.ForeignKey
    /// 数据来源于数据库表的关联字段，支持两种模式：
    /// 1. 外键关联：关联其他表的字段（如产品关联供应商）
    /// 2. 自身表引用：关联当前目标表自身的字段（如产品类目的父类ID、字段复制等）
    /// </summary>
    [Serializable]
    [XmlRoot("DatabaseReferenceConfig")]
    public class DatabaseReferenceConfig : DataSourceConfigBase
    {
        /// <summary>
        /// 是否为自身表引用
        /// true: 引用当前目标表自身的字段（如树结构中的父类ID）
        /// false: 引用其他表的字段（外键关联）
        /// </summary>
        [XmlElement("IsSelfReference")]
        public bool IsSelfReference { get; set; } = false;

        /// <summary>
        /// 关联表名（英文）
        /// 当IsSelfReference=true时，此值为目标表名
        /// </summary>
        [XmlElement("ForeignTableName")]
        public string ForeignTableName { get; set; }

        /// <summary>
        /// 关联表显示名（中文）
        /// </summary>
        [XmlElement("ForeignTableDisplayName")]
        public string ForeignTableDisplayName { get; set; }

        /// <summary>
        /// 关联字段名（英文）
        /// </summary>
        [XmlElement("ForeignFieldName")]
        public string ForeignFieldName { get; set; }

        /// <summary>
        /// 关联字段显示名（中文）
        /// </summary>
        [XmlElement("ForeignFieldDisplayName")]
        public string ForeignFieldDisplayName { get; set; }

        /// <summary>
        /// 显示字段名（用于在UI中显示的字段）
        /// </summary>
        [XmlElement("DisplayFieldName")]
        public string DisplayFieldName { get; set; }

        /// <summary>
        /// 显示字段显示名（中文）
        /// </summary>
        [XmlElement("DisplayFieldDisplayName")]
        public string DisplayFieldDisplayName { get; set; }

        /// <summary>
        /// 外键来源列（Excel中的列名）
        /// </summary>
        [XmlElement("ForeignKeySourceColumn")]
        public SerializableKeyValuePair<string> ForeignKeySourceColumn { get; set; }

        /// <summary>
        /// 当外键关联失败时是否使用默认值
        /// </summary>
        [XmlElement("UseDefaultWhenNotFound")]
        public bool UseDefaultWhenNotFound { get; set; }

        /// <summary>
        /// 外键关联失败时的默认值
        /// </summary>
        [XmlElement("DefaultValueWhenNotFound")]
        public string DefaultValueWhenNotFound { get; set; }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        public override bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(ForeignTableName))
            {
                errorMessage = "请选择关联表";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ForeignFieldName))
            {
                errorMessage = "请选择关联字段";
                return false;
            }

            if (IsSelfReference && !string.IsNullOrWhiteSpace(ForeignTableName))
            {
                if (TargetEntityType != null && ForeignTableName != TargetEntityType.Name)
                {
                    errorMessage = "自身引用必须引用当前目标表";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 目标实体类型（用于验证自身引用）
        /// </summary>
        [XmlIgnore]
        public Type TargetEntityType { get; set; }
    }

    #endregion

    #region 列拼接配置

    /// <summary>
    /// 列拼接配置
    /// 对应枚举值：DataSourceType.ColumnConcat
    /// 将Excel中的多个列值拼接后赋值给目标字段
    /// </summary>
    [Serializable]
    [XmlRoot("ColumnConcatConfig")]
    public class ColumnConcatConfig : DataSourceConfigBase
    {
        /// <summary>
        /// 数据来源类型
        /// </summary>
        /// <summary>
        /// 拼接的列名列表（旧版本兼容）
        /// </summary>
        [XmlArray("SourceColumns")]
        [XmlArrayItem("Column")]
        public List<string> SourceColumns { get; set; } = new List<string>();

        /// <summary>
        /// 拼接的列名列表（新架构）
        /// </summary>
        [XmlArray("ConcatColumns")]
        [XmlArrayItem("Column")]
        public List<SerializableKeyValuePair<string>> ConcatColumns { get; set; } = new List<SerializableKeyValuePair<string>>();

        /// <summary>
        /// 拼接分隔符
        /// </summary>
        [XmlElement("Separator")]
        public string Separator { get; set; } = string.Empty;

        /// <summary>
        /// 是否忽略空值列
        /// </summary>
        [XmlElement("IgnoreEmptyColumns")]
        public bool IgnoreEmptyColumns { get; set; } = true;

        /// <summary>
        /// 是否去除空白
        /// </summary>
        [XmlElement("TrimWhitespace")]
        public bool TrimWhitespace { get; set; } = true;

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        public override bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (ConcatColumns == null || ConcatColumns.Count < 2)
            {
                errorMessage = "请至少选择两个要拼接的列";
                return false;
            }

            return true;
        }
    }

    #endregion

    #region Excel图片配置

    /// <summary>
    /// 图片存储类型
    /// </summary>
    public enum ImageStorageType
    {
        [System.ComponentModel.Description("保存为文件")]
        File = 0,

        [System.ComponentModel.Description("保存为二进制")]
        Binary = 1,

        [System.ComponentModel.Description("Base64编码")]
        Base64 = 2,

        [System.ComponentModel.Description("文件路径")]
        FilePath = 3
    }

    /// <summary>
    /// 图片命名规则
    /// </summary>
    public enum ImageNamingRule
    {
        [System.ComponentModel.Description("原始文件名")]
        Original = 0,

        [System.ComponentModel.Description("GUID命名")]
        Guid = 1,

        [System.ComponentModel.Description("指定列值")]
        ColumnValue = 2,

        [System.ComponentModel.Description("时间戳")]
        Timestamp = 3,

        [System.ComponentModel.Description("组合命名")]
        Combined = 4,

        [System.ComponentModel.Description("自增序号")]
        AutoIncrement = 5
    }

    /// <summary>
    /// 图片列类型
    /// </summary>
    public enum ImageColumnType
    {
        [System.ComponentModel.Description("图片路径")]
        Path = 0,

        [System.ComponentModel.Description("二进制图片")]
        Binary = 1
    }

    /// <summary>
    /// 图片格式转换类型
    /// </summary>
    public enum ImageConvertType
    {
        [System.ComponentModel.Description("保持原格式")]
        KeepOriginal = 0,

        [System.ComponentModel.Description("转换为JPEG")]
        ToJpeg = 1,

        [System.ComponentModel.Description("转换为PNG")]
        ToPng = 2,

        [System.ComponentModel.Description("转换为WebP")]
        ToWebP = 3
    }

    /// <summary>
    /// Excel图片配置
    /// 对应枚举值：DataSourceType.ExcelImage
    /// 数据来源于Excel中的嵌入式图片
    /// </summary>
    [Serializable]
    [XmlRoot("ExcelImageConfig")]
    public class ExcelImageConfig : DataSourceConfigBase
    {
        /// <summary>
        /// 图片存储类型
        /// </summary>
        [XmlElement("StorageType")]
        public ImageStorageType StorageType { get; set; } = ImageStorageType.File;

        /// <summary>
        /// 图片命名规则
        /// </summary>
        [XmlElement("NamingRule")]
        public ImageNamingRule NamingRule { get; set; } = ImageNamingRule.Guid;

        /// <summary>
        /// 图片输出目录（当StorageType为File时使用）
        /// </summary>
        [XmlElement("OutputDirectory")]
        public string OutputDirectory { get; set; } = string.Empty;

        /// <summary>
        /// 命名引用列（当NamingRule为ColumnValue时使用）
        /// </summary>
        [XmlElement("NamingReferenceColumn")]
        public string NamingReferenceColumn { get; set; } = string.Empty;

        /// <summary>
        /// 是否保留原始扩展名
        /// </summary>
        [XmlElement("PreserveExtension")]
        public bool PreserveExtension { get; set; } = true;

        /// <summary>
        /// 图片格式转换类型
        /// </summary>
        [XmlElement("ConvertType")]
        public ImageConvertType ConvertType { get; set; } = ImageConvertType.KeepOriginal;

        /// <summary>
        /// 图片质量（0-100，仅对JPEG有效）
        /// </summary>
        [XmlElement("Quality")]
        public int Quality { get; set; } = 90;

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        public override bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (StorageType == ImageStorageType.File && string.IsNullOrWhiteSpace(OutputDirectory))
            {
                errorMessage = "请指定图片输出目录";
                return false;
            }

            if (NamingRule == ImageNamingRule.ColumnValue && string.IsNullOrWhiteSpace(NamingReferenceColumn))
            {
                errorMessage = "请选择命名引用列";
                return false;
            }

            if (Quality < 0 || Quality > 100)
            {
                errorMessage = "图片质量必须在0-100之间";
                return false;
            }

            return true;
        }
    }

    #endregion

}