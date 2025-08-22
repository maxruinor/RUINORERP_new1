
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:40
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 盘点表
    /// </summary>
    partial class tb_StocktakeQuery
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
     
     this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCheckNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCheckNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();





this.lblCarryingTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCarryingTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCheck_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCheck_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCarryingDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCarryingDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;


this.lblDiffTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiffTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCheckTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCheckTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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
                 //#####Employee_ID###Int64
//属性测试25Employee_ID
//属性测试25Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,25);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 1;
this.lblEmployee_ID.Text = "盘点负责人";
//111======25
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,21);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 1;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####Location_ID###Int64
//属性测试50Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,50);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 2;
this.lblLocation_ID.Text = "盘点仓库";
//111======50
this.cmbLocation_ID.Location = new System.Drawing.Point(173,46);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 2;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####50CheckNo###String
this.lblCheckNo.AutoSize = true;
this.lblCheckNo.Location = new System.Drawing.Point(100,75);
this.lblCheckNo.Name = "lblCheckNo";
this.lblCheckNo.Size = new System.Drawing.Size(41, 12);
this.lblCheckNo.TabIndex = 3;
this.lblCheckNo.Text = "盘点单号";
this.txtCheckNo.Location = new System.Drawing.Point(173,71);
this.txtCheckNo.Name = "txtCheckNo";
this.txtCheckNo.Size = new System.Drawing.Size(100, 21);
this.txtCheckNo.TabIndex = 3;
this.Controls.Add(this.lblCheckNo);
this.Controls.Add(this.txtCheckNo);

           //#####CheckMode###Int32
//属性测试100CheckMode
//属性测试100CheckMode

           //#####Adjust_Type###Int32
//属性测试125Adjust_Type
//属性测试125Adjust_Type

           //#####CheckResult###Int32
//属性测试150CheckResult
//属性测试150CheckResult

           //#####CarryingTotalQty###Int32
//属性测试175CarryingTotalQty
//属性测试175CarryingTotalQty

           //#####CarryingTotalAmount###Decimal
this.lblCarryingTotalAmount.AutoSize = true;
this.lblCarryingTotalAmount.Location = new System.Drawing.Point(100,200);
this.lblCarryingTotalAmount.Name = "lblCarryingTotalAmount";
this.lblCarryingTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblCarryingTotalAmount.TabIndex = 8;
this.lblCarryingTotalAmount.Text = "载账总成本";
//111======200
this.txtCarryingTotalAmount.Location = new System.Drawing.Point(173,196);
this.txtCarryingTotalAmount.Name ="txtCarryingTotalAmount";
this.txtCarryingTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtCarryingTotalAmount.TabIndex = 8;
this.Controls.Add(this.lblCarryingTotalAmount);
this.Controls.Add(this.txtCarryingTotalAmount);

           //#####Check_date###DateTime
this.lblCheck_date.AutoSize = true;
this.lblCheck_date.Location = new System.Drawing.Point(100,225);
this.lblCheck_date.Name = "lblCheck_date";
this.lblCheck_date.Size = new System.Drawing.Size(41, 12);
this.lblCheck_date.TabIndex = 9;
this.lblCheck_date.Text = "盘点日期";
//111======225
this.dtpCheck_date.Location = new System.Drawing.Point(173,221);
this.dtpCheck_date.Name ="dtpCheck_date";
this.dtpCheck_date.Size = new System.Drawing.Size(100, 21);
this.dtpCheck_date.TabIndex = 9;
this.Controls.Add(this.lblCheck_date);
this.Controls.Add(this.dtpCheck_date);

           //#####CarryingDate###DateTime
this.lblCarryingDate.AutoSize = true;
this.lblCarryingDate.Location = new System.Drawing.Point(100,250);
this.lblCarryingDate.Name = "lblCarryingDate";
this.lblCarryingDate.Size = new System.Drawing.Size(41, 12);
this.lblCarryingDate.TabIndex = 10;
this.lblCarryingDate.Text = "载账日期";
//111======250
this.dtpCarryingDate.Location = new System.Drawing.Point(173,246);
this.dtpCarryingDate.Name ="dtpCarryingDate";
this.dtpCarryingDate.ShowCheckBox =true;
this.dtpCarryingDate.Size = new System.Drawing.Size(100, 21);
this.dtpCarryingDate.TabIndex = 10;
this.Controls.Add(this.lblCarryingDate);
this.Controls.Add(this.dtpCarryingDate);

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

           //#####1000Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,375);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 15;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,371);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 15;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####DiffTotalQty###Int32
//属性测试400DiffTotalQty
//属性测试400DiffTotalQty

           //#####DiffTotalAmount###Decimal
this.lblDiffTotalAmount.AutoSize = true;
this.lblDiffTotalAmount.Location = new System.Drawing.Point(100,425);
this.lblDiffTotalAmount.Name = "lblDiffTotalAmount";
this.lblDiffTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblDiffTotalAmount.TabIndex = 17;
this.lblDiffTotalAmount.Text = "差异总金额";
//111======425
this.txtDiffTotalAmount.Location = new System.Drawing.Point(173,421);
this.txtDiffTotalAmount.Name ="txtDiffTotalAmount";
this.txtDiffTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtDiffTotalAmount.TabIndex = 17;
this.Controls.Add(this.lblDiffTotalAmount);
this.Controls.Add(this.txtDiffTotalAmount);

           //#####CheckTotalQty###Int32
//属性测试450CheckTotalQty
//属性测试450CheckTotalQty

           //#####CheckTotalAmount###Decimal
this.lblCheckTotalAmount.AutoSize = true;
this.lblCheckTotalAmount.Location = new System.Drawing.Point(100,475);
this.lblCheckTotalAmount.Name = "lblCheckTotalAmount";
this.lblCheckTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblCheckTotalAmount.TabIndex = 19;
this.lblCheckTotalAmount.Text = "盘点总成本";
//111======475
this.txtCheckTotalAmount.Location = new System.Drawing.Point(173,471);
this.txtCheckTotalAmount.Name ="txtCheckTotalAmount";
this.txtCheckTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtCheckTotalAmount.TabIndex = 19;
this.Controls.Add(this.lblCheckTotalAmount);
this.Controls.Add(this.txtCheckTotalAmount);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,500);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 20;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,496);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 20;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试525DataStatus
//属性测试525DataStatus

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,550);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 22;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,546);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 22;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试575Approver_by
//属性测试575Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,600);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 24;
this.lblApprover_at.Text = "审批时间";
//111======600
this.dtpApprover_at.Location = new System.Drawing.Point(173,596);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 24;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,650);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 26;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,646);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 26;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试675PrintStatus
//属性测试675PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblCheckNo );
this.Controls.Add(this.txtCheckNo );

                
                
                
                
                this.Controls.Add(this.lblCarryingTotalAmount );
this.Controls.Add(this.txtCarryingTotalAmount );

                this.Controls.Add(this.lblCheck_date );
this.Controls.Add(this.dtpCheck_date );

                this.Controls.Add(this.lblCarryingDate );
this.Controls.Add(this.dtpCarryingDate );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                
                this.Controls.Add(this.lblDiffTotalAmount );
this.Controls.Add(this.txtDiffTotalAmount );

                
                this.Controls.Add(this.lblCheckTotalAmount );
this.Controls.Add(this.txtCheckTotalAmount );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                    
            this.Name = "tb_StocktakeQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCheckNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCheckNo;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCarryingTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCarryingTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCheck_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCheck_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCarryingDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCarryingDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiffTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiffTotalAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCheckTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCheckTotalAmount;

    
        
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


