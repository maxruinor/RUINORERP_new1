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
            this.lvExistingSessions = new Krypton.Toolkit.KryptonListView();
            this.columnHeaderSessionId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLoginTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderClientIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDeviceInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlButtons = new Krypton.Toolkit.KryptonPanel();
            this.btnForceOffline = new Krypton.Toolkit.KryptonButton();
            this.btnOfflineSelf = new Krypton.Toolkit.KryptonButton();
            this.btnCancelLogin = new Krypton.Toolkit.KryptonButton();
            this.btnConfirm = new Krypton.Toolkit.KryptonButton();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(560, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Values.Text = "登录冲突";
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(20, 60);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(560, 40);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Values.Text = "您的账号已在其他地方登录，请选择处理方式：";
            // 
            // lvExistingSessions
            // 
            this.lvExistingSessions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvExistingSessions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSessionId,
            this.columnHeaderLoginTime,
            this.columnHeaderClientIp,
            this.columnHeaderDeviceInfo,
            this.columnHeaderStatus});
            this.lvExistingSessions.FullRowSelect = true;
            this.lvExistingSessions.GridLines = true;
            this.lvExistingSessions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            this.lvExistingSessions.HideSelection = false;
            this.lvExistingSessions.Location = new System.Drawing.Point(20, 110);
            this.lvExistingSessions.MultiSelect = false;
            this.lvExistingSessions.Name = "lvExistingSessions";
            this.lvExistingSessions.Size = new System.Drawing.Size(560, 150);
            this.lvExistingSessions.TabIndex = 2;
            this.lvExistingSessions.UseCompatibleStateImageBehavior = false;
            this.lvExistingSessions.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderSessionId
            // 
            this.columnHeaderSessionId.Text = "会话ID";
            this.columnHeaderSessionId.Width = 120;
            // 
            // columnHeaderLoginTime
            // 
            this.columnHeaderLoginTime.Text = "登录时间";
            this.columnHeaderLoginTime.Width = 140;
            // 
            // columnHeaderClientIp
            // 
            this.columnHeaderClientIp.Text = "客户端IP";
            this.columnHeaderClientIp.Width = 100;
            // 
            // columnHeaderDeviceInfo
            // 
            this.columnHeaderDeviceInfo.Text = "设备信息";
            this.columnHeaderDeviceInfo.Width = 120;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "状态";
            this.columnHeaderStatus.Width = 80;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.Controls.Add(this.btnForceOffline);
            this.pnlButtons.Controls.Add(this.btnOfflineSelf);
            this.pnlButtons.Controls.Add(this.btnCancelLogin);
            this.pnlButtons.Controls.Add(this.btnConfirm);
            this.pnlButtons.Location = new System.Drawing.Point(20, 280);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(560, 80);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnForceOffline
            // 
            this.btnForceOffline.Location = new System.Drawing.Point(10, 10);
            this.btnForceOffline.Name = "btnForceOffline";
            this.btnForceOffline.Size = new System.Drawing.Size(120, 35);
            this.btnForceOffline.TabIndex = 0;
            this.btnForceOffline.Values.Text = "强制对方下线";
            // 
            // btnOfflineSelf
            // 
            this.btnOfflineSelf.Location = new System.Drawing.Point(140, 10);
            this.btnOfflineSelf.Name = "btnOfflineSelf";
            this.btnOfflineSelf.Size = new System.Drawing.Size(100, 35);
            this.btnOfflineSelf.TabIndex = 1;
            this.btnOfflineSelf.Values.Text = "自己下线";
            // 
            // btnCancelLogin
            // 
            this.btnCancelLogin.Location = new System.Drawing.Point(250, 10);
            this.btnCancelLogin.Name = "btnCancelLogin";
            this.btnCancelLogin.Size = new System.Drawing.Size(100, 35);
            this.btnCancelLogin.TabIndex = 2;
            this.btnCancelLogin.Values.Text = "取消登录";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(470, 10);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 35);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Values.Text = "确认";
            // 
            // lblInstruction
            // 
            this.lblInstruction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInstruction.Location = new System.Drawing.Point(20, 370);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(400, 20);
            this.lblInstruction.TabIndex = 4;
            this.lblInstruction.Values.Text = "请选择处理方式后点击确认继续。";
            // 
            // DuplicateLoginDialog
            // 
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lvExistingSessions);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lblInstruction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DuplicateLoginDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "重复登录确认";
            this.Load += new System.EventHandler(this.DuplicateLoginDialog_Load);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonLabel lblTitle;
        private Krypton.Toolkit.KryptonLabel lblMessage;
        private Krypton.Toolkit.KryptonLabel lblInstruction;
        private Krypton.Toolkit.KryptonListView lvExistingSessions;
        private System.Windows.Forms.ColumnHeader columnHeaderSessionId;
        private System.Windows.Forms.ColumnHeader columnHeaderLoginTime;
        private System.Windows.Forms.ColumnHeader columnHeaderClientIp;
        private System.Windows.Forms.ColumnHeader columnHeaderDeviceInfo;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private Krypton.Toolkit.KryptonPanel pnlButtons;
        private Krypton.Toolkit.KryptonButton btnForceOffline;
        private Krypton.Toolkit.KryptonButton btnOfflineSelf;
        private Krypton.Toolkit.KryptonButton btnCancelLogin;
        private Krypton.Toolkit.KryptonButton btnConfirm;
    }
}