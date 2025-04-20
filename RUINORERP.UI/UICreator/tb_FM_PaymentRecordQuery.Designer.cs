
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 18:12:08
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 收款付款记录表-记录所有资金流动一批订单可分账户分批付 记录真实资金流动，用户需确保其 与银行流水一致
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
     
     

this.lblSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPaymentDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPaymentDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblReferenceNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReferenceNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtReferenceNo.Multiline = true;

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
                 //#####BizType###Int32
//属性测试25BizType
//属性测试25BizType
//属性测试25BizType
//属性测试25BizType
//属性测试25BizType
//属性测试25BizType
//属性测试25BizType
//属性测试25BizType

           //#####SourceBilllID###Int64
//属性测试50SourceBilllID
//属性测试50SourceBilllID
//属性测试50SourceBilllID
//属性测试50SourceBilllID
//属性测试50SourceBilllID
//属性测试50SourceBilllID
//属性测试50SourceBilllID
//属性测试50SourceBilllID

           //#####30SourceBillNO###String
this.lblSourceBillNO.AutoSize = true;
this.lblSourceBillNO.Location = new System.Drawing.Point(100,75);
this.lblSourceBillNO.Name = "lblSourceBillNO";
this.lblSourceBillNO.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNO.TabIndex = 3;
this.lblSourceBillNO.Text = "来源单号";
this.txtSourceBillNO.Location = new System.Drawing.Point(173,71);
this.txtSourceBillNO.Name = "txtSourceBillNO";
this.txtSourceBillNO.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNO.TabIndex = 3;
this.Controls.Add(this.lblSourceBillNO);
this.Controls.Add(this.txtSourceBillNO);

           //#####ReceivePaymentType###Int64
//属性测试100ReceivePaymentType
//属性测试100ReceivePaymentType
//属性测试100ReceivePaymentType
//属性测试100ReceivePaymentType
//属性测试100ReceivePaymentType
//属性测试100ReceivePaymentType
//属性测试100ReceivePaymentType
//属性测试100ReceivePaymentType

           //#####Account_id###Int64
//属性测试125Account_id
//属性测试125Account_id
//属性测试125Account_id
//属性测试125Account_id
//属性测试125Account_id
//属性测试125Account_id
//属性测试125Account_id
//属性测试125Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,125);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 5;
this.lblAccount_id.Text = "公司账户";
//111======125
this.cmbAccount_id.Location = new System.Drawing.Point(173,121);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 5;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####CustomerVendor_ID###Int64
//属性测试150CustomerVendor_ID
//属性测试150CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,150);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 6;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======150
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,146);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 6;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####PayeeInfoID###Int64
//属性测试175PayeeInfoID
//属性测试175PayeeInfoID
//属性测试175PayeeInfoID
//属性测试175PayeeInfoID
//属性测试175PayeeInfoID
this.lblPayeeInfoID.AutoSize = true;
this.lblPayeeInfoID.Location = new System.Drawing.Point(100,175);
this.lblPayeeInfoID.Name = "lblPayeeInfoID";
this.lblPayeeInfoID.Size = new System.Drawing.Size(41, 12);
this.lblPayeeInfoID.TabIndex = 7;
this.lblPayeeInfoID.Text = "收款信息";
//111======175
this.cmbPayeeInfoID.Location = new System.Drawing.Point(173,171);
this.cmbPayeeInfoID.Name ="cmbPayeeInfoID";
this.cmbPayeeInfoID.Size = new System.Drawing.Size(100, 21);
this.cmbPayeeInfoID.TabIndex = 7;
this.Controls.Add(this.lblPayeeInfoID);
this.Controls.Add(this.cmbPayeeInfoID);

           //#####100PayeeAccountNo###String
this.lblPayeeAccountNo.AutoSize = true;
this.lblPayeeAccountNo.Location = new System.Drawing.Point(100,200);
this.lblPayeeAccountNo.Name = "lblPayeeAccountNo";
this.lblPayeeAccountNo.Size = new System.Drawing.Size(41, 12);
this.lblPayeeAccountNo.TabIndex = 8;
this.lblPayeeAccountNo.Text = "收款账号";
this.txtPayeeAccountNo.Location = new System.Drawing.Point(173,196);
this.txtPayeeAccountNo.Name = "txtPayeeAccountNo";
this.txtPayeeAccountNo.Size = new System.Drawing.Size(100, 21);
this.txtPayeeAccountNo.TabIndex = 8;
this.Controls.Add(this.lblPayeeAccountNo);
this.Controls.Add(this.txtPayeeAccountNo);

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

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,250);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 10;
this.lblExchangeRate.Text = "汇率";
//111======250
this.txtExchangeRate.Location = new System.Drawing.Point(173,246);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 10;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####ForeignPaidAmount###Decimal
this.lblForeignPaidAmount.AutoSize = true;
this.lblForeignPaidAmount.Location = new System.Drawing.Point(100,275);
this.lblForeignPaidAmount.Name = "lblForeignPaidAmount";
this.lblForeignPaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignPaidAmount.TabIndex = 11;
this.lblForeignPaidAmount.Text = "支付金额外币";
//111======275
this.txtForeignPaidAmount.Location = new System.Drawing.Point(173,271);
this.txtForeignPaidAmount.Name ="txtForeignPaidAmount";
this.txtForeignPaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignPaidAmount.TabIndex = 11;
this.Controls.Add(this.lblForeignPaidAmount);
this.Controls.Add(this.txtForeignPaidAmount);

           //#####LocalPaidAmount###Decimal
this.lblLocalPaidAmount.AutoSize = true;
this.lblLocalPaidAmount.Location = new System.Drawing.Point(100,300);
this.lblLocalPaidAmount.Name = "lblLocalPaidAmount";
this.lblLocalPaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalPaidAmount.TabIndex = 12;
this.lblLocalPaidAmount.Text = "支付金额本币";
//111======300
this.txtLocalPaidAmount.Location = new System.Drawing.Point(173,296);
this.txtLocalPaidAmount.Name ="txtLocalPaidAmount";
this.txtLocalPaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalPaidAmount.TabIndex = 12;
this.Controls.Add(this.lblLocalPaidAmount);
this.Controls.Add(this.txtLocalPaidAmount);

           //#####PaymentDate###DateTime
this.lblPaymentDate.AutoSize = true;
this.lblPaymentDate.Location = new System.Drawing.Point(100,325);
this.lblPaymentDate.Name = "lblPaymentDate";
this.lblPaymentDate.Size = new System.Drawing.Size(41, 12);
this.lblPaymentDate.TabIndex = 13;
this.lblPaymentDate.Text = "支付日期";
//111======325
this.dtpPaymentDate.Location = new System.Drawing.Point(173,321);
this.dtpPaymentDate.Name ="dtpPaymentDate";
this.dtpPaymentDate.ShowCheckBox =true;
this.dtpPaymentDate.Size = new System.Drawing.Size(100, 21);
this.dtpPaymentDate.TabIndex = 13;
this.Controls.Add(this.lblPaymentDate);
this.Controls.Add(this.dtpPaymentDate);

           //#####Employee_ID###Int64
//属性测试350Employee_ID
//属性测试350Employee_ID
//属性测试350Employee_ID
//属性测试350Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,350);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 14;
this.lblEmployee_ID.Text = "经办人";
//111======350
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,346);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 14;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####DepartmentID###Int64
//属性测试375DepartmentID
//属性测试375DepartmentID
//属性测试375DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,375);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 15;
this.lblDepartmentID.Text = "部门";
//111======375
this.cmbDepartmentID.Location = new System.Drawing.Point(173,371);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 15;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ProjectGroup_ID###Int64
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
//属性测试400ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,400);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 16;
this.lblProjectGroup_ID.Text = "项目组";
//111======400
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,396);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 16;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####Paytype_ID###Int64
//属性测试425Paytype_ID
//属性测试425Paytype_ID
//属性测试425Paytype_ID
//属性测试425Paytype_ID
//属性测试425Paytype_ID
//属性测试425Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,425);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 17;
this.lblPaytype_ID.Text = "付款方式";
//111======425
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,421);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 17;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####FMPaymentStatus###Int32
//属性测试450FMPaymentStatus
//属性测试450FMPaymentStatus
//属性测试450FMPaymentStatus
//属性测试450FMPaymentStatus
//属性测试450FMPaymentStatus
//属性测试450FMPaymentStatus
//属性测试450FMPaymentStatus
//属性测试450FMPaymentStatus

           //#####300ReferenceNo###String
this.lblReferenceNo.AutoSize = true;
this.lblReferenceNo.Location = new System.Drawing.Point(100,475);
this.lblReferenceNo.Name = "lblReferenceNo";
this.lblReferenceNo.Size = new System.Drawing.Size(41, 12);
this.lblReferenceNo.TabIndex = 19;
this.lblReferenceNo.Text = "交易参考号";
this.txtReferenceNo.Location = new System.Drawing.Point(173,471);
this.txtReferenceNo.Name = "txtReferenceNo";
this.txtReferenceNo.Size = new System.Drawing.Size(100, 21);
this.txtReferenceNo.TabIndex = 19;
this.Controls.Add(this.lblReferenceNo);
this.Controls.Add(this.txtReferenceNo);

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,500);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 20;
this.lblRemark.Text = "备注";
this.txtRemark.Location = new System.Drawing.Point(173,496);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 20;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

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
//属性测试550Created_by
//属性测试550Created_by
//属性测试550Created_by

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
//属性测试600Modified_by
//属性测试600Modified_by
//属性测试600Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,625);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 25;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,621);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 25;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
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
//属性测试675Approver_by
//属性测试675Approver_by
//属性测试675Approver_by

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                this.Controls.Add(this.lblSourceBillNO );
this.Controls.Add(this.txtSourceBillNO );

                
                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblPayeeInfoID );
this.Controls.Add(this.cmbPayeeInfoID );

                this.Controls.Add(this.lblPayeeAccountNo );
this.Controls.Add(this.txtPayeeAccountNo );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblForeignPaidAmount );
this.Controls.Add(this.txtForeignPaidAmount );

                this.Controls.Add(this.lblLocalPaidAmount );
this.Controls.Add(this.txtLocalPaidAmount );

                this.Controls.Add(this.lblPaymentDate );
this.Controls.Add(this.dtpPaymentDate );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                
                this.Controls.Add(this.lblReferenceNo );
this.Controls.Add(this.txtReferenceNo );

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

                    
            this.Name = "tb_FM_PaymentRecordQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNO;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignPaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignPaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalPaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalPaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPaymentDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReferenceNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReferenceNo;

    
        
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


