// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:22
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 业务编号规则
    /// </summary>
    partial class tb_sys_BillNoRuleEdit
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
     this.lblRuleName = new Krypton.Toolkit.KryptonLabel();
this.txtRuleName = new Krypton.Toolkit.KryptonTextBox();

this.lblPrefix = new Krypton.Toolkit.KryptonLabel();
this.txtPrefix = new Krypton.Toolkit.KryptonTextBox();

this.lblDateFormat = new Krypton.Toolkit.KryptonLabel();
this.txtDateFormat = new Krypton.Toolkit.KryptonTextBox();

this.lblSequenceLength = new Krypton.Toolkit.KryptonLabel();
this.txtSequenceLength = new Krypton.Toolkit.KryptonTextBox();

this.lblUseCheckDigit = new Krypton.Toolkit.KryptonLabel();
this.chkUseCheckDigit = new Krypton.Toolkit.KryptonCheckBox();
this.chkUseCheckDigit.Values.Text ="";

this.lblRedisKeyPattern = new Krypton.Toolkit.KryptonLabel();
this.txtRedisKeyPattern = new Krypton.Toolkit.KryptonTextBox();
this.txtRedisKeyPattern.Multiline = true;

this.lblResetMode = new Krypton.Toolkit.KryptonLabel();
this.txtResetMode = new Krypton.Toolkit.KryptonTextBox();

this.lblIsActive = new Krypton.Toolkit.KryptonLabel();
this.chkIsActive = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsActive.Values.Text ="";
this.chkIsActive.Checked = true;
this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblDescription = new Krypton.Toolkit.KryptonLabel();
this.txtDescription = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####200RuleName###String
this.lblRuleName.AutoSize = true;
this.lblRuleName.Location = new System.Drawing.Point(100,25);
this.lblRuleName.Name = "lblRuleName";
this.lblRuleName.Size = new System.Drawing.Size(41, 12);
this.lblRuleName.TabIndex = 1;
this.lblRuleName.Text = "规则名称";
this.txtRuleName.Location = new System.Drawing.Point(173,21);
this.txtRuleName.Name = "txtRuleName";
this.txtRuleName.Size = new System.Drawing.Size(100, 21);
this.txtRuleName.TabIndex = 1;
this.Controls.Add(this.lblRuleName);
this.Controls.Add(this.txtRuleName);

           //#####200Prefix###String
this.lblPrefix.AutoSize = true;
this.lblPrefix.Location = new System.Drawing.Point(100,50);
this.lblPrefix.Name = "lblPrefix";
this.lblPrefix.Size = new System.Drawing.Size(41, 12);
this.lblPrefix.TabIndex = 2;
this.lblPrefix.Text = "前缀";
this.txtPrefix.Location = new System.Drawing.Point(173,46);
this.txtPrefix.Name = "txtPrefix";
this.txtPrefix.Size = new System.Drawing.Size(100, 21);
this.txtPrefix.TabIndex = 2;
this.Controls.Add(this.lblPrefix);
this.Controls.Add(this.txtPrefix);

           //#####DateFormat###Int32
this.lblDateFormat.AutoSize = true;
this.lblDateFormat.Location = new System.Drawing.Point(100,75);
this.lblDateFormat.Name = "lblDateFormat";
this.lblDateFormat.Size = new System.Drawing.Size(41, 12);
this.lblDateFormat.TabIndex = 3;
this.lblDateFormat.Text = "日期格式";
this.txtDateFormat.Location = new System.Drawing.Point(173,71);
this.txtDateFormat.Name = "txtDateFormat";
this.txtDateFormat.Size = new System.Drawing.Size(100, 21);
this.txtDateFormat.TabIndex = 3;
this.Controls.Add(this.lblDateFormat);
this.Controls.Add(this.txtDateFormat);

           //#####SequenceLength###Int32
this.lblSequenceLength.AutoSize = true;
this.lblSequenceLength.Location = new System.Drawing.Point(100,100);
this.lblSequenceLength.Name = "lblSequenceLength";
this.lblSequenceLength.Size = new System.Drawing.Size(41, 12);
this.lblSequenceLength.TabIndex = 4;
this.lblSequenceLength.Text = "流水号长度";
this.txtSequenceLength.Location = new System.Drawing.Point(173,96);
this.txtSequenceLength.Name = "txtSequenceLength";
this.txtSequenceLength.Size = new System.Drawing.Size(100, 21);
this.txtSequenceLength.TabIndex = 4;
this.Controls.Add(this.lblSequenceLength);
this.Controls.Add(this.txtSequenceLength);

           //#####UseCheckDigit###Boolean
this.lblUseCheckDigit.AutoSize = true;
this.lblUseCheckDigit.Location = new System.Drawing.Point(100,125);
this.lblUseCheckDigit.Name = "lblUseCheckDigit";
this.lblUseCheckDigit.Size = new System.Drawing.Size(41, 12);
this.lblUseCheckDigit.TabIndex = 5;
this.lblUseCheckDigit.Text = "是否使用校验位";
this.chkUseCheckDigit.Location = new System.Drawing.Point(173,121);
this.chkUseCheckDigit.Name = "chkUseCheckDigit";
this.chkUseCheckDigit.Size = new System.Drawing.Size(100, 21);
this.chkUseCheckDigit.TabIndex = 5;
this.Controls.Add(this.lblUseCheckDigit);
this.Controls.Add(this.chkUseCheckDigit);

           //#####3000RedisKeyPattern###String
this.lblRedisKeyPattern.AutoSize = true;
this.lblRedisKeyPattern.Location = new System.Drawing.Point(100,150);
this.lblRedisKeyPattern.Name = "lblRedisKeyPattern";
this.lblRedisKeyPattern.Size = new System.Drawing.Size(41, 12);
this.lblRedisKeyPattern.TabIndex = 6;
this.lblRedisKeyPattern.Text = "Redis键模式";
this.txtRedisKeyPattern.Location = new System.Drawing.Point(173,146);
this.txtRedisKeyPattern.Name = "txtRedisKeyPattern";
this.txtRedisKeyPattern.Size = new System.Drawing.Size(100, 21);
this.txtRedisKeyPattern.TabIndex = 6;
this.Controls.Add(this.lblRedisKeyPattern);
this.Controls.Add(this.txtRedisKeyPattern);

           //#####ResetMode###Int32
this.lblResetMode.AutoSize = true;
this.lblResetMode.Location = new System.Drawing.Point(100,175);
this.lblResetMode.Name = "lblResetMode";
this.lblResetMode.Size = new System.Drawing.Size(41, 12);
this.lblResetMode.TabIndex = 7;
this.lblResetMode.Text = "重置模式";
this.txtResetMode.Location = new System.Drawing.Point(173,171);
this.txtResetMode.Name = "txtResetMode";
this.txtResetMode.Size = new System.Drawing.Size(100, 21);
this.txtResetMode.TabIndex = 7;
this.Controls.Add(this.lblResetMode);
this.Controls.Add(this.txtResetMode);

           //#####IsActive###Boolean
this.lblIsActive.AutoSize = true;
this.lblIsActive.Location = new System.Drawing.Point(100,200);
this.lblIsActive.Name = "lblIsActive";
this.lblIsActive.Size = new System.Drawing.Size(41, 12);
this.lblIsActive.TabIndex = 8;
this.lblIsActive.Text = "是否启用";
this.chkIsActive.Location = new System.Drawing.Point(173,196);
this.chkIsActive.Name = "chkIsActive";
this.chkIsActive.Size = new System.Drawing.Size(100, 21);
this.chkIsActive.TabIndex = 8;
this.Controls.Add(this.lblIsActive);
this.Controls.Add(this.chkIsActive);

           //#####200Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,225);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 9;
this.lblDescription.Text = "规则描述";
this.txtDescription.Location = new System.Drawing.Point(173,221);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 9;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

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
                this.Controls.Add(this.lblRuleName );
this.Controls.Add(this.txtRuleName );

                this.Controls.Add(this.lblPrefix );
this.Controls.Add(this.txtPrefix );

                this.Controls.Add(this.lblDateFormat );
this.Controls.Add(this.txtDateFormat );

                this.Controls.Add(this.lblSequenceLength );
this.Controls.Add(this.txtSequenceLength );

                this.Controls.Add(this.lblUseCheckDigit );
this.Controls.Add(this.chkUseCheckDigit );

                this.Controls.Add(this.lblRedisKeyPattern );
this.Controls.Add(this.txtRedisKeyPattern );

                this.Controls.Add(this.lblResetMode );
this.Controls.Add(this.txtResetMode );

                this.Controls.Add(this.lblIsActive );
this.Controls.Add(this.chkIsActive );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_sys_BillNoRuleEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_sys_BillNoRuleEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblRuleName;
private Krypton.Toolkit.KryptonTextBox txtRuleName;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrefix;
private Krypton.Toolkit.KryptonTextBox txtPrefix;

    
        
              private Krypton.Toolkit.KryptonLabel lblDateFormat;
private Krypton.Toolkit.KryptonTextBox txtDateFormat;

    
        
              private Krypton.Toolkit.KryptonLabel lblSequenceLength;
private Krypton.Toolkit.KryptonTextBox txtSequenceLength;

    
        
              private Krypton.Toolkit.KryptonLabel lblUseCheckDigit;
private Krypton.Toolkit.KryptonCheckBox chkUseCheckDigit;

    
        
              private Krypton.Toolkit.KryptonLabel lblRedisKeyPattern;
private Krypton.Toolkit.KryptonTextBox txtRedisKeyPattern;

    
        
              private Krypton.Toolkit.KryptonLabel lblResetMode;
private Krypton.Toolkit.KryptonTextBox txtResetMode;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsActive;
private Krypton.Toolkit.KryptonCheckBox chkIsActive;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
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

