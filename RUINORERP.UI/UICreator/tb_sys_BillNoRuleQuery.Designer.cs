
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
    partial class tb_sys_BillNoRuleQuery
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
     
     this.lblRuleName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRuleName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPrefix = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrefix = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblUseCheckDigit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkUseCheckDigit = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkUseCheckDigit.Values.Text ="";

this.lblRedisKeyPattern = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRedisKeyPattern = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRedisKeyPattern.Multiline = true;


this.lblIsActive = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsActive = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsActive.Values.Text ="";
this.chkIsActive.Checked = true;
this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
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

           //#####SequenceLength###Int32

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRuleName );
this.Controls.Add(this.txtRuleName );

                this.Controls.Add(this.lblPrefix );
this.Controls.Add(this.txtPrefix );

                
                
                this.Controls.Add(this.lblUseCheckDigit );
this.Controls.Add(this.chkUseCheckDigit );

                this.Controls.Add(this.lblRedisKeyPattern );
this.Controls.Add(this.txtRedisKeyPattern );

                
                this.Controls.Add(this.lblIsActive );
this.Controls.Add(this.chkIsActive );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_sys_BillNoRuleQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRuleName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRuleName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrefix;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrefix;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUseCheckDigit;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkUseCheckDigit;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRedisKeyPattern;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRedisKeyPattern;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsActive;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsActive;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


