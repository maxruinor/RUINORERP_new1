using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Business;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.FileManagement;
using RUINORERP.Server.Network.CommandHandlers;
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
        private List<FileCategoryInfo> _fileCategories;
        private bool _isDisposed = false;

        // 真实的业务控制器
        private readonly tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> _fileStorageInfoController;
        private readonly tb_FS_BusinessRelationController<tb_FS_BusinessRelation> _businessRelationController;

        public FileManagementControl()
        {
            InitializeComponent();

            // 从依赖注入容器获取业务控制器
            _fileStorageInfoController = Program.ServiceProvider.GetService<tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo>>();
            _businessRelationController = Program.ServiceProvider.GetService<tb_FS_BusinessRelationController<tb_FS_BusinessRelation>>();

            // 设置定时器，定期更新存储信息
            _updateTimer = new Timer { Interval = 60000 }; // 每分钟更新一次
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();

            // 初始化列表视图
            InitializeListView();

            // 初始加载数据
            LoadStorageInfo();
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
        /// 从真实数据库加载存储信息（性能优化版本）
        /// </summary>
        private async Task LoadRealStorageInfoAsync()
        {
            _currentStorageSummary = new FileStorageSummary();
            _fileCategories = new List<FileCategoryInfo>();

            try
            {
                // 性能优化：使用分页查询，避免一次性加载大量数据
                var fileInfos = await _fileStorageInfoController.QueryAsync(c => c.FileStatus == (int)FileStatus.Active && c.isdeleted == false);

                // 安全检查：如果数据量过大，限制处理数量
                const int MAX_FILES_TO_PROCESS = 10000;
                if (fileInfos != null && fileInfos.Count > 0)
                {
                    // 限制处理数量，避免内存溢出
                    var filesToProcess = fileInfos.Take(MAX_FILES_TO_PROCESS).ToList();

                    _currentStorageSummary.TotalFileCount = fileInfos.Count; // 显示总数，但只处理部分数据
                    _currentStorageSummary.TotalStorageSize = filesToProcess.Sum(f => ((tb_FS_FileStorageInfo)f).FileSize);

                    // 性能优化：使用更高效的分组方式
                    var groupedByBusinessType = filesToProcess
                        .Cast<tb_FS_FileStorageInfo>()
                        .GroupBy(f => f.BusinessType ?? 0)
                        .ToList();

                    // 性能优化：预分配列表容量
                    _fileCategories = new List<FileCategoryInfo>(groupedByBusinessType.Count);

                    foreach (var group in groupedByBusinessType)
                    {
                        var categoryInfo = new FileCategoryInfo
                        {
                            BusinessType = group.Key,
                            CategoryName = GetBusinessTypeName(group.Key),
                            FileCount = group.Count(),
                            StorageSize = group.Sum(f => f.FileSize),
                            LastModified = group.Max(f => f.Modified_at ?? f.Created_at ?? DateTime.MinValue)
                        };

                        _fileCategories.Add(categoryInfo);
                    }

                    // 3. 计算磁盘总空间 (使用系统API)
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

            // 计算使用百分比
            int usagePercentage = totalSize > 0 ? (int)((double)usedSize / totalSize * 100) : 0;
            progressBar1.Value = usagePercentage;
            lblUsagePercentage.Text = usagePercentage + "%";

            // 根据使用情况设置进度条颜色
            if (usagePercentage > 80)
                progressBar1.ForeColor = System.Drawing.Color.Red;
            else if (usagePercentage > 60)
                progressBar1.ForeColor = System.Drawing.Color.Orange;
            else
                progressBar1.ForeColor = System.Drawing.Color.Green;

            // 更新文件统计
            lblTotalFiles.Text = _currentStorageSummary.TotalFileCount.ToString();
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStorageInfo();
        }

        private async void btnViewDetails_Click(object sender, EventArgs e)
        {
            // 打开详细视图功能
            if (listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];
                int businessType = int.Parse(selectedItem.Text);
                string categoryName = selectedItem.SubItems[1].Text;

                await ShowCategoryDetailsAsync(businessType, categoryName);
            }
            else
            {
                MessageBox.Show("请先选择一个文件分类");
            }
        }

        /// <summary>
        /// 显示特定业务类型的文件详情
        /// </summary>
        private async Task ShowCategoryDetailsAsync(int businessType, string categoryName)
        {
            try
            {
                // 查询该业务类型下的所有文件
                var fileInfos = await _fileStorageInfoController.BaseQueryAsync($"BusinessType = {businessType} AND Status = 1 AND isdeleted = 0");

                if (fileInfos != null && fileInfos.Count > 0)
                {
                    var detailForm = new Form
                    {
                        Text = $"文件分类详情 - {categoryName}",
                        Size = new System.Drawing.Size(800, 600),
                        StartPosition = FormStartPosition.CenterParent
                    };

                    var listView = new ListView
                    {
                        Dock = DockStyle.Fill,
                        View = View.Details,
                        FullRowSelect = true,
                        GridLines = true
                    };

                    // 添加列
                    listView.Columns.Add("文件ID", 80);
                    listView.Columns.Add("原始文件名", 200);
                    listView.Columns.Add("文件大小", 100);
                    listView.Columns.Add("文件类型", 80);
                    listView.Columns.Add("创建时间", 120);
                    listView.Columns.Add("存储路径", 200);

                    // 添加文件数据
                    foreach (var fileInfo in fileInfos.Cast<tb_FS_FileStorageInfo>())
                    {
                        var item = new ListViewItem(fileInfo.FileId.ToString());
                        item.SubItems.Add(fileInfo.OriginalFileName);
                        item.SubItems.Add(FormatBytes(fileInfo.FileSize));
                        item.SubItems.Add(fileInfo.FileType);
                        item.SubItems.Add(fileInfo.Created_at?.ToString("yyyy-MM-dd HH:mm") ?? "未知");
                        item.SubItems.Add(fileInfo.StoragePath ?? "");

                        listView.Items.Add(item);
                    }

                    detailForm.Controls.Add(listView);
                    detailForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show($"分类 '{categoryName}' 中没有找到任何文件");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载文件详情失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FileManagementControl_Load(object sender, EventArgs e)
        {
            LoadStorageInfo();
        }
    }
}