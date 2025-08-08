
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:10
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 采购退货入库单
    /// </summary>
    partial class tb_PurReturnEntryQuery
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
        
     //for start
     
     this.lblPurReEntryNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurReEntryNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPurEntryRe_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPurEntryRe_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPurEntryReNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurEntryReNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblTotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBillDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpBillDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";


this.lblDeposit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDeposit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblTotalDiscountAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalDiscountAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblReceiptInvoiceClosed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkReceiptInvoiceClosed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkReceiptInvoiceClosed.Values.Text ="";



this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblGenerateVouchers = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGenerateVouchers = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGenerateVouchers.Values.Text ="";

this.lblVoucherNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtVoucherNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50PurReEntryNo###String
this.lblPurReEntryNo.AutoSize = true;
this.lblPurReEntryNo.Location = new System.Drawing.Point(100,25);
this.lblPurReEntryNo.Name = "lblPurReEntryNo";
this.lblPurReEntryNo.Size = new System.Drawing.Size(41, 12);
this.lblPurReEntryNo.TabIndex = 1;
this.lblPurReEntryNo.Text = "入库单号";
this.txtPurReEntryNo.Location = new System.Drawing.Point(173,21);
this.txtPurReEntryNo.Name = "txtPurReEntryNo";
this.txtPurReEntryNo.Size = new System.Drawing.Size(100, 21);
this.txtPurReEntryNo.TabIndex = 1;
this.Controls.Add(this.lblPurReEntryNo);
this.Controls.Add(this.txtPurReEntryNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "供应商";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####DepartmentID###Int64
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,75);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 3;
this.lblDepartmentID.Text = "部门";
//111======75
this.cmbDepartmentID.Location = new System.Drawing.Point(173,71);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 3;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####Employee_ID###Int64
//属性测试100Employee_ID
//属性测试100Employee_ID
//属性测试100Employee_ID
//属性测试100Employee_ID
//属性测试100Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,100);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 4;
this.lblEmployee_ID.Text = "经办人";
//111======100
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,96);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 4;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####Paytype_ID###Int64
//属性测试125Paytype_ID
//属性测试125Paytype_ID
//属性测试125Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,125);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 5;
this.lblPaytype_ID.Text = "付款类型";
//111======125
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,121);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 5;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####PurEntryRe_ID###Int64
//属性测试150PurEntryRe_ID
this.lblPurEntryRe_ID.AutoSize = true;
this.lblPurEntryRe_ID.Location = new System.Drawing.Point(100,150);
this.lblPurEntryRe_ID.Name = "lblPurEntryRe_ID";
this.lblPurEntryRe_ID.Size = new System.Drawing.Size(41, 12);
this.lblPurEntryRe_ID.TabIndex = 6;
this.lblPurEntryRe_ID.Text = "采购退货单";
//111======150
this.cmbPurEntryRe_ID.Location = new System.Drawing.Point(173,146);
this.cmbPurEntryRe_ID.Name ="cmbPurEntryRe_ID";
this.cmbPurEntryRe_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPurEntryRe_ID.TabIndex = 6;
this.Controls.Add(this.lblPurEntryRe_ID);
this.Controls.Add(this.cmbPurEntryRe_ID);

           //#####50PurEntryReNo###String
this.lblPurEntryReNo.AutoSize = true;
this.lblPurEntryReNo.Location = new System.Drawing.Point(100,175);
this.lblPurEntryReNo.Name = "lblPurEntryReNo";
this.lblPurEntryReNo.Size = new System.Drawing.Size(41, 12);
this.lblPurEntryReNo.TabIndex = 7;
this.lblPurEntryReNo.Text = "采购退货单号";
this.txtPurEntryReNo.Location = new System.Drawing.Point(173,171);
this.txtPurEntryReNo.Name = "txtPurEntryReNo";
this.txtPurEntryReNo.Size = new System.Drawing.Size(100, 21);
this.txtPurEntryReNo.TabIndex = 7;
this.Controls.Add(this.lblPurEntryReNo);
this.Controls.Add(this.txtPurEntryReNo);

           //#####TotalQty###Int32
//属性测试200TotalQty
//属性测试200TotalQty
//属性测试200TotalQty
//属性测试200TotalQty
//属性测试200TotalQty

           //#####TotalTaxAmount###Decimal
this.lblTotalTaxAmount.AutoSize = true;
this.lblTotalTaxAmount.Location = new System.Drawing.Point(100,225);
this.lblTotalTaxAmount.Name = "lblTotalTaxAmount";
this.lblTotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalTaxAmount.TabIndex = 9;
this.lblTotalTaxAmount.Text = "合计税额";
//111======225
this.txtTotalTaxAmount.Location = new System.Drawing.Point(173,221);
this.txtTotalTaxAmount.Name ="txtTotalTaxAmount";
this.txtTotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalTaxAmount.TabIndex = 9;
this.Controls.Add(this.lblTotalTaxAmount);
this.Controls.Add(this.txtTotalTaxAmount);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,250);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 10;
this.lblTotalAmount.Text = "合计金额";
//111======250
this.txtTotalAmount.Location = new System.Drawing.Point(173,246);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 10;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####BillDate###DateTime
this.lblBillDate.AutoSize = true;
this.lblBillDate.Location = new System.Drawing.Point(100,275);
this.lblBillDate.Name = "lblBillDate";
this.lblBillDate.Size = new System.Drawing.Size(41, 12);
this.lblBillDate.TabIndex = 11;
this.lblBillDate.Text = "单据日期";
//111======275
this.dtpBillDate.Location = new System.Drawing.Point(173,271);
this.dtpBillDate.Name ="dtpBillDate";
this.dtpBillDate.Size = new System.Drawing.Size(100, 21);
this.dtpBillDate.TabIndex = 11;
this.Controls.Add(this.lblBillDate);
this.Controls.Add(this.dtpBillDate);

           //#####50ShippingWay###String
this.lblShippingWay.AutoSize = true;
this.lblShippingWay.Location = new System.Drawing.Point(100,300);
this.lblShippingWay.Name = "lblShippingWay";
this.lblShippingWay.Size = new System.Drawing.Size(41, 12);
this.lblShippingWay.TabIndex = 12;
this.lblShippingWay.Text = "发货方式";
this.txtShippingWay.Location = new System.Drawing.Point(173,296);
this.txtShippingWay.Name = "txtShippingWay";
this.txtShippingWay.Size = new System.Drawing.Size(100, 21);
this.txtShippingWay.TabIndex = 12;
this.Controls.Add(this.lblShippingWay);
this.Controls.Add(this.txtShippingWay);

           //#####50TrackNo###String
this.lblTrackNo.AutoSize = true;
this.lblTrackNo.Location = new System.Drawing.Point(100,325);
this.lblTrackNo.Name = "lblTrackNo";
this.lblTrackNo.Size = new System.Drawing.Size(41, 12);
this.lblTrackNo.TabIndex = 13;
this.lblTrackNo.Text = "物流单号";
this.txtTrackNo.Location = new System.Drawing.Point(173,321);
this.txtTrackNo.Name = "txtTrackNo";
this.txtTrackNo.Size = new System.Drawing.Size(100, 21);
this.txtTrackNo.TabIndex = 13;
this.Controls.Add(this.lblTrackNo);
this.Controls.Add(this.txtTrackNo);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,350);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 14;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,346);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 14;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,375);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 15;
this.lblCreated_at.Text = "创建时间";
//111======375
this.dtpCreated_at.Location = new System.Drawing.Point(173,371);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 15;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试400Created_by
//属性测试400Created_by
//属性测试400Created_by
//属性测试400Created_by
//属性测试400Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,425);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 17;
this.lblModified_at.Text = "修改时间";
//111======425
this.dtpModified_at.Location = new System.Drawing.Point(173,421);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 17;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试450Modified_by
//属性测试450Modified_by
//属性测试450Modified_by
//属性测试450Modified_by
//属性测试450Modified_by

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,475);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 19;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,471);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 19;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,500);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 20;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,496);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 20;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,550);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 22;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,546);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 22;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,575);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 23;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,571);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 23;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####KeepAccountsType###Int32
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType

           //#####Deposit###Decimal
this.lblDeposit.AutoSize = true;
this.lblDeposit.Location = new System.Drawing.Point(100,625);
this.lblDeposit.Name = "lblDeposit";
this.lblDeposit.Size = new System.Drawing.Size(41, 12);
this.lblDeposit.TabIndex = 25;
this.lblDeposit.Text = "订金";
//111======625
this.txtDeposit.Location = new System.Drawing.Point(173,621);
this.txtDeposit.Name ="txtDeposit";
this.txtDeposit.Size = new System.Drawing.Size(100, 21);
this.txtDeposit.TabIndex = 25;
this.Controls.Add(this.lblDeposit);
this.Controls.Add(this.txtDeposit);

           //#####TaxDeductionType###Int32
//属性测试650TaxDeductionType
//属性测试650TaxDeductionType
//属性测试650TaxDeductionType
//属性测试650TaxDeductionType
//属性测试650TaxDeductionType

           //#####TotalDiscountAmount###Decimal
this.lblTotalDiscountAmount.AutoSize = true;
this.lblTotalDiscountAmount.Location = new System.Drawing.Point(100,675);
this.lblTotalDiscountAmount.Name = "lblTotalDiscountAmount";
this.lblTotalDiscountAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalDiscountAmount.TabIndex = 27;
this.lblTotalDiscountAmount.Text = "折扣金额总计";
//111======675
this.txtTotalDiscountAmount.Location = new System.Drawing.Point(173,671);
this.txtTotalDiscountAmount.Name ="txtTotalDiscountAmount";
this.txtTotalDiscountAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalDiscountAmount.TabIndex = 27;
this.Controls.Add(this.lblTotalDiscountAmount);
this.Controls.Add(this.txtTotalDiscountAmount);

           //#####ReceiptInvoiceClosed###Boolean
this.lblReceiptInvoiceClosed.AutoSize = true;
this.lblReceiptInvoiceClosed.Location = new System.Drawing.Point(100,700);
this.lblReceiptInvoiceClosed.Name = "lblReceiptInvoiceClosed";
this.lblReceiptInvoiceClosed.Size = new System.Drawing.Size(41, 12);
this.lblReceiptInvoiceClosed.TabIndex = 28;
this.lblReceiptInvoiceClosed.Text = "立帐结案";
this.chkReceiptInvoiceClosed.Location = new System.Drawing.Point(173,696);
this.chkReceiptInvoiceClosed.Name = "chkReceiptInvoiceClosed";
this.chkReceiptInvoiceClosed.Size = new System.Drawing.Size(100, 21);
this.chkReceiptInvoiceClosed.TabIndex = 28;
this.Controls.Add(this.lblReceiptInvoiceClosed);
this.Controls.Add(this.chkReceiptInvoiceClosed);

           //#####DataStatus###Int32
//属性测试725DataStatus
//属性测试725DataStatus
//属性测试725DataStatus
//属性测试725DataStatus
//属性测试725DataStatus

           //#####Approver_by###Int64
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,775);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 31;
this.lblApprover_at.Text = "审批时间";
//111======775
this.dtpApprover_at.Location = new System.Drawing.Point(173,771);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 31;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####PrintStatus###Int32
//属性测试800PrintStatus
//属性测试800PrintStatus
//属性测试800PrintStatus
//属性测试800PrintStatus
//属性测试800PrintStatus

           //#####GenerateVouchers###Boolean
this.lblGenerateVouchers.AutoSize = true;
this.lblGenerateVouchers.Location = new System.Drawing.Point(100,825);
this.lblGenerateVouchers.Name = "lblGenerateVouchers";
this.lblGenerateVouchers.Size = new System.Drawing.Size(41, 12);
this.lblGenerateVouchers.TabIndex = 33;
this.lblGenerateVouchers.Text = "生成凭证";
this.chkGenerateVouchers.Location = new System.Drawing.Point(173,821);
this.chkGenerateVouchers.Name = "chkGenerateVouchers";
this.chkGenerateVouchers.Size = new System.Drawing.Size(100, 21);
this.chkGenerateVouchers.TabIndex = 33;
this.Controls.Add(this.lblGenerateVouchers);
this.Controls.Add(this.chkGenerateVouchers);

           //#####50VoucherNO###String
this.lblVoucherNO.AutoSize = true;
this.lblVoucherNO.Location = new System.Drawing.Point(100,850);
this.lblVoucherNO.Name = "lblVoucherNO";
this.lblVoucherNO.Size = new System.Drawing.Size(41, 12);
this.lblVoucherNO.TabIndex = 34;
this.lblVoucherNO.Text = "凭证号码";
this.txtVoucherNO.Location = new System.Drawing.Point(173,846);
this.txtVoucherNO.Name = "txtVoucherNO";
this.txtVoucherNO.Size = new System.Drawing.Size(100, 21);
this.txtVoucherNO.TabIndex = 34;
this.Controls.Add(this.lblVoucherNO);
this.Controls.Add(this.txtVoucherNO);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPurReEntryNo );
this.Controls.Add(this.txtPurReEntryNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                this.Controls.Add(this.lblPurEntryRe_ID );
this.Controls.Add(this.cmbPurEntryRe_ID );

                this.Controls.Add(this.lblPurEntryReNo );
this.Controls.Add(this.txtPurEntryReNo );

                
                this.Controls.Add(this.lblTotalTaxAmount );
this.Controls.Add(this.txtTotalTaxAmount );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblBillDate );
this.Controls.Add(this.dtpBillDate );

                this.Controls.Add(this.lblShippingWay );
this.Controls.Add(this.txtShippingWay );

                this.Controls.Add(this.lblTrackNo );
this.Controls.Add(this.txtTrackNo );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                
                this.Controls.Add(this.lblDeposit );
this.Controls.Add(this.txtDeposit );

                
                this.Controls.Add(this.lblTotalDiscountAmount );
this.Controls.Add(this.txtTotalDiscountAmount );

                this.Controls.Add(this.lblReceiptInvoiceClosed );
this.Controls.Add(this.chkReceiptInvoiceClosed );

                
                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblGenerateVouchers );
this.Controls.Add(this.chkGenerateVouchers );

                this.Controls.Add(this.lblVoucherNO );
this.Controls.Add(this.txtVoucherNO );

                    
            this.Name = "tb_PurReturnEntryQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurReEntryNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurReEntryNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurEntryRe_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPurEntryRe_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurEntryReNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurEntryReNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBillDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpBillDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingWay;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingWay;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTrackNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTrackNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeposit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDeposit;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalDiscountAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalDiscountAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReceiptInvoiceClosed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkReceiptInvoiceClosed;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGenerateVouchers;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGenerateVouchers;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVoucherNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtVoucherNO;

    
    
   
 





    }
}


