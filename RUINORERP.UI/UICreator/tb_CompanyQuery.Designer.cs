
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:16
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
    partial class tb_CompanyQuery
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
     
     this.lblCompanyCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCompanyCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblENName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtENName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblShortName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShortName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLegalPersonName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLegalPersonName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUnifiedSocialCreditIdentifier = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnifiedSocialCreditIdentifier = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPhone = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPhone = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFax = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtAddress.Multiline = true;

this.lblENAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtENAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtENAddress.Multiline = true;

this.lblWebsite = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWebsite = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtWebsite.Multiline = true;

this.lblEmail = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEmail = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInvoiceTitle = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInvoiceTitle = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInvoiceTaxNumber = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInvoiceTaxNumber = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInvoiceAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInvoiceAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInvoiceTEL = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInvoiceTEL = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInvoiceBankAccount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInvoiceBankAccount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInvoiceBankName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInvoiceBankName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30CompanyCode###String
this.lblCompanyCode.AutoSize = true;
this.lblCompanyCode.Location = new System.Drawing.Point(100,25);
this.lblCompanyCode.Name = "lblCompanyCode";
this.lblCompanyCode.Size = new System.Drawing.Size(41, 12);
this.lblCompanyCode.TabIndex = 1;
this.lblCompanyCode.Text = "公司代号";
this.txtCompanyCode.Location = new System.Drawing.Point(173,21);
this.txtCompanyCode.Name = "txtCompanyCode";
this.txtCompanyCode.Size = new System.Drawing.Size(100, 21);
this.txtCompanyCode.TabIndex = 1;
this.Controls.Add(this.lblCompanyCode);
this.Controls.Add(this.txtCompanyCode);

           //#####100CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,50);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 2;
this.lblCNName.Text = "名称";
this.txtCNName.Location = new System.Drawing.Point(173,46);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 2;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####100ENName###String
this.lblENName.AutoSize = true;
this.lblENName.Location = new System.Drawing.Point(100,75);
this.lblENName.Name = "lblENName";
this.lblENName.Size = new System.Drawing.Size(41, 12);
this.lblENName.TabIndex = 3;
this.lblENName.Text = "英语名称";
this.txtENName.Location = new System.Drawing.Point(173,71);
this.txtENName.Name = "txtENName";
this.txtENName.Size = new System.Drawing.Size(100, 21);
this.txtENName.TabIndex = 3;
this.Controls.Add(this.lblENName);
this.Controls.Add(this.txtENName);

           //#####50ShortName###String
this.lblShortName.AutoSize = true;
this.lblShortName.Location = new System.Drawing.Point(100,100);
this.lblShortName.Name = "lblShortName";
this.lblShortName.Size = new System.Drawing.Size(41, 12);
this.lblShortName.TabIndex = 4;
this.lblShortName.Text = "简称";
this.txtShortName.Location = new System.Drawing.Point(173,96);
this.txtShortName.Name = "txtShortName";
this.txtShortName.Size = new System.Drawing.Size(100, 21);
this.txtShortName.TabIndex = 4;
this.Controls.Add(this.lblShortName);
this.Controls.Add(this.txtShortName);

           //#####50LegalPersonName###String
this.lblLegalPersonName.AutoSize = true;
this.lblLegalPersonName.Location = new System.Drawing.Point(100,125);
this.lblLegalPersonName.Name = "lblLegalPersonName";
this.lblLegalPersonName.Size = new System.Drawing.Size(41, 12);
this.lblLegalPersonName.TabIndex = 5;
this.lblLegalPersonName.Text = "法人姓名";
this.txtLegalPersonName.Location = new System.Drawing.Point(173,121);
this.txtLegalPersonName.Name = "txtLegalPersonName";
this.txtLegalPersonName.Size = new System.Drawing.Size(100, 21);
this.txtLegalPersonName.TabIndex = 5;
this.Controls.Add(this.lblLegalPersonName);
this.Controls.Add(this.txtLegalPersonName);

           //#####50UnifiedSocialCreditIdentifier###String
this.lblUnifiedSocialCreditIdentifier.AutoSize = true;
this.lblUnifiedSocialCreditIdentifier.Location = new System.Drawing.Point(100,150);
this.lblUnifiedSocialCreditIdentifier.Name = "lblUnifiedSocialCreditIdentifier";
this.lblUnifiedSocialCreditIdentifier.Size = new System.Drawing.Size(41, 12);
this.lblUnifiedSocialCreditIdentifier.TabIndex = 6;
this.lblUnifiedSocialCreditIdentifier.Text = "公司执照代码";
this.txtUnifiedSocialCreditIdentifier.Location = new System.Drawing.Point(173,146);
this.txtUnifiedSocialCreditIdentifier.Name = "txtUnifiedSocialCreditIdentifier";
this.txtUnifiedSocialCreditIdentifier.Size = new System.Drawing.Size(100, 21);
this.txtUnifiedSocialCreditIdentifier.TabIndex = 6;
this.Controls.Add(this.lblUnifiedSocialCreditIdentifier);
this.Controls.Add(this.txtUnifiedSocialCreditIdentifier);

           //#####100Contact###String
this.lblContact.AutoSize = true;
this.lblContact.Location = new System.Drawing.Point(100,175);
this.lblContact.Name = "lblContact";
this.lblContact.Size = new System.Drawing.Size(41, 12);
this.lblContact.TabIndex = 7;
this.lblContact.Text = "联系人";
this.txtContact.Location = new System.Drawing.Point(173,171);
this.txtContact.Name = "txtContact";
this.txtContact.Size = new System.Drawing.Size(100, 21);
this.txtContact.TabIndex = 7;
this.Controls.Add(this.lblContact);
this.Controls.Add(this.txtContact);

           //#####100Phone###String
this.lblPhone.AutoSize = true;
this.lblPhone.Location = new System.Drawing.Point(100,200);
this.lblPhone.Name = "lblPhone";
this.lblPhone.Size = new System.Drawing.Size(41, 12);
this.lblPhone.TabIndex = 8;
this.lblPhone.Text = "电话";
this.txtPhone.Location = new System.Drawing.Point(173,196);
this.txtPhone.Name = "txtPhone";
this.txtPhone.Size = new System.Drawing.Size(100, 21);
this.txtPhone.TabIndex = 8;
this.Controls.Add(this.lblPhone);
this.Controls.Add(this.txtPhone);

           //#####100Fax###String
this.lblFax.AutoSize = true;
this.lblFax.Location = new System.Drawing.Point(100,225);
this.lblFax.Name = "lblFax";
this.lblFax.Size = new System.Drawing.Size(41, 12);
this.lblFax.TabIndex = 9;
this.lblFax.Text = "传真";
this.txtFax.Location = new System.Drawing.Point(173,221);
this.txtFax.Name = "txtFax";
this.txtFax.Size = new System.Drawing.Size(100, 21);
this.txtFax.TabIndex = 9;
this.Controls.Add(this.lblFax);
this.Controls.Add(this.txtFax);

           //#####255Address###String
this.lblAddress.AutoSize = true;
this.lblAddress.Location = new System.Drawing.Point(100,250);
this.lblAddress.Name = "lblAddress";
this.lblAddress.Size = new System.Drawing.Size(41, 12);
this.lblAddress.TabIndex = 10;
this.lblAddress.Text = "营业地址";
this.txtAddress.Location = new System.Drawing.Point(173,246);
this.txtAddress.Name = "txtAddress";
this.txtAddress.Size = new System.Drawing.Size(100, 21);
this.txtAddress.TabIndex = 10;
this.Controls.Add(this.lblAddress);
this.Controls.Add(this.txtAddress);

           //#####255ENAddress###String
this.lblENAddress.AutoSize = true;
this.lblENAddress.Location = new System.Drawing.Point(100,275);
this.lblENAddress.Name = "lblENAddress";
this.lblENAddress.Size = new System.Drawing.Size(41, 12);
this.lblENAddress.TabIndex = 11;
this.lblENAddress.Text = "英文地址";
this.txtENAddress.Location = new System.Drawing.Point(173,271);
this.txtENAddress.Name = "txtENAddress";
this.txtENAddress.Size = new System.Drawing.Size(100, 21);
this.txtENAddress.TabIndex = 11;
this.Controls.Add(this.lblENAddress);
this.Controls.Add(this.txtENAddress);

           //#####255Website###String
this.lblWebsite.AutoSize = true;
this.lblWebsite.Location = new System.Drawing.Point(100,300);
this.lblWebsite.Name = "lblWebsite";
this.lblWebsite.Size = new System.Drawing.Size(41, 12);
this.lblWebsite.TabIndex = 12;
this.lblWebsite.Text = "网址";
this.txtWebsite.Location = new System.Drawing.Point(173,296);
this.txtWebsite.Name = "txtWebsite";
this.txtWebsite.Size = new System.Drawing.Size(100, 21);
this.txtWebsite.TabIndex = 12;
this.Controls.Add(this.lblWebsite);
this.Controls.Add(this.txtWebsite);

           //#####100Email###String
this.lblEmail.AutoSize = true;
this.lblEmail.Location = new System.Drawing.Point(100,325);
this.lblEmail.Name = "lblEmail";
this.lblEmail.Size = new System.Drawing.Size(41, 12);
this.lblEmail.TabIndex = 13;
this.lblEmail.Text = "电子邮件";
this.txtEmail.Location = new System.Drawing.Point(173,321);
this.txtEmail.Name = "txtEmail";
this.txtEmail.Size = new System.Drawing.Size(100, 21);
this.txtEmail.TabIndex = 13;
this.Controls.Add(this.lblEmail);
this.Controls.Add(this.txtEmail);

           //#####200InvoiceTitle###String
this.lblInvoiceTitle.AutoSize = true;
this.lblInvoiceTitle.Location = new System.Drawing.Point(100,350);
this.lblInvoiceTitle.Name = "lblInvoiceTitle";
this.lblInvoiceTitle.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceTitle.TabIndex = 14;
this.lblInvoiceTitle.Text = "发票抬头";
this.txtInvoiceTitle.Location = new System.Drawing.Point(173,346);
this.txtInvoiceTitle.Name = "txtInvoiceTitle";
this.txtInvoiceTitle.Size = new System.Drawing.Size(100, 21);
this.txtInvoiceTitle.TabIndex = 14;
this.Controls.Add(this.lblInvoiceTitle);
this.Controls.Add(this.txtInvoiceTitle);

           //#####200InvoiceTaxNumber###String
this.lblInvoiceTaxNumber.AutoSize = true;
this.lblInvoiceTaxNumber.Location = new System.Drawing.Point(100,375);
this.lblInvoiceTaxNumber.Name = "lblInvoiceTaxNumber";
this.lblInvoiceTaxNumber.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceTaxNumber.TabIndex = 15;
this.lblInvoiceTaxNumber.Text = "纳税人识别号";
this.txtInvoiceTaxNumber.Location = new System.Drawing.Point(173,371);
this.txtInvoiceTaxNumber.Name = "txtInvoiceTaxNumber";
this.txtInvoiceTaxNumber.Size = new System.Drawing.Size(100, 21);
this.txtInvoiceTaxNumber.TabIndex = 15;
this.Controls.Add(this.lblInvoiceTaxNumber);
this.Controls.Add(this.txtInvoiceTaxNumber);

           //#####200InvoiceAddress###String
this.lblInvoiceAddress.AutoSize = true;
this.lblInvoiceAddress.Location = new System.Drawing.Point(100,400);
this.lblInvoiceAddress.Name = "lblInvoiceAddress";
this.lblInvoiceAddress.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceAddress.TabIndex = 16;
this.lblInvoiceAddress.Text = "发票地址";
this.txtInvoiceAddress.Location = new System.Drawing.Point(173,396);
this.txtInvoiceAddress.Name = "txtInvoiceAddress";
this.txtInvoiceAddress.Size = new System.Drawing.Size(100, 21);
this.txtInvoiceAddress.TabIndex = 16;
this.Controls.Add(this.lblInvoiceAddress);
this.Controls.Add(this.txtInvoiceAddress);

           //#####50InvoiceTEL###String
this.lblInvoiceTEL.AutoSize = true;
this.lblInvoiceTEL.Location = new System.Drawing.Point(100,425);
this.lblInvoiceTEL.Name = "lblInvoiceTEL";
this.lblInvoiceTEL.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceTEL.TabIndex = 17;
this.lblInvoiceTEL.Text = "发票电话";
this.txtInvoiceTEL.Location = new System.Drawing.Point(173,421);
this.txtInvoiceTEL.Name = "txtInvoiceTEL";
this.txtInvoiceTEL.Size = new System.Drawing.Size(100, 21);
this.txtInvoiceTEL.TabIndex = 17;
this.Controls.Add(this.lblInvoiceTEL);
this.Controls.Add(this.txtInvoiceTEL);

           //#####150InvoiceBankAccount###String
this.lblInvoiceBankAccount.AutoSize = true;
this.lblInvoiceBankAccount.Location = new System.Drawing.Point(100,450);
this.lblInvoiceBankAccount.Name = "lblInvoiceBankAccount";
this.lblInvoiceBankAccount.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceBankAccount.TabIndex = 18;
this.lblInvoiceBankAccount.Text = "银行账号";
this.txtInvoiceBankAccount.Location = new System.Drawing.Point(173,446);
this.txtInvoiceBankAccount.Name = "txtInvoiceBankAccount";
this.txtInvoiceBankAccount.Size = new System.Drawing.Size(100, 21);
this.txtInvoiceBankAccount.TabIndex = 18;
this.Controls.Add(this.lblInvoiceBankAccount);
this.Controls.Add(this.txtInvoiceBankAccount);

           //#####100InvoiceBankName###String
this.lblInvoiceBankName.AutoSize = true;
this.lblInvoiceBankName.Location = new System.Drawing.Point(100,475);
this.lblInvoiceBankName.Name = "lblInvoiceBankName";
this.lblInvoiceBankName.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceBankName.TabIndex = 19;
this.lblInvoiceBankName.Text = "开户行";
this.txtInvoiceBankName.Location = new System.Drawing.Point(173,471);
this.txtInvoiceBankName.Name = "txtInvoiceBankName";
this.txtInvoiceBankName.Size = new System.Drawing.Size(100, 21);
this.txtInvoiceBankName.TabIndex = 19;
this.Controls.Add(this.lblInvoiceBankName);
this.Controls.Add(this.txtInvoiceBankName);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,500);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 20;
this.lblCreated_at.Text = "创建时间";
//111======500
this.dtpCreated_at.Location = new System.Drawing.Point(173,496);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 20;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,550);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 22;
this.lblModified_at.Text = "修改时间";
//111======550
this.dtpModified_at.Location = new System.Drawing.Point(173,546);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 22;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,600);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 24;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,596);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 24;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCompanyCode );
this.Controls.Add(this.txtCompanyCode );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblENName );
this.Controls.Add(this.txtENName );

                this.Controls.Add(this.lblShortName );
this.Controls.Add(this.txtShortName );

                this.Controls.Add(this.lblLegalPersonName );
this.Controls.Add(this.txtLegalPersonName );

                this.Controls.Add(this.lblUnifiedSocialCreditIdentifier );
this.Controls.Add(this.txtUnifiedSocialCreditIdentifier );

                this.Controls.Add(this.lblContact );
this.Controls.Add(this.txtContact );

                this.Controls.Add(this.lblPhone );
this.Controls.Add(this.txtPhone );

                this.Controls.Add(this.lblFax );
this.Controls.Add(this.txtFax );

                this.Controls.Add(this.lblAddress );
this.Controls.Add(this.txtAddress );

                this.Controls.Add(this.lblENAddress );
this.Controls.Add(this.txtENAddress );

                this.Controls.Add(this.lblWebsite );
this.Controls.Add(this.txtWebsite );

                this.Controls.Add(this.lblEmail );
this.Controls.Add(this.txtEmail );

                this.Controls.Add(this.lblInvoiceTitle );
this.Controls.Add(this.txtInvoiceTitle );

                this.Controls.Add(this.lblInvoiceTaxNumber );
this.Controls.Add(this.txtInvoiceTaxNumber );

                this.Controls.Add(this.lblInvoiceAddress );
this.Controls.Add(this.txtInvoiceAddress );

                this.Controls.Add(this.lblInvoiceTEL );
this.Controls.Add(this.txtInvoiceTEL );

                this.Controls.Add(this.lblInvoiceBankAccount );
this.Controls.Add(this.txtInvoiceBankAccount );

                this.Controls.Add(this.lblInvoiceBankName );
this.Controls.Add(this.txtInvoiceBankName );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "tb_CompanyQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCompanyCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCompanyCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblENName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtENName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShortName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShortName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLegalPersonName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLegalPersonName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnifiedSocialCreditIdentifier;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnifiedSocialCreditIdentifier;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPhone;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPhone;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFax;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblENAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtENAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWebsite;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmail;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEmail;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiceTitle;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInvoiceTitle;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiceTaxNumber;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInvoiceTaxNumber;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiceAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInvoiceAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiceTEL;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInvoiceTEL;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiceBankAccount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInvoiceBankAccount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiceBankName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInvoiceBankName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


