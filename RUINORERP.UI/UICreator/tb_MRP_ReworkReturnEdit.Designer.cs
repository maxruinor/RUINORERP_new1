// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 18:30:49
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 返工退库
    /// </summary>
    partial class tb_MRP_ReworkReturnEdit
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
            this.lblReturnDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpReturnDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblExpectedReturnDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpExpectedReturnDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblReasonForRework = new Krypton.Toolkit.KryptonLabel();
            this.txtReasonForRework = new Krypton.Toolkit.KryptonTextBox();
            this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
            this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
            this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
            this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
            this.lblKeepAccountsType = new Krypton.Toolkit.KryptonLabel();
            this.txtKeepAccountsType = new Krypton.Toolkit.KryptonTextBox();
            this.lblReceiptInvoiceClosed = new Krypton.Toolkit.KryptonLabel();
            this.chkReceiptInvoiceClosed = new Krypton.Toolkit.KryptonCheckBox();
            this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
            this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
            this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
            this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();
            this.lblGenerateVouchers = new Krypton.Toolkit.KryptonLabel();
            this.chkGenerateVouchers = new Krypton.Toolkit.KryptonCheckBox();
            this.lblVoucherID = new Krypton.Toolkit.KryptonLabel();
            this.txtVoucherID = new Krypton.Toolkit.KryptonTextBox();
            this.SuspendLayout();
            // 
            // lblReturnDate
            // 
            this.lblReturnDate.Location = new System.Drawing.Point(100, 225);
            this.lblReturnDate.Name = "lblReturnDate";
            this.lblReturnDate.Size = new System.Drawing.Size(62, 20);
            this.lblReturnDate.TabIndex = 9;
            this.lblReturnDate.Values.Text = "退回日期";
            // 
            // dtpReturnDate
            // 
            this.dtpReturnDate.Location = new System.Drawing.Point(173, 221);
            this.dtpReturnDate.Name = "dtpReturnDate";
            this.dtpReturnDate.Size = new System.Drawing.Size(100, 21);
            this.dtpReturnDate.TabIndex = 9;
            // 
            // lblExpectedReturnDate
            // 
            this.lblExpectedReturnDate.Location = new System.Drawing.Point(100, 250);
            this.lblExpectedReturnDate.Name = "lblExpectedReturnDate";
            this.lblExpectedReturnDate.Size = new System.Drawing.Size(62, 20);
            this.lblExpectedReturnDate.TabIndex = 10;
            this.lblExpectedReturnDate.Values.Text = "预完工期";
            // 
            // dtpExpectedReturnDate
            // 
            this.dtpExpectedReturnDate.Location = new System.Drawing.Point(173, 246);
            this.dtpExpectedReturnDate.Name = "dtpExpectedReturnDate";
            this.dtpExpectedReturnDate.ShowCheckBox = true;
            this.dtpExpectedReturnDate.Size = new System.Drawing.Size(100, 21);
            this.dtpExpectedReturnDate.TabIndex = 10;
            // 
            // lblReasonForRework
            // 
            this.lblReasonForRework.Location = new System.Drawing.Point(100, 275);
            this.lblReasonForRework.Name = "lblReasonForRework";
            this.lblReasonForRework.Size = new System.Drawing.Size(62, 20);
            this.lblReasonForRework.TabIndex = 11;
            this.lblReasonForRework.Values.Text = "返工原因";
            // 
            // txtReasonForRework
            // 
            this.txtReasonForRework.Location = new System.Drawing.Point(173, 271);
            this.txtReasonForRework.Multiline = true;
            this.txtReasonForRework.Name = "txtReasonForRework";
            this.txtReasonForRework.Size = new System.Drawing.Size(100, 21);
            this.txtReasonForRework.TabIndex = 11;
            // 
            // lblisdeleted
            // 
            this.lblisdeleted.Location = new System.Drawing.Point(100, 300);
            this.lblisdeleted.Name = "lblisdeleted";
            this.lblisdeleted.Size = new System.Drawing.Size(62, 20);
            this.lblisdeleted.TabIndex = 12;
            this.lblisdeleted.Values.Text = "逻辑删除";
            // 
            // chkisdeleted
            // 
            this.chkisdeleted.Location = new System.Drawing.Point(173, 296);
            this.chkisdeleted.Name = "chkisdeleted";
            this.chkisdeleted.Size = new System.Drawing.Size(19, 13);
            this.chkisdeleted.TabIndex = 12;
            this.chkisdeleted.Values.Text = "";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(100, 425);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 17;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(173, 421);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(100, 21);
            this.txtNotes.TabIndex = 17;
            // 
            // lblApprovalOpinions
            // 
            this.lblApprovalOpinions.Location = new System.Drawing.Point(100, 450);
            this.lblApprovalOpinions.Name = "lblApprovalOpinions";
            this.lblApprovalOpinions.Size = new System.Drawing.Size(62, 20);
            this.lblApprovalOpinions.TabIndex = 18;
            this.lblApprovalOpinions.Values.Text = "审批意见";
            // 
            // txtApprovalOpinions
            // 
            this.txtApprovalOpinions.Location = new System.Drawing.Point(173, 446);
            this.txtApprovalOpinions.Name = "txtApprovalOpinions";
            this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 23);
            this.txtApprovalOpinions.TabIndex = 18;
            // 
            // lblApprovalResults
            // 
            this.lblApprovalResults.Location = new System.Drawing.Point(100, 500);
            this.lblApprovalResults.Name = "lblApprovalResults";
            this.lblApprovalResults.Size = new System.Drawing.Size(62, 20);
            this.lblApprovalResults.TabIndex = 20;
            this.lblApprovalResults.Values.Text = "审批结果";
            // 
            // chkApprovalResults
            // 
            this.chkApprovalResults.Location = new System.Drawing.Point(173, 496);
            this.chkApprovalResults.Name = "chkApprovalResults";
            this.chkApprovalResults.Size = new System.Drawing.Size(19, 13);
            this.chkApprovalResults.TabIndex = 20;
            this.chkApprovalResults.Values.Text = "";
            // 
            // lblKeepAccountsType
            // 
            this.lblKeepAccountsType.Location = new System.Drawing.Point(100, 525);
            this.lblKeepAccountsType.Name = "lblKeepAccountsType";
            this.lblKeepAccountsType.Size = new System.Drawing.Size(62, 20);
            this.lblKeepAccountsType.TabIndex = 21;
            this.lblKeepAccountsType.Values.Text = "立帐类型";
            // 
            // txtKeepAccountsType
            // 
            this.txtKeepAccountsType.Location = new System.Drawing.Point(173, 521);
            this.txtKeepAccountsType.Name = "txtKeepAccountsType";
            this.txtKeepAccountsType.Size = new System.Drawing.Size(100, 23);
            this.txtKeepAccountsType.TabIndex = 21;
            // 
            // lblReceiptInvoiceClosed
            // 
            this.lblReceiptInvoiceClosed.Location = new System.Drawing.Point(100, 550);
            this.lblReceiptInvoiceClosed.Name = "lblReceiptInvoiceClosed";
            this.lblReceiptInvoiceClosed.Size = new System.Drawing.Size(62, 20);
            this.lblReceiptInvoiceClosed.TabIndex = 22;
            this.lblReceiptInvoiceClosed.Values.Text = "立帐结案";
            // 
            // chkReceiptInvoiceClosed
            // 
            this.chkReceiptInvoiceClosed.Location = new System.Drawing.Point(173, 546);
            this.chkReceiptInvoiceClosed.Name = "chkReceiptInvoiceClosed";
            this.chkReceiptInvoiceClosed.Size = new System.Drawing.Size(19, 13);
            this.chkReceiptInvoiceClosed.TabIndex = 22;
            this.chkReceiptInvoiceClosed.Values.Text = "";
            // 
            // lblDataStatus
            // 
            this.lblDataStatus.Location = new System.Drawing.Point(100, 575);
            this.lblDataStatus.Name = "lblDataStatus";
            this.lblDataStatus.Size = new System.Drawing.Size(62, 20);
            this.lblDataStatus.TabIndex = 23;
            this.lblDataStatus.Values.Text = "数据状态";
            // 
            // txtDataStatus
            // 
            this.txtDataStatus.Location = new System.Drawing.Point(173, 571);
            this.txtDataStatus.Name = "txtDataStatus";
            this.txtDataStatus.Size = new System.Drawing.Size(100, 23);
            this.txtDataStatus.TabIndex = 23;
            // 
            // lblApprover_by
            // 
            this.lblApprover_by.Location = new System.Drawing.Point(100, 600);
            this.lblApprover_by.Name = "lblApprover_by";
            this.lblApprover_by.Size = new System.Drawing.Size(49, 20);
            this.lblApprover_by.TabIndex = 24;
            this.lblApprover_by.Values.Text = "审批人";
            // 
            // txtApprover_by
            // 
            this.txtApprover_by.Location = new System.Drawing.Point(173, 596);
            this.txtApprover_by.Name = "txtApprover_by";
            this.txtApprover_by.Size = new System.Drawing.Size(100, 23);
            this.txtApprover_by.TabIndex = 24;
            // 
            // lblApprover_at
            // 
            this.lblApprover_at.Location = new System.Drawing.Point(100, 625);
            this.lblApprover_at.Name = "lblApprover_at";
            this.lblApprover_at.Size = new System.Drawing.Size(62, 20);
            this.lblApprover_at.TabIndex = 25;
            this.lblApprover_at.Values.Text = "审批时间";
            // 
            // dtpApprover_at
            // 
            this.dtpApprover_at.Location = new System.Drawing.Point(173, 621);
            this.dtpApprover_at.Name = "dtpApprover_at";
            this.dtpApprover_at.ShowCheckBox = true;
            this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
            this.dtpApprover_at.TabIndex = 25;
            // 
            // lblPrintStatus
            // 
            this.lblPrintStatus.Location = new System.Drawing.Point(100, 650);
            this.lblPrintStatus.Name = "lblPrintStatus";
            this.lblPrintStatus.Size = new System.Drawing.Size(62, 20);
            this.lblPrintStatus.TabIndex = 26;
            this.lblPrintStatus.Values.Text = "打印状态";
            // 
            // txtPrintStatus
            // 
            this.txtPrintStatus.Location = new System.Drawing.Point(173, 646);
            this.txtPrintStatus.Name = "txtPrintStatus";
            this.txtPrintStatus.Size = new System.Drawing.Size(100, 23);
            this.txtPrintStatus.TabIndex = 26;
            // 
            // lblGenerateVouchers
            // 
            this.lblGenerateVouchers.Location = new System.Drawing.Point(100, 675);
            this.lblGenerateVouchers.Name = "lblGenerateVouchers";
            this.lblGenerateVouchers.Size = new System.Drawing.Size(62, 20);
            this.lblGenerateVouchers.TabIndex = 27;
            this.lblGenerateVouchers.Values.Text = "生成凭证";
            // 
            // chkGenerateVouchers
            // 
            this.chkGenerateVouchers.Location = new System.Drawing.Point(173, 671);
            this.chkGenerateVouchers.Name = "chkGenerateVouchers";
            this.chkGenerateVouchers.Size = new System.Drawing.Size(19, 13);
            this.chkGenerateVouchers.TabIndex = 27;
            this.chkGenerateVouchers.Values.Text = "";
            // 
            // lblVoucherID
            // 
            this.lblVoucherID.Location = new System.Drawing.Point(100, 700);
            this.lblVoucherID.Name = "lblVoucherID";
            this.lblVoucherID.Size = new System.Drawing.Size(62, 20);
            this.lblVoucherID.TabIndex = 28;
            this.lblVoucherID.Values.Text = "凭证号码";
            // 
            // txtVoucherID
            // 
            this.txtVoucherID.Location = new System.Drawing.Point(173, 696);
            this.txtVoucherID.Name = "txtVoucherID";
            this.txtVoucherID.Size = new System.Drawing.Size(100, 23);
            this.txtVoucherID.TabIndex = 28;
            // 
            // tb_MRP_ReworkReturnEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblReturnDate);
            this.Controls.Add(this.dtpReturnDate);
            this.Controls.Add(this.lblExpectedReturnDate);
            this.Controls.Add(this.dtpExpectedReturnDate);
            this.Controls.Add(this.lblReasonForRework);
            this.Controls.Add(this.txtReasonForRework);
            this.Controls.Add(this.lblisdeleted);
            this.Controls.Add(this.chkisdeleted);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblApprovalOpinions);
            this.Controls.Add(this.txtApprovalOpinions);
            this.Controls.Add(this.lblApprovalResults);
            this.Controls.Add(this.chkApprovalResults);
            this.Controls.Add(this.lblKeepAccountsType);
            this.Controls.Add(this.txtKeepAccountsType);
            this.Controls.Add(this.lblReceiptInvoiceClosed);
            this.Controls.Add(this.chkReceiptInvoiceClosed);
            this.Controls.Add(this.lblDataStatus);
            this.Controls.Add(this.txtDataStatus);
            this.Controls.Add(this.lblApprover_by);
            this.Controls.Add(this.txtApprover_by);
            this.Controls.Add(this.lblApprover_at);
            this.Controls.Add(this.dtpApprover_at);
            this.Controls.Add(this.lblPrintStatus);
            this.Controls.Add(this.txtPrintStatus);
            this.Controls.Add(this.lblGenerateVouchers);
            this.Controls.Add(this.chkGenerateVouchers);
            this.Controls.Add(this.lblVoucherID);
            this.Controls.Add(this.txtVoucherID);
            this.Name = "tb_MRP_ReworkReturnEdit";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start








    
        
              private Krypton.Toolkit.KryptonLabel lblReturnDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpectedReturnDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpExpectedReturnDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblReasonForRework;
private Krypton.Toolkit.KryptonTextBox txtReasonForRework;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;





    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblKeepAccountsType;
private Krypton.Toolkit.KryptonTextBox txtKeepAccountsType;

    
        
              private Krypton.Toolkit.KryptonLabel lblReceiptInvoiceClosed;
private Krypton.Toolkit.KryptonCheckBox chkReceiptInvoiceClosed;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblGenerateVouchers;
private Krypton.Toolkit.KryptonCheckBox chkGenerateVouchers;

    
        
              private Krypton.Toolkit.KryptonLabel lblVoucherID;
private Krypton.Toolkit.KryptonTextBox txtVoucherID;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

