using Krypton.Toolkit;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;

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

namespace RUINORERP.UI.IM
{
    public partial class BusinessMessagePrompt : KryptonForm
    {
        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

        private FlowLayoutPanel messageFlowLayoutPanel;
        private Timer messageTimer;

        public ReminderData ReminderData { get; set; }
    
        
        // 添加公共方法来设置发送者文本
        public void SetSenderText(string text)
        {
            if (txtSender != null)
            {
                txtSender.Text = text;
            }
        }
        
        // 添加公共方法来设置主题文本
        public void SetSubjectText(string text)
        {
            if (txtSubject != null)
            {
                txtSubject.Text = text;
            }
        }

        public BusinessMessagePrompt()
        {
            InitializeComponent();
            // 设置窗体启动位置为手动
            this.StartPosition = FormStartPosition.Manual;

            // 设置窗体的初始位置为屏幕右下角
            this.SetDesktopLocation(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );

            // 初始化消息流布局面板
            messageFlowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            this.Controls.Add(messageFlowLayoutPanel);

            // 初始化消息计时器
            messageTimer = new Timer
            {
                Interval = 5000 // 消息显示的时间间隔，单位毫秒
            };
            messageTimer.Tick += MessageTimer_Tick;
        }

        public string Content { get; set; } = string.Empty;

        private void MessageTimer_Tick(object sender, EventArgs e)
        {
            // 模拟消息到达，显示新消息
            ShowMessage(Content);
        }

        private void ShowMessage(string message)
        {
            // 创建一个新的消息标签
            Label messageLabel = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = SystemColors.ControlText,
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.FixedSingle
            };

            // 将消息标签添加到流布局面板
            messageFlowLayoutPanel.Controls.Add(messageLabel);

            // 调整窗体大小以适应消息
            this.Width = messageFlowLayoutPanel.Width + messageFlowLayoutPanel.Padding.Horizontal;
            this.Height = messageFlowLayoutPanel.Height + messageFlowLayoutPanel.Padding.Vertical;

            // 计时器启动，用于在一定时间后隐藏消息
            messageTimer.Start();
        }

        QueryParameter parameter { get; set; }

        private void BusinessMessagePrompt_Load(object sender, EventArgs e)
        {
            txtContent.Text = Content;
            lblSendTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // 确保窗体在显示时位于屏幕右下角
            this.SetDesktopLocation(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );

            // 如果是业务消息，显示导航按钮
            if (ReminderData?.BizType != BizType.无对应数据 && ReminderData?.BizPrimaryKey > 0)
            {
                btnNavigate.Visible = true;
                btnNavigate.Text = $"查看{ReminderData.BizType}单据";
            }
            else
            {
                btnNavigate.Visible = false;
            }

            AddCommandForWait();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MarkMessageAsRead();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            MarkMessageAsRead();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnNavigate_Click(object sender, EventArgs e)
        {
            try
            {
                // 导航到具体业务单据
                // 使用主窗体中的增强版消息管理器实例
                var messageManager = MainForm.Instance.GetMessageManager() as EnhancedMessageManager;
                messageManager?.NavigateToBusinessDocument(ReminderData.BizType, ReminderData.BizPrimaryKey);

                // 标记消息为已读
                MarkMessageAsRead();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导航到业务单据时发生错误: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnWaitReminder_Click(object sender, EventArgs e)
        {
            WaitReminder(sender);
        }

        /// <summary>
        /// 标记消息为已读
        /// </summary>
        private void MarkMessageAsRead()
        {
            // 标记消息为已读
            ReminderData.IsRead = true;
            // 可以在这里添加更新服务器状态的逻辑
        }

        /// <summary>
        /// 响应服务器，更新消息状态
        /// </summary>
        /// <param name="isRead">消息是否已读</param>
        /// <param name="interval">提醒间隔</param>
        private void ResponseToServer(bool isRead, int interval = 20)
        {
            #warning  todo by watson
            //回复服务器
            //ClientResponseData response = new ClientResponseData();
            //response.BizPrimaryKey = ReminderData.BizPrimaryKey;
            //response.Status = status;
            //response.RemindInterval = interval;
            ////向服务器推送工作流提醒的列表 typeof(T).Name
            //OriginalData beatDataDel = ClientDataBuilder.工作流提醒回复();
            //MainForm.Instance.ecs.AddSendData(beatDataDel);
        }

        #region 生成稍候提醒的指令

        private void AddCommandForWait()
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

            this.kryptonContextMenu1.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            contextMenuItems});

            contextMenuItems.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            menuItem5分钟后,
            menuItem10分钟后,
            menuItem一小时后,
            menuItem一天后
            });
            if (this.kryptonContextMenu1.Items.Count == 0)
            {
                this.kryptonContextMenu1.Items.Add(contextMenuItems);
            }
        }

        #endregion

        private void kryptonCommandWait_Execute(object sender, EventArgs e)
        {
            WaitReminder(sender);
        }

        private void WaitReminder(object sender)
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
            // 将消息标记为等待提醒状态，使用IsRead=false表示未读
            ResponseToServer(false, interval);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}