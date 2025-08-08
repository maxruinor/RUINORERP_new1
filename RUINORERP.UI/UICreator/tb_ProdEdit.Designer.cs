// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:51
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 货品基本信息表
    /// </summary>
    partial class tb_ProdEdit
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
     this.lblProductNo = new Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblImagesPath = new Krypton.Toolkit.KryptonLabel();
this.txtImagesPath = new Krypton.Toolkit.KryptonTextBox();
this.txtImagesPath.Multiline = true;


this.lblENName = new Krypton.Toolkit.KryptonLabel();
this.txtENName = new Krypton.Toolkit.KryptonTextBox();
this.txtENName.Multiline = true;

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblVendorModelCode = new Krypton.Toolkit.KryptonLabel();
this.txtVendorModelCode = new Krypton.Toolkit.KryptonTextBox();

this.lblShortCode = new Krypton.Toolkit.KryptonLabel();
this.txtShortCode = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblSourceType = new Krypton.Toolkit.KryptonLabel();
this.txtSourceType = new Krypton.Toolkit.KryptonTextBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblPropertyType = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyType = new Krypton.Toolkit.KryptonTextBox();

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCategory_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCategory_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbType_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblBrand = new Krypton.Toolkit.KryptonLabel();
this.txtBrand = new Krypton.Toolkit.KryptonTextBox();

this.lblProductENDesc = new Krypton.Toolkit.KryptonLabel();
this.txtProductENDesc = new Krypton.Toolkit.KryptonTextBox();
this.txtProductENDesc.Multiline = true;

this.lblProductCNDesc = new Krypton.Toolkit.KryptonLabel();
this.txtProductCNDesc = new Krypton.Toolkit.KryptonTextBox();
this.txtProductCNDesc.Multiline = true;

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomsCode = new Krypton.Toolkit.KryptonLabel();
this.txtCustomsCode = new Krypton.Toolkit.KryptonTextBox();

this.lblTag = new Krypton.Toolkit.KryptonLabel();
this.txtTag = new Krypton.Toolkit.KryptonTextBox();
this.txtTag.Multiline = true;

this.lblSalePublish = new Krypton.Toolkit.KryptonLabel();
this.chkSalePublish = new Krypton.Toolkit.KryptonCheckBox();
this.chkSalePublish.Values.Text ="";

this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblIs_available = new Krypton.Toolkit.KryptonLabel();
this.chkIs_available = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_available.Values.Text ="";
this.chkIs_available.Checked = true;
this.chkIs_available.CheckState = System.Windows.Forms.CheckState.Checked;

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
     
            //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,25);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 1;
this.lblProductNo.Text = "品号";
this.txtProductNo.Location = new System.Drawing.Point(173,21);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 1;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,50);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 2;
this.lblCNName.Text = "品名";
this.txtCNName.Location = new System.Drawing.Point(173,46);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 2;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####2000ImagesPath###String
this.lblImagesPath.AutoSize = true;
this.lblImagesPath.Location = new System.Drawing.Point(100,75);
this.lblImagesPath.Name = "lblImagesPath";
this.lblImagesPath.Size = new System.Drawing.Size(41, 12);
this.lblImagesPath.TabIndex = 3;
this.lblImagesPath.Text = "图片组";
this.txtImagesPath.Location = new System.Drawing.Point(173,71);
this.txtImagesPath.Name = "txtImagesPath";
this.txtImagesPath.Size = new System.Drawing.Size(100, 21);
this.txtImagesPath.TabIndex = 3;
this.Controls.Add(this.lblImagesPath);
this.Controls.Add(this.txtImagesPath);

           //#####2147483647Images###Binary

           //#####255ENName###String
this.lblENName.AutoSize = true;
this.lblENName.Location = new System.Drawing.Point(100,125);
this.lblENName.Name = "lblENName";
this.lblENName.Size = new System.Drawing.Size(41, 12);
this.lblENName.TabIndex = 5;
this.lblENName.Text = "英文名称";
this.txtENName.Location = new System.Drawing.Point(173,121);
this.txtENName.Name = "txtENName";
this.txtENName.Size = new System.Drawing.Size(100, 21);
this.txtENName.TabIndex = 5;
this.Controls.Add(this.lblENName);
this.Controls.Add(this.txtENName);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,150);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 6;
this.lblModel.Text = "型号";
this.txtModel.Location = new System.Drawing.Point(173,146);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 6;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####50VendorModelCode###String
this.lblVendorModelCode.AutoSize = true;
this.lblVendorModelCode.Location = new System.Drawing.Point(100,175);
this.lblVendorModelCode.Name = "lblVendorModelCode";
this.lblVendorModelCode.Size = new System.Drawing.Size(41, 12);
this.lblVendorModelCode.TabIndex = 7;
this.lblVendorModelCode.Text = "厂商型号";
this.txtVendorModelCode.Location = new System.Drawing.Point(173,171);
this.txtVendorModelCode.Name = "txtVendorModelCode";
this.txtVendorModelCode.Size = new System.Drawing.Size(100, 21);
this.txtVendorModelCode.TabIndex = 7;
this.Controls.Add(this.lblVendorModelCode);
this.Controls.Add(this.txtVendorModelCode);

           //#####50ShortCode###String
this.lblShortCode.AutoSize = true;
this.lblShortCode.Location = new System.Drawing.Point(100,200);
this.lblShortCode.Name = "lblShortCode";
this.lblShortCode.Size = new System.Drawing.Size(41, 12);
this.lblShortCode.TabIndex = 8;
this.lblShortCode.Text = "短码";
this.txtShortCode.Location = new System.Drawing.Point(173,196);
this.txtShortCode.Name = "txtShortCode";
this.txtShortCode.Size = new System.Drawing.Size(100, 21);
this.txtShortCode.TabIndex = 8;
this.Controls.Add(this.lblShortCode);
this.Controls.Add(this.txtShortCode);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,225);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 9;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,221);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 9;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####SourceType###Int32
//属性测试250SourceType
//属性测试250SourceType
//属性测试250SourceType
//属性测试250SourceType
//属性测试250SourceType
//属性测试250SourceType
//属性测试250SourceType
//属性测试250SourceType
this.lblSourceType.AutoSize = true;
this.lblSourceType.Location = new System.Drawing.Point(100,250);
this.lblSourceType.Name = "lblSourceType";
this.lblSourceType.Size = new System.Drawing.Size(41, 12);
this.lblSourceType.TabIndex = 10;
this.lblSourceType.Text = "货品来源";
this.txtSourceType.Location = new System.Drawing.Point(173,246);
this.txtSourceType.Name = "txtSourceType";
this.txtSourceType.Size = new System.Drawing.Size(100, 21);
this.txtSourceType.TabIndex = 10;
this.Controls.Add(this.lblSourceType);
this.Controls.Add(this.txtSourceType);

           //#####DepartmentID###Int64
//属性测试275DepartmentID
//属性测试275DepartmentID
//属性测试275DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,275);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 11;
this.lblDepartmentID.Text = "所属部门";
//111======275
this.cmbDepartmentID.Location = new System.Drawing.Point(173,271);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 11;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####PropertyType###Int32
//属性测试300PropertyType
//属性测试300PropertyType
//属性测试300PropertyType
//属性测试300PropertyType
//属性测试300PropertyType
//属性测试300PropertyType
//属性测试300PropertyType
//属性测试300PropertyType
this.lblPropertyType.AutoSize = true;
this.lblPropertyType.Location = new System.Drawing.Point(100,300);
this.lblPropertyType.Name = "lblPropertyType";
this.lblPropertyType.Size = new System.Drawing.Size(41, 12);
this.lblPropertyType.TabIndex = 12;
this.lblPropertyType.Text = "属性类型";
this.txtPropertyType.Location = new System.Drawing.Point(173,296);
this.txtPropertyType.Name = "txtPropertyType";
this.txtPropertyType.Size = new System.Drawing.Size(100, 21);
this.txtPropertyType.TabIndex = 12;
this.Controls.Add(this.lblPropertyType);
this.Controls.Add(this.txtPropertyType);

           //#####Unit_ID###Int64
//属性测试325Unit_ID
//属性测试325Unit_ID
//属性测试325Unit_ID
//属性测试325Unit_ID
//属性测试325Unit_ID
//属性测试325Unit_ID
//属性测试325Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,325);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 13;
this.lblUnit_ID.Text = "单位";
//111======325
this.cmbUnit_ID.Location = new System.Drawing.Point(173,321);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 13;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####Category_ID###Int64
//属性测试350Category_ID
//属性测试350Category_ID
//属性测试350Category_ID
//属性测试350Category_ID
//属性测试350Category_ID
this.lblCategory_ID.AutoSize = true;
this.lblCategory_ID.Location = new System.Drawing.Point(100,350);
this.lblCategory_ID.Name = "lblCategory_ID";
this.lblCategory_ID.Size = new System.Drawing.Size(41, 12);
this.lblCategory_ID.TabIndex = 14;
this.lblCategory_ID.Text = "类别";
//111======350
this.cmbCategory_ID.Location = new System.Drawing.Point(173,346);
this.cmbCategory_ID.Name ="cmbCategory_ID";
this.cmbCategory_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCategory_ID.TabIndex = 14;
this.Controls.Add(this.lblCategory_ID);
this.Controls.Add(this.cmbCategory_ID);

           //#####Type_ID###Int64
//属性测试375Type_ID
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
this.lblType_ID.Text = "货品类型";
//111======375
this.cmbType_ID.Location = new System.Drawing.Point(173,371);
this.cmbType_ID.Name ="cmbType_ID";
this.cmbType_ID.Size = new System.Drawing.Size(100, 21);
this.cmbType_ID.TabIndex = 15;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.cmbType_ID);

           //#####CustomerVendor_ID###Int64
//属性测试400CustomerVendor_ID
//属性测试400CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,400);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 16;
this.lblCustomerVendor_ID.Text = "厂商";
//111======400
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,396);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 16;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####Location_ID###Int64
//属性测试425Location_ID
//属性测试425Location_ID
//属性测试425Location_ID
//属性测试425Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,425);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 17;
this.lblLocation_ID.Text = "默认仓库";
//111======425
this.cmbLocation_ID.Location = new System.Drawing.Point(173,421);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 17;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####Rack_ID###Int64
//属性测试450Rack_ID
//属性测试450Rack_ID
//属性测试450Rack_ID
//属性测试450Rack_ID
//属性测试450Rack_ID
//属性测试450Rack_ID
//属性测试450Rack_ID
//属性测试450Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,450);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 18;
this.lblRack_ID.Text = "默认货架";
//111======450
this.cmbRack_ID.Location = new System.Drawing.Point(173,446);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 18;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

           //#####Employee_ID###Int64
//属性测试475Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,475);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 19;
this.lblEmployee_ID.Text = "业务员";
//111======475
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,471);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 19;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####50Brand###String
this.lblBrand.AutoSize = true;
this.lblBrand.Location = new System.Drawing.Point(100,500);
this.lblBrand.Name = "lblBrand";
this.lblBrand.Size = new System.Drawing.Size(41, 12);
this.lblBrand.TabIndex = 20;
this.lblBrand.Text = "品牌";
this.txtBrand.Location = new System.Drawing.Point(173,496);
this.txtBrand.Name = "txtBrand";
this.txtBrand.Size = new System.Drawing.Size(100, 21);
this.txtBrand.TabIndex = 20;
this.Controls.Add(this.lblBrand);
this.Controls.Add(this.txtBrand);

           //#####2147483647ProductENDesc###String
this.lblProductENDesc.AutoSize = true;
this.lblProductENDesc.Location = new System.Drawing.Point(100,525);
this.lblProductENDesc.Name = "lblProductENDesc";
this.lblProductENDesc.Size = new System.Drawing.Size(41, 12);
this.lblProductENDesc.TabIndex = 21;
this.lblProductENDesc.Text = "英文详情";
this.txtProductENDesc.Location = new System.Drawing.Point(173,521);
this.txtProductENDesc.Name = "txtProductENDesc";
this.txtProductENDesc.Size = new System.Drawing.Size(100, 21);
this.txtProductENDesc.TabIndex = 21;
this.txtProductENDesc.Multiline = true;
this.Controls.Add(this.lblProductENDesc);
this.Controls.Add(this.txtProductENDesc);

           //#####2147483647ProductCNDesc###String
this.lblProductCNDesc.AutoSize = true;
this.lblProductCNDesc.Location = new System.Drawing.Point(100,550);
this.lblProductCNDesc.Name = "lblProductCNDesc";
this.lblProductCNDesc.Size = new System.Drawing.Size(41, 12);
this.lblProductCNDesc.TabIndex = 22;
this.lblProductCNDesc.Text = "中文详情";
this.txtProductCNDesc.Location = new System.Drawing.Point(173,546);
this.txtProductCNDesc.Name = "txtProductCNDesc";
this.txtProductCNDesc.Size = new System.Drawing.Size(100, 21);
this.txtProductCNDesc.TabIndex = 22;
this.txtProductCNDesc.Multiline = true;
this.Controls.Add(this.lblProductCNDesc);
this.Controls.Add(this.txtProductCNDesc);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,575);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 23;
this.lblTaxRate.Text = "税率";
//111======575
this.txtTaxRate.Location = new System.Drawing.Point(173,571);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 23;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####30CustomsCode###String
this.lblCustomsCode.AutoSize = true;
this.lblCustomsCode.Location = new System.Drawing.Point(100,600);
this.lblCustomsCode.Name = "lblCustomsCode";
this.lblCustomsCode.Size = new System.Drawing.Size(41, 12);
this.lblCustomsCode.TabIndex = 24;
this.lblCustomsCode.Text = "海关编码";
this.txtCustomsCode.Location = new System.Drawing.Point(173,596);
this.txtCustomsCode.Name = "txtCustomsCode";
this.txtCustomsCode.Size = new System.Drawing.Size(100, 21);
this.txtCustomsCode.TabIndex = 24;
this.Controls.Add(this.lblCustomsCode);
this.Controls.Add(this.txtCustomsCode);

           //#####250Tag###String
this.lblTag.AutoSize = true;
this.lblTag.Location = new System.Drawing.Point(100,625);
this.lblTag.Name = "lblTag";
this.lblTag.Size = new System.Drawing.Size(41, 12);
this.lblTag.TabIndex = 25;
this.lblTag.Text = "标签";
this.txtTag.Location = new System.Drawing.Point(173,621);
this.txtTag.Name = "txtTag";
this.txtTag.Size = new System.Drawing.Size(100, 21);
this.txtTag.TabIndex = 25;
this.Controls.Add(this.lblTag);
this.Controls.Add(this.txtTag);

           //#####SalePublish###Boolean
this.lblSalePublish.AutoSize = true;
this.lblSalePublish.Location = new System.Drawing.Point(100,650);
this.lblSalePublish.Name = "lblSalePublish";
this.lblSalePublish.Size = new System.Drawing.Size(41, 12);
this.lblSalePublish.TabIndex = 26;
this.lblSalePublish.Text = "参与分销";
this.chkSalePublish.Location = new System.Drawing.Point(173,646);
this.chkSalePublish.Name = "chkSalePublish";
this.chkSalePublish.Size = new System.Drawing.Size(100, 21);
this.chkSalePublish.TabIndex = 26;
this.Controls.Add(this.lblSalePublish);
this.Controls.Add(this.chkSalePublish);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,675);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 27;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,671);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 27;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####Is_available###Boolean
this.lblIs_available.AutoSize = true;
this.lblIs_available.Location = new System.Drawing.Point(100,700);
this.lblIs_available.Name = "lblIs_available";
this.lblIs_available.Size = new System.Drawing.Size(41, 12);
this.lblIs_available.TabIndex = 28;
this.lblIs_available.Text = "是否可用";
this.chkIs_available.Location = new System.Drawing.Point(173,696);
this.chkIs_available.Name = "chkIs_available";
this.chkIs_available.Size = new System.Drawing.Size(100, 21);
this.chkIs_available.TabIndex = 28;
this.Controls.Add(this.lblIs_available);
this.Controls.Add(this.chkIs_available);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,725);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 29;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,721);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 29;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,750);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 30;
this.lblCreated_at.Text = "创建时间";
//111======750
this.dtpCreated_at.Location = new System.Drawing.Point(173,746);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 30;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试775Created_by
//属性测试775Created_by
//属性测试775Created_by
//属性测试775Created_by
//属性测试775Created_by
//属性测试775Created_by
//属性测试775Created_by
//属性测试775Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,775);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 31;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,771);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 31;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,800);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 32;
this.lblModified_at.Text = "修改时间";
//111======800
this.dtpModified_at.Location = new System.Drawing.Point(173,796);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 32;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试825Modified_by
//属性测试825Modified_by
//属性测试825Modified_by
//属性测试825Modified_by
//属性测试825Modified_by
//属性测试825Modified_by
//属性测试825Modified_by
//属性测试825Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,825);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 33;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,821);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 33;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,850);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 34;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,846);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 34;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
//属性测试875DataStatus
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,875);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 35;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,871);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 35;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

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
           // this.kryptonPanel1.TabIndex = 35;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblImagesPath );
this.Controls.Add(this.txtImagesPath );

                
                this.Controls.Add(this.lblENName );
this.Controls.Add(this.txtENName );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblVendorModelCode );
this.Controls.Add(this.txtVendorModelCode );

                this.Controls.Add(this.lblShortCode );
this.Controls.Add(this.txtShortCode );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblSourceType );
this.Controls.Add(this.txtSourceType );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblPropertyType );
this.Controls.Add(this.txtPropertyType );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.cmbUnit_ID );

                this.Controls.Add(this.lblCategory_ID );
this.Controls.Add(this.cmbCategory_ID );

                this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.cmbType_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblBrand );
this.Controls.Add(this.txtBrand );

                this.Controls.Add(this.lblProductENDesc );
this.Controls.Add(this.txtProductENDesc );

                this.Controls.Add(this.lblProductCNDesc );
this.Controls.Add(this.txtProductCNDesc );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblCustomsCode );
this.Controls.Add(this.txtCustomsCode );

                this.Controls.Add(this.lblTag );
this.Controls.Add(this.txtTag );

                this.Controls.Add(this.lblSalePublish );
this.Controls.Add(this.chkSalePublish );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblIs_available );
this.Controls.Add(this.chkIs_available );

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

                            // 
            // "tb_ProdEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProdEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProductNo;
private Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblImagesPath;
private Krypton.Toolkit.KryptonTextBox txtImagesPath;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblENName;
private Krypton.Toolkit.KryptonTextBox txtENName;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblVendorModelCode;
private Krypton.Toolkit.KryptonTextBox txtVendorModelCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblShortCode;
private Krypton.Toolkit.KryptonTextBox txtShortCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceType;
private Krypton.Toolkit.KryptonTextBox txtSourceType;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPropertyType;
private Krypton.Toolkit.KryptonTextBox txtPropertyType;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCategory_ID;
private Krypton.Toolkit.KryptonComboBox cmbCategory_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblType_ID;
private Krypton.Toolkit.KryptonComboBox cmbType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRack_ID;
private Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBrand;
private Krypton.Toolkit.KryptonTextBox txtBrand;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductENDesc;
private Krypton.Toolkit.KryptonTextBox txtProductENDesc;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductCNDesc;
private Krypton.Toolkit.KryptonTextBox txtProductCNDesc;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomsCode;
private Krypton.Toolkit.KryptonTextBox txtCustomsCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblTag;
private Krypton.Toolkit.KryptonTextBox txtTag;

    
        
              private Krypton.Toolkit.KryptonLabel lblSalePublish;
private Krypton.Toolkit.KryptonCheckBox chkSalePublish;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_available;
private Krypton.Toolkit.KryptonCheckBox chkIs_available;

    
        
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

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

