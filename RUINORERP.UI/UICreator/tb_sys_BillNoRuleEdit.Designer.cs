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
            this.lblRedisKeyPattern = new Krypton.Toolkit.KryptonLabel();
            this.txtRedisKeyPattern = new Krypton.Toolkit.KryptonTextBox();
            this.lblResetMode = new Krypton.Toolkit.KryptonLabel();
            this.txtResetMode = new Krypton.Toolkit.KryptonTextBox();
            this.SuspendLayout();
            // 
            // lblRuleName
            // 
            this.lblRuleName.Location = new System.Drawing.Point(100, 25);
            this.lblRuleName.Name = "lblRuleName";
            this.lblRuleName.Size = new System.Drawing.Size(62, 20);
            this.lblRuleName.TabIndex = 1;
            this.lblRuleName.Values.Text = "规则名称";
            // 
            // txtRuleName
            // 
            this.txtRuleName.Location = new System.Drawing.Point(259, 25);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.Size = new System.Drawing.Size(100, 23);
            this.txtRuleName.TabIndex = 1;
            // 
            // lblPrefix
            // 
            this.lblPrefix.Location = new System.Drawing.Point(100, 50);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(36, 20);
            this.lblPrefix.TabIndex = 2;
            this.lblPrefix.Values.Text = "前缀";
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(259, 50);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(100, 23);
            this.txtPrefix.TabIndex = 2;
            // 
            // lblDateFormat
            // 
            this.lblDateFormat.Location = new System.Drawing.Point(100, 75);
            this.lblDateFormat.Name = "lblDateFormat";
            this.lblDateFormat.Size = new System.Drawing.Size(62, 20);
            this.lblDateFormat.TabIndex = 3;
            this.lblDateFormat.Values.Text = "日期格式";
            // 
            // txtDateFormat
            // 
            this.txtDateFormat.Location = new System.Drawing.Point(259, 75);
            this.txtDateFormat.Name = "txtDateFormat";
            this.txtDateFormat.Size = new System.Drawing.Size(100, 23);
            this.txtDateFormat.TabIndex = 3;
            // 
            // lblSequenceLength
            // 
            this.lblSequenceLength.Location = new System.Drawing.Point(100, 100);
            this.lblSequenceLength.Name = "lblSequenceLength";
            this.lblSequenceLength.Size = new System.Drawing.Size(75, 20);
            this.lblSequenceLength.TabIndex = 4;
            this.lblSequenceLength.Values.Text = "流水号长度";
            // 
            // txtSequenceLength
            // 
            this.txtSequenceLength.Location = new System.Drawing.Point(259, 100);
            this.txtSequenceLength.Name = "txtSequenceLength";
            this.txtSequenceLength.Size = new System.Drawing.Size(100, 23);
            this.txtSequenceLength.TabIndex = 4;
            // 
            // lblUseCheckDigit
            // 
            this.lblUseCheckDigit.Location = new System.Drawing.Point(74, 124);
            this.lblUseCheckDigit.Name = "lblUseCheckDigit";
            this.lblUseCheckDigit.Size = new System.Drawing.Size(101, 20);
            this.lblUseCheckDigit.TabIndex = 5;
            this.lblUseCheckDigit.Values.Text = "是否使用校验位";
            // 
            // chkUseCheckDigit
            // 
            this.chkUseCheckDigit.Location = new System.Drawing.Point(204, 124);
            this.chkUseCheckDigit.Name = "chkUseCheckDigit";
            this.chkUseCheckDigit.Size = new System.Drawing.Size(19, 13);
            this.chkUseCheckDigit.TabIndex = 5;
            this.chkUseCheckDigit.Values.Text = "";
            // 
            // lblRedisKeyPattern
            // 
            this.lblRedisKeyPattern.Location = new System.Drawing.Point(100, 150);
            this.lblRedisKeyPattern.Name = "lblRedisKeyPattern";
            this.lblRedisKeyPattern.Size = new System.Drawing.Size(79, 20);
            this.lblRedisKeyPattern.TabIndex = 6;
            this.lblRedisKeyPattern.Values.Text = "Redis键模式";
            // 
            // txtRedisKeyPattern
            // 
            this.txtRedisKeyPattern.Location = new System.Drawing.Point(259, 150);
            this.txtRedisKeyPattern.Multiline = true;
            this.txtRedisKeyPattern.Name = "txtRedisKeyPattern";
            this.txtRedisKeyPattern.Size = new System.Drawing.Size(100, 21);
            this.txtRedisKeyPattern.TabIndex = 6;
            // 
            // lblResetMode
            // 
            this.lblResetMode.Location = new System.Drawing.Point(100, 175);
            this.lblResetMode.Name = "lblResetMode";
            this.lblResetMode.Size = new System.Drawing.Size(62, 20);
            this.lblResetMode.TabIndex = 7;
            this.lblResetMode.Values.Text = "重置模式";
            // 
            // txtResetMode
            // 
            this.txtResetMode.Location = new System.Drawing.Point(259, 175);
            this.txtResetMode.Name = "txtResetMode";
            this.txtResetMode.Size = new System.Drawing.Size(100, 23);
            this.txtResetMode.TabIndex = 7;
            // 
            // tb_sys_BillNoRuleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblRuleName);
            this.Controls.Add(this.txtRuleName);
            this.Controls.Add(this.lblPrefix);
            this.Controls.Add(this.txtPrefix);
            this.Controls.Add(this.lblDateFormat);
            this.Controls.Add(this.txtDateFormat);
            this.Controls.Add(this.lblSequenceLength);
            this.Controls.Add(this.txtSequenceLength);
            this.Controls.Add(this.lblUseCheckDigit);
            this.Controls.Add(this.chkUseCheckDigit);
            this.Controls.Add(this.lblRedisKeyPattern);
            this.Controls.Add(this.txtRedisKeyPattern);
            this.Controls.Add(this.lblResetMode);
            this.Controls.Add(this.txtResetMode);
            this.Name = "tb_sys_BillNoRuleEdit";
            this.Size = new System.Drawing.Size(911, 490);
            this.Load += new System.EventHandler(this.tb_sys_BillNoRuleEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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







    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

