namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    partial class FrmAttributeRulesConfig
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
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.dgvRules = new System.Windows.Forms.DataGridView();
            this.kryptonGroupBox1 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kbtnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kbtnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kbtnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox2 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kbtnAddDefaultRules = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kbtnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kbtnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).BeginInit();
            this.kryptonGroupBox2.SuspendLayout();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kbtnOK);
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(684, 461);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // dgvRules
            // 
            this.dgvRules.AllowUserToAddRows = false;
            this.dgvRules.AllowUserToDeleteRows = false;
            this.dgvRules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRules.Location = new System.Drawing.Point(3, 17);
            this.dgvRules.Name = "dgvRules";
            this.dgvRules.RowHeadersVisible = false;
            this.dgvRules.RowTemplate.Height = 23;
            this.dgvRules.Size = new System.Drawing.Size(678, 180);
            this.dgvRules.TabIndex = 0;
            this.dgvRules.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRules_CellDoubleClick);
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Controls.Add(this.dgvRules);
            this.kryptonGroupBox1.Controls.Add(this.kbtnAdd);
            this.kryptonGroupBox1.Controls.Add(this.kbtnEdit);
            this.kryptonGroupBox1.Controls.Add(this.kbtnDelete);
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            this.kryptonGroupBox1.Size = new System.Drawing.Size(684, 380);
            this.kryptonGroupBox1.TabIndex = 0;
            this.kryptonGroupBox1.Values.Heading = "属性提取规则";
            // 
            // kbtnAdd
            // 
            this.kbtnAdd.Location = new System.Drawing.Point(15, 205);
            this.kbtnAdd.Name = "kbtnAdd";
            this.kbtnAdd.Size = new System.Drawing.Size(75, 23);
            this.kbtnAdd.TabIndex = 1;
            this.kbtnAdd.Values.Text = "添加";
            this.kbtnAdd.Click += new System.EventHandler(this.kbtnAdd_Click);
            // 
            // kbtnEdit
            // 
            this.kbtnEdit.Location = new System.Drawing.Point(96, 205);
            this.kbtnEdit.Name = "kbtnEdit";
            this.kbtnEdit.Size = new System.Drawing.Size(75, 23);
            this.kbtnEdit.TabIndex = 2;
            this.kbtnEdit.Values.Text = "编辑";
            this.kbtnEdit.Click += new System.EventHandler(this.kbtnEdit_Click);
            // 
            // kbtnDelete
            // 
            this.kbtnDelete.Location = new System.Drawing.Point(177, 205);
            this.kbtnDelete.Name = "kbtnDelete";
            this.kbtnDelete.Size = new System.Drawing.Size(75, 23);
            this.kbtnDelete.TabIndex = 3;
            this.kbtnDelete.Values.Text = "删除";
            this.kbtnDelete.Click += new System.EventHandler(this.kbtnDelete_Click);
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.Controls.Add(this.kbtnAddDefaultRules);
            this.kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(0, 380);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            this.kryptonGroupBox2.Size = new System.Drawing.Size(684, 60);
            this.kryptonGroupBox2.TabIndex = 1;
            this.kryptonGroupBox2.Values.Heading = "快速配置";
            // 
            // kbtnAddDefaultRules
            // 
            this.kbtnAddDefaultRules.Location = new System.Drawing.Point(15, 20);
            this.kbtnAddDefaultRules.Name = "kbtnAddDefaultRules";
            this.kbtnAddDefaultRules.Size = new System.Drawing.Size(200, 23);
            this.kbtnAddDefaultRules.TabIndex = 0;
            this.kbtnAddDefaultRules.Values.Text = "添加常用属性规则（颜色、规格）";
            this.kbtnAddDefaultRules.Click += new System.EventHandler(this.kbtnAddDefaultRules_Click);
            // 
            // kbtnOK
            // 
            this.kbtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kbtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.kbtnOK.Location = new System.Drawing.Point(524, 426);
            this.kbtnOK.Name = "kbtnOK";
            this.kbtnOK.Size = new System.Drawing.Size(75, 23);
            this.kbtnOK.TabIndex = 4;
            this.kbtnOK.Values.Text = "确定";
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(609, 426);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(75, 23);
            this.kbtnCancel.TabIndex = 5;
            this.kbtnCancel.Values.Text = "取消";
            // 
            // FrmAttributeRulesConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAttributeRulesConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "属性提取规则配置";
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private System.Windows.Forms.DataGridView dgvRules;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtnAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtnAddDefaultRules;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtnOK;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtnCancel;
    }
}
