
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:19
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
    partial class tb_CRM_LeadsQuery
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
     
     this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblwwSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtwwSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSocialTools = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblGetCustomerSource = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtGetCustomerSource = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtGetCustomerSource.Multiline = true;

this.lblInterestedProducts = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInterestedProducts = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact_Phone = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact_Phone = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblContact_Email = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtContact_Email = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPosition = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPosition = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSalePlatform = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSalePlatform = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtAddress.Multiline = true;

this.lblWebsite = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWebsite = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtWebsite.Multiline = true;

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

this.lblConverted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkConverted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkConverted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Employee_ID###Int64
//属性测试25Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,25);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 1;
this.lblEmployee_ID.Text = "收集人";
//111======25
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,21);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 1;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####LeadsStatus###Int32
//属性测试50LeadsStatus

           //#####200wwSocialTools###String
this.lblwwSocialTools.AutoSize = true;
this.lblwwSocialTools.Location = new System.Drawing.Point(100,75);
this.lblwwSocialTools.Name = "lblwwSocialTools";
this.lblwwSocialTools.Size = new System.Drawing.Size(41, 12);
this.lblwwSocialTools.TabIndex = 3;
this.lblwwSocialTools.Text = "其他/IM工具";
this.txtwwSocialTools.Location = new System.Drawing.Point(173,71);
this.txtwwSocialTools.Name = "txtwwSocialTools";
this.txtwwSocialTools.Size = new System.Drawing.Size(100, 21);
this.txtwwSocialTools.TabIndex = 3;
this.Controls.Add(this.lblwwSocialTools);
this.Controls.Add(this.txtwwSocialTools);

           //#####200SocialTools###String
this.lblSocialTools.AutoSize = true;
this.lblSocialTools.Location = new System.Drawing.Point(100,100);
this.lblSocialTools.Name = "lblSocialTools";
this.lblSocialTools.Size = new System.Drawing.Size(41, 12);
this.lblSocialTools.TabIndex = 4;
this.lblSocialTools.Text = "旺旺/IM工具";
this.txtSocialTools.Location = new System.Drawing.Point(173,96);
this.txtSocialTools.Name = "txtSocialTools";
this.txtSocialTools.Size = new System.Drawing.Size(100, 21);
this.txtSocialTools.TabIndex = 4;
this.Controls.Add(this.lblSocialTools);
this.Controls.Add(this.txtSocialTools);

           //#####100CustomerName###String
this.lblCustomerName.AutoSize = true;
this.lblCustomerName.Location = new System.Drawing.Point(100,125);
this.lblCustomerName.Name = "lblCustomerName";
this.lblCustomerName.Size = new System.Drawing.Size(41, 12);
this.lblCustomerName.TabIndex = 5;
this.lblCustomerName.Text = "客户名/线索名";
this.txtCustomerName.Location = new System.Drawing.Point(173,121);
this.txtCustomerName.Name = "txtCustomerName";
this.txtCustomerName.Size = new System.Drawing.Size(100, 21);
this.txtCustomerName.TabIndex = 5;
this.Controls.Add(this.lblCustomerName);
this.Controls.Add(this.txtCustomerName);

           //#####250GetCustomerSource###String
this.lblGetCustomerSource.AutoSize = true;
this.lblGetCustomerSource.Location = new System.Drawing.Point(100,150);
this.lblGetCustomerSource.Name = "lblGetCustomerSource";
this.lblGetCustomerSource.Size = new System.Drawing.Size(41, 12);
this.lblGetCustomerSource.TabIndex = 6;
this.lblGetCustomerSource.Text = "获客来源";
this.txtGetCustomerSource.Location = new System.Drawing.Point(173,146);
this.txtGetCustomerSource.Name = "txtGetCustomerSource";
this.txtGetCustomerSource.Size = new System.Drawing.Size(100, 21);
this.txtGetCustomerSource.TabIndex = 6;
this.Controls.Add(this.lblGetCustomerSource);
this.Controls.Add(this.txtGetCustomerSource);

           //#####50InterestedProducts###String
this.lblInterestedProducts.AutoSize = true;
this.lblInterestedProducts.Location = new System.Drawing.Point(100,175);
this.lblInterestedProducts.Name = "lblInterestedProducts";
this.lblInterestedProducts.Size = new System.Drawing.Size(41, 12);
this.lblInterestedProducts.TabIndex = 7;
this.lblInterestedProducts.Text = "兴趣产品";
this.txtInterestedProducts.Location = new System.Drawing.Point(173,171);
this.txtInterestedProducts.Name = "txtInterestedProducts";
this.txtInterestedProducts.Size = new System.Drawing.Size(100, 21);
this.txtInterestedProducts.TabIndex = 7;
this.Controls.Add(this.lblInterestedProducts);
this.Controls.Add(this.txtInterestedProducts);

           //#####50Contact_Name###String
this.lblContact_Name.AutoSize = true;
this.lblContact_Name.Location = new System.Drawing.Point(100,200);
this.lblContact_Name.Name = "lblContact_Name";
this.lblContact_Name.Size = new System.Drawing.Size(41, 12);
this.lblContact_Name.TabIndex = 8;
this.lblContact_Name.Text = "联系人姓名";
this.txtContact_Name.Location = new System.Drawing.Point(173,196);
this.txtContact_Name.Name = "txtContact_Name";
this.txtContact_Name.Size = new System.Drawing.Size(100, 21);
this.txtContact_Name.TabIndex = 8;
this.Controls.Add(this.lblContact_Name);
this.Controls.Add(this.txtContact_Name);

           //#####50Contact_Phone###String
this.lblContact_Phone.AutoSize = true;
this.lblContact_Phone.Location = new System.Drawing.Point(100,225);
this.lblContact_Phone.Name = "lblContact_Phone";
this.lblContact_Phone.Size = new System.Drawing.Size(41, 12);
this.lblContact_Phone.TabIndex = 9;
this.lblContact_Phone.Text = "电话";
this.txtContact_Phone.Location = new System.Drawing.Point(173,221);
this.txtContact_Phone.Name = "txtContact_Phone";
this.txtContact_Phone.Size = new System.Drawing.Size(100, 21);
this.txtContact_Phone.TabIndex = 9;
this.Controls.Add(this.lblContact_Phone);
this.Controls.Add(this.txtContact_Phone);

           //#####100Contact_Email###String
this.lblContact_Email.AutoSize = true;
this.lblContact_Email.Location = new System.Drawing.Point(100,250);
this.lblContact_Email.Name = "lblContact_Email";
this.lblContact_Email.Size = new System.Drawing.Size(41, 12);
this.lblContact_Email.TabIndex = 10;
this.lblContact_Email.Text = "邮箱";
this.txtContact_Email.Location = new System.Drawing.Point(173,246);
this.txtContact_Email.Name = "txtContact_Email";
this.txtContact_Email.Size = new System.Drawing.Size(100, 21);
this.txtContact_Email.TabIndex = 10;
this.Controls.Add(this.lblContact_Email);
this.Controls.Add(this.txtContact_Email);

           //#####50Position###String
this.lblPosition.AutoSize = true;
this.lblPosition.Location = new System.Drawing.Point(100,275);
this.lblPosition.Name = "lblPosition";
this.lblPosition.Size = new System.Drawing.Size(41, 12);
this.lblPosition.TabIndex = 11;
this.lblPosition.Text = "职位";
this.txtPosition.Location = new System.Drawing.Point(173,271);
this.txtPosition.Name = "txtPosition";
this.txtPosition.Size = new System.Drawing.Size(100, 21);
this.txtPosition.TabIndex = 11;
this.Controls.Add(this.lblPosition);
this.Controls.Add(this.txtPosition);

           //#####50SalePlatform###String
this.lblSalePlatform.AutoSize = true;
this.lblSalePlatform.Location = new System.Drawing.Point(100,300);
this.lblSalePlatform.Name = "lblSalePlatform";
this.lblSalePlatform.Size = new System.Drawing.Size(41, 12);
this.lblSalePlatform.TabIndex = 12;
this.lblSalePlatform.Text = "销售平台";
this.txtSalePlatform.Location = new System.Drawing.Point(173,296);
this.txtSalePlatform.Name = "txtSalePlatform";
this.txtSalePlatform.Size = new System.Drawing.Size(100, 21);
this.txtSalePlatform.TabIndex = 12;
this.Controls.Add(this.lblSalePlatform);
this.Controls.Add(this.txtSalePlatform);

           //#####255Address###String
this.lblAddress.AutoSize = true;
this.lblAddress.Location = new System.Drawing.Point(100,325);
this.lblAddress.Name = "lblAddress";
this.lblAddress.Size = new System.Drawing.Size(41, 12);
this.lblAddress.TabIndex = 13;
this.lblAddress.Text = "地址";
this.txtAddress.Location = new System.Drawing.Point(173,321);
this.txtAddress.Name = "txtAddress";
this.txtAddress.Size = new System.Drawing.Size(100, 21);
this.txtAddress.TabIndex = 13;
this.Controls.Add(this.lblAddress);
this.Controls.Add(this.txtAddress);

           //#####255Website###String
this.lblWebsite.AutoSize = true;
this.lblWebsite.Location = new System.Drawing.Point(100,350);
this.lblWebsite.Name = "lblWebsite";
this.lblWebsite.Size = new System.Drawing.Size(41, 12);
this.lblWebsite.TabIndex = 14;
this.lblWebsite.Text = "网址";
this.txtWebsite.Location = new System.Drawing.Point(173,346);
this.txtWebsite.Name = "txtWebsite";
this.txtWebsite.Size = new System.Drawing.Size(100, 21);
this.txtWebsite.TabIndex = 14;
this.Controls.Add(this.lblWebsite);
this.Controls.Add(this.txtWebsite);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,375);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 15;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,371);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 15;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,400);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 16;
this.lblCreated_at.Text = "创建时间";
//111======400
this.dtpCreated_at.Location = new System.Drawing.Point(173,396);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 16;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试425Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,450);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 18;
this.lblModified_at.Text = "修改时间";
//111======450
this.dtpModified_at.Location = new System.Drawing.Point(173,446);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 18;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试475Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,500);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 20;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,496);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 20;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Converted###Boolean
this.lblConverted.AutoSize = true;
this.lblConverted.Location = new System.Drawing.Point(100,525);
this.lblConverted.Name = "lblConverted";
this.lblConverted.Size = new System.Drawing.Size(41, 12);
this.lblConverted.TabIndex = 21;
this.lblConverted.Text = "已转化";
this.chkConverted.Location = new System.Drawing.Point(173,521);
this.chkConverted.Name = "chkConverted";
this.chkConverted.Size = new System.Drawing.Size(100, 21);
this.chkConverted.TabIndex = 21;
this.Controls.Add(this.lblConverted);
this.Controls.Add(this.chkConverted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                
                this.Controls.Add(this.lblwwSocialTools );
this.Controls.Add(this.txtwwSocialTools );

                this.Controls.Add(this.lblSocialTools );
this.Controls.Add(this.txtSocialTools );

                this.Controls.Add(this.lblCustomerName );
this.Controls.Add(this.txtCustomerName );

                this.Controls.Add(this.lblGetCustomerSource );
this.Controls.Add(this.txtGetCustomerSource );

                this.Controls.Add(this.lblInterestedProducts );
this.Controls.Add(this.txtInterestedProducts );

                this.Controls.Add(this.lblContact_Name );
this.Controls.Add(this.txtContact_Name );

                this.Controls.Add(this.lblContact_Phone );
this.Controls.Add(this.txtContact_Phone );

                this.Controls.Add(this.lblContact_Email );
this.Controls.Add(this.txtContact_Email );

                this.Controls.Add(this.lblPosition );
this.Controls.Add(this.txtPosition );

                this.Controls.Add(this.lblSalePlatform );
this.Controls.Add(this.txtSalePlatform );

                this.Controls.Add(this.lblAddress );
this.Controls.Add(this.txtAddress );

                this.Controls.Add(this.lblWebsite );
this.Controls.Add(this.txtWebsite );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblConverted );
this.Controls.Add(this.chkConverted );

                    
            this.Name = "tb_CRM_LeadsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblwwSocialTools;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtwwSocialTools;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSocialTools;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSocialTools;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGetCustomerSource;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGetCustomerSource;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInterestedProducts;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInterestedProducts;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact_Phone;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact_Phone;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblContact_Email;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtContact_Email;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPosition;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPosition;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSalePlatform;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSalePlatform;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWebsite;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConverted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkConverted;

    
    
   
 





    }
}


