// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/26/2025 22:26:18
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 记录收款 与应收的匹配，核销表
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

ReversedSettlementID主外字段不一致。this.lblBizType = new Krypton.Toolkit.KryptonLabel();
this.txtBizType = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblSourceBillID = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillID = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNO = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNO = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblSourceBizType = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBizType = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblSourceCurrencyID = new Krypton.Toolkit.KryptonLabel();
this.txtSourceCurrencyID = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtSourceExchangeRate = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblTargetBizType = new Krypton.Toolkit.KryptonLabel();
this.txtTargetBizType = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblTargetBillID = new Krypton.Toolkit.KryptonLabel();
this.txtTargetBillID = new Krypton.Toolkit.KryptonTextBox();

this.lblTargetBillNO = new Krypton.Toolkit.KryptonLabel();
this.txtTargetBillNO = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblTargetCurrencyID = new Krypton.Toolkit.KryptonLabel();
this.txtTargetCurrencyID = new Krypton.Toolkit.KryptonTextBox();

this.lblTargetExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtTargetExchangeRate = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblReceivePaymentType = new Krypton.Toolkit.KryptonLabel();
this.txtReceivePaymentType = new Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_id = new Krypton.Toolkit.KryptonTextBox();

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
this.lblSettlementNo.Text = "来源单号";
this.txtSettlementNo.Location = new System.Drawing.Point(173,21);
this.txtSettlementNo.Name = "txtSettlementNo";
this.txtSettlementNo.Size = new System.Drawing.Size(100, 21);
this.txtSettlementNo.TabIndex = 1;
this.Controls.Add(this.lblSettlementNo);
this.Controls.Add(this.txtSettlementNo);

           //#####BizType###Int32
//属性测试50BizType
ReversedSettlementID主外字段不一致。this.lblBizType.AutoSize = true;
this.lblBizType.Location = new System.Drawing.Point(100,50);
this.lblBizType.Name = "lblBizType";
this.lblBizType.Size = new System.Drawing.Size(41, 12);
this.lblBizType.TabIndex = 2;
this.lblBizType.Text = "业务类型";
this.txtBizType.Location = new System.Drawing.Point(173,46);
this.txtBizType.Name = "txtBizType";
this.txtBizType.Size = new System.Drawing.Size(100, 21);
this.txtBizType.TabIndex = 2;
this.Controls.Add(this.lblBizType);
this.Controls.Add(this.txtBizType);

           //#####SourceBillID###Int64
//属性测试75SourceBillID
ReversedSettlementID主外字段不一致。this.lblSourceBillID.AutoSize = true;
this.lblSourceBillID.Location = new System.Drawing.Point(100,75);
this.lblSourceBillID.Name = "lblSourceBillID";
this.lblSourceBillID.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillID.TabIndex = 3;
this.lblSourceBillID.Text = "来源单据";
this.txtSourceBillID.Location = new System.Drawing.Point(173,71);
this.txtSourceBillID.Name = "txtSourceBillID";
this.txtSourceBillID.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillID.TabIndex = 3;
this.Controls.Add(this.lblSourceBillID);
this.Controls.Add(this.txtSourceBillID);

           //#####30SourceBillNO###String
this.lblSourceBillNO.AutoSize = true;
this.lblSourceBillNO.Location = new System.Drawing.Point(100,100);
this.lblSourceBillNO.Name = "lblSourceBillNO";
this.lblSourceBillNO.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNO.TabIndex = 4;
this.lblSourceBillNO.Text = "来源单据编号";
this.txtSourceBillNO.Location = new System.Drawing.Point(173,96);
this.txtSourceBillNO.Name = "txtSourceBillNO";
this.txtSourceBillNO.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNO.TabIndex = 4;
this.Controls.Add(this.lblSourceBillNO);
this.Controls.Add(this.txtSourceBillNO);

           //#####SourceBizType###Int32
//属性测试125SourceBizType
ReversedSettlementID主外字段不一致。this.lblSourceBizType.AutoSize = true;
this.lblSourceBizType.Location = new System.Drawing.Point(100,125);
this.lblSourceBizType.Name = "lblSourceBizType";
this.lblSourceBizType.Size = new System.Drawing.Size(41, 12);
this.lblSourceBizType.TabIndex = 5;
this.lblSourceBizType.Text = "来源单据类型";
this.txtSourceBizType.Location = new System.Drawing.Point(173,121);
this.txtSourceBizType.Name = "txtSourceBizType";
this.txtSourceBizType.Size = new System.Drawing.Size(100, 21);
this.txtSourceBizType.TabIndex = 5;
this.Controls.Add(this.lblSourceBizType);
this.Controls.Add(this.txtSourceBizType);

           //#####SourceCurrencyID###Int64
//属性测试150SourceCurrencyID
ReversedSettlementID主外字段不一致。this.lblSourceCurrencyID.AutoSize = true;
this.lblSourceCurrencyID.Location = new System.Drawing.Point(100,150);
this.lblSourceCurrencyID.Name = "lblSourceCurrencyID";
this.lblSourceCurrencyID.Size = new System.Drawing.Size(41, 12);
this.lblSourceCurrencyID.TabIndex = 6;
this.lblSourceCurrencyID.Text = "来源币种";
this.txtSourceCurrencyID.Location = new System.Drawing.Point(173,146);
this.txtSourceCurrencyID.Name = "txtSourceCurrencyID";
this.txtSourceCurrencyID.Size = new System.Drawing.Size(100, 21);
this.txtSourceCurrencyID.TabIndex = 6;
this.Controls.Add(this.lblSourceCurrencyID);
this.Controls.Add(this.txtSourceCurrencyID);

           //#####SourceExchangeRate###Decimal
this.lblSourceExchangeRate.AutoSize = true;
this.lblSourceExchangeRate.Location = new System.Drawing.Point(100,175);
this.lblSourceExchangeRate.Name = "lblSourceExchangeRate";
this.lblSourceExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblSourceExchangeRate.TabIndex = 7;
this.lblSourceExchangeRate.Text = "来源汇率";
//111======175
this.txtSourceExchangeRate.Location = new System.Drawing.Point(173,171);
this.txtSourceExchangeRate.Name ="txtSourceExchangeRate";
this.txtSourceExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtSourceExchangeRate.TabIndex = 7;
this.Controls.Add(this.lblSourceExchangeRate);
this.Controls.Add(this.txtSourceExchangeRate);

           //#####TargetBizType###Int32
//属性测试200TargetBizType
ReversedSettlementID主外字段不一致。this.lblTargetBizType.AutoSize = true;
this.lblTargetBizType.Location = new System.Drawing.Point(100,200);
this.lblTargetBizType.Name = "lblTargetBizType";
this.lblTargetBizType.Size = new System.Drawing.Size(41, 12);
this.lblTargetBizType.TabIndex = 8;
this.lblTargetBizType.Text = "目标单据类型";
this.txtTargetBizType.Location = new System.Drawing.Point(173,196);
this.txtTargetBizType.Name = "txtTargetBizType";
this.txtTargetBizType.Size = new System.Drawing.Size(100, 21);
this.txtTargetBizType.TabIndex = 8;
this.Controls.Add(this.lblTargetBizType);
this.Controls.Add(this.txtTargetBizType);

           //#####TargetBillID###Int64
//属性测试225TargetBillID
ReversedSettlementID主外字段不一致。this.lblTargetBillID.AutoSize = true;
this.lblTargetBillID.Location = new System.Drawing.Point(100,225);
this.lblTargetBillID.Name = "lblTargetBillID";
this.lblTargetBillID.Size = new System.Drawing.Size(41, 12);
this.lblTargetBillID.TabIndex = 9;
this.lblTargetBillID.Text = "目标单据";
this.txtTargetBillID.Location = new System.Drawing.Point(173,221);
this.txtTargetBillID.Name = "txtTargetBillID";
this.txtTargetBillID.Size = new System.Drawing.Size(100, 21);
this.txtTargetBillID.TabIndex = 9;
this.Controls.Add(this.lblTargetBillID);
this.Controls.Add(this.txtTargetBillID);

           //#####30TargetBillNO###String
this.lblTargetBillNO.AutoSize = true;
this.lblTargetBillNO.Location = new System.Drawing.Point(100,250);
this.lblTargetBillNO.Name = "lblTargetBillNO";
this.lblTargetBillNO.Size = new System.Drawing.Size(41, 12);
this.lblTargetBillNO.TabIndex = 10;
this.lblTargetBillNO.Text = "目标单据编号";
this.txtTargetBillNO.Location = new System.Drawing.Point(173,246);
this.txtTargetBillNO.Name = "txtTargetBillNO";
this.txtTargetBillNO.Size = new System.Drawing.Size(100, 21);
this.txtTargetBillNO.TabIndex = 10;
this.Controls.Add(this.lblTargetBillNO);
this.Controls.Add(this.txtTargetBillNO);

           //#####TargetCurrencyID###Int64
//属性测试275TargetCurrencyID
ReversedSettlementID主外字段不一致。this.lblTargetCurrencyID.AutoSize = true;
this.lblTargetCurrencyID.Location = new System.Drawing.Point(100,275);
this.lblTargetCurrencyID.Name = "lblTargetCurrencyID";
this.lblTargetCurrencyID.Size = new System.Drawing.Size(41, 12);
this.lblTargetCurrencyID.TabIndex = 11;
this.lblTargetCurrencyID.Text = "目标币种";
this.txtTargetCurrencyID.Location = new System.Drawing.Point(173,271);
this.txtTargetCurrencyID.Name = "txtTargetCurrencyID";
this.txtTargetCurrencyID.Size = new System.Drawing.Size(100, 21);
this.txtTargetCurrencyID.TabIndex = 11;
this.Controls.Add(this.lblTargetCurrencyID);
this.Controls.Add(this.txtTargetCurrencyID);

           //#####TargetExchangeRate###Decimal
this.lblTargetExchangeRate.AutoSize = true;
this.lblTargetExchangeRate.Location = new System.Drawing.Point(100,300);
this.lblTargetExchangeRate.Name = "lblTargetExchangeRate";
this.lblTargetExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblTargetExchangeRate.TabIndex = 12;
this.lblTargetExchangeRate.Text = "目标汇率";
//111======300
this.txtTargetExchangeRate.Location = new System.Drawing.Point(173,296);
this.txtTargetExchangeRate.Name ="txtTargetExchangeRate";
this.txtTargetExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtTargetExchangeRate.TabIndex = 12;
this.Controls.Add(this.lblTargetExchangeRate);
this.Controls.Add(this.txtTargetExchangeRate);

           //#####ReceivePaymentType###Int64
//属性测试325ReceivePaymentType
ReversedSettlementID主外字段不一致。this.lblReceivePaymentType.AutoSize = true;
this.lblReceivePaymentType.Location = new System.Drawing.Point(100,325);
this.lblReceivePaymentType.Name = "lblReceivePaymentType";
this.lblReceivePaymentType.Size = new System.Drawing.Size(41, 12);
this.lblReceivePaymentType.TabIndex = 13;
this.lblReceivePaymentType.Text = "收付类型";
this.txtReceivePaymentType.Location = new System.Drawing.Point(173,321);
this.txtReceivePaymentType.Name = "txtReceivePaymentType";
this.txtReceivePaymentType.Size = new System.Drawing.Size(100, 21);
this.txtReceivePaymentType.TabIndex = 13;
this.Controls.Add(this.lblReceivePaymentType);
this.Controls.Add(this.txtReceivePaymentType);

           //#####Account_id###Int64
//属性测试350Account_id
ReversedSettlementID主外字段不一致。this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,350);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 14;
this.lblAccount_id.Text = "公司账户";
this.txtAccount_id.Location = new System.Drawing.Point(173,346);
this.txtAccount_id.Name = "txtAccount_id";
this.txtAccount_id.Size = new System.Drawing.Size(100, 21);
this.txtAccount_id.TabIndex = 14;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.txtAccount_id);

           //#####CustomerVendor_ID###Int64
//属性测试375CustomerVendor_ID
ReversedSettlementID主外字段不一致。this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,375);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 15;
this.lblCustomerVendor_ID.Text = "往来单位";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,371);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 15;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####SettledForeignAmount###Decimal
this.lblSettledForeignAmount.AutoSize = true;
this.lblSettledForeignAmount.Location = new System.Drawing.Point(100,400);
this.lblSettledForeignAmount.Name = "lblSettledForeignAmount";
this.lblSettledForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblSettledForeignAmount.TabIndex = 16;
this.lblSettledForeignAmount.Text = "核销金额外币";
//111======400
this.txtSettledForeignAmount.Location = new System.Drawing.Point(173,396);
this.txtSettledForeignAmount.Name ="txtSettledForeignAmount";
this.txtSettledForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtSettledForeignAmount.TabIndex = 16;
this.Controls.Add(this.lblSettledForeignAmount);
this.Controls.Add(this.txtSettledForeignAmount);

           //#####SettledLocalAmount###Decimal
this.lblSettledLocalAmount.AutoSize = true;
this.lblSettledLocalAmount.Location = new System.Drawing.Point(100,425);
this.lblSettledLocalAmount.Name = "lblSettledLocalAmount";
this.lblSettledLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSettledLocalAmount.TabIndex = 17;
this.lblSettledLocalAmount.Text = "核销金额本币";
//111======425
this.txtSettledLocalAmount.Location = new System.Drawing.Point(173,421);
this.txtSettledLocalAmount.Name ="txtSettledLocalAmount";
this.txtSettledLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSettledLocalAmount.TabIndex = 17;
this.Controls.Add(this.lblSettledLocalAmount);
this.Controls.Add(this.txtSettledLocalAmount);

           //#####IsAutoSettlement###Boolean
this.lblIsAutoSettlement.AutoSize = true;
this.lblIsAutoSettlement.Location = new System.Drawing.Point(100,450);
this.lblIsAutoSettlement.Name = "lblIsAutoSettlement";
this.lblIsAutoSettlement.Size = new System.Drawing.Size(41, 12);
this.lblIsAutoSettlement.TabIndex = 18;
this.lblIsAutoSettlement.Text = "是否自动核销";
this.chkIsAutoSettlement.Location = new System.Drawing.Point(173,446);
this.chkIsAutoSettlement.Name = "chkIsAutoSettlement";
this.chkIsAutoSettlement.Size = new System.Drawing.Size(100, 21);
this.chkIsAutoSettlement.TabIndex = 18;
this.Controls.Add(this.lblIsAutoSettlement);
this.Controls.Add(this.chkIsAutoSettlement);

           //#####IsReversed###Boolean
this.lblIsReversed.AutoSize = true;
this.lblIsReversed.Location = new System.Drawing.Point(100,475);
this.lblIsReversed.Name = "lblIsReversed";
this.lblIsReversed.Size = new System.Drawing.Size(41, 12);
this.lblIsReversed.TabIndex = 19;
this.lblIsReversed.Text = "是否冲销";
this.chkIsReversed.Location = new System.Drawing.Point(173,471);
this.chkIsReversed.Name = "chkIsReversed";
this.chkIsReversed.Size = new System.Drawing.Size(100, 21);
this.chkIsReversed.TabIndex = 19;
this.Controls.Add(this.lblIsReversed);
this.Controls.Add(this.chkIsReversed);

           //#####ReversedSettlementID###Int64
//属性测试500ReversedSettlementID
ReversedSettlementID主外字段不一致。this.lblReversedSettlementID.AutoSize = true;
this.lblReversedSettlementID.Location = new System.Drawing.Point(100,500);
this.lblReversedSettlementID.Name = "lblReversedSettlementID";
this.lblReversedSettlementID.Size = new System.Drawing.Size(41, 12);
this.lblReversedSettlementID.TabIndex = 20;
this.lblReversedSettlementID.Text = "对冲记录";
this.txtReversedSettlementID.Location = new System.Drawing.Point(173,496);
this.txtReversedSettlementID.Name = "txtReversedSettlementID";
this.txtReversedSettlementID.Size = new System.Drawing.Size(100, 21);
this.txtReversedSettlementID.TabIndex = 20;
this.Controls.Add(this.lblReversedSettlementID);
this.Controls.Add(this.txtReversedSettlementID);

           //#####SettleDate###DateTime
this.lblSettleDate.AutoSize = true;
this.lblSettleDate.Location = new System.Drawing.Point(100,525);
this.lblSettleDate.Name = "lblSettleDate";
this.lblSettleDate.Size = new System.Drawing.Size(41, 12);
this.lblSettleDate.TabIndex = 21;
this.lblSettleDate.Text = "核销日期";
//111======525
this.dtpSettleDate.Location = new System.Drawing.Point(173,521);
this.dtpSettleDate.Name ="dtpSettleDate";
this.dtpSettleDate.Size = new System.Drawing.Size(100, 21);
this.dtpSettleDate.TabIndex = 21;
this.Controls.Add(this.lblSettleDate);
this.Controls.Add(this.dtpSettleDate);

           //#####300Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,550);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 22;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,546);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 22;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####SettlementType###Int32
//属性测试575SettlementType
ReversedSettlementID主外字段不一致。this.lblSettlementType.AutoSize = true;
this.lblSettlementType.Location = new System.Drawing.Point(100,575);
this.lblSettlementType.Name = "lblSettlementType";
this.lblSettlementType.Size = new System.Drawing.Size(41, 12);
this.lblSettlementType.TabIndex = 23;
this.lblSettlementType.Text = "核销状态";
this.txtSettlementType.Location = new System.Drawing.Point(173,571);
this.txtSettlementType.Name = "txtSettlementType";
this.txtSettlementType.Size = new System.Drawing.Size(100, 21);
this.txtSettlementType.TabIndex = 23;
this.Controls.Add(this.lblSettlementType);
this.Controls.Add(this.txtSettlementType);

           //#####300EvidenceImagePath###String
this.lblEvidenceImagePath.AutoSize = true;
this.lblEvidenceImagePath.Location = new System.Drawing.Point(100,600);
this.lblEvidenceImagePath.Name = "lblEvidenceImagePath";
this.lblEvidenceImagePath.Size = new System.Drawing.Size(41, 12);
this.lblEvidenceImagePath.TabIndex = 24;
this.lblEvidenceImagePath.Text = "凭证图";
this.txtEvidenceImagePath.Location = new System.Drawing.Point(173,596);
this.txtEvidenceImagePath.Name = "txtEvidenceImagePath";
this.txtEvidenceImagePath.Size = new System.Drawing.Size(100, 21);
this.txtEvidenceImagePath.TabIndex = 24;
this.Controls.Add(this.lblEvidenceImagePath);
this.Controls.Add(this.txtEvidenceImagePath);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,625);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 25;
this.lblCreated_at.Text = "创建时间";
//111======625
this.dtpCreated_at.Location = new System.Drawing.Point(173,621);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 25;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试650Created_by
ReversedSettlementID主外字段不一致。this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,650);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 26;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,646);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 26;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
           // this.kryptonPanel1.TabIndex = 26;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSettlementNo );
this.Controls.Add(this.txtSettlementNo );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblBizType );
this.Controls.Add(this.txtBizType );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblSourceBillID );
this.Controls.Add(this.txtSourceBillID );

                this.Controls.Add(this.lblSourceBillNO );
this.Controls.Add(this.txtSourceBillNO );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblSourceBizType );
this.Controls.Add(this.txtSourceBizType );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblSourceCurrencyID );
this.Controls.Add(this.txtSourceCurrencyID );

                this.Controls.Add(this.lblSourceExchangeRate );
this.Controls.Add(this.txtSourceExchangeRate );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblTargetBizType );
this.Controls.Add(this.txtTargetBizType );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblTargetBillID );
this.Controls.Add(this.txtTargetBillID );

                this.Controls.Add(this.lblTargetBillNO );
this.Controls.Add(this.txtTargetBillNO );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblTargetCurrencyID );
this.Controls.Add(this.txtTargetCurrencyID );

                this.Controls.Add(this.lblTargetExchangeRate );
this.Controls.Add(this.txtTargetExchangeRate );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblReceivePaymentType );
this.Controls.Add(this.txtReceivePaymentType );

                ReversedSettlementID主外字段不一致。this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.txtAccount_id );

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

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblBizType;
private Krypton.Toolkit.KryptonTextBox txtBizType;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSourceBillID;
private Krypton.Toolkit.KryptonTextBox txtSourceBillID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNO;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSourceBizType;
private Krypton.Toolkit.KryptonTextBox txtSourceBizType;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSourceCurrencyID;
private Krypton.Toolkit.KryptonTextBox txtSourceCurrencyID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtSourceExchangeRate;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblTargetBizType;
private Krypton.Toolkit.KryptonTextBox txtTargetBizType;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblTargetBillID;
private Krypton.Toolkit.KryptonTextBox txtTargetBillID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTargetBillNO;
private Krypton.Toolkit.KryptonTextBox txtTargetBillNO;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblTargetCurrencyID;
private Krypton.Toolkit.KryptonTextBox txtTargetCurrencyID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTargetExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtTargetExchangeRate;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblReceivePaymentType;
private Krypton.Toolkit.KryptonTextBox txtReceivePaymentType;

    
        
              ReversedSettlementID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonTextBox txtAccount_id;

    
        
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

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

