// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/26/2025 12:18:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 用户接收提醒内容
    /// </summary>
    partial class tb_ReminderResultEdit
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
     this.lblRuleId = new Krypton.Toolkit.KryptonLabel();
this.cmbRuleId = new Krypton.Toolkit.KryptonComboBox();

this.lblReminderBizType = new Krypton.Toolkit.KryptonLabel();
this.txtReminderBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblTriggerTime = new Krypton.Toolkit.KryptonLabel();
this.dtpTriggerTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblMessage = new Krypton.Toolkit.KryptonLabel();
this.txtMessage = new Krypton.Toolkit.KryptonTextBox();

this.lblIsRead = new Krypton.Toolkit.KryptonLabel();
this.chkIsRead = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsRead.Values.Text ="";

this.lblReadTime = new Krypton.Toolkit.KryptonLabel();
this.dtpReadTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblJsonResult = new Krypton.Toolkit.KryptonLabel();
this.txtJsonResult = new Krypton.Toolkit.KryptonTextBox();
this.txtJsonResult.Multiline = true;

    
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
     
            //#####RuleId###Int64
//属性测试25RuleId
this.lblRuleId.AutoSize = true;
this.lblRuleId.Location = new System.Drawing.Point(100,25);
this.lblRuleId.Name = "lblRuleId";
this.lblRuleId.Size = new System.Drawing.Size(41, 12);
this.lblRuleId.TabIndex = 1;
this.lblRuleId.Text = "提醒规则";
//111======25
this.cmbRuleId.Location = new System.Drawing.Point(173,21);
this.cmbRuleId.Name ="cmbRuleId";
this.cmbRuleId.Size = new System.Drawing.Size(100, 21);
this.cmbRuleId.TabIndex = 1;
this.Controls.Add(this.lblRuleId);
this.Controls.Add(this.cmbRuleId);

           //#####ReminderBizType###Int32
//属性测试50ReminderBizType
this.lblReminderBizType.AutoSize = true;
this.lblReminderBizType.Location = new System.Drawing.Point(100,50);
this.lblReminderBizType.Name = "lblReminderBizType";
this.lblReminderBizType.Size = new System.Drawing.Size(41, 12);
this.lblReminderBizType.TabIndex = 2;
this.lblReminderBizType.Text = "提醒类型";
this.txtReminderBizType.Location = new System.Drawing.Point(173,46);
this.txtReminderBizType.Name = "txtReminderBizType";
this.txtReminderBizType.Size = new System.Drawing.Size(100, 21);
this.txtReminderBizType.TabIndex = 2;
this.Controls.Add(this.lblReminderBizType);
this.Controls.Add(this.txtReminderBizType);

           //#####TriggerTime###DateTime
this.lblTriggerTime.AutoSize = true;
this.lblTriggerTime.Location = new System.Drawing.Point(100,75);
this.lblTriggerTime.Name = "lblTriggerTime";
this.lblTriggerTime.Size = new System.Drawing.Size(41, 12);
this.lblTriggerTime.TabIndex = 3;
this.lblTriggerTime.Text = "提醒时间";
//111======75
this.dtpTriggerTime.Location = new System.Drawing.Point(173,71);
this.dtpTriggerTime.Name ="dtpTriggerTime";
this.dtpTriggerTime.Size = new System.Drawing.Size(100, 21);
this.dtpTriggerTime.TabIndex = 3;
this.Controls.Add(this.lblTriggerTime);
this.Controls.Add(this.dtpTriggerTime);

           //#####200Message###String
this.lblMessage.AutoSize = true;
this.lblMessage.Location = new System.Drawing.Point(100,100);
this.lblMessage.Name = "lblMessage";
this.lblMessage.Size = new System.Drawing.Size(41, 12);
this.lblMessage.TabIndex = 4;
this.lblMessage.Text = "提醒内容";
this.txtMessage.Location = new System.Drawing.Point(173,96);
this.txtMessage.Name = "txtMessage";
this.txtMessage.Size = new System.Drawing.Size(100, 21);
this.txtMessage.TabIndex = 4;
this.Controls.Add(this.lblMessage);
this.Controls.Add(this.txtMessage);

           //#####IsRead###Boolean
this.lblIsRead.AutoSize = true;
this.lblIsRead.Location = new System.Drawing.Point(100,125);
this.lblIsRead.Name = "lblIsRead";
this.lblIsRead.Size = new System.Drawing.Size(41, 12);
this.lblIsRead.TabIndex = 5;
this.lblIsRead.Text = "已读";
this.chkIsRead.Location = new System.Drawing.Point(173,121);
this.chkIsRead.Name = "chkIsRead";
this.chkIsRead.Size = new System.Drawing.Size(100, 21);
this.chkIsRead.TabIndex = 5;
this.Controls.Add(this.lblIsRead);
this.Controls.Add(this.chkIsRead);

           //#####ReadTime###DateTime
this.lblReadTime.AutoSize = true;
this.lblReadTime.Location = new System.Drawing.Point(100,150);
this.lblReadTime.Name = "lblReadTime";
this.lblReadTime.Size = new System.Drawing.Size(41, 12);
this.lblReadTime.TabIndex = 6;
this.lblReadTime.Text = "读取时间";
//111======150
this.dtpReadTime.Location = new System.Drawing.Point(173,146);
this.dtpReadTime.Name ="dtpReadTime";
this.dtpReadTime.ShowCheckBox =true;
this.dtpReadTime.Size = new System.Drawing.Size(100, 21);
this.dtpReadTime.TabIndex = 6;
this.Controls.Add(this.lblReadTime);
this.Controls.Add(this.dtpReadTime);

           //#####2147483647JsonResult###String
this.lblJsonResult.AutoSize = true;
this.lblJsonResult.Location = new System.Drawing.Point(100,175);
this.lblJsonResult.Name = "lblJsonResult";
this.lblJsonResult.Size = new System.Drawing.Size(41, 12);
this.lblJsonResult.TabIndex = 7;
this.lblJsonResult.Text = "扩展JSON结果";
this.txtJsonResult.Location = new System.Drawing.Point(173,171);
this.txtJsonResult.Name = "txtJsonResult";
this.txtJsonResult.Size = new System.Drawing.Size(100, 21);
this.txtJsonResult.TabIndex = 7;
this.txtJsonResult.Multiline = true;
this.Controls.Add(this.lblJsonResult);
this.Controls.Add(this.txtJsonResult);

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
           // this.kryptonPanel1.TabIndex = 7;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRuleId );
this.Controls.Add(this.cmbRuleId );

                this.Controls.Add(this.lblReminderBizType );
this.Controls.Add(this.txtReminderBizType );

                this.Controls.Add(this.lblTriggerTime );
this.Controls.Add(this.dtpTriggerTime );

                this.Controls.Add(this.lblMessage );
this.Controls.Add(this.txtMessage );

                this.Controls.Add(this.lblIsRead );
this.Controls.Add(this.chkIsRead );

                this.Controls.Add(this.lblReadTime );
this.Controls.Add(this.dtpReadTime );

                this.Controls.Add(this.lblJsonResult );
this.Controls.Add(this.txtJsonResult );

                            // 
            // "tb_ReminderResultEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ReminderResultEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblRuleId;
private Krypton.Toolkit.KryptonComboBox cmbRuleId;

    
        
              private Krypton.Toolkit.KryptonLabel lblReminderBizType;
private Krypton.Toolkit.KryptonTextBox txtReminderBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblTriggerTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpTriggerTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblMessage;
private Krypton.Toolkit.KryptonTextBox txtMessage;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsRead;
private Krypton.Toolkit.KryptonCheckBox chkIsRead;

    
        
              private Krypton.Toolkit.KryptonLabel lblReadTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpReadTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblJsonResult;
private Krypton.Toolkit.KryptonTextBox txtJsonResult;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

