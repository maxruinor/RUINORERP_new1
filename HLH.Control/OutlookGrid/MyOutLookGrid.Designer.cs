namespace SHControls.OutlookGrid
{
    partial class MyOutLookGrid
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyOutLookGrid));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripCancelGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.展开群组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.收拢群组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消群组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripbtnManagerView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripbtnSort = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAgainSet = new System.Windows.Forms.Button();
            this.btnReMnve = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.btnSortNO = new System.Windows.Forms.Button();
            this.btnSortOK = new System.Windows.Forms.Button();
            this.listBoxPX = new System.Windows.Forms.ListBox();
            this.outlookGrid1 = new OutlookStyleControls.OutlookGrid();
            this.customizeGrid1 = new HLH.WinControl.Mycontrol.CustomizeGrid();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outlookGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCancelGroup,
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.toolStripbtnManagerView,
            this.toolStripSeparator2,
            this.toolStripbtnSort});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(357, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripCancelGroup
            // 
            this.toolStripCancelGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripCancelGroup.Image = ((System.Drawing.Image)(resources.GetObject("toolStripCancelGroup.Image")));
            this.toolStripCancelGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCancelGroup.Name = "toolStripCancelGroup";
            this.toolStripCancelGroup.Size = new System.Drawing.Size(60, 22);
            this.toolStripCancelGroup.Text = "切换皮肤";
            this.toolStripCancelGroup.Click += new System.EventHandler(this.toolStripCancelGroup_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.展开群组ToolStripMenuItem,
            this.收拢群组ToolStripMenuItem,
            this.取消群组ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(69, 22);
            this.toolStripDropDownButton1.Text = "群组状态";
            // 
            // 展开群组ToolStripMenuItem
            // 
            this.展开群组ToolStripMenuItem.Name = "展开群组ToolStripMenuItem";
            this.展开群组ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.展开群组ToolStripMenuItem.Text = "展开群组";
            this.展开群组ToolStripMenuItem.Click += new System.EventHandler(this.展开群组ToolStripMenuItem_Click);
            // 
            // 收拢群组ToolStripMenuItem
            // 
            this.收拢群组ToolStripMenuItem.Name = "收拢群组ToolStripMenuItem";
            this.收拢群组ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.收拢群组ToolStripMenuItem.Text = "收拢群组";
            this.收拢群组ToolStripMenuItem.Click += new System.EventHandler(this.收拢群组ToolStripMenuItem_Click);
            // 
            // 取消群组ToolStripMenuItem
            // 
            this.取消群组ToolStripMenuItem.Name = "取消群组ToolStripMenuItem";
            this.取消群组ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.取消群组ToolStripMenuItem.Text = "取消群组";
            this.取消群组ToolStripMenuItem.Click += new System.EventHandler(this.取消群组ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripbtnManagerView
            // 
            this.toolStripbtnManagerView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripbtnManagerView.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnManagerView.Image")));
            this.toolStripbtnManagerView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnManagerView.Name = "toolStripbtnManagerView";
            this.toolStripbtnManagerView.Size = new System.Drawing.Size(72, 22);
            this.toolStripbtnManagerView.Text = "管理列视图";
            this.toolStripbtnManagerView.Click += new System.EventHandler(this.toolStripbtnManagerView_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripbtnSort
            // 
            this.toolStripbtnSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripbtnSort.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnSort.Image")));
            this.toolStripbtnSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnSort.Name = "toolStripbtnSort";
            this.toolStripbtnSort.Size = new System.Drawing.Size(36, 22);
            this.toolStripbtnSort.Text = "排序";
            this.toolStripbtnSort.Click += new System.EventHandler(this.toolStripbtnSort_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnAgainSet);
            this.panel1.Controls.Add(this.btnReMnve);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnYes);
            this.panel1.Controls.Add(this.listBox2);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Location = new System.Drawing.Point(24, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(312, 227);
            this.panel1.TabIndex = 2;
            this.panel1.Visible = false;
            // 
            // btnAgainSet
            // 
            this.btnAgainSet.Location = new System.Drawing.Point(126, 178);
            this.btnAgainSet.Name = "btnAgainSet";
            this.btnAgainSet.Size = new System.Drawing.Size(55, 23);
            this.btnAgainSet.TabIndex = 7;
            this.btnAgainSet.Text = "重置(&S)";
            this.btnAgainSet.UseVisualStyleBackColor = true;
            this.btnAgainSet.Click += new System.EventHandler(this.btnAgainSet_Click);
            // 
            // btnReMnve
            // 
            this.btnReMnve.Location = new System.Drawing.Point(126, 59);
            this.btnReMnve.Name = "btnReMnve";
            this.btnReMnve.Size = new System.Drawing.Size(55, 23);
            this.btnReMnve.TabIndex = 6;
            this.btnReMnve.Text = "<==(&M)";
            this.btnReMnve.UseVisualStyleBackColor = true;
            this.btnReMnve.Click += new System.EventHandler(this.btnReMnve_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(126, 29);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(55, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "==>(&A)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "移动除的列:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "显示的列:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(166, 203);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(69, 203);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "确定(&O)";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(187, 29);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(123, 172);
            this.listBox2.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 29);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(123, 172);
            this.listBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.radioButton2);
            this.panel2.Controls.Add(this.radioButton1);
            this.panel2.Controls.Add(this.btnSortNO);
            this.panel2.Controls.Add(this.btnSortOK);
            this.panel2.Controls.Add(this.listBoxPX);
            this.panel2.Location = new System.Drawing.Point(94, 44);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(177, 227);
            this.panel2.TabIndex = 8;
            this.panel2.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(101, 9);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.Text = "降序";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(36, 8);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "升序";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // btnSortNO
            // 
            this.btnSortNO.Location = new System.Drawing.Point(97, 200);
            this.btnSortNO.Name = "btnSortNO";
            this.btnSortNO.Size = new System.Drawing.Size(75, 23);
            this.btnSortNO.TabIndex = 2;
            this.btnSortNO.Text = "取消";
            this.btnSortNO.UseVisualStyleBackColor = true;
            this.btnSortNO.Click += new System.EventHandler(this.btnSortNO_Click);
            // 
            // btnSortOK
            // 
            this.btnSortOK.Location = new System.Drawing.Point(16, 200);
            this.btnSortOK.Name = "btnSortOK";
            this.btnSortOK.Size = new System.Drawing.Size(75, 23);
            this.btnSortOK.TabIndex = 1;
            this.btnSortOK.Text = "确定";
            this.btnSortOK.UseVisualStyleBackColor = true;
            this.btnSortOK.Click += new System.EventHandler(this.btnSortOK_Click);
            // 
            // listBoxPX
            // 
            this.listBoxPX.FormattingEnabled = true;
            this.listBoxPX.ItemHeight = 12;
            this.listBoxPX.Location = new System.Drawing.Point(32, 27);
            this.listBoxPX.Name = "listBoxPX";
            this.listBoxPX.Size = new System.Drawing.Size(120, 172);
            this.listBoxPX.TabIndex = 0;
            // 
            // outlookGrid1
            // 
            this.outlookGrid1.AllowUserToAddRows = false;
            this.outlookGrid1.AllowUserToDeleteRows = false;
            this.outlookGrid1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.outlookGrid1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.outlookGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.outlookGrid1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.outlookGrid1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.outlookGrid1.CollapseIcon = ((System.Drawing.Image)(resources.GetObject("outlookGrid1.CollapseIcon")));
            this.outlookGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.outlookGrid1.DefaultCellStyle = dataGridViewCellStyle2;
            this.outlookGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outlookGrid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.outlookGrid1.ExpandIcon = ((System.Drawing.Image)(resources.GetObject("outlookGrid1.ExpandIcon")));
            this.outlookGrid1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outlookGrid1.GridColor = System.Drawing.SystemColors.Control;
            this.outlookGrid1.Location = new System.Drawing.Point(0, 25);
            this.outlookGrid1.Name = "outlookGrid1";
            this.outlookGrid1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.outlookGrid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.outlookGrid1.RowHeadersVisible = false;
            this.outlookGrid1.SaveDataTable = null;
            this.outlookGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.outlookGrid1.Size = new System.Drawing.Size(357, 284);
            this.outlookGrid1.SummaryColumns = null;
            this.outlookGrid1.TabIndex = 0;
            // 
            // customizeGrid1
            // 
            this.customizeGrid1.DisplayText = "Hello & World!";
            this.customizeGrid1.Location = new System.Drawing.Point(279, 0);
            this.customizeGrid1.Name = "customizeGrid1";
            this.customizeGrid1.Size = new System.Drawing.Size(75, 23);
            this.customizeGrid1.TabIndex = 9;
            this.customizeGrid1.targetDataGridView = this.outlookGrid1;
            this.customizeGrid1.UseVisualStyleBackColor = true;
            // 
            // MyOutLookGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customizeGrid1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.outlookGrid1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MyOutLookGrid";
            this.Size = new System.Drawing.Size(357, 309);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outlookGrid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public OutlookStyleControls.OutlookGrid outlookGrid1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripCancelGroup;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 展开群组ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 收拢群组ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消群组ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripbtnManagerView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button btnAgainSet;
        private System.Windows.Forms.Button btnReMnve;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripbtnSort;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox listBoxPX;
        private System.Windows.Forms.Button btnSortNO;
        private System.Windows.Forms.Button btnSortOK;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private HLH.WinControl.Mycontrol.CustomizeGrid customizeGrid1;
    }
}
