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
        /// 是否为外键
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 是否值唯一
        /// </summary>
        public bool IsUniqueValue { get; set; }

        /// <summary>
        /// 是否忽略空值（为空时不导入）
        /// </summary>
        public bool IgnoreEmptyValue { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否系统生成
        /// </summary>
        public bool IsSystemGenerated { get; set; }

        /// <summary>
        /// 外键表引用
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyTable { get; set; }

        /// <summary>
        /// 外键字段引用
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyField { get; set; }

        /// <summary>
        /// 数据来源类型
        /// </summary>
        public DataSourceType SelectedDataSourceType { get; set; }

        /// <summary>
        /// 自身引用字段
        /// </summary>
        public SerializableKeyValuePair<string> SelfReferenceField { get; set; }

        /// <summary>
        /// 复制字段
        /// </summary>
        public SerializableKeyValuePair<string> CopyFromField { get; set; }

        /// <summary>
        /// 外键来源列（Excel中的列名）
        /// 用于指定Excel中作为外键关联依据的来源列（如"供应商名称"列）
        /// </summary>
        public ForeignKeySourceColumnConfig ForeignKeySourceColumn { get; set; }

        /// <summary>
        /// 列拼接配置
        /// </summary>
        public ColumnConcatConfig ConcatConfig { get; set; }

        /// <summary>
        /// 枚举类型完整名称
        /// </summary>
        public string EnumTypeName { get; set; }

        /// <summary>
        /// 枚举默认值配置
        /// </summary>
        public EnumDefaultConfig EnumDefaultConfig { get; set; }

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
            LoadRelatedTables();
            LoadDataSourceTypes();
            _dataBindingHelper = new DataBindingHelper();
             _tableSchemaManager = Startup.GetFromFac<ITableSchemaManager>();
            
            // 手动绑定事件
            kcmbDataSourceType.SelectedIndexChanged += kcmbDataSourceType_SelectedIndexChanged;
            kcmbSelfReferenceField.SelectedIndexChanged += kcmbSelfReferenceField_SelectedIndexChanged;
            kcmbCopyFromField.SelectedIndexChanged += kcmbCopyFromField_SelectedIndexChanged;
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
        /// 加载数据来源类型列表
        /// </summary>
        private void LoadDataSourceTypes()
        {
            try
            {
                kcmbDataSourceType.Items.Clear();
                kcmbDataSourceType.Items.Add("Excel数据源");
                kcmbDataSourceType.Items.Add("默认值");
                kcmbDataSourceType.Items.Add("系统生成");
                kcmbDataSourceType.Items.Add("外键关联");
                kcmbDataSourceType.Items.Add("自身字段引用");
                kcmbDataSourceType.Items.Add("字段复制");
                kcmbDataSourceType.Items.Add("列拼接");
                kcmbDataSourceType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据来源类型失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void FrmColumnPropertyConfig_Load(object sender, EventArgs e)
        {
            if (CurrentMapping != null)
            {
                // 初始化当前配置
                // 通过DataSourceType判断是否为外键关联
                bool isForeignKey = CurrentMapping.DataSourceType == DataSourceType.ForeignKey;
                kchkIsForeignKey.Checked = isForeignKey;
                kchkIsUniqueValue.Checked = CurrentMapping.IsUniqueValue;
                kchkIgnoreEmptyValue.Checked = CurrentMapping.IgnoreEmptyValue;
                kchkIsSystemGenerated.Checked = CurrentMapping.IsSystemGenerated;
                ktxtDefaultValue.Text = CurrentMapping.DefaultValue ?? string.Empty;

                IsForeignKey = isForeignKey;
                IsUniqueValue = CurrentMapping.IsUniqueValue;
                IgnoreEmptyValue = CurrentMapping.IgnoreEmptyValue;
                DefaultValue = CurrentMapping.DefaultValue;
                IsSystemGenerated = CurrentMapping.IsSystemGenerated;
                EnumTypeName = CurrentMapping.EnumTypeName;

                // 初始化数据来源类型
                kcmbDataSourceType.SelectedIndex = (int)CurrentMapping.DataSourceType;
                SelectedDataSourceType = CurrentMapping.DataSourceType;

                // 初始化关联表信息
                if (CurrentMapping.ForeignConfig != null && !string.IsNullOrEmpty(CurrentMapping.ForeignConfig.ForeignKeyTable?.Key))
                {
                    // 查找对应的显示文本
                    for (int i = 0; i < kcmbRelatedTable.Items.Count; i++)
                    {
                        string itemText = kcmbRelatedTable.Items[i].ToString();
                        if (itemText.Contains(CurrentMapping.ForeignConfig.ForeignKeyTable.Key))
                        {
                            kcmbRelatedTable.SelectedIndex = i;
                            break;
                        }
                    }
                }

                ForeignKeyTable = CurrentMapping.ForeignConfig?.ForeignKeyTable;
                ktxtRelatedField.Text = CurrentMapping.ForeignConfig?.ForeignKeyField?.Value;

                // 初始化外键来源列
                if (CurrentMapping.ForeignConfig?.ForeignKeySourceColumn != null)
                {
                    ForeignKeySourceColumn = CurrentMapping.ForeignConfig.ForeignKeySourceColumn;
                }
                LoadForeignKeySourceColumns();
                if (ForeignKeySourceColumn != null && !string.IsNullOrEmpty(ForeignKeySourceColumn.ExcelColumnName))
                {
                    // 查找匹配的项
                    string searchText = ForeignKeySourceColumn.ExcelColumnName;

                    for (int i = 0; i < kcmbForeignExcelSourceColumn.Items.Count; i++)
                    {
                        if (kcmbForeignExcelSourceColumn.Items[i].ToString() == searchText ||
                            kcmbForeignExcelSourceColumn.Items[i].ToString().Contains($"({ForeignKeySourceColumn.ExcelColumnName})"))
                        {
                            kcmbForeignExcelSourceColumn.SelectedIndex = i;
                            break;
                        }
                    }
                }

                // 初始化自身引用字段
                if (CurrentMapping.DataSourceType == DataSourceType.SelfReference &&
                    CurrentMapping.SelfReferenceField != null)
                {
                    LoadSelfReferenceFields();
                    SelfReferenceField = CurrentMapping.SelfReferenceField;
                    kcmbSelfReferenceField.SelectedItem = CurrentMapping.SelfReferenceField?.Value;
                }

                // 初始化字段复制
                if (CurrentMapping.DataSourceType == DataSourceType.FieldCopy &&
                    CurrentMapping.CopyFromField != null)
                {
                    LoadCopyFromFields();
                    CopyFromField = CurrentMapping.CopyFromField;
                    kcmbCopyFromField.SelectedItem = CurrentMapping.CopyFromField?.Value;
                }

                // 初始化列拼接配置
                if (CurrentMapping.DataSourceType == DataSourceType.ColumnConcat &&
                    CurrentMapping.ConcatConfig != null)
                {
                    ConcatConfig = CurrentMapping.ConcatConfig;

                    // 加载Excel列列表（会自动选中已配置的列）
                    LoadConcatSourceColumns();

                    // 加载分隔符
                    ktxtSeparator.Text = ConcatConfig.Separator ?? string.Empty;

                    // 加载选项
                    kchkTrimWhitespace.Checked = ConcatConfig.TrimWhitespace;
                    kchkIgnoreEmptyColumns.Checked = ConcatConfig.IgnoreEmptyColumns;
                }
            }

            // 更新控件状态
            UpdateControlStates();
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

            // 同步复选框状态
            kchkIsForeignKey.Enabled = (dataSourceType == DataSourceType.ForeignKey);
            kchkIsSystemGenerated.Enabled = (dataSourceType == DataSourceType.SystemGenerated);

            // 根据数据来源类型设置复选框
            if (dataSourceType == DataSourceType.ForeignKey)
            {
                kchkIsForeignKey.Checked = true;
            }
            else if (dataSourceType == DataSourceType.SystemGenerated)
            {
                kchkIsSystemGenerated.Checked = true;
            }
            else
            {
                kchkIsForeignKey.Checked = false;
                kchkIsSystemGenerated.Checked = false;
            }

            // 控制GroupBox的显示和隐藏
            kryptonGroupBoxForeignType.Visible = (dataSourceType == DataSourceType.ForeignKey);
            kryptonGroupBoxConcat.Visible = (dataSourceType == DataSourceType.ColumnConcat);

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
        private void LoadForeignKeySourceColumns()
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
                else if (CurrentMapping != null && !string.IsNullOrEmpty(CurrentMapping.ExcelColumn))
                {
                    // 如果没有传入Excel列列表，但当前映射有Excel列，则使用该列
                    // 并检查是否是特殊标记的列
                    if (!CurrentMapping.ExcelColumn.StartsWith("[") && !CurrentMapping.ExcelColumn.StartsWith("("))
                    {
                        kcmbForeignExcelSourceColumn.Items.Add(CurrentMapping.ExcelColumn);
                    }
                }

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

                // 如果在EntityTypeMappings中找不到，使用原来的GetTableType方法
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

                // 清空并添加字段到下拉框
                ktxtRelatedField.Items.Clear();
                kcmbForeignDbSourceColumn.Items.Clear();
                foreach (var field in fieldNameList)
                {
                    ktxtRelatedField.Items.Add(field.Value); // 添加中文名
                    kcmbForeignDbSourceColumn.Items.Add(field.Value); // 添加中文名
                }

                // 如果有已选中的字段，保持选中状态
                if (!string.IsNullOrEmpty(ForeignKeyField?.Value))
                {
                    int index = ktxtRelatedField.Items.IndexOf(ForeignKeyField?.Value);
                    if (index >= 0)
                    {
                        ktxtRelatedField.SelectedIndex = index;
                    }
                }

                // 如果有已选中的字段，保持选中状态
                string selectedDisplayName = ForeignKeySourceColumn?.DisplayName;
                if (!string.IsNullOrEmpty(selectedDisplayName))
                {
                    int index = kcmbForeignDbSourceColumn.Items.IndexOf(selectedDisplayName);
                    if (index >= 0)
                    {
                        kcmbForeignDbSourceColumn.SelectedIndex = index;
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
                ForeignKeyField = new SerializableKeyValuePair<string>(field.Key, field.Value);
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnOK_Click(object sender, EventArgs e)
        {
            DataSourceType dataSourceType = (DataSourceType)kcmbDataSourceType.SelectedIndex;
            SelectedDataSourceType = dataSourceType;

            // 根据数据来源类型进行验证
            if (dataSourceType == DataSourceType.ForeignKey)
            {
                if (kcmbRelatedTable.SelectedIndex <= 0)
                {
                    MessageBox.Show("请选择关联表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ktxtRelatedField.SelectedIndex < 0 && string.IsNullOrWhiteSpace(ktxtRelatedField.Text))
                {
                    MessageBox.Show("请选择关联表字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 获取关联表名（提取括号中的表名）
                string selectedTable = kcmbRelatedTable.SelectedItem.ToString();
                int startIndex = selectedTable.IndexOf('(') + 1;
                int endIndex = selectedTable.IndexOf(')');
                string tableName;
                if (startIndex > 0 && endIndex > startIndex)
                {
                    tableName = selectedTable.Substring(startIndex, endIndex - startIndex);
                }
                else
                {
                    tableName = selectedTable;
                }

                string displayName = ktxtRelatedField.SelectedItem?.ToString() ?? ktxtRelatedField.Text;
                var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
                if (field.Value != null)
                {
                    ForeignKeyTable = new SerializableKeyValuePair<string>(tableName, GetTableDisplayName(tableName));
                    ForeignKeyField = new SerializableKeyValuePair<string>(field.Key, field.Value);
                }

                // 获取外键来源列
                if (kcmbForeignExcelSourceColumn.SelectedIndex > 0)
                {
                    string selectedColumnText = kcmbForeignExcelSourceColumn.SelectedItem.ToString();

                    // 解析选择的文本，格式可能是："显示名称 (Excel列名)" 或只是 "Excel列名"
                    string excelColumnName;
                    string columnDisplayName = string.Empty;

                    int columnStartIndex = selectedColumnText.LastIndexOf('(');
                    int columnEndIndex = selectedColumnText.LastIndexOf(')');

                    if (columnStartIndex > 0 && columnEndIndex > columnStartIndex)
                    {
                        // 格式："显示名称 (Excel列名)"
                        columnDisplayName = selectedColumnText.Substring(0, columnStartIndex).Trim();
                        excelColumnName = selectedColumnText.Substring(columnStartIndex + 1, columnEndIndex - columnStartIndex - 1);
                    }
                    else
                    {
                        // 格式：只是 "Excel列名"
                        excelColumnName = selectedColumnText;
                        columnDisplayName = excelColumnName;
                    }

                    // 获取数据库真实字段名（从下拉框选择）
                    string dbFieldName = string.Empty;
                    if (kcmbForeignDbSourceColumn.SelectedIndex >= 0 && _fieldInfoDict != null)
                    {
                        string selectedDbFieldDisplay = kcmbForeignDbSourceColumn.SelectedItem.ToString();
                        var dbField = _fieldInfoDict.FirstOrDefault(f => f.Value == selectedDbFieldDisplay);
                        if (dbField.Value != null)
                        {
                            dbFieldName = dbField.Key; // 数据库真实字段名
                        }
                    }

                    // 构建外键来源列配置
                    ForeignKeySourceColumn = new ForeignKeySourceColumnConfig
                    {
                        ExcelColumnName = excelColumnName,
                        DisplayName = columnDisplayName,
                        DatabaseFieldName = dbFieldName
                    };
                }
                else
                {
                    ForeignKeySourceColumn = null;
                }
            }
            else if (dataSourceType == DataSourceType.SelfReference)
            {
                if (kcmbSelfReferenceField.SelectedIndex < 0 && string.IsNullOrWhiteSpace(kcmbSelfReferenceField.Text))
                {
                    MessageBox.Show("请选择自身引用字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string displayName = kcmbSelfReferenceField.SelectedItem?.ToString() ?? kcmbSelfReferenceField.Text;
                var selfRefField = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
                if (selfRefField.Value != null)
                {
                    SelfReferenceField = new SerializableKeyValuePair<string>(selfRefField.Key, selfRefField.Value);
                }
            }
            else if (dataSourceType == DataSourceType.FieldCopy)
            {
                if (kcmbCopyFromField.SelectedIndex < 0 && string.IsNullOrWhiteSpace(kcmbCopyFromField.Text))
                {
                    MessageBox.Show("请选择要复制的字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string displayName = kcmbCopyFromField.SelectedItem?.ToString() ?? kcmbCopyFromField.Text;
                var copyField = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
                if (copyField.Value != null)
                {
                    CopyFromField = new SerializableKeyValuePair<string>(copyField.Key, copyField.Value);
                }
            }
            else if (dataSourceType == DataSourceType.DefaultValue)
            {
                //这里应该去尝试获取值
                if (string.IsNullOrWhiteSpace(DefaultValue))
                {
                    MessageBox.Show("请输入默认值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (dataSourceType == DataSourceType.ColumnConcat)
            {
                // 获取选中的列
                var selectedColumns = klstSourceColumns.SelectedItems.Cast<string>().ToList();

                // 验证列拼接配置
                if (selectedColumns.Count < 2)
                {
                    MessageBox.Show($"列拼接功能需要至少选择2个Excel列\n当前已选择: {selectedColumns.Count} 列\n\n提示: 按住Ctrl键可多选", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 构建列拼接配置
                ConcatConfig = new ColumnConcatConfig
                {
                    SourceColumns = selectedColumns,
                    Separator = ktxtSeparator.Text.Trim(),
                    TrimWhitespace = kchkTrimWhitespace.Checked,
                    IgnoreEmptyColumns = kchkIgnoreEmptyColumns.Checked
                };

                IsForeignKey = false;
                IsSystemGenerated = false;
                DefaultValue = string.Empty;
            }

            // 根据数据来源类型设置属性
            if (dataSourceType == DataSourceType.ForeignKey)
            {
                IsForeignKey = true;
                IsSystemGenerated = false;
            }
            else if (dataSourceType == DataSourceType.SystemGenerated)
            {
                IsForeignKey = false;
                IsSystemGenerated = true;
            }
            else if (dataSourceType == DataSourceType.DefaultValue)
            {
                IsForeignKey = false;
                IsSystemGenerated = false;
                // 从动态控件获取默认值
                DefaultValue = GetDefaultValueFromDynamicControl();

                // 如果是枚举类型控件，保存完整的枚举配置
                if (_dynamicDefaultValueControl?.Name == "cmbDynamicDefaultEnum" &&
                    _dynamicDefaultValueControl is KryptonComboBox enumComboBox &&
                    enumComboBox.SelectedItem is EnumItemInfo enumInfo)
                {
                    EnumDefaultConfig = new EnumDefaultConfig
                    {
                        EnumTypeName = enumInfo.EnumType.FullName,
                        EnumValue = enumInfo.EnumValue,
                        EnumName = enumInfo.EnumName,
                        EnumDisplayName = enumInfo.DisplayName
                    };
                    EnumTypeName = enumInfo.EnumType.FullName; // 保持向后兼容
                }
                else
                {
                    EnumDefaultConfig = null;
                }
            }
            else if (dataSourceType == DataSourceType.SelfReference)
            {
                IsForeignKey = false;
                IsSystemGenerated = false;
                DefaultValue = string.Empty;
            }
            else if (dataSourceType == DataSourceType.FieldCopy)
            {
                IsForeignKey = false;
                IsSystemGenerated = false;
                DefaultValue = string.Empty;
            }
            else
            {
                IsForeignKey = false;
                IsSystemGenerated = false;
            }

            IsUniqueValue = kchkIsUniqueValue.Checked;
            IgnoreEmptyValue = kchkIgnoreEmptyValue.Checked;

            // 验证：如果既不是系统生成，又没有设置默认值，也不是外键或自身引用，需要提示
            if (dataSourceType == DataSourceType.Excel && string.IsNullOrWhiteSpace(DefaultValue))
            {
                var result = MessageBox.Show(
                    "该字段既不是系统生成的，也没有设置默认值，也没有特殊来源。\n" +
                    "如果没有Excel数据源，该字段将为空。\n\n" +
                    "确定要继续吗？",
                    "提示",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

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
        /// 数据来源类型选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kcmbDataSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlStates();

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

                // 清空并添加字段到下拉框
                kcmbSelfReferenceField.Items.Clear();
                foreach (var field in fieldNameList)
                {
                    kcmbSelfReferenceField.Items.Add(field.Value); // 添加中文名
                }

                // 如果有已选中的字段，保持选中状态
                if (!string.IsNullOrEmpty(SelfReferenceField?.Value))
                {
                    int index = kcmbSelfReferenceField.Items.IndexOf(SelfReferenceField?.Value);
                    if (index >= 0)
                    {
                        kcmbSelfReferenceField.SelectedIndex = index;
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
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kcmbSelfReferenceField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbSelfReferenceField.SelectedIndex < 0 || _fieldInfoDict == null)
            {
                return;
            }

            // 根据中文显示名称获取实际字段名
            string displayName = kcmbSelfReferenceField.SelectedItem.ToString();
            var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
            if (field.Value != null)
            {
                SelfReferenceField = new SerializableKeyValuePair<string>(field.Key, field.Value);

                // 显示字段信息提示,类似外键关联的处理方式
                MessageBox.Show(
                    $"已选择自身引用字段:\n" +
                    $"字段名称: {field.Key}\n" +
                    $"显示名称: {field.Value}\n\n" +
                    $"说明: 当前字段将引用同一条记录的【{field.Value}】字段的值。",
                    "自身引用字段提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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

                // 清空并添加字段到下拉框
                kcmbCopyFromField.Items.Clear();
                foreach (var field in fieldNameList)
                {
                    // 排除当前字段本身
                    if (field.Key != CurrentMapping?.SystemField?.Key)
                    {
                        kcmbCopyFromField.Items.Add(field.Value); // 添加中文名
                    }
                }

                // 如果有已选中的字段，保持选中状态
                if (!string.IsNullOrEmpty(CopyFromField?.Value))
                {
                    int index = kcmbCopyFromField.Items.IndexOf(CopyFromField?.Value);
                    if (index >= 0)
                    {
                        kcmbCopyFromField.SelectedIndex = index;
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
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kcmbCopyFromField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbCopyFromField.SelectedIndex < 0 || _fieldInfoDict == null)
            {
                return;
            }

            // 根据中文显示名称获取实际字段名
            string displayName = kcmbCopyFromField.SelectedItem.ToString();
            var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
            if (field.Value != null)
            {
                CopyFromField = new SerializableKeyValuePair<string>(field.Key, field.Value);

                // 显示字段信息提示
                MessageBox.Show(
                    $"已选择要复制的字段:\n" +
                    $"字段名称: {field.Key}\n" +
                    $"显示名称: {field.Value}\n\n" +
                    $"说明: 当前字段【{CurrentMapping?.SystemField?.Value}】将复制【{field.Value}】字段的值。\n" +
                    $"例如: Excel只提供了产品编码,产品名称字段可以复制产品编码的值。",
                    "字段复制提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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

        private void kcmbForeignDbSourceColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbForeignDbSourceColumn.SelectedIndex < 0 || _fieldInfoDict == null)
            {
                return;
            }

            // 根据中文显示名称获取实际字段名
            string displayName = kcmbForeignDbSourceColumn.SelectedItem.ToString();
            var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);

            if (field.Value != null)
            {
                // 如果已经有Excel列名，更新数据库字段名部分
                string excelColumnName = ForeignKeySourceColumn?.ExcelColumnName ?? string.Empty;
                if (string.IsNullOrEmpty(excelColumnName))
                {
                    excelColumnName = kcmbForeignExcelSourceColumn.SelectedItem.ToString();
                }


                // 构建外键来源列配置
                ForeignKeySourceColumn = new ForeignKeySourceColumnConfig
                {
                    ExcelColumnName = excelColumnName,
                    DisplayName = field.Value,
                    DatabaseFieldName = field.Key
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

                    // 如果已有配置，选中对应的列
                    if (ConcatConfig != null && ConcatConfig.SourceColumns != null)
                    {
                        for (int i = 0; i < klstSourceColumns.Items.Count; i++)
                        {
                            if (ConcatConfig.SourceColumns.Contains(klstSourceColumns.Items[i].ToString()))
                            {
                                klstSourceColumns.SetSelected(i, true);
                            }
                        }
                    }
                }
                else if (CurrentMapping != null && !string.IsNullOrEmpty(CurrentMapping.ExcelColumn))
                {
                    // 如果没有传入Excel列列表，但当前映射有Excel列，则使用该列
                    if (!CurrentMapping.ExcelColumn.StartsWith("[") && !CurrentMapping.ExcelColumn.StartsWith("("))
                    {
                        klstSourceColumns.Items.Add(CurrentMapping.ExcelColumn);
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

                // 检查是否手动指定了枚举类型（优先级高于预定义）
                if (enumType == null && !string.IsNullOrEmpty(CurrentMapping.EnumTypeName))
                {
                    enumType = AssemblyLoader.GetType("RUINORERP.Model", CurrentMapping.EnumTypeName);
                    if (enumType != null && enumType.IsEnum)
                    {
                        GenerateEnumControl(enumType);
                        return;
                    }
                }

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
        /// 移除动态生成的默认值控件
        /// </summary>
        private void RemoveDefaultValueControl()
        {
            if (_dynamicDefaultValueControl != null && _dynamicDefaultValueControl != ktxtDefaultValue)
            {
                kryptonPanel1.Controls.Remove(_dynamicDefaultValueControl);
                _dynamicDefaultValueControl.Dispose();
                _dynamicDefaultValueControl = null;
            }
        }

        /// <summary>
        /// 显示默认文本框
        /// </summary>
        private void ShowDefaultTextBox()
        {
            ktxtDefaultValue.Visible = true;
            ktxtDefaultValue.Location = new System.Drawing.Point(120, 220);
            ktxtDefaultValue.Size = new System.Drawing.Size(280, 23);
            _dynamicDefaultValueControl = ktxtDefaultValue;
        }

        /// <summary>
        /// 生成布尔类型控件
        /// </summary>
        private void GenerateBooleanControl()
        {
            var checkBox = new KryptonCheckBox
            {
                Name = "chkDynamicDefaultBool",
                Text = "如果默认是，则请勾选",
                Location = new System.Drawing.Point(120, 220),
                Size = new System.Drawing.Size(200, 20)
            };

            // 设置初始值
            bool boolValue = false;
            if (bool.TryParse(DefaultValue, out boolValue))
            {
                checkBox.Checked = boolValue;
            }

            // 绑定事件
            checkBox.CheckedChanged += (s, e) =>
            {
                DefaultValue = checkBox.Checked.ToString();
            };

            kryptonPanel1.Controls.Add(checkBox);
            checkBox.BringToFront();
            _dynamicDefaultValueControl = checkBox;
            ktxtDefaultValue.Visible = false;
        }

        /// <summary>
        /// 生成日期时间类型控件
        /// </summary>
        private void GenerateDateTimeControl()
        {
            var dateTimePicker = new KryptonDateTimePicker
            {
                Name = "dtpDynamicDefaultDateTime",
                ShowCheckBox = true,
                Width = 160,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm:ss",
                Value = DateTime.Now,
                Location = new System.Drawing.Point(120, 220),
                Size = new System.Drawing.Size(180, 23)
            };

            // 设置初始值
            if (DateTime.TryParse(DefaultValue, out DateTime dateTimeValue))
            {
                dateTimePicker.Value = dateTimeValue;
                dateTimePicker.Checked = true;
            }

            // 绑定事件
            dateTimePicker.ValueChanged += (s, e) =>
            {
                if (dateTimePicker.Checked)
                {
                    DefaultValue = dateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    DefaultValue = string.Empty;
                }
            };

            kryptonPanel1.Controls.Add(dateTimePicker);
            dateTimePicker.BringToFront();
            _dynamicDefaultValueControl = dateTimePicker;
            ktxtDefaultValue.Visible = false;
        }

        /// <summary>
        /// 生成枚举类型控件
        /// </summary>
        private void GenerateEnumControl(Type enumType)
        {
            var comboBox = new KryptonComboBox
            {
                Name = "cmbDynamicDefaultEnum",
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150,
                Location = new System.Drawing.Point(120, 220),
                Size = new System.Drawing.Size(280, 23)
            };

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
                comboBox.Items.Add(enumInfo);
            }

            // 设置初始值
            if (!string.IsNullOrEmpty(DefaultValue) && CurrentMapping?.EnumDefaultConfig != null)
            {
                // 尝试根据已保存的枚举值查找对应的项
                foreach (EnumItemInfo item in comboBox.Items)
                {
                    if (item.EnumValue == CurrentMapping.EnumDefaultConfig.EnumValue)
                    {
                        comboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(DefaultValue))
            {
                // 兼容旧版本：通过枚举名称查找
                try
                {
                    object enumValue = Enum.Parse(enumType, DefaultValue);
                    foreach (EnumItemInfo item in comboBox.Items)
                    {
                        if (item.EnumName == enumValue.ToString())
                        {
                            comboBox.SelectedItem = item;
                            break;
                        }
                    }
                }
                catch { }
            }

            // 绑定事件
            comboBox.SelectedIndexChanged += (s, e) =>
            {
                if (comboBox.SelectedItem is EnumItemInfo selectedInfo)
                {
                    // 更新DefaultValue为枚举名称
                    DefaultValue = selectedInfo.EnumName;
                }
            };

            kryptonPanel1.Controls.Add(comboBox);
            comboBox.BringToFront();
            _dynamicDefaultValueControl = comboBox;
            ktxtDefaultValue.Visible = false;
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
                    var descAttr = field.GetCustomAttributes(System.ComponentModel.DescriptionAttribute.class, false)
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
            var comboBox = new KryptonComboBox
            {
                Name = "cmbDynamicDefaultForeignKey",
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150,
                Location = new System.Drawing.Point(120, 220),
                Size = new System.Drawing.Size(280, 23)
            };

            try
            {
                if (fkAttr != null)
                {
                    // 获取关联表类型
                    string fkTableName = fkAttr.FKTableName;
                    Type fkEntityType = null;

                    // 从EntityTypeMappings中查找关联表类型
                    var mapping = UCBasicDataImport.EntityTypeMappings
                        .FirstOrDefault(m => m.Value.Name == fkTableName || m.Value.Name.Contains(fkTableName));

                    if (mapping.Value != null)
                    {
                        fkEntityType = mapping.Value;
                    }

                    if (fkEntityType != null)
                    {
                        // 使用UIGenerateHelper绑定外键数据
                        BindForeignKeyData(comboBox, fkEntityType, fkAttr);

                        // 设置初始值（如果有默认值）
                        if (!string.IsNullOrEmpty(DefaultValue) && comboBox.DataSource != null)
                        {
                            // 尝试通过ValueMember查找对应的项
                            string primaryKey = GetPrimaryKeyName(fkEntityType);
                            foreach (System.Data.DataRowView row in comboBox.Items)
                            {
                                if (row[primaryKey]?.ToString() == DefaultValue)
                                {
                                    comboBox.SelectedValue = row[primaryKey];
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        comboBox.Items.Add($"未找到关联表: {fkTableName}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"生成外键控件失败: {ex.Message}");
                comboBox.Items.Add($"加载失败: {ex.Message}");
            }

            // 绑定事件
            comboBox.SelectedIndexChanged += (s, e) =>
            {
                if (comboBox.SelectedIndex >= 0 && comboBox.SelectedItem != null)
                {
                    // 保存选中的值（ID）
                    var selectedValue = comboBox.SelectedValue;
                    if (selectedValue != null)
                    {
                        DefaultValue = selectedValue.ToString();
                    }
                }
            };

            kryptonPanel1.Controls.Add(comboBox);
            comboBox.BringToFront();
            _dynamicDefaultValueControl = comboBox;
            ktxtDefaultValue.Visible = false;
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
    }
}
