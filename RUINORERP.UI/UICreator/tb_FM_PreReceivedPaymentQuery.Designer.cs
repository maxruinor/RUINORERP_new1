
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 18:12:13
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    partial class tb_FM_PreReceivedPaymentQuery
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
     
     this.lblPreRPNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPreRPNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPrePayDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPrePayDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblPrePaymentReason = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrePaymentReason = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblForeignPrepaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignPrepaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalPrepaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalPrepaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalPrepaidAmountInWords = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalPrepaidAmountInWords = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblPaymentImagePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPaymentImagePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtPaymentImagePath.Multiline = true;

this.lblRemark = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemark = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRemark.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30PreRPNO###String
this.lblPreRPNO.AutoSize = true;
this.lblPreRPNO.Location = new System.Drawing.Point(100,25);
this.lblPreRPNO.Name = "lblPreRPNO";
this.lblPreRPNO.Size = new System.Drawing.Size(41, 12);
this.lblPreRPNO.TabIndex = 1;
this.lblPreRPNO.Text = "单据编号";
this.txtPreRPNO.Location = new System.Drawing.Point(173,21);
this.txtPreRPNO.Name = "txtPreRPNO";
this.txtPreRPNO.Size = new System.Drawing.Size(100, 21);
this.txtPreRPNO.TabIndex = 1;
this.Controls.Add(this.lblPreRPNO);
this.Controls.Add(this.txtPreRPNO);

           //#####Account_id###Int64
//属性测试50Account_id
//属性测试50Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,50);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 2;
this.lblAccount_id.Text = "收付款账户";
//111======50
this.cmbAccount_id.Location = new System.Drawing.Point(173,46);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 2;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####CustomerVendor_ID###Int64
//属性测试75CustomerVendor_ID
//属性测试75CustomerVendor_ID
//属性测试75CustomerVendor_ID
//属性测试75CustomerVendor_ID
//属性测试75CustomerVendor_ID
//属性测试75CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,75);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 3;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======75
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,71);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 3;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####PayeeInfoID###Int64
//属性测试100PayeeInfoID
//属性测试100PayeeInfoID
//属性测试100PayeeInfoID
this.lblPayeeInfoID.AutoSize = true;
this.lblPayeeInfoID.Location = new System.Drawing.Point(100,100);
this.lblPayeeInfoID.Name = "lblPayeeInfoID";
this.lblPayeeInfoID.Size = new System.Drawing.Size(41, 12);
this.lblPayeeInfoID.TabIndex = 4;
this.lblPayeeInfoID.Text = "收款信息";
//111======100
this.cmbPayeeInfoID.Location = new System.Drawing.Point(173,96);
this.cmbPayeeInfoID.Name ="cmbPayeeInfoID";
this.cmbPayeeInfoID.Size = new System.Drawing.Size(100, 21);
this.cmbPayeeInfoID.TabIndex = 4;
this.Controls.Add(this.lblPayeeInfoID);
this.Controls.Add(this.cmbPayeeInfoID);

           //#####100PayeeAccountNo###String
this.lblPayeeAccountNo.AutoSize = true;
this.lblPayeeAccountNo.Location = new System.Drawing.Point(100,125);
this.lblPayeeAccountNo.Name = "lblPayeeAccountNo";
this.lblPayeeAccountNo.Size = new System.Drawing.Size(41, 12);
this.lblPayeeAccountNo.TabIndex = 5;
this.lblPayeeAccountNo.Text = "收款账号";
this.txtPayeeAccountNo.Location = new System.Drawing.Point(173,121);
this.txtPayeeAccountNo.Name = "txtPayeeAccountNo";
this.txtPayeeAccountNo.Size = new System.Drawing.Size(100, 21);
this.txtPayeeAccountNo.TabIndex = 5;
this.Controls.Add(this.lblPayeeAccountNo);
this.Controls.Add(this.txtPayeeAccountNo);

           //#####Employee_ID###Int64
//属性测试150Employee_ID
//属性测试150Employee_ID
//属性测试150Employee_ID
//属性测试150Employee_ID
//属性测试150Employee_ID
//属性测试150Employee_ID
//属性测试150Employee_ID
//属性测试150Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,150);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 6;
this.lblEmployee_ID.Text = "经办人";
//111======150
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,146);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 6;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####DepartmentID###Int64
//属性测试175DepartmentID
//属性测试175DepartmentID
//属性测试175DepartmentID
//属性测试175DepartmentID
//属性测试175DepartmentID
//属性测试175DepartmentID
//属性测试175DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,175);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 7;
this.lblDepartmentID.Text = "部门";
//111======175
this.cmbDepartmentID.Location = new System.Drawing.Point(173,171);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 7;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ProjectGroup_ID###Int64
//属性测试200ProjectGroup_ID
//属性测试200ProjectGroup_ID
//属性测试200ProjectGroup_ID
//属性测试200ProjectGroup_ID
//属性测试200ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,200);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 8;
this.lblProjectGroup_ID.Text = "项目组";
//111======200
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,196);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 8;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####Paytype_ID###Int64
//属性测试225Paytype_ID
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

           //#####Currency_ID###Int64
//属性测试250Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,250);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 10;
this.lblCurrency_ID.Text = "币别";
//111======250
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,246);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 10;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,275);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 11;
this.lblExchangeRate.Text = "汇率";
//111======275
this.txtExchangeRate.Location = new System.Drawing.Point(173,271);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 11;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####PrePayDate###DateTime
this.lblPrePayDate.AutoSize = true;
this.lblPrePayDate.Location = new System.Drawing.Point(100,300);
this.lblPrePayDate.Name = "lblPrePayDate";
this.lblPrePayDate.Size = new System.Drawing.Size(41, 12);
this.lblPrePayDate.TabIndex = 12;
this.lblPrePayDate.Text = "付款日期";
//111======300
this.dtpPrePayDate.Location = new System.Drawing.Point(173,296);
this.dtpPrePayDate.Name ="dtpPrePayDate";
this.dtpPrePayDate.Size = new System.Drawing.Size(100, 21);
this.dtpPrePayDate.TabIndex = 12;
this.Controls.Add(this.lblPrePayDate);
this.Controls.Add(this.dtpPrePayDate);

           //#####200PrePaymentReason###String
this.lblPrePaymentReason.AutoSize = true;
this.lblPrePaymentReason.Location = new System.Drawing.Point(100,325);
this.lblPrePaymentReason.Name = "lblPrePaymentReason";
this.lblPrePaymentReason.Size = new System.Drawing.Size(41, 12);
this.lblPrePaymentReason.TabIndex = 13;
this.lblPrePaymentReason.Text = "事由";
this.txtPrePaymentReason.Location = new System.Drawing.Point(173,321);
this.txtPrePaymentReason.Name = "txtPrePaymentReason";
this.txtPrePaymentReason.Size = new System.Drawing.Size(100, 21);
this.txtPrePaymentReason.TabIndex = 13;
this.Controls.Add(this.lblPrePaymentReason);
this.Controls.Add(this.txtPrePaymentReason);

           //#####SourceBill_BizType###Int32
//属性测试350SourceBill_BizType
//属性测试350SourceBill_BizType
//属性测试350SourceBill_BizType
//属性测试350SourceBill_BizType
//属性测试350SourceBill_BizType
//属性测试350SourceBill_BizType
//属性测试350SourceBill_BizType
//属性测试350SourceBill_BizType

           //#####SourceBill_ID###Int64
//属性测试375SourceBill_ID
//属性测试375SourceBill_ID
//属性测试375SourceBill_ID
//属性测试375SourceBill_ID
//属性测试375SourceBill_ID
//属性测试375SourceBill_ID
//属性测试375SourceBill_ID
//属性测试375SourceBill_ID

           //#####30SourceBillNO###String
this.lblSourceBillNO.AutoSize = true;
this.lblSourceBillNO.Location = new System.Drawing.Point(100,400);
this.lblSourceBillNO.Name = "lblSourceBillNO";
this.lblSourceBillNO.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNO.TabIndex = 16;
this.lblSourceBillNO.Text = "来源单号";
this.txtSourceBillNO.Location = new System.Drawing.Point(173,396);
this.txtSourceBillNO.Name = "txtSourceBillNO";
this.txtSourceBillNO.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNO.TabIndex = 16;
this.Controls.Add(this.lblSourceBillNO);
this.Controls.Add(this.txtSourceBillNO);

           //#####FMPaymentStatus###Int32
//属性测试425FMPaymentStatus
//属性测试425FMPaymentStatus
//属性测试425FMPaymentStatus
//属性测试425FMPaymentStatus
//属性测试425FMPaymentStatus
//属性测试425FMPaymentStatus
//属性测试425FMPaymentStatus
//属性测试425FMPaymentStatus

           //#####ForeignPrepaidAmount###Decimal
this.lblForeignPrepaidAmount.AutoSize = true;
this.lblForeignPrepaidAmount.Location = new System.Drawing.Point(100,450);
this.lblForeignPrepaidAmount.Name = "lblForeignPrepaidAmount";
this.lblForeignPrepaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignPrepaidAmount.TabIndex = 18;
this.lblForeignPrepaidAmount.Text = "预定金额外币";
//111======450
this.txtForeignPrepaidAmount.Location = new System.Drawing.Point(173,446);
this.txtForeignPrepaidAmount.Name ="txtForeignPrepaidAmount";
this.txtForeignPrepaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignPrepaidAmount.TabIndex = 18;
this.Controls.Add(this.lblForeignPrepaidAmount);
this.Controls.Add(this.txtForeignPrepaidAmount);

           //#####LocalPrepaidAmount###Decimal
this.lblLocalPrepaidAmount.AutoSize = true;
this.lblLocalPrepaidAmount.Location = new System.Drawing.Point(100,475);
this.lblLocalPrepaidAmount.Name = "lblLocalPrepaidAmount";
this.lblLocalPrepaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalPrepaidAmount.TabIndex = 19;
this.lblLocalPrepaidAmount.Text = "预定金额本币";
//111======475
this.txtLocalPrepaidAmount.Location = new System.Drawing.Point(173,471);
this.txtLocalPrepaidAmount.Name ="txtLocalPrepaidAmount";
this.txtLocalPrepaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalPrepaidAmount.TabIndex = 19;
this.Controls.Add(this.lblLocalPrepaidAmount);
this.Controls.Add(this.txtLocalPrepaidAmount);

           //#####150LocalPrepaidAmountInWords###String
this.lblLocalPrepaidAmountInWords.AutoSize = true;
this.lblLocalPrepaidAmountInWords.Location = new System.Drawing.Point(100,500);
this.lblLocalPrepaidAmountInWords.Name = "lblLocalPrepaidAmountInWords";
this.lblLocalPrepaidAmountInWords.Size = new System.Drawing.Size(41, 12);
this.lblLocalPrepaidAmountInWords.TabIndex = 20;
this.lblLocalPrepaidAmountInWords.Text = "大写预定金额本币";
this.txtLocalPrepaidAmountInWords.Location = new System.Drawing.Point(173,496);
this.txtLocalPrepaidAmountInWords.Name = "txtLocalPrepaidAmountInWords";
this.txtLocalPrepaidAmountInWords.Size = new System.Drawing.Size(100, 21);
this.txtLocalPrepaidAmountInWords.TabIndex = 20;
this.Controls.Add(this.lblLocalPrepaidAmountInWords);
this.Controls.Add(this.txtLocalPrepaidAmountInWords);

           //#####ForeignPaidAmount###Decimal
this.lblForeignPaidAmount.AutoSize = true;
this.lblForeignPaidAmount.Location = new System.Drawing.Point(100,525);
this.lblForeignPaidAmount.Name = "lblForeignPaidAmount";
this.lblForeignPaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignPaidAmount.TabIndex = 21;
this.lblForeignPaidAmount.Text = "核销金额外币";
//111======525
this.txtForeignPaidAmount.Location = new System.Drawing.Point(173,521);
this.txtForeignPaidAmount.Name ="txtForeignPaidAmount";
this.txtForeignPaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignPaidAmount.TabIndex = 21;
this.Controls.Add(this.lblForeignPaidAmount);
this.Controls.Add(this.txtForeignPaidAmount);

           //#####LocalPaidAmount###Decimal
this.lblLocalPaidAmount.AutoSize = true;
this.lblLocalPaidAmount.Location = new System.Drawing.Point(100,550);
this.lblLocalPaidAmount.Name = "lblLocalPaidAmount";
this.lblLocalPaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalPaidAmount.TabIndex = 22;
this.lblLocalPaidAmount.Text = "核销金额本币";
//111======550
this.txtLocalPaidAmount.Location = new System.Drawing.Point(173,546);
this.txtLocalPaidAmount.Name ="txtLocalPaidAmount";
this.txtLocalPaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalPaidAmount.TabIndex = 22;
this.Controls.Add(this.lblLocalPaidAmount);
this.Controls.Add(this.txtLocalPaidAmount);

           //#####ForeignBalanceAmount###Decimal
this.lblForeignBalanceAmount.AutoSize = true;
this.lblForeignBalanceAmount.Location = new System.Drawing.Point(100,575);
this.lblForeignBalanceAmount.Name = "lblForeignBalanceAmount";
this.lblForeignBalanceAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignBalanceAmount.TabIndex = 23;
this.lblForeignBalanceAmount.Text = "余额外币";
//111======575
this.txtForeignBalanceAmount.Location = new System.Drawing.Point(173,571);
this.txtForeignBalanceAmount.Name ="txtForeignBalanceAmount";
this.txtForeignBalanceAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignBalanceAmount.TabIndex = 23;
this.Controls.Add(this.lblForeignBalanceAmount);
this.Controls.Add(this.txtForeignBalanceAmount);

           //#####LocalBalanceAmount###Decimal
this.lblLocalBalanceAmount.AutoSize = true;
this.lblLocalBalanceAmount.Location = new System.Drawing.Point(100,600);
this.lblLocalBalanceAmount.Name = "lblLocalBalanceAmount";
this.lblLocalBalanceAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalBalanceAmount.TabIndex = 24;
this.lblLocalBalanceAmount.Text = "余额本币";
//111======600
this.txtLocalBalanceAmount.Location = new System.Drawing.Point(173,596);
this.txtLocalBalanceAmount.Name ="txtLocalBalanceAmount";
this.txtLocalBalanceAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalBalanceAmount.TabIndex = 24;
this.Controls.Add(this.lblLocalBalanceAmount);
this.Controls.Add(this.txtLocalBalanceAmount);

           //#####ReceivePaymentType###Int32
//属性测试625ReceivePaymentType
//属性测试625ReceivePaymentType
//属性测试625ReceivePaymentType
//属性测试625ReceivePaymentType
//属性测试625ReceivePaymentType
//属性测试625ReceivePaymentType
//属性测试625ReceivePaymentType
//属性测试625ReceivePaymentType

           //#####300PaymentImagePath###String
this.lblPaymentImagePath.AutoSize = true;
this.lblPaymentImagePath.Location = new System.Drawing.Point(100,650);
this.lblPaymentImagePath.Name = "lblPaymentImagePath";
this.lblPaymentImagePath.Size = new System.Drawing.Size(41, 12);
this.lblPaymentImagePath.TabIndex = 26;
this.lblPaymentImagePath.Text = "付款凭证";
this.txtPaymentImagePath.Location = new System.Drawing.Point(173,646);
this.txtPaymentImagePath.Name = "txtPaymentImagePath";
this.txtPaymentImagePath.Size = new System.Drawing.Size(100, 21);
this.txtPaymentImagePath.TabIndex = 26;
this.Controls.Add(this.lblPaymentImagePath);
this.Controls.Add(this.txtPaymentImagePath);

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,675);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 27;
this.lblRemark.Text = "备注";
this.txtRemark.Location = new System.Drawing.Point(173,671);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 27;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,700);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 28;
this.lblCreated_at.Text = "创建时间";
//111======700
this.dtpCreated_at.Location = new System.Drawing.Point(173,696);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 28;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,750);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 30;
this.lblModified_at.Text = "修改时间";
//111======750
this.dtpModified_at.Location = new System.Drawing.Point(173,746);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 30;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,800);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 32;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,796);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 32;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,825);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 33;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,821);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 33;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试850Approver_by
//属性测试850Approver_by
//属性测试850Approver_by
//属性测试850Approver_by
//属性测试850Approver_by
//属性测试850Approver_by
//属性测试850Approver_by
//属性测试850Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,875);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 35;
this.lblApprover_at.Text = "审批时间";
//111======875
this.dtpApprover_at.Location = new System.Drawing.Point(173,871);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 35;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,925);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 37;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,921);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 37;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试950PrintStatus
//属性测试950PrintStatus
//属性测试950PrintStatus
//属性测试950PrintStatus
//属性测试950PrintStatus
//属性测试950PrintStatus
//属性测试950PrintStatus
//属性测试950PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPreRPNO );
this.Controls.Add(this.txtPreRPNO );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblPayeeInfoID );
this.Controls.Add(this.cmbPayeeInfoID );

                this.Controls.Add(this.lblPayeeAccountNo );
this.Controls.Add(this.txtPayeeAccountNo );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblPrePayDate );
this.Controls.Add(this.dtpPrePayDate );

                this.Controls.Add(this.lblPrePaymentReason );
this.Controls.Add(this.txtPrePaymentReason );

                
                
                this.Controls.Add(this.lblSourceBillNO );
this.Controls.Add(this.txtSourceBillNO );

                
                this.Controls.Add(this.lblForeignPrepaidAmount );
this.Controls.Add(this.txtForeignPrepaidAmount );

                this.Controls.Add(this.lblLocalPrepaidAmount );
this.Controls.Add(this.txtLocalPrepaidAmount );

                this.Controls.Add(this.lblLocalPrepaidAmountInWords );
this.Controls.Add(this.txtLocalPrepaidAmountInWords );

                this.Controls.Add(this.lblForeignPaidAmount );
this.Controls.Add(this.txtForeignPaidAmount );

                this.Controls.Add(this.lblLocalPaidAmount );
this.Controls.Add(this.txtLocalPaidAmount );

                this.Controls.Add(this.lblForeignBalanceAmount );
this.Controls.Add(this.txtForeignBalanceAmount );

                this.Controls.Add(this.lblLocalBalanceAmount );
this.Controls.Add(this.txtLocalBalanceAmount );

                
                this.Controls.Add(this.lblPaymentImagePath );
this.Controls.Add(this.txtPaymentImagePath );

                this.Controls.Add(this.lblRemark );
this.Controls.Add(this.txtRemark );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                    
            this.Name = "tb_FM_PreReceivedPaymentQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPreRPNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPreRPNO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrePayDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPrePayDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrePaymentReason;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrePaymentReason;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNO;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignPrepaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignPrepaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalPrepaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalPrepaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalPrepaidAmountInWords;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalPrepaidAmountInWords;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignPaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignPaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalPaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalPaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignBalanceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignBalanceAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalBalanceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalBalanceAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentImagePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPaymentImagePath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemark;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemark;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
    
   
 





    }
}


