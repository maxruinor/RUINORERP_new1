using Krypton.Toolkit;
using RUINORERP.Business.Cache;
using RUINORERP.Common;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列属性配置对话框
    /// 用于配置列的各种属性，包括外键、唯一性、默认值、系统生成等
    /// </summary>
    public partial class FrmColumnPropertyConfig : KryptonForm
    {
        /// <summary>
        /// 当前映射配置
        /// </summary>
        public ColumnMapping CurrentMapping { get; set; }

        /// <summary>
        /// 目标实体类型
        /// </summary>
        public Type TargetEntityType { get; set; }

        /// <summary>
        /// 数据源配置（返回值）
        /// </summary>
        public DataSourceConfigBase DataSourceConfig { get; set; }

        /// <summary>
        /// Excel列名列表
        /// </summary>
        public List<string> ExcelColumns { get; set; }

        /// <summary>
        /// 字段信息字典（字段名 -> 中文名）
        /// </summary>
        private System.Collections.Concurrent.ConcurrentDictionary<string, string> _fieldInfoDict;

        /// <summary>
        /// 数据绑定辅助类
        /// </summary>
        private DataBindingHelper _dataBindingHelper;

        /// <summary>
        /// 数据库客户端
        /// </summary>
        private ISqlSugarClient _db;

        private readonly ITableSchemaManager _tableSchemaManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmColumnPropertyConfig()
        {
            InitializeComponent();

            // 设计时跳过初始化逻辑
            if (DesignMode)
            {
                return;
            }

            // ✅ 在设计时模式下,不要执行可能出错的初始化代码
            try
            {
                LoadRelatedTables();
                _dataBindingHelper = new DataBindingHelper();
                _tableSchemaManager = Startup.GetFromFac<ITableSchemaManager>();

                // ✅ 事件绑定已在 Designer.cs 中完成,不需要在此处重复绑定
                this.FormClosing += FrmColumnPropertyConfig_FormClosing;
            }
            catch (Exception ex)
            {
                // 设计时出错不影响设计器加载
                System.Diagnostics.Debug.WriteLine($"FrmColumnPropertyConfig 初始化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 带数据库客户端的构造函数
        /// </summary>
        public FrmColumnPropertyConfig(ISqlSugarClient db) : this()
        {
            _db = db;
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        private void FrmColumnPropertyConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 不需要清理动态控件，已移除相关逻辑
        }

        /// <summary>
        /// 加载关联表列表
        /// </summary>
        private void LoadRelatedTables()
        {
            try
            {
                // 添加支持的关联表
                kcmbRelatedTable.Items.Clear();
                kcmbRelatedTable.Items.Add("请选择");

                // 使用UCBasicDataImport中的EntityTypeMappings加载关联表
                if (UCBasicDataImport.EntityTypeMappings != null)
                {
                    // 去重后添加到下拉框
                    var uniqueTables = new Dictionary<string, Type>();
                    foreach (var mapping in UCBasicDataImport.EntityTypeMappings)
                    {
                        if (!uniqueTables.ContainsValue(mapping.Value))
                        {
                            uniqueTables.Add(mapping.Key, mapping.Value);
                        }
                    }

                    // 添加到下拉框
                    foreach (var table in uniqueTables)
                    {
                        kcmbRelatedTable.Items.Add($"{table.Key} ({table.Value.Name})");
                    }
                }
                else
                {
                    // 回退到硬编码方式
                    kcmbRelatedTable.Items.Add("供应商表 (tb_CustomerVendor)");
                    kcmbRelatedTable.Items.Add("产品类目表 (tb_ProdCategories)");
                    kcmbRelatedTable.Items.Add("产品基本信息表 (tb_Prod)");
                    kcmbRelatedTable.Items.Add("产品详情信息表 (tb_ProdDetail)");
                    kcmbRelatedTable.Items.Add("产品属性表 (tb_ProdProperty)");
                    kcmbRelatedTable.Items.Add("产品属性值表 (tb_ProdPropertyValue)");
                }

                kcmbRelatedTable.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载关联表列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
 

        /// <summary>
        /// 窗体加载事件（参考 UCSaleOrder.cs 的 BindData 方法）
        /// </summary>
        private void FrmColumnPropertyConfig_Load(object sender, EventArgs e)
        {
            if (CurrentMapping == null) return;

            // ✅ 确保配置对象存在且类型匹配
            EnsureDataSourceConfigExists();

            // 1. 绑定业务键相关控件
            DataBindingHelper.BindData4CheckBox<ColumnMapping>(CurrentMapping, m => m.IsBusinessKey, kchkIsBusinessKey, false);
            DataBindingHelper.BindData4CheckBox<ColumnMapping>(CurrentMapping, m => m.IsUniqueValue, kchkIsUniqueValue, false);
            // 2. 绑定数据来源类型
            DataBindingHelper.BindData4CmbByEnum<ColumnMapping>(CurrentMapping, m => m.ColumnDataSourceType, typeof(DataSourceType), kcmbDataSourceType, false);

            // 3. 根据 ColumnDataSourceType 切换到对应的 Tab 页
            kryptonTabControl.SelectedIndex = CurrentMapping.ColumnDataSourceType;

            // 4. 绑定数据源配置对象到控件（双向绑定）
            BindDataSourceConfigByType(CurrentMapping.ColumnDataSourceType);

            // 5. 加载下拉框数据（如果需要）
            LoadDropdownDataIfNeeded(CurrentMapping.ColumnDataSourceType);

            // 6. 更新控件状态
            UpdateControlStates();
        }

        /// <summary>
        /// 确保数据源配置对象存在且类型匹配
        /// </summary>
        private void EnsureDataSourceConfigExists()
        {
            var targetType = (DataSourceType)CurrentMapping.ColumnDataSourceType;
            
            if (CurrentMapping.DataSourceConfig == null || !IsConfigTypeMatch(CurrentMapping.DataSourceConfig, targetType))
            {
                CurrentMapping.DataSourceConfig = CreateDefaultConfig(targetType);
            }
        }

        /// <summary>
        /// 检查配置对象类型是否匹配
        /// </summary>
        private bool IsConfigTypeMatch(DataSourceConfigBase config, DataSourceType targetType)
        {
            return targetType switch
            {
                DataSourceType.Excel => config is ExcelConfig,
                DataSourceType.DefaultFixedValue => config is DefaultValueConfig,
                DataSourceType.SystemGenerated => config is SystemGeneratedConfig,
                DataSourceType.ForeignKey => config is DatabaseReferenceConfig,
                DataSourceType.ColumnConcat => config is ColumnConcatConfig,
                DataSourceType.ExcelImage => config is ExcelImageConfig,
                _ => false
            };
        }

        /// <summary>
        /// 创建默认配置对象
        /// </summary>
        private DataSourceConfigBase CreateDefaultConfig(DataSourceType type)
        {
            return type switch
            {
                DataSourceType.Excel => new ExcelConfig(),
                DataSourceType.DefaultFixedValue => new DefaultValueConfig(),
                DataSourceType.SystemGenerated => new SystemGeneratedConfig(),
                DataSourceType.ForeignKey => new DatabaseReferenceConfig(),
                DataSourceType.ColumnConcat => new ColumnConcatConfig(),
                DataSourceType.ExcelImage => new ExcelImageConfig(),
                _ => new ExcelConfig()
            };
        }

        /// <summary>
        /// 根据数据源类型绑定配置对象
        /// </summary>
        private void BindDataSourceConfigByType(int dataSourceType)
        {
            if (CurrentMapping.DataSourceConfig == null) return;

            switch (dataSourceType)
            {
                case (int)DataSourceType.Excel:
                    BindExcelConfig(CurrentMapping.DataSourceConfig as ExcelConfig);
                    break;
                case (int)DataSourceType.DefaultFixedValue:
                    BindDefaultValueConfig(CurrentMapping.DataSourceConfig as DefaultValueConfig);
                    break;
                case (int)DataSourceType.SystemGenerated:
                    BindSystemGeneratedConfig(CurrentMapping.DataSourceConfig as SystemGeneratedConfig);
                    break;
                case (int)DataSourceType.ForeignKey:
                    BindDatabaseReferenceConfig(CurrentMapping.DataSourceConfig as DatabaseReferenceConfig);
                    break;
                case (int)DataSourceType.ColumnConcat:
                    BindColumnConcatConfig(CurrentMapping.DataSourceConfig as ColumnConcatConfig);
                    break;
                case (int)DataSourceType.ExcelImage:
                    BindExcelImageConfig(CurrentMapping.DataSourceConfig as ExcelImageConfig);
                    break;
            }
        }

        /// <summary>
        /// 根据数据源类型加载下拉框数据
        /// </summary>
        private void LoadDropdownDataIfNeeded(int dataSourceType)
        {
            switch (dataSourceType)
            {
                case (int)DataSourceType.ForeignKey:
                    // 数据库表关联引用：加载外键来源列
                    LoadForeignKeySourceColumns();
                    break;
                case (int)DataSourceType.ColumnConcat:
                    LoadConcatSourceColumns();
                    break;
                case (int)DataSourceType.ExcelImage:
                    LoadImageNamingColumns();
                    break;
            }
        }


        /// <summary>
        /// 绑定 Excel 配置
        /// </summary>
        private void BindExcelConfig(ExcelConfig config)
        {
            if (config == null) return;

            // 使用 DataBindingHelper 进行双向绑定
            DataBindingHelper.BindData4CheckBox<ExcelConfig>(
                config,
                c => c.IgnoreEmptyValue,
                kchkIgnoreEmptyValue,
                false);

            // 绑定空值默认值文本框
            DataBindingHelper.BindData4TextBox<ExcelConfig>(
                config,
                c => c.EmptyValueDefault,
                txtExcelDefaultValue,
                BindDataType4TextBox.Text,
                false);
        }

        /// <summary>
        /// 忽略空值复选框状态改变事件
        /// 实现与空值默认值的二选一互斥逻辑
        /// </summary>
        private void kchkIgnoreEmptyValue_CheckedChanged(object sender, EventArgs e)
        {
            if (kchkIgnoreEmptyValue.Checked)
            {
                // 如果勾选了忽略空值，则清空空值默认值
                txtExcelDefaultValue.Text = string.Empty;
            }
        }

        /// <summary>
        /// 空值默认值文本框文本改变事件
        /// 实现与忽略空值的二选一互斥逻辑
        /// </summary>
        private void txtExcelDefaultValue_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtExcelDefaultValue.Text))
            {
                // 如果输入了空值默认值，则取消勾选忽略空值
                kchkIgnoreEmptyValue.Checked = false;
            }
        }

        /// <summary>
        /// 绑定默认固定值配置
        /// 只需要一个简单的文本框输入字符串值，后台会根据目标列类型自动转换
        /// 
        /// 注意：Designer.cs 中保留了其他动态控件（cmbDynamicDefaultList、cmbDynamicDefaultEnum等）的定义，
        /// 但它们都设置为 Visible = false，不再使用。这是为了保持设计器兼容性。
        /// </summary>
        private void BindDefaultValueConfig(DefaultValueConfig config)
        {
            if (config == null) return;

            // 使用 DataBindingHelper 进行双向绑定
            DataBindingHelper.BindData4TextBox<DefaultValueConfig>(
                config,
                c => c.Value,
                ktxtDefaultValue,
                BindDataType4TextBox.Text,
                false);
        }

        /// <summary>
        /// 绑定系统生成配置（参考 UCSystemConfigEdit.cs 第105-126行）
        /// </summary>
        private void BindSystemGeneratedConfig(SystemGeneratedConfig config)
        {
            if (config == null) return;

            DataBindingHelper.BindData4CmbByEnum<SystemGeneratedConfig>(
                config,
                c => (int)c.GeneratedType,
                typeof(SystemGeneratedType),
                kcmbSystemGeneratedType,
                false);

            DataBindingHelper.BindData4TextBox<SystemGeneratedConfig>(
                config,
                c => c.DateTimeFormat,
                ktxtDateTimeFormat,
                BindDataType4TextBox.Text,
                false);

            DataBindingHelper.BindData4TextBox<SystemGeneratedConfig>(
                config,
                c => c.BusinessCodePrefix,
                ktxtBusinessCodePrefix,
                BindDataType4TextBox.Text,
                false);

            DataBindingHelper.BindData4CmbByEnum<SystemGeneratedConfig>(
                config,
                c => (int)c.BusinessCodeRule,
                typeof(BusinessCodeRule),
                kcmbBusinessCodeRule,
                false);

            DataBindingHelper.BindData4TextBox<SystemGeneratedConfig>(
                config,
                c => c.SequenceDigits,
                ktxtSequenceDigits,
                BindDataType4TextBox.Qty,
                false);

            DataBindingHelper.BindData4TextBox<SystemGeneratedConfig>(
                config,
                c => c.CustomExpression,
                ktxtCustomExpression,
                BindDataType4TextBox.Text,
                false);

            DataBindingHelper.BindData4TextBox<SystemGeneratedConfig>(
                config,
                c => c.CustomDefaultValue,
                ktxtCustomDefaultValue,
                BindDataType4TextBox.Text,
                false);

            // ✅ 绑定实体业务编号类型（ComboBox SelectedItem 绑定）
            BindComboBoxSelectedItem(config, nameof(SystemGeneratedConfig.EntityBizCodeType), kcmbEntityBizCodeType);
        }

        /// <summary>
        /// 绑定数据库表关联引用配置
        /// 支持两种模式：外键关联和自身表引用
        /// </summary>
        private void BindDatabaseReferenceConfig(DatabaseReferenceConfig config)
        {
            if (config == null) return;

            // 1. 绑定自身表引用复选框
            DataBindingHelper.BindData4CheckBox<DatabaseReferenceConfig>(
                config,
                c => c.IsSelfReference,
                chkIsSelfReference,
                false);

            // 2. 加载关联表列表
            LoadRelatedTables();

            // 3. 如果是自身表引用，自动选中目标表
            if (config.IsSelfReference)
            {
                config.ForeignTableName = TargetEntityType?.Name;
                config.ForeignTableDisplayName = GetTargetTableDisplayName();
                kcmbRelatedTable.Enabled = false;
            }

            // 4. 选中对应的关联表
            if (!string.IsNullOrEmpty(config.ForeignTableName))
            {
                for (int i = 0; i < kcmbRelatedTable.Items.Count; i++)
                {
                    string itemText = kcmbRelatedTable.Items[i].ToString();
                    if (itemText.Contains(config.ForeignTableName))
                    {
                        kcmbRelatedTable.SelectedIndex = i;
                        break;
                    }
                }

                // 加载关联表的字段
                LoadTableFields(config.ForeignTableName);
            }

            // 5. 绑定关联字段显示名称
            if (!string.IsNullOrEmpty(config.ForeignFieldDisplayName))
            {
                ktxtRelatedField.SelectedItem = config.ForeignFieldDisplayName;
            }

            // 6. 加载外键来源列并选中已配置的值
            string foreignKeySourceColumn = config.ForeignKeySourceColumn?.Key;
            LoadForeignKeySourceColumns(foreignKeySourceColumn);
        }

        /// <summary>
        /// 获取目标表的显示名称
        /// </summary>
        private string GetTargetTableDisplayName()
        {
            if (TargetEntityType == null) return string.Empty;
            
            // 从 FieldNameList 中查找目标表的中文名
            foreach (var kvp in _fieldInfoDict)
            {
                if (kvp.Key == TargetEntityType.Name)
                {
                    return kvp.Value;
                }
            }
            
            return TargetEntityType.Name;
        }

        /// <summary>
        /// 加载数据库表关联引用的字段列表
        /// </summary>
        private void LoadDatabaseReferenceFields(DatabaseReferenceConfig config)
        {
            if (config == null) return;

            string tableName = config.IsSelfReference 
                ? TargetEntityType?.Name 
                : config.ForeignTableName;

            if (!string.IsNullOrEmpty(tableName))
            {
                LoadTableFields(tableName);
            }
        }

        /// <summary>
        /// 绑定 ComboBox SelectedItem 到字符串属性（通用方法）
        /// </summary>
        private void BindComboBoxSelectedItem(object config, string propertyName, KryptonComboBox comboBox)
        {
            comboBox.DataBindings.Clear();
            var binding = new Binding("SelectedItem", config, propertyName, 
                false, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (s, args) => args.Value = args.Value ?? string.Empty;
            binding.Parse += (s, args) => args.Value = args.Value ?? string.Empty;
            comboBox.DataBindings.Add(binding);
        }

        /// <summary>
        /// 绑定列拼接配置
        /// </summary>
        private void BindColumnConcatConfig(ColumnConcatConfig config)
        {
            if (config == null) return;

            // 加载 Excel 列列表
            LoadConcatSourceColumns();

            // 选中已配置的列（ListBox 无法直接绑定，需要手动设置）
            if (config.ConcatColumns != null)
            {
                for (int i = 0; i < klstSourceColumns.Items.Count; i++)
                {
                    string columnName = klstSourceColumns.Items[i].ToString();
                    if (config.ConcatColumns.Any(c => c.Key == columnName))
                    {
                        klstSourceColumns.SetSelected(i, true);
                    }
                }
            }

            // 使用 DataBindingHelper 进行双向绑定
            DataBindingHelper.BindData4TextBox<ColumnConcatConfig>(
                config,
                c => c.Separator,
                ktxtSeparator,
                BindDataType4TextBox.Text,
                false);

            DataBindingHelper.BindData4CheckBox<ColumnConcatConfig>(
                config,
                c => c.TrimWhitespace,
                kchkTrimWhitespace,
                false);

            DataBindingHelper.BindData4CheckBox<ColumnConcatConfig>(
                config,
                c => c.IgnoreEmptyColumns,
                kchkIgnoreEmptyColumns,
                false);
        }

        /// <summary>
        /// 绑定 Excel 图片配置
        /// </summary>
        private void BindExcelImageConfig(ExcelImageConfig config)
        {
            if (config == null) return;

            DataBindingHelper.BindData4CmbByEnum<ExcelImageConfig>(
                config,
                c => (int)c.StorageType,
                typeof(ImageStorageType),
                kcmbImageStorageType,
                false);

            DataBindingHelper.BindData4CmbByEnum<ExcelImageConfig>(
                config,
                c => (int)c.NamingRule,
                typeof(ImageNamingRule),
                kcmbImageNamingRule,
                false);

            DataBindingHelper.BindData4TextBox<ExcelImageConfig>(
                config,
                c => c.OutputDirectory,
                ktxtImageOutputDir,
                BindDataType4TextBox.Text,
                false);

            // 双向绑定到命名引用列
            BindComboBoxSelectedItem(config, nameof(ExcelImageConfig.NamingReferenceColumn), kcmbImageNamingColumn);
        }

        /// <summary>
        /// 更新控件状态
        /// </summary>
        private void UpdateControlStates()
        {
            // ✅ 安全检查:确保 SelectedIndex 有效
            if (kcmbDataSourceType == null || kcmbDataSourceType.SelectedIndex < 0)
            {
                return;
            }

            DataSourceType dataSourceType = (DataSourceType)kcmbDataSourceType.SelectedIndex;

            // 根据数据来源类型控制控件状态
            kcmbRelatedTable.Enabled = (dataSourceType == DataSourceType.ForeignKey);
            ktxtRelatedField.Enabled = (dataSourceType == DataSourceType.ForeignKey);
            kcmbForeignExcelSourceColumn.Enabled = (dataSourceType == DataSourceType.ForeignKey);
         

            // 根据数据来源类型设置相关标志（不再需要，直接使用 DataSourceType 判断）
            // IsForeignKey = (dataSourceType == DataSourceType.ForeignKey);
            // IsSystemGenerated = (dataSourceType == DataSourceType.SystemGenerated);

            // 控制GroupBox的显示和隐藏
            kryptonGroupBoxForeignType.Visible = (dataSourceType == DataSourceType.ForeignKey);
            kryptonGroupBoxConcat.Visible = (dataSourceType == DataSourceType.ColumnConcat);

            // ✅ 控制图片配置GroupBox的显示和隐藏（选择ExcelImage类型时自动显示）
            kryptonGroupBoxImageType.Visible = (dataSourceType == DataSourceType.ExcelImage);

            // 控制系统生成配置GroupBox的显示和隐藏
            kryptonGroupBoxSystemGenerated.Visible = (dataSourceType == DataSourceType.SystemGenerated);

            // 更新系统生成配置控件的可用性
            UpdateSystemGeneratedControlStates();

            // 处理默认固定值控件：始终显示文本框，不需要动态生成
            // 用户只需在文本框中输入字符串值，后台会根据目标列类型自动转换
        }

        /// <summary>
        /// 关联表选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kcmbRelatedTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbRelatedTable.SelectedIndex <= 0)
            {
                ktxtRelatedField.Items.Clear();
                return;
            }

            // 获取选中的表名
            string selectedTable = kcmbRelatedTable.SelectedItem.ToString();
            string tableName = string.Empty;

            // 提取表名（从括号中）
            int startIndex = selectedTable.IndexOf('(') + 1;
            int endIndex = selectedTable.IndexOf(')');
            if (startIndex > 0 && endIndex > startIndex)
            {
                tableName = selectedTable.Substring(startIndex, endIndex - startIndex);
            }

            // 加载表的字段列表
            LoadTableFields(tableName);
        }

        /// <summary>
        /// 自身表引用复选框状态改变事件
        /// </summary>
        private void chkIsSelfReference_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentMapping?.DataSourceConfig is DatabaseReferenceConfig config)
            {
                config.IsSelfReference = chkIsSelfReference.Checked;

                if (chkIsSelfReference.Checked)
                {
                    // 勾选时，自动设置为目标表并禁用下拉框
                    config.ForeignTableName = TargetEntityType?.Name;
                    config.ForeignTableDisplayName = GetTargetTableDisplayName();
                    kcmbRelatedTable.Enabled = false;
                    
                    // 加载目标表的字段
                    LoadTableFields(config.ForeignTableName);
                }
                else
                {
                    // 取消勾选时，启用下拉框并清空已选值
                    kcmbRelatedTable.Enabled = true;
                    kcmbRelatedTable.SelectedIndex = 0;
                    ktxtRelatedField.Items.Clear();
                    
                    config.ForeignTableName = null;
                    config.ForeignTableDisplayName = null;
                    config.ForeignFieldName = null;
                    config.ForeignFieldDisplayName = null;
                }
            }
        }

        /// <summary>
        /// 加载外键来源列（Excel列）
        /// </summary>
        /// <param name="selectedValue">需要选中的值（可选）</param>
        private void LoadForeignKeySourceColumns(string selectedValue = null)
        {
            try
            {
                kcmbForeignExcelSourceColumn.Items.Clear();
                kcmbForeignExcelSourceColumn.Items.Add("请选择Excel列（可选）");

                // 如果有传入Excel列列表，则加载
                if (ExcelColumns != null && ExcelColumns.Count > 0)
                {
                    foreach (var column in ExcelColumns)
                    {
                        kcmbForeignExcelSourceColumn.Items.Add(column);
                    }
                }
                else if (CurrentMapping != null && !string.IsNullOrEmpty(CurrentMapping.OriginalExcelColumn))
                {
                    // 如果没有传入Excel列列表，但当前映射有Excel列，则使用该列
                    // 并检查是否是特殊标记的列
                    if (!CurrentMapping.OriginalExcelColumn.StartsWith("[") && !CurrentMapping.OriginalExcelColumn.StartsWith("("))
                    {
                        kcmbForeignExcelSourceColumn.Items.Add(CurrentMapping.OriginalExcelColumn);
                    }
                }

                // 如果有指定需要选中的值，则选中它
                if (!string.IsNullOrEmpty(selectedValue))
                {
                    for (int i = 0; i < kcmbForeignExcelSourceColumn.Items.Count; i++)
                    {
                        if (kcmbForeignExcelSourceColumn.Items[i].ToString() == selectedValue)
                        {
                            kcmbForeignExcelSourceColumn.SelectedIndex = i;
                            return;
                        }
                    }
                }

                // 否则选择第一项
                kcmbForeignExcelSourceColumn.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载外键来源列失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载表的字段列表
        /// </summary>
        /// <param name="tableName">表名</param>
        private void LoadTableFields(string tableName)
        {
            try
            {
                Type tableType = null;

                // 优先使用UCBasicDataImport中的EntityTypeMappings
                if (UCBasicDataImport.EntityTypeMappings != null)
                {
                    // 查找对应的中文描述
                    foreach (var mapping in UCBasicDataImport.EntityTypeMappings)
                    {
                        if (mapping.Value.Name == tableName)
                        {
                            tableType = mapping.Value;
                            break;
                        }
                    }
                }

                // 如果在EntityTypeMappings中找不到,使用原来的GetTableType方法
                if (tableType == null)
                {
                    tableType = GetTableType(tableName);
                }

                if (tableType == null)
                {
                    MessageBox.Show($"无法找到表类型: {tableName}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 获取字段信息
                var fieldNameList = UIHelper.GetFieldNameList(true, tableType);
                _fieldInfoDict = fieldNameList;

                // ✅ 先清空下拉框,避免重复添加
                ktxtRelatedField.Items.Clear();
                kcmbForeignDbSourceColumn.Items.Clear();

                // ✅ 使用HashSet去重,防止字段名重复
                var addedFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var field in fieldNameList)
                {
                    // 只有未添加过的字段才添加
                    if (!addedFields.Contains(field.Value))
                    {
                        ktxtRelatedField.Items.Add(field.Value); // 添加中文名
                        kcmbForeignDbSourceColumn.Items.Add(field.Value); // 添加中文名
                        addedFields.Add(field.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表字段失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据表名获取类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表类型</returns>
        private Type GetTableType(string tableName)
        {
            // 优先使用UCBasicDataImport中的EntityTypeMappings
            if (UCBasicDataImport.EntityTypeMappings != null)
            {
                foreach (var mapping in UCBasicDataImport.EntityTypeMappings)
                {
                    if (mapping.Value.Name == tableName)
                    {
                        return mapping.Value;
                    }
                }
            }

            // 回退到原来的Type.GetType方法
            try
            {
                return Type.GetType($"RUINORERP.Model.{tableName}");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 关联表字段选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void ktxtRelatedField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ktxtRelatedField.SelectedIndex < 0 || _fieldInfoDict == null)
            {
                return;
            }

            // 根据中文显示名称获取实际字段名
            string displayName = ktxtRelatedField.SelectedItem.ToString();
            var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
            if (field.Value != null)
            {
                // 直接更新配置对象
                if (CurrentMapping?.DataSourceConfig is DatabaseReferenceConfig config)
                {
                    config.ForeignFieldName = field.Key;
                    config.ForeignFieldDisplayName = field.Value;
                }
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void kbtnOK_Click(object sender, EventArgs e)
        {
            // ✅ 确保配置对象存在
            EnsureDataSourceConfigExists();

            DataSourceType dataSourceType = (DataSourceType)kcmbDataSourceType.SelectedIndex;

            // ✅ 同步ListBox选择状态到配置（列拼接）
            if (dataSourceType == DataSourceType.ColumnConcat && CurrentMapping.DataSourceConfig is ColumnConcatConfig concatConfig)
            {
                concatConfig.ConcatColumns.Clear();
                foreach (int index in klstSourceColumns.SelectedIndices)
                {
                    string columnName = klstSourceColumns.Items[index].ToString();
                    concatConfig.ConcatColumns.Add(new SerializableKeyValuePair<string>
                    {
                        Key = columnName,
                        Value = columnName
                    });
                }
            }

            // ✅ 使用验证适配器进行配置验证
            var validator = new ImportValidationAdapter();
            if (!validator.ValidateColumnMapping(CurrentMapping, out List<string> validationErrors, TargetEntityType))
            {
                string errorMsg = "列配置验证失败：\n" + string.Join("\n", validationErrors);
                MessageBox.Show(errorMsg, "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ 直接使用配置对象（双向绑定已自动同步控件值到配置对象）
            DataSourceConfig = CurrentMapping.DataSourceConfig;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        /// <summary>
        /// 是否为外键复选框点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kchkIsForeignKey_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        /// <summary>
        /// 是否系统生成复选框点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kchkIsSystemGenerated_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        /// <summary>
        /// 更新系统生成配置控件的状态
        /// 根据选中的生成类型显示/隐藏相关配置项
        /// </summary>
        private void UpdateSystemGeneratedControlStates()
        {
            if (kcmbSystemGeneratedType == null || kcmbBusinessCodeRule == null) return;

            // ✅ 安全检查:确保 SelectedIndex 有效
            if (kcmbSystemGeneratedType.SelectedIndex < 0)
            {
                // 如果未选择任何项,隐藏所有配置控件
                kryptonLabel22.Visible = false;
                ktxtDateTimeFormat.Visible = false;
                kryptonLabel23.Visible = false;
                kcmbBusinessCodeRule.Visible = false;
                kryptonLabel24.Visible = false;
                ktxtBusinessCodePrefix.Visible = false;
                kryptonLabel25.Visible = false;
                ktxtSequenceDigits.Visible = false;
                kryptonLabel26.Visible = false;
                ktxtCustomDefaultValue.Visible = false;
                kryptonLabel27.Visible = false;
                ktxtCustomExpression.Visible = false;
                return;
            }

            SystemGeneratedType generatedType = (SystemGeneratedType)kcmbSystemGeneratedType.SelectedIndex;

            // 时间格式配置
            kryptonLabel22.Visible = (generatedType == SystemGeneratedType.DateTime ||
                                      generatedType == SystemGeneratedType.Date);
            ktxtDateTimeFormat.Visible = kryptonLabel22.Visible;

            // 业务编码相关配置
            kryptonLabel23.Visible = (generatedType == SystemGeneratedType.BusinessCode);
            kcmbBusinessCodeRule.Visible = kryptonLabel23.Visible;
            
            // ✅ 安全检查:确保 BusinessCodeRule 的 SelectedIndex 有效
            bool shouldShowPrefix = false;
            if (kcmbBusinessCodeRule.SelectedIndex >= 0)
            {
                shouldShowPrefix = (kcmbBusinessCodeRule.SelectedIndex == (int)BusinessCodeRule.PrefixDateSequence ||
                                   kcmbBusinessCodeRule.SelectedIndex == (int)BusinessCodeRule.PrefixSequence);
            }
            kryptonLabel24.Visible = (generatedType == SystemGeneratedType.BusinessCode && shouldShowPrefix);
            ktxtBusinessCodePrefix.Visible = kryptonLabel24.Visible;

            // 序号位数配置
            kryptonLabel25.Visible = (generatedType == SystemGeneratedType.BusinessCode ||
                                      generatedType == SystemGeneratedType.Sequence);
            ktxtSequenceDigits.Visible = kryptonLabel25.Visible;

            // 自定义默认值配置
            kryptonLabel26.Visible = (generatedType == SystemGeneratedType.Status ||
                                      generatedType == SystemGeneratedType.IsDeleted);
            ktxtCustomDefaultValue.Visible = kryptonLabel26.Visible;

            // 自定义表达式配置
            kryptonLabel27.Visible = (generatedType == SystemGeneratedType.CustomExpression);
            ktxtCustomExpression.Visible = kryptonLabel27.Visible;
        }

        /// <summary>
        /// 系统生成类型选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kcmbSystemGeneratedType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSystemGeneratedControlStates();
        }


        /// <summary>
        /// 数据来源类型选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kcmbDataSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbDataSourceType.SelectedIndex < 0) return;

            var newType = (DataSourceType)kcmbDataSourceType.SelectedIndex;

            // ✅ 创建新的配置对象（如果类型不匹配）
            if (CurrentMapping.DataSourceConfig == null || 
                !IsConfigTypeMatch(CurrentMapping.DataSourceConfig, newType))
            {
                // 保存旧配置（如果需要迁移数据）
                var oldConfig = CurrentMapping.DataSourceConfig;
                
                // 创建新类型的默认配置
                CurrentMapping.DataSourceConfig = CreateDefaultConfig(newType);
                
                // ✅ 尝试迁移相关数据（如Excel列名）
                MigrateConfigData(oldConfig, CurrentMapping.DataSourceConfig, newType);
            }

            // ✅ 重新绑定配置到控件
            BindDataSourceConfigByType(kcmbDataSourceType.SelectedIndex);
            
            // 切换Tab页
            if (kcmbDataSourceType.SelectedIndex >= 0 && kcmbDataSourceType.SelectedIndex < kryptonTabControl.TabCount)
            {
                kryptonTabControl.SelectedIndex = kcmbDataSourceType.SelectedIndex;
            }

            // 加载下拉框数据
            LoadDropdownDataIfNeeded(kcmbDataSourceType.SelectedIndex);
            
            UpdateControlStates();
        }

        /// <summary>
        /// 迁移配置数据（在切换数据源类型时保留相关信息）
        /// </summary>
        private void MigrateConfigData(DataSourceConfigBase oldConfig, DataSourceConfigBase newConfig, DataSourceType newType)
        {
            if (oldConfig == null || newConfig == null) return;

            // 如果旧配置是Excel配置，尝试保留Excel列名
            if (oldConfig is ExcelConfig excelConfig)
            {
                switch (newType)
                {
                    case DataSourceType.ForeignKey:
                        // 迁移到外键配置时，保留Excel来源列
                        if (newConfig is DatabaseReferenceConfig fkConfig && 
                            !string.IsNullOrEmpty(excelConfig.ExcelColumn))
                        {
                            fkConfig.ForeignKeySourceColumn = new SerializableKeyValuePair<string>
                            {
                                Key = excelConfig.ExcelColumn,
                                Value = excelConfig.ExcelColumn
                            };
                        }
                        break;
                        
                    case DataSourceType.ColumnConcat:
                        // 迁移到列拼接时，将原列作为第一个拼接列
                        if (newConfig is ColumnConcatConfig concatConfig && 
                            !string.IsNullOrEmpty(excelConfig.ExcelColumn))
                        {
                            concatConfig.ConcatColumns.Add(new SerializableKeyValuePair<string>
                            {
                                Key = excelConfig.ExcelColumn,
                                Value = excelConfig.ExcelColumn
                            });
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 加载图片命名参考列
        /// </summary>
        private void LoadImageNamingColumns()
        {
            try
            {
                if (kcmbImageNamingColumn == null) return;

                kcmbImageNamingColumn.Items.Clear();
                kcmbImageNamingColumn.Items.Add("使用自动递增命名");

                // 加载Excel列作为命名参考
                if (ExcelColumns != null && ExcelColumns.Count > 0)
                {
                    foreach (var column in ExcelColumns)
                    {
                        kcmbImageNamingColumn.Items.Add(column);
                    }
                }

                kcmbImageNamingColumn.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载图片命名列失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载自身引用字段列表
        /// </summary>
        private void LoadSelfReferenceFields()
        {
            try
            {
                if (TargetEntityType == null)
                {
                    MessageBox.Show("未设置目标实体类型", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 获取字段信息
                var fieldNameList = UIHelper.GetFieldNameList(false, TargetEntityType);
                _fieldInfoDict = fieldNameList;
 

                // ✅ 使用HashSet去重,防止字段名重复
                var addedFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var field in fieldNameList)
                {
                    // 只有未添加过的字段才添加
                    if (!addedFields.Contains(field.Value))
                    {
                        addedFields.Add(field.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载自身引用字段失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     



      

        /// <summary>
        /// 获取表的中文显示名称
        /// 使用UCBasicDataImport.EntityTypeMappings获取强类型映射
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>中文显示名称</returns>
        private string GetTableDisplayName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return string.Empty;

            // 使用UCBasicDataImport中的EntityTypeMappings获取表的中文显示名称
            if (UCBasicDataImport.EntityTypeMappings != null)
            {
                foreach (var mapping in UCBasicDataImport.EntityTypeMappings)
                {
                    if (mapping.Value.Name == tableName)
                    {
                        return mapping.Key;
                    }
                }
            }

            // 回退到表名本身
            return tableName;
        }

        /// <summary>
        /// 外键数据库字段选择改变事件
        /// 注意：显示名称已通过双向绑定自动同步，这里只需要同步英文字段名
        /// </summary>
        private void kcmbForeignDbSourceColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbForeignDbSourceColumn.SelectedIndex < 0 || _fieldInfoDict == null)
            {
                return;
            }

            string displayName = kcmbForeignDbSourceColumn.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(displayName)) return;

            var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
            if (field.Value != null && CurrentMapping?.DataSourceConfig is DatabaseReferenceConfig config)
            {
                config.ForeignKeySourceColumn = new SerializableKeyValuePair<string>
                {
                    Key = config.ForeignKeySourceColumn?.Key ?? string.Empty,
                    Value = field.Key
                };
            }
        }

        /// <summary>
        /// 加载列拼接的Excel列列表
        /// </summary>
        private void LoadConcatSourceColumns()
        {
            try
            {
                klstSourceColumns.Items.Clear();

                // 如果有传入Excel列列表，则加载所有可用列
                if (ExcelColumns != null && ExcelColumns.Count > 0)
                {
                    foreach (var column in ExcelColumns)
                    {
                        klstSourceColumns.Items.Add(column);
                    }
                }
                else if (CurrentMapping != null && !string.IsNullOrEmpty(CurrentMapping.OriginalExcelColumn))
                {
                    // 如果没有传入Excel列列表，但当前映射有Excel列，则使用该列
                    if (!CurrentMapping.OriginalExcelColumn.StartsWith("[") && !CurrentMapping.OriginalExcelColumn.StartsWith("("))
                    {
                        klstSourceColumns.Items.Add(CurrentMapping.OriginalExcelColumn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载列拼接源列失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 图片列复选框变更事件（已废弃，保留以防Designer引用）
        /// ✅ 当数据源类型为ExcelImage时，自动显示图片配置GroupBox
        /// </summary>
        private void kchkIsImageColumn_CheckedChanged(object sender, EventArgs e)
        {
            // ✅ 此方法已废弃，图片配置的显示由数据源类型控制
            // UpdateControlStates();
        }
    }
}
