using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Business;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.FileManagement;
using RUINORERP.Server.Network.CommandHandlers;
using RUINORERP.Server.Helpers;
using RUINORERP.Server.Network.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server.Controls
{
    /// <summary>
    /// 文件管理控件 - 基于真实数据库表结构实现
    /// 使用 tb_FS_FileStorageInfo 和 tb_FS_BusinessRelation 表进行数据管理
    /// </summary>
    public partial class FileManagementControl : UserControl
    {
        private readonly Timer _updateTimer;
        private FileStorageSummary _currentStorageSummary;
        private FileStorageMonitorInfo _currentMonitorInfo;
        private List<FileCategoryInfo> _fileCategories;
        private bool _isDisposed = false;

        // 真实的业务控制器
        private readonly tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> _fileStorageInfoController;
        private readonly tb_FS_BusinessRelationController<tb_FS_BusinessRelation> _businessRelationController;

        // 文件监控服务
        private readonly FileStorageMonitorService _monitorService;

        public FileManagementControl()
        {
            InitializeComponent();

            // 从依赖注入容器获取业务控制器和监控服务
            _fileStorageInfoController = Program.ServiceProvider.GetService<tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo>>();
            _businessRelationController = Program.ServiceProvider.GetService<tb_FS_BusinessRelationController<tb_FS_BusinessRelation>>();
            _monitorService = Program.ServiceProvider.GetService<FileStorageMonitorService>();

            // 设置定时器，定期更新存储信息
            _updateTimer = new Timer { Interval = 60000 }; // 每分钟更新一次
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();

            // 初始化列表视图
            InitializeListView();
        }

        /// <summary>
        /// 释放资源，防止内存泄漏
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                // 停止并释放定时器
                if (_updateTimer != null)
                {
                    _updateTimer.Stop();
                    _updateTimer.Tick -= UpdateTimer_Tick;
                    _updateTimer.Dispose();
                }

                // 释放ListView资源
                if (listView1 != null)
                {
                    listView1.Items.Clear();
                    listView1.Dispose();
                }

                _isDisposed = true;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 文件存储汇总信息
        /// </summary>
        public class FileStorageSummary
        {
            public long TotalFileCount { get; set; }
            public long TotalStorageSize { get; set; }
            public long TotalDiskSpace { get; set; }
            public List<FileCategoryInfo> Categories { get; set; }
        }

        /// <summary>
        /// 文件分类信息
        /// </summary>
        public class FileCategoryInfo
        {
            public string CategoryName { get; set; }
            public int BusinessType { get; set; }
            public long FileCount { get; set; }
            public long StorageSize { get; set; }
            public DateTime LastModified { get; set; }
        }

        private void InitializeListView()
        {
            // 设置列表视图属性
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.AllowColumnReorder = true;

            // 添加基于真实数据库表的列
            listView1.Columns.Add("业务类型", 100);
            listView1.Columns.Add("文件分类", 120);
            listView1.Columns.Add("文件数量", 80);
            listView1.Columns.Add("存储大小", 100);
            listView1.Columns.Add("最后修改时间", 150);
            listView1.Columns.Add("状态", 80);
        }

        private async void UpdateTimer_Tick(object sender, EventArgs e)
        {
            await LoadStorageInfo();
        }

        public async Task LoadStorageInfo()
        {
            try
            {
                // 使用真实数据库服务获取存储信息
                await LoadRealStorageInfoAsync();

                // 安全更新UI（确保在UI线程上执行）
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        UpdateStorageSummary();
                        UpdateCategoryList();
                        UpdateStatusLabels(true, "数据加载成功");
                    }));
                }
                else
                {
                    UpdateStorageSummary();
                    UpdateCategoryList();
                    UpdateStatusLabels(true, "数据加载成功");
                }
            }
            catch (Exception ex)
            {
                // 安全更新错误状态
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateStatusLabels(false, "更新失败: " + ex.Message)));
                }
                else
                {
                    UpdateStatusLabels(false, "更新失败: " + ex.Message);
                }
                System.Diagnostics.Debug.WriteLine("加载存储信息失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 安全更新状态标签
        /// </summary>
        private void UpdateStatusLabels(bool success, string message)
        {
            lblLastUpdateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            picStatusIndicator.BackColor = success ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            lblStatus.Text = message;
        }

        /// <summary>
        /// 从真实数据库加载存储信息（性能优化版本，集成监控服务）
        /// </summary>
        private async Task LoadRealStorageInfoAsync()
        {
            _currentStorageSummary = new FileStorageSummary();
            _fileCategories = new List<FileCategoryInfo>();

            try
            {
                // 优先直接查询数据库,避免DbContext冲突
                var fileInfos = await _fileStorageInfoController.QueryAsync(c => c.FileStatus == (int)FileStatus.Active && c.isdeleted == false);

                // 先完成主查询结果处理,避免并发查询导致DbContext冲突
                if (fileInfos != null && fileInfos.Count > 0)
                {
                    // 安全检查：如果数据量过大，限制处理数量
                    const int MAX_FILES_TO_PROCESS = 10000;
                    var filesToProcess = fileInfos.Take(MAX_FILES_TO_PROCESS).ToList();

                    _currentStorageSummary.TotalFileCount = fileInfos.Count;
                    _currentStorageSummary.TotalStorageSize = filesToProcess.Sum(f => ((tb_FS_FileStorageInfo)f).FileSize > 0 ? ((tb_FS_FileStorageInfo)f).FileSize : 0);

                    // 性能优化：使用更高效的分组方式
                    var groupedByBusinessType = filesToProcess
                        .Cast<tb_FS_FileStorageInfo>()
                        .GroupBy(f => f.BusinessType ?? 0)
                        .ToList();

                    _fileCategories = groupedByBusinessType.Select(group => new FileCategoryInfo
                    {
                        BusinessType = group.Key,
                        CategoryName = GetBusinessTypeName(group.Key),
                        FileCount = group.Count(),
                        StorageSize = group.Sum(f => f.FileSize > 0 ? f.FileSize : 0),
                        LastModified = group.Max(f => f.Modified_at ?? f.Created_at ?? DateTime.MinValue)
                    }).ToList();

                    // 计算磁盘总空间 (使用系统API)
                    try
                    {
                        var driveInfo = new System.IO.DriveInfo(System.IO.Path.GetPathRoot(Environment.CurrentDirectory));
                        _currentStorageSummary.TotalDiskSpace = driveInfo.TotalSize;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("获取磁盘空间失败: " + ex.Message);
                        _currentStorageSummary.TotalDiskSpace = 100L * 1024L * 1024L * 1024L; // 默认100GB
                    }
                }
                else
                {
                    // 没有数据时的默认值
                    _currentStorageSummary.TotalFileCount = 0;
                    _currentStorageSummary.TotalStorageSize = 0;
                    _currentStorageSummary.TotalDiskSpace = 100L * 1024L * 1024L * 1024L; // 默认100GB
                }

                // 主查询和结果处理完成后,再获取监控信息
                if (_monitorService != null && fileInfos != null && fileInfos.Count > 0)
                {
                    try
                    {
                        _currentMonitorInfo = await _monitorService.GetMonitorInfoAsync();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("获取监控信息失败: " + ex.Message);
                        // 监控服务失败不影响主流程
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("加载真实存储信息失败: " + ex.Message);
                throw;
            }
        }



        /// <summary>
        /// 根据业务类型代码获取业务类型名称
        /// </summary>
        private string GetBusinessTypeName(int businessType)
        {
            return businessType switch
            {
                1 => "产品图片",
                2 => "报销凭证",
                3 => "付款凭证",
                4 => "合同文件",
                5 => "技术文档",
                6 => "客户资料",
                7 => "财务报表",
                8 => "人事档案",
                9 => "采购文件",
                10 => "销售资料",
                _ => "其他文件"
            };
        }

        private void UpdateStorageSummary()
        {
            if (_currentStorageSummary == null)
                return;

            // 更新存储概览 - 使用真实数据
            long totalSize = _currentStorageSummary.TotalDiskSpace;
            long usedSize = _currentStorageSummary.TotalStorageSize;
            long freeSize = totalSize - usedSize;

            lblTotalStorage.Text = FormatBytes(totalSize);
            lblUsedStorage.Text = FormatBytes(usedSize);
            lblFreeStorage.Text = FormatBytes(freeSize);

            // 计算使用百分比 - 优先使用监控服务的更精确数据
            int usagePercentage = 0;
            if (_currentMonitorInfo != null)
            {
                usagePercentage = (int)_currentMonitorInfo.FileStorageUsagePercentage;
            }
            else
            {
                usagePercentage = totalSize > 0 ? (int)((double)usedSize / totalSize * 100) : 0;
            }

            progressBar1.Value = usagePercentage;
            lblUsagePercentage.Text = usagePercentage + "%";

            // 根据使用情况设置进度条颜色和状态
            if (_currentMonitorInfo != null)
            {
                if (_currentMonitorInfo.IsCritical)
                {
                    progressBar1.ForeColor = System.Drawing.Color.Red;
                    UpdateStatusLabels(true, $"紧急警告: 磁盘使用率 {usagePercentage}%");
                }
                else if (_currentMonitorInfo.IsWarning)
                {
                    progressBar1.ForeColor = System.Drawing.Color.Orange;
                    UpdateStatusLabels(true, $"警告: 磁盘使用率 {usagePercentage}%");
                }
                else
                {
                    progressBar1.ForeColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                if (usagePercentage > 80)
                    progressBar1.ForeColor = System.Drawing.Color.Red;
                else if (usagePercentage > 60)
                    progressBar1.ForeColor = System.Drawing.Color.Orange;
                else
                    progressBar1.ForeColor = System.Drawing.Color.Green;
            }

            // 更新文件统计
            lblTotalFiles.Text = _currentStorageSummary.TotalFileCount.ToString();

            // 如果有监控信息，显示额外信息
            if (_currentMonitorInfo != null)
            {
                // 可以在状态栏或标签显示过期文件数和孤立文件数
                var extraInfo = $"正常: {_currentMonitorInfo.ActiveFiles}";
                if (_currentMonitorInfo.ExpiredFiles > 0)
                    extraInfo += $" | 过期: {_currentMonitorInfo.ExpiredFiles}";
                if (_currentMonitorInfo.OrphanedFiles > 0)
                    extraInfo += $" | 孤立: {_currentMonitorInfo.OrphanedFiles}";

                // 更新状态标签文本，如果之前没有设置为警告/紧急状态
                if (!_currentMonitorInfo.IsWarning && !_currentMonitorInfo.IsCritical)
                {
                    UpdateStatusLabels(true, $"数据加载成功 - {extraInfo}");
                }
            }
        }

        private void UpdateCategoryList()
        {
            listView1.Items.Clear();

            if (_fileCategories == null || _fileCategories.Count == 0)
                return;

            foreach (var category in _fileCategories)
            {
                ListViewItem item = new ListViewItem(category.BusinessType.ToString());
                item.SubItems.Add(category.CategoryName);
                item.SubItems.Add(category.FileCount.ToString());
                item.SubItems.Add(FormatBytes(category.StorageSize));
                item.SubItems.Add(category.LastModified.ToString("yyyy-MM-dd HH:mm:ss"));

                // 根据文件数量设置状态
                string status = category.FileCount > 0 ? "正常" : "无文件";
                item.SubItems.Add(status);

                // 根据文件数量设置行颜色
                if (category.FileCount > 100)
                    item.BackColor = System.Drawing.Color.LightGreen;
                else if (category.FileCount > 50)
                    item.BackColor = System.Drawing.Color.LightYellow;
                else if (category.FileCount > 0)
                    item.BackColor = System.Drawing.Color.LightBlue;
                else
                    item.BackColor = System.Drawing.Color.LightGray;

                listView1.Items.Add(item);
            }
        }

        private string FormatBytes(long bytes)
        {
            if (bytes < 1024)
                return $"{bytes} B";
            else if (bytes < 1024 * 1024)
                return $"{bytes / 1024.0:F2} KB";
            else if (bytes < 1024 * 1024 * 1024)
                return $"{bytes / (1024.0 * 1024.0):F2} MB";
            else
                return $"{bytes / (1024.0 * 1024.0 * 1024.0):F2} GB";
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadStorageInfo();
        }

        private async void btnCleanupFiles_Click(object sender, EventArgs e)
        {
            await PerformManualCleanupAsync();
        }

        private void btnMonitorDetails_Click(object sender, EventArgs e)
        {
            ShowMonitorDetails();
        }

        /// <summary>
        /// 查看已删除文件
        /// </summary>
        private async void btnViewDeletedFiles_Click(object sender, EventArgs e)
        {
            await ShowDeletedFilesAsync();
        }

        private async void btnViewDetails_Click(object sender, EventArgs e)
        {
            // 打开详细视图功能
            if (listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];
                int businessType = int.Parse(selectedItem.Text);
                string categoryName = selectedItem.SubItems[1].Text;

                await ShowCategoryDetailsAsync(businessType);
            }
            else
            {
                MessageBox.Show("请先选择一个文件分类");
            }
        }

        /// <summary>
        /// 显示特定业务类型的文件详情
        /// </summary>
        private async Task ShowCategoryDetailsAsync(int businessType)
        {
            try
            {
                // 查询该业务类型下的所有文件及其业务关联信息
                var fileInfos = await _fileStorageInfoController.QueryAsync(c => c.FileStatus == (int)FileStatus.Active && c.BusinessType == businessType && c.isdeleted == false);

                if (fileInfos != null && fileInfos.Count > 0)
                {
                    var detailForm = new Form
                    {
                        Text = $"文件分类详情-{(BizType)businessType}",
                        Size = new System.Drawing.Size(1100, 700),
                        StartPosition = FormStartPosition.CenterParent
                    };

                    var listView = new ListView
                    {
                        Dock = DockStyle.Fill,
                        View = View.Details,
                        FullRowSelect = true,
                        GridLines = true
                    };

                    // 添加列 - 包含业务关联表的字段
                    listView.Columns.Add("文件ID", 80);
                    listView.Columns.Add("原始文件名", 180);
                    listView.Columns.Add("文件大小", 100);
                    listView.Columns.Add("文件类型", 80);
                    listView.Columns.Add("业务编号", 120);
                    listView.Columns.Add("业务主键ID", 100);
                    listView.Columns.Add("关联字段", 100);
                    listView.Columns.Add("是否明细表", 80);
                    listView.Columns.Add("明细主键ID", 100);
                    listView.Columns.Add("关联版本号", 80);
                    listView.Columns.Add("主文件", 60);
                    listView.Columns.Add("活跃", 60);
                    listView.Columns.Add("创建时间", 140);

                    // 获取所有文件ID列表
                    var fileIds = fileInfos.Cast<tb_FS_FileStorageInfo>().Select(f => f.FileId).ToList();

                    // 通过FileId查询业务关联信息
                    var businessRelations = await _businessRelationController.QueryAsync(br => fileIds.Contains( br.FileId) && !br.isdeleted);

                    // 创建FileId到业务关联的映射字典
                    var relationDict = new System.Collections.Generic.Dictionary<long, tb_FS_BusinessRelation>();
                    if (businessRelations != null)
                    {
                        foreach (var relation in businessRelations.Cast<tb_FS_BusinessRelation>())
                        {
                            if (!relationDict.ContainsKey(relation.FileId))
                            {
                                relationDict[relation.FileId] = relation;
                            }
                        }
                    }

                    // 添加文件数据
                    foreach (var fileInfo in fileInfos.Cast<tb_FS_FileStorageInfo>())
                    {
                        var item = new ListViewItem(fileInfo.FileId.ToString());
                        item.SubItems.Add(fileInfo.OriginalFileName);
                        item.SubItems.Add(FormatBytes(fileInfo.FileSize > 0 ? fileInfo.FileSize : 0));
                        item.SubItems.Add(fileInfo.FileType);

                        // 通过FileId从字典中获取业务关联信息
                        if (relationDict.TryGetValue(fileInfo.FileId, out var businessRelation))
                        {
                            item.SubItems.Add(businessRelation.BusinessNo ?? "");
                            item.SubItems.Add(businessRelation.BusinessId.ToString());
                            item.SubItems.Add(businessRelation.RelatedField ?? "");
                            item.SubItems.Add(businessRelation.IsDetailTable ? "是" : "否");
                            item.SubItems.Add(businessRelation.DetailId?.ToString() ?? "");
                            item.SubItems.Add(businessRelation.VersionNo.ToString());
                            item.SubItems.Add(businessRelation.IsMainFile ? "是" : "否");
                            item.SubItems.Add(businessRelation.IsActive ? "是" : "否");
                        }
                        else
                        {
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                        }

                        item.SubItems.Add(fileInfo.Created_at?.ToString("yyyy-MM-dd HH:mm") ?? "未知");

                        listView.Items.Add(item);
                    }

                    detailForm.Controls.Add(listView);
                    detailForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show($"分类 '{(BizType)businessType}' 中没有找到任何文件");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载文件详情失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void FileManagementControl_Load(object sender, EventArgs e)
        {
            await LoadStorageInfo();
        }

        /// <summary>
        /// 手动触发存储空间清理
        /// </summary>
        private async Task PerformManualCleanupAsync()
        {
            if (_monitorService == null)
            {
                MessageBox.Show("监控服务未启动,无法执行清理操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                "确定要执行文件清理吗?\n\n" +
                "清理内容包括:\n" +
                "- 过期文件(超过7天未访问)\n" +
                "- 孤立文件(无业务关联)\n\n" +
                "建议先备份数据!",
                "确认清理",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult != DialogResult.Yes)
                return;

            try
            {
                // 显示等待光标
                this.Cursor = Cursors.WaitCursor;
                btnRefresh.Enabled = false;

                // 获取清理服务
                var cleanupService = Program.ServiceProvider.GetService<FileCleanupService>();
                if (cleanupService == null)
                {
                    MessageBox.Show("清理服务未找到", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 清理过期文件
                var expiredCount = await cleanupService.CleanupExpiredFilesAsync(
                    daysThreshold: 7,
                    physicalDelete: true
                );

                // 清理孤立文件
                var orphanedCount = await cleanupService.CleanupOrphanedFilesAsync(
                    daysThreshold: 3,
                    physicalDelete: true
                );

                var message = $"清理完成!\n\n" +
                             $"过期文件清理: {expiredCount} 个\n" +
                             $"孤立文件清理: {orphanedCount} 个\n" +
                             $"总计清理: {expiredCount + orphanedCount} 个";

                MessageBox.Show(message, "清理结果", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 刷新数据
                await LoadStorageInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清理失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnRefresh.Enabled = true;
            }
        }

        /// <summary>
        /// 显示监控详细信息
        /// </summary>
        private void ShowMonitorDetails()
        {
            if (_currentMonitorInfo == null)
            {
                MessageBox.Show("监控数据不可用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var details = new Form
            {
                Text = "文件存储监控详情",
                Size = new System.Drawing.Size(600, 500),
                StartPosition = FormStartPosition.CenterParent
            };

            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            listView.Columns.Add("监控项", 200);
            listView.Columns.Add("值", 300);

            var items = new[]
            {
                new { Name = "总文件数", Value = _currentMonitorInfo.TotalFiles.ToString() },
                new { Name = "正常文件数", Value = _currentMonitorInfo.ActiveFiles.ToString() },
                new { Name = "过期文件数", Value = _currentMonitorInfo.ExpiredFiles.ToString() },
                new { Name = "孤立文件数", Value = _currentMonitorInfo.OrphanedFiles.ToString() },
                new { Name = "总存储大小", Value = _currentMonitorInfo.TotalStorageSizeFormatted },
                new { Name = "文件存储使用率", Value = $"{_currentMonitorInfo.FileStorageUsagePercentage:F2}%" },
                new { Name = "磁盘使用率", Value = $"{_currentMonitorInfo.DiskUsagePercentage:F2}%" },
                new { Name = "可用磁盘空间", Value = $"{_currentMonitorInfo.FreeDiskSpaceGB:F2} GB" },
                new { Name = "总磁盘空间", Value = $"{_currentMonitorInfo.TotalDiskSpaceGB:F2} GB" },
                new { Name = "配置最大存储空间", Value = $"{_currentMonitorInfo.MaxStorageSizeGB:F2} GB" },
                new { Name = "警告阈值", Value = $"{_currentMonitorInfo.WarningThreshold}%" },
                new { Name = "紧急阈值", Value = $"{_currentMonitorInfo.CriticalThreshold}%" },
                new { Name = "最后检查时间", Value = _currentMonitorInfo.LastCheckTime.ToString("yyyy-MM-dd HH:mm:ss") },
                new { Name = "警告状态", Value = _currentMonitorInfo.IsWarning ? "是" : "否" },
                new { Name = "紧急状态", Value = _currentMonitorInfo.IsCritical ? "是" : "否" }
            };

            foreach (var item in items)
            {
                var listViewItem = new ListViewItem(item.Name);
                listViewItem.SubItems.Add(item.Value);
                listView.Items.Add(listViewItem);
            }

            details.Controls.Add(listView);
            details.ShowDialog();
        }

        /// <summary>
        /// 显示已删除文件列表并提供管理功能
        /// </summary>
        private async Task ShowDeletedFilesAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // 获取清理服务
                var cleanupService = Program.ServiceProvider.GetService<FileCleanupService>();
                if (cleanupService == null)
                {
                    MessageBox.Show("清理服务未找到", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 获取已删除文件列表
                var deletedFiles = await cleanupService.GetDeletedFilesAsync();

                // 创建已删除文件管理窗体
                var deletedFilesForm = new Form
                {
                    Text = "已删除文件管理",
                    Size = new System.Drawing.Size(1200, 700),
                    StartPosition = FormStartPosition.CenterParent,
                    MinimumSize = new System.Drawing.Size(1000, 600)
                };

                // 创建顶部面板（按钮）
                var topPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 60,
                    Padding = new System.Windows.Forms.Padding(10)
                };

                var btnPhysicalDelete = new Button
                {
                    Text = "物理删除选中文件",
                    Location = new System.Drawing.Point(10, 10),
                    Size = new System.Drawing.Size(150, 40),
                    BackColor = System.Drawing.Color.LightCoral,
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnPhysicalDelete.FlatAppearance.BorderSize = 0;

                var btnRestore = new Button
                {
                    Text = "恢复选中文件",
                    Location = new System.Drawing.Point(170, 10),
                    Size = new System.Drawing.Size(120, 40),
                    BackColor = System.Drawing.Color.LightGreen,
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnRestore.FlatAppearance.BorderSize = 0;

                var btnRefreshDeleted = new Button
                {
                    Text = "刷新列表",
                    Location = new System.Drawing.Point(300, 10),
                    Size = new System.Drawing.Size(100, 40),
                    FlatStyle = FlatStyle.Flat
                };

                var btnSelectAll = new Button
                {
                    Text = "全选",
                    Location = new System.Drawing.Point(410, 10),
                    Size = new System.Drawing.Size(80, 40),
                    FlatStyle = FlatStyle.Flat
                };

                var btnUnselectAll = new Button
                {
                    Text = "取消全选",
                    Location = new System.Drawing.Point(500, 10),
                    Size = new System.Drawing.Size(100, 40),
                    FlatStyle = FlatStyle.Flat
                };

                var lblCount = new Label
                {
                    Text = $"共 {deletedFiles.Count} 个已删除文件",
                    Location = new System.Drawing.Point(620, 20),
                    AutoSize = true,
                    Font = new System.Drawing.Font("微软雅黑", 10, System.Drawing.FontStyle.Bold)
                };

                topPanel.Controls.AddRange(new Control[] { 
                    btnPhysicalDelete, btnRestore, btnRefreshDeleted, btnSelectAll, btnUnselectAll, lblCount 
                });

                // 创建ListView
                var listView = new ListView
                {
                    Dock = DockStyle.Fill,
                    View = View.Details,
                    FullRowSelect = true,
                    GridLines = true,
                    MultiSelect = true,
                    CheckBoxes = true
                };

                // 添加列
                listView.Columns.Add("关联ID", 80);
                listView.Columns.Add("文件ID", 80);
                listView.Columns.Add("原始文件名", 200);
                listView.Columns.Add("文件大小", 100);
                listView.Columns.Add("业务编号", 120);
                listView.Columns.Add("业务类型", 120);
                listView.Columns.Add("关联字段", 120);
                listView.Columns.Add("业务主键ID", 100);
                listView.Columns.Add("文件状态", 80);
                listView.Columns.Add("删除时间", 160);

                // 添加数据
                foreach (var file in deletedFiles)
                {
                    try
                    {
                        var item = new ListViewItem(file.RelationId.ToString());
                        item.Tag = file.RelationId;
                        item.SubItems.Add(file.FileId.ToString());
                        item.SubItems.Add(file.OriginalFileName ?? "未知文件名");
                        item.SubItems.Add(file.FileSizeFormatted);
                        item.SubItems.Add(file.BusinessNo ?? "");
                        item.SubItems.Add(file.BusinessTypeName);
                        item.SubItems.Add(file.RelatedField ?? "");
                        item.SubItems.Add(file.BusinessId?.ToString() ?? "");
                        item.SubItems.Add(file.FileStatusName);
                        item.SubItems.Add(file.DeletedTime.ToString("yyyy-MM-dd HH:mm:ss"));

                        // 根据删除时间设置颜色
                        var daysSinceDeleted = (DateTime.Now - file.DeletedTime).Days;
                        if (daysSinceDeleted > 30)
                            item.BackColor = System.Drawing.Color.LightPink;
                        else if (daysSinceDeleted > 7)
                            item.BackColor = System.Drawing.Color.LightYellow;

                        listView.Items.Add(item);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"添加已删除文件到列表失败: {ex.Message}");
                        // 跳过有问题的记录，继续处理其他文件
                    }
                }

                // 刷新按钮事件
                btnRefreshDeleted.Click += async (s, e) =>
                {
                    try
                    {
                        deletedFilesForm.Cursor = Cursors.WaitCursor;
                        var refreshedFiles = await cleanupService.GetDeletedFilesAsync();
                        
                        listView.Items.Clear();
                        foreach (var file in refreshedFiles)
                        {
                            var item = new ListViewItem(file.RelationId.ToString());
                            item.Tag = file.RelationId;
                            item.SubItems.Add(file.FileId.ToString());
                            item.SubItems.Add(file.OriginalFileName);
                            item.SubItems.Add(file.FileSizeFormatted);
                            item.SubItems.Add(file.BusinessNo ?? "");
                            item.SubItems.Add(file.BusinessTypeName);
                            item.SubItems.Add(file.RelatedField ?? "");
                            item.SubItems.Add(file.BusinessId?.ToString() ?? "");
                            item.SubItems.Add(file.FileStatusName);
                            item.SubItems.Add(file.DeletedTime.ToString("yyyy-MM-dd HH:mm:ss"));

                            var daysSinceDeleted = (DateTime.Now - file.DeletedTime).Days;
                            if (daysSinceDeleted > 30)
                                item.BackColor = System.Drawing.Color.LightPink;
                            else if (daysSinceDeleted > 7)
                                item.BackColor = System.Drawing.Color.LightYellow;

                            listView.Items.Add(item);
                        }

                        lblCount.Text = $"共 {refreshedFiles.Count} 个已删除文件";
                        MessageBox.Show("刷新成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"刷新失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        deletedFilesForm.Cursor = Cursors.Default;
                    }
                };

                // 全选按钮事件
                btnSelectAll.Click += (s, e) =>
                {
                    foreach (ListViewItem item in listView.Items)
                        item.Checked = true;
                };

                // 取消全选按钮事件
                btnUnselectAll.Click += (s, e) =>
                {
                    foreach (ListViewItem item in listView.Items)
                        item.Checked = false;
                };

                // 物理删除按钮事件
                btnPhysicalDelete.Click += async (s, e) =>
                {
                    var selectedItems = listView.CheckedItems.Cast<ListViewItem>().ToList();
                    if (selectedItems.Count == 0)
                    {
                        MessageBox.Show("请先选择要删除的文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var confirmResult = MessageBox.Show(
                        $"确定要物理删除选中的 {selectedItems.Count} 个文件吗？\n\n" +
                        "注意：此操作不可恢复，将永久删除物理文件和数据库记录！",
                        "确认删除",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirmResult != DialogResult.Yes)
                        return;

                    try
                    {
                        deletedFilesForm.Cursor = Cursors.WaitCursor;

                        var relationIds = selectedItems.Select(item => (long)item.Tag).ToList();
                        var deletedCount = await cleanupService.PhysicalDeleteDeletedFilesAsync(relationIds);

                        MessageBox.Show(
                            $"物理删除完成！\n\n成功删除 {deletedCount} 个文件",
                            "删除结果",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        // 刷新列表
                        var refreshedFiles = await cleanupService.GetDeletedFilesAsync();
                        listView.Items.Clear();
                        foreach (var file in refreshedFiles)
                        {
                            var item = new ListViewItem(file.RelationId.ToString());
                            item.Tag = file.RelationId;
                            item.SubItems.Add(file.FileId.ToString());
                            item.SubItems.Add(file.OriginalFileName);
                            item.SubItems.Add(file.FileSizeFormatted);
                            item.SubItems.Add(file.BusinessNo ?? "");
                            item.SubItems.Add(file.BusinessTypeName);
                            item.SubItems.Add(file.RelatedField ?? "");
                            item.SubItems.Add(file.BusinessId?.ToString() ?? "");
                            item.SubItems.Add(file.FileStatusName);
                            item.SubItems.Add(file.DeletedTime.ToString("yyyy-MM-dd HH:mm:ss"));

                            var daysSinceDeleted = (DateTime.Now - file.DeletedTime).Days;
                            if (daysSinceDeleted > 30)
                                item.BackColor = System.Drawing.Color.LightPink;
                            else if (daysSinceDeleted > 7)
                                item.BackColor = System.Drawing.Color.LightYellow;

                            listView.Items.Add(item);
                        }

                        lblCount.Text = $"共 {refreshedFiles.Count} 个已删除文件";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"物理删除失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        deletedFilesForm.Cursor = Cursors.Default;
                    }
                };

                // 恢复按钮事件
                btnRestore.Click += async (s, e) =>
                {
                    var selectedItems = listView.CheckedItems.Cast<ListViewItem>().ToList();
                    if (selectedItems.Count == 0)
                    {
                        MessageBox.Show("请先选择要恢复的文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        deletedFilesForm.Cursor = Cursors.WaitCursor;

                        var relationIds = selectedItems.Select(item => (long)item.Tag).ToList();
                        var restoredCount = await cleanupService.RestoreDeletedFilesAsync(relationIds);

                        MessageBox.Show(
                            $"恢复完成！\n\n成功恢复 {restoredCount} 个文件关联",
                            "恢复结果",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        // 刷新列表
                        var refreshedFiles = await cleanupService.GetDeletedFilesAsync();
                        listView.Items.Clear();
                        foreach (var file in refreshedFiles)
                        {
                            var item = new ListViewItem(file.RelationId.ToString());
                            item.Tag = file.RelationId;
                            item.SubItems.Add(file.FileId.ToString());
                            item.SubItems.Add(file.OriginalFileName);
                            item.SubItems.Add(file.FileSizeFormatted);
                            item.SubItems.Add(file.BusinessNo ?? "");
                            item.SubItems.Add(file.BusinessTypeName);
                            item.SubItems.Add(file.RelatedField ?? "");
                            item.SubItems.Add(file.BusinessId?.ToString() ?? "");
                            item.SubItems.Add(file.FileStatusName);
                            item.SubItems.Add(file.DeletedTime.ToString("yyyy-MM-dd HH:mm:ss"));

                            var daysSinceDeleted = (DateTime.Now - file.DeletedTime).Days;
                            if (daysSinceDeleted > 30)
                                item.BackColor = System.Drawing.Color.LightPink;
                            else if (daysSinceDeleted > 7)
                                item.BackColor = System.Drawing.Color.LightYellow;

                            listView.Items.Add(item);
                        }

                        lblCount.Text = $"共 {refreshedFiles.Count} 个已删除文件";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"恢复失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        deletedFilesForm.Cursor = Cursors.Default;
                    }
                };

                deletedFilesForm.Controls.Add(topPanel);
                deletedFilesForm.Controls.Add(listView);
                deletedFilesForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载已删除文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}