﻿using System;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer1 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCBillOfMaterialsService));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainer2 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainerQuery = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonPanelProd = new Krypton.Toolkit.KryptonPanel();
            this.txtBrand = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel13 = new Krypton.Toolkit.KryptonLabel();
            this.txtName = new Krypton.Toolkit.KryptonTextBox();
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
            this.txtFilesName = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel8 = new Krypton.Toolkit.KryptonLabel();
            this.btnQueryForGoods = new Krypton.Toolkit.KryptonButton();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.txtMaxRows = new Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonNavigator1 = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPage产品 = new Krypton.Navigator.KryptonPage();
            this.newSumDataGridView产品 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonBOM = new Krypton.Navigator.KryptonPage();
            this.treeListView1 = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSKU = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader型号 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.newSumDataGridViewBOM = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.bindingSourceProdDetail = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceBOM = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceGroup = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bsBoms = new System.Windows.Forms.BindingSource(this.components);
            this.chkMultiBOMProd = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel7 = new Krypton.Toolkit.KryptonLabel();
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
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewBOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroup)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsBoms)).BeginInit();
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
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.btnQueryForGoods);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.kryptonLabel2);
            this.kryptonSplitContainerQuery.Panel2.Controls.Add(this.txtMaxRows);
            this.kryptonSplitContainerQuery.Size = new System.Drawing.Size(1118, 167);
            this.kryptonSplitContainerQuery.SplitterDistance = 893;
            this.kryptonSplitContainerQuery.TabIndex = 161;
            // 
            // kryptonPanelProd
            // 
            this.kryptonPanelProd.AutoSize = true;
            this.kryptonPanelProd.Controls.Add(this.chkMultiBOMProd);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel7);
            this.kryptonPanelProd.Controls.Add(this.txtBrand);
            this.kryptonPanelProd.Controls.Add(this.kryptonLabel13);
            this.kryptonPanelProd.Controls.Add(this.txtName);
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
            // txtBrand
            // 
            this.txtBrand.Location = new System.Drawing.Point(712, 29);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(126, 23);
            this.txtBrand.TabIndex = 175;
            // 
            // kryptonLabel13
            // 
            this.kryptonLabel13.Location = new System.Drawing.Point(671, 30);
            this.kryptonLabel13.Name = "kryptonLabel13";
            this.kryptonLabel13.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel13.TabIndex = 174;
            this.kryptonLabel13.Values.Text = "品牌";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(318, 2);
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
            // cmbdepartment
            // 
            this.cmbdepartment.DropDownWidth = 205;
            this.cmbdepartment.IntegralHeight = false;
            this.cmbdepartment.Location = new System.Drawing.Point(712, 5);
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
            this.kryptonLabel6.Location = new System.Drawing.Point(671, 7);
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
            this.kryptonLabel4.Location = new System.Drawing.Point(660, 59);
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
            this.txtSKU码.Location = new System.Drawing.Point(712, 58);
            this.txtSKU码.Name = "txtSKU码";
            this.txtSKU码.Size = new System.Drawing.Size(126, 23);
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
            this.lblNo.Location = new System.Drawing.Point(17, 5);
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
            // btnQueryForGoods
            // 
            this.btnQueryForGoods.Location = new System.Drawing.Point(60, 23);
            this.btnQueryForGoods.Name = "btnQueryForGoods";
            this.btnQueryForGoods.Size = new System.Drawing.Size(92, 36);
            this.btnQueryForGoods.TabIndex = 145;
            this.btnQueryForGoods.Values.Text = "查询";
            this.btnQueryForGoods.Click += new System.EventHandler(this.btnQueryForGoods_Click);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(13, 137);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel2.TabIndex = 146;
            this.kryptonLabel2.Values.Text = "最大行数";
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
            this.kryptonNavigator1.SelectedIndex = 0;
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
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridView产品.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
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
            this.newSumDataGridView产品.Size = new System.Drawing.Size(1116, 286);
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
            this.newSumDataGridView产品.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.KryptonDataGridView产品_CellPainting);
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
            this.kryptonBOM.Visible = false;
            // 
            // treeListView1
            // 
            this.treeListView1.AllowColumnReorder = true;
            this.treeListView1.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeaderSKU,
            this.columnHeader2,
            this.columnHeader型号,
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
            // columnHeaderSKU
            // 
            this.columnHeaderSKU.Text = "SKU";
            this.columnHeaderSKU.Width = 102;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "属性";
            this.columnHeader2.Width = 109;
            // 
            // columnHeader型号
            // 
            this.columnHeader型号.Text = "型号";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "规格";
            this.columnHeader3.Width = 111;
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
            // newSumDataGridViewBOM
            // 
            this.newSumDataGridViewBOM.AllowUserToAddRows = false;
            this.newSumDataGridViewBOM.AllowUserToDeleteRows = false;
            this.newSumDataGridViewBOM.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridViewBOM.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.newSumDataGridViewBOM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newSumDataGridViewBOM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newSumDataGridViewBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newSumDataGridViewBOM.CustomRowNo = false;
            this.newSumDataGridViewBOM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridViewBOM.FieldNameList = null;
            this.newSumDataGridViewBOM.IsShowSumRow = false;
            this.newSumDataGridViewBOM.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridViewBOM.Name = "newSumDataGridViewBOM";
            this.newSumDataGridViewBOM.ReadOnly = true;
            this.newSumDataGridViewBOM.RowTemplate.Height = 23;
            this.newSumDataGridViewBOM.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newSumDataGridViewBOM.Size = new System.Drawing.Size(1118, 192);
            this.newSumDataGridViewBOM.SumColumns = null;
            this.newSumDataGridViewBOM.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridViewBOM.SumRowCellFormat = "N2";
            this.newSumDataGridViewBOM.TabIndex = 101;
            this.newSumDataGridViewBOM.UseCustomColumnDisplay = true;
            this.newSumDataGridViewBOM.UseSelectedColumn = false;
            this.newSumDataGridViewBOM.Use是否使用内置右键功能 = true;
            this.newSumDataGridViewBOM.XmlFileName = "";
            this.newSumDataGridViewBOM.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.KryptonDataGridViewBOM_CellPainting);
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
            // chkMultiBOMProd
            // 
            this.chkMultiBOMProd.Location = new System.Drawing.Point(712, 92);
            this.chkMultiBOMProd.Name = "chkMultiBOMProd";
            this.chkMultiBOMProd.Size = new System.Drawing.Size(19, 13);
            this.chkMultiBOMProd.TabIndex = 177;
            this.chkMultiBOMProd.Values.Text = "";
            // 
            // kryptonLabel7
            // 
            this.kryptonLabel7.Location = new System.Drawing.Point(629, 86);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(75, 20);
            this.kryptonLabel7.TabIndex = 176;
            this.kryptonLabel7.Values.Text = "多配方产品";
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
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewBOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroup)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsBoms)).EndInit();
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
        private System.Windows.Forms.TreeListView treeListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imageList1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private Krypton.Toolkit.KryptonTextBox txtProp;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private Krypton.Toolkit.KryptonLabel kryptonLabel6;
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
        private ColumnHeader columnHeaderSKU;
        private ColumnHeader columnHeader型号;
        private Krypton.Toolkit.KryptonTextBox txtBrand;
        private Krypton.Toolkit.KryptonLabel kryptonLabel13;
        private Krypton.Toolkit.KryptonComboBox cmbdepartment;
        private UControls.NewSumDataGridView newSumDataGridViewBOM;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem toolStripMenuItem1;
        private BindingSource bsBoms;
        private Krypton.Toolkit.KryptonCheckBox chkMultiBOMProd;
        private Krypton.Toolkit.KryptonLabel kryptonLabel7;
    }
}