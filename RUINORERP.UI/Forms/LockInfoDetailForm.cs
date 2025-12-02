using System;
using System.Drawing;
using System.Windows.Forms;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.Forms
{
    /// <summary>
    /// 锁定信息详情窗体
    /// 优化版本：使用设计器生成的Tab布局，同时提供刷新和解锁功能
    /// </summary>
    public partial class LockInfoDetailForm : Form
    {
        private readonly LockInfo _lockInfo;
        private readonly ClientLocalLockCacheService _lockCacheService;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lockInfo">要显示的锁定信息对象</param>
        public LockInfoDetailForm(LockInfo lockInfo)
        {
            InitializeComponent();
            _lockInfo = lockInfo ?? throw new ArgumentNullException(nameof(lockInfo));
            _lockCacheService = Startup.GetFromFac<ClientLocalLockCacheService>();
            
            // 设置窗体样式和布局
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 初始化界面并绑定事件
            InitializeCustomComponents();
            DisplayLockInfoDetails(lockInfo);
        }
        
        /// <summary>
        /// 初始化自定义组件和事件绑定
        /// </summary>
        private void InitializeCustomComponents()
        {
            // 关闭按钮事件由Designer.cs管理，这里不再重复绑定
            
            // 在底部添加额外的操作按钮
            Panel actionPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                Padding = new Padding(10)
            };
            
            Button btnRefresh = new Button
            {
                Text = "刷新信息",
                Size = new Size(100, 28),
                Location = new Point(10, 5)
            };
            btnRefresh.Click += btnRefreshInfo_Click;
            
            Button btnUnlock = new Button
            {
                Text = "解锁单据",
                Size = new Size(100, 28),
                Location = new Point(120, 5),
                Enabled = _lockInfo?.IsLocked ?? false
            };
            btnUnlock.Click += btnUnlockInfo_Click;
            
            actionPanel.Controls.Add(btnRefresh);
            actionPanel.Controls.Add(btnUnlock);
            
            // 调整主布局以容纳新面板
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanelMain.RowCount = 3;
            tableLayoutPanelMain.SetRow(actionPanel, 2);
            tableLayoutPanelMain.Controls.Add(actionPanel, 0, 2);
        }
        
        /// <summary>
        /// 显示锁定信息详情
        /// </summary>
        /// <param name="lockInfo">锁定信息对象</param>
        private void DisplayLockInfoDetails(LockInfo lockInfo)
        {
            if (lockInfo == null)
                return;

            // 基本信息
            lblLockKey.Text = lockInfo.LockKey ?? "";
            lblLockId.Text = string.Empty;
            lblBillID.Text = lockInfo.BillID.ToString();
            lblBillNo.Text = lockInfo.BillNo ?? "";

            // 用户信息
            lblLockedUserId.Text = lockInfo.LockedUserId.ToString();
            lblLockedUserName.Text = lockInfo.LockedUserName ?? "";

            // 时间信息
            if (lockInfo.GetType().GetProperty("LockTime")?.GetValue(lockInfo) is DateTime lockTime)
                lblLockTime.Text = lockTime.ToString("yyyy-MM-dd HH:mm:ss");
            
            // 过期时间
            if (lockInfo.ExpireTime.HasValue)
                lblExpireTime.Text = lockInfo.ExpireTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            else
                lblExpireTime.Text = string.Empty;

            // 更新最后心跳和更新时间
            if (lockInfo.GetType().GetProperty("LastHeartbeat")?.GetValue(lockInfo) is DateTime lastHeartbeat)
                lblLastHeartbeat.Text = lastHeartbeat.ToString("yyyy-MM-dd HH:mm:ss");
            
            lblLastUpdateTime.Text = lockInfo.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");

            // 状态信息
            lblIsLocked.Text = lockInfo.IsLocked ? "已锁定" : "未锁定";
            if (lblIsLocked.Text == "已锁定")
                lblIsLocked.ForeColor = Color.Red;
                
            lblIsExpired.Text = lockInfo.IsExpired ? "是" : "否";
            if (lblIsExpired.Text == "是")
                lblIsExpired.ForeColor = Color.Orange;
                
            if (lockInfo.GetType().GetProperty("IsOrphaned")?.GetValue(lockInfo) is bool isOrphaned)
                lblIsOrphaned.Text = isOrphaned ? "是" : "否";
            else
                lblIsOrphaned.Text = "否";
                
            if (lockInfo.GetType().GetProperty("IsAboutToExpire")?.GetValue(lockInfo) is bool isAboutToExpire)
                lblIsAboutToExpire.Text = isAboutToExpire ? "是" : "否";
            else
                lblIsAboutToExpire.Text = "否";
            
            if (lockInfo.GetType().GetProperty("Type")?.GetValue(lockInfo) is Enum lockType)
                lblType.Text = lockType.ToString();
            else
                lblType.Text = "未知";

            // 业务信息
            if (lockInfo.GetType().GetProperty("bizType")?.GetValue(lockInfo) is object bizType)
                lblBizType.Text = bizType.ToString();
            else
                lblBizType.Text = string.Empty;
                
            lblMenuID.Text = lockInfo.MenuID.ToString();
            
            if (lockInfo.GetType().GetProperty("SessionId")?.GetValue(lockInfo) is string sessionId)
                lblSessionId.Text = sessionId;
            else
                lblSessionId.Text = string.Empty;

            // 其他信息
            lblRemark.Text = lockInfo.Remark ?? "";
            
            if (lockInfo.GetType().GetProperty("HeartbeatCount")?.GetValue(lockInfo) is int heartbeatCount)
                lblHeartbeatCount.Text = heartbeatCount.ToString();
            else
                lblHeartbeatCount.Text = "0";
                
            if (lockInfo.GetType().GetProperty("Duration")?.GetValue(lockInfo) is int duration)
                lblDuration.Text = duration.ToString();
            else
                lblDuration.Text = "0";
                
            if (lockInfo.GetType().GetProperty("ExpireTimestamp")?.GetValue(lockInfo) is long expireTimestamp)
                lblExpireTimestamp.Text = expireTimestamp.ToString();
            else
                lblExpireTimestamp.Text = string.Empty;
                
            if (lockInfo.GetType().GetProperty("RemainingLockTimeMs")?.GetValue(lockInfo) is long remainingTime)
                lblRemainingLockTimeMs.Text = remainingTime.ToString();
            else
                lblRemainingLockTimeMs.Text = string.Empty;

            // 计算并显示锁定持续时间
            if (lockInfo.GetType().GetProperty("LockTime")?.GetValue(lockInfo) is DateTime lockStartTime)
            {
                var durationTime = DateTime.Now - lockStartTime;
                lblDurationText.Text = $"已锁定: {durationTime.Hours}小时 {durationTime.Minutes}分钟 {durationTime.Seconds}秒";
            }
            else
            {
                lblDurationText.Text = "计算持续时间失败";
            }

            // 组合显示状态文本
            string statusText = lockInfo.IsLocked ? "已锁定" : "未锁定";
            if (lockInfo.IsExpired)
                statusText += " (已过期)";
            if (lblIsOrphaned.Text == "是")
                statusText += " (孤儿锁)";
            if (lblIsAboutToExpire.Text == "是")
                statusText += " (即将过期)";
            
            lblStatus.Text = statusText;
        }
        
        /// <summary>
        /// 刷新按钮点击事件处理
        /// </summary>
        private async void btnRefreshInfo_Click(object sender, EventArgs e)
        {
            try
            {
                // 从缓存服务刷新最新的锁定信息
                var updatedLockInfo = await _lockCacheService.GetLockInfoAsync(_lockInfo.BillID);
                if (updatedLockInfo != null)
                {
                    // 更新本地对象
                    UpdateLockInfoFromSource(_lockInfo, updatedLockInfo);
                    // 重新显示
                    DisplayLockInfoDetails(_lockInfo);
                    MessageBox.Show("锁定信息已成功刷新", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("未能获取最新锁定信息，可能已被释放", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新锁定信息失败: {ex.Message}", "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 解锁按钮点击事件处理
        /// </summary>
        private async void btnUnlockInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确认要解锁此单据吗？此操作可能会影响正在编辑的用户。", "操作确认", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    long currentUserId = MainForm.Instance.AppContext.CurrentUser.UserID;
                    bool success = await _lockCacheService.UnlockAsync(_lockInfo.BillID, currentUserId);
                    
                    if (success)
                    {
                        MessageBox.Show("单据解锁成功", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("解锁操作失败，请检查权限或网络连接", "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解锁操作异常: {ex.Message}", "系统异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        /// <summary>
        /// 更新锁定信息对象
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="source">源对象</param>
        private void UpdateLockInfoFromSource(LockInfo target, LockInfo source)
        {
            if (target == null || source == null)
                return;
            
            // 更新所有可读可写的属性
            var properties = typeof(LockInfo).GetProperties();
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite && property.Name != "BillID") // 保持原始BillID
                {
                    try
                    {
                        var value = property.GetValue(source);
                        property.SetValue(target, value);
                    }
                    catch { /* 忽略属性更新错误 */ }
                }
            }
        }
    }
}