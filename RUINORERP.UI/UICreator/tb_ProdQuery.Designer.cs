
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/07/2024 19:06:29
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
    partial class tb_ProdQuery
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
     
     this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblImagesPath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtImagesPath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtImagesPath.Multiline = true;


this.lblENName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtENName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtENName.Multiline = true;

this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblShortCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShortCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;


this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCategory_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCategory_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblType_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbType_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblBrand = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBrand = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProductENDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductENDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtProductENDesc.Multiline = true;

this.lblProductCNDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductCNDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtProductCNDesc.Multiline = true;

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomsCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomsCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTag = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTag = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtTag.Multiline = true;

this.lblSalePublish = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkSalePublish = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkSalePublish.Values.Text ="";

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblIs_available = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_available = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_available.Values.Text ="";
this.chkIs_available.Checked = true;
this.chkIs_available.CheckState = System.Windows.Forms.CheckState.Checked;

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

           //#####50ShortCode###String
this.lblShortCode.AutoSize = true;
this.lblShortCode.Location = new System.Drawing.Point(100,175);
this.lblShortCode.Name = "lblShortCode";
this.lblShortCode.Size = new System.Drawing.Size(41, 12);
this.lblShortCode.TabIndex = 7;
this.lblShortCode.Text = "短码";
this.txtShortCode.Location = new System.Drawing.Point(173,171);
this.txtShortCode.Name = "txtShortCode";
this.txtShortCode.Size = new System.Drawing.Size(100, 21);
this.txtShortCode.TabIndex = 7;
this.Controls.Add(this.lblShortCode);
this.Controls.Add(this.txtShortCode);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,200);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 8;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,196);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 8;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####SourceType###Int32
//属性测试225SourceType
//属性测试225SourceType
//属性测试225SourceType
//属性测试225SourceType
//属性测试225SourceType
//属性测试225SourceType
//属性测试225SourceType
//属性测试225SourceType

           //#####DepartmentID###Int64
//属性测试250DepartmentID
//属性测试250DepartmentID
//属性测试250DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,250);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 10;
this.lblDepartmentID.Text = "所属部门";
//111======250
this.cmbDepartmentID.Location = new System.Drawing.Point(173,246);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 10;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####PropertyType###Int32
//属性测试275PropertyType
//属性测试275PropertyType
//属性测试275PropertyType
//属性测试275PropertyType
//属性测试275PropertyType
//属性测试275PropertyType
//属性测试275PropertyType
//属性测试275PropertyType

           //#####Unit_ID###Int64
//属性测试300Unit_ID
//属性测试300Unit_ID
//属性测试300Unit_ID
//属性测试300Unit_ID
//属性测试300Unit_ID
//属性测试300Unit_ID
//属性测试300Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,300);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 12;
this.lblUnit_ID.Text = "单位";
//111======300
this.cmbUnit_ID.Location = new System.Drawing.Point(173,296);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 12;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####Category_ID###Int64
//属性测试325Category_ID
//属性测试325Category_ID
//属性测试325Category_ID
//属性测试325Category_ID
//属性测试325Category_ID
this.lblCategory_ID.AutoSize = true;
this.lblCategory_ID.Location = new System.Drawing.Point(100,325);
this.lblCategory_ID.Name = "lblCategory_ID";
this.lblCategory_ID.Size = new System.Drawing.Size(41, 12);
this.lblCategory_ID.TabIndex = 13;
this.lblCategory_ID.Text = "类别";
//111======325
this.cmbCategory_ID.Location = new System.Drawing.Point(173,321);
this.cmbCategory_ID.Name ="cmbCategory_ID";
this.cmbCategory_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCategory_ID.TabIndex = 13;
this.Controls.Add(this.lblCategory_ID);
this.Controls.Add(this.cmbCategory_ID);

           //#####Type_ID###Int64
//属性测试350Type_ID
//属性测试350Type_ID
//属性测试350Type_ID
//属性测试350Type_ID
//属性测试350Type_ID
//属性测试350Type_ID
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,350);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 14;
this.lblType_ID.Text = "货品类型";
//111======350
this.cmbType_ID.Location = new System.Drawing.Point(173,346);
this.cmbType_ID.Name ="cmbType_ID";
this.cmbType_ID.Size = new System.Drawing.Size(100, 21);
this.cmbType_ID.TabIndex = 14;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.cmbType_ID);

           //#####CustomerVendor_ID###Int64
//属性测试375CustomerVendor_ID
//属性测试375CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,375);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 15;
this.lblCustomerVendor_ID.Text = "厂商";
//111======375
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,371);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 15;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####Location_ID###Int64
//属性测试400Location_ID
//属性测试400Location_ID
//属性测试400Location_ID
//属性测试400Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,400);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 16;
this.lblLocation_ID.Text = "默认仓库";
//111======400
this.cmbLocation_ID.Location = new System.Drawing.Point(173,396);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 16;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####Rack_ID###Int64
//属性测试425Rack_ID
//属性测试425Rack_ID
//属性测试425Rack_ID
//属性测试425Rack_ID
//属性测试425Rack_ID
//属性测试425Rack_ID
//属性测试425Rack_ID
//属性测试425Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,425);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 17;
this.lblRack_ID.Text = "默认货架";
//111======425
this.cmbRack_ID.Location = new System.Drawing.Point(173,421);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 17;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

           //#####Employee_ID###Int64
//属性测试450Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,450);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 18;
this.lblEmployee_ID.Text = "业务员";
//111======450
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,446);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 18;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####50Brand###String
this.lblBrand.AutoSize = true;
this.lblBrand.Location = new System.Drawing.Point(100,475);
this.lblBrand.Name = "lblBrand";
this.lblBrand.Size = new System.Drawing.Size(41, 12);
this.lblBrand.TabIndex = 19;
this.lblBrand.Text = "品牌";
this.txtBrand.Location = new System.Drawing.Point(173,471);
this.txtBrand.Name = "txtBrand";
this.txtBrand.Size = new System.Drawing.Size(100, 21);
this.txtBrand.TabIndex = 19;
this.Controls.Add(this.lblBrand);
this.Controls.Add(this.txtBrand);

           //#####2147483647ProductENDesc###String
this.lblProductENDesc.AutoSize = true;
this.lblProductENDesc.Location = new System.Drawing.Point(100,500);
this.lblProductENDesc.Name = "lblProductENDesc";
this.lblProductENDesc.Size = new System.Drawing.Size(41, 12);
this.lblProductENDesc.TabIndex = 20;
this.lblProductENDesc.Text = "英文详情";
this.txtProductENDesc.Location = new System.Drawing.Point(173,496);
this.txtProductENDesc.Name = "txtProductENDesc";
this.txtProductENDesc.Size = new System.Drawing.Size(100, 21);
this.txtProductENDesc.TabIndex = 20;
this.txtProductENDesc.Multiline = true;
this.Controls.Add(this.lblProductENDesc);
this.Controls.Add(this.txtProductENDesc);

           //#####2147483647ProductCNDesc###String
this.lblProductCNDesc.AutoSize = true;
this.lblProductCNDesc.Location = new System.Drawing.Point(100,525);
this.lblProductCNDesc.Name = "lblProductCNDesc";
this.lblProductCNDesc.Size = new System.Drawing.Size(41, 12);
this.lblProductCNDesc.TabIndex = 21;
this.lblProductCNDesc.Text = "中文详情";
this.txtProductCNDesc.Location = new System.Drawing.Point(173,521);
this.txtProductCNDesc.Name = "txtProductCNDesc";
this.txtProductCNDesc.Size = new System.Drawing.Size(100, 21);
this.txtProductCNDesc.TabIndex = 21;
this.txtProductCNDesc.Multiline = true;
this.Controls.Add(this.lblProductCNDesc);
this.Controls.Add(this.txtProductCNDesc);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,550);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 22;
this.lblTaxRate.Text = "税率";
//111======550
this.txtTaxRate.Location = new System.Drawing.Point(173,546);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 22;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####30CustomsCode###String
this.lblCustomsCode.AutoSize = true;
this.lblCustomsCode.Location = new System.Drawing.Point(100,575);
this.lblCustomsCode.Name = "lblCustomsCode";
this.lblCustomsCode.Size = new System.Drawing.Size(41, 12);
this.lblCustomsCode.TabIndex = 23;
this.lblCustomsCode.Text = "海关编码";
this.txtCustomsCode.Location = new System.Drawing.Point(173,571);
this.txtCustomsCode.Name = "txtCustomsCode";
this.txtCustomsCode.Size = new System.Drawing.Size(100, 21);
this.txtCustomsCode.TabIndex = 23;
this.Controls.Add(this.lblCustomsCode);
this.Controls.Add(this.txtCustomsCode);

           //#####250Tag###String
this.lblTag.AutoSize = true;
this.lblTag.Location = new System.Drawing.Point(100,600);
this.lblTag.Name = "lblTag";
this.lblTag.Size = new System.Drawing.Size(41, 12);
this.lblTag.TabIndex = 24;
this.lblTag.Text = "标签";
this.txtTag.Location = new System.Drawing.Point(173,596);
this.txtTag.Name = "txtTag";
this.txtTag.Size = new System.Drawing.Size(100, 21);
this.txtTag.TabIndex = 24;
this.Controls.Add(this.lblTag);
this.Controls.Add(this.txtTag);

           //#####SalePublish###Boolean
this.lblSalePublish.AutoSize = true;
this.lblSalePublish.Location = new System.Drawing.Point(100,625);
this.lblSalePublish.Name = "lblSalePublish";
this.lblSalePublish.Size = new System.Drawing.Size(41, 12);
this.lblSalePublish.TabIndex = 25;
this.lblSalePublish.Text = "参与分销";
this.chkSalePublish.Location = new System.Drawing.Point(173,621);
this.chkSalePublish.Name = "chkSalePublish";
this.chkSalePublish.Size = new System.Drawing.Size(100, 21);
this.chkSalePublish.TabIndex = 25;
this.Controls.Add(this.lblSalePublish);
this.Controls.Add(this.chkSalePublish);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,650);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 26;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,646);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 26;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####Is_available###Boolean
this.lblIs_available.AutoSize = true;
this.lblIs_available.Location = new System.Drawing.Point(100,675);
this.lblIs_available.Name = "lblIs_available";
this.lblIs_available.Size = new System.Drawing.Size(41, 12);
this.lblIs_available.TabIndex = 27;
this.lblIs_available.Text = "是否可用";
this.chkIs_available.Location = new System.Drawing.Point(173,671);
this.chkIs_available.Name = "chkIs_available";
this.chkIs_available.Size = new System.Drawing.Size(100, 21);
this.chkIs_available.TabIndex = 27;
this.Controls.Add(this.lblIs_available);
this.Controls.Add(this.chkIs_available);

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
//属性测试850DataStatus
//属性测试850DataStatus
//属性测试850DataStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                this.Controls.Add(this.lblShortCode );
this.Controls.Add(this.txtShortCode );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                
                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                
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

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                
                    
            this.Name = "tb_ProdQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblImagesPath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtImagesPath;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblENName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtENName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShortCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShortCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnit_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCategory_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCategory_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblType_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbType_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRack_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBrand;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBrand;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductENDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductENDesc;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductCNDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductCNDesc;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomsCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomsCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTag;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTag;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSalePublish;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkSalePublish;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_available;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_available;

    
        
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


