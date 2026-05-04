using Krypton.Toolkit;
using RUINORERP.Business.Cache;
using RUINORERP.Common;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.ImportEngine.Enums;
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
        public IDataSourceConfig DataSourceConfig { get; set; }

        /// <summary>
        /// Excel列名列表
        /// </summary>
        public List<string> ExcelColumns { get; set; }

        /// <summary>
        /// 字段信息字典（字段名 -> 中文名）
        /// </summary>
        private System.Collections.Concurrent.ConcurrentDictionary<string, string> _fieldInfoDict;



        /// <summary>
        /// 动态生成的默认值控件
        /// </summary>
        private Control _dynamicDefaultValueControl;

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

            LoadRelatedTables();
            _dataBindingHelper = new DataBindingHelper();
            _tableSchemaManager = Startup.GetFromFac<ITableSchemaManager>();

            // 手动绑定事件
            //kcmbDataSourceType.SelectedIndexChanged += kcmbDataSourceType_SelectedIndexChanged;
            //kcmbSelfReferenceField.SelectedIndexChanged += kcmbSelfReferenceField_SelectedIndexChanged;
            //kcmbCopyFromField.SelectedIndexChanged += kcmbCopyFromField_SelectedIndexChanged;
            this.FormClosing += FrmColumnPropertyConfig_FormClosing;
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
            // 清理动态控件
            RemoveDefaultValueControl();
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

            // 1. 绑定业务键相关控件
            DataBindingHelper.BindData4CheckBox<ColumnMapping>(CurrentMapping, m => m.IsBusinessKey, kchkIsBusinessKey, false);

            // 2. 绑定数据来源类型
            DataBindingHelper.BindData4CmbByEnum<ColumnMapping>(CurrentMapping, m => (int)m.ColumnDataSourceType, typeof(DataSourceType), kcmbDataSourceType, false);

            // 3. 根据 DataSourceType 切换到对应的 Tab 页
            kryptonTabControl.SelectedIndex = (int)CurrentMapping.ColumnDataSourceType;

            // 4. 绑定数据源配置对象到控件（双向绑定）
            if (CurrentMapping.DataSourceConfig != null)
            {
                switch (CurrentMapping.ColumnDataSourceType)
                {
                    case (int)DataSourceType.Excel:
                        BindExcelConfig(CurrentMapping.DataSourceConfig as ExcelConfig);
                        break;
                    case (int)DataSourceType.DefaultValue:
                        BindDefaultValueConfig(CurrentMapping.DataSourceConfig as DefaultValueConfig);
                        break;
                    case (int)DataSourceType.SystemGenerated:
                        BindSystemGeneratedConfig(CurrentMapping.DataSourceConfig as SystemGeneratedConfig);
                        break;
                    case (int)DataSourceType.ForeignKey:
                        BindForeignKeyConfig(CurrentMapping.DataSourceConfig as ForeignKeyConfig);
                        break;
                    case (int)DataSourceType.SelfReference:
                        BindSelfReferenceConfig(CurrentMapping.DataSourceConfig as SelfReferenceConfig);
                        break;
                    case (int)DataSourceType.FieldCopy:
                        BindFieldCopyConfig(CurrentMapping.DataSourceConfig as FieldCopyConfig);
                        break;
                    case (int)DataSourceType.ColumnConcat:
                        BindColumnConcatConfig(CurrentMapping.DataSourceConfig as ColumnConcatConfig);
                        break;
                    case (int)DataSourceType.ExcelImage:
                        BindExcelImageConfig(CurrentMapping.DataSourceConfig as ExcelImageConfig);
                        break;
                }
            }

            // 5. 加载下拉框数据（如果需要）
            LoadDropdownDataIfNeeded(CurrentMapping.ColumnDataSourceType);

            // 6. 更新控件状态
            UpdateControlStates();
        }

        /// <summary>
        /// 根据数据源类型加载下拉框数据
        /// </summary>
        private void LoadDropdownDataIfNeeded(int dataSourceType)
        {
            switch (dataSourceType)
            {
                case (int)DataSourceType.SelfReference:
                    LoadSelfReferenceFields();
                    break;
                case (int)DataSourceType.FieldCopy:
                    LoadCopyFromFields();
                    break;
                case (int)DataSourceType.ColumnConcat:
                    LoadConcatSourceColumns();
                    break;
                case (int)DataSourceType.ExcelImage:
                    LoadImageNamingColumns();
                    break;
                case (int)DataSourceType.ForeignKey:
                    LoadForeignKeySourceColumns();
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
        /// 绑定默认值配置
        /// </summary>
        private void BindDefaultValueConfig(DefaultValueConfig config)
        {
            if (config == null) return;

            // 生成动态默认值控件
            GenerateDefaultValueControl();

            // 设置默认值
            if (_dynamicDefaultValueControl == ktxtDefaultValue)
            {
                ktxtDefaultValue.Text = config.Value ?? string.Empty;
            }
            else if (_dynamicDefaultValueControl == chkDynamicDefaultBool)
            {
                bool boolValue = false;
                if (bool.TryParse(config.Value, out boolValue))
                {
                    chkDynamicDefaultBool.Checked = boolValue;
                }
            }
            else if (_dynamicDefaultValueControl == dtpDynamicDefaultDateTime)
            {
                if (DateTime.TryParse(config.Value, out DateTime dateTimeValue))
                {
                    dtpDynamicDefaultDateTime.Value = dateTimeValue;
                    dtpDynamicDefaultDateTime.Checked = true;
                }
            }
            else if (_dynamicDefaultValueControl == cmbDynamicDefaultEnum && !string.IsNullOrEmpty(config.EnumTypeName))
            {
                // 枚举类型需要特殊处理
                Type enumType = AssemblyLoader.GetType("RUINORERP.Model", config.EnumTypeName);
                if (enumType != null && enumType.IsEnum)
                {
                    foreach (EnumItemInfo item in cmbDynamicDefaultEnum.Items)
                    {
                        if (item.EnumValue == config.EnumValue)
                        {
                            cmbDynamicDefaultEnum.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            else if (_dynamicDefaultValueControl == cmbDynamicDefaultList)
            {
                if (!string.IsNullOrEmpty(config.Value))
                {
                    cmbDynamicDefaultList.SelectedValue = config.Value;
                }
            }
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
        }

        /// <summary>
        /// 绑定外键关联配置
        /// </summary>
        private void BindForeignKeyConfig(ForeignKeyConfig config)
        {
            if (config == null) return;

            // 加载关联表列表
            LoadRelatedTables();

            // 选中对应的关联表
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

            // 绑定外键字段显示名称
            if (!string.IsNullOrEmpty(config.ForeignFieldDisplayName))
            {
                ktxtRelatedField.SelectedItem = config.ForeignFieldDisplayName;
            }

            // 加载外键来源列并选中已配置的值
            string foreignKeySourceColumn = config.ForeignKeySourceColumn?.Key;
            LoadForeignKeySourceColumns(foreignKeySourceColumn);
        }

        /// <summary>
        /// 绑定自身引用配置
        /// </summary>
        private void BindSelfReferenceConfig(SelfReferenceConfig config)
        {
            if (config == null) return;

            LoadSelfReferenceFields();

            // 双向绑定到显示名称（SelectedItem 绑定）
            kcmbSelfReferenceField.DataBindings.Clear();
            var binding = new Binding("SelectedItem", config, nameof(SelfReferenceConfig.ReferenceFieldDisplayName), 
                false, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (s, args) => args.Value = args.Value ?? string.Empty;
            binding.Parse += (s, args) => args.Value = args.Value ?? string.Empty;
            kcmbSelfReferenceField.DataBindings.Add(binding);
        }

        /// <summary>
        /// 绑定字段复制配置
        /// </summary>
        private void BindFieldCopyConfig(FieldCopyConfig config)
        {
            if (config == null) return;

            LoadCopyFromFields();

            // 双向绑定到显示名称（SelectedItem 绑定）
            kcmbCopyFromField.DataBindings.Clear();
            var binding = new Binding("SelectedItem", config, nameof(FieldCopyConfig.SourceFieldDisplayName), 
                false, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (s, args) => args.Value = args.Value ?? string.Empty;
            binding.Parse += (s, args) => args.Value = args.Value ?? string.Empty;
            kcmbCopyFromField.DataBindings.Add(binding);
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
            kcmbImageNamingColumn.DataBindings.Clear();
            var binding = new Binding("SelectedItem", config, nameof(ExcelImageConfig.NamingReferenceColumn), 
                false, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (s, args) => args.Value = args.Value ?? string.Empty;
            binding.Parse += (s, args) => args.Value = args.Value ?? string.Empty;
            kcmbImageNamingColumn.DataBindings.Add(binding);
        }

        /// <summary>
        /// 更新控件状态
        /// </summary>
        private void UpdateControlStates()
        {
            DataSourceType dataSourceType = (DataSourceType)kcmbDataSourceType.SelectedIndex;

            // 根据数据来源类型控制控件状态
            kcmbRelatedTable.Enabled = (dataSourceType == DataSourceType.ForeignKey);
            ktxtRelatedField.Enabled = (dataSourceType == DataSourceType.ForeignKey);
            kcmbForeignExcelSourceColumn.Enabled = (dataSourceType == DataSourceType.ForeignKey);
            kcmbSelfReferenceField.Enabled = (dataSourceType == DataSourceType.SelfReference);
            kcmbCopyFromField.Enabled = (dataSourceType == DataSourceType.FieldCopy);

            // 根据数据来源类型设置相关标志（不再需要，直接使用 DataSourceType 判断）
            // IsForeignKey = (dataSourceType == DataSourceType.ForeignKey);
            // IsSystemGenerated = (dataSourceType == DataSourceType.SystemGenerated);

            // 控制GroupBox的显示和隐藏
            kryptonGroupBoxForeignType.Visible = (dataSourceType == DataSourceType.ForeignKey);
            kryptonGroupBoxConcat.Visible = (dataSourceType == DataSourceType.ColumnConcat);

            // 控制图片配置GroupBox的显示和隐藏
            kryptonGroupBoxImageType.Visible = (dataSourceType == DataSourceType.ExcelImage) || kchkIsImageColumn.Checked;

            // 控制系统生成配置GroupBox的显示和隐藏
            kryptonGroupBoxSystemGenerated.Visible = (dataSourceType == DataSourceType.SystemGenerated);

            // 更新系统生成配置控件的可用性
            UpdateSystemGeneratedControlStates();

            // 如果选择了Excel图片类型，自动勾选图片列
            if (dataSourceType == DataSourceType.ExcelImage)
            {
                kchkIsImageColumn.Checked = true;
            }

            // 处理默认值控件：选择默认值时动态生成控件
            if (dataSourceType == DataSourceType.DefaultValue)
            {
                GenerateDefaultValueControl();
            }
            else
            {
                RemoveDefaultValueControl();
            }
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
                if (CurrentMapping?.DataSourceConfig is ForeignKeyConfig config)
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
            DataSourceType dataSourceType = (DataSourceType)kcmbDataSourceType.SelectedIndex;

            // 2. 使用验证适配器进行配置验证
            var validator = new ImportValidationAdapter();
            if (!validator.ValidateColumnMapping(CurrentMapping, out List<string> validationErrors))
            {
                string errorMsg = "列配置验证失败：\n" + string.Join("\n", validationErrors);
                MessageBox.Show(errorMsg, "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. 直接使用配置对象（双向绑定已自动同步控件值到配置对象）
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
            if (kcmbSystemGeneratedType == null) return;

            SystemGeneratedType generatedType = (SystemGeneratedType)kcmbSystemGeneratedType.SelectedIndex;

            // 时间格式配置
            kryptonLabel22.Visible = (generatedType == SystemGeneratedType.DateTime ||
                                      generatedType == SystemGeneratedType.Date);
            ktxtDateTimeFormat.Visible = kryptonLabel22.Visible;

            // 业务编码相关配置
            kryptonLabel23.Visible = (generatedType == SystemGeneratedType.BusinessCode);
            kcmbBusinessCodeRule.Visible = kryptonLabel23.Visible;
            kryptonLabel24.Visible = (generatedType == SystemGeneratedType.BusinessCode &&
                                     (kcmbBusinessCodeRule.SelectedIndex == (int)BusinessCodeRule.PrefixDateSequence ||
                                      kcmbBusinessCodeRule.SelectedIndex == (int)BusinessCodeRule.PrefixSequence));
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
            UpdateControlStates();

            // ✅ 根据数据来源类型自动切换到对应的Tab页
            kryptonTabControl.SelectedIndex = (int)kcmbDataSourceType.SelectedValue;

            // 如果选择了自身引用，加载当前表的字段列表
            if (kcmbDataSourceType.SelectedIndex == (int)DataSourceType.SelfReference)
            {
                LoadSelfReferenceFields();
            }

            // 如果选择了字段复制，加载当前表的字段列表
            if (kcmbDataSourceType.SelectedIndex == (int)DataSourceType.FieldCopy)
            {
                LoadCopyFromFields();
            }

            // 如果选择了列拼接，加载Excel列列表
            if (kcmbDataSourceType.SelectedIndex == (int)DataSourceType.ColumnConcat)
            {
                LoadConcatSourceColumns();
            }

            // 如果选择了Excel图片，加载图片命名参考列
            if (kcmbDataSourceType.SelectedIndex == (int)DataSourceType.ExcelImage)
            {
                LoadImageNamingColumns();
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

                // ✅ 先清空下拉框,避免重复添加
                kcmbSelfReferenceField.Items.Clear();

                // ✅ 使用HashSet去重,防止字段名重复
                var addedFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var field in fieldNameList)
                {
                    // 只有未添加过的字段才添加
                    if (!addedFields.Contains(field.Value))
                    {
                        kcmbSelfReferenceField.Items.Add(field.Value); // 添加中文名
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
        /// 自身引用字段选择改变事件
        /// 注意：中文显示名已通过双向绑定自动同步，这里只需要同步英文字段名
        /// </summary>
        private void kcmbSelfReferenceField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbSelfReferenceField.SelectedIndex < 0 || _fieldInfoDict == null)
            {
                return;
            }

            string displayName = kcmbSelfReferenceField.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(displayName)) return;

            var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
            if (field.Value != null && CurrentMapping?.DataSourceConfig is SelfReferenceConfig config)
            {
                config.ReferenceFieldName = field.Key;
            }
        }

        /// <summary>
        /// 加载可复制的字段列表
        /// </summary>
        private void LoadCopyFromFields()
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

                // ✅ 先清空下拉框,避免重复添加
                kcmbCopyFromField.Items.Clear();

                // ✅ 使用HashSet去重,防止字段名重复
                var addedFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var field in fieldNameList)
                {
                    // 排除当前字段本身
                    if (field.Key != CurrentMapping?.SystemField?.Key)
                    {
                        // 只有未添加过的字段才添加
                        if (!addedFields.Contains(field.Value))
                        {
                            kcmbCopyFromField.Items.Add(field.Value); // 添加中文名
                            addedFields.Add(field.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载可复制字段失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 字段复制选择改变事件
        /// 注意：中文显示名已通过双向绑定自动同步，这里只需要同步英文字段名
        /// </summary>
        private void kcmbCopyFromField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbCopyFromField.SelectedIndex < 0 || _fieldInfoDict == null)
            {
                return;
            }

            string displayName = kcmbCopyFromField.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(displayName)) return;

            var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
            if (field.Value != null && CurrentMapping?.DataSourceConfig is FieldCopyConfig config)
            {
                config.SourceFieldName = field.Key;
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
            if (field.Value != null && CurrentMapping?.DataSourceConfig is ForeignKeyConfig config)
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
        /// 生成动态默认值控件
        /// 根据目标字段的数据类型自动生成合适的输入控件
        /// </summary>
        private void GenerateDefaultValueControl()
        {
            try
            {
                // 移除旧控件
                RemoveDefaultValueControl();

                if (CurrentMapping?.SystemField == null || TargetEntityType == null)
                {
                    // 如果没有映射信息，使用默认文本框
                    ShowDefaultTextBox();
                    return;
                }

                // 获取字段属性信息
                string fieldName = CurrentMapping.SystemField.Key;
                PropertyInfo property = TargetEntityType.GetProperty(fieldName);
                if (property == null)
                {
                    ShowDefaultTextBox();
                    return;
                }

                Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                // 获取预定义的枚举类型（针对特殊字段）
                Type enumType = GetPredefinedEnumType(TargetEntityType.Name, fieldName);


                //根据生成目标表中的的目标字段的特性。去查找是不是有外键

                // 根据字段类型生成控件
                if (propertyType == typeof(bool))
                {
                    GenerateBooleanControl();
                }
                else if (propertyType == typeof(DateTime))
                {
                    GenerateDateTimeControl();
                }
                else if (propertyType.IsEnum)
                {
                    GenerateEnumControl(propertyType);
                }
                else if (enumType != null && enumType.IsEnum)
                {
                    GenerateEnumControl(enumType);
                }
                else if (propertyType == typeof(long) || propertyType == typeof(long?) || propertyType == typeof(int) || propertyType == typeof(int?))
                {
                    // 检查是否有外键特性
                    var fkAttr = property?.GetCustomAttribute<FKRelationAttribute>();
                    if (fkAttr != null)
                    {
                        GenerateForeignKeyControl(property, fieldName, fkAttr);
                    }
                    else
                    {
                        ShowDefaultTextBox();
                    }
                }
                else
                {
                    ShowDefaultTextBox();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"生成默认值控件失败: {ex.Message}");
                ShowDefaultTextBox();
            }
        }

        /// <summary>
        /// 获取预定义的枚举类型（针对特殊字段）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>枚举类型，无预定义返回null</returns>
        private Type GetPredefinedEnumType(string tableName, string fieldName)
        {
            return EntityImportHelper.GetPredefinedEnumType(tableName, fieldName);
        }

        /// <summary>
        /// 移除动态生成的默认值控件（已废弃，现在使用 HideAllDefaultValueControls）
        /// </summary>
        private void RemoveDefaultValueControl()
        {
            // 不再需要动态删除控件，只需隐藏即可
            HideAllDefaultValueControls();
            _dynamicDefaultValueControl = null;
        }

        /// <summary>
        /// 隐藏所有默认值控件
        /// </summary>
        private void HideAllDefaultValueControls()
        {
            ktxtDefaultValue.Visible = false;
            chkDynamicDefaultBool.Visible = false;
            dtpDynamicDefaultDateTime.Visible = false;
            cmbDynamicDefaultEnum.Visible = false;
            cmbDynamicDefaultList.Visible = false;
        }

        /// <summary>
        /// 显示默认文本框
        /// </summary>
        private void ShowDefaultTextBox()
        {
            // 隐藏其他默认值控件
            HideAllDefaultValueControls();

            ktxtDefaultValue.Visible = true;
            _dynamicDefaultValueControl = ktxtDefaultValue;
        }

        /// <summary>
        /// 生成布尔类型控件
        /// </summary>
        private void GenerateBooleanControl()
        {
            // 隐藏其他默认值控件
            HideAllDefaultValueControls();

            // 显示布尔控件
            chkDynamicDefaultBool.Visible = true;
            _dynamicDefaultValueControl = chkDynamicDefaultBool;

            // 设置初始值
            if (CurrentMapping?.DataSourceConfig is DefaultValueConfig config)
            {
                bool boolValue = false;
                if (bool.TryParse(config.Value, out boolValue))
                {
                    chkDynamicDefaultBool.Checked = boolValue;
                }
            }

            // 绑定事件（先解绑再绑定，避免重复）
            chkDynamicDefaultBool.CheckedChanged -= ChkDynamicDefaultBool_CheckedChanged;
            chkDynamicDefaultBool.CheckedChanged += ChkDynamicDefaultBool_CheckedChanged;
        }

        /// <summary>
        /// 布尔控件值改变事件
        /// </summary>
        private void ChkDynamicDefaultBool_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentMapping?.DataSourceConfig is DefaultValueConfig config)
            {
                config.Value = chkDynamicDefaultBool.Checked.ToString();
            }
        }

        /// <summary>
        /// 生成日期时间类型控件
        /// </summary>
        private void GenerateDateTimeControl()
        {
            // 隐藏其他默认值控件
            HideAllDefaultValueControls();

            // 显示日期时间控件
            dtpDynamicDefaultDateTime.Visible = true;
            _dynamicDefaultValueControl = dtpDynamicDefaultDateTime;

            // 绑定事件（先解绑再绑定，避免重复）
            dtpDynamicDefaultDateTime.ValueChanged -= DtpDynamicDefaultDateTime_ValueChanged;
            dtpDynamicDefaultDateTime.ValueChanged += DtpDynamicDefaultDateTime_ValueChanged;
        }

        /// <summary>
        /// 日期时间控件值改变事件
        /// </summary>
        private void DtpDynamicDefaultDateTime_ValueChanged(object sender, EventArgs e)
        {
            // 直接更新当前映射的默认值配置（如果存在）
            if (CurrentMapping?.DataSourceConfig is DefaultValueConfig defaultConfig)
            {
                if (dtpDynamicDefaultDateTime.Checked)
                {
                    defaultConfig.Value = dtpDynamicDefaultDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    defaultConfig.Value = string.Empty;
                }
            }
        }

        /// <summary>
        /// 生成枚举类型控件
        /// </summary>
        private void GenerateEnumControl(Type enumType)
        {
            // 隐藏其他默认值控件
            HideAllDefaultValueControls();

            // 清空并重新加载枚举项
            cmbDynamicDefaultEnum.Items.Clear();

            // 加载枚举值（带显示文本）
            foreach (var value in Enum.GetValues(enumType))
            {
                // 创建一个枚举信息对象存储
                var enumInfo = new EnumItemInfo
                {
                    EnumType = enumType,
                    EnumValue = (int)value,
                    EnumName = value.ToString(),
                    DisplayName = GetEnumDisplayName(enumType, value)
                };
                cmbDynamicDefaultEnum.Items.Add(enumInfo);
            }

            // 显示枚举控件
            cmbDynamicDefaultEnum.Visible = true;
            _dynamicDefaultValueControl = cmbDynamicDefaultEnum;

            // 绑定事件（先解绑再绑定，避免重复）
            cmbDynamicDefaultEnum.SelectedIndexChanged -= CmbDynamicDefaultEnum_SelectedIndexChanged;
            cmbDynamicDefaultEnum.SelectedIndexChanged += CmbDynamicDefaultEnum_SelectedIndexChanged;
        }

        /// <summary>
        /// 枚举控件选择改变事件
        /// </summary>
        private void CmbDynamicDefaultEnum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDynamicDefaultEnum.SelectedItem is EnumItemInfo selectedInfo)
            {
                // 直接更新当前映射的默认值配置（如果存在）
                if (CurrentMapping?.DataSourceConfig is DefaultValueConfig defaultConfig)
                {
                    defaultConfig.EnumTypeName = selectedInfo.EnumType.FullName;
                    defaultConfig.EnumValue = selectedInfo.EnumValue;
                    defaultConfig.EnumName = selectedInfo.EnumName;
                    defaultConfig.EnumDisplayName = selectedInfo.DisplayName;
                    defaultConfig.Value = selectedInfo.EnumName;
                }
            }
        }

        /// <summary>
        /// 获取枚举值的显示名称（优先使用Description特性）
        /// </summary>
        private string GetEnumDisplayName(Type enumType, object enumValue)
        {
            try
            {
                var field = enumType.GetField(enumValue.ToString());
                if (field != null)
                {
                    var descAttr = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                        .FirstOrDefault() as System.ComponentModel.DescriptionAttribute;
                    if (descAttr != null && !string.IsNullOrEmpty(descAttr.Description))
                    {
                        return descAttr.Description;
                    }
                }
            }
            catch { }
            return enumValue.ToString();
        }

        /// <summary>
        /// 枚举项信息（用于ComboBox显示和值存储）
        /// </summary>
        public class EnumItemInfo
        {
            public Type EnumType { get; set; }
            public int EnumValue { get; set; }
            public string EnumName { get; set; }
            public string DisplayName { get; set; }

            public override string ToString()
            {
                return !string.IsNullOrEmpty(DisplayName) ? $"{DisplayName} ({EnumName})" : EnumName;
            }
        }

        /// <summary>
        /// 生成外键类型控件（下拉列表）
        /// </summary>
        private void GenerateForeignKeyControl(PropertyInfo property, string fieldName, FKRelationAttribute fkAttr)
        {
            // 隐藏其他默认值控件
            HideAllDefaultValueControls();

            // 清空cmbDynamicDefaultList
            cmbDynamicDefaultList.DataSource = null;
            cmbDynamicDefaultList.Items.Clear();

            try
            {
                if (fkAttr != null)
                {
                    // 获取关联表类型
                    string fkTableName = fkAttr.FKTableName;
                    Type fkEntityType = null;

                    // 从 EntityTypeMappings 中查找关联表类型
                    var mapping = UCBasicDataImport.EntityTypeMappings
                        .FirstOrDefault(m => m.Value.Name == fkTableName || m.Value.Name.Contains(fkTableName));

                    if (mapping.Value != null)
                    {
                        fkEntityType = mapping.Value;
                    }

                    if (fkEntityType != null)
                    {
                        // 使用 UIGenerateHelper 绑定外键数据
                        BindForeignKeyData(cmbDynamicDefaultList, fkEntityType, fkAttr);
                    }
                    else
                    {
                        cmbDynamicDefaultList.Items.Add($"未找到关联表: {fkTableName}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"生成外键控件失败: {ex.Message}");
                cmbDynamicDefaultList.Items.Clear();
                cmbDynamicDefaultList.Items.Add($"加载失败: {ex.Message}");
            }

            // 显示外键控件
            cmbDynamicDefaultList.Visible = true;
            _dynamicDefaultValueControl = cmbDynamicDefaultList;

            // 绑定事件（先解绑再绑定，避免重复）
            cmbDynamicDefaultList.SelectedIndexChanged -= CmbDynamicDefaultList_SelectedIndexChanged;
            cmbDynamicDefaultList.SelectedIndexChanged += CmbDynamicDefaultList_SelectedIndexChanged;
        }

        /// <summary>
        /// 外键控件选择改变事件
        /// </summary>
        private void CmbDynamicDefaultList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDynamicDefaultList.SelectedIndex >= 0 && cmbDynamicDefaultList.SelectedItem != null)
            {
                // 保存选中的值（ID）
                var selectedValue = cmbDynamicDefaultList.SelectedValue;
                if (selectedValue != null)
                {
                    // 直接更新当前映射的默认值配置（如果存在）
                    if (CurrentMapping?.DataSourceConfig is DefaultValueConfig defaultConfig)
                    {
                        defaultConfig.Value = selectedValue.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 绑定外键数据到下拉列表
        /// </summary>
        /// <param name="comboBox">下拉列表控件</param>
        /// <param name="fkEntityType">外键关联实体类型</param>
        /// <param name="fkAttr">外键关系特性</param>
        private void BindForeignKeyData(KryptonComboBox comboBox, Type fkEntityType, FKRelationAttribute fkAttr)
        {
            try
            {
                // 如果数据库客户端为空，提示用户
                if (_db == null)
                {
                    comboBox.Items.Clear();
                    comboBox.Items.Add("数据库连接不可用");
                    return;
                }

                // 获取主键名称
                string primaryKey = GetPrimaryKeyName(fkEntityType);
                string displayField = string.Empty;
                // 获取显示字段（通常是名称字段）
                var tableSchema = _tableSchemaManager.GetSchemaInfo(fkEntityType.Name);
                if (tableSchema != null)
                {
                    displayField = tableSchema.DisplayField;
                }
                // 构建查询
                string query = $"SELECT {primaryKey}, {displayField} FROM {fkAttr.FKTableName} ORDER BY {displayField}";

                // 执行查询获取数据
                var dataTable = _db.Ado.GetDataTable(query);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    // 手动绑定数据到KryptonComboBox
                    comboBox.DataSource = dataTable;
                    comboBox.DisplayMember = displayField;
                    comboBox.ValueMember = primaryKey;
                }
                else
                {
                    comboBox.Items.Clear();
                    comboBox.Items.Add("无可用数据");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"绑定外键数据失败: {ex.Message}");
                comboBox.Items.Clear();
                comboBox.Items.Add($"加载失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取实体的主键名称
        /// </summary>
        private string GetPrimaryKeyName(Type entityType)
        {
            var properties = entityType.GetProperties();
            foreach (var prop in properties)
            {
                var pkAttr = prop.GetCustomAttribute<SugarColumn>();
                if (pkAttr != null && pkAttr.IsPrimaryKey)
                {
                    return prop.Name;
                }
            }
            return entityType.Name + "ID"; // 默认命名规则
        }


        /// <summary>
        /// 从动态控件获取默认值
        /// </summary>
        private string GetDefaultValueFromDynamicControl()
        {
            if (_dynamicDefaultValueControl == ktxtDefaultValue)
            {
                return ktxtDefaultValue.Text.Trim();
            }

            switch (_dynamicDefaultValueControl?.Name)
            {
                case "chkDynamicDefaultBool":
                    return (_dynamicDefaultValueControl as KryptonCheckBox)?.Checked.ToString() ?? "False";
                case "dtpDynamicDefaultDateTime":
                    var dtp = _dynamicDefaultValueControl as KryptonDateTimePicker;
                    return dtp?.Checked == true ? dtp.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                case "cmbDynamicDefaultEnum":
                    // 枚举控件返回枚举名称
                    var enumComboBox = _dynamicDefaultValueControl as KryptonComboBox;
                    if (enumComboBox?.SelectedItem is EnumItemInfo enumInfo)
                    {
                        return enumInfo.EnumName;
                    }
                    return enumComboBox?.SelectedItem?.ToString() ?? string.Empty;
                case "cmbDynamicDefaultForeignKey":
                    // 外键控件应该返回SelectedValue（主键ID），而不是SelectedItem（显示字段值）
                    var comboBox = _dynamicDefaultValueControl as KryptonComboBox;
                    return comboBox?.SelectedValue?.ToString() ?? string.Empty;
                default:
                    return ktxtDefaultValue.Text.Trim();
            }
        }

        /// <summary>
        /// 图片列复选框变更事件
        /// </summary>
        private void kchkIsImageColumn_CheckedChanged(object sender, EventArgs e)
        {
            // 更新GroupBox的可见性
            kryptonGroupBoxImageType.Visible = kchkIsImageColumn.Checked;

            // 启用或禁用图片类型下拉框
            kcmbImageColumnType.Enabled = kchkIsImageColumn.Checked;
        }
    }
}
