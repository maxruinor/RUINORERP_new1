
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:27
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 
    /// </summary>
    partial class View_BOMQuery
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

this.lblBOM_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBOM_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;






this.lblEffective_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEffective_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblis_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkis_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkis_enabled.Values.Text ="";

this.lblis_available = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkis_available = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkis_available.Values.Text ="";

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
                 //#####BOM_ID###Int64

           //#####50BOM_No###String
this.lblBOM_No.AutoSize = true;
this.lblBOM_No.Location = new System.Drawing.Point(100,50);
this.lblBOM_No.Name = "lblBOM_No";
this.lblBOM_No.Size = new System.Drawing.Size(41, 12);
this.lblBOM_No.TabIndex = 2;
this.lblBOM_No.Text = "";
this.txtBOM_No.Location = new System.Drawing.Point(173,46);
this.txtBOM_No.Name = "txtBOM_No";
this.txtBOM_No.Size = new System.Drawing.Size(100, 21);
this.txtBOM_No.TabIndex = 2;
this.Controls.Add(this.lblBOM_No);
this.Controls.Add(this.txtBOM_No);

           //#####100BOM_Name###String
this.lblBOM_Name.AutoSize = true;
this.lblBOM_Name.Location = new System.Drawing.Point(100,75);
this.lblBOM_Name.Name = "lblBOM_Name";
this.lblBOM_Name.Size = new System.Drawing.Size(41, 12);
this.lblBOM_Name.TabIndex = 3;
this.lblBOM_Name.Text = "";
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
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,96);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 4;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####Type_ID###Int64

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,150);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 6;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,146);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 6;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####Employee_ID###Int64

           //#####ProdDetailID###Int64

           //#####DepartmentID###Int64

           //#####Doc_ID###Int64

           //#####BOM_S_VERID###Int64

           //#####Effective_at###DateTime
this.lblEffective_at.AutoSize = true;
this.lblEffective_at.Location = new System.Drawing.Point(100,300);
this.lblEffective_at.Name = "lblEffective_at";
this.lblEffective_at.Size = new System.Drawing.Size(41, 12);
this.lblEffective_at.TabIndex = 12;
this.lblEffective_at.Text = "";
//111======300
this.dtpEffective_at.Location = new System.Drawing.Point(173,296);
this.dtpEffective_at.Name ="dtpEffective_at";
this.dtpEffective_at.Size = new System.Drawing.Size(100, 21);
this.dtpEffective_at.TabIndex = 12;
this.Controls.Add(this.lblEffective_at);
this.Controls.Add(this.dtpEffective_at);

           //#####is_enabled###Boolean
this.lblis_enabled.AutoSize = true;
this.lblis_enabled.Location = new System.Drawing.Point(100,325);
this.lblis_enabled.Name = "lblis_enabled";
this.lblis_enabled.Size = new System.Drawing.Size(41, 12);
this.lblis_enabled.TabIndex = 13;
this.lblis_enabled.Text = "";
this.chkis_enabled.Location = new System.Drawing.Point(173,321);
this.chkis_enabled.Name = "chkis_enabled";
this.chkis_enabled.Size = new System.Drawing.Size(100, 21);
this.chkis_enabled.TabIndex = 13;
this.Controls.Add(this.lblis_enabled);
this.Controls.Add(this.chkis_enabled);

           //#####is_available###Boolean
this.lblis_available.AutoSize = true;
this.lblis_available.Location = new System.Drawing.Point(100,350);
this.lblis_available.Name = "lblis_available";
this.lblis_available.Size = new System.Drawing.Size(41, 12);
this.lblis_available.TabIndex = 14;
this.lblis_available.Text = "";
this.chkis_available.Location = new System.Drawing.Point(173,346);
this.chkis_available.Name = "chkis_available";
this.chkis_available.Size = new System.Drawing.Size(100, 21);
this.chkis_available.TabIndex = 14;
this.Controls.Add(this.lblis_available);
this.Controls.Add(this.chkis_available);

           //#####OutApportionedCost###Decimal
this.lblOutApportionedCost.AutoSize = true;
this.lblOutApportionedCost.Location = new System.Drawing.Point(100,375);
this.lblOutApportionedCost.Name = "lblOutApportionedCost";
this.lblOutApportionedCost.Size = new System.Drawing.Size(41, 12);
this.lblOutApportionedCost.TabIndex = 15;
this.lblOutApportionedCost.Text = "";
//111======375
this.txtOutApportionedCost.Location = new System.Drawing.Point(173,371);
this.txtOutApportionedCost.Name ="txtOutApportionedCost";
this.txtOutApportionedCost.Size = new System.Drawing.Size(100, 21);
this.txtOutApportionedCost.TabIndex = 15;
this.Controls.Add(this.lblOutApportionedCost);
this.Controls.Add(this.txtOutApportionedCost);

           //#####SelfApportionedCost###Decimal
this.lblSelfApportionedCost.AutoSize = true;
this.lblSelfApportionedCost.Location = new System.Drawing.Point(100,400);
this.lblSelfApportionedCost.Name = "lblSelfApportionedCost";
this.lblSelfApportionedCost.Size = new System.Drawing.Size(41, 12);
this.lblSelfApportionedCost.TabIndex = 16;
this.lblSelfApportionedCost.Text = "";
//111======400
this.txtSelfApportionedCost.Location = new System.Drawing.Point(173,396);
this.txtSelfApportionedCost.Name ="txtSelfApportionedCost";
this.txtSelfApportionedCost.Size = new System.Drawing.Size(100, 21);
this.txtSelfApportionedCost.TabIndex = 16;
this.Controls.Add(this.lblSelfApportionedCost);
this.Controls.Add(this.txtSelfApportionedCost);

           //#####TotalSelfManuCost###Decimal
this.lblTotalSelfManuCost.AutoSize = true;
this.lblTotalSelfManuCost.Location = new System.Drawing.Point(100,425);
this.lblTotalSelfManuCost.Name = "lblTotalSelfManuCost";
this.lblTotalSelfManuCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalSelfManuCost.TabIndex = 17;
this.lblTotalSelfManuCost.Text = "";
//111======425
this.txtTotalSelfManuCost.Location = new System.Drawing.Point(173,421);
this.txtTotalSelfManuCost.Name ="txtTotalSelfManuCost";
this.txtTotalSelfManuCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalSelfManuCost.TabIndex = 17;
this.Controls.Add(this.lblTotalSelfManuCost);
this.Controls.Add(this.txtTotalSelfManuCost);

           //#####TotalOutManuCost###Decimal
this.lblTotalOutManuCost.AutoSize = true;
this.lblTotalOutManuCost.Location = new System.Drawing.Point(100,450);
this.lblTotalOutManuCost.Name = "lblTotalOutManuCost";
this.lblTotalOutManuCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalOutManuCost.TabIndex = 18;
this.lblTotalOutManuCost.Text = "";
//111======450
this.txtTotalOutManuCost.Location = new System.Drawing.Point(173,446);
this.txtTotalOutManuCost.Name ="txtTotalOutManuCost";
this.txtTotalOutManuCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalOutManuCost.TabIndex = 18;
this.Controls.Add(this.lblTotalOutManuCost);
this.Controls.Add(this.txtTotalOutManuCost);

           //#####TotalMaterialCost###Decimal
this.lblTotalMaterialCost.AutoSize = true;
this.lblTotalMaterialCost.Location = new System.Drawing.Point(100,475);
this.lblTotalMaterialCost.Name = "lblTotalMaterialCost";
this.lblTotalMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialCost.TabIndex = 19;
this.lblTotalMaterialCost.Text = "";
//111======475
this.txtTotalMaterialCost.Location = new System.Drawing.Point(173,471);
this.txtTotalMaterialCost.Name ="txtTotalMaterialCost";
this.txtTotalMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialCost.TabIndex = 19;
this.Controls.Add(this.lblTotalMaterialCost);
this.Controls.Add(this.txtTotalMaterialCost);

           //#####TotalMaterialQty###Decimal
this.lblTotalMaterialQty.AutoSize = true;
this.lblTotalMaterialQty.Location = new System.Drawing.Point(100,500);
this.lblTotalMaterialQty.Name = "lblTotalMaterialQty";
this.lblTotalMaterialQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalMaterialQty.TabIndex = 20;
this.lblTotalMaterialQty.Text = "";
//111======500
this.txtTotalMaterialQty.Location = new System.Drawing.Point(173,496);
this.txtTotalMaterialQty.Name ="txtTotalMaterialQty";
this.txtTotalMaterialQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalMaterialQty.TabIndex = 20;
this.Controls.Add(this.lblTotalMaterialQty);
this.Controls.Add(this.txtTotalMaterialQty);

           //#####OutputQty###Decimal
this.lblOutputQty.AutoSize = true;
this.lblOutputQty.Location = new System.Drawing.Point(100,525);
this.lblOutputQty.Name = "lblOutputQty";
this.lblOutputQty.Size = new System.Drawing.Size(41, 12);
this.lblOutputQty.TabIndex = 21;
this.lblOutputQty.Text = "";
//111======525
this.txtOutputQty.Location = new System.Drawing.Point(173,521);
this.txtOutputQty.Name ="txtOutputQty";
this.txtOutputQty.Size = new System.Drawing.Size(100, 21);
this.txtOutputQty.TabIndex = 21;
this.Controls.Add(this.lblOutputQty);
this.Controls.Add(this.txtOutputQty);

           //#####PeopleQty###Decimal
this.lblPeopleQty.AutoSize = true;
this.lblPeopleQty.Location = new System.Drawing.Point(100,550);
this.lblPeopleQty.Name = "lblPeopleQty";
this.lblPeopleQty.Size = new System.Drawing.Size(41, 12);
this.lblPeopleQty.TabIndex = 22;
this.lblPeopleQty.Text = "";
//111======550
this.txtPeopleQty.Location = new System.Drawing.Point(173,546);
this.txtPeopleQty.Name ="txtPeopleQty";
this.txtPeopleQty.Size = new System.Drawing.Size(100, 21);
this.txtPeopleQty.TabIndex = 22;
this.Controls.Add(this.lblPeopleQty);
this.Controls.Add(this.txtPeopleQty);

           //#####WorkingHour###Decimal
this.lblWorkingHour.AutoSize = true;
this.lblWorkingHour.Location = new System.Drawing.Point(100,575);
this.lblWorkingHour.Name = "lblWorkingHour";
this.lblWorkingHour.Size = new System.Drawing.Size(41, 12);
this.lblWorkingHour.TabIndex = 23;
this.lblWorkingHour.Text = "";
//111======575
this.txtWorkingHour.Location = new System.Drawing.Point(173,571);
this.txtWorkingHour.Name ="txtWorkingHour";
this.txtWorkingHour.Size = new System.Drawing.Size(100, 21);
this.txtWorkingHour.TabIndex = 23;
this.Controls.Add(this.lblWorkingHour);
this.Controls.Add(this.txtWorkingHour);

           //#####MachineHour###Decimal
this.lblMachineHour.AutoSize = true;
this.lblMachineHour.Location = new System.Drawing.Point(100,600);
this.lblMachineHour.Name = "lblMachineHour";
this.lblMachineHour.Size = new System.Drawing.Size(41, 12);
this.lblMachineHour.TabIndex = 24;
this.lblMachineHour.Text = "";
//111======600
this.txtMachineHour.Location = new System.Drawing.Point(173,596);
this.txtMachineHour.Name ="txtMachineHour";
this.txtMachineHour.Size = new System.Drawing.Size(100, 21);
this.txtMachineHour.TabIndex = 24;
this.Controls.Add(this.lblMachineHour);
this.Controls.Add(this.txtMachineHour);

           //#####ExpirationDate###DateTime
this.lblExpirationDate.AutoSize = true;
this.lblExpirationDate.Location = new System.Drawing.Point(100,625);
this.lblExpirationDate.Name = "lblExpirationDate";
this.lblExpirationDate.Size = new System.Drawing.Size(41, 12);
this.lblExpirationDate.TabIndex = 25;
this.lblExpirationDate.Text = "";
//111======625
this.dtpExpirationDate.Location = new System.Drawing.Point(173,621);
this.dtpExpirationDate.Name ="dtpExpirationDate";
this.dtpExpirationDate.ShowCheckBox =true;
this.dtpExpirationDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpirationDate.TabIndex = 25;
this.Controls.Add(this.lblExpirationDate);
this.Controls.Add(this.dtpExpirationDate);

           //#####DailyQty###Decimal
this.lblDailyQty.AutoSize = true;
this.lblDailyQty.Location = new System.Drawing.Point(100,650);
this.lblDailyQty.Name = "lblDailyQty";
this.lblDailyQty.Size = new System.Drawing.Size(41, 12);
this.lblDailyQty.TabIndex = 26;
this.lblDailyQty.Text = "";
//111======650
this.txtDailyQty.Location = new System.Drawing.Point(173,646);
this.txtDailyQty.Name ="txtDailyQty";
this.txtDailyQty.Size = new System.Drawing.Size(100, 21);
this.txtDailyQty.TabIndex = 26;
this.Controls.Add(this.lblDailyQty);
this.Controls.Add(this.txtDailyQty);

           //#####SelfProductionAllCosts###Decimal
this.lblSelfProductionAllCosts.AutoSize = true;
this.lblSelfProductionAllCosts.Location = new System.Drawing.Point(100,675);
this.lblSelfProductionAllCosts.Name = "lblSelfProductionAllCosts";
this.lblSelfProductionAllCosts.Size = new System.Drawing.Size(41, 12);
this.lblSelfProductionAllCosts.TabIndex = 27;
this.lblSelfProductionAllCosts.Text = "";
//111======675
this.txtSelfProductionAllCosts.Location = new System.Drawing.Point(173,671);
this.txtSelfProductionAllCosts.Name ="txtSelfProductionAllCosts";
this.txtSelfProductionAllCosts.Size = new System.Drawing.Size(100, 21);
this.txtSelfProductionAllCosts.TabIndex = 27;
this.Controls.Add(this.lblSelfProductionAllCosts);
this.Controls.Add(this.txtSelfProductionAllCosts);

           //#####OutProductionAllCosts###Decimal
this.lblOutProductionAllCosts.AutoSize = true;
this.lblOutProductionAllCosts.Location = new System.Drawing.Point(100,700);
this.lblOutProductionAllCosts.Name = "lblOutProductionAllCosts";
this.lblOutProductionAllCosts.Size = new System.Drawing.Size(41, 12);
this.lblOutProductionAllCosts.TabIndex = 28;
this.lblOutProductionAllCosts.Text = "";
//111======700
this.txtOutProductionAllCosts.Location = new System.Drawing.Point(173,696);
this.txtOutProductionAllCosts.Name ="txtOutProductionAllCosts";
this.txtOutProductionAllCosts.Size = new System.Drawing.Size(100, 21);
this.txtOutProductionAllCosts.TabIndex = 28;
this.Controls.Add(this.lblOutProductionAllCosts);
this.Controls.Add(this.txtOutProductionAllCosts);

           //#####2147483647BOM_Iimage###Binary

           //#####500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,750);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 30;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,746);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 30;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,775);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 31;
this.lblCreated_at.Text = "";
//111======775
this.dtpCreated_at.Location = new System.Drawing.Point(173,771);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 31;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,825);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 33;
this.lblModified_at.Text = "";
//111======825
this.dtpModified_at.Location = new System.Drawing.Point(173,821);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 33;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,875);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 35;
this.lblisdeleted.Text = "";
this.chkisdeleted.Location = new System.Drawing.Point(173,871);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 35;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,925);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 37;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,921);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 37;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,975);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 39;
this.lblApprover_at.Text = "";
//111======975
this.dtpApprover_at.Location = new System.Drawing.Point(173,971);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 39;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,1025);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 41;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,1021);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 41;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblBOM_No );
this.Controls.Add(this.txtBOM_No );

                this.Controls.Add(this.lblBOM_Name );
this.Controls.Add(this.txtBOM_Name );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                
                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                
                
                
                
                
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

                    
            this.Name = "View_BOMQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_No;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBOM_No;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBOM_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBOM_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
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


