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
            this.lblTitle = new Krypton.Toolkit.KryptonLabel();
            this.lblMessage = new Krypton.Toolkit.KryptonLabel();
            this.lblInstruction = new Krypton.Toolkit.KryptonLabel();
            this.pnlSessionInfo = new Krypton.Toolkit.KryptonPanel();
            this.lblSessionInfoLabel = new Krypton.Toolkit.KryptonLabel();
            this.lblSessionInfo = new Krypton.Toolkit.KryptonLabel();
            this.pnlButtons = new Krypton.Toolkit.KryptonPanel();
            this.btnForceOffline = new Krypton.Toolkit.KryptonButton();
            this.btnCancelLogin = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSessionInfo)).BeginInit();
            this.pnlSessionInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(86, 20);
            this.lblTitle.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Values.Text = "⚠️ 登录冲突";
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("微软雅黑", 9.5F);
            this.lblMessage.Location = new System.Drawing.Point(30, 70);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(502, 20);
            this.lblMessage.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Values.Text = "检测到您的账号已在其他设备或浏览器中登录，为了保护账号安全，请选择处理方式：";
            // 
            // lblInstruction
            // 
            this.lblInstruction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInstruction.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.lblInstruction.Location = new System.Drawing.Point(30, 315);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(281, 20);
            this.lblInstruction.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblInstruction.TabIndex = 4;
            this.lblInstruction.Values.Text = "💡 提示：选择处理方式后点击确认按钮继续操作";
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
            // 
            // lblSessionInfoLabel
            // 
            this.lblSessionInfoLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lblSessionInfoLabel.Location = new System.Drawing.Point(15, 12);
            this.lblSessionInfoLabel.Name = "lblSessionInfoLabel";
            this.lblSessionInfoLabel.Size = new System.Drawing.Size(112, 20);
            this.lblSessionInfoLabel.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.lblSessionInfoLabel.TabIndex = 0;
            this.lblSessionInfoLabel.Values.Text = "📍 当前登录信息：";
            // 
            // lblSessionInfo
            // 
            this.lblSessionInfo.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.lblSessionInfo.Location = new System.Drawing.Point(15, 40);
            this.lblSessionInfo.Name = "lblSessionInfo";
            this.lblSessionInfo.Size = new System.Drawing.Size(122, 20);
            this.lblSessionInfo.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.lblSessionInfo.TabIndex = 1;
            this.lblSessionInfo.Values.Text = "正在加载登录信息...";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.Controls.Add(this.btnForceOffline);
            this.pnlButtons.Controls.Add(this.btnCancelLogin);
            this.pnlButtons.Location = new System.Drawing.Point(30, 215);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(540, 85);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnForceOffline
            // 
            this.btnForceOffline.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnForceOffline.Location = new System.Drawing.Point(30, 20);
            this.btnForceOffline.Name = "btnForceOffline";
            this.btnForceOffline.Size = new System.Drawing.Size(140, 40);
            this.btnForceOffline.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnForceOffline.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.None;
            this.btnForceOffline.StateCommon.Border.Rounding = 6F;
            this.btnForceOffline.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnForceOffline.TabIndex = 0;
            this.btnForceOffline.Values.Text = "🚫 强制对方下线";
            // 
            // btnCancelLogin
            // 
            this.btnCancelLogin.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCancelLogin.Location = new System.Drawing.Point(332, 20);
            this.btnCancelLogin.Name = "btnCancelLogin";
            this.btnCancelLogin.Size = new System.Drawing.Size(125, 40);
            this.btnCancelLogin.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancelLogin.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.None;
            this.btnCancelLogin.StateCommon.Border.Rounding = 6F;
            this.btnCancelLogin.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnCancelLogin.TabIndex = 2;
            this.btnCancelLogin.Values.Text = "❌ 取消登陆";
            // 
            // DuplicateLoginDialog
            // 
            this.BackColor = System.Drawing.Color.White;
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
            this.Load += new System.EventHandler(this.DuplicateLoginDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSessionInfo)).EndInit();
            this.pnlSessionInfo.ResumeLayout(false);
            this.pnlSessionInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private Krypton.Toolkit.KryptonButton btnCancelLogin;
    }
}