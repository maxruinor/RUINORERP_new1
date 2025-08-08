
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:20
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 币别换算表
    /// </summary>
    partial class tb_CurrencyExchangeRateQuery
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
     
     this.lblConversionName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtConversionName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
this.lblEffectiveDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEffectiveDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblExpirationDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpExpirationDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDefaultExchRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDefaultExchRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblExecuteExchRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExecuteExchRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50ConversionName###String
this.lblConversionName.AutoSize = true;
this.lblConversionName.Location = new System.Drawing.Point(100,25);
this.lblConversionName.Name = "lblConversionName";
this.lblConversionName.Size = new System.Drawing.Size(41, 12);
this.lblConversionName.TabIndex = 1;
this.lblConversionName.Text = "换算名称";
this.txtConversionName.Location = new System.Drawing.Point(173,21);
this.txtConversionName.Name = "txtConversionName";
this.txtConversionName.Size = new System.Drawing.Size(100, 21);
this.txtConversionName.TabIndex = 1;
this.Controls.Add(this.lblConversionName);
this.Controls.Add(this.txtConversionName);

           //#####BaseCurrencyID###Int64
//属性测试50BaseCurrencyID
BaseCurrencyID主外字段不一致。//属性测试50BaseCurrencyID
TargetCurrencyID主外字段不一致。
           //#####TargetCurrencyID###Int64
//属性测试75TargetCurrencyID
BaseCurrencyID主外字段不一致。//属性测试75TargetCurrencyID
TargetCurrencyID主外字段不一致。
           //#####EffectiveDate###DateTime
this.lblEffectiveDate.AutoSize = true;
this.lblEffectiveDate.Location = new System.Drawing.Point(100,100);
this.lblEffectiveDate.Name = "lblEffectiveDate";
this.lblEffectiveDate.Size = new System.Drawing.Size(41, 12);
this.lblEffectiveDate.TabIndex = 4;
this.lblEffectiveDate.Text = "生效日期";
//111======100
this.dtpEffectiveDate.Location = new System.Drawing.Point(173,96);
this.dtpEffectiveDate.Name ="dtpEffectiveDate";
this.dtpEffectiveDate.Size = new System.Drawing.Size(100, 21);
this.dtpEffectiveDate.TabIndex = 4;
this.Controls.Add(this.lblEffectiveDate);
this.Controls.Add(this.dtpEffectiveDate);

           //#####ExpirationDate###DateTime
this.lblExpirationDate.AutoSize = true;
this.lblExpirationDate.Location = new System.Drawing.Point(100,125);
this.lblExpirationDate.Name = "lblExpirationDate";
this.lblExpirationDate.Size = new System.Drawing.Size(41, 12);
this.lblExpirationDate.TabIndex = 5;
this.lblExpirationDate.Text = "有效日期";
//111======125
this.dtpExpirationDate.Location = new System.Drawing.Point(173,121);
this.dtpExpirationDate.Name ="dtpExpirationDate";
this.dtpExpirationDate.ShowCheckBox =true;
this.dtpExpirationDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpirationDate.TabIndex = 5;
this.Controls.Add(this.lblExpirationDate);
this.Controls.Add(this.dtpExpirationDate);

           //#####DefaultExchRate###Decimal
this.lblDefaultExchRate.AutoSize = true;
this.lblDefaultExchRate.Location = new System.Drawing.Point(100,150);
this.lblDefaultExchRate.Name = "lblDefaultExchRate";
this.lblDefaultExchRate.Size = new System.Drawing.Size(41, 12);
this.lblDefaultExchRate.TabIndex = 6;
this.lblDefaultExchRate.Text = "预设汇率";
//111======150
this.txtDefaultExchRate.Location = new System.Drawing.Point(173,146);
this.txtDefaultExchRate.Name ="txtDefaultExchRate";
this.txtDefaultExchRate.Size = new System.Drawing.Size(100, 21);
this.txtDefaultExchRate.TabIndex = 6;
this.Controls.Add(this.lblDefaultExchRate);
this.Controls.Add(this.txtDefaultExchRate);

           //#####ExecuteExchRate###Decimal
this.lblExecuteExchRate.AutoSize = true;
this.lblExecuteExchRate.Location = new System.Drawing.Point(100,175);
this.lblExecuteExchRate.Name = "lblExecuteExchRate";
this.lblExecuteExchRate.Size = new System.Drawing.Size(41, 12);
this.lblExecuteExchRate.TabIndex = 7;
this.lblExecuteExchRate.Text = "执行汇率";
//111======175
this.txtExecuteExchRate.Location = new System.Drawing.Point(173,171);
this.txtExecuteExchRate.Name ="txtExecuteExchRate";
this.txtExecuteExchRate.Size = new System.Drawing.Size(100, 21);
this.txtExecuteExchRate.TabIndex = 7;
this.Controls.Add(this.lblExecuteExchRate);
this.Controls.Add(this.txtExecuteExchRate);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,200);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 8;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,196);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 8;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####100Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,225);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 9;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,221);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 9;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

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
//属性测试275Created_by
BaseCurrencyID主外字段不一致。//属性测试275Created_by
TargetCurrencyID主外字段不一致。
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
//属性测试325Modified_by
BaseCurrencyID主外字段不一致。//属性测试325Modified_by
TargetCurrencyID主外字段不一致。
          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblConversionName );
this.Controls.Add(this.txtConversionName );

                BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
                BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
                this.Controls.Add(this.lblEffectiveDate );
this.Controls.Add(this.dtpEffectiveDate );

                this.Controls.Add(this.lblExpirationDate );
this.Controls.Add(this.dtpExpirationDate );

                this.Controls.Add(this.lblDefaultExchRate );
this.Controls.Add(this.txtDefaultExchRate );

                this.Controls.Add(this.lblExecuteExchRate );
this.Controls.Add(this.txtExecuteExchRate );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
                    
            this.Name = "tb_CurrencyExchangeRateQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConversionName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtConversionName;

    
        
              BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
    
        
              BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEffectiveDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpirationDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpExpirationDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDefaultExchRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDefaultExchRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExecuteExchRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExecuteExchRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。
    
    
   
 





    }
}


