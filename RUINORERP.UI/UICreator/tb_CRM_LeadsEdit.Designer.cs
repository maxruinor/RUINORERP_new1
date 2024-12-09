// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:15:46
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 线索机会-询盘
    /// </summary>
    partial class tb_CRM_LeadsEdit
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
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblLeadsStatus = new Krypton.Toolkit.KryptonLabel();
            this.txtLeadsStatus = new Krypton.Toolkit.KryptonTextBox();
            this.lblSocialTools = new Krypton.Toolkit.KryptonLabel();
            this.txtSocialTools = new Krypton.Toolkit.KryptonTextBox();
            this.lblCustomerName = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerName = new Krypton.Toolkit.KryptonTextBox();
            this.lblCustomerTags = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerTags = new Krypton.Toolkit.KryptonTextBox();
            this.lblGetCustomerSource = new Krypton.Toolkit.KryptonLabel();
            this.txtGetCustomerSource = new Krypton.Toolkit.KryptonTextBox();
            this.lblInterestedProducts = new Krypton.Toolkit.KryptonLabel();
            this.txtInterestedProducts = new Krypton.Toolkit.KryptonTextBox();
            this.lblContact_Name = new Krypton.Toolkit.KryptonLabel();
            this.txtContact_Name = new Krypton.Toolkit.KryptonTextBox();
            this.lblContact_Phone = new Krypton.Toolkit.KryptonLabel();
            this.txtContact_Phone = new Krypton.Toolkit.KryptonTextBox();
            this.lblContact_Email = new Krypton.Toolkit.KryptonLabel();
            this.txtContact_Email = new Krypton.Toolkit.KryptonTextBox();
            this.lblPosition = new Krypton.Toolkit.KryptonLabel();
            this.txtPosition = new Krypton.Toolkit.KryptonTextBox();
            this.lblSalePlatform = new Krypton.Toolkit.KryptonLabel();
            this.txtSalePlatform = new Krypton.Toolkit.KryptonTextBox();
            this.lblAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblWebsite = new Krypton.Toolkit.KryptonLabel();
            this.txtWebsite = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(100, 25);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(49, 20);
            this.lblEmployee_ID.TabIndex = 1;
            this.lblEmployee_ID.Values.Text = "收集人";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(173, 21);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbEmployee_ID.TabIndex = 1;
            // 
            // lblLeadsStatus
            // 
            this.lblLeadsStatus.Location = new System.Drawing.Point(100, 50);
            this.lblLeadsStatus.Name = "lblLeadsStatus";
            this.lblLeadsStatus.Size = new System.Drawing.Size(62, 20);
            this.lblLeadsStatus.TabIndex = 2;
            this.lblLeadsStatus.Values.Text = "线索状态";
            // 
            // txtLeadsStatus
            // 
            this.txtLeadsStatus.Location = new System.Drawing.Point(173, 46);
            this.txtLeadsStatus.Name = "txtLeadsStatus";
            this.txtLeadsStatus.Size = new System.Drawing.Size(100, 23);
            this.txtLeadsStatus.TabIndex = 2;
            // 
            // lblSocialTools
            // 
            this.lblSocialTools.Location = new System.Drawing.Point(100, 75);
            this.lblSocialTools.Name = "lblSocialTools";
            this.lblSocialTools.Size = new System.Drawing.Size(62, 20);
            this.lblSocialTools.TabIndex = 3;
            this.lblSocialTools.Values.Text = "社交工具";
            // 
            // txtSocialTools
            // 
            this.txtSocialTools.Location = new System.Drawing.Point(173, 71);
            this.txtSocialTools.Name = "txtSocialTools";
            this.txtSocialTools.Size = new System.Drawing.Size(100, 23);
            this.txtSocialTools.TabIndex = 3;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Location = new System.Drawing.Point(100, 100);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerName.TabIndex = 4;
            this.lblCustomerName.Values.Text = "客户名称";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new System.Drawing.Point(173, 96);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(100, 23);
            this.txtCustomerName.TabIndex = 4;
            // 
            // lblCustomerTags
            // 
            this.lblCustomerTags.Location = new System.Drawing.Point(100, 125);
            this.lblCustomerTags.Name = "lblCustomerTags";
            this.lblCustomerTags.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerTags.TabIndex = 5;
            this.lblCustomerTags.Values.Text = "客户标签";
            // 
            // txtCustomerTags
            // 
            this.txtCustomerTags.Location = new System.Drawing.Point(173, 121);
            this.txtCustomerTags.Multiline = true;
            this.txtCustomerTags.Name = "txtCustomerTags";
            this.txtCustomerTags.Size = new System.Drawing.Size(100, 21);
            this.txtCustomerTags.TabIndex = 5;
            // 
            // lblGetCustomerSource
            // 
            this.lblGetCustomerSource.Location = new System.Drawing.Point(100, 150);
            this.lblGetCustomerSource.Name = "lblGetCustomerSource";
            this.lblGetCustomerSource.Size = new System.Drawing.Size(62, 20);
            this.lblGetCustomerSource.TabIndex = 6;
            this.lblGetCustomerSource.Values.Text = "获客来源";
            // 
            // txtGetCustomerSource
            // 
            this.txtGetCustomerSource.Location = new System.Drawing.Point(173, 146);
            this.txtGetCustomerSource.Multiline = true;
            this.txtGetCustomerSource.Name = "txtGetCustomerSource";
            this.txtGetCustomerSource.Size = new System.Drawing.Size(100, 21);
            this.txtGetCustomerSource.TabIndex = 6;
            // 
            // lblInterestedProducts
            // 
            this.lblInterestedProducts.Location = new System.Drawing.Point(100, 175);
            this.lblInterestedProducts.Name = "lblInterestedProducts";
            this.lblInterestedProducts.Size = new System.Drawing.Size(62, 20);
            this.lblInterestedProducts.TabIndex = 7;
            this.lblInterestedProducts.Values.Text = "兴趣产品";
            // 
            // txtInterestedProducts
            // 
            this.txtInterestedProducts.Location = new System.Drawing.Point(173, 171);
            this.txtInterestedProducts.Name = "txtInterestedProducts";
            this.txtInterestedProducts.Size = new System.Drawing.Size(100, 23);
            this.txtInterestedProducts.TabIndex = 7;
            // 
            // lblContact_Name
            // 
            this.lblContact_Name.Location = new System.Drawing.Point(100, 200);
            this.lblContact_Name.Name = "lblContact_Name";
            this.lblContact_Name.Size = new System.Drawing.Size(75, 20);
            this.lblContact_Name.TabIndex = 8;
            this.lblContact_Name.Values.Text = "联系人姓名";
            // 
            // txtContact_Name
            // 
            this.txtContact_Name.Location = new System.Drawing.Point(173, 196);
            this.txtContact_Name.Name = "txtContact_Name";
            this.txtContact_Name.Size = new System.Drawing.Size(100, 23);
            this.txtContact_Name.TabIndex = 8;
            // 
            // lblContact_Phone
            // 
            this.lblContact_Phone.Location = new System.Drawing.Point(100, 225);
            this.lblContact_Phone.Name = "lblContact_Phone";
            this.lblContact_Phone.Size = new System.Drawing.Size(36, 20);
            this.lblContact_Phone.TabIndex = 9;
            this.lblContact_Phone.Values.Text = "电话";
            // 
            // txtContact_Phone
            // 
            this.txtContact_Phone.Location = new System.Drawing.Point(173, 221);
            this.txtContact_Phone.Name = "txtContact_Phone";
            this.txtContact_Phone.Size = new System.Drawing.Size(100, 23);
            this.txtContact_Phone.TabIndex = 9;
            // 
            // lblContact_Email
            // 
            this.lblContact_Email.Location = new System.Drawing.Point(100, 250);
            this.lblContact_Email.Name = "lblContact_Email";
            this.lblContact_Email.Size = new System.Drawing.Size(36, 20);
            this.lblContact_Email.TabIndex = 10;
            this.lblContact_Email.Values.Text = "邮箱";
            // 
            // txtContact_Email
            // 
            this.txtContact_Email.Location = new System.Drawing.Point(173, 246);
            this.txtContact_Email.Name = "txtContact_Email";
            this.txtContact_Email.Size = new System.Drawing.Size(100, 23);
            this.txtContact_Email.TabIndex = 10;
            // 
            // lblPosition
            // 
            this.lblPosition.Location = new System.Drawing.Point(100, 275);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(36, 20);
            this.lblPosition.TabIndex = 11;
            this.lblPosition.Values.Text = "职位";
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(173, 271);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(100, 23);
            this.txtPosition.TabIndex = 11;
            // 
            // lblSalePlatform
            // 
            this.lblSalePlatform.Location = new System.Drawing.Point(100, 300);
            this.lblSalePlatform.Name = "lblSalePlatform";
            this.lblSalePlatform.Size = new System.Drawing.Size(62, 20);
            this.lblSalePlatform.TabIndex = 12;
            this.lblSalePlatform.Values.Text = "销售平台";
            // 
            // txtSalePlatform
            // 
            this.txtSalePlatform.Location = new System.Drawing.Point(173, 296);
            this.txtSalePlatform.Name = "txtSalePlatform";
            this.txtSalePlatform.Size = new System.Drawing.Size(100, 23);
            this.txtSalePlatform.TabIndex = 12;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(100, 325);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(36, 20);
            this.lblAddress.TabIndex = 13;
            this.lblAddress.Values.Text = "地址";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(173, 321);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(100, 21);
            this.txtAddress.TabIndex = 13;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Location = new System.Drawing.Point(100, 350);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(36, 20);
            this.lblWebsite.TabIndex = 14;
            this.lblWebsite.Values.Text = "网址";
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(173, 346);
            this.txtWebsite.Multiline = true;
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(100, 21);
            this.txtWebsite.TabIndex = 14;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(100, 375);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 15;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(173, 371);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(100, 21);
            this.txtNotes.TabIndex = 15;
            // 
            // tb_CRM_LeadsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblEmployee_ID);
            this.Controls.Add(this.cmbEmployee_ID);
            this.Controls.Add(this.lblLeadsStatus);
            this.Controls.Add(this.txtLeadsStatus);
            this.Controls.Add(this.lblSocialTools);
            this.Controls.Add(this.txtSocialTools);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.lblCustomerTags);
            this.Controls.Add(this.txtCustomerTags);
            this.Controls.Add(this.lblGetCustomerSource);
            this.Controls.Add(this.txtGetCustomerSource);
            this.Controls.Add(this.lblInterestedProducts);
            this.Controls.Add(this.txtInterestedProducts);
            this.Controls.Add(this.lblContact_Name);
            this.Controls.Add(this.txtContact_Name);
            this.Controls.Add(this.lblContact_Phone);
            this.Controls.Add(this.txtContact_Phone);
            this.Controls.Add(this.lblContact_Email);
            this.Controls.Add(this.txtContact_Email);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.lblSalePlatform);
            this.Controls.Add(this.txtSalePlatform);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblWebsite);
            this.Controls.Add(this.txtWebsite);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Name = "tb_CRM_LeadsEdit";
            this.Size = new System.Drawing.Size(911, 576);
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLeadsStatus;
private Krypton.Toolkit.KryptonTextBox txtLeadsStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblSocialTools;
private Krypton.Toolkit.KryptonTextBox txtSocialTools;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerName;
private Krypton.Toolkit.KryptonTextBox txtCustomerName;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerTags;
private Krypton.Toolkit.KryptonTextBox txtCustomerTags;

    
        
              private Krypton.Toolkit.KryptonLabel lblGetCustomerSource;
private Krypton.Toolkit.KryptonTextBox txtGetCustomerSource;

    
        
              private Krypton.Toolkit.KryptonLabel lblInterestedProducts;
private Krypton.Toolkit.KryptonTextBox txtInterestedProducts;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact_Name;
private Krypton.Toolkit.KryptonTextBox txtContact_Name;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact_Phone;
private Krypton.Toolkit.KryptonTextBox txtContact_Phone;

    
        
              private Krypton.Toolkit.KryptonLabel lblContact_Email;
private Krypton.Toolkit.KryptonTextBox txtContact_Email;

    
        
              private Krypton.Toolkit.KryptonLabel lblPosition;
private Krypton.Toolkit.KryptonTextBox txtPosition;

    
        
              private Krypton.Toolkit.KryptonLabel lblSalePlatform;
private Krypton.Toolkit.KryptonTextBox txtSalePlatform;

    
        
              private Krypton.Toolkit.KryptonLabel lblAddress;
private Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblWebsite;
private Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;






    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

