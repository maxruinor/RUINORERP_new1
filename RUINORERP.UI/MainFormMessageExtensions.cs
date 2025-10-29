using RUINORERP.UI.IM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands.Message;

namespace RUINORERP.UI
{
    /// <summary>
    /// 主窗体消息功能扩展
    /// </summary>
    public static class MainFormMessageExtensions
    {
        /// <summary>
        /// 初始化增强版消息菜单
        /// </summary>
        public static void InitializeEnhancedMessageMenu(this MainForm mainForm)
        {
            try
            {
                MenuStrip menuStrip = null;

                // 查找主菜单条
                menuStrip = mainForm.Controls.Find("MenuStripMain", true).FirstOrDefault() as MenuStrip;
                if (menuStrip == null)
                {
                    menuStrip = mainForm.Controls.OfType<MenuStrip>().FirstOrDefault();
                }

                if (menuStrip != null)
                {
                    // 查找或创建消息菜单
                    ToolStripMenuItem messageMenu = null;
                    foreach (ToolStripMenuItem item in menuStrip.Items)
                    {
                        if (item.Text.StartsWith("消息"))
                        {
                            messageMenu = item;
                            break;
                        }
                    }

                    // 如果不存在，创建消息菜单
                    if (messageMenu == null)
                    {
                        messageMenu = new ToolStripMenuItem("消息");
                        menuStrip.Items.Add(messageMenu);
                    }

                    // 清除现有菜单项
                    messageMenu.DropDownItems.Clear();

                    // 添加查看消息中心菜单项
                    var showMessageCenterItem = new ToolStripMenuItem("消息中心");
                    showMessageCenterItem.Click += (s, e) => ShowMessageCenter(mainForm);
                    messageMenu.DropDownItems.Add(showMessageCenterItem);

                    // 添加分隔符
                    messageMenu.DropDownItems.Add(new ToolStripSeparator());

                    // 添加标记全部已读菜单项
                    var markAllReadItem = new ToolStripMenuItem("全部标记为已读");
                    markAllReadItem.Click += (s, e) => MarkAllMessagesAsRead(mainForm);
                    messageMenu.DropDownItems.Add(markAllReadItem);
                }
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，避免影响主程序运行
            }
        }

        /// <summary>
        /// 显示消息中心
        /// </summary>
        private static void ShowMessageCenter(MainForm mainForm)
        {
            try
            {
                var messageManager = mainForm.GetMessageManager() as EnhancedMessageManager;
                messageManager?.ShowEnhancedMessageList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开消息中心时发生错误: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 标记所有消息为已读
        /// </summary>
        private static void MarkAllMessagesAsRead(MainForm mainForm)
        {
            try
            {
                var result = MessageBox.Show(
                    "确定要将所有消息标记为已读吗？", "确认操作", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var messageManager = mainForm.GetMessageManager();
                    messageManager?.MarkAllMessagesAsRead();

                    MessageBox.Show("已成功将所有消息标记为已读", "操作成功", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"标记消息为已读时发生错误: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 获取消息管理器实例
        /// </summary>
        private static MessageManager GetMessageManager(this MainForm mainForm)
        {
            // 直接使用主窗体中的消息管理器实例
            return mainForm.GetMessageManager();
        }

        /// <summary>
        /// 处理服务器推送的业务消息
        /// </summary>
        public static void ProcessBusinessMessage(this MainForm mainForm, ReminderData message)
        {
            try
            {
                var messageManager = mainForm.GetMessageManager() as EnhancedMessageManager;
                messageManager?.ProcessBusinessMessage(message);
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，避免影响主程序运行
            }
        }
    }
}