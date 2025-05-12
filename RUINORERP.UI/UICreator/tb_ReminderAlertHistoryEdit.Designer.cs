// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理
    /// </summary>
    partial class tb_ReminderAlertHistoryEdit
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
     this.lblAlertId = new Krypton.Toolkit.KryptonLabel();
this.cmbAlertId = new Krypton.Toolkit.KryptonComboBox();

this.lblUser_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbUser_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblIsRead = new Krypton.Toolkit.KryptonLabel();
this.chkIsRead = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsRead.Values.Text ="";

this.lblMessage = new Krypton.Toolkit.KryptonLabel();
this.txtMessage = new Krypton.Toolkit.KryptonTextBox();
this.txtMessage.Multiline = true;

this.lblTriggerTime = new Krypton.Toolkit.KryptonLabel();
this.dtpTriggerTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblReminderBizType = new Krypton.Toolkit.KryptonLabel();
this.txtReminderBizType = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####AlertId###Int64
//属性测试25AlertId
this.lblAlertId.AutoSize = true;
this.lblAlertId.Location = new System.Drawing.Point(100,25);
this.lblAlertId.Name = "lblAlertId";
this.lblAlertId.Size = new System.Drawing.Size(41, 12);
this.lblAlertId.TabIndex = 1;
this.lblAlertId.Text = "";
//111======25
this.cmbAlertId.Location = new System.Drawing.Point(173,21);
this.cmbAlertId.Name ="cmbAlertId";
this.cmbAlertId.Size = new System.Drawing.Size(100, 21);
this.cmbAlertId.TabIndex = 1;
this.Controls.Add(this.lblAlertId);
this.Controls.Add(this.cmbAlertId);

           //#####User_ID###Int64
//属性测试50User_ID
//属性测试50User_ID
this.lblUser_ID.AutoSize = true;
this.lblUser_ID.Location = new System.Drawing.Point(100,50);
this.lblUser_ID.Name = "lblUser_ID";
this.lblUser_ID.Size = new System.Drawing.Size(41, 12);
this.lblUser_ID.TabIndex = 2;
this.lblUser_ID.Text = "";
//111======50
this.cmbUser_ID.Location = new System.Drawing.Point(173,46);
this.cmbUser_ID.Name ="cmbUser_ID";
this.cmbUser_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUser_ID.TabIndex = 2;
this.Controls.Add(this.lblUser_ID);
this.Controls.Add(this.cmbUser_ID);

           //#####IsRead###Boolean
this.lblIsRead.AutoSize = true;
this.lblIsRead.Location = new System.Drawing.Point(100,75);
this.lblIsRead.Name = "lblIsRead";
this.lblIsRead.Size = new System.Drawing.Size(41, 12);
this.lblIsRead.TabIndex = 3;
this.lblIsRead.Text = "";
this.chkIsRead.Location = new System.Drawing.Point(173,71);
this.chkIsRead.Name = "chkIsRead";
this.chkIsRead.Size = new System.Drawing.Size(100, 21);
this.chkIsRead.TabIndex = 3;
this.Controls.Add(this.lblIsRead);
this.Controls.Add(this.chkIsRead);

           //#####2147483647Message###String
this.lblMessage.AutoSize = true;
this.lblMessage.Location = new System.Drawing.Point(100,100);
this.lblMessage.Name = "lblMessage";
this.lblMessage.Size = new System.Drawing.Size(41, 12);
this.lblMessage.TabIndex = 4;
this.lblMessage.Text = "";
this.txtMessage.Location = new System.Drawing.Point(173,96);
this.txtMessage.Name = "txtMessage";
this.txtMessage.Size = new System.Drawing.Size(100, 21);
this.txtMessage.TabIndex = 4;
this.txtMessage.Multiline = true;
this.Controls.Add(this.lblMessage);
this.Controls.Add(this.txtMessage);

           //#####TriggerTime###DateTime
this.lblTriggerTime.AutoSize = true;
this.lblTriggerTime.Location = new System.Drawing.Point(100,125);
this.lblTriggerTime.Name = "lblTriggerTime";
this.lblTriggerTime.Size = new System.Drawing.Size(41, 12);
this.lblTriggerTime.TabIndex = 5;
this.lblTriggerTime.Text = "";
//111======125
this.dtpTriggerTime.Location = new System.Drawing.Point(173,121);
this.dtpTriggerTime.Name ="dtpTriggerTime";
this.dtpTriggerTime.Size = new System.Drawing.Size(100, 21);
this.dtpTriggerTime.TabIndex = 5;
this.Controls.Add(this.lblTriggerTime);
this.Controls.Add(this.dtpTriggerTime);

           //#####ReminderBizType###Int32
//属性测试150ReminderBizType
//属性测试150ReminderBizType
this.lblReminderBizType.AutoSize = true;
this.lblReminderBizType.Location = new System.Drawing.Point(100,150);
this.lblReminderBizType.Name = "lblReminderBizType";
this.lblReminderBizType.Size = new System.Drawing.Size(41, 12);
this.lblReminderBizType.TabIndex = 6;
this.lblReminderBizType.Text = "";
this.txtReminderBizType.Location = new System.Drawing.Point(173,146);
this.txtReminderBizType.Name = "txtReminderBizType";
this.txtReminderBizType.Size = new System.Drawing.Size(100, 21);
this.txtReminderBizType.TabIndex = 6;
this.Controls.Add(this.lblReminderBizType);
this.Controls.Add(this.txtReminderBizType);

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
           // this.kryptonPanel1.TabIndex = 6;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblAlertId );
this.Controls.Add(this.cmbAlertId );

                this.Controls.Add(this.lblUser_ID );
this.Controls.Add(this.cmbUser_ID );

                this.Controls.Add(this.lblIsRead );
this.Controls.Add(this.chkIsRead );

                this.Controls.Add(this.lblMessage );
this.Controls.Add(this.txtMessage );

                this.Controls.Add(this.lblTriggerTime );
this.Controls.Add(this.dtpTriggerTime );

                this.Controls.Add(this.lblReminderBizType );
this.Controls.Add(this.txtReminderBizType );

                            // 
            // "tb_ReminderAlertHistoryEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ReminderAlertHistoryEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblAlertId;
private Krypton.Toolkit.KryptonComboBox cmbAlertId;

    
        
              private Krypton.Toolkit.KryptonLabel lblUser_ID;
private Krypton.Toolkit.KryptonComboBox cmbUser_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsRead;
private Krypton.Toolkit.KryptonCheckBox chkIsRead;

    
        
              private Krypton.Toolkit.KryptonLabel lblMessage;
private Krypton.Toolkit.KryptonTextBox txtMessage;

    
        
              private Krypton.Toolkit.KryptonLabel lblTriggerTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpTriggerTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblReminderBizType;
private Krypton.Toolkit.KryptonTextBox txtReminderBizType;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

