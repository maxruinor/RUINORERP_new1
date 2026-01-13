using Krypton.Toolkit;
using RUINORERP.UI.UControls;

namespace RUINORERP.UI.SysConfig
{
    partial class UCMenuEdit
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMenuEdit));
            this.tree_MainMenu = new Krypton.Toolkit.KryptonTreeView();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_modify = new System.Windows.Forms.Button();
            this.btn_del = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCaption_C = new System.Windows.Forms.Label();
            this.lblCaption_E = new System.Windows.Forms.Label();
            this.txt_MenuName = new Krypton.Toolkit.KryptonTextBox();
            this.txt_CaptionE = new Krypton.Toolkit.KryptonTextBox();
            this.txt_CaptonC = new Krypton.Toolkit.KryptonTextBox();
            this.chkisview = new KryptonCheckBox();
            this.chkEnable = new KryptonCheckBox();
            this.txtDiscription = new Krypton.Toolkit.KryptonTextBox();
            this.lblDiscription = new System.Windows.Forms.Label();
            this.lblOID = new System.Windows.Forms.Label();
            this.dataGridView1 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.btnLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.txtClassPath = new Krypton.Toolkit.KryptonTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAssembly = new Krypton.Toolkit.KryptonTextBox();
            this.txtFormName = new Krypton.Toolkit.KryptonTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMenuType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxTreeView1 = new RUINOR.WinFormsUI.ComboBoxTreeView();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainer2 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainer3 = new Krypton.Toolkit.KryptonSplitContainer();
            this.txtBizInterface = new Krypton.Toolkit.KryptonTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSort = new Krypton.Toolkit.KryptonTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtMenuName = new System.Windows.Forms.ToolStripTextBox();
            this.txtBizType = new Krypton.Toolkit.KryptonTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBIBizBaseform = new Krypton.Toolkit.KryptonTextBox();
            this.txtBIBaseForm = new Krypton.Toolkit.KryptonTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkOnlyNew = new System.Windows.Forms.CheckBox();
            this.txtEntityName = new Krypton.Toolkit.KryptonTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.kryptonPanelQuery = new Krypton.Toolkit.KryptonPanel();
            this.groupLine1 = new WinLib.Line.GroupLine();
            this.txtUIPropertyIdentifier = new Krypton.Toolkit.KryptonTextBox();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer3.Panel1)).BeginInit();
            this.kryptonSplitContainer3.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer3.Panel2)).BeginInit();
            this.kryptonSplitContainer3.Panel2.SuspendLayout();
            this.kryptonSplitContainer3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).BeginInit();
            this.kryptonPanelQuery.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree_MainMenu
            // 
            this.tree_MainMenu.AllowDrop = true;
            this.tree_MainMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree_MainMenu.Location = new System.Drawing.Point(0, 0);
            this.tree_MainMenu.Name = "tree_MainMenu";
            this.tree_MainMenu.Size = new System.Drawing.Size(213, 759);
            this.tree_MainMenu.TabIndex = 0;
            this.tree_MainMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_MainMenu_AfterSelect);
            this.tree_MainMenu.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tree_MainMenu_ItemDrag);
            this.tree_MainMenu.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_MainMenu_NodeMouseClick);
            this.tree_MainMenu.DragDrop += new System.Windows.Forms.DragEventHandler(this.tree_MainMenu_DragDropAsync);
            this.tree_MainMenu.DragOver += new System.Windows.Forms.DragEventHandler(this.tree_MainMenu_DragOver);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(219, 17);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_Refresh.TabIndex = 19;
            this.btn_Refresh.Text = "刷新(&R)";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(314, 16);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 20;
            this.btn_add.Text = "添加(&A)";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_save
            // 
            this.btn_save.Enabled = false;
            this.btn_save.Location = new System.Drawing.Point(601, 16);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 22;
            this.btn_save.Text = "保存(&S)";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_modify
            // 
            this.btn_modify.Location = new System.Drawing.Point(409, 17);
            this.btn_modify.Name = "btn_modify";
            this.btn_modify.Size = new System.Drawing.Size(75, 23);
            this.btn_modify.TabIndex = 21;
            this.btn_modify.Text = "修改(&M)";
            this.btn_modify.UseVisualStyleBackColor = true;
            this.btn_modify.Click += new System.EventHandler(this.btn_modify_Click);
            // 
            // btn_del
            // 
            this.btn_del.Location = new System.Drawing.Point(504, 16);
            this.btn_del.Name = "btn_del";
            this.btn_del.Size = new System.Drawing.Size(75, 23);
            this.btn_del.TabIndex = 23;
            this.btn_del.Text = "删除(&D)";
            this.btn_del.UseVisualStyleBackColor = true;
            this.btn_del.Click += new System.EventHandler(this.btn_del_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Enabled = false;
            this.btn_cancel.Location = new System.Drawing.Point(694, 16);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 24;
            this.btn_cancel.Text = "取消(&C)";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_ReinitMenu
            // 
            this.btn_ReinitMenu.Location = new System.Drawing.Point(785, 16);
            this.btn_ReinitMenu.Name = "btn_ReinitMenu";
            this.btn_ReinitMenu.Size = new System.Drawing.Size(100, 23);
            this.btn_ReinitMenu.TabIndex = 25;
            this.btn_ReinitMenu.Text = "重新初始化菜单";
            this.btn_ReinitMenu.UseVisualStyleBackColor = true;
            this.btn_ReinitMenu.Click += new System.EventHandler(this.btn_ReinitMenu_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(26, 81);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(53, 12);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "菜单名称";
            // 
            // lblCaption_C
            // 
            this.lblCaption_C.AutoSize = true;
            this.lblCaption_C.Location = new System.Drawing.Point(-10, 105);
            this.lblCaption_C.Name = "lblCaption_C";
            this.lblCaption_C.Size = new System.Drawing.Size(89, 12);
            this.lblCaption_C.TabIndex = 5;
            this.lblCaption_C.Text = "菜单标题(中文)";
            // 
            // lblCaption_E
            // 
            this.lblCaption_E.AutoSize = true;
            this.lblCaption_E.Location = new System.Drawing.Point(258, 115);
            this.lblCaption_E.Name = "lblCaption_E";
            this.lblCaption_E.Size = new System.Drawing.Size(89, 12);
            this.lblCaption_E.TabIndex = 7;
            this.lblCaption_E.Text = "菜单标题(英文)";
            // 
            // txt_MenuName
            // 
            this.txt_MenuName.Location = new System.Drawing.Point(85, 77);
            this.txt_MenuName.Name = "txt_MenuName";
            this.txt_MenuName.Size = new System.Drawing.Size(167, 21);
            this.txt_MenuName.TabIndex = 4;
            // 
            // txt_CaptionE
            // 
            this.txt_CaptionE.Location = new System.Drawing.Point(355, 106);
            this.txt_CaptionE.Name = "txt_CaptionE";
            this.txt_CaptionE.Size = new System.Drawing.Size(167, 21);
            this.txt_CaptionE.TabIndex = 8;
            // 
            // txt_CaptonC
            // 
            this.txt_CaptonC.Location = new System.Drawing.Point(85, 105);
            this.txt_CaptonC.Name = "txt_CaptonC";
            this.txt_CaptonC.Size = new System.Drawing.Size(167, 21);
            this.txt_CaptonC.TabIndex = 6;
            // 
            // chkisview
            // 
            this.chkisview.AutoSize = true;
            this.chkisview.Checked = true;
            this.chkisview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkisview.Location = new System.Drawing.Point(354, 133);
            this.chkisview.Name = "chkisview";
            this.chkisview.Size = new System.Drawing.Size(72, 16);
            this.chkisview.TabIndex = 12;
            this.chkisview.Text = "是否可见";
            // 
            // chkEnable
            // 
            this.chkEnable.AutoSize = true;
            this.chkEnable.Checked = true;
            this.chkEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnable.Location = new System.Drawing.Point(447, 133);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(72, 16);
            this.chkEnable.TabIndex = 14;
            this.chkEnable.Text = "是否可用";
            // 
            // txtDiscription
            // 
            this.txtDiscription.Location = new System.Drawing.Point(85, 218);
            this.txtDiscription.Name = "txtDiscription";
            this.txtDiscription.Size = new System.Drawing.Size(705, 21);
            this.txtDiscription.TabIndex = 18;
            // 
            // lblDiscription
            // 
            this.lblDiscription.AutoSize = true;
            this.lblDiscription.Location = new System.Drawing.Point(26, 221);
            this.lblDiscription.Name = "lblDiscription";
            this.lblDiscription.Size = new System.Drawing.Size(53, 12);
            this.lblDiscription.TabIndex = 17;
            this.lblDiscription.Text = "菜单描述";
            // 
            // lblOID
            // 
            this.lblOID.AutoSize = true;
            this.lblOID.Location = new System.Drawing.Point(776, 14);
            this.lblOID.Name = "lblOID";
            this.lblOID.Size = new System.Drawing.Size(0, 12);
            this.lblOID.TabIndex = 29;
            this.lblOID.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenucCnfigurator = ((System.Collections.Generic.List<RUINORERP.UI.UControls.ContextMenuController>)(resources.GetObject("dataGridView1.ContextMenucCnfigurator")));
            this.dataGridView1.CustomRowNo = false;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.FieldNameList = null;
            this.dataGridView1.IsShowSumRow = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.NeedSaveColumnsXml = true;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(844, 457);
            this.dataGridView1.SumColumns = null;
            this.dataGridView1.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView1.SumRowCellFormat = "N2";
            this.dataGridView1.TabIndex = 30;
            this.dataGridView1.UseCustomColumnDisplay = true;
            this.dataGridView1.UseSelectedColumn = false;
            this.dataGridView1.Use是否使用内置右键功能 = true;
            this.dataGridView1.XmlFileName = "";
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(476, 6);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(160, 23);
            this.btnLoad.TabIndex = 34;
            this.btnLoad.Text = "重新载入程序集(&L)";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "菜单路径";
            // 
            // txtClassPath
            // 
            this.txtClassPath.Location = new System.Drawing.Point(85, 191);
            this.txtClassPath.Name = "txtClassPath";
            this.txtClassPath.Size = new System.Drawing.Size(705, 21);
            this.txtClassPath.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 37;
            this.label1.Text = "上级菜单节点";
            // 
            // txtAssembly
            // 
            this.txtAssembly.Location = new System.Drawing.Point(102, 35);
            this.txtAssembly.Name = "txtAssembly";
            this.txtAssembly.Size = new System.Drawing.Size(371, 21);
            this.txtAssembly.TabIndex = 40;
            this.txtAssembly.Visible = false;
            this.txtAssembly.TextChanged += new System.EventHandler(this.txtAssembly_TextChanged);
            // 
            // txtFormName
            // 
            this.txtFormName.Location = new System.Drawing.Point(355, 78);
            this.txtFormName.Name = "txtFormName";
            this.txtFormName.Size = new System.Drawing.Size(167, 21);
            this.txtFormName.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "类型名称";
            // 
            // cmbMenuType
            // 
            this.cmbMenuType.FormattingEnabled = true;
            this.cmbMenuType.Items.AddRange(new object[] {
            "行为菜单",
            "导航菜单",
            "直接操作"});
            this.cmbMenuType.Location = new System.Drawing.Point(85, 138);
            this.cmbMenuType.Name = "cmbMenuType";
            this.cmbMenuType.Size = new System.Drawing.Size(165, 20);
            this.cmbMenuType.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 46;
            this.label4.Text = "菜单类型";
            // 
            // comboBoxTreeView1
            // 
            this.comboBoxTreeView1.FormattingEnabled = true;
            this.comboBoxTreeView1.Location = new System.Drawing.Point(102, 8);
            this.comboBoxTreeView1.Name = "comboBoxTreeView1";
            this.comboBoxTreeView1.Size = new System.Drawing.Size(371, 20);
            this.comboBoxTreeView1.TabIndex = 47;
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            this.kryptonSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonSplitContainer2);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btn_Refresh);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btn_add);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btn_modify);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btn_save);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btn_cancel);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btn_del);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(1062, 819);
            this.kryptonSplitContainer1.SplitterDistance = 759;
            this.kryptonSplitContainer1.TabIndex = 48;
            // 
            // kryptonSplitContainer2
            // 
            this.kryptonSplitContainer2.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer2.Name = "kryptonSplitContainer2";
            // 
            // kryptonSplitContainer2.Panel1
            // 
            this.kryptonSplitContainer2.Panel1.Controls.Add(this.tree_MainMenu);
            // 
            // kryptonSplitContainer2.Panel2
            // 
            this.kryptonSplitContainer2.Panel2.Controls.Add(this.kryptonSplitContainer3);
            this.kryptonSplitContainer2.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.kryptonSplitContainer2_Panel2_Paint);
            this.kryptonSplitContainer2.Size = new System.Drawing.Size(1062, 759);
            this.kryptonSplitContainer2.SplitterDistance = 213;
            this.kryptonSplitContainer2.TabIndex = 49;
            // 
            // kryptonSplitContainer3
            // 
            this.kryptonSplitContainer3.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer3.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer3.Name = "kryptonSplitContainer3";
            this.kryptonSplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer3.Panel1
            // 
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtUIPropertyIdentifier);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label11);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtBizInterface);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label10);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtSort);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label8);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.toolStrip1);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtBizType);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label7);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.lblDiscription);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtBIBizBaseform);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtBIBaseForm);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label9);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtDiscription);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label6);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.btnLoad);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.chkOnlyNew);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.lblOID);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label1);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.chkEnable);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.comboBoxTreeView1);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label2);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label4);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.chkisview);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.lblName);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtClassPath);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.cmbMenuType);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txt_CaptonC);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.lblCaption_C);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txt_CaptionE);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtEntityName);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtAssembly);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txtFormName);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.txt_MenuName);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.lblCaption_E);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label3);
            this.kryptonSplitContainer3.Panel1.Controls.Add(this.label5);
            // 
            // kryptonSplitContainer3.Panel2
            // 
            this.kryptonSplitContainer3.Panel2.Controls.Add(this.kryptonPanelQuery);
            this.kryptonSplitContainer3.Size = new System.Drawing.Size(844, 759);
            this.kryptonSplitContainer3.SplitterDistance = 300;
            this.kryptonSplitContainer3.SplitterWidth = 2;
            this.kryptonSplitContainer3.TabIndex = 52;
            // 
            // txtBizInterface
            // 
            this.txtBizInterface.Location = new System.Drawing.Point(354, 164);
            this.txtBizInterface.Name = "txtBizInterface";
            this.txtBizInterface.Size = new System.Drawing.Size(174, 21);
            this.txtBizInterface.TabIndex = 57;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(271, 168);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 56;
            this.label10.Text = "业务接口标识";
            // 
            // txtSort
            // 
            this.txtSort.Location = new System.Drawing.Point(616, 50);
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(174, 21);
            this.txtSort.TabIndex = 55;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(581, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 54;
            this.label8.Text = "排序";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtMenuName});
            this.toolStrip1.Location = new System.Drawing.Point(0, 275);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(844, 25);
            this.toolStrip1.TabIndex = 31;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(46, 22);
            this.toolStripLabel1.Text = "菜单名";
            // 
            // txtMenuName
            // 
            this.txtMenuName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtMenuName.Name = "txtMenuName";
            this.txtMenuName.Size = new System.Drawing.Size(200, 25);
            this.txtMenuName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMenuName_KeyPress);
            // 
            // txtBizType
            // 
            this.txtBizType.Location = new System.Drawing.Point(85, 164);
            this.txtBizType.Name = "txtBizType";
            this.txtBizType.Size = new System.Drawing.Size(167, 21);
            this.txtBizType.TabIndex = 53;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 52;
            this.label7.Text = "业务类型";
            // 
            // txtBIBizBaseform
            // 
            this.txtBIBizBaseform.Location = new System.Drawing.Point(616, 137);
            this.txtBIBizBaseform.Name = "txtBIBizBaseform";
            this.txtBIBizBaseform.Size = new System.Drawing.Size(174, 21);
            this.txtBIBizBaseform.TabIndex = 51;
            // 
            // txtBIBaseForm
            // 
            this.txtBIBaseForm.Location = new System.Drawing.Point(616, 105);
            this.txtBIBaseForm.Name = "txtBIBaseForm";
            this.txtBIBaseForm.Size = new System.Drawing.Size(174, 21);
            this.txtBIBaseForm.TabIndex = 51;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(557, 143);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 50;
            this.label9.Text = "业务基类";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(557, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 50;
            this.label6.Text = "框架基类";
            // 
            // chkOnlyNew
            // 
            this.chkOnlyNew.AutoSize = true;
            this.chkOnlyNew.Location = new System.Drawing.Point(656, 10);
            this.chkOnlyNew.Name = "chkOnlyNew";
            this.chkOnlyNew.Size = new System.Drawing.Size(96, 16);
            this.chkOnlyNew.TabIndex = 49;
            this.chkOnlyNew.Text = "排除已有菜单";
            this.chkOnlyNew.UseVisualStyleBackColor = true;
            this.chkOnlyNew.CheckedChanged += new System.EventHandler(this.chkOnlyNew_CheckedChanged);
            // 
            // txtEntityName
            // 
            this.txtEntityName.Location = new System.Drawing.Point(616, 77);
            this.txtEntityName.Name = "txtEntityName";
            this.txtEntityName.Size = new System.Drawing.Size(174, 21);
            this.txtEntityName.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(557, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 43;
            this.label5.Text = "实体名称";
            // 
            // kryptonPanelQuery
            // 
            this.kryptonPanelQuery.Controls.Add(this.groupLine1);
            this.kryptonPanelQuery.Controls.Add(this.dataGridView1);
            this.kryptonPanelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelQuery.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelQuery.Name = "kryptonPanelQuery";
            this.kryptonPanelQuery.Size = new System.Drawing.Size(844, 457);
            this.kryptonPanelQuery.TabIndex = 31;
            // 
            // groupLine1
            // 
            this.groupLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupLine1.Location = new System.Drawing.Point(0, 456);
            this.groupLine1.Name = "groupLine1";
            this.groupLine1.Size = new System.Drawing.Size(844, 1);
            this.groupLine1.TabIndex = 2;
            // 
            // txtUIPropertyIdentifier
            // 
            this.txtUIPropertyIdentifier.Location = new System.Drawing.Point(616, 164);
            this.txtUIPropertyIdentifier.Name = "txtUIPropertyIdentifier";
            this.txtUIPropertyIdentifier.Size = new System.Drawing.Size(174, 21);
            this.txtUIPropertyIdentifier.TabIndex = 59;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(545, 168);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 58;
            this.label11.Text = "UI属性标识";
            // 
            // UCMenuEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Name = "UCMenuEdit";
            this.Size = new System.Drawing.Size(1062, 819);
            this.Load += new System.EventHandler(this.frmMenuEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2)).EndInit();
            this.kryptonSplitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer3.Panel1)).EndInit();
            this.kryptonSplitContainer3.Panel1.ResumeLayout(false);
            this.kryptonSplitContainer3.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer3.Panel2)).EndInit();
            this.kryptonSplitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer3)).EndInit();
            this.kryptonSplitContainer3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).EndInit();
            this.kryptonPanelQuery.ResumeLayout(false);
            // 
            // contextMenuStripMenu
            // 
            this.contextMenuStripMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAddSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAddSubMenu});
            this.contextMenuStripMenu.Name = "contextMenuStripMenu";
            this.contextMenuStripMenu.Size = new System.Drawing.Size(137, 26);
            // 
            // toolStripMenuItemAddSubMenu
            // 
            this.toolStripMenuItemAddSubMenu.Name = "toolStripMenuItemAddSubMenu";
            this.toolStripMenuItemAddSubMenu.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItemAddSubMenu.Text = "添加子菜单";
            this.toolStripMenuItemAddSubMenu.Click += new System.EventHandler(this.toolStripMenuItemAddSubMenu_Click);
            // 
            // tree_MainMenu
            // 
            this.tree_MainMenu.ContextMenuStrip = this.contextMenuStripMenu;
            // 
            // UCMenuEdit
            // 
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonTreeView tree_MainMenu;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_modify;
        private System.Windows.Forms.Button btn_del;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCaption_C;
        private System.Windows.Forms.Label lblCaption_E;
        private Krypton.Toolkit.KryptonTextBox txt_MenuName;
        private Krypton.Toolkit.KryptonTextBox txt_CaptionE;
        private Krypton.Toolkit.KryptonTextBox txt_CaptonC;
        private KryptonCheckBox chkisview;
        private KryptonCheckBox chkEnable;
        private Krypton.Toolkit.KryptonTextBox txtDiscription;
        private System.Windows.Forms.Label lblDiscription;
        private System.Windows.Forms.Label lblOID;
        private NewSumDataGridView dataGridView1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private Krypton.Toolkit.KryptonTextBox txtClassPath;
        private System.Windows.Forms.Label label1;
        private Krypton.Toolkit.KryptonTextBox txtAssembly;
        private Krypton.Toolkit.KryptonTextBox txtFormName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbMenuType;
        private System.Windows.Forms.Label label4;
        private RUINOR.WinFormsUI.ComboBoxTreeView comboBoxTreeView1;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer2;
        private System.Windows.Forms.CheckBox chkOnlyNew;
        private Krypton.Toolkit.KryptonTextBox txtEntityName;
        private System.Windows.Forms.Label label5;
        private Krypton.Toolkit.KryptonTextBox txtBIBaseForm;
        private System.Windows.Forms.Label label6;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer3;
        private Krypton.Toolkit.KryptonTextBox txtBizType;
        private System.Windows.Forms.Label label7;
        private Krypton.Toolkit.KryptonTextBox txtSort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtMenuName;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelQuery;
        private WinLib.Line.GroupLine groupLine1;
        private Krypton.Toolkit.KryptonTextBox txtBIBizBaseform;
        private System.Windows.Forms.Label label9;
        private Krypton.Toolkit.KryptonTextBox txtBizInterface;
        private System.Windows.Forms.Label label10;
        private Krypton.Toolkit.KryptonTextBox txtUIPropertyIdentifier;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddSubMenu;
        private System.Windows.Forms.Button btn_ReinitMenu;
    }
}