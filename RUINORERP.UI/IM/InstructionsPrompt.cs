using Krypton.Toolkit;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.BaseForm;

using RUINORERP.UI.Common;

using RUINORERP.UI.UserCenter;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using RUINORERP.PacketSpec.Commands;

using RUINORERP.PacketSpec.Enums;
using Timer = System.Windows.Forms.Timer;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 指令流程性的提示窗口
    /// 后面消息队列中 会根据类型处理不同的消息指令
    ///public Queue<ReminderData> MessageList = new Queue<ReminderData>();
    /// </summary>
    public partial class InstructionsPrompt : Krypton.Toolkit.KryptonForm
    {

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

        private FlowLayoutPanel messageFlowLayoutPanel;
        private Timer messageTimer;

        private MessageData _messageData;
        public MessageData MessageData
        {
            get => _messageData;
            set => _messageData = value;
        }
        
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
        
        // 添加公共方法来隐藏操作按钮
        public void HideActionButtons()
        {
            if (btnAgree != null)
            {
                btnAgree.Visible = false;
            }
            
            if (btnRefuse != null)
            {
                btnRefuse.Visible = false;
            }
        }
        
        public InstructionsPrompt()
        {
            InitializeComponent();
            InitializeForm();
        }
        
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="messageService">消息服务</param>
        public InstructionsPrompt(MessageData messageData, ILogger logger, MessageService messageService)
        {
            InitializeComponent();
            _messageData = messageData;
            InitializeForm();
            
            // 更新显示的消息内容
            if (_messageData != null)
            {
                txtSender.Text = _messageData.Sender ?? "系统";
                txtSubject.Text = _messageData.Title ?? "系统通知";
                txtContent.Text = _messageData.Content;
                Content = _messageData.Content;
            }
        }
        
        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
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
        public string Title { get; internal set; }

        private void MessagePrompt_Load(object sender, EventArgs e)
        {
            txtContent.Text = Content;
            lblSendTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // 确保窗体在显示时位于屏幕右下角
            this.SetDesktopLocation(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );

            AddCommandForWait();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RefuseUnLock();
            // this.DialogResult = DialogResult.Cancel;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
#warning TODO: 这里需要完善具体逻辑，当前仅为占位
            //通知服务器解锁
            //ClientLockManagerCmd cmd = new ClientLockManagerCmd(CommandDirection.Send);
            //cmd.lockCmd = LockCmd.UNLOCK;
            //UnLockInfo lockRequest = new UnLockInfo();
            //lockRequest.BillID = _messageData.BizId;
            //lockRequest.LockedUserID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            //lockRequest.LockedUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name;
            //lockRequest.MenuID = 0;
            //lockRequest.PacketId = cmd.PacketId;
            //if (lockRequest.BillData == null && _messageData.BizData != null)
            //{
            //    lockRequest.BillData = _messageData.BizData as CommBillData;
            //}
            //cmd.RequestInfo = lockRequest;
            //MainForm.Instance.dispatcher.DispatchAsync(cmd, CancellationToken.None);
            this.DialogResult = DialogResult.OK;
            this.Close();
            }
        

        private void MenuPowerHelper_OnSetQueryConditionsDelegate(object QueryDto, UserCenter.QueryParameter nodeParameter)
        {
            if (QueryDto == null)
            {
                return;
            }
            //查询条件给值前先将条件清空
            foreach (var item in nodeParameter.queryFilter.QueryFields)
            {
                if (item.FKTableName.IsNotEmptyOrNull() && item.IsRelated)
                {
                    QueryDto.SetPropertyValue(item.FieldName, -1L);
                    continue;
                }
                if (item.FieldPropertyInfo.PropertyType.IsGenericType && item.FieldPropertyInfo.PropertyType.GetBaseType().Name == "DateTime")
                {
                    QueryDto.SetPropertyValue(item.FieldName, null);
                    if (QueryDto.ContainsProperty(item.FieldName + "_Start"))
                    {
                        QueryDto.SetPropertyValue(item.FieldName + "_Start", null);
                    }
                    if (QueryDto.ContainsProperty(item.FieldName + "_End"))
                    {
                        QueryDto.SetPropertyValue(item.FieldName + "_End", null);
                    }
                    continue;
                }

            }



            //传入查询对象的实例，
            foreach (ConditionalModel item in nodeParameter.conditionals)
            {
                if (item.ConditionalType == ConditionalType.Equal)
                {
                    switch (item.CSharpTypeName)
                    {
                        case "int":
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToInt());
                            break;
                        case "long":
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToLong());
                            break;
                        case "bool":
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToBool());
                            break;
                        default:
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue);
                            break;
                    }
                }
            }



        }

        private void btnWaitReminder_Click(object sender, EventArgs e)
        {
            WaitReminder(sender);
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="billid"></param>
        private void RefuseUnLock()
        {
#warning TODO: 这里需要完善具体逻辑，当前仅为占位

            ////谁拒绝谁的什么请求
            //ClientLockManagerCmd cmd = new ClientLockManagerCmd(CommandDirection.Send);
            //cmd.lockCmd = LockCmd.RefuseUnLock;
            //RefuseUnLockInfo lockRequest = new RefuseUnLockInfo();
            //lockRequest.BillID = _messageData.BizId;
            //if (lockRequest.BillData == null && _messageData.BizData != null)
            //{
            //    lockRequest.BillData = _messageData.BizData as CommBillData;
            //}
            //lockRequest.RefuseUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name;
            //lockRequest.RefuseUserID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

            //lockRequest.RequestUserName = _messageData.Sender;
            //lockRequest.RequestUserID = _messageData.SenderId;

            //lockRequest.PacketId = cmd.PacketId;
            //拒绝谁？
            //cmd.RequestInfo = lockRequest;
            //MainForm.Instance.dispatcher.DispatchAsync(cmd, CancellationToken.None);
            //cmd.LockChanged += (sender, e) =>
            //{
            //    MessageBox.Show("已经向锁定者发送了解锁请求。等待结果中");
            //};
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

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
