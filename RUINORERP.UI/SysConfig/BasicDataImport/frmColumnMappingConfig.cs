using Krypton.Toolkit;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using RUINORERP.Global;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列映射配置窗体
    /// 用于配置Excel列与系统字段的映射关系
    /// </summary>
    public partial class frmColumnMappingConfig : KryptonForm
    {
        /// <summary>
        /// 映射配置文件路径
        /// </summary>
        private string _configFilePath;

        /// <summary>
        /// 列映射配置管理器
        /// </summary>
        private ColumnMappingManager _columnMappingManager;

        /// <summary>
        /// 目标实体类型
        /// </summary>
        public Type TargetEntityType { get; set; }

        /// <summary>
        /// Excel数据（用于预览）
        /// </summary>
        public DataTable ExcelData { get; set; }



        /// <summary>
        /// 是否为编辑模式
        /// </summary>
        public bool IsEditMode { get; set; }


        /// <summary>
        /// 映射配置保存事件
        /// </summary>
        public event EventHandler MappingSaved;

        /// <summary>
        /// 已保存的映射配置名称
        /// </summary>
        public string SavedMappingName { get; private set; }

        /// <summary>
        /// 导入配置对象（包含全局配置和列映射）
        /// </summary>
        public ImportConfiguration ImportConfig { get; set; }

        /// <summary>
        /// 原始配置的深拷贝（用于取消时恢复）
        /// </summary>
        private ImportConfiguration _originalConfigSnapshot;

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmColumnMappingConfig()
        {
            InitializeComponent();

            // 设计时跳过初始化逻辑
            if (DesignMode)
            {
                return;
            }

            ImportConfig = new ImportConfiguration();
            _columnMappingManager = new ColumnMappingManager();
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="entityType">目标实体类型</param>
        public frmColumnMappingConfig(Type entityType) : this()
        {
            TargetEntityType = entityType;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="entityType">目标实体类型</param>
        /// <param name="excelData">Excel数据</param>
        public frmColumnMappingConfig(Type entityType, DataTable excelData) : this(entityType)
        {
            ExcelData = excelData;
        }

        /// <summary>
        /// 系统字段列表双击事件 - 直接添加映射
        /// </summary>
        private void listBoxSystemFields_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxSystemFields.SelectedItem == null)
            {
                return;
            }

            // 获取选中的系统字段（从KeyValuePair中获取Key和Value）
            if (!(listBoxSystemFields.SelectedItem is KeyValuePair<string, string> selectedItem))
            {
                return;
            }
            string systemField = selectedItem.Key; // 英文字段名
            string systemFieldDisplay = selectedItem.Value; // 中文显示名

            // 检查是否已存在该系统字段的映射
            var existingMapping = ImportConfig.ColumnMappings.GetMappingBySystemField(systemField);
            if (existingMapping != null)
            {
                MessageBox.Show($"系统字段 \"{systemFieldDisplay}\" 已被映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取用户选择的Excel列（可选）
            string excelColumn = listBoxExcelColumns.SelectedItem?.ToString();

            // 如果选择了Excel列，检查是否已被映射
            if (!string.IsNullOrEmpty(excelColumn))
            {
                if (ImportConfig.ColumnMappings.GetMappingByExcelColumn(excelColumn) != null)
                {
                    MessageBox.Show($"Excel列 \"{excelColumn}\" 已被映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 直接创建并添加映射
            CreateAndConfigureMapping(systemFieldDisplay, systemField, excelColumn);
        }

        /// <summary>
        /// 创建并配置新映射
        /// </summary>
        /// <param name="systemField">系统字段名</param>
        /// <param name="excelColumn">Excel列名(可选)</param>
        private void CreateAndConfigureMapping(string systemFieldDisplay, string systemField, string excelColumn)
        {
            // 验证:如果两边都选择了,必须都选择
            bool hasExcelColumn = !string.IsNullOrEmpty(excelColumn);
            bool hasSystemField = !string.IsNullOrEmpty(systemField);

            if (hasExcelColumn && hasSystemField)
            {
                // 两边都选择了,创建映射
                var mapping = new ColumnMapping
                {
                    OriginalExcelColumn = excelColumn,
                    SystemField = new SerializableKeyValuePair<string>(systemField, systemFieldDisplay),
                    ColumnDataSourceType = (int)DataSourceType.Excel,
                    DataSourceConfig = new ExcelConfig { ExcelColumn = excelColumn }
                };

                // 先添加到集合
                ImportConfig.ColumnMappings.Add(mapping);

                // 从两个列表中移除已选择的项
                RemoveFromExcelColumns(excelColumn);
                RemoveFromSystemFields(systemField);

                // 更新映射列表显示
                UpdateMappingsList();

                // 选中新添加的映射项
                SelectNewlyAddedMapping(mapping);

                // 打开属性配置对话框
                ConfigureMappingProperty(mapping);
            }
            else if (!hasExcelColumn && hasSystemField)
            {
                // 只选择了系统字段,Excel中没有指定列
                // 创建新映射               
                var mapping = new ColumnMapping
                {
                    OriginalExcelColumn = string.Empty,
                    SystemField = new SerializableKeyValuePair<string>(systemField, systemFieldDisplay),
                    ColumnDataSourceType = (int)DataSourceType.DefaultFixedValue,
                    DataSourceConfig = new DefaultValueConfig()
                };

                // 先添加到集合
                ImportConfig.ColumnMappings.Add(mapping);

                // 从系统字段列表中移除
                RemoveFromSystemFields(systemField);

                // 更新映射列表显示
                UpdateMappingsList();

                // 选中新添加的映射项
                SelectNewlyAddedMapping(mapping);

                // 打开属性配置对话框
                ConfigureMappingProperty(mapping);

                // 验证:如果用户没有修改数据来源类型,且是Excel数据源但没有Excel列,则删除
                if (mapping.ColumnDataSourceType == (int)DataSourceType.Excel)
                {
                    MessageBox.Show("由于没有选择Excel来源列,不能选择\"Excel数据源\"类型。请选择:默认值、系统生成、外键关联、自身字段引用、字段复制、列拼接或Excel图片。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // 删除刚添加的映射
                    ImportConfig.ColumnMappings.RemoveAt(ImportConfig.ColumnMappings.Count - 1);
                    RestoreSystemField(mapping);
                    UpdateMappingsList();
                    return;
                }

                // ✅ 只有当用户没有选择原始Excel列时,才根据数据来源类型设置显示文本
                // 如果用户已经选择了Excel列,则保留原始值
                if (!hasExcelColumn)
                {
                    switch (mapping.ColumnDataSourceType)
                    {
                        case (int)DataSourceType.SystemGenerated:
                            var sysConfig = mapping.DataSourceConfig as SystemGeneratedConfig;
                            mapping.OriginalExcelColumn = $"[系统生成] {systemField}";
                            break;
                        case (int)DataSourceType.DefaultFixedValue:
                            var defaultConfig = mapping.DataSourceConfig as DefaultValueConfig;
                            mapping.OriginalExcelColumn = $"[默认值:{defaultConfig?.Value}] {systemField}";
                            break;
                        case (int)DataSourceType.ForeignKey:
                            var dbRefConfig = mapping.DataSourceConfig as DatabaseReferenceConfig;
                            if (dbRefConfig != null)
                            {
                                string refType = dbRefConfig.IsSelfReference ? "自身表" : "外键";
                                string fieldDisplayName = GetFieldDisplayName(dbRefConfig.ForeignTableName, dbRefConfig.ForeignFieldName);
                                mapping.OriginalExcelColumn = $"[{refType}关联:{fieldDisplayName}] {systemField}";
                            }
                            break;
                        case (int)DataSourceType.ColumnConcat:
                            var concatConfig = mapping.DataSourceConfig as ColumnConcatConfig;
                            string concatCols = string.Join("+", concatConfig?.SourceColumns ?? new List<string>());
                            mapping.OriginalExcelColumn = $"[列拼接:{concatCols}] {systemField}";
                            break;
                        case (int)DataSourceType.ExcelImage:
                            var imageConfig = mapping.DataSourceConfig as ExcelImageConfig;
                            string namingRule = imageConfig?.NamingRule.ToString() ?? "AutoIncrement";
                            mapping.OriginalExcelColumn = $"[Excel图片:{namingRule}] {systemField}";
                            break;
                    }
                }

                // 更新映射列表显示(因为OriginalExcelColumn可能已更新)
                UpdateMappingsList();
            }
            else
            {
                MessageBox.Show("必须选择系统字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 配置映射属性
        /// </summary>
        /// <param name="mapping">要配置的映射</param>
        /// <returns>是否确认配置</returns>
        private bool ConfigureMappingProperty(ColumnMapping mapping)
        {
            using (var propertyDialog = new FrmColumnPropertyConfig(MainForm.Instance.AppContext.Db)
            {
                CurrentMapping = mapping,
                TargetEntityType = TargetEntityType,
                ExcelColumns = GetExcelColumnsList()
            })
            {
                if (propertyDialog.ShowDialog() == DialogResult.OK)
                {
                    // ✅ 保存统一的数据源配置接口（唯一数据源）
                    mapping.DataSourceConfig = propertyDialog.DataSourceConfig;
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取Excel列列表
        /// </summary>
        /// <returns>Excel列名列表</returns>
        private List<string> GetExcelColumnsList()
        {
            var columns = new List<string>();
            if (ExcelData != null)
            {
                foreach (DataColumn col in ExcelData.Columns)
                {
                    columns.Add(col.ColumnName);
                }
            }
            return columns;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void frmColumnMappingConfig_Load(object sender, EventArgs e)
        {
            try
            {
                // ✅ 注册窗体关闭事件（处理用户点击X按钮的情况）
                this.FormClosing += frmColumnMappingConfig_FormClosing;

                // 绑定配置名称到 ImportConfig（双向绑定）
                BindMappingName();

                // 绑定去重配置到 ImportConfig（双向绑定）
                BindDeduplicateConfig();

                // 绑定存在性策略到 ImportConfig（双向绑定）
                BindExistenceConfig();

                // 初始化去重策略ComboBox默认值
                kcmbDeduplicateStrategy.SelectedIndex = 0;

                // 初始化存在性策略ComboBox默认值
                kcmbExistenceStrategy.SelectedIndex = 0;
                // 设置配置文件路径
                _configFilePath = ColumnMappingConstants.GetConfigFilePath();

                // 确保目录存在
                if (!Directory.Exists(_configFilePath))
                {
                    Directory.CreateDirectory(_configFilePath);
                }

                // 加载系统字段
                LoadSystemFields();

                // 如果有Excel数据，加载Excel列（只要有列定义就加载，不要求有数据行）
                if (ExcelData != null && ExcelData.Columns.Count > 0)
                {
                    LoadExcelColumns();
                }

                // 加载已保存的映射配置
                LoadSavedMappings();

                // 设置窗口标题
                SetWindowTitle();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化窗体失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体关闭事件（处理用户点击X按钮的情况）
        /// </summary>
        private void frmColumnMappingConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ✅ 如果是编辑模式且用户取消了操作，恢复原始配置状态
            if (IsEditMode && _originalConfigSnapshot != null && this.DialogResult != DialogResult.OK)
            {
                RestoreOriginalConfiguration();
            }
        }

        /// <summary>
        /// 重新初始化所有绑定和列表（当 ImportConfig 改变时调用）
        /// </summary>
        private void RebindAll()
        {
            // 重新绑定所有配置
            BindMappingName();
            BindDeduplicateConfig();
            BindExistenceConfig();

            // 重新加载字段列表
            LoadSystemFields();
            if (ExcelData != null && ExcelData.Columns.Count > 0)
            {
                LoadExcelColumns();
            }

            // 更新映射列表显示
            UpdateMappingsList();
        }

        /// <summary>
        /// 重置为新的映射配置
        /// </summary>
        private void ResetToNewConfig()
        {
            // ✅ 清空当前配置的列映射（因为双向绑定，需要清理）
            ImportConfig.ColumnMappings.Clear();
            
            // 创建新的配置对象
            ImportConfig = new ImportConfiguration();
            IsEditMode = false;
            
            // ✅ 清除原始配置快照（因为是新建模式）
            _originalConfigSnapshot = null;

            RebindAll();

            // 重置已保存配置选择
            comboBoxSavedMappings.SelectedIndex = 0;

            // 更新窗口标题
            SetWindowTitle();
        }

        /// <summary>
        /// 加载已保存的配置对象
        /// </summary>
        private void LoadConfig(ImportConfiguration config)
        {
            if (config == null)
            {
                MessageBox.Show("加载配置失败: 配置数据为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ 创建原始配置的深拷贝快照（用于取消时恢复）
            _originalConfigSnapshot = CloneConfiguration(config);

            ImportConfig = config;


            RebindAll();

            // 显示去重字段配置状态
            if (config.EnableDeduplication && config.DeduplicateFields != null && config.DeduplicateFields.Count > 0)
            {
                MainForm.Instance.PrintInfoLog($"加载配置：已配置 {config.DeduplicateFields.Count} 个去重字段");
            }

            // 从列表中移除已映射的项
            foreach (var mapping in ImportConfig.ColumnMappings)
            {
                // 只有真正的Excel列才需要从可用列表中移除
                if (!string.IsNullOrEmpty(mapping.OriginalExcelColumn) &&
                    !mapping.OriginalExcelColumn.StartsWith("[") &&
                    !mapping.OriginalExcelColumn.Contains("]"))
                {
                    RemoveFromExcelColumns(mapping.OriginalExcelColumn);
                }

                // 从系统字段列表中移除
                if (mapping.SystemField != null && !string.IsNullOrEmpty(mapping.SystemField.Key))
                {
                    RemoveFromSystemFields(mapping.SystemField.Key);
                }
            }

            // 更新映射列表显示
            UpdateMappingsList();
        }

        /// <summary>
        /// 绑定配置名称到 ImportConfig（双向绑定）
        /// </summary>
        private void BindMappingName()
        {
            DataBindingHelper.BindData4TextBox<ImportConfiguration>(
                ImportConfig,
                nameof(ImportConfig.MappingName),
                textBoxMappingName,
                BindDataType4TextBox.Text,
                false);
        }

        /// <summary>
        /// 绑定去重配置到 ImportConfig（双向绑定）
        /// </summary>
        private void BindDeduplicateConfig()
        {
            // 绑定启用去重复选框
            DataBindingHelper.BindData4CheckBox<ImportConfiguration>(
                ImportConfig,
                nameof(ImportConfig.EnableDeduplication),
                chkRemoveDuplicates,
                false);

            // 绑定去重策略ComboBox
            DataBindingHelper.BindData4CmbByEnum<ImportConfiguration>(
                ImportConfig,
                nameof(ImportConfig.DeduplicateStrategy),
                typeof(DeduplicateStrategyType),
                kcmbDeduplicateStrategy,
                false);
        }

        /// <summary>
        /// 绑定存在性策略到 ImportConfig（双向绑定）
        /// </summary>
        private void BindExistenceConfig()
        {
            // 使用 DataBindingHelper 绑定存在性策略 ComboBox
            DataBindingHelper.BindData4CmbByEnum<ImportConfiguration, ExistenceStrategyType>(
                ImportConfig,
                nameof(ImportConfig.ExistenceStrategy),
                kcmbExistenceStrategy,
                false);
        }

        /// <summary>
        /// 设置窗口标题
        /// </summary>
        private void SetWindowTitle()
        {
            if (IsEditMode && !string.IsNullOrEmpty(ImportConfig.MappingName))
            {
                this.Text = $"编辑列映射配置 - {ImportConfig.MappingName}";
            }
            else
            {
                this.Text = "新建列映射配置";
            }
        }

        /// <summary>
        /// 加载系统字段
        /// </summary>
        private void LoadSystemFields()
        {
            try
            {
                if (TargetEntityType == null)
                {
                    MessageBox.Show("未设置目标实体类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 获取字段信息
                FieldNameList = UIHelper.GetFieldNameList(false, TargetEntityType);

                // 获取该实体类型的预设字段（在导入时会自动填充默认值的字段）
                var predefinedFields = EntityImportHelper.GetPredefinedFields(TargetEntityType);

                // 清空并添加字段到列表框（使用KeyValuePair存储Key和Value）
                listBoxSystemFields.Items.Clear();
                listBoxSystemFields.DisplayMember = "Value";
                listBoxSystemFields.ValueMember = "Key";
                foreach (var field in FieldNameList)
                {
                    // 跳过预设字段，这些字段在导入时会自动填充
                    if (predefinedFields.Contains(field.Key))
                    {
                        continue;
                    }

                    // 检查是否为必填字段
                    bool isRequired = IsFieldRequired(field.Key);
                    string displayText = isRequired ? $"* {field.Value}" : field.Value;
                    listBoxSystemFields.Items.Add(new KeyValuePair<string, string>(field.Key, displayText));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载系统字段失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 检查字段是否必填
        /// 通过FluentValidation验证器判断字段是否必填
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns>是否必填</returns>
        private bool IsFieldRequired(string fieldName)
        {
            try
            {
                if (TargetEntityType == null || string.IsNullOrEmpty(fieldName))
                {
                    return false;
                }

                // 获取属性
                PropertyInfo property = TargetEntityType.GetProperty(fieldName);
                if (property == null)
                {
                    return false;
                }

                // 首先检查SugarColumnAttribute的IsNullable
                bool isNullable = true;
                var sugarColumnAttribute = property.GetCustomAttributes(false)
                    .FirstOrDefault(attr => attr.GetType().Name == "SugarColumnAttribute");

                if (sugarColumnAttribute != null)
                {
                    try
                    {
                        var isNullableProperty = sugarColumnAttribute.GetType().GetProperty("IsNullable");
                        if (isNullableProperty != null)
                        {
                            isNullable = (bool)isNullableProperty.GetValue(sugarColumnAttribute);
                        }
                    }
                    catch
                    {
                        // 如果获取失败，继续使用验证器检查
                    }
                }

                // 如果IsNullable为false，则必填
                if (!isNullable)
                {
                    return true;
                }

                // 尝试获取验证器并检查必填规则
                try
                {
                    // 获取验证器类型名：实体名 + "Validator"
                    string validatorTypeName = TargetEntityType.Name + "Validator";
                    string fullTypeName = $"RUINORERP.Business.{validatorTypeName}";

                    // 使用AssemblyLoader工具类加载程序集
                    Type validatorType = AssemblyLoader.GetType("RUINORERP.Business", fullTypeName);

                    if (validatorType != null && MainForm.Instance?.AppContext != null)
                    {
                        // 从依赖注入容器获取验证器实例
                        object validator = MainForm.Instance.AppContext.GetRequiredService(validatorType);

                        if (validator != null)
                        {
                            // 获取验证器的Type
                            Type validatorInterface = validator.GetType();

                            // 获取GetEnumerator方法来遍历验证规则
                            var enumeratorMethod = validatorInterface.GetMethod("GetEnumerator");
                            if (enumeratorMethod != null)
                            {
                                var enumerator = enumeratorMethod.Invoke(validator, null);
                                if (enumerator != null)
                                {
                                    var moveNextMethod = enumerator.GetType().GetMethod("MoveNext");
                                    var getCurrentMethod = enumerator.GetType().GetProperty("Current");

                                    // 遍历所有验证规则
                                    while ((bool)moveNextMethod.Invoke(enumerator, null))
                                    {
                                        var currentRule = getCurrentMethod.GetValue(enumerator);
                                        if (currentRule != null)
                                        {
                                            // 获取PropertyName
                                            var propertyNameProp = currentRule.GetType().GetProperty("PropertyName");
                                            if (propertyNameProp != null)
                                            {
                                                string rulePropertyName = propertyNameProp.GetValue(currentRule) as string;
                                                if (rulePropertyName == fieldName)
                                                {
                                                    // 获取Components
                                                    var componentsProp = currentRule.GetType().GetProperty("Components");
                                                    if (componentsProp != null)
                                                    {
                                                        var components = componentsProp.GetValue(currentRule);
                                                        if (components != null)
                                                        {
                                                            foreach (var component in (System.Collections.IEnumerable)components)
                                                            {
                                                                // 检查是否为必填验证器
                                                                if (IsRequiredValidator(component, fieldName, TargetEntityType))
                                                                {
                                                                    return true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // 如果验证器检查失败，回退到属性检查
                }

                // 最后检查RequiredAttribute
                bool hasRequiredAttribute = property.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;
                return hasRequiredAttribute;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断验证器组件是否为必填验证器
        /// 参考BaseEditGeneric中的实现
        /// </summary>
        /// <param name="component">验证器组件</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>如果是必填验证器返回true，否则返回false</returns>
        private bool IsRequiredValidator(object component, string propertyName, Type entityType)
        {
            try
            {
                if (component == null)
                {
                    return false;
                }

                // 获取Validator属性
                var validatorProp = component.GetType().GetProperty("Validator");
                if (validatorProp == null)
                {
                    return false;
                }

                var validator = validatorProp.GetValue(component);
                if (validator == null)
                {
                    return false;
                }

                // 获取Validator.Name
                var nameProp = validator.GetType().GetProperty("Name");
                if (nameProp == null)
                {
                    return false;
                }

                string validatorName = nameProp.GetValue(validator) as string;
                if (string.IsNullOrEmpty(validatorName))
                {
                    return false;
                }

                // 检查常见的必填验证器类型
                switch (validatorName)
                {
                    case "NotEmptyValidator":
                    case "NotNullValidator":
                        return true;

                    case "PredicateValidator":
                        // 对于外键验证器，需要检查属性是否为可空类型
                        return IsRequiredForeignKeyValidator(component, propertyName, entityType);

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断验证器组件是否为必填的外键验证器
        /// 参考BaseEditGeneric中的实现
        /// </summary>
        /// <param name="component">验证器组件</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>如果是必填的外键验证器返回true，否则返回false</returns>
        private bool IsRequiredForeignKeyValidator(object component, string propertyName, Type entityType)
        {
            try
            {
                // 检查是否是PredicateValidator（用于Must方法）
                var validatorProp = component.GetType().GetProperty("Validator");
                var validator = validatorProp?.GetValue(component);
                var nameProp = validator?.GetType().GetProperty("Name");
                string validatorName = nameProp?.GetValue(validator) as string;

                if (validatorName != "PredicateValidator")
                {
                    return false;
                }

                // 获取属性信息
                var propertyInfo = entityType.GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    return false;
                }

                // 检查属性是否有FKRelationAttribute特性，判断是否为外键
                var fkAttr = propertyInfo.GetCustomAttribute<FKRelationAttribute>(false);
                if (fkAttr == null)
                {
                    return false; // 不是外键
                }

                // 检查属性类型是否为非空类型（非可空类型）
                // 如果是值类型且不是可空类型，则为必填
                if (propertyInfo.PropertyType.IsValueType)
                {
                    // 检查是否是可空值类型
                    var underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    // 如果underlyingType为null，表示不是可空类型，即为必填
                    return underlyingType == null;
                }

                // 对于引用类型，通常认为是可空的
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 加载Excel列
        /// </summary>
        private void LoadExcelColumns()
        {
            try
            {
                listBoxExcelColumns.Items.Clear();
                if (ExcelData != null)
                {
                    foreach (DataColumn column in ExcelData.Columns)
                    {
                        listBoxExcelColumns.Items.Add(column.ColumnName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载Excel列失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载已保存的映射配置
        /// </summary>
        private void LoadSavedMappings()
        {
            try
            {
                // 保存当前选择
                string currentSelection = comboBoxSavedMappings.SelectedItem?.ToString();

                // 临时取消事件订阅，避免设置 SelectedIndex 时触发重复绑定
                comboBoxSavedMappings.SelectedIndexChanged -= comboBoxSavedMappings_SelectedIndexChanged;

                comboBoxSavedMappings.Items.Clear();
                comboBoxSavedMappings.Items.Add("-- 新建映射 --");

                // 直接加载所有配置（添加"[配置]"前缀以保持与主界面一致）
                var savedMappings = _columnMappingManager.GetAllMappingNames();
                foreach (var mappingName in savedMappings)
                {
                    comboBoxSavedMappings.Items.Add($"[配置] {mappingName}");
                }

                // 恢复选择
                if (!string.IsNullOrEmpty(currentSelection))
                {
                    int index = comboBoxSavedMappings.Items.IndexOf(currentSelection);
                    if (index >= 0)
                        comboBoxSavedMappings.SelectedIndex = index;
                    else
                        comboBoxSavedMappings.SelectedIndex = 0;
                }
                else
                {
                    comboBoxSavedMappings.SelectedIndex = 0;
                }

                // 重新订阅事件
                comboBoxSavedMappings.SelectedIndexChanged += comboBoxSavedMappings_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载已保存的映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 列的显示，unitName,<单位,true>
        /// 列名，列中文，是否显示
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public ConcurrentDictionary<string, string> FieldNameList { get; set; } = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 获取字段的中文显示名称
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名（英文）</param>
        /// <returns>中文显示名称</returns>
        private string GetFieldDisplayName(string tableName, string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return fieldName;

            try
            {
                Type entityType = null;
                if (UCBasicDataImport.EntityTypeMappings != null)
                {
                    foreach (var mapping in UCBasicDataImport.EntityTypeMappings)
                    {
                        if (mapping.Value.Name == tableName)
                        {
                            entityType = mapping.Value;
                            break;
                        }
                    }
                }

                if (entityType != null)
                {
                    var propertyInfo = entityType.GetProperty(fieldName);
                    if (propertyInfo != null)
                    {
                        var descAttr = propertyInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
                        if (descAttr != null)
                        {
                            return descAttr.Description;
                        }

                        var advAttr = propertyInfo.GetCustomAttribute<RUINORERP.Global.CustomAttribute.AdvQueryAttribute>();
                        if (advAttr != null)
                        {
                            return advAttr.ColDesc;
                        }
                    }
                }
            }
            catch
            {
                // 忽略反射异常
            }

            return fieldName;
        }

        /// <summary>
        /// 添加映射
        /// </summary>
        private void kbtnAddMapping_Click(object sender, EventArgs e)
        {
            // 验证：必须选择系统字段
            if (listBoxSystemFields.SelectedItem == null)
            {
                MessageBox.Show("必须选择系统字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 从KeyValuePair中获取Key和Value
            if (!(listBoxSystemFields.SelectedItem is KeyValuePair<string, string> selectedItem))
            {
                MessageBox.Show("必须选择系统字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string systemField = selectedItem.Key; // 英文字段名
            string systemFieldDisplay = selectedItem.Value; // 中文显示名

            // 检查是否已存在该系统字段的映射
            if (ImportConfig.ColumnMappings.GetMappingBySystemField(systemField) != null)
            {
                MessageBox.Show($"系统字段 \"{systemFieldDisplay}\" 已被映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取用户选择的Excel列
            string excelColumn = listBoxExcelColumns.SelectedItem?.ToString();

            // 如果选择了Excel列，检查是否已被映射
            if (!string.IsNullOrEmpty(excelColumn))
            {
                if (ImportConfig.ColumnMappings.GetMappingByExcelColumn(excelColumn) != null)
                {
                    MessageBox.Show($"Excel列 \"{excelColumn}\" 已被映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 直接创建并配置映射（无论是否有Excel列）
            CreateAndConfigureMapping(systemFieldDisplay, systemField, excelColumn);
        }

        /// <summary>
        /// 存储匹配结果的置信度信息（用于颜色标识）
        /// </summary>
        private Dictionary<string, double> _matchConfidenceScores = new Dictionary<string, double>();

        /// <summary>
        /// 更新映射列表显示
        /// </summary>
        private void UpdateMappingsList()
        {
            listBoxMappings.Items.Clear();

            foreach (var mapping in ImportConfig.ColumnMappings)
            {
                string displayText = $"{mapping.OriginalExcelColumn} -> {mapping.SystemField?.Value}";
                string key = $"{mapping.OriginalExcelColumn}->{mapping.SystemField?.Key}";

                // 添加属性标识
                List<string> flags = new List<string>();

                // 根据数据来源类型添加标识
                switch (mapping.ColumnDataSourceType)
                {
                    case (int)DataSourceType.Excel:
                        // Excel数据源，显示Excel列名
                        var excelConfig = mapping.DataSourceConfig as ExcelConfig;
                        if (excelConfig != null && !string.IsNullOrEmpty(excelConfig.ExcelColumn))
                        {
                            flags.Add($"Excel列:{excelConfig.ExcelColumn}");
                        }
                        break;
                    case (int)DataSourceType.DefaultFixedValue:
                        var defaultConfig = mapping.DataSourceConfig as DefaultValueConfig;
                        flags.Add($"默认值:{defaultConfig?.Value}");
                        break;
                    case (int)DataSourceType.SystemGenerated:
                        flags.Add("系统生成");
                        break;
                    case (int)DataSourceType.ForeignKey:
                        var dbRefConfig = mapping.DataSourceConfig as DatabaseReferenceConfig;
                        if (dbRefConfig != null)
                        {
                            string refType = dbRefConfig.IsSelfReference ? "自身表" : "外键";
                            string fieldDisplayName = GetFieldDisplayName(dbRefConfig.ForeignTableName, dbRefConfig.ForeignFieldName);
                            flags.Add($"{refType}关联:{fieldDisplayName}");
                        }
                        break;
                    case (int)DataSourceType.ColumnConcat:
                        var concatConfig = mapping.DataSourceConfig as ColumnConcatConfig;
                        string concatCols = string.Join("+", concatConfig?.SourceColumns ?? new List<string>());
                        flags.Add($"列拼接:{concatCols}");
                        break;
                }

                if (mapping.IsUniqueValue) flags.Add("唯一");

                if (flags.Count > 0)
                {
                    displayText += $" ({string.Join(", ", flags)})";
                }

                listBoxMappings.Items.Add(displayText);
            }
        }

        /// <summary>
        /// 设置匹配置信度分数（供智能匹配后调用）
        /// </summary>
        public void SetMatchConfidenceScores(Dictionary<string, double> scores)
        {
            _matchConfidenceScores = scores ?? new Dictionary<string, double>();
        }

        /// <summary>
        /// 删除映射
        /// </summary>
        private void kbtnRemoveMapping_Click(object sender, EventArgs e)
        {
            if (listBoxMappings.SelectedItem == null)
            {
                MessageBox.Show("请选择要删除的映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = listBoxMappings.SelectedIndex;
            var mapping = ImportConfig.ColumnMappings[selectedIndex];

            // 删除前，将Excel列和系统字段恢复到对应的列表中
            RestoreExcelColumn(mapping);
            RestoreSystemField(mapping);

            // 从集合中删除
            ImportConfig.ColumnMappings.RemoveAt(selectedIndex);
            UpdateMappingsList();
        }

        /// <summary>
        /// 设置列属性
        /// </summary>
        private void kbtnSetColumnProperty_Click(object sender, EventArgs e)
        {
            OpenPropertyDialog();
        }

        /// <summary>
        /// 双击映射列表 - 打开属性配置
        /// </summary>
        private void listBoxMappings_DoubleClick(object sender, EventArgs e)
        {
            OpenPropertyDialog();
        }

        /// <summary>
        /// 打开属性配置对话框
        /// 是对指定的目标表字段进行属性配置，不是对映射好后面的列来配置
        /// </summary>
        private void OpenPropertyDialog()
        {
            var mapping = GetSelectedMapping();
            if (mapping == null)
            {
                MessageBox.Show("请选择要配置的映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 使用统一的配置方法
            if (ConfigureMappingProperty(mapping))
            {
                // 更新显示
                UpdateMappingsList();
            }
        }

        /// <summary>
        /// 获取当前选中的映射
        /// </summary>
        /// <returns>选中的映射，如果没有选中则返回null</returns>
        private ColumnMapping GetSelectedMapping()
        {
            if (listBoxMappings.SelectedItem == null)
            {
                return null;
            }

            int selectedIndex = listBoxMappings.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= ImportConfig.ColumnMappings.Count)
            {
                return null;
            }

            return ImportConfig.ColumnMappings[selectedIndex];
        }

        /// <summary>
        /// 保存映射配置
        /// </summary>
        private void kbtnSaveMapping_Click(object sender, EventArgs e)
        {



            // 验证去重配置
            if (ImportConfig.EnableDeduplication)
            {
                if (kcmbDeduplicateStrategy.SelectedIndex < 0)
                {
                    MessageBox.Show("已启用去重功能，请选择去重策略（保留第一条记录或保留最后一条记录）",
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    kcmbDeduplicateStrategy.Focus();
                    return;
                }
            }

            try
            {
                // 使用验证适配器进行配置验证
                var validator = new ImportValidationAdapter();
                if (!validator.ValidateImportConfiguration(ImportConfig, out List<string> validationErrors))
                {
                    string errorMsg = "导入配置验证失败：\n" + string.Join("\n", validationErrors);
                    MessageBox.Show(errorMsg, "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 更新全局配置（去重配置和名称已通过双向绑定自动同步）
                ImportConfig.EntityType = TargetEntityType?.Name;
                ImportConfig.ColumnMappings = ImportConfig.ColumnMappings.ToList();
                ImportConfig.UpdateTimestamp();

                // 保存配置
                _columnMappingManager ??= new ColumnMappingManager();
                _columnMappingManager.SaveConfiguration(ImportConfig);

                SavedMappingName = ImportConfig.MappingName;
                MessageBox.Show("映射配置保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 触发保存事件
                MappingSaved?.Invoke(this, EventArgs.Empty);

                // 刷新已保存映射列表
                LoadSavedMappings();

                // 【修复】选中新保存的映射（带"[配置]"前缀）
                string newItemText = $"[配置] {ImportConfig.MappingName}";
                int newIndex = comboBoxSavedMappings.Items.IndexOf(newItemText);
                if (newIndex >= 0)
                {
                    comboBoxSavedMappings.SelectedIndex = newIndex;
                }

                // ✅ 保存成功后，清除原始配置快照（因为已保存）
                _originalConfigSnapshot = null;

                // 关闭窗体
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载已保存的映射配置或模板
        /// </summary>
        private void comboBoxSavedMappings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSavedMappings.SelectedIndex <= 0)
            {
                // 新建映射 - 重置为新的配置
                ResetToNewConfig();
                return;
            }

            string selectedItem = comboBoxSavedMappings.SelectedItem.ToString();

            try
            {
                // 提取真实的配置名称(去除前缀)
                string mappingName = selectedItem;
                if (selectedItem.StartsWith("[配置] "))
                {
                    mappingName = selectedItem.Substring(5);
                }

                _columnMappingManager ??= new ColumnMappingManager();
                var config = _columnMappingManager.LoadConfiguration(mappingName, TargetEntityType);

                // 加载配置对象并重新绑定
                LoadConfig(config);

                IsEditMode = true;
                SetWindowTitle();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void kbtnCancel_Click(object sender, EventArgs e)
        {
            // ✅ 如果是编辑模式且用户取消了操作，恢复原始配置状态
            if (IsEditMode && _originalConfigSnapshot != null)
            {
                RestoreOriginalConfiguration();
            }

            this.Close();
        }

        /// <summary>
        /// 自动匹配映射
        /// </summary>
        private void kbtnAutoMatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxExcelColumns.Items.Count == 0 || listBoxSystemFields.Items.Count == 0)
                {
                    MessageBox.Show("没有可匹配的列或字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 【优化】使用 SmartColumnMatcher 进行智能匹配
                var matcher = new SmartColumnMatcher(MainForm.Instance.AppContext.Db);

                // 获取 Excel 列列表
                var excelColumns = listBoxExcelColumns.Items.Cast<object>()
                    .Select(item => item.ToString())
                    .ToList();

                // 执行智能匹配
                var matchResults = matcher.MatchColumns(excelColumns, TargetEntityType);

                if (matchResults.Count == 0)
                {
                    MessageBox.Show("未找到匹配的列，请手动配置映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 清空现有映射
                ImportConfig.ColumnMappings.Clear();

                // 【优化】保存匹配置信度分数
                var confidenceScores = new Dictionary<string, double>();

                // 根据匹配结果创建映射
                foreach (var result in matchResults)
                {
                    // 查找系统字段显示名称（从KeyValuePair中获取）
                    string systemFieldDisplay = null;
                    foreach (var item in listBoxSystemFields.Items)
                    {
                        if (item is KeyValuePair<string, string> kvp && kvp.Key == result.DbColumn)
                        {
                            systemFieldDisplay = kvp.Value;
                            break;
                        }
                    }

                    if (systemFieldDisplay == null)
                        continue;

                    // 创建映射
                    var mapping = new ColumnMapping
                    {
                        OriginalExcelColumn = result.ExcelColumn,
                        SystemField = new SerializableKeyValuePair<string>(result.DbColumn, systemFieldDisplay),
                        IsUniqueValue = result.IsPrimaryKey,  // 【优化】智能识别主键
                        ColumnDataSourceType = (int)DataSourceType.Excel,
                        DataSourceConfig = new ExcelConfig { ExcelColumn = result.ExcelColumn }
                    };

                    ImportConfig.ColumnMappings.Add(mapping);

                    // 【优化】保存置信度分数
                    string key = $"{result.ExcelColumn}->{result.DbColumn}";
                    confidenceScores[key] = result.Score;

                    // 从两个列表中移除已选择的项
                    RemoveFromExcelColumns(result.ExcelColumn);
                    RemoveFromSystemFields(result.DbColumn);
                }

                // 【优化】设置置信度分数供界面显示使用
                SetMatchConfidenceScores(confidenceScores);

                // 更新映射列表显示
                UpdateMappingsList();

                // 【优化】显示详细的匹配结果统计
                int highConfidence = matchResults.Count(r => r.Score >= 0.9);
                int mediumConfidence = matchResults.Count(r => r.Score >= 0.75 && r.Score < 0.9);
                int lowConfidence = matchResults.Count(r => r.Score < 0.75);
                int primaryKeys = matchResults.Count(r => r.IsPrimaryKey);

                string message = $"智能匹配完成：\n\n" +
                    $"✅总匹配数: {matchResults.Count}\n" +
                    $"  - 高置信度(≥90%): {highConfidence}\n" +
                    $"  - 中置信度(75-90%): {mediumConfidence}\n" +
                    $"  - 低置信度(<75%): {lowConfidence}\n\n" +
                    $"✅识别主键: {primaryKeys} 个\n\n" +
                    $"请检查映射结果，如有需要请手动调整。";

                MessageBox.Show(message, "智能匹配结果", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 记录日志
                MainForm.Instance.PrintInfoLog($"智能匹配完成: 实体={TargetEntityType?.Name}, 匹配数={matchResults.Count}, 主键数={primaryKeys}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"自动匹配失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 应用 AI 建议到当前映射列表
        /// </summary>
        private void ApplyAiSuggestions(RUINORERP.Business.AIServices.DataImport.IntelligentMappingResult aiResult)
        {
            // 清空现有映射
            ImportConfig.ColumnMappings.Clear();

            var confidenceScores = new Dictionary<string, double>();

            foreach (var kvp in aiResult.Mappings)
            {
                string excelCol = kvp.Key;
                var suggestion = kvp.Value;

                // 查找对应的系统字段显示名（从KeyValuePair中获取）
                string systemFieldDisplay = null;
                string systemFieldKey = suggestion.TargetField;

                foreach (var item in listBoxSystemFields.Items)
                {
                    if (item is KeyValuePair<string, string> fieldKvp && fieldKvp.Key == systemFieldKey)
                    {
                        systemFieldDisplay = fieldKvp.Value;
                        break;
                    }
                }

                if (systemFieldDisplay == null) continue;

                var mapping = new ColumnMapping
                {
                    OriginalExcelColumn = excelCol,
                    SystemField = new SerializableKeyValuePair<string>(systemFieldKey, systemFieldDisplay),
                    IsUniqueValue = (suggestion.TargetField == aiResult.SuggestedLogicalKey),
                    DataSourceConfig = new ExcelConfig { ExcelColumn = excelCol }
                };

                ImportConfig.ColumnMappings.Add(mapping);
                confidenceScores[$"{excelCol}->{systemFieldKey}"] = suggestion.Confidence;

                RemoveFromExcelColumns(excelCol);
                RemoveFromSystemFields(systemFieldKey);
            }

            SetMatchConfidenceScores(confidenceScores);
            UpdateMappingsList();

            MessageBox.Show($"已应用 AI 建议：共匹配 {aiResult.Mappings.Count} 个字段，建议逻辑主键为 [{aiResult.SuggestedLogicalKey}]。", "AI 辅助完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 去重复选框改变事件
        /// </summary>
        private void chkRemoveDuplicates_CheckedChanged(object sender, EventArgs e)
        {
            // 根据是否启用去重控制相关控件状态
            kcmbDeduplicateStrategy.Enabled = chkRemoveDuplicates.Checked;
            kbtnConfigDeduplicateFields.Enabled = chkRemoveDuplicates.Checked;
        }

        /// <summary>
        /// 配置去重字段按钮点击事件
        /// </summary>
        private void kbtnConfigDeduplicateFields_Click(object sender, EventArgs e)
        {
            using (var deduplicateConfigDialog = new frmDeduplicateFieldConfig())
            {
                var allFields = new List<DeduplicateFieldItem>();
                var addedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var mapping in ImportConfig.ColumnMappings)
                {
                    if (mapping.SystemField != null && !string.IsNullOrEmpty(mapping.SystemField.Key) &&
                        addedKeys.Add(mapping.SystemField.Key))
                    {
                        string displayName = mapping.SystemField.Value ?? mapping.SystemField.Key;
                        if (!string.IsNullOrEmpty(mapping.OriginalExcelColumn) &&
                            mapping.ColumnDataSourceType == (int)DataSourceType.Excel)
                        {
                            displayName = $"{displayName} (← {mapping.OriginalExcelColumn})";
                        }

                        allFields.Add(new DeduplicateFieldItem
                        {
                            Key = mapping.SystemField.Key,
                            DisplayName = displayName,
                            Category = "系统字段"
                        });
                    }

                    if (mapping.ColumnDataSourceType == (int)DataSourceType.ForeignKey &&
                        mapping.DataSourceConfig is DatabaseReferenceConfig fkConfig &&
                        !string.IsNullOrEmpty(fkConfig.ForeignKeySourceColumn?.Key) &&
                        addedKeys.Add(fkConfig.ForeignKeySourceColumn.Key))
                    {
                        allFields.Add(new DeduplicateFieldItem
                        {
                            Key = fkConfig.ForeignKeySourceColumn.Key,
                            DisplayName = fkConfig.ForeignKeySourceColumn.Value ?? fkConfig.ForeignKeySourceColumn.Key,
                            Category = "Excel源列"
                        });
                    }

                    if (mapping.ColumnDataSourceType == (int)DataSourceType.ColumnConcat &&
                        mapping.DataSourceConfig is ColumnConcatConfig concatConfig &&
                        concatConfig.ConcatColumns != null)
                    {
                        foreach (var srcCol in concatConfig.ConcatColumns)
                        {
                            if (!string.IsNullOrEmpty(srcCol?.Key) && addedKeys.Add(srcCol.Key))
                            {
                                allFields.Add(new DeduplicateFieldItem
                                {
                                    Key = srcCol.Key,
                                    DisplayName = srcCol.Value ?? srcCol.Key,
                                    Category = "Excel源列"
                                });
                            }
                        }
                    }
                }

                deduplicateConfigDialog.AvailableFields = allFields;
                deduplicateConfigDialog.SelectedFields = ImportConfig.DeduplicateFields?.ToList() ?? new List<string>();
                deduplicateConfigDialog.IgnoreEmptyValues = ImportConfig.IgnoreEmptyValuesInDeduplication;

                if (deduplicateConfigDialog.ShowDialog() == DialogResult.OK)
                {
                    ImportConfig.DeduplicateFields = deduplicateConfigDialog.SelectedFields;
                    ImportConfig.IgnoreEmptyValuesInDeduplication = deduplicateConfigDialog.IgnoreEmptyValues;

                    int selectedCount = (ImportConfig.DeduplicateFields?.Count ?? 0);
                    MessageBox.Show($"去重字段配置已更新，共选择 {selectedCount} 个去重字段",
                        "配置成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        private void kbtnDeleteMapping_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsEditMode || string.IsNullOrEmpty(ImportConfig.MappingName))
                {
                    MessageBox.Show("只有编辑已保存的配置时才能删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(
                    $"确定要删除配置 \"{ImportConfig.MappingName}\" 吗？此操作不可撤销。",
                    "确认删除",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                string filePath = Path.Combine(_configFilePath, $"{ImportConfig.MappingName}.xml");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                MessageBox.Show($"配置 \"{ImportConfig.MappingName}\" 已删除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 加载全新的初始化配置
                InitializeNewMapping();

                // 刷新已保存映射列表                LoadSavedMappings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化新的映射配置
        /// </summary>
        private void InitializeNewMapping()
        {
            ResetToNewConfig();
        }

        /// <summary>
        /// 从Excel列列表中移除指定的列
        /// </summary>
        /// <param name="excelColumn">要移除的Excel列名</param>
        private void RemoveFromExcelColumns(string excelColumn)
        {
            if (string.IsNullOrEmpty(excelColumn))
            {
                return;
            }

            // 检查是否是系统生成或默认值的情况，这些不需要从Excel列表中移除
            if (excelColumn.StartsWith("[系统生成]") || excelColumn.StartsWith("[默认值"))
            {
                return;
            }

            // 查找并移除对应的项
            for (int i = 0; i < listBoxExcelColumns.Items.Count; i++)
            {
                if (listBoxExcelColumns.Items[i].ToString() == excelColumn)
                {
                    listBoxExcelColumns.Items.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 从系统字段列表中移除指定的字段
        /// </summary>
        /// <param name="systemField">要移除的系统字段名(英文Key)</param>
        private void RemoveFromSystemFields(string systemFieldKey)
        {
            if (string.IsNullOrEmpty(systemFieldKey))
            {
                return;
            }

            // 查找并移除对应的项（现在存储的是KeyValuePair）
            for (int i = 0; i < listBoxSystemFields.Items.Count; i++)
            {
                if (listBoxSystemFields.Items[i] is KeyValuePair<string, string> item && item.Key == systemFieldKey)
                {
                    listBoxSystemFields.Items.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 将Excel列恢复到Excel列列表中
        /// </summary>
        /// <param name="mapping">包含Excel列信息的映射</param>
        private void RestoreExcelColumn(ColumnMapping mapping)
        {
            // ✅ 从OriginalExcelColumn获取原始Excel列名
            string columnToRestore = mapping.OriginalExcelColumn;

            // 如果 OriginalExcelColumn 为空，无法恢复
            if (string.IsNullOrEmpty(columnToRestore))
            {
                return;
            }

            // ✅ 检查是否是特殊数据来源的情况（虚拟列）
            bool isSpecialDataSource = columnToRestore.StartsWith("[") && columnToRestore.Contains("]");

            if (isSpecialDataSource)
            {
                // 这些情况不需要恢复到Excel列表
                return;
            }

            // 检查Excel列是否已经在列表中
            bool alreadyExists = false;
            foreach (var item in listBoxExcelColumns.Items)
            {
                if (item.ToString() == columnToRestore)
                {
                    alreadyExists = true;
                    break;
                }
            }

            // 如果不存在，添加到列表中并保持排序
            if (!alreadyExists)
            {
                listBoxExcelColumns.Items.Add(columnToRestore);
                // ✅ 保持列表有序
                listBoxExcelColumns.Sorted = true;
            }
        }

        /// <summary>
        /// 将系统字段恢复到系统字段列表中
        /// </summary>
        /// <param name="mapping">包含系统字段信息的映射</param>
        private void RestoreSystemField(ColumnMapping mapping)
        {
            if (mapping.SystemField == null || string.IsNullOrEmpty(mapping.SystemField.Key))
            {
                return;
            }

            // 检查字段是否已经在列表中（现在存储的是KeyValuePair）
            bool alreadyExists = false;
            foreach (var item in listBoxSystemFields.Items)
            {
                if (item is KeyValuePair<string, string> kvp && kvp.Key == mapping.SystemField?.Key)
                {
                    alreadyExists = true;
                    break;
                }
            }

            // 如果不存在，添加到列表中（检查是否必填）
            if (!alreadyExists)
            {
                bool isRequired = IsFieldRequired(mapping.SystemField?.Key);
                string displayText = isRequired ? $"* {mapping.SystemField?.Value}" : mapping.SystemField?.Value;
                listBoxSystemFields.Items.Add(new KeyValuePair<string, string>(mapping.SystemField.Key, displayText));
            }
        }

        /// <summary>
        /// 选中新添加的映射项
        /// </summary>
        /// <param name="mapping">要选中的映射</param>
        private void SelectNewlyAddedMapping(ColumnMapping mapping)
        {
            // 查找新添加的映射在列表中的索引
            int index = ImportConfig.ColumnMappings.IndexOf(mapping);
            if (index >= 0 && index < listBoxMappings.Items.Count)
            {
                listBoxMappings.SelectedIndex = index;
            }
        }

        /// <summary>
        /// 深拷贝配置对象（用于创建快照）
        /// </summary>
        /// <param name="config">要拷贝的配置对象</param>
        /// <returns>深拷贝后的配置对象</returns>
        private ImportConfiguration CloneConfiguration(ImportConfiguration config)
        {
            if (config == null)
            {
                return null;
            }

            var clone = new ImportConfiguration
            {
                MappingName = config.MappingName,
                EntityType = config.EntityType,
                ImportType = config.ImportType,
                TargetTable = config.TargetTable != null ? new SerializableKeyValuePair<string>(config.TargetTable.Key, config.TargetTable.Value) : null,
                ExistenceStrategy = config.ExistenceStrategy,
                EnableDeduplication = config.EnableDeduplication,
                DeduplicateFields = config.DeduplicateFields != null ? new List<string>(config.DeduplicateFields) : new List<string>(),
                DeduplicateStrategy = config.DeduplicateStrategy,
                IgnoreEmptyValuesInDeduplication = config.IgnoreEmptyValuesInDeduplication,
                CreateTime = config.CreateTime,
                UpdateTime = config.UpdateTime,
                Description = config.Description,
                Version = config.Version,
                DependentTables = config.DependentTables != null ? new List<string>(config.DependentTables) : new List<string>(),
                ColumnMappings = new List<ColumnMapping>()
            };

            // 深拷贝列映射集合
            if (config.ColumnMappings != null)
            {
                foreach (var mapping in config.ColumnMappings)
                {
                    clone.ColumnMappings.Add(CloneColumnMapping(mapping));
                }
            }

            return clone;
        }

        /// <summary>
        /// 深拷贝列映射对象
        /// </summary>
        /// <param name="mapping">要拷贝的映射对象</param>
        /// <returns>深拷贝后的映射对象</returns>
        private ColumnMapping CloneColumnMapping(ColumnMapping mapping)
        {
            if (mapping == null)
            {
                return null;
            }

            var clone = new ColumnMapping
            {
                MappingId = mapping.MappingId,
                OriginalExcelColumn = mapping.OriginalExcelColumn,
                SystemField = mapping.SystemField != null 
                    ? new SerializableKeyValuePair<string>(mapping.SystemField.Key, mapping.SystemField.Value) 
                    : null,
                IsUniqueValue = mapping.IsUniqueValue,
                IsBusinessKey = mapping.IsBusinessKey,
                DataSourceConfig = CloneDataSourceConfig(mapping.DataSourceConfig)
            };

            return clone;
        }

        /// <summary>
        /// 深拷贝数据源配置对象
        /// </summary>
        /// <param name="config">要拷贝的配置对象</param>
        /// <returns>深拷贝后的配置对象</returns>
        private DataSourceConfigBase CloneDataSourceConfig(DataSourceConfigBase config)
        {
            if (config == null)
            {
                return null;
            }

            // 根据具体类型进行深拷贝
            switch (config)
            {
                case ExcelConfig excelConfig:
                    return new ExcelConfig
                    {
                        ExcelColumn = excelConfig.ExcelColumn
                    };

                case DefaultValueConfig defaultConfig:
                    return new DefaultValueConfig
                    {
                        Value = defaultConfig.Value
                    };

                case SystemGeneratedConfig sysConfig:
                    return new SystemGeneratedConfig
                    {
                        GeneratedType = sysConfig.GeneratedType,
                        DateTimeFormat = sysConfig.DateTimeFormat,
                        BusinessCodePrefix = sysConfig.BusinessCodePrefix,
                        BusinessCodeRule = sysConfig.BusinessCodeRule,
                        SequenceDigits = sysConfig.SequenceDigits,
                        CustomExpression = sysConfig.CustomExpression
                    };

                case DatabaseReferenceConfig foreignConfig:
                    return new DatabaseReferenceConfig
                    {
                        IsSelfReference = foreignConfig.IsSelfReference,
                        ForeignTableName = foreignConfig.ForeignTableName,
                        ForeignFieldName = foreignConfig.ForeignFieldName,
                        ForeignKeySourceColumn = foreignConfig.ForeignKeySourceColumn != null
                            ? new SerializableKeyValuePair<string>(foreignConfig.ForeignKeySourceColumn.Key, foreignConfig.ForeignKeySourceColumn.Value)
                            : null
                    };

                case ColumnConcatConfig concatConfig:
                    return new ColumnConcatConfig
                    {
                        SourceColumns = concatConfig.SourceColumns != null 
                            ? new List<string>(concatConfig.SourceColumns) 
                            : new List<string>(),
                        Separator = concatConfig.Separator
                    };

                case ExcelImageConfig imageConfig:
                    return new ExcelImageConfig
                    {
                        StorageType = imageConfig.StorageType,
                        NamingRule = imageConfig.NamingRule,
                        OutputDirectory = imageConfig.OutputDirectory,
                        NamingReferenceColumn = imageConfig.NamingReferenceColumn,
                        PreserveExtension = imageConfig.PreserveExtension,
                        ConvertType = imageConfig.ConvertType,
                        Quality = imageConfig.Quality
                    };

                default:
                    // 未知类型，返回原对象（浅拷贝）
                    return config;
            }
        }

        /// <summary>
        /// 恢复原始配置状态（取消编辑时调用）
        /// </summary>
        private void RestoreOriginalConfiguration()
        {
            if (_originalConfigSnapshot == null)
            {
                return;
            }

            try
            {
                // ✅ 将原始配置的属性复制回当前配置
                ImportConfig.MappingName = _originalConfigSnapshot.MappingName;
                ImportConfig.EntityType = _originalConfigSnapshot.EntityType;
                ImportConfig.ImportType = _originalConfigSnapshot.ImportType;
                ImportConfig.TargetTable = _originalConfigSnapshot.TargetTable;
                ImportConfig.ExistenceStrategy = _originalConfigSnapshot.ExistenceStrategy;
                ImportConfig.EnableDeduplication = _originalConfigSnapshot.EnableDeduplication;
                ImportConfig.DeduplicateFields = _originalConfigSnapshot.DeduplicateFields != null 
                    ? new List<string>(_originalConfigSnapshot.DeduplicateFields) 
                    : new List<string>();
                ImportConfig.DeduplicateStrategy = _originalConfigSnapshot.DeduplicateStrategy;
                ImportConfig.IgnoreEmptyValuesInDeduplication = _originalConfigSnapshot.IgnoreEmptyValuesInDeduplication;
                ImportConfig.Description = _originalConfigSnapshot.Description;
                ImportConfig.Version = _originalConfigSnapshot.Version;
                ImportConfig.DependentTables = _originalConfigSnapshot.DependentTables != null
                    ? new List<string>(_originalConfigSnapshot.DependentTables)
                    : new List<string>();

                // ✅ 清空并恢复列映射集合
                ImportConfig.ColumnMappings.Clear();
                if (_originalConfigSnapshot.ColumnMappings != null)
                {
                    foreach (var mapping in _originalConfigSnapshot.ColumnMappings)
                    {
                        ImportConfig.ColumnMappings.Add(CloneColumnMapping(mapping));
                    }
                }

                // ✅ 重新绑定UI
                RebindAll();

                MainForm.Instance.PrintInfoLog($"已取消编辑，恢复到原始配置: {_originalConfigSnapshot.MappingName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"恢复原始配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
