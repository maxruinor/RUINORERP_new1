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
using System.Reflection;
using RUINORERP.Business.BizMapperService;
using SqlSugar;
using System.Drawing;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Model;
using RUINORERP.UI.Common;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 增强版消息管理器 - 负责处理所有与消息相关的功能，支持业务导航和增强UI
    /// </summary>
    public class EnhancedMessageManager : MessageManager
    {
        // 不再需要BizTypeMapper，使用静态的EntityMappingHelper代替
        private readonly ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="notificationService">通知服务</param>
        public EnhancedMessageManager(ILogger logger = null, NotificationService notificationService = null) 
            : base(logger, notificationService)
        {
            // EntityMappingHelper已经在应用程序启动时初始化，无需在此处创建
            _logger = logger;
        }

        /// <summary>
        /// 处理业务消息并支持导航到具体业务单据
        /// </summary>
        public void ProcessBusinessMessage(ReminderData message)
        {
            try
            {
                // 根据消息类型显示不同的提示框
                switch (message.messageCmd)
                {
                    case MessageCmdType.UnLockRequest:
                        ShowUnlockRequestPrompt(message);
                        break;
                    case MessageCmdType.Notice:
                        ShowNoticePrompt(message);
                        break;
                    case MessageCmdType.Business:
                        ShowBusinessMessagePrompt(message);
                        break;
                    default:
                        ShowDefaultMessagePrompt(message);
                        break;
                }

                // 添加到消息队列
                AddMessage(message);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理业务消息时发生异常");
            }
        }

        /// <summary>
        /// 显示解锁请求提示
        /// </summary>
        private void ShowUnlockRequestPrompt(ReminderData message)
        {
            var prompt = new InstructionsPrompt();
            prompt.ReminderData = message;
            // 使用公共属性而不是直接访问控件
            prompt.SetSenderText(message.SenderEmployeeName ?? "系统");
            prompt.SetSubjectText($"请求解锁【{message.BizType}】");
            prompt.Content = message.ReminderContent;

            // 在UI线程上显示
            if (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Invoke(new Action(() =>
                {
                    prompt.Show();
                    prompt.TopMost = true;
                }));
            }
        }

        /// <summary>
        /// 显示通知提示
        /// </summary>
        private void ShowNoticePrompt(ReminderData message)
        {
            var prompt = new InstructionsPrompt();
            prompt.ReminderData = message;
            // 使用公共属性而不是直接访问控件
            prompt.SetSenderText(message.SenderEmployeeName ?? "系统");
            prompt.SetSubjectText(message.RemindSubject ?? "系统通知");
            prompt.Content = message.ReminderContent;
            prompt.HideActionButtons(); // 隐藏操作按钮

            // 在UI线程上显示
            if (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Invoke(new Action(() =>
                {
                    prompt.Show();
                    prompt.TopMost = true;
                }));
            }
        }

        /// <summary>
        /// 显示默认消息提示
        /// </summary>
        private void ShowDefaultMessagePrompt(ReminderData message)
        {
            var prompt = new MessagePrompt();
            prompt.ReminderData = message;
            // 使用公共属性而不是直接访问控件
            prompt.SetSenderText(message.SenderEmployeeName ?? "系统");

            if (!string.IsNullOrEmpty(message.RemindSubject))
            {
                prompt.SetSubjectText($"【{message.BizType}】{message.RemindSubject}");
            }
            else
            {
                prompt.SetSubjectText("请求协助");
            }

            prompt.Content = message.ReminderContent;

            // 在UI线程上显示
            if (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Invoke(new Action(() =>
                {
                    prompt.Show();
                    prompt.TopMost = true;
                }));
            }
        }

        /// <summary>
        /// 显示业务消息提示并支持导航
        /// </summary>
        private void ShowBusinessMessagePrompt(ReminderData message)
        {
            var prompt = new BusinessMessagePrompt();
            prompt.ReminderData = message;
            // 不再设置BizTypeMapper
            // 使用公共属性而不是直接访问控件
            prompt.SetSenderText(message.SenderEmployeeName ?? "系统");
            prompt.SetSubjectText(message.RemindSubject ?? "业务消息");
            prompt.Content = message.ReminderContent;

            // 在UI线程上显示
            if (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Invoke(new Action(() =>
                {
                    prompt.Show();
                    prompt.TopMost = true;
                }));
            }
        }

        /// <summary>
        /// 导航到具体业务单据
        /// </summary>
        public void NavigateToBusinessDocument(BizType bizType, long bizId)
        {
            try
            {
                // 获取业务类型对应的实体类型
                var tableType = EntityMappingHelper.GetEntityType(bizType);
                if (tableType == null)
                {
                    _logger?.LogWarning($"未找到业务类型 {bizType} 对应的实体类型");
                    return;
                }

                // 获取主键字段名
                var primaryKeyName = BaseUIHelper.GetEntityPrimaryKey(tableType);

                // 构建查询条件
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

                // 查找对应的菜单信息
                var menuInfo = FindMenuInfoForEntity(tableType.Name);
                if (menuInfo == null)
                {
                    _logger?.LogWarning($"未找到实体 {tableType.Name} 对应的菜单信息");
                    return;
                }

                // 执行菜单事件，打开业务单据
                var menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                var queryParameter = new RUINORERP.UI.UserCenter.QueryParameter
                {
                    conditionals = queryConditions,
                    tableType = tableType
                };

                // 创建实体实例
                var instance = Activator.CreateInstance(tableType);

                // 执行菜单事件
                menuPowerHelper.ExecuteEvents(menuInfo, instance, queryParameter);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"导航到业务单据时发生异常: BizType={bizType}, BizId={bizId}");
            }
        }

        /// <summary>
        /// 根据实体名称查找菜单信息
        /// </summary>
        private tb_MenuInfo FindMenuInfoForEntity(string entityName)
        {
            // 通过MainForm获取菜单列表
            var mainForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f is MainForm) as MainForm;
            if (mainForm?.MenuList != null)
            {
                return mainForm.MenuList.FirstOrDefault(m => 
                    m.IsVisble && 
                    m.EntityName == entityName && 
                    m.BIBaseForm == "BaseBillEditGeneric`2");
            }

            return null;
        }

        /// <summary>
        /// 显示增强版消息列表
        /// </summary>
        public void ShowEnhancedMessageList()
        {
            try
            {
                // 创建消息列表窗口
                Form messageListForm = new Form
                {
                    Text = "消息中心",
                    Size = new System.Drawing.Size(1000, 700),
                    StartPosition = FormStartPosition.CenterScreen,
                    Icon = SystemIcons.Information
                };

                // 获取所有消息的副本
                var messagesCopy = GetAllMessages().OrderByDescending(m => m.CreateTime).ToList();

                // 创建顶部工具栏
                ToolStrip toolStrip = new ToolStrip
                {
                    Dock = DockStyle.Top
                };

                // 创建数据网格视图
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

                // 添加刷新按钮
                ToolStripButton refreshButton = new ToolStripButton("刷新", null, (s, e) => RefreshMessageList(dataGridView));
                refreshButton.ToolTipText = "刷新消息列表";
                toolStrip.Items.Add(refreshButton);

                // 添加分隔符
                toolStrip.Items.Add(new ToolStripSeparator());

                // 添加过滤器标签
                ToolStripLabel filterLabel = new ToolStripLabel("显示:");
                toolStrip.Items.Add(filterLabel);

                // 添加过滤器下拉框
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
                toolStrip.Items.Add(filterComboBox);

                // 添加列
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
                    DataPropertyName = "RemindSubject", 
                    Width = 200 
                });

                dataGridView.Columns.Add(new DataGridViewTextBoxColumn
                { 
                    Name = "Content", 
                    HeaderText = "内容", 
                    DataPropertyName = "ReminderContent", 
                    Width = 300, 
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
                });

                dataGridView.Columns.Add(new DataGridViewTextBoxColumn
                { 
                    Name = "Sender", 
                    HeaderText = "发送者", 
                    DataPropertyName = "SenderEmployeeName", 
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

                // 设置数据源
                var bindingList = new BindingList<ReminderData>(messagesCopy);
                dataGridView.DataSource = bindingList;

                // 设置行样式
                dataGridView.RowPrePaint += (s, e) =>
                {
                    if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
                    {
                        var row = dataGridView.Rows[e.RowIndex];
                        if (row.DataBoundItem is ReminderData message)
                        {
                            // 未读消息显示为粗体
                            if (!message.IsRead)
                            {
                                row.DefaultCellStyle.Font = new System.Drawing.Font(dataGridView.Font, System.Drawing.FontStyle.Bold);
                            }
                            else
                            {
                                row.DefaultCellStyle.Font = new System.Drawing.Font(dataGridView.Font, System.Drawing.FontStyle.Regular);
                            }

                            // 不同类型的消息显示不同颜色
                            switch (message.messageCmd)
                            {
                                case MessageCmdType.UnLockRequest:
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                                    break;
                                case MessageCmdType.Business:
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                                    break;
                                case MessageCmdType.Notice:
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                                    break;
                                default:
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                                    break;
                            }
                        }
                    }
                };

                // 双击事件 - 导航到业务单据或显示详细信息
                dataGridView.CellDoubleClick += (s, e) =>
                {
                    if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
                    {
                        var row = dataGridView.Rows[e.RowIndex];
                        if (row.DataBoundItem is ReminderData message)
                        {
                            HandleMessageDoubleClick(message);
                        }
                    }
                };

                // 创建底部按钮面板
                Panel buttonPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    BorderStyle = BorderStyle.FixedSingle
                };

                // 创建标记已读按钮
                Button markAsReadButton = new Button
                {
                    Text = "标记选中为已读",
                    Location = new System.Drawing.Point(10, 10),
                    Size = new System.Drawing.Size(120, 30)
                };
                markAsReadButton.Click += (sender, e) => MarkSelectedMessagesAsRead(dataGridView);
                buttonPanel.Controls.Add(markAsReadButton);

                // 创建标记全部已读按钮
                Button markAllAsReadButton = new Button
                {
                    Text = "全部标记为已读",
                    Location = new System.Drawing.Point(140, 10),
                    Size = new System.Drawing.Size(120, 30)
                };
                markAllAsReadButton.Click += (sender, e) => MarkAllMessagesAsReadWithConfirmation(dataGridView);
                buttonPanel.Controls.Add(markAllAsReadButton);

                // 创建删除按钮
                Button deleteButton = new Button
                {
                    Text = "删除选中",
                    Location = new System.Drawing.Point(270, 10),
                    Size = new System.Drawing.Size(100, 30)
                };
                deleteButton.Click += (sender, e) => DeleteSelectedMessages(dataGridView);
                buttonPanel.Controls.Add(deleteButton);

                // 过滤消息显示
                filterComboBox.SelectedIndexChanged += (sender, e) =>
                {
                    FilterMessages(dataGridView, filterComboBox.SelectedItem.ToString(), bindingList);
                };

                // 添加控件
                messageListForm.Controls.Add(dataGridView);
                messageListForm.Controls.Add(buttonPanel);
                messageListForm.Controls.Add(toolStrip);

                // 显示对话框
                messageListForm.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "显示增强版消息列表时发生异常");
                MessageBox.Show("显示消息列表时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 处理消息双击事件
        /// </summary>
        private void HandleMessageDoubleClick(ReminderData message)
        {
            try
            {
                // 如果是业务消息且有业务主键，导航到具体业务单据
                if (message.BizType != BizType.无对应数据 && message.BizPrimaryKey > 0)
                {
                    NavigateToBusinessDocument(message.BizType, message.BizPrimaryKey);
                    // 标记为已读
                    message.IsRead = true;
                    OnMessageStatusChanged();
                }
                else
                {
                    // 显示消息详细信息
                    ShowMessageDetail(message);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理消息双击事件时发生异常");
                MessageBox.Show($"处理消息时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示消息详细信息
        /// </summary>
        private void ShowMessageDetail(ReminderData message)
        {
            var detailForm = new Form
            {
                Text = "消息详情",
                Size = new System.Drawing.Size(600, 400),
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

            // 添加标签和值
            AddLabelValuePair(tableLayoutPanel, 0, "业务类型:", message.BizType.ToString());
            AddLabelValuePair(tableLayoutPanel, 1, "标题:", message.RemindSubject ?? "");
            AddLabelValuePair(tableLayoutPanel, 2, "发送者:", message.SenderEmployeeName ?? "");
            AddLabelValuePair(tableLayoutPanel, 3, "发送时间:", message.CreateTime.ToString());
            AddLabelValuePair(tableLayoutPanel, 4, "状态:", message.IsRead ? "已读" : "未读");

            // 内容区域
            var contentLabel = new Label
            {
                Text = "内容:",
                TextAlign = ContentAlignment.TopRight,
                Dock = DockStyle.Fill
            };
            tableLayoutPanel.Controls.Add(contentLabel, 0, 5);

            var contentTextBox = new TextBox
            {
                Text = message.ReminderContent ?? "",
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                ReadOnly = true
            };
            tableLayoutPanel.Controls.Add(contentTextBox, 1, 5);

            detailForm.Controls.Add(tableLayoutPanel);
            detailForm.ShowDialog();
        }

        /// <summary>
        /// 添加标签和值对到表格布局
        /// </summary>
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

        /// <summary>
        /// 标记选中消息为已读
        /// </summary>
        private void MarkSelectedMessagesAsRead(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                var updatedCount = 0;

                foreach (DataGridViewRow row in dataGridView.SelectedRows)
                {
                    if (row.DataBoundItem is ReminderData message && !message.IsRead)
                    {
                        message.IsRead = true;
                        updatedCount++;
                    }
                }

                if (updatedCount > 0)
                {
                    dataGridView.Refresh();
                    OnMessageStatusChanged();
                    MessageBox.Show($"已成功将{updatedCount}条消息标记为已读", "操作成功", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 标记所有消息为已读（带确认）
        /// </summary>
        private void MarkAllMessagesAsReadWithConfirmation(DataGridView dataGridView)
        {
            var result = MessageBox.Show(
                "确定要将所有消息标记为已读吗？", "确认操作", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MarkAllMessagesAsRead();
                dataGridView.Refresh();
                MessageBox.Show("已成功将所有消息标记为已读", "操作成功", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 删除选中消息
        /// </summary>
        private void DeleteSelectedMessages(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show(
                    $"确定要删除选中的{dataGridView.SelectedRows.Count}条消息吗？", "确认删除", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var bindingSource = dataGridView.DataSource as BindingSource;
                    var bindingList = dataGridView.DataSource as BindingList<ReminderData>;

                    if (bindingList != null)
                    {
                        // 从绑定列表中移除选中的消息
                        var selectedMessages = new List<ReminderData>();
                        foreach (DataGridViewRow row in dataGridView.SelectedRows)
                        {
                            if (row.DataBoundItem is ReminderData message)
                            {
                                selectedMessages.Add(message);
                            }
                        }

                        foreach (var message in selectedMessages)
                        {
                            bindingList.Remove(message);
                        }

                        MessageBox.Show($"已成功删除{selectedMessages.Count}条消息", "操作成功", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        /// <summary>
        /// 刷新消息列表
        /// </summary>
        private void RefreshMessageList(DataGridView dataGridView)
        {
            var messagesCopy = GetAllMessages().OrderByDescending(m => m.CreateTime).ToList();
            var bindingList = new BindingList<ReminderData>(messagesCopy);
            dataGridView.DataSource = bindingList;
        }

        /// <summary>
        /// 过滤消息显示
        /// </summary>
        private void FilterMessages(DataGridView dataGridView, string filter, BindingList<ReminderData> bindingList)
        {
            var messagesCopy = GetAllMessages().OrderByDescending(m => m.CreateTime).ToList();

            List<ReminderData> filteredMessages = filter switch
            {
                "未读消息" => messagesCopy.Where(m => !m.IsRead).ToList(),
                "已读消息" => messagesCopy.Where(m => m.IsRead).ToList(),
                "业务消息" => messagesCopy.Where(m => m.messageCmd == MessageCmdType.Business).ToList(),
                "系统通知" => messagesCopy.Where(m => m.messageCmd == MessageCmdType.Notice).ToList(),
                "解锁请求" => messagesCopy.Where(m => m.messageCmd == MessageCmdType.UnLockRequest).ToList(),
                _ => messagesCopy
            };

            // 应用过滤
            dataGridView.DataSource = new BindingList<ReminderData>(filteredMessages);
        }
    }
}