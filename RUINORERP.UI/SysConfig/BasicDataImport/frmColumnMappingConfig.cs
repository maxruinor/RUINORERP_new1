using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.UI.Common;
using System.Xml.Serialization;

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
        /// 构造函数
        /// </summary>
        public frmColumnMappingConfig()
        {
            InitializeComponent();
            ColumnMappings = new ColumnMappingCollection();
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
            CreateAndConfigureMapping(systemField, null);
        }

        /// <summary>
        /// 创建并配置新映射
        /// </summary>
        /// <param name="systemField">系统字段名</param>
        /// <param name="excelColumn">Excel列名（可选）</param>
        private void CreateAndConfigureMapping(string systemField, string excelColumn)
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
                    SystemField = systemField,
                    MappingName = textBoxMappingName.Text,
                    EntityType = TargetEntityType?.Name,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
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
                    SystemField = systemField,
                    MappingName = textBoxMappingName.Text,
                    EntityType = TargetEntityType?.Name,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                // 打开属性配置对话框
                if (ConfigureMappingProperty(mapping))
                {
                    // 如果用户确认了配置，添加到集合
                    ColumnMappings.Add(mapping);

                    // 验证必须设置属性（系统生成或默认值）
                    if (!mapping.IsSystemGenerated && string.IsNullOrEmpty(mapping.DefaultValue))
                    {
                        MessageBox.Show("由于Excel中没有指定列，必须设置\"系统生成\"或\"默认值\"属性。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // 不添加到集合
                        ColumnMappings.RemoveAt(ColumnMappings.Count - 1);
                        return;
                    }

                    // 根据属性设置显示的Excel列
                    if (mapping.IsSystemGenerated)
                    {
                        mapping.ExcelColumn = $"[系统生成] {systemField}";
                    }
                    else if (!string.IsNullOrEmpty(mapping.DefaultValue))
                    {
                        mapping.ExcelColumn = $"[默认值:{mapping.DefaultValue}] {systemField}";
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
            using (var propertyDialog = new FrmColumnPropertyConfig
            {
                CurrentMapping = mapping,
                TargetEntityType = TargetEntityType
            })
            {
                if (propertyDialog.ShowDialog() == DialogResult.OK)
                {
                    // 保存用户配置的属性
                    mapping.IsForeignKey = propertyDialog.IsForeignKey;
                    mapping.IsUniqueValue = propertyDialog.IsUniqueValue;
                    mapping.DefaultValue = propertyDialog.DefaultValue;
                    mapping.IsSystemGenerated = propertyDialog.IsSystemGenerated;
                    mapping.RelatedTableName = propertyDialog.RelatedTableName;
                    mapping.RelatedTableField = propertyDialog.RelatedTableField;
                    mapping.RelatedTableFieldName = propertyDialog.RelatedTableFieldName;
                    mapping.UpdateTime = DateTime.Now;

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void frmColumnMappingConfig_Load(object sender, EventArgs e)
        {
            try
            {
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
                var fieldNameList = UIHelper.GetFieldNameList(false, TargetEntityType);

                // 清空并添加字段到列表框
                listBoxSystemFields.Items.Clear();
                foreach (var field in fieldNameList)
                {
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

                // 检查是否有RequiredAttribute
                bool hasRequiredAttribute = property.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

                // 检查是否有SugarColumnAttribute且IsNullable为false
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
                        // 如果获取失败，默认为可空
                    }
                }

                // 有RequiredAttribute或IsNullable为false则为必填
                return hasRequiredAttribute || !isNullable;
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
                comboBoxSavedMappings.Items.Clear();
                comboBoxSavedMappings.Items.Add("-- 新建映射 --");

                if (!Directory.Exists(_configFilePath))
                {
                    return;
                }

                // 获取所有配置文件
                var configFiles = Directory.GetFiles(_configFilePath, "*.xml");
                foreach (var file in configFiles)
                {
                    string mappingName = Path.GetFileNameWithoutExtension(file);
                    comboBoxSavedMappings.Items.Add(mappingName);
                }

                comboBoxSavedMappings.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载已保存的映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            // 验证：如果Excel列列表不为空，必须选择Excel列
            if (listBoxExcelColumns.Items.Count > 0 && listBoxExcelColumns.SelectedItem == null)
            {
                MessageBox.Show("Excel列列表中有数据，必须选择Excel列。如果不使用Excel列，请删除或清空Excel数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string systemFieldDisplay = listBoxSystemFields.SelectedItem.ToString();

            // 去掉必填标识，获取实际字段名
            string systemField = systemFieldDisplay.StartsWith("* ")
                ? systemFieldDisplay.Substring(2)
                : systemFieldDisplay;

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

            // 创建并配置新映射
            CreateAndConfigureMapping(systemField, excelColumn);
        }

        /// <summary>
        /// 更新映射列表显示
        /// </summary>
        private void UpdateMappingsList()
        {
            listBoxMappings.Items.Clear();
            foreach (var mapping in ColumnMappings)
            {
                string displayText = $"{mapping.ExcelColumn} -> {mapping.SystemField}";
                
                // 添加属性标识
                List<string> flags = new List<string>();
                if (mapping.IsForeignKey) flags.Add("外键");
                if (mapping.IsUniqueValue) flags.Add("唯一");
                if (mapping.IsSystemGenerated) flags.Add("系统生成");
                
                if (flags.Count > 0)
                {
                    displayText += $" ({string.Join(", ", flags)})";
                }

                listBoxMappings.Items.Add(displayText);
            }
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
        /// 保存映射配置
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

            try
            {
                // 更新映射名称
                foreach (var mapping in ColumnMappings)
                {
                    mapping.MappingName = mappingName;
                    mapping.UpdateTime = DateTime.Now;
                }

                // 保存到文件
                string filePath = Path.Combine(_configFilePath, $"{mappingName}.xml");
                var serializer = new XmlSerializer(typeof(ColumnMappingCollection));
                using (var writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, ColumnMappings);
                }

                SavedMappingName = mappingName;
                MessageBox.Show("映射配置保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 触发保存事件
                MappingSaved?.Invoke(this, EventArgs.Empty);

                // 刷新已保存映射列表
                LoadSavedMappings();

                // 选中新保存的映射
                comboBoxSavedMappings.SelectedIndex = comboBoxSavedMappings.Items.IndexOf(mappingName);

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
        /// 加载已保存的映射配置
        /// </summary>
        private void comboBoxSavedMappings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSavedMappings.SelectedIndex <= 0)
            {
                // 新建映射 - 重新加载所有列表
                textBoxMappingName.Text = string.Empty;
                ColumnMappings.Clear();
                UpdateMappingsList();

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

            try
            {
                string mappingName = comboBoxSavedMappings.SelectedItem.ToString();
                string filePath = Path.Combine(_configFilePath, $"{mappingName}.xml");

                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"映射配置文件不存在: {mappingName}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var serializer = new XmlSerializer(typeof(ColumnMappingCollection));
                using (var reader = new StreamReader(filePath))
                {
                    ColumnMappings = (ColumnMappingCollection)serializer.Deserialize(reader);
                }

                textBoxMappingName.Text = mappingName;

                // 重新加载Excel列和系统字段列表
                LoadSystemFields();
                if (ExcelData != null && ExcelData.Rows.Count > 0)
                {
                    LoadExcelColumns();
                }

                // 从列表中移除已映射的项
                foreach (var mapping in ColumnMappings)
                {
                    RemoveFromExcelColumns(mapping.ExcelColumn);
                    RemoveFromSystemFields(mapping.SystemField);
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

                // 清空现有映射
                ColumnMappings.Clear();

                // 自动匹配列名和字段名
                foreach (var excelColumnItem in listBoxExcelColumns.Items)
                {
                    string excelColumn = excelColumnItem.ToString();

                    // 查找匹配的系统字段
                    foreach (var systemFieldItem in listBoxSystemFields.Items)
                    {
                        string systemFieldDisplay = systemFieldItem.ToString();
                        string systemField = systemFieldDisplay.StartsWith("* ")
                            ? systemFieldDisplay.Substring(2)
                            : systemFieldDisplay;

                        // 简单的字符串匹配（忽略大小写和空格）
                        if (string.Equals(excelColumn.Replace(" ", ""), systemField.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                        {
                            // 创建映射
                            var mapping = new ColumnMapping
                            {
                                ExcelColumn = excelColumn,
                                SystemField = systemField,
                                MappingName = textBoxMappingName.Text,
                                EntityType = TargetEntityType?.Name,
                                CreateTime = DateTime.Now,
                                UpdateTime = DateTime.Now
                            };

                            ColumnMappings.Add(mapping);

                            // 从两个列表中移除已选择的项
                            RemoveFromExcelColumns(excelColumn);
                            RemoveFromSystemFields(systemField);

                            break;
                        }
                    }
                }

                // 更新映射列表显示
                UpdateMappingsList();

                MessageBox.Show($"自动匹配完成，共找到 {ColumnMappings.Count} 个映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"自动匹配失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // 检查是否是系统生成或默认值的情况
            if (!string.IsNullOrEmpty(mapping.ExcelColumn) &&
                (mapping.ExcelColumn.StartsWith("[系统生成]") || mapping.ExcelColumn.StartsWith("[默认值")))
            {
                // 这些情况不需要恢复到Excel列表
                return;
            }

            // 如果有原始的Excel列名，恢复到列表中
            if (!string.IsNullOrEmpty(mapping.ExcelColumn) && !string.IsNullOrEmpty(mapping.SystemField))
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
            if (string.IsNullOrEmpty(mapping.SystemField))
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

                if (fieldName == mapping.SystemField)
                {
                    alreadyExists = true;
                    break;
                }
            }

            // 如果不存在，添加到列表中（检查是否必填）
            if (!alreadyExists)
            {
                bool isRequired = IsFieldRequired(mapping.SystemField);
                string displayText = isRequired ? $"* {mapping.SystemField}" : mapping.SystemField;
                listBoxSystemFields.Items.Add(displayText);
            }
        }
    }
}
