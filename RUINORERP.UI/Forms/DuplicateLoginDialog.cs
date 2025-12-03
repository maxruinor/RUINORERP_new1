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
            this.Size = new Size(600, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 设置标题
            lblTitle.Values.Text = "⚠️ 登录冲突";

            // 设置消息内容
            lblMessage.Values.Text = _duplicateLoginResult.Message ?? "检测到您的账号已在其他设备或浏览器中登录，为了保护账号安全，请选择处理方式：";

            // 设置现有会话信息
            DisplayExistingSessions();

            // 设置按钮
            SetupActionButtons();

            // 设置说明文本
            lblInstruction.Values.Text = "💡 提示：选择处理方式后点击确认按钮继续操作";
        }

        /// <summary>
        /// 显示现有会话信息
        /// </summary>
        private void DisplayExistingSessions()
        {
            if (_duplicateLoginResult.ExistingSessions?.Count > 0)
            {
                var session = _duplicateLoginResult.ExistingSessions[0]; // 获取第一个会话信息
                
                string sessionInfo = $"🕐 {session.LoginTime:yyyy-MM-dd HH:mm:ss} | 🌐 IP: {session.ClientIp} | 💻 {session.DeviceInfo} | 📊 {session.StatusDescription}";
                
                lblSessionInfo.Values.Text = sessionInfo;
            }
            else
            {
                lblSessionInfo.Values.Text = "❌ 未找到其他登录会话信息";
            }
        }

        /// <summary>
        /// 设置操作按钮
        /// </summary>
        private void SetupActionButtons()
        {
            // 踢掉其他设备按钮
            btnForceOffline.Values.Text = "🚫 强制对方下线";
            btnForceOffline.DialogResult = DialogResult.None;
            btnForceOffline.Click += BtnForceOffline_Click;

            // 自己下线按钮
            btnOfflineSelf.Values.Text = "👤 自己下线";
            btnOfflineSelf.DialogResult = DialogResult.None;
            btnOfflineSelf.Click += BtnOfflineSelf_Click;
            btnOfflineSelf.Visible = true; // 显示所有选项

            // 放弃登录按钮
            btnCancelLogin.Values.Text = "❌ 取消";
            btnCancelLogin.DialogResult = DialogResult.None;
            btnCancelLogin.Click += BtnCancelLogin_Click;

            // 确认按钮
            btnConfirm.Values.Text = "✓ 确认";
            btnConfirm.DialogResult = DialogResult.OK;
            btnConfirm.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
            btnConfirm.StateCommon.Back.Color1 = Color.FromArgb(40, 167, 69);
            btnConfirm.StateCommon.Content.ShortText.Color1 = Color.White;
            btnConfirm.StateCommon.Border.Rounding = 6;
            btnConfirm.Click += BtnConfirm_Click;

            // 设置按钮工具提示
            SetupButtonTooltips();
        }

        /// <summary>
        /// 设置按钮工具提示
        /// </summary>
        private void SetupButtonTooltips()
        {
            btnForceOffline.ToolTipValues.Description = "将其他设备上的登录踢下线，保持当前登录状态";
            btnForceOffline.ToolTipValues.EnableToolTips = true;
            btnForceOffline.ToolTipValues.Heading = "踢掉其他设备并继续登录";

            btnOfflineSelf.ToolTipValues.Description = "取消当前登录，保持其他地方的登录状态";
            btnOfflineSelf.ToolTipValues.EnableToolTips = true;
            btnOfflineSelf.ToolTipValues.Heading = "自己下线";

            btnCancelLogin.ToolTipValues.Description = "取消本次登录操作，返回登录界面";
            btnCancelLogin.ToolTipValues.EnableToolTips = true;
            btnCancelLogin.ToolTipValues.Heading = "放弃登录";

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
            // 重置所有按钮到默认状态
            btnForceOffline.StateCommon.Back.Color1 = Color.FromArgb(220, 53, 69);
            btnForceOffline.StateCommon.Content.ShortText.Color1 = Color.White;
            
            btnOfflineSelf.StateCommon.Back.Color1 = Color.FromArgb(255, 193, 7);
            btnOfflineSelf.StateCommon.Content.ShortText.Color1 = Color.White;
            
            btnCancelLogin.StateCommon.Back.Color1 = Color.FromArgb(108, 117, 125);
            btnCancelLogin.StateCommon.Content.ShortText.Color1 = Color.White;

            // 设置选中按钮状态（高亮显示）
            if (selectedButton != null)
            {
                // 临时改变选中按钮的颜色以示区别
                if (selectedButton == btnForceOffline)
                {
                    btnForceOffline.StateCommon.Back.Color1 = Color.FromArgb(200, 35, 51); // 深红色
                }
                else if (selectedButton == btnOfflineSelf)
                {
                    btnOfflineSelf.StateCommon.Back.Color1 = Color.FromArgb(255, 170, 0); // 深黄色
                }
                else if (selectedButton == btnCancelLogin)
                {
                    btnCancelLogin.StateCommon.Back.Color1 = Color.FromArgb(88, 95, 102); // 深灰色
                }
            }
        }

        #endregion

        /// <summary>
        /// 窗体加载时设置默认选择
        /// </summary>
        private void DuplicateLoginDialog_Load(object sender, EventArgs e)
        {
            // 默认选择踢掉其他设备并继续登录
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