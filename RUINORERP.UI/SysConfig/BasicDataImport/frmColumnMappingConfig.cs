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
using RUINORERP.Model.ImportEngine.Models;

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
        /// 导入模板管理器
        /// </summary>
        private ImportTemplateManager _templateManager;

        /// <summary>
        /// 目标实体类型
        /// </summary>
        public Type TargetEntityType { get; set; }

        /// <summary>
        /// Excel数据（用于预览）
        /// </summary>
        public DataTable ExcelData { get; set; }

        /// <summary>
        /// 列映射集合
        /// </summary>
        public ColumnMappingCollection ColumnMappings { get; set; }

        /// <summary>
        /// 是否为编辑模式
        /// </summary>
        public bool IsEditMode { get; set; }

        /// <summary>
        /// 原始映射配置名称
        /// </summary>
        public string OriginalMappingName { get; set; }

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
        /// 构造函数
        /// </summary>
        public frmColumnMappingConfig()
        {
            InitializeComponent();
            ColumnMappings = new ColumnMappingCollection();
            ImportConfig = new ImportConfiguration();
            _columnMappingManager = new ColumnMappingManager();
            _templateManager = new ImportTemplateManager();
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
        /// 系统字段列表双击事件
        /// </summary>
        private void listBoxSystemFields_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxSystemFields.SelectedItem == null)
            {
                return;
            }

            // 获取选中的系统字段
            string systemFieldDisplay = listBoxSystemFields.SelectedItem.ToString();
            string systemField = systemFieldDisplay.StartsWith("* ")
                ? systemFieldDisplay.Substring(2)
                : systemFieldDisplay;

            // 检查是否已存在该系统字段的映射
            var existingMapping = ColumnMappings.GetMappingBySystemField(systemField);
            if (existingMapping != null)
            {
                // 已存在映射，打开属性配置进行编辑
                ConfigureMappingProperty(existingMapping);
                return;
            }

            // 不存在映射，创建新映射并配置属性
            CreateAndConfigureMapping(systemFieldDisplay,systemField, null);
        }

        /// <summary>
        /// 创建并配置新映射
        /// </summary>
        /// <param name="systemField">系统字段名</param>
        /// <param name="excelColumn">Excel列名（可选）</param>
        private void CreateAndConfigureMapping(string systemFieldDisplay, string systemField, string excelColumn)
        {
            // 验证：如果两边都选择了，必须都选择
            bool hasExcelColumn = !string.IsNullOrEmpty(excelColumn);
            bool hasSystemField = !string.IsNullOrEmpty(systemField);

            if (hasExcelColumn && hasSystemField)
            {
                // 两边都选择了，创建映射
                var mapping = new ColumnMapping
                {
                    ExcelColumn = excelColumn,
                    SystemField = new SerializableKeyValuePair<string>(systemField, systemFieldDisplay)
                };

                // 打开属性配置对话框
                if (ConfigureMappingProperty(mapping))
                {
                    // 如果用户确认了配置，添加到集合
                    ColumnMappings.Add(mapping);

                    // 从两个列表中移除已选择的项
                    RemoveFromExcelColumns(excelColumn);
                    RemoveFromSystemFields(systemField);

                    // 更新映射列表显示
                    UpdateMappingsList();
                }
            }
            else if (!hasExcelColumn && hasSystemField)
            {
                // 只选择了系统字段，Excel中没有指定列
                // 创建新映射
                var mapping = new ColumnMapping
                {
                    ExcelColumn = string.Empty,
                    SystemField = new SerializableKeyValuePair<string>(systemField, systemFieldDisplay)
                };

                // 打开属性配置对话框
                if (ConfigureMappingProperty(mapping))
                {
                    // 如果用户确认了配置，添加到集合
                    ColumnMappings.Add(mapping);

                    // 验证必须设置特殊数据来源
                    if (mapping.DataSourceType == DataSourceType.Excel)
                    {
                        MessageBox.Show("由于Excel中没有指定列，必须设置数据来源类型为：默认值、系统生成、外键关联或自身字段引用。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // 不添加到集合
                        ColumnMappings.RemoveAt(ColumnMappings.Count - 1);
                        return;
                    }

                    // 根据数据来源类型设置显示的Excel列
                    switch (mapping.DataSourceType)
                    {
                        case DataSourceType.SystemGenerated:
                            mapping.ExcelColumn = $"[系统生成] {systemField}";
                            break;
                        case DataSourceType.DefaultValue:
                            mapping.ExcelColumn = $"[默认值:{mapping.DefaultValue}] {systemField}";
                            break;
                        case DataSourceType.ForeignKey:
                            mapping.ExcelColumn = $"[外键关联:{mapping.ForeignConfig?.ForeignKeyTable?.Value}] {systemField}";
                            break;
                    case DataSourceType.SelfReference:
                        mapping.ExcelColumn = $"[自身引用:{mapping.SelfReferenceField?.Value}] {systemField}";
                        break;

                    case DataSourceType.FieldCopy:
                        mapping.ExcelColumn = $"[字段复制:{mapping.CopyFromField?.Value}] {systemField}";
                        break;

                    case DataSourceType.ColumnConcat:
                        // 格式化显示列拼接配置
                        string concatCols = string.Join("+", mapping.ConcatConfig?.SourceColumns ?? new List<string>());
                        mapping.ExcelColumn = $"[列拼接:{concatCols}] {systemField}";
                        break;

                    case DataSourceType.ExcelImage:
                        // 格式化显示Excel图片配置
                        string namingRule = mapping.ImageConfig?.NamingRule.ToString() ?? "AutoIncrement";
                        mapping.ExcelColumn = $"[Excel图片:{namingRule}] {systemField}";
                        break;
                    }

                    // 从系统字段列表中移除
                    RemoveFromSystemFields(systemField);

                    // 更新映射列表显示
                    UpdateMappingsList();
                }
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
                    // 保存用户配置的属性
                    mapping.IsUniqueValue = propertyDialog.IsUniqueValue;
                    mapping.IgnoreEmptyValue = propertyDialog.IgnoreEmptyValue;
                    mapping.DefaultValue = propertyDialog.DefaultValue;
                    mapping.IsSystemGenerated = propertyDialog.IsSystemGenerated;
                    mapping.DataSourceType = propertyDialog.SelectedDataSourceType;
                    mapping.SelfReferenceField = propertyDialog.SelfReferenceField;
                    mapping.CopyFromField = propertyDialog.CopyFromField;
                    mapping.EnumDefaultConfig = propertyDialog.EnumDefaultConfig;
                    mapping.IsImageColumn = propertyDialog.IsImageColumn;
                    mapping.ImageColumnType = propertyDialog.ImageColumnType;

                    // 保存外键关联配置
                    if (propertyDialog.SelectedDataSourceType == DataSourceType.ForeignKey)
                    {
                        mapping.ForeignConfig = new ForeignRelatedConfig
                        {
                            ForeignKeyTable = propertyDialog.ForeignKeyTable,
                            ForeignKeyField = propertyDialog.ForeignKeyField,
                            ForeignKeySourceColumn = propertyDialog.ForeignKeySourceColumn
                        };
                    }
                    else
                    {
                        mapping.ForeignConfig = null;
                    }

                    // 保存列拼接配置
                    if (propertyDialog.SelectedDataSourceType == DataSourceType.ColumnConcat)
                    {
                        mapping.ConcatConfig = propertyDialog.ConcatConfig;
                    }
                    else
                    {
                        mapping.ConcatConfig = null;
                    }

                    // 保存Excel图片配置
                    if (propertyDialog.SelectedDataSourceType == DataSourceType.ExcelImage)
                    {
                        mapping.ImageConfig = propertyDialog.ImageConfig;
                        mapping.IsImageColumn = true;
                    }
                    else if (!propertyDialog.IsImageColumn)
                    {
                        mapping.ImageConfig = null;
                    }

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
                // 【修复】初始化去重策略ComboBox默认值
                kcmbDeduplicateStrategy.SelectedIndex = 0; // 默认选择“保留第一条记录”
                
                // 设置配置文件路径
                _configFilePath = ColumnMappingConstants.GetConfigFilePath();

                // 确保目录存在
                if (!Directory.Exists(_configFilePath))
                {
                    Directory.CreateDirectory(_configFilePath);
                }

                // 加载系统字段
                LoadSystemFields();

                // 如果有Excel数据，加载Excel列
                if (ExcelData != null && ExcelData.Rows.Count > 0)
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
        /// 设置窗口标题
        /// </summary>
        private void SetWindowTitle()
        {
            if (IsEditMode && !string.IsNullOrEmpty(OriginalMappingName))
            {
                this.Text = $"编辑列映射配置 - {OriginalMappingName}";
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

                // 清空并添加字段到列表框
                listBoxSystemFields.Items.Clear();
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
                    listBoxSystemFields.Items.Add(displayText);
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
        /// 加载已保存的映射配置和模板
        /// 【优化】统一加载配置和模板
        /// </summary>
        private void LoadSavedMappings()
        {
            try
            {
                // 【优化】使用新的 LoadTemplateList 方法
                LoadTemplateList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载已保存的映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 列的显示，unitName,<单位,true>
        /// 列名，列中文，是否显示1
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public ConcurrentDictionary<string, string> FieldNameList { get; set; } = new ConcurrentDictionary<string, string>();


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

            string systemFieldDisplay = listBoxSystemFields.SelectedItem.ToString();

            // 去掉必填标识，获取实际显示字段名
            systemFieldDisplay = systemFieldDisplay.StartsWith("* ")
               ? systemFieldDisplay.Substring(2)
               : systemFieldDisplay;

            // 获取实际显示字段名
            string systemField = FieldNameList.FirstOrDefault(c => c.Value == systemFieldDisplay).Key;
            // 检查是否已存在该系统字段的映射
            if (ColumnMappings.GetMappingBySystemField(systemField) != null)
            {
                MessageBox.Show($"系统字段 \"{systemField}\" 已被映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取用户选择的Excel列
            string excelColumn = listBoxExcelColumns.SelectedItem?.ToString();

            // 如果选择了Excel列，检查是否已被映射
            if (!string.IsNullOrEmpty(excelColumn))
            {
                if (ColumnMappings.GetMappingByExcelColumn(excelColumn) != null)
                {
                    MessageBox.Show($"Excel列 \"{excelColumn}\" 已被映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 验证：如果没有选择Excel列，则不允许选择"Excel数据源"类型
            if (string.IsNullOrEmpty(excelColumn))
            {
                // 创建没有Excel列的映射（用户可以在属性设置中指定数据来源类型）
                var mapping = new ColumnMapping
                {
                    ExcelColumn = string.Empty,
                    SystemField = new SerializableKeyValuePair<string>(systemField, systemFieldDisplay),
                    DataSourceType = DataSourceType.DefaultValue // 默认为默认值，让用户在属性设置中修改
                };

                // 打开属性配置对话框，让用户选择数据来源类型
                if (ConfigureMappingProperty(mapping))
                {
                    // 如果用户选择了"Excel数据源"但没有Excel列，则不允许
                    if (mapping.DataSourceType == DataSourceType.Excel)
                    {
                        MessageBox.Show("由于没有选择Excel来源列，不能选择\"Excel数据源\"类型。请选择：默认值、系统生成、外键关联或自身字段引用。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 如果用户确认了配置，添加到集合
                    ColumnMappings.Add(mapping);

                    // 从系统字段列表中移除
                    RemoveFromSystemFields(systemField);

                    // 更新映射列表显示
                    UpdateMappingsList();
                }
            }
            else
            {
                // 创建并配置新映射（有Excel列的情况）
                CreateAndConfigureMapping(systemFieldDisplay, systemField, excelColumn);
            }
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
            
            foreach (var mapping in ColumnMappings)
            {
                string displayText = $"{mapping.ExcelColumn} -> {mapping.SystemField?.Value}";
                string key = $"{mapping.ExcelColumn}->{mapping.SystemField?.Key}";

                // 添加属性标识
                List<string> flags = new List<string>();

                // 根据数据来源类型添加标识
                switch (mapping.DataSourceType)
                {
                    case DataSourceType.Excel:
                        // Excel数据源，如果有默认值则显示
                        if (!string.IsNullOrEmpty(mapping.DefaultValue))
                        {
                            flags.Add($"默认值:{mapping.DefaultValue}");
                        }
                        break;
                    case DataSourceType.DefaultValue:
                        flags.Add($"默认值:{mapping.DefaultValue}");
                        break;
                    case DataSourceType.SystemGenerated:
                        flags.Add("系统生成");
                        break;
                    case DataSourceType.ForeignKey:
                        flags.Add("外键");
                        break;
                    case DataSourceType.SelfReference:
                        flags.Add("自身引用");
                        break;

                    case DataSourceType.FieldCopy:
                        flags.Add($"复制:{mapping.CopyFromField?.Value}");
                        break;
                    case DataSourceType.ColumnConcat:
                        string concatCols = string.Join("+", mapping.ConcatConfig?.SourceColumns ?? new List<string>());
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
            var mapping = ColumnMappings[selectedIndex];

            // 删除前，将Excel列和系统字段恢复到对应的列表中
            RestoreExcelColumn(mapping);
            RestoreSystemField(mapping);

            // 从集合中删除
            ColumnMappings.RemoveAt(selectedIndex);
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
        /// 双击映射列表
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
            if (selectedIndex < 0 || selectedIndex >= ColumnMappings.Count)
            {
                return null;
            }

            return ColumnMappings[selectedIndex];
        }

        /// <summary>
        /// 保存映射配置0
        /// </summary>
        private void kbtnSaveMapping_Click(object sender, EventArgs e)
        {
            string mappingName = textBoxMappingName.Text.Trim();
            if (string.IsNullOrEmpty(mappingName))
            {
                MessageBox.Show("请输入映射配置名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ColumnMappings.Count == 0)
            {
                MessageBox.Show("请添加至少一个映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 【修复】验证去重配置
            if (chkRemoveDuplicates.Checked)
            {
                if (kcmbDeduplicateStrategy.SelectedIndex < 0)
                {
                    MessageBox.Show("已启用去重功能，请选择去重策略（保留第一条记录 或 保留最后一条记录）", 
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    kcmbDeduplicateStrategy.Focus();
                    return;
                }
            }

            try
            {
                // 更新全局配置
                ImportConfig.MappingName = mappingName;
                ImportConfig.EntityType = TargetEntityType?.Name;
                ImportConfig.ColumnMappings = ColumnMappings.ToList();
                ImportConfig.EnableDeduplication = chkRemoveDuplicates.Checked;
                ImportConfig.DeduplicateStrategy = (DeduplicateStrategy)kcmbDeduplicateStrategy.SelectedIndex;
                ImportConfig.UpdateTimestamp();

                // 保存配置
                _columnMappingManager ??= new ColumnMappingManager();
                _columnMappingManager.SaveConfiguration(ImportConfig);

                SavedMappingName = mappingName;
                MessageBox.Show("映射配置保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 触发保存事件
                MappingSaved?.Invoke(this, EventArgs.Empty);

                // 刷新已保存映射列表
                LoadSavedMappings();

                // 【修复】选中新保存的映射 - 需要匹配带前缀的格式
                string newItemText = $"[配置] {mappingName}";
                int newIndex = comboBoxSavedMappings.Items.IndexOf(newItemText);
                if (newIndex >= 0)
                {
                    comboBoxSavedMappings.SelectedIndex = newIndex;
                }

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
        /// 【优化】支持模板和配置的统一加载
        /// </summary>
        private void comboBoxSavedMappings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSavedMappings.SelectedIndex <= 0)
            {
                // 新建映射 - 重新加载所有列表
                textBoxMappingName.Text = string.Empty;
                ColumnMappings.Clear();
                ImportConfig = new ImportConfiguration();
                UpdateMappingsList();

                // 【修复】重置去重设置到默认值
                chkRemoveDuplicates.Checked = false;
                kcmbDeduplicateStrategy.SelectedIndex = 0; // 默认选择“保留第一条记录”

                // 重新加载Excel列和系统字段列表
                LoadSystemFields();
                if (ExcelData != null && ExcelData.Rows.Count > 0)
                {
                    LoadExcelColumns();
                }

                IsEditMode = false;
                OriginalMappingName = null;
                SetWindowTitle();
                return;
            }

            string selectedItem = comboBoxSavedMappings.SelectedItem.ToString();

            // 【优化】处理模板选择
            if (selectedItem.StartsWith("[模板] "))
            {
                string templateName = selectedItem.Substring(5); // 移除 "[模板] " 前缀
                ApplyImportTemplate(templateName);
                return;
            }

            // 【优化】处理配置选择
            if (selectedItem.StartsWith("[配置] "))
            {
                selectedItem = selectedItem.Substring(5); // 移除 "[配置] " 前缀
            }

            try
            {
                // 【修复】提取真实的配置名称(去除前缀)
                string mappingName = selectedItem;
                if (selectedItem.StartsWith("[配置] "))
                {
                    mappingName = selectedItem.Substring(5); // 移除 "[配置] " 前缀
                }

                _columnMappingManager ??= new ColumnMappingManager();
                var config = _columnMappingManager.LoadConfiguration(mappingName, TargetEntityType);

                if (config == null)
                {
                    MessageBox.Show($"加载配置失败: 配置数据为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ImportConfig = config;
                ColumnMappings = new ColumnMappingCollection(config?.ColumnMappings ?? new List<ColumnMapping>());
                textBoxMappingName.Text = config?.MappingName ?? string.Empty;

                // 更新去重复选框状态和策略
                chkRemoveDuplicates.Checked = config?.EnableDeduplication ?? false;
                kcmbDeduplicateStrategy.SelectedIndex = (int)(config?.DeduplicateStrategy ?? DeduplicateStrategy.FirstOccurrence);

                // 【修复】重新加载Excel列和系统字段列表
                LoadSystemFields();
                if (ExcelData != null && ExcelData.Rows.Count > 0)
                {
                    LoadExcelColumns();
                }

                // 【修复】从列表中移除已映射的项
                foreach (var mapping in ColumnMappings)
                {
                    // 只有真正的Excel列才需要从可用列表中移除
                    if (!string.IsNullOrEmpty(mapping.ExcelColumn) && 
                        !mapping.ExcelColumn.StartsWith("[") && 
                        !mapping.ExcelColumn.Contains("]"))
                    {
                        RemoveFromExcelColumns(mapping.ExcelColumn);
                    }
                    
                    if (mapping.SystemField != null && !string.IsNullOrEmpty(mapping.SystemField.Key))
                    {
                        RemoveFromSystemFields(mapping.SystemField.Key);
                    }
                }

                UpdateMappingsList();

                IsEditMode = true;
                OriginalMappingName = mappingName;
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
                ColumnMappings.Clear();
                
                // 【优化】保存匹配置信度分数
                var confidenceScores = new Dictionary<string, double>();
                
                // 根据匹配结果创建映射
                foreach (var result in matchResults)
                {
                    // 查找系统字段显示名称
                    string systemFieldDisplay = null;
                    foreach (var item in listBoxSystemFields.Items)
                    {
                        string display = item.ToString();
                        string field = display.StartsWith("* ") ? display.Substring(2) : display;
                        if (field == result.DbColumn)
                        {
                            systemFieldDisplay = display;
                            break;
                        }
                    }
                    
                    if (systemFieldDisplay == null)
                        continue;
                    
                    // 创建映射
                    var mapping = new ColumnMapping
                    {
                        ExcelColumn = result.ExcelColumn,
                        SystemField = new SerializableKeyValuePair<string>(result.DbColumn, systemFieldDisplay),
                        IsUniqueValue = result.IsPrimaryKey  // 【优化】智能识别主键
                    };
                    
                    ColumnMappings.Add(mapping);
                    
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
                
                string message = $"智能匹配完成！\n\n" +
                    $"✓ 总匹配数: {matchResults.Count}\n" +
                    $"  - 高置信度(≥90%): {highConfidence}\n" +
                    $"  - 中置信度(75-90%): {mediumConfidence}\n" +
                    $"  - 低置信度(<75%): {lowConfidence}\n\n" +
                    $"✓ 识别主键: {primaryKeys} 个\n\n" +
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
        /// AI 辅助映射
        /// </summary>
        private async void kbtnAiMatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (TargetEntityType == null)
                {
                    MessageBox.Show("请先选择数据类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var excelHeaders = GetExcelColumnsList();
                if (!excelHeaders.Any())
                {
                    MessageBox.Show("未检测到 Excel 数据，请先加载文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                kbtnAiMatch.Enabled = false;
                kbtnAiMatch.Text = "AI 分析中...";

                // 调用 Business 层的 AI 服务
                var engine = new RUINORERP.Business.ImportEngine.SmartImportEngine(MainForm.Instance.AppContext.Db);
                var result = await engine.GetAiMappingSuggestionsAsync(excelHeaders, TargetEntityType);

                kbtnAiMatch.Enabled = true;
                kbtnAiMatch.Text = "AI 辅助映射";

                if (result.Mappings.Count == 0)
                {
                    MessageBox.Show("AI 未能生成有效的映射建议，请尝试手动匹配或检查 AI 服务配置。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 应用 AI 建议
                ApplyAiSuggestions(result);
            }
            catch (Exception ex)
            {
                kbtnAiMatch.Enabled = true;
                kbtnAiMatch.Text = "AI 辅助映射";
                MessageBox.Show($"AI 辅助映射失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 应用 AI 建议到当前映射列表
        /// </summary>
        private void ApplyAiSuggestions(RUINORERP.Business.AIServices.DataImport.IntelligentMappingResult aiResult)
        {
            // 清空现有映射
            ColumnMappings.Clear();
            
            var confidenceScores = new Dictionary<string, double>();
            
            foreach (var kvp in aiResult.Mappings)
            {
                string excelCol = kvp.Key;
                var suggestion = kvp.Value;
                
                // 查找对应的系统字段显示名
                string systemFieldDisplay = null;
                string systemFieldKey = suggestion.TargetField;
                
                foreach (var item in listBoxSystemFields.Items)
                {
                    string display = item.ToString();
                    string field = display.StartsWith("* ") ? display.Substring(2) : display;
                    if (field == systemFieldKey)
                    {
                        systemFieldDisplay = display;
                        break;
                    }
                }
                
                if (systemFieldDisplay == null) continue;

                var mapping = new ColumnMapping
                {
                    ExcelColumn = excelCol,
                    SystemField = new SerializableKeyValuePair<string>(systemFieldKey, systemFieldDisplay),
                    IsUniqueValue = (suggestion.TargetField == aiResult.SuggestedLogicalKey)
                };
                
                ColumnMappings.Add(mapping);
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
            // 创建去重字段配置对话框
            using (var deduplicateConfigDialog = new frmDeduplicateFieldConfig())
            {
                // 设置可用字段
                deduplicateConfigDialog.AvailableFields = ColumnMappings.Select(m => m.SystemField).ToList();

                // 设置已选字段
                deduplicateConfigDialog.SelectedFields = ImportConfig.DeduplicateFields?.ToList() ?? new List<string>();

                // 设置是否忽略空值
                deduplicateConfigDialog.IgnoreEmptyValues = ImportConfig.IgnoreEmptyValuesInDeduplication;

                if (deduplicateConfigDialog.ShowDialog() == DialogResult.OK)
                {
                    // 保存去重字段配置
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
                // 只有在编辑模式下才能删除配置
                if (!IsEditMode || string.IsNullOrEmpty(OriginalMappingName))
                {
                    MessageBox.Show("只有编辑已保存的配置时才能删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 确认删除
                var result = MessageBox.Show(
                    $"确定要删除配置 \"{OriginalMappingName}\" 吗？此操作不可撤销。",
                    "确认删除",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // 删除配置文件
                string filePath = Path.Combine(_configFilePath, $"{OriginalMappingName}.xml");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                MessageBox.Show($"配置 \"{OriginalMappingName}\" 已删除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 加载全新的初始化配置
                InitializeNewMapping();

                // 刷新已保存映射列表
                LoadSavedMappings();
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
            // 清空当前映射
            ColumnMappings.Clear();

            // 重置编辑模式
            IsEditMode = false;
            OriginalMappingName = null;
            textBoxMappingName.Text = string.Empty;

            // 重新加载Excel列和系统字段列表
            LoadSystemFields();
            if (ExcelData != null && ExcelData.Rows.Count > 0)
            {
                LoadExcelColumns();
            }

            // 清空映射列表显示
            UpdateMappingsList();

            // 重置已保存配置选择
            comboBoxSavedMappings.SelectedIndex = 0;

            // 更新窗口标题
            SetWindowTitle();
        }

        #region 【新增】模板管理功能

        /// <summary>
        /// 保存为导入模板
        /// </summary>
        private void SaveAsTemplate()
        {
            if (ColumnMappings.Count == 0)
            {
                MessageBox.Show("请添加至少一个映射后再保存模板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 输入模板名称
            string templateName = KryptonInputBox.Show("请输入模板名称:", "保存为模板", $"{TargetEntityType?.Name ?? "数据"}导入模板");
            if (string.IsNullOrWhiteSpace(templateName))
                return;

            try
            {
                // 更新导入配置
                ImportConfig.MappingName = textBoxMappingName.Text.Trim();
                ImportConfig.EntityType = TargetEntityType?.FullName;
                ImportConfig.ColumnMappings = ColumnMappings.ToList();
                ImportConfig.EnableDeduplication = chkRemoveDuplicates.Checked;
                ImportConfig.DeduplicateStrategy = (DeduplicateStrategy)kcmbDeduplicateStrategy.SelectedIndex;

                // 创建模板
                var template = _templateManager.CreateTemplateFromConfig(
                    ImportConfig,
                    templateName,
                    $"{TargetEntityType?.Name ?? "数据"}导入模板 - {DateTime.Now:yyyy-MM-dd HH:mm}");

                // 保存模板
                _templateManager.SaveTemplate(template);

                MessageBox.Show($"模板 [{templateName}] 保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 刷新模板列表
                LoadTemplateList();

                // 记录日志
                MainForm.Instance.PrintInfoLog($"保存导入模板: {templateName}, 实体: {TargetEntityType?.Name}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存模板失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 应用导入模板
        /// </summary>
        private void ApplyImportTemplate(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                return;

            var template = _templateManager.LoadTemplate(templateName);
            if (template == null)
            {
                MessageBox.Show($"模板 [{templateName}] 不存在或已损坏", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 检查实体类型是否匹配
            if (template.EntityType != TargetEntityType?.FullName)
            {
                var result = MessageBox.Show(
                    $"模板 [{templateName}] 是为实体 [{template.EntityType}] 创建的，\n" +
                    $"当前实体是 [{TargetEntityType?.FullName}]。\n\n" +
                    $"是否仍要应用此模板？",
                    "实体类型不匹配",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;
            }

            try
            {
                // 应用模板到当前配置
                _templateManager.ApplyTemplate(ImportConfig, template);

                // 更新列映射集合
                ColumnMappings = new ColumnMappingCollection();
                foreach (var mapping in ImportConfig.ColumnMappings)
                {
                    ColumnMappings.Add(mapping);
                }

                // 更新界面显示
                UpdateMappingsList();

                // 更新去重设置
                chkRemoveDuplicates.Checked = template.EnableDeduplication;
                if (template.DeduplicateStrategy == DeduplicateStrategy.LastOccurrence)
                    kcmbDeduplicateStrategy.SelectedIndex = 1;
                else
                    kcmbDeduplicateStrategy.SelectedIndex = 0;

                // 更新可用列列表
                RefreshAvailableColumns();

                MessageBox.Show($"模板 [{templateName}] 应用成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 记录日志
                MainForm.Instance.PrintInfoLog($"应用导入模板: {templateName}, 映射数: {ColumnMappings.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用模板失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载模板列表
        /// </summary>
        private void LoadTemplateList()
        {
            // 保存当前选择
            string currentSelection = comboBoxSavedMappings.SelectedItem?.ToString();

            comboBoxSavedMappings.Items.Clear();
            comboBoxSavedMappings.Items.Add("-- 新建映射 --");

            // 添加已保存的映射配置
            var savedMappings = _columnMappingManager.GetAllMappingNames();
            foreach (var mappingName in savedMappings)
            {
                comboBoxSavedMappings.Items.Add($"[配置] {mappingName}");
            }

            // 添加模板（按实体类型筛选）
            var templates = _templateManager.GetTemplatesForEntity(TargetEntityType);
            foreach (var template in templates)
            {
                comboBoxSavedMappings.Items.Add($"[模板] {template.TemplateName}");
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
        }

        /// <summary>
        /// 刷新可用列列表（应用模板后）
        /// </summary>
        private void RefreshAvailableColumns()
        {
            // 重新加载所有列
            LoadSystemFields();
            if (ExcelData != null && ExcelData.Rows.Count > 0)
            {
                LoadExcelColumns();
            }

            // 从可用列表中移除已映射的列
            foreach (var mapping in ColumnMappings)
            {
                if (!string.IsNullOrEmpty(mapping.ExcelColumn))
                {
                    RemoveFromExcelColumns(mapping.ExcelColumn);
                }
                if (mapping.SystemField != null)
                {
                    RemoveFromSystemFields(mapping.SystemField.Key);
                }
            }
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        private void DeleteTemplate(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                return;

            var result = MessageBox.Show(
                $"确定要删除模板 [{templateName}] 吗？\n此操作不可恢复。",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                if (_templateManager.DeleteTemplate(templateName))
                {
                    MessageBox.Show($"模板 [{templateName}] 删除成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTemplateList();
                    MainForm.Instance.PrintInfoLog($"删除导入模板: {templateName}");
                }
                else
                {
                    MessageBox.Show($"模板 [{templateName}] 不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除模板失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

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
        /// <param name="systemField">要移除的系统字段名</param>
        private void RemoveFromSystemFields(string systemField)
        {
            if (string.IsNullOrEmpty(systemField))
            {
                return;
            }

            // 查找并移除对应的项（考虑是否有必填标识）
            for (int i = 0; i < listBoxSystemFields.Items.Count; i++)
            {
                string displayText = listBoxSystemFields.Items[i].ToString();
                string fieldName = displayText.StartsWith("* ")
                    ? displayText.Substring(2)
                    : displayText;

                if (fieldName == systemField)
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
            // 检查是否是特殊数据来源的情况
            bool isSpecialDataSource = mapping.ExcelColumn.StartsWith("[系统生成]") ||
                                     mapping.ExcelColumn.StartsWith("[默认值") ||
                                     mapping.ExcelColumn.StartsWith("[外键关联]") ||
                                     mapping.ExcelColumn.StartsWith("[自身引用]") ||
                                     mapping.ExcelColumn.StartsWith("[字段复制]");

            if (!string.IsNullOrEmpty(mapping.ExcelColumn) && isSpecialDataSource)
            {
                // 这些情况不需要恢复到Excel列表
                return;
            }

            // 如果有原始的Excel列名，恢复到列表中
            if (!string.IsNullOrEmpty(mapping.ExcelColumn) && mapping.SystemField != null)
            {
                // 检查Excel列是否已经在列表中
                bool alreadyExists = false;
                foreach (var item in listBoxExcelColumns.Items)
                {
                    if (item.ToString() == mapping.ExcelColumn)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                // 如果不存在，添加到列表中
                if (!alreadyExists)
                {
                    listBoxExcelColumns.Items.Add(mapping.ExcelColumn);
                }
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

            // 检查字段是否已经在列表中
            bool alreadyExists = false;
            foreach (var item in listBoxSystemFields.Items)
            {
                string displayText = item.ToString();
                string fieldName = displayText.StartsWith("* ")
                    ? displayText.Substring(2)
                    : displayText;

                if (fieldName == mapping.SystemField?.Key)
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
                listBoxSystemFields.Items.Add(displayText);
            }
        }
    }
}
