// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:22
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 系统注册信息
    /// </summary>
    partial class tb_sys_RegistrationInfoEdit
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
     this.lblCompanyName = new Krypton.Toolkit.KryptonLabel();
this.txtCompanyName = new Krypton.Toolkit.KryptonTextBox();

this.lblFunctionModule = new Krypton.Toolkit.KryptonLabel();
this.txtFunctionModule = new Krypton.Toolkit.KryptonTextBox();
this.txtFunctionModule.Multiline = true;

this.lblContactName = new Krypton.Toolkit.KryptonLabel();
this.txtContactName = new Krypton.Toolkit.KryptonTextBox();

this.lblPhoneNumber = new Krypton.Toolkit.KryptonLabel();
this.txtPhoneNumber = new Krypton.Toolkit.KryptonTextBox();

this.lblMachineCode = new Krypton.Toolkit.KryptonLabel();
this.txtMachineCode = new Krypton.Toolkit.KryptonTextBox();
this.txtMachineCode.Multiline = true;

this.lblRegistrationCode = new Krypton.Toolkit.KryptonLabel();
this.txtRegistrationCode = new Krypton.Toolkit.KryptonTextBox();
this.txtRegistrationCode.Multiline = true;

this.lblConcurrentUsers = new Krypton.Toolkit.KryptonLabel();
this.txtConcurrentUsers = new Krypton.Toolkit.KryptonTextBox();

this.lblExpirationDate = new Krypton.Toolkit.KryptonLabel();
this.dtpExpirationDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblProductVersion = new Krypton.Toolkit.KryptonLabel();
this.txtProductVersion = new Krypton.Toolkit.KryptonTextBox();

this.lblLicenseType = new Krypton.Toolkit.KryptonLabel();
this.txtLicenseType = new Krypton.Toolkit.KryptonTextBox();

this.lblPurchaseDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPurchaseDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblRegistrationDate = new Krypton.Toolkit.KryptonLabel();
this.dtpRegistrationDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblIsRegistered = new Krypton.Toolkit.KryptonLabel();
this.chkIsRegistered = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsRegistered.Values.Text ="";

this.lblRemarks = new Krypton.Toolkit.KryptonLabel();
this.txtRemarks = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####200CompanyName###String
this.lblCompanyName.AutoSize = true;
this.lblCompanyName.Location = new System.Drawing.Point(100,25);
this.lblCompanyName.Name = "lblCompanyName";
this.lblCompanyName.Size = new System.Drawing.Size(41, 12);
this.lblCompanyName.TabIndex = 1;
this.lblCompanyName.Text = "注册公司名";
this.txtCompanyName.Location = new System.Drawing.Point(173,21);
this.txtCompanyName.Name = "txtCompanyName";
this.txtCompanyName.Size = new System.Drawing.Size(100, 21);
this.txtCompanyName.TabIndex = 1;
this.Controls.Add(this.lblCompanyName);
this.Controls.Add(this.txtCompanyName);

           //#####3000FunctionModule###String
this.lblFunctionModule.AutoSize = true;
this.lblFunctionModule.Location = new System.Drawing.Point(100,50);
this.lblFunctionModule.Name = "lblFunctionModule";
this.lblFunctionModule.Size = new System.Drawing.Size(41, 12);
this.lblFunctionModule.TabIndex = 2;
this.lblFunctionModule.Text = "功能模块";
this.txtFunctionModule.Location = new System.Drawing.Point(173,46);
this.txtFunctionModule.Name = "txtFunctionModule";
this.txtFunctionModule.Size = new System.Drawing.Size(100, 21);
this.txtFunctionModule.TabIndex = 2;
this.Controls.Add(this.lblFunctionModule);
this.Controls.Add(this.txtFunctionModule);

           //#####200ContactName###String
this.lblContactName.AutoSize = true;
this.lblContactName.Location = new System.Drawing.Point(100,75);
this.lblContactName.Name = "lblContactName";
this.lblContactName.Size = new System.Drawing.Size(41, 12);
this.lblContactName.TabIndex = 3;
this.lblContactName.Text = "联系人姓名";
this.txtContactName.Location = new System.Drawing.Point(173,71);
this.txtContactName.Name = "txtContactName";
this.txtContactName.Size = new System.Drawing.Size(100, 21);
this.txtContactName.TabIndex = 3;
this.Controls.Add(this.lblContactName);
this.Controls.Add(this.txtContactName);

           //#####200PhoneNumber###String
this.lblPhoneNumber.AutoSize = true;
this.lblPhoneNumber.Location = new System.Drawing.Point(100,100);
this.lblPhoneNumber.Name = "lblPhoneNumber";
this.lblPhoneNumber.Size = new System.Drawing.Size(41, 12);
this.lblPhoneNumber.TabIndex = 4;
this.lblPhoneNumber.Text = "手机号";
this.txtPhoneNumber.Location = new System.Drawing.Point(173,96);
this.txtPhoneNumber.Name = "txtPhoneNumber";
this.txtPhoneNumber.Size = new System.Drawing.Size(100, 21);
this.txtPhoneNumber.TabIndex = 4;
this.Controls.Add(this.lblPhoneNumber);
this.Controls.Add(this.txtPhoneNumber);

           //#####3000MachineCode###String
this.lblMachineCode.AutoSize = true;
this.lblMachineCode.Location = new System.Drawing.Point(100,125);
this.lblMachineCode.Name = "lblMachineCode";
this.lblMachineCode.Size = new System.Drawing.Size(41, 12);
this.lblMachineCode.TabIndex = 5;
this.lblMachineCode.Text = "机器码";
this.txtMachineCode.Location = new System.Drawing.Point(173,121);
this.txtMachineCode.Name = "txtMachineCode";
this.txtMachineCode.Size = new System.Drawing.Size(100, 21);
this.txtMachineCode.TabIndex = 5;
this.Controls.Add(this.lblMachineCode);
this.Controls.Add(this.txtMachineCode);

           //#####3000RegistrationCode###String
this.lblRegistrationCode.AutoSize = true;
this.lblRegistrationCode.Location = new System.Drawing.Point(100,150);
this.lblRegistrationCode.Name = "lblRegistrationCode";
this.lblRegistrationCode.Size = new System.Drawing.Size(41, 12);
this.lblRegistrationCode.TabIndex = 6;
this.lblRegistrationCode.Text = "注册码";
this.txtRegistrationCode.Location = new System.Drawing.Point(173,146);
this.txtRegistrationCode.Name = "txtRegistrationCode";
this.txtRegistrationCode.Size = new System.Drawing.Size(100, 21);
this.txtRegistrationCode.TabIndex = 6;
this.Controls.Add(this.lblRegistrationCode);
this.Controls.Add(this.txtRegistrationCode);

           //#####ConcurrentUsers###Int32
this.lblConcurrentUsers.AutoSize = true;
this.lblConcurrentUsers.Location = new System.Drawing.Point(100,175);
this.lblConcurrentUsers.Name = "lblConcurrentUsers";
this.lblConcurrentUsers.Size = new System.Drawing.Size(41, 12);
this.lblConcurrentUsers.TabIndex = 7;
this.lblConcurrentUsers.Text = "同时在线用户数";
this.txtConcurrentUsers.Location = new System.Drawing.Point(173,171);
this.txtConcurrentUsers.Name = "txtConcurrentUsers";
this.txtConcurrentUsers.Size = new System.Drawing.Size(100, 21);
this.txtConcurrentUsers.TabIndex = 7;
this.Controls.Add(this.lblConcurrentUsers);
this.Controls.Add(this.txtConcurrentUsers);

           //#####ExpirationDate###DateTime
this.lblExpirationDate.AutoSize = true;
this.lblExpirationDate.Location = new System.Drawing.Point(100,200);
this.lblExpirationDate.Name = "lblExpirationDate";
this.lblExpirationDate.Size = new System.Drawing.Size(41, 12);
this.lblExpirationDate.TabIndex = 8;
this.lblExpirationDate.Text = "截止时间";
//111======200
this.dtpExpirationDate.Location = new System.Drawing.Point(173,196);
this.dtpExpirationDate.Name ="dtpExpirationDate";
this.dtpExpirationDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpirationDate.TabIndex = 8;
this.Controls.Add(this.lblExpirationDate);
this.Controls.Add(this.dtpExpirationDate);

           //#####200ProductVersion###String
this.lblProductVersion.AutoSize = true;
this.lblProductVersion.Location = new System.Drawing.Point(100,225);
this.lblProductVersion.Name = "lblProductVersion";
this.lblProductVersion.Size = new System.Drawing.Size(41, 12);
this.lblProductVersion.TabIndex = 9;
this.lblProductVersion.Text = "版本信息";
this.txtProductVersion.Location = new System.Drawing.Point(173,221);
this.txtProductVersion.Name = "txtProductVersion";
this.txtProductVersion.Size = new System.Drawing.Size(100, 21);
this.txtProductVersion.TabIndex = 9;
this.Controls.Add(this.lblProductVersion);
this.Controls.Add(this.txtProductVersion);

           //#####20LicenseType###String
this.lblLicenseType.AutoSize = true;
this.lblLicenseType.Location = new System.Drawing.Point(100,250);
this.lblLicenseType.Name = "lblLicenseType";
this.lblLicenseType.Size = new System.Drawing.Size(41, 12);
this.lblLicenseType.TabIndex = 10;
this.lblLicenseType.Text = "授权类型";
this.txtLicenseType.Location = new System.Drawing.Point(173,246);
this.txtLicenseType.Name = "txtLicenseType";
this.txtLicenseType.Size = new System.Drawing.Size(100, 21);
this.txtLicenseType.TabIndex = 10;
this.Controls.Add(this.lblLicenseType);
this.Controls.Add(this.txtLicenseType);

           //#####PurchaseDate###DateTime
this.lblPurchaseDate.AutoSize = true;
this.lblPurchaseDate.Location = new System.Drawing.Point(100,275);
this.lblPurchaseDate.Name = "lblPurchaseDate";
this.lblPurchaseDate.Size = new System.Drawing.Size(41, 12);
this.lblPurchaseDate.TabIndex = 11;
this.lblPurchaseDate.Text = "购买日期";
//111======275
this.dtpPurchaseDate.Location = new System.Drawing.Point(173,271);
this.dtpPurchaseDate.Name ="dtpPurchaseDate";
this.dtpPurchaseDate.Size = new System.Drawing.Size(100, 21);
this.dtpPurchaseDate.TabIndex = 11;
this.Controls.Add(this.lblPurchaseDate);
this.Controls.Add(this.dtpPurchaseDate);

           //#####RegistrationDate###DateTime
this.lblRegistrationDate.AutoSize = true;
this.lblRegistrationDate.Location = new System.Drawing.Point(100,300);
this.lblRegistrationDate.Name = "lblRegistrationDate";
this.lblRegistrationDate.Size = new System.Drawing.Size(41, 12);
this.lblRegistrationDate.TabIndex = 12;
this.lblRegistrationDate.Text = "注册时间";
//111======300
this.dtpRegistrationDate.Location = new System.Drawing.Point(173,296);
this.dtpRegistrationDate.Name ="dtpRegistrationDate";
this.dtpRegistrationDate.Size = new System.Drawing.Size(100, 21);
this.dtpRegistrationDate.TabIndex = 12;
this.Controls.Add(this.lblRegistrationDate);
this.Controls.Add(this.dtpRegistrationDate);

           //#####IsRegistered###Boolean
this.lblIsRegistered.AutoSize = true;
this.lblIsRegistered.Location = new System.Drawing.Point(100,325);
this.lblIsRegistered.Name = "lblIsRegistered";
this.lblIsRegistered.Size = new System.Drawing.Size(41, 12);
this.lblIsRegistered.TabIndex = 13;
this.lblIsRegistered.Text = "已注册";
this.chkIsRegistered.Location = new System.Drawing.Point(173,321);
this.chkIsRegistered.Name = "chkIsRegistered";
this.chkIsRegistered.Size = new System.Drawing.Size(100, 21);
this.chkIsRegistered.TabIndex = 13;
this.Controls.Add(this.lblIsRegistered);
this.Controls.Add(this.chkIsRegistered);

           //#####200Remarks###String
this.lblRemarks.AutoSize = true;
this.lblRemarks.Location = new System.Drawing.Point(100,350);
this.lblRemarks.Name = "lblRemarks";
this.lblRemarks.Size = new System.Drawing.Size(41, 12);
this.lblRemarks.TabIndex = 14;
this.lblRemarks.Text = "备注";
this.txtRemarks.Location = new System.Drawing.Point(173,346);
this.txtRemarks.Name = "txtRemarks";
this.txtRemarks.Size = new System.Drawing.Size(100, 21);
this.txtRemarks.TabIndex = 14;
this.Controls.Add(this.lblRemarks);
this.Controls.Add(this.txtRemarks);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,375);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 15;
this.lblCreated_at.Text = "创建时间";
//111======375
this.dtpCreated_at.Location = new System.Drawing.Point(173,371);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 15;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,400);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 16;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,396);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 16;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,425);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 17;
this.lblModified_at.Text = "修改时间";
//111======425
this.dtpModified_at.Location = new System.Drawing.Point(173,421);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 17;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,450);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 18;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,446);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 18;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
           // this.kryptonPanel1.TabIndex = 18;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCompanyName );
this.Controls.Add(this.txtCompanyName );

                this.Controls.Add(this.lblFunctionModule );
this.Controls.Add(this.txtFunctionModule );

                this.Controls.Add(this.lblContactName );
this.Controls.Add(this.txtContactName );

                this.Controls.Add(this.lblPhoneNumber );
this.Controls.Add(this.txtPhoneNumber );

                this.Controls.Add(this.lblMachineCode );
this.Controls.Add(this.txtMachineCode );

                this.Controls.Add(this.lblRegistrationCode );
this.Controls.Add(this.txtRegistrationCode );

                this.Controls.Add(this.lblConcurrentUsers );
this.Controls.Add(this.txtConcurrentUsers );

                this.Controls.Add(this.lblExpirationDate );
this.Controls.Add(this.dtpExpirationDate );

                this.Controls.Add(this.lblProductVersion );
this.Controls.Add(this.txtProductVersion );

                this.Controls.Add(this.lblLicenseType );
this.Controls.Add(this.txtLicenseType );

                this.Controls.Add(this.lblPurchaseDate );
this.Controls.Add(this.dtpPurchaseDate );

                this.Controls.Add(this.lblRegistrationDate );
this.Controls.Add(this.dtpRegistrationDate );

                this.Controls.Add(this.lblIsRegistered );
this.Controls.Add(this.chkIsRegistered );

                this.Controls.Add(this.lblRemarks );
this.Controls.Add(this.txtRemarks );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_sys_RegistrationInfoEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_sys_RegistrationInfoEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCompanyName;
private Krypton.Toolkit.KryptonTextBox txtCompanyName;

    
        
              private Krypton.Toolkit.KryptonLabel lblFunctionModule;
private Krypton.Toolkit.KryptonTextBox txtFunctionModule;

    
        
              private Krypton.Toolkit.KryptonLabel lblContactName;
private Krypton.Toolkit.KryptonTextBox txtContactName;

    
        
              private Krypton.Toolkit.KryptonLabel lblPhoneNumber;
private Krypton.Toolkit.KryptonTextBox txtPhoneNumber;

    
        
              private Krypton.Toolkit.KryptonLabel lblMachineCode;
private Krypton.Toolkit.KryptonTextBox txtMachineCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblRegistrationCode;
private Krypton.Toolkit.KryptonTextBox txtRegistrationCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblConcurrentUsers;
private Krypton.Toolkit.KryptonTextBox txtConcurrentUsers;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpirationDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpExpirationDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductVersion;
private Krypton.Toolkit.KryptonTextBox txtProductVersion;

    
        
              private Krypton.Toolkit.KryptonLabel lblLicenseType;
private Krypton.Toolkit.KryptonTextBox txtLicenseType;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurchaseDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPurchaseDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblRegistrationDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpRegistrationDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsRegistered;
private Krypton.Toolkit.KryptonCheckBox chkIsRegistered;

    
        
              private Krypton.Toolkit.KryptonLabel lblRemarks;
private Krypton.Toolkit.KryptonTextBox txtRemarks;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

