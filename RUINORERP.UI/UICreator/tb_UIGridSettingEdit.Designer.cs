// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2025 15:31:54
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// UI表格设置
    /// </summary>
    partial class tb_UIGridSettingEdit
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
     this.lblUIMenuPID = new Krypton.Toolkit.KryptonLabel();
this.cmbUIMenuPID = new Krypton.Toolkit.KryptonComboBox();

this.lblGridKeyName = new Krypton.Toolkit.KryptonLabel();
this.txtGridKeyName = new Krypton.Toolkit.KryptonTextBox();
this.txtGridKeyName.Multiline = true;

this.lblColsSetting = new Krypton.Toolkit.KryptonLabel();
this.txtColsSetting = new Krypton.Toolkit.KryptonTextBox();
this.txtColsSetting.Multiline = true;

    
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
     
            //#####UIMenuPID###Int64
//属性测试25UIMenuPID
this.lblUIMenuPID.AutoSize = true;
this.lblUIMenuPID.Location = new System.Drawing.Point(100,25);
this.lblUIMenuPID.Name = "lblUIMenuPID";
this.lblUIMenuPID.Size = new System.Drawing.Size(41, 12);
this.lblUIMenuPID.TabIndex = 1;
this.lblUIMenuPID.Text = "菜单设置";
//111======25
this.cmbUIMenuPID.Location = new System.Drawing.Point(173,21);
this.cmbUIMenuPID.Name ="cmbUIMenuPID";
this.cmbUIMenuPID.Size = new System.Drawing.Size(100, 21);
this.cmbUIMenuPID.TabIndex = 1;
this.Controls.Add(this.lblUIMenuPID);
this.Controls.Add(this.cmbUIMenuPID);

           //#####255GridKeyName###String
this.lblGridKeyName.AutoSize = true;
this.lblGridKeyName.Location = new System.Drawing.Point(100,50);
this.lblGridKeyName.Name = "lblGridKeyName";
this.lblGridKeyName.Size = new System.Drawing.Size(41, 12);
this.lblGridKeyName.TabIndex = 2;
this.lblGridKeyName.Text = "表格名称";
this.txtGridKeyName.Location = new System.Drawing.Point(173,46);
this.txtGridKeyName.Name = "txtGridKeyName";
this.txtGridKeyName.Size = new System.Drawing.Size(100, 21);
this.txtGridKeyName.TabIndex = 2;
this.Controls.Add(this.lblGridKeyName);
this.Controls.Add(this.txtGridKeyName);

           //#####2147483647ColsSetting###String
this.lblColsSetting.AutoSize = true;
this.lblColsSetting.Location = new System.Drawing.Point(100,75);
this.lblColsSetting.Name = "lblColsSetting";
this.lblColsSetting.Size = new System.Drawing.Size(41, 12);
this.lblColsSetting.TabIndex = 3;
this.lblColsSetting.Text = "列设置信息";
this.txtColsSetting.Location = new System.Drawing.Point(173,71);
this.txtColsSetting.Name = "txtColsSetting";
this.txtColsSetting.Size = new System.Drawing.Size(100, 21);
this.txtColsSetting.TabIndex = 3;
this.txtColsSetting.Multiline = true;
this.Controls.Add(this.lblColsSetting);
this.Controls.Add(this.txtColsSetting);

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
           // this.kryptonPanel1.TabIndex = 3;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblUIMenuPID );
this.Controls.Add(this.cmbUIMenuPID );

                this.Controls.Add(this.lblGridKeyName );
this.Controls.Add(this.txtGridKeyName );

                this.Controls.Add(this.lblColsSetting );
this.Controls.Add(this.txtColsSetting );

                            // 
            // "tb_UIGridSettingEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_UIGridSettingEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblUIMenuPID;
private Krypton.Toolkit.KryptonComboBox cmbUIMenuPID;

    
        
              private Krypton.Toolkit.KryptonLabel lblGridKeyName;
private Krypton.Toolkit.KryptonTextBox txtGridKeyName;

    
        
              private Krypton.Toolkit.KryptonLabel lblColsSetting;
private Krypton.Toolkit.KryptonTextBox txtColsSetting;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

