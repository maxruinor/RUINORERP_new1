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
    /// 用户表
    /// </summary>
    partial class tb_UserInfoEdit
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

this.lblUserName = new Krypton.Toolkit.KryptonLabel();
this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
this.txtUserName.Multiline = true;

this.lblPassword = new Krypton.Toolkit.KryptonLabel();
this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
this.txtPassword.Multiline = true;

this.lblis_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkis_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkis_enabled.Values.Text ="";
this.chkis_enabled.Checked = true;
this.chkis_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblis_available = new Krypton.Toolkit.KryptonLabel();
this.chkis_available = new Krypton.Toolkit.KryptonCheckBox();
this.chkis_available.Values.Text ="";
this.chkis_available.Checked = true;
this.chkis_available.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblIsSuperUser = new Krypton.Toolkit.KryptonLabel();
this.chkIsSuperUser = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsSuperUser.Values.Text ="";

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

this.lblLastlogin_at = new Krypton.Toolkit.KryptonLabel();
this.dtpLastlogin_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblLastlogout_at = new Krypton.Toolkit.KryptonLabel();
this.dtpLastlogout_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

    
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
this.lblEmployee_ID.Text = "员工信息";
//111======25
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,21);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 1;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####255UserName###String
this.lblUserName.AutoSize = true;
this.lblUserName.Location = new System.Drawing.Point(100,50);
this.lblUserName.Name = "lblUserName";
this.lblUserName.Size = new System.Drawing.Size(41, 12);
this.lblUserName.TabIndex = 2;
this.lblUserName.Text = "用户名";
this.txtUserName.Location = new System.Drawing.Point(173,46);
this.txtUserName.Name = "txtUserName";
this.txtUserName.Size = new System.Drawing.Size(100, 21);
this.txtUserName.TabIndex = 2;
this.Controls.Add(this.lblUserName);
this.Controls.Add(this.txtUserName);

           //#####255Password###String
this.lblPassword.AutoSize = true;
this.lblPassword.Location = new System.Drawing.Point(100,75);
this.lblPassword.Name = "lblPassword";
this.lblPassword.Size = new System.Drawing.Size(41, 12);
this.lblPassword.TabIndex = 3;
this.lblPassword.Text = "密码";
this.txtPassword.Location = new System.Drawing.Point(173,71);
this.txtPassword.Name = "txtPassword";
this.txtPassword.Size = new System.Drawing.Size(100, 21);
this.txtPassword.TabIndex = 3;
this.Controls.Add(this.lblPassword);
this.Controls.Add(this.txtPassword);

           //#####is_enabled###Boolean
this.lblis_enabled.AutoSize = true;
this.lblis_enabled.Location = new System.Drawing.Point(100,100);
this.lblis_enabled.Name = "lblis_enabled";
this.lblis_enabled.Size = new System.Drawing.Size(41, 12);
this.lblis_enabled.TabIndex = 4;
this.lblis_enabled.Text = "是否启用";
this.chkis_enabled.Location = new System.Drawing.Point(173,96);
this.chkis_enabled.Name = "chkis_enabled";
this.chkis_enabled.Size = new System.Drawing.Size(100, 21);
this.chkis_enabled.TabIndex = 4;
this.Controls.Add(this.lblis_enabled);
this.Controls.Add(this.chkis_enabled);

           //#####is_available###Boolean
this.lblis_available.AutoSize = true;
this.lblis_available.Location = new System.Drawing.Point(100,125);
this.lblis_available.Name = "lblis_available";
this.lblis_available.Size = new System.Drawing.Size(41, 12);
this.lblis_available.TabIndex = 5;
this.lblis_available.Text = "是否可用";
this.chkis_available.Location = new System.Drawing.Point(173,121);
this.chkis_available.Name = "chkis_available";
this.chkis_available.Size = new System.Drawing.Size(100, 21);
this.chkis_available.TabIndex = 5;
this.Controls.Add(this.lblis_available);
this.Controls.Add(this.chkis_available);

           //#####IsSuperUser###Boolean
this.lblIsSuperUser.AutoSize = true;
this.lblIsSuperUser.Location = new System.Drawing.Point(100,150);
this.lblIsSuperUser.Name = "lblIsSuperUser";
this.lblIsSuperUser.Size = new System.Drawing.Size(41, 12);
this.lblIsSuperUser.TabIndex = 6;
this.lblIsSuperUser.Text = "超级用户";
this.chkIsSuperUser.Location = new System.Drawing.Point(173,146);
this.chkIsSuperUser.Name = "chkIsSuperUser";
this.chkIsSuperUser.Size = new System.Drawing.Size(100, 21);
this.chkIsSuperUser.TabIndex = 6;
this.Controls.Add(this.lblIsSuperUser);
this.Controls.Add(this.chkIsSuperUser);

           //#####100Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,175);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 7;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,171);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 7;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Lastlogin_at###DateTime
this.lblLastlogin_at.AutoSize = true;
this.lblLastlogin_at.Location = new System.Drawing.Point(100,200);
this.lblLastlogin_at.Name = "lblLastlogin_at";
this.lblLastlogin_at.Size = new System.Drawing.Size(41, 12);
this.lblLastlogin_at.TabIndex = 8;
this.lblLastlogin_at.Text = "最后登陆时间";
//111======200
this.dtpLastlogin_at.Location = new System.Drawing.Point(173,196);
this.dtpLastlogin_at.Name ="dtpLastlogin_at";
this.dtpLastlogin_at.ShowCheckBox =true;
this.dtpLastlogin_at.Size = new System.Drawing.Size(100, 21);
this.dtpLastlogin_at.TabIndex = 8;
this.Controls.Add(this.lblLastlogin_at);
this.Controls.Add(this.dtpLastlogin_at);

           //#####Lastlogout_at###DateTime
this.lblLastlogout_at.AutoSize = true;
this.lblLastlogout_at.Location = new System.Drawing.Point(100,225);
this.lblLastlogout_at.Name = "lblLastlogout_at";
this.lblLastlogout_at.Size = new System.Drawing.Size(41, 12);
this.lblLastlogout_at.TabIndex = 9;
this.lblLastlogout_at.Text = "最后登出时间";
//111======225
this.dtpLastlogout_at.Location = new System.Drawing.Point(173,221);
this.dtpLastlogout_at.Name ="dtpLastlogout_at";
this.dtpLastlogout_at.ShowCheckBox =true;
this.dtpLastlogout_at.Size = new System.Drawing.Size(100, 21);
this.dtpLastlogout_at.TabIndex = 9;
this.Controls.Add(this.lblLastlogout_at);
this.Controls.Add(this.dtpLastlogout_at);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,250);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 10;
this.lblCreated_at.Text = "创建时间";
//111======250
this.dtpCreated_at.Location = new System.Drawing.Point(173,246);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 10;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试275Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,275);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 11;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,271);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 11;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,300);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 12;
this.lblModified_at.Text = "修改时间";
//111======300
this.dtpModified_at.Location = new System.Drawing.Point(173,296);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 12;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试325Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,325);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 13;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,321);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 13;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
           // this.kryptonPanel1.TabIndex = 13;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblUserName );
this.Controls.Add(this.txtUserName );

                this.Controls.Add(this.lblPassword );
this.Controls.Add(this.txtPassword );

                this.Controls.Add(this.lblis_enabled );
this.Controls.Add(this.chkis_enabled );

                this.Controls.Add(this.lblis_available );
this.Controls.Add(this.chkis_available );

                this.Controls.Add(this.lblIsSuperUser );
this.Controls.Add(this.chkIsSuperUser );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblLastlogin_at );
this.Controls.Add(this.dtpLastlogin_at );

                this.Controls.Add(this.lblLastlogout_at );
this.Controls.Add(this.dtpLastlogout_at );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_UserInfoEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_UserInfoEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblUserName;
private Krypton.Toolkit.KryptonTextBox txtUserName;

    
        
              private Krypton.Toolkit.KryptonLabel lblPassword;
private Krypton.Toolkit.KryptonTextBox txtPassword;

    
        
              private Krypton.Toolkit.KryptonLabel lblis_enabled;
private Krypton.Toolkit.KryptonCheckBox chkis_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblis_available;
private Krypton.Toolkit.KryptonCheckBox chkis_available;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsSuperUser;
private Krypton.Toolkit.KryptonCheckBox chkIsSuperUser;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblLastlogin_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpLastlogin_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblLastlogout_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpLastlogout_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

