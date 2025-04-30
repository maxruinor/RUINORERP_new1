
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/30/2025 15:18:07
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
    partial class tb_FM_PaymentSettlementQuery
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
     
     this.lblSettlementNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSettlementNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。
ReversedSettlementID主外字段不一致。
this.lblSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。
ReversedSettlementID主外字段不一致。
this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。
ReversedSettlementID主外字段不一致。
this.lblTargetBillNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTargetBillNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

ReversedSettlementID主外字段不一致。
ReversedSettlementID主外字段不一致。
ReversedSettlementID主外字段不一致。
ReversedSettlementID主外字段不一致。
this.lblSettledForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSettledForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSettledLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSettledLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsAutoSettlement = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsAutoSettlement = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsAutoSettlement.Values.Text ="";

this.lblIsReversed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsReversed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsReversed.Values.Text ="";

ReversedSettlementID主外字段不一致。
this.lblSettleDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpSettleDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

ReversedSettlementID主外字段不一致。
this.lblEvidenceImagePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEvidenceImagePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtEvidenceImagePath.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

ReversedSettlementID主外字段不一致。
    //for end
    this.SuspendLayout();
    
         //for start
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
ReversedSettlementID主外字段不一致。
           //#####SourceBillID###Int64
//属性测试75SourceBillID
ReversedSettlementID主外字段不一致。
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
ReversedSettlementID主外字段不一致。
           //#####SourceCurrencyID###Int64
//属性测试150SourceCurrencyID
ReversedSettlementID主外字段不一致。
           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,175);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 7;
this.lblExchangeRate.Text = "汇率";
//111======175
this.txtExchangeRate.Location = new System.Drawing.Point(173,171);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 7;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####TargetBizType###Int32
//属性测试200TargetBizType
ReversedSettlementID主外字段不一致。
           //#####TargetBillID###Int64
//属性测试225TargetBillID
ReversedSettlementID主外字段不一致。
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
ReversedSettlementID主外字段不一致。
           //#####ReceivePaymentType###Int32
//属性测试300ReceivePaymentType
ReversedSettlementID主外字段不一致。
           //#####Account_id###Int64
//属性测试325Account_id
ReversedSettlementID主外字段不一致。
           //#####CustomerVendor_ID###Int64
//属性测试350CustomerVendor_ID
ReversedSettlementID主外字段不一致。
           //#####SettledForeignAmount###Decimal
this.lblSettledForeignAmount.AutoSize = true;
this.lblSettledForeignAmount.Location = new System.Drawing.Point(100,375);
this.lblSettledForeignAmount.Name = "lblSettledForeignAmount";
this.lblSettledForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblSettledForeignAmount.TabIndex = 15;
this.lblSettledForeignAmount.Text = "核销金额外币";
//111======375
this.txtSettledForeignAmount.Location = new System.Drawing.Point(173,371);
this.txtSettledForeignAmount.Name ="txtSettledForeignAmount";
this.txtSettledForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtSettledForeignAmount.TabIndex = 15;
this.Controls.Add(this.lblSettledForeignAmount);
this.Controls.Add(this.txtSettledForeignAmount);

           //#####SettledLocalAmount###Decimal
this.lblSettledLocalAmount.AutoSize = true;
this.lblSettledLocalAmount.Location = new System.Drawing.Point(100,400);
this.lblSettledLocalAmount.Name = "lblSettledLocalAmount";
this.lblSettledLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSettledLocalAmount.TabIndex = 16;
this.lblSettledLocalAmount.Text = "核销金额本币";
//111======400
this.txtSettledLocalAmount.Location = new System.Drawing.Point(173,396);
this.txtSettledLocalAmount.Name ="txtSettledLocalAmount";
this.txtSettledLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSettledLocalAmount.TabIndex = 16;
this.Controls.Add(this.lblSettledLocalAmount);
this.Controls.Add(this.txtSettledLocalAmount);

           //#####IsAutoSettlement###Boolean
this.lblIsAutoSettlement.AutoSize = true;
this.lblIsAutoSettlement.Location = new System.Drawing.Point(100,425);
this.lblIsAutoSettlement.Name = "lblIsAutoSettlement";
this.lblIsAutoSettlement.Size = new System.Drawing.Size(41, 12);
this.lblIsAutoSettlement.TabIndex = 17;
this.lblIsAutoSettlement.Text = "是否自动核销";
this.chkIsAutoSettlement.Location = new System.Drawing.Point(173,421);
this.chkIsAutoSettlement.Name = "chkIsAutoSettlement";
this.chkIsAutoSettlement.Size = new System.Drawing.Size(100, 21);
this.chkIsAutoSettlement.TabIndex = 17;
this.Controls.Add(this.lblIsAutoSettlement);
this.Controls.Add(this.chkIsAutoSettlement);

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

           //#####ReversedSettlementID###Int64
//属性测试475ReversedSettlementID
ReversedSettlementID主外字段不一致。
           //#####SettleDate###DateTime
this.lblSettleDate.AutoSize = true;
this.lblSettleDate.Location = new System.Drawing.Point(100,500);
this.lblSettleDate.Name = "lblSettleDate";
this.lblSettleDate.Size = new System.Drawing.Size(41, 12);
this.lblSettleDate.TabIndex = 20;
this.lblSettleDate.Text = "核销日期";
//111======500
this.dtpSettleDate.Location = new System.Drawing.Point(173,496);
this.dtpSettleDate.Name ="dtpSettleDate";
this.dtpSettleDate.Size = new System.Drawing.Size(100, 21);
this.dtpSettleDate.TabIndex = 20;
this.Controls.Add(this.lblSettleDate);
this.Controls.Add(this.dtpSettleDate);

           //#####300Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,525);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 21;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,521);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 21;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####SettlementType###Int32
//属性测试550SettlementType
ReversedSettlementID主外字段不一致。
           //#####300EvidenceImagePath###String
this.lblEvidenceImagePath.AutoSize = true;
this.lblEvidenceImagePath.Location = new System.Drawing.Point(100,575);
this.lblEvidenceImagePath.Name = "lblEvidenceImagePath";
this.lblEvidenceImagePath.Size = new System.Drawing.Size(41, 12);
this.lblEvidenceImagePath.TabIndex = 23;
this.lblEvidenceImagePath.Text = "凭证图";
this.txtEvidenceImagePath.Location = new System.Drawing.Point(173,571);
this.txtEvidenceImagePath.Name = "txtEvidenceImagePath";
this.txtEvidenceImagePath.Size = new System.Drawing.Size(100, 21);
this.txtEvidenceImagePath.TabIndex = 23;
this.Controls.Add(this.lblEvidenceImagePath);
this.Controls.Add(this.txtEvidenceImagePath);

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
ReversedSettlementID主外字段不一致。
          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSettlementNo );
this.Controls.Add(this.txtSettlementNo );

                ReversedSettlementID主外字段不一致。
                ReversedSettlementID主外字段不一致。
                this.Controls.Add(this.lblSourceBillNO );
this.Controls.Add(this.txtSourceBillNO );

                ReversedSettlementID主外字段不一致。
                ReversedSettlementID主外字段不一致。
                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                ReversedSettlementID主外字段不一致。
                ReversedSettlementID主外字段不一致。
                this.Controls.Add(this.lblTargetBillNO );
this.Controls.Add(this.txtTargetBillNO );

                ReversedSettlementID主外字段不一致。
                ReversedSettlementID主外字段不一致。
                ReversedSettlementID主外字段不一致。
                ReversedSettlementID主外字段不一致。
                this.Controls.Add(this.lblSettledForeignAmount );
this.Controls.Add(this.txtSettledForeignAmount );

                this.Controls.Add(this.lblSettledLocalAmount );
this.Controls.Add(this.txtSettledLocalAmount );

                this.Controls.Add(this.lblIsAutoSettlement );
this.Controls.Add(this.chkIsAutoSettlement );

                this.Controls.Add(this.lblIsReversed );
this.Controls.Add(this.chkIsReversed );

                ReversedSettlementID主外字段不一致。
                this.Controls.Add(this.lblSettleDate );
this.Controls.Add(this.dtpSettleDate );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                ReversedSettlementID主外字段不一致。
                this.Controls.Add(this.lblEvidenceImagePath );
this.Controls.Add(this.txtEvidenceImagePath );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                ReversedSettlementID主外字段不一致。
                    
            this.Name = "tb_FM_PaymentSettlementQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSettlementNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSettlementNo;

    
        
              ReversedSettlementID主外字段不一致。
    
        
              ReversedSettlementID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNO;

    
        
              ReversedSettlementID主外字段不一致。
    
        
              ReversedSettlementID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              ReversedSettlementID主外字段不一致。
    
        
              ReversedSettlementID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTargetBillNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTargetBillNO;

    
        
              ReversedSettlementID主外字段不一致。
    
        
              ReversedSettlementID主外字段不一致。
    
        
              ReversedSettlementID主外字段不一致。
    
        
              ReversedSettlementID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSettledForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSettledForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSettledLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSettledLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsAutoSettlement;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsAutoSettlement;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsReversed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsReversed;

    
        
              ReversedSettlementID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSettleDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpSettleDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              ReversedSettlementID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEvidenceImagePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEvidenceImagePath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              ReversedSettlementID主外字段不一致。
    
    
   
 





    }
}


