namespace RUINORERP.UI.SysConfig
{
    partial class UCProjectGroupAssigneeEmployees
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.TreeView1 = new Krypton.Toolkit.KryptonTreeView();
            this.kryptonSplitContainer2 = new Krypton.Toolkit.KryptonSplitContainer();
            this.dataGridView1 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.chkReverseSelect = new Krypton.Toolkit.KryptonCheckBox();
            this.chkNotALL = new Krypton.Toolkit.KryptonCheckBox();
            this.chkALL = new Krypton.Toolkit.KryptonCheckBox();
            this.btnSave = new Krypton.Toolkit.KryptonButton();
            this.bindingSourceList = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel1)).BeginInit();
            this.kryptonSplitContainer2.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel2)).BeginInit();
            this.kryptonSplitContainer2.Panel2.SuspendLayout();
            this.kryptonSplitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.TreeView1);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.kryptonSplitContainer2);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(861, 669);
            this.kryptonSplitContainer1.SplitterDistance = 201;
            this.kryptonSplitContainer1.TabIndex = 0;
            // 
            // TreeView1
            // 
            this.TreeView1.CheckBoxes = true;
            this.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView1.Location = new System.Drawing.Point(0, 0);
            this.TreeView1.Name = "TreeView1";
            this.TreeView1.Size = new System.Drawing.Size(201, 669);
            this.TreeView1.TabIndex = 0;
            this.TreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            this.TreeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView1_BeforeSelect);
            // 
            // kryptonSplitContainer2
            // 
            this.kryptonSplitContainer2.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer2.Name = "kryptonSplitContainer2";
            this.kryptonSplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer2.Panel1
            // 
            this.kryptonSplitContainer2.Panel1.Controls.Add(this.dataGridView1);
            // 
            // kryptonSplitContainer2.Panel2
            // 
            this.kryptonSplitContainer2.Panel2.Controls.Add(this.chkReverseSelect);
            this.kryptonSplitContainer2.Panel2.Controls.Add(this.chkNotALL);
            this.kryptonSplitContainer2.Panel2.Controls.Add(this.chkALL);
            this.kryptonSplitContainer2.Panel2.Controls.Add(this.btnSave);
            this.kryptonSplitContainer2.Panel2.StateNormal.Color1 = System.Drawing.SystemColors.ActiveCaption;
            this.kryptonSplitContainer2.Size = new System.Drawing.Size(655, 669);
            this.kryptonSplitContainer2.SplitterDistance = 579;
            this.kryptonSplitContainer2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.CustomRowNo = false;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.FieldNameList = null;
            this.dataGridView1.IsShowSumRow = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.NeedSaveColumnsXml = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(655, 579);
            this.dataGridView1.SumColumns = null;
            this.dataGridView1.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView1.SumRowCellFormat = "N2";
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.UseCustomColumnDisplay = true;
            this.dataGridView1.UseSelectedColumn = false;
            this.dataGridView1.Use是否使用内置右键功能 = true;
            this.dataGridView1.XmlFileName = "";
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView1_CellFormatting);
            // 
            // chkReverseSelect
            // 
            this.chkReverseSelect.Location = new System.Drawing.Point(194, 23);
            this.chkReverseSelect.Name = "chkReverseSelect";
            this.chkReverseSelect.Size = new System.Drawing.Size(49, 20);
            this.chkReverseSelect.TabIndex = 1;
            this.chkReverseSelect.Values.Text = "反选";
            this.chkReverseSelect.CheckedChanged += new System.EventHandler(this.chkReverseSelect_CheckedChanged);
            // 
            // chkNotALL
            // 
            this.chkNotALL.Location = new System.Drawing.Point(123, 23);
            this.chkNotALL.Name = "chkNotALL";
            this.chkNotALL.Size = new System.Drawing.Size(62, 20);
            this.chkNotALL.TabIndex = 1;
            this.chkNotALL.Values.Text = "全不选";
            this.chkNotALL.CheckedChanged += new System.EventHandler(this.chkNotALL_CheckedChanged);
            // 
            // chkALL
            // 
            this.chkALL.Location = new System.Drawing.Point(58, 23);
            this.chkALL.Name = "chkALL";
            this.chkALL.Size = new System.Drawing.Size(49, 20);
            this.chkALL.TabIndex = 1;
            this.chkALL.Values.Text = "全选";
            this.chkALL.CheckedChanged += new System.EventHandler(this.chkALL_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(288, 23);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 35);
            this.btnSave.TabIndex = 0;
            this.btnSave.Values.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // UCProjectGroupEmployeesAssignee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Name = "UCProjectGroupEmployeesAssignee";
            this.Size = new System.Drawing.Size(861, 669);
            this.Load += new System.EventHandler(this.UCUserAuthorization_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel1)).EndInit();
            this.kryptonSplitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel2)).EndInit();
            this.kryptonSplitContainer2.Panel2.ResumeLayout(false);
            this.kryptonSplitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2)).EndInit();
            this.kryptonSplitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private Krypton.Toolkit.KryptonTreeView TreeView1;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer2;
        internal UControls.NewSumDataGridView dataGridView1;
        internal System.Windows.Forms.BindingSource bindingSourceList;
        private Krypton.Toolkit.KryptonCheckBox chkALL;
        private Krypton.Toolkit.KryptonButton btnSave;
        private Krypton.Toolkit.KryptonCheckBox chkReverseSelect;
        private Krypton.Toolkit.KryptonCheckBox chkNotALL;
    }
}
