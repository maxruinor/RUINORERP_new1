// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/12/2025 21:29:56
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 开票资料表
    /// </summary>
    partial class tb_BillingInformationEdit
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
     this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblTitle = new Krypton.Toolkit.KryptonLabel();
this.txtTitle = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxNumber = new Krypton.Toolkit.KryptonLabel();
this.txtTaxNumber = new Krypton.Toolkit.KryptonTextBox();

this.lblAddress = new Krypton.Toolkit.KryptonLabel();
this.txtAddress = new Krypton.Toolkit.KryptonTextBox();

this.lblPITEL = new Krypton.Toolkit.KryptonLabel();
this.txtPITEL = new Krypton.Toolkit.KryptonTextBox();

this.lblBankAccount = new Krypton.Toolkit.KryptonLabel();
this.txtBankAccount = new Krypton.Toolkit.KryptonTextBox();

this.lblBankName = new Krypton.Toolkit.KryptonLabel();
this.txtBankName = new Krypton.Toolkit.KryptonTextBox();

this.lblEmail = new Krypton.Toolkit.KryptonLabel();
this.txtEmail = new Krypton.Toolkit.KryptonTextBox();

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

this.lblIsActive = new Krypton.Toolkit.KryptonLabel();
this.chkIsActive = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsActive.Values.Text ="";
this.chkIsActive.Checked = true;
this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;

    
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
     
            //#####CustomerVendor_ID###Int64
//属性测试25CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,25);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 1;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======25
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,21);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 1;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####200Title###String
this.lblTitle.AutoSize = true;
this.lblTitle.Location = new System.Drawing.Point(100,50);
this.lblTitle.Name = "lblTitle";
this.lblTitle.Size = new System.Drawing.Size(41, 12);
this.lblTitle.TabIndex = 2;
this.lblTitle.Text = "抬头";
this.txtTitle.Location = new System.Drawing.Point(173,46);
this.txtTitle.Name = "txtTitle";
this.txtTitle.Size = new System.Drawing.Size(100, 21);
this.txtTitle.TabIndex = 2;
this.Controls.Add(this.lblTitle);
this.Controls.Add(this.txtTitle);

           //#####200TaxNumber###String
this.lblTaxNumber.AutoSize = true;
this.lblTaxNumber.Location = new System.Drawing.Point(100,75);
this.lblTaxNumber.Name = "lblTaxNumber";
this.lblTaxNumber.Size = new System.Drawing.Size(41, 12);
this.lblTaxNumber.TabIndex = 3;
this.lblTaxNumber.Text = "税号";
this.txtTaxNumber.Location = new System.Drawing.Point(173,71);
this.txtTaxNumber.Name = "txtTaxNumber";
this.txtTaxNumber.Size = new System.Drawing.Size(100, 21);
this.txtTaxNumber.TabIndex = 3;
this.Controls.Add(this.lblTaxNumber);
this.Controls.Add(this.txtTaxNumber);

           //#####200Address###String
this.lblAddress.AutoSize = true;
this.lblAddress.Location = new System.Drawing.Point(100,100);
this.lblAddress.Name = "lblAddress";
this.lblAddress.Size = new System.Drawing.Size(41, 12);
this.lblAddress.TabIndex = 4;
this.lblAddress.Text = "地址";
this.txtAddress.Location = new System.Drawing.Point(173,96);
this.txtAddress.Name = "txtAddress";
this.txtAddress.Size = new System.Drawing.Size(100, 21);
this.txtAddress.TabIndex = 4;
this.Controls.Add(this.lblAddress);
this.Controls.Add(this.txtAddress);

           //#####50PITEL###String
this.lblPITEL.AutoSize = true;
this.lblPITEL.Location = new System.Drawing.Point(100,125);
this.lblPITEL.Name = "lblPITEL";
this.lblPITEL.Size = new System.Drawing.Size(41, 12);
this.lblPITEL.TabIndex = 5;
this.lblPITEL.Text = "电话";
this.txtPITEL.Location = new System.Drawing.Point(173,121);
this.txtPITEL.Name = "txtPITEL";
this.txtPITEL.Size = new System.Drawing.Size(100, 21);
this.txtPITEL.TabIndex = 5;
this.Controls.Add(this.lblPITEL);
this.Controls.Add(this.txtPITEL);

           //#####150BankAccount###String
this.lblBankAccount.AutoSize = true;
this.lblBankAccount.Location = new System.Drawing.Point(100,150);
this.lblBankAccount.Name = "lblBankAccount";
this.lblBankAccount.Size = new System.Drawing.Size(41, 12);
this.lblBankAccount.TabIndex = 6;
this.lblBankAccount.Text = "银行账号";
this.txtBankAccount.Location = new System.Drawing.Point(173,146);
this.txtBankAccount.Name = "txtBankAccount";
this.txtBankAccount.Size = new System.Drawing.Size(100, 21);
this.txtBankAccount.TabIndex = 6;
this.Controls.Add(this.lblBankAccount);
this.Controls.Add(this.txtBankAccount);

           //#####50BankName###String
this.lblBankName.AutoSize = true;
this.lblBankName.Location = new System.Drawing.Point(100,175);
this.lblBankName.Name = "lblBankName";
this.lblBankName.Size = new System.Drawing.Size(41, 12);
this.lblBankName.TabIndex = 7;
this.lblBankName.Text = "开户行";
this.txtBankName.Location = new System.Drawing.Point(173,171);
this.txtBankName.Name = "txtBankName";
this.txtBankName.Size = new System.Drawing.Size(100, 21);
this.txtBankName.TabIndex = 7;
this.Controls.Add(this.lblBankName);
this.Controls.Add(this.txtBankName);

           //#####150Email###String
this.lblEmail.AutoSize = true;
this.lblEmail.Location = new System.Drawing.Point(100,200);
this.lblEmail.Name = "lblEmail";
this.lblEmail.Size = new System.Drawing.Size(41, 12);
this.lblEmail.TabIndex = 8;
this.lblEmail.Text = "邮箱";
this.txtEmail.Location = new System.Drawing.Point(173,196);
this.txtEmail.Name = "txtEmail";
this.txtEmail.Size = new System.Drawing.Size(100, 21);
this.txtEmail.TabIndex = 8;
this.Controls.Add(this.lblEmail);
this.Controls.Add(this.txtEmail);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,225);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 9;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,221);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 9;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,250);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 10;
this.lblCreated_at.Text = "创建时间";
//111======250
this.dtpCreated_at.Location = new System.Drawing.Point(173,246);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 10;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试275Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,275);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 11;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,271);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 11;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,300);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 12;
this.lblModified_at.Text = "修改时间";
//111======300
this.dtpModified_at.Location = new System.Drawing.Point(173,296);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 12;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试325Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,325);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 13;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,321);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 13;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,350);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 14;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,346);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 14;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####IsActive###Boolean
this.lblIsActive.AutoSize = true;
this.lblIsActive.Location = new System.Drawing.Point(100,375);
this.lblIsActive.Name = "lblIsActive";
this.lblIsActive.Size = new System.Drawing.Size(41, 12);
this.lblIsActive.TabIndex = 15;
this.lblIsActive.Text = "激活";
this.chkIsActive.Location = new System.Drawing.Point(173,371);
this.chkIsActive.Name = "chkIsActive";
this.chkIsActive.Size = new System.Drawing.Size(100, 21);
this.chkIsActive.TabIndex = 15;
this.Controls.Add(this.lblIsActive);
this.Controls.Add(this.chkIsActive);

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
           // this.kryptonPanel1.TabIndex = 15;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblTitle );
this.Controls.Add(this.txtTitle );

                this.Controls.Add(this.lblTaxNumber );
this.Controls.Add(this.txtTaxNumber );

                this.Controls.Add(this.lblAddress );
this.Controls.Add(this.txtAddress );

                this.Controls.Add(this.lblPITEL );
this.Controls.Add(this.txtPITEL );

                this.Controls.Add(this.lblBankAccount );
this.Controls.Add(this.txtBankAccount );

                this.Controls.Add(this.lblBankName );
this.Controls.Add(this.txtBankName );

                this.Controls.Add(this.lblEmail );
this.Controls.Add(this.txtEmail );

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

                this.Controls.Add(this.lblIsActive );
this.Controls.Add(this.chkIsActive );

                            // 
            // "tb_BillingInformationEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_BillingInformationEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTitle;
private Krypton.Toolkit.KryptonTextBox txtTitle;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxNumber;
private Krypton.Toolkit.KryptonTextBox txtTaxNumber;

    
        
              private Krypton.Toolkit.KryptonLabel lblAddress;
private Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblPITEL;
private Krypton.Toolkit.KryptonTextBox txtPITEL;

    
        
              private Krypton.Toolkit.KryptonLabel lblBankAccount;
private Krypton.Toolkit.KryptonTextBox txtBankAccount;

    
        
              private Krypton.Toolkit.KryptonLabel lblBankName;
private Krypton.Toolkit.KryptonTextBox txtBankName;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmail;
private Krypton.Toolkit.KryptonTextBox txtEmail;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblIsActive;
private Krypton.Toolkit.KryptonCheckBox chkIsActive;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

