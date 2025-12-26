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
using RUINORERP.UI;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 指令流程性的提示窗口
    /// 后面消息队列中 会根据类型处理不同的消息指令
    ///public Queue<ReminderData> MessageList = new Queue<ReminderData>();
    /// </summary>
    public partial class InstructionsPrompt : BaseMessagePrompt
    {
        private FlowLayoutPanel messageFlowLayoutPanel;
        private Timer messageTimer;

        /// <summary>
        /// 初始化组件
        /// </summary>
        protected override void InitializeComponents()
        {
            InitializeComponent();
            InitializeForm();
        }

        /// <summary>
        /// 更新消息显示
        /// </summary>
        protected override void UpdateMessageDisplay()
        {
            if (MessageData != null)
            {
                txtSender.Text = MessageData.SenderName ?? "系统";
                txtSubject.Text = MessageData.Title ?? "系统通知";
                txtContent.Text = MessageData.Content;
                Content = MessageData.Content;
                lblSendTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
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
            : base()
        {
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="messageService">消息服务</param>
        public InstructionsPrompt(MessageData messageData, ILogger logger, MessageService messageService)
            : base(messageData)
        {
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
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

            // 移除对AddCommandForWait方法的调用
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消当前操作，不发送任何解锁请求
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageData == null)
                {
                    throw new ArgumentNullException(nameof(MessageData), "消息数据为空，无法处理解锁请求");
                }

                // 获取ClientLockManagementService实例
                var lockManagementService = Startup.GetFromFac<ClientLockManagementService>();
                if (lockManagementService == null)
                {
                    throw new InvalidOperationException("无法获取锁管理服务实例");
                }

                // 调用同意解锁方法
                await lockManagementService.AgreeUnlockAsync(
                    MessageData.BizId,
                    0, // MenuID
                    MessageData.SenderId, // 请求解锁的用户ID
                    MessageData.SenderName // 请求解锁的用户名
                );

                MessageBox.Show("已向服务器发送了同意解锁请求", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "处理同意解锁请求时发生错误");
                MessageBox.Show($"处理同意解锁请求时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// 拒绝解锁请求
        /// </summary>
        private async Task RefuseUnLockAsync()
        {
            try
            {
                if (MessageData == null)
                {
                    throw new ArgumentNullException(nameof(MessageData), "消息数据为空，无法处理拒绝解锁请求");
                }

                // 获取ClientLockManagementService实例
                var lockManagementService = Startup.GetFromFac<ClientLockManagementService>();
                if (lockManagementService == null)
                {
                    throw new InvalidOperationException("无法获取锁管理服务实例");
                }

                // 调用拒绝解锁方法
                await lockManagementService.RefuseUnlockAsync(
                    MessageData.BizId,
                    0, // MenuID
                    MessageData.SenderId, // 请求解锁的用户ID
                    MessageData.SenderName // 请求解锁的用户名
                );

                MessageBox.Show("已向锁定者发送了拒绝解锁请求", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "处理拒绝解锁请求时发生错误");
                MessageBox.Show($"处理拒绝解锁请求时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 生成稍候提醒的指令

        private void AddCommandForWait()
        {
            // 移除对基类方法的调用，因为基类可能没有这个方法
            // 或者根据实际情况实现稍候提醒的逻辑
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
