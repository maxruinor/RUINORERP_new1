// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/16/2024 18:39:01
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
    partial class tb_CRM_CustomerEdit
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
     this.lblCustomerName = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerName = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblLeadID = new Krypton.Toolkit.KryptonLabel();
this.cmbLeadID = new Krypton.Toolkit.KryptonComboBox();

this.lblRegion_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbRegion_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblProvinceID = new Krypton.Toolkit.KryptonLabel();
this.cmbProvinceID = new Krypton.Toolkit.KryptonComboBox();

this.lblCityID = new Krypton.Toolkit.KryptonLabel();
this.cmbCityID = new Krypton.Toolkit.KryptonComboBox();

this.lblwwSocialTools = new Krypton.Toolkit.KryptonLabel();
this.txtwwSocialTools = new Krypton.Toolkit.KryptonTextBox();

this.lblSocialTools = new Krypton.Toolkit.KryptonLabel();
this.txtSocialTools = new Krypton.Toolkit.KryptonTextBox();

this.lblContact_Name = new Krypton.Toolkit.KryptonLabel();
this.txtContact_Name = new Krypton.Toolkit.KryptonTextBox();

this.lblContact_Email = new Krypton.Toolkit.KryptonLabel();
this.txtContact_Email = new Krypton.Toolkit.KryptonTextBox();

this.lblContact_Phone = new Krypton.Toolkit.KryptonLabel();
this.txtContact_Phone = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerAddress = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerAddress = new Krypton.Toolkit.KryptonTextBox();
this.txtCustomerAddress.Multiline = true;

this.lblRepeatCustomer = new Krypton.Toolkit.KryptonLabel();
this.chkRepeatCustomer = new Krypton.Toolkit.KryptonCheckBox();
this.chkRepeatCustomer.Values.Text ="";

this.lblCustomerStatus = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerTags = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerTags = new Krypton.Toolkit.KryptonTextBox();
this.txtCustomerTags.Multiline = true;

this.lblCoreProductInfo = new Krypton.Toolkit.KryptonLabel();
this.txtCoreProductInfo = new Krypton.Toolkit.KryptonTextBox();

this.lblGetCustomerSource = new Krypton.Toolkit.KryptonLabel();
this.txtGetCustomerSource = new Krypton.Toolkit.KryptonTextBox();
this.txtGetCustomerSource.Multiline = true;

this.lblSalePlatform = new Krypton.Toolkit.KryptonLabel();
this.txtSalePlatform = new Krypton.Toolkit.KryptonTextBox();

this.lblWebsite = new Krypton.Toolkit.KryptonLabel();
this.txtWebsite = new Krypton.Toolkit.KryptonTextBox();
this.txtWebsite.Multiline = true;

this.lblCustomerLevel = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerLevel = new Krypton.Toolkit.KryptonTextBox();

this.lblPurchaseCount = new Krypton.Toolkit.KryptonLabel();
this.txtPurchaseCount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalPurchaseAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalPurchaseAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblDaysSinceLastPurchase = new Krypton.Toolkit.KryptonLabel();
this.txtDaysSinceLastPurchase = new Krypton.Toolkit.KryptonTextBox();

this.lblLastPurchaseDate = new Krypton.Toolkit.KryptonLabel();
this.dtpLastPurchaseDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblFirstPurchaseDate = new Krypton.Toolkit.KryptonLabel();
this.dtpFirstPurchaseDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblConverted = new Krypton.Toolkit.KryptonLabel();
this.chkConverted = new Krypton.Toolkit.KryptonCheckBox();
this.chkConverted.Values.Text ="";

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
this.lblCustomerStatus.AutoSize = true;
this.lblCustomerStatus.Location = new System.Drawing.Point(100,375);
this.lblCustomerStatus.Name = "lblCustomerStatus";
this.lblCustomerStatus.Size = new System.Drawing.Size(41, 12);
this.lblCustomerStatus.TabIndex = 15;
this.lblCustomerStatus.Text = "客户状态";
this.txtCustomerStatus.Location = new System.Drawing.Point(173,371);
this.txtCustomerStatus.Name = "txtCustomerStatus";
this.txtCustomerStatus.Size = new System.Drawing.Size(100, 21);
this.txtCustomerStatus.TabIndex = 15;
this.Controls.Add(this.lblCustomerStatus);
this.Controls.Add(this.txtCustomerStatus);

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
this.lblCustomerLevel.AutoSize = true;
this.lblCustomerLevel.Location = new System.Drawing.Point(100,525);
this.lblCustomerLevel.Name = "lblCustomerLevel";
this.lblCustomerLevel.Size = new System.Drawing.Size(41, 12);
this.lblCustomerLevel.TabIndex = 21;
this.lblCustomerLevel.Text = "客户级别";
this.txtCustomerLevel.Location = new System.Drawing.Point(173,521);
this.txtCustomerLevel.Name = "txtCustomerLevel";
this.txtCustomerLevel.Size = new System.Drawing.Size(100, 21);
this.txtCustomerLevel.TabIndex = 21;
this.Controls.Add(this.lblCustomerLevel);
this.Controls.Add(this.txtCustomerLevel);

           //#####PurchaseCount###Int32
//属性测试550PurchaseCount
//属性测试550PurchaseCount
//属性测试550PurchaseCount
//属性测试550PurchaseCount
//属性测试550PurchaseCount
//属性测试550PurchaseCount
this.lblPurchaseCount.AutoSize = true;
this.lblPurchaseCount.Location = new System.Drawing.Point(100,550);
this.lblPurchaseCount.Name = "lblPurchaseCount";
this.lblPurchaseCount.Size = new System.Drawing.Size(41, 12);
this.lblPurchaseCount.TabIndex = 22;
this.lblPurchaseCount.Text = "采购次数";
this.txtPurchaseCount.Location = new System.Drawing.Point(173,546);
this.txtPurchaseCount.Name = "txtPurchaseCount";
this.txtPurchaseCount.Size = new System.Drawing.Size(100, 21);
this.txtPurchaseCount.TabIndex = 22;
this.Controls.Add(this.lblPurchaseCount);
this.Controls.Add(this.txtPurchaseCount);

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
this.lblDaysSinceLastPurchase.AutoSize = true;
this.lblDaysSinceLastPurchase.Location = new System.Drawing.Point(100,600);
this.lblDaysSinceLastPurchase.Name = "lblDaysSinceLastPurchase";
this.lblDaysSinceLastPurchase.Size = new System.Drawing.Size(41, 12);
this.lblDaysSinceLastPurchase.TabIndex = 24;
this.lblDaysSinceLastPurchase.Text = "最近距上次采购间隔天";
this.txtDaysSinceLastPurchase.Location = new System.Drawing.Point(173,596);
this.txtDaysSinceLastPurchase.Name = "txtDaysSinceLastPurchase";
this.txtDaysSinceLastPurchase.Size = new System.Drawing.Size(100, 21);
this.txtDaysSinceLastPurchase.TabIndex = 24;
this.Controls.Add(this.lblDaysSinceLastPurchase);
this.Controls.Add(this.txtDaysSinceLastPurchase);

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
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,750);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 30;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,746);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 30;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,800);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 32;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,796);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 32;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
           // this.kryptonPanel1.TabIndex = 33;

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

                this.Controls.Add(this.lblCustomerStatus );
this.Controls.Add(this.txtCustomerStatus );

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

                this.Controls.Add(this.lblCustomerLevel );
this.Controls.Add(this.txtCustomerLevel );

                this.Controls.Add(this.lblPurchaseCount );
this.Controls.Add(this.txtPurchaseCount );

                this.Controls.Add(this.lblTotalPurchaseAmount );
this.Controls.Add(this.txtTotalPurchaseAmount );

                this.Controls.Add(this.lblDaysSinceLastPurchase );
this.Controls.Add(this.txtDaysSinceLastPurchase );

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

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                            // 
            // "tb_CRM_CustomerEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CRM_CustomerEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCustomerName;
private Krypton.Toolkit.KryptonTextBox txtCustomerName;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLeadID;
private Krypton.Toolkit.KryptonComboBox cmbLeadID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRegion_ID;
private Krypton.Toolkit.KryptonComboBox cmbRegion_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProvinceID;
private Krypton.Toolkit.KryptonComboBox cmbProvinceID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCityID;
private Krypton.Toolkit.KryptonComboBox cmbCityID;

    
        
              private Krypton.Toolkit.KryptonLabel lblwwSocialTools;
private Krypton.Toolkit.KryptonTextBox txtwwSocialTools;

    
        
              private Krypton.Toolkit.KryptonLabel lblSocialTools;
private Krypton.Toolkit.KryptonTextBox txtSocialTools;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact_Name;
private Krypton.Toolkit.KryptonTextBox txtContact_Name;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact_Email;
private Krypton.Toolkit.KryptonTextBox txtContact_Email;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact_Phone;
private Krypton.Toolkit.KryptonTextBox txtContact_Phone;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerAddress;
private Krypton.Toolkit.KryptonTextBox txtCustomerAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblRepeatCustomer;
private Krypton.Toolkit.KryptonCheckBox chkRepeatCustomer;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerStatus;
private Krypton.Toolkit.KryptonTextBox txtCustomerStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerTags;
private Krypton.Toolkit.KryptonTextBox txtCustomerTags;

    
        
              private Krypton.Toolkit.KryptonLabel lblCoreProductInfo;
private Krypton.Toolkit.KryptonTextBox txtCoreProductInfo;

    
        
              private Krypton.Toolkit.KryptonLabel lblGetCustomerSource;
private Krypton.Toolkit.KryptonTextBox txtGetCustomerSource;

    
        
              private Krypton.Toolkit.KryptonLabel lblSalePlatform;
private Krypton.Toolkit.KryptonTextBox txtSalePlatform;

    
        
              private Krypton.Toolkit.KryptonLabel lblWebsite;
private Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerLevel;
private Krypton.Toolkit.KryptonTextBox txtCustomerLevel;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurchaseCount;
private Krypton.Toolkit.KryptonTextBox txtPurchaseCount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalPurchaseAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalPurchaseAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblDaysSinceLastPurchase;
private Krypton.Toolkit.KryptonTextBox txtDaysSinceLastPurchase;

    
        
              private Krypton.Toolkit.KryptonLabel lblLastPurchaseDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpLastPurchaseDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblFirstPurchaseDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpFirstPurchaseDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblConverted;
private Krypton.Toolkit.KryptonCheckBox chkConverted;

    
        
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

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

