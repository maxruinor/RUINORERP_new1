﻿// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:22
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 提醒内容
    /// </summary>
    partial class tb_ReminderAlertEdit
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

this.lblAlertTime = new Krypton.Toolkit.KryptonLabel();
this.dtpAlertTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblMessage = new Krypton.Toolkit.KryptonLabel();
this.txtMessage = new Krypton.Toolkit.KryptonTextBox();

    
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
this.lblRuleId.Text = "";
//111======25
this.cmbRuleId.Location = new System.Drawing.Point(173,21);
this.cmbRuleId.Name ="cmbRuleId";
this.cmbRuleId.Size = new System.Drawing.Size(100, 21);
this.cmbRuleId.TabIndex = 1;
this.Controls.Add(this.lblRuleId);
this.Controls.Add(this.cmbRuleId);

           //#####AlertTime###DateTime
this.lblAlertTime.AutoSize = true;
this.lblAlertTime.Location = new System.Drawing.Point(100,50);
this.lblAlertTime.Name = "lblAlertTime";
this.lblAlertTime.Size = new System.Drawing.Size(41, 12);
this.lblAlertTime.TabIndex = 2;
this.lblAlertTime.Text = "";
//111======50
this.dtpAlertTime.Location = new System.Drawing.Point(173,46);
this.dtpAlertTime.Name ="dtpAlertTime";
this.dtpAlertTime.ShowCheckBox =true;
this.dtpAlertTime.Size = new System.Drawing.Size(100, 21);
this.dtpAlertTime.TabIndex = 2;
this.Controls.Add(this.lblAlertTime);
this.Controls.Add(this.dtpAlertTime);

           //#####200Message###String
this.lblMessage.AutoSize = true;
this.lblMessage.Location = new System.Drawing.Point(100,75);
this.lblMessage.Name = "lblMessage";
this.lblMessage.Size = new System.Drawing.Size(41, 12);
this.lblMessage.TabIndex = 3;
this.lblMessage.Text = "";
this.txtMessage.Location = new System.Drawing.Point(173,71);
this.txtMessage.Name = "txtMessage";
this.txtMessage.Size = new System.Drawing.Size(100, 21);
this.txtMessage.TabIndex = 3;
this.Controls.Add(this.lblMessage);
this.Controls.Add(this.txtMessage);

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
           // this.kryptonPanel1.TabIndex = 3;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRuleId );
this.Controls.Add(this.cmbRuleId );

                this.Controls.Add(this.lblAlertTime );
this.Controls.Add(this.dtpAlertTime );

                this.Controls.Add(this.lblMessage );
this.Controls.Add(this.txtMessage );

                            // 
            // "tb_ReminderAlertEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ReminderAlertEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblAlertTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpAlertTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblMessage;
private Krypton.Toolkit.KryptonTextBox txtMessage;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

