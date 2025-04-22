
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
     
     this.lblPaymentId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPaymentId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();




this.lblSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();






this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSettledForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSettledForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSettledLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSettledLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPamountInWords = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPamountInWords = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSettleDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpSettleDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;


this.lblEvidenceImagePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEvidenceImagePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtEvidenceImagePath.Multiline = true;

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

           //#####SourceBilllID###Int64
//属性测试75SourceBilllID

           //#####SourceBillDetailID###Int64
//属性测试100SourceBillDetailID

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

           //#####Account_id###Int64
//属性测试175Account_id

           //#####Employee_ID###Int64
//属性测试200Employee_ID

           //#####CustomerVendor_ID###Int64
//属性测试225CustomerVendor_ID

           //#####Currency_ID###Int64
//属性测试250Currency_ID

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPaymentId );
this.Controls.Add(this.cmbPaymentId );

                
                
                
                this.Controls.Add(this.lblSourceBillNO );
this.Controls.Add(this.txtSourceBillNO );

                
                
                
                
                
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

                
                this.Controls.Add(this.lblEvidenceImagePath );
this.Controls.Add(this.txtEvidenceImagePath );

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

                
                    
            this.Name = "tb_FM_PaymentSettlementQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPaymentId;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNO;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSettledForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSettledForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSettledLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSettledLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPamountInWords;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPamountInWords;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSettleDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpSettleDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEvidenceImagePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEvidenceImagePath;

    
        
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


