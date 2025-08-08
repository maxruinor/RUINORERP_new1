// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:41
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 领料单(包括生产和托工)
    /// </summary>
    partial class tb_MaterialRequisitionEdit
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
     this.lblMaterialRequisitionNO = new Krypton.Toolkit.KryptonLabel();
this.txtMaterialRequisitionNO = new Krypton.Toolkit.KryptonTextBox();

this.lblMONO = new Krypton.Toolkit.KryptonLabel();
this.txtMONO = new Krypton.Toolkit.KryptonTextBox();

this.lblDeliveryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpDeliveryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.txtDepartmentID = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblMOID = new Krypton.Toolkit.KryptonLabel();
this.cmbMOID = new Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.txtProjectGroup_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblShippingAddress = new Krypton.Toolkit.KryptonLabel();
this.txtShippingAddress = new Krypton.Toolkit.KryptonTextBox();
this.txtShippingAddress.Multiline = true;

this.lblshippingWay = new Krypton.Toolkit.KryptonLabel();
this.txtshippingWay = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalPrice = new Krypton.Toolkit.KryptonLabel();
this.txtTotalPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalCost = new Krypton.Toolkit.KryptonLabel();
this.txtTotalCost = new Krypton.Toolkit.KryptonTextBox();

this.lblExpectedQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtExpectedQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalSendQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalSendQty = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalReQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalReQty = new Krypton.Toolkit.KryptonTextBox();

this.lblTrackNo = new Krypton.Toolkit.KryptonLabel();
this.txtTrackNo = new Krypton.Toolkit.KryptonTextBox();

this.lblShipCost = new Krypton.Toolkit.KryptonLabel();
this.txtShipCost = new Krypton.Toolkit.KryptonTextBox();

this.lblReApply = new Krypton.Toolkit.KryptonLabel();
this.chkReApply = new Krypton.Toolkit.KryptonCheckBox();
this.chkReApply.Values.Text ="";

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

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

this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblGeneEvidence = new Krypton.Toolkit.KryptonLabel();
this.chkGeneEvidence = new Krypton.Toolkit.KryptonCheckBox();
this.chkGeneEvidence.Values.Text ="";

this.lblOutgoing = new Krypton.Toolkit.KryptonLabel();
this.chkOutgoing = new Krypton.Toolkit.KryptonCheckBox();
this.chkOutgoing.Values.Text ="";

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
     
            //#####50MaterialRequisitionNO###String
this.lblMaterialRequisitionNO.AutoSize = true;
this.lblMaterialRequisitionNO.Location = new System.Drawing.Point(100,25);
this.lblMaterialRequisitionNO.Name = "lblMaterialRequisitionNO";
this.lblMaterialRequisitionNO.Size = new System.Drawing.Size(41, 12);
this.lblMaterialRequisitionNO.TabIndex = 1;
this.lblMaterialRequisitionNO.Text = "领料单号";
this.txtMaterialRequisitionNO.Location = new System.Drawing.Point(173,21);
this.txtMaterialRequisitionNO.Name = "txtMaterialRequisitionNO";
this.txtMaterialRequisitionNO.Size = new System.Drawing.Size(100, 21);
this.txtMaterialRequisitionNO.TabIndex = 1;
this.Controls.Add(this.lblMaterialRequisitionNO);
this.Controls.Add(this.txtMaterialRequisitionNO);

           //#####100MONO###String
this.lblMONO.AutoSize = true;
this.lblMONO.Location = new System.Drawing.Point(100,50);
this.lblMONO.Name = "lblMONO";
this.lblMONO.Size = new System.Drawing.Size(41, 12);
this.lblMONO.TabIndex = 2;
this.lblMONO.Text = "制令单号";
this.txtMONO.Location = new System.Drawing.Point(173,46);
this.txtMONO.Name = "txtMONO";
this.txtMONO.Size = new System.Drawing.Size(100, 21);
this.txtMONO.TabIndex = 2;
this.Controls.Add(this.lblMONO);
this.Controls.Add(this.txtMONO);

           //#####DeliveryDate###DateTime
this.lblDeliveryDate.AutoSize = true;
this.lblDeliveryDate.Location = new System.Drawing.Point(100,75);
this.lblDeliveryDate.Name = "lblDeliveryDate";
this.lblDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryDate.TabIndex = 3;
this.lblDeliveryDate.Text = "领取日期";
//111======75
this.dtpDeliveryDate.Location = new System.Drawing.Point(173,71);
this.dtpDeliveryDate.Name ="dtpDeliveryDate";
this.dtpDeliveryDate.ShowCheckBox =true;
this.dtpDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpDeliveryDate.TabIndex = 3;
this.Controls.Add(this.lblDeliveryDate);
this.Controls.Add(this.dtpDeliveryDate);

           //#####Location_ID###Int64
//属性测试100Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,100);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 4;
this.lblLocation_ID.Text = "经办人";
this.txtLocation_ID.Location = new System.Drawing.Point(173,96);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 4;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####Employee_ID###Int64
//属性测试125Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,125);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 5;
this.lblEmployee_ID.Text = "经办人";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,121);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 5;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####DepartmentID###Int64
//属性测试150DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,150);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 6;
this.lblDepartmentID.Text = "生产部门";
this.txtDepartmentID.Location = new System.Drawing.Point(173,146);
this.txtDepartmentID.Name = "txtDepartmentID";
this.txtDepartmentID.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentID.TabIndex = 6;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.txtDepartmentID);

           //#####CustomerVendor_ID###Int64
//属性测试175CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,175);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 7;
this.lblCustomerVendor_ID.Text = "外发厂商";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,171);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 7;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####MOID###Int64
//属性测试200MOID
this.lblMOID.AutoSize = true;
this.lblMOID.Location = new System.Drawing.Point(100,200);
this.lblMOID.Name = "lblMOID";
this.lblMOID.Size = new System.Drawing.Size(41, 12);
this.lblMOID.TabIndex = 8;
this.lblMOID.Text = "制令单";
//111======200
this.cmbMOID.Location = new System.Drawing.Point(173,196);
this.cmbMOID.Name ="cmbMOID";
this.cmbMOID.Size = new System.Drawing.Size(100, 21);
this.cmbMOID.TabIndex = 8;
this.Controls.Add(this.lblMOID);
this.Controls.Add(this.cmbMOID);

           //#####ProjectGroup_ID###Int64
//属性测试225ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,225);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 9;
this.lblProjectGroup_ID.Text = "项目组";
this.txtProjectGroup_ID.Location = new System.Drawing.Point(173,221);
this.txtProjectGroup_ID.Name = "txtProjectGroup_ID";
this.txtProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.txtProjectGroup_ID.TabIndex = 9;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.txtProjectGroup_ID);

           //#####255ShippingAddress###String
this.lblShippingAddress.AutoSize = true;
this.lblShippingAddress.Location = new System.Drawing.Point(100,250);
this.lblShippingAddress.Name = "lblShippingAddress";
this.lblShippingAddress.Size = new System.Drawing.Size(41, 12);
this.lblShippingAddress.TabIndex = 10;
this.lblShippingAddress.Text = "发货地址";
this.txtShippingAddress.Location = new System.Drawing.Point(173,246);
this.txtShippingAddress.Name = "txtShippingAddress";
this.txtShippingAddress.Size = new System.Drawing.Size(100, 21);
this.txtShippingAddress.TabIndex = 10;
this.Controls.Add(this.lblShippingAddress);
this.Controls.Add(this.txtShippingAddress);

           //#####50shippingWay###String
this.lblshippingWay.AutoSize = true;
this.lblshippingWay.Location = new System.Drawing.Point(100,275);
this.lblshippingWay.Name = "lblshippingWay";
this.lblshippingWay.Size = new System.Drawing.Size(41, 12);
this.lblshippingWay.TabIndex = 11;
this.lblshippingWay.Text = "发货方式";
this.txtshippingWay.Location = new System.Drawing.Point(173,271);
this.txtshippingWay.Name = "txtshippingWay";
this.txtshippingWay.Size = new System.Drawing.Size(100, 21);
this.txtshippingWay.TabIndex = 11;
this.Controls.Add(this.lblshippingWay);
this.Controls.Add(this.txtshippingWay);

           //#####TotalPrice###Decimal
this.lblTotalPrice.AutoSize = true;
this.lblTotalPrice.Location = new System.Drawing.Point(100,300);
this.lblTotalPrice.Name = "lblTotalPrice";
this.lblTotalPrice.Size = new System.Drawing.Size(41, 12);
this.lblTotalPrice.TabIndex = 12;
this.lblTotalPrice.Text = "总金额";
//111======300
this.txtTotalPrice.Location = new System.Drawing.Point(173,296);
this.txtTotalPrice.Name ="txtTotalPrice";
this.txtTotalPrice.Size = new System.Drawing.Size(100, 21);
this.txtTotalPrice.TabIndex = 12;
this.Controls.Add(this.lblTotalPrice);
this.Controls.Add(this.txtTotalPrice);

           //#####TotalCost###Decimal
this.lblTotalCost.AutoSize = true;
this.lblTotalCost.Location = new System.Drawing.Point(100,325);
this.lblTotalCost.Name = "lblTotalCost";
this.lblTotalCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalCost.TabIndex = 13;
this.lblTotalCost.Text = "总成本";
//111======325
this.txtTotalCost.Location = new System.Drawing.Point(173,321);
this.txtTotalCost.Name ="txtTotalCost";
this.txtTotalCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalCost.TabIndex = 13;
this.Controls.Add(this.lblTotalCost);
this.Controls.Add(this.txtTotalCost);

           //#####ExpectedQuantity###Int32
//属性测试350ExpectedQuantity
this.lblExpectedQuantity.AutoSize = true;
this.lblExpectedQuantity.Location = new System.Drawing.Point(100,350);
this.lblExpectedQuantity.Name = "lblExpectedQuantity";
this.lblExpectedQuantity.Size = new System.Drawing.Size(41, 12);
this.lblExpectedQuantity.TabIndex = 14;
this.lblExpectedQuantity.Text = "预计产量";
this.txtExpectedQuantity.Location = new System.Drawing.Point(173,346);
this.txtExpectedQuantity.Name = "txtExpectedQuantity";
this.txtExpectedQuantity.Size = new System.Drawing.Size(100, 21);
this.txtExpectedQuantity.TabIndex = 14;
this.Controls.Add(this.lblExpectedQuantity);
this.Controls.Add(this.txtExpectedQuantity);

           //#####TotalSendQty###Int32
//属性测试375TotalSendQty
this.lblTotalSendQty.AutoSize = true;
this.lblTotalSendQty.Location = new System.Drawing.Point(100,375);
this.lblTotalSendQty.Name = "lblTotalSendQty";
this.lblTotalSendQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalSendQty.TabIndex = 15;
this.lblTotalSendQty.Text = "实发总数";
this.txtTotalSendQty.Location = new System.Drawing.Point(173,371);
this.txtTotalSendQty.Name = "txtTotalSendQty";
this.txtTotalSendQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalSendQty.TabIndex = 15;
this.Controls.Add(this.lblTotalSendQty);
this.Controls.Add(this.txtTotalSendQty);

           //#####TotalReQty###Int32
//属性测试400TotalReQty
this.lblTotalReQty.AutoSize = true;
this.lblTotalReQty.Location = new System.Drawing.Point(100,400);
this.lblTotalReQty.Name = "lblTotalReQty";
this.lblTotalReQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalReQty.TabIndex = 16;
this.lblTotalReQty.Text = "退回总数";
this.txtTotalReQty.Location = new System.Drawing.Point(173,396);
this.txtTotalReQty.Name = "txtTotalReQty";
this.txtTotalReQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalReQty.TabIndex = 16;
this.Controls.Add(this.lblTotalReQty);
this.Controls.Add(this.txtTotalReQty);

           //#####50TrackNo###String
this.lblTrackNo.AutoSize = true;
this.lblTrackNo.Location = new System.Drawing.Point(100,425);
this.lblTrackNo.Name = "lblTrackNo";
this.lblTrackNo.Size = new System.Drawing.Size(41, 12);
this.lblTrackNo.TabIndex = 17;
this.lblTrackNo.Text = "物流单号";
this.txtTrackNo.Location = new System.Drawing.Point(173,421);
this.txtTrackNo.Name = "txtTrackNo";
this.txtTrackNo.Size = new System.Drawing.Size(100, 21);
this.txtTrackNo.TabIndex = 17;
this.Controls.Add(this.lblTrackNo);
this.Controls.Add(this.txtTrackNo);

           //#####ShipCost###Decimal
this.lblShipCost.AutoSize = true;
this.lblShipCost.Location = new System.Drawing.Point(100,450);
this.lblShipCost.Name = "lblShipCost";
this.lblShipCost.Size = new System.Drawing.Size(41, 12);
this.lblShipCost.TabIndex = 18;
this.lblShipCost.Text = "运费";
//111======450
this.txtShipCost.Location = new System.Drawing.Point(173,446);
this.txtShipCost.Name ="txtShipCost";
this.txtShipCost.Size = new System.Drawing.Size(100, 21);
this.txtShipCost.TabIndex = 18;
this.Controls.Add(this.lblShipCost);
this.Controls.Add(this.txtShipCost);

           //#####ReApply###Boolean
this.lblReApply.AutoSize = true;
this.lblReApply.Location = new System.Drawing.Point(100,475);
this.lblReApply.Name = "lblReApply";
this.lblReApply.Size = new System.Drawing.Size(41, 12);
this.lblReApply.TabIndex = 19;
this.lblReApply.Text = "是否补领";
this.chkReApply.Location = new System.Drawing.Point(173,471);
this.chkReApply.Name = "chkReApply";
this.chkReApply.Size = new System.Drawing.Size(100, 21);
this.chkReApply.TabIndex = 19;
this.Controls.Add(this.lblReApply);
this.Controls.Add(this.chkReApply);

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

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,525);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 21;
this.lblCreated_at.Text = "创建时间";
//111======525
this.dtpCreated_at.Location = new System.Drawing.Point(173,521);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 21;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试550Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,550);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 22;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,546);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 22;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,575);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 23;
this.lblModified_at.Text = "修改时间";
//111======575
this.dtpModified_at.Location = new System.Drawing.Point(173,571);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 23;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试600Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,600);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 24;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,596);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 24;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,625);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 25;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,621);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 25;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,650);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 26;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,646);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 26;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试675Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,675);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 27;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,671);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 27;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,700);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 28;
this.lblApprover_at.Text = "审批时间";
//111======700
this.dtpApprover_at.Location = new System.Drawing.Point(173,696);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 28;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,750);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 30;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,746);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 30;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####DataStatus###Int32
//属性测试775DataStatus
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,775);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 31;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,771);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 31;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####GeneEvidence###Boolean
this.lblGeneEvidence.AutoSize = true;
this.lblGeneEvidence.Location = new System.Drawing.Point(100,800);
this.lblGeneEvidence.Name = "lblGeneEvidence";
this.lblGeneEvidence.Size = new System.Drawing.Size(41, 12);
this.lblGeneEvidence.TabIndex = 32;
this.lblGeneEvidence.Text = "产生凭证";
this.chkGeneEvidence.Location = new System.Drawing.Point(173,796);
this.chkGeneEvidence.Name = "chkGeneEvidence";
this.chkGeneEvidence.Size = new System.Drawing.Size(100, 21);
this.chkGeneEvidence.TabIndex = 32;
this.Controls.Add(this.lblGeneEvidence);
this.Controls.Add(this.chkGeneEvidence);

           //#####Outgoing###Boolean
this.lblOutgoing.AutoSize = true;
this.lblOutgoing.Location = new System.Drawing.Point(100,825);
this.lblOutgoing.Name = "lblOutgoing";
this.lblOutgoing.Size = new System.Drawing.Size(41, 12);
this.lblOutgoing.TabIndex = 33;
this.lblOutgoing.Text = "外发加工";
this.chkOutgoing.Location = new System.Drawing.Point(173,821);
this.chkOutgoing.Name = "chkOutgoing";
this.chkOutgoing.Size = new System.Drawing.Size(100, 21);
this.chkOutgoing.TabIndex = 33;
this.Controls.Add(this.lblOutgoing);
this.Controls.Add(this.chkOutgoing);

           //#####PrintStatus###Int32
//属性测试850PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,850);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 34;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,846);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 34;
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
           // this.kryptonPanel1.TabIndex = 34;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMaterialRequisitionNO );
this.Controls.Add(this.txtMaterialRequisitionNO );

                this.Controls.Add(this.lblMONO );
this.Controls.Add(this.txtMONO );

                this.Controls.Add(this.lblDeliveryDate );
this.Controls.Add(this.dtpDeliveryDate );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.txtDepartmentID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblMOID );
this.Controls.Add(this.cmbMOID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.txtProjectGroup_ID );

                this.Controls.Add(this.lblShippingAddress );
this.Controls.Add(this.txtShippingAddress );

                this.Controls.Add(this.lblshippingWay );
this.Controls.Add(this.txtshippingWay );

                this.Controls.Add(this.lblTotalPrice );
this.Controls.Add(this.txtTotalPrice );

                this.Controls.Add(this.lblTotalCost );
this.Controls.Add(this.txtTotalCost );

                this.Controls.Add(this.lblExpectedQuantity );
this.Controls.Add(this.txtExpectedQuantity );

                this.Controls.Add(this.lblTotalSendQty );
this.Controls.Add(this.txtTotalSendQty );

                this.Controls.Add(this.lblTotalReQty );
this.Controls.Add(this.txtTotalReQty );

                this.Controls.Add(this.lblTrackNo );
this.Controls.Add(this.txtTrackNo );

                this.Controls.Add(this.lblShipCost );
this.Controls.Add(this.txtShipCost );

                this.Controls.Add(this.lblReApply );
this.Controls.Add(this.chkReApply );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

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

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblGeneEvidence );
this.Controls.Add(this.chkGeneEvidence );

                this.Controls.Add(this.lblOutgoing );
this.Controls.Add(this.chkOutgoing );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                            // 
            // "tb_MaterialRequisitionEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_MaterialRequisitionEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblMaterialRequisitionNO;
private Krypton.Toolkit.KryptonTextBox txtMaterialRequisitionNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblMONO;
private Krypton.Toolkit.KryptonTextBox txtMONO;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeliveryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpDeliveryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonTextBox txtDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblMOID;
private Krypton.Toolkit.KryptonComboBox cmbMOID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonTextBox txtProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblShippingAddress;
private Krypton.Toolkit.KryptonTextBox txtShippingAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblshippingWay;
private Krypton.Toolkit.KryptonTextBox txtshippingWay;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalPrice;
private Krypton.Toolkit.KryptonTextBox txtTotalPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalCost;
private Krypton.Toolkit.KryptonTextBox txtTotalCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpectedQuantity;
private Krypton.Toolkit.KryptonTextBox txtExpectedQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalSendQty;
private Krypton.Toolkit.KryptonTextBox txtTotalSendQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalReQty;
private Krypton.Toolkit.KryptonTextBox txtTotalReQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblTrackNo;
private Krypton.Toolkit.KryptonTextBox txtTrackNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblShipCost;
private Krypton.Toolkit.KryptonTextBox txtShipCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblReApply;
private Krypton.Toolkit.KryptonCheckBox chkReApply;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblGeneEvidence;
private Krypton.Toolkit.KryptonCheckBox chkGeneEvidence;

    
        
              private Krypton.Toolkit.KryptonLabel lblOutgoing;
private Krypton.Toolkit.KryptonCheckBox chkOutgoing;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

