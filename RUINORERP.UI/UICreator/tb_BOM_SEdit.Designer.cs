﻿// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:34
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 标准物料表BOM_BillOfMateria_S-要适当冗余? 生产是从0开始的。先有下级才有上级。
    /// </summary>
    partial class tb_BOM_SEdit
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
     this.lblBOM_No = new Krypton.Toolkit.KryptonLabel();
this.txtBOM_No = new Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblBOM_Name = new Krypton.Toolkit.KryptonLabel();
this.txtBOM_Name = new Krypton.Toolkit.KryptonTextBox();

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblDoc_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbDoc_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblBOM_S_VERID = new Krypton.Toolkit.KryptonLabel();
this.cmbBOM_S_VERID = new Krypton.Toolkit.KryptonComboBox();

this.lblEffective_at = new Krypton.Toolkit.KryptonLabel();
this.dtpEffective_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblis_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkis_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkis_enabled.Values.Text ="";
this.chkis_enabled.Checked = true;
this.chkis_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblis_available = new Krypton.Toolkit.KryptonLabel();
this.chkis_available = new Krypton.Toolkit.KryptonCheckBox();
this.chkis_available.Values.Text ="";
this.chkis_available.Checked = true;
this.chkis_available.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblManufacturingCost = new Krypton.Toolkit.KryptonLabel();
this.txtManufacturingCost = new Krypton.Toolkit.KryptonTextBox();

this.lblOutManuCost = new Krypton.Toolkit.KryptonLabel();
this.txtOutManuCost = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalMaterialCost = new Krypton.Toolkit.KryptonLabel();
this.txtTotalMaterialCost = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalMaterialQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalMaterialQty = new Krypton.Toolkit.KryptonTextBox();

this.lblOutputQty = new Krypton.Toolkit.KryptonLabel();
this.txtOutputQty = new Krypton.Toolkit.KryptonTextBox();

this.lblPeopleQty = new Krypton.Toolkit.KryptonLabel();
this.txtPeopleQty = new Krypton.Toolkit.KryptonTextBox();

this.lblWorkingHour = new Krypton.Toolkit.KryptonLabel();
this.txtWorkingHour = new Krypton.Toolkit.KryptonTextBox();

this.lblMachineHour = new Krypton.Toolkit.KryptonLabel();
this.txtMachineHour = new Krypton.Toolkit.KryptonTextBox();

this.lblExpirationDate = new Krypton.Toolkit.KryptonLabel();
this.dtpExpirationDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblDailyQty = new Krypton.Toolkit.KryptonLabel();
this.txtDailyQty = new Krypton.Toolkit.KryptonTextBox();

this.lblSelfProductionAllCosts = new Krypton.Toolkit.KryptonLabel();
this.txtSelfProductionAllCosts = new Krypton.Toolkit.KryptonTextBox();

this.lblOutProductionAllCosts = new Krypton.Toolkit.KryptonLabel();
this.txtOutProductionAllCosts = new Krypton.Toolkit.KryptonTextBox();


this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####50BOM_No###String
this.lblBOM_No.AutoSize = true;
this.lblBOM_No.Location = new System.Drawing.Point(100,25);
this.lblBOM_No.Name = "lblBOM_No";
this.lblBOM_No.Size = new System.Drawing.Size(41, 12);
this.lblBOM_No.TabIndex = 1;
this.lblBOM_No.Text = "配方编号";
this.txtBOM_No.Location = new System.Drawing.Point(173,21);
this.txtBOM_No.Name = "txtBOM_No";
this.txtBOM_No.Size = new System.Drawing.Size(100, 21);
this.txtBOM_No.TabIndex = 1;
this.Controls.Add(this.lblBOM_No);
this.Controls.Add(this.txtBOM_No);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,50);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 2;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,46);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 2;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####100BOM_Name###String
this.lblBOM_Name.AutoSize = true;
this.lblBOM_Name.Location = new System.Drawing.Point(100,75);
this.lblBOM_Name.Name = "lblBOM_Name";
this.lblBOM_Name.Size = new System.Drawing.Size(41, 12);
this.lblBOM_Name.TabIndex = 3;
this.lblBOM_Name.Text = "配方名称";
this.txtBOM_Name.Location = new System.Drawing.Point(173,71);
this.txtBOM_Name.Name = "txtBOM_Name";
this.txtBOM_Name.Size = new System.Drawing.Size(100, 21);
this.txtBOM_Name.TabIndex = 3;
this.Controls.Add(this.lblBOM_Name);
this.Controls.Add(this.txtBOM_Name);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,100);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 4;
this.lblSKU.Text = "SKU码";
this.txtSKU.Location = new System.Drawing.Point(173,96);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 4;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####ProdDetailID###Int64
//属性测试125ProdDetailID
//属性测试125ProdDetailID
//属性测试125ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,125);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 5;
this.lblProdDetailID.Text = "母件";
//111======125
this.cmbProdDetailID.Location = new System.Drawing.Point(173,121);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 5;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####DepartmentID###Int64
//属性测试150DepartmentID
//属性测试150DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,150);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 6;
this.lblDepartmentID.Text = "制造部门";
//111======150
this.cmbDepartmentID.Location = new System.Drawing.Point(173,146);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 6;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####Doc_ID###Int64
//属性测试175Doc_ID
this.lblDoc_ID.AutoSize = true;
this.lblDoc_ID.Location = new System.Drawing.Point(100,175);
this.lblDoc_ID.Name = "lblDoc_ID";
this.lblDoc_ID.Size = new System.Drawing.Size(41, 12);
this.lblDoc_ID.TabIndex = 7;
this.lblDoc_ID.Text = "工艺文件";
//111======175
this.cmbDoc_ID.Location = new System.Drawing.Point(173,171);
this.cmbDoc_ID.Name ="cmbDoc_ID";
this.cmbDoc_ID.Size = new System.Drawing.Size(100, 21);
this.cmbDoc_ID.TabIndex = 7;
this.Controls.Add(this.lblDoc_ID);
this.Controls.Add(this.cmbDoc_ID);

           //#####BOM_S_VERID###Int64
//属性测试200BOM_S_VERID
//属性测试200BOM_S_VERID
//属性测试200BOM_S_VERID
//属性测试200BOM_S_VERID
this.lblBOM_S_VERID.AutoSize = true;
this.lblBOM_S_VERID.Location = new System.Drawing.Point(100,200);
this.lblBOM_S_VERID.Name = "lblBOM_S_VERID";
this.lblBOM_S_VERID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_S_VERID.TabIndex = 8;
this.lblBOM_S_VERID.Text = "版本号";
//111======200
this.cmbBOM_S_VERID.Location = new System.Drawing.Point(173,196);
this.cmbBOM_S_VERID.Name ="cmbBOM_S_VERID";
this.cmbBOM_S_VERID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_S_VERID.TabIndex = 8;
this.Controls.Add(this.lblBOM_S_VERID);
this.Controls.Add(this.cmbBOM_S_VERID);

           //#####Effective_at###DateTime
this.lblEffective_at.AutoSize = true;
this.lblEffective_at.Location = new System.Drawing.Point(100,225);
this.lblEffective_at.Name = "lblEffective_at";
this.lblEffective_at.Size = new System.Drawing.Size(41, 12);
this.lblEffective_at.TabIndex = 9;
this.lblEffective_at.Text = "生效时间";
//111======225
this.dtpEffective_at.Location = new System.Drawing.Point(173,221);
this.dtpEffective_at.Name ="dtpEffective_at";
this.dtpEffective_at.Size = new System.Drawing.Size(100, 21);
this.dtpEffective_at.TabIndex = 9;
this.Controls.Add(this.lblEffective_at);
this.Controls.Add(this.dtpEffective_at);

           //#####is_enabled###Boolean
this.lblis_enabled.AutoSize = true;
this.lblis_enabled.Location = new System.Drawing.Point(100,250);
this.lblis_enabled.Name = "lblis_enabled";
this.lblis_enabled.Size = new System.Drawing.Size(41, 12);
this.lblis_enabled.TabIndex = 10;
this.lblis_enabled.Text = "是否启用";
this.chkis_enabled.Location = new System.Drawing.Point(173,246);
this.chkis_enabled.Name = "chkis_enabled";
this.chkis_enabled.Size = new System.Drawing.Size(100, 21);
this.chkis_enabled.TabIndex = 10;
this.Controls.Add(this.lblis_enabled);
this.Controls.Add(this.chkis_enabled);

           //#####is_available###Boolean
this.lblis_available.AutoSize = true;
this.lblis_available.Location = new System.Drawing.Point(100,275);
this.lblis_available.Name = "lblis_available";
this.lblis_available.Size = new System.Drawing.Size(41, 12);
this.lblis_available.TabIndex = 11;
this.lblis_available.Text = "是否可用";
this.chkis_available.Location = new System.Drawing.Point(173,271);
this.chkis_available.Name = "chkis_available";
this.chkis_available.Size = new System.Drawing.Size(100, 21);
this.chkis_available.TabIndex = 11;
this.Controls.Add(this.lblis_available);
this.Controls.Add(this.chkis_available);

           //#####ManufacturingCost###Decimal
this.lblManufacturingCost.AutoSize = true;
this.lblManufacturingCost.Location = new System.Drawing.Point(100,300);
this.lblManufacturingCost.Name = "lblManufacturingCost";
this.lblManufacturingCost.Size = new System.Drawing.Size(41, 12);
this.lblManufacturingCost.TabIndex = 12;
this.lblManufacturingCost.Text = "自产制造费用";
//111======300
this.txtManufacturingCost.Location = new System.Drawing.Point(173,296);
this.txtManufacturingCost.Name ="txtManufacturingCost";
this.txtManufacturingCost.Size = new System.Drawing.Size(100, 21);
this.txtManufacturingCost.TabIndex = 12;
this.Controls.Add(this.lblManufacturingCost);
this.Controls.Add(this.txtManufacturingCost);

           //#####OutManuCost###Decimal
this.lblOutManuCost.AutoSize = true;
this.lblOutManuCost.Location = new System.Drawing.Point(100,325);
this.lblOutManuCost.Name = "lblOutManuCost";
this.lblOutManuCost.Size = new System.Drawing.Size(41, 12);
this.lblOutManuCost.TabIndex = 13;
this.lblOutManuCost.Text = "外发费用";
//111======325
this.txtOutManuCost.Location = new System.Drawing.Point(173,321);
this.txtOutManuCost.Name ="txtOutManuCost";
this.txtOutManuCost.Size = new System.Drawing.Size(100, 21);
this.txtOutManuCost.TabIndex = 13;
this.Controls.Add(this.lblOutManuCost);
this.Controls.Add(this.txtOutManuCost);

           //#####TotalMaterialCost###Decimal
this.lblTotalMaterialCost.AutoSize = true;
this.lblTotalMaterialCost.Location = new System.Drawing.Point(100,350);
this.lblTotalMaterialCost.Name = "lblTotalMaterialCost";
this.lblTotalMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialCost.TabIndex = 14;
this.lblTotalMaterialCost.Text = "总物料费用";
//111======350
this.txtTotalMaterialCost.Location = new System.Drawing.Point(173,346);
this.txtTotalMaterialCost.Name ="txtTotalMaterialCost";
this.txtTotalMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialCost.TabIndex = 14;
this.Controls.Add(this.lblTotalMaterialCost);
this.Controls.Add(this.txtTotalMaterialCost);

           //#####TotalMaterialQty###Decimal
this.lblTotalMaterialQty.AutoSize = true;
this.lblTotalMaterialQty.Location = new System.Drawing.Point(100,375);
this.lblTotalMaterialQty.Name = "lblTotalMaterialQty";
this.lblTotalMaterialQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialQty.TabIndex = 15;
this.lblTotalMaterialQty.Text = "用料总量";
//111======375
this.txtTotalMaterialQty.Location = new System.Drawing.Point(173,371);
this.txtTotalMaterialQty.Name ="txtTotalMaterialQty";
this.txtTotalMaterialQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialQty.TabIndex = 15;
this.Controls.Add(this.lblTotalMaterialQty);
this.Controls.Add(this.txtTotalMaterialQty);

           //#####OutputQty###Decimal
this.lblOutputQty.AutoSize = true;
this.lblOutputQty.Location = new System.Drawing.Point(100,400);
this.lblOutputQty.Name = "lblOutputQty";
this.lblOutputQty.Size = new System.Drawing.Size(41, 12);
this.lblOutputQty.TabIndex = 16;
this.lblOutputQty.Text = "产出量";
//111======400
this.txtOutputQty.Location = new System.Drawing.Point(173,396);
this.txtOutputQty.Name ="txtOutputQty";
this.txtOutputQty.Size = new System.Drawing.Size(100, 21);
this.txtOutputQty.TabIndex = 16;
this.Controls.Add(this.lblOutputQty);
this.Controls.Add(this.txtOutputQty);

           //#####PeopleQty###Decimal
this.lblPeopleQty.AutoSize = true;
this.lblPeopleQty.Location = new System.Drawing.Point(100,425);
this.lblPeopleQty.Name = "lblPeopleQty";
this.lblPeopleQty.Size = new System.Drawing.Size(41, 12);
this.lblPeopleQty.TabIndex = 17;
this.lblPeopleQty.Text = "人数";
//111======425
this.txtPeopleQty.Location = new System.Drawing.Point(173,421);
this.txtPeopleQty.Name ="txtPeopleQty";
this.txtPeopleQty.Size = new System.Drawing.Size(100, 21);
this.txtPeopleQty.TabIndex = 17;
this.Controls.Add(this.lblPeopleQty);
this.Controls.Add(this.txtPeopleQty);

           //#####WorkingHour###Decimal
this.lblWorkingHour.AutoSize = true;
this.lblWorkingHour.Location = new System.Drawing.Point(100,450);
this.lblWorkingHour.Name = "lblWorkingHour";
this.lblWorkingHour.Size = new System.Drawing.Size(41, 12);
this.lblWorkingHour.TabIndex = 18;
this.lblWorkingHour.Text = "工时";
//111======450
this.txtWorkingHour.Location = new System.Drawing.Point(173,446);
this.txtWorkingHour.Name ="txtWorkingHour";
this.txtWorkingHour.Size = new System.Drawing.Size(100, 21);
this.txtWorkingHour.TabIndex = 18;
this.Controls.Add(this.lblWorkingHour);
this.Controls.Add(this.txtWorkingHour);

           //#####MachineHour###Decimal
this.lblMachineHour.AutoSize = true;
this.lblMachineHour.Location = new System.Drawing.Point(100,475);
this.lblMachineHour.Name = "lblMachineHour";
this.lblMachineHour.Size = new System.Drawing.Size(41, 12);
this.lblMachineHour.TabIndex = 19;
this.lblMachineHour.Text = "机时";
//111======475
this.txtMachineHour.Location = new System.Drawing.Point(173,471);
this.txtMachineHour.Name ="txtMachineHour";
this.txtMachineHour.Size = new System.Drawing.Size(100, 21);
this.txtMachineHour.TabIndex = 19;
this.Controls.Add(this.lblMachineHour);
this.Controls.Add(this.txtMachineHour);

           //#####ExpirationDate###DateTime
this.lblExpirationDate.AutoSize = true;
this.lblExpirationDate.Location = new System.Drawing.Point(100,500);
this.lblExpirationDate.Name = "lblExpirationDate";
this.lblExpirationDate.Size = new System.Drawing.Size(41, 12);
this.lblExpirationDate.TabIndex = 20;
this.lblExpirationDate.Text = "截止日期";
//111======500
this.dtpExpirationDate.Location = new System.Drawing.Point(173,496);
this.dtpExpirationDate.Name ="dtpExpirationDate";
this.dtpExpirationDate.ShowCheckBox =true;
this.dtpExpirationDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpirationDate.TabIndex = 20;
this.Controls.Add(this.lblExpirationDate);
this.Controls.Add(this.dtpExpirationDate);

           //#####DailyQty###Decimal
this.lblDailyQty.AutoSize = true;
this.lblDailyQty.Location = new System.Drawing.Point(100,525);
this.lblDailyQty.Name = "lblDailyQty";
this.lblDailyQty.Size = new System.Drawing.Size(41, 12);
this.lblDailyQty.TabIndex = 21;
this.lblDailyQty.Text = "日产量";
//111======525
this.txtDailyQty.Location = new System.Drawing.Point(173,521);
this.txtDailyQty.Name ="txtDailyQty";
this.txtDailyQty.Size = new System.Drawing.Size(100, 21);
this.txtDailyQty.TabIndex = 21;
this.Controls.Add(this.lblDailyQty);
this.Controls.Add(this.txtDailyQty);

           //#####SelfProductionAllCosts###Decimal
this.lblSelfProductionAllCosts.AutoSize = true;
this.lblSelfProductionAllCosts.Location = new System.Drawing.Point(100,550);
this.lblSelfProductionAllCosts.Name = "lblSelfProductionAllCosts";
this.lblSelfProductionAllCosts.Size = new System.Drawing.Size(41, 12);
this.lblSelfProductionAllCosts.TabIndex = 22;
this.lblSelfProductionAllCosts.Text = "自产总成本";
//111======550
this.txtSelfProductionAllCosts.Location = new System.Drawing.Point(173,546);
this.txtSelfProductionAllCosts.Name ="txtSelfProductionAllCosts";
this.txtSelfProductionAllCosts.Size = new System.Drawing.Size(100, 21);
this.txtSelfProductionAllCosts.TabIndex = 22;
this.Controls.Add(this.lblSelfProductionAllCosts);
this.Controls.Add(this.txtSelfProductionAllCosts);

           //#####OutProductionAllCosts###Decimal
this.lblOutProductionAllCosts.AutoSize = true;
this.lblOutProductionAllCosts.Location = new System.Drawing.Point(100,575);
this.lblOutProductionAllCosts.Name = "lblOutProductionAllCosts";
this.lblOutProductionAllCosts.Size = new System.Drawing.Size(41, 12);
this.lblOutProductionAllCosts.TabIndex = 23;
this.lblOutProductionAllCosts.Text = "外发总成本";
//111======575
this.txtOutProductionAllCosts.Location = new System.Drawing.Point(173,571);
this.txtOutProductionAllCosts.Name ="txtOutProductionAllCosts";
this.txtOutProductionAllCosts.Size = new System.Drawing.Size(100, 21);
this.txtOutProductionAllCosts.TabIndex = 23;
this.Controls.Add(this.lblOutProductionAllCosts);
this.Controls.Add(this.txtOutProductionAllCosts);

           //#####2147483647BOM_Iimage###Binary

           //#####500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,625);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 25;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,621);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 25;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,650);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 26;
this.lblCreated_at.Text = "创建时间";
//111======650
this.dtpCreated_at.Location = new System.Drawing.Point(173,646);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 26;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试675Created_by
//属性测试675Created_by
//属性测试675Created_by
//属性测试675Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,675);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 27;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,671);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 27;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,700);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 28;
this.lblModified_at.Text = "修改时间";
//111======700
this.dtpModified_at.Location = new System.Drawing.Point(173,696);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 28;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试725Modified_by
//属性测试725Modified_by
//属性测试725Modified_by
//属性测试725Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,725);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 29;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,721);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 29;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,750);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 30;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,746);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 30;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试775DataStatus
//属性测试775DataStatus
//属性测试775DataStatus
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

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,800);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 32;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,796);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 32;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试825Approver_by
//属性测试825Approver_by
//属性测试825Approver_by
//属性测试825Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,825);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 33;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,821);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 33;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,850);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 34;
this.lblApprover_at.Text = "审批时间";
//111======850
this.dtpApprover_at.Location = new System.Drawing.Point(173,846);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 34;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,900);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 36;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,896);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 36;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

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
           // this.kryptonPanel1.TabIndex = 36;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblBOM_No );
this.Controls.Add(this.txtBOM_No );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblBOM_Name );
this.Controls.Add(this.txtBOM_Name );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblDoc_ID );
this.Controls.Add(this.cmbDoc_ID );

                this.Controls.Add(this.lblBOM_S_VERID );
this.Controls.Add(this.cmbBOM_S_VERID );

                this.Controls.Add(this.lblEffective_at );
this.Controls.Add(this.dtpEffective_at );

                this.Controls.Add(this.lblis_enabled );
this.Controls.Add(this.chkis_enabled );

                this.Controls.Add(this.lblis_available );
this.Controls.Add(this.chkis_available );

                this.Controls.Add(this.lblManufacturingCost );
this.Controls.Add(this.txtManufacturingCost );

                this.Controls.Add(this.lblOutManuCost );
this.Controls.Add(this.txtOutManuCost );

                this.Controls.Add(this.lblTotalMaterialCost );
this.Controls.Add(this.txtTotalMaterialCost );

                this.Controls.Add(this.lblTotalMaterialQty );
this.Controls.Add(this.txtTotalMaterialQty );

                this.Controls.Add(this.lblOutputQty );
this.Controls.Add(this.txtOutputQty );

                this.Controls.Add(this.lblPeopleQty );
this.Controls.Add(this.txtPeopleQty );

                this.Controls.Add(this.lblWorkingHour );
this.Controls.Add(this.txtWorkingHour );

                this.Controls.Add(this.lblMachineHour );
this.Controls.Add(this.txtMachineHour );

                this.Controls.Add(this.lblExpirationDate );
this.Controls.Add(this.dtpExpirationDate );

                this.Controls.Add(this.lblDailyQty );
this.Controls.Add(this.txtDailyQty );

                this.Controls.Add(this.lblSelfProductionAllCosts );
this.Controls.Add(this.txtSelfProductionAllCosts );

                this.Controls.Add(this.lblOutProductionAllCosts );
this.Controls.Add(this.txtOutProductionAllCosts );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

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

                            // 
            // "tb_BOM_SEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_BOM_SEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblBOM_No;
private Krypton.Toolkit.KryptonTextBox txtBOM_No;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_Name;
private Krypton.Toolkit.KryptonTextBox txtBOM_Name;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDoc_ID;
private Krypton.Toolkit.KryptonComboBox cmbDoc_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_S_VERID;
private Krypton.Toolkit.KryptonComboBox cmbBOM_S_VERID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEffective_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpEffective_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblis_enabled;
private Krypton.Toolkit.KryptonCheckBox chkis_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblis_available;
private Krypton.Toolkit.KryptonCheckBox chkis_available;

    
        
              private Krypton.Toolkit.KryptonLabel lblManufacturingCost;
private Krypton.Toolkit.KryptonTextBox txtManufacturingCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblOutManuCost;
private Krypton.Toolkit.KryptonTextBox txtOutManuCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalMaterialCost;
private Krypton.Toolkit.KryptonTextBox txtTotalMaterialCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalMaterialQty;
private Krypton.Toolkit.KryptonTextBox txtTotalMaterialQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblOutputQty;
private Krypton.Toolkit.KryptonTextBox txtOutputQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblPeopleQty;
private Krypton.Toolkit.KryptonTextBox txtPeopleQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblWorkingHour;
private Krypton.Toolkit.KryptonTextBox txtWorkingHour;

    
        
              private Krypton.Toolkit.KryptonLabel lblMachineHour;
private Krypton.Toolkit.KryptonTextBox txtMachineHour;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpirationDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpExpirationDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblDailyQty;
private Krypton.Toolkit.KryptonTextBox txtDailyQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSelfProductionAllCosts;
private Krypton.Toolkit.KryptonTextBox txtSelfProductionAllCosts;

    
        
              private Krypton.Toolkit.KryptonLabel lblOutProductionAllCosts;
private Krypton.Toolkit.KryptonTextBox txtOutProductionAllCosts;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
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

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}
