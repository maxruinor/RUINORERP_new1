namespace RUINORERP.UI
{
    partial class FrmLogin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.lblpwd = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.chksaveIDpwd = new System.Windows.Forms.CheckBox();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnok = new System.Windows.Forms.Button();
            this.txtPassWord = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.fadeTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.chkSelectServer = new System.Windows.Forms.CheckBox();
            this.chkAutoReminderUpdate = new System.Windows.Forms.CheckBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.gbIPPort = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.gbIPPort.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblpwd
            // 
            this.lblpwd.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblpwd.Location = new System.Drawing.Point(86, 150);
            this.lblpwd.Name = "lblpwd";
            this.lblpwd.Size = new System.Drawing.Size(65, 16);
            this.lblpwd.TabIndex = 13;
            this.lblpwd.Text = "密    码:";
            // 
            // lblID
            // 
            this.lblID.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblID.Location = new System.Drawing.Point(86, 112);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(65, 16);
            this.lblID.TabIndex = 12;
            this.lblID.Text = "账    号:";
            // 
            // chksaveIDpwd
            // 
            this.chksaveIDpwd.Location = new System.Drawing.Point(159, 200);
            this.chksaveIDpwd.Name = "chksaveIDpwd";
            this.chksaveIDpwd.Size = new System.Drawing.Size(144, 24);
            this.chksaveIDpwd.TabIndex = 9;
            this.chksaveIDpwd.Text = "保存账号密码";
            this.chksaveIDpwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chksaveIDpwd_KeyPress);
            // 
            // btncancel
            // 
            this.btncancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btncancel.Location = new System.Drawing.Point(264, 236);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(73, 35);
            this.btncancel.TabIndex = 11;
            this.btncancel.Text = "取消";
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnok
            // 
            this.btnok.Location = new System.Drawing.Point(159, 236);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(73, 35);
            this.btnok.TabIndex = 10;
            this.btnok.Text = "登陆";
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // txtPassWord
            // 
            this.txtPassWord.AccessibleDescription = "password";
            this.txtPassWord.AccessibleName = "password";
            this.txtPassWord.Location = new System.Drawing.Point(157, 148);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(178, 21);
            this.txtPassWord.TabIndex = 8;
            this.txtPassWord.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassWord_KeyPress);
            // 
            // txtUserName
            // 
            this.txtUserName.AccessibleDescription = "";
            this.txtUserName.AccessibleName = "";
            this.txtUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtUserName.Location = new System.Drawing.Point(157, 110);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(178, 21);
            this.txtUserName.TabIndex = 7;
            this.txtUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserName_KeyPress);
            // 
            // fadeTimer
            // 
            this.fadeTimer.Interval = 50;
            this.fadeTimer.Tick += new System.EventHandler(this.fadeTimer_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(52, -3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 103);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pictureBox2.Image = global::RUINORERP.UI.Properties.Resources.logo11;
            this.pictureBox2.Location = new System.Drawing.Point(0, 63);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(46, 176);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            // 
            // txtServerIP
            // 
            this.txtServerIP.AccessibleDescription = "";
            this.txtServerIP.AccessibleName = "";
            this.txtServerIP.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtServerIP.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtServerIP.Location = new System.Drawing.Point(51, 18);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(96, 21);
            this.txtServerIP.TabIndex = 17;
            this.txtServerIP.Text = "192.168.0.254";
            this.txtServerIP.TextChanged += new System.EventHandler(this.txtServerIP_TextChanged);
            // 
            // chkSelectServer
            // 
            this.chkSelectServer.AutoSize = true;
            this.chkSelectServer.Location = new System.Drawing.Point(347, 308);
            this.chkSelectServer.Name = "chkSelectServer";
            this.chkSelectServer.Size = new System.Drawing.Size(84, 16);
            this.chkSelectServer.TabIndex = 18;
            this.chkSelectServer.Text = "指定服务器";
            this.chkSelectServer.UseVisualStyleBackColor = true;
            this.chkSelectServer.CheckedChanged += new System.EventHandler(this.chkSelectServer_CheckedChanged);
            // 
            // chkAutoReminderUpdate
            // 
            this.chkAutoReminderUpdate.AutoSize = true;
            this.chkAutoReminderUpdate.Checked = true;
            this.chkAutoReminderUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoReminderUpdate.Location = new System.Drawing.Point(280, 204);
            this.chkAutoReminderUpdate.Name = "chkAutoReminderUpdate";
            this.chkAutoReminderUpdate.Size = new System.Drawing.Size(96, 16);
            this.chkAutoReminderUpdate.TabIndex = 19;
            this.chkAutoReminderUpdate.Text = "自动提醒更新";
            this.chkAutoReminderUpdate.UseVisualStyleBackColor = true;
            this.chkAutoReminderUpdate.Visible = false;
            // 
            // txtPort
            // 
            this.txtPort.AccessibleDescription = "";
            this.txtPort.AccessibleName = "";
            this.txtPort.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtPort.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtPort.Location = new System.Drawing.Point(200, 18);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(46, 21);
            this.txtPort.TabIndex = 20;
            this.txtPort.Text = "3001";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(22, 22);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(23, 12);
            this.lblIP.TabIndex = 23;
            this.lblIP.Text = "IP:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(159, 22);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(35, 12);
            this.lblPort.TabIndex = 24;
            this.lblPort.Text = "Port:";
            // 
            // gbIPPort
            // 
            this.gbIPPort.Controls.Add(this.txtPort);
            this.gbIPPort.Controls.Add(this.lblPort);
            this.gbIPPort.Controls.Add(this.txtServerIP);
            this.gbIPPort.Controls.Add(this.lblIP);
            this.gbIPPort.Location = new System.Drawing.Point(71, 285);
            this.gbIPPort.Name = "gbIPPort";
            this.gbIPPort.Size = new System.Drawing.Size(262, 49);
            this.gbIPPort.TabIndex = 25;
            this.gbIPPort.TabStop = false;
            this.gbIPPort.Text = "服务器信息";
            this.gbIPPort.Visible = false;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CancelButton = this.btncancel;
            this.ClientSize = new System.Drawing.Size(461, 348);
            this.ControlBox = false;
            this.Controls.Add(this.gbIPPort);
            this.Controls.Add(this.chkAutoReminderUpdate);
            this.Controls.Add(this.chkSelectServer);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblpwd);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.chksaveIDpwd);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.txtPassWord);
            this.Controls.Add(this.txtUserName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登陆窗口";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.gbIPPort.ResumeLayout(false);
            this.gbIPPort.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblpwd;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.CheckBox chksaveIDpwd;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnok;
        private System.Windows.Forms.TextBox txtPassWord;
        internal System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Timer fadeTimer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.PictureBox pictureBox2;
        internal System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.CheckBox chkSelectServer;
        private System.Windows.Forms.CheckBox chkAutoReminderUpdate;
        private System.Windows.Forms.Label lblIP;
        internal System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.GroupBox gbIPPort;
    }
}