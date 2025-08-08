// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:17
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 联系人表-爱好跟进
    /// </summary>
    partial class tb_CRM_ContactEdit
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
     this.lblCustomer_id = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomer_id = new Krypton.Toolkit.KryptonComboBox();

this.lblSocialTools = new Krypton.Toolkit.KryptonLabel();
this.txtSocialTools = new Krypton.Toolkit.KryptonTextBox();

this.lblContact_Name = new Krypton.Toolkit.KryptonLabel();
this.txtContact_Name = new Krypton.Toolkit.KryptonTextBox();

this.lblContact_Email = new Krypton.Toolkit.KryptonLabel();
this.txtContact_Email = new Krypton.Toolkit.KryptonTextBox();

this.lblContact_Phone = new Krypton.Toolkit.KryptonLabel();
this.txtContact_Phone = new Krypton.Toolkit.KryptonTextBox();

this.lblPosition = new Krypton.Toolkit.KryptonLabel();
this.txtPosition = new Krypton.Toolkit.KryptonTextBox();

this.lblPreferences = new Krypton.Toolkit.KryptonLabel();
this.txtPreferences = new Krypton.Toolkit.KryptonTextBox();

this.lblAddress = new Krypton.Toolkit.KryptonLabel();
this.txtAddress = new Krypton.Toolkit.KryptonTextBox();
this.txtAddress.Multiline = true;

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
     
            //#####Customer_id###Int64
//属性测试25Customer_id
this.lblCustomer_id.AutoSize = true;
this.lblCustomer_id.Location = new System.Drawing.Point(100,25);
this.lblCustomer_id.Name = "lblCustomer_id";
this.lblCustomer_id.Size = new System.Drawing.Size(41, 12);
this.lblCustomer_id.TabIndex = 1;
this.lblCustomer_id.Text = "目标客户";
//111======25
this.cmbCustomer_id.Location = new System.Drawing.Point(173,21);
this.cmbCustomer_id.Name ="cmbCustomer_id";
this.cmbCustomer_id.Size = new System.Drawing.Size(100, 21);
this.cmbCustomer_id.TabIndex = 1;
this.Controls.Add(this.lblCustomer_id);
this.Controls.Add(this.cmbCustomer_id);

           //#####200SocialTools###String
this.lblSocialTools.AutoSize = true;
this.lblSocialTools.Location = new System.Drawing.Point(100,50);
this.lblSocialTools.Name = "lblSocialTools";
this.lblSocialTools.Size = new System.Drawing.Size(41, 12);
this.lblSocialTools.TabIndex = 2;
this.lblSocialTools.Text = "社交账号";
this.txtSocialTools.Location = new System.Drawing.Point(173,46);
this.txtSocialTools.Name = "txtSocialTools";
this.txtSocialTools.Size = new System.Drawing.Size(100, 21);
this.txtSocialTools.TabIndex = 2;
this.Controls.Add(this.lblSocialTools);
this.Controls.Add(this.txtSocialTools);

           //#####50Contact_Name###String
this.lblContact_Name.AutoSize = true;
this.lblContact_Name.Location = new System.Drawing.Point(100,75);
this.lblContact_Name.Name = "lblContact_Name";
this.lblContact_Name.Size = new System.Drawing.Size(41, 12);
this.lblContact_Name.TabIndex = 3;
this.lblContact_Name.Text = "姓名";
this.txtContact_Name.Location = new System.Drawing.Point(173,71);
this.txtContact_Name.Name = "txtContact_Name";
this.txtContact_Name.Size = new System.Drawing.Size(100, 21);
this.txtContact_Name.TabIndex = 3;
this.Controls.Add(this.lblContact_Name);
this.Controls.Add(this.txtContact_Name);

           //#####100Contact_Email###String
this.lblContact_Email.AutoSize = true;
this.lblContact_Email.Location = new System.Drawing.Point(100,100);
this.lblContact_Email.Name = "lblContact_Email";
this.lblContact_Email.Size = new System.Drawing.Size(41, 12);
this.lblContact_Email.TabIndex = 4;
this.lblContact_Email.Text = "邮箱";
this.txtContact_Email.Location = new System.Drawing.Point(173,96);
this.txtContact_Email.Name = "txtContact_Email";
this.txtContact_Email.Size = new System.Drawing.Size(100, 21);
this.txtContact_Email.TabIndex = 4;
this.Controls.Add(this.lblContact_Email);
this.Controls.Add(this.txtContact_Email);

           //#####30Contact_Phone###String
this.lblContact_Phone.AutoSize = true;
this.lblContact_Phone.Location = new System.Drawing.Point(100,125);
this.lblContact_Phone.Name = "lblContact_Phone";
this.lblContact_Phone.Size = new System.Drawing.Size(41, 12);
this.lblContact_Phone.TabIndex = 5;
this.lblContact_Phone.Text = "电话";
this.txtContact_Phone.Location = new System.Drawing.Point(173,121);
this.txtContact_Phone.Name = "txtContact_Phone";
this.txtContact_Phone.Size = new System.Drawing.Size(100, 21);
this.txtContact_Phone.TabIndex = 5;
this.Controls.Add(this.lblContact_Phone);
this.Controls.Add(this.txtContact_Phone);

           //#####50Position###String
this.lblPosition.AutoSize = true;
this.lblPosition.Location = new System.Drawing.Point(100,150);
this.lblPosition.Name = "lblPosition";
this.lblPosition.Size = new System.Drawing.Size(41, 12);
this.lblPosition.TabIndex = 6;
this.lblPosition.Text = "职位";
this.txtPosition.Location = new System.Drawing.Point(173,146);
this.txtPosition.Name = "txtPosition";
this.txtPosition.Size = new System.Drawing.Size(100, 21);
this.txtPosition.TabIndex = 6;
this.Controls.Add(this.lblPosition);
this.Controls.Add(this.txtPosition);

           //#####200Preferences###String
this.lblPreferences.AutoSize = true;
this.lblPreferences.Location = new System.Drawing.Point(100,175);
this.lblPreferences.Name = "lblPreferences";
this.lblPreferences.Size = new System.Drawing.Size(41, 12);
this.lblPreferences.TabIndex = 7;
this.lblPreferences.Text = "爱好";
this.txtPreferences.Location = new System.Drawing.Point(173,171);
this.txtPreferences.Name = "txtPreferences";
this.txtPreferences.Size = new System.Drawing.Size(100, 21);
this.txtPreferences.TabIndex = 7;
this.Controls.Add(this.lblPreferences);
this.Controls.Add(this.txtPreferences);

           //#####255Address###String
this.lblAddress.AutoSize = true;
this.lblAddress.Location = new System.Drawing.Point(100,200);
this.lblAddress.Name = "lblAddress";
this.lblAddress.Size = new System.Drawing.Size(41, 12);
this.lblAddress.TabIndex = 8;
this.lblAddress.Text = "联系地址";
this.txtAddress.Location = new System.Drawing.Point(173,196);
this.txtAddress.Name = "txtAddress";
this.txtAddress.Size = new System.Drawing.Size(100, 21);
this.txtAddress.TabIndex = 8;
this.Controls.Add(this.lblAddress);
this.Controls.Add(this.txtAddress);

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
           // this.kryptonPanel1.TabIndex = 14;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCustomer_id );
this.Controls.Add(this.cmbCustomer_id );

                this.Controls.Add(this.lblSocialTools );
this.Controls.Add(this.txtSocialTools );

                this.Controls.Add(this.lblContact_Name );
this.Controls.Add(this.txtContact_Name );

                this.Controls.Add(this.lblContact_Email );
this.Controls.Add(this.txtContact_Email );

                this.Controls.Add(this.lblContact_Phone );
this.Controls.Add(this.txtContact_Phone );

                this.Controls.Add(this.lblPosition );
this.Controls.Add(this.txtPosition );

                this.Controls.Add(this.lblPreferences );
this.Controls.Add(this.txtPreferences );

                this.Controls.Add(this.lblAddress );
this.Controls.Add(this.txtAddress );

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

                            // 
            // "tb_CRM_ContactEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CRM_ContactEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCustomer_id;
private Krypton.Toolkit.KryptonComboBox cmbCustomer_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblSocialTools;
private Krypton.Toolkit.KryptonTextBox txtSocialTools;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact_Name;
private Krypton.Toolkit.KryptonTextBox txtContact_Name;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact_Email;
private Krypton.Toolkit.KryptonTextBox txtContact_Email;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact_Phone;
private Krypton.Toolkit.KryptonTextBox txtContact_Phone;

    
        
              private Krypton.Toolkit.KryptonLabel lblPosition;
private Krypton.Toolkit.KryptonTextBox txtPosition;

    
        
              private Krypton.Toolkit.KryptonLabel lblPreferences;
private Krypton.Toolkit.KryptonTextBox txtPreferences;

    
        
              private Krypton.Toolkit.KryptonLabel lblAddress;
private Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
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

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

