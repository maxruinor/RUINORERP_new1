
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:21
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 客户厂商表 开票资料这种与财务有关另外开表
    /// </summary>
    partial class tb_CustomerVendorQuery
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
     
     this.lblCVCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCVCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCVName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCVName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCVName.Multiline = true;

this.lblShortName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShortName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblType_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbType_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblIsExclusive = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsExclusive = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsExclusive.Values.Text ="";
this.chkIsExclusive.Checked = true;
this.chkIsExclusive.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblArea = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtArea = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMobilePhone = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMobilePhone = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFax = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPhone = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPhone = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmail = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEmail = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtAddress.Multiline = true;

this.lblWebsite = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWebsite = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtWebsite.Multiline = true;

this.lblCustomerCreditLimit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerCreditLimit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblSupplierCreditLimit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSupplierCreditLimit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblIsCustomer = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsCustomer = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsCustomer.Values.Text ="";

this.lblIsVendor = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsVendor = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsVendor.Values.Text ="";

this.lblIsOther = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsOther = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsOther.Values.Text ="";

this.lblSpecialNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecialNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecialNotes.Multiline = true;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


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

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50CVCode###String
this.lblCVCode.AutoSize = true;
this.lblCVCode.Location = new System.Drawing.Point(100,25);
this.lblCVCode.Name = "lblCVCode";
this.lblCVCode.Size = new System.Drawing.Size(41, 12);
this.lblCVCode.TabIndex = 1;
this.lblCVCode.Text = "编号";
this.txtCVCode.Location = new System.Drawing.Point(173,21);
this.txtCVCode.Name = "txtCVCode";
this.txtCVCode.Size = new System.Drawing.Size(100, 21);
this.txtCVCode.TabIndex = 1;
this.Controls.Add(this.lblCVCode);
this.Controls.Add(this.txtCVCode);

           //#####255CVName###String
this.lblCVName.AutoSize = true;
this.lblCVName.Location = new System.Drawing.Point(100,50);
this.lblCVName.Name = "lblCVName";
this.lblCVName.Size = new System.Drawing.Size(41, 12);
this.lblCVName.TabIndex = 2;
this.lblCVName.Text = "全称";
this.txtCVName.Location = new System.Drawing.Point(173,46);
this.txtCVName.Name = "txtCVName";
this.txtCVName.Size = new System.Drawing.Size(100, 21);
this.txtCVName.TabIndex = 2;
this.Controls.Add(this.lblCVName);
this.Controls.Add(this.txtCVName);

           //#####50ShortName###String
this.lblShortName.AutoSize = true;
this.lblShortName.Location = new System.Drawing.Point(100,75);
this.lblShortName.Name = "lblShortName";
this.lblShortName.Size = new System.Drawing.Size(41, 12);
this.lblShortName.TabIndex = 3;
this.lblShortName.Text = "简称";
this.txtShortName.Location = new System.Drawing.Point(173,71);
this.txtShortName.Name = "txtShortName";
this.txtShortName.Size = new System.Drawing.Size(100, 21);
this.txtShortName.TabIndex = 3;
this.Controls.Add(this.lblShortName);
this.Controls.Add(this.txtShortName);

           //#####Type_ID###Int64
//属性测试100Type_ID
//属性测试100Type_ID
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,100);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 4;
this.lblType_ID.Text = "客户厂商类型";
//111======100
this.cmbType_ID.Location = new System.Drawing.Point(173,96);
this.cmbType_ID.Name ="cmbType_ID";
this.cmbType_ID.Size = new System.Drawing.Size(100, 21);
this.cmbType_ID.TabIndex = 4;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.cmbType_ID);

           //#####Employee_ID###Int64
//属性测试125Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,125);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 5;
this.lblEmployee_ID.Text = "责任人";
//111======125
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,121);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 5;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####IsExclusive###Boolean
this.lblIsExclusive.AutoSize = true;
this.lblIsExclusive.Location = new System.Drawing.Point(100,150);
this.lblIsExclusive.Name = "lblIsExclusive";
this.lblIsExclusive.Size = new System.Drawing.Size(41, 12);
this.lblIsExclusive.TabIndex = 6;
this.lblIsExclusive.Text = "责任人专属";
this.chkIsExclusive.Location = new System.Drawing.Point(173,146);
this.chkIsExclusive.Name = "chkIsExclusive";
this.chkIsExclusive.Size = new System.Drawing.Size(100, 21);
this.chkIsExclusive.TabIndex = 6;
this.Controls.Add(this.lblIsExclusive);
this.Controls.Add(this.chkIsExclusive);

           //#####Paytype_ID###Int64
//属性测试175Paytype_ID
//属性测试175Paytype_ID
//属性测试175Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,175);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 7;
this.lblPaytype_ID.Text = "默认交易方式";
//111======175
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,171);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 7;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####Customer_id###Int64
//属性测试200Customer_id
//属性测试200Customer_id
//属性测试200Customer_id

           //#####50Area###String
this.lblArea.AutoSize = true;
this.lblArea.Location = new System.Drawing.Point(100,225);
this.lblArea.Name = "lblArea";
this.lblArea.Size = new System.Drawing.Size(41, 12);
this.lblArea.TabIndex = 9;
this.lblArea.Text = "所在地区";
this.txtArea.Location = new System.Drawing.Point(173,221);
this.txtArea.Name = "txtArea";
this.txtArea.Size = new System.Drawing.Size(100, 21);
this.txtArea.TabIndex = 9;
this.Controls.Add(this.lblArea);
this.Controls.Add(this.txtArea);

           //#####50Contact###String
this.lblContact.AutoSize = true;
this.lblContact.Location = new System.Drawing.Point(100,250);
this.lblContact.Name = "lblContact";
this.lblContact.Size = new System.Drawing.Size(41, 12);
this.lblContact.TabIndex = 10;
this.lblContact.Text = "联系人";
this.txtContact.Location = new System.Drawing.Point(173,246);
this.txtContact.Name = "txtContact";
this.txtContact.Size = new System.Drawing.Size(100, 21);
this.txtContact.TabIndex = 10;
this.Controls.Add(this.lblContact);
this.Controls.Add(this.txtContact);

           //#####50MobilePhone###String
this.lblMobilePhone.AutoSize = true;
this.lblMobilePhone.Location = new System.Drawing.Point(100,275);
this.lblMobilePhone.Name = "lblMobilePhone";
this.lblMobilePhone.Size = new System.Drawing.Size(41, 12);
this.lblMobilePhone.TabIndex = 11;
this.lblMobilePhone.Text = "手机";
this.txtMobilePhone.Location = new System.Drawing.Point(173,271);
this.txtMobilePhone.Name = "txtMobilePhone";
this.txtMobilePhone.Size = new System.Drawing.Size(100, 21);
this.txtMobilePhone.TabIndex = 11;
this.Controls.Add(this.lblMobilePhone);
this.Controls.Add(this.txtMobilePhone);

           //#####50Fax###String
this.lblFax.AutoSize = true;
this.lblFax.Location = new System.Drawing.Point(100,300);
this.lblFax.Name = "lblFax";
this.lblFax.Size = new System.Drawing.Size(41, 12);
this.lblFax.TabIndex = 12;
this.lblFax.Text = "传真";
this.txtFax.Location = new System.Drawing.Point(173,296);
this.txtFax.Name = "txtFax";
this.txtFax.Size = new System.Drawing.Size(100, 21);
this.txtFax.TabIndex = 12;
this.Controls.Add(this.lblFax);
this.Controls.Add(this.txtFax);

           //#####50Phone###String
this.lblPhone.AutoSize = true;
this.lblPhone.Location = new System.Drawing.Point(100,325);
this.lblPhone.Name = "lblPhone";
this.lblPhone.Size = new System.Drawing.Size(41, 12);
this.lblPhone.TabIndex = 13;
this.lblPhone.Text = "座机";
this.txtPhone.Location = new System.Drawing.Point(173,321);
this.txtPhone.Name = "txtPhone";
this.txtPhone.Size = new System.Drawing.Size(100, 21);
this.txtPhone.TabIndex = 13;
this.Controls.Add(this.lblPhone);
this.Controls.Add(this.txtPhone);

           //#####100Email###String
this.lblEmail.AutoSize = true;
this.lblEmail.Location = new System.Drawing.Point(100,350);
this.lblEmail.Name = "lblEmail";
this.lblEmail.Size = new System.Drawing.Size(41, 12);
this.lblEmail.TabIndex = 14;
this.lblEmail.Text = "邮箱";
this.txtEmail.Location = new System.Drawing.Point(173,346);
this.txtEmail.Name = "txtEmail";
this.txtEmail.Size = new System.Drawing.Size(100, 21);
this.txtEmail.TabIndex = 14;
this.Controls.Add(this.lblEmail);
this.Controls.Add(this.txtEmail);

           //#####255Address###String
this.lblAddress.AutoSize = true;
this.lblAddress.Location = new System.Drawing.Point(100,375);
this.lblAddress.Name = "lblAddress";
this.lblAddress.Size = new System.Drawing.Size(41, 12);
this.lblAddress.TabIndex = 15;
this.lblAddress.Text = "地址";
this.txtAddress.Location = new System.Drawing.Point(173,371);
this.txtAddress.Name = "txtAddress";
this.txtAddress.Size = new System.Drawing.Size(100, 21);
this.txtAddress.TabIndex = 15;
this.Controls.Add(this.lblAddress);
this.Controls.Add(this.txtAddress);

           //#####255Website###String
this.lblWebsite.AutoSize = true;
this.lblWebsite.Location = new System.Drawing.Point(100,400);
this.lblWebsite.Name = "lblWebsite";
this.lblWebsite.Size = new System.Drawing.Size(41, 12);
this.lblWebsite.TabIndex = 16;
this.lblWebsite.Text = "网址";
this.txtWebsite.Location = new System.Drawing.Point(173,396);
this.txtWebsite.Name = "txtWebsite";
this.txtWebsite.Size = new System.Drawing.Size(100, 21);
this.txtWebsite.TabIndex = 16;
this.Controls.Add(this.lblWebsite);
this.Controls.Add(this.txtWebsite);

           //#####CustomerCreditLimit###Decimal
this.lblCustomerCreditLimit.AutoSize = true;
this.lblCustomerCreditLimit.Location = new System.Drawing.Point(100,425);
this.lblCustomerCreditLimit.Name = "lblCustomerCreditLimit";
this.lblCustomerCreditLimit.Size = new System.Drawing.Size(41, 12);
this.lblCustomerCreditLimit.TabIndex = 17;
this.lblCustomerCreditLimit.Text = "客户信用额度";
//111======425
this.txtCustomerCreditLimit.Location = new System.Drawing.Point(173,421);
this.txtCustomerCreditLimit.Name ="txtCustomerCreditLimit";
this.txtCustomerCreditLimit.Size = new System.Drawing.Size(100, 21);
this.txtCustomerCreditLimit.TabIndex = 17;
this.Controls.Add(this.lblCustomerCreditLimit);
this.Controls.Add(this.txtCustomerCreditLimit);

           //#####CustomerCreditDays###Int32
//属性测试450CustomerCreditDays
//属性测试450CustomerCreditDays
//属性测试450CustomerCreditDays

           //#####SupplierCreditLimit###Decimal
this.lblSupplierCreditLimit.AutoSize = true;
this.lblSupplierCreditLimit.Location = new System.Drawing.Point(100,475);
this.lblSupplierCreditLimit.Name = "lblSupplierCreditLimit";
this.lblSupplierCreditLimit.Size = new System.Drawing.Size(41, 12);
this.lblSupplierCreditLimit.TabIndex = 19;
this.lblSupplierCreditLimit.Text = "供应商信用额度";
//111======475
this.txtSupplierCreditLimit.Location = new System.Drawing.Point(173,471);
this.txtSupplierCreditLimit.Name ="txtSupplierCreditLimit";
this.txtSupplierCreditLimit.Size = new System.Drawing.Size(100, 21);
this.txtSupplierCreditLimit.TabIndex = 19;
this.Controls.Add(this.lblSupplierCreditLimit);
this.Controls.Add(this.txtSupplierCreditLimit);

           //#####SupplierCreditDays###Int32
//属性测试500SupplierCreditDays
//属性测试500SupplierCreditDays
//属性测试500SupplierCreditDays

           //#####IsCustomer###Boolean
this.lblIsCustomer.AutoSize = true;
this.lblIsCustomer.Location = new System.Drawing.Point(100,525);
this.lblIsCustomer.Name = "lblIsCustomer";
this.lblIsCustomer.Size = new System.Drawing.Size(41, 12);
this.lblIsCustomer.TabIndex = 21;
this.lblIsCustomer.Text = "是客户";
this.chkIsCustomer.Location = new System.Drawing.Point(173,521);
this.chkIsCustomer.Name = "chkIsCustomer";
this.chkIsCustomer.Size = new System.Drawing.Size(100, 21);
this.chkIsCustomer.TabIndex = 21;
this.Controls.Add(this.lblIsCustomer);
this.Controls.Add(this.chkIsCustomer);

           //#####IsVendor###Boolean
this.lblIsVendor.AutoSize = true;
this.lblIsVendor.Location = new System.Drawing.Point(100,550);
this.lblIsVendor.Name = "lblIsVendor";
this.lblIsVendor.Size = new System.Drawing.Size(41, 12);
this.lblIsVendor.TabIndex = 22;
this.lblIsVendor.Text = "是供应商";
this.chkIsVendor.Location = new System.Drawing.Point(173,546);
this.chkIsVendor.Name = "chkIsVendor";
this.chkIsVendor.Size = new System.Drawing.Size(100, 21);
this.chkIsVendor.TabIndex = 22;
this.Controls.Add(this.lblIsVendor);
this.Controls.Add(this.chkIsVendor);

           //#####IsOther###Boolean
this.lblIsOther.AutoSize = true;
this.lblIsOther.Location = new System.Drawing.Point(100,575);
this.lblIsOther.Name = "lblIsOther";
this.lblIsOther.Size = new System.Drawing.Size(41, 12);
this.lblIsOther.TabIndex = 23;
this.lblIsOther.Text = "是其他";
this.chkIsOther.Location = new System.Drawing.Point(173,571);
this.chkIsOther.Name = "chkIsOther";
this.chkIsOther.Size = new System.Drawing.Size(100, 21);
this.chkIsOther.TabIndex = 23;
this.Controls.Add(this.lblIsOther);
this.Controls.Add(this.chkIsOther);

           //#####500SpecialNotes###String
this.lblSpecialNotes.AutoSize = true;
this.lblSpecialNotes.Location = new System.Drawing.Point(100,600);
this.lblSpecialNotes.Name = "lblSpecialNotes";
this.lblSpecialNotes.Size = new System.Drawing.Size(41, 12);
this.lblSpecialNotes.TabIndex = 24;
this.lblSpecialNotes.Text = "特殊要求";
this.txtSpecialNotes.Location = new System.Drawing.Point(173,596);
this.txtSpecialNotes.Name = "txtSpecialNotes";
this.txtSpecialNotes.Size = new System.Drawing.Size(100, 21);
this.txtSpecialNotes.TabIndex = 24;
this.Controls.Add(this.lblSpecialNotes);
this.Controls.Add(this.txtSpecialNotes);

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

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,750);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 30;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,746);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 30;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####Is_available###Boolean
this.lblIs_available.AutoSize = true;
this.lblIs_available.Location = new System.Drawing.Point(100,775);
this.lblIs_available.Name = "lblIs_available";
this.lblIs_available.Size = new System.Drawing.Size(41, 12);
this.lblIs_available.TabIndex = 31;
this.lblIs_available.Text = "是否可用";
this.chkIs_available.Location = new System.Drawing.Point(173,771);
this.chkIs_available.Name = "chkIs_available";
this.chkIs_available.Size = new System.Drawing.Size(100, 21);
this.chkIs_available.TabIndex = 31;
this.Controls.Add(this.lblIs_available);
this.Controls.Add(this.chkIs_available);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,800);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 32;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,796);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 32;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCVCode );
this.Controls.Add(this.txtCVCode );

                this.Controls.Add(this.lblCVName );
this.Controls.Add(this.txtCVName );

                this.Controls.Add(this.lblShortName );
this.Controls.Add(this.txtShortName );

                this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.cmbType_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblIsExclusive );
this.Controls.Add(this.chkIsExclusive );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                
                this.Controls.Add(this.lblArea );
this.Controls.Add(this.txtArea );

                this.Controls.Add(this.lblContact );
this.Controls.Add(this.txtContact );

                this.Controls.Add(this.lblMobilePhone );
this.Controls.Add(this.txtMobilePhone );

                this.Controls.Add(this.lblFax );
this.Controls.Add(this.txtFax );

                this.Controls.Add(this.lblPhone );
this.Controls.Add(this.txtPhone );

                this.Controls.Add(this.lblEmail );
this.Controls.Add(this.txtEmail );

                this.Controls.Add(this.lblAddress );
this.Controls.Add(this.txtAddress );

                this.Controls.Add(this.lblWebsite );
this.Controls.Add(this.txtWebsite );

                this.Controls.Add(this.lblCustomerCreditLimit );
this.Controls.Add(this.txtCustomerCreditLimit );

                
                this.Controls.Add(this.lblSupplierCreditLimit );
this.Controls.Add(this.txtSupplierCreditLimit );

                
                this.Controls.Add(this.lblIsCustomer );
this.Controls.Add(this.chkIsCustomer );

                this.Controls.Add(this.lblIsVendor );
this.Controls.Add(this.chkIsVendor );

                this.Controls.Add(this.lblIsOther );
this.Controls.Add(this.chkIsOther );

                this.Controls.Add(this.lblSpecialNotes );
this.Controls.Add(this.txtSpecialNotes );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblIs_available );
this.Controls.Add(this.chkIs_available );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_CustomerVendorQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCVCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCVCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCVName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCVName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShortName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShortName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblType_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbType_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsExclusive;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsExclusive;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblArea;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtArea;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMobilePhone;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMobilePhone;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFax;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPhone;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPhone;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmail;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEmail;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWebsite;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerCreditLimit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerCreditLimit;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSupplierCreditLimit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSupplierCreditLimit;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsCustomer;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsCustomer;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsVendor;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsVendor;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsOther;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsOther;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecialNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecialNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_available;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_available;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


