
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
    partial class tb_ImagesQuery
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
     
     this.lblImages = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtImages = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtImages.Multiline = true;

this.lblImages_Path = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtImages_Path = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtImages_Path.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblImages );
this.Controls.Add(this.txtImages );

                this.Controls.Add(this.lblImages_Path );
this.Controls.Add(this.txtImages_Path );

                    
            this.Name = "tb_ImagesQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblImages;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtImages;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblImages_Path;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtImages_Path;

    
    
   
 





    }
}


