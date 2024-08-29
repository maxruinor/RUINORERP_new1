using System;
using System.Windows.Forms;

namespace RUINORERP.UI.ProductEAV
{
    partial class UCProdQuery
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
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer1 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCProdQuery));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainer2 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainerQuery = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonPanelProd = new Krypton.Toolkit.KryptonPanel();
            this.chksku_available = new Krypton.Toolkit.KryptonCheckBox();
            this.chksku_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel11 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel12 = new Krypton.Toolkit.KryptonLabel();
            this.chkProd_available = new Krypton.Toolkit.KryptonCheckBox();
            this.chkProd_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblis_available = new Krypton.Toolkit.KryptonLabel();
            this.lblis_enabled = new Krypton.Toolkit.KryptonLabel();
            this.lblLastInventoryDate = new Krypton.Toolkit.KryptonLabel();
            this.dtp2 = new Krypton.Toolkit.KryptonDateTimePicker();
            this.dtp1 = new Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonLabel10 = new Krypton.Toolkit.KryptonLabel();
            this.cmbStockJudgement = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel9 = new Krypton.Toolkit.KryptonLabel();
            this.txtName = new Krypton.Toolkit.KryptonTextBox();
            this.cmbLocation = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel7 = new Krypton.Toolkit.KryptonLabel();
            this.chkIncludingChild = new Krypton.Toolkit.KryptonCheckBox();
            this.lblcategory_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbdepartment = new Krypton.Toolkit.KryptonComboBox();
            this.txtShortCode = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel6 = new Krypton.Toolkit.KryptonLabel();
            this.lblShortCode = new Krypton.Toolkit.KryptonLabel();
            this.lblName = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel5 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            this.txtcategory_ID = new RUINOR.WinFormsUI.ComboBoxTreeView();
            this.txtSKU码 = new Krypton.Toolkit.KryptonTextBox();
            this.txtProp = new Krypton.Toolkit.KryptonTextBox();
            this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
            this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
            this.txtType_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
            this.txtModel = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.txtBarCode = new Krypton.Toolkit.KryptonTextBox();
            this.lblNo = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.txtNo = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonPanelGroup = new Krypton.Toolkit.KryptonPanel();
            this.chkProdBundle_available = new Krypton.Toolkit.KryptonCheckBox();
            this.chkProdBundle_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel14 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel15 = new Krypton.Toolkit.KryptonLabel();
            this.txt组合名称 = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel8 = new Krypton.Toolkit.KryptonLabel();
            this.btnQueryForGoods = new Krypton.Toolkit.KryptonButton();
            this.chk包含父级节点 = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.chkMultiSelect = new Krypton.Toolkit.KryptonCheckBox();
            this.txtMaxRows = new Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonNavigator1 = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPage产品 = new Krypton.Navigator.KryptonPage();
            this.newSumDataGridView产品 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonBOM = new Krypton.Navigator.KryptonPage();
            this.treeListView1 = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.kryptonPage产品组合 = new Krypton.Navigator.KryptonPage();
            this.newSumDataGridView产品组合 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.bindingSourceProdDetail = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceBOM = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceGroup = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.箱规ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.按SKU添加箱规ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerQuery.Panel1)).BeginInit();
            this.kryptonSplitContainerQuery.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerQuery.Panel2)).BeginInit();
            this.kryptonSplitContainerQuery.Panel2.SuspendLayout();
            this.kryptonSplitContainerQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelProd)).BeginInit();
            this.kryptonPanelProd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStockJudgement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbdepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtType_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelGroup)).BeginInit();
            this.kryptonPanelGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).BeginInit();
            this.kryptonNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage产品)).BeginInit();
            this.kryptonPage产品.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridView产品)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonBOM)).BeginInit();
            this.kryptonBOM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage产品组合)).BeginInit();
            this.kryptonPage产品组合.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridView产品组合)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroup)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btnOk);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(1118, 682);
            this.kryptonSplitContainer1.SplitterDistance = 620;
            this.kryptonSplitContainer1.TabIndex = 0;
            // 
            // kryptonSplitContainer2
            // 
            this.kryptonSplitContainer2.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.kryptonSplitContainer2.IsSplitterFixed = true;
            this.kryptonSplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer2.Name = "kryptonSplitContainer2";
            this.kryptonSplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer2.Panel1
            // 
            this.kryptonSplitContainer2.Panel1.Controls.Add(this.kryptonSplitContainerQuery);
            // 
            // kryptonSplitContainer2.Panel2
            // 
            this.kryptonSplitContainer2.Panel2.Controls.Add(this.kryptonNavigator1);
            this.kryptonSplitContainer2.Size = new System.Drawing.Size(1118, 620);
            this.kryptonSplitContainer2.SplitterDistance = 167;
            this.kryptonSplitContainer2.TabIndex = 0;
            // 
            // kryptonSplitContainerQuery
            // 
            this.kryptonSplitContainerQuery.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainerQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainerQuery.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainerQuery.Name = "kryptonSplitContainerQuery";
            // 
            // kryptonSplitContainerQuery.Panel1
            // 
            this.kryptonSplitContainerQuery.Panel1.Controls.Add(this.kryptonPanelProd);
            this.kryptonSplitContainerQuery.Panel1.Controls.Add(this.kryptonPanelGroup);
            // 
            // kryptonSplitContainerQuery.Panel2
            // 
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.btnQueryForGoods);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.chk包含父级节点);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.kryptonLabel2);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.chkMultiSelect);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.txtMaxRows);
            this.kryptonSplitContainerQuery.Size = new System.Drawing.Size(1118, 167);
            this.kryptonSplitContainerQuery.SplitterDistance = 893;
            this.kryptonSplitContainerQuery.TabIndex = 161;
            // 
            // kryptonPanelProd
            // 
            this.kryptonPanelProd.AutoSize = true;
            this.kryptonPanelProd.Controls.Add(this.chksku_available);
            this.kryptonPanelProd.Controls.Add(this.chksku_enabled);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel11);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel12);
            this.kryptonPanelProd.Controls.Add(this.chkProd_available);
            this.kryptonPanelProd.Controls.Add(this.chkProd_enabled);
            this.kryptonPanelProd.Controls.Add(this.lblis_available);
            this.kryptonPanelProd.Controls.Add(this.lblis_enabled);
            this.kryptonPanelProd.Controls.Add(this.lblLastInventoryDate);
            this.kryptonPanelProd.Controls.Add(this.dtp2);
            this.kryptonPanelProd.Controls.Add(this.dtp1);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel10);
            this.kryptonPanelProd.Controls.Add(this.cmbStockJudgement);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel9);
            this.kryptonPanelProd.Controls.Add(this.txtName);
            this.kryptonPanelProd.Controls.Add(this.cmbLocation);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel7);
            this.kryptonPanelProd.Controls.Add(this.chkIncludingChild);
            this.kryptonPanelProd.Controls.Add(this.lblcategory_ID);
            this.kryptonPanelProd.Controls.Add(this.cmbdepartment);
            this.kryptonPanelProd.Controls.Add(this.txtShortCode);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel6);
            this.kryptonPanelProd.Controls.Add(this.lblShortCode);
            this.kryptonPanelProd.Controls.Add(this.lblName);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel5);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel4);
            this.kryptonPanelProd.Controls.Add(this.txtcategory_ID);
            this.kryptonPanelProd.Controls.Add(this.txtSKU码);
            this.kryptonPanelProd.Controls.Add(this.txtProp);
            this.kryptonPanelProd.Controls.Add(this.txtSpecifications);
            this.kryptonPanelProd.Controls.Add(this.lblSpecifications);
            this.kryptonPanelProd.Controls.Add(this.txtType_ID);
            this.kryptonPanelProd.Controls.Add(this.lblType_ID);
            this.kryptonPanelProd.Controls.Add(this.txtModel);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel3);
            this.kryptonPanelProd.Controls.Add(this.txtBarCode);
            this.kryptonPanelProd.Controls.Add(this.lblNo);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel1);
            this.kryptonPanelProd.Controls.Add(this.txtNo);
            this.kryptonPanelProd.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanelProd.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelProd.Name = "kryptonPanelProd";
            this.kryptonPanelProd.Size = new System.Drawing.Size(893, 133);
            this.kryptonPanelProd.TabIndex = 160;
            // 
            // chksku_available
            // 
            this.chksku_available.Location = new System.Drawing.Point(821, 74);
            this.chksku_available.Name = "chksku_available";
            this.chksku_available.Size = new System.Drawing.Size(19, 13);
            this.chksku_available.TabIndex = 172;
            this.chksku_available.Values.Text = "";
            // 
            // chksku_enabled
            // 
            this.chksku_enabled.Location = new System.Drawing.Point(821, 52);
            this.chksku_enabled.Name = "chksku_enabled";
            this.chksku_enabled.Size = new System.Drawing.Size(19, 13);
            this.chksku_enabled.TabIndex = 173;
            this.chksku_enabled.Values.Text = "";
            // 
            // kryptonLabel11
            // 
            this.kryptonLabel11.Location = new System.Drawing.Point(757, 71);
            this.kryptonLabel11.Name = "kryptonLabel11";
            this.kryptonLabel11.Size = new System.Drawing.Size(59, 20);
            this.kryptonLabel11.TabIndex = 171;
            this.kryptonLabel11.Values.Text = "SKU可用";
            // 
            // kryptonLabel12
            // 
            this.kryptonLabel12.Location = new System.Drawing.Point(757, 50);
            this.kryptonLabel12.Name = "kryptonLabel12";
            this.kryptonLabel12.Size = new System.Drawing.Size(59, 20);
            this.kryptonLabel12.TabIndex = 170;
            this.kryptonLabel12.Values.Text = "SKU启用";
            // 
            // chkProd_available
            // 
            this.chkProd_available.Location = new System.Drawing.Point(821, 33);
            this.chkProd_available.Name = "chkProd_available";
            this.chkProd_available.Size = new System.Drawing.Size(19, 13);
            this.chkProd_available.TabIndex = 168;
            this.chkProd_available.Values.Text = "";
            // 
            // chkProd_enabled
            // 
            this.chkProd_enabled.Location = new System.Drawing.Point(821, 9);
            this.chkProd_enabled.Name = "chkProd_enabled";
            this.chkProd_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkProd_enabled.TabIndex = 169;
            this.chkProd_enabled.Values.Text = "";
            // 
            // lblis_available
            // 
            this.lblis_available.Location = new System.Drawing.Point(754, 30);
            this.lblis_available.Name = "lblis_available";
            this.lblis_available.Size = new System.Drawing.Size(62, 20);
            this.lblis_available.TabIndex = 167;
            this.lblis_available.Values.Text = "产品可用";
            // 
            // lblis_enabled
            // 
            this.lblis_enabled.Location = new System.Drawing.Point(754, 7);
            this.lblis_enabled.Name = "lblis_enabled";
            this.lblis_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblis_enabled.TabIndex = 166;
            this.lblis_enabled.Values.Text = "产品启用";
            // 
            // lblLastInventoryDate
            // 
            this.lblLastInventoryDate.Location = new System.Drawing.Point(497, 103);
            this.lblLastInventoryDate.Name = "lblLastInventoryDate";
            this.lblLastInventoryDate.Size = new System.Drawing.Size(88, 20);
            this.lblLastInventoryDate.TabIndex = 165;
            this.lblLastInventoryDate.Values.Text = "最近盘点时间";
            // 
            // dtp2
            // 
            this.dtp2.Checked = false;
            this.dtp2.Location = new System.Drawing.Point(725, 102);
            this.dtp2.Name = "dtp2";
            this.dtp2.ShowCheckBox = true;
            this.dtp2.Size = new System.Drawing.Size(125, 21);
            this.dtp2.TabIndex = 163;
            // 
            // dtp1
            // 
            this.dtp1.Checked = false;
            this.dtp1.Location = new System.Drawing.Point(591, 102);
            this.dtp1.Name = "dtp1";
            this.dtp1.ShowCheckBox = true;
            this.dtp1.Size = new System.Drawing.Size(125, 21);
            this.dtp1.TabIndex = 162;
            // 
            // kryptonLabel10
            // 
            this.kryptonLabel10.Location = new System.Drawing.Point(711, 102);
            this.kryptonLabel10.Name = "kryptonLabel10";
            this.kryptonLabel10.Size = new System.Drawing.Size(19, 20);
            this.kryptonLabel10.TabIndex = 164;
            this.kryptonLabel10.Values.Text = "~";
            // 
            // cmbStockJudgement
            // 
            this.cmbStockJudgement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStockJudgement.DropDownWidth = 205;
            this.cmbStockJudgement.IntegralHeight = false;
            this.cmbStockJudgement.Items.AddRange(new object[] {
            "请选择",
            "大于零",
            "等于零",
            "小于零"});
            this.cmbStockJudgement.Location = new System.Drawing.Point(591, 6);
            this.cmbStockJudgement.Name = "cmbStockJudgement";
            this.cmbStockJudgement.Size = new System.Drawing.Size(126, 21);
            this.cmbStockJudgement.TabIndex = 161;
            // 
            // kryptonLabel9
            // 
            this.kryptonLabel9.Location = new System.Drawing.Point(550, 7);
            this.kryptonLabel9.Name = "kryptonLabel9";
            this.kryptonLabel9.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel9.TabIndex = 160;
            this.kryptonLabel9.Values.Text = "库存";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(318, 2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(163, 23);
            this.txtName.TabIndex = 135;
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownWidth = 205;
            this.cmbLocation.IntegralHeight = false;
            this.cmbLocation.Location = new System.Drawing.Point(591, 40);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(126, 21);
            this.cmbLocation.TabIndex = 159;
            // 
            // kryptonLabel7
            // 
            this.kryptonLabel7.Location = new System.Drawing.Point(550, 39);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel7.TabIndex = 158;
            this.kryptonLabel7.Values.Text = "库位";
            // 
            // chkIncludingChild
            // 
            this.chkIncludingChild.Location = new System.Drawing.Point(208, 110);
            this.chkIncludingChild.Name = "chkIncludingChild";
            this.chkIncludingChild.Size = new System.Drawing.Size(62, 20);
            this.chkIncludingChild.TabIndex = 148;
            this.chkIncludingChild.Values.Text = "含子类";
            // 
            // lblcategory_ID
            // 
            this.lblcategory_ID.Location = new System.Drawing.Point(17, 109);
            this.lblcategory_ID.Name = "lblcategory_ID";
            this.lblcategory_ID.Size = new System.Drawing.Size(36, 20);
            this.lblcategory_ID.TabIndex = 139;
            this.lblcategory_ID.Values.Text = "类别";
            // 
            // cmbdepartment
            // 
            this.cmbdepartment.DropDownWidth = 205;
            this.cmbdepartment.IntegralHeight = false;
            this.cmbdepartment.Location = new System.Drawing.Point(591, 69);
            this.cmbdepartment.Name = "cmbdepartment";
            this.cmbdepartment.Size = new System.Drawing.Size(126, 21);
            this.cmbdepartment.TabIndex = 157;
            // 
            // txtShortCode
            // 
            this.txtShortCode.Location = new System.Drawing.Point(55, 55);
            this.txtShortCode.Name = "txtShortCode";
            this.txtShortCode.Size = new System.Drawing.Size(151, 23);
            this.txtShortCode.TabIndex = 137;
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Location = new System.Drawing.Point(550, 71);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel6.TabIndex = 156;
            this.kryptonLabel6.Values.Text = "部门";
            // 
            // lblShortCode
            // 
            this.lblShortCode.Location = new System.Drawing.Point(4, 59);
            this.lblShortCode.Name = "lblShortCode";
            this.lblShortCode.Size = new System.Drawing.Size(49, 20);
            this.lblShortCode.TabIndex = 136;
            this.lblShortCode.Values.Text = "助记码";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(277, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(36, 20);
            this.lblName.TabIndex = 134;
            this.lblName.Values.Text = "品名";
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(17, 86);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel5.TabIndex = 154;
            this.kryptonLabel5.Values.Text = "属性";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(267, 108);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(46, 20);
            this.kryptonLabel4.TabIndex = 151;
            this.kryptonLabel4.Values.Text = "SKU码";
            // 
            // txtcategory_ID
            // 
            this.txtcategory_ID.FormattingEnabled = true;
            this.txtcategory_ID.Location = new System.Drawing.Point(56, 109);
            this.txtcategory_ID.Name = "txtcategory_ID";
            this.txtcategory_ID.Size = new System.Drawing.Size(150, 20);
            this.txtcategory_ID.TabIndex = 140;
            // 
            // txtSKU码
            // 
            this.txtSKU码.Location = new System.Drawing.Point(319, 107);
            this.txtSKU码.Name = "txtSKU码";
            this.txtSKU码.Size = new System.Drawing.Size(162, 23);
            this.txtSKU码.TabIndex = 152;
            // 
            // txtProp
            // 
            this.txtProp.Location = new System.Drawing.Point(55, 82);
            this.txtProp.Name = "txtProp";
            this.txtProp.Size = new System.Drawing.Size(151, 23);
            this.txtProp.TabIndex = 155;
            // 
            // txtSpecifications
            // 
            this.txtSpecifications.Location = new System.Drawing.Point(55, 29);
            this.txtSpecifications.Name = "txtSpecifications";
            this.txtSpecifications.Size = new System.Drawing.Size(151, 23);
            this.txtSpecifications.TabIndex = 144;
            // 
            // lblSpecifications
            // 
            this.lblSpecifications.Location = new System.Drawing.Point(17, 29);
            this.lblSpecifications.Name = "lblSpecifications";
            this.lblSpecifications.Size = new System.Drawing.Size(36, 20);
            this.lblSpecifications.TabIndex = 143;
            this.lblSpecifications.Values.Text = "规格";
            // 
            // txtType_ID
            // 
            this.txtType_ID.DropDownWidth = 205;
            this.txtType_ID.IntegralHeight = false;
            this.txtType_ID.Location = new System.Drawing.Point(319, 80);
            this.txtType_ID.Name = "txtType_ID";
            this.txtType_ID.Size = new System.Drawing.Size(162, 21);
            this.txtType_ID.TabIndex = 142;
            // 
            // lblType_ID
            // 
            this.lblType_ID.Location = new System.Drawing.Point(251, 81);
            this.lblType_ID.Name = "lblType_ID";
            this.lblType_ID.Size = new System.Drawing.Size(62, 20);
            this.lblType_ID.TabIndex = 141;
            this.lblType_ID.Values.Text = "产品类型";
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(318, 28);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(163, 23);
            this.txtModel.TabIndex = 133;
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(277, 27);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel3.TabIndex = 132;
            this.kryptonLabel3.Values.Text = "型号";
            // 
            // txtBarCode
            // 
            this.txtBarCode.Location = new System.Drawing.Point(319, 55);
            this.txtBarCode.Name = "txtBarCode";
            this.txtBarCode.Size = new System.Drawing.Size(162, 23);
            this.txtBarCode.TabIndex = 138;
            // 
            // lblNo
            // 
            this.lblNo.Location = new System.Drawing.Point(13, 5);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(36, 20);
            this.lblNo.TabIndex = 132;
            this.lblNo.Values.Text = "品号";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(277, 56);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel1.TabIndex = 136;
            this.kryptonLabel1.Values.Text = "条码";
            // 
            // txtNo
            // 
            this.txtNo.Location = new System.Drawing.Point(56, 4);
            this.txtNo.Name = "txtNo";
            this.txtNo.Size = new System.Drawing.Size(150, 23);
            this.txtNo.TabIndex = 133;
            // 
            // kryptonPanelGroup
            // 
            this.kryptonPanelGroup.AutoSize = true;
            this.kryptonPanelGroup.Controls.Add(this.chkProdBundle_available);
            this.kryptonPanelGroup.Controls.Add(this.chkProdBundle_enabled);
            this.kryptonPanelGroup.Controls.Add(this.kryptonLabel14);
            this.kryptonPanelGroup.Controls.Add(this.kryptonLabel15);
            this.kryptonPanelGroup.Controls.Add(this.txt组合名称);
            this.kryptonPanelGroup.Controls.Add(this.kryptonLabel8);
            this.kryptonPanelGroup.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanelGroup.Location = new System.Drawing.Point(0, 136);
            this.kryptonPanelGroup.Name = "kryptonPanelGroup";
            this.kryptonPanelGroup.Size = new System.Drawing.Size(893, 31);
            this.kryptonPanelGroup.TabIndex = 161;
            // 
            // chkProdBundle_available
            // 
            this.chkProdBundle_available.Location = new System.Drawing.Point(866, 8);
            this.chkProdBundle_available.Name = "chkProdBundle_available";
            this.chkProdBundle_available.Size = new System.Drawing.Size(19, 13);
            this.chkProdBundle_available.TabIndex = 141;
            this.chkProdBundle_available.Values.Text = "";
            // 
            // chkProdBundle_enabled
            // 
            this.chkProdBundle_enabled.Location = new System.Drawing.Point(751, 8);
            this.chkProdBundle_enabled.Name = "chkProdBundle_enabled";
            this.chkProdBundle_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkProdBundle_enabled.TabIndex = 140;
            this.chkProdBundle_enabled.Values.Text = "";
            // 
            // kryptonLabel14
            // 
            this.kryptonLabel14.Location = new System.Drawing.Point(801, 6);
            this.kryptonLabel14.Name = "kryptonLabel14";
            this.kryptonLabel14.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel14.TabIndex = 139;
            this.kryptonLabel14.Values.Text = "组合可用";
            // 
            // kryptonLabel15
            // 
            this.kryptonLabel15.Location = new System.Drawing.Point(686, 6);
            this.kryptonLabel15.Name = "kryptonLabel15";
            this.kryptonLabel15.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel15.TabIndex = 138;
            this.kryptonLabel15.Values.Text = "组合启用";
            // 
            // txt组合名称
            // 
            this.txt组合名称.Location = new System.Drawing.Point(80, 3);
            this.txt组合名称.Name = "txt组合名称";
            this.txt组合名称.Size = new System.Drawing.Size(422, 23);
            this.txt组合名称.TabIndex = 137;
            // 
            // kryptonLabel8
            // 
            this.kryptonLabel8.Location = new System.Drawing.Point(12, 8);
            this.kryptonLabel8.Name = "kryptonLabel8";
            this.kryptonLabel8.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel8.TabIndex = 136;
            this.kryptonLabel8.Values.Text = "组合名称";
            // 
            // btnQueryForGoods
            // 
            this.btnQueryForGoods.Location = new System.Drawing.Point(43, 23);
            this.btnQueryForGoods.Name = "btnQueryForGoods";
            this.btnQueryForGoods.Size = new System.Drawing.Size(92, 36);
            this.btnQueryForGoods.TabIndex = 145;
            this.btnQueryForGoods.Values.Text = "查询";
            this.btnQueryForGoods.Click += new System.EventHandler(this.btnQueryForGoods_Click);
            // 
            // chk包含父级节点
            // 
            this.chk包含父级节点.Location = new System.Drawing.Point(60, 97);
            this.chk包含父级节点.Name = "chk包含父级节点";
            this.chk包含父级节点.Size = new System.Drawing.Size(101, 20);
            this.chk包含父级节点.TabIndex = 153;
            this.chk包含父级节点.Values.Text = "包含父级节点";
            this.chk包含父级节点.Visible = false;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(13, 137);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel2.TabIndex = 146;
            this.kryptonLabel2.Values.Text = "最大行数";
            // 
            // chkMultiSelect
            // 
            this.chkMultiSelect.Location = new System.Drawing.Point(60, 71);
            this.chkMultiSelect.Name = "chkMultiSelect";
            this.chkMultiSelect.Size = new System.Drawing.Size(75, 20);
            this.chkMultiSelect.TabIndex = 150;
            this.chkMultiSelect.Values.Text = "批量选择";
            this.chkMultiSelect.CheckedChanged += new System.EventHandler(this.chkMultiSelect_CheckedChanged);
            // 
            // txtMaxRows
            // 
            this.txtMaxRows.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMaxRows.Location = new System.Drawing.Point(81, 135);
            this.txtMaxRows.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtMaxRows.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtMaxRows.Name = "txtMaxRows";
            this.txtMaxRows.Size = new System.Drawing.Size(63, 22);
            this.txtMaxRows.TabIndex = 147;
            this.txtMaxRows.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // kryptonNavigator1
            // 
            this.kryptonNavigator1.ControlKryptonFormFeatures = false;
            this.kryptonNavigator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigator1.Location = new System.Drawing.Point(0, 0);
            this.kryptonNavigator1.Name = "kryptonNavigator1";
            this.kryptonNavigator1.NavigatorMode = Krypton.Navigator.NavigatorMode.BarTabGroup;
            this.kryptonNavigator1.Owner = null;
            this.kryptonNavigator1.PageBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonNavigator1.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPage产品,
            this.kryptonBOM,
            this.kryptonPage产品组合});
            this.kryptonNavigator1.SelectedIndex = 1;
            this.kryptonNavigator1.Size = new System.Drawing.Size(1118, 448);
            this.kryptonNavigator1.TabIndex = 0;
            this.kryptonNavigator1.Text = "kryptonNavigator1";
            this.kryptonNavigator1.Click += new System.EventHandler(this.chkMultiSelect_CheckedChanged);
            // 
            // kryptonPage产品
            // 
            this.kryptonPage产品.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage产品.Controls.Add(this.newSumDataGridView产品);
            this.kryptonPage产品.Flags = 65534;
            this.kryptonPage产品.LastVisibleSet = true;
            this.kryptonPage产品.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage产品.Name = "kryptonPage产品";
            this.kryptonPage产品.Size = new System.Drawing.Size(1070, 421);
            this.kryptonPage产品.Text = "产品";
            this.kryptonPage产品.ToolTipTitle = "Page ToolTip";
            this.kryptonPage产品.UniqueName = "8E44BECED7CC43E6DCBBD12911C38C48";
            // 
            // newSumDataGridView产品
            // 
            this.newSumDataGridView产品.AllowUserToAddRows = false;
            this.newSumDataGridView产品.AllowUserToDeleteRows = false;
            this.newSumDataGridView产品.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridView产品.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.newSumDataGridView产品.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newSumDataGridView产品.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newSumDataGridView产品.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newSumDataGridView产品.CustomRowNo = false;
            this.newSumDataGridView产品.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridView产品.FieldNameList = null;
            this.newSumDataGridView产品.IsShowSumRow = false;
            this.newSumDataGridView产品.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridView产品.Name = "newSumDataGridView产品";
            this.newSumDataGridView产品.ReadOnly = true;
            this.newSumDataGridView产品.RowTemplate.Height = 23;
            this.newSumDataGridView产品.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newSumDataGridView产品.Size = new System.Drawing.Size(1070, 421);
            this.newSumDataGridView产品.SumColumns = null;
            this.newSumDataGridView产品.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridView产品.SumRowCellFormat = "N2";
            this.newSumDataGridView产品.TabIndex = 100;
            this.newSumDataGridView产品.UseCustomColumnDisplay = true;
            this.newSumDataGridView产品.UseSelectedColumn = false;
            this.newSumDataGridView产品.Use是否使用内置右键功能 = true;
            this.newSumDataGridView产品.XmlFileName = "";
            this.newSumDataGridView产品.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonDataGridView1_CellContentClick);
            this.newSumDataGridView产品.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonDataGridView1_CellDoubleClick);
            this.newSumDataGridView产品.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView1_CellFormatting);
            // 
            // kryptonBOM
            // 
            this.kryptonBOM.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonBOM.Controls.Add(this.treeListView1);
            this.kryptonBOM.Flags = 65534;
            this.kryptonBOM.LastVisibleSet = true;
            this.kryptonBOM.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonBOM.Name = "kryptonBOM";
            this.kryptonBOM.Size = new System.Drawing.Size(1116, 421);
            this.kryptonBOM.Text = "BOM";
            this.kryptonBOM.ToolTipTitle = "Page ToolTip";
            this.kryptonBOM.UniqueName = "6FBE127FB8EF4FCAC1976C4AE1CB7B7D";
            // 
            // treeListView1
            // 
            this.treeListView1.AllowColumnReorder = true;
            this.treeListView1.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            treeListViewItemCollectionComparer1.Column = 0;
            treeListViewItemCollectionComparer1.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.treeListView1.Comparer = treeListViewItemCollectionComparer1;
            this.treeListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView1.GridLines = true;
            this.treeListView1.HideSelection = false;
            this.treeListView1.LabelEdit = true;
            this.treeListView1.Location = new System.Drawing.Point(0, 0);
            this.treeListView1.Name = "treeListView1";
            this.treeListView1.ShowItemToolTips = true;
            this.treeListView1.Size = new System.Drawing.Size(1116, 421);
            this.treeListView1.SmallImageList = this.imageList1;
            this.treeListView1.TabIndex = 1;
            this.treeListView1.UseCompatibleStateImageBehavior = false;
            this.treeListView1.BeforeExpand += new System.Windows.Forms.TreeListViewCancelEventHandler(this.treeListView1_BeforeExpand);
            this.treeListView1.BeforeCollapse += new System.Windows.Forms.TreeListViewCancelEventHandler(this.treeListView1_BeforeCollapse);
            this.treeListView1.DoubleClick += new System.EventHandler(this.treeListView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "属性";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "规格";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "类型";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "实际库存";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "拟销数量";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "在制数量";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "在途数量";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "未发数量";
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "预警数量";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1.ico");
            this.imageList1.Images.SetKeyName(1, "4.ico");
            this.imageList1.Images.SetKeyName(2, "6.ico");
            this.imageList1.Images.SetKeyName(3, "7.ico");
            this.imageList1.Images.SetKeyName(4, "9.ico");
            this.imageList1.Images.SetKeyName(5, "279.GIF");
            // 
            // kryptonPage产品组合
            // 
            this.kryptonPage产品组合.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage产品组合.Controls.Add(this.newSumDataGridView产品组合);
            this.kryptonPage产品组合.Flags = 65534;
            this.kryptonPage产品组合.LastVisibleSet = true;
            this.kryptonPage产品组合.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage产品组合.Name = "kryptonPage产品组合";
            this.kryptonPage产品组合.Size = new System.Drawing.Size(1116, 421);
            this.kryptonPage产品组合.Text = "产品组合";
            this.kryptonPage产品组合.ToolTipTitle = "Page ToolTip";
            this.kryptonPage产品组合.UniqueName = "827BA60CAD844C8272B7DAF507A171DF";
            // 
            // newSumDataGridView产品组合
            // 
            this.newSumDataGridView产品组合.AllowUserToAddRows = false;
            this.newSumDataGridView产品组合.AllowUserToDeleteRows = false;
            this.newSumDataGridView产品组合.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridView产品组合.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.newSumDataGridView产品组合.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newSumDataGridView产品组合.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newSumDataGridView产品组合.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newSumDataGridView产品组合.CustomRowNo = false;
            this.newSumDataGridView产品组合.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridView产品组合.FieldNameList = null;
            this.newSumDataGridView产品组合.IsShowSumRow = false;
            this.newSumDataGridView产品组合.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridView产品组合.Name = "newSumDataGridView产品组合";
            this.newSumDataGridView产品组合.ReadOnly = true;
            this.newSumDataGridView产品组合.RowTemplate.Height = 23;
            this.newSumDataGridView产品组合.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newSumDataGridView产品组合.Size = new System.Drawing.Size(1116, 421);
            this.newSumDataGridView产品组合.SumColumns = null;
            this.newSumDataGridView产品组合.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridView产品组合.SumRowCellFormat = "N2";
            this.newSumDataGridView产品组合.TabIndex = 101;
            this.newSumDataGridView产品组合.UseCustomColumnDisplay = true;
            this.newSumDataGridView产品组合.UseSelectedColumn = false;
            this.newSumDataGridView产品组合.Use是否使用内置右键功能 = true;
            this.newSumDataGridView产品组合.XmlFileName = "";
            this.newSumDataGridView产品组合.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newSumDataGridView产品组合_CellContentClick);
            this.newSumDataGridView产品组合.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newSumDataGridView产品组合_CellDoubleClick);
            this.newSumDataGridView产品组合.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newSumDataGridView产品组合_CellFormatting);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(579, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(406, 14);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.箱规ToolStripMenuItem,
            this.按SKU添加箱规ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(231, 48);
            // 
            // 箱规ToolStripMenuItem
            // 
            this.箱规ToolStripMenuItem.Name = "箱规ToolStripMenuItem";
            this.箱规ToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.箱规ToolStripMenuItem.Text = "按【品    名】添加包装参数";
            this.箱规ToolStripMenuItem.Click += new System.EventHandler(this.箱规ToolStripMenuItem_Click);
            // 
            // 按SKU添加箱规ToolStripMenuItem
            // 
            this.按SKU添加箱规ToolStripMenuItem.Name = "按SKU添加箱规ToolStripMenuItem";
            this.按SKU添加箱规ToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.按SKU添加箱规ToolStripMenuItem.Text = "按【多属性】添加包装参数";
            this.按SKU添加箱规ToolStripMenuItem.Click += new System.EventHandler(this.按SKU添加箱规ToolStripMenuItem_Click);
            // 
            // UCProdQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Name = "UCProdQuery";
            this.Size = new System.Drawing.Size(1118, 682);
            this.Load += new System.EventHandler(this.QueryForm_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerQuery.Panel1)).EndInit();
            this.kryptonSplitContainerQuery.Panel1.ResumeLayout(false);
            this.kryptonSplitContainerQuery.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerQuery.Panel2)).EndInit();
            this.kryptonSplitContainerQuery.Panel2.ResumeLayout(false);
            this.kryptonSplitContainerQuery.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerQuery)).EndInit();
            this.kryptonSplitContainerQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelProd)).EndInit();
            this.kryptonPanelProd.ResumeLayout(false);
            this.kryptonPanelProd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStockJudgement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbdepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtType_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelGroup)).EndInit();
            this.kryptonPanelGroup.ResumeLayout(false);
            this.kryptonPanelGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).EndInit();
            this.kryptonNavigator1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage产品)).EndInit();
            this.kryptonPage产品.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridView产品)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonBOM)).EndInit();
            this.kryptonBOM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage产品组合)).EndInit();
            this.kryptonPage产品组合.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridView产品组合)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroup)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       

        #endregion

        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private Krypton.Navigator.KryptonNavigator kryptonNavigator1;
        private Krypton.Navigator.KryptonPage kryptonPage产品;
        private Krypton.Navigator.KryptonPage kryptonPage产品组合;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Navigator.KryptonPage kryptonBOM;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer2;
        private UControls.NewSumDataGridView newSumDataGridView产品;
        private RUINOR.WinFormsUI.ComboBoxTreeView txtcategory_ID;
        private Krypton.Toolkit.KryptonLabel lblNo;
        private Krypton.Toolkit.KryptonTextBox txtNo;
        private Krypton.Toolkit.KryptonLabel lblName;
        private Krypton.Toolkit.KryptonTextBox txtName;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel lblShortCode;
        private Krypton.Toolkit.KryptonTextBox txtShortCode;
        private Krypton.Toolkit.KryptonTextBox txtBarCode;
        private Krypton.Toolkit.KryptonLabel lblcategory_ID;
        private Krypton.Toolkit.KryptonComboBox txtType_ID;
        private Krypton.Toolkit.KryptonLabel lblType_ID;
        private Krypton.Toolkit.KryptonLabel lblSpecifications;
        private Krypton.Toolkit.KryptonTextBox txtSpecifications;
        private Krypton.Toolkit.KryptonButton btnQueryForGoods;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonNumericUpDown txtMaxRows;
        private System.Windows.Forms.BindingSource bindingSourceProdDetail;
        private System.Windows.Forms.BindingSource bindingSourceBOM;
        private System.Windows.Forms.BindingSource bindingSourceGroup;
        private Krypton.Toolkit.KryptonCheckBox chkIncludingChild;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonTextBox txtModel;
        private Krypton.Toolkit.KryptonCheckBox chkMultiSelect;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonTextBox txtSKU码;
        private System.Windows.Forms.TreeListView treeListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imageList1;
        private Krypton.Toolkit.KryptonCheckBox chk包含父级节点;
        private Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private Krypton.Toolkit.KryptonTextBox txtProp;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private Krypton.Toolkit.KryptonComboBox cmbdepartment;
        private Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private Krypton.Toolkit.KryptonComboBox cmbLocation;
        private Krypton.Toolkit.KryptonLabel kryptonLabel7;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 箱规ToolStripMenuItem;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelProd;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainerQuery;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelGroup;
        private Krypton.Toolkit.KryptonTextBox txt组合名称;
        private Krypton.Toolkit.KryptonLabel kryptonLabel8;
        private Krypton.Toolkit.KryptonComboBox cmbStockJudgement;
        private Krypton.Toolkit.KryptonLabel kryptonLabel9;
        private ToolStripMenuItem 按SKU添加箱规ToolStripMenuItem;
        private ToolTip toolTip1;
        internal Krypton.Toolkit.KryptonDateTimePicker dtp2;
        internal Krypton.Toolkit.KryptonDateTimePicker dtp1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel10;
        private Krypton.Toolkit.KryptonLabel lblLastInventoryDate;
        private Krypton.Toolkit.KryptonCheckBox chkProd_available;
        private Krypton.Toolkit.KryptonCheckBox chkProd_enabled;
        private Krypton.Toolkit.KryptonLabel lblis_available;
        private Krypton.Toolkit.KryptonLabel lblis_enabled;
        private Krypton.Toolkit.KryptonCheckBox chksku_available;
        private Krypton.Toolkit.KryptonCheckBox chksku_enabled;
        private Krypton.Toolkit.KryptonLabel kryptonLabel11;
        private Krypton.Toolkit.KryptonLabel kryptonLabel12;
        private Krypton.Toolkit.KryptonLabel kryptonLabel15;
        private Krypton.Toolkit.KryptonLabel kryptonLabel14;
        private Krypton.Toolkit.KryptonCheckBox chkProdBundle_enabled;
        private UControls.NewSumDataGridView newSumDataGridView产品组合;
        private Krypton.Toolkit.KryptonCheckBox chkProdBundle_available;
    }
}