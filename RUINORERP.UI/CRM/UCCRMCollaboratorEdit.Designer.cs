namespace RUINORERP.UI.CRM
{
    partial class UCCRMCollaboratorEdit
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
            this.lblCustomer_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbCustomer_id = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(172, 185);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定(&S)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(290, 185);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.cmbEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.lblCustomer_id);
            this.kryptonPanel1.Controls.Add(this.cmbCustomer_id);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(530, 269);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblCustomer_id
            // 
            this.lblCustomer_id.Location = new System.Drawing.Point(79, 38);
            this.lblCustomer_id.Name = "lblCustomer_id";
            this.lblCustomer_id.Size = new System.Drawing.Size(62, 20);
            this.lblCustomer_id.TabIndex = 53;
            this.lblCustomer_id.Values.Text = "目标客户";
            // 
            // cmbCustomer_id
            // 
            this.cmbCustomer_id.DropDownWidth = 100;
            this.cmbCustomer_id.IntegralHeight = false;
            this.cmbCustomer_id.Location = new System.Drawing.Point(152, 34);
            this.cmbCustomer_id.Name = "cmbCustomer_id";
            this.cmbCustomer_id.Size = new System.Drawing.Size(272, 21);
            this.cmbCustomer_id.TabIndex = 54;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(84, 85);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel1.TabIndex = 111;
            this.kryptonLabel1.Values.Text = "协作员工";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(152, 84);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(272, 21);
            this.cmbEmployee_ID.TabIndex = 110;
            // 
            // UCCRMCollaboratorEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 269);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCRMCollaboratorEdit";
            this.Load += new System.EventHandler(this.UCLeadsEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblCustomer_id;
        private Krypton.Toolkit.KryptonComboBox cmbCustomer_id;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
    }
}
