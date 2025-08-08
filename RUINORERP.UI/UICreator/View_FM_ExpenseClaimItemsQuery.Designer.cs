
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
    /// 费用报销统计分析
    /// </summary>
    partial class View_FM_ExpenseClaimItemsQuery
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

this.lblDocumentDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDocumentDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();





this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblClaimName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtClaimName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtClaimName.Multiline = true;






this.lblTranDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpTranDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblSingleAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSingleAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEvidenceImagePath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEvidenceImagePath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtEvidenceImagePath.Multiline = true;

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30ClaimNo###String
this.lblClaimNo.AutoSize = true;
this.lblClaimNo.Location = new System.Drawing.Point(100,25);
this.lblClaimNo.Name = "lblClaimNo";
this.lblClaimNo.Size = new System.Drawing.Size(41, 12);
this.lblClaimNo.TabIndex = 1;
this.lblClaimNo.Text = "";
this.txtClaimNo.Location = new System.Drawing.Point(173,21);
this.txtClaimNo.Name = "txtClaimNo";
this.txtClaimNo.Size = new System.Drawing.Size(100, 21);
this.txtClaimNo.TabIndex = 1;
this.Controls.Add(this.lblClaimNo);
this.Controls.Add(this.txtClaimNo);

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

           //#####Employee_ID###Int64

           //#####PayeeInfoID###Int64

           //#####Created_by###Int64

           //#####DataStatus###Int32

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,175);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 7;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,171);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 7;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,225);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 9;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,221);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 9;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####Approver_by###Int64

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,275);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 11;
this.lblApprover_at.Text = "";
//111======275
this.dtpApprover_at.Location = new System.Drawing.Point(173,271);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 11;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####1000Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,300);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 12;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,296);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 12;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####300ClaimName###String
this.lblClaimName.AutoSize = true;
this.lblClaimName.Location = new System.Drawing.Point(100,325);
this.lblClaimName.Name = "lblClaimName";
this.lblClaimName.Size = new System.Drawing.Size(41, 12);
this.lblClaimName.TabIndex = 13;
this.lblClaimName.Text = "";
this.txtClaimName.Location = new System.Drawing.Point(173,321);
this.txtClaimName.Name = "txtClaimName";
this.txtClaimName.Size = new System.Drawing.Size(100, 21);
this.txtClaimName.TabIndex = 13;
this.Controls.Add(this.lblClaimName);
this.Controls.Add(this.txtClaimName);

           //#####DepartmentID###Int64

           //#####ExpenseType_id###Int64

           //#####Account_id###Int64

           //#####Subject_id###Int64

           //#####ProjectGroup_ID###Int64

           //#####TranDate###DateTime
this.lblTranDate.AutoSize = true;
this.lblTranDate.Location = new System.Drawing.Point(100,475);
this.lblTranDate.Name = "lblTranDate";
this.lblTranDate.Size = new System.Drawing.Size(41, 12);
this.lblTranDate.TabIndex = 19;
this.lblTranDate.Text = "";
//111======475
this.dtpTranDate.Location = new System.Drawing.Point(173,471);
this.dtpTranDate.Name ="dtpTranDate";
this.dtpTranDate.Size = new System.Drawing.Size(100, 21);
this.dtpTranDate.TabIndex = 19;
this.Controls.Add(this.lblTranDate);
this.Controls.Add(this.dtpTranDate);

           //#####SingleAmount###Decimal
this.lblSingleAmount.AutoSize = true;
this.lblSingleAmount.Location = new System.Drawing.Point(100,500);
this.lblSingleAmount.Name = "lblSingleAmount";
this.lblSingleAmount.Size = new System.Drawing.Size(41, 12);
this.lblSingleAmount.TabIndex = 20;
this.lblSingleAmount.Text = "";
//111======500
this.txtSingleAmount.Location = new System.Drawing.Point(173,496);
this.txtSingleAmount.Name ="txtSingleAmount";
this.txtSingleAmount.Size = new System.Drawing.Size(100, 21);
this.txtSingleAmount.TabIndex = 20;
this.Controls.Add(this.lblSingleAmount);
this.Controls.Add(this.txtSingleAmount);

           //#####300EvidenceImagePath###String
this.lblEvidenceImagePath.AutoSize = true;
this.lblEvidenceImagePath.Location = new System.Drawing.Point(100,525);
this.lblEvidenceImagePath.Name = "lblEvidenceImagePath";
this.lblEvidenceImagePath.Size = new System.Drawing.Size(41, 12);
this.lblEvidenceImagePath.TabIndex = 21;
this.lblEvidenceImagePath.Text = "";
this.txtEvidenceImagePath.Location = new System.Drawing.Point(173,521);
this.txtEvidenceImagePath.Name = "txtEvidenceImagePath";
this.txtEvidenceImagePath.Size = new System.Drawing.Size(100, 21);
this.txtEvidenceImagePath.TabIndex = 21;
this.Controls.Add(this.lblEvidenceImagePath);
this.Controls.Add(this.txtEvidenceImagePath);

           //#####500Summary###String
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblClaimNo );
this.Controls.Add(this.txtClaimNo );

                this.Controls.Add(this.lblDocumentDate );
this.Controls.Add(this.dtpDocumentDate );

                
                
                
                
                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblClaimName );
this.Controls.Add(this.txtClaimName );

                
                
                
                
                
                this.Controls.Add(this.lblTranDate );
this.Controls.Add(this.dtpTranDate );

                this.Controls.Add(this.lblSingleAmount );
this.Controls.Add(this.txtSingleAmount );

                this.Controls.Add(this.lblEvidenceImagePath );
this.Controls.Add(this.txtEvidenceImagePath );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "View_FM_ExpenseClaimItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClaimNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClaimNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDocumentDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDocumentDate;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClaimName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClaimName;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTranDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpTranDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSingleAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSingleAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEvidenceImagePath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEvidenceImagePath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


