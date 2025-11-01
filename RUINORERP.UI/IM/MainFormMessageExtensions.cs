using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Log;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// MainForm消息功能扩展类
    /// 用于处理消息相关的扩展方法
    /// </summary>
    public static class MainFormMessageExtensions
    {
        /// <summary>
        /// 为MainForm显示消息列表
        /// </summary>
        /// <param name="messageManager">消息管理器实例</param>
        /// <param name="logger">日志记录器</param>
        public static void ShowMessageListWithErrorHandling(this EnhancedMessageManager messageManager, ILogger logger = null)
        {
            try
            {
                messageManager?.ShowMessageList();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "显示消息列表时发生异常");
                MessageBox.Show("显示消息列表时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 处理消息状态变更事件
        /// </summary>
        /// <param name="form">窗体实例</param>
        /// <param name="updateStatusBarAction">更新状态栏的动作</param>
        /// <param name="logger">日志记录器</param>
        public static void HandleMessageStatusChanged(this Form form, Action updateStatusBarAction, ILogger logger = null)
        {
            try
            {
                if (form.InvokeRequired)
                {
                    form.Invoke(updateStatusBarAction);
                }
                else
                {
                    updateStatusBarAction();
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "更新消息状态显示时发生异常");
            }
        }
        
        /// <summary>
        /// 更新状态栏的未读消息显示
        /// </summary>
        /// <param name="form">窗体实例</param>
        /// <param name="messageManager">消息管理器实例</param>
        public static void UpdateMessageStatusDisplay(this Form form, EnhancedMessageManager messageManager)
        {
            if (messageManager == null) return;
            
            int unreadCount = messageManager.UnreadMessageCount;
            if (unreadCount <= 0) return;
            
            // 查找StatusStrip并更新或创建消息状态栏标签
            var statusStrip = form.Controls.Find("statusStrip1", true).FirstOrDefault() as StatusStrip;
            if (statusStrip != null)
            {
                var existingLabel = statusStrip.Items.Find("toolStripStatusMessage", false).FirstOrDefault() as ToolStripStatusLabel;
                if (existingLabel != null)
                {
                    existingLabel.Text = $"未读消息: {unreadCount}";
                }
                else
                {
                    ToolStripStatusLabel newLabel = new ToolStripStatusLabel
                    {
                        Text = $"未读消息: {unreadCount}",
                        Name = "toolStripStatusMessage",
                        Margin = new Padding(5, 0, 5, 0)
                    };
                    statusStrip.Items.Add(newLabel);
                }
            }
        }
    }
}