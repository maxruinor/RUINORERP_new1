using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
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
    public partial class BusinessMessagePrompt : BaseMessagePrompt
    {
        private FlowLayoutPanel messageFlowLayoutPanel;
        private Timer messageTimer;
        
        /// <summary>
        /// 初始化组件
        /// </summary>
        protected override void InitializeComponents()
        {
            InitializeComponent();
            InitializeMessageComponents();
        }
        
        /// <summary>
        /// 设置发送者文本
        /// </summary>
        /// <param name="text">发送者名称</param>
        public override void SetSenderText(string text)
        {
            if (txtSender != null)
            {
                txtSender.Text = text;
            }
        }
        
        /// <summary>
        /// 设置主题文本
        /// </summary>
        /// <param name="text">消息主题</param>
        public override void SetSubjectText(string text)
        {
            if (txtSubject != null)
            {
                txtSubject.Text = text;
            }
        }

        public BusinessMessagePrompt() : base()
        {
        }
        
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="messageData">消息数据</param>
        public BusinessMessagePrompt(MessageData messageData) : base(messageData)
        {            
        }
        
        /// <summary>
        /// 初始化消息组件
        /// </summary>
        private void InitializeMessageComponents()
        {
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

        private void MessageTimer_Tick(object sender, EventArgs e)
        {
            // 模拟消息到达，显示新消息
            if (MessageData != null)
            {
                ShowMessage(MessageData);
            }
        }
        
        /// <summary>
        /// 更新消息显示
        /// 根据业务消息数据更新UI组件
        /// </summary>
        protected override void UpdateMessageDisplay()
        {
            try
            {
                if (MessageData == null) return;
                
                // 设置基本信息
                if (txtSender != null) txtSender.Text = MessageData.Sender ?? "系统";
                if (txtSubject != null) txtSubject.Text = MessageData.Title ?? "业务消息";
                if (txtContent != null) txtContent.Text = MessageData.Content;
                
                // 业务消息特定处理
               // this.Icon = Properties.Resources.Business;
                
                // 显示业务相关信息
                if (MessageData.BizType != BizType.无对应数据 && MessageData.BizId > 0)
                {
                    // 显示业务类型和ID信息
                    string bizInfo = $"业务类型: {MessageData.BizType}, 业务ID: {MessageData.BizId}";
                    Logger.LogDebug(bizInfo);
                    // 注意：txtBizInfo控件不存在，信息已记录到日志
                }
                
                // 处理业务数据
                if (MessageData.BizData != null)
                {
                    // 可以在这里解析和显示BizData中的业务信息
                    Logger.LogDebug($"显示业务数据: {MessageData.BizData}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "更新业务消息显示时发生错误");
            }
        }
        
        /// <summary>
        /// 显示消息标签
        /// 为业务消息创建并显示标签
        /// </summary>
        /// <param name="messageData">消息数据</param>
        private void ShowMessage(MessageData messageData)
        {            
            if (messageData == null) return;
            
            // 创建一个新的消息标签
            Label messageLabel = new Label
            {
                Text = messageData.Content ?? "空消息",
                AutoSize = true,
                Margin = new Padding(10),
                Font = new Font("Microsoft YaHei UI", 9),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 130, 180),
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            // 添加到消息流布局面板
            messageFlowLayoutPanel.Controls.Add(messageLabel);
            
            // 设置标签的点击事件
            messageLabel.Click += (sender, e) =>
            {
                this.BringToFront();
                this.Focus();
            };
            
            // 自动移除旧消息，保留最新的5条
            if (messageFlowLayoutPanel.Controls.Count > 5)
            {
                messageFlowLayoutPanel.Controls.RemoveAt(0);
            }
            
            // 调整窗体大小以适应消息
            this.Width = messageFlowLayoutPanel.Width + messageFlowLayoutPanel.Padding.Horizontal;
            this.Height = messageFlowLayoutPanel.Height + messageFlowLayoutPanel.Padding.Vertical;
            
            // 计时器启动，用于在一定时间后隐藏消息
            messageTimer.Start();
        }
        
        QueryParameter parameter { get; set; }

        private void BusinessMessagePrompt_Load(object sender, EventArgs e)
        {
            // 使用MessageData设置内容
            if (MessageData != null)
            {
                if (txtContent != null) txtContent.Text = MessageData.Content ?? string.Empty;
                if (lblSendTime != null) 
                    lblSendTime.Text = MessageData.SendTime.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                // 如果是业务消息，显示导航按钮
                if (btnNavigate != null && MessageData.BizType != BizType.无对应数据 && MessageData.BizId > 0)
                {
                    btnNavigate.Visible = true;
                    btnNavigate.Text = $"查看{MessageData.BizType}单据";
                }
                else if (btnNavigate != null)
                {
                    btnNavigate.Visible = false;
                }
            }
            
            // 确保窗体在显示时位于屏幕右下角
            this.SetDesktopLocation(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );

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

 

        /// <summary>
        /// 标记消息为已读
        /// 使用MessageManager更新消息状态
        /// </summary>
        private void MarkMessageAsRead()
        {            
            try
            {
                if (MessageData != null && !MessageData.IsRead)
                {
                    MessageData.MarkAsRead();
                    
                    // 异步更新消息状态
                    Task.Run(async () =>
                    {
                        try
                        {
                            // 使用EnhancedMessageManager来标记消息为已读
                            var messageManager = Startup.GetFromFac<EnhancedMessageManager>();
                            if (messageManager != null)
                            {
                                messageManager.MarkAsRead(MessageData.Id);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, "异步更新消息状态时发生错误");
                        }
                    });
                    
                    Logger.LogDebug($"消息已标记为已读: {MessageData.Id}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "标记消息为已读时发生错误");
            }
        }

        private void btnNavigate_Click(object sender, EventArgs e)
        {
            try
            {
                // 导航到具体业务单据
                // 使用主窗体中的增强版消息管理器实例
                var messageManager = Startup.GetFromFac<EnhancedMessageManager>();
                messageManager?.NavigateToBusinessDocument(MessageData.BizType, MessageData.BizId);

                // 标记消息为已读
                MarkMessageAsRead();
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "导航到业务单据时发生错误");
                // 显示错误消息
                MainForm.Instance.PrintInfoLog("导航到业务单据失败，请稍后重试。");
                this.Close();
            }
        }

        private void btnWaitReminder_Click(object sender, EventArgs e)
        {
            WaitReminder(sender);
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