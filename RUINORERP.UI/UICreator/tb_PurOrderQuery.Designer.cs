
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:07
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据
    /// </summary>
    partial class tb_PurOrderQuery
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
     
     this.lblPurOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSOrder_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbSOrder_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPDID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPDID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPurDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPurDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblPreDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPreDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblForeignShipCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignShipCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblShipCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShipCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblActualAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtActualAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblArrival_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpArrival_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";


this.lblPrePay = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkPrePay = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkPrePay.Values.Text ="";

this.lblPrePayMoney = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrePayMoney = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsCustomizedOrder = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsCustomizedOrder = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsCustomizedOrder.Values.Text ="";

this.lblAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignDeposit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignDeposit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDeposit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDeposit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblRefNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRefNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100PurOrderNo###String
this.lblPurOrderNo.AutoSize = true;
this.lblPurOrderNo.Location = new System.Drawing.Point(100,25);
this.lblPurOrderNo.Name = "lblPurOrderNo";
this.lblPurOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblPurOrderNo.TabIndex = 1;
this.lblPurOrderNo.Text = "采购单号";
this.txtPurOrderNo.Location = new System.Drawing.Point(173,21);
this.txtPurOrderNo.Name = "txtPurOrderNo";
this.txtPurOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtPurOrderNo.TabIndex = 1;
this.Controls.Add(this.lblPurOrderNo);
this.Controls.Add(this.txtPurOrderNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "厂商";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####Employee_ID###Int64
//属性测试75Employee_ID
//属性测试75Employee_ID
//属性测试75Employee_ID
//属性测试75Employee_ID
//属性测试75Employee_ID
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
//属性测试100DepartmentID
//属性测试100DepartmentID
//属性测试100DepartmentID
//属性测试100DepartmentID
//属性测试100DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,100);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 4;
this.lblDepartmentID.Text = "使用部门";
//111======100
this.cmbDepartmentID.Location = new System.Drawing.Point(173,96);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 4;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ProjectGroup_ID###Int64
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
//属性测试125ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,125);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 5;
this.lblProjectGroup_ID.Text = "项目组";
//111======125
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,121);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 5;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####PayStatus###Int32
//属性测试150PayStatus
//属性测试150PayStatus
//属性测试150PayStatus
//属性测试150PayStatus
//属性测试150PayStatus
//属性测试150PayStatus
//属性测试150PayStatus
//属性测试150PayStatus
//属性测试150PayStatus
//属性测试150PayStatus

           //#####Paytype_ID###Int64
//属性测试175Paytype_ID
//属性测试175Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,175);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 7;
this.lblPaytype_ID.Text = "交易方式";
//111======175
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,171);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 7;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####PayeeInfoID###Int64
//属性测试200PayeeInfoID
//属性测试200PayeeInfoID
//属性测试200PayeeInfoID
//属性测试200PayeeInfoID
//属性测试200PayeeInfoID
//属性测试200PayeeInfoID
this.lblPayeeInfoID.AutoSize = true;
this.lblPayeeInfoID.Location = new System.Drawing.Point(100,200);
this.lblPayeeInfoID.Name = "lblPayeeInfoID";
this.lblPayeeInfoID.Size = new System.Drawing.Size(41, 12);
this.lblPayeeInfoID.TabIndex = 8;
this.lblPayeeInfoID.Text = "收款信息";
//111======200
this.cmbPayeeInfoID.Location = new System.Drawing.Point(173,196);
this.cmbPayeeInfoID.Name ="cmbPayeeInfoID";
this.cmbPayeeInfoID.Size = new System.Drawing.Size(100, 21);
this.cmbPayeeInfoID.TabIndex = 8;
this.Controls.Add(this.lblPayeeInfoID);
this.Controls.Add(this.cmbPayeeInfoID);

           //#####SOrder_ID###Int64
//属性测试225SOrder_ID
//属性测试225SOrder_ID
//属性测试225SOrder_ID
this.lblSOrder_ID.AutoSize = true;
this.lblSOrder_ID.Location = new System.Drawing.Point(100,225);
this.lblSOrder_ID.Name = "lblSOrder_ID";
this.lblSOrder_ID.Size = new System.Drawing.Size(41, 12);
this.lblSOrder_ID.TabIndex = 9;
this.lblSOrder_ID.Text = "销售订单";
//111======225
this.cmbSOrder_ID.Location = new System.Drawing.Point(173,221);
this.cmbSOrder_ID.Name ="cmbSOrder_ID";
this.cmbSOrder_ID.Size = new System.Drawing.Size(100, 21);
this.cmbSOrder_ID.TabIndex = 9;
this.Controls.Add(this.lblSOrder_ID);
this.Controls.Add(this.cmbSOrder_ID);

           //#####50SOrderNo###String
this.lblSOrderNo.AutoSize = true;
this.lblSOrderNo.Location = new System.Drawing.Point(100,250);
this.lblSOrderNo.Name = "lblSOrderNo";
this.lblSOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblSOrderNo.TabIndex = 10;
this.lblSOrderNo.Text = "订单编号";
this.txtSOrderNo.Location = new System.Drawing.Point(173,246);
this.txtSOrderNo.Name = "txtSOrderNo";
this.txtSOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtSOrderNo.TabIndex = 10;
this.Controls.Add(this.lblSOrderNo);
this.Controls.Add(this.txtSOrderNo);

           //#####PDID###Int64
//属性测试275PDID
//属性测试275PDID
//属性测试275PDID
//属性测试275PDID
//属性测试275PDID
//属性测试275PDID
//属性测试275PDID
this.lblPDID.AutoSize = true;
this.lblPDID.Location = new System.Drawing.Point(100,275);
this.lblPDID.Name = "lblPDID";
this.lblPDID.Size = new System.Drawing.Size(41, 12);
this.lblPDID.TabIndex = 11;
this.lblPDID.Text = "生产需求";
//111======275
this.cmbPDID.Location = new System.Drawing.Point(173,271);
this.cmbPDID.Name ="cmbPDID";
this.cmbPDID.Size = new System.Drawing.Size(100, 21);
this.cmbPDID.TabIndex = 11;
this.Controls.Add(this.lblPDID);
this.Controls.Add(this.cmbPDID);

           //#####PurDate###DateTime
this.lblPurDate.AutoSize = true;
this.lblPurDate.Location = new System.Drawing.Point(100,300);
this.lblPurDate.Name = "lblPurDate";
this.lblPurDate.Size = new System.Drawing.Size(41, 12);
this.lblPurDate.TabIndex = 12;
this.lblPurDate.Text = "采购日期";
//111======300
this.dtpPurDate.Location = new System.Drawing.Point(173,296);
this.dtpPurDate.Name ="dtpPurDate";
this.dtpPurDate.Size = new System.Drawing.Size(100, 21);
this.dtpPurDate.TabIndex = 12;
this.Controls.Add(this.lblPurDate);
this.Controls.Add(this.dtpPurDate);

           //#####PreDeliveryDate###DateTime
this.lblPreDeliveryDate.AutoSize = true;
this.lblPreDeliveryDate.Location = new System.Drawing.Point(100,325);
this.lblPreDeliveryDate.Name = "lblPreDeliveryDate";
this.lblPreDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblPreDeliveryDate.TabIndex = 13;
this.lblPreDeliveryDate.Text = "预交日期";
//111======325
this.dtpPreDeliveryDate.Location = new System.Drawing.Point(173,321);
this.dtpPreDeliveryDate.Name ="dtpPreDeliveryDate";
this.dtpPreDeliveryDate.ShowCheckBox =true;
this.dtpPreDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreDeliveryDate.TabIndex = 13;
this.Controls.Add(this.lblPreDeliveryDate);
this.Controls.Add(this.dtpPreDeliveryDate);

           //#####TotalQty###Int32
//属性测试350TotalQty
//属性测试350TotalQty
//属性测试350TotalQty
//属性测试350TotalQty
//属性测试350TotalQty
//属性测试350TotalQty
//属性测试350TotalQty
//属性测试350TotalQty
//属性测试350TotalQty
//属性测试350TotalQty

           //#####TotalUndeliveredQty###Int32
//属性测试375TotalUndeliveredQty
//属性测试375TotalUndeliveredQty
//属性测试375TotalUndeliveredQty
//属性测试375TotalUndeliveredQty
//属性测试375TotalUndeliveredQty
//属性测试375TotalUndeliveredQty
//属性测试375TotalUndeliveredQty
//属性测试375TotalUndeliveredQty
//属性测试375TotalUndeliveredQty
//属性测试375TotalUndeliveredQty

           //#####ForeignShipCost###Decimal
this.lblForeignShipCost.AutoSize = true;
this.lblForeignShipCost.Location = new System.Drawing.Point(100,400);
this.lblForeignShipCost.Name = "lblForeignShipCost";
this.lblForeignShipCost.Size = new System.Drawing.Size(41, 12);
this.lblForeignShipCost.TabIndex = 16;
this.lblForeignShipCost.Text = "运费外币";
//111======400
this.txtForeignShipCost.Location = new System.Drawing.Point(173,396);
this.txtForeignShipCost.Name ="txtForeignShipCost";
this.txtForeignShipCost.Size = new System.Drawing.Size(100, 21);
this.txtForeignShipCost.TabIndex = 16;
this.Controls.Add(this.lblForeignShipCost);
this.Controls.Add(this.txtForeignShipCost);

           //#####ShipCost###Decimal
this.lblShipCost.AutoSize = true;
this.lblShipCost.Location = new System.Drawing.Point(100,425);
this.lblShipCost.Name = "lblShipCost";
this.lblShipCost.Size = new System.Drawing.Size(41, 12);
this.lblShipCost.TabIndex = 17;
this.lblShipCost.Text = "运费本币";
//111======425
this.txtShipCost.Location = new System.Drawing.Point(173,421);
this.txtShipCost.Name ="txtShipCost";
this.txtShipCost.Size = new System.Drawing.Size(100, 21);
this.txtShipCost.TabIndex = 17;
this.Controls.Add(this.lblShipCost);
this.Controls.Add(this.txtShipCost);

           //#####TotalTaxAmount###Decimal
this.lblTotalTaxAmount.AutoSize = true;
this.lblTotalTaxAmount.Location = new System.Drawing.Point(100,450);
this.lblTotalTaxAmount.Name = "lblTotalTaxAmount";
this.lblTotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalTaxAmount.TabIndex = 18;
this.lblTotalTaxAmount.Text = "总税额";
//111======450
this.txtTotalTaxAmount.Location = new System.Drawing.Point(173,446);
this.txtTotalTaxAmount.Name ="txtTotalTaxAmount";
this.txtTotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalTaxAmount.TabIndex = 18;
this.Controls.Add(this.lblTotalTaxAmount);
this.Controls.Add(this.txtTotalTaxAmount);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,475);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 19;
this.lblTotalAmount.Text = "货款金额";
//111======475
this.txtTotalAmount.Location = new System.Drawing.Point(173,471);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 19;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####ActualAmount###Decimal
this.lblActualAmount.AutoSize = true;
this.lblActualAmount.Location = new System.Drawing.Point(100,500);
this.lblActualAmount.Name = "lblActualAmount";
this.lblActualAmount.Size = new System.Drawing.Size(41, 12);
this.lblActualAmount.TabIndex = 20;
this.lblActualAmount.Text = "实付金额";
//111======500
this.txtActualAmount.Location = new System.Drawing.Point(173,496);
this.txtActualAmount.Name ="txtActualAmount";
this.txtActualAmount.Size = new System.Drawing.Size(100, 21);
this.txtActualAmount.TabIndex = 20;
this.Controls.Add(this.lblActualAmount);
this.Controls.Add(this.txtActualAmount);

           //#####TotalUntaxedAmount###Decimal
this.lblTotalUntaxedAmount.AutoSize = true;
this.lblTotalUntaxedAmount.Location = new System.Drawing.Point(100,525);
this.lblTotalUntaxedAmount.Name = "lblTotalUntaxedAmount";
this.lblTotalUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalUntaxedAmount.TabIndex = 21;
this.lblTotalUntaxedAmount.Text = "未税总金额";
//111======525
this.txtTotalUntaxedAmount.Location = new System.Drawing.Point(173,521);
this.txtTotalUntaxedAmount.Name ="txtTotalUntaxedAmount";
this.txtTotalUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalUntaxedAmount.TabIndex = 21;
this.Controls.Add(this.lblTotalUntaxedAmount);
this.Controls.Add(this.txtTotalUntaxedAmount);

           //#####Arrival_date###DateTime
this.lblArrival_date.AutoSize = true;
this.lblArrival_date.Location = new System.Drawing.Point(100,550);
this.lblArrival_date.Name = "lblArrival_date";
this.lblArrival_date.Size = new System.Drawing.Size(41, 12);
this.lblArrival_date.TabIndex = 22;
this.lblArrival_date.Text = "到货日期";
//111======550
this.dtpArrival_date.Location = new System.Drawing.Point(173,546);
this.dtpArrival_date.Name ="dtpArrival_date";
this.dtpArrival_date.ShowCheckBox =true;
this.dtpArrival_date.Size = new System.Drawing.Size(100, 21);
this.dtpArrival_date.TabIndex = 22;
this.Controls.Add(this.lblArrival_date);
this.Controls.Add(this.dtpArrival_date);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,575);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 23;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,571);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 23;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####KeepAccountsType###Int32
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType
//属性测试600KeepAccountsType

           //#####PrePay###Boolean
this.lblPrePay.AutoSize = true;
this.lblPrePay.Location = new System.Drawing.Point(100,625);
this.lblPrePay.Name = "lblPrePay";
this.lblPrePay.Size = new System.Drawing.Size(41, 12);
this.lblPrePay.TabIndex = 25;
this.lblPrePay.Text = "预付款";
this.chkPrePay.Location = new System.Drawing.Point(173,621);
this.chkPrePay.Name = "chkPrePay";
this.chkPrePay.Size = new System.Drawing.Size(100, 21);
this.chkPrePay.TabIndex = 25;
this.Controls.Add(this.lblPrePay);
this.Controls.Add(this.chkPrePay);

           //#####PrePayMoney###Decimal
this.lblPrePayMoney.AutoSize = true;
this.lblPrePayMoney.Location = new System.Drawing.Point(100,650);
this.lblPrePayMoney.Name = "lblPrePayMoney";
this.lblPrePayMoney.Size = new System.Drawing.Size(41, 12);
this.lblPrePayMoney.TabIndex = 26;
this.lblPrePayMoney.Text = "预付";
//111======650
this.txtPrePayMoney.Location = new System.Drawing.Point(173,646);
this.txtPrePayMoney.Name ="txtPrePayMoney";
this.txtPrePayMoney.Size = new System.Drawing.Size(100, 21);
this.txtPrePayMoney.TabIndex = 26;
this.Controls.Add(this.lblPrePayMoney);
this.Controls.Add(this.txtPrePayMoney);

           //#####IsCustomizedOrder###Boolean
this.lblIsCustomizedOrder.AutoSize = true;
this.lblIsCustomizedOrder.Location = new System.Drawing.Point(100,675);
this.lblIsCustomizedOrder.Name = "lblIsCustomizedOrder";
this.lblIsCustomizedOrder.Size = new System.Drawing.Size(41, 12);
this.lblIsCustomizedOrder.TabIndex = 27;
this.lblIsCustomizedOrder.Text = "定制单";
this.chkIsCustomizedOrder.Location = new System.Drawing.Point(173,671);
this.chkIsCustomizedOrder.Name = "chkIsCustomizedOrder";
this.chkIsCustomizedOrder.Size = new System.Drawing.Size(100, 21);
this.chkIsCustomizedOrder.TabIndex = 27;
this.Controls.Add(this.lblIsCustomizedOrder);
this.Controls.Add(this.chkIsCustomizedOrder);

           //#####Account_id###Int64
//属性测试700Account_id
//属性测试700Account_id
//属性测试700Account_id
//属性测试700Account_id
//属性测试700Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,700);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 28;
this.lblAccount_id.Text = "收款账户";
//111======700
this.cmbAccount_id.Location = new System.Drawing.Point(173,696);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 28;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####Currency_ID###Int64
//属性测试725Currency_ID
//属性测试725Currency_ID
//属性测试725Currency_ID
//属性测试725Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,725);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 29;
this.lblCurrency_ID.Text = "币别";
//111======725
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,721);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 29;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,750);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 30;
this.lblExchangeRate.Text = "汇率";
//111======750
this.txtExchangeRate.Location = new System.Drawing.Point(173,746);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 30;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####ForeignTotalAmount###Decimal
this.lblForeignTotalAmount.AutoSize = true;
this.lblForeignTotalAmount.Location = new System.Drawing.Point(100,775);
this.lblForeignTotalAmount.Name = "lblForeignTotalAmount";
this.lblForeignTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignTotalAmount.TabIndex = 31;
this.lblForeignTotalAmount.Text = "金额外币";
//111======775
this.txtForeignTotalAmount.Location = new System.Drawing.Point(173,771);
this.txtForeignTotalAmount.Name ="txtForeignTotalAmount";
this.txtForeignTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignTotalAmount.TabIndex = 31;
this.Controls.Add(this.lblForeignTotalAmount);
this.Controls.Add(this.txtForeignTotalAmount);

           //#####ForeignDeposit###Decimal
this.lblForeignDeposit.AutoSize = true;
this.lblForeignDeposit.Location = new System.Drawing.Point(100,800);
this.lblForeignDeposit.Name = "lblForeignDeposit";
this.lblForeignDeposit.Size = new System.Drawing.Size(41, 12);
this.lblForeignDeposit.TabIndex = 32;
this.lblForeignDeposit.Text = "订金外币";
//111======800
this.txtForeignDeposit.Location = new System.Drawing.Point(173,796);
this.txtForeignDeposit.Name ="txtForeignDeposit";
this.txtForeignDeposit.Size = new System.Drawing.Size(100, 21);
this.txtForeignDeposit.TabIndex = 32;
this.Controls.Add(this.lblForeignDeposit);
this.Controls.Add(this.txtForeignDeposit);

           //#####Deposit###Decimal
this.lblDeposit.AutoSize = true;
this.lblDeposit.Location = new System.Drawing.Point(100,825);
this.lblDeposit.Name = "lblDeposit";
this.lblDeposit.Size = new System.Drawing.Size(41, 12);
this.lblDeposit.TabIndex = 33;
this.lblDeposit.Text = "订金";
//111======825
this.txtDeposit.Location = new System.Drawing.Point(173,821);
this.txtDeposit.Name ="txtDeposit";
this.txtDeposit.Size = new System.Drawing.Size(100, 21);
this.txtDeposit.TabIndex = 33;
this.Controls.Add(this.lblDeposit);
this.Controls.Add(this.txtDeposit);

           //#####TaxDeductionType###Int32
//属性测试850TaxDeductionType
//属性测试850TaxDeductionType
//属性测试850TaxDeductionType
//属性测试850TaxDeductionType
//属性测试850TaxDeductionType
//属性测试850TaxDeductionType
//属性测试850TaxDeductionType
//属性测试850TaxDeductionType
//属性测试850TaxDeductionType
//属性测试850TaxDeductionType

           //#####DataStatus###Int32
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,900);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 36;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,896);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 36;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,925);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 37;
this.lblCloseCaseOpinions.Text = "结案意见";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,921);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 37;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,950);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 38;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,946);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 38;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,1000);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 40;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,996);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 40;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####Approver_by###Int64
//属性测试1025Approver_by
//属性测试1025Approver_by
//属性测试1025Approver_by
//属性测试1025Approver_by
//属性测试1025Approver_by
//属性测试1025Approver_by
//属性测试1025Approver_by
//属性测试1025Approver_by
//属性测试1025Approver_by
//属性测试1025Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,1050);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 42;
this.lblApprover_at.Text = "审批时间";
//111======1050
this.dtpApprover_at.Location = new System.Drawing.Point(173,1046);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 42;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####PrintStatus###Int32
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,1100);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 44;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,1096);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 44;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,1125);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 45;
this.lblCreated_at.Text = "创建时间";
//111======1125
this.dtpCreated_at.Location = new System.Drawing.Point(173,1121);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 45;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试1150Created_by
//属性测试1150Created_by
//属性测试1150Created_by
//属性测试1150Created_by
//属性测试1150Created_by
//属性测试1150Created_by
//属性测试1150Created_by
//属性测试1150Created_by
//属性测试1150Created_by
//属性测试1150Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,1175);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 47;
this.lblModified_at.Text = "修改时间";
//111======1175
this.dtpModified_at.Location = new System.Drawing.Point(173,1171);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 47;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试1200Modified_by
//属性测试1200Modified_by
//属性测试1200Modified_by
//属性测试1200Modified_by
//属性测试1200Modified_by
//属性测试1200Modified_by
//属性测试1200Modified_by
//属性测试1200Modified_by
//属性测试1200Modified_by
//属性测试1200Modified_by

           //#####RefBillID###Int64
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID

           //#####50RefNO###String
this.lblRefNO.AutoSize = true;
this.lblRefNO.Location = new System.Drawing.Point(100,1250);
this.lblRefNO.Name = "lblRefNO";
this.lblRefNO.Size = new System.Drawing.Size(41, 12);
this.lblRefNO.TabIndex = 50;
this.lblRefNO.Text = "引用单据";
this.txtRefNO.Location = new System.Drawing.Point(173,1246);
this.txtRefNO.Name = "txtRefNO";
this.txtRefNO.Size = new System.Drawing.Size(100, 21);
this.txtRefNO.TabIndex = 50;
this.Controls.Add(this.lblRefNO);
this.Controls.Add(this.txtRefNO);

           //#####RefBizType###Int32
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPurOrderNo );
this.Controls.Add(this.txtPurOrderNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                
                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                this.Controls.Add(this.lblPayeeInfoID );
this.Controls.Add(this.cmbPayeeInfoID );

                this.Controls.Add(this.lblSOrder_ID );
this.Controls.Add(this.cmbSOrder_ID );

                this.Controls.Add(this.lblSOrderNo );
this.Controls.Add(this.txtSOrderNo );

                this.Controls.Add(this.lblPDID );
this.Controls.Add(this.cmbPDID );

                this.Controls.Add(this.lblPurDate );
this.Controls.Add(this.dtpPurDate );

                this.Controls.Add(this.lblPreDeliveryDate );
this.Controls.Add(this.dtpPreDeliveryDate );

                
                
                this.Controls.Add(this.lblForeignShipCost );
this.Controls.Add(this.txtForeignShipCost );

                this.Controls.Add(this.lblShipCost );
this.Controls.Add(this.txtShipCost );

                this.Controls.Add(this.lblTotalTaxAmount );
this.Controls.Add(this.txtTotalTaxAmount );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblActualAmount );
this.Controls.Add(this.txtActualAmount );

                this.Controls.Add(this.lblTotalUntaxedAmount );
this.Controls.Add(this.txtTotalUntaxedAmount );

                this.Controls.Add(this.lblArrival_date );
this.Controls.Add(this.dtpArrival_date );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                
                this.Controls.Add(this.lblPrePay );
this.Controls.Add(this.chkPrePay );

                this.Controls.Add(this.lblPrePayMoney );
this.Controls.Add(this.txtPrePayMoney );

                this.Controls.Add(this.lblIsCustomizedOrder );
this.Controls.Add(this.chkIsCustomizedOrder );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblForeignTotalAmount );
this.Controls.Add(this.txtForeignTotalAmount );

                this.Controls.Add(this.lblForeignDeposit );
this.Controls.Add(this.txtForeignDeposit );

                this.Controls.Add(this.lblDeposit );
this.Controls.Add(this.txtDeposit );

                
                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                
                this.Controls.Add(this.lblRefNO );
this.Controls.Add(this.txtRefNO );

                
                    
            this.Name = "tb_PurOrderQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurOrderNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSOrder_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSOrder_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSOrderNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPDID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPDID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPurDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPreDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPreDeliveryDate;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignShipCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignShipCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShipCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShipCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblActualAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtActualAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalUntaxedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalUntaxedAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblArrival_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpArrival_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrePay;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkPrePay;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrePayMoney;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrePayMoney;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsCustomizedOrder;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsCustomizedOrder;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignDeposit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignDeposit;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeposit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDeposit;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRefNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRefNO;

    
        
              
    
    
   
 





    }
}


