namespace RUINORERP.UI.CommonUI
{
    partial class frmFormProperty
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
            this.btnLoadFormData = new Krypton.Toolkit.KryptonButton();
            this.btnSaveFormData = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.timerForToolTip = new System.Windows.Forms.Timer(this.components);
            this.errorProviderForAllInput = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTipBase = new System.Windows.Forms.ToolTip(this.components);
            this.bindingSourceEdit = new System.Windows.Forms.BindingSource(this.components);
            this.flowLayoutPanelButtonsArea = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.flowLayoutPanelButtonsArea);
            this.kryptonPanel1.Controls.Add(this.btnLoadFormData);
            this.kryptonPanel1.Controls.Add(this.btnSaveFormData);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(604, 450);
            this.kryptonPanel1.TabIndex = 4;
            // 
            // btnLoadFormData
            // 
            this.btnLoadFormData.Location = new System.Drawing.Point(230, 58);
            this.btnLoadFormData.Name = "btnLoadFormData";
            this.btnLoadFormData.Size = new System.Drawing.Size(90, 25);
            this.btnLoadFormData.TabIndex = 15;
            this.btnLoadFormData.Values.Text = "加载最新数据";
 
            // 
            // btnSaveFormData
            // 
            this.btnSaveFormData.Location = new System.Drawing.Point(230, 12);
            this.btnSaveFormData.Name = "btnSaveFormData";
            this.btnSaveFormData.Size = new System.Drawing.Size(90, 25);
            this.btnSaveFormData.TabIndex = 14;
            this.btnSaveFormData.Values.Text = "保存当前单据";
           
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(307, 404);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(183, 404);
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
            // flowLayoutPanelButtonsArea
            // 
            this.flowLayoutPanelButtonsArea.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanelButtonsArea.Name = "flowLayoutPanelButtonsArea";
            this.flowLayoutPanelButtonsArea.Size = new System.Drawing.Size(151, 168);
            this.flowLayoutPanelButtonsArea.TabIndex = 16;
            // 
            // frmFormProperty
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(604, 450);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmFormProperty";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "菜单属性";
            this.Load += new System.EventHandler(this.frmApproval_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private System.Windows.Forms.Timer timerForToolTip;
        public System.Windows.Forms.ErrorProvider errorProviderForAllInput;
        internal System.Windows.Forms.ToolTip toolTipBase;
        internal System.Windows.Forms.BindingSource bindingSourceEdit;
        private Krypton.Toolkit.KryptonButton btnLoadFormData;
        private Krypton.Toolkit.KryptonButton btnSaveFormData;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtonsArea;
    }
}