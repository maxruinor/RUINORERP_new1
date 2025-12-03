namespace RUINORERP.UI.Forms
{
    partial class DuplicateLoginDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new Krypton.Toolkit.KryptonLabel();
            this.lblMessage = new Krypton.Toolkit.KryptonLabel();
            this.lblInstruction = new Krypton.Toolkit.KryptonLabel();
            this.pnlSessionInfo = new Krypton.Toolkit.KryptonPanel();
            this.lblSessionInfoLabel = new Krypton.Toolkit.KryptonLabel();
            this.lblSessionInfo = new Krypton.Toolkit.KryptonLabel();
            this.pnlButtons = new Krypton.Toolkit.KryptonPanel();
            this.btnForceOffline = new Krypton.Toolkit.KryptonButton();
            this.btnOfflineSelf = new Krypton.Toolkit.KryptonButton();
            this.btnCancelLogin = new Krypton.Toolkit.KryptonButton();
            this.btnConfirm = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSessionInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(30, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(540, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Values.Text = "⚠️ 登录冲突";
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(220, 53, 69);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(30, 70);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(540, 50);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Values.Text = "检测到您的账号已在其他设备或浏览器中登录，为了保护账号安全，请选择处理方式：";
            this.lblMessage.Font = new System.Drawing.Font("微软雅黑", 9.5F);
            this.lblMessage.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(52, 58, 64);
            // 
            // pnlSessionInfo
            // 
            this.pnlSessionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSessionInfo.Controls.Add(this.lblSessionInfoLabel);
            this.pnlSessionInfo.Controls.Add(this.lblSessionInfo);
            this.pnlSessionInfo.Location = new System.Drawing.Point(30, 130);
            this.pnlSessionInfo.Name = "pnlSessionInfo";
            this.pnlSessionInfo.Size = new System.Drawing.Size(540, 70);
            this.pnlSessionInfo.TabIndex = 2;
            // 移除边框设置，使用默认样式
            // 
            // lblSessionInfoLabel
            // 
            this.lblSessionInfoLabel.Location = new System.Drawing.Point(15, 12);
            this.lblSessionInfoLabel.Name = "lblSessionInfoLabel";
            this.lblSessionInfoLabel.Size = new System.Drawing.Size(100, 25);
            this.lblSessionInfoLabel.TabIndex = 0;
            this.lblSessionInfoLabel.Values.Text = "📍 当前登录信息：";
            this.lblSessionInfoLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lblSessionInfoLabel.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(52, 58, 64);
            // 
            // lblSessionInfo
            // 
            this.lblSessionInfo.Location = new System.Drawing.Point(15, 40);
            this.lblSessionInfo.Name = "lblSessionInfo";
            this.lblSessionInfo.Size = new System.Drawing.Size(510, 20);
            this.lblSessionInfo.TabIndex = 1;
            this.lblSessionInfo.Values.Text = "正在加载登录信息...";
            this.lblSessionInfo.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.lblSessionInfo.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(73, 80, 87);

            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.Controls.Add(this.btnForceOffline);
            this.pnlButtons.Controls.Add(this.btnOfflineSelf);
            this.pnlButtons.Controls.Add(this.btnCancelLogin);
            this.pnlButtons.Controls.Add(this.btnConfirm);
            this.pnlButtons.Location = new System.Drawing.Point(30, 215);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(540, 85);
            this.pnlButtons.TabIndex = 3;
            // 移除边框设置，使用默认样式
            // 
            // btnForceOffline
            // 
            this.btnForceOffline.Location = new System.Drawing.Point(15, 20);
            this.btnForceOffline.Name = "btnForceOffline";
            this.btnForceOffline.Size = new System.Drawing.Size(140, 40);
            this.btnForceOffline.TabIndex = 0;
            this.btnForceOffline.Values.Text = "🚫 强制对方下线";
            this.btnForceOffline.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnForceOffline.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnForceOffline.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnForceOffline.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnForceOffline.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.None;
            this.btnForceOffline.StateCommon.Border.Rounding = 6;
            // 
            // btnOfflineSelf
            // 
            this.btnOfflineSelf.Location = new System.Drawing.Point(165, 20);
            this.btnOfflineSelf.Name = "btnOfflineSelf";
            this.btnOfflineSelf.Size = new System.Drawing.Size(120, 40);
            this.btnOfflineSelf.TabIndex = 1;
            this.btnOfflineSelf.Values.Text = "👤 自己下线";
            this.btnOfflineSelf.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnOfflineSelf.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnOfflineSelf.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(255, 193, 7);
            this.btnOfflineSelf.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnOfflineSelf.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.None;
            this.btnOfflineSelf.StateCommon.Border.Rounding = 6;
            // 
            // btnCancelLogin
            // 
            this.btnCancelLogin.Location = new System.Drawing.Point(295, 20);
            this.btnCancelLogin.Name = "btnCancelLogin";
            this.btnCancelLogin.Size = new System.Drawing.Size(100, 40);
            this.btnCancelLogin.TabIndex = 2;
            this.btnCancelLogin.Values.Text = "❌ 取消";
            this.btnCancelLogin.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCancelLogin.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnCancelLogin.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnCancelLogin.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnCancelLogin.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.None;
            this.btnCancelLogin.StateCommon.Border.Rounding = 6;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(445, 20);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 40);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Values.Text = "✓ 确认";
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnConfirm.ButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnConfirm.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnConfirm.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnConfirm.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.None;
            this.btnConfirm.StateCommon.Border.Rounding = 6;
            // 
            // lblInstruction
            // 
            this.lblInstruction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInstruction.Location = new System.Drawing.Point(30, 310);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(480, 25);
            this.lblInstruction.TabIndex = 4;
            this.lblInstruction.Values.Text = "💡 提示：选择处理方式后点击确认按钮继续操作";
            this.lblInstruction.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.lblInstruction.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(108, 117, 125);
            // 
            // DuplicateLoginDialog
            // 
            this.ClientSize = new System.Drawing.Size(600, 350);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.pnlSessionInfo);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lblInstruction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DuplicateLoginDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "重复登录确认";
            this.BackColor = System.Drawing.Color.White;
            this.Load += new System.EventHandler(this.DuplicateLoginDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSessionInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonLabel lblTitle;
        private Krypton.Toolkit.KryptonLabel lblMessage;
        private Krypton.Toolkit.KryptonLabel lblInstruction;
        private Krypton.Toolkit.KryptonPanel pnlSessionInfo;
        private Krypton.Toolkit.KryptonLabel lblSessionInfoLabel;
        private Krypton.Toolkit.KryptonLabel lblSessionInfo;
        private Krypton.Toolkit.KryptonPanel pnlButtons;
        private Krypton.Toolkit.KryptonButton btnForceOffline;
        private Krypton.Toolkit.KryptonButton btnOfflineSelf;
        private Krypton.Toolkit.KryptonButton btnCancelLogin;
        private Krypton.Toolkit.KryptonButton btnConfirm;
    }
}