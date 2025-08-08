// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 库存表
    /// </summary>
    partial class tb_InventoryEdit
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
     this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblInitInventory = new Krypton.Toolkit.KryptonLabel();
this.txtInitInventory = new Krypton.Toolkit.KryptonTextBox();

this.lblAlert_Use = new Krypton.Toolkit.KryptonLabel();
this.txtAlert_Use = new Krypton.Toolkit.KryptonTextBox();

this.lblOn_the_way_Qty = new Krypton.Toolkit.KryptonLabel();
this.txtOn_the_way_Qty = new Krypton.Toolkit.KryptonTextBox();

this.lblSale_Qty = new Krypton.Toolkit.KryptonLabel();
this.txtSale_Qty = new Krypton.Toolkit.KryptonTextBox();

this.lblMakingQty = new Krypton.Toolkit.KryptonLabel();
this.txtMakingQty = new Krypton.Toolkit.KryptonTextBox();

this.lblNotOutQty = new Krypton.Toolkit.KryptonLabel();
this.txtNotOutQty = new Krypton.Toolkit.KryptonTextBox();

this.lblBatchNumber = new Krypton.Toolkit.KryptonLabel();
this.txtBatchNumber = new Krypton.Toolkit.KryptonTextBox();

this.lblAlert_Quantity = new Krypton.Toolkit.KryptonLabel();
this.txtAlert_Quantity = new Krypton.Toolkit.KryptonTextBox();

this.lblCostFIFO = new Krypton.Toolkit.KryptonLabel();
this.txtCostFIFO = new Krypton.Toolkit.KryptonTextBox();

this.lblCostMonthlyWA = new Krypton.Toolkit.KryptonLabel();
this.txtCostMonthlyWA = new Krypton.Toolkit.KryptonTextBox();

this.lblCostMovingWA = new Krypton.Toolkit.KryptonLabel();
this.txtCostMovingWA = new Krypton.Toolkit.KryptonTextBox();

this.lblInv_AdvCost = new Krypton.Toolkit.KryptonLabel();
this.txtInv_AdvCost = new Krypton.Toolkit.KryptonTextBox();

this.lblInv_Cost = new Krypton.Toolkit.KryptonLabel();
this.txtInv_Cost = new Krypton.Toolkit.KryptonTextBox();

this.lblInv_SubtotalCostMoney = new Krypton.Toolkit.KryptonLabel();
this.txtInv_SubtotalCostMoney = new Krypton.Toolkit.KryptonTextBox();

this.lblLatestOutboundTime = new Krypton.Toolkit.KryptonLabel();
this.dtpLatestOutboundTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblLatestStorageTime = new Krypton.Toolkit.KryptonLabel();
this.dtpLatestStorageTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblLastInventoryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpLastInventoryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

    
    //for end
   // ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
   // this.kryptonPanel1.SuspendLayout();
    this.SuspendLayout();
    
            // 
            // btnOk
            // 
            //this.btnOk.Location = new System.Drawing.Point(126, 355);
            //this.btnOk.Name = "btnOk";
            //this.btnOk.Size = new System.Drawing.Size(90, 25);
            //this.btnOk.TabIndex = 0;
           // this.btnOk.Values.Text = "确定";
            //this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
           // this.btnCancel.Location = new System.Drawing.Point(244, 355);
            //this.btnCancel.Name = "btnCancel";
            //this.btnCancel.Size = new System.Drawing.Size(90, 25);
            //this.btnCancel.TabIndex = 1;
            //this.btnCancel.Values.Text = "取消";
           // this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
         //for size
     
            //#####ProdDetailID###Int64
//属性测试25ProdDetailID
//属性测试25ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,25);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 1;
this.lblProdDetailID.Text = "货品详情";
//111======25
this.cmbProdDetailID.Location = new System.Drawing.Point(173,21);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 1;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####Rack_ID###Int64
//属性测试50Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,50);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 2;
this.lblRack_ID.Text = "货架";
//111======50
this.cmbRack_ID.Location = new System.Drawing.Point(173,46);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 2;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

           //#####Location_ID###Int64
//属性测试75Location_ID
//属性测试75Location_ID
//属性测试75Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,75);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 3;
this.lblLocation_ID.Text = "库位";
//111======75
this.cmbLocation_ID.Location = new System.Drawing.Point(173,71);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 3;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####Quantity###Int32
//属性测试100Quantity
//属性测试100Quantity
//属性测试100Quantity
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,100);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 4;
this.lblQuantity.Text = "实际库存";
this.txtQuantity.Location = new System.Drawing.Point(173,96);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 4;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####InitInventory###Int32
//属性测试125InitInventory
//属性测试125InitInventory
//属性测试125InitInventory
this.lblInitInventory.AutoSize = true;
this.lblInitInventory.Location = new System.Drawing.Point(100,125);
this.lblInitInventory.Name = "lblInitInventory";
this.lblInitInventory.Size = new System.Drawing.Size(41, 12);
this.lblInitInventory.TabIndex = 5;
this.lblInitInventory.Text = "期初数量";
this.txtInitInventory.Location = new System.Drawing.Point(173,121);
this.txtInitInventory.Name = "txtInitInventory";
this.txtInitInventory.Size = new System.Drawing.Size(100, 21);
this.txtInitInventory.TabIndex = 5;
this.Controls.Add(this.lblInitInventory);
this.Controls.Add(this.txtInitInventory);

           //#####Alert_Use###Int32
//属性测试150Alert_Use
//属性测试150Alert_Use
//属性测试150Alert_Use
this.lblAlert_Use.AutoSize = true;
this.lblAlert_Use.Location = new System.Drawing.Point(100,150);
this.lblAlert_Use.Name = "lblAlert_Use";
this.lblAlert_Use.Size = new System.Drawing.Size(41, 12);
this.lblAlert_Use.TabIndex = 6;
this.lblAlert_Use.Text = "使用预警";
this.txtAlert_Use.Location = new System.Drawing.Point(173,146);
this.txtAlert_Use.Name = "txtAlert_Use";
this.txtAlert_Use.Size = new System.Drawing.Size(100, 21);
this.txtAlert_Use.TabIndex = 6;
this.Controls.Add(this.lblAlert_Use);
this.Controls.Add(this.txtAlert_Use);

           //#####On_the_way_Qty###Int32
//属性测试175On_the_way_Qty
//属性测试175On_the_way_Qty
//属性测试175On_the_way_Qty
this.lblOn_the_way_Qty.AutoSize = true;
this.lblOn_the_way_Qty.Location = new System.Drawing.Point(100,175);
this.lblOn_the_way_Qty.Name = "lblOn_the_way_Qty";
this.lblOn_the_way_Qty.Size = new System.Drawing.Size(41, 12);
this.lblOn_the_way_Qty.TabIndex = 7;
this.lblOn_the_way_Qty.Text = "在途库存";
this.txtOn_the_way_Qty.Location = new System.Drawing.Point(173,171);
this.txtOn_the_way_Qty.Name = "txtOn_the_way_Qty";
this.txtOn_the_way_Qty.Size = new System.Drawing.Size(100, 21);
this.txtOn_the_way_Qty.TabIndex = 7;
this.Controls.Add(this.lblOn_the_way_Qty);
this.Controls.Add(this.txtOn_the_way_Qty);

           //#####Sale_Qty###Int32
//属性测试200Sale_Qty
//属性测试200Sale_Qty
//属性测试200Sale_Qty
this.lblSale_Qty.AutoSize = true;
this.lblSale_Qty.Location = new System.Drawing.Point(100,200);
this.lblSale_Qty.Name = "lblSale_Qty";
this.lblSale_Qty.Size = new System.Drawing.Size(41, 12);
this.lblSale_Qty.TabIndex = 8;
this.lblSale_Qty.Text = "拟销售量";
this.txtSale_Qty.Location = new System.Drawing.Point(173,196);
this.txtSale_Qty.Name = "txtSale_Qty";
this.txtSale_Qty.Size = new System.Drawing.Size(100, 21);
this.txtSale_Qty.TabIndex = 8;
this.Controls.Add(this.lblSale_Qty);
this.Controls.Add(this.txtSale_Qty);

           //#####MakingQty###Int32
//属性测试225MakingQty
//属性测试225MakingQty
//属性测试225MakingQty
this.lblMakingQty.AutoSize = true;
this.lblMakingQty.Location = new System.Drawing.Point(100,225);
this.lblMakingQty.Name = "lblMakingQty";
this.lblMakingQty.Size = new System.Drawing.Size(41, 12);
this.lblMakingQty.TabIndex = 9;
this.lblMakingQty.Text = "在制数量";
this.txtMakingQty.Location = new System.Drawing.Point(173,221);
this.txtMakingQty.Name = "txtMakingQty";
this.txtMakingQty.Size = new System.Drawing.Size(100, 21);
this.txtMakingQty.TabIndex = 9;
this.Controls.Add(this.lblMakingQty);
this.Controls.Add(this.txtMakingQty);

           //#####NotOutQty###Int32
//属性测试250NotOutQty
//属性测试250NotOutQty
//属性测试250NotOutQty
this.lblNotOutQty.AutoSize = true;
this.lblNotOutQty.Location = new System.Drawing.Point(100,250);
this.lblNotOutQty.Name = "lblNotOutQty";
this.lblNotOutQty.Size = new System.Drawing.Size(41, 12);
this.lblNotOutQty.TabIndex = 10;
this.lblNotOutQty.Text = "未发数量";
this.txtNotOutQty.Location = new System.Drawing.Point(173,246);
this.txtNotOutQty.Name = "txtNotOutQty";
this.txtNotOutQty.Size = new System.Drawing.Size(100, 21);
this.txtNotOutQty.TabIndex = 10;
this.Controls.Add(this.lblNotOutQty);
this.Controls.Add(this.txtNotOutQty);

           //#####BatchNumber###Int32
//属性测试275BatchNumber
//属性测试275BatchNumber
//属性测试275BatchNumber
this.lblBatchNumber.AutoSize = true;
this.lblBatchNumber.Location = new System.Drawing.Point(100,275);
this.lblBatchNumber.Name = "lblBatchNumber";
this.lblBatchNumber.Size = new System.Drawing.Size(41, 12);
this.lblBatchNumber.TabIndex = 11;
this.lblBatchNumber.Text = "批次管理";
this.txtBatchNumber.Location = new System.Drawing.Point(173,271);
this.txtBatchNumber.Name = "txtBatchNumber";
this.txtBatchNumber.Size = new System.Drawing.Size(100, 21);
this.txtBatchNumber.TabIndex = 11;
this.Controls.Add(this.lblBatchNumber);
this.Controls.Add(this.txtBatchNumber);

           //#####Alert_Quantity###Int32
//属性测试300Alert_Quantity
//属性测试300Alert_Quantity
//属性测试300Alert_Quantity
this.lblAlert_Quantity.AutoSize = true;
this.lblAlert_Quantity.Location = new System.Drawing.Point(100,300);
this.lblAlert_Quantity.Name = "lblAlert_Quantity";
this.lblAlert_Quantity.Size = new System.Drawing.Size(41, 12);
this.lblAlert_Quantity.TabIndex = 12;
this.lblAlert_Quantity.Text = "预警值";
this.txtAlert_Quantity.Location = new System.Drawing.Point(173,296);
this.txtAlert_Quantity.Name = "txtAlert_Quantity";
this.txtAlert_Quantity.Size = new System.Drawing.Size(100, 21);
this.txtAlert_Quantity.TabIndex = 12;
this.Controls.Add(this.lblAlert_Quantity);
this.Controls.Add(this.txtAlert_Quantity);

           //#####CostFIFO###Decimal
this.lblCostFIFO.AutoSize = true;
this.lblCostFIFO.Location = new System.Drawing.Point(100,325);
this.lblCostFIFO.Name = "lblCostFIFO";
this.lblCostFIFO.Size = new System.Drawing.Size(41, 12);
this.lblCostFIFO.TabIndex = 13;
this.lblCostFIFO.Text = "先进先出成本";
//111======325
this.txtCostFIFO.Location = new System.Drawing.Point(173,321);
this.txtCostFIFO.Name ="txtCostFIFO";
this.txtCostFIFO.Size = new System.Drawing.Size(100, 21);
this.txtCostFIFO.TabIndex = 13;
this.Controls.Add(this.lblCostFIFO);
this.Controls.Add(this.txtCostFIFO);

           //#####CostMonthlyWA###Decimal
this.lblCostMonthlyWA.AutoSize = true;
this.lblCostMonthlyWA.Location = new System.Drawing.Point(100,350);
this.lblCostMonthlyWA.Name = "lblCostMonthlyWA";
this.lblCostMonthlyWA.Size = new System.Drawing.Size(41, 12);
this.lblCostMonthlyWA.TabIndex = 14;
this.lblCostMonthlyWA.Text = "月加权平均成本";
//111======350
this.txtCostMonthlyWA.Location = new System.Drawing.Point(173,346);
this.txtCostMonthlyWA.Name ="txtCostMonthlyWA";
this.txtCostMonthlyWA.Size = new System.Drawing.Size(100, 21);
this.txtCostMonthlyWA.TabIndex = 14;
this.Controls.Add(this.lblCostMonthlyWA);
this.Controls.Add(this.txtCostMonthlyWA);

           //#####CostMovingWA###Decimal
this.lblCostMovingWA.AutoSize = true;
this.lblCostMovingWA.Location = new System.Drawing.Point(100,375);
this.lblCostMovingWA.Name = "lblCostMovingWA";
this.lblCostMovingWA.Size = new System.Drawing.Size(41, 12);
this.lblCostMovingWA.TabIndex = 15;
this.lblCostMovingWA.Text = "移动加权平均成本";
//111======375
this.txtCostMovingWA.Location = new System.Drawing.Point(173,371);
this.txtCostMovingWA.Name ="txtCostMovingWA";
this.txtCostMovingWA.Size = new System.Drawing.Size(100, 21);
this.txtCostMovingWA.TabIndex = 15;
this.Controls.Add(this.lblCostMovingWA);
this.Controls.Add(this.txtCostMovingWA);

           //#####Inv_AdvCost###Decimal
this.lblInv_AdvCost.AutoSize = true;
this.lblInv_AdvCost.Location = new System.Drawing.Point(100,400);
this.lblInv_AdvCost.Name = "lblInv_AdvCost";
this.lblInv_AdvCost.Size = new System.Drawing.Size(41, 12);
this.lblInv_AdvCost.TabIndex = 16;
this.lblInv_AdvCost.Text = "实际成本";
//111======400
this.txtInv_AdvCost.Location = new System.Drawing.Point(173,396);
this.txtInv_AdvCost.Name ="txtInv_AdvCost";
this.txtInv_AdvCost.Size = new System.Drawing.Size(100, 21);
this.txtInv_AdvCost.TabIndex = 16;
this.Controls.Add(this.lblInv_AdvCost);
this.Controls.Add(this.txtInv_AdvCost);

           //#####Inv_Cost###Decimal
this.lblInv_Cost.AutoSize = true;
this.lblInv_Cost.Location = new System.Drawing.Point(100,425);
this.lblInv_Cost.Name = "lblInv_Cost";
this.lblInv_Cost.Size = new System.Drawing.Size(41, 12);
this.lblInv_Cost.TabIndex = 17;
this.lblInv_Cost.Text = "货品成本";
//111======425
this.txtInv_Cost.Location = new System.Drawing.Point(173,421);
this.txtInv_Cost.Name ="txtInv_Cost";
this.txtInv_Cost.Size = new System.Drawing.Size(100, 21);
this.txtInv_Cost.TabIndex = 17;
this.Controls.Add(this.lblInv_Cost);
this.Controls.Add(this.txtInv_Cost);

           //#####Inv_SubtotalCostMoney###Decimal
this.lblInv_SubtotalCostMoney.AutoSize = true;
this.lblInv_SubtotalCostMoney.Location = new System.Drawing.Point(100,450);
this.lblInv_SubtotalCostMoney.Name = "lblInv_SubtotalCostMoney";
this.lblInv_SubtotalCostMoney.Size = new System.Drawing.Size(41, 12);
this.lblInv_SubtotalCostMoney.TabIndex = 18;
this.lblInv_SubtotalCostMoney.Text = "成本小计";
//111======450
this.txtInv_SubtotalCostMoney.Location = new System.Drawing.Point(173,446);
this.txtInv_SubtotalCostMoney.Name ="txtInv_SubtotalCostMoney";
this.txtInv_SubtotalCostMoney.Size = new System.Drawing.Size(100, 21);
this.txtInv_SubtotalCostMoney.TabIndex = 18;
this.Controls.Add(this.lblInv_SubtotalCostMoney);
this.Controls.Add(this.txtInv_SubtotalCostMoney);

           //#####LatestOutboundTime###DateTime
this.lblLatestOutboundTime.AutoSize = true;
this.lblLatestOutboundTime.Location = new System.Drawing.Point(100,475);
this.lblLatestOutboundTime.Name = "lblLatestOutboundTime";
this.lblLatestOutboundTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestOutboundTime.TabIndex = 19;
this.lblLatestOutboundTime.Text = "最新出库时间";
//111======475
this.dtpLatestOutboundTime.Location = new System.Drawing.Point(173,471);
this.dtpLatestOutboundTime.Name ="dtpLatestOutboundTime";
this.dtpLatestOutboundTime.ShowCheckBox =true;
this.dtpLatestOutboundTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestOutboundTime.TabIndex = 19;
this.Controls.Add(this.lblLatestOutboundTime);
this.Controls.Add(this.dtpLatestOutboundTime);

           //#####LatestStorageTime###DateTime
this.lblLatestStorageTime.AutoSize = true;
this.lblLatestStorageTime.Location = new System.Drawing.Point(100,500);
this.lblLatestStorageTime.Name = "lblLatestStorageTime";
this.lblLatestStorageTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestStorageTime.TabIndex = 20;
this.lblLatestStorageTime.Text = "最新入库时间";
//111======500
this.dtpLatestStorageTime.Location = new System.Drawing.Point(173,496);
this.dtpLatestStorageTime.Name ="dtpLatestStorageTime";
this.dtpLatestStorageTime.ShowCheckBox =true;
this.dtpLatestStorageTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestStorageTime.TabIndex = 20;
this.Controls.Add(this.lblLatestStorageTime);
this.Controls.Add(this.dtpLatestStorageTime);

           //#####LastInventoryDate###DateTime
this.lblLastInventoryDate.AutoSize = true;
this.lblLastInventoryDate.Location = new System.Drawing.Point(100,525);
this.lblLastInventoryDate.Name = "lblLastInventoryDate";
this.lblLastInventoryDate.Size = new System.Drawing.Size(41, 12);
this.lblLastInventoryDate.TabIndex = 21;
this.lblLastInventoryDate.Text = "最后盘点时间";
//111======525
this.dtpLastInventoryDate.Location = new System.Drawing.Point(173,521);
this.dtpLastInventoryDate.Name ="dtpLastInventoryDate";
this.dtpLastInventoryDate.ShowCheckBox =true;
this.dtpLastInventoryDate.Size = new System.Drawing.Size(100, 21);
this.dtpLastInventoryDate.TabIndex = 21;
this.Controls.Add(this.lblLastInventoryDate);
this.Controls.Add(this.dtpLastInventoryDate);

           //#####250Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,550);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 22;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,546);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 22;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,575);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 23;
this.lblCreated_at.Text = "创建时间";
//111======575
this.dtpCreated_at.Location = new System.Drawing.Point(173,571);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 23;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试600Created_by
//属性测试600Created_by
//属性测试600Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,600);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 24;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,596);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 24;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,625);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 25;
this.lblModified_at.Text = "修改时间";
//111======625
this.dtpModified_at.Location = new System.Drawing.Point(173,621);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 25;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试650Modified_by
//属性测试650Modified_by
//属性测试650Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,650);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 26;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,646);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 26;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

        //for 加入到容器
            //components = new System.ComponentModel.Container();
           
            //this.Controls.Add(this.btnCancel);
            //this.Controls.Add(this.btnOk);
            // 
            // kryptonPanel1
            // 
          //  this.kryptonPanel1.Controls.Add(this.btnCancel);
         //   this.kryptonPanel1.Controls.Add(this.btnOk);
           // this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
           // this.kryptonPanel1.Name = "kryptonPanel1";
           // this.kryptonPanel1.Size = new System.Drawing.Size(404, 300);
           // this.kryptonPanel1.TabIndex = 26;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblInitInventory );
this.Controls.Add(this.txtInitInventory );

                this.Controls.Add(this.lblAlert_Use );
this.Controls.Add(this.txtAlert_Use );

                this.Controls.Add(this.lblOn_the_way_Qty );
this.Controls.Add(this.txtOn_the_way_Qty );

                this.Controls.Add(this.lblSale_Qty );
this.Controls.Add(this.txtSale_Qty );

                this.Controls.Add(this.lblMakingQty );
this.Controls.Add(this.txtMakingQty );

                this.Controls.Add(this.lblNotOutQty );
this.Controls.Add(this.txtNotOutQty );

                this.Controls.Add(this.lblBatchNumber );
this.Controls.Add(this.txtBatchNumber );

                this.Controls.Add(this.lblAlert_Quantity );
this.Controls.Add(this.txtAlert_Quantity );

                this.Controls.Add(this.lblCostFIFO );
this.Controls.Add(this.txtCostFIFO );

                this.Controls.Add(this.lblCostMonthlyWA );
this.Controls.Add(this.txtCostMonthlyWA );

                this.Controls.Add(this.lblCostMovingWA );
this.Controls.Add(this.txtCostMovingWA );

                this.Controls.Add(this.lblInv_AdvCost );
this.Controls.Add(this.txtInv_AdvCost );

                this.Controls.Add(this.lblInv_Cost );
this.Controls.Add(this.txtInv_Cost );

                this.Controls.Add(this.lblInv_SubtotalCostMoney );
this.Controls.Add(this.txtInv_SubtotalCostMoney );

                this.Controls.Add(this.lblLatestOutboundTime );
this.Controls.Add(this.dtpLatestOutboundTime );

                this.Controls.Add(this.lblLatestStorageTime );
this.Controls.Add(this.dtpLatestStorageTime );

                this.Controls.Add(this.lblLastInventoryDate );
this.Controls.Add(this.dtpLastInventoryDate );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_InventoryEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_InventoryEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRack_ID;
private Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblInitInventory;
private Krypton.Toolkit.KryptonTextBox txtInitInventory;

    
        
              private Krypton.Toolkit.KryptonLabel lblAlert_Use;
private Krypton.Toolkit.KryptonTextBox txtAlert_Use;

    
        
              private Krypton.Toolkit.KryptonLabel lblOn_the_way_Qty;
private Krypton.Toolkit.KryptonTextBox txtOn_the_way_Qty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSale_Qty;
private Krypton.Toolkit.KryptonTextBox txtSale_Qty;

    
        
              private Krypton.Toolkit.KryptonLabel lblMakingQty;
private Krypton.Toolkit.KryptonTextBox txtMakingQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotOutQty;
private Krypton.Toolkit.KryptonTextBox txtNotOutQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblBatchNumber;
private Krypton.Toolkit.KryptonTextBox txtBatchNumber;

    
        
              private Krypton.Toolkit.KryptonLabel lblAlert_Quantity;
private Krypton.Toolkit.KryptonTextBox txtAlert_Quantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblCostFIFO;
private Krypton.Toolkit.KryptonTextBox txtCostFIFO;

    
        
              private Krypton.Toolkit.KryptonLabel lblCostMonthlyWA;
private Krypton.Toolkit.KryptonTextBox txtCostMonthlyWA;

    
        
              private Krypton.Toolkit.KryptonLabel lblCostMovingWA;
private Krypton.Toolkit.KryptonTextBox txtCostMovingWA;

    
        
              private Krypton.Toolkit.KryptonLabel lblInv_AdvCost;
private Krypton.Toolkit.KryptonTextBox txtInv_AdvCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblInv_Cost;
private Krypton.Toolkit.KryptonTextBox txtInv_Cost;

    
        
              private Krypton.Toolkit.KryptonLabel lblInv_SubtotalCostMoney;
private Krypton.Toolkit.KryptonTextBox txtInv_SubtotalCostMoney;

    
        
              private Krypton.Toolkit.KryptonLabel lblLatestOutboundTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpLatestOutboundTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblLatestStorageTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpLatestStorageTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblLastInventoryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpLastInventoryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

