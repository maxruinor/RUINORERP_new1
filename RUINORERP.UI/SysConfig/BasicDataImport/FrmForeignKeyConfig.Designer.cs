namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    partial class FrmForeignKeyConfig
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmForeignKeyConfig));
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            this.kbtnOK = new Krypton.Toolkit.KryptonButton();
            this.ktxtRelatedField = new Krypton.Toolkit.KryptonTextBox();
            this.kcmbRelatedTable = new Krypton.Toolkit.KryptonComboBox();
            this.kchkIsForeignKey = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbRelatedTable)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Controls.Add(this.kbtnOK);
            this.kryptonPanel1.Controls.Add(this.ktxtRelatedField);
            this.kryptonPanel1.Controls.Add(this.kcmbRelatedTable);
            this.kryptonPanel1.Controls.Add(this.kchkIsForeignKey);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(400, 200);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.Location = new System.Drawing.Point(250, 150);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(80, 30);
            this.kbtnCancel.TabIndex = 7;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.kbtnCancel_Click);
            // 
            // kbtnOK
            // 
            this.kbtnOK.Location = new System.Drawing.Point(150, 150);
            this.kbtnOK.Name = "kbtnOK";
            this.kbtnOK.Size = new System.Drawing.Size(80, 30);
            this.kbtnOK.TabIndex = 6;
            this.kbtnOK.Values.Text = "确定";
            this.kbtnOK.Click += new System.EventHandler(this.kbtnOK_Click);
            // 
            // ktxtRelatedField
            // 
            this.ktxtRelatedField.Location = new System.Drawing.Point(120, 110);
            this.ktxtRelatedField.Name = "ktxtRelatedField";
            this.ktxtRelatedField.Size = new System.Drawing.Size(210, 23);
            this.ktxtRelatedField.TabIndex = 5;
            // 
            // kcmbRelatedTable
            // 
            this.kcmbRelatedTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbRelatedTable.DropDownWidth = 200;
            this.kcmbRelatedTable.IntegralHeight = false;
            this.kcmbRelatedTable.Location = new System.Drawing.Point(120, 70);
            this.kcmbRelatedTable.Name = "kcmbRelatedTable";
            this.kcmbRelatedTable.Size = new System.Drawing.Size(210, 21);
            this.kcmbRelatedTable.TabIndex = 4;
            // 
            // kchkIsForeignKey
            // 
            this.kchkIsForeignKey.Location = new System.Drawing.Point(120, 30);
            this.kchkIsForeignKey.Name = "kchkIsForeignKey";
            this.kchkIsForeignKey.Size = new System.Drawing.Size(100, 20);
            this.kchkIsForeignKey.TabIndex = 3;
            this.kchkIsForeignKey.Values.Text = "是外键";
            this.kchkIsForeignKey.CheckedChanged += new System.EventHandler(this.kchkIsForeignKey_CheckedChanged);
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(30, 115);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(85, 20);
            this.kryptonLabel3.TabIndex = 2;
            this.kryptonLabel3.Values.Text = "关联表字段:";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(30, 75);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel2.TabIndex = 1;
            this.kryptonLabel2.Values.Text = "关联表:";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(30, 35);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "外键设置:";
            // 
            // FrmForeignKeyConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "FrmForeignKeyConfig";
            this.Text = "外键配置";
            this.Load += new System.EventHandler(this.FrmForeignKeyConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbRelatedTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonButton kbtnOK;
        private Krypton.Toolkit.KryptonTextBox ktxtRelatedField;
        private Krypton.Toolkit.KryptonComboBox kcmbRelatedTable;
        private Krypton.Toolkit.KryptonCheckBox kchkIsForeignKey;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}
