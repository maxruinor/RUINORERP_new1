using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.UI.Common;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列映射配置窗体
    /// 用于配置Excel列与系统字段的映射关系
    /// </summary>
    public partial class frmColumnMappingConfig : KryptonForm
    {
        /// <summary>
        /// 映射配置管理器
        /// </summary>
        private ColumnMappingManager _mappingManager;

        /// <summary>
        /// Excel数据表格
        /// </summary>
        public DataTable ExcelData { get; set; }

        /// <summary>
        /// 导入目标实体类型
        /// </summary>
        public Type TargetEntityType { get; set; }

        /// <summary>
        /// 列映射配置集合
        /// </summary>
        public ColumnMappingCollection ColumnMappings { get; set; }

        /// <summary>
        /// 映射配置保存成功事件
        /// </summary>
        public event EventHandler MappingSaved;

        /// <summary>
        /// 最后保存的映射配置名称
        /// </summary>
        public string SavedMappingName { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmColumnMappingConfig()
        {
            InitializeComponent();
            _mappingManager = new ColumnMappingManager();
        }
        
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void frmColumnMappingConfig_Load(object sender, EventArgs e)
        {
            // 初始化映射配置集合
            if (ColumnMappings == null)
            {
                ColumnMappings = new ColumnMappingCollection();
            }
            
            // 加载Excel列名
            LoadExcelColumns();
            
            // 加载系统字段
            LoadSystemFields();
            
            // 加载现有映射配置
            LoadExistingMappings();
        }
        
        /// <summary>
        /// 加载Excel列名
        /// </summary>
        private void LoadExcelColumns()
        {
            if (ExcelData == null || ExcelData.Columns.Count == 0)
            {
                MessageBox.Show("Excel数据为空，无法加载列名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            listBoxExcelColumns.Items.Clear();
            foreach (DataColumn column in ExcelData.Columns)
            {
                listBoxExcelColumns.Items.Add(column.ColumnName);
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

                // 使用UIHelper获取实体的字段信息
                var fieldNameList = UIHelper.GetFieldNameList(false, TargetEntityType);

                listBoxSystemFields.Items.Clear();
                foreach (var field in fieldNameList)
                {
                    string fieldInfo = $"{field.Key} - {field.Value}";
                    listBoxSystemFields.Items.Add(fieldInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载系统字段失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 加载现有映射配置
        /// </summary>
        private void LoadExistingMappings()
        {
            if (ColumnMappings == null || ColumnMappings.Count == 0)
            {
                return;
            }
            
            listBoxMappings.Items.Clear();
            foreach (var mapping in ColumnMappings)
            {
                string mappingInfo = $"{mapping.ExcelColumn} → {mapping.SystemField}";
                if (mapping.IsUniqueKey)
                {
                    mappingInfo += " (唯一键)";
                }
                listBoxMappings.Items.Add(mappingInfo);
            }
        }
        
        /// <summary>
        /// 添加映射按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnAddMapping_Click(object sender, EventArgs e)
        {
            if (listBoxExcelColumns.SelectedItem == null || listBoxSystemFields.SelectedItem == null)
            {
                MessageBox.Show("请选择要映射的Excel列和系统字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string excelColumn = listBoxExcelColumns.SelectedItem.ToString();
            string systemFieldInfo = listBoxSystemFields.SelectedItem.ToString();
            string systemField = systemFieldInfo.Split('-')[0].Trim();
            string dataType = systemFieldInfo.Split('-')[1].Trim();
            
            // 检查是否已存在映射
            if (ColumnMappings.Any(m => m.ExcelColumn == excelColumn || m.SystemField == systemField))
            {
                MessageBox.Show("该Excel列或系统字段已存在映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // 创建映射配置
            var mapping = new ColumnMapping
            {
                ExcelColumn = excelColumn,
                SystemField = systemField,
                DataType = dataType,
                IsUniqueKey = false,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            
            ColumnMappings.Add(mapping);
            
            // 更新映射列表
            string mappingInfo = $"{mapping.ExcelColumn} → {mapping.SystemField}";
            listBoxMappings.Items.Add(mappingInfo);
            
            // 从列表中移除已映射的项
            listBoxExcelColumns.Items.Remove(excelColumn);
            listBoxSystemFields.Items.Remove(systemFieldInfo);
        }
        
        /// <summary>
        /// 删除映射按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnRemoveMapping_Click(object sender, EventArgs e)
        {
            if (listBoxMappings.SelectedItem == null)
            {
                MessageBox.Show("请选择要删除的映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string selectedMapping = listBoxMappings.SelectedItem.ToString();
            string[] mappingParts = selectedMapping.Split(new string[] { " → " }, StringSplitOptions.None);
            if (mappingParts.Length != 2)
            {
                MessageBox.Show("无效的映射格式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string excelColumn = mappingParts[0];
            string systemField = mappingParts[1].Split('-')[0].Trim();
            
            // 查找并删除映射
            var mappingToRemove = ColumnMappings.FirstOrDefault(m => m.ExcelColumn == excelColumn && m.SystemField == systemField);
            if (mappingToRemove != null)
            {
                ColumnMappings.Remove(mappingToRemove);
                
                // 更新列表
                listBoxMappings.Items.Remove(selectedMapping);
                
                // 将项添加回原始列表
                listBoxExcelColumns.Items.Add(excelColumn);
                
                // 重新加载系统字段，因为可能需要重新获取字段信息
                LoadSystemFields();
            }
        }
        
        /// <summary>
        /// 设置唯一键按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnSetUniqueKey_Click(object sender, EventArgs e)
        {
            if (listBoxMappings.SelectedItem == null)
            {
                MessageBox.Show("请选择要设置为唯一键的映射", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // 清除所有唯一键标记
            foreach (var mapping in ColumnMappings)
            {
                mapping.IsUniqueKey = false;
            }
            
            // 设置选中项为唯一键
            string selectedMapping = listBoxMappings.SelectedItem.ToString();
            string[] mappingParts = selectedMapping.Split(new string[] { " → " }, StringSplitOptions.None);
            if (mappingParts.Length == 2)
            {
                string excelColumn = mappingParts[0];
                string systemField = mappingParts[1].Split('-')[0].Trim();
                
                var mappingToSet = ColumnMappings.FirstOrDefault(m => m.ExcelColumn == excelColumn && m.SystemField == systemField);
                if (mappingToSet != null)
                {
                    mappingToSet.IsUniqueKey = true;
                }
            }
            
            // 更新映射列表
            LoadExistingMappings();
        }
        
        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnSave_Click(object sender, EventArgs e)
        {
            if (ColumnMappings.Count == 0)
            {
                MessageBox.Show("请至少配置一个映射关系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 检查是否设置了唯一键
            if (!ColumnMappings.Any(m => m.IsUniqueKey))
            {
                if (MessageBox.Show("尚未设置唯一键，这可能导致数据重复导入。是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            // 询问是否保存配置
            if (MessageBox.Show("是否保存当前映射配置，以便下次使用？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SaveMappingToFile();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 保存映射配置到文件
        /// </summary>
        private void SaveMappingToFile()
        {
            try
            {
                string mappingName = Microsoft.VisualBasic.Interaction.InputBox("请输入映射配置名称", "保存映射配置", "");
                if (string.IsNullOrWhiteSpace(mappingName))
                {
                    return;
                }

                // 检查配置是否已存在
                if (_mappingManager.MappingExists(mappingName))
                {
                    if (MessageBox.Show($"配置名称 \"{mappingName}\" 已存在，是否覆盖？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                // 保存配置
                _mappingManager.SaveMapping(ColumnMappings, mappingName, TargetEntityType?.Name);
                SavedMappingName = mappingName;

                // 触发映射配置保存成功事件
                MappingSaved?.Invoke(this, EventArgs.Empty);

                MessageBox.Show($"映射配置 \"{mappingName}\" 保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存映射配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// 自动匹配按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnAutoMatch_Click(object sender, EventArgs e)
        {
            // 自动匹配列名相同的Excel列和系统字段
            List<string> matchedExcelColumns = new List<string>();
            List<string> matchedSystemFields = new List<string>();
            
            foreach (string excelColumn in listBoxExcelColumns.Items)
            {
                foreach (string systemFieldInfo in listBoxSystemFields.Items)
                {
                    string systemField = systemFieldInfo.Split('(')[0].Trim();
                    
                    if (string.Equals(excelColumn, systemField, StringComparison.OrdinalIgnoreCase))
                    {
                        // 创建映射配置
                        var mapping = new ColumnMapping
                        {
                            ExcelColumn = excelColumn,
                            SystemField = systemField,
                            DataType = systemFieldInfo.Split('-')[1].Trim(),
                            IsUniqueKey = false,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        
                        ColumnMappings.Add(mapping);
                        matchedExcelColumns.Add(excelColumn);
                        matchedSystemFields.Add(systemFieldInfo);
                        break;
                    }
                }
            }
            
            // 移除已匹配的项
            foreach (string excelColumn in matchedExcelColumns)
            {
                listBoxExcelColumns.Items.Remove(excelColumn);
            }
            
            foreach (string systemFieldInfo in matchedSystemFields)
            {
                listBoxSystemFields.Items.Remove(systemFieldInfo);
            }
            
            // 更新映射列表
            LoadExistingMappings();
            
            MessageBox.Show($"自动匹配完成，共匹配 {matchedExcelColumns.Count} 个字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}