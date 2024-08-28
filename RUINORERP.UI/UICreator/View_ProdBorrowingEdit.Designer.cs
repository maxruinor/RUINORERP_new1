// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/27/2024 19:22:35
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 借出单统计
    /// </summary>
    partial class View_ProdBorrowingEdit
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
     this.lblBorrowID = new Krypton.Toolkit.KryptonLabel();
this.txtBorrowID = new Krypton.Toolkit.KryptonTextBox();

this.lblBorrowNo = new Krypton.Toolkit.KryptonLabel();
this.txtBorrowNo = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblOut_date = new Krypton.Toolkit.KryptonLabel();
this.dtpOut_date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblDueDate = new Krypton.Toolkit.KryptonLabel();
this.dtpDueDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblReason = new Krypton.Toolkit.KryptonLabel();
this.txtReason = new Krypton.Toolkit.KryptonTextBox();
this.txtReason.Multiline = true;

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblCloseCaseOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
this.txtType_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblQty = new Krypton.Toolkit.KryptonLabel();
this.txtQty = new Krypton.Toolkit.KryptonTextBox();

this.lblReQty = new Krypton.Toolkit.KryptonLabel();
this.txtReQty = new Krypton.Toolkit.KryptonTextBox();

this.lblPrice = new Krypton.Toolkit.KryptonLabel();
this.txtPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalPirceAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalPirceAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCost = new Krypton.Toolkit.KryptonLabel();
this.txtCost = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    
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
     
            //#####BorrowID###Int64
this.lblBorrowID.AutoSize = true;
this.lblBorrowID.Location = new System.Drawing.Point(100,25);
this.lblBorrowID.Name = "lblBorrowID";
this.lblBorrowID.Size = new System.Drawing.Size(41, 12);
this.lblBorrowID.TabIndex = 1;
this.lblBorrowID.Text = "BorrowID";
this.txtBorrowID.Location = new System.Drawing.Point(173,21);
this.txtBorrowID.Name = "txtBorrowID";
this.txtBorrowID.Size = new System.Drawing.Size(100, 21);
this.txtBorrowID.TabIndex = 1;
this.Controls.Add(this.lblBorrowID);
this.Controls.Add(this.txtBorrowID);

           //#####50BorrowNo###String
this.lblBorrowNo.AutoSize = true;
this.lblBorrowNo.Location = new System.Drawing.Point(100,50);
this.lblBorrowNo.Name = "lblBorrowNo";
this.lblBorrowNo.Size = new System.Drawing.Size(41, 12);
this.lblBorrowNo.TabIndex = 2;
this.lblBorrowNo.Text = "借出单号";
this.txtBorrowNo.Location = new System.Drawing.Point(173,46);
this.txtBorrowNo.Name = "txtBorrowNo";
this.txtBorrowNo.Size = new System.Drawing.Size(100, 21);
this.txtBorrowNo.TabIndex = 2;
this.Controls.Add(this.lblBorrowNo);
this.Controls.Add(this.txtBorrowNo);

           //#####CustomerVendor_ID###Int64
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,75);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 3;
this.lblCustomerVendor_ID.Text = "接收单位";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,71);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 3;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,100);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 4;
this.lblEmployee_ID.Text = "借出人";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,96);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 4;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####Out_date###DateTime
this.lblOut_date.AutoSize = true;
this.lblOut_date.Location = new System.Drawing.Point(100,125);
this.lblOut_date.Name = "lblOut_date";
this.lblOut_date.Size = new System.Drawing.Size(41, 12);
this.lblOut_date.TabIndex = 5;
this.lblOut_date.Text = "出库日期";
//111======125
this.dtpOut_date.Location = new System.Drawing.Point(173,121);
this.dtpOut_date.Name ="dtpOut_date";
this.dtpOut_date.ShowCheckBox =true;
this.dtpOut_date.Size = new System.Drawing.Size(100, 21);
this.dtpOut_date.TabIndex = 5;
this.Controls.Add(this.lblOut_date);
this.Controls.Add(this.dtpOut_date);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,150);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 6;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,146);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 6;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####DueDate###DateTime
this.lblDueDate.AutoSize = true;
this.lblDueDate.Location = new System.Drawing.Point(100,175);
this.lblDueDate.Name = "lblDueDate";
this.lblDueDate.Size = new System.Drawing.Size(41, 12);
this.lblDueDate.TabIndex = 7;
this.lblDueDate.Text = "到期日期";
//111======175
this.dtpDueDate.Location = new System.Drawing.Point(173,171);
this.dtpDueDate.Name ="dtpDueDate";
this.dtpDueDate.ShowCheckBox =true;
this.dtpDueDate.Size = new System.Drawing.Size(100, 21);
this.dtpDueDate.TabIndex = 7;
this.Controls.Add(this.lblDueDate);
this.Controls.Add(this.dtpDueDate);

           //#####500Reason###String
this.lblReason.AutoSize = true;
this.lblReason.Location = new System.Drawing.Point(100,200);
this.lblReason.Name = "lblReason";
this.lblReason.Size = new System.Drawing.Size(41, 12);
this.lblReason.TabIndex = 8;
this.lblReason.Text = "借出原因";
this.txtReason.Location = new System.Drawing.Point(173,196);
this.txtReason.Name = "txtReason";
this.txtReason.Size = new System.Drawing.Size(100, 21);
this.txtReason.TabIndex = 8;
this.Controls.Add(this.lblReason);
this.Controls.Add(this.txtReason);

           //#####DataStatus###Int32
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,225);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 9;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,221);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 9;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,250);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 10;
this.lblCloseCaseOpinions.Text = "结案意见";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,246);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 10;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,275);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 11;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,271);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 11;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,300);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 12;
this.lblProdDetailID.Text = "产品";
this.txtProdDetailID.Location = new System.Drawing.Point(173,296);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 12;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,325);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 13;
this.lblSKU.Text = "SKU码";
this.txtSKU.Location = new System.Drawing.Point(173,321);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 13;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,350);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 14;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,346);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 14;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,375);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 15;
this.lblCNName.Text = "品名";
this.txtCNName.Location = new System.Drawing.Point(173,371);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 15;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,400);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 16;
this.lblModel.Text = "型号";
this.txtModel.Location = new System.Drawing.Point(173,396);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 16;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Type_ID###Int64
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,425);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 17;
this.lblType_ID.Text = "产品类型";
this.txtType_ID.Location = new System.Drawing.Point(173,421);
this.txtType_ID.Name = "txtType_ID";
this.txtType_ID.Size = new System.Drawing.Size(100, 21);
this.txtType_ID.TabIndex = 17;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.txtType_ID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,450);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 18;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,446);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 18;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,475);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 19;
this.lblLocation_ID.Text = "库位";
this.txtLocation_ID.Location = new System.Drawing.Point(173,471);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 19;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####Qty###Int32
this.lblQty.AutoSize = true;
this.lblQty.Location = new System.Drawing.Point(100,500);
this.lblQty.Name = "lblQty";
this.lblQty.Size = new System.Drawing.Size(41, 12);
this.lblQty.TabIndex = 20;
this.lblQty.Text = "借出数量";
this.txtQty.Location = new System.Drawing.Point(173,496);
this.txtQty.Name = "txtQty";
this.txtQty.Size = new System.Drawing.Size(100, 21);
this.txtQty.TabIndex = 20;
this.Controls.Add(this.lblQty);
this.Controls.Add(this.txtQty);

           //#####ReQty###Int32
this.lblReQty.AutoSize = true;
this.lblReQty.Location = new System.Drawing.Point(100,525);
this.lblReQty.Name = "lblReQty";
this.lblReQty.Size = new System.Drawing.Size(41, 12);
this.lblReQty.TabIndex = 21;
this.lblReQty.Text = "归还数量";
this.txtReQty.Location = new System.Drawing.Point(173,521);
this.txtReQty.Name = "txtReQty";
this.txtReQty.Size = new System.Drawing.Size(100, 21);
this.txtReQty.TabIndex = 21;
this.Controls.Add(this.lblReQty);
this.Controls.Add(this.txtReQty);

           //#####Price###Decimal
this.lblPrice.AutoSize = true;
this.lblPrice.Location = new System.Drawing.Point(100,550);
this.lblPrice.Name = "lblPrice";
this.lblPrice.Size = new System.Drawing.Size(41, 12);
this.lblPrice.TabIndex = 22;
this.lblPrice.Text = "售价";
//111======550
this.txtPrice.Location = new System.Drawing.Point(173,546);
this.txtPrice.Name ="txtPrice";
this.txtPrice.Size = new System.Drawing.Size(100, 21);
this.txtPrice.TabIndex = 22;
this.Controls.Add(this.lblPrice);
this.Controls.Add(this.txtPrice);

           //#####SubtotalPirceAmount###Decimal
this.lblSubtotalPirceAmount.AutoSize = true;
this.lblSubtotalPirceAmount.Location = new System.Drawing.Point(100,575);
this.lblSubtotalPirceAmount.Name = "lblSubtotalPirceAmount";
this.lblSubtotalPirceAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalPirceAmount.TabIndex = 23;
this.lblSubtotalPirceAmount.Text = "金额小计";
//111======575
this.txtSubtotalPirceAmount.Location = new System.Drawing.Point(173,571);
this.txtSubtotalPirceAmount.Name ="txtSubtotalPirceAmount";
this.txtSubtotalPirceAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalPirceAmount.TabIndex = 23;
this.Controls.Add(this.lblSubtotalPirceAmount);
this.Controls.Add(this.txtSubtotalPirceAmount);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,600);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 24;
this.lblCost.Text = "成本";
//111======600
this.txtCost.Location = new System.Drawing.Point(173,596);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 24;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,625);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 25;
this.lblSubtotalCostAmount.Text = "成本小计";
//111======625
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,621);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 25;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####500Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,650);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 26;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,646);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 26;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

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
                this.Controls.Add(this.lblBorrowID );
this.Controls.Add(this.txtBorrowID );

                this.Controls.Add(this.lblBorrowNo );
this.Controls.Add(this.txtBorrowNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblOut_date );
this.Controls.Add(this.dtpOut_date );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblDueDate );
this.Controls.Add(this.dtpDueDate );

                this.Controls.Add(this.lblReason );
this.Controls.Add(this.txtReason );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.txtType_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblQty );
this.Controls.Add(this.txtQty );

                this.Controls.Add(this.lblReQty );
this.Controls.Add(this.txtReQty );

                this.Controls.Add(this.lblPrice );
this.Controls.Add(this.txtPrice );

                this.Controls.Add(this.lblSubtotalPirceAmount );
this.Controls.Add(this.txtSubtotalPirceAmount );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "View_ProdBorrowingEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_ProdBorrowingEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblBorrowID;
private Krypton.Toolkit.KryptonTextBox txtBorrowID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBorrowNo;
private Krypton.Toolkit.KryptonTextBox txtBorrowNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblOut_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpOut_date;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblDueDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpDueDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblReason;
private Krypton.Toolkit.KryptonTextBox txtReason;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblType_ID;
private Krypton.Toolkit.KryptonTextBox txtType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblQty;
private Krypton.Toolkit.KryptonTextBox txtQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblReQty;
private Krypton.Toolkit.KryptonTextBox txtReQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrice;
private Krypton.Toolkit.KryptonTextBox txtPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalPirceAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalPirceAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCost;
private Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

