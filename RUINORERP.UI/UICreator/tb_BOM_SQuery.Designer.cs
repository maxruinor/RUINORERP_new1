
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/25/2024 20:53:10
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
    partial class tb_BOM_SQuery
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
     
     this.lblBOM_No = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBOM_No = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblBOM_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBOM_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDoc_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDoc_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBOM_S_VERID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBOM_S_VERID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEffective_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEffective_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblis_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkis_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkis_enabled.Values.Text ="";
this.chkis_enabled.Checked = true;
this.chkis_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblis_available = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkis_available = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkis_available.Values.Text ="";
this.chkis_available.Checked = true;
this.chkis_available.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblOutApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOutApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSelfApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSelfApportionedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalSelfManuCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalSelfManuCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalOutManuCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalOutManuCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalMaterialQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalMaterialQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOutputQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOutputQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPeopleQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPeopleQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWorkingHour = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWorkingHour = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMachineHour = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMachineHour = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblExpirationDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpExpirationDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDailyQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDailyQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSelfProductionAllCosts = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSelfProductionAllCosts = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOutProductionAllCosts = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOutProductionAllCosts = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


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

           //#####Employee_ID###Int64
//属性测试175Employee_ID
//属性测试175Employee_ID
//属性测试175Employee_ID
//属性测试175Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,175);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 7;
this.lblEmployee_ID.Text = "报销人";
//111======175
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,171);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 7;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####Doc_ID###Int64
//属性测试200Doc_ID
this.lblDoc_ID.AutoSize = true;
this.lblDoc_ID.Location = new System.Drawing.Point(100,200);
this.lblDoc_ID.Name = "lblDoc_ID";
this.lblDoc_ID.Size = new System.Drawing.Size(41, 12);
this.lblDoc_ID.TabIndex = 8;
this.lblDoc_ID.Text = "工艺文件";
//111======200
this.cmbDoc_ID.Location = new System.Drawing.Point(173,196);
this.cmbDoc_ID.Name ="cmbDoc_ID";
this.cmbDoc_ID.Size = new System.Drawing.Size(100, 21);
this.cmbDoc_ID.TabIndex = 8;
this.Controls.Add(this.lblDoc_ID);
this.Controls.Add(this.cmbDoc_ID);

           //#####BOM_S_VERID###Int64
//属性测试225BOM_S_VERID
//属性测试225BOM_S_VERID
//属性测试225BOM_S_VERID
//属性测试225BOM_S_VERID
//属性测试225BOM_S_VERID
this.lblBOM_S_VERID.AutoSize = true;
this.lblBOM_S_VERID.Location = new System.Drawing.Point(100,225);
this.lblBOM_S_VERID.Name = "lblBOM_S_VERID";
this.lblBOM_S_VERID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_S_VERID.TabIndex = 9;
this.lblBOM_S_VERID.Text = "版本号";
//111======225
this.cmbBOM_S_VERID.Location = new System.Drawing.Point(173,221);
this.cmbBOM_S_VERID.Name ="cmbBOM_S_VERID";
this.cmbBOM_S_VERID.Size = new System.Drawing.Size(100, 21);
this.cmbBOM_S_VERID.TabIndex = 9;
this.Controls.Add(this.lblBOM_S_VERID);
this.Controls.Add(this.cmbBOM_S_VERID);

           //#####Effective_at###DateTime
this.lblEffective_at.AutoSize = true;
this.lblEffective_at.Location = new System.Drawing.Point(100,250);
this.lblEffective_at.Name = "lblEffective_at";
this.lblEffective_at.Size = new System.Drawing.Size(41, 12);
this.lblEffective_at.TabIndex = 10;
this.lblEffective_at.Text = "生效时间";
//111======250
this.dtpEffective_at.Location = new System.Drawing.Point(173,246);
this.dtpEffective_at.Name ="dtpEffective_at";
this.dtpEffective_at.Size = new System.Drawing.Size(100, 21);
this.dtpEffective_at.TabIndex = 10;
this.Controls.Add(this.lblEffective_at);
this.Controls.Add(this.dtpEffective_at);

           //#####is_enabled###Boolean
this.lblis_enabled.AutoSize = true;
this.lblis_enabled.Location = new System.Drawing.Point(100,275);
this.lblis_enabled.Name = "lblis_enabled";
this.lblis_enabled.Size = new System.Drawing.Size(41, 12);
this.lblis_enabled.TabIndex = 11;
this.lblis_enabled.Text = "是否启用";
this.chkis_enabled.Location = new System.Drawing.Point(173,271);
this.chkis_enabled.Name = "chkis_enabled";
this.chkis_enabled.Size = new System.Drawing.Size(100, 21);
this.chkis_enabled.TabIndex = 11;
this.Controls.Add(this.lblis_enabled);
this.Controls.Add(this.chkis_enabled);

           //#####is_available###Boolean
this.lblis_available.AutoSize = true;
this.lblis_available.Location = new System.Drawing.Point(100,300);
this.lblis_available.Name = "lblis_available";
this.lblis_available.Size = new System.Drawing.Size(41, 12);
this.lblis_available.TabIndex = 12;
this.lblis_available.Text = "是否可用";
this.chkis_available.Location = new System.Drawing.Point(173,296);
this.chkis_available.Name = "chkis_available";
this.chkis_available.Size = new System.Drawing.Size(100, 21);
this.chkis_available.TabIndex = 12;
this.Controls.Add(this.lblis_available);
this.Controls.Add(this.chkis_available);

           //#####OutApportionedCost###Decimal
this.lblOutApportionedCost.AutoSize = true;
this.lblOutApportionedCost.Location = new System.Drawing.Point(100,325);
this.lblOutApportionedCost.Name = "lblOutApportionedCost";
this.lblOutApportionedCost.Size = new System.Drawing.Size(41, 12);
this.lblOutApportionedCost.TabIndex = 13;
this.lblOutApportionedCost.Text = "处发分摊费用";
//111======325
this.txtOutApportionedCost.Location = new System.Drawing.Point(173,321);
this.txtOutApportionedCost.Name ="txtOutApportionedCost";
this.txtOutApportionedCost.Size = new System.Drawing.Size(100, 21);
this.txtOutApportionedCost.TabIndex = 13;
this.Controls.Add(this.lblOutApportionedCost);
this.Controls.Add(this.txtOutApportionedCost);

           //#####SelfApportionedCost###Decimal
this.lblSelfApportionedCost.AutoSize = true;
this.lblSelfApportionedCost.Location = new System.Drawing.Point(100,350);
this.lblSelfApportionedCost.Name = "lblSelfApportionedCost";
this.lblSelfApportionedCost.Size = new System.Drawing.Size(41, 12);
this.lblSelfApportionedCost.TabIndex = 14;
this.lblSelfApportionedCost.Text = "自制分摊费用";
//111======350
this.txtSelfApportionedCost.Location = new System.Drawing.Point(173,346);
this.txtSelfApportionedCost.Name ="txtSelfApportionedCost";
this.txtSelfApportionedCost.Size = new System.Drawing.Size(100, 21);
this.txtSelfApportionedCost.TabIndex = 14;
this.Controls.Add(this.lblSelfApportionedCost);
this.Controls.Add(this.txtSelfApportionedCost);

           //#####TotalSelfManuCost###Decimal
this.lblTotalSelfManuCost.AutoSize = true;
this.lblTotalSelfManuCost.Location = new System.Drawing.Point(100,375);
this.lblTotalSelfManuCost.Name = "lblTotalSelfManuCost";
this.lblTotalSelfManuCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalSelfManuCost.TabIndex = 15;
this.lblTotalSelfManuCost.Text = "自产制造费用";
//111======375
this.txtTotalSelfManuCost.Location = new System.Drawing.Point(173,371);
this.txtTotalSelfManuCost.Name ="txtTotalSelfManuCost";
this.txtTotalSelfManuCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalSelfManuCost.TabIndex = 15;
this.Controls.Add(this.lblTotalSelfManuCost);
this.Controls.Add(this.txtTotalSelfManuCost);

           //#####TotalOutManuCost###Decimal
this.lblTotalOutManuCost.AutoSize = true;
this.lblTotalOutManuCost.Location = new System.Drawing.Point(100,400);
this.lblTotalOutManuCost.Name = "lblTotalOutManuCost";
this.lblTotalOutManuCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalOutManuCost.TabIndex = 16;
this.lblTotalOutManuCost.Text = "外发费用";
//111======400
this.txtTotalOutManuCost.Location = new System.Drawing.Point(173,396);
this.txtTotalOutManuCost.Name ="txtTotalOutManuCost";
this.txtTotalOutManuCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalOutManuCost.TabIndex = 16;
this.Controls.Add(this.lblTotalOutManuCost);
this.Controls.Add(this.txtTotalOutManuCost);

           //#####TotalMaterialCost###Decimal
this.lblTotalMaterialCost.AutoSize = true;
this.lblTotalMaterialCost.Location = new System.Drawing.Point(100,425);
this.lblTotalMaterialCost.Name = "lblTotalMaterialCost";
this.lblTotalMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialCost.TabIndex = 17;
this.lblTotalMaterialCost.Text = "总物料费用";
//111======425
this.txtTotalMaterialCost.Location = new System.Drawing.Point(173,421);
this.txtTotalMaterialCost.Name ="txtTotalMaterialCost";
this.txtTotalMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialCost.TabIndex = 17;
this.Controls.Add(this.lblTotalMaterialCost);
this.Controls.Add(this.txtTotalMaterialCost);

           //#####TotalMaterialQty###Decimal
this.lblTotalMaterialQty.AutoSize = true;
this.lblTotalMaterialQty.Location = new System.Drawing.Point(100,450);
this.lblTotalMaterialQty.Name = "lblTotalMaterialQty";
this.lblTotalMaterialQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialQty.TabIndex = 18;
this.lblTotalMaterialQty.Text = "用料总量";
//111======450
this.txtTotalMaterialQty.Location = new System.Drawing.Point(173,446);
this.txtTotalMaterialQty.Name ="txtTotalMaterialQty";
this.txtTotalMaterialQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialQty.TabIndex = 18;
this.Controls.Add(this.lblTotalMaterialQty);
this.Controls.Add(this.txtTotalMaterialQty);

           //#####OutputQty###Decimal
this.lblOutputQty.AutoSize = true;
this.lblOutputQty.Location = new System.Drawing.Point(100,475);
this.lblOutputQty.Name = "lblOutputQty";
this.lblOutputQty.Size = new System.Drawing.Size(41, 12);
this.lblOutputQty.TabIndex = 19;
this.lblOutputQty.Text = "产出量";
//111======475
this.txtOutputQty.Location = new System.Drawing.Point(173,471);
this.txtOutputQty.Name ="txtOutputQty";
this.txtOutputQty.Size = new System.Drawing.Size(100, 21);
this.txtOutputQty.TabIndex = 19;
this.Controls.Add(this.lblOutputQty);
this.Controls.Add(this.txtOutputQty);

           //#####PeopleQty###Decimal
this.lblPeopleQty.AutoSize = true;
this.lblPeopleQty.Location = new System.Drawing.Point(100,500);
this.lblPeopleQty.Name = "lblPeopleQty";
this.lblPeopleQty.Size = new System.Drawing.Size(41, 12);
this.lblPeopleQty.TabIndex = 20;
this.lblPeopleQty.Text = "人数";
//111======500
this.txtPeopleQty.Location = new System.Drawing.Point(173,496);
this.txtPeopleQty.Name ="txtPeopleQty";
this.txtPeopleQty.Size = new System.Drawing.Size(100, 21);
this.txtPeopleQty.TabIndex = 20;
this.Controls.Add(this.lblPeopleQty);
this.Controls.Add(this.txtPeopleQty);

           //#####WorkingHour###Decimal
this.lblWorkingHour.AutoSize = true;
this.lblWorkingHour.Location = new System.Drawing.Point(100,525);
this.lblWorkingHour.Name = "lblWorkingHour";
this.lblWorkingHour.Size = new System.Drawing.Size(41, 12);
this.lblWorkingHour.TabIndex = 21;
this.lblWorkingHour.Text = "工时";
//111======525
this.txtWorkingHour.Location = new System.Drawing.Point(173,521);
this.txtWorkingHour.Name ="txtWorkingHour";
this.txtWorkingHour.Size = new System.Drawing.Size(100, 21);
this.txtWorkingHour.TabIndex = 21;
this.Controls.Add(this.lblWorkingHour);
this.Controls.Add(this.txtWorkingHour);

           //#####MachineHour###Decimal
this.lblMachineHour.AutoSize = true;
this.lblMachineHour.Location = new System.Drawing.Point(100,550);
this.lblMachineHour.Name = "lblMachineHour";
this.lblMachineHour.Size = new System.Drawing.Size(41, 12);
this.lblMachineHour.TabIndex = 22;
this.lblMachineHour.Text = "机时";
//111======550
this.txtMachineHour.Location = new System.Drawing.Point(173,546);
this.txtMachineHour.Name ="txtMachineHour";
this.txtMachineHour.Size = new System.Drawing.Size(100, 21);
this.txtMachineHour.TabIndex = 22;
this.Controls.Add(this.lblMachineHour);
this.Controls.Add(this.txtMachineHour);

           //#####ExpirationDate###DateTime
this.lblExpirationDate.AutoSize = true;
this.lblExpirationDate.Location = new System.Drawing.Point(100,575);
this.lblExpirationDate.Name = "lblExpirationDate";
this.lblExpirationDate.Size = new System.Drawing.Size(41, 12);
this.lblExpirationDate.TabIndex = 23;
this.lblExpirationDate.Text = "截止日期";
//111======575
this.dtpExpirationDate.Location = new System.Drawing.Point(173,571);
this.dtpExpirationDate.Name ="dtpExpirationDate";
this.dtpExpirationDate.ShowCheckBox =true;
this.dtpExpirationDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpirationDate.TabIndex = 23;
this.Controls.Add(this.lblExpirationDate);
this.Controls.Add(this.dtpExpirationDate);

           //#####DailyQty###Decimal
this.lblDailyQty.AutoSize = true;
this.lblDailyQty.Location = new System.Drawing.Point(100,600);
this.lblDailyQty.Name = "lblDailyQty";
this.lblDailyQty.Size = new System.Drawing.Size(41, 12);
this.lblDailyQty.TabIndex = 24;
this.lblDailyQty.Text = "日产量";
//111======600
this.txtDailyQty.Location = new System.Drawing.Point(173,596);
this.txtDailyQty.Name ="txtDailyQty";
this.txtDailyQty.Size = new System.Drawing.Size(100, 21);
this.txtDailyQty.TabIndex = 24;
this.Controls.Add(this.lblDailyQty);
this.Controls.Add(this.txtDailyQty);

           //#####SelfProductionAllCosts###Decimal
this.lblSelfProductionAllCosts.AutoSize = true;
this.lblSelfProductionAllCosts.Location = new System.Drawing.Point(100,625);
this.lblSelfProductionAllCosts.Name = "lblSelfProductionAllCosts";
this.lblSelfProductionAllCosts.Size = new System.Drawing.Size(41, 12);
this.lblSelfProductionAllCosts.TabIndex = 25;
this.lblSelfProductionAllCosts.Text = "自产总成本";
//111======625
this.txtSelfProductionAllCosts.Location = new System.Drawing.Point(173,621);
this.txtSelfProductionAllCosts.Name ="txtSelfProductionAllCosts";
this.txtSelfProductionAllCosts.Size = new System.Drawing.Size(100, 21);
this.txtSelfProductionAllCosts.TabIndex = 25;
this.Controls.Add(this.lblSelfProductionAllCosts);
this.Controls.Add(this.txtSelfProductionAllCosts);

           //#####OutProductionAllCosts###Decimal
this.lblOutProductionAllCosts.AutoSize = true;
this.lblOutProductionAllCosts.Location = new System.Drawing.Point(100,650);
this.lblOutProductionAllCosts.Name = "lblOutProductionAllCosts";
this.lblOutProductionAllCosts.Size = new System.Drawing.Size(41, 12);
this.lblOutProductionAllCosts.TabIndex = 26;
this.lblOutProductionAllCosts.Text = "外发总成本";
//111======650
this.txtOutProductionAllCosts.Location = new System.Drawing.Point(173,646);
this.txtOutProductionAllCosts.Name ="txtOutProductionAllCosts";
this.txtOutProductionAllCosts.Size = new System.Drawing.Size(100, 21);
this.txtOutProductionAllCosts.TabIndex = 26;
this.Controls.Add(this.lblOutProductionAllCosts);
this.Controls.Add(this.txtOutProductionAllCosts);

           //#####2147483647BOM_Iimage###Binary

           //#####500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,700);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 28;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,696);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 28;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,725);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 29;
this.lblCreated_at.Text = "创建时间";
//111======725
this.dtpCreated_at.Location = new System.Drawing.Point(173,721);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 29;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试750Created_by
//属性测试750Created_by
//属性测试750Created_by
//属性测试750Created_by
//属性测试750Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,775);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 31;
this.lblModified_at.Text = "修改时间";
//111======775
this.dtpModified_at.Location = new System.Drawing.Point(173,771);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 31;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试800Modified_by
//属性测试800Modified_by
//属性测试800Modified_by
//属性测试800Modified_by
//属性测试800Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,825);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 33;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,821);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 33;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试850DataStatus
//属性测试850DataStatus
//属性测试850DataStatus
//属性测试850DataStatus
//属性测试850DataStatus

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,875);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 35;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,871);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 35;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试900Approver_by
//属性测试900Approver_by
//属性测试900Approver_by
//属性测试900Approver_by
//属性测试900Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,925);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 37;
this.lblApprover_at.Text = "审批时间";
//111======925
this.dtpApprover_at.Location = new System.Drawing.Point(173,921);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 37;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,975);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 39;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,971);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 39;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

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

                this.Controls.Add(this.lblOutApportionedCost );
this.Controls.Add(this.txtOutApportionedCost );

                this.Controls.Add(this.lblSelfApportionedCost );
this.Controls.Add(this.txtSelfApportionedCost );

                this.Controls.Add(this.lblTotalSelfManuCost );
this.Controls.Add(this.txtTotalSelfManuCost );

                this.Controls.Add(this.lblTotalOutManuCost );
this.Controls.Add(this.txtTotalOutManuCost );

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

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                    
            this.Name = "tb_BOM_SQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_No;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBOM_No;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBOM_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDoc_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDoc_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_S_VERID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBOM_S_VERID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEffective_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEffective_at;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblis_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkis_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblis_available;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkis_available;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutApportionedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOutApportionedCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSelfApportionedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSelfApportionedCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalSelfManuCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalSelfManuCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalOutManuCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalOutManuCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalMaterialCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalMaterialCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalMaterialQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalMaterialQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutputQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOutputQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPeopleQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPeopleQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWorkingHour;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWorkingHour;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMachineHour;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMachineHour;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpirationDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpExpirationDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDailyQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDailyQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSelfProductionAllCosts;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSelfProductionAllCosts;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutProductionAllCosts;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOutProductionAllCosts;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
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


