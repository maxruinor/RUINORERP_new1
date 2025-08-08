// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:43
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 菜单程序集信息表
    /// </summary>
    partial class tb_MenuInfoEdit
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
     this.lblModuleID = new Krypton.Toolkit.KryptonLabel();
this.cmbModuleID = new Krypton.Toolkit.KryptonComboBox();

this.lblMenuName = new Krypton.Toolkit.KryptonLabel();
this.txtMenuName = new Krypton.Toolkit.KryptonTextBox();
this.txtMenuName.Multiline = true;

this.lblMenuType = new Krypton.Toolkit.KryptonLabel();
this.txtMenuType = new Krypton.Toolkit.KryptonTextBox();

this.lblUIPropertyIdentifier = new Krypton.Toolkit.KryptonLabel();
this.txtUIPropertyIdentifier = new Krypton.Toolkit.KryptonTextBox();

this.lblBizInterface = new Krypton.Toolkit.KryptonLabel();
this.txtBizInterface = new Krypton.Toolkit.KryptonTextBox();

this.lblBIBizBaseForm = new Krypton.Toolkit.KryptonLabel();
this.txtBIBizBaseForm = new Krypton.Toolkit.KryptonTextBox();

this.lblBIBaseForm = new Krypton.Toolkit.KryptonLabel();
this.txtBIBaseForm = new Krypton.Toolkit.KryptonTextBox();

this.lblBizType = new Krypton.Toolkit.KryptonLabel();
this.txtBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblUIType = new Krypton.Toolkit.KryptonLabel();
this.txtUIType = new Krypton.Toolkit.KryptonTextBox();

this.lblCaptionCN = new Krypton.Toolkit.KryptonLabel();
this.txtCaptionCN = new Krypton.Toolkit.KryptonTextBox();
this.txtCaptionCN.Multiline = true;

this.lblCaptionEN = new Krypton.Toolkit.KryptonLabel();
this.txtCaptionEN = new Krypton.Toolkit.KryptonTextBox();
this.txtCaptionEN.Multiline = true;

this.lblFormName = new Krypton.Toolkit.KryptonLabel();
this.txtFormName = new Krypton.Toolkit.KryptonTextBox();
this.txtFormName.Multiline = true;

this.lblClassPath = new Krypton.Toolkit.KryptonLabel();
this.txtClassPath = new Krypton.Toolkit.KryptonTextBox();
this.txtClassPath.Multiline = true;

this.lblEntityName = new Krypton.Toolkit.KryptonLabel();
this.txtEntityName = new Krypton.Toolkit.KryptonTextBox();

this.lblIsVisble = new Krypton.Toolkit.KryptonLabel();
this.chkIsVisble = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsVisble.Values.Text ="";

this.lblIsEnabled = new Krypton.Toolkit.KryptonLabel();
this.chkIsEnabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsEnabled.Values.Text ="";
this.chkIsEnabled.Checked = true;
this.chkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblParent_id = new Krypton.Toolkit.KryptonLabel();
this.txtParent_id = new Krypton.Toolkit.KryptonTextBox();

this.lblDiscription = new Krypton.Toolkit.KryptonLabel();
this.txtDiscription = new Krypton.Toolkit.KryptonTextBox();
this.txtDiscription.Multiline = true;

this.lblMenuNo = new Krypton.Toolkit.KryptonLabel();
this.txtMenuNo = new Krypton.Toolkit.KryptonTextBox();
this.txtMenuNo.Multiline = true;

this.lblMenuLevel = new Krypton.Toolkit.KryptonLabel();
this.txtMenuLevel = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblSort = new Krypton.Toolkit.KryptonLabel();
this.txtSort = new Krypton.Toolkit.KryptonTextBox();

this.lblHotKey = new Krypton.Toolkit.KryptonLabel();
this.txtHotKey = new Krypton.Toolkit.KryptonTextBox();

this.lblDefaultLayout = new Krypton.Toolkit.KryptonLabel();
this.txtDefaultLayout = new Krypton.Toolkit.KryptonTextBox();
this.txtDefaultLayout.Multiline = true;

    
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
     
            //#####ModuleID###Int64
//属性测试25ModuleID
this.lblModuleID.AutoSize = true;
this.lblModuleID.Location = new System.Drawing.Point(100,25);
this.lblModuleID.Name = "lblModuleID";
this.lblModuleID.Size = new System.Drawing.Size(41, 12);
this.lblModuleID.TabIndex = 1;
this.lblModuleID.Text = "模块";
//111======25
this.cmbModuleID.Location = new System.Drawing.Point(173,21);
this.cmbModuleID.Name ="cmbModuleID";
this.cmbModuleID.Size = new System.Drawing.Size(100, 21);
this.cmbModuleID.TabIndex = 1;
this.Controls.Add(this.lblModuleID);
this.Controls.Add(this.cmbModuleID);

           //#####255MenuName###String
this.lblMenuName.AutoSize = true;
this.lblMenuName.Location = new System.Drawing.Point(100,50);
this.lblMenuName.Name = "lblMenuName";
this.lblMenuName.Size = new System.Drawing.Size(41, 12);
this.lblMenuName.TabIndex = 2;
this.lblMenuName.Text = "菜单名称";
this.txtMenuName.Location = new System.Drawing.Point(173,46);
this.txtMenuName.Name = "txtMenuName";
this.txtMenuName.Size = new System.Drawing.Size(100, 21);
this.txtMenuName.TabIndex = 2;
this.Controls.Add(this.lblMenuName);
this.Controls.Add(this.txtMenuName);

           //#####20MenuType###String
this.lblMenuType.AutoSize = true;
this.lblMenuType.Location = new System.Drawing.Point(100,75);
this.lblMenuType.Name = "lblMenuType";
this.lblMenuType.Size = new System.Drawing.Size(41, 12);
this.lblMenuType.TabIndex = 3;
this.lblMenuType.Text = "菜单类型";
this.txtMenuType.Location = new System.Drawing.Point(173,71);
this.txtMenuType.Name = "txtMenuType";
this.txtMenuType.Size = new System.Drawing.Size(100, 21);
this.txtMenuType.TabIndex = 3;
this.Controls.Add(this.lblMenuType);
this.Controls.Add(this.txtMenuType);

           //#####150UIPropertyIdentifier###String
this.lblUIPropertyIdentifier.AutoSize = true;
this.lblUIPropertyIdentifier.Location = new System.Drawing.Point(100,100);
this.lblUIPropertyIdentifier.Name = "lblUIPropertyIdentifier";
this.lblUIPropertyIdentifier.Size = new System.Drawing.Size(41, 12);
this.lblUIPropertyIdentifier.TabIndex = 4;
this.lblUIPropertyIdentifier.Text = "注入业务基类";
this.txtUIPropertyIdentifier.Location = new System.Drawing.Point(173,96);
this.txtUIPropertyIdentifier.Name = "txtUIPropertyIdentifier";
this.txtUIPropertyIdentifier.Size = new System.Drawing.Size(100, 21);
this.txtUIPropertyIdentifier.TabIndex = 4;
this.Controls.Add(this.lblUIPropertyIdentifier);
this.Controls.Add(this.txtUIPropertyIdentifier);

           //#####150BizInterface###String
this.lblBizInterface.AutoSize = true;
this.lblBizInterface.Location = new System.Drawing.Point(100,125);
this.lblBizInterface.Name = "lblBizInterface";
this.lblBizInterface.Size = new System.Drawing.Size(41, 12);
this.lblBizInterface.TabIndex = 5;
this.lblBizInterface.Text = "注入业务基类";
this.txtBizInterface.Location = new System.Drawing.Point(173,121);
this.txtBizInterface.Name = "txtBizInterface";
this.txtBizInterface.Size = new System.Drawing.Size(100, 21);
this.txtBizInterface.TabIndex = 5;
this.Controls.Add(this.lblBizInterface);
this.Controls.Add(this.txtBizInterface);

           //#####150BIBizBaseForm###String
this.lblBIBizBaseForm.AutoSize = true;
this.lblBIBizBaseForm.Location = new System.Drawing.Point(100,150);
this.lblBIBizBaseForm.Name = "lblBIBizBaseForm";
this.lblBIBizBaseForm.Size = new System.Drawing.Size(41, 12);
this.lblBIBizBaseForm.TabIndex = 6;
this.lblBIBizBaseForm.Text = "业务接口标识";
this.txtBIBizBaseForm.Location = new System.Drawing.Point(173,146);
this.txtBIBizBaseForm.Name = "txtBIBizBaseForm";
this.txtBIBizBaseForm.Size = new System.Drawing.Size(100, 21);
this.txtBIBizBaseForm.TabIndex = 6;
this.Controls.Add(this.lblBIBizBaseForm);
this.Controls.Add(this.txtBIBizBaseForm);

           //#####100BIBaseForm###String
this.lblBIBaseForm.AutoSize = true;
this.lblBIBaseForm.Location = new System.Drawing.Point(100,175);
this.lblBIBaseForm.Name = "lblBIBaseForm";
this.lblBIBaseForm.Size = new System.Drawing.Size(41, 12);
this.lblBIBaseForm.TabIndex = 7;
this.lblBIBaseForm.Text = "注入框架基类";
this.txtBIBaseForm.Location = new System.Drawing.Point(173,171);
this.txtBIBaseForm.Name = "txtBIBaseForm";
this.txtBIBaseForm.Size = new System.Drawing.Size(100, 21);
this.txtBIBaseForm.TabIndex = 7;
this.Controls.Add(this.lblBIBaseForm);
this.Controls.Add(this.txtBIBaseForm);

           //#####BizType###Int32
//属性测试200BizType
this.lblBizType.AutoSize = true;
this.lblBizType.Location = new System.Drawing.Point(100,200);
this.lblBizType.Name = "lblBizType";
this.lblBizType.Size = new System.Drawing.Size(41, 12);
this.lblBizType.TabIndex = 8;
this.lblBizType.Text = "业务类型";
this.txtBizType.Location = new System.Drawing.Point(173,196);
this.txtBizType.Name = "txtBizType";
this.txtBizType.Size = new System.Drawing.Size(100, 21);
this.txtBizType.TabIndex = 8;
this.Controls.Add(this.lblBizType);
this.Controls.Add(this.txtBizType);

           //#####UIType###Int32
//属性测试225UIType
this.lblUIType.AutoSize = true;
this.lblUIType.Location = new System.Drawing.Point(100,225);
this.lblUIType.Name = "lblUIType";
this.lblUIType.Size = new System.Drawing.Size(41, 12);
this.lblUIType.TabIndex = 9;
this.lblUIType.Text = "窗体类型";
this.txtUIType.Location = new System.Drawing.Point(173,221);
this.txtUIType.Name = "txtUIType";
this.txtUIType.Size = new System.Drawing.Size(100, 21);
this.txtUIType.TabIndex = 9;
this.Controls.Add(this.lblUIType);
this.Controls.Add(this.txtUIType);

           //#####250CaptionCN###String
this.lblCaptionCN.AutoSize = true;
this.lblCaptionCN.Location = new System.Drawing.Point(100,250);
this.lblCaptionCN.Name = "lblCaptionCN";
this.lblCaptionCN.Size = new System.Drawing.Size(41, 12);
this.lblCaptionCN.TabIndex = 10;
this.lblCaptionCN.Text = "中文显示";
this.txtCaptionCN.Location = new System.Drawing.Point(173,246);
this.txtCaptionCN.Name = "txtCaptionCN";
this.txtCaptionCN.Size = new System.Drawing.Size(100, 21);
this.txtCaptionCN.TabIndex = 10;
this.Controls.Add(this.lblCaptionCN);
this.Controls.Add(this.txtCaptionCN);

           //#####250CaptionEN###String
this.lblCaptionEN.AutoSize = true;
this.lblCaptionEN.Location = new System.Drawing.Point(100,275);
this.lblCaptionEN.Name = "lblCaptionEN";
this.lblCaptionEN.Size = new System.Drawing.Size(41, 12);
this.lblCaptionEN.TabIndex = 11;
this.lblCaptionEN.Text = "英文显示";
this.txtCaptionEN.Location = new System.Drawing.Point(173,271);
this.txtCaptionEN.Name = "txtCaptionEN";
this.txtCaptionEN.Size = new System.Drawing.Size(100, 21);
this.txtCaptionEN.TabIndex = 11;
this.Controls.Add(this.lblCaptionEN);
this.Controls.Add(this.txtCaptionEN);

           //#####255FormName###String
this.lblFormName.AutoSize = true;
this.lblFormName.Location = new System.Drawing.Point(100,300);
this.lblFormName.Name = "lblFormName";
this.lblFormName.Size = new System.Drawing.Size(41, 12);
this.lblFormName.TabIndex = 12;
this.lblFormName.Text = "窗体名称";
this.txtFormName.Location = new System.Drawing.Point(173,296);
this.txtFormName.Name = "txtFormName";
this.txtFormName.Size = new System.Drawing.Size(100, 21);
this.txtFormName.TabIndex = 12;
this.Controls.Add(this.lblFormName);
this.Controls.Add(this.txtFormName);

           //#####500ClassPath###String
this.lblClassPath.AutoSize = true;
this.lblClassPath.Location = new System.Drawing.Point(100,325);
this.lblClassPath.Name = "lblClassPath";
this.lblClassPath.Size = new System.Drawing.Size(41, 12);
this.lblClassPath.TabIndex = 13;
this.lblClassPath.Text = "类路径";
this.txtClassPath.Location = new System.Drawing.Point(173,321);
this.txtClassPath.Name = "txtClassPath";
this.txtClassPath.Size = new System.Drawing.Size(100, 21);
this.txtClassPath.TabIndex = 13;
this.Controls.Add(this.lblClassPath);
this.Controls.Add(this.txtClassPath);

           //#####100EntityName###String
this.lblEntityName.AutoSize = true;
this.lblEntityName.Location = new System.Drawing.Point(100,350);
this.lblEntityName.Name = "lblEntityName";
this.lblEntityName.Size = new System.Drawing.Size(41, 12);
this.lblEntityName.TabIndex = 14;
this.lblEntityName.Text = "关联实体名";
this.txtEntityName.Location = new System.Drawing.Point(173,346);
this.txtEntityName.Name = "txtEntityName";
this.txtEntityName.Size = new System.Drawing.Size(100, 21);
this.txtEntityName.TabIndex = 14;
this.Controls.Add(this.lblEntityName);
this.Controls.Add(this.txtEntityName);

           //#####IsVisble###Boolean
this.lblIsVisble.AutoSize = true;
this.lblIsVisble.Location = new System.Drawing.Point(100,375);
this.lblIsVisble.Name = "lblIsVisble";
this.lblIsVisble.Size = new System.Drawing.Size(41, 12);
this.lblIsVisble.TabIndex = 15;
this.lblIsVisble.Text = "是否可见";
this.chkIsVisble.Location = new System.Drawing.Point(173,371);
this.chkIsVisble.Name = "chkIsVisble";
this.chkIsVisble.Size = new System.Drawing.Size(100, 21);
this.chkIsVisble.TabIndex = 15;
this.Controls.Add(this.lblIsVisble);
this.Controls.Add(this.chkIsVisble);

           //#####IsEnabled###Boolean
this.lblIsEnabled.AutoSize = true;
this.lblIsEnabled.Location = new System.Drawing.Point(100,400);
this.lblIsEnabled.Name = "lblIsEnabled";
this.lblIsEnabled.Size = new System.Drawing.Size(41, 12);
this.lblIsEnabled.TabIndex = 16;
this.lblIsEnabled.Text = "是否启用";
this.chkIsEnabled.Location = new System.Drawing.Point(173,396);
this.chkIsEnabled.Name = "chkIsEnabled";
this.chkIsEnabled.Size = new System.Drawing.Size(100, 21);
this.chkIsEnabled.TabIndex = 16;
this.Controls.Add(this.lblIsEnabled);
this.Controls.Add(this.chkIsEnabled);

           //#####Parent_id###Int64
//属性测试425Parent_id
this.lblParent_id.AutoSize = true;
this.lblParent_id.Location = new System.Drawing.Point(100,425);
this.lblParent_id.Name = "lblParent_id";
this.lblParent_id.Size = new System.Drawing.Size(41, 12);
this.lblParent_id.TabIndex = 17;
this.lblParent_id.Text = "父ID";
this.txtParent_id.Location = new System.Drawing.Point(173,421);
this.txtParent_id.Name = "txtParent_id";
this.txtParent_id.Size = new System.Drawing.Size(100, 21);
this.txtParent_id.TabIndex = 17;
this.Controls.Add(this.lblParent_id);
this.Controls.Add(this.txtParent_id);

           //#####250Discription###String
this.lblDiscription.AutoSize = true;
this.lblDiscription.Location = new System.Drawing.Point(100,450);
this.lblDiscription.Name = "lblDiscription";
this.lblDiscription.Size = new System.Drawing.Size(41, 12);
this.lblDiscription.TabIndex = 18;
this.lblDiscription.Text = "描述";
this.txtDiscription.Location = new System.Drawing.Point(173,446);
this.txtDiscription.Name = "txtDiscription";
this.txtDiscription.Size = new System.Drawing.Size(100, 21);
this.txtDiscription.TabIndex = 18;
this.Controls.Add(this.lblDiscription);
this.Controls.Add(this.txtDiscription);

           //#####250MenuNo###String
this.lblMenuNo.AutoSize = true;
this.lblMenuNo.Location = new System.Drawing.Point(100,475);
this.lblMenuNo.Name = "lblMenuNo";
this.lblMenuNo.Size = new System.Drawing.Size(41, 12);
this.lblMenuNo.TabIndex = 19;
this.lblMenuNo.Text = "菜单编码";
this.txtMenuNo.Location = new System.Drawing.Point(173,471);
this.txtMenuNo.Name = "txtMenuNo";
this.txtMenuNo.Size = new System.Drawing.Size(100, 21);
this.txtMenuNo.TabIndex = 19;
this.Controls.Add(this.lblMenuNo);
this.Controls.Add(this.txtMenuNo);

           //#####MenuLevel###Int32
//属性测试500MenuLevel
this.lblMenuLevel.AutoSize = true;
this.lblMenuLevel.Location = new System.Drawing.Point(100,500);
this.lblMenuLevel.Name = "lblMenuLevel";
this.lblMenuLevel.Size = new System.Drawing.Size(41, 12);
this.lblMenuLevel.TabIndex = 20;
this.lblMenuLevel.Text = "菜单级别";
this.txtMenuLevel.Location = new System.Drawing.Point(173,496);
this.txtMenuLevel.Name = "txtMenuLevel";
this.txtMenuLevel.Size = new System.Drawing.Size(100, 21);
this.txtMenuLevel.TabIndex = 20;
this.Controls.Add(this.lblMenuLevel);
this.Controls.Add(this.txtMenuLevel);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,525);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 21;
this.lblCreated_at.Text = "创建时间";
//111======525
this.dtpCreated_at.Location = new System.Drawing.Point(173,521);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 21;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试550Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,550);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 22;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,546);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 22;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,575);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 23;
this.lblModified_at.Text = "修改时间";
//111======575
this.dtpModified_at.Location = new System.Drawing.Point(173,571);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 23;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试600Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,600);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 24;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,596);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 24;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####Sort###Int32
//属性测试625Sort
this.lblSort.AutoSize = true;
this.lblSort.Location = new System.Drawing.Point(100,625);
this.lblSort.Name = "lblSort";
this.lblSort.Size = new System.Drawing.Size(41, 12);
this.lblSort.TabIndex = 25;
this.lblSort.Text = "排序";
this.txtSort.Location = new System.Drawing.Point(173,621);
this.txtSort.Name = "txtSort";
this.txtSort.Size = new System.Drawing.Size(100, 21);
this.txtSort.TabIndex = 25;
this.Controls.Add(this.lblSort);
this.Controls.Add(this.txtSort);

           //#####50HotKey###String
this.lblHotKey.AutoSize = true;
this.lblHotKey.Location = new System.Drawing.Point(100,650);
this.lblHotKey.Name = "lblHotKey";
this.lblHotKey.Size = new System.Drawing.Size(41, 12);
this.lblHotKey.TabIndex = 26;
this.lblHotKey.Text = "热键";
this.txtHotKey.Location = new System.Drawing.Point(173,646);
this.txtHotKey.Name = "txtHotKey";
this.txtHotKey.Size = new System.Drawing.Size(100, 21);
this.txtHotKey.TabIndex = 26;
this.Controls.Add(this.lblHotKey);
this.Controls.Add(this.txtHotKey);

           //#####2147483647DefaultLayout###String
this.lblDefaultLayout.AutoSize = true;
this.lblDefaultLayout.Location = new System.Drawing.Point(100,675);
this.lblDefaultLayout.Name = "lblDefaultLayout";
this.lblDefaultLayout.Size = new System.Drawing.Size(41, 12);
this.lblDefaultLayout.TabIndex = 27;
this.lblDefaultLayout.Text = "菜单默认布局";
this.txtDefaultLayout.Location = new System.Drawing.Point(173,671);
this.txtDefaultLayout.Name = "txtDefaultLayout";
this.txtDefaultLayout.Size = new System.Drawing.Size(100, 21);
this.txtDefaultLayout.TabIndex = 27;
this.txtDefaultLayout.Multiline = true;
this.Controls.Add(this.lblDefaultLayout);
this.Controls.Add(this.txtDefaultLayout);

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
           // this.kryptonPanel1.TabIndex = 27;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblModuleID );
this.Controls.Add(this.cmbModuleID );

                this.Controls.Add(this.lblMenuName );
this.Controls.Add(this.txtMenuName );

                this.Controls.Add(this.lblMenuType );
this.Controls.Add(this.txtMenuType );

                this.Controls.Add(this.lblUIPropertyIdentifier );
this.Controls.Add(this.txtUIPropertyIdentifier );

                this.Controls.Add(this.lblBizInterface );
this.Controls.Add(this.txtBizInterface );

                this.Controls.Add(this.lblBIBizBaseForm );
this.Controls.Add(this.txtBIBizBaseForm );

                this.Controls.Add(this.lblBIBaseForm );
this.Controls.Add(this.txtBIBaseForm );

                this.Controls.Add(this.lblBizType );
this.Controls.Add(this.txtBizType );

                this.Controls.Add(this.lblUIType );
this.Controls.Add(this.txtUIType );

                this.Controls.Add(this.lblCaptionCN );
this.Controls.Add(this.txtCaptionCN );

                this.Controls.Add(this.lblCaptionEN );
this.Controls.Add(this.txtCaptionEN );

                this.Controls.Add(this.lblFormName );
this.Controls.Add(this.txtFormName );

                this.Controls.Add(this.lblClassPath );
this.Controls.Add(this.txtClassPath );

                this.Controls.Add(this.lblEntityName );
this.Controls.Add(this.txtEntityName );

                this.Controls.Add(this.lblIsVisble );
this.Controls.Add(this.chkIsVisble );

                this.Controls.Add(this.lblIsEnabled );
this.Controls.Add(this.chkIsEnabled );

                this.Controls.Add(this.lblParent_id );
this.Controls.Add(this.txtParent_id );

                this.Controls.Add(this.lblDiscription );
this.Controls.Add(this.txtDiscription );

                this.Controls.Add(this.lblMenuNo );
this.Controls.Add(this.txtMenuNo );

                this.Controls.Add(this.lblMenuLevel );
this.Controls.Add(this.txtMenuLevel );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblSort );
this.Controls.Add(this.txtSort );

                this.Controls.Add(this.lblHotKey );
this.Controls.Add(this.txtHotKey );

                this.Controls.Add(this.lblDefaultLayout );
this.Controls.Add(this.txtDefaultLayout );

                            // 
            // "tb_MenuInfoEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_MenuInfoEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblModuleID;
private Krypton.Toolkit.KryptonComboBox cmbModuleID;

    
        
              private Krypton.Toolkit.KryptonLabel lblMenuName;
private Krypton.Toolkit.KryptonTextBox txtMenuName;

    
        
              private Krypton.Toolkit.KryptonLabel lblMenuType;
private Krypton.Toolkit.KryptonTextBox txtMenuType;

    
        
              private Krypton.Toolkit.KryptonLabel lblUIPropertyIdentifier;
private Krypton.Toolkit.KryptonTextBox txtUIPropertyIdentifier;

    
        
              private Krypton.Toolkit.KryptonLabel lblBizInterface;
private Krypton.Toolkit.KryptonTextBox txtBizInterface;

    
        
              private Krypton.Toolkit.KryptonLabel lblBIBizBaseForm;
private Krypton.Toolkit.KryptonTextBox txtBIBizBaseForm;

    
        
              private Krypton.Toolkit.KryptonLabel lblBIBaseForm;
private Krypton.Toolkit.KryptonTextBox txtBIBaseForm;

    
        
              private Krypton.Toolkit.KryptonLabel lblBizType;
private Krypton.Toolkit.KryptonTextBox txtBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblUIType;
private Krypton.Toolkit.KryptonTextBox txtUIType;

    
        
              private Krypton.Toolkit.KryptonLabel lblCaptionCN;
private Krypton.Toolkit.KryptonTextBox txtCaptionCN;

    
        
              private Krypton.Toolkit.KryptonLabel lblCaptionEN;
private Krypton.Toolkit.KryptonTextBox txtCaptionEN;

    
        
              private Krypton.Toolkit.KryptonLabel lblFormName;
private Krypton.Toolkit.KryptonTextBox txtFormName;

    
        
              private Krypton.Toolkit.KryptonLabel lblClassPath;
private Krypton.Toolkit.KryptonTextBox txtClassPath;

    
        
              private Krypton.Toolkit.KryptonLabel lblEntityName;
private Krypton.Toolkit.KryptonTextBox txtEntityName;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsVisble;
private Krypton.Toolkit.KryptonCheckBox chkIsVisble;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsEnabled;
private Krypton.Toolkit.KryptonCheckBox chkIsEnabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblParent_id;
private Krypton.Toolkit.KryptonTextBox txtParent_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiscription;
private Krypton.Toolkit.KryptonTextBox txtDiscription;

    
        
              private Krypton.Toolkit.KryptonLabel lblMenuNo;
private Krypton.Toolkit.KryptonTextBox txtMenuNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblMenuLevel;
private Krypton.Toolkit.KryptonTextBox txtMenuLevel;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblSort;
private Krypton.Toolkit.KryptonTextBox txtSort;

    
        
              private Krypton.Toolkit.KryptonLabel lblHotKey;
private Krypton.Toolkit.KryptonTextBox txtHotKey;

    
        
              private Krypton.Toolkit.KryptonLabel lblDefaultLayout;
private Krypton.Toolkit.KryptonTextBox txtDefaultLayout;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

