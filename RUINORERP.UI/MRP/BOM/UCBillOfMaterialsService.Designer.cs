using System;
using System.Windows.Forms;

namespace RUINORERP.UI.MRP.BOM
{
    partial class UCBillOfMaterialsService
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCBillOfMaterialsService));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainer2 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainerQuery = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonPanelProd = new Krypton.Toolkit.KryptonPanel();
            this.kryptonPanelBOM = new Krypton.Toolkit.KryptonPanel();
            this.txtBOM_Name = new Krypton.Toolkit.KryptonTextBox();
            this.txtSKUName = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel9 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel10 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel12 = new Krypton.Toolkit.KryptonLabel();
            this.txtSKUCode = new Krypton.Toolkit.KryptonTextBox();
            this.cmbParentProdType = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel16 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel18 = new Krypton.Toolkit.KryptonLabel();
            this.txtBOM_NO = new Krypton.Toolkit.KryptonTextBox();
            this.txtName = new Krypton.Toolkit.KryptonTextBox();
            this.chkIncludingChild = new Krypton.Toolkit.KryptonCheckBox();
            this.lblcategory_ID = new Krypton.Toolkit.KryptonLabel();
            this.txtShortCode = new Krypton.Toolkit.KryptonTextBox();
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
            this.txtFilesName = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel8 = new Krypton.Toolkit.KryptonLabel();
            this.chkMultiBOMProd = new Krypton.Toolkit.KryptonCheckBox();
            this.btnQueryForGoods = new Krypton.Toolkit.KryptonButton();
            this.btnCheckInvalidBOM = new Krypton.Toolkit.KryptonButton();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel7 = new Krypton.Toolkit.KryptonLabel();
            this.txtMaxRows = new Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonNavigator1 = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPage产品 = new Krypton.Navigator.KryptonPage();
            this.newSumDataGridView产品 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonBOM = new Krypton.Navigator.KryptonPage();
            this.newSumDataGridViewMain = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonPage产品组合 = new Krypton.Navigator.KryptonPage();
            this.newSumDataGridView产品组合 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.newSumDataGridViewBOM = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bindingSourceProdDetail = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceBOM = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceGroup = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bsBoms = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceBomMain = new System.Windows.Forms.BindingSource(this.components);
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelBOM)).BeginInit();
            this.kryptonPanelBOM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbParentProdType)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage产品组合)).BeginInit();
            this.kryptonPage产品组合.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridView产品组合)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewBOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroup)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsBoms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBomMain)).BeginInit();
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
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.newSumDataGridViewBOM);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(1118, 682);
            this.kryptonSplitContainer1.SplitterDistance = 485;
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
            this.kryptonSplitContainer2.Size = new System.Drawing.Size(1118, 485);
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
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.chkMultiBOMProd);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.btnQueryForGoods);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.btnCheckInvalidBOM);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.kryptonLabel2);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.kryptonLabel7);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.txtMaxRows);
            this.kryptonSplitContainerQuery.Size = new System.Drawing.Size(1118, 167);
            this.kryptonSplitContainerQuery.SplitterDistance = 893;
            this.kryptonSplitContainerQuery.TabIndex = 161;
            // 
            // kryptonPanelProd
            // 
            this.kryptonPanelProd.AutoSize = true;
            this.kryptonPanelProd.Controls.Add(this.kryptonPanelBOM);
            this.kryptonPanelProd.Controls.Add(this.txtName);
            this.kryptonPanelProd.Controls.Add(this.chkIncludingChild);
            this.kryptonPanelProd.Controls.Add(this.lblcategory_ID);
            this.kryptonPanelProd.Controls.Add(this.txtShortCode);
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
            this.kryptonPanelProd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelProd.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelProd.Name = "kryptonPanelProd";
            this.kryptonPanelProd.Size = new System.Drawing.Size(893, 136);
            this.kryptonPanelProd.TabIndex = 160;
            // 
            // kryptonPanelBOM
            // 
            this.kryptonPanelBOM.AutoSize = true;
            this.kryptonPanelBOM.Controls.Add(this.txtBOM_Name);
            this.kryptonPanelBOM.Controls.Add(this.txtSKUName);
            this.kryptonPanelBOM.Controls.Add(this.kryptonLabel9);
            this.kryptonPanelBOM.Controls.Add(this.kryptonLabel10);
            this.kryptonPanelBOM.Controls.Add(this.kryptonLabel12);
            this.kryptonPanelBOM.Controls.Add(this.txtSKUCode);
            this.kryptonPanelBOM.Controls.Add(this.cmbParentProdType);
            this.kryptonPanelBOM.Controls.Add(this.kryptonLabel16);
            this.kryptonPanelBOM.Controls.Add(this.kryptonLabel18);
            this.kryptonPanelBOM.Controls.Add(this.txtBOM_NO);
            this.kryptonPanelBOM.Location = new System.Drawing.Point(4, 16);
            this.kryptonPanelBOM.Name = "kryptonPanelBOM";
            this.kryptonPanelBOM.Size = new System.Drawing.Size(625, 103);
            this.kryptonPanelBOM.TabIndex = 161;
            // 
            // txtBOM_Name
            // 
            this.txtBOM_Name.Location = new System.Drawing.Point(339, 2);
            this.txtBOM_Name.Name = "txtBOM_Name";
            this.txtBOM_Name.Size = new System.Drawing.Size(163, 23);
            this.txtBOM_Name.TabIndex = 135;
            // 
            // txtSKUName
            // 
            this.txtSKUName.Location = new System.Drawing.Point(90, 33);
            this.txtSKUName.Name = "txtSKUName";
            this.txtSKUName.Size = new System.Drawing.Size(151, 23);
            this.txtSKUName.TabIndex = 137;
            // 
            // kryptonLabel9
            // 
            this.kryptonLabel9.Location = new System.Drawing.Point(25, 36);
            this.kryptonLabel9.Name = "kryptonLabel9";
            this.kryptonLabel9.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel9.TabIndex = 136;
            this.kryptonLabel9.Values.Text = "母件名称";
            // 
            // kryptonLabel10
            // 
            this.kryptonLabel10.Location = new System.Drawing.Point(271, 4);
            this.kryptonLabel10.Name = "kryptonLabel10";
            this.kryptonLabel10.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel10.TabIndex = 134;
            this.kryptonLabel10.Values.Text = "配方名称";
            // 
            // kryptonLabel12
            // 
            this.kryptonLabel12.Location = new System.Drawing.Point(262, 37);
            this.kryptonLabel12.Name = "kryptonLabel12";
            this.kryptonLabel12.Size = new System.Drawing.Size(72, 20);
            this.kryptonLabel12.TabIndex = 151;
            this.kryptonLabel12.Values.Text = "母件SKU码";
            // 
            // txtSKUCode
            // 
            this.txtSKUCode.Location = new System.Drawing.Point(340, 36);
            this.txtSKUCode.Name = "txtSKUCode";
            this.txtSKUCode.Size = new System.Drawing.Size(162, 23);
            this.txtSKUCode.TabIndex = 152;
            // 
            // cmbParentProdType
            // 
            this.cmbParentProdType.DropDownWidth = 205;
            this.cmbParentProdType.IntegralHeight = false;
            this.cmbParentProdType.Location = new System.Drawing.Point(90, 71);
            this.cmbParentProdType.Name = "cmbParentProdType";
            this.cmbParentProdType.Size = new System.Drawing.Size(412, 21);
            this.cmbParentProdType.TabIndex = 142;
            // 
            // kryptonLabel16
            // 
            this.kryptonLabel16.Location = new System.Drawing.Point(25, 72);
            this.kryptonLabel16.Name = "kryptonLabel16";
            this.kryptonLabel16.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel16.TabIndex = 141;
            this.kryptonLabel16.Values.Text = "母件类型";
            // 
            // kryptonLabel18
            // 
            this.kryptonLabel18.Location = new System.Drawing.Point(25, 5);
            this.kryptonLabel18.Name = "kryptonLabel18";
            this.kryptonLabel18.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel18.TabIndex = 132;
            this.kryptonLabel18.Values.Text = "配方编号";
            // 
            // txtBOM_NO
            // 
            this.txtBOM_NO.Location = new System.Drawing.Point(90, 4);
            this.txtBOM_NO.Name = "txtBOM_NO";
            this.txtBOM_NO.Size = new System.Drawing.Size(150, 23);
            this.txtBOM_NO.TabIndex = 133;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(339, 2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(163, 23);
            this.txtName.TabIndex = 135;
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
            // txtShortCode
            // 
            this.txtShortCode.Location = new System.Drawing.Point(55, 55);
            this.txtShortCode.Name = "txtShortCode";
            this.txtShortCode.Size = new System.Drawing.Size(151, 23);
            this.txtShortCode.TabIndex = 137;
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
            this.lblName.Location = new System.Drawing.Point(301, 3);
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
            this.kryptonLabel4.Location = new System.Drawing.Point(291, 109);
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
            this.txtSKU码.Location = new System.Drawing.Point(340, 106);
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
            this.lblSpecifications.Location = new System.Drawing.Point(17, 32);
            this.lblSpecifications.Name = "lblSpecifications";
            this.lblSpecifications.Size = new System.Drawing.Size(36, 20);
            this.lblSpecifications.TabIndex = 143;
            this.lblSpecifications.Values.Text = "规格";
            // 
            // txtType_ID
            // 
            this.txtType_ID.DropDownWidth = 205;
            this.txtType_ID.IntegralHeight = false;
            this.txtType_ID.Location = new System.Drawing.Point(340, 80);
            this.txtType_ID.Name = "txtType_ID";
            this.txtType_ID.Size = new System.Drawing.Size(162, 21);
            this.txtType_ID.TabIndex = 142;
            // 
            // lblType_ID
            // 
            this.lblType_ID.Location = new System.Drawing.Point(275, 81);
            this.lblType_ID.Name = "lblType_ID";
            this.lblType_ID.Size = new System.Drawing.Size(62, 20);
            this.lblType_ID.TabIndex = 141;
            this.lblType_ID.Values.Text = "产品类型";
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(339, 28);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(163, 23);
            this.txtModel.TabIndex = 133;
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(301, 27);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel3.TabIndex = 132;
            this.kryptonLabel3.Values.Text = "型号";
            // 
            // txtBarCode
            // 
            this.txtBarCode.Location = new System.Drawing.Point(340, 55);
            this.txtBarCode.Name = "txtBarCode";
            this.txtBarCode.Size = new System.Drawing.Size(162, 23);
            this.txtBarCode.TabIndex = 138;
            // 
            // lblNo
            // 
            this.lblNo.Location = new System.Drawing.Point(17, 5);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(36, 20);
            this.lblNo.TabIndex = 132;
            this.lblNo.Values.Text = "品号";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(301, 56);
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
            this.kryptonPanelGroup.Controls.Add(this.txtFilesName);
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
            this.kryptonLabel14.Values.Text = "文件可用";
            // 
            // kryptonLabel15
            // 
            this.kryptonLabel15.Location = new System.Drawing.Point(686, 6);
            this.kryptonLabel15.Name = "kryptonLabel15";
            this.kryptonLabel15.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel15.TabIndex = 138;
            this.kryptonLabel15.Values.Text = "文件启用";
            // 
            // txtFilesName
            // 
            this.txtFilesName.Location = new System.Drawing.Point(80, 3);
            this.txtFilesName.Name = "txtFilesName";
            this.txtFilesName.Size = new System.Drawing.Size(422, 23);
            this.txtFilesName.TabIndex = 137;
            // 
            // kryptonLabel8
            // 
            this.kryptonLabel8.Location = new System.Drawing.Point(12, 8);
            this.kryptonLabel8.Name = "kryptonLabel8";
            this.kryptonLabel8.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel8.TabIndex = 136;
            this.kryptonLabel8.Values.Text = "文件名称";
            // 
            // chkMultiBOMProd
            // 
            this.chkMultiBOMProd.Location = new System.Drawing.Point(139, 67);
            this.chkMultiBOMProd.Name = "chkMultiBOMProd";
            this.chkMultiBOMProd.Size = new System.Drawing.Size(19, 13);
            this.chkMultiBOMProd.TabIndex = 177;
            this.chkMultiBOMProd.Values.Text = "";
            // 
            // btnQueryForGoods
            // 
            this.btnQueryForGoods.Location = new System.Drawing.Point(66, 16);
            this.btnQueryForGoods.Name = "btnQueryForGoods";
            this.btnQueryForGoods.Size = new System.Drawing.Size(92, 36);
            this.btnQueryForGoods.TabIndex = 145;
            this.btnQueryForGoods.Values.Text = "查询";
            this.btnQueryForGoods.Click += new System.EventHandler(this.btnQueryForGoods_Click);
            // 
            // btnCheckInvalidBOM
            // 
            this.btnCheckInvalidBOM.Location = new System.Drawing.Point(66, 94);
            this.btnCheckInvalidBOM.Name = "btnCheckInvalidBOM";
            this.btnCheckInvalidBOM.Size = new System.Drawing.Size(92, 36);
            this.btnCheckInvalidBOM.TabIndex = 144;
            this.btnCheckInvalidBOM.Values.Text = "配方检测";
            this.btnCheckInvalidBOM.Click += new System.EventHandler(this.btnCheckInvalidBOM_Click);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(13, 137);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel2.TabIndex = 146;
            this.kryptonLabel2.Values.Text = "最大行数";
            // 
            // kryptonLabel7
            // 
            this.kryptonLabel7.Location = new System.Drawing.Point(60, 65);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(75, 20);
            this.kryptonLabel7.TabIndex = 176;
            this.kryptonLabel7.Values.Text = "多配方产品";
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
            this.kryptonNavigator1.Size = new System.Drawing.Size(1118, 313);
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
            this.kryptonPage产品.Size = new System.Drawing.Size(1116, 286);
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
            this.newSumDataGridView产品.BizInvisibleCols = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("newSumDataGridView产品.BizInvisibleCols")));
            this.newSumDataGridView产品.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newSumDataGridView产品.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newSumDataGridView产品.CustomRowNo = false;
            this.newSumDataGridView产品.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridView产品.EnableFiltering = false;
            this.newSumDataGridView产品.FieldNameList = null;
            this.newSumDataGridView产品.IsShowSumRow = false;
            this.newSumDataGridView产品.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridView产品.Name = "newSumDataGridView产品";
            this.newSumDataGridView产品.NeedSaveColumnsXml = true;
            this.newSumDataGridView产品.ReadOnly = true;
            this.newSumDataGridView产品.RowTemplate.Height = 23;
            this.newSumDataGridView产品.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newSumDataGridView产品.Size = new System.Drawing.Size(1116, 286);
            this.newSumDataGridView产品.SumColumns = null;
            this.newSumDataGridView产品.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridView产品.SumRowCellFormat = "N2";
            this.newSumDataGridView产品.TabIndex = 100;
            this.newSumDataGridView产品.UseBatchEditColumn = false;
            this.newSumDataGridView产品.UseCustomColumnDisplay = true;
            this.newSumDataGridView产品.UseSelectedColumn = false;
            this.newSumDataGridView产品.Use是否使用内置右键功能 = true;
            this.newSumDataGridView产品.XmlFileName = "";
            this.newSumDataGridView产品.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonDataGridView1_CellContentClick);
            this.newSumDataGridView产品.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonDataGridView1_CellDoubleClick);
            this.newSumDataGridView产品.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView1_CellFormatting);
            this.newSumDataGridView产品.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.KryptonDataGridView产品_CellPainting);
            // 
            // kryptonBOM
            // 
            this.kryptonBOM.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonBOM.Controls.Add(this.newSumDataGridViewMain);
            this.kryptonBOM.Flags = 65534;
            this.kryptonBOM.LastVisibleSet = true;
            this.kryptonBOM.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonBOM.Name = "kryptonBOM";
            this.kryptonBOM.Size = new System.Drawing.Size(1116, 286);
            this.kryptonBOM.Text = "BOM";
            this.kryptonBOM.ToolTipTitle = "Page ToolTip";
            this.kryptonBOM.UniqueName = "6FBE127FB8EF4FCAC1976C4AE1CB7B7D";
            // 
            // newSumDataGridViewMain
            // 
            this.newSumDataGridViewMain.AllowUserToAddRows = false;
            this.newSumDataGridViewMain.AllowUserToDeleteRows = false;
            this.newSumDataGridViewMain.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridViewMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.newSumDataGridViewMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newSumDataGridViewMain.BizInvisibleCols = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("newSumDataGridViewMain.BizInvisibleCols")));
            this.newSumDataGridViewMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newSumDataGridViewMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newSumDataGridViewMain.CustomRowNo = false;
            this.newSumDataGridViewMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridViewMain.EnableFiltering = false;
            this.newSumDataGridViewMain.FieldNameList = ((System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.KeyValuePair<string, bool>>)(resources.GetObject("newSumDataGridViewMain.FieldNameList")));
            this.newSumDataGridViewMain.IsShowSumRow = false;
            this.newSumDataGridViewMain.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridViewMain.Name = "newSumDataGridViewMain";
            this.newSumDataGridViewMain.NeedSaveColumnsXml = true;
            this.newSumDataGridViewMain.ReadOnly = true;
            this.newSumDataGridViewMain.RowTemplate.Height = 23;
            this.newSumDataGridViewMain.Size = new System.Drawing.Size(1116, 286);
            this.newSumDataGridViewMain.SumColumns = null;
            this.newSumDataGridViewMain.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridViewMain.SumRowCellFormat = "N2";
            this.newSumDataGridViewMain.TabIndex = 1;
            this.newSumDataGridViewMain.UseBatchEditColumn = false;
            this.newSumDataGridViewMain.UseCustomColumnDisplay = true;
            this.newSumDataGridViewMain.UseSelectedColumn = false;
            this.newSumDataGridViewMain.Use是否使用内置右键功能 = true;
            this.newSumDataGridViewMain.XmlFileName = "";
            // 
            // kryptonPage产品组合
            // 
            this.kryptonPage产品组合.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage产品组合.Controls.Add(this.newSumDataGridView产品组合);
            this.kryptonPage产品组合.Flags = 65534;
            this.kryptonPage产品组合.LastVisibleSet = true;
            this.kryptonPage产品组合.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage产品组合.Name = "kryptonPage产品组合";
            this.kryptonPage产品组合.Size = new System.Drawing.Size(1116, 286);
            this.kryptonPage产品组合.Text = "配方文档";
            this.kryptonPage产品组合.ToolTipTitle = "Page ToolTip";
            this.kryptonPage产品组合.UniqueName = "827BA60CAD844C8272B7DAF507A171DF";
            this.kryptonPage产品组合.Visible = false;
            // 
            // newSumDataGridView产品组合
            // 
            this.newSumDataGridView产品组合.AllowUserToAddRows = false;
            this.newSumDataGridView产品组合.AllowUserToDeleteRows = false;
            this.newSumDataGridView产品组合.AllowUserToOrderColumns = true;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridView产品组合.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.newSumDataGridView产品组合.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newSumDataGridView产品组合.BizInvisibleCols = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("newSumDataGridView产品组合.BizInvisibleCols")));
            this.newSumDataGridView产品组合.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newSumDataGridView产品组合.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newSumDataGridView产品组合.CustomRowNo = false;
            this.newSumDataGridView产品组合.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridView产品组合.EnableFiltering = false;
            this.newSumDataGridView产品组合.FieldNameList = null;
            this.newSumDataGridView产品组合.IsShowSumRow = false;
            this.newSumDataGridView产品组合.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridView产品组合.Name = "newSumDataGridView产品组合";
            this.newSumDataGridView产品组合.NeedSaveColumnsXml = true;
            this.newSumDataGridView产品组合.ReadOnly = true;
            this.newSumDataGridView产品组合.RowTemplate.Height = 23;
            this.newSumDataGridView产品组合.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newSumDataGridView产品组合.Size = new System.Drawing.Size(1116, 286);
            this.newSumDataGridView产品组合.SumColumns = null;
            this.newSumDataGridView产品组合.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridView产品组合.SumRowCellFormat = "N2";
            this.newSumDataGridView产品组合.TabIndex = 101;
            this.newSumDataGridView产品组合.UseBatchEditColumn = false;
            this.newSumDataGridView产品组合.UseCustomColumnDisplay = true;
            this.newSumDataGridView产品组合.UseSelectedColumn = false;
            this.newSumDataGridView产品组合.Use是否使用内置右键功能 = true;
            this.newSumDataGridView产品组合.XmlFileName = "";
            this.newSumDataGridView产品组合.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newSumDataGridView产品组合_CellContentClick);
            this.newSumDataGridView产品组合.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newSumDataGridView产品组合_CellDoubleClick);
            this.newSumDataGridView产品组合.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newSumDataGridView产品组合_CellFormatting);
            // 
            // newSumDataGridViewBOM
            // 
            this.newSumDataGridViewBOM.AllowUserToAddRows = false;
            this.newSumDataGridViewBOM.AllowUserToDeleteRows = false;
            this.newSumDataGridViewBOM.AllowUserToOrderColumns = true;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridViewBOM.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.newSumDataGridViewBOM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newSumDataGridViewBOM.BizInvisibleCols = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("newSumDataGridViewBOM.BizInvisibleCols")));
            this.newSumDataGridViewBOM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newSumDataGridViewBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newSumDataGridViewBOM.CustomRowNo = false;
            this.newSumDataGridViewBOM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridViewBOM.EnableFiltering = false;
            this.newSumDataGridViewBOM.FieldNameList = null;
            this.newSumDataGridViewBOM.IsShowSumRow = false;
            this.newSumDataGridViewBOM.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridViewBOM.Name = "newSumDataGridViewBOM";
            this.newSumDataGridViewBOM.NeedSaveColumnsXml = true;
            this.newSumDataGridViewBOM.ReadOnly = true;
            this.newSumDataGridViewBOM.RowTemplate.Height = 23;
            this.newSumDataGridViewBOM.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newSumDataGridViewBOM.Size = new System.Drawing.Size(1118, 192);
            this.newSumDataGridViewBOM.SumColumns = null;
            this.newSumDataGridViewBOM.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridViewBOM.SumRowCellFormat = "N2";
            this.newSumDataGridViewBOM.TabIndex = 101;
            this.newSumDataGridViewBOM.UseBatchEditColumn = false;
            this.newSumDataGridViewBOM.UseCustomColumnDisplay = true;
            this.newSumDataGridViewBOM.UseSelectedColumn = false;
            this.newSumDataGridViewBOM.Use是否使用内置右键功能 = true;
            this.newSumDataGridViewBOM.XmlFileName = "";
            this.newSumDataGridViewBOM.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.KryptonDataGridViewBOM_CellPainting);
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(166, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItem1.Text = "指定为默认配方";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // UCBillOfMaterialsService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Name = "UCBillOfMaterialsService";
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelBOM)).EndInit();
            this.kryptonPanelBOM.ResumeLayout(false);
            this.kryptonPanelBOM.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbParentProdType)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage产品组合)).EndInit();
            this.kryptonPage产品组合.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridView产品组合)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewBOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroup)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsBoms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBomMain)).EndInit();
            this.ResumeLayout(false);

        }

       

        #endregion

        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private Krypton.Navigator.KryptonNavigator kryptonNavigator1;
        private Krypton.Navigator.KryptonPage kryptonPage产品;
        private Krypton.Navigator.KryptonPage kryptonPage产品组合;
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
        private Krypton.Toolkit.KryptonButton btnCheckInvalidBOM;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonNumericUpDown txtMaxRows;
        private System.Windows.Forms.BindingSource bindingSourceProdDetail;
        private System.Windows.Forms.BindingSource bindingSourceBOM;
        private System.Windows.Forms.BindingSource bindingSourceGroup;
        private Krypton.Toolkit.KryptonCheckBox chkIncludingChild;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonTextBox txtModel;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonTextBox txtSKU码;
        private System.Windows.Forms.ImageList imageList1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private Krypton.Toolkit.KryptonTextBox txtProp;
        private ContextMenuStrip contextMenuStrip1;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelProd;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainerQuery;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelGroup;
        private Krypton.Toolkit.KryptonTextBox txtFilesName;
        private Krypton.Toolkit.KryptonLabel kryptonLabel8;
        private ToolTip toolTip1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel15;
        private Krypton.Toolkit.KryptonLabel kryptonLabel14;
        private Krypton.Toolkit.KryptonCheckBox chkProdBundle_enabled;
        private UControls.NewSumDataGridView newSumDataGridView产品组合;
        private Krypton.Toolkit.KryptonCheckBox chkProdBundle_available;
        private UControls.NewSumDataGridView newSumDataGridViewBOM;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem toolStripMenuItem1;
        private BindingSource bsBoms;
        private Krypton.Toolkit.KryptonCheckBox chkMultiBOMProd;
        private Krypton.Toolkit.KryptonLabel kryptonLabel7;
        internal UControls.NewSumDataGridView newSumDataGridViewMain;
        private BindingSource bindingSourceBomMain;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelBOM;
        private Krypton.Toolkit.KryptonTextBox txtBOM_Name;
        private Krypton.Toolkit.KryptonLabel kryptonLabel10;
        private Krypton.Toolkit.KryptonLabel kryptonLabel12;
        private Krypton.Toolkit.KryptonTextBox txtSKUCode;
        private Krypton.Toolkit.KryptonComboBox cmbParentProdType;
        private Krypton.Toolkit.KryptonLabel kryptonLabel16;
        private Krypton.Toolkit.KryptonTextBox txtSKUName;
        private Krypton.Toolkit.KryptonLabel kryptonLabel9;
        private Krypton.Toolkit.KryptonLabel kryptonLabel18;
        private Krypton.Toolkit.KryptonTextBox txtBOM_NO;
    }
}