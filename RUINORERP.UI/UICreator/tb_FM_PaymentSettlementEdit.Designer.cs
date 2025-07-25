﻿// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:03
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 记录收款 与应收的匹配，核销表 核销记录用于跟踪资金与债权债务的冲抵关系，确保财务数据可追溯。正常的收款，支付不需要保存核销记录
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
     this.lblSettlementNo = new Krypton.Toolkit.KryptonLabel();
this.txtSettlementNo = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblSourceBizType = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBizType = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblSourceBillId = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillId = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNo = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new Krypton.Toolkit.KryptonTextBox();

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblTargetBizType = new Krypton.Toolkit.KryptonLabel();
this.txtTargetBizType = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblTargetBillId = new Krypton.Toolkit.KryptonLabel();
this.txtTargetBillId = new Krypton.Toolkit.KryptonTextBox();

this.lblTargetBillNo = new Krypton.Toolkit.KryptonLabel();
this.txtTargetBillNo = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblReceivePaymentType = new Krypton.Toolkit.KryptonLabel();
this.txtReceivePaymentType = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new Krypton.Toolkit.KryptonComboBox();
ReversedSettlementID主外字段不一致。
ReversedSettlementID主外字段不一致。this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSettledForeignAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSettledForeignAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSettledLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSettledLocalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblIsAutoSettlement = new Krypton.Toolkit.KryptonLabel();
this.chkIsAutoSettlement = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsAutoSettlement.Values.Text ="";

this.lblIsReversed = new Krypton.Toolkit.KryptonLabel();
this.chkIsReversed = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsReversed.Values.Text ="";

ReversedSettlementID主外字段不一致。this.lblReversedSettlementID = new Krypton.Toolkit.KryptonLabel();
this.txtReversedSettlementID = new Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new Krypton.Toolkit.KryptonComboBox();
ReversedSettlementID主外字段不一致。
this.lblSettleDate = new Krypton.Toolkit.KryptonLabel();
this.dtpSettleDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

ReversedSettlementID主外字段不一致。this.lblSettlementType = new Krypton.Toolkit.KryptonLabel();
this.txtSettlementType = new Krypton.Toolkit.KryptonTextBox();

this.lblEvidenceImagePath = new Krypton.Toolkit.KryptonLabel();
this.txtEvidenceImagePath = new Krypton.Toolkit.KryptonTextBox();
this.txtEvidenceImagePath.Multiline = true;

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

ReversedSettlementID主外字段不一致。this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    
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
     
            //#####30SettlementNo###String
this.lblSettlementNo.AutoSize = true;
this.lblSettlementNo.Location = new System.Drawing.Point(100,25);
this.lblSettlementNo.Name = "lblSettlementNo";
this.lblSettlementNo.Size = new System.Drawing.Size(41, 12);
this.lblSettlementNo.TabIndex = 1;
this.lblSettlementNo.Text = "核销单号";
this.txtSettlementNo.Location = new System.Drawing.Point(173,21);
this.txtSettlementNo.Name = "txtSettlementNo";
this.txtSettlementNo.Size = new System.Drawing.Size(100, 21);
this.txtSettlementNo.TabIndex = 1;
this.Controls.Add(this.lblSettlementNo);
this.Controls.Add(this.txtSettlementNo);

           //#####SourceBizType###Int32
//属性测试50SourceBizType
//属性测试50SourceBizType
//属性测试50SourceBizType
ReversedSettlementID主外字段不一致。this.lblSourceBizType.AutoSize = true;
this.lblSourceBizType.Location = new System.Drawing.Point(100,50);
this.lblSourceBizType.Name = "lblSourceBizType";
this.lblSourceBizType.Size = new System.Drawing.Size(41, 12);
this.lblSourceBizType.TabIndex = 2;
this.lblSourceBizType.Text = "来源单据类型";
this.txtSourceBizType.Location = new System.Drawing.Point(173,46);
this.txtSourceBizType.Name = "txtSourceBizType";
this.txtSourceBizType.Size = new System.Drawing.Size(100, 21);
this.txtSourceBizType.TabIndex = 2;
this.Controls.Add(this.lblSourceBizType);
this.Controls.Add(this.txtSourceBizType);

           //#####SourceBillId###Int64
//属性测试75SourceBillId
//属性测试75SourceBillId
//属性测试75SourceBillId
ReversedSettlementID主外字段不一致。this.lblSourceBillId.AutoSize = true;
this.lblSourceBillId.Location = new System.Drawing.Point(100,75);
this.lblSourceBillId.Name = "lblSourceBillId";
this.lblSourceBillId.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillId.TabIndex = 3;
this.lblSourceBillId.Text = "来源单据";
this.txtSourceBillId.Location = new System.Drawing.Point(173,71);
this.txtSourceBillId.Name = "txtSourceBillId";
this.txtSourceBillId.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillId.TabIndex = 3;
this.Controls.Add(this.lblSourceBillId);
this.Controls.Add(this.txtSourceBillId);

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,100);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 4;
this.lblSourceBillNo.Text = "来源单据编号";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,96);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 4;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,125);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 5;
this.lblExchangeRate.Text = "汇率";
//111======125
this.txtExchangeRate.Location = new System.Drawing.Point(173,121);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 5;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####TargetBizType###Int32
//属性测试150TargetBizType
//属性测试150TargetBizType
//属性测试150TargetBizType
ReversedSettlementID主外字段不一致。this.lblTargetBizType.AutoSize = true;
this.lblTargetBizType.Location = new System.Drawing.Point(100,150);
this.lblTargetBizType.Name = "lblTargetBizType";
this.lblTargetBizType.Size = new System.Drawing.Size(41, 12);
this.lblTargetBizType.TabIndex = 6;
this.lblTargetBizType.Text = "目标单据类型";
this.txtTargetBizType.Location = new System.Drawing.Point(173,146);
this.txtTargetBizType.Name = "txtTargetBizType";
this.txtTargetBizType.Size = new System.Drawing.Size(100, 21);
this.txtTargetBizType.TabIndex = 6;
this.Controls.Add(this.lblTargetBizType);
this.Controls.Add(this.txtTargetBizType);

           //#####TargetBillId###Int64
//属性测试175TargetBillId
//属性测试175TargetBillId
//属性测试175TargetBillId
ReversedSettlementID主外字段不一致。this.lblTargetBillId.AutoSize = true;
this.lblTargetBillId.Location = new System.Drawing.Point(100,175);
this.lblTargetBillId.Name = "lblTargetBillId";
this.lblTargetBillId.Size = new System.Drawing.Size(41, 12);
this.lblTargetBillId.TabIndex = 7;
this.lblTargetBillId.Text = "目标单据";
this.txtTargetBillId.Location = new System.Drawing.Point(173,171);
this.txtTargetBillId.Name = "txtTargetBillId";
this.txtTargetBillId.Size = new System.Drawing.Size(100, 21);
this.txtTargetBillId.TabIndex = 7;
this.Controls.Add(this.lblTargetBillId);
this.Controls.Add(this.txtTargetBillId);

           //#####30TargetBillNo###String
this.lblTargetBillNo.AutoSize = true;
this.lblTargetBillNo.Location = new System.Drawing.Point(100,200);
this.lblTargetBillNo.Name = "lblTargetBillNo";
this.lblTargetBillNo.Size = new System.Drawing.Size(41, 12);
this.lblTargetBillNo.TabIndex = 8;
this.lblTargetBillNo.Text = "目标单据编号";
this.txtTargetBillNo.Location = new System.Drawing.Point(173,196);
this.txtTargetBillNo.Name = "txtTargetBillNo";
this.txtTargetBillNo.Size = new System.Drawing.Size(100, 21);
this.txtTargetBillNo.TabIndex = 8;
this.Controls.Add(this.lblTargetBillNo);
this.Controls.Add(this.txtTargetBillNo);

           //#####ReceivePaymentType###Int32
//属性测试225ReceivePaymentType
//属性测试225ReceivePaymentType
//属性测试225ReceivePaymentType
ReversedSettlementID主外字段不一致。this.lblReceivePaymentType.AutoSize = true;
this.lblReceivePaymentType.Location = new System.Drawing.Point(100,225);
this.lblReceivePaymentType.Name = "lblReceivePaymentType";
this.lblReceivePaymentType.Size = new System.Drawing.Size(41, 12);
this.lblReceivePaymentType.TabIndex = 9;
this.lblReceivePaymentType.Text = "收付类型";
this.txtReceivePaymentType.Location = new System.Drawing.Point(173,221);
this.txtReceivePaymentType.Name = "txtReceivePaymentType";
this.txtReceivePaymentType.Size = new System.Drawing.Size(100, 21);
this.txtReceivePaymentType.TabIndex = 9;
this.Controls.Add(this.lblReceivePaymentType);
this.Controls.Add(this.txtReceivePaymentType);

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

           //#####CustomerVendor_ID###Int64
//属性测试275CustomerVendor_ID
//属性测试275CustomerVendor_ID
//属性测试275CustomerVendor_ID
ReversedSettlementID主外字段不一致。this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,275);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 11;
this.lblCustomerVendor_ID.Text = "往来单位";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,271);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 11;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

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

           //#####IsAutoSettlement###Boolean
this.lblIsAutoSettlement.AutoSize = true;
this.lblIsAutoSettlement.Location = new System.Drawing.Point(100,350);
this.lblIsAutoSettlement.Name = "lblIsAutoSettlement";
this.lblIsAutoSettlement.Size = new System.Drawing.Size(41, 12);
this.lblIsAutoSettlement.TabIndex = 14;
this.lblIsAutoSettlement.Text = "是否自动核销";
this.chkIsAutoSettlement.Location = new System.Drawing.Point(173,346);
this.chkIsAutoSettlement.Name = "chkIsAutoSettlement";
this.chkIsAutoSettlement.Size = new System.Drawing.Size(100, 21);
this.chkIsAutoSettlement.TabIndex = 14;
this.Controls.Add(this.lblIsAutoSettlement);
this.Controls.Add(this.chkIsAutoSettlement);

           //#####IsReversed###Boolean
this.lblIsReversed.AutoSize = true;
this.lblIsReversed.Location = new System.Drawing.Point(100,375);
this.lblIsReversed.Name = "lblIsReversed";
this.lblIsReversed.Size = new System.Drawing.Size(41, 12);
this.lblIsReversed.TabIndex = 15;
this.lblIsReversed.Text = "是否冲销";
this.chkIsReversed.Location = new System.Drawing.Point(173,371);
this.chkIsReversed.Name = "chkIsReversed";
this.chkIsReversed.Size = new System.Drawing.Size(100, 21);
this.chkIsReversed.TabIndex = 15;
this.Controls.Add(this.lblIsReversed);
this.Controls.Add(this.chkIsReversed);

           //#####ReversedSettlementID###Int64
//属性测试400ReversedSettlementID
//属性测试400ReversedSettlementID
//属性测试400ReversedSettlementID
ReversedSettlementID主外字段不一致。this.lblReversedSettlementID.AutoSize = true;
this.lblReversedSettlementID.Location = new System.Drawing.Point(100,400);
this.lblReversedSettlementID.Name = "lblReversedSettlementID";
this.lblReversedSettlementID.Size = new System.Drawing.Size(41, 12);
this.lblReversedSettlementID.TabIndex = 16;
this.lblReversedSettlementID.Text = "对冲记录";
this.txtReversedSettlementID.Location = new System.Drawing.Point(173,396);
this.txtReversedSettlementID.Name = "txtReversedSettlementID";
this.txtReversedSettlementID.Size = new System.Drawing.Size(100, 21);
this.txtReversedSettlementID.TabIndex = 16;
this.Controls.Add(this.lblReversedSettlementID);
this.Controls.Add(this.txtReversedSettlementID);

           //#####Currency_ID###Int64
//属性测试425Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,425);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 17;
this.lblCurrency_ID.Text = "币别";
//111======425
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,421);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 17;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####SettleDate###DateTime
this.lblSettleDate.AutoSize = true;
this.lblSettleDate.Location = new System.Drawing.Point(100,450);
this.lblSettleDate.Name = "lblSettleDate";
this.lblSettleDate.Size = new System.Drawing.Size(41, 12);
this.lblSettleDate.TabIndex = 18;
this.lblSettleDate.Text = "核销日期";
//111======450
this.dtpSettleDate.Location = new System.Drawing.Point(173,446);
this.dtpSettleDate.Name ="dtpSettleDate";
this.dtpSettleDate.Size = new System.Drawing.Size(100, 21);
this.dtpSettleDate.TabIndex = 18;
this.Controls.Add(this.lblSettleDate);
this.Controls.Add(this.dtpSettleDate);

           //#####300Notes###String
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

           //#####SettlementType###Int32
//属性测试500SettlementType
//属性测试500SettlementType
//属性测试500SettlementType
ReversedSettlementID主外字段不一致。this.lblSettlementType.AutoSize = true;
this.lblSettlementType.Location = new System.Drawing.Point(100,500);
this.lblSettlementType.Name = "lblSettlementType";
this.lblSettlementType.Size = new System.Drawing.Size(41, 12);
this.lblSettlementType.TabIndex = 20;
this.lblSettlementType.Text = "核销状态";
this.txtSettlementType.Location = new System.Drawing.Point(173,496);
this.txtSettlementType.Name = "txtSettlementType";
this.txtSettlementType.Size = new System.Drawing.Size(100, 21);
this.txtSettlementType.TabIndex = 20;
this.Controls.Add(this.lblSettlementType);
this.Controls.Add(this.txtSettlementType);

           //#####300EvidenceImagePath###String
this.lblEvidenceImagePath.AutoSize = true;
this.lblEvidenceImagePath.Location = new System.Drawing.Point(100,525);
this.lblEvidenceImagePath.Name = "lblEvidenceImagePath";
this.lblEvidenceImagePath.Size = new System.Drawing.Size(41, 12);
this.lblEvidenceImagePath.TabIndex = 21;
this.lblEvidenceImagePath.Text = "凭证图";
this.txtEvidenceImagePath.Location = new System.Drawing.Point(173,521);
this.txtEvidenceImagePath.Name = "txtEvidenceImagePath";
this.txtEvidenceImagePath.Size = new System.Drawing.Size(100, 21);
this.txtEvidenceImagePath.TabIndex = 21;
this.Controls.Add(this.lblEvidenceImagePath);
this.Controls.Add(this.txtEvidenceImagePath);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,550);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 22;
this.lblCreated_at.Text = "创建时间";
//111======550
this.dtpCreated_at.Location = new System.Drawing.Point(173,546);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 22;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试575Created_by
//属性测试575Created_by
//属性测试575Created_by
ReversedSettlementID主外字段不一致。this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,575);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 23;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,571);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 23;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,600);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 24;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,596);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 24;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

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
           // this.kryptonPanel1.TabIndex = 24;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSettlementNo );
this.Controls.Add(this.txtSettlementNo );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblSourceBizType );
this.Controls.Add(this.txtSourceBizType );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblSourceBillId );
this.Controls.Add(this.txtSourceBillId );

                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblTargetBizType );
this.Controls.Add(this.txtTargetBizType );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblTargetBillId );
this.Controls.Add(this.txtTargetBillId );

                this.Controls.Add(this.lblTargetBillNo );
this.Controls.Add(this.txtTargetBillNo );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblReceivePaymentType );
this.Controls.Add(this.txtReceivePaymentType );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblSettledForeignAmount );
this.Controls.Add(this.txtSettledForeignAmount );

                this.Controls.Add(this.lblSettledLocalAmount );
this.Controls.Add(this.txtSettledLocalAmount );

                this.Controls.Add(this.lblIsAutoSettlement );
this.Controls.Add(this.chkIsAutoSettlement );

                this.Controls.Add(this.lblIsReversed );
this.Controls.Add(this.chkIsReversed );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblReversedSettlementID );
this.Controls.Add(this.txtReversedSettlementID );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblSettleDate );
this.Controls.Add(this.dtpSettleDate );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblSettlementType );
this.Controls.Add(this.txtSettlementType );

                this.Controls.Add(this.lblEvidenceImagePath );
this.Controls.Add(this.txtEvidenceImagePath );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

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
     
         
              private Krypton.Toolkit.KryptonLabel lblSettlementNo;
private Krypton.Toolkit.KryptonTextBox txtSettlementNo;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSourceBizType;
private Krypton.Toolkit.KryptonTextBox txtSourceBizType;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSourceBillId;
private Krypton.Toolkit.KryptonTextBox txtSourceBillId;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblTargetBizType;
private Krypton.Toolkit.KryptonTextBox txtTargetBizType;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblTargetBillId;
private Krypton.Toolkit.KryptonTextBox txtTargetBillId;

    
        
              private Krypton.Toolkit.KryptonLabel lblTargetBillNo;
private Krypton.Toolkit.KryptonTextBox txtTargetBillNo;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblReceivePaymentType;
private Krypton.Toolkit.KryptonTextBox txtReceivePaymentType;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonComboBox cmbAccount_id;
ReversedSettlementID主外字段不一致。
    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSettledForeignAmount;
private Krypton.Toolkit.KryptonTextBox txtSettledForeignAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSettledLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtSettledLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsAutoSettlement;
private Krypton.Toolkit.KryptonCheckBox chkIsAutoSettlement;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsReversed;
private Krypton.Toolkit.KryptonCheckBox chkIsReversed;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblReversedSettlementID;
private Krypton.Toolkit.KryptonTextBox txtReversedSettlementID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;
ReversedSettlementID主外字段不一致。
    
        
              private Krypton.Toolkit.KryptonLabel lblSettleDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpSettleDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSettlementType;
private Krypton.Toolkit.KryptonTextBox txtSettlementType;

    
        
              private Krypton.Toolkit.KryptonLabel lblEvidenceImagePath;
private Krypton.Toolkit.KryptonTextBox txtEvidenceImagePath;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

