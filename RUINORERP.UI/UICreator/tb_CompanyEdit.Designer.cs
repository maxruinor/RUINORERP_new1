// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/12/2025 21:15:58
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 系统使用者公司
    /// </summary>
    partial class tb_CompanyEdit
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
            this.lblCompanyCode = new Krypton.Toolkit.KryptonLabel();
            this.txtCompanyCode = new Krypton.Toolkit.KryptonTextBox();
            this.lblCNName = new Krypton.Toolkit.KryptonLabel();
            this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
            this.lblENName = new Krypton.Toolkit.KryptonLabel();
            this.txtENName = new Krypton.Toolkit.KryptonTextBox();
            this.lblShortName = new Krypton.Toolkit.KryptonLabel();
            this.txtShortName = new Krypton.Toolkit.KryptonTextBox();
            this.lblLegalPersonName = new Krypton.Toolkit.KryptonLabel();
            this.txtLegalPersonName = new Krypton.Toolkit.KryptonTextBox();
            this.lblUnifiedSocialCreditIdentifier = new Krypton.Toolkit.KryptonLabel();
            this.txtUnifiedSocialCreditIdentifier = new Krypton.Toolkit.KryptonTextBox();
            this.lblContact = new Krypton.Toolkit.KryptonLabel();
            this.txtContact = new Krypton.Toolkit.KryptonTextBox();
            this.lblPhone = new Krypton.Toolkit.KryptonLabel();
            this.txtPhone = new Krypton.Toolkit.KryptonTextBox();
            this.lblAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblENAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtENAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblWebsite = new Krypton.Toolkit.KryptonLabel();
            this.txtWebsite = new Krypton.Toolkit.KryptonTextBox();
            this.lblEmail = new Krypton.Toolkit.KryptonLabel();
            this.txtEmail = new Krypton.Toolkit.KryptonTextBox();
            this.lblInvoiceTitle = new Krypton.Toolkit.KryptonLabel();
            this.txtInvoiceTitle = new Krypton.Toolkit.KryptonTextBox();
            this.lblInvoiceTaxNumber = new Krypton.Toolkit.KryptonLabel();
            this.txtInvoiceTaxNumber = new Krypton.Toolkit.KryptonTextBox();
            this.lblInvoiceAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtInvoiceAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblInvoiceTEL = new Krypton.Toolkit.KryptonLabel();
            this.txtInvoiceTEL = new Krypton.Toolkit.KryptonTextBox();
            this.lblInvoiceBankAccount = new Krypton.Toolkit.KryptonLabel();
            this.txtInvoiceBankAccount = new Krypton.Toolkit.KryptonTextBox();
            this.lblInvoiceBankName = new Krypton.Toolkit.KryptonLabel();
            this.txtInvoiceBankName = new Krypton.Toolkit.KryptonTextBox();
            this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
            this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
            this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.SuspendLayout();
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Location = new System.Drawing.Point(100, 25);
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Size = new System.Drawing.Size(62, 20);
            this.lblCompanyCode.TabIndex = 1;
            this.lblCompanyCode.Values.Text = "公司代号";
            // 
            // txtCompanyCode
            // 
            this.txtCompanyCode.Location = new System.Drawing.Point(173, 21);
            this.txtCompanyCode.Name = "txtCompanyCode";
            this.txtCompanyCode.Size = new System.Drawing.Size(100, 23);
            this.txtCompanyCode.TabIndex = 1;
            // 
            // lblCNName
            // 
            this.lblCNName.Location = new System.Drawing.Point(100, 50);
            this.lblCNName.Name = "lblCNName";
            this.lblCNName.Size = new System.Drawing.Size(36, 20);
            this.lblCNName.TabIndex = 2;
            this.lblCNName.Values.Text = "名称";
            // 
            // txtCNName
            // 
            this.txtCNName.Location = new System.Drawing.Point(173, 46);
            this.txtCNName.Name = "txtCNName";
            this.txtCNName.Size = new System.Drawing.Size(100, 23);
            this.txtCNName.TabIndex = 2;
            // 
            // lblENName
            // 
            this.lblENName.Location = new System.Drawing.Point(100, 75);
            this.lblENName.Name = "lblENName";
            this.lblENName.Size = new System.Drawing.Size(62, 20);
            this.lblENName.TabIndex = 3;
            this.lblENName.Values.Text = "英语名称";
            // 
            // txtENName
            // 
            this.txtENName.Location = new System.Drawing.Point(173, 71);
            this.txtENName.Name = "txtENName";
            this.txtENName.Size = new System.Drawing.Size(100, 23);
            this.txtENName.TabIndex = 3;
            // 
            // lblShortName
            // 
            this.lblShortName.Location = new System.Drawing.Point(100, 100);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(36, 20);
            this.lblShortName.TabIndex = 4;
            this.lblShortName.Values.Text = "简称";
            // 
            // txtShortName
            // 
            this.txtShortName.Location = new System.Drawing.Point(173, 96);
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(100, 23);
            this.txtShortName.TabIndex = 4;
            // 
            // lblLegalPersonName
            // 
            this.lblLegalPersonName.Location = new System.Drawing.Point(100, 125);
            this.lblLegalPersonName.Name = "lblLegalPersonName";
            this.lblLegalPersonName.Size = new System.Drawing.Size(62, 20);
            this.lblLegalPersonName.TabIndex = 5;
            this.lblLegalPersonName.Values.Text = "法人姓名";
            // 
            // txtLegalPersonName
            // 
            this.txtLegalPersonName.Location = new System.Drawing.Point(173, 121);
            this.txtLegalPersonName.Name = "txtLegalPersonName";
            this.txtLegalPersonName.Size = new System.Drawing.Size(100, 23);
            this.txtLegalPersonName.TabIndex = 5;
            // 
            // lblUnifiedSocialCreditIdentifier
            // 
            this.lblUnifiedSocialCreditIdentifier.Location = new System.Drawing.Point(100, 150);
            this.lblUnifiedSocialCreditIdentifier.Name = "lblUnifiedSocialCreditIdentifier";
            this.lblUnifiedSocialCreditIdentifier.Size = new System.Drawing.Size(88, 20);
            this.lblUnifiedSocialCreditIdentifier.TabIndex = 6;
            this.lblUnifiedSocialCreditIdentifier.Values.Text = "公司执照代码";
            // 
            // txtUnifiedSocialCreditIdentifier
            // 
            this.txtUnifiedSocialCreditIdentifier.Location = new System.Drawing.Point(173, 146);
            this.txtUnifiedSocialCreditIdentifier.Name = "txtUnifiedSocialCreditIdentifier";
            this.txtUnifiedSocialCreditIdentifier.Size = new System.Drawing.Size(100, 23);
            this.txtUnifiedSocialCreditIdentifier.TabIndex = 6;
            // 
            // lblContact
            // 
            this.lblContact.Location = new System.Drawing.Point(100, 175);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(49, 20);
            this.lblContact.TabIndex = 7;
            this.lblContact.Values.Text = "联系人";
            // 
            // txtContact
            // 
            this.txtContact.Location = new System.Drawing.Point(173, 171);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(100, 23);
            this.txtContact.TabIndex = 7;
            // 
            // lblPhone
            // 
            this.lblPhone.Location = new System.Drawing.Point(100, 200);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(36, 20);
            this.lblPhone.TabIndex = 8;
            this.lblPhone.Values.Text = "电话";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(173, 196);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(100, 23);
            this.txtPhone.TabIndex = 8;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(100, 225);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(62, 20);
            this.lblAddress.TabIndex = 9;
            this.lblAddress.Values.Text = "营业地址";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(173, 221);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(100, 21);
            this.txtAddress.TabIndex = 9;
            // 
            // lblENAddress
            // 
            this.lblENAddress.Location = new System.Drawing.Point(100, 250);
            this.lblENAddress.Name = "lblENAddress";
            this.lblENAddress.Size = new System.Drawing.Size(62, 20);
            this.lblENAddress.TabIndex = 10;
            this.lblENAddress.Values.Text = "英文地址";
            // 
            // txtENAddress
            // 
            this.txtENAddress.Location = new System.Drawing.Point(173, 246);
            this.txtENAddress.Multiline = true;
            this.txtENAddress.Name = "txtENAddress";
            this.txtENAddress.Size = new System.Drawing.Size(100, 21);
            this.txtENAddress.TabIndex = 10;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Location = new System.Drawing.Point(100, 275);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(36, 20);
            this.lblWebsite.TabIndex = 11;
            this.lblWebsite.Values.Text = "网址";
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(173, 271);
            this.txtWebsite.Multiline = true;
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(100, 21);
            this.txtWebsite.TabIndex = 11;
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(100, 300);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(62, 20);
            this.lblEmail.TabIndex = 12;
            this.lblEmail.Values.Text = "电子邮件";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(173, 296);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(100, 23);
            this.txtEmail.TabIndex = 12;
            // 
            // lblInvoiceTitle
            // 
            this.lblInvoiceTitle.Location = new System.Drawing.Point(100, 325);
            this.lblInvoiceTitle.Name = "lblInvoiceTitle";
            this.lblInvoiceTitle.Size = new System.Drawing.Size(62, 20);
            this.lblInvoiceTitle.TabIndex = 13;
            this.lblInvoiceTitle.Values.Text = "发票抬头";
            // 
            // txtInvoiceTitle
            // 
            this.txtInvoiceTitle.Location = new System.Drawing.Point(173, 321);
            this.txtInvoiceTitle.Name = "txtInvoiceTitle";
            this.txtInvoiceTitle.Size = new System.Drawing.Size(100, 23);
            this.txtInvoiceTitle.TabIndex = 13;
            // 
            // lblInvoiceTaxNumber
            // 
            this.lblInvoiceTaxNumber.Location = new System.Drawing.Point(100, 350);
            this.lblInvoiceTaxNumber.Name = "lblInvoiceTaxNumber";
            this.lblInvoiceTaxNumber.Size = new System.Drawing.Size(88, 20);
            this.lblInvoiceTaxNumber.TabIndex = 14;
            this.lblInvoiceTaxNumber.Values.Text = "纳税人识别号";
            // 
            // txtInvoiceTaxNumber
            // 
            this.txtInvoiceTaxNumber.Location = new System.Drawing.Point(173, 346);
            this.txtInvoiceTaxNumber.Name = "txtInvoiceTaxNumber";
            this.txtInvoiceTaxNumber.Size = new System.Drawing.Size(100, 23);
            this.txtInvoiceTaxNumber.TabIndex = 14;
            // 
            // lblInvoiceAddress
            // 
            this.lblInvoiceAddress.Location = new System.Drawing.Point(100, 375);
            this.lblInvoiceAddress.Name = "lblInvoiceAddress";
            this.lblInvoiceAddress.Size = new System.Drawing.Size(62, 20);
            this.lblInvoiceAddress.TabIndex = 15;
            this.lblInvoiceAddress.Values.Text = "发票地址";
            // 
            // txtInvoiceAddress
            // 
            this.txtInvoiceAddress.Location = new System.Drawing.Point(173, 371);
            this.txtInvoiceAddress.Name = "txtInvoiceAddress";
            this.txtInvoiceAddress.Size = new System.Drawing.Size(100, 23);
            this.txtInvoiceAddress.TabIndex = 15;
            // 
            // lblInvoiceTEL
            // 
            this.lblInvoiceTEL.Location = new System.Drawing.Point(100, 400);
            this.lblInvoiceTEL.Name = "lblInvoiceTEL";
            this.lblInvoiceTEL.Size = new System.Drawing.Size(62, 20);
            this.lblInvoiceTEL.TabIndex = 16;
            this.lblInvoiceTEL.Values.Text = "发票电话";
            // 
            // txtInvoiceTEL
            // 
            this.txtInvoiceTEL.Location = new System.Drawing.Point(173, 396);
            this.txtInvoiceTEL.Name = "txtInvoiceTEL";
            this.txtInvoiceTEL.Size = new System.Drawing.Size(100, 23);
            this.txtInvoiceTEL.TabIndex = 16;
            // 
            // lblInvoiceBankAccount
            // 
            this.lblInvoiceBankAccount.Location = new System.Drawing.Point(100, 425);
            this.lblInvoiceBankAccount.Name = "lblInvoiceBankAccount";
            this.lblInvoiceBankAccount.Size = new System.Drawing.Size(62, 20);
            this.lblInvoiceBankAccount.TabIndex = 17;
            this.lblInvoiceBankAccount.Values.Text = "银行账号";
            // 
            // txtInvoiceBankAccount
            // 
            this.txtInvoiceBankAccount.Location = new System.Drawing.Point(173, 421);
            this.txtInvoiceBankAccount.Name = "txtInvoiceBankAccount";
            this.txtInvoiceBankAccount.Size = new System.Drawing.Size(100, 23);
            this.txtInvoiceBankAccount.TabIndex = 17;
            // 
            // lblInvoiceBankName
            // 
            this.lblInvoiceBankName.Location = new System.Drawing.Point(100, 450);
            this.lblInvoiceBankName.Name = "lblInvoiceBankName";
            this.lblInvoiceBankName.Size = new System.Drawing.Size(49, 20);
            this.lblInvoiceBankName.TabIndex = 18;
            this.lblInvoiceBankName.Values.Text = "开户行";
            // 
            // txtInvoiceBankName
            // 
            this.txtInvoiceBankName.Location = new System.Drawing.Point(173, 446);
            this.txtInvoiceBankName.Name = "txtInvoiceBankName";
            this.txtInvoiceBankName.Size = new System.Drawing.Size(100, 23);
            this.txtInvoiceBankName.TabIndex = 18;
            // 
            // lblCreated_at
            // 
            this.lblCreated_at.Location = new System.Drawing.Point(100, 475);
            this.lblCreated_at.Name = "lblCreated_at";
            this.lblCreated_at.Size = new System.Drawing.Size(62, 20);
            this.lblCreated_at.TabIndex = 19;
            this.lblCreated_at.Values.Text = "创建时间";
            // 
            // dtpCreated_at
            // 
            this.dtpCreated_at.Location = new System.Drawing.Point(173, 471);
            this.dtpCreated_at.Name = "dtpCreated_at";
            this.dtpCreated_at.ShowCheckBox = true;
            this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
            this.dtpCreated_at.TabIndex = 19;
            // 
            // lblCreated_by
            // 
            this.lblCreated_by.Location = new System.Drawing.Point(100, 500);
            this.lblCreated_by.Name = "lblCreated_by";
            this.lblCreated_by.Size = new System.Drawing.Size(49, 20);
            this.lblCreated_by.TabIndex = 20;
            this.lblCreated_by.Values.Text = "创建人";
            // 
            // txtCreated_by
            // 
            this.txtCreated_by.Location = new System.Drawing.Point(173, 496);
            this.txtCreated_by.Name = "txtCreated_by";
            this.txtCreated_by.Size = new System.Drawing.Size(100, 23);
            this.txtCreated_by.TabIndex = 20;
            // 
            // lblModified_at
            // 
            this.lblModified_at.Location = new System.Drawing.Point(100, 525);
            this.lblModified_at.Name = "lblModified_at";
            this.lblModified_at.Size = new System.Drawing.Size(62, 20);
            this.lblModified_at.TabIndex = 21;
            this.lblModified_at.Values.Text = "修改时间";
            // 
            // dtpModified_at
            // 
            this.dtpModified_at.Location = new System.Drawing.Point(173, 521);
            this.dtpModified_at.Name = "dtpModified_at";
            this.dtpModified_at.ShowCheckBox = true;
            this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
            this.dtpModified_at.TabIndex = 21;
            // 
            // lblModified_by
            // 
            this.lblModified_by.Location = new System.Drawing.Point(100, 550);
            this.lblModified_by.Name = "lblModified_by";
            this.lblModified_by.Size = new System.Drawing.Size(49, 20);
            this.lblModified_by.TabIndex = 22;
            this.lblModified_by.Values.Text = "修改人";
            // 
            // txtModified_by
            // 
            this.txtModified_by.Location = new System.Drawing.Point(173, 546);
            this.txtModified_by.Name = "txtModified_by";
            this.txtModified_by.Size = new System.Drawing.Size(100, 23);
            this.txtModified_by.TabIndex = 22;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(100, 575);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 23;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(173, 571);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(100, 21);
            this.txtNotes.TabIndex = 23;
            // 
            // tb_CompanyEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCompanyCode);
            this.Controls.Add(this.txtCompanyCode);
            this.Controls.Add(this.lblCNName);
            this.Controls.Add(this.txtCNName);
            this.Controls.Add(this.lblENName);
            this.Controls.Add(this.txtENName);
            this.Controls.Add(this.lblShortName);
            this.Controls.Add(this.txtShortName);
            this.Controls.Add(this.lblLegalPersonName);
            this.Controls.Add(this.txtLegalPersonName);
            this.Controls.Add(this.lblUnifiedSocialCreditIdentifier);
            this.Controls.Add(this.txtUnifiedSocialCreditIdentifier);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.txtContact);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblENAddress);
            this.Controls.Add(this.txtENAddress);
            this.Controls.Add(this.lblWebsite);
            this.Controls.Add(this.txtWebsite);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblInvoiceTitle);
            this.Controls.Add(this.txtInvoiceTitle);
            this.Controls.Add(this.lblInvoiceTaxNumber);
            this.Controls.Add(this.txtInvoiceTaxNumber);
            this.Controls.Add(this.lblInvoiceAddress);
            this.Controls.Add(this.txtInvoiceAddress);
            this.Controls.Add(this.lblInvoiceTEL);
            this.Controls.Add(this.txtInvoiceTEL);
            this.Controls.Add(this.lblInvoiceBankAccount);
            this.Controls.Add(this.txtInvoiceBankAccount);
            this.Controls.Add(this.lblInvoiceBankName);
            this.Controls.Add(this.txtInvoiceBankName);
            this.Controls.Add(this.lblCreated_at);
            this.Controls.Add(this.dtpCreated_at);
            this.Controls.Add(this.lblCreated_by);
            this.Controls.Add(this.txtCreated_by);
            this.Controls.Add(this.lblModified_at);
            this.Controls.Add(this.dtpModified_at);
            this.Controls.Add(this.lblModified_by);
            this.Controls.Add(this.txtModified_by);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Name = "tb_CompanyEdit";
            this.Size = new System.Drawing.Size(911, 684);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCompanyCode;
private Krypton.Toolkit.KryptonTextBox txtCompanyCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblENName;
private Krypton.Toolkit.KryptonTextBox txtENName;

    
        
              private Krypton.Toolkit.KryptonLabel lblShortName;
private Krypton.Toolkit.KryptonTextBox txtShortName;

    
        
              private Krypton.Toolkit.KryptonLabel lblLegalPersonName;
private Krypton.Toolkit.KryptonTextBox txtLegalPersonName;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnifiedSocialCreditIdentifier;
private Krypton.Toolkit.KryptonTextBox txtUnifiedSocialCreditIdentifier;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact;
private Krypton.Toolkit.KryptonTextBox txtContact;

    
        
              private Krypton.Toolkit.KryptonLabel lblPhone;
private Krypton.Toolkit.KryptonTextBox txtPhone;

    
        
              private Krypton.Toolkit.KryptonLabel lblAddress;
private Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblENAddress;
private Krypton.Toolkit.KryptonTextBox txtENAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblWebsite;
private Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmail;
private Krypton.Toolkit.KryptonTextBox txtEmail;

    
        
              private Krypton.Toolkit.KryptonLabel lblInvoiceTitle;
private Krypton.Toolkit.KryptonTextBox txtInvoiceTitle;

    
        
              private Krypton.Toolkit.KryptonLabel lblInvoiceTaxNumber;
private Krypton.Toolkit.KryptonTextBox txtInvoiceTaxNumber;

    
        
              private Krypton.Toolkit.KryptonLabel lblInvoiceAddress;
private Krypton.Toolkit.KryptonTextBox txtInvoiceAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblInvoiceTEL;
private Krypton.Toolkit.KryptonTextBox txtInvoiceTEL;

    
        
              private Krypton.Toolkit.KryptonLabel lblInvoiceBankAccount;
private Krypton.Toolkit.KryptonTextBox txtInvoiceBankAccount;

    
        
              private Krypton.Toolkit.KryptonLabel lblInvoiceBankName;
private Krypton.Toolkit.KryptonTextBox txtInvoiceBankName;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

