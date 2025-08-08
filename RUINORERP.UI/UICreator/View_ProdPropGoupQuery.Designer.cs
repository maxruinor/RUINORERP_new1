
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:34
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 
    /// </summary>
    partial class View_ProdPropGoupQuery
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
     
     
this.lblprop = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtprop = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdDetailID###Int64

           //#####-1prop###String
this.lblprop.AutoSize = true;
this.lblprop.Location = new System.Drawing.Point(100,50);
this.lblprop.Name = "lblprop";
this.lblprop.Size = new System.Drawing.Size(41, 12);
this.lblprop.TabIndex = 2;
this.lblprop.Text = "";
this.txtprop.Location = new System.Drawing.Point(173,46);
this.txtprop.Name = "txtprop";
this.txtprop.Size = new System.Drawing.Size(100, 21);
this.txtprop.TabIndex = 2;
this.Controls.Add(this.lblprop);
this.Controls.Add(this.txtprop);

           //#####ProdBaseID###Int64

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblprop );
this.Controls.Add(this.txtprop );

                
                    
            this.Name = "View_ProdPropGoupQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblprop;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtprop;

    
        
              
    
    
   
 





    }
}


