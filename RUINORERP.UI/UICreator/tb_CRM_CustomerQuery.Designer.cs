
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:18
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 目标客户-公海客户CRM系统中使用，给成交客户作外键引用
    /// </summary>
    partial class tb_CRM_CustomerQuery
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
     
     this.lblCustomerName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLeadID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLeadID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRegion_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRegion_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProvinceID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProvinceID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCityID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCityID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblwwSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtwwSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact_Email = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact_Email = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact_Phone = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact_Phone = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCustomerAddress.Multiline = true;

this.lblRepeatCustomer = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkRepeatCustomer = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkRepeatCustomer.Values.Text ="";


this.lblCustomerTags = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerTags = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCustomerTags.Multiline = true;

this.lblCoreProductInfo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCoreProductInfo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblGetCustomerSource = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtGetCustomerSource = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtGetCustomerSource.Multiline = true;

this.lblSalePlatform = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSalePlatform = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWebsite = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWebsite = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtWebsite.Multiline = true;



this.lblTotalPurchaseAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalPurchaseAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblLastPurchaseDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLastPurchaseDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblFirstPurchaseDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpFirstPurchaseDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblConverted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkConverted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkConverted.Values.Text ="";

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

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50CustomerName###String
this.lblCustomerName.AutoSize = true;
this.lblCustomerName.Location = new System.Drawing.Point(100,25);
this.lblCustomerName.Name = "lblCustomerName";
this.lblCustomerName.Size = new System.Drawing.Size(41, 12);
this.lblCustomerName.TabIndex = 1;
this.lblCustomerName.Text = "客户名称";
this.txtCustomerName.Location = new System.Drawing.Point(173,21);
this.txtCustomerName.Name = "txtCustomerName";
this.txtCustomerName.Size = new System.Drawing.Size(100, 21);
this.txtCustomerName.TabIndex = 1;
this.Controls.Add(this.lblCustomerName);
this.Controls.Add(this.txtCustomerName);

           //#####Employee_ID###Int64
//属性测试50Employee_ID
//属性测试50Employee_ID
//属性测试50Employee_ID
//属性测试50Employee_ID
//属性测试50Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "对接人";
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
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,75);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 3;
this.lblDepartmentID.Text = "部门";
//111======75
this.cmbDepartmentID.Location = new System.Drawing.Point(173,71);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 3;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####LeadID###Int64
//属性测试100LeadID
//属性测试100LeadID
//属性测试100LeadID
//属性测试100LeadID
this.lblLeadID.AutoSize = true;
this.lblLeadID.Location = new System.Drawing.Point(100,100);
this.lblLeadID.Name = "lblLeadID";
this.lblLeadID.Size = new System.Drawing.Size(41, 12);
this.lblLeadID.TabIndex = 4;
this.lblLeadID.Text = "线索";
//111======100
this.cmbLeadID.Location = new System.Drawing.Point(173,96);
this.cmbLeadID.Name ="cmbLeadID";
this.cmbLeadID.Size = new System.Drawing.Size(100, 21);
this.cmbLeadID.TabIndex = 4;
this.Controls.Add(this.lblLeadID);
this.Controls.Add(this.cmbLeadID);

           //#####Region_ID###Int64
//属性测试125Region_ID
this.lblRegion_ID.AutoSize = true;
this.lblRegion_ID.Location = new System.Drawing.Point(100,125);
this.lblRegion_ID.Name = "lblRegion_ID";
this.lblRegion_ID.Size = new System.Drawing.Size(41, 12);
this.lblRegion_ID.TabIndex = 5;
this.lblRegion_ID.Text = "地区";
//111======125
this.cmbRegion_ID.Location = new System.Drawing.Point(173,121);
this.cmbRegion_ID.Name ="cmbRegion_ID";
this.cmbRegion_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRegion_ID.TabIndex = 5;
this.Controls.Add(this.lblRegion_ID);
this.Controls.Add(this.cmbRegion_ID);

           //#####ProvinceID###Int64
//属性测试150ProvinceID
//属性测试150ProvinceID
//属性测试150ProvinceID
//属性测试150ProvinceID
//属性测试150ProvinceID
//属性测试150ProvinceID
this.lblProvinceID.AutoSize = true;
this.lblProvinceID.Location = new System.Drawing.Point(100,150);
this.lblProvinceID.Name = "lblProvinceID";
this.lblProvinceID.Size = new System.Drawing.Size(41, 12);
this.lblProvinceID.TabIndex = 6;
this.lblProvinceID.Text = "省";
//111======150
this.cmbProvinceID.Location = new System.Drawing.Point(173,146);
this.cmbProvinceID.Name ="cmbProvinceID";
this.cmbProvinceID.Size = new System.Drawing.Size(100, 21);
this.cmbProvinceID.TabIndex = 6;
this.Controls.Add(this.lblProvinceID);
this.Controls.Add(this.cmbProvinceID);

           //#####CityID###Int64
//属性测试175CityID
//属性测试175CityID
//属性测试175CityID
this.lblCityID.AutoSize = true;
this.lblCityID.Location = new System.Drawing.Point(100,175);
this.lblCityID.Name = "lblCityID";
this.lblCityID.Size = new System.Drawing.Size(41, 12);
this.lblCityID.TabIndex = 7;
this.lblCityID.Text = "城市";
//111======175
this.cmbCityID.Location = new System.Drawing.Point(173,171);
this.cmbCityID.Name ="cmbCityID";
this.cmbCityID.Size = new System.Drawing.Size(100, 21);
this.cmbCityID.TabIndex = 7;
this.Controls.Add(this.lblCityID);
this.Controls.Add(this.cmbCityID);

           //#####200wwSocialTools###String
this.lblwwSocialTools.AutoSize = true;
this.lblwwSocialTools.Location = new System.Drawing.Point(100,200);
this.lblwwSocialTools.Name = "lblwwSocialTools";
this.lblwwSocialTools.Size = new System.Drawing.Size(41, 12);
this.lblwwSocialTools.TabIndex = 8;
this.lblwwSocialTools.Text = "旺旺/IM工具";
this.txtwwSocialTools.Location = new System.Drawing.Point(173,196);
this.txtwwSocialTools.Name = "txtwwSocialTools";
this.txtwwSocialTools.Size = new System.Drawing.Size(100, 21);
this.txtwwSocialTools.TabIndex = 8;
this.Controls.Add(this.lblwwSocialTools);
this.Controls.Add(this.txtwwSocialTools);

           //#####200SocialTools###String
this.lblSocialTools.AutoSize = true;
this.lblSocialTools.Location = new System.Drawing.Point(100,225);
this.lblSocialTools.Name = "lblSocialTools";
this.lblSocialTools.Size = new System.Drawing.Size(41, 12);
this.lblSocialTools.TabIndex = 9;
this.lblSocialTools.Text = "其他/IM工具";
this.txtSocialTools.Location = new System.Drawing.Point(173,221);
this.txtSocialTools.Name = "txtSocialTools";
this.txtSocialTools.Size = new System.Drawing.Size(100, 21);
this.txtSocialTools.TabIndex = 9;
this.Controls.Add(this.lblSocialTools);
this.Controls.Add(this.txtSocialTools);

           //#####50Contact_Name###String
this.lblContact_Name.AutoSize = true;
this.lblContact_Name.Location = new System.Drawing.Point(100,250);
this.lblContact_Name.Name = "lblContact_Name";
this.lblContact_Name.Size = new System.Drawing.Size(41, 12);
this.lblContact_Name.TabIndex = 10;
this.lblContact_Name.Text = "联系人姓名";
this.txtContact_Name.Location = new System.Drawing.Point(173,246);
this.txtContact_Name.Name = "txtContact_Name";
this.txtContact_Name.Size = new System.Drawing.Size(100, 21);
this.txtContact_Name.TabIndex = 10;
this.Controls.Add(this.lblContact_Name);
this.Controls.Add(this.txtContact_Name);

           //#####100Contact_Email###String
this.lblContact_Email.AutoSize = true;
this.lblContact_Email.Location = new System.Drawing.Point(100,275);
this.lblContact_Email.Name = "lblContact_Email";
this.lblContact_Email.Size = new System.Drawing.Size(41, 12);
this.lblContact_Email.TabIndex = 11;
this.lblContact_Email.Text = "邮箱";
this.txtContact_Email.Location = new System.Drawing.Point(173,271);
this.txtContact_Email.Name = "txtContact_Email";
this.txtContact_Email.Size = new System.Drawing.Size(100, 21);
this.txtContact_Email.TabIndex = 11;
this.Controls.Add(this.lblContact_Email);
this.Controls.Add(this.txtContact_Email);

           //#####30Contact_Phone###String
this.lblContact_Phone.AutoSize = true;
this.lblContact_Phone.Location = new System.Drawing.Point(100,300);
this.lblContact_Phone.Name = "lblContact_Phone";
this.lblContact_Phone.Size = new System.Drawing.Size(41, 12);
this.lblContact_Phone.TabIndex = 12;
this.lblContact_Phone.Text = "电话";
this.txtContact_Phone.Location = new System.Drawing.Point(173,296);
this.txtContact_Phone.Name = "txtContact_Phone";
this.txtContact_Phone.Size = new System.Drawing.Size(100, 21);
this.txtContact_Phone.TabIndex = 12;
this.Controls.Add(this.lblContact_Phone);
this.Controls.Add(this.txtContact_Phone);

           //#####300CustomerAddress###String
this.lblCustomerAddress.AutoSize = true;
this.lblCustomerAddress.Location = new System.Drawing.Point(100,325);
this.lblCustomerAddress.Name = "lblCustomerAddress";
this.lblCustomerAddress.Size = new System.Drawing.Size(41, 12);
this.lblCustomerAddress.TabIndex = 13;
this.lblCustomerAddress.Text = "客户地址";
this.txtCustomerAddress.Location = new System.Drawing.Point(173,321);
this.txtCustomerAddress.Name = "txtCustomerAddress";
this.txtCustomerAddress.Size = new System.Drawing.Size(100, 21);
this.txtCustomerAddress.TabIndex = 13;
this.Controls.Add(this.lblCustomerAddress);
this.Controls.Add(this.txtCustomerAddress);

           //#####RepeatCustomer###Boolean
this.lblRepeatCustomer.AutoSize = true;
this.lblRepeatCustomer.Location = new System.Drawing.Point(100,350);
this.lblRepeatCustomer.Name = "lblRepeatCustomer";
this.lblRepeatCustomer.Size = new System.Drawing.Size(41, 12);
this.lblRepeatCustomer.TabIndex = 14;
this.lblRepeatCustomer.Text = "重复客户";
this.chkRepeatCustomer.Location = new System.Drawing.Point(173,346);
this.chkRepeatCustomer.Name = "chkRepeatCustomer";
this.chkRepeatCustomer.Size = new System.Drawing.Size(100, 21);
this.chkRepeatCustomer.TabIndex = 14;
this.Controls.Add(this.lblRepeatCustomer);
this.Controls.Add(this.chkRepeatCustomer);

           //#####CustomerStatus###Int32
//属性测试375CustomerStatus
//属性测试375CustomerStatus
//属性测试375CustomerStatus
//属性测试375CustomerStatus
//属性测试375CustomerStatus
//属性测试375CustomerStatus

           //#####500CustomerTags###String
this.lblCustomerTags.AutoSize = true;
this.lblCustomerTags.Location = new System.Drawing.Point(100,400);
this.lblCustomerTags.Name = "lblCustomerTags";
this.lblCustomerTags.Size = new System.Drawing.Size(41, 12);
this.lblCustomerTags.TabIndex = 16;
this.lblCustomerTags.Text = "客户标签";
this.txtCustomerTags.Location = new System.Drawing.Point(173,396);
this.txtCustomerTags.Name = "txtCustomerTags";
this.txtCustomerTags.Size = new System.Drawing.Size(100, 21);
this.txtCustomerTags.TabIndex = 16;
this.Controls.Add(this.lblCustomerTags);
this.Controls.Add(this.txtCustomerTags);

           //#####200CoreProductInfo###String
this.lblCoreProductInfo.AutoSize = true;
this.lblCoreProductInfo.Location = new System.Drawing.Point(100,425);
this.lblCoreProductInfo.Name = "lblCoreProductInfo";
this.lblCoreProductInfo.Size = new System.Drawing.Size(41, 12);
this.lblCoreProductInfo.TabIndex = 17;
this.lblCoreProductInfo.Text = "获客来源";
this.txtCoreProductInfo.Location = new System.Drawing.Point(173,421);
this.txtCoreProductInfo.Name = "txtCoreProductInfo";
this.txtCoreProductInfo.Size = new System.Drawing.Size(100, 21);
this.txtCoreProductInfo.TabIndex = 17;
this.Controls.Add(this.lblCoreProductInfo);
this.Controls.Add(this.txtCoreProductInfo);

           //#####250GetCustomerSource###String
this.lblGetCustomerSource.AutoSize = true;
this.lblGetCustomerSource.Location = new System.Drawing.Point(100,450);
this.lblGetCustomerSource.Name = "lblGetCustomerSource";
this.lblGetCustomerSource.Size = new System.Drawing.Size(41, 12);
this.lblGetCustomerSource.TabIndex = 18;
this.lblGetCustomerSource.Text = "主营产品信息";
this.txtGetCustomerSource.Location = new System.Drawing.Point(173,446);
this.txtGetCustomerSource.Name = "txtGetCustomerSource";
this.txtGetCustomerSource.Size = new System.Drawing.Size(100, 21);
this.txtGetCustomerSource.TabIndex = 18;
this.Controls.Add(this.lblGetCustomerSource);
this.Controls.Add(this.txtGetCustomerSource);

           //#####50SalePlatform###String
this.lblSalePlatform.AutoSize = true;
this.lblSalePlatform.Location = new System.Drawing.Point(100,475);
this.lblSalePlatform.Name = "lblSalePlatform";
this.lblSalePlatform.Size = new System.Drawing.Size(41, 12);
this.lblSalePlatform.TabIndex = 19;
this.lblSalePlatform.Text = "销售平台";
this.txtSalePlatform.Location = new System.Drawing.Point(173,471);
this.txtSalePlatform.Name = "txtSalePlatform";
this.txtSalePlatform.Size = new System.Drawing.Size(100, 21);
this.txtSalePlatform.TabIndex = 19;
this.Controls.Add(this.lblSalePlatform);
this.Controls.Add(this.txtSalePlatform);

           //#####255Website###String
this.lblWebsite.AutoSize = true;
this.lblWebsite.Location = new System.Drawing.Point(100,500);
this.lblWebsite.Name = "lblWebsite";
this.lblWebsite.Size = new System.Drawing.Size(41, 12);
this.lblWebsite.TabIndex = 20;
this.lblWebsite.Text = "网址";
this.txtWebsite.Location = new System.Drawing.Point(173,496);
this.txtWebsite.Name = "txtWebsite";
this.txtWebsite.Size = new System.Drawing.Size(100, 21);
this.txtWebsite.TabIndex = 20;
this.Controls.Add(this.lblWebsite);
this.Controls.Add(this.txtWebsite);

           //#####CustomerLevel###Int32
//属性测试525CustomerLevel
//属性测试525CustomerLevel
//属性测试525CustomerLevel
//属性测试525CustomerLevel
//属性测试525CustomerLevel
//属性测试525CustomerLevel

           //#####PurchaseCount###Int32
//属性测试550PurchaseCount
//属性测试550PurchaseCount
//属性测试550PurchaseCount
//属性测试550PurchaseCount
//属性测试550PurchaseCount
//属性测试550PurchaseCount

           //#####TotalPurchaseAmount###Decimal
this.lblTotalPurchaseAmount.AutoSize = true;
this.lblTotalPurchaseAmount.Location = new System.Drawing.Point(100,575);
this.lblTotalPurchaseAmount.Name = "lblTotalPurchaseAmount";
this.lblTotalPurchaseAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalPurchaseAmount.TabIndex = 23;
this.lblTotalPurchaseAmount.Text = "采购金额";
//111======575
this.txtTotalPurchaseAmount.Location = new System.Drawing.Point(173,571);
this.txtTotalPurchaseAmount.Name ="txtTotalPurchaseAmount";
this.txtTotalPurchaseAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalPurchaseAmount.TabIndex = 23;
this.Controls.Add(this.lblTotalPurchaseAmount);
this.Controls.Add(this.txtTotalPurchaseAmount);

           //#####DaysSinceLastPurchase###Int32
//属性测试600DaysSinceLastPurchase
//属性测试600DaysSinceLastPurchase
//属性测试600DaysSinceLastPurchase
//属性测试600DaysSinceLastPurchase
//属性测试600DaysSinceLastPurchase
//属性测试600DaysSinceLastPurchase

           //#####LastPurchaseDate###DateTime
this.lblLastPurchaseDate.AutoSize = true;
this.lblLastPurchaseDate.Location = new System.Drawing.Point(100,625);
this.lblLastPurchaseDate.Name = "lblLastPurchaseDate";
this.lblLastPurchaseDate.Size = new System.Drawing.Size(41, 12);
this.lblLastPurchaseDate.TabIndex = 25;
this.lblLastPurchaseDate.Text = "最近采购日期";
//111======625
this.dtpLastPurchaseDate.Location = new System.Drawing.Point(173,621);
this.dtpLastPurchaseDate.Name ="dtpLastPurchaseDate";
this.dtpLastPurchaseDate.ShowCheckBox =true;
this.dtpLastPurchaseDate.Size = new System.Drawing.Size(100, 21);
this.dtpLastPurchaseDate.TabIndex = 25;
this.Controls.Add(this.lblLastPurchaseDate);
this.Controls.Add(this.dtpLastPurchaseDate);

           //#####FirstPurchaseDate###DateTime
this.lblFirstPurchaseDate.AutoSize = true;
this.lblFirstPurchaseDate.Location = new System.Drawing.Point(100,650);
this.lblFirstPurchaseDate.Name = "lblFirstPurchaseDate";
this.lblFirstPurchaseDate.Size = new System.Drawing.Size(41, 12);
this.lblFirstPurchaseDate.TabIndex = 26;
this.lblFirstPurchaseDate.Text = "首次采购日期";
//111======650
this.dtpFirstPurchaseDate.Location = new System.Drawing.Point(173,646);
this.dtpFirstPurchaseDate.Name ="dtpFirstPurchaseDate";
this.dtpFirstPurchaseDate.ShowCheckBox =true;
this.dtpFirstPurchaseDate.Size = new System.Drawing.Size(100, 21);
this.dtpFirstPurchaseDate.TabIndex = 26;
this.Controls.Add(this.lblFirstPurchaseDate);
this.Controls.Add(this.dtpFirstPurchaseDate);

           //#####Converted###Boolean
this.lblConverted.AutoSize = true;
this.lblConverted.Location = new System.Drawing.Point(100,675);
this.lblConverted.Name = "lblConverted";
this.lblConverted.Size = new System.Drawing.Size(41, 12);
this.lblConverted.TabIndex = 27;
this.lblConverted.Text = "已转化";
this.chkConverted.Location = new System.Drawing.Point(173,671);
this.chkConverted.Name = "chkConverted";
this.chkConverted.Size = new System.Drawing.Size(100, 21);
this.chkConverted.TabIndex = 27;
this.Controls.Add(this.lblConverted);
this.Controls.Add(this.chkConverted);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,700);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 28;
this.lblNotes.Text = "备注";
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCustomerName );
this.Controls.Add(this.txtCustomerName );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblLeadID );
this.Controls.Add(this.cmbLeadID );

                this.Controls.Add(this.lblRegion_ID );
this.Controls.Add(this.cmbRegion_ID );

                this.Controls.Add(this.lblProvinceID );
this.Controls.Add(this.cmbProvinceID );

                this.Controls.Add(this.lblCityID );
this.Controls.Add(this.cmbCityID );

                this.Controls.Add(this.lblwwSocialTools );
this.Controls.Add(this.txtwwSocialTools );

                this.Controls.Add(this.lblSocialTools );
this.Controls.Add(this.txtSocialTools );

                this.Controls.Add(this.lblContact_Name );
this.Controls.Add(this.txtContact_Name );

                this.Controls.Add(this.lblContact_Email );
this.Controls.Add(this.txtContact_Email );

                this.Controls.Add(this.lblContact_Phone );
this.Controls.Add(this.txtContact_Phone );

                this.Controls.Add(this.lblCustomerAddress );
this.Controls.Add(this.txtCustomerAddress );

                this.Controls.Add(this.lblRepeatCustomer );
this.Controls.Add(this.chkRepeatCustomer );

                
                this.Controls.Add(this.lblCustomerTags );
this.Controls.Add(this.txtCustomerTags );

                this.Controls.Add(this.lblCoreProductInfo );
this.Controls.Add(this.txtCoreProductInfo );

                this.Controls.Add(this.lblGetCustomerSource );
this.Controls.Add(this.txtGetCustomerSource );

                this.Controls.Add(this.lblSalePlatform );
this.Controls.Add(this.txtSalePlatform );

                this.Controls.Add(this.lblWebsite );
this.Controls.Add(this.txtWebsite );

                
                
                this.Controls.Add(this.lblTotalPurchaseAmount );
this.Controls.Add(this.txtTotalPurchaseAmount );

                
                this.Controls.Add(this.lblLastPurchaseDate );
this.Controls.Add(this.dtpLastPurchaseDate );

                this.Controls.Add(this.lblFirstPurchaseDate );
this.Controls.Add(this.dtpFirstPurchaseDate );

                this.Controls.Add(this.lblConverted );
this.Controls.Add(this.chkConverted );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_CRM_CustomerQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLeadID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLeadID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRegion_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRegion_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProvinceID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProvinceID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCityID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCityID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblwwSocialTools;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtwwSocialTools;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSocialTools;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSocialTools;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact_Email;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact_Email;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact_Phone;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact_Phone;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepeatCustomer;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkRepeatCustomer;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerTags;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerTags;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCoreProductInfo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCoreProductInfo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGetCustomerSource;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGetCustomerSource;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSalePlatform;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSalePlatform;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWebsite;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalPurchaseAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalPurchaseAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLastPurchaseDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLastPurchaseDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFirstPurchaseDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpFirstPurchaseDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConverted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkConverted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


