
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
    partial class tb_ReminderAlertHistoryQuery
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
     
     this.lblAlertId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbAlertId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblUser_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUser_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblIsRead = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsRead = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsRead.Values.Text ="";

this.lblMessage = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMessage = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtMessage.Multiline = true;

this.lblTriggerTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpTriggerTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                
                    
            this.Name = "tb_ReminderAlertHistoryQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAlertId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbAlertId;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUser_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUser_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsRead;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsRead;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMessage;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMessage;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTriggerTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpTriggerTime;

    
        
              
    
    
   
 





    }
}


