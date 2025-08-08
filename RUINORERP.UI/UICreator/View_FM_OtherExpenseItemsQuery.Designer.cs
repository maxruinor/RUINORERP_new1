
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:28
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 其他费用统计分析
    /// </summary>
    partial class View_FM_OtherExpenseItemsQuery
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

this.lblDocumentDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDocumentDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEXPOrINC = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkEXPOrINC = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkEXPOrINC.Values.Text ="";

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";



this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


this.lblExpenseName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExpenseName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtExpenseName.Multiline = true;






this.lblSingleTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSingleTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCheckOutDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCheckOutDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIncludeTax.Values.Text ="";

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30ExpenseNo###String
this.lblExpenseNo.AutoSize = true;
this.lblExpenseNo.Location = new System.Drawing.Point(100,25);
this.lblExpenseNo.Name = "lblExpenseNo";
this.lblExpenseNo.Size = new System.Drawing.Size(41, 12);
this.lblExpenseNo.TabIndex = 1;
this.lblExpenseNo.Text = "";
this.txtExpenseNo.Location = new System.Drawing.Point(173,21);
this.txtExpenseNo.Name = "txtExpenseNo";
this.txtExpenseNo.Size = new System.Drawing.Size(100, 21);
this.txtExpenseNo.TabIndex = 1;
this.Controls.Add(this.lblExpenseNo);
this.Controls.Add(this.txtExpenseNo);

           //#####DocumentDate###DateTime
this.lblDocumentDate.AutoSize = true;
this.lblDocumentDate.Location = new System.Drawing.Point(100,50);
this.lblDocumentDate.Name = "lblDocumentDate";
this.lblDocumentDate.Size = new System.Drawing.Size(41, 12);
this.lblDocumentDate.TabIndex = 2;
this.lblDocumentDate.Text = "";
//111======50
this.dtpDocumentDate.Location = new System.Drawing.Point(173,46);
this.dtpDocumentDate.Name ="dtpDocumentDate";
this.dtpDocumentDate.ShowCheckBox =true;
this.dtpDocumentDate.Size = new System.Drawing.Size(100, 21);
this.dtpDocumentDate.TabIndex = 2;
this.Controls.Add(this.lblDocumentDate);
this.Controls.Add(this.dtpDocumentDate);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,75);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 3;
this.lblTotalAmount.Text = "";
//111======75
this.txtTotalAmount.Location = new System.Drawing.Point(173,71);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 3;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####EXPOrINC###Boolean
this.lblEXPOrINC.AutoSize = true;
this.lblEXPOrINC.Location = new System.Drawing.Point(100,100);
this.lblEXPOrINC.Name = "lblEXPOrINC";
this.lblEXPOrINC.Size = new System.Drawing.Size(41, 12);
this.lblEXPOrINC.TabIndex = 4;
this.lblEXPOrINC.Text = "";
this.chkEXPOrINC.Location = new System.Drawing.Point(173,96);
this.chkEXPOrINC.Name = "chkEXPOrINC";
this.chkEXPOrINC.Size = new System.Drawing.Size(100, 21);
this.chkEXPOrINC.TabIndex = 4;
this.Controls.Add(this.lblEXPOrINC);
this.Controls.Add(this.chkEXPOrINC);

           //#####1000Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,125);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 5;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,121);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 5;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,175);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 7;
this.lblisdeleted.Text = "";
this.chkisdeleted.Location = new System.Drawing.Point(173,171);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 7;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,250);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 10;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,246);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 10;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####Currency_ID###Int64

           //#####300ExpenseName###String
this.lblExpenseName.AutoSize = true;
this.lblExpenseName.Location = new System.Drawing.Point(100,300);
this.lblExpenseName.Name = "lblExpenseName";
this.lblExpenseName.Size = new System.Drawing.Size(41, 12);
this.lblExpenseName.TabIndex = 12;
this.lblExpenseName.Text = "";
this.txtExpenseName.Location = new System.Drawing.Point(173,296);
this.txtExpenseName.Name = "txtExpenseName";
this.txtExpenseName.Size = new System.Drawing.Size(100, 21);
this.txtExpenseName.TabIndex = 12;
this.Controls.Add(this.lblExpenseName);
this.Controls.Add(this.txtExpenseName);

           //#####Employee_ID###Int64

           //#####DepartmentID###Int64

           //#####ExpenseType_id###Int64

           //#####Account_id###Int64

           //#####CustomerVendor_ID###Int64

           //#####SingleTotalAmount###Decimal
this.lblSingleTotalAmount.AutoSize = true;
this.lblSingleTotalAmount.Location = new System.Drawing.Point(100,450);
this.lblSingleTotalAmount.Name = "lblSingleTotalAmount";
this.lblSingleTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSingleTotalAmount.TabIndex = 18;
this.lblSingleTotalAmount.Text = "";
//111======450
this.txtSingleTotalAmount.Location = new System.Drawing.Point(173,446);
this.txtSingleTotalAmount.Name ="txtSingleTotalAmount";
this.txtSingleTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSingleTotalAmount.TabIndex = 18;
this.Controls.Add(this.lblSingleTotalAmount);
this.Controls.Add(this.txtSingleTotalAmount);

           //#####Subject_id###Int64

           //#####CheckOutDate###DateTime
this.lblCheckOutDate.AutoSize = true;
this.lblCheckOutDate.Location = new System.Drawing.Point(100,500);
this.lblCheckOutDate.Name = "lblCheckOutDate";
this.lblCheckOutDate.Size = new System.Drawing.Size(41, 12);
this.lblCheckOutDate.TabIndex = 20;
this.lblCheckOutDate.Text = "";
//111======500
this.dtpCheckOutDate.Location = new System.Drawing.Point(173,496);
this.dtpCheckOutDate.Name ="dtpCheckOutDate";
this.dtpCheckOutDate.Size = new System.Drawing.Size(100, 21);
this.dtpCheckOutDate.TabIndex = 20;
this.Controls.Add(this.lblCheckOutDate);
this.Controls.Add(this.dtpCheckOutDate);

           //#####IncludeTax###Boolean
this.lblIncludeTax.AutoSize = true;
this.lblIncludeTax.Location = new System.Drawing.Point(100,525);
this.lblIncludeTax.Name = "lblIncludeTax";
this.lblIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIncludeTax.TabIndex = 21;
this.lblIncludeTax.Text = "";
this.chkIncludeTax.Location = new System.Drawing.Point(173,521);
this.chkIncludeTax.Name = "chkIncludeTax";
this.chkIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIncludeTax.TabIndex = 21;
this.Controls.Add(this.lblIncludeTax);
this.Controls.Add(this.chkIncludeTax);

           //#####100Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,550);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 22;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,546);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 22;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,575);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 23;
this.lblTaxAmount.Text = "";
//111======575
this.txtTaxAmount.Location = new System.Drawing.Point(173,571);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 23;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####UntaxedAmount###Decimal
this.lblUntaxedAmount.AutoSize = true;
this.lblUntaxedAmount.Location = new System.Drawing.Point(100,600);
this.lblUntaxedAmount.Name = "lblUntaxedAmount";
this.lblUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedAmount.TabIndex = 24;
this.lblUntaxedAmount.Text = "";
//111======600
this.txtUntaxedAmount.Location = new System.Drawing.Point(173,596);
this.txtUntaxedAmount.Name ="txtUntaxedAmount";
this.txtUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedAmount.TabIndex = 24;
this.Controls.Add(this.lblUntaxedAmount);
this.Controls.Add(this.txtUntaxedAmount);

           //#####ProjectGroup_ID###Int64

           //#####Created_by###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblExpenseNo );
this.Controls.Add(this.txtExpenseNo );

                this.Controls.Add(this.lblDocumentDate );
this.Controls.Add(this.dtpDocumentDate );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblEXPOrINC );
this.Controls.Add(this.chkEXPOrINC );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                this.Controls.Add(this.lblExpenseName );
this.Controls.Add(this.txtExpenseName );

                
                
                
                
                
                this.Controls.Add(this.lblSingleTotalAmount );
this.Controls.Add(this.txtSingleTotalAmount );

                
                this.Controls.Add(this.lblCheckOutDate );
this.Controls.Add(this.dtpCheckOutDate );

                this.Controls.Add(this.lblIncludeTax );
this.Controls.Add(this.chkIncludeTax );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblUntaxedAmount );
this.Controls.Add(this.txtUntaxedAmount );

                
                
                    
            this.Name = "View_FM_OtherExpenseItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpenseNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExpenseNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDocumentDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDocumentDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEXPOrINC;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkEXPOrINC;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpenseName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExpenseName;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSingleTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSingleTotalAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCheckOutDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCheckOutDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedAmount;

    
        
              
    
        
              
    
    
   
 





    }
}


