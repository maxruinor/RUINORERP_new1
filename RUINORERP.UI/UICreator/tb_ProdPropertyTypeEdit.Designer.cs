// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:57
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品属性类型EVA
    /// </summary>
    partial class tb_ProdPropertyTypeEdit
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
     this.lblPropertyTypeName = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyTypeName = new Krypton.Toolkit.KryptonTextBox();

this.lblPropertyTypeDesc = new Krypton.Toolkit.KryptonLabel();
this.txtPropertyTypeDesc = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####50PropertyTypeName###String
this.lblPropertyTypeName.AutoSize = true;
this.lblPropertyTypeName.Location = new System.Drawing.Point(100,25);
this.lblPropertyTypeName.Name = "lblPropertyTypeName";
this.lblPropertyTypeName.Size = new System.Drawing.Size(41, 12);
this.lblPropertyTypeName.TabIndex = 1;
this.lblPropertyTypeName.Text = "属性类型名称";
this.txtPropertyTypeName.Location = new System.Drawing.Point(173,21);
this.txtPropertyTypeName.Name = "txtPropertyTypeName";
this.txtPropertyTypeName.Size = new System.Drawing.Size(100, 21);
this.txtPropertyTypeName.TabIndex = 1;
this.Controls.Add(this.lblPropertyTypeName);
this.Controls.Add(this.txtPropertyTypeName);

           //#####100PropertyTypeDesc###String
this.lblPropertyTypeDesc.AutoSize = true;
this.lblPropertyTypeDesc.Location = new System.Drawing.Point(100,50);
this.lblPropertyTypeDesc.Name = "lblPropertyTypeDesc";
this.lblPropertyTypeDesc.Size = new System.Drawing.Size(41, 12);
this.lblPropertyTypeDesc.TabIndex = 2;
this.lblPropertyTypeDesc.Text = "属性类型描述";
this.txtPropertyTypeDesc.Location = new System.Drawing.Point(173,46);
this.txtPropertyTypeDesc.Name = "txtPropertyTypeDesc";
this.txtPropertyTypeDesc.Size = new System.Drawing.Size(100, 21);
this.txtPropertyTypeDesc.TabIndex = 2;
this.Controls.Add(this.lblPropertyTypeDesc);
this.Controls.Add(this.txtPropertyTypeDesc);

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
           // this.kryptonPanel1.TabIndex = 2;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPropertyTypeName );
this.Controls.Add(this.txtPropertyTypeName );

                this.Controls.Add(this.lblPropertyTypeDesc );
this.Controls.Add(this.txtPropertyTypeDesc );

                            // 
            // "tb_ProdPropertyTypeEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ProdPropertyTypeEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPropertyTypeName;
private Krypton.Toolkit.KryptonTextBox txtPropertyTypeName;

    
        
              private Krypton.Toolkit.KryptonLabel lblPropertyTypeDesc;
private Krypton.Toolkit.KryptonTextBox txtPropertyTypeDesc;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

