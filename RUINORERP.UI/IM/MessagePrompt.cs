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
    public partial class MessagePrompt : Krypton.Toolkit.KryptonForm
    {
        private FlowLayoutPanel messageFlowLayoutPanel;
        private Timer messageTimer;

        public MessagePrompt()
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

        private void MessagePrompt_Load(object sender, EventArgs e)
        {
            txtContent.Text = Content;
            // 确保窗体在显示时位于屏幕右下角
            this.SetDesktopLocation(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
