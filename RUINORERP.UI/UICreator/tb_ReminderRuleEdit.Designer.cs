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
    /// 提醒规则
    /// </summary>
    partial class tb_ReminderRuleEdit
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

this.lblRuleEngineType = new Krypton.Toolkit.KryptonLabel();
this.txtRuleEngineType = new Krypton.Toolkit.KryptonTextBox();

this.lblDescription = new Krypton.Toolkit.KryptonLabel();
this.txtDescription = new Krypton.Toolkit.KryptonTextBox();
this.txtDescription.Multiline = true;

this.lblReminderBizType = new Krypton.Toolkit.KryptonLabel();
this.txtReminderBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckIntervalByMinutes = new Krypton.Toolkit.KryptonLabel();
this.txtCheckIntervalByMinutes = new Krypton.Toolkit.KryptonTextBox();

this.lblReminderPriority = new Krypton.Toolkit.KryptonLabel();
this.txtReminderPriority = new Krypton.Toolkit.KryptonTextBox();

this.lblIsEnabled = new Krypton.Toolkit.KryptonLabel();
this.chkIsEnabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsEnabled.Values.Text ="";

this.lblNotifyChannels = new Krypton.Toolkit.KryptonLabel();
this.txtNotifyChannels = new Krypton.Toolkit.KryptonTextBox();

this.lblEffectiveDate = new Krypton.Toolkit.KryptonLabel();
this.dtpEffectiveDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblExpireDate = new Krypton.Toolkit.KryptonLabel();
this.dtpExpireDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCondition = new Krypton.Toolkit.KryptonLabel();
this.txtCondition = new Krypton.Toolkit.KryptonTextBox();
this.txtCondition.Multiline = true;

this.lblNotifyRecipientNames = new Krypton.Toolkit.KryptonLabel();
this.txtNotifyRecipientNames = new Krypton.Toolkit.KryptonTextBox();
this.txtNotifyRecipientNames.Multiline = true;

this.lblNotifyRecipients = new Krypton.Toolkit.KryptonLabel();
this.txtNotifyRecipients = new Krypton.Toolkit.KryptonTextBox();
this.txtNotifyRecipients.Multiline = true;

this.lblNotifyMessage = new Krypton.Toolkit.KryptonLabel();
this.txtNotifyMessage = new Krypton.Toolkit.KryptonTextBox();
this.txtNotifyMessage.Multiline = true;

this.lblJsonConfig = new Krypton.Toolkit.KryptonLabel();
this.txtJsonConfig = new Krypton.Toolkit.KryptonTextBox();
this.txtJsonConfig.Multiline = true;

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

           //#####RuleEngineType###Int32
this.lblRuleEngineType.AutoSize = true;
this.lblRuleEngineType.Location = new System.Drawing.Point(100,50);
this.lblRuleEngineType.Name = "lblRuleEngineType";
this.lblRuleEngineType.Size = new System.Drawing.Size(41, 12);
this.lblRuleEngineType.TabIndex = 2;
this.lblRuleEngineType.Text = "引擎类型";
this.txtRuleEngineType.Location = new System.Drawing.Point(173,46);
this.txtRuleEngineType.Name = "txtRuleEngineType";
this.txtRuleEngineType.Size = new System.Drawing.Size(100, 21);
this.txtRuleEngineType.TabIndex = 2;
this.Controls.Add(this.lblRuleEngineType);
this.Controls.Add(this.txtRuleEngineType);

           //#####500Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,75);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 3;
this.lblDescription.Text = "规则描述";
this.txtDescription.Location = new System.Drawing.Point(173,71);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 3;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####ReminderBizType###Int32
this.lblReminderBizType.AutoSize = true;
this.lblReminderBizType.Location = new System.Drawing.Point(100,100);
this.lblReminderBizType.Name = "lblReminderBizType";
this.lblReminderBizType.Size = new System.Drawing.Size(41, 12);
this.lblReminderBizType.TabIndex = 4;
this.lblReminderBizType.Text = "业务类型";
this.txtReminderBizType.Location = new System.Drawing.Point(173,96);
this.txtReminderBizType.Name = "txtReminderBizType";
this.txtReminderBizType.Size = new System.Drawing.Size(100, 21);
this.txtReminderBizType.TabIndex = 4;
this.Controls.Add(this.lblReminderBizType);
this.Controls.Add(this.txtReminderBizType);

           //#####CheckIntervalByMinutes###Int32
this.lblCheckIntervalByMinutes.AutoSize = true;
this.lblCheckIntervalByMinutes.Location = new System.Drawing.Point(100,125);
this.lblCheckIntervalByMinutes.Name = "lblCheckIntervalByMinutes";
this.lblCheckIntervalByMinutes.Size = new System.Drawing.Size(41, 12);
this.lblCheckIntervalByMinutes.TabIndex = 5;
this.lblCheckIntervalByMinutes.Text = "检测频率(分钟)";
this.txtCheckIntervalByMinutes.Location = new System.Drawing.Point(173,121);
this.txtCheckIntervalByMinutes.Name = "txtCheckIntervalByMinutes";
this.txtCheckIntervalByMinutes.Size = new System.Drawing.Size(100, 21);
this.txtCheckIntervalByMinutes.TabIndex = 5;
this.Controls.Add(this.lblCheckIntervalByMinutes);
this.Controls.Add(this.txtCheckIntervalByMinutes);

           //#####ReminderPriority###Int32
this.lblReminderPriority.AutoSize = true;
this.lblReminderPriority.Location = new System.Drawing.Point(100,150);
this.lblReminderPriority.Name = "lblReminderPriority";
this.lblReminderPriority.Size = new System.Drawing.Size(41, 12);
this.lblReminderPriority.TabIndex = 6;
this.lblReminderPriority.Text = "优先级";
this.txtReminderPriority.Location = new System.Drawing.Point(173,146);
this.txtReminderPriority.Name = "txtReminderPriority";
this.txtReminderPriority.Size = new System.Drawing.Size(100, 21);
this.txtReminderPriority.TabIndex = 6;
this.Controls.Add(this.lblReminderPriority);
this.Controls.Add(this.txtReminderPriority);

           //#####IsEnabled###Boolean
this.lblIsEnabled.AutoSize = true;
this.lblIsEnabled.Location = new System.Drawing.Point(100,175);
this.lblIsEnabled.Name = "lblIsEnabled";
this.lblIsEnabled.Size = new System.Drawing.Size(41, 12);
this.lblIsEnabled.TabIndex = 7;
this.lblIsEnabled.Text = "是否启用";
this.chkIsEnabled.Location = new System.Drawing.Point(173,171);
this.chkIsEnabled.Name = "chkIsEnabled";
this.chkIsEnabled.Size = new System.Drawing.Size(100, 21);
this.chkIsEnabled.TabIndex = 7;
this.Controls.Add(this.lblIsEnabled);
this.Controls.Add(this.chkIsEnabled);

           //#####50NotifyChannels###String
this.lblNotifyChannels.AutoSize = true;
this.lblNotifyChannels.Location = new System.Drawing.Point(100,200);
this.lblNotifyChannels.Name = "lblNotifyChannels";
this.lblNotifyChannels.Size = new System.Drawing.Size(41, 12);
this.lblNotifyChannels.TabIndex = 8;
this.lblNotifyChannels.Text = "通知渠道";
this.txtNotifyChannels.Location = new System.Drawing.Point(173,196);
this.txtNotifyChannels.Name = "txtNotifyChannels";
this.txtNotifyChannels.Size = new System.Drawing.Size(100, 21);
this.txtNotifyChannels.TabIndex = 8;
this.Controls.Add(this.lblNotifyChannels);
this.Controls.Add(this.txtNotifyChannels);

           //#####EffectiveDate###DateTime
this.lblEffectiveDate.AutoSize = true;
this.lblEffectiveDate.Location = new System.Drawing.Point(100,225);
this.lblEffectiveDate.Name = "lblEffectiveDate";
this.lblEffectiveDate.Size = new System.Drawing.Size(41, 12);
this.lblEffectiveDate.TabIndex = 9;
this.lblEffectiveDate.Text = "生效日期";
//111======225
this.dtpEffectiveDate.Location = new System.Drawing.Point(173,221);
this.dtpEffectiveDate.Name ="dtpEffectiveDate";
this.dtpEffectiveDate.Size = new System.Drawing.Size(100, 21);
this.dtpEffectiveDate.TabIndex = 9;
this.Controls.Add(this.lblEffectiveDate);
this.Controls.Add(this.dtpEffectiveDate);

           //#####ExpireDate###DateTime
this.lblExpireDate.AutoSize = true;
this.lblExpireDate.Location = new System.Drawing.Point(100,250);
this.lblExpireDate.Name = "lblExpireDate";
this.lblExpireDate.Size = new System.Drawing.Size(41, 12);
this.lblExpireDate.TabIndex = 10;
this.lblExpireDate.Text = "过期时间";
//111======250
this.dtpExpireDate.Location = new System.Drawing.Point(173,246);
this.dtpExpireDate.Name ="dtpExpireDate";
this.dtpExpireDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpireDate.TabIndex = 10;
this.Controls.Add(this.lblExpireDate);
this.Controls.Add(this.dtpExpireDate);

           //#####2147483647Condition###String
this.lblCondition.AutoSize = true;
this.lblCondition.Location = new System.Drawing.Point(100,275);
this.lblCondition.Name = "lblCondition";
this.lblCondition.Size = new System.Drawing.Size(41, 12);
this.lblCondition.TabIndex = 11;
this.lblCondition.Text = "规则条件";
this.txtCondition.Location = new System.Drawing.Point(173,271);
this.txtCondition.Name = "txtCondition";
this.txtCondition.Size = new System.Drawing.Size(100, 21);
this.txtCondition.TabIndex = 11;
this.txtCondition.Multiline = true;
this.Controls.Add(this.lblCondition);
this.Controls.Add(this.txtCondition);

           //#####2147483647NotifyRecipientNames###String
this.lblNotifyRecipientNames.AutoSize = true;
this.lblNotifyRecipientNames.Location = new System.Drawing.Point(100,300);
this.lblNotifyRecipientNames.Name = "lblNotifyRecipientNames";
this.lblNotifyRecipientNames.Size = new System.Drawing.Size(41, 12);
this.lblNotifyRecipientNames.TabIndex = 12;
this.lblNotifyRecipientNames.Text = "通知接收人员";
this.txtNotifyRecipientNames.Location = new System.Drawing.Point(173,296);
this.txtNotifyRecipientNames.Name = "txtNotifyRecipientNames";
this.txtNotifyRecipientNames.Size = new System.Drawing.Size(100, 21);
this.txtNotifyRecipientNames.TabIndex = 12;
this.txtNotifyRecipientNames.Multiline = true;
this.Controls.Add(this.lblNotifyRecipientNames);
this.Controls.Add(this.txtNotifyRecipientNames);

           //#####2147483647NotifyRecipients###String
this.lblNotifyRecipients.AutoSize = true;
this.lblNotifyRecipients.Location = new System.Drawing.Point(100,325);
this.lblNotifyRecipients.Name = "lblNotifyRecipients";
this.lblNotifyRecipients.Size = new System.Drawing.Size(41, 12);
this.lblNotifyRecipients.TabIndex = 13;
this.lblNotifyRecipients.Text = "通知接收人员ID";
this.txtNotifyRecipients.Location = new System.Drawing.Point(173,321);
this.txtNotifyRecipients.Name = "txtNotifyRecipients";
this.txtNotifyRecipients.Size = new System.Drawing.Size(100, 21);
this.txtNotifyRecipients.TabIndex = 13;
this.txtNotifyRecipients.Multiline = true;
this.Controls.Add(this.lblNotifyRecipients);
this.Controls.Add(this.txtNotifyRecipients);

           //#####2147483647NotifyMessage###String
this.lblNotifyMessage.AutoSize = true;
this.lblNotifyMessage.Location = new System.Drawing.Point(100,350);
this.lblNotifyMessage.Name = "lblNotifyMessage";
this.lblNotifyMessage.Size = new System.Drawing.Size(41, 12);
this.lblNotifyMessage.TabIndex = 14;
this.lblNotifyMessage.Text = "通知消息模板";
this.txtNotifyMessage.Location = new System.Drawing.Point(173,346);
this.txtNotifyMessage.Name = "txtNotifyMessage";
this.txtNotifyMessage.Size = new System.Drawing.Size(100, 21);
this.txtNotifyMessage.TabIndex = 14;
this.txtNotifyMessage.Multiline = true;
this.Controls.Add(this.lblNotifyMessage);
this.Controls.Add(this.txtNotifyMessage);

           //#####2147483647JsonConfig###String
this.lblJsonConfig.AutoSize = true;
this.lblJsonConfig.Location = new System.Drawing.Point(100,375);
this.lblJsonConfig.Name = "lblJsonConfig";
this.lblJsonConfig.Size = new System.Drawing.Size(41, 12);
this.lblJsonConfig.TabIndex = 15;
this.lblJsonConfig.Text = "扩展JSON配置";
this.txtJsonConfig.Location = new System.Drawing.Point(173,371);
this.txtJsonConfig.Name = "txtJsonConfig";
this.txtJsonConfig.Size = new System.Drawing.Size(100, 21);
this.txtJsonConfig.TabIndex = 15;
this.txtJsonConfig.Multiline = true;
this.Controls.Add(this.lblJsonConfig);
this.Controls.Add(this.txtJsonConfig);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,400);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 16;
this.lblCreated_at.Text = "创建时间";
//111======400
this.dtpCreated_at.Location = new System.Drawing.Point(173,396);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 16;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,425);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 17;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,421);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 17;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,450);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 18;
this.lblModified_at.Text = "修改时间";
//111======450
this.dtpModified_at.Location = new System.Drawing.Point(173,446);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 18;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,475);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 19;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,471);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 19;
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
           // this.kryptonPanel1.TabIndex = 19;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRuleName );
this.Controls.Add(this.txtRuleName );

                this.Controls.Add(this.lblRuleEngineType );
this.Controls.Add(this.txtRuleEngineType );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblReminderBizType );
this.Controls.Add(this.txtReminderBizType );

                this.Controls.Add(this.lblCheckIntervalByMinutes );
this.Controls.Add(this.txtCheckIntervalByMinutes );

                this.Controls.Add(this.lblReminderPriority );
this.Controls.Add(this.txtReminderPriority );

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

                this.Controls.Add(this.lblNotifyRecipientNames );
this.Controls.Add(this.txtNotifyRecipientNames );

                this.Controls.Add(this.lblNotifyRecipients );
this.Controls.Add(this.txtNotifyRecipients );

                this.Controls.Add(this.lblNotifyMessage );
this.Controls.Add(this.txtNotifyMessage );

                this.Controls.Add(this.lblJsonConfig );
this.Controls.Add(this.txtJsonConfig );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_ReminderRuleEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ReminderRuleEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblRuleEngineType;
private Krypton.Toolkit.KryptonTextBox txtRuleEngineType;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblReminderBizType;
private Krypton.Toolkit.KryptonTextBox txtReminderBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckIntervalByMinutes;
private Krypton.Toolkit.KryptonTextBox txtCheckIntervalByMinutes;

    
        
              private Krypton.Toolkit.KryptonLabel lblReminderPriority;
private Krypton.Toolkit.KryptonTextBox txtReminderPriority;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsEnabled;
private Krypton.Toolkit.KryptonCheckBox chkIsEnabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotifyChannels;
private Krypton.Toolkit.KryptonTextBox txtNotifyChannels;

    
        
              private Krypton.Toolkit.KryptonLabel lblEffectiveDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpireDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpExpireDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblCondition;
private Krypton.Toolkit.KryptonTextBox txtCondition;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotifyRecipientNames;
private Krypton.Toolkit.KryptonTextBox txtNotifyRecipientNames;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotifyRecipients;
private Krypton.Toolkit.KryptonTextBox txtNotifyRecipients;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotifyMessage;
private Krypton.Toolkit.KryptonTextBox txtNotifyMessage;

    
        
              private Krypton.Toolkit.KryptonLabel lblJsonConfig;
private Krypton.Toolkit.KryptonTextBox txtJsonConfig;

    
        
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

