namespace RUINORERP.UI.SysConfig
{
    partial class UCRoleAuthorization
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCRoleAuthorization));
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.TreeView1 = new RUINOR.WinFormsUI.TreeViewThreeState.ThreeStateTreeView();
            this.kryptonNavigator1 = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPage1 = new Krypton.Navigator.KryptonPage();
            this.dataGridView1 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonPage3 = new Krypton.Navigator.KryptonPage();
            this.dataGridView2 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.BaseToolStrip = new System.Windows.Forms.ToolStrip();
            this.contextMenuStrip4InitData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemInitBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemInitField = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmRoleInfo = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.aaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bbbToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.cccToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoAll = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).BeginInit();
            this.kryptonNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).BeginInit();
            this.kryptonPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).BeginInit();
            this.kryptonPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.BaseToolStrip.SuspendLayout();
            this.contextMenuStrip4InitData.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
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
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.kryptonNavigator1);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.BaseToolStrip);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(853, 645);
            this.kryptonSplitContainer1.SplitterDistance = 199;
            this.kryptonSplitContainer1.TabIndex = 1;
            // 
            // TreeView1
            // 
            this.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView1.Location = new System.Drawing.Point(0, 0);
            this.TreeView1.Name = "TreeView1";
            this.TreeView1.Size = new System.Drawing.Size(199, 645);
            this.TreeView1.SummaryDescription = "1，添加和级联状态变化到相关的节点，即子节点和或父节点，以及有选择的能力 ";
            this.TreeView1.TabIndex = 0;
            this.TreeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterCheck);
            this.TreeView1.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tVtypeList_DrawNode);
            this.TreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            // 
            // kryptonNavigator1
            // 
            this.kryptonNavigator1.Bar.BarMapExtraText = Krypton.Navigator.MapKryptonPageText.None;
            this.kryptonNavigator1.Bar.BarMapImage = Krypton.Navigator.MapKryptonPageImage.Small;
            this.kryptonNavigator1.Bar.BarMapText = Krypton.Navigator.MapKryptonPageText.TextTitle;
            this.kryptonNavigator1.Bar.BarMultiline = Krypton.Navigator.BarMultiline.Singleline;
            this.kryptonNavigator1.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Top;
            this.kryptonNavigator1.Bar.CheckButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
            this.kryptonNavigator1.Bar.ItemAlignment = Krypton.Toolkit.RelativePositionAlign.Near;
            this.kryptonNavigator1.Bar.ItemMaximumSize = new System.Drawing.Size(200, 200);
            this.kryptonNavigator1.Bar.ItemMinimumSize = new System.Drawing.Size(20, 20);
            this.kryptonNavigator1.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.Auto;
            this.kryptonNavigator1.Bar.ItemSizing = Krypton.Navigator.BarItemSizing.SameHeight;
            this.kryptonNavigator1.Bar.TabBorderStyle = Krypton.Toolkit.TabBorderStyle.OneNote;
            this.kryptonNavigator1.Bar.TabStyle = Krypton.Toolkit.TabStyle.HighProfile;
            this.kryptonNavigator1.ControlKryptonFormFeatures = false;
            this.kryptonNavigator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigator1.Location = new System.Drawing.Point(0, 25);
            this.kryptonNavigator1.Name = "kryptonNavigator1";
            this.kryptonNavigator1.NavigatorMode = Krypton.Navigator.NavigatorMode.BarTabGroup;
            this.kryptonNavigator1.Owner = null;
            this.kryptonNavigator1.PageBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonNavigator1.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPage1,
            this.kryptonPage3});
            this.kryptonNavigator1.SelectedIndex = 0;
            this.kryptonNavigator1.Size = new System.Drawing.Size(649, 620);
            this.kryptonNavigator1.TabIndex = 1;
            this.kryptonNavigator1.Text = "kryptonNavigator1";
            // 
            // kryptonPage1
            // 
            this.kryptonPage1.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage1.Controls.Add(this.dataGridView1);
            this.kryptonPage1.Flags = 65534;
            this.kryptonPage1.LastVisibleSet = true;
            this.kryptonPage1.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage1.Name = "kryptonPage1";
            this.kryptonPage1.Size = new System.Drawing.Size(647, 589);
            this.kryptonPage1.StateNormal.Tab.Border.Color1 = System.Drawing.Color.White;
            this.kryptonPage1.StateNormal.Tab.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPage1.Text = "操作配置";
            this.kryptonPage1.TextDescription = "主要资料";
            this.kryptonPage1.TextTitle = "主要资料";
            this.kryptonPage1.ToolTipTitle = "Page ToolTip";
            this.kryptonPage1.UniqueName = "60445288435B49021FB28348D07C5399";
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
            this.dataGridView1.NeedSaveColumnsXml = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(647, 589);
            this.dataGridView1.SumColumns = null;
            this.dataGridView1.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView1.SumRowCellFormat = "N2";
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.UseCustomColumnDisplay = true;
            this.dataGridView1.UseSelectedColumn = false;
            this.dataGridView1.Use是否使用内置右键功能 = true;
            this.dataGridView1.XmlFileName = "";
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView1_CellFormatting);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // kryptonPage3
            // 
            this.kryptonPage3.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage3.Controls.Add(this.dataGridView2);
            this.kryptonPage3.Flags = 65534;
            this.kryptonPage3.LastVisibleSet = true;
            this.kryptonPage3.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage3.Name = "kryptonPage3";
            this.kryptonPage3.Size = new System.Drawing.Size(647, 589);
            this.kryptonPage3.Text = "字段配置";
            this.kryptonPage3.ToolTipTitle = "Page ToolTip";
            this.kryptonPage3.UniqueName = "4005A0325384478A20BFEBB7440319BD";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.CustomRowNo = false;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.FieldNameList = null;
            this.dataGridView2.IsShowSumRow = false;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.NeedSaveColumnsXml = false;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(647, 589);
            this.dataGridView2.SumColumns = null;
            this.dataGridView2.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView2.SumRowCellFormat = "N2";
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.UseCustomColumnDisplay = true;
            this.dataGridView2.UseSelectedColumn = false;
            this.dataGridView2.Use是否使用内置右键功能 = true;
            this.dataGridView2.XmlFileName = "";
            this.dataGridView2.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView2_CellFormatting);
            this.dataGridView2.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_ColumnHeaderMouseClick);
            this.dataGridView2.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView2_DataError);
            // 
            // BaseToolStrip
            // 
            this.BaseToolStrip.ContextMenuStrip = this.contextMenuStrip4InitData;
            this.BaseToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BaseToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmRoleInfo,
            this.toolStripSeparator1,
            this.toolStripButtonSave,
            this.btnClose,
            this.toolStripSeparator2,
            this.toolStripDropDownButton1,
            this.toolStripSplitButton1});
            this.BaseToolStrip.Location = new System.Drawing.Point(0, 0);
            this.BaseToolStrip.Name = "BaseToolStrip";
            this.BaseToolStrip.Size = new System.Drawing.Size(649, 25);
            this.BaseToolStrip.TabIndex = 56;
            this.BaseToolStrip.Text = "toolStrip1";
            // 
            // contextMenuStrip4InitData
            // 
            this.contextMenuStrip4InitData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip4InitData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemInitBtn,
            this.toolStripMenuItemInitField});
            this.contextMenuStrip4InitData.Name = "contextMenuStrip4InitData";
            this.contextMenuStrip4InitData.Size = new System.Drawing.Size(127, 48);
            // 
            // toolStripMenuItemInitBtn
            // 
            this.toolStripMenuItemInitBtn.Name = "toolStripMenuItemInitBtn";
            this.toolStripMenuItemInitBtn.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItemInitBtn.Text = "添加按钮";
            this.toolStripMenuItemInitBtn.Click += new System.EventHandler(this.toolStripMenuItemInitBtn_Click);
            // 
            // toolStripMenuItemInitField
            // 
            this.toolStripMenuItemInitField.Name = "toolStripMenuItemInitField";
            this.toolStripMenuItemInitField.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItemInitField.Text = "添加字段";
            this.toolStripMenuItemInitField.Click += new System.EventHandler(this.toolStripMenuItemInitField_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Image = global::RUINORERP.UI.Properties.Resources.Work_area;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(49, 22);
            this.toolStripLabel1.Text = "角色";
            // 
            // cmRoleInfo
            // 
            this.cmRoleInfo.Name = "cmRoleInfo";
            this.cmRoleInfo.Size = new System.Drawing.Size(150, 25);
            this.cmRoleInfo.SelectedIndexChanged += new System.EventHandler(this.cmRoleInfo_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::RUINORERP.UI.Properties.Resources.save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonSave.Text = "保存";
            this.toolStripButtonSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::RUINORERP.UI.Properties.Resources.Exit;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(53, 22);
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aaToolStripMenuItem,
            this.bbbToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // aaToolStripMenuItem
            // 
            this.aaToolStripMenuItem.Name = "aaToolStripMenuItem";
            this.aaToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.aaToolStripMenuItem.Text = "aa";
            // 
            // bbbToolStripMenuItem
            // 
            this.bbbToolStripMenuItem.Name = "bbbToolStripMenuItem";
            this.bbbToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.bbbToolStripMenuItem.Text = "bbb";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cccToolStripMenuItem,
            this.dddToolStripMenuItem});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // cccToolStripMenuItem
            // 
            this.cccToolStripMenuItem.Name = "cccToolStripMenuItem";
            this.cccToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.cccToolStripMenuItem.Text = "ccc";
            // 
            // dddToolStripMenuItem
            // 
            this.dddToolStripMenuItem.Name = "dddToolStripMenuItem";
            this.dddToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.dddToolStripMenuItem.Text = "ddd";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAll,
            this.selectNoAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(114, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // selectAll
            // 
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(113, 22);
            this.selectAll.Text = "全选";
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // selectNoAll
            // 
            this.selectNoAll.Name = "selectNoAll";
            this.selectNoAll.Size = new System.Drawing.Size(113, 22);
            this.selectNoAll.Text = "全不选";
            this.selectNoAll.Click += new System.EventHandler(this.selectNoAll_Click);
            // 
            // UCRoleAuthorization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Name = "UCRoleAuthorization";
            this.Size = new System.Drawing.Size(853, 645);
            this.Load += new System.EventHandler(this.UCRoleAuthorization_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            this.kryptonSplitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).EndInit();
            this.kryptonNavigator1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).EndInit();
            this.kryptonPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).EndInit();
            this.kryptonPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.BaseToolStrip.ResumeLayout(false);
            this.BaseToolStrip.PerformLayout();
            this.contextMenuStrip4InitData.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RUINOR.WinFormsUI.TreeViewThreeState.ThreeStateTreeView TreeView1;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private Krypton.Navigator.KryptonNavigator kryptonNavigator1;
        private Krypton.Navigator.KryptonPage kryptonPage1;
        private Krypton.Navigator.KryptonPage kryptonPage3;
        private UControls.NewSumDataGridView dataGridView1;
        private UControls.NewSumDataGridView dataGridView2;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.BindingSource bindingSource2;
        internal System.Windows.Forms.ToolStrip BaseToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmRoleInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAll;
        private System.Windows.Forms.ToolStripMenuItem selectNoAll;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip4InitData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInitBtn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInitField;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem aaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bbbToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cccToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dddToolStripMenuItem;
    }
}
