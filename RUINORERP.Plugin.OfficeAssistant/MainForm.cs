using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Drawing;
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
        private readonly string configFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "office_assistant_config.json");
        
        // 标记是否已完成配置加载，避免在加载前保存空配置
        private bool isConfigLoaded = false;
        
        // 是否显示相同数据
        private bool showSameData = false;
        
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
            // 初始化对比模式下拉框选项
            cmbComparisonMode.Items.Add("存在性检查");
            cmbComparisonMode.Items.Add("数据差异");
            cmbComparisonMode.Items.Add("自定义列对比");
            cmbComparisonMode.SelectedIndex = 0; // 默认选择"存在性检查"

            // 添加帮助菜单
            var helpMenu = new ToolStripMenuItem("帮助(&H)");
            var aboutItem = new ToolStripMenuItem("关于(&A)");
            aboutItem.Click += (sender, e) => {
                var aboutBox = new AboutBox();
                aboutBox.ShowDialog(this);
            };
            helpMenu.DropDownItems.Add(aboutItem);
            
            // 将帮助菜单添加到主菜单栏
            if (this.MainMenuStrip != null)
            {
                this.MainMenuStrip.Items.Add(helpMenu);
            }
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
            finally
            {
                // 标记配置已加载完成，此后可以保存配置
                isConfigLoaded = true;
            }
        }
        
        /// <summary>
        /// 应用配置
        /// </summary>
        private void ApplyConfig()
        {
            try
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
                
                // 应用键列映射
                lstKeyColumns.Items.Clear();
                if (appConfig.KeyColumns != null)
                {
                    foreach (var keyColumn in appConfig.KeyColumns)
                    {
                        lstKeyColumns.Items.Add(keyColumn);
                    }
                }
                
                // 应用比较列映射
                lstCompareColumns.Items.Clear();
                if (appConfig.CompareColumns != null)
                {
                    foreach (var compareColumn in appConfig.CompareColumns)
                    {
                        lstCompareColumns.Items.Add(compareColumn);
                    }
                }
                
                // 如果启用了自动加载且文件路径都存在，则自动加载文件
                if (appConfig.AutoLoadFiles && 
                    !string.IsNullOrEmpty(appConfig.OldFilePath) && 
                    !string.IsNullOrEmpty(appConfig.NewFilePath) &&
                    File.Exists(appConfig.OldFilePath) &&
                    File.Exists(appConfig.NewFilePath))
                {
                    // 延迟加载文件，确保界面已完全初始化
                    Task.Run(async () => 
                    {
                        await Task.Delay(500); // 增加延迟时间以确保界面完全加载
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(async () => await AutoLoadFiles()));
                        }
                        else
                        {
                            await AutoLoadFiles();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // 更新进度到30%
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => {
                        progressBar.Value = 30;
                    }));
                }
                else
                {
                    progressBar.Value = 30;
                }

                // 异步加载新文件数据（只加载前10行用于预览）
                newFileData = await ExcelHelper.ReadExcelDataAsync(appConfig.NewFilePath, 10);

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

                // 填充工作表列表
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        PopulateWorksheetLists();
                    }));
                }
                else
                {
                    PopulateWorksheetLists();
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
            // 如果配置尚未加载完成，避免保存空配置
            if (!isConfigLoaded)
                return;
                
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
                
                // 保存键列映射
                appConfig.KeyColumns = new List<string>();
                foreach (var item in lstKeyColumns.Items)
                {
                    appConfig.KeyColumns.Add(item.ToString());
                }
                
                // 保存比较列映射
                appConfig.CompareColumns = new List<string>();
                foreach (var item in lstCompareColumns.Items)
                {
                    appConfig.CompareColumns.Add(item.ToString());
                }
                
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
        
        /// <summary>
        /// 填充工作表列表
        /// </summary>
        private void PopulateWorksheetLists()
        {
            try
            {
                // 清空现有列表
                cmbOldWorksheet.Items.Clear();
                cmbNewWorksheet.Items.Clear();
                
                // 填充旧文件工作表列表
                if (!string.IsNullOrEmpty(txtOldFilePath.Text) && File.Exists(txtOldFilePath.Text))
                {
                    var oldWorksheets = ExcelHelper.GetWorksheetNames(txtOldFilePath.Text);
                    foreach (var worksheet in oldWorksheets)
                    {
                        cmbOldWorksheet.Items.Add(worksheet);
                    }
                    
                    if (cmbOldWorksheet.Items.Count > 0)
                    {
                        cmbOldWorksheet.SelectedIndex = 0;
                    }
                }
                
                // 填充新文件工作表列表
                if (!string.IsNullOrEmpty(txtNewFilePath.Text) && File.Exists(txtNewFilePath.Text))
                {
                    var newWorksheets = ExcelHelper.GetWorksheetNames(txtNewFilePath.Text);
                    foreach (var worksheet in newWorksheets)
                    {
                        cmbNewWorksheet.Items.Add(worksheet);
                    }
                    
                    if (cmbNewWorksheet.Items.Count > 0)
                    {
                        cmbNewWorksheet.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载工作表列表时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                // 保存当前的键列和比较列映射到配置
                SaveAppConfig();
                MessageBox.Show("映射配置已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存映射配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 加载映射配置按钮点击事件
        /// </summary>
        private void btnLoadMapping_Click(object sender, EventArgs e)
        {
            try
            {
                // 从配置文件加载键列和比较列映射
                if (File.Exists(configFilePath))
                {
                    var config = AppConfig.Load(configFilePath);
                    
                    // 应用键列映射
                    lstKeyColumns.Items.Clear();
                    if (config.KeyColumns != null)
                    {
                        foreach (var keyColumn in config.KeyColumns)
                        {
                            lstKeyColumns.Items.Add(keyColumn);
                        }
                    }
                    
                    // 应用比较列映射
                    lstCompareColumns.Items.Clear();
                    if (config.CompareColumns != null)
                    {
                        foreach (var compareColumn in config.CompareColumns)
                        {
                            lstCompareColumns.Items.Add(compareColumn);
                        }
                    }
                    
                    MessageBox.Show("映射配置已加载", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("未找到配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载映射配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        
        /// <summary>
        /// 键列列表双击事件处理
        /// </summary>
        private void lstKeyColumns_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstKeyColumns.SelectedItem != null)
            {
                // 确认是否删除
                var result = MessageBox.Show($"确定要删除键列映射 '{lstKeyColumns.SelectedItem}' 吗？", 
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    lstKeyColumns.Items.Remove(lstKeyColumns.SelectedItem);
                    SaveAppConfig(); // 保存配置
                }
            }
        }
        
        /// <summary>
        /// 比较列列表双击事件处理
        /// </summary>
        private void lstCompareColumns_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstCompareColumns.SelectedItem != null)
            {
                // 确认是否删除
                var result = MessageBox.Show($"确定要删除比较列映射 '{lstCompareColumns.SelectedItem}' 吗？", 
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    lstCompareColumns.Items.Remove(lstCompareColumns.SelectedItem);
                    SaveAppConfig(); // 保存配置
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
        private void chkAutoLoadFiles_CheckedChanged(object sender, EventArgs e)
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

                // 移除提示信息，用户可以通过预览看到数据是否加载成功
                // MessageBox.Show("文件加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            try
            {
                // 检查是否已加载文件
                if (oldFileData == null || newFileData == null)
                {
                    MessageBox.Show("请先加载Excel文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查是否选择了键列
                if (lstKeyColumns.Items.Count == 0)
                {
                    MessageBox.Show("请先选择键列映射，用于确定记录的唯一性。对比操作必须基于键列进行，键列用于标识每条记录的唯一性。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查键列映射是否正确
                foreach (var item in lstKeyColumns.Items)
                {
                    var mapping = item.ToString();
                    var parts = mapping.Split(new string[] { " -> " }, StringSplitOptions.None);
                    if (parts.Length != 2)
                    {
                        MessageBox.Show($"键列映射 '{mapping}' 格式不正确，请重新设置键列映射。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 检查是否选择了比较列
                if (lstCompareColumns.Items.Count == 0)
                {
                    var msgResult = MessageBox.Show("未选择比较列映射，将比较所有数据列。是否继续？", "提示", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msgResult == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    // 检查比较列映射是否正确
                    foreach (var item in lstCompareColumns.Items)
                    {
                        var mapping = item.ToString();
                        var parts = mapping.Split(new string[] { " -> " }, StringSplitOptions.None);
                        if (parts.Length != 2)
                        {
                            MessageBox.Show($"比较列映射 '{mapping}' 格式不正确，请重新设置比较列映射。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // 构建对比配置
                var config = new Core.ComparisonConfig
                {
                    CaseSensitive = chkCaseSensitive.Checked,
                    IgnoreSpaces = chkIgnoreSpaces.Checked,
                    OldKeyColumns = new List<int>(),
                    NewKeyColumns = new List<int>(),
                    OldCompareColumns = new List<int>(),
                    NewCompareColumns = new List<int>(),
                    CompareColumns = new List<int>(),
                    OldWorksheetName = cmbOldWorksheet.SelectedItem?.ToString(),
                    NewWorksheetName = cmbNewWorksheet.SelectedItem?.ToString(),
                    Mode = GetComparisonMode()
                };

                // 解析键列映射
                foreach (var item in lstKeyColumns.Items)
                {
                    var mapping = item.ToString();
                    var parts = mapping.Split(new string[] { " -> " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        var oldColumnIndex = oldFileData.Columns.IndexOf(parts[0]);
                        var newColumnIndex = newFileData.Columns.IndexOf(parts[1]);
                        
                        if (oldColumnIndex >= 0 && newColumnIndex >= 0)
                        {
                            config.OldKeyColumns.Add(oldColumnIndex);
                            config.NewKeyColumns.Add(newColumnIndex);
                        }
                        else
                        {
                            MessageBox.Show($"键列映射 '{mapping}' 中的列在数据中未找到，请检查列名是否正确", 
                                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // 解析比较列映射
                foreach (var item in lstCompareColumns.Items)
                {
                    var mapping = item.ToString();
                    var parts = mapping.Split(new string[] { " -> " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        var oldColumnIndex = oldFileData.Columns.IndexOf(parts[0]);
                        var newColumnIndex = newFileData.Columns.IndexOf(parts[1]);
                        
                        if (oldColumnIndex >= 0 && newColumnIndex >= 0)
                        {
                            config.OldCompareColumns.Add(oldColumnIndex);
                            config.NewCompareColumns.Add(newColumnIndex);
                            config.CompareColumns.Add(oldColumnIndex);
                        }
                        else
                        {
                            MessageBox.Show($"比较列映射 '{mapping}' 中的列在数据中未找到，请检查列名是否正确", 
                                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // 如果没有指定比较列，则比较所有列（除了键列）
                if (config.OldCompareColumns.Count == 0)
                {
                    for (int i = 0; i < oldFileData.Columns.Count; i++)
                    {
                        // 跳过键列
                        if (!config.OldKeyColumns.Contains(i))
                        {
                            config.OldCompareColumns.Add(i);
                            // 尝试在新数据中找到同名列
                            var columnName = oldFileData.Columns[i].ColumnName;
                            var newIndex = newFileData.Columns.IndexOf(columnName);
                            if (newIndex >= 0)
                            {
                                config.NewCompareColumns.Add(newIndex);
                            }
                            else
                            {
                                // 如果找不到同名列，则使用相同索引（如果存在）
                                config.NewCompareColumns.Add(Math.Min(i, newFileData.Columns.Count - 1));
                            }
                            config.CompareColumns.Add(i);
                        }
                    }
                }

                // 执行对比
                var engine = new Core.ComparisonEngine();
                var result = engine.Compare(txtOldFilePath.Text, txtNewFilePath.Text, config);

                // 在主界面显示结果摘要
                DisplayResultSummary(result);

                // 显示结果摘要
                var summaryMessage = $"对比完成:\n" +
                                $"旧数据文件记录数: {result.Summary.TotalOldRecords}\n" +
                                $"新数据文件记录数: {result.Summary.TotalNewRecords}\n" +
                                $"新数据有旧数据没有: {result.Summary.AddedCount}\n" +
                                $"旧数据有新数据没有: {result.Summary.DeletedCount}\n" +
                                $"数据发生变化: {result.Summary.ModifiedCount}";
                
                var dialogResult = MessageBox.Show(summaryMessage + "\n\n是否查看详细结果？", "对比结果", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                
                if (dialogResult == DialogResult.Yes)
                {
                    // 显示详细结果
                    var resultForm = new ComparisonResultForm(result);
                    resultForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"对比过程中发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 导出结果按钮点击事件
        /// </summary>
        private void btnExportResults_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否已加载文件
                if (oldFileData == null || newFileData == null)
                {
                    MessageBox.Show("请先加载Excel文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查是否选择了键列
                if (lstKeyColumns.Items.Count == 0)
                {
                    MessageBox.Show("请先选择键列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 构建对比配置
                var config = new Core.ComparisonConfig
                {
                    CaseSensitive = chkCaseSensitive.Checked,
                    IgnoreSpaces = chkIgnoreSpaces.Checked,
                    OldKeyColumns = new List<int>(),
                    NewKeyColumns = new List<int>(),
                    OldCompareColumns = new List<int>(),
                    NewCompareColumns = new List<int>(),
                    CompareColumns = new List<int>(),
                    OldWorksheetName = cmbOldWorksheet.SelectedItem?.ToString(),
                    NewWorksheetName = cmbNewWorksheet.SelectedItem?.ToString(),
                    Mode = GetComparisonMode()
                };

                // 解析键列映射
                foreach (var item in lstKeyColumns.Items)
                {
                    var mapping = item.ToString();
                    var parts = mapping.Split(new string[] { " -> " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        var oldColumnIndex = oldFileData.Columns.IndexOf(parts[0]);
                        var newColumnIndex = newFileData.Columns.IndexOf(parts[1]);
                        
                        if (oldColumnIndex >= 0 && newColumnIndex >= 0)
                        {
                            config.OldKeyColumns.Add(oldColumnIndex);
                            config.NewKeyColumns.Add(newColumnIndex);
                        }
                    }
                }

                // 解析比较列映射
                foreach (var item in lstCompareColumns.Items)
                {
                    var mapping = item.ToString();
                    var parts = mapping.Split(new string[] { " -> " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        var oldColumnIndex = oldFileData.Columns.IndexOf(parts[0]);
                        var newColumnIndex = newFileData.Columns.IndexOf(parts[1]);
                        
                        if (oldColumnIndex >= 0 && newColumnIndex >= 0)
                        {
                            config.OldCompareColumns.Add(oldColumnIndex);
                            config.NewCompareColumns.Add(newColumnIndex);
                            config.CompareColumns.Add(oldColumnIndex);
                        }
                    }
                }

                // 如果没有指定比较列，则比较所有列（除了键列）
                if (config.OldCompareColumns.Count == 0)
                {
                    for (int i = 0; i < oldFileData.Columns.Count; i++)
                    {
                        // 跳过键列
                        if (!config.OldKeyColumns.Contains(i))
                        {
                            config.OldCompareColumns.Add(i);
                            // 尝试在新数据中找到同名列
                            var columnName = oldFileData.Columns[i].ColumnName;
                            var newIndex = newFileData.Columns.IndexOf(columnName);
                            if (newIndex >= 0)
                            {
                                config.NewCompareColumns.Add(newIndex);
                            }
                            else
                            {
                                // 如果找不到同名列，则使用相同索引（如果存在）
                                config.NewCompareColumns.Add(Math.Min(i, newFileData.Columns.Count - 1));
                            }
                            config.CompareColumns.Add(i);
                        }
                    }
                }

                // 执行对比
                var engine = new Core.ComparisonEngine();
                var result = engine.Compare(txtOldFilePath.Text, txtNewFilePath.Text, config);

                // 导出结果到Excel文件
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "导出对比结果";
                    saveFileDialog.Filter = "Excel文件|*.xlsx|所有文件|*.*";
                    
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var exporter = new Core.ComparisonResultExporter();
                        exporter.ExportToExcel(result, saveFileDialog.FileName);
                        MessageBox.Show("对比结果已成功导出到Excel文件", "导出完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出过程中发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 清除结果按钮点击事件
        /// </summary>
        private void btnClearResults_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空键列和比较列列表
                lstKeyColumns.Items.Clear();
                lstCompareColumns.Items.Clear();
                
                // 清空数据预览
                dgvOldFilePreview.DataSource = null;
                dgvNewFilePreview.DataSource = null;
                
                // 重置文件数据
                oldFileData = null;
                newFileData = null;
                
                // 清空文件路径
                txtOldFilePath.Text = "";
                txtNewFilePath.Text = "";
                
                // 重置摘要信息
                lblOldFileSummary.Text = "行数: 0, 列数: 0";
                lblNewFileSummary.Text = "行数: 0, 列数: 0";
                
                MessageBox.Show("已清除所有结果和加载的数据", "清除完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清除过程中发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
             #endregion

        /// <summary>
        /// 显示结果摘要到主界面
        /// </summary>
        /// <param name="result">对比结果</param>
        private void DisplayResultSummary(Core.ComparisonResult result)
        {
            // 清空现有数据
            dgvComparisonResults.DataSource = null;
            
            // 创建详细对比数据表
            var comparisonTable = new DataTable();
            
            // 添加键列
            int keyColumnCount = result.AddedRecords.FirstOrDefault()?.KeyValues?.Length ?? 0;
            for (int i = 0; i < keyColumnCount; i++)
            {
                comparisonTable.Columns.Add($"键{i + 1}");
            }
            
            // 如果有比较列配置，添加新旧数据列
            if (lstCompareColumns.Items.Count > 0)
            {
                foreach (var item in lstCompareColumns.Items)
                {
                    var mapping = item.ToString();
                    var parts = mapping.Split(new string[] { " -> " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        comparisonTable.Columns.Add($"[旧]{parts[0]}");
                        comparisonTable.Columns.Add($"[新]{parts[1]}");
                    }
                }
            }
            
            // 添加新增记录（新有旧没有）
            foreach (var record in result.AddedRecords)
            {
                var row = comparisonTable.NewRow();
                
                // 填充键值
                for (int i = 0; i < record.KeyValues?.Length; i++)
                {
                    row[i] = record.KeyValues[i];
                }
                
                // 填充新数据值（旧数据列留空）
                int columnIndex = record.KeyValues?.Length ?? 0;
                if (record.Data != null)
                {
                    var dataValues = record.Data.Values.ToList();
                    for (int i = 0; i < dataValues.Count; i++)
                    {
                        // 旧数据列留空，新数据列填值
                        row[columnIndex + i * 2] = ""; // 旧数据列
                        row[columnIndex + i * 2 + 1] = dataValues[i]?.ToString() ?? ""; // 新数据列
                    }
                }
                
                comparisonTable.Rows.Add(row);
            }
            
            // 添加删除记录（旧有新没有）
            foreach (var record in result.DeletedRecords)
            {
                var row = comparisonTable.NewRow();
                
                // 填充键值
                for (int i = 0; i < record.KeyValues?.Length; i++)
                {
                    row[i] = record.KeyValues[i];
                }
                
                // 填充旧数据值（新数据列留空）
                int columnIndex = record.KeyValues?.Length ?? 0;
                if (record.Data != null)
                {
                    var dataValues = record.Data.Values.ToList();
                    for (int i = 0; i < dataValues.Count; i++)
                    {
                        // 旧数据列填值，新数据列留空
                        row[columnIndex + i * 2] = dataValues[i]?.ToString() ?? ""; // 旧数据列
                        row[columnIndex + i * 2 + 1] = ""; // 新数据列
                    }
                }
                
                comparisonTable.Rows.Add(row);
            }
            
            // 添加修改记录（两边都有但值不同）
            foreach (var record in result.ModifiedRecords)
            {
                var row = comparisonTable.NewRow();
                
                // 填充键值
                for (int i = 0; i < record.KeyValues?.Length; i++)
                {
                    row[i] = record.KeyValues[i];
                }
                
                // 填充差异值
                int columnIndex = record.KeyValues?.Length ?? 0;
                if (record.Differences != null)
                {
                    var differences = record.Differences.ToList();
                    for (int i = 0; i < differences.Count; i++)
                    {
                        row[columnIndex + i * 2] = differences[i].Value.OldValue ?? ""; // 旧数据列
                        row[columnIndex + i * 2 + 1] = differences[i].Value.NewValue ?? ""; // 新数据列
                    }
                }
                
                comparisonTable.Rows.Add(row);
            }
            
            // 绑定到DataGridView
            dgvComparisonResults.DataSource = comparisonTable;
            
            // 应用颜色编码
            ApplyComparisonColorCoding();
        }
        
        /// <summary>
        /// 应用对比结果颜色编码
        /// </summary>
        private void ApplyComparisonColorCoding()
        {
            // 为DataGridView应用颜色编码
            foreach (DataGridViewRow row in dgvComparisonResults.Rows)
            {
                // 跳过键列，从键列之后开始处理
                int keyColumnCount = 0;
                if (dgvComparisonResults.DataSource is DataTable dt)
                {
                    // 计算键列数量
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName.StartsWith("键"))
                        {
                            keyColumnCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                
                // 处理数据列（成对处理）
                for (int i = keyColumnCount; i < row.Cells.Count; i += 2)
                {
                    var oldCell = row.Cells[i];      // 旧数据列
                    var newCell = row.Cells[i + 1];  // 新数据列
                    
                    // 获取单元格值
                    string oldValue = oldCell.Value?.ToString() ?? "";
                    string newValue = newCell.Value?.ToString() ?? "";
                    
                    // 根据值的情况设置背景色
                    if (string.IsNullOrEmpty(oldValue) && !string.IsNullOrEmpty(newValue))
                    {
                        // 新有旧没有 - 绿色
                        newCell.Style.BackColor = Color.LightGreen;
                        oldCell.Style.BackColor = Color.LightGreen;
                    }
                    else if (!string.IsNullOrEmpty(oldValue) && string.IsNullOrEmpty(newValue))
                    {
                        // 旧有新没有 - 红色
                        oldCell.Style.BackColor = Color.LightCoral;
                        newCell.Style.BackColor = Color.LightCoral;
                    }
                    else if (!string.IsNullOrEmpty(oldValue) && !string.IsNullOrEmpty(newValue))
                    {
                        if (oldValue != newValue)
                        {
                            // 值不同 - 黄色
                            oldCell.Style.BackColor = Color.LightYellow;
                            newCell.Style.BackColor = Color.LightYellow;
                        }
                        else
                        {
                            // 值相同 - 白色（如果启用显示相同数据）或默认色
                            if (showSameData)
                            {
                                oldCell.Style.BackColor = Color.White;
                                newCell.Style.BackColor = Color.White;
                            }
                        }
                    }
                }
            }
        }
        
        #region Helper Methods
        /// <summary>
        /// 根据选择的对比模式获取枚举值
        /// </summary>
        /// <returns>对比模式枚举值</returns>
        private Core.ComparisonMode GetComparisonMode()
        {
            var selectedMode = cmbComparisonMode.SelectedItem?.ToString();
            switch (selectedMode)
            {
                case "存在性检查":
                    return Core.ComparisonMode.ExistenceCheck;
                case "数据差异":
                    return Core.ComparisonMode.DataDifference;
                case "自定义列对比":
                    return Core.ComparisonMode.CustomColumns;
                default:
                    return Core.ComparisonMode.DataDifference; // 默认模式
            }
        }
        /// <summary>
        /// 显示相同数据复选框状态改变事件
        /// </summary>
        private void chkShowSameData_CheckedChanged(object sender, EventArgs e)
        {
            showSameData = chkShowSameData.Checked;
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}