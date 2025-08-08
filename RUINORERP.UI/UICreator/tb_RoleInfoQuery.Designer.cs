
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:11
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 角色表
    /// </summary>
    partial class tb_RoleInfoQuery
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
     
     this.lblRoleName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRoleName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDesc.Multiline = true;

this.lblRolePropertyID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRolePropertyID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50RoleName###String
this.lblRoleName.AutoSize = true;
this.lblRoleName.Location = new System.Drawing.Point(100,25);
this.lblRoleName.Name = "lblRoleName";
this.lblRoleName.Size = new System.Drawing.Size(41, 12);
this.lblRoleName.TabIndex = 1;
this.lblRoleName.Text = "角色名称";
this.txtRoleName.Location = new System.Drawing.Point(173,21);
this.txtRoleName.Name = "txtRoleName";
this.txtRoleName.Size = new System.Drawing.Size(100, 21);
this.txtRoleName.TabIndex = 1;
this.Controls.Add(this.lblRoleName);
this.Controls.Add(this.txtRoleName);

           //#####250Desc###String
this.lblDesc.AutoSize = true;
this.lblDesc.Location = new System.Drawing.Point(100,50);
this.lblDesc.Name = "lblDesc";
this.lblDesc.Size = new System.Drawing.Size(41, 12);
this.lblDesc.TabIndex = 2;
this.lblDesc.Text = "描述";
this.txtDesc.Location = new System.Drawing.Point(173,46);
this.txtDesc.Name = "txtDesc";
this.txtDesc.Size = new System.Drawing.Size(100, 21);
this.txtDesc.TabIndex = 2;
this.Controls.Add(this.lblDesc);
this.Controls.Add(this.txtDesc);

           //#####RolePropertyID###Int64
//属性测试75RolePropertyID
this.lblRolePropertyID.AutoSize = true;
this.lblRolePropertyID.Location = new System.Drawing.Point(100,75);
this.lblRolePropertyID.Name = "lblRolePropertyID";
this.lblRolePropertyID.Size = new System.Drawing.Size(41, 12);
this.lblRolePropertyID.TabIndex = 3;
this.lblRolePropertyID.Text = "";
//111======75
this.cmbRolePropertyID.Location = new System.Drawing.Point(173,71);
this.cmbRolePropertyID.Name ="cmbRolePropertyID";
this.cmbRolePropertyID.Size = new System.Drawing.Size(100, 21);
this.cmbRolePropertyID.TabIndex = 3;
this.Controls.Add(this.lblRolePropertyID);
this.Controls.Add(this.cmbRolePropertyID);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRoleName );
this.Controls.Add(this.txtRoleName );

                this.Controls.Add(this.lblDesc );
this.Controls.Add(this.txtDesc );

                this.Controls.Add(this.lblRolePropertyID );
this.Controls.Add(this.cmbRolePropertyID );

                    
            this.Name = "tb_RoleInfoQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRoleName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRoleName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDesc;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDesc;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRolePropertyID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRolePropertyID;

    
    
   
 





    }
}


