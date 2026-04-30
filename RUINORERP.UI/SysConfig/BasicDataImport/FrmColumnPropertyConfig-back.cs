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
        public SerializableKeyValuePair<string> ForeignKeySourceColumn { get; set; }

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
        public DefaultValueConfig EnumDefaultConfig { get; set; }

        /// <summary>
        /// 是否为图片列
        /// </summary>
        public bool IsImageColumn { get; set; }

        /// <summary>
        /// 图片列类型
        /// </summary>
        public ImageColumnType ImageColumnType { get; set; } = ImageColumnType.Path;

        /// <summary>
        /// 图片配置
        /// </summary>
        public ExcelImageConfig ImageConfig { get; set; }

        /// <summary>
        /// 数据源配置（统一接口）
        /// </summary>
        public IDataSourceConfig DataSourceConfig { get; set; }

        /// <summary>
        /// 是否为业务键字段
        /// </summary>
        public bool IsBusinessKey { get; set; }

        /// <summary>
        /// 数据库存在性处理策略（默认跳过）
        /// </summary>
        public ExistenceStrategy ExistenceStrategy { get; set; } = ExistenceStrategy.Skip;

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
                kcmbDataSourceType.Items.Add("Excel图片");
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
                // ✅ 初始化业务键配置
                kchkIsBusinessKey.Checked = CurrentMapping.IsBusinessKey;
                IsBusinessKey = CurrentMapping.IsBusinessKey;
                ExistenceStrategy = CurrentMapping.ExistenceStrategy;
                kcmbExistenceStrategy.SelectedIndex = (int)CurrentMapping.ExistenceStrategy;

                // 初始化数据来源类型
                kcmbDataSourceType.SelectedIndex = (int)CurrentMapping.DataSourceType;
                SelectedDataSourceType = CurrentMapping.DataSourceType;

                // 初始化图片配置（从统一配置接口读取）
                var imageConfig = CurrentMapping.DataSourceConfig as ExcelImageConfig;
                if (imageConfig != null)
                {
                    kcmbImageStorageType.SelectedIndex = (int)imageConfig.StorageType;
                    kcmbImageNamingRule.SelectedIndex = (int)imageConfig.NamingRule;
                    ktxtImageOutputDir.Text = imageConfig.OutputDirectory;
                    kcmbImageNamingColumn.SelectedItem = imageConfig.NamingReferenceColumn;
                }

                // 初始化系统生成配置（从统一配置接口读取）
                var sysConfig = CurrentMapping.DataSourceConfig as SystemGeneratedConfig;
                if (sysConfig != null)
                {
                    kcmbSystemGeneratedType.SelectedIndex = (int)sysConfig.GeneratedType;
                    ktxtDateTimeFormat.Text = sysConfig.DateTimeFormat;
                    ktxtBusinessCodePrefix.Text = sysConfig.BusinessCodePrefix;
                    kcmbBusinessCodeRule.SelectedIndex = (int)sysConfig.BusinessCodeRule;
                    ktxtSequenceDigits.Text = sysConfig.SequenceDigits.ToString();
                    ktxtCustomExpression.Text = sysConfig.CustomExpression;
                    ktxtCustomDefaultValue.Text = sysConfig.CustomDefaultValue;
                }

                // 初始化关联表信息（从统一配置接口读取）
                var foreignConfig = CurrentMapping.DataSourceConfig as ForeignKeyConfig;
                if (foreignConfig != null && !string.IsNullOrEmpty(foreignConfig.ForeignTableName))
                {
                    // 查找对应的显示文本
                    for (int i = 0; i < kcmbRelatedTable.Items.Count; i++)
                    {
                        string itemText = kcmbRelatedTable.Items[i].ToString();
                        if (itemText.Contains(foreignConfig.ForeignTableName))
                        {
                            kcmbRelatedTable.SelectedIndex = i;
                            break;
                        }
                    }
                    ktxtRelatedField.Text = foreignConfig.ForeignFieldDisplayName;
                }

                // 初始化外键来源列（从统一配置接口读取）
                if (foreignConfig != null && !string.IsNullOrEmpty(foreignConfig.DisplayFieldName))
                {
                    ForeignKeySourceColumn = new SerializableKeyValuePair<string>(foreignConfig.DisplayFieldName, foreignConfig.DisplayFieldName);
                }
                LoadForeignKeySourceColumns();
                if (ForeignKeySourceColumn != null && !string.IsNullOrEmpty(ForeignKeySourceColumn.Key))
                {
                    string sourceColumn = ForeignKeySourceColumn.Key;
                    // 查找匹配的项
                    for (int i = 0; i < kcmbForeignExcelSourceColumn.Items.Count; i++)
                    {
                        if (kcmbForeignExcelSourceColumn.Items[i].ToString() == sourceColumn ||
                            kcmbForeignExcelSourceColumn.Items[i].ToString().Contains($"({sourceColumn})"))
                        {
                            kcmbForeignExcelSourceColumn.SelectedIndex = i;
                            break;
                        }
                    }
                }

                // 初始化自身引用字段（从统一配置接口读取）
                var selfRefConfig = CurrentMapping.DataSourceConfig as SelfReferenceConfig;
                if (CurrentMapping.DataSourceType == DataSourceType.SelfReference && selfRefConfig != null)
                {
                    LoadSelfReferenceFields();
                    kcmbSelfReferenceField.SelectedItem = selfRefConfig.ReferenceFieldDisplayName;
                }

                // 初始化字段复制（从统一配置接口读取）
                var copyConfig = CurrentMapping.DataSourceConfig as FieldCopyConfig;
                if (CurrentMapping.DataSourceType == DataSourceType.FieldCopy && copyConfig != null)
                {
                    LoadCopyFromFields();
                    kcmbCopyFromField.SelectedItem = copyConfig.SourceFieldDisplayName;
                }

                // 初始化列拼接配置（从统一配置接口读取）
                var concatConfig = CurrentMapping.DataSourceConfig as ColumnConcatConfig;
                if (CurrentMapping.DataSourceType == DataSourceType.ColumnConcat && concatConfig != null)
                {
                    ConcatConfig = concatConfig;

                    // 加载Excel列列表（会自动选中已配置的列）
                    LoadConcatSourceColumns();

                    // 加载分隔符
                    ktxtSeparator.Text = concatConfig.Separator ?? string.Empty;

                    // 加载选项
                    kchkTrimWhitespace.Checked = concatConfig.TrimWhitespace;
                    kchkIgnoreEmptyColumns.Checked = concatConfig.IgnoreEmptyColumns;
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

            // 根据数据来源类型设置相关标志
            IsForeignKey = (dataSourceType == DataSourceType.ForeignKey);
            IsSystemGenerated = (dataSourceType == DataSourceType.SystemGenerated);

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
        
                // 如果有已选中的字段,保持选中状态
                if (!string.IsNullOrEmpty(ForeignKeyField?.Value))
                {
                    int index = ktxtRelatedField.Items.IndexOf(ForeignKeyField?.Value);
                    if (index >= 0)
                    {
                        ktxtRelatedField.SelectedIndex = index;
                    }
                }
        
                // 如果有已选中的字段,保持选中状态
                string selectedDisplayName = ForeignKeySourceColumn?.Value;
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

            // ✅ 构建临时映射对象用于验证
            var tempMapping = new ColumnMapping
            {
                SystemField = CurrentMapping?.SystemField,
                ExcelColumn = CurrentMapping?.ExcelColumn,
                DataSourceType = dataSourceType,
                IsRequired = CurrentMapping?.IsRequired ?? false,  // ✅ 从当前映射中获取
                IsUniqueValue = kchkIsUniqueValue.Checked,
                IgnoreEmptyValue = kchkIgnoreEmptyValue.Checked,
                DefaultValue = string.Empty,
                IsBusinessKey = kchkIsBusinessKey.Checked,
                ExistenceStrategy = (ExistenceStrategy)(kcmbExistenceStrategy?.SelectedIndex ?? 1)
            };

            // ✅ 使用验证适配器进行配置验证
            var validator = new ImportValidationAdapter();
            if (!validator.ValidateColumnMapping(tempMapping, out List<string> validationErrors))
            {
                string errorMsg = "列配置验证失败：\n" + string.Join("\n", validationErrors);
                MessageBox.Show(errorMsg, "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ 使用策略模式保存数据源配置
            SaveDataSourceConfig(dataSourceType);

            IsUniqueValue = kchkIsUniqueValue.Checked;
            IsImageColumn = kchkIsImageColumn.Checked;
            ImageColumnType = (ImageColumnType)kcmbImageColumnType.SelectedIndex;
            
            // ✅ 保存业务键配置
            IsBusinessKey = kchkIsBusinessKey.Checked;
            ExistenceStrategy = (ExistenceStrategy)(kcmbExistenceStrategy?.SelectedIndex ?? 1);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 使用策略模式保存数据源配置
        /// </summary>
        /// <param name="dataSourceType">数据源类型</param>
        private void SaveDataSourceConfig(DataSourceType dataSourceType)
        {
            var strategy = DataSourceConfigStrategyManager.GetStrategy(dataSourceType);
            if (strategy == null)
                return;

            // 使用策略验证配置
            if (!strategy.Validate(this, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "配置验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 使用策略保存配置
            DataSourceConfig = strategy.SaveToConfig(this);
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
        /// 业务键字段复选框点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kchkIsBusinessKey_CheckedChanged(object sender, EventArgs e)
        {
            // 当勾选业务键时，自动设置存在性策略为“跳过”
            if (kchkIsBusinessKey.Checked)
            {
                kcmbExistenceStrategy.SelectedIndex = 0; // Skip
            }
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
            kryptonTabControl.SelectedIndex = kcmbDataSourceType.SelectedIndex;

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
        
                // 如果有已选中的字段,保持选中状态
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
        
                // 如果有已选中的字段,保持选中状态
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
                string excelColumnName = ForeignKeySourceColumn?.Key ?? string.Empty;
                if (string.IsNullOrEmpty(excelColumnName))
                {
                    excelColumnName = kcmbForeignExcelSourceColumn.SelectedItem.ToString();
                }


                // 构建外键来源列配置
                ForeignKeySourceColumn = new SerializableKeyValuePair<string>
                {
                    Key = excelColumnName,
                    Value = field.Value
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
                if (enumType == null && !string.IsNullOrEmpty(CurrentMapping.EnumDefaultConfig?.EnumTypeName))
                {
                    enumType = AssemblyLoader.GetType("RUINORERP.Model", CurrentMapping.EnumDefaultConfig.EnumTypeName);
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
            bool boolValue = false;
            if (bool.TryParse(DefaultValue, out boolValue))
            {
                chkDynamicDefaultBool.Checked = boolValue;
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
            DefaultValue = chkDynamicDefaultBool.Checked.ToString();
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

            // 设置初始值
            if (DateTime.TryParse(DefaultValue, out DateTime dateTimeValue))
            {
                dtpDynamicDefaultDateTime.Value = dateTimeValue;
                dtpDynamicDefaultDateTime.Checked = true;
            }

            // 绑定事件（先解绑再绑定，避免重复）
            dtpDynamicDefaultDateTime.ValueChanged -= DtpDynamicDefaultDateTime_ValueChanged;
            dtpDynamicDefaultDateTime.ValueChanged += DtpDynamicDefaultDateTime_ValueChanged;
        }

        /// <summary>
        /// 日期时间控件值改变事件
        /// </summary>
        private void DtpDynamicDefaultDateTime_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDynamicDefaultDateTime.Checked)
            {
                DefaultValue = dtpDynamicDefaultDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                DefaultValue = string.Empty;
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

            // 设置初始值
            if (!string.IsNullOrEmpty(DefaultValue) && CurrentMapping?.EnumDefaultConfig != null)
            {
                // 尝试根据已保存的枚举值查找对应的项
                foreach (EnumItemInfo item in cmbDynamicDefaultEnum.Items)
                {
                    if (item.EnumValue == CurrentMapping.EnumDefaultConfig.EnumValue)
                    {
                        cmbDynamicDefaultEnum.SelectedItem = item;
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
                    foreach (EnumItemInfo item in cmbDynamicDefaultEnum.Items)
                    {
                        if (item.EnumName == enumValue.ToString())
                        {
                            cmbDynamicDefaultEnum.SelectedItem = item;
                            break;
                        }
                    }
                }
                catch { }
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
                // 更新DefaultValue为枚举名称
                DefaultValue = selectedInfo.EnumName;
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
        
                        // 设置初始值（如果有默认值）
                        if (!string.IsNullOrEmpty(DefaultValue) && cmbDynamicDefaultList.DataSource != null)
                        {
                            // 尝试通过 ValueMember 查找对应的项
                            string primaryKey = GetPrimaryKeyName(fkEntityType);
                            foreach (System.Data.DataRowView row in cmbDynamicDefaultList.Items)
                            {
                                if (row[primaryKey]?.ToString() == DefaultValue)
                                {
                                    cmbDynamicDefaultList.SelectedValue = row[primaryKey];
                                    break;
                                }
                            }
                        }
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
                    DefaultValue = selectedValue.ToString();
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
