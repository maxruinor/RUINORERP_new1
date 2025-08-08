// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:41
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 调拨明细统计
    /// </summary>
    partial class View_StockTransferItemsEdit
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
     this.lblStockTransferNo = new Krypton.Toolkit.KryptonLabel();
this.txtStockTransferNo = new Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID_from = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID_from = new Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID_to = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID_to = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblTransfer_date = new Krypton.Toolkit.KryptonLabel();
this.dtpTransfer_date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblProductNo = new Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new Krypton.Toolkit.KryptonTextBox();

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblCategory_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCategory_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.txtUnit_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblQty = new Krypton.Toolkit.KryptonLabel();
this.txtQty = new Krypton.Toolkit.KryptonTextBox();

this.lblTransPrice = new Krypton.Toolkit.KryptonLabel();
this.txtTransPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblCost = new Krypton.Toolkit.KryptonLabel();
this.txtCost = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblSubtotalTransferPirceAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTransferPirceAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####50StockTransferNo###String
this.lblStockTransferNo.AutoSize = true;
this.lblStockTransferNo.Location = new System.Drawing.Point(100,25);
this.lblStockTransferNo.Name = "lblStockTransferNo";
this.lblStockTransferNo.Size = new System.Drawing.Size(41, 12);
this.lblStockTransferNo.TabIndex = 1;
this.lblStockTransferNo.Text = "";
this.txtStockTransferNo.Location = new System.Drawing.Point(173,21);
this.txtStockTransferNo.Name = "txtStockTransferNo";
this.txtStockTransferNo.Size = new System.Drawing.Size(100, 21);
this.txtStockTransferNo.TabIndex = 1;
this.Controls.Add(this.lblStockTransferNo);
this.Controls.Add(this.txtStockTransferNo);

           //#####Location_ID_from###Int64
this.lblLocation_ID_from.AutoSize = true;
this.lblLocation_ID_from.Location = new System.Drawing.Point(100,50);
this.lblLocation_ID_from.Name = "lblLocation_ID_from";
this.lblLocation_ID_from.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID_from.TabIndex = 2;
this.lblLocation_ID_from.Text = "";
this.txtLocation_ID_from.Location = new System.Drawing.Point(173,46);
this.txtLocation_ID_from.Name = "txtLocation_ID_from";
this.txtLocation_ID_from.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID_from.TabIndex = 2;
this.Controls.Add(this.lblLocation_ID_from);
this.Controls.Add(this.txtLocation_ID_from);

           //#####Location_ID_to###Int64
this.lblLocation_ID_to.AutoSize = true;
this.lblLocation_ID_to.Location = new System.Drawing.Point(100,75);
this.lblLocation_ID_to.Name = "lblLocation_ID_to";
this.lblLocation_ID_to.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID_to.TabIndex = 3;
this.lblLocation_ID_to.Text = "";
this.txtLocation_ID_to.Location = new System.Drawing.Point(173,71);
this.txtLocation_ID_to.Name = "txtLocation_ID_to";
this.txtLocation_ID_to.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID_to.TabIndex = 3;
this.Controls.Add(this.lblLocation_ID_to);
this.Controls.Add(this.txtLocation_ID_to);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,100);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 4;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,96);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 4;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####Transfer_date###DateTime
this.lblTransfer_date.AutoSize = true;
this.lblTransfer_date.Location = new System.Drawing.Point(100,125);
this.lblTransfer_date.Name = "lblTransfer_date";
this.lblTransfer_date.Size = new System.Drawing.Size(41, 12);
this.lblTransfer_date.TabIndex = 5;
this.lblTransfer_date.Text = "";
//111======125
this.dtpTransfer_date.Location = new System.Drawing.Point(173,121);
this.dtpTransfer_date.Name ="dtpTransfer_date";
this.dtpTransfer_date.ShowCheckBox =true;
this.dtpTransfer_date.Size = new System.Drawing.Size(100, 21);
this.dtpTransfer_date.TabIndex = 5;
this.Controls.Add(this.lblTransfer_date);
this.Controls.Add(this.dtpTransfer_date);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,175);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 7;
this.lblCreated_by.Text = "";
this.txtCreated_by.Location = new System.Drawing.Point(173,171);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 7;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,200);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 8;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,196);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 8;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####DataStatus###Int32
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,225);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 9;
this.lblDataStatus.Text = "";
this.txtDataStatus.Location = new System.Drawing.Point(173,221);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 9;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,250);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 10;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,246);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 10;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,300);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 12;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,296);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 12;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,325);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 13;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,321);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 13;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,350);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 14;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,346);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 14;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,375);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 15;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,371);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 15;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,400);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 16;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,396);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 16;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,425);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 17;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,421);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 17;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64
this.lblCategory_ID.AutoSize = true;
this.lblCategory_ID.Location = new System.Drawing.Point(100,450);
this.lblCategory_ID.Name = "lblCategory_ID";
this.lblCategory_ID.Size = new System.Drawing.Size(41, 12);
this.lblCategory_ID.TabIndex = 18;
this.lblCategory_ID.Text = "";
this.txtCategory_ID.Location = new System.Drawing.Point(173,446);
this.txtCategory_ID.Name = "txtCategory_ID";
this.txtCategory_ID.Size = new System.Drawing.Size(100, 21);
this.txtCategory_ID.TabIndex = 18;
this.Controls.Add(this.lblCategory_ID);
this.Controls.Add(this.txtCategory_ID);

           //#####Unit_ID###Int64
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,475);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 19;
this.lblUnit_ID.Text = "";
this.txtUnit_ID.Location = new System.Drawing.Point(173,471);
this.txtUnit_ID.Name = "txtUnit_ID";
this.txtUnit_ID.Size = new System.Drawing.Size(100, 21);
this.txtUnit_ID.TabIndex = 19;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.txtUnit_ID);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,500);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 20;
this.lblProdDetailID.Text = "";
this.txtProdDetailID.Location = new System.Drawing.Point(173,496);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 20;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,525);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 21;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,521);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 21;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Qty###Int32
this.lblQty.AutoSize = true;
this.lblQty.Location = new System.Drawing.Point(100,550);
this.lblQty.Name = "lblQty";
this.lblQty.Size = new System.Drawing.Size(41, 12);
this.lblQty.TabIndex = 22;
this.lblQty.Text = "";
this.txtQty.Location = new System.Drawing.Point(173,546);
this.txtQty.Name = "txtQty";
this.txtQty.Size = new System.Drawing.Size(100, 21);
this.txtQty.TabIndex = 22;
this.Controls.Add(this.lblQty);
this.Controls.Add(this.txtQty);

           //#####TransPrice###Decimal
this.lblTransPrice.AutoSize = true;
this.lblTransPrice.Location = new System.Drawing.Point(100,575);
this.lblTransPrice.Name = "lblTransPrice";
this.lblTransPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransPrice.TabIndex = 23;
this.lblTransPrice.Text = "";
//111======575
this.txtTransPrice.Location = new System.Drawing.Point(173,571);
this.txtTransPrice.Name ="txtTransPrice";
this.txtTransPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransPrice.TabIndex = 23;
this.Controls.Add(this.lblTransPrice);
this.Controls.Add(this.txtTransPrice);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,600);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 24;
this.lblCost.Text = "";
//111======600
this.txtCost.Location = new System.Drawing.Point(173,596);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 24;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####500Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,625);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 25;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,621);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 25;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####SubtotalTransferPirceAmount###Decimal
this.lblSubtotalTransferPirceAmount.AutoSize = true;
this.lblSubtotalTransferPirceAmount.Location = new System.Drawing.Point(100,650);
this.lblSubtotalTransferPirceAmount.Name = "lblSubtotalTransferPirceAmount";
this.lblSubtotalTransferPirceAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTransferPirceAmount.TabIndex = 26;
this.lblSubtotalTransferPirceAmount.Text = "";
//111======650
this.txtSubtotalTransferPirceAmount.Location = new System.Drawing.Point(173,646);
this.txtSubtotalTransferPirceAmount.Name ="txtSubtotalTransferPirceAmount";
this.txtSubtotalTransferPirceAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTransferPirceAmount.TabIndex = 26;
this.Controls.Add(this.lblSubtotalTransferPirceAmount);
this.Controls.Add(this.txtSubtotalTransferPirceAmount);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,675);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 27;
this.lblSubtotalCostAmount.Text = "";
//111======675
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,671);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 27;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

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
           // this.kryptonPanel1.TabIndex = 27;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblStockTransferNo );
this.Controls.Add(this.txtStockTransferNo );

                this.Controls.Add(this.lblLocation_ID_from );
this.Controls.Add(this.txtLocation_ID_from );

                this.Controls.Add(this.lblLocation_ID_to );
this.Controls.Add(this.txtLocation_ID_to );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblTransfer_date );
this.Controls.Add(this.dtpTransfer_date );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblCategory_ID );
this.Controls.Add(this.txtCategory_ID );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.txtUnit_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblQty );
this.Controls.Add(this.txtQty );

                this.Controls.Add(this.lblTransPrice );
this.Controls.Add(this.txtTransPrice );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblSubtotalTransferPirceAmount );
this.Controls.Add(this.txtSubtotalTransferPirceAmount );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                            // 
            // "View_StockTransferItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_StockTransferItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblStockTransferNo;
private Krypton.Toolkit.KryptonTextBox txtStockTransferNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID_from;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID_from;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID_to;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID_to;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTransfer_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpTransfer_date;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductNo;
private Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblCategory_ID;
private Krypton.Toolkit.KryptonTextBox txtCategory_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonTextBox txtUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblQty;
private Krypton.Toolkit.KryptonTextBox txtQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblTransPrice;
private Krypton.Toolkit.KryptonTextBox txtTransPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblCost;
private Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalTransferPirceAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalTransferPirceAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

