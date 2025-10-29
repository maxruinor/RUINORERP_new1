using RUINORERP.Model.TransModel;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Services;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.BaseForm;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.UI.Log;
using System.ComponentModel;
using RUINORERP.Business;
using RUINORERP.PacketSpec.Commands;
using Timer = System.Windows.Forms.Timer;
using RUINORERP.Model.CommonModel;
using RUINORERP.Global;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 消息管理器 - 负责处理所有与消息相关的功能
    /// </summary>
    public class MessageManager : IDisposable
    {
        /// <summary>
        /// 消息管理器单例实例
        /// </summary>
        public static MessageManager Instance { get; private set; }
    
        // 消息队列锁
        private readonly object _messageQueueLock = new object();

        // 消息队列的最大消息数量
        private const int MAX_MESSAGE_COUNT = 100;

        // 缓存的消息列表
        private readonly List<RUINORERP.Model.TransModel.ReminderData> _messages = new List<RUINORERP.Model.TransModel.ReminderData>();

        // 未读消息数量
        private int _unreadMessageCount = 0;

        // 消息处理定时器
        private Timer _messageTimer;

        // 日志记录器
        private readonly ILogger logger;

        // 通知服务
        private readonly NotificationService _notificationService;

        // 消息状态更新事件
        public event EventHandler MessageStatusChanged;

        // 消息接收事件
        public event EventHandler<RUINORERP.UI.Network.Services.MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <returns>消息列表</returns>
        public List<RUINORERP.Model.TransModel.ReminderData> GetAllMessages()
        {
            try
            {
                lock (_messageQueueLock)
                {
                    return new List<RUINORERP.Model.TransModel.ReminderData>(_messages);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "获取所有消息时发生异常");
                return new List<RUINORERP.Model.TransModel.ReminderData>();
            }
        }

        /// <summary>
        /// 将所有消息标记为已读
        /// </summary>
        public void MarkAllMessagesAsRead()
        {
            try
            {
                bool hasChanges = false;

                lock (_messageQueueLock)
                {
                    foreach (var message in _messages)
                    {
                        if (!message.IsRead)
                        {
                            message.IsRead = true;
                            hasChanges = true;
                        }
                    }
                }

                if (hasChanges)
                {
                    // 触发消息状态变更事件
                    OnMessageStatusChanged();
                    logger?.LogInformation("所有消息已标记为已读");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "将所有消息标记为已读时发生异常");
            }
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        public int UnreadMessageCount
        {
            get { return GetUnreadMessageCount(); }
            private set
            {
                _unreadMessageCount = value;
                // 不再触发带参数的事件
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="notificationService">通知服务</param>
        public MessageManager(ILogger logger = null, NotificationService notificationService = null)
        {
            this.logger = logger;
            _notificationService = notificationService;
            // 设置单例实例
            Instance = this;

            // 初始化消息处理定时器
            InitializeMessageTimer();
        }

        /// <summary>
        /// 初始化消息处理定时器
        /// </summary>
        private void InitializeMessageTimer()
        {
            _messageTimer = new Timer();
            _messageTimer.Interval = 1000;
            _messageTimer.Tick += (sender, e) => ProcessMessages(null);
            _messageTimer.Start();
            // 初始化未读消息计数
            _unreadMessageCount = 0;
        }

        /// <summary>
        /// 处理消息队列中的消息
        /// </summary>
        /// <param name="state">状态对象</param>
        private void ProcessMessages(object state)
        {
            try
            {
                ReminderData messageInfo = null;

                lock (_messageQueueLock)
                {
                    if (_messages.Count > 0)
                    {
                        // 获取并移除第一条消息
                        messageInfo = _messages[0];
                        _messages.RemoveAt(0);
                    }
                }

                if (messageInfo != null)
                {
                    // 创建Network.Services中的MessageReceivedEventArgs实例并传递给事件处理方法
        var messageArgs = new RUINORERP.UI.Network.Services.MessageReceivedEventArgs
        {
            MessageType = "UserMessage",
            Data = messageInfo,
            Timestamp = DateTime.Now
        };
        OnMessageReceived(messageArgs);
                    ShowMessagePrompt(messageInfo);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理消息时发生异常");
            }
        }

        /// <summary>
        /// 显示消息提示
        /// </summary>
        /// <param name="messageInfo">消息信息</param>
        private void ShowMessagePrompt(ReminderData messageInfo)
        {
            try
            {
                // 根据消息类型显示不同的提示框
                if (messageInfo.messageCmd == MessageCmdType.UnLockRequest || messageInfo.messageCmd == MessageCmdType.Notice)
                {
                    InstructionsPrompt instructionsPrompt = new InstructionsPrompt();
                    instructionsPrompt.ReminderData = messageInfo;
                    instructionsPrompt.txtSender.Text = messageInfo.SenderEmployeeName ?? "系统";

                    if (messageInfo.messageCmd == MessageCmdType.UnLockRequest)
                    {
                        instructionsPrompt.txtSubject.Text = $"请求解锁【{messageInfo.BizType.ToString()}】"; // BizType不是可空类型，直接调用ToString()
                    }
                    else
                    {
                        instructionsPrompt.btnAgree.Visible = false;
                        instructionsPrompt.btnRefuse.Visible = false;
                        instructionsPrompt.txtSubject.Text = messageInfo.RemindSubject;
                    }

                    instructionsPrompt.Content = messageInfo.ReminderContent;

                    // 在UI线程上显示
                    if (Application.OpenForms.Count > 0)
                    {
                        Application.OpenForms[0].Invoke(new Action(() =>
                        {
                            instructionsPrompt.Show();
                            instructionsPrompt.TopMost = true;
                        }));
                    }
                }
                else
                {
                    MessagePrompt messager = new MessagePrompt();
                
                    messager.txtSender.Text = messageInfo.SenderEmployeeName ?? "系统";

                    if (!string.IsNullOrEmpty(messageInfo.RemindSubject))
                    {
                        // 使用ToString()确保正确转换BizType
                        messager.txtSubject.Text = $"【{messageInfo.BizType.ToString()}】{messageInfo.RemindSubject}"; // BizType不是可空类型，直接调用ToString()
                    }
                    else
                    {
                        messager.txtSubject.Text = "请求协助";
                    }

                    messager.Content = messageInfo.ReminderContent;
                    messager.ReminderData = messageInfo;

                    // 在UI线程上显示
                    if (Application.OpenForms.Count > 0)
                    {
                        Application.OpenForms[0].Invoke(new Action(() =>
                        {
                            messager.Show();
                            messager.TopMost = true;
                        }));
                    }
                }

                // 标记消息为已读
                MarkMessageAsRead(messageInfo);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "显示消息提示时发生异常");
            }
        }

        /// <summary>
        /// 订阅服务器消息事件
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        public void SubscribeServerMessageEvents(ClientCommunicationService communicationService)
        {
            try
            {
                logger?.LogInformation("开始订阅服务器消息事件");

                if (communicationService == null)
                {
                    logger?.LogWarning("通信服务为空，无法订阅消息事件");
                    return;
                }

                // 订阅服务器推送的弹窗消息
                communicationService.SubscribeCommand(MessageCommands.SendPopupMessage, (packet, data) =>
                {
                    // 创建MessageReceivedEventArgs实例并传递给事件处理方法
                    var args = new RUINORERP.UI.Network.Services.MessageReceivedEventArgs
                    {
                        MessageType = "Popup",
                        Data = data,
                        Timestamp = DateTime.Now
                    };
                    OnPopupMessageReceived(args);
                });

                // 订阅服务器推送的普通消息
                communicationService.SubscribeCommand(MessageCommands.SendMessageToUser, (packet, data) =>
                {
                    // 创建MessageReceivedEventArgs实例并传递给事件处理方法
                    var args = new RUINORERP.UI.Network.Services.MessageReceivedEventArgs
                    {
                        MessageType = "User",
                        Data = data,
                        Timestamp = DateTime.Now
                    };
                    OnUserMessageReceived(args);
                });

                // 订阅系统通知
                communicationService.SubscribeCommand(MessageCommands.SendSystemNotification, (packet, data) =>
                {
                    // 创建MessageReceivedEventArgs实例并传递给事件处理方法
                    var args = new RUINORERP.UI.Network.Services.MessageReceivedEventArgs
                    {
                        MessageType = "SystemNotification",
                        Data = data,
                        Timestamp = DateTime.Now
                    };
                    OnSystemNotificationReceived(args);
                });

                logger?.LogInformation("已成功订阅服务器消息事件");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "订阅服务器消息事件时发生异常");
            }
        }

        /// <summary>
        /// 处理弹窗消息
        /// </summary>
        /// <param name="args">消息接收事件参数</param>
        private void OnPopupMessageReceived(RUINORERP.UI.Network.Services.MessageReceivedEventArgs args)
        {
            try
            {
                logger?.LogDebug("收到弹窗消息");

                // 处理弹窗消息逻辑
                if (args?.Data != null)
                {
                    var messageData = args.Data as Dictionary<string, object>;
                    if (messageData != null)
                    {
                        var reminderData = CreateReminderDataFromDictionary(messageData, MessageCmdType.Message);
                        if (reminderData != null)
                        {
                            AddMessageToList(reminderData);
                            logger?.LogInformation("成功处理弹窗消息: {Title}", reminderData.RemindSubject ?? "无标题");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理弹窗消息时发生异常");
            }
        }

        /// <summary>
        /// 处理用户消息
        /// </summary>
        /// <param name="args">消息接收事件参数</param>
        private void OnUserMessageReceived(RUINORERP.UI.Network.Services.MessageReceivedEventArgs args)
        {
            try
            {
                logger?.LogDebug("收到用户消息");

                // 处理用户消息逻辑
                if (args?.Data != null)
                {
                    ReminderData reminderData = null;

                    // 尝试直接转换
                    if (args.Data is ReminderData)
                    {
                        reminderData = args.Data as ReminderData;
                    }
                    // 尝试从字典转换
                    else if (args.Data is Dictionary<string, object>)
                    {
                        var messageData = args.Data as Dictionary<string, object>;
                        reminderData = CreateReminderDataFromDictionary(messageData, MessageCmdType.Message);
                    }

                    if (reminderData != null)
                    {
                        AddMessageToList(reminderData);
                        logger?.LogInformation("成功处理用户消息: {Title}", reminderData.RemindSubject ?? "无标题");
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理用户消息时发生异常");
            }
        }

        /// <summary>
        /// 处理系统通知
        /// </summary>
        /// <param name="args">消息接收事件参数</param>
        private void OnSystemNotificationReceived(RUINORERP.UI.Network.Services.MessageReceivedEventArgs args)
        {
            try
            {
                logger?.LogDebug("收到系统通知");

                // 处理系统通知逻辑
                if (args?.Data != null)
                {
                    ReminderData reminderData = null;

                    // 尝试直接转换
                    if (args.Data is ReminderData)
                    {
                        reminderData = args.Data as ReminderData;
                    }
                    // 尝试从字典转换
                    else if (args.Data is Dictionary<string, object>)
                    {
                        var messageData = args.Data as Dictionary<string, object>;
                        reminderData = CreateReminderDataFromDictionary(messageData, MessageCmdType.Notice);
                    }

                    if (reminderData != null)
                    {
                        AddMessageToList(reminderData);
                        ShowSystemNotification(reminderData);
                        logger?.LogInformation("成功处理系统通知: {Title}", reminderData.RemindSubject ?? "无标题");
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理系统通知时发生异常");
            }
        }

        /// <summary>
        /// 从字典创建ReminderData对象
        /// </summary>
        /// <param name="data">数据字典</param>
        /// <param name="defaultCmdType">默认命令类型</param>
        /// <returns>ReminderData对象</returns>
        private ReminderData CreateReminderDataFromDictionary(Dictionary<string, object> data, MessageCmdType defaultCmdType)
        {
            var reminderData = new ReminderData
            {
                messageCmd = defaultCmdType
            };

            // 尝试从字典中获取各个字段
            if (data.ContainsKey("Message")) reminderData.ReminderContent = data["Message"]?.ToString();
            if (data.ContainsKey("Title")) reminderData.RemindSubject = data["Title"]?.ToString();
            if (data.ContainsKey("SenderEmployeeName")) reminderData.SenderEmployeeName = data["SenderEmployeeName"]?.ToString();
            if (data.ContainsKey("Content")) reminderData.ReminderContent = data["Content"]?.ToString();
            if (data.ContainsKey("Subject")) reminderData.RemindSubject = data["Subject"]?.ToString();
            if (data.ContainsKey("BizType"))
            {
                try { reminderData.BizType = (BizType)Enum.Parse(typeof(BizType), data["BizType"]?.ToString()); }
                catch { } // 忽略类型转换错误
            }

            return reminderData;
        }

        /// <summary>
        /// 添加消息到列表（兼容方法）
        /// </summary>
        /// <param name="message">消息对象</param>
        private void AddMessageToList(RUINORERP.Model.TransModel.ReminderData message)
        {
            AddMessage(message);
        }

        /// <summary>
        /// 添加消息到队列
        /// </summary>
        /// <param name="message">消息对象</param>
        public void AddMessage(RUINORERP.Model.TransModel.ReminderData message)
        {
            try
            {
                if (message == null)
                {
                    logger?.LogWarning("尝试添加空消息到队列");
                    return;
                }

                lock (_messageQueueLock)
                {
                    // 控制列表大小，移除最旧的消息
                    while (_messages.Count >= MAX_MESSAGE_COUNT)
                    {
                        _messages.RemoveAt(0);
                    }

                    _messages.Add(message);

                    // 如果消息未读，触发状态变更事件
                    if (!message.IsRead)
                    {
                        // 触发消息状态变更事件
                        OnMessageStatusChanged();
                    }
                }

                logger?.LogInformation($"消息已添加到队列，主题: {message.RemindSubject ?? "无主题"}");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "添加消息到队列时发生异常");
            }
        }

        /// <summary>
        /// 添加消息到队列（重载，兼容字符串参数）
        /// </summary>
        /// <param name="source">消息来源</param>
        /// <param name="content">消息内容</param>
        /// <param name="sendTime">发送时间</param>
        public void AddMessage(string source, string content, DateTime sendTime)
        {
            try
            {
                // 创建ReminderData对象
                var message = new RUINORERP.Model.TransModel.ReminderData
                {
                    SenderEmployeeName = source,
                    ReminderContent = content,
                    SendTime = sendTime.ToString(), // 转换为字符串，因为ReminderData.SendTime是string类型
                    RemindSubject = source,
                    IsRead = false,
                    messageCmd = RUINORERP.Model.TransModel.MessageCmdType.Message
                };

                // 调用主方法
                AddMessage(message);

                logger?.LogInformation($"已添加文本消息，来源: {source}");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "添加文本消息到队列时发生异常");
            }
        }

        /// <summary>
        /// 添加系统通知
        /// </summary>
        /// <param name="title">通知标题</param>
        /// <param name="content">通知内容</param>
        public void AddSystemNotification(string title, string content)
        {
            try
            {
                // 创建系统通知消息
                var notification = new RUINORERP.Model.TransModel.ReminderData
                {
                    SenderEmployeeName = "系统",
                    RemindSubject = title,
                    ReminderContent = content,
                    SendTime = DateTime.Now.ToString(), // 转换为字符串，因为ReminderData.SendTime是string类型
                    IsRead = false,
                    messageCmd = RUINORERP.Model.TransModel.MessageCmdType.Notice
                };

                // 添加到消息队列
                AddMessage(notification);

                logger?.LogInformation($"已添加系统通知: {title}");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "添加系统通知时发生异常");
            }
        }

        /// <summary>
        /// 显示系统通知
        /// </summary>
        /// <param name="reminderData">通知数据</param>
        private void ShowSystemNotification(ReminderData reminderData)
        {
            try
            {
                // 显示系统通知
                if (Application.OpenForms.Count > 0)
                {
                    Application.OpenForms[0].Invoke(new Action(() =>
                    {
                        try
                        {
                            // 尝试使用NotificationService发送通知
                            _notificationService?.SendNotification(
                                new Notification {
                                    Subject = reminderData.RemindSubject ?? "系统通知",
                                    Content = reminderData.ReminderContent
                                });
                        }
                        catch (Exception)
                        {
                            // 如果NotificationService不可用，使用备用方式
                            NotificationBox notificationBox = NotificationBox.Instance();
                            notificationBox.ShowForm($"{reminderData.RemindSubject ?? "系统通知"}\n{reminderData.ReminderContent}");
                            // 设置定时器自动关闭通知
                            System.Threading.Timer timer = null;
                            timer = new System.Threading.Timer((state) => {
                                Application.OpenForms[0].Invoke(new Action(() => {
                                    notificationBox.CloseForm();
                                }));
                                timer.Dispose();
                            }, null, 5000, Timeout.Infinite);
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "显示系统通知时发生异常");
            }
        }

        /// <summary>
        /// 显示消息列表
        /// </summary>
        public void ShowMessageList()
        {
            try
            {
                logger?.LogInformation("正在显示消息列表");

                // 创建消息列表窗口
                Form messageListForm = new Form
                {
                    Text = "消息列表",
                    Size = new System.Drawing.Size(800, 600),
                    StartPosition = FormStartPosition.CenterScreen
                };

                // 获取所有消息的副本，使用SendTime替代CreateTime
                var messagesCopy = GetAllMessages().OrderByDescending(m => m.SendTime).ToList();
                logger?.LogDebug("获取到{Count}条消息", messagesCopy.Count);

                // 创建顶部过滤面板
                Panel filterPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 30,
                    BorderStyle = BorderStyle.FixedSingle
                };

                // 添加过滤器标签
                Label filterLabel = new Label
                {
                    Text = "显示:",
                    Dock = DockStyle.Left,
                    Width = 40,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };
                filterPanel.Controls.Add(filterLabel);

                // 添加过滤器下拉框
                ComboBox filterComboBox = new ComboBox
                {
                    Dock = DockStyle.Left,
                    Width = 100,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                filterComboBox.Items.AddRange(new object[] { "全部消息", "未读消息", "已读消息" });
                filterComboBox.SelectedIndex = 0;
                filterPanel.Controls.Add(filterComboBox);

                // 创建数据网格视图
                DataGridView dataGridView = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoGenerateColumns = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect
                };

                // 添加列
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Title", HeaderText = "标题", DataPropertyName = "RemindSubject", Width = 150 });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Content", HeaderText = "内容", DataPropertyName = "ReminderContent", Width = 300, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Sender", HeaderText = "发送者", DataPropertyName = "SenderEmployeeName", Width = 100 });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Time", HeaderText = "时间", DataPropertyName = "SendTime", Width = 150 });
                dataGridView.Columns.Add(new DataGridViewCheckBoxColumn
                { Name = "IsRead", HeaderText = "已读", DataPropertyName = "IsRead", Width = 50 });

                // 设置数据源
                BindingList<RUINORERP.Model.TransModel.ReminderData> bindingList = new BindingList<RUINORERP.Model.TransModel.ReminderData>(messagesCopy);
                dataGridView.DataSource = bindingList;

                // 创建底部按钮面板
                Panel buttonPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    BorderStyle = BorderStyle.FixedSingle
                };

                // 创建标记已读按钮
                Button markAsReadButton = new Button
                {
                    Text = "标记选中为已读",
                    Dock = DockStyle.Left,
                    Width = 120,
                    Margin = new Padding(5)
                };
                buttonPanel.Controls.Add(markAsReadButton);

                // 创建标记全部已读按钮
                Button markAllAsReadButton = new Button
                {
                    Text = "全部标记为已读",
                    Dock = DockStyle.Left,
                    Width = 120,
                    Margin = new Padding(5)
                };
                buttonPanel.Controls.Add(markAllAsReadButton);

                // 标记选中消息为已读
                markAsReadButton.Click += (sender, e) =>
                {
                    try
                    {
                        if (dataGridView.SelectedRows.Count > 0)
                        {
                            logger?.LogDebug("标记选中的{Count}条消息为已读", dataGridView.SelectedRows.Count);

                            // 记录需要更新的消息
                            List<RUINORERP.Model.TransModel.ReminderData> updatedReminders = new List<RUINORERP.Model.TransModel.ReminderData>();

                            foreach (DataGridViewRow row in dataGridView.SelectedRows)
                            {
                                if (row.DataBoundItem is RUINORERP.Model.TransModel.ReminderData reminder && !reminder.IsRead)
                                {
                                    reminder.IsRead = true;
                                    updatedReminders.Add(reminder);
                                }
                            }

                            if (updatedReminders.Count > 0)
                            {
                                dataGridView.Refresh();

                                // 更新原队列中的已读状态
                                MarkMessagesAsRead(updatedReminders);

                                // 触发消息状态变更事件
                                OnMessageStatusChanged();

                                logger?.LogInformation("成功标记{Count}条消息为已读", updatedReminders.Count);
                                MessageBox.Show("已成功标记选中消息为已读", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "标记消息为已读时发生异常");
                        MessageBox.Show("操作失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // 标记全部已读按钮点击事件
                markAllAsReadButton.Click += (sender, e) =>
                {
                    try
                    {
                        logger?.LogDebug("标记所有消息为已读");

                        // 获取当前显示的所有消息
                        var visibleMessages = ((BindingList<RUINORERP.Model.TransModel.ReminderData>)dataGridView.DataSource).Where(m => !m.IsRead).ToList();

                        if (visibleMessages.Count > 0)
                        {
                            foreach (var reminder in visibleMessages)
                            {
                                reminder.IsRead = true;
                            }

                            dataGridView.Refresh();

                            // 更新原队列中的已读状态
                            MarkMessagesAsRead(visibleMessages);

                            // 触发消息状态变更事件
                            OnMessageStatusChanged();

                            logger?.LogInformation("成功标记全部{Count}条消息为已读", visibleMessages.Count);
                            MessageBox.Show("已成功标记全部消息为已读", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "标记全部消息为已读时发生异常");
                        MessageBox.Show("操作失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // 过滤器变更事件
                filterComboBox.SelectedIndexChanged += (sender, e) =>
                {
                    try
                    {
                        string filter = filterComboBox.SelectedItem.ToString();
                        logger?.LogDebug("应用消息过滤器: {Filter}", filter);

                        switch (filter)
                        {
                            case "全部消息":
                                dataGridView.DataSource = new BindingList<RUINORERP.Model.TransModel.ReminderData>(messagesCopy);
                                break;
                            case "未读消息":
                                dataGridView.DataSource = new BindingList<RUINORERP.Model.TransModel.ReminderData>(messagesCopy.Where(m => !m.IsRead).ToList());
                                break;
                            case "已读消息":
                                dataGridView.DataSource = new BindingList<RUINORERP.Model.TransModel.ReminderData>(messagesCopy.Where(m => m.IsRead).ToList());
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "切换消息过滤器时发生异常");
                    }
                };

                // 添加控件到窗口
                messageListForm.Controls.Add(dataGridView);
                messageListForm.Controls.Add(filterPanel);
                messageListForm.Controls.Add(buttonPanel);

                // 显示窗口
                messageListForm.ShowDialog();

                logger?.LogInformation("消息列表窗口已关闭");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "显示消息列表时发生异常");
                MessageBox.Show("显示消息列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// 将指定消息标记为已读
        /// </summary>
        /// <param name="message">要标记的消息</param>
        public void MarkMessageAsRead(RUINORERP.Model.TransModel.ReminderData message)
        {
            try
            {
                if (message == null)
                {
                    logger?.LogWarning("尝试标记空消息为已读");
                    return;
                }

                lock (_messageQueueLock)
                {
                    // 在列表中查找并更新对应的消息，使用SendTime替代CreateTime
                    var listMessage = _messages.FirstOrDefault(m =>
                        m.RemindSubject == message.RemindSubject &&
                        m.SendTime == message.SendTime &&
                        m.ReminderContent == message.ReminderContent);

                    if (listMessage != null && !listMessage.IsRead)
                    {
                        listMessage.IsRead = true;

                        // 触发消息状态变更事件
                        OnMessageStatusChanged();
                        logger?.LogInformation($"消息已标记为已读: {message.RemindSubject ?? "无主题"}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "将消息标记为已读时发生异常");
            }
        }

        /// <summary>
        /// 通过标题和内容标记消息为已读（兼容方法）
        /// </summary>
        /// <param name="title">消息标题</param>
        /// <param name="content">消息内容</param>
        public void MarkMessageAsRead(string title, string content)
        {
            try
            {
                lock (_messageQueueLock)
                {
                    // 查找匹配的消息
                    var message = _messages.FirstOrDefault(m =>
                        m.RemindSubject == title &&
                        m.ReminderContent == content &&
                        !m.IsRead);

                    if (message != null)
                    {
                        message.IsRead = true;

                        // 触发消息状态变更事件
                        OnMessageStatusChanged();
                        logger?.LogInformation($"通过标题和内容标记消息为已读: {title}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "通过标题和内容标记消息为已读时发生异常");
            }
        }

        /// <summary>
        /// 标记多条消息为已读
        /// </summary>
        /// <param name="messages">消息列表</param>
        public void MarkMessagesAsRead(List<RUINORERP.Model.TransModel.ReminderData> messages)
        {
            lock (_messageQueueLock)
            {
                foreach (var msg in _messages)
                {
                    if (messages.Any(m =>
                        m.RemindSubject == msg.RemindSubject &&
                        m.SendTime == msg.SendTime &&
                        m.SenderEmployeeName == msg.SenderEmployeeName))
                    {
                        if (!msg.IsRead)
                        {
                            msg.IsRead = true;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 触发消息状态变更事件
        /// </summary>
        protected virtual void OnMessageStatusChanged()
        {
            try
            {
                // 触发消息状态变更事件
                MessageStatusChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                // 记录异常
                logger?.LogError(ex, "消息状态变更事件触发失败");
            }
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <returns>未读消息数量</returns>
        public int GetUnreadMessageCount()
        {
            try
            {
                lock (_messageQueueLock)
                {
                    return _messages.Count(m => !m.IsRead);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "获取未读消息数量时发生异常");
                return 0;
            }
        }



        /// <summary>
        /// 初始化消息菜单
        /// </summary>
        /// <param name="menuStrip">主菜单条</param>
        public void InitializeMessageMenu(MenuStrip menuStrip)
        {
            try
            {
                if (menuStrip == null)
                    return;

                // 检查是否存在消息菜单
                ToolStripMenuItem messageMenu = null;
                foreach (ToolStripMenuItem item in menuStrip.Items)
                {
                    if (item.Text == "消息")
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

                // 添加查看消息列表菜单项
                bool hasViewMessageList = false;
                foreach (ToolStripItem item in messageMenu.DropDownItems)
                {
                    if (item.Text == "查看消息列表")
                    {
                        hasViewMessageList = true;
                        break;
                    }
                }

                if (!hasViewMessageList)
                {
                    ToolStripMenuItem viewMessageListItem = new ToolStripMenuItem("查看消息列表");
                    viewMessageListItem.Click += (s, e) =>
                    {
                        try
                        {
                            ShowMessageList();
                        }
                        catch (Exception ex)
                        {
                            logger?.LogError(ex, "点击查看消息列表时发生异常");
                            MessageBox.Show("打开消息列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };
                    messageMenu.DropDownItems.Add(viewMessageListItem);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "初始化消息菜单时发生异常");
            }
        }

        /// <summary>
        /// 触发消息状态变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        // 移除带参数的OnMessageStatusChanged方法，使用无参版本统一处理

        /// <summary>
        /// 触发消息接收事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnMessageReceived(RUINORERP.UI.Network.Services.MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_messageTimer != null)
            {
                _messageTimer.Stop();
                _messageTimer.Dispose();
            }
        }
    }

    /// <summary>
    /// 消息状态变更事件参数
    /// </summary>
    // MessageStatusChangedEventArgs类已移除，使用标准EventHandler接口

    /// <summary>
    // 已移除本地的MessageReceivedEventArgs类，使用RUINORERP.UI.Network.Services命名空间中的同名类

    // 已移除NotificationType枚举，因为NotificationService已提供相关功能
}