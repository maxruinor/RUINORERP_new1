// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:12
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 记录收款 与应收的匹配，核销表-支持多对多、行项级核销 
    /// </summary>
    partial class tb_FM_PaymentSettlementEdit
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
     this.lblPaymentId = new Krypton.Toolkit.KryptonLabel();
this.cmbPaymentId = new Krypton.Toolkit.KryptonComboBox();

this.lblBizType = new Krypton.Toolkit.KryptonLabel();
this.txtBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBilllID = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBilllID = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNO = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNO = new Krypton.Toolkit.KryptonTextBox();

this.lblReceivePaymentType = new Krypton.Toolkit.KryptonLabel();
this.txtReceivePaymentType = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_id = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCurrency_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

this.lblSettledForeignAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSettledForeignAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSettledLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSettledLocalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblPamountInWords = new Krypton.Toolkit.KryptonLabel();
this.txtPamountInWords = new Krypton.Toolkit.KryptonTextBox();

this.lblSettleDate = new Krypton.Toolkit.KryptonLabel();
this.dtpSettleDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblSettlementType = new Krypton.Toolkit.KryptonLabel();
this.txtSettlementType = new Krypton.Toolkit.KryptonTextBox();

this.lblEvidenceImagePath = new Krypton.Toolkit.KryptonLabel();
this.txtEvidenceImagePath = new Krypton.Toolkit.KryptonTextBox();
this.txtEvidenceImagePath.Multiline = true;

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
     
            //#####PaymentId###Int64
//属性测试25PaymentId
this.lblPaymentId.AutoSize = true;
this.lblPaymentId.Location = new System.Drawing.Point(100,25);
this.lblPaymentId.Name = "lblPaymentId";
this.lblPaymentId.Size = new System.Drawing.Size(41, 12);
this.lblPaymentId.TabIndex = 1;
this.lblPaymentId.Text = "收付款单";
//111======25
this.cmbPaymentId.Location = new System.Drawing.Point(173,21);
this.cmbPaymentId.Name ="cmbPaymentId";
this.cmbPaymentId.Size = new System.Drawing.Size(100, 21);
this.cmbPaymentId.TabIndex = 1;
this.Controls.Add(this.lblPaymentId);
this.Controls.Add(this.cmbPaymentId);

           //#####BizType###Int32
//属性测试50BizType
this.lblBizType.AutoSize = true;
this.lblBizType.Location = new System.Drawing.Point(100,50);
this.lblBizType.Name = "lblBizType";
this.lblBizType.Size = new System.Drawing.Size(41, 12);
this.lblBizType.TabIndex = 2;
this.lblBizType.Text = "来源业务";
this.txtBizType.Location = new System.Drawing.Point(173,46);
this.txtBizType.Name = "txtBizType";
this.txtBizType.Size = new System.Drawing.Size(100, 21);
this.txtBizType.TabIndex = 2;
this.Controls.Add(this.lblBizType);
this.Controls.Add(this.txtBizType);

           //#####SourceBilllID###Int64
//属性测试75SourceBilllID
this.lblSourceBilllID.AutoSize = true;
this.lblSourceBilllID.Location = new System.Drawing.Point(100,75);
this.lblSourceBilllID.Name = "lblSourceBilllID";
this.lblSourceBilllID.Size = new System.Drawing.Size(41, 12);
this.lblSourceBilllID.TabIndex = 3;
this.lblSourceBilllID.Text = "来源单据";
this.txtSourceBilllID.Location = new System.Drawing.Point(173,71);
this.txtSourceBilllID.Name = "txtSourceBilllID";
this.txtSourceBilllID.Size = new System.Drawing.Size(100, 21);
this.txtSourceBilllID.TabIndex = 3;
this.Controls.Add(this.lblSourceBilllID);
this.Controls.Add(this.txtSourceBilllID);

           //#####SourceBillDetailID###Int64
//属性测试100SourceBillDetailID
this.lblSourceBillDetailID.AutoSize = true;
this.lblSourceBillDetailID.Location = new System.Drawing.Point(100,100);
this.lblSourceBillDetailID.Name = "lblSourceBillDetailID";
this.lblSourceBillDetailID.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillDetailID.TabIndex = 4;
this.lblSourceBillDetailID.Text = "单据明细";
this.txtSourceBillDetailID.Location = new System.Drawing.Point(173,96);
this.txtSourceBillDetailID.Name = "txtSourceBillDetailID";
this.txtSourceBillDetailID.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillDetailID.TabIndex = 4;
this.Controls.Add(this.lblSourceBillDetailID);
this.Controls.Add(this.txtSourceBillDetailID);

           //#####30SourceBillNO###String
this.lblSourceBillNO.AutoSize = true;
this.lblSourceBillNO.Location = new System.Drawing.Point(100,125);
this.lblSourceBillNO.Name = "lblSourceBillNO";
this.lblSourceBillNO.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNO.TabIndex = 5;
this.lblSourceBillNO.Text = "来源单号";
this.txtSourceBillNO.Location = new System.Drawing.Point(173,121);
this.txtSourceBillNO.Name = "txtSourceBillNO";
this.txtSourceBillNO.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNO.TabIndex = 5;
this.Controls.Add(this.lblSourceBillNO);
this.Controls.Add(this.txtSourceBillNO);

           //#####ReceivePaymentType###Int64
//属性测试150ReceivePaymentType
this.lblReceivePaymentType.AutoSize = true;
this.lblReceivePaymentType.Location = new System.Drawing.Point(100,150);
this.lblReceivePaymentType.Name = "lblReceivePaymentType";
this.lblReceivePaymentType.Size = new System.Drawing.Size(41, 12);
this.lblReceivePaymentType.TabIndex = 6;
this.lblReceivePaymentType.Text = "收付类型";
this.txtReceivePaymentType.Location = new System.Drawing.Point(173,146);
this.txtReceivePaymentType.Name = "txtReceivePaymentType";
this.txtReceivePaymentType.Size = new System.Drawing.Size(100, 21);
this.txtReceivePaymentType.TabIndex = 6;
this.Controls.Add(this.lblReceivePaymentType);
this.Controls.Add(this.txtReceivePaymentType);

           //#####Account_id###Int64
//属性测试175Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,175);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 7;
this.lblAccount_id.Text = "公司账户";
this.txtAccount_id.Location = new System.Drawing.Point(173,171);
this.txtAccount_id.Name = "txtAccount_id";
this.txtAccount_id.Size = new System.Drawing.Size(100, 21);
this.txtAccount_id.TabIndex = 7;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.txtAccount_id);

           //#####Employee_ID###Int64
//属性测试200Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,200);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 8;
this.lblEmployee_ID.Text = "核销人";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,196);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 8;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####CustomerVendor_ID###Int64
//属性测试225CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,225);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 9;
this.lblCustomerVendor_ID.Text = "往来单位";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,221);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 9;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####Currency_ID###Int64
//属性测试250Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,250);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 10;
this.lblCurrency_ID.Text = "币别";
this.txtCurrency_ID.Location = new System.Drawing.Point(173,246);
this.txtCurrency_ID.Name = "txtCurrency_ID";
this.txtCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.txtCurrency_ID.TabIndex = 10;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.txtCurrency_ID);

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

           //#####SettledForeignAmount###Decimal
this.lblSettledForeignAmount.AutoSize = true;
this.lblSettledForeignAmount.Location = new System.Drawing.Point(100,300);
this.lblSettledForeignAmount.Name = "lblSettledForeignAmount";
this.lblSettledForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblSettledForeignAmount.TabIndex = 12;
this.lblSettledForeignAmount.Text = "核销金额外币";
//111======300
this.txtSettledForeignAmount.Location = new System.Drawing.Point(173,296);
this.txtSettledForeignAmount.Name ="txtSettledForeignAmount";
this.txtSettledForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtSettledForeignAmount.TabIndex = 12;
this.Controls.Add(this.lblSettledForeignAmount);
this.Controls.Add(this.txtSettledForeignAmount);

           //#####SettledLocalAmount###Decimal
this.lblSettledLocalAmount.AutoSize = true;
this.lblSettledLocalAmount.Location = new System.Drawing.Point(100,325);
this.lblSettledLocalAmount.Name = "lblSettledLocalAmount";
this.lblSettledLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSettledLocalAmount.TabIndex = 13;
this.lblSettledLocalAmount.Text = "核销金额本币";
//111======325
this.txtSettledLocalAmount.Location = new System.Drawing.Point(173,321);
this.txtSettledLocalAmount.Name ="txtSettledLocalAmount";
this.txtSettledLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSettledLocalAmount.TabIndex = 13;
this.Controls.Add(this.lblSettledLocalAmount);
this.Controls.Add(this.txtSettledLocalAmount);

           //#####100PamountInWords###String
this.lblPamountInWords.AutoSize = true;
this.lblPamountInWords.Location = new System.Drawing.Point(100,350);
this.lblPamountInWords.Name = "lblPamountInWords";
this.lblPamountInWords.Size = new System.Drawing.Size(41, 12);
this.lblPamountInWords.TabIndex = 14;
this.lblPamountInWords.Text = "大写收款金额";
this.txtPamountInWords.Location = new System.Drawing.Point(173,346);
this.txtPamountInWords.Name = "txtPamountInWords";
this.txtPamountInWords.Size = new System.Drawing.Size(100, 21);
this.txtPamountInWords.TabIndex = 14;
this.Controls.Add(this.lblPamountInWords);
this.Controls.Add(this.txtPamountInWords);

           //#####SettleDate###DateTime
this.lblSettleDate.AutoSize = true;
this.lblSettleDate.Location = new System.Drawing.Point(100,375);
this.lblSettleDate.Name = "lblSettleDate";
this.lblSettleDate.Size = new System.Drawing.Size(41, 12);
this.lblSettleDate.TabIndex = 15;
this.lblSettleDate.Text = "核销日期";
//111======375
this.dtpSettleDate.Location = new System.Drawing.Point(173,371);
this.dtpSettleDate.Name ="dtpSettleDate";
this.dtpSettleDate.Size = new System.Drawing.Size(100, 21);
this.dtpSettleDate.TabIndex = 15;
this.Controls.Add(this.lblSettleDate);
this.Controls.Add(this.dtpSettleDate);

           //#####300Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,400);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 16;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,396);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 16;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####SettlementType###Int32
//属性测试425SettlementType
this.lblSettlementType.AutoSize = true;
this.lblSettlementType.Location = new System.Drawing.Point(100,425);
this.lblSettlementType.Name = "lblSettlementType";
this.lblSettlementType.Size = new System.Drawing.Size(41, 12);
this.lblSettlementType.TabIndex = 17;
this.lblSettlementType.Text = "付款状态";
this.txtSettlementType.Location = new System.Drawing.Point(173,421);
this.txtSettlementType.Name = "txtSettlementType";
this.txtSettlementType.Size = new System.Drawing.Size(100, 21);
this.txtSettlementType.TabIndex = 17;
this.Controls.Add(this.lblSettlementType);
this.Controls.Add(this.txtSettlementType);

           //#####300EvidenceImagePath###String
this.lblEvidenceImagePath.AutoSize = true;
this.lblEvidenceImagePath.Location = new System.Drawing.Point(100,450);
this.lblEvidenceImagePath.Name = "lblEvidenceImagePath";
this.lblEvidenceImagePath.Size = new System.Drawing.Size(41, 12);
this.lblEvidenceImagePath.TabIndex = 18;
this.lblEvidenceImagePath.Text = "凭证图";
this.txtEvidenceImagePath.Location = new System.Drawing.Point(173,446);
this.txtEvidenceImagePath.Name = "txtEvidenceImagePath";
this.txtEvidenceImagePath.Size = new System.Drawing.Size(100, 21);
this.txtEvidenceImagePath.TabIndex = 18;
this.Controls.Add(this.lblEvidenceImagePath);
this.Controls.Add(this.txtEvidenceImagePath);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,475);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 19;
this.lblCreated_at.Text = "创建时间";
//111======475
this.dtpCreated_at.Location = new System.Drawing.Point(173,471);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 19;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试500Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,500);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 20;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,496);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 20;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,525);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 21;
this.lblModified_at.Text = "修改时间";
//111======525
this.dtpModified_at.Location = new System.Drawing.Point(173,521);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 21;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试550Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,550);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 22;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,546);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 22;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,575);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 23;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,571);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 23;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,600);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 24;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,596);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 24;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试625Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,625);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 25;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,621);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 25;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,650);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 26;
this.lblApprover_at.Text = "审批时间";
//111======650
this.dtpApprover_at.Location = new System.Drawing.Point(173,646);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 26;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,700);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 28;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,696);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 28;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试725PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,725);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 29;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,721);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 29;
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
           // this.kryptonPanel1.TabIndex = 29;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPaymentId );
this.Controls.Add(this.cmbPaymentId );

                this.Controls.Add(this.lblBizType );
this.Controls.Add(this.txtBizType );

                this.Controls.Add(this.lblSourceBilllID );
this.Controls.Add(this.txtSourceBilllID );

                this.Controls.Add(this.lblSourceBillDetailID );
this.Controls.Add(this.txtSourceBillDetailID );

                this.Controls.Add(this.lblSourceBillNO );
this.Controls.Add(this.txtSourceBillNO );

                this.Controls.Add(this.lblReceivePaymentType );
this.Controls.Add(this.txtReceivePaymentType );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.txtAccount_id );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.txtCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblSettledForeignAmount );
this.Controls.Add(this.txtSettledForeignAmount );

                this.Controls.Add(this.lblSettledLocalAmount );
this.Controls.Add(this.txtSettledLocalAmount );

                this.Controls.Add(this.lblPamountInWords );
this.Controls.Add(this.txtPamountInWords );

                this.Controls.Add(this.lblSettleDate );
this.Controls.Add(this.dtpSettleDate );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblSettlementType );
this.Controls.Add(this.txtSettlementType );

                this.Controls.Add(this.lblEvidenceImagePath );
this.Controls.Add(this.txtEvidenceImagePath );

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
            // "tb_FM_PaymentSettlementEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_PaymentSettlementEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPaymentId;
private Krypton.Toolkit.KryptonComboBox cmbPaymentId;

    
        
              private Krypton.Toolkit.KryptonLabel lblBizType;
private Krypton.Toolkit.KryptonTextBox txtBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBilllID;
private Krypton.Toolkit.KryptonTextBox txtSourceBilllID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillDetailID;
private Krypton.Toolkit.KryptonTextBox txtSourceBillDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblReceivePaymentType;
private Krypton.Toolkit.KryptonTextBox txtReceivePaymentType;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonTextBox txtAccount_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonTextBox txtCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSettledForeignAmount;
private Krypton.Toolkit.KryptonTextBox txtSettledForeignAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSettledLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtSettledLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblPamountInWords;
private Krypton.Toolkit.KryptonTextBox txtPamountInWords;

    
        
              private Krypton.Toolkit.KryptonLabel lblSettleDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpSettleDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblSettlementType;
private Krypton.Toolkit.KryptonTextBox txtSettlementType;

    
        
              private Krypton.Toolkit.KryptonLabel lblEvidenceImagePath;
private Krypton.Toolkit.KryptonTextBox txtEvidenceImagePath;

    
        
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

