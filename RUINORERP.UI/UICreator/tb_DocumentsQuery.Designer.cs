
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:22
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 文档表
    /// </summary>
    partial class tb_DocumentsQuery
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
     
     this.lblFiles_Path = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFiles_Path = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtFiles_Path.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####500Files_Path###String
this.lblFiles_Path.AutoSize = true;
this.lblFiles_Path.Location = new System.Drawing.Point(100,25);
this.lblFiles_Path.Name = "lblFiles_Path";
this.lblFiles_Path.Size = new System.Drawing.Size(41, 12);
this.lblFiles_Path.TabIndex = 1;
this.lblFiles_Path.Text = "";
this.txtFiles_Path.Location = new System.Drawing.Point(173,21);
this.txtFiles_Path.Name = "txtFiles_Path";
this.txtFiles_Path.Size = new System.Drawing.Size(100, 21);
this.txtFiles_Path.TabIndex = 1;
this.Controls.Add(this.lblFiles_Path);
this.Controls.Add(this.txtFiles_Path);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblFiles_Path );
this.Controls.Add(this.txtFiles_Path );

                    
            this.Name = "tb_DocumentsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFiles_Path;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFiles_Path;

    
    
   
 





    }
}


