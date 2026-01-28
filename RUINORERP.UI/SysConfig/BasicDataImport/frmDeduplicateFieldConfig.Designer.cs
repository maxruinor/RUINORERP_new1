namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    partial class frmDeduplicateFieldConfig
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
            this.chkIgnoreEmptyValues = new Krypton.Toolkit.KryptonCheckBox();
            this.klblFieldCount = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.listBoxSelectedFields = new System.Windows.Forms.ListBox();
            this.chkListAvailableFields = new System.Windows.Forms.CheckedListBox();
            this.kbtnSelectNone = new Krypton.Toolkit.KryptonButton();
            this.kbtnSelectAll = new Krypton.Toolkit.KryptonButton();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            this.kbtnOK = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.chkIgnoreEmptyValues);
            this.kryptonPanel1.Controls.Add(this.klblFieldCount);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.listBoxSelectedFields);
            this.kryptonPanel1.Controls.Add(this.chkListAvailableFields);
            this.kryptonPanel1.Controls.Add(this.kbtnSelectNone);
            this.kryptonPanel1.Controls.Add(this.kbtnSelectAll);
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Controls.Add(this.kbtnOK);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(600, 450);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // chkIgnoreEmptyValues
            // 
            this.chkIgnoreEmptyValues.Checked = true;
            this.chkIgnoreEmptyValues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreEmptyValues.Location = new System.Drawing.Point(33, 410);
            this.chkIgnoreEmptyValues.Name = "chkIgnoreEmptyValues";
            this.chkIgnoreEmptyValues.Size = new System.Drawing.Size(192, 20);
            this.chkIgnoreEmptyValues.TabIndex = 11;
            this.chkIgnoreEmptyValues.Values.Text = "忽略空值（去重时跳过空值）";
            this.chkIgnoreEmptyValues.CheckedChanged += new System.EventHandler(this.chkIgnoreEmptyValues_CheckedChanged);
            // 
            // klblFieldCount
            // 
            this.klblFieldCount.Location = new System.Drawing.Point(33, 340);
            this.klblFieldCount.Name = "klblFieldCount";
            this.klblFieldCount.Size = new System.Drawing.Size(127, 20);
            this.klblFieldCount.TabIndex = 10;
            this.klblFieldCount.Values.Text = "已选择 0 个去重字段";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(420, 25);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(91, 20);
            this.kryptonLabel2.TabIndex = 9;
            this.kryptonLabel2.Values.Text = "已选择的字段:";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(20, 25);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(115, 20);
            this.kryptonLabel1.TabIndex = 8;
            this.kryptonLabel1.Values.Text = "可用字段 (请选择):";
            // 
            // listBoxSelectedFields
            // 
            this.listBoxSelectedFields.FormattingEnabled = true;
            this.listBoxSelectedFields.ItemHeight = 12;
            this.listBoxSelectedFields.Location = new System.Drawing.Point(420, 50);
            this.listBoxSelectedFields.Name = "listBoxSelectedFields";
            this.listBoxSelectedFields.Size = new System.Drawing.Size(160, 268);
            this.listBoxSelectedFields.TabIndex = 2;
            // 
            // chkListAvailableFields
            // 
            this.chkListAvailableFields.CheckOnClick = true;
            this.chkListAvailableFields.FormattingEnabled = true;
            this.chkListAvailableFields.Location = new System.Drawing.Point(20, 50);
            this.chkListAvailableFields.Name = "chkListAvailableFields";
            this.chkListAvailableFields.Size = new System.Drawing.Size(370, 260);
            this.chkListAvailableFields.TabIndex = 1;
            this.chkListAvailableFields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListAvailableFields_ItemCheck);
            // 
            // kbtnSelectNone
            // 
            this.kbtnSelectNone.Location = new System.Drawing.Point(353, 340);
            this.kbtnSelectNone.Name = "kbtnSelectNone";
            this.kbtnSelectNone.Size = new System.Drawing.Size(80, 25);
            this.kbtnSelectNone.TabIndex = 5;
            this.kbtnSelectNone.Values.Text = "全不选";
            this.kbtnSelectNone.Click += new System.EventHandler(this.kbtnSelectNone_Click);
            // 
            // kbtnSelectAll
            // 
            this.kbtnSelectAll.Location = new System.Drawing.Point(263, 340);
            this.kbtnSelectAll.Name = "kbtnSelectAll";
            this.kbtnSelectAll.Size = new System.Drawing.Size(80, 25);
            this.kbtnSelectAll.TabIndex = 4;
            this.kbtnSelectAll.Values.Text = "全选";
            this.kbtnSelectAll.Click += new System.EventHandler(this.kbtnSelectAll_Click);
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(500, 400);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(80, 30);
            this.kbtnCancel.TabIndex = 7;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.kbtnCancel_Click);
            // 
            // kbtnOK
            // 
            this.kbtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.kbtnOK.Location = new System.Drawing.Point(410, 400);
            this.kbtnOK.Name = "kbtnOK";
            this.kbtnOK.Size = new System.Drawing.Size(80, 30);
            this.kbtnOK.TabIndex = 6;
            this.kbtnOK.Values.Text = "确定";
            this.kbtnOK.Click += new System.EventHandler(this.kbtnOK_Click);
            // 
            // frmDeduplicateFieldConfig
            // 
            this.AcceptButton = this.kbtnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.kbtnCancel;
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "frmDeduplicateFieldConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "配置去重字段";
            this.Load += new System.EventHandler(this.frmDeduplicateFieldConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kbtnSelectAll;
        private Krypton.Toolkit.KryptonButton kbtnSelectNone;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonButton kbtnOK;
        private System.Windows.Forms.CheckedListBox chkListAvailableFields;
        private System.Windows.Forms.ListBox listBoxSelectedFields;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonLabel klblFieldCount;
        private Krypton.Toolkit.KryptonCheckBox chkIgnoreEmptyValues;
    }
}
