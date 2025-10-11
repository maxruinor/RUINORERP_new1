using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Plugin.OfficeAssistant.Models;
using RUINORERP.Plugin.OfficeAssistant.Shared;

namespace RUINORERP.Plugin.OfficeAssistant
{
    public partial class MainForm : Form
    {
        // 用于存储加载的Excel数据
        private DataTable oldFileData;
        private DataTable newFileData;
        
        // 用于存储列映射关系
        private Dictionary<string, string> columnMapping = new Dictionary<string, string>();
        
        // 应用程序配置
        private AppConfig appConfig;
        
        // 配置文件路径
        private readonly string configFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "appconfig.json");
        
        public MainForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            LoadAppConfig();
            
            // 注册窗体关闭事件
            this.FormClosing += MainForm_FormClosing;
        }
        
        /// <summary>
        /// 窗体关闭事件处理
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 在程序退出时保存配置
            SaveAppConfig();
        }
        
        /// <summary>
        /// 初始化自定义组件
        /// </summary>
        private void InitializeCustomComponents()
        {

        }
        
        /// <summary>
        /// 加载应用程序配置
        /// </summary>
        private void LoadAppConfig()
        {
            try
            {
                if (File.Exists(configFilePath))
                {
                    appConfig = AppConfig.Load(configFilePath);
                }
                else
                {
                    appConfig = new AppConfig();
                }
                
                // 应用配置
                ApplyConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appConfig = new AppConfig();
            }
        }
        
        /// <summary>
        /// 应用配置
        /// </summary>
        private void ApplyConfig()
        {
            // 设置文件路径
            txtOldFilePath.Text = appConfig.OldFilePath ?? "";
            txtNewFilePath.Text = appConfig.NewFilePath ?? "";
            
            // 设置自动加载选项
            chkAutoLoadFiles.Checked = appConfig.AutoLoadFiles;
            
            // 设置对比选项
            chkCaseSensitive.Checked = appConfig.CaseSensitive;
            chkIgnoreSpaces.Checked = appConfig.IgnoreSpaces;
            
            // 设置对比模式
            if (!string.IsNullOrEmpty(appConfig.ComparisonMode))
            {
                cmbComparisonMode.SelectedItem = appConfig.ComparisonMode;
            }
            
            // 如果启用了自动加载且文件路径都存在，则自动加载文件
            if (appConfig.AutoLoadFiles && 
                !string.IsNullOrEmpty(appConfig.OldFilePath) && 
                !string.IsNullOrEmpty(appConfig.NewFilePath) &&
                File.Exists(appConfig.OldFilePath) &&
                File.Exists(appConfig.NewFilePath))
            {
                // 延迟加载文件，确保界面已完全初始化
                Task.Delay(100).ContinueWith(_ => 
                {
                    this.Invoke(new Action(async () => await AutoLoadFiles()));
                });
            }
        }
        
        /// <summary>
        /// 自动加载文件
        /// </summary>
        private async Task AutoLoadFiles()
        {
            try
            {
                // 显示加载进度
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        progressBar.Maximum = 100;
                    }));
                }
                else
                {
                    progressBar.Style = ProgressBarStyle.Continuous;
                    progressBar.Value = 0;
                    progressBar.Maximum = 100;
                }

                // 设置进度为10%
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 10;
                    }));
                }
                else
                {
                    progressBar.Value = 10;
                }

                // 异步加载旧文件数据（只加载前10行用于预览）
                oldFileData = await ExcelHelper.ReadExcelDataAsync(appConfig.OldFilePath, 10);

                // 更新进度到50%
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 50;
                    }));
                }
                else
                {
                    progressBar.Value = 50;
                }

                // 异步加载新文件数据（只加载前10行用于预览）
                newFileData = await ExcelHelper.ReadExcelDataAsync(appConfig.NewFilePath, 10);

                // 更新进度到90%
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 90;
                    }));
                }
                else
                {
                    progressBar.Value = 90;
                }

                // 显示数据预览
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        DisplayDataPreview();
                    }));
                }
                else
                {
                    DisplayDataPreview();
                }

                // 填充列名列表
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        PopulateColumnLists();
                    }));
                }
                else
                {
                    PopulateColumnLists();
                }

                // 完成进度
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 100;
                    }));
                }
                else
                {
                    progressBar.Value = 100;
                }

                // 短暂延迟后重置进度条
                await Task.Delay(100);
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 0;
                    }));
                }
                else
                {
                    progressBar.Value = 0;
                }
            }
            catch (Exception ex)
            {
                // 重置进度条
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 0;
                    }));
                }
                else
                {
                    progressBar.Value = 0;
                }

                MessageBox.Show($"自动加载文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 保存应用程序配置
        /// </summary>
        private void SaveAppConfig()
        {
            try
            {
                // 确保配置对象不为null
                if (appConfig == null)
                {
                    appConfig = new AppConfig();
                }
                
                // 更新配置对象
                appConfig.OldFilePath = txtOldFilePath.Text;
                appConfig.NewFilePath = txtNewFilePath.Text;
                appConfig.AutoLoadFiles = chkAutoLoadFiles.Checked;
                appConfig.CaseSensitive = chkCaseSensitive.Checked;
                appConfig.IgnoreSpaces = chkIgnoreSpaces.Checked;
                appConfig.ComparisonMode = cmbComparisonMode.SelectedItem?.ToString();
                
                // 保存配置
                appConfig.Save(configFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        #region 文件选择区域事件处理
        
        /// <summary>
        /// 选择旧文件按钮点击事件
        /// </summary>
        private void btnSelectOldFile_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "选择旧Excel文件";
                openFileDialog.Filter = "Excel文件|*.xlsx;*.xls|所有文件|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtOldFilePath.Text = openFileDialog.FileName;
                    SaveAppConfig(); // 保存配置
                }
            }
        }
        
        /// <summary>
        /// 选择新文件按钮点击事件
        /// </summary>
        private void btnSelectNewFile_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "选择新文件Excel文件";
                openFileDialog.Filter = "Excel文件|*.xlsx;*.xls|所有文件|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtNewFilePath.Text = openFileDialog.FileName;
                    SaveAppConfig(); // 保存配置
                }
            }
        }
        
        /// <summary>
        /// 旧文件路径文本框拖拽进入事件
        /// </summary>
        private void txtOldFilePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        
        /// <summary>
        /// 旧文件路径文本框拖拽释放事件
        /// </summary>
        private void txtOldFilePath_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    txtOldFilePath.Text = files[0];
                    SaveAppConfig(); // 保存配置
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"拖拽文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 新文件路径文本框拖拽进入事件
        /// </summary>
        private void txtNewFilePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        
        /// <summary>
        /// 新文件路径文本框拖拽释放事件
        /// </summary>
        private void txtNewFilePath_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    txtNewFilePath.Text = files[0];
                    SaveAppConfig(); // 保存配置
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"拖拽文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 加载文件按钮点击事件
        /// </summary>
        private async void btnLoadFiles_Click(object sender, EventArgs e)
        {
            await LoadFilesAsync();
        }
        
        /// <summary>
        /// 显示数据预览
        /// </summary>
        private void DisplayDataPreview()
        {
            // 显示旧文件数据预览（使用已加载的数据）
            if (oldFileData != null)
            {
                dgvOldFilePreview.DataSource = oldFileData;
                
                // 更新旧文件概要信息
                UpdateFileSummary(lblOldFileSummary, oldFileData);
            }
            
            // 显示新文件数据预览（使用已加载的数据）
            if (newFileData != null)
            {
                dgvNewFilePreview.DataSource = newFileData;
                
                // 更新新文件概要信息
                UpdateFileSummary(lblNewFileSummary, newFileData);
            }
        }
        
        /// <summary>
        /// 更新文件概要信息
        /// </summary>
        /// <param name="summaryLabel">用于显示概要信息的Label控件</param>
        /// <param name="data">数据表</param>
        private void UpdateFileSummary(Label summaryLabel, DataTable data)
        {
            if (data != null)
            {
                summaryLabel.Text = $"行数: {data.Rows.Count}, 列数: {data.Columns.Count}";
            }
            else
            {
                summaryLabel.Text = "行数: 0, 列数: 0";
            }
        }

        /// <summary>
        /// 填充列名列表
        /// </summary>
        private void PopulateColumnLists()
        {
            // 清空现有列表
            lstOldColumns.Items.Clear();
            lstNewColumns.Items.Clear();
            lstKeyColumns.Items.Clear();
            lstCompareColumns.Items.Clear();
            
            // 填充旧文件列名
            if (oldFileData != null)
            {
                foreach (DataColumn column in oldFileData.Columns)
                {
                    lstOldColumns.Items.Add(column.ColumnName);
                }
            }
            
            // 填充新文件列名
            if (newFileData != null)
            {
                foreach (DataColumn column in newFileData.Columns)
                {
                    lstNewColumns.Items.Add(column.ColumnName);
                }
            }
        }
        
        #endregion
        
        #region 列映射配置区域事件处理
        
        /// <summary>
        /// 自动匹配列按钮点击事件
        /// </summary>
        private void btnAutoMatchColumns_Click(object sender, EventArgs e)
        {
            if (oldFileData == null || newFileData == null)
            {
                MessageBox.Show("请先加载文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // 清空现有映射
            columnMapping.Clear();
            
            // 自动匹配列名相同的列
            foreach (DataColumn oldColumn in oldFileData.Columns)
            {
                foreach (DataColumn newColumn in newFileData.Columns)
                {
                    if (oldColumn.ColumnName.Equals(newColumn.ColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        columnMapping[oldColumn.ColumnName] = newColumn.ColumnName;
                        break;
                    }
                }
            }
            
            MessageBox.Show($"自动匹配完成，共匹配到 {columnMapping.Count} 个相同列名的列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// 保存映射配置按钮点击事件
        /// </summary>
        private void btnSaveMapping_Click(object sender, EventArgs e)
        {
            // TODO: 实现保存映射配置功能
            MessageBox.Show("保存映射配置功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// 加载映射配置按钮点击事件
        /// </summary>
        private void btnLoadMapping_Click(object sender, EventArgs e)
        {
            // TODO: 实现加载映射配置功能
            MessageBox.Show("加载映射配置功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// 添加键列按钮点击事件
        /// </summary>
        private void btnAddKeyColumn_Click(object sender, EventArgs e)
        {
            if (lstOldColumns.SelectedItems.Count == 0 || lstNewColumns.SelectedItems.Count == 0)
            {
                MessageBox.Show("请在两个列表中各选择一个列作为键列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (lstOldColumns.SelectedItems.Count != lstNewColumns.SelectedItems.Count)
            {
                MessageBox.Show("请选择相同数量的旧文件列和新文件列作为键列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // 添加到键列列表
            for (int i = 0; i < lstOldColumns.SelectedItems.Count; i++)
            {
                var oldColumn = lstOldColumns.SelectedItems[i].ToString();
                var newColumn = lstNewColumns.SelectedItems[i].ToString();
                
                // 检查是否已存在
                bool exists = false;
                foreach (var item in lstKeyColumns.Items)
                {
                    if (item.ToString().StartsWith($"{oldColumn} -> "))
                    {
                        exists = true;
                        break;
                    }
                }
                
                if (!exists)
                {
                    lstKeyColumns.Items.Add($"{oldColumn} -> {newColumn}");
                }
            }
        }
        
        /// <summary>
        /// 添加比较列按钮点击事件
        /// </summary>
        private void btnAddCompareColumn_Click(object sender, EventArgs e)
        {
            if (lstOldColumns.SelectedItems.Count == 0 || lstNewColumns.SelectedItems.Count == 0)
            {
                MessageBox.Show("请在两个列表中各选择一个列作为比较列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (lstOldColumns.SelectedItems.Count != lstNewColumns.SelectedItems.Count)
            {
                MessageBox.Show("请选择相同数量的旧文件列和新文件列作为比较列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // 添加到比较列列表
            for (int i = 0; i < lstOldColumns.SelectedItems.Count; i++)
            {
                var oldColumn = lstOldColumns.SelectedItems[i].ToString();
                var newColumn = lstNewColumns.SelectedItems[i].ToString();
                
                // 检查是否已存在
                bool exists = false;
                foreach (var item in lstCompareColumns.Items)
                {
                    if (item.ToString().StartsWith($"{oldColumn} -> "))
                    {
                        exists = true;
                        break;
                    }
                }
                
                if (!exists)
                {
                    lstCompareColumns.Items.Add($"{oldColumn} -> {newColumn}");
                }
            }
        }
        
        #endregion
        
        #region 对比设置区域事件处理
        
        /// <summary>
        /// 区分大小写复选框状态改变事件
        /// </summary>
        private void chkCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            SaveAppConfig(); // 保存配置
        }
        
        /// <summary>
        /// 忽略空格复选框状态改变事件
        /// </summary>
        private void chkIgnoreSpaces_CheckedChanged(object sender, EventArgs e)
        {
            SaveAppConfig(); // 保存配置
        }
        
        /// <summary>
        /// 对比模式下拉框选择改变事件
        /// </summary>
        private void cmbComparisonMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveAppConfig(); // 保存配置
        }
        
        /// <summary>
        /// 自动加载文件复选框状态改变事件
        /// </summary>
        private async void chkAutoLoadFiles_CheckedChanged(object sender, EventArgs e)
        {
            SaveAppConfig(); // 保存配置
            
            // 如果勾选了自动加载且文件路径都存在，则不立即加载文件
            // 文件将在ApplyConfig方法中通过AutoLoadFiles异步加载
            if (chkAutoLoadFiles.Checked && 
                !string.IsNullOrEmpty(txtOldFilePath.Text) && 
                !string.IsNullOrEmpty(txtNewFilePath.Text) &&
                File.Exists(txtOldFilePath.Text) &&
                File.Exists(txtNewFilePath.Text))
            {
                // 只保存配置，不立即加载文件
                // 文件加载由ApplyConfig中的自动加载逻辑处理
            }
        }
        
        /// <summary>
        /// 加载文件的异步方法
        /// </summary>
        private async Task LoadFilesAsync()
        {
            try
            {
                // 检查文件路径
                if (string.IsNullOrEmpty(txtOldFilePath.Text) || string.IsNullOrEmpty(txtNewFilePath.Text))
                {
                    MessageBox.Show("请选择两个Excel文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查文件是否存在
                if (!File.Exists(txtOldFilePath.Text))
                {
                    MessageBox.Show("旧文件不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(txtNewFilePath.Text))
                {
                    MessageBox.Show("新文件不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 检查文件扩展名
                string oldFileExt = Path.GetExtension(txtOldFilePath.Text)?.ToLower();
                string newFileExt = Path.GetExtension(txtNewFilePath.Text)?.ToLower();

                if (oldFileExt != ".xlsx" && oldFileExt != ".xls")
                {
                    MessageBox.Show("旧文件不是Excel文件格式(.xlsx或.xls)", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (newFileExt != ".xlsx" && newFileExt != ".xls")
                {
                    MessageBox.Show("新文件不是Excel文件格式(.xlsx或.xls)", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 保存配置
                SaveAppConfig();

                // 显示加载进度
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        progressBar.Maximum = 100;
                    }));
                }
                else
                {
                    progressBar.Style = ProgressBarStyle.Continuous;
                    progressBar.Value = 0;
                    progressBar.Maximum = 100;
                }

                // 设置进度为10%
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 10;
                    }));
                }
                else
                {
                    progressBar.Value = 10;
                }

                // 异步加载旧文件数据（只加载前10行用于预览）
                oldFileData = await ExcelHelper.ReadExcelDataAsync(txtOldFilePath.Text, 10);

                // 更新进度到50%
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 50;
                    }));
                }
                else
                {
                    progressBar.Value = 50;
                }

                // 异步加载新文件数据（只加载前10行用于预览）
                newFileData = await ExcelHelper.ReadExcelDataAsync(txtNewFilePath.Text, 10);

                // 更新进度到90%
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 90;
                    }));
                }
                else
                {
                    progressBar.Value = 90;
                }

                // 显示数据预览
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        DisplayDataPreview();
                    }));
                }
                else
                {
                    DisplayDataPreview();
                }

                // 填充列名列表
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        PopulateColumnLists();
                    }));
                }
                else
                {
                    PopulateColumnLists();
                }

                // 完成进度
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 100;
                    }));
                }
                else
                {
                    progressBar.Value = 100;
                }

                // 短暂延迟后重置进度条
                await Task.Delay(100);
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 0;
                    }));
                }
                else
                {
                    progressBar.Value = 0;
                }

                MessageBox.Show("文件加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // 重置进度条
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 0;
                    }));
                }
                else
                {
                    progressBar.Value = 0;
                }

                MessageBox.Show($"加载文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
        
        #region 操作按钮区域事件处理
        
        /// <summary>
        /// 开始对比按钮点击事件
        /// </summary>
        private void btnStartComparison_Click(object sender, EventArgs e)
        {
            // TODO: 实现开始对比功能
            MessageBox.Show("开始对比功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// 导出结果按钮点击事件
        /// </summary>
        private void btnExportResults_Click(object sender, EventArgs e)
        {
            // TODO: 实现导出结果功能
            MessageBox.Show("导出结果功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// 清除结果按钮点击事件
        /// </summary>
        private void btnClearResults_Click(object sender, EventArgs e)
        {
            // TODO: 实现清除结果功能
            MessageBox.Show("清除结果功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        #endregion
    }
}