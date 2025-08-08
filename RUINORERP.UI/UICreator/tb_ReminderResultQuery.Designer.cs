
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
    /// 用户接收提醒内容
    /// </summary>
    partial class tb_ReminderResultQuery
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
     
     this.lblRuleId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRuleId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblTriggerTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpTriggerTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblMessage = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMessage = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsRead = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsRead = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsRead.Values.Text ="";

this.lblReadTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpReadTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblJsonResult = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtJsonResult = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtJsonResult.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRuleId );
this.Controls.Add(this.cmbRuleId );

                
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

                    
            this.Name = "tb_ReminderResultQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRuleId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRuleId;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTriggerTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpTriggerTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMessage;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMessage;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsRead;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsRead;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReadTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpReadTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblJsonResult;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtJsonResult;

    
    
   
 





    }
}


