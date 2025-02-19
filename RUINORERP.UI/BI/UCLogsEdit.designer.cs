namespace RUINORERP.UI.BI
{
    partial class UCLogsEdit
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.lblDate = new Krypton.Toolkit.KryptonLabel();
            this.lblLevel = new Krypton.Toolkit.KryptonLabel();
            this.txtLevel = new Krypton.Toolkit.KryptonTextBox();
            this.lblLogger = new Krypton.Toolkit.KryptonLabel();
            this.txtLogger = new Krypton.Toolkit.KryptonTextBox();
            this.lblMessage = new Krypton.Toolkit.KryptonLabel();
            this.txtMessage = new Krypton.Toolkit.KryptonTextBox();
            this.lblException = new Krypton.Toolkit.KryptonLabel();
            this.txtException = new Krypton.Toolkit.KryptonTextBox();
            this.lblOperator = new Krypton.Toolkit.KryptonLabel();
            this.txtOperator = new Krypton.Toolkit.KryptonTextBox();
            this.lblModName = new Krypton.Toolkit.KryptonLabel();
            this.txtModName = new Krypton.Toolkit.KryptonTextBox();
            this.lblPath = new Krypton.Toolkit.KryptonLabel();
            this.txtPath = new Krypton.Toolkit.KryptonTextBox();
            this.lblActionName = new Krypton.Toolkit.KryptonLabel();
            this.txtActionName = new Krypton.Toolkit.KryptonTextBox();
            this.lblIP = new Krypton.Toolkit.KryptonLabel();
            this.txtIP = new Krypton.Toolkit.KryptonTextBox();
            this.lblMAC = new Krypton.Toolkit.KryptonLabel();
            this.txtMAC = new Krypton.Toolkit.KryptonTextBox();
            this.lblMachineName = new Krypton.Toolkit.KryptonLabel();
            this.txtMachineName = new Krypton.Toolkit.KryptonTextBox();
            this.txtDate = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(341, 570);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(506, 570);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.txtDate);
            this.kryptonPanel1.Controls.Add(this.lblDate);
            this.kryptonPanel1.Controls.Add(this.lblLevel);
            this.kryptonPanel1.Controls.Add(this.txtLevel);
            this.kryptonPanel1.Controls.Add(this.lblLogger);
            this.kryptonPanel1.Controls.Add(this.txtLogger);
            this.kryptonPanel1.Controls.Add(this.lblMessage);
            this.kryptonPanel1.Controls.Add(this.txtMessage);
            this.kryptonPanel1.Controls.Add(this.lblException);
            this.kryptonPanel1.Controls.Add(this.txtException);
            this.kryptonPanel1.Controls.Add(this.lblOperator);
            this.kryptonPanel1.Controls.Add(this.txtOperator);
            this.kryptonPanel1.Controls.Add(this.lblModName);
            this.kryptonPanel1.Controls.Add(this.txtModName);
            this.kryptonPanel1.Controls.Add(this.lblPath);
            this.kryptonPanel1.Controls.Add(this.txtPath);
            this.kryptonPanel1.Controls.Add(this.lblActionName);
            this.kryptonPanel1.Controls.Add(this.txtActionName);
            this.kryptonPanel1.Controls.Add(this.lblIP);
            this.kryptonPanel1.Controls.Add(this.txtIP);
            this.kryptonPanel1.Controls.Add(this.lblMAC);
            this.kryptonPanel1.Controls.Add(this.txtMAC);
            this.kryptonPanel1.Controls.Add(this.lblMachineName);
            this.kryptonPanel1.Controls.Add(this.txtMachineName);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(936, 607);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(16, 12);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(36, 20);
            this.lblDate.TabIndex = 13;
            this.lblDate.Values.Text = "时间";
            // 
            // lblLevel
            // 
            this.lblLevel.Location = new System.Drawing.Point(16, 39);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(36, 20);
            this.lblLevel.TabIndex = 15;
            this.lblLevel.Values.Text = "级别";
            // 
            // txtLevel
            // 
            this.txtLevel.Location = new System.Drawing.Point(52, 33);
            this.txtLevel.Name = "txtLevel";
            this.txtLevel.Size = new System.Drawing.Size(235, 23);
            this.txtLevel.TabIndex = 16;
            // 
            // lblLogger
            // 
            this.lblLogger.Location = new System.Drawing.Point(3, 116);
            this.lblLogger.Name = "lblLogger";
            this.lblLogger.Size = new System.Drawing.Size(49, 20);
            this.lblLogger.TabIndex = 17;
            this.lblLogger.Values.Text = "记录器";
            // 
            // txtLogger
            // 
            this.txtLogger.Location = new System.Drawing.Point(52, 113);
            this.txtLogger.Name = "txtLogger";
            this.txtLogger.Size = new System.Drawing.Size(235, 23);
            this.txtLogger.TabIndex = 18;
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(16, 147);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(36, 20);
            this.lblMessage.TabIndex = 19;
            this.lblMessage.Values.Text = "消息";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(52, 145);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtMessage.Size = new System.Drawing.Size(836, 172);
            this.txtMessage.TabIndex = 20;
            // 
            // lblException
            // 
            this.lblException.Location = new System.Drawing.Point(16, 229);
            this.lblException.Name = "lblException";
            this.lblException.Size = new System.Drawing.Size(36, 20);
            this.lblException.TabIndex = 21;
            this.lblException.Values.Text = "异常";
            // 
            // txtException
            // 
            this.txtException.Location = new System.Drawing.Point(52, 324);
            this.txtException.Multiline = true;
            this.txtException.Name = "txtException";
            this.txtException.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtException.Size = new System.Drawing.Size(836, 240);
            this.txtException.TabIndex = 22;
            // 
            // lblOperator
            // 
            this.lblOperator.Location = new System.Drawing.Point(3, 65);
            this.lblOperator.Name = "lblOperator";
            this.lblOperator.Size = new System.Drawing.Size(49, 20);
            this.lblOperator.TabIndex = 24;
            this.lblOperator.Values.Text = "操作者";
            // 
            // txtOperator
            // 
            this.txtOperator.Location = new System.Drawing.Point(52, 59);
            this.txtOperator.Name = "txtOperator";
            this.txtOperator.Size = new System.Drawing.Size(235, 23);
            this.txtOperator.TabIndex = 23;
            // 
            // lblModName
            // 
            this.lblModName.Location = new System.Drawing.Point(3, 89);
            this.lblModName.Name = "lblModName";
            this.lblModName.Size = new System.Drawing.Size(49, 20);
            this.lblModName.TabIndex = 25;
            this.lblModName.Values.Text = "模块名";
            // 
            // txtModName
            // 
            this.txtModName.Location = new System.Drawing.Point(52, 84);
            this.txtModName.Name = "txtModName";
            this.txtModName.Size = new System.Drawing.Size(235, 23);
            this.txtModName.TabIndex = 26;
            // 
            // lblPath
            // 
            this.lblPath.Location = new System.Drawing.Point(571, 120);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(36, 20);
            this.lblPath.TabIndex = 27;
            this.lblPath.Values.Text = "路径";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(616, 115);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(272, 23);
            this.txtPath.TabIndex = 28;
            // 
            // lblActionName
            // 
            this.lblActionName.Location = new System.Drawing.Point(571, 18);
            this.lblActionName.Name = "lblActionName";
            this.lblActionName.Size = new System.Drawing.Size(36, 20);
            this.lblActionName.TabIndex = 29;
            this.lblActionName.Values.Text = "动作";
            // 
            // txtActionName
            // 
            this.txtActionName.Location = new System.Drawing.Point(616, 14);
            this.txtActionName.Name = "txtActionName";
            this.txtActionName.Size = new System.Drawing.Size(272, 23);
            this.txtActionName.TabIndex = 30;
            // 
            // lblIP
            // 
            this.lblIP.Location = new System.Drawing.Point(546, 43);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(62, 20);
            this.lblIP.TabIndex = 31;
            this.lblIP.Values.Text = "网络地址";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(616, 39);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(272, 23);
            this.txtIP.TabIndex = 32;
            // 
            // lblMAC
            // 
            this.lblMAC.Location = new System.Drawing.Point(546, 68);
            this.lblMAC.Name = "lblMAC";
            this.lblMAC.Size = new System.Drawing.Size(62, 20);
            this.lblMAC.TabIndex = 33;
            this.lblMAC.Values.Text = "物理地址";
            // 
            // txtMAC
            // 
            this.txtMAC.Location = new System.Drawing.Point(616, 64);
            this.txtMAC.Name = "txtMAC";
            this.txtMAC.Size = new System.Drawing.Size(272, 23);
            this.txtMAC.TabIndex = 34;
            // 
            // lblMachineName
            // 
            this.lblMachineName.Location = new System.Drawing.Point(558, 93);
            this.lblMachineName.Name = "lblMachineName";
            this.lblMachineName.Size = new System.Drawing.Size(49, 20);
            this.lblMachineName.TabIndex = 35;
            this.lblMachineName.Values.Text = "电脑名";
            // 
            // txtMachineName
            // 
            this.txtMachineName.Location = new System.Drawing.Point(616, 89);
            this.txtMachineName.Name = "txtMachineName";
            this.txtMachineName.Size = new System.Drawing.Size(272, 23);
            this.txtMachineName.TabIndex = 36;
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(52, 8);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(235, 23);
            this.txtDate.TabIndex = 37;
            // 
            // UCLogsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 607);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCLogsEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblDate;
        private Krypton.Toolkit.KryptonLabel lblLevel;
        private Krypton.Toolkit.KryptonTextBox txtLevel;
        private Krypton.Toolkit.KryptonLabel lblLogger;
        private Krypton.Toolkit.KryptonTextBox txtLogger;
        private Krypton.Toolkit.KryptonLabel lblMessage;
        private Krypton.Toolkit.KryptonTextBox txtMessage;
        private Krypton.Toolkit.KryptonLabel lblException;
        private Krypton.Toolkit.KryptonTextBox txtException;
        private Krypton.Toolkit.KryptonLabel lblOperator;
        private Krypton.Toolkit.KryptonTextBox txtOperator;
        private Krypton.Toolkit.KryptonLabel lblModName;
        private Krypton.Toolkit.KryptonTextBox txtModName;
        private Krypton.Toolkit.KryptonLabel lblPath;
        private Krypton.Toolkit.KryptonTextBox txtPath;
        private Krypton.Toolkit.KryptonLabel lblActionName;
        private Krypton.Toolkit.KryptonTextBox txtActionName;
        private Krypton.Toolkit.KryptonLabel lblIP;
        private Krypton.Toolkit.KryptonTextBox txtIP;
        private Krypton.Toolkit.KryptonLabel lblMAC;
        private Krypton.Toolkit.KryptonTextBox txtMAC;
        private Krypton.Toolkit.KryptonLabel lblMachineName;
        private Krypton.Toolkit.KryptonTextBox txtMachineName;
        private Krypton.Toolkit.KryptonTextBox txtDate;
    }
}
