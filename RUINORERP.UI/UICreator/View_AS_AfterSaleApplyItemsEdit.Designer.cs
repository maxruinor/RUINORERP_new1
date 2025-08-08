// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:26
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 售后申请明细统计
    /// </summary>
    partial class View_AS_AfterSaleApplyItemsEdit
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
     this.lblASApplyNo = new Krypton.Toolkit.KryptonLabel();
this.txtASApplyNo = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.txtProjectGroup_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblApplyDate = new Krypton.Toolkit.KryptonLabel();
this.dtpApplyDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerSourceNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerSourceNo = new Krypton.Toolkit.KryptonTextBox();

this.lblPriority = new Krypton.Toolkit.KryptonLabel();
this.txtPriority = new Krypton.Toolkit.KryptonTextBox();

this.lblASProcessStatus = new Krypton.Toolkit.KryptonLabel();
this.txtASProcessStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalConfirmedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtTotalConfirmedQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblRepairEvaluationOpinion = new Krypton.Toolkit.KryptonLabel();
this.txtRepairEvaluationOpinion = new Krypton.Toolkit.KryptonTextBox();
this.txtRepairEvaluationOpinion.Multiline = true;

this.lblExpenseAllocationMode = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseAllocationMode = new Krypton.Toolkit.KryptonTextBox();

this.lblExpenseBearerType = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseBearerType = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalDeliveredQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalDeliveredQty = new Krypton.Toolkit.KryptonTextBox();

this.lblFaultDescription = new Krypton.Toolkit.KryptonLabel();
this.txtFaultDescription = new Krypton.Toolkit.KryptonTextBox();
this.txtFaultDescription.Multiline = true;

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerPartNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new Krypton.Toolkit.KryptonTextBox();

this.lblConfirmedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtConfirmedQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblDeliveredQty = new Krypton.Toolkit.KryptonLabel();
this.txtDeliveredQty = new Krypton.Toolkit.KryptonTextBox();

this.lblInitialQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtInitialQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblProdBaseID = new Krypton.Toolkit.KryptonLabel();
this.txtProdBaseID = new Krypton.Toolkit.KryptonTextBox();

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.txtUnit_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblprop = new Krypton.Toolkit.KryptonLabel();
this.txtprop = new Krypton.Toolkit.KryptonTextBox();

this.lblProductNo = new Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new Krypton.Toolkit.KryptonTextBox();

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblCategory_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCategory_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
this.txtType_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    
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
     
            //#####50ASApplyNo###String
this.lblASApplyNo.AutoSize = true;
this.lblASApplyNo.Location = new System.Drawing.Point(100,25);
this.lblASApplyNo.Name = "lblASApplyNo";
this.lblASApplyNo.Size = new System.Drawing.Size(41, 12);
this.lblASApplyNo.TabIndex = 1;
this.lblASApplyNo.Text = "";
this.txtASApplyNo.Location = new System.Drawing.Point(173,21);
this.txtASApplyNo.Name = "txtASApplyNo";
this.txtASApplyNo.Size = new System.Drawing.Size(100, 21);
this.txtASApplyNo.TabIndex = 1;
this.Controls.Add(this.lblASApplyNo);
this.Controls.Add(this.txtASApplyNo);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,46);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 2;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####ProjectGroup_ID###Int64
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,75);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 3;
this.lblProjectGroup_ID.Text = "";
this.txtProjectGroup_ID.Location = new System.Drawing.Point(173,71);
this.txtProjectGroup_ID.Name = "txtProjectGroup_ID";
this.txtProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.txtProjectGroup_ID.TabIndex = 3;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.txtProjectGroup_ID);

           //#####CustomerVendor_ID###Int64
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,100);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 4;
this.lblCustomerVendor_ID.Text = "";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,96);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 4;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####ApplyDate###DateTime
this.lblApplyDate.AutoSize = true;
this.lblApplyDate.Location = new System.Drawing.Point(100,125);
this.lblApplyDate.Name = "lblApplyDate";
this.lblApplyDate.Size = new System.Drawing.Size(41, 12);
this.lblApplyDate.TabIndex = 5;
this.lblApplyDate.Text = "";
//111======125
this.dtpApplyDate.Location = new System.Drawing.Point(173,121);
this.dtpApplyDate.Name ="dtpApplyDate";
this.dtpApplyDate.ShowCheckBox =true;
this.dtpApplyDate.Size = new System.Drawing.Size(100, 21);
this.dtpApplyDate.TabIndex = 5;
this.Controls.Add(this.lblApplyDate);
this.Controls.Add(this.dtpApplyDate);

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

           //#####50CustomerSourceNo###String
this.lblCustomerSourceNo.AutoSize = true;
this.lblCustomerSourceNo.Location = new System.Drawing.Point(100,200);
this.lblCustomerSourceNo.Name = "lblCustomerSourceNo";
this.lblCustomerSourceNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerSourceNo.TabIndex = 8;
this.lblCustomerSourceNo.Text = "";
this.txtCustomerSourceNo.Location = new System.Drawing.Point(173,196);
this.txtCustomerSourceNo.Name = "txtCustomerSourceNo";
this.txtCustomerSourceNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerSourceNo.TabIndex = 8;
this.Controls.Add(this.lblCustomerSourceNo);
this.Controls.Add(this.txtCustomerSourceNo);

           //#####Priority###Int32
this.lblPriority.AutoSize = true;
this.lblPriority.Location = new System.Drawing.Point(100,225);
this.lblPriority.Name = "lblPriority";
this.lblPriority.Size = new System.Drawing.Size(41, 12);
this.lblPriority.TabIndex = 9;
this.lblPriority.Text = "";
this.txtPriority.Location = new System.Drawing.Point(173,221);
this.txtPriority.Name = "txtPriority";
this.txtPriority.Size = new System.Drawing.Size(100, 21);
this.txtPriority.TabIndex = 9;
this.Controls.Add(this.lblPriority);
this.Controls.Add(this.txtPriority);

           //#####ASProcessStatus###Int32
this.lblASProcessStatus.AutoSize = true;
this.lblASProcessStatus.Location = new System.Drawing.Point(100,250);
this.lblASProcessStatus.Name = "lblASProcessStatus";
this.lblASProcessStatus.Size = new System.Drawing.Size(41, 12);
this.lblASProcessStatus.TabIndex = 10;
this.lblASProcessStatus.Text = "";
this.txtASProcessStatus.Location = new System.Drawing.Point(173,246);
this.txtASProcessStatus.Name = "txtASProcessStatus";
this.txtASProcessStatus.Size = new System.Drawing.Size(100, 21);
this.txtASProcessStatus.TabIndex = 10;
this.Controls.Add(this.lblASProcessStatus);
this.Controls.Add(this.txtASProcessStatus);

           //#####TotalConfirmedQuantity###Int32
this.lblTotalConfirmedQuantity.AutoSize = true;
this.lblTotalConfirmedQuantity.Location = new System.Drawing.Point(100,275);
this.lblTotalConfirmedQuantity.Name = "lblTotalConfirmedQuantity";
this.lblTotalConfirmedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblTotalConfirmedQuantity.TabIndex = 11;
this.lblTotalConfirmedQuantity.Text = "";
this.txtTotalConfirmedQuantity.Location = new System.Drawing.Point(173,271);
this.txtTotalConfirmedQuantity.Name = "txtTotalConfirmedQuantity";
this.txtTotalConfirmedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtTotalConfirmedQuantity.TabIndex = 11;
this.Controls.Add(this.lblTotalConfirmedQuantity);
this.Controls.Add(this.txtTotalConfirmedQuantity);

           //#####500RepairEvaluationOpinion###String
this.lblRepairEvaluationOpinion.AutoSize = true;
this.lblRepairEvaluationOpinion.Location = new System.Drawing.Point(100,300);
this.lblRepairEvaluationOpinion.Name = "lblRepairEvaluationOpinion";
this.lblRepairEvaluationOpinion.Size = new System.Drawing.Size(41, 12);
this.lblRepairEvaluationOpinion.TabIndex = 12;
this.lblRepairEvaluationOpinion.Text = "";
this.txtRepairEvaluationOpinion.Location = new System.Drawing.Point(173,296);
this.txtRepairEvaluationOpinion.Name = "txtRepairEvaluationOpinion";
this.txtRepairEvaluationOpinion.Size = new System.Drawing.Size(100, 21);
this.txtRepairEvaluationOpinion.TabIndex = 12;
this.Controls.Add(this.lblRepairEvaluationOpinion);
this.Controls.Add(this.txtRepairEvaluationOpinion);

           //#####ExpenseAllocationMode###Int32
this.lblExpenseAllocationMode.AutoSize = true;
this.lblExpenseAllocationMode.Location = new System.Drawing.Point(100,325);
this.lblExpenseAllocationMode.Name = "lblExpenseAllocationMode";
this.lblExpenseAllocationMode.Size = new System.Drawing.Size(41, 12);
this.lblExpenseAllocationMode.TabIndex = 13;
this.lblExpenseAllocationMode.Text = "";
this.txtExpenseAllocationMode.Location = new System.Drawing.Point(173,321);
this.txtExpenseAllocationMode.Name = "txtExpenseAllocationMode";
this.txtExpenseAllocationMode.Size = new System.Drawing.Size(100, 21);
this.txtExpenseAllocationMode.TabIndex = 13;
this.Controls.Add(this.lblExpenseAllocationMode);
this.Controls.Add(this.txtExpenseAllocationMode);

           //#####ExpenseBearerType###Int32
this.lblExpenseBearerType.AutoSize = true;
this.lblExpenseBearerType.Location = new System.Drawing.Point(100,350);
this.lblExpenseBearerType.Name = "lblExpenseBearerType";
this.lblExpenseBearerType.Size = new System.Drawing.Size(41, 12);
this.lblExpenseBearerType.TabIndex = 14;
this.lblExpenseBearerType.Text = "";
this.txtExpenseBearerType.Location = new System.Drawing.Point(173,346);
this.txtExpenseBearerType.Name = "txtExpenseBearerType";
this.txtExpenseBearerType.Size = new System.Drawing.Size(100, 21);
this.txtExpenseBearerType.TabIndex = 14;
this.Controls.Add(this.lblExpenseBearerType);
this.Controls.Add(this.txtExpenseBearerType);

           //#####TotalDeliveredQty###Int32
this.lblTotalDeliveredQty.AutoSize = true;
this.lblTotalDeliveredQty.Location = new System.Drawing.Point(100,375);
this.lblTotalDeliveredQty.Name = "lblTotalDeliveredQty";
this.lblTotalDeliveredQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalDeliveredQty.TabIndex = 15;
this.lblTotalDeliveredQty.Text = "";
this.txtTotalDeliveredQty.Location = new System.Drawing.Point(173,371);
this.txtTotalDeliveredQty.Name = "txtTotalDeliveredQty";
this.txtTotalDeliveredQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalDeliveredQty.TabIndex = 15;
this.Controls.Add(this.lblTotalDeliveredQty);
this.Controls.Add(this.txtTotalDeliveredQty);

           //#####500FaultDescription###String
this.lblFaultDescription.AutoSize = true;
this.lblFaultDescription.Location = new System.Drawing.Point(100,400);
this.lblFaultDescription.Name = "lblFaultDescription";
this.lblFaultDescription.Size = new System.Drawing.Size(41, 12);
this.lblFaultDescription.TabIndex = 16;
this.lblFaultDescription.Text = "";
this.txtFaultDescription.Location = new System.Drawing.Point(173,396);
this.txtFaultDescription.Name = "txtFaultDescription";
this.txtFaultDescription.Size = new System.Drawing.Size(100, 21);
this.txtFaultDescription.TabIndex = 16;
this.Controls.Add(this.lblFaultDescription);
this.Controls.Add(this.txtFaultDescription);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,425);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 17;
this.lblProdDetailID.Text = "";
this.txtProdDetailID.Location = new System.Drawing.Point(173,421);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 17;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####Location_ID###Int64
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,450);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 18;
this.lblLocation_ID.Text = "";
this.txtLocation_ID.Location = new System.Drawing.Point(173,446);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 18;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####100CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,475);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 19;
this.lblCustomerPartNo.Text = "";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,471);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 19;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####ConfirmedQuantity###Int32
this.lblConfirmedQuantity.AutoSize = true;
this.lblConfirmedQuantity.Location = new System.Drawing.Point(100,500);
this.lblConfirmedQuantity.Name = "lblConfirmedQuantity";
this.lblConfirmedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblConfirmedQuantity.TabIndex = 20;
this.lblConfirmedQuantity.Text = "";
this.txtConfirmedQuantity.Location = new System.Drawing.Point(173,496);
this.txtConfirmedQuantity.Name = "txtConfirmedQuantity";
this.txtConfirmedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtConfirmedQuantity.TabIndex = 20;
this.Controls.Add(this.lblConfirmedQuantity);
this.Controls.Add(this.txtConfirmedQuantity);

           //#####DeliveredQty###Int32
this.lblDeliveredQty.AutoSize = true;
this.lblDeliveredQty.Location = new System.Drawing.Point(100,525);
this.lblDeliveredQty.Name = "lblDeliveredQty";
this.lblDeliveredQty.Size = new System.Drawing.Size(41, 12);
this.lblDeliveredQty.TabIndex = 21;
this.lblDeliveredQty.Text = "";
this.txtDeliveredQty.Location = new System.Drawing.Point(173,521);
this.txtDeliveredQty.Name = "txtDeliveredQty";
this.txtDeliveredQty.Size = new System.Drawing.Size(100, 21);
this.txtDeliveredQty.TabIndex = 21;
this.Controls.Add(this.lblDeliveredQty);
this.Controls.Add(this.txtDeliveredQty);

           //#####InitialQuantity###Int32
this.lblInitialQuantity.AutoSize = true;
this.lblInitialQuantity.Location = new System.Drawing.Point(100,550);
this.lblInitialQuantity.Name = "lblInitialQuantity";
this.lblInitialQuantity.Size = new System.Drawing.Size(41, 12);
this.lblInitialQuantity.TabIndex = 22;
this.lblInitialQuantity.Text = "";
this.txtInitialQuantity.Location = new System.Drawing.Point(173,546);
this.txtInitialQuantity.Name = "txtInitialQuantity";
this.txtInitialQuantity.Size = new System.Drawing.Size(100, 21);
this.txtInitialQuantity.TabIndex = 22;
this.Controls.Add(this.lblInitialQuantity);
this.Controls.Add(this.txtInitialQuantity);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,575);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 23;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,571);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 23;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,600);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 24;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,596);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 24;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####ProdBaseID###Int64
this.lblProdBaseID.AutoSize = true;
this.lblProdBaseID.Location = new System.Drawing.Point(100,625);
this.lblProdBaseID.Name = "lblProdBaseID";
this.lblProdBaseID.Size = new System.Drawing.Size(41, 12);
this.lblProdBaseID.TabIndex = 25;
this.lblProdBaseID.Text = "";
this.txtProdBaseID.Location = new System.Drawing.Point(173,621);
this.txtProdBaseID.Name = "txtProdBaseID";
this.txtProdBaseID.Size = new System.Drawing.Size(100, 21);
this.txtProdBaseID.TabIndex = 25;
this.Controls.Add(this.lblProdBaseID);
this.Controls.Add(this.txtProdBaseID);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,650);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 26;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,646);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 26;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,675);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 27;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,671);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 27;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####Unit_ID###Int64
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,700);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 28;
this.lblUnit_ID.Text = "";
this.txtUnit_ID.Location = new System.Drawing.Point(173,696);
this.txtUnit_ID.Name = "txtUnit_ID";
this.txtUnit_ID.Size = new System.Drawing.Size(100, 21);
this.txtUnit_ID.TabIndex = 28;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.txtUnit_ID);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,725);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 29;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,721);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 29;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####Quantity###Int32
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,750);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 30;
this.lblQuantity.Text = "";
this.txtQuantity.Location = new System.Drawing.Point(173,746);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 30;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####-1prop###String
this.lblprop.AutoSize = true;
this.lblprop.Location = new System.Drawing.Point(100,775);
this.lblprop.Name = "lblprop";
this.lblprop.Size = new System.Drawing.Size(41, 12);
this.lblprop.TabIndex = 31;
this.lblprop.Text = "";
this.txtprop.Location = new System.Drawing.Point(173,771);
this.txtprop.Name = "txtprop";
this.txtprop.Size = new System.Drawing.Size(100, 21);
this.txtprop.TabIndex = 31;
this.Controls.Add(this.lblprop);
this.Controls.Add(this.txtprop);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,800);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 32;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,796);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 32;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,825);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 33;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,821);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 33;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64
this.lblCategory_ID.AutoSize = true;
this.lblCategory_ID.Location = new System.Drawing.Point(100,850);
this.lblCategory_ID.Name = "lblCategory_ID";
this.lblCategory_ID.Size = new System.Drawing.Size(41, 12);
this.lblCategory_ID.TabIndex = 34;
this.lblCategory_ID.Text = "";
this.txtCategory_ID.Location = new System.Drawing.Point(173,846);
this.txtCategory_ID.Name = "txtCategory_ID";
this.txtCategory_ID.Size = new System.Drawing.Size(100, 21);
this.txtCategory_ID.TabIndex = 34;
this.Controls.Add(this.lblCategory_ID);
this.Controls.Add(this.txtCategory_ID);

           //#####Type_ID###Int64
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,875);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 35;
this.lblType_ID.Text = "";
this.txtType_ID.Location = new System.Drawing.Point(173,871);
this.txtType_ID.Name = "txtType_ID";
this.txtType_ID.Size = new System.Drawing.Size(100, 21);
this.txtType_ID.TabIndex = 35;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.txtType_ID);

           //#####DataStatus###Int32
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,900);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 36;
this.lblDataStatus.Text = "";
this.txtDataStatus.Location = new System.Drawing.Point(173,896);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 36;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,950);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 38;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,946);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 38;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,975);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 39;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,971);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 39;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,1000);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 40;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,996);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 40;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

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
           // this.kryptonPanel1.TabIndex = 40;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblASApplyNo );
this.Controls.Add(this.txtASApplyNo );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.txtProjectGroup_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblApplyDate );
this.Controls.Add(this.dtpApplyDate );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblCustomerSourceNo );
this.Controls.Add(this.txtCustomerSourceNo );

                this.Controls.Add(this.lblPriority );
this.Controls.Add(this.txtPriority );

                this.Controls.Add(this.lblASProcessStatus );
this.Controls.Add(this.txtASProcessStatus );

                this.Controls.Add(this.lblTotalConfirmedQuantity );
this.Controls.Add(this.txtTotalConfirmedQuantity );

                this.Controls.Add(this.lblRepairEvaluationOpinion );
this.Controls.Add(this.txtRepairEvaluationOpinion );

                this.Controls.Add(this.lblExpenseAllocationMode );
this.Controls.Add(this.txtExpenseAllocationMode );

                this.Controls.Add(this.lblExpenseBearerType );
this.Controls.Add(this.txtExpenseBearerType );

                this.Controls.Add(this.lblTotalDeliveredQty );
this.Controls.Add(this.txtTotalDeliveredQty );

                this.Controls.Add(this.lblFaultDescription );
this.Controls.Add(this.txtFaultDescription );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblConfirmedQuantity );
this.Controls.Add(this.txtConfirmedQuantity );

                this.Controls.Add(this.lblDeliveredQty );
this.Controls.Add(this.txtDeliveredQty );

                this.Controls.Add(this.lblInitialQuantity );
this.Controls.Add(this.txtInitialQuantity );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblProdBaseID );
this.Controls.Add(this.txtProdBaseID );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.txtUnit_ID );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblprop );
this.Controls.Add(this.txtprop );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblCategory_ID );
this.Controls.Add(this.txtCategory_ID );

                this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.txtType_ID );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "View_AS_AfterSaleApplyItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_AS_AfterSaleApplyItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblASApplyNo;
private Krypton.Toolkit.KryptonTextBox txtASApplyNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonTextBox txtProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblApplyDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpApplyDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerSourceNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerSourceNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblPriority;
private Krypton.Toolkit.KryptonTextBox txtPriority;

    
        
              private Krypton.Toolkit.KryptonLabel lblASProcessStatus;
private Krypton.Toolkit.KryptonTextBox txtASProcessStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalConfirmedQuantity;
private Krypton.Toolkit.KryptonTextBox txtTotalConfirmedQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblRepairEvaluationOpinion;
private Krypton.Toolkit.KryptonTextBox txtRepairEvaluationOpinion;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseAllocationMode;
private Krypton.Toolkit.KryptonTextBox txtExpenseAllocationMode;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseBearerType;
private Krypton.Toolkit.KryptonTextBox txtExpenseBearerType;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalDeliveredQty;
private Krypton.Toolkit.KryptonTextBox txtTotalDeliveredQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblFaultDescription;
private Krypton.Toolkit.KryptonTextBox txtFaultDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblConfirmedQuantity;
private Krypton.Toolkit.KryptonTextBox txtConfirmedQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeliveredQty;
private Krypton.Toolkit.KryptonTextBox txtDeliveredQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblInitialQuantity;
private Krypton.Toolkit.KryptonTextBox txtInitialQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdBaseID;
private Krypton.Toolkit.KryptonTextBox txtProdBaseID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonTextBox txtUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblprop;
private Krypton.Toolkit.KryptonTextBox txtprop;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductNo;
private Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblCategory_ID;
private Krypton.Toolkit.KryptonTextBox txtCategory_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblType_ID;
private Krypton.Toolkit.KryptonTextBox txtType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

