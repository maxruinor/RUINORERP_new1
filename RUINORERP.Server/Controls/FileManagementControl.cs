using RUINORERP.Server.Network.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using RUINORERP.PacketSpec.Models.FileManagement;

namespace RUINORERP.Server.Controls
{
    public partial class FileManagementControl : UserControl
    {
        private readonly FileCommandHandler _fileCommandHandler;
        private readonly Timer _updateTimer;
        private StorageUsageInfoData _currentStorageInfo;

        public FileManagementControl()
        {
            InitializeComponent();
            
            // 初始化文件命令处理器（需要从实际服务中获取）
            _fileCommandHandler = GetFileCommandHandler();
            
            // 设置定时器，定期更新存储信息
            _updateTimer = new Timer { Interval = 60000 }; // 每分钟更新一次
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();
            
            // 初始化列表视图
            InitializeListView();
            
            // 初始加载数据
            LoadStorageInfo();
        }

        private FileCommandHandler GetFileCommandHandler()
        {   
            // 这里应该从依赖注入或服务定位器获取实际的FileCommandHandler实例
            // 由于是示例，这里返回null
            return null;
        }

        private void InitializeListView()
        {   
            // 设置列表视图属性
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.AllowColumnReorder = true;
            
            // 添加列
            listView1.Columns.Add("文件分类", 120);
            listView1.Columns.Add("文件数量", 80);
            listView1.Columns.Add("已用空间", 100);
            listView1.Columns.Add("最后同步时间", 150);
            listView1.Columns.Add("同步状态", 100);
            listView1.Columns.Add("错误信息", 200);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {   
            LoadStorageInfo();
        }

        public async void LoadStorageInfo()
        {   
            try
            {   
                // 模拟从服务器获取存储信息
                // 实际应该调用FileCommandHandler或相关服务获取数据
                _currentStorageInfo = GetMockStorageInfo();
                
                // 更新UI
                UpdateStorageSummary();
                UpdateCategoryList();
                
                // 更新状态标签
                lblLastUpdateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                picStatusIndicator.BackColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {   
                picStatusIndicator.BackColor = System.Drawing.Color.Red;
                lblStatus.Text = "更新失败: " + ex.Message;
                Console.WriteLine("加载存储信息失败: " + ex.Message);
            }
        }

        private StorageUsageInfoData GetMockStorageInfo()
        {   
            // 创建模拟数据用于演示
            var storageInfo = new StorageUsageInfoData
            {   
                TotalSize = 50L * 1024L * 1024L * 1024L, // 50GB
                TotalFileCount = 1250,
                CategoryUsage = new Dictionary<string, CategoryUsage>()
                {
                    { "产品图片", new CategoryUsage { FileCount = 843, TotalSize = 12L * 1024 * 1024 * 1024 } },
                    { "报销凭证", new CategoryUsage { FileCount = 312, TotalSize = (long)(2.5 * 1024 * 1024 * 1024) } },
                    { "付款凭证", new CategoryUsage { FileCount = 95, TotalSize = (long)(0.5 * 1024 * 1024 * 1024) } }
                }
            };
            
            return storageInfo;
        }

        private void UpdateStorageSummary()
        {   
            if (_currentStorageInfo == null)
                return;
            
            // 更新存储概览
            long totalSize = _currentStorageInfo.TotalSize;
            // 计算已用空间（所有分类的大小总和）
            long usedSize = _currentStorageInfo.CategoryUsage.Sum(c => c.Value.TotalSize);
            long freeSize = totalSize - usedSize;
            
            lblTotalStorage.Text = FormatBytes(totalSize);
            lblUsedStorage.Text = FormatBytes(usedSize);
            lblFreeStorage.Text = FormatBytes(freeSize);
            
            // 计算使用百分比
            int usagePercentage = (int)((double)usedSize / totalSize * 100);
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
            lblTotalFiles.Text = _currentStorageInfo.TotalFileCount.ToString();
        }

        private void UpdateCategoryList()
        {   
            listView1.Items.Clear();
            
            if (_currentStorageInfo == null || _currentStorageInfo.Categories == null)
                return;
            
            foreach (var category in _currentStorageInfo.Categories)
            {   
                ListViewItem item = new ListViewItem(category.CategoryName);
                item.SubItems.Add(category.FileCount.ToString());
                item.SubItems.Add(FormatBytes(category.StorageSize));
                item.SubItems.Add(category.LastSyncTime.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(category.SyncStatus);
                item.SubItems.Add(category.ErrorMessage ?? "无");
                
                // 根据同步状态设置行颜色
                if (category.SyncStatus == "已同步")
                    item.BackColor = System.Drawing.Color.LightGreen;
                else if (category.SyncStatus == "同步中")
                    item.BackColor = System.Drawing.Color.LightYellow;
                else if (category.SyncStatus == "同步失败")
                    item.BackColor = System.Drawing.Color.LightPink;
                
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

        private void btnViewDetails_Click(object sender, EventArgs e)
        {   
            // 打开详细视图功能
            if (listView1.SelectedItems.Count > 0)
            {   
                string categoryName = listView1.SelectedItems[0].Text;
                ShowCategoryDetails(categoryName);
            }
            else
            {   
                MessageBox.Show("请先选择一个文件分类");
            }
        }

        private void ShowCategoryDetails(string categoryName)
        {   
            // 这里可以实现显示特定分类的详细信息
            // 例如打开一个新窗口显示该分类下的所有文件
            MessageBox.Show($"显示分类 '{categoryName}' 的详细信息");
        }

        private void FileManagementControl_Load(object sender, EventArgs e)
        {   
            LoadStorageInfo();
        }
    }
}