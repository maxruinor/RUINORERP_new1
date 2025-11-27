using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Server.Comm;
using RUINORERP.Business.CommService;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Network.Interfaces.Services;

namespace RUINORERP.Server.Controls
{
    public partial class LockDataViewerControl : UserControl
    {
        private System.Windows.Forms.Timer refreshTimer;

        public LockDataViewerControl()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeData()
        {
            try
            {
                // 初始化缓存表列表
                LoadCacheTableList();
                
                // 初始化DataGridView
                dataGridViewData.AutoGenerateColumns = false;
                dataGridViewData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化数据查看器时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 30000; // 30秒刷新一次
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }

        private void DataViewerControl_Load(object sender, EventArgs e)
        {
            // 设置控件属性
        }

        /// <summary>
        /// 加载缓存表列表
        /// </summary>
        private void LoadCacheTableList()
        {
            try
            {
                listBoxTableList.Items.Clear();
                
                // 添加锁定信息列表
                listBoxTableList.Items.Add("锁定信息列表");
                
                // 添加缓存表列表 - 暂时注释掉，等待缓存管理器修复
                // if (MyCacheManager.Instance != null && MyCacheManager.Instance.NewTableList != null)
                // {
                //     foreach (KeyValuePair<string, KeyValuePair<string, string>> table in MyCacheManager.Instance.NewTableList)
                //     {
                //         listBoxTableList.Items.Add(table.Value.Value);
                //     }
                // }
                
                // 如果有项目，选择第一个
                if (listBoxTableList.Items.Count > 0)
                {
                    listBoxTableList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载缓存表列表时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 列表框事件处理

        private void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTableList.SelectedItem != null)
                {
                    string selectedTable = listBoxTableList.SelectedItem.ToString();
                    LoadTableData(selectedTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 数据加载

        /// <summary>
        /// 加载指定表的数据
        /// </summary>
        /// <param name="tableName">表名</param>
        private void LoadTableData(string tableName)
        {
            try
            {
                // 清空当前数据
                dataGridViewData.DataSource = null;
                
                if (tableName == "锁定信息列表")
                {
                    // 显示锁定信息
                    var lockManager = Program.ServiceProvider.GetRequiredService<ILockManagerService>();
                    var lockItems = lockManager.GetAllLockedDocuments();
                    dataGridViewData.DataSource = lockItems;
                    return;
                }
                
                
                
                // 获取缓存数据 - 修复类型推断问题
                var cacheData = RUINORERP.Business.Cache.EntityCacheHelper.GetEntityList<object>(tableName);
                if (cacheData != null && cacheData.Count > 0)
                {
                    // 将数据绑定到DataGridView
                    var bindingList = new BindingList<object>(cacheData);
                    dataGridViewData.DataSource = bindingList;
                }
                else
                {
                    MessageBox.Show("未找到缓存数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 按钮事件处理

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTableList.SelectedItem != null)
                {
                    string selectedTable = listBoxTableList.SelectedItem.ToString();
                    LoadTableData(selectedTable);
                    
                    MessageBox.Show("数据已刷新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV文件|*.csv|文本文件|*.txt|所有文件|*.*";
                saveFileDialog.Title = "导出数据到文件";
                saveFileDialog.FileName = "data_export.csv";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportDataToFile(saveFileDialog.FileName);
                    MessageBox.Show($"数据已导出到 {saveFileDialog.FileName}", "导出完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnReloadCache_Click(object sender, EventArgs e)
        {
            try
            {
                // 重新加载缓存
                await frmMainNew.Instance.InitConfig(true);
                
                // 刷新表列表
                LoadCacheTableList();
                
                MessageBox.Show("缓存已重新加载", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重新加载缓存时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLockStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                ShowLockStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示锁定统计信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StatsUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateLockStatsDisplay();
            }
            catch (Exception ex)
            {
                // 静默处理统计更新错误，避免频繁弹窗
                Console.WriteLine($"更新锁定统计信息显示时出错: {ex.Message}");
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTableList.SelectedItem != null)
                {
                    string selectedTable = listBoxTableList.SelectedItem.ToString();
                    LoadTableData(selectedTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"自动刷新数据时出错: {ex.Message}");
            }
        }

        private void UpdateLockStatsDisplay()
        {
            try
            {
                var lockManager = Program.ServiceProvider.GetRequiredService<ILockManagerService>();
            if (lockManager != null)
            {
                var stats = lockManager.GetLockStatistics();
                    if (stats != null)
                    {
                        string statsText = $"锁定统计 - 总数: {stats.TotalLocks}, 活跃: {stats.ActiveLocks}, 等待: {stats.WaitingLocks}, 峰值: {stats.MonitorData?.PeakConcurrentLocks ?? 0}";
                        
                        // 如果存在历史记录数，也显示出来
                        if (stats is RUINORERP.Server.Network.Services.LockInfoStatistics enhancedStats)
                        {
                            statsText += $", 历史: {enhancedStats.HistoryRecordCount}";
                        }
                        
                        if (lblLockStats.InvokeRequired)
                        {
                            lblLockStats.Invoke(new Action(() => lblLockStats.Text = statsText));
                        }
                        else
                        {
                            lblLockStats.Text = statsText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新锁定统计信息显示时出错: {ex.Message}");
            }
        }

        private void ShowLockStatistics()
        {
            try
            {
                var lockManager = Program.ServiceProvider.GetRequiredService<ILockManagerService>();
            if (lockManager != null)
            {
                var stats = lockManager.GetLockStatistics();
                    if (stats != null)
                    {
                        var message = new StringBuilder();
                        message.AppendLine("=== 锁定统计信息 ===");
                        message.AppendLine($"总锁定数: {stats.TotalLocks}");
                        message.AppendLine($"活跃锁定数: {stats.ActiveLocks}");
                        message.AppendLine($"等待锁定数: {stats.RequestingUnlock}");
                        message.AppendLine($"峰值并发数: {stats.MonitorData?.PeakConcurrentLocks ?? 0}");
                        message.AppendLine($"过期锁定数: {stats.ExpiredLocks}");
                        message.AppendLine($"用户锁定数: {stats.LocksByUser}");
                        message.AppendLine($"历史记录数: {stats.HistoryRecordCount}");
                        
                        // 添加监控数据详细信息
                        if (stats.MonitorData != null)
                        {
                            message.AppendLine();
                            message.AppendLine("=== 监控数据 ===");
                            message.AppendLine($"总添加数: {stats.MonitorData.TotalLocksAdded}");
                            message.AppendLine($"总移除数: {stats.MonitorData.TotalLocksRemoved}");
                            message.AppendLine($"总过期数: {stats.MonitorData.TotalLocksExpired}");
                            message.AppendLine($"当前并发数: {stats.MonitorData.CurrentConcurrentLocks}");
                            message.AppendLine($"最后重置时间: {stats.MonitorData.LastResetTime:yyyy-MM-dd HH:mm:ss}");
                        }
                        
                        // 添加业务类型统计
                        if (stats.LocksByBizType != null && stats.LocksByBizType.Count > 0)
                        {
                            message.AppendLine();
                            message.AppendLine("=== 业务类型统计 ===");
                            foreach (var bizType in stats.LocksByBizType)
                            {
                                message.AppendLine($"{bizType.Key}: {bizType.Value}");
                            }
                        }
                        
                        // 添加状态统计
                        if (stats.LocksByStatus != null && stats.LocksByStatus.Count > 0)
                        {
                            message.AppendLine();
                            message.AppendLine("=== 状态统计 ===");
                            foreach (var status in stats.LocksByStatus)
                            {
                                message.AppendLine($"{status.Key}: {status.Value}");
                            }
                        }
                        
                        // 添加时间戳
                        message.AppendLine();
                        message.AppendLine($"统计时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                        
                        MessageBox.Show(message.ToString(), "锁定统计信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("无法获取锁定统计信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("锁定管理器未初始化", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示锁定统计信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 数据导出

        /// <summary>
        /// 导出数据到文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        private void ExportDataToFile(string fileName)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();
                
                // 添加列标题
                if (dataGridViewData.Columns.Count > 0)
                {
                    List<string> headers = new List<string>();
                    foreach (DataGridViewColumn column in dataGridViewData.Columns)
                    {
                        headers.Add(column.HeaderText);
                    }
                    csvContent.AppendLine(string.Join(",", headers));
                }
                
                // 添加数据行
                foreach (DataGridViewRow row in dataGridViewData.Rows)
                {
                    if (row.IsNewRow) continue;
                    
                    List<string> values = new List<string>();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        values.Add(cell.Value?.ToString() ?? "");
                    }
                    csvContent.AppendLine(string.Join(",", values));
                }
                
                // 写入文件
                System.IO.File.WriteAllText(fileName, csvContent.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"导出数据到文件时出错: {ex.Message}");
            }
        }

        #endregion

        #region DataGridView事件处理

        private void dataGridViewData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewData.Rows.Count)
            {
                // 显示详细信息
                var rowData = dataGridViewData.Rows[e.RowIndex].DataBoundItem;
                if (rowData != null)
                {
                    string details = GetObjectDetails(rowData);
                    MessageBox.Show(details, "详细信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 获取对象的详细信息
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>详细信息字符串</returns>
        private string GetObjectDetails(object obj)
        {
            try
            {
                StringBuilder details = new StringBuilder();
                Type type = obj.GetType();
                
                details.AppendLine($"类型: {type.Name}");
                details.AppendLine();
                
                // 获取所有公共属性
                var properties = type.GetProperties();
                foreach (var prop in properties)
                {
                    try
                    {
                        var value = prop.GetValue(obj);
                        details.AppendLine($"{prop.Name}: {value ?? "null"}");
                    }
                    catch (Exception ex)
                    {
                        details.AppendLine($"{prop.Name}: [获取值时出错: {ex.Message}]");
                    }
                }
                
                return details.ToString();
            }
            catch (Exception ex)
            {
                return $"获取对象详细信息时出错: {ex.Message}";
            }
        }

        #endregion
    }
}