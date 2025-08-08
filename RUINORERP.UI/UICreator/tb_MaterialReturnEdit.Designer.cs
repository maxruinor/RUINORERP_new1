// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:42
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本
    /// </summary>
    partial class tb_MaterialReturnEdit
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
     this.lblBillNo = new Krypton.Toolkit.KryptonLabel();
this.txtBillNo = new Krypton.Toolkit.KryptonTextBox();

this.lblBillType = new Krypton.Toolkit.KryptonLabel();
this.txtBillType = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblOutgoing = new Krypton.Toolkit.KryptonLabel();
this.chkOutgoing = new Krypton.Toolkit.KryptonCheckBox();
this.chkOutgoing.Values.Text ="";

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblTotalQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalQty = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalCostAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalCostAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblReturnDate = new Krypton.Toolkit.KryptonLabel();
this.dtpReturnDate = new Krypton.Toolkit.KryptonDateTimePicker();

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

this.lblMR_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbMR_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblMaterialRequisitionNO = new Krypton.Toolkit.KryptonLabel();
this.txtMaterialRequisitionNO = new Krypton.Toolkit.KryptonTextBox();

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


this.lblGeneEvidence = new Krypton.Toolkit.KryptonLabel();
this.chkGeneEvidence = new Krypton.Toolkit.KryptonCheckBox();
this.chkGeneEvidence.Values.Text ="";

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
     
            //#####50BillNo###String
this.lblBillNo.AutoSize = true;
this.lblBillNo.Location = new System.Drawing.Point(100,25);
this.lblBillNo.Name = "lblBillNo";
this.lblBillNo.Size = new System.Drawing.Size(41, 12);
this.lblBillNo.TabIndex = 1;
this.lblBillNo.Text = "退料单号";
this.txtBillNo.Location = new System.Drawing.Point(173,21);
this.txtBillNo.Name = "txtBillNo";
this.txtBillNo.Size = new System.Drawing.Size(100, 21);
this.txtBillNo.TabIndex = 1;
this.Controls.Add(this.lblBillNo);
this.Controls.Add(this.txtBillNo);

           //#####BillType###Int32
//属性测试50BillType
//属性测试50BillType
//属性测试50BillType
//属性测试50BillType
this.lblBillType.AutoSize = true;
this.lblBillType.Location = new System.Drawing.Point(100,50);
this.lblBillType.Name = "lblBillType";
this.lblBillType.Size = new System.Drawing.Size(41, 12);
this.lblBillType.TabIndex = 2;
this.lblBillType.Text = "单据类别";
this.txtBillType.Location = new System.Drawing.Point(173,46);
this.txtBillType.Name = "txtBillType";
this.txtBillType.Size = new System.Drawing.Size(100, 21);
this.txtBillType.TabIndex = 2;
this.Controls.Add(this.lblBillType);
this.Controls.Add(this.txtBillType);

           //#####Employee_ID###Int64
//属性测试75Employee_ID
//属性测试75Employee_ID
//属性测试75Employee_ID
//属性测试75Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,75);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 3;
this.lblEmployee_ID.Text = "经办人";
//111======75
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,71);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 3;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####DepartmentID###Int64
//属性测试100DepartmentID
//属性测试100DepartmentID
//属性测试100DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,100);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 4;
this.lblDepartmentID.Text = "生产部门";
//111======100
this.cmbDepartmentID.Location = new System.Drawing.Point(173,96);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 4;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####Outgoing###Boolean
this.lblOutgoing.AutoSize = true;
this.lblOutgoing.Location = new System.Drawing.Point(100,125);
this.lblOutgoing.Name = "lblOutgoing";
this.lblOutgoing.Size = new System.Drawing.Size(41, 12);
this.lblOutgoing.TabIndex = 5;
this.lblOutgoing.Text = "外发加工";
this.chkOutgoing.Location = new System.Drawing.Point(173,121);
this.chkOutgoing.Name = "chkOutgoing";
this.chkOutgoing.Size = new System.Drawing.Size(100, 21);
this.chkOutgoing.TabIndex = 5;
this.Controls.Add(this.lblOutgoing);
this.Controls.Add(this.chkOutgoing);

           //#####CustomerVendor_ID###Int64
//属性测试150CustomerVendor_ID
//属性测试150CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,150);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 6;
this.lblCustomerVendor_ID.Text = "加工厂商";
//111======150
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,146);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 6;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####TotalQty###Decimal
this.lblTotalQty.AutoSize = true;
this.lblTotalQty.Location = new System.Drawing.Point(100,175);
this.lblTotalQty.Name = "lblTotalQty";
this.lblTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalQty.TabIndex = 7;
this.lblTotalQty.Text = "总数量";
//111======175
this.txtTotalQty.Location = new System.Drawing.Point(173,171);
this.txtTotalQty.Name ="txtTotalQty";
this.txtTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalQty.TabIndex = 7;
this.Controls.Add(this.lblTotalQty);
this.Controls.Add(this.txtTotalQty);

           //#####TotalCostAmount###Decimal
this.lblTotalCostAmount.AutoSize = true;
this.lblTotalCostAmount.Location = new System.Drawing.Point(100,200);
this.lblTotalCostAmount.Name = "lblTotalCostAmount";
this.lblTotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalCostAmount.TabIndex = 8;
this.lblTotalCostAmount.Text = "总成本";
//111======200
this.txtTotalCostAmount.Location = new System.Drawing.Point(173,196);
this.txtTotalCostAmount.Name ="txtTotalCostAmount";
this.txtTotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalCostAmount.TabIndex = 8;
this.Controls.Add(this.lblTotalCostAmount);
this.Controls.Add(this.txtTotalCostAmount);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,225);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 9;
this.lblTotalAmount.Text = "总金额";
//111======225
this.txtTotalAmount.Location = new System.Drawing.Point(173,221);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 9;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####ReturnDate###DateTime
this.lblReturnDate.AutoSize = true;
this.lblReturnDate.Location = new System.Drawing.Point(100,250);
this.lblReturnDate.Name = "lblReturnDate";
this.lblReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblReturnDate.TabIndex = 10;
this.lblReturnDate.Text = "退料日期";
//111======250
this.dtpReturnDate.Location = new System.Drawing.Point(173,246);
this.dtpReturnDate.Name ="dtpReturnDate";
this.dtpReturnDate.ShowCheckBox =true;
this.dtpReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpReturnDate.TabIndex = 10;
this.Controls.Add(this.lblReturnDate);
this.Controls.Add(this.dtpReturnDate);

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

           //#####255Notes###String
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

           //#####MR_ID###Int64
//属性测试400MR_ID
this.lblMR_ID.AutoSize = true;
this.lblMR_ID.Location = new System.Drawing.Point(100,400);
this.lblMR_ID.Name = "lblMR_ID";
this.lblMR_ID.Size = new System.Drawing.Size(41, 12);
this.lblMR_ID.TabIndex = 16;
this.lblMR_ID.Text = "领料单";
//111======400
this.cmbMR_ID.Location = new System.Drawing.Point(173,396);
this.cmbMR_ID.Name ="cmbMR_ID";
this.cmbMR_ID.Size = new System.Drawing.Size(100, 21);
this.cmbMR_ID.TabIndex = 16;
this.Controls.Add(this.lblMR_ID);
this.Controls.Add(this.cmbMR_ID);

           //#####50MaterialRequisitionNO###String
this.lblMaterialRequisitionNO.AutoSize = true;
this.lblMaterialRequisitionNO.Location = new System.Drawing.Point(100,425);
this.lblMaterialRequisitionNO.Name = "lblMaterialRequisitionNO";
this.lblMaterialRequisitionNO.Size = new System.Drawing.Size(41, 12);
this.lblMaterialRequisitionNO.TabIndex = 17;
this.lblMaterialRequisitionNO.Text = "领料单号";
this.txtMaterialRequisitionNO.Location = new System.Drawing.Point(173,421);
this.txtMaterialRequisitionNO.Name = "txtMaterialRequisitionNO";
this.txtMaterialRequisitionNO.Size = new System.Drawing.Size(100, 21);
this.txtMaterialRequisitionNO.TabIndex = 17;
this.Controls.Add(this.lblMaterialRequisitionNO);
this.Controls.Add(this.txtMaterialRequisitionNO);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,450);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 18;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,446);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 18;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试475DataStatus
//属性测试475DataStatus
//属性测试475DataStatus
//属性测试475DataStatus
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,475);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 19;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,471);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 19;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,500);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 20;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,496);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 20;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试525Approver_by
//属性测试525Approver_by
//属性测试525Approver_by
//属性测试525Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,525);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 21;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,521);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 21;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,550);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 22;
this.lblApprover_at.Text = "审批时间";
//111======550
this.dtpApprover_at.Location = new System.Drawing.Point(173,546);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 22;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####GeneEvidence###Boolean
this.lblGeneEvidence.AutoSize = true;
this.lblGeneEvidence.Location = new System.Drawing.Point(100,600);
this.lblGeneEvidence.Name = "lblGeneEvidence";
this.lblGeneEvidence.Size = new System.Drawing.Size(41, 12);
this.lblGeneEvidence.TabIndex = 24;
this.lblGeneEvidence.Text = "产生凭证";
this.chkGeneEvidence.Location = new System.Drawing.Point(173,596);
this.chkGeneEvidence.Name = "chkGeneEvidence";
this.chkGeneEvidence.Size = new System.Drawing.Size(100, 21);
this.chkGeneEvidence.TabIndex = 24;
this.Controls.Add(this.lblGeneEvidence);
this.Controls.Add(this.chkGeneEvidence);

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,625);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 25;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,621);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 25;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试650PrintStatus
//属性测试650PrintStatus
//属性测试650PrintStatus
//属性测试650PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,650);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 26;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,646);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 26;
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
           // this.kryptonPanel1.TabIndex = 26;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblBillNo );
this.Controls.Add(this.txtBillNo );

                this.Controls.Add(this.lblBillType );
this.Controls.Add(this.txtBillType );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblOutgoing );
this.Controls.Add(this.chkOutgoing );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblTotalQty );
this.Controls.Add(this.txtTotalQty );

                this.Controls.Add(this.lblTotalCostAmount );
this.Controls.Add(this.txtTotalCostAmount );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblReturnDate );
this.Controls.Add(this.dtpReturnDate );

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

                this.Controls.Add(this.lblMR_ID );
this.Controls.Add(this.cmbMR_ID );

                this.Controls.Add(this.lblMaterialRequisitionNO );
this.Controls.Add(this.txtMaterialRequisitionNO );

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

                
                this.Controls.Add(this.lblGeneEvidence );
this.Controls.Add(this.chkGeneEvidence );

                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                            // 
            // "tb_MaterialReturnEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_MaterialReturnEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblBillNo;
private Krypton.Toolkit.KryptonTextBox txtBillNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblBillType;
private Krypton.Toolkit.KryptonTextBox txtBillType;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblOutgoing;
private Krypton.Toolkit.KryptonCheckBox chkOutgoing;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalQty;
private Krypton.Toolkit.KryptonTextBox txtTotalQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalCostAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalCostAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblReturnDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblMR_ID;
private Krypton.Toolkit.KryptonComboBox cmbMR_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblMaterialRequisitionNO;
private Krypton.Toolkit.KryptonTextBox txtMaterialRequisitionNO;

    
        
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

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblGeneEvidence;
private Krypton.Toolkit.KryptonCheckBox chkGeneEvidence;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

