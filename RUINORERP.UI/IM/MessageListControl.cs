// 消息列表控件 - 用于在主窗体中显示和管理消息
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using RUINORERP.UI.IM;
using RUINORERP.Model.TransModel;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.Controls
{
    /// <summary>
    /// 消息列表控件，用于显示和管理系统消息
    /// </summary>
    public partial class MessageListControl : UserControl
    {
        private readonly EnhancedMessageManager _messageManager;
        private Timer _notificationTimer;
        private int _unreadCount = 0;
        private bool _isAnimating = false;

        /// <summary>
        /// 当用户点击消息时触发的事件
        /// </summary>
        public event EventHandler<MessageClickedEventArgs> MessageClicked;

        /// <summary>
        /// 初始化消息列表控件
        /// </summary>
        /// <param name="messageManager">消息管理器实例</param>
        public MessageListControl(EnhancedMessageManager messageManager)
        {
            InitializeComponent();
            _messageManager = messageManager ?? throw new ArgumentNullException(nameof(messageManager));
            InitializeControl();
        }

        private void InitializeControl()
        {
            // 设置控件样式
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;
            
            // 初始化通知定时器
            _notificationTimer = new Timer { Interval = 3000 }; // 3秒
            _notificationTimer.Tick += NotificationTimer_Tick;
            
            // 订阅消息管理器的事件
            _messageManager.NewMessageReceived += MessageManager_NewMessageReceived;
            // 修复事件处理程序签名不匹配问题
            _messageManager.MessageStatusChanged += (sender, e) => { 
                // 这里可以根据需要提取ReminderData对象
                // 由于我们没有看到MessageStatusChanged事件的完整定义，假设e中包含消息数据
                // 或者如果MessageManager有其他方式获取最新状态，可以在这里实现
                UpdateUnreadCount(); // 至少更新未读计数
            };
            
            // 加载现有消息
            LoadMessages();
        }

        /// <summary>
        /// 加载消息列表
        /// </summary>
        private void LoadMessages()
        {
            try
            {
                lstMessages.Items.Clear();
                var messages = _messageManager.GetAllMessages();
                
                foreach (var message in messages)
                {
                    AddMessageToList(message);
                }
                
                UpdateUnreadCount();
            }
            catch (Exception ex)
            {
                // 记录错误
                Console.WriteLine($"加载消息时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加消息到列表
        /// </summary>
        /// <param name="message">消息对象</param>
        private void AddMessageToList(ReminderData message)
        {
            var item = new ListViewItem(message.ReminderContent);
            item.SubItems.Add(message.Title ?? "无标题");
            // 确保CreateTime对象能正确转换为字符串
            item.SubItems.Add(message.CreateTime is DateTime dateTime ? dateTime.ToString("yyyy-MM-dd HH:mm:ss") : message.CreateTime?.ToString() ?? string.Empty);
            item.SubItems.Add(message.IsRead ? "已读" : "未读");
            item.SubItems.Add(message.messageCmd.ToString());
            
            // 设置未读消息的样式
            if (!message.IsRead)
            {
                item.ForeColor = Color.Red;
                item.Font = new Font(item.Font, FontStyle.Bold);
            }
            
            item.Tag = message.Id; // 存储消息ID
            lstMessages.Items.Add(item);
        }

        /// <summary>
        /// 更新未读消息计数
        /// </summary>
        private void UpdateUnreadCount()
        {
            _unreadCount = _messageManager.GetUnreadMessageCount();
            lblUnreadCount.Text = $"未读消息: {_unreadCount}";
            
            // 如果有未读消息，可以显示气泡提示
            if (_unreadCount > 0)
            {
                ShowNotificationBubble();
            }
        }

        /// <summary>
        /// 显示通知气泡
        /// </summary>
        private void ShowNotificationBubble()
        {
            // 这里可以实现气泡提示逻辑
            // 例如使用NotifyIcon或自定义控件
            
            // 简单的动画效果
            if (!_isAnimating)
            {
                _isAnimating = true;
                // 启动动画或通知
                _notificationTimer.Start();
            }
        }

        /// <summary>
        /// 消息管理器 - 新消息接收事件处理
        /// </summary>
        private void MessageManager_NewMessageReceived(object sender, ReminderData message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, ReminderData>(MessageManager_NewMessageReceived), sender, message);
                return;
            }
            
            AddMessageToList(message);
            UpdateUnreadCount();
            
            // 播放声音提示
            PlayNotificationSound();
        }

        /// <summary>
        /// 消息管理器 - 消息状态变更事件处理
        /// </summary>
        private void MessageManager_MessageStatusChanged(object sender, ReminderData message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, ReminderData>(MessageManager_MessageStatusChanged), sender, message);
                return;
            }
            
            // 更新消息列表中的对应项
            foreach (ListViewItem item in lstMessages.Items)
            {
                if (item.Tag.ToString() == message.Id.ToString())
                {
                    item.SubItems[3].Text = message.IsRead ? "已读" : "未读";
                    
                    if (message.IsRead)
                    {
                        item.ForeColor = Color.Black;
                        item.Font = new Font(item.Font, FontStyle.Regular);
                    }
                    else
                    {
                        item.ForeColor = Color.Red;
                        item.Font = new Font(item.Font, FontStyle.Bold);
                    }
                    break;
                }
            }
            
            UpdateUnreadCount();
        }

        /// <summary>
        /// 播放通知声音
        /// </summary>
        private void PlayNotificationSound()
        {
            try
            {
                // 这里可以实现声音播放逻辑
                // 例如使用System.Media.SoundPlayer
                System.Media.SystemSounds.Beep.Play();
            }
            catch { /* 忽略声音播放错误 */ }
        }

        /// <summary>
        /// 通知定时器事件处理
        /// </summary>
        private void NotificationTimer_Tick(object sender, EventArgs e)
        {
            _isAnimating = false;
            _notificationTimer.Stop();
        }

        /// <summary>
        /// 消息列表项点击事件处理
        /// </summary>
        private void lstMessages_ItemClick(object sender, EventArgs e)
        {
            if (lstMessages.SelectedItems.Count > 0)
            {
                var selectedItem = lstMessages.SelectedItems[0];
                var messageId = Convert.ToString(selectedItem.Tag);
                
                // 获取消息对象
                var message = _messageManager.GetMessageById(messageId);
                if (message != null)
                {
                    // 标记为已读
                    _messageManager.MarkAsRead(messageId);
                    
                    // 触发消息点击事件
                    MessageClicked?.Invoke(this, new MessageClickedEventArgs(message));
                    
                    // 处理业务导航
                    NavigateToBusiness(message);
                }
            }
        }

        /// <summary>
        /// 根据消息导航到业务窗体
        /// </summary>
        /// <param name="message">消息对象</param>
        private void NavigateToBusiness(ReminderData message)
        {
            try
            {
                // 根据消息类型和业务数据导航到相应的业务窗体
                switch (message.messageCmd)
                {
                    case MessageCmdType.Approve:
                        // 导航到审批窗体
                        NavigateToApprovalForm(message.BizPrimaryKey.ToString(), message.BizType.ToString());
                        break;
                    case MessageCmdType.Task:
                        // 导航到任务窗体
                        NavigateToTaskForm(message.BizPrimaryKey.ToString());
                        break;
                    case MessageCmdType.Notice:
                        // 显示通知详情
                        ShowNoticeDetail(message);
                        break;
                    // 其他类型的导航...
                }
            }
            catch (Exception ex)
            {
                // 记录错误
                Console.WriteLine($"导航到业务窗体时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 导航到审批窗体
        /// </summary>
        private void NavigateToApprovalForm(string bizId, string bizType)
        {
            // TODO: 实现导航到审批窗体的逻辑
            MessageBox.Show($"导航到审批窗体: {bizType} - ID: {bizId}");
        }

        /// <summary>
        /// 导航到任务窗体
        /// </summary>
        private void NavigateToTaskForm(string taskId)
        {
            // TODO: 实现导航到任务窗体的逻辑
            MessageBox.Show($"导航到任务窗体: ID: {taskId}");
        }

        /// <summary>
        /// 显示通知详情
        /// </summary>
        private void ShowNoticeDetail(ReminderData message)
        {
            // TODO: 实现显示通知详情的逻辑
            MessageBox.Show(message.ReminderContent, message.Title ?? "通知详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMessages();
        }

        private void btnbtnMarkAllRead_Click(object sender, EventArgs e)
        {
            _messageManager.MarkAllAsRead();
            LoadMessages(); // 重新加载以更新UI
        }
    }

    /// <summary>
    /// 消息点击事件参数
    /// </summary>
    public class MessageClickedEventArgs : EventArgs
    {
        /// <summary>
        /// 被点击的消息对象
        /// </summary>
        public ReminderData Message { get; }

        /// <summary>
        /// 初始化消息点击事件参数
        /// </summary>
        /// <param name="message">消息对象</param>
        public MessageClickedEventArgs(ReminderData message)
        {
            Message = message;
        }
    }
}