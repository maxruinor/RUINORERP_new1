// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:45
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 网店信息表
    /// </summary>
    partial class tb_OnlineStoreInfoEdit
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
     this.lblStoreCode = new Krypton.Toolkit.KryptonLabel();
this.txtStoreCode = new Krypton.Toolkit.KryptonTextBox();

this.lblStoreName = new Krypton.Toolkit.KryptonLabel();
this.txtStoreName = new Krypton.Toolkit.KryptonTextBox();

this.lblPlatformName = new Krypton.Toolkit.KryptonLabel();
this.txtPlatformName = new Krypton.Toolkit.KryptonTextBox();

this.lblContact = new Krypton.Toolkit.KryptonLabel();
this.txtContact = new Krypton.Toolkit.KryptonTextBox();

this.lblPhone = new Krypton.Toolkit.KryptonLabel();
this.txtPhone = new Krypton.Toolkit.KryptonTextBox();
this.txtPhone.Multiline = true;

this.lblAddress = new Krypton.Toolkit.KryptonLabel();
this.txtAddress = new Krypton.Toolkit.KryptonTextBox();
this.txtAddress.Multiline = true;

this.lblWebsite = new Krypton.Toolkit.KryptonLabel();
this.txtWebsite = new Krypton.Toolkit.KryptonTextBox();
this.txtWebsite.Multiline = true;

this.lblResponsiblePerson = new Krypton.Toolkit.KryptonLabel();
this.txtResponsiblePerson = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####50StoreCode###String
this.lblStoreCode.AutoSize = true;
this.lblStoreCode.Location = new System.Drawing.Point(100,25);
this.lblStoreCode.Name = "lblStoreCode";
this.lblStoreCode.Size = new System.Drawing.Size(41, 12);
this.lblStoreCode.TabIndex = 1;
this.lblStoreCode.Text = "项目代码";
this.txtStoreCode.Location = new System.Drawing.Point(173,21);
this.txtStoreCode.Name = "txtStoreCode";
this.txtStoreCode.Size = new System.Drawing.Size(100, 21);
this.txtStoreCode.TabIndex = 1;
this.Controls.Add(this.lblStoreCode);
this.Controls.Add(this.txtStoreCode);

           //#####50StoreName###String
this.lblStoreName.AutoSize = true;
this.lblStoreName.Location = new System.Drawing.Point(100,50);
this.lblStoreName.Name = "lblStoreName";
this.lblStoreName.Size = new System.Drawing.Size(41, 12);
this.lblStoreName.TabIndex = 2;
this.lblStoreName.Text = "项目名称";
this.txtStoreName.Location = new System.Drawing.Point(173,46);
this.txtStoreName.Name = "txtStoreName";
this.txtStoreName.Size = new System.Drawing.Size(100, 21);
this.txtStoreName.TabIndex = 2;
this.Controls.Add(this.lblStoreName);
this.Controls.Add(this.txtStoreName);

           //#####100PlatformName###String
this.lblPlatformName.AutoSize = true;
this.lblPlatformName.Location = new System.Drawing.Point(100,75);
this.lblPlatformName.Name = "lblPlatformName";
this.lblPlatformName.Size = new System.Drawing.Size(41, 12);
this.lblPlatformName.TabIndex = 3;
this.lblPlatformName.Text = "平台名称";
this.txtPlatformName.Location = new System.Drawing.Point(173,71);
this.txtPlatformName.Name = "txtPlatformName";
this.txtPlatformName.Size = new System.Drawing.Size(100, 21);
this.txtPlatformName.TabIndex = 3;
this.Controls.Add(this.lblPlatformName);
this.Controls.Add(this.txtPlatformName);

           //#####50Contact###String
this.lblContact.AutoSize = true;
this.lblContact.Location = new System.Drawing.Point(100,100);
this.lblContact.Name = "lblContact";
this.lblContact.Size = new System.Drawing.Size(41, 12);
this.lblContact.TabIndex = 4;
this.lblContact.Text = "联系人";
this.txtContact.Location = new System.Drawing.Point(173,96);
this.txtContact.Name = "txtContact";
this.txtContact.Size = new System.Drawing.Size(100, 21);
this.txtContact.TabIndex = 4;
this.Controls.Add(this.lblContact);
this.Controls.Add(this.txtContact);

           //#####255Phone###String
this.lblPhone.AutoSize = true;
this.lblPhone.Location = new System.Drawing.Point(100,125);
this.lblPhone.Name = "lblPhone";
this.lblPhone.Size = new System.Drawing.Size(41, 12);
this.lblPhone.TabIndex = 5;
this.lblPhone.Text = "电话";
this.txtPhone.Location = new System.Drawing.Point(173,121);
this.txtPhone.Name = "txtPhone";
this.txtPhone.Size = new System.Drawing.Size(100, 21);
this.txtPhone.TabIndex = 5;
this.Controls.Add(this.lblPhone);
this.Controls.Add(this.txtPhone);

           //#####255Address###String
this.lblAddress.AutoSize = true;
this.lblAddress.Location = new System.Drawing.Point(100,150);
this.lblAddress.Name = "lblAddress";
this.lblAddress.Size = new System.Drawing.Size(41, 12);
this.lblAddress.TabIndex = 6;
this.lblAddress.Text = "地址";
this.txtAddress.Location = new System.Drawing.Point(173,146);
this.txtAddress.Name = "txtAddress";
this.txtAddress.Size = new System.Drawing.Size(100, 21);
this.txtAddress.TabIndex = 6;
this.Controls.Add(this.lblAddress);
this.Controls.Add(this.txtAddress);

           //#####255Website###String
this.lblWebsite.AutoSize = true;
this.lblWebsite.Location = new System.Drawing.Point(100,175);
this.lblWebsite.Name = "lblWebsite";
this.lblWebsite.Size = new System.Drawing.Size(41, 12);
this.lblWebsite.TabIndex = 7;
this.lblWebsite.Text = "网址";
this.txtWebsite.Location = new System.Drawing.Point(173,171);
this.txtWebsite.Name = "txtWebsite";
this.txtWebsite.Size = new System.Drawing.Size(100, 21);
this.txtWebsite.TabIndex = 7;
this.Controls.Add(this.lblWebsite);
this.Controls.Add(this.txtWebsite);

           //#####50ResponsiblePerson###String
this.lblResponsiblePerson.AutoSize = true;
this.lblResponsiblePerson.Location = new System.Drawing.Point(100,200);
this.lblResponsiblePerson.Name = "lblResponsiblePerson";
this.lblResponsiblePerson.Size = new System.Drawing.Size(41, 12);
this.lblResponsiblePerson.TabIndex = 8;
this.lblResponsiblePerson.Text = "负责人";
this.txtResponsiblePerson.Location = new System.Drawing.Point(173,196);
this.txtResponsiblePerson.Name = "txtResponsiblePerson";
this.txtResponsiblePerson.Size = new System.Drawing.Size(100, 21);
this.txtResponsiblePerson.TabIndex = 8;
this.Controls.Add(this.lblResponsiblePerson);
this.Controls.Add(this.txtResponsiblePerson);

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
           // this.kryptonPanel1.TabIndex = 13;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblStoreCode );
this.Controls.Add(this.txtStoreCode );

                this.Controls.Add(this.lblStoreName );
this.Controls.Add(this.txtStoreName );

                this.Controls.Add(this.lblPlatformName );
this.Controls.Add(this.txtPlatformName );

                this.Controls.Add(this.lblContact );
this.Controls.Add(this.txtContact );

                this.Controls.Add(this.lblPhone );
this.Controls.Add(this.txtPhone );

                this.Controls.Add(this.lblAddress );
this.Controls.Add(this.txtAddress );

                this.Controls.Add(this.lblWebsite );
this.Controls.Add(this.txtWebsite );

                this.Controls.Add(this.lblResponsiblePerson );
this.Controls.Add(this.txtResponsiblePerson );

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

                            // 
            // "tb_OnlineStoreInfoEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_OnlineStoreInfoEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblStoreCode;
private Krypton.Toolkit.KryptonTextBox txtStoreCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblStoreName;
private Krypton.Toolkit.KryptonTextBox txtStoreName;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlatformName;
private Krypton.Toolkit.KryptonTextBox txtPlatformName;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact;
private Krypton.Toolkit.KryptonTextBox txtContact;

    
        
              private Krypton.Toolkit.KryptonLabel lblPhone;
private Krypton.Toolkit.KryptonTextBox txtPhone;

    
        
              private Krypton.Toolkit.KryptonLabel lblAddress;
private Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblWebsite;
private Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              private Krypton.Toolkit.KryptonLabel lblResponsiblePerson;
private Krypton.Toolkit.KryptonTextBox txtResponsiblePerson;

    
        
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

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

