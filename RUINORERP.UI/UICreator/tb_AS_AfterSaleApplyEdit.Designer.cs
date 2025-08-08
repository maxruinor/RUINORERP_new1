// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:05
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 售后申请单 -登记，评估，清单，确认。目标是维修翻新
    /// </summary>
    partial class tb_AS_AfterSaleApplyEdit
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

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCustomerSourceNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerSourceNo = new Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblPriority = new Krypton.Toolkit.KryptonLabel();
this.txtPriority = new Krypton.Toolkit.KryptonTextBox();

this.lblASProcessStatus = new Krypton.Toolkit.KryptonLabel();
this.txtASProcessStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblTotalInitialQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtTotalInitialQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalConfirmedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtTotalConfirmedQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblApplyDate = new Krypton.Toolkit.KryptonLabel();
this.dtpApplyDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblPreDeliveryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPreDeliveryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblShippingAddress = new Krypton.Toolkit.KryptonLabel();
this.txtShippingAddress = new Krypton.Toolkit.KryptonTextBox();
this.txtShippingAddress.Multiline = true;

this.lblShippingWay = new Krypton.Toolkit.KryptonLabel();
this.txtShippingWay = new Krypton.Toolkit.KryptonTextBox();

this.lblInWarrantyPeriod = new Krypton.Toolkit.KryptonLabel();
this.chkInWarrantyPeriod = new Krypton.Toolkit.KryptonCheckBox();
this.chkInWarrantyPeriod.Values.Text ="";

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblRepairEvaluationOpinion = new Krypton.Toolkit.KryptonLabel();
this.txtRepairEvaluationOpinion = new Krypton.Toolkit.KryptonTextBox();
this.txtRepairEvaluationOpinion.Multiline = true;

this.lblExpenseAllocationMode = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseAllocationMode = new Krypton.Toolkit.KryptonTextBox();

this.lblExpenseBearerType = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseBearerType = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblTotalDeliveredQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalDeliveredQty = new Krypton.Toolkit.KryptonTextBox();

this.lblMaterialFeeConfirmed = new Krypton.Toolkit.KryptonLabel();
this.chkMaterialFeeConfirmed = new Krypton.Toolkit.KryptonCheckBox();
this.chkMaterialFeeConfirmed.Values.Text ="";

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();

    
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
this.lblASApplyNo.Text = "申请编号";
this.txtASApplyNo.Location = new System.Drawing.Point(173,21);
this.txtASApplyNo.Name = "txtASApplyNo";
this.txtASApplyNo.Size = new System.Drawing.Size(100, 21);
this.txtASApplyNo.TabIndex = 1;
this.Controls.Add(this.lblASApplyNo);
this.Controls.Add(this.txtASApplyNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "申请客户";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####50CustomerSourceNo###String
this.lblCustomerSourceNo.AutoSize = true;
this.lblCustomerSourceNo.Location = new System.Drawing.Point(100,75);
this.lblCustomerSourceNo.Name = "lblCustomerSourceNo";
this.lblCustomerSourceNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerSourceNo.TabIndex = 3;
this.lblCustomerSourceNo.Text = "来源单号";
this.txtCustomerSourceNo.Location = new System.Drawing.Point(173,71);
this.txtCustomerSourceNo.Name = "txtCustomerSourceNo";
this.txtCustomerSourceNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerSourceNo.TabIndex = 3;
this.Controls.Add(this.lblCustomerSourceNo);
this.Controls.Add(this.txtCustomerSourceNo);

           //#####Location_ID###Int64
//属性测试100Location_ID
//属性测试100Location_ID
//属性测试100Location_ID
//属性测试100Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,100);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 4;
this.lblLocation_ID.Text = "售后暂存仓";
//111======100
this.cmbLocation_ID.Location = new System.Drawing.Point(173,96);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 4;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####Priority###Int32
//属性测试125Priority
//属性测试125Priority
//属性测试125Priority
//属性测试125Priority
this.lblPriority.AutoSize = true;
this.lblPriority.Location = new System.Drawing.Point(100,125);
this.lblPriority.Name = "lblPriority";
this.lblPriority.Size = new System.Drawing.Size(41, 12);
this.lblPriority.TabIndex = 5;
this.lblPriority.Text = "紧急程度";
this.txtPriority.Location = new System.Drawing.Point(173,121);
this.txtPriority.Name = "txtPriority";
this.txtPriority.Size = new System.Drawing.Size(100, 21);
this.txtPriority.TabIndex = 5;
this.Controls.Add(this.lblPriority);
this.Controls.Add(this.txtPriority);

           //#####ASProcessStatus###Int32
//属性测试150ASProcessStatus
//属性测试150ASProcessStatus
//属性测试150ASProcessStatus
//属性测试150ASProcessStatus
this.lblASProcessStatus.AutoSize = true;
this.lblASProcessStatus.Location = new System.Drawing.Point(100,150);
this.lblASProcessStatus.Name = "lblASProcessStatus";
this.lblASProcessStatus.Size = new System.Drawing.Size(41, 12);
this.lblASProcessStatus.TabIndex = 6;
this.lblASProcessStatus.Text = "处理状态";
this.txtASProcessStatus.Location = new System.Drawing.Point(173,146);
this.txtASProcessStatus.Name = "txtASProcessStatus";
this.txtASProcessStatus.Size = new System.Drawing.Size(100, 21);
this.txtASProcessStatus.TabIndex = 6;
this.Controls.Add(this.lblASProcessStatus);
this.Controls.Add(this.txtASProcessStatus);

           //#####Employee_ID###Int64
//属性测试175Employee_ID
//属性测试175Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,175);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 7;
this.lblEmployee_ID.Text = "业务员";
//111======175
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,171);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 7;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####ProjectGroup_ID###Int64
//属性测试200ProjectGroup_ID
//属性测试200ProjectGroup_ID
//属性测试200ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,200);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 8;
this.lblProjectGroup_ID.Text = "项目小组";
//111======200
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,196);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 8;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####TotalInitialQuantity###Int32
//属性测试225TotalInitialQuantity
//属性测试225TotalInitialQuantity
//属性测试225TotalInitialQuantity
//属性测试225TotalInitialQuantity
this.lblTotalInitialQuantity.AutoSize = true;
this.lblTotalInitialQuantity.Location = new System.Drawing.Point(100,225);
this.lblTotalInitialQuantity.Name = "lblTotalInitialQuantity";
this.lblTotalInitialQuantity.Size = new System.Drawing.Size(41, 12);
this.lblTotalInitialQuantity.TabIndex = 9;
this.lblTotalInitialQuantity.Text = "登记数量";
this.txtTotalInitialQuantity.Location = new System.Drawing.Point(173,221);
this.txtTotalInitialQuantity.Name = "txtTotalInitialQuantity";
this.txtTotalInitialQuantity.Size = new System.Drawing.Size(100, 21);
this.txtTotalInitialQuantity.TabIndex = 9;
this.Controls.Add(this.lblTotalInitialQuantity);
this.Controls.Add(this.txtTotalInitialQuantity);

           //#####TotalConfirmedQuantity###Int32
//属性测试250TotalConfirmedQuantity
//属性测试250TotalConfirmedQuantity
//属性测试250TotalConfirmedQuantity
//属性测试250TotalConfirmedQuantity
this.lblTotalConfirmedQuantity.AutoSize = true;
this.lblTotalConfirmedQuantity.Location = new System.Drawing.Point(100,250);
this.lblTotalConfirmedQuantity.Name = "lblTotalConfirmedQuantity";
this.lblTotalConfirmedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblTotalConfirmedQuantity.TabIndex = 10;
this.lblTotalConfirmedQuantity.Text = "复核数量";
this.txtTotalConfirmedQuantity.Location = new System.Drawing.Point(173,246);
this.txtTotalConfirmedQuantity.Name = "txtTotalConfirmedQuantity";
this.txtTotalConfirmedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtTotalConfirmedQuantity.TabIndex = 10;
this.Controls.Add(this.lblTotalConfirmedQuantity);
this.Controls.Add(this.txtTotalConfirmedQuantity);

           //#####ApplyDate###DateTime
this.lblApplyDate.AutoSize = true;
this.lblApplyDate.Location = new System.Drawing.Point(100,275);
this.lblApplyDate.Name = "lblApplyDate";
this.lblApplyDate.Size = new System.Drawing.Size(41, 12);
this.lblApplyDate.TabIndex = 11;
this.lblApplyDate.Text = "申请日期";
//111======275
this.dtpApplyDate.Location = new System.Drawing.Point(173,271);
this.dtpApplyDate.Name ="dtpApplyDate";
this.dtpApplyDate.Size = new System.Drawing.Size(100, 21);
this.dtpApplyDate.TabIndex = 11;
this.Controls.Add(this.lblApplyDate);
this.Controls.Add(this.dtpApplyDate);

           //#####PreDeliveryDate###DateTime
this.lblPreDeliveryDate.AutoSize = true;
this.lblPreDeliveryDate.Location = new System.Drawing.Point(100,300);
this.lblPreDeliveryDate.Name = "lblPreDeliveryDate";
this.lblPreDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblPreDeliveryDate.TabIndex = 12;
this.lblPreDeliveryDate.Text = "预交日期";
//111======300
this.dtpPreDeliveryDate.Location = new System.Drawing.Point(173,296);
this.dtpPreDeliveryDate.Name ="dtpPreDeliveryDate";
this.dtpPreDeliveryDate.ShowCheckBox =true;
this.dtpPreDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreDeliveryDate.TabIndex = 12;
this.Controls.Add(this.lblPreDeliveryDate);
this.Controls.Add(this.dtpPreDeliveryDate);

           //#####500ShippingAddress###String
this.lblShippingAddress.AutoSize = true;
this.lblShippingAddress.Location = new System.Drawing.Point(100,325);
this.lblShippingAddress.Name = "lblShippingAddress";
this.lblShippingAddress.Size = new System.Drawing.Size(41, 12);
this.lblShippingAddress.TabIndex = 13;
this.lblShippingAddress.Text = "收货地址";
this.txtShippingAddress.Location = new System.Drawing.Point(173,321);
this.txtShippingAddress.Name = "txtShippingAddress";
this.txtShippingAddress.Size = new System.Drawing.Size(100, 21);
this.txtShippingAddress.TabIndex = 13;
this.Controls.Add(this.lblShippingAddress);
this.Controls.Add(this.txtShippingAddress);

           //#####50ShippingWay###String
this.lblShippingWay.AutoSize = true;
this.lblShippingWay.Location = new System.Drawing.Point(100,350);
this.lblShippingWay.Name = "lblShippingWay";
this.lblShippingWay.Size = new System.Drawing.Size(41, 12);
this.lblShippingWay.TabIndex = 14;
this.lblShippingWay.Text = "发货方式";
this.txtShippingWay.Location = new System.Drawing.Point(173,346);
this.txtShippingWay.Name = "txtShippingWay";
this.txtShippingWay.Size = new System.Drawing.Size(100, 21);
this.txtShippingWay.TabIndex = 14;
this.Controls.Add(this.lblShippingWay);
this.Controls.Add(this.txtShippingWay);

           //#####InWarrantyPeriod###Boolean
this.lblInWarrantyPeriod.AutoSize = true;
this.lblInWarrantyPeriod.Location = new System.Drawing.Point(100,375);
this.lblInWarrantyPeriod.Name = "lblInWarrantyPeriod";
this.lblInWarrantyPeriod.Size = new System.Drawing.Size(41, 12);
this.lblInWarrantyPeriod.TabIndex = 15;
this.lblInWarrantyPeriod.Text = "保修期内";
this.chkInWarrantyPeriod.Location = new System.Drawing.Point(173,371);
this.chkInWarrantyPeriod.Name = "chkInWarrantyPeriod";
this.chkInWarrantyPeriod.Size = new System.Drawing.Size(100, 21);
this.chkInWarrantyPeriod.TabIndex = 15;
this.Controls.Add(this.lblInWarrantyPeriod);
this.Controls.Add(this.chkInWarrantyPeriod);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,400);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 16;
this.lblCreated_at.Text = "创建时间";
//111======400
this.dtpCreated_at.Location = new System.Drawing.Point(173,396);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 16;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试425Created_by
//属性测试425Created_by
//属性测试425Created_by
//属性测试425Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,425);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 17;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,421);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 17;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,450);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 18;
this.lblModified_at.Text = "修改时间";
//111======450
this.dtpModified_at.Location = new System.Drawing.Point(173,446);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 18;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试475Modified_by
//属性测试475Modified_by
//属性测试475Modified_by
//属性测试475Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,475);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 19;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,471);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 19;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####500RepairEvaluationOpinion###String
this.lblRepairEvaluationOpinion.AutoSize = true;
this.lblRepairEvaluationOpinion.Location = new System.Drawing.Point(100,500);
this.lblRepairEvaluationOpinion.Name = "lblRepairEvaluationOpinion";
this.lblRepairEvaluationOpinion.Size = new System.Drawing.Size(41, 12);
this.lblRepairEvaluationOpinion.TabIndex = 20;
this.lblRepairEvaluationOpinion.Text = "维修评估意见";
this.txtRepairEvaluationOpinion.Location = new System.Drawing.Point(173,496);
this.txtRepairEvaluationOpinion.Name = "txtRepairEvaluationOpinion";
this.txtRepairEvaluationOpinion.Size = new System.Drawing.Size(100, 21);
this.txtRepairEvaluationOpinion.TabIndex = 20;
this.Controls.Add(this.lblRepairEvaluationOpinion);
this.Controls.Add(this.txtRepairEvaluationOpinion);

           //#####ExpenseAllocationMode###Int32
//属性测试525ExpenseAllocationMode
//属性测试525ExpenseAllocationMode
//属性测试525ExpenseAllocationMode
//属性测试525ExpenseAllocationMode
this.lblExpenseAllocationMode.AutoSize = true;
this.lblExpenseAllocationMode.Location = new System.Drawing.Point(100,525);
this.lblExpenseAllocationMode.Name = "lblExpenseAllocationMode";
this.lblExpenseAllocationMode.Size = new System.Drawing.Size(41, 12);
this.lblExpenseAllocationMode.TabIndex = 21;
this.lblExpenseAllocationMode.Text = "费用承担模式";
this.txtExpenseAllocationMode.Location = new System.Drawing.Point(173,521);
this.txtExpenseAllocationMode.Name = "txtExpenseAllocationMode";
this.txtExpenseAllocationMode.Size = new System.Drawing.Size(100, 21);
this.txtExpenseAllocationMode.TabIndex = 21;
this.Controls.Add(this.lblExpenseAllocationMode);
this.Controls.Add(this.txtExpenseAllocationMode);

           //#####ExpenseBearerType###Int32
//属性测试550ExpenseBearerType
//属性测试550ExpenseBearerType
//属性测试550ExpenseBearerType
//属性测试550ExpenseBearerType
this.lblExpenseBearerType.AutoSize = true;
this.lblExpenseBearerType.Location = new System.Drawing.Point(100,550);
this.lblExpenseBearerType.Name = "lblExpenseBearerType";
this.lblExpenseBearerType.Size = new System.Drawing.Size(41, 12);
this.lblExpenseBearerType.TabIndex = 22;
this.lblExpenseBearerType.Text = "费用承担方";
this.txtExpenseBearerType.Location = new System.Drawing.Point(173,546);
this.txtExpenseBearerType.Name = "txtExpenseBearerType";
this.txtExpenseBearerType.Size = new System.Drawing.Size(100, 21);
this.txtExpenseBearerType.TabIndex = 22;
this.Controls.Add(this.lblExpenseBearerType);
this.Controls.Add(this.txtExpenseBearerType);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,575);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 23;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,571);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 23;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####TotalDeliveredQty###Int32
//属性测试600TotalDeliveredQty
//属性测试600TotalDeliveredQty
//属性测试600TotalDeliveredQty
//属性测试600TotalDeliveredQty
this.lblTotalDeliveredQty.AutoSize = true;
this.lblTotalDeliveredQty.Location = new System.Drawing.Point(100,600);
this.lblTotalDeliveredQty.Name = "lblTotalDeliveredQty";
this.lblTotalDeliveredQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalDeliveredQty.TabIndex = 24;
this.lblTotalDeliveredQty.Text = "交付数量";
this.txtTotalDeliveredQty.Location = new System.Drawing.Point(173,596);
this.txtTotalDeliveredQty.Name = "txtTotalDeliveredQty";
this.txtTotalDeliveredQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalDeliveredQty.TabIndex = 24;
this.Controls.Add(this.lblTotalDeliveredQty);
this.Controls.Add(this.txtTotalDeliveredQty);

           //#####MaterialFeeConfirmed###Boolean
this.lblMaterialFeeConfirmed.AutoSize = true;
this.lblMaterialFeeConfirmed.Location = new System.Drawing.Point(100,625);
this.lblMaterialFeeConfirmed.Name = "lblMaterialFeeConfirmed";
this.lblMaterialFeeConfirmed.Size = new System.Drawing.Size(41, 12);
this.lblMaterialFeeConfirmed.TabIndex = 25;
this.lblMaterialFeeConfirmed.Text = "费用确认状态";
this.chkMaterialFeeConfirmed.Location = new System.Drawing.Point(173,621);
this.chkMaterialFeeConfirmed.Name = "chkMaterialFeeConfirmed";
this.chkMaterialFeeConfirmed.Size = new System.Drawing.Size(100, 21);
this.chkMaterialFeeConfirmed.TabIndex = 25;
this.Controls.Add(this.lblMaterialFeeConfirmed);
this.Controls.Add(this.chkMaterialFeeConfirmed);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,650);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 26;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,646);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 26;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,675);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 27;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,671);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 27;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试700Approver_by
//属性测试700Approver_by
//属性测试700Approver_by
//属性测试700Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,700);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 28;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,696);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 28;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,725);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 29;
this.lblApprover_at.Text = "审批时间";
//111======725
this.dtpApprover_at.Location = new System.Drawing.Point(173,721);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 29;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,775);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 31;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,771);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 31;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####DataStatus###Int32
//属性测试800DataStatus
//属性测试800DataStatus
//属性测试800DataStatus
//属性测试800DataStatus
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,800);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 32;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,796);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 32;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####PrintStatus###Int32
//属性测试825PrintStatus
//属性测试825PrintStatus
//属性测试825PrintStatus
//属性测试825PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,825);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 33;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,821);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 33;
this.Controls.Add(this.lblPrintStatus);
this.Controls.Add(this.txtPrintStatus);

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
           // this.kryptonPanel1.TabIndex = 33;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblASApplyNo );
this.Controls.Add(this.txtASApplyNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblCustomerSourceNo );
this.Controls.Add(this.txtCustomerSourceNo );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblPriority );
this.Controls.Add(this.txtPriority );

                this.Controls.Add(this.lblASProcessStatus );
this.Controls.Add(this.txtASProcessStatus );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblTotalInitialQuantity );
this.Controls.Add(this.txtTotalInitialQuantity );

                this.Controls.Add(this.lblTotalConfirmedQuantity );
this.Controls.Add(this.txtTotalConfirmedQuantity );

                this.Controls.Add(this.lblApplyDate );
this.Controls.Add(this.dtpApplyDate );

                this.Controls.Add(this.lblPreDeliveryDate );
this.Controls.Add(this.dtpPreDeliveryDate );

                this.Controls.Add(this.lblShippingAddress );
this.Controls.Add(this.txtShippingAddress );

                this.Controls.Add(this.lblShippingWay );
this.Controls.Add(this.txtShippingWay );

                this.Controls.Add(this.lblInWarrantyPeriod );
this.Controls.Add(this.chkInWarrantyPeriod );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblRepairEvaluationOpinion );
this.Controls.Add(this.txtRepairEvaluationOpinion );

                this.Controls.Add(this.lblExpenseAllocationMode );
this.Controls.Add(this.txtExpenseAllocationMode );

                this.Controls.Add(this.lblExpenseBearerType );
this.Controls.Add(this.txtExpenseBearerType );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblTotalDeliveredQty );
this.Controls.Add(this.txtTotalDeliveredQty );

                this.Controls.Add(this.lblMaterialFeeConfirmed );
this.Controls.Add(this.chkMaterialFeeConfirmed );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                            // 
            // "tb_AS_AfterSaleApplyEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_AS_AfterSaleApplyEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerSourceNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerSourceNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPriority;
private Krypton.Toolkit.KryptonTextBox txtPriority;

    
        
              private Krypton.Toolkit.KryptonLabel lblASProcessStatus;
private Krypton.Toolkit.KryptonTextBox txtASProcessStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalInitialQuantity;
private Krypton.Toolkit.KryptonTextBox txtTotalInitialQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalConfirmedQuantity;
private Krypton.Toolkit.KryptonTextBox txtTotalConfirmedQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblApplyDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpApplyDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPreDeliveryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPreDeliveryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblShippingAddress;
private Krypton.Toolkit.KryptonTextBox txtShippingAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblShippingWay;
private Krypton.Toolkit.KryptonTextBox txtShippingWay;

    
        
              private Krypton.Toolkit.KryptonLabel lblInWarrantyPeriod;
private Krypton.Toolkit.KryptonCheckBox chkInWarrantyPeriod;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblRepairEvaluationOpinion;
private Krypton.Toolkit.KryptonTextBox txtRepairEvaluationOpinion;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseAllocationMode;
private Krypton.Toolkit.KryptonTextBox txtExpenseAllocationMode;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseBearerType;
private Krypton.Toolkit.KryptonTextBox txtExpenseBearerType;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalDeliveredQty;
private Krypton.Toolkit.KryptonTextBox txtTotalDeliveredQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblMaterialFeeConfirmed;
private Krypton.Toolkit.KryptonCheckBox chkMaterialFeeConfirmed;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

