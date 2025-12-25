using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 消息显示组件基类
    /// 提供统一的消息显示和处理功能
    /// </summary>
    public abstract class BaseMessagePrompt : KryptonForm
    {
        /// <summary>
        /// 消息数据
        /// </summary>
        public MessageData MessageData { get; set; }
        
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected readonly ILogger<BaseMessagePrompt> Logger;
        
        /// <summary>
        /// 消息管理器
        /// </summary>
        protected readonly EnhancedMessageManager MessageManager;
        
        /// <summary>
        /// 菜单权限助手
        /// </summary>
        protected MenuPowerHelper MenuPowerHelper;
        
        /// <summary>
        /// 初始化组件
        /// 子类必须实现此方法来初始化UI组件
        /// </summary>
        protected abstract void InitializeComponents();
        
        /// <summary>
        /// 更新消息显示
        /// 子类必须实现此方法来根据消息数据更新UI
        /// </summary>
        protected abstract void UpdateMessageDisplay();
        
        /// <summary>
        /// 显示消息
        /// 通用的消息显示方法
        /// </summary>
        /// <param name="messageData">消息数据</param>
        public virtual void ShowMessage(MessageData messageData)
        {
            try
            {
                MessageData = messageData;
                
                // 更新消息为已读状态
                if (!messageData.IsRead)
                {
                    messageData.MarkAsRead();
                }
                
                UpdateMessageDisplay();
                
                // 设置窗口位置和显示
                PositionForm();
                ShowMessageForm();
                
                Logger.LogDebug($"显示消息: {messageData.Title}, 类型: {messageData.MessageType}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "显示消息时发生错误");
            }
        }
        
        /// <summary>
        /// 设置发送者文本
        /// </summary>
        /// <param name="text">发送者名称</param>
        public abstract void SetSenderText(string text);
        
        /// <summary>
        /// 设置主题文本
        /// </summary>
        /// <param name="text">消息主题</param>
        public abstract void SetSubjectText(string text);
        
        /// <summary>
        /// 定位窗体
        /// 默认定位到屏幕右下角
        /// 子类可以重写此方法自定义位置
        /// </summary>
        protected virtual void PositionForm()
        {
            this.StartPosition = FormStartPosition.Manual;
            
            // 设置窗体的初始位置为屏幕右下角
            this.SetDesktopLocation(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );
        }
        
        /// <summary>
        /// 显示消息窗体
        /// 子类可以重写此方法自定义显示逻辑
        /// </summary>
        protected virtual void ShowMessageForm()
        {
            // 如果已经显示，则闪烁窗口获取用户注意
            if (this.Visible)
            {
                this.Focus();
                this.BringToFront();
            }
            else
            {
                this.Show();
                this.BringToFront();
            }
        }
        
        /// <summary>
        /// 设置统一的视觉样式
        /// </summary>
        protected void SetUnifiedVisualStyle()
        {
            // 设置基础样式
            this.Font = new Font("微软雅黑", 9);
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            
            // 设置窗口大小
            this.ClientSize = new Size(400, 280);
            
            // 设置窗口标题
            this.Text = "系统消息";
        }
        

        
        /// <summary>
        /// 处理确认操作
        /// </summary>
        /// <param name="status">确认状态</param>
        protected virtual async Task HandleConfirmationAsync(ConfirmStatus status)
        {
            try
            {
                if (MessageData != null)
                {
                    MessageData.ConfirmStatus = status;
                    MessageData.ConfirmTime = DateTime.Now;
                    
                    // 使用消息管理器标记消息状态变更
                    if (MessageManager != null)
                    {
                        // 这里只是更新内存中的消息状态，实际的数据持久化应该在MessageManager中处理
                        await Task.Run(() =>
                        {
                            // 标记为已读
                            MessageManager.MarkAsRead(MessageData.MessageId);
                            Logger.LogDebug($"已更新消息状态: {MessageData.MessageId}, 确认状态: {status}");
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "处理消息确认时发生错误");
            }
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseMessagePrompt()
        {
            // 获取服务实例
            MenuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            Logger = Startup.GetFromFac<ILogger<BaseMessagePrompt>>();
            MessageManager = Startup.GetFromFac<EnhancedMessageManager>();
            
            // 初始化组件
            InitializeComponents();
            
            // 设置统一的视觉样式
            SetUnifiedVisualStyle();
        }
        
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="messageManager">消息管理器</param>
        protected BaseMessagePrompt(MessageData messageData, ILogger<BaseMessagePrompt> logger = null, EnhancedMessageManager messageManager = null)
            : this()
        {
            MessageData = messageData;
            Logger = logger ?? Logger;
            MessageManager = messageManager ?? MessageManager;
            
            // 如果提供了消息数据，则更新显示
            if (messageData != null)
            {
                UpdateMessageDisplay();
            }
        }
        
        /// <summary>
        /// 生成稍候提醒的指令
        /// </summary>
        /// <param name="kryptonContextMenu1">上下文菜单</param>
        protected void AddCommandForWait(KryptonContextMenu kryptonContextMenu1)
        {
            KryptonContextMenuItems contextMenuItems = new KryptonContextMenuItems();
            KryptonContextMenuItem menuItem5分钟后 = new KryptonContextMenuItem();
            KryptonCommand command5分钟后 = new KryptonCommand();
            command5分钟后.Execute += kryptonCommandWait_Execute;
            menuItem5分钟后.KryptonCommand = command5分钟后;
            menuItem5分钟后.Text = "五分钟后";
            command5分钟后.Text = menuItem5分钟后.Text;

            KryptonContextMenuItem menuItem10分钟后 = new KryptonContextMenuItem();
            KryptonCommand command十分钟后 = new KryptonCommand();
            command十分钟后.Execute += kryptonCommandWait_Execute;
            menuItem10分钟后.KryptonCommand = command十分钟后;
            menuItem10分钟后.Text = "十分钟后";
            command十分钟后.Text = menuItem10分钟后.Text;

            KryptonContextMenuItem menuItem一小时后 = new KryptonContextMenuItem();
            KryptonCommand command一小时后 = new KryptonCommand();
            command一小时后.Execute += kryptonCommandWait_Execute;
            menuItem一小时后.KryptonCommand = command一小时后;
            menuItem一小时后.Text = "一小时后";
            command一小时后.Text = menuItem一小时后.Text;

            KryptonContextMenuItem menuItem一天后 = new KryptonContextMenuItem();
            KryptonCommand command一天后 = new KryptonCommand();
            command一天后.Execute += kryptonCommandWait_Execute;
            menuItem一天后.KryptonCommand = command一天后;
            menuItem一天后.Text = "一天后";
            command一天后.Text = menuItem一天后.Text;

            kryptonContextMenu1.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            contextMenuItems});

            contextMenuItems.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            menuItem5分钟后,
            menuItem10分钟后,
            menuItem一小时后,
            menuItem一天后
            });
            if (kryptonContextMenu1.Items.Count == 0)
            {
                kryptonContextMenu1.Items.Add(contextMenuItems);
            }
        }
        
        /// <summary>
        /// 稍候提醒命令执行事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        protected virtual void kryptonCommandWait_Execute(object sender, EventArgs e)
        {
            WaitReminder(sender);
        }
        
        /// <summary>
        /// 稍候提醒处理
        /// </summary>
        /// <param name="sender">发送者</param>
        protected virtual void WaitReminder(object sender)
        {
            int interval = 60;
            if (sender is KryptonDropButton dropButton)
            {
                //默认5分钟
                interval = 60 * 5;
            }
            else if (sender is KryptonCommand command)
            {
                switch (command.Text)
                {
                    case "五分钟后":
                        interval = 300;
                        break;
                    case "十分钟后":
                        interval = 600;
                        break;
                    case "一小时后":
                        interval = 3600;
                        break;
                    case "一天后":
                        interval = 3600 * 24;
                        break;
                    default:
                        break;
                }
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}