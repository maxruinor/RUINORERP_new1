﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 18:05:10
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960
    /// </summary>
    partial class tb_ManufacturingOrderQuery
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
     
     this.lblMONO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMONO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPDNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPDNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

CustomerVendor_ID_Out主外字段不一致。this.lblPDCID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPDCID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

CustomerVendor_ID_Out主外字段不一致。this.lblPDID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPDID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

CustomerVendor_ID_Out主外字段不一致。
CustomerVendor_ID_Out主外字段不一致。
CustomerVendor_ID_Out主外字段不一致。
this.lblPreStartDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPreStartDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblPreEndDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPreEndDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

CustomerVendor_ID_Out主外字段不一致。this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

CustomerVendor_ID_Out主外字段不一致。this.lblBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBOM_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBOM_No = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBOM_No = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

CustomerVendor_ID_Out主外字段不一致。this.lblType_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbType_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

CustomerVendor_ID_Out主外字段不一致。this.lblUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

CustomerVendor_ID_Out主外字段不一致。this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

CustomerVendor_ID_Out主外字段不一致。this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

CustomerVendor_ID_Out主外字段不一致。this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

CustomerVendor_ID_Out主外字段不一致。
CustomerVendor_ID_Out主外字段不一致。this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

CustomerVendor_ID_Out主外字段不一致。
this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

CustomerVendor_ID_Out主外字段不一致。
this.lblCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalManuFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalManuFee = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalProductionCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalProductionCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPeopleQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPeopleQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWorkingHour = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWorkingHour = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMachineHour = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMachineHour = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIncludeSubBOM = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIncludeSubBOM = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIncludeSubBOM.Values.Text ="";

this.lblIsOutSourced = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsOutSourced = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsOutSourced.Values.Text ="";

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

CustomerVendor_ID_Out主外字段不一致。
this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblApproverTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApproverTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


CustomerVendor_ID_Out主外字段不一致。
this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

CustomerVendor_ID_Out主外字段不一致。
    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100MONO###String
this.lblMONO.AutoSize = true;
this.lblMONO.Location = new System.Drawing.Point(100,25);
this.lblMONO.Name = "lblMONO";
this.lblMONO.Size = new System.Drawing.Size(41, 12);
this.lblMONO.TabIndex = 1;
this.lblMONO.Text = "制令单号";
this.txtMONO.Location = new System.Drawing.Point(173,21);
this.txtMONO.Name = "txtMONO";
this.txtMONO.Size = new System.Drawing.Size(100, 21);
this.txtMONO.TabIndex = 1;
this.Controls.Add(this.lblMONO);
this.Controls.Add(this.txtMONO);

           //#####100PDNO###String
this.lblPDNO.AutoSize = true;
this.lblPDNO.Location = new System.Drawing.Point(100,50);
this.lblPDNO.Name = "lblPDNO";
this.lblPDNO.Size = new System.Drawing.Size(41, 12);
this.lblPDNO.TabIndex = 2;
this.lblPDNO.Text = "需求单号";
this.txtPDNO.Location = new System.Drawing.Point(173,46);
this.txtPDNO.Name = "txtPDNO";
this.txtPDNO.Size = new System.Drawing.Size(100, 21);
this.txtPDNO.TabIndex = 2;
this.Controls.Add(this.lblPDNO);
this.Controls.Add(this.txtPDNO);

           //#####PDCID###Int64
//属性测试75PDCID
CustomerVendor_ID_Out主外字段不一致。//属性测试75PDCID
//属性测试75PDCID
this.lblPDCID.AutoSize = true;
this.lblPDCID.Location = new System.Drawing.Point(100,75);
this.lblPDCID.Name = "lblPDCID";
this.lblPDCID.Size = new System.Drawing.Size(41, 12);
this.lblPDCID.TabIndex = 3;
this.lblPDCID.Text = "自制品";
//111======75
this.cmbPDCID.Location = new System.Drawing.Point(173,71);
this.cmbPDCID.Name ="cmbPDCID";
this.cmbPDCID.Size = new System.Drawing.Size(100, 21);
this.cmbPDCID.TabIndex = 3;
this.Controls.Add(this.lblPDCID);
this.Controls.Add(this.cmbPDCID);

           //#####PDID###Int64
//属性测试100PDID
CustomerVendor_ID_Out主外字段不一致。//属性测试100PDID
//属性测试100PDID
//属性测试100PDID
//属性测试100PDID
//属性测试100PDID
this.lblPDID.AutoSize = true;
this.lblPDID.Location = new System.Drawing.Point(100,100);
this.lblPDID.Name = "lblPDID";
this.lblPDID.Size = new System.Drawing.Size(41, 12);
this.lblPDID.TabIndex = 4;
this.lblPDID.Text = "需求单据";
//111======100
this.cmbPDID.Location = new System.Drawing.Point(173,96);
this.cmbPDID.Name ="cmbPDID";
this.cmbPDID.Size = new System.Drawing.Size(100, 21);
this.cmbPDID.TabIndex = 4;
this.Controls.Add(this.lblPDID);
this.Controls.Add(this.cmbPDID);

           //#####QuantityDelivered###Int32
//属性测试125QuantityDelivered
CustomerVendor_ID_Out主外字段不一致。//属性测试125QuantityDelivered
//属性测试125QuantityDelivered
//属性测试125QuantityDelivered
//属性测试125QuantityDelivered
//属性测试125QuantityDelivered
//属性测试125QuantityDelivered
//属性测试125QuantityDelivered
//属性测试125QuantityDelivered
//属性测试125QuantityDelivered
//属性测试125QuantityDelivered

           //#####ManufacturingQty###Int32
//属性测试150ManufacturingQty
CustomerVendor_ID_Out主外字段不一致。//属性测试150ManufacturingQty
//属性测试150ManufacturingQty
//属性测试150ManufacturingQty
//属性测试150ManufacturingQty
//属性测试150ManufacturingQty
//属性测试150ManufacturingQty
//属性测试150ManufacturingQty
//属性测试150ManufacturingQty
//属性测试150ManufacturingQty
//属性测试150ManufacturingQty

           //#####Priority###Int32
//属性测试175Priority
CustomerVendor_ID_Out主外字段不一致。//属性测试175Priority
//属性测试175Priority
//属性测试175Priority
//属性测试175Priority
//属性测试175Priority
//属性测试175Priority
//属性测试175Priority
//属性测试175Priority
//属性测试175Priority
//属性测试175Priority

           //#####PreStartDate###DateTime
this.lblPreStartDate.AutoSize = true;
this.lblPreStartDate.Location = new System.Drawing.Point(100,200);
this.lblPreStartDate.Name = "lblPreStartDate";
this.lblPreStartDate.Size = new System.Drawing.Size(41, 12);
this.lblPreStartDate.TabIndex = 8;
this.lblPreStartDate.Text = "预开工日";
//111======200
this.dtpPreStartDate.Location = new System.Drawing.Point(173,196);
this.dtpPreStartDate.Name ="dtpPreStartDate";
this.dtpPreStartDate.ShowCheckBox =true;
this.dtpPreStartDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreStartDate.TabIndex = 8;
this.Controls.Add(this.lblPreStartDate);
this.Controls.Add(this.dtpPreStartDate);

           //#####PreEndDate###DateTime
this.lblPreEndDate.AutoSize = true;
this.lblPreEndDate.Location = new System.Drawing.Point(100,225);
this.lblPreEndDate.Name = "lblPreEndDate";
this.lblPreEndDate.Size = new System.Drawing.Size(41, 12);
this.lblPreEndDate.TabIndex = 9;
this.lblPreEndDate.Text = "预完工日";
//111======225
this.dtpPreEndDate.Location = new System.Drawing.Point(173,221);
this.dtpPreEndDate.Name ="dtpPreEndDate";
this.dtpPreEndDate.ShowCheckBox =true;
this.dtpPreEndDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreEndDate.TabIndex = 9;
this.Controls.Add(this.lblPreEndDate);
this.Controls.Add(this.dtpPreEndDate);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,250);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 10;
this.lblSKU.Text = "母件SKU码";
this.txtSKU.Location = new System.Drawing.Point(173,246);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 10;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,275);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 11;
this.lblCNName.Text = "母件品名";
this.txtCNName.Location = new System.Drawing.Point(173,271);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 11;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####ProdDetailID###Int64
//属性测试300ProdDetailID
CustomerVendor_ID_Out主外字段不一致。//属性测试300ProdDetailID
//属性测试300ProdDetailID
//属性测试300ProdDetailID
//属性测试300ProdDetailID
//属性测试300ProdDetailID
//属性测试300ProdDetailID
//属性测试300ProdDetailID
//属性测试300ProdDetailID
//属性测试300ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,300);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 12;
this.lblProdDetailID.Text = "货品";
//111======300
this.cmbProdDetailID.Location = new System.Drawing.Point(173,296);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 12;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####BOM_ID###Int64
//属性测试325BOM_ID
CustomerVendor_ID_Out主外字段不一致。//属性测试325BOM_ID
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,325);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 13;
this.lblBOM_ID.Text = "配方名称";
//111======325
this.cmbBOM_ID.Location = new System.Drawing.Point(173,321);
this.cmbBOM_ID.Name ="cmbBOM_ID";
this.cmbBOM_ID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_ID.TabIndex = 13;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.cmbBOM_ID);

           //#####100BOM_No###String
this.lblBOM_No.AutoSize = true;
this.lblBOM_No.Location = new System.Drawing.Point(100,350);
this.lblBOM_No.Name = "lblBOM_No";
this.lblBOM_No.Size = new System.Drawing.Size(41, 12);
this.lblBOM_No.TabIndex = 14;
this.lblBOM_No.Text = "配方号";
this.txtBOM_No.Location = new System.Drawing.Point(173,346);
this.txtBOM_No.Name = "txtBOM_No";
this.txtBOM_No.Size = new System.Drawing.Size(100, 21);
this.txtBOM_No.TabIndex = 14;
this.Controls.Add(this.lblBOM_No);
this.Controls.Add(this.txtBOM_No);

           //#####Type_ID###Int64
//属性测试375Type_ID
CustomerVendor_ID_Out主外字段不一致。//属性测试375Type_ID
//属性测试375Type_ID
//属性测试375Type_ID
//属性测试375Type_ID
//属性测试375Type_ID
//属性测试375Type_ID
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,375);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 15;
this.lblType_ID.Text = "母件类型";
//111======375
this.cmbType_ID.Location = new System.Drawing.Point(173,371);
this.cmbType_ID.Name ="cmbType_ID";
this.cmbType_ID.Size = new System.Drawing.Size(100, 21);
this.cmbType_ID.TabIndex = 15;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.cmbType_ID);

           //#####Unit_ID###Int64
//属性测试400Unit_ID
CustomerVendor_ID_Out主外字段不一致。//属性测试400Unit_ID
//属性测试400Unit_ID
//属性测试400Unit_ID
//属性测试400Unit_ID
//属性测试400Unit_ID
//属性测试400Unit_ID
//属性测试400Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,400);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 16;
this.lblUnit_ID.Text = "单位";
//111======400
this.cmbUnit_ID.Location = new System.Drawing.Point(173,396);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 16;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####100CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,425);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 17;
this.lblCustomerPartNo.Text = "客户料号";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,421);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 17;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,450);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 18;
this.lblSpecifications.Text = "母件规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,446);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 18;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,475);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 19;
this.lblproperty.Text = "母件属性";
this.txtproperty.Location = new System.Drawing.Point(173,471);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 19;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Employee_ID###Int64
//属性测试500Employee_ID
CustomerVendor_ID_Out主外字段不一致。//属性测试500Employee_ID
//属性测试500Employee_ID
//属性测试500Employee_ID
//属性测试500Employee_ID
//属性测试500Employee_ID
//属性测试500Employee_ID
//属性测试500Employee_ID
//属性测试500Employee_ID
//属性测试500Employee_ID
//属性测试500Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,500);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 20;
this.lblEmployee_ID.Text = "制单人";
//111======500
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,496);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 20;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####Location_ID###Int64
//属性测试525Location_ID
CustomerVendor_ID_Out主外字段不一致。//属性测试525Location_ID
//属性测试525Location_ID
//属性测试525Location_ID
//属性测试525Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,525);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 21;
this.lblLocation_ID.Text = "预入库位";
//111======525
this.cmbLocation_ID.Location = new System.Drawing.Point(173,521);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 21;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####DepartmentID###Int64
//属性测试550DepartmentID
CustomerVendor_ID_Out主外字段不一致。//属性测试550DepartmentID
//属性测试550DepartmentID
//属性测试550DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,550);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 22;
this.lblDepartmentID.Text = "需求部门";
//111======550
this.cmbDepartmentID.Location = new System.Drawing.Point(173,546);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 22;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####CustomerVendor_ID_Out###Int64
//属性测试575CustomerVendor_ID_Out
CustomerVendor_ID_Out主外字段不一致。//属性测试575CustomerVendor_ID_Out
//属性测试575CustomerVendor_ID_Out
//属性测试575CustomerVendor_ID_Out
//属性测试575CustomerVendor_ID_Out
//属性测试575CustomerVendor_ID_Out
//属性测试575CustomerVendor_ID_Out
//属性测试575CustomerVendor_ID_Out
//属性测试575CustomerVendor_ID_Out
//属性测试575CustomerVendor_ID_Out
//属性测试575CustomerVendor_ID_Out

           //#####CustomerVendor_ID###Int64
//属性测试600CustomerVendor_ID
CustomerVendor_ID_Out主外字段不一致。this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,600);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 24;
this.lblCustomerVendor_ID.Text = "需求客户";
//111======600
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,596);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 24;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,625);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 25;
this.lblCreated_at.Text = "创建时间";
//111======625
this.dtpCreated_at.Location = new System.Drawing.Point(173,621);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 25;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试650Created_by
CustomerVendor_ID_Out主外字段不一致。//属性测试650Created_by
//属性测试650Created_by
//属性测试650Created_by
//属性测试650Created_by
//属性测试650Created_by
//属性测试650Created_by
//属性测试650Created_by
//属性测试650Created_by
//属性测试650Created_by
//属性测试650Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,675);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 27;
this.lblModified_at.Text = "修改时间";
//111======675
this.dtpModified_at.Location = new System.Drawing.Point(173,671);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 27;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试700Modified_by
CustomerVendor_ID_Out主外字段不一致。//属性测试700Modified_by
//属性测试700Modified_by
//属性测试700Modified_by
//属性测试700Modified_by
//属性测试700Modified_by
//属性测试700Modified_by
//属性测试700Modified_by
//属性测试700Modified_by
//属性测试700Modified_by
//属性测试700Modified_by

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,725);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 29;
this.lblCloseCaseOpinions.Text = "结案情况";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,721);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 29;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,750);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 30;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,746);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 30;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####ApportionedCost###Decimal
this.lblApportionedCost.AutoSize = true;
this.lblApportionedCost.Location = new System.Drawing.Point(100,775);
this.lblApportionedCost.Name = "lblApportionedCost";
this.lblApportionedCost.Size = new System.Drawing.Size(41, 12);
this.lblApportionedCost.TabIndex = 31;
this.lblApportionedCost.Text = "分摊成本";
//111======775
this.txtApportionedCost.Location = new System.Drawing.Point(173,771);
this.txtApportionedCost.Name ="txtApportionedCost";
this.txtApportionedCost.Size = new System.Drawing.Size(100, 21);
this.txtApportionedCost.TabIndex = 31;
this.Controls.Add(this.lblApportionedCost);
this.Controls.Add(this.txtApportionedCost);

           //#####TotalManuFee###Decimal
this.lblTotalManuFee.AutoSize = true;
this.lblTotalManuFee.Location = new System.Drawing.Point(100,800);
this.lblTotalManuFee.Name = "lblTotalManuFee";
this.lblTotalManuFee.Size = new System.Drawing.Size(41, 12);
this.lblTotalManuFee.TabIndex = 32;
this.lblTotalManuFee.Text = "总制造费用";
//111======800
this.txtTotalManuFee.Location = new System.Drawing.Point(173,796);
this.txtTotalManuFee.Name ="txtTotalManuFee";
this.txtTotalManuFee.Size = new System.Drawing.Size(100, 21);
this.txtTotalManuFee.TabIndex = 32;
this.Controls.Add(this.lblTotalManuFee);
this.Controls.Add(this.txtTotalManuFee);

           //#####TotalMaterialCost###Decimal
this.lblTotalMaterialCost.AutoSize = true;
this.lblTotalMaterialCost.Location = new System.Drawing.Point(100,825);
this.lblTotalMaterialCost.Name = "lblTotalMaterialCost";
this.lblTotalMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialCost.TabIndex = 33;
this.lblTotalMaterialCost.Text = "总材料成本";
//111======825
this.txtTotalMaterialCost.Location = new System.Drawing.Point(173,821);
this.txtTotalMaterialCost.Name ="txtTotalMaterialCost";
this.txtTotalMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialCost.TabIndex = 33;
this.Controls.Add(this.lblTotalMaterialCost);
this.Controls.Add(this.txtTotalMaterialCost);

           //#####TotalProductionCost###Decimal
this.lblTotalProductionCost.AutoSize = true;
this.lblTotalProductionCost.Location = new System.Drawing.Point(100,850);
this.lblTotalProductionCost.Name = "lblTotalProductionCost";
this.lblTotalProductionCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalProductionCost.TabIndex = 34;
this.lblTotalProductionCost.Text = "生产总成本";
//111======850
this.txtTotalProductionCost.Location = new System.Drawing.Point(173,846);
this.txtTotalProductionCost.Name ="txtTotalProductionCost";
this.txtTotalProductionCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalProductionCost.TabIndex = 34;
this.Controls.Add(this.lblTotalProductionCost);
this.Controls.Add(this.txtTotalProductionCost);

           //#####PeopleQty###Decimal
this.lblPeopleQty.AutoSize = true;
this.lblPeopleQty.Location = new System.Drawing.Point(100,875);
this.lblPeopleQty.Name = "lblPeopleQty";
this.lblPeopleQty.Size = new System.Drawing.Size(41, 12);
this.lblPeopleQty.TabIndex = 35;
this.lblPeopleQty.Text = "人数";
//111======875
this.txtPeopleQty.Location = new System.Drawing.Point(173,871);
this.txtPeopleQty.Name ="txtPeopleQty";
this.txtPeopleQty.Size = new System.Drawing.Size(100, 21);
this.txtPeopleQty.TabIndex = 35;
this.Controls.Add(this.lblPeopleQty);
this.Controls.Add(this.txtPeopleQty);

           //#####WorkingHour###Decimal
this.lblWorkingHour.AutoSize = true;
this.lblWorkingHour.Location = new System.Drawing.Point(100,900);
this.lblWorkingHour.Name = "lblWorkingHour";
this.lblWorkingHour.Size = new System.Drawing.Size(41, 12);
this.lblWorkingHour.TabIndex = 36;
this.lblWorkingHour.Text = "工时";
//111======900
this.txtWorkingHour.Location = new System.Drawing.Point(173,896);
this.txtWorkingHour.Name ="txtWorkingHour";
this.txtWorkingHour.Size = new System.Drawing.Size(100, 21);
this.txtWorkingHour.TabIndex = 36;
this.Controls.Add(this.lblWorkingHour);
this.Controls.Add(this.txtWorkingHour);

           //#####MachineHour###Decimal
this.lblMachineHour.AutoSize = true;
this.lblMachineHour.Location = new System.Drawing.Point(100,925);
this.lblMachineHour.Name = "lblMachineHour";
this.lblMachineHour.Size = new System.Drawing.Size(41, 12);
this.lblMachineHour.TabIndex = 37;
this.lblMachineHour.Text = "机时";
//111======925
this.txtMachineHour.Location = new System.Drawing.Point(173,921);
this.txtMachineHour.Name ="txtMachineHour";
this.txtMachineHour.Size = new System.Drawing.Size(100, 21);
this.txtMachineHour.TabIndex = 37;
this.Controls.Add(this.lblMachineHour);
this.Controls.Add(this.txtMachineHour);

           //#####IncludeSubBOM###Boolean
this.lblIncludeSubBOM.AutoSize = true;
this.lblIncludeSubBOM.Location = new System.Drawing.Point(100,950);
this.lblIncludeSubBOM.Name = "lblIncludeSubBOM";
this.lblIncludeSubBOM.Size = new System.Drawing.Size(41, 12);
this.lblIncludeSubBOM.TabIndex = 38;
this.lblIncludeSubBOM.Text = "上层驱动";
this.chkIncludeSubBOM.Location = new System.Drawing.Point(173,946);
this.chkIncludeSubBOM.Name = "chkIncludeSubBOM";
this.chkIncludeSubBOM.Size = new System.Drawing.Size(100, 21);
this.chkIncludeSubBOM.TabIndex = 38;
this.Controls.Add(this.lblIncludeSubBOM);
this.Controls.Add(this.chkIncludeSubBOM);

           //#####IsOutSourced###Boolean
this.lblIsOutSourced.AutoSize = true;
this.lblIsOutSourced.Location = new System.Drawing.Point(100,975);
this.lblIsOutSourced.Name = "lblIsOutSourced";
this.lblIsOutSourced.Size = new System.Drawing.Size(41, 12);
this.lblIsOutSourced.TabIndex = 39;
this.lblIsOutSourced.Text = "是否托工";
this.chkIsOutSourced.Location = new System.Drawing.Point(173,971);
this.chkIsOutSourced.Name = "chkIsOutSourced";
this.chkIsOutSourced.Size = new System.Drawing.Size(100, 21);
this.chkIsOutSourced.TabIndex = 39;
this.Controls.Add(this.lblIsOutSourced);
this.Controls.Add(this.chkIsOutSourced);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,1000);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 40;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,996);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 40;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Approver_by###Int64
//属性测试1025Approver_by
CustomerVendor_ID_Out主外字段不一致。//属性测试1025Approver_by
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

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,1075);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 43;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,1071);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 43;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApproverTime###DateTime
this.lblApproverTime.AutoSize = true;
this.lblApproverTime.Location = new System.Drawing.Point(100,1100);
this.lblApproverTime.Name = "lblApproverTime";
this.lblApproverTime.Size = new System.Drawing.Size(41, 12);
this.lblApproverTime.TabIndex = 44;
this.lblApproverTime.Text = "审批时间";
//111======1100
this.dtpApproverTime.Location = new System.Drawing.Point(173,1096);
this.dtpApproverTime.Name ="dtpApproverTime";
this.dtpApproverTime.ShowCheckBox =true;
this.dtpApproverTime.Size = new System.Drawing.Size(100, 21);
this.dtpApproverTime.TabIndex = 44;
this.Controls.Add(this.lblApproverTime);
this.Controls.Add(this.dtpApproverTime);

           //#####ApprovalStatus###SByte

           //#####DataStatus###Int32
//属性测试1150DataStatus
CustomerVendor_ID_Out主外字段不一致。//属性测试1150DataStatus
//属性测试1150DataStatus
//属性测试1150DataStatus
//属性测试1150DataStatus
//属性测试1150DataStatus
//属性测试1150DataStatus
//属性测试1150DataStatus
//属性测试1150DataStatus
//属性测试1150DataStatus
//属性测试1150DataStatus

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,1175);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 47;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,1171);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 47;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试1200PrintStatus
CustomerVendor_ID_Out主外字段不一致。//属性测试1200PrintStatus
//属性测试1200PrintStatus
//属性测试1200PrintStatus
//属性测试1200PrintStatus
//属性测试1200PrintStatus
//属性测试1200PrintStatus
//属性测试1200PrintStatus
//属性测试1200PrintStatus
//属性测试1200PrintStatus
//属性测试1200PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMONO );
this.Controls.Add(this.txtMONO );

                this.Controls.Add(this.lblPDNO );
this.Controls.Add(this.txtPDNO );

                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblPDCID );
this.Controls.Add(this.cmbPDCID );

                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblPDID );
this.Controls.Add(this.cmbPDID );

                CustomerVendor_ID_Out主外字段不一致。
                CustomerVendor_ID_Out主外字段不一致。
                CustomerVendor_ID_Out主外字段不一致。
                this.Controls.Add(this.lblPreStartDate );
this.Controls.Add(this.dtpPreStartDate );

                this.Controls.Add(this.lblPreEndDate );
this.Controls.Add(this.dtpPreEndDate );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.cmbBOM_ID );

                this.Controls.Add(this.lblBOM_No );
this.Controls.Add(this.txtBOM_No );

                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.cmbType_ID );

                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.cmbUnit_ID );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                CustomerVendor_ID_Out主外字段不一致。
                CustomerVendor_ID_Out主外字段不一致。this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                CustomerVendor_ID_Out主外字段不一致。
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                CustomerVendor_ID_Out主外字段不一致。
                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApportionedCost );
this.Controls.Add(this.txtApportionedCost );

                this.Controls.Add(this.lblTotalManuFee );
this.Controls.Add(this.txtTotalManuFee );

                this.Controls.Add(this.lblTotalMaterialCost );
this.Controls.Add(this.txtTotalMaterialCost );

                this.Controls.Add(this.lblTotalProductionCost );
this.Controls.Add(this.txtTotalProductionCost );

                this.Controls.Add(this.lblPeopleQty );
this.Controls.Add(this.txtPeopleQty );

                this.Controls.Add(this.lblWorkingHour );
this.Controls.Add(this.txtWorkingHour );

                this.Controls.Add(this.lblMachineHour );
this.Controls.Add(this.txtMachineHour );

                this.Controls.Add(this.lblIncludeSubBOM );
this.Controls.Add(this.chkIncludeSubBOM );

                this.Controls.Add(this.lblIsOutSourced );
this.Controls.Add(this.chkIsOutSourced );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                CustomerVendor_ID_Out主外字段不一致。
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblApproverTime );
this.Controls.Add(this.dtpApproverTime );

                
                CustomerVendor_ID_Out主外字段不一致。
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                CustomerVendor_ID_Out主外字段不一致。
                    
            this.Name = "tb_ManufacturingOrderQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMONO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMONO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPDNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPDNO;

    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPDCID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPDCID;

    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPDID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPDID;

    
        
              CustomerVendor_ID_Out主外字段不一致。
    
        
              CustomerVendor_ID_Out主外字段不一致。
    
        
              CustomerVendor_ID_Out主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPreStartDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPreStartDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPreEndDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPreEndDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBOM_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_No;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBOM_No;

    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblType_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbType_ID;

    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnit_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              CustomerVendor_ID_Out主外字段不一致。
    
        
              CustomerVendor_ID_Out主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              CustomerVendor_ID_Out主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              CustomerVendor_ID_Out主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApportionedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApportionedCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalManuFee;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalManuFee;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalMaterialCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalMaterialCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalProductionCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalProductionCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPeopleQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPeopleQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWorkingHour;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWorkingHour;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMachineHour;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMachineHour;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIncludeSubBOM;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIncludeSubBOM;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsOutSourced;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsOutSourced;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              CustomerVendor_ID_Out主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApproverTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApproverTime;

    
        
              
    
        
              CustomerVendor_ID_Out主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              CustomerVendor_ID_Out主外字段不一致。
    
    
   
 





    }
}

