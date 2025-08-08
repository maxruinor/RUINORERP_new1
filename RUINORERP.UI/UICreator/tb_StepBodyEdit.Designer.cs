// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:17
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 步骤定义
    /// </summary>
    partial class tb_StepBodyEdit
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
     this.lblPara_Id = new Krypton.Toolkit.KryptonLabel();
this.cmbPara_Id = new Krypton.Toolkit.KryptonComboBox();

this.lblName = new Krypton.Toolkit.KryptonLabel();
this.txtName = new Krypton.Toolkit.KryptonTextBox();

this.lblDisplayName = new Krypton.Toolkit.KryptonLabel();
this.txtDisplayName = new Krypton.Toolkit.KryptonTextBox();

this.lblTypeFullName = new Krypton.Toolkit.KryptonLabel();
this.txtTypeFullName = new Krypton.Toolkit.KryptonTextBox();

this.lblAssemblyFullName = new Krypton.Toolkit.KryptonLabel();
this.txtAssemblyFullName = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####Para_Id###Int64
//属性测试25Para_Id
this.lblPara_Id.AutoSize = true;
this.lblPara_Id.Location = new System.Drawing.Point(100,25);
this.lblPara_Id.Name = "lblPara_Id";
this.lblPara_Id.Size = new System.Drawing.Size(41, 12);
this.lblPara_Id.TabIndex = 1;
this.lblPara_Id.Text = "输入参数";
//111======25
this.cmbPara_Id.Location = new System.Drawing.Point(173,21);
this.cmbPara_Id.Name ="cmbPara_Id";
this.cmbPara_Id.Size = new System.Drawing.Size(100, 21);
this.cmbPara_Id.TabIndex = 1;
this.Controls.Add(this.lblPara_Id);
this.Controls.Add(this.cmbPara_Id);

           //#####50Name###String
this.lblName.AutoSize = true;
this.lblName.Location = new System.Drawing.Point(100,50);
this.lblName.Name = "lblName";
this.lblName.Size = new System.Drawing.Size(41, 12);
this.lblName.TabIndex = 2;
this.lblName.Text = "名称";
this.txtName.Location = new System.Drawing.Point(173,46);
this.txtName.Name = "txtName";
this.txtName.Size = new System.Drawing.Size(100, 21);
this.txtName.TabIndex = 2;
this.Controls.Add(this.lblName);
this.Controls.Add(this.txtName);

           //#####50DisplayName###String
this.lblDisplayName.AutoSize = true;
this.lblDisplayName.Location = new System.Drawing.Point(100,75);
this.lblDisplayName.Name = "lblDisplayName";
this.lblDisplayName.Size = new System.Drawing.Size(41, 12);
this.lblDisplayName.TabIndex = 3;
this.lblDisplayName.Text = "显示名称";
this.txtDisplayName.Location = new System.Drawing.Point(173,71);
this.txtDisplayName.Name = "txtDisplayName";
this.txtDisplayName.Size = new System.Drawing.Size(100, 21);
this.txtDisplayName.TabIndex = 3;
this.Controls.Add(this.lblDisplayName);
this.Controls.Add(this.txtDisplayName);

           //#####50TypeFullName###String
this.lblTypeFullName.AutoSize = true;
this.lblTypeFullName.Location = new System.Drawing.Point(100,100);
this.lblTypeFullName.Name = "lblTypeFullName";
this.lblTypeFullName.Size = new System.Drawing.Size(41, 12);
this.lblTypeFullName.TabIndex = 4;
this.lblTypeFullName.Text = "类型全名";
this.txtTypeFullName.Location = new System.Drawing.Point(173,96);
this.txtTypeFullName.Name = "txtTypeFullName";
this.txtTypeFullName.Size = new System.Drawing.Size(100, 21);
this.txtTypeFullName.TabIndex = 4;
this.Controls.Add(this.lblTypeFullName);
this.Controls.Add(this.txtTypeFullName);

           //#####50AssemblyFullName###String
this.lblAssemblyFullName.AutoSize = true;
this.lblAssemblyFullName.Location = new System.Drawing.Point(100,125);
this.lblAssemblyFullName.Name = "lblAssemblyFullName";
this.lblAssemblyFullName.Size = new System.Drawing.Size(41, 12);
this.lblAssemblyFullName.TabIndex = 5;
this.lblAssemblyFullName.Text = "标题";
this.txtAssemblyFullName.Location = new System.Drawing.Point(173,121);
this.txtAssemblyFullName.Name = "txtAssemblyFullName";
this.txtAssemblyFullName.Size = new System.Drawing.Size(100, 21);
this.txtAssemblyFullName.TabIndex = 5;
this.Controls.Add(this.lblAssemblyFullName);
this.Controls.Add(this.txtAssemblyFullName);

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
                this.Controls.Add(this.lblPara_Id );
this.Controls.Add(this.cmbPara_Id );

                this.Controls.Add(this.lblName );
this.Controls.Add(this.txtName );

                this.Controls.Add(this.lblDisplayName );
this.Controls.Add(this.txtDisplayName );

                this.Controls.Add(this.lblTypeFullName );
this.Controls.Add(this.txtTypeFullName );

                this.Controls.Add(this.lblAssemblyFullName );
this.Controls.Add(this.txtAssemblyFullName );

                            // 
            // "tb_StepBodyEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_StepBodyEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPara_Id;
private Krypton.Toolkit.KryptonComboBox cmbPara_Id;

    
        
              private Krypton.Toolkit.KryptonLabel lblName;
private Krypton.Toolkit.KryptonTextBox txtName;

    
        
              private Krypton.Toolkit.KryptonLabel lblDisplayName;
private Krypton.Toolkit.KryptonTextBox txtDisplayName;

    
        
              private Krypton.Toolkit.KryptonLabel lblTypeFullName;
private Krypton.Toolkit.KryptonTextBox txtTypeFullName;

    
        
              private Krypton.Toolkit.KryptonLabel lblAssemblyFullName;
private Krypton.Toolkit.KryptonTextBox txtAssemblyFullName;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

