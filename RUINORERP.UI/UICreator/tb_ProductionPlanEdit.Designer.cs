// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:01
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
    partial class tb_ProductionPlanEdit
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
     this.lblSOrder_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbSOrder_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblSaleOrderNo = new Krypton.Toolkit.KryptonLabel();
this.txtSaleOrderNo = new Krypton.Toolkit.KryptonTextBox();

this.lblPPNo = new Krypton.Toolkit.KryptonLabel();
this.txtPPNo = new Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblPriority = new Krypton.Toolkit.KryptonLabel();
this.txtPriority = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblRequirementDate = new Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblPlanDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPlanDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblTotalCompletedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtTotalCompletedQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtTotalQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();


this.lblAnalyzed = new Krypton.Toolkit.KryptonLabel();
this.chkAnalyzed = new Krypton.Toolkit.KryptonCheckBox();
this.chkAnalyzed.Values.Text ="";

this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCloseCaseOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new Krypton.Toolkit.KryptonTextBox();

    
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
this.lblPriority.AutoSize = true;
this.lblPriority.Location = new System.Drawing.Point(100,150);
this.lblPriority.Name = "lblPriority";
this.lblPriority.Size = new System.Drawing.Size(41, 12);
this.lblPriority.TabIndex = 6;
this.lblPriority.Text = "紧急程度";
this.txtPriority.Location = new System.Drawing.Point(173,146);
this.txtPriority.Name = "txtPriority";
this.txtPriority.Size = new System.Drawing.Size(100, 21);
this.txtPriority.TabIndex = 6;
this.Controls.Add(this.lblPriority);
this.Controls.Add(this.txtPriority);

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
this.lblTotalCompletedQuantity.AutoSize = true;
this.lblTotalCompletedQuantity.Location = new System.Drawing.Point(100,250);
this.lblTotalCompletedQuantity.Name = "lblTotalCompletedQuantity";
this.lblTotalCompletedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblTotalCompletedQuantity.TabIndex = 10;
this.lblTotalCompletedQuantity.Text = "完成数";
this.txtTotalCompletedQuantity.Location = new System.Drawing.Point(173,246);
this.txtTotalCompletedQuantity.Name = "txtTotalCompletedQuantity";
this.txtTotalCompletedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtTotalCompletedQuantity.TabIndex = 10;
this.Controls.Add(this.lblTotalCompletedQuantity);
this.Controls.Add(this.txtTotalCompletedQuantity);

           //#####TotalQuantity###Int32
//属性测试275TotalQuantity
//属性测试275TotalQuantity
//属性测试275TotalQuantity
//属性测试275TotalQuantity
this.lblTotalQuantity.AutoSize = true;
this.lblTotalQuantity.Location = new System.Drawing.Point(100,275);
this.lblTotalQuantity.Name = "lblTotalQuantity";
this.lblTotalQuantity.Size = new System.Drawing.Size(41, 12);
this.lblTotalQuantity.TabIndex = 11;
this.lblTotalQuantity.Text = "计划数";
this.txtTotalQuantity.Location = new System.Drawing.Point(173,271);
this.txtTotalQuantity.Name = "txtTotalQuantity";
this.txtTotalQuantity.Size = new System.Drawing.Size(100, 21);
this.txtTotalQuantity.TabIndex = 11;
this.Controls.Add(this.lblTotalQuantity);
this.Controls.Add(this.txtTotalQuantity);

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
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,325);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 13;
this.lblDataStatus.Text = "单据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,321);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 13;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

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
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,375);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 15;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,371);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 15;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,425);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 17;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,421);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 17;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,575);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 23;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,571);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 23;
this.Controls.Add(this.lblPrintStatus);
this.Controls.Add(this.txtPrintStatus);

           //#####Approver_by###Int64
//属性测试600Approver_by
//属性测试600Approver_by
//属性测试600Approver_by
//属性测试600Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,600);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 24;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,596);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 24;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

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
           // this.kryptonPanel1.TabIndex = 26;

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

                this.Controls.Add(this.lblPriority );
this.Controls.Add(this.txtPriority );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblPlanDate );
this.Controls.Add(this.dtpPlanDate );

                this.Controls.Add(this.lblTotalCompletedQuantity );
this.Controls.Add(this.txtTotalCompletedQuantity );

                this.Controls.Add(this.lblTotalQuantity );
this.Controls.Add(this.txtTotalQuantity );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblAnalyzed );
this.Controls.Add(this.chkAnalyzed );

                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                            // 
            // "tb_ProductionPlanEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProductionPlanEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblSOrder_ID;
private Krypton.Toolkit.KryptonComboBox cmbSOrder_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleOrderNo;
private Krypton.Toolkit.KryptonTextBox txtSaleOrderNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblPPNo;
private Krypton.Toolkit.KryptonTextBox txtPPNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPriority;
private Krypton.Toolkit.KryptonTextBox txtPriority;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRequirementDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlanDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPlanDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalCompletedQuantity;
private Krypton.Toolkit.KryptonTextBox txtTotalCompletedQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalQuantity;
private Krypton.Toolkit.KryptonTextBox txtTotalQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblAnalyzed;
private Krypton.Toolkit.KryptonCheckBox chkAnalyzed;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

