// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 图片表
    /// </summary>
    partial class tb_ImagesEdit
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
     this.lblImages = new Krypton.Toolkit.KryptonLabel();
this.txtImages = new Krypton.Toolkit.KryptonTextBox();
this.txtImages.Multiline = true;

this.lblImages_Path = new Krypton.Toolkit.KryptonLabel();
this.txtImages_Path = new Krypton.Toolkit.KryptonTextBox();
this.txtImages_Path.Multiline = true;

    
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
     
            //#####255Images###String
this.lblImages.AutoSize = true;
this.lblImages.Location = new System.Drawing.Point(100,25);
this.lblImages.Name = "lblImages";
this.lblImages.Size = new System.Drawing.Size(41, 12);
this.lblImages.TabIndex = 1;
this.lblImages.Text = "";
this.txtImages.Location = new System.Drawing.Point(173,21);
this.txtImages.Name = "txtImages";
this.txtImages.Size = new System.Drawing.Size(100, 21);
this.txtImages.TabIndex = 1;
this.Controls.Add(this.lblImages);
this.Controls.Add(this.txtImages);

           //#####500Images_Path###String
this.lblImages_Path.AutoSize = true;
this.lblImages_Path.Location = new System.Drawing.Point(100,50);
this.lblImages_Path.Name = "lblImages_Path";
this.lblImages_Path.Size = new System.Drawing.Size(41, 12);
this.lblImages_Path.TabIndex = 2;
this.lblImages_Path.Text = "";
this.txtImages_Path.Location = new System.Drawing.Point(173,46);
this.txtImages_Path.Name = "txtImages_Path";
this.txtImages_Path.Size = new System.Drawing.Size(100, 21);
this.txtImages_Path.TabIndex = 2;
this.Controls.Add(this.lblImages_Path);
this.Controls.Add(this.txtImages_Path);

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
                this.Controls.Add(this.lblImages );
this.Controls.Add(this.txtImages );

                this.Controls.Add(this.lblImages_Path );
this.Controls.Add(this.txtImages_Path );

                            // 
            // "tb_ImagesEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_ImagesEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblImages;
private Krypton.Toolkit.KryptonTextBox txtImages;

    
        
              private Krypton.Toolkit.KryptonLabel lblImages_Path;
private Krypton.Toolkit.KryptonTextBox txtImages_Path;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

