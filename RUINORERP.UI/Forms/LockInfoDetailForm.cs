using System;
using System.Windows.Forms;
using RUINORERP.PacketSpec.Models.Lock;

namespace RUINORERP.UI.Forms
{
    /// <summary>
    /// 锁定信息详情显示窗口
    /// 用于展示LockInfo实体的完整信息
    /// </summary>
    public partial class LockInfoDetailForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lockInfo">要显示的锁定信息对象</param>
        public LockInfoDetailForm(LockInfo lockInfo)
        {
            InitializeComponent();
            DisplayLockInfoDetails(lockInfo);
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
            // LockId属性不存在，使用空字符串
            lblLockId.Text = "";
            lblBillID.Text = lockInfo.BillID.ToString() ?? "";
            lblBillNo.Text = lockInfo.BillNo ?? "";

            // 用户信息
            lblLockedUserId.Text = lockInfo.LockedUserId.ToString() ?? "";
            lblLockedUserName.Text = lockInfo.LockedUserName ?? "";

            // 时间信息
            lblLockTime.Text = lockInfo.LockTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            // 修复ExpireTime的处理，检查是否为null
            if (lockInfo.ExpireTime.HasValue)
                lblExpireTime.Text = lockInfo.ExpireTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            else
                lblExpireTime.Text = "";
            
            // LastHeartbeat不是可空类型
            lblLastHeartbeat.Text = lockInfo.LastHeartbeat.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            // LastUpdateTime不是可空类型
            lblLastUpdateTime.Text = lockInfo.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            // 状态信息
            lblIsLocked.Text = lockInfo.IsLocked ? "是" : "否";
            lblIsExpired.Text = lockInfo.IsExpired ? "是" : "否";
            lblIsOrphaned.Text = lockInfo.IsOrphaned ? "是" : "否";
            lblIsAboutToExpire.Text = lockInfo.IsAboutToExpire ? "是" : "否";
            
            // 业务信息
            // 修复BizType的处理，不能直接与字符串比较
            lblBizType.Text = lockInfo.bizType.ToString() ?? "";
            lblMenuID.Text = lockInfo.MenuID.ToString() ?? "";
            
            // 会话信息
            lblSessionId.Text = lockInfo.SessionId ?? "";
            
            // 其他信息
            lblRemark.Text = lockInfo.Remark ?? "";
            // LockType不是可空类型
            lblType.Text = lockInfo.Type.ToString() ?? "";
            
            // 根据锁定状态组合显示文本
            string statusText = "";
            if (lockInfo.IsLocked)
                statusText = "已锁定";
            else
                statusText = "未锁定";
            
            if (lockInfo.IsExpired)
                statusText += " (已过期)";
            
            if (lockInfo.IsOrphaned)
                statusText += " (孤儿锁)";
            
            if (lockInfo.IsAboutToExpire)
                statusText += " (即将过期)";
            
            lblStatus.Text = statusText;
            
            // 统计信息
            // HeartbeatCount不是可空类型
            lblHeartbeatCount.Text = lockInfo.HeartbeatCount.ToString() ?? "0";
            // Duration不是可空类型
            lblDuration.Text = lockInfo.Duration.ToString() ?? "0";
            
            // 计算属性
            if (lockInfo.ExpireTimestamp.HasValue)
                lblExpireTimestamp.Text = lockInfo.ExpireTimestamp.Value.ToString();
            else
                lblExpireTimestamp.Text = "";
                
            if (lockInfo.RemainingLockTimeMs.HasValue)
                lblRemainingLockTimeMs.Text = lockInfo.RemainingLockTimeMs.Value.ToString();
            else
                lblRemainingLockTimeMs.Text = "";
            
            // 计算并显示锁定持续时间
            var duration = DateTime.Now - lockInfo.LockTime;
            lblDurationText.Text = $"{duration.Days}天 {duration.Hours}小时 {duration.Minutes}分钟 {duration.Seconds}秒";
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}