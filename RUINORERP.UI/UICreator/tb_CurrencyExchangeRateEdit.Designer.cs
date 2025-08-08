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
    partial class tb_CurrencyExchangeRateEdit
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
     this.lblConversionName = new Krypton.Toolkit.KryptonLabel();
this.txtConversionName = new Krypton.Toolkit.KryptonTextBox();

BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。this.lblBaseCurrencyID = new Krypton.Toolkit.KryptonLabel();
this.txtBaseCurrencyID = new Krypton.Toolkit.KryptonTextBox();

BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。this.lblTargetCurrencyID = new Krypton.Toolkit.KryptonLabel();
this.txtTargetCurrencyID = new Krypton.Toolkit.KryptonTextBox();

this.lblEffectiveDate = new Krypton.Toolkit.KryptonLabel();
this.dtpEffectiveDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblExpirationDate = new Krypton.Toolkit.KryptonLabel();
this.dtpExpirationDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblDefaultExchRate = new Krypton.Toolkit.KryptonLabel();
this.txtDefaultExchRate = new Krypton.Toolkit.KryptonTextBox();

this.lblExecuteExchRate = new Krypton.Toolkit.KryptonLabel();
this.txtExecuteExchRate = new Krypton.Toolkit.KryptonTextBox();

this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
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
TargetCurrencyID主外字段不一致。this.lblBaseCurrencyID.AutoSize = true;
this.lblBaseCurrencyID.Location = new System.Drawing.Point(100,50);
this.lblBaseCurrencyID.Name = "lblBaseCurrencyID";
this.lblBaseCurrencyID.Size = new System.Drawing.Size(41, 12);
this.lblBaseCurrencyID.TabIndex = 2;
this.lblBaseCurrencyID.Text = "基本币别";
this.txtBaseCurrencyID.Location = new System.Drawing.Point(173,46);
this.txtBaseCurrencyID.Name = "txtBaseCurrencyID";
this.txtBaseCurrencyID.Size = new System.Drawing.Size(100, 21);
this.txtBaseCurrencyID.TabIndex = 2;
this.Controls.Add(this.lblBaseCurrencyID);
this.Controls.Add(this.txtBaseCurrencyID);

           //#####TargetCurrencyID###Int64
//属性测试75TargetCurrencyID
BaseCurrencyID主外字段不一致。//属性测试75TargetCurrencyID
TargetCurrencyID主外字段不一致。this.lblTargetCurrencyID.AutoSize = true;
this.lblTargetCurrencyID.Location = new System.Drawing.Point(100,75);
this.lblTargetCurrencyID.Name = "lblTargetCurrencyID";
this.lblTargetCurrencyID.Size = new System.Drawing.Size(41, 12);
this.lblTargetCurrencyID.TabIndex = 3;
this.lblTargetCurrencyID.Text = "目标币别";
this.txtTargetCurrencyID.Location = new System.Drawing.Point(173,71);
this.txtTargetCurrencyID.Name = "txtTargetCurrencyID";
this.txtTargetCurrencyID.Size = new System.Drawing.Size(100, 21);
this.txtTargetCurrencyID.TabIndex = 3;
this.Controls.Add(this.lblTargetCurrencyID);
this.Controls.Add(this.txtTargetCurrencyID);

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
TargetCurrencyID主外字段不一致。this.lblCreated_by.AutoSize = true;
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
//属性测试325Modified_by
BaseCurrencyID主外字段不一致。//属性测试325Modified_by
TargetCurrencyID主外字段不一致。this.lblModified_by.AutoSize = true;
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
                this.Controls.Add(this.lblConversionName );
this.Controls.Add(this.txtConversionName );

                BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。this.Controls.Add(this.lblBaseCurrencyID );
this.Controls.Add(this.txtBaseCurrencyID );

                BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。this.Controls.Add(this.lblTargetCurrencyID );
this.Controls.Add(this.txtTargetCurrencyID );

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

                BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_CurrencyExchangeRateEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CurrencyExchangeRateEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblConversionName;
private Krypton.Toolkit.KryptonTextBox txtConversionName;

    
        
              BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblBaseCurrencyID;
private Krypton.Toolkit.KryptonTextBox txtBaseCurrencyID;

    
        
              BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblTargetCurrencyID;
private Krypton.Toolkit.KryptonTextBox txtTargetCurrencyID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEffectiveDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpirationDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpExpirationDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblDefaultExchRate;
private Krypton.Toolkit.KryptonTextBox txtDefaultExchRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblExecuteExchRate;
private Krypton.Toolkit.KryptonTextBox txtExecuteExchRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              BaseCurrencyID主外字段不一致。TargetCurrencyID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

