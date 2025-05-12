
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
    /// 库存策略通过这里设置的条件查询出一个库存集合提醒给用户
    /// </summary>
    partial class tb_ReminderRuleQuery
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

this.lblDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDescription.Multiline = true;



this.lblIsEnabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsEnabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsEnabled.Values.Text ="";

this.lblNotifyChannels = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotifyChannels = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEffectiveDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEffectiveDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblExpireDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpExpireDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCondition = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCondition = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCondition.Multiline = true;

this.lblNotifyRecipients = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotifyRecipients = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotifyRecipients.Multiline = true;

this.lblNotifyMessage = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotifyMessage = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotifyMessage.Multiline = true;

this.lblJsonConfig = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtJsonConfig = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtJsonConfig.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####100RuleName###String
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

           //#####500Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,50);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 2;
this.lblDescription.Text = "规则 描述";
this.txtDescription.Location = new System.Drawing.Point(173,46);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 2;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####ReminderBizType###Int32

           //#####Priority###Int32

           //#####IsEnabled###Boolean
this.lblIsEnabled.AutoSize = true;
this.lblIsEnabled.Location = new System.Drawing.Point(100,125);
this.lblIsEnabled.Name = "lblIsEnabled";
this.lblIsEnabled.Size = new System.Drawing.Size(41, 12);
this.lblIsEnabled.TabIndex = 5;
this.lblIsEnabled.Text = "是否启用";
this.chkIsEnabled.Location = new System.Drawing.Point(173,121);
this.chkIsEnabled.Name = "chkIsEnabled";
this.chkIsEnabled.Size = new System.Drawing.Size(100, 21);
this.chkIsEnabled.TabIndex = 5;
this.Controls.Add(this.lblIsEnabled);
this.Controls.Add(this.chkIsEnabled);

           //#####50NotifyChannels###String
this.lblNotifyChannels.AutoSize = true;
this.lblNotifyChannels.Location = new System.Drawing.Point(100,150);
this.lblNotifyChannels.Name = "lblNotifyChannels";
this.lblNotifyChannels.Size = new System.Drawing.Size(41, 12);
this.lblNotifyChannels.TabIndex = 6;
this.lblNotifyChannels.Text = "通知渠道";
this.txtNotifyChannels.Location = new System.Drawing.Point(173,146);
this.txtNotifyChannels.Name = "txtNotifyChannels";
this.txtNotifyChannels.Size = new System.Drawing.Size(100, 21);
this.txtNotifyChannels.TabIndex = 6;
this.Controls.Add(this.lblNotifyChannels);
this.Controls.Add(this.txtNotifyChannels);

           //#####EffectiveDate###DateTime
this.lblEffectiveDate.AutoSize = true;
this.lblEffectiveDate.Location = new System.Drawing.Point(100,175);
this.lblEffectiveDate.Name = "lblEffectiveDate";
this.lblEffectiveDate.Size = new System.Drawing.Size(41, 12);
this.lblEffectiveDate.TabIndex = 7;
this.lblEffectiveDate.Text = "生效日期";
//111======175
this.dtpEffectiveDate.Location = new System.Drawing.Point(173,171);
this.dtpEffectiveDate.Name ="dtpEffectiveDate";
this.dtpEffectiveDate.Size = new System.Drawing.Size(100, 21);
this.dtpEffectiveDate.TabIndex = 7;
this.Controls.Add(this.lblEffectiveDate);
this.Controls.Add(this.dtpEffectiveDate);

           //#####ExpireDate###DateTime
this.lblExpireDate.AutoSize = true;
this.lblExpireDate.Location = new System.Drawing.Point(100,200);
this.lblExpireDate.Name = "lblExpireDate";
this.lblExpireDate.Size = new System.Drawing.Size(41, 12);
this.lblExpireDate.TabIndex = 8;
this.lblExpireDate.Text = "过期时间";
//111======200
this.dtpExpireDate.Location = new System.Drawing.Point(173,196);
this.dtpExpireDate.Name ="dtpExpireDate";
this.dtpExpireDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpireDate.TabIndex = 8;
this.Controls.Add(this.lblExpireDate);
this.Controls.Add(this.dtpExpireDate);

           //#####2147483647Condition###String
this.lblCondition.AutoSize = true;
this.lblCondition.Location = new System.Drawing.Point(100,225);
this.lblCondition.Name = "lblCondition";
this.lblCondition.Size = new System.Drawing.Size(41, 12);
this.lblCondition.TabIndex = 9;
this.lblCondition.Text = "规则条件";
this.txtCondition.Location = new System.Drawing.Point(173,221);
this.txtCondition.Name = "txtCondition";
this.txtCondition.Size = new System.Drawing.Size(100, 21);
this.txtCondition.TabIndex = 9;
this.txtCondition.Multiline = true;
this.Controls.Add(this.lblCondition);
this.Controls.Add(this.txtCondition);

           //#####2147483647NotifyRecipients###String
this.lblNotifyRecipients.AutoSize = true;
this.lblNotifyRecipients.Location = new System.Drawing.Point(100,250);
this.lblNotifyRecipients.Name = "lblNotifyRecipients";
this.lblNotifyRecipients.Size = new System.Drawing.Size(41, 12);
this.lblNotifyRecipients.TabIndex = 10;
this.lblNotifyRecipients.Text = "知接收人";
this.txtNotifyRecipients.Location = new System.Drawing.Point(173,246);
this.txtNotifyRecipients.Name = "txtNotifyRecipients";
this.txtNotifyRecipients.Size = new System.Drawing.Size(100, 21);
this.txtNotifyRecipients.TabIndex = 10;
this.txtNotifyRecipients.Multiline = true;
this.Controls.Add(this.lblNotifyRecipients);
this.Controls.Add(this.txtNotifyRecipients);

           //#####2147483647NotifyMessage###String
this.lblNotifyMessage.AutoSize = true;
this.lblNotifyMessage.Location = new System.Drawing.Point(100,275);
this.lblNotifyMessage.Name = "lblNotifyMessage";
this.lblNotifyMessage.Size = new System.Drawing.Size(41, 12);
this.lblNotifyMessage.TabIndex = 11;
this.lblNotifyMessage.Text = "通知消息模板";
this.txtNotifyMessage.Location = new System.Drawing.Point(173,271);
this.txtNotifyMessage.Name = "txtNotifyMessage";
this.txtNotifyMessage.Size = new System.Drawing.Size(100, 21);
this.txtNotifyMessage.TabIndex = 11;
this.txtNotifyMessage.Multiline = true;
this.Controls.Add(this.lblNotifyMessage);
this.Controls.Add(this.txtNotifyMessage);

           //#####2147483647JsonConfig###String
this.lblJsonConfig.AutoSize = true;
this.lblJsonConfig.Location = new System.Drawing.Point(100,300);
this.lblJsonConfig.Name = "lblJsonConfig";
this.lblJsonConfig.Size = new System.Drawing.Size(41, 12);
this.lblJsonConfig.TabIndex = 12;
this.lblJsonConfig.Text = "扩展JSON配置";
this.txtJsonConfig.Location = new System.Drawing.Point(173,296);
this.txtJsonConfig.Name = "txtJsonConfig";
this.txtJsonConfig.Size = new System.Drawing.Size(100, 21);
this.txtJsonConfig.TabIndex = 12;
this.txtJsonConfig.Multiline = true;
this.Controls.Add(this.lblJsonConfig);
this.Controls.Add(this.txtJsonConfig);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,325);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 13;
this.lblCreated_at.Text = "创建时间";
//111======325
this.dtpCreated_at.Location = new System.Drawing.Point(173,321);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 13;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,375);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 15;
this.lblModified_at.Text = "修改时间";
//111======375
this.dtpModified_at.Location = new System.Drawing.Point(173,371);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 15;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRuleName );
this.Controls.Add(this.txtRuleName );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                
                
                this.Controls.Add(this.lblIsEnabled );
this.Controls.Add(this.chkIsEnabled );

                this.Controls.Add(this.lblNotifyChannels );
this.Controls.Add(this.txtNotifyChannels );

                this.Controls.Add(this.lblEffectiveDate );
this.Controls.Add(this.dtpEffectiveDate );

                this.Controls.Add(this.lblExpireDate );
this.Controls.Add(this.dtpExpireDate );

                this.Controls.Add(this.lblCondition );
this.Controls.Add(this.txtCondition );

                this.Controls.Add(this.lblNotifyRecipients );
this.Controls.Add(this.txtNotifyRecipients );

                this.Controls.Add(this.lblNotifyMessage );
this.Controls.Add(this.txtNotifyMessage );

                this.Controls.Add(this.lblJsonConfig );
this.Controls.Add(this.txtJsonConfig );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_ReminderRuleQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRuleName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRuleName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsEnabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsEnabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotifyChannels;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotifyChannels;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEffectiveDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpireDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpExpireDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCondition;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCondition;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotifyRecipients;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotifyRecipients;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotifyMessage;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotifyMessage;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblJsonConfig;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtJsonConfig;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


