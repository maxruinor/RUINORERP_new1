// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:39
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
    partial class tb_StocktakeEdit
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
     this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCheckNo = new Krypton.Toolkit.KryptonLabel();
this.txtCheckNo = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckMode = new Krypton.Toolkit.KryptonLabel();
this.txtCheckMode = new Krypton.Toolkit.KryptonTextBox();

this.lblAdjust_Type = new Krypton.Toolkit.KryptonLabel();
this.txtAdjust_Type = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckResult = new Krypton.Toolkit.KryptonLabel();
this.txtCheckResult = new Krypton.Toolkit.KryptonTextBox();

this.lblCarryingTotalQty = new Krypton.Toolkit.KryptonLabel();
this.txtCarryingTotalQty = new Krypton.Toolkit.KryptonTextBox();

this.lblCarryingTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCarryingTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCheck_date = new Krypton.Toolkit.KryptonLabel();
this.dtpCheck_date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCarryingDate = new Krypton.Toolkit.KryptonLabel();
this.dtpCarryingDate = new Krypton.Toolkit.KryptonDateTimePicker();

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

this.lblDiffTotalQty = new Krypton.Toolkit.KryptonLabel();
this.txtDiffTotalQty = new Krypton.Toolkit.KryptonTextBox();

this.lblDiffTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtDiffTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckTotalQty = new Krypton.Toolkit.KryptonLabel();
this.txtCheckTotalQty = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCheckTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();

    
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
this.lblCheckMode.AutoSize = true;
this.lblCheckMode.Location = new System.Drawing.Point(100,100);
this.lblCheckMode.Name = "lblCheckMode";
this.lblCheckMode.Size = new System.Drawing.Size(41, 12);
this.lblCheckMode.TabIndex = 4;
this.lblCheckMode.Text = "盘点方式";
this.txtCheckMode.Location = new System.Drawing.Point(173,96);
this.txtCheckMode.Name = "txtCheckMode";
this.txtCheckMode.Size = new System.Drawing.Size(100, 21);
this.txtCheckMode.TabIndex = 4;
this.Controls.Add(this.lblCheckMode);
this.Controls.Add(this.txtCheckMode);

           //#####Adjust_Type###Int32
//属性测试125Adjust_Type
//属性测试125Adjust_Type
this.lblAdjust_Type.AutoSize = true;
this.lblAdjust_Type.Location = new System.Drawing.Point(100,125);
this.lblAdjust_Type.Name = "lblAdjust_Type";
this.lblAdjust_Type.Size = new System.Drawing.Size(41, 12);
this.lblAdjust_Type.TabIndex = 5;
this.lblAdjust_Type.Text = "调整类型";
this.txtAdjust_Type.Location = new System.Drawing.Point(173,121);
this.txtAdjust_Type.Name = "txtAdjust_Type";
this.txtAdjust_Type.Size = new System.Drawing.Size(100, 21);
this.txtAdjust_Type.TabIndex = 5;
this.Controls.Add(this.lblAdjust_Type);
this.Controls.Add(this.txtAdjust_Type);

           //#####CheckResult###Int32
//属性测试150CheckResult
//属性测试150CheckResult
this.lblCheckResult.AutoSize = true;
this.lblCheckResult.Location = new System.Drawing.Point(100,150);
this.lblCheckResult.Name = "lblCheckResult";
this.lblCheckResult.Size = new System.Drawing.Size(41, 12);
this.lblCheckResult.TabIndex = 6;
this.lblCheckResult.Text = "盘点结果";
this.txtCheckResult.Location = new System.Drawing.Point(173,146);
this.txtCheckResult.Name = "txtCheckResult";
this.txtCheckResult.Size = new System.Drawing.Size(100, 21);
this.txtCheckResult.TabIndex = 6;
this.Controls.Add(this.lblCheckResult);
this.Controls.Add(this.txtCheckResult);

           //#####CarryingTotalQty###Int32
//属性测试175CarryingTotalQty
//属性测试175CarryingTotalQty
this.lblCarryingTotalQty.AutoSize = true;
this.lblCarryingTotalQty.Location = new System.Drawing.Point(100,175);
this.lblCarryingTotalQty.Name = "lblCarryingTotalQty";
this.lblCarryingTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblCarryingTotalQty.TabIndex = 7;
this.lblCarryingTotalQty.Text = "载账总数量";
this.txtCarryingTotalQty.Location = new System.Drawing.Point(173,171);
this.txtCarryingTotalQty.Name = "txtCarryingTotalQty";
this.txtCarryingTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtCarryingTotalQty.TabIndex = 7;
this.Controls.Add(this.lblCarryingTotalQty);
this.Controls.Add(this.txtCarryingTotalQty);

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
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,300);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 12;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,296);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 12;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,350);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 14;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,346);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 14;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
this.lblDiffTotalQty.AutoSize = true;
this.lblDiffTotalQty.Location = new System.Drawing.Point(100,400);
this.lblDiffTotalQty.Name = "lblDiffTotalQty";
this.lblDiffTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblDiffTotalQty.TabIndex = 16;
this.lblDiffTotalQty.Text = "差异总数量";
this.txtDiffTotalQty.Location = new System.Drawing.Point(173,396);
this.txtDiffTotalQty.Name = "txtDiffTotalQty";
this.txtDiffTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtDiffTotalQty.TabIndex = 16;
this.Controls.Add(this.lblDiffTotalQty);
this.Controls.Add(this.txtDiffTotalQty);

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
this.lblCheckTotalQty.AutoSize = true;
this.lblCheckTotalQty.Location = new System.Drawing.Point(100,450);
this.lblCheckTotalQty.Name = "lblCheckTotalQty";
this.lblCheckTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblCheckTotalQty.TabIndex = 18;
this.lblCheckTotalQty.Text = "盘点总数量";
this.txtCheckTotalQty.Location = new System.Drawing.Point(173,446);
this.txtCheckTotalQty.Name = "txtCheckTotalQty";
this.txtCheckTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtCheckTotalQty.TabIndex = 18;
this.Controls.Add(this.lblCheckTotalQty);
this.Controls.Add(this.txtCheckTotalQty);

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
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,525);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 21;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,521);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 21;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

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
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,575);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 23;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,571);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 23;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

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
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,675);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 27;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,671);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 27;
this.Controls.Add(this.lblPrintStatus);
this.Controls.Add(this.txtPrintStatus);

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
           // this.kryptonPanel1.TabIndex = 27;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblCheckNo );
this.Controls.Add(this.txtCheckNo );

                this.Controls.Add(this.lblCheckMode );
this.Controls.Add(this.txtCheckMode );

                this.Controls.Add(this.lblAdjust_Type );
this.Controls.Add(this.txtAdjust_Type );

                this.Controls.Add(this.lblCheckResult );
this.Controls.Add(this.txtCheckResult );

                this.Controls.Add(this.lblCarryingTotalQty );
this.Controls.Add(this.txtCarryingTotalQty );

                this.Controls.Add(this.lblCarryingTotalAmount );
this.Controls.Add(this.txtCarryingTotalAmount );

                this.Controls.Add(this.lblCheck_date );
this.Controls.Add(this.dtpCheck_date );

                this.Controls.Add(this.lblCarryingDate );
this.Controls.Add(this.dtpCarryingDate );

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

                this.Controls.Add(this.lblDiffTotalQty );
this.Controls.Add(this.txtDiffTotalQty );

                this.Controls.Add(this.lblDiffTotalAmount );
this.Controls.Add(this.txtDiffTotalAmount );

                this.Controls.Add(this.lblCheckTotalQty );
this.Controls.Add(this.txtCheckTotalQty );

                this.Controls.Add(this.lblCheckTotalAmount );
this.Controls.Add(this.txtCheckTotalAmount );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                            // 
            // "tb_StocktakeEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_StocktakeEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckNo;
private Krypton.Toolkit.KryptonTextBox txtCheckNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckMode;
private Krypton.Toolkit.KryptonTextBox txtCheckMode;

    
        
              private Krypton.Toolkit.KryptonLabel lblAdjust_Type;
private Krypton.Toolkit.KryptonTextBox txtAdjust_Type;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckResult;
private Krypton.Toolkit.KryptonTextBox txtCheckResult;

    
        
              private Krypton.Toolkit.KryptonLabel lblCarryingTotalQty;
private Krypton.Toolkit.KryptonTextBox txtCarryingTotalQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCarryingTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtCarryingTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheck_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpCheck_date;

    
        
              private Krypton.Toolkit.KryptonLabel lblCarryingDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpCarryingDate;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblDiffTotalQty;
private Krypton.Toolkit.KryptonTextBox txtDiffTotalQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiffTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtDiffTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckTotalQty;
private Krypton.Toolkit.KryptonTextBox txtCheckTotalQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtCheckTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

