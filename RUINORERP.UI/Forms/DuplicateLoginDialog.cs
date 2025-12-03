using RUINORERP.PacketSpec.Models.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RUINORERP.UI.Forms
{
    /// <summary>
    /// 重复登录确认对话框
    /// 用于处理重复登录情况下的用户选择确认
    /// </summary>
    public partial class DuplicateLoginDialog : Krypton.Toolkit.KryptonForm
    {
        private DuplicateLoginAction _selectedAction = DuplicateLoginAction.Cancel;
        private readonly DuplicateLoginResult _duplicateLoginResult;

        /// <summary>
        /// 获取用户选择的重复登录处理方式
        /// </summary>
        public DuplicateLoginAction SelectedAction => _selectedAction;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="duplicateLoginResult">重复登录结果信息</param>
        public DuplicateLoginDialog(DuplicateLoginResult duplicateLoginResult)
        {
            _duplicateLoginResult = duplicateLoginResult ?? throw new ArgumentNullException(nameof(duplicateLoginResult));
            InitializeComponent();
            InitializeDialogContent();
        }

        /// <summary>
        /// 初始化对话框内容
        /// </summary>
        private void InitializeDialogContent()
        {
            // 设置窗体属性
            this.Text = "重复登录确认";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 设置标题
            lblTitle.Text = "登录冲突";
            lblTitle.Font = new Font("微软雅黑", 12f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);

            // 设置消息内容
            lblMessage.Text = _duplicateLoginResult.Message ?? "您的账号已在其他地方登录，请选择处理方式：";
            lblMessage.Font = new Font("微软雅黑", 9f);
            lblMessage.ForeColor = Color.FromArgb(85, 85, 85);

            // 设置现有会话信息
            DisplayExistingSessions();

            // 设置按钮
            SetupActionButtons();

            // 设置说明文本
            lblInstruction.Text = "请选择处理方式后点击确认继续。";
            lblInstruction.Font = new Font("微软雅黑", 8.5f);
            lblInstruction.ForeColor = Color.FromArgb(128, 128, 128);
        }

        /// <summary>
        /// 显示现有会话信息
        /// </summary>
        private void DisplayExistingSessions()
        {
            if (_duplicateLoginResult.ExistingSessions?.Count > 0)
            {
                var sessions = _duplicateLoginResult.ExistingSessions;
                lvExistingSessions.Items.Clear();

                foreach (var session in sessions)
                {
                    var item = new ListViewItem(new string[]
                    {
                        session.SessionId ?? "未知",
                        session.LoginTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        session.ClientIp ?? "未知",
                        session.DeviceInfo ?? "未知设备",
                        session.StatusDescription ?? "未知状态"
                    })
                    {
                        Tag = session,
                        BackColor = session.IsLocal ? Color.FromArgb(230, 255, 230) : Color.White
                    };

                    lvExistingSessions.Items.Add(item);
                }

                // 调整列宽
                lvExistingSessions.Columns[0].Width = 120; // SessionId
                lvExistingSessions.Columns[1].Width = 140; // LoginTime
                lvExistingSessions.Columns[2].Width = 100; // ClientIp
                lvExistingSessions.Columns[3].Width = 120; // DeviceInfo
                lvExistingSessions.Columns[4].Width = 100; // StatusDescription
            }
        }

        /// <summary>
        /// 设置操作按钮
        /// </summary>
        private void SetupActionButtons()
        {
            // 强制对方下线按钮
            btnForceOffline.Text = "强制对方下线";
            btnForceOffline.DialogResult = DialogResult.None;
            btnForceOffline.Click += BtnForceOffline_Click;

            // 自己下线按钮
            btnOfflineSelf.Text = "自己下线";
            btnOfflineSelf.DialogResult = DialogResult.None;
            btnOfflineSelf.Click += BtnOfflineSelf_Click;

            // 取消登录按钮
            btnCancelLogin.Text = "取消登录";
            btnCancelLogin.DialogResult = DialogResult.None;
            btnCancelLogin.Click += BtnCancelLogin_Click;

            // 确认按钮
            btnConfirm.Text = "确认";
            btnConfirm.DialogResult = DialogResult.OK;
            btnConfirm.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
            btnConfirm.StateCommon.Back.Color1 = Color.FromArgb(52, 168, 83);
            btnConfirm.StateCommon.Content.ShortText.Color1 = Color.White;
            btnConfirm.Click += BtnConfirm_Click;

            // 设置按钮工具提示
            SetupButtonTooltips();
        }

        /// <summary>
        /// 设置按钮工具提示
        /// </summary>
        private void SetupButtonTooltips()
        {
            btnForceOffline.ToolTipValues.Description = "将其他地方的登录踢下线，保持当前登录";
            btnForceOffline.ToolTipValues.EnableToolTips = true;
            btnForceOffline.ToolTipValues.Heading = "强制对方下线";

            btnOfflineSelf.ToolTipValues.Description = "取消当前登录，保持其他地方的登录状态";
            btnOfflineSelf.ToolTipValues.EnableToolTips = true;
            btnOfflineSelf.ToolTipValues.Heading = "自己下线";

            btnCancelLogin.ToolTipValues.Description = "取消本次登录操作";
            btnCancelLogin.ToolTipValues.EnableToolTips = true;
            btnCancelLogin.ToolTipValues.Heading = "取消登录";

            btnConfirm.ToolTipValues.Description = "执行选择的操作并继续";
            btnConfirm.ToolTipValues.EnableToolTips = true;
            btnConfirm.ToolTipValues.Heading = "确认";
        }

        #region 事件处理

        /// <summary>
        /// 强制对方下线按钮点击事件
        /// </summary>
        private void BtnForceOffline_Click(object sender, EventArgs e)
        {
            _selectedAction = DuplicateLoginAction.ForceOfflineOthers;
            UpdateButtonSelection(btnForceOffline);
        }

        /// <summary>
        /// 自己下线按钮点击事件
        /// </summary>
        private void BtnOfflineSelf_Click(object sender, EventArgs e)
        {
            _selectedAction = DuplicateLoginAction.OfflineSelf;
            UpdateButtonSelection(btnOfflineSelf);
        }

        /// <summary>
        /// 取消登录按钮点击事件
        /// </summary>
        private void BtnCancelLogin_Click(object sender, EventArgs e)
        {
            _selectedAction = DuplicateLoginAction.Cancel;
            UpdateButtonSelection(btnCancelLogin);
        }

        /// <summary>
        /// 确认按钮点击事件
        /// </summary>
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (_selectedAction == DuplicateLoginAction.Cancel)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        /// <summary>
        /// 更新按钮选择状态
        /// </summary>
        /// <param name="selectedButton">选中的按钮</param>
        private void UpdateButtonSelection(Krypton.Toolkit.KryptonButton selectedButton)
        {
            // 重置所有按钮状态
            btnForceOffline.StateCommon.Back.Color1 = Color.FromArgb(240, 240, 240);
            btnForceOffline.StateCommon.Content.ShortText.Color1 = Color.FromArgb(51, 51, 51);
            
            btnOfflineSelf.StateCommon.Back.Color1 = Color.FromArgb(240, 240, 240);
            btnOfflineSelf.StateCommon.Content.ShortText.Color1 = Color.FromArgb(51, 51, 51);
            
            btnCancelLogin.StateCommon.Back.Color1 = Color.FromArgb(240, 240, 240);
            btnCancelLogin.StateCommon.Content.ShortText.Color1 = Color.FromArgb(51, 51, 51);

            // 设置选中按钮状态
            if (selectedButton != null)
            {
                selectedButton.StateCommon.Back.Color1 = Color.FromArgb(52, 168, 83);
                selectedButton.StateCommon.Content.ShortText.Color1 = Color.White;
            }
        }

        #endregion

        /// <summary>
        /// 窗体加载时设置默认选择
        /// </summary>
        private void DuplicateLoginDialog_Load(object sender, EventArgs e)
        {
            // 默认选择强制对方下线
            _selectedAction = DuplicateLoginAction.ForceOfflineOthers;
            UpdateButtonSelection(btnForceOffline);
        }

        /// <summary>
        /// 处理键盘事件
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return true;
            }
            else if (keyData == Keys.Enter)
            {
                BtnConfirm_Click(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}