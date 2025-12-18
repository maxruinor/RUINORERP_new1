using log4net;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Business.CommService;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server.Controls
{
    public partial class LockDataViewerControl : UserControl
    {
        private ILockManagerService _lockManagerService;
        private System.Windows.Forms.Timer refreshTimer;

        public LockDataViewerControl()
        {
            InitializeComponent();
            _lockManagerService = Startup.GetFromFac<ILockManagerService>();
            InitializeTimer();
            InitializeData();
            // 移除重复的事件订阅，避免双击时显示两次详细信息
            // dataGridViewData.CellDoubleClick += dataGridViewData_CellDoubleClick;
        }
 

        private ContextMenuStrip contextMenuStrip;

        private void InitializeData()
        {
            try
            {
                // 初始化锁定信息列表
                LoadLockInfoList();

                // 初始化DataGridView
                dataGridViewData.AutoGenerateColumns = false;
                dataGridViewData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewData.AllowUserToAddRows = false;
                dataGridViewData.ReadOnly = true;
                dataGridViewData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewData.RowHeadersVisible = false;

                // 配置自定义列
                ConfigureDataGridViewColumns();

                // 添加数据绑定完成事件
                dataGridViewData.DataBindingComplete += dataGridViewData_DataBindingComplete;

                // 初始化右键菜单
                InitializeContextMenu();

                // 为DataGridView添加鼠标点击事件
                dataGridViewData.MouseClick += dataGridViewData_MouseClick;

                // 添加解锁过期单据按钮
                AddUnlockExpiredButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化锁定数据查看器时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加解锁过期单据按钮
        /// </summary>
        private void AddUnlockExpiredButton()
        {
            Button btnUnlockExpired = new Button
            {
                Name = "btnUnlockExpired",
                Text = "解锁过期单据",
                Size = new Size(120, 30),
                Margin = new Padding(5)
            };
            btnUnlockExpired.Click += BtnUnlockExpired_Click;

            // 添加到按钮面板
            if (panelButtons != null)
            {
                panelButtons.Controls.Add(btnUnlockExpired);
            }
        }

        /// <summary>
        /// 解锁所有过期单据
        /// </summary>
        private async void BtnUnlockExpired_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "确定要解锁所有过期的单据吗？\n此操作将自动解除所有已过期的锁定。",
                    "确认解锁过期单据",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                // 获取所有锁定单据并筛选过期项
                var lockInfos = _lockManagerService.GetAllLockedDocuments();
                int unlockedCount = 0;
                foreach (var lockInfo in lockInfos)
                {
                    // 检查是否锁定状态为false（已过期）
                    if (!lockInfo.IsLocked)
                    {
                        await _lockManagerService.ForceUnlockDocumentAsync(lockInfo.BillID);
                        {
                            unlockedCount++;
                        }
                    }
                }

                // 刷新数据
                if (listBoxTableList.SelectedItem != null)
                {
                    string selectedList = listBoxTableList.SelectedItem.ToString();
                    LoadTableData(selectedList);
                }

                MessageBox.Show($"成功解锁 {unlockedCount} 条过期单据", "操作结果",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解锁过期单据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化右键菜单
        /// </summary>
        private void InitializeContextMenu()
        {
            contextMenuStrip = new ContextMenuStrip();


            // 添加解锁菜单项
            ToolStripMenuItem menuUnlock = new ToolStripMenuItem("解锁单据");
            menuUnlock.Click += menuUnlock_Click;
            contextMenuStrip.Items.Add(menuUnlock);

            // 添加刷新菜单项
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem menuRefresh = new ToolStripMenuItem("刷新");
            menuRefresh.Click += menuRefresh_Click;
            contextMenuStrip.Items.Add(menuRefresh);

            // 设置右键菜单到DataGridView
            dataGridViewData.ContextMenuStrip = contextMenuStrip;
        }

        private void dataGridViewData_MouseClick(object sender, MouseEventArgs e)
        {
            // 当用户右键点击时，确保点击的行被选中
            if (e.Button == MouseButtons.Right)
            {
                // 获取点击的位置对应的行索引
                var hitTest = dataGridViewData.HitTest(e.X, e.Y);
                if (hitTest.RowIndex >= 0)
                {
                    // 选中点击的行
                    dataGridViewData.ClearSelection();
                    dataGridViewData.Rows[hitTest.RowIndex].Selected = true;
                }
            }
        }

        // 右键菜单事件处理程序
        private void menuViewDetails_Click(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 获取锁定时长文本
        /// </summary>
        private string GetLockDurationText(DateTime lockTime)
        {
            TimeSpan duration = DateTime.Now - lockTime;

            if (duration.TotalMinutes < 1)
                return $"{duration.Seconds}秒";
            else if (duration.TotalHours < 1)
                return $"{duration.Minutes}分{duration.Seconds}秒";
            else if (duration.TotalDays < 1)
                return $"{duration.Hours}小时{duration.Minutes}分钟";
            else
                return $"{duration.Days}天{duration.Hours}小时";
        }

        private void menuUnlock_Click(object sender, EventArgs e)
        {
            UnlockSelectedDocuments();
        }

        /// <summary>
        /// 解锁选中的单据
        /// </summary>
        private async void UnlockSelectedDocuments()
        {
            try
            {
                if (dataGridViewData.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要解锁的单据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 确认解锁操作
                DialogResult result = MessageBox.Show(
                    $"确定要解锁选中的 {dataGridViewData.SelectedRows.Count} 条单据吗？\n此操作将强制解除锁定状态，可能导致数据冲突。",
                    "确认解锁",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;


                int successCount = 0;
                int failCount = 0;

                foreach (DataGridViewRow row in dataGridViewData.SelectedRows)
                {
                    if (row.DataBoundItem is LockInfo lockInfo)
                    {
                        try
                        {
                            var unlockResult = await _lockManagerService.ForceUnlockDocumentAsync(lockInfo.BillID);
                            if (unlockResult.IsSuccess)
                                successCount++;
                            else
                                failCount++;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"解锁单据 {lockInfo.BillID} 失败: {ex.Message}");
                            failCount++;
                        }
                    }
                }

                // 刷新数据
                if (listBoxTableList.SelectedItem != null)
                {
                    string selectedList = listBoxTableList.SelectedItem.ToString();
                    LoadTableData(selectedList);
                }

                // 显示解锁结果
                string message = $"解锁完成：成功 {successCount} 条，失败 {failCount} 条";
                MessageBox.Show(message, "操作结果", MessageBoxButtons.OK,
                    failCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行解锁操作时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuRefresh_Click(object sender, EventArgs e)
        {
            // 调用刷新按钮的点击事件
            btnRefresh_Click(sender, e);
        }

        // 已删除重复的ShowSelectedRowDetails方法实现

        /// <summary>
        /// 配置DataGridView的自定义列
        /// </summary>
        private void ConfigureDataGridViewColumns()
        {
            try
            {
                dataGridViewData.Columns.Clear();

                // 设置列标题不换行
                dataGridViewData.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
                // 设置列宽自动调整
                dataGridViewData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // 添加最重要的锁定信息列

                // 单据编号
                dataGridViewData.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "BillNo",
                    HeaderText = "单据编号",
                    Name = "colBillNo",
                    FillWeight = 15
                });

                // 用户信息
                dataGridViewData.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "LockedUserName",
                    HeaderText = "锁定用户名",
                    Name = "colLockedUserName",
                    FillWeight = 12
                });

                // 时间信息
                dataGridViewData.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "LockTime",
                    HeaderText = "锁定时间",
                    Name = "colLockTime",
                    FillWeight = 15,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" }
                });

                dataGridViewData.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ExpireTime",
                    HeaderText = "过期时间",
                    Name = "colExpireTime",
                    FillWeight = 15,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" }
                });

                // 状态信息
                dataGridViewData.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "IsLocked",
                    HeaderText = "是否锁定",
                    Name = "colIsLocked",
                    FillWeight = 8
                });

                dataGridViewData.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "IsExpired",
                    HeaderText = "是否过期",
                    Name = "colIsExpired",
                    FillWeight = 8
                });

                // 业务信息
                dataGridViewData.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "bizType",
                    HeaderText = "业务类型",
                    Name = "colBizType",
                    FillWeight = 10
                });

                // 会话信息
                dataGridViewData.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "SessionId",
                    HeaderText = "会话ID",
                    Name = "colSessionId",
                    FillWeight = 15
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"配置DataGridView列时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 数据绑定完成事件，用于设置行样式
        /// </summary>
        private void dataGridViewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridViewData.Rows)
                {
                    if (row.DataBoundItem is LockInfo lockInfo)
                    {
                        // 为已释放的锁定项设置不同的背景色
                        if (!lockInfo.IsLocked)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightPink;
                        }
                        // 为锁定但长时间未更新的项设置警示颜色（例如30分钟内）
                        else if (DateTime.Now - lockInfo.LastHeartbeat > TimeSpan.FromMinutes(30))
                        {
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"设置行样式时出错: {ex.Message}");
            }
        }

        private void InitializeTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 20000; // 20秒刷新一次
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }

        private void DataViewerControl_Load(object sender, EventArgs e)
        {
            // 设置控件属性
        }

        /// <summary>
        /// 加载锁定信息列表
        /// </summary>
        private void LoadLockInfoList()
        {
            try
            {
                // 清空列表
                listBoxTableList.Items.Clear();

                // 添加锁定信息列表
                listBoxTableList.Items.Add("锁定信息列表");

                // 默认选中第一个项
                if (listBoxTableList.Items.Count > 0)
                {
                    listBoxTableList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载锁定信息列表时出错: {ex.Message}");
                MessageBox.Show($"加载锁定信息列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// 加载锁定单据数据
        /// </summary>
        /// <param name="listName">列表名称</param>
        private void LoadTableData(string listName)
        {
            try
            {
                // 清空当前数据
                dataGridViewData.DataSource = null;

                // 确保列配置正确
                ConfigureDataGridViewColumns();

                if (string.IsNullOrEmpty(listName))
                {
                    System.Diagnostics.Debug.WriteLine("列表名称为空，无法加载数据");
                    return;
                }

                if (listName == "锁定信息列表")
                {
                    // 检查服务提供者
                    if (Program.ServiceProvider == null)
                    {
                        MessageBox.Show("服务提供者未初始化，请检查应用程序配置", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {



                        // 获取锁定信息
                        var lockInfos = _lockManagerService.GetAllLockedDocuments();

                        // 处理锁定信息列表
                        if (lockInfos != null && lockInfos.Any())
                        {
                            // 使用BindingList进行更好的数据绑定
                            dataGridViewData.DataSource = new BindingList<LockInfo>(lockInfos);
                            // 记录状态信息到控制台
                            System.Diagnostics.Debug.WriteLine($"共 {lockInfos.Count} 条锁定记录");

                            // 自动调整列宽以适应内容
                            AutoAdjustColumnWidths();
                        }
                        else
                        {
                            // 清空列并添加空消息列
                            dataGridViewData.Columns.Clear();
                            dataGridViewData.Columns.Add("EmptyMessage", "提示");
                            dataGridViewData.Rows.Add("暂无锁定数据");
                            dataGridViewData.ReadOnly = true;
                            System.Diagnostics.Debug.WriteLine("当前没有锁定记录");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"获取锁定信息时出错: {ex.Message}");
                        MessageBox.Show($"无法获取锁定信息数据: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载表数据时出错: {ex.Message}");
                // 确保数据网格视图已清空
                dataGridViewData.DataSource = null;
                dataGridViewData.Columns.Clear();
                MessageBox.Show($"加载锁定数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 自动调整列宽以适应内容
        /// </summary>
        private void AutoAdjustColumnWidths()
        {
            try
            {
                // 对所有列应用自动调整宽度
                foreach (DataGridViewColumn column in dataGridViewData.Columns)
                {
                    // 对于布尔类型列，使用默认宽度
                    if (column is DataGridViewCheckBoxColumn)
                    {
                        column.Width = 70; // 设置一个固定宽度给复选框列
                    }
                    else
                    {
                        // 自动调整宽度以适应标题
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        // 然后根据内容调整，但不超过最大宽度
                        int contentWidth = column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true);
                        column.Width = Math.Min(contentWidth, 200); // 设置最大宽度为200
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"自动调整列宽时出错: {ex.Message}");
            }
        }

        #endregion

        #region 按钮事件处理

        private async void btnRefresh_Click(object sender, EventArgs e)
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
                MessageBox.Show($"刷新数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 清理孤儿锁按钮点击事件处理程序
        /// </summary>
        private async void btnCleanupOrphanedLocks_Click(object sender, EventArgs e)
        {
            try
            {
                // 显示确认对话框
                DialogResult result = MessageBox.Show(
                    "确定要清理所有孤儿锁吗？\n此操作将释放所有因客户端异常断开而未正确释放的锁定。",
                    "确认清理孤儿锁",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                // 获取孤儿锁检测服务
                var orphanedLockDetector = Program.ServiceProvider.GetService<OrphanedLockDetector>();
                if (orphanedLockDetector == null)
                {
                    MessageBox.Show("无法获取孤儿锁检测服务", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 获取所有锁定的文档信息
                var lockInfos = _lockManagerService.GetAllLockedDocuments();
                
                // 执行孤儿锁检测和清理
                await orphanedLockDetector.DetectAndCleanupAsync(lockInfos);

                // 刷新数据显示
                if (listBoxTableList.SelectedItem != null)
                {
                    string selectedTable = listBoxTableList.SelectedItem.ToString();
                    LoadTableData(selectedTable);
                }

                MessageBox.Show("孤儿锁清理完成", "操作结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清理孤儿锁时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                System.Diagnostics.Debug.WriteLine($"更新锁定统计信息显示时出错: {ex.Message}");
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // 自动刷新数据
                if (listBoxTableList.SelectedItem != null)
                {
                    string selectedTable = listBoxTableList.SelectedItem.ToString();
                    LoadTableData(selectedTable);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"自动刷新数据时出错: {ex.Message}");
                // 使用BeginInvoke确保在UI线程上记录错误
                BeginInvoke((Action)(() =>
                {
                    System.Diagnostics.Debug.WriteLine($"UI线程记录: 自动刷新锁定数据异常: {ex.Message}");
                }));
            }
        }

        private void UpdateLockStatsDisplay()
        {
            try
            {
                var stats = _lockManagerService.GetLockStatistics();
                if (stats != null)
                {
                    string statsText = $"锁定统计 - 总数: {stats.TotalLocks}, 活跃: {stats.ActiveLocks}, 等待: {stats.RequestingUnlock}, 峰值: {stats.MonitorData?.PeakConcurrentLocks ?? 0}";

                    // 如果存在历史记录数，也显示出来
                    if (stats is LockInfoStatistics enhancedStats)
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"更新锁定统计信息显示时出错: {ex.Message}");
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



        #region DataGridView事件处理

        private void dataGridViewData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewData.Rows.Count)
            {
                var row = dataGridViewData.Rows[e.RowIndex];
                var lockInfo = row.DataBoundItem as LockInfo;

                if (lockInfo != null)
                {
                    // 显示基本锁定信息，避免使用跨框架引用
                    string details = GetObjectDetails(lockInfo);
                    MessageBox.Show(details, "锁定信息详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
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