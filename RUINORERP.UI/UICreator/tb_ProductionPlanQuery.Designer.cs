
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:02
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 生产计划表 应该是分析来的。可能来自于生产需求单，比方系统根据库存情况分析销售情况等也也可以手动。也可以程序分析
    /// </summary>
    partial class tb_ProductionPlanQuery
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
     
     this.lblSOrder_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbSOrder_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSaleOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSaleOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPPNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPPNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblPlanDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPlanDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";


this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblAnalyzed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkAnalyzed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkAnalyzed.Values.Text ="";

this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";



this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####SOrder_ID###Int64
//属性测试25SOrder_ID
this.lblSOrder_ID.AutoSize = true;
this.lblSOrder_ID.Location = new System.Drawing.Point(100,25);
this.lblSOrder_ID.Name = "lblSOrder_ID";
this.lblSOrder_ID.Size = new System.Drawing.Size(41, 12);
this.lblSOrder_ID.TabIndex = 1;
this.lblSOrder_ID.Text = "销售单号";
//111======25
this.cmbSOrder_ID.Location = new System.Drawing.Point(173,21);
this.cmbSOrder_ID.Name ="cmbSOrder_ID";
this.cmbSOrder_ID.Size = new System.Drawing.Size(100, 21);
this.cmbSOrder_ID.TabIndex = 1;
this.Controls.Add(this.lblSOrder_ID);
this.Controls.Add(this.cmbSOrder_ID);

           //#####50SaleOrderNo###String
this.lblSaleOrderNo.AutoSize = true;
this.lblSaleOrderNo.Location = new System.Drawing.Point(100,50);
this.lblSaleOrderNo.Name = "lblSaleOrderNo";
this.lblSaleOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblSaleOrderNo.TabIndex = 2;
this.lblSaleOrderNo.Text = "销售单号";
this.txtSaleOrderNo.Location = new System.Drawing.Point(173,46);
this.txtSaleOrderNo.Name = "txtSaleOrderNo";
this.txtSaleOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtSaleOrderNo.TabIndex = 2;
this.Controls.Add(this.lblSaleOrderNo);
this.Controls.Add(this.txtSaleOrderNo);

           //#####100PPNo###String
this.lblPPNo.AutoSize = true;
this.lblPPNo.Location = new System.Drawing.Point(100,75);
this.lblPPNo.Name = "lblPPNo";
this.lblPPNo.Size = new System.Drawing.Size(41, 12);
this.lblPPNo.TabIndex = 3;
this.lblPPNo.Text = "计划单号";
this.txtPPNo.Location = new System.Drawing.Point(173,71);
this.txtPPNo.Name = "txtPPNo";
this.txtPPNo.Size = new System.Drawing.Size(100, 21);
this.txtPPNo.TabIndex = 3;
this.Controls.Add(this.lblPPNo);
this.Controls.Add(this.txtPPNo);

           //#####ProjectGroup_ID###Int64
//属性测试100ProjectGroup_ID
//属性测试100ProjectGroup_ID
//属性测试100ProjectGroup_ID
//属性测试100ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,100);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 4;
this.lblProjectGroup_ID.Text = "项目组";
//111======100
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,96);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 4;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####DepartmentID###Int64
//属性测试125DepartmentID
//属性测试125DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,125);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 5;
this.lblDepartmentID.Text = "需求部门";
//111======125
this.cmbDepartmentID.Location = new System.Drawing.Point(173,121);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 5;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####Priority###Int32
//属性测试150Priority
//属性测试150Priority
//属性测试150Priority
//属性测试150Priority

           //#####Employee_ID###Int64
//属性测试175Employee_ID
//属性测试175Employee_ID
//属性测试175Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,175);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 7;
this.lblEmployee_ID.Text = "经办人";
//111======175
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,171);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 7;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,200);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 8;
this.lblRequirementDate.Text = "需求日期";
//111======200
this.dtpRequirementDate.Location = new System.Drawing.Point(173,196);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 8;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

           //#####PlanDate###DateTime
this.lblPlanDate.AutoSize = true;
this.lblPlanDate.Location = new System.Drawing.Point(100,225);
this.lblPlanDate.Name = "lblPlanDate";
this.lblPlanDate.Size = new System.Drawing.Size(41, 12);
this.lblPlanDate.TabIndex = 9;
this.lblPlanDate.Text = "制单日期";
//111======225
this.dtpPlanDate.Location = new System.Drawing.Point(173,221);
this.dtpPlanDate.Name ="dtpPlanDate";
this.dtpPlanDate.Size = new System.Drawing.Size(100, 21);
this.dtpPlanDate.TabIndex = 9;
this.Controls.Add(this.lblPlanDate);
this.Controls.Add(this.dtpPlanDate);

           //#####TotalCompletedQuantity###Int32
//属性测试250TotalCompletedQuantity
//属性测试250TotalCompletedQuantity
//属性测试250TotalCompletedQuantity
//属性测试250TotalCompletedQuantity

           //#####TotalQuantity###Int32
//属性测试275TotalQuantity
//属性测试275TotalQuantity
//属性测试275TotalQuantity
//属性测试275TotalQuantity

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,300);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 12;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,296);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 12;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试325DataStatus
//属性测试325DataStatus
//属性测试325DataStatus
//属性测试325DataStatus

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,350);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 14;
this.lblCreated_at.Text = "创建时间";
//111======350
this.dtpCreated_at.Location = new System.Drawing.Point(173,346);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 14;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试375Created_by
//属性测试375Created_by
//属性测试375Created_by
//属性测试375Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,400);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 16;
this.lblModified_at.Text = "修改时间";
//111======400
this.dtpModified_at.Location = new System.Drawing.Point(173,396);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 16;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试425Modified_by
//属性测试425Modified_by
//属性测试425Modified_by
//属性测试425Modified_by

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,450);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 18;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,446);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 18;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
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

           //#####ApprovalStatus###SByte

           //#####Analyzed###Boolean
this.lblAnalyzed.AutoSize = true;
this.lblAnalyzed.Location = new System.Drawing.Point(100,525);
this.lblAnalyzed.Name = "lblAnalyzed";
this.lblAnalyzed.Size = new System.Drawing.Size(41, 12);
this.lblAnalyzed.TabIndex = 21;
this.lblAnalyzed.Text = "已分析";
this.chkAnalyzed.Location = new System.Drawing.Point(173,521);
this.chkAnalyzed.Name = "chkAnalyzed";
this.chkAnalyzed.Size = new System.Drawing.Size(100, 21);
this.chkAnalyzed.TabIndex = 21;
this.Controls.Add(this.lblAnalyzed);
this.Controls.Add(this.chkAnalyzed);

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,550);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 22;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,546);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 22;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试575PrintStatus
//属性测试575PrintStatus
//属性测试575PrintStatus
//属性测试575PrintStatus

           //#####Approver_by###Int64
//属性测试600Approver_by
//属性测试600Approver_by
//属性测试600Approver_by
//属性测试600Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,625);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 25;
this.lblApprover_at.Text = "审批时间";
//111======625
this.dtpApprover_at.Location = new System.Drawing.Point(173,621);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 25;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,650);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 26;
this.lblCloseCaseOpinions.Text = "审批意见";
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
                this.Controls.Add(this.lblSOrder_ID );
this.Controls.Add(this.cmbSOrder_ID );

                this.Controls.Add(this.lblSaleOrderNo );
this.Controls.Add(this.txtSaleOrderNo );

                this.Controls.Add(this.lblPPNo );
this.Controls.Add(this.txtPPNo );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblPlanDate );
this.Controls.Add(this.dtpPlanDate );

                
                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblAnalyzed );
this.Controls.Add(this.chkAnalyzed );

                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                    
            this.Name = "tb_ProductionPlanQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSOrder_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSOrder_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSaleOrderNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPPNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPPNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRequirementDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlanDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPlanDate;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAnalyzed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkAnalyzed;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
    
   
 





    }
}


