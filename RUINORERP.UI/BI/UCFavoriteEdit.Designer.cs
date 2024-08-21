namespace RUINORERP.UI.BI
{
    partial class UCFavoriteEdit
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
            this.lblRef_Table_Name = new Krypton.Toolkit.KryptonLabel();
            this.txtRef_Table_Name = new Krypton.Toolkit.KryptonTextBox();
            this.lblModuleName = new Krypton.Toolkit.KryptonLabel();
            this.txtModuleName = new Krypton.Toolkit.KryptonTextBox();
            this.lblBusinessType = new Krypton.Toolkit.KryptonLabel();
            this.txtBusinessType = new Krypton.Toolkit.KryptonTextBox();
            this.lblPubli_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chkPubli_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblis_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chkis_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblis_available = new Krypton.Toolkit.KryptonLabel();
            this.chkis_available = new Krypton.Toolkit.KryptonCheckBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(188, 361);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(306, 361);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblRef_Table_Name);
            this.kryptonPanel1.Controls.Add(this.txtRef_Table_Name);
            this.kryptonPanel1.Controls.Add(this.lblModuleName);
            this.kryptonPanel1.Controls.Add(this.txtModuleName);
            this.kryptonPanel1.Controls.Add(this.lblBusinessType);
            this.kryptonPanel1.Controls.Add(this.txtBusinessType);
            this.kryptonPanel1.Controls.Add(this.lblPubli_enabled);
            this.kryptonPanel1.Controls.Add(this.chkPubli_enabled);
            this.kryptonPanel1.Controls.Add(this.lblis_enabled);
            this.kryptonPanel1.Controls.Add(this.chkis_enabled);
            this.kryptonPanel1.Controls.Add(this.lblis_available);
            this.kryptonPanel1.Controls.Add(this.chkis_available);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(597, 407);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblRef_Table_Name
            // 
            this.lblRef_Table_Name.Location = new System.Drawing.Point(69, 16);
            this.lblRef_Table_Name.Name = "lblRef_Table_Name";
            this.lblRef_Table_Name.Size = new System.Drawing.Size(60, 20);
            this.lblRef_Table_Name.TabIndex = 13;
            this.lblRef_Table_Name.Values.Text = "引用表名";
            // 
            // txtRef_Table_Name
            // 
            this.txtRef_Table_Name.Location = new System.Drawing.Point(142, 12);
            this.txtRef_Table_Name.Name = "txtRef_Table_Name";
            this.txtRef_Table_Name.Size = new System.Drawing.Size(328, 20);
            this.txtRef_Table_Name.TabIndex = 14;
            // 
            // lblModuleName
            // 
            this.lblModuleName.Location = new System.Drawing.Point(81, 41);
            this.lblModuleName.Name = "lblModuleName";
            this.lblModuleName.Size = new System.Drawing.Size(48, 20);
            this.lblModuleName.TabIndex = 15;
            this.lblModuleName.Values.Text = "模块名";
            // 
            // txtModuleName
            // 
            this.txtModuleName.Location = new System.Drawing.Point(142, 37);
            this.txtModuleName.Multiline = true;
            this.txtModuleName.Name = "txtModuleName";
            this.txtModuleName.Size = new System.Drawing.Size(328, 21);
            this.txtModuleName.TabIndex = 16;
            // 
            // lblBusinessType
            // 
            this.lblBusinessType.Location = new System.Drawing.Point(69, 66);
            this.lblBusinessType.Name = "lblBusinessType";
            this.lblBusinessType.Size = new System.Drawing.Size(60, 20);
            this.lblBusinessType.TabIndex = 17;
            this.lblBusinessType.Values.Text = "业务类型";
            // 
            // txtBusinessType
            // 
            this.txtBusinessType.Location = new System.Drawing.Point(142, 62);
            this.txtBusinessType.Multiline = true;
            this.txtBusinessType.Name = "txtBusinessType";
            this.txtBusinessType.Size = new System.Drawing.Size(328, 21);
            this.txtBusinessType.TabIndex = 18;
            // 
            // lblPubli_enabled
            // 
            this.lblPubli_enabled.Location = new System.Drawing.Point(69, 91);
            this.lblPubli_enabled.Name = "lblPubli_enabled";
            this.lblPubli_enabled.Size = new System.Drawing.Size(60, 20);
            this.lblPubli_enabled.TabIndex = 19;
            this.lblPubli_enabled.Values.Text = "是否公开";
            // 
            // chkPubli_enabled
            // 
            this.chkPubli_enabled.Location = new System.Drawing.Point(142, 95);
            this.chkPubli_enabled.Name = "chkPubli_enabled";
            this.chkPubli_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkPubli_enabled.TabIndex = 20;
            this.chkPubli_enabled.Values.Text = "";
            // 
            // lblis_enabled
            // 
            this.lblis_enabled.Location = new System.Drawing.Point(233, 91);
            this.lblis_enabled.Name = "lblis_enabled";
            this.lblis_enabled.Size = new System.Drawing.Size(60, 20);
            this.lblis_enabled.TabIndex = 22;
            this.lblis_enabled.Values.Text = "是否启用";
            // 
            // chkis_enabled
            // 
            this.chkis_enabled.Location = new System.Drawing.Point(299, 95);
            this.chkis_enabled.Name = "chkis_enabled";
            this.chkis_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkis_enabled.TabIndex = 21;
            this.chkis_enabled.Values.Text = "";
            // 
            // lblis_available
            // 
            this.lblis_available.Location = new System.Drawing.Point(385, 91);
            this.lblis_available.Name = "lblis_available";
            this.lblis_available.Size = new System.Drawing.Size(60, 20);
            this.lblis_available.TabIndex = 23;
            this.lblis_available.Values.Text = "是否可用";
            // 
            // chkis_available
            // 
            this.chkis_available.Location = new System.Drawing.Point(451, 95);
            this.chkis_available.Name = "chkis_available";
            this.chkis_available.Size = new System.Drawing.Size(19, 13);
            this.chkis_available.TabIndex = 24;
            this.chkis_available.Values.Text = "";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(69, 143);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(60, 20);
            this.lblNotes.TabIndex = 25;
            this.lblNotes.Values.Text = "备注说明";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(142, 143);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(328, 192);
            this.txtNotes.TabIndex = 26;
            // 
            // UCFavoriteEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 407);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCFavoriteEdit";
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
        private Krypton.Toolkit.KryptonLabel lblRef_Table_Name;
        private Krypton.Toolkit.KryptonTextBox txtRef_Table_Name;
        private Krypton.Toolkit.KryptonLabel lblModuleName;
        private Krypton.Toolkit.KryptonTextBox txtModuleName;
        private Krypton.Toolkit.KryptonLabel lblBusinessType;
        private Krypton.Toolkit.KryptonTextBox txtBusinessType;
        private Krypton.Toolkit.KryptonLabel lblPubli_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkPubli_enabled;
        private Krypton.Toolkit.KryptonLabel lblis_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkis_enabled;
        private Krypton.Toolkit.KryptonLabel lblis_available;
        private Krypton.Toolkit.KryptonCheckBox chkis_available;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
    }
}
