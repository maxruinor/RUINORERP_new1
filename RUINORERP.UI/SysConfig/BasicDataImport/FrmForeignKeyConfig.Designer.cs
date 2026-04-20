namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    partial class FrmForeignKeyConfig
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.kbtnSelectSourceField = new Krypton.Toolkit.KryptonButton();
            this.kcmbSourceField = new Krypton.Toolkit.KryptonComboBox();
            this.klblSourceField = new Krypton.Toolkit.KryptonLabel();
            this.ktxtSourceField = new Krypton.Toolkit.KryptonTextBox();
            this.kbtnSelectKeyField = new Krypton.Toolkit.KryptonButton();
            this.kcmbKeyField = new Krypton.Toolkit.KryptonComboBox();
            this.klblKeyField = new Krypton.Toolkit.KryptonLabel();
            this.ktxtKeyField = new Krypton.Toolkit.KryptonTextBox();
            this.kbtnSelectTable = new Krypton.Toolkit.KryptonButton();
            this.kcmbSelectTable = new Krypton.Toolkit.KryptonComboBox();
            this.klblTableName = new Krypton.Toolkit.KryptonLabel();
            this.ktxtTableName = new Krypton.Toolkit.KryptonTextBox();
            this.kchkAutoCreate = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonPanel2 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnOK = new Krypton.Toolkit.KryptonButton();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbSourceField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbKeyField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbSelectTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(600, 450);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.kbtnSelectSourceField);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kcmbSourceField);
            this.kryptonGroupBox1.Panel.Controls.Add(this.klblSourceField);
            this.kryptonGroupBox1.Panel.Controls.Add(this.ktxtSourceField);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kbtnSelectKeyField);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kcmbKeyField);
            this.kryptonGroupBox1.Panel.Controls.Add(this.klblKeyField);
            this.kryptonGroupBox1.Panel.Controls.Add(this.ktxtKeyField);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kbtnSelectTable);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kcmbSelectTable);
            this.kryptonGroupBox1.Panel.Controls.Add(this.klblTableName);
            this.kryptonGroupBox1.Panel.Controls.Add(this.ktxtTableName);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kchkAutoCreate);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(600, 450);
            this.kryptonGroupBox1.TabIndex = 0;
            this.kryptonGroupBox1.Values.Heading = "外键关联配置";
            // 
            // kbtnSelectSourceField
            // 
            this.kbtnSelectSourceField.Location = new System.Drawing.Point(520, 180);
            this.kbtnSelectSourceField.Name = "kbtnSelectSourceField";
            this.kbtnSelectSourceField.Size = new System.Drawing.Size(60, 25);
            this.kbtnSelectSourceField.TabIndex = 9;
            this.kbtnSelectSourceField.Values.Text = "选择";
            this.kbtnSelectSourceField.Click += new System.EventHandler(this.kbtnSelectSourceField_Click);
            // 
            // kcmbSourceField
            // 
            this.kcmbSourceField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbSourceField.Location = new System.Drawing.Point(350, 180);
            this.kcmbSourceField.Name = "kcmbSourceField";
            this.kcmbSourceField.Size = new System.Drawing.Size(160, 25);
            this.kcmbSourceField.TabIndex = 8;
            this.kcmbSourceField.SelectedIndexChanged += new System.EventHandler(this.kcmbSelectTable_SelectedIndexChanged);
            // 
            // klblSourceField
            // 
            this.klblSourceField.Location = new System.Drawing.Point(20, 182);
            this.klblSourceField.Name = "klblSourceField";
            this.klblSourceField.Size = new System.Drawing.Size(70, 22);
            this.klblSourceField.TabIndex = 7;
            this.klblSourceField.Values.Text = "源字段:";
            // 
            // ktxtSourceField
            // 
            this.ktxtSourceField.Location = new System.Drawing.Point(100, 180);
            this.ktxtSourceField.Name = "ktxtSourceField";
            this.ktxtSourceField.Size = new System.Drawing.Size(240, 25);
            this.ktxtSourceField.TabIndex = 6;
            // 
            // kbtnSelectKeyField
            // 
            this.kbtnSelectKeyField.Location = new System.Drawing.Point(520, 140);
            this.kbtnSelectKeyField.Name = "kbtnSelectKeyField";
            this.kbtnSelectKeyField.Size = new System.Drawing.Size(60, 25);
            this.kbtnSelectKeyField.TabIndex = 5;
            this.kbtnSelectKeyField.Values.Text = "选择";
            this.kbtnSelectKeyField.Click += new System.EventHandler(this.kbtnSelectKeyField_Click);
            // 
            // kcmbKeyField
            // 
            this.kcmbKeyField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbKeyField.Location = new System.Drawing.Point(350, 140);
            this.kcmbKeyField.Name = "kcmbKeyField";
            this.kcmbKeyField.Size = new System.Drawing.Size(160, 25);
            this.kcmbKeyField.TabIndex = 4;
            this.kcmbKeyField.SelectedIndexChanged += new System.EventHandler(this.kcmbSelectTable_SelectedIndexChanged);
            // 
            // klblKeyField
            // 
            this.klblKeyField.Location = new System.Drawing.Point(20, 142);
            this.klblKeyField.Name = "klblKeyField";
            this.klblKeyField.Size = new System.Drawing.Size(70, 22);
            this.klblKeyField.TabIndex = 3;
            this.klblKeyField.Values.Text = "主键字段:";
            // 
            // ktxtKeyField
            // 
            this.ktxtKeyField.Location = new System.Drawing.Point(100, 140);
            this.ktxtKeyField.Name = "ktxtKeyField";
            this.ktxtKeyField.Size = new System.Drawing.Size(240, 25);
            this.ktxtKeyField.TabIndex = 2;
            // 
            // kbtnSelectTable
            // 
            this.kbtnSelectTable.Location = new System.Drawing.Point(520, 100);
            this.kbtnSelectTable.Name = "kbtnSelectTable";
            this.kbtnSelectTable.Size = new System.Drawing.Size(60, 25);
            this.kbtnSelectTable.TabIndex = 1;
            this.kbtnSelectTable.Values.Text = "选择";
            this.kbtnSelectTable.Click += new System.EventHandler(this.kbtnSelectTable_Click);
            // 
            // kcmbSelectTable
            // 
            this.kcmbSelectTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbSelectTable.Location = new System.Drawing.Point(350, 100);
            this.kcmbSelectTable.Name = "kcmbSelectTable";
            this.kcmbSelectTable.Size = new System.Drawing.Size(160, 25);
            this.kcmbSelectTable.TabIndex = 0;
            this.kcmbSelectTable.SelectedIndexChanged += new System.EventHandler(this.kcmbSelectTable_SelectedIndexChanged);
            // 
            // klblTableName
            // 
            this.klblTableName.Location = new System.Drawing.Point(20, 102);
            this.klblTableName.Name = "klblTableName";
            this.klblTableName.Size = new System.Drawing.Size(70, 22);
            this.klblTableName.TabIndex = 0;
            this.klblTableName.Values.Text = "关联表:";
            // 
            // ktxtTableName
            // 
            this.ktxtTableName.Location = new System.Drawing.Point(100, 100);
            this.ktxtTableName.Name = "ktxtTableName";
            this.ktxtTableName.Size = new System.Drawing.Size(240, 25);
            this.ktxtTableName.TabIndex = 0;
            // 
            // kchkAutoCreate
            // 
            this.kchkAutoCreate.Location = new System.Drawing.Point(100, 220);
            this.kchkAutoCreate.Name = "kchkAutoCreate";
            this.kchkAutoCreate.Size = new System.Drawing.Size(150, 22);
            this.kchkAutoCreate.TabIndex = 10;
            this.kchkAutoCreate.Values.Text = "不存在时自动创建";
            // 
            // kryptonPanel2
            // 
            this.kryptonPanel2.Controls.Add(this.kbtnOK);
            this.kryptonPanel2.Controls.Add(this.kbtnCancel);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 390);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(600, 60);
            this.kryptonPanel2.TabIndex = 1;
            // 
            // kbtnOK
            // 
            this.kbtnOK.Location = new System.Drawing.Point(400, 15);
            this.kbtnOK.Name = "kbtnOK";
            this.kbtnOK.Size = new System.Drawing.Size(80, 30);
            this.kbtnOK.TabIndex = 1;
            this.kbtnOK.Values.Text = "确定";
            this.kbtnOK.Click += new System.EventHandler(this.kbtnOK_Click);
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(500, 15);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(80, 30);
            this.kbtnCancel.TabIndex = 0;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.kbtnCancel_Click);
            // 
            // FrmForeignKeyConfig
            // 
            this.AcceptButton = this.kbtnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.kbtnCancel;
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.kryptonPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmForeignKeyConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "外键关联配置";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kcmbSourceField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbKeyField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbSelectTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private Krypton.Toolkit.KryptonButton kbtnOK;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonLabel klblTableName;
        private Krypton.Toolkit.KryptonTextBox ktxtTableName;
        private Krypton.Toolkit.KryptonButton kbtnSelectTable;
        private Krypton.Toolkit.KryptonComboBox kcmbSelectTable;
        private Krypton.Toolkit.KryptonButton kbtnSelectKeyField;
        private Krypton.Toolkit.KryptonComboBox kcmbKeyField;
        private Krypton.Toolkit.KryptonLabel klblKeyField;
        private Krypton.Toolkit.KryptonTextBox ktxtKeyField;
        private Krypton.Toolkit.KryptonButton kbtnSelectSourceField;
        private Krypton.Toolkit.KryptonComboBox kcmbSourceField;
        private Krypton.Toolkit.KryptonLabel klblSourceField;
        private Krypton.Toolkit.KryptonTextBox ktxtSourceField;
        private Krypton.Toolkit.KryptonCheckBox kchkAutoCreate;
    }
}
