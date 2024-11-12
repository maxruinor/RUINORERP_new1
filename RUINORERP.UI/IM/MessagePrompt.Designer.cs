namespace RUINORERP.UI.IM
{
    partial class MessagePrompt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bindingSourceEdit = new System.Windows.Forms.BindingSource(this.components);
            this.errorProviderForAllInput = new System.Windows.Forms.ErrorProvider(this.components);
            this.timerForToolTip = new System.Windows.Forms.Timer(this.components);
            this.txtSender = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.txtSubject = new Krypton.Toolkit.KryptonTextBox();
            this.lblUnitName = new Krypton.Toolkit.KryptonLabel();
            this.lblDesc = new Krypton.Toolkit.KryptonLabel();
            this.txtContent = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonGroupBoxCurrentNode = new Krypton.Toolkit.KryptonGroupBox();
            this.toolTipBase = new System.Windows.Forms.ToolTip(this.components);
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode.Panel)).BeginInit();
            this.kryptonGroupBoxCurrentNode.Panel.SuspendLayout();
            this.kryptonGroupBoxCurrentNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProviderForAllInput
            // 
            this.errorProviderForAllInput.ContainerControl = this;
            // 
            // timerForToolTip
            // 
            this.timerForToolTip.Interval = 1000;
            // 
            // txtSender
            // 
            this.txtSender.Location = new System.Drawing.Point(74, 7);
            this.txtSender.Name = "txtSender";
            this.txtSender.Size = new System.Drawing.Size(208, 23);
            this.txtSender.TabIndex = 68;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(10, 7);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(49, 20);
            this.kryptonLabel1.TabIndex = 67;
            this.kryptonLabel1.Values.Text = "发送人";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(243, 351);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(125, 351);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 12;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(74, 34);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(208, 23);
            this.txtSubject.TabIndex = 9;
            // 
            // lblUnitName
            // 
            this.lblUnitName.Location = new System.Drawing.Point(10, 34);
            this.lblUnitName.Name = "lblUnitName";
            this.lblUnitName.Size = new System.Drawing.Size(36, 20);
            this.lblUnitName.TabIndex = 8;
            this.lblUnitName.Values.Text = "主题";
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(10, 82);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(36, 20);
            this.lblDesc.TabIndex = 10;
            this.lblDesc.Values.Text = "内容";
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(74, 82);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(210, 128);
            this.txtContent.TabIndex = 11;
            // 
            // kryptonGroupBoxCurrentNode
            // 
            this.kryptonGroupBoxCurrentNode.Location = new System.Drawing.Point(66, 12);
            this.kryptonGroupBoxCurrentNode.Name = "kryptonGroupBoxCurrentNode";
            // 
            // kryptonGroupBoxCurrentNode.Panel
            // 
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.txtSender);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.txtSubject);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.lblUnitName);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.lblDesc);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.txtContent);
            this.kryptonGroupBoxCurrentNode.Size = new System.Drawing.Size(329, 311);
            this.kryptonGroupBoxCurrentNode.TabIndex = 14;
            this.kryptonGroupBoxCurrentNode.Values.Heading = "消息概览";
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBoxCurrentNode);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(438, 388);
            this.kryptonPanel1.TabIndex = 5;
            // 
            // MessagePrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 388);
            this.Controls.Add(this.kryptonPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessagePrompt";
            this.Text = "消息提示";
            this.Load += new System.EventHandler(this.MessagePrompt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode.Panel)).EndInit();
            this.kryptonGroupBoxCurrentNode.Panel.ResumeLayout(false);
            this.kryptonGroupBoxCurrentNode.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode)).EndInit();
            this.kryptonGroupBoxCurrentNode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.BindingSource bindingSourceEdit;
        public System.Windows.Forms.ErrorProvider errorProviderForAllInput;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxCurrentNode;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel lblUnitName;
        private Krypton.Toolkit.KryptonLabel lblDesc;
        private Krypton.Toolkit.KryptonTextBox txtContent;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private System.Windows.Forms.Timer timerForToolTip;
        internal System.Windows.Forms.ToolTip toolTipBase;
        public Krypton.Toolkit.KryptonTextBox txtSender;
        public Krypton.Toolkit.KryptonTextBox txtSubject;
    }
}