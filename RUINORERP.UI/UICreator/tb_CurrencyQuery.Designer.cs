
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
    /// 币别资料表
    /// </summary>
    partial class tb_CurrencyQuery
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
     
     this.lblCountry = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCountry = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCurrencyCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCurrencyCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCurrencyName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCurrencyName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCurrencySymbol = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCurrencySymbol = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIs_BaseCurrency = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_BaseCurrency = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_BaseCurrency.Values.Text ="";

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50Country###String
this.lblCountry.AutoSize = true;
this.lblCountry.Location = new System.Drawing.Point(100,25);
this.lblCountry.Name = "lblCountry";
this.lblCountry.Size = new System.Drawing.Size(41, 12);
this.lblCountry.TabIndex = 1;
this.lblCountry.Text = "国家";
this.txtCountry.Location = new System.Drawing.Point(173,21);
this.txtCountry.Name = "txtCountry";
this.txtCountry.Size = new System.Drawing.Size(100, 21);
this.txtCountry.TabIndex = 1;
this.Controls.Add(this.lblCountry);
this.Controls.Add(this.txtCountry);

           //#####50CurrencyCode###String
this.lblCurrencyCode.AutoSize = true;
this.lblCurrencyCode.Location = new System.Drawing.Point(100,50);
this.lblCurrencyCode.Name = "lblCurrencyCode";
this.lblCurrencyCode.Size = new System.Drawing.Size(41, 12);
this.lblCurrencyCode.TabIndex = 2;
this.lblCurrencyCode.Text = "币别代码";
this.txtCurrencyCode.Location = new System.Drawing.Point(173,46);
this.txtCurrencyCode.Name = "txtCurrencyCode";
this.txtCurrencyCode.Size = new System.Drawing.Size(100, 21);
this.txtCurrencyCode.TabIndex = 2;
this.Controls.Add(this.lblCurrencyCode);
this.Controls.Add(this.txtCurrencyCode);

           //#####20CurrencyName###String
this.lblCurrencyName.AutoSize = true;
this.lblCurrencyName.Location = new System.Drawing.Point(100,75);
this.lblCurrencyName.Name = "lblCurrencyName";
this.lblCurrencyName.Size = new System.Drawing.Size(41, 12);
this.lblCurrencyName.TabIndex = 3;
this.lblCurrencyName.Text = "币别名称";
this.txtCurrencyName.Location = new System.Drawing.Point(173,71);
this.txtCurrencyName.Name = "txtCurrencyName";
this.txtCurrencyName.Size = new System.Drawing.Size(100, 21);
this.txtCurrencyName.TabIndex = 3;
this.Controls.Add(this.lblCurrencyName);
this.Controls.Add(this.txtCurrencyName);

           //#####50CurrencySymbol###String
this.lblCurrencySymbol.AutoSize = true;
this.lblCurrencySymbol.Location = new System.Drawing.Point(100,100);
this.lblCurrencySymbol.Name = "lblCurrencySymbol";
this.lblCurrencySymbol.Size = new System.Drawing.Size(41, 12);
this.lblCurrencySymbol.TabIndex = 4;
this.lblCurrencySymbol.Text = "币别符号";
this.txtCurrencySymbol.Location = new System.Drawing.Point(173,96);
this.txtCurrencySymbol.Name = "txtCurrencySymbol";
this.txtCurrencySymbol.Size = new System.Drawing.Size(100, 21);
this.txtCurrencySymbol.TabIndex = 4;
this.Controls.Add(this.lblCurrencySymbol);
this.Controls.Add(this.txtCurrencySymbol);

           //#####Is_BaseCurrency###Boolean
this.lblIs_BaseCurrency.AutoSize = true;
this.lblIs_BaseCurrency.Location = new System.Drawing.Point(100,125);
this.lblIs_BaseCurrency.Name = "lblIs_BaseCurrency";
this.lblIs_BaseCurrency.Size = new System.Drawing.Size(41, 12);
this.lblIs_BaseCurrency.TabIndex = 5;
this.lblIs_BaseCurrency.Text = "为本位币";
this.chkIs_BaseCurrency.Location = new System.Drawing.Point(173,121);
this.chkIs_BaseCurrency.Name = "chkIs_BaseCurrency";
this.chkIs_BaseCurrency.Size = new System.Drawing.Size(100, 21);
this.chkIs_BaseCurrency.TabIndex = 5;
this.Controls.Add(this.lblIs_BaseCurrency);
this.Controls.Add(this.chkIs_BaseCurrency);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,150);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 6;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,146);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 6;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,175);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 7;
this.lblCreated_at.Text = "创建时间";
//111======175
this.dtpCreated_at.Location = new System.Drawing.Point(173,171);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 7;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,225);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 9;
this.lblModified_at.Text = "修改时间";
//111======225
this.dtpModified_at.Location = new System.Drawing.Point(173,221);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 9;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCountry );
this.Controls.Add(this.txtCountry );

                this.Controls.Add(this.lblCurrencyCode );
this.Controls.Add(this.txtCurrencyCode );

                this.Controls.Add(this.lblCurrencyName );
this.Controls.Add(this.txtCurrencyName );

                this.Controls.Add(this.lblCurrencySymbol );
this.Controls.Add(this.txtCurrencySymbol );

                this.Controls.Add(this.lblIs_BaseCurrency );
this.Controls.Add(this.chkIs_BaseCurrency );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_CurrencyQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCountry;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCountry;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrencyCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCurrencyCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrencyName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCurrencyName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrencySymbol;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCurrencySymbol;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_BaseCurrency;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_BaseCurrency;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


