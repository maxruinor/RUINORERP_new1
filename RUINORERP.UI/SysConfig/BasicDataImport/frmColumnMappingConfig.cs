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
        /// 窗体加载事件
        /// </summary>
        private void frmColumnMappingConfig_Load(object sender, EventArgs e)
        {
            try
            {
                // 设置配置文件路径
                _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfigFiles", "ColumnMappings");

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
            if (listBoxExcelColumns.SelectedItem == null)
            {
                MessageBox.Show("请选择Excel列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBoxSystemFields.SelectedItem == null)
            {
                MessageBox.Show("请选择系统字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string excelColumn = listBoxExcelColumns.SelectedItem.ToString();
            string systemFieldDisplay = listBoxSystemFields.SelectedItem.ToString();

            // 去掉必填标识
            string systemField = systemFieldDisplay.StartsWith("* ") 
                ? systemFieldDisplay.Substring(2) 
                : systemFieldDisplay;

            // 检查是否已存在该映射
            if (ColumnMappings.GetMappingByExcelColumn(excelColumn) != null)
            {
                MessageBox.Show("该Excel列已映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            // 添加到集合
            ColumnMappings.Add(mapping);

            // 更新映射列表显示
            UpdateMappingsList();
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

            using (var propertyDialog = new FrmColumnPropertyConfig
            {
                CurrentMapping = mapping,
                TargetEntityType = TargetEntityType
            })
            {
                if (propertyDialog.ShowDialog() == DialogResult.OK)
                {
                    // 更新映射属性
                    mapping.IsForeignKey = propertyDialog.IsForeignKey;
                    mapping.IsUniqueValue = propertyDialog.IsUniqueValue;
                    mapping.DefaultValue = propertyDialog.DefaultValue;
                    mapping.IsSystemGenerated = propertyDialog.IsSystemGenerated;
                    mapping.RelatedTableName = propertyDialog.RelatedTableName;
                    mapping.RelatedTableField = propertyDialog.RelatedTableField;
                    mapping.RelatedTableFieldName = propertyDialog.RelatedTableFieldName;
                    mapping.UpdateTime = DateTime.Now;

                    // 更新显示
                    UpdateMappingsList();
                }
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
                // 新建映射
                textBoxMappingName.Text = string.Empty;
                ColumnMappings.Clear();
                UpdateMappingsList();
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

            // 清空映射列表显示
            UpdateMappingsList();

            // 重置已保存配置选择
            comboBoxSavedMappings.SelectedIndex = 0;

            // 更新窗口标题
            SetWindowTitle();
        }
    }
}
