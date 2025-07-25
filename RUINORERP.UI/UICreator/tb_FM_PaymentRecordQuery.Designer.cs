﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:00
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 收付款记录表
    /// </summary>
    partial class tb_FM_PaymentRecordQuery
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
     
     this.lblPaymentNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPaymentNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNos = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNos = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSourceBillNos.Multiline = true;

this.lblIsFromPlatform = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsFromPlatform = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsFromPlatform.Values.Text ="";

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblTotalForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPaymentDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPaymentDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblPaymentImagePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPaymentImagePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtPaymentImagePath.Multiline = true;

this.lblReferenceNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReferenceNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtReferenceNo.Multiline = true;

this.lblIsReversed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsReversed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsReversed.Values.Text ="";

ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblReversedOriginalNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReversedOriginalNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblReversedByPaymentNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReversedByPaymentNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRemark = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemark = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRemark.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30PaymentNo###String
this.lblPaymentNo.AutoSize = true;
this.lblPaymentNo.Location = new System.Drawing.Point(100,25);
this.lblPaymentNo.Name = "lblPaymentNo";
this.lblPaymentNo.Size = new System.Drawing.Size(41, 12);
this.lblPaymentNo.TabIndex = 1;
this.lblPaymentNo.Text = "支付单号";
this.txtPaymentNo.Location = new System.Drawing.Point(173,21);
this.txtPaymentNo.Name = "txtPaymentNo";
this.txtPaymentNo.Size = new System.Drawing.Size(100, 21);
this.txtPaymentNo.TabIndex = 1;
this.Controls.Add(this.lblPaymentNo);
this.Controls.Add(this.txtPaymentNo);

           //#####ReceivePaymentType###Int32
//属性测试50ReceivePaymentType
//属性测试50ReceivePaymentType
//属性测试50ReceivePaymentType
//属性测试50ReceivePaymentType
//属性测试50ReceivePaymentType
//属性测试50ReceivePaymentType
//属性测试50ReceivePaymentType
ReversedByPaymentId主外字段不一致。//属性测试50ReceivePaymentType
ReversedOriginalId主外字段不一致。
           //#####Account_id###Int64
//属性测试75Account_id
//属性测试75Account_id
//属性测试75Account_id
//属性测试75Account_id
//属性测试75Account_id
//属性测试75Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,75);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 3;
this.lblAccount_id.Text = "公司账户";
//111======75
this.cmbAccount_id.Location = new System.Drawing.Point(173,71);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 3;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####CustomerVendor_ID###Int64
//属性测试100CustomerVendor_ID
//属性测试100CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,100);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 4;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======100
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,96);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 4;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####PayeeInfoID###Int64
//属性测试125PayeeInfoID
//属性测试125PayeeInfoID
//属性测试125PayeeInfoID
//属性测试125PayeeInfoID
this.lblPayeeInfoID.AutoSize = true;
this.lblPayeeInfoID.Location = new System.Drawing.Point(100,125);
this.lblPayeeInfoID.Name = "lblPayeeInfoID";
this.lblPayeeInfoID.Size = new System.Drawing.Size(41, 12);
this.lblPayeeInfoID.TabIndex = 5;
this.lblPayeeInfoID.Text = "收款信息";
//111======125
this.cmbPayeeInfoID.Location = new System.Drawing.Point(173,121);
this.cmbPayeeInfoID.Name ="cmbPayeeInfoID";
this.cmbPayeeInfoID.Size = new System.Drawing.Size(100, 21);
this.cmbPayeeInfoID.TabIndex = 5;
this.Controls.Add(this.lblPayeeInfoID);
this.Controls.Add(this.cmbPayeeInfoID);

           //#####100PayeeAccountNo###String
this.lblPayeeAccountNo.AutoSize = true;
this.lblPayeeAccountNo.Location = new System.Drawing.Point(100,150);
this.lblPayeeAccountNo.Name = "lblPayeeAccountNo";
this.lblPayeeAccountNo.Size = new System.Drawing.Size(41, 12);
this.lblPayeeAccountNo.TabIndex = 6;
this.lblPayeeAccountNo.Text = "收款账号";
this.txtPayeeAccountNo.Location = new System.Drawing.Point(173,146);
this.txtPayeeAccountNo.Name = "txtPayeeAccountNo";
this.txtPayeeAccountNo.Size = new System.Drawing.Size(100, 21);
this.txtPayeeAccountNo.TabIndex = 6;
this.Controls.Add(this.lblPayeeAccountNo);
this.Controls.Add(this.txtPayeeAccountNo);

           //#####1000SourceBillNos###String
this.lblSourceBillNos.AutoSize = true;
this.lblSourceBillNos.Location = new System.Drawing.Point(100,175);
this.lblSourceBillNos.Name = "lblSourceBillNos";
this.lblSourceBillNos.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNos.TabIndex = 7;
this.lblSourceBillNos.Text = "来源单号";
this.txtSourceBillNos.Location = new System.Drawing.Point(173,171);
this.txtSourceBillNos.Name = "txtSourceBillNos";
this.txtSourceBillNos.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNos.TabIndex = 7;
this.Controls.Add(this.lblSourceBillNos);
this.Controls.Add(this.txtSourceBillNos);

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

           //#####Currency_ID###Int64
//属性测试225Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,225);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 9;
this.lblCurrency_ID.Text = "币别";
//111======225
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,221);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 9;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####TotalForeignAmount###Decimal
this.lblTotalForeignAmount.AutoSize = true;
this.lblTotalForeignAmount.Location = new System.Drawing.Point(100,250);
this.lblTotalForeignAmount.Name = "lblTotalForeignAmount";
this.lblTotalForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalForeignAmount.TabIndex = 10;
this.lblTotalForeignAmount.Text = "支付金额外币";
//111======250
this.txtTotalForeignAmount.Location = new System.Drawing.Point(173,246);
this.txtTotalForeignAmount.Name ="txtTotalForeignAmount";
this.txtTotalForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalForeignAmount.TabIndex = 10;
this.Controls.Add(this.lblTotalForeignAmount);
this.Controls.Add(this.txtTotalForeignAmount);

           //#####TotalLocalAmount###Decimal
this.lblTotalLocalAmount.AutoSize = true;
this.lblTotalLocalAmount.Location = new System.Drawing.Point(100,275);
this.lblTotalLocalAmount.Name = "lblTotalLocalAmount";
this.lblTotalLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalLocalAmount.TabIndex = 11;
this.lblTotalLocalAmount.Text = "支付金额本币";
//111======275
this.txtTotalLocalAmount.Location = new System.Drawing.Point(173,271);
this.txtTotalLocalAmount.Name ="txtTotalLocalAmount";
this.txtTotalLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalLocalAmount.TabIndex = 11;
this.Controls.Add(this.lblTotalLocalAmount);
this.Controls.Add(this.txtTotalLocalAmount);

           //#####PaymentDate###DateTime
this.lblPaymentDate.AutoSize = true;
this.lblPaymentDate.Location = new System.Drawing.Point(100,300);
this.lblPaymentDate.Name = "lblPaymentDate";
this.lblPaymentDate.Size = new System.Drawing.Size(41, 12);
this.lblPaymentDate.TabIndex = 12;
this.lblPaymentDate.Text = "支付日期";
//111======300
this.dtpPaymentDate.Location = new System.Drawing.Point(173,296);
this.dtpPaymentDate.Name ="dtpPaymentDate";
this.dtpPaymentDate.ShowCheckBox =true;
this.dtpPaymentDate.Size = new System.Drawing.Size(100, 21);
this.dtpPaymentDate.TabIndex = 12;
this.Controls.Add(this.lblPaymentDate);
this.Controls.Add(this.dtpPaymentDate);

           //#####Employee_ID###Int64
//属性测试325Employee_ID
//属性测试325Employee_ID
//属性测试325Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,325);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 13;
this.lblEmployee_ID.Text = "经办人";
//111======325
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,321);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 13;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####Paytype_ID###Int64
//属性测试350Paytype_ID
//属性测试350Paytype_ID
//属性测试350Paytype_ID
//属性测试350Paytype_ID
//属性测试350Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,350);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 14;
this.lblPaytype_ID.Text = "付款方式";
//111======350
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,346);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 14;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####PaymentStatus###Int32
//属性测试375PaymentStatus
//属性测试375PaymentStatus
//属性测试375PaymentStatus
//属性测试375PaymentStatus
//属性测试375PaymentStatus
//属性测试375PaymentStatus
//属性测试375PaymentStatus
ReversedByPaymentId主外字段不一致。//属性测试375PaymentStatus
ReversedOriginalId主外字段不一致。
           //#####300PaymentImagePath###String
this.lblPaymentImagePath.AutoSize = true;
this.lblPaymentImagePath.Location = new System.Drawing.Point(100,400);
this.lblPaymentImagePath.Name = "lblPaymentImagePath";
this.lblPaymentImagePath.Size = new System.Drawing.Size(41, 12);
this.lblPaymentImagePath.TabIndex = 16;
this.lblPaymentImagePath.Text = "付款凭证";
this.txtPaymentImagePath.Location = new System.Drawing.Point(173,396);
this.txtPaymentImagePath.Name = "txtPaymentImagePath";
this.txtPaymentImagePath.Size = new System.Drawing.Size(100, 21);
this.txtPaymentImagePath.TabIndex = 16;
this.Controls.Add(this.lblPaymentImagePath);
this.Controls.Add(this.txtPaymentImagePath);

           //#####300ReferenceNo###String
this.lblReferenceNo.AutoSize = true;
this.lblReferenceNo.Location = new System.Drawing.Point(100,425);
this.lblReferenceNo.Name = "lblReferenceNo";
this.lblReferenceNo.Size = new System.Drawing.Size(41, 12);
this.lblReferenceNo.TabIndex = 17;
this.lblReferenceNo.Text = "交易参考号";
this.txtReferenceNo.Location = new System.Drawing.Point(173,421);
this.txtReferenceNo.Name = "txtReferenceNo";
this.txtReferenceNo.Size = new System.Drawing.Size(100, 21);
this.txtReferenceNo.TabIndex = 17;
this.Controls.Add(this.lblReferenceNo);
this.Controls.Add(this.txtReferenceNo);

           //#####IsReversed###Boolean
this.lblIsReversed.AutoSize = true;
this.lblIsReversed.Location = new System.Drawing.Point(100,450);
this.lblIsReversed.Name = "lblIsReversed";
this.lblIsReversed.Size = new System.Drawing.Size(41, 12);
this.lblIsReversed.TabIndex = 18;
this.lblIsReversed.Text = "是否冲销";
this.chkIsReversed.Location = new System.Drawing.Point(173,446);
this.chkIsReversed.Name = "chkIsReversed";
this.chkIsReversed.Size = new System.Drawing.Size(100, 21);
this.chkIsReversed.TabIndex = 18;
this.Controls.Add(this.lblIsReversed);
this.Controls.Add(this.chkIsReversed);

           //#####ReversedOriginalId###Int64
//属性测试475ReversedOriginalId
//属性测试475ReversedOriginalId
//属性测试475ReversedOriginalId
//属性测试475ReversedOriginalId
//属性测试475ReversedOriginalId
//属性测试475ReversedOriginalId
//属性测试475ReversedOriginalId
ReversedByPaymentId主外字段不一致。//属性测试475ReversedOriginalId
ReversedOriginalId主外字段不一致。
           //#####30ReversedOriginalNo###String
this.lblReversedOriginalNo.AutoSize = true;
this.lblReversedOriginalNo.Location = new System.Drawing.Point(100,500);
this.lblReversedOriginalNo.Name = "lblReversedOriginalNo";
this.lblReversedOriginalNo.Size = new System.Drawing.Size(41, 12);
this.lblReversedOriginalNo.TabIndex = 20;
this.lblReversedOriginalNo.Text = "冲销单号";
this.txtReversedOriginalNo.Location = new System.Drawing.Point(173,496);
this.txtReversedOriginalNo.Name = "txtReversedOriginalNo";
this.txtReversedOriginalNo.Size = new System.Drawing.Size(100, 21);
this.txtReversedOriginalNo.TabIndex = 20;
this.Controls.Add(this.lblReversedOriginalNo);
this.Controls.Add(this.txtReversedOriginalNo);

           //#####ReversedByPaymentId###Int64
//属性测试525ReversedByPaymentId
//属性测试525ReversedByPaymentId
//属性测试525ReversedByPaymentId
//属性测试525ReversedByPaymentId
//属性测试525ReversedByPaymentId
//属性测试525ReversedByPaymentId
//属性测试525ReversedByPaymentId
ReversedByPaymentId主外字段不一致。//属性测试525ReversedByPaymentId
ReversedOriginalId主外字段不一致。
           //#####30ReversedByPaymentNo###String
this.lblReversedByPaymentNo.AutoSize = true;
this.lblReversedByPaymentNo.Location = new System.Drawing.Point(100,550);
this.lblReversedByPaymentNo.Name = "lblReversedByPaymentNo";
this.lblReversedByPaymentNo.Size = new System.Drawing.Size(41, 12);
this.lblReversedByPaymentNo.TabIndex = 22;
this.lblReversedByPaymentNo.Text = "被冲销单号";
this.txtReversedByPaymentNo.Location = new System.Drawing.Point(173,546);
this.txtReversedByPaymentNo.Name = "txtReversedByPaymentNo";
this.txtReversedByPaymentNo.Size = new System.Drawing.Size(100, 21);
this.txtReversedByPaymentNo.TabIndex = 22;
this.Controls.Add(this.lblReversedByPaymentNo);
this.Controls.Add(this.txtReversedByPaymentNo);

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,575);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 23;
this.lblRemark.Text = "备注";
this.txtRemark.Location = new System.Drawing.Point(173,571);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 23;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,600);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 24;
this.lblCreated_at.Text = "创建时间";
//111======600
this.dtpCreated_at.Location = new System.Drawing.Point(173,596);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 24;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试625Created_by
//属性测试625Created_by
//属性测试625Created_by
//属性测试625Created_by
//属性测试625Created_by
//属性测试625Created_by
//属性测试625Created_by
ReversedByPaymentId主外字段不一致。//属性测试625Created_by
ReversedOriginalId主外字段不一致。
           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,650);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 26;
this.lblModified_at.Text = "修改时间";
//111======650
this.dtpModified_at.Location = new System.Drawing.Point(173,646);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 26;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试675Modified_by
//属性测试675Modified_by
//属性测试675Modified_by
//属性测试675Modified_by
//属性测试675Modified_by
//属性测试675Modified_by
//属性测试675Modified_by
ReversedByPaymentId主外字段不一致。//属性测试675Modified_by
ReversedOriginalId主外字段不一致。
           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,700);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 28;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,696);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 28;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,725);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 29;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,721);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 29;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by
ReversedByPaymentId主外字段不一致。//属性测试750Approver_by
ReversedOriginalId主外字段不一致。
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

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,825);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 33;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,821);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 33;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试850PrintStatus
//属性测试850PrintStatus
//属性测试850PrintStatus
//属性测试850PrintStatus
//属性测试850PrintStatus
//属性测试850PrintStatus
//属性测试850PrintStatus
ReversedByPaymentId主外字段不一致。//属性测试850PrintStatus
ReversedOriginalId主外字段不一致。
          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPaymentNo );
this.Controls.Add(this.txtPaymentNo );

                ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblPayeeInfoID );
this.Controls.Add(this.cmbPayeeInfoID );

                this.Controls.Add(this.lblPayeeAccountNo );
this.Controls.Add(this.txtPayeeAccountNo );

                this.Controls.Add(this.lblSourceBillNos );
this.Controls.Add(this.txtSourceBillNos );

                this.Controls.Add(this.lblIsFromPlatform );
this.Controls.Add(this.chkIsFromPlatform );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblTotalForeignAmount );
this.Controls.Add(this.txtTotalForeignAmount );

                this.Controls.Add(this.lblTotalLocalAmount );
this.Controls.Add(this.txtTotalLocalAmount );

                this.Controls.Add(this.lblPaymentDate );
this.Controls.Add(this.dtpPaymentDate );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
                this.Controls.Add(this.lblPaymentImagePath );
this.Controls.Add(this.txtPaymentImagePath );

                this.Controls.Add(this.lblReferenceNo );
this.Controls.Add(this.txtReferenceNo );

                this.Controls.Add(this.lblIsReversed );
this.Controls.Add(this.chkIsReversed );

                ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
                this.Controls.Add(this.lblReversedOriginalNo );
this.Controls.Add(this.txtReversedOriginalNo );

                ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
                this.Controls.Add(this.lblReversedByPaymentNo );
this.Controls.Add(this.txtReversedByPaymentNo );

                this.Controls.Add(this.lblRemark );
this.Controls.Add(this.txtRemark );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
                    
            this.Name = "tb_FM_PaymentRecordQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPaymentNo;

    
        
              ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbAccount_id;
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNos;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNos;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsFromPlatform;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsFromPlatform;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPaymentDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentImagePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPaymentImagePath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReferenceNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReferenceNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsReversed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsReversed;

    
        
              ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReversedOriginalNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReversedOriginalNo;

    
        
              ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReversedByPaymentNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReversedByPaymentNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemark;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemark;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。
    
    
   
 





    }
}


