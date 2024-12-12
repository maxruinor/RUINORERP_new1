
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/12/2024 11:32:08
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 费用报销单
    /// </summary>
    partial class tb_FM_ExpenseClaimQuery
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
     
     this.lblClaimNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtClaimNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDocumentDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDocumentDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblClaimAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtClaimAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblApprovedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIncludeTax.Values.Text ="";

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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


this.lblCloseCaseImagePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseImagePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCloseCaseImagePath.Multiline = true;

this.lblCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30ClaimNo###String
this.lblClaimNo.AutoSize = true;
this.lblClaimNo.Location = new System.Drawing.Point(100,25);
this.lblClaimNo.Name = "lblClaimNo";
this.lblClaimNo.Size = new System.Drawing.Size(41, 12);
this.lblClaimNo.TabIndex = 1;
this.lblClaimNo.Text = "单据编号";
this.txtClaimNo.Location = new System.Drawing.Point(173,21);
this.txtClaimNo.Name = "txtClaimNo";
this.txtClaimNo.Size = new System.Drawing.Size(100, 21);
this.txtClaimNo.TabIndex = 1;
this.Controls.Add(this.lblClaimNo);
this.Controls.Add(this.txtClaimNo);

           //#####Currency_ID###Int64
//属性测试50Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,50);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 2;
this.lblCurrency_ID.Text = "币别";
//111======50
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,46);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 2;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####Employee_ID###Int64
//属性测试75Employee_ID
//属性测试75Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,75);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 3;
this.lblEmployee_ID.Text = "报销人";
//111======75
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,71);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 3;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

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

           //#####DocumentDate###DateTime
this.lblDocumentDate.AutoSize = true;
this.lblDocumentDate.Location = new System.Drawing.Point(100,125);
this.lblDocumentDate.Name = "lblDocumentDate";
this.lblDocumentDate.Size = new System.Drawing.Size(41, 12);
this.lblDocumentDate.TabIndex = 5;
this.lblDocumentDate.Text = "报销日期";
//111======125
this.dtpDocumentDate.Location = new System.Drawing.Point(173,121);
this.dtpDocumentDate.Name ="dtpDocumentDate";
this.dtpDocumentDate.ShowCheckBox =true;
this.dtpDocumentDate.Size = new System.Drawing.Size(100, 21);
this.dtpDocumentDate.TabIndex = 5;
this.Controls.Add(this.lblDocumentDate);
this.Controls.Add(this.dtpDocumentDate);

           //#####ClaimAmount###Decimal
this.lblClaimAmount.AutoSize = true;
this.lblClaimAmount.Location = new System.Drawing.Point(100,150);
this.lblClaimAmount.Name = "lblClaimAmount";
this.lblClaimAmount.Size = new System.Drawing.Size(41, 12);
this.lblClaimAmount.TabIndex = 6;
this.lblClaimAmount.Text = "报销金额";
//111======150
this.txtClaimAmount.Location = new System.Drawing.Point(173,146);
this.txtClaimAmount.Name ="txtClaimAmount";
this.txtClaimAmount.Size = new System.Drawing.Size(100, 21);
this.txtClaimAmount.TabIndex = 6;
this.Controls.Add(this.lblClaimAmount);
this.Controls.Add(this.txtClaimAmount);

           //#####ApprovedAmount###Decimal
this.lblApprovedAmount.AutoSize = true;
this.lblApprovedAmount.Location = new System.Drawing.Point(100,175);
this.lblApprovedAmount.Name = "lblApprovedAmount";
this.lblApprovedAmount.Size = new System.Drawing.Size(41, 12);
this.lblApprovedAmount.TabIndex = 7;
this.lblApprovedAmount.Text = "核准金额";
//111======175
this.txtApprovedAmount.Location = new System.Drawing.Point(173,171);
this.txtApprovedAmount.Name ="txtApprovedAmount";
this.txtApprovedAmount.Size = new System.Drawing.Size(100, 21);
this.txtApprovedAmount.TabIndex = 7;
this.Controls.Add(this.lblApprovedAmount);
this.Controls.Add(this.txtApprovedAmount);

           //#####IncludeTax###Boolean
this.lblIncludeTax.AutoSize = true;
this.lblIncludeTax.Location = new System.Drawing.Point(100,200);
this.lblIncludeTax.Name = "lblIncludeTax";
this.lblIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIncludeTax.TabIndex = 8;
this.lblIncludeTax.Text = "含税";
this.chkIncludeTax.Location = new System.Drawing.Point(173,196);
this.chkIncludeTax.Name = "chkIncludeTax";
this.chkIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIncludeTax.TabIndex = 8;
this.Controls.Add(this.lblIncludeTax);
this.Controls.Add(this.chkIncludeTax);

           //#####1000Notes###String
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

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,250);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 10;
this.lblTaxAmount.Text = "税额";
//111======250
this.txtTaxAmount.Location = new System.Drawing.Point(173,246);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 10;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,275);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 11;
this.lblTaxRate.Text = "税率";
//111======275
this.txtTaxRate.Location = new System.Drawing.Point(173,271);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 11;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####UntaxedAmount###Decimal
this.lblUntaxedAmount.AutoSize = true;
this.lblUntaxedAmount.Location = new System.Drawing.Point(100,300);
this.lblUntaxedAmount.Name = "lblUntaxedAmount";
this.lblUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedAmount.TabIndex = 12;
this.lblUntaxedAmount.Text = "未税本位币";
//111======300
this.txtUntaxedAmount.Location = new System.Drawing.Point(173,296);
this.txtUntaxedAmount.Name ="txtUntaxedAmount";
this.txtUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedAmount.TabIndex = 12;
this.Controls.Add(this.lblUntaxedAmount);
this.Controls.Add(this.txtUntaxedAmount);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,325);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 13;
this.lblCreated_at.Text = "创建时间";
//111======325
this.dtpCreated_at.Location = new System.Drawing.Point(173,321);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 13;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试350Created_by
//属性测试350Created_by
//属性测试350Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,375);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 15;
this.lblModified_at.Text = "修改时间";
//111======375
this.dtpModified_at.Location = new System.Drawing.Point(173,371);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 15;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试400Modified_by
//属性测试400Modified_by
//属性测试400Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,425);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 17;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,421);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 17;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试450DataStatus
//属性测试450DataStatus
//属性测试450DataStatus

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,475);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 19;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,471);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 19;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试500Approver_by
//属性测试500Approver_by
//属性测试500Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,525);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 21;
this.lblApprover_at.Text = "审批时间";
//111======525
this.dtpApprover_at.Location = new System.Drawing.Point(173,521);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 21;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,575);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 23;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,571);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 23;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试600PrintStatus
//属性测试600PrintStatus
//属性测试600PrintStatus

           //#####300CloseCaseImagePath###String
this.lblCloseCaseImagePath.AutoSize = true;
this.lblCloseCaseImagePath.Location = new System.Drawing.Point(100,625);
this.lblCloseCaseImagePath.Name = "lblCloseCaseImagePath";
this.lblCloseCaseImagePath.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseImagePath.TabIndex = 25;
this.lblCloseCaseImagePath.Text = "结案凭证";
this.txtCloseCaseImagePath.Location = new System.Drawing.Point(173,621);
this.txtCloseCaseImagePath.Name = "txtCloseCaseImagePath";
this.txtCloseCaseImagePath.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseImagePath.TabIndex = 25;
this.Controls.Add(this.lblCloseCaseImagePath);
this.Controls.Add(this.txtCloseCaseImagePath);

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,650);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 26;
this.lblCloseCaseOpinions.Text = "结案意见";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,646);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 26;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblClaimNo );
this.Controls.Add(this.txtClaimNo );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblPayeeInfoID );
this.Controls.Add(this.cmbPayeeInfoID );

                this.Controls.Add(this.lblDocumentDate );
this.Controls.Add(this.dtpDocumentDate );

                this.Controls.Add(this.lblClaimAmount );
this.Controls.Add(this.txtClaimAmount );

                this.Controls.Add(this.lblApprovedAmount );
this.Controls.Add(this.txtApprovedAmount );

                this.Controls.Add(this.lblIncludeTax );
this.Controls.Add(this.chkIncludeTax );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblUntaxedAmount );
this.Controls.Add(this.txtUntaxedAmount );

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

                
                this.Controls.Add(this.lblCloseCaseImagePath );
this.Controls.Add(this.txtCloseCaseImagePath );

                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                    
            this.Name = "tb_FM_ExpenseClaimQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClaimNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClaimNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDocumentDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDocumentDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClaimAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClaimAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovedAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedAmount;

    
        
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

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseImagePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseImagePath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
    
   
 





    }
}


