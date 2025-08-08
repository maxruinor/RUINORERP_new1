
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:28
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 发票
    /// </summary>
    partial class tb_FM_InvoiceQuery
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
     
     this.lblInvoiceNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInvoiceNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBillingInfo_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBillingInfo_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblInvoiceDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpInvoiceDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalWithTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalWithTax = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsRed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsRed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsRed.Values.Text ="";

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;


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
                 //#####60InvoiceNo###String
this.lblInvoiceNo.AutoSize = true;
this.lblInvoiceNo.Location = new System.Drawing.Point(100,25);
this.lblInvoiceNo.Name = "lblInvoiceNo";
this.lblInvoiceNo.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceNo.TabIndex = 1;
this.lblInvoiceNo.Text = "发票号码";
this.txtInvoiceNo.Location = new System.Drawing.Point(173,21);
this.txtInvoiceNo.Name = "txtInvoiceNo";
this.txtInvoiceNo.Size = new System.Drawing.Size(100, 21);
this.txtInvoiceNo.TabIndex = 1;
this.Controls.Add(this.lblInvoiceNo);
this.Controls.Add(this.txtInvoiceNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "付款单位";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####BillingInfo_ID###Int64
//属性测试75BillingInfo_ID
//属性测试75BillingInfo_ID
this.lblBillingInfo_ID.AutoSize = true;
this.lblBillingInfo_ID.Location = new System.Drawing.Point(100,75);
this.lblBillingInfo_ID.Name = "lblBillingInfo_ID";
this.lblBillingInfo_ID.Size = new System.Drawing.Size(41, 12);
this.lblBillingInfo_ID.TabIndex = 3;
this.lblBillingInfo_ID.Text = "开票资料";
//111======75
this.cmbBillingInfo_ID.Location = new System.Drawing.Point(173,71);
this.cmbBillingInfo_ID.Name ="cmbBillingInfo_ID";
this.cmbBillingInfo_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBillingInfo_ID.TabIndex = 3;
this.Controls.Add(this.lblBillingInfo_ID);
this.Controls.Add(this.cmbBillingInfo_ID);

           //#####InvoiceDate###DateTime
this.lblInvoiceDate.AutoSize = true;
this.lblInvoiceDate.Location = new System.Drawing.Point(100,100);
this.lblInvoiceDate.Name = "lblInvoiceDate";
this.lblInvoiceDate.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceDate.TabIndex = 4;
this.lblInvoiceDate.Text = "开票日期";
//111======100
this.dtpInvoiceDate.Location = new System.Drawing.Point(173,96);
this.dtpInvoiceDate.Name ="dtpInvoiceDate";
this.dtpInvoiceDate.Size = new System.Drawing.Size(100, 21);
this.dtpInvoiceDate.TabIndex = 4;
this.Controls.Add(this.lblInvoiceDate);
this.Controls.Add(this.dtpInvoiceDate);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,125);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 5;
this.lblTaxAmount.Text = "税额";
//111======125
this.txtTaxAmount.Location = new System.Drawing.Point(173,121);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 5;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####TotalWithTax###Decimal
this.lblTotalWithTax.AutoSize = true;
this.lblTotalWithTax.Location = new System.Drawing.Point(100,150);
this.lblTotalWithTax.Name = "lblTotalWithTax";
this.lblTotalWithTax.Size = new System.Drawing.Size(41, 12);
this.lblTotalWithTax.TabIndex = 6;
this.lblTotalWithTax.Text = "价税合计";
//111======150
this.txtTotalWithTax.Location = new System.Drawing.Point(173,146);
this.txtTotalWithTax.Name ="txtTotalWithTax";
this.txtTotalWithTax.Size = new System.Drawing.Size(100, 21);
this.txtTotalWithTax.TabIndex = 6;
this.Controls.Add(this.lblTotalWithTax);
this.Controls.Add(this.txtTotalWithTax);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,175);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 7;
this.lblTotalAmount.Text = "发票总金额（不含税）";
//111======175
this.txtTotalAmount.Location = new System.Drawing.Point(173,171);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 7;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####IsRed###Boolean
this.lblIsRed.AutoSize = true;
this.lblIsRed.Location = new System.Drawing.Point(100,200);
this.lblIsRed.Name = "lblIsRed";
this.lblIsRed.Size = new System.Drawing.Size(41, 12);
this.lblIsRed.TabIndex = 8;
this.lblIsRed.Text = "红字发票";
this.chkIsRed.Location = new System.Drawing.Point(173,196);
this.chkIsRed.Name = "chkIsRed";
this.chkIsRed.Size = new System.Drawing.Size(100, 21);
this.chkIsRed.TabIndex = 8;
this.Controls.Add(this.lblIsRed);
this.Controls.Add(this.chkIsRed);

           //#####300Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,225);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 9;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,221);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 9;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####ReceivePaymentType###Int32
//属性测试250ReceivePaymentType
//属性测试250ReceivePaymentType

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,275);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 11;
this.lblCreated_at.Text = "创建时间";
//111======275
this.dtpCreated_at.Location = new System.Drawing.Point(173,271);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 11;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试300Created_by
//属性测试300Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,325);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 13;
this.lblModified_at.Text = "修改时间";
//111======325
this.dtpModified_at.Location = new System.Drawing.Point(173,321);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 13;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试350Modified_by
//属性测试350Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,375);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 15;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,371);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 15;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,400);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 16;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,396);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 16;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试425Approver_by
//属性测试425Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,450);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 18;
this.lblApprover_at.Text = "审批时间";
//111======450
this.dtpApprover_at.Location = new System.Drawing.Point(173,446);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 18;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,500);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 20;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,496);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 20;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####DataStatus###Int32
//属性测试525DataStatus
//属性测试525DataStatus

           //#####PrintStatus###Int32
//属性测试550PrintStatus
//属性测试550PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblInvoiceNo );
this.Controls.Add(this.txtInvoiceNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblBillingInfo_ID );
this.Controls.Add(this.cmbBillingInfo_ID );

                this.Controls.Add(this.lblInvoiceDate );
this.Controls.Add(this.dtpInvoiceDate );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblTotalWithTax );
this.Controls.Add(this.txtTotalWithTax );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblIsRed );
this.Controls.Add(this.chkIsRed );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                
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

                
                
                    
            this.Name = "tb_FM_InvoiceQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiceNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInvoiceNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBillingInfo_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBillingInfo_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiceDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpInvoiceDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalWithTax;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalWithTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsRed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsRed;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              
    
        
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


