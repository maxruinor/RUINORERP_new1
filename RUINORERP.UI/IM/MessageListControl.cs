// 消息列表控件 - 用于在主窗体中显示和管理消息
using RUINORERP.Business.BizMapperService;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.UI.Common;
using RUINORERP.UI.IM;
using RUINORERP.UI.UserCenter;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.IM
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
        private NotifyIcon _bubbleNotifyIcon;
        private Timer _bubbleTimer;
        private bool _isBubbleVisible = false;

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

            // 初始化气泡通知
            InitializeBubbleNotification();

            // 订阅消息管理器的事件
            _messageManager.MessageStatusChanged += MessageManager_MessageStatusChanged;
            _messageManager.UnreadMessageCountChanged += MessageManager_UnreadMessageCountChanged;

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
                _unreadCount = 0;

                // 从消息管理器获取所有消息
                var messages = _messageManager.GetAllMessages();

                // 添加消息到列表
                foreach (var message in messages)
                {
                    AddMessageToList(message);
                }

                UpdateUnreadCount();
            }
            catch (Exception ex)
            {
                // 记录错误
                System.Diagnostics.Debug.WriteLine($"加载消息时发生错误: {ex.Message}");
                MessageBox.Show("加载消息失败，请稍后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加消息到列表
        /// </summary>
        /// <param name="message">消息对象</param>
        private void AddMessageToList(RUINORERP.Model.TransModel.MessageData message)
        {
            var item = new ListViewItem(message.Content);
            item.SubItems.Add(message.Title ?? "无标题");
            // 使用SendTime作为消息时间
            item.SubItems.Add(message.SendTime.ToString("yyyy-MM-dd HH:mm:ss"));
            item.SubItems.Add(message.IsRead ? "已读" : "未读");
            item.SubItems.Add(message.MessageType.ToString());

            // 设置未读消息的样式
            if (!message.IsRead)
            {
                item.ForeColor = Color.Red;
                item.Font = new Font(item.Font, FontStyle.Bold);
            }

            item.Tag = message.MessageId; // 存储消息ID
            lstMessages.Items.Add(item);
        }

        /// <summary>
        /// 更新未读消息计数
        /// </summary>
        private void UpdateUnreadCount()
        {
            _unreadCount = _messageManager.GetUnreadMessageCount();
            lblUnreadCount.Text = $"未读消息: {_unreadCount}";
        }

        /// <summary>
        /// 显示消息通知
        /// </summary>
        /// <param name="message">消息对象</param>
        private void ShowNotification(RUINORERP.Model.TransModel.MessageData message)
        {
            // TODO: 实现更复杂的通知逻辑
            // 这里仅作为示例
            var notification = new NotifyIcon
            {
                Icon = SystemIcons.Information,
                Visible = true,
                BalloonTipTitle = message.Title ?? "新消息",
                BalloonTipText = message.Content,
                BalloonTipIcon = ToolTipIcon.Info
            };

            // 显示通知
            notification.ShowBalloonTip(3000);

            // 点击通知事件
            notification.MouseClick += (s, e) =>
            {
                // 显示消息详情
                ShowNoticeDetail(message);
                notification.Visible = false;
            };

            // 定时器，用于隐藏通知
            Timer timer = new Timer { Interval = 5000 };
            timer.Tick += (s, e) =>
            {
                notification.Visible = false;
                timer.Dispose();
            };
            timer.Start();
        }

        /// <summary>
        /// 消息管理器 - 未读消息数量变化事件处理
        /// </summary>
        private void MessageManager_UnreadMessageCountChanged(object sender, int count)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, int>(MessageManager_UnreadMessageCountChanged), sender, count);
                return;
            }

            // 处理未读消息数量变化
            int oldCount = _unreadCount;
            UpdateUnreadCount();

            // 未读消息数量增加时刷新消息列表和显示动态提示
            if (count > oldCount)
            {
                // 播放声音提示
                PlayNotificationSound();

                // 重新加载消息列表以获取新消息
                LoadMessages();

                // 启动动画通知
                StartNotificationAnimation();

                // 显示气泡提示
                ShowBubbleNotification(count);

                // 注意：自动弹窗功能由EnhancedMessageManager负责处理
                // 这里只更新列表UI，不显示弹窗
            }
        }



        /// <summary>
        /// 启动通知动画
        /// </summary>
        private void StartNotificationAnimation()
        {
            _isAnimating = true;
            _notificationTimer.Start();
        }

        /// <summary>
        /// 消息管理器 - 消息状态变更事件处理
        /// </summary>
        private void MessageManager_MessageStatusChanged(object sender, RUINORERP.Model.TransModel.MessageData message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, RUINORERP.Model.TransModel.MessageData>(MessageManager_MessageStatusChanged), sender, message);
                return;
            }

            // 如果消息为null，通常表示消息已被删除
            if (message == null)
            {
                LoadMessages(); // 重新加载整个列表以更新UI
            }
            else
            {
                // 检查是否为新增消息（消息ID不在当前列表中）
                bool isNewMessage = true;
                foreach (ListViewItem item in lstMessages.Items)
                {
                    if (item.Tag != null && item.Tag.ToString() == message.MessageId.ToString())
                    {
                        isNewMessage = false;
                        // 更新现有消息项
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

                // 如果是新消息，重新加载整个列表以显示新消息
                if (isNewMessage)
                {
                    LoadMessages();
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
        /// 初始化气泡通知
        /// </summary>
        private void InitializeBubbleNotification()
        {
            // 创建气泡通知图标
            _bubbleNotifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Information,
                Visible = false,
                BalloonTipIcon = ToolTipIcon.Info
            };

            // 气泡点击事件
            _bubbleNotifyIcon.BalloonTipClicked += (s, e) =>
            {
                // 点击气泡时显示消息中心
                ShowMessageCenterTab();
                HideBubbleNotification();
            };

            // 气泡定时器
            _bubbleTimer = new Timer { Interval = 5000 }; // 5秒后自动隐藏
            _bubbleTimer.Tick += (s, e) =>
            {
                HideBubbleNotification();
            };
        }

        /// <summary>
        /// 显示气泡通知
        /// </summary>
        /// <param name="unreadCount">未读消息数量</param>
        private void ShowBubbleNotification(int unreadCount)
        {
            if (_isBubbleVisible) return;

            try
            {
                _bubbleNotifyIcon.BalloonTipTitle = "新消息通知";
                _bubbleNotifyIcon.BalloonTipText = $"您有{unreadCount}条未读消息，点击查看详情";
                _bubbleNotifyIcon.Visible = true;
                _bubbleNotifyIcon.ShowBalloonTip(3000); // 显示3秒

                _isBubbleVisible = true;
                _bubbleTimer.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示气泡通知时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 隐藏气泡通知
        /// </summary>
        private void HideBubbleNotification()
        {
            if (!_isBubbleVisible) return;

            try
            {
                _bubbleTimer.Stop();
                _bubbleNotifyIcon.Visible = false;
                _isBubbleVisible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"隐藏气泡通知时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示消息中心标签页
        /// </summary>
        private void ShowMessageCenterTab()
        {
            try
            {
                // 查找主窗体中的消息中心标签页并激活
                var mainForm = this.FindForm() as MainForm;
                if (mainForm != null)
                {
                    // 通过反射或其他方式激活消息中心标签页
                    // 这里需要根据实际的MainForm实现来调用
                    mainForm.Invoke(new Action(() =>
                    {
                        // 激活消息中心标签页的逻辑
                        // 具体实现依赖于MainForm的标签页管理方式
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示消息中心标签页时发生错误: {ex.Message}");
            }
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
                if (selectedItem.Tag is long messageId)
                {
                    // 获取消息对象
                    var message = _messageManager.GetMessageById(messageId);
                    if (message != null)
                    {
                        // 标记为已读
                        _messageManager.MarkAsRead(messageId);

                        // 触发消息点击事件
                        MessageClicked?.Invoke(this, new MessageClickedEventArgs(message));

                        // 使用MessagePrompt显示消息内容
                        ShowMessageWithPrompt(message);

                        // 处理业务导航
                        NavigateToBusiness(message);
                    }
                }
            }
        }

        /// <summary>
        /// 使用MessagePrompt显示消息内容
        /// </summary>
        /// <param name="message">消息对象</param>
        private void ShowMessageWithPrompt(MessageData message)
        {
            try
            {
                if (message == null) return;

                // 使用依赖注入获取MessagePrompt实例
                var messagePrompt = Startup.GetFromFac<MessagePrompt>();
                if (messagePrompt != null)
                {
                    // 设置MessageData属性以触发消息显示更新
                    messagePrompt.MessageData = message;
                    if (message.BizData is TodoUpdate todoUpdate)
                    {

                        message.Content = $"【{todoUpdate.BillNo}】-" + message.Content;
                    }

                    var EntityInfo = EntityMappingHelper.GetEntityInfo(message.BizType);
                    // 显示消息提示窗口
                    if (Application.OpenForms.Count > 0)
                    {
                        Application.OpenForms[0].Invoke(new Action(() =>
                        {
                            // 确保窗体使用默认大小显示
                            messagePrompt.StartPosition = FormStartPosition.CenterScreen;
                            // 强制更新消息显示
                            messagePrompt.UpdateMessageDisplay();
                            messagePrompt.ShowDialog();
                        }));
                    }
                }
                else
                {
                    // 如果无法从依赖注入获取，创建新实例
                    var prompt = new MessagePrompt(message, _messageManager);
                    if (Application.OpenForms.Count > 0)
                    {
                        Application.OpenForms[0].Invoke(new Action(() =>
                        {
                            // 确保窗体使用默认大小显示
                            prompt.StartPosition = FormStartPosition.CenterScreen;
                            // 强制更新消息显示
                            prompt.UpdateMessageDisplay();
                            prompt.ShowDialog();
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用MessagePrompt显示消息时发生错误: {ex.Message}");
                MessageBox.Show($"显示消息详情时发生错误: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据消息导航到业务窗体
        /// </summary>
        /// <param name="message">消息对象</param>
        private async Task NavigateToBusiness(RUINORERP.Model.TransModel.MessageData message)
        {
            try
            {
                // 根据消息类型和业务数据导航到相应的业务窗体
                switch (message.MessageType)
                {
                    case RUINORERP.Model.TransModel.MessageType.Approve:
                        // 导航到审批窗体
                        await NavigateToApprovalForm(message.BizId.ToString(), message.BizType);
                        break;
                    case RUINORERP.Model.TransModel.MessageType.Task:
                        // 导航到任务窗体
                        await NavigateToTaskForm(message.BizId.ToString());
                        break;
                    case RUINORERP.Model.TransModel.MessageType.Notice:
                        // 显示通知详情
                        ShowNoticeDetail(message);
                        break;
                    case RUINORERP.Model.TransModel.MessageType.Reminder:
                        // 导航到工作流提醒相关的业务窗体
                        await NavigateToWorkflowReminderForm(message);
                        break;
                        // 其他类型的导航...
                }
            }
            catch (Exception ex)
            {
                // 记录错误
                System.Diagnostics.Debug.WriteLine($"导航到业务窗体时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 导航到审批窗体
        /// </summary>
        private async Task NavigateToApprovalForm(string bizId, BizType bizType)
        {
            try
            {
                if (string.IsNullOrEmpty(bizId))
                {
                    MessageBox.Show("业务ID或业务类型为空，无法导航到审批表单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 转换业务ID为long类型
                if (!long.TryParse(bizId, out long businessId))
                {
                    MessageBox.Show("业务ID格式不正确，无法导航到审批表单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 查找对应的菜单信息
                var menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                var menuInfo = menuPowerHelper.MenuList.FirstOrDefault(m => m.EntityName.Equals(bizType.ToString(), StringComparison.OrdinalIgnoreCase));

                if (menuInfo == null)
                {
                    MessageBox.Show($"未找到业务类型 '{bizType}' 对应的审批表单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 获取实体类型
                var tableType = EntityMappingHelper.GetEntityType(bizType);
                if (tableType == null)
                {
                    MessageBox.Show($"未找到业务类型 '{bizType}' 对应的实体类型", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 创建查询参数
                var queryParameter = new QueryParameter
                {
                    conditionals = new List<IConditionalModel> {
                        new ConditionalModel {
                            FieldName = "ID",
                            ConditionalType = ConditionalType.Equal,
                            FieldValue = businessId.ToString(),
                            CSharpTypeName = "long"
                        }
                    },
                    tableType = tableType
                };

                // 执行导航操作
                var instance = Activator.CreateInstance(tableType);
                await menuPowerHelper.ExecuteEvents(menuInfo, instance, queryParameter);

                // 标记相关消息为已读
                MarkRelatedMessagesAsRead(businessId, bizType);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导航到审批表单时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"导航到审批表单失败: {ex.ToString()}");
            }
        }

        /// <summary>
        /// 根据业务ID和类型标记相关消息为已读
        /// </summary>
        /// <param name="bizId">业务ID</param>
        /// <param name="bizType">业务类型</param>
        private void MarkRelatedMessagesAsRead(long bizId, BizType bizType)
        {
            try
            {
                var messages = _messageManager.GetAllMessages();
                var relatedMessages = messages.Where(m => m.BizId == bizId && m.BizType == bizType).ToList();

                foreach (var message in relatedMessages)
                {
                    if (!message.IsRead)
                    {
                        _messageManager.MarkAsRead(message.MessageId);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"标记相关消息为已读失败: {ex.ToString()}");
            }
        }

        /// <summary>
        /// 导航到任务窗体
        /// </summary>
        private async Task NavigateToTaskForm(string taskId)
        {
            try
            {
                if (string.IsNullOrEmpty(taskId))
                {
                    MessageBox.Show("任务ID为空，无法导航到任务表单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 转换任务ID为long类型
                if (!long.TryParse(taskId, out long businessId))
                {
                    MessageBox.Show("任务ID格式不正确，无法导航到任务表单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 任务相关的业务类型通常是"tb_Task"或类似命名
                string taskBizType = "tb_Task";

                // 查找对应的菜单信息
                var menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                var menuInfo = menuPowerHelper.MenuList.FirstOrDefault(m =>
                    m.EntityName.Equals(taskBizType, StringComparison.OrdinalIgnoreCase) ||
                    m.MenuName.Contains("任务"));

                if (menuInfo == null)
                {
                    // 如果没找到，尝试使用通用的任务处理表单
                    menuInfo = menuPowerHelper.MenuList.FirstOrDefault(m =>
                        m.BIBaseForm.Contains("Task"));
                }

                if (menuInfo == null)
                {
                    MessageBox.Show("未找到对应的任务处理表单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //// 获取实体类型
                //var tableType = EntityMappingHelper.GetEntityType(taskBizType);
                //if (tableType == null)
                //{
                //    // 如果没找到，尝试使用默认的任务实体类型
                //    tableType = Type.GetType($"RUINORERP.Model.{taskBizType}");
                //}

                //if (tableType == null)
                //{
                //    MessageBox.Show("未找到任务对应的实体类型", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                //// 创建查询参数
                //var queryParameter = new QueryParameter
                //{
                //    conditionals = new List<IConditionalModel> {
                //        new ConditionalModel {
                //            FieldName = "ID",
                //            ConditionalType = ConditionalType.Equal,
                //            FieldValue = businessId.ToString(),
                //            CSharpTypeName = "long"
                //        }
                //    },
                //    tableType = tableType
                //};

                //// 执行导航操作
                //var instance = Activator.CreateInstance(tableType);
                //await menuPowerHelper.ExecuteEvents(menuInfo, instance, queryParameter);

                //// 标记相关消息为已读
                //MarkRelatedMessagesAsRead(businessId, taskBizType);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导航到任务表单时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"导航到任务表单失败: {ex.ToString()}");
            }
        }

        /// <summary>
        /// 导航到工作流提醒相关的业务窗体
        /// </summary>
        /// <param name="message">工作流提醒消息</param>
        private async Task NavigateToWorkflowReminderForm(RUINORERP.Model.TransModel.MessageData message)
        {
            try
            {
                if (message == null)
                {
                    MessageBox.Show("消息为空，无法导航到工作流提醒表单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 工作流提醒消息通常包含业务ID和业务类型
                if (message.BizId > 0 && !string.IsNullOrEmpty(message.BizType.ToString()))
                {
                    // 尝试导航到对应的业务窗体
                    await NavigateToApprovalForm(message.BizId.ToString(), message.BizType);
                }
                else
                {
                    // 如果没有具体的业务信息，显示消息详情
                    ShowNoticeDetail(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导航到工作流提醒表单时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"导航到工作流提醒表单失败: {ex.ToString()}");
            }
        }

        /// <summary>
        /// 显示通知详情
        /// </summary>
        private void ShowNoticeDetail(RUINORERP.Model.TransModel.MessageData message)
        {
            try
            {
                // 创建一个详细的消息详情窗口
                var detailForm = new Form
                {
                    Text = message.Title ?? "通知详情",
                    Size = new Size(600, 400),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    BackColor = Color.White,
                    Font = new Font("微软雅黑", 9)
                };

                // 创建表格布局面板
                var tableLayoutPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 7,
                    Padding = new Padding(10, 10, 10, 10),
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
                };

                // 设置列和行样式
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                for (int i = 0; i < 6; i++)
                {
                    tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                }
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                // 添加标题和内容
                AddDetailRow(tableLayoutPanel, 0, "发送者:", message.SenderName ?? "系统");
                AddDetailRow(tableLayoutPanel, 1, "消息类型:", message.MessageType.ToString());
                AddDetailRow(tableLayoutPanel, 2, "业务类型:", message.BizType.ToString());
                AddDetailRow(tableLayoutPanel, 3, "业务ID:", message.BizId.ToString());
                AddDetailRow(tableLayoutPanel, 4, "发送时间:", message.SendTime.ToString("yyyy-MM-dd HH:mm:ss"));
                AddDetailRow(tableLayoutPanel, 5, "状态:", message.IsRead ? "已读" : "未读");

                // 添加内容标签
                var contentLabel = new Label
                {
                    Text = "内容:",
                    TextAlign = ContentAlignment.TopRight,
                    Dock = DockStyle.Fill,
                    Font = new Font("微软雅黑", 9, FontStyle.Bold),
                    Padding = new Padding(5, 5, 5, 5)
                };
                tableLayoutPanel.Controls.Add(contentLabel, 0, 6);

                // 添加内容文本框
                var contentTextBox = new TextBox
                {
                    Text = message.Content,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    Font = new Font("微软雅黑", 9),
                    Padding = new Padding(5, 5, 5, 5)
                };
                tableLayoutPanel.Controls.Add(contentTextBox, 1, 6);

                // 添加确定按钮
                var btnOk = new Button
                {
                    Text = "确定",
                    DialogResult = DialogResult.OK,
                    Width = 80,
                    Height = 30,
                    BackColor = Color.FromArgb(64, 158, 255),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("微软雅黑", 9, FontStyle.Bold)
                };
                btnOk.FlatAppearance.BorderSize = 0;
                btnOk.Click += (sender, e) => detailForm.Close();

                // 创建底部按钮面板
                var buttonPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    BackColor = Color.White
                };
                btnOk.Location = new Point(buttonPanel.Width - 90, 10);
                buttonPanel.Controls.Add(btnOk);

                // 添加控件到窗体
                detailForm.Controls.Add(tableLayoutPanel);
                detailForm.Controls.Add(buttonPanel);

                // 添加事件处理，确保按钮位置正确
                detailForm.Resize += (sender, e) =>
                {
                    btnOk.Location = new Point(detailForm.ClientSize.Width - 100, buttonPanel.Height - 40);
                };

                // 显示对话框
                detailForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示消息详情时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"显示消息详情失败: {ex.ToString()}");
            }
        }

        /// <summary>
        /// 向详情窗口添加一行
        /// </summary>
        /// <param name="panel">表格布局面板</param>
        /// <param name="row">行索引</param>
        /// <param name="labelText">标签文本</param>
        /// <param name="valueText">值文本</param>
        private void AddDetailRow(TableLayoutPanel panel, int row, string labelText, string valueText)
        {
            // 添加标签
            var label = new Label
            {
                Text = labelText,
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                Font = new Font("微软雅黑", 9, FontStyle.Bold),
                Padding = new Padding(5, 5, 5, 5)
            };
            panel.Controls.Add(label, 0, row);

            // 添加值
            var value = new Label
            {
                Text = valueText,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("微软雅黑", 9),
                Padding = new Padding(5, 5, 5, 5),
                AutoEllipsis = true
            };
            panel.Controls.Add(value, 1, row);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMessages();
        }

        private void btnbtnMarkAllRead_Click(object sender, EventArgs e)
        {
            // 标记所有消息为已读
            _messageManager.MarkAllAsRead();
            LoadMessages(); // 重新加载以更新UI
        }


        #region 清空消息功能

        /// <summary>
        /// 清除30天前的消息
        /// </summary>
        private void menuClear30Days_Click(object sender, EventArgs e)
        {
            ClearMessagesByDays(30);
        }

        /// <summary>
        /// 清除60天前的消息
        /// </summary>
        private void menuClear60Days_Click(object sender, EventArgs e)
        {
            ClearMessagesByDays(60);
        }

        /// <summary>
        /// 清除180天前的消息
        /// </summary>
        private void menuClear180Days_Click(object sender, EventArgs e)
        {
            ClearMessagesByDays(180);
        }

        /// <summary>
        /// 清除所有消息
        /// </summary>
        private void menuClearAll_Click(object sender, EventArgs e)
        {
            ClearAllMessages();
        }

        /// <summary>
        /// 按天数清除消息
        /// </summary>
        /// <param name="days">天数</param>
        private void ClearMessagesByDays(int days)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-days);

                // 确认对话框
                var result = MessageBox.Show($"确定要清除{cutoffDate:yyyy-MM-dd}之前的所有消息吗？\n\n此操作将删除所有{cutoffDate:yyyy-MM-dd}之前的消息，包括持久化数据。",
                    "确认清除消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // 清除指定天数前的消息
                var messagesToDelete = _messageManager.GetAllMessages()
                    .Where(m => m.SendTime < cutoffDate)
                    .ToList();

                if (messagesToDelete.Count == 0)
                {
                    MessageBox.Show($"没有找到{cutoffDate:yyyy-MM-dd}之前的消息。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 执行清除操作
                foreach (var message in messagesToDelete)
                {
                    _messageManager.DeleteMessage(message.MessageId);
                }

                // 重新加载消息列表
                LoadMessages();

                MessageBox.Show($"已成功清除{cutoffDate:yyyy-MM-dd}之前的{messagesToDelete.Count}条消息。",
                    "清除完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清除消息时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"清除消息失败: {ex.ToString()}");
            }
        }

        /// <summary>
        /// 清除所有消息
        /// </summary>
        private void ClearAllMessages()
        {
            try
            {
                // 确认对话框
                var result = MessageBox.Show("确定要清除所有消息吗？\n\n此操作将删除所有消息，包括持久化数据，且无法恢复。",
                    "确认清除所有消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                var allMessages = _messageManager.GetAllMessages();

                if (allMessages.Count == 0)
                {
                    MessageBox.Show("当前没有可清除的消息。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 执行清除所有消息操作
                _messageManager.ClearAllMessages();

                // 重新加载消息列表
                LoadMessages();

                MessageBox.Show($"已成功清除所有{allMessages.Count}条消息。",
                    "清除完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清除所有消息时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"清除所有消息失败: {ex.ToString()}");
            }
        }

        #endregion


    }

    /// <summary>
    /// 消息点击事件参数
    /// </summary>
    public class MessageClickedEventArgs : EventArgs
    {
        /// <summary>
        /// 被点击的消息对象
        /// </summary>
        public RUINORERP.Model.TransModel.MessageData Message { get; }

        /// <summary>
        /// 初始化消息点击事件参数
        /// </summary>
        /// <param name="message">消息对象</param>
        public MessageClickedEventArgs(RUINORERP.Model.TransModel.MessageData message)
        {
            Message = message;
        }


     
    }

}