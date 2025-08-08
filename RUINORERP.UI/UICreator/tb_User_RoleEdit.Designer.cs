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
    partial class tb_User_RoleEdit
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
     this.lblUser_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbUser_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblRoleID = new Krypton.Toolkit.KryptonLabel();
this.cmbRoleID = new Krypton.Toolkit.KryptonComboBox();

this.lblAuthorized = new Krypton.Toolkit.KryptonLabel();
this.chkAuthorized = new Krypton.Toolkit.KryptonCheckBox();
this.chkAuthorized.Values.Text ="";

this.lblDefaultRole = new Krypton.Toolkit.KryptonLabel();
this.chkDefaultRole = new Krypton.Toolkit.KryptonCheckBox();
this.chkDefaultRole.Values.Text ="";

    
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
           // this.kryptonPanel1.TabIndex = 4;

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

                            // 
            // "tb_User_RoleEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_User_RoleEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblUser_ID;
private Krypton.Toolkit.KryptonComboBox cmbUser_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRoleID;
private Krypton.Toolkit.KryptonComboBox cmbRoleID;

    
        
              private Krypton.Toolkit.KryptonLabel lblAuthorized;
private Krypton.Toolkit.KryptonCheckBox chkAuthorized;

    
        
              private Krypton.Toolkit.KryptonLabel lblDefaultRole;
private Krypton.Toolkit.KryptonCheckBox chkDefaultRole;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

