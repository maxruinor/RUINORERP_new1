namespace RUINORERP.UI.CommonUI
{
    partial class frmGenericOpinion<T>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpinion));
            this.openFileDialog4Img = new System.Windows.Forms.OpenFileDialog();
            this.toolTipBase = new System.Windows.Forms.ToolTip(this.components);
            this.errorProviderForAllInput = new System.Windows.Forms.ErrorProvider(this.components);
            this.picBoxAttachment = new RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox();
            this.lblAttachment = new Krypton.Toolkit.KryptonLabel();
            this.txtBillNO = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.txtBillType = new Krypton.Toolkit.KryptonTextBox();
            this.timerForToolTip = new System.Windows.Forms.Timer(this.components);
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.lblBillType = new Krypton.Toolkit.KryptonLabel();
            this.lblOpinion = new Krypton.Toolkit.KryptonLabel();
            this.bindingSourceEdit = new System.Windows.Forms.BindingSource(this.components);
            this.kryptonGroupBoxCurrentNode = new Krypton.Toolkit.KryptonGroupBox();
            this.panelAttachment = new Krypton.Toolkit.KryptonPanel();
            this.txtOpinion = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAttachment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode.Panel)).BeginInit();
            this.kryptonGroupBoxCurrentNode.Panel.SuspendLayout();
            this.kryptonGroupBoxCurrentNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAttachment)).BeginInit();
            this.panelAttachment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog4Img
            // 
            this.openFileDialog4Img.FileName = "*.jpg";
            // 
            // errorProviderForAllInput
            // 
            this.errorProviderForAllInput.ContainerControl = this;
            // 
            // picBoxAttachment
            // 
            this.picBoxAttachment.AllowDrop = true;
            this.picBoxAttachment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBoxAttachment.Image = global::RUINORERP.UI.Properties.Resources.nopic;
            this.picBoxAttachment.Location = new System.Drawing.Point(64, 3);
            this.picBoxAttachment.Name = "picBoxAttachment";
            this.picBoxAttachment.RowImage = ((RUINORERP.Global.Model.DataRowImage)(resources.GetObject("picBoxAttachment.RowImage")));
            this.picBoxAttachment.Size = new System.Drawing.Size(235, 190);
            this.picBoxAttachment.TabIndex = 15;
            this.picBoxAttachment.TabStop = false;
            // 
            // lblAttachment
            // 
            this.lblAttachment.Location = new System.Drawing.Point(3, 3);
            this.lblAttachment.Name = "lblAttachment";
            this.lblAttachment.Size = new System.Drawing.Size(62, 20);
            this.lblAttachment.TabIndex = 70;
            this.lblAttachment.Values.Text = "结案凭证";
            // 
            // txtBillNO
            // 
            this.txtBillNO.Location = new System.Drawing.Point(74, 17);
            this.txtBillNO.Name = "txtBillNO";
            this.txtBillNO.Size = new System.Drawing.Size(208, 23);
            this.txtBillNO.TabIndex = 68;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(10, 17);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel1.TabIndex = 67;
            this.kryptonLabel1.Values.Text = "单据号码:";
            // 
            // txtBillType
            // 
            this.txtBillType.Location = new System.Drawing.Point(74, 44);
            this.txtBillType.Name = "txtBillType";
            this.txtBillType.Size = new System.Drawing.Size(208, 23);
            this.txtBillType.TabIndex = 9;
            // 
            // timerForToolTip
            // 
            this.timerForToolTip.Interval = 1000;
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
            // lblBillType
            // 
            this.lblBillType.Location = new System.Drawing.Point(10, 44);
            this.lblBillType.Name = "lblBillType";
            this.lblBillType.Size = new System.Drawing.Size(62, 20);
            this.lblBillType.TabIndex = 8;
            this.lblBillType.Values.Text = "单据信息";
            // 
            // lblOpinion
            // 
            this.lblOpinion.Location = new System.Drawing.Point(3, 69);
            this.lblOpinion.Name = "lblOpinion";
            this.lblOpinion.Size = new System.Drawing.Size(62, 20);
            this.lblOpinion.TabIndex = 10;
            this.lblOpinion.Values.Text = "结案意见";
            // 
            // kryptonGroupBoxCurrentNode
            // 
            this.kryptonGroupBoxCurrentNode.Location = new System.Drawing.Point(12, 12);
            this.kryptonGroupBoxCurrentNode.Name = "kryptonGroupBoxCurrentNode";
            // 
            // kryptonGroupBoxCurrentNode.Panel
            // 
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.panelAttachment);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.txtBillNO);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.txtBillType);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.lblBillType);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.lblOpinion);
            this.kryptonGroupBoxCurrentNode.Panel.Controls.Add(this.txtOpinion);
            this.kryptonGroupBoxCurrentNode.Size = new System.Drawing.Size(541, 459);
            this.kryptonGroupBoxCurrentNode.TabIndex = 14;
            this.kryptonGroupBoxCurrentNode.Values.Heading = "请输入结案的情况";
            // 
            // panelAttachment
            // 
            this.panelAttachment.Controls.Add(this.picBoxAttachment);
            this.panelAttachment.Controls.Add(this.lblAttachment);
            this.panelAttachment.Location = new System.Drawing.Point(10, 221);
            this.panelAttachment.Name = "panelAttachment";
            this.panelAttachment.Size = new System.Drawing.Size(496, 193);
            this.panelAttachment.TabIndex = 71;
            // 
            // txtOpinion
            // 
            this.txtOpinion.Location = new System.Drawing.Point(74, 69);
            this.txtOpinion.Multiline = true;
            this.txtOpinion.Name = "txtOpinion";
            this.txtOpinion.Size = new System.Drawing.Size(432, 146);
            this.txtOpinion.TabIndex = 11;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBoxCurrentNode);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(583, 515);
            this.kryptonPanel1.TabIndex = 5;
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
            // frmGenericOpinion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 515);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "frmGenericOpinion";
            this.Text = "frmGenericOpinion";
            this.Click += new System.EventHandler(this.frmGenericOpinion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAttachment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode.Panel)).EndInit();
            this.kryptonGroupBoxCurrentNode.Panel.ResumeLayout(false);
            this.kryptonGroupBoxCurrentNode.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxCurrentNode)).EndInit();
            this.kryptonGroupBoxCurrentNode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelAttachment)).EndInit();
            this.panelAttachment.ResumeLayout(false);
            this.panelAttachment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog4Img;
        internal System.Windows.Forms.ToolTip toolTipBase;
        public System.Windows.Forms.ErrorProvider errorProviderForAllInput;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxCurrentNode;
        private RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox picBoxAttachment;
        private Krypton.Toolkit.KryptonLabel lblAttachment;
        private Krypton.Toolkit.KryptonTextBox txtBillNO;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonTextBox txtBillType;
        private Krypton.Toolkit.KryptonLabel lblBillType;
        private Krypton.Toolkit.KryptonLabel lblOpinion;
        public Krypton.Toolkit.KryptonTextBox txtOpinion;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private System.Windows.Forms.Timer timerForToolTip;
        internal System.Windows.Forms.BindingSource bindingSourceEdit;
        private Krypton.Toolkit.KryptonPanel panelAttachment;
    }
}