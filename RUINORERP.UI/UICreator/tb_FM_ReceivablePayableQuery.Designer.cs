
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:34
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 应收应付表
    /// </summary>
    partial class tb_FM_ReceivablePayableQuery
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
     
     this.lblARAPNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtARAPNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblIsExpenseType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsExpenseType = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsExpenseType.Values.Text ="";

this.lblIsFromPlatform = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsFromPlatform = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsFromPlatform.Values.Text ="";

this.lblPlatformOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPlatformOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblShippingFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingFee = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalForeignPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalForeignPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalLocalPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalLocalPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDueDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDueDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblInvoiceId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbInvoiceId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblInvoiced = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkInvoiced = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkInvoiced.Values.Text ="";

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblTaxTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedTotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedTotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


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
                 //#####30ARAPNo###String
this.lblARAPNo.AutoSize = true;
this.lblARAPNo.Location = new System.Drawing.Point(100,25);
this.lblARAPNo.Name = "lblARAPNo";
this.lblARAPNo.Size = new System.Drawing.Size(41, 12);
this.lblARAPNo.TabIndex = 1;
this.lblARAPNo.Text = "单据编号";
this.txtARAPNo.Location = new System.Drawing.Point(173,21);
this.txtARAPNo.Name = "txtARAPNo";
this.txtARAPNo.Size = new System.Drawing.Size(100, 21);
this.txtARAPNo.TabIndex = 1;
this.Controls.Add(this.lblARAPNo);
this.Controls.Add(this.txtARAPNo);

           //#####SourceBizType###Int32
//属性测试50SourceBizType
//属性测试50SourceBizType
//属性测试50SourceBizType
//属性测试50SourceBizType
//属性测试50SourceBizType
//属性测试50SourceBizType
//属性测试50SourceBizType
//属性测试50SourceBizType

           //#####SourceBillId###Int64
//属性测试75SourceBillId
//属性测试75SourceBillId
//属性测试75SourceBillId
//属性测试75SourceBillId
//属性测试75SourceBillId
//属性测试75SourceBillId
//属性测试75SourceBillId
//属性测试75SourceBillId

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,100);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 4;
this.lblSourceBillNo.Text = "来源单号";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,96);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 4;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

           //#####CustomerVendor_ID###Int64
//属性测试125CustomerVendor_ID
//属性测试125CustomerVendor_ID
//属性测试125CustomerVendor_ID
//属性测试125CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,125);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 5;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======125
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,121);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 5;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####Currency_ID###Int64
//属性测试150Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,150);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 6;
this.lblCurrency_ID.Text = "币别";
//111======150
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,146);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 6;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####IsExpenseType###Boolean
this.lblIsExpenseType.AutoSize = true;
this.lblIsExpenseType.Location = new System.Drawing.Point(100,175);
this.lblIsExpenseType.Name = "lblIsExpenseType";
this.lblIsExpenseType.Size = new System.Drawing.Size(41, 12);
this.lblIsExpenseType.TabIndex = 7;
this.lblIsExpenseType.Text = "费用单据";
this.chkIsExpenseType.Location = new System.Drawing.Point(173,171);
this.chkIsExpenseType.Name = "chkIsExpenseType";
this.chkIsExpenseType.Size = new System.Drawing.Size(100, 21);
this.chkIsExpenseType.TabIndex = 7;
this.Controls.Add(this.lblIsExpenseType);
this.Controls.Add(this.chkIsExpenseType);

           //#####IsFromPlatform###Boolean
this.lblIsFromPlatform.AutoSize = true;
this.lblIsFromPlatform.Location = new System.Drawing.Point(100,200);
this.lblIsFromPlatform.Name = "lblIsFromPlatform";
this.lblIsFromPlatform.Size = new System.Drawing.Size(41, 12);
this.lblIsFromPlatform.TabIndex = 8;
this.lblIsFromPlatform.Text = "平台单";
this.chkIsFromPlatform.Location = new System.Drawing.Point(173,196);
this.chkIsFromPlatform.Name = "chkIsFromPlatform";
this.chkIsFromPlatform.Size = new System.Drawing.Size(100, 21);
this.chkIsFromPlatform.TabIndex = 8;
this.Controls.Add(this.lblIsFromPlatform);
this.Controls.Add(this.chkIsFromPlatform);

           //#####100PlatformOrderNo###String
this.lblPlatformOrderNo.AutoSize = true;
this.lblPlatformOrderNo.Location = new System.Drawing.Point(100,225);
this.lblPlatformOrderNo.Name = "lblPlatformOrderNo";
this.lblPlatformOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblPlatformOrderNo.TabIndex = 9;
this.lblPlatformOrderNo.Text = "平台单号";
this.txtPlatformOrderNo.Location = new System.Drawing.Point(173,221);
this.txtPlatformOrderNo.Name = "txtPlatformOrderNo";
this.txtPlatformOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtPlatformOrderNo.TabIndex = 9;
this.Controls.Add(this.lblPlatformOrderNo);
this.Controls.Add(this.txtPlatformOrderNo);

           //#####Account_id###Int64
//属性测试250Account_id
//属性测试250Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,250);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 10;
this.lblAccount_id.Text = "公司账户";
//111======250
this.cmbAccount_id.Location = new System.Drawing.Point(173,246);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 10;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####PayeeInfoID###Int64
//属性测试275PayeeInfoID
//属性测试275PayeeInfoID
//属性测试275PayeeInfoID
this.lblPayeeInfoID.AutoSize = true;
this.lblPayeeInfoID.Location = new System.Drawing.Point(100,275);
this.lblPayeeInfoID.Name = "lblPayeeInfoID";
this.lblPayeeInfoID.Size = new System.Drawing.Size(41, 12);
this.lblPayeeInfoID.TabIndex = 11;
this.lblPayeeInfoID.Text = "收款信息";
//111======275
this.cmbPayeeInfoID.Location = new System.Drawing.Point(173,271);
this.cmbPayeeInfoID.Name ="cmbPayeeInfoID";
this.cmbPayeeInfoID.Size = new System.Drawing.Size(100, 21);
this.cmbPayeeInfoID.TabIndex = 11;
this.Controls.Add(this.lblPayeeInfoID);
this.Controls.Add(this.cmbPayeeInfoID);

           //#####100PayeeAccountNo###String
this.lblPayeeAccountNo.AutoSize = true;
this.lblPayeeAccountNo.Location = new System.Drawing.Point(100,300);
this.lblPayeeAccountNo.Name = "lblPayeeAccountNo";
this.lblPayeeAccountNo.Size = new System.Drawing.Size(41, 12);
this.lblPayeeAccountNo.TabIndex = 12;
this.lblPayeeAccountNo.Text = "收款账号";
this.txtPayeeAccountNo.Location = new System.Drawing.Point(173,296);
this.txtPayeeAccountNo.Name = "txtPayeeAccountNo";
this.txtPayeeAccountNo.Size = new System.Drawing.Size(100, 21);
this.txtPayeeAccountNo.TabIndex = 12;
this.Controls.Add(this.lblPayeeAccountNo);
this.Controls.Add(this.txtPayeeAccountNo);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,325);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 13;
this.lblExchangeRate.Text = "汇率";
//111======325
this.txtExchangeRate.Location = new System.Drawing.Point(173,321);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 13;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####ReceivePaymentType###Int32
//属性测试350ReceivePaymentType
//属性测试350ReceivePaymentType
//属性测试350ReceivePaymentType
//属性测试350ReceivePaymentType
//属性测试350ReceivePaymentType
//属性测试350ReceivePaymentType
//属性测试350ReceivePaymentType
//属性测试350ReceivePaymentType

           //#####ShippingFee###Decimal
this.lblShippingFee.AutoSize = true;
this.lblShippingFee.Location = new System.Drawing.Point(100,375);
this.lblShippingFee.Name = "lblShippingFee";
this.lblShippingFee.Size = new System.Drawing.Size(41, 12);
this.lblShippingFee.TabIndex = 15;
this.lblShippingFee.Text = "运费";
//111======375
this.txtShippingFee.Location = new System.Drawing.Point(173,371);
this.txtShippingFee.Name ="txtShippingFee";
this.txtShippingFee.Size = new System.Drawing.Size(100, 21);
this.txtShippingFee.TabIndex = 15;
this.Controls.Add(this.lblShippingFee);
this.Controls.Add(this.txtShippingFee);

           //#####TotalForeignPayableAmount###Decimal
this.lblTotalForeignPayableAmount.AutoSize = true;
this.lblTotalForeignPayableAmount.Location = new System.Drawing.Point(100,400);
this.lblTotalForeignPayableAmount.Name = "lblTotalForeignPayableAmount";
this.lblTotalForeignPayableAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalForeignPayableAmount.TabIndex = 16;
this.lblTotalForeignPayableAmount.Text = "总金额外币";
//111======400
this.txtTotalForeignPayableAmount.Location = new System.Drawing.Point(173,396);
this.txtTotalForeignPayableAmount.Name ="txtTotalForeignPayableAmount";
this.txtTotalForeignPayableAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalForeignPayableAmount.TabIndex = 16;
this.Controls.Add(this.lblTotalForeignPayableAmount);
this.Controls.Add(this.txtTotalForeignPayableAmount);

           //#####TotalLocalPayableAmount###Decimal
this.lblTotalLocalPayableAmount.AutoSize = true;
this.lblTotalLocalPayableAmount.Location = new System.Drawing.Point(100,425);
this.lblTotalLocalPayableAmount.Name = "lblTotalLocalPayableAmount";
this.lblTotalLocalPayableAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalLocalPayableAmount.TabIndex = 17;
this.lblTotalLocalPayableAmount.Text = "总金额本币";
//111======425
this.txtTotalLocalPayableAmount.Location = new System.Drawing.Point(173,421);
this.txtTotalLocalPayableAmount.Name ="txtTotalLocalPayableAmount";
this.txtTotalLocalPayableAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalLocalPayableAmount.TabIndex = 17;
this.Controls.Add(this.lblTotalLocalPayableAmount);
this.Controls.Add(this.txtTotalLocalPayableAmount);

           //#####ForeignPaidAmount###Decimal
this.lblForeignPaidAmount.AutoSize = true;
this.lblForeignPaidAmount.Location = new System.Drawing.Point(100,450);
this.lblForeignPaidAmount.Name = "lblForeignPaidAmount";
this.lblForeignPaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignPaidAmount.TabIndex = 18;
this.lblForeignPaidAmount.Text = "已核销外币";
//111======450
this.txtForeignPaidAmount.Location = new System.Drawing.Point(173,446);
this.txtForeignPaidAmount.Name ="txtForeignPaidAmount";
this.txtForeignPaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignPaidAmount.TabIndex = 18;
this.Controls.Add(this.lblForeignPaidAmount);
this.Controls.Add(this.txtForeignPaidAmount);

           //#####LocalPaidAmount###Decimal
this.lblLocalPaidAmount.AutoSize = true;
this.lblLocalPaidAmount.Location = new System.Drawing.Point(100,475);
this.lblLocalPaidAmount.Name = "lblLocalPaidAmount";
this.lblLocalPaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalPaidAmount.TabIndex = 19;
this.lblLocalPaidAmount.Text = "已核销本币";
//111======475
this.txtLocalPaidAmount.Location = new System.Drawing.Point(173,471);
this.txtLocalPaidAmount.Name ="txtLocalPaidAmount";
this.txtLocalPaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalPaidAmount.TabIndex = 19;
this.Controls.Add(this.lblLocalPaidAmount);
this.Controls.Add(this.txtLocalPaidAmount);

           //#####ForeignBalanceAmount###Decimal
this.lblForeignBalanceAmount.AutoSize = true;
this.lblForeignBalanceAmount.Location = new System.Drawing.Point(100,500);
this.lblForeignBalanceAmount.Name = "lblForeignBalanceAmount";
this.lblForeignBalanceAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignBalanceAmount.TabIndex = 20;
this.lblForeignBalanceAmount.Text = "未核销外币";
//111======500
this.txtForeignBalanceAmount.Location = new System.Drawing.Point(173,496);
this.txtForeignBalanceAmount.Name ="txtForeignBalanceAmount";
this.txtForeignBalanceAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignBalanceAmount.TabIndex = 20;
this.Controls.Add(this.lblForeignBalanceAmount);
this.Controls.Add(this.txtForeignBalanceAmount);

           //#####LocalBalanceAmount###Decimal
this.lblLocalBalanceAmount.AutoSize = true;
this.lblLocalBalanceAmount.Location = new System.Drawing.Point(100,525);
this.lblLocalBalanceAmount.Name = "lblLocalBalanceAmount";
this.lblLocalBalanceAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalBalanceAmount.TabIndex = 21;
this.lblLocalBalanceAmount.Text = "未核销本币";
//111======525
this.txtLocalBalanceAmount.Location = new System.Drawing.Point(173,521);
this.txtLocalBalanceAmount.Name ="txtLocalBalanceAmount";
this.txtLocalBalanceAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalBalanceAmount.TabIndex = 21;
this.Controls.Add(this.lblLocalBalanceAmount);
this.Controls.Add(this.txtLocalBalanceAmount);

           //#####DueDate###DateTime
this.lblDueDate.AutoSize = true;
this.lblDueDate.Location = new System.Drawing.Point(100,550);
this.lblDueDate.Name = "lblDueDate";
this.lblDueDate.Size = new System.Drawing.Size(41, 12);
this.lblDueDate.TabIndex = 22;
this.lblDueDate.Text = "到期日";
//111======550
this.dtpDueDate.Location = new System.Drawing.Point(173,546);
this.dtpDueDate.Name ="dtpDueDate";
this.dtpDueDate.ShowCheckBox =true;
this.dtpDueDate.Size = new System.Drawing.Size(100, 21);
this.dtpDueDate.TabIndex = 22;
this.Controls.Add(this.lblDueDate);
this.Controls.Add(this.dtpDueDate);

           //#####DepartmentID###Int64
//属性测试575DepartmentID
//属性测试575DepartmentID
//属性测试575DepartmentID
//属性测试575DepartmentID
//属性测试575DepartmentID
//属性测试575DepartmentID
//属性测试575DepartmentID
//属性测试575DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,575);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 23;
this.lblDepartmentID.Text = "部门";
//111======575
this.cmbDepartmentID.Location = new System.Drawing.Point(173,571);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 23;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ProjectGroup_ID###Int64
//属性测试600ProjectGroup_ID
//属性测试600ProjectGroup_ID
//属性测试600ProjectGroup_ID
//属性测试600ProjectGroup_ID
//属性测试600ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,600);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 24;
this.lblProjectGroup_ID.Text = "项目组";
//111======600
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,596);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 24;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####Employee_ID###Int64
//属性测试625Employee_ID
//属性测试625Employee_ID
//属性测试625Employee_ID
//属性测试625Employee_ID
//属性测试625Employee_ID
//属性测试625Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,625);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 25;
this.lblEmployee_ID.Text = "经办人";
//111======625
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,621);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 25;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####InvoiceId###Int64
//属性测试650InvoiceId
//属性测试650InvoiceId
//属性测试650InvoiceId
//属性测试650InvoiceId
//属性测试650InvoiceId
//属性测试650InvoiceId
//属性测试650InvoiceId
this.lblInvoiceId.AutoSize = true;
this.lblInvoiceId.Location = new System.Drawing.Point(100,650);
this.lblInvoiceId.Name = "lblInvoiceId";
this.lblInvoiceId.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceId.TabIndex = 26;
this.lblInvoiceId.Text = "发票";
//111======650
this.cmbInvoiceId.Location = new System.Drawing.Point(173,646);
this.cmbInvoiceId.Name ="cmbInvoiceId";
this.cmbInvoiceId.Size = new System.Drawing.Size(100, 21);
this.cmbInvoiceId.TabIndex = 26;
this.Controls.Add(this.lblInvoiceId);
this.Controls.Add(this.cmbInvoiceId);

           //#####Invoiced###Boolean
this.lblInvoiced.AutoSize = true;
this.lblInvoiced.Location = new System.Drawing.Point(100,675);
this.lblInvoiced.Name = "lblInvoiced";
this.lblInvoiced.Size = new System.Drawing.Size(41, 12);
this.lblInvoiced.TabIndex = 27;
this.lblInvoiced.Text = "已开票";
this.chkInvoiced.Location = new System.Drawing.Point(173,671);
this.chkInvoiced.Name = "chkInvoiced";
this.chkInvoiced.Size = new System.Drawing.Size(100, 21);
this.chkInvoiced.TabIndex = 27;
this.Controls.Add(this.lblInvoiced);
this.Controls.Add(this.chkInvoiced);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,700);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 28;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,696);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 28;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####TaxTotalAmount###Decimal
this.lblTaxTotalAmount.AutoSize = true;
this.lblTaxTotalAmount.Location = new System.Drawing.Point(100,725);
this.lblTaxTotalAmount.Name = "lblTaxTotalAmount";
this.lblTaxTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxTotalAmount.TabIndex = 29;
this.lblTaxTotalAmount.Text = "税额总计";
//111======725
this.txtTaxTotalAmount.Location = new System.Drawing.Point(173,721);
this.txtTaxTotalAmount.Name ="txtTaxTotalAmount";
this.txtTaxTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxTotalAmount.TabIndex = 29;
this.Controls.Add(this.lblTaxTotalAmount);
this.Controls.Add(this.txtTaxTotalAmount);

           //#####UntaxedTotalAmont###Decimal
this.lblUntaxedTotalAmont.AutoSize = true;
this.lblUntaxedTotalAmont.Location = new System.Drawing.Point(100,750);
this.lblUntaxedTotalAmont.Name = "lblUntaxedTotalAmont";
this.lblUntaxedTotalAmont.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedTotalAmont.TabIndex = 30;
this.lblUntaxedTotalAmont.Text = "未税总计";
//111======750
this.txtUntaxedTotalAmont.Location = new System.Drawing.Point(173,746);
this.txtUntaxedTotalAmont.Name ="txtUntaxedTotalAmont";
this.txtUntaxedTotalAmont.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedTotalAmont.TabIndex = 30;
this.Controls.Add(this.lblUntaxedTotalAmont);
this.Controls.Add(this.txtUntaxedTotalAmont);

           //#####ARAPStatus###Int32
//属性测试775ARAPStatus
//属性测试775ARAPStatus
//属性测试775ARAPStatus
//属性测试775ARAPStatus
//属性测试775ARAPStatus
//属性测试775ARAPStatus
//属性测试775ARAPStatus
//属性测试775ARAPStatus

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,800);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 32;
this.lblRemark.Text = "备注";
this.txtRemark.Location = new System.Drawing.Point(173,796);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 32;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,825);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 33;
this.lblCreated_at.Text = "创建时间";
//111======825
this.dtpCreated_at.Location = new System.Drawing.Point(173,821);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 33;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试850Created_by
//属性测试850Created_by
//属性测试850Created_by
//属性测试850Created_by
//属性测试850Created_by
//属性测试850Created_by
//属性测试850Created_by
//属性测试850Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,875);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 35;
this.lblModified_at.Text = "修改时间";
//111======875
this.dtpModified_at.Location = new System.Drawing.Point(173,871);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 35;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试900Modified_by
//属性测试900Modified_by
//属性测试900Modified_by
//属性测试900Modified_by
//属性测试900Modified_by
//属性测试900Modified_by
//属性测试900Modified_by
//属性测试900Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,925);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 37;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,921);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 37;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,950);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 38;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,946);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 38;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试975Approver_by
//属性测试975Approver_by
//属性测试975Approver_by
//属性测试975Approver_by
//属性测试975Approver_by
//属性测试975Approver_by
//属性测试975Approver_by
//属性测试975Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,1000);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 40;
this.lblApprover_at.Text = "审批时间";
//111======1000
this.dtpApprover_at.Location = new System.Drawing.Point(173,996);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 40;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,1050);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 42;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,1046);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 42;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblARAPNo );
this.Controls.Add(this.txtARAPNo );

                
                
                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblIsExpenseType );
this.Controls.Add(this.chkIsExpenseType );

                this.Controls.Add(this.lblIsFromPlatform );
this.Controls.Add(this.chkIsFromPlatform );

                this.Controls.Add(this.lblPlatformOrderNo );
this.Controls.Add(this.txtPlatformOrderNo );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblPayeeInfoID );
this.Controls.Add(this.cmbPayeeInfoID );

                this.Controls.Add(this.lblPayeeAccountNo );
this.Controls.Add(this.txtPayeeAccountNo );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                
                this.Controls.Add(this.lblShippingFee );
this.Controls.Add(this.txtShippingFee );

                this.Controls.Add(this.lblTotalForeignPayableAmount );
this.Controls.Add(this.txtTotalForeignPayableAmount );

                this.Controls.Add(this.lblTotalLocalPayableAmount );
this.Controls.Add(this.txtTotalLocalPayableAmount );

                this.Controls.Add(this.lblForeignPaidAmount );
this.Controls.Add(this.txtForeignPaidAmount );

                this.Controls.Add(this.lblLocalPaidAmount );
this.Controls.Add(this.txtLocalPaidAmount );

                this.Controls.Add(this.lblForeignBalanceAmount );
this.Controls.Add(this.txtForeignBalanceAmount );

                this.Controls.Add(this.lblLocalBalanceAmount );
this.Controls.Add(this.txtLocalBalanceAmount );

                this.Controls.Add(this.lblDueDate );
this.Controls.Add(this.dtpDueDate );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblInvoiceId );
this.Controls.Add(this.cmbInvoiceId );

                this.Controls.Add(this.lblInvoiced );
this.Controls.Add(this.chkInvoiced );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                this.Controls.Add(this.lblTaxTotalAmount );
this.Controls.Add(this.txtTaxTotalAmount );

                this.Controls.Add(this.lblUntaxedTotalAmont );
this.Controls.Add(this.txtUntaxedTotalAmont );

                
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

                
                    
            this.Name = "tb_FM_ReceivablePayableQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblARAPNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtARAPNo;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsExpenseType;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsExpenseType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsFromPlatform;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsFromPlatform;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlatformOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPlatformOrderNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingFee;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingFee;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalForeignPayableAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalForeignPayableAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalLocalPayableAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalLocalPayableAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignPaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignPaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalPaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalPaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignBalanceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignBalanceAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalBalanceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalBalanceAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDueDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDueDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiceId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbInvoiceId;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiced;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkInvoiced;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedTotalAmont;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedTotalAmont;

    
        
              
    
        
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


