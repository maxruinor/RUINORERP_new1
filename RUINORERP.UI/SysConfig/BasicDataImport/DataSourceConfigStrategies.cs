using RUINORERP.Model.ImportEngine.Enums;
using System;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 数据源配置策略接口
    /// 定义加载和保存配置的统一契约
    /// </summary>
    public interface IDataSourceConfigStrategy
    {
        /// <summary>
        /// 数据源类型
        /// </summary>
        DataSourceType DataSourceType { get; }

        /// <summary>
        /// 从配置加载到UI控件
        /// </summary>
        /// <param name="form">配置窗体</param>
        /// <param name="config">数据源配置</param>
        void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config);

        /// <summary>
        /// 从UI控件保存到配置
        /// </summary>
        /// <param name="form">配置窗体</param>
        /// <returns>数据源配置对象</returns>
        IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form);

        /// <summary>
        /// 验证配置
        /// </summary>
        /// <param name="form">配置窗体</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>是否验证通过</returns>
        bool Validate(FrmColumnPropertyConfig form, out string errorMessage);
    }

    /// <summary>
    /// 数据源配置策略基类
    /// 提供默认实现
    /// </summary>
    public abstract class DataSourceConfigStrategyBase : IDataSourceConfigStrategy
    {
        public abstract DataSourceType DataSourceType { get; }

        public virtual void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config)
        {
            // 默认不执行任何操作
        }

        public abstract IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form);

        public virtual bool Validate(FrmColumnPropertyConfig form, out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    #region Excel数据源策略

    public class ExcelConfigStrategy : DataSourceConfigStrategyBase
    {
        public override DataSourceType DataSourceType => DataSourceType.Excel;

        public override void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config)
        {
            var excelConfig = config as ExcelConfig;
            if (excelConfig != null)
            {
                form.kchkIgnoreEmptyValue.Checked = excelConfig.IgnoreEmptyValue;
            }
        }

        public override IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form)
        {
            string excelColumn = string.Empty;
            if (form.CurrentMapping != null)
            {
                var existingConfig = form.CurrentMapping.DataSourceConfig as ExcelConfig;
                if (existingConfig != null)
                {
                    excelColumn = existingConfig.ExcelColumn;
                }
            }

            return new ExcelConfig
            {
                ExcelColumn = excelColumn,
                IgnoreEmptyValue = form.kchkIgnoreEmptyValue.Checked
            };
        }
    }

    #endregion

    #region 默认值策略

    public class DefaultValueConfigStrategy : DataSourceConfigStrategyBase
    {
        public override DataSourceType DataSourceType => DataSourceType.DefaultValue;

        public override void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config)
        {
            var defaultConfig = config as DefaultValueConfig;
            if (defaultConfig != null)
            {
                form.DefaultValue = defaultConfig.Value;
                form.EnumTypeName = defaultConfig.EnumTypeName;
                form.EnumDefaultConfig = defaultConfig;
            }
        }

        public override IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form)
        {
            string defaultValue = form.GetDefaultValueFromDynamicControl();
            if (string.IsNullOrWhiteSpace(defaultValue))
            {
                return null;
            }

            var config = new DefaultValueConfig
            {
                Value = defaultValue
            };

            // 如果是枚举类型控件，保存完整的枚举配置
            var enumComboBox = form.GetDynamicDefaultValueControl() as Krypton.Toolkit.KryptonComboBox;
            if (enumComboBox != null && enumComboBox.Name == "cmbDynamicDefaultEnum" &&
                enumComboBox.SelectedItem is EntityImportHelper.EnumItemInfo enumInfo)
            {
                config.EnumTypeName = enumInfo.EnumType.FullName;
                config.EnumValue = enumInfo.EnumValue;
                config.EnumName = enumInfo.EnumName;
                config.EnumDisplayName = enumInfo.DisplayName;
            }

            return config;
        }

        public override bool Validate(FrmColumnPropertyConfig form, out string errorMessage)
        {
            string defaultValue = form.GetDefaultValueFromDynamicControl();
            if (string.IsNullOrWhiteSpace(defaultValue))
            {
                errorMessage = "请输入默认值";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
    }

    #endregion

    #region 系统生成策略

    public class SystemGeneratedConfigStrategy : DataSourceConfigStrategyBase
    {
        public override DataSourceType DataSourceType => DataSourceType.SystemGenerated;

        public override void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config)
        {
            var sysConfig = config as SystemGeneratedConfig;
            if (sysConfig != null)
            {
                form.kcmbSystemGeneratedType.SelectedIndex = (int)sysConfig.GeneratedType;
                form.ktxtDateTimeFormat.Text = sysConfig.DateTimeFormat;
                form.ktxtBusinessCodePrefix.Text = sysConfig.BusinessCodePrefix;
                form.kcmbBusinessCodeRule.SelectedIndex = (int)sysConfig.BusinessCodeRule;
                form.ktxtSequenceDigits.Text = sysConfig.SequenceDigits.ToString();
                form.ktxtCustomExpression.Text = sysConfig.CustomExpression;
                form.ktxtCustomDefaultValue.Text = sysConfig.CustomDefaultValue;
            }
        }

        public override IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form)
        {
            return new SystemGeneratedConfig
            {
                GeneratedType = (SystemGeneratedType)(form.kcmbSystemGeneratedType?.SelectedIndex ?? 0),
                DateTimeFormat = form.ktxtDateTimeFormat?.Text ?? "yyyy-MM-dd HH:mm:ss",
                BusinessCodePrefix = form.ktxtBusinessCodePrefix?.Text ?? string.Empty,
                BusinessCodeRule = (BusinessCodeRule)(form.kcmbBusinessCodeRule?.SelectedIndex ?? 0),
                SequenceDigits = int.TryParse(form.ktxtSequenceDigits?.Text, out int digits) ? digits : 4,
                CustomExpression = form.ktxtCustomExpression?.Text ?? string.Empty,
                CustomDefaultValue = form.ktxtCustomDefaultValue?.Text ?? "1"
            };
        }

        public override bool Validate(FrmColumnPropertyConfig form, out string errorMessage)
        {
            var config = SaveToConfig(form) as SystemGeneratedConfig;
            return config.Validate(out errorMessage);
        }
    }

    #endregion

    #region 外键关联策略

    public class ForeignKeyConfigStrategy : DataSourceConfigStrategyBase
    {
        public override DataSourceType DataSourceType => DataSourceType.ForeignKey;

        public override void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config)
        {
            var foreignConfig = config as ForeignKeyConfig;
            if (foreignConfig != null && !string.IsNullOrEmpty(foreignConfig.ForeignTableName))
            {
                // 查找对应的显示文本
                for (int i = 0; i < form.kcmbRelatedTable.Items.Count; i++)
                {
                    string itemText = form.kcmbRelatedTable.Items[i].ToString();
                    if (itemText.Contains(foreignConfig.ForeignTableName))
                    {
                        form.kcmbRelatedTable.SelectedIndex = i;
                        break;
                    }
                }
                form.ktxtRelatedField.Text = foreignConfig.ForeignFieldDisplayName;

                // 初始化外键来源列
                if (!string.IsNullOrEmpty(foreignConfig.DisplayFieldName))
                {
                    form.ForeignKeySourceColumn = new SerializableKeyValuePair<string>(foreignConfig.DisplayFieldName, foreignConfig.DisplayFieldName);
                }
                form.LoadForeignKeySourceColumns();
                if (form.ForeignKeySourceColumn != null && !string.IsNullOrEmpty(form.ForeignKeySourceColumn.Key))
                {
                    string sourceColumn = form.ForeignKeySourceColumn.Key;
                    for (int i = 0; i < form.kcmbForeignExcelSourceColumn.Items.Count; i++)
                    {
                        if (form.kcmbForeignExcelSourceColumn.Items[i].ToString() == sourceColumn ||
                            form.kcmbForeignExcelSourceColumn.Items[i].ToString().Contains($"({sourceColumn})"))
                        {
                            form.kcmbForeignExcelSourceColumn.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        public override IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form)
        {
            string selectedTable = form.kcmbRelatedTable.SelectedItem.ToString();
            int startIndex = selectedTable.IndexOf('(') + 1;
            int endIndex = selectedTable.IndexOf(')');
            string tableName = startIndex > 0 && endIndex > startIndex
                ? selectedTable.Substring(startIndex, endIndex - startIndex)
                : selectedTable;

            string displayName = form.ktxtRelatedField.SelectedItem?.ToString() ?? form.ktxtRelatedField.Text;
            var field = form.FieldInfoDict.FirstOrDefault(f => f.Value == displayName);

            string excelColumnName = string.Empty;
            string columnDisplayName = string.Empty;
            if (form.kcmbForeignExcelSourceColumn.SelectedIndex > 0)
            {
                string selectedColumnText = form.kcmbForeignExcelSourceColumn.SelectedItem.ToString();
                int columnStartIndex = selectedColumnText.LastIndexOf('(');
                int columnEndIndex = selectedColumnText.LastIndexOf(')');

                if (columnStartIndex > 0 && columnEndIndex > columnStartIndex)
                {
                    columnDisplayName = selectedColumnText.Substring(0, columnStartIndex).Trim();
                    excelColumnName = selectedColumnText.Substring(columnStartIndex + 1, columnEndIndex - columnStartIndex - 1);
                }
                else
                {
                    excelColumnName = selectedColumnText;
                    columnDisplayName = excelColumnName;
                }
            }

            return new ForeignKeyConfig
            {
                ForeignTableName = tableName,
                ForeignTableDisplayName = form.GetTableDisplayName(tableName),
                ForeignFieldName = field.Key ?? string.Empty,
                ForeignFieldDisplayName = field.Value ?? displayName,
                DisplayFieldName = excelColumnName,
                DisplayFieldDisplayName = columnDisplayName
            };
        }

        public override bool Validate(FrmColumnPropertyConfig form, out string errorMessage)
        {
            var config = SaveToConfig(form) as ForeignKeyConfig;
            return config?.Validate(out errorMessage) ?? false;
        }
    }

    #endregion

    #region 自身引用策略

    public class SelfReferenceConfigStrategy : DataSourceConfigStrategyBase
    {
        public override DataSourceType DataSourceType => DataSourceType.SelfReference;

        public override void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config)
        {
            var selfRefConfig = config as SelfReferenceConfig;
            if (selfRefConfig != null)
            {
                form.LoadSelfReferenceFields();
                form.kcmbSelfReferenceField.SelectedItem = selfRefConfig.ReferenceFieldDisplayName;
            }
        }

        public override IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form)
        {
            string displayName = form.kcmbSelfReferenceField.SelectedItem?.ToString() ?? form.kcmbSelfReferenceField.Text;
            var selfRefField = form.FieldInfoDict.FirstOrDefault(f => f.Value == displayName);

            return new SelfReferenceConfig
            {
                ReferenceFieldName = selfRefField.Key ?? string.Empty,
                ReferenceFieldDisplayName = selfRefField.Value ?? displayName
            };
        }

        public override bool Validate(FrmColumnPropertyConfig form, out string errorMessage)
        {
            var config = SaveToConfig(form) as SelfReferenceConfig;
            return config?.Validate(out errorMessage) ?? false;
        }
    }

    #endregion

    #region 字段复制策略

    public class FieldCopyConfigStrategy : DataSourceConfigStrategyBase
    {
        public override DataSourceType DataSourceType => DataSourceType.FieldCopy;

        public override void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config)
        {
            var copyConfig = config as FieldCopyConfig;
            if (copyConfig != null)
            {
                form.LoadCopyFromFields();
                form.kcmbCopyFromField.SelectedItem = copyConfig.SourceFieldDisplayName;
            }
        }

        public override IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form)
        {
            string displayName = form.kcmbCopyFromField.SelectedItem?.ToString() ?? form.kcmbCopyFromField.Text;
            var copyField = form.FieldInfoDict.FirstOrDefault(f => f.Value == displayName);

            return new FieldCopyConfig
            {
                SourceFieldName = copyField.Key ?? string.Empty,
                SourceFieldDisplayName = copyField.Value ?? displayName
            };
        }

        public override bool Validate(FrmColumnPropertyConfig form, out string errorMessage)
        {
            var config = SaveToConfig(form) as FieldCopyConfig;
            return config?.Validate(out errorMessage) ?? false;
        }
    }

    #endregion

    #region 列拼接策略

    public class ColumnConcatConfigStrategy : DataSourceConfigStrategyBase
    {
        public override DataSourceType DataSourceType => DataSourceType.ColumnConcat;

        public override void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config)
        {
            var concatConfig = config as ColumnConcatConfig;
            if (concatConfig != null)
            {
                form.ConcatConfig = concatConfig;
                form.LoadConcatSourceColumns();
                form.ktxtSeparator.Text = concatConfig.Separator ?? string.Empty;
                form.kchkTrimWhitespace.Checked = concatConfig.TrimWhitespace;
                form.kchkIgnoreEmptyColumns.Checked = concatConfig.IgnoreEmptyColumns;
            }
        }

        public override IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form)
        {
            var selectedColumns = form.klstSourceColumns.SelectedItems.Cast<string>().ToList();

            return new ColumnConcatConfig
            {
                SourceColumns = selectedColumns,
                Separator = form.ktxtSeparator.Text.Trim(),
                TrimWhitespace = form.kchkTrimWhitespace.Checked,
                IgnoreEmptyColumns = form.kchkIgnoreEmptyColumns.Checked
            };
        }

        public override bool Validate(FrmColumnPropertyConfig form, out string errorMessage)
        {
            var config = SaveToConfig(form) as ColumnConcatConfig;
            return config?.Validate(out errorMessage) ?? false;
        }
    }

    #endregion

    #region Excel图片策略

    public class ExcelImageConfigStrategy : DataSourceConfigStrategyBase
    {
        public override DataSourceType DataSourceType => DataSourceType.ExcelImage;

        public override void LoadFromConfig(FrmColumnPropertyConfig form, IDataSourceConfig config)
        {
            var imageConfig = config as ExcelImageConfig;
            if (imageConfig != null)
            {
                form.kcmbImageStorageType.SelectedIndex = (int)imageConfig.StorageType;
                form.kcmbImageNamingRule.SelectedIndex = (int)imageConfig.NamingRule;
                form.ktxtImageOutputDir.Text = imageConfig.OutputDirectory;
                form.kcmbImageNamingColumn.SelectedItem = imageConfig.NamingReferenceColumn;
            }
        }

        public override IDataSourceConfig SaveToConfig(FrmColumnPropertyConfig form)
        {
            return new ExcelImageConfig
            {
                StorageType = (ImageStorageType)(form.kcmbImageStorageType?.SelectedIndex ?? 0),
                NamingRule = (ImageNamingRule)(form.kcmbImageNamingRule?.SelectedIndex ?? 0),
                OutputDirectory = form.ktxtImageOutputDir?.Text ?? string.Empty,
                NamingReferenceColumn = form.kcmbImageNamingColumn?.SelectedItem?.ToString() ?? string.Empty
            };
        }

        public override bool Validate(FrmColumnPropertyConfig form, out string errorMessage)
        {
            var config = SaveToConfig(form) as ExcelImageConfig;
            return config?.Validate(out errorMessage) ?? false;
        }
    }

    #endregion

    /// <summary>
    /// 数据源配置策略管理器
    /// 负责注册和获取策略
    /// </summary>
    public static class DataSourceConfigStrategyManager
    {
        private static readonly Dictionary<DataSourceType, IDataSourceConfigStrategy> _strategies = new Dictionary<DataSourceType, IDataSourceConfigStrategy>();

        static DataSourceConfigStrategyManager()
        {
            // 注册所有策略
            RegisterStrategy(new ExcelConfigStrategy());
            RegisterStrategy(new DefaultValueConfigStrategy());
            RegisterStrategy(new SystemGeneratedConfigStrategy());
            RegisterStrategy(new ForeignKeyConfigStrategy());
            RegisterStrategy(new SelfReferenceConfigStrategy());
            RegisterStrategy(new FieldCopyConfigStrategy());
            RegisterStrategy(new ColumnConcatConfigStrategy());
            RegisterStrategy(new ExcelImageConfigStrategy());
        }

        /// <summary>
        /// 注册策略
        /// </summary>
        /// <param name="strategy">策略实例</param>
        public static void RegisterStrategy(IDataSourceConfigStrategy strategy)
        {
            if (!_strategies.ContainsKey(strategy.DataSourceType))
            {
                _strategies[strategy.DataSourceType] = strategy;
            }
        }

        /// <summary>
        /// 获取策略
        /// </summary>
        /// <param name="dataSourceType">数据源类型</param>
        /// <returns>策略实例，如果不存在返回null</returns>
        public static IDataSourceConfigStrategy GetStrategy(DataSourceType dataSourceType)
        {
            return _strategies.TryGetValue(dataSourceType, out var strategy) ? strategy : null;
        }

        /// <summary>
        /// 判断是否存在指定类型的策略
        /// </summary>
        /// <param name="dataSourceType">数据源类型</param>
        /// <returns>是否存在</returns>
        public static bool HasStrategy(DataSourceType dataSourceType)
        {
            return _strategies.ContainsKey(dataSourceType);
        }
    }
}