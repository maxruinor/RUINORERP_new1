
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
    partial class tb_MenuInfoQuery
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
     
     this.lblModuleID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbModuleID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblMenuName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMenuName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtMenuName.Multiline = true;

this.lblMenuType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMenuType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUIPropertyIdentifier = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUIPropertyIdentifier = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBizInterface = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBizInterface = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBIBizBaseForm = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBIBizBaseForm = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBIBaseForm = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBIBaseForm = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblCaptionCN = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCaptionCN = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCaptionCN.Multiline = true;

this.lblCaptionEN = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCaptionEN = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCaptionEN.Multiline = true;

this.lblFormName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFormName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtFormName.Multiline = true;

this.lblClassPath = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtClassPath = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtClassPath.Multiline = true;

this.lblEntityName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEntityName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsVisble = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsVisble = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsVisble.Values.Text ="";

this.lblIsEnabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsEnabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsEnabled.Values.Text ="";
this.chkIsEnabled.Checked = true;
this.chkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;


this.lblDiscription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDiscription.Multiline = true;

this.lblMenuNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMenuNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtMenuNo.Multiline = true;


this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblHotKey = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtHotKey = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDefaultLayout = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDefaultLayout = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDefaultLayout.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
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

           //#####UIType###Int32
//属性测试225UIType

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

           //#####Sort###Int32
//属性测试625Sort

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                
                this.Controls.Add(this.lblDiscription );
this.Controls.Add(this.txtDiscription );

                this.Controls.Add(this.lblMenuNo );
this.Controls.Add(this.txtMenuNo );

                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                
                this.Controls.Add(this.lblHotKey );
this.Controls.Add(this.txtHotKey );

                this.Controls.Add(this.lblDefaultLayout );
this.Controls.Add(this.txtDefaultLayout );

                    
            this.Name = "tb_MenuInfoQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModuleID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbModuleID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMenuName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMenuName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMenuType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMenuType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUIPropertyIdentifier;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUIPropertyIdentifier;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBizInterface;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBizInterface;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBIBizBaseForm;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBIBizBaseForm;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBIBaseForm;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBIBaseForm;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCaptionCN;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCaptionCN;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCaptionEN;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCaptionEN;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFormName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFormName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClassPath;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClassPath;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEntityName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEntityName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsVisble;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsVisble;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsEnabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsEnabled;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMenuNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMenuNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblHotKey;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtHotKey;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDefaultLayout;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDefaultLayout;

    
    
   
 





    }
}


