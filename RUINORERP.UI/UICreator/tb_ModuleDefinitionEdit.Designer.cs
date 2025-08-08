// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:43
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 功能模块定义（仅限部分已经硬码并体现于菜单表中）
    /// </summary>
    partial class tb_ModuleDefinitionEdit
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
     this.lblModuleNo = new Krypton.Toolkit.KryptonLabel();
this.txtModuleNo = new Krypton.Toolkit.KryptonTextBox();

this.lblModuleName = new Krypton.Toolkit.KryptonLabel();
this.txtModuleName = new Krypton.Toolkit.KryptonTextBox();

this.lblVisible = new Krypton.Toolkit.KryptonLabel();
this.chkVisible = new Krypton.Toolkit.KryptonCheckBox();
this.chkVisible.Values.Text ="";

this.lblAvailable = new Krypton.Toolkit.KryptonLabel();
this.chkAvailable = new Krypton.Toolkit.KryptonCheckBox();
this.chkAvailable.Values.Text ="";

this.lblIconFile_Path = new Krypton.Toolkit.KryptonLabel();
this.txtIconFile_Path = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####50ModuleNo###String
this.lblModuleNo.AutoSize = true;
this.lblModuleNo.Location = new System.Drawing.Point(100,25);
this.lblModuleNo.Name = "lblModuleNo";
this.lblModuleNo.Size = new System.Drawing.Size(41, 12);
this.lblModuleNo.TabIndex = 1;
this.lblModuleNo.Text = "模块编号";
this.txtModuleNo.Location = new System.Drawing.Point(173,21);
this.txtModuleNo.Name = "txtModuleNo";
this.txtModuleNo.Size = new System.Drawing.Size(100, 21);
this.txtModuleNo.TabIndex = 1;
this.Controls.Add(this.lblModuleNo);
this.Controls.Add(this.txtModuleNo);

           //#####20ModuleName###String
this.lblModuleName.AutoSize = true;
this.lblModuleName.Location = new System.Drawing.Point(100,50);
this.lblModuleName.Name = "lblModuleName";
this.lblModuleName.Size = new System.Drawing.Size(41, 12);
this.lblModuleName.TabIndex = 2;
this.lblModuleName.Text = "模块名称";
this.txtModuleName.Location = new System.Drawing.Point(173,46);
this.txtModuleName.Name = "txtModuleName";
this.txtModuleName.Size = new System.Drawing.Size(100, 21);
this.txtModuleName.TabIndex = 2;
this.Controls.Add(this.lblModuleName);
this.Controls.Add(this.txtModuleName);

           //#####Visible###Boolean
this.lblVisible.AutoSize = true;
this.lblVisible.Location = new System.Drawing.Point(100,75);
this.lblVisible.Name = "lblVisible";
this.lblVisible.Size = new System.Drawing.Size(41, 12);
this.lblVisible.TabIndex = 3;
this.lblVisible.Text = "是否可见";
this.chkVisible.Location = new System.Drawing.Point(173,71);
this.chkVisible.Name = "chkVisible";
this.chkVisible.Size = new System.Drawing.Size(100, 21);
this.chkVisible.TabIndex = 3;
this.Controls.Add(this.lblVisible);
this.Controls.Add(this.chkVisible);

           //#####Available###Boolean
this.lblAvailable.AutoSize = true;
this.lblAvailable.Location = new System.Drawing.Point(100,100);
this.lblAvailable.Name = "lblAvailable";
this.lblAvailable.Size = new System.Drawing.Size(41, 12);
this.lblAvailable.TabIndex = 4;
this.lblAvailable.Text = "是否可用";
this.chkAvailable.Location = new System.Drawing.Point(173,96);
this.chkAvailable.Name = "chkAvailable";
this.chkAvailable.Size = new System.Drawing.Size(100, 21);
this.chkAvailable.TabIndex = 4;
this.Controls.Add(this.lblAvailable);
this.Controls.Add(this.chkAvailable);

           //#####100IconFile_Path###String
this.lblIconFile_Path.AutoSize = true;
this.lblIconFile_Path.Location = new System.Drawing.Point(100,125);
this.lblIconFile_Path.Name = "lblIconFile_Path";
this.lblIconFile_Path.Size = new System.Drawing.Size(41, 12);
this.lblIconFile_Path.TabIndex = 5;
this.lblIconFile_Path.Text = "图标路径";
this.txtIconFile_Path.Location = new System.Drawing.Point(173,121);
this.txtIconFile_Path.Name = "txtIconFile_Path";
this.txtIconFile_Path.Size = new System.Drawing.Size(100, 21);
this.txtIconFile_Path.TabIndex = 5;
this.Controls.Add(this.lblIconFile_Path);
this.Controls.Add(this.txtIconFile_Path);

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
           // this.kryptonPanel1.TabIndex = 5;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblModuleNo );
this.Controls.Add(this.txtModuleNo );

                this.Controls.Add(this.lblModuleName );
this.Controls.Add(this.txtModuleName );

                this.Controls.Add(this.lblVisible );
this.Controls.Add(this.chkVisible );

                this.Controls.Add(this.lblAvailable );
this.Controls.Add(this.chkAvailable );

                this.Controls.Add(this.lblIconFile_Path );
this.Controls.Add(this.txtIconFile_Path );

                            // 
            // "tb_ModuleDefinitionEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ModuleDefinitionEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblModuleNo;
private Krypton.Toolkit.KryptonTextBox txtModuleNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblModuleName;
private Krypton.Toolkit.KryptonTextBox txtModuleName;

    
        
              private Krypton.Toolkit.KryptonLabel lblVisible;
private Krypton.Toolkit.KryptonCheckBox chkVisible;

    
        
              private Krypton.Toolkit.KryptonLabel lblAvailable;
private Krypton.Toolkit.KryptonCheckBox chkAvailable;

    
        
              private Krypton.Toolkit.KryptonLabel lblIconFile_Path;
private Krypton.Toolkit.KryptonTextBox txtIconFile_Path;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

