namespace RUINORERP.UI.CommonUI
{
    partial class frmOpinion
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
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonGroupBoxCurrentNode = new Krypton.Toolkit.KryptonGroupBox();
            this.picBoxCloseImage = new RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox();
            this.lblCloseImage = new Krypton.Toolkit.KryptonLabel();
            this.txtBillNO = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.txtBillType = new Krypton.Toolkit.KryptonTextBox();
            this.lblUnitName = new Krypton.Toolkit.KryptonLabel();
            this.lblDesc = new Krypton.Toolkit.KryptonLabel();
            this.txtOpinion = new Krypton.Toolkit.KryptonTextBox();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.timerForToolTip = new System.Windows.Forms.Timer(this.components);
            this.errorProviderForAllInput = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTipBase = new System.Windows.Forms.ToolTip(this.components);
            this.bindingSourceEdit = new System.Windows.Forms.BindingSource(this.components);
            this.openFileDialog4Img = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode.Panel)).BeginInit();
            this.kryptonGroupBoxCurrentNode.Panel.SuspendLayout();
            this.kryptonGroupBoxCurrentNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCloseImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBoxCurrentNode);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(553, 523);
            this.kryptonPanel1.TabIndex = 4;
            // 
            // kryptonGroupBoxCurrentNode
            // 
            this.kryptonGroupBoxCurrentNode.Location = new System.Drawing.Point(12, 12);
            this.kryptonGroupBoxCurrentNode.Name = "kryptonGroupBoxCurrentNode";
            // 
            // kryptonGroupBoxCurrentNode.Panel
            // 
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.picBoxCloseImage);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.lblCloseImage);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.txtBillNO);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.txtBillType);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.lblUnitName);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.lblDesc);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.txtOpinion);
            this.kryptonGroupBoxCurrentNode.Size = new System.Drawing.Size(541, 459);
            this.kryptonGroupBoxCurrentNode.TabIndex = 14;
            this.kryptonGroupBoxCurrentNode.Values.Heading = "请输入结案的情况";
            // 
            // picBoxCloseImage
            // 
            this.picBoxCloseImage.AllowDrop = true;
            this.picBoxCloseImage.Image = global::RUINORERP.UI.Properties.Resources.nopic;
            this.picBoxCloseImage.Location = new System.Drawing.Point(74, 221);
            this.picBoxCloseImage.Name = "picBoxCloseImage";
            this.picBoxCloseImage.Size = new System.Drawing.Size(252, 199);
            this.picBoxCloseImage.TabIndex = 15;
            this.picBoxCloseImage.TabStop = false;
            // 
            // lblCloseImage
            // 
            this.lblCloseImage.Location = new System.Drawing.Point(7, 219);
            this.lblCloseImage.Name = "lblCloseImage";
            this.lblCloseImage.Size = new System.Drawing.Size(62, 20);
            this.lblCloseImage.TabIndex = 70;
            this.lblCloseImage.Values.Text = "结案凭证";
            // 
            // txtBillNO
            // 
            this.txtBillNO.Location = new System.Drawing.Point(74, 7);
            this.txtBillNO.Name = "txtBillNO";
            this.txtBillNO.Size = new System.Drawing.Size(208, 23);
            this.txtBillNO.TabIndex = 68;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(10, 7);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel1.TabIndex = 67;
            this.kryptonLabel1.Values.Text = "单据号码:";
            // 
            // txtBillType
            // 
            this.txtBillType.Location = new System.Drawing.Point(74, 34);
            this.txtBillType.Name = "txtBillType";
            this.txtBillType.Size = new System.Drawing.Size(208, 23);
            this.txtBillType.TabIndex = 9;
            // 
            // lblUnitName
            // 
            this.lblUnitName.Location = new System.Drawing.Point(10, 34);
            this.lblUnitName.Name = "lblUnitName";
            this.lblUnitName.Size = new System.Drawing.Size(62, 20);
            this.lblUnitName.TabIndex = 8;
            this.lblUnitName.Values.Text = "单据信息";
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(10, 63);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(62, 20);
            this.lblDesc.TabIndex = 10;
            this.lblDesc.Values.Text = "结案意见";
            // 
            // txtOpinion
            // 
            this.txtOpinion.Location = new System.Drawing.Point(74, 63);
            this.txtOpinion.Multiline = true;
            this.txtOpinion.Name = "txtOpinion";
            this.txtOpinion.Size = new System.Drawing.Size(432, 152);
            this.txtOpinion.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(291, 477);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(179, 477);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 12;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // timerForToolTip
            // 
            this.timerForToolTip.Interval = 1000;
            // 
            // errorProviderForAllInput
            // 
            this.errorProviderForAllInput.ContainerControl = this;
            // 
            // openFileDialog4Img
            // 
            this.openFileDialog4Img.FileName = "*.jpg";
            // 
            // frmOpinion
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(553, 523);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmOpinion";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "结案";
            this.Load += new System.EventHandler(this.frmApproval_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode.Panel)).EndInit();
            this.kryptonGroupBoxCurrentNode.Panel.ResumeLayout(false);
            this.kryptonGroupBoxCurrentNode.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode)).EndInit();
            this.kryptonGroupBoxCurrentNode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCloseImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxCurrentNode;
        private Krypton.Toolkit.KryptonTextBox txtBillType;
        private Krypton.Toolkit.KryptonLabel lblUnitName;
        private Krypton.Toolkit.KryptonLabel lblDesc;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonTextBox txtBillNO;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private System.Windows.Forms.Timer timerForToolTip;
        public System.Windows.Forms.ErrorProvider errorProviderForAllInput;
        internal System.Windows.Forms.ToolTip toolTipBase;
        internal System.Windows.Forms.BindingSource bindingSourceEdit;
        public Krypton.Toolkit.KryptonTextBox txtOpinion;
        private Krypton.Toolkit.KryptonLabel lblCloseImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog4Img;
        private RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox picBoxCloseImage;
    }
}