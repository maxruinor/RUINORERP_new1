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
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCustomer_id
            // 
            this.lblCustomer_id.Location = new System.Drawing.Point(100, 25);
            this.lblCustomer_id.Name = "lblCustomer_id";
            this.lblCustomer_id.Size = new System.Drawing.Size(62, 20);
            this.lblCustomer_id.TabIndex = 1;
            this.lblCustomer_id.Values.Text = "机会客户";
            // 
            // cmbCustomer_id
            // 
            this.cmbCustomer_id.DropDownWidth = 100;
            this.cmbCustomer_id.IntegralHeight = false;
            this.cmbCustomer_id.Location = new System.Drawing.Point(173, 21);
            this.cmbCustomer_id.Name = "cmbCustomer_id";
            this.cmbCustomer_id.Size = new System.Drawing.Size(100, 21);
            this.cmbCustomer_id.TabIndex = 1;
            // 
            // lblSocialTools
            // 
            this.lblSocialTools.Location = new System.Drawing.Point(100, 50);
            this.lblSocialTools.Name = "lblSocialTools";
            this.lblSocialTools.Size = new System.Drawing.Size(62, 20);
            this.lblSocialTools.TabIndex = 2;
            this.lblSocialTools.Values.Text = "社交工具";
            // 
            // txtSocialTools
            // 
            this.txtSocialTools.Location = new System.Drawing.Point(173, 46);
            this.txtSocialTools.Name = "txtSocialTools";
            this.txtSocialTools.Size = new System.Drawing.Size(100, 23);
            this.txtSocialTools.TabIndex = 2;
            // 
            // lblContact_Name
            // 
            this.lblContact_Name.Location = new System.Drawing.Point(100, 75);
            this.lblContact_Name.Name = "lblContact_Name";
            this.lblContact_Name.Size = new System.Drawing.Size(36, 20);
            this.lblContact_Name.TabIndex = 3;
            this.lblContact_Name.Values.Text = "姓名";
            // 
            // txtContact_Name
            // 
            this.txtContact_Name.Location = new System.Drawing.Point(173, 71);
            this.txtContact_Name.Name = "txtContact_Name";
            this.txtContact_Name.Size = new System.Drawing.Size(100, 23);
            this.txtContact_Name.TabIndex = 3;
            // 
            // lblContact_Email
            // 
            this.lblContact_Email.Location = new System.Drawing.Point(100, 100);
            this.lblContact_Email.Name = "lblContact_Email";
            this.lblContact_Email.Size = new System.Drawing.Size(36, 20);
            this.lblContact_Email.TabIndex = 4;
            this.lblContact_Email.Values.Text = "邮箱";
            // 
            // txtContact_Email
            // 
            this.txtContact_Email.Location = new System.Drawing.Point(173, 96);
            this.txtContact_Email.Name = "txtContact_Email";
            this.txtContact_Email.Size = new System.Drawing.Size(100, 23);
            this.txtContact_Email.TabIndex = 4;
            // 
            // lblContact_Phone
            // 
            this.lblContact_Phone.Location = new System.Drawing.Point(100, 125);
            this.lblContact_Phone.Name = "lblContact_Phone";
            this.lblContact_Phone.Size = new System.Drawing.Size(36, 20);
            this.lblContact_Phone.TabIndex = 5;
            this.lblContact_Phone.Values.Text = "电话";
            // 
            // txtContact_Phone
            // 
            this.txtContact_Phone.Location = new System.Drawing.Point(173, 121);
            this.txtContact_Phone.Name = "txtContact_Phone";
            this.txtContact_Phone.Size = new System.Drawing.Size(100, 23);
            this.txtContact_Phone.TabIndex = 5;
            // 
            // lblPosition
            // 
            this.lblPosition.Location = new System.Drawing.Point(100, 150);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(36, 20);
            this.lblPosition.TabIndex = 6;
            this.lblPosition.Values.Text = "职位";
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(173, 146);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(100, 23);
            this.txtPosition.TabIndex = 6;
            // 
            // lblPreferences
            // 
            this.lblPreferences.Location = new System.Drawing.Point(100, 175);
            this.lblPreferences.Name = "lblPreferences";
            this.lblPreferences.Size = new System.Drawing.Size(36, 20);
            this.lblPreferences.TabIndex = 7;
            this.lblPreferences.Values.Text = "爱好";
            // 
            // txtPreferences
            // 
            this.txtPreferences.Location = new System.Drawing.Point(173, 171);
            this.txtPreferences.Name = "txtPreferences";
            this.txtPreferences.Size = new System.Drawing.Size(100, 23);
            this.txtPreferences.TabIndex = 7;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(100, 200);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(62, 20);
            this.lblAddress.TabIndex = 8;
            this.lblAddress.Values.Text = "联系地址";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(173, 196);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(100, 21);
            this.txtAddress.TabIndex = 8;
            // 
            // tb_CRM_ContactEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCustomer_id);
            this.Controls.Add(this.cmbCustomer_id);
            this.Controls.Add(this.lblSocialTools);
            this.Controls.Add(this.txtSocialTools);
            this.Controls.Add(this.lblContact_Name);
            this.Controls.Add(this.txtContact_Name);
            this.Controls.Add(this.lblContact_Email);
            this.Controls.Add(this.txtContact_Email);
            this.Controls.Add(this.lblContact_Phone);
            this.Controls.Add(this.txtContact_Phone);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.lblPreferences);
            this.Controls.Add(this.txtPreferences);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Name = "tb_CRM_ContactEdit";
            this.Size = new System.Drawing.Size(911, 490);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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







    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

