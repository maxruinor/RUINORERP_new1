using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.UControls;
using RUINORERP.PacketSpec.Models.Lock;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.UI.AdvancedUIModule;
using static RUINORERP.Model.ModuleMenuDefine;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.Forms; // 添加缺少的命名空间引用


namespace RUINORERP.UI.SysConfig
{
    [RUINORERP.UI.Common.MenuAttrAssemblyInfo("锁定状态管理", 模块定义.系统设置, 系统设置.系统工具)]
    public partial class UCClientLockManage : UserControl, IContextMenuInfoAuth
    {
        private readonly ClientLocalLockCacheService _lockCacheService;
        private readonly ClientLockManagementService lockManagementService;
        private System.Windows.Forms.Timer _refreshTimer;
        private int _totalCacheCount = 0;
        private int _lockedCount = 0;
        private int _expiredCount = 0;
        private int _currentUserLockedCount = 0;

        public UCClientLockManage()
        {
            InitializeComponent();
            _lockCacheService = Startup.GetFromFac<ClientLocalLockCacheService>();
            lockManagementService = Startup.GetFromFac<ClientLockManagementService>();
            InitializeRefreshTimer();
            InitializeLockDataGridView();
            LoadLockData();
            
            // 订阅行双击事件
            dgvLockInfo.CellDoubleClick += dgvLockInfo_CellDoubleClick;
        }
        
        /// <summary>
        /// 处理表格行双击事件，弹出锁定信息详情窗口
        /// 优化版本：使用单一大型窗体显示详细信息
        /// </summary>
        private void dgvLockInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvLockInfo.Rows.Count)
            {
                var row = dgvLockInfo.Rows[e.RowIndex];
                var lockInfo = row.DataBoundItem as LockInfo;
                
                if (lockInfo != null)
                {
                    // 显示锁定信息详情，使用单独的大型窗体展示
                    LockInfoDetailForm detailForm = new LockInfoDetailForm(lockInfo);
                    detailForm.StartPosition = FormStartPosition.CenterParent;
                    detailForm.ShowDialog(this);
                    
                    // 关闭详情窗体后刷新锁定列表，确保显示最新状态
                    LoadLockData();
                }
            }
        }

        private void InitializeLockDataGridView()
        {
            dgvLockInfo.AutoGenerateColumns = false;
            dgvLockInfo.AllowUserToAddRows = false;
            dgvLockInfo.AllowUserToDeleteRows = false;
            dgvLockInfo.ReadOnly = true;

            // 添加最重要的锁定信息列
            // 单据编号

            dgvLockInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BillNo",
                HeaderText = "单据编号",
                Width = 100
            });

            dgvLockInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LockedUserName",
                HeaderText = "锁定用户",
                Width = 120
            });

            dgvLockInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LockTime",
                HeaderText = "锁定时间",
                Width = 150
            });

            dgvLockInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ExpireTime",
                HeaderText = "过期时间",
                Width = 150
            });

            dgvLockInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "bizType",
                HeaderText = "业务类型",
                Width = 120
            });

            dgvLockInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IsExpired",
                HeaderText = "是否过期",
                Width = 80
            });
        }

        private void InitializeRefreshTimer()
        {
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 30000; // 30秒刷新一次
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadLockData();
        }

        private void LoadLockData()
        {
            try
            {
                var lockInfos = GetAllLockInfos();
                UpdateStatistics(lockInfos);
                dgvLockInfo.DataSource = new BindingList<LockInfo>(lockInfos);
                UpdateStatusLabels();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载锁定数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<LockInfo> GetAllLockInfos()
        {
            try
            {
                // 直接使用公共方法获取所有锁定信息，避免反射访问私有字段
                return _lockCacheService.GetAllLockInfos();
            }
            catch (Exception ex)
            {
                // 记录异常日志
                System.Diagnostics.Debug.WriteLine($"获取锁定信息失败: {ex.Message}");
                return new List<LockInfo>();
            }
        }

        private void UpdateStatistics(List<LockInfo> lockInfos)
        {
            _totalCacheCount = lockInfos.Count;
            _lockedCount = lockInfos.Count(info => info.IsLocked);
            _expiredCount = lockInfos.Count(info => info.IsExpired);
            _currentUserLockedCount = lockInfos.Count(info => info.IsLocked && info.LockedUserName == Environment.UserName);
        }

        private void UpdateStatusLabels()
        {
            lblTotalCacheCount.Text = $"总缓存数: {_totalCacheCount}";
            lblLockedCount.Text = $"锁定项数: {_lockedCount}";
            lblExpiredCount.Text = $"过期项数: {_expiredCount}";
            lblCurrentUserLockedCount.Text = $"当前用户锁定项数: {_currentUserLockedCount}";
        }

        private void btnRefreshLockData_Click(object sender, EventArgs e)
        {
            LoadLockData();
        }

        private async void btnUnlockSelected_Click(object sender, EventArgs e)
        {
            if (dgvLockInfo.SelectedRows.Count > 0)
            {
                // 禁用按钮防止重复点击
                btnUnlockSelected.Enabled = false;
                
                try
                {
                    // 使用异步方式逐个解锁
                    int successCount = 0;
                    foreach (DataGridViewRow row in dgvLockInfo.SelectedRows)
                    {
                        if (row.DataBoundItem is LockInfo lockInfo)
                        {
                            if (await UnlockItemAsync(lockInfo))
                            {
                                successCount++;
                            }
                        }
                    }
                    
                    // 刷新数据
                    LoadLockData();
                    
                    if (successCount > 0)
                    {
                        MessageBox.Show($"成功解锁 {successCount} 个项目", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                finally
                {
                    // 确保按钮恢复可用状态
                    btnUnlockSelected.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("请先选择要解锁的项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClearAllCache_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清空所有锁定缓存吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _lockCacheService.ClearAllCache();
                    LoadLockData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"清空缓存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task<bool> UnlockItemAsync(LockInfo lockInfo)
        {
            try
            {
                long userid = MainForm.Instance.AppContext.CurUserInfo.UserID;
                // 使用正确的异步调用方式，避免死锁
                await _lockCacheService.UnlockAsync(lockInfo.BillID, userid);
                await lockManagementService.UnlockBillAsync(lockInfo.BillID);
                return true;
            }
            catch (Exception ex)
            {
                string lockKey = lockInfo?.LockKey ?? "未知";
                MessageBox.Show($"解锁项 {lockKey} 失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void dgvLockInfo_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLockInfo.Rows[e.RowIndex].DataBoundItem is LockInfo lockInfo)
            {
                if (lockInfo.IsExpired)
                {
                    dgvLockInfo.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                }
                else if (lockInfo.IsLocked)
                {
                    dgvLockInfo.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                }
                else
                {
                    dgvLockInfo.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        public List<UControls.ContextMenuController> AddContextMenu()
        {
            return new List<UControls.ContextMenuController>();
        }
    }
}