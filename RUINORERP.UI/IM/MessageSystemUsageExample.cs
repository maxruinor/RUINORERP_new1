// 消息系统使用示例 - 展示如何在主窗体中集成消息系统
using System;
using System.Windows.Forms;
using RUINORERP.UI.Controls;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 消息系统使用示例类
    /// 这个类展示了如何在主窗体中集成和使用消息管理器和消息列表控件
    /// </summary>
    public class MessageSystemUsageExample
    {
        private EnhancedMessageManager _messageManager;
        private MessageListControl _messageListControl;
        private ToolStripMenuItem _messageMenuItem;
        private NotifyIcon _notifyIcon;
        private Form _mainForm;

        /// <summary>
        /// 在主窗体中初始化消息系统
        /// </summary>
        /// <param name="mainForm">主窗体实例</param>
        /// <param name="messageManager">消息管理器实例（通常通过依赖注入获取）</param>
        public void InitializeMessageSystem(Form mainForm, EnhancedMessageManager messageManager)
        {
            _mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
            _messageManager = messageManager ?? throw new ArgumentNullException(nameof(messageManager));

            // 1. 创建消息列表控件
            CreateMessageListControl();

            // 2. 创建消息通知区域图标
            CreateNotificationIcon();

            // 3. 创建主菜单中的消息菜单项
            CreateMessageMenuItem();

            // 4. 设置消息系统事件处理
            SetupMessageEventHandlers();

            // 5. 初始加载消息
            LoadInitialMessages();
        }

        /// <summary>
        /// 创建消息列表控件
        /// </summary>
        private void CreateMessageListControl()
        {
            // 创建消息列表控件
            _messageListControl = new MessageListControl(_messageManager);
            _messageListControl.Dock = DockStyle.Fill;
            _messageListControl.Visible = false;
            
            // 添加到主窗体（假设主窗体有一个SplitContainer或Panel可以容纳）
            // 这里需要根据实际的主窗体结构进行调整
            if (_mainForm.Controls.ContainsKey("pnlContent"))
            {
                var contentPanel = _mainForm.Controls["pnlContent"] as Panel;
                if (contentPanel != null)
                {
                    contentPanel.Controls.Add(_messageListControl);
                }
            }
            else
            {
                // 如果没有特定的容器，创建一个标签页
                TabPage messageTab = new TabPage("消息中心");
                messageTab.Controls.Add(_messageListControl);
                
                // 尝试添加到主窗体的TabControl
                if (_mainForm.Controls.ContainsKey("tabMain"))
                {
                    var tabControl = _mainForm.Controls["tabMain"] as TabControl;
                    if (tabControl != null)
                    {
                        tabControl.TabPages.Add(messageTab);
                    }
                }
            }

            // 订阅消息点击事件
            _messageListControl.MessageClicked += MessageListControl_MessageClicked;
        }

        /// <summary>
        /// 创建系统托盘通知图标
        /// </summary>
        private void CreateNotificationIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = _mainForm.Icon,
                Text = "RUINOR ERP系统",
                Visible = true
            };

            // 创建右键菜单
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("查看消息", null, ViewMessages_Click);
            contextMenu.Items.Add("退出", null, ExitApplication_Click);
            
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// 创建主菜单中的消息菜单项
        /// </summary>
        private void CreateMessageMenuItem()
        {
            _messageMenuItem = new ToolStripMenuItem("消息中心");
            _messageMenuItem.Click += MessageMenuItem_Click;
            
            // 添加未读消息计数的子项
            ToolStripMenuItem unreadMenuItem = new ToolStripMenuItem("未读消息: 0");
            unreadMenuItem.Enabled = false;
            _messageMenuItem.DropDownItems.Add(unreadMenuItem);
            
            _messageMenuItem.DropDownItems.Add(new ToolStripSeparator());
            _messageMenuItem.DropDownItems.Add("查看全部消息", null, ViewMessages_Click);
            _messageMenuItem.DropDownItems.Add("全部标记已读", null, MarkAllAsRead_Click);

            // 将菜单项添加到主菜单（需要根据实际主菜单结构调整）
            if (_mainForm.MainMenuStrip != null)
            {
                _mainForm.MainMenuStrip.Items.Add(_messageMenuItem);
            }
        }

        /// <summary>
        /// 设置消息事件处理器
        /// </summary>
        private void SetupMessageEventHandlers()
        {
            // 订阅消息管理器的事件
            _messageManager.NewMessageReceived += MessageManager_NewMessageReceived;
            _messageManager.MessageStatusChanged += MessageManager_MessageStatusChanged;
        }

        /// <summary>
        /// 加载初始消息
        /// </summary>
        private void LoadInitialMessages()
        {
            // 初始加载时可以从服务器获取历史消息
            // 这里只是示例，实际实现需要调用相应的服务方法
            RefreshUnreadCount();
        }

        /// <summary>
        /// 刷新未读消息计数
        /// </summary>
        private void RefreshUnreadCount()
        {
            int unreadCount = _messageManager.GetUnreadMessageCount();
            
            // 更新菜单项文本
            if (_messageMenuItem.DropDownItems.Count > 0 && 
                _messageMenuItem.DropDownItems[0] is ToolStripMenuItem unreadMenuItem)
            {
                unreadMenuItem.Text = $"未读消息: {unreadCount}";
            }

            // 更新主菜单项显示
            _messageMenuItem.Text = unreadCount > 0 ? $"消息中心 ({unreadCount})" : "消息中心";
        }

        /// <summary>
        /// 显示新消息通知
        /// </summary>
        /// <param name="message">新消息</param>
        private void ShowNewMessageNotification(ReminderData message)
        {
            // 显示系统托盘气泡通知
            _notifyIcon.ShowBalloonTip(3000, message.Title ?? "新消息", 
                message.ReminderContent, ToolTipIcon.Info);

            // 可以在这里添加声音播放
            // PlayNotificationSound();

            // 可以添加闪烁任务栏等其他提示方式
            FlashTaskbar();
        }

        /// <summary>
        /// 闪烁任务栏提示
        /// </summary>
        private void FlashTaskbar()
        {
            // 闪烁任务栏提示
            _mainForm.WindowState = FormWindowState.Minimized;
            _mainForm.WindowState = FormWindowState.Normal;
            _mainForm.Activate();
        }

        /// <summary>
        /// 显示消息列表
        /// </summary>
        private void ShowMessageList()
        {
            if (_messageListControl != null)
            {
                _messageListControl.Visible = true;
                _messageListControl.BringToFront();
            }
        }

        #region 事件处理方法

        /// <summary>
        /// 消息管理器 - 新消息接收事件处理
        /// </summary>
        private void MessageManager_NewMessageReceived(object sender, ReminderData message)
        {
            if (_mainForm.InvokeRequired)
            {
                _mainForm.Invoke(new Action<object, ReminderData>(MessageManager_NewMessageReceived), sender, message);
                return;
            }

            RefreshUnreadCount();
            ShowNewMessageNotification(message);
        }

        /// <summary>
        /// 消息管理器 - 消息状态变更事件处理
        /// </summary>
        private void MessageManager_MessageStatusChanged(object sender, ReminderData message)
        {
            if (_mainForm.InvokeRequired)
            {
                _mainForm.Invoke(new Action<object, ReminderData>(MessageManager_MessageStatusChanged), sender, message);
                return;
            }

            RefreshUnreadCount();
        }

        /// <summary>
        /// 消息列表控件 - 消息点击事件处理
        /// </summary>
        private void MessageListControl_MessageClicked(object sender, MessageClickedEventArgs e)
        {
            // 可以在这里添加额外的消息点击处理逻辑
            // 例如记录消息点击日志等
        }

        /// <summary>
        /// 菜单项点击事件 - 显示消息中心
        /// </summary>
        private void MessageMenuItem_Click(object sender, EventArgs e)
        {
            ShowMessageList();
        }

        /// <summary>
        /// 查看消息菜单项点击事件
        /// </summary>
        private void ViewMessages_Click(object sender, EventArgs e)
        {
            _mainForm.WindowState = FormWindowState.Normal;
            _mainForm.Activate();
            ShowMessageList();
        }

        /// <summary>
        /// 全部标记已读菜单项点击事件
        /// </summary>
        private void MarkAllAsRead_Click(object sender, EventArgs e)
        {
            _messageManager.MarkAllAsRead();
            RefreshUnreadCount();
        }

        /// <summary>
        /// 退出应用程序菜单项点击事件
        /// </summary>
        private void ExitApplication_Click(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            _mainForm.Close();
        }

        #endregion

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
            }

            if (_messageListControl != null)
            {
                _messageListControl.MessageClicked -= MessageListControl_MessageClicked;
                _messageListControl.Dispose();
            }

            if (_messageManager != null)
            {
                _messageManager.NewMessageReceived -= MessageManager_NewMessageReceived;
                _messageManager.MessageStatusChanged -= MessageManager_MessageStatusChanged;
            }
        }
    }

    // 使用说明：
    // 1. 在主窗体的Load事件中初始化消息系统：
    //    private MessageSystemUsageExample _messageSystem;
    //    
    //    private void MainForm_Load(object sender, EventArgs e)
    //    {
    //        // 假设通过依赖注入获取EnhancedMessageManager实例
    //        var messageManager = DependencyResolver.Current.GetService<EnhancedMessageManager>();
    //        _messageSystem = new MessageSystemUsageExample();
    //        _messageSystem.InitializeMessageSystem(this, messageManager);
    //    }
    //
    // 2. 在主窗体的FormClosing事件中释放资源：
    //    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    //    {
    //        _messageSystem?.Dispose();
    //    }
    //
    // 3. 确保在应用程序启动时注册EnhancedMessageManager到依赖注入容器
}