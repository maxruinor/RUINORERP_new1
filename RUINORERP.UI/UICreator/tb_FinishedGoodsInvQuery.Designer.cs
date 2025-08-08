
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:24
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 成品入库单 要进一步完善
    /// </summary>
    partial class tb_FinishedGoodsInvQuery
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
     
     this.lblDeliveryBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDeliveryBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblIsOutSourced = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsOutSourced = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsOutSourced.Values.Text ="";

this.lblShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMONo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMONo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMOID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbMOID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";


this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblGeneEvidence = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGeneEvidence = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGeneEvidence.Values.Text ="";

this.lblTotalNetMachineHours = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalNetMachineHours = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalNetWorkingHours = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalNetWorkingHours = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalManuFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalManuFee = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalProductionCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalProductionCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50DeliveryBillNo###String
this.lblDeliveryBillNo.AutoSize = true;
this.lblDeliveryBillNo.Location = new System.Drawing.Point(100,25);
this.lblDeliveryBillNo.Name = "lblDeliveryBillNo";
this.lblDeliveryBillNo.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryBillNo.TabIndex = 1;
this.lblDeliveryBillNo.Text = "缴库单号";
this.txtDeliveryBillNo.Location = new System.Drawing.Point(173,21);
this.txtDeliveryBillNo.Name = "txtDeliveryBillNo";
this.txtDeliveryBillNo.Size = new System.Drawing.Size(100, 21);
this.txtDeliveryBillNo.TabIndex = 1;
this.Controls.Add(this.lblDeliveryBillNo);
this.Controls.Add(this.txtDeliveryBillNo);

           //#####Employee_ID###Int64
//属性测试50Employee_ID
//属性测试50Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "经办人员";
//111======50
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,46);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 2;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####DepartmentID###Int64
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,75);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 3;
this.lblDepartmentID.Text = "生产部门";
//111======75
this.cmbDepartmentID.Location = new System.Drawing.Point(173,71);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 3;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####CustomerVendor_ID###Int64
//属性测试100CustomerVendor_ID
//属性测试100CustomerVendor_ID
//属性测试100CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,100);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 4;
this.lblCustomerVendor_ID.Text = "外发工厂";
//111======100
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,96);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 4;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####DeliveryDate###DateTime
this.lblDeliveryDate.AutoSize = true;
this.lblDeliveryDate.Location = new System.Drawing.Point(100,125);
this.lblDeliveryDate.Name = "lblDeliveryDate";
this.lblDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryDate.TabIndex = 5;
this.lblDeliveryDate.Text = "缴库日期";
//111======125
this.dtpDeliveryDate.Location = new System.Drawing.Point(173,121);
this.dtpDeliveryDate.Name ="dtpDeliveryDate";
this.dtpDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpDeliveryDate.TabIndex = 5;
this.Controls.Add(this.lblDeliveryDate);
this.Controls.Add(this.dtpDeliveryDate);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "创建时间";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试175Created_by
//属性测试175Created_by
//属性测试175Created_by
//属性测试175Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,200);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 8;
this.lblModified_at.Text = "修改时间";
//111======200
this.dtpModified_at.Location = new System.Drawing.Point(173,196);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 8;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试225Modified_by
//属性测试225Modified_by
//属性测试225Modified_by
//属性测试225Modified_by

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,250);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 10;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,246);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 10;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####IsOutSourced###Boolean
this.lblIsOutSourced.AutoSize = true;
this.lblIsOutSourced.Location = new System.Drawing.Point(100,275);
this.lblIsOutSourced.Name = "lblIsOutSourced";
this.lblIsOutSourced.Size = new System.Drawing.Size(41, 12);
this.lblIsOutSourced.TabIndex = 11;
this.lblIsOutSourced.Text = "是否托工";
this.chkIsOutSourced.Location = new System.Drawing.Point(173,271);
this.chkIsOutSourced.Name = "chkIsOutSourced";
this.chkIsOutSourced.Size = new System.Drawing.Size(100, 21);
this.chkIsOutSourced.TabIndex = 11;
this.Controls.Add(this.lblIsOutSourced);
this.Controls.Add(this.chkIsOutSourced);

           //#####50ShippingWay###String
this.lblShippingWay.AutoSize = true;
this.lblShippingWay.Location = new System.Drawing.Point(100,300);
this.lblShippingWay.Name = "lblShippingWay";
this.lblShippingWay.Size = new System.Drawing.Size(41, 12);
this.lblShippingWay.TabIndex = 12;
this.lblShippingWay.Text = "发货方式";
this.txtShippingWay.Location = new System.Drawing.Point(173,296);
this.txtShippingWay.Name = "txtShippingWay";
this.txtShippingWay.Size = new System.Drawing.Size(100, 21);
this.txtShippingWay.TabIndex = 12;
this.Controls.Add(this.lblShippingWay);
this.Controls.Add(this.txtShippingWay);

           //#####50TrackNo###String
this.lblTrackNo.AutoSize = true;
this.lblTrackNo.Location = new System.Drawing.Point(100,325);
this.lblTrackNo.Name = "lblTrackNo";
this.lblTrackNo.Size = new System.Drawing.Size(41, 12);
this.lblTrackNo.TabIndex = 13;
this.lblTrackNo.Text = "物流单号";
this.txtTrackNo.Location = new System.Drawing.Point(173,321);
this.txtTrackNo.Name = "txtTrackNo";
this.txtTrackNo.Size = new System.Drawing.Size(100, 21);
this.txtTrackNo.TabIndex = 13;
this.Controls.Add(this.lblTrackNo);
this.Controls.Add(this.txtTrackNo);

           //#####50MONo###String
this.lblMONo.AutoSize = true;
this.lblMONo.Location = new System.Drawing.Point(100,350);
this.lblMONo.Name = "lblMONo";
this.lblMONo.Size = new System.Drawing.Size(41, 12);
this.lblMONo.TabIndex = 14;
this.lblMONo.Text = "制令单号";
this.txtMONo.Location = new System.Drawing.Point(173,346);
this.txtMONo.Name = "txtMONo";
this.txtMONo.Size = new System.Drawing.Size(100, 21);
this.txtMONo.TabIndex = 14;
this.Controls.Add(this.lblMONo);
this.Controls.Add(this.txtMONo);

           //#####MOID###Int64
//属性测试375MOID
this.lblMOID.AutoSize = true;
this.lblMOID.Location = new System.Drawing.Point(100,375);
this.lblMOID.Name = "lblMOID";
this.lblMOID.Size = new System.Drawing.Size(41, 12);
this.lblMOID.TabIndex = 15;
this.lblMOID.Text = "制令单";
//111======375
this.cmbMOID.Location = new System.Drawing.Point(173,371);
this.cmbMOID.Name ="cmbMOID";
this.cmbMOID.Size = new System.Drawing.Size(100, 21);
this.cmbMOID.TabIndex = 15;
this.Controls.Add(this.lblMOID);
this.Controls.Add(this.cmbMOID);

           //#####TotalQty###Int32
//属性测试400TotalQty
//属性测试400TotalQty
//属性测试400TotalQty
//属性测试400TotalQty

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
//属性测试450DataStatus
//属性测试450DataStatus

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

           //#####Approver_by###Int64
//属性测试500Approver_by
//属性测试500Approver_by
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

           //#####TotalNetMachineHours###Decimal
this.lblTotalNetMachineHours.AutoSize = true;
this.lblTotalNetMachineHours.Location = new System.Drawing.Point(100,625);
this.lblTotalNetMachineHours.Name = "lblTotalNetMachineHours";
this.lblTotalNetMachineHours.Size = new System.Drawing.Size(41, 12);
this.lblTotalNetMachineHours.TabIndex = 25;
this.lblTotalNetMachineHours.Text = "总机时";
//111======625
this.txtTotalNetMachineHours.Location = new System.Drawing.Point(173,621);
this.txtTotalNetMachineHours.Name ="txtTotalNetMachineHours";
this.txtTotalNetMachineHours.Size = new System.Drawing.Size(100, 21);
this.txtTotalNetMachineHours.TabIndex = 25;
this.Controls.Add(this.lblTotalNetMachineHours);
this.Controls.Add(this.txtTotalNetMachineHours);

           //#####TotalNetWorkingHours###Decimal
this.lblTotalNetWorkingHours.AutoSize = true;
this.lblTotalNetWorkingHours.Location = new System.Drawing.Point(100,650);
this.lblTotalNetWorkingHours.Name = "lblTotalNetWorkingHours";
this.lblTotalNetWorkingHours.Size = new System.Drawing.Size(41, 12);
this.lblTotalNetWorkingHours.TabIndex = 26;
this.lblTotalNetWorkingHours.Text = "总工时";
//111======650
this.txtTotalNetWorkingHours.Location = new System.Drawing.Point(173,646);
this.txtTotalNetWorkingHours.Name ="txtTotalNetWorkingHours";
this.txtTotalNetWorkingHours.Size = new System.Drawing.Size(100, 21);
this.txtTotalNetWorkingHours.TabIndex = 26;
this.Controls.Add(this.lblTotalNetWorkingHours);
this.Controls.Add(this.txtTotalNetWorkingHours);

           //#####TotalApportionedCost###Decimal
this.lblTotalApportionedCost.AutoSize = true;
this.lblTotalApportionedCost.Location = new System.Drawing.Point(100,675);
this.lblTotalApportionedCost.Name = "lblTotalApportionedCost";
this.lblTotalApportionedCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalApportionedCost.TabIndex = 27;
this.lblTotalApportionedCost.Text = "总分摊成本";
//111======675
this.txtTotalApportionedCost.Location = new System.Drawing.Point(173,671);
this.txtTotalApportionedCost.Name ="txtTotalApportionedCost";
this.txtTotalApportionedCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalApportionedCost.TabIndex = 27;
this.Controls.Add(this.lblTotalApportionedCost);
this.Controls.Add(this.txtTotalApportionedCost);

           //#####TotalManuFee###Decimal
this.lblTotalManuFee.AutoSize = true;
this.lblTotalManuFee.Location = new System.Drawing.Point(100,700);
this.lblTotalManuFee.Name = "lblTotalManuFee";
this.lblTotalManuFee.Size = new System.Drawing.Size(41, 12);
this.lblTotalManuFee.TabIndex = 28;
this.lblTotalManuFee.Text = "总制造费用";
//111======700
this.txtTotalManuFee.Location = new System.Drawing.Point(173,696);
this.txtTotalManuFee.Name ="txtTotalManuFee";
this.txtTotalManuFee.Size = new System.Drawing.Size(100, 21);
this.txtTotalManuFee.TabIndex = 28;
this.Controls.Add(this.lblTotalManuFee);
this.Controls.Add(this.txtTotalManuFee);

           //#####TotalProductionCost###Decimal
this.lblTotalProductionCost.AutoSize = true;
this.lblTotalProductionCost.Location = new System.Drawing.Point(100,725);
this.lblTotalProductionCost.Name = "lblTotalProductionCost";
this.lblTotalProductionCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalProductionCost.TabIndex = 29;
this.lblTotalProductionCost.Text = "生产总成本";
//111======725
this.txtTotalProductionCost.Location = new System.Drawing.Point(173,721);
this.txtTotalProductionCost.Name ="txtTotalProductionCost";
this.txtTotalProductionCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalProductionCost.TabIndex = 29;
this.Controls.Add(this.lblTotalProductionCost);
this.Controls.Add(this.txtTotalProductionCost);

           //#####TotalMaterialCost###Decimal
this.lblTotalMaterialCost.AutoSize = true;
this.lblTotalMaterialCost.Location = new System.Drawing.Point(100,750);
this.lblTotalMaterialCost.Name = "lblTotalMaterialCost";
this.lblTotalMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialCost.TabIndex = 30;
this.lblTotalMaterialCost.Text = "总材料成本";
//111======750
this.txtTotalMaterialCost.Location = new System.Drawing.Point(173,746);
this.txtTotalMaterialCost.Name ="txtTotalMaterialCost";
this.txtTotalMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialCost.TabIndex = 30;
this.Controls.Add(this.lblTotalMaterialCost);
this.Controls.Add(this.txtTotalMaterialCost);

           //#####PrintStatus###Int32
//属性测试775PrintStatus
//属性测试775PrintStatus
//属性测试775PrintStatus
//属性测试775PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblDeliveryBillNo );
this.Controls.Add(this.txtDeliveryBillNo );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblDeliveryDate );
this.Controls.Add(this.dtpDeliveryDate );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIsOutSourced );
this.Controls.Add(this.chkIsOutSourced );

                this.Controls.Add(this.lblShippingWay );
this.Controls.Add(this.txtShippingWay );

                this.Controls.Add(this.lblTrackNo );
this.Controls.Add(this.txtTrackNo );

                this.Controls.Add(this.lblMONo );
this.Controls.Add(this.txtMONo );

                this.Controls.Add(this.lblMOID );
this.Controls.Add(this.cmbMOID );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblGeneEvidence );
this.Controls.Add(this.chkGeneEvidence );

                this.Controls.Add(this.lblTotalNetMachineHours );
this.Controls.Add(this.txtTotalNetMachineHours );

                this.Controls.Add(this.lblTotalNetWorkingHours );
this.Controls.Add(this.txtTotalNetWorkingHours );

                this.Controls.Add(this.lblTotalApportionedCost );
this.Controls.Add(this.txtTotalApportionedCost );

                this.Controls.Add(this.lblTotalManuFee );
this.Controls.Add(this.txtTotalManuFee );

                this.Controls.Add(this.lblTotalProductionCost );
this.Controls.Add(this.txtTotalProductionCost );

                this.Controls.Add(this.lblTotalMaterialCost );
this.Controls.Add(this.txtTotalMaterialCost );

                
                    
            this.Name = "tb_FinishedGoodsInvQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeliveryBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDeliveryBillNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDeliveryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsOutSourced;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsOutSourced;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingWay;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingWay;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTrackNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTrackNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMONo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMONo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMOID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMOID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGeneEvidence;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGeneEvidence;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalNetMachineHours;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalNetMachineHours;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalNetWorkingHours;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalNetWorkingHours;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalApportionedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalApportionedCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalManuFee;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalManuFee;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalProductionCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalProductionCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalMaterialCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalMaterialCost;

    
        
              
    
    
   
 





    }
}


