namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    partial class frmColumnMappingConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmColumnMappingConfig));
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnAutoMatch = new Krypton.Toolkit.KryptonButton();
            this.kbtnSetUniqueKey = new Krypton.Toolkit.KryptonButton();
            this.kbtnRemoveMapping = new Krypton.Toolkit.KryptonButton();
            this.kbtnAddMapping = new Krypton.Toolkit.KryptonButton();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            this.kbtnSave = new Krypton.Toolkit.KryptonButton();
            this.listBoxMappings = new Krypton.Toolkit.KryptonListBox();
            this.listBoxSystemFields = new Krypton.Toolkit.KryptonListBox();
            this.listBoxExcelColumns = new Krypton.Toolkit.KryptonListBox();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kbtnAutoMatch);
            this.kryptonPanel1.Controls.Add(this.kbtnSetUniqueKey);
            this.kryptonPanel1.Controls.Add(this.kbtnRemoveMapping);
            this.kryptonPanel1.Controls.Add(this.kbtnAddMapping);
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Controls.Add(this.kbtnSave);
            this.kryptonPanel1.Controls.Add(this.listBoxMappings);
            this.kryptonPanel1.Controls.Add(this.listBoxSystemFields);
            this.kryptonPanel1.Controls.Add(this.listBoxExcelColumns);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(800, 600);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kbtnAutoMatch
            // 
            this.kbtnAutoMatch.Location = new System.Drawing.Point(350, 250);
            this.kbtnAutoMatch.Name = "kbtnAutoMatch";
            this.kbtnAutoMatch.Size = new System.Drawing.Size(100, 25);
            this.kbtnAutoMatch.TabIndex = 11;
            this.kbtnAutoMatch.Values.Text = "自动匹配";
            this.kbtnAutoMatch.Click += new System.EventHandler(this.kbtnAutoMatch_Click);
            // 
            // kbtnSetUniqueKey
            // 
            this.kbtnSetUniqueKey.Location = new System.Drawing.Point(550, 250);
            this.kbtnSetUniqueKey.Name = "kbtnSetUniqueKey";
            this.kbtnSetUniqueKey.Size = new System.Drawing.Size(100, 25);
            this.kbtnSetUniqueKey.TabIndex = 10;
            this.kbtnSetUniqueKey.Values.Text = "设置唯一键";
            this.kbtnSetUniqueKey.Click += new System.EventHandler(this.kbtnSetUniqueKey_Click);
            // 
            // kbtnRemoveMapping
            // 
            this.kbtnRemoveMapping.Location = new System.Drawing.Point(660, 250);
            this.kbtnRemoveMapping.Name = "kbtnRemoveMapping";
            this.kbtnRemoveMapping.Size = new System.Drawing.Size(100, 25);
            this.kbtnRemoveMapping.TabIndex = 9;
            this.kbtnRemoveMapping.Values.Text = "删除映射";
            this.kbtnRemoveMapping.Click += new System.EventHandler(this.kbtnRemoveMapping_Click);
            // 
            // kbtnAddMapping
            // 
            this.kbtnAddMapping.Location = new System.Drawing.Point(460, 250);
            this.kbtnAddMapping.Name = "kbtnAddMapping";
            this.kbtnAddMapping.Size = new System.Drawing.Size(80, 25);
            this.kbtnAddMapping.TabIndex = 8;
            this.kbtnAddMapping.Values.Text = "添加映射";
            this.kbtnAddMapping.Click += new System.EventHandler(this.kbtnAddMapping_Click);
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.Location = new System.Drawing.Point(680, 540);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(80, 30);
            this.kbtnCancel.TabIndex = 7;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.kbtnCancel_Click);
            // 
            // kbtnSave
            // 
            this.kbtnSave.Location = new System.Drawing.Point(590, 540);
            this.kbtnSave.Name = "kbtnSave";
            this.kbtnSave.Size = new System.Drawing.Size(80, 30);
            this.kbtnSave.TabIndex = 6;
            this.kbtnSave.Values.Text = "保存";
            this.kbtnSave.Click += new System.EventHandler(this.kbtnSave_Click);
            // 
            // listBoxMappings
            // 
            this.listBoxMappings.Location = new System.Drawing.Point(10, 300);
            this.listBoxMappings.Name = "listBoxMappings";
            this.listBoxMappings.Size = new System.Drawing.Size(770, 220);
            this.listBoxMappings.TabIndex = 5;
            // 
            // listBoxSystemFields
            // 
            this.listBoxSystemFields.Location = new System.Drawing.Point(460, 50);
            this.listBoxSystemFields.Name = "listBoxSystemFields";
            this.listBoxSystemFields.Size = new System.Drawing.Size(320, 180);
            this.listBoxSystemFields.TabIndex = 4;
            // 
            // listBoxExcelColumns
            // 
            this.listBoxExcelColumns.Location = new System.Drawing.Point(10, 50);
            this.listBoxExcelColumns.Name = "listBoxExcelColumns";
            this.listBoxExcelColumns.Size = new System.Drawing.Size(320, 180);
            this.listBoxExcelColumns.TabIndex = 3;
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(10, 280);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(70, 20);
            this.kryptonLabel3.TabIndex = 2;
            this.kryptonLabel3.Values.Text = "映射结果:";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(460, 25);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(70, 20);
            this.kryptonLabel2.TabIndex = 1;
            this.kryptonLabel2.Values.Text = "系统字段:";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(10, 25);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(70, 20);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "Excel列:";
            // 
            // frmColumnMappingConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "frmColumnMappingConfig";
            this.Text = "列映射配置";
            this.Load += new System.EventHandler(this.frmColumnMappingConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonListBox listBoxExcelColumns;
        private Krypton.Toolkit.KryptonListBox listBoxSystemFields;
        private Krypton.Toolkit.KryptonListBox listBoxMappings;
        private Krypton.Toolkit.KryptonButton kbtnSave;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonButton kbtnAddMapping;
        private Krypton.Toolkit.KryptonButton kbtnRemoveMapping;
        private Krypton.Toolkit.KryptonButton kbtnSetUniqueKey;
        private Krypton.Toolkit.KryptonButton kbtnAutoMatch;
    }
}