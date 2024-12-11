// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/11/2024 20:44:23
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
     this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblLeadsStatus = new Krypton.Toolkit.KryptonLabel();
this.txtLeadsStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblwwSocialTools = new Krypton.Toolkit.KryptonLabel();
this.txtwwSocialTools = new Krypton.Toolkit.KryptonTextBox();

this.lblSocialTools = new Krypton.Toolkit.KryptonLabel();
this.txtSocialTools = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerName = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerName = new Krypton.Toolkit.KryptonTextBox();

this.lblGetCustomerSource = new Krypton.Toolkit.KryptonLabel();
this.txtGetCustomerSource = new Krypton.Toolkit.KryptonTextBox();
this.txtGetCustomerSource.Multiline = true;

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
this.txtAddress.Multiline = true;

this.lblWebsite = new Krypton.Toolkit.KryptonLabel();
this.txtWebsite = new Krypton.Toolkit.KryptonTextBox();
this.txtWebsite.Multiline = true;

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

this.lblConverted = new Krypton.Toolkit.KryptonLabel();
this.chkConverted = new Krypton.Toolkit.KryptonCheckBox();
this.chkConverted.Values.Text ="";

    
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
this.lblLeadsStatus.AutoSize = true;
this.lblLeadsStatus.Location = new System.Drawing.Point(100,50);
this.lblLeadsStatus.Name = "lblLeadsStatus";
this.lblLeadsStatus.Size = new System.Drawing.Size(41, 12);
this.lblLeadsStatus.TabIndex = 2;
this.lblLeadsStatus.Text = "线索状态";
this.txtLeadsStatus.Location = new System.Drawing.Point(173,46);
this.txtLeadsStatus.Name = "txtLeadsStatus";
this.txtLeadsStatus.Size = new System.Drawing.Size(100, 21);
this.txtLeadsStatus.TabIndex = 2;
this.Controls.Add(this.lblLeadsStatus);
this.Controls.Add(this.txtLeadsStatus);

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
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,425);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 17;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,421);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 17;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,475);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 19;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,471);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 19;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
           // this.kryptonPanel1.TabIndex = 21;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblLeadsStatus );
this.Controls.Add(this.txtLeadsStatus );

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

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblConverted );
this.Controls.Add(this.chkConverted );

                            // 
            // "tb_CRM_LeadsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CRM_LeadsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLeadsStatus;
private Krypton.Toolkit.KryptonTextBox txtLeadsStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblwwSocialTools;
private Krypton.Toolkit.KryptonTextBox txtwwSocialTools;

    
        
              private Krypton.Toolkit.KryptonLabel lblSocialTools;
private Krypton.Toolkit.KryptonTextBox txtSocialTools;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerName;
private Krypton.Toolkit.KryptonTextBox txtCustomerName;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblConverted;
private Krypton.Toolkit.KryptonCheckBox chkConverted;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

