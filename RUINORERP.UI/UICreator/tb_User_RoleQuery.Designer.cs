
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 用户角色关系表
    /// </summary>
    partial class tb_User_RoleQuery
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
     
     this.lblUser_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUser_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRoleID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRoleID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblAuthorized = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkAuthorized = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkAuthorized.Values.Text ="";

this.lblDefaultRole = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkDefaultRole = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkDefaultRole.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####User_ID###Int64
//属性测试25User_ID
this.lblUser_ID.AutoSize = true;
this.lblUser_ID.Location = new System.Drawing.Point(100,25);
this.lblUser_ID.Name = "lblUser_ID";
this.lblUser_ID.Size = new System.Drawing.Size(41, 12);
this.lblUser_ID.TabIndex = 1;
this.lblUser_ID.Text = "用户";
//111======25
this.cmbUser_ID.Location = new System.Drawing.Point(173,21);
this.cmbUser_ID.Name ="cmbUser_ID";
this.cmbUser_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUser_ID.TabIndex = 1;
this.Controls.Add(this.lblUser_ID);
this.Controls.Add(this.cmbUser_ID);

           //#####RoleID###Int64
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

           //#####Authorized###Boolean
this.lblAuthorized.AutoSize = true;
this.lblAuthorized.Location = new System.Drawing.Point(100,75);
this.lblAuthorized.Name = "lblAuthorized";
this.lblAuthorized.Size = new System.Drawing.Size(41, 12);
this.lblAuthorized.TabIndex = 3;
this.lblAuthorized.Text = "已授权";
this.chkAuthorized.Location = new System.Drawing.Point(173,71);
this.chkAuthorized.Name = "chkAuthorized";
this.chkAuthorized.Size = new System.Drawing.Size(100, 21);
this.chkAuthorized.TabIndex = 3;
this.Controls.Add(this.lblAuthorized);
this.Controls.Add(this.chkAuthorized);

           //#####DefaultRole###Boolean
this.lblDefaultRole.AutoSize = true;
this.lblDefaultRole.Location = new System.Drawing.Point(100,100);
this.lblDefaultRole.Name = "lblDefaultRole";
this.lblDefaultRole.Size = new System.Drawing.Size(41, 12);
this.lblDefaultRole.TabIndex = 4;
this.lblDefaultRole.Text = "默认角色";
this.chkDefaultRole.Location = new System.Drawing.Point(173,96);
this.chkDefaultRole.Name = "chkDefaultRole";
this.chkDefaultRole.Size = new System.Drawing.Size(100, 21);
this.chkDefaultRole.TabIndex = 4;
this.Controls.Add(this.lblDefaultRole);
this.Controls.Add(this.chkDefaultRole);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblUser_ID );
this.Controls.Add(this.cmbUser_ID );

                this.Controls.Add(this.lblRoleID );
this.Controls.Add(this.cmbRoleID );

                this.Controls.Add(this.lblAuthorized );
this.Controls.Add(this.chkAuthorized );

                this.Controls.Add(this.lblDefaultRole );
this.Controls.Add(this.chkDefaultRole );

                    
            this.Name = "tb_User_RoleQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUser_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUser_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRoleID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRoleID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAuthorized;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkAuthorized;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDefaultRole;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkDefaultRole;

    
    
   
 





    }
}


