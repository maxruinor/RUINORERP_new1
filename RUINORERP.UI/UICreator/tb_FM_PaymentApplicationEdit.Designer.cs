// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/06/2025 13:58:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 付款申请单-目前代替纸的申请单将来完善明细则用付款单的主子表来完成系统可以根据客户来自动生成经人确认
    /// </summary>
    partial class tb_FM_PaymentApplicationEdit
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
            this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCurrency_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbAccount_id = new Krypton.Toolkit.KryptonComboBox();
            this.lblIsAdvancePayment = new Krypton.Toolkit.KryptonLabel();
            this.chkIsAdvancePayment = new Krypton.Toolkit.KryptonCheckBox();
            this.lblPrePaymentBill_id = new Krypton.Toolkit.KryptonLabel();
            this.txtPrePaymentBill_id = new Krypton.Toolkit.KryptonTextBox();
            this.lblPayReasonItems = new Krypton.Toolkit.KryptonLabel();
            this.txtPayReasonItems = new Krypton.Toolkit.KryptonTextBox();
            this.lblInvoiceDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpInvoiceDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblPaymentDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpPaymentDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblOverpaymentAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtOverpaymentAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
            this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
            this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
            this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
            this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
            this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();
            this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
            this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();
            this.lblCloseCaseImagePath = new Krypton.Toolkit.KryptonLabel();
            this.txtCloseCaseImagePath = new Krypton.Toolkit.KryptonTextBox();
            this.lblCloseCaseOpinions = new Krypton.Toolkit.KryptonLabel();
            this.txtCloseCaseOpinions = new Krypton.Toolkit.KryptonTextBox();
            this.txtPayeeAccountNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblPayeeAccountNo = new Krypton.Toolkit.KryptonLabel();
            this.cmbPayeeInfoID = new Krypton.Toolkit.KryptonComboBox();
            this.lblPayeeInfoID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();
            this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
            this.txtApplicationNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblApplicationNo = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPayeeInfoID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartmentID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCurrency_ID
            // 
            this.lblCurrency_ID.Location = new System.Drawing.Point(100, 175);
            this.lblCurrency_ID.Name = "lblCurrency_ID";
            this.lblCurrency_ID.Size = new System.Drawing.Size(36, 20);
            this.lblCurrency_ID.TabIndex = 7;
            this.lblCurrency_ID.Values.Text = "币别";
            // 
            // cmbCurrency_ID
            // 
            this.cmbCurrency_ID.DropDownWidth = 100;
            this.cmbCurrency_ID.IntegralHeight = false;
            this.cmbCurrency_ID.Location = new System.Drawing.Point(173, 171);
            this.cmbCurrency_ID.Name = "cmbCurrency_ID";
            this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbCurrency_ID.TabIndex = 7;
            // 
            // lblAccount_id
            // 
            this.lblAccount_id.Location = new System.Drawing.Point(100, 200);
            this.lblAccount_id.Name = "lblAccount_id";
            this.lblAccount_id.Size = new System.Drawing.Size(62, 20);
            this.lblAccount_id.TabIndex = 8;
            this.lblAccount_id.Values.Text = "付款账户";
            // 
            // cmbAccount_id
            // 
            this.cmbAccount_id.DropDownWidth = 100;
            this.cmbAccount_id.IntegralHeight = false;
            this.cmbAccount_id.Location = new System.Drawing.Point(173, 196);
            this.cmbAccount_id.Name = "cmbAccount_id";
            this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
            this.cmbAccount_id.TabIndex = 8;
            // 
            // lblIsAdvancePayment
            // 
            this.lblIsAdvancePayment.Location = new System.Drawing.Point(100, 225);
            this.lblIsAdvancePayment.Name = "lblIsAdvancePayment";
            this.lblIsAdvancePayment.Size = new System.Drawing.Size(62, 20);
            this.lblIsAdvancePayment.TabIndex = 9;
            this.lblIsAdvancePayment.Values.Text = "为预付款";
            // 
            // chkIsAdvancePayment
            // 
            this.chkIsAdvancePayment.Location = new System.Drawing.Point(173, 221);
            this.chkIsAdvancePayment.Name = "chkIsAdvancePayment";
            this.chkIsAdvancePayment.Size = new System.Drawing.Size(19, 13);
            this.chkIsAdvancePayment.TabIndex = 9;
            this.chkIsAdvancePayment.Values.Text = "";
            // 
            // lblPrePaymentBill_id
            // 
            this.lblPrePaymentBill_id.Location = new System.Drawing.Point(100, 250);
            this.lblPrePaymentBill_id.Name = "lblPrePaymentBill_id";
            this.lblPrePaymentBill_id.Size = new System.Drawing.Size(49, 20);
            this.lblPrePaymentBill_id.TabIndex = 10;
            this.lblPrePaymentBill_id.Values.Text = "预付单";
            // 
            // txtPrePaymentBill_id
            // 
            this.txtPrePaymentBill_id.Location = new System.Drawing.Point(173, 246);
            this.txtPrePaymentBill_id.Name = "txtPrePaymentBill_id";
            this.txtPrePaymentBill_id.Size = new System.Drawing.Size(100, 23);
            this.txtPrePaymentBill_id.TabIndex = 10;
            // 
            // lblPayReasonItems
            // 
            this.lblPayReasonItems.Location = new System.Drawing.Point(100, 275);
            this.lblPayReasonItems.Name = "lblPayReasonItems";
            this.lblPayReasonItems.Size = new System.Drawing.Size(62, 20);
            this.lblPayReasonItems.TabIndex = 11;
            this.lblPayReasonItems.Values.Text = "付款项目";
            // 
            // txtPayReasonItems
            // 
            this.txtPayReasonItems.Location = new System.Drawing.Point(173, 271);
            this.txtPayReasonItems.Multiline = true;
            this.txtPayReasonItems.Name = "txtPayReasonItems";
            this.txtPayReasonItems.Size = new System.Drawing.Size(100, 21);
            this.txtPayReasonItems.TabIndex = 11;
            // 
            // lblInvoiceDate
            // 
            this.lblInvoiceDate.Location = new System.Drawing.Point(100, 300);
            this.lblInvoiceDate.Name = "lblInvoiceDate";
            this.lblInvoiceDate.Size = new System.Drawing.Size(62, 20);
            this.lblInvoiceDate.TabIndex = 12;
            this.lblInvoiceDate.Values.Text = "对账日期";
            // 
            // dtpInvoiceDate
            // 
            this.dtpInvoiceDate.Location = new System.Drawing.Point(173, 296);
            this.dtpInvoiceDate.Name = "dtpInvoiceDate";
            this.dtpInvoiceDate.ShowCheckBox = true;
            this.dtpInvoiceDate.Size = new System.Drawing.Size(100, 21);
            this.dtpInvoiceDate.TabIndex = 12;
            // 
            // lblPaymentDate
            // 
            this.lblPaymentDate.Location = new System.Drawing.Point(100, 325);
            this.lblPaymentDate.Name = "lblPaymentDate";
            this.lblPaymentDate.Size = new System.Drawing.Size(62, 20);
            this.lblPaymentDate.TabIndex = 13;
            this.lblPaymentDate.Values.Text = "付款日期";
            // 
            // dtpPaymentDate
            // 
            this.dtpPaymentDate.Location = new System.Drawing.Point(173, 321);
            this.dtpPaymentDate.Name = "dtpPaymentDate";
            this.dtpPaymentDate.ShowCheckBox = true;
            this.dtpPaymentDate.Size = new System.Drawing.Size(100, 21);
            this.dtpPaymentDate.TabIndex = 13;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(100, 350);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 14;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(173, 346);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(100, 21);
            this.txtNotes.TabIndex = 14;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Location = new System.Drawing.Point(100, 375);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(62, 20);
            this.lblTotalAmount.TabIndex = 15;
            this.lblTotalAmount.Values.Text = "付款金额";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Location = new System.Drawing.Point(173, 371);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(100, 23);
            this.txtTotalAmount.TabIndex = 15;
            // 
            // lblOverpaymentAmount
            // 
            this.lblOverpaymentAmount.Location = new System.Drawing.Point(100, 400);
            this.lblOverpaymentAmount.Name = "lblOverpaymentAmount";
            this.lblOverpaymentAmount.Size = new System.Drawing.Size(62, 20);
            this.lblOverpaymentAmount.TabIndex = 16;
            this.lblOverpaymentAmount.Values.Text = "超付金额";
            // 
            // txtOverpaymentAmount
            // 
            this.txtOverpaymentAmount.Location = new System.Drawing.Point(173, 396);
            this.txtOverpaymentAmount.Name = "txtOverpaymentAmount";
            this.txtOverpaymentAmount.Size = new System.Drawing.Size(100, 23);
            this.txtOverpaymentAmount.TabIndex = 16;
            // 
            // lblApprovalOpinions
            // 
            this.lblApprovalOpinions.Location = new System.Drawing.Point(487, 204);
            this.lblApprovalOpinions.Name = "lblApprovalOpinions";
            this.lblApprovalOpinions.Size = new System.Drawing.Size(62, 20);
            this.lblApprovalOpinions.TabIndex = 22;
            this.lblApprovalOpinions.Values.Text = "审批意见";
            // 
            // txtApprovalOpinions
            // 
            this.txtApprovalOpinions.Location = new System.Drawing.Point(560, 200);
            this.txtApprovalOpinions.Multiline = true;
            this.txtApprovalOpinions.Name = "txtApprovalOpinions";
            this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
            this.txtApprovalOpinions.TabIndex = 22;
            // 
            // lblApprover_by
            // 
            this.lblApprover_by.Location = new System.Drawing.Point(487, 229);
            this.lblApprover_by.Name = "lblApprover_by";
            this.lblApprover_by.Size = new System.Drawing.Size(49, 20);
            this.lblApprover_by.TabIndex = 23;
            this.lblApprover_by.Values.Text = "审批人";
            // 
            // txtApprover_by
            // 
            this.txtApprover_by.Location = new System.Drawing.Point(560, 225);
            this.txtApprover_by.Name = "txtApprover_by";
            this.txtApprover_by.Size = new System.Drawing.Size(100, 23);
            this.txtApprover_by.TabIndex = 23;
            // 
            // lblApprover_at
            // 
            this.lblApprover_at.Location = new System.Drawing.Point(487, 254);
            this.lblApprover_at.Name = "lblApprover_at";
            this.lblApprover_at.Size = new System.Drawing.Size(62, 20);
            this.lblApprover_at.TabIndex = 24;
            this.lblApprover_at.Values.Text = "审批时间";
            // 
            // dtpApprover_at
            // 
            this.dtpApprover_at.Location = new System.Drawing.Point(560, 250);
            this.dtpApprover_at.Name = "dtpApprover_at";
            this.dtpApprover_at.ShowCheckBox = true;
            this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
            this.dtpApprover_at.TabIndex = 24;
            // 
            // lblApprovalResults
            // 
            this.lblApprovalResults.Location = new System.Drawing.Point(487, 304);
            this.lblApprovalResults.Name = "lblApprovalResults";
            this.lblApprovalResults.Size = new System.Drawing.Size(62, 20);
            this.lblApprovalResults.TabIndex = 26;
            this.lblApprovalResults.Values.Text = "审批结果";
            // 
            // chkApprovalResults
            // 
            this.chkApprovalResults.Location = new System.Drawing.Point(560, 300);
            this.chkApprovalResults.Name = "chkApprovalResults";
            this.chkApprovalResults.Size = new System.Drawing.Size(19, 13);
            this.chkApprovalResults.TabIndex = 26;
            this.chkApprovalResults.Values.Text = "";
            // 
            // lblDataStatus
            // 
            this.lblDataStatus.Location = new System.Drawing.Point(487, 329);
            this.lblDataStatus.Name = "lblDataStatus";
            this.lblDataStatus.Size = new System.Drawing.Size(62, 20);
            this.lblDataStatus.TabIndex = 27;
            this.lblDataStatus.Values.Text = "数据状态";
            // 
            // txtDataStatus
            // 
            this.txtDataStatus.Location = new System.Drawing.Point(560, 325);
            this.txtDataStatus.Name = "txtDataStatus";
            this.txtDataStatus.Size = new System.Drawing.Size(100, 23);
            this.txtDataStatus.TabIndex = 27;
            // 
            // lblPrintStatus
            // 
            this.lblPrintStatus.Location = new System.Drawing.Point(487, 354);
            this.lblPrintStatus.Name = "lblPrintStatus";
            this.lblPrintStatus.Size = new System.Drawing.Size(62, 20);
            this.lblPrintStatus.TabIndex = 28;
            this.lblPrintStatus.Values.Text = "打印状态";
            // 
            // txtPrintStatus
            // 
            this.txtPrintStatus.Location = new System.Drawing.Point(560, 350);
            this.txtPrintStatus.Name = "txtPrintStatus";
            this.txtPrintStatus.Size = new System.Drawing.Size(100, 23);
            this.txtPrintStatus.TabIndex = 28;
            // 
            // lblCloseCaseImagePath
            // 
            this.lblCloseCaseImagePath.Location = new System.Drawing.Point(487, 379);
            this.lblCloseCaseImagePath.Name = "lblCloseCaseImagePath";
            this.lblCloseCaseImagePath.Size = new System.Drawing.Size(62, 20);
            this.lblCloseCaseImagePath.TabIndex = 29;
            this.lblCloseCaseImagePath.Values.Text = "结案凭证";
            // 
            // txtCloseCaseImagePath
            // 
            this.txtCloseCaseImagePath.Location = new System.Drawing.Point(560, 375);
            this.txtCloseCaseImagePath.Multiline = true;
            this.txtCloseCaseImagePath.Name = "txtCloseCaseImagePath";
            this.txtCloseCaseImagePath.Size = new System.Drawing.Size(100, 21);
            this.txtCloseCaseImagePath.TabIndex = 29;
            // 
            // lblCloseCaseOpinions
            // 
            this.lblCloseCaseOpinions.Location = new System.Drawing.Point(487, 404);
            this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
            this.lblCloseCaseOpinions.Size = new System.Drawing.Size(62, 20);
            this.lblCloseCaseOpinions.TabIndex = 30;
            this.lblCloseCaseOpinions.Values.Text = "结案意见";
            // 
            // txtCloseCaseOpinions
            // 
            this.txtCloseCaseOpinions.Location = new System.Drawing.Point(560, 400);
            this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
            this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 23);
            this.txtCloseCaseOpinions.TabIndex = 30;
            // 
            // txtPayeeAccountNo
            // 
            this.txtPayeeAccountNo.Location = new System.Drawing.Point(173, 146);
            this.txtPayeeAccountNo.Name = "txtPayeeAccountNo";
            this.txtPayeeAccountNo.Size = new System.Drawing.Size(100, 23);
            this.txtPayeeAccountNo.TabIndex = 6;
            // 
            // lblPayeeAccountNo
            // 
            this.lblPayeeAccountNo.Location = new System.Drawing.Point(100, 150);
            this.lblPayeeAccountNo.Name = "lblPayeeAccountNo";
            this.lblPayeeAccountNo.Size = new System.Drawing.Size(62, 20);
            this.lblPayeeAccountNo.TabIndex = 6;
            this.lblPayeeAccountNo.Values.Text = "收款账号";
            // 
            // cmbPayeeInfoID
            // 
            this.cmbPayeeInfoID.DropDownWidth = 100;
            this.cmbPayeeInfoID.IntegralHeight = false;
            this.cmbPayeeInfoID.Location = new System.Drawing.Point(173, 121);
            this.cmbPayeeInfoID.Name = "cmbPayeeInfoID";
            this.cmbPayeeInfoID.Size = new System.Drawing.Size(100, 21);
            this.cmbPayeeInfoID.TabIndex = 5;
            // 
            // lblPayeeInfoID
            // 
            this.lblPayeeInfoID.Location = new System.Drawing.Point(100, 125);
            this.lblPayeeInfoID.Name = "lblPayeeInfoID";
            this.lblPayeeInfoID.Size = new System.Drawing.Size(62, 20);
            this.lblPayeeInfoID.TabIndex = 5;
            this.lblPayeeInfoID.Values.Text = "收款信息";
            // 
            // cmbCustomerVendor_ID
            // 
            this.cmbCustomerVendor_ID.DropDownWidth = 100;
            this.cmbCustomerVendor_ID.IntegralHeight = false;
            this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173, 96);
            this.cmbCustomerVendor_ID.Name = "cmbCustomerVendor_ID";
            this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbCustomerVendor_ID.TabIndex = 4;
            // 
            // lblCustomerVendor_ID
            // 
            this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100, 100);
            this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
            this.lblCustomerVendor_ID.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerVendor_ID.TabIndex = 4;
            this.lblCustomerVendor_ID.Values.Text = "收款单位";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(173, 71);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbEmployee_ID.TabIndex = 3;
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(100, 75);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(49, 20);
            this.lblEmployee_ID.TabIndex = 3;
            this.lblEmployee_ID.Values.Text = "申请人";
            // 
            // cmbDepartmentID
            // 
            this.cmbDepartmentID.DropDownWidth = 100;
            this.cmbDepartmentID.IntegralHeight = false;
            this.cmbDepartmentID.Location = new System.Drawing.Point(173, 46);
            this.cmbDepartmentID.Name = "cmbDepartmentID";
            this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
            this.cmbDepartmentID.TabIndex = 2;
            // 
            // lblDepartmentID
            // 
            this.lblDepartmentID.Location = new System.Drawing.Point(100, 50);
            this.lblDepartmentID.Name = "lblDepartmentID";
            this.lblDepartmentID.Size = new System.Drawing.Size(36, 20);
            this.lblDepartmentID.TabIndex = 2;
            this.lblDepartmentID.Values.Text = "部门";
            // 
            // txtApplicationNo
            // 
            this.txtApplicationNo.Location = new System.Drawing.Point(173, 21);
            this.txtApplicationNo.Name = "txtApplicationNo";
            this.txtApplicationNo.Size = new System.Drawing.Size(100, 23);
            this.txtApplicationNo.TabIndex = 1;
            // 
            // lblApplicationNo
            // 
            this.lblApplicationNo.Location = new System.Drawing.Point(100, 25);
            this.lblApplicationNo.Name = "lblApplicationNo";
            this.lblApplicationNo.Size = new System.Drawing.Size(62, 20);
            this.lblApplicationNo.TabIndex = 1;
            this.lblApplicationNo.Values.Text = "申请单号";
            // 
            // tb_FM_PaymentApplicationEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblApplicationNo);
            this.Controls.Add(this.txtApplicationNo);
            this.Controls.Add(this.lblDepartmentID);
            this.Controls.Add(this.cmbDepartmentID);
            this.Controls.Add(this.lblEmployee_ID);
            this.Controls.Add(this.cmbEmployee_ID);
            this.Controls.Add(this.lblCustomerVendor_ID);
            this.Controls.Add(this.cmbCustomerVendor_ID);
            this.Controls.Add(this.lblPayeeInfoID);
            this.Controls.Add(this.cmbPayeeInfoID);
            this.Controls.Add(this.lblPayeeAccountNo);
            this.Controls.Add(this.txtPayeeAccountNo);
            this.Controls.Add(this.lblCurrency_ID);
            this.Controls.Add(this.cmbCurrency_ID);
            this.Controls.Add(this.lblAccount_id);
            this.Controls.Add(this.cmbAccount_id);
            this.Controls.Add(this.lblIsAdvancePayment);
            this.Controls.Add(this.chkIsAdvancePayment);
            this.Controls.Add(this.lblPrePaymentBill_id);
            this.Controls.Add(this.txtPrePaymentBill_id);
            this.Controls.Add(this.lblPayReasonItems);
            this.Controls.Add(this.txtPayReasonItems);
            this.Controls.Add(this.lblInvoiceDate);
            this.Controls.Add(this.dtpInvoiceDate);
            this.Controls.Add(this.lblPaymentDate);
            this.Controls.Add(this.dtpPaymentDate);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.txtTotalAmount);
            this.Controls.Add(this.lblOverpaymentAmount);
            this.Controls.Add(this.txtOverpaymentAmount);
            this.Controls.Add(this.lblApprovalOpinions);
            this.Controls.Add(this.txtApprovalOpinions);
            this.Controls.Add(this.lblApprover_by);
            this.Controls.Add(this.txtApprover_by);
            this.Controls.Add(this.lblApprover_at);
            this.Controls.Add(this.dtpApprover_at);
            this.Controls.Add(this.lblApprovalResults);
            this.Controls.Add(this.chkApprovalResults);
            this.Controls.Add(this.lblDataStatus);
            this.Controls.Add(this.txtDataStatus);
            this.Controls.Add(this.lblPrintStatus);
            this.Controls.Add(this.txtPrintStatus);
            this.Controls.Add(this.lblCloseCaseImagePath);
            this.Controls.Add(this.txtCloseCaseImagePath);
            this.Controls.Add(this.lblCloseCaseOpinions);
            this.Controls.Add(this.txtCloseCaseOpinions);
            this.Name = "tb_FM_PaymentApplicationEdit";
            this.Size = new System.Drawing.Size(911, 832);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPayeeInfoID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartmentID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start






    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsAdvancePayment;
private Krypton.Toolkit.KryptonCheckBox chkIsAdvancePayment;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrePaymentBill_id;
private Krypton.Toolkit.KryptonTextBox txtPrePaymentBill_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayReasonItems;
private Krypton.Toolkit.KryptonTextBox txtPayReasonItems;

    
        
              private Krypton.Toolkit.KryptonLabel lblInvoiceDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpInvoiceDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaymentDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPaymentDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblOverpaymentAmount;
private Krypton.Toolkit.KryptonTextBox txtOverpaymentAmount;






    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblCloseCaseImagePath;
private Krypton.Toolkit.KryptonTextBox txtCloseCaseImagePath;

    
        
              private Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;
        private Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;
        private Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
        private Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;
        private Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
        private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;
        private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
        private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
        private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;
        private Krypton.Toolkit.KryptonLabel lblDepartmentID;
        private Krypton.Toolkit.KryptonTextBox txtApplicationNo;
        private Krypton.Toolkit.KryptonLabel lblApplicationNo;


        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;







    }
}

