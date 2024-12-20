using RUINORERP.UI.UControls;
using System.Windows.Forms;

namespace RUINORERP.UI.BaseForm
{
    partial class BaseListGeneric<T>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseList));
            this.kryptonPanelMain = new Krypton.Toolkit.KryptonPanel();
            this.kryptonGroup中间 = new Krypton.Toolkit.KryptonGroup();
            this.dataGridView1 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonHeaderGroupTop = new Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup1 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonPanel条件生成容器 = new Krypton.Toolkit.KryptonPanel();
            this.bindingNavigatorList = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.BaseToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonModify = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtnImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnFunction = new System.Windows.Forms.ToolStripDropDownButton();
            this.复制性新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripbtnProperty = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtMaxRows = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripbtnHelp = new System.Windows.Forms.ToolStripButton();
            this.kryptonContextMenuItem2 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItem1 = new Krypton.Toolkit.KryptonContextMenuItem();
            this.bindingSourceList = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).BeginInit();
            this.kryptonPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间.Panel)).BeginInit();
            this.kryptonGroup中间.Panel.SuspendLayout();
            this.kryptonGroup中间.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).BeginInit();
            this.kryptonHeaderGroupTop.Panel.SuspendLayout();
            this.kryptonHeaderGroupTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel条件生成容器)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorList)).BeginInit();
            this.bindingNavigatorList.SuspendLayout();
            this.BaseToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanelMain
            // 
            this.kryptonPanelMain.Controls.Add(this.kryptonGroup中间);
            this.kryptonPanelMain.Controls.Add(this.kryptonHeaderGroupTop);
            this.kryptonPanelMain.Controls.Add(this.bindingNavigatorList);
            this.kryptonPanelMain.Controls.Add(this.BaseToolStrip);
            this.kryptonPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelMain.Name = "kryptonPanelMain";
            this.kryptonPanelMain.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonPanelMain.Size = new System.Drawing.Size(927, 609);
            this.kryptonPanelMain.TabIndex = 0;
            // 
            // kryptonGroup中间
            // 
            this.kryptonGroup中间.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroup中间.Location = new System.Drawing.Point(0, 110);
            this.kryptonGroup中间.Name = "kryptonGroup中间";
            // 
            // kryptonGroup中间.Panel
            // 
            this.kryptonGroup中间.Panel.Controls.Add(this.dataGridView1);
            this.kryptonGroup中间.Size = new System.Drawing.Size(927, 474);
            this.kryptonGroup中间.TabIndex = 62;
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
            this.dataGridView1.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(925, 472);
            this.dataGridView1.SumColumns = null;
            this.dataGridView1.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView1.SumRowCellFormat = "N2";
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.UseCustomColumnDisplay = true;
            this.dataGridView1.UseSelectedColumn = false;
            this.dataGridView1.Use是否使用内置右键功能 = true;
            this.dataGridView1.XmlFileName = "";
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView1_CellFormatting);
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // kryptonHeaderGroupTop
            // 
            this.kryptonHeaderGroupTop.AutoSize = true;
            this.kryptonHeaderGroupTop.ButtonSpecs.Add(this.buttonSpecHeaderGroup1);
            this.kryptonHeaderGroupTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeaderGroupTop.GroupBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            this.kryptonHeaderGroupTop.HeaderVisibleSecondary = false;
            this.kryptonHeaderGroupTop.Location = new System.Drawing.Point(0, 25);
            this.kryptonHeaderGroupTop.Name = "kryptonHeaderGroupTop";
            // 
            // kryptonHeaderGroupTop.Panel
            // 
            this.kryptonHeaderGroupTop.Panel.Controls.Add(this.kryptonPanel条件生成容器);
            this.kryptonHeaderGroupTop.Size = new System.Drawing.Size(927, 85);
            this.kryptonHeaderGroupTop.TabIndex = 61;
            this.kryptonHeaderGroupTop.ValuesPrimary.Heading = "";
            this.kryptonHeaderGroupTop.ValuesPrimary.Image = global::RUINORERP.UI.Properties.Resources.searcher1;
            this.kryptonHeaderGroupTop.CollapsedChanged += new System.EventHandler(this.kryptonHeaderGroupTop_CollapsedChanged);
            // 
            // buttonSpecHeaderGroup1
            // 
            this.buttonSpecHeaderGroup1.Type = Krypton.Toolkit.PaletteButtonSpecStyle.ArrowDown;
            this.buttonSpecHeaderGroup1.UniqueName = "2f39b39ee6fe403f99a3c0ae9adb6728";
            // 
            // kryptonPanel条件生成容器
            // 
            this.kryptonPanel条件生成容器.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanel条件生成容器.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel条件生成容器.Name = "kryptonPanel条件生成容器";
            this.kryptonPanel条件生成容器.Size = new System.Drawing.Size(925, 59);
            this.kryptonPanel条件生成容器.TabIndex = 0;
            // 
            // bindingNavigatorList
            // 
            this.bindingNavigatorList.AddNewItem = null;
            this.bindingNavigatorList.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigatorList.DeleteItem = null;
            this.bindingNavigatorList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bindingNavigatorList.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bindingNavigatorList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2});
            this.bindingNavigatorList.Location = new System.Drawing.Point(0, 584);
            this.bindingNavigatorList.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigatorList.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigatorList.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigatorList.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigatorList.Name = "bindingNavigatorList";
            this.bindingNavigatorList.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigatorList.Size = new System.Drawing.Size(927, 25);
            this.bindingNavigatorList.TabIndex = 1;
            this.bindingNavigatorList.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(29, 22);
            this.bindingNavigatorCountItem.Text = "/ {0}";
            this.bindingNavigatorCountItem.ToolTipText = "总项数";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
            this.bindingNavigatorMoveFirstItem.Click += new System.EventHandler(this.bindingNavigatorMoveFirstItem_Click);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
            this.bindingNavigatorMovePreviousItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "位置";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "当前位置";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
            this.bindingNavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            this.bindingNavigatorMoveLastItem.Click += new System.EventHandler(this.bindingNavigatorMoveLastItem_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // BaseToolStrip
            // 
            this.BaseToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BaseToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonDelete,
            this.toolStripButtonModify,
            this.toolStripButtonSave,
            this.tsbtnSelected,
            this.toolStripButton4,
            this.toolStripSeparator2,
            this.toolStripButton13,
            this.toolStripSeparator1,
            this.toolStripBtnImport,
            this.toolStripBtnExport,
            this.toolStripbtnFunction,
            this.toolStripbtnProperty,
            this.toolStripButton12,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.txtMaxRows,
            this.toolStripbtnHelp});
            this.BaseToolStrip.Location = new System.Drawing.Point(0, 0);
            this.BaseToolStrip.Name = "BaseToolStrip";
            this.BaseToolStrip.Size = new System.Drawing.Size(927, 25);
            this.BaseToolStrip.TabIndex = 1;
            this.BaseToolStrip.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = global::RUINORERP.UI.Properties.Resources.add;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonAdd.Text = "新增";
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = global::RUINORERP.UI.Properties.Resources.Delete_ok;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonDelete.Text = "删除";
            // 
            // toolStripButtonModify
            // 
            this.toolStripButtonModify.Image = global::RUINORERP.UI.Properties.Resources.Edit_page;
            this.toolStripButtonModify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonModify.Name = "toolStripButtonModify";
            this.toolStripButtonModify.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonModify.Text = "修改";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::RUINORERP.UI.Properties.Resources.save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonSave.Text = "保存";
            // 
            // tsbtnSelected
            // 
            this.tsbtnSelected.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSelected.Image")));
            this.tsbtnSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSelected.Name = "tsbtnSelected";
            this.tsbtnSelected.Size = new System.Drawing.Size(53, 22);
            this.tsbtnSelected.Text = "选中";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton4.Text = "查询";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton13
            // 
            this.toolStripButton13.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton13.Image")));
            this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton13.Name = "toolStripButton13";
            this.toolStripButton13.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton13.Text = "打印";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripBtnImport
            // 
            this.toolStripBtnImport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnImport.Image")));
            this.toolStripBtnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnImport.Name = "toolStripBtnImport";
            this.toolStripBtnImport.Size = new System.Drawing.Size(53, 22);
            this.toolStripBtnImport.Text = "导入";
            // 
            // toolStripBtnExport
            // 
            this.toolStripBtnExport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnExport.Image")));
            this.toolStripBtnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnExport.Name = "toolStripBtnExport";
            this.toolStripBtnExport.Size = new System.Drawing.Size(53, 22);
            this.toolStripBtnExport.Text = "导出";
            // 
            // toolStripbtnFunction
            // 
            this.toolStripbtnFunction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制性新增ToolStripMenuItem});
            this.toolStripbtnFunction.Image = global::RUINORERP.UI.Properties.Resources.objectItem;
            this.toolStripbtnFunction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnFunction.Name = "toolStripbtnFunction";
            this.toolStripbtnFunction.Size = new System.Drawing.Size(62, 22);
            this.toolStripbtnFunction.Text = "功能";
            // 
            // 复制性新增ToolStripMenuItem
            // 
            this.复制性新增ToolStripMenuItem.Image = global::RUINORERP.UI.Properties.Resources.add;
            this.复制性新增ToolStripMenuItem.Name = "复制性新增ToolStripMenuItem";
            this.复制性新增ToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.复制性新增ToolStripMenuItem.Text = "复制性新增";
            // 
            // toolStripbtnProperty
            // 
            this.toolStripbtnProperty.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnProperty.Image")));
            this.toolStripbtnProperty.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnProperty.Name = "toolStripbtnProperty";
            this.toolStripbtnProperty.Size = new System.Drawing.Size(53, 22);
            this.toolStripbtnProperty.Text = "属性";
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton12.Image")));
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton12.Text = "关闭";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(59, 22);
            this.toolStripLabel1.Text = "最大行数";
            // 
            // txtMaxRows
            // 
            this.txtMaxRows.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtMaxRows.Name = "txtMaxRows";
            this.txtMaxRows.Size = new System.Drawing.Size(100, 25);
            this.txtMaxRows.Text = "200";
            // 
            // toolStripbtnHelp
            // 
            this.toolStripbtnHelp.Image = global::RUINORERP.UI.Properties.Resources.Help0;
            this.toolStripbtnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnHelp.Name = "toolStripbtnHelp";
            this.toolStripbtnHelp.Size = new System.Drawing.Size(53, 20);
            this.toolStripbtnHelp.Text = "帮助";
            // 
            // kryptonContextMenuItem2
            // 
            this.kryptonContextMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("kryptonContextMenuItem2.Image")));
            this.kryptonContextMenuItem2.Text = "E&xit";
            // 
            // kryptonContextMenuItem1
            // 
            this.kryptonContextMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("kryptonContextMenuItem1.Image")));
            this.kryptonContextMenuItem1.Text = "E&xit";
            // 
            // BaseListGeneric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanelMain);
            this.Name = "BaseListGeneric";
            this.Size = new System.Drawing.Size(927, 609);
            this.Load += new System.EventHandler(this.BaseList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).EndInit();
            this.kryptonPanelMain.ResumeLayout(false);
            this.kryptonPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间.Panel)).EndInit();
            this.kryptonGroup中间.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间)).EndInit();
            this.kryptonGroup中间.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).EndInit();
            this.kryptonHeaderGroupTop.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).EndInit();
            this.kryptonHeaderGroupTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel条件生成容器)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorList)).EndInit();
            this.bindingNavigatorList.ResumeLayout(false);
            this.bindingNavigatorList.PerformLayout();
            this.BaseToolStrip.ResumeLayout(false);
            this.BaseToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanelMain;
        internal System.Windows.Forms.ToolStripButton toolStripButtonModify;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        internal System.Windows.Forms.ToolStrip BaseToolStrip;

        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        internal System.Windows.Forms.BindingNavigator bindingNavigatorList;
        internal System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        internal ToolStripButton tsbtnSelected;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripBtnExport;
        private ToolStripButton toolStripBtnImport;
        internal BindingSource bindingSourceList;
        private ToolStripDropDownButton toolStripbtnFunction;
        private ToolStripMenuItem 复制性新增ToolStripMenuItem;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem1;
        private Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem2;
        internal Krypton.Toolkit.KryptonPanel kryptonPanel条件生成容器;
        internal NewSumDataGridView dataGridView1;
        private Krypton.Toolkit.KryptonGroup kryptonGroup中间;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
        public Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroupTop;
        public ToolStripButton toolStripbtnProperty;
        public ToolStripButton toolStripButtonDelete;
        public ToolStripButton toolStripButtonAdd;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripLabel toolStripLabel1;
        public ToolStripTextBox txtMaxRows;
        private ToolStripButton toolStripbtnHelp;
    }
}
