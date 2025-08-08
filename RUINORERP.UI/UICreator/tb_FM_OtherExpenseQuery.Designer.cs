
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
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单
    /// </summary>
    partial class tb_FM_OtherExpenseQuery
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
     
     this.lblExpenseNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExpenseNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDocumentDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDocumentDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblEXPOrINC = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkEXPOrINC = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkEXPOrINC.Values.Text ="";
this.chkEXPOrINC.Checked = true;
this.chkEXPOrINC.CheckState = System.Windows.Forms.CheckState.Checked;

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


this.lblApprovedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCloseCaseImagePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseImagePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCloseCaseImagePath.Multiline = true;

this.lblCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30ExpenseNo###String
this.lblExpenseNo.AutoSize = true;
this.lblExpenseNo.Location = new System.Drawing.Point(100,25);
this.lblExpenseNo.Name = "lblExpenseNo";
this.lblExpenseNo.Size = new System.Drawing.Size(41, 12);
this.lblExpenseNo.TabIndex = 1;
this.lblExpenseNo.Text = "单据编号";
this.txtExpenseNo.Location = new System.Drawing.Point(173,21);
this.txtExpenseNo.Name = "txtExpenseNo";
this.txtExpenseNo.Size = new System.Drawing.Size(100, 21);
this.txtExpenseNo.TabIndex = 1;
this.Controls.Add(this.lblExpenseNo);
this.Controls.Add(this.txtExpenseNo);

           //#####Employee_ID###Int64
//属性测试50Employee_ID
//属性测试50Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "制单人";
//111======50
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,46);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 2;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####DocumentDate###DateTime
this.lblDocumentDate.AutoSize = true;
this.lblDocumentDate.Location = new System.Drawing.Point(100,75);
this.lblDocumentDate.Name = "lblDocumentDate";
this.lblDocumentDate.Size = new System.Drawing.Size(41, 12);
this.lblDocumentDate.TabIndex = 3;
this.lblDocumentDate.Text = "单据日期";
//111======75
this.dtpDocumentDate.Location = new System.Drawing.Point(173,71);
this.dtpDocumentDate.Name ="dtpDocumentDate";
this.dtpDocumentDate.Size = new System.Drawing.Size(100, 21);
this.dtpDocumentDate.TabIndex = 3;
this.Controls.Add(this.lblDocumentDate);
this.Controls.Add(this.dtpDocumentDate);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,100);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 4;
this.lblTotalAmount.Text = "总金额";
//111======100
this.txtTotalAmount.Location = new System.Drawing.Point(173,96);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 4;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####PayStatus###Int32
//属性测试125PayStatus
//属性测试125PayStatus

           //#####Paytype_ID###Int64
//属性测试150Paytype_ID
//属性测试150Paytype_ID

           //#####EXPOrINC###Boolean
this.lblEXPOrINC.AutoSize = true;
this.lblEXPOrINC.Location = new System.Drawing.Point(100,175);
this.lblEXPOrINC.Name = "lblEXPOrINC";
this.lblEXPOrINC.Size = new System.Drawing.Size(41, 12);
this.lblEXPOrINC.TabIndex = 7;
this.lblEXPOrINC.Text = "为收入";
this.chkEXPOrINC.Location = new System.Drawing.Point(173,171);
this.chkEXPOrINC.Name = "chkEXPOrINC";
this.chkEXPOrINC.Size = new System.Drawing.Size(100, 21);
this.chkEXPOrINC.TabIndex = 7;
this.Controls.Add(this.lblEXPOrINC);
this.Controls.Add(this.chkEXPOrINC);

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

           //#####ApprovedAmount###Decimal
this.lblApprovedAmount.AutoSize = true;
this.lblApprovedAmount.Location = new System.Drawing.Point(100,625);
this.lblApprovedAmount.Name = "lblApprovedAmount";
this.lblApprovedAmount.Size = new System.Drawing.Size(41, 12);
this.lblApprovedAmount.TabIndex = 25;
this.lblApprovedAmount.Text = "核准金额";
//111======625
this.txtApprovedAmount.Location = new System.Drawing.Point(173,621);
this.txtApprovedAmount.Name ="txtApprovedAmount";
this.txtApprovedAmount.Size = new System.Drawing.Size(100, 21);
this.txtApprovedAmount.TabIndex = 25;
this.Controls.Add(this.lblApprovedAmount);
this.Controls.Add(this.txtApprovedAmount);

           //#####Currency_ID###Int64
//属性测试650Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,650);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 26;
this.lblCurrency_ID.Text = "币种";
//111======650
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,646);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 26;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####300CloseCaseImagePath###String
this.lblCloseCaseImagePath.AutoSize = true;
this.lblCloseCaseImagePath.Location = new System.Drawing.Point(100,675);
this.lblCloseCaseImagePath.Name = "lblCloseCaseImagePath";
this.lblCloseCaseImagePath.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseImagePath.TabIndex = 27;
this.lblCloseCaseImagePath.Text = "结案凭证";
this.txtCloseCaseImagePath.Location = new System.Drawing.Point(173,671);
this.txtCloseCaseImagePath.Name = "txtCloseCaseImagePath";
this.txtCloseCaseImagePath.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseImagePath.TabIndex = 27;
this.Controls.Add(this.lblCloseCaseImagePath);
this.Controls.Add(this.txtCloseCaseImagePath);

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,700);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 28;
this.lblCloseCaseOpinions.Text = "结案意见";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,696);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 28;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblExpenseNo );
this.Controls.Add(this.txtExpenseNo );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblDocumentDate );
this.Controls.Add(this.dtpDocumentDate );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                
                
                this.Controls.Add(this.lblEXPOrINC );
this.Controls.Add(this.chkEXPOrINC );

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

                
                this.Controls.Add(this.lblApprovedAmount );
this.Controls.Add(this.txtApprovedAmount );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblCloseCaseImagePath );
this.Controls.Add(this.txtCloseCaseImagePath );

                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                    
            this.Name = "tb_FM_OtherExpenseQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpenseNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExpenseNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDocumentDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDocumentDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEXPOrINC;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkEXPOrINC;

    
        
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

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovedAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseImagePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseImagePath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
    
   
 





    }
}


