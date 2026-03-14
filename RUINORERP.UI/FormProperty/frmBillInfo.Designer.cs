namespace RUINORERP.UI.FormProperty
{
    partial class frmBillInfo
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
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.btnClose = new Krypton.Toolkit.KryptonButton();
            this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
            this.txtCreated_at = new Krypton.Toolkit.KryptonTextBox();
            this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
            this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblUpdated_at = new Krypton.Toolkit.KryptonLabel();
            this.txtUpdated_at = new Krypton.Toolkit.KryptonTextBox();
            this.lblUpdated_by = new Krypton.Toolkit.KryptonLabel();
            this.txtUpdated_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblApproved_at = new Krypton.Toolkit.KryptonLabel();
            this.txtApproved_at = new Krypton.Toolkit.KryptonTextBox();
            this.lblApproved_by = new Krypton.Toolkit.KryptonLabel();
            this.txtApproved_by = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.txtApproved_by);
            this.kryptonPanel1.Controls.Add(this.lblApproved_by);
            this.kryptonPanel1.Controls.Add(this.txtApproved_at);
            this.kryptonPanel1.Controls.Add(this.lblApproved_at);
            this.kryptonPanel1.Controls.Add(this.txtUpdated_by);
            this.kryptonPanel1.Controls.Add(this.lblUpdated_by);
            this.kryptonPanel1.Controls.Add(this.txtUpdated_at);
            this.kryptonPanel1.Controls.Add(this.lblUpdated_at);
            this.kryptonPanel1.Controls.Add(this.txtCreated_by);
            this.kryptonPanel1.Controls.Add(this.lblCreated_by);
            this.kryptonPanel1.Controls.Add(this.txtCreated_at);
            this.kryptonPanel1.Controls.Add(this.lblCreated_at);
            this.kryptonPanel1.Controls.Add(this.btnClose);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(420, 280);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(165, 235);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 25);
            this.btnClose.TabIndex = 0;
            this.btnClose.Values.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCreated_at
            // 
            this.lblCreated_at.Location = new System.Drawing.Point(30, 25);
            this.lblCreated_at.Name = "lblCreated_at";
            this.lblCreated_at.Size = new System.Drawing.Size(60, 20);
            this.lblCreated_at.TabIndex = 1;
            this.lblCreated_at.Values.Text = "创建时间：";
            // 
            // txtCreated_at
            // 
            this.txtCreated_at.Location = new System.Drawing.Point(110, 22);
            this.txtCreated_at.Name = "txtCreated_at";
            this.txtCreated_at.ReadOnly = true;
            this.txtCreated_at.Size = new System.Drawing.Size(280, 23);
            this.txtCreated_at.TabIndex = 2;
            // 
            // lblCreated_by
            // 
            this.lblCreated_by.Location = new System.Drawing.Point(30, 55);
            this.lblCreated_by.Name = "lblCreated_by";
            this.lblCreated_by.Size = new System.Drawing.Size(60, 20);
            this.lblCreated_by.TabIndex = 3;
            this.lblCreated_by.Values.Text = "创建人：";
            // 
            // txtCreated_by
            // 
            this.txtCreated_by.Location = new System.Drawing.Point(110, 52);
            this.txtCreated_by.Name = "txtCreated_by";
            this.txtCreated_by.ReadOnly = true;
            this.txtCreated_by.Size = new System.Drawing.Size(280, 23);
            this.txtCreated_by.TabIndex = 4;
            // 
            // lblUpdated_at
            // 
            this.lblUpdated_at.Location = new System.Drawing.Point(30, 85);
            this.lblUpdated_at.Name = "lblUpdated_at";
            this.lblUpdated_at.Size = new System.Drawing.Size(60, 20);
            this.lblUpdated_at.TabIndex = 5;
            this.lblUpdated_at.Values.Text = "修改时间：";
            // 
            // txtUpdated_at
            // 
            this.txtUpdated_at.Location = new System.Drawing.Point(110, 82);
            this.txtUpdated_at.Name = "txtUpdated_at";
            this.txtUpdated_at.ReadOnly = true;
            this.txtUpdated_at.Size = new System.Drawing.Size(280, 23);
            this.txtUpdated_at.TabIndex = 6;
            // 
            // lblUpdated_by
            // 
            this.lblUpdated_by.Location = new System.Drawing.Point(30, 115);
            this.lblUpdated_by.Name = "lblUpdated_by";
            this.lblUpdated_by.Size = new System.Drawing.Size(60, 20);
            this.lblUpdated_by.TabIndex = 7;
            this.lblUpdated_by.Values.Text = "修改人：";
            // 
            // txtUpdated_by
            // 
            this.txtUpdated_by.Location = new System.Drawing.Point(110, 112);
            this.txtUpdated_by.Name = "txtUpdated_by";
            this.txtUpdated_by.ReadOnly = true;
            this.txtUpdated_by.Size = new System.Drawing.Size(280, 23);
            this.txtUpdated_by.TabIndex = 8;
            // 
            // lblApproved_at
            // 
            this.lblApproved_at.Location = new System.Drawing.Point(30, 145);
            this.lblApproved_at.Name = "lblApproved_at";
            this.lblApproved_at.Size = new System.Drawing.Size(60, 20);
            this.lblApproved_at.TabIndex = 9;
            this.lblApproved_at.Values.Text = "审核时间：";
            // 
            // txtApproved_at
            // 
            this.txtApproved_at.Location = new System.Drawing.Point(110, 142);
            this.txtApproved_at.Name = "txtApproved_at";
            this.txtApproved_at.ReadOnly = true;
            this.txtApproved_at.Size = new System.Drawing.Size(280, 23);
            this.txtApproved_at.TabIndex = 10;
            // 
            // lblApproved_by
            // 
            this.lblApproved_by.Location = new System.Drawing.Point(30, 175);
            this.lblApproved_by.Name = "lblApproved_by";
            this.lblApproved_by.Size = new System.Drawing.Size(60, 20);
            this.lblApproved_by.TabIndex = 11;
            this.lblApproved_by.Values.Text = "审核人：";
            // 
            // txtApproved_by
            // 
            this.txtApproved_by.Location = new System.Drawing.Point(110, 172);
            this.txtApproved_by.Name = "txtApproved_by";
            this.txtApproved_by.ReadOnly = true;
            this.txtApproved_by.Size = new System.Drawing.Size(280, 23);
            this.txtApproved_by.TabIndex = 12;
            // 
            // frmBillInfo
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(420, 280);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmBillInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "操作记录";
            this.Load += new System.EventHandler(this.frmBillInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton btnClose;
        private Krypton.Toolkit.KryptonLabel lblCreated_at;
        private Krypton.Toolkit.KryptonTextBox txtCreated_at;
        private Krypton.Toolkit.KryptonLabel lblCreated_by;
        private Krypton.Toolkit.KryptonTextBox txtCreated_by;
        private Krypton.Toolkit.KryptonLabel lblUpdated_at;
        private Krypton.Toolkit.KryptonTextBox txtUpdated_at;
        private Krypton.Toolkit.KryptonLabel lblUpdated_by;
        private Krypton.Toolkit.KryptonTextBox txtUpdated_by;
        private Krypton.Toolkit.KryptonLabel lblApproved_at;
        private Krypton.Toolkit.KryptonTextBox txtApproved_at;
        private Krypton.Toolkit.KryptonLabel lblApproved_by;
        private Krypton.Toolkit.KryptonTextBox txtApproved_by;
    }
}
