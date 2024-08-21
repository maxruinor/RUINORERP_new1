namespace RUINORERP.UI.ProductEAV
{
    partial class frmProductQuery
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chk显示图片 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.dataGridView1 = new HLH.WinControl.MyDataGrid.NewSumDataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.编辑选中产品ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入到ZencartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更新到ZencartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更新到ConnshopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标记为已经更新到connshopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtProductID);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.chk显示图片);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.cmbProductType);
            this.splitContainer1.Panel1.Controls.Add(this.btnQuery);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(886, 714);
            this.splitContainer1.SplitterDistance = 133;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtProductID
            // 
            this.txtProductID.Location = new System.Drawing.Point(80, 38);
            this.txtProductID.Name = "txtProductID";
            this.txtProductID.Size = new System.Drawing.Size(142, 21);
            this.txtProductID.TabIndex = 167;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 166;
            this.label4.Text = "产品ID:";
            // 
            // chk显示图片
            // 
            this.chk显示图片.AutoSize = true;
            this.chk显示图片.Location = new System.Drawing.Point(717, 62);
            this.chk显示图片.Name = "chk显示图片";
            this.chk显示图片.Size = new System.Drawing.Size(72, 16);
            this.chk显示图片.TabIndex = 165;
            this.chk显示图片.Text = "显示图片";
            this.chk显示图片.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 164;
            this.label3.Text = "产品类型:";
            // 
            // cmbProductType
            // 
            this.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductType.FormattingEnabled = true;
            this.cmbProductType.Location = new System.Drawing.Point(80, 12);
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(200, 20);
            this.cmbProductType.TabIndex = 163;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(717, 13);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
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
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.MistyRose;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.SkyBlue;
            this.dataGridView1.IsShowSumRow = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Green;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersWidth = 59;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(886, 577);
            this.dataGridView1.SumColumns = null;
            this.dataGridView1.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView1.SumRowCellFormat = "N2";
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.编辑选中产品ToolStripMenuItem,
            this.导入到ZencartToolStripMenuItem,
            this.更新到ZencartToolStripMenuItem,
            this.更新到ConnshopToolStripMenuItem,
            this.标记为已经更新到connshopToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(230, 114);
            // 
            // 编辑选中产品ToolStripMenuItem
            // 
            this.编辑选中产品ToolStripMenuItem.Name = "编辑选中产品ToolStripMenuItem";
            this.编辑选中产品ToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.编辑选中产品ToolStripMenuItem.Text = "编辑选中产品";
            this.编辑选中产品ToolStripMenuItem.Click += new System.EventHandler(this.编辑选中产品ToolStripMenuItem_Click);
            // 
            // 导入到ZencartToolStripMenuItem
            // 
            this.导入到ZencartToolStripMenuItem.Name = "导入到ZencartToolStripMenuItem";
            this.导入到ZencartToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.导入到ZencartToolStripMenuItem.Text = "新增加Zencart";
            this.导入到ZencartToolStripMenuItem.Click += new System.EventHandler(this.导入到ZencartToolStripMenuItem_Click);
            // 
            // 更新到ZencartToolStripMenuItem
            // 
            this.更新到ZencartToolStripMenuItem.Name = "更新到ZencartToolStripMenuItem";
            this.更新到ZencartToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.更新到ZencartToolStripMenuItem.Text = "更新到Zencart";
            this.更新到ZencartToolStripMenuItem.Click += new System.EventHandler(this.更新到ZencartToolStripMenuItem_Click);
            // 
            // 更新到ConnshopToolStripMenuItem
            // 
            this.更新到ConnshopToolStripMenuItem.Name = "更新到ConnshopToolStripMenuItem";
            this.更新到ConnshopToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.更新到ConnshopToolStripMenuItem.Text = "更新到Connshop";
            this.更新到ConnshopToolStripMenuItem.Click += new System.EventHandler(this.更新到ConnshopToolStripMenuItem_Click);
            // 
            // 标记为已经更新到connshopToolStripMenuItem
            // 
            this.标记为已经更新到connshopToolStripMenuItem.Name = "标记为已经更新到connshopToolStripMenuItem";
            this.标记为已经更新到connshopToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.标记为已经更新到connshopToolStripMenuItem.Text = "标记为已经更新到connshop";
            this.标记为已经更新到connshopToolStripMenuItem.Click += new System.EventHandler(this.标记为已经更新到connshopToolStripMenuItem_Click);
            // 
            // frmProductQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 714);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmProductQuery";
            this.Text = "frmProductQuery";
            this.Load += new System.EventHandler(this.frmProductQuery_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private HLH.WinControl.MyDataGrid.NewSumDataGridView dataGridView1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 编辑选中产品ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入到ZencartToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProductType;
        private System.Windows.Forms.ToolStripMenuItem 更新到ZencartToolStripMenuItem;
        private System.Windows.Forms.CheckBox chk显示图片;
        private System.Windows.Forms.TextBox txtProductID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem 更新到ConnshopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标记为已经更新到connshopToolStripMenuItem;
    }
}