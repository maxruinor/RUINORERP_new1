
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:21
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 售后申请单 -登记，评估，清单，确认。目标是维修翻新
    /// </summary>
    partial class tb_AS_AfterSaleApplyQuery
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
     
     this.lblASApplyNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtASApplyNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCustomerSourceNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerSourceNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();



this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();



this.lblApplyDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApplyDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblPreDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPreDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblShippingAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtShippingAddress.Multiline = true;

this.lblShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInWarrantyPeriod = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkInWarrantyPeriod = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkInWarrantyPeriod.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblRepairEvaluationOpinion = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRepairEvaluationOpinion = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRepairEvaluationOpinion.Multiline = true;



this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;


this.lblMaterialFeeConfirmed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkMaterialFeeConfirmed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkMaterialFeeConfirmed.Values.Text ="";

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
                 //#####50ASApplyNo###String
this.lblASApplyNo.AutoSize = true;
this.lblASApplyNo.Location = new System.Drawing.Point(100,25);
this.lblASApplyNo.Name = "lblASApplyNo";
this.lblASApplyNo.Size = new System.Drawing.Size(41, 12);
this.lblASApplyNo.TabIndex = 1;
this.lblASApplyNo.Text = "申请编号";
this.txtASApplyNo.Location = new System.Drawing.Point(173,21);
this.txtASApplyNo.Name = "txtASApplyNo";
this.txtASApplyNo.Size = new System.Drawing.Size(100, 21);
this.txtASApplyNo.TabIndex = 1;
this.Controls.Add(this.lblASApplyNo);
this.Controls.Add(this.txtASApplyNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "申请客户";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####50CustomerSourceNo###String
this.lblCustomerSourceNo.AutoSize = true;
this.lblCustomerSourceNo.Location = new System.Drawing.Point(100,75);
this.lblCustomerSourceNo.Name = "lblCustomerSourceNo";
this.lblCustomerSourceNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerSourceNo.TabIndex = 3;
this.lblCustomerSourceNo.Text = "来源单号";
this.txtCustomerSourceNo.Location = new System.Drawing.Point(173,71);
this.txtCustomerSourceNo.Name = "txtCustomerSourceNo";
this.txtCustomerSourceNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerSourceNo.TabIndex = 3;
this.Controls.Add(this.lblCustomerSourceNo);
this.Controls.Add(this.txtCustomerSourceNo);

           //#####Location_ID###Int64
//属性测试100Location_ID
//属性测试100Location_ID
//属性测试100Location_ID
//属性测试100Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,100);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 4;
this.lblLocation_ID.Text = "售后暂存仓";
//111======100
this.cmbLocation_ID.Location = new System.Drawing.Point(173,96);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 4;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####Priority###Int32
//属性测试125Priority
//属性测试125Priority
//属性测试125Priority
//属性测试125Priority

           //#####ASProcessStatus###Int32
//属性测试150ASProcessStatus
//属性测试150ASProcessStatus
//属性测试150ASProcessStatus
//属性测试150ASProcessStatus

           //#####Employee_ID###Int64
//属性测试175Employee_ID
//属性测试175Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,175);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 7;
this.lblEmployee_ID.Text = "业务员";
//111======175
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,171);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 7;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####ProjectGroup_ID###Int64
//属性测试200ProjectGroup_ID
//属性测试200ProjectGroup_ID
//属性测试200ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,200);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 8;
this.lblProjectGroup_ID.Text = "项目小组";
//111======200
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,196);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 8;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####TotalInitialQuantity###Int32
//属性测试225TotalInitialQuantity
//属性测试225TotalInitialQuantity
//属性测试225TotalInitialQuantity
//属性测试225TotalInitialQuantity

           //#####TotalConfirmedQuantity###Int32
//属性测试250TotalConfirmedQuantity
//属性测试250TotalConfirmedQuantity
//属性测试250TotalConfirmedQuantity
//属性测试250TotalConfirmedQuantity

           //#####ApplyDate###DateTime
this.lblApplyDate.AutoSize = true;
this.lblApplyDate.Location = new System.Drawing.Point(100,275);
this.lblApplyDate.Name = "lblApplyDate";
this.lblApplyDate.Size = new System.Drawing.Size(41, 12);
this.lblApplyDate.TabIndex = 11;
this.lblApplyDate.Text = "申请日期";
//111======275
this.dtpApplyDate.Location = new System.Drawing.Point(173,271);
this.dtpApplyDate.Name ="dtpApplyDate";
this.dtpApplyDate.Size = new System.Drawing.Size(100, 21);
this.dtpApplyDate.TabIndex = 11;
this.Controls.Add(this.lblApplyDate);
this.Controls.Add(this.dtpApplyDate);

           //#####PreDeliveryDate###DateTime
this.lblPreDeliveryDate.AutoSize = true;
this.lblPreDeliveryDate.Location = new System.Drawing.Point(100,300);
this.lblPreDeliveryDate.Name = "lblPreDeliveryDate";
this.lblPreDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblPreDeliveryDate.TabIndex = 12;
this.lblPreDeliveryDate.Text = "预交日期";
//111======300
this.dtpPreDeliveryDate.Location = new System.Drawing.Point(173,296);
this.dtpPreDeliveryDate.Name ="dtpPreDeliveryDate";
this.dtpPreDeliveryDate.ShowCheckBox =true;
this.dtpPreDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreDeliveryDate.TabIndex = 12;
this.Controls.Add(this.lblPreDeliveryDate);
this.Controls.Add(this.dtpPreDeliveryDate);

           //#####500ShippingAddress###String
this.lblShippingAddress.AutoSize = true;
this.lblShippingAddress.Location = new System.Drawing.Point(100,325);
this.lblShippingAddress.Name = "lblShippingAddress";
this.lblShippingAddress.Size = new System.Drawing.Size(41, 12);
this.lblShippingAddress.TabIndex = 13;
this.lblShippingAddress.Text = "收货地址";
this.txtShippingAddress.Location = new System.Drawing.Point(173,321);
this.txtShippingAddress.Name = "txtShippingAddress";
this.txtShippingAddress.Size = new System.Drawing.Size(100, 21);
this.txtShippingAddress.TabIndex = 13;
this.Controls.Add(this.lblShippingAddress);
this.Controls.Add(this.txtShippingAddress);

           //#####50ShippingWay###String
this.lblShippingWay.AutoSize = true;
this.lblShippingWay.Location = new System.Drawing.Point(100,350);
this.lblShippingWay.Name = "lblShippingWay";
this.lblShippingWay.Size = new System.Drawing.Size(41, 12);
this.lblShippingWay.TabIndex = 14;
this.lblShippingWay.Text = "发货方式";
this.txtShippingWay.Location = new System.Drawing.Point(173,346);
this.txtShippingWay.Name = "txtShippingWay";
this.txtShippingWay.Size = new System.Drawing.Size(100, 21);
this.txtShippingWay.TabIndex = 14;
this.Controls.Add(this.lblShippingWay);
this.Controls.Add(this.txtShippingWay);

           //#####InWarrantyPeriod###Boolean
this.lblInWarrantyPeriod.AutoSize = true;
this.lblInWarrantyPeriod.Location = new System.Drawing.Point(100,375);
this.lblInWarrantyPeriod.Name = "lblInWarrantyPeriod";
this.lblInWarrantyPeriod.Size = new System.Drawing.Size(41, 12);
this.lblInWarrantyPeriod.TabIndex = 15;
this.lblInWarrantyPeriod.Text = "保修期内";
this.chkInWarrantyPeriod.Location = new System.Drawing.Point(173,371);
this.chkInWarrantyPeriod.Name = "chkInWarrantyPeriod";
this.chkInWarrantyPeriod.Size = new System.Drawing.Size(100, 21);
this.chkInWarrantyPeriod.TabIndex = 15;
this.Controls.Add(this.lblInWarrantyPeriod);
this.Controls.Add(this.chkInWarrantyPeriod);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,400);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 16;
this.lblCreated_at.Text = "创建时间";
//111======400
this.dtpCreated_at.Location = new System.Drawing.Point(173,396);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 16;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试425Created_by
//属性测试425Created_by
//属性测试425Created_by
//属性测试425Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,450);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 18;
this.lblModified_at.Text = "修改时间";
//111======450
this.dtpModified_at.Location = new System.Drawing.Point(173,446);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 18;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试475Modified_by
//属性测试475Modified_by
//属性测试475Modified_by
//属性测试475Modified_by

           //#####500RepairEvaluationOpinion###String
this.lblRepairEvaluationOpinion.AutoSize = true;
this.lblRepairEvaluationOpinion.Location = new System.Drawing.Point(100,500);
this.lblRepairEvaluationOpinion.Name = "lblRepairEvaluationOpinion";
this.lblRepairEvaluationOpinion.Size = new System.Drawing.Size(41, 12);
this.lblRepairEvaluationOpinion.TabIndex = 20;
this.lblRepairEvaluationOpinion.Text = "维修评估意见";
this.txtRepairEvaluationOpinion.Location = new System.Drawing.Point(173,496);
this.txtRepairEvaluationOpinion.Name = "txtRepairEvaluationOpinion";
this.txtRepairEvaluationOpinion.Size = new System.Drawing.Size(100, 21);
this.txtRepairEvaluationOpinion.TabIndex = 20;
this.Controls.Add(this.lblRepairEvaluationOpinion);
this.Controls.Add(this.txtRepairEvaluationOpinion);

           //#####ExpenseAllocationMode###Int32
//属性测试525ExpenseAllocationMode
//属性测试525ExpenseAllocationMode
//属性测试525ExpenseAllocationMode
//属性测试525ExpenseAllocationMode

           //#####ExpenseBearerType###Int32
//属性测试550ExpenseBearerType
//属性测试550ExpenseBearerType
//属性测试550ExpenseBearerType
//属性测试550ExpenseBearerType

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,575);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 23;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,571);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 23;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####TotalDeliveredQty###Int32
//属性测试600TotalDeliveredQty
//属性测试600TotalDeliveredQty
//属性测试600TotalDeliveredQty
//属性测试600TotalDeliveredQty

           //#####MaterialFeeConfirmed###Boolean
this.lblMaterialFeeConfirmed.AutoSize = true;
this.lblMaterialFeeConfirmed.Location = new System.Drawing.Point(100,625);
this.lblMaterialFeeConfirmed.Name = "lblMaterialFeeConfirmed";
this.lblMaterialFeeConfirmed.Size = new System.Drawing.Size(41, 12);
this.lblMaterialFeeConfirmed.TabIndex = 25;
this.lblMaterialFeeConfirmed.Text = "费用确认状态";
this.chkMaterialFeeConfirmed.Location = new System.Drawing.Point(173,621);
this.chkMaterialFeeConfirmed.Name = "chkMaterialFeeConfirmed";
this.chkMaterialFeeConfirmed.Size = new System.Drawing.Size(100, 21);
this.chkMaterialFeeConfirmed.TabIndex = 25;
this.Controls.Add(this.lblMaterialFeeConfirmed);
this.Controls.Add(this.chkMaterialFeeConfirmed);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,650);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 26;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,646);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 26;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,675);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 27;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,671);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 27;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试700Approver_by
//属性测试700Approver_by
//属性测试700Approver_by
//属性测试700Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,725);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 29;
this.lblApprover_at.Text = "审批时间";
//111======725
this.dtpApprover_at.Location = new System.Drawing.Point(173,721);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 29;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,775);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 31;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,771);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 31;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####DataStatus###Int32
//属性测试800DataStatus
//属性测试800DataStatus
//属性测试800DataStatus
//属性测试800DataStatus

           //#####PrintStatus###Int32
//属性测试825PrintStatus
//属性测试825PrintStatus
//属性测试825PrintStatus
//属性测试825PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblASApplyNo );
this.Controls.Add(this.txtASApplyNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblCustomerSourceNo );
this.Controls.Add(this.txtCustomerSourceNo );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                
                
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                
                
                this.Controls.Add(this.lblApplyDate );
this.Controls.Add(this.dtpApplyDate );

                this.Controls.Add(this.lblPreDeliveryDate );
this.Controls.Add(this.dtpPreDeliveryDate );

                this.Controls.Add(this.lblShippingAddress );
this.Controls.Add(this.txtShippingAddress );

                this.Controls.Add(this.lblShippingWay );
this.Controls.Add(this.txtShippingWay );

                this.Controls.Add(this.lblInWarrantyPeriod );
this.Controls.Add(this.chkInWarrantyPeriod );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblRepairEvaluationOpinion );
this.Controls.Add(this.txtRepairEvaluationOpinion );

                
                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                
                this.Controls.Add(this.lblMaterialFeeConfirmed );
this.Controls.Add(this.chkMaterialFeeConfirmed );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                
                    
            this.Name = "tb_AS_AfterSaleApplyQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblASApplyNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtASApplyNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerSourceNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerSourceNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApplyDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApplyDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPreDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPreDeliveryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingWay;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingWay;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInWarrantyPeriod;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkInWarrantyPeriod;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairEvaluationOpinion;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRepairEvaluationOpinion;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaterialFeeConfirmed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkMaterialFeeConfirmed;

    
        
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


