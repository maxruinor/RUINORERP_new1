
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/28/2024 18:55:32
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
    partial class tb_sys_RegistrationInfoQuery
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
     
     this.lblCompanyName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCompanyName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContactName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContactName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPhoneNumber = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPhoneNumber = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMachineCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMachineCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtMachineCode.Multiline = true;

this.lblRegistrationCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRegistrationCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblExpirationDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpExpirationDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblProductVersion = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductVersion = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblPurchaseDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPurchaseDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblRegistrationDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpRegistrationDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblIsRegistered = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsRegistered = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsRegistered.Values.Text ="";

this.lblRemarks = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemarks = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
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

           //#####200ContactName###String
this.lblContactName.AutoSize = true;
this.lblContactName.Location = new System.Drawing.Point(100,50);
this.lblContactName.Name = "lblContactName";
this.lblContactName.Size = new System.Drawing.Size(41, 12);
this.lblContactName.TabIndex = 2;
this.lblContactName.Text = "联系人姓名";
this.txtContactName.Location = new System.Drawing.Point(173,46);
this.txtContactName.Name = "txtContactName";
this.txtContactName.Size = new System.Drawing.Size(100, 21);
this.txtContactName.TabIndex = 2;
this.Controls.Add(this.lblContactName);
this.Controls.Add(this.txtContactName);

           //#####200PhoneNumber###String
this.lblPhoneNumber.AutoSize = true;
this.lblPhoneNumber.Location = new System.Drawing.Point(100,75);
this.lblPhoneNumber.Name = "lblPhoneNumber";
this.lblPhoneNumber.Size = new System.Drawing.Size(41, 12);
this.lblPhoneNumber.TabIndex = 3;
this.lblPhoneNumber.Text = "手机号";
this.txtPhoneNumber.Location = new System.Drawing.Point(173,71);
this.txtPhoneNumber.Name = "txtPhoneNumber";
this.txtPhoneNumber.Size = new System.Drawing.Size(100, 21);
this.txtPhoneNumber.TabIndex = 3;
this.Controls.Add(this.lblPhoneNumber);
this.Controls.Add(this.txtPhoneNumber);

           //#####1000MachineCode###String
this.lblMachineCode.AutoSize = true;
this.lblMachineCode.Location = new System.Drawing.Point(100,100);
this.lblMachineCode.Name = "lblMachineCode";
this.lblMachineCode.Size = new System.Drawing.Size(41, 12);
this.lblMachineCode.TabIndex = 4;
this.lblMachineCode.Text = "机器码";
this.txtMachineCode.Location = new System.Drawing.Point(173,96);
this.txtMachineCode.Name = "txtMachineCode";
this.txtMachineCode.Size = new System.Drawing.Size(100, 21);
this.txtMachineCode.TabIndex = 4;
this.Controls.Add(this.lblMachineCode);
this.Controls.Add(this.txtMachineCode);

           //#####100RegistrationCode###String
this.lblRegistrationCode.AutoSize = true;
this.lblRegistrationCode.Location = new System.Drawing.Point(100,125);
this.lblRegistrationCode.Name = "lblRegistrationCode";
this.lblRegistrationCode.Size = new System.Drawing.Size(41, 12);
this.lblRegistrationCode.TabIndex = 5;
this.lblRegistrationCode.Text = "注册码";
this.txtRegistrationCode.Location = new System.Drawing.Point(173,121);
this.txtRegistrationCode.Name = "txtRegistrationCode";
this.txtRegistrationCode.Size = new System.Drawing.Size(100, 21);
this.txtRegistrationCode.TabIndex = 5;
this.Controls.Add(this.lblRegistrationCode);
this.Controls.Add(this.txtRegistrationCode);

           //#####ConcurrentUsers###Int32

           //#####ExpirationDate###DateTime
this.lblExpirationDate.AutoSize = true;
this.lblExpirationDate.Location = new System.Drawing.Point(100,175);
this.lblExpirationDate.Name = "lblExpirationDate";
this.lblExpirationDate.Size = new System.Drawing.Size(41, 12);
this.lblExpirationDate.TabIndex = 7;
this.lblExpirationDate.Text = "截止时间";
//111======175
this.dtpExpirationDate.Location = new System.Drawing.Point(173,171);
this.dtpExpirationDate.Name ="dtpExpirationDate";
this.dtpExpirationDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpirationDate.TabIndex = 7;
this.Controls.Add(this.lblExpirationDate);
this.Controls.Add(this.dtpExpirationDate);

           //#####200ProductVersion###String
this.lblProductVersion.AutoSize = true;
this.lblProductVersion.Location = new System.Drawing.Point(100,200);
this.lblProductVersion.Name = "lblProductVersion";
this.lblProductVersion.Size = new System.Drawing.Size(41, 12);
this.lblProductVersion.TabIndex = 8;
this.lblProductVersion.Text = "版本信息";
this.txtProductVersion.Location = new System.Drawing.Point(173,196);
this.txtProductVersion.Name = "txtProductVersion";
this.txtProductVersion.Size = new System.Drawing.Size(100, 21);
this.txtProductVersion.TabIndex = 8;
this.Controls.Add(this.lblProductVersion);
this.Controls.Add(this.txtProductVersion);

           //#####LicenseType###Int32

           //#####PurchaseDate###DateTime
this.lblPurchaseDate.AutoSize = true;
this.lblPurchaseDate.Location = new System.Drawing.Point(100,250);
this.lblPurchaseDate.Name = "lblPurchaseDate";
this.lblPurchaseDate.Size = new System.Drawing.Size(41, 12);
this.lblPurchaseDate.TabIndex = 10;
this.lblPurchaseDate.Text = "购买日期";
//111======250
this.dtpPurchaseDate.Location = new System.Drawing.Point(173,246);
this.dtpPurchaseDate.Name ="dtpPurchaseDate";
this.dtpPurchaseDate.Size = new System.Drawing.Size(100, 21);
this.dtpPurchaseDate.TabIndex = 10;
this.Controls.Add(this.lblPurchaseDate);
this.Controls.Add(this.dtpPurchaseDate);

           //#####RegistrationDate###DateTime
this.lblRegistrationDate.AutoSize = true;
this.lblRegistrationDate.Location = new System.Drawing.Point(100,275);
this.lblRegistrationDate.Name = "lblRegistrationDate";
this.lblRegistrationDate.Size = new System.Drawing.Size(41, 12);
this.lblRegistrationDate.TabIndex = 11;
this.lblRegistrationDate.Text = "注册时间";
//111======275
this.dtpRegistrationDate.Location = new System.Drawing.Point(173,271);
this.dtpRegistrationDate.Name ="dtpRegistrationDate";
this.dtpRegistrationDate.Size = new System.Drawing.Size(100, 21);
this.dtpRegistrationDate.TabIndex = 11;
this.Controls.Add(this.lblRegistrationDate);
this.Controls.Add(this.dtpRegistrationDate);

           //#####IsRegistered###Boolean
this.lblIsRegistered.AutoSize = true;
this.lblIsRegistered.Location = new System.Drawing.Point(100,300);
this.lblIsRegistered.Name = "lblIsRegistered";
this.lblIsRegistered.Size = new System.Drawing.Size(41, 12);
this.lblIsRegistered.TabIndex = 12;
this.lblIsRegistered.Text = "已注册";
this.chkIsRegistered.Location = new System.Drawing.Point(173,296);
this.chkIsRegistered.Name = "chkIsRegistered";
this.chkIsRegistered.Size = new System.Drawing.Size(100, 21);
this.chkIsRegistered.TabIndex = 12;
this.Controls.Add(this.lblIsRegistered);
this.Controls.Add(this.chkIsRegistered);

           //#####200Remarks###String
this.lblRemarks.AutoSize = true;
this.lblRemarks.Location = new System.Drawing.Point(100,325);
this.lblRemarks.Name = "lblRemarks";
this.lblRemarks.Size = new System.Drawing.Size(41, 12);
this.lblRemarks.TabIndex = 13;
this.lblRemarks.Text = "备注";
this.txtRemarks.Location = new System.Drawing.Point(173,321);
this.txtRemarks.Name = "txtRemarks";
this.txtRemarks.Size = new System.Drawing.Size(100, 21);
this.txtRemarks.TabIndex = 13;
this.Controls.Add(this.lblRemarks);
this.Controls.Add(this.txtRemarks);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,350);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 14;
this.lblCreated_at.Text = "创建时间";
//111======350
this.dtpCreated_at.Location = new System.Drawing.Point(173,346);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 14;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,400);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 16;
this.lblModified_at.Text = "修改时间";
//111======400
this.dtpModified_at.Location = new System.Drawing.Point(173,396);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 16;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCompanyName );
this.Controls.Add(this.txtCompanyName );

                this.Controls.Add(this.lblContactName );
this.Controls.Add(this.txtContactName );

                this.Controls.Add(this.lblPhoneNumber );
this.Controls.Add(this.txtPhoneNumber );

                this.Controls.Add(this.lblMachineCode );
this.Controls.Add(this.txtMachineCode );

                this.Controls.Add(this.lblRegistrationCode );
this.Controls.Add(this.txtRegistrationCode );

                
                this.Controls.Add(this.lblExpirationDate );
this.Controls.Add(this.dtpExpirationDate );

                this.Controls.Add(this.lblProductVersion );
this.Controls.Add(this.txtProductVersion );

                
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

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_sys_RegistrationInfoQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCompanyName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCompanyName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContactName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContactName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPhoneNumber;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPhoneNumber;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMachineCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMachineCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRegistrationCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRegistrationCode;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpirationDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpExpirationDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductVersion;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductVersion;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurchaseDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPurchaseDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRegistrationDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpRegistrationDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsRegistered;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsRegistered;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemarks;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemarks;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


