namespace RUINORERP.UI.BI
{
    partial class UCStorageRackEdit
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
            this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblRackNO = new Krypton.Toolkit.KryptonLabel();
            this.txtRackNO = new Krypton.Toolkit.KryptonTextBox();
            this.lblRackName = new Krypton.Toolkit.KryptonLabel();
            this.txtRackName = new Krypton.Toolkit.KryptonTextBox();
            this.lblRackLocation = new Krypton.Toolkit.KryptonLabel();
            this.txtRackLocation = new Krypton.Toolkit.KryptonTextBox();
            this.lblDesc = new Krypton.Toolkit.KryptonLabel();
            this.txtDesc = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocation_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(140, 323);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(258, 323);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblLocation_ID);
            this.kryptonPanel1.Controls.Add(this.cmbLocation_ID);
            this.kryptonPanel1.Controls.Add(this.lblRackNO);
            this.kryptonPanel1.Controls.Add(this.txtRackNO);
            this.kryptonPanel1.Controls.Add(this.lblRackName);
            this.kryptonPanel1.Controls.Add(this.txtRackName);
            this.kryptonPanel1.Controls.Add(this.lblRackLocation);
            this.kryptonPanel1.Controls.Add(this.txtRackLocation);
            this.kryptonPanel1.Controls.Add(this.lblDesc);
            this.kryptonPanel1.Controls.Add(this.txtDesc);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(471, 379);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblLocation_ID
            // 
            this.lblLocation_ID.Location = new System.Drawing.Point(57, 26);
            this.lblLocation_ID.Name = "lblLocation_ID";
            this.lblLocation_ID.Size = new System.Drawing.Size(60, 20);
            this.lblLocation_ID.TabIndex = 6;
            this.lblLocation_ID.Values.Text = "所属仓库";
            // 
            // cmbLocation_ID
            // 
            this.cmbLocation_ID.DropDownWidth = 100;
            this.cmbLocation_ID.Location = new System.Drawing.Point(130, 22);
            this.cmbLocation_ID.Name = "cmbLocation_ID";
            this.cmbLocation_ID.Size = new System.Drawing.Size(235, 21);
            this.cmbLocation_ID.TabIndex = 7;
            // 
            // lblRackNO
            // 
            this.lblRackNO.Location = new System.Drawing.Point(57, 63);
            this.lblRackNO.Name = "lblRackNO";
            this.lblRackNO.Size = new System.Drawing.Size(60, 20);
            this.lblRackNO.TabIndex = 8;
            this.lblRackNO.Values.Text = "货架编号";
            // 
            // txtRackNO
            // 
            this.txtRackNO.Location = new System.Drawing.Point(130, 59);
            this.txtRackNO.Name = "txtRackNO";
            this.txtRackNO.Size = new System.Drawing.Size(235, 20);
            this.txtRackNO.TabIndex = 9;
            // 
            // lblRackName
            // 
            this.lblRackName.Location = new System.Drawing.Point(57, 102);
            this.lblRackName.Name = "lblRackName";
            this.lblRackName.Size = new System.Drawing.Size(60, 20);
            this.lblRackName.TabIndex = 10;
            this.lblRackName.Values.Text = "货架名称";
            // 
            // txtRackName
            // 
            this.txtRackName.Location = new System.Drawing.Point(130, 98);
            this.txtRackName.Name = "txtRackName";
            this.txtRackName.Size = new System.Drawing.Size(235, 20);
            this.txtRackName.TabIndex = 11;
            // 
            // lblRackLocation
            // 
            this.lblRackLocation.Location = new System.Drawing.Point(57, 143);
            this.lblRackLocation.Name = "lblRackLocation";
            this.lblRackLocation.Size = new System.Drawing.Size(60, 20);
            this.lblRackLocation.TabIndex = 12;
            this.lblRackLocation.Values.Text = "货架位置";
            // 
            // txtRackLocation
            // 
            this.txtRackLocation.Location = new System.Drawing.Point(130, 139);
            this.txtRackLocation.Name = "txtRackLocation";
            this.txtRackLocation.Size = new System.Drawing.Size(235, 20);
            this.txtRackLocation.TabIndex = 13;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(57, 196);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(35, 20);
            this.lblDesc.TabIndex = 14;
            this.lblDesc.Values.Text = "描述";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(130, 196);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(235, 96);
            this.txtDesc.TabIndex = 15;
            // 
            // UCStorageRackEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 379);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCStorageRackEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocation_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblLocation_ID;
        private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;
        private Krypton.Toolkit.KryptonLabel lblRackNO;
        private Krypton.Toolkit.KryptonTextBox txtRackNO;
        private Krypton.Toolkit.KryptonLabel lblRackName;
        private Krypton.Toolkit.KryptonTextBox txtRackName;
        private Krypton.Toolkit.KryptonLabel lblRackLocation;
        private Krypton.Toolkit.KryptonTextBox txtRackLocation;
        private Krypton.Toolkit.KryptonLabel lblDesc;
        private Krypton.Toolkit.KryptonTextBox txtDesc;
    }
}
