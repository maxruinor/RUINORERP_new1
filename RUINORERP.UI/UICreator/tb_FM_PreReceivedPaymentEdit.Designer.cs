// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 15:37:44
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
    partial class tb_FM_PreReceivedPaymentEdit
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
     this.lblPreRPNO = new Krypton.Toolkit.KryptonLabel();
this.txtPreRPNO = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblPayeeInfoID = new Krypton.Toolkit.KryptonLabel();
this.cmbPayeeInfoID = new Krypton.Toolkit.KryptonComboBox();

this.lblPayeeAccountNo = new Krypton.Toolkit.KryptonLabel();
this.txtPayeeAccountNo = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

this.lblPrePayDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPrePayDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblPrePaymentReason = new Krypton.Toolkit.KryptonLabel();
this.txtPrePaymentReason = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBizType = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillId = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillId = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNo = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new Krypton.Toolkit.KryptonTextBox();

this.lblPrePaymentStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrePaymentStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignPrepaidAmount = new Krypton.Toolkit.KryptonLabel();
this.txtForeignPrepaidAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblLocalPrepaidAmount = new Krypton.Toolkit.KryptonLabel();
this.txtLocalPrepaidAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblLocalPrepaidAmountInWords = new Krypton.Toolkit.KryptonLabel();
this.txtLocalPrepaidAmountInWords = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignPaidAmount = new Krypton.Toolkit.KryptonLabel();
this.txtForeignPaidAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblLocalPaidAmount = new Krypton.Toolkit.KryptonLabel();
this.txtLocalPaidAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblLocalRefundAmount = new Krypton.Toolkit.KryptonLabel();
this.txtLocalRefundAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignRefundAmount = new Krypton.Toolkit.KryptonLabel();
this.txtForeignRefundAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignBalanceAmount = new Krypton.Toolkit.KryptonLabel();
this.txtForeignBalanceAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblLocalBalanceAmount = new Krypton.Toolkit.KryptonLabel();
this.txtLocalBalanceAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblReceivePaymentType = new Krypton.Toolkit.KryptonLabel();
this.txtReceivePaymentType = new Krypton.Toolkit.KryptonTextBox();

this.lblPaymentImagePath = new Krypton.Toolkit.KryptonLabel();
this.txtPaymentImagePath = new Krypton.Toolkit.KryptonTextBox();
this.txtPaymentImagePath.Multiline = true;

this.lblIsAvailable = new Krypton.Toolkit.KryptonLabel();
this.chkIsAvailable = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsAvailable.Values.Text ="";
this.chkIsAvailable.Checked = true;
this.chkIsAvailable.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblRemark = new Krypton.Toolkit.KryptonLabel();
this.txtRemark = new Krypton.Toolkit.KryptonTextBox();
this.txtRemark.Multiline = true;

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

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
this.lblAccount_id.Text = "公司账户";
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

           //#####SourceBizType###Int32
//属性测试350SourceBizType
//属性测试350SourceBizType
//属性测试350SourceBizType
//属性测试350SourceBizType
//属性测试350SourceBizType
//属性测试350SourceBizType
//属性测试350SourceBizType
//属性测试350SourceBizType
this.lblSourceBizType.AutoSize = true;
this.lblSourceBizType.Location = new System.Drawing.Point(100,350);
this.lblSourceBizType.Name = "lblSourceBizType";
this.lblSourceBizType.Size = new System.Drawing.Size(41, 12);
this.lblSourceBizType.TabIndex = 14;
this.lblSourceBizType.Text = "来源业务";
this.txtSourceBizType.Location = new System.Drawing.Point(173,346);
this.txtSourceBizType.Name = "txtSourceBizType";
this.txtSourceBizType.Size = new System.Drawing.Size(100, 21);
this.txtSourceBizType.TabIndex = 14;
this.Controls.Add(this.lblSourceBizType);
this.Controls.Add(this.txtSourceBizType);

           //#####SourceBillId###Int64
//属性测试375SourceBillId
//属性测试375SourceBillId
//属性测试375SourceBillId
//属性测试375SourceBillId
//属性测试375SourceBillId
//属性测试375SourceBillId
//属性测试375SourceBillId
//属性测试375SourceBillId
this.lblSourceBillId.AutoSize = true;
this.lblSourceBillId.Location = new System.Drawing.Point(100,375);
this.lblSourceBillId.Name = "lblSourceBillId";
this.lblSourceBillId.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillId.TabIndex = 15;
this.lblSourceBillId.Text = "来源单据";
this.txtSourceBillId.Location = new System.Drawing.Point(173,371);
this.txtSourceBillId.Name = "txtSourceBillId";
this.txtSourceBillId.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillId.TabIndex = 15;
this.Controls.Add(this.lblSourceBillId);
this.Controls.Add(this.txtSourceBillId);

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,400);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 16;
this.lblSourceBillNo.Text = "来源单号";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,396);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 16;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

           //#####PrePaymentStatus###Int64
//属性测试425PrePaymentStatus
//属性测试425PrePaymentStatus
//属性测试425PrePaymentStatus
//属性测试425PrePaymentStatus
//属性测试425PrePaymentStatus
//属性测试425PrePaymentStatus
//属性测试425PrePaymentStatus
//属性测试425PrePaymentStatus
this.lblPrePaymentStatus.AutoSize = true;
this.lblPrePaymentStatus.Location = new System.Drawing.Point(100,425);
this.lblPrePaymentStatus.Name = "lblPrePaymentStatus";
this.lblPrePaymentStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrePaymentStatus.TabIndex = 17;
this.lblPrePaymentStatus.Text = "预收付状态";
this.txtPrePaymentStatus.Location = new System.Drawing.Point(173,421);
this.txtPrePaymentStatus.Name = "txtPrePaymentStatus";
this.txtPrePaymentStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrePaymentStatus.TabIndex = 17;
this.Controls.Add(this.lblPrePaymentStatus);
this.Controls.Add(this.txtPrePaymentStatus);

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

           //#####LocalRefundAmount###Decimal
this.lblLocalRefundAmount.AutoSize = true;
this.lblLocalRefundAmount.Location = new System.Drawing.Point(100,575);
this.lblLocalRefundAmount.Name = "lblLocalRefundAmount";
this.lblLocalRefundAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalRefundAmount.TabIndex = 23;
this.lblLocalRefundAmount.Text = "退款金额本币";
//111======575
this.txtLocalRefundAmount.Location = new System.Drawing.Point(173,571);
this.txtLocalRefundAmount.Name ="txtLocalRefundAmount";
this.txtLocalRefundAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalRefundAmount.TabIndex = 23;
this.Controls.Add(this.lblLocalRefundAmount);
this.Controls.Add(this.txtLocalRefundAmount);

           //#####ForeignRefundAmount###Decimal
this.lblForeignRefundAmount.AutoSize = true;
this.lblForeignRefundAmount.Location = new System.Drawing.Point(100,600);
this.lblForeignRefundAmount.Name = "lblForeignRefundAmount";
this.lblForeignRefundAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignRefundAmount.TabIndex = 24;
this.lblForeignRefundAmount.Text = "退款金额外币";
//111======600
this.txtForeignRefundAmount.Location = new System.Drawing.Point(173,596);
this.txtForeignRefundAmount.Name ="txtForeignRefundAmount";
this.txtForeignRefundAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignRefundAmount.TabIndex = 24;
this.Controls.Add(this.lblForeignRefundAmount);
this.Controls.Add(this.txtForeignRefundAmount);

           //#####ForeignBalanceAmount###Decimal
this.lblForeignBalanceAmount.AutoSize = true;
this.lblForeignBalanceAmount.Location = new System.Drawing.Point(100,625);
this.lblForeignBalanceAmount.Name = "lblForeignBalanceAmount";
this.lblForeignBalanceAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignBalanceAmount.TabIndex = 25;
this.lblForeignBalanceAmount.Text = "余额外币";
//111======625
this.txtForeignBalanceAmount.Location = new System.Drawing.Point(173,621);
this.txtForeignBalanceAmount.Name ="txtForeignBalanceAmount";
this.txtForeignBalanceAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignBalanceAmount.TabIndex = 25;
this.Controls.Add(this.lblForeignBalanceAmount);
this.Controls.Add(this.txtForeignBalanceAmount);

           //#####LocalBalanceAmount###Decimal
this.lblLocalBalanceAmount.AutoSize = true;
this.lblLocalBalanceAmount.Location = new System.Drawing.Point(100,650);
this.lblLocalBalanceAmount.Name = "lblLocalBalanceAmount";
this.lblLocalBalanceAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalBalanceAmount.TabIndex = 26;
this.lblLocalBalanceAmount.Text = "余额本币";
//111======650
this.txtLocalBalanceAmount.Location = new System.Drawing.Point(173,646);
this.txtLocalBalanceAmount.Name ="txtLocalBalanceAmount";
this.txtLocalBalanceAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalBalanceAmount.TabIndex = 26;
this.Controls.Add(this.lblLocalBalanceAmount);
this.Controls.Add(this.txtLocalBalanceAmount);

           //#####ReceivePaymentType###Int32
//属性测试675ReceivePaymentType
//属性测试675ReceivePaymentType
//属性测试675ReceivePaymentType
//属性测试675ReceivePaymentType
//属性测试675ReceivePaymentType
//属性测试675ReceivePaymentType
//属性测试675ReceivePaymentType
//属性测试675ReceivePaymentType
this.lblReceivePaymentType.AutoSize = true;
this.lblReceivePaymentType.Location = new System.Drawing.Point(100,675);
this.lblReceivePaymentType.Name = "lblReceivePaymentType";
this.lblReceivePaymentType.Size = new System.Drawing.Size(41, 12);
this.lblReceivePaymentType.TabIndex = 27;
this.lblReceivePaymentType.Text = "收付类型";
this.txtReceivePaymentType.Location = new System.Drawing.Point(173,671);
this.txtReceivePaymentType.Name = "txtReceivePaymentType";
this.txtReceivePaymentType.Size = new System.Drawing.Size(100, 21);
this.txtReceivePaymentType.TabIndex = 27;
this.Controls.Add(this.lblReceivePaymentType);
this.Controls.Add(this.txtReceivePaymentType);

           //#####300PaymentImagePath###String
this.lblPaymentImagePath.AutoSize = true;
this.lblPaymentImagePath.Location = new System.Drawing.Point(100,700);
this.lblPaymentImagePath.Name = "lblPaymentImagePath";
this.lblPaymentImagePath.Size = new System.Drawing.Size(41, 12);
this.lblPaymentImagePath.TabIndex = 28;
this.lblPaymentImagePath.Text = "付款凭证";
this.txtPaymentImagePath.Location = new System.Drawing.Point(173,696);
this.txtPaymentImagePath.Name = "txtPaymentImagePath";
this.txtPaymentImagePath.Size = new System.Drawing.Size(100, 21);
this.txtPaymentImagePath.TabIndex = 28;
this.Controls.Add(this.lblPaymentImagePath);
this.Controls.Add(this.txtPaymentImagePath);

           //#####IsAvailable###Boolean
this.lblIsAvailable.AutoSize = true;
this.lblIsAvailable.Location = new System.Drawing.Point(100,725);
this.lblIsAvailable.Name = "lblIsAvailable";
this.lblIsAvailable.Size = new System.Drawing.Size(41, 12);
this.lblIsAvailable.TabIndex = 29;
this.lblIsAvailable.Text = "是否可用";
this.chkIsAvailable.Location = new System.Drawing.Point(173,721);
this.chkIsAvailable.Name = "chkIsAvailable";
this.chkIsAvailable.Size = new System.Drawing.Size(100, 21);
this.chkIsAvailable.TabIndex = 29;
this.Controls.Add(this.lblIsAvailable);
this.Controls.Add(this.chkIsAvailable);

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,750);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 30;
this.lblRemark.Text = "备注";
this.txtRemark.Location = new System.Drawing.Point(173,746);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 30;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,775);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 31;
this.lblCreated_at.Text = "创建时间";
//111======775
this.dtpCreated_at.Location = new System.Drawing.Point(173,771);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 31;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试800Created_by
//属性测试800Created_by
//属性测试800Created_by
//属性测试800Created_by
//属性测试800Created_by
//属性测试800Created_by
//属性测试800Created_by
//属性测试800Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,800);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 32;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,796);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 32;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,825);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 33;
this.lblModified_at.Text = "修改时间";
//111======825
this.dtpModified_at.Location = new System.Drawing.Point(173,821);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 33;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试850Modified_by
//属性测试850Modified_by
//属性测试850Modified_by
//属性测试850Modified_by
//属性测试850Modified_by
//属性测试850Modified_by
//属性测试850Modified_by
//属性测试850Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,850);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 34;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,846);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 34;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,875);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 35;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,871);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 35;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,900);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 36;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,896);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 36;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试925Approver_by
//属性测试925Approver_by
//属性测试925Approver_by
//属性测试925Approver_by
//属性测试925Approver_by
//属性测试925Approver_by
//属性测试925Approver_by
//属性测试925Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,925);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 37;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,921);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 37;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,950);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 38;
this.lblApprover_at.Text = "审批时间";
//111======950
this.dtpApprover_at.Location = new System.Drawing.Point(173,946);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 38;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,1000);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 40;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,996);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 40;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试1025PrintStatus
//属性测试1025PrintStatus
//属性测试1025PrintStatus
//属性测试1025PrintStatus
//属性测试1025PrintStatus
//属性测试1025PrintStatus
//属性测试1025PrintStatus
//属性测试1025PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,1025);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 41;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,1021);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 41;
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
           // this.kryptonPanel1.TabIndex = 41;

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

                this.Controls.Add(this.lblSourceBizType );
this.Controls.Add(this.txtSourceBizType );

                this.Controls.Add(this.lblSourceBillId );
this.Controls.Add(this.txtSourceBillId );

                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                this.Controls.Add(this.lblPrePaymentStatus );
this.Controls.Add(this.txtPrePaymentStatus );

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

                this.Controls.Add(this.lblLocalRefundAmount );
this.Controls.Add(this.txtLocalRefundAmount );

                this.Controls.Add(this.lblForeignRefundAmount );
this.Controls.Add(this.txtForeignRefundAmount );

                this.Controls.Add(this.lblForeignBalanceAmount );
this.Controls.Add(this.txtForeignBalanceAmount );

                this.Controls.Add(this.lblLocalBalanceAmount );
this.Controls.Add(this.txtLocalBalanceAmount );

                this.Controls.Add(this.lblReceivePaymentType );
this.Controls.Add(this.txtReceivePaymentType );

                this.Controls.Add(this.lblPaymentImagePath );
this.Controls.Add(this.txtPaymentImagePath );

                this.Controls.Add(this.lblIsAvailable );
this.Controls.Add(this.chkIsAvailable );

                this.Controls.Add(this.lblRemark );
this.Controls.Add(this.txtRemark );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

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

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                            // 
            // "tb_FM_PreReceivedPaymentEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_PreReceivedPaymentEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPreRPNO;
private Krypton.Toolkit.KryptonTextBox txtPreRPNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
private Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrePayDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPrePayDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrePaymentReason;
private Krypton.Toolkit.KryptonTextBox txtPrePaymentReason;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBizType;
private Krypton.Toolkit.KryptonTextBox txtSourceBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillId;
private Krypton.Toolkit.KryptonTextBox txtSourceBillId;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrePaymentStatus;
private Krypton.Toolkit.KryptonTextBox txtPrePaymentStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignPrepaidAmount;
private Krypton.Toolkit.KryptonTextBox txtForeignPrepaidAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocalPrepaidAmount;
private Krypton.Toolkit.KryptonTextBox txtLocalPrepaidAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocalPrepaidAmountInWords;
private Krypton.Toolkit.KryptonTextBox txtLocalPrepaidAmountInWords;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignPaidAmount;
private Krypton.Toolkit.KryptonTextBox txtForeignPaidAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocalPaidAmount;
private Krypton.Toolkit.KryptonTextBox txtLocalPaidAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocalRefundAmount;
private Krypton.Toolkit.KryptonTextBox txtLocalRefundAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignRefundAmount;
private Krypton.Toolkit.KryptonTextBox txtForeignRefundAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignBalanceAmount;
private Krypton.Toolkit.KryptonTextBox txtForeignBalanceAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocalBalanceAmount;
private Krypton.Toolkit.KryptonTextBox txtLocalBalanceAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblReceivePaymentType;
private Krypton.Toolkit.KryptonTextBox txtReceivePaymentType;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaymentImagePath;
private Krypton.Toolkit.KryptonTextBox txtPaymentImagePath;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsAvailable;
private Krypton.Toolkit.KryptonCheckBox chkIsAvailable;

    
        
              private Krypton.Toolkit.KryptonLabel lblRemark;
private Krypton.Toolkit.KryptonTextBox txtRemark;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

