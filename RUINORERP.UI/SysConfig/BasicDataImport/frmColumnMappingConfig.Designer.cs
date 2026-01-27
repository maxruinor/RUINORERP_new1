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
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnAutoMatch = new Krypton.Toolkit.KryptonButton();
            this.kbtnDeleteMapping = new Krypton.Toolkit.KryptonButton();
            this.kbtnSetColumnProperty = new Krypton.Toolkit.KryptonButton();
            this.kbtnRemoveMapping = new Krypton.Toolkit.KryptonButton();
            this.kbtnAddMapping = new Krypton.Toolkit.KryptonButton();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            this.kbtnSaveMapping = new Krypton.Toolkit.KryptonButton();
            this.listBoxMappings = new Krypton.Toolkit.KryptonListBox();
            this.listBoxSystemFields = new Krypton.Toolkit.KryptonListBox();
            this.listBoxExcelColumns = new Krypton.Toolkit.KryptonListBox();
            this.kryptonLabel5 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxSavedMappings = new Krypton.Toolkit.KryptonComboBox();
            this.textBoxMappingName = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSavedMappings)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kbtnAutoMatch);
            this.kryptonPanel1.Controls.Add(this.kbtnDeleteMapping);
            this.kryptonPanel1.Controls.Add(this.kbtnSetColumnProperty);
            this.kryptonPanel1.Controls.Add(this.kbtnRemoveMapping);
            this.kryptonPanel1.Controls.Add(this.kbtnAddMapping);
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Controls.Add(this.kbtnSaveMapping);
            this.kryptonPanel1.Controls.Add(this.listBoxMappings);
            this.kryptonPanel1.Controls.Add(this.listBoxSystemFields);
            this.kryptonPanel1.Controls.Add(this.listBoxExcelColumns);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel5);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.comboBoxSavedMappings);
            this.kryptonPanel1.Controls.Add(this.textBoxMappingName);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(944, 600);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kbtnAutoMatch
            // 
            this.kbtnAutoMatch.Location = new System.Drawing.Point(471, 246);
            this.kbtnAutoMatch.Name = "kbtnAutoMatch";
            this.kbtnAutoMatch.Size = new System.Drawing.Size(100, 25);
            this.kbtnAutoMatch.TabIndex = 11;
            this.kbtnAutoMatch.Values.Text = "自动匹配";
            this.kbtnAutoMatch.Click += new System.EventHandler(this.kbtnAutoMatch_Click);
            // 
            // kbtnDeleteMapping
            // 
            this.kbtnDeleteMapping.Location = new System.Drawing.Point(96, 576);
            this.kbtnDeleteMapping.Name = "kbtnDeleteMapping";
            this.kbtnDeleteMapping.Size = new System.Drawing.Size(80, 21);
            this.kbtnDeleteMapping.TabIndex = 14;
            this.kbtnDeleteMapping.Values.Text = "删除配置";
            this.kbtnDeleteMapping.Click += new System.EventHandler(this.kbtnDeleteMapping_Click);
            // 
            // kbtnSetColumnProperty
            // 
            this.kbtnSetColumnProperty.Location = new System.Drawing.Point(786, 300);
            this.kbtnSetColumnProperty.Name = "kbtnSetColumnProperty";
            this.kbtnSetColumnProperty.Size = new System.Drawing.Size(100, 25);
            this.kbtnSetColumnProperty.TabIndex = 12;
            this.kbtnSetColumnProperty.Values.Text = "设置列属性";
            this.kbtnSetColumnProperty.Click += new System.EventHandler(this.kbtnSetColumnProperty_Click);
            // 
            // kbtnRemoveMapping
            // 
            this.kbtnRemoveMapping.Location = new System.Drawing.Point(786, 497);
            this.kbtnRemoveMapping.Name = "kbtnRemoveMapping";
            this.kbtnRemoveMapping.Size = new System.Drawing.Size(100, 25);
            this.kbtnRemoveMapping.TabIndex = 9;
            this.kbtnRemoveMapping.Values.Text = "删除映射";
            this.kbtnRemoveMapping.Click += new System.EventHandler(this.kbtnRemoveMapping_Click);
            // 
            // kbtnAddMapping
            // 
            this.kbtnAddMapping.Location = new System.Drawing.Point(691, 246);
            this.kbtnAddMapping.Name = "kbtnAddMapping";
            this.kbtnAddMapping.Size = new System.Drawing.Size(80, 25);
            this.kbtnAddMapping.TabIndex = 8;
            this.kbtnAddMapping.Values.Text = "添加映射";
            this.kbtnAddMapping.Click += new System.EventHandler(this.kbtnAddMapping_Click);
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(700, 539);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(80, 30);
            this.kbtnCancel.TabIndex = 7;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.kbtnCancel_Click);
            // 
            // kbtnSaveMapping
            // 
            this.kbtnSaveMapping.Location = new System.Drawing.Point(561, 540);
            this.kbtnSaveMapping.Name = "kbtnSaveMapping";
            this.kbtnSaveMapping.Size = new System.Drawing.Size(120, 30);
            this.kbtnSaveMapping.TabIndex = 6;
            this.kbtnSaveMapping.Values.Text = "确定并保存";
            this.kbtnSaveMapping.Click += new System.EventHandler(this.kbtnSaveMapping_Click);
            // 
            // listBoxMappings
            // 
            this.listBoxMappings.Location = new System.Drawing.Point(10, 300);
            this.listBoxMappings.Name = "listBoxMappings";
            this.listBoxMappings.Size = new System.Drawing.Size(770, 222);
            this.listBoxMappings.TabIndex = 5;
            this.listBoxMappings.DoubleClick += new System.EventHandler(this.listBoxMappings_DoubleClick);
            // 
            // listBoxSystemFields
            //
            this.listBoxSystemFields.Location = new System.Drawing.Point(460, 50);
            this.listBoxSystemFields.Name = "listBoxSystemFields";
            this.listBoxSystemFields.Size = new System.Drawing.Size(320, 180);
            this.listBoxSystemFields.TabIndex = 4;
            this.listBoxSystemFields.DoubleClick += new System.EventHandler(this.listBoxSystemFields_DoubleClick);
            // 
            // listBoxExcelColumns
            // 
            this.listBoxExcelColumns.Location = new System.Drawing.Point(10, 50);
            this.listBoxExcelColumns.Name = "listBoxExcelColumns";
            this.listBoxExcelColumns.Size = new System.Drawing.Size(320, 180);
            this.listBoxExcelColumns.TabIndex = 3;
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(12, 550);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(78, 20);
            this.kryptonLabel5.TabIndex = 13;
            this.kryptonLabel5.Values.Text = "已保存配置:";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(269, 549);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel4.TabIndex = 12;
            this.kryptonLabel4.Values.Text = "配置名称:";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(10, 280);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel3.TabIndex = 2;
            this.kryptonLabel3.Values.Text = "映射结果:";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(460, 25);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel2.TabIndex = 1;
            this.kryptonLabel2.Values.Text = "系统字段:";
            // 
            // kryptonLabel1
            //
            this.kryptonLabel1.Location = new System.Drawing.Point(10, 25);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(150, 20);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "Excel列 (可选):";
            // 
            // comboBoxSavedMappings
            // 
            this.comboBoxSavedMappings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSavedMappings.DropDownWidth = 130;
            this.comboBoxSavedMappings.FormattingEnabled = true;
            this.comboBoxSavedMappings.IntegralHeight = false;
            this.comboBoxSavedMappings.Location = new System.Drawing.Point(96, 549);
            this.comboBoxSavedMappings.Name = "comboBoxSavedMappings";
            this.comboBoxSavedMappings.Size = new System.Drawing.Size(130, 21);
            this.comboBoxSavedMappings.TabIndex = 15;
            this.comboBoxSavedMappings.SelectedIndexChanged += new System.EventHandler(this.comboBoxSavedMappings_SelectedIndexChanged);
            // 
            // textBoxMappingName
            // 
            this.textBoxMappingName.Location = new System.Drawing.Point(353, 547);
            this.textBoxMappingName.Name = "textBoxMappingName";
            this.textBoxMappingName.Size = new System.Drawing.Size(156, 23);
            this.textBoxMappingName.TabIndex = 14;
            // 
            // frmColumnMappingConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 600);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "frmColumnMappingConfig";
            this.Text = "列映射配置";
            this.Load += new System.EventHandler(this.frmColumnMappingConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxSavedMappings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonListBox listBoxExcelColumns;
        private Krypton.Toolkit.KryptonListBox listBoxSystemFields;
        private Krypton.Toolkit.KryptonListBox listBoxMappings;
        private Krypton.Toolkit.KryptonButton kbtnSaveMapping;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonButton kbtnAddMapping;
        private Krypton.Toolkit.KryptonButton kbtnRemoveMapping;
        private Krypton.Toolkit.KryptonButton kbtnSetColumnProperty;
        private Krypton.Toolkit.KryptonButton kbtnAutoMatch;
        private Krypton.Toolkit.KryptonButton kbtnDeleteMapping;
        private Krypton.Toolkit.KryptonComboBox comboBoxSavedMappings;
        private Krypton.Toolkit.KryptonTextBox textBoxMappingName;
    }
}