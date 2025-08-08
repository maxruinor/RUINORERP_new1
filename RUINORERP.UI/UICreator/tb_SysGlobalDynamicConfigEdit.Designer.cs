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
    /// 系统全局动态配置表 行转列
    /// </summary>
    partial class tb_SysGlobalDynamicConfigEdit
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
     this.lblConfigKey = new Krypton.Toolkit.KryptonLabel();
this.txtConfigKey = new Krypton.Toolkit.KryptonTextBox();
this.txtConfigKey.Multiline = true;

this.lblConfigValue = new Krypton.Toolkit.KryptonLabel();
this.txtConfigValue = new Krypton.Toolkit.KryptonTextBox();
this.txtConfigValue.Multiline = true;

this.lblDescription = new Krypton.Toolkit.KryptonLabel();
this.txtDescription = new Krypton.Toolkit.KryptonTextBox();

this.lblValueType = new Krypton.Toolkit.KryptonLabel();
this.txtValueType = new Krypton.Toolkit.KryptonTextBox();

this.lblConfigType = new Krypton.Toolkit.KryptonLabel();
this.txtConfigType = new Krypton.Toolkit.KryptonTextBox();

this.lblIsActive = new Krypton.Toolkit.KryptonLabel();
this.chkIsActive = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsActive.Values.Text ="";
this.chkIsActive.Checked = true;
this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;

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

           //#####200Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,75);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 3;
this.lblDescription.Text = "配置描述";
this.txtDescription.Location = new System.Drawing.Point(173,71);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 3;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####ValueType###Int32
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
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,200);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 8;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,196);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 8;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,250);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 10;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,246);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 10;
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
           // this.kryptonPanel1.TabIndex = 10;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblConfigKey );
this.Controls.Add(this.txtConfigKey );

                this.Controls.Add(this.lblConfigValue );
this.Controls.Add(this.txtConfigValue );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblValueType );
this.Controls.Add(this.txtValueType );

                this.Controls.Add(this.lblConfigType );
this.Controls.Add(this.txtConfigType );

                this.Controls.Add(this.lblIsActive );
this.Controls.Add(this.chkIsActive );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_SysGlobalDynamicConfigEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_SysGlobalDynamicConfigEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblConfigKey;
private Krypton.Toolkit.KryptonTextBox txtConfigKey;

    
        
              private Krypton.Toolkit.KryptonLabel lblConfigValue;
private Krypton.Toolkit.KryptonTextBox txtConfigValue;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblValueType;
private Krypton.Toolkit.KryptonTextBox txtValueType;

    
        
              private Krypton.Toolkit.KryptonLabel lblConfigType;
private Krypton.Toolkit.KryptonTextBox txtConfigType;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsActive;
private Krypton.Toolkit.KryptonCheckBox chkIsActive;

    
        
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

