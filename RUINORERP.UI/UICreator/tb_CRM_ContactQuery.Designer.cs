
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:42
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
    partial class tb_CRM_ContactQuery
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
     
     this.lblCustomer_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomer_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact_Email = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact_Email = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact_Phone = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact_Phone = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPosition = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPosition = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPreferences = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPreferences = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtAddress.Multiline = true;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Customer_id###Int64
//属性测试25Customer_id
this.lblCustomer_id.AutoSize = true;
this.lblCustomer_id.Location = new System.Drawing.Point(100,25);
this.lblCustomer_id.Name = "lblCustomer_id";
this.lblCustomer_id.Size = new System.Drawing.Size(41, 12);
this.lblCustomer_id.TabIndex = 1;
this.lblCustomer_id.Text = "机会客户";
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
this.lblSocialTools.Text = "社交工具";
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_CRM_ContactQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomer_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomer_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSocialTools;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSocialTools;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact_Email;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact_Email;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact_Phone;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact_Phone;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPosition;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPosition;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPreferences;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPreferences;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


