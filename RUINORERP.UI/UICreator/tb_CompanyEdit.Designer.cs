// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/07/2025 21:48:19
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
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
this.txtAddress.Multiline = true;

this.lblENAddress = new Krypton.Toolkit.KryptonLabel();
this.txtENAddress = new Krypton.Toolkit.KryptonTextBox();
this.txtENAddress.Multiline = true;

this.lblWebsite = new Krypton.Toolkit.KryptonLabel();
this.txtWebsite = new Krypton.Toolkit.KryptonTextBox();
this.txtWebsite.Multiline = true;

this.lblEmail = new Krypton.Toolkit.KryptonLabel();
this.txtEmail = new Krypton.Toolkit.KryptonTextBox();

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
this.txtNotes.Multiline = true;

    
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

           //#####255Address###String
this.lblAddress.AutoSize = true;
this.lblAddress.Location = new System.Drawing.Point(100,225);
this.lblAddress.Name = "lblAddress";
this.lblAddress.Size = new System.Drawing.Size(41, 12);
this.lblAddress.TabIndex = 9;
this.lblAddress.Text = "地址";
this.txtAddress.Location = new System.Drawing.Point(173,221);
this.txtAddress.Name = "txtAddress";
this.txtAddress.Size = new System.Drawing.Size(100, 21);
this.txtAddress.TabIndex = 9;
this.Controls.Add(this.lblAddress);
this.Controls.Add(this.txtAddress);

           //#####255ENAddress###String
this.lblENAddress.AutoSize = true;
this.lblENAddress.Location = new System.Drawing.Point(100,250);
this.lblENAddress.Name = "lblENAddress";
this.lblENAddress.Size = new System.Drawing.Size(41, 12);
this.lblENAddress.TabIndex = 10;
this.lblENAddress.Text = "英文地址";
this.txtENAddress.Location = new System.Drawing.Point(173,246);
this.txtENAddress.Name = "txtENAddress";
this.txtENAddress.Size = new System.Drawing.Size(100, 21);
this.txtENAddress.TabIndex = 10;
this.Controls.Add(this.lblENAddress);
this.Controls.Add(this.txtENAddress);

           //#####255Website###String
this.lblWebsite.AutoSize = true;
this.lblWebsite.Location = new System.Drawing.Point(100,275);
this.lblWebsite.Name = "lblWebsite";
this.lblWebsite.Size = new System.Drawing.Size(41, 12);
this.lblWebsite.TabIndex = 11;
this.lblWebsite.Text = "网址";
this.txtWebsite.Location = new System.Drawing.Point(173,271);
this.txtWebsite.Name = "txtWebsite";
this.txtWebsite.Size = new System.Drawing.Size(100, 21);
this.txtWebsite.TabIndex = 11;
this.Controls.Add(this.lblWebsite);
this.Controls.Add(this.txtWebsite);

           //#####100Email###String
this.lblEmail.AutoSize = true;
this.lblEmail.Location = new System.Drawing.Point(100,300);
this.lblEmail.Name = "lblEmail";
this.lblEmail.Size = new System.Drawing.Size(41, 12);
this.lblEmail.TabIndex = 12;
this.lblEmail.Text = "电子邮件";
this.txtEmail.Location = new System.Drawing.Point(173,296);
this.txtEmail.Name = "txtEmail";
this.txtEmail.Size = new System.Drawing.Size(100, 21);
this.txtEmail.TabIndex = 12;
this.Controls.Add(this.lblEmail);
this.Controls.Add(this.txtEmail);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,325);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 13;
this.lblCreated_at.Text = "创建时间";
//111======325
this.dtpCreated_at.Location = new System.Drawing.Point(173,321);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 13;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,350);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 14;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,346);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 14;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,375);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 15;
this.lblModified_at.Text = "修改时间";
//111======375
this.dtpModified_at.Location = new System.Drawing.Point(173,371);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 15;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,400);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 16;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,396);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 16;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,425);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 17;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,421);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 17;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

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
           // this.kryptonPanel1.TabIndex = 17;

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

                this.Controls.Add(this.lblAddress );
this.Controls.Add(this.txtAddress );

                this.Controls.Add(this.lblENAddress );
this.Controls.Add(this.txtENAddress );

                this.Controls.Add(this.lblWebsite );
this.Controls.Add(this.txtWebsite );

                this.Controls.Add(this.lblEmail );
this.Controls.Add(this.txtEmail );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_CompanyEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CompanyEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
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

