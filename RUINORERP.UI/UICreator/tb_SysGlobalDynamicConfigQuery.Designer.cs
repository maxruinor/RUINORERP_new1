
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 09:38:35
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 系统全局动态配置表 行转列
    /// </summary>
    partial class tb_SysGlobalDynamicConfigQuery
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
     
     this.lblConfigKey = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtConfigKey = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtConfigKey.Multiline = true;

this.lblConfigValue = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtConfigValue = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtConfigValue.Multiline = true;

this.lblDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkDescription = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkDescription.Values.Text ="";

this.lblValueType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtValueType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblConfigType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtConfigType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsActive = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsActive = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsActive.Values.Text ="";
this.chkIsActive.Checked = true;
this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####255ConfigKey###String
this.lblConfigKey.AutoSize = true;
this.lblConfigKey.Location = new System.Drawing.Point(100,25);
this.lblConfigKey.Name = "lblConfigKey";
this.lblConfigKey.Size = new System.Drawing.Size(41, 12);
this.lblConfigKey.TabIndex = 1;
this.lblConfigKey.Text = "配置项";
this.txtConfigKey.Location = new System.Drawing.Point(173,21);
this.txtConfigKey.Name = "txtConfigKey";
this.txtConfigKey.Size = new System.Drawing.Size(100, 21);
this.txtConfigKey.TabIndex = 1;
this.Controls.Add(this.lblConfigKey);
this.Controls.Add(this.txtConfigKey);

           //#####2147483647ConfigValue###String
this.lblConfigValue.AutoSize = true;
this.lblConfigValue.Location = new System.Drawing.Point(100,50);
this.lblConfigValue.Name = "lblConfigValue";
this.lblConfigValue.Size = new System.Drawing.Size(41, 12);
this.lblConfigValue.TabIndex = 2;
this.lblConfigValue.Text = "配置值";
this.txtConfigValue.Location = new System.Drawing.Point(173,46);
this.txtConfigValue.Name = "txtConfigValue";
this.txtConfigValue.Size = new System.Drawing.Size(100, 21);
this.txtConfigValue.TabIndex = 2;
this.txtConfigValue.Multiline = true;
this.Controls.Add(this.lblConfigValue);
this.Controls.Add(this.txtConfigValue);

           //#####Description###Boolean
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,75);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 3;
this.lblDescription.Text = "配置描述";
this.chkDescription.Location = new System.Drawing.Point(173,71);
this.chkDescription.Name = "chkDescription";
this.chkDescription.Size = new System.Drawing.Size(100, 21);
this.chkDescription.TabIndex = 3;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.chkDescription);

           //#####50ValueType###String
this.lblValueType.AutoSize = true;
this.lblValueType.Location = new System.Drawing.Point(100,100);
this.lblValueType.Name = "lblValueType";
this.lblValueType.Size = new System.Drawing.Size(41, 12);
this.lblValueType.TabIndex = 4;
this.lblValueType.Text = "配置项的值类型";
this.txtValueType.Location = new System.Drawing.Point(173,96);
this.txtValueType.Name = "txtValueType";
this.txtValueType.Size = new System.Drawing.Size(100, 21);
this.txtValueType.TabIndex = 4;
this.Controls.Add(this.lblValueType);
this.Controls.Add(this.txtValueType);

           //#####100ConfigType###String
this.lblConfigType.AutoSize = true;
this.lblConfigType.Location = new System.Drawing.Point(100,125);
this.lblConfigType.Name = "lblConfigType";
this.lblConfigType.Size = new System.Drawing.Size(41, 12);
this.lblConfigType.TabIndex = 5;
this.lblConfigType.Text = "配置类型";
this.txtConfigType.Location = new System.Drawing.Point(173,121);
this.txtConfigType.Name = "txtConfigType";
this.txtConfigType.Size = new System.Drawing.Size(100, 21);
this.txtConfigType.TabIndex = 5;
this.Controls.Add(this.lblConfigType);
this.Controls.Add(this.txtConfigType);

           //#####IsActive###Boolean
this.lblIsActive.AutoSize = true;
this.lblIsActive.Location = new System.Drawing.Point(100,150);
this.lblIsActive.Name = "lblIsActive";
this.lblIsActive.Size = new System.Drawing.Size(41, 12);
this.lblIsActive.TabIndex = 6;
this.lblIsActive.Text = "启用";
this.chkIsActive.Location = new System.Drawing.Point(173,146);
this.chkIsActive.Name = "chkIsActive";
this.chkIsActive.Size = new System.Drawing.Size(100, 21);
this.chkIsActive.TabIndex = 6;
this.Controls.Add(this.lblIsActive);
this.Controls.Add(this.chkIsActive);

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
                this.Controls.Add(this.lblConfigKey );
this.Controls.Add(this.txtConfigKey );

                this.Controls.Add(this.lblConfigValue );
this.Controls.Add(this.txtConfigValue );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.chkDescription );

                this.Controls.Add(this.lblValueType );
this.Controls.Add(this.txtValueType );

                this.Controls.Add(this.lblConfigType );
this.Controls.Add(this.txtConfigType );

                this.Controls.Add(this.lblIsActive );
this.Controls.Add(this.chkIsActive );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                    
            this.Name = "tb_SysGlobalDynamicConfigQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConfigKey;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtConfigKey;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConfigValue;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtConfigValue;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDescription;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkDescription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblValueType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtValueType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConfigType;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtConfigType;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsActive;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsActive;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
    
   
 





    }
}


