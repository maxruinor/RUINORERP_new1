// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 维修工单  工时费 材料费
    /// </summary>
    partial class tb_AS_RepairOrderEdit
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
     this.lblRepairOrderNo = new Krypton.Toolkit.KryptonLabel();
this.txtRepairOrderNo = new Krypton.Toolkit.KryptonTextBox();

this.lblASApplyID = new Krypton.Toolkit.KryptonLabel();
this.cmbASApplyID = new Krypton.Toolkit.KryptonComboBox();

this.lblASApplyNo = new Krypton.Toolkit.KryptonLabel();
this.txtASApplyNo = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblRepairStatus = new Krypton.Toolkit.KryptonLabel();
this.txtRepairStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPayStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPayStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblTotalQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalQty = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalDeliveredQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalDeliveredQty = new Krypton.Toolkit.KryptonTextBox();

this.lblLaborCost = new Krypton.Toolkit.KryptonLabel();
this.txtLaborCost = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalMaterialAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalMaterialAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerPaidAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerPaidAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblExpenseAllocationMode = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseAllocationMode = new Krypton.Toolkit.KryptonTextBox();

this.lblExpenseBearerType = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseBearerType = new Krypton.Toolkit.KryptonTextBox();

this.lblRepairStartDate = new Krypton.Toolkit.KryptonLabel();
this.dtpRepairStartDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblPreDeliveryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPreDeliveryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblTotalMaterialCost = new Krypton.Toolkit.KryptonLabel();
this.txtTotalMaterialCost = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####50RepairOrderNo###String
this.lblRepairOrderNo.AutoSize = true;
this.lblRepairOrderNo.Location = new System.Drawing.Point(100,25);
this.lblRepairOrderNo.Name = "lblRepairOrderNo";
this.lblRepairOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblRepairOrderNo.TabIndex = 1;
this.lblRepairOrderNo.Text = "维修工单号";
this.txtRepairOrderNo.Location = new System.Drawing.Point(173,21);
this.txtRepairOrderNo.Name = "txtRepairOrderNo";
this.txtRepairOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtRepairOrderNo.TabIndex = 1;
this.Controls.Add(this.lblRepairOrderNo);
this.Controls.Add(this.txtRepairOrderNo);

           //#####ASApplyID###Int64
//属性测试50ASApplyID
//属性测试50ASApplyID
//属性测试50ASApplyID
//属性测试50ASApplyID
//属性测试50ASApplyID
this.lblASApplyID.AutoSize = true;
this.lblASApplyID.Location = new System.Drawing.Point(100,50);
this.lblASApplyID.Name = "lblASApplyID";
this.lblASApplyID.Size = new System.Drawing.Size(41, 12);
this.lblASApplyID.TabIndex = 2;
this.lblASApplyID.Text = "售后申请单";
//111======50
this.cmbASApplyID.Location = new System.Drawing.Point(173,46);
this.cmbASApplyID.Name ="cmbASApplyID";
this.cmbASApplyID.Size = new System.Drawing.Size(100, 21);
this.cmbASApplyID.TabIndex = 2;
this.Controls.Add(this.lblASApplyID);
this.Controls.Add(this.cmbASApplyID);

           //#####50ASApplyNo###String
this.lblASApplyNo.AutoSize = true;
this.lblASApplyNo.Location = new System.Drawing.Point(100,75);
this.lblASApplyNo.Name = "lblASApplyNo";
this.lblASApplyNo.Size = new System.Drawing.Size(41, 12);
this.lblASApplyNo.TabIndex = 3;
this.lblASApplyNo.Text = "售后申请编号";
this.txtASApplyNo.Location = new System.Drawing.Point(173,71);
this.txtASApplyNo.Name = "txtASApplyNo";
this.txtASApplyNo.Size = new System.Drawing.Size(100, 21);
this.txtASApplyNo.TabIndex = 3;
this.Controls.Add(this.lblASApplyNo);
this.Controls.Add(this.txtASApplyNo);

           //#####Employee_ID###Int64
//属性测试100Employee_ID
//属性测试100Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,100);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 4;
this.lblEmployee_ID.Text = "经办人员";
//111======100
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,96);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 4;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####ProjectGroup_ID###Int64
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,125);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 5;
this.lblProjectGroup_ID.Text = "项目小组";
//111======125
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,121);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 5;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####CustomerVendor_ID###Int64
//属性测试150CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,150);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 6;
this.lblCustomerVendor_ID.Text = "所属客户";
//111======150
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,146);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 6;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####RepairStatus###Int32
//属性测试175RepairStatus
//属性测试175RepairStatus
//属性测试175RepairStatus
//属性测试175RepairStatus
//属性测试175RepairStatus
this.lblRepairStatus.AutoSize = true;
this.lblRepairStatus.Location = new System.Drawing.Point(100,175);
this.lblRepairStatus.Name = "lblRepairStatus";
this.lblRepairStatus.Size = new System.Drawing.Size(41, 12);
this.lblRepairStatus.TabIndex = 7;
this.lblRepairStatus.Text = "维修状态";
this.txtRepairStatus.Location = new System.Drawing.Point(173,171);
this.txtRepairStatus.Name = "txtRepairStatus";
this.txtRepairStatus.Size = new System.Drawing.Size(100, 21);
this.txtRepairStatus.TabIndex = 7;
this.Controls.Add(this.lblRepairStatus);
this.Controls.Add(this.txtRepairStatus);

           //#####PayStatus###Int32
//属性测试200PayStatus
//属性测试200PayStatus
//属性测试200PayStatus
//属性测试200PayStatus
//属性测试200PayStatus
this.lblPayStatus.AutoSize = true;
this.lblPayStatus.Location = new System.Drawing.Point(100,200);
this.lblPayStatus.Name = "lblPayStatus";
this.lblPayStatus.Size = new System.Drawing.Size(41, 12);
this.lblPayStatus.TabIndex = 8;
this.lblPayStatus.Text = "付款状态";
this.txtPayStatus.Location = new System.Drawing.Point(173,196);
this.txtPayStatus.Name = "txtPayStatus";
this.txtPayStatus.Size = new System.Drawing.Size(100, 21);
this.txtPayStatus.TabIndex = 8;
this.Controls.Add(this.lblPayStatus);
this.Controls.Add(this.txtPayStatus);

           //#####Paytype_ID###Int64
//属性测试225Paytype_ID
//属性测试225Paytype_ID
//属性测试225Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,225);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 9;
this.lblPaytype_ID.Text = "付款方式";
//111======225
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,221);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 9;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####TotalQty###Int32
//属性测试250TotalQty
//属性测试250TotalQty
//属性测试250TotalQty
//属性测试250TotalQty
//属性测试250TotalQty
this.lblTotalQty.AutoSize = true;
this.lblTotalQty.Location = new System.Drawing.Point(100,250);
this.lblTotalQty.Name = "lblTotalQty";
this.lblTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalQty.TabIndex = 10;
this.lblTotalQty.Text = "总数量";
this.txtTotalQty.Location = new System.Drawing.Point(173,246);
this.txtTotalQty.Name = "txtTotalQty";
this.txtTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalQty.TabIndex = 10;
this.Controls.Add(this.lblTotalQty);
this.Controls.Add(this.txtTotalQty);

           //#####TotalDeliveredQty###Int32
//属性测试275TotalDeliveredQty
//属性测试275TotalDeliveredQty
//属性测试275TotalDeliveredQty
//属性测试275TotalDeliveredQty
//属性测试275TotalDeliveredQty
this.lblTotalDeliveredQty.AutoSize = true;
this.lblTotalDeliveredQty.Location = new System.Drawing.Point(100,275);
this.lblTotalDeliveredQty.Name = "lblTotalDeliveredQty";
this.lblTotalDeliveredQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalDeliveredQty.TabIndex = 11;
this.lblTotalDeliveredQty.Text = "交付数量";
this.txtTotalDeliveredQty.Location = new System.Drawing.Point(173,271);
this.txtTotalDeliveredQty.Name = "txtTotalDeliveredQty";
this.txtTotalDeliveredQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalDeliveredQty.TabIndex = 11;
this.Controls.Add(this.lblTotalDeliveredQty);
this.Controls.Add(this.txtTotalDeliveredQty);

           //#####LaborCost###Decimal
this.lblLaborCost.AutoSize = true;
this.lblLaborCost.Location = new System.Drawing.Point(100,300);
this.lblLaborCost.Name = "lblLaborCost";
this.lblLaborCost.Size = new System.Drawing.Size(41, 12);
this.lblLaborCost.TabIndex = 12;
this.lblLaborCost.Text = "总人工成本";
//111======300
this.txtLaborCost.Location = new System.Drawing.Point(173,296);
this.txtLaborCost.Name ="txtLaborCost";
this.txtLaborCost.Size = new System.Drawing.Size(100, 21);
this.txtLaborCost.TabIndex = 12;
this.Controls.Add(this.lblLaborCost);
this.Controls.Add(this.txtLaborCost);

           //#####TotalMaterialAmount###Decimal
this.lblTotalMaterialAmount.AutoSize = true;
this.lblTotalMaterialAmount.Location = new System.Drawing.Point(100,325);
this.lblTotalMaterialAmount.Name = "lblTotalMaterialAmount";
this.lblTotalMaterialAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialAmount.TabIndex = 13;
this.lblTotalMaterialAmount.Text = "总材料费用";
//111======325
this.txtTotalMaterialAmount.Location = new System.Drawing.Point(173,321);
this.txtTotalMaterialAmount.Name ="txtTotalMaterialAmount";
this.txtTotalMaterialAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialAmount.TabIndex = 13;
this.Controls.Add(this.lblTotalMaterialAmount);
this.Controls.Add(this.txtTotalMaterialAmount);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,350);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 14;
this.lblTotalAmount.Text = "总费用";
//111======350
this.txtTotalAmount.Location = new System.Drawing.Point(173,346);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 14;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####CustomerPaidAmount###Decimal
this.lblCustomerPaidAmount.AutoSize = true;
this.lblCustomerPaidAmount.Location = new System.Drawing.Point(100,375);
this.lblCustomerPaidAmount.Name = "lblCustomerPaidAmount";
this.lblCustomerPaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPaidAmount.TabIndex = 15;
this.lblCustomerPaidAmount.Text = "客户支付金额";
//111======375
this.txtCustomerPaidAmount.Location = new System.Drawing.Point(173,371);
this.txtCustomerPaidAmount.Name ="txtCustomerPaidAmount";
this.txtCustomerPaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPaidAmount.TabIndex = 15;
this.Controls.Add(this.lblCustomerPaidAmount);
this.Controls.Add(this.txtCustomerPaidAmount);

           //#####ExpenseAllocationMode###Int32
//属性测试400ExpenseAllocationMode
//属性测试400ExpenseAllocationMode
//属性测试400ExpenseAllocationMode
//属性测试400ExpenseAllocationMode
//属性测试400ExpenseAllocationMode
this.lblExpenseAllocationMode.AutoSize = true;
this.lblExpenseAllocationMode.Location = new System.Drawing.Point(100,400);
this.lblExpenseAllocationMode.Name = "lblExpenseAllocationMode";
this.lblExpenseAllocationMode.Size = new System.Drawing.Size(41, 12);
this.lblExpenseAllocationMode.TabIndex = 16;
this.lblExpenseAllocationMode.Text = "费用承担模式";
this.txtExpenseAllocationMode.Location = new System.Drawing.Point(173,396);
this.txtExpenseAllocationMode.Name = "txtExpenseAllocationMode";
this.txtExpenseAllocationMode.Size = new System.Drawing.Size(100, 21);
this.txtExpenseAllocationMode.TabIndex = 16;
this.Controls.Add(this.lblExpenseAllocationMode);
this.Controls.Add(this.txtExpenseAllocationMode);

           //#####ExpenseBearerType###Int32
//属性测试425ExpenseBearerType
//属性测试425ExpenseBearerType
//属性测试425ExpenseBearerType
//属性测试425ExpenseBearerType
//属性测试425ExpenseBearerType
this.lblExpenseBearerType.AutoSize = true;
this.lblExpenseBearerType.Location = new System.Drawing.Point(100,425);
this.lblExpenseBearerType.Name = "lblExpenseBearerType";
this.lblExpenseBearerType.Size = new System.Drawing.Size(41, 12);
this.lblExpenseBearerType.TabIndex = 17;
this.lblExpenseBearerType.Text = "费用承担方";
this.txtExpenseBearerType.Location = new System.Drawing.Point(173,421);
this.txtExpenseBearerType.Name = "txtExpenseBearerType";
this.txtExpenseBearerType.Size = new System.Drawing.Size(100, 21);
this.txtExpenseBearerType.TabIndex = 17;
this.Controls.Add(this.lblExpenseBearerType);
this.Controls.Add(this.txtExpenseBearerType);

           //#####RepairStartDate###DateTime
this.lblRepairStartDate.AutoSize = true;
this.lblRepairStartDate.Location = new System.Drawing.Point(100,450);
this.lblRepairStartDate.Name = "lblRepairStartDate";
this.lblRepairStartDate.Size = new System.Drawing.Size(41, 12);
this.lblRepairStartDate.TabIndex = 18;
this.lblRepairStartDate.Text = "开始日期";
//111======450
this.dtpRepairStartDate.Location = new System.Drawing.Point(173,446);
this.dtpRepairStartDate.Name ="dtpRepairStartDate";
this.dtpRepairStartDate.Size = new System.Drawing.Size(100, 21);
this.dtpRepairStartDate.TabIndex = 18;
this.Controls.Add(this.lblRepairStartDate);
this.Controls.Add(this.dtpRepairStartDate);

           //#####PreDeliveryDate###DateTime
this.lblPreDeliveryDate.AutoSize = true;
this.lblPreDeliveryDate.Location = new System.Drawing.Point(100,475);
this.lblPreDeliveryDate.Name = "lblPreDeliveryDate";
this.lblPreDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblPreDeliveryDate.TabIndex = 19;
this.lblPreDeliveryDate.Text = "预交日期";
//111======475
this.dtpPreDeliveryDate.Location = new System.Drawing.Point(173,471);
this.dtpPreDeliveryDate.Name ="dtpPreDeliveryDate";
this.dtpPreDeliveryDate.ShowCheckBox =true;
this.dtpPreDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreDeliveryDate.TabIndex = 19;
this.Controls.Add(this.lblPreDeliveryDate);
this.Controls.Add(this.dtpPreDeliveryDate);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,500);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 20;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,496);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 20;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,525);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 21;
this.lblCreated_at.Text = "创建时间";
//111======525
this.dtpCreated_at.Location = new System.Drawing.Point(173,521);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 21;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试550Created_by
//属性测试550Created_by
//属性测试550Created_by
//属性测试550Created_by
//属性测试550Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,550);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 22;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,546);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 22;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,575);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 23;
this.lblModified_at.Text = "修改时间";
//111======575
this.dtpModified_at.Location = new System.Drawing.Point(173,571);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 23;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试600Modified_by
//属性测试600Modified_by
//属性测试600Modified_by
//属性测试600Modified_by
//属性测试600Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,600);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 24;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,596);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 24;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####DataStatus###Int32
//属性测试625DataStatus
//属性测试625DataStatus
//属性测试625DataStatus
//属性测试625DataStatus
//属性测试625DataStatus
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,625);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 25;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,621);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 25;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,650);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 26;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,646);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 26;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试675Approver_by
//属性测试675Approver_by
//属性测试675Approver_by
//属性测试675Approver_by
//属性测试675Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,675);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 27;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,671);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 27;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,700);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 28;
this.lblApprover_at.Text = "审批时间";
//111======700
this.dtpApprover_at.Location = new System.Drawing.Point(173,696);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 28;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,750);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 30;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,746);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 30;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试775PrintStatus
//属性测试775PrintStatus
//属性测试775PrintStatus
//属性测试775PrintStatus
//属性测试775PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,775);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 31;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,771);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 31;
this.Controls.Add(this.lblPrintStatus);
this.Controls.Add(this.txtPrintStatus);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,800);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 32;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,796);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 32;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####TotalMaterialCost###Decimal
this.lblTotalMaterialCost.AutoSize = true;
this.lblTotalMaterialCost.Location = new System.Drawing.Point(100,825);
this.lblTotalMaterialCost.Name = "lblTotalMaterialCost";
this.lblTotalMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialCost.TabIndex = 33;
this.lblTotalMaterialCost.Text = "总材料成本";
//111======825
this.txtTotalMaterialCost.Location = new System.Drawing.Point(173,821);
this.txtTotalMaterialCost.Name ="txtTotalMaterialCost";
this.txtTotalMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialCost.TabIndex = 33;
this.Controls.Add(this.lblTotalMaterialCost);
this.Controls.Add(this.txtTotalMaterialCost);

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
                this.Controls.Add(this.lblRepairOrderNo );
this.Controls.Add(this.txtRepairOrderNo );

                this.Controls.Add(this.lblASApplyID );
this.Controls.Add(this.cmbASApplyID );

                this.Controls.Add(this.lblASApplyNo );
this.Controls.Add(this.txtASApplyNo );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblRepairStatus );
this.Controls.Add(this.txtRepairStatus );

                this.Controls.Add(this.lblPayStatus );
this.Controls.Add(this.txtPayStatus );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                this.Controls.Add(this.lblTotalQty );
this.Controls.Add(this.txtTotalQty );

                this.Controls.Add(this.lblTotalDeliveredQty );
this.Controls.Add(this.txtTotalDeliveredQty );

                this.Controls.Add(this.lblLaborCost );
this.Controls.Add(this.txtLaborCost );

                this.Controls.Add(this.lblTotalMaterialAmount );
this.Controls.Add(this.txtTotalMaterialAmount );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblCustomerPaidAmount );
this.Controls.Add(this.txtCustomerPaidAmount );

                this.Controls.Add(this.lblExpenseAllocationMode );
this.Controls.Add(this.txtExpenseAllocationMode );

                this.Controls.Add(this.lblExpenseBearerType );
this.Controls.Add(this.txtExpenseBearerType );

                this.Controls.Add(this.lblRepairStartDate );
this.Controls.Add(this.dtpRepairStartDate );

                this.Controls.Add(this.lblPreDeliveryDate );
this.Controls.Add(this.dtpPreDeliveryDate );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblTotalMaterialCost );
this.Controls.Add(this.txtTotalMaterialCost );

                            // 
            // "tb_AS_RepairOrderEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_AS_RepairOrderEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblRepairOrderNo;
private Krypton.Toolkit.KryptonTextBox txtRepairOrderNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblASApplyID;
private Krypton.Toolkit.KryptonComboBox cmbASApplyID;

    
        
              private Krypton.Toolkit.KryptonLabel lblASApplyNo;
private Krypton.Toolkit.KryptonTextBox txtASApplyNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRepairStatus;
private Krypton.Toolkit.KryptonTextBox txtRepairStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayStatus;
private Krypton.Toolkit.KryptonTextBox txtPayStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalQty;
private Krypton.Toolkit.KryptonTextBox txtTotalQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalDeliveredQty;
private Krypton.Toolkit.KryptonTextBox txtTotalDeliveredQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblLaborCost;
private Krypton.Toolkit.KryptonTextBox txtLaborCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalMaterialAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalMaterialAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPaidAmount;
private Krypton.Toolkit.KryptonTextBox txtCustomerPaidAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseAllocationMode;
private Krypton.Toolkit.KryptonTextBox txtExpenseAllocationMode;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseBearerType;
private Krypton.Toolkit.KryptonTextBox txtExpenseBearerType;

    
        
              private Krypton.Toolkit.KryptonLabel lblRepairStartDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpRepairStartDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPreDeliveryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPreDeliveryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalMaterialCost;
private Krypton.Toolkit.KryptonTextBox txtTotalMaterialCost;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

