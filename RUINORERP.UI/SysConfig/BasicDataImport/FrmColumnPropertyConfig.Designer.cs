namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    partial class FrmColumnPropertyConfig
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            this.kbtnOK = new Krypton.Toolkit.KryptonButton();
            this.kchkIsUniqueValue = new Krypton.Toolkit.KryptonCheckBox();
            this.kchkIsSystemGenerated = new Krypton.Toolkit.KryptonCheckBox();
            this.ktxtDefaultValue = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel5 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            this.ktxtRelatedField = new Krypton.Toolkit.KryptonComboBox();
            this.kcmbRelatedTable = new Krypton.Toolkit.KryptonComboBox();
            this.kchkIsForeignKey = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ktxtRelatedField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbRelatedTable)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Controls.Add(this.kbtnOK);
            this.kryptonPanel1.Controls.Add(this.kchkIsUniqueValue);
            this.kryptonPanel1.Controls.Add(this.kchkIsSystemGenerated);
            this.kryptonPanel1.Controls.Add(this.ktxtDefaultValue);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel5);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.ktxtRelatedField);
            this.kryptonPanel1.Controls.Add(this.kcmbRelatedTable);
            this.kryptonPanel1.Controls.Add(this.kchkIsForeignKey);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(450, 380);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(280, 330);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(80, 30);
            this.kbtnCancel.TabIndex = 13;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.kbtnCancel_Click);
            // 
            // kbtnOK
            // 
            this.kbtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.kbtnOK.Location = new System.Drawing.Point(180, 330);
            this.kbtnOK.Name = "kbtnOK";
            this.kbtnOK.Size = new System.Drawing.Size(80, 30);
            this.kbtnOK.TabIndex = 12;
            this.kbtnOK.Values.Text = "确定";
            this.kbtnOK.Click += new System.EventHandler(this.kbtnOK_Click);
            // 
            // kchkIsUniqueValue
            // 
            this.kchkIsUniqueValue.Location = new System.Drawing.Point(325, 30);
            this.kchkIsUniqueValue.Name = "kchkIsUniqueValue";
            this.kchkIsUniqueValue.Size = new System.Drawing.Size(75, 20);
            this.kchkIsUniqueValue.TabIndex = 11;
            this.kchkIsUniqueValue.Values.Text = "值唯一性";
            // 
            // kchkIsSystemGenerated
            // 
            this.kchkIsSystemGenerated.Location = new System.Drawing.Point(120, 130);
            this.kchkIsSystemGenerated.Name = "kchkIsSystemGenerated";
            this.kchkIsSystemGenerated.Size = new System.Drawing.Size(75, 20);
            this.kchkIsSystemGenerated.TabIndex = 10;
            this.kchkIsSystemGenerated.Values.Text = "系统生成";
            this.kchkIsSystemGenerated.CheckedChanged += new System.EventHandler(this.kchkIsSystemGenerated_CheckedChanged);
            // 
            // ktxtDefaultValue
            // 
            this.ktxtDefaultValue.Location = new System.Drawing.Point(120, 190);
            this.ktxtDefaultValue.Name = "ktxtDefaultValue";
            this.ktxtDefaultValue.Size = new System.Drawing.Size(280, 23);
            this.ktxtDefaultValue.TabIndex = 9;
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(30, 190);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(52, 20);
            this.kryptonLabel5.TabIndex = 8;
            this.kryptonLabel5.Values.Text = "默认值:";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(30, 130);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel4.TabIndex = 7;
            this.kryptonLabel4.Values.Text = "系统生成:";
            // 
            // ktxtRelatedField
            // 
            this.ktxtRelatedField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ktxtRelatedField.DropDownWidth = 200;
            this.ktxtRelatedField.IntegralHeight = false;
            this.ktxtRelatedField.Location = new System.Drawing.Point(120, 270);
            this.ktxtRelatedField.Name = "ktxtRelatedField";
            this.ktxtRelatedField.Size = new System.Drawing.Size(280, 21);
            this.ktxtRelatedField.TabIndex = 6;
            this.ktxtRelatedField.SelectedIndexChanged += new System.EventHandler(this.ktxtRelatedField_SelectedIndexChanged);
            // 
            // kcmbRelatedTable
            // 
            this.kcmbRelatedTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbRelatedTable.DropDownWidth = 200;
            this.kcmbRelatedTable.IntegralHeight = false;
            this.kcmbRelatedTable.Location = new System.Drawing.Point(120, 230);
            this.kcmbRelatedTable.Name = "kcmbRelatedTable";
            this.kcmbRelatedTable.Size = new System.Drawing.Size(280, 21);
            this.kcmbRelatedTable.TabIndex = 5;
            this.kcmbRelatedTable.SelectedIndexChanged += new System.EventHandler(this.kcmbRelatedTable_SelectedIndexChanged);
            // 
            // kchkIsForeignKey
            // 
            this.kchkIsForeignKey.Location = new System.Drawing.Point(120, 30);
            this.kchkIsForeignKey.Name = "kchkIsForeignKey";
            this.kchkIsForeignKey.Size = new System.Drawing.Size(62, 20);
            this.kchkIsForeignKey.TabIndex = 4;
            this.kchkIsForeignKey.Values.Text = "是外键";
            this.kchkIsForeignKey.CheckedChanged += new System.EventHandler(this.kchkIsForeignKey_CheckedChanged);
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(30, 270);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel3.TabIndex = 3;
            this.kryptonLabel3.Values.Text = "关联字段:";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(30, 230);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(52, 20);
            this.kryptonLabel2.TabIndex = 2;
            this.kryptonLabel2.Values.Text = "关联表:";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(30, 30);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(39, 20);
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "外键:";
            // 
            // FrmColumnPropertyConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 380);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "FrmColumnPropertyConfig";
            this.Text = "设置列属性";
            this.Load += new System.EventHandler(this.FrmColumnPropertyConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ktxtRelatedField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbRelatedTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonButton kbtnOK;
        private Krypton.Toolkit.KryptonCheckBox kchkIsUniqueValue;
        private Krypton.Toolkit.KryptonCheckBox kchkIsSystemGenerated;
        private Krypton.Toolkit.KryptonTextBox ktxtDefaultValue;
        private Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonComboBox ktxtRelatedField;
        private Krypton.Toolkit.KryptonComboBox kcmbRelatedTable;
        private Krypton.Toolkit.KryptonCheckBox kchkIsForeignKey;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}
