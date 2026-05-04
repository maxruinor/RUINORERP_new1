using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using RUINORERP.Global;
using RUINORERP.Model.ImportEngine.Enums;
using RUINORERP.Model.ImportEngine.Models;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列映射配置模型
    /// 用于存储Excel列与系统字段的映射关系
    /// 采用统一的数据源配置架构，每种数据来源类型都有对应的配置类
    /// </summary>
    [Serializable]
    [XmlRoot("ColumnMapping")]
    [XmlInclude(typeof(ExcelConfig))]
    [XmlInclude(typeof(DefaultValueConfig))]
    [XmlInclude(typeof(SystemGeneratedConfig))]
    [XmlInclude(typeof(ForeignKeyConfig))]
    [XmlInclude(typeof(SelfReferenceConfig))]
    [XmlInclude(typeof(FieldCopyConfig))]
    [XmlInclude(typeof(ColumnConcatConfig))]
    [XmlInclude(typeof(ExcelImageConfig))]
    public class ColumnMapping : INotifyPropertyChanged
    {
        #region 属性变更通知

        /// <summary>
        /// 属性变更事件
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 触发属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名称（调用者无需传递，编译器自动填充）</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置属性值并触发变更事件
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">属性名称（调用者无需传递，编译器自动填充）</param>
        /// <returns>如果值发生变化返回 true，否则返回 false</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region 基础属性

        /// <summary>
        /// 映射ID（唯一标识）
        /// </summary>
        [XmlElement("MappingId")]
        public Guid MappingId { get; set; } = Guid.NewGuid();

        private SerializableKeyValuePair<string> _systemField;
        /// <summary>
        /// 系统字段引用（键值对：Key=英文字段名, Value=中文显示名）
        /// </summary>
        [XmlElement("SystemField")]
        public SerializableKeyValuePair<string> SystemField
        {
            get => _systemField;
            set => SetProperty(ref _systemField, value);
        }

        private bool _isUniqueValue;
        /// <summary>
        /// 是否值唯一（用于去重判断）
        /// </summary>
        [XmlElement("IsUniqueValue")]
        public bool IsUniqueValue
        {
            get => _isUniqueValue;
            set => SetProperty(ref _isUniqueValue, value);
        }

        private bool _isBusinessKey;
        /// <summary>
        /// 是否为业务键字段（用于唯一性校验）
        /// </summary>
        [XmlElement("IsBusinessKey")]
        public bool IsBusinessKey
        {
            get => _isBusinessKey;
            set => SetProperty(ref _isBusinessKey, value);
        }

        private string _dataType;
        /// <summary>
        /// 数据类型（目标字段的数据类型）
        /// </summary>
        [XmlElement("DataType")]
        public string DataType
        {
            get => _dataType;
            set => SetProperty(ref _dataType, value);
        }

        #endregion

        #region 统一数据源配置

        private int _columnDataSourceType = (int)DataSourceType.Excel;
        /// <summary>
        /// 数据来源类型
        /// 用于标识字段数据的来源方式
        /// </summary>
        [XmlElement("DataSourceType")]
        public int ColumnDataSourceType
        {
            get => _columnDataSourceType;
            set => SetProperty(ref _columnDataSourceType, value);
        }

        private IDataSourceConfig _dataSourceConfig;
        /// <summary>
        /// 数据源配置（统一配置接口）
        /// 根据DataSourceType的不同，存储对应的配置对象
        /// </summary>
        [XmlElement("DataSourceConfig")]
        public IDataSourceConfig DataSourceConfig
        {
            get => _dataSourceConfig;
            set => SetProperty(ref _dataSourceConfig, value);
        }

        #endregion

        #region 列映射基础属性

        private string _originalExcelColumn;
        /// <summary>
        /// 原始Excel列名
        /// 用户选择的Excel列名，如果未选择则为空
        /// 用于显示和删除映射时恢复Excel列列表
        /// </summary>
        [XmlElement("OriginalExcelColumn")]
        public string OriginalExcelColumn
        {
            get => _originalExcelColumn;
            set => SetProperty(ref _originalExcelColumn, value);
        }

        private string _targetDataType;
        /// <summary>
        /// 目标字段数据类型
        /// 用于在配置对话框中显示对应的Tab页
        /// </summary>
        [XmlElement("TargetDataType")]
        public string TargetDataType
        {
            get => _targetDataType;
            set => SetProperty(ref _targetDataType, value);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取系统字段名
        /// </summary>
        public string GetSystemFieldName()
        {
            return SystemField?.Key ?? string.Empty;
        }

        /// <summary>
        /// 获取系统字段显示名
        /// </summary>
        public string GetSystemFieldDisplayName()
        {
            return SystemField?.Value ?? string.Empty;
        }

        /// <summary>
        /// 获取配置的显示描述
        /// </summary>
        public string GetConfigDescription()
        {
            switch (ColumnDataSourceType)
            {
                case (int)DataSourceType.Excel:
                    var excelConfig = DataSourceConfig as ExcelConfig;
                    return $"Excel列: {excelConfig?.ExcelColumn ?? string.Empty}";

                case (int)DataSourceType.DefaultValue:
                    var defaultConfig = DataSourceConfig as DefaultValueConfig;
                    return $"默认值: {defaultConfig?.Value ?? string.Empty}";

                case (int)DataSourceType.SystemGenerated:
                    var sysConfig = DataSourceConfig as SystemGeneratedConfig;
                    return $"系统生成: {sysConfig?.GetGeneratedTypeDisplayName() ?? "系统生成"}";

                case (int)DataSourceType.ForeignKey:
                    var foreignConfig = DataSourceConfig as ForeignKeyConfig;
                    return $"外键关联: {foreignConfig?.ForeignTableDisplayName ?? "外键"}";

                case (int)DataSourceType.SelfReference:
                    var selfConfig = DataSourceConfig as SelfReferenceConfig;
                    return $"自身引用: {selfConfig?.ReferenceFieldDisplayName ?? "自身引用"}";

                case (int)DataSourceType.FieldCopy:
                    var copyConfig = DataSourceConfig as FieldCopyConfig;
                    return $"字段复制: {copyConfig?.SourceFieldDisplayName ?? "字段复制"}";

                case (int)DataSourceType.ColumnConcat:
                    return "列拼接";

                case (int)DataSourceType.ExcelImage:
                    return "Excel图片";

                default:
                    return "未知来源";
            }
        }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            // 验证系统字段必须设置
            if (SystemField == null || string.IsNullOrWhiteSpace(SystemField.Key))
            {
                errorMessage = "请选择系统字段";
                return false;
            }

            // 如果有数据源配置，验证配置
            if (DataSourceConfig != null)
            {
                return DataSourceConfig.Validate(out errorMessage);
            }

            return true;
        }

        #endregion
    }
}