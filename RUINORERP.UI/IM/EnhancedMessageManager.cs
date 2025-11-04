using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.UserCenter;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Model.TransModel;
using RUINORERP.Business.BizMapperService;
using Microsoft.Extensions.Logging;
// 移除Microsoft.Extensions.Logging引用，使用应用程序中定义的ILogger

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 增强版消息管理器
    /// 负责处理各类消息的接收、存储、展示和交互
    /// </summary>
    public class EnhancedMessageManager : IDisposable
    {
        private readonly ILogger<EnhancedMessageManager> _logger;
        private readonly MessageService _messageService;
        private readonly List<MessageData> _messageList = new List<MessageData>();
        private readonly object _messagesLock = new object();
        private int _unreadMessageCount = 0;
        private Timer _messageCheckTimer;
        private bool _disposed = false;
        
        /// <summary>
        /// 消息状态变更事件
        /// </summary>
        public event EventHandler<MessageData> MessageStatusChanged;
        
        /// <summary>
        /// 未读消息计数变更事件
        /// </summary>
        public event EventHandler<int> UnreadMessageCountChanged;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="messageService">消息服务</param>
        public EnhancedMessageManager(ILogger<EnhancedMessageManager> logger, MessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
            
            // 初始化只读字段
            _messageCheckTimer = new Timer
            {
                Interval = 30000 // 30秒检查一次未读消息
            };
            _messageCheckTimer.Tick += MessageCheckTimer_Tick;
            
            InitializeMessageService();
            _messageCheckTimer.Start();
        }
        
        /// <summary>
        /// 初始化消息服务
        /// </summary>
        private void InitializeMessageService()
        {
            try
            {
                _logger.LogInformation("开始初始化消息服务订阅");
                
                // 先检查_messageService是否为null
                if (_messageService == null)
                {
                    _logger.LogError("MessageService实例为null，无法订阅消息事件");
                    return;
                }
                
                // 取消之前可能存在的订阅，避免重复订阅导致的多次执行
                _messageService.PopupMessageReceived -= OnPopupMessageReceived;
                _messageService.BusinessMessageReceived -= OnBusinessMessageReceived;
                _messageService.DepartmentMessageReceived -= OnDepartmentMessageReceived;
                _messageService.BroadcastMessageReceived -= OnBroadcastMessageReceived;
                _messageService.SystemNotificationReceived -= OnSystemNotificationReceived;
                
                // 重新订阅所有消息事件
                _messageService.PopupMessageReceived += OnPopupMessageReceived;
                _messageService.BusinessMessageReceived += OnBusinessMessageReceived;
                _messageService.DepartmentMessageReceived += OnDepartmentMessageReceived;
                _messageService.BroadcastMessageReceived += OnBroadcastMessageReceived;
                _messageService.SystemNotificationReceived += OnSystemNotificationReceived;
                
                _logger.LogInformation("已成功初始化消息服务并订阅所有消息事件");
                
                // 记录当前订阅状态，便于调试
                LogEventSubscriptionStatus();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化消息服务时发生异常");
            }
        }
        
        /// <summary>
        /// 记录事件订阅状态，用于调试
        /// </summary>
        private void LogEventSubscriptionStatus()
        {
            try
            {
                if (_messageService == null)
                    return;
                    
                // 使用反射获取事件订阅状态，这在调试时非常有用
                System.Reflection.FieldInfo popupEventField = _messageService.GetType().GetField("PopupMessageReceived", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                
                if (popupEventField != null)
                {
                    Delegate popupEventDelegate = popupEventField.GetValue(_messageService) as Delegate;
                    if (popupEventDelegate != null)
                    {
                        int handlerCount = popupEventDelegate.GetInvocationList().Length;
                        _logger.LogInformation("弹窗消息事件当前订阅者数量: {HandlerCount}", handlerCount);
                    }
                    else
                    {
                        _logger.LogWarning("弹窗消息事件当前没有订阅者");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "记录事件订阅状态时发生异常（非关键错误）");
            }
        }
        
        // 初始化定时器方法已整合到构造函数中
        
        private void MessageCheckTimer_Tick(object sender, EventArgs e)
        {
            // 定期检查未读消息数量并更新UI
            try
            {
                UpdateUnreadMessageCount();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查未读消息时发生错误");
            }
        }
        
        /// <summary>
        /// 更新未读消息计数
        /// </summary>
        private void UpdateUnreadMessageCount()
        {
            int unreadCount = GetUnreadMessageCount();
            if (_unreadMessageCount != unreadCount)
            {
                _unreadMessageCount = unreadCount;
                _logger?.LogDebug($"已更新未读消息计数: {unreadCount}");
                // 未读消息计数变化时，触发未读消息计数变更事件
                UnreadMessageCountChanged?.Invoke(this, unreadCount);
            }
        }
        
        // 处理接收到的弹窗消息
        private void OnPopupMessageReceived(MessageData messageData)
        {
            try
            {
                if (messageData != null)
                {
                    AddMessage(messageData);
                    ShowDefaultMessagePrompt(messageData);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理弹窗消息时发生错误");
            }
        }
        
        // 处理接收到的业务消息
        private void OnBusinessMessageReceived(MessageData messageData)
        {
            ProcessBusinessMessage(messageData);
        }
        
        // 处理接收到的部门消息
        private void OnDepartmentMessageReceived(MessageData messageData)
        {
            try
            {
                if (messageData != null)
                {
                    AddMessage(messageData);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理部门消息时发生错误");
            }
        }
        
        // 处理接收到的广播消息
        private void OnBroadcastMessageReceived(MessageData messageData)
        {
            ShowDefaultMessagePrompt(messageData);
        }
        
        // 处理接收到的系统通知
        private void OnSystemNotificationReceived(MessageData messageData)
        {
            ShowDefaultMessagePrompt(messageData);
        }
        

        

        
        // 获取未读消息数量
        public int GetUnreadMessageCount()
        {
            lock (_messagesLock)
            {
                return _messageList.Count(msg => !msg.IsRead);
            }
        }
        
        // 获取所有消息
        public List<MessageData> GetAllMessages()
        {
            lock (_messagesLock)
            {
                return _messageList.ToList();
            }
        }
        
        // 根据ID获取消息
        public MessageData GetMessageById(long id)
        {
            lock (_messagesLock)
            {
                return _messageList.FirstOrDefault(msg => msg.Id == id);
            }
        }
        
        // 标记消息为已读
        public void MarkAsRead(long id)
        {
            lock (_messagesLock)
            {
                var message = GetMessageById(id);
                if (message != null && !message.IsRead)
                {
                    message.IsRead = true;
                    message.ReadTime = DateTime.Now;
                    OnMessageStatusChanged(message);
                }
            }
        }
        
        // 标记所有消息为已读
        public void MarkAllAsRead()
        {
            try
            {
                List<MessageData> updatedMessages = new List<MessageData>();
                lock (_messagesLock)
                {
                    foreach (var message in _messageList.Where(msg => !msg.IsRead))
                    {
                        message.IsRead = true;
                        message.ReadTime = DateTime.Now;
                        updatedMessages.Add(message);
                    }
                }
                
                // 对每个更新的消息触发状态变更事件
                foreach (var message in updatedMessages)
                {
                    OnMessageStatusChanged(message);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "将所有消息标记为已读时发生错误");
                throw;
            }
        }
        
        // 触发消息状态变更事件
        protected virtual void OnMessageStatusChanged(MessageData message)
        {
            _unreadMessageCount = GetUnreadMessageCount();
            MessageStatusChanged?.Invoke(this, message);
        }
        
        // 删除消息
        public void DeleteMessage(long id)
        {
            lock (_messagesLock)
            {
                var message = GetMessageById(id);
                if (message != null)
                {
                    _messageList.Remove(message);
                    // 如果删除的是未读消息，更新未读计数
                    if (!message.IsRead)
                    {
                        _unreadMessageCount = Math.Max(0, _unreadMessageCount - 1);
                    }
                }
            }
            
            // 触发状态变更事件，传递null表示消息已被删除
            OnMessageStatusChanged(null);
        }
        
        // 添加消息
        public void AddMessage(MessageData message)
        {
            if (message == null)
                return;
                
            lock (_messagesLock)
            {
                bool exists = _messageList.Any(m => m.Id == message.Id);
                if (!exists)
                {
                    _messageList.Insert(0, message);
                }
            }
            
            // 更新未读消息计数并触发事件
            UpdateUnreadMessageCount();
            
            // 异步处理业务逻辑
            Task.Run(() => ProcessBusinessMessage(message));
        }
        
        // 添加消息（重载方法）
        public void AddMessage(string content, string sender = "系统")
        {
            var message = new MessageData
            {
                Id = DateTime.Now.Ticks, // 使用时间戳作为唯一ID
                Content = content,
                Sender = sender,
                CreateTime = DateTime.Now,
                IsRead = false,
                MessageType = MessageType.System
            };
            AddMessage(message);
        }
   
        // 处理业务消息
        public void ProcessBusinessMessage(MessageData messageData)
        {
            try
            {
                _logger?.Info($"处理业务消息: {messageData.Title}");
                
                // 添加到消息列表
                lock (_messagesLock)
                {
                    if (!_messageList.Any(m => m.Id == messageData.Id))
                    {
                        _messageList.Add(messageData);
                        _unreadMessageCount++;
                        OnMessageStatusChanged(messageData);
                    }
                }
                
                // 根据消息类型显示不同的提示框
                switch (messageData.MessageType)
                {
                    case MessageType.UnLockRequest:
                        ShowUnlockRequestPrompt(messageData);
                        break;
                    case MessageType.Notice:
                        ShowNoticePrompt(messageData);
                        break;
                    case MessageType.Business:
                        ShowBusinessMessagePrompt(messageData);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理业务消息时发生错误");
            }
        }
        
        // 处理业务消息（公开方法）
        public void ProcessBusinessMessagePublic(MessageData message)
        {
            ProcessBusinessMessage(message);
        }
        
        // 显示默认消息提示
        private void ShowDefaultMessagePrompt(MessageData messageData)
        {
            try
            {
                _logger?.Info($"显示系统通知: {messageData.Title}");
                
                // 添加到消息列表
                lock (_messagesLock)
                {
                    if (!_messageList.Any(m => m.Id == messageData.Id))
                    {
                        _messageList.Add(messageData);
                        _unreadMessageCount++;
                        OnMessageStatusChanged(messageData);
                    }
                }
                
                // 创建消息提示窗口
                var prompt = new MessagePrompt(messageData, this);
                
                // 显示提示窗口
                if (Application.OpenForms.Count > 0)
                {
                    Application.OpenForms[0].Invoke(new Action(() =>
                    {
                        prompt.Show();
                        prompt.TopMost = true;
                    }));
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "显示默认消息提示时发生错误");
            }
        }
        

        
        // 显示解锁请求提示
        private void ShowUnlockRequestPrompt(MessageData message)
        {
            try
            {
                var prompt = new InstructionsPrompt(message, _logger, _messageService);
                prompt.Title = "解锁请求";
                
                if (Application.OpenForms.Count > 0)
                {
                    Application.OpenForms[0].Invoke(new Action(() =>
                    {
                        prompt.Show();
                        prompt.TopMost = true;
                    }));
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "显示解锁请求提示时发生错误");
            }
        }
        
        // 显示通知提示
        private void ShowNoticePrompt(MessageData message)
        {
            try
            {
                var prompt = new InstructionsPrompt(message, _logger, _messageService);
                prompt.HideActionButtons();
                
                if (Application.OpenForms.Count > 0)
                {
                    Application.OpenForms[0].Invoke(new Action(() =>
                    {
                        prompt.Show();
                        prompt.TopMost = true;
                    }));
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "显示通知提示时发生错误");
            }
        }
        
        // 显示默认消息提示 - 移除重复的方法实现
        
        // 显示业务消息提示
        private void ShowBusinessMessagePrompt(MessageData message)
        {
            try
            {
                // 使用依赖注入获取MessagePrompt实例，而不是直接new
                var prompt = Startup.GetFromFac<MessagePrompt>();
                if (prompt != null)
                {
                    prompt.MessageData = message;
                    
                    if (Application.OpenForms.Count > 0)
                    {
                        Application.OpenForms[0].Invoke(new Action(() =>
                        {
                            prompt.Show();
                            prompt.TopMost = true;
                        }));
                    }
                }
                else
                {
                    _logger?.LogWarning("无法从依赖注入容器获取MessagePrompt实例");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "显示业务消息提示时发生错误");
            }
        }
        
        // 导航到业务单据
        public void NavigateToBusinessDocument(BizType bizType, long bizId)
        {
            try
            {
                var tableType = EntityMappingHelper.GetEntityType(bizType);
                if (tableType == null)
                {
                    _logger?.Info($"未找到业务类型 {bizType} 对应的实体类型");
                    return;
                }
                
                var primaryKeyName = BaseUIHelper.GetEntityPrimaryKey(tableType);
                var queryConditions = new List<IConditionalModel>
                {
                    new ConditionalModel 
                    { 
                        FieldName = primaryKeyName, 
                        ConditionalType = ConditionalType.Equal, 
                        FieldValue = bizId.ToString(), 
                        CSharpTypeName = "long" 
                    }
                };
                
                var menuInfo = FindMenuInfoForEntity(tableType.Name);
                if (menuInfo == null)
                {
                    _logger?.Info($"未找到实体 {tableType.Name} 对应的菜单信息");
                    return;
                }
                
                var menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                var queryParameter = new QueryParameter
                {
                    conditionals = queryConditions,
                    tableType = tableType
                };
                
                var instance = Activator.CreateInstance(tableType);
                menuPowerHelper.ExecuteEvents(menuInfo, instance, queryParameter);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"导航到业务单据时发生错误: BizType={bizType}, BizId={bizId}");
            }
        }
        
        // 查找菜单信息
        private tb_MenuInfo FindMenuInfoForEntity(string entityName)
        {
            try
            {
                // 尝试通过MenuPowerHelper获取菜单信息，避免直接依赖MainForm
                var menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                if (menuPowerHelper != null)
                {
                    return menuPowerHelper.MenuList.FirstOrDefault(m => 
                        m.IsVisble && 
                        m.EntityName == entityName && 
                        m.BIBaseForm == "BaseBillEditGeneric`2");
                }
                
              
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"查找实体 {entityName} 对应的菜单信息时发生错误");
            }
            return null;
        }
        
        // 显示消息列表
        public void ShowMessageList()
        {
            ShowEnhancedMessageList();
        }
        
        // 显示增强版消息列表
        public void ShowEnhancedMessageList()
        {
            try
            {
                Form messageListForm = new Form
                {
                    Text = "消息中心",
                    Size = new Size(1000, 700),
                    StartPosition = FormStartPosition.CenterScreen,
                    Icon = SystemIcons.Information
                };
                
                var messagesCopy = GetAllMessages().OrderByDescending(m => m.CreateTime).ToList();
                
                ToolStrip toolStrip = new ToolStrip
                {
                    Dock = DockStyle.Top
                };
                
                DataGridView dataGridView = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoGenerateColumns = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = true,
                    AllowUserToResizeRows = false
                };
                
                // 添加按钮和列
                AddToolStripButtons(toolStrip, dataGridView);
                AddDataGridViewColumns(dataGridView);
                
                // 设置数据源和样式
                SetupDataGridViewDataSource(dataGridView, messagesCopy);
                SetupDataGridViewStyles(dataGridView);
                
                // 添加底部按钮
                Panel buttonPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    BorderStyle = BorderStyle.FixedSingle
                };
                AddButtonPanelButtons(buttonPanel, dataGridView);
                
                // 添加控件到窗体
                messageListForm.Controls.Add(dataGridView);
                messageListForm.Controls.Add(buttonPanel);
                messageListForm.Controls.Add(toolStrip);
                
                messageListForm.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "显示消息列表时发生错误");
                MessageBox.Show("显示消息列表时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // 添加工具栏按钮
        private void AddToolStripButtons(ToolStrip toolStrip, DataGridView dataGridView)
        {
            ToolStripButton refreshButton = new ToolStripButton("刷新", null, (s, e) => RefreshMessageList(dataGridView));
            refreshButton.ToolTipText = "刷新消息列表";
            toolStrip.Items.Add(refreshButton);
            
            toolStrip.Items.Add(new ToolStripSeparator());
            
            ToolStripLabel filterLabel = new ToolStripLabel("显示:");
            toolStrip.Items.Add(filterLabel);
            
            ToolStripComboBox filterComboBox = new ToolStripComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            filterComboBox.Items.AddRange(new object[] { 
                "全部消息", 
                "未读消息", 
                "已读消息", 
                "业务消息", 
                "系统通知", 
                "解锁请求" 
            });
            filterComboBox.SelectedIndex = 0;
            filterComboBox.SelectedIndexChanged += (sender, e) =>
            {
                FilterMessages(dataGridView, filterComboBox.SelectedItem.ToString());
            };
            toolStrip.Items.Add(filterComboBox);
        }
        
        // 添加数据网格视图列
        private void AddDataGridViewColumns(DataGridView dataGridView)
        {
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            { 
                Name = "BizType", 
                HeaderText = "业务类型", 
                DataPropertyName = "BizType", 
                Width = 120 
            });
            
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            { 
                Name = "Title", 
                HeaderText = "标题", 
                DataPropertyName = "Title", 
                Width = 200 
            });
            
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            { 
                Name = "Content", 
                HeaderText = "内容", 
                DataPropertyName = "Content", 
                Width = 300, 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
            });
            
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            { 
                Name = "Sender", 
                HeaderText = "发送者", 
                DataPropertyName = "Sender", 
                Width = 100 
            });
            
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            { 
                Name = "Time", 
                HeaderText = "时间", 
                DataPropertyName = "CreateTime", 
                Width = 150 
            });
            
            dataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            { 
                Name = "IsRead", 
                HeaderText = "已读", 
                DataPropertyName = "IsRead", 
                Width = 50 
            });
        }
        
        // 设置数据源
        private void SetupDataGridViewDataSource(DataGridView dataGridView, List<MessageData> messages)
        {
            var bindingList = new BindingList<MessageData>(messages);
            dataGridView.DataSource = bindingList;
            
            // 添加双击事件
            dataGridView.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
                {
                    var row = dataGridView.Rows[e.RowIndex];
                    if (row.DataBoundItem is MessageData message)
                    {
                        HandleMessageDoubleClick(message);
                    }
                }
            };
        }
        
        // 设置数据网格视图样式
        private void SetupDataGridViewStyles(DataGridView dataGridView)
        {
            dataGridView.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
                {
                    var row = dataGridView.Rows[e.RowIndex];
                    if (row.DataBoundItem is MessageData message)
                    {
                        // 未读消息显示为粗体
                        if (!message.IsRead)
                        {
                            row.DefaultCellStyle.Font = new Font(dataGridView.Font, FontStyle.Bold);
                        }
                        else
                        {
                            row.DefaultCellStyle.Font = new Font(dataGridView.Font, FontStyle.Regular);
                        }
                        
                        // 不同类型的消息显示不同颜色
                        switch (message.MessageType)
                        {
                            case MessageType.UnLockRequest:
                                row.DefaultCellStyle.BackColor = Color.LightYellow;
                                break;
                            case MessageType.Business:
                                row.DefaultCellStyle.BackColor = Color.LightBlue;
                                break;
                            case MessageType.Notice:
                                row.DefaultCellStyle.BackColor = Color.LightGreen;
                                break;
                            default:
                                row.DefaultCellStyle.BackColor = Color.White;
                                break;
                        }
                    }
                }
            };
        }
        
        // 添加底部按钮
        private void AddButtonPanelButtons(Panel buttonPanel, DataGridView dataGridView)
        {
            Button markAsReadButton = new Button
            {
                Text = "标记选中为已读",
                Location = new Point(10, 10),
                Size = new Size(120, 30)
            };
            markAsReadButton.Click += (sender, e) => MarkSelectedMessagesAsRead(dataGridView);
            buttonPanel.Controls.Add(markAsReadButton);
            
            Button markAllAsReadButton = new Button
            {
                Text = "全部标记为已读",
                Location = new Point(140, 10),
                Size = new Size(120, 30)
            };
            markAllAsReadButton.Click += (sender, e) => MarkAllMessagesAsReadWithConfirmation(dataGridView);
            buttonPanel.Controls.Add(markAllAsReadButton);
            
            Button deleteButton = new Button
            {
                Text = "删除选中",
                Location = new Point(270, 10),
                Size = new Size(100, 30)
            };
            deleteButton.Click += (sender, e) => DeleteSelectedMessages(dataGridView);
            buttonPanel.Controls.Add(deleteButton);
        }
        
        // 处理消息双击事件
        private void HandleMessageDoubleClick(MessageData message)
        {
            try
            {
                if (message.BizType != BizType.无对应数据 && message.BizId > 0)
                {
                    NavigateToBusinessDocument(message.BizType, message.BizId);
                    MarkAsRead(message.Id);
                }
                else
                {
                    ShowMessageDetail(message);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理消息双击事件时发生错误");
                MessageBox.Show($"处理消息时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // 显示消息详情
        private void ShowMessageDetail(MessageData message)
        {
            var detailForm = new Form
            {
                Text = "消息详情",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            
            var tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6
            };
            
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            
            AddLabelValuePair(tableLayoutPanel, 0, "业务类型:", message.BizType.ToString());
            AddLabelValuePair(tableLayoutPanel, 1, "标题:", message.Title ?? "");
            AddLabelValuePair(tableLayoutPanel, 2, "发送者:", message.Sender ?? "");
            AddLabelValuePair(tableLayoutPanel, 3, "发送时间:", message.CreateTime.ToString());
            AddLabelValuePair(tableLayoutPanel, 4, "状态:", message.IsRead ? "已读" : "未读");
            
            var contentLabel = new Label
            {
                Text = "内容:",
                TextAlign = ContentAlignment.TopRight,
                Dock = DockStyle.Fill
            };
            tableLayoutPanel.Controls.Add(contentLabel, 0, 5);
            
            var contentTextBox = new TextBox
            {
                Text = message.Content ?? "",
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                ReadOnly = true
            };
            tableLayoutPanel.Controls.Add(contentTextBox, 1, 5);
            
            detailForm.Controls.Add(tableLayoutPanel);
            detailForm.ShowDialog();
        }
        
        // 添加标签值对
        private void AddLabelValuePair(TableLayoutPanel panel, int row, string labelText, string valueText)
        {
            var label = new Label
            {
                Text = labelText,
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill
            };
            panel.Controls.Add(label, 0, row);
            
            var value = new Label
            {
                Text = valueText,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };
            panel.Controls.Add(value, 1, row);
        }
        
        // 标记选中消息为已读
        private void MarkSelectedMessagesAsRead(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                int updatedCount = 0;
                
                foreach (DataGridViewRow row in dataGridView.SelectedRows)
                {
                    if (row.DataBoundItem is MessageData message && !message.IsRead)
                    {
                        MarkAsRead(message.Id);
                        updatedCount++;
                    }
                }
                
                if (updatedCount > 0)
                {
                    dataGridView.Refresh();
                    MessageBox.Show($"已成功将{updatedCount}条消息标记为已读", "操作成功", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        
        // 标记所有消息为已读（带确认）
        private void MarkAllMessagesAsReadWithConfirmation(DataGridView dataGridView)
        {
            var result = MessageBox.Show(
                "确定要将所有消息标记为已读吗？", "确认操作", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                MarkAllAsRead();
                dataGridView.Refresh();
                MessageBox.Show("已成功将所有消息标记为已读", "操作成功", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        // 删除选中消息
        private void DeleteSelectedMessages(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show(
                    $"确定要删除选中的{dataGridView.SelectedRows.Count}条消息吗？", "确认删除", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    var bindingList = dataGridView.DataSource as BindingList<MessageData>;
                    if (bindingList != null)
                    {
                        var selectedMessages = new List<MessageData>();
                        foreach (DataGridViewRow row in dataGridView.SelectedRows)
                        {
                            if (row.DataBoundItem is MessageData message)
                            {
                                selectedMessages.Add(message);
                            }
                        }
                        
                        foreach (var message in selectedMessages)
                        {
                            bindingList.Remove(message);
                            DeleteMessage(message.Id); // 使用DeleteMessage方法
                        }
                        
                        MessageBox.Show($"已成功删除{selectedMessages.Count}条消息", "操作成功", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        
        // 刷新消息列表
        private void RefreshMessageList(DataGridView dataGridView)
        {
            var messagesCopy = GetAllMessages().OrderByDescending(m => m.CreateTime).ToList();
            dataGridView.DataSource = new BindingList<MessageData>(messagesCopy);
        }
        
        // 过滤消息
        private void FilterMessages(DataGridView dataGridView, string filter)
        {
            var messagesCopy = GetAllMessages().OrderByDescending(m => m.CreateTime).ToList();
            
            List<MessageData> filteredMessages = filter switch
            {
                "未读消息" => messagesCopy.Where(m => !m.IsRead).ToList(),
                "已读消息" => messagesCopy.Where(m => m.IsRead).ToList(),
                "业务消息" => messagesCopy.Where(m => m.MessageType == MessageType.Business).ToList(),
                "系统通知" => messagesCopy.Where(m => m.MessageType == MessageType.Notice).ToList(),
                "解锁请求" => messagesCopy.Where(m => m.MessageType == MessageType.UnLockRequest).ToList(),
                _ => messagesCopy
            };
            
            dataGridView.DataSource = new BindingList<MessageData>(filteredMessages);
        }
        
        // 释放资源
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_messageCheckTimer != null)
                    {
                        _messageCheckTimer.Stop();
                        _messageCheckTimer.Dispose();
                    }
                    
                    if (_messageService != null)
                    {
                        try
                        {
                            _messageService.PopupMessageReceived -= OnPopupMessageReceived;
                            _messageService.BusinessMessageReceived -= OnBusinessMessageReceived;
                            _messageService.DepartmentMessageReceived -= OnDepartmentMessageReceived;
                            _messageService.BroadcastMessageReceived -= OnBroadcastMessageReceived;
                            _messageService.SystemNotificationReceived -= OnSystemNotificationReceived;
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "取消订阅消息服务事件时发生错误");
                        }
                    }
                }
                
                _disposed = true;
            }
        }
        
        ~EnhancedMessageManager()
        {
            Dispose(false);
        }
    }
}