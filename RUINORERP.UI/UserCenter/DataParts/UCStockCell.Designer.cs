namespace RUINORERP.UI.UserCenter.DataParts
{
    partial class UCStockCell
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCStockCell));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.kryptonHeaderGroup1 = new Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup1 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonCommandRefresh = new Krypton.Toolkit.KryptonCommand();
            this.kryptonPanelSaleMain = new Krypton.Toolkit.KryptonPanel();
            this.kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonTreeGridViewOtherIn = new Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView();
            this.单据状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonTreeGridViewOtherOut = new Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonTreeGridView1 = new Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView();
            this.订单状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.buttonSpecHeaderGroup2 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelSaleMain)).BeginInit();
            this.kryptonPanelSaleMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridViewOtherIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridViewOtherOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // kryptonHeaderGroup1
            // 
            this.kryptonHeaderGroup1.ButtonSpecs.Add(this.buttonSpecHeaderGroup1);
            this.kryptonHeaderGroup1.ButtonSpecs.Add(this.buttonSpecHeaderGroup2);
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            // 
            // kryptonHeaderGroup1.Panel
            // 
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.kryptonPanelSaleMain);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(715, 317);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "库存情况";
            // 
            // buttonSpecHeaderGroup1
            // 
            this.buttonSpecHeaderGroup1.Enabled = Krypton.Toolkit.ButtonEnabled.True;
            this.buttonSpecHeaderGroup1.KryptonCommand = this.kryptonCommandRefresh;
            this.buttonSpecHeaderGroup1.Text = "2314234";
            this.buttonSpecHeaderGroup1.UniqueName = "2a2f048c9e8b44ddbec5fa6edfc938b8";
            // 
            // kryptonCommandRefresh
            // 
            this.kryptonCommandRefresh.AssignedButtonSpec = this.buttonSpecHeaderGroup1;
            this.kryptonCommandRefresh.Text = "刷新";
            this.kryptonCommandRefresh.Execute += new System.EventHandler(this.kryptonCommand1_Execute);
            // 
            // kryptonPanelSaleMain
            // 
            this.kryptonPanelSaleMain.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanelSaleMain.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanelSaleMain.Controls.Add(this.kryptonTreeGridView1);
            this.kryptonPanelSaleMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelSaleMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelSaleMain.Name = "kryptonPanelSaleMain";
            this.kryptonPanelSaleMain.Size = new System.Drawing.Size(713, 264);
            this.kryptonPanelSaleMain.TabIndex = 0;
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.Location = new System.Drawing.Point(3, 0);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.kryptonTreeGridViewOtherIn);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(371, 189);
            this.kryptonGroupBox2.TabIndex = 7;
            this.kryptonGroupBox2.Values.Heading = "其它入库情况";
            // 
            // kryptonTreeGridViewOtherIn
            // 
            this.kryptonTreeGridViewOtherIn.AllowUserToAddRows = false;
            this.kryptonTreeGridViewOtherIn.AllowUserToDeleteRows = false;
            this.kryptonTreeGridViewOtherIn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.kryptonTreeGridViewOtherIn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.单据状态,
            this.Column1});
            this.kryptonTreeGridViewOtherIn.DataSource = null;
            this.kryptonTreeGridViewOtherIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonTreeGridViewOtherIn.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.kryptonTreeGridViewOtherIn.HideColumns = ((System.Collections.Generic.List<string>)(resources.GetObject("kryptonTreeGridViewOtherIn.HideColumns")));
            this.kryptonTreeGridViewOtherIn.ImageList = null;
            this.kryptonTreeGridViewOtherIn.Location = new System.Drawing.Point(0, 0);
            this.kryptonTreeGridViewOtherIn.Name = "kryptonTreeGridViewOtherIn";
            this.kryptonTreeGridViewOtherIn.ParentIdRootValue = ((long)(0));
            this.kryptonTreeGridViewOtherIn.RowHeadersVisible = false;
            this.kryptonTreeGridViewOtherIn.SelectFilter = "";
            this.kryptonTreeGridViewOtherIn.Size = new System.Drawing.Size(367, 165);
            this.kryptonTreeGridViewOtherIn.SortColumnName = "";
            this.kryptonTreeGridViewOtherIn.TabIndex = 2;
            this.kryptonTreeGridViewOtherIn.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonTreeGridViewOtherIn_CellDoubleClick);
            // 
            // 单据状态
            // 
            this.单据状态.Frozen = true;
            this.单据状态.HeaderText = "单据状态";
            this.单据状态.Name = "单据状态";
            this.单据状态.ReadOnly = true;
            this.单据状态.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.单据状态.Width = 150;
            // 
            // Column1
            // 
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "数量";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 120;
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(380, 3);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonTreeGridViewOtherOut);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(319, 186);
            this.kryptonGroupBox1.TabIndex = 6;
            this.kryptonGroupBox1.Values.Heading = "其它出库情况";
            // 
            // kryptonTreeGridViewOtherOut
            // 
            this.kryptonTreeGridViewOtherOut.AllowUserToAddRows = false;
            this.kryptonTreeGridViewOtherOut.AllowUserToDeleteRows = false;
            this.kryptonTreeGridViewOtherOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.kryptonTreeGridViewOtherOut.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Column2});
            this.kryptonTreeGridViewOtherOut.DataSource = null;
            this.kryptonTreeGridViewOtherOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonTreeGridViewOtherOut.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.kryptonTreeGridViewOtherOut.HideColumns = ((System.Collections.Generic.List<string>)(resources.GetObject("kryptonTreeGridViewOtherOut.HideColumns")));
            this.kryptonTreeGridViewOtherOut.ImageList = null;
            this.kryptonTreeGridViewOtherOut.Location = new System.Drawing.Point(0, 0);
            this.kryptonTreeGridViewOtherOut.Name = "kryptonTreeGridViewOtherOut";
            this.kryptonTreeGridViewOtherOut.ParentIdRootValue = ((long)(0));
            this.kryptonTreeGridViewOtherOut.RowHeadersVisible = false;
            this.kryptonTreeGridViewOtherOut.SelectFilter = "";
            this.kryptonTreeGridViewOtherOut.Size = new System.Drawing.Size(315, 162);
            this.kryptonTreeGridViewOtherOut.SortColumnName = "";
            this.kryptonTreeGridViewOtherOut.TabIndex = 3;
            this.kryptonTreeGridViewOtherOut.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonTreeGridViewOtherOut_CellDoubleClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "单据状态";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // Column2
            // 
            this.Column2.Frozen = true;
            this.Column2.HeaderText = "数量";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 120;
            // 
            // kryptonTreeGridView1
            // 
            this.kryptonTreeGridView1.AllowUserToAddRows = false;
            this.kryptonTreeGridView1.AllowUserToDeleteRows = false;
            this.kryptonTreeGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.kryptonTreeGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.订单状态,
            this.数量});
            this.kryptonTreeGridView1.DataSource = null;
            this.kryptonTreeGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.kryptonTreeGridView1.HideColumns = ((System.Collections.Generic.List<string>)(resources.GetObject("kryptonTreeGridView1.HideColumns")));
            this.kryptonTreeGridView1.ImageList = null;
            this.kryptonTreeGridView1.Location = new System.Drawing.Point(5, 209);
            this.kryptonTreeGridView1.Name = "kryptonTreeGridView1";
            this.kryptonTreeGridView1.ParentIdRootValue = ((long)(0));
            this.kryptonTreeGridView1.SelectFilter = "";
            this.kryptonTreeGridView1.Size = new System.Drawing.Size(266, 162);
            this.kryptonTreeGridView1.SortColumnName = "";
            this.kryptonTreeGridView1.TabIndex = 1;
            this.kryptonTreeGridView1.Visible = false;
            this.kryptonTreeGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonTreeGridView1_CellDoubleClick);
            // 
            // 订单状态
            // 
            this.订单状态.Frozen = true;
            this.订单状态.HeaderText = "订单状态";
            this.订单状态.Name = "订单状态";
            this.订单状态.ReadOnly = true;
            this.订单状态.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.订单状态.Width = 80;
            // 
            // 数量
            // 
            this.数量.Frozen = true;
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.ReadOnly = true;
            this.数量.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // buttonSpecHeaderGroup2
            // 
            this.buttonSpecHeaderGroup2.Checked = Krypton.Toolkit.ButtonCheckState.Unchecked;
            this.buttonSpecHeaderGroup2.Type = Krypton.Toolkit.PaletteButtonSpecStyle.ArrowUp;
            this.buttonSpecHeaderGroup2.UniqueName = "e135944788c145cfa57bb0f617ecb39a";
            // 
            // UCStockCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.kryptonHeaderGroup1);
            this.Name = "UCStockCell";
            this.Size = new System.Drawing.Size(715, 317);
            this.Load += new System.EventHandler(this.UCStockCell_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelSaleMain)).EndInit();
            this.kryptonPanelSaleMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridViewOtherIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridViewOtherOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private Krypton.Toolkit.KryptonPanel kryptonPanelSaleMain;
        private System.Windows.Forms.Timer timer1;
        private Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView kryptonTreeGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单状态;
        private System.Windows.Forms.DataGridViewLinkColumn 数量;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
        private Krypton.Toolkit.KryptonCommand kryptonCommandRefresh;
        private Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView kryptonTreeGridViewOtherOut;
        private Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView kryptonTreeGridViewOtherIn;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据状态;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup2;
    }
}
