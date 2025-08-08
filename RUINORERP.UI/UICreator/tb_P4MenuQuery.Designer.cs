
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:47
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 菜单权限表
    /// </summary>
    partial class tb_P4MenuQuery
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
     
     this.lblMenuID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbMenuID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRoleID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRoleID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblModuleID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbModuleID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblIsVisble = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsVisble = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsVisble.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####MenuID###Int64
//属性测试25MenuID
this.lblMenuID.AutoSize = true;
this.lblMenuID.Location = new System.Drawing.Point(100,25);
this.lblMenuID.Name = "lblMenuID";
this.lblMenuID.Size = new System.Drawing.Size(41, 12);
this.lblMenuID.TabIndex = 1;
this.lblMenuID.Text = "菜单";
//111======25
this.cmbMenuID.Location = new System.Drawing.Point(173,21);
this.cmbMenuID.Name ="cmbMenuID";
this.cmbMenuID.Size = new System.Drawing.Size(100, 21);
this.cmbMenuID.TabIndex = 1;
this.Controls.Add(this.lblMenuID);
this.Controls.Add(this.cmbMenuID);

           //#####RoleID###Int64
//属性测试50RoleID
//属性测试50RoleID
//属性测试50RoleID
this.lblRoleID.AutoSize = true;
this.lblRoleID.Location = new System.Drawing.Point(100,50);
this.lblRoleID.Name = "lblRoleID";
this.lblRoleID.Size = new System.Drawing.Size(41, 12);
this.lblRoleID.TabIndex = 2;
this.lblRoleID.Text = "角色";
//111======50
this.cmbRoleID.Location = new System.Drawing.Point(173,46);
this.cmbRoleID.Name ="cmbRoleID";
this.cmbRoleID.Size = new System.Drawing.Size(100, 21);
this.cmbRoleID.TabIndex = 2;
this.Controls.Add(this.lblRoleID);
this.Controls.Add(this.cmbRoleID);

           //#####ModuleID###Int64
//属性测试75ModuleID
//属性测试75ModuleID
this.lblModuleID.AutoSize = true;
this.lblModuleID.Location = new System.Drawing.Point(100,75);
this.lblModuleID.Name = "lblModuleID";
this.lblModuleID.Size = new System.Drawing.Size(41, 12);
this.lblModuleID.TabIndex = 3;
this.lblModuleID.Text = "模块";
//111======75
this.cmbModuleID.Location = new System.Drawing.Point(173,71);
this.cmbModuleID.Name ="cmbModuleID";
this.cmbModuleID.Size = new System.Drawing.Size(100, 21);
this.cmbModuleID.TabIndex = 3;
this.Controls.Add(this.lblModuleID);
this.Controls.Add(this.cmbModuleID);

           //#####IsVisble###Boolean
this.lblIsVisble.AutoSize = true;
this.lblIsVisble.Location = new System.Drawing.Point(100,100);
this.lblIsVisble.Name = "lblIsVisble";
this.lblIsVisble.Size = new System.Drawing.Size(41, 12);
this.lblIsVisble.TabIndex = 4;
this.lblIsVisble.Text = "是否可见";
this.chkIsVisble.Location = new System.Drawing.Point(173,96);
this.chkIsVisble.Name = "chkIsVisble";
this.chkIsVisble.Size = new System.Drawing.Size(100, 21);
this.chkIsVisble.TabIndex = 4;
this.Controls.Add(this.lblIsVisble);
this.Controls.Add(this.chkIsVisble);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,125);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 5;
this.lblCreated_at.Text = "创建时间";
//111======125
this.dtpCreated_at.Location = new System.Drawing.Point(173,121);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 5;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试150Created_by
//属性测试150Created_by
//属性测试150Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,175);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 7;
this.lblModified_at.Text = "修改时间";
//111======175
this.dtpModified_at.Location = new System.Drawing.Point(173,171);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 7;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试200Modified_by
//属性测试200Modified_by
//属性测试200Modified_by

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMenuID );
this.Controls.Add(this.cmbMenuID );

                this.Controls.Add(this.lblRoleID );
this.Controls.Add(this.cmbRoleID );

                this.Controls.Add(this.lblModuleID );
this.Controls.Add(this.cmbModuleID );

                this.Controls.Add(this.lblIsVisble );
this.Controls.Add(this.chkIsVisble );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_P4MenuQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMenuID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMenuID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRoleID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRoleID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModuleID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbModuleID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsVisble;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsVisble;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


