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
    /// 机会客户，公海客户 CRM系统中使用，给成交客户作外键引用
    /// </summary>
    partial class tb_CRM_CustomerEdit
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
            this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
            this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();
            this.lblLeadID = new Krypton.Toolkit.KryptonLabel();
            this.cmbLeadID = new Krypton.Toolkit.KryptonComboBox();
            this.lblRegion_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbRegion_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblProvinceID = new Krypton.Toolkit.KryptonLabel();
            this.cmbProvinceID = new Krypton.Toolkit.KryptonComboBox();
            this.lblCityID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCityID = new Krypton.Toolkit.KryptonComboBox();
            this.lblCustomerName = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerName = new Krypton.Toolkit.KryptonTextBox();
            this.lblCustomerAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblRepeatCustomer = new Krypton.Toolkit.KryptonLabel();
            this.chkRepeatCustomer = new Krypton.Toolkit.KryptonCheckBox();
            this.lblCustomerTags = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerTags = new Krypton.Toolkit.KryptonTextBox();
            this.lblCustomerStatus = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerStatus = new Krypton.Toolkit.KryptonTextBox();
            this.lblGetCustomerSource = new Krypton.Toolkit.KryptonLabel();
            this.txtGetCustomerSource = new Krypton.Toolkit.KryptonTextBox();
            this.lblSalePlatform = new Krypton.Toolkit.KryptonLabel();
            this.txtSalePlatform = new Krypton.Toolkit.KryptonTextBox();
            this.lblWebsite = new Krypton.Toolkit.KryptonLabel();
            this.txtWebsite = new Krypton.Toolkit.KryptonTextBox();
            this.lblCustomerLevel = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerLevel = new Krypton.Toolkit.KryptonTextBox();
            this.lblPurchaseCount = new Krypton.Toolkit.KryptonLabel();
            this.txtPurchaseCount = new Krypton.Toolkit.KryptonTextBox();
            this.lblTotalPurchaseAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtTotalPurchaseAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblDaysSinceLastPurchase = new Krypton.Toolkit.KryptonLabel();
            this.txtDaysSinceLastPurchase = new Krypton.Toolkit.KryptonTextBox();
            this.lblLastPurchaseDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpLastPurchaseDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblFirstPurchaseDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpFirstPurchaseDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartmentID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLeadID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRegion_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProvinceID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCityID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(100, 25);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(49, 20);
            this.lblEmployee_ID.TabIndex = 1;
            this.lblEmployee_ID.Values.Text = "对接人";
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
            // lblDepartmentID
            // 
            this.lblDepartmentID.Location = new System.Drawing.Point(100, 50);
            this.lblDepartmentID.Name = "lblDepartmentID";
            this.lblDepartmentID.Size = new System.Drawing.Size(36, 20);
            this.lblDepartmentID.TabIndex = 2;
            this.lblDepartmentID.Values.Text = "部门";
            // 
            // cmbDepartmentID
            // 
            this.cmbDepartmentID.DropDownWidth = 100;
            this.cmbDepartmentID.IntegralHeight = false;
            this.cmbDepartmentID.Location = new System.Drawing.Point(173, 46);
            this.cmbDepartmentID.Name = "cmbDepartmentID";
            this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
            this.cmbDepartmentID.TabIndex = 2;
            // 
            // lblLeadID
            // 
            this.lblLeadID.Location = new System.Drawing.Point(100, 75);
            this.lblLeadID.Name = "lblLeadID";
            this.lblLeadID.Size = new System.Drawing.Size(36, 20);
            this.lblLeadID.TabIndex = 3;
            this.lblLeadID.Values.Text = "线索";
            // 
            // cmbLeadID
            // 
            this.cmbLeadID.DropDownWidth = 100;
            this.cmbLeadID.IntegralHeight = false;
            this.cmbLeadID.Location = new System.Drawing.Point(173, 71);
            this.cmbLeadID.Name = "cmbLeadID";
            this.cmbLeadID.Size = new System.Drawing.Size(100, 21);
            this.cmbLeadID.TabIndex = 3;
            // 
            // lblRegion_ID
            // 
            this.lblRegion_ID.Location = new System.Drawing.Point(100, 100);
            this.lblRegion_ID.Name = "lblRegion_ID";
            this.lblRegion_ID.Size = new System.Drawing.Size(36, 20);
            this.lblRegion_ID.TabIndex = 4;
            this.lblRegion_ID.Values.Text = "地区";
            // 
            // cmbRegion_ID
            // 
            this.cmbRegion_ID.DropDownWidth = 100;
            this.cmbRegion_ID.IntegralHeight = false;
            this.cmbRegion_ID.Location = new System.Drawing.Point(173, 96);
            this.cmbRegion_ID.Name = "cmbRegion_ID";
            this.cmbRegion_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbRegion_ID.TabIndex = 4;
            // 
            // lblProvinceID
            // 
            this.lblProvinceID.Location = new System.Drawing.Point(100, 125);
            this.lblProvinceID.Name = "lblProvinceID";
            this.lblProvinceID.Size = new System.Drawing.Size(23, 20);
            this.lblProvinceID.TabIndex = 5;
            this.lblProvinceID.Values.Text = "省";
            // 
            // cmbProvinceID
            // 
            this.cmbProvinceID.DropDownWidth = 100;
            this.cmbProvinceID.IntegralHeight = false;
            this.cmbProvinceID.Location = new System.Drawing.Point(173, 121);
            this.cmbProvinceID.Name = "cmbProvinceID";
            this.cmbProvinceID.Size = new System.Drawing.Size(100, 21);
            this.cmbProvinceID.TabIndex = 5;
            // 
            // lblCityID
            // 
            this.lblCityID.Location = new System.Drawing.Point(100, 150);
            this.lblCityID.Name = "lblCityID";
            this.lblCityID.Size = new System.Drawing.Size(36, 20);
            this.lblCityID.TabIndex = 6;
            this.lblCityID.Values.Text = "城市";
            // 
            // cmbCityID
            // 
            this.cmbCityID.DropDownWidth = 100;
            this.cmbCityID.IntegralHeight = false;
            this.cmbCityID.Location = new System.Drawing.Point(173, 146);
            this.cmbCityID.Name = "cmbCityID";
            this.cmbCityID.Size = new System.Drawing.Size(100, 21);
            this.cmbCityID.TabIndex = 6;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Location = new System.Drawing.Point(100, 175);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerName.TabIndex = 7;
            this.lblCustomerName.Values.Text = "客户名称";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new System.Drawing.Point(173, 171);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(100, 23);
            this.txtCustomerName.TabIndex = 7;
            // 
            // lblCustomerAddress
            // 
            this.lblCustomerAddress.Location = new System.Drawing.Point(100, 200);
            this.lblCustomerAddress.Name = "lblCustomerAddress";
            this.lblCustomerAddress.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerAddress.TabIndex = 8;
            this.lblCustomerAddress.Values.Text = "客户地址";
            // 
            // txtCustomerAddress
            // 
            this.txtCustomerAddress.Location = new System.Drawing.Point(173, 196);
            this.txtCustomerAddress.Multiline = true;
            this.txtCustomerAddress.Name = "txtCustomerAddress";
            this.txtCustomerAddress.Size = new System.Drawing.Size(100, 21);
            this.txtCustomerAddress.TabIndex = 8;
            // 
            // lblRepeatCustomer
            // 
            this.lblRepeatCustomer.Location = new System.Drawing.Point(100, 225);
            this.lblRepeatCustomer.Name = "lblRepeatCustomer";
            this.lblRepeatCustomer.Size = new System.Drawing.Size(62, 20);
            this.lblRepeatCustomer.TabIndex = 9;
            this.lblRepeatCustomer.Values.Text = "重复客户";
            // 
            // chkRepeatCustomer
            // 
            this.chkRepeatCustomer.Location = new System.Drawing.Point(173, 221);
            this.chkRepeatCustomer.Name = "chkRepeatCustomer";
            this.chkRepeatCustomer.Size = new System.Drawing.Size(19, 13);
            this.chkRepeatCustomer.TabIndex = 9;
            this.chkRepeatCustomer.Values.Text = "";
            // 
            // lblCustomerTags
            // 
            this.lblCustomerTags.Location = new System.Drawing.Point(376, 28);
            this.lblCustomerTags.Name = "lblCustomerTags";
            this.lblCustomerTags.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerTags.TabIndex = 10;
            this.lblCustomerTags.Values.Text = "客户标签";
            // 
            // txtCustomerTags
            // 
            this.txtCustomerTags.Location = new System.Drawing.Point(449, 24);
            this.txtCustomerTags.Multiline = true;
            this.txtCustomerTags.Name = "txtCustomerTags";
            this.txtCustomerTags.Size = new System.Drawing.Size(100, 21);
            this.txtCustomerTags.TabIndex = 10;
            // 
            // lblCustomerStatus
            // 
            this.lblCustomerStatus.Location = new System.Drawing.Point(376, 53);
            this.lblCustomerStatus.Name = "lblCustomerStatus";
            this.lblCustomerStatus.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerStatus.TabIndex = 11;
            this.lblCustomerStatus.Values.Text = "客户状态";
            // 
            // txtCustomerStatus
            // 
            this.txtCustomerStatus.Location = new System.Drawing.Point(449, 49);
            this.txtCustomerStatus.Name = "txtCustomerStatus";
            this.txtCustomerStatus.Size = new System.Drawing.Size(100, 23);
            this.txtCustomerStatus.TabIndex = 11;
            // 
            // lblGetCustomerSource
            // 
            this.lblGetCustomerSource.Location = new System.Drawing.Point(376, 78);
            this.lblGetCustomerSource.Name = "lblGetCustomerSource";
            this.lblGetCustomerSource.Size = new System.Drawing.Size(62, 20);
            this.lblGetCustomerSource.TabIndex = 12;
            this.lblGetCustomerSource.Values.Text = "获客来源";
            // 
            // txtGetCustomerSource
            // 
            this.txtGetCustomerSource.Location = new System.Drawing.Point(449, 74);
            this.txtGetCustomerSource.Multiline = true;
            this.txtGetCustomerSource.Name = "txtGetCustomerSource";
            this.txtGetCustomerSource.Size = new System.Drawing.Size(100, 21);
            this.txtGetCustomerSource.TabIndex = 12;
            // 
            // lblSalePlatform
            // 
            this.lblSalePlatform.Location = new System.Drawing.Point(376, 103);
            this.lblSalePlatform.Name = "lblSalePlatform";
            this.lblSalePlatform.Size = new System.Drawing.Size(62, 20);
            this.lblSalePlatform.TabIndex = 13;
            this.lblSalePlatform.Values.Text = "销售平台";
            // 
            // txtSalePlatform
            // 
            this.txtSalePlatform.Location = new System.Drawing.Point(449, 99);
            this.txtSalePlatform.Name = "txtSalePlatform";
            this.txtSalePlatform.Size = new System.Drawing.Size(100, 23);
            this.txtSalePlatform.TabIndex = 13;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Location = new System.Drawing.Point(376, 128);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(36, 20);
            this.lblWebsite.TabIndex = 14;
            this.lblWebsite.Values.Text = "网址";
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(449, 124);
            this.txtWebsite.Multiline = true;
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(100, 21);
            this.txtWebsite.TabIndex = 14;
            // 
            // lblCustomerLevel
            // 
            this.lblCustomerLevel.Location = new System.Drawing.Point(376, 153);
            this.lblCustomerLevel.Name = "lblCustomerLevel";
            this.lblCustomerLevel.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerLevel.TabIndex = 15;
            this.lblCustomerLevel.Values.Text = "客户级别";
            // 
            // txtCustomerLevel
            // 
            this.txtCustomerLevel.Location = new System.Drawing.Point(449, 149);
            this.txtCustomerLevel.Name = "txtCustomerLevel";
            this.txtCustomerLevel.Size = new System.Drawing.Size(100, 23);
            this.txtCustomerLevel.TabIndex = 15;
            // 
            // lblPurchaseCount
            // 
            this.lblPurchaseCount.Location = new System.Drawing.Point(376, 178);
            this.lblPurchaseCount.Name = "lblPurchaseCount";
            this.lblPurchaseCount.Size = new System.Drawing.Size(62, 20);
            this.lblPurchaseCount.TabIndex = 16;
            this.lblPurchaseCount.Values.Text = "采购次数";
            // 
            // txtPurchaseCount
            // 
            this.txtPurchaseCount.Location = new System.Drawing.Point(449, 174);
            this.txtPurchaseCount.Name = "txtPurchaseCount";
            this.txtPurchaseCount.Size = new System.Drawing.Size(100, 23);
            this.txtPurchaseCount.TabIndex = 16;
            // 
            // lblTotalPurchaseAmount
            // 
            this.lblTotalPurchaseAmount.Location = new System.Drawing.Point(376, 203);
            this.lblTotalPurchaseAmount.Name = "lblTotalPurchaseAmount";
            this.lblTotalPurchaseAmount.Size = new System.Drawing.Size(62, 20);
            this.lblTotalPurchaseAmount.TabIndex = 17;
            this.lblTotalPurchaseAmount.Values.Text = "采购金额";
            // 
            // txtTotalPurchaseAmount
            // 
            this.txtTotalPurchaseAmount.Location = new System.Drawing.Point(449, 199);
            this.txtTotalPurchaseAmount.Name = "txtTotalPurchaseAmount";
            this.txtTotalPurchaseAmount.Size = new System.Drawing.Size(100, 23);
            this.txtTotalPurchaseAmount.TabIndex = 17;
            // 
            // lblDaysSinceLastPurchase
            // 
            this.lblDaysSinceLastPurchase.Location = new System.Drawing.Point(376, 228);
            this.lblDaysSinceLastPurchase.Name = "lblDaysSinceLastPurchase";
            this.lblDaysSinceLastPurchase.Size = new System.Drawing.Size(114, 20);
            this.lblDaysSinceLastPurchase.TabIndex = 18;
            this.lblDaysSinceLastPurchase.Values.Text = "距上次采购间隔天";
            // 
            // txtDaysSinceLastPurchase
            // 
            this.txtDaysSinceLastPurchase.Location = new System.Drawing.Point(449, 224);
            this.txtDaysSinceLastPurchase.Name = "txtDaysSinceLastPurchase";
            this.txtDaysSinceLastPurchase.Size = new System.Drawing.Size(100, 23);
            this.txtDaysSinceLastPurchase.TabIndex = 18;
            // 
            // lblLastPurchaseDate
            // 
            this.lblLastPurchaseDate.Location = new System.Drawing.Point(376, 253);
            this.lblLastPurchaseDate.Name = "lblLastPurchaseDate";
            this.lblLastPurchaseDate.Size = new System.Drawing.Size(88, 20);
            this.lblLastPurchaseDate.TabIndex = 19;
            this.lblLastPurchaseDate.Values.Text = "最近采购日期";
            // 
            // dtpLastPurchaseDate
            // 
            this.dtpLastPurchaseDate.Location = new System.Drawing.Point(449, 249);
            this.dtpLastPurchaseDate.Name = "dtpLastPurchaseDate";
            this.dtpLastPurchaseDate.ShowCheckBox = true;
            this.dtpLastPurchaseDate.Size = new System.Drawing.Size(100, 21);
            this.dtpLastPurchaseDate.TabIndex = 19;
            // 
            // lblFirstPurchaseDate
            // 
            this.lblFirstPurchaseDate.Location = new System.Drawing.Point(376, 278);
            this.lblFirstPurchaseDate.Name = "lblFirstPurchaseDate";
            this.lblFirstPurchaseDate.Size = new System.Drawing.Size(88, 20);
            this.lblFirstPurchaseDate.TabIndex = 20;
            this.lblFirstPurchaseDate.Values.Text = "首次采购日期";
            // 
            // dtpFirstPurchaseDate
            // 
            this.dtpFirstPurchaseDate.Location = new System.Drawing.Point(449, 274);
            this.dtpFirstPurchaseDate.Name = "dtpFirstPurchaseDate";
            this.dtpFirstPurchaseDate.ShowCheckBox = true;
            this.dtpFirstPurchaseDate.Size = new System.Drawing.Size(100, 21);
            this.dtpFirstPurchaseDate.TabIndex = 20;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(376, 303);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 21;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(449, 299);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(100, 21);
            this.txtNotes.TabIndex = 21;
            // 
            // tb_CRM_CustomerEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblEmployee_ID);
            this.Controls.Add(this.cmbEmployee_ID);
            this.Controls.Add(this.lblDepartmentID);
            this.Controls.Add(this.cmbDepartmentID);
            this.Controls.Add(this.lblLeadID);
            this.Controls.Add(this.cmbLeadID);
            this.Controls.Add(this.lblRegion_ID);
            this.Controls.Add(this.cmbRegion_ID);
            this.Controls.Add(this.lblProvinceID);
            this.Controls.Add(this.cmbProvinceID);
            this.Controls.Add(this.lblCityID);
            this.Controls.Add(this.cmbCityID);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.lblCustomerAddress);
            this.Controls.Add(this.txtCustomerAddress);
            this.Controls.Add(this.lblRepeatCustomer);
            this.Controls.Add(this.chkRepeatCustomer);
            this.Controls.Add(this.lblCustomerTags);
            this.Controls.Add(this.txtCustomerTags);
            this.Controls.Add(this.lblCustomerStatus);
            this.Controls.Add(this.txtCustomerStatus);
            this.Controls.Add(this.lblGetCustomerSource);
            this.Controls.Add(this.txtGetCustomerSource);
            this.Controls.Add(this.lblSalePlatform);
            this.Controls.Add(this.txtSalePlatform);
            this.Controls.Add(this.lblWebsite);
            this.Controls.Add(this.txtWebsite);
            this.Controls.Add(this.lblCustomerLevel);
            this.Controls.Add(this.txtCustomerLevel);
            this.Controls.Add(this.lblPurchaseCount);
            this.Controls.Add(this.txtPurchaseCount);
            this.Controls.Add(this.lblTotalPurchaseAmount);
            this.Controls.Add(this.txtTotalPurchaseAmount);
            this.Controls.Add(this.lblDaysSinceLastPurchase);
            this.Controls.Add(this.txtDaysSinceLastPurchase);
            this.Controls.Add(this.lblLastPurchaseDate);
            this.Controls.Add(this.dtpLastPurchaseDate);
            this.Controls.Add(this.lblFirstPurchaseDate);
            this.Controls.Add(this.dtpFirstPurchaseDate);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Name = "tb_CRM_CustomerEdit";
            this.Size = new System.Drawing.Size(911, 785);
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartmentID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLeadID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRegion_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProvinceID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCityID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLeadID;
private Krypton.Toolkit.KryptonComboBox cmbLeadID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRegion_ID;
private Krypton.Toolkit.KryptonComboBox cmbRegion_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProvinceID;
private Krypton.Toolkit.KryptonComboBox cmbProvinceID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCityID;
private Krypton.Toolkit.KryptonComboBox cmbCityID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerName;
private Krypton.Toolkit.KryptonTextBox txtCustomerName;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerAddress;
private Krypton.Toolkit.KryptonTextBox txtCustomerAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblRepeatCustomer;
private Krypton.Toolkit.KryptonCheckBox chkRepeatCustomer;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerTags;
private Krypton.Toolkit.KryptonTextBox txtCustomerTags;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerStatus;
private Krypton.Toolkit.KryptonTextBox txtCustomerStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblGetCustomerSource;
private Krypton.Toolkit.KryptonTextBox txtGetCustomerSource;

    
        
              private Krypton.Toolkit.KryptonLabel lblSalePlatform;
private Krypton.Toolkit.KryptonTextBox txtSalePlatform;

    
        
              private Krypton.Toolkit.KryptonLabel lblWebsite;
private Krypton.Toolkit.KryptonTextBox txtWebsite;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerLevel;
private Krypton.Toolkit.KryptonTextBox txtCustomerLevel;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurchaseCount;
private Krypton.Toolkit.KryptonTextBox txtPurchaseCount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalPurchaseAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalPurchaseAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblDaysSinceLastPurchase;
private Krypton.Toolkit.KryptonTextBox txtDaysSinceLastPurchase;

    
        
              private Krypton.Toolkit.KryptonLabel lblLastPurchaseDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpLastPurchaseDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblFirstPurchaseDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpFirstPurchaseDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;






    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

