// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/25/2025 10:38:51
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
    partial class tb_CustomerVendorEdit
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
            this.lblCVCode = new Krypton.Toolkit.KryptonLabel();
            this.txtCVCode = new Krypton.Toolkit.KryptonTextBox();
            this.lblCVName = new Krypton.Toolkit.KryptonLabel();
            this.txtCVName = new Krypton.Toolkit.KryptonTextBox();
            this.lblShortName = new Krypton.Toolkit.KryptonLabel();
            this.txtShortName = new Krypton.Toolkit.KryptonTextBox();
            this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbType_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblIsExclusive = new Krypton.Toolkit.KryptonLabel();
            this.chkIsExclusive = new Krypton.Toolkit.KryptonCheckBox();
            this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbPaytype_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblCustomer_id = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomer_id = new Krypton.Toolkit.KryptonTextBox();
            this.lblArea = new Krypton.Toolkit.KryptonLabel();
            this.txtArea = new Krypton.Toolkit.KryptonTextBox();
            this.lblContact = new Krypton.Toolkit.KryptonLabel();
            this.txtContact = new Krypton.Toolkit.KryptonTextBox();
            this.lblPhone = new Krypton.Toolkit.KryptonLabel();
            this.txtPhone = new Krypton.Toolkit.KryptonTextBox();
            this.lblAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblWebsite = new Krypton.Toolkit.KryptonLabel();
            this.txtWebsite = new Krypton.Toolkit.KryptonTextBox();
            this.lblCreditLimit = new Krypton.Toolkit.KryptonLabel();
            this.txtCreditLimit = new Krypton.Toolkit.KryptonTextBox();
            this.lblCreditDays = new Krypton.Toolkit.KryptonLabel();
            this.txtCreditDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblIsCustomer = new Krypton.Toolkit.KryptonLabel();
            this.chkIsCustomer = new Krypton.Toolkit.KryptonCheckBox();
            this.lblIsVendor = new Krypton.Toolkit.KryptonLabel();
            this.chkIsVendor = new Krypton.Toolkit.KryptonCheckBox();
            this.lblIsOther = new Krypton.Toolkit.KryptonLabel();
            this.chkIsOther = new Krypton.Toolkit.KryptonCheckBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
            this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
            this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblIs_available = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_available = new Krypton.Toolkit.KryptonCheckBox();
            this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
            this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbType_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPaytype_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCVCode
            // 
            this.lblCVCode.Location = new System.Drawing.Point(100, 25);
            this.lblCVCode.Name = "lblCVCode";
            this.lblCVCode.Size = new System.Drawing.Size(36, 20);
            this.lblCVCode.TabIndex = 1;
            this.lblCVCode.Values.Text = "编号";
            // 
            // txtCVCode
            // 
            this.txtCVCode.Location = new System.Drawing.Point(173, 21);
            this.txtCVCode.Name = "txtCVCode";
            this.txtCVCode.Size = new System.Drawing.Size(100, 23);
            this.txtCVCode.TabIndex = 1;
            // 
            // lblCVName
            // 
            this.lblCVName.Location = new System.Drawing.Point(100, 50);
            this.lblCVName.Name = "lblCVName";
            this.lblCVName.Size = new System.Drawing.Size(36, 20);
            this.lblCVName.TabIndex = 2;
            this.lblCVName.Values.Text = "全称";
            // 
            // txtCVName
            // 
            this.txtCVName.Location = new System.Drawing.Point(173, 46);
            this.txtCVName.Multiline = true;
            this.txtCVName.Name = "txtCVName";
            this.txtCVName.Size = new System.Drawing.Size(100, 21);
            this.txtCVName.TabIndex = 2;
            // 
            // lblShortName
            // 
            this.lblShortName.Location = new System.Drawing.Point(100, 75);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(36, 20);
            this.lblShortName.TabIndex = 3;
            this.lblShortName.Values.Text = "简称";
            // 
            // txtShortName
            // 
            this.txtShortName.Location = new System.Drawing.Point(173, 71);
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(100, 23);
            this.txtShortName.TabIndex = 3;
            // 
            // lblType_ID
            // 
            this.lblType_ID.Location = new System.Drawing.Point(100, 100);
            this.lblType_ID.Name = "lblType_ID";
            this.lblType_ID.Size = new System.Drawing.Size(88, 20);
            this.lblType_ID.TabIndex = 4;
            this.lblType_ID.Values.Text = "客户厂商类型";
            // 
            // cmbType_ID
            // 
            this.cmbType_ID.DropDownWidth = 100;
            this.cmbType_ID.IntegralHeight = false;
            this.cmbType_ID.Location = new System.Drawing.Point(173, 96);
            this.cmbType_ID.Name = "cmbType_ID";
            this.cmbType_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbType_ID.TabIndex = 4;
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(100, 125);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(49, 20);
            this.lblEmployee_ID.TabIndex = 5;
            this.lblEmployee_ID.Values.Text = "责任人";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(173, 121);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbEmployee_ID.TabIndex = 5;
            // 
            // lblIsExclusive
            // 
            this.lblIsExclusive.Location = new System.Drawing.Point(100, 150);
            this.lblIsExclusive.Name = "lblIsExclusive";
            this.lblIsExclusive.Size = new System.Drawing.Size(75, 20);
            this.lblIsExclusive.TabIndex = 6;
            this.lblIsExclusive.Values.Text = "责任人专属";
            // 
            // chkIsExclusive
            // 
            this.chkIsExclusive.Checked = true;
            this.chkIsExclusive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsExclusive.Location = new System.Drawing.Point(173, 146);
            this.chkIsExclusive.Name = "chkIsExclusive";
            this.chkIsExclusive.Size = new System.Drawing.Size(19, 13);
            this.chkIsExclusive.TabIndex = 6;
            this.chkIsExclusive.Values.Text = "";
            // 
            // lblPaytype_ID
            // 
            this.lblPaytype_ID.Location = new System.Drawing.Point(100, 175);
            this.lblPaytype_ID.Name = "lblPaytype_ID";
            this.lblPaytype_ID.Size = new System.Drawing.Size(88, 20);
            this.lblPaytype_ID.TabIndex = 7;
            this.lblPaytype_ID.Values.Text = "默认交易方式";
            // 
            // cmbPaytype_ID
            // 
            this.cmbPaytype_ID.DropDownWidth = 100;
            this.cmbPaytype_ID.IntegralHeight = false;
            this.cmbPaytype_ID.Location = new System.Drawing.Point(173, 171);
            this.cmbPaytype_ID.Name = "cmbPaytype_ID";
            this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbPaytype_ID.TabIndex = 7;
            // 
            // lblCustomer_id
            // 
            this.lblCustomer_id.Location = new System.Drawing.Point(100, 200);
            this.lblCustomer_id.Name = "lblCustomer_id";
            this.lblCustomer_id.Size = new System.Drawing.Size(62, 20);
            this.lblCustomer_id.TabIndex = 8;
            this.lblCustomer_id.Values.Text = "目标客户";
            // 
            // txtCustomer_id
            // 
            this.txtCustomer_id.Location = new System.Drawing.Point(173, 196);
            this.txtCustomer_id.Name = "txtCustomer_id";
            this.txtCustomer_id.Size = new System.Drawing.Size(100, 23);
            this.txtCustomer_id.TabIndex = 8;
            // 
            // lblArea
            // 
            this.lblArea.Location = new System.Drawing.Point(100, 225);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(62, 20);
            this.lblArea.TabIndex = 9;
            this.lblArea.Values.Text = "所在地区";
            // 
            // txtArea
            // 
            this.txtArea.Location = new System.Drawing.Point(173, 221);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(100, 23);
            this.txtArea.TabIndex = 9;
            // 
            // lblContact
            // 
            this.lblContact.Location = new System.Drawing.Point(100, 250);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(49, 20);
            this.lblContact.TabIndex = 10;
            this.lblContact.Values.Text = "联系人";
            // 
            // txtContact
            // 
            this.txtContact.Location = new System.Drawing.Point(173, 246);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(100, 23);
            this.txtContact.TabIndex = 10;
            // 
            // lblPhone
            // 
            this.lblPhone.Location = new System.Drawing.Point(100, 275);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(36, 20);
            this.lblPhone.TabIndex = 11;
            this.lblPhone.Values.Text = "电话";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(173, 271);
            this.txtPhone.Multiline = true;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(100, 21);
            this.txtPhone.TabIndex = 11;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(100, 300);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(36, 20);
            this.lblAddress.TabIndex = 12;
            this.lblAddress.Values.Text = "地址";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(173, 296);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(100, 21);
            this.txtAddress.TabIndex = 12;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Location = new System.Drawing.Point(100, 325);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(36, 20);
            this.lblWebsite.TabIndex = 13;
            this.lblWebsite.Values.Text = "网址";
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(173, 321);
            this.txtWebsite.Multiline = true;
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(100, 21);
            this.txtWebsite.TabIndex = 13;
            // 
            // lblCreditLimit
            // 
            this.lblCreditLimit.Location = new System.Drawing.Point(100, 350);
            this.lblCreditLimit.Name = "lblCreditLimit";
            this.lblCreditLimit.Size = new System.Drawing.Size(62, 20);
            this.lblCreditLimit.TabIndex = 14;
            this.lblCreditLimit.Values.Text = "信用额度";
            // 
            // txtCreditLimit
            // 
            this.txtCreditLimit.Location = new System.Drawing.Point(173, 346);
            this.txtCreditLimit.Name = "txtCreditLimit";
            this.txtCreditLimit.Size = new System.Drawing.Size(100, 23);
            this.txtCreditLimit.TabIndex = 14;
            // 
            // lblCreditDays
            // 
            this.lblCreditDays.Location = new System.Drawing.Point(100, 375);
            this.lblCreditDays.Name = "lblCreditDays";
            this.lblCreditDays.Size = new System.Drawing.Size(62, 20);
            this.lblCreditDays.TabIndex = 15;
            this.lblCreditDays.Values.Text = "账期天数";
            // 
            // txtCreditDays
            // 
            this.txtCreditDays.Location = new System.Drawing.Point(173, 371);
            this.txtCreditDays.Name = "txtCreditDays";
            this.txtCreditDays.Size = new System.Drawing.Size(100, 23);
            this.txtCreditDays.TabIndex = 15;
            // 
            // lblIsCustomer
            // 
            this.lblIsCustomer.Location = new System.Drawing.Point(100, 400);
            this.lblIsCustomer.Name = "lblIsCustomer";
            this.lblIsCustomer.Size = new System.Drawing.Size(49, 20);
            this.lblIsCustomer.TabIndex = 16;
            this.lblIsCustomer.Values.Text = "是客户";
            // 
            // chkIsCustomer
            // 
            this.chkIsCustomer.Location = new System.Drawing.Point(173, 396);
            this.chkIsCustomer.Name = "chkIsCustomer";
            this.chkIsCustomer.Size = new System.Drawing.Size(19, 13);
            this.chkIsCustomer.TabIndex = 16;
            this.chkIsCustomer.Values.Text = "";
            // 
            // lblIsVendor
            // 
            this.lblIsVendor.Location = new System.Drawing.Point(100, 425);
            this.lblIsVendor.Name = "lblIsVendor";
            this.lblIsVendor.Size = new System.Drawing.Size(62, 20);
            this.lblIsVendor.TabIndex = 17;
            this.lblIsVendor.Values.Text = "是供应商";
            // 
            // chkIsVendor
            // 
            this.chkIsVendor.Location = new System.Drawing.Point(173, 421);
            this.chkIsVendor.Name = "chkIsVendor";
            this.chkIsVendor.Size = new System.Drawing.Size(19, 13);
            this.chkIsVendor.TabIndex = 17;
            this.chkIsVendor.Values.Text = "";
            // 
            // lblIsOther
            // 
            this.lblIsOther.Location = new System.Drawing.Point(100, 450);
            this.lblIsOther.Name = "lblIsOther";
            this.lblIsOther.Size = new System.Drawing.Size(49, 20);
            this.lblIsOther.TabIndex = 18;
            this.lblIsOther.Values.Text = "是其他";
            // 
            // chkIsOther
            // 
            this.chkIsOther.Location = new System.Drawing.Point(173, 446);
            this.chkIsOther.Name = "chkIsOther";
            this.chkIsOther.Size = new System.Drawing.Size(19, 13);
            this.chkIsOther.TabIndex = 18;
            this.chkIsOther.Values.Text = "";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(100, 475);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 19;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(173, 471);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(100, 21);
            this.txtNotes.TabIndex = 19;
            // 
            // lblCreated_at
            // 
            this.lblCreated_at.Location = new System.Drawing.Point(100, 500);
            this.lblCreated_at.Name = "lblCreated_at";
            this.lblCreated_at.Size = new System.Drawing.Size(62, 20);
            this.lblCreated_at.TabIndex = 20;
            this.lblCreated_at.Values.Text = "创建时间";
            // 
            // dtpCreated_at
            // 
            this.dtpCreated_at.Location = new System.Drawing.Point(173, 496);
            this.dtpCreated_at.Name = "dtpCreated_at";
            this.dtpCreated_at.ShowCheckBox = true;
            this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
            this.dtpCreated_at.TabIndex = 20;
            // 
            // lblCreated_by
            // 
            this.lblCreated_by.Location = new System.Drawing.Point(100, 525);
            this.lblCreated_by.Name = "lblCreated_by";
            this.lblCreated_by.Size = new System.Drawing.Size(49, 20);
            this.lblCreated_by.TabIndex = 21;
            this.lblCreated_by.Values.Text = "创建人";
            // 
            // txtCreated_by
            // 
            this.txtCreated_by.Location = new System.Drawing.Point(173, 521);
            this.txtCreated_by.Name = "txtCreated_by";
            this.txtCreated_by.Size = new System.Drawing.Size(100, 23);
            this.txtCreated_by.TabIndex = 21;
            // 
            // lblModified_at
            // 
            this.lblModified_at.Location = new System.Drawing.Point(100, 550);
            this.lblModified_at.Name = "lblModified_at";
            this.lblModified_at.Size = new System.Drawing.Size(62, 20);
            this.lblModified_at.TabIndex = 22;
            this.lblModified_at.Values.Text = "修改时间";
            // 
            // dtpModified_at
            // 
            this.dtpModified_at.Location = new System.Drawing.Point(173, 546);
            this.dtpModified_at.Name = "dtpModified_at";
            this.dtpModified_at.ShowCheckBox = true;
            this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
            this.dtpModified_at.TabIndex = 22;
            // 
            // lblModified_by
            // 
            this.lblModified_by.Location = new System.Drawing.Point(100, 575);
            this.lblModified_by.Name = "lblModified_by";
            this.lblModified_by.Size = new System.Drawing.Size(49, 20);
            this.lblModified_by.TabIndex = 23;
            this.lblModified_by.Values.Text = "修改人";
            // 
            // txtModified_by
            // 
            this.txtModified_by.Location = new System.Drawing.Point(173, 571);
            this.txtModified_by.Name = "txtModified_by";
            this.txtModified_by.Size = new System.Drawing.Size(100, 23);
            this.txtModified_by.TabIndex = 23;
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(100, 600);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 24;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Checked = true;
            this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIs_enabled.Location = new System.Drawing.Point(173, 596);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 24;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblIs_available
            // 
            this.lblIs_available.Location = new System.Drawing.Point(100, 625);
            this.lblIs_available.Name = "lblIs_available";
            this.lblIs_available.Size = new System.Drawing.Size(62, 20);
            this.lblIs_available.TabIndex = 25;
            this.lblIs_available.Values.Text = "是否可用";
            // 
            // chkIs_available
            // 
            this.chkIs_available.Checked = true;
            this.chkIs_available.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIs_available.Location = new System.Drawing.Point(173, 621);
            this.chkIs_available.Name = "chkIs_available";
            this.chkIs_available.Size = new System.Drawing.Size(19, 13);
            this.chkIs_available.TabIndex = 25;
            this.chkIs_available.Values.Text = "";
            // 
            // lblisdeleted
            // 
            this.lblisdeleted.Location = new System.Drawing.Point(100, 650);
            this.lblisdeleted.Name = "lblisdeleted";
            this.lblisdeleted.Size = new System.Drawing.Size(62, 20);
            this.lblisdeleted.TabIndex = 26;
            this.lblisdeleted.Values.Text = "逻辑删除";
            // 
            // chkisdeleted
            // 
            this.chkisdeleted.Location = new System.Drawing.Point(173, 646);
            this.chkisdeleted.Name = "chkisdeleted";
            this.chkisdeleted.Size = new System.Drawing.Size(19, 13);
            this.chkisdeleted.TabIndex = 26;
            this.chkisdeleted.Values.Text = "";
            // 
            // tb_CustomerVendorEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCVCode);
            this.Controls.Add(this.txtCVCode);
            this.Controls.Add(this.lblCVName);
            this.Controls.Add(this.txtCVName);
            this.Controls.Add(this.lblShortName);
            this.Controls.Add(this.txtShortName);
            this.Controls.Add(this.lblType_ID);
            this.Controls.Add(this.cmbType_ID);
            this.Controls.Add(this.lblEmployee_ID);
            this.Controls.Add(this.cmbEmployee_ID);
            this.Controls.Add(this.lblIsExclusive);
            this.Controls.Add(this.chkIsExclusive);
            this.Controls.Add(this.lblPaytype_ID);
            this.Controls.Add(this.cmbPaytype_ID);
            this.Controls.Add(this.lblCustomer_id);
            this.Controls.Add(this.txtCustomer_id);
            this.Controls.Add(this.lblArea);
            this.Controls.Add(this.txtArea);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.txtContact);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblWebsite);
            this.Controls.Add(this.txtWebsite);
            this.Controls.Add(this.lblCreditLimit);
            this.Controls.Add(this.txtCreditLimit);
            this.Controls.Add(this.lblCreditDays);
            this.Controls.Add(this.txtCreditDays);
            this.Controls.Add(this.lblIsCustomer);
            this.Controls.Add(this.chkIsCustomer);
            this.Controls.Add(this.lblIsVendor);
            this.Controls.Add(this.chkIsVendor);
            this.Controls.Add(this.lblIsOther);
            this.Controls.Add(this.chkIsOther);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblCreated_at);
            this.Controls.Add(this.dtpCreated_at);
            this.Controls.Add(this.lblCreated_by);
            this.Controls.Add(this.txtCreated_by);
            this.Controls.Add(this.lblModified_at);
            this.Controls.Add(this.dtpModified_at);
            this.Controls.Add(this.lblModified_by);
            this.Controls.Add(this.txtModified_by);
            this.Controls.Add(this.lblIs_enabled);
            this.Controls.Add(this.chkIs_enabled);
            this.Controls.Add(this.lblIs_available);
            this.Controls.Add(this.chkIs_available);
            this.Controls.Add(this.lblisdeleted);
            this.Controls.Add(this.chkisdeleted);
            this.Name = "tb_CustomerVendorEdit";
            this.Size = new System.Drawing.Size(911, 707);
            ((System.ComponentModel.ISupportInitialize)(this.cmbType_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPaytype_ID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCVCode;
private Krypton.Toolkit.KryptonTextBox txtCVCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblCVName;
private Krypton.Toolkit.KryptonTextBox txtCVName;

    
        
              private Krypton.Toolkit.KryptonLabel lblShortName;
private Krypton.Toolkit.KryptonTextBox txtShortName;

    
        
              private Krypton.Toolkit.KryptonLabel lblType_ID;
private Krypton.Toolkit.KryptonComboBox cmbType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsExclusive;
private Krypton.Toolkit.KryptonCheckBox chkIsExclusive;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomer_id;
private Krypton.Toolkit.KryptonTextBox txtCustomer_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblArea;
private Krypton.Toolkit.KryptonTextBox txtArea;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact;
private Krypton.Toolkit.KryptonTextBox txtContact;

    
        
              private Krypton.Toolkit.KryptonLabel lblPhone;
private Krypton.Toolkit.KryptonTextBox txtPhone;

    
        
              private Krypton.Toolkit.KryptonLabel lblAddress;
private Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblWebsite;
private Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreditLimit;
private Krypton.Toolkit.KryptonTextBox txtCreditLimit;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreditDays;
private Krypton.Toolkit.KryptonTextBox txtCreditDays;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsCustomer;
private Krypton.Toolkit.KryptonCheckBox chkIsCustomer;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsVendor;
private Krypton.Toolkit.KryptonCheckBox chkIsVendor;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsOther;
private Krypton.Toolkit.KryptonCheckBox chkIsOther;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_available;
private Krypton.Toolkit.KryptonCheckBox chkIs_available;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

