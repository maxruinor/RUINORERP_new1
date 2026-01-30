namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    public partial class FrmMultiAttributeImportConfig
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
            this.kbtnOK = new Krypton.Toolkit.KryptonButton();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonLabel6 = new Krypton.Toolkit.KryptonLabel();
            this.txtCNNameCleanupPattern = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel5 = new Krypton.Toolkit.KryptonLabel();
            this.txtUnitColumn = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            this.txtSpecificationsColumn = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.txtCNNameColumn = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.txtProductNoColumn = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.chkEnableMultiAttribute = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonLabel7 = new Krypton.Toolkit.KryptonLabel();
            this.txtGroupByField = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonGroupBox1.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Controls.Add(this.kbtnOK);
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(684, 461);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kbtnOK
            // 
            this.kbtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kbtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.kbtnOK.Location = new System.Drawing.Point(508, 426);
            this.kbtnOK.Name = "kbtnOK";
            this.kbtnOK.Size = new System.Drawing.Size(75, 23);
            this.kbtnOK.TabIndex = 3;
            this.kbtnOK.Values.Text = "确定";
            this.kbtnOK.Click += new System.EventHandler(this.kbtnOK_Click);
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(593, 426);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(75, 23);
            this.kbtnCancel.TabIndex = 4;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.kbtnCancel_Click);
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Controls.Add(this.kryptonLabel6);
            this.kryptonGroupBox1.Controls.Add(this.txtCNNameCleanupPattern);
            this.kryptonGroupBox1.Controls.Add(this.kryptonLabel5);
            this.kryptonGroupBox1.Controls.Add(this.txtUnitColumn);
            this.kryptonGroupBox1.Controls.Add(this.kryptonLabel4);
            this.kryptonGroupBox1.Controls.Add(this.txtSpecificationsColumn);
            this.kryptonGroupBox1.Controls.Add(this.kryptonLabel3);
            this.kryptonGroupBox1.Controls.Add(this.txtCNNameColumn);
            this.kryptonGroupBox1.Controls.Add(this.kryptonLabel2);
            this.kryptonGroupBox1.Controls.Add(this.txtProductNoColumn);
            this.kryptonGroupBox1.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBox1.Controls.Add(this.chkEnableMultiAttribute);
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            this.kryptonGroupBox1.Size = new System.Drawing.Size(684, 120);
            this.kryptonGroupBox1.TabIndex = 0;
            this.kryptonGroupBox1.Values.Heading = "基础产品字段映射";
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Location = new System.Drawing.Point(435, 75);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(115, 20);
            this.kryptonLabel6.TabIndex = 10;
            this.kryptonLabel6.Values.Text = "品名清理规则：";
            // 
            // txtCNNameCleanupPattern
            // 
            this.txtCNNameCleanupPattern.Location = new System.Drawing.Point(435, 95);
            this.txtCNNameCleanupPattern.Name = "txtCNNameCleanupPattern";
            this.txtCNNameCleanupPattern.Size = new System.Drawing.Size(233, 22);
            this.txtCNNameCleanupPattern.TabIndex = 9;
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(289, 75);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(40, 20);
            this.kryptonLabel5.TabIndex = 8;
            this.kryptonLabel5.Values.Text = "单位：";
            // 
            // txtUnitColumn
            // 
            this.txtUnitColumn.Location = new System.Drawing.Point(289, 95);
            this.txtUnitColumn.Name = "txtUnitColumn";
            this.txtUnitColumn.Size = new System.Drawing.Size(130, 22);
            this.txtUnitColumn.TabIndex = 7;
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(143, 75);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(40, 20);
            this.kryptonLabel4.TabIndex = 6;
            this.kryptonLabel4.Values.Text = "规格：";
            // 
            // txtSpecificationsColumn
            // 
            this.txtSpecificationsColumn.Location = new System.Drawing.Point(143, 95);
            this.txtSpecificationsColumn.Name = "txtSpecificationsColumn";
            this.txtSpecificationsColumn.Size = new System.Drawing.Size(130, 22);
            this.txtSpecificationsColumn.TabIndex = 5;
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(12, 75);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(40, 20);
            this.kryptonLabel3.TabIndex = 4;
            this.kryptonLabel3.Values.Text = "品名：";
            // 
            // txtCNNameColumn
            // 
            this.txtCNNameColumn.Location = new System.Drawing.Point(12, 95);
            this.txtCNNameColumn.Name = "txtCNNameColumn";
            this.txtCNNameColumn.Size = new System.Drawing.Size(115, 22);
            this.txtCNNameColumn.TabIndex = 3;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(12, 35);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(40, 20);
            this.kryptonLabel2.TabIndex = 2;
            this.kryptonLabel2.Values.Text = "品号：";
            // 
            // txtProductNoColumn
            // 
            this.txtProductNoColumn.Location = new System.Drawing.Point(12, 55);
            this.txtProductNoColumn.Name = "txtProductNoColumn";
            this.txtProductNoColumn.Size = new System.Drawing.Size(261, 22);
            this.txtProductNoColumn.TabIndex = 1;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(12, 13);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(175, 20);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "启用多属性产品导入：";
            // 
            // chkEnableMultiAttribute
            // 
            this.chkEnableMultiAttribute.Location = new System.Drawing.Point(193, 13);
            this.chkEnableMultiAttribute.Name = "chkEnableMultiAttribute";
            this.chkEnableMultiAttribute.Size = new System.Drawing.Size(90, 20);
            this.chkEnableMultiAttribute.TabIndex = 0;
            this.chkEnableMultiAttribute.Values.Text = "启用";
            this.chkEnableMultiAttribute.CheckedChanged += new System.EventHandler(this.chkEnableMultiAttribute_CheckedChanged);
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.Controls.Add(this.kryptonLabel7);
            this.kryptonGroupBox2.Controls.Add(this.txtGroupByField);
            this.kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(0, 120);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            this.kryptonGroupBox2.Size = new System.Drawing.Size(684, 60);
            this.kryptonGroupBox2.TabIndex = 1;
            this.kryptonGroupBox2.Values.Heading = "产品分组";
            // 
            // kryptonLabel7
            // 
            this.kryptonLabel7.Location = new System.Drawing.Point(12, 13);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(180, 20);
            this.kryptonLabel7.TabIndex = 1;
            this.kryptonLabel7.Values.Text = "产品分组字段（如：末位流水编号）：";
            // 
            // txtGroupByField
            // 
            this.txtGroupByField.Location = new System.Drawing.Point(12, 33);
            this.txtGroupByField.Name = "txtGroupByField";
            this.txtGroupByField.Size = new System.Drawing.Size(260, 22);
            this.txtGroupByField.TabIndex = 0;
            // 
            // FrmMultiAttributeImportConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "FrmMultiAttributeImportConfig";
            this.Text = "多属性产品导入配置";
            this.kryptonGroupBox1.ResumeLayout(false);
            this.kryptonGroupBox1.PerformLayout();
            this.kryptonGroupBox2.ResumeLayout(false);
            this.kryptonGroupBox2.PerformLayout();
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kbtnOK;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonCheckBox chkEnableMultiAttribute;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonTextBox txtProductNoColumn;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonTextBox txtCNNameColumn;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonTextBox txtSpecificationsColumn;
        private Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private Krypton.Toolkit.KryptonTextBox txtUnitColumn;
        private Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private Krypton.Toolkit.KryptonTextBox txtCNNameCleanupPattern;
        private Krypton.Toolkit.KryptonLabel kryptonLabel7;
        private Krypton.Toolkit.KryptonTextBox txtGroupByField;
    }
}
