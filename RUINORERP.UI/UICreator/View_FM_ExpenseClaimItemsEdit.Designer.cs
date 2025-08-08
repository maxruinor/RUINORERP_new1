// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:27
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
    partial class View_FM_ExpenseClaimItemsEdit
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
     this.lblClaimNo = new Krypton.Toolkit.KryptonLabel();
this.txtClaimNo = new Krypton.Toolkit.KryptonTextBox();

this.lblDocumentDate = new Krypton.Toolkit.KryptonLabel();
this.dtpDocumentDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPayeeInfoID = new Krypton.Toolkit.KryptonLabel();
this.txtPayeeInfoID = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblClaimName = new Krypton.Toolkit.KryptonLabel();
this.txtClaimName = new Krypton.Toolkit.KryptonTextBox();
this.txtClaimName.Multiline = true;

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.txtDepartmentID = new Krypton.Toolkit.KryptonTextBox();

this.lblExpenseType_id = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseType_id = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_id = new Krypton.Toolkit.KryptonTextBox();

this.lblSubject_id = new Krypton.Toolkit.KryptonLabel();
this.txtSubject_id = new Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.txtProjectGroup_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblTranDate = new Krypton.Toolkit.KryptonLabel();
this.dtpTranDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblSingleAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSingleAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblEvidenceImagePath = new Krypton.Toolkit.KryptonLabel();
this.txtEvidenceImagePath = new Krypton.Toolkit.KryptonTextBox();
this.txtEvidenceImagePath.Multiline = true;

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    
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
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,75);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 3;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,71);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 3;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####PayeeInfoID###Int64
this.lblPayeeInfoID.AutoSize = true;
this.lblPayeeInfoID.Location = new System.Drawing.Point(100,100);
this.lblPayeeInfoID.Name = "lblPayeeInfoID";
this.lblPayeeInfoID.Size = new System.Drawing.Size(41, 12);
this.lblPayeeInfoID.TabIndex = 4;
this.lblPayeeInfoID.Text = "";
this.txtPayeeInfoID.Location = new System.Drawing.Point(173,96);
this.txtPayeeInfoID.Name = "txtPayeeInfoID";
this.txtPayeeInfoID.Size = new System.Drawing.Size(100, 21);
this.txtPayeeInfoID.TabIndex = 4;
this.Controls.Add(this.lblPayeeInfoID);
this.Controls.Add(this.txtPayeeInfoID);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,125);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 5;
this.lblCreated_by.Text = "";
this.txtCreated_by.Location = new System.Drawing.Point(173,121);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 5;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####DataStatus###Int32
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,150);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 6;
this.lblDataStatus.Text = "";
this.txtDataStatus.Location = new System.Drawing.Point(173,146);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 6;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

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
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,250);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 10;
this.lblApprover_by.Text = "";
this.txtApprover_by.Location = new System.Drawing.Point(173,246);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 10;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

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
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,350);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 14;
this.lblDepartmentID.Text = "";
this.txtDepartmentID.Location = new System.Drawing.Point(173,346);
this.txtDepartmentID.Name = "txtDepartmentID";
this.txtDepartmentID.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentID.TabIndex = 14;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.txtDepartmentID);

           //#####ExpenseType_id###Int64
this.lblExpenseType_id.AutoSize = true;
this.lblExpenseType_id.Location = new System.Drawing.Point(100,375);
this.lblExpenseType_id.Name = "lblExpenseType_id";
this.lblExpenseType_id.Size = new System.Drawing.Size(41, 12);
this.lblExpenseType_id.TabIndex = 15;
this.lblExpenseType_id.Text = "";
this.txtExpenseType_id.Location = new System.Drawing.Point(173,371);
this.txtExpenseType_id.Name = "txtExpenseType_id";
this.txtExpenseType_id.Size = new System.Drawing.Size(100, 21);
this.txtExpenseType_id.TabIndex = 15;
this.Controls.Add(this.lblExpenseType_id);
this.Controls.Add(this.txtExpenseType_id);

           //#####Account_id###Int64
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,400);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 16;
this.lblAccount_id.Text = "";
this.txtAccount_id.Location = new System.Drawing.Point(173,396);
this.txtAccount_id.Name = "txtAccount_id";
this.txtAccount_id.Size = new System.Drawing.Size(100, 21);
this.txtAccount_id.TabIndex = 16;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.txtAccount_id);

           //#####Subject_id###Int64
this.lblSubject_id.AutoSize = true;
this.lblSubject_id.Location = new System.Drawing.Point(100,425);
this.lblSubject_id.Name = "lblSubject_id";
this.lblSubject_id.Size = new System.Drawing.Size(41, 12);
this.lblSubject_id.TabIndex = 17;
this.lblSubject_id.Text = "";
this.txtSubject_id.Location = new System.Drawing.Point(173,421);
this.txtSubject_id.Name = "txtSubject_id";
this.txtSubject_id.Size = new System.Drawing.Size(100, 21);
this.txtSubject_id.TabIndex = 17;
this.Controls.Add(this.lblSubject_id);
this.Controls.Add(this.txtSubject_id);

           //#####ProjectGroup_ID###Int64
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,450);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 18;
this.lblProjectGroup_ID.Text = "";
this.txtProjectGroup_ID.Location = new System.Drawing.Point(173,446);
this.txtProjectGroup_ID.Name = "txtProjectGroup_ID";
this.txtProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.txtProjectGroup_ID.TabIndex = 18;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.txtProjectGroup_ID);

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
           // this.kryptonPanel1.TabIndex = 22;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblClaimNo );
this.Controls.Add(this.txtClaimNo );

                this.Controls.Add(this.lblDocumentDate );
this.Controls.Add(this.dtpDocumentDate );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblPayeeInfoID );
this.Controls.Add(this.txtPayeeInfoID );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblClaimName );
this.Controls.Add(this.txtClaimName );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.txtDepartmentID );

                this.Controls.Add(this.lblExpenseType_id );
this.Controls.Add(this.txtExpenseType_id );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.txtAccount_id );

                this.Controls.Add(this.lblSubject_id );
this.Controls.Add(this.txtSubject_id );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.txtProjectGroup_ID );

                this.Controls.Add(this.lblTranDate );
this.Controls.Add(this.dtpTranDate );

                this.Controls.Add(this.lblSingleAmount );
this.Controls.Add(this.txtSingleAmount );

                this.Controls.Add(this.lblEvidenceImagePath );
this.Controls.Add(this.txtEvidenceImagePath );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "View_FM_ExpenseClaimItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_FM_ExpenseClaimItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblClaimNo;
private Krypton.Toolkit.KryptonTextBox txtClaimNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblDocumentDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpDocumentDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private Krypton.Toolkit.KryptonTextBox txtPayeeInfoID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblClaimName;
private Krypton.Toolkit.KryptonTextBox txtClaimName;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonTextBox txtDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseType_id;
private Krypton.Toolkit.KryptonTextBox txtExpenseType_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonTextBox txtAccount_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubject_id;
private Krypton.Toolkit.KryptonTextBox txtSubject_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonTextBox txtProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTranDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpTranDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSingleAmount;
private Krypton.Toolkit.KryptonTextBox txtSingleAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblEvidenceImagePath;
private Krypton.Toolkit.KryptonTextBox txtEvidenceImagePath;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

